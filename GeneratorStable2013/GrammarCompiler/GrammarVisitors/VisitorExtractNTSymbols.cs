 
using System.Collections.Generic;
using GrammarCompiler.GrammarVisitors;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  /// <summary>
  /// DerivationContext for <see cref="VisitorExtractNTSymbols"/>
  /// </summary>
  internal class ExtractNotTerminalsContext : DerivationContext //Base
  {
    public List<string> NotTerminals;
    public List<Seqence> Seqences;

    public ExtractNotTerminalsContext(Grammar aGrammar) : base(aGrammar)
    {
      NotTerminals = new List<string>();
      Seqences = new List<Seqence>();
    }
  }

  /// <summary>
  /// Обходит правую часть правил грамматик и собирает нетерминальные символы в правой части правила в список
  /// Также извлекает последовательности, TODO в будущем переименовать
  /// </summary>
  internal class VisitorExtractNTSymbols : WalkVisitorBase
  {
    public override IDerivation Visit(NonTerminal aSymbol, DerivationContext aContext)
    {
      ExtractNotTerminalsContext lContext = aContext as ExtractNotTerminalsContext;
      lContext.NotTerminals.Add(aSymbol.Text);
      return null;
    }

    public override IDerivation Visit(Seqence aSeqence, DerivationContext aContext)
    {
      ExtractNotTerminalsContext lContext = aContext as ExtractNotTerminalsContext;
      lContext.Seqences.Add(aSeqence);
      return base.Visit(aSeqence, aContext);
    }
  }
}