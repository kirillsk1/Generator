using System;
namespace GrammarCompiler
{
  /// <summary>
  /// Символ может быть IsCyclic = true по разным причиам.
  /// Этот enum Уточняет по каким (источник IsCyclic = true).
  /// </summary>
  [Flags]
  public enum CycicKind
  {
    None = 0,
    /// <summary>
    /// Помечен как первичный циклический нетерминал (^ оранжевый в LiveGram)
    /// </summary>
    CyclicOrigin = 1,
    /// <summary>
    /// Опущен как повтор (* бледно розовый в LiveGram)
    /// </summary>
    SkippedAsSeen = 2,
    /// <summary>
    /// CyclicAttractor
    /// Означает что из этого символа детерминированно (обязательно - нет других альтернатив) выводится циклический символ
    /// </summary>
    CyclicPropagated = 4
  }  
}
