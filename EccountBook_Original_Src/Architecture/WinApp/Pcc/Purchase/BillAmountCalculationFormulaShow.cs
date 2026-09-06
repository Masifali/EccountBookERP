using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.Pcc.Purchase;

public class BillAmountCalculationFormulaShow : Form
{
	private IContainer components;

	public BillAmountCalculationFormulaShow()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(326, 209);
		base.Name = "BillAmountCalculationFormulaShow";
		this.Text = "Calculation Formula";
		base.ResumeLayout(false);
	}
}
