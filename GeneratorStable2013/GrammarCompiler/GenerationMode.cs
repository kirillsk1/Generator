 
namespace GrammarCompiler
{
  public enum eGenerationMode
  {
    /// <summary>
    /// Рекурсивный обход сверху вниз, слева направо
    /// </summary>
    RecursiveTopDown,
    /// <summary>
    /// Обход такой же, как и RecursiveTopDown, только выполняется итеративно, чтобы избежать возможности StackOverflow
    /// Это основной рекомендуемый режим, поскольку он необходим для реализации семантических связей (обращение к уже выведенной части программы).
    /// </summary>
    IterativeTopDown,
    /// <summary>
    /// Вывод по слоям (слева направо сверху вниз).
    /// Для демонстрации
    /// </summary>
    IterativeLeftRight
  }
}