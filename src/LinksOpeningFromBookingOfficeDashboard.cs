// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Janus.Data;
using Janus.Windows.GridEX;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.PreBookingAndDelivery
{

public class LinksOpeningFromBookingOfficeDashboard : Form
{
	public int SortNo;

	public int CostCenterId;

	public DateTime FromDate;

	public DateTime ToDate;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel PanelForCaptionIncludingBooking;

	private Panel CardsPanel;

	private Panel panel5;

	private Label label1;

	private Panel panel8;

	private Label lblAmount;

	private Label label9;

	private Panel panel7;

	private Label lblWeight;

	private Label label7;

	private Panel panel6;

	private Label lblQty;

	private Label label5;

	private Label lblCustomers;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private TabPage tabPage5;

	private TabPage tabPage6;

	internal GridEX grdItem;

	internal GridEX grdCustomer;

	internal GridEX grdItemCustomer;

	internal GridEX grdCustomerItem;

	internal GridEX grdItemPack;

	internal GridEX grdPackItem;

	private Panel panelTransitSale;

	internal GridEX grdSaleOrTransit;

	public LinksOpeningFromBookingOfficeDashboard(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void MyOrdersStatus_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 650);
			base.StartPosition = FormStartPosition.CenterScreen;
			if (SortNo == 1)
			{
				label23.Text = "Pending Orders";
				PanelForCaptionIncludingBooking.Visible = true;
				panelTransitSale.Visible = false;
				PanelForCaptionIncludingBooking.Dock = DockStyle.Fill;
				CardsPanel.Visible = true;
				GetBookingDemandData();
			}
			else if (SortNo == 2)
			{
				label23.Text = "Confirm Orders";
				PanelForCaptionIncludingBooking.Visible = true;
				panelTransitSale.Visible = false;
				PanelForCaptionIncludingBooking.Dock = DockStyle.Fill;
				CardsPanel.Visible = true;
				GetPendingOrdersData();
			}
			else if (SortNo == 3)
			{
				label23.Text = "Loading In-Process";
				lblCustomers.Text = "0";
				lblQty.Text = "0";
				lblWeight.Text = "0";
				lblAmount.Text = "0";
				PanelForCaptionIncludingBooking.Visible = false;
				CardsPanel.Visible = false;
				panelTransitSale.Visible = true;
				GetDeliveryInProcessData();
			}
			else if (SortNo == 4)
			{
				label23.Text = "Delivery In Transit";
				lblCustomers.Text = "0";
				lblQty.Text = "0";
				lblWeight.Text = "0";
				lblAmount.Text = "0";
				PanelForCaptionIncludingBooking.Visible = false;
				CardsPanel.Visible = false;
				panelTransitSale.Visible = true;
				GetDeliveryInTransitData();
			}
			else if (SortNo == 5)
			{
				label23.Text = "Pending Delivery For Bills";
				lblCustomers.Text = "0";
				lblQty.Text = "0";
				lblWeight.Text = "0";
				lblAmount.Text = "0";
				panelTransitSale.Visible = true;
				PanelForCaptionIncludingBooking.Visible = false;
				CardsPanel.Visible = false;
				GetPendingDeliveryForBillsData();
			}
			else if (SortNo == 6)
			{
				label23.Text = "Sales Bill Confirmation";
				lblCustomers.Text = "0";
				lblQty.Text = "0";
				lblWeight.Text = "0";
				lblAmount.Text = "0";
				panelTransitSale.Visible = true;
				PanelForCaptionIncludingBooking.Visible = false;
				CardsPanel.Visible = false;
				GetSaleBillsData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetBookingDemandData()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		try
		{
			grdItem.ClearStructure();
			grdCustomer.ClearStructure();
			grdItemCustomer.ClearStructure();
			grdCustomerItem.ClearStructure();
			grdPackItem.ClearStructure();
			grdItemPack.ClearStructure();
			DataSet ds = PreBookingOrder.PreBookingOrderDashboard_Detail(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FromDate = FromDate,
				ToDate = ToDate,
				AppId = UserAccount.AppId,
				CostCenterId = CostCenterId
			});
			if (ds.Tables.Count <= 0)
			{
				return;
			}
			if (ds.Tables[0].Rows.Count > 0)
			{
				int Customers = Conversion.ToInt(ds.Tables[0].Rows[0]["TotalCustomers"]);
				double Qty = Conversion.ToDouble(ds.Tables[0].Rows[0]["Qty"]);
				double Weight = Conversion.ToDouble(ds.Tables[0].Rows[0]["Weight"]);
				double Amount = Conversion.ToDouble(ds.Tables[0].Rows[0]["Amount"]);
				lblCustomers.Text = Customers.ToString("#,##0.###");
				lblQty.Text = Qty.ToString("#,##0.###");
				lblWeight.Text = Weight.ToString("#,##0.###");
				lblAmount.Text = Amount.ToString("#,##0.###");
			}
			tabControl1.SelectedIndex = 0;
			DataTable dtItem = ds.Tables[1];
			if (dtItem.Rows.Count > 0)
			{
				DataTable dt6 = new DataTable();
				dt6.Columns.Add("Id", typeof(int));
				dt6.Columns.Add("ItemName", typeof(string));
				dt6.Columns.Add("Qty", typeof(double));
				dt6.Columns.Add("Weight", typeof(double));
				dt6.Columns.Add("Amount", typeof(double));
				for (int n = 0; n < dtItem.Rows.Count; n++)
				{
					dt6.Rows.Add(dtItem.Rows[n]["OrderItemId"], dtItem.Rows[n]["ItemName"], dtItem.Rows[n]["Qty"], dtItem.Rows[n]["Weight"], dtItem.Rows[n]["Amount"]);
				}
				grdItem.DataSource = dt6;
				grdItem.RetrieveStructure();
				grdItem.ColumnAutoResize = true;
				grdItem.RootTable.Columns["Id"].Visible = false;
				grdItem.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdItem.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdItem.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdItem.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdItem.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdItem.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdItem.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdItem.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdItem.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdItem.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdItem.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdItem.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdItem.RootTable.Columns["ItemName"].Width = 350;
				grdItem.RootTable.Columns["Qty"].Width = 100;
				grdItem.RootTable.Columns["Weight"].Width = 110;
				grdItem.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtCustomer = ds.Tables[2];
			if (dtCustomer.Rows.Count > 0)
			{
				DataTable dt5 = new DataTable();
				dt5.Columns.Add("Id", typeof(int));
				dt5.Columns.Add("CustomerName", typeof(string));
				dt5.Columns.Add("Qty", typeof(double));
				dt5.Columns.Add("Weight", typeof(double));
				dt5.Columns.Add("Amount", typeof(double));
				for (int m = 0; m < dtCustomer.Rows.Count; m++)
				{
					dt5.Rows.Add(dtCustomer.Rows[m]["OrderSupCustId"], dtCustomer.Rows[m]["CustomerName"], dtCustomer.Rows[m]["Qty"], dtCustomer.Rows[m]["Weight"], dtCustomer.Rows[m]["Amount"]);
				}
				grdCustomer.DataSource = dt5;
				grdCustomer.RetrieveStructure();
				grdCustomer.ColumnAutoResize = true;
				grdCustomer.RootTable.Columns["Id"].Visible = false;
				grdCustomer.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdCustomer.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdCustomer.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdCustomer.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdCustomer.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdCustomer.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdCustomer.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdCustomer.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdCustomer.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdCustomer.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdCustomer.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdCustomer.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdCustomer.RootTable.Columns["CustomerName"].Width = 350;
				grdCustomer.RootTable.Columns["Qty"].Width = 100;
				grdCustomer.RootTable.Columns["Weight"].Width = 110;
				grdCustomer.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtCustomerItem = ds.Tables[4];
			if (dtCustomerItem.Rows.Count > 0)
			{
				DataTable dt4 = new DataTable();
				dt4.Columns.Add("ItemId", typeof(int));
				dt4.Columns.Add("ItemName", typeof(string));
				dt4.Columns.Add("SuppCustId", typeof(int));
				dt4.Columns.Add("CustomerName", typeof(string));
				dt4.Columns.Add("Qty", typeof(double));
				dt4.Columns.Add("Weight", typeof(double));
				dt4.Columns.Add("Amount", typeof(double));
				for (int l = 0; l < dtCustomerItem.Rows.Count; l++)
				{
					dt4.Rows.Add(dtCustomerItem.Rows[l]["OrderItemId"], dtCustomerItem.Rows[l]["ItemName"], dtCustomerItem.Rows[l]["OrderSupCustId"], dtCustomerItem.Rows[l]["CustomerName"], dtCustomerItem.Rows[l]["Qty"], dtCustomerItem.Rows[l]["Weight"], dtCustomerItem.Rows[l]["Amount"]);
				}
				grdCustomerItem.DataSource = dt4;
				grdCustomerItem.RetrieveStructure();
				grdCustomer.ColumnAutoResize = true;
				grdCustomerItem.GroupTotals = (GroupTotals)2;
				grdCustomerItem.RootTable.Groups.Add("CustomerName");
				grdCustomerItem.HideColumnsWhenGrouped = (InheritableBoolean)1;
				grdCustomerItem.RootTable.Columns["ItemId"].Visible = false;
				grdCustomerItem.RootTable.Columns["SuppCustId"].Visible = false;
				grdCustomerItem.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdCustomerItem.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdCustomerItem.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdCustomerItem.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdCustomerItem.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdCustomerItem.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdCustomerItem.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdCustomerItem.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdCustomerItem.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdCustomerItem.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdCustomerItem.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdCustomerItem.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdCustomerItem.RootTable.Columns["ItemName"].Width = 350;
				grdCustomerItem.RootTable.Columns["Qty"].Width = 100;
				grdCustomerItem.RootTable.Columns["Weight"].Width = 110;
				grdCustomerItem.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtItemCustomer = ds.Tables[4];
			if (dtCustomerItem.Rows.Count > 0)
			{
				DataTable dt3 = new DataTable();
				dt3.Columns.Add("ItemId", typeof(int));
				dt3.Columns.Add("ItemName", typeof(string));
				dt3.Columns.Add("SuppCustId", typeof(int));
				dt3.Columns.Add("CustomerName", typeof(string));
				dt3.Columns.Add("Qty", typeof(double));
				dt3.Columns.Add("Weight", typeof(double));
				dt3.Columns.Add("Amount", typeof(double));
				for (int k = 0; k < dtItemCustomer.Rows.Count; k++)
				{
					dt3.Rows.Add(dtItemCustomer.Rows[k]["OrderItemId"], dtItemCustomer.Rows[k]["ItemName"], dtItemCustomer.Rows[k]["OrderSupCustId"], dtItemCustomer.Rows[k]["CustomerName"], dtItemCustomer.Rows[k]["Qty"], dtItemCustomer.Rows[k]["Weight"], dtItemCustomer.Rows[k]["Amount"]);
				}
				grdItemCustomer.DataSource = dt3;
				grdItemCustomer.RetrieveStructure();
				grdItemCustomer.ColumnAutoResize = true;
				grdItemCustomer.GroupTotals = (GroupTotals)2;
				grdItemCustomer.RootTable.Groups.Add("ItemName");
				grdItemCustomer.HideColumnsWhenGrouped = (InheritableBoolean)1;
				grdItemCustomer.RootTable.Columns["ItemId"].Visible = false;
				grdItemCustomer.RootTable.Columns["SuppCustId"].Visible = false;
				grdItemCustomer.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdItemCustomer.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdItemCustomer.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdItemCustomer.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdItemCustomer.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdItemCustomer.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdItemCustomer.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdItemCustomer.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdItemCustomer.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdItemCustomer.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdItemCustomer.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdItemCustomer.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdItemCustomer.RootTable.Columns["CustomerName"].Width = 350;
				grdItemCustomer.RootTable.Columns["Qty"].Width = 100;
				grdItemCustomer.RootTable.Columns["Weight"].Width = 110;
				grdItemCustomer.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtItemPack = ds.Tables[3];
			if (dtItemPack.Rows.Count > 0)
			{
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("ItemId", typeof(int));
				dt2.Columns.Add("ItemName", typeof(string));
				dt2.Columns.Add("UomId", typeof(int));
				dt2.Columns.Add("PackUom", typeof(string));
				dt2.Columns.Add("Qty", typeof(double));
				dt2.Columns.Add("Weight", typeof(double));
				dt2.Columns.Add("Amount", typeof(double));
				for (int j = 0; j < dtItemPack.Rows.Count; j++)
				{
					dt2.Rows.Add(dtItemPack.Rows[j]["OrderItemId"], dtItemPack.Rows[j]["ItemName"], dtItemPack.Rows[j]["OrderItemUOMId"], dtItemPack.Rows[j]["PackUom"], dtItemPack.Rows[j]["Qty"], dtItemPack.Rows[j]["Weight"], dtItemPack.Rows[j]["Amount"]);
				}
				grdItemPack.DataSource = dt2;
				grdItemPack.RetrieveStructure();
				grdItemPack.ColumnAutoResize = true;
				grdItemPack.GroupTotals = (GroupTotals)2;
				grdItemPack.RootTable.Groups.Add("ItemName");
				grdItemPack.HideColumnsWhenGrouped = (InheritableBoolean)1;
				grdItemPack.RootTable.Columns["ItemId"].Visible = false;
				grdItemPack.RootTable.Columns["UomId"].Visible = false;
				grdItemPack.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdItemPack.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdItemPack.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdItemPack.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdItemPack.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdItemPack.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdItemPack.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdItemPack.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdItemPack.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdItemPack.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdItemPack.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdItemPack.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdItemPack.RootTable.Columns["PackUom"].Width = 350;
				grdItemPack.RootTable.Columns["Qty"].Width = 100;
				grdItemPack.RootTable.Columns["Weight"].Width = 110;
				grdItemPack.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtPackItem = ds.Tables[3];
			if (dtItemPack.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("UomId", typeof(int));
				dt.Columns.Add("PackUom", typeof(string));
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				for (int i = 0; i < dtPackItem.Rows.Count; i++)
				{
					dt.Rows.Add(dtPackItem.Rows[i]["OrderItemId"], dtPackItem.Rows[i]["ItemName"], dtPackItem.Rows[i]["OrderItemUOMId"], dtPackItem.Rows[i]["PackUom"], dtPackItem.Rows[i]["Qty"], dtPackItem.Rows[i]["Weight"], dtPackItem.Rows[i]["Amount"]);
				}
				grdPackItem.DataSource = dt;
				grdPackItem.RetrieveStructure();
				grdPackItem.ColumnAutoResize = true;
				grdPackItem.GroupTotals = (GroupTotals)2;
				grdPackItem.RootTable.Groups.Add("PackUom");
				grdPackItem.HideColumnsWhenGrouped = (InheritableBoolean)1;
				grdPackItem.RootTable.Columns["ItemId"].Visible = false;
				grdPackItem.RootTable.Columns["UomId"].Visible = false;
				grdPackItem.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdPackItem.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdPackItem.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdPackItem.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdPackItem.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdPackItem.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdPackItem.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdPackItem.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdPackItem.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdPackItem.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdPackItem.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdPackItem.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdPackItem.RootTable.Columns["ItemName"].Width = 350;
				grdPackItem.RootTable.Columns["Qty"].Width = 100;
				grdPackItem.RootTable.Columns["Weight"].Width = 110;
				grdPackItem.RootTable.Columns["Amount"].Width = 110;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPendingOrdersData()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		try
		{
			grdItem.ClearStructure();
			grdCustomer.ClearStructure();
			grdItemCustomer.ClearStructure();
			grdCustomerItem.ClearStructure();
			grdPackItem.ClearStructure();
			grdItemPack.ClearStructure();
			DataSet ds = PreBookingOrder.OutstandingOrders_PreDashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FromDate = FromDate,
				ToDate = ToDate,
				AppId = UserAccount.AppId,
				CostCenterId = CostCenterId
			});
			if (ds.Tables.Count <= 0)
			{
				return;
			}
			if (ds.Tables[0].Rows.Count > 0)
			{
				DataTable dataTable = ds.Tables[0];
				int Customers = Conversion.ToInt(dataTable.Rows[0]["TotalCustomers"]);
				double Qty = Conversion.ToDouble(dataTable.Rows[0]["Qty"]);
				double Weight = Conversion.ToDouble(dataTable.Rows[0]["Weight"]);
				double Amount = Conversion.ToDouble(dataTable.Rows[0]["Amount"]);
				lblCustomers.Text = Customers.ToString("#,##0.###");
				lblQty.Text = Qty.ToString("#,##0.###");
				lblWeight.Text = Weight.ToString("#,##0.###");
				lblAmount.Text = Amount.ToString("#,##0.###");
			}
			tabControl1.SelectedIndex = 0;
			DataTable dtItem = ds.Tables[1];
			if (dtItem.Rows.Count > 0)
			{
				DataTable dt6 = new DataTable();
				dt6.Columns.Add("Id", typeof(int));
				dt6.Columns.Add("ItemName", typeof(string));
				dt6.Columns.Add("Qty", typeof(double));
				dt6.Columns.Add("Weight", typeof(double));
				dt6.Columns.Add("Amount", typeof(double));
				for (int n = 0; n < dtItem.Rows.Count; n++)
				{
					dt6.Rows.Add(dtItem.Rows[n]["OrderItemId"], dtItem.Rows[n]["ItemName"], dtItem.Rows[n]["Qty"], dtItem.Rows[n]["Weight"], dtItem.Rows[n]["Amount"]);
				}
				grdItem.DataSource = dt6;
				grdItem.RetrieveStructure();
				grdItem.ColumnAutoResize = true;
				grdItem.RootTable.Columns["Id"].Visible = false;
				grdItem.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdItem.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdItem.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdItem.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdItem.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdItem.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdItem.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdItem.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdItem.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdItem.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdItem.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdItem.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdItem.RootTable.Columns["ItemName"].Width = 350;
				grdItem.RootTable.Columns["Qty"].Width = 100;
				grdItem.RootTable.Columns["Weight"].Width = 110;
				grdItem.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtCustomer = ds.Tables[2];
			if (dtCustomer.Rows.Count > 0)
			{
				DataTable dt5 = new DataTable();
				dt5.Columns.Add("Id", typeof(int));
				dt5.Columns.Add("CustomerName", typeof(string));
				dt5.Columns.Add("Qty", typeof(double));
				dt5.Columns.Add("Weight", typeof(double));
				dt5.Columns.Add("Amount", typeof(double));
				for (int m = 0; m < dtCustomer.Rows.Count; m++)
				{
					dt5.Rows.Add(dtCustomer.Rows[m]["OrderSupCustId"], dtCustomer.Rows[m]["CustomerName"], dtCustomer.Rows[m]["Qty"], dtCustomer.Rows[m]["Weight"], dtCustomer.Rows[m]["Amount"]);
				}
				grdCustomer.DataSource = dt5;
				grdCustomer.RetrieveStructure();
				grdCustomer.ColumnAutoResize = true;
				grdCustomer.RootTable.Columns["Id"].Visible = false;
				grdCustomer.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdCustomer.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdCustomer.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdCustomer.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdCustomer.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdCustomer.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdCustomer.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdCustomer.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdCustomer.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdCustomer.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdCustomer.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdCustomer.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdCustomer.RootTable.Columns["CustomerName"].Width = 350;
				grdCustomer.RootTable.Columns["Qty"].Width = 100;
				grdCustomer.RootTable.Columns["Weight"].Width = 110;
				grdCustomer.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtItemPack = ds.Tables[3];
			if (dtItemPack.Rows.Count > 0)
			{
				DataTable dt4 = new DataTable();
				dt4.Columns.Add("ItemId", typeof(int));
				dt4.Columns.Add("ItemName", typeof(string));
				dt4.Columns.Add("UomId", typeof(int));
				dt4.Columns.Add("PackUom", typeof(string));
				dt4.Columns.Add("Qty", typeof(double));
				dt4.Columns.Add("Weight", typeof(double));
				dt4.Columns.Add("Amount", typeof(double));
				for (int l = 0; l < dtItemPack.Rows.Count; l++)
				{
					dt4.Rows.Add(dtItemPack.Rows[l]["OrderItemId"], dtItemPack.Rows[l]["ItemName"], dtItemPack.Rows[l]["OrderItemUOMId"], dtItemPack.Rows[l]["PackUom"], dtItemPack.Rows[l]["Qty"], dtItemPack.Rows[l]["Weight"], dtItemPack.Rows[l]["Amount"]);
				}
				grdItemPack.DataSource = dt4;
				grdItemPack.RetrieveStructure();
				grdItemPack.ColumnAutoResize = true;
				grdItemPack.GroupTotals = (GroupTotals)2;
				grdItemPack.RootTable.Groups.Add("ItemName");
				grdItemPack.HideColumnsWhenGrouped = (InheritableBoolean)1;
				grdItemPack.RootTable.Columns["ItemId"].Visible = false;
				grdItemPack.RootTable.Columns["UomId"].Visible = false;
				grdItemPack.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdItemPack.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdItemPack.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdItemPack.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdItemPack.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdItemPack.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdItemPack.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdItemPack.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdItemPack.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdItemPack.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdItemPack.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdItemPack.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdItemPack.RootTable.Columns["PackUom"].Width = 350;
				grdItemPack.RootTable.Columns["Qty"].Width = 100;
				grdItemPack.RootTable.Columns["Weight"].Width = 110;
				grdItemPack.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtPackItem = dtItemPack.Copy();
			if (dtPackItem.Rows.Count > 0)
			{
				DataTable dt3 = new DataTable();
				dt3.Columns.Add("ItemId", typeof(int));
				dt3.Columns.Add("ItemName", typeof(string));
				dt3.Columns.Add("UomId", typeof(int));
				dt3.Columns.Add("PackUom", typeof(string));
				dt3.Columns.Add("Qty", typeof(double));
				dt3.Columns.Add("Weight", typeof(double));
				dt3.Columns.Add("Amount", typeof(double));
				for (int k = 0; k < dtPackItem.Rows.Count; k++)
				{
					dt3.Rows.Add(dtPackItem.Rows[k]["OrderItemId"], dtPackItem.Rows[k]["ItemName"], dtPackItem.Rows[k]["OrderItemUOMId"], dtPackItem.Rows[k]["PackUom"], dtPackItem.Rows[k]["Qty"], dtPackItem.Rows[k]["Weight"], dtPackItem.Rows[k]["Amount"]);
				}
				grdPackItem.DataSource = dt3;
				grdPackItem.RetrieveStructure();
				grdPackItem.ColumnAutoResize = true;
				grdPackItem.GroupTotals = (GroupTotals)2;
				grdPackItem.RootTable.Groups.Add("PackUom");
				grdPackItem.HideColumnsWhenGrouped = (InheritableBoolean)1;
				grdPackItem.RootTable.Columns["ItemId"].Visible = false;
				grdPackItem.RootTable.Columns["UomId"].Visible = false;
				grdPackItem.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdPackItem.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdPackItem.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdPackItem.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdPackItem.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdPackItem.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdPackItem.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdPackItem.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdPackItem.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdPackItem.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdPackItem.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdPackItem.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdPackItem.RootTable.Columns["ItemName"].Width = 350;
				grdPackItem.RootTable.Columns["Qty"].Width = 100;
				grdPackItem.RootTable.Columns["Weight"].Width = 110;
				grdPackItem.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtCustomerItem = ds.Tables[4];
			if (dtCustomerItem.Rows.Count > 0)
			{
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("ItemId", typeof(int));
				dt2.Columns.Add("SuppCustId", typeof(int));
				dt2.Columns.Add("ItemName", typeof(string));
				dt2.Columns.Add("CustomerName", typeof(string));
				dt2.Columns.Add("Qty", typeof(double));
				dt2.Columns.Add("Weight", typeof(double));
				dt2.Columns.Add("Amount", typeof(double));
				for (int j = 0; j < dtCustomerItem.Rows.Count; j++)
				{
					dt2.Rows.Add(dtCustomerItem.Rows[j]["OrderItemId"], dtCustomerItem.Rows[j]["OrderSupCustId"], dtCustomerItem.Rows[j]["ItemName"], dtCustomerItem.Rows[j]["CustomerName"], dtCustomerItem.Rows[j]["Qty"], dtCustomerItem.Rows[j]["Weight"], dtCustomerItem.Rows[j]["Amount"]);
				}
				grdCustomerItem.DataSource = dt2;
				grdCustomerItem.RetrieveStructure();
				grdCustomerItem.ColumnAutoResize = true;
				grdCustomerItem.GroupTotals = (GroupTotals)2;
				grdCustomerItem.RootTable.Groups.Add("CustomerName");
				grdCustomerItem.HideColumnsWhenGrouped = (InheritableBoolean)1;
				grdCustomerItem.RootTable.Columns["ItemId"].Visible = false;
				grdCustomerItem.RootTable.Columns["SuppCustId"].Visible = false;
				grdCustomerItem.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdCustomerItem.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdCustomerItem.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdCustomerItem.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdCustomerItem.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdCustomerItem.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdCustomerItem.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdCustomerItem.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdCustomerItem.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdCustomerItem.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdCustomerItem.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdCustomerItem.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdCustomerItem.RootTable.Columns["ItemName"].Width = 350;
				grdCustomerItem.RootTable.Columns["Qty"].Width = 100;
				grdCustomerItem.RootTable.Columns["Weight"].Width = 110;
				grdCustomerItem.RootTable.Columns["Amount"].Width = 110;
			}
			DataTable dtItemCustomer = dtCustomerItem.Copy();
			if (dtItemCustomer.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("SuppCustId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				for (int i = 0; i < dtItemCustomer.Rows.Count; i++)
				{
					dt.Rows.Add(dtItemCustomer.Rows[i]["OrderItemId"], dtItemCustomer.Rows[i]["OrderSupCustId"], dtItemCustomer.Rows[i]["ItemName"], dtItemCustomer.Rows[i]["CustomerName"], dtItemCustomer.Rows[i]["Qty"], dtItemCustomer.Rows[i]["Weight"], dtItemCustomer.Rows[i]["Amount"]);
				}
				grdItemCustomer.DataSource = dt;
				grdItemCustomer.RetrieveStructure();
				grdItemCustomer.ColumnAutoResize = true;
				grdItemCustomer.GroupTotals = (GroupTotals)2;
				grdItemCustomer.RootTable.Groups.Add("ItemName");
				grdItemCustomer.HideColumnsWhenGrouped = (InheritableBoolean)1;
				grdItemCustomer.RootTable.Columns["ItemId"].Visible = false;
				grdItemCustomer.RootTable.Columns["SuppCustId"].Visible = false;
				grdItemCustomer.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdItemCustomer.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdItemCustomer.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdItemCustomer.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdItemCustomer.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdItemCustomer.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdItemCustomer.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdItemCustomer.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdItemCustomer.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdItemCustomer.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdItemCustomer.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdItemCustomer.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdItemCustomer.RootTable.Columns["CustomerName"].Width = 350;
				grdItemCustomer.RootTable.Columns["Qty"].Width = 100;
				grdItemCustomer.RootTable.Columns["Weight"].Width = 110;
				grdItemCustomer.RootTable.Columns["Amount"].Width = 110;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDeliveryInProcessData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvGdn.getLoadingInProcessDataForMobileApp(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDate,
				ToDate = ToDate,
				UserId = UserAccount.ID,
				AppId = UserAccount.AppId,
				CostCenterId = CostCenterId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable transit = new DataTable();
				transit.Columns.Add("Id", typeof(int));
				transit.Columns.Add("DoNo", typeof(int));
				transit.Columns.Add("SupplierCustomerId", typeof(int));
				transit.Columns.Add("CustomerName", typeof(string));
				transit.Columns.Add("DocDate", typeof(DateTime));
				transit.Columns.Add("OutDatetime", typeof(DateTime));
				transit.Columns.Add("GpId", typeof(int));
				transit.Columns.Add("GpNo", typeof(int));
				transit.Columns.Add("DriverName", typeof(string));
				transit.Columns.Add("DriverCell", typeof(string));
				transit.Columns.Add("CarriageAmount", typeof(double));
				transit.Columns.Add("VehicleNo", typeof(string));
				transit.Columns.Add("Qty", typeof(double));
				transit.Columns.Add("Weight", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					transit.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocNo"], dt.Rows[i]["SupplierCustomerId"], dt.Rows[i]["CustomerName"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]), Conversion.ToDateTime(dt.Rows[i]["OutTime"]), dt.Rows[i]["GpId"], dt.Rows[i]["GpNo"], dt.Rows[i]["DriverName"], dt.Rows[i]["DriverCellNo"], dt.Rows[i]["CarriageAmount"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["ItemQty"], dt.Rows[i]["NetWeight"]);
				}
				grdSaleOrTransit.DataSource = transit;
				grdSaleOrTransit.RetrieveStructure();
				grdSaleOrTransit.AllowEdit = (InheritableBoolean)2;
				grdSaleOrTransit.AllowDelete = (InheritableBoolean)2;
				grdSaleOrTransit.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)grdSaleOrTransit.RootTable.Columns)
				{
					item.HeaderStyle.TextAlignment = (TextAlignment)2;
				}
				grdSaleOrTransit.RootTable.Columns["Id"].Visible = false;
				grdSaleOrTransit.RootTable.Columns["GpId"].Visible = false;
				grdSaleOrTransit.RootTable.Columns["SupplierCustomerId"].Visible = false;
				grdSaleOrTransit.RootTable.Columns["DocDate"].Visible = false;
				grdSaleOrTransit.RootTable.Columns["OutDatetime"].FormatString = "dd-MMM-yy hh:mm tt";
				grdSaleOrTransit.RootTable.Columns["DoNo"].Width = Constants.InventoryConstants.DoNoConstant;
				grdSaleOrTransit.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
				grdSaleOrTransit.RootTable.Columns["OutDatetime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
				grdSaleOrTransit.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
				grdSaleOrTransit.RootTable.Columns["DriverName"].Width = Constants.InventoryConstants.DriverName;
				grdSaleOrTransit.RootTable.Columns["DriverCell"].Width = Constants.InventoryConstants.DriverCellNoConstant;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdSaleOrTransit.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
				grdSaleOrTransit.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
				grdSaleOrTransit.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
				grdSaleOrTransit.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["DoNo"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["CustomerName"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["OutDatetime"].FilterEditType = (FilterEditType)5;
				grdSaleOrTransit.RootTable.Columns["GpNo"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["DriverName"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["DriverCell"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["VehicleNo"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdSaleOrTransit.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns.Add("Do_Slip");
				grdSaleOrTransit.RootTable.Columns["Do_Slip"].Caption = "Do_Slip";
				grdSaleOrTransit.RootTable.Columns["Do_Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdSaleOrTransit.RootTable.Columns["Do_Slip"].ButtonStyle = (ButtonStyle)5;
				grdSaleOrTransit.RootTable.Columns["Do_Slip"].ButtonText = "Do_Slip";
				grdSaleOrTransit.RootTable.Columns["Do_Slip"].Width = 60;
				grdSaleOrTransit.RootTable.Columns.Add("GpSlip");
				grdSaleOrTransit.RootTable.Columns["GpSlip"].Caption = "Gp_Slip";
				grdSaleOrTransit.RootTable.Columns["GpSlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdSaleOrTransit.RootTable.Columns["GpSlip"].ButtonStyle = (ButtonStyle)5;
				grdSaleOrTransit.RootTable.Columns["GpSlip"].ButtonText = "Gp_Slip";
				grdSaleOrTransit.RootTable.Columns["GpSlip"].Width = 60;
				grdSaleOrTransit.RootTable.Columns["Do_Slip"].Position = 0;
				grdSaleOrTransit.RootTable.Columns["GpSlip"].Position = 1;
				grdSaleOrTransit.FrozenColumns = 2;
			}
			else
			{
				grdSaleOrTransit.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDeliveryInTransitData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvGdn.getGdnDataForMobileApp(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDate,
				ToDate = ToDate,
				UserId = UserAccount.ID,
				AppId = UserAccount.AppId,
				CostCenterId = CostCenterId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable transit = new DataTable();
				transit.Columns.Add("Id", typeof(int));
				transit.Columns.Add("InDatetime", typeof(DateTime));
				transit.Columns.Add("OutDatetime", typeof(DateTime));
				transit.Columns.Add("GpNo", typeof(int));
				transit.Columns.Add("Customer", typeof(string));
				transit.Columns.Add("DriverName", typeof(string));
				transit.Columns.Add("DriverCell", typeof(string));
				transit.Columns.Add("CarriageAmount", typeof(double));
				transit.Columns.Add("VehicleNo", typeof(string));
				transit.Columns.Add("Qty", typeof(double));
				transit.Columns.Add("Weight", typeof(double));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					transit.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["InTime"], dt.Rows[i]["OutTime"], dt.Rows[i]["GpNo"], dt.Rows[i]["CustomerName"], dt.Rows[i]["DriverName"], dt.Rows[i]["DriverCellNo"], dt.Rows[i]["CarriageAmount"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["ItemQty"], dt.Rows[i]["NetWeight"]);
				}
				grdSaleOrTransit.DataSource = transit;
				grdSaleOrTransit.RetrieveStructure();
				grdSaleOrTransit.AllowEdit = (InheritableBoolean)2;
				grdSaleOrTransit.AllowDelete = (InheritableBoolean)2;
				grdSaleOrTransit.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)grdSaleOrTransit.RootTable.Columns)
				{
					item.HeaderStyle.TextAlignment = (TextAlignment)2;
				}
				grdSaleOrTransit.RootTable.Columns["Id"].Visible = false;
				grdSaleOrTransit.RootTable.Columns["Id"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["OutDatetime"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["InDateTime"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["GpNo"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["Customer"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["DriverName"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["DriverCell"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["Qty"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["Weight"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["InDatetime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
				grdSaleOrTransit.RootTable.Columns["OutDatetime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
				grdSaleOrTransit.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName;
				grdSaleOrTransit.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
				grdSaleOrTransit.RootTable.Columns["DriverName"].Width = Constants.InventoryConstants.DriverName;
				grdSaleOrTransit.RootTable.Columns["DriverCell"].Width = Constants.InventoryConstants.DriverCellNoConstant;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdSaleOrTransit.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
				grdSaleOrTransit.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
				grdSaleOrTransit.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
				grdSaleOrTransit.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["OutDatetime"].FilterEditType = (FilterEditType)5;
				grdSaleOrTransit.RootTable.Columns["InDatetime"].FilterEditType = (FilterEditType)5;
				grdSaleOrTransit.RootTable.Columns["GpNo"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["Customer"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["DriverName"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["DriverCell"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["VehicleNo"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["OutDatetime"].FormatString = "dd-MMM-yy hh:mm tt";
				grdSaleOrTransit.RootTable.Columns["InDatetime"].FormatString = "dd-MMM-yy hh:mm tt";
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdSaleOrTransit.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			}
			else
			{
				grdSaleOrTransit.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPendingDeliveryForBillsData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvSaleInvoice.getPendingDeliveryForBills(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDate,
				ToDate = ToDate,
				UserId = UserAccount.ID,
				AppId = UserAccount.AppId,
				CostCenterId = CostCenterId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable transit = new DataTable();
				transit.Columns.Add("Id", typeof(int));
				transit.Columns.Add("GdnNo", typeof(int));
				transit.Columns.Add("GdnDate", typeof(DateTime));
				transit.Columns.Add("Customer", typeof(string));
				transit.Columns.Add("GpNo", typeof(int));
				transit.Columns.Add("InTime", typeof(DateTime));
				transit.Columns.Add("OutTime", typeof(DateTime));
				transit.Columns.Add("VehicleNo", typeof(string));
				transit.Columns.Add("DriverName", typeof(string));
				transit.Columns.Add("DriverCellNo", typeof(string));
				transit.Columns.Add("CarriageAmount", typeof(double));
				transit.Columns.Add("Qty", typeof(double));
				transit.Columns.Add("Weight", typeof(double));
				transit.Columns.Add("GdnStatus", typeof(string));
				transit.Columns.Add("CustomerRemarks", typeof(string));
				transit.Columns.Add("EntryDate", typeof(DateTime));
				transit.Columns.Add("EntryUser", typeof(string));
				transit.Columns.Add("ModifyDate", typeof(DateTime));
				transit.Columns.Add("ModifyUser", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					transit.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocNo"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["CustomerName"], dt.Rows[i]["GpNo"], dt.Rows[i]["InTime"], dt.Rows[i]["OutTime"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["DriverName"], dt.Rows[i]["DriverCellNo"], dt.Rows[i]["CarriageAmount"], dt.Rows[i]["ItemQty"], dt.Rows[i]["NetWeight"], dt.Rows[i]["GdnStatus"], dt.Rows[i]["CustomerRemarks"], dt.Rows[i]["EntryDate"], dt.Rows[i]["EntryUser"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["ModifyUser"]);
				}
				grdSaleOrTransit.DataSource = transit;
				grdSaleOrTransit.RetrieveStructure();
				grdSaleOrTransit.AllowEdit = (InheritableBoolean)2;
				grdSaleOrTransit.AllowDelete = (InheritableBoolean)2;
				grdSaleOrTransit.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)grdSaleOrTransit.RootTable.Columns)
				{
					item.HeaderStyle.TextAlignment = (TextAlignment)2;
				}
				grdSaleOrTransit.RootTable.Columns["Id"].Visible = false;
				grdSaleOrTransit.RootTable.Columns["InTime"].FormatString = "dd-MMM-yy hh:mm tt";
				grdSaleOrTransit.RootTable.Columns["OutTime"].FormatString = "dd-MMM-yy hh:mm tt";
				grdSaleOrTransit.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
				grdSaleOrTransit.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
				grdSaleOrTransit.RootTable.Columns["GdnNo"].Width = Constants.InventoryConstants.DocNoConstant;
				grdSaleOrTransit.RootTable.Columns["GdnDate"].Width = Constants.InventoryConstants.DateConstant;
				grdSaleOrTransit.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName;
				grdSaleOrTransit.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
				grdSaleOrTransit.RootTable.Columns["InTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
				grdSaleOrTransit.RootTable.Columns["OutTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
				grdSaleOrTransit.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
				grdSaleOrTransit.RootTable.Columns["DriverName"].Width = Constants.InventoryConstants.DriverName;
				grdSaleOrTransit.RootTable.Columns["DriverCellNo"].Width = Constants.InventoryConstants.DriverCellNoConstant;
				grdSaleOrTransit.RootTable.Columns["CarriageAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdSaleOrTransit.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
				grdSaleOrTransit.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
				grdSaleOrTransit.RootTable.Columns["GdnStatus"].Width = Constants.InventoryConstants.StatusConstant;
				grdSaleOrTransit.RootTable.Columns["CustomerRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
				grdSaleOrTransit.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
				grdSaleOrTransit.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
				grdSaleOrTransit.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
				grdSaleOrTransit.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
				grdSaleOrTransit.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns.Add("Slip");
				grdSaleOrTransit.RootTable.Columns["Slip"].Caption = "Slip";
				grdSaleOrTransit.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdSaleOrTransit.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
				grdSaleOrTransit.RootTable.Columns["Slip"].ButtonText = "Slip";
				grdSaleOrTransit.RootTable.Columns["Slip"].Width = 40;
				grdSaleOrTransit.RootTable.Columns["Slip"].Position = 0;
				grdSaleOrTransit.FrozenColumns = 1;
			}
			else
			{
				grdSaleOrTransit.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSaleBillsData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			DataTable dt = InvSaleInvoice.getSaleDataForMobileApp(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDate,
				ToDate = ToDate,
				UserId = UserAccount.ID,
				AppId = UserAccount.AppId,
				CostCenterId = CostCenterId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable transit = new DataTable();
				transit.Columns.Add("Id", typeof(int));
				transit.Columns.Add("DocumentTypeId", typeof(int));
				transit.Columns.Add("BillNo", typeof(int));
				transit.Columns.Add("BillDate", typeof(DateTime));
				transit.Columns.Add("DueDays", typeof(int));
				transit.Columns.Add("DueDate", typeof(DateTime));
				transit.Columns.Add("Qty", typeof(double));
				transit.Columns.Add("Weight", typeof(double));
				transit.Columns.Add("BillAmount", typeof(double));
				transit.Columns.Add("ApprovedStatus", typeof(string));
				transit.Columns.Add("CustomerRemarks", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					transit.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["DocNo"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["DueDays"], Conversion.ToDateTime(dt.Rows[i]["DueDate"]).ToShortDateString(), dt.Rows[i]["ItemQty"], dt.Rows[i]["ItemWeight"], dt.Rows[i]["BillAmount"], dt.Rows[i]["ApprovedStatus"], "");
				}
				grdSaleOrTransit.DataSource = transit;
				grdSaleOrTransit.RetrieveStructure();
				grdSaleOrTransit.RootTable.Columns["Id"].Visible = false;
				grdSaleOrTransit.RootTable.Columns["DocumentTypeId"].Visible = false;
				grdSaleOrTransit.RootTable.Columns["Id"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["BillNo"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["BillDate"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["DueDays"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["DueDate"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["Qty"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["Weight"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["BillAmount"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["ApprovedStatus"].EditType = (EditType)0;
				grdSaleOrTransit.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["DocumentTypeId"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["BillNo"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["BillDate"].FilterEditType = (FilterEditType)6;
				grdSaleOrTransit.RootTable.Columns["DueDays"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["DueDate"].FilterEditType = (FilterEditType)6;
				grdSaleOrTransit.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["BillAmount"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["ApprovedStatus"].FilterEditType = (FilterEditType)1;
				grdSaleOrTransit.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdSaleOrTransit.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdSaleOrTransit.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["Qty"].FormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				grdSaleOrTransit.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdSaleOrTransit.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				grdSaleOrTransit.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				CommonServices.GridAutoAdjustmentNew(grdSaleOrTransit);
				grdSaleOrTransit.RootTable.Columns.Add("SlipBill");
				grdSaleOrTransit.RootTable.Columns["SlipBill"].Caption = "Slip";
				grdSaleOrTransit.RootTable.Columns["SlipBill"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdSaleOrTransit.RootTable.Columns["SlipBill"].ButtonStyle = (ButtonStyle)5;
				grdSaleOrTransit.RootTable.Columns["SlipBill"].ButtonText = "Slip";
				grdSaleOrTransit.RootTable.Columns["SlipBill"].Width = 40;
				grdSaleOrTransit.RootTable.Columns.Add("ConfirmBill");
				grdSaleOrTransit.RootTable.Columns["ConfirmBill"].Caption = "Confirm";
				grdSaleOrTransit.RootTable.Columns["ConfirmBill"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdSaleOrTransit.RootTable.Columns["ConfirmBill"].ButtonStyle = (ButtonStyle)5;
				grdSaleOrTransit.RootTable.Columns["ConfirmBill"].ButtonText = "Confirm";
				grdSaleOrTransit.RootTable.Columns["ConfirmBill"].Width = 60;
				grdSaleOrTransit.RootTable.Columns["SlipBill"].Position = 0;
				grdSaleOrTransit.RootTable.Columns["ConfirmBill"].Position = 1;
				grdSaleOrTransit.FrozenColumns = 2;
			}
			else
			{
				grdSaleOrTransit.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleOrTransit_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdSaleOrTransit.CurrentRow;
			if (row == null || (int)row.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(row.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				CommonServices.InvGdnSlip260(Id);
			}
			else if (e.Column.Key == "Do_Slip")
			{
				CommonServices.InvDeliveryOrderSlip(Id, 84);
			}
			else if (e.Column.Key == "GpSlip")
			{
				int GpId = Conversion.ToInt(row.Cells["GpId"].Value);
				DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = GpId
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "290-InvRptOutwardGatePassSlip.rpt");
			}
			else if (e.Column.Key == "Confirm")
			{
				string Remarks = Conversion.ToString(row.Cells["CustomerRemarks"].Value);
				if (Remarks == null || Remarks == string.Empty)
				{
					MessageBox.Show("Remarks Required!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				InvGdn.GdnStatusUpdate(Id, Remarks);
				MessageBox.Show("Transit Confirmed Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				GetDeliveryInTransitData();
			}
			else if (e.Column.Key == "SlipBill")
			{
				switch (Conversion.ToInt(row.Cells["DocumentTypeId"].Value))
				{
				case 95:
				{
					int SupCustId = Conversion.ToInt(row.Cells["SupplierCustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(row.Cells["DocDate"].Value);
					CommonServices.SaleInvoicetSlip_301(Id, SupCustId, 0, Date, Date);
					break;
				}
				case 99:
					CommonServices.SaleInvoiceDirectPartySlip_294(Id);
					break;
				}
			}
			else if (e.Column.Key == "ConfirmBill")
			{
				string Remarks2 = Conversion.ToString(row.Cells["CustomerRemarks"].Value);
				if (Remarks2 == null || Remarks2 == string.Empty)
				{
					MessageBox.Show("Remarks Required!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				InvSaleInvoice.InvoiceStatusUpdate(Id, Remarks2);
				MessageBox.Show("Bill Confirmed Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				GetSaleBillsData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MyOrdersStatus_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_16f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fe: Expected O, but got Unknown
		GridEXLayout grdItem_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdCustomer_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdItemCustomer_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdCustomerItem_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdItemPack_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackItem_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSaleOrTransit_DesignTimeLayout = new GridEXLayout();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.PanelForCaptionIncludingBooking = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdItem = new GridEX();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdCustomer = new GridEX();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdItemCustomer = new GridEX();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdCustomerItem = new GridEX();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdItemPack = new GridEX();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdPackItem = new GridEX();
		this.CardsPanel = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.lblAmount = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.lblWeight = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.lblQty = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblCustomers = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panelTransitSale = new System.Windows.Forms.Panel();
		this.grdSaleOrTransit = new GridEX();
		this.panel3.SuspendLayout();
		this.PanelForCaptionIncludingBooking.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItem).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomer).BeginInit();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItemCustomer).BeginInit();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerItem).BeginInit();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItemPack).BeginInit();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackItem).BeginInit();
		this.CardsPanel.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panelTransitSale.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrTransit).BeginInit();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1234, 28);
		this.panel3.TabIndex = 2;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(152, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "My Order's Status";
		this.PanelForCaptionIncludingBooking.Controls.Add(this.tabControl1);
		this.PanelForCaptionIncludingBooking.Controls.Add(this.CardsPanel);
		this.PanelForCaptionIncludingBooking.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelForCaptionIncludingBooking.Location = new System.Drawing.Point(0, 28);
		this.PanelForCaptionIncludingBooking.Name = "PanelForCaptionIncludingBooking";
		this.PanelForCaptionIncludingBooking.Size = new System.Drawing.Size(1234, 256);
		this.PanelForCaptionIncludingBooking.TabIndex = 5;
		this.PanelForCaptionIncludingBooking.Visible = false;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Controls.Add(this.tabPage5);
		this.tabControl1.Controls.Add(this.tabPage6);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 101);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(734, 155);
		this.tabControl1.TabIndex = 1;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdItem);
		this.tabPage1.Location = new System.Drawing.Point(4, 23);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(726, 128);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Item";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdItem.AllowEdit = (InheritableBoolean)2;
		grdItem_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdItem.DesignTimeLayout = grdItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdItem).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdItem.DynamicFiltering = true;
		this.grdItem.FilterMode = (FilterMode)2;
		this.grdItem.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdItem).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItem.GroupByBoxVisible = false;
		this.grdItem.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdItem).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdItem).Name = "grdItem";
		((ScrollableBase)this.grdItem).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItem).Size = new System.Drawing.Size(720, 122);
		((System.Windows.Forms.Control)(object)this.grdItem).TabIndex = 2;
		this.grdItem.TotalRow = (InheritableBoolean)1;
		this.grdItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItem).VisualStyle = (VisualStyle)4;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomer);
		this.tabPage2.Location = new System.Drawing.Point(4, 23);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(726, 128);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Customer";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdCustomer.AllowEdit = (InheritableBoolean)2;
		grdCustomer_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomer.DesignTimeLayout = grdCustomer_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomer).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdCustomer.DynamicFiltering = true;
		this.grdCustomer.FilterMode = (FilterMode)2;
		this.grdCustomer.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCustomer).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomer.GroupByBoxVisible = false;
		this.grdCustomer.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdCustomer).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdCustomer).Name = "grdCustomer";
		((ScrollableBase)this.grdCustomer).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomer).Size = new System.Drawing.Size(720, 122);
		((System.Windows.Forms.Control)(object)this.grdCustomer).TabIndex = 2;
		this.grdCustomer.TotalRow = (InheritableBoolean)1;
		this.grdCustomer.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomer).VisualStyle = (VisualStyle)4;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdItemCustomer);
		this.tabPage3.Location = new System.Drawing.Point(4, 23);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(726, 128);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Item_Customer";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdItemCustomer.AllowEdit = (InheritableBoolean)2;
		grdItemCustomer_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdItemCustomer.DesignTimeLayout = grdItemCustomer_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdItemCustomer).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdItemCustomer.DynamicFiltering = true;
		this.grdItemCustomer.FilterMode = (FilterMode)2;
		this.grdItemCustomer.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdItemCustomer).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItemCustomer.GroupByBoxVisible = false;
		this.grdItemCustomer.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdItemCustomer).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdItemCustomer).Name = "grdItemCustomer";
		((ScrollableBase)this.grdItemCustomer).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItemCustomer).Size = new System.Drawing.Size(726, 128);
		((System.Windows.Forms.Control)(object)this.grdItemCustomer).TabIndex = 2;
		this.grdItemCustomer.TotalRow = (InheritableBoolean)1;
		this.grdItemCustomer.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItemCustomer).VisualStyle = (VisualStyle)4;
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerItem);
		this.tabPage4.Location = new System.Drawing.Point(4, 23);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(726, 128);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "Customer_Item";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdCustomerItem.AllowEdit = (InheritableBoolean)2;
		grdCustomerItem_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerItem.DesignTimeLayout = grdCustomerItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerItem).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdCustomerItem.DynamicFiltering = true;
		this.grdCustomerItem.FilterMode = (FilterMode)2;
		this.grdCustomerItem.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCustomerItem).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerItem.GroupByBoxVisible = false;
		this.grdCustomerItem.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdCustomerItem).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdCustomerItem).Name = "grdCustomerItem";
		((ScrollableBase)this.grdCustomerItem).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerItem).Size = new System.Drawing.Size(726, 128);
		((System.Windows.Forms.Control)(object)this.grdCustomerItem).TabIndex = 2;
		this.grdCustomerItem.TotalRow = (InheritableBoolean)1;
		this.grdCustomerItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerItem).VisualStyle = (VisualStyle)4;
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdItemPack);
		this.tabPage5.Location = new System.Drawing.Point(4, 23);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(726, 128);
		this.tabPage5.TabIndex = 4;
		this.tabPage5.Text = "Item_Pack";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.grdItemPack.AllowEdit = (InheritableBoolean)2;
		grdItemPack_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdItemPack.DesignTimeLayout = grdItemPack_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdItemPack).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdItemPack.DynamicFiltering = true;
		this.grdItemPack.FilterMode = (FilterMode)2;
		this.grdItemPack.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdItemPack).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItemPack.GroupByBoxVisible = false;
		this.grdItemPack.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdItemPack).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdItemPack).Name = "grdItemPack";
		((ScrollableBase)this.grdItemPack).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItemPack).Size = new System.Drawing.Size(726, 128);
		((System.Windows.Forms.Control)(object)this.grdItemPack).TabIndex = 3;
		this.grdItemPack.TotalRow = (InheritableBoolean)1;
		this.grdItemPack.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItemPack).VisualStyle = (VisualStyle)4;
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackItem);
		this.tabPage6.Location = new System.Drawing.Point(4, 23);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(726, 128);
		this.tabPage6.TabIndex = 5;
		this.tabPage6.Text = "Pack_Item";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.grdPackItem.AllowEdit = (InheritableBoolean)2;
		grdPackItem_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackItem.DesignTimeLayout = grdPackItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackItem).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackItem.DynamicFiltering = true;
		this.grdPackItem.FilterMode = (FilterMode)2;
		this.grdPackItem.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackItem).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackItem.GroupByBoxVisible = false;
		this.grdPackItem.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPackItem).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPackItem).Name = "grdPackItem";
		((ScrollableBase)this.grdPackItem).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackItem).Size = new System.Drawing.Size(726, 128);
		((System.Windows.Forms.Control)(object)this.grdPackItem).TabIndex = 3;
		this.grdPackItem.TotalRow = (InheritableBoolean)1;
		this.grdPackItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackItem).VisualStyle = (VisualStyle)4;
		this.CardsPanel.Controls.Add(this.panel8);
		this.CardsPanel.Controls.Add(this.panel7);
		this.CardsPanel.Controls.Add(this.panel6);
		this.CardsPanel.Controls.Add(this.panel5);
		this.CardsPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.CardsPanel.Location = new System.Drawing.Point(0, 0);
		this.CardsPanel.Name = "CardsPanel";
		this.CardsPanel.Size = new System.Drawing.Size(1234, 101);
		this.CardsPanel.TabIndex = 0;
		this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel8.Controls.Add(this.lblAmount);
		this.panel8.Controls.Add(this.label9);
		this.panel8.Location = new System.Drawing.Point(565, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(131, 94);
		this.panel8.TabIndex = 138;
		this.lblAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmount.Location = new System.Drawing.Point(3, 56);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(123, 22);
		this.lblAmount.TabIndex = 135;
		this.lblAmount.Text = "0";
		this.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(129, 38);
		this.label9.TabIndex = 133;
		this.label9.Text = "Amount";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel7.Controls.Add(this.lblWeight);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Location = new System.Drawing.Point(385, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(131, 94);
		this.panel7.TabIndex = 137;
		this.lblWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWeight.Location = new System.Drawing.Point(3, 56);
		this.lblWeight.Name = "lblWeight";
		this.lblWeight.Size = new System.Drawing.Size(123, 22);
		this.lblWeight.TabIndex = 135;
		this.lblWeight.Text = "0";
		this.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(129, 38);
		this.label7.TabIndex = 133;
		this.label7.Text = "Weight";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel6.Controls.Add(this.lblQty);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Location = new System.Drawing.Point(205, 5);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(131, 94);
		this.panel6.TabIndex = 136;
		this.lblQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblQty.Location = new System.Drawing.Point(3, 56);
		this.lblQty.Name = "lblQty";
		this.lblQty.Size = new System.Drawing.Size(123, 22);
		this.lblQty.TabIndex = 135;
		this.lblQty.Text = "0";
		this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(0, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(129, 38);
		this.label5.TabIndex = 133;
		this.label5.Text = "Qty";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel5.Controls.Add(this.lblCustomers);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Location = new System.Drawing.Point(25, 4);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(131, 94);
		this.panel5.TabIndex = 0;
		this.lblCustomers.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCustomers.Location = new System.Drawing.Point(3, 56);
		this.lblCustomers.Name = "lblCustomers";
		this.lblCustomers.Size = new System.Drawing.Size(123, 22);
		this.lblCustomers.TabIndex = 135;
		this.lblCustomers.Text = "0";
		this.lblCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(129, 38);
		this.label1.TabIndex = 133;
		this.label1.Text = "Customer's";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panelTransitSale.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleOrTransit);
		this.panelTransitSale.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelTransitSale.Location = new System.Drawing.Point(0, 284);
		this.panelTransitSale.Name = "panelTransitSale";
		this.panelTransitSale.Size = new System.Drawing.Size(1234, 389);
		this.panelTransitSale.TabIndex = 6;
		this.panelTransitSale.Visible = false;
		grdSaleOrTransit_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSaleOrTransit.DesignTimeLayout = grdSaleOrTransit_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSaleOrTransit).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSaleOrTransit.DynamicFiltering = true;
		this.grdSaleOrTransit.FilterMode = (FilterMode)2;
		this.grdSaleOrTransit.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSaleOrTransit).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleOrTransit.GroupByBoxVisible = false;
		this.grdSaleOrTransit.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSaleOrTransit).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSaleOrTransit).Name = "grdSaleOrTransit";
		((ScrollableBase)this.grdSaleOrTransit).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleOrTransit).Size = new System.Drawing.Size(1234, 389);
		((System.Windows.Forms.Control)(object)this.grdSaleOrTransit).TabIndex = 4;
		this.grdSaleOrTransit.TotalRow = (InheritableBoolean)1;
		this.grdSaleOrTransit.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleOrTransit).VisualStyle = (VisualStyle)4;
		this.grdSaleOrTransit.ColumnButtonClick += new ColumnActionEventHandler(grdSaleOrTransit_ColumnButtonClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1234, 673);
		base.Controls.Add(this.panelTransitSale);
		base.Controls.Add(this.PanelForCaptionIncludingBooking);
		base.Controls.Add(this.panel3);
		base.Name = "LinksOpeningFromBookingOfficeDashboard";
		this.Text = "MyOrdersStatus";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(MyOrdersStatus_FormClosed);
		base.Load += new System.EventHandler(MyOrdersStatus_Load);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.PanelForCaptionIncludingBooking.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdItem).EndInit();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomer).EndInit();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdItemCustomer).EndInit();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerItem).EndInit();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdItemPack).EndInit();
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackItem).EndInit();
		this.CardsPanel.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panelTransitSale.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrTransit).EndInit();
		base.ResumeLayout(false);
	}
}

}
