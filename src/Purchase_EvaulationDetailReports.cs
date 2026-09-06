// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
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
namespace Architecture.WinApp.pcc.Reports
{

public class Purchase_EvaulationDetailReports : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtPurchaseRegister = new DataTable();

	private DataTable dtPurchaseSummaryByItem = new DataTable();

	private DataTable dtPurchaseSummaryByItemandCity = new DataTable();

	private DataTable dtPurchaseSummaryByItemandVarient = new DataTable();

	private DataTable dtPurchaseSummaryByItemNWarehouse = new DataTable();

	private DataTable dtPurchaseSummaryByItemNVarientCity = new DataTable();

	private DataTable dtPurchaseSummaryByCustomer = new DataTable();

	private DataTable dtPurchaseSummaryByCustomerandItem = new DataTable();

	private DataTable dtPurchaseSummaryByCustomerandCity = new DataTable();

	private DataTable dtPurchaseSummaryByCustomerandVareient = new DataTable();

	private DataTable dtPurchaseSummaryByCustomerItemandCity = new DataTable();

	private DataTable dtPurchaseSummaryByCity = new DataTable();

	private DataTable dtPurchaseSummaryByParentCategory = new DataTable();

	private DataTable dtPurchaseSummaryByItemAndParentCategory = new DataTable();

	private DataTable dtPurchaseSummaryByCustomerAndParentCategory = new DataTable();

	private DataTable dtGrid = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrint;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX DataGridHistory;

	private Label label2;

	private DateTimePicker txtToDate;

	private Label label1;

	private DateTimePicker txtDateFrom;

	private Label label3;

	private TextBox txtFromDocNo;

	private TextBox txtToDocNo;

	internal UltraCombo cmbItemCatgory;

	private Label label6;

	internal UltraCombo cmbJobLot;

	private Label label9;

	internal UltraCombo cmbSupplierCustomer;

	private Label label10;

	internal UltraCombo cmbWareHouse;

	private Label label11;

	internal UltraCombo cmbActivity;

	private Label label12;

	private Button btnShow;

	internal UltraCombo cmbItem;

	private Label label13;

	private Panel panel5;

	private Label lblCaption;

	internal UltraCombo cmbCity;

	private Label label5;

	internal UltraCombo cmbdistrict;

	private Label label14;

	internal UltraCombo CmbParentCategory;

	private Label label15;

	private Label label17;

	internal UltraCombo cmbItemclass;

	private Label label16;

	internal UltraCombo cmbItemtype;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateType;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortcut;

	private Panel panel6;

	private Label label4;

	private RadioButton RadSaleReturn;

	private RadioButton RadSales;

