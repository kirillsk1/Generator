 
using System;
using System.IO;
using GrammarCompiler.GrammarVisitors;
using GrammarCompiler.PhraseHierarchy;

namespace GrammarCompiler.Transforms
{
  /// <summary>
  /// Преобразует грамматику так, чтобы в ней было меньше повторов.
  /// 
  /// </summary>    
  public class ToQuantifiers : GrammarTransforms
  {
    private Rule mCurrentRule;
    private string mRule_before;
    private static bool wasTransformed;

    protected override void DoTransform()
    {
      mStatistics.Log("\r\n *** Преобразования последовательностей из двух символов ***\r\n");
      StatStatsTransform();
      mStatistics.Log("\r\n *** Обобщенные преобразования последовательностей произвольной длины ***\r\n");
      GeneralTransform();
      File.WriteAllText(@"C:\transform-statistics.txt", mStatistics.ToString());
    }

    /// Частное преобразование. Проверяет только правила состоящие из двух альтернатив.
    /// Находит правила вида
    /// S := A | A, S => A+                 //шаблон (*1)
    /// S := A | S, A => A+                 //шаблон (*1')
    /// B := A | A, S => A, [S]             //шаблон (*2)
    /// B := A | S, A    [S], A             //шаблон (*2')
    /// и заменяет их на
    /// S := A+  // {1..*}
    /// B := A, [S]     //{0..1}
    ///
    /// 
    /// stats := stat | stat, Stats      //Пример (*1)
    /// Exprs := Add | Add, Adds        //Пример (*2)
    /// struct_declaration_list := struct_declaration\   //Пример (*1')
    ///                            | struct_declaration_list, struct_declaration
    private void StatStatsTransform()
    {
      foreach (Rule rule in mGrammar.Rules.Values)
      {
        AlternativeSet alt = rule.RightSide as AlternativeSet;
        if (alt != null)
        {
          if (alt.Phrases.Count == 2)
          {
            NonTerminal first_symbol_A = alt.Phrases[0] as NonTerminal;
            Seqence sequence = alt.Phrases[1] as Seqence;
            if (first_symbol_A != null && sequence != null && sequence.Phrases.Count == 2)
            {
              NonTerminal seq_symbol1 = sequence.Phrases[0] as NonTerminal;
              NonTerminal seq_symbol2 = sequence.Phrases[1] as NonTerminal;
              if (seq_symbol1 != null && seq_symbol2 != null)
              {
                NonTerminal seq_symbol_A = null;
                NonTerminal seq_symbol_S = null;
                int repl_i = 0;
                // Структура как нужно, теперь анализируем какие символы одинаковы?
                if (first_symbol_A.Text == seq_symbol1.Text)
                {
                  seq_symbol_A = seq_symbol1;
                  seq_symbol_S = seq_symbol2;
                  repl_i = 1;
                }
                else if (first_symbol_A.Text == seq_symbol2.Text)
                {
                  seq_symbol_A = seq_symbol2;
                  seq_symbol_S = seq_symbol1;
                  repl_i = 0;
                }

                if (seq_symbol_A != null)
                {
                  string rule_before = rule.ToString();
                  if (rule.Name == seq_symbol_S.Text)
                  {
                    //Совпадение с шаблоном (*1) или (*1') найдено
                    // создаем квантификатор вида {stat}
                    QuantifiedPhrase quant = new QuantifiedPhrase(mGrammar, first_symbol_A, 1, Int32.MaxValue);
                    // подменяем им правую часть
                    rule.RightSide = quant;
                  }
                  else
                  {
                    //Совпадение с шаблоном (*2) или (*2') найдено
                    // создаем квантификатор вида A?
                    QuantifiedPhrase quant = new QuantifiedPhrase(mGrammar, seq_symbol_S, 0, 1);
                    // подменяем альтернативу в правой части последовательностью
                    rule.RightSide = sequence;
                    // подменяем второй элемент последовательности квантификатором
                    sequence.Phrases[repl_i] = quant;
                  }
                  mStatistics.AffectedRules.Add(rule.Name);
                  mStatistics.Log(rule_before, rule.ToString());
                }
              }
            }
          }
          //GeneralizedNonMandatryInclusion(alt);
        }
      } //foreach
    }


