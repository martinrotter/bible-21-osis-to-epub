namespace BibleDoEpubu.ObjektovyModel
{
  internal class FormatovaniTextu : CastTextu
  {
    #region Vlastnosti

    public bool Kurziva
    {
      get;
      set;
    }

    public bool Tucne
    {
      get;
      set;
    }

    #endregion

    #region Metody

    public override string PrevestNaHtml()
    {
      return base.PrevestNaHtml();
    }

    #endregion
  }
}