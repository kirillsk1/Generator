 
using System.Windows.Forms;

namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// ������� ����� ��� ���� ��������� �������� ����������� ������������� ������ ����� ������� ����������.
  /// ��������� IPhrase
  /// ��������:
  ///   ������ �� Grammar, Alias 
  ///   ��������� UsageCount, IncUsageCount 
  /// </summary>  
  public class PhraseBase : IPhrase
  {
    public readonly Grammar Grammar;
    private string mAlias;
    private bool mIsCyclic;
    private int mUsageCount;
    private IPhrase mParent;

    public PhraseBase(Grammar aGrammar) : this(aGrammar, null)
      // ����� ����� ���. �������� �� ������ �������� ��� ��������������� � ����� ��������.
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
    /// ������� ������� ��� ���� ������� ��� ������������
    /// </summary>
    public int UsageCount
    {
      get { return mUsageCount; }
    }

    /// <summary>
    /// ���������� ��� ������ ������������
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
    /// ����� ����� ������ �� ����� ���� �� �����, ��� ������ ����� �������� ����������� ������
    /// � ����� ������ ������ ������ �� ���, ��� ��� ���� ����� ����������� (��������, ������������������ ������ ����� �����������,
    /// � ����� ����������� ����� �����������, ���� ��� ������������ �����������.
    /// ���� ��� ����� �����������, ��� ������ �������������� ��� ����� �� ��������, �.�. ������� Parent.PropagateCycle
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