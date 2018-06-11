using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        StavecKnihy.Append("INSERT INTO bible_knihy (id, kod, nazev, order) VALUES " +
                           $"({poradi + 1}, '{kniha.Id}', '{bible.MapovaniZkratekKnih[kniha.Id].Nadpis}', {poradi + 1});\n");

        Tuple<StringBuilder, StringBuilder> nadpisyVerseKnihy = VygenerovatSqlProKnihu(bible, kniha);

        StavecNadpisy.Append(nadpisyVerseKnihy.Item1);
        StavecVerse.Append(nadpisyVerseKnihy.Item2);
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

    public Tuple<StringBuilder, StringBuilder> VygenerovatSqlProKnihu(Bible bible, Kniha kniha)
    {
      var dataKnihy = VygenerovatCastSql(kniha, bible, kniha);
      return dataKnihy;
    }

    public Tuple<StringBuilder, StringBuilder> VygenerovatCastSql(CastTextu cast, Bible bible, Kniha kniha)
    {
      StringBuilder nadpisy = new StringBuilder();
      StringBuilder verse = new StringBuilder();

      if (cast is HlavniCastKnihy hlavniCast)
      {
        VlozitSqlNadpis(nadpisy, hlavniCast.Nadpis);
        PocitadloNadpisu++;

        foreach (CastTextu potomek in cast.Potomci)
        {
          Tuple<StringBuilder, StringBuilder> sqlCast = VygenerovatCastSql(potomek, bible, kniha);

          nadpisy.Append(sqlCast.Item1);
          verse.Append(sqlCast.Item2);
        }
      }
      else if (cast is CastKnihy castKnihy)
      {
        VlozitSqlNadpis(nadpisy, castKnihy.Nadpis);
        PocitadloNadpisu++;

        foreach (CastTextu potomek in cast.Potomci)
        {
          Tuple<StringBuilder, StringBuilder> sqlCast = VygenerovatCastSql(potomek, bible, kniha);

          nadpisy.Append(sqlCast.Item1);
          verse.Append(sqlCast.Item2);
        }
      }
      else if (cast is UvodKapitoly)
      {
        PocitadloKapitol++;
        PocitadloVerse = 1;
      }
      else if (cast is Vers)
      {
        if (!string.IsNullOrEmpty(AktualniTextVerse))
        {
          // TODO: přidej aktualní verš?
        }

        PocitadloVerse++;
        AktualniTextVerse = string.Empty;

        foreach (CastTextu potomek in cast.Potomci)
        {
          Tuple<StringBuilder, StringBuilder> sqlCast = VygenerovatCastSql(potomek, bible, kniha);

          nadpisy.Append(sqlCast.Item1);
          verse.Append(sqlCast.Item2);
        }
      }
      else if (cast is Kniha)
      {
        foreach (CastTextu potomek in cast.Potomci)
        {
          Tuple<StringBuilder, StringBuilder> sqlCast = VygenerovatCastSql(potomek, bible, kniha);

          nadpisy.Append(sqlCast.Item1);
          verse.Append(sqlCast.Item2);
        }
      }

      return new Tuple<StringBuilder, StringBuilder>(nadpisy, verse);
    }

    private void VlozitSqlNadpis(StringBuilder nadpisy, string nadpis)
    {
      nadpisy.AppendLine($"INSERT INTO bible_nadpisy (id, kniha_id, kapitola, vers, text, offset) " +
                         $"VALUES({PocitadloNadpisu}, {PoradiKnihy}, '{PocitadloKapitol}', '{PocitadloVerse}', '{nadpis}', 0),");
    }

    #endregion
  }
}