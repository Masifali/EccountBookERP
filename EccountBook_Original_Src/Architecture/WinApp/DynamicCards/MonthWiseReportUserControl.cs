using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.DynamicCards;

public class MonthWiseReportUserControl : UserControl
{
	private string _Month;

	private string _Amount;

	private IContainer components;

	private Label label6;

	private Label lblAmount;

	private Label lblMonth;

	public string Month
	{
		get
		{
			return _Month;
		}
		set
		{
			_Month = value;
			lblMonth.Text = value;
		}
	}

	public string Amount
	{
		get
		{
			return _Amount;
		}
		set
		{
			_Amount = value;
			lblAmount.Text = value;
		}
	}

	public MonthWiseReportUserControl()
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
		this.label6 = new System.Windows.Forms.Label();
		this.lblAmount = new System.Windows.Forms.Label();
		this.lblMonth = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(84, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(538, 17);
		this.label6.TabIndex = 7;
		this.label6.Text = "----------------------------------------------------------------------------------------------------------";
		this.lblAmount.AutoSize = true;
		this.lblAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmount.Location = new System.Drawing.Point(648, 4);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(15, 17);
		this.lblAmount.TabIndex = 6;
		this.lblAmount.Text = "0";
		this.lblMonth.AutoSize = true;
		this.lblMonth.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMonth.Location = new System.Drawing.Point(4, 4);
		this.lblMonth.Name = "lblMonth";
		this.lblMonth.Size = new System.Drawing.Size(53, 17);
		this.lblMonth.TabIndex = 5;
		this.lblMonth.Text = "Month:";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.label6);
		base.Controls.Add(this.lblAmount);
		base.Controls.Add(this.lblMonth);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "MonthWiseReportUserControl";
		base.Size = new System.Drawing.Size(763, 26);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
