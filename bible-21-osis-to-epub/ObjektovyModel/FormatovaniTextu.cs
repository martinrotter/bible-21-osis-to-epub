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
      if (Kurziva)
      {
        return $"<span class=\"kurziva\">{base.PrevestNaHtml()}</span>";
      }
      else
      {
        return base.PrevestNaHtml();
      }
    }

    #endregion
  }
}