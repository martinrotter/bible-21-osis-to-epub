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
      xml.Load(xmlOsisSoubor);

      Bible bible = new Bible();

      return bible;
    }

    #endregion
  }
}