    //Обобщенное преобразование. Проверяет все альтернативы в правой части.
    private void GeneralTransform()
    {
      DerivationContext context = new DerivationContext(mGrammar);
      context.Visitor = new AltVisitor(this);
      foreach (Rule rule in mGrammar.Rules.Values)
      {
        mCurrentRule = rule;
        if (rule.Name == "struct_or_union")
        {
          ;
        }
        wasTransformed = true;
        while (wasTransformed)
        {
          wasTransformed = false;
          rule.RightSide.Accept(context);
        }
      }
    }

    /// <summary>
    /// Проверяет правило на наличие такого шаблона:
    /// 
    /// R := A1 | A2 | ... | An
    /// 
    /// Ai := B1,...,Bn,            E1,...,En
    /// Aj := B1,...,Bn, C1,...,Cn, E1,...,En
    /// 
    /// => (делает преобразование)
    /// remove Ai
    /// Aj := B1,...,Bn, (C1,...,Cn)?, E1,...,En
    /// 
    /// Еще более общий случай:
    /// Ai := B1,...,Bn, D1,...,Dq  E1,...,Ek
    /// Aj := B1,...,Bn, C1,...,Cp, E1,...,Ek
    /// 
    /// => (делает преобразование)
    /// remove Ai
    /// Aj := B1,...,Bn, (D1,...,Dq | C1,...,Cp), E1,...,Ek
    /// 
    /// </summary>
    /// <param name="R">Right side of rule as AlternativeSet</param>    
    /// <returns>Возвращает true если в правило было внесено изменение, при этом сразу же выходит</returns>
    private bool GeneralizedNonMandatryInclusion(AlternativeSet R)
    {
      for (int i = 0; i < R.Phrases.Count; i++) // 000
        for (int j = 0; j < i; j++) // 100 j<i , FindNonMandatryInclusion учитывает сразу Ai,Aj и Aj,Ai
        {
          // 110
          Seqence Ai = R.Phrases[i] as Seqence;
          Seqence Aj = R.Phrases[j] as Seqence;

//argument_expression_list := assignment_expression\
//   | argument_expression_list, ",", assignment_expression
//=>
//argument_expression_list := assignment_expression\
//   | [ argument_expression_list, "," ], assignment_expression


//argument_expression_list := assignment_expression\
//   | [ argument_expression_list, "," ], assignment_expression
//=>
//argument_expression_list := assignment_expression\
//   | [ [ argument_expression_list, "," ] ], assignment_expression

          //if (Ai != null && Ai.Count > 1 || Aj != null && Aj.Count > 1)
          //{
          //  if (R.Phrases[i] is Symbol)
          //  {
          //    Ai = new Seqence(mGrammar, R.Phrases[i]);
          //  }
          //  if (R.Phrases[j] is Symbol)
          //  {
          //    Aj = new Seqence(mGrammar, R.Phrases[j]);
          //  }
          if (Ai != null && Aj != null)
            // Если встретилась НЕ последовательность, ничего страшного, продолжаем просмотр этого правила
          {
            bool swap;
            mRule_before = mCurrentRule.ToString();
            Seqence new_A = FindNonMandatryInclusion(Ai, Aj, out swap);
            if (new_A != null)
            {
              if (!swap)
              {
                R.Phrases[j] = new_A;
                R.Phrases.Remove(Ai);
              }
              else
              {
                R.Phrases[i] = new_A;
                R.Phrases.Remove(Aj);
              }
              wasTransformed = true;
              mStatistics.Log(mRule_before, mCurrentRule.ToString());
              return true;
            }
          }
          //}
        }
      wasTransformed = false;
      return false;
    }

