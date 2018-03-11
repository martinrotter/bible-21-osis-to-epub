using System;
using System.IO;
using BibleDoEpubu.ObjektovyModel;

namespace BibleDoEpubu
{
  internal class EpubGenerator
  {
    #region Metody

    public EpubGenerator()
    {

    }

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

      return epubSoubor;
    }

    #endregion
  }
}