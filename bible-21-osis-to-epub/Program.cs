using System;
using System.Linq;
using BibleDoEpubu.ObjektovyModel;

namespace BibleDoEpubu
{
  public class Program
  {
    #region Metody

    public static void Main(string[] args)
    {
      Parser parser = new Parser();
      Bible bible = parser.NacistBibli(args.First());
      /*
      EpubGenerator epubGenerator = new EpubGenerator();

      epubGenerator.VygenerovatEpub(bible, true);
      epubGenerator.VygenerovatEpub(bible, false);

      HtmlGenerator htmlGenerator = new HtmlGenerator();

      htmlGenerator.VygenerovatHtml(bible, true);
      htmlGenerator.VygenerovatHtml(bible, false);
      */
      SqlGenerator sqlGenerator = new SqlGenerator();

      sqlGenerator.VygenerovatSql(bible);

      Console.WriteLine("Hotovo...");
    }

    #endregion
  }
}