    /// <summary>
    /// 1) Ищет совпадения в начале и в конце последовательностей.
    /// Если ничего не совпало выходим
    /// 2) Удаляем последовательность которая короче. Если Aj короче swap = true
    /// 3) Берем подпоследовательности Cp,Dq
    /// 4) Мастрячим замену - если D нету, квантификатор, иначе альтернативу
    /// </summary>
    /// <param name="Ai">B1,...,Bn,[D1,...,Dq],E1,...,Ek</param>
    /// <param name="Aj">B1,...,Bn, C1,...,Cp, E1,...,Ek</param>
    /// <param name="swap">true если удаляем Aj, а не Ai</param>
    /// <returns>B1,...,Bn, (C1,...,Cn)?, D1,...,Dn</returns>
    private Seqence FindNonMandatryInclusion(Seqence Ai, Seqence Aj, out bool swap)
    {
      // 1) Ищет совпадения в начале и в конце последовательностей.
      int start_len = 0; //FindEqualSeq(A1, A2, 1);
      int Ci = Ai.Phrases.Count - 1;
      int Cj = Aj.Phrases.Count - 1;
      while (start_len <= Ci
             && start_len <= Cj
             && Ai.Phrases[start_len] is NonTerminal
             && Aj.Phrases[start_len] is NonTerminal
             && Ai.Phrases[start_len].ToString() == Aj.Phrases[start_len].ToString()
        )
      {
        start_len++;
      }

      int end_len = 0; //FindEqualSeq(A1, A2, -1);      
      while (Ci >= start_len
             && Cj >= start_len
             && Ai.Phrases[Ci] is NonTerminal
             && Aj.Phrases[Cj] is NonTerminal
             && Ai.Phrases[Ci].ToString() == Aj.Phrases[Cj].ToString()
        )
      {
        Ci--;
        Cj--;
        end_len++;
      }

      //Теперь у нас
      //start_len=n             end_len=k
      //  B1,...,Bn, D1,...,Dq, E1,...,Ek
      //  B1,...,Bn, C1,...,Cp, E1,...,Ek

      // 2) Анализируем варианты
      swap = (Aj.Phrases.Count < Ai.Phrases.Count); //true Aj короче - ее и удалим
      if (start_len > 0 || end_len > 0)
      {
        if (swap)
        {
          Seqence At = Ai;
          Ai = Aj;
          Aj = At;
        }
        int sub_d_len = Ai.Phrases.Count - start_len - end_len;
        int sub_c_len = Aj.Phrases.Count - start_len - end_len;
        //Выделим D1,...,Dq из Ai
        Seqence sub_d = Ai.SubSequence(start_len, sub_d_len);
        //Выделим C1,...,Cp
        Seqence sub_c = Aj.SubSequence(start_len, sub_c_len);
        if (sub_c != null)
        {
          IPhrase replacement;
          if (sub_d != null)
          {
// Альтернатива (D|C)
            if (sub_c.Count == 1 && sub_c.Phrases[0] is AlternativeSet)
            {
              (sub_c.Phrases[0] as AlternativeSet).Add(sub_d);
              return Aj;
            }
            else if (sub_d.Count == 1 && sub_d.Phrases[0] is AlternativeSet)
            {
              (sub_d.Phrases[0] as AlternativeSet).Add(sub_c);
              swap = true;
              return Ai;
            }
            else
            {
              replacement = new AlternativeSet(mGrammar, sub_d);
              (replacement as AlternativeSet).Phrases.Add(sub_c);
            }
          }
          else
          {
// Необязательная C?
            replacement = new QuantifiedPhrase(mGrammar, sub_c, 0, 1);
          }
          return ReplaceSubSequence(Aj, start_len, sub_c_len, replacement);
        }
        else
          throw new Exception(
            "FindNonMandatryInclusion. Подпоследовательность С нулевая. Такое может быть только если среди альтернатив две одинаковые последовательности, например S = A,B | A,B ");
      } //иначе вообще нет совпадений
      return null;
    }

    private Seqence ReplaceSubSequence(Seqence source, int start_ind, int remove_len, IPhrase replacement)
    {
      Seqence result = new Seqence(mGrammar);
      int i;
      for (i = 0; i < start_ind; i++)
      {
        result.Phrases.Add(source.Phrases[i]);
      }
      result.Phrases.Add(replacement);
      for (i += remove_len; i < source.Phrases.Count; i++)
      {
        result.Phrases.Add(source.Phrases[i]);
      }
      return result;
    }

    private class AltVisitor : WalkVisitorBase
    {
      private readonly ToQuantifiers mMainObject;

      public AltVisitor(ToQuantifiers aMainObject)
      {
        if (aMainObject == null) throw new ArgumentNullException();
        mMainObject = aMainObject;
      }

      public override IDerivation Visit(AlternativeSet aAlternativeSet, DerivationContext aContext)
      {
        if (!mMainObject.GeneralizedNonMandatryInclusion(aAlternativeSet))
        {
          return base.Visit(aAlternativeSet, aContext);
        }
        return null;
      }
    }
  }
}