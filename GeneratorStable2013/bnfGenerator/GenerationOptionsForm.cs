 
using System;
using bnfGenerator.Properties;
using GrammarCompiler;

namespace bnfGenerator
{
  public partial class GenerationOptionsForm : ToolWindow
  {
    private readonly GrammarOptions mOpt;

    public GenerationOptionsForm()
    {
      InitializeComponent();
      mOpt = new GrammarOptions();
    }

    protected override void OnLoad(EventArgs e)
    {
      mOpt.AlternativeAlg =
        (AlternativeSelectAlg)
        Enum.Parse(typeof (AlternativeSelectAlg), Settings.Default.AlternativeSelectAlg);
      switch (mOpt.AlternativeAlg)
      {
        case AlternativeSelectAlg.Enum:
          radioOpEnum.Checked = true;
          break;
        case AlternativeSelectAlg.NormalDistr:
          radioOpNormalDistr.Checked = true;
          break;
        case AlternativeSelectAlg.RndDistr:
          radioOpUniformDistr.Checked = true;
          break;
        case AlternativeSelectAlg.MinRnd:
          radioOpMinRND.Checked = true;
          break;
        case AlternativeSelectAlg.Pairs:
          radioOpPairs.Checked = true;
          break;
      }
      base.OnLoad(e);

      txtPathSaveToFile.Enabled = cbSaveToFiles.Checked;
      txtTotalTests.Enabled = cbTotalTests.Checked;
      btnChooseFolder.Enabled = cbSaveToFiles.Checked;
    }

    public AlternativeSelectAlg GetAlg()
    {
      if (radioOpEnum.Checked)
      {
        return AlternativeSelectAlg.Enum;
      }
      else if (radioOpNormalDistr.Checked)
      {
        return AlternativeSelectAlg.NormalDistr;
      }
      else if (radioOpUniformDistr.Checked)
      {
        return AlternativeSelectAlg.RndDistr;
      }
      else if (radioOpMinRND.Checked)
      {
        return AlternativeSelectAlg.MinRnd;
      }
      else if (radioOpPairs.Checked)
      {
        return AlternativeSelectAlg.Pairs;
      }
      throw new ApplicationException("No algorithm selected");
    }

    public GrammarOptions GetOptions()
    {
      mOpt.AlternativeAlg = GetAlg();
      mOpt.RREnable = cbRREnable.Checked;
      mOpt.AllEnumInOne = cbAllEnumInOne.Checked;

      mOpt.TestsAmount = cbTotalTests.Checked ?
                      Convert.ToInt32(txtTotalTests.Text) : 1;

      mOpt.LevelRestriction = cbRestrictLevel.Checked ?
                      Convert.ToInt32(tbRestrictLevel.Text) : Int32.MaxValue;


      Settings.Default.Save();
      return mOpt;
    }

    private void btnChooseFolder_Click(object sender, EventArgs e)
    {
      dlgSetFolder.ShowDialog();
      txtPathSaveToFile.Text = dlgSetFolder.SelectedPath;
    }

    private void cbSaveToFiles_CheckedChanged(object sender, EventArgs e)
    {
      txtPathSaveToFile.Enabled = cbSaveToFiles.Checked;
      btnChooseFolder.Enabled = cbSaveToFiles.Checked;
    }

    private void txtTotalTests_TextChanged(object sender, EventArgs e)
    {
      try
      {
        int.Parse(txtTotalTests.Text);
      }
      catch
      {
        txtTotalTests.Text = "";
      }
    }

    private void cbTotalTests_CheckedChanged(object sender, EventArgs e)
    {
      txtTotalTests.Enabled = cbTotalTests.Checked;
    }
  }
}