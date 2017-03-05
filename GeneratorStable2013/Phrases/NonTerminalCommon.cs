 
using System.Collections.Generic;

namespace GrammarCompiler.PhraseHierarchy
{
  public class NonTerminalCommon : PhraseBase
  {
    public string mText;
    public readonly List<NonTerminal> AllOccurences = new List<NonTerminal>();

    public NonTerminalCommon(Grammar aGrammar, string aText) : base(aGrammar)
    {
      mText = aText;
    }
  }
}