namespace bnfGenerator
{
  partial class DerivationTree
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DerivationTree));
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.SuspendLayout();
      // 
      // treeView1
      // 
      this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView1.Location = new System.Drawing.Point(0, 0);
      this.treeView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.treeView1.Name = "treeView1";
      this.treeView1.Size = new System.Drawing.Size(389, 327);
      this.treeView1.TabIndex = 0;
      // 
      // DerivationTree
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(389, 327);
      this.Controls.Add(this.treeView1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
      this.Name = "DerivationTree";
      this.TabText = "Derivation Tree";
      this.Text = "Derivation Tree";
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.TreeView treeView1;

  }
}