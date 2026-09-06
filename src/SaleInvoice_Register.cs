// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Salt.Reports
{

public class SaleInvoice_Register : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtInvoice = new DataTable();

	private DataTable dtSalesRegister = new DataTable();

	private DataTable dtSalesSummaryByItem = new DataTable();

	private DataTable dtSalesSummaryByItemandCity = new DataTable();

	private DataTable dtSalesSummaryByItemandPackSize = new DataTable();

	private DataTable dtSalesSummaryByItemNWarehouse = new DataTable();

	private DataTable dtSalesSummaryByItemNPackSizeCity = new DataTable();

	private DataTable dtSalesSummaryByCustomer = new DataTable();

	private DataTable dtSalesSummaryByCustomerandItem = new DataTable();

	private DataTable dtSalesSummaryByCustomerandCity = new DataTable();

	private DataTable dtSalesSummaryByCustomerItemandCity = new DataTable();

	private DataTable dtSalesSummaryByCustomerandPackSize = new DataTable();

	private DataTable dtSalesSummaryByParentCategory = new DataTable();

	private DataTable dtSalesSummaryByItemAndParentCategory = new DataTable();

	private DataTable dtSalesSummaryByCustomerAndParentCategory = new DataTable();

	private DataTable dtSummary = new DataTable();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Label label23;

	internal UltraCombo cmbitem;

	private Label label6;

	private TextBox txtInvoiceNoto;

	private DateTimePicker GRNToDate;

	private DateTimePicker GRNfromdate;

	private Label label27;

	private Label label33;

	private TextBox txtInvoiceNoFrom;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Label label1;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private ToolStripDropDownButton tsDropDown;

	private TextBox txtsaleorderto;

	private TextBox txtsaleorderfrom;

	private Label label9;

	private TextBox txtrateto;

	private TextBox txtratefrom;

	private Label label5;

	private Label label10;

	internal UltraCombo cmbWareHouse;

	private Label label11;

	internal UltraCombo CmbJobLot;

	private Label label12;

	internal UltraCombo cmbDateType;

	private GroupBox groupBox1;

	private Label label13;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private Panel panel5;

	private Panel panel3;

	private Panel panel4;

	private Panel panel6;

	private Panel panel7;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNewSummary;

	private ToolStripButton btnRefreshSummary;

	private ToolStripButton btnPrint;

	private ToolStripButton btnShortcutKeysSummary;

	private Panel panel8;

	private Label lblCaption;

	private Panel panel9;

	private GroupBox groupBox2;

	private Label label19;

	internal UltraCombo cmbDateTypeSummary;

	internal UltraCombo CmbDistrictSummary;

	private DateTimePicker txtDateFromSummary;

	private Label label14;

	private Label label17;

	internal UltraCombo cmbCitySummary;

	private Label label3;

	private Label label4;

	internal UltraCombo cmbItemclassSummary;

	private Label label16;

	private Button btnShowSumamry;

	private DateTimePicker txtToDateSummary;

	internal UltraCombo cmbActivitySummary;

	internal UltraCombo CmbItemSummary;

	private Label label7;

	internal UltraCombo cmbItemTypeSummary;

	private Label label8;

	private Label label20;

	internal UltraCombo cmbUOM;

	private Label label21;

	private Label label22;

	private TextBox txtFromDocNoSummary;

	private TextBox txtToDocNoSummary;

	internal UltraCombo CmbWarehouseSummary;

	private Label label24;

	private Label label25;

	internal UltraCombo CmbPartySummary;

	internal UltraCombo cmbItemCatgorySummary;

	private Label label26;

	private Label label28;

	internal UltraCombo CmbJobLotSummary;

	internal UltraCombo CmbParentCategorySummary;

	private Label label29;

	private Panel panel10;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel11;

	internal GridEX grdSummary;

	public SaleInvoice_Register(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleInvoiceRegister_Load(object sender, EventArgs e)
	{
		try
		{
			ReportsLoad();
			Datetypefill();
			AllComboBind();
			cmbDateType.Focus();
			gridHisory();
			dtSalesRegister.Columns.Add("DocDate", typeof(DateTime));
			dtSalesRegister.Columns.Add("DocNo", typeof(int));
			dtSalesRegister.Columns.Add("Id", typeof(int));
			dtSalesRegister.Columns.Add("DocumentTypeId", typeof(int));
			dtSalesRegister.Columns.Add("DocType", typeof(string));
			dtSalesRegister.Columns.Add("PartyName", typeof(string));
			dtSalesRegister.Columns.Add("ItemCode", typeof(int));
			dtSalesRegister.Columns.Add("ItemName", typeof(string));
			dtSalesRegister.Columns.Add("PackUom", typeof(string));
			dtSalesRegister.Columns.Add("JobLot", typeof(string));
			dtSalesRegister.Columns.Add("PackingType", typeof(string));
			dtSalesRegister.Columns.Add("VehicleNo", typeof(string));
			dtSalesRegister.Columns.Add("ItemQty", typeof(double));
			dtSalesRegister.Columns.Add("BillWeight", typeof(double));
			dtSalesRegister.Columns.Add("Amount", typeof(double));
			dtSalesRegister.Columns.Add("AvgRate", typeof(double));
			dtSalesRegister.Columns.Add("PrctByAmount", typeof(double));
			dtSalesRegister.Columns.Add("PrctByWeight", typeof(double));
			dtSalesRegister.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByItem.Columns.Add("ItemCode", typeof(int));
			dtSalesSummaryByItem.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItem.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItem.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItem.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItem.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItem.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByItemandCity.Columns.Add("ItemCode", typeof(int));
			dtSalesSummaryByItemandCity.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItemandCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByItemandPackSize.Columns.Add("ItemCode", typeof(int));
			dtSalesSummaryByItemandPackSize.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItemandPackSize.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByItemandPackSize.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByItemNWarehouse.Columns.Add("WarehouseName", typeof(string));
			dtSalesSummaryByItemNWarehouse.Columns.Add("ItemCode", typeof(int));
			dtSalesSummaryByItemNWarehouse.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItemNWarehouse.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("ItemCode", typeof(int));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCustomer.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomer.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCustomerandItem.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerandItem.Columns.Add("ItemCode", typeof(int));
			dtSalesSummaryByCustomerandItem.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByCustomerandItem.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByCustomerandItem.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCustomerandCity.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerandCity.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByCustomerandCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("ItemCode", typeof(int));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByParentCategory.Columns.Add("ParentCategory", typeof(string));
			dtSalesSummaryByParentCategory.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByParentCategory.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByParentCategory.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByParentCategory.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByParentCategory.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByParentCategory.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemAndParentCategory.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItemAndParentCategory.Columns.Add("ParentCategory", typeof(string));
			dtSalesSummaryByItemAndParentCategory.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItemAndParentCategory.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItemAndParentCategory.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItemAndParentCategory.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemAndParentCategory.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItemAndParentCategory.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("CustomerName", typeof(string));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("ParentCategory", typeof(string));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("PrctByWeight", typeof(double));
			AllComboBindSummary();
			ActivityFill();
			ItemUOMFill();
			((UltraGridBase)cmbActivitySummary).Rows[0].Activate();
			PrintButtonManage();
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
			if (((UltraGridBase)cmbActivitySummary).ActiveRow != null)
			{
				if (((Control)(object)cmbActivitySummary).Text == "Sales Register")
				{
					btnPrint.Text = "1809_01_SalesRegisterSummary";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item")
				{
					btnPrint.Text = "1809_05-SalesRegisterSummaryByItemWithoutPacking";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item & City")
				{
					btnPrint.Text = "1809_15-SalesSummaryByItem&City";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item & Pack Size")
				{
					btnPrint.Text = "1809_03-SalesRegisterSummaryByItem";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item & Warehouse")
				{
					btnPrint.Text = "1809_06-SalesRegisterSummaryByWarehouse";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item,Pack Size & City")
				{
					btnPrint.Text = "1809_14-SalesSummaryByItemPackSize&City";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer")
				{
					btnPrint.Text = "1809_02-SalesRegisterSummaryByCustomer";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer & Item")
				{
					btnPrint.Text = "1809_04-SalesRegisterSummaryByCustomer&Item";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer & City")
				{
					btnPrint.Text = "1809_07-SalesSummaryByCustomer&City";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer,Item & City")
				{
					btnPrint.Text = "1809_09-SalesSummaryByCustomerItem&City";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer & Pack Size")
				{
					btnPrint.Text = "1809_10-SalesSummaryByCustomer&PackSize";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Parent Category")
				{
					btnPrint.Text = "1809_11-SalesSummaryByParentCategory";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Parent Category & Item")
				{
					btnPrint.Text = "1809_12-SalesSummaryByParentCategory&Item";
				}
				else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Parent Category & Customer")
				{
					btnPrint.Text = "1809_13-SalesSummaryByParentCategory&Customer";
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

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateType, "Id", "Parameters", "Date Type", ZeroIndex: false);
			DDL.BindDDL(dt, cmbDateTypeSummary, "Id", "Parameters", "Date Type", ZeroIndex: false);
			((UltraGridBase)cmbDateType).Rows[2].Activate();
			((UltraGridBase)cmbDateTypeSummary).Rows[2].Activate();
			((UltraGridBase)cmbDateType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbDateTypeSummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
		//IL_0059: Expected O, but got Unknown
		try
		{
			DataTable dt = InvSaleInvoice.AllComboBindAgainstSaleInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = "1809"
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtsup = new DataTable();
			dtsup.Columns.Add("Id", typeof(int));
			dtsup.Columns.Add("SupplierName");
			DataTable dtWh = new DataTable();
			dtWh.Columns.Add("Id", typeof(int));
			dtWh.Columns.Add("Warehouse");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			DataTable dtJob = new DataTable();
			dtJob.Columns.Add("Id", typeof(int));
			dtJob.Columns.Add("JobLot");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtsup.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "WareHouse")
				{
					dtWh.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemName")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "JobLot")
				{
					dtJob.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtsup, CmbSupplier, "Id", "SupplierName", "Supplier Name", ZeroIndex: false);
			DDL.BindDDL(dtWh, cmbWareHouse, "Id", "Warehouse", "Warehouse", ZeroIndex: false);
			DDL.BindDDL(dtItem, cmbitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtJob, CmbJobLot, "Id", "JobLot", "Job Lot", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllComboBindSummary()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			DataTable dt = InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				ReportType = "Sale"
			});
			if (dt.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetParentCategory"
					select r;
				DataTable dtParent = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsType = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetItemType"
					select r;
				DataTable dtItemType = (rowsType.Any() ? rowsType.CopyToDataTable() : dt.Clone());
				_ = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetItemClass"
					select r;
				DataTable dtClass = (rowsType.Any() ? rowsType.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsCategory = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetItemCategory"
					select r;
				DataTable dtCategory = (rowsCategory.Any() ? rowsCategory.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsCustomer = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetSupplierCustomer"
					select r;
				DataTable dtSupplierCustomer = (rowsCustomer.Any() ? rowsCustomer.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsItem = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetItems"
					select r;
				DataTable dtItem = (rowsItem.Any() ? rowsItem.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsCity = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetCity"
					select r;
				DataTable dtCity = (rowsCity.Any() ? rowsCity.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsJob = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetJobLot"
					select r;
				DataTable dtJobLot = (rowsJob.Any() ? rowsJob.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsWarehouse = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetWarehouse"
					select r;
				DataTable dtWareHouse = (rowsWarehouse.Any() ? rowsWarehouse.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsDistrict = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetDistrict"
					select r;
				DataTable dtDistrict = (rowsDistrict.Any() ? rowsDistrict.CopyToDataTable() : dt.Clone());
				DDL.BindDDLNew(dtParent, CmbParentCategorySummary, "Id", "RefName", "Parent Category", false);
				((UltraGridBase)CmbParentCategorySummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtItemType, cmbItemTypeSummary, "Id", "RefName", "Item Type", false);
				((UltraGridBase)cmbItemTypeSummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtClass, cmbItemclassSummary, "Id", "RefName", "Class Group Name", false);
				((UltraGridBase)cmbItemclassSummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtCategory, cmbItemCatgorySummary, "Id", "RefName", "Item Category", false);
				((UltraGridBase)cmbItemCatgorySummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtSupplierCustomer, CmbPartySummary, "Id", "RefName", "Party Name", false);
				DDL.BindDDLNew(dtItem, CmbItemSummary, "Id", "RefName", "Item Name", false);
				((UltraGridBase)CmbItemSummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtCity, cmbCitySummary, "Id", "RefName", "CityName", false);
				DDL.BindDDLNew(dtJobLot, CmbJobLotSummary, "Id", "RefName", "JobLot", false);
				((UltraGridBase)CmbJobLotSummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtWareHouse, cmbWareHouse, "Id", "RefName", "WareHouse", false);
				((UltraGridBase)cmbWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtDistrict, CmbDistrictSummary, "Id", "RefName", "District", false);
				((UltraGridBase)CmbDistrictSummary).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "1KG");
			dt.Rows.Add(5, "5KG");
			dt.Rows.Add(10, "10KG");
			dt.Rows.Add(20, "20KG");
			dt.Rows.Add(25, "25KG");
			dt.Rows.Add(40, "40KG");
			dt.Rows.Add(50, "50KG");
			dt.Rows.Add(60, "60KG");
			dt.Rows.Add(65, "65KG");
			dt.Rows.Add(80, "80KG");
			dt.Rows.Add(100, "100KG");
			DDL.BindDDL(dt, cmbUOM, "Id", "UOM", "Pack UOM", ZeroIndex: false);
			((UltraGridBase)cmbUOM).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			dt.Rows.Add(1, "Sales Register");
			dt.Rows.Add(2, "Sales Summary By Item");
			dt.Rows.Add(3, "Sales Summary By Item & City");
			dt.Rows.Add(4, "Sales Summary By Item & Pack Size");
			dt.Rows.Add(5, "Sales Summary By Item & Warehouse");
			dt.Rows.Add(6, "Sales Summary By Item,Pack Size & City");
			dt.Rows.Add(7, "Sales Summary By Customer");
			dt.Rows.Add(8, "Sales Summary By Customer & Item");
			dt.Rows.Add(9, "Sales Summary By Customer & City");
			dt.Rows.Add(10, "Sales Summary By Customer,Item & City");
			dt.Rows.Add(11, "Sales Summary By Customer & Pack Size");
			dt.Rows.Add(12, "Sales Summary By Parent Category");
			dt.Rows.Add(13, "Sales Summary By Parent Category & Item");
			dt.Rows.Add(14, "Sales Summary By Parent Category & Customer");
			DDL.BindDDL(dt, cmbActivitySummary, "Id", "Name", "Activity", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHisory()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtInvoice.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = GRNfromdate.Value;
			obj.ToDate = GRNToDate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtInvoiceNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtInvoiceNoto.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbitem.Value);
			obj.BillNoFrom = Conversion.ToInt((object)txtsaleorderfrom.Text.Trim());
			obj.BillNoTo = Conversion.ToInt((object)txtsaleorderto.Text.Trim());
			obj.RateFrom = Conversion.ToDouble((object)txtratefrom.Text.Trim());
			obj.RateTo = Conversion.ToDouble((object)txtrateto.Text.Trim());
			obj.WarehouseId = Conversion.ToInt(cmbWareHouse.Value);
			obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			dtInvoice = InvSaleInvoiceReports.InvSaleInvoice_RegisterSalt(obj);
			if (dtInvoice.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("InvoiceDate", typeof(DateTime));
				dt.Columns.Add("SuppCustId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("ManualBillNo", typeof(string));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("OrderNo", typeof(string));
				dt.Columns.Add("OrderDate", typeof(DateTime));
				dt.Columns.Add("WareHouse", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("SaleGLAC", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("PackUom", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("AdLsWeight", typeof(double));
				dt.Columns.Add("StockWeight", typeof(double));
				dt.Columns.Add("NetBillWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUOM", typeof(string));
				dt.Columns.Add("RateCut", typeof(double));
				dt.Columns.Add("RateCutAmount", typeof(double));
				dt.Columns.Add("NetRate", typeof(double));
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("FreightAmount", typeof(double));
				dt.Columns.Add("ExpenseAmount", typeof(double));
				dt.Columns.Add("JournalAmount", typeof(double));
				dt.Columns.Add("CommissionAmount", typeof(double));
				dt.Columns.Add("GpNo", typeof(string));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("ItemNetAmount", typeof(double));
				dt.Columns.Add("PartyBillAmount", typeof(double));
				dt.Clear();
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtInvoice.Rows[i]["Id"]), Conversion.ToInt(dtInvoice.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dtInvoice.Rows[i]["VoucherHeadId"]), Conversion.ToString(dtInvoice.Rows[i]["InvoiceNo"]), Conversion.ToDateTime(dtInvoice.Rows[i]["InvoiceDate"]).ToShortDateString(), Conversion.ToInt(dtInvoice.Rows[i]["SupplierCustomerId"]), Conversion.ToString(dtInvoice.Rows[i]["CustomerName"]), Conversion.ToString(dtInvoice.Rows[i]["PaymentTerm"]), Conversion.ToInt(dtInvoice.Rows[i]["DueDays"]), Conversion.ToDateTime(dtInvoice.Rows[i]["DueDate"]), Conversion.ToString(dtInvoice.Rows[i]["ManualBillNo"]), Conversion.ToString(dtInvoice.Rows[i]["CompanyNamecmagnt"]), Conversion.ToString(dtInvoice.Rows[i]["OrderNo"]), Conversion.ToDateTime(dtInvoice.Rows[i]["OrderDate"]), Conversion.ToString(dtInvoice.Rows[i]["WareHouseName"]), Conversion.ToInt(dtInvoice.Rows[i]["ItemId"]), Conversion.ToInt(dtInvoice.Rows[i]["SaleGLAC"]), Conversion.ToString(dtInvoice.Rows[i]["ItemName"]), Conversion.ToString(dtInvoice.Rows[i]["JobLotDescription"]), Conversion.ToString(dtInvoice.Rows[i]["PackTypeDesc"]), Conversion.ToString(dtInvoice.Rows[i]["PackUom"]), Conversion.ToDouble(dtInvoice.Rows[i]["ItemQty"]), Conversion.ToDouble(dtInvoice.Rows[i]["GrossWeight"]), Conversion.ToDouble(dtInvoice.Rows[i]["AdLsWeight"]), Conversion.ToDouble(dtInvoice.Rows[i]["StockWeight"]), Conversion.ToDouble(dtInvoice.Rows[i]["NetBillWeight"]), Conversion.ToDouble(dtInvoice.Rows[i]["ItemRate"]), Conversion.ToString(dtInvoice.Rows[i]["RateUOM"]), Conversion.ToDouble(dtInvoice.Rows[i]["RateCut"]), Conversion.ToDouble(dtInvoice.Rows[i]["RateCutAmount"]), Conversion.ToDouble(dtInvoice.Rows[i]["NetRate"]), Conversion.ToDouble(dtInvoice.Rows[i]["ItemAmount"]), Conversion.ToDouble(dtInvoice.Rows[i]["FreightAmount"]), Conversion.ToDouble(dtInvoice.Rows[i]["ExpenseAmount"]), Conversion.ToDouble(dtInvoice.Rows[i]["JournalAmount"]), Conversion.ToDouble(dtInvoice.Rows[i]["CommissionAmount"]), Conversion.ToString(dtInvoice.Rows[i]["GpNo"]), Conversion.ToDateTime(dtInvoice.Rows[i]["GpDate"]).ToShortDateString(), Conversion.ToString(dtInvoice.Rows[i]["VehicleNo"]), Conversion.ToDouble(dtInvoice.Rows[i]["ItemNetAmount"]), Conversion.ToDouble(dtInvoice.Rows[i]["PartyBillAmount"]));
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GridSettings();
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			gridHisory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["SaleGLAC"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["SuppCustId"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["InvoiceDate"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["StockWeight"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["AdLsWeight"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExpenseAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["ExpenseAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommissionAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["FreightAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["JournalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["JournalAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["JournalAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["JournalAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["PartyBillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PartyBillAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 60;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportsLoad()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("Sales_Salt");
			if (files != null && files.Length != 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					tsDropDown.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[i]));
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsDropDown_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtInvoice.Rows.Count == 0 || dtInvoice == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtInvoice, e.ClickedItem.Text + ".rpt");
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
			txtInvoiceNoFrom.Text = string.Empty;
			txtInvoiceNoto.Text = string.Empty;
			txtratefrom.Text = string.Empty;
			txtrateto.Text = string.Empty;
			txtsaleorderfrom.Text = string.Empty;
			txtsaleorderto.Text = string.Empty;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)cmbWareHouse).Text = string.Empty;
			((Control)(object)cmbitem).Text = string.Empty;
			Datetypefill();
			cmbDateType.Focus();
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
			AllComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleInvoiceHistory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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

	private void frmSaleInvoiceHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbDateType.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbDateType.Focus();
				}
				if (e.Control && e.Alt)
				{
					MakeShortCutKeys();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewSummary_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshSummary_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowSumamry_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbDateTypeSummary.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdSummary).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbDateTypeSummary.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.Alt)
				{
					MakeShortCutKeysForSumamryTab();
				}
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl + P", "For print");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Filter");
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

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				GridEXRow currentRow = DataGridHistory.CurrentRow;
				int getId = Conversion.ToInt(currentRow.Cells["Id"].Value);
				int SupCustId = Conversion.ToInt(currentRow.Cells["SuppCustId"].Value);
				DateTime Date = Conversion.ToDateTime(currentRow.Cells["InvoiceDate"].Value);
				CommonServices.SaleInvoicetSlip_301(getId, SupCustId, 0, Date, Date);
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value), Conversion.ToInt(DataGridHistory.CurrentRow.Cells["DocumentTypeId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (e.Column.Key == "InvoiceNo")
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				new SaleInvoiceReverseActivity(UserAccount, id).Show();
			}
			if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SuppCustId"].Value), GRNfromdate.Value, GRNToDate.Value);
			}
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), GRNfromdate.Value, GRNToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				int id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				Conversion.ToInt((object)item.Cells["DocumentTypeId"].Value.ToString());
				Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				int ItemId = Conversion.ToInt(item.Cells["OrderItemId"].Value);
				int supid = Conversion.ToInt(item.Cells["SuppCustId"].Value);
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					DateTime Date = Conversion.ToDateTime(item.Cells["InvoiceDate"].Value);
					CommonServices.SaleInvoicetSlip_301(id, supid, 0, Date, Date);
				}
				if (DataGridHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value), Conversion.ToInt(DataGridHistory.CurrentRow.Cells["DocumentTypeId"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "InvoiceNo")
				{
					new SaleInvoiceReverseActivity(UserAccount, id).Show();
				}
				if (DataGridHistory.CurrentColumn.Key == "CustomerName")
				{
					CommonServices.GoToCustomerLedgerFromLinkedEvent(id, GRNfromdate.Value, GRNToDate.Value);
				}
				if (DataGridHistory.CurrentColumn.Key == "ItemName")
				{
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(ItemId, GRNfromdate.Value, GRNToDate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtratefrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtrateto_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtInvoiceNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtInvoiceNoto_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsaleorderfrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsaleorderto_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateType.Value) == 1)
			{
				GRNfromdate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				GRNfromdate.Value = DateTime.Today.AddDays(-7.0);
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				GRNfromdate.Value = first;
				GRNToDate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				GRNfromdate.Value = first2;
				GRNToDate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
	}

	private void cmbDateTypeSummary_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeSummary.Value) == 1)
			{
				txtDateFromSummary.Value = DateTime.Now;
				txtToDateSummary.Value = DateTime.Now;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				txtDateFromSummary.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtDateFromSummary.Value = first;
				txtToDateSummary.Value = DateTime.Now;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtDateFromSummary.Value = first2;
				txtToDateSummary.Value = DateTime.Now;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				txtDateFromSummary.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowSumamry_Click(object sender, EventArgs e)
	{
		try
		{
			btnPrint.Visible = true;
			SummaryGridFill();
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SummaryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFromSummary.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDateSummary.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoSummary.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoSummary.Text);
			obj.ItemClassGroupId = Conversion.ToInt(cmbItemclassSummary.Value);
			obj.ParentCategoryId = Conversion.ToInt(CmbParentCategorySummary.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbItemCatgorySummary.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemTypeSummary.Value);
			obj.WarehouseId = Conversion.ToInt(CmbWarehouseSummary.Value);
			obj.JobLotId = Conversion.ToInt(CmbJobLotSummary.Value);
			obj.SupplierCustomerId = Conversion.ToInt(CmbPartySummary.Value);
			obj.ItemId = Conversion.ToInt(CmbItemSummary.Value);
			obj.PackSizeFrom = Conversion.ToInt(cmbUOM.Value);
			obj.DistrictId = Conversion.ToInt(CmbDistrictSummary.Value);
			obj.CityId = Conversion.ToInt(cmbCitySummary.Value);
			obj.ReportType = ((Control)(object)cmbActivitySummary).Text;
			dtSummary = InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt(obj);
			if (dtSummary.Rows.Count > 0)
			{
				if (((UltraGridBase)cmbActivitySummary).ActiveRow != null)
				{
					if (((Control)(object)cmbActivitySummary).Text == "Sales Register")
					{
						dtSalesRegister.Rows.Clear();
						for (int i = 0; i < dtSummary.Rows.Count; i++)
						{
							dtSalesRegister.Rows.Add(Conversion.ToDateTime(dtSummary.Rows[i]["DocDate"]).ToShortDateString(), dtSummary.Rows[i]["DocCodeNo"], dtSummary.Rows[i]["RefDocIdNo"], dtSummary.Rows[i]["RefDocumentTypeId"], dtSummary.Rows[i]["DocumentTypeCode"], dtSummary.Rows[i]["Customer"], dtSummary.Rows[i]["ItemCode"], dtSummary.Rows[i]["ItemName"], dtSummary.Rows[i]["UOMCode"], dtSummary.Rows[i]["JobLotCode"], dtSummary.Rows[i]["PackTypeCode"], dtSummary.Rows[i]["VehicleNo"], dtSummary.Rows[i]["QtyOut"], dtSummary.Rows[i]["BillWeightOut"], dtSummary.Rows[i]["AmountOut"], dtSummary.Rows[i]["AvgRate"], dtSummary.Rows[i]["PrcntOfTotal"], dtSummary.Rows[i]["PrcntOfTotalWeight"], dtSummary.Rows[i]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesRegister";
						grdSummary.DataSource = dtSalesRegister;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item")
					{
						dtSalesSummaryByItem.Rows.Clear();
						for (int j = 0; j < dtSummary.Rows.Count; j++)
						{
							dtSalesSummaryByItem.Rows.Add(dtSummary.Rows[j]["ItemCode"], dtSummary.Rows[j]["ItemName"], dtSummary.Rows[j]["QtyOut"], dtSummary.Rows[j]["BillWeightOut"], dtSummary.Rows[j]["AmountOut"], dtSummary.Rows[j]["AvgRate"], dtSummary.Rows[j]["PrcntOfTotal"], dtSummary.Rows[j]["PrcntOfTotalWeight"], dtSummary.Rows[j]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByItem";
						grdSummary.DataSource = dtSalesSummaryByItem;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item & Pack Size")
					{
						dtSalesSummaryByItemandPackSize.Rows.Clear();
						for (int k = 0; k < dtSummary.Rows.Count; k++)
						{
							dtSalesSummaryByItemandPackSize.Rows.Add(dtSummary.Rows[k]["ItemCode"], dtSummary.Rows[k]["ItemName"], dtSummary.Rows[k]["UOMCode"], dtSummary.Rows[k]["QtyOut"], dtSummary.Rows[k]["BillWeightOut"], dtSummary.Rows[k]["AmountOut"], dtSummary.Rows[k]["AvgRate"], dtSummary.Rows[k]["PrcntOfTotal"], dtSummary.Rows[k]["PrcntOfTotalWeight"], dtSummary.Rows[k]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByItemandPackSize";
						grdSummary.DataSource = dtSalesSummaryByItemandPackSize;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item & Warehouse")
					{
						dtSalesSummaryByItemNWarehouse.Rows.Clear();
						for (int l = 0; l < dtSummary.Rows.Count; l++)
						{
							dtSalesSummaryByItemNWarehouse.Rows.Add(dtSummary.Rows[l]["WarehouseName"], dtSummary.Rows[l]["ItemCode"], dtSummary.Rows[l]["ItemName"], dtSummary.Rows[l]["QtyOut"], dtSummary.Rows[l]["BillWeightOut"], dtSummary.Rows[l]["AmountOut"], dtSummary.Rows[l]["AvgRate"], dtSummary.Rows[l]["PrcntOfTotal"], dtSummary.Rows[l]["PrcntOfTotalWeight"], dtSummary.Rows[l]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByItemNWarehouse";
						grdSummary.DataSource = dtSalesSummaryByItemNWarehouse;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item & City")
					{
						dtSalesSummaryByItemandCity.Rows.Clear();
						for (int m = 0; m < dtSummary.Rows.Count; m++)
						{
							dtSalesSummaryByItemandCity.Rows.Add(dtSummary.Rows[m]["ItemCode"], dtSummary.Rows[m]["ItemName"], dtSummary.Rows[m]["QtyOut"], dtSummary.Rows[m]["BillWeightOut"], dtSummary.Rows[m]["AmountOut"], dtSummary.Rows[m]["AvgRate"], dtSummary.Rows[m]["PrcntOfTotal"], dtSummary.Rows[m]["PrcntOfTotalWeight"], dtSummary.Rows[m]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByItemAndCity";
						grdSummary.DataSource = dtSalesSummaryByItemandCity;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item,Pack Size & City")
					{
						dtSalesSummaryByItemNPackSizeCity.Rows.Clear();
						for (int n = 0; n < dtSummary.Rows.Count; n++)
						{
							dtSalesSummaryByItemNPackSizeCity.Rows.Add(dtSummary.Rows[n]["ItemCode"], dtSummary.Rows[n]["ItemName"], dtSummary.Rows[n]["UOMCode"], dtSummary.Rows[n]["QtyOut"], dtSummary.Rows[n]["BillWeightOut"], dtSummary.Rows[n]["AmountOut"], dtSummary.Rows[n]["AvgRate"], dtSummary.Rows[n]["PrcntOfTotal"], dtSummary.Rows[n]["PrcntOfTotalWeight"], dtSummary.Rows[n]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByItemNPackSizeCity";
						grdSummary.DataSource = dtSalesSummaryByItemNPackSizeCity;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer")
					{
						dtSalesSummaryByCustomer.Rows.Clear();
						for (int i2 = 0; i2 < dtSummary.Rows.Count; i2++)
						{
							dtSalesSummaryByCustomer.Rows.Add(dtSummary.Rows[i2]["Customer"], dtSummary.Rows[i2]["QtyOut"], dtSummary.Rows[i2]["BillWeightOut"], dtSummary.Rows[i2]["AmountOut"], dtSummary.Rows[i2]["AvgRate"], dtSummary.Rows[i2]["PrcntOfTotal"], dtSummary.Rows[i2]["PrcntOfTotalWeight"], dtSummary.Rows[i2]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByCustomer";
						grdSummary.DataSource = dtSalesSummaryByCustomer;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer & Item")
					{
						dtSalesSummaryByCustomerandItem.Rows.Clear();
						for (int i3 = 0; i3 < dtSummary.Rows.Count; i3++)
						{
							dtSalesSummaryByCustomerandItem.Rows.Add(dtSummary.Rows[i3]["Customer"], dtSummary.Rows[i3]["ItemCode"], dtSummary.Rows[i3]["ItemName"], dtSummary.Rows[i3]["UOMCode"], dtSummary.Rows[i3]["QtyOut"], dtSummary.Rows[i3]["BillWeightOut"], dtSummary.Rows[i3]["AmountOut"], dtSummary.Rows[i3]["AvgRate"], dtSummary.Rows[i3]["PrcntOfTotal"], dtSummary.Rows[i3]["PrcntOfTotalWeight"], dtSummary.Rows[i3]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByCustomerandItem";
						grdSummary.DataSource = dtSalesSummaryByCustomerandItem;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer & City")
					{
						dtSalesSummaryByCustomerandCity.Rows.Clear();
						for (int i4 = 0; i4 < dtSummary.Rows.Count; i4++)
						{
							dtSalesSummaryByCustomerandCity.Rows.Add(dtSummary.Rows[i4]["Customer"], dtSummary.Rows[i4]["UOMCode"], dtSummary.Rows[i4]["QtyOut"], dtSummary.Rows[i4]["BillWeightOut"], dtSummary.Rows[i4]["AmountOut"], dtSummary.Rows[i4]["AvgRate"], dtSummary.Rows[i4]["PrcntOfTotal"], dtSummary.Rows[i4]["PrcntOfTotalWeight"], dtSummary.Rows[i4]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByCustomerandCity";
						grdSummary.DataSource = dtSalesSummaryByCustomerandCity;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer,Item & City")
					{
						dtSalesSummaryByCustomerItemandCity.Rows.Clear();
						for (int i5 = 0; i5 < dtSummary.Rows.Count; i5++)
						{
							dtSalesSummaryByCustomerItemandCity.Rows.Add(dtSummary.Rows[i5]["Customer"], dtSummary.Rows[i5]["ItemCode"], dtSummary.Rows[i5]["ItemName"], dtSummary.Rows[i5]["UOMCode"], dtSummary.Rows[i5]["QtyOut"], dtSummary.Rows[i5]["BillWeightOut"], dtSummary.Rows[i5]["AmountOut"], dtSummary.Rows[i5]["AvgRate"], dtSummary.Rows[i5]["PrcntOfTotal"], dtSummary.Rows[i5]["PrcntOfTotalWeight"], dtSummary.Rows[i5]["CityName"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByCustomerItemandCity";
						grdSummary.DataSource = dtSalesSummaryByCustomerItemandCity;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer & Pack Size")
					{
						dtSalesSummaryByCustomerandPackSize.Rows.Clear();
						for (int i6 = 0; i6 < dtSummary.Rows.Count; i6++)
						{
							dtSalesSummaryByCustomerandPackSize.Rows.Add(dtSummary.Rows[i6]["Customer"], dtSummary.Rows[i6]["UOMCode"], dtSummary.Rows[i6]["QtyOut"], dtSummary.Rows[i6]["BillWeightOut"], dtSummary.Rows[i6]["AmountOut"], dtSummary.Rows[i6]["AvgRate"], dtSummary.Rows[i6]["PrcntOfTotal"], dtSummary.Rows[i6]["PrcntOfTotalWeight"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByCustomerandPackSize";
						grdSummary.DataSource = dtSalesSummaryByCustomerandPackSize;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Parent Category")
					{
						dtSalesSummaryByParentCategory.Rows.Clear();
						for (int i7 = 0; i7 < dtSummary.Rows.Count; i7++)
						{
							dtSalesSummaryByParentCategory.Rows.Add(dtSummary.Rows[i7]["ParentCategory"], dtSummary.Rows[i7]["QtyOut"], dtSummary.Rows[i7]["BillWeightOut"], dtSummary.Rows[i7]["AmountOut"], dtSummary.Rows[i7]["AvgRate"], dtSummary.Rows[i7]["PrcntOfTotal"], dtSummary.Rows[i7]["PrcntOfTotalWeight"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByParentCategory";
						grdSummary.DataSource = dtSalesSummaryByParentCategory;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Parent Category & Item")
					{
						dtSalesSummaryByItemAndParentCategory.Rows.Clear();
						for (int i8 = 0; i8 < dtSummary.Rows.Count; i8++)
						{
							dtSalesSummaryByItemAndParentCategory.Rows.Add(dtSummary.Rows[i8]["ItemName"], dtSummary.Rows[i8]["ParentCategory"], dtSummary.Rows[i8]["QtyOut"], dtSummary.Rows[i8]["BillWeightOut"], dtSummary.Rows[i8]["AmountOut"], dtSummary.Rows[i8]["AvgRate"], dtSummary.Rows[i8]["PrcntOfTotal"], dtSummary.Rows[i8]["PrcntOfTotalWeight"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByItemAndParentCategory";
						grdSummary.DataSource = dtSalesSummaryByItemAndParentCategory;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
					else if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Parent Category & Customer")
					{
						dtSalesSummaryByCustomerAndParentCategory.Rows.Clear();
						for (int i9 = 0; i9 < dtSummary.Rows.Count; i9++)
						{
							dtSalesSummaryByCustomerAndParentCategory.Rows.Add(dtSummary.Rows[i9]["Customer"], dtSummary.Rows[i9]["ParentCategory"], dtSummary.Rows[i9]["QtyOut"], dtSummary.Rows[i9]["BillWeightOut"], dtSummary.Rows[i9]["AmountOut"], dtSummary.Rows[i9]["AvgRate"], dtSummary.Rows[i9]["PrcntOfTotal"], dtSummary.Rows[i9]["PrcntOfTotalWeight"]);
						}
						((Control)(object)grdSummary).Name = "SalesSummaryByCustomerAndParentCategory";
						grdSummary.DataSource = dtSalesSummaryByCustomerAndParentCategory;
						grdSummary.RetrieveStructure();
						SummaryGridSetting();
					}
				}
				else
				{
					grdSummary.ClearStructure();
				}
			}
			else
			{
				grdSummary.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SummaryGridSetting()
	{
		try
		{
			grdSummary.RootTable.Columns["PrctByAmount"].AggregateFunction = (AggregateFunction)2;
			grdSummary.RootTable.Columns["PrctByAmount"].TextAlignment = (TextAlignment)3;
			grdSummary.RootTable.Columns["PrctByAmount"].FormatString = "#,##0.####";
			grdSummary.RootTable.Columns["PrctByAmount"].TotalFormatString = "#,##0.####";
			grdSummary.RootTable.Columns["PrctByWeight"].AggregateFunction = (AggregateFunction)2;
			grdSummary.RootTable.Columns["PrctByWeight"].TextAlignment = (TextAlignment)3;
			grdSummary.RootTable.Columns["PrctByWeight"].FormatString = "#,##0.####";
			grdSummary.RootTable.Columns["PrctByWeight"].TotalFormatString = "#,##0.####";
			grdSummary.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdSummary.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdSummary.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdSummary.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdSummary.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			grdSummary.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			grdSummary.RootTable.Columns["BillWeight"].FormatString = "#,##0.####";
			grdSummary.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.####";
			grdSummary.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdSummary.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdSummary.RootTable.Columns["Amount"].FormatString = "#,##0.####";
			grdSummary.RootTable.Columns["Amount"].TotalFormatString = "#,##0.####";
			grdSummary.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdSummary.RootTable.Columns["AvgRate"].FormatString = "#,##0.####";
			if (((Control)(object)cmbActivitySummary).Text == "Sales Register")
			{
				grdSummary.RootTable.Columns["DocumentTypeId"].Visible = false;
				grdSummary.RootTable.Columns["Id"].Visible = false;
			}
			if (((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item & Pack Size" || ((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item" || ((Control)(object)cmbActivitySummary).Text == "Sales Summary By Item & Warehouse" || ((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer" || ((Control)(object)cmbActivitySummary).Text == "Sales Summary By Customer & Item")
			{
				grdSummary.RootTable.Columns["CityName"].Visible = false;
			}
			CommonServices.GridAutoAdjustmentNew(grdSummary);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdSummary).Name);
			if (fs != null)
			{
				grdSummary.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewSummary_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDocNoSummary.Text = string.Empty;
			txtToDocNoSummary.Text = string.Empty;
			((Control)(object)cmbItemclassSummary).Text = string.Empty;
			((Control)(object)CmbParentCategorySummary).Text = string.Empty;
			((Control)(object)cmbItemCatgorySummary).Text = string.Empty;
			((Control)(object)cmbItemTypeSummary).Text = string.Empty;
			((Control)(object)CmbWarehouseSummary).Text = string.Empty;
			((Control)(object)CmbJobLotSummary).Text = string.Empty;
			((Control)(object)CmbPartySummary).Text = string.Empty;
			((Control)(object)CmbItemSummary).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			((Control)(object)CmbDistrictSummary).Text = string.Empty;
			((UltraGridBase)cmbActivitySummary).Rows[0].Activate();
			((UltraGridBase)cmbDateType).Rows[2].Activate();
			cmbDateType.Focus();
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
			AllComboBindSummary();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
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
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrint.Text == "1809_01_SalesRegisterSummary")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummary, "1809_01_SalesRegisterSummary.rpt");
			}
			else if (btnPrint.Text == "1809_02-SalesRegisterSummaryByCustomer")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtSummary, "1809_02-SalesRegisterSummaryByCustomer.rpt");
			}
			else if (btnPrint.Text == "1809_03-SalesRegisterSummaryByItem")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtSummary, "1809_03-SalesRegisterSummaryByItem.rpt");
			}
			else if (btnPrint.Text == "1809_04-SalesRegisterSummaryByCustomer&Item")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtSummary, "1809_04-SalesRegisterSummaryByCustomer&Item.rpt");
			}
			else if (btnPrint.Text == "1809_05-SalesRegisterSummaryByItemWithoutPacking")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtSummary, "1809_05-SalesRegisterSummaryByItemWithoutPacking.rpt");
			}
			else if (btnPrint.Text == "1809_06-SalesRegisterSummaryByWarehouse")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtSummary, "1809_06-SalesRegisterSummaryByWarehouse.rpt");
			}
			else if (btnPrint.Text == "1809_15-SalesSummaryByItem&City")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtSummary, "1809_15-SalesSummaryByItem&City.rpt");
			}
			else if (btnPrint.Text == "1809_14-SalesSummaryByItemPackSize&City")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtSummary, "1809_14-SalesSummaryByItemPackSize&City.rpt");
			}
			else if (btnPrint.Text == "1809_07-SalesSummaryByCustomer&City")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtSummary, "1809_07-SalesSummaryByCustomer&City.rpt");
			}
			else if (btnPrint.Text == "1809_09-SalesSummaryByCustomerItem&City")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtSummary, "1809_09-SalesSummaryByCustomerItem&City.rpt");
			}
			else if (btnPrint.Text == "1809_10-SalesSummaryByCustomer&PackSize")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtSummary, "1809_10-SalesSummaryByCustomer&PackSize.rpt");
			}
			else if (btnPrint.Text == "1809_11-SalesSummaryByParentCategory")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val12 = new Reporting();
				val12.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val12.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val12.ShowReportWithDataTable(dtSummary, "1809_11-SalesSummaryByParentCategory.rpt");
			}
			else if (btnPrint.Text == "1809_12-SalesSummaryByParentCategory&Item")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val13 = new Reporting();
				val13.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val13.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val13.ShowReportWithDataTable(dtSummary, "1809_12-SalesSummaryByParentCategory&Item.rpt");
			}
			else if (btnPrint.Text == "1809_13-SalesSummaryByParentCategory&Customer")
			{
				if (dtSummary.Rows.Count == 0 || dtSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val14 = new Reporting();
				val14.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val14.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val14.ShowReportWithDataTable(dtSummary, "1809_13-SalesSummaryByParentCategory&Customer.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutKeysSummary_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeysForSumamryTab();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeysForSumamryTab()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Filter");
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
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Expected O, but got Unknown
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Expected O, but got Unknown
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Expected O, but got Unknown
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Expected O, but got Unknown
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Expected O, but got Unknown
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Expected O, but got Unknown
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Expected O, but got Unknown
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Expected O, but got Unknown
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Expected O, but got Unknown
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Expected O, but got Unknown
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Expected O, but got Unknown
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f4: Expected O, but got Unknown
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Expected O, but got Unknown
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Expected O, but got Unknown
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Expected O, but got Unknown
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Expected O, but got Unknown
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Expected O, but got Unknown
		//IL_0ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efc: Expected O, but got Unknown
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f13: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance137 = new Appearance();
		GridEXLayout grdSummary_DesignTimeLayout = new GridEXLayout();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
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
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label13 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtsaleorderfrom = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbDateType = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.cmbitem = new UltraCombo();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.txtInvoiceNoFrom = new System.Windows.Forms.TextBox();
		this.txtsaleorderto = new System.Windows.Forms.TextBox();
		this.cmbWareHouse = new UltraCombo();
		this.txtInvoiceNoto = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtratefrom = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtrateto = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tsDropDown = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdSummary = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbDateTypeSummary = new UltraCombo();
		this.CmbDistrictSummary = new UltraCombo();
		this.txtDateFromSummary = new System.Windows.Forms.DateTimePicker();
		this.label14 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.cmbCitySummary = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbItemclassSummary = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.btnShowSumamry = new System.Windows.Forms.Button();
		this.txtToDateSummary = new System.Windows.Forms.DateTimePicker();
		this.cmbActivitySummary = new UltraCombo();
		this.CmbItemSummary = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbItemTypeSummary = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtFromDocNoSummary = new System.Windows.Forms.TextBox();
		this.txtToDocNoSummary = new System.Windows.Forms.TextBox();
		this.CmbWarehouseSummary = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbPartySummary = new UltraCombo();
		this.cmbItemCatgorySummary = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbJobLotSummary = new UltraCombo();
		this.CmbParentCategorySummary = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewSummary = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeysSummary = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummary).BeginInit();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDistrictSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCitySummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemclassSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemTypeSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartySummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCatgorySummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategorySummary).BeginInit();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1242, 695);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1242, 695);
		this.tabControl1.TabIndex = 11;
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1234, 666);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail Register";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 193);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1228, 470);
		this.panel6.TabIndex = 4;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1228, 470);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 170);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1228, 23);
		this.panel4.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1189, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 11;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(4, 1);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(171, 19);
		this.label13.TabIndex = 6;
		this.label13.Text = "Sale Invoice History";
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 58);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1228, 112);
		this.panel5.TabIndex = 2;
		this.groupBox1.Controls.Add(this.txtsaleorderfrom);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.GRNfromdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox1.Controls.Add(this.GRNToDate);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtInvoiceNoFrom);
		this.groupBox1.Controls.Add(this.txtsaleorderto);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouse);
		this.groupBox1.Controls.Add(this.txtInvoiceNoto);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtratefrom);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtrateto);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1054, 104);
		this.groupBox1.TabIndex = 210;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.txtsaleorderfrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsaleorderfrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsaleorderfrom.Location = new System.Drawing.Point(339, 46);
		this.txtsaleorderfrom.Name = "txtsaleorderfrom";
		this.txtsaleorderfrom.Size = new System.Drawing.Size(81, 23);
		this.txtsaleorderfrom.TabIndex = 5;
		this.txtsaleorderfrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsaleorderfrom_KeyPress);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 22);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 209;
		this.label12.Text = "Date Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(765, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(58, 15);
		this.label1.TabIndex = 185;
		this.label1.Text = "Customer";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(985, 44);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 27);
		this.btnshow.TabIndex = 13;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(73, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 0;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(6, 79);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 189;
		this.label33.Text = "To Date";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(507, 79);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(43, 15);
		this.label11.TabIndex = 207;
		this.label11.Text = "JobLot";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(828, 16);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 9;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance100;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(578, 73);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 12;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 50);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 190;
		this.label27.Text = "From Date";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(215, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(121, 15);
		this.label2.TabIndex = 188;
		this.label2.Text = "InvoiceNo From && To";
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(73, 46);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(138, 23);
		this.GRNfromdate.TabIndex = 1;
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(578, 16);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 10;
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(73, 75);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(138, 23);
		this.GRNToDate.TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(507, 22);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 194;
		this.label6.Text = "Item Name";
		this.txtInvoiceNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNoFrom.Location = new System.Drawing.Point(339, 18);
		this.txtInvoiceNoFrom.Name = "txtInvoiceNoFrom";
		this.txtInvoiceNoFrom.Size = new System.Drawing.Size(81, 23);
		this.txtInvoiceNoFrom.TabIndex = 3;
		this.txtInvoiceNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInvoiceNoFrom_KeyPress);
		this.txtsaleorderto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsaleorderto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsaleorderto.Location = new System.Drawing.Point(423, 46);
		this.txtsaleorderto.Name = "txtsaleorderto";
		this.txtsaleorderto.Size = new System.Drawing.Size(81, 23);
		this.txtsaleorderto.TabIndex = 6;
		this.txtsaleorderto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsaleorderto_KeyPress);
		this.cmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance127;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Location = new System.Drawing.Point(578, 44);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Name = "cmbWareHouse";
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).TabIndex = 11;
		this.txtInvoiceNoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNoto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNoto.Location = new System.Drawing.Point(423, 18);
		this.txtInvoiceNoto.Name = "txtInvoiceNoto";
		this.txtInvoiceNoto.Size = new System.Drawing.Size(81, 23);
		this.txtInvoiceNoto.TabIndex = 4;
		this.txtInvoiceNoto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInvoiceNoto_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(507, 50);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(69, 15);
		this.label10.TabIndex = 205;
		this.label10.Text = "WareHouse";
		this.txtratefrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtratefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtratefrom.Location = new System.Drawing.Point(339, 75);
		this.txtratefrom.Name = "txtratefrom";
		this.txtratefrom.ShortcutsEnabled = false;
		this.txtratefrom.Size = new System.Drawing.Size(81, 23);
		this.txtratefrom.TabIndex = 7;
		this.txtratefrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtratefrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtratefrom_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(215, 50);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(96, 15);
		this.label9.TabIndex = 202;
		this.label9.Text = "Order From && To";
		this.txtrateto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrateto.Location = new System.Drawing.Point(423, 75);
		this.txtrateto.Name = "txtrateto";
		this.txtrateto.ShortcutsEnabled = false;
		this.txtrateto.Size = new System.Drawing.Size(81, 23);
		this.txtrateto.TabIndex = 8;
		this.txtrateto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtrateto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtrateto_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(215, 79);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(89, 15);
		this.label5.TabIndex = 198;
		this.label5.Text = "Rate From && To";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 32);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1228, 26);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(171, 19);
		this.label23.TabIndex = 6;
		this.label23.Text = "Sale Invoice History";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1228, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.tsDropDown, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1228, 29);
		this.toolStrip3.TabIndex = 10;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 26);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 26);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.tsDropDown.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsDropDown.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDropDown.Name = "tsDropDown";
		this.tsDropDown.Size = new System.Drawing.Size(70, 26);
		this.tsDropDown.Text = "Print";
		this.tsDropDown.ToolTipText = "Sales";
		this.tsDropDown.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDown_DropDownItemClicked);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(126, 26);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1234, 666);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Summary Register";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummary);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(3, 219);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1228, 444);
		this.panel11.TabIndex = 170;
		this.grdSummary.AllowEdit = (InheritableBoolean)2;
		grdSummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummary.DesignTimeLayout = grdSummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummary.DynamicFiltering = true;
		this.grdSummary.FilterMode = (FilterMode)2;
		this.grdSummary.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummary.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSummary).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSummary).Name = "grdSummary";
		((ScrollableBase)this.grdSummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummary).Size = new System.Drawing.Size(1228, 444);
		((System.Windows.Forms.Control)(object)this.grdSummary).TabIndex = 1;
		this.grdSummary.TotalRow = (InheritableBoolean)1;
		this.grdSummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummary).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar2);
		this.panel10.Controls.Add(this.label15);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 195);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1228, 24);
		this.panel10.TabIndex = 169;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1189, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 10;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 2);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(196, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Activity Wise Summary";
		this.panel9.Controls.Add(this.groupBox2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 55);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1228, 140);
		this.panel9.TabIndex = 168;
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeSummary);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDistrictSummary);
		this.groupBox2.Controls.Add(this.txtDateFromSummary);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCitySummary);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemclassSummary);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.btnShowSumamry);
		this.groupBox2.Controls.Add(this.txtToDateSummary);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivitySummary);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemSummary);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemTypeSummary);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.txtFromDocNoSummary);
		this.groupBox2.Controls.Add(this.txtToDocNoSummary);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseSummary);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartySummary);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCatgorySummary);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotSummary);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategorySummary);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1209, 133);
		this.groupBox2.TabIndex = 178;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(6, 20);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 126;
		this.label19.Text = "Date Type";
		this.cmbDateTypeSummary.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance172).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeSummary).Location = new System.Drawing.Point(127, 14);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeSummary).Name = "cmbDateTypeSummary";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeSummary).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeSummary).TabIndex = 0;
		this.cmbDateTypeSummary.ValueChanged += new System.EventHandler(cmbDateTypeSummary_ValueChanged);
		this.CmbDistrictSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbDistrictSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDistrictSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDistrictSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance188).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDistrictSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDistrictSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDistrictSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDistrictSummary).Location = new System.Drawing.Point(950, 41);
		((System.Windows.Forms.Control)(object)this.CmbDistrictSummary).Name = "CmbDistrictSummary";
		((System.Windows.Forms.Control)(object)this.CmbDistrictSummary).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.CmbDistrictSummary).TabIndex = 15;
		this.txtDateFromSummary.CustomFormat = "dd-MMM-yy";
		this.txtDateFromSummary.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDateFromSummary.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFromSummary.Location = new System.Drawing.Point(127, 43);
		this.txtDateFromSummary.Name = "txtDateFromSummary";
		this.txtDateFromSummary.Size = new System.Drawing.Size(168, 23);
		this.txtDateFromSummary.TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(882, 47);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(45, 15);
		this.label14.TabIndex = 170;
		this.label14.Text = "District";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(302, 20);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(61, 15);
		this.label17.TabIndex = 174;
		this.label17.Text = "Item Class";
		this.cmbCitySummary.AllowNull = (DefaultableBoolean)1;
		this.cmbCitySummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCitySummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCitySummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCitySummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance46;
		((SpecialBoxBase)((UltraGridBase)this.cmbCitySummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCitySummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCitySummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCitySummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCitySummary).Location = new System.Drawing.Point(950, 69);
		((System.Windows.Forms.Control)(object)this.cmbCitySummary).Name = "cmbCitySummary";
		((System.Windows.Forms.Control)(object)this.cmbCitySummary).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbCitySummary).TabIndex = 16;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 47);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 1;
		this.label3.Text = "From Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(883, 75);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(62, 15);
		this.label4.TabIndex = 168;
		this.label4.Text = "City Name";
		this.cmbItemclassSummary.AllowNull = (DefaultableBoolean)1;
		this.cmbItemclassSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemclassSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemclassSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemclassSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemclassSummary).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemclassSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemclassSummary).Location = new System.Drawing.Point(395, 14);
		((System.Windows.Forms.Control)(object)this.cmbItemclassSummary).Name = "cmbItemclassSummary";
		((System.Windows.Forms.Control)(object)this.cmbItemclassSummary).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemclassSummary).TabIndex = 5;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(302, 103);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 172;
		this.label16.Text = "Item Type";
		this.btnShowSumamry.BackColor = System.Drawing.Color.Teal;
		this.btnShowSumamry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowSumamry.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowSumamry.ForeColor = System.Drawing.Color.White;
		this.btnShowSumamry.Location = new System.Drawing.Point(1143, 97);
		this.btnShowSumamry.Name = "btnShowSumamry";
		this.btnShowSumamry.Size = new System.Drawing.Size(61, 26);
		this.btnShowSumamry.TabIndex = 19;
		this.btnShowSumamry.Text = "Show";
		this.btnShowSumamry.UseVisualStyleBackColor = false;
		this.btnShowSumamry.Click += new System.EventHandler(btnShowSumamry_Click);
		this.txtToDateSummary.CustomFormat = "dd-MMM-yy";
		this.txtToDateSummary.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtToDateSummary.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateSummary.Location = new System.Drawing.Point(127, 71);
		this.txtToDateSummary.Name = "txtToDateSummary";
		this.txtToDateSummary.Size = new System.Drawing.Size(168, 23);
		this.txtToDateSummary.TabIndex = 2;
		this.cmbActivitySummary.AllowNull = (DefaultableBoolean)1;
		this.cmbActivitySummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivitySummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivitySummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivitySummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivitySummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivitySummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivitySummary).Location = new System.Drawing.Point(950, 97);
		((System.Windows.Forms.Control)(object)this.cmbActivitySummary).Name = "cmbActivitySummary";
		((System.Windows.Forms.Control)(object)this.cmbActivitySummary).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivitySummary).TabIndex = 18;
		this.CmbItemSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbItemSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance195).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemSummary).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemSummary).Location = new System.Drawing.Point(647, 97);
		((System.Windows.Forms.Control)(object)this.CmbItemSummary).Name = "CmbItemSummary";
		((System.Windows.Forms.Control)(object)this.CmbItemSummary).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemSummary).TabIndex = 12;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(882, 103);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(47, 15);
		this.label7.TabIndex = 163;
		this.label7.Text = "Activity";
		this.cmbItemTypeSummary.AllowNull = (DefaultableBoolean)1;
		this.cmbItemTypeSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemTypeSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemTypeSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemTypeSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemTypeSummary).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemTypeSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemTypeSummary).Location = new System.Drawing.Point(395, 97);
		((System.Windows.Forms.Control)(object)this.cmbItemTypeSummary).Name = "cmbItemTypeSummary";
		((System.Windows.Forms.Control)(object)this.cmbItemTypeSummary).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemTypeSummary).TabIndex = 8;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(573, 103);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 165;
		this.label8.Text = "Item Name";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(6, 75);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(48, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "To Date";
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance193;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance199).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance200).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(950, 14);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 13;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(6, 103);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(107, 15);
		this.label21.TabIndex = 5;
		this.label21.Text = "From && To Doc No";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(882, 20);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(35, 15);
		this.label22.TabIndex = 153;
		this.label22.Text = "UOM";
		this.txtFromDocNoSummary.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFromDocNoSummary.Location = new System.Drawing.Point(127, 99);
		this.txtFromDocNoSummary.Name = "txtFromDocNoSummary";
		this.txtFromDocNoSummary.Size = new System.Drawing.Size(79, 23);
		this.txtFromDocNoSummary.TabIndex = 3;
		this.txtToDocNoSummary.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtToDocNoSummary.Location = new System.Drawing.Point(212, 99);
		this.txtToDocNoSummary.Name = "txtToDocNoSummary";
		this.txtToDocNoSummary.Size = new System.Drawing.Size(83, 23);
		this.txtToDocNoSummary.TabIndex = 4;
		this.CmbWarehouseSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseSummary).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWarehouseSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseSummary).Location = new System.Drawing.Point(647, 14);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseSummary).Name = "CmbWarehouseSummary";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseSummary).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseSummary).TabIndex = 9;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(573, 20);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(69, 15);
		this.label24.TabIndex = 161;
		this.label24.Text = "WareHouse";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(302, 47);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(91, 15);
		this.label25.TabIndex = 151;
		this.label25.Text = "Parent Category";
		this.CmbPartySummary.AllowNull = (DefaultableBoolean)1;
		this.CmbPartySummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartySummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartySummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartySummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartySummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartySummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartySummary).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPartySummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartySummary).Location = new System.Drawing.Point(647, 69);
		((System.Windows.Forms.Control)(object)this.CmbPartySummary).Name = "CmbPartySummary";
		((System.Windows.Forms.Control)(object)this.CmbPartySummary).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartySummary).TabIndex = 11;
		this.cmbItemCatgorySummary.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCatgorySummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCatgorySummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCatgorySummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCatgorySummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCatgorySummary).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemCatgorySummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCatgorySummary).Location = new System.Drawing.Point(395, 69);
		((System.Windows.Forms.Control)(object)this.cmbItemCatgorySummary).Name = "cmbItemCatgorySummary";
		((System.Windows.Forms.Control)(object)this.cmbItemCatgorySummary).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCatgorySummary).TabIndex = 7;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(573, 75);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(69, 15);
		this.label26.TabIndex = 159;
		this.label26.Text = "Party Name";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(302, 75);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(82, 15);
		this.label28.TabIndex = 151;
		this.label28.Text = "Item Category";
		this.CmbJobLotSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotSummary).Location = new System.Drawing.Point(647, 41);
		((System.Windows.Forms.Control)(object)this.CmbJobLotSummary).Name = "CmbJobLotSummary";
		((System.Windows.Forms.Control)(object)this.CmbJobLotSummary).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotSummary).TabIndex = 10;
		this.CmbParentCategorySummary.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategorySummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategorySummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategorySummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategorySummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategorySummary).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbParentCategorySummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategorySummary).Location = new System.Drawing.Point(395, 41);
		((System.Windows.Forms.Control)(object)this.CmbParentCategorySummary).Name = "CmbParentCategorySummary";
		((System.Windows.Forms.Control)(object)this.CmbParentCategorySummary).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategorySummary).TabIndex = 6;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(573, 47);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(43, 15);
		this.label29.TabIndex = 157;
		this.label29.Text = "JobLot";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.lblCaption);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 31);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1228, 24);
		this.panel8.TabIndex = 167;
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(5, 2);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(207, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Sales Summary Register";
		this.panel7.Controls.Add(this.toolStrip1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1228, 28);
		this.panel7.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNewSummary, this.btnRefreshSummary, this.btnPrint, this.btnShortcutKeysSummary });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1228, 27);
		this.toolStrip1.TabIndex = 8;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewSummary.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewSummary.Name = "btnNewSummary";
		this.btnNewSummary.Size = new System.Drawing.Size(68, 24);
		this.btnNewSummary.Text = "&Reset";
		this.btnNewSummary.Click += new System.EventHandler(btnNewSummary_Click);
		this.btnRefreshSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshSummary.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummary.Name = "btnRefreshSummary";
		this.btnRefreshSummary.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshSummary.Text = "&Refresh";
		this.btnRefreshSummary.Click += new System.EventHandler(btnRefreshSummary_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortcutKeysSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeysSummary.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeysSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeysSummary.Name = "btnShortcutKeysSummary";
		this.btnShortcutKeysSummary.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeysSummary.Text = "ShortCut Keys";
		this.btnShortcutKeysSummary.Click += new System.EventHandler(btnShortcutKeysSummary_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1242, 695);
		base.Controls.Add(this.panel1);
		base.Name = "SaleInvoice_Register";
		this.Text = "Sale Invoice Register";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSaleInvoiceHistory_FormClosing);
		base.Load += new System.EventHandler(frmSaleInvoiceRegister_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummary).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDistrictSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCitySummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemclassSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemTypeSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartySummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCatgorySummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategorySummary).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
