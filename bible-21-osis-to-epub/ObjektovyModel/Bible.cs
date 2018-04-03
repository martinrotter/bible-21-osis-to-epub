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

    public Dictionary<string, InformaceOKnize> MapovaniZkratekKnih
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

    private Dictionary<string, InformaceOKnize> NacistMapovaniZkratekKnih()
    {
      return new Dictionary<string, InformaceOKnize>
      {
        // Starý zákon.
        {"Gen", "Gn;Genesis"},
        {"Exod", "Ex;Exodus"},
        {"Lev", "Lv;Leviticus"},
        {"Num", "Nu;Numeri"},
        {"Deut", "Dt;Deuteronomium"},
        {"Josh", "Joz;Jozue"},
        {"Judg", "Sd;Soudců"},
        {"Ruth", "Rt;Rút"},
        {"1Sam", "1S;1. Samuel"},
        {"2Sam", "2S;2. Samuel"},
        {"1Kgs", "1Kr;1. Královská"},
        {"2Kgs", "2Kr;2. Královská"},
        {"1Chr", "1Pa;1. Letopisů"},
        {"2Chr", "2Pa;2. Letopisů"},
        {"Ezra", "Ezd;Ezdráš"},
        {"Neh", "Neh;Nehemiáš"},
        {"Esth", "Est;Ester"},
        {"Job", "Jb;Job"},
        {"Ps", "Ž;Žalmy"},
        {"Prov", "Př;Přísloví"},
        {"Eccl", "Kaz;Kazatel"},
        {"Song", "Pís;Píseň písní"},
        {"Isa", "Iz;Izaiáš"},
        {"Jer", "Jr;Jeremiáš"},
        {"Lam", "Pl;Pláč"},
        {"Ezek", "Ez;Ezechiel"},
        {"Dan", "Dn;Daniel"},
        {"Hos", "Oz;Ozeáš"},
        {"Joel", "Jl;Joel"},
        {"Amos", "Am;Amos"},
        {"Obad", "Abd;Abdiáš"},
        {"Jonah", "Jon;Jonáš"},
        {"Nah", "Na;Nahum"},
        {"Mic", "Mi;Micheáš"},
        {"Hab", "Ab;Abakuk"},
        {"Zeph", "Sf;Sofoniáš"},
        {"Hag", "Ag;Ageus"},
        {"Zech", "Za;Zachariáš"},
        {"Mal", "Mal;Malachiáš"},

        // Deuterokanické knihy (ke Starému zákonu).
        {"Tob", "Tob;Tobiáš"},
        {"Bar", "Bar;Baruch"},
        {"AddEsth", "Estp;Ester"},
        {"AddDan", "Danp;Přídavky k Danielovi"},
        {"Sir", "Sír;Sirachovec"},
        {"Jdt", "Jud;Judita"},
        {"Wis", "Mdr;Moudrost Šalomounova"},
        {"1Macc", "1Mak;1. Makabejská"},
        {"2Macc", "2Mak;2. Makabejská"},

        // Nový zákon.
        {"Matt", "Mt;Matouš"},
        {"Mark", "Mk;Marek"},
        {"Luke", "L;Lukáš"},
        {"John", "J;Jan"},
        {"Acts", "Sk;Skutky"},
        {"Rom", "Ř;Římanům"},
        {"1Cor", "1Kor;1. Korintským"},
        {"2Cor", "2Kor;2. Korintským"},
        {"Gal", "Ga;Galatským"},
        {"Eph", "Ef;Efeským"},
        {"Phil", "Fp;Filipským"},
        {"Col", "Ko;Koloským"},
        {"1Thess", "1Te;1. Tesalonickým"},
        {"2Thess", "2Te;2. Tesalonickým"},
        {"1Tim", "1Tm;1. Timoteus"},
        {"2Tim", "2Tm;2. Timoteus"},
        {"Titus", "Tt;Titus"},
        {"Phlm", "Fm;Filemon"},
        {"Heb", "Žd;Židům"},
        {"Jas", "Jk;Jakub"},
        {"1Pet", "1Pt;1. Petr"},
        {"2Pet", "2Pt;2. Petr"},
        {"1John", "1J;1. Jan"},
        {"2John", "2J;2. Jan"},
        {"3John", "3J;3. Jan"},
        {"Jude", "Ju;Juda"},
        {"Rev", "Zj;Zjevení"}
      };
    }

    #endregion
  }
}