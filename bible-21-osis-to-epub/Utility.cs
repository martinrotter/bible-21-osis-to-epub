using System.IO;

namespace BibleDoEpubu
{
  internal static class Utility
  {
    public static void VymazatAdresar(string adresar)
    {
      DirectoryInfo dir = new DirectoryInfo(adresar);

      foreach (FileInfo fi in dir.GetFiles())
      {
        fi.Delete();
      }

      foreach (DirectoryInfo di in dir.GetDirectories())
      {
        VymazatAdresar(di.FullName);
        di.Delete();
      }
    }
  }
}