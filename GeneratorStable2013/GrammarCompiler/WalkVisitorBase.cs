 
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler.GrammarVisitors
{
  /// <summary>
  /// Базовый визитор предназначен для простого обхода ВП правой части грамматики.
  /// </summary>
  public class WalkVisitorBase : IVisitor
  {
    public virtual IDerivation Visit(Terminal aSymbol, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(NonTerminal aSymbol, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(Seqence aSeqence, DerivationContext aContext)
    {
      for (int i = 0; i < aSeqence.Count; i++)
      {
        aSeqence.Phrases[i].Accept(NewContext(aSeqence, aContext));
      }
      return null;
    }

    /// <summary>
    /// Нужен чтобы в производных классах можно было создавать новые контексты <see cref="CyclesDetectVisitor"/>
    /// </summary>
    /// <param name="aPhrase">phrase переданная в Visit</param>
    /// <param name="aContext">DerivationContext</param>
    /// <returns>new DerivationContext</returns>
    protected virtual DerivationContext NewContext(IPhrase aPhrase, DerivationContext aContext)
    {
      return aContext;
    }

    public virtual IDerivation Visit(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      for (int i = 0; i < aAlternativeSet.Count; i++)
      {
        aAlternativeSet.Phrases[i].Accept(NewContext(aAlternativeSet, aContext));
      }
      return null;
    }

    //

    public virtual IDerivation Visit(TransCallPhrase aTransCallPhrase, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(QuantifiedPhrase aQuantifiedPhrase, DerivationContext aContext)
    {
      aQuantifiedPhrase.Phrase.Accept(NewContext(aQuantifiedPhrase, aContext));
      return null;
    }

    public virtual IDerivation Visit(PlaceHolderPhrase aPlaceHolderPhrase, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(PlaceHolderAssignPhrase aPlaceHolderAssignPhrase, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(ExprOp aExprOp, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(Access aAccess, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(AccessArray aAccessArray, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(AccessSeq aAccessSeq, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(ExprInt aExpr, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(ExprDouble aExpr, DerivationContext aContext)
    {
      return null;
    }

    public virtual IDerivation Visit(IPhrase aPhr, DerivationContext aContext)
    {
      return null;
    }
  }
}