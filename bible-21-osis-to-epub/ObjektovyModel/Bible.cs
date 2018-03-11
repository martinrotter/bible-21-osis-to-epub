using System.Collections.Generic;

namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Top-level objekt reprezentující celou Bibli.
  /// </summary>
  internal class Bible
  {
    public Metadata Metadata
    {
      get;
      set;
    }

    public Revize Revize
    {
      get;
      set;
    }

    public List<Kniha> Knihy
    {
      get;
      set;
    } = new List<Kniha>();
  }
}