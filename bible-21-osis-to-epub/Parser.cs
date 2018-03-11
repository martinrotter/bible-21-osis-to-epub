using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using BibleDoEpubu.ObjektovyModel;

namespace BibleDoEpubu
{
  internal class Parser
  {
    #region Metody

    public static Bible NacistBibli(string xmlOsisSoubor)
    {
      XmlDocument xml = new XmlDocument();
      XmlNamespaceManager ns = VygenerovatNamespaceManager(xml);

      xml.Load(xmlOsisSoubor);

      XmlNode osisText = xml.SelectSingleNode("/os:osis/os:osisText", ns);

      Bible bible = new Bible()
      {
        Revize = new Revize()
        {
          Datum = DateTime.ParseExact(
            osisText?.SelectSingleNode("os:header/os:revisionDesc/os:date", ns)?.InnerText,
            "yyyy.M.d",
            CultureInfo.InvariantCulture),
          Popis = osisText?.SelectSingleNode("os:header/os:revisionDesc/os:p", ns)?.InnerText
        },
        Metadata = new Metadata()
        {
          Copyright = osisText?.SelectSingleNode("os:header/os:work/os:rights", ns)?.InnerText,
          Isbn = osisText?.SelectSingleNode("os:header/os:work/os:identifier", ns)?.InnerText,
          Nazev = osisText?.SelectSingleNode("os:header/os:work/os:title", ns)?.InnerText,
          Vydavatel = osisText?.SelectSingleNode("os:header/os:work/os:publisher", ns)?.InnerText
        }
      };

      // Provedeme načtení knih.
      foreach (XmlNode kniha in osisText?.SelectNodes("//os:div[@type='book']", ns))
      {
        Kniha k = NacistKnihu(kniha);
        bible.Knihy.Add(k);
        break;
      }



      return bible;
    }

    private static XmlNamespaceManager VygenerovatNamespaceManager(XmlDocument xml)
    {
      XmlNamespaceManager ns = new XmlNamespaceManager(xml.NameTable);

      ns.AddNamespace("os", Konstanty.XmlJmenneProstory.Osis);
      ns.AddNamespace("xsi", Konstanty.XmlJmenneProstory.Xsi);
      return ns;
    }

    private static Kniha NacistKnihu(XmlNode xml)
    {
      Kniha kniha = new Kniha();
      XmlNamespaceManager ns = VygenerovatNamespaceManager(xml.OwnerDocument);

      kniha.Id = xml.SelectSingleNode("@osisID").InnerText;

      // Budeme iterativně zpracovávat seznam XML potomků a přidávát
      // ekvivalenty.
      List<CastTextu> rodice = new List<CastTextu>
      {
        kniha
      };

      // Tento seznam obashuje seznamy XML prvků, které patří do daného rodiče.
      List<List<XmlNode>> xmlProRodice = new List<List<XmlNode>>()
      {
        xml.ChildNodes.OfType<XmlNode>().ToList()
      };

      while (rodice.Count > 0)
      {
        CastTextu rodic = rodice[0];
        rodice.RemoveAt(0);

        List<XmlNode> xmlProTohotoRodice = xmlProRodice[0];
        xmlProRodice.RemoveAt(0);

        foreach (XmlNode xmlPotomek in xmlProTohotoRodice)
        {
          if (xmlPotomek is XmlElement)
          {
            XmlElement xmlPotomekElem = xmlPotomek as XmlElement;

            if (
              xmlPotomek.Name == "div" && xmlPotomekElem.HasAttribute("type") &&
              xmlPotomek.Attributes["type"].InnerText == "majorSection")
            {
              // Máme major section.
              HlavniCastKnihy hlavniCast = new HlavniCastKnihy
              {
                Nadpis = xmlPotomek.SelectSingleNode("os:title", ns).InnerText
              };


              rodic.PridatPotomka(hlavniCast);
              rodice.Add(hlavniCast);
              xmlProRodice.Add(xmlPotomek.ChildNodes.OfType<XmlNode>().ToList());
            }
            else if (
              xmlPotomek.Name == "div" && xmlPotomekElem.HasAttribute("type") &&
              xmlPotomek.Attributes["type"].InnerText == "section")
            {
              CastKnihy castKnihy = new CastKnihy()
              {
                Nadpis = xmlPotomek.SelectSingleNode("os:title", ns).InnerText
              };

              rodic.PridatPotomka(castKnihy);
              rodice.Add(castKnihy);
              xmlProRodice.Add(xmlPotomek.ChildNodes.OfType<XmlNode>().ToList());
            }
            else if (xmlPotomek.Name == "lg")
            {
              // Máme báseň.
              Poezie poezie = new Poezie();

              rodic.PridatPotomka(poezie);
              rodice.Add(poezie);
              xmlProRodice.Add(xmlPotomek.ChildNodes.OfType<XmlNode>().ToList());
            }
            else if (xmlPotomek.Name == "p")
            {
              // Odstavec složený z veršů, řádků básně či dalšího obsahu.
              Odstavec odstavec = new Odstavec();

              rodic.PridatPotomka(odstavec);
              rodice.Add(odstavec);
              xmlProRodice.Add(xmlPotomek.ChildNodes.OfType<XmlNode>().ToList());
            }
            else if (xmlPotomek.Name == "chapter")
            {
              if (xmlPotomekElem.HasAttribute("osisID"))
              {
                UvodKapitoly uvodKapitoly = new UvodKapitoly()
                {
                  Id = xmlPotomek.SelectSingleNode("@osisID", ns).InnerText
                };

                rodic.PridatPotomka(uvodKapitoly);
                rodice.Add(uvodKapitoly);
                xmlProRodice.Add(xmlPotomek.ChildNodes.OfType<XmlNode>().ToList());
              }
              else
              {
                // Konec kapitoly, zatím se neřeší.
              }
            }
            else if (xmlPotomekElem.Name == "verse")
            {
              if (xmlPotomekElem.HasAttribute("osisID"))
              {
                Vers vers = new Vers()
                {
                  Id = xmlPotomek.SelectSingleNode("@osisID", ns).InnerText
                };

                rodic.PridatPotomka(vers);
                rodice.Add(vers);
                xmlProRodice.Add(xmlPotomek.ChildNodes.OfType<XmlNode>().ToList());
              }
              else
              {
                // Konec verše, zatím se neřeší.
              }
            }
            else
            {

            }
          }
          else if (xmlPotomek is XmlText)
          {
            // Máme čistě textový záznam.
            CastTextuSTextem textovaCast = new CastTextuSTextem()
            {
              TextovaData = xmlPotomek.Value.TrimEnd('\n', '\r', ' ')
            };

            rodic.PridatPotomka(textovaCast);
            rodice.Add(textovaCast);
            xmlProRodice.Add(xmlPotomek.ChildNodes.OfType<XmlNode>().ToList());
          }
          else
          {

          }
        }
      }


      return kniha;
    }

    #endregion
  }
}