	public Purchase_EvaulationDetailReports(UserAccount userAccount)
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
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1120, 500);
			dtPurchaseRegister.Columns.Add("DocDate", typeof(DateTime));
			dtPurchaseRegister.Columns.Add("DocCodeNo", typeof(string));
			dtPurchaseRegister.Columns.Add("PartyName", typeof(string));
			dtPurchaseRegister.Columns.Add("ItemName", typeof(string));
			dtPurchaseRegister.Columns.Add("Varient", typeof(string));
			dtPurchaseRegister.Columns.Add("JobLotCode", typeof(string));
			dtPurchaseRegister.Columns.Add("VehicleNo", typeof(string));
			dtPurchaseRegister.Columns.Add("ItemQty", typeof(double));
			dtPurchaseRegister.Columns.Add("BillWeight", typeof(double));
			dtPurchaseRegister.Columns.Add("Amount", typeof(double));
			dtPurchaseRegister.Columns.Add("AvgRate", typeof(double));
			dtPurchaseRegister.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseRegister.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseRegister.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItem.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByItem.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByItem.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByItem.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByItem.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByItem.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByItemandCity.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItemandCity.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByItemandCity.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByItemandCity.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByItemandCity.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByItemandCity.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByItemandCity.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByItemandCity.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByItemandVarient.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItemandVarient.Columns.Add("Varient", typeof(string));
			dtPurchaseSummaryByItemandVarient.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByItemandVarient.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByItemandVarient.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByItemandVarient.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByItemandVarient.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByItemandVarient.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByItemNWarehouse.Columns.Add("WarehouseName", typeof(string));
			dtPurchaseSummaryByItemNWarehouse.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItemNWarehouse.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByItemNWarehouse.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByItemNWarehouse.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByItemNWarehouse.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByItemNWarehouse.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByItemNWarehouse.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByItemNVarientCity.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItemNVarientCity.Columns.Add("Varient", typeof(string));
			dtPurchaseSummaryByItemNVarientCity.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByItemNVarientCity.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByItemNVarientCity.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByItemNVarientCity.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByItemNVarientCity.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByItemNVarientCity.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByItemNVarientCity.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByCustomer.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByCustomer.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByCustomer.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByCustomer.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByCustomer.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByCustomer.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByCustomer.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByCustomerandItem.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByCustomerandItem.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByCustomerandItem.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByCustomerandItem.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByCustomerandItem.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByCustomerandItem.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByCustomerandItem.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByCustomerandItem.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByCustomerandCity.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByCustomerandCity.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByCustomerandCity.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByCustomerandCity.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByCustomerandCity.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByCustomerandCity.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByCustomerandCity.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByCustomerandCity.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByCustomerandVareient.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByCustomerandVareient.Columns.Add("Varient", typeof(string));
			dtPurchaseSummaryByCustomerandVareient.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByCustomerandVareient.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByCustomerandVareient.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByCustomerandVareient.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByCustomerandVareient.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByCustomerandVareient.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByCustomerItemandCity.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByCustomerItemandCity.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByCustomerItemandCity.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByCustomerItemandCity.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByCustomerItemandCity.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByCustomerItemandCity.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByCustomerItemandCity.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByCustomerItemandCity.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByCustomerItemandCity.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByCity.Columns.Add("CityName", typeof(string));
			dtPurchaseSummaryByCity.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByCity.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByCity.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByCity.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByCity.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByCity.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByParentCategory.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByParentCategory.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByParentCategory.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByParentCategory.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByParentCategory.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByParentCategory.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByParentCategory.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByItemAndParentCategory.Columns.Add("ItemName", typeof(string));
			dtPurchaseSummaryByItemAndParentCategory.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByItemAndParentCategory.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByItemAndParentCategory.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByItemAndParentCategory.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByItemAndParentCategory.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByItemAndParentCategory.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByItemAndParentCategory.Columns.Add("PrctByWeight", typeof(double));
			dtPurchaseSummaryByCustomerAndParentCategory.Columns.Add("PartyName", typeof(string));
			dtPurchaseSummaryByCustomerAndParentCategory.Columns.Add("ParentCategory", typeof(string));
			dtPurchaseSummaryByCustomerAndParentCategory.Columns.Add("ItemQty", typeof(double));
			dtPurchaseSummaryByCustomerAndParentCategory.Columns.Add("BillWeight", typeof(double));
			dtPurchaseSummaryByCustomerAndParentCategory.Columns.Add("Amount", typeof(double));
			dtPurchaseSummaryByCustomerAndParentCategory.Columns.Add("AvgRate", typeof(double));
			dtPurchaseSummaryByCustomerAndParentCategory.Columns.Add("PrctByAmount", typeof(double));
			dtPurchaseSummaryByCustomerAndParentCategory.Columns.Add("PrctByWeight", typeof(double));
			AllComboBind();
			ActivityFill();
			Datetypefill();
			((UltraGridBase)cmbActivity).Rows[0].Activate();
			PrintButtonManage();
			cmbDateType.Focus();
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
			DDL.BindDDL(dt, cmbDateType, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateType).Rows[2].Activate();
			((UltraGridBase)cmbDateType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			DataTable dt = InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				ReportType = "Purchase"
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
				DDL.BindDDLNew(dtParent, CmbParentCategory, "Id", "RefName", "Parent Category", false);
				((UltraGridBase)CmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtItemType, cmbItemtype, "Id", "RefName", "Item Type", false);
				((UltraGridBase)cmbItemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtClass, cmbItemclass, "Id", "RefName", "Class Group Name", false);
				((UltraGridBase)cmbItemclass).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtCategory, cmbItemCatgory, "Id", "RefName", "Item Category", false);
				((UltraGridBase)cmbItemCatgory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtSupplierCustomer, cmbSupplierCustomer, "Id", "RefName", "Party Name", false);
				DDL.BindDDLNew(dtItem, cmbItem, "Id", "RefName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtCity, cmbCity, "Id", "RefName", "CityName", false);
				DDL.BindDDLNew(dtJobLot, cmbJobLot, "Id", "RefName", "JobLot", false);
				((UltraGridBase)cmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtWareHouse, cmbWareHouse, "Id", "RefName", "WareHouse", false);
				((UltraGridBase)cmbWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtDistrict, cmbdistrict, "DistrictId", "District", "District", ZeroIndex: false);
				((UltraGridBase)cmbdistrict).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
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
			dt.Rows.Add(1, "Purchase Register");
			dt.Rows.Add(2, "Purchase Summary By Item");
			dt.Rows.Add(3, "Purchase Summary By City");
			dt.Rows.Add(4, "Purchase Summary By Item & City");
			dt.Rows.Add(5, "Purchase Summary By Item & Varient");
			dt.Rows.Add(6, "Purchase Summary By Item & Warehouse");
			dt.Rows.Add(7, "Purchase Summary By Item,Varient & City");
			dt.Rows.Add(8, "Purchase Summary By Supplier");
			dt.Rows.Add(9, "Purchase Summary By Supplier & Item");
			dt.Rows.Add(10, "Purchase Summary By Supplier & City");
			dt.Rows.Add(11, "Purchase Summary By Supplier & Varient");
			dt.Rows.Add(12, "Purchase Summary By Supplier,Item & City");
			dt.Rows.Add(13, "Purchase Summary By Parent Category");
			dt.Rows.Add(14, "Purchase Summary By Parent Category & Item");
			dt.Rows.Add(15, "Purchase Summary By Parent Category & Supplier");
			DDL.BindDDL(dt, cmbActivity, "Id", "Name", "Activity", ZeroIndex: false);
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
				txtDateFrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				txtDateFrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtDateFrom.Value = first;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtDateFrom.Value = first2;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
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
			btnPrint.Visible = true;
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
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		try
		{
			if (!RadSales.Checked && !RadSaleReturn.Checked)
			{
				MessageBox.Show("Please Select on Value Either Purchases or Return");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDate.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNo.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNo.Text);
			obj.ParentCategoryId = Conversion.ToInt(CmbParentCategory.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbItemCatgory.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierCustomer.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.JobLotId = Conversion.ToInt(cmbJobLot.Value);
			obj.CityId = Conversion.ToInt(cmbCity.Value);
			obj.WarehouseId = Conversion.ToInt(cmbWareHouse.Value);
			obj.DistrictId = Conversion.ToInt(cmbdistrict.Value);
			obj.ItemClassGroupId = Conversion.ToInt(cmbItemclass.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtype.Value);
			if (RadSales.Checked)
			{
				obj.ActionId = 1;
			}
			else if (RadSaleReturn.Checked)
			{
				obj.ActionId = 2;
			}
			obj.ReportType = ((Control)(object)cmbActivity).Text;
			dtGrid = InvPurchaseInvoice.Purchase_EvaulationDetailReports(obj);
			if (dtGrid.Rows.Count > 0)
			{
				if (((UltraGridBase)cmbActivity).ActiveRow != null)
				{
					if (((Control)(object)cmbActivity).Text == "Purchase Register")
					{
						dtPurchaseRegister.Rows.Clear();
						for (int i = 0; i < dtGrid.Rows.Count; i++)
						{
							dtPurchaseRegister.Rows.Add(Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), dtGrid.Rows[i]["DocCodeNo"], dtGrid.Rows[i]["Supplier"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["UOMCode"], dtGrid.Rows[i]["JobLotCode"], dtGrid.Rows[i]["VehicleNo"], dtGrid.Rows[i]["QtyIn"], dtGrid.Rows[i]["BillWeightIn"], dtGrid.Rows[i]["AmountIn"], dtGrid.Rows[i]["AvgRate"], dtGrid.Rows[i]["PrcntOfTotal"], dtGrid.Rows[i]["PrcntOfTotalWeight"], dtGrid.Rows[i]["CityName"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseRegister";
						DataGridHistory.DataSource = dtPurchaseRegister;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item")
					{
						dtPurchaseSummaryByItem.Rows.Clear();
						for (int j = 0; j < dtGrid.Rows.Count; j++)
						{
							dtPurchaseSummaryByItem.Rows.Add(dtGrid.Rows[j]["ItemName"], dtGrid.Rows[j]["QtyIn"], dtGrid.Rows[j]["BillWeightIn"], dtGrid.Rows[j]["AmountIn"], dtGrid.Rows[j]["AvgRate"], dtGrid.Rows[j]["PrcntOfTotal"], dtGrid.Rows[j]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByItem";
						DataGridHistory.DataSource = dtPurchaseSummaryByItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item & Varient")
					{
						dtPurchaseSummaryByItemandVarient.Rows.Clear();
						for (int k = 0; k < dtGrid.Rows.Count; k++)
						{
							dtPurchaseSummaryByItemandVarient.Rows.Add(dtGrid.Rows[k]["ItemName"], dtGrid.Rows[k]["UOMCode"], dtGrid.Rows[k]["QtyIn"], dtGrid.Rows[k]["BillWeightIn"], dtGrid.Rows[k]["AmountIn"], dtGrid.Rows[k]["AvgRate"], dtGrid.Rows[k]["PrcntOfTotal"], dtGrid.Rows[k]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByItemandVarient";
						DataGridHistory.DataSource = dtPurchaseSummaryByItemandVarient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item & Warehouse")
					{
						dtPurchaseSummaryByItemNWarehouse.Rows.Clear();
						for (int l = 0; l < dtGrid.Rows.Count; l++)
						{
							dtPurchaseSummaryByItemNWarehouse.Rows.Add(dtGrid.Rows[l]["WarehouseName"], dtGrid.Rows[l]["ItemName"], dtGrid.Rows[l]["QtyIn"], dtGrid.Rows[l]["BillWeightIn"], dtGrid.Rows[l]["AmountIn"], dtGrid.Rows[l]["AvgRate"], dtGrid.Rows[l]["PrcntOfTotal"], dtGrid.Rows[l]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByItemNWarehouse";
						DataGridHistory.DataSource = dtPurchaseSummaryByItemNWarehouse;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item & City")
					{
						dtPurchaseSummaryByItemandCity.Rows.Clear();
						for (int m = 0; m < dtGrid.Rows.Count; m++)
						{
							dtPurchaseSummaryByItemandCity.Rows.Add(dtGrid.Rows[m]["ItemName"], dtGrid.Rows[m]["QtyIn"], dtGrid.Rows[m]["BillWeightIn"], dtGrid.Rows[m]["AmountIn"], dtGrid.Rows[m]["AvgRate"], dtGrid.Rows[m]["PrcntOfTotal"], dtGrid.Rows[m]["PrcntOfTotalWeight"], dtGrid.Rows[m]["CityName"]);
						}
						DataGridHistory.DataSource = dtPurchaseSummaryByItemandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item,Varient & City")
					{
						dtPurchaseSummaryByItemNVarientCity.Rows.Clear();
						for (int n = 0; n < dtGrid.Rows.Count; n++)
						{
							dtPurchaseSummaryByItemNVarientCity.Rows.Add(dtGrid.Rows[n]["ItemName"], dtGrid.Rows[n]["UOMCode"], dtGrid.Rows[n]["QtyIn"], dtGrid.Rows[n]["BillWeightIn"], dtGrid.Rows[n]["AmountIn"], dtGrid.Rows[n]["AvgRate"], dtGrid.Rows[n]["PrcntOfTotal"], dtGrid.Rows[n]["PrcntOfTotalWeight"], dtGrid.Rows[n]["CityName"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByItemNVarientCity";
						DataGridHistory.DataSource = dtPurchaseSummaryByItemNVarientCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier")
					{
						dtPurchaseSummaryByCustomer.Rows.Clear();
						for (int i2 = 0; i2 < dtGrid.Rows.Count; i2++)
						{
							dtPurchaseSummaryByCustomer.Rows.Add(dtGrid.Rows[i2]["Supplier"], dtGrid.Rows[i2]["QtyIn"], dtGrid.Rows[i2]["BillWeightIn"], dtGrid.Rows[i2]["AmountIn"], dtGrid.Rows[i2]["AvgRate"], dtGrid.Rows[i2]["PrcntOfTotal"], dtGrid.Rows[i2]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByCustomer";
						DataGridHistory.DataSource = dtPurchaseSummaryByCustomer;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier & Item")
					{
						dtPurchaseSummaryByCustomerandItem.Rows.Clear();
						for (int i3 = 0; i3 < dtGrid.Rows.Count; i3++)
						{
							dtPurchaseSummaryByCustomerandItem.Rows.Add(dtGrid.Rows[i3]["Supplier"], dtGrid.Rows[i3]["ItemName"], dtGrid.Rows[i3]["QtyIn"], dtGrid.Rows[i3]["BillWeightIn"], dtGrid.Rows[i3]["AmountIn"], dtGrid.Rows[i3]["AvgRate"], dtGrid.Rows[i3]["PrcntOfTotal"], dtGrid.Rows[i3]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByCustomerandItem";
						DataGridHistory.DataSource = dtPurchaseSummaryByCustomerandItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier & City")
					{
						dtPurchaseSummaryByCustomerandCity.Rows.Clear();
						for (int i4 = 0; i4 < dtGrid.Rows.Count; i4++)
						{
							dtPurchaseSummaryByCustomerandCity.Rows.Add(dtGrid.Rows[i4]["Supplier"], dtGrid.Rows[i4]["QtyIn"], dtGrid.Rows[i4]["BillWeightIn"], dtGrid.Rows[i4]["AmountIn"], dtGrid.Rows[i4]["AvgRate"], dtGrid.Rows[i4]["PrcntOfTotal"], dtGrid.Rows[i4]["PrcntOfTotalWeight"], dtGrid.Rows[i4]["CityName"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByCustomerandCity";
						DataGridHistory.DataSource = dtPurchaseSummaryByCustomerandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier & Varient")
					{
						dtPurchaseSummaryByCustomerandVareient.Rows.Clear();
						for (int i5 = 0; i5 < dtGrid.Rows.Count; i5++)
						{
							dtPurchaseSummaryByCustomerandVareient.Rows.Add(dtGrid.Rows[i5]["Supplier"], dtGrid.Rows[i5]["UOMCode"], dtGrid.Rows[i5]["QtyIn"], dtGrid.Rows[i5]["BillWeightIn"], dtGrid.Rows[i5]["AmountIn"], dtGrid.Rows[i5]["AvgRate"], dtGrid.Rows[i5]["PrcntOfTotal"], dtGrid.Rows[i5]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByCustomerandVareient";
						DataGridHistory.DataSource = dtPurchaseSummaryByCustomerandVareient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier,Item & City")
					{
						dtPurchaseSummaryByCustomerItemandCity.Rows.Clear();
						for (int i6 = 0; i6 < dtGrid.Rows.Count; i6++)
						{
							dtPurchaseSummaryByCustomerItemandCity.Rows.Add(dtGrid.Rows[i6]["Supplier"], dtGrid.Rows[i6]["ItemName"], dtGrid.Rows[i6]["QtyIn"], dtGrid.Rows[i6]["BillWeightIn"], dtGrid.Rows[i6]["AmountIn"], dtGrid.Rows[i6]["AvgRate"], dtGrid.Rows[i6]["PrcntOfTotal"], dtGrid.Rows[i6]["PrcntOfTotalWeight"], dtGrid.Rows[i6]["CityName"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByCustomerItemandCity";
						DataGridHistory.DataSource = dtPurchaseSummaryByCustomerItemandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By City")
					{
						dtPurchaseSummaryByCity.Rows.Clear();
						for (int i7 = 0; i7 < dtGrid.Rows.Count; i7++)
						{
							dtPurchaseSummaryByCity.Rows.Add(dtGrid.Rows[i7]["CityName"], dtGrid.Rows[i7]["QtyIn"], dtGrid.Rows[i7]["BillWeightIn"], dtGrid.Rows[i7]["AmountIn"], dtGrid.Rows[i7]["AvgRate"], dtGrid.Rows[i7]["PrcntOfTotal"], dtGrid.Rows[i7]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByCity";
						DataGridHistory.DataSource = dtPurchaseSummaryByCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Parent Category")
					{
						dtPurchaseSummaryByParentCategory.Rows.Clear();
						for (int i8 = 0; i8 < dtGrid.Rows.Count; i8++)
						{
							dtPurchaseSummaryByParentCategory.Rows.Add(dtGrid.Rows[i8]["ParentCategory"], dtGrid.Rows[i8]["QtyIn"], dtGrid.Rows[i8]["BillWeightIn"], dtGrid.Rows[i8]["AmountIn"], dtGrid.Rows[i8]["AvgRate"], dtGrid.Rows[i8]["PrcntOfTotal"], dtGrid.Rows[i8]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByParentCategory";
						DataGridHistory.DataSource = dtPurchaseSummaryByParentCategory;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Parent Category & Item")
					{
						dtPurchaseSummaryByItemAndParentCategory.Rows.Clear();
						for (int i9 = 0; i9 < dtGrid.Rows.Count; i9++)
						{
							dtPurchaseSummaryByItemAndParentCategory.Rows.Add(dtGrid.Rows[i9]["ItemName"], dtGrid.Rows[i9]["ParentCategory"], dtGrid.Rows[i9]["QtyIn"], dtGrid.Rows[i9]["BillWeightIn"], dtGrid.Rows[i9]["AmountIn"], dtGrid.Rows[i9]["AvgRate"], dtGrid.Rows[i9]["PrcntOfTotal"], dtGrid.Rows[i9]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByItemAndParentCategory";
						DataGridHistory.DataSource = dtPurchaseSummaryByItemAndParentCategory;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Parent Category & Supplier")
					{
						dtPurchaseSummaryByCustomerAndParentCategory.Rows.Clear();
						for (int i10 = 0; i10 < dtGrid.Rows.Count; i10++)
						{
							dtPurchaseSummaryByCustomerAndParentCategory.Rows.Add(dtGrid.Rows[i10]["Supplier"], dtGrid.Rows[i10]["ParentCategory"], dtGrid.Rows[i10]["QtyIn"], dtGrid.Rows[i10]["BillWeightIn"], dtGrid.Rows[i10]["AmountIn"], dtGrid.Rows[i10]["AvgRate"], dtGrid.Rows[i10]["PrcntOfTotal"], dtGrid.Rows[i10]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "PurchaseSummaryByCustomerAndParentCategory";
						DataGridHistory.DataSource = dtPurchaseSummaryByCustomerAndParentCategory;
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
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["PrctByAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["PrctByAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PrctByAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["PrctByAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["PrctByWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["PrctByWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PrctByWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["PrctByWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Amount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AvgRate"].FormatString = "0,0";
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
			MessageBox.Show(ex.ToString());
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
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrint.Text == "1859_01-PurchaseRegisterSummary")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGrid, "1859_01-PurchaseRegisterSummary.rpt");
			}
			else if (btnPrint.Text == "1859_02-PurchaseRegisterSummaryByItem")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtGrid, "1859_02-PurchaseRegisterSummaryByItem.rpt");
			}
			else if (btnPrint.Text == "1859_03-PurchaseSummaryByItem&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtGrid, "1859_03-PurchaseSummaryByItem&City.rpt");
			}
			else if (btnPrint.Text == "1859_04-PurchaseRegisterSummaryByItem&Varient")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtGrid, "1859_04-PurchaseRegisterSummaryByItem&Varient.rpt");
			}
			else if (btnPrint.Text == "1859_05-PurchaseRegisterSummaryByItem&Warehouse")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtGrid, "1859_05-PurchaseRegisterSummaryByItem&Warehouse.rpt");
			}
			else if (btnPrint.Text == "1859_06-PurchaseSummaryByItemPackSize&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtGrid, "1859_06-PurchaseSummaryByItemPackSize&City.rpt");
			}
			else if (btnPrint.Text == "1859_07-PurchaseRegisterSummaryBySupplier")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtGrid, "1859_07-PurchaseRegisterSummaryBySupplier.rpt");
			}
			else if (btnPrint.Text == "1859_08-PurchaseSummaryBySupplier&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtGrid, "1859_08-PurchaseSummaryBySupplier&Item.rpt");
			}
			else if (btnPrint.Text == "1859_09-PurchaseRegisterSummaryBySupplier&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtGrid, "1859_09-PurchaseRegisterSummaryBySupplier&City.rpt");
			}
			else if (btnPrint.Text == "1859_10-PurchaseSummaryBySupplierItem&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtGrid, "1859_10-PurchaseSummaryBySupplierItem&City.rpt");
			}
			else if (btnPrint.Text == "1859_11-PurchaseSummaryBySupplier&Varient")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtGrid, "1859_11-PurchaseSummaryBySupplier&Varient.rpt");
			}
			else if (btnPrint.Text == "1859_12-PurchaseSummaryByCity")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val12 = new Reporting();
				val12.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val12.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val12.ShowReportWithDataTable(dtGrid, "1859_12-PurchaseSummaryByCity.rpt");
			}
			else if (btnPrint.Text == "1859_13-PurchaseSummaryByParentCategory")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val13 = new Reporting();
				val13.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val13.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val13.ShowReportWithDataTable(dtGrid, "1859_13-PurchaseSummaryByParentCategory.rpt");
			}
			else if (btnPrint.Text == "1859_14-PurchaseSummaryByParentCategory&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val14 = new Reporting();
				val14.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val14.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val14.ShowReportWithDataTable(dtGrid, "1859_14-PurchaseSummaryByParentCategory&Item.rpt");
			}
			else if (btnPrint.Text == "1859_15-PurchaseSummaryByParentCategory&Supplier")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val15 = new Reporting();
				val15.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val15.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val15.ShowReportWithDataTable(dtGrid, "1859_15-PurchaseSummaryByParentCategory&Supplier.rpt");
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
				if (((Control)(object)cmbActivity).Text == "Purchase Register")
				{
					btnPrint.Text = "1859_01-PurchaseRegisterSummary";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item")
				{
					btnPrint.Text = "1859_02-PurchaseRegisterSummaryByItem";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item & City")
				{
					btnPrint.Text = "1859_03-PurchaseSummaryByItem&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item & Varient")
				{
					btnPrint.Text = "1859_04-PurchaseRegisterSummaryByItem&Varient";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item & Warehouse")
				{
					btnPrint.Text = "1859_05-PurchaseRegisterSummaryByItem&Warehouse";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Item,Varient & City")
				{
					btnPrint.Text = "1859_06-PurchaseSummaryByItemPackSize&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier")
				{
					btnPrint.Text = "1859_07-PurchaseRegisterSummaryBySupplier";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier & Item")
				{
					btnPrint.Text = "1859_08-PurchaseSummaryBySupplier&Item";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier & City")
				{
					btnPrint.Text = "1859_09-PurchaseRegisterSummaryBySupplier&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier,Item & City")
				{
					btnPrint.Text = "1859_10-PurchaseSummaryBySupplierItem&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Supplier & Varient")
				{
					btnPrint.Text = "1859_11-PurchaseSummaryBySupplier&Varient";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By City")
				{
					btnPrint.Text = "1859_12-PurchaseSummaryByCity";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Parent Category")
				{
					btnPrint.Text = "1859_13-PurchaseSummaryByParentCategory";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Parent Category & Item")
				{
					btnPrint.Text = "1859_14-PurchaseSummaryByParentCategory&Item";
				}
				else if (((Control)(object)cmbActivity).Text == "Purchase Summary By Parent Category & Supplier")
				{
					btnPrint.Text = "1859_15-PurchaseSummaryByParentCategory&Supplier";
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
			txtFromDocNo.Text = string.Empty;
			txtToDocNo.Text = string.Empty;
			((Control)(object)cmbItemclass).Text = string.Empty;
			((Control)(object)CmbParentCategory).Text = string.Empty;
			((Control)(object)cmbItemCatgory).Text = string.Empty;
			((Control)(object)cmbItemtype).Text = string.Empty;
			((Control)(object)cmbWareHouse).Text = string.Empty;
			((Control)(object)cmbJobLot).Text = string.Empty;
			((Control)(object)cmbSupplierCustomer).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbdistrict).Text = string.Empty;
			((Control)(object)cmbdistrict).Text = string.Empty;
			((UltraGridBase)cmbActivity).Rows[0].Activate();
			cmbDateType.Focus();
			Datetypefill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmEvaulationDetailSalesReports_FormClosing(object sender, FormClosingEventArgs e)
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
			AllComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "DataGridHistory")
			{
				cmbDateType.Focus();
			}
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

	private void frmEvaulationDetailSalesReports_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
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
			dt.Rows.Add("Ctrl+P", "For print 312-SalesRegisterSummary");
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

	private void btnShortcut_Click(object sender, EventArgs e)
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

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow item = DataGridHistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					_ = DataGridHistory.CurrentColumn;
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
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Expected O, but got Unknown
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Expected O, but got Unknown
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
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
		Appearance appearance71 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance134 = new Appearance();
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
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
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
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.pcc.Reports.Purchase_EvaulationDetailReports));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.cmbdistrict = new UltraCombo();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.label14 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.cmbCity = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbItemclass = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.cmbActivity = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbItemtype = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtFromDocNo = new System.Windows.Forms.TextBox();
		this.txtToDocNo = new System.Windows.Forms.TextBox();
		this.cmbWareHouse = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbSupplierCustomer = new UltraCombo();
		this.cmbItemCatgory = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbJobLot = new UltraCombo();
		this.CmbParentCategory = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortcut = new System.Windows.Forms.ToolStripButton();
		this.RadSaleReturn = new System.Windows.Forms.RadioButton();
		this.RadSales = new System.Windows.Forms.RadioButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdistrict).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemclass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCatgory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1104, 461);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 192);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1104, 269);
		this.panel4.TabIndex = 2;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1104, 240);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1104, 29);
		this.panel6.TabIndex = 167;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1057, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 9;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label4.Location = new System.Drawing.Point(7, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 19);
		this.label4.TabIndex = 0;
		this.label4.Text = "History";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1104, 163);
		this.panel3.TabIndex = 1;
		this.groupBox1.Controls.Add(this.RadSaleReturn);
		this.groupBox1.Controls.Add(this.RadSales);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdistrict);
		this.groupBox1.Controls.Add(this.txtDateFrom);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCity);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemclass);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add(this.txtToDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivity);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtype);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtFromDocNo);
		this.groupBox1.Controls.Add(this.txtToDocNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouse);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCatgory);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 29);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1098, 131);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(6, 20);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 126;
		this.label19.Text = "Date Type";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(89, 14);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(164, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 0;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.cmbdistrict.AllowNull = (DefaultableBoolean)1;
		this.cmbdistrict.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdistrict.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdistrict.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance128).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdistrict).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdistrict.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdistrict).Location = new System.Drawing.Point(900, 14);
		((System.Windows.Forms.Control)(object)this.cmbdistrict).Name = "cmbdistrict";
		((System.Windows.Forms.Control)(object)this.cmbdistrict).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbdistrict).TabIndex = 13;
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(89, 44);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(164, 23);
		this.txtDateFrom.TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(837, 20);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(45, 15);
		this.label14.TabIndex = 170;
		this.label14.Text = "District";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(257, 20);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(61, 15);
		this.label17.TabIndex = 174;
		this.label17.Text = "Item Class";
		this.cmbCity.AllowNull = (DefaultableBoolean)1;
		this.cmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance46;
		((SpecialBoxBase)((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCity).Location = new System.Drawing.Point(901, 42);
		((System.Windows.Forms.Control)(object)this.cmbCity).Name = "cmbCity";
		((System.Windows.Forms.Control)(object)this.cmbCity).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.cmbCity).TabIndex = 14;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 48);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "From Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(837, 48);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(62, 15);
		this.label5.TabIndex = 168;
		this.label5.Text = "City Name";
		this.cmbItemclass.AllowNull = (DefaultableBoolean)1;
		this.cmbItemclass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemclass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemclass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemclass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemclass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemclass).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemclass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemclass).Location = new System.Drawing.Point(350, 14);
		((System.Windows.Forms.Control)(object)this.cmbItemclass).Name = "cmbItemclass";
		((System.Windows.Forms.Control)(object)this.cmbItemclass).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemclass).TabIndex = 5;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(257, 104);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 172;
		this.label16.Text = "Item Type";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(1029, 98);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(61, 26);
		this.btnShow.TabIndex = 18;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(89, 72);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(164, 23);
		this.txtToDate.TabIndex = 2;
		this.cmbActivity.AllowNull = (DefaultableBoolean)1;
		this.cmbActivity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbActivity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Location = new System.Drawing.Point(901, 70);
		((System.Windows.Forms.Control)(object)this.cmbActivity).Name = "cmbActivity";
		((System.Windows.Forms.Control)(object)this.cmbActivity).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivity).TabIndex = 15;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(602, 98);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 12;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(837, 76);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(47, 15);
		this.label12.TabIndex = 163;
		this.label12.Text = "Activity";
		this.cmbItemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Location = new System.Drawing.Point(350, 98);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Name = "cmbItemtype";
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).TabIndex = 8;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(528, 104);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(67, 15);
		this.label13.TabIndex = 165;
		this.label13.Text = "Item Name";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 76);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 3;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 104);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 5;
		this.label3.Text = "From && To No";
		this.txtFromDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFromDocNo.Location = new System.Drawing.Point(89, 100);
		this.txtFromDocNo.Name = "txtFromDocNo";
		this.txtFromDocNo.Size = new System.Drawing.Size(79, 23);
		this.txtFromDocNo.TabIndex = 3;
		this.txtFromDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNo_KeyPress);
		this.txtToDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtToDocNo.Location = new System.Drawing.Point(170, 100);
		this.txtToDocNo.Name = "txtToDocNo";
		this.txtToDocNo.Size = new System.Drawing.Size(83, 23);
		this.txtToDocNo.TabIndex = 4;
		this.txtToDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNo_KeyPress);
		this.cmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Location = new System.Drawing.Point(602, 14);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Name = "cmbWareHouse";
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).TabIndex = 9;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(528, 20);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 161;
		this.label11.Text = "WareHouse";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(257, 48);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(91, 15);
		this.label6.TabIndex = 151;
		this.label6.Text = "Parent Category";
		this.cmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Location = new System.Drawing.Point(602, 70);
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Name = "cmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).TabIndex = 11;
		this.cmbItemCatgory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCatgory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCatgory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCatgory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCatgory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCatgory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemCatgory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).Location = new System.Drawing.Point(350, 70);
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).Name = "cmbItemCatgory";
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).TabIndex = 7;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(528, 76);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(69, 15);
		this.label10.TabIndex = 159;
		this.label10.Text = "Party Name";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(257, 76);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(82, 15);
		this.label15.TabIndex = 151;
		this.label15.Text = "Item Category";
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(602, 42);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 10;
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(350, 42);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 6;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(528, 48);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(43, 15);
		this.label9.TabIndex = 157;
		this.label9.Text = "JobLot";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.lblCaption);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1104, 29);
		this.panel5.TabIndex = 166;
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(7, 5);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(157, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Purchase Register";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1104, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.btnPrint, this.btnShortcut });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1104, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortcut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcut.Image = (System.Drawing.Image)resources.GetObject("btnShortcut.Image");
		this.btnShortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcut.Name = "btnShortcut";
		this.btnShortcut.Size = new System.Drawing.Size(106, 24);
		this.btnShortcut.Text = "ShortCut Keys";
		this.btnShortcut.Click += new System.EventHandler(btnShortcut_Click);
		this.RadSaleReturn.AutoSize = true;
		this.RadSaleReturn.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.RadSaleReturn.Location = new System.Drawing.Point(948, 102);
		this.RadSaleReturn.Name = "RadSaleReturn";
		this.RadSaleReturn.Size = new System.Drawing.Size(76, 18);
		this.RadSaleReturn.TabIndex = 17;
		this.RadSaleReturn.Text = "Returns";
		this.RadSaleReturn.UseVisualStyleBackColor = true;
		this.RadSales.AutoSize = true;
		this.RadSales.Checked = true;
		this.RadSales.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.RadSales.Location = new System.Drawing.Point(840, 102);
		this.RadSales.Name = "RadSales";
		this.RadSales.Size = new System.Drawing.Size(93, 18);
		this.RadSales.TabIndex = 16;
		this.RadSales.TabStop = true;
		this.RadSales.Text = "Purchases";
		this.RadSales.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1104, 461);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "Purchase_EvaulationDetailReports";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEvaulationDetailSalesReports_FormClosing);
		base.Load += new System.EventHandler(frmEvaulationDetailSalesReports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmEvaulationDetailSalesReports_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdistrict).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemclass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCatgory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
