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
namespace Architecture.WinApp.ModernHistories
{

public class InvoiceHeaderCard : UserControl
{
	private string _RequestFrom;

	private bool _IsSelected;

	private int _InvoiceId;

	private int _InvoiceType;

	private int _DocumentTypeId;

	private string _SupplierName;

	private string _InvoiceNo;

	private string _OrderNumber;

	private string _CaptionForInvoiceType;

	private string _ApprovedOrNot;

	private string _DocDate;

	private string _BillAmount;

	private IContainer components;

	private Label lblApprovedOrNot;

	private Label lblBillAmount;

	private Label lblInvoiceDate;

	private Label lblPurchaseCaption;

	private Label lblOrderNumber;

	private Label lblInvoiceNumber;

	private Label lableOrder;

	private Label label11;

	private Label lblSupplierName;

	public CheckBox CbApprove;

	public string RequestFrom
	{
		get
		{
			return _RequestFrom;
		}
		set
		{
			_RequestFrom = value;
		}
	}

	public bool IsSelected
	{
		get
		{
			return _IsSelected;
		}
		set
		{
			_IsSelected = value;
		}
	}

	public int InvoiceId
	{
		get
		{
			return _InvoiceId;
		}
		set
		{
			_InvoiceId = value;
		}
	}

	public int InvoiceType
	{
		get
		{
			return _InvoiceType;
		}
		set
		{
			_InvoiceType = value;
		}
	}

	public int DocumentTypeId
	{
		get
		{
			return _DocumentTypeId;
		}
		set
		{
			_DocumentTypeId = value;
		}
	}

	public string SupplierName
	{
		get
		{
			return _SupplierName;
		}
		set
		{
			_SupplierName = value;
			lblSupplierName.Text = value;
		}
	}

	public string InvoiceNumber
	{
		get
		{
			return _InvoiceNo;
		}
		set
		{
			_InvoiceNo = value;
			lblInvoiceNumber.Text = value;
		}
	}

	public string OrderNumber
	{
		get
		{
			return _OrderNumber;
		}
		set
		{
			_OrderNumber = value;
			lblOrderNumber.Text = value;
		}
	}

	public string CaptionForInvoiceType
	{
		get
		{
			return _CaptionForInvoiceType;
		}
		set
		{
			_CaptionForInvoiceType = value;
			lblPurchaseCaption.Text = value;
		}
	}

	public string ApprovedOrNot
	{
		get
		{
			return _ApprovedOrNot;
		}
		set
		{
			_ApprovedOrNot = value;
			lblApprovedOrNot.Text = value;
		}
	}

	public string DocDate
	{
		get
		{
			return _DocDate;
		}
		set
		{
			_DocDate = value;
			lblInvoiceDate.Text = value;
		}
	}

	public string BillAmount
	{
		get
		{
			return _BillAmount;
		}
		set
		{
			_BillAmount = value;
			lblBillAmount.Text = value;
		}
	}

	[Browsable(true)]
	public event EventHandler UserControlClicked;

