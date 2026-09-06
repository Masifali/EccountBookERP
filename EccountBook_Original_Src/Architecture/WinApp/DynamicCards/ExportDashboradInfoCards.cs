using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.DynamicCards;

public class ExportDashboradInfoCards : UserControl
{
	private string _MTons;

	private string _FcyCode;

	private string _FcyRate;

	private string _FcyAmount;

	private string _PkrAmount;

	private IContainer components;

	private Label lblPkrAmount;

	private Label lblMtons;

	private Label lblFcyAmount;

	private Label lblFcyRate;

	private Label lblFcyCode;

	public string Mtons
	{
		get
		{
			return _MTons;
		}
		set
		{
			_MTons = value;
			lblMtons.Text = value;
		}
	}

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

	public ExportDashboradInfoCards()
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
		this.lblMtons = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.lblFcyRate = new System.Windows.Forms.Label();
		this.lblFcyCode = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblPkrAmount.AutoSize = true;
		this.lblPkrAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPkrAmount.ForeColor = System.Drawing.Color.Black;
		this.lblPkrAmount.Location = new System.Drawing.Point(423, 4);
		this.lblPkrAmount.Name = "lblPkrAmount";
		this.lblPkrAmount.Size = new System.Drawing.Size(80, 17);
		this.lblPkrAmount.TabIndex = 15;
		this.lblPkrAmount.Text = "PKR Amount";
		this.lblMtons.AutoSize = true;
		this.lblMtons.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMtons.ForeColor = System.Drawing.Color.Black;
		this.lblMtons.Location = new System.Drawing.Point(11, 4);
		this.lblMtons.Name = "lblMtons";
		this.lblMtons.Size = new System.Drawing.Size(51, 17);
		this.lblMtons.TabIndex = 11;
		this.lblMtons.Text = "M Tons";
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyAmount.ForeColor = System.Drawing.Color.Black;
		this.lblFcyAmount.Location = new System.Drawing.Point(308, 4);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(75, 17);
		this.lblFcyAmount.TabIndex = 14;
		this.lblFcyAmount.Text = "Fcy Amount";
		this.lblFcyRate.AutoSize = true;
		this.lblFcyRate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyRate.ForeColor = System.Drawing.Color.Black;
		this.lblFcyRate.Location = new System.Drawing.Point(199, 4);
		this.lblFcyRate.Name = "lblFcyRate";
		this.lblFcyRate.Size = new System.Drawing.Size(56, 17);
		this.lblFcyRate.TabIndex = 13;
		this.lblFcyRate.Text = "Fcy Rate";
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.ForeColor = System.Drawing.Color.Black;
		this.lblFcyCode.Location = new System.Drawing.Point(121, 4);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(26, 17);
		this.lblFcyCode.TabIndex = 12;
		this.lblFcyCode.Text = "Fcy";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblPkrAmount);
		base.Controls.Add(this.lblMtons);
		base.Controls.Add(this.lblFcyAmount);
		base.Controls.Add(this.lblFcyRate);
		base.Controls.Add(this.lblFcyCode);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "ExportDashboradInfoCards";
		base.Size = new System.Drawing.Size(538, 26);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
