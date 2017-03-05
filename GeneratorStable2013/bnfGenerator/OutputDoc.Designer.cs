

namespace bnfGenerator
{
  partial class OutputDoc
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputDoc));
      this.scintillaControl1 = new Scintilla.ScintillaControl();
      this.SuspendLayout();
      // 
      // scintillaControl1
      // 
      this.scintillaControl1.Configuration = null;
      this.scintillaControl1.ConfigurationLanguage = null;
      this.scintillaControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.scintillaControl1.LegacyConfiguration = null;
      this.scintillaControl1.Location = new System.Drawing.Point(0, 0);
      this.scintillaControl1.Name = "scintillaControl1";
      this.scintillaControl1.Size = new System.Drawing.Size(389, 327);
      this.scintillaControl1.SmartIndentingEnabled = false;
      this.scintillaControl1.TabIndex = 0;
      this.scintillaControl1.Text = "scintillaControl1";
      // 
      // OutputDoc
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(389, 327);
      this.Controls.Add(this.scintillaControl1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
      this.Name = "OutputDoc";
      this.TabText = "OutputDoc";
      this.Text = "OutputDoc";
      this.ResumeLayout(false);

    }

    #endregion

    private Scintilla.ScintillaControl scintillaControl1;

  }
}