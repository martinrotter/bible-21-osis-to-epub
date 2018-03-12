using System.Collections.Generic;

namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Top-level objekt reprezentující celou Bibli.
  /// </summary>
  internal class Bible
  {
    #region Vlastnosti

    public List<Kniha> Knihy
    {
      get;
      set;
    } = new List<Kniha>();

    public Dictionary<string, string> MapovaniZkratekKnih
    {
      get;
    }

    public Metadata Metadata
    {
      get;
      set;
    }

    public Revize Revize
    {
      get;
      set;
    }

    #endregion

    #region Konstruktory

    public Bible()
    {
      MapovaniZkratekKnih = NacistMapovaniZkratekKnih();
    }

    #endregion

    #region Metody

    private Dictionary<string, string> NacistMapovaniZkratekKnih()
    {
      return new Dictionary<string, string>
      {
        // Starý zákon.
        {"Gen", "Genesis"},
        {"Exod", "Exodus"},
        {"Lev", "Leviticus"},
        {"Num", "Numeri"},
        {"Deut", "Deuteronomium"},
        {"Josh", "Jozue"},
        {"Judg", "Soudců"},
        {"Ruth", "Rút"},
        {"1Sam", "1. Samuel"},
        {"2Sam", "2. Samuel"},
        {"1Kgs", "1. Královská"},
        {"2Kgs", "2. Královská"},
        {"1Chr", "1. Letopisů"},
        {"2Chr", "2. Letopisů"},
        {"Ezra", "Ezdráš"},
        {"Neh", "Nehemiáš"},
        {"Esth", "Ester"},
        {"Job", "Job"},
        {"Ps", "Žalmy"},
        {"Prov", "Přísloví"},
        {"Eccl", "Kazatel"},
        {"Song", "Píseň"},
        {"Isa", "Izaiáš"},
        {"Jer", "Jeremiáš"},
        {"Lam", "Pláč"},
        {"Ezek", "Ezechiel"},
        {"Dan", "Daniel"},
        {"Hos", "Ozeáš"},
        {"Joel", "Joel"},
        {"Amos", "Amos"},
        {"Obad", "Abdiáš"},
        {"Jonah", "Jonáš"},
        {"Nah", "Nahum"},
        {"Mic", "Micheáš"},
        {"Hab", "Abakuk"},
        {"Zeph", "Sofoniáš"},
        {"Hag", "Ageus"},
        {"Zech", "Zachariáš"},
        {"Mal", "Malachiáš"},

        // Deuterokanické knihy (ke Starému zákonu).
        {"Tob", "Tóbijáš"},
        {"Bar", "Báruk"},
        {"AddEsth", "Ester (přídavky)"},
        {"AddDan", "Daniel (přídavky)"},
        {"Sir", "Sírach"},
        {"Jdt", "Júdit"},
        {"Wis", "Moudrost Šalamounova"},
        {"1Macc", "1. Makabejských"},
        {"2Macc", "2. Makabejských"},

        // Nový zákon.
        {"Matt", "Matouš"},
        {"Mark", "Marek"},
        {"Luke", "Lukáš"},
        {"John", "Jan"},
        {"Acts", "Skutky"},
        {"Rom", "Římanům"},
        {"1Cor", "1. Korintským"},
        {"2Cor", "2. Korintským"},
        {"Gal", "Galatským"},
        {"Eph", "Efeským"},
        {"Phil", "Filipským"},
        {"Col", "Koloským"},
        {"1Thess", "1. Tesalonickým"},
        {"2Thess", "2. Tesalonickým"},
        {"1Tim", "1. Timoteus"},
        {"2Tim", "2. Timoteus"},
        {"Titus", "Titus"},
        {"Phlm", "Filemon"},
        {"Heb", "Židům"},
        {"Jas", "Jakub"},
        {"1Pet", "1. Petr"},
        {"2Pet", "2. Petr"},
        {"1John", "1. Jan"},
        {"2John", "2. Jan"},
        {"3John", "3. Jan"},
        {"Jude", "Juda"},
        {"Rev", "Zjevení"}
      };
    }

    #endregion
  }
}