using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Model;
using Architecture.WinApp.DynamicCards;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Dashboard
{

public class OrderManagementDashboard : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label label1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel6;

	private Panel panel8;

	private Label label2;

	private Panel panel5;

	private Panel panel7;

	private Label label3;

	private FlowLayoutPanel PurchaseFlowLayout;

	private FlowLayoutPanel SaleFlowLayout;

	public OrderManagementDashboard(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void OrderManagementDashboard_Load(object sender, EventArgs e)
	{
		try
		{
			GetDataAndGenerateCards();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataAndGenerateCards()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			PurchaseFlowLayout.Controls.Clear();
			DataTable dt = Dashboard.SPU_Invetnory_OrderManagementDashBoards(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ToDate = DateTime.Now
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<string>("TranType") == "Purchase"
				select r;
			DataTable dtPurchase = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
			if (dtPurchase.Rows.Count > 0)
			{
				OrderManageMentCardHeader[] CardsList = new OrderManageMentCardHeader[dtPurchase.AsEnumerable().Max((DataRow r) => r.Field<int>("CardCount"))];
				int j;
				for (j = 0; j < CardsList.Length; j++)
				{
					CardsList[j] = new OrderManageMentCardHeader(UserAccount);
					DataTable dtAtI2 = (from r in dtPurchase.AsEnumerable()
						where r.Field<int>("CardCount") == j + 1
						select r).CopyToDataTable();
					CardsList[j].dataTable = dtAtI2;
					PurchaseFlowLayout.Controls.Add(CardsList[j]);
				}
			}
			EnumerableRowCollection<DataRow> RowsForSale = from r in dt.AsEnumerable()
				where r.Field<string>("TranType") == "Sales"
				select r;
			DataTable dtSale = (RowsForSale.Any() ? RowsForSale.CopyToDataTable() : dt.Clone());
			if (dtSale.Rows.Count <= 0)
			{
				return;
			}
			OrderManageMentCardHeader[] CardsListForSale = new OrderManageMentCardHeader[dtSale.AsEnumerable().Max((DataRow r) => r.Field<int>("CardCount"))];
			int i;
			for (i = 0; i < CardsListForSale.Length; i++)
			{
				CardsListForSale[i] = new OrderManageMentCardHeader(UserAccount);
				DataTable dtAtI = (from r in dtSale.AsEnumerable()
					where r.Field<int>("CardCount") == i + 1
					select r).CopyToDataTable();
				CardsListForSale[i].dataTable = dtAtI;
				SaleFlowLayout.Controls.Add(CardsListForSale[i]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderManagementDashboard_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
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
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.SaleFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.PurchaseFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1276, 713);
		this.panel1.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 42);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1276, 671);
		this.panel4.TabIndex = 3;
		this.panel6.Controls.Add(this.SaleFlowLayout);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(633, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(639, 671);
		this.panel6.TabIndex = 1;
		this.SaleFlowLayout.AutoScroll = true;
		this.SaleFlowLayout.BackColor = System.Drawing.Color.White;
		this.SaleFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.SaleFlowLayout.Location = new System.Drawing.Point(0, 39);
		this.SaleFlowLayout.Name = "SaleFlowLayout";
		this.SaleFlowLayout.Size = new System.Drawing.Size(639, 632);
		this.SaleFlowLayout.TabIndex = 6;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.label2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.ForeColor = System.Drawing.Color.Black;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(639, 39);
		this.panel8.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(211, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(155, 32);
		this.label2.TabIndex = 0;
		this.label2.Text = "Sale Orders";
		this.panel5.Controls.Add(this.PurchaseFlowLayout);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(633, 671);
		this.panel5.TabIndex = 0;
		this.PurchaseFlowLayout.AutoScroll = true;
		this.PurchaseFlowLayout.BackColor = System.Drawing.Color.White;
		this.PurchaseFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PurchaseFlowLayout.Location = new System.Drawing.Point(0, 39);
		this.PurchaseFlowLayout.Name = "PurchaseFlowLayout";
		this.PurchaseFlowLayout.Size = new System.Drawing.Size(633, 632);
		this.PurchaseFlowLayout.TabIndex = 5;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.label3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(633, 39);
		this.panel7.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(153, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(219, 32);
		this.label3.TabIndex = 0;
		this.label3.Text = "Purchase Orders";
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1276, 42);
		this.panel2.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(426, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(317, 30);
		this.label1.TabIndex = 0;
		this.label1.Text = "Order Management Dashboard";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1276, 713);
		base.Controls.Add(this.panel1);
		base.Name = "OrderManagementDashboard";
		this.Text = "Order Management Dashboard";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OrderManagementDashboard_FormClosing);
		base.Load += new System.EventHandler(OrderManagementDashboard_Load);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
