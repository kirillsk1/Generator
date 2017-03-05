 
using System.Collections.Generic;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler
{
  /// <summary>
  /// ����� �������� ������ ������, ��������,
  /// main -> stat -> for -> if
  /// � ����������� ��������� ��������� ������ (WaitingSymbol)
  /// 
  /// ������� � ���������� ������� � ���� ���������� ��� ��������� �����������,
  /// ���� �� ��������� � WaitingSymbol
  /// </summary>
  internal class Twig //Prutik
  {
    private readonly List<string> mPath;
    private int mWaitingSymbolIndex;
    private bool mIsPassed;

    public Twig(List<string> aPath)
    {
      mPath = aPath;
      mWaitingSymbolIndex = 0;
      mIsPassed = false;
    }

    public string WaitingSymbol
    {
      get { return mPath[mWaitingSymbolIndex]; }
    }

    public void MatchNext(string aSymbolName)
    {
      if (!mIsPassed && aSymbolName == WaitingSymbol)
      {
        //Move next
        if (mWaitingSymbolIndex < mPath.Count - 1)
        {
          mWaitingSymbolIndex++;
        }
        else
        {
          mIsPassed = true;
        }
      }
    }

    public bool IsPassed
    {
      get { return mIsPassed; }
    }
  }

  /// <summary>
  /// �� ������ ����������� ���������� ������ ���������� ��������� �������� ������
  /// � ������� ������ � ���������� ��������� ����� �� ����� ���������������� ������.
  /// </summary>
  internal class VisitorExpandPairs : VisitorExpandRnd
  {
    public Twig Twig;
    private ExtractNotTerminalsContext mExtrSymContext;

    public override IDerivation Visit(Terminal aSymbol, DerivationContext aContext)
    {
      return base.Visit(aSymbol, aContext);
    }

    public override IDerivation Visit(NonTerminal aSymbol, DerivationContext aContext)
    {
      //���� �� ����� �� �������� ���������� �������, ��������� � ����������
      Twig.MatchNext(aSymbol.Text);
      return base.Visit(aSymbol, aContext);
    }

    public override IDerivation VisitInternal(AlternativeSet aAlternativeSet, DerivationContext aContext)
    {
      if (!Twig.IsPassed)
      {
        //������� ����������� ������� ������� ���� ���
        if (null == mExtrSymContext)
        {
          mExtrSymContext = new ExtractNotTerminalsContext(aContext.Grammar);
          mExtrSymContext.Visitor = new VisitorExtractNTSymbols();
        }
        //��������� ������ �����������, ���������� �������� ������
        AlternativeSet lGoodAlts = new AlternativeSet(aContext.Grammar);
        foreach (IPhrase lPhr in aAlternativeSet.Phrases)
        {
          mExtrSymContext.NotTerminals.Clear();
          lPhr.Accept(mExtrSymContext);
          if (mExtrSymContext.NotTerminals.Contains(Twig.WaitingSymbol))
          {
            lGoodAlts.Phrases.Add(lPhr);
          }
        }
        if (lGoodAlts.Count > 0)
        {
          int i = mRnd.Next(lGoodAlts.Count);
          return lGoodAlts.GetAlternative(aContext, i);
        }
      }
      return base.VisitInternal(aAlternativeSet, aContext);
    }
  }
}