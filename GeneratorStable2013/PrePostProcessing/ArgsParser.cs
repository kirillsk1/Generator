 
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace PrePostProcessing
{
  public class ArgsParser
  {
    public int countTest = int.MinValue;
    public string configTest = "undefined";
    public string langTest = "undefined";
    public string spathTest = "undefined";
    public string grammarTest = "undefined";
    public static int XmlValidity = 0;

    public ArgsParser(string[] args)
    {
      string res;
      foreach (string arg in args)
      {
        res = "";
        string getArg = arg + "      ";
        string key = arg.Substring(0, 3);
        switch (key)
        {
          case "-c-":
            res = getArg.Substring(3, getArg.Length - 3);
            res = res.Trim();
            try
            {
              countTest = Convert.ToInt32(res);
            }
            catch
            {
              countTest = int.MaxValue;
            }
            //if (countTest == 0 && countTest > 5000)
            //{
            //    countTest = 0;
            //}
            break;
          case "-l-":
            res = getArg.Substring(3, getArg.Length - 3);
            res = res.Trim();
            langTest = res;
            break;
          case "-p-":
            res = getArg.Substring(3, getArg.Length - 3);
            res = res.Trim();
            configTest = res;
            break;
          case "-s-":
            res = getArg.Substring(3, getArg.Length - 3);
            res = res.Trim();
            spathTest = res;
            break;
        }
      }
    }

    public bool Prove()
    {
      bool cheked = true;
      Console.WriteLine("Проверка параметров:");
      //Console.WriteLine(Path.GetTempPath());
      if (countTest == int.MaxValue)
      {
        Console.WriteLine("Параметр с ключом -c- (количество тестов) задан неверно.");
        cheked = false;
      }
      if (countTest == int.MinValue)
      {
        Console.WriteLine("Не задан обязательный параметр: количество тестов");
        countTest = 0;
        cheked = false;
      }
      if (countTest <= 0 || countTest > 5000 && countTest != int.MinValue)
      {
        Console.WriteLine("Количество тестов " + countTest + " должно быть больше ноля и меньше 5000");
        cheked = false;
      }
      if (langTest == "undefined")
      {
        Console.WriteLine("Не задан обязательный параметр: язык генерации");
        cheked = false;
      }
      if (configTest == "undefined")
      {
        Console.WriteLine("Не задан обязательный параметр: имя файла конфигурации");
      }
      if (!File.Exists(configTest))
      {
        Console.WriteLine("Не найден файл конфигурации " + configTest);
        cheked = false;
      }
      else
      {
        if (!File.Exists("validating/config.xsd"))
        {
          Console.WriteLine("Не найден необходимый файл config.xsd");
          cheked = false;
        }
        else
        {
          if (XmlValid("config.xml", "urn:config-schema", "validating/config.xsd") == 0)
          {
            Console.WriteLine(
              "Файл конфигурации config.xml поврежден или отредактирован с ошибками");
            cheked = false;
          }
        }
      }

      grammarTest = "grammars/grammar_" + langTest + ".txt";
      if (!File.Exists(grammarTest))
      {
        Console.WriteLine("Не найден файл грамматики " + grammarTest);
        Console.WriteLine("Возможно, задан неподдерживаемый язык генерации или файл грамматик был поврежден");
        cheked = false;
      }
      if (!File.Exists("validating/members.xml"))
      {
        Console.WriteLine("Не найден файл поддерживаемых элементов members.xml");
        cheked = false;
      }
      else
      {
        if (!File.Exists("validating/members.xsd"))
        {
          Console.WriteLine("Не найден необходимый файл members.xsd");
          cheked = false;
        }
        else
        {
          if (XmlValid("validating/members.xml", "urn:members-schema", "validating/members.xsd") == 0)
          {
            Console.WriteLine(
              "Файл поддерживаемых элементов members.xml поврежден или отредактирован с ошибками");
            cheked = false;
          }
        }
      }
      if (spathTest != "undefined")
      {
        if (!Directory.Exists(spathTest))
        {
          Console.WriteLine("Каталог " + spathTest + " для сохранения тестов не найден");
          cheked = false;
        }
      }
      return cheked;
    }

    public string SetFileName(int count, int i)
    {
      int cLength = count.ToString().Length;
      int iLength = i.ToString().Length;
      string name = "";
      for (int j = 0; j < cLength - iLength; j++)
      {
        name += "0";
      }
      return name += i.ToString();
    }

    private int XmlValid(string XmlName, string XmlNS, string XmlXsd)
    {
      // Create the XmlSchemaSet class.
      int errValid = 0;
      XmlSchemaSet sc = new XmlSchemaSet();
      StreamReader readerTxt = new StreamReader(XmlName);
      //string[] tmp = new string[255];
      ArrayList tmp = new ArrayList();
      int tmpI = 0;
      while (!readerTxt.EndOfStream)
      {
        tmp.Add(readerTxt.ReadLine());
      }
      string LeftSide = "";
      string rightSide = "";
      string middle = "xmlns=\"" + XmlNS + "\"";
      string totalRep = "";
      string ReplaceStr = tmp[1].ToString();
      char[] replaceChars = ReplaceStr.ToCharArray();
      char sp = char.Parse(" ");
      char rt = char.Parse(">");

      int j = 0;
      while (replaceChars[j] != sp && replaceChars[j] != rt)
      {
        LeftSide += replaceChars[j];
        j++;
      }
      while (j < replaceChars.Length)
      {
        rightSide += replaceChars[j];
        j++;
      }
      totalRep += LeftSide + " " + middle + rightSide;
      using (StreamWriter writer = new StreamWriter(Path.GetTempPath() + "tmpXML.xml"))
      {
        writer.WriteLine(tmp[0].ToString());
        writer.WriteLine(totalRep);
        tmpI = 2;
        while (tmpI < tmp.Count)
        {
          writer.WriteLine(tmp[tmpI].ToString());
          //tmp[tmpI] = readerTxt.ReadLine();
          tmpI++;
        }
        writer.Close();
      }

      //root.AppendChild(Attribute )
      //if (root.Attributes[0].Value.ToString() != XmlNS )
      //{
      //    errValid++;
      //    return errValid;
      //}
      //Console.WriteLine(root.Attributes[0].Value);
      // Add the schema to the collection.
      sc.Add(XmlNS, XmlXsd);

      // Set the validation settings.
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.ValidationType = ValidationType.Schema;
      settings.Schemas = sc;
      settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

      // Create the XmlReader object.
      XmlReader reader = XmlReader.Create(Path.GetTempPath() + "tmpXML.xml", settings);

      // Parse the file. 
      while (reader.Read()) ;
      if (XmlValidity != 0)
      {
        XmlValidity = 0;
        errValid++;
        reader.Close();
        File.Delete(Path.GetTempPath() + "tmpXML.xml");
        return errValid;
      }
      else
      {
        XmlValidity = 0;
        errValid = 0;
        reader.Close();
        File.Delete(Path.GetTempPath() + "tmpXML.xml");
        return errValid;
      }
      return errValid;
    }

    private static void ValidationCallBack(object sender, ValidationEventArgs e)
    {
      XmlValidity++;
      //Console.WriteLine("Validation Error: {0}", e.Message);
    }

    //    private string argLines = "";
    //    ArgsParser (string[] args)
    //    {
    //        {
    //            string sCount_of_tests = "error";
    //            int result = 2;

    //            foreach (string arg in args)
    //            {
    //                argLines += arg;
    //                string getArg = arg + "          ";
    //                string key = getArg.Substring(0, 3);
    //                switch (key.ToLower())
    //                {
    //                    case "-c-":
    //                        sCount_of_tests = getArg.Substring(3, getArg.Length - 3);
    //                        sCount_of_tests = sCount_of_tests.Trim();
    //                        break;
    //                    case "-p-":
    //                        paramsPath = getArg.Substring(3, getArg.Length - 3);
    //                        paramsPath = paramsPath.Trim();
    //                        break;
    //                    case "-s-":
    //                        testPath = getArg.Substring(3, getArg.Length - 3);
    //                        testPath = testPath.Trim();
    //                        break;
    //                }
    //            }
    //            try
    //            {
    //                count_of_tests = Convert.ToInt32(sCount_of_tests);
    //            }
    //            catch
    //            {
    //                result = 0;
    //            }
    //            if (result != 0)
    //            {
    //                if (count_of_tests < 1 || count_of_tests > 5000)
    //                {
    //                    result = 1;
    //                }
    //            }
    //            return result;
    //        }
    //    }
  }
}