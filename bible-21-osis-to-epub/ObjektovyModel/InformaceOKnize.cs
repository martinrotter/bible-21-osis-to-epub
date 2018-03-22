namespace BibleDoEpubu.ObjektovyModel
{
  internal class InformaceOKnize
  {
    #region Vlastnosti

    public string CeskaZkratka
    {
      get;
    }

    public string Nadpis
    {
      get;
    }

    #endregion

    #region Konstruktory

    public InformaceOKnize(string zkratka, string nadpis)
    {
      CeskaZkratka = zkratka;
      Nadpis = nadpis;
    }

    #endregion

    #region Metody

    public static implicit operator InformaceOKnize(string zkratkaNadpis)
    {
      int indexStrednik = zkratkaNadpis.IndexOf(';');

      return new InformaceOKnize(zkratkaNadpis.Substring(0, indexStrednik), zkratkaNadpis.Substring(indexStrednik + 1));
    }

    #endregion
  }
}