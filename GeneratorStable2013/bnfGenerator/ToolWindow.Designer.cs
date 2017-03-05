namespace bnfGenerator
{
  partial class ToolWindow
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.autoHideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoHideToolStripMenuItem,
            this.hideToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
      // 
      // hideToolStripMenuItem
      // 
      this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
      this.hideToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.hideToolStripMenuItem.Text = "Hide";
      this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
      // 
      // autoHideToolStripMenuItem
      // 
      this.autoHideToolStripMenuItem.Name = "autoHideToolStripMenuItem";
      this.autoHideToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.autoHideToolStripMenuItem.Text = "Auto Hide";
      this.autoHideToolStripMenuItem.Click += new System.EventHandler(this.autoHideToolStripMenuItem_Click);
      // 
      // ToolWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 266);
      this.Name = "ToolWindow";
      this.TabPageContextMenuStrip = this.contextMenuStrip1;
      this.TabText = "ToolWindow";
      this.Text = "ToolWindow";
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem autoHideToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
  }
}