using System.Collections.Generic;
using System.Text;

namespace BibleDoEpubu.ObjektovyModel
{
  internal class CastTextu
  {
    #region Vlastnosti

    public List<CastTextu> Potomci
    {
      get;
      set;
    } = new List<CastTextu>();

    public CastTextu Rodic
    {
      get;
      set;
    }

    public string TextovaData
    {
      get;
      set;
    }

    #endregion

    #region Metody

    public void PridatPotomka(CastTextu potomek)
    {
      Potomci.Remove(potomek);
      Potomci.Add(potomek);

      potomek.Rodic = this;
    }

    public void OdstranitPotomka(CastTextu potomek)
    {
      Potomci.Remove(potomek);
      potomek.Rodic = null;
    }

    /// <summary>
    ///   Vrátí HTML/XML reprezentaci tohoto kousku textu.
    /// </summary>
    /// <remarks>
    ///   Například pro verš vrátí jeho text.
    /// </remarks>
    /// <returns>
    ///   Vrátí HTML/XML reprezentaci tohoto kousku textu.
    /// </returns>
    public virtual string PrevestNaHtml()
    {
      StringBuilder stavec = new StringBuilder();

      foreach (CastTextu potomek in Potomci)
      {
        stavec.Append(potomek.PrevestNaHtml());
      }

      return stavec.ToString();
    }

    #endregion
  }
}