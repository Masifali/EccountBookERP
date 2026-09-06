using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
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

namespace Architecture.WinApp.Inventory_Reports;

public class frmEvaulationDetailSalesReports : Form
{
	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

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

	private DataTable dtSalesSummaryByCustomerAndReferenceParty = new DataTable();

	private DataTable dtSalesSummaryByCustomerItemandReferenceParty = new DataTable();

	private DataTable dtSalesSummaryByCustomerandPackSize = new DataTable();

	private DataTable dtSalesSummaryByParentCategory = new DataTable();

	private DataTable dtSalesSummaryByItemAndParentCategory = new DataTable();

	private DataTable dtSalesSummaryByCustomerAndParentCategory = new DataTable();

	private DataTable dtSalesSummaryByReferenceParty = new DataTable();

	private DataTable dtSalesSummaryByReferencePartyAndCity = new DataTable();

	private DataTable dtSalesSummaryByReferencePartyAndItem = new DataTable();

	private DataTable dtSalesSummaryByReferencePartyItemAndPackSize = new DataTable();

	private DataTable dtSalesSummaryByReferencePartyAndPackSize = new DataTable();

	private DataTable dtSalesSummaryInvoiceByProfitability = new DataTable();

	private DataTable dtSalesSummaryByHsCode = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtBranch = new DataTable();

	private int[] AryColWidth;

	public DateTime FromDate;

	public DateTime ToDate;

	public string BranchesIds = "";

	private bool SaleCostingJobOrderWise;

	public bool RequestFromAnotherForm;

	public int StockAccountId;

	public int ItemId;

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

	private DateTimePicker txtToDate;

	private Label label1;

	private DateTimePicker txtDateFrom;

	private Label label3;

	private TextBox txtFromDocNo;

	private TextBox txtToDocNo;

	internal UltraCombo cmbItemCatgory;

	private Label label6;

	internal UltraCombo cmbUOM;

	private Label label7;

	internal UltraCombo cmbCropYear;

	private Label label8;

	internal UltraCombo cmbJobLot;

	private Label label9;

	internal UltraCombo cmbSupplierCustomer;

	private Label label10;

	internal UltraCombo cmbWareHouse;

	private Label label11;

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

	internal UltraCombo CmbRefernceParty;

	private Label label18;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateType;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortcut;

	private Panel panel6;

	private Label label4;

	private Label label2;

	internal UltraCombo cmbBranchName;

	public UltraCombo cmbActivity;

	internal UltraCombo CmbOtherCategory;

	private Label label20;

	internal UltraCombo cmbCustomGroup;

	private Label label21;

	internal UltraCombo CmbCgsAccount;

	private Label label22;

	internal UltraCombo CmbSaleAccount;

	private Label label34;

	internal UltraCombo CmbStockAccount;

	private Label label38;

