namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Část knihy.
  /// </summary>
  /// <remarks>
  /// Odpovídá div type="section".
  /// </remarks>
  internal class CastKnihy : CastTextu
  {
    #region Vlastnosti

    /// <summary>
    /// Odpovídá 4. úrovni nadpisu.
    /// </summary>
    public string Nadpis
    {
      get;
      set;
    }

    #endregion
  }
}