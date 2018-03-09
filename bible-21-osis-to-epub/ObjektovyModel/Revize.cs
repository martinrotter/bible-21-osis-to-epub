using System;

namespace BibleDoEpubu.ObjektovyModel
{
  internal class Revize
  {
    #region Vlastnosti

    public DateTime Datum
    {
      get;
      set;
    }

    /// <summary>
    /// Popis revize.
    /// </summary>
    /// <example></example>
    public string Popis
    {
      get;
      set;
    }

    #endregion
  }
}