	public frmEvaulationDetailSalesReports(UserAccount userAccount)
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
			base.AutoScrollMinSize = new Size(1230, 800);
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			dtSalesRegister.Columns.Add("Id", typeof(int));
			dtSalesRegister.Columns.Add("DocumentTypeId", typeof(int));
			dtSalesRegister.Columns.Add("BranchName", typeof(string));
			dtSalesRegister.Columns.Add("DocType", typeof(string));
			dtSalesRegister.Columns.Add("SoNo", typeof(int));
			dtSalesRegister.Columns.Add("GpNo", typeof(int));
			dtSalesRegister.Columns.Add("GdnNo", typeof(int));
			dtSalesRegister.Columns.Add("BillDate", typeof(DateTime));
			dtSalesRegister.Columns.Add("BillNo", typeof(int));
			dtSalesRegister.Columns.Add("BranchSrNo", typeof(int));
			dtSalesRegister.Columns.Add("ManualBillNo", typeof(string));
			dtSalesRegister.Columns.Add("PartyName", typeof(string));
			dtSalesRegister.Columns.Add("WareHouseName", typeof(string));
			dtSalesRegister.Columns.Add("ItemCode", typeof(string));
			dtSalesRegister.Columns.Add("ItemName", typeof(string));
			dtSalesRegister.Columns.Add("HsCode", typeof(string));
			dtSalesRegister.Columns.Add("PackUom", typeof(string));
			dtSalesRegister.Columns.Add("CropYear", typeof(string));
			dtSalesRegister.Columns.Add("JobLot", typeof(string));
			dtSalesRegister.Columns.Add("PackingType", typeof(string));
			dtSalesRegister.Columns.Add("VehicleNo", typeof(string));
			dtSalesRegister.Columns.Add("ItemQty", typeof(double));
			dtSalesRegister.Columns.Add("BillWeight", typeof(double));
			dtSalesRegister.Columns.Add("StockWeight", typeof(double));
			dtSalesRegister.Columns.Add("ItemRate", typeof(double));
			dtSalesRegister.Columns.Add("RateCut", typeof(double));
			dtSalesRegister.Columns.Add("NetRate", typeof(double));
			dtSalesRegister.Columns.Add("ItemAmountWithoutExpense", typeof(double));
			dtSalesRegister.Columns.Add("AvgRate", typeof(double));
			dtSalesRegister.Columns.Add("AvgRate40Kg", typeof(double));
			dtSalesRegister.Columns.Add("Expenses", typeof(double));
			dtSalesRegister.Columns.Add("Freight", typeof(double));
			dtSalesRegister.Columns.Add("Commission", typeof(double));
			dtSalesRegister.Columns.Add("Amount", typeof(double));
			dtSalesRegister.Columns.Add("PrctByAmount", typeof(double));
			dtSalesRegister.Columns.Add("PrctByWeight", typeof(double));
			dtSalesRegister.Columns.Add("CityName", typeof(string));
			dtSalesRegister.Columns.Add("TicketNos", typeof(string));
			dtSalesSummaryByItem.Columns.Add("ItemCode", typeof(int));
			dtSalesSummaryByItem.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByItem.Columns.Add("HsCode", typeof(string));
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
			dtSalesSummaryByItemandPackSize.Columns.Add("AvgRate40Kg", typeof(double));
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
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("AvgRate40Kg", typeof(double));
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
			dtSalesSummaryByCustomerandItem.Columns.Add("AvgRate40Kg", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCustomerandCity.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerandCity.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByCustomerandCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("AvgRate40Kg", typeof(double));
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
			dtSalesSummaryByCustomerItemandCity.Columns.Add("AvgRate40Kg", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByCustomerAndReferenceParty.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerAndReferenceParty.Columns.Add("ReferenceParty", typeof(string));
			dtSalesSummaryByCustomerAndReferenceParty.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerAndReferenceParty.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerAndReferenceParty.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerAndReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerAndReferenceParty.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerAndReferenceParty.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("ItemCode", typeof(string));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("ReferenceParty", typeof(string));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("AvgRate40Kg", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("AvgRate40Kg", typeof(double));
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
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("PartyName", typeof(string));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("ParentCategory", typeof(string));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByCustomerAndParentCategory.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("ReferenceParty", typeof(string));
			dtSalesSummaryByReferenceParty.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("ReferenceParty", typeof(string));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("ReferenceParty", typeof(string));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("ItemCode", typeof(string));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("ReferenceParty", typeof(string));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("ItemCode", typeof(string));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("AvgRate40Kg", typeof(double));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByReferencePartyItemAndPackSize.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndPackSize.Columns.Add("ReferenceParty", typeof(string));
			dtSalesSummaryByReferencePartyAndPackSize.Columns.Add("PackUom", typeof(string));
			dtSalesSummaryByReferencePartyAndPackSize.Columns.Add("ItemQty", typeof(double));
			dtSalesSummaryByReferencePartyAndPackSize.Columns.Add("BillWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndPackSize.Columns.Add("Amount", typeof(double));
			dtSalesSummaryByReferencePartyAndPackSize.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByReferencePartyAndPackSize.Columns.Add("AvgRate40Kg", typeof(double));
			dtSalesSummaryByReferencePartyAndPackSize.Columns.Add("PrctByAmount", typeof(double));
			dtSalesSummaryByReferencePartyAndPackSize.Columns.Add("PrctByWeight", typeof(double));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("Id", typeof(int));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("DocumentTypeCode", typeof(string));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("DocDate", typeof(DateTime));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("DocNo", typeof(int));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("GdnNo", typeof(int));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("ItemName", typeof(string));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("HsCode", typeof(string));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("SaleQty", typeof(decimal));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("SaleWeight", typeof(decimal));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("SaleRate", typeof(decimal));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("SaleAmount", typeof(decimal));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("JobOrderNo", typeof(string));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("RateOfJobNo", typeof(decimal));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("ProfitLossPerRate", typeof(decimal));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("ProfitLoss", typeof(decimal));
			dtSalesSummaryInvoiceByProfitability.Columns.Add("ProfitLossPercent", typeof(decimal));
			dtSalesSummaryByHsCode.Columns.Add("BranchesId", typeof(int));
			dtSalesSummaryByHsCode.Columns.Add("BranchName", typeof(string));
			dtSalesSummaryByHsCode.Columns.Add("HsCode", typeof(string));
			dtSalesSummaryByHsCode.Columns.Add("ItemQty", typeof(decimal));
			dtSalesSummaryByHsCode.Columns.Add("BillWeight", typeof(decimal));
			dtSalesSummaryByHsCode.Columns.Add("Amount", typeof(decimal));
			dtSalesSummaryByHsCode.Columns.Add("AvgRate", typeof(decimal));
			dtSalesSummaryByHsCode.Columns.Add("PrctByAmount", typeof(decimal));
			dtSalesSummaryByHsCode.Columns.Add("PrctByWeight", typeof(decimal));
			waitForm.Show(this);
			BranchesdtFillDbCall();
			BranchesBind();
			AllComboBind(AllComboDbCall());
			ActivityFill();
			ItemUOMFill();
			Datetypefill();
			if (((Control)(object)cmbActivity).Text == string.Empty)
			{
				((UltraGridBase)cmbActivity).Rows[0].Activate();
			}
			if (BranchesIds != null && BranchesIds != string.Empty)
			{
				((Control)(object)cmbBranchName).Text = string.Empty;
				string[] valuesToMatch = BranchesIds.Split(',');
				RowEnumerator enumerator = ((UltraGridBase)cmbBranchName).Rows.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						UltraGridRow row = enumerator.Current;
						string[] array = valuesToMatch;
						for (int i = 0; i < array.Length; i++)
						{
							if (Conversion.ToInt((object)array[i]) == Conversion.ToInt(row.Cells["BranchId"].Value))
							{
								row.Cells["Selected"].Value = true;
								break;
							}
						}
					}
				}
				finally
				{
					if (enumerator is IDisposable disposable)
					{
						disposable.Dispose();
					}
				}
			}
			if (!Conversion.CheckDateTimeNull(FromDate) || !Conversion.CheckDateTimeNull(ToDate) || Conversion.ToInt(cmbSupplierCustomer.Value) > 0 || Conversion.ToInt(cmbItem.Value) > 0 || StockAccountId > 0 || ItemId > 0)
			{
				txtDateFrom.Value = FromDate;
				txtToDate.Value = ToDate;
				if (StockAccountId > 0)
				{
					CmbStockAccount.Value = StockAccountId;
				}
				if (ItemId > 0)
				{
					cmbItem.Value = ItemId;
				}
				GridFill();
			}
			else
			{
				((UltraGridBase)cmbDateType).Rows[2].Activate();
			}
			PrintButtonManage();
			cmbDateType.Focus();
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

	private void Datetypefill()
	{
		try
		{
			InfragisticsHelper.BindComboDateType(cmbDateType, 0);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BranchesdtFillDbCall()
	{
		try
		{
			dtBranch.Clear();
			dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BranchesBind()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)cmbBranchName).Text);
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (dtBranch.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtBranch, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
			UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj).Key = "Selected";
			((HeaderBase)obj.Header).Caption = string.Empty;
			obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj.DataType = typeof(bool);
			((HeaderBase)obj.Header).VisiblePosition = 0;
			cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			if (string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)cmbBranchName).Text)))
			{
				if (!string.IsNullOrEmpty(BranchName))
				{
					((Control)(object)cmbBranchName).Text = BranchName;
				}
				else
				{
					((Control)(object)cmbBranchName).Text = Conversion.ToString(dtBranch.Rows[0]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable AllComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.ReportType = "Sale";
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
			return InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllComboBind(DataTable dt)
	{
		try
		{
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
				EnumerableRowCollection<DataRow> rowsCrop = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetCropYear"
					select r;
				DataTable dtCrop = (rowsCrop.Any() ? rowsCrop.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsRef = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetReferenceParties"
					select r;
				DataTable dtRef = (rowsRef.Any() ? rowsRef.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsOtherCategory = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "OtherCategory"
					select r;
				DataTable dtOtherCategory = (rowsOtherCategory.Any() ? rowsOtherCategory.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsAccountsCustomGroups = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetCustomGroups"
					select r;
				DataTable dtAccountsCustomGroups = (rowsAccountsCustomGroups.Any() ? rowsAccountsCustomGroups.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsSaleAccount = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetSalesAccount"
					select r;
				DataTable dtSaleAccount = (rowsSaleAccount.Any() ? rowsSaleAccount.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsStockAccount = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetStockAccount"
					select r;
				DataTable dtStockAccount = (rowsStockAccount.Any() ? rowsStockAccount.CopyToDataTable() : dt.Clone());
				EnumerableRowCollection<DataRow> rowsCgsAccount = from r in dt.AsEnumerable()
					where r.Field<string>("Activity") == "GetCGSAccount"
					select r;
				DataTable dtCgsAccount = (rowsCgsAccount.Any() ? rowsCgsAccount.CopyToDataTable() : dt.Clone());
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
				DDL.BindDDLNew(dtDistrict, cmbdistrict, "Id", "RefName", "District", false);
				((UltraGridBase)cmbdistrict).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtCrop, cmbCropYear, "Id", "RefName", "CropYear", false);
				((UltraGridBase)cmbCropYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtRef, CmbRefernceParty, "Id", "RefName", "Reference Party", false);
				DDL.BindDDLNew(dtOtherCategory, CmbOtherCategory, "Id", "RefName", "Other Category", false);
				((UltraGridBase)CmbOtherCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtAccountsCustomGroups, cmbCustomGroup, "Id", "RefName", "Custom Groups", false);
				((UltraGridBase)cmbCustomGroup).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtSaleAccount, CmbSaleAccount, "Id", "RefName", "Sale Account", false);
				DDL.BindDDLNew(dtStockAccount, CmbStockAccount, "Id", "RefName", "Stock Account", false);
				DDL.BindDDLNew(dtCgsAccount, CmbCgsAccount, "Id", "RefName", "CGS Account", false);
				UltraGridColumn obj = ((UltraGridBase)cmbCustomGroup).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbCustomGroup.CheckedListSettings.CheckStateMember = "Selected";
				cmbCustomGroup.LimitToList = false;
				((EditorCheckedListSettings)cmbCustomGroup.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbCustomGroup.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbCustomGroup.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemUOMFill(int ItemId = 0)
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
			dt.Rows.Add(11, "Sales Summary By Customer & ReferenceParty");
			dt.Rows.Add(12, "Sales Summary By Customer,Item & ReferenceParty");
			dt.Rows.Add(13, "Sales Summary By Customer & Pack Size");
			dt.Rows.Add(14, "Sales Summary By Parent Category");
			dt.Rows.Add(15, "Sales Summary By Parent Category & Item");
			dt.Rows.Add(16, "Sales Summary By Parent Category & Customer");
			dt.Rows.Add(17, "Sales Summary By ReferenceParty");
			dt.Rows.Add(18, "Sales Summary By ReferenceParty & City");
			dt.Rows.Add(19, "Sales Summary By ReferenceParty & Item");
			dt.Rows.Add(20, "Sales Summary By ReferenceParty,Item & Pack Size");
			dt.Rows.Add(21, "Sales Summary By ReferenceParty & Pack Size");
			dt.Rows.Add(22, "INVOICE WISE PROFITABLITY");
			dt.Rows.Add(23, "Sales Summary HsCode");
			DDL.BindDDL(dt, cmbActivity, "Id", "Name", "Activity", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbItem_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			ItemUOMFill(Conversion.ToInt(cmbItem.Value));
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

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchName).Text != string.Empty && ((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				AllComboBind(AllComboDbCall());
				return;
			}
			((Control)(object)CmbParentCategory).Text = string.Empty;
			((UltraGridBase)CmbParentCategory).DataSource = null;
			((Control)(object)cmbItemclass).Text = string.Empty;
			((UltraGridBase)cmbItemclass).DataSource = null;
			((Control)(object)cmbItemCatgory).Text = string.Empty;
			((UltraGridBase)cmbItemCatgory).DataSource = null;
			((Control)(object)cmbItemtype).Text = string.Empty;
			((UltraGridBase)cmbItemtype).DataSource = null;
			((Control)(object)cmbWareHouse).Text = string.Empty;
			((UltraGridBase)cmbWareHouse).DataSource = null;
			((Control)(object)cmbJobLot).Text = string.Empty;
			((UltraGridBase)cmbJobLot).DataSource = null;
			((Control)(object)cmbSupplierCustomer).Text = string.Empty;
			((UltraGridBase)cmbSupplierCustomer).DataSource = null;
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((UltraGridBase)cmbCropYear).DataSource = null;
			((Control)(object)cmbdistrict).Text = string.Empty;
			((UltraGridBase)cmbdistrict).DataSource = null;
			((Control)(object)cmbCity).Text = string.Empty;
			((UltraGridBase)cmbCity).DataSource = null;
			((Control)(object)CmbRefernceParty).Text = string.Empty;
			((UltraGridBase)CmbRefernceParty).DataSource = null;
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
			waitForm.Show(this);
			btnPrint.Visible = true;
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_3a75: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDate.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNo.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNo.Text);
			obj.ParentCategoryId = Conversion.ToInt(CmbParentCategory.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbItemCatgory.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierCustomer.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.PackSizeFrom = Conversion.ToInt(cmbUOM.Value);
			obj.CropYear = ((Control)(object)cmbCropYear).Text;
			obj.JobLotId = Conversion.ToInt(cmbJobLot.Value);
			obj.CityId = Conversion.ToInt(cmbCity.Value);
			obj.WarehouseId = Conversion.ToInt(cmbWareHouse.Value);
			obj.DistrictId = Conversion.ToInt(cmbdistrict.Value);
			obj.ItemClassGroupId = Conversion.ToInt(cmbItemclass.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtype.Value);
			obj.OtherCategoryId = Conversion.ToInt(CmbOtherCategory.Value);
			obj.SaleAccountId = Conversion.ToInt(CmbSaleAccount.Value);
			obj.StockAccountId = Conversion.ToInt(CmbStockAccount.Value);
			obj.CGSAccountId = Conversion.ToInt(CmbCgsAccount.Value);
			obj.ReportType = ((Control)(object)cmbActivity).Text;
			obj.RefDocSubId = Conversion.ToInt(CmbRefernceParty.Value);
			string BranchIds = "";
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
				if (Conversion.ToString((object)((Control)(object)cmbCustomGroup).Text) != string.Empty)
				{
					DataTable dtCustom = (DataTable)((UltraGridBase)cmbCustomGroup).DataSource;
					array = (Conversion.ToString((object)((Control)(object)cmbCustomGroup).Text) + ",").Split(',');
					foreach (string author in array)
					{
						DataRow[] dr = dtCustom.Select("RefName='" + Conversion.ToString((object)author) + "'");
						if (dr.Length != 0)
						{
							CustomGroupIds = CustomGroupIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
						}
					}
					obj.CustomGroupIds = CustomGroupIds;
				}
				dtGrid = InventoryStockEvalautionDetail.EvaulationDetailSalesReports(obj);
				if (dtGrid.Rows.Count > 0)
				{
					if (((Control)(object)cmbActivity).Text != null && ((Control)(object)cmbActivity).Text != string.Empty)
					{
						if (((Control)(object)cmbActivity).Text == "Sales Register")
						{
							dtSalesRegister.Rows.Clear();
							for (int i = 0; i < dtGrid.Rows.Count; i++)
							{
								dtSalesRegister.Rows.Add(dtGrid.Rows[i]["RefDocIdNo"], dtGrid.Rows[i]["RefDocumentTypeId"], dtGrid.Rows[i]["BranchName"], dtGrid.Rows[i]["DocumentTypeCode"], dtGrid.Rows[i]["SoNo"], dtGrid.Rows[i]["GpNoDcNo"], dtGrid.Rows[i]["GdnNo"], Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), dtGrid.Rows[i]["DocCodeNo"], dtGrid.Rows[i]["BranchSrNo"], dtGrid.Rows[i]["ReferenceNo"], dtGrid.Rows[i]["Customer"], dtGrid.Rows[i]["WarehouseName"], dtGrid.Rows[i]["ItemCode"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["HsCode"], dtGrid.Rows[i]["UOMCode"], dtGrid.Rows[i]["CropBatch"], dtGrid.Rows[i]["JobLotCode"], dtGrid.Rows[i]["PackTypeCode"], dtGrid.Rows[i]["VehicleNo"], dtGrid.Rows[i]["QtyOut"], dtGrid.Rows[i]["BillWeightOut"], dtGrid.Rows[i]["StockWeight"], dtGrid.Rows[i]["ItemRate"], dtGrid.Rows[i]["RateCut"], Conversion.ToDouble(dtGrid.Rows[i]["ItemRate"]) - Conversion.ToDouble(dtGrid.Rows[i]["RateCut"]), dtGrid.Rows[i]["ItemAmountWithoutExpense"], dtGrid.Rows[i]["AvgRate"], dtGrid.Rows[i]["AvgRate40Kg"], dtGrid.Rows[i]["Expenses"], dtGrid.Rows[i]["Freight"], dtGrid.Rows[i]["Commission"], dtGrid.Rows[i]["AmountOut"], dtGrid.Rows[i]["PrcntOfTotal"], dtGrid.Rows[i]["PrcntOfTotalWeight"], dtGrid.Rows[i]["CityName"], dtGrid.Rows[i]["TicketNos"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesRegister";
							DataGridHistory.DataSource = dtSalesRegister;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["BillDate"].Width = 75;
							DataGridHistory.RootTable.Columns["BillNo"].Width = 55;
							DataGridHistory.RootTable.Columns["SoNo"].Width = 55;
							DataGridHistory.RootTable.Columns["GpNo"].Width = 55;
							DataGridHistory.RootTable.Columns["GdnNo"].Width = 55;
							DataGridHistory.RootTable.Columns["DocType"].Width = 60;
							DataGridHistory.RootTable.Columns["ManualBillNo"].Width = 80;
							DataGridHistory.RootTable.Columns["PartyName"].Width = 120;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["WarehouseName"].Width = 120;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 120;
							DataGridHistory.RootTable.Columns["HsCode"].Width = 100;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistory.RootTable.Columns["CropYear"].Width = 65;
							DataGridHistory.RootTable.Columns["JobLot"].Width = 100;
							DataGridHistory.RootTable.Columns["PackingType"].Width = 90;
							DataGridHistory.RootTable.Columns["VehicleNo"].Width = 90;
							DataGridHistory.RootTable.Columns["TicketNos"].Width = 90;
							DataGridHistory.RootTable.Columns["BranchName"].Width = 120;
							DataGridHistory.RootTable.Columns["BranchSrNo"].Width = 70;
							DataGridHistory.RootTable.Columns["Expenses"].Width = 120;
							DataGridHistory.RootTable.Columns["Freight"].Width = 120;
							DataGridHistory.RootTable.Columns["Commission"].Width = 120;
							DataGridHistory.RootTable.Columns["ItemRate"].Width = 85;
							DataGridHistory.RootTable.Columns["RateCut"].Width = 75;
							DataGridHistory.RootTable.Columns["NetRate"].Width = 85;
							DataGridHistory.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
							DataGridHistory.RootTable.Columns["ItemAmountWithoutExpense"].Caption = "Item Amount";
							DataGridHistory.RootTable.Columns["Amount"].Caption = "Item Net Amount";
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item")
						{
							dtSalesSummaryByItem.Rows.Clear();
							for (int j = 0; j < dtGrid.Rows.Count; j++)
							{
								dtSalesSummaryByItem.Rows.Add(dtGrid.Rows[j]["ItemCode"], dtGrid.Rows[j]["ItemName"], dtGrid.Rows[j]["HsCode"], dtGrid.Rows[j]["QtyOut"], dtGrid.Rows[j]["BillWeightOut"], dtGrid.Rows[j]["AmountOut"], dtGrid.Rows[j]["AvgRate40Kg"], dtGrid.Rows[j]["PrcntOfTotal"], dtGrid.Rows[j]["PrcntOfTotalWeight"], dtGrid.Rows[j]["CityName"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByItem";
							DataGridHistory.DataSource = dtSalesSummaryByItem;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 150;
							DataGridHistory.RootTable.Columns["HsCode"].Width = 100;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & Pack Size")
						{
							dtSalesSummaryByItemandPackSize.Rows.Clear();
							for (int k = 0; k < dtGrid.Rows.Count; k++)
							{
								dtSalesSummaryByItemandPackSize.Rows.Add(dtGrid.Rows[k]["ItemCode"], dtGrid.Rows[k]["ItemName"], dtGrid.Rows[k]["UOMCode"], dtGrid.Rows[k]["QtyOut"], dtGrid.Rows[k]["BillWeightOut"], dtGrid.Rows[k]["AmountOut"], dtGrid.Rows[k]["AvgRate"], dtGrid.Rows[k]["AvgRate40Kg"], dtGrid.Rows[k]["PrcntOfTotal"], dtGrid.Rows[k]["PrcntOfTotalWeight"], dtGrid.Rows[k]["CityName"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByItemandPackSize";
							DataGridHistory.DataSource = dtSalesSummaryByItemandPackSize;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 150;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & Warehouse")
						{
							dtSalesSummaryByItemNWarehouse.Rows.Clear();
							for (int l = 0; l < dtGrid.Rows.Count; l++)
							{
								dtSalesSummaryByItemNWarehouse.Rows.Add(dtGrid.Rows[l]["WarehouseName"], dtGrid.Rows[l]["ItemCode"], dtGrid.Rows[l]["ItemName"], dtGrid.Rows[l]["QtyOut"], dtGrid.Rows[l]["BillWeightOut"], dtGrid.Rows[l]["AmountOut"], dtGrid.Rows[l]["AvgRate40Kg"], dtGrid.Rows[l]["PrcntOfTotal"], dtGrid.Rows[l]["PrcntOfTotalWeight"], dtGrid.Rows[l]["CityName"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByItemNWarehouse";
							DataGridHistory.DataSource = dtSalesSummaryByItemNWarehouse;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["WarehouseName"].Width = 120;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 150;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & City")
						{
							dtSalesSummaryByItemandCity.Rows.Clear();
							for (int m = 0; m < dtGrid.Rows.Count; m++)
							{
								dtSalesSummaryByItemandCity.Rows.Add(dtGrid.Rows[m]["ItemCode"], dtGrid.Rows[m]["ItemName"], dtGrid.Rows[m]["QtyOut"], dtGrid.Rows[m]["BillWeightOut"], dtGrid.Rows[m]["AmountOut"], dtGrid.Rows[m]["AvgRate40Kg"], dtGrid.Rows[m]["PrcntOfTotal"], dtGrid.Rows[m]["PrcntOfTotalWeight"], dtGrid.Rows[m]["CityName"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByItemandCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 150;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item,Pack Size & City")
						{
							dtSalesSummaryByItemNPackSizeCity.Rows.Clear();
							for (int n = 0; n < dtGrid.Rows.Count; n++)
							{
								dtSalesSummaryByItemNPackSizeCity.Rows.Add(dtGrid.Rows[n]["ItemCode"], dtGrid.Rows[n]["ItemName"], dtGrid.Rows[n]["UOMCode"], dtGrid.Rows[n]["QtyOut"], dtGrid.Rows[n]["BillWeightOut"], dtGrid.Rows[n]["AmountOut"], dtGrid.Rows[n]["AvgRate"], dtGrid.Rows[n]["AvgRate40Kg"], dtGrid.Rows[n]["PrcntOfTotal"], dtGrid.Rows[n]["PrcntOfTotalWeight"], dtGrid.Rows[n]["CityName"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByItemNPackSizeCity";
							DataGridHistory.DataSource = dtSalesSummaryByItemNPackSizeCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 150;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer")
						{
							dtSalesSummaryByCustomer.Rows.Clear();
							for (int i2 = 0; i2 < dtGrid.Rows.Count; i2++)
							{
								dtSalesSummaryByCustomer.Rows.Add(dtGrid.Rows[i2]["Customer"], dtGrid.Rows[i2]["QtyOut"], dtGrid.Rows[i2]["BillWeightOut"], dtGrid.Rows[i2]["AmountOut"], dtGrid.Rows[i2]["AvgRate40Kg"], dtGrid.Rows[i2]["PrcntOfTotal"], dtGrid.Rows[i2]["PrcntOfTotalWeight"], dtGrid.Rows[i2]["CityName"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomer";
							DataGridHistory.DataSource = dtSalesSummaryByCustomer;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = 150;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & Item")
						{
							dtSalesSummaryByCustomerandItem.Rows.Clear();
							for (int i3 = 0; i3 < dtGrid.Rows.Count; i3++)
							{
								dtSalesSummaryByCustomerandItem.Rows.Add(dtGrid.Rows[i3]["Customer"], dtGrid.Rows[i3]["ItemCode"], dtGrid.Rows[i3]["ItemName"], dtGrid.Rows[i3]["UOMCode"], dtGrid.Rows[i3]["QtyOut"], dtGrid.Rows[i3]["BillWeightOut"], dtGrid.Rows[i3]["AmountOut"], dtGrid.Rows[i3]["AvgRate"], dtGrid.Rows[i3]["AvgRate40Kg"], dtGrid.Rows[i3]["PrcntOfTotal"], dtGrid.Rows[i3]["PrcntOfTotalWeight"], dtGrid.Rows[i3]["CityName"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerandItem";
							DataGridHistory.DataSource = dtSalesSummaryByCustomerandItem;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = 150;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 120;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & City")
						{
							dtSalesSummaryByCustomerandCity.Rows.Clear();
							for (int i4 = 0; i4 < dtGrid.Rows.Count; i4++)
							{
								dtSalesSummaryByCustomerandCity.Rows.Add(dtGrid.Rows[i4]["Customer"], dtGrid.Rows[i4]["UOMCode"], dtGrid.Rows[i4]["QtyOut"], dtGrid.Rows[i4]["BillWeightOut"], dtGrid.Rows[i4]["AmountOut"], dtGrid.Rows[i4]["AvgRate"], dtGrid.Rows[i4]["AvgRate40Kg"], dtGrid.Rows[i4]["PrcntOfTotal"], dtGrid.Rows[i4]["PrcntOfTotalWeight"], dtGrid.Rows[i4]["CityName"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerandCity";
							DataGridHistory.DataSource = dtSalesSummaryByCustomerandCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = 150;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer,Item & City")
						{
							dtSalesSummaryByCustomerItemandCity.Rows.Clear();
							for (int i5 = 0; i5 < dtGrid.Rows.Count; i5++)
							{
								dtSalesSummaryByCustomerItemandCity.Rows.Add(dtGrid.Rows[i5]["Customer"], dtGrid.Rows[i5]["ItemCode"], dtGrid.Rows[i5]["ItemName"], dtGrid.Rows[i5]["UOMCode"], dtGrid.Rows[i5]["QtyOut"], dtGrid.Rows[i5]["BillWeightOut"], dtGrid.Rows[i5]["AmountOut"], dtGrid.Rows[i5]["AvgRate"], dtGrid.Rows[i5]["AvgRate40Kg"], dtGrid.Rows[i5]["PrcntOfTotal"], dtGrid.Rows[i5]["PrcntOfTotalWeight"], dtGrid.Rows[i5]["CityName"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerItemandCity";
							DataGridHistory.DataSource = dtSalesSummaryByCustomerItemandCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = 150;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 120;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & ReferenceParty")
						{
							dtSalesSummaryByCustomerAndReferenceParty.Rows.Clear();
							for (int i6 = 0; i6 < dtGrid.Rows.Count; i6++)
							{
								dtSalesSummaryByCustomerAndReferenceParty.Rows.Add(dtGrid.Rows[i6]["Customer"], dtGrid.Rows[i6]["ReferencePartyName"], dtGrid.Rows[i6]["QtyOut"], dtGrid.Rows[i6]["BillWeightOut"], dtGrid.Rows[i6]["AmountOut"], dtGrid.Rows[i6]["AvgRate40Kg"], dtGrid.Rows[i6]["PrcntOfTotal"], dtGrid.Rows[i6]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerAndReferenceParty";
							DataGridHistory.DataSource = dtSalesSummaryByCustomerAndReferenceParty;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = 150;
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = 120;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer,Item & ReferenceParty")
						{
							dtSalesSummaryByCustomerItemandReferenceParty.Rows.Clear();
							for (int i7 = 0; i7 < dtGrid.Rows.Count; i7++)
							{
								dtSalesSummaryByCustomerItemandReferenceParty.Rows.Add(dtGrid.Rows[i7]["Customer"], dtGrid.Rows[i7]["ItemCode"], dtGrid.Rows[i7]["ItemName"], dtGrid.Rows[i7]["ReferencePartyName"], dtGrid.Rows[i7]["UOMCode"], dtGrid.Rows[i7]["QtyOut"], dtGrid.Rows[i7]["BillWeightOut"], dtGrid.Rows[i7]["AmountOut"], dtGrid.Rows[i7]["AvgRate"], dtGrid.Rows[i7]["AvgRate40Kg"], dtGrid.Rows[i7]["PrcntOfTotal"], dtGrid.Rows[i7]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerItemAndReferenceParty";
							DataGridHistory.DataSource = dtSalesSummaryByCustomerItemandReferenceParty;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = 150;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 120;
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = 120;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & Pack Size")
						{
							dtSalesSummaryByCustomerandPackSize.Rows.Clear();
							for (int i8 = 0; i8 < dtGrid.Rows.Count; i8++)
							{
								dtSalesSummaryByCustomerandPackSize.Rows.Add(dtGrid.Rows[i8]["Customer"], dtGrid.Rows[i8]["UOMCode"], dtGrid.Rows[i8]["QtyOut"], dtGrid.Rows[i8]["BillWeightOut"], dtGrid.Rows[i8]["AmountOut"], dtGrid.Rows[i8]["AvgRate"], dtGrid.Rows[i8]["AvgRate40Kg"], dtGrid.Rows[i8]["PrcntOfTotal"], dtGrid.Rows[i8]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerandPackSize";
							DataGridHistory.DataSource = dtSalesSummaryByCustomerandPackSize;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = 150;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category")
						{
							dtSalesSummaryByParentCategory.Rows.Clear();
							for (int i9 = 0; i9 < dtGrid.Rows.Count; i9++)
							{
								dtSalesSummaryByParentCategory.Rows.Add(dtGrid.Rows[i9]["ParentCategory"], dtGrid.Rows[i9]["QtyOut"], dtGrid.Rows[i9]["BillWeightOut"], dtGrid.Rows[i9]["AmountOut"], dtGrid.Rows[i9]["AvgRate40Kg"], dtGrid.Rows[i9]["PrcntOfTotal"], dtGrid.Rows[i9]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByParentCategory";
							DataGridHistory.DataSource = dtSalesSummaryByParentCategory;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ParentCategory"].Width = 110;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category & Item")
						{
							dtSalesSummaryByItemAndParentCategory.Rows.Clear();
							for (int i10 = 0; i10 < dtGrid.Rows.Count; i10++)
							{
								dtSalesSummaryByItemAndParentCategory.Rows.Add(dtGrid.Rows[i10]["ItemName"], dtGrid.Rows[i10]["ParentCategory"], dtGrid.Rows[i10]["QtyOut"], dtGrid.Rows[i10]["BillWeightOut"], dtGrid.Rows[i10]["AmountOut"], dtGrid.Rows[i10]["AvgRate40Kg"], dtGrid.Rows[i10]["PrcntOfTotal"], dtGrid.Rows[i10]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByItemAndParentCategory";
							DataGridHistory.DataSource = dtSalesSummaryByItemAndParentCategory;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ParentCategory"].Width = 110;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 150;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category & Customer")
						{
							dtSalesSummaryByCustomerAndParentCategory.Rows.Clear();
							for (int i11 = 0; i11 < dtGrid.Rows.Count; i11++)
							{
								dtSalesSummaryByCustomerAndParentCategory.Rows.Add(dtGrid.Rows[i11]["Customer"], dtGrid.Rows[i11]["ParentCategory"], dtGrid.Rows[i11]["QtyOut"], dtGrid.Rows[i11]["BillWeightOut"], dtGrid.Rows[i11]["AmountOut"], dtGrid.Rows[i11]["AvgRate40Kg"], dtGrid.Rows[i11]["PrcntOfTotal"], dtGrid.Rows[i11]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByCustomerAndParentCategory";
							DataGridHistory.DataSource = dtSalesSummaryByCustomerAndParentCategory;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = 150;
							DataGridHistory.RootTable.Columns["ParentCategory"].Width = 110;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty")
						{
							dtSalesSummaryByReferenceParty.Rows.Clear();
							for (int i12 = 0; i12 < dtGrid.Rows.Count; i12++)
							{
								dtSalesSummaryByReferenceParty.Rows.Add(dtGrid.Rows[i12]["ReferencePartyName"], dtGrid.Rows[i12]["QtyOut"], dtGrid.Rows[i12]["BillWeightOut"], dtGrid.Rows[i12]["AmountOut"], dtGrid.Rows[i12]["AvgRate40Kg"], dtGrid.Rows[i12]["PrcntOfTotal"], dtGrid.Rows[i12]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByReferenceParty";
							DataGridHistory.DataSource = dtSalesSummaryByReferenceParty;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = 150;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty & City")
						{
							dtSalesSummaryByReferencePartyAndCity.Rows.Clear();
							for (int i13 = 0; i13 < dtGrid.Rows.Count; i13++)
							{
								dtSalesSummaryByReferencePartyAndCity.Rows.Add(dtGrid.Rows[i13]["ReferencePartyName"], dtGrid.Rows[i13]["CityName"], dtGrid.Rows[i13]["QtyOut"], dtGrid.Rows[i13]["BillWeightOut"], dtGrid.Rows[i13]["AmountOut"], dtGrid.Rows[i13]["AvgRate40Kg"], dtGrid.Rows[i13]["PrcntOfTotal"], dtGrid.Rows[i13]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByReferencePartyAndCity";
							DataGridHistory.DataSource = dtSalesSummaryByReferencePartyAndCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = 150;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty & Item")
						{
							dtSalesSummaryByReferencePartyAndItem.Rows.Clear();
							for (int i14 = 0; i14 < dtGrid.Rows.Count; i14++)
							{
								dtSalesSummaryByReferencePartyAndItem.Rows.Add(dtGrid.Rows[i14]["ReferencePartyName"], dtGrid.Rows[i14]["ItemCode"], dtGrid.Rows[i14]["ItemName"], dtGrid.Rows[i14]["QtyOut"], dtGrid.Rows[i14]["BillWeightOut"], dtGrid.Rows[i14]["AmountOut"], dtGrid.Rows[i14]["AvgRate40Kg"], dtGrid.Rows[i14]["PrcntOfTotal"], dtGrid.Rows[i14]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByReferencePartyAndItem";
							DataGridHistory.DataSource = dtSalesSummaryByReferencePartyAndItem;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = 150;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 120;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty,Item & Pack Size")
						{
							dtSalesSummaryByReferencePartyItemAndPackSize.Rows.Clear();
							for (int i15 = 0; i15 < dtGrid.Rows.Count; i15++)
							{
								dtSalesSummaryByReferencePartyItemAndPackSize.Rows.Add(dtGrid.Rows[i15]["ReferencePartyName"], dtGrid.Rows[i15]["ItemCode"], dtGrid.Rows[i15]["ItemName"], dtGrid.Rows[i15]["UOMCode"], dtGrid.Rows[i15]["QtyOut"], dtGrid.Rows[i15]["BillWeightOut"], dtGrid.Rows[i15]["AmountOut"], dtGrid.Rows[i15]["AvgRate"], dtGrid.Rows[i15]["AvgRate40Kg"], dtGrid.Rows[i15]["PrcntOfTotal"], dtGrid.Rows[i15]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByReferencePartyItemAndPackSize";
							DataGridHistory.DataSource = dtSalesSummaryByReferencePartyItemAndPackSize;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = 150;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = 65;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 150;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty & Pack Size")
						{
							dtSalesSummaryByReferencePartyAndPackSize.Rows.Clear();
							for (int i16 = 0; i16 < dtGrid.Rows.Count; i16++)
							{
								dtSalesSummaryByReferencePartyAndPackSize.Rows.Add(dtGrid.Rows[i16]["ReferencePartyName"], dtGrid.Rows[i16]["UOMCode"], dtGrid.Rows[i16]["QtyOut"], dtGrid.Rows[i16]["BillWeightOut"], dtGrid.Rows[i16]["AmountOut"], dtGrid.Rows[i16]["AvgRate"], dtGrid.Rows[i16]["AvgRate40Kg"], dtGrid.Rows[i16]["PrcntOfTotal"], dtGrid.Rows[i16]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByReferencePartyAndPackSize";
							DataGridHistory.DataSource = dtSalesSummaryByReferencePartyAndPackSize;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = 150;
							DataGridHistory.RootTable.Columns["PackUom"].Width = 65;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivity).Text == "INVOICE WISE PROFITABLITY")
						{
							dtSalesSummaryInvoiceByProfitability.Rows.Clear();
							for (int i17 = 0; i17 < dtGrid.Rows.Count; i17++)
							{
								dtSalesSummaryInvoiceByProfitability.Rows.Add(dtGrid.Rows[i17]["RefDocIdNo"], dtGrid.Rows[i17]["DocumentTypeCode"], dtGrid.Rows[i17]["DocDate"], dtGrid.Rows[i17]["DocCodeNo"], dtGrid.Rows[i17]["GdnNo"], dtGrid.Rows[i17]["ItemName"], dtGrid.Rows[i17]["HsCode"], dtGrid.Rows[i17]["QtyOut"], dtGrid.Rows[i17]["BillWeightOut"], dtGrid.Rows[i17]["AvgRate"], dtGrid.Rows[i17]["ItemAmountWithoutExpense"], dtGrid.Rows[i17]["JobOrderNo"], dtGrid.Rows[i17]["CgsRate"], dtGrid.Rows[i17]["ProfitLossPerRate"], dtGrid.Rows[i17]["ProfitLoss"], dtGrid.Rows[i17]["ProfitLossPercent"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryInvoiceWiseProfitability";
							DataGridHistory.DataSource = dtSalesSummaryInvoiceByProfitability;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["Id"].Visible = false;
							DataGridHistory.RootTable.Columns["JobOrderNo"].Visible = SaleCostingJobOrderWise;
							DataGridHistory.RootTable.Columns["RateOfJobNo"].Caption = (SaleCostingJobOrderWise ? "RateOfJobNo" : "CgsRate");
							DataGridHistory.RootTable.HeaderLines = 2;
							foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
							{
								item.HeaderAlignment = (TextAlignment)2;
							}
							DataGridHistory.RootTable.Columns["DocumentTypeCode"].Width = 65;
							DataGridHistory.RootTable.Columns["DocumentTypeCode"].Caption = "Doc Type";
							DataGridHistory.RootTable.Columns["DocDate"].Width = 85;
							DataGridHistory.RootTable.Columns["DocNo"].Width = 70;
							DataGridHistory.RootTable.Columns["GdnNo"].Width = 70;
							DataGridHistory.RootTable.Columns["ItemName"].Width = 170;
							DataGridHistory.RootTable.Columns["HsCode"].Width = 100;
							DataGridHistory.RootTable.Columns["SaleQty"].Width = 70;
							DataGridHistory.RootTable.Columns["SaleWeight"].Width = 80;
							DataGridHistory.RootTable.Columns["SaleRate"].Width = 70;
							DataGridHistory.RootTable.Columns["SaleAmount"].Width = 70;
							DataGridHistory.RootTable.Columns["JobOrderNo"].Width = 70;
							DataGridHistory.RootTable.Columns["RateOfJobNo"].Width = 100;
							DataGridHistory.RootTable.Columns["ProfitLossPerRate"].Width = 90;
							DataGridHistory.RootTable.Columns["ProfitLoss"].Width = 90;
							DataGridHistory.RootTable.Columns["ProfitLossPercent"].Width = 90;
							DataGridHistory.RootTable.Columns["SaleRate"].TextAlignment = (TextAlignment)3;
							DataGridHistory.RootTable.Columns["SaleRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
							DataGridHistory.RootTable.Columns["SaleQty"].AggregateFunction = (AggregateFunction)2;
							DataGridHistory.RootTable.Columns["SaleQty"].TextAlignment = (TextAlignment)3;
							DataGridHistory.RootTable.Columns["SaleQty"].FormatString = "#,##0.###";
							DataGridHistory.RootTable.Columns["SaleQty"].TotalFormatString = "#,##0.###";
							DataGridHistory.RootTable.Columns["SaleWeight"].AggregateFunction = (AggregateFunction)2;
							DataGridHistory.RootTable.Columns["SaleWeight"].TextAlignment = (TextAlignment)3;
							DataGridHistory.RootTable.Columns["SaleWeight"].FormatString = "#,##0.###";
							DataGridHistory.RootTable.Columns["SaleWeight"].TotalFormatString = "#,##0.###";
							DataGridHistory.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
							DataGridHistory.RootTable.Columns["SaleAmount"].TextAlignment = (TextAlignment)3;
							DataGridHistory.RootTable.Columns["SaleAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
							DataGridHistory.RootTable.Columns["SaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
							DataGridHistory.RootTable.Columns["RateOfJobNo"].TextAlignment = (TextAlignment)3;
							DataGridHistory.RootTable.Columns["RateOfJobNo"].FormatString = clsGlobalVariables.DecimalRateFormate;
							DataGridHistory.RootTable.Columns["ProfitLossPerRate"].AggregateFunction = (AggregateFunction)2;
							DataGridHistory.RootTable.Columns["ProfitLossPerRate"].TextAlignment = (TextAlignment)3;
							DataGridHistory.RootTable.Columns["ProfitLossPerRate"].FormatString = clsGlobalVariables.stringFormatboth;
							DataGridHistory.RootTable.Columns["ProfitLossPerRate"].TotalFormatString = clsGlobalVariables.stringFormatboth;
							DataGridHistory.RootTable.Columns["ProfitLoss"].AggregateFunction = (AggregateFunction)2;
							DataGridHistory.RootTable.Columns["ProfitLoss"].TextAlignment = (TextAlignment)3;
							DataGridHistory.RootTable.Columns["ProfitLoss"].FormatString = clsGlobalVariables.stringFormatboth;
							DataGridHistory.RootTable.Columns["ProfitLoss"].TotalFormatString = clsGlobalVariables.stringFormatboth;
							DataGridHistory.RootTable.Columns["ProfitLossPercent"].TextAlignment = (TextAlignment)3;
							DataGridHistory.RootTable.Columns["ProfitLossPercent"].FormatString = clsGlobalVariables.DecimalRateFormate;
							ctrlGrdBar1_Load(null, null);
						}
						else if (((Control)(object)cmbActivity).Text == "Sales Summary HsCode")
						{
							dtSalesSummaryByHsCode.Rows.Clear();
							for (int i18 = 0; i18 < dtGrid.Rows.Count; i18++)
							{
								dtSalesSummaryByHsCode.Rows.Add(dtGrid.Rows[i18]["BranchesId"], dtGrid.Rows[i18]["BranchName"], dtGrid.Rows[i18]["HsCode"], dtGrid.Rows[i18]["QtyOut"], dtGrid.Rows[i18]["BillWeightOut"], dtGrid.Rows[i18]["AmountOut"], dtGrid.Rows[i18]["AvgRate40Kg"], dtGrid.Rows[i18]["PrcntOfTotal"], dtGrid.Rows[i18]["PrcntOfTotalWeight"]);
							}
							((Control)(object)DataGridHistory).Name = "SalesSummaryByHsCode";
							DataGridHistory.DataSource = dtSalesSummaryByHsCode;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["BranchesId"].Visible = false;
							DataGridHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
							DataGridHistory.RootTable.Columns["HsCode"].Width = 100;
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
				return;
			}
			cmbBranchName.Focus();
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

	private void ConfigureNumericalColumn(GridEXColumn column)
	{
		if (column.Key.Contains("Amount"))
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.stringFormatsingle;
			column.TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		else if (column.Key.Contains("Rate"))
		{
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.DecimalRateFormate;
		}
		else
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = "#,##0.##";
			column.TotalFormatString = "#,##0.##";
		}
	}

	private void DataGridHistorySetting()
	{
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		try
		{
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["ItemQty"].Width = 80;
			DataGridHistory.RootTable.Columns["BillWeight"].Width = 90;
			DataGridHistory.RootTable.Columns["AvgRate"].Width = 70;
			if (DataGridHistory.RootTable.Columns.Contains("AvgRate40Kg"))
			{
				DataGridHistory.RootTable.Columns["AvgRate"].Caption = "Avg Rate PackUom";
				DataGridHistory.RootTable.Columns["AvgRate"].Width = 85;
				DataGridHistory.RootTable.Columns["AvgRate40Kg"].Width = 75;
				DataGridHistory.RootTable.Columns["AvgRate40Kg"].Caption = "Avg Rate 40Kg";
				DataGridHistory.RootTable.Columns["AvgRate40Kg"].TextAlignment = (TextAlignment)3;
				DataGridHistory.RootTable.Columns["AvgRate40Kg"].FormatString = "#,##0.###";
			}
			else
			{
				DataGridHistory.RootTable.Columns["AvgRate"].Caption = "Avg Rate 40Kg";
			}
			DataGridHistory.RootTable.Columns["Amount"].Width = 120;
			DataGridHistory.RootTable.Columns["PrctByAmount"].Width = 70;
			DataGridHistory.RootTable.Columns["PrctByWeight"].Width = 70;
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			if (((Control)(object)cmbActivity).Text == "Sales Register")
			{
				DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
				DataGridHistory.RootTable.Columns["Id"].Visible = false;
				GridEX_Helper.AddSelectButton(DataGridHistory, "Select", 20, 0);
				GridEX_Helper.AddButton(DataGridHistory, "ApprovalDetail", "Approval Detail", 100, 1);
				DataGridHistory.FrozenColumns = 2;
			}
			if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & Pack Size" || ((Control)(object)cmbActivity).Text == "Sales Summary By Item" || ((Control)(object)cmbActivity).Text == "Sales Summary By Item & Warehouse" || ((Control)(object)cmbActivity).Text == "Sales Summary By Customer" || ((Control)(object)cmbActivity).Text == "Sales Summary By Customer & Item")
			{
				DataGridHistory.RootTable.Columns["CityName"].Visible = false;
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
			waitForm.Close();
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
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrint.Text == "312-SalesRegisterSummary")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGrid, "312-SalesRegisterSummary.rpt");
			}
			else if (btnPrint.Text == "314-SalesRegisterSummaryByItem")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtGrid, "314-SalesRegisterSummaryByItem.rpt");
			}
			else if (btnPrint.Text == "316-SalesRegisterSummaryByItemWithoutPacking")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtGrid, "316-SalesRegisterSummaryByItemWithoutPacking.rpt");
			}
			else if (btnPrint.Text == "317-SalesRegisterSummaryByWarehouse")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtGrid, "317-SalesRegisterSummaryByWarehouse.rpt");
			}
			else if (btnPrint.Text == "342-SalesSummaryByItem&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtGrid, "342-SalesSummaryByItem&City.rpt");
			}
			else if (btnPrint.Text == "344-SalesSummaryByItemPackSize&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtGrid, "344-SalesSummaryByItemPackSize&City.rpt");
			}
			else if (btnPrint.Text == "313-SalesRegisterSummaryByCustomer")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtGrid, "313-SalesRegisterSummaryByCustomer.rpt");
			}
			else if (btnPrint.Text == "315-SalesRegisterSummaryByCustomer&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtGrid, "315-SalesRegisterSummaryByCustomer&Item.rpt");
			}
			else if (btnPrint.Text == "343-SalesSummaryByCustomer&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtGrid, "343-SalesSummaryByCustomer&City.rpt");
			}
			else if (btnPrint.Text == "345-SalesSummaryByCustomerItem&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtGrid, "345-SalesSummaryByCustomerItem&City.rpt");
			}
			else if (btnPrint.Text == "346-SalesSummaryByCustomer&PackSize")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtGrid, "346-SalesSummaryByCustomer&PackSize.rpt");
			}
			else if (btnPrint.Text == "347-SalesSummaryByParentCategory")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val12 = new Reporting();
				val12.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val12.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val12.ShowReportWithDataTable(dtGrid, "347-SalesSummaryByParentCategory.rpt");
			}
			else if (btnPrint.Text == "348-SalesSummaryByParentCategory&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val13 = new Reporting();
				val13.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val13.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val13.ShowReportWithDataTable(dtGrid, "348-SalesSummaryByParentCategory&Item.rpt");
			}
			else if (btnPrint.Text == "349-SalesSummaryByParentCategory&Customer")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val14 = new Reporting();
				val14.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val14.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val14.ShowReportWithDataTable(dtGrid, "349-SalesSummaryByParentCategory&Customer.rpt");
			}
			else if (btnPrint.Text == "350-SalesSummaryByReferenceParty")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val15 = new Reporting();
				val15.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val15.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val15.ShowReportWithDataTable(dtGrid, "350-SalesSummaryByReferenceParty.rpt");
			}
			else if (btnPrint.Text == "351-SalesSummaryByReferenceParty&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val16 = new Reporting();
				val16.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val16.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val16.ShowReportWithDataTable(dtGrid, "351-SalesSummaryByReferenceParty&City.rpt");
			}
			else if (btnPrint.Text == "352-SalesSummaryByReferenceParty&PackSize")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val17 = new Reporting();
				val17.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val17.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val17.ShowReportWithDataTable(dtGrid, "352-SalesSummaryByReferenceParty&PackSize.rpt");
			}
			else if (btnPrint.Text == "376-SalesSummaryByCustomer,ItemReferenceParty")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val18 = new Reporting();
				val18.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val18.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val18.ShowReportWithDataTable(dtGrid, "376-SalesSummaryByCustomer,ItemReferenceParty.rpt");
			}
			else if (btnPrint.Text == "377-SalesSummaryByReferenceParty&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val19 = new Reporting();
				val19.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val19.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val19.ShowReportWithDataTable(dtGrid, "377-SalesSummaryByReferenceParty&Item.rpt");
			}
			else if (btnPrint.Text == "378-SalesSummaryByCustomer&ReferenceParty")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val20 = new Reporting();
				val20.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val20.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val20.ShowReportWithDataTable(dtGrid, "378-SalesSummaryByCustomer&ReferenceParty.rpt");
			}
			else if (btnPrint.Text == "379-SalesSummaryByReferenceParty,Item&PackSize")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val21 = new Reporting();
				val21.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val21.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val21.ShowReportWithDataTable(dtGrid, "379-SalesSummaryByReferenceParty,Item&PackSize.rpt");
			}
			else if (btnPrint.Text == "381-SaleInvoicewiseProfitablity")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val22 = new Reporting();
				val22.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val22.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val22.ShowReportWithDataTable(dtGrid, "381-SaleInvoicewiseProfitablity.rpt");
			}
			else if (btnPrint.Text == "397-SaleSummaryByHsCode")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val23 = new Reporting();
				val23.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val23.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val23.ShowReportWithDataTable(dtGrid, "397-SaleSummaryByHsCode.rpt");
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
					btnPrint.Text = "312-SalesRegisterSummary";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item")
				{
					btnPrint.Text = "316-SalesRegisterSummaryByItemWithoutPacking";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & City")
				{
					btnPrint.Text = "342-SalesSummaryByItem&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & Pack Size")
				{
					btnPrint.Text = "314-SalesRegisterSummaryByItem";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item & Warehouse")
				{
					btnPrint.Text = "317-SalesRegisterSummaryByWarehouse";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Item,Pack Size & City")
				{
					btnPrint.Text = "344-SalesSummaryByItemPackSize&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer")
				{
					btnPrint.Text = "313-SalesRegisterSummaryByCustomer";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & Item")
				{
					btnPrint.Text = "315-SalesRegisterSummaryByCustomer&Item";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & City")
				{
					btnPrint.Text = "343-SalesSummaryByCustomer&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer,Item & City")
				{
					btnPrint.Text = "345-SalesSummaryByCustomerItem&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & Pack Size")
				{
					btnPrint.Text = "346-SalesSummaryByCustomer&PackSize";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category")
				{
					btnPrint.Text = "347-SalesSummaryByParentCategory";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category & Item")
				{
					btnPrint.Text = "348-SalesSummaryByParentCategory&Item";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Parent Category & Customer")
				{
					btnPrint.Text = "349-SalesSummaryByParentCategory&Customer";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty")
				{
					btnPrint.Text = "350-SalesSummaryByReferenceParty";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty & City")
				{
					btnPrint.Text = "351-SalesSummaryByReferenceParty&City";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty & Pack Size")
				{
					btnPrint.Text = "352-SalesSummaryByReferenceParty&PackSize";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer & ReferenceParty")
				{
					btnPrint.Text = "378-SalesSummaryByCustomer&ReferenceParty";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By Customer,Item & ReferenceParty")
				{
					btnPrint.Text = "376-SalesSummaryByCustomer,ItemReferenceParty";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty & Item")
				{
					btnPrint.Text = "377-SalesSummaryByReferenceParty&Item";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary By ReferenceParty,Item & Pack Size")
				{
					btnPrint.Text = "379-SalesSummaryByReferenceParty,Item&PackSize";
				}
				else if (((Control)(object)cmbActivity).Text == "INVOICE WISE PROFITABLITY")
				{
					btnPrint.Text = "381-SaleInvoicewiseProfitablity";
				}
				else if (((Control)(object)cmbActivity).Text == "Sales Summary HsCode")
				{
					btnPrint.Text = "397-SaleSummaryByHsCode";
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
			((Control)(object)cmbUOM).Text = string.Empty;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((Control)(object)cmbdistrict).Text = string.Empty;
			((Control)(object)cmbdistrict).Text = string.Empty;
			((Control)(object)CmbRefernceParty).Text = string.Empty;
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
			waitForm.Show(this);
			BranchesdtFillDbCall();
			BranchesBind();
			AllComboBind(AllComboDbCall());
			ItemUOMFill();
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
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
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
			if (e.Alt)
			{
				if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
				{
					if (btnPrint.Text == "312-SalesRegisterSummary")
					{
						if (dtGrid.Rows.Count == 0 || dtGrid == null)
						{
							throw new Exception("Record Not Found For Display");
						}
						Reporting val = new Reporting();
						val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						val.ShowReportWithDataTable(dtGrid, "312-SalesRegisterSummary.rpt");
					}
				}
				else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
				{
					if (btnPrint.Text == "314-SalesRegisterSummaryByItem")
					{
						if (dtGrid.Rows.Count == 0 || dtGrid == null)
						{
							throw new Exception("Record Not Found For Display");
						}
						Reporting val2 = new Reporting();
						val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						val2.ShowReportWithDataTable(dtGrid, "314-SalesRegisterSummaryByItem.rpt");
					}
				}
				else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
				{
					if (btnPrint.Text == "316-SalesRegisterSummaryByItemWithoutPacking")
					{
						if (dtGrid.Rows.Count == 0 || dtGrid == null)
						{
							throw new Exception("Record Not Found For Display");
						}
						Reporting val3 = new Reporting();
						val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						val3.ShowReportWithDataTable(dtGrid, "316-SalesRegisterSummaryByItemWithoutPacking.rpt");
					}
				}
				else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
				{
					if (btnPrint.Text == "317-SalesRegisterSummaryByWarehouse")
					{
						if (dtGrid.Rows.Count == 0 || dtGrid == null)
						{
							throw new Exception("Record Not Found For Display");
						}
						Reporting val4 = new Reporting();
						val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						val4.ShowReportWithDataTable(dtGrid, "317-SalesRegisterSummaryByWarehouse.rpt");
					}
				}
				else if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
				{
					if (btnPrint.Text == "313-SalesRegisterSummaryByCustomer")
					{
						if (dtGrid.Rows.Count == 0 || dtGrid == null)
						{
							throw new Exception("Record Not Found For Display");
						}
						Reporting val5 = new Reporting();
						val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						val5.ShowReportWithDataTable(dtGrid, "313-SalesRegisterSummaryByCustomer.rpt");
					}
				}
				else if ((e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6) && btnPrint.Text == "315-SalesRegisterSummaryByCustomer&Item")
				{
					if (dtGrid.Rows.Count == 0 || dtGrid == null)
					{
						throw new Exception("Record Not Found For Display");
					}
					Reporting val6 = new Reporting();
					val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val6.ShowReportWithDataTable(dtGrid, "315-SalesRegisterSummaryByCustomer&Item.rpt");
				}
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
			dt.Rows.Add("Alt + 1", "For print 312-SalesRegisterSummary");
			dt.Rows.Add("Alt + 2", "For print 314-SalesRegisterSummaryByItem");
			dt.Rows.Add("Alt + 3", "For print 316-SalesRegisterSummaryByItemWithoutPacking");
			dt.Rows.Add("Alt + 4", "For print 317-SalesRegisterSummaryByWarehouse");
			dt.Rows.Add("Alt + 5", "For print 313-SalesRegisterSummaryByCustomer");
			dt.Rows.Add("Alt + 6", "For print 315-SalesRegisterSummaryByCustomer&Item");
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && DataGridHistory.CurrentColumn != null && DataGridHistory.CurrentColumn.Key == "ApprovalDetail")
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

	private void DataGridHistory_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbActivity).Text == "INVOICE WISE PROFITABLITY")
			{
				Color posColor = Color.Green;
				Color negColor = Color.Red;
				string[] array = new string[3] { "ProfitLossPerRate", "ProfitLoss", "ProfitLossPercent" };
				foreach (string colName in array)
				{
					GridEXColumn column = DataGridHistory.RootTable.Columns[colName];
					if (column != null)
					{
						GridEXCell Cell = e.Row.Cells[((JanusCollectionElement)column).Index];
						decimal value = Conversion.ToDecimal(Cell.Value);
						Cell.FormatStyle = new GridEXFormatStyle();
						if (value > 0m)
						{
							Cell.FormatStyle.ForeColor = posColor;
							Cell.FormatStyle.FontBold = (TriState)2;
						}
						else if (value < 0m)
						{
							Cell.FormatStyle.FontBold = (TriState)2;
							Cell.FormatStyle.ForeColor = negColor;
						}
						else
						{
							Cell.FormatStyle.ForeColor = Color.Black;
						}
					}
				}
			}
			if (((Control)(object)cmbActivity).Text == "Sales Register" && (int)e.Row.RowType == 0)
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
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Expected O, but got Unknown
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Expected O, but got Unknown
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Expected O, but got Unknown
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Expected O, but got Unknown
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Expected O, but got Unknown
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Expected O, but got Unknown
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Expected O, but got Unknown
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Expected O, but got Unknown
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Expected O, but got Unknown
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Expected O, but got Unknown
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Expected O, but got Unknown
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Expected O, but got Unknown
		//IL_082d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Expected O, but got Unknown
		//IL_084e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0858: Expected O, but got Unknown
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Expected O, but got Unknown
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Expected O, but got Unknown
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Expected O, but got Unknown
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Expected O, but got Unknown
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Expected O, but got Unknown
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Expected O, but got Unknown
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Expected O, but got Unknown
		//IL_0956: Unknown result type (might be due to invalid IL or missing references)
		//IL_0960: Expected O, but got Unknown
		//IL_0d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d42: Expected O, but got Unknown
		//IL_0d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d59: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
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
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
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
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.frmEvaulationDetailSalesReports));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbStockAccount = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbCgsAccount = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbSaleAccount = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbOtherCategory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbBranchName = new UltraCombo();
		this.cmbItemclass = new UltraCombo();
		this.cmbItemtype = new UltraCombo();
		this.cmbItemCatgory = new UltraCombo();
		this.CmbParentCategory = new UltraCombo();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtFromDocNo = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.CmbRefernceParty = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.cmbdistrict = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.cmbCustomGroup = new UltraCombo();
		this.cmbCity = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.cmbActivity = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbCropYear = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtToDocNo = new System.Windows.Forms.TextBox();
		this.cmbWareHouse = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbSupplierCustomer = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbJobLot = new UltraCombo();
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
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCgsAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSaleAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemclass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCatgory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefernceParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdistrict).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1241, 636);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 239);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1241, 397);
		this.panel4.TabIndex = 2;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1241, 368);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.FormattingRow += new RowLoadEventHandler(DataGridHistory_FormattingRow);
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1241, 29);
		this.panel6.TabIndex = 167;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1194, 2);
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
		this.label4.Size = new System.Drawing.Size(142, 19);
		this.label4.TabIndex = 0;
		this.label4.Text = "Filtered Records";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1241, 210);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockAccount);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCgsAccount);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSaleAccount);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherCategory);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemclass);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtype);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCatgory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.groupBox1.Controls.Add(this.txtDateFrom);
		this.groupBox1.Controls.Add(this.txtFromDocNo);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefernceParty);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdistrict);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomGroup);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCity);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add(this.txtToDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivity);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtToDocNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouse);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 28);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1160, 177);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbStockAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbStockAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance163;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Location = new System.Drawing.Point(952, 65);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Name = "CmbStockAccount";
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).TabIndex = 22;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(888, 71);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(59, 15);
		this.label38.TabIndex = 609;
		this.label38.Text = "Stock A/c";
		this.CmbCgsAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCgsAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCgsAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCgsAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCgsAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance166;
		((SpecialBoxBase)((UltraGridBase)this.CmbCgsAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCgsAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCgsAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCgsAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCgsAccount).Location = new System.Drawing.Point(952, 92);
		((System.Windows.Forms.Control)(object)this.CmbCgsAccount).Name = "CmbCgsAccount";
		((System.Windows.Forms.Control)(object)this.CmbCgsAccount).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCgsAccount).TabIndex = 23;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(888, 98);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(52, 15);
		this.label22.TabIndex = 607;
		this.label22.Text = "CGS A/C";
		this.CmbSaleAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbSaleAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSaleAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSaleAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSaleAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbSaleAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSaleAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSaleAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSaleAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSaleAccount).Location = new System.Drawing.Point(952, 38);
		((System.Windows.Forms.Control)(object)this.CmbSaleAccount).Name = "CmbSaleAccount";
		((System.Windows.Forms.Control)(object)this.CmbSaleAccount).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbSaleAccount).TabIndex = 21;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(888, 44);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(52, 15);
		this.label34.TabIndex = 605;
		this.label34.Text = "Sale A/C";
		this.CmbOtherCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance241).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance241).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance243;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance244).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance244).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance248).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOtherCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOtherCategory).Location = new System.Drawing.Point(98, 146);
		((System.Windows.Forms.Control)(object)this.CmbOtherCategory).Name = "CmbOtherCategory";
		((System.Windows.Forms.Control)(object)this.CmbOtherCategory).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherCategory).TabIndex = 7;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(7, 152);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(87, 15);
		this.label20.TabIndex = 603;
		this.label20.Text = "Other Category";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(7, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(79, 15);
		this.label2.TabIndex = 601;
		this.label2.Text = "Branch Name";
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
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(98, 11);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.cmbItemclass.AllowNull = (DefaultableBoolean)1;
		this.cmbItemclass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemclass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemclass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemclass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemclass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemclass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemclass).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemclass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemclass).Location = new System.Drawing.Point(98, 119);
		((System.Windows.Forms.Control)(object)this.cmbItemclass).Name = "cmbItemclass";
		((System.Windows.Forms.Control)(object)this.cmbItemclass).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemclass).TabIndex = 6;
		this.cmbItemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Location = new System.Drawing.Point(431, 65);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Name = "cmbItemtype";
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).TabIndex = 10;
		this.cmbItemCatgory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCatgory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCatgory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCatgory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCatgory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCatgory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCatgory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemCatgory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).Location = new System.Drawing.Point(431, 38);
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).Name = "cmbItemCatgory";
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCatgory).TabIndex = 9;
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(431, 11);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 8;
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(98, 67);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(118, 23);
		this.txtDateFrom.TabIndex = 2;
		this.txtFromDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFromDocNo.Location = new System.Drawing.Point(98, 94);
		this.txtFromDocNo.Name = "txtFromDocNo";
		this.txtFromDocNo.Size = new System.Drawing.Size(118, 23);
		this.txtFromDocNo.TabIndex = 4;
		this.txtFromDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNo_KeyPress);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(7, 44);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 126;
		this.label19.Text = "Date Type";
		this.CmbRefernceParty.AllowNull = (DefaultableBoolean)1;
		this.CmbRefernceParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefernceParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefernceParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefernceParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefernceParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefernceParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefernceParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefernceParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefernceParty).Location = new System.Drawing.Point(670, 38);
		((System.Windows.Forms.Control)(object)this.CmbRefernceParty).Name = "CmbRefernceParty";
		((System.Windows.Forms.Control)(object)this.CmbRefernceParty).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefernceParty).TabIndex = 15;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(600, 36);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(66, 30);
		this.label18.TabIndex = 176;
		this.label18.Text = "Reference Party";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance208).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(98, 38);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 1;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.cmbdistrict.AllowNull = (DefaultableBoolean)1;
		this.cmbdistrict.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdistrict.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdistrict.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Appearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance216;
		((SpecialBoxBase)((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance224).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdistrict).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdistrict).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdistrict.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdistrict).Location = new System.Drawing.Point(670, 119);
		((System.Windows.Forms.Control)(object)this.cmbdistrict).Name = "cmbdistrict";
		((System.Windows.Forms.Control)(object)this.cmbdistrict).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbdistrict).TabIndex = 18;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(600, 125);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(45, 15);
		this.label14.TabIndex = 170;
		this.label14.Text = "District";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(7, 125);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(61, 15);
		this.label17.TabIndex = 174;
		this.label17.Text = "Item Class";
		this.cmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).Location = new System.Drawing.Point(952, 11);
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).Name = "cmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomGroup).TabIndex = 20;
		this.cmbCity.AllowNull = (DefaultableBoolean)1;
		this.cmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCity).Location = new System.Drawing.Point(670, 146);
		((System.Windows.Forms.Control)(object)this.cmbCity).Name = "cmbCity";
		((System.Windows.Forms.Control)(object)this.cmbCity).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbCity).TabIndex = 19;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(887, 17);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(65, 15);
		this.label21.TabIndex = 168;
		this.label21.Text = "Custom Ac";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 71);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(94, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "Date From && To ";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(600, 152);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(62, 15);
		this.label5.TabIndex = 168;
		this.label5.Text = "City Name";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(340, 71);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 172;
		this.label16.Text = "Item Type";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(1091, 146);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(61, 26);
		this.btnShow.TabIndex = 25;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(218, 67);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(118, 23);
		this.txtToDate.TabIndex = 3;
		this.cmbActivity.AllowNull = (DefaultableBoolean)1;
		this.cmbActivity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbActivity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Location = new System.Drawing.Point(952, 119);
		((System.Windows.Forms.Control)(object)this.cmbActivity).Name = "cmbActivity";
		((System.Windows.Forms.Control)(object)this.cmbActivity).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivity).TabIndex = 24;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance231).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(670, 11);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 14;
		this.cmbItem.ValueChanged += new System.EventHandler(cmbItem_ValueChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(887, 125);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(47, 15);
		this.label12.TabIndex = 163;
		this.label12.Text = "Activity";
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(670, 65);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 16;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(600, 17);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(67, 15);
		this.label13.TabIndex = 165;
		this.label13.Text = "Item Name";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(600, 71);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(57, 15);
		this.label8.TabIndex = 155;
		this.label8.Text = "Crop Year";
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance229;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance235).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance236).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(670, 92);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 17;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 98);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(91, 15);
		this.label3.TabIndex = 5;
		this.label3.Text = "Doc From && To ";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(600, 98);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(35, 15);
		this.label7.TabIndex = 153;
		this.label7.Text = "UOM";
		this.txtToDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtToDocNo.Location = new System.Drawing.Point(218, 94);
		this.txtToDocNo.Name = "txtToDocNo";
		this.txtToDocNo.Size = new System.Drawing.Size(118, 23);
		this.txtToDocNo.TabIndex = 5;
		this.txtToDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNo_KeyPress);
		this.cmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance151;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance184).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Location = new System.Drawing.Point(431, 92);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Name = "cmbWareHouse";
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).TabIndex = 11;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(340, 98);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 161;
		this.label11.Text = "WareHouse";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(340, 17);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(91, 15);
		this.label6.TabIndex = 151;
		this.label6.Text = "Parent Category";
		this.cmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance195).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance196).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Location = new System.Drawing.Point(431, 146);
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Name = "cmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).TabIndex = 13;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(340, 152);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(69, 15);
		this.label10.TabIndex = 159;
		this.label10.Text = "Party Name";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(340, 44);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(82, 15);
		this.label15.TabIndex = 151;
		this.label15.Text = "Item Category";
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(431, 119);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 12;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(340, 125);
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
		this.panel5.Size = new System.Drawing.Size(1241, 29);
		this.panel5.TabIndex = 166;
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(6, 4);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(126, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Sales Register";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1241, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.btnPrint, this.btnShortcut });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1241, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
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
		base.ClientSize = new System.Drawing.Size(1241, 636);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmEvaulationDetailSalesReports";
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
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCgsAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSaleAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemclass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCatgory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefernceParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdistrict).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
