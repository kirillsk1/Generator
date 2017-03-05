 
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;

namespace PrePostProcessing
{
  public class ConfigParser
  {
    public XmlDocument docMembers = new XmlDocument();
    public XmlNode rootMembers;
    public XmlDocument docConfig = new XmlDocument();
    public XmlNode rootConfig;
    public ArrayList inds = new ArrayList();

    public ConfigParser(string XmlConfName, string XmlMemberName)
    {
      docConfig.Load(XmlConfName);
      rootConfig = docConfig.DocumentElement;
      docMembers.Load(XmlMemberName);
      rootMembers = docMembers.DocumentElement;
    }

    public ArrayList SearchAttribute(string xmlPath, string attrTarget, string mode)
    {
      ArrayList result = new ArrayList();
      XmlNodeList nList;
      xmlPath += "/@" + attrTarget;
      if (mode == "config")
      {
        nList = rootConfig.SelectNodes(xmlPath);
      }
      else
      {
        nList = rootMembers.SelectNodes(xmlPath);
      }
      if (nList.Count != 0)
      {
        result.Add(nList[0].Name);
        result.Add(nList[0].Value);
      }
      else
      {
        result.Add("");
        result.Add("0");
      }
      return result;
    }

    public int arrayLength()
    {
      XmlNode arrayNode = rootConfig.SelectSingleNode("/program/params/@lengthArray");
      int res = 0;
      res = Convert.ToInt16(arrayNode.Value);
      return res;
    }

    public int IndexCount()
    {
      XmlNode arrayNode = rootConfig.SelectSingleNode("/program/params/@intIndex");
      int res = 0;
      res = Convert.ToInt16(arrayNode.Value);
      return res;
    }

    public XmlNodeList arrayChildren(string xmlPath, string mode)
    {
      XmlNodeList childNodes;
      if (mode == "config")
      {
        childNodes = rootConfig.SelectSingleNode(xmlPath).ChildNodes;
      }
      else
      {
        childNodes = rootMembers.SelectSingleNode(xmlPath).ChildNodes;
      }
      return childNodes;
    }

    public string blockvars()
    {
      string xmlPath = "/program/params";
      Regex re = new Regex(@"Array");
      XmlNodeList members = arrayChildren("/supportedItems/C/definitions", "members");
      ArrayList item = new ArrayList();
      ArrayList type = new ArrayList();
      string blockvar = "blockvars := ";
      string definition = "";
      string variables = "";
      string varList = "variables := ";
      int nameCount = 0;
      foreach (XmlNode member in members)
      {
        string def = "";
        string var = "";
        nameCount = 0;
        item = SearchAttribute(xmlPath, member.Name, "config");
        if (item[0].ToString() != "" && item[1].ToString() != "0")
        {
          //blockvar += "def_" + item[0] + " , ";
          ArrayList defType = SearchAttribute("/supportedItems/C/definitions/" + item[0].ToString(), "type", "members");
          if (re.IsMatch(item[0].ToString()))
          {
            for (int i = 0; i < Convert.ToInt16(item[1]); i++)
            {
              //def += defType[1] + " " + item[0] + "_" + nameCount + "[" + arrayLength() + "];, ";
              var += item[0] + "_" + nameCount + "[, indExpr ,] | ";
              nameCount++;
            }
            //def = def.Substring(0, def.Length - 2);
            var = var.Substring(0, var.Length - 2);
            definition += "def_" + item[0] + " := " + "2 " + "\r\n";
            variables += item[0] + " := " + var + "\r\n";
            varList += item[0] + " | ";
          }
          else
          {
            for (int i = 0; i < Convert.ToInt16(item[1]); i++)
            {
              //def += defType[1] + " " + item[0] + "_" + nameCount + ";, ";
              var += item[0] + "_" + nameCount + " | ";
              inds.Add(item[0] + "_" + nameCount);
              nameCount++;
            }
            //def = def.Substring(0, def.Length - 2);
            var = var.Substring(0, var.Length - 2);
            definition += "def_" + item[0] + " := " + "1 " + "\r\n";
            variables += item[0] + " := " + var + "\r\n";
          }
        }
      }
      varList = varList.Substring(0, varList.Length - 2);
      //blockvar = blockvar.Substring(0, blockvar.Length-2);
      blockvar += "replaceVars";
      return blockvar + "\r\n" + definition + variables + "\r\n" + varList + "\r\n";
    }

    public string blockstats()
    {
      string blockstat = "bodyStat := ";
      string XmlPath = "/program/body";
      ArrayList item;
      XmlNodeList members = arrayChildren("/supportedItems/C/statements", "members");
      foreach (XmlNode member in members)
      {
        item = SearchAttribute(XmlPath, member.Name, "config");
        if (item != null && item[1].ToString() != "0")
        {
          blockstat += item[0] + " | ";
        }
      }
      blockstat = blockstat.Substring(0, blockstat.Length - 2) + "\r\n";
      XmlPath = "/program/";
      XmlNodeList bodyMembers = arrayChildren("/program", "config");
      foreach (XmlNode member in members)
      {
        bool foundNode = false;
        string NodeName = "";
        string stat = "";
        for (int i = 0; i < bodyMembers.Count; i++)
        {
          if (member.Name == bodyMembers[i].Name)
          {
            foundNode = true;
            NodeName = member.Name.ToString();
            break;
          }
        }
        if (foundNode)
        {
          foreach (XmlNode mem in members)
          {
            item = SearchAttribute(XmlPath + NodeName, mem.Name, "config");
            if (item != null && item[1].ToString() != "0")
            {
              stat += item[0] + " | ";
            }
          }
          stat = stat.Substring(0, stat.Length - 2);
          blockstat += NodeName + NodeName + " := " + stat + "\r\n";
        }
      }
      blockstat = blockstat.Substring(0, blockstat.Length - 2);
      return blockstat;
    }
  }
}