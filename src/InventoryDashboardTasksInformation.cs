using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.AnalyticDashboard;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Dashboard;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.ExportReports;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.DashBoard
{

public class InventoryDashboardTasksInformation : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private Panel panel10;

	private Label label5;

	private Panel panel18;

	private Button btnshow;

	private Label label59;

	private Label label60;

	private DateTimePicker ToDate;

	private DateTimePicker FromDate;

	private Panel panel4;

	private Panel panel2;

	private Panel panel5;

	private Label label2;

	private Panel panel3;

	private Label label1;

	private FlowLayoutPanel flowLayoutPanel1;

	private FlowLayoutPanel flowLayoutPanel2;

	private FlowLayoutPanel flowLayoutPanel4;

	private FlowLayoutPanel flowLayoutPanel3;

	private Label label3;

	internal UltraCombo CmbDateType;

	private Label label4;

	private Label label6;

	public InventoryDashboardTasksInformation(UserAccount Puser)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void InventoryDashboardTasksInformation_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1250, 570);
			DateTypeFill();
			((UltraGridBase)CmbDateType).Rows[0].Activate();
			CmbDateType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DateTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Parameters");
			dt.Rows.Add(1, "This Day");
			dt.Rows.Add(2, "This Week");
			dt.Rows.Add(3, "This Month");
			dt.Rows.Add(4, "This Year");
			dt.Rows.Add(5, "Financial Year");
			DDL.BindDDL(dt, CmbDateType, "Id", "Parameters", "Date Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateCards();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCards()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		try
		{
			flowLayoutPanel1.Controls.Clear();
			flowLayoutPanel2.Controls.Clear();
			flowLayoutPanel3.Controls.Clear();
			flowLayoutPanel4.Controls.Clear();
			DataSet ds = Dashboard.TaskDoneStatus_Dashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDate.Value,
				ToDate = ToDate.Value
			});
			if (ds.Tables.Count <= 0)
			{
				return;
			}
			EnumerableRowCollection<DataRow> PurchaseRows = from row in ds.Tables[0].AsEnumerable()
				where row.Field<int>("TransId") == 1
				select row;
			DataTable dtPurchaseWork = (PurchaseRows.Any() ? PurchaseRows.CopyToDataTable() : ds.Tables[0].Clone());
			EnumerableRowCollection<DataRow> SaleRows = from row in ds.Tables[0].AsEnumerable()
				where row.Field<int>("TransId") == 2
				select row;
			DataTable dtSaleWork = (SaleRows.Any() ? SaleRows.CopyToDataTable() : ds.Tables[0].Clone());
			EnumerableRowCollection<DataRow> PurchaseWagesRows = from row in ds.Tables[1].AsEnumerable()
				where row.Field<int>("TransId") == 1
				select row;
			DataTable dtPurchaseWagesWork = (PurchaseWagesRows.Any() ? PurchaseWagesRows.CopyToDataTable() : ds.Tables[1].Clone());
			EnumerableRowCollection<DataRow> SaleWagesRows = from row in ds.Tables[1].AsEnumerable()
				where row.Field<int>("TransId") == 2
				select row;
			DataTable dtSaleWagesWork = (SaleWagesRows.Any() ? SaleWagesRows.CopyToDataTable() : ds.Tables[1].Clone());
			if (dtPurchaseWork.Rows.Count > 0)
			{
				InventoryTaskInfoCard1[] card1 = new InventoryTaskInfoCard1[dtPurchaseWork.Rows.Count];
				for (int k = 0; k < card1.Length; k++)
				{
					card1[k] = new InventoryTaskInfoCard1();
					card1[k].TransId = Conversion.ToInt(dtPurchaseWork.Rows[k]["TransId"]);
					card1[k].ParentId = Conversion.ToInt(dtPurchaseWork.Rows[k]["ParentId"]);
					card1[k].key = Conversion.ToString(dtPurchaseWork.Rows[k]["TransTypeKey"]);
					card1[k].Title = Conversion.ToString(dtPurchaseWork.Rows[k]["TransType"]);
					card1[k].VehicleNos = Conversion.ToDouble(dtPurchaseWork.Rows[k]["VehicleNos"]).ToString("#,##0.####");
					card1[k].Qty = Conversion.ToDouble(dtPurchaseWork.Rows[k]["Qty"]).ToString("#,##0.####");
					card1[k].Weight = Conversion.ToDouble(dtPurchaseWork.Rows[k]["Weight"]).ToString("#,##0.####");
					card1[k].UserControlClicked += UserControl_Click;
					flowLayoutPanel1.Controls.Add(card1[k]);
				}
				if (dtPurchaseWork.Rows.Count <= 3)
				{
					flowLayoutPanel1.Height = 113;
				}
				else
				{
					flowLayoutPanel1.Height = 225;
				}
			}
			if (dtSaleWork.Rows.Count > 0)
			{
				InventoryTaskInfoCard1[] card2 = new InventoryTaskInfoCard1[dtSaleWork.Rows.Count];
				for (int l = 0; l < card2.Length; l++)
				{
					card2[l] = new InventoryTaskInfoCard1();
					card2[l].TransId = Conversion.ToInt(dtSaleWork.Rows[l]["TransId"]);
					card2[l].ParentId = Conversion.ToInt(dtSaleWork.Rows[l]["ParentId"]);
					card2[l].key = Conversion.ToString(dtSaleWork.Rows[l]["TransTypeKey"]);
					card2[l].Title = Conversion.ToString(dtSaleWork.Rows[l]["TransType"]);
					card2[l].VehicleNos = Conversion.ToDouble(dtSaleWork.Rows[l]["VehicleNos"]).ToString("#,##0.####");
					card2[l].Qty = Conversion.ToDouble(dtSaleWork.Rows[l]["Qty"]).ToString("#,##0.####");
					card2[l].Weight = Conversion.ToDouble(dtSaleWork.Rows[l]["Weight"]).ToString("#,##0.####");
					card2[l].UserControlClicked += UserControl_Click;
					flowLayoutPanel3.Controls.Add(card2[l]);
				}
				if (dtSaleWork.Rows.Count <= 3)
				{
					flowLayoutPanel3.Height = 113;
				}
				else
				{
					flowLayoutPanel3.Height = 225;
				}
			}
			if (dtPurchaseWagesWork.Rows.Count > 0)
			{
				List<int> distinctIds2 = (from row in dtPurchaseWagesWork.AsEnumerable()
					select Conversion.ToInt(row["CardNo"])).Distinct().ToList();
				InventoryTaskInfoCard2ForWages[] Wagescard2 = new InventoryTaskInfoCard2ForWages[distinctIds2.Count()];
				int j;
				for (j = 0; j < Wagescard2.Length; j++)
				{
					DataTable distinctData2 = (from row in dtPurchaseWagesWork.AsEnumerable()
						where row.Field<int>("CardNo") == Conversion.ToInt((object)distinctIds2[j])
						select row).CopyToDataTable();
					Wagescard2[j] = new InventoryTaskInfoCard2ForWages();
					Wagescard2[j].TransId = Conversion.ToInt(distinctData2.Rows[0]["TransId"]);
					Wagescard2[j].DocumentTypeId = Conversion.ToInt(distinctData2.Rows[0]["RefDocumentTypeId"]);
					Wagescard2[j].Title = Conversion.ToString(distinctData2.Rows[0]["TransType"]);
					Wagescard2[j].ValuesFlowLayoutPanel.Controls.Clear();
					double TotalQty2 = 0.0;
					double totalWeight2 = 0.0;
					double totalAmount2 = 0.0;
					double AvgRate2 = 0.0;
					InventoryTaskInfoCard3ForWagesValues[] WagesValueCard2 = new InventoryTaskInfoCard3ForWagesValues[distinctData2.Rows.Count];
					for (int n = 0; n < distinctData2.Rows.Count; n++)
					{
						WagesValueCard2[n] = new InventoryTaskInfoCard3ForWagesValues();
						WagesValueCard2[n].Activity = Conversion.ToString(distinctData2.Rows[n]["Activity"]);
						TotalQty2 += Conversion.ToDouble(distinctData2.Rows[n]["Qty"]);
						WagesValueCard2[n].Qty = Conversion.ToDouble(distinctData2.Rows[n]["Qty"]).ToString("#,##0.###");
						totalWeight2 += Conversion.ToDouble(distinctData2.Rows[n]["Weight"]);
						WagesValueCard2[n].Weight = Conversion.ToDouble(distinctData2.Rows[n]["Weight"]).ToString("#,##0.###");
						AvgRate2 += Conversion.ToDouble(distinctData2.Rows[n]["AvgRate"]);
						WagesValueCard2[n].Rate = Conversion.ToDouble(distinctData2.Rows[n]["AvgRate"]).ToString("#,##0.###");
						totalAmount2 += Conversion.ToDouble(distinctData2.Rows[n]["Amount"]);
						WagesValueCard2[n].Amount = Conversion.ToDouble(distinctData2.Rows[n]["Amount"]).ToString("#,##0.###");
						Wagescard2[j].ValuesFlowLayoutPanel.Controls.Add(WagesValueCard2[n]);
						if (n > 0 && n < 3)
						{
							Wagescard2[j].ValuesFlowLayoutPanel.Height += 30;
							Wagescard2[j].Height += 30;
						}
					}
					Wagescard2[j].QtyTotal = TotalQty2.ToString("#,##0.####");
					Wagescard2[j].WeightTotal = totalWeight2.ToString("#,##0.####");
					Wagescard2[j].AvgRate = (AvgRate2 / (double)distinctData2.Rows.Count).ToString("#,##0.####");
					Wagescard2[j].AmountTotal = totalAmount2.ToString("#,##0.####");
					Wagescard2[j].UserControlClicked += WagesControl_Click;
					flowLayoutPanel2.Controls.Add(Wagescard2[j]);
				}
			}
			if (dtSaleWagesWork.Rows.Count <= 0)
			{
				return;
			}
			List<int> distinctIds = (from row in dtSaleWagesWork.AsEnumerable()
				select Conversion.ToInt(row["CardNo"])).Distinct().ToList();
			InventoryTaskInfoCard2ForWages[] Wagescard = new InventoryTaskInfoCard2ForWages[distinctIds.Count()];
			int i;
			for (i = 0; i < Wagescard.Length; i++)
			{
				DataTable distinctData = (from row in dtSaleWagesWork.AsEnumerable()
					where row.Field<int>("CardNo") == Conversion.ToInt((object)distinctIds[i])
					select row).CopyToDataTable();
				Wagescard[i] = new InventoryTaskInfoCard2ForWages();
				Wagescard[i].TransId = Conversion.ToInt(distinctData.Rows[0]["TransId"]);
				Wagescard[i].DocumentTypeId = Conversion.ToInt(distinctData.Rows[0]["RefDocumentTypeId"]);
				Wagescard[i].Title = Conversion.ToString(distinctData.Rows[0]["TransType"]);
				Wagescard[i].ValuesFlowLayoutPanel.Controls.Clear();
				double TotalQty = 0.0;
				double totalWeight = 0.0;
				double totalAmount = 0.0;
				double AvgRate = 0.0;
				InventoryTaskInfoCard3ForWagesValues[] WagesValueCard = new InventoryTaskInfoCard3ForWagesValues[distinctData.Rows.Count];
				for (int m = 0; m < distinctData.Rows.Count; m++)
				{
					WagesValueCard[m] = new InventoryTaskInfoCard3ForWagesValues();
					WagesValueCard[m].TransId = Conversion.ToInt(distinctData.Rows[m]["TransId"]);
					WagesValueCard[m].Activity = Conversion.ToString(distinctData.Rows[m]["Activity"]);
					TotalQty += Conversion.ToDouble(distinctData.Rows[m]["Qty"]);
					WagesValueCard[m].Qty = Conversion.ToDouble(distinctData.Rows[m]["Qty"]).ToString("#,##0.###");
					totalWeight += Conversion.ToDouble(distinctData.Rows[m]["Weight"]);
					WagesValueCard[m].Weight = Conversion.ToDouble(distinctData.Rows[m]["Weight"]).ToString("#,##0.###");
					totalAmount += Conversion.ToDouble(distinctData.Rows[m]["Amount"]);
					WagesValueCard[m].Amount = Conversion.ToDouble(distinctData.Rows[m]["Amount"]).ToString("#,##0.###");
					AvgRate += Conversion.ToDouble(distinctData.Rows[m]["AvgRate"]);
					WagesValueCard[m].Rate = Conversion.ToDouble(distinctData.Rows[m]["AvgRate"]).ToString("#,##0.###");
					Wagescard[i].ValuesFlowLayoutPanel.Controls.Add(WagesValueCard[m]);
					if (m > 0 && m < 3)
					{
						Wagescard[i].ValuesFlowLayoutPanel.Height += 30;
						Wagescard[i].Height += 30;
					}
				}
				Wagescard[i].QtyTotal = TotalQty.ToString("#,##0.####");
				Wagescard[i].WeightTotal = totalWeight.ToString("#,##0.####");
				Wagescard[i].AvgRate = (AvgRate / (double)distinctData.Rows.Count).ToString("#,##0.####");
				Wagescard[i].AmountTotal = totalAmount.ToString("#,##0.####");
				Wagescard[i].UserControlClicked += WagesControl_Click;
				flowLayoutPanel4.Controls.Add(Wagescard[i]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		try
		{
			InventoryTaskInfoCard1 obj = (InventoryTaskInfoCard1)sender;
			string key = obj.key;
			_ = obj.TransId;
			int ParentId = obj.ParentId;
			switch (key)
			{
			case "GRN":
			{
				frmGRNHistory frmGRNHistory = new frmGRNHistory(UserAccount);
				frmGRNHistory.RequestedByOtherDocument = true;
				frmGRNHistory.GRNfromdate.Value = FromDate.Value;
				frmGRNHistory.GRNToDate.Value = ToDate.Value;
				frmGRNHistory.ParentId = ParentId;
				frmGRNHistory.ShowDialog();
				frmGRNHistory.BringToFront();
				break;
			}
			case "GPI":
			case "GPO":
			{
				frmPendingWorksRpt frmPendingWorksRpt = new frmPendingWorksRpt(UserAccount);
				frmPendingWorksRpt.datFromDate.Value = FromDate.Value;
				frmPendingWorksRpt.datToDate.Value = ToDate.Value;
				frmPendingWorksRpt.ShowDialog();
				frmPendingWorksRpt.BringToFront();
				break;
			}
			case "PI":
			{
				PurchaseAnalytiicsDashBoard purchaseAnalytiicsDashBoard = new PurchaseAnalytiicsDashBoard(UserAccount);
				purchaseAnalytiicsDashBoard.fromdate.Value = FromDate.Value;
				purchaseAnalytiicsDashBoard.todate.Value = ToDate.Value;
				purchaseAnalytiicsDashBoard.RequestedByOtherDocument = true;
				purchaseAnalytiicsDashBoard.Tag = ((ParentId == 1) ? "frmPaddyPurchaseAnalytics" : "frmRicePurchaseAnalytics");
				purchaseAnalytiicsDashBoard.ShowDialog();
				purchaseAnalytiicsDashBoard.BringToFront();
				break;
			}
			case "GDN":
			{
				frmGDNHistory frmGDNHistory = new frmGDNHistory(UserAccount);
				frmGDNHistory.datGdnFromDate.Value = FromDate.Value;
				frmGDNHistory.dateGdnToDate.Value = ToDate.Value;
				frmGDNHistory.RequestedByOtherDocument = true;
				frmGDNHistory.ParentId = ParentId;
				frmGDNHistory.ShowDialog();
				frmGDNHistory.BringToFront();
				break;
			}
			case "SI":
			{
				SalesAnalyticsDashBoard salesAnalyticsDashBoard = new SalesAnalyticsDashBoard(UserAccount);
				salesAnalyticsDashBoard.fromdate.Value = FromDate.Value;
				salesAnalyticsDashBoard.todate.Value = ToDate.Value;
				salesAnalyticsDashBoard.RequestedByOtherDocument = true;
				salesAnalyticsDashBoard.ParentCategoryId = ParentId;
				salesAnalyticsDashBoard.ShowDialog();
				salesAnalyticsDashBoard.BringToFront();
				break;
			}
			case "Forwarding":
			{
				ExImForwardingHistory exImForwardingHistory = new ExImForwardingHistory(UserAccount);
				exImForwardingHistory.txtdatefrom.Value = FromDate.Value;
				exImForwardingHistory.txtdateto.Value = ToDate.Value;
				exImForwardingHistory.RequestedByOtherDocument = true;
				exImForwardingHistory.ShowDialog();
				exImForwardingHistory.BringToFront();
				break;
			}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesControl_Click(object sender, EventArgs e)
	{
		try
		{
			InventoryTaskInfoCard2ForWages obj = (InventoryTaskInfoCard2ForWages)sender;
			_ = obj.TransId;
			int DocumentTypeId = obj.DocumentTypeId;
			frmStockContractorWagesHistory frmStockContractorWagesHistory = new frmStockContractorWagesHistory(UserAccount);
			frmStockContractorWagesHistory.RequestedByOtherDocument = true;
			frmStockContractorWagesHistory.txtFromDate.Value = FromDate.Value;
			frmStockContractorWagesHistory.txtToDate.Value = ToDate.Value;
			frmStockContractorWagesHistory.documentType = DocumentTypeId;
			frmStockContractorWagesHistory.ShowDialog();
			frmStockContractorWagesHistory.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbDateType.Value) == 1)
			{
				FromDate.Value = DateTime.Today;
				ToDate.Value = DateTime.Today;
			}
			if (Conversion.ToInt(CmbDateType.Value) == 2)
			{
				FromDate.Value = DateTime.Today.AddDays(-7.0);
				ToDate.Value = DateTime.Today;
			}
			if (Conversion.ToInt(CmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first2 = now.Date.AddDays(-(now.Date.Day - 1));
				FromDate.Value = first2;
				ToDate.Value = DateTime.Today;
			}
			if (Conversion.ToInt(CmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first = new DateTime(year, 1, 1);
				FromDate.Value = first;
				ToDate.Value = DateTime.Today;
			}
			if (Conversion.ToInt(CmbDateType.Value) == 5)
			{
				FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InventoryDashboardTasksInformation_KeyDown(object sender, KeyEventArgs e)
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
		this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbDateType = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).BeginInit();
		this.panel10.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel18);
		this.panel1.Controls.Add(this.panel10);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1254, 671);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add(this.flowLayoutPanel4);
		this.panel4.Controls.Add(this.flowLayoutPanel3);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(611, 85);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(643, 586);
		this.panel4.TabIndex = 70;
		this.flowLayoutPanel4.AutoScroll = true;
		this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 139);
		this.flowLayoutPanel4.Name = "flowLayoutPanel4";
		this.flowLayoutPanel4.Size = new System.Drawing.Size(643, 447);
		this.flowLayoutPanel4.TabIndex = 70;
		this.flowLayoutPanel3.AutoScroll = true;
		this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 26);
		this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
		this.flowLayoutPanel3.Name = "flowLayoutPanel3";
		this.flowLayoutPanel3.Size = new System.Drawing.Size(643, 113);
		this.flowLayoutPanel3.TabIndex = 69;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
		this.panel5.Size = new System.Drawing.Size(643, 26);
		this.panel5.TabIndex = 67;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(35, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(96, 19);
		this.label2.TabIndex = 66;
		this.label2.Text = "Sale Tasks";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Tahoma", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(-24, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(32, 25);
		this.label6.TabIndex = 68;
		this.label6.Text = "\"\"";
		this.panel2.Controls.Add(this.flowLayoutPanel2);
		this.panel2.Controls.Add(this.flowLayoutPanel1);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(0, 85);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(611, 586);
		this.panel2.TabIndex = 68;
		this.flowLayoutPanel2.AutoScroll = true;
		this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 139);
		this.flowLayoutPanel2.Name = "flowLayoutPanel2";
		this.flowLayoutPanel2.Size = new System.Drawing.Size(611, 447);
		this.flowLayoutPanel2.TabIndex = 69;
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 26);
		this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(611, 113);
		this.flowLayoutPanel1.TabIndex = 68;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.ForeColor = System.Drawing.Color.White;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(611, 26);
		this.panel3.TabIndex = 67;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Tahoma", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(603, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(32, 25);
		this.label4.TabIndex = 67;
		this.label4.Text = "\"\"";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(135, 19);
		this.label1.TabIndex = 66;
		this.label1.Text = "Purchase Tasks";
		this.panel18.Controls.Add(this.label3);
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateType);
		this.panel18.Controls.Add(this.btnshow);
		this.panel18.Controls.Add(this.label59);
		this.panel18.Controls.Add(this.label60);
		this.panel18.Controls.Add(this.FromDate);
		this.panel18.Controls.Add(this.ToDate);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 53);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1254, 32);
		this.panel18.TabIndex = 67;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(9, 9);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(60, 15);
		this.label3.TabIndex = 141;
		this.label3.Text = "Date Type";
		this.CmbDateType.AllowNull = (DefaultableBoolean)1;
		this.CmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateType).Location = new System.Drawing.Point(70, 3);
		((System.Windows.Forms.Control)(object)this.CmbDateType).Name = "CmbDateType";
		((System.Windows.Forms.Control)(object)this.CmbDateType).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateType).TabIndex = 0;
		this.CmbDateType.ValueChanged += new System.EventHandler(CmbDateType_ValueChanged);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(596, 3);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(71, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(424, 9);
		this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(48, 15);
		this.label59.TabIndex = 139;
		this.label59.Text = "To Date";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(227, 9);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(63, 15);
		this.label60.TabIndex = 13;
		this.label60.Text = "From Date";
		this.FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(296, 5);
		this.FromDate.Margin = new System.Windows.Forms.Padding(4);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(122, 23);
		this.FromDate.TabIndex = 1;
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(478, 5);
		this.ToDate.Margin = new System.Windows.Forms.Padding(4);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(116, 23);
		this.ToDate.TabIndex = 2;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 27);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1254, 26);
		this.panel10.TabIndex = 66;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(4, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(103, 19);
		this.label5.TabIndex = 66;
		this.label5.Text = "Tasks Done";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1254, 27);
		this.toolStrip1.TabIndex = 27;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1254, 671);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "InventoryDashboardTasksInformation";
		this.Text = "InventoryDashboardTasksInformation";
		base.Load += new System.EventHandler(InventoryDashboardTasksInformation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InventoryDashboardTasksInformation_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateType).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