	public InvoiceHeaderCard()
	{
		InitializeComponent();
		base.Click += ControlClicked;
		lblSupplierName.Click += ControlClicked;
		lblInvoiceNumber.Click += ControlClicked;
		lblOrderNumber.Click += ControlClicked;
		lblPurchaseCaption.Click += ControlClicked;
		lblApprovedOrNot.Click += ControlClicked;
		lblInvoiceDate.Click += ControlClicked;
		lblBillAmount.Click += ControlClicked;
		CbApprove.Click += ControlClicked;
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void InvoiceHeaderCard_Load(object sender, EventArgs e)
	{
		try
		{
			if (CaptionForInvoiceType == "Regular Purchase")
			{
				lableOrder.Show();
				lblOrderNumber.Show();
				lblPurchaseCaption.BackColor = Color.Teal;
			}
			else if (CaptionForInvoiceType == "Market Purchase")
			{
				lableOrder.Hide();
				lblOrderNumber.Hide();
				lblPurchaseCaption.BackColor = Color.MediumPurple;
			}
			else if (CaptionForInvoiceType == "Gate Purchase")
			{
				lableOrder.Hide();
				lblOrderNumber.Hide();
				lblPurchaseCaption.BackColor = Color.ForestGreen;
			}
			if (ApprovedOrNot == "Approved")
			{
				lblApprovedOrNot.BackColor = Color.Teal;
			}
			else
			{
				lblApprovedOrNot.BackColor = Color.Red;
			}
			if (InvoiceType == 1)
			{
				lblPurchaseCaption.Location = new Point(9, 72);
				lblApprovedOrNot.Location = new Point(208, 72);
			}
			else
			{
				lblPurchaseCaption.Hide();
				lblApprovedOrNot.Location = new Point(104, 72);
			}
			if (RequestFrom == "Purchase Invoice Approval" || RequestFrom == "Sale Invoice Approval")
			{
				CbApprove.Show();
			}
			else
			{
				CbApprove.Hide();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CbApprove_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (CbApprove.Checked)
			{
				IsSelected = true;
			}
			else
			{
				IsSelected = false;
			}
			base.Tag = "FromCheckBox";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceHeaderCard_Click(object sender, EventArgs e)
	{
		try
		{
			base.Tag = "UserControlClick";
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
		this.lblApprovedOrNot = new System.Windows.Forms.Label();
		this.lblBillAmount = new System.Windows.Forms.Label();
		this.lblInvoiceDate = new System.Windows.Forms.Label();
		this.lblPurchaseCaption = new System.Windows.Forms.Label();
		this.lblOrderNumber = new System.Windows.Forms.Label();
		this.lblInvoiceNumber = new System.Windows.Forms.Label();
		this.lableOrder = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.lblSupplierName = new System.Windows.Forms.Label();
		this.CbApprove = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		this.lblApprovedOrNot.AutoSize = true;
		this.lblApprovedOrNot.BackColor = System.Drawing.Color.Red;
		this.lblApprovedOrNot.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedOrNot.ForeColor = System.Drawing.Color.White;
		this.lblApprovedOrNot.Location = new System.Drawing.Point(208, 72);
		this.lblApprovedOrNot.Name = "lblApprovedOrNot";
		this.lblApprovedOrNot.Size = new System.Drawing.Size(92, 17);
		this.lblApprovedOrNot.TabIndex = 17;
		this.lblApprovedOrNot.Text = "Not Approved";
		this.lblBillAmount.AutoSize = true;
		this.lblBillAmount.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBillAmount.Location = new System.Drawing.Point(203, 93);
		this.lblBillAmount.Name = "lblBillAmount";
		this.lblBillAmount.Size = new System.Drawing.Size(112, 21);
		this.lblBillAmount.TabIndex = 16;
		this.lblBillAmount.Text = "1,121,121,255";
		this.lblInvoiceDate.AutoSize = true;
		this.lblInvoiceDate.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblInvoiceDate.ForeColor = System.Drawing.Color.Teal;
		this.lblInvoiceDate.Location = new System.Drawing.Point(8, 93);
		this.lblInvoiceDate.Name = "lblInvoiceDate";
		this.lblInvoiceDate.Size = new System.Drawing.Size(76, 21);
		this.lblInvoiceDate.TabIndex = 15;
		this.lblInvoiceDate.Text = "23-12-22";
		this.lblPurchaseCaption.AutoSize = true;
		this.lblPurchaseCaption.BackColor = System.Drawing.Color.Teal;
		this.lblPurchaseCaption.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPurchaseCaption.ForeColor = System.Drawing.Color.White;
		this.lblPurchaseCaption.Location = new System.Drawing.Point(9, 72);
		this.lblPurchaseCaption.Name = "lblPurchaseCaption";
		this.lblPurchaseCaption.Size = new System.Drawing.Size(109, 17);
		this.lblPurchaseCaption.TabIndex = 14;
		this.lblPurchaseCaption.Text = "Regular Purchase";
		this.lblOrderNumber.AutoSize = true;
		this.lblOrderNumber.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOrderNumber.ForeColor = System.Drawing.Color.SteelBlue;
		this.lblOrderNumber.Location = new System.Drawing.Point(260, 50);
		this.lblOrderNumber.Name = "lblOrderNumber";
		this.lblOrderNumber.Size = new System.Drawing.Size(15, 17);
		this.lblOrderNumber.TabIndex = 13;
		this.lblOrderNumber.Text = "0";
		this.lblInvoiceNumber.AutoSize = true;
		this.lblInvoiceNumber.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblInvoiceNumber.ForeColor = System.Drawing.Color.SteelBlue;
		this.lblInvoiceNumber.Location = new System.Drawing.Point(67, 50);
		this.lblInvoiceNumber.Name = "lblInvoiceNumber";
		this.lblInvoiceNumber.Size = new System.Drawing.Size(15, 17);
		this.lblInvoiceNumber.TabIndex = 12;
		this.lblInvoiceNumber.Text = "0";
		this.lableOrder.AutoSize = true;
		this.lableOrder.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lableOrder.ForeColor = System.Drawing.Color.SteelBlue;
		this.lableOrder.Location = new System.Drawing.Point(209, 50);
		this.lableOrder.Name = "lableOrder";
		this.lableOrder.Size = new System.Drawing.Size(55, 17);
		this.lableOrder.TabIndex = 11;
		this.lableOrder.Text = "Order #";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.SteelBlue;
		this.label11.Location = new System.Drawing.Point(7, 50);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(64, 17);
		this.label11.TabIndex = 10;
		this.label11.Text = "Invoice #";
		this.lblSupplierName.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSupplierName.Location = new System.Drawing.Point(3, 4);
		this.lblSupplierName.Name = "lblSupplierName";
		this.lblSupplierName.Size = new System.Drawing.Size(297, 46);
		this.lblSupplierName.TabIndex = 9;
		this.lblSupplierName.Text = "Al Khan Rice Mill Narowal Rana Safarash 12121 121212 1212";
		this.CbApprove.AutoSize = true;
		this.CbApprove.Location = new System.Drawing.Point(302, 6);
		this.CbApprove.Name = "CbApprove";
		this.CbApprove.Size = new System.Drawing.Size(15, 14);
		this.CbApprove.TabIndex = 31;
		this.CbApprove.UseVisualStyleBackColor = true;
		this.CbApprove.CheckedChanged += new System.EventHandler(CbApprove_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.CbApprove);
		base.Controls.Add(this.lblApprovedOrNot);
		base.Controls.Add(this.lblBillAmount);
		base.Controls.Add(this.lblInvoiceDate);
		base.Controls.Add(this.lblPurchaseCaption);
		base.Controls.Add(this.lblOrderNumber);
		base.Controls.Add(this.lblInvoiceNumber);
		base.Controls.Add(this.lableOrder);
		base.Controls.Add(this.label11);
		base.Controls.Add(this.lblSupplierName);
		this.Cursor = System.Windows.Forms.Cursors.Hand;
		base.Name = "InvoiceHeaderCard";
		base.Size = new System.Drawing.Size(320, 114);
		base.Load += new System.EventHandler(InvoiceHeaderCard_Load);
		base.Click += new System.EventHandler(InvoiceHeaderCard_Click);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
