namespace BibleDoEpubu.ObjektovyModel
{
  internal class RadekPoezie : CastTextu
  {
    #region Metody

    public override string PrevestNaHtml()
    {
      return base.PrevestNaHtml() + "<br/>";
    }

    #endregion
  }
}