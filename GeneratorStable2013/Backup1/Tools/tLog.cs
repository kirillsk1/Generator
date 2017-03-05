 
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tools
{
  public class TLog
  {
    public static int Indent = 8;

    public static string IndentStr()
    {
      return new string(' ', Indent);
    }

    private static readonly Dictionary<string, TraceSource> mSources = new Dictionary<string, TraceSource>();

    public static TraceSource GetSource(string aSourceName)
    {
      TraceSource lSource;
      if (!mSources.ContainsKey(aSourceName))
      {
        lSource = new TraceSource(aSourceName, SourceLevels.All);
        if (lSource == null) throw new Exception("No Source: " + aSourceName);
        mSources.Add(aSourceName, lSource);
      }
      else
      {
        lSource = mSources[aSourceName];
      }
      return lSource;
    }

    public static void Write(string aTxt)
    {
      //GetSource("Default").TraceInformation(aTxt);
    }

    public static void Write(string aSource, string aTxt)
    {
      //GetSource(aSource).TraceInformation(aTxt);
    }

    public static void Write(string aTxt, params object[] p)
    {
      //GetSource("Default").TraceInformation(aTxt, p);
    }

    public static void Write(string aSource, string aTxt, params object[] p)
    {
      //GetSource(aSource).TraceInformation(aTxt, p);
    }
  }
}