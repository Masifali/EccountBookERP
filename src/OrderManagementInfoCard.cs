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

public class OrderManagementInfoCard : UserControl
{
	public class CardClickedEventArgs : EventArgs
	{
		public string ClickedLabel { get; set; }

		public CardClickedEventArgs(string clickedLabel)
		{
			ClickedLabel = clickedLabel;
		}
	}

	private string _ItemId;

	private string _ItemName;

	private string _OrderQty;

	private string _OrderWeight;

	private string _StockQty;

	private string _StockWeight;

	private string _NoOfParties;

	private string _NoOfOrders;

	private string _TranType;

	private IContainer components;

	private Label lblStockWeight;

	private Label lblStockQty;

	private Label lblOrderWeight;

	private Label lblOrderQty;

	private Label lblItemName;

	private Label lblNoOfParties;

	private Label lblNoOfOrders;

	public string ItemId
	{
		get
		{
			return _ItemId;
		}
		set
		{
			_ItemId = value;
		}
	}

	public string ItemName
	{
		get
		{
			return _ItemName;
		}
		set
		{
			_ItemName = value;
			lblItemName.Text = value;
		}
	}

	public string OrderQty
	{
		get
		{
			return _OrderQty;
		}
		set
		{
			_OrderQty = value;
			lblOrderQty.Text = value;
		}
	}

	public string OrderWeight
	{
		get
		{
			return _OrderWeight;
		}
		set
		{
			_OrderWeight = value;
			lblOrderWeight.Text = value;
		}
	}

	public string StockQty
	{
		get
		{
			return _StockQty;
		}
		set
		{
			_StockQty = value;
			lblStockQty.Text = value;
		}
	}

	public string StockWeight
	{
		get
		{
			return _StockWeight;
		}
		set
		{
			_StockWeight = value;
			lblStockWeight.Text = value;
		}
	}

	public string NoOfParties
	{
		get
		{
			return _NoOfParties;
		}
		set
		{
			_NoOfParties = value;
			lblNoOfParties.Text = value;
		}
	}

	public string NoOfOrders
	{
		get
		{
			return _NoOfOrders;
		}
		set
		{
			_NoOfOrders = value;
			lblNoOfOrders.Text = value;
		}
	}

	public string TranType
	{
		get
		{
			return _TranType;
		}
		set
		{
			_TranType = value;
		}
	}

	[Browsable(true)]
	public event EventHandler<CardClickedEventArgs> UserControlClicked;

	public OrderManagementInfoCard()
	{
		InitializeComponent();
		lblItemName.Click += delegate(object s, EventArgs e)
		{
			ControlClicked(s, e, "ItemName");
		};
		lblStockWeight.Click += delegate(object s, EventArgs e)
		{
			ControlClicked(s, e, "StockWeight");
		};
	}

	private void ControlClicked(object sender, EventArgs e, string labelName)
	{
		this.UserControlClicked?.Invoke(this, new CardClickedEventArgs(labelName));
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
		this.lblStockWeight = new System.Windows.Forms.Label();
		this.lblStockQty = new System.Windows.Forms.Label();
		this.lblOrderWeight = new System.Windows.Forms.Label();
		this.lblOrderQty = new System.Windows.Forms.Label();
		this.lblItemName = new System.Windows.Forms.Label();
		this.lblNoOfParties = new System.Windows.Forms.Label();
		this.lblNoOfOrders = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblStockWeight.AutoEllipsis = true;
		this.lblStockWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStockWeight.ForeColor = System.Drawing.Color.Blue;
		this.lblStockWeight.Location = new System.Drawing.Point(423, 4);
		this.lblStockWeight.Name = "lblStockWeight";
		this.lblStockWeight.Size = new System.Drawing.Size(78, 15);
		this.lblStockWeight.TabIndex = 15;
		this.lblStockWeight.Tag = "StockWeight";
		this.lblStockWeight.Text = "0";
		this.lblStockWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblStockQty.AutoEllipsis = true;
		this.lblStockQty.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStockQty.ForeColor = System.Drawing.Color.Black;
		this.lblStockQty.Location = new System.Drawing.Point(354, 4);
		this.lblStockQty.Name = "lblStockQty";
		this.lblStockQty.Size = new System.Drawing.Size(63, 15);
		this.lblStockQty.TabIndex = 14;
		this.lblStockQty.Text = "0";
		this.lblStockQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblOrderWeight.AutoEllipsis = true;
		this.lblOrderWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOrderWeight.ForeColor = System.Drawing.Color.Black;
		this.lblOrderWeight.Location = new System.Drawing.Point(264, 4);
		this.lblOrderWeight.Name = "lblOrderWeight";
		this.lblOrderWeight.Size = new System.Drawing.Size(82, 15);
		this.lblOrderWeight.TabIndex = 13;
		this.lblOrderWeight.Text = "0";
		this.lblOrderWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblOrderQty.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOrderQty.ForeColor = System.Drawing.Color.Black;
		this.lblOrderQty.Location = new System.Drawing.Point(203, 4);
		this.lblOrderQty.Name = "lblOrderQty";
		this.lblOrderQty.Size = new System.Drawing.Size(58, 15);
		this.lblOrderQty.TabIndex = 12;
		this.lblOrderQty.Text = "0";
		this.lblOrderQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblItemName.AutoEllipsis = true;
		this.lblItemName.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblItemName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblItemName.ForeColor = System.Drawing.Color.Blue;
		this.lblItemName.Location = new System.Drawing.Point(3, 3);
		this.lblItemName.Name = "lblItemName";
		this.lblItemName.Size = new System.Drawing.Size(197, 17);
		this.lblItemName.TabIndex = 11;
		this.lblItemName.Tag = "ItemName";
		this.lblItemName.Text = "Item Name";
		this.lblNoOfParties.AutoEllipsis = true;
		this.lblNoOfParties.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNoOfParties.ForeColor = System.Drawing.Color.Black;
		this.lblNoOfParties.Location = new System.Drawing.Point(508, 4);
		this.lblNoOfParties.Name = "lblNoOfParties";
		this.lblNoOfParties.Size = new System.Drawing.Size(44, 15);
		this.lblNoOfParties.TabIndex = 16;
		this.lblNoOfParties.Text = "0";
		this.lblNoOfParties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblNoOfOrders.AutoEllipsis = true;
		this.lblNoOfOrders.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNoOfOrders.ForeColor = System.Drawing.Color.Black;
		this.lblNoOfOrders.Location = new System.Drawing.Point(560, 4);
		this.lblNoOfOrders.Name = "lblNoOfOrders";
		this.lblNoOfOrders.Size = new System.Drawing.Size(43, 15);
		this.lblNoOfOrders.TabIndex = 17;
		this.lblNoOfOrders.Text = "0";
		this.lblNoOfOrders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.lblNoOfOrders);
		base.Controls.Add(this.lblNoOfParties);
		base.Controls.Add(this.lblStockWeight);
		base.Controls.Add(this.lblStockQty);
		base.Controls.Add(this.lblOrderWeight);
		base.Controls.Add(this.lblOrderQty);
		base.Controls.Add(this.lblItemName);
		base.Name = "OrderManagementInfoCard";
		base.Size = new System.Drawing.Size(606, 25);
		base.ResumeLayout(false);
	}
}

}
