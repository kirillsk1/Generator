 
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using bnfGenerator;

namespace PrePostProcessing
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      try
      {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
        DateTime dBegin = DateTime.Now;
        string source = "";
        string blockvar = "";
        //string[] myArgs = new string[4] { "-c-10", "-l-C", "-p-config.xml", "-s-ForTests" };
        HelpText help = new HelpText();
        ArgsParser aParser = new ArgsParser(args);
        Console.Title = "Test Generator";
        if (args.Length == 0)
        {
          Console.WriteLine(help.Hello());
          //Console.WriteLine("Для выхода нажмите любую клавишу");
          Console.SetCursorPosition(0, 0);
          Console.ReadKey();
          return;
        }

        if (!aParser.Prove())
        {
          Console.WriteLine("Устраните пожалуйста ошибки и повторите запуск.");
          Console.WriteLine("Запуск Генератора без параметров приведет к появлению Справки.");
          Console.ReadKey();
          return;
        }
        Console.WriteLine("Параметры заданы корректно.");
        Console.WriteLine("Начинается процесс генерации.");
        string subDirName = DateTime.Now.ToString("hh-mm-ss-dd-MM-yyyy");
        Grammar mGrammar;
        ConfigParser parser = new ConfigParser(aParser.configTest, "validating/members.xml");
        blockvar = parser.blockvars();
        StreamReader reader = new StreamReader(aParser.grammarTest);
        source = reader.ReadToEnd();
        using (StreamWriter writer = new StreamWriter( /*Path.GetTempPath()+*/"tmpGrammar.txt"))
        {
          writer.WriteLine(source);
          writer.WriteLine(blockvar);
          writer.WriteLine(parser.blockstats());
        }
        mGrammar = Grammar.FromTextFile( /*Path.GetTempPath() + */"tmpGrammar.txt");
        mGrammar.mTransductor = new transductor();
        string exMessage;
        string s = "";
        int totalPassed = 0;
        int totalFailed = 0;
        int count_of_ex;
        for (int i = 1; i <= aParser.countTest; i++)
        {
          count_of_ex = 0;
          // Console.WriteLine("deep1");
          do
          {
            exMessage = "";
            try
            {
              s = Alternative.Print(mGrammar.Generate());
              //Console.WriteLine("deep2");
            }
            catch (Exception ex)
            {
              exMessage = ex.Message;
              count_of_ex++;
              //Console.WriteLine("deep");
              if (count_of_ex == 10)
              {
                Console.WriteLine("Генератор сообщает:");
                Console.WriteLine("Слишком большая глубина вложенности. Тест не может быть гарантированно корректным.");
                Console.WriteLine(
                  "Попробуйте настроить конфигурационный файл так, чтобы встречалось меньше вложенных конструкций.");
                Console.ReadKey();
                return;
              }
            }
          } while (exMessage == "Слишком большая глубина вложенности. Тест не может быть гарантированно корректным.");
          replaceHolding sFormat =
            new replaceHolding(s, parser.inds, parser.arrayLength(), parser.IndexCount());
          string tmp;
          tmp = sFormat.PrintText();
          string tmpFileName = /*Path.GetTempPath() + "\\*/ "tmpName.c";
          using (StreamWriter tmpFile = File.CreateText(tmpFileName))
          {
            tmpFile.WriteLine(tmp);
          }
          FileInfo fSize = new FileInfo(tmpFileName);
          int Size = (int) fSize.Length;
          int TimeToExit = 0;
          ProcessStartInfo proc = new ProcessStartInfo("MinGW/bin/gcc.exe", tmpFileName);
          proc.RedirectStandardError = true;
          proc.UseShellExecute = false;
          Process p = Process.Start(proc);
          //p.WaitForExit(10000);
          while (!p.HasExited)
          {
            TimeToExit++;
            //Console.WriteLine(TimeToExit);
          }

          string warningString = p.StandardError.ReadToEnd();
          Console.WriteLine(warningString);
          //p.Dispose();
          if (warningString.Contains("error"))
          {
            Console.WriteLine("failed");
            totalFailed++;
            continue;
          }
          else
          {
            Console.WriteLine("passed");
            totalPassed++;
            if (Size > 50*1024)
            {
              Console.WriteLine("too long");
              continue;
            }
          }
          p.Dispose();
          File.Delete(tmpFileName);
          if (aParser.spathTest != "undefined")
          {
            DirectoryInfo mainDir = new DirectoryInfo(aParser.spathTest);
            if (mainDir.Exists)
            {
              //DirectoryInfo subDir = mainDir.CreateSubdirectory(DateTime.Today.ToString("dd-mM-yyyy"));
              DirectoryInfo subDir = mainDir.CreateSubdirectory(subDirName);
              using (
                StreamWriter curFile =
                  File.CreateText(subDir.ToString() + "\\" + aParser.SetFileName(aParser.countTest, i) +
                                  ".c"))
              {
                curFile.WriteLine(tmp);
                Console.WriteLine("--" + i + "--");
              }
            }
          }
          else
          {
            Console.WriteLine(tmp);
            Console.WriteLine("--" + i + "--");
          }
          //File.Delete(Path.GetTempPath() + "tmpGrammar.txt");
          //File.Delete("tmpGrammar.txt");
          File.Delete("a.exe");
        }
        DateTime dEnd = DateTime.Now;
        TimeSpan total = dEnd - dBegin;
        string totalReport = "На генерацию затрачено ";
        if (total.Hours != 0)
        {
          totalReport += total.Hours + " ч. ";
        }
        if (total.Minutes != 0)
        {
          totalReport += total.Minutes + " мин. ";
        }
        if (total.Seconds != 0)
        {
          totalReport += total.Seconds + " сек. ";
        }
        Console.WriteLine(totalReport);
        Console.WriteLine("Попытка сгенерировать: " + aParser.countTest);
        Console.WriteLine("Из них правильных: " + totalPassed);
        Console.WriteLine("Из них неправильных: " + totalFailed);
        //Console.WriteLine(dBegin.ToLongTimeString());
        //Console.WriteLine(dEnd.ToLongTimeString());
        //Console.WriteLine(total.TotalSeconds);
        //Console.WriteLine(total.Seconds);
        Console.WriteLine("Генерация тестов завершена.");
        Console.WriteLine("Для выхода нажмите любую клавишу...");
        Console.ReadKey();
      }
      catch (Exception myEx)
      {
        Console.WriteLine("Ошибка генерации. Проверьте корректность входных данных.");
        Console.WriteLine("Для выхода нажмите любую клавишу...");
        Console.WriteLine("Генератор сообщает:");
        Console.WriteLine(myEx.Message);
        Console.ReadKey();
        return;
      }
    }

    public static string WorkingDir
    {
      get
      {
        string mWorkingDir = Directory.GetCurrentDirectory();
        if (mWorkingDir.EndsWith(@"\bin\Debug"))
        {
          mWorkingDir = mWorkingDir.Replace(@"\bin\Debug", "");
          mWorkingDir = Path.GetDirectoryName(mWorkingDir);
        }
        return mWorkingDir;
      }
    }
  }
}