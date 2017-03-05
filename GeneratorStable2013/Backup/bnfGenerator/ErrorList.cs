 
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GrammarCompiler;

namespace bnfGenerator
{
  public partial class ErrorList : ToolWindow
  {
    private List<GrammarSyntaxException> mErrors;
    private readonly GrammarDoc mGrammarDoc;

    public ErrorList()
    {
      InitializeComponent();
    }

    public ErrorList(GrammarDoc aGrammarDoc) : this()
    {
      mGrammarDoc = aGrammarDoc;
      mGrammarDoc.ErrorListControl = this;
    }

    public List<GrammarSyntaxException> Errors
    {
      get { return mErrors; }
      set
      {
        mErrors = value;
        ReBind();
      }
    }

    private void ReBind()
    {
      listView1.Items.Clear();
      foreach (GrammarSyntaxException lError in mErrors)
      {
        AddError(lError);
      }
    }

    public void AddError(GrammarSyntaxException aException)
    {
      ListViewItem lItem = new ListViewItem(new string[]
                                              {
                                                listView1.Items.Count.ToString(),
                                                aException.Message,
                                                (aException.Line == -1) ? "" : aException.Line.ToString(),
                                                (aException.Col == -1) ? "" : aException.Col.ToString(),
                                              });
      //lItem.Text = listView1.Items.Count.ToString();
      listView1.Items.Add(lItem);
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (mGrammarDoc != null && listView1.SelectedItems.Count > 0)
      {
        ListViewItem lSel = listView1.SelectedItems[0];
        string lLine = lSel.SubItems[2].Text;
        string lCol = lSel.SubItems[3].Text;
        int ln, col;
        if (int.TryParse(lLine, out ln) && int.TryParse(lCol, out col))
        {
          mGrammarDoc.MarkError(ln, col);
        }
      }
    }
  }
}