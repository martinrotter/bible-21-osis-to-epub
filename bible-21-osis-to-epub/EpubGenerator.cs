using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using BibleDoEpubu.ObjektovyModel;
using ICSharpCode.SharpZipLib.Zip;

namespace BibleDoEpubu
{
  internal class EpubGenerator
  {
    #region Metody

    public string VygenerovatEpub(Bible bible)
    {
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
        string htmlObsah = kniha.PrevestNaHtml();

        htmlObsah = $"<h1>{bible.MapovaniZkratekKnih[kniha.Id]}</h1>" + htmlObsah;
        htmlObsah = string.Format(htmlMustr, bible.MapovaniZkratekKnih[kniha.Id], htmlObsah);

        manifesty.Add($"<item href=\"html/{nazevSouboruKnihy}\" id=\"id-{nazevSouboruKnihy}\" media-type=\"application/xhtml+xml\"/>");
        spine.Add($"<itemref idref=\"id-{nazevSouboruKnihy}\"/>");

        File.WriteAllText(souborKnihy, htmlObsah, Encoding.UTF8);

        pocitadloKnih++;
      }

      // Kopírujeme a generujeme podpůrne soubory.
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
        string.Join("\n", spine));

      File.WriteAllText(Path.Combine(obsahAdresar, "content.opf"), obsahOpf);

      // Konverze zip -> epub.
      FastZip zip = new FastZip();

      FastZip z = new FastZip
      {
        CreateEmptyDirectories = true
      };

      z.CreateZip(
        $"{bible.Metadata.Nazev}-{bible.Revize.Datum.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.epub",
        epubAdresar, true, string.Empty);

      return epubSoubor;
    }

    #endregion
  }
}