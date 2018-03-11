namespace BibleDoEpubu.ObjektovyModel
{
  internal class CastTextuSTextem : CastTextu
  {
    public override string PrevestNaHtml()
    {
      return TextovaData;
    }
  }
}