// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
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
namespace Architecture.WinApp.Inventory_Reports
{

public class PurchaseRegisterNew : Form
{
	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable detailHistory = new DataTable();

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

	private DataTable dtPurchaseSummaryByHsCode = new DataTable();

	private DataTable dtBranch = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	public int SupplierId;

	public int StockAccountId;

	public int ItemId;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX DataGridHistory;

	private Panel panel3;

	private Button btnShow;

	internal UltraCombo cmbActivity;

	private Label label12;

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

	private Label label8;

	internal UltraCombo cmbCropYear;

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

	internal UltraCombo CmbCropYearSumm;

	internal UltraCombo CmbItemClassSumm;

	private Label label28;

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

	internal UltraCombo cmbBranchName;

	private Label label48;

	internal UltraCombo CmbBranchNameSumm;

	private Label label16;

	internal UltraCombo CmbDocumentType;

	private Label label2;

	private ToolStripDropDownButton btnPrint;

	internal UltraCombo CmbCustomGroupSummary;

	private Label label35;

	internal UltraCombo CmbCustomGroup;

	private Label label37;

	private Label label36;

	internal UltraCombo CmbStockAccount;

	private Label label38;

	internal UltraCombo CmbStockAccountSummary;

	private Label label34;

	public PurchaseRegisterNew(UserAccount userAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
			waitForm.Show(this);
			dtDetail.Columns.Add("ParentCategory", typeof(string));
			dtDetail.Columns.Add("TransactionType", typeof(string));
			dtDetail.Columns.Add("PurchaseType", typeof(string));
			dtDetail.Columns.Add("SupplierName", typeof(string));
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("DocumentTypeId", typeof(int));
			dtDetail.Columns.Add("PurchaseOrderId", typeof(int));
			dtDetail.Columns.Add("PoNo", typeof(int));
			dtDetail.Columns.Add("BillDate", typeof(DateTime));
			dtDetail.Columns.Add("BillNo", typeof(int));
			dtDetail.Columns.Add("BranchSrNo", typeof(int));
			dtDetail.Columns.Add("ManualNo", typeof(string));
			dtDetail.Columns.Add("WareHouseName", typeof(string));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("PackUom", typeof(string));
			dtDetail.Columns.Add("CropYear", typeof(string));
			dtDetail.Columns.Add("JobLot", typeof(string));
			dtDetail.Columns.Add("PackingType", typeof(string));
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("QtyByParentCategory", typeof(double));
			dtDetail.Columns.Add("BillWeight", typeof(double));
			dtDetail.Columns.Add("StockWeight", typeof(double));
			dtDetail.Columns.Add("ItemRate", typeof(double));
			dtDetail.Columns.Add("RateCut", typeof(double));
			dtDetail.Columns.Add("RateCutAmount", typeof(double));
			dtDetail.Columns.Add("BillAmount", typeof(double));
			dtDetail.Columns.Add("Commission", typeof(double));
			dtDetail.Columns.Add("Freight", typeof(double));
			dtDetail.Columns.Add("Expense", typeof(double));
			dtDetail.Columns.Add("Wages", typeof(double));
			dtDetail.Columns.Add("EmptyBagsAmount", typeof(double));
			dtDetail.Columns.Add("EmptyBagsAmount40KG", typeof(double));
			dtDetail.Columns.Add("FreightDeduction", typeof(double));
			dtDetail.Columns.Add("FreightDeduction40KG", typeof(double));
			dtDetail.Columns.Add("ItemAddLess(JV)", typeof(double));
			dtDetail.Columns.Add("TotalExpense", typeof(double));
			dtDetail.Columns.Add("Exp/40kg", typeof(double));
			dtDetail.Columns.Add("Amount+Exp", typeof(double));
			dtDetail.Columns.Add("PartyBillAmount", typeof(double));
			dtDetail.Columns.Add("PartyAddLess_JV", typeof(double));
			dtDetail.Columns.Add("RateWithExp", typeof(double));
			dtDetail.Columns.Add("StockAvgRateKg", typeof(double));
			dtDetail.Columns.Add("AvgRatePerKgOnBillWeight", typeof(double));
			dtDetail.Columns.Add("GpNo", typeof(int));
			dtDetail.Columns.Add("GpDate", typeof(DateTime));
			dtDetail.Columns.Add("GrnDate", typeof(DateTime));
			dtDetail.Columns.Add("GrnNo", typeof(int));
			dtDetail.Columns.Add("VehicleNo", typeof(string));
			dtDetail.Columns.Add("TicketNos", typeof(string));
			dtDetail.Columns.Add("CommissionAgent", typeof(string));
			dtDetail.Columns.Add("CommissionType", typeof(string));
			dtDetail.Columns.Add("CommRate", typeof(double));
			dtDetail.Columns.Add("BrokerName", typeof(string));
			dtDetail.Columns.Add("BrokeryType", typeof(string));
			dtDetail.Columns.Add("BrokeryRate", typeof(double));
			dtDetail.Columns.Add("CityName", typeof(string));
			dtComparisonByItem.Columns.Add("ParentCategory", typeof(string));
			dtComparisonByItem.Columns.Add("ItemName", typeof(string));
			dtComparisonByItem.Columns.Add("CropYear", typeof(string));
			dtComparisonByItem.Columns.Add("ItemQty", typeof(double));
			dtComparisonByItem.Columns.Add("QtyByParentCategory", typeof(double));
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
			dtComparisonByItem.Columns.Add("EmptyBagsAmount", typeof(double));
			dtComparisonByItem.Columns.Add("EmptyBagsAmount40KG", typeof(double));
			dtComparisonByItem.Columns.Add("FreightDeduction", typeof(double));
			dtComparisonByItem.Columns.Add("FreightDeduction40KG", typeof(double));
			dtComparisonByItem.Columns.Add("Freight", typeof(double));
			dtComparisonByItem.Columns.Add("Commission", typeof(double));
			dtComparisonByItem.Columns.Add("Expense", typeof(double));
			dtComparisonByItem.Columns.Add("Wages", typeof(double));
			dtComparisonByItem.Columns.Add("ItemAddLess(JV)", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("ParentCategory", typeof(string));
			dtComparisonByItemSupplier.Columns.Add("ItemName", typeof(string));
			dtComparisonByItemSupplier.Columns.Add("CropYear", typeof(string));
			dtComparisonByItemSupplier.Columns.Add("SupplierName", typeof(string));
			dtComparisonByItemSupplier.Columns.Add("ItemQty", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("QtyByParentCategory", typeof(double));
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
			dtComparisonByItemSupplier.Columns.Add("EmptyBagsAmount", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("EmptyBagsAmount40KG", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("FreightDeduction", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("FreightDeduction40KG", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Freight", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Commission", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Expense", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("Wages", typeof(double));
			dtComparisonByItemSupplier.Columns.Add("ItemAddLess(JV)", typeof(double));
			dtPurchaseRegister.Columns.Add("DocDate", typeof(DateTime));
			dtPurchaseRegister.Columns.Add("RefDocumentTypeId", typeof(int));
			dtPurchaseRegister.Columns.Add("DocumentType", typeof(string));
			dtPurchaseRegister.Columns.Add("DocCodeNo", typeof(int));
			dtPurchaseRegister.Columns.Add("PartyName", typeof(string));
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
			dtPurchaseRegister.Columns.Add("EbTotal", typeof(decimal));
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
			dtPurchaseRegister.Columns.Add("Wages", typeof(decimal));
			dtPurchaseRegister.Columns.Add("EmptyBagsAmount", typeof(decimal));
			dtPurchaseRegister.Columns.Add("FreightDeduction", typeof(decimal));
			dtPurchaseRegister.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseRegister.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseRegister.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseRegister.Columns.Add("ItemType", typeof(string));
			dtPurchaseRegister.Columns.Add("ItemCategory", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemCode", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("EbTotal", typeof(decimal));
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
			dtPurchaseSummaryByItem.Columns.Add("Wages", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("EmptyBagsAmount", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("FreightDeduction", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByItem.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemCategory", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemType", typeof(string));
			dtPurchaseSummaryBySupplier.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryBySupplier.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryBySupplier.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryBySupplier.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("EbTotal", typeof(decimal));
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
			dtPurchaseSummaryBySupplier.Columns.Add("Wages", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("EmptyBagsAmount", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("FreightDeduction", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryBySupplier.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ItemCode", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("EbTotal", typeof(decimal));
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
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("Wages", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("EmptyBagsAmount", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("FreightDeduction", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ItemCategory", typeof(string));
			dtPurchaseSummaryByItemAndSupplier.Columns.Add("ItemType", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ItemCode", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("WarehouseName", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("EbTotal", typeof(decimal));
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
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("Wages", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("EmptyBagsAmount", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("FreightDeduction", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ItemCategory", typeof(string));
			dtPurchaseSummaryByItemAndWarehouse.Columns.Add("ItemType", typeof(string));
			dtPurchaseSummaryByParentCategory.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByParentCategory.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByParentCategory.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByParentCategory.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("EbTotal", typeof(decimal));
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
			dtPurchaseSummaryByParentCategory.Columns.Add("Wages", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("EmptyBagsAmount", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("FreightDeduction", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByParentCategory.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("EbTotal", typeof(decimal));
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
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("Wages", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("EmptyBagsAmount", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("FreightDeduction", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByParentCategoryAndSupplier.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByHsCode.Columns.Add("HsCode", typeof(string));
			dtPurchaseSummaryByHsCode.Columns.Add("UOMCode", typeof(string));
			dtPurchaseSummaryByHsCode.Columns.Add("Qty", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("GrossWeight", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("EbTotal", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("AddLess", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("BillWeight", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("StockWeight", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("AvgRate", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("Amount", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("TotalExpenses", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("TotalAmountWithExp", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("AvgRateExp", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("Freight", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("Commission", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("Expenses", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("Wages", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("EmptyBagsAmount", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("FreightDeduction", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtPurchaseSummaryByHsCode.Columns.Add("PrcntOfTotalWeight", typeof(decimal));
			BranchesFill();
			AllcomboFillFromPurchaseInvoice();
			ActivityFill();
			AllcomboFillFromEvaluation();
			ReportTypeSummaryFill();
			CustomeGroupsFill();
			((UltraGridBase)cmbActivity).Rows[0].Activate();
			PrintButtonManage();
			ParameterFill();
			if (Conversion.CheckDateTimeNull(FromDate))
			{
				txtDateFrom.Value = DateTime.Now.AddDays(-15.0);
				return;
			}
			txtDateFrom.Value = FromDate;
			txtToDate.Value = ToDate;
			if (SupplierId > 0)
			{
				cmbsupplier.Value = SupplierId;
			}
			if (StockAccountId > 0)
			{
				CmbStockAccount.Value = StockAccountId;
			}
			if (ItemId > 0)
			{
				cmbItemName.Value = ItemId;
			}
			GridFill();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void BranchesFill()
	{
		try
		{
			dtBranch = InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbBranchNameSumm, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbBranchNameSumm).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbBranchNameSumm.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranchNameSumm.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranchNameSumm.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranchNameSumm.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranchNameSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)CmbBranchNameSumm).Text = UserAccount.BranchName.ToString();
				DDL.BindDDL(dtBranch, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj2 = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomeGroupsFill()
	{
		try
		{
			DataTable dtCustome = CommonServices.CustomeGroupsDefine(2);
			if (dtCustome.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCustome, CmbCustomGroup, "Id", "AcLookUpsDescription", "Custom Group", false);
				DDL.BindDDLNew(dtCustome, CmbCustomGroupSummary, "Id", "AcLookUpsDescription", "Custom Group", false);
				UltraGridColumn obj = ((UltraGridBase)CmbCustomGroupSummary).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbCustomGroupSummary.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbCustomGroupSummary.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbCustomGroupSummary.CheckedListSettings).ListSeparator = ",";
				CmbCustomGroupSummary.LimitToList = false;
				((EditorCheckedListSettings)CmbCustomGroupSummary.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbCustomGroupSummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbCustomGroupSummary).DisplayLayout.Bands[0].Columns[1].Width = 300;
				UltraGridColumn obj2 = ((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				CmbCustomGroup.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbCustomGroup.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbCustomGroup.CheckedListSettings).ListSeparator = ",";
				CmbCustomGroup.LimitToList = false;
				((EditorCheckedListSettings)CmbCustomGroup.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbCustomGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			throw new Exception(ex.Message);
		}
	}

	private void AllcomboFillFromPurchaseInvoice()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			((UltraGridBase)cmbparentcategoryCombo).DataSource = null;
			((Control)(object)cmbparentcategoryCombo).Text = string.Empty;
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			DataTable dtCombo = InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(obj);
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
			DataTable dtCrop = new DataTable();
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("Name");
			dtCrop.Clear();
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
			DataTable dtDocType = new DataTable();
			dtDocType.Columns.Add("Id");
			dtDocType.Columns.Add("Name");
			dtDocType.Clear();
			DataTable dtstock = new DataTable();
			dtstock.Columns.Add("Id");
			dtstock.Columns.Add("Name");
			dtstock.Clear();
			HashSet<int> validIds = new HashSet<int> { 1, 2, 4 };
			for (int i = 0; i < dtCombo.Rows.Count; i++)
			{
				string activity = Conversion.ToString(dtCombo.Rows[i]["Activity"]);
				int id = Convert.ToInt32(dtCombo.Rows[i]["Id"]);
				switch (activity)
				{
				case "ItemClassGroup":
					dtItemClass.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "ItemParentCategory":
					if (validIds.Contains(id))
					{
						dtParentCategory.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					}
					break;
				case "ItemCategory":
					dtItemCategory.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "ItemType":
					dtItemtype.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "CropYear":
					dtCrop.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "PackingType":
					dtPackingType.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "JobLot":
					dtJobLot.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "WareHouse":
					dtwarehouse.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "ItemName":
					dtItemName.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "StockAccount":
					dtstock.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "Supplier":
					dtSupp.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "CommissionAgent":
					dtComm.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				case "DocumentType":
					dtDocType.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					break;
				}
			}
			DDL.BindDDL(dtItemClass, cmbItemClass, "Id", "Name", "Item Class", ZeroIndex: true);
			DDL.BindDDL(dtParentCategory, cmbparentcategoryCombo, "Id", "Name", "Parent Category", ZeroIndex: false);
			UltraGridColumn obj2 = ((UltraGridBase)cmbparentcategoryCombo).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj2).Key = "Selected";
			((HeaderBase)obj2.Header).Caption = string.Empty;
			obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj2.DataType = typeof(bool);
			((HeaderBase)obj2.Header).VisiblePosition = 0;
			cmbparentcategoryCombo.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbparentcategoryCombo.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbparentcategoryCombo.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbparentcategoryCombo.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			DDL.BindDDL(dtItemCategory, cmbItemCategory, "Id", "Name", "Item Category", ZeroIndex: true);
			DDL.BindDDL(dtItemtype, cmbItemtype, "Id", "Name", "Item Type", ZeroIndex: true);
			DDL.BindDDL(dtCrop, cmbCropYear, "Id", "Name", "Crop Year", ZeroIndex: true);
			DDL.BindDDL(dtPackingType, cmbPackingtype, "Id", "Name", "Packing Type", ZeroIndex: true);
			DDL.BindDDL(dtJobLot, cmbjoblot, "Id", "Name", "Job Lot", ZeroIndex: true);
			DDL.BindDDL(dtwarehouse, cmbwarehouse, "Id", "Name", "Warehouse Name", ZeroIndex: true);
			DDL.BindDDL(dtItemName, cmbItemName, "Id", "Name", "Item Name", ZeroIndex: true);
			DDL.BindDDL(dtstock, CmbStockAccount, "Id", "Name", "Stock Account", ZeroIndex: true);
			DDL.BindDDL(dtSupp, cmbsupplier, "Id", "Name", "Party Name", ZeroIndex: true);
			DDL.BindDDL(dtComm, cmbcommagent, "Id", "Name", "Commission Agent", ZeroIndex: true);
			DDL.BindDDL(dtDocType, CmbDocumentType, "Id", "Name", "Transaction Type", ZeroIndex: true);
			((UltraGridBase)cmbItemClass).Rows[0].Activate();
			((UltraGridBase)cmbItemCategory).Rows[0].Activate();
			((UltraGridBase)cmbItemtype).Rows[0].Activate();
			((UltraGridBase)cmbCropYear).Rows[0].Activate();
			((UltraGridBase)cmbPackingtype).Rows[0].Activate();
			((UltraGridBase)cmbjoblot).Rows[0].Activate();
			((UltraGridBase)cmbwarehouse).Rows[0].Activate();
			((UltraGridBase)cmbItemName).Rows[0].Activate();
			((UltraGridBase)CmbStockAccount).Rows[0].Activate();
			((UltraGridBase)cmbsupplier).Rows[0].Activate();
			((UltraGridBase)cmbcommagent).Rows[0].Activate();
			((UltraGridBase)CmbDocumentType).Rows[0].Activate();
			((UltraGridBase)cmbItemClass).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbparentcategoryCombo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbCropYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbPackingtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbjoblot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbwarehouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbStockAccount).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbsupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbcommagent).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllcomboFillFromEvaluation()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			((UltraGridBase)cmbParentcategorySumm).DataSource = null;
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = "56,57";
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			DataTable dtCombo = StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists(obj);
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
			DataTable dtCrop = new DataTable();
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("Name");
			dtCrop.Clear();
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
			DataTable dtStock = dtDistrict.Clone();
			HashSet<int> validIds = new HashSet<int> { 1, 2, 4 };
			for (int i = 0; i < dtCombo.Rows.Count; i++)
			{
				string activityType = Conversion.ToString(dtCombo.Rows[i]["ActivityType"]);
				int id = Conversion.ToInt(dtCombo.Rows[i]["Id"]);
				string name = Conversion.ToString(dtCombo.Rows[i]["name"]);
				switch (activityType)
				{
				case "ItemClassGroup":
					dtItemClass.Rows.Add(id, name);
					break;
				case "ParentCategories":
					if (validIds.Contains(id))
					{
						dtParentCategory.Rows.Add(id, name);
					}
					break;
				case "ItemCategories":
					dtItemCategory.Rows.Add(id, name);
					break;
				case "ItemTypes":
					dtItemtype.Rows.Add(id, name);
					break;
				case "CropYear":
					dtCrop.Rows.Add(id, name);
					break;
				case "PackingType":
					dtPackingType.Rows.Add(id, name);
					break;
				case "JobLot":
					dtJobLot.Rows.Add(id, name);
					break;
				case "Warehouse":
					dtwarehouse.Rows.Add(id, name);
					break;
				case "Items":
					dtItemName.Rows.Add(id, name);
					break;
				case "Supplier_Customer":
					dtSupp.Rows.Add(id, name);
					break;
				case "City":
					dtCity.Rows.Add(id, name);
					break;
				case "District":
					dtDistrict.Rows.Add(id, name);
					break;
				case "Stock_Account":
					dtStock.Rows.Add(id, name);
					break;
				}
			}
			DDL.BindDDL(dtItemClass, CmbItemClassSumm, "Id", "Name", "Item Class", ZeroIndex: true);
			DDL.BindDDL(dtParentCategory, cmbParentcategorySumm, "Id", "Name", "Parent Category", ZeroIndex: false);
			UltraGridColumn obj2 = ((UltraGridBase)cmbParentcategorySumm).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj2).Key = "Selected";
			((HeaderBase)obj2.Header).Caption = string.Empty;
			obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj2.DataType = typeof(bool);
			((HeaderBase)obj2.Header).VisiblePosition = 0;
			cmbParentcategorySumm.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbParentcategorySumm.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbParentcategorySumm.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbParentcategorySumm.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			DDL.BindDDL(dtItemCategory, CmbCategorySumm, "Id", "Name", "Item Category", ZeroIndex: true);
			DDL.BindDDL(dtItemtype, cmbItemtypeSumm, "Id", "Name", "Item Type", ZeroIndex: true);
			DDL.BindDDL(dtCrop, CmbCropYearSumm, "Id", "Name", "Crop Year", ZeroIndex: true);
			DDL.BindDDL(dtPackingType, CmbPackingTypeSumm, "Id", "Name", "Packing Type", ZeroIndex: true);
			DDL.BindDDL(dtJobLot, cmbJobLotSumm, "Id", "Name", "Job Lot", ZeroIndex: true);
			DDL.BindDDL(dtwarehouse, CmbWareHouseSumm, "Id", "Name", "Warehouse Name", ZeroIndex: true);
			DDL.BindDDL(dtItemName, cmbItemNameSumm, "Id", "Name", "Item Name", ZeroIndex: true);
			DDL.BindDDL(dtSupp, cmbSupplierSumm, "Id", "Name", "Party Name", ZeroIndex: true);
			DDL.BindDDL(dtCity, cmbcitySumm, "Id", "Name", "City", ZeroIndex: true);
			DDL.BindDDL(dtDistrict, cmbDistrictSumm, "Id", "Name", "Distict", ZeroIndex: true);
			DDL.BindDDL(dtStock, CmbStockAccountSummary, "Id", "Name", "Stock A/c", ZeroIndex: true);
			((UltraGridBase)CmbItemClassSumm).Rows[0].Activate();
			((UltraGridBase)CmbCategorySumm).Rows[0].Activate();
			((UltraGridBase)cmbItemtypeSumm).Rows[0].Activate();
			((UltraGridBase)CmbCropYearSumm).Rows[0].Activate();
			((UltraGridBase)CmbPackingTypeSumm).Rows[0].Activate();
			((UltraGridBase)cmbJobLotSumm).Rows[0].Activate();
			((UltraGridBase)CmbWareHouseSumm).Rows[0].Activate();
			((UltraGridBase)cmbItemNameSumm).Rows[0].Activate();
			((UltraGridBase)cmbSupplierSumm).Rows[0].Activate();
			((UltraGridBase)cmbcitySumm).Rows[0].Activate();
			((UltraGridBase)cmbDistrictSumm).Rows[0].Activate();
			((UltraGridBase)CmbStockAccountSummary).Rows[0].Activate();
			((UltraGridBase)CmbItemClassSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbParentcategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbCategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemtypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbCropYearSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbPackingTypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbJobLotSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbWareHouseSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbItemNameSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbcitySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbDistrictSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbStockAccountSummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			throw new Exception(ex.Message);
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
			dtreporttype.Rows.Add("Purchase Summary By HsCode", "Purchase Summary By HsCode");
			DDL.BindDDLNew(dtreporttype, cmbReportTypeSummary, "Id", "name", "Report Type", false);
			((UltraGridBase)cmbReportTypeSummary).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			GridFill();
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void GridFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtDetail.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDate.Value);
			obj.BillNoFrom = Conversion.ToInt((object)txtFromDocNo.Text.Trim());
			obj.BillNoTo = Conversion.ToInt((object)txtToDocNo.Text.Trim());
			obj.ItemClassId = Conversion.ToInt(cmbItemClass.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtype.Value);
			obj.CropYear = ((((UltraGridBase)cmbCropYear).ActiveRow != null && Conversion.ToInt(cmbCropYear.Value) > 0) ? Conversion.ToString((object)((Control)(object)cmbCropYear).Text) : null);
			obj.PackingTypeId = Conversion.ToInt(cmbPackingtype.Value);
			obj.JobLotId = Conversion.ToInt(cmbjoblot.Value);
			obj.WarehouseId = Conversion.ToInt(cmbwarehouse.Value);
			obj.ItemId = Conversion.ToInt(cmbItemName.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbsupplier.Value);
			obj.CommissionAgentId = Conversion.ToInt(cmbcommagent.Value);
			obj.DocumentTypeId = Conversion.ToInt(CmbDocumentType.Value);
			obj.AccountId = Conversion.ToInt(CmbStockAccount.Value);
			obj.ReportType = Conversion.ToString((object)((Control)(object)cmbActivity).Text.Trim());
			string BranchIds = "";
			string ParentCategoryIds = "";
			DataTable dtParentCategory = (DataTable)((UltraGridBase)cmbparentcategoryCombo).DataSource;
			if (Conversion.ToString((object)((Control)(object)cmbparentcategoryCombo).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbparentcategoryCombo).Text) + ",").Split(',');
				foreach (string author3 in array)
				{
					DataRow[] dr3 = dtParentCategory.Select("Name='" + Conversion.ToString((object)author3) + "'");
					if (dr3.Length != 0)
					{
						ParentCategoryIds = ParentCategoryIds + "," + Conversion.ToString(dr3[0].ItemArray[0]);
					}
				}
				obj.ParentCategoryIds = ParentCategoryIds;
			}
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author2 in array)
				{
					DataRow[] dr2 = dtBranch.Select("BranchName='" + Conversion.ToString((object)author2) + "'");
					if (dr2.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr2[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				string CustomGroupIds = "";
				if (Conversion.ToString((object)((Control)(object)CmbCustomGroup).Text) != string.Empty)
				{
					DataTable dtCutom = (DataTable)((UltraGridBase)CmbCustomGroup).DataSource;
					array = (Conversion.ToString((object)((Control)(object)CmbCustomGroup).Text) + ",").Split(',');
					foreach (string author in array)
					{
						DataRow[] dr = dtCutom.Select("AcLookUpsDescription='" + Conversion.ToString((object)author) + "'");
						if (dr.Length != 0)
						{
							CustomGroupIds = CustomGroupIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
						}
					}
					obj.CustomGroupIds = CustomGroupIds;
				}
				DataSet ds = InventoryStockEvalautionDetail.PurchasesingRegister(obj);
				detailHistory = ds.Tables?[0].Copy();
				if (detailHistory.Rows.Count > 0)
				{
					DatatableHelper.ReplaceCompLogoColumn(detailHistory, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
					if (((UltraGridBase)cmbActivity).ActiveRow == null)
					{
						throw new Exception("Report Type field required");
					}
					if (((Control)(object)cmbActivity).Text == "Detail")
					{
						dtDetail.Rows.Clear();
						for (int i = 0; i < detailHistory.Rows.Count; i++)
						{
							dtDetail.Rows.Add(detailHistory.Rows[i]["ParentCategory"], detailHistory.Rows[i]["TransactionType"], detailHistory.Rows[i]["PurchaseType"], detailHistory.Rows[i]["SupplierName"], detailHistory.Rows[i]["Id"], detailHistory.Rows[i]["DocumentTypeId"], detailHistory.Rows[i]["PurchaseOrderId"], detailHistory.Rows[i]["PoNo"], Conversion.ToDateTime(detailHistory.Rows[i]["BillDate"]).ToShortDateString(), detailHistory.Rows[i]["BillNo"], detailHistory.Rows[i]["BranchSrNo"], detailHistory.Rows[i]["ManualNo"], detailHistory.Rows[i]["WareHouseName"], detailHistory.Rows[i]["ItemName"], detailHistory.Rows[i]["UOMCode"], detailHistory.Rows[i]["CropYear"], detailHistory.Rows[i]["JobLotCode"], detailHistory.Rows[i]["PackTypeCode"], (Conversion.ToInt(detailHistory.Rows[i]["DocumentTypeId"]) == 59) ? (0.0 - Conversion.ToDouble(detailHistory.Rows[i]["ItemQty"])) : Conversion.ToDouble(detailHistory.Rows[i]["ItemQty"]), (Conversion.ToInt(detailHistory.Rows[i]["DocumentTypeId"]) == 59) ? (0.0 - Conversion.ToDouble(detailHistory.Rows[i]["QtyByParentCategory"])) : Conversion.ToDouble(detailHistory.Rows[i]["QtyByParentCategory"]), (Conversion.ToInt(detailHistory.Rows[i]["DocumentTypeId"]) == 59) ? (0.0 - Conversion.ToDouble(detailHistory.Rows[i]["BillWeight"])) : Conversion.ToDouble(detailHistory.Rows[i]["BillWeight"]), (Conversion.ToInt(detailHistory.Rows[i]["DocumentTypeId"]) == 59) ? (0.0 - Conversion.ToDouble(detailHistory.Rows[i]["StockWeight"])) : Conversion.ToDouble(detailHistory.Rows[i]["StockWeight"]), detailHistory.Rows[i]["ItemRate"], detailHistory.Rows[i]["RateCut"], detailHistory.Rows[i]["RateCutAmount"], (Conversion.ToInt(detailHistory.Rows[i]["DocumentTypeId"]) == 59) ? (0.0 - Conversion.ToDouble(detailHistory.Rows[i]["BillAmount"])) : Conversion.ToDouble(detailHistory.Rows[i]["BillAmount"]), detailHistory.Rows[i]["Commission"], detailHistory.Rows[i]["Freight"], detailHistory.Rows[i]["OExpense"], detailHistory.Rows[i]["WagesAmount"], detailHistory.Rows[i]["EmptyBagsAmount"], detailHistory.Rows[i]["EmptyBagsAmount40KG"], detailHistory.Rows[i]["FreightDeduction"], detailHistory.Rows[i]["FreightDeduction40KG"], detailHistory.Rows[i]["ItemAddLess_JV"], detailHistory.Rows[i]["TotalExpense"], detailHistory.Rows[i]["TotalExp40Kg"], (Conversion.ToInt(detailHistory.Rows[i]["DocumentTypeId"]) == 59) ? (0.0 - Conversion.ToDouble(detailHistory.Rows[i]["TotalAmount"])) : Conversion.ToDouble(detailHistory.Rows[i]["TotalAmount"]), detailHistory.Rows[i]["PartyBillAmount"], detailHistory.Rows[i]["PartyAddLess_JV"], detailHistory.Rows[i]["RateWithExp"], detailHistory.Rows[i]["StockAvgRateKg"], detailHistory.Rows[i]["AvgRatePerKgOnBillWeight"], detailHistory.Rows[i]["GpNo"], Conversion.ToDateTime(detailHistory.Rows[i]["GpDate"]).ToShortDateString(), Conversion.ToDateTime(detailHistory.Rows[i]["GrnDate"]).ToShortDateString(), detailHistory.Rows[i]["GrnNo"], detailHistory.Rows[i]["VehicleNo"], detailHistory.Rows[i]["TicketNos"], detailHistory.Rows[i]["CommissionAgent"], detailHistory.Rows[i]["CommissionType"], detailHistory.Rows[i]["CommRate"], detailHistory.Rows[i]["BrokerName"], detailHistory.Rows[i]["BrokeryType"], detailHistory.Rows[i]["BrokeryRate"], detailHistory.Rows[i]["CityName"]);
						}
						DataGridHistory.DataSource = dtDetail;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Comparison By Item")
					{
						dtComparisonByItem.Rows.Clear();
						for (int j = 0; j < detailHistory.Rows.Count; j++)
						{
							dtComparisonByItem.Rows.Add(detailHistory.Rows[j]["ParentCategory"], detailHistory.Rows[j]["ItemName"], detailHistory.Rows[j]["CropYear"], detailHistory.Rows[j]["ItemQty"], detailHistory.Rows[j]["QtyByParentCategory"], detailHistory.Rows[j]["BillWeight"], detailHistory.Rows[j]["StockWeight"], detailHistory.Rows[j]["ItemRate"], detailHistory.Rows[j]["BillAmount"], detailHistory.Rows[j]["TotalExpense"], detailHistory.Rows[j]["TotalAmount"], detailHistory.Rows[j]["RateWithExp"], detailHistory.Rows[j]["TotalExp40Kg"], detailHistory.Rows[j]["Freight40Kg"], detailHistory.Rows[j]["Comm40Kg"], detailHistory.Rows[j]["OExpense40Kg"], detailHistory.Rows[j]["Wages40Kg"], detailHistory.Rows[j]["EmptyBagsAmount"], detailHistory.Rows[j]["EmptyBagsAmount40KG"], detailHistory.Rows[j]["FreightDeduction"], detailHistory.Rows[j]["FreightDeduction40KG"], detailHistory.Rows[j]["Freight"], detailHistory.Rows[j]["Commission"], detailHistory.Rows[j]["OExpense"], detailHistory.Rows[j]["WagesAmount"], detailHistory.Rows[j]["ItemAddLess_JV"]);
						}
						DataGridHistory.DataSource = dtComparisonByItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Comparison By Item and Supplier")
					{
						dtComparisonByItemSupplier.Rows.Clear();
						for (int k = 0; k < detailHistory.Rows.Count; k++)
						{
							dtComparisonByItemSupplier.Rows.Add(detailHistory.Rows[k]["ParentCategory"], detailHistory.Rows[k]["ItemName"], detailHistory.Rows[k]["CropYear"], detailHistory.Rows[k]["SupplierName"], detailHistory.Rows[k]["ItemQty"], detailHistory.Rows[k]["QtyByParentCategory"], detailHistory.Rows[k]["BillWeight"], detailHistory.Rows[k]["StockWeight"], detailHistory.Rows[k]["ItemRate"], detailHistory.Rows[k]["BillAmount"], detailHistory.Rows[k]["TotalExpense"], detailHistory.Rows[k]["TotalAmount"], detailHistory.Rows[k]["RateWithExp"], detailHistory.Rows[k]["TotalExp40Kg"], detailHistory.Rows[k]["Freight40Kg"], detailHistory.Rows[k]["Comm40Kg"], detailHistory.Rows[k]["OExpense40Kg"], detailHistory.Rows[k]["Wages40Kg"], detailHistory.Rows[k]["EmptyBagsAmount"], detailHistory.Rows[k]["EmptyBagsAmount40KG"], detailHistory.Rows[k]["FreightDeduction"], detailHistory.Rows[k]["FreightDeduction40KG"], detailHistory.Rows[k]["Freight"], detailHistory.Rows[k]["Commission"], detailHistory.Rows[k]["OExpense"], detailHistory.Rows[k]["WagesAmount"], detailHistory.Rows[k]["ItemAddLess_JV"]);
						}
						DataGridHistory.DataSource = dtComparisonByItemSupplier;
						DataGridHistory.RetrieveStructure();
						DataGridHistory.RootTable.Columns["SupplierName"].Width = 150;
						DataGridHistorySetting();
					}
				}
				else
				{
					DataGridHistory.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 3, 2);
			DataGridHistory.RootTable.Groups.Add("ParentCategory");
			DataGridHistory.GroupTotals = (GroupTotals)2;
			DataGridHistory.RootTable.Columns["ParentCategory"].Width = 100;
			DataGridHistory.RootTable.Columns["ItemName"].Width = 180;
			DataGridHistory.RootTable.Columns["CropYear"].Width = 75;
			DataGridHistory.RootTable.Columns["ItemQty"].Width = 80;
			DataGridHistory.RootTable.Columns["QtyByParentCategory"].Width = 90;
			DataGridHistory.RootTable.Columns["BillWeight"].Width = 85;
			DataGridHistory.RootTable.Columns["StockWeight"].Width = 85;
			DataGridHistory.RootTable.Columns["ItemRate"].Width = 70;
			DataGridHistory.RootTable.Columns["BillAmount"].Width = 100;
			DataGridHistory.RootTable.Columns["Commission"].Width = 80;
			DataGridHistory.RootTable.Columns["Freight"].Width = 80;
			DataGridHistory.RootTable.Columns["Expense"].Width = 80;
			DataGridHistory.RootTable.Columns["Wages"].Width = 75;
			DataGridHistory.RootTable.Columns["TotalExpense"].Width = 75;
			DataGridHistory.RootTable.Columns["Amount+Exp"].Width = 100;
			DataGridHistory.RootTable.Columns["BillAmount"].Caption = "Item Amount";
			DataGridHistory.RootTable.Columns["EmptyBagsAmount40KG"].Caption = "Empty Bags Amount 40KG";
			DataGridHistory.RootTable.Columns["FreightDeduction40KG"].Caption = "Freight Deduction 40KG";
			if (((Control)(object)cmbActivity).Text != "Detail")
			{
				DataGridHistory.RootTable.Columns["TotalExp40Kg"].Caption = "Total Exp 40Kg";
				DataGridHistory.RootTable.Columns["Freight40Kg"].Caption = "Freight 40Kg";
				DataGridHistory.RootTable.Columns["Comm40Kg"].Caption = "Comm 40Kg";
				DataGridHistory.RootTable.Columns["Expense40Kg"].Caption = "Expense 40Kg";
				DataGridHistory.RootTable.Columns["Wages40Kg"].Caption = "Wages 40Kg";
				DataGridHistory.RootTable.Columns["RateWithExp"].Width = 85;
				DataGridHistory.RootTable.Columns["TotalExp40Kg"].Width = 85;
				DataGridHistory.RootTable.Columns["Freight40Kg"].Width = 85;
				DataGridHistory.RootTable.Columns["Comm40Kg"].Width = 75;
				DataGridHistory.RootTable.Columns["Expense40Kg"].Width = 80;
				DataGridHistory.RootTable.Columns["Wages40Kg"].Width = 80;
			}
			if (((Control)(object)cmbActivity).Text == "Detail")
			{
				DataGridHistory.RootTable.Columns["Id"].Visible = false;
				DataGridHistory.RootTable.Columns["PurchaseOrderId"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
				DataGridHistory.RootTable.Columns["PoNo"].ColumnType = (ColumnType)5;
				DataGridHistory.RootTable.Columns["BillNo"].ColumnType = (ColumnType)5;
				DataGridHistory.RootTable.Columns["PoNo"].Caption = "Order No";
				DataGridHistory.RootTable.Columns["Expense"].Caption = "Other Expense";
				DataGridHistory.RootTable.Columns["RateWithExp"].Caption = "AvgRate 40Kg";
				DataGridHistory.RootTable.Columns["StockAvgRateKg"].Caption = "AvgRate PerKG (@StockWeight)";
				DataGridHistory.RootTable.Columns["AvgRatePerKgOnBillWeight"].Caption = "AvgRate PerKg (@BillWeight)";
				DataGridHistory.RootTable.Columns["TransactionType"].Width = 80;
				DataGridHistory.RootTable.Columns["PurchaseType"].Width = 100;
				DataGridHistory.RootTable.Columns["SupplierName"].Width = 150;
				DataGridHistory.RootTable.Columns["BillDate"].Width = 80;
				DataGridHistory.RootTable.Columns["BillNo"].Width = 50;
				DataGridHistory.RootTable.Columns["BranchSrNo"].Width = 50;
				DataGridHistory.RootTable.Columns["ManualNo"].Width = 80;
				DataGridHistory.RootTable.Columns["GpNo"].Width = 50;
				DataGridHistory.RootTable.Columns["PoNo"].Width = 50;
				DataGridHistory.RootTable.Columns["GpDate"].Width = 75;
				DataGridHistory.RootTable.Columns["GrnDate"].Width = 80;
				DataGridHistory.RootTable.Columns["GrnNo"].Width = 50;
				DataGridHistory.RootTable.Columns["PackUom"].Width = 55;
				DataGridHistory.RootTable.Columns["JobLot"].Width = 90;
				DataGridHistory.RootTable.Columns["PackingType"].Width = 95;
				DataGridHistory.RootTable.Columns["Exp/40kg"].Width = 70;
				DataGridHistory.RootTable.Columns["PartyBillAmount"].Width = 100;
				DataGridHistory.RootTable.Columns["PartyAddLess_JV"].Width = 100;
				DataGridHistory.RootTable.Columns["RateWithExp"].Width = 70;
				DataGridHistory.RootTable.Columns["WareHouseName"].Width = 110;
				DataGridHistory.RootTable.Columns["TicketNos"].Width = 70;
				DataGridHistory.RootTable.Columns["AvgRatePerKgOnBillWeight"].Width = 90;
				GridEX_Helper.AddSelectButton(DataGridHistory, "Select", 20, 0);
				GridEX_Helper.AddButton(DataGridHistory, "ApprovalDetail", "ApprovalDetail", 100, 1);
				DataGridHistory.FrozenColumns = 2;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				if (e.Column.Key == "PoNo")
				{
					int PurchaseOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
					new ActivityDetails(UserAccount, PurchaseOrderId).Show();
				}
				if (e.Column.Key == "BillNo")
				{
					int BillId = Conversion.ToInt(r.Cells["Id"].Value);
					new PurchaseInvoiceReverseActivity(UserAccount, BillId).Show();
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
	}

	private void PrintButtonManage()
	{
		try
		{
			if (Conversion.ToInt(cmbActivity.Value) > 0)
			{
				if (((Control)(object)cmbActivity).Text == "Detail")
				{
					btnPrint.DropDownItems.Clear();
					string[] files3 = CommonServices.DynamicReportsLoad("PurchaseRegister_DetailReports");
					if (files3 != null && files3.Length != 0)
					{
						for (int k = 0; k < files3.Length; k++)
						{
							btnPrint.DropDownItems.Add(Path.GetFileNameWithoutExtension(files3[k]));
						}
					}
				}
				if (((Control)(object)cmbActivity).Text == "Comparison By Item")
				{
					btnPrint.DropDownItems.Clear();
					string[] files2 = CommonServices.DynamicReportsLoad("PurchaseRegister_ComparisonByItemReports");
					if (files2 != null && files2.Length != 0)
					{
						for (int j = 0; j < files2.Length; j++)
						{
							btnPrint.DropDownItems.Add(Path.GetFileNameWithoutExtension(files2[j]));
						}
					}
				}
				if (!(((Control)(object)cmbActivity).Text == "Comparison By Item and Supplier"))
				{
					return;
				}
				btnPrint.DropDownItems.Clear();
				string[] files = CommonServices.DynamicReportsLoad("PurchaseRegister_ComparisonByItemAndSupplierReports");
				if (files != null && files.Length != 0)
				{
					for (int i = 0; i < files.Length; i++)
					{
						btnPrint.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[i]));
					}
				}
			}
			else
			{
				btnPrint.Visible = false;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			waitForm.Show(this);
			AllcomboFillFromPurchaseInvoice();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
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
			waitForm.Show(this);
			AllcomboFillFromEvaluation();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
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
			((Control)(object)CmbCropYearSumm).Text = string.Empty;
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		try
		{
			waitForm.Show(this);
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
			obj.CropYear = ((((UltraGridBase)CmbCropYearSumm).ActiveRow != null && Conversion.ToInt(CmbCropYearSumm.Value) > 0) ? Conversion.ToString((object)((Control)(object)CmbCropYearSumm).Text) : null);
			obj.PackingTypeId = Conversion.ToInt(CmbPackingTypeSumm.Value);
			obj.DistrictId = Conversion.ToInt(cmbDistrictSumm.Value);
			obj.AccountId = Conversion.ToInt(CmbStockAccountSummary.Value);
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
			string BranchIds = "";
			string ParentCategoryIds = "";
			DataTable dtParentCategory = (DataTable)((UltraGridBase)cmbParentcategorySumm).DataSource;
			if (Conversion.ToString((object)((Control)(object)cmbParentcategorySumm).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbParentcategorySumm).Text) + ",").Split(',');
				foreach (string author2 in array)
				{
					DataRow[] dr2 = dtParentCategory.Select("Name='" + Conversion.ToString((object)author2) + "'");
					if (dr2.Length != 0)
					{
						ParentCategoryIds = ParentCategoryIds + "," + Conversion.ToString(dr2[0].ItemArray[0]);
					}
				}
				obj.ParentCategoryIds = ParentCategoryIds;
			}
			if (Conversion.ToString((object)((Control)(object)CmbBranchNameSumm).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchNameSumm).Text) + ",").Split(',');
				foreach (string author3 in array)
				{
					DataRow[] dr3 = dtBranch.Select("BranchName='" + Conversion.ToString((object)author3) + "'");
					if (dr3.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr3[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				string CustomGroupIds = "";
				if (Conversion.ToString((object)((Control)(object)CmbCustomGroupSummary).Text) != string.Empty)
				{
					DataTable dtCustomGroup = (DataTable)((UltraGridBase)CmbCustomGroupSummary).DataSource;
					array = (Conversion.ToString((object)((Control)(object)CmbCustomGroupSummary).Text) + ",").Split(',');
					foreach (string author in array)
					{
						DataRow[] dr = dtCustomGroup.Select("AcLookUpsDescription='" + Conversion.ToString((object)author) + "'");
						if (dr.Length != 0)
						{
							CustomGroupIds = CustomGroupIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
						}
					}
					obj.CustomGroupIds = CustomGroupIds;
				}
				dtGridSummary = InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations(obj);
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
								dtPurchaseRegister.Rows.Add(row["DocDate"], row["RefDocumentTypeId"], row["DocumentTypeCode"], row["DocCodeNo"], row["PartyName"], row["ItemCode"], row["ItemName"], row["UOMCode"], row["CropBatch"], row["JobLotCode"], row["PackTypeCode"], row["VehicleNo"], row["CityName"], row["Qty"], row["GrossWeight"], row["EbTotal"], row["AddLess"], row["BillWeight"], row["StockWeight"], row["AvgRate"], row["Amount"], row["TotalExpenses"], row["TotalAmountWithExp"], row["AvgRateExp"], row["Freight"], row["Commission"], row["Expenses"], row["Wages"], row["EmptyBagsAmount"], row["FreightDeduction"], row["PrcntOfTotal"], row["PrcntOfTotalWeight"], row["ParentCategory"], row["ItemCategory"], row["ItemType"]);
							}
							GridSummary.DataSource = dtPurchaseRegister;
							GridSummary.RetrieveStructure();
							GridSummary.RootTable.Columns["DocCodeNo"].Caption = "Doc No";
							GridSummary.RootTable.Columns["DocumentType"].Caption = "Doc Type";
							GridSummary.RootTable.Columns["CropBatch"].Caption = "Crop Year";
							GridSummary.RootTable.Columns["JobLotCode"].Caption = "Job Lot";
							GridSummary.RootTable.Columns["PackTypeCode"].Caption = "Packing Type";
							GridSummary.RootTable.Columns["DocDate"].Width = 80;
							GridSummary.RootTable.Columns["DocumentType"].Width = 70;
							GridSummary.RootTable.Columns["DocCodeNo"].Width = 50;
							GridSummary.RootTable.Columns["PartyName"].Width = 120;
							GridSummary.RootTable.Columns["ParentCategory"].Width = 110;
							GridSummary.RootTable.Columns["ItemCategory"].Width = 110;
							GridSummary.RootTable.Columns["ItemType"].Width = 100;
							GridSummary.RootTable.Columns["ItemCode"].Width = 60;
							GridSummary.RootTable.Columns["ItemName"].Width = 120;
							GridSummary.RootTable.Columns["UOMCode"].Width = 60;
							GridSummary.RootTable.Columns["CropBatch"].Width = 75;
							GridSummary.RootTable.Columns["JobLotCode"].Width = 90;
							GridSummary.RootTable.Columns["PackTypeCode"].Width = 95;
							GridSummary.RootTable.Columns["VehicleNo"].Width = 80;
							GridSummary.RootTable.Columns["CityName"].Width = 120;
							GridSummarySetting();
						}
						else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item")
						{
							((Control)(object)GridSummary).Name = "dtPurchaseSummaryByItem";
							dtPurchaseSummaryByItem.Rows.Clear();
							foreach (DataRow row2 in dtGridSummary.Rows)
							{
								dtPurchaseSummaryByItem.Rows.Add(row2["CityName"], row2["ItemCode"], row2["ItemName"], row2["UOMCode"], row2["Qty"], row2["GrossWeight"], row2["EbTotal"], row2["AddLess"], row2["BillWeight"], row2["StockWeight"], row2["AvgRate"], row2["Amount"], row2["TotalExpenses"], row2["TotalAmountWithExp"], row2["AvgRateExp"], row2["Freight"], row2["Commission"], row2["Expenses"], row2["Wages"], row2["EmptyBagsAmount"], row2["FreightDeduction"], row2["PrcntOfTotal"], row2["PrcntOfTotalWeight"], row2["ParentCategory"], row2["ItemCategory"], row2["ItemType"]);
							}
							GridSummary.DataSource = dtPurchaseSummaryByItem;
							GridSummary.RetrieveStructure();
							GridSummary.RootTable.Columns["ParentCategory"].Width = 110;
							GridSummary.RootTable.Columns["ItemCategory"].Width = 110;
							GridSummary.RootTable.Columns["ItemType"].Width = 100;
							GridSummary.RootTable.Columns["ItemCode"].Width = 80;
							GridSummary.RootTable.Columns["ItemName"].Width = 120;
							GridSummary.RootTable.Columns["UOMCode"].Width = 60;
							GridSummarySetting();
						}
						else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Supplier")
						{
							((Control)(object)GridSummary).Name = "dtPurchaseSummaryBySupplier";
							dtPurchaseSummaryBySupplier.Rows.Clear();
							foreach (DataRow row3 in dtGridSummary.Rows)
							{
								dtPurchaseSummaryBySupplier.Rows.Add(row3["CityName"], row3["PartyName"], row3["UOMCode"], row3["Qty"], row3["GrossWeight"], row3["EbTotal"], row3["AddLess"], row3["BillWeight"], row3["StockWeight"], row3["AvgRate"], row3["Amount"], row3["TotalExpenses"], row3["TotalAmountWithExp"], row3["AvgRateExp"], row3["Freight"], row3["Commission"], row3["Expenses"], row3["Wages"], row3["EmptyBagsAmount"], row3["FreightDeduction"], row3["PrcntOfTotal"], row3["PrcntOfTotalWeight"]);
							}
							GridSummary.DataSource = dtPurchaseSummaryBySupplier;
							GridSummary.RetrieveStructure();
							GridSummary.RootTable.Columns["CityName"].Width = 110;
							GridSummary.RootTable.Columns["PartyName"].Width = 120;
							GridSummary.RootTable.Columns["UOMCode"].Width = 60;
							GridSummarySetting();
						}
						else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item & Supplier")
						{
							((Control)(object)GridSummary).Name = "dtPurchaseSummaryByItemAndSupplier";
							dtPurchaseSummaryByItemAndSupplier.Rows.Clear();
							foreach (DataRow row4 in dtGridSummary.Rows)
							{
								dtPurchaseSummaryByItemAndSupplier.Rows.Add(row4["CityName"], row4["PartyName"], row4["ItemCode"], row4["ItemName"], row4["UOMCode"], row4["Qty"], row4["GrossWeight"], row4["EbTotal"], row4["AddLess"], row4["BillWeight"], row4["StockWeight"], row4["AvgRate"], row4["Amount"], row4["TotalExpenses"], row4["TotalAmountWithExp"], row4["AvgRateExp"], row4["Freight"], row4["Commission"], row4["Expenses"], row4["Wages"], row4["EmptyBagsAmount"], row4["FreightDeduction"], row4["PrcntOfTotal"], row4["PrcntOfTotalWeight"], row4["ParentCategory"], row4["ItemCategory"], row4["ItemType"]);
							}
							GridSummary.DataSource = dtPurchaseSummaryByItemAndSupplier;
							GridSummary.RetrieveStructure();
							GridSummary.RootTable.Columns["CityName"].Width = 110;
							GridSummary.RootTable.Columns["PartyName"].Width = 120;
							GridSummary.RootTable.Columns["ParentCategory"].Width = 110;
							GridSummary.RootTable.Columns["ItemCategory"].Width = 110;
							GridSummary.RootTable.Columns["ItemType"].Width = 100;
							GridSummary.RootTable.Columns["ItemCode"].Width = 80;
							GridSummary.RootTable.Columns["ItemName"].Width = 120;
							GridSummary.RootTable.Columns["UOMCode"].Width = 60;
							GridSummarySetting();
						}
						else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item & Warehouse")
						{
							((Control)(object)GridSummary).Name = "dtPurchaseSummaryByItemAndWarehouse";
							dtPurchaseSummaryByItemAndWarehouse.Rows.Clear();
							foreach (DataRow row5 in dtGridSummary.Rows)
							{
								dtPurchaseSummaryByItemAndWarehouse.Rows.Add(row5["CityName"], row5["ItemCode"], row5["ItemName"], row5["WarehouseName"], row5["UOMCode"], row5["Qty"], row5["GrossWeight"], row5["EbTotal"], row5["AddLess"], row5["BillWeight"], row5["StockWeight"], row5["AvgRate"], row5["Amount"], row5["TotalExpenses"], row5["TotalAmountWithExp"], row5["AvgRateExp"], row5["Freight"], row5["Commission"], row5["Expenses"], row5["Wages"], row5["EmptyBagsAmount"], row5["FreightDeduction"], row5["PrcntOfTotal"], row5["PrcntOfTotalWeight"], row5["ParentCategory"], row5["ItemCategory"], row5["ItemType"]);
							}
							GridSummary.DataSource = dtPurchaseSummaryByItemAndWarehouse;
							GridSummary.RetrieveStructure();
							GridSummary.RootTable.Columns["CityName"].Width = 110;
							GridSummary.RootTable.Columns["ParentCategory"].Width = 110;
							GridSummary.RootTable.Columns["ItemCategory"].Width = 110;
							GridSummary.RootTable.Columns["ItemType"].Width = 100;
							GridSummary.RootTable.Columns["ItemCode"].Width = 80;
							GridSummary.RootTable.Columns["ItemName"].Width = 120;
							GridSummary.RootTable.Columns["WarehouseName"].Width = 110;
							GridSummary.RootTable.Columns["UOMCode"].Width = 60;
							GridSummarySetting();
						}
						else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Parent Category")
						{
							((Control)(object)GridSummary).Name = "dtPurchaseSummaryByParentCategory";
							dtPurchaseSummaryByParentCategory.Rows.Clear();
							foreach (DataRow row6 in dtGridSummary.Rows)
							{
								dtPurchaseSummaryByParentCategory.Rows.Add(row6["CityName"], row6["ParentCategory"], row6["UOMCode"], row6["Qty"], row6["GrossWeight"], row6["EbTotal"], row6["AddLess"], row6["BillWeight"], row6["StockWeight"], row6["AvgRate"], row6["Amount"], row6["TotalExpenses"], row6["TotalAmountWithExp"], row6["AvgRateExp"], row6["Freight"], row6["Commission"], row6["Expenses"], row6["Wages"], row6["EmptyBagsAmount"], row6["FreightDeduction"], row6["PrcntOfTotal"], row6["PrcntOfTotalWeight"]);
							}
							GridSummary.DataSource = dtPurchaseSummaryByParentCategory;
							GridSummary.RetrieveStructure();
							GridSummary.RootTable.Columns["CityName"].Width = 70;
							GridSummary.RootTable.Columns["ParentCategory"].Width = 110;
							GridSummary.RootTable.Columns["UOMCode"].Width = 60;
							GridSummarySetting();
						}
						else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Parent Category & Supplier")
						{
							((Control)(object)GridSummary).Name = "dtPurchaseSummaryByParentCategoryAndSupplier";
							dtPurchaseSummaryByParentCategoryAndSupplier.Rows.Clear();
							foreach (DataRow row7 in dtGridSummary.Rows)
							{
								dtPurchaseSummaryByParentCategoryAndSupplier.Rows.Add(row7["CityName"], row7["PartyName"], row7["ParentCategory"], row7["UOMCode"], row7["Qty"], row7["GrossWeight"], row7["EbTotal"], row7["AddLess"], row7["BillWeight"], row7["StockWeight"], row7["AvgRate"], row7["Amount"], row7["TotalExpenses"], row7["TotalAmountWithExp"], row7["AvgRateExp"], row7["Freight"], row7["Commission"], row7["Expenses"], row7["Wages"], row7["EmptyBagsAmount"], row7["FreightDeduction"], row7["PrcntOfTotal"], row7["PrcntOfTotalWeight"]);
							}
							GridSummary.DataSource = dtPurchaseSummaryByParentCategoryAndSupplier;
							GridSummary.RetrieveStructure();
							GridSummary.RootTable.Columns["CityName"].Width = 110;
							GridSummary.RootTable.Columns["PartyName"].Width = 120;
							GridSummary.RootTable.Columns["ParentCategory"].Width = 110;
							GridSummary.RootTable.Columns["UOMCode"].Width = 60;
							GridSummarySetting();
						}
						else
						{
							if (!(((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By HsCode"))
							{
								return;
							}
							((Control)(object)GridSummary).Name = "PurchaseSummaryByHsCode";
							dtPurchaseSummaryByHsCode.Rows.Clear();
							foreach (DataRow row8 in dtGridSummary.Rows)
							{
								dtPurchaseSummaryByHsCode.Rows.Add(row8["CityName"], row8["HsCode"], row8["UOMCode"], row8["Qty"], row8["GrossWeight"], row8["EbTotal"], row8["AddLess"], row8["BillWeight"], row8["StockWeight"], row8["AvgRate"], row8["Amount"], row8["TotalExpenses"], row8["TotalAmountWithExp"], row8["AvgRateExp"], row8["Freight"], row8["Commission"], row8["Expenses"], row8["Wages"], row8["EmptyBagsAmount"], row8["FreightDeduction"], row8["PrcntOfTotal"], row8["PrcntOfTotalWeight"]);
							}
							GridSummary.DataSource = dtPurchaseSummaryByHsCode;
							GridSummary.RetrieveStructure();
							GridSummary.RootTable.Columns["CityName"].Width = 110;
							GridSummary.RootTable.Columns["HsCode"].Width = 100;
							GridSummary.RootTable.Columns["UOMCode"].Width = 60;
							GridSummarySetting();
						}
					}
					else
					{
						GridSummary.DataSource = null;
					}
				}
				else
				{
					GridSummary.DataSource = null;
				}
				return;
			}
			CmbBranchNameSumm.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void GridSummarySetting()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridSummary.AllowDelete = (InheritableBoolean)2;
			GridSummary.AllowEdit = (InheritableBoolean)2;
			if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Register")
			{
				GridSummary.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			}
			foreach (GridEXColumn item in (JanusCollectionBase)GridSummary.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			if (((Control)(object)cmbReportTypeSummary).Text != "Purchase Summary By Supplier" && ((Control)(object)cmbReportTypeSummary).Text != "Purchase Summary By HsCode")
			{
				GridSummary.RootTable.Groups.Add("ParentCategory");
				GridSummary.GroupTotals = (GroupTotals)2;
			}
			GridSummary.RootTable.HeaderLines = 2;
			GridSummary.RootTable.Columns["PrcntOfTotal"].Caption = "Pct Of Total";
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].Caption = "Pct Of Total Weight";
			GridSummary.RootTable.Columns["UOMCode"].Caption = "Pack Uom";
			GridSummary.RootTable.Columns["Qty"].Width = 70;
			GridSummary.RootTable.Columns["GrossWeight"].Width = 80;
			GridSummary.RootTable.Columns["EbTotal"].Width = 65;
			GridSummary.RootTable.Columns["AddLess"].Width = 70;
			GridSummary.RootTable.Columns["BillWeight"].Width = 90;
			GridSummary.RootTable.Columns["StockWeight"].Width = 85;
			GridSummary.RootTable.Columns["AvgRate"].Width = 75;
			GridSummary.RootTable.Columns["Amount"].Width = 100;
			GridSummary.RootTable.Columns["TotalExpenses"].Width = 85;
			GridSummary.RootTable.Columns["TotalAmountWithExp"].Width = 100;
			GridSummary.RootTable.Columns["AvgRateExp"].Width = 75;
			GridSummary.RootTable.Columns["Freight"].Width = 75;
			GridSummary.RootTable.Columns["Commission"].Width = 80;
			GridSummary.RootTable.Columns["Expenses"].Width = 70;
			GridSummary.RootTable.Columns["Wages"].Width = 70;
			GridSummary.RootTable.Columns["EmptyBagsAmount"].Width = 85;
			GridSummary.RootTable.Columns["FreightDeduction"].Width = 85;
			GridSummary.RootTable.Columns["PrcntOfTotal"].Width = 60;
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].Width = 80;
			GridSummary.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["EbTotal"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["AddLess"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["AddLess"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["BillWeight"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
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
			GridSummary.RootTable.Columns["Wages"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["Wages"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["Wages"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["Wages"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["EmptyBagsAmount"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["EmptyBagsAmount"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["EmptyBagsAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["EmptyBagsAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["FreightDeduction"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["FreightDeduction"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["FreightDeduction"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["FreightDeduction"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridSummary.RootTable.Columns["PrcntOfTotal"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["PrcntOfTotal"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["PrcntOfTotal"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["PrcntOfTotal"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["PrcntOfTotalWeight"].TotalFormatString = "#,##0.###";
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrintSumm.Text == "56_01_PurchaseRegister")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGridSummary, "56_01_PurchaseRegister.rpt");
			}
			else if (btnPrintSumm.Text == "56_02_PurchaseSummaryByItem")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtGridSummary, "56_02_PurchaseSummaryByItem.rpt");
			}
			else if (btnPrintSumm.Text == "56_03_PurchaseSummaryByItemAndWarehouse")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtGridSummary, "56_03_PurchaseSummaryByItemAndWarehouse.rpt");
			}
			else if (btnPrintSumm.Text == "56_04_PurchaseSummaryBySupplier")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtGridSummary, "56_04_PurchaseSummaryBySupplier.rpt");
			}
			else if (btnPrintSumm.Text == "56_05_PurchaseSummaryByItemAndSupplier")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtGridSummary, "56_05_PurchaseSummaryByItemAndSupplier.rpt");
			}
			else if (btnPrintSumm.Text == "56_06_PurchaseSummaryByParentCategory")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtGridSummary, "56_06_PurchaseSummaryByParentCategory.rpt");
			}
			else if (btnPrintSumm.Text == "56_07_PurchaseSummaryByParentCategoryAndSupplier")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtGridSummary, "56_07_PurchaseSummaryByParentCategoryAndSupplier.rpt");
			}
			else if (btnPrintSumm.Text == "56_09_PurchaseSummaryByHsCode")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtGridSummary, "56_09_PurchaseSummaryByHsCode.rpt");
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
			if (((Control)(object)cmbReportTypeSummary).Text != string.Empty)
			{
				btnPrintSumm.Visible = true;
				if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Register")
				{
					btnPrintSumm.Text = "56_01_PurchaseRegister";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item")
				{
					btnPrintSumm.Text = "56_02_PurchaseSummaryByItem";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item & Warehouse")
				{
					btnPrintSumm.Text = "56_03_PurchaseSummaryByItemAndWarehouse";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Supplier")
				{
					btnPrintSumm.Text = "56_04_PurchaseSummaryBySupplier";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Item & Supplier")
				{
					btnPrintSumm.Text = "56_05_PurchaseSummaryByItemAndSupplier";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Parent Category")
				{
					btnPrintSumm.Text = "56_06_PurchaseSummaryByParentCategory";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By Parent Category & Supplier")
				{
					btnPrintSumm.Text = "56_07_PurchaseSummaryByParentCategoryAndSupplier";
				}
				else if (((Control)(object)cmbReportTypeSummary).Text == "Purchase Summary By HsCode")
				{
					btnPrintSumm.Text = "56_09_PurchaseSummaryByHsCode";
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
				txtToDateSumm.Value = DateTime.Today;
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

	private void btnShortcutKeys_Click_1(object sender, EventArgs e)
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
					btnRefreshSummary_Click(null, null);
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
					txtToDate.Focus();
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

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchName).Text != string.Empty && ((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				AllcomboFillFromPurchaseInvoice();
				return;
			}
			((Control)(object)cmbItemClass).Text = string.Empty;
			((UltraGridBase)cmbItemClass).DataSource = null;
			((Control)(object)cmbparentcategoryCombo).Text = string.Empty;
			((UltraGridBase)cmbparentcategoryCombo).DataSource = null;
			((Control)(object)cmbItemCategory).Text = string.Empty;
			((UltraGridBase)cmbItemCategory).DataSource = null;
			((Control)(object)cmbItemtype).Text = string.Empty;
			((UltraGridBase)cmbItemtype).DataSource = null;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((UltraGridBase)cmbCropYear).DataSource = null;
			((Control)(object)cmbPackingtype).Text = string.Empty;
			((UltraGridBase)cmbPackingtype).DataSource = null;
			((Control)(object)cmbjoblot).Text = string.Empty;
			((UltraGridBase)cmbjoblot).DataSource = null;
			((Control)(object)cmbwarehouse).Text = string.Empty;
			((UltraGridBase)cmbwarehouse).DataSource = null;
			((Control)(object)cmbItemName).Text = string.Empty;
			((UltraGridBase)cmbItemName).DataSource = null;
			((Control)(object)cmbsupplier).Text = string.Empty;
			((UltraGridBase)cmbsupplier).DataSource = null;
			((Control)(object)cmbcommagent).Text = string.Empty;
			((UltraGridBase)cmbcommagent).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBranchNameSumm_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchName).Text != string.Empty && ((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				AllcomboFillFromEvaluation();
				return;
			}
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((UltraGridBase)cmbParentcategorySumm).DataSource = null;
			((Control)(object)CmbItemClassSumm).Text = string.Empty;
			((UltraGridBase)CmbItemClassSumm).DataSource = null;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((UltraGridBase)CmbCategorySumm).DataSource = null;
			((Control)(object)cmbItemtypeSumm).Text = string.Empty;
			((UltraGridBase)cmbItemtypeSumm).DataSource = null;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((UltraGridBase)cmbItemNameSumm).DataSource = null;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			((UltraGridBase)cmbJobLotSumm).DataSource = null;
			((Control)(object)CmbWareHouseSumm).Text = string.Empty;
			((UltraGridBase)CmbWareHouseSumm).DataSource = null;
			((Control)(object)CmbCropYearSumm).Text = string.Empty;
			((UltraGridBase)CmbCropYearSumm).DataSource = null;
			((Control)(object)CmbPackingTypeSumm).Text = string.Empty;
			((UltraGridBase)CmbPackingTypeSumm).DataSource = null;
			((Control)(object)cmbDistrictSumm).Text = string.Empty;
			((UltraGridBase)cmbDistrictSumm).DataSource = null;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((UltraGridBase)cmbcitySumm).DataSource = null;
			((Control)(object)cmbSupplierSumm).Text = string.Empty;
			((UltraGridBase)cmbSupplierSumm).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (detailHistory.Rows.Count > 0 && detailHistory != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(detailHistory, e.ClickedItem.Text + ".rpt");
				return;
			}
			throw new Exception("Record not found for Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbActivity).Text == "Detail" && (int)e.Row.RowType == 0)
			{
				if (e.Row.IsChecked)
				{
					e.Row.RowStyle = new GridEXFormatStyle();
					e.Row.RowStyle.BackColor = Color.LightGreen;
					e.Row.RowStyle.ForeColor = Color.DarkGreen;
				}
				else
				{
					e.Row.RowStyle = null;
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
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Expected O, but got Unknown
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Expected O, but got Unknown
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Expected O, but got Unknown
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Expected O, but got Unknown
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Expected O, but got Unknown
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Expected O, but got Unknown
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Expected O, but got Unknown
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Expected O, but got Unknown
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Expected O, but got Unknown
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a00: Expected O, but got Unknown
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Expected O, but got Unknown
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Expected O, but got Unknown
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Expected O, but got Unknown
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Expected O, but got Unknown
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4d: Expected O, but got Unknown
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Expected O, but got Unknown
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Expected O, but got Unknown
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Expected O, but got Unknown
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a79: Expected O, but got Unknown
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Expected O, but got Unknown
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Expected O, but got Unknown
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Expected O, but got Unknown
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Expected O, but got Unknown
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Expected O, but got Unknown
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Expected O, but got Unknown
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1e: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Expected O, but got Unknown
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Expected O, but got Unknown
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Expected O, but got Unknown
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b60: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6b: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b81: Expected O, but got Unknown
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b97: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Expected O, but got Unknown
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Expected O, but got Unknown
		//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Expected O, but got Unknown
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Expected O, but got Unknown
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Expected O, but got Unknown
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bef: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfa: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1b: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c26: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Expected O, but got Unknown
		//IL_0c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c52: Expected O, but got Unknown
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5d: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Expected O, but got Unknown
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Expected O, but got Unknown
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c94: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Expected O, but got Unknown
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caa: Expected O, but got Unknown
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Expected O, but got Unknown
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc0: Expected O, but got Unknown
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccb: Expected O, but got Unknown
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd6: Expected O, but got Unknown
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce1: Expected O, but got Unknown
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cec: Expected O, but got Unknown
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Expected O, but got Unknown
		//IL_0cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Expected O, but got Unknown
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0d: Expected O, but got Unknown
		//IL_0d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d18: Expected O, but got Unknown
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Expected O, but got Unknown
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d92: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Expected O, but got Unknown
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc9: Expected O, but got Unknown
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd4: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddf: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dea: Expected O, but got Unknown
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Expected O, but got Unknown
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e84: Expected O, but got Unknown
		//IL_0e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea5: Expected O, but got Unknown
		//IL_0ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb0: Expected O, but got Unknown
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed1: Expected O, but got Unknown
		//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Expected O, but got Unknown
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efd: Expected O, but got Unknown
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f08: Expected O, but got Unknown
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Expected O, but got Unknown
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Expected O, but got Unknown
		//IL_0f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa2: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Expected O, but got Unknown
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Expected O, but got Unknown
		//IL_103d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1047: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Expected O, but got Unknown
		//IL_105e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1068: Expected O, but got Unknown
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_107e: Expected O, but got Unknown
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109f: Expected O, but got Unknown
		//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10aa: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c0: Expected O, but got Unknown
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e1: Expected O, but got Unknown
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ec: Expected O, but got Unknown
		//IL_1103: Unknown result type (might be due to invalid IL or missing references)
		//IL_110d: Expected O, but got Unknown
		//IL_110e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1118: Expected O, but got Unknown
		//IL_117c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1186: Expected O, but got Unknown
		//IL_1192: Unknown result type (might be due to invalid IL or missing references)
		//IL_119c: Expected O, but got Unknown
		//IL_1706: Unknown result type (might be due to invalid IL or missing references)
		//IL_1710: Expected O, but got Unknown
		//IL_171d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1727: Expected O, but got Unknown
		//IL_1734: Unknown result type (might be due to invalid IL or missing references)
		//IL_173e: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance345 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
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
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance330 = new Appearance();
		GridEXLayout GridSummary_DesignTimeLayout = new GridEXLayout();
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance333 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance384 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance387 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance390 = new Appearance();
		Appearance appearance391 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance394 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance396 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.PurchaseRegisterNew));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Detail = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbStockAccount = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbCustomGroup = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.cmbcommagent = new UltraCombo();
		this.CmbDocumentType = new UltraCombo();
		this.cmbsupplier = new UltraCombo();
		this.cmbBranchName = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtToDocNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.txtFromDocNo = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbActivity = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
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
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbCropYear = new UltraCombo();
		this.cmbPackingtype = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnReferesh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.Summary = new System.Windows.Forms.TabPage();
		this.GridSummary = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbStockAccountSummary = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbCustomGroupSummary = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.CmbBranchNameSumm = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.chkPackUom = new System.Windows.Forms.CheckBox();
		this.chkCity = new System.Windows.Forms.CheckBox();
		this.CmbDateTypeSumm = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.CmbCropYearSumm = new UltraCombo();
		this.CmbItemClassSumm = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
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
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcategoryCombo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbwarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingtype).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.Summary.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridSummary).BeginInit();
		this.panel9.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccountSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroupSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearSumm).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1184, 461);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.Detail);
		this.tabControl1.Controls.Add(this.Summary);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 461);
		this.tabControl1.TabIndex = 3;
		this.Detail.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.Detail.Controls.Add(this.panel2);
		this.Detail.Controls.Add(this.panel4);
		this.Detail.Controls.Add(this.panel3);
		this.Detail.Location = new System.Drawing.Point(4, 25);
		this.Detail.Name = "Detail";
		this.Detail.Padding = new System.Windows.Forms.Padding(3);
		this.Detail.Size = new System.Drawing.Size(1176, 432);
		this.Detail.TabIndex = 0;
		this.Detail.Text = "Detail Register";
		this.Detail.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.AlternatingColors = true;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		this.DataGridHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 222);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1170, 207);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.FormattingRow += new RowLoadEventHandler(DataGridHistory_FormattingRow);
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 200);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1170, 22);
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
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 21);
		this.ctrlGrdBar1.TabIndex = 9;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockAccount);
		this.panel4.Controls.Add(this.label38);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.panel4.Controls.Add(this.label37);
		this.panel4.Controls.Add(this.label36);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplier);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.txtDateFrom);
		this.panel4.Controls.Add(this.txtToDocNo);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtToDate);
		this.panel4.Controls.Add(this.txtFromDocNo);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivity);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.btnShow);
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
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingtype);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 56);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1170, 144);
		this.panel4.TabIndex = 0;
		this.CmbStockAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbStockAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Location = new System.Drawing.Point(927, 60);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Name = "CmbStockAccount";
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).TabIndex = 18;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(831, 66);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(85, 15);
		this.label38.TabIndex = 600;
		this.label38.Text = "Stock Account";
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(927, 31);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 17;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(831, 37);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(84, 15);
		this.label37.TabIndex = 600;
		this.label37.Text = "Custom Group";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(5, 64);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(48, 15);
		this.label36.TabIndex = 230;
		this.label36.Text = "To Date";
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(632, 112);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 15;
		this.CmbDocumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Location = new System.Drawing.Point(927, 4);
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Name = "CmbDocumentType";
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentType).TabIndex = 16;
		this.cmbsupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Location = new System.Drawing.Point(632, 85);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Name = "cmbsupplier";
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).TabIndex = 14;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance343).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance343).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance343).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance343).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance344).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance344;
		((AppearanceBase)appearance345).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance345).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance348).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance348;
		((AppearanceBase)appearance349).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance349).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance349;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance350).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance350).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance350).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance350).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance350).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance350;
		((AppearanceBase)appearance351).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance351;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance352).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance352).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance352;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance354;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(82, 4);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(5, 10);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(79, 15);
		this.label48.TabIndex = 229;
		this.label48.Text = "Branch Name";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 91);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(70, 15);
		this.label15.TabIndex = 189;
		this.label15.Text = "No From,To";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateFrom.Location = new System.Drawing.Point(82, 33);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(216, 22);
		this.txtDateFrom.TabIndex = 1;
		this.txtToDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNo.Location = new System.Drawing.Point(191, 87);
		this.txtToDocNo.Name = "txtToDocNo";
		this.txtToDocNo.Size = new System.Drawing.Size(107, 23);
		this.txtToDocNo.TabIndex = 4;
		this.txtToDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNo_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 37);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "From Date";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(82, 60);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(216, 22);
		this.txtToDate.TabIndex = 2;
		this.txtFromDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNo.Location = new System.Drawing.Point(82, 87);
		this.txtFromDocNo.Name = "txtFromDocNo";
		this.txtFromDocNo.Size = new System.Drawing.Size(107, 23);
		this.txtFromDocNo.TabIndex = 3;
		this.txtFromDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNo_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(555, 118);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(78, 15);
		this.label14.TabIndex = 185;
		this.label14.Text = "Comm Agent";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(831, 10);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(96, 15);
		this.label2.TabIndex = 163;
		this.label2.Text = "Transaction Type";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(831, 95);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(70, 15);
		this.label12.TabIndex = 163;
		this.label12.Text = "Report Type";
		this.cmbActivity.AllowNull = (DefaultableBoolean)1;
		this.cmbActivity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance355).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance355).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance355;
		((UltraGridBase)this.cmbActivity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance356).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance356).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance356).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance356).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance356;
		((AppearanceBase)appearance357).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance357;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance358).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance358).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance358).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance358).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance358;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance359).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance359).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance359;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance360).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance360;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance361).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance361;
		((AppearanceBase)appearance362).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance362).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance362;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance363).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance363).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance363).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance363).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance363).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance363;
		((AppearanceBase)appearance365).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance365;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance366).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance366).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance366;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance367;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbActivity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Location = new System.Drawing.Point(927, 89);
		((System.Windows.Forms.Control)(object)this.cmbActivity).Name = "cmbActivity";
		((System.Windows.Forms.Control)(object)this.cmbActivity).Size = new System.Drawing.Size(187, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivity).TabIndex = 19;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(555, 91);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(69, 15);
		this.label13.TabIndex = 183;
		this.label13.Text = "Party Name";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(1114, 89);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(51, 26);
		this.btnShow.TabIndex = 20;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbparentcategoryCombo.AllowNull = (DefaultableBoolean)1;
		this.cmbparentcategoryCombo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparentcategoryCombo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparentcategoryCombo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparentcategoryCombo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).Location = new System.Drawing.Point(391, 4);
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).Name = "cmbparentcategoryCombo";
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.cmbparentcategoryCombo).TabIndex = 6;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(555, 10);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(49, 15);
		this.label11.TabIndex = 181;
		this.label11.Text = "Job Lot ";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(302, 10);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(90, 15);
		this.label3.TabIndex = 165;
		this.label3.Text = "Parent category";
		this.cmbjoblot.AllowNull = (DefaultableBoolean)1;
		this.cmbjoblot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbjoblot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbjoblot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbjoblot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Location = new System.Drawing.Point(632, 4);
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Name = "cmbjoblot";
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbjoblot).TabIndex = 11;
		this.cmbItemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance241;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance247).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Location = new System.Drawing.Point(391, 58);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Name = "cmbItemtype";
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).TabIndex = 8;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(555, 37);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 15);
		this.label10.TabIndex = 179;
		this.label10.Text = "Warehouse";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(302, 64);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 15);
		this.label4.TabIndex = 167;
		this.label4.Text = "Item Type";
		this.cmbwarehouse.AllowNull = (DefaultableBoolean)1;
		this.cmbwarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbwarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbwarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance254;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbwarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbwarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).Location = new System.Drawing.Point(632, 31);
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).Name = "cmbwarehouse";
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbwarehouse).TabIndex = 12;
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance267;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance273).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(391, 31);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 7;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(555, 64);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 177;
		this.label9.Text = "Item Name";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(302, 37);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(82, 15);
		this.label5.TabIndex = 169;
		this.label5.Text = "Item Category";
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance281).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance281;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance286).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance287).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(632, 58);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 13;
		this.cmbItemClass.AllowNull = (DefaultableBoolean)1;
		this.cmbItemClass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemClass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemClass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance293).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance293).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance294).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance294;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance295).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance295).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance295).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance296;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance299).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance300).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance300).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance300).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance300).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance302).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance302;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemClass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemClass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Location = new System.Drawing.Point(82, 112);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Name = "cmbItemClass";
		((System.Windows.Forms.Control)(object)this.cmbItemClass).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemClass).TabIndex = 5;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(302, 91);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(57, 15);
		this.label8.TabIndex = 175;
		this.label8.Text = "Crop Year";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 118);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(61, 15);
		this.label6.TabIndex = 171;
		this.label6.Text = "Item Class";
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance306).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance306).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance307).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance307;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance308).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance308).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance308).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance309).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance309;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance313).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance313).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance314).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance314).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance314).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance315).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance316).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(391, 85);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 9;
		this.cmbPackingtype.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance319).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance319).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance319).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance319;
		((AppearanceBase)appearance320).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance320;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance321).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance321).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance321).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance322).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance322;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance324;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance325;
		((AppearanceBase)appearance326).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance326).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance326;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance327).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance327).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance327).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance327).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance327;
		((AppearanceBase)appearance328).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance330;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Location = new System.Drawing.Point(391, 112);
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Name = "cmbPackingtype";
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).TabIndex = 10;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(302, 118);
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
		this.panel3.Size = new System.Drawing.Size(1170, 53);
		this.panel3.TabIndex = 0;
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 30);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(221, 19);
		this.label17.TabIndex = 190;
		this.label17.Text = "Purchase Invoice Register";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.BtnReferesh, this.btnPrint, this.btnShortcutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1170, 27);
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
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(70, 24);
		this.btnPrint.Text = "Print";
		this.btnPrint.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(btnPrint_DropDownItemClicked);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click_1);
		this.Summary.Controls.Add((System.Windows.Forms.Control)(object)this.GridSummary);
		this.Summary.Controls.Add(this.panel9);
		this.Summary.Controls.Add(this.panel7);
		this.Summary.Controls.Add(this.panel8);
		this.Summary.Location = new System.Drawing.Point(4, 25);
		this.Summary.Name = "Summary";
		this.Summary.Padding = new System.Windows.Forms.Padding(3);
		this.Summary.Size = new System.Drawing.Size(1176, 432);
		this.Summary.TabIndex = 1;
		this.Summary.Text = "Summary Register";
		this.Summary.UseVisualStyleBackColor = true;
		this.GridSummary.AllowEdit = (InheritableBoolean)2;
		this.GridSummary.AlternatingColors = true;
		((ScrollableBase)this.GridSummary).BackColor = System.Drawing.Color.White;
		this.GridSummary.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridSummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridSummary.DesignTimeLayout = GridSummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridSummary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridSummary.FilterMode = (FilterMode)2;
		this.GridSummary.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridSummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridSummary.GroupTotals = (GroupTotals)2;
		this.GridSummary.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridSummary).Location = new System.Drawing.Point(3, 223);
		((System.Windows.Forms.Control)(object)this.GridSummary).Name = "GridSummary";
		((ScrollableBase)this.GridSummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridSummary).Size = new System.Drawing.Size(1170, 206);
		((System.Windows.Forms.Control)(object)this.GridSummary).TabIndex = 0;
		this.GridSummary.TotalRow = (InheritableBoolean)1;
		this.GridSummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridSummary).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 198);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1170, 25);
		this.panel9.TabIndex = 11;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1123, 0);
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
		this.panel7.Size = new System.Drawing.Size(1170, 165);
		this.panel7.TabIndex = 9;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockAccountSummary);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroupSummary);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchNameSumm);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.chkPackUom);
		this.groupBox2.Controls.Add(this.chkCity);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemClassSumm);
		this.groupBox2.Controls.Add(this.label28);
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
		this.groupBox2.Size = new System.Drawing.Size(1161, 157);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Criteria's";
		this.CmbStockAccountSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbStockAccountSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockAccountSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockAccountSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance268).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockAccountSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockAccountSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockAccountSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockAccountSummary).Location = new System.Drawing.Point(933, 99);
		((System.Windows.Forms.Control)(object)this.CmbStockAccountSummary).Name = "CmbStockAccountSummary";
		((System.Windows.Forms.Control)(object)this.CmbStockAccountSummary).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockAccountSummary).TabIndex = 20;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(856, 105);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(60, 15);
		this.label34.TabIndex = 602;
		this.label34.Text = "Stock A/C";
		this.CmbCustomGroupSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroupSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroupSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroupSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroupSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomGroupSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupSummary).Location = new System.Drawing.Point(933, 45);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupSummary).Name = "CmbCustomGroupSummary";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupSummary).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroupSummary).TabIndex = 18;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(856, 43);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(48, 30);
		this.label35.TabIndex = 598;
		this.label35.Text = "Custom\r\nGroup";
		this.CmbBranchNameSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance368).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance368;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance369).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance369).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance369).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance369).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance369;
		((AppearanceBase)appearance370).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance370;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance371).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance371).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance371).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance371).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance371;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance372).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance372).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance372;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance373).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance373;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance374).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance374;
		((AppearanceBase)appearance376).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance376).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance376;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance377).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance377).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance377).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance377).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance377).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance377;
		((AppearanceBase)appearance378).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance378;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance379).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance379).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance379;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance380).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance380;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSumm).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSumm).Location = new System.Drawing.Point(93, 18);
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSumm).Name = "CmbBranchNameSumm";
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSumm).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSumm).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBranchNameSumm).Leave += new System.EventHandler(CmbBranchNameSumm_Leave);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(2, 24);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(79, 15);
		this.label16.TabIndex = 596;
		this.label16.Text = "Branch Name";
		this.chkPackUom.AutoSize = true;
		this.chkPackUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPackUom.Location = new System.Drawing.Point(1007, 130);
		this.chkPackUom.Name = "chkPackUom";
		this.chkPackUom.Size = new System.Drawing.Size(80, 19);
		this.chkPackUom.TabIndex = 22;
		this.chkPackUom.Text = "Pack Uom";
		this.chkPackUom.UseVisualStyleBackColor = true;
		this.chkCity.AutoSize = true;
		this.chkCity.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCity.Location = new System.Drawing.Point(935, 130);
		this.chkCity.Name = "chkCity";
		this.chkCity.Size = new System.Drawing.Size(75, 19);
		this.chkCity.TabIndex = 21;
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
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateTypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Location = new System.Drawing.Point(93, 45);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Name = "CmbDateTypeSumm";
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).TabIndex = 1;
		this.CmbDateTypeSumm.ValueChanged += new System.EventHandler(CmbDateTypeSumm_ValueChanged);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(2, 51);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(60, 15);
		this.label40.TabIndex = 594;
		this.label40.Text = "Date Type";
		this.CmbCropYearSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance312).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance312).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance312).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance323).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance323;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance332).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance332).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance332).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance333).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance333;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance334).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance335;
		((AppearanceBase)appearance336).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance336).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance337).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance337).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance337).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance337).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance338).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance338;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance339).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance340;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearSumm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYearSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearSumm).Location = new System.Drawing.Point(653, 45);
		((System.Windows.Forms.Control)(object)this.CmbCropYearSumm).Name = "CmbCropYearSumm";
		((System.Windows.Forms.Control)(object)this.CmbCropYearSumm).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearSumm).TabIndex = 13;
		this.CmbItemClassSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbItemClassSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemClassSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemClassSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance331).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance331).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance331).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance331).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance342).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance342;
		((AppearanceBase)appearance353).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance353).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance353;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance364).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance364;
		((AppearanceBase)appearance375).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance375).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance375;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance386).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance386).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance386).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance386).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance386).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemClassSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance386;
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
		((System.Windows.Forms.Control)(object)this.CmbItemClassSumm).Location = new System.Drawing.Point(392, 18);
		((System.Windows.Forms.Control)(object)this.CmbItemClassSumm).Name = "CmbItemClassSumm";
		((System.Windows.Forms.Control)(object)this.CmbItemClassSumm).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemClassSumm).TabIndex = 7;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(573, 51);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(57, 15);
		this.label28.TabIndex = 162;
		this.label28.Text = "Crop Year";
		this.cmbItemtypeSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemtypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Location = new System.Drawing.Point(392, 72);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Name = "cmbItemtypeSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).TabIndex = 9;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label26.Location = new System.Drawing.Point(324, 24);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(61, 15);
		this.label26.TabIndex = 163;
		this.label26.Text = "Item Class";
		this.CmbPackingTypeSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingTypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingTypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingTypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance381).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance381;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance382).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance382).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance382).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance382).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance382;
		((AppearanceBase)appearance383).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance383;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance384).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance384).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance384).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance384;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance385).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance385).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance385;
		((AppearanceBase)appearance387).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance387).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance387;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance388;
		((AppearanceBase)appearance389).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance389).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance389;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance390).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance390).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance390).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance390).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance390).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance390;
		((AppearanceBase)appearance391).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance391;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance392).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance392).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance392;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance393).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance393;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingTypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingTypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).Location = new System.Drawing.Point(653, 72);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).Name = "CmbPackingTypeSumm";
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeSumm).TabIndex = 14;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(856, 24);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(51, 15);
		this.label19.TabIndex = 123;
		this.label19.Text = "Supplier";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(573, 78);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(77, 15);
		this.label20.TabIndex = 158;
		this.label20.Text = "Packing Type";
		this.cmbSupplierSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).Location = new System.Drawing.Point(933, 18);
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).Name = "cmbSupplierSumm";
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierSumm).TabIndex = 17;
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
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Location = new System.Drawing.Point(653, 99);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Name = "cmbDistrictSumm";
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).TabIndex = 15;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(573, 132);
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
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Location = new System.Drawing.Point(653, 126);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Name = "cmbcitySumm";
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).TabIndex = 16;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(324, 105);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(35, 15);
		this.label21.TabIndex = 125;
		this.label21.Text = "Item ";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(573, 105);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(45, 15);
		this.label31.TabIndex = 151;
		this.label31.Text = "District";
		this.cmbReportTypeSummary.AllowNull = (DefaultableBoolean)1;
		this.cmbReportTypeSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportTypeSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportTypeSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportTypeSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReportTypeSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).Location = new System.Drawing.Point(933, 72);
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).Name = "cmbReportTypeSummary";
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportTypeSummary).TabIndex = 19;
		this.cmbItemNameSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemNameSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Location = new System.Drawing.Point(392, 99);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Name = "cmbItemNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).TabIndex = 10;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(856, 78);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(70, 15);
		this.label22.TabIndex = 155;
		this.label22.Text = "Report Type";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(2, 78);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(91, 15);
		this.label29.TabIndex = 127;
		this.label29.Text = "From && To Date";
		this.CmbWareHouseSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance394).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance394;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance395).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance395).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance395).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance395).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance395;
		((AppearanceBase)appearance396).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance396;
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
		((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm).Location = new System.Drawing.Point(653, 18);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm).Name = "CmbWareHouseSumm";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseSumm).TabIndex = 12;
		this.cmbJobLotSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance341).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance346).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance346;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance347;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLotSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Location = new System.Drawing.Point(392, 126);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Name = "cmbJobLotSumm";
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).TabIndex = 11;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(573, 24);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(69, 15);
		this.label25.TabIndex = 153;
		this.label25.Text = "WareHouse";
		this.txtFromDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtFromDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateSumm.Location = new System.Drawing.Point(93, 74);
		this.txtFromDateSumm.Name = "txtFromDateSumm";
		this.txtFromDateSumm.Size = new System.Drawing.Size(112, 23);
		this.txtFromDateSumm.TabIndex = 2;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(324, 132);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(46, 15);
		this.label32.TabIndex = 153;
		this.label32.Text = "Job Lot";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(2, 105);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(82, 15);
		this.label23.TabIndex = 129;
		this.label23.Text = "From && To No";
		this.txtToDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtToDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateSumm.Location = new System.Drawing.Point(206, 74);
		this.txtToDateSumm.Name = "txtToDateSumm";
		this.txtToDateSumm.Size = new System.Drawing.Size(112, 23);
		this.txtToDateSumm.TabIndex = 3;
		this.txtFromDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoSumm.Location = new System.Drawing.Point(93, 101);
		this.txtFromDocNoSumm.Name = "txtFromDocNoSumm";
		this.txtFromDocNoSumm.Size = new System.Drawing.Size(112, 23);
		this.txtFromDocNoSumm.TabIndex = 4;
		this.txtToDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoSumm.Location = new System.Drawing.Point(206, 101);
		this.txtToDocNoSumm.Name = "txtToDocNoSumm";
		this.txtToDocNoSumm.Size = new System.Drawing.Size(112, 23);
		this.txtToDocNoSumm.TabIndex = 5;
		this.BtnShowSumm.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowSumm.FlatAppearance.BorderSize = 0;
		this.BtnShowSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowSumm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowSumm.ForeColor = System.Drawing.Color.White;
		this.BtnShowSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowSumm.Location = new System.Drawing.Point(1098, 126);
		this.BtnShowSumm.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowSumm.Name = "BtnShowSumm";
		this.BtnShowSumm.Size = new System.Drawing.Size(55, 26);
		this.BtnShowSumm.TabIndex = 23;
		this.BtnShowSumm.Text = "Show";
		this.BtnShowSumm.UseVisualStyleBackColor = false;
		this.BtnShowSumm.Click += new System.EventHandler(BtnShowSumm_Click);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(2, 132);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(91, 15);
		this.label24.TabIndex = 138;
		this.label24.Text = "Parent Category";
		this.cmbParentcategorySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbParentcategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentcategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentcategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Location = new System.Drawing.Point(93, 126);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Name = "cmbParentcategorySumm";
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).TabIndex = 6;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(324, 51);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(54, 15);
		this.label27.TabIndex = 138;
		this.label27.Text = "Category";
		this.CmbCategorySumm.AllowNull = (DefaultableBoolean)1;
		this.CmbCategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Location = new System.Drawing.Point(392, 45);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Name = "CmbCategorySumm";
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).TabIndex = 8;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(324, 78);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(32, 15);
		this.label33.TabIndex = 138;
		this.label33.Text = "Type";
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1170, 30);
		this.panel8.TabIndex = 10;
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNewSummary, this.btnRefreshSummary, this.btnPrintSumm, this.BtnShortCutKeysSumm });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1170, 30);
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
		base.ClientSize = new System.Drawing.Size(1184, 461);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "PurchaseRegisterNew";
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
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcategoryCombo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbwarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemClass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccountSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroupSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearSumm).EndInit();
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

}
