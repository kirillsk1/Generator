using System;
namespace GrammarCompiler
{
  public class GenerateProgressEventArgs : EventArgs
  {
    public string RootText;
    public string CurrentListText;
    public int CurrentListLevel;
    public int CurrentInRoot;
    public int TotalInRoot;
    public int CurrentInCurrent;
    public int MaxLevel;
    /// <summary>
    /// Если не null, значит генератор остановился на паузе на этом элементе
    /// можно back trace
    /// </summary>
    public ListDerivation PausedAtList;
  }
}