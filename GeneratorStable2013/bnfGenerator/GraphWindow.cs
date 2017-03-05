 
using System;
using System.Windows.Forms;
using VisualStructure;

namespace bnfGenerator
{
  public partial class GraphWindow : ToolWindow
  {
    private readonly GraphView mGraphView;

    public GraphBuilder GraphBuilder
    {
      get { return mGraphView.GraphBuilder; }
    }

    public GraphWindow()
    {
      InitializeComponent();
      mGraphView = new GraphView();
      Controls.Add(mGraphView);
      mGraphView.Dock = DockStyle.Fill;
      Text = "Graph";
    }

    private void GraphView_Load(object sender, EventArgs e)
    {
    }
  }
}