namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Verš.
  /// </summary>
  /// <remarks>
  /// Odpovídá tagu verse.
  /// </remarks>
  internal class Vers : CastTextu
  {
    #region Vlastnosti

    /// <summary>
    /// ID (číslo) verše, například Gen 1:3
    /// je citace verše 3, kapitola 1, z knihy Genesis.
    /// </summary>
    public string Id
    {
      get;
      set;
    }

    #endregion

    public override string PrevestNaHtml()
    {
      return Potomci.Count == 0 ? " " : ($"<sup>Id</sup> " + base.PrevestNaHtml());
    }
  }
}