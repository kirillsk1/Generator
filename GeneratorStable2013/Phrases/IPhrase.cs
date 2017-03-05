 
using System.Windows.Forms;

namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// Фраза - один символ или последовательность или набор альтернатив или другая конструкция,
  /// например, повторение символа
  /// По умолчанию это один символ
  /// 
  /// a := b
  /// a := b, c
  /// a := b | c
  /// a := b, (c | d), e, ((x,y) | z)
  /// a := (b, c) | (d, e) - так расставлены скобки по умолчанию
  /// a := a, (b | c){2..5}
  /// a := a, (b | c)+
  /// a := a, (b | c)&lt;=5
  /// Phrase := Sym,  
  /// 
  /// Этот интерфейс лежит в основе всей иерархии внутреннего представления правой части грамматики
  /// </summary>
  public interface IPhrase
  {
    IDerivation Accept(DerivationContext aContext);
    TreeNode ToTree(TreeNode aParentNode);
    bool IsCyclic { get; set; }
    int UsageCount { get; }
    void IncUsageCount();
    string Alias { get; set; }
    IPhrase Parent { get; set; }

    /// <summary>
    /// Вызов этого метода на фразе дает ее знать, что данная фраза содержит циклический символ
    /// И фраза должна решить значит ли это, что она тоже будет циклической (например, последовательность всегда будет циклической,
    /// а набор альтернатив будет циклическим, если все альтернативы циклические.
    /// Если эта фраза циклическая, она должна распространить это вверх по иерархии, т.е. вызвать Parent.PropagateCycle
    /// </summary>
    void PropagateCycle();
  }
}