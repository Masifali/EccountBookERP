using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.DashBoard;
using Architecture.WinApp.Inventory_Stocks_Report;
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

public class OrderManageMentCardHeader : UserControl
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable _dt;

	private IContainer components;

	private Panel panel1;

	private FlowLayoutPanel flowLayout;

	private Panel panel3;

	private Label label9;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label5;

	private Panel panel2;

	private Label lblTitle;

	private Label label2;

	private Label label1;

	public DataTable dataTable
	{
		get
		{
			return _dt;
		}
		set
		{
			_dt = value;
		}
	}

	public OrderManageMentCardHeader(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void OrderManageMentCardHeader_Load(object sender, EventArgs e)
	{
		try
		{
			flowLayout.Controls.Clear();
			if (dataTable.Rows.Count > 0)
			{
				lblTitle.Text = Conversion.ToString(dataTable.Rows[0]["ActivityDescription"]);
				OrderManagementInfoCard[] CardsList = new OrderManagementInfoCard[dataTable.Rows.Count];
				for (int i = 0; i < CardsList.Length; i++)
				{
					CardsList[i] = new OrderManagementInfoCard();
					CardsList[i].TranType = Conversion.ToString(dataTable.Rows[i]["TranType"]);
					CardsList[i].ItemId = Conversion.ToString(dataTable.Rows[i]["ItemId"]);
					CardsList[i].ItemName = Conversion.ToString(dataTable.Rows[i]["ItemName"]);
					double oqty = Conversion.ToDouble(dataTable.Rows[i]["OrderBalanceQty"]);
					CardsList[i].OrderQty = oqty.ToString("#,#;(#,#);0");
					double oWeight = Conversion.ToDouble(dataTable.Rows[i]["OrderBalalanceWeight"]);
					CardsList[i].OrderWeight = oWeight.ToString("#,#;(#,#);0");
					double sQty = Conversion.ToDouble(dataTable.Rows[i]["StockBalanceQty"]);
					CardsList[i].StockQty = sQty.ToString("#,#;(#,#);0");
					double sWeight = Conversion.ToDouble(dataTable.Rows[i]["StockBalanceWeight"]);
					CardsList[i].StockWeight = sWeight.ToString("#,#;(#,#);0");
					double Parties = Conversion.ToDouble(dataTable.Rows[i]["NoOfParties"]);
					CardsList[i].NoOfParties = Parties.ToString();
					double orders = Conversion.ToDouble(dataTable.Rows[i]["OrderCount"]);
					CardsList[i].NoOfOrders = orders.ToString();
					flowLayout.Controls.Add(CardsList[i]);
					CardsList[i].UserControlClicked += UserControl_Click;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click(object sender, OrderManagementInfoCard.CardClickedEventArgs e)
	{
		try
		{
			OrderManagementInfoCard obj = (OrderManagementInfoCard)sender;
			int ItemId = Conversion.ToInt((object)obj.ItemId);
			if (ItemId <= 0)
			{
				return;
			}
			if (e.ClickedLabel == "ItemName")
			{
				if (obj.TranType == "Purchase")
				{
					OrderManagemantPopUpForPurchaseSale orderManagemantPopUpForPurchaseSale = new OrderManagemantPopUpForPurchaseSale(UserAccount);
					orderManagemantPopUpForPurchaseSale.ItemId = ItemId;
					orderManagemantPopUpForPurchaseSale.InvoiceType = 1;
					orderManagemantPopUpForPurchaseSale.Show();
				}
				else
				{
					OrderManagemantPopUpForPurchaseSale orderManagemantPopUpForPurchaseSale2 = new OrderManagemantPopUpForPurchaseSale(UserAccount);
					orderManagemantPopUpForPurchaseSale2.ItemId = ItemId;
					orderManagemantPopUpForPurchaseSale2.InvoiceType = 2;
					orderManagemantPopUpForPurchaseSale2.Show();
				}
			}
			else if (e.ClickedLabel == "StockWeight" && Conversion.ToDouble((object)obj.StockWeight) > 0.0)
			{
				List<tblUserRights> rights = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "RptGenrateStocks" || x.ScreenName.Trim() == "frmStockReportWithValues").ToList();
				if (!rights.Any() || !rights.Any((tblUserRights r) => Conversion.ToBool((object)r.Value.ToString())))
				{
					MessageBox.Show("You don't have the right");
					return;
				}
				frmStockReportWithValues frmStockReportWithValues = new frmStockReportWithValues(UserAccount);
				frmStockReportWithValues.cmbItem.Value = obj.ItemId;
				frmStockReportWithValues.Show();
				frmStockReportWithValues.GridBind();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.flowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lblTitle = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.flowLayout);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(615, 286);
		this.panel1.TabIndex = 1;
		this.flowLayout.AutoScroll = true;
		this.flowLayout.Location = new System.Drawing.Point(1, 72);
		this.flowLayout.Name = "flowLayout";
		this.flowLayout.Size = new System.Drawing.Size(612, 213);
		this.flowLayout.TabIndex = 2;
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Location = new System.Drawing.Point(0, 32);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(612, 40);
		this.panel3.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Teal;
		this.label9.Location = new System.Drawing.Point(437, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(53, 32);
		this.label9.TabIndex = 10;
		this.label9.Text = "Stock \r\nWeight";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Teal;
		this.label8.Location = new System.Drawing.Point(351, 11);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(70, 16);
		this.label8.TabIndex = 9;
		this.label8.Text = "Stock Qty";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Teal;
		this.label7.Location = new System.Drawing.Point(282, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(53, 32);
		this.label7.TabIndex = 8;
		this.label7.Text = "Order \r\nWeight";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Teal;
		this.label6.Location = new System.Drawing.Point(197, 11);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(71, 16);
		this.label6.TabIndex = 7;
		this.label6.Text = "Order Qty";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Teal;
		this.label5.Location = new System.Drawing.Point(8, 11);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(76, 16);
		this.label5.TabIndex = 6;
		this.label5.Text = "Item Name";
		this.panel2.Controls.Add(this.lblTitle);
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(612, 31);
		this.panel2.TabIndex = 0;
		this.lblTitle.BackColor = System.Drawing.Color.Teal;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.ForeColor = System.Drawing.Color.White;
		this.lblTitle.Location = new System.Drawing.Point(0, 0);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(612, 31);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "Title";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Teal;
		this.label1.Location = new System.Drawing.Point(504, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(53, 32);
		this.label1.TabIndex = 11;
		this.label1.Text = "No. Of \r\nParties";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Teal;
		this.label2.Location = new System.Drawing.Point(560, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(51, 32);
		this.label2.TabIndex = 12;
		this.label2.Text = "No. Of \r\nOrders";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.panel1);
		base.Name = "OrderManageMentCardHeader";
		base.Size = new System.Drawing.Size(616, 289);
		base.Load += new System.EventHandler(OrderManageMentCardHeader_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
