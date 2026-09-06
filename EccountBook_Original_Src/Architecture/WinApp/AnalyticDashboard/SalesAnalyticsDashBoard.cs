using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.AnalyticDashboard;

public class SalesAnalyticsDashBoard : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtComparisonList = new DataTable();

	private DataTable dtGridItemWise = new DataTable();

	private DataTable dtGridCustomerWise = new DataTable();

	private DataTable dtBreakUpDataForItemWise = new DataTable();

	private DataTable dtBreakUpDataForSupplierWise = new DataTable();

	public bool RequestedByOtherDocument;

	public int ParentCategoryId;

	private bool BranchFeature;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Label lblComparison;

	private Panel panel5;

	private Label label27;

	internal UltraCombo CmbItemType;

	private Label label6;

	internal UltraCombo CmbItemName;

	private Label label5;

	internal UltraCombo CmbCropYear;

	private Label label4;

	internal UltraCombo CmbJotLot;

	private Label label3;

	internal UltraCombo CmbItemCategory;

	private Label label2;

	internal UltraCombo CmbParty;

	private Label label1;

	private Button btnshow;

	private Panel panel13;

	private Panel panel8;

	private Panel panel12;

	private Label txtTotalAmount;

	private Label label13;

	private Panel panel11;

	private Label txtTotalWeight;

	private Label label11;

	private Panel panel10;

	private Label txtTotalQty;

	private Label label9;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnRefresh;

	internal UltraCombo cmbparentcategory;

	private Label label10;

	internal GridEX grdItemWise;

	private Panel panel7;

	private Label label7;

	internal GridEX grdCustomerWise;

	private Panel panel9;

	private Label label8;

	public DateTimePicker todate;

	public DateTimePicker fromdate;

	internal UltraCombo CmbBranches;

	private Label Branches;

	private ToolStrip toolStrip1;

	private ToolStripButton btnPrintItemWise;

	private ToolStripButton btnPrintSupplierWise;

	public SalesAnalyticsDashBoard(UserAccount Puser)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void frmAnalyticsDashboard_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			panel1.AutoScroll = true;
			BranchFeature = CommonServices.GetERPFeatureById(11);
			AllComboBind();
			BranchesFill();
			ParentCategoryFill();
			if (!RequestedByOtherDocument)
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			else
			{
				cmbparentcategory.Value = ParentCategoryId;
			}
			if (((UltraGridBase)cmbparentcategory).ActiveRow != null)
			{
				GetComparisonData();
			}
			if (dtGridCustomerWise.Rows.Count > 0)
			{
				GetTotals();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			DataTable dt = VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, "", 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbBranches, "Id", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbBranches).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbBranches.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranches.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranches).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			DataTable dt = InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				ReportType = "Sale",
				Activity = "GetParentCategory"
			});
			if (dt != null && dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbparentcategory, "Id", "RefName", "Parent Category", false);
				((UltraGridBase)cmbparentcategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbparentcategory).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllComboBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			DataTable dt = InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				ReportType = "Sale",
				Ids = Conversion.ToString(cmbparentcategory.Value)
			});
			if (dt.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rowsCategory = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetItemCategory"
					select r;
				DataTable dtCategory = (rowsCategory.Any() ? rowsCategory.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsType = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetItemType"
					select r;
				DataTable dtItemType = (rowsType.Any() ? rowsType.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsCustomer = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetSupplierCustomer"
					select r;
				DataTable dtSupplierCustomer = (rowsCustomer.Any() ? rowsCustomer.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsItem = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetItems"
					select r;
				DataTable dtItem = (rowsItem.Any() ? rowsItem.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsJob = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetJobLot"
					select r;
				DataTable dtJobLot = (rowsJob.Any() ? rowsJob.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsCropYear = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetCropYear"
					select r;
				DataTable dtCropYear = (rowsCropYear.Any() ? rowsCropYear.CopyToDataTable() : dt.Clone());
				DDL.BindDDLNew(dtItemType, CmbItemType, "Id", "RefName", "Item Type", false);
				((UltraGridBase)CmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtCategory, CmbItemCategory, "Id", "RefName", "Item Category", false);
				((UltraGridBase)CmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtSupplierCustomer, CmbParty, "Id", "RefName", "Party Name", false);
				DDL.BindDDLNew(dtItem, CmbItemName, "Id", "RefName", "Item Name", false);
				((UltraGridBase)CmbItemName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtJobLot, CmbJotLot, "Id", "RefName", "JobLot", false);
				((UltraGridBase)CmbJotLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtCropYear, CmbCropYear, "Id", "RefName", "CropYear", false);
				((UltraGridBase)CmbCropYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbparentcategory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbparentcategory).ActiveRow != null && Conversion.ToInt(cmbparentcategory.Value) > 0)
			{
				AllComboBind();
				return;
			}
			((UltraGridBase)CmbItemCategory).DataSource = null;
			((Control)(object)CmbItemCategory).Text = "";
			((UltraGridBase)CmbItemType).DataSource = null;
			((Control)(object)CmbItemType).Text = "";
			((UltraGridBase)CmbItemName).DataSource = null;
			((Control)(object)CmbItemName).Text = "";
			((UltraGridBase)CmbCropYear).DataSource = null;
			((Control)(object)CmbCropYear).Text = "";
			((UltraGridBase)CmbJotLot).DataSource = null;
			((Control)(object)CmbJotLot).Text = "";
			((UltraGridBase)CmbParty).DataSource = null;
			((Control)(object)CmbParty).Text = "";
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
			if (((UltraGridBase)cmbparentcategory).ActiveRow == null)
			{
				MessageBox.Show("Please Select Parent Category First...");
				return;
			}
			GetComparisonData();
			if (dtGridItemWise.Rows.Count > 0)
			{
				GetTotals();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetComparisonData()
	{
		try
		{
			GetComparisonDataAgainstSummary("Item Wise Sales Summary");
			if (dtGridItemWise.Rows.Count > 0)
			{
				GetComparisonDataAgainstSummary("Customer Wise Sales Summary");
				GetBreakupDataAgainstActivityBreakUpName("Item And Customer Wise Comparison");
				GetBreakupDataAgainstActivityBreakUpName("Customer And Item Wise Comparison");
				return;
			}
			txtTotalQty.Text = "0";
			txtTotalWeight.Text = "0";
			txtTotalAmount.Text = "0";
			dtGridCustomerWise.Rows.Clear();
			grdCustomerWise.ClearStructure();
			MessageBox.Show("No Record Found!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetBreakupDataAgainstActivityBreakUpName(string ReportName)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.ReportType = ReportName;
			obj.InventoryParentCategories = Conversion.ToInt(cmbparentcategory.Value);
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.JobLotId = Conversion.ToInt(CmbJotLot.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
			obj.CropYear = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
			obj.BranchesIds = (BranchFeature ? BranchIds : "");
			if (ReportName == "Item And Customer Wise Comparison")
			{
				dtBreakUpDataForItemWise = InvSaleInvoiceReports.InventoryStockEvaluation_LocalSalesComaprisonsByCustomerItem_Report(obj);
			}
			else if (ReportName == "Customer And Item Wise Comparison")
			{
				dtBreakUpDataForSupplierWise = InvSaleInvoiceReports.InventoryStockEvaluation_LocalSalesComaprisonsByCustomerItem_Report(obj);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetComparisonDataAgainstSummary(string ReportName)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.ReportType = ReportName;
			obj.InventoryParentCategories = Conversion.ToInt(cmbparentcategory.Value);
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(CmbParty.Value);
			obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			obj.JobLotId = Conversion.ToInt(CmbJotLot.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
			obj.CropYear = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
			obj.BranchesIds = (BranchFeature ? BranchIds : "");
			if (ReportName == "Item Wise Sales Summary")
			{
				dtGridItemWise = InvSaleInvoiceReports.InventoryStockEvaluation_LocalSalesComaprisons_Report(obj);
				if (dtGridItemWise.Rows.Count > 0)
				{
					DataTable dtGrid = new DataTable();
					dtGrid.Columns.Add("BranchesId", typeof(int));
					dtGrid.Columns.Add("BranchName", typeof(string));
					dtGrid.Columns.Add("GroupCaption", typeof(string));
					dtGrid.Columns.Add("GroupId", typeof(int));
					dtGrid.Columns.Add("GroupName", typeof(string));
					dtGrid.Columns.Add("CropYear", typeof(string));
					dtGrid.Columns.Add("ParentCategory", typeof(string));
					dtGrid.Columns.Add("TotalAmount", typeof(double));
					dtGrid.Columns.Add("PrctOfTotalAmount", typeof(double));
					dtGrid.Columns.Add("Qty", typeof(double));
					dtGrid.Columns.Add("WeightKg", typeof(double));
					dtGrid.Columns.Add("AvgRate", typeof(double));
					dtGrid.Columns.Add("ItemAmount", typeof(double));
					dtGrid.Columns.Add("AddExpenses", typeof(double));
					dtGrid.Columns.Add("LessExpenses", typeof(double));
					dtGrid.Columns.Add("Amount", typeof(double));
					dtGrid.Columns.Add("AvgRateExp", typeof(double));
					dtGrid.Columns.Add("SalesAccountId", typeof(int));
					dtGrid.Columns.Add("SalesAccount", typeof(string));
					dtGrid.Clear();
					for (int i = 0; i < dtGridItemWise.Rows.Count; i++)
					{
						dtGrid.Rows.Add(dtGridItemWise.Rows[i]["BranchesId"], dtGridItemWise.Rows[i]["BranchName"], dtGridItemWise.Rows[i]["GroupCaption"], dtGridItemWise.Rows[i]["GroupId"], dtGridItemWise.Rows[i]["GroupName"], dtGridItemWise.Rows[i]["CropBatch"], dtGridItemWise.Rows[i]["ParentCategory"], dtGridItemWise.Rows[i]["TotalAmount"], dtGridItemWise.Rows[i]["PrctOfTotalAmount"], dtGridItemWise.Rows[i]["Qty"], dtGridItemWise.Rows[i]["WeightKg"], dtGridItemWise.Rows[i]["AvgRate"], dtGridItemWise.Rows[i]["ItemAmount"], dtGridItemWise.Rows[i]["AddExpenses"], dtGridItemWise.Rows[i]["LessExpenses"], dtGridItemWise.Rows[i]["Amount"], dtGridItemWise.Rows[i]["AvgRateExp"], dtGridItemWise.Rows[i]["SalesAccountId"], dtGridItemWise.Rows[i]["SalesAccount"]);
					}
					grdItemWise.DataSource = dtGrid;
					grdItemWise.RetrieveStructure();
					ItemWiseGridSettings();
				}
				else
				{
					grdItemWise.ClearStructure();
				}
				dtBreakUpDataForItemWise = InvSaleInvoiceReports.InventoryStockEvaluation_LocalSalesComaprisonsByCustomerItem_Report(obj);
			}
			else
			{
				if (!(ReportName == "Customer Wise Sales Summary"))
				{
					return;
				}
				dtGridCustomerWise = InvSaleInvoiceReports.InventoryStockEvaluation_LocalSalesComaprisons_Report(obj);
				if (dtGridCustomerWise.Rows.Count > 0)
				{
					DataTable dtGrid2 = new DataTable();
					dtGrid2.Columns.Add("BranchesId", typeof(int));
					dtGrid2.Columns.Add("BranchName", typeof(string));
					dtGrid2.Columns.Add("GroupCaption", typeof(string));
					dtGrid2.Columns.Add("GroupId", typeof(int));
					dtGrid2.Columns.Add("GroupName", typeof(string));
					dtGrid2.Columns.Add("CropYear", typeof(string));
					dtGrid2.Columns.Add("ParentCategory", typeof(string));
					dtGrid2.Columns.Add("TotalAmount", typeof(double));
					dtGrid2.Columns.Add("PrctOfTotalAmount", typeof(double));
					dtGrid2.Columns.Add("Qty", typeof(double));
					dtGrid2.Columns.Add("WeightKg", typeof(double));
					dtGrid2.Columns.Add("AvgRate", typeof(double));
					dtGrid2.Columns.Add("ItemAmount", typeof(double));
					dtGrid2.Columns.Add("AddExpenses", typeof(double));
					dtGrid2.Columns.Add("LessExpenses", typeof(double));
					dtGrid2.Columns.Add("Amount", typeof(double));
					dtGrid2.Columns.Add("AvgRateExp", typeof(double));
					dtGrid2.Clear();
					for (int j = 0; j < dtGridCustomerWise.Rows.Count; j++)
					{
						dtGrid2.Rows.Add(dtGridCustomerWise.Rows[j]["BranchesId"], dtGridCustomerWise.Rows[j]["BranchName"], dtGridCustomerWise.Rows[j]["GroupCaption"], dtGridCustomerWise.Rows[j]["GroupId"], dtGridCustomerWise.Rows[j]["GroupName"], dtGridCustomerWise.Rows[j]["CropBatch"], dtGridCustomerWise.Rows[j]["ParentCategory"], dtGridCustomerWise.Rows[j]["TotalAmount"], dtGridCustomerWise.Rows[j]["PrctOfTotalAmount"], dtGridCustomerWise.Rows[j]["Qty"], dtGridCustomerWise.Rows[j]["WeightKg"], dtGridCustomerWise.Rows[j]["AvgRate"], dtGridCustomerWise.Rows[j]["ItemAmount"], dtGridCustomerWise.Rows[j]["AddExpenses"], dtGridCustomerWise.Rows[j]["LessExpenses"], dtGridCustomerWise.Rows[j]["Amount"], dtGridCustomerWise.Rows[j]["AvgRateExp"]);
					}
					grdCustomerWise.DataSource = dtGrid2;
					grdCustomerWise.RetrieveStructure();
					CustomerWiseGridSettings();
				}
				else
				{
					grdCustomerWise.ClearStructure();
				}
				dtBreakUpDataForSupplierWise = InvSaleInvoiceReports.InventoryStockEvaluation_LocalSalesComaprisonsByCustomerItem_Report(obj);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemWiseGridSettings()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdItemWise.AllowDelete = (InheritableBoolean)2;
			grdItemWise.AllowEdit = (InheritableBoolean)2;
			grdItemWise.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdItemWise.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdItemWise.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdItemWise.RootTable.Columns["SalesAccountId"].Visible = false;
			grdItemWise.RootTable.Columns["BranchName"].Width = 120;
			grdItemWise.RootTable.Columns["GroupCaption"].Width = 70;
			grdItemWise.RootTable.Columns["GroupName"].Width = 150;
			grdItemWise.RootTable.Columns["CropYear"].Width = 70;
			grdItemWise.RootTable.Columns["ParentCategory"].Width = 70;
			grdItemWise.RootTable.Columns["TotalAmount"].Width = 70;
			grdItemWise.RootTable.Columns["PrctOfTotalAmount"].Width = 65;
			grdItemWise.RootTable.Columns["Qty"].Width = 70;
			grdItemWise.RootTable.Columns["WeightKg"].Width = 90;
			grdItemWise.RootTable.Columns["AvgRate"].Width = 70;
			grdItemWise.RootTable.Columns["AvgRateExp"].Width = 70;
			grdItemWise.RootTable.Columns["ItemAmount"].Width = 110;
			grdItemWise.RootTable.Columns["AddExpenses"].Width = 90;
			grdItemWise.RootTable.Columns["LessExpenses"].Width = 90;
			grdItemWise.RootTable.Columns["Amount"].Width = 110;
			grdItemWise.RootTable.Columns["SalesAccount"].Width = Constants.InventoryConstants.AccountTitle + 30;
			grdItemWise.RootTable.Columns.Add("BreakUp");
			grdItemWise.RootTable.Columns["BreakUp"].Caption = "BreakUp";
			grdItemWise.RootTable.Columns["BreakUp"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdItemWise.RootTable.Columns["BreakUp"].ButtonStyle = (ButtonStyle)5;
			grdItemWise.RootTable.Columns["BreakUp"].ButtonText = "Party Wise";
			grdItemWise.RootTable.Columns["BreakUp"].Width = 80;
			grdItemWise.RootTable.Columns["BreakUp"].ButtonToolTipText = "Party Wise Break Up";
			grdItemWise.RootTable.Columns["BreakUp"].Position = 0;
			grdItemWise.FrozenColumns = 1;
			grdItemWise.RootTable.Columns["BranchesId"].Visible = false;
			grdItemWise.RootTable.Columns["GroupCaption"].Visible = false;
			grdItemWise.RootTable.Columns["GroupId"].Visible = false;
			grdItemWise.RootTable.Columns["ParentCategory"].Visible = false;
			grdItemWise.RootTable.Columns["TotalAmount"].Visible = false;
			grdItemWise.RootTable.Columns["GroupName"].ColumnType = (ColumnType)5;
			grdItemWise.RootTable.Columns["SalesAccount"].ColumnType = (ColumnType)5;
			grdItemWise.RootTable.Columns["GroupName"].Caption = "Item Name";
			grdItemWise.RootTable.Columns["PrctOfTotalAmount"].Caption = "% Of Total Amount";
			grdItemWise.RootTable.Columns["AvgRateExp"].Caption = "Avg Rate + Expenses";
			grdItemWise.RootTable.Columns["PrctOfTotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdItemWise.RootTable.Columns["PrctOfTotalAmount"].TextAlignment = (TextAlignment)3;
			grdItemWise.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdItemWise.RootTable.Columns["WeightKg"].AggregateFunction = (AggregateFunction)2;
			grdItemWise.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdItemWise.RootTable.Columns["AddExpenses"].AggregateFunction = (AggregateFunction)2;
			grdItemWise.RootTable.Columns["LessExpenses"].AggregateFunction = (AggregateFunction)2;
			grdItemWise.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdItemWise.RootTable.Columns["AvgRateExp"].AggregateFunction = (AggregateFunction)3;
			grdItemWise.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdItemWise.RootTable.Columns["WeightKg"].TextAlignment = (TextAlignment)3;
			grdItemWise.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdItemWise.RootTable.Columns["AddExpenses"].TextAlignment = (TextAlignment)3;
			grdItemWise.RootTable.Columns["LessExpenses"].TextAlignment = (TextAlignment)3;
			grdItemWise.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdItemWise.RootTable.Columns["AvgRateExp"].TextAlignment = (TextAlignment)3;
			grdItemWise.RootTable.Columns["PrctOfTotalAmount"].FormatString = "#,#.##";
			grdItemWise.RootTable.Columns["Qty"].FormatString = "#,#";
			grdItemWise.RootTable.Columns["WeightKg"].FormatString = "#,#";
			grdItemWise.RootTable.Columns["ItemAmount"].FormatString = "#,#.##";
			grdItemWise.RootTable.Columns["AddExpenses"].FormatString = "#,#.##";
			grdItemWise.RootTable.Columns["LessExpenses"].FormatString = "#,#.##";
			grdItemWise.RootTable.Columns["Amount"].FormatString = "#,#.##";
			grdItemWise.RootTable.Columns["AvgRateExp"].FormatString = "#,#.##";
			grdItemWise.RootTable.Columns["PrctOfTotalAmount"].TotalFormatString = "#,#.##";
			grdItemWise.RootTable.Columns["Qty"].TotalFormatString = "#,#";
			grdItemWise.RootTable.Columns["WeightKg"].TotalFormatString = "#,#";
			grdItemWise.RootTable.Columns["ItemAmount"].TotalFormatString = "#,#.##";
			grdItemWise.RootTable.Columns["AddExpenses"].TotalFormatString = "#,#.##";
			grdItemWise.RootTable.Columns["LessExpenses"].TotalFormatString = "#,#.##";
			grdItemWise.RootTable.Columns["Amount"].TotalFormatString = "#,#.##";
			grdItemWise.RootTable.Columns["AvgRateExp"].TotalFormatString = "#,#.##";
			ctrlGrdBar1_Load(null, null);
			panel7.Width = ((Control)(object)grdItemWise).Width;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdItemWise_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdItemWise.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "GroupName")
				{
					frmEvaulationDetailSalesReports obj = new frmEvaulationDetailSalesReports(UserAccount)
					{
						FromDate = fromdate.Value,
						ToDate = todate.Value
					};
					obj.cmbItem.Value = Conversion.ToInt(r.Cells["GroupId"].Value);
					string BranchId = Conversion.ToString(r.Cells["BranchesId"].Value);
					obj.BranchesIds = ((!BranchFeature) ? "" : ((BranchId != null && BranchId != string.Empty) ? BranchId : ""));
					obj.WindowState = FormWindowState.Maximized;
					obj.ShowDialog();
				}
				else if (e.Column.Key == "SalesAccount")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["SalesAccountId"].Value), fromdate.Value, todate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdItemWise_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (!(e.Column.Key == "BreakUp"))
			{
				return;
			}
			DataTable dtForBreakup = new DataTable();
			dtForBreakup = dtBreakUpDataForItemWise.Clone();
			for (int i = 0; i < dtBreakUpDataForItemWise.Rows.Count; i++)
			{
				string obj = grdItemWise.CurrentRow.Cells["GroupName"].Value.ToString();
				string BranchesId = grdItemWise.CurrentRow.Cells["BranchesId"].Value.ToString();
				if (!(obj == dtBreakUpDataForItemWise.Rows[i]["FirstGroupName"].ToString()))
				{
					continue;
				}
				if (BranchesId != null && BranchesId != string.Empty)
				{
					if (BranchesId == dtBreakUpDataForItemWise.Rows[i]["BranchesId"].ToString())
					{
						DataRow dr = dtBreakUpDataForItemWise.Rows[i];
						dtForBreakup.Rows.Add(dr.ItemArray);
					}
				}
				else
				{
					DataRow dr2 = dtBreakUpDataForItemWise.Rows[i];
					dtForBreakup.Rows.Add(dr2.ItemArray);
				}
			}
			bool flag = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			AnalayticalAnalysisPopUp analayticalAnalysisPopUp = new AnalayticalAnalysisPopUp(UserAccount, dtForBreakup, 0, flag);
			analayticalAnalysisPopUp.WindowState = FormWindowState.Maximized;
			analayticalAnalysisPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerWiseGridSettings()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdCustomerWise.AllowDelete = (InheritableBoolean)2;
			grdCustomerWise.AllowEdit = (InheritableBoolean)2;
			grdCustomerWise.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdCustomerWise.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdCustomerWise.RootTable.Columns.Add("BreakUp");
			grdCustomerWise.RootTable.Columns["BreakUp"].Caption = "Break Up";
			grdCustomerWise.RootTable.Columns["BreakUp"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdCustomerWise.RootTable.Columns["BreakUp"].ButtonStyle = (ButtonStyle)5;
			grdCustomerWise.RootTable.Columns["BreakUp"].ButtonText = "Item Wise";
			grdCustomerWise.RootTable.Columns["BreakUp"].Width = 80;
			grdCustomerWise.RootTable.Columns["BreakUp"].ButtonToolTipText = "ItemWise BreakUp";
			grdCustomerWise.RootTable.Columns["BreakUp"].Position = 0;
			grdCustomerWise.FrozenColumns = 1;
			grdCustomerWise.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdCustomerWise.RootTable.Columns["BranchName"].Width = 120;
			grdCustomerWise.RootTable.Columns["GroupCaption"].Width = 70;
			grdCustomerWise.RootTable.Columns["GroupName"].Width = 150;
			grdCustomerWise.RootTable.Columns["CropYear"].Width = 70;
			grdCustomerWise.RootTable.Columns["ParentCategory"].Width = 70;
			grdCustomerWise.RootTable.Columns["TotalAmount"].Width = 100;
			grdCustomerWise.RootTable.Columns["PrctOfTotalAmount"].Width = 70;
			grdCustomerWise.RootTable.Columns["Qty"].Width = 70;
			grdCustomerWise.RootTable.Columns["WeightKg"].Width = 90;
			grdCustomerWise.RootTable.Columns["AvgRate"].Width = 80;
			grdCustomerWise.RootTable.Columns["AvgRateExp"].Width = 80;
			grdCustomerWise.RootTable.Columns["Amount"].Width = 120;
			grdCustomerWise.RootTable.Columns["ItemAmount"].Width = 120;
			grdCustomerWise.RootTable.Columns["AddExpenses"].Width = 90;
			grdCustomerWise.RootTable.Columns["LessExpenses"].Width = 90;
			grdCustomerWise.RootTable.Columns["BranchesId"].Visible = false;
			grdCustomerWise.RootTable.Columns["GroupCaption"].Visible = false;
			grdCustomerWise.RootTable.Columns["GroupId"].Visible = false;
			grdCustomerWise.RootTable.Columns["ParentCategory"].Visible = false;
			grdCustomerWise.RootTable.Columns["TotalAmount"].Visible = false;
			grdCustomerWise.RootTable.Columns["GroupName"].ColumnType = (ColumnType)5;
			grdCustomerWise.RootTable.Columns["GroupName"].Caption = "Party Name";
			grdCustomerWise.RootTable.Columns["PrctOfTotalAmount"].Caption = "% Of Total Amount";
			grdCustomerWise.RootTable.Columns["AvgRateExp"].Caption = "Avg Rate + Expenses";
			grdCustomerWise.RootTable.Columns["PrctOfTotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdCustomerWise.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdCustomerWise.RootTable.Columns["WeightKg"].AggregateFunction = (AggregateFunction)2;
			grdCustomerWise.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdCustomerWise.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdCustomerWise.RootTable.Columns["AddExpenses"].AggregateFunction = (AggregateFunction)2;
			grdCustomerWise.RootTable.Columns["LessExpenses"].AggregateFunction = (AggregateFunction)2;
			grdCustomerWise.RootTable.Columns["AvgRateExp"].AggregateFunction = (AggregateFunction)3;
			grdCustomerWise.RootTable.Columns["PrctOfTotalAmount"].TextAlignment = (TextAlignment)3;
			grdCustomerWise.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdCustomerWise.RootTable.Columns["WeightKg"].TextAlignment = (TextAlignment)3;
			grdCustomerWise.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdCustomerWise.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdCustomerWise.RootTable.Columns["AddExpenses"].TextAlignment = (TextAlignment)3;
			grdCustomerWise.RootTable.Columns["LessExpenses"].TextAlignment = (TextAlignment)3;
			grdCustomerWise.RootTable.Columns["AvgRateExp"].TextAlignment = (TextAlignment)3;
			grdCustomerWise.RootTable.Columns["PrctOfTotalAmount"].FormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["Qty"].FormatString = "#,#";
			grdCustomerWise.RootTable.Columns["WeightKg"].FormatString = "#,#";
			grdCustomerWise.RootTable.Columns["Amount"].FormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["ItemAmount"].FormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["AddExpenses"].FormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["LessExpenses"].FormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["AvgRateExp"].FormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["PrctOfTotalAmount"].TotalFormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["Qty"].TotalFormatString = "#,#";
			grdCustomerWise.RootTable.Columns["WeightKg"].TotalFormatString = "#,#";
			grdCustomerWise.RootTable.Columns["Amount"].TotalFormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["ItemAmount"].TotalFormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["AddExpenses"].TotalFormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["LessExpenses"].TotalFormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["AvgRateExp"].TotalFormatString = "#,#.##";
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdCustomerWise_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			DataTable dtForBreakup = new DataTable();
			dtForBreakup = dtBreakUpDataForSupplierWise.Clone();
			for (int i = 0; i < dtBreakUpDataForSupplierWise.Rows.Count; i++)
			{
				string obj = grdCustomerWise.CurrentRow.Cells["GroupName"].Value.ToString();
				string BranchesId = grdCustomerWise.CurrentRow.Cells["BranchesId"].Value.ToString();
				if (!(obj == dtBreakUpDataForSupplierWise.Rows[i]["FirstGroupName"].ToString()))
				{
					continue;
				}
				if (BranchesId != null && BranchesId != string.Empty)
				{
					if (BranchesId == dtBreakUpDataForSupplierWise.Rows[i]["BranchesId"].ToString())
					{
						DataRow dr = dtBreakUpDataForSupplierWise.Rows[i];
						dtForBreakup.Rows.Add(dr.ItemArray);
					}
				}
				else
				{
					DataRow dr2 = dtBreakUpDataForSupplierWise.Rows[i];
					dtForBreakup.Rows.Add(dr2.ItemArray);
				}
			}
			bool flag = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			AnalayticalAnalysisPopUp analayticalAnalysisPopUp = new AnalayticalAnalysisPopUp(UserAccount, dtForBreakup, 0, flag);
			analayticalAnalysisPopUp.WindowState = FormWindowState.Maximized;
			analayticalAnalysisPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerWise_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			frmEvaulationDetailSalesReports obj = new frmEvaulationDetailSalesReports(UserAccount)
			{
				FromDate = fromdate.Value,
				ToDate = todate.Value
			};
			obj.cmbSupplierCustomer.Value = Conversion.ToInt(grdCustomerWise.CurrentRow.Cells["GroupId"].Value);
			string BranchId = Conversion.ToString(grdCustomerWise.CurrentRow.Cells["BranchesId"].Value);
			obj.BranchesIds = ((!BranchFeature) ? "" : ((BranchId != null && BranchId != string.Empty) ? BranchId : ""));
			obj.WindowState = FormWindowState.Maximized;
			obj.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTotals()
	{
		try
		{
			double qty = Conversion.ToDouble(grdCustomerWise.GetTotal(grdCustomerWise.RootTable.Columns["Qty"], (AggregateFunction)2));
			txtTotalQty.Text = qty.ToString("#,#;(#,#);0");
			double weight = Conversion.ToDouble(grdCustomerWise.GetTotal(grdCustomerWise.RootTable.Columns["WeightKg"], (AggregateFunction)2));
			txtTotalWeight.Text = weight.ToString("#,#;(#,#);0");
			double amount = Conversion.ToDouble(dtGridCustomerWise.Rows[0]["TotalAmount"]);
			txtTotalAmount.Text = amount.ToString("#,#;(#,#);0");
			double AvgRate = Conversion.ToDouble((object)txtTotalAmount.Text) / Conversion.ToDouble((object)txtTotalWeight.Text) * 40.0;
			grdItemWise.RootTable.Columns["AvgRate"].Caption = "Avg Rate 40Kg";
			grdItemWise.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdItemWise.RootTable.Columns["AvgRate"].FormatString = "#,#.##";
			grdItemWise.RootTable.Columns["AvgRate"].TotalFormatString = "#,#.##";
			GridEXRow totalRow = grdItemWise.GetTotalRow();
			if (totalRow != null)
			{
				totalRow.Cells["AvgRate"].Value = AvgRate;
				totalRow.Cells["AvgRate"].Text = AvgRate.ToString("#,#.##");
			}
			grdCustomerWise.RootTable.Columns["AvgRate"].Caption = "Avg Rate 40Kg";
			grdCustomerWise.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdCustomerWise.RootTable.Columns["AvgRate"].FormatString = "#,#.##";
			grdCustomerWise.RootTable.Columns["AvgRate"].TotalFormatString = "#,#.##";
			GridEXRow totalRow_Customer = grdCustomerWise.GetTotalRow();
			if (totalRow_Customer != null)
			{
				totalRow_Customer.Cells["AvgRate"].Value = AvgRate;
				totalRow_Customer.Cells["AvgRate"].Text = AvgRate.ToString("#,#.##");
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdItemWise).Name);
			if (fs != null)
			{
				grdItemWise.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdCustomerWise).Name);
			if (fs != null)
			{
				grdCustomerWise.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ParentCategoryFill();
			if (((UltraGridBase)cmbparentcategory).ActiveRow != null && Conversion.ToInt(cmbparentcategory.Value) > 0)
			{
				AllComboBind();
				return;
			}
			((UltraGridBase)CmbItemCategory).DataSource = null;
			((Control)(object)CmbItemCategory).Text = "";
			((UltraGridBase)CmbItemType).DataSource = null;
			((Control)(object)CmbItemType).Text = "";
			((UltraGridBase)CmbItemName).DataSource = null;
			((Control)(object)CmbItemName).Text = "";
			((UltraGridBase)CmbCropYear).DataSource = null;
			((Control)(object)CmbCropYear).Text = "";
			((UltraGridBase)CmbJotLot).DataSource = null;
			((Control)(object)CmbJotLot).Text = "";
			((UltraGridBase)CmbParty).DataSource = null;
			((Control)(object)CmbParty).Text = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SalesAnalyticsDashBoard_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnPrintItemWise_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGridItemWise == null || dtGridItemWise.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtGridItemWise, "122_03_SaleAnalyticsItemWise.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnPrintSupplierWise_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGridCustomerWise == null || dtGridCustomerWise.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtGridCustomerWise, "122_04_SaleAnalyticsCustomerWise.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Expected O, but got Unknown
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Expected O, but got Unknown
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Expected O, but got Unknown
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Expected O, but got Unknown
		//IL_0bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c07: Expected O, but got Unknown
		GridEXLayout grdCustomerWise_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdItemWise_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AnalyticDashboard.SalesAnalyticsDashBoard));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdCustomerWise = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdItemWise = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.cmbparentcategory = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.txtTotalAmount = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.btnRefresh = new System.Windows.Forms.Button();
		this.panel11 = new System.Windows.Forms.Panel();
		this.txtTotalWeight = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.txtTotalQty = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbItemType = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbJotLot = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbItemCategory = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbParty = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.lblComparison = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrintItemWise = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSupplierWise = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerWise).BeginInit();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItemWise).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcategory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJotLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParty).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel13);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1030, 661);
		this.panel1.TabIndex = 0;
		this.panel13.AutoScroll = true;
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerWise);
		this.panel13.Controls.Add(this.panel9);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 449);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1030, 212);
		this.panel13.TabIndex = 52;
		this.grdCustomerWise.AllowEdit = (InheritableBoolean)2;
		this.grdCustomerWise.AllowRemoveColumns = (InheritableBoolean)1;
		grdCustomerWise_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerWise.DesignTimeLayout = grdCustomerWise_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerWise).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdCustomerWise.DynamicFiltering = true;
		this.grdCustomerWise.FilterMode = (FilterMode)2;
		this.grdCustomerWise.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCustomerWise).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerWise.GroupByBoxVisible = false;
		this.grdCustomerWise.GroupTotals = (GroupTotals)2;
		this.grdCustomerWise.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdCustomerWise).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdCustomerWise).Name = "grdCustomerWise";
		((ScrollableBase)this.grdCustomerWise).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerWise).Size = new System.Drawing.Size(1030, 187);
		((System.Windows.Forms.Control)(object)this.grdCustomerWise).TabIndex = 53;
		this.grdCustomerWise.TotalRow = (InheritableBoolean)1;
		this.grdCustomerWise.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerWise).VisualStyle = (VisualStyle)4;
		this.grdCustomerWise.ColumnButtonClick += new ColumnActionEventHandler(grdCustomerWise_ColumnButtonClick);
		this.grdCustomerWise.LinkClicked += new ColumnActionEventHandler(grdCustomerWise_LinkClicked);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label8);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1030, 25);
		this.panel9.TabIndex = 52;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(988, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdCustomerWise;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 23);
		this.ctrlGrdBar2.TabIndex = 49;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(198, 19);
		this.label8.TabIndex = 3;
		this.label8.Text = "Party Wise Comparison";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel8.AutoScroll = true;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdItemWise);
		this.panel8.Controls.Add(this.panel7);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 199);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1030, 250);
		this.panel8.TabIndex = 50;
		this.grdItemWise.AllowEdit = (InheritableBoolean)2;
		this.grdItemWise.AlternatingColors = true;
		grdItemWise_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdItemWise.DesignTimeLayout = grdItemWise_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdItemWise).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdItemWise.DynamicFiltering = true;
		this.grdItemWise.FilterMode = (FilterMode)2;
		this.grdItemWise.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdItemWise).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItemWise.GroupByBoxVisible = false;
		this.grdItemWise.GroupTotals = (GroupTotals)2;
		this.grdItemWise.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdItemWise).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdItemWise).Name = "grdItemWise";
		((ScrollableBase)this.grdItemWise).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItemWise).Size = new System.Drawing.Size(1030, 221);
		((System.Windows.Forms.Control)(object)this.grdItemWise).TabIndex = 51;
		this.grdItemWise.TotalRow = (InheritableBoolean)1;
		this.grdItemWise.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItemWise).VisualStyle = (VisualStyle)4;
		this.grdItemWise.ColumnButtonClick += new ColumnActionEventHandler(grdItemWise_ColumnButtonClick);
		this.grdItemWise.LinkClicked += new ColumnActionEventHandler(grdItemWise_LinkClicked);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1030, 29);
		this.panel7.TabIndex = 50;
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(3, 5);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(193, 19);
		this.label7.TabIndex = 2;
		this.label7.Text = "Item Wise Comparison";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(986, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdItemWise;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 48;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel5.Controls.Add(this.Branches);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbparentcategory);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Controls.Add(this.btnRefresh);
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJotLot);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParty);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.todate);
		this.panel5.Controls.Add(this.fromdate);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 53);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1030, 146);
		this.panel5.TabIndex = 0;
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(102, 4);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 0;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(9, 10);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 596;
		this.Branches.Text = "Branches";
		this.cmbparentcategory.AllowNull = (DefaultableBoolean)1;
		this.cmbparentcategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparentcategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparentcategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbparentcategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).Location = new System.Drawing.Point(102, 58);
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).Name = "cmbparentcategory";
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).Leave += new System.EventHandler(cmbparentcategory_Leave);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(8, 64);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(91, 15);
		this.label10.TabIndex = 121;
		this.label10.Text = "Parent Category";
		this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel12.Controls.Add(this.txtTotalAmount);
		this.panel12.Controls.Add(this.label13);
		this.panel12.Location = new System.Drawing.Point(692, 85);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(253, 52);
		this.panel12.TabIndex = 3;
		this.panel12.TabStop = true;
		this.txtTotalAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(-1, 20);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.Size = new System.Drawing.Size(253, 30);
		this.txtTotalAmount.TabIndex = 2;
		this.txtTotalAmount.Text = "0";
		this.txtTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(-1, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(253, 20);
		this.label13.TabIndex = 1;
		this.label13.Text = "Total Amount";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnRefresh.FlatAppearance.BorderSize = 0;
		this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.ForeColor = System.Drawing.Color.White;
		this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnRefresh.Location = new System.Drawing.Point(948, 4);
		this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(73, 26);
		this.btnRefresh.TabIndex = 11;
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.UseVisualStyleBackColor = false;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel11.Controls.Add(this.txtTotalWeight);
		this.panel11.Controls.Add(this.label11);
		this.panel11.Location = new System.Drawing.Point(400, 86);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(218, 52);
		this.panel11.TabIndex = 3;
		this.panel11.TabStop = true;
		this.txtTotalWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalWeight.Location = new System.Drawing.Point(1, 23);
		this.txtTotalWeight.Name = "txtTotalWeight";
		this.txtTotalWeight.Size = new System.Drawing.Size(216, 27);
		this.txtTotalWeight.TabIndex = 2;
		this.txtTotalWeight.Text = "0";
		this.txtTotalWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label11.BackColor = System.Drawing.Color.Teal;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(0, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(217, 23);
		this.label11.TabIndex = 1;
		this.label11.Text = "Total Weight";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(948, 58);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(73, 26);
		this.btnshow.TabIndex = 10;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel10.Controls.Add(this.label9);
		this.panel10.Controls.Add(this.txtTotalQty);
		this.panel10.Location = new System.Drawing.Point(102, 86);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(211, 52);
		this.panel10.TabIndex = 0;
		this.panel10.TabStop = true;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, -1);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(210, 23);
		this.label9.TabIndex = 1;
		this.label9.Text = "Total Qty";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtTotalQty.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalQty.Location = new System.Drawing.Point(6, 19);
		this.txtTotalQty.Name = "txtTotalQty";
		this.txtTotalQty.Size = new System.Drawing.Size(211, 30);
		this.txtTotalQty.TabIndex = 2;
		this.txtTotalQty.Text = "0";
		this.txtTotalQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(400, 58);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(316, 64);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(57, 15);
		this.label4.TabIndex = 115;
		this.label4.Text = "Crop Year";
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(400, 31);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(316, 37);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 15);
		this.label6.TabIndex = 119;
		this.label6.Text = "Item Type";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(693, 31);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 8;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(624, 37);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 117;
		this.label5.Text = "Item Name";
		this.CmbJotLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJotLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJotLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJotLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJotLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbJotLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJotLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJotLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJotLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJotLot).Location = new System.Drawing.Point(693, 4);
		((System.Windows.Forms.Control)(object)this.CmbJotLot).Name = "CmbJotLot";
		((System.Windows.Forms.Control)(object)this.CmbJotLot).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbJotLot).TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(623, 10);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 15);
		this.label3.TabIndex = 113;
		this.label3.Text = "Job Lot";
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(400, 4);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(315, 10);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(82, 15);
		this.label2.TabIndex = 111;
		this.label2.Text = "Item Category";
		this.CmbParty.AllowNull = (DefaultableBoolean)1;
		this.CmbParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.CmbParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParty).Location = new System.Drawing.Point(693, 58);
		((System.Windows.Forms.Control)(object)this.CmbParty).Name = "CmbParty";
		((System.Windows.Forms.Control)(object)this.CmbParty).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbParty).TabIndex = 9;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(624, 64);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 109;
		this.label1.Text = "Party Name";
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(208, 33);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(105, 22);
		this.todate.TabIndex = 2;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(102, 33);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(105, 22);
		this.fromdate.TabIndex = 1;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(8, 37);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(79, 15);
		this.label27.TabIndex = 107;
		this.label27.Text = "From,To Date";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.lblComparison);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1030, 53);
		this.panel3.TabIndex = 1;
		this.lblComparison.AutoSize = true;
		this.lblComparison.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblComparison.ForeColor = System.Drawing.Color.White;
		this.lblComparison.Location = new System.Drawing.Point(1, 28);
		this.lblComparison.Name = "lblComparison";
		this.lblComparison.Size = new System.Drawing.Size(223, 19);
		this.lblComparison.TabIndex = 1;
		this.lblComparison.Text = "Sales Analytics Dashboard";
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnPrintItemWise, this.btnPrintSupplierWise });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1030, 25);
		this.toolStrip1.TabIndex = 4;
		this.toolStrip1.Text = "toolStrip1";
		this.btnPrintItemWise.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintItemWise.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintItemWise.Name = "btnPrintItemWise";
		this.btnPrintItemWise.Size = new System.Drawing.Size(116, 22);
		this.btnPrintItemWise.Text = "122_03_ItemWise";
		this.btnPrintItemWise.Click += new System.EventHandler(btnPrintItemWise_Click);
		this.btnPrintSupplierWise.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSupplierWise.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSupplierWise.Name = "btnPrintSupplierWise";
		this.btnPrintSupplierWise.Size = new System.Drawing.Size(144, 22);
		this.btnPrintSupplierWise.Text = "122_04_CustomerWise";
		this.btnPrintSupplierWise.Click += new System.EventHandler(btnPrintSupplierWise_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1030, 661);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "SalesAnalyticsDashBoard";
		this.Text = "frmAnalyticsDashboard";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SalesAnalyticsDashBoard_FormClosing);
		base.Load += new System.EventHandler(frmAnalyticsDashboard_Load);
		this.panel1.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerWise).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdItemWise).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcategory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJotLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParty).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
