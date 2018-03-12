namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Úvod kapitoly.
  /// </summary>
  /// <remarks>
  /// Odpovídá tagu chapter sID="..." osisID="...".
  /// </remarks>
  internal class UvodKapitoly : CastTextu
  {
    #region Vlastnosti

    /// <summary>
    /// ID (číslo) kapitoly, například Gen 1:3
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
      return $"<h3>{Id}</h3>\n";
    }
  }
}