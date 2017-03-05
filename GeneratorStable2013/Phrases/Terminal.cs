 
using System.Windows.Forms;

namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// Представляет терминальный символ во внутреннем представлении правой части грамматики
  /// </summary>
  public class Terminal : PhraseBase
  {
    private string mText;

    public string Text
    {
      get { return mText; }
      set { mText = value; }
    }

    public Terminal(Grammar aGrammar, string aText)
      : base(aGrammar)
    {
      mText = aText;
    }


    public Terminal(Terminal aSymbol)
      : base(aSymbol.Grammar)
    {
      mText = aSymbol.Text;
    }

    public override TreeNode ToTree(TreeNode aNode)
    {
      TreeNode lNode = base.ToTree(aNode);
      //lNode.Text = Text + (IsCyclic ? " *C*" : "");
      return lNode;
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }

    public override string ToString()
    {
      return string.Format(@"""{0}""", Text);
    }
  }
}