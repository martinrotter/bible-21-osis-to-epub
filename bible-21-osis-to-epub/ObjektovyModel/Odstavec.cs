namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Odstavec.
  /// </summary>
  /// <remarks>
  /// Odpovídá tagu p. Tag p v OSIS většinou obsahuje verše.
  /// </remarks>
  internal class Odstavec : CastTextu
  {
    #region Metody

    public override string PrevestNaHtml()
    {
      return "<p>" + base.PrevestNaHtml() + "</p>";
    }

    #endregion
  }
}