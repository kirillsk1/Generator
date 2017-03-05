namespace bnfGenerator
{
  partial class MainForm
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
      this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.changeWorkingDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.grammarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addNewGrammarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.useToQuantifiersTransformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.grammarIndexingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.grammarOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.generateBySetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuStop = new System.Windows.Forms.ToolStripMenuItem();
      this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.savePositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
      this.lexerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.recursiveTopDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.iterativeTopDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.iterativeLeftRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tbRun = new System.Windows.Forms.ToolStripButton();
      this.tbGenOneProg = new System.Windows.Forms.ToolStripButton();
      this.tbGenStep = new System.Windows.Forms.ToolStripButton();
      this.btnPause = new System.Windows.Forms.ToolStripButton();
      this.toolStrip2 = new System.Windows.Forms.ToolStrip();
      this.lblStatTest = new System.Windows.Forms.ToolStripLabel();
      this.lblStatCount = new System.Windows.Forms.ToolStripLabel();
      this.lblStatCur = new System.Windows.Forms.ToolStripLabel();
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.menuStrip1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.toolStrip2.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // dockPanel1
      // 
      this.dockPanel1.ActiveAutoHideContent = null;
      this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dockPanel1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
      this.dockPanel1.Location = new System.Drawing.Point(0, 0);
      this.dockPanel1.Margin = new System.Windows.Forms.Padding(3, 50, 3, 10);
      this.dockPanel1.Name = "dockPanel1";
      this.dockPanel1.Size = new System.Drawing.Size(514, 217);
      this.dockPanel1.TabIndex = 0;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.grammarsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuStop,
            this.windowToolStripMenuItem,
            this.toolStripMenuItem2,
            this.helpToolStripMenuItem,
            this.modeToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(514, 24);
      this.menuStrip1.TabIndex = 3;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeWorkingDirectoryToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // changeWorkingDirectoryToolStripMenuItem
      // 
      this.changeWorkingDirectoryToolStripMenuItem.Name = "changeWorkingDirectoryToolStripMenuItem";
      this.changeWorkingDirectoryToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
      this.changeWorkingDirectoryToolStripMenuItem.Text = "Change working directory...";
      // 
      // grammarsToolStripMenuItem
      // 
      this.grammarsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewGrammarToolStripMenuItem,
            this.useToQuantifiersTransformToolStripMenuItem});
      this.grammarsToolStripMenuItem.Name = "grammarsToolStripMenuItem";
      this.grammarsToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
      this.grammarsToolStripMenuItem.Text = "Grammars";
      // 
      // addNewGrammarToolStripMenuItem
      // 
      this.addNewGrammarToolStripMenuItem.Name = "addNewGrammarToolStripMenuItem";
      this.addNewGrammarToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
      this.addNewGrammarToolStripMenuItem.Text = "Add new grammar";
      // 
      // useToQuantifiersTransformToolStripMenuItem
      // 
      this.useToQuantifiersTransformToolStripMenuItem.Checked = global::bnfGenerator.Properties.Settings.Default.UseToQuantTransform;
      this.useToQuantifiersTransformToolStripMenuItem.Name = "useToQuantifiersTransformToolStripMenuItem";
      this.useToQuantifiersTransformToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
      this.useToQuantifiersTransformToolStripMenuItem.Text = "Use ToQuantifiers Transform";
      this.useToQuantifiersTransformToolStripMenuItem.Click += new System.EventHandler(this.useToQuantifiersTransformToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grammarIndexingToolStripMenuItem,
            this.grammarOutputToolStripMenuItem,
            this.generateBySetToolStripMenuItem});
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(64, 20);
      this.toolStripMenuItem1.Text = "Generate";
      // 
      // grammarIndexingToolStripMenuItem
      // 
      this.grammarIndexingToolStripMenuItem.Name = "grammarIndexingToolStripMenuItem";
      this.grammarIndexingToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
      this.grammarIndexingToolStripMenuItem.Text = "Grammar Indexing";
      this.grammarIndexingToolStripMenuItem.Click += new System.EventHandler(this.grammarIndexingToolStripMenuItem_Click);
      // 
      // grammarOutputToolStripMenuItem
      // 
      this.grammarOutputToolStripMenuItem.Name = "grammarOutputToolStripMenuItem";
      this.grammarOutputToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
      this.grammarOutputToolStripMenuItem.Text = "Grammar Output";
      this.grammarOutputToolStripMenuItem.Click += new System.EventHandler(this.grammarOutputToolStripMenuItem_Click);
      // 
      // generateBySetToolStripMenuItem
      // 
      this.generateBySetToolStripMenuItem.Name = "generateBySetToolStripMenuItem";
      this.generateBySetToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
      this.generateBySetToolStripMenuItem.Text = "Generate by Set";
      this.generateBySetToolStripMenuItem.Click += new System.EventHandler(this.generateBySetToolStripMenuItem_Click);
      // 
      // toolStripMenuStop
      // 
      this.toolStripMenuStop.Name = "toolStripMenuStop";
      this.toolStripMenuStop.Size = new System.Drawing.Size(41, 20);
      this.toolStripMenuStop.Text = "Stop";
      this.toolStripMenuStop.Click += new System.EventHandler(this.Stop_Click);
      // 
      // windowToolStripMenuItem
      // 
      this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.savePositionToolStripMenuItem});
      this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
      this.windowToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
      this.windowToolStripMenuItem.Text = "Window";
      // 
      // savePositionToolStripMenuItem
      // 
      this.savePositionToolStripMenuItem.Name = "savePositionToolStripMenuItem";
      this.savePositionToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
      this.savePositionToolStripMenuItem.Text = "Save position";
      this.savePositionToolStripMenuItem.Click += new System.EventHandler(this.savePositionToolStripMenuItem_Click);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lexerToolStripMenuItem});
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(45, 20);
      this.toolStripMenuItem2.Text = "Tests";
      // 
      // lexerToolStripMenuItem
      // 
      this.lexerToolStripMenuItem.Name = "lexerToolStripMenuItem";
      this.lexerToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
      this.lexerToolStripMenuItem.Text = "Lexer";
      this.lexerToolStripMenuItem.Click += new System.EventHandler(this.lexerToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // modeToolStripMenuItem
      // 
      this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recursiveTopDownToolStripMenuItem,
            this.iterativeTopDownToolStripMenuItem,
            this.iterativeLeftRightToolStripMenuItem});
      this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
      this.modeToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
      this.modeToolStripMenuItem.Text = "Mode";
      // 
      // recursiveTopDownToolStripMenuItem
      // 
      this.recursiveTopDownToolStripMenuItem.CheckOnClick = true;
      this.recursiveTopDownToolStripMenuItem.Name = "recursiveTopDownToolStripMenuItem";
      this.recursiveTopDownToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.recursiveTopDownToolStripMenuItem.Text = "Recursive Top-Down";
      this.recursiveTopDownToolStripMenuItem.Click += new System.EventHandler(this.recursiveTopDownToolStripMenuItem_Click);
      // 
      // iterativeTopDownToolStripMenuItem
      // 
      this.iterativeTopDownToolStripMenuItem.AutoToolTip = true;
      this.iterativeTopDownToolStripMenuItem.CheckOnClick = true;
      this.iterativeTopDownToolStripMenuItem.Name = "iterativeTopDownToolStripMenuItem";
      this.iterativeTopDownToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.iterativeTopDownToolStripMenuItem.Text = "Iterative Top-Down";
      this.iterativeTopDownToolStripMenuItem.Click += new System.EventHandler(this.iterativeTopDownToolStripMenuItem_Click);
      // 
      // iterativeLeftRightToolStripMenuItem
      // 
      this.iterativeLeftRightToolStripMenuItem.CheckOnClick = true;
      this.iterativeLeftRightToolStripMenuItem.Name = "iterativeLeftRightToolStripMenuItem";
      this.iterativeLeftRightToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.iterativeLeftRightToolStripMenuItem.Text = "Iterative Left-Right";
      this.iterativeLeftRightToolStripMenuItem.Click += new System.EventHandler(this.iterativeLeftRightToolStripMenuItem_Click);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbRun,
            this.tbGenOneProg,
            this.tbGenStep,
            this.btnPause});
      this.toolStrip1.Location = new System.Drawing.Point(3, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(137, 25);
      this.toolStrip1.TabIndex = 5;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // tbRun
      // 
      this.tbRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.tbRun.Image = global::bnfGenerator.Properties.Resources.imgRun;
      this.tbRun.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tbRun.Name = "tbRun";
      this.tbRun.Size = new System.Drawing.Size(23, 22);
      this.tbRun.Text = "Generate All";
      this.tbRun.Click += new System.EventHandler(this.grammarOutputToolStripMenuItem_Click);
      // 
      // tbGenOneProg
      // 
      this.tbGenOneProg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.tbGenOneProg.Image = global::bnfGenerator.Properties.Resources.imOver;
      this.tbGenOneProg.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tbGenOneProg.Name = "tbGenOneProg";
      this.tbGenOneProg.Size = new System.Drawing.Size(23, 22);
      this.tbGenOneProg.Text = "Generate One Prog (step over)";
      this.tbGenOneProg.Click += new System.EventHandler(this.tbGenOneProg_Click);
      // 
      // tbGenStep
      // 
      this.tbGenStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.tbGenStep.Image = global::bnfGenerator.Properties.Resources.imInto;
      this.tbGenStep.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tbGenStep.Name = "tbGenStep";
      this.tbGenStep.Size = new System.Drawing.Size(23, 22);
      this.tbGenStep.Text = "Step next derivation";
      this.tbGenStep.Click += new System.EventHandler(this.tbGenStep_Click);
      // 
      // btnPause
      // 
      this.btnPause.Image = global::bnfGenerator.Properties.Resources.imgRun;
      this.btnPause.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnPause.Name = "btnPause";
      this.btnPause.Size = new System.Drawing.Size(56, 22);
      this.btnPause.Text = "Pause";
      this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
      // 
      // toolStrip2
      // 
      this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatTest,
            this.lblStatCount,
            this.lblStatCur});
      this.toolStrip2.Location = new System.Drawing.Point(140, 0);
      this.toolStrip2.Name = "toolStrip2";
      this.toolStrip2.Size = new System.Drawing.Size(89, 25);
      this.toolStrip2.TabIndex = 8;
      this.toolStrip2.Text = "toolStrip2";
      // 
      // lblStatTest
      // 
      this.lblStatTest.Name = "lblStatTest";
      this.lblStatTest.Size = new System.Drawing.Size(53, 22);
      this.lblStatTest.Text = "Test 0 / 0";
      // 
      // lblStatCount
      // 
      this.lblStatCount.Name = "lblStatCount";
      this.lblStatCount.Size = new System.Drawing.Size(13, 22);
      this.lblStatCount.Text = "0";
      // 
      // lblStatCur
      // 
      this.lblStatCur.Name = "lblStatCur";
      this.lblStatCur.Size = new System.Drawing.Size(11, 22);
      this.lblStatCur.Text = "-";
      // 
      // toolStripContainer1
      // 
      // 
      // toolStripContainer1.ContentPanel
      // 
      this.toolStripContainer1.ContentPanel.Controls.Add(this.dockPanel1);
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(514, 217);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.Location = new System.Drawing.Point(0, 24);
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.Size = new System.Drawing.Size(514, 242);
      this.toolStripContainer1.TabIndex = 11;
      this.toolStripContainer1.Text = "toolStripContainer1";
      // 
      // toolStripContainer1.TopToolStripPanel
      // 
      this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
      this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(514, 266);
      this.Controls.Add(this.toolStripContainer1);
      this.Controls.Add(this.menuStrip1);
      this.IsMdiContainer = true;
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "MainForm";
      this.Text = "BNF Generator";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.toolStrip2.ResumeLayout(false);
      this.toolStrip2.PerformLayout();
      this.toolStripContainer1.ContentPanel.ResumeLayout(false);
      this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.TopToolStripPanel.PerformLayout();
      this.toolStripContainer1.ResumeLayout(false);
      this.toolStripContainer1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem changeWorkingDirectoryToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem lexerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuStop;
      private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.ToolStripMenuItem savePositionToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem grammarIndexingToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem grammarOutputToolStripMenuItem;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton tbRun;
    private System.Windows.Forms.ToolStripButton tbGenOneProg;
    private System.Windows.Forms.ToolStripButton tbGenStep;
    private System.Windows.Forms.ToolStripMenuItem grammarsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addNewGrammarToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem useToQuantifiersTransformToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem recursiveTopDownToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem iterativeTopDownToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem iterativeLeftRightToolStripMenuItem;
    private System.Windows.Forms.ToolStrip toolStrip2;
    private System.Windows.Forms.ToolStripLabel lblStatTest;
    private System.Windows.Forms.ToolStripLabel lblStatCount;
    private System.Windows.Forms.ToolStripLabel lblStatCur;
    private System.Windows.Forms.ToolStripButton btnPause;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.ToolStripMenuItem generateBySetToolStripMenuItem;
  }
}