using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BibleDoEpubu.ObjektovyModel;
using ICSharpCode.SharpZipLib.Zip;

namespace BibleDoEpubu
{
  internal class HtmlGenerator
  {
    #region Vlastnosti

    private List<PouzitaPoznamka> PouzitePoznamky
    {
      get;
      set;
    } = new List<PouzitaPoznamka>();

    #endregion

    #region Metody

    public string VygenerovatKnihu(Kniha kniha, Bible bible, bool dlouheCislaVerse)
    {
      return VygenerovatCastTextu(kniha, kniha, bible, dlouheCislaVerse);
    }

    private string VygenerovatCastTextu(CastTextu cast, Kniha kniha, Bible bible, bool dlouheCislaVerse)
    {
      if (cast is HlavniCastKnihy)
      {
        StringBuilder stavec = new StringBuilder();

        stavec.Append($"<h2>{(cast as HlavniCastKnihy).Nadpis}</h2>\n");

        foreach (CastTextu potomek in cast.Potomci)
        {
          stavec.Append(VygenerovatCastTextu(potomek, kniha, bible, dlouheCislaVerse));
        }

        return stavec.ToString();
      }
      else if (cast is CastKnihy)
      {
        StringBuilder stavec = new StringBuilder();

        stavec.Append($"<h4>{(cast as CastKnihy).Nadpis}</h4>\n");

        foreach (CastTextu potomek in cast.Potomci)
        {
          stavec.Append(VygenerovatCastTextu(potomek, kniha, bible, dlouheCislaVerse));
        }

        return stavec.ToString();
      }
      else if (cast is UvodKapitoly)
      {
        return $"<h3>Kapitola {ZiskatKratkeCisloVerse((cast as UvodKapitoly).Id)}</h3>\n";
      }
      else if (cast is Vers)
      {
        StringBuilder stavec = new StringBuilder();

        if (dlouheCislaVerse)
        {
          stavec.Append($"<sup>{ZiskatDlouheCisloVerse((cast as Vers).Id)}</sup>");
        }
        else
        {
          // S tooltipem.
          string kratkeCislo = ZiskatKratkeCisloVerse((cast as Vers).Id);
          string dlouheCislo = ZiskatDlouheCisloVerse((cast as Vers).Id);
          stavec.Append($"<sup><a href=\"#\" data-html=\"true\" data-toggle=\"tooltip\" title=\"{HttpUtility.HtmlEncode(dlouheCislo)}\">{kratkeCislo}</a></sup>");
        }

        foreach (CastTextu potomek in cast.Potomci)
        {
          stavec.Append(VygenerovatCastTextu(potomek, kniha, bible, dlouheCislaVerse));
        }

        return stavec.ToString();
      }
      else if (cast is Poznamka)
      {
        StringBuilder stavec = new StringBuilder();

        foreach (CastTextu potomek in cast.Potomci)
        {
          stavec.Append(VygenerovatCastTextu(potomek, kniha, bible, dlouheCislaVerse));
        }

        PouzitaPoznamka poznamka = new PouzitaPoznamka
        {
          Text = stavec.ToString(),
          Id = $"pozn-{PouzitePoznamky.Count + 1}"
        };

        PouzitePoznamky.Add(poznamka);
        return $"<sup class=\"poznamka\"><a href=\"#\" data-html=\"true\" data-toggle=\"tooltip\" title=\"{HttpUtility.HtmlEncode(poznamka.Text)}\">[{PouzitePoznamky.Count}]</a></sup>";
      }
      else if (cast is Poezie)
      {
        StringBuilder stavec = new StringBuilder();

        stavec.Append("<p class=\"poezie\">");

        foreach (CastTextu potomek in cast.Potomci)
        {
          stavec.Append(VygenerovatCastTextu(potomek, kniha, bible, dlouheCislaVerse));
        }

        stavec.Append("</p>");

        return stavec.ToString();
      }
      else if (cast is RadekPoezie)
      {
        StringBuilder stavec = new StringBuilder();

        foreach (CastTextu potomek in cast.Potomci)
        {
          stavec.Append(VygenerovatCastTextu(potomek, kniha, bible, dlouheCislaVerse));
        }

        stavec.Append("<br/>");

        return stavec.ToString();
      }
      else if (cast is Odstavec)
      {
        StringBuilder stavec = new StringBuilder();

        stavec.Append("<p>");

        foreach (CastTextu potomek in cast.Potomci)
        {
          stavec.Append(VygenerovatCastTextu(potomek, kniha, bible, dlouheCislaVerse));
        }

        stavec.Append("</p>");

        return stavec.ToString();
      }
      else if (cast is FormatovaniTextu)
      {
        StringBuilder stavec = new StringBuilder();

        if ((cast as FormatovaniTextu).Kurziva)
        {
          stavec.Append("<span class=\"kurziva\">");
        }

        foreach (CastTextu potomek in cast.Potomci)
        {
          stavec.Append(VygenerovatCastTextu(potomek, kniha, bible, dlouheCislaVerse));
        }

        stavec.Append("</span>");

        return stavec.ToString();
      }
      else if (cast is CastPoezie)
      {
        return $"<h5>{cast.TextovaData}</h5>\n";
      }
      else if (cast is CastTextuSTextem)
      {
        return cast.TextovaData;
      }
      else
      {
        StringBuilder stavec = new StringBuilder();

        foreach (CastTextu potomek in cast.Potomci)
        {
          stavec.Append(VygenerovatCastTextu(potomek, kniha, bible, dlouheCislaVerse));
        }

        return stavec.ToString();
      }
    }

    private static string ZiskatKratkeCisloVerse(string id)
    {
      return id.Substring(id.LastIndexOf('.') + 1);
    }

    private static string ZiskatDlouheCisloVerse(string id)
    {
      StringBuilder bldr = new StringBuilder(id)
      {
        [id.IndexOf('.')] = ' ',
        [id.LastIndexOf('.')] = ':'
      };

      return bldr.ToString();
    }

    public string VygenerovatHtml(Bible bible, bool dlouhaCislaVerse)
    {
      PouzitePoznamky.Clear();

      string pracovniAdresar = Environment.CurrentDirectory;
      string htmlSoubor = Path.Combine(
        pracovniAdresar,
        $"{bible.Metadata.Nazev}-{bible.Revize.Datum.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
        $"{(dlouhaCislaVerse ? "-l" : string.Empty)}.html");

      if (File.Exists(htmlSoubor))
      {
        File.Delete(htmlSoubor);
      }

      List<string> sekce = new List<string>();
      List<string> obsahy = new List<string>();

      foreach (Kniha kniha in bible.Knihy)
      {
        sekce.Add($"<li><a href=\"#{kniha.Id}\">{bible.MapovaniZkratekKnih[kniha.Id]}</a></li>");
        obsahy.Add($"<h1 id=\"{kniha.Id}\">{bible.MapovaniZkratekKnih[kniha.Id]}</h1>" + VygenerovatKnihu(kniha, bible, dlouhaCislaVerse));
      }

      File.WriteAllText(
        htmlSoubor,
        Properties.Resources.bible_cela
          .Replace("{0}", bible.Metadata.Nazev)
          .Replace("{1}", string.Join("\n", sekce))
          .Replace("{2}", string.Join("\n", obsahy)),
        Encoding.UTF8);


      return htmlSoubor;
    }

    #endregion
  }
}