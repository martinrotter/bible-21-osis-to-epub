namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Popisuje podnadpis poezie, odpovídá 4. úrovni nadpisu.
  /// </summary>
  internal class CastPoezie : CastTextuSTextem
  {
    public override string PrevestNaHtml()
    {
      return $"<h5>{TextovaData}</h5>\n";
    }
  }
}