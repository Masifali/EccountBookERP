using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.ModernHistories;

public class WagesInfoControl : UserControl
{
	private int _ContractorId;

	private string _ContractorName;

	private string _WagesAccountName;

	private string _Quantity;

	private string _Weight;

	private string _Packsize;

	private string _Rate;

	private string _Amount;

	private IContainer components;

	private Label lblAmount;

	private Label lblRate;

	private Label lblPackSize;

	private Label lblWeight;

	private Label lblQuantity;

	private Label lblWagesAccountName;

	private Label lblContractorName;

	public int ContractorId
	{
		get
		{
			return _ContractorId;
		}
		set
		{
			_ContractorId = value;
		}
	}

	public string ContractorName
	{
		get
		{
			return _ContractorName;
		}
		set
		{
			_ContractorName = value;
			lblContractorName.Text = value;
		}
	}

	public string WagesAccountName
	{
		get
		{
			return _WagesAccountName;
		}
		set
		{
			_WagesAccountName = value;
			lblWagesAccountName.Text = value;
		}
	}

	public string Quantity
	{
		get
		{
			return _Quantity;
		}
		set
		{
			_Quantity = value;
			lblQuantity.Text = value;
		}
	}

	public string Weight
	{
		get
		{
			return _Weight;
		}
		set
		{
			_Weight = value;
			lblWeight.Text = value;
		}
	}

	public string PackSize
	{
		get
		{
			return _Packsize;
		}
		set
		{
			_Packsize = value;
			lblPackSize.Text = value;
		}
	}

	public string Rate
	{
		get
		{
			return _Rate;
		}
		set
		{
			_Rate = value;
			lblRate.Text = value;
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

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public WagesInfoControl()
	{
		InitializeComponent();
		lblContractorName.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
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
		this.lblAmount = new System.Windows.Forms.Label();
		this.lblRate = new System.Windows.Forms.Label();
		this.lblPackSize = new System.Windows.Forms.Label();
		this.lblWeight = new System.Windows.Forms.Label();
		this.lblQuantity = new System.Windows.Forms.Label();
		this.lblWagesAccountName = new System.Windows.Forms.Label();
		this.lblContractorName = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblAmount.AutoSize = true;
		this.lblAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmount.ForeColor = System.Drawing.Color.Black;
		this.lblAmount.Location = new System.Drawing.Point(576, 4);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(58, 17);
		this.lblAmount.TabIndex = 15;
		this.lblAmount.Text = "Amount";
		this.lblRate.AutoSize = true;
		this.lblRate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRate.ForeColor = System.Drawing.Color.Black;
		this.lblRate.Location = new System.Drawing.Point(533, 4);
		this.lblRate.Name = "lblRate";
		this.lblRate.Size = new System.Drawing.Size(35, 17);
		this.lblRate.TabIndex = 14;
		this.lblRate.Text = "Rate";
		this.lblPackSize.AutoSize = true;
		this.lblPackSize.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPackSize.ForeColor = System.Drawing.Color.Black;
		this.lblPackSize.Location = new System.Drawing.Point(459, 4);
		this.lblPackSize.Name = "lblPackSize";
		this.lblPackSize.Size = new System.Drawing.Size(64, 17);
		this.lblPackSize.TabIndex = 13;
		this.lblPackSize.Text = "Pack Size";
		this.lblWeight.AutoSize = true;
		this.lblWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWeight.ForeColor = System.Drawing.Color.Black;
		this.lblWeight.Location = new System.Drawing.Point(392, 4);
		this.lblWeight.Name = "lblWeight";
		this.lblWeight.Size = new System.Drawing.Size(53, 17);
		this.lblWeight.TabIndex = 12;
		this.lblWeight.Text = "Weight";
		this.lblQuantity.AutoSize = true;
		this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblQuantity.ForeColor = System.Drawing.Color.Black;
		this.lblQuantity.Location = new System.Drawing.Point(339, 4);
		this.lblQuantity.Name = "lblQuantity";
		this.lblQuantity.Size = new System.Drawing.Size(30, 17);
		this.lblQuantity.TabIndex = 11;
		this.lblQuantity.Text = "Qty";
		this.lblWagesAccountName.AutoEllipsis = true;
		this.lblWagesAccountName.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWagesAccountName.ForeColor = System.Drawing.Color.Black;
		this.lblWagesAccountName.Location = new System.Drawing.Point(209, 4);
		this.lblWagesAccountName.Name = "lblWagesAccountName";
		this.lblWagesAccountName.Size = new System.Drawing.Size(116, 17);
		this.lblWagesAccountName.TabIndex = 10;
		this.lblWagesAccountName.Text = "Wages Account";
		this.lblContractorName.AutoEllipsis = true;
		this.lblContractorName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblContractorName.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblContractorName.ForeColor = System.Drawing.Color.Blue;
		this.lblContractorName.Location = new System.Drawing.Point(3, 4);
		this.lblContractorName.Name = "lblContractorName";
		this.lblContractorName.Size = new System.Drawing.Size(200, 17);
		this.lblContractorName.TabIndex = 9;
		this.lblContractorName.Text = "Contractor Name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblAmount);
		base.Controls.Add(this.lblRate);
		base.Controls.Add(this.lblPackSize);
		base.Controls.Add(this.lblWeight);
		base.Controls.Add(this.lblQuantity);
		base.Controls.Add(this.lblWagesAccountName);
		base.Controls.Add(this.lblContractorName);
		base.Name = "WagesInfoControl";
		base.Size = new System.Drawing.Size(638, 27);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
