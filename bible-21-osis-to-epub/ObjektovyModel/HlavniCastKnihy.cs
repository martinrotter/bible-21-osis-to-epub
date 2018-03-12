namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Reprezentace hlavní části knihy.
  /// </summary>
  /// <remarks>
  /// Dle OSIS odpovídá div type="majorSection", 2. úroveň nadpisu.
  /// </remarks>
  internal class HlavniCastKnihy : CastTextu
  {
    #region Vlastnosti

    /// <summary>
    /// Odpovídá 2. úrovni nadpisu.
    /// </summary>
    public string Nadpis
    {
      get;
      set;
    }

    #endregion
  }
}