using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using BibleDoEpubu.ObjektovyModel;

namespace BibleDoEpubu
{
  internal class SqlGenerator
  {
    #region Vlastnosti

    private List<PouzitaPoznamka> PouzitePoznamky
    {
      get;
      set;
    } = new List<PouzitaPoznamka>();

    #endregion

    #region Metody

    public string VygenerovatSql(Bible bible)
    {
      StringBuilder stavec = new StringBuilder();
      int poradi = 1;


      foreach (Kniha kniha in bible.Knihy)
      {
        stavec.Append($"INSERT INTO bible_knihy (id, kod, nazev, order) VALUES " +
                      $"({poradi}, {kniha.Id}, {bible.MapovaniZkratekKnih[kniha.Id]}, {poradi});\n");
        stavec.Append(VygenerovatSqlProKnihu(bible, kniha, poradi++));
      }

      return stavec.ToString();
    }

    public string VygenerovatSqlProKnihu(Bible bible, Kniha kniha, int poradi)
    {
      

      return string.Empty;
    }

    public string VygenerovatCastSql(CastTextu cast, Bible bible, Kniha kniha, int poradi)
    {
      StringBuilder stavec = new StringBuilder();

      if (cast is HlavniCastKnihy)
      {

      }
      else if (cast is UvodKapitoly)
      {

      }
      else if (cast is CastKnihy)
      {

      }
      else if (cast is Vers)
      {

      }

      return stavec.ToString();
    }

    #endregion
  }
}