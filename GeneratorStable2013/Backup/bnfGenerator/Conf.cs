 
using System;
using System.Configuration;

namespace bnfGenerator
{
  internal class Conf
  {
    /// <summary>
    /// Читает целое из файла параметров, если его нет, возвращает зн. по умолчанию.
    /// </summary>
    /// <param name="?"></param>
    public static int GetIntDef(string aParamName, int aDefault)
    {
      int lRes = aDefault;
      if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings[aParamName]))
      {
        Int32.TryParse(ConfigurationSettings.AppSettings[aParamName], out lRes);
      }
      return lRes;
    }
  }
}