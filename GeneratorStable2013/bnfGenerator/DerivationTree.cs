 
using System.Windows.Forms;

namespace bnfGenerator
{
  public partial class DerivationTree : ToolWindow
  {
    public DerivationTree()
    {
      InitializeComponent();
    }

    internal void SetTree(TreeNode aTreeNode)
    {
      treeView1.Nodes.Clear();
      treeView1.Nodes.Add(aTreeNode);
      treeView1.ExpandAll();
    }
  }
}