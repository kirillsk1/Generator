 
namespace GrammarCompiler.Transforms
{
  /// <summary>
  /// Выполняет Эквивалентные Преобразования над грамматикой
  /// Базовый класс.
  /// </summary>
  public abstract class GrammarTransforms
  {
    protected Grammar mGrammar;
    protected TransformStatistics mStatistics;

    /// <summary>
    /// Надо переопределить в производных классах
    /// </summary>
    protected abstract void DoTransform();

    /// <summary>
    /// Выполняет преобразование
    /// </summary>
    /// <param name="grammar">Преобразование выполняется над этим экземпляром</param>
    /// <returns>Информация о произведенных заменах</returns>
    public TransformStatistics Transform(Grammar aGrammar)
    {
      mGrammar = aGrammar;
      mStatistics = new TransformStatistics();

      DoTransform();
      aGrammar.FindCycles();
      return mStatistics;
    }
  }
}