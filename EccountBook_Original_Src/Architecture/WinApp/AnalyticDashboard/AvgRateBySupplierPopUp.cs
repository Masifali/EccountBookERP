using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.AnalyticDashboard;

public class AvgRateBySupplierPopUp : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable data = new DataTable();

	private DataTable dtGrid = new DataTable();

	private int[] AryColWidth;

	private int ParentCategorId;

	private bool BranchSelected;

	private bool BranchFeature;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnPrint;

	private Panel panel4;

	private Label lblFor;

	internal GridEX GridAverageRateBySupplier;

	public AvgRateBySupplierPopUp(UserAccount pUser, DataTable dt, int ParentId = 0, bool branchSelected = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		data = dt;
		UserAccount = pUser;
		ParentCategorId = ParentId;
		BranchSelected = branchSelected;
	}

	private void AvgRateBySupplierPopUp_Load(object sender, EventArgs e)
	{
		try
		{
			BranchFeature = CommonServices.GetERPFeatureById(11);
			if (data.Rows.Count > 0)
			{
				if (dtGrid.Columns.Count == 0)
				{
					dtGrid.Columns.Add("BranchesId", typeof(int));
					dtGrid.Columns.Add("BranchName", typeof(string));
					dtGrid.Columns.Add("SupplierCustomerId", typeof(int));
					dtGrid.Columns.Add("CompanyName", typeof(string));
					dtGrid.Columns.Add("ItemId", typeof(int));
					dtGrid.Columns.Add("CropYear", typeof(string));
					dtGrid.Columns.Add("DocDate", typeof(DateTime));
					dtGrid.Columns.Add("ItemQty", typeof(double));
					dtGrid.Columns.Add("NetBillWeight", typeof(double));
					dtGrid.Columns.Add("PackSize", typeof(double));
					dtGrid.Columns.Add("PackSizeQty", typeof(double));
					dtGrid.Columns.Add("AvgRate", typeof(double));
					dtGrid.Columns.Add("ItemAmount", typeof(double));
					dtGrid.Columns.Add("Exp40Kg", typeof(double));
					dtGrid.Columns.Add("AvgRateExp", typeof(double));
					dtGrid.Columns.Add("ItemExpAmount", typeof(double));
					dtGrid.Columns.Add("FrtExp40Kg", typeof(double));
					dtGrid.Columns.Add("CommExp40Kg", typeof(double));
					dtGrid.Columns.Add("WagesExp40Kg", typeof(double));
					dtGrid.Columns.Add("MiscExp40Kg", typeof(double));
					dtGrid.Columns.Add("ItemName", typeof(string));
					dtGrid.Columns.Add("TotalWeightByItem", typeof(double));
					dtGrid.Columns.Add("TotPurPrcnt", typeof(double));
					dtGrid.Columns.Add("MiscExp", typeof(double));
					dtGrid.Columns.Add("FreigtExp", typeof(double));
					dtGrid.Columns.Add("CommExp", typeof(double));
					dtGrid.Columns.Add("EmptyBagsAmountExp", typeof(double));
					dtGrid.Columns.Add("EmptyBagsAmountExp40Kg", typeof(double));
					dtGrid.Columns.Add("FreightDeductionExp", typeof(double));
					dtGrid.Columns.Add("FreightDeductionExp40Kg", typeof(double));
					dtGrid.Columns.Add("TotalExpense", typeof(double));
					dtGrid.Columns.Add("AnalysisQty", typeof(double));
					dtGrid.Columns.Add("Moisture", typeof(double));
					dtGrid.Columns.Add("Empty Shell / Trash", typeof(double));
					dtGrid.Columns.Add("Dust/Stone", typeof(double));
					dtGrid.Columns.Add("Broken", typeof(double));
					dtGrid.Columns.Add("AGL", typeof(double));
					dtGrid.Columns.Add("Damage", typeof(double));
				}
				foreach (DataRow row in data.Rows)
				{
					DataRow newRow = dtGrid.NewRow();
					newRow["BranchesId"] = row["BranchesId"];
					newRow["BranchName"] = row["BranchName"];
					newRow["SupplierCustomerId"] = row["SupplierCustomerId"];
					newRow["CompanyName"] = row["CompanyName"];
					newRow["ItemId"] = row["ItemId"];
					newRow["CropYear"] = row["CropYear"];
					newRow["DocDate"] = row["DocDate"];
					newRow["ItemQty"] = row["ItemQty"];
					newRow["NetBillWeight"] = row["NetBillWeight"];
					newRow["PackSize"] = row["PackSize"];
					newRow["PackSizeQty"] = row["PackSizeQty"];
					newRow["AvgRate"] = row["AvgRate"];
					newRow["ItemAmount"] = row["ItemAmount"];
					newRow["Exp40Kg"] = row["Exp40Kg"];
					newRow["AvgRateExp"] = row["AvgRateExp"];
					newRow["ItemExpAmount"] = row["ItemExpAmount"];
					newRow["FrtExp40Kg"] = row["FrtExp40Kg"];
					newRow["CommExp40Kg"] = row["CommExp40Kg"];
					newRow["WagesExp40Kg"] = row["WagesExp40Kg"];
					newRow["MiscExp40Kg"] = row["MiscExp40Kg"];
					newRow["ItemName"] = row["ItemName"];
					newRow["TotalWeightByItem"] = row["TotalWeightByItem"];
					newRow["TotPurPrcnt"] = row["TotPurPrcnt"];
					newRow["MiscExp"] = row["MiscExp"];
					newRow["FreigtExp"] = row["FreigtExp"];
					newRow["CommExp"] = row["CommExp"];
					newRow["EmptyBagsAmountExp"] = row["EmptyBagsAmountExp"];
					newRow["EmptyBagsAmountExp40Kg"] = row["EmptyBagsAmountExp40Kg"];
					newRow["FreightDeductionExp"] = row["FreightDeductionExp"];
					newRow["FreightDeductionExp40Kg"] = row["FreightDeductionExp40Kg"];
					newRow["TotalExpense"] = row["TotalExpense"];
					newRow["AnalysisQty"] = row["AnalysisQty"];
					newRow["Moisture"] = row["Moisture"];
					newRow["Empty Shell / Trash"] = row["Empty Shell / Trash"];
					newRow["Dust/Stone"] = row["Dust/Stone"];
					newRow["Broken"] = row["Broken"];
					newRow["AGL"] = row["AGL"];
					newRow["Damage"] = row["Damage"];
					dtGrid.Rows.Add(newRow);
				}
				GridAverageRateBySupplier.DataSource = dtGrid;
				GridAvgRateBySupplierSettings();
			}
			else
			{
				GridAverageRateBySupplier.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridAvgRateBySupplierSettings()
	{
		try
		{
			GridAverageRateBySupplier.RootTable.HeaderLines = 3;
			GridAverageRateBySupplier.RootTable.ColumnSets["DocDate"].Visible = false;
			GridAverageRateBySupplier.RootTable.Columns["BranchesId"].Visible = false;
			GridAverageRateBySupplier.RootTable.ColumnSets["BranchName"].Visible = BranchFeature && BranchSelected;
			GridAverageRateBySupplier.RootTable.Columns["BranchName"].Visible = BranchFeature && BranchSelected;
			if (ParentCategorId == 1)
			{
				GridAverageRateBySupplier.RootTable.Columns["Empty Shell / Trash"].TextAlignment = (TextAlignment)3;
				GridAverageRateBySupplier.RootTable.Columns["Dust/Stone"].TextAlignment = (TextAlignment)3;
				GridAverageRateBySupplier.RootTable.Columns["Empty Shell / Trash"].Width = 90;
				GridAverageRateBySupplier.RootTable.Columns["Dust/Stone"].Width = 80;
				GridAverageRateBySupplier.RootTable.Columns["Dust/Stone"].FormatString = "#,#.###";
				GridAverageRateBySupplier.RootTable.Columns["Empty Shell / Trash"].FormatString = "#,#.###";
				GridAverageRateBySupplier.RootTable.Columns["Moisture"].FormatString = "#,#.###";
				GridAverageRateBySupplier.RootTable.ColumnSets["Broken"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["AGL"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["Damage"].Visible = false;
			}
			else if (ParentCategorId == 2)
			{
				GridAverageRateBySupplier.RootTable.Columns["Broken"].TextAlignment = (TextAlignment)3;
				GridAverageRateBySupplier.RootTable.Columns["AGL"].TextAlignment = (TextAlignment)3;
				GridAverageRateBySupplier.RootTable.Columns["Damage"].TextAlignment = (TextAlignment)3;
				GridAverageRateBySupplier.RootTable.Columns["Broken"].Width = 60;
				GridAverageRateBySupplier.RootTable.Columns["AGL"].Width = 60;
				GridAverageRateBySupplier.RootTable.Columns["Damage"].Width = 65;
				GridAverageRateBySupplier.RootTable.Columns["Broken"].FormatString = "#,#.###";
				GridAverageRateBySupplier.RootTable.Columns["AGL"].FormatString = "#,#.###";
				GridAverageRateBySupplier.RootTable.Columns["Damage"].FormatString = "#,#.###";
				GridAverageRateBySupplier.RootTable.ColumnSets["Empty Shell / Trash"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["Dust/Stone"].Visible = false;
			}
			else
			{
				GridAverageRateBySupplier.RootTable.ColumnSets["Empty Shell / Trash"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["Dust/Stone"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["Broken"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["AGL"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["Damage"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["Moisture"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["AnalysisQty"].Visible = false;
				GridAverageRateBySupplier.RootTable.ColumnSets["DocDate"].Visible = false;
				GridAverageRateBySupplier.RootTable.Groups.Add("DocDate").GroupFormatString = "dd-MMM-yy";
			}
			GridAverageRateBySupplier.RootTable.Columns["Moisture"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["Moisture"].FormatString = "#,#.###";
			GridAverageRateBySupplier.RootTable.Columns["BranchName"].Width = 120;
			GridAverageRateBySupplier.RootTable.Columns["AnalysisQty"].Width = 80;
			GridAverageRateBySupplier.RootTable.Columns["AvgRate"].Width = 70;
			GridAverageRateBySupplier.RootTable.Columns["ItemQty"].Width = 70;
			GridAverageRateBySupplier.RootTable.Columns["NetBillWeight"].Width = 105;
			GridAverageRateBySupplier.RootTable.Columns["ItemAmount"].Width = 105;
			GridAverageRateBySupplier.RootTable.Columns["MiscExp40Kg"].Width = 70;
			GridAverageRateBySupplier.RootTable.Columns["FrtExp40Kg"].Width = 70;
			GridAverageRateBySupplier.RootTable.Columns["AvgRateExp"].Width = 70;
			GridAverageRateBySupplier.RootTable.Columns["CommExp40Kg"].Width = 70;
			GridAverageRateBySupplier.RootTable.Columns["WagesExp40Kg"].Width = 75;
			GridAverageRateBySupplier.RootTable.Columns["ItemExpAmount"].Width = 130;
			GridAverageRateBySupplier.RootTable.Columns["Exp40Kg"].Width = 80;
			GridAverageRateBySupplier.RootTable.Columns["CompanyName"].Width = 260;
			GridAverageRateBySupplier.RootTable.Columns["CropYear"].Width = 70;
			GridAverageRateBySupplier.RootTable.Columns["PackSize"].Width = 60;
			GridAverageRateBySupplier.RootTable.Columns["PackSizeQty"].Width = 75;
			GridAverageRateBySupplier.RootTable.Columns["AvgRateExp"].Width = 100;
			GridAverageRateBySupplier.RootTable.Columns["ItemName"].Width = 140;
			GridAverageRateBySupplier.RootTable.Columns["Moisture"].Width = 70;
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp"].Width = 100;
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp"].Caption = "Empty Bags Amount Expense";
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp40Kg"].Width = 100;
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp40Kg"].Caption = "Empty Bags Amount Expense 40Kg";
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp"].Width = 100;
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp"].Caption = "Freight Deduction Expense";
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp40Kg"].Width = 100;
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp40Kg"].Caption = "Freight Deduction Expense 40Kg";
			GridAverageRateBySupplier.RootTable.Columns["CompanyName"].ColumnType = (ColumnType)5;
			GridAverageRateBySupplier.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			GridAverageRateBySupplier.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridAverageRateBySupplier.RootTable.ColumnSets["MiscExp"].Visible = false;
			GridAverageRateBySupplier.RootTable.ColumnSets["FreigtExp"].Visible = false;
			GridAverageRateBySupplier.RootTable.ColumnSets["CommExp"].Visible = false;
			GridAverageRateBySupplier.RootTable.ColumnSets["TotalExpense"].Visible = false;
			GridAverageRateBySupplier.RootTable.Columns["ItemId"].Visible = false;
			GridAverageRateBySupplier.RootTable.ColumnSets["TotalWeightByItem"].Visible = false;
			GridAverageRateBySupplier.RootTable.ColumnSets["TotPurPrcnt"].Visible = false;
			GridAverageRateBySupplier.RootTable.Columns["CompanyName"].Caption = "PartyName";
			GridAverageRateBySupplier.RootTable.Columns["CropYear"].Caption = "Crop";
			GridAverageRateBySupplier.RootTable.Columns["NetBillWeight"].Caption = "Net Weight";
			GridAverageRateBySupplier.RootTable.Columns["ItemAmount"].Caption = "Amount";
			GridAverageRateBySupplier.RootTable.Columns["FrtExp40Kg"].Caption = "Freight / 40Kg";
			GridAverageRateBySupplier.RootTable.Columns["CommExp40Kg"].Caption = "Comm / 40Kg";
			GridAverageRateBySupplier.RootTable.Columns["WagesExp40Kg"].Caption = "Wages / 40Kg";
			GridAverageRateBySupplier.RootTable.Columns["MiscExp40Kg"].Caption = "Other / 40Kg";
			GridAverageRateBySupplier.RootTable.Columns["AnalysisQty"].AggregateFunction = (AggregateFunction)2;
			GridAverageRateBySupplier.RootTable.Columns["AnalysisQty"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["AnalysisQty"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["AnalysisQty"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridAverageRateBySupplier.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["PackSizeQty"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["PackSizeQty"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["AvgRate"].AggregateFunction = (AggregateFunction)3;
			GridAverageRateBySupplier.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["AvgRate"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["AvgRate"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			GridAverageRateBySupplier.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GridAverageRateBySupplier.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["ItemAmount"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["TotalExpense"].AggregateFunction = (AggregateFunction)2;
			GridAverageRateBySupplier.RootTable.Columns["TotalExpense"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["TotalExpense"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["TotalExpense"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["ItemExpAmount"].AggregateFunction = (AggregateFunction)2;
			GridAverageRateBySupplier.RootTable.Columns["ItemExpAmount"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["ItemExpAmount"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["ItemExpAmount"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp"].AggregateFunction = (AggregateFunction)2;
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp"].AggregateFunction = (AggregateFunction)2;
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["Exp40Kg"].AggregateFunction = (AggregateFunction)2;
			GridAverageRateBySupplier.RootTable.Columns["Exp40Kg"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["Exp40Kg"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["Exp40Kg"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["TotalWeightByItem"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["TotalWeightByItem"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["EmptyBagsAmountExp40Kg"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["FreightDeductionExp40Kg"].FormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["FrtExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["FrtExp40Kg"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["WagesExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["WagesExp40Kg"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["MiscExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["MiscExp40Kg"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["CommExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["CommExp40Kg"].TotalFormatString = "#,##0.###";
			GridAverageRateBySupplier.RootTable.Columns["AvgRateExp"].TextAlignment = (TextAlignment)3;
			GridAverageRateBySupplier.RootTable.Columns["AvgRateExp"].FormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (data == null || data.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(data, "222-InvRptPurchaseInvoicePurchaseAvgRateByItemSupplier.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridAverageRateBySupplier_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridAverageRateBySupplier.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				if (e.Column.Key == "ItemName")
				{
					PurchaseRegisterNew purchaseRegisterNew = new PurchaseRegisterNew(UserAccount);
					purchaseRegisterNew.FromDate = clsGlobalVariables.ActiveYr.Start_Period;
					purchaseRegisterNew.ToDate = DateTime.Now;
					purchaseRegisterNew.ItemId = ItemId;
					purchaseRegisterNew.ShowDialog();
				}
				else if (e.Column.Key == "CompanyName")
				{
					PurchaseRegisterNew purchaseRegisterNew2 = new PurchaseRegisterNew(UserAccount);
					purchaseRegisterNew2.FromDate = clsGlobalVariables.ActiveYr.Start_Period;
					purchaseRegisterNew2.ToDate = DateTime.Now;
					purchaseRegisterNew2.ItemId = ItemId;
					purchaseRegisterNew2.SupplierId = SupplierCustomerId;
					purchaseRegisterNew2.ShowDialog();
				}
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		GridEXLayout GridAverageRateBySupplier_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AnalyticDashboard.AvgRateBySupplierPopUp));
		this.panel1 = new System.Windows.Forms.Panel();
		this.GridAverageRateBySupplier = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.lblFor = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAverageRateBySupplier).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.GridAverageRateBySupplier);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(824, 471);
		this.panel1.TabIndex = 0;
		this.GridAverageRateBySupplier.AllowEdit = (InheritableBoolean)2;
		this.GridAverageRateBySupplier.AlternatingColors = true;
		this.GridAverageRateBySupplier.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridAverageRateBySupplier_DesignTimeLayout.LayoutString = resources.GetString("GridAverageRateBySupplier_DesignTimeLayout.LayoutString");
		this.GridAverageRateBySupplier.DesignTimeLayout = GridAverageRateBySupplier_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridAverageRateBySupplier).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAverageRateBySupplier.DynamicFiltering = true;
		this.GridAverageRateBySupplier.FilterMode = (FilterMode)2;
		this.GridAverageRateBySupplier.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAverageRateBySupplier).Font = new System.Drawing.Font("Verdana", 9f);
		this.GridAverageRateBySupplier.GroupTotals = (GroupTotals)2;
		this.GridAverageRateBySupplier.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridAverageRateBySupplier).Location = new System.Drawing.Point(0, 58);
		((System.Windows.Forms.Control)(object)this.GridAverageRateBySupplier).Name = "GridAverageRateBySupplier";
		((ScrollableBase)this.GridAverageRateBySupplier).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAverageRateBySupplier).Size = new System.Drawing.Size(824, 413);
		((System.Windows.Forms.Control)(object)this.GridAverageRateBySupplier).TabIndex = 53;
		this.GridAverageRateBySupplier.TotalRow = (InheritableBoolean)1;
		this.GridAverageRateBySupplier.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAverageRateBySupplier).VisualStyle = (VisualStyle)4;
		this.GridAverageRateBySupplier.LinkClicked += new ColumnActionEventHandler(GridAverageRateBySupplier_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.lblFor);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 25);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(824, 33);
		this.panel4.TabIndex = 52;
		this.lblFor.AutoSize = true;
		this.lblFor.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFor.ForeColor = System.Drawing.Color.White;
		this.lblFor.Location = new System.Drawing.Point(4, 6);
		this.lblFor.Name = "lblFor";
		this.lblFor.Size = new System.Drawing.Size(195, 19);
		this.lblFor.TabIndex = 5;
		this.lblFor.Text = "Average Rate By Party";
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(824, 25);
		this.toolStrip1.TabIndex = 51;
		this.toolStrip1.Text = "toolStrip1";
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(179, 22);
		this.btnPrint.Text = "222-&Avg Rate By Party";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(824, 471);
		base.Controls.Add(this.panel1);
		base.Name = "AvgRateBySupplierPopUp";
		this.Text = "Avg Rate By Party PopUp";
		base.Load += new System.EventHandler(AvgRateBySupplierPopUp_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAverageRateBySupplier).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
