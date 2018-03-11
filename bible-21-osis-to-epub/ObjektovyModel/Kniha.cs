namespace BibleDoEpubu.ObjektovyModel
{
  /// <summary>
  /// Reprezentuje jednu knihu Bible (například Genesis).
  /// </summary>
  internal class Kniha : CastTextu
  {
    /// <summary>
    /// ID knihy, např. "Gen".
    /// </summary>
    public string Id { get; set; }
  }
}