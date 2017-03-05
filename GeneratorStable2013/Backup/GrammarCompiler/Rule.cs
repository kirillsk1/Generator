 
using GrammarCompiler.PhraseHierarchy;
using Tools;

namespace GrammarCompiler
{
  public class Rule
  {
    public string Name;
    public IPhrase RightSide;
    private readonly string mSeparator;
    //protected Grammar mGrammar;

    public Rule(Grammar aGrammar, Lexema aLex)
    {
      if (aLex == null || aLex.Type != LexemType.NotTerminal)
      {
        throw new GrammarSyntaxException("Rule Name expected");
      }

      Name = aLex.Text;
      aLex = aLex.Next();
      if (aLex != null && aLex.Type == LexemType.Terminal)
      {
        mSeparator = aLex.Text;
        aLex = aLex.Next();
      }

      if (aLex == null || aLex.Type != LexemType.IsSign)
      {
        throw new GrammarSyntaxException(":= expected");
      }

      aLex = aLex.Next();

      PhraseParser lParser = new PhraseParser(aGrammar, aLex);
      RightSide = lParser.ParseRule();
    }

    public override string ToString()
    {
      return string.Format("{0} {2}:= {1}", Name, RightSide, mSeparator);
    }

    public IDerivation Expand(DerivationContext aContext)
    {
      TLog.Write("R", TLog.IndentStr() + Name);
      TLog.Indent += 2;
      IDerivation lDeriv = RightSide.Accept(aContext);
      TLog.Indent -= 2;
      ListDerivation lListDer = lDeriv as ListDerivation;
      if (lListDer != null && mSeparator != null)
      {
        lListDer.Separator = mSeparator;
      }
      return lDeriv;
    }
  }
}