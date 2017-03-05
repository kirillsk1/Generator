using System;
namespace GrammarCompiler.ResultSaver
{
  public class ProcessTextEventArgs : EventArgs
  {
    /// <summary>
    /// —генерированный текст который нужно обработать
    /// </summary>
    public string Text;
    /// <summary>
    /// ”казывает, что не надо сохран€ть этот текст (например, когда он не прошел проверку на MinGW)
    /// </summary>
    public bool CancelSave;

    /// <summary>
    /// ќписание ошибок из-за которых обработчик установил CancelSave=true
    /// </summary>
    public string CancelReason;
  }
}