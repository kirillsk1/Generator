 
using System.Collections.Generic;
using System.Windows.Forms;

namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// Общий базовый класс для Последовательности и Набора Альтернатив, поскольку и то и другое содержит список подфраз
  /// </summary>
  public abstract class PhraseList : PhraseBase
  {
    protected readonly List<IPhrase> mPhrases;

    public List<IPhrase> Phrases
    {
      get { return mPhrases; }
    }

    public int Count
    {
      get { return mPhrases.Count; }
    }

    public PhraseList(Grammar aGrammar, IPhrase aPhr)
      : this(aGrammar)
    {
      mPhrases.Add(aPhr);
    }

    public PhraseList(Grammar aGrammar)
      : base(aGrammar)
    {
      mPhrases = new List<IPhrase>();
    }

    public static explicit operator List<IPhrase>(PhraseList phr)
    {
      return phr.mPhrases;
    }

    public virtual void Add(IPhrase aPhr2)
    {
      if (aPhr2 is PhraseList && GetType() == aPhr2.GetType())
      {
        PhraseList lPhrList = aPhr2 as PhraseList;
        mPhrases.AddRange((List<IPhrase>) lPhrList);
      }
      else
      {
        mPhrases.Add(aPhr2);
      }
    }

    public override TreeNode ToTree(TreeNode aNode)
    {
      TreeNode lNode = base.ToTree(aNode);
      //lNode.Text += (IsCyclic ? " *C*" : "");
      foreach (IPhrase lPhras in mPhrases)
      {
        lPhras.ToTree(lNode);
      }
      return lNode;
    }
  }
}