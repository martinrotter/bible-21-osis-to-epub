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

      EpubGenerator generator = new EpubGenerator();    
      string epubCesta = generator.VygenerovatEpub(bible);
    }

    #endregion
  }
}