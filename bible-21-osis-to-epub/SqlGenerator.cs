using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BibleDoEpubu.ObjektovyModel;

namespace BibleDoEpubu
{
  internal class SqlGenerator
  {
    #region Vlastnosti

    private string AktualniTextVerse
    {
      get;
      set;
    }

    private List<string> Nadpisy
    {
      get;
      set;
    } = new List<string>();

    private int PocitadloKapitol
    {
      get;
      set;
    }

    private int PocitadloNadpisu
    {
      get;
      set;
    }

    private int GlobalniPocitadloVersu
    {
      get;
      set;
    }

    private int PocitadloVerse
    {
      get;
      set;
    }

    private int PoradiKnihy
    {
      get;
      set;
    }

    private List<PouzitaPoznamka> PouzitePoznamky
    {
      get;
      set;
    } = new List<PouzitaPoznamka>();

    private StringBuilder StavecKnihy
    {
      get;
    } = new StringBuilder();

    private StringBuilder StavecNadpisy
    {
      get;
    } = new StringBuilder();

    private StringBuilder StavecVerse
    {
      get;
    } = new StringBuilder();

    private List<string> Verse
    {
      get;
      set;
    } = new List<string>();

    #endregion

    #region Metody

    public string VygenerovatSql(Bible bible)
    {
      PocitadloNadpisu = 1;

      for (int poradi = 0; poradi < bible.Knihy.Count; poradi++)
      {
        Kniha kniha = bible.Knihy[poradi];

        PoradiKnihy = poradi + 1;
        PocitadloKapitol = 0;
        PocitadloVerse = 0;
        AktualniTextVerse = string.Empty;
        Nadpisy.Clear();
        Verse.Clear();

        StavecKnihy.Append("INSERT INTO bible_knihy (id, kod, nazev, `order`) VALUES " +
                           $"({poradi + 1}, '{kniha.Id}', '{bible.MapovaniZkratekKnih[kniha.Id].Nadpis}', {poradi + 1});\n");

        VygenerovatSqlProKnihu(bible, kniha);

        StavecNadpisy.Append(string.Join(string.Empty, Nadpisy));
        StavecNadpisy.AppendLine();

        StavecVerse.Append("INSERT INTO bible_verse (kniha_id, kapitola, vers, text, stripped, `order`) VALUES \n");
        StavecVerse.Append(string.Join(",\n", Verse));
        StavecVerse.Append(";");
      }

      string pracovniAdresar = Environment.CurrentDirectory;
      string sqlSoubor = Path.Combine(pracovniAdresar, $"{bible.Metadata.Nazev}.sql");
      Encoding kodovani = new UTF8Encoding(false);

      File.WriteAllText(sqlSoubor, Properties.Resources.sql_sablona, kodovani);
      File.AppendAllText(sqlSoubor, StavecKnihy.ToString(), kodovani);
      File.AppendAllText(sqlSoubor, StavecNadpisy.ToString(), kodovani);
      File.AppendAllText(sqlSoubor, StavecVerse.ToString(), kodovani);

      return sqlSoubor;
    }

    public void VygenerovatSqlProKnihu(Bible bible, Kniha kniha)
    {
      VygenerovatCastSql(kniha, bible, kniha);
    }

    public void VygenerovatCastSql(CastTextu cast, Bible bible, Kniha kniha)
    {
      if (cast is HlavniCastKnihy || cast is CastKnihy)
      {
        if (cast is HlavniCastKnihy)
        {
          PridatRozpracovanyVers();
          PocitadloVerse = 1;
        }

        VlozitSqlNadpis(cast is HlavniCastKnihy knihy ? knihy.Nadpis : ((CastKnihy) cast).Nadpis);

        foreach (CastTextu potomek in cast.Potomci)
        {
          VygenerovatCastSql(potomek, bible, kniha);
        }
      }
      else if (cast is UvodKapitoly)
      {
        PridatRozpracovanyVers();
        PocitadloVerse = 1;

        PocitadloKapitol++;
      }
      else if (cast is Vers)
      {
        PridatRozpracovanyVers();
      }
      else if (cast is Poezie)
      {
        foreach (CastTextu potomek in cast.Potomci)
        {
          VygenerovatCastSql(potomek, bible, kniha);
        }
      }
      else if (cast is RadekPoezie)
      {
        foreach (CastTextu potomek in cast.Potomci)
        {
          VygenerovatCastSql(potomek, bible, kniha);
        }

        AktualniTextVerse += "<br/>";
      }
      else if (cast is Odstavec)
      {
        foreach (CastTextu potomek in cast.Potomci)
        {
          VygenerovatCastSql(potomek, bible, kniha);
        }
      }
      else if (cast is FormatovaniTextu)
      {
        if ((cast as FormatovaniTextu).Kurziva)
        {
          AktualniTextVerse += "<i>";

          foreach (CastTextu potomek in cast.Potomci)
          {
            VygenerovatCastSql(potomek, bible, kniha);
          }

          AktualniTextVerse += "</i>";
        }
      }
      else if (cast is CastPoezie)
      {
        AktualniTextVerse += $"<h5>{cast.TextovaData}</h5>\n";
      }
      else if (cast is CastTextuSTextem)
      {
        AktualniTextVerse += cast.TextovaData;
      }
      else if (cast is Kniha)
      {
        foreach (CastTextu potomek in cast.Potomci)
        {
          PocitadloVerse = 1;

          VygenerovatCastSql(potomek, bible, kniha);

          PridatRozpracovanyVers();
        }
      }
    }

    private void PridatRozpracovanyVers()
    {
      if (!string.IsNullOrEmpty(AktualniTextVerse))
      {
        Verse.Add($"({PoradiKnihy}, '{PocitadloKapitol}', '{PocitadloVerse}', '{AktualniTextVerse}', " +
                  $"'{OstripovatVers(AktualniTextVerse)}', {GlobalniPocitadloVersu++})");
        PocitadloVerse++;
      }

      AktualniTextVerse = string.Empty;
    }

    private object OstripovatVers(string aktualniTextVerse)
    {
      return Regex.Replace(aktualniTextVerse, "[^\\x00-\\x7f]", string.Empty);
    }

    private void VlozitSqlNadpis(string nadpis)
    {
      Nadpisy.Add($"INSERT INTO bible_nadpisy (id, kniha_id, kapitola, vers, text, offset) " +
                  $"VALUES({PocitadloNadpisu}, {PoradiKnihy}, '{PocitadloKapitol}', '{PocitadloVerse}', '{nadpis}', 0);\n");

      PocitadloNadpisu++;
    }

    #endregion
  }
}