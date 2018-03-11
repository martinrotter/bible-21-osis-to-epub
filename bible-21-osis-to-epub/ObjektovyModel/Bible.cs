using System.Collections.Generic;
using System.Security.Policy;

namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Top-level objekt reprezentující celou Bibli.
  /// </summary>
  internal class Bible
  {
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

    public List<Kniha> Knihy
    {
      get;
      set;
    } = new List<Kniha>();

    public Dictionary<string, string> MapovaniZkratekKnih { get; }

    public Bible()
    {
      MapovaniZkratekKnih = NacistMapovaniZkratekKnih();
    }

    private Dictionary<string, string> NacistMapovaniZkratekKnih()
    {
      return new Dictionary<string, string>()
      {
        // Starý zákon.
        {"Gen", "Genesis"},
        {"Gen", "Exodus"},
        {"Gen", "Leviticus"},
        {"Gen", "Numeri"},
        {"Gen", "Deuteronomium"},
        {"Gen", "Jozue"},
        {"Gen", "Soudců"},
        {"Gen", "Rút"},
        {"Gen", "1. Samuel"},
        {"Gen", "2. Samuel"},
        {"Gen", "1. Královská"},
        {"Gen", "2. Královská"},
        {"Gen", "1. Letopisů"},
        {"Gen", "2. Letopisů"},
        {"Gen", "Ezdráš"},
        {"Gen", "Nehemiáš"},
        {"Gen", "Ester"},
        {"Gen", "Job"},
        {"Gen", "Žalmy"},
        {"Gen", "Přísloví"},
        {"Gen", "Kazatel"},
        {"Gen", "Píseň"},
        {"Gen", "Izaiáš"},
        {"Gen", "Jeremiáš"},
        {"Gen", "Pláč"},
        {"Gen", "Ezechiel"},
        {"Gen", "Daniel"},
        {"Gen", "Ozeáš"},
        {"Gen", "Joel"},
        {"Gen", "Amos"},
        {"Gen", "Abdiáš"},
        {"Gen", "Jonáš"},
        {"Gen", "Nahum"},
        {"Gen", "Abakuk"},
        {"Gen", "Sofoniáš"},
        {"Gen", "Ageus"},
        {"Gen", "Zachariáš"},
        {"Gen", "Malachiáš"},

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
        {"Gen", "Matouš"},
        {"Gen", "Marek"},
        {"Gen", "Lukáš"},
        {"Gen", "Jan"},
        {"Gen", "Skutky"},
        {"Gen", "Římanům"},
        {"Gen", "1. Korintským"},
        {"Gen", "2. Korintským"},
        {"Gen", "Galatským"},
        {"Gen", "Efeským"},
        {"Gen", "Filipským"},
        {"Gen", "Koloským"},
        {"Gen", "1. Tesalonickým"},
        {"Gen", "2. Tesalonickým"},
        {"Gen", "1. Timoteus"},
        {"Gen", "2. Timoteus"},
        {"Gen", "Titus"},
        {"Gen", "Filemon"},
        {"Gen", "Židům"},
        {"Gen", "Jakub"},
        {"Gen", "1. Petr"},
        {"Gen", "2. Petr"},
        {"Gen", "1. Jan"},
        {"Gen", "2. Jan"},
        {"Gen", "3. Jan"},
        {"Gen", "Juda"},
        {"Gen", "Zjevení"}
      };
    }
  }
}