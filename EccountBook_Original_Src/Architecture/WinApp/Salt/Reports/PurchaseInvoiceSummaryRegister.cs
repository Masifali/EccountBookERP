using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Salt.Reports;

public class PurchaseInvoiceSummaryRegister : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtDetail = new DataTable();

	private DataTable dtComparisonByItem = new DataTable();

	private DataTable dtComparisonByItemSupplier = new DataTable();

	private DataTable dtGridSummary = new DataTable();

	private DataTable dtPurchaseRegister = new DataTable();

	private DataTable dtPurchaseSummaryByItem = new DataTable();

	private DataTable dtPurchaseSummaryBySupplier = new DataTable();

	private DataTable dtPurchaseSummaryByItemAndSupplier = new DataTable();

	private DataTable dtPurchaseSummaryByItemAndWarehouse = new DataTable();

	private DataTable dtPurchaseSummaryByParentCategory = new DataTable();

	private DataTable dtPurchaseSummaryByParentCategoryAndSupplier = new DataTable();

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

	private Label label3;

	internal UltraCombo cmbparentcategoryCombo;

	private Label label11;

	internal UltraCombo cmbjoblot;

	private Label label10;

	internal UltraCombo cmbwarehouse;

	private Label label9;

	internal UltraCombo cmbItemName;

	private Label label7;

	internal UltraCombo cmbPackingtype;

	private Label label6;

	internal UltraCombo cmbItemClass;

	private Label label5;

	internal UltraCombo cmbItemCategory;

	private Label label4;

	internal UltraCombo cmbItemtype;

	private Label label14;

	internal UltraCombo cmbcommagent;

	private Label label13;

	internal UltraCombo cmbsupplier;

	private Label label15;

	private TextBox txtToDocNo;

	private Label label16;

	private TextBox txtFromDocNo;

	private Label label17;

	private TabControl tabControl1;

	private TabPage Detail;

	private Label label18;

	private TabPage Summary;

	internal GridEX GridSummary;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar5;

	private Panel panel7;

	private GroupBox groupBox2;

	internal UltraCombo CmbDateTypeSumm;

	private Label label40;

	internal UltraCombo CmbItemClassSumm;

	internal UltraCombo cmbItemtypeSumm;

	private Label label26;

	internal UltraCombo CmbPackingTypeSumm;

	private Label label19;

	private Label label20;

	internal UltraCombo cmbSupplierSumm;

	internal UltraCombo cmbDistrictSumm;

	private Label label30;

	internal UltraCombo cmbcitySumm;

	private Label label21;

	private Label label31;

	internal UltraCombo cmbReportTypeSummary;

	internal UltraCombo cmbItemNameSumm;

	private Label label22;

	private Label label29;

	internal UltraCombo cmbJobLotSumm;

	private DateTimePicker txtFromDateSumm;

	private Label label32;

	private Label label23;

	private DateTimePicker txtToDateSumm;

	private TextBox txtFromDocNoSumm;

	private TextBox txtToDocNoSumm;

	private Button BtnShowSumm;

	private Label label24;

	internal UltraCombo cmbParentcategorySumm;

	private Label label27;

	internal UltraCombo CmbCategorySumm;

	private Label label33;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewSummary;

	private ToolStripButton btnRefreshSummary;

	private ToolStripButton btnPrintSumm;

	internal UltraCombo CmbWareHouseSumm;

	private Label label25;

	private CheckBox chkPackUom;

	private CheckBox chkCity;

	private ToolStripButton btnShortcutKeys;

	private ToolStripButton BtnReferesh;

	private ToolStripButton BtnShortCutKeysSumm;

	public PurchaseInvoiceSummaryRegister(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmEvaulationDetailSalesReports_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 500);
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("DocumentTypeId", typeof(int));
			dtDetail.Columns.Add("TransactionType", typeof(string));
			dtDetail.Columns.Add("BillDate", typeof(DateTime));
			dtDetail.Columns.Add("BillNo", typeof(int));
			dtDetail.Columns.Add("ManualNo", typeof(string));
			dtDetail.Columns.Add("SupplierName", typeof(string));
			dtDetail.Columns.Add("ParentCategory", typeof(string));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("PackUom", typeof(string));
			dtDetail.Columns.Add("JobLot", typeof(string));
			dtDetail.Columns.Add("PackingType", typeof(string));
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("BillWeight", typeof(double));
			dtDetail.Columns.Add("StockWeight", typeof(double));
			dtDetail.Columns.Add("ItemRate", typeof(double));
			dtDetail.Columns.Add("BillAmount", typeof(double));
			dtDetail.Columns.Add("Commission", typeof(double));
			dtDetail.Columns.Add("Freight", typeof(double));
			dtDetail.Columns.Add("Expense", typeof(double));
			dtDetail.Columns.Add("Wages", typeof(double));
			dtDetail.Columns.Add("TotalExpense", typeof(double));
			dtDetail.Columns.Add("Amount+Exp", typeof(double));
			dtDetail.Columns.Add("GpNo", typeof(int));
			dtDetail.Columns.Add("GpDate", typeof(DateTime));
			dtDetail.Columns.Add("VehicleNo", typeof(string));
			dtComparisonByItem.Columns.Add("ParentCategory", typeof(string));
			dtComparisonByItem.Columns.Add("ItemName", typeof(string));
			dtComparisonByItem.Columns.Add("ItemQty", typeof(double));
			dtComparisonByItem.Columns.Add("BillWeight", typeof(double));
			dtComparisonByItem.Columns.Add("StockWeight", typeof(double));
			dtComparisonByItem.Columns.Add("ItemRate", typeof(double));
			dtComparisonByItem.Columns.Add("BillAmount", typeof(double));
			dtComparisonByItem.Columns.Add("TotalExpense", typeof(double));
			dtComparisonByItem.Columns.Add("Amount+Exp", typeof(double));
			dtComparisonByItem.Columns.Add("RateWithExp", typeof(double));
			dtComparisonByItem.Columns.Add("TotalExp40Kg", typeof(double));
			dtComparisonByItem.Columns.Add("Freight40Kg", typeof(double));
			dtComparisonByItem.Columns.Add("Comm40Kg", typeof(double));
			dtComparisonByItem.Columns.Add("Expense40Kg", typeof(double));
			dtComparisonByItem.Columns.Add("Wages40Kg", typeof(double));
			dtComparisonByItem.Columns.Add("Freight", typeof(double));
			dtComparisonByItem.Columns.Add("Commission", typeof(double));
			dtComparisonByItem.Columns.Add("Expense", typeof(double));
			dtComparisonByItem.Columns.Add("Wages", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("ParentCategory", typeof(string));
			dtComparisonByItemSupplier.Columns.Add("SupplierName", typeof(string));
			dtComparisonByItemSupplier.Columns.Add("ItemName", typeof(string));
			dtComparisonByItemSupplier.Columns.Add("ItemQty", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("BillWeight", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("StockWeight", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("ItemRate", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("BillAmount", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("TotalExpense", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Amount+Exp", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("RateWithExp", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("TotalExp40Kg", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Freight40Kg", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Comm40Kg", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Expense40Kg", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Wages40Kg", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Freight", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Commission", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Expense", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Wages", typeof(double));
			dtPurchaseRegister.Columns.Add("DocDate", typeof(DateTime));
			dtPurchaseRegister.Columns.Add("RefDocumentTypeId", typeof(int));
			dtPurchaseRegister.Columns.Add("DocumentType", typeof(string));
			dtPurchaseRegister.Columns.Add("DocCodeNo", typeof(int));
			dtPurchaseRegister.Columns.Add("PartyName", typeof(string));
			dtPurchaseRegister.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseRegister.Columns.Add("ItemCategory", typeof(string));
			dtPurchaseRegister.Columns.Add("ItemType", typeof(string));
			dtPurchaseRegister.Columns.Add("ItemCode", typeof(string));
			dtPurchaseRegister.Columns.Add("ItemName", typeof(string));
			dtPurchaseRegister.Columns.Add("UOMCode", typeof(string));
			dtPurchaseRegister.Columns.Add("CropBatch", typeof(string));
			dtPurchaseRegister.Columns.Add("JobLotCode", typeof(string));
			dtPurchaseRegister.Columns.Add("PackTypeCode", typeof(string));
			dtPurchaseRegister.Columns.Add("VehicleNo", typeof(string));
			dtPurchaseRegister.Columns.Add("CityName", typeof(string));
			dtPurchaseRegister.Columns.Add("Qty", typeof(decimal));
			dtPurchaseRegister.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseRegister.Columns.Add("AddLess", typeof(decimal));
			dtPurchaseRegister.Columns.Add("BillWeight", typeof(decimal));
			dtPurchaseRegister.Columns.Add("StockWeight", typeof(decimal));
			dtPurchaseRegister.Columns.Add("AvgRate", typeof(decimal));
			dtPurchaseRegister.Columns.Add("Amount", typeof(decimal));
			dtPurchaseRegister.Columns.Add("TotalExpenses", typeof(decimal));
			dtPurchaseRegister.Columns.Add("TotalAmountWithExp", typeof(decimal));
			dtPurchaseRegister.Columns.Add("AvgRateExp", typeof(decimal));
			dtPurchaseRegister.Columns.Add("Freight", typeof(decimal));
			dtPurchaseRegister.Columns.Add("Commission", typeof(decimal));
			dtPurchaseRegister.Columns.Add("Expenses", typeof(decimal));
			dtPurchaseRegister.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseRegister.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemCategory", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemType", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemCode", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("AddLess", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("BillWeight", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("StockWeight", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("AvgRate", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("Amount", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("TotalExpenses", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("TotalAmountWithExp", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("AvgRateExp", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("Freight", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("Commission", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("Expenses", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryBySupplier.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryBySupplier.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryBySupplier.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("AddLess", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("BillWeight", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("StockWeight", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("AvgRate", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("Amount", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("TotalExpenses", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("TotalAmountWithExp", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("AvgRateExp", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("Freight", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("Commission", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("Expenses", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ItemCategory", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ItemType", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ItemCode", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("AddLess", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("BillWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("StockWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("AvgRate", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("Amount", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("TotalExpenses", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("TotalAmountWithExp", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("AvgRateExp", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("Freight", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("Commission", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("Expenses", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ItemCategory", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ItemType", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ItemCode", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("WarehouseName", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("AddLess", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("BillWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("StockWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("AvgRate", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("Amount", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("TotalExpenses", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("TotalAmountWithExp", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("AvgRateExp", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("Freight", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("Commission", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("Expenses", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByParentCategory.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByParentCategory.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByParentCategory.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("AddLess", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("BillWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("StockWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("AvgRate", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("Amount", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("TotalExpenses", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("TotalAmountWithExp", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("AvgRateExp", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("Freight", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("Commission", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("Expenses", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("AddLess", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("BillWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("StockWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("AvgRate", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("Amount", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("TotalExpenses", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("TotalAmountWithExp", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("AvgRateExp", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("Freight", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("Commission", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("Expenses", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			AllcomboFillFromPurchaseInvoice();
			AllcomboFillFromEvaluation();
			ActivityFill();
			ReportTypeSummaryFill();
			((UltraGridBase)cmbActivity).Rows[0].Activate();
			PrintButtonManage();
			ParameterFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDateTypeSumm, "Id", "Parameters", "Parameters", ZeroIndex: false);
				((UltraGridBase)CmbDateTypeSumm).Rows[1].Activate();
			}
			else
			{
				((Control)(object)CmbDateTypeSumm).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllcomboFillFromPurchaseInvoice()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCombo = InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtItemClass = new DataTable();
			dtItemClass.Columns.Add("Id");
			dtItemClass.Columns.Add("Name");
			dtItemClass.Clear();
			DataTable dtParentCategory = new DataTable();
			dtParentCategory.Columns.Add("Id");
			dtParentCategory.Columns.Add("Name");
			dtParentCategory.Clear();
			DataTable dtItemCategory = new DataTable();
			dtItemCategory.Columns.Add("Id");
			dtItemCategory.Columns.Add("Name");
			dtItemCategory.Clear();
			DataTable dtItemtype = new DataTable();
			dtItemtype.Columns.Add("Id");
			dtItemtype.Columns.Add("Name");
			dtItemtype.Clear();
			DataTable dtPackingType = new DataTable();
			dtPackingType.Columns.Add("Id");
			dtPackingType.Columns.Add("Name");
			dtPackingType.Clear();
			DataTable dtJobLot = new DataTable();
			dtJobLot.Columns.Add("Id");
			dtJobLot.Columns.Add("Name");
			dtJobLot.Clear();
			DataTable dtwarehouse = new DataTable();
			dtwarehouse.Columns.Add("Id");
			dtwarehouse.Columns.Add("Name");
			dtwarehouse.Clear();
			DataTable dtItemName = new DataTable();
			dtItemName.Columns.Add("Id");
			dtItemName.Columns.Add("Name");
			dtItemName.Clear();
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id");
			dtSupp.Columns.Add("Name");
			dtSupp.Clear();
			DataTable dtComm = new DataTable();
			dtComm.Columns.Add("Id");
			dtComm.Columns.Add("Name");
			dtComm.Clear();
			for (int i = 0; i < dtCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "ItemClassGroup")
				{
					dtItemClass.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "ItemParentCategory")
				{
					dtParentCategory.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "ItemCategory")
				{
					dtItemCategory.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "ItemType")
				{
					dtItemtype.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "PackingType")
				{
					dtPackingType.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "JobLot")
				{
					dtJobLot.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "WareHouse")
				{
					dtwarehouse.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "ItemName")
				{
					dtItemName.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "Supplier")
				{
					dtSupp.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["Activity"]) == "CommissionAgent")
				{
					dtComm.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtItemClass, cmbItemClass, "Id", "Name", "Item Class", ZeroIndex: false);
			DDL.BindDDL(dtParentCategory, cmbparentcategoryCombo, "Id", "Name", "Parent Category", ZeroIndex: false);
			DDL.BindDDL(dtItemCategory, cmbItemCategory, "Id", "Name", "Item Category", ZeroIndex: false);
			DDL.BindDDL(dtItemtype, cmbItemtype, "Id", "Name", "Item Type", ZeroIndex: false);
			DDL.BindDDL(dtPackingType, cmbPackingtype, "Id", "Name", "Packing Type", ZeroIndex: false);
			DDL.BindDDL(dtJobLot, cmbjoblot, "Id", "Name", "Job Lot", ZeroIndex: false);
			DDL.BindDDL(dtwarehouse, cmbwarehouse, "Id", "Name", "Warehouse Name", ZeroIndex: false);
			DDL.BindDDL(dtItemName, cmbItemName, "Id", "Name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtSupp, cmbsupplier, "Id", "Name", "Party Name", ZeroIndex: false);
			DDL.BindDDL(dtComm, cmbcommagent, "Id", "Name", "Commission Agent", ZeroIndex: false);
			((UltraGridBase)cmbItemClass).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbparentcategoryCombo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbPackingtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbjoblot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbwarehouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbsupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbcommagent).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllcomboFillFromEvaluation()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dtCombo = StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "1804"
			});
			if (dtCombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtItemClass = new DataTable();
			dtItemClass.Columns.Add("Id");
			dtItemClass.Columns.Add("Name");
			dtItemClass.Clear();
			DataTable dtParentCategory = new DataTable();
			dtParentCategory.Columns.Add("Id");
			dtParentCategory.Columns.Add("Name");
			dtParentCategory.Clear();
			DataTable dtItemCategory = new DataTable();
			dtItemCategory.Columns.Add("Id");
			dtItemCategory.Columns.Add("Name");
			dtItemCategory.Clear();
			DataTable dtItemtype = new DataTable();
			dtItemtype.Columns.Add("Id");
			dtItemtype.Columns.Add("Name");
			dtItemtype.Clear();
			DataTable dtPackingType = new DataTable();
			dtPackingType.Columns.Add("Id");
			dtPackingType.Columns.Add("Name");
			dtPackingType.Clear();
			DataTable dtJobLot = new DataTable();
			dtJobLot.Columns.Add("Id");
			dtJobLot.Columns.Add("Name");
			dtJobLot.Clear();
			DataTable dtwarehouse = new DataTable();
			dtwarehouse.Columns.Add("Id");
			dtwarehouse.Columns.Add("Name");
			dtwarehouse.Clear();
			DataTable dtItemName = new DataTable();
			dtItemName.Columns.Add("Id");
			dtItemName.Columns.Add("Name");
			dtItemName.Clear();
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id");
			dtSupp.Columns.Add("Name");
			dtSupp.Clear();
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Id");
			dataTable.Columns.Add("Name");
			dataTable.Clear();
			DataTable dtCity = new DataTable();
			dtCity.Columns.Add("Id");
			dtCity.Columns.Add("Name");
			dtCity.Clear();
			DataTable dtDistrict = new DataTable();
			dtDistrict.Columns.Add("Id");
			dtDistrict.Columns.Add("Name");
			dtDistrict.Clear();
			for (int i = 0; i < dtCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "ItemClassGroup")
				{
					dtItemClass.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "ParentCategories")
				{
					dtParentCategory.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "ItemCategories")
				{
					dtItemCategory.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "ItemTypes")
				{
					dtItemtype.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "PackingType")
				{
					dtPackingType.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "JobLot")
				{
					dtJobLot.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "Warehouse")
				{
					dtwarehouse.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "Items")
				{
					dtItemName.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "Supplier_Customer")
				{
					dtSupp.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "City")
				{
					dtCity.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtCombo.Rows[i]["ActivityType"]) == "District")
				{
					dtDistrict.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["name"]);
				}
			}
			DDL.BindDDL(dtItemClass, CmbItemClassSumm, "Id", "Name", "Item Class", ZeroIndex: false);
			DDL.BindDDL(dtParentCategory, cmbParentcategorySumm, "Id", "Name", "Parent Category", ZeroIndex: false);
			DDL.BindDDL(dtItemCategory, CmbCategorySumm, "Id", "Name", "Item Category", ZeroIndex: false);
			DDL.BindDDL(dtItemtype, cmbItemtypeSumm, "Id", "Name", "Item Type", ZeroIndex: false);
			DDL.BindDDL(dtPackingType, CmbPackingTypeSumm, "Id", "Name", "Packing Type", ZeroIndex: false);
			DDL.BindDDL(dtJobLot, cmbJobLotSumm, "Id", "Name", "Job Lot", ZeroIndex: false);
			DDL.BindDDL(dtwarehouse, CmbWareHouseSumm, "Id", "Name", "Warehouse Name", ZeroIndex: false);
			DDL.BindDDL(dtItemName, cmbItemNameSumm, "Id", "Name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtSupp, cmbSupplierSumm, "Id", "Name", "Party Name", ZeroIndex: false);
			DDL.BindDDL(dtCity, cmbcitySumm, "Id", "Name", "City", ZeroIndex: false);
			DDL.BindDDL(dtDistrict, cmbDistrictSumm, "Id", "Name", "Distict", ZeroIndex: false);
			((UltraGridBase)CmbItemClassSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbParentcategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbCategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemtypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbPackingTypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbJobLotSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbWareHouseSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemNameSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbcitySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbDistrictSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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

	private void ReportTypeSummaryFill()
	{
		try
		{
			DataTable dtreporttype = new DataTable();
			dtreporttype.Columns.Add("Id");
			dtreporttype.Columns.Add("name");
			dtreporttype.Rows.Add("Purchase Register", "Purchase Register");
			dtreporttype.Rows.Add("Purchase Summary By Item", "Purchase Summary By Item");
			dtreporttype.Rows.Add("Purchase Summary By Item & Warehouse", "Purchase Summary By Item & Warehouse");
			dtreporttype.Rows.Add("Purchase Summary By Supplier", "Purchase Summary By Supplier");
			dtreporttype.Rows.Add("Purchase Summary By Item & Supplier", "Purchase Summary By Item & Supplier");
			dtreporttype.Rows.Add("Purchase Summary By Parent Category", "Purchase Summary By Parent Category");
			dtreporttype.Rows.Add("Purchase Summary By Parent Category & Supplier", "Purchase Summary By Parent Category & Supplier");
			DDL.BindDDLNew(dtreporttype, cmbReportTypeSummary, "Id", "name", "Report Type", false);
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

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		try
		{
			DataTable dtGrid = InvPurchaseInvoice.PurchaseInvoice_ReportSalt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Conversion.ToDateTime((object)txtDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				BillNoFrom = Conversion.ToInt((object)txtFromDocNo.Text.Trim()),
				BillNoTo = Conversion.ToInt((object)txtToDocNo.Text.Trim()),
				ItemClassId = Conversion.ToInt(cmbItemClass.Value),
				InventoryParentCategories = Conversion.ToInt(cmbparentcategoryCombo.Value),
				ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value),
				ItemTypeId = Conversion.ToInt(cmbItemtype.Value),
				PackingTypeId = Conversion.ToInt(cmbPackingtype.Value),
				JobLotId = Conversion.ToInt(cmbjoblot.Value),
				WarehouseId = Conversion.ToInt(cmbwarehouse.Value),
				ItemId = Conversion.ToInt(cmbItemName.Value),
				SupplierCustomerId = Conversion.ToInt(cmbsupplier.Value),
				CommissionAgentId = Conversion.ToInt(cmbcommagent.Value),
				ReportType = Conversion.ToString((object)((Control)(object)cmbActivity).Text.Trim()),
				InventoryParentCategories = Conversion.ToInt(cmbparentcategoryCombo.Value)
			});
			if (dtGrid.Rows.Count > 0)
			{
				if (((UltraGridBase)cmbActivity).ActiveRow != null)
				{
					if (((Control)(object)cmbActivity).Text == "Detail")
					{
						dtDetail.Rows.Clear();
						for (int k = 0; k < dtGrid.Rows.Count; k++)
						{
							dtDetail.Rows.Add(dtGrid.Rows[k]["Id"], dtGrid.Rows[k]["DocumentTypeId"], dtGrid.Rows[k]["TransactionType"], Conversion.ToDateTime(dtGrid.Rows[k]["BillDate"]).ToShortDateString(), dtGrid.Rows[k]["BillNo"], dtGrid.Rows[k]["ManualNo"], dtGrid.Rows[k]["SupplierName"], dtGrid.Rows[k]["ParentCategory"], dtGrid.Rows[k]["ItemName"], dtGrid.Rows[k]["UOMCode"], dtGrid.Rows[k]["JobLotCode"], dtGrid.Rows[k]["PackTypeCode"], dtGrid.Rows[k]["ItemQty"], dtGrid.Rows[k]["BillWeight"], dtGrid.Rows[k]["StockWeight"], dtGrid.Rows[k]["ItemRate"], dtGrid.Rows[k]["BillAmount"], dtGrid.Rows[k]["Commission"], dtGrid.Rows[k]["Freight"], dtGrid.Rows[k]["OExpense"], dtGrid.Rows[k]["WagesAmount"], dtGrid.Rows[k]["TotalExpense"], dtGrid.Rows[k]["TotalAmount"], dtGrid.Rows[k]["GpNo"], Conversion.ToDateTime(dtGrid.Rows[k]["GpDate"]).ToShortDateString(), dtGrid.Rows[k]["VehicleNo"]);
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
							dtComparisonByItem.Rows.Add(dtGrid.Rows[j]["ParentCategory"], dtGrid.Rows[j]["ItemName"], dtGrid.Rows[j]["ItemQty"], dtGrid.Rows[j]["BillWeight"], dtGrid.Rows[j]["StockWeight"], dtGrid.Rows[j]["ItemRate"], dtGrid.Rows[j]["BillAmount"], dtGrid.Rows[j]["TotalExpense"], dtGrid.Rows[j]["TotalAmount"], dtGrid.Rows[j]["RateWithExp"], dtGrid.Rows[j]["TotalExp40Kg"], dtGrid.Rows[j]["Freight40Kg"], dtGrid.Rows[j]["Comm40Kg"], dtGrid.Rows[j]["OExpense40Kg"], dtGrid.Rows[j]["Wages40Kg"], dtGrid.Rows[j]["Freight"], dtGrid.Rows[j]["Commission"], dtGrid.Rows[j]["OExpense"], dtGrid.Rows[j]["WagesAmount"]);
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
							dtComparisonByItemSupplier.Rows.Add(dtGrid.Rows[i]["ParentCategory"], dtGrid.Rows[i]["SupplierName"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["ItemQty"], dtGrid.Rows[i]["BillWeight"], dtGrid.Rows[i]["StockWeight"], dtGrid.Rows[i]["ItemRate"], dtGrid.Rows[i]["BillAmount"], dtGrid.Rows[i]["TotalExpense"], dtGrid.Rows[i]["TotalAmount"], dtGrid.Rows[i]["RateWithExp"], dtGrid.Rows[i]["TotalExp40Kg"], dtGrid.Rows[i]["Freight40Kg"], dtGrid.Rows[i]["Comm40Kg"], dtGrid.Rows[i]["OExpense40Kg"], dtGrid.Rows[i]["Wages40Kg"], dtGrid.Rows[i]["Freight"], dtGrid.Rows[i]["Commission"], dtGrid.Rows[i]["OExpense"], dtGrid.Rows[i]["WagesAmount"]);
						}
						DataGridHistory.DataSource = dtComparisonByItemSupplier;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
				}
				else
				{
					MessageBox.Show("Report Type feild required");
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
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			DataGridHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["BillAmount"].Caption = "Item Amount";
			DataGridHistory.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["TotalExpense"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalExpense"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["TotalExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["TotalExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Wages"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Wages"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Wages"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Wages"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Amount+Exp"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Amount+Exp"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Amount+Exp"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			if (((Control)(object)cmbActivity).Text == "Detail")
			{
				DataGridHistory.RootTable.Columns["Id"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			}
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (DataGridHistory.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, Conversion.ToString((object)((Control)(object)DataGridHistory).Name));
			if (fs != null)
			{
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
			if (btnPrint.Text == "1804_01-PurchaseRegisterSummary")
			{
				if (dtDetail.Rows.Count == 0 || dtDetail == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtDetail, "1804_01-PurchaseRegisterSummary.rpt");
			}
			if (btnPrint.Text == "1804_02-PurchaseRegisterSummaryByItem&Customer")
			{
				if (dtComparisonByItemSupplier.Rows.Count == 0 || dtComparisonByItemSupplier == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtComparisonByItemSupplier, "1804_02-PurchaseRegisterSummaryByItem&Customer.rpt");
			}
			if (btnPrint.Text == "1804_03-PurchaseRegisterSummaryByItem")
			{
				if (dtComparisonByItem.Rows.Count == 0 || dtComparisonByItem == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtComparisonByItem, "1804_03-PurchaseRegisterSummaryByItem.rpt");
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
					btnPrint.Text = "1804_01-PurchaseRegisterSummary";
				}
				else if (((Control)(object)cmbActivity).Text == "Comparison By Item and Supplier")
				{
					btnPrint.Text = "1804_02-PurchaseRegisterSummaryByItem&Customer";
				}
				else if (((Control)(object)cmbActivity).Text == "Comparison By Item")
				{
					btnPrint.Text = "1804_03-PurchaseRegisterSummaryByItem";
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

	private void PurchaseRegisterNew_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			AllcomboFillFromPurchaseInvoice();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtToDocNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshSummary_Click(object sender, EventArgs e)
	{
		try
		{
			AllcomboFillFromEvaluation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewSummary_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbSupplierSumm).Text = string.Empty;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((Control)(object)cmbDistrictSumm).Text = string.Empty;
			((Control)(object)CmbItemClassSumm).Text = string.Empty;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			txtFromDocNoSumm.Clear();
			txtToDocNoSumm.Clear();
			((UltraGridBase)cmbReportTypeSummary).Rows[0].Activate();
			BtnShowSumm_Click(null, null);
			txtFromDateSumm.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowSumm_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbReportTypeSummary).ActiveRow != null)
			{
				GridSummaryFill();
				PrintButtonManageSummary();
			}
			else
			{
				MessageBox.Show("Please Select Activity First...");
				cmbReportTypeSummary.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			txtFromDateSumm.Focus();
		}
	}

	private void GridSummaryFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtGridSummary.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtFromDateSumm.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDateSumm.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoSumm.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoSumm.Text);
			obj.ItemClassGroupId = Conversion.ToInt(CmbItemClassSumm.Value);
			obj.ParentCategoryId = Conversion.ToInt(cmbParentcategorySumm.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbCategorySumm.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtypeSumm.Value);
			obj.ItemId = Conversion.ToInt(cmbItemNameSumm.Value);
			obj.JobLotId = Conversion.ToInt(cmbJobLotSumm.Value);
			obj.WarehouseId = Conversion.ToInt(CmbWareHouseSumm.Value);
			obj.PackingTypeId = Conversion.ToInt(CmbPackingTypeSumm.Value);
			obj.DistrictId = Conversion.ToInt(cmbDistrictSumm.Value);
			obj.CityId = Conversion.ToInt(cmbcitySumm.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierSumm.Value);
			if (chkCity.Checked)
			{
				obj.AppId = 1;
			}
			if (chkPackUom.Checked)
			{
				obj.PackUomTo = 1.0;
			}
			obj.ReportType = ((Control)(object)cmbReportTypeSummary).Text;
			dtGridSummary = InvPurchaseInvoice.PurchaseFromEvaulationsWithActivitiesReportSalt(obj);
			if (dtGridSummary.Rows.Count > 0)
			{
				if (((UltraGridBase)cmbReportTypeSummary).ActiveRow != null)
				{
					if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Register")
					{
						((Control)(object)GridSummary).Name = "OrderRegister";
						dtPurchaseRegister.Rows.Clear();
						foreach (DataRow row in dtGridSummary.Rows)
						{
							dtPurchaseRegister.Rows.Add(row["DocDate"], row["RefDocumentTypeId"], row["DocumentTypeCode"], row["DocCodeNo"], row["PartyName"], row["ParentCategory"], row["ItemCategory"], row["ItemType"], row["ItemCode"], row["ItemName"], row["UOMCode"], row["CropBatch"], row["JobLotCode"], row["PackTypeCode"], row["VehicleNo"], row["CityName"], row["Qty"], row["GrossWeight"], row["AddLess"], row["BillWeight"], row["StockWeight"], row["AvgRate"], row["Amount"], row["TotalExpenses"], row["TotalAmountWithExp"], row["AvgRateExp"], row["Freight"], row["Commission"], row["Expenses"], row["PrcntOfTotal"], row["PrcntOfTotalWeight"]);
						}
						GridSummary.DataSource = dtPurchaseRegister;
						GridSummary.RetrieveStructure();
						GridSummarySetting();
					}
					else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item")
					{
						((Control)(object)GridSummary).Name = "dtPurchaseSummaryByItem";
						dtPurchaseSummaryByItem.Rows.Clear();
						foreach (DataRow row2 in dtGridSummary.Rows)
						{
							dtPurchaseSummaryByItem.Rows.Add(row2["CityName"], row2["ParentCategory"], row2["ItemCategory"], row2["ItemType"], row2["ItemCode"], row2["ItemName"], row2["UOMCode"], row2["Qty"], row2["GrossWeight"], row2["AddLess"], row2["BillWeight"], row2["StockWeight"], row2["AvgRate"], row2["Amount"], row2["TotalExpenses"], row2["TotalAmountWithExp"], row2["AvgRateExp"], row2["Freight"], row2["Commission"], row2["Expenses"], row2["PrcntOfTotal"], row2["PrcntOfTotalWeight"]);
						}
						GridSummary.DataSource = dtPurchaseSummaryByItem;
						GridSummary.RetrieveStructure();
						GridSummarySetting();
					}
					else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Supplier")
					{
						((Control)(object)GridSummary).Name = "dtPurchaseSummaryBySupplier";
						dtPurchaseSummaryBySupplier.Rows.Clear();
						foreach (DataRow row3 in dtGridSummary.Rows)
						{
							dtPurchaseSummaryBySupplier.Rows.Add(row3["CityName"], row3["PartyName"], row3["UOMCode"], row3["Qty"], row3["GrossWeight"], row3["AddLess"], row3["BillWeight"], row3["StockWeight"], row3["AvgRate"], row3["Amount"], row3["TotalExpenses"], row3["TotalAmountWithExp"], row3["AvgRateExp"], row3["Freight"], row3["Commission"], row3["Expenses"], row3["PrcntOfTotal"], row3["PrcntOfTotalWeight"]);
						}
						GridSummary.DataSource = dtPurchaseSummaryBySupplier;
						GridSummary.RetrieveStructure();
						GridSummarySetting();
					}
					else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item & Supplier")
					{
						((Control)(object)GridSummary).Name = "dtPurchaseSummaryByItemAndSupplier";
						dtPurchaseSummaryByItemAndSupplier.Rows.Clear();
						foreach (DataRow row4 in dtGridSummary.Rows)
						{
							dtPurchaseSummaryByItemAndSupplier.Rows.Add(row4["CityName"], row4["PartyName"], row4["ParentCategory"], row4["ItemCategory"], row4["ItemType"], row4["ItemCode"], row4["ItemName"], row4["UOMCode"], row4["Qty"], row4["GrossWeight"], row4["AddLess"], row4["BillWeight"], row4["StockWeight"], row4["AvgRate"], row4["Amount"], row4["TotalExpenses"], row4["TotalAmountWithExp"], row4["AvgRateExp"], row4["Freight"], row4["Commission"], row4["Expenses"], row4["PrcntOfTotal"], row4["PrcntOfTotalWeight"]);
						}
						GridSummary.DataSource = dtPurchaseSummaryByItemAndSupplier;
						GridSummary.RetrieveStructure();
						GridSummarySetting();
					}
					else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item & Warehouse")
					{
						((Control)(object)GridSummary).Name = "dtPurchaseSummaryByItemAndWarehouse";
						dtPurchaseSummaryByItemAndWarehouse.Rows.Clear();
						foreach (DataRow row6 in dtGridSummary.Rows)
						{
							dtPurchaseSummaryByItemAndWarehouse.Rows.Add(row6["CityName"], row6["ParentCategory"], row6["ItemCategory"], row6["ItemType"], row6["ItemCode"], row6["ItemName"], row6["WarehouseName"], row6["UOMCode"], row6["Qty"], row6["GrossWeight"], row6["AddLess"], row6["BillWeight"], row6["StockWeight"], row6["AvgRate"], row6["Amount"], row6["TotalExpenses"], row6["TotalAmountWithExp"], row6["AvgRateExp"], row6["Freight"], row6["Commission"], row6["Expenses"], row6["PrcntOfTotal"], row6["PrcntOfTotalWeight"]);
						}
						GridSummary.DataSource = dtPurchaseSummaryByItemAndWarehouse;
						GridSummary.RetrieveStructure();
						GridSummarySetting();
					}
					else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Parent Category")
					{
						((Control)(object)GridSummary).Name = "dtPurchaseSummaryByParentCategory";
						dtPurchaseSummaryByParentCategory.Rows.Clear();
						foreach (DataRow row7 in dtGridSummary.Rows)
						{
							dtPurchaseSummaryByParentCategory.Rows.Add(row7["CityName"], row7["ParentCategory"], row7["UOMCode"], row7["Qty"], row7["GrossWeight"], row7["AddLess"], row7["BillWeight"], row7["StockWeight"], row7["AvgRate"], row7["Amount"], row7["TotalExpenses"], row7["TotalAmountWithExp"], row7["AvgRateExp"], row7["Freight"], row7["Commission"], row7["Expenses"], row7["PrcntOfTotal"], row7["PrcntOfTotalWeight"]);
						}
						GridSummary.DataSource = dtPurchaseSummaryByParentCategory;
						GridSummary.RetrieveStructure();
						GridSummarySetting();
					}
					else
					{
						if (!(((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Parent Category & Supplier"))
						{
							return;
						}
						((Control)(object)GridSummary).Name = "dtPurchaseSummaryByParentCategoryAndSupplier";
						dtPurchaseSummaryByParentCategoryAndSupplier.Rows.Clear();
						foreach (DataRow row5 in dtGridSummary.Rows)
						{
							dtPurchaseSummaryByParentCategoryAndSupplier.Rows.Add(row5["CityName"], row5["PartyName"], row5["ParentCategory"], row5["UOMCode"], row5["Qty"], row5["GrossWeight"], row5["AddLess"], row5["BillWeight"], row5["StockWeight"], row5["AvgRate"], row5["Amount"], row5["TotalExpenses"], row5["TotalAmountWithExp"], row5["AvgRateExp"], row5["Freight"], row5["Commission"], row5["Expenses"], row5["PrcntOfTotal"], row5["PrcntOfTotalWeight"]);
						}
						GridSummary.DataSource = dtPurchaseSummaryByParentCategoryAndSupplier;
						GridSummary.RetrieveStructure();
						GridSummarySetting();
					}
				}
				else
				{
					GridSummary.ClearStructure();
				}
			}
			else
			{
				GridSummary.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummarySetting()
	{
		try
		{
			GridSummary.AllowDelete = (InheritableBoolean)2;
			GridSummary.AllowEdit = (InheritableBoolean)2;
			if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Register")
			{
				GridSummary.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			}
			GridSummary.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["Qty"].FormatString = "##,#.###";
			GridSummary.RootTable.Columns["Qty"].TotalFormatString = "##,#.###";
			GridSummary.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["GrossWeight"].FormatString = "##,#.###";
			GridSummary.RootTable.Columns["GrossWeight"].TotalFormatString = "##,#.###";
			GridSummary.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["AddLess"].FormatString = "##,#.###";
			GridSummary.RootTable.Columns["AddLess"].TotalFormatString = "##,#.###";
			GridSummary.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["BillWeight"].FormatString = "##,#.###";
			GridSummary.RootTable.Columns["BillWeight"].TotalFormatString = "##,#.###";
			GridSummary.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["StockWeight"].FormatString = "##,#.###";
			GridSummary.RootTable.Columns["StockWeight"].TotalFormatString = "##,#.###";
			GridSummary.RootTable.Columns["AvgRate"].AggregateFunction = (AggregateFunction)3;
			GridSummary.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["AvgRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridSummary.RootTable.Columns["AvgRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			GridSummary.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["TotalExpenses"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["TotalExpenses"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["TotalExpenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["TotalExpenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["TotalAmountWithExp"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["TotalAmountWithExp"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["TotalAmountWithExp"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["TotalAmountWithExp"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["AvgRateExp"].AggregateFunction = (AggregateFunction)3;
			GridSummary.RootTable.Columns["AvgRateExp"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["AvgRateExp"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridSummary.RootTable.Columns["AvgRateExp"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			GridSummary.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["Expenses"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["Expenses"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["Expenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["Expenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["PrcntOfTotal"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["PrcntOfTotal"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["PrcntOfTotal"].FormatString = "#,#0.###";
			GridSummary.RootTable.Columns["PrcntOfTotal"].TotalFormatString = "#,#0.###";
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].FormatString = "#,#0.###";
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].TotalFormatString = "#,#0.###";
			CommonServices.GridAutoAdjustmentNew(GridSummary);
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummary_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridSummary.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				_ = r.Cells.Count;
				_ = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummary_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridSummary.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				_ = r.Cells.Count;
				_ = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintSumm_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrintSumm.Text == "1804_01_PurchaseRegister")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGridSummary, "1804_01_PurchaseRegister.rpt");
			}
			else if (btnPrintSumm.Text == "1804_02_PurchaseSummaryByItem")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtGridSummary, "1804_02_PurchaseSummaryByItem.rpt");
			}
			else if (btnPrintSumm.Text == "1804_03_PurchaseSummaryByItemAndWarehouse")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtGridSummary, "1804_03_PurchaseSummaryByItemAndWarehouse.rpt");
			}
			else if (btnPrintSumm.Text == "1804_04_PurchaseSummaryBySupplier")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtGridSummary, "1804_04_PurchaseSummaryBySupplier.rpt");
			}
			else if (btnPrintSumm.Text == "1804_05_PurchaseSummaryByItemAndSupplier")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtGridSummary, "1804_05_PurchaseSummaryByItemAndSupplier.rpt");
			}
			else if (btnPrintSumm.Text == "1804_06_PurchaseSummaryByParentCategory")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtGridSummary, "1804_06_PurchaseSummaryByParentCategory.rpt");
			}
			else if (btnPrintSumm.Text == "1804_07_PurchaseSummaryByParentCategoryAndSupplier")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtGridSummary, "1804_07_PurchaseSummaryByParentCategoryAndSupplier.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintButtonManageSummary()
	{
		try
		{
			if (((UltraGridBase)cmbReportTypeSummary).ActiveRow != null)
			{
				btnPrintSumm.Visible = true;
				if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Register")
				{
					btnPrintSumm.Text = "1804_01_PurchaseRegister";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item")
				{
					btnPrintSumm.Text = "1804_02_PurchaseSummaryByItem";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item & Warehouse")
				{
					btnPrintSumm.Text = "1804_03_PurchaseSummaryByItemAndWarehouse";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Supplier")
				{
					btnPrintSumm.Text = "1804_04_PurchaseSummaryBySupplier";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item & Supplier")
				{
					btnPrintSumm.Text = "1804_05_PurchaseSummaryByItemAndSupplier";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Parent Category")
				{
					btnPrintSumm.Text = "1804_06_PurchaseSummaryByParentCategory";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Parent Category & Supplier")
				{
					btnPrintSumm.Text = "1804_07_PurchaseSummaryByParentCategoryAndSupplier";
				}
			}
			else
			{
				btnPrintSumm.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, Conversion.ToString((object)((Control)(object)GridSummary).Name));
		if (fs != null)
		{
			GridSummary.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void CmbDateTypeSumm_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(CmbDateTypeSumm.Value) == 1)
			{
				txtFromDateSumm.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeSumm.Value) == 2)
			{
				txtFromDateSumm.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(CmbDateTypeSumm.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDateSumm.Value = first;
				txtToDateSumm.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeSumm.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromDateSumm.Value = first2;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeSumm.Value) == 5)
			{
				txtFromDateSumm.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutKeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseRegisterNew_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnShow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrint_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnPrint_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDateFrom.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					txtDateFrom.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNewSummary_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshSummary_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				BtnShowSumm_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrintSumm_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				txtFromDateSumm.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				txtFromDateSumm.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)GridSummary).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutKeysSumm_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoSumm_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtToDocNoSumm_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
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
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Expected O, but got Unknown
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Expected O, but got Unknown
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Expected O, but got Unknown
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Expected O, but got Unknown
		//IL_0969: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Expected O, but got Unknown
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Expected O, but got Unknown
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_099f: Expected O, but got Unknown
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09aa: Expected O, but got Unknown
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cb: Expected O, but got Unknown
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Expected O, but got Unknown
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Expected O, but got Unknown
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Expected O, but got Unknown
		//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a18: Expected O, but got Unknown
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a86: Expected O, but got Unknown
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Expected O, but got Unknown
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Expected O, but got Unknown
		//IL_0aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Expected O, but got Unknown
		//IL_0b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0a: Expected O, but got Unknown
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Expected O, but got Unknown
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b36: Expected O, but got Unknown
		//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4c: Expected O, but got Unknown
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6d: Expected O, but got Unknown
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b78: Expected O, but got Unknown
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b99: Expected O, but got Unknown
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Expected O, but got Unknown
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Expected O, but got Unknown
		//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Expected O, but got Unknown
		//IL_1117: Unknown result type (might be due to invalid IL or missing references)
		//IL_1121: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		GridEXLayout GridSummary_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance12 = new Appearance();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Detail = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtToDocNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.txtFromDocNo = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbcommagent = new UltraCombo();
		this.cmbActivity = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbsupplier = new UltraCombo();
		this.cmbparentcategoryCombo = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbjoblot = new UltraCombo();
		this.cmbItemtype = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbwarehouse = new UltraCombo();
		this.cmbItemCategory = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbItemName = new UltraCombo();
		this.cmbItemClass = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbPackingtype = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnReferesh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.Summary = new System.Windows.Forms.TabPage();
		this.GridSummary = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.chkPackUom = new System.Windows.Forms.CheckBox();
		this.chkCity = new System.Windows.Forms.CheckBox();
		this.CmbDateTypeSumm = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.CmbItemClassSumm = new UltraCombo();
		this.cmbItemtypeSumm = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbPackingTypeSumm = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbSupplierSumm = new UltraCombo();
		this.cmbDistrictSumm = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbcitySumm = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.cmbReportTypeSummary = new UltraCombo();
		this.cmbItemNameSumm = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbWareHouseSumm = new UltraCombo();
		this.cmbJobLotSumm = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.txtFromDateSumm = new System.Windows.Forms.DateTimePicker();
		this.label32 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtToDateSumm = new System.Windows.Forms.DateTimePicker();
		this.txtFromDocNoSumm = new System.Windows.Forms.TextBox();
		this.txtToDocNoSumm = new System.Windows.Forms.TextBox();
		this.BtnShowSumm = new System.Windows.Forms.Button();
		this.label24 = new System.Windows.Forms.Label();
		this.cmbParentcategorySumm = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbCategorySumm = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewSummary = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSumm = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutKeysSumm = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Detail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcategoryCombo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbwarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingtype).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.Summary.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridSummary).BeginInit();
		this.panel9.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemClassSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportTypeSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1144, 618);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.Detail);
		this.tabControl1.Controls.Add(this.Summary);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1144, 618);
		this.tabControl1.TabIndex = 3;
		this.Detail.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.Detail.Controls.Add(this.panel2);
		this.Detail.Controls.Add(this.panel4);
		this.Detail.Controls.Add(this.panel3);
		this.Detail.Location = new System.Drawing.Point(4, 25);
		this.Detail.Name = "Detail";
		this.Detail.Padding = new System.Windows.Forms.Padding(3);
		this.Detail.Size = new System.Drawing.Size(1136, 589);
		this.Detail.TabIndex = 0;
		this.Detail.Text = "Detail Register";
		this.Detail.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.AlternatingColors = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 204);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1130, 382);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 182);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1130, 22);
		this.panel2.TabIndex = 0;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 2);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(112, 16);
		this.label18.TabIndex = 189;
		this.label18.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1089, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 21);
		this.ctrlGrdBar1.TabIndex = 9;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.txtDateFrom);
		this.panel4.Controls.Add(this.txtToDocNo);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.txtToDate);
		this.panel4.Controls.Add(this.txtFromDocNo);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivity);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.btnShow);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplier);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbjoblot);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtype);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbwarehouse);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemClass);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingtype);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 56);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1130, 126);
		this.panel4.TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 99);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(64, 15);
		this.label15.TabIndex = 189;
		this.label15.Text = "Doc No To";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateFrom.Location = new System.Drawing.Point(84, 8);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(130, 23);
		this.txtDateFrom.TabIndex = 0;
		this.txtToDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNo.Location = new System.Drawing.Point(84, 95);
		this.txtToDocNo.Name = "txtToDocNo";
		this.txtToDocNo.Size = new System.Drawing.Size(130, 23);
		this.txtToDocNo.TabIndex = 3;
		this.txtToDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNo_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "From Date";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(5, 70);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(79, 15);
		this.label16.TabIndex = 188;
		this.label16.Text = "Doc No From";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(84, 37);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(130, 23);
		this.txtToDate.TabIndex = 1;
		this.txtFromDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNo.Location = new System.Drawing.Point(84, 66);
		this.txtFromDocNo.Name = "txtFromDocNo";
		this.txtFromDocNo.Size = new System.Drawing.Size(130, 23);
		this.txtFromDocNo.TabIndex = 2;
		this.txtFromDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNo_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 41);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 3;
		this.label2.Text = "To Date";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(804, 41);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(78, 15);
		this.label14.TabIndex = 185;
		this.label14.Text = "Comm Agent";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(804, 70);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(70, 15);
		this.label12.TabIndex = 163;
		this.label12.Text = "Report Type";
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(882, 35);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 13;
		this.cmbActivity.AllowNull = (DefaultableBoolean)1;
		this.cmbActivity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbActivity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbActivity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Location = new System.Drawing.Point(882, 64);
		((System.Windows.Forms.Control)(object)this.cmbActivity).Name = "cmbActivity";
		((System.Windows.Forms.Control)(object)this.cmbActivity).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivity).TabIndex = 14;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(804, 12);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(69, 15);
		this.label13.TabIndex = 183;
		this.label13.Text = "Party Name";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(1068, 93);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(57, 26);
		this.btnShow.TabIndex = 15;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbsupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Location = new System.Drawing.Point(882, 6);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Name = "cmbsupplier";
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).TabIndex = 12;
		this.cmbparentcategoryCombo.AllowNull = (DefaultableBoolean)1;
		this.cmbparentcategoryCombo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparentcategoryCombo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparentcategoryCombo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance163;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance168).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbparentcategoryCombo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).Location = new System.Drawing.Point(309, 35);
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).Name = "cmbparentcategoryCombo";
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(516, 41);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(49, 15);
		this.label11.TabIndex = 181;
		this.label11.Text = "Job Lot ";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(218, 41);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(90, 15);
		this.label3.TabIndex = 165;
		this.label3.Text = "Parent category";
		this.cmbjoblot.AllowNull = (DefaultableBoolean)1;
		this.cmbjoblot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbjoblot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbjoblot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance182).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbjoblot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Location = new System.Drawing.Point(593, 35);
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Name = "cmbjoblot";
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbjoblot).TabIndex = 9;
		this.cmbItemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance196).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Location = new System.Drawing.Point(309, 93);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Name = "cmbItemtype";
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).TabIndex = 7;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(516, 70);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 15);
		this.label10.TabIndex = 179;
		this.label10.Text = "Warehouse";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(218, 99);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 15);
		this.label4.TabIndex = 167;
		this.label4.Text = "Item Type";
		this.cmbwarehouse.AllowNull = (DefaultableBoolean)1;
		this.cmbwarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbwarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbwarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbwarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).Location = new System.Drawing.Point(593, 64);
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).Name = "cmbwarehouse";
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).TabIndex = 10;
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance216;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance222).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance223).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(309, 64);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 6;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(516, 99);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 177;
		this.label9.Text = "Item Name";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(218, 70);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(82, 15);
		this.label5.TabIndex = 169;
		this.label5.Text = "Item Category";
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance230;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance231).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance231).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance235).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance236).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(593, 93);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 11;
		this.cmbItemClass.AllowNull = (DefaultableBoolean)1;
		this.cmbItemClass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemClass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemClass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance243;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance244).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance244).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance248).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemClass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Location = new System.Drawing.Point(309, 6);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Name = "cmbItemClass";
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(218, 12);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(61, 15);
		this.label6.TabIndex = 171;
		this.label6.Text = "Item Class";
		this.cmbPackingtype.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance254).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance254).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance255;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance256).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance256).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Location = new System.Drawing.Point(593, 6);
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Name = "cmbPackingtype";
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).TabIndex = 8;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(516, 12);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(77, 15);
		this.label7.TabIndex = 173;
		this.label7.Text = "Packing Type";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.toolStrip3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1130, 53);
		this.panel3.TabIndex = 0;
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(5, 31);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(221, 19);
		this.label17.TabIndex = 190;
		this.label17.Text = "Purchase Invoice Register";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.BtnReferesh, this.btnPrint, this.btnShortcutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1130, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnReferesh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnReferesh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnReferesh.Name = "BtnReferesh";
		this.BtnReferesh.Size = new System.Drawing.Size(80, 24);
		this.BtnReferesh.Text = "&Refresh";
		this.BtnReferesh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.Summary.Controls.Add((System.Windows.Forms.Control)(object)this.GridSummary);
		this.Summary.Controls.Add(this.panel9);
		this.Summary.Controls.Add(this.panel7);
		this.Summary.Controls.Add(this.panel8);
		this.Summary.Location = new System.Drawing.Point(4, 25);
		this.Summary.Name = "Summary";
		this.Summary.Padding = new System.Windows.Forms.Padding(3);
		this.Summary.Size = new System.Drawing.Size(1136, 589);
		this.Summary.TabIndex = 1;
		this.Summary.Text = "Summary Register";
		this.Summary.UseVisualStyleBackColor = true;
		this.GridSummary.AllowEdit = (InheritableBoolean)2;
		this.GridSummary.AlternatingColors = true;
		((ScrollableBase)this.GridSummary).BackColor = System.Drawing.Color.White;
		GridSummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridSummary.DesignTimeLayout = GridSummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridSummary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridSummary.FilterMode = (FilterMode)2;
		this.GridSummary.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridSummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridSummary.GroupTotals = (GroupTotals)2;
		this.GridSummary.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridSummary).Location = new System.Drawing.Point(3, 205);
		((System.Windows.Forms.Control)(object)this.GridSummary).Name = "GridSummary";
		((ScrollableBase)this.GridSummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridSummary).Size = new System.Drawing.Size(1130, 381);
		((System.Windows.Forms.Control)(object)this.GridSummary).TabIndex = 0;
		this.GridSummary.TotalRow = (InheritableBoolean)1;
		this.GridSummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridSummary).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 180);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1130, 25);
		this.panel9.TabIndex = 11;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1083, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.GridSummary;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 33);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1130, 147);
		this.panel7.TabIndex = 9;
		this.groupBox2.Controls.Add(this.chkPackUom);
		this.groupBox2.Controls.Add(this.chkCity);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemClassSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDistrictSumm);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcitySumm);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemNameSumm);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLotSumm);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.txtFromDateSumm);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.txtToDateSumm);
		this.groupBox2.Controls.Add(this.txtFromDocNoSumm);
		this.groupBox2.Controls.Add(this.txtToDocNoSumm);
		this.groupBox2.Controls.Add(this.BtnShowSumm);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategorySumm);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 4);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1120, 136);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Criteria's";
		this.chkPackUom.AutoSize = true;
		this.chkPackUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPackUom.Location = new System.Drawing.Point(944, 104);
		this.chkPackUom.Name = "chkPackUom";
		this.chkPackUom.Size = new System.Drawing.Size(80, 19);
		this.chkPackUom.TabIndex = 18;
		this.chkPackUom.Text = "Pack Uom";
		this.chkPackUom.UseVisualStyleBackColor = true;
		this.chkCity.AutoSize = true;
		this.chkCity.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCity.Location = new System.Drawing.Point(871, 104);
		this.chkCity.Name = "chkCity";
		this.chkCity.Size = new System.Drawing.Size(75, 19);
		this.chkCity.TabIndex = 17;
		this.chkCity.Text = "City Wise";
		this.chkCity.UseVisualStyleBackColor = true;
		this.CmbDateTypeSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbDateTypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateTypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateTypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateTypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Location = new System.Drawing.Point(93, 15);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Name = "CmbDateTypeSumm";
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).TabIndex = 0;
		this.CmbDateTypeSumm.ValueChanged += new System.EventHandler(CmbDateTypeSumm_ValueChanged);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(2, 21);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(60, 15);
		this.label40.TabIndex = 594;
		this.label40.Text = "Date Type";
		this.CmbItemClassSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbItemClassSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemClassSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemClassSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance172;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance265).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance265).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance265).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance267;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance271;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance272).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance278).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemClassSumm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemClassSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemClassSumm).Location = new System.Drawing.Point(368, 15);
		((System.Windows.Forms.Control)(object)this.CmbItemClassSumm).Name = "CmbItemClassSumm";
		((System.Windows.Forms.Control)(object)this.CmbItemClassSumm).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemClassSumm).TabIndex = 6;
		this.cmbItemtypeSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemtypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Location = new System.Drawing.Point(368, 73);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Name = "cmbItemtypeSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).TabIndex = 8;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label26.Location = new System.Drawing.Point(304, 21);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(61, 15);
		this.label26.TabIndex = 163;
		this.label26.Text = "Item Class";
		this.CmbPackingTypeSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingTypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingTypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingTypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance275;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance276).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance276).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance281).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance282).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance283).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingTypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).Location = new System.Drawing.Point(628, 73);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).Name = "CmbPackingTypeSumm";
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).TabIndex = 12;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(801, 50);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(51, 15);
		this.label19.TabIndex = 123;
		this.label19.Text = "Supplier";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(550, 79);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(77, 15);
		this.label20.TabIndex = 158;
		this.label20.Text = "Packing Type";
		this.cmbSupplierSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance122;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).Location = new System.Drawing.Point(871, 44);
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).Name = "cmbSupplierSumm";
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).TabIndex = 15;
		this.cmbDistrictSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbDistrictSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDistrictSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDistrictSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDistrictSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Location = new System.Drawing.Point(628, 102);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Name = "cmbDistrictSumm";
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).TabIndex = 13;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(801, 21);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(30, 15);
		this.label30.TabIndex = 149;
		this.label30.Text = "City ";
		this.cmbcitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbcitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Location = new System.Drawing.Point(871, 15);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Name = "cmbcitySumm";
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).TabIndex = 14;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(304, 108);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(35, 15);
		this.label21.TabIndex = 125;
		this.label21.Text = "Item ";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(550, 108);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(45, 15);
		this.label31.TabIndex = 151;
		this.label31.Text = "District";
		this.cmbReportTypeSummary.AllowNull = (DefaultableBoolean)1;
		this.cmbReportTypeSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportTypeSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportTypeSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance142).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReportTypeSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).Location = new System.Drawing.Point(871, 73);
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).Name = "cmbReportTypeSummary";
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).TabIndex = 16;
		this.cmbItemNameSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance109;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance115).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemNameSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Location = new System.Drawing.Point(368, 102);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Name = "cmbItemNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).TabIndex = 9;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(801, 79);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(70, 15);
		this.label22.TabIndex = 155;
		this.label22.Text = "Report Type";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(2, 50);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(91, 15);
		this.label29.TabIndex = 127;
		this.label29.Text = "From && To Date";
		this.CmbWareHouseSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance288;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouseSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm).Location = new System.Drawing.Point(628, 44);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm).Name = "CmbWareHouseSumm";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm).TabIndex = 11;
		this.cmbJobLotSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance173;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance228).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance239).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance266).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLotSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Location = new System.Drawing.Point(628, 15);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Name = "cmbJobLotSumm";
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).TabIndex = 10;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(550, 50);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(69, 15);
		this.label25.TabIndex = 153;
		this.label25.Text = "WareHouse";
		this.txtFromDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtFromDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateSumm.Location = new System.Drawing.Point(93, 46);
		this.txtFromDateSumm.Name = "txtFromDateSumm";
		this.txtFromDateSumm.Size = new System.Drawing.Size(103, 23);
		this.txtFromDateSumm.TabIndex = 1;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(550, 21);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(46, 15);
		this.label32.TabIndex = 153;
		this.label32.Text = "Job Lot";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(2, 79);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(82, 15);
		this.label23.TabIndex = 129;
		this.label23.Text = "From && To No";
		this.txtToDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtToDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateSumm.Location = new System.Drawing.Point(197, 46);
		this.txtToDateSumm.Name = "txtToDateSumm";
		this.txtToDateSumm.Size = new System.Drawing.Size(103, 23);
		this.txtToDateSumm.TabIndex = 2;
		this.txtFromDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoSumm.Location = new System.Drawing.Point(93, 75);
		this.txtFromDocNoSumm.Name = "txtFromDocNoSumm";
		this.txtFromDocNoSumm.Size = new System.Drawing.Size(103, 23);
		this.txtFromDocNoSumm.TabIndex = 3;
		this.txtFromDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoSumm_KeyPress);
		this.txtToDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoSumm.Location = new System.Drawing.Point(197, 75);
		this.txtToDocNoSumm.Name = "txtToDocNoSumm";
		this.txtToDocNoSumm.Size = new System.Drawing.Size(103, 23);
		this.txtToDocNoSumm.TabIndex = 4;
		this.txtToDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoSumm_KeyPress);
		this.BtnShowSumm.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowSumm.FlatAppearance.BorderSize = 0;
		this.BtnShowSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowSumm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowSumm.ForeColor = System.Drawing.Color.White;
		this.BtnShowSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowSumm.Location = new System.Drawing.Point(1057, 102);
		this.BtnShowSumm.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowSumm.Name = "BtnShowSumm";
		this.BtnShowSumm.Size = new System.Drawing.Size(55, 26);
		this.BtnShowSumm.TabIndex = 19;
		this.BtnShowSumm.Text = "Show";
		this.BtnShowSumm.UseVisualStyleBackColor = false;
		this.BtnShowSumm.Click += new System.EventHandler(BtnShowSumm_Click);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(2, 108);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(91, 15);
		this.label24.TabIndex = 138;
		this.label24.Text = "Parent Category";
		this.cmbParentcategorySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbParentcategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentcategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentcategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentcategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Location = new System.Drawing.Point(93, 102);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Name = "cmbParentcategorySumm";
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).TabIndex = 5;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(304, 50);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(54, 15);
		this.label27.TabIndex = 138;
		this.label27.Text = "Category";
		this.CmbCategorySumm.AllowNull = (DefaultableBoolean)1;
		this.CmbCategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Location = new System.Drawing.Point(368, 44);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Name = "CmbCategorySumm";
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).TabIndex = 7;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(304, 79);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(32, 15);
		this.label33.TabIndex = 138;
		this.label33.Text = "Type";
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1130, 30);
		this.panel8.TabIndex = 10;
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNewSummary, this.btnRefreshSummary, this.btnPrintSumm, this.BtnShortCutKeysSumm });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1130, 30);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewSummary.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewSummary.Name = "BtnNewSummary";
		this.BtnNewSummary.Size = new System.Drawing.Size(59, 27);
		this.BtnNewSummary.Text = "&New";
		this.BtnNewSummary.Click += new System.EventHandler(BtnNewSummary_Click);
		this.btnRefreshSummary.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummary.Name = "btnRefreshSummary";
		this.btnRefreshSummary.Size = new System.Drawing.Size(80, 27);
		this.btnRefreshSummary.Text = "&Refresh";
		this.btnRefreshSummary.Click += new System.EventHandler(btnRefreshSummary_Click);
		this.btnPrintSumm.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSumm.Name = "btnPrintSumm";
		this.btnPrintSumm.Size = new System.Drawing.Size(61, 27);
		this.btnPrintSumm.Text = "Print";
		this.btnPrintSumm.Click += new System.EventHandler(btnPrintSumm_Click);
		this.BtnShortCutKeysSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutKeysSumm.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutKeysSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutKeysSumm.Name = "BtnShortCutKeysSumm";
		this.BtnShortCutKeysSumm.Size = new System.Drawing.Size(126, 27);
		this.BtnShortCutKeysSumm.Text = "ShortCut Keys";
		this.BtnShortCutKeysSumm.Click += new System.EventHandler(BtnShortCutKeysSumm_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1144, 618);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PurchaseInvoiceSummaryRegister";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchaseRegisterNew_FormClosing);
		base.Load += new System.EventHandler(frmEvaulationDetailSalesReports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchaseRegisterNew_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Detail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcategoryCombo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbwarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingtype).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.Summary.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridSummary).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemClassSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportTypeSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
