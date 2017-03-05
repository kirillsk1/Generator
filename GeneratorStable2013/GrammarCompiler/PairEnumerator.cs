 
using System;
using System.Collections.Generic;

namespace GrammarCompiler
{
  public class PairEnumerator
  {
    private readonly Generator mGenerator;
    private readonly Grammar mGrammar;

    public delegate void PairDelegate(string aRuleA, string aRuleB, List<string> aPath);

    public PairEnumerator(Generator aGenerator)
    {
      if (aGenerator == null) throw new ArgumentNullException();
      mGenerator = aGenerator;
      mGrammar = mGenerator.Grammar;
    }

    public void Enumerate(PairDelegate aPairDelegate)
    {
      foreach (Rule lRuleA in mGrammar.Rules.Values)
      {
        //���������, �������� �� � �� Main
        if (mGrammar.GrammarGraph.isAchieved(mGrammar.MainSymbol.Text, lRuleA.Name))
        {
          List<string> lPathMainToA = mGrammar.GrammarGraph.GetPath(mGrammar.MainSymbol.Text, lRuleA.Name);
          lPathMainToA.RemoveAt(lPathMainToA.Count - 1);
          foreach (Rule lRuleB in mGrammar.Rules.Values)
          {
            //����� ���� �� lRuleA � lRuleB
            bool lAchieved = mGrammar.GrammarGraph.isAchieved(lRuleA.Name, lRuleB.Name);
            if (lAchieved)
            {
              List<string> lPathAtoB = mGrammar.GrammarGraph.GetPath(lRuleA.Name, lRuleB.Name);
              lPathAtoB.InsertRange(0, lPathMainToA);
              string lPath = string.Join(" -> ", lPathAtoB.ToArray());
              Log(string.Format("���� ��� ���� {0}, {1}: {2}", lRuleA.Name, lRuleB.Name, lPath));
              if (null != aPairDelegate)
              {
                //Log(
                aPairDelegate(lRuleA.Name, lRuleB.Name, lPathAtoB);
                //   );
              }
            }
            else
            {
              Log(string.Format("���� {0}, {1} �����������", lRuleA.Name, lRuleB.Name));
            }
          }
        }
        else
        {
          Log(string.Format("������ {0} �� �������� �� ��������", lRuleA.Name));
        }
      }
    }

    private void Log(string aString)
    {
      mGenerator.ResultSaver.MessageToLog(aString);
    }
  }
}