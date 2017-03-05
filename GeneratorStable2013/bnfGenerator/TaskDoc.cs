 
namespace bnfGenerator
{
  public partial class TaskDoc : SciEditDoc
  {
    public TaskDoc()
    {
      InitializeComponent();
      TabText = "Task(Config)";
      WorkSubDir = "MyTasks";
      FileExt = "*.xml";
      SciLang = "xml";
    }
  }
}