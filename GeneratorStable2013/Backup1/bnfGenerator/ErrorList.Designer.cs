namespace bnfGenerator
{
  partial class ErrorList
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorList));
      this.listView1 = new System.Windows.Forms.ListView();
      this.No = new System.Windows.Forms.ColumnHeader();
      this.Description = new System.Windows.Forms.ColumnHeader();
      this.Line = new System.Windows.Forms.ColumnHeader();
      this.Col = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // listView1
      // 
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.No,
            this.Description,
            this.Line,
            this.Col});
      this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listView1.Location = new System.Drawing.Point(0, 0);
      this.listView1.Name = "listView1";
      this.listView1.ShowItemToolTips = true;
      this.listView1.Size = new System.Drawing.Size(292, 266);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
      // 
      // No
      // 
      this.No.Text = "No";
      this.No.Width = 39;
      // 
      // Description
      // 
      this.Description.Text = "Description";
      this.Description.Width = 225;
      // 
      // Line
      // 
      this.Line.Text = "Line";
      this.Line.Width = 35;
      // 
      // Col
      // 
      this.Col.Text = "Col";
      this.Col.Width = 30;
      // 
      // ErrorList
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 266);
      this.Controls.Add(this.listView1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.Name = "ErrorList";
      this.TabText = "Error List";
      this.Text = "Error List";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ColumnHeader No;
    private System.Windows.Forms.ColumnHeader Description;
    private System.Windows.Forms.ColumnHeader Line;
    private System.Windows.Forms.ColumnHeader Col;

  }
}