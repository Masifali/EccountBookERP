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
namespace Architecture.WinApp.ModernHistories
{

public class OtherInfoCard1 : UserControl
{
	private string _Name;

	private string _Amount;

	private string _Remarks;

	private IContainer components;

	private Panel CTPHeaderPanel;

	private Label Remarks;

	private Label Amount;

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

	public string amount
	{
		get
		{
			return _Amount;
		}
		set
		{
			_Amount = value;
			Amount.Text = value;
		}
	}

	public string remarks
	{
		get
		{
			return _Remarks;
		}
		set
		{
			_Remarks = value;
			Remarks.Text = value;
		}
	}

	public OtherInfoCard1()
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
		this.CTPHeaderPanel = new System.Windows.Forms.Panel();
		this.Remarks = new System.Windows.Forms.Label();
		this.Amount = new System.Windows.Forms.Label();
		this.AccountName = new System.Windows.Forms.Label();
		this.CTPHeaderPanel.SuspendLayout();
		base.SuspendLayout();
		this.CTPHeaderPanel.BackColor = System.Drawing.Color.White;
		this.CTPHeaderPanel.Controls.Add(this.Remarks);
		this.CTPHeaderPanel.Controls.Add(this.Amount);
		this.CTPHeaderPanel.Controls.Add(this.AccountName);
		this.CTPHeaderPanel.Location = new System.Drawing.Point(0, 0);
		this.CTPHeaderPanel.Name = "CTPHeaderPanel";
		this.CTPHeaderPanel.Size = new System.Drawing.Size(459, 25);
		this.CTPHeaderPanel.TabIndex = 2;
		this.Remarks.AutoEllipsis = true;
		this.Remarks.BackColor = System.Drawing.Color.White;
		this.Remarks.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Remarks.ForeColor = System.Drawing.Color.DimGray;
		this.Remarks.Location = new System.Drawing.Point(273, 4);
		this.Remarks.Name = "Remarks";
		this.Remarks.Size = new System.Drawing.Size(184, 17);
		this.Remarks.TabIndex = 2;
		this.Remarks.Text = "Remarks";
		this.Amount.BackColor = System.Drawing.Color.White;
		this.Amount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Amount.ForeColor = System.Drawing.Color.DimGray;
		this.Amount.Location = new System.Drawing.Point(196, 4);
		this.Amount.Name = "Amount";
		this.Amount.Size = new System.Drawing.Size(71, 17);
		this.Amount.TabIndex = 1;
		this.Amount.Text = "Amount";
		this.AccountName.AutoEllipsis = true;
		this.AccountName.BackColor = System.Drawing.Color.White;
		this.AccountName.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.AccountName.ForeColor = System.Drawing.Color.DimGray;
		this.AccountName.Location = new System.Drawing.Point(1, 3);
		this.AccountName.Name = "AccountName";
		this.AccountName.Size = new System.Drawing.Size(189, 21);
		this.AccountName.TabIndex = 0;
		this.AccountName.Text = "Name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.CTPHeaderPanel);
		base.Name = "OtherInfoCard1";
		base.Size = new System.Drawing.Size(460, 25);
		this.CTPHeaderPanel.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
