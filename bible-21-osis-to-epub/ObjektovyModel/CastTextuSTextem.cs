namespace BibleDoEpubu.ObjektovyModel
{
  internal class CastTextuSTextem : CastTextu
  {
    #region Metody

    public override string PrevestNaHtml()
    {
      return TextovaData;
    }

    #endregion
  }
}