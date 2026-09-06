using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Architecture.WinApp.ModernHistories;

public class OtherInfoCard2 : UserControl
{
	private string _Name;

	private string _Debit;

	private string _Credit;

	private IContainer components;

	private Label lblCredit;

	private Label lblDebit;

	private Label AccountName;

	public string name
	{
		get
		{
			return _Name;
		}
		set
		{
			_Name = value;
			AccountName.Text = value;
		}
	}

	public string Debit
	{
		get
		{
			return _Debit;
		}
		set
		{
			_Debit = value;
			lblDebit.Text = value;
		}
	}

	public string Credit
	{
		get
		{
			return _Credit;
		}
		set
		{
			_Credit = value;
			lblCredit.Text = value;
		}
	}

	public OtherInfoCard2()
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
		this.lblCredit = new System.Windows.Forms.Label();
		this.lblDebit = new System.Windows.Forms.Label();
		this.AccountName = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblCredit.BackColor = System.Drawing.Color.White;
		this.lblCredit.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCredit.ForeColor = System.Drawing.Color.DimGray;
		this.lblCredit.Location = new System.Drawing.Point(387, 3);
		this.lblCredit.Name = "lblCredit";
		this.lblCredit.Size = new System.Drawing.Size(66, 17);
		this.lblCredit.TabIndex = 2;
		this.lblCredit.Text = "Credit";
		this.lblDebit.BackColor = System.Drawing.Color.White;
		this.lblDebit.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDebit.ForeColor = System.Drawing.Color.DimGray;
		this.lblDebit.Location = new System.Drawing.Point(298, 3);
		this.lblDebit.Name = "lblDebit";
		this.lblDebit.Size = new System.Drawing.Size(64, 17);
		this.lblDebit.TabIndex = 1;
		this.lblDebit.Text = "Debit";
		this.AccountName.AutoEllipsis = true;
		this.AccountName.BackColor = System.Drawing.Color.White;
		this.AccountName.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.AccountName.ForeColor = System.Drawing.Color.DimGray;
		this.AccountName.Location = new System.Drawing.Point(3, 1);
		this.AccountName.Name = "AccountName";
		this.AccountName.Size = new System.Drawing.Size(288, 19);
		this.AccountName.TabIndex = 0;
		this.AccountName.Text = "Freight Of Serani Goods Gujranwala";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblCredit);
		base.Controls.Add(this.lblDebit);
		base.Controls.Add(this.AccountName);
		base.Name = "OtherInfoCard2";
		base.Size = new System.Drawing.Size(456, 23);
		base.ResumeLayout(false);
	}
}
