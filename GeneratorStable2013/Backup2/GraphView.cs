 
using System;
using System.Windows.Forms;
using Microsoft.Glee.GraphViewerGdi;

namespace VisualStructure
{
  internal delegate void SimpleDeligate();

  public partial class GraphView : UserControl
  {
    private GViewer gViewer;

    public GraphView()
    {
      InitializeComponent();
      gViewer = new GViewer();
      Controls.Add(gViewer);
      gViewer.Dock = DockStyle.Fill;

      mGraphBuilder = new GraphBuilder();
      mGraphBuilder.Updated += mGraphBuilder_Updated;
    }


    private void mGraphBuilder_Updated(object sender, EventArgs e)
    {
      try
      {
        Invoke(new SimpleDeligate(UpdateView));
      }
      catch (Exception ex)
      {
      }
    }

    private void UpdateView()
    {
      gViewer.Graph = mGraphBuilder.g;
    }

    private GraphBuilder mGraphBuilder;

    public GraphBuilder GraphBuilder
    {
      get { return mGraphBuilder; }
    }
  }
}