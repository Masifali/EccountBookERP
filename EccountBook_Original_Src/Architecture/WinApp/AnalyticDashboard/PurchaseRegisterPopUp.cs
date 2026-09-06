using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.AnalyticDashboard;

public class PurchaseRegisterPopUp : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtDetail = new DataTable();

	private DataTable dtComparisonByItem = new DataTable();

	private DataTable dtComparisonByItemSupplier = new DataTable();

	private ReportsParameters param = new ReportsParameters();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrint;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX DataGridHistory;

	private Panel panel3;

	private Button btnShow;

	internal UltraCombo cmbActivity;

	private Label label12;

	private Label label2;

	private DateTimePicker txtToDate;

	private Label label1;

	private DateTimePicker txtDateFrom;

	public PurchaseRegisterPopUp(UserAccount userAccount, ReportsParameters Param)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
		param = Param;
	}

	private void frmEvaulationDetailSalesReports_Load(object sender, EventArgs e)
	{
		try
		{
			dtDetail.Columns.Add("ParentCategory");
			dtDetail.Columns.Add("TransactionType");
			dtDetail.Columns.Add("SupplierName");
			dtDetail.Columns.Add("BillDate");
			dtDetail.Columns.Add("BillNo");
			dtDetail.Columns.Add("GpNo");
			dtDetail.Columns.Add("GpDate");
			dtDetail.Columns.Add("GrnDate");
			dtDetail.Columns.Add("GrnNo");
			dtDetail.Columns.Add("VehicleNo");
			dtDetail.Columns.Add("ItemName");
			dtDetail.Columns.Add("UOMCode");
			dtDetail.Columns.Add("CropYear");
			dtDetail.Columns.Add("JobLotCode");
			dtDetail.Columns.Add("PackTypeCode");
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("BillWeight", typeof(double));
			dtDetail.Columns.Add("StockWeight", typeof(double));
			dtDetail.Columns.Add("ItemRate", typeof(double));
			dtDetail.Columns.Add("BillAmount", typeof(double));
			dtDetail.Columns.Add("Commission", typeof(double));
			dtDetail.Columns.Add("Freight", typeof(double));
			dtDetail.Columns.Add("OExpense", typeof(double));
			dtDetail.Columns.Add("TotalExpense", typeof(double));
			dtDetail.Columns.Add("TotalAmount", typeof(double));
			dtComparisonByItem.Columns.Add("ParentCategory");
			dtComparisonByItem.Columns.Add("ItemName");
			dtComparisonByItem.Columns.Add("CropYear");
			dtComparisonByItem.Columns.Add("ItemQty", typeof(double));
			dtComparisonByItem.Columns.Add("BillWeight", typeof(double));
			dtComparisonByItem.Columns.Add("StockWeight", typeof(double));
			dtComparisonByItem.Columns.Add("BillAmount", typeof(double));
			dtComparisonByItem.Columns.Add("ItemRate", typeof(double));
			dtComparisonByItem.Columns.Add("Commission", typeof(double));
			dtComparisonByItem.Columns.Add("Comm40Kg", typeof(double));
			dtComparisonByItem.Columns.Add("Freight", typeof(double));
			dtComparisonByItem.Columns.Add("Freight40Kg", typeof(double));
			dtComparisonByItem.Columns.Add("OExpense", typeof(double));
			dtComparisonByItem.Columns.Add("OExpense40Kg", typeof(double));
			dtComparisonByItem.Columns.Add("TotalExpense", typeof(double));
			dtComparisonByItem.Columns.Add("TotalExp40Kg", typeof(double));
			dtComparisonByItem.Columns.Add("TotalAmount", typeof(double));
			dtComparisonByItem.Columns.Add("RateWithExp", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("ParentCategory");
			dtComparisonByItemSupplier.Columns.Add("ItemName");
			dtComparisonByItemSupplier.Columns.Add("CropYear");
			dtComparisonByItemSupplier.Columns.Add("SupplierName");
			dtComparisonByItemSupplier.Columns.Add("ItemQty", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("BillWeight", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("StockWeight", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("BillAmount", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("ItemRate", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Commission", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Comm40Kg", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Freight", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Freight40Kg", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("OExpense", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("OExpense40Kg", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("TotalExpense", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("TotalExp40Kg", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("TotalAmount", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("RateWithExp", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("ItemTotal", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("ItemSupplierTotal", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("%OfTotal", typeof(double));
			txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			ActivityFill();
			((UltraGridBase)cmbActivity).Rows[0].Activate();
			GridFill();
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ActivityFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Detail");
			dt.Rows.Add(2, "Comparison By Item");
			dt.Rows.Add(3, "Comparison By Item and Supplier");
			DDL.BindDDL(dt, cmbActivity, "Id", "Name", "Activity", ZeroIndex: false);
			((UltraGridBase)cmbActivity).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		try
		{
			DataTable dtGrid = InventoryStockEvalautionDetail.PurchasesingRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Conversion.ToDateTime((object)txtDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				ReportType = Conversion.ToString((object)((Control)(object)cmbActivity).Text.Trim()),
				SupplierCustomerId = param.SupplierCustomerId,
				InventoryParentCategories = param.InventoryParentCategories
			}).Tables?[0].Copy();
			if (dtGrid.Rows.Count > 0)
			{
				if (((UltraGridBase)cmbActivity).ActiveRow != null)
				{
					if (((Control)(object)cmbActivity).Text == "Detail")
					{
						dtDetail.Rows.Clear();
						for (int k = 0; k < dtGrid.Rows.Count; k++)
						{
							dtDetail.Rows.Add(dtGrid.Rows[k]["ParentCategory"], dtGrid.Rows[k]["TransactionType"], dtGrid.Rows[k]["SupplierName"], dtGrid.Rows[k]["BillDate"], dtGrid.Rows[k]["BillNo"], dtGrid.Rows[k]["GpNo"], dtGrid.Rows[k]["GpDate"], dtGrid.Rows[k]["GrnDate"], dtGrid.Rows[k]["GrnNo"], dtGrid.Rows[k]["VehicleNo"], dtGrid.Rows[k]["ItemName"], dtGrid.Rows[k]["UOMCode"], dtGrid.Rows[k]["CropYear"], dtGrid.Rows[k]["JobLotCode"], dtGrid.Rows[k]["PackTypeCode"], dtGrid.Rows[k]["ItemQty"], dtGrid.Rows[k]["BillWeight"], dtGrid.Rows[k]["StockWeight"], dtGrid.Rows[k]["ItemRate"], dtGrid.Rows[k]["BillAmount"], dtGrid.Rows[k]["Commission"], dtGrid.Rows[k]["Freight"], dtGrid.Rows[k]["OExpense"], dtGrid.Rows[k]["TotalExpense"], dtGrid.Rows[k]["TotalAmount"]);
						}
						DataGridHistory.DataSource = dtDetail;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					if (((Control)(object)cmbActivity).Text == "Comparison By Item")
					{
						dtComparisonByItem.Rows.Clear();
						for (int j = 0; j < dtGrid.Rows.Count; j++)
						{
							dtComparisonByItem.Rows.Add(dtGrid.Rows[j]["ParentCategory"], dtGrid.Rows[j]["ItemName"], dtGrid.Rows[j]["CropYear"], dtGrid.Rows[j]["ItemQty"], dtGrid.Rows[j]["BillWeight"], dtGrid.Rows[j]["StockWeight"], dtGrid.Rows[j]["BillAmount"], dtGrid.Rows[j]["ItemRate"], dtGrid.Rows[j]["Commission"], dtGrid.Rows[j]["Comm40Kg"], dtGrid.Rows[j]["Freight"], dtGrid.Rows[j]["Freight40Kg"], dtGrid.Rows[j]["OExpense"], dtGrid.Rows[j]["OExpense40Kg"], dtGrid.Rows[j]["TotalExpense"], dtGrid.Rows[j]["TotalExp40Kg"], dtGrid.Rows[j]["TotalAmount"], dtGrid.Rows[j]["RateWithExp"]);
						}
						DataGridHistory.DataSource = dtComparisonByItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					if (((Control)(object)cmbActivity).Text == "Comparison By Item and Supplier")
					{
						dtComparisonByItemSupplier.Rows.Clear();
						for (int i = 0; i < dtGrid.Rows.Count; i++)
						{
							dtComparisonByItemSupplier.Rows.Add(dtGrid.Rows[i]["ParentCategory"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["CropYear"], dtGrid.Rows[i]["SupplierName"], dtGrid.Rows[i]["ItemQty"], dtGrid.Rows[i]["BillWeight"], dtGrid.Rows[i]["StockWeight"], dtGrid.Rows[i]["BillAmount"], dtGrid.Rows[i]["ItemRate"], dtGrid.Rows[i]["Commission"], dtGrid.Rows[i]["Comm40Kg"], dtGrid.Rows[i]["Freight"], dtGrid.Rows[i]["Freight40Kg"], dtGrid.Rows[i]["TotalExpense"], dtGrid.Rows[i]["TotalExp40Kg"], dtGrid.Rows[i]["TotalAmount"], dtGrid.Rows[i]["RateWithExp"], dtGrid.Rows[i]["ItemTotal"], dtGrid.Rows[i]["ItemSupplierTotal"], dtGrid.Rows[i]["%OfTotal"]);
						}
						DataGridHistory.DataSource = dtComparisonByItemSupplier;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
				}
				else
				{
					DataGridHistory.ClearStructure();
				}
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["StockWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			DataGridHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemRate"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["ItemRate"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillAmount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["BillAmount"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Commission"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Commission"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Freight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Freight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["OExpense"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OExpense"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OExpense"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["OExpense"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["OExpense"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OExpense"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OExpense"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["OExpense"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["TotalExpense"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalExpense"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TotalExpense"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalExpense"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalAmount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalAmount"].TotalFormatString = "0,0";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GridFill();
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrint.Text == "234-PurchaseRegisterSummary")
			{
				if (dtDetail.Rows.Count == 0 || dtDetail == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtDetail, "234-PurchaseRegisterSummary.rpt");
			}
			if (btnPrint.Text == "235-PurchaseRegisterSummaryByItem&Customer")
			{
				if (dtComparisonByItemSupplier.Rows.Count == 0 || dtComparisonByItemSupplier == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtComparisonByItemSupplier, "235-PurchaseRegisterSummaryByItem&Customer.rpt");
			}
			if (btnPrint.Text == "236-PurchaseRegisterSummaryByItem")
			{
				if (dtComparisonByItem.Rows.Count == 0 || dtComparisonByItem == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtComparisonByItem, "236-PurchaseRegisterSummaryByItem.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintButtonManage()
	{
		try
		{
			if (((UltraGridBase)cmbActivity).ActiveRow != null)
			{
				if (((Control)(object)cmbActivity).Text == "Detail")
				{
					btnPrint.Text = "234-PurchaseRegisterSummary";
				}
				if (((Control)(object)cmbActivity).Text == "Comparison By Item")
				{
					btnPrint.Text = "236-PurchaseRegisterSummaryByItem";
				}
				if (((Control)(object)cmbActivity).Text == "Comparison By Item and Supplier")
				{
					btnPrint.Text = "235-PurchaseRegisterSummaryByItem&Customer";
				}
			}
			else
			{
				btnPrint.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbActivity = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1169, 694);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 87);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1169, 607);
		this.panel4.TabIndex = 2;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistory.AlternatingColors = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1169, 607);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.btnShow);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivity);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.txtToDate);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtDateFrom);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1169, 58);
		this.panel3.TabIndex = 1;
		this.btnShow.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.Location = new System.Drawing.Point(493, 21);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(93, 26);
		this.btnShow.TabIndex = 4;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = true;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbActivity.AllowNull = (DefaultableBoolean)1;
		this.cmbActivity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Location = new System.Drawing.Point(320, 21);
		((System.Windows.Forms.Control)(object)this.cmbActivity).Name = "cmbActivity";
		((System.Windows.Forms.Control)(object)this.cmbActivity).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivity).TabIndex = 3;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(325, 1);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(47, 15);
		this.label12.TabIndex = 163;
		this.label12.Text = "Activity";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(170, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(47, 15);
		this.label2.TabIndex = 3;
		this.label2.Text = "To Date";
		this.txtToDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(170, 24);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(147, 23);
		this.txtToDate.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(22, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "From Date";
		this.txtDateFrom.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateFrom.Location = new System.Drawing.Point(22, 24);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(147, 23);
		this.txtDateFrom.TabIndex = 0;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1169, 29);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1115, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 9;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.btnPrint });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1169, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1169, 694);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PurchaseRegisterPopUp";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmEvaulationDetailSalesReports_Load);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
