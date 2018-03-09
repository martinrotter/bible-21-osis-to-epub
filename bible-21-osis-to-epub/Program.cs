using System.Linq;
using BibleDoEpubu.ObjektovyModel;

namespace BibleDoEpubu
{
  public class Program
  {
    #region Metody

    public static void Main(string[] args)
    {
      Bible bible = Parser.NacistBibli(args.First());
    }

    #endregion
  }
}