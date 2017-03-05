 

namespace bnfGenerator
{
  public partial class OutputDoc : ToolWindow
  {
    public OutputDoc()
    {
      InitializeComponent();
      scintillaControl1.Configuration = SciEditDoc.ScintillaConfig;
      scintillaControl1.ConfigurationLanguage = "cpp";
    }

    internal void SetText(string s)
    {
      scintillaControl1.Text = s;
    }
  }
}