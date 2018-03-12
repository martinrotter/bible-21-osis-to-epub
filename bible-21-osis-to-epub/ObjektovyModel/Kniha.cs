namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Reprezentuje jednu knihu Bible (například Genesis).
  /// </summary>
  internal class Kniha : CastTextu
  {
    #region Vlastnosti

    /// <summary>
    /// ID knihy, např. "Gen"., první úroveň nadpisu.
    /// </summary>
    public string Id
    {
      get;
      set;
    }

    #endregion
  }
}