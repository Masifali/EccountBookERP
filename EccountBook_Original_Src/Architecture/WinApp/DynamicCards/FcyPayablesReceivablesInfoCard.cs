using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.DynamicCards;

public class FcyPayablesReceivablesInfoCard : UserControl
{
	private string _FcyCode;

	private string _FcyAmount;

	private string _PkrAmount;

	private string _FcyRate;

	private IContainer components;

	private Label lblPkrAmount;

	private Label lblFcyAmount;

	private Label lblFcyRate;

	private Label lblFcyCode;

	public string FcyCode
	{
		get
		{
			return _FcyCode;
		}
		set
		{
			_FcyCode = value;
			lblFcyCode.Text = value;
		}
	}

	public string FcyAmount
	{
		get
		{
			return _FcyAmount;
		}
		set
		{
			_FcyAmount = value;
			lblFcyAmount.Text = value;
		}
	}

	public string PkrAmount
	{
		get
		{
			return _PkrAmount;
		}
		set
		{
			_PkrAmount = value;
			lblPkrAmount.Text = value;
		}
	}

	public string FcyRate
	{
		get
		{
			return _FcyRate;
		}
		set
		{
			_FcyRate = value;
			lblFcyRate.Text = value;
		}
	}

	public FcyPayablesReceivablesInfoCard()
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
		this.lblPkrAmount = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.lblFcyRate = new System.Windows.Forms.Label();
		this.lblFcyCode = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblPkrAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPkrAmount.ForeColor = System.Drawing.Color.Black;
		this.lblPkrAmount.Location = new System.Drawing.Point(220, 2);
		this.lblPkrAmount.Name = "lblPkrAmount";
		this.lblPkrAmount.Size = new System.Drawing.Size(144, 17);
		this.lblPkrAmount.TabIndex = 15;
		this.lblPkrAmount.Text = "525";
		this.lblPkrAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyAmount.ForeColor = System.Drawing.Color.Black;
		this.lblFcyAmount.Location = new System.Drawing.Point(101, 3);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(114, 17);
		this.lblFcyAmount.TabIndex = 14;
		this.lblFcyAmount.Text = "524";
		this.lblFcyAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFcyRate.AutoSize = true;
		this.lblFcyRate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyRate.ForeColor = System.Drawing.Color.Black;
		this.lblFcyRate.Location = new System.Drawing.Point(378, 3);
		this.lblFcyRate.Name = "lblFcyRate";
		this.lblFcyRate.Size = new System.Drawing.Size(56, 17);
		this.lblFcyRate.TabIndex = 13;
		this.lblFcyRate.Text = "Fcy Rate";
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.ForeColor = System.Drawing.Color.Black;
		this.lblFcyCode.Location = new System.Drawing.Point(3, 3);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 17);
		this.lblFcyCode.TabIndex = 12;
		this.lblFcyCode.Text = "DOLLAR";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblPkrAmount);
		base.Controls.Add(this.lblFcyAmount);
		base.Controls.Add(this.lblFcyRate);
		base.Controls.Add(this.lblFcyCode);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "FcyPayablesReceivablesInfoCard";
		base.Size = new System.Drawing.Size(439, 23);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
