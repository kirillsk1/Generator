 
using System;
namespace GrammarCompiler.ResultSaver
{
  /// <summary>
  /// Сохраняет сгенерированный текст
  /// 
  /// Предназначен для сохранения программ в отдельные файлы (метод Save)
  /// А также ведет лог сообщений
  /// </summary>
  public interface IResultSaver
  {
    /// <summary>
    /// Сброс. Очищает лог
    /// </summary>
    void Reset();
    /// <summary>
    /// Обрабатывет и сохраняет сгенерированный текст
    /// </summary>
    /// <param name="aText">текст</param>
    void Save(string aText);
    /// <summary>
    /// Добавляет сообщение в лог (и возврат строки).
    /// </summary>
    /// <param name="aText">сообщение</param>
    void MessageToLog(string aText);
    //возвращает готовый лог
    string GetFinalLog();
    /// <summary>
    /// Вызывается в методе Save перед сохранением текста. Обработчик этого события может изменть (отформатировать и т.д.) текст или отменить его сохранение.
    /// </summary>
    event EventHandler<ProcessTextEventArgs> ProcessText;

    /// <summary>
    /// Количество сохраненных текстов (отбракованные MinGW не учитываются)
    /// </summary>
    int SavedCount { get; }

    /// <summary>
    /// Сколько текстов ПОДРЯД было отвергнуто компилятором
    /// </summary>
    int BadResults { get; }
    
    /// <summary>
    /// Сколько текстов ВСЕГО было отвергнуто компилятором
    /// </summary>
    int TotalBadResults { get; }    
  }
}