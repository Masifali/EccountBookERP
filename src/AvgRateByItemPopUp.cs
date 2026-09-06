// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.AnalyticDashboard
{

public class AvgRateByItemPopUp : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable data = new DataTable();

	private DataTable dtGrid = new DataTable();

	private bool FlagForAvgRateItemSupplierId;

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

	internal GridEX GridAvgRateByItem;

	public AvgRateByItemPopUp(UserAccount pUser, DataTable dt, int ParentId = 0, bool branchSelected = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		data = dt;
		UserAccount = pUser;
		ParentCategorId = ParentId;
		BranchSelected = branchSelected;
	}

	private void AvgRateByItemPopUp_Load(object sender, EventArgs e)
	{
		try
		{
			BranchFeature = CommonServices.GetERPFeatureById(11);
			if (data.Rows.Count > 0)
			{
				if (data.Columns.Contains("SupplierCustomerId"))
				{
					FlagForAvgRateItemSupplierId = true;
				}
				if (FlagForAvgRateItemSupplierId)
				{
					if (dtGrid.Columns.Count == 0)
					{
						dtGrid.Columns.Add("BranchesId", typeof(int));
						dtGrid.Columns.Add("BranchName", typeof(string));
						dtGrid.Columns.Add("SupplierCustomerId", typeof(int));
						dtGrid.Columns.Add("ItemId", typeof(int));
						dtGrid.Columns.Add("CropYear", typeof(string));
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
						dtGrid.Columns.Add("CompanyName", typeof(string));
					}
					foreach (DataRow row in data.Rows)
					{
						DataRow newRow = dtGrid.NewRow();
						newRow["BranchesId"] = row["BranchesId"];
						newRow["BranchName"] = row["BranchName"];
						newRow["SupplierCustomerId"] = row["SupplierCustomerId"];
						newRow["ItemId"] = row["ItemId"];
						newRow["CropYear"] = row["CropYear"];
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
						newRow["CompanyName"] = row["CompanyName"];
						dtGrid.Rows.Add(newRow);
					}
				}
				else
				{
					if (dtGrid.Columns.Count == 0)
					{
						dtGrid.Columns.Add("BranchesId", typeof(int));
						dtGrid.Columns.Add("BranchName", typeof(string));
						dtGrid.Columns.Add("DocDate", typeof(DateTime));
						dtGrid.Columns.Add("ItemId", typeof(int));
						dtGrid.Columns.Add("ItemName", typeof(string));
						dtGrid.Columns.Add("TypeDescription", typeof(string));
						dtGrid.Columns.Add("CropYear", typeof(string));
						dtGrid.Columns.Add("ItemQty", typeof(double));
						dtGrid.Columns.Add("NetBillWeight", typeof(double));
						dtGrid.Columns.Add("AvgRate", typeof(double));
						dtGrid.Columns.Add("ItemAmount", typeof(double));
						dtGrid.Columns.Add("Exp40Kg", typeof(double));
						dtGrid.Columns.Add("AvgRateExp", typeof(double));
						dtGrid.Columns.Add("ItemExpAmount", typeof(double));
						dtGrid.Columns.Add("FrtExp40Kg", typeof(double));
						dtGrid.Columns.Add("CommExp40Kg", typeof(double));
						dtGrid.Columns.Add("WagesExp40Kg", typeof(double));
						dtGrid.Columns.Add("MiscExp40Kg", typeof(double));
						dtGrid.Columns.Add("FreigtExp", typeof(double));
						dtGrid.Columns.Add("CommExp", typeof(double));
						dtGrid.Columns.Add("WagesExp", typeof(double));
						dtGrid.Columns.Add("MiscExp", typeof(double));
						dtGrid.Columns.Add("EmptyBagsAmountExp", typeof(double));
						dtGrid.Columns.Add("EmptyBagsAmountExp40Kg", typeof(double));
						dtGrid.Columns.Add("FreightDeductionExp", typeof(double));
						dtGrid.Columns.Add("FreightDeductionExp40Kg", typeof(double));
						dtGrid.Columns.Add("TotalExpense", typeof(double));
						dtGrid.Columns.Add("TotalWeightByItem", typeof(double));
						dtGrid.Columns.Add("TotPurPrcnt", typeof(double));
					}
					foreach (DataRow row2 in data.Rows)
					{
						DataRow newRow2 = dtGrid.NewRow();
						newRow2["BranchesId"] = row2["BranchesId"];
						newRow2["BranchName"] = row2["BranchName"];
						newRow2["DocDate"] = row2["DocDate"];
						newRow2["ItemId"] = row2["ItemId"];
						newRow2["ItemName"] = row2["ItemName"];
						newRow2["TypeDescription"] = row2["TypeDescription"];
						newRow2["CropYear"] = row2["CropYear"];
						newRow2["ItemQty"] = row2["ItemQty"];
						newRow2["NetBillWeight"] = row2["NetBillWeight"];
						newRow2["AvgRate"] = row2["AvgRate"];
						newRow2["ItemAmount"] = row2["ItemAmount"];
						newRow2["Exp40Kg"] = row2["Exp40Kg"];
						newRow2["AvgRateExp"] = row2["AvgRateExp"];
						newRow2["ItemExpAmount"] = row2["ItemExpAmount"];
						newRow2["FrtExp40Kg"] = row2["FrtExp40Kg"];
						newRow2["CommExp40Kg"] = row2["CommExp40Kg"];
						newRow2["WagesExp40Kg"] = row2["WagesExp40Kg"];
						newRow2["MiscExp40Kg"] = row2["MiscExp40Kg"];
						newRow2["FreigtExp"] = row2["FreigtExp"];
						newRow2["CommExp"] = row2["CommExp"];
						newRow2["WagesExp"] = row2["WagesExp"];
						newRow2["MiscExp"] = row2["MiscExp"];
						newRow2["EmptyBagsAmountExp"] = row2["EmptyBagsAmountExp"];
						newRow2["EmptyBagsAmountExp40Kg"] = row2["EmptyBagsAmountExp40Kg"];
						newRow2["FreightDeductionExp"] = row2["FreightDeductionExp"];
						newRow2["FreightDeductionExp40Kg"] = row2["FreightDeductionExp40Kg"];
						newRow2["TotalExpense"] = row2["TotalExpense"];
						newRow2["TotalWeightByItem"] = row2["TotalWeightByItem"];
						newRow2["TotPurPrcnt"] = row2["TotPurPrcnt"];
						dtGrid.Rows.Add(newRow2);
					}
				}
				GridAvgRateByItem.DataSource = dtGrid;
				GridAvgRateByItemSettings();
			}
			else
			{
				GridAvgRateByItem.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridAvgRateByItemSettings()
	{
		try
		{
			GridAvgRateByItem.RootTable.Columns["PackSizeQty"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["BranchesId"].Visible = false;
			GridAvgRateByItem.RootTable.ColumnSets["BranchName"].Visible = BranchFeature && BranchSelected;
			GridAvgRateByItem.RootTable.Columns["BranchName"].Visible = BranchFeature && BranchSelected;
			GridAvgRateByItem.RootTable.HeaderLines = 3;
			if (FlagForAvgRateItemSupplierId)
			{
				_ = ((JanusCollectionBase)GridAvgRateByItem.RootTable.Columns).Count;
				GridAvgRateByItem.RootTable.Columns["SupplierCustomerId"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["DocDate"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["WagesExp"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["CompanyName"].Caption = "PartyName";
				if (ParentCategorId == 1)
				{
					GridAvgRateByItem.RootTable.Columns["Empty Shell / Trash"].Width = 90;
					GridAvgRateByItem.RootTable.Columns["Dust/Stone"].Width = 75;
					GridAvgRateByItem.RootTable.Columns["Empty Shell / Trash"].TextAlignment = (TextAlignment)3;
					GridAvgRateByItem.RootTable.Columns["Dust/Stone"].TextAlignment = (TextAlignment)3;
					GridAvgRateByItem.RootTable.Columns["Dust/Stone"].FormatString = "#,#.###";
					GridAvgRateByItem.RootTable.Columns["Empty Shell / Trash"].FormatString = "#,#.###";
					GridAvgRateByItem.RootTable.ColumnSets["Broken"].Visible = false;
					GridAvgRateByItem.RootTable.ColumnSets["AGL"].Visible = false;
					GridAvgRateByItem.RootTable.ColumnSets["Damage"].Visible = false;
				}
				else if (ParentCategorId == 2)
				{
					GridAvgRateByItem.RootTable.Columns["Broken"].Width = 60;
					GridAvgRateByItem.RootTable.Columns["AGL"].Width = 60;
					GridAvgRateByItem.RootTable.Columns["Damage"].Width = 60;
					GridAvgRateByItem.RootTable.Columns["Broken"].TextAlignment = (TextAlignment)3;
					GridAvgRateByItem.RootTable.Columns["AGL"].TextAlignment = (TextAlignment)3;
					GridAvgRateByItem.RootTable.Columns["Damage"].TextAlignment = (TextAlignment)3;
					GridAvgRateByItem.RootTable.Columns["Broken"].FormatString = "#,#.###";
					GridAvgRateByItem.RootTable.Columns["AGL"].FormatString = "#,#.###";
					GridAvgRateByItem.RootTable.Columns["Damage"].FormatString = "#,#.###";
					GridAvgRateByItem.RootTable.ColumnSets["Empty Shell / Trash"].Visible = false;
					GridAvgRateByItem.RootTable.ColumnSets["Dust/Stone"].Visible = false;
				}
				else
				{
					GridAvgRateByItem.RootTable.ColumnSets["Empty Shell / Trash"].Visible = false;
					GridAvgRateByItem.RootTable.ColumnSets["Dust/Stone"].Visible = false;
					GridAvgRateByItem.RootTable.ColumnSets["Broken"].Visible = false;
					GridAvgRateByItem.RootTable.ColumnSets["AGL"].Visible = false;
					GridAvgRateByItem.RootTable.ColumnSets["Damage"].Visible = false;
				}
				GridAvgRateByItem.RootTable.Columns["Moisture"].TextAlignment = (TextAlignment)3;
				GridAvgRateByItem.RootTable.Columns["Moisture"].FormatString = "#,#.###";
				GridAvgRateByItem.RootTable.Columns["AnalysisQty"].AggregateFunction = (AggregateFunction)2;
				GridAvgRateByItem.RootTable.Columns["AnalysisQty"].TextAlignment = (TextAlignment)3;
				GridAvgRateByItem.RootTable.Columns["AnalysisQty"].FormatString = "#,##0";
				GridAvgRateByItem.RootTable.Columns["AnalysisQty"].TotalFormatString = "#,##0";
			}
			else
			{
				GridAvgRateByItem.RootTable.Columns["ItemId"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["PackSize"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["PackSizeQty"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["CompanyName"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["ItemName"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["Broken"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["AGL"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["Damage"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["Empty Shell / Trash"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["Dust/Stone"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["Moisture"].Visible = false;
				GridAvgRateByItem.RootTable.ColumnSets["AnalysisQty"].Visible = false;
				GridAvgRateByItem.RootTable.Columns["DocDate"].ColumnType = (ColumnType)5;
				GridAvgRateByItem.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
				GridAvgRateByItem.RootTable.Groups.Add("ItemName");
			}
			GridAvgRateByItem.RootTable.ColumnSets["TypeDescription"].Visible = false;
			GridAvgRateByItem.RootTable.ColumnSets["TotalExpense"].Visible = false;
			GridAvgRateByItem.RootTable.ColumnSets["TotalWeightByItem"].Visible = false;
			GridAvgRateByItem.RootTable.ColumnSets["TotPurPrcnt"].Visible = false;
			GridAvgRateByItem.RootTable.ColumnSets["MiscExp"].Visible = false;
			GridAvgRateByItem.RootTable.ColumnSets["FreigtExp"].Visible = false;
			GridAvgRateByItem.RootTable.ColumnSets["CommExp"].Visible = false;
			GridAvgRateByItem.RootTable.Columns["BranchName"].Width = 120;
			GridAvgRateByItem.RootTable.Columns["DocDate"].Width = 80;
			GridAvgRateByItem.RootTable.Columns["PackSize"].Width = 60;
			GridAvgRateByItem.RootTable.Columns["PackSizeQty"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["CropYear"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["PackSize"].Width = 60;
			GridAvgRateByItem.RootTable.Columns["PackSizeQty"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["ItemQty"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["AvgRate"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["AvgRate"].Caption = "Average Rate";
			GridAvgRateByItem.RootTable.Columns["NetBillWeight"].Width = 95;
			GridAvgRateByItem.RootTable.Columns["NetBillWeight"].Caption = "Net Weight";
			GridAvgRateByItem.RootTable.Columns["ItemAmount"].Width = 105;
			GridAvgRateByItem.RootTable.Columns["ItemExpAmount"].Width = 130;
			GridAvgRateByItem.RootTable.Columns["ItemExpAmount"].Caption = "Item Expense Amount";
			GridAvgRateByItem.RootTable.Columns["Exp40Kg"].Width = 80;
			GridAvgRateByItem.RootTable.Columns["Exp40Kg"].Caption = "Expense 40Kg";
			GridAvgRateByItem.RootTable.Columns["FrtExp40Kg"].Width = 100;
			GridAvgRateByItem.RootTable.Columns["FrtExp40Kg"].Caption = "Freight Expense 40Kg";
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp"].Width = 100;
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp"].Caption = "Empty Bags Amount Expense";
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp40Kg"].Width = 100;
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp40Kg"].Caption = "Empty Bags Amount Expense 40Kg";
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp"].Width = 100;
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp"].Caption = "Freight Deduction Expense";
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp40Kg"].Width = 100;
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp40Kg"].Caption = "Freight Deduction Expense 40Kg";
			GridAvgRateByItem.RootTable.Columns["CommExp40Kg"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["CommExp40Kg"].Caption = "Comm / 40Kg";
			GridAvgRateByItem.RootTable.Columns["WagesExp40Kg"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["WagesExp40Kg"].Caption = "Wages / 40Kg";
			GridAvgRateByItem.RootTable.Columns["MiscExp40Kg"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["MiscExp40Kg"].Caption = "Other / 40Kg";
			GridAvgRateByItem.RootTable.Columns["AvgRateExp"].Width = 100;
			GridAvgRateByItem.RootTable.Columns["AvgRateExp"].Caption = "Average Rate Expense";
			GridAvgRateByItem.RootTable.Columns["Empty Shell / Trash"].Width = 90;
			GridAvgRateByItem.RootTable.Columns["Dust/Stone"].Width = 80;
			GridAvgRateByItem.RootTable.Columns["Broken"].Width = 60;
			GridAvgRateByItem.RootTable.Columns["AGL"].Width = 60;
			GridAvgRateByItem.RootTable.Columns["Damage"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["AnalysisQty"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["Moisture"].Width = 70;
			GridAvgRateByItem.RootTable.Columns["CompanyName"].Width = 150;
			GridAvgRateByItem.RootTable.Columns["ItemName"].Width = 150;
			GridAvgRateByItem.RootTable.Columns["ItemName"].Position = 1;
			GridAvgRateByItem.RootTable.Columns["AvgRateExp"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["WagesExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["WagesExp"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			GridAvgRateByItem.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			GridAvgRateByItem.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["ItemAmount"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["MiscExp40Kg"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["MiscExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["MiscExp40Kg"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["MiscExp40Kg"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["FrtExp40Kg"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["FrtExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["FrtExp40Kg"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["FrtExp40Kg"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["CommExp40Kg"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["CommExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["CommExp40Kg"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["CommExp40Kg"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["TotalExpense"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["TotalExpense"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["TotalExpense"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["TotalExpense"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["ItemExpAmount"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["ItemExpAmount"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["ItemExpAmount"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["ItemExpAmount"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["Exp40Kg"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["Exp40Kg"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["Exp40Kg"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["Exp40Kg"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp40Kg"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp40Kg"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["EmptyBagsAmountExp40Kg"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp40Kg"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp40Kg"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp40Kg"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["FreightDeductionExp40Kg"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["WagesExp"].AggregateFunction = (AggregateFunction)2;
			GridAvgRateByItem.RootTable.Columns["WagesExp"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["WagesExp"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["WagesExp"].TotalFormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["AvgRate"].AggregateFunction = (AggregateFunction)3;
			GridAvgRateByItem.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			GridAvgRateByItem.RootTable.Columns["AvgRate"].FormatString = "#,##0.####";
			GridAvgRateByItem.RootTable.Columns["AvgRate"].TotalFormatString = "#,##0.####";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridAvgRateByItem_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			if (!(data.Rows[0]["DocDate"].ToString() != "") || !(data.Rows[0]["DocDate"].ToString() != string.Empty))
			{
				return;
			}
			DataTable dt = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime(GridAvgRateByItem.CurrentRow.Cells["DocDate"].Value);
			obj.ToDate = obj.FromDate;
			obj.ItemId = Conversion.ToInt(GridAvgRateByItem.CurrentRow.Cells["ItemId"].Value);
			obj.BranchesIds = ((!BranchFeature) ? "" : (BranchSelected ? Conversion.ToString(GridAvgRateByItem.CurrentRow.Cells["BranchesId"].Value) : ""));
			dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceAvgRateBySupplier222(obj);
			if (dt.Rows.Count > 0)
			{
				string columnNameToUpdate = "DocDate";
				DateTime newValue = Conversion.ToDateTime((object)obj.FromDate);
				foreach (DataRow row in dt.Rows)
				{
					row[columnNameToUpdate] = newValue;
				}
				bool flag = obj.BranchesIds != string.Empty;
				AvgRateBySupplierPopUp avgRateBySupplierPopUp = new AvgRateBySupplierPopUp(UserAccount, dt, 0, flag);
				avgRateBySupplierPopUp.WindowState = FormWindowState.Maximized;
				avgRateBySupplierPopUp.Show();
			}
			else
			{
				MessageBox.Show("No Data Found");
			}
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
			val.ShowReportWithDataTable(data, "221-InvRptPurchaseInvoicePurchaseAvgRateByItem.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("No Record Found");
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
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		GridEXLayout GridAvgRateByItem_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AnalyticDashboard.AvgRateByItemPopUp));
		this.panel1 = new System.Windows.Forms.Panel();
		this.GridAvgRateByItem = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.lblFor = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAvgRateByItem).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.GridAvgRateByItem);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(800, 450);
		this.panel1.TabIndex = 0;
		this.GridAvgRateByItem.AllowEdit = (InheritableBoolean)2;
		this.GridAvgRateByItem.AlternatingColors = true;
		this.GridAvgRateByItem.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridAvgRateByItem_DesignTimeLayout.LayoutString = resources.GetString("GridAvgRateByItem_DesignTimeLayout.LayoutString");
		this.GridAvgRateByItem.DesignTimeLayout = GridAvgRateByItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAvgRateByItem.DynamicFiltering = true;
		this.GridAvgRateByItem.FilterMode = (FilterMode)2;
		this.GridAvgRateByItem.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Font = new System.Drawing.Font("Verdana", 9f);
		this.GridAvgRateByItem.GroupTotals = (GroupTotals)2;
		this.GridAvgRateByItem.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Location = new System.Drawing.Point(0, 53);
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Name = "GridAvgRateByItem";
		((ScrollableBase)this.GridAvgRateByItem).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).Size = new System.Drawing.Size(800, 397);
		((System.Windows.Forms.Control)(object)this.GridAvgRateByItem).TabIndex = 54;
		this.GridAvgRateByItem.TotalRow = (InheritableBoolean)1;
		this.GridAvgRateByItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAvgRateByItem).VisualStyle = (VisualStyle)4;
		this.GridAvgRateByItem.LinkClicked += new ColumnActionEventHandler(GridAvgRateByItem_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.lblFor);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 25);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(800, 28);
		this.panel4.TabIndex = 53;
		this.lblFor.AutoSize = true;
		this.lblFor.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFor.ForeColor = System.Drawing.Color.White;
		this.lblFor.Location = new System.Drawing.Point(4, 3);
		this.lblFor.Name = "lblFor";
		this.lblFor.Size = new System.Drawing.Size(190, 19);
		this.lblFor.TabIndex = 5;
		this.lblFor.Text = "Average Rate By Item";
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(800, 25);
		this.toolStrip1.TabIndex = 52;
		this.toolStrip1.Text = "toolStrip1";
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(174, 22);
		this.btnPrint.Text = "221-&Avg Rate By Item";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Controls.Add(this.panel1);
		base.Name = "AvgRateByItemPopUp";
		this.Text = "Avg Rate By Item PopUp";
		base.Load += new System.EventHandler(AvgRateByItemPopUp_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAvgRateByItem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
