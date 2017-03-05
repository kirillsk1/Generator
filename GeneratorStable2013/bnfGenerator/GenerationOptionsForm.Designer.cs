namespace bnfGenerator
{
    partial class GenerationOptionsForm
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
          this.panel1 = new System.Windows.Forms.Panel();
          this.groupBox3 = new System.Windows.Forms.GroupBox();
          this.label3 = new System.Windows.Forms.Label();
          this.groupBox4 = new System.Windows.Forms.GroupBox();
          this.label2 = new System.Windows.Forms.Label();
          this.btnChooseFolder = new System.Windows.Forms.Button();
          this.label1 = new System.Windows.Forms.Label();
          this.groupBox2 = new System.Windows.Forms.GroupBox();
          this.cbAllEnumInOne = new System.Windows.Forms.CheckBox();
          this.groupBox1 = new System.Windows.Forms.GroupBox();
          this.radioOpPairs = new System.Windows.Forms.RadioButton();
          this.radioOpMinRND = new System.Windows.Forms.RadioButton();
          this.radioOpUniformDistr = new System.Windows.Forms.RadioButton();
          this.radioOpEnum = new System.Windows.Forms.RadioButton();
          this.radioOpNormalDistr = new System.Windows.Forms.RadioButton();
          this.dlgSetFolder = new System.Windows.Forms.FolderBrowserDialog();
          this.cbRestrictLevel = new System.Windows.Forms.CheckBox();
          this.tbRestrictLevel = new System.Windows.Forms.TextBox();
          this.txtLabelDir = new System.Windows.Forms.TextBox();
          this.radioOpLangF = new System.Windows.Forms.RadioButton();
          this.radioOpLangC = new System.Windows.Forms.RadioButton();
          this.txtTotalTests = new System.Windows.Forms.TextBox();
          this.cbTotalTests = new System.Windows.Forms.CheckBox();
          this.txtPathSaveToFile = new System.Windows.Forms.TextBox();
          this.cbSaveToFiles = new System.Windows.Forms.CheckBox();
          this.cbMinGWEnabled = new System.Windows.Forms.CheckBox();
          this.cbPPprocessingEnabled = new System.Windows.Forms.CheckBox();
          this.cbRREnable = new System.Windows.Forms.CheckBox();
          this.panel1.SuspendLayout();
          this.groupBox3.SuspendLayout();
          this.groupBox4.SuspendLayout();
          this.groupBox2.SuspendLayout();
          this.groupBox1.SuspendLayout();
          this.SuspendLayout();
          // 
          // panel1
          // 
          this.panel1.Controls.Add(this.groupBox3);
          this.panel1.Controls.Add(this.groupBox2);
          this.panel1.Controls.Add(this.groupBox1);
          this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panel1.Location = new System.Drawing.Point(0, 0);
          this.panel1.Name = "panel1";
          this.panel1.Size = new System.Drawing.Size(292, 648);
          this.panel1.TabIndex = 0;
          // 
          // groupBox3
          // 
          this.groupBox3.Controls.Add(this.cbRestrictLevel);
          this.groupBox3.Controls.Add(this.tbRestrictLevel);
          this.groupBox3.Controls.Add(this.label3);
          this.groupBox3.Controls.Add(this.txtLabelDir);
          this.groupBox3.Controls.Add(this.groupBox4);
          this.groupBox3.Controls.Add(this.txtTotalTests);
          this.groupBox3.Controls.Add(this.label2);
          this.groupBox3.Controls.Add(this.cbTotalTests);
          this.groupBox3.Controls.Add(this.btnChooseFolder);
          this.groupBox3.Controls.Add(this.label1);
          this.groupBox3.Controls.Add(this.txtPathSaveToFile);
          this.groupBox3.Controls.Add(this.cbSaveToFiles);
          this.groupBox3.Controls.Add(this.cbMinGWEnabled);
          this.groupBox3.Controls.Add(this.cbPPprocessingEnabled);
          this.groupBox3.Location = new System.Drawing.Point(12, 246);
          this.groupBox3.Name = "groupBox3";
          this.groupBox3.Size = new System.Drawing.Size(267, 390);
          this.groupBox3.TabIndex = 2;
          this.groupBox3.TabStop = false;
          this.groupBox3.Text = "Language options";
          // 
          // label3
          // 
          this.label3.AutoSize = true;
          this.label3.Location = new System.Drawing.Point(11, 345);
          this.label3.Name = "label3";
          this.label3.Size = new System.Drawing.Size(91, 13);
          this.label3.TabIndex = 12;
          this.label3.Text = "Directory label:";
          // 
          // groupBox4
          // 
          this.groupBox4.Controls.Add(this.radioOpLangF);
          this.groupBox4.Controls.Add(this.radioOpLangC);
          this.groupBox4.Location = new System.Drawing.Point(11, 67);
          this.groupBox4.Name = "groupBox4";
          this.groupBox4.Size = new System.Drawing.Size(234, 45);
          this.groupBox4.TabIndex = 10;
          this.groupBox4.TabStop = false;
          this.groupBox4.Text = "Language";
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(14, 138);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(103, 13);
          this.label2.TabIndex = 7;
          this.label2.Text = "Tests quantity";
          // 
          // btnChooseFolder
          // 
          this.btnChooseFolder.Location = new System.Drawing.Point(181, 266);
          this.btnChooseFolder.Name = "btnChooseFolder";
          this.btnChooseFolder.Size = new System.Drawing.Size(73, 23);
          this.btnChooseFolder.TabIndex = 5;
          this.btnChooseFolder.Text = "Set directory";
          this.btnChooseFolder.UseVisualStyleBackColor = true;
          this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(14, 219);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(166, 13);
          this.label1.TabIndex = 4;
          this.label1.Text = "Tests saving path";
          // 
          // groupBox2
          // 
          this.groupBox2.Controls.Add(this.cbAllEnumInOne);
          this.groupBox2.Controls.Add(this.cbRREnable);
          this.groupBox2.Location = new System.Drawing.Point(12, 164);
          this.groupBox2.Name = "groupBox2";
          this.groupBox2.Size = new System.Drawing.Size(268, 63);
          this.groupBox2.TabIndex = 1;
          this.groupBox2.TabStop = false;
          this.groupBox2.Text = "Grammar Options";
          // 
          // cbAllEnumInOne
          // 
          this.cbAllEnumInOne.AutoSize = true;
          this.cbAllEnumInOne.Location = new System.Drawing.Point(7, 40);
          this.cbAllEnumInOne.Name = "cbAllEnumInOne";
          this.cbAllEnumInOne.Size = new System.Drawing.Size(93, 17);
          this.cbAllEnumInOne.TabIndex = 1;
          this.cbAllEnumInOne.Text = "AllEnumInOne";
          this.cbAllEnumInOne.UseVisualStyleBackColor = true;
          // 
          // groupBox1
          // 
         // this.groupBox1.Controls.Add(this.radioOpPairs);
          //this.groupBox1.Controls.Add(this.radioOpMinRND);
          this.groupBox1.Controls.Add(this.radioOpUniformDistr);
          this.groupBox1.Controls.Add(this.radioOpEnum);
          this.groupBox1.Controls.Add(this.radioOpNormalDistr);
          this.groupBox1.Location = new System.Drawing.Point(12, 12);
          this.groupBox1.Name = "groupBox1";
          this.groupBox1.Size = new System.Drawing.Size(268, 146);
          this.groupBox1.TabIndex = 0;
          this.groupBox1.TabStop = false;
          this.groupBox1.Text = "Generation Modes";
          // 
          // radioOpPairs
          // 
          this.radioOpPairs.AutoSize = true;
          this.radioOpPairs.Location = new System.Drawing.Point(7, 113);
          this.radioOpPairs.Name = "radioOpPairs";
          this.radioOpPairs.Size = new System.Drawing.Size(129, 17);
          this.radioOpPairs.TabIndex = 2;
          this.radioOpPairs.TabStop = true;
          this.radioOpPairs.Text = "Генерация по парам";
          this.radioOpPairs.UseVisualStyleBackColor = true;
          // 
          // radioOpMinRND
          // 
          this.radioOpMinRND.AutoSize = true;
          this.radioOpMinRND.Location = new System.Drawing.Point(7, 89);
          this.radioOpMinRND.Name = "radioOpMinRND";
          this.radioOpMinRND.Size = new System.Drawing.Size(152, 17);
          this.radioOpMinRND.TabIndex = 1;
          this.radioOpMinRND.TabStop = true;
          this.radioOpMinRND.Text = "Распределение по дугам";
          this.radioOpMinRND.UseVisualStyleBackColor = true;
          // 
          // radioOpUniformDistr
          // 
          this.radioOpUniformDistr.AutoSize = true;
          this.radioOpUniformDistr.Location = new System.Drawing.Point(7, 66);
          this.radioOpUniformDistr.Name = "radioOpUniformDistr";
          this.radioOpUniformDistr.Size = new System.Drawing.Size(186, 17);
          this.radioOpUniformDistr.TabIndex = 0;
          this.radioOpUniformDistr.Text = "Universal distribution";
          this.radioOpUniformDistr.UseVisualStyleBackColor = true;
          // 
          // radioOpEnum
          // 
          this.radioOpEnum.AutoSize = true;
          this.radioOpEnum.Location = new System.Drawing.Point(7, 43);
          this.radioOpEnum.Name = "radioOpEnum";
          this.radioOpEnum.Size = new System.Drawing.Size(69, 17);
          this.radioOpEnum.TabIndex = 0;
          this.radioOpEnum.TabStop = true;
          this.radioOpEnum.Text = "Enumeration";
          this.radioOpEnum.UseVisualStyleBackColor = true;
          // 
          // radioOpNormalDistr
          // 
          this.radioOpNormalDistr.AutoSize = true;
          this.radioOpNormalDistr.Checked = true;
          this.radioOpNormalDistr.Location = new System.Drawing.Point(7, 20);
          this.radioOpNormalDistr.Name = "radioOpNormalDistr";
          this.radioOpNormalDistr.Size = new System.Drawing.Size(170, 17);
          this.radioOpNormalDistr.TabIndex = 0;
          this.radioOpNormalDistr.TabStop = true;
          this.radioOpNormalDistr.Text = "Normal distribution";
          this.radioOpNormalDistr.UseVisualStyleBackColor = true;
          // 
          // cbRestrictLevel
          // 
          this.cbRestrictLevel.AutoSize = true;
          this.cbRestrictLevel.Checked = global::bnfGenerator.Properties.Settings.Default.RestrictLevel;
          this.cbRestrictLevel.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::bnfGenerator.Properties.Settings.Default, "RestrictLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.cbRestrictLevel.Location = new System.Drawing.Point(11, 289);
          this.cbRestrictLevel.Name = "cbRestrictLevel";
          this.cbRestrictLevel.Size = new System.Drawing.Size(202, 17);
          this.cbRestrictLevel.TabIndex = 15;
          this.cbRestrictLevel.Text = "Limit nesting level:";
          this.cbRestrictLevel.UseVisualStyleBackColor = true;
          // 
          // tbRestrictLevel
          // 
          this.tbRestrictLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::bnfGenerator.Properties.Settings.Default, "RestrictLevelValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.tbRestrictLevel.Location = new System.Drawing.Point(11, 312);
          this.tbRestrictLevel.Name = "tbRestrictLevel";
          this.tbRestrictLevel.Size = new System.Drawing.Size(243, 20);
          this.tbRestrictLevel.TabIndex = 13;
          this.tbRestrictLevel.Text = global::bnfGenerator.Properties.Settings.Default.RestrictLevelValue;
          // 
          // txtLabelDir
          // 
          this.txtLabelDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::bnfGenerator.Properties.Settings.Default, "TestLabel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.txtLabelDir.Location = new System.Drawing.Point(11, 364);
          this.txtLabelDir.Name = "txtLabelDir";
          this.txtLabelDir.Size = new System.Drawing.Size(243, 20);
          this.txtLabelDir.TabIndex = 11;
          this.txtLabelDir.Text = global::bnfGenerator.Properties.Settings.Default.TestLabel;
          // 
          // radioOpLangF
          // 
          this.radioOpLangF.AutoSize = true;
          this.radioOpLangF.Checked = global::bnfGenerator.Properties.Settings.Default.FortLang;
          this.radioOpLangF.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::bnfGenerator.Properties.Settings.Default, "FortLang", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.radioOpLangF.Location = new System.Drawing.Point(125, 22);
          this.radioOpLangF.Name = "radioOpLangF";
          this.radioOpLangF.Size = new System.Drawing.Size(58, 17);
          this.radioOpLangF.TabIndex = 10;
          this.radioOpLangF.TabStop = true;
          this.radioOpLangF.Text = "Fortran";
          this.radioOpLangF.UseVisualStyleBackColor = true;
          // 
          // radioOpLangC
          // 
          this.radioOpLangC.AutoSize = true;
          this.radioOpLangC.Checked = global::bnfGenerator.Properties.Settings.Default.LangC;
          this.radioOpLangC.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::bnfGenerator.Properties.Settings.Default, "LangC", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.radioOpLangC.Location = new System.Drawing.Point(6, 22);
          this.radioOpLangC.Name = "radioOpLangC";
          this.radioOpLangC.Size = new System.Drawing.Size(32, 17);
          this.radioOpLangC.TabIndex = 9;
          this.radioOpLangC.TabStop = true;
          this.radioOpLangC.Text = "C";
          this.radioOpLangC.UseVisualStyleBackColor = true;
          // 
          // txtTotalTests
          // 
          this.txtTotalTests.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::bnfGenerator.Properties.Settings.Default, "TotalTestsValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.txtTotalTests.Location = new System.Drawing.Point(11, 154);
          this.txtTotalTests.Name = "txtTotalTests";
          this.txtTotalTests.Size = new System.Drawing.Size(234, 20);
          this.txtTotalTests.TabIndex = 8;
          this.txtTotalTests.Text = global::bnfGenerator.Properties.Settings.Default.TotalTestsValue;
          this.txtTotalTests.TextChanged += new System.EventHandler(this.txtTotalTests_TextChanged);
          // 
          // cbTotalTests
          // 
          this.cbTotalTests.AutoSize = true;
          this.cbTotalTests.Checked = global::bnfGenerator.Properties.Settings.Default.TotalTestsAmount;
          this.cbTotalTests.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::bnfGenerator.Properties.Settings.Default, "TotalTestsAmount", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.cbTotalTests.Location = new System.Drawing.Point(6, 118);
          this.cbTotalTests.Name = "cbTotalTests";
          this.cbTotalTests.Size = new System.Drawing.Size(172, 17);
          this.cbTotalTests.TabIndex = 6;
          this.cbTotalTests.Text = "Use limitation of tests quantity";
          this.cbTotalTests.UseVisualStyleBackColor = true;
          this.cbTotalTests.CheckedChanged += new System.EventHandler(this.cbTotalTests_CheckedChanged);
          // 
          // txtPathSaveToFile
          // 
          this.txtPathSaveToFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::bnfGenerator.Properties.Settings.Default, "TestSavePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.txtPathSaveToFile.Location = new System.Drawing.Point(11, 235);
          this.txtPathSaveToFile.Multiline = true;
          this.txtPathSaveToFile.Name = "txtPathSaveToFile";
          this.txtPathSaveToFile.Size = new System.Drawing.Size(243, 25);
          this.txtPathSaveToFile.TabIndex = 3;
          this.txtPathSaveToFile.Text = global::bnfGenerator.Properties.Settings.Default.TestSavePath;
          // 
          // cbSaveToFiles
          // 
          this.cbSaveToFiles.AutoSize = true;
          this.cbSaveToFiles.Checked = global::bnfGenerator.Properties.Settings.Default.TestSaveCheck;
          this.cbSaveToFiles.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::bnfGenerator.Properties.Settings.Default, "TestSaveCheck", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.cbSaveToFiles.Location = new System.Drawing.Point(6, 190);
          this.cbSaveToFiles.Name = "cbSaveToFiles";
          this.cbSaveToFiles.Size = new System.Drawing.Size(154, 17);
          this.cbSaveToFiles.TabIndex = 2;
          this.cbSaveToFiles.Text = "Save tests to files";
          this.cbSaveToFiles.UseVisualStyleBackColor = true;
          this.cbSaveToFiles.CheckedChanged += new System.EventHandler(this.cbSaveToFiles_CheckedChanged);
          // 
          // cbMinGWEnabled
          // 
          this.cbMinGWEnabled.AutoSize = true;
          this.cbMinGWEnabled.Checked = global::bnfGenerator.Properties.Settings.Default.CompilerCheck;
          this.cbMinGWEnabled.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::bnfGenerator.Properties.Settings.Default, "CompilerCheck", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.cbMinGWEnabled.Location = new System.Drawing.Point(7, 43);
          this.cbMinGWEnabled.Name = "cbMinGWEnabled";
          this.cbMinGWEnabled.Size = new System.Drawing.Size(227, 17);
          this.cbMinGWEnabled.TabIndex = 1;
          this.cbMinGWEnabled.Text = "Validation with compiler";
          this.cbMinGWEnabled.UseVisualStyleBackColor = true;
          // 
          // cbPPprocessingEnabled
          // 
          this.cbPPprocessingEnabled.AutoSize = true;
          this.cbPPprocessingEnabled.Checked = global::bnfGenerator.Properties.Settings.Default.ProcessTextCheck;
          this.cbPPprocessingEnabled.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::bnfGenerator.Properties.Settings.Default, "ProcessTextCheck", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.cbPPprocessingEnabled.Location = new System.Drawing.Point(7, 20);
          this.cbPPprocessingEnabled.Name = "cbPPprocessingEnabled";
          this.cbPPprocessingEnabled.Size = new System.Drawing.Size(159, 17);
          this.cbPPprocessingEnabled.TabIndex = 0;
          this.cbPPprocessingEnabled.Text = "Language processing";
          this.cbPPprocessingEnabled.UseVisualStyleBackColor = true;
          // 
          // cbRREnable
          // 
          this.cbRREnable.AutoSize = true;
          this.cbRREnable.Checked = global::bnfGenerator.Properties.Settings.Default.OptRREnable;
          this.cbRREnable.CheckState = System.Windows.Forms.CheckState.Checked;
          this.cbRREnable.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::bnfGenerator.Properties.Settings.Default, "OptRREnable", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.cbRREnable.Location = new System.Drawing.Point(7, 20);
          this.cbRREnable.Name = "cbRREnable";
          this.cbRREnable.Size = new System.Drawing.Size(78, 17);
          this.cbRREnable.TabIndex = 0;
          this.cbRREnable.Text = "RR Enable";
          this.cbRREnable.UseVisualStyleBackColor = true;
          // 
          // GenerationOptionsForm
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(292, 648);
          this.Controls.Add(this.panel1);
          this.Name = "GenerationOptionsForm";
          this.TabText = "Options";
          this.Text = "GenerationOptionsForm";
          this.panel1.ResumeLayout(false);
          this.groupBox3.ResumeLayout(false);
          this.groupBox3.PerformLayout();
          this.groupBox4.ResumeLayout(false);
          this.groupBox4.PerformLayout();
          this.groupBox2.ResumeLayout(false);
          this.groupBox2.PerformLayout();
          this.groupBox1.ResumeLayout(false);
          this.groupBox1.PerformLayout();
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioOpUniformDistr;
        private System.Windows.Forms.RadioButton radioOpEnum;
        private System.Windows.Forms.RadioButton radioOpNormalDistr;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbRREnable;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.CheckBox cbPPprocessingEnabled;
        public System.Windows.Forms.CheckBox cbSaveToFiles;
        public System.Windows.Forms.CheckBox cbMinGWEnabled;
        private System.Windows.Forms.FolderBrowserDialog dlgSetFolder;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtPathSaveToFile;
        private System.Windows.Forms.CheckBox cbAllEnumInOne;
        public System.Windows.Forms.CheckBox cbTotalTests;
        public System.Windows.Forms.TextBox txtTotalTests;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioOpMinRND;
        private System.Windows.Forms.RadioButton radioOpPairs;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioOpLangC;
        private System.Windows.Forms.RadioButton radioOpLangF;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtLabelDir;
        public System.Windows.Forms.CheckBox cbRestrictLevel;
        public System.Windows.Forms.TextBox tbRestrictLevel;
    }
}