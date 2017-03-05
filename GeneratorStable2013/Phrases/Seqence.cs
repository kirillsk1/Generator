 
using System;

namespace GrammarCompiler.PhraseHierarchy
{
  /// <summary>
  /// ������������ ������������������ �� ���������� ������������� ������ ����� ����������
  /// ��������, 
  /// S := A, B
  /// </summary>  
  public class Seqence : PhraseList
  {
    public bool InsertSpace;

    public Seqence(Grammar aGrammar, IPhrase aPhr)
      : base(aGrammar, aPhr)
    {
      InsertSpace = true;
    }

    public Seqence(Grammar aGrammar)
      : base(aGrammar)
    {
      InsertSpace = true;
    }

    public override IDerivation Accept(DerivationContext aContext)
    {
      return aContext.Visitor.Visit(this, aContext);
    }

    //public override bool IsCyclic(string aRuleName)
    //{
    //  {
    //    foreach (IPhrase phr in mPhrases)
    //    {
    //      if (phr.IsCyclic(aRuleName))
    //      {
    //        return true;
    //      }
    //    }
    //    return false;
    //  }
    //}

    /// <summary>
    /// ���������� ���������������������. ���������� SubString
    /// </summary>
    /// <param name="from_index">��������� ������</param>
    /// <param name="count">���������� ���������</param>
    /// <returns>null, ���� ���������� ��������� = 0</returns>
    internal Seqence SubSequence(int from_index, int count)
    {
      if (count == 0) return null;
      if (count < 0 || from_index < 0) throw new ArgumentOutOfRangeException("SubSequence params can not be < 0");
      if (from_index + count > Phrases.Count)
        throw new ArgumentOutOfRangeException(
          string.Format("SubSequence out of range. Phrases.Count: {0}, from_index: {1}, count: {2}", Phrases.Count,
                        from_index, count));
      Seqence result = new Seqence(Grammar, Phrases[from_index++]);
      count--;
      while (count > 0)
      {
        result.Phrases.Add(Phrases[from_index++]);
        count--;
      }
      return result;
    }

    public override string ToString()
    {
      string result = "";
      foreach (IPhrase phrase in Phrases)
      {
        if (result != "") result += ", ";
        if (phrase is AlternativeSet)
        {
          result += "(" + (phrase as AlternativeSet).ToString(false) + ")";
        }
        else
          result += phrase.ToString();
      }
      return result;
    }
  }
}