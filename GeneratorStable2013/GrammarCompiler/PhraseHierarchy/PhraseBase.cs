 
using System.Windows.Forms;

namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// Базовый класс для всех возможных объектов внутреннего представления правой части правила грамматики.
  /// Реализует IPhrase
  /// Содержит:
  ///   ссылку на Grammar, Alias 
  ///   Реализует UsageCount, IncUsageCount 
  /// </summary>  
  public class PhraseBase : IPhrase
  {
    public readonly Grammar Grammar;
    private string mAlias;
    private bool mIsCyclic;
    private int mUsageCount;
    private IPhrase mParent;

    public PhraseBase(Grammar aGrammar) : this(aGrammar, null)
      // Пусть будет так. Родитель не всегда известен при конструировании и может меняться.
    {
    }

    public PhraseBase(Grammar aGrammar, IPhrase aParent)
    {
      Grammar = aGrammar;
      mParent = aParent;
    }

    public virtual IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }

    public virtual TreeNode ToTree(TreeNode aNode)
    {
      return aNode.Nodes.Add(GetType().Name);
    }

    public bool IsCyclic
    {
      get { return mIsCyclic; }
      set { mIsCyclic = value; }
    }

    /// <summary>
    /// Считает сколько раз была выбрана эта альтернатива
    /// </summary>
    public int UsageCount
    {
      get { return mUsageCount; }
    }

    /// <summary>
    /// Вызывается при выборе альтернативы
    /// </summary>
    public void IncUsageCount()
    {
      mUsageCount++;
    }

    public string Alias
    {
      get { return mAlias; }
      set { mAlias = value; }
    }

    public IPhrase Parent
    {
      get { return mParent; }
      set { mParent = value; }
    }

    /// <summary>
    /// Вызов этого метода на фразе дает ее знать, что данная фраза содержит циклический символ
    /// И фраза должна решить значит ли это, что она тоже будет циклической (например, последовательность всегда будет циклической,
    /// а набор альтернатив будет циклическим, если все альтернативы циклические.
    /// Если эта фраза циклическая, она должна распространить это вверх по иерархии, т.е. вызвать Parent.PropagateCycle
    /// </summary>
    public virtual void PropagateCycle()
    {
      IsCyclic = true;
      if (Parent != null)
      {
        Parent.PropagateCycle();
      }
    }
  }
}