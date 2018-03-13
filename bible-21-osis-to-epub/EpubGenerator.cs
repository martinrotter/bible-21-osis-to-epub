using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BibleDoEpubu.ObjektovyModel;
using ICSharpCode.SharpZipLib.Zip;

namespace BibleDoEpubu
{
  internal class EpubGenerator
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

        stavec.Append($"<sup>{(dlouheCislaVerse ? ZiskatDlouheCisloVerse((cast as Vers).Id) : ZiskatKratkeCisloVerse((cast as Vers).Id))}</sup>");

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

        return $"<sup class=\"poznamka\"><a href=\"kniha-XX-poznamky.html#{poznamka.Id}\" epub:type=\"noteref\">[{PouzitePoznamky.Count}]</a></sup>";
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

    public string VygenerovatEpub(Bible bible, bool dlouhaCislaVerse)
    {
      PouzitePoznamky.Clear();

      string pracovniAdresar = Environment.CurrentDirectory;
      string epubAdresar = Path.Combine(pracovniAdresar, bible.Metadata.Nazev);
      string epubSoubor = Path.Combine(pracovniAdresar, $"{bible.Metadata.Nazev}.epub");

      if (Directory.Exists(epubAdresar))
      {
        Utility.VymazatAdresar(epubAdresar);
      }

      Directory.CreateDirectory(epubAdresar);

      if (File.Exists(epubSoubor))
      {
        File.Delete(epubSoubor);
      }

      // Vygenerujeme úvodní texty, titulní obrázek,
      // úvodní nakladatelské informace, seznam knih.
      string obsahAdresar = Path.Combine(epubAdresar, "OEBPS");
      string metaAdresar = Path.Combine(epubAdresar, "META-INF");
      string htmlAdresar = Path.Combine(obsahAdresar, "html");
      string cssAdresar = Path.Combine(obsahAdresar, "css");
      string obrazkyAdresar = Path.Combine(obsahAdresar, "img");

      Directory.CreateDirectory(htmlAdresar);
      Directory.CreateDirectory(cssAdresar);
      Directory.CreateDirectory(metaAdresar);
      Directory.CreateDirectory(obrazkyAdresar);

      int pocitadloKnih = 1;
      List<string> manifesty = new List<string>();
      List<string> spine = new List<string>();

      foreach (Kniha kniha in bible.Knihy)
      {
        string nazevSouboruKnihy = $"kniha-{pocitadloKnih}-{kniha.Id}.html";
        string souborKnihy = Path.Combine(htmlAdresar, nazevSouboruKnihy);
        string htmlMustr = Properties.Resources.kniha.Clone() as string;
        string htmlObsah = VygenerovatKnihu(kniha, bible, dlouhaCislaVerse);

        htmlObsah = $"<h1>{bible.MapovaniZkratekKnih[kniha.Id]}</h1>" + htmlObsah;
        htmlObsah = string.Format(htmlMustr, bible.MapovaniZkratekKnih[kniha.Id], htmlObsah);

        manifesty.Add($"<item href=\"html/{nazevSouboruKnihy}\" id=\"id-{nazevSouboruKnihy}\" media-type=\"application/xhtml+xml\"/>");
        spine.Add($"<itemref idref=\"id-{nazevSouboruKnihy}\"/>");

        File.WriteAllText(souborKnihy, htmlObsah, Encoding.UTF8);

        pocitadloKnih++;
      }

      // Připravíme sumář poznámek pod čarou.
      string sumarPoznamek = Properties.Resources.kniha_XX_poznamky;

      sumarPoznamek = string.Format(
        sumarPoznamek,
        string.Join(
          "\n",
          PouzitePoznamky.Select((pozn, idx) => $"<p id=\"{pozn.Id}\" epub:type=\"endnote\">[{idx + 1}] {pozn.Text}</p>")));

      File.WriteAllText(Path.Combine(htmlAdresar, "kniha-XX-poznamky.html"), sumarPoznamek, Encoding.UTF8);

      // Kopírujeme a generujeme podpůrné soubory.
      File.WriteAllText(Path.Combine(cssAdresar, "kniha.css"), Properties.Resources.kniha_css);
      File.WriteAllBytes(Path.Combine(epubAdresar, "mimetype"), Properties.Resources.mimetype);
      File.WriteAllText(Path.Combine(metaAdresar, "container.xml"), Properties.Resources.container);
      Properties.Resources.cover.Save(Path.Combine(obrazkyAdresar, "cover.png"), ImageFormat.Png);
      Properties.Resources.logo.Save(Path.Combine(obrazkyAdresar, "logo.png"), ImageFormat.Png);

      // Nyní dynamické soubory.
      string obsahOpf = Encoding.UTF8.GetString(Properties.Resources.content);

      obsahOpf = string.Format(
        obsahOpf,
        bible.Metadata.Nazev,
        bible.Revize.Datum.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        string.Join("\n", manifesty),
        string.Join("\n", spine),
        $"html/kniha-{1}-{bible.Knihy.First().Id}.html",
        bible.MapovaniZkratekKnih[bible.Knihy.First().Id]);

      File.WriteAllText(Path.Combine(obsahAdresar, "content.opf"), obsahOpf);

      // Generování úvodního souboru.
      string uvodniSoubor = Properties.Resources.kniha_0_uvod;

      uvodniSoubor = string.Format(uvodniSoubor, bible.Metadata.Nazev, bible.Revize.Datum.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
      File.WriteAllText(Path.Combine(htmlAdresar, "kniha-0-uvod.html"), uvodniSoubor, Encoding.UTF8);
      File.WriteAllText(Path.Combine(htmlAdresar, "kniha-0-cover.html"), Properties.Resources.kniha_0_cover, Encoding.UTF8);

      // Konverze zip -> epub.
      FastZip zip = new FastZip();

      FastZip z = new FastZip
      {
        CreateEmptyDirectories = true
      };

      z.CreateZip(
        $"{bible.Metadata.Nazev}-{bible.Revize.Datum.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
        $"{(dlouhaCislaVerse ? "-l" : string.Empty)}.epub",
        epubAdresar, true, string.Empty);

      return epubSoubor;
    }

    #endregion
  }
}