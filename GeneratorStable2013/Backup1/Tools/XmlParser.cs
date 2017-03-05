 
using System;
using System.Xml;

namespace XmlParsing
{
  public class XmlParser
  {
    public string mConfName;
    public XmlDocument doc = new XmlDocument();
    public XmlNode root;

    public XmlParser(string XmlFileName)
    {
      mConfName = XmlFileName;
      doc.Load(mConfName);
      root = doc.DocumentElement;
    }

    public int SearchInt(string[] xmlPath, string attrTarget)
    {
      int lRes = -1;
      string s = Search(xmlPath, attrTarget);
      if (!string.IsNullOrEmpty(s))
      {
        int.TryParse(s, out lRes);
      }
      return lRes;
    }

    public string Search(string[] xmlPath, string attrTarget)
    {
      string result = "";
      string log = "";
      string path = "/";
      int count = xmlPath.Length;
      int subpath = 0;
      XmlNode nNode;
      path += string.Join("/", xmlPath, 0, count);
      path += "/";
      path += "@" + attrTarget;
      log += "Попытка найти атрибут " + attrTarget + " по пути: " + path + "\n";
      do
      {
        nNode = root.SelectSingleNode(path);
        if (nNode != null)
        {
          result += /*nNode.Name + " = " +*/ nNode.Value;
          log += "Попытка удачна!\n";
          log += result + "\n";
          Console.WriteLine(log);
          return result;
        }
        else
        {
          subpath++;
          count--;
          log += "Попытка неудачна :( \n";
        }
        path = "/Root/";
        if (count != 0)
        {
          path += string.Join("/", xmlPath, subpath, count);
          path += "/";
          path += "@" + attrTarget;
        }
        else
        {
          path = "/Root/@" + attrTarget;
        }
        log += "Попытка найти атрибут " + attrTarget + "по пути: " + path + "\n";
      } while (xmlPath.Length != subpath && result == "");
      nNode = root.SelectSingleNode(path);
      if (nNode != null)
      {
        result += nNode.Name + " = " + nNode.Value;
        log += "Попытка удачна!\n";
        log += result + "\n";
        Console.WriteLine(log);
        return result;
      }
      else
      {
        log += "Попытка неудачна :( \n В документе нет атрибута " + attrTarget + "\n";
        result = null; //"undefined";
      }
      Console.WriteLine(log);
      return result;
    }

    public string SearchAtRootOnly(string xmlElement, string attrTarget)
    {
      string result = "";
      string path = "/";
      XmlNode nNode;
      path += "Root/" + xmlElement;
      path += "/";
      path += "@" + attrTarget;
      nNode = root.SelectSingleNode(path);
      if (nNode != null)
      {
        result += /*nNode.Name + " = " +*/ nNode.Value;
      }
      return result;
    }
  }
}