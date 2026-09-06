// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

public class Sales_EvaulationDetailReports : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtSalesRegister = new DataTable();

	private DataTable dtSalesSummaryByVehicles = new DataTable();

	private DataTable dtSalesSummaryByItem = new DataTable();

	private DataTable dtSalesSummaryByItemandCity = new DataTable();

	private DataTable dtSalesSummaryByItemandVarient = new DataTable();

	private DataTable dtSalesSummaryByItemNWarehouse = new DataTable();

	private DataTable dtSalesSummaryByItemNVarientCity = new DataTable();

	private DataTable dtSalesSummaryByCustomer = new DataTable();

	private DataTable dtSalesSummaryByCustomerandItem = new DataTable();

	private DataTable dtSalesSummaryByCustomerandCity = new DataTable();

	private DataTable dtSalesSummaryByCustomerandVareient = new DataTable();

	private DataTable dtSalesSummaryByCustomerItemandVareient = new DataTable();

	private DataTable dtSalesSummaryByCustomerItemandCity = new DataTable();

	private DataTable dtSalesSummaryByCity = new DataTable();

	private DataTable dtSalesSummaryByParentCategory = new DataTable();

	private DataTable dtSalesSummaryByItemAndParentCategory = new DataTable();

	private DataTable dtSalesSummaryByCustomerAndParentCategory = new DataTable();

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

	internal UltraCombo CmbVehicleNo;

	private Label label7;

	public Sales_EvaulationDetailReports(UserAccount userAccount)
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
			base.AutoScrollMinSize = new Size(1220, 500);
			dtSalesRegister.Columns.Add("DocDate", typeof(DateTime));
			dtSalesRegister.Columns.Add("DocCodeNo", typeof(string));
			dtSalesRegister.Columns.Add("Customer", typeof(string));
			dtSalesRegister.Columns.Add("ItemName", typeof(string));
			dtSalesRegister.Columns.Add("Varient", typeof(string));
			dtSalesRegister.Columns.Add("JobLotCode", typeof(string));
			dtSalesRegister.Columns.Add("VehicleNo", typeof(string));
			dtSalesRegister.Columns.Add("ItemQty", typeof(double));
			dtSalesRegister.Columns.Add("BillWeight", typeof(double));
			dtSalesRegister.Columns.Add("Amount", typeof(double));
			dtSalesRegister.Columns.Add("AvgRate", typeof(double));
			dtSalesRegister.Columns.Add("PrctByAmount", typeof(double));
			dtSalesRegister.Columns.Add("PrctByWeight", typeof(double));
			dtSalesRegister.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByVehicles.Columns.Add("Id", typeof(int));
			dtSalesSummaryByVehicles.Columns.Add("DocumentTypeId", typeof(int));
			dtSalesSummaryByVehicles.Columns.Add("DocumentType", typeof(string));
			dtSalesSummaryByVehicles.Columns.Add("InvoiceDate", typeof(DateTime));
			dtSalesSummaryByVehicles.Columns.Add("InvoiceNo", typeof(string));
			dtSalesSummaryByVehicles.Columns.Add("Customer", typeof(string));
			dtSalesSummaryByVehicles.Columns.Add("ReferenceNo", typeof(string));
			dtSalesSummaryByVehicles.Columns.Add("ReferenceParty", typeof(string));
			dtSalesSummaryByVehicles.Columns.Add("RefPartyAddress", typeof(string));
			dtSalesSummaryByVehicles.Columns.Add("GdnNo", typeof(string));
			dtSalesSummaryByVehicles.Columns.Add("Distance", typeof(string));
			dtSalesSummaryByVehicles.Columns.Add("VehicleNo", typeof(string));
			dtSalesSummaryByItem.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItem.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItem.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItem.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItem.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItemandCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByItemandVarient.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItemandVarient.Columns.Add("Varient", typeof(string));
			dtSalesSummaryByItemandVarient.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItemandVarient.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItemandVarient.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItemandVarient.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemandVarient.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItemandVarient.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("WarehouseName", typeof(string));
			dtSalesSummaryByItemNWarehouse.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItemNWarehouse.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItemNWarehouse.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemNVarientCity.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItemNVarientCity.Columns.Add("Varient", typeof(string));
			dtSalesSummaryByItemNVarientCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByItemNVarientCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByItemNVarientCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByItemNVarientCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemNVarientCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByItemNVarientCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByItemNVarientCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCustomer.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomer.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerandItem.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByCustomerandItem.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerItemandVareient.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerItemandVareient.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByCustomerItemandVareient.Columns.Add("Varient", typeof(string));
			dtSalesSummaryByCustomerItemandVareient.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerItemandVareient.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerItemandVareient.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerItemandVareient.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerItemandVareient.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerItemandVareient.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerandCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCustomerandVareient.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerandVareient.Columns.Add("Varient", typeof(string));
			dtSalesSummaryByCustomerandVareient.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerandVareient.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerandVareient.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerandVareient.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandVareient.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerandVareient.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCity.Columns.Add("PrctByWeight", typeof(double));
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
				ReportType = "Sale"
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtParent = new DataTable();
			dtParent.Columns.Add("Id");
			dtParent.Columns.Add("RefName");
			DataTable dtItemType = new DataTable();
			dtItemType.Columns.Add("Id");
			dtItemType.Columns.Add("RefName");
			DataTable dtClass = new DataTable();
			dtClass.Columns.Add("Id");
			dtClass.Columns.Add("RefName");
			DataTable dtCategory = new DataTable();
			dtCategory.Columns.Add("Id");
			dtCategory.Columns.Add("RefName");
			DataTable dtSupplierCustomer = new DataTable();
			dtSupplierCustomer.Columns.Add("Id");
			dtSupplierCustomer.Columns.Add("RefName");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("RefName");
			DataTable dtCity = new DataTable();
			dtCity.Columns.Add("Id");
			dtCity.Columns.Add("RefName");
			DataTable dtJobLot = new DataTable();
			dtJobLot.Columns.Add("Id");
			dtJobLot.Columns.Add("RefName");
			DataTable dtWareHouse = new DataTable();
			dtWareHouse.Columns.Add("Id");
			dtWareHouse.Columns.Add("RefName");
			DataTable dtVehicle = new DataTable();
			dtVehicle.Columns.Add("Id");
			dtVehicle.Columns.Add("RefName");
			DataTable dtDistrict = new DataTable();
			dtDistrict.Columns.Add("Id");
			dtDistrict.Columns.Add("RefName");
			foreach (DataRow r in dt.Rows)
			{
				if (Conversion.ToString(r["Activity"]) == "GetParentCategory")
				{
					dtParent.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetItemType")
				{
					dtItemType.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetItemClass")
				{
					dtClass.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetItemCategory")
				{
					dtCategory.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetSupplierCustomer")
				{
					dtSupplierCustomer.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetItems")
				{
					dtItem.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetCity")
				{
					dtCity.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetJobLot")
				{
					dtJobLot.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetWarehouse")
				{
					dtWareHouse.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetVehicleNos")
				{
					dtVehicle.Rows.Add(r["Id"], r["RefName"]);
				}
				if (Conversion.ToString(r["Activity"]) == "GetDistrict")
				{
					dtDistrict.Rows.Add(r["Id"], r["RefName"]);
				}
			}
			if (dtParent.Rows.Count > 0)
			{
				DDL.BindDDL(dtParent, CmbParentCategory, "Id", "RefName", "Parent Category", ZeroIndex: false);
				((UltraGridBase)CmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemType.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemType, cmbItemtype, "Id", "RefName", "Item Type", ZeroIndex: false);
				((UltraGridBase)cmbItemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtClass.Rows.Count > 0)
			{
				DDL.BindDDL(dtClass, cmbItemclass, "Id", "RefName", "Class Group Name", ZeroIndex: false);
				((UltraGridBase)cmbItemclass).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtCategory.Rows.Count > 0)
			{
				DDL.BindDDL(dtCategory, cmbItemCatgory, "Id", "RefName", "Item Category", ZeroIndex: false);
				((UltraGridBase)cmbItemCatgory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtSupplierCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplierCustomer, cmbSupplierCustomer, "Id", "RefName", "Party Name", ZeroIndex: false);
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "RefName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtCity.Rows.Count > 0)
			{
				DDL.BindDDL(dtCity, cmbCity, "Id", "RefName", "CityName", ZeroIndex: false);
			}
			if (dtJobLot.Rows.Count > 0)
			{
				DDL.BindDDL(dtJobLot, cmbJobLot, "Id", "RefName", "JobLot", ZeroIndex: false);
				((UltraGridBase)cmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtWareHouse.Rows.Count > 0)
			{
				DDL.BindDDL(dtWareHouse, cmbWareHouse, "Id", "RefName", "WareHouse", ZeroIndex: false);
				((UltraGridBase)cmbWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtVehicle.Rows.Count > 0)
			{
				DDL.BindDDL(dtVehicle, CmbVehicleNo, "Id", "RefName", "Vehicle No", ZeroIndex: false);
			}
			if (dtDistrict.Rows.Count > 0)
			{
				DDL.BindDDL(dtDistrict, cmbdistrict, "Id", "RefName", "District", ZeroIndex: false);
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
			dt.Rows.Add(1, "Sales Register");
			dt.Rows.Add(2, "Sales Summary By Vehicles");
			dt.Rows.Add(3, "Sales Summary By Item");
			dt.Rows.Add(4, "Sales Summary By Item & City");
			dt.Rows.Add(5, "Sales Summary By Item & Varient");
			dt.Rows.Add(6, "Sales Summary By Item & Warehouse");
			dt.Rows.Add(7, "Sales Summary By Item,Varient & City");
			dt.Rows.Add(8, "Sales Summary By Customer");
			dt.Rows.Add(9, "Sales Summary By Customer & Item");
			dt.Rows.Add(10, "Sales Summary By Customer & City");
			dt.Rows.Add(11, "Sales Summary By Customer & Varient");
			dt.Rows.Add(12, "Sales Summary By Customer,Item & Varient");
			dt.Rows.Add(13, "Sales Summary By Customer,Item & City");
			dt.Rows.Add(14, "Sales Summary By City");
			dt.Rows.Add(15, "Sales Summary By Parent Category");
			dt.Rows.Add(16, "Sales Summary By Parent Category & Item");
			dt.Rows.Add(17, "Sales Summary By Parent Category & Customer");
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
				MessageBox.Show("Please Select on Value Either Sales or Return");
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
			if (Conversion.ToInt(CmbVehicleNo.Value) > 0)
			{
				obj.VehicleNo = Conversion.ToString((object)((Control)(object)CmbVehicleNo).Text);
			}
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
			dtGrid = InvSaleInvoice.EvaulationDetailSalesReports(obj);
			if (dtGrid.Rows.Count > 0)
			{
				if (((UltraGridBase)cmbActivity).ActiveRow != null)
				{
					if (((Control)(object)cmbActivity).Text == "Sales Register")
					{
						dtSalesRegister.Rows.Clear();
						for (int i = 0; i < dtGrid.Rows.Count; i++)
						{
							dtSalesRegister.Rows.Add(Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), dtGrid.Rows[i]["DocCodeNo"], dtGrid.Rows[i]["Customer"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["UOMCode"], dtGrid.Rows[i]["JobLotCode"], dtGrid.Rows[i]["VehicleNo"], dtGrid.Rows[i]["QtyOut"], dtGrid.Rows[i]["BillWeightOut"], dtGrid.Rows[i]["AmountOut"], dtGrid.Rows[i]["AvgRate"], dtGrid.Rows[i]["PrcntOfTotal"], dtGrid.Rows[i]["PrcntOfTotalWeight"], dtGrid.Rows[i]["CityName"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesRegister";
						DataGridHistory.DataSource = dtSalesRegister;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Vehicles")
					{
						dtSalesSummaryByVehicles.Rows.Clear();
						for (int j = 0; j < dtGrid.Rows.Count; j++)
						{
							dtSalesSummaryByVehicles.Rows.Add(dtGrid.Rows[j]["RefDocIdNo"], dtGrid.Rows[j]["RefDocumentTypeId"], dtGrid.Rows[j]["DocumentTypeCode"], dtGrid.Rows[j]["DocDate"], dtGrid.Rows[j]["DocCodeNo"], dtGrid.Rows[j]["Customer"], dtGrid.Rows[j]["ReferenceNo"], dtGrid.Rows[j]["ReferencePartyName"], dtGrid.Rows[j]["RefPartyAddress"], dtGrid.Rows[j]["GdnNo"], dtGrid.Rows[j]["Distance"], dtGrid.Rows[j]["VehicleNo"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByVehicles";
						DataGridHistory.DataSource = dtSalesSummaryByVehicles;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item")
					{
						dtSalesSummaryByItem.Rows.Clear();
						for (int k = 0; k < dtGrid.Rows.Count; k++)
						{
							dtSalesSummaryByItem.Rows.Add(dtGrid.Rows[k]["ItemName"], dtGrid.Rows[k]["QtyOut"], dtGrid.Rows[k]["BillWeightOut"], dtGrid.Rows[k]["AmountOut"], dtGrid.Rows[k]["AvgRate"], dtGrid.Rows[k]["PrcntOfTotal"], dtGrid.Rows[k]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByItem";
						DataGridHistory.DataSource = dtSalesSummaryByItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & Varient")
					{
						dtSalesSummaryByItemandVarient.Rows.Clear();
						for (int l = 0; l < dtGrid.Rows.Count; l++)
						{
							dtSalesSummaryByItemandVarient.Rows.Add(dtGrid.Rows[l]["ItemName"], dtGrid.Rows[l]["UOMCode"], dtGrid.Rows[l]["QtyOut"], dtGrid.Rows[l]["BillWeightOut"], dtGrid.Rows[l]["AmountOut"], dtGrid.Rows[l]["AvgRate"], dtGrid.Rows[l]["PrcntOfTotal"], dtGrid.Rows[l]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByItemandVarient";
						DataGridHistory.DataSource = dtSalesSummaryByItemandVarient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer,Item & Varient")
					{
						dtSalesSummaryByCustomerItemandVareient.Rows.Clear();
						for (int m = 0; m < dtGrid.Rows.Count; m++)
						{
							dtSalesSummaryByCustomerItemandVareient.Rows.Add(dtGrid.Rows[m]["Customer"], dtGrid.Rows[m]["ItemName"], dtGrid.Rows[m]["UOMCode"], dtGrid.Rows[m]["QtyOut"], dtGrid.Rows[m]["BillWeightOut"], dtGrid.Rows[m]["AmountOut"], dtGrid.Rows[m]["AvgRate"], dtGrid.Rows[m]["PrcntOfTotal"], dtGrid.Rows[m]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerItemandVareient";
						DataGridHistory.DataSource = dtSalesSummaryByCustomerItemandVareient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & Warehouse")
					{
						dtSalesSummaryByItemNWarehouse.Rows.Clear();
						for (int n = 0; n < dtGrid.Rows.Count; n++)
						{
							dtSalesSummaryByItemNWarehouse.Rows.Add(dtGrid.Rows[n]["WarehouseName"], dtGrid.Rows[n]["ItemName"], dtGrid.Rows[n]["QtyOut"], dtGrid.Rows[n]["BillWeightOut"], dtGrid.Rows[n]["AmountOut"], dtGrid.Rows[n]["AvgRate"], dtGrid.Rows[n]["PrcntOfTotal"], dtGrid.Rows[n]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByItemNWarehouse";
						DataGridHistory.DataSource = dtSalesSummaryByItemNWarehouse;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & City")
					{
						dtSalesSummaryByItemandCity.Rows.Clear();
						for (int i2 = 0; i2 < dtGrid.Rows.Count; i2++)
						{
							dtSalesSummaryByItemandCity.Rows.Add(dtGrid.Rows[i2]["ItemName"], dtGrid.Rows[i2]["QtyOut"], dtGrid.Rows[i2]["BillWeightOut"], dtGrid.Rows[i2]["AmountOut"], dtGrid.Rows[i2]["AvgRate"], dtGrid.Rows[i2]["PrcntOfTotal"], dtGrid.Rows[i2]["PrcntOfTotalWeight"], dtGrid.Rows[i2]["CityName"]);
						}
						DataGridHistory.DataSource = dtSalesSummaryByItemandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item,Varient & City")
					{
						dtSalesSummaryByItemNVarientCity.Rows.Clear();
						for (int i3 = 0; i3 < dtGrid.Rows.Count; i3++)
						{
							dtSalesSummaryByItemNVarientCity.Rows.Add(dtGrid.Rows[i3]["ItemName"], dtGrid.Rows[i3]["UOMCode"], dtGrid.Rows[i3]["QtyOut"], dtGrid.Rows[i3]["BillWeightOut"], dtGrid.Rows[i3]["AmountOut"], dtGrid.Rows[i3]["AvgRate"], dtGrid.Rows[i3]["PrcntOfTotal"], dtGrid.Rows[i3]["PrcntOfTotalWeight"], dtGrid.Rows[i3]["CityName"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByItemNVarientCity";
						DataGridHistory.DataSource = dtSalesSummaryByItemNVarientCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer")
					{
						dtSalesSummaryByCustomer.Rows.Clear();
						for (int i4 = 0; i4 < dtGrid.Rows.Count; i4++)
						{
							dtSalesSummaryByCustomer.Rows.Add(dtGrid.Rows[i4]["Customer"], dtGrid.Rows[i4]["QtyOut"], dtGrid.Rows[i4]["BillWeightOut"], dtGrid.Rows[i4]["AmountOut"], dtGrid.Rows[i4]["AvgRate"], dtGrid.Rows[i4]["PrcntOfTotal"], dtGrid.Rows[i4]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomer";
						DataGridHistory.DataSource = dtSalesSummaryByCustomer;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & Item")
					{
						dtSalesSummaryByCustomerandItem.Rows.Clear();
						for (int i5 = 0; i5 < dtGrid.Rows.Count; i5++)
						{
							dtSalesSummaryByCustomerandItem.Rows.Add(dtGrid.Rows[i5]["Customer"], dtGrid.Rows[i5]["ItemName"], dtGrid.Rows[i5]["QtyOut"], dtGrid.Rows[i5]["BillWeightOut"], dtGrid.Rows[i5]["AmountOut"], dtGrid.Rows[i5]["AvgRate"], dtGrid.Rows[i5]["PrcntOfTotal"], dtGrid.Rows[i5]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerandItem";
						DataGridHistory.DataSource = dtSalesSummaryByCustomerandItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & City")
					{
						dtSalesSummaryByCustomerandCity.Rows.Clear();
						for (int i6 = 0; i6 < dtGrid.Rows.Count; i6++)
						{
							dtSalesSummaryByCustomerandCity.Rows.Add(dtGrid.Rows[i6]["Customer"], dtGrid.Rows[i6]["QtyOut"], dtGrid.Rows[i6]["BillWeightOut"], dtGrid.Rows[i6]["AmountOut"], dtGrid.Rows[i6]["AvgRate"], dtGrid.Rows[i6]["PrcntOfTotal"], dtGrid.Rows[i6]["PrcntOfTotalWeight"], dtGrid.Rows[i6]["CityName"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerandCity";
						DataGridHistory.DataSource = dtSalesSummaryByCustomerandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & Varient")
					{
						dtSalesSummaryByCustomerandVareient.Rows.Clear();
						for (int i7 = 0; i7 < dtGrid.Rows.Count; i7++)
						{
							dtSalesSummaryByCustomerandVareient.Rows.Add(dtGrid.Rows[i7]["Customer"], dtGrid.Rows[i7]["UOMCode"], dtGrid.Rows[i7]["QtyOut"], dtGrid.Rows[i7]["BillWeightOut"], dtGrid.Rows[i7]["AmountOut"], dtGrid.Rows[i7]["AvgRate"], dtGrid.Rows[i7]["PrcntOfTotal"], dtGrid.Rows[i7]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerandVareient";
						DataGridHistory.DataSource = dtSalesSummaryByCustomerandVareient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer,Item & City")
					{
						dtSalesSummaryByCustomerItemandCity.Rows.Clear();
						for (int i8 = 0; i8 < dtGrid.Rows.Count; i8++)
						{
							dtSalesSummaryByCustomerItemandCity.Rows.Add(dtGrid.Rows[i8]["Customer"], dtGrid.Rows[i8]["ItemName"], dtGrid.Rows[i8]["QtyOut"], dtGrid.Rows[i8]["BillWeightOut"], dtGrid.Rows[i8]["AmountOut"], dtGrid.Rows[i8]["AvgRate"], dtGrid.Rows[i8]["PrcntOfTotal"], dtGrid.Rows[i8]["PrcntOfTotalWeight"], dtGrid.Rows[i8]["CityName"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerItemandCity";
						DataGridHistory.DataSource = dtSalesSummaryByCustomerItemandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By City")
					{
						dtSalesSummaryByCity.Rows.Clear();
						for (int i9 = 0; i9 < dtGrid.Rows.Count; i9++)
						{
							dtSalesSummaryByCity.Rows.Add(dtGrid.Rows[i9]["CityName"], dtGrid.Rows[i9]["QtyOut"], dtGrid.Rows[i9]["BillWeightOut"], dtGrid.Rows[i9]["AmountOut"], dtGrid.Rows[i9]["AvgRate"], dtGrid.Rows[i9]["PrcntOfTotal"], dtGrid.Rows[i9]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByCity";
						DataGridHistory.DataSource = dtSalesSummaryByCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category")
					{
						dtSalesSummaryByParentCategory.Rows.Clear();
						for (int i10 = 0; i10 < dtGrid.Rows.Count; i10++)
						{
							dtSalesSummaryByParentCategory.Rows.Add(dtGrid.Rows[i10]["ParentCategory"], dtGrid.Rows[i10]["QtyOut"], dtGrid.Rows[i10]["BillWeightOut"], dtGrid.Rows[i10]["AmountOut"], dtGrid.Rows[i10]["AvgRate"], dtGrid.Rows[i10]["PrcntOfTotal"], dtGrid.Rows[i10]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByParentCategory";
						DataGridHistory.DataSource = dtSalesSummaryByParentCategory;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category & Item")
					{
						dtSalesSummaryByItemAndParentCategory.Rows.Clear();
						for (int i11 = 0; i11 < dtGrid.Rows.Count; i11++)
						{
							dtSalesSummaryByItemAndParentCategory.Rows.Add(dtGrid.Rows[i11]["ItemName"], dtGrid.Rows[i11]["ParentCategory"], dtGrid.Rows[i11]["QtyOut"], dtGrid.Rows[i11]["BillWeightOut"], dtGrid.Rows[i11]["AmountOut"], dtGrid.Rows[i11]["AvgRate"], dtGrid.Rows[i11]["PrcntOfTotal"], dtGrid.Rows[i11]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByItemAndParentCategory";
						DataGridHistory.DataSource = dtSalesSummaryByItemAndParentCategory;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category & Customer")
					{
						dtSalesSummaryByCustomerAndParentCategory.Rows.Clear();
						for (int i12 = 0; i12 < dtGrid.Rows.Count; i12++)
						{
							dtSalesSummaryByCustomerAndParentCategory.Rows.Add(dtGrid.Rows[i12]["Customer"], dtGrid.Rows[i12]["ParentCategory"], dtGrid.Rows[i12]["QtyOut"], dtGrid.Rows[i12]["BillWeightOut"], dtGrid.Rows[i12]["AmountOut"], dtGrid.Rows[i12]["AvgRate"], dtGrid.Rows[i12]["PrcntOfTotal"], dtGrid.Rows[i12]["PrcntOfTotalWeight"]);
						}
						((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerAndParentCategory";
						DataGridHistory.DataSource = dtSalesSummaryByCustomerAndParentCategory;
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
			if (Conversion.ToString((object)((Control)(object)cmbActivity).Text) == "Sales Summary By Vehicles")
			{
				DataGridHistory.RootTable.Groups.Add("VehicleNo");
				DataGridHistory.RootTable.Columns["VehicleNo"].Visible = false;
				DataGridHistory.RootTable.GroupTotals = (GroupTotals)1;
				DataGridHistory.RootTable.Columns["Id"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			}
			else
			{
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
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrint.Text == "1880-SalesRegisterSummary")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGrid, "1880-SalesRegisterSummary.rpt");
			}
			else if (btnPrint.Text == "1889-SalesRegisterSummaryByItem")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtGrid, "1889-SalesRegisterSummaryByItem.rpt");
			}
			else if (btnPrint.Text == "1890-SalesSummaryByItem&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtGrid, "1890-SalesSummaryByItem&City.rpt");
			}
			else if (btnPrint.Text == "1891-SalesRegisterSummaryByItem&Varient")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtGrid, "1891-SalesRegisterSummaryByItem&Varient.rpt");
			}
			else if (btnPrint.Text == "1892-SalesRegisterSummaryByItem&Warehouse")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtGrid, "1892-SalesRegisterSummaryByItem&Warehouse.rpt");
			}
			else if (btnPrint.Text == "1893-SalesSummaryByItemPackSize&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtGrid, "1893-SalesSummaryByItemPackSize&City.rpt");
			}
			else if (btnPrint.Text == "1894-SalesRegisterSummaryByCustomer")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtGrid, "1894-SalesRegisterSummaryByCustomer.rpt");
			}
			else if (btnPrint.Text == "1895-SalesSummaryByCustomer&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtGrid, "1895-SalesSummaryByCustomer&Item.rpt");
			}
			else if (btnPrint.Text == "1896-SalesRegisterSummaryByCustomer&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtGrid, "1896-SalesRegisterSummaryByCustomer&City.rpt");
			}
			else if (btnPrint.Text == "1898-SalesSummaryByCustomerItem&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtGrid, "1898-SalesSummaryByCustomerItem&City.rpt");
			}
			else if (btnPrint.Text == "1897-SalesSummaryByCustomer&Varient")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtGrid, "1897-SalesSummaryByCustomer&Varient.rpt");
			}
			else if (btnPrint.Text == "1899-SalesSummaryByCity")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val12 = new Reporting();
				val12.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val12.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val12.ShowReportWithDataTable(dtGrid, "1899-SalesSummaryByCity.rpt");
			}
			else if (btnPrint.Text == "1881-SalesSummaryByParentCategory")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val13 = new Reporting();
				val13.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val13.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val13.ShowReportWithDataTable(dtGrid, "1881-SalesSummaryByParentCategory.rpt");
			}
			else if (btnPrint.Text == "1882-SalesSummaryByParentCategory&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val14 = new Reporting();
				val14.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val14.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val14.ShowReportWithDataTable(dtGrid, "1882-SalesSummaryByParentCategory&Item.rpt");
			}
			else if (btnPrint.Text == "1883-SalesSummaryByParentCategory&Customter")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val15 = new Reporting();
				val15.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val15.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val15.ShowReportWithDataTable(dtGrid, "1883-SalesSummaryByParentCategory&Customter.rpt");
			}
			else if (btnPrint.Text == "1884-SalesSummaryByVehicles")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val16 = new Reporting();
				val16.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val16.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val16.ShowReportWithDataTable(dtGrid, "1884-SalesSummaryByVehicles.rpt");
			}
			else if (btnPrint.Text == "1885-SalesSummaryByCustomerItemandVareient")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val17 = new Reporting();
				val17.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val17.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val17.ShowReportWithDataTable(dtGrid, "1885-SalesSummaryByCustomerItemandVareient.rpt");
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
				if (((Control)(object)cmbActivity).Text == "Sales Register")
				{
					btnPrint.Text = "1880-SalesRegisterSummary";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item")
				{
					btnPrint.Text = "1889-SalesRegisterSummaryByItem";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & City")
				{
					btnPrint.Text = "1890-SalesSummaryByItem&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & Varient")
				{
					btnPrint.Text = "1891-SalesRegisterSummaryByItem&Varient";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & Warehouse")
				{
					btnPrint.Text = "1892-SalesRegisterSummaryByItem&Warehouse";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item,Varient & City")
				{
					btnPrint.Text = "1893-SalesSummaryByItemPackSize&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer")
				{
					btnPrint.Text = "1894-SalesRegisterSummaryByCustomer";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & Item")
				{
					btnPrint.Text = "1895-SalesSummaryByCustomer&Item";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & City")
				{
					btnPrint.Text = "1896-SalesRegisterSummaryByCustomer&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer,Item & City")
				{
					btnPrint.Text = "1898-SalesSummaryByCustomerItem&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & Pack Size")
				{
					btnPrint.Text = "1897-SalesSummaryByCustomer&Varient";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By City")
				{
					btnPrint.Text = "1899-SalesSummaryByCity";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category")
				{
					btnPrint.Text = "1881-SalesSummaryByParentCategory";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category & Item")
				{
					btnPrint.Text = "1882-SalesSummaryByParentCategory&Item";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category & Customer")
				{
					btnPrint.Text = "1883-SalesSummaryByParentCategory&Customter";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Vehicles")
				{
					btnPrint.Text = "1884-SalesSummaryByVehicles";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer,Item & Varient")
				{
					btnPrint.Text = "1885-SalesSummaryByCustomerItemandVareient";
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
			if (base.ActiveControl != null && base.ActiveControl.Name == "DataGridHistory")
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
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Expected O, but got Unknown
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
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
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance146 = new Appearance();
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
		Appearance appearance82 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
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
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.pcc.Reports.Sales_EvaulationDetailReports));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RadSaleReturn = new System.Windows.Forms.RadioButton();
		this.RadSales = new System.Windows.Forms.RadioButton();
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
		this.CmbVehicleNo = new UltraCombo();
		this.cmbActivity = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
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
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNo).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1204, 461);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 192);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1204, 269);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1204, 240);
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
		this.panel6.Size = new System.Drawing.Size(1204, 29);
		this.panel6.TabIndex = 167;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1157, 2);
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
		this.panel3.Size = new System.Drawing.Size(1204, 163);
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
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivity);
		this.groupBox1.Controls.Add(this.label7);
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
		this.groupBox1.Size = new System.Drawing.Size(1192, 131);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.RadSaleReturn.AutoSize = true;
		this.RadSaleReturn.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.RadSaleReturn.Location = new System.Drawing.Point(1098, 46);
		this.RadSaleReturn.Name = "RadSaleReturn";
		this.RadSaleReturn.Size = new System.Drawing.Size(76, 18);
		this.RadSaleReturn.TabIndex = 17;
		this.RadSaleReturn.Text = "Returns";
		this.RadSaleReturn.UseVisualStyleBackColor = true;
		this.RadSales.AutoSize = true;
		this.RadSales.Checked = true;
		this.RadSales.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.RadSales.Location = new System.Drawing.Point(1098, 74);
		this.RadSales.Name = "RadSales";
		this.RadSales.Size = new System.Drawing.Size(61, 18);
		this.RadSales.TabIndex = 18;
		this.RadSales.TabStop = true;
		this.RadSales.Text = "Sales";
		this.RadSales.UseVisualStyleBackColor = true;
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
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance115).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance128).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance130;
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
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
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
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
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
		((System.Windows.Forms.Control)(object)this.cmbCity).Location = new System.Drawing.Point(900, 42);
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
		this.btnShow.Location = new System.Drawing.Point(1098, 98);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(61, 26);
		this.btnShow.TabIndex = 19;
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
		this.CmbVehicleNo.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVehicleNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Location = new System.Drawing.Point(900, 70);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Name = "CmbVehicleNo";
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).TabIndex = 15;
		this.cmbActivity.AllowNull = (DefaultableBoolean)1;
		this.cmbActivity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbActivity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance139).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Location = new System.Drawing.Point(900, 98);
		((System.Windows.Forms.Control)(object)this.cmbActivity).Name = "cmbActivity";
		((System.Windows.Forms.Control)(object)this.cmbActivity).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivity).TabIndex = 16;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(837, 76);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(64, 15);
		this.label7.TabIndex = 163;
		this.label7.Text = "Vehicle No";
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
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance83;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance138).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
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
		this.label12.Location = new System.Drawing.Point(837, 104);
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
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
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
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
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
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
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
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
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
		this.panel5.Size = new System.Drawing.Size(1204, 29);
		this.panel5.TabIndex = 166;
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(7, 5);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(126, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Sales Register";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1204, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.btnPrint, this.btnShortcut });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1204, 27);
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1204, 461);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "Sales_EvaulationDetailReports";
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
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNo).EndInit();
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
