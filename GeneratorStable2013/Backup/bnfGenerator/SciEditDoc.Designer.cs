namespace bnfGenerator
{
  partial class SciEditDoc
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
      this.btnReParse = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.button1 = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.scintillaControl1 = new Scintilla.ScintillaControl();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnReParse
      // 
      this.btnReParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnReParse.Location = new System.Drawing.Point(273, 12);
      this.btnReParse.Margin = new System.Windows.Forms.Padding(4);
      this.btnReParse.Name = "btnReParse";
      this.btnReParse.Size = new System.Drawing.Size(100, 28);
      this.btnReParse.TabIndex = 0;
      this.btnReParse.Text = "Parse again";
      this.btnReParse.UseVisualStyleBackColor = true;
      this.btnReParse.Click += new System.EventHandler(this.btnReParse_Click_1);
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(17, 12);
      this.btnSave.Margin = new System.Windows.Forms.Padding(4);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(100, 28);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.btnSave);
      this.panel1.Controls.Add(this.btnReParse);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 272);
      this.panel1.Margin = new System.Windows.Forms.Padding(4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(389, 55);
      this.panel1.TabIndex = 5;
      // 
      // comboBox1
      // 
      this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(17, 25);
      this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(313, 24);
      this.comboBox1.TabIndex = 0;
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.Location = new System.Drawing.Point(340, 25);
      this.button1.Margin = new System.Windows.Forms.Padding(4);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(33, 28);
      this.button1.TabIndex = 1;
      this.button1.Text = "...";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.button1);
      this.groupBox1.Controls.Add(this.comboBox1);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
      this.groupBox1.Size = new System.Drawing.Size(389, 66);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Select grammar file:";
      // 
      // scintillaControl1
      // 
      this.scintillaControl1.Configuration = null;
      this.scintillaControl1.ConfigurationLanguage = null;
      this.scintillaControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.scintillaControl1.LegacyConfiguration = null;
      this.scintillaControl1.Location = new System.Drawing.Point(0, 66);
      this.scintillaControl1.Name = "scintillaControl1";
      this.scintillaControl1.Size = new System.Drawing.Size(389, 206);
      this.scintillaControl1.SmartIndentingEnabled = false;
      this.scintillaControl1.TabIndex = 6;
      this.scintillaControl1.Text = "scintillaControl1";
      // 
      // SciEditDoc
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(389, 327);
      this.Controls.Add(this.scintillaControl1);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.groupBox1);
      this.Name = "SciEditDoc";
      this.Text = "SciEditDoc";
      this.panel1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnReParse;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.GroupBox groupBox1;
    private Scintilla.ScintillaControl scintillaControl1;

  }
}