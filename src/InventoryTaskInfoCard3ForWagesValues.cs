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

public class InventoryTaskInfoCard3ForWagesValues : UserControl
{
	private int _TransId;

	private int _ParentId;

	private string _Activity;

	private string _Qty;

	private string _Weight;

	private string _Rate;

	private string _Amount;

	private IContainer components;

	private Label lblActivity;

	private Label lblWeight;

	private Label lblQty;

	private Label lblAmount;

	private Label lblRate;

	public int TransId
	{
		get
		{
			return _TransId;
		}
		set
		{
			_TransId = value;
		}
	}

	public int ParentId
	{
		get
		{
			return _ParentId;
		}
		set
		{
			_ParentId = value;
		}
	}

	public string Activity
	{
		get
		{
			return _Activity;
		}
		set
		{
			_Activity = value;
			lblActivity.Text = value;
		}
	}

	public string Qty
	{
		get
		{
			return _Qty;
		}
		set
		{
			_Qty = value;
			lblQty.Text = value;
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

	public InventoryTaskInfoCard3ForWagesValues()
	{
		InitializeComponent();
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void CompletePurchaseSaleDocs_Load(object sender, EventArgs e)
	{
		try
		{
			if (TransId == 1)
			{
				BackColor = Color.FromArgb(255, 221, 149);
			}
			else if (TransId == 2)
			{
				BackColor = Color.FromArgb(137, 207, 243);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
		this.lblActivity = new System.Windows.Forms.Label();
		this.lblAmount = new System.Windows.Forms.Label();
		this.lblRate = new System.Windows.Forms.Label();
		this.lblWeight = new System.Windows.Forms.Label();
		this.lblQty = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblActivity.AutoEllipsis = true;
		this.lblActivity.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblActivity.Location = new System.Drawing.Point(3, 2);
		this.lblActivity.Name = "lblActivity";
		this.lblActivity.Size = new System.Drawing.Size(130, 16);
		this.lblActivity.TabIndex = 2;
		this.lblActivity.Text = "Unloading";
		this.lblAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblAmount.Location = new System.Drawing.Point(427, 2);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(124, 16);
		this.lblAmount.TabIndex = 13;
		this.lblAmount.Text = "12971.255";
		this.lblAmount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.lblRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblRate.Location = new System.Drawing.Point(356, 2);
		this.lblRate.Name = "lblRate";
		this.lblRate.Size = new System.Drawing.Size(65, 16);
		this.lblRate.TabIndex = 12;
		this.lblRate.Text = "5.85";
		this.lblRate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.lblWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblWeight.Location = new System.Drawing.Point(238, 2);
		this.lblWeight.Name = "lblWeight";
		this.lblWeight.Size = new System.Drawing.Size(116, 16);
		this.lblWeight.TabIndex = 11;
		this.lblWeight.Text = "146400.00";
		this.lblWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.lblQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.lblQty.Location = new System.Drawing.Point(145, 2);
		this.lblQty.Name = "lblQty";
		this.lblQty.Size = new System.Drawing.Size(85, 16);
		this.lblQty.TabIndex = 10;
		this.lblQty.Text = "2440";
		this.lblQty.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(255, 221, 149);
		base.Controls.Add(this.lblActivity);
		base.Controls.Add(this.lblQty);
		base.Controls.Add(this.lblWeight);
		base.Controls.Add(this.lblRate);
		base.Controls.Add(this.lblAmount);
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InventoryTaskInfoCard3ForWagesValues";
		base.Size = new System.Drawing.Size(553, 21);
		base.Load += new System.EventHandler(CompletePurchaseSaleDocs_Load);
		base.ResumeLayout(false);
	}
}

}
