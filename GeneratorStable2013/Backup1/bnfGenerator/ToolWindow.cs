 
using System;
using WeifenLuo.WinFormsUI.Docking;

namespace bnfGenerator
{
  public partial class ToolWindow : DockContent
  {
    public ToolWindow()
    {
      InitializeComponent();
    }

    private void autoHideToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DockState = ToggleAutoHideState(DockState);
    }

    private static DockState ToggleAutoHideState(DockState state)
    {
      if (state == DockState.DockLeft)
      {
        return DockState.DockLeftAutoHide;
      }
      else if (state == DockState.DockRight)
      {
        return DockState.DockRightAutoHide;
      }
      else if (state == DockState.DockTop)
      {
        return DockState.DockTopAutoHide;
      }
      else if (state == DockState.DockBottom)
      {
        return DockState.DockBottomAutoHide;
      }
      else if (state == DockState.DockLeftAutoHide)
      {
        return DockState.DockLeft;
      }
      else if (state == DockState.DockRightAutoHide)
      {
        return DockState.DockRight;
      }
      else if (state == DockState.DockTopAutoHide)
      {
        return DockState.DockTop;
      }
      else if (state == DockState.DockBottomAutoHide)
      {
        return DockState.DockBottom;
      }
      else
      {
        return state;
      }
    }

    private void hideToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DockState = DockState.Hidden;
    }
  }
}