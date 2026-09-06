using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.DynamicCards
{

public class LableWithValue : UserControl
{
	private string _CurrencyLable;

	private string _CurrencyAmount;

	private IContainer components;

	private Label lblCurrency;

	private Label lblValue;

	public string Currency
	{
		get
		{
			return _CurrencyLable;
		}
		set
		{
			_CurrencyLable = value;
			lblCurrency.Text = value;
		}
	}

	public string Amount
	{
		get
		{
			return _CurrencyAmount;
		}
		set
		{
			_CurrencyAmount = value;
			lblValue.Text = value;
		}
	}

	public LableWithValue()
	{
		InitializeComponent();
	}

	private void lblCurrency_Click(object sender, EventArgs e)
	{
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
		this.lblCurrency = new System.Windows.Forms.Label();
		this.lblValue = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblCurrency.BackColor = System.Drawing.Color.Teal;
		this.lblCurrency.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblCurrency.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCurrency.ForeColor = System.Drawing.Color.White;
		this.lblCurrency.Location = new System.Drawing.Point(0, 0);
		this.lblCurrency.Name = "lblCurrency";
		this.lblCurrency.Size = new System.Drawing.Size(164, 26);
		this.lblCurrency.TabIndex = 0;
		this.lblCurrency.Text = "Currency";
		this.lblCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblCurrency.Click += new System.EventHandler(lblCurrency_Click);
		this.lblValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblValue.Location = new System.Drawing.Point(4, 32);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(157, 20);
		this.lblValue.TabIndex = 1;
		this.lblValue.Text = "label2";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.lblValue);
		base.Controls.Add(this.lblCurrency);
		base.Name = "LableWithValue";
		base.Size = new System.Drawing.Size(164, 61);
		base.ResumeLayout(false);
	}
}

}
