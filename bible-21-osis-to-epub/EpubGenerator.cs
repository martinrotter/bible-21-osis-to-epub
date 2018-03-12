using System;
using System.IO;
using BibleDoEpubu.ObjektovyModel;

namespace BibleDoEpubu
{
  internal class EpubGenerator
  {
    #region Konstruktory

    #endregion

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
      string htmlAdresar = Path.Combine(obsahAdresar, "html");
      string cssAdresar = Path.Combine(obsahAdresar, "css");

      Directory.CreateDirectory(htmlAdresar);
      Directory.CreateDirectory(cssAdresar);

      int pocitadloKnih = 1;
      int pocitadloPoznamek = 1;

      foreach (Kniha kniha in bible.Knihy)
      {
        string souborKnihy = Path.Combine(htmlAdresar, $"kniha-{pocitadloKnih}-{kniha.Id}.html");

        File.WriteAllText();

        pocitadloKnih++;
        break;
      }

      return epubSoubor;
    }

    #endregion
  }
}