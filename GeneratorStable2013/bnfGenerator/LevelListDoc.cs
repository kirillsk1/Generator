using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrammarCompiler;

namespace bnfGenerator
{
  public partial class LevelListDoc : ToolWindow
  {
    public OutputDoc OutputDoc;

    public LevelListDoc()
    {
      InitializeComponent();
      Text = "LevelList";
    }

    //public void AddError(GrammarSyntaxException aException)
    //{
    //  ListViewItem lItem = new ListViewItem(new string[]
    //                                          {
    //                                            listView1.Items.Count.ToString(),
    //                                            aException.Message,
    //                                            (aException.Line == -1) ? "" : aException.Line.ToString(),
    //                                            (aException.Col == -1) ? "" : aException.Col.ToString(),
    //                                          });
    //  //lItem.Text = listView1.Items.Count.ToString();
    //  listView1.Items.Add(lItem);
    //}

    public void Add(string text, object obj)
    {
      listView1.Items.Insert(0, text).Tag = obj;
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listView1.SelectedItems.Count > 0)
      {
        ListDerivation der = listView1.SelectedItems[0].Tag as ListDerivation;
        OutputDoc.SetText(Formatter.Format(der.ToString()));
      }
    }

    public void Clear()
    {
      listView1.Clear();
    }
  }
}
