 
using System.Collections.Generic;

namespace GrammarCompiler.Transforms
{
  public class TransformStatistics
  {
    public readonly List<string> AffectedRules = new List<string>();
    public string mLog;

    public override string ToString()
    {
      return string.Format("Преобразовано {0} правил: {1} \r\n Подробный лог:\r\n {2}", AffectedRules.Count,
                           string.Join(", ", AffectedRules.ToArray()), mLog);
    }

    internal void Log(string aMsg)
    {
      mLog += aMsg + "\r\n";
    }

    internal void Log(string rule_before, string rule_after)
    {
      Log(rule_before);
      Log("=>");
      Log(rule_after);
      Log("\r\n");
    }
  }
}