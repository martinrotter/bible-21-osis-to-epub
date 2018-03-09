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

    public string Nadpis
    {
      get;
      set;
    }

    #endregion
  }
}