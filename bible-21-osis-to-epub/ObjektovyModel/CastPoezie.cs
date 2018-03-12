namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Popisuje podnadpis poezie, odpovídá 4. úrovni nadpisu.
  /// </summary>
  internal class CastPoezie : CastTextuSTextem
  {
    #region Metody

    public override string PrevestNaHtml()
    {
      return $"<h5>{TextovaData}</h5>\n";
    }

    #endregion
  }
}