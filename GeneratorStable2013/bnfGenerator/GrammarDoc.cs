 
using GrammarCompiler;

namespace bnfGenerator
{
  public partial class GrammarDoc : SciEditDoc
  {
    public Grammar mGrammar = null;
    public ErrorList ErrorListControl;
    public OutputDoc mLexerDebugOutput;
    public DerivationTree mDerivationTree;

    public void RefreshGrammarTree()
    {
      if (mDerivationTree != null)
      {
        mDerivationTree.treeView1.Nodes.Clear();
        mGrammar.ToTree(mDerivationTree.treeView1.Nodes);
        mDerivationTree.treeView1.ExpandAll();
      }
    }

    protected override void ReParse()
    {
      base.ReParse();
      mGrammar = Grammar.FromText(Text);
      if (ErrorListControl != null)
      {
        ErrorListControl.Errors = mGrammar.SyntaxErrors;
        //Показать ошибки
        if (mGrammar.SyntaxErrors.Count > 0)
        {
          ErrorListControl.Show();
        }
      }
      if (mLexerDebugOutput != null)
      {
        mLexerDebugOutput.SetText(mGrammar.LexerDebugText);
      }
      RefreshGrammarTree();
    }


    public GrammarDoc()
    {
      InitializeComponent();
      WorkSubDir = "MyGrammars";
      FileExt = "*.txt";
      SciLang = "cpp";
    }
  }
}