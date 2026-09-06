// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.pcc.Reports
{

public class SaleOrderRegister : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtOrderRegister = new DataTable();

	private DataTable dtOrderSummaryByOrderNoAndCustomer = new DataTable();

	private DataTable dtOrderSummaryByOrderNoAndSalesMan = new DataTable();

	private DataTable dtOrderSummaryByItem = new DataTable();

	private DataTable dtOrderSummaryByItemandVarient = new DataTable();

	private DataTable dtOrderSummaryByItemandCity = new DataTable();

	private DataTable dtOrderSummaryByItemVarientAndCity = new DataTable();

	private DataTable dtOrderSummaryByCustomer = new DataTable();

	private DataTable dtOrderSummaryByCustomerandItem = new DataTable();

	private DataTable dtOrderSummaryByCustomerandCity = new DataTable();

	private DataTable dtOrderSummaryByCustomerItemandCity = new DataTable();

	private DataTable dtOrderSummaryByCustomerandVarient = new DataTable();

	private DataTable dtOrderSummaryByCustomerItemAndvarient = new DataTable();

	private DataTable dtOrderSummaryByCustomerandReferenceParty = new DataTable();

	private DataTable dtOrderSummaryByCustomerItemandReferenceParty = new DataTable();

	private DataTable dtOrderSummaryByCustomerVarientandReferenceParty = new DataTable();

	private DataTable dtOrderSummaryByCustomerItemVarientandReferenceParty = new DataTable();

	private DataTable dtOrderSummaryByReferenceParty = new DataTable();

	private DataTable dtOrderSummaryByReferencePartyAndItem = new DataTable();

	private DataTable dtOrderSummaryByReferencePartyAndVarient = new DataTable();

	private DataTable dtOrderSummaryByReferencePartyItemAndVarient = new DataTable();

	private DataTable dtOrderSummaryByReferencePartyAndCity = new DataTable();

	public DataTable dtGrid = new DataTable();

	public DataTable dtGridSummary = new DataTable();

	public DataTable dtBindCombo = new DataTable();

	public DataTable dtRegisteredRefParty = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Label label4;

	private TextBox txtOrderNoFrom;

	private TextBox txtOrderNoTo;

	private GroupBox groupBox1;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label9;

	internal UltraCombo cmbItem;

	internal UltraCombo cmbItemtype;

	private Label label5;

	internal UltraCombo cmbCategory;

	private Label label8;

	internal UltraCombo cmbParentCategory;

	private Label label6;

	private Label label3;

	internal UltraCombo cmbApproveStatus;

	private Label label13;

	internal UltraCombo CmbOrderStatus;

	private Label label14;

	internal UltraCombo cmbperemeter;

	private Label label15;

	private Label label1;

	private ToolStripButton printRegister;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdDetail;

	private Panel panel4;

	internal GridEX grdMain;

	private Panel panel2;

	private GroupBox groupBox3;

	private Label label7;

	private Label label12;

	private Label label11;

	private Label label10;

	private Label label24;

	private Label label25;

	private TextBox txtBalanceAmountHeader;

	private TextBox txtBalanceWeightHeadrer;

	private TextBox txtBalanceQtyHeader;

	private TextBox txtDispatchAmountHeader;

	private TextBox txtDispatchWeightHeader;

	private TextBox txtDispatchQtyHeader;

	private TextBox txtOrderAmountHeader;

	private TextBox txtQrderWeightHeader;

	private TextBox txtOrderQtyHeader;

	private TabPage tabPage2;

	internal GridEX grd;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton BtnRegister;

	internal GridEX DataGridHistory;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar5;

	private Panel panel7;

	private GroupBox groupBox2;

	internal UltraCombo CmbOrderStatusSumm;

	internal UltraCombo CmbApprovalStatusSumm;

	private Label label28;

	internal UltraCombo cmbItemtypeSumm;

	private Label label26;

	internal UltraCombo cmbRefPartyDetailSumm;

	private Label label16;

	private Label label19;

	internal UltraCombo cmbCustomerSumm;

	internal UltraCombo cmbDistrictSumm;

	private Label label30;

	internal UltraCombo cmbcitySumm;

	private Label label17;

	private Label label31;

	internal UltraCombo cmbReportType;

	internal UltraCombo cmbItemNameSumm;

	private Label label18;

	private Label label29;

	internal UltraCombo cmbJobLotSumm;

	private DateTimePicker txtFromDateSumm;

	private Label label32;

	private Label label21;

	private DateTimePicker txtToDateSumm;

	private TextBox txtFromDocNoSumm;

	private TextBox txtToDocNoSumm;

	private Button BtnShowSumm;

	private Label label20;

	internal UltraCombo cmbParentcategorySumm;

	private Label label27;

	internal UltraCombo CmbCategorySumm;

	private Label label33;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripButton btnRefreshSummary;

	private ToolStripButton btnPrintSumm;

	internal UltraCombo CmbDateTypeSumm;

	private Label label40;

	private ToolStripButton BtnNewSummary;

	private Button BtnOpenStatus;

	private Label label22;

	private Button BtnCompleteStatus;

	private Button BtnCancelStatus;

	internal UltraCombo CmbRegisteredRefParty;

	internal UltraCombo cmbsaleman;

	internal UltraCombo CmbPartyGroup;

	private Label cmbsaleman1;

	private Label label34;

	private Label label35;

	private Label label36;

	public SaleOrderRegister(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 700);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			dtRegisteredRefParty.Columns.Add("Id");
			dtRegisteredRefParty.Columns.Add("name");
			dtOrderRegister.Columns.Add("Id", typeof(int));
			dtOrderRegister.Columns.Add("DocumentTypeId", typeof(int));
			dtOrderRegister.Columns.Add("DocType", typeof(string));
			dtOrderRegister.Columns.Add("DocDate", typeof(DateTime));
			dtOrderRegister.Columns.Add("DocNo", typeof(int));
			dtOrderRegister.Columns.Add("PartyName", typeof(string));
			dtOrderRegister.Columns.Add("ReferenceParty", typeof(string));
			dtOrderRegister.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderRegister.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderRegister.Columns.Add("ItemCode", typeof(string));
			dtOrderRegister.Columns.Add("ItemName", typeof(string));
			dtOrderRegister.Columns.Add("JobLot", typeof(string));
			dtOrderRegister.Columns.Add("Varient", typeof(string));
			dtOrderRegister.Columns.Add("VarientQty", typeof(double));
			dtOrderRegister.Columns.Add("OrderQty", typeof(double));
			dtOrderRegister.Columns.Add("DispatchedQty", typeof(double));
			dtOrderRegister.Columns.Add("BalQty", typeof(double));
			dtOrderRegister.Columns.Add("OrderWeight", typeof(double));
			dtOrderRegister.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderRegister.Columns.Add("BalWeight", typeof(double));
			dtOrderRegister.Columns.Add("OrderAmount", typeof(double));
			dtOrderRegister.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderRegister.Columns.Add("BalAmount", typeof(double));
			dtOrderRegister.Columns.Add("AvgRate", typeof(double));
			dtOrderRegister.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderRegister.Columns.Add("CityName", typeof(string));
			dtOrderRegister.Columns.Add("BuildingStorey", typeof(string));
			dtOrderRegister.Columns.Add("BuildingHeight", typeof(string));
			dtOrderRegister.Columns.Add("BuildingArea", typeof(string));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("Id", typeof(int));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("DocDate", typeof(DateTime));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("DocNo", typeof(int));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("Customer", typeof(string));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("ReferenceNo", typeof(string));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("ReferencePartyDetail", typeof(string));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("OrderQty", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("OrderWeight", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("OrderAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("DiscountAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("LabourAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("CarriageAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("CommissionAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("NetOrderAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("AvgRate", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("PercentOfTotal", typeof(decimal));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("BuildingStorey", typeof(string));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("BuildingHeight", typeof(string));
			dtOrderSummaryByOrderNoAndCustomer.Columns.Add("BuildingArea", typeof(string));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("Id", typeof(int));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("DocDate", typeof(DateTime));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("DocNo", typeof(int));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("CommissionAgent", typeof(string));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("Customer", typeof(string));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("ReferenceNo", typeof(string));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("ReferencePartyDetail", typeof(string));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("OrderQty", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("OrderWeight", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("OrderAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("DiscountAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("LabourAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("CarriageAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("CommissionAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("NetOrderAmount", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("AvgRate", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("PercentOfTotal", typeof(decimal));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("BuildingStorey", typeof(string));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("BuildingHeight", typeof(string));
			dtOrderSummaryByOrderNoAndSalesMan.Columns.Add("BuildingArea", typeof(string));
			dtOrderSummaryByItem.Columns.Add("Id");
			dtOrderSummaryByItem.Columns.Add("ItemCode");
			dtOrderSummaryByItem.Columns.Add("ItemName");
			dtOrderSummaryByItem.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByItem.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByItem.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByItem.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByItem.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByItem.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByItem.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByItem.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByItem.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByItem.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByItem.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("Id");
			dtOrderSummaryByItemandVarient.Columns.Add("ItemCode");
			dtOrderSummaryByItemandVarient.Columns.Add("ItemName");
			dtOrderSummaryByItemandVarient.Columns.Add("Varient");
			dtOrderSummaryByItemandVarient.Columns.Add("VarientQty", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByItemandVarient.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("Id");
			dtOrderSummaryByItemandCity.Columns.Add("ItemCode");
			dtOrderSummaryByItemandCity.Columns.Add("ItemName");
			dtOrderSummaryByItemandCity.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByItemandCity.Columns.Add("CityName");
			dtOrderSummaryByItemVarientAndCity.Columns.Add("Id");
			dtOrderSummaryByItemVarientAndCity.Columns.Add("ItemCode");
			dtOrderSummaryByItemVarientAndCity.Columns.Add("ItemName");
			dtOrderSummaryByItemVarientAndCity.Columns.Add("Varient");
			dtOrderSummaryByItemVarientAndCity.Columns.Add("VarientQty", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByItemVarientAndCity.Columns.Add("CityName");
			dtOrderSummaryByCustomer.Columns.Add("Id");
			dtOrderSummaryByCustomer.Columns.Add("PartyName");
			dtOrderSummaryByCustomer.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomer.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("Id");
			dtOrderSummaryByCustomerandItem.Columns.Add("PartyName");
			dtOrderSummaryByCustomerandItem.Columns.Add("ItemCode");
			dtOrderSummaryByCustomerandItem.Columns.Add("ItemName");
			dtOrderSummaryByCustomerandItem.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomerandItem.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("Id");
			dtOrderSummaryByCustomerandVarient.Columns.Add("PartyName");
			dtOrderSummaryByCustomerandVarient.Columns.Add("Varient");
			dtOrderSummaryByCustomerandVarient.Columns.Add("VarientQty", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomerandVarient.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("Id");
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("PartyName");
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("ItemCode");
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("ItemName");
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("Varient");
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("VarientQty", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomerItemAndvarient.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("Id");
			dtOrderSummaryByCustomerandCity.Columns.Add("PartyName");
			dtOrderSummaryByCustomerandCity.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByCustomerandCity.Columns.Add("CityName");
			dtOrderSummaryByCustomerItemandCity.Columns.Add("Id");
			dtOrderSummaryByCustomerItemandCity.Columns.Add("PartyName");
			dtOrderSummaryByCustomerItemandCity.Columns.Add("ItemCode");
			dtOrderSummaryByCustomerItemandCity.Columns.Add("ItemName");
			dtOrderSummaryByCustomerItemandCity.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByCustomerItemandCity.Columns.Add("CityName");
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("Id");
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("PartyName");
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("ReferenceParty");
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomerandReferenceParty.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("Id");
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("PartyName");
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("Varient");
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("VarientQty", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("ReferenceParty");
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomerVarientandReferenceParty.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("Id");
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("PartyName");
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("Itemcode");
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("ItemName");
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("ReferenceParty");
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomerItemandReferenceParty.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("Id");
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("PartyName");
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("Itemcode");
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("ItemName");
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("Varient");
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("VarientQty", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("ReferenceParty");
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByCustomerItemVarientandReferenceParty.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("Id");
			dtOrderSummaryByReferenceParty.Columns.Add("ReferenceParty");
			dtOrderSummaryByReferenceParty.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByReferenceParty.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByReferenceParty.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByReferenceParty.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("Id");
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("ReferenceParty");
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("ItemCode");
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("ItemName");
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByReferencePartyAndItem.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("Id");
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("ReferenceParty");
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("PackUom");
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("VarientQty", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByReferencePartyAndVarient.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("Id");
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("ReferenceParty");
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("ItemCode");
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("ItemName");
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("Varient");
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("VarientQty", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByReferencePartyItemAndVarient.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("Id");
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("CityName");
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("ReferenceParty");
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("ReferencePartyAddress", typeof(string));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("DispatchedQty", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("DispatchedWeight", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("DispatchedAmount", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryByReferencePartyAndCity.Columns.Add("PercentOfTotal", typeof(double));
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			ComboFill();
			ParameterFill();
			StatusFill();
			ApproveStatusFill();
			ReportTypeFill();
			tabControl1.SelectedIndex = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "UnApproved");
			dt.Rows.Add(2, "Approve");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApproveStatus, "Id", "Status", "Status", ZeroIndex: false);
			DDL.BindDDL(dt, CmbApprovalStatusSumm, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApproveStatus).Rows[2].Activate();
			((UltraGridBase)CmbApprovalStatusSumm).Rows[2].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Cancel");
			dt.Rows.Add(3, "Complete");
			dt.Rows.Add(4, "All");
			DDL.BindDDL(dt, CmbOrderStatus, "Id", "Status", "Status", ZeroIndex: false);
			DDL.BindDDL(dt, CmbOrderStatusSumm, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbOrderStatus).Rows[3].Activate();
			((UltraGridBase)CmbOrderStatusSumm).Rows[3].Activate();
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
				DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
				DDL.BindDDL(dt, CmbDateTypeSumm, "Id", "Parameters", "Parameters", ZeroIndex: false);
				((UltraGridBase)cmbperemeter).Rows[1].Activate();
				((UltraGridBase)CmbDateTypeSumm).Rows[1].Activate();
			}
			else
			{
				((Control)(object)cmbperemeter).Text = string.Empty;
				((Control)(object)CmbDateTypeSumm).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				FromDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				FromDate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				FromDate.Value = first;
				Todate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				FromDate.Value = first2;
				Todate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				Todate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				txtToDateSumm.Value = DateTime.Today;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		try
		{
			dtRegisteredRefParty.Clear();
			((UltraGridBase)CmbRegisteredRefParty).DataSource = null;
			((Control)(object)CmbRegisteredRefParty).Text = "";
			DataTable dtParentCategory = new DataTable();
			dtParentCategory.Columns.Add("Id");
			dtParentCategory.Columns.Add("name");
			DataTable dtItemCategories = new DataTable();
			dtItemCategories.Columns.Add("Id");
			dtItemCategories.Columns.Add("name");
			DataTable dtItemType = new DataTable();
			dtItemType.Columns.Add("Id");
			dtItemType.Columns.Add("name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("name");
			DataTable dtJob = new DataTable();
			dtJob.Columns.Add("Id");
			dtJob.Columns.Add("name");
			DataTable dtDistrict = new DataTable();
			dtDistrict.Columns.Add("Id");
			dtDistrict.Columns.Add("name");
			DataTable dtCity = new DataTable();
			dtCity.Columns.Add("Id");
			dtCity.Columns.Add("name");
			DataTable dtSupplieerCustomer = new DataTable();
			dtSupplieerCustomer.Columns.Add("Id");
			dtSupplieerCustomer.Columns.Add("name");
			DataTable dtRefParty = new DataTable();
			dtRefParty.Columns.Add("Id");
			dtRefParty.Columns.Add("name");
			DataTable dtsaleman = new DataTable();
			dtsaleman.Columns.Add("Id");
			dtsaleman.Columns.Add("name");
			DataTable dtCustomerGroup = new DataTable();
			dtCustomerGroup.Columns.Add("Id");
			dtCustomerGroup.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = SaleOrder.GetDataForDropDownFromSaleOrder(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ParentCategory")
				{
					dtParentCategory.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ItemCategory")
				{
					dtItemCategories.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ItemType")
				{
					dtItemType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "JobLot")
				{
					dtJob.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "District")
				{
					dtDistrict.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "City")
				{
					dtCity.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Customer")
				{
					dtSupplieerCustomer.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ReferencePartyName")
				{
					dtRefParty.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "RegisteredReferenceParty")
				{
					dtRegisteredRefParty.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "RefSalesMan")
				{
					dtsaleman.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "CustomerGroup")
				{
					dtCustomerGroup.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtParentCategory.Rows.Count > 0)
			{
				DDL.BindDDL(dtParentCategory, cmbParentCategory, "Id", "name", "Parent Category", ZeroIndex: false);
				DDL.BindDDL(dtParentCategory, cmbParentcategorySumm, "Id", "name", "Parent Category", ZeroIndex: false);
				((UltraGridBase)cmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbParentcategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemCategories.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemCategories, cmbCategory, "Id", "name", "Item Categories", ZeroIndex: false);
				DDL.BindDDL(dtItemCategories, CmbCategorySumm, "Id", "name", "Item Categories", ZeroIndex: false);
				((UltraGridBase)cmbCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbCategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemType.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemType, cmbItemtype, "Id", "name", "Item Type", ZeroIndex: false);
				DDL.BindDDL(dtItemType, cmbItemtypeSumm, "Id", "name", "Item Type", ZeroIndex: false);
				((UltraGridBase)cmbItemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbItemtypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "name", "Item", ZeroIndex: false);
				DDL.BindDDL(dtItem, cmbItemNameSumm, "Id", "name", "Item", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDL(dtJob, cmbJobLotSumm, "Id", "name", "JobLot", ZeroIndex: false);
			}
			if (dtDistrict.Rows.Count > 0)
			{
				DDL.BindDDL(dtDistrict, cmbDistrictSumm, "Id", "name", "District", ZeroIndex: false);
			}
			if (dtCity.Rows.Count > 0)
			{
				DDL.BindDDL(dtCity, cmbcitySumm, "Id", "name", "City", ZeroIndex: false);
			}
			if (dtSupplieerCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplieerCustomer, CmbSupplier, "Id", "name", "Customer Name", ZeroIndex: false);
				DDL.BindDDL(dtSupplieerCustomer, cmbCustomerSumm, "Id", "name", "Customer Name", ZeroIndex: false);
			}
			if (dtRefParty.Rows.Count > 0)
			{
				DDL.BindDDL(dtRefParty, cmbRefPartyDetailSumm, "Id", "name", "Reference Party Name", ZeroIndex: false);
			}
			if (dtRegisteredRefParty.Rows.Count > 0)
			{
				DDL.BindDDL(dtRegisteredRefParty, CmbRegisteredRefParty, "Id", "name", "Sales Person", ZeroIndex: false);
				UltraGridColumn obj2 = ((UltraGridBase)CmbRegisteredRefParty).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				CmbRegisteredRefParty.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbRegisteredRefParty.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbRegisteredRefParty.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbRegisteredRefParty.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
			if (dtsaleman.Rows.Count > 0)
			{
				DDL.BindDDL(dtsaleman, cmbsaleman, "Id", "name", "Sale Man", ZeroIndex: false);
			}
			if (dtCustomerGroup.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomerGroup, CmbPartyGroup, "Id", "name", "Party Group", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportTypeFill()
	{
		try
		{
			DataTable dtreporttype = new DataTable();
			dtreporttype.Columns.Add("Id");
			dtreporttype.Columns.Add("name");
			dtreporttype.Rows.Add("Order Register", "Order Register");
			dtreporttype.Rows.Add("Order Summary By OrderNo & Customer", "Order Summary By OrderNo & Customer");
			dtreporttype.Rows.Add("Order Summary By OrderNo Customer & SalesMan", "Order Summary By OrderNo Customer & SalesMan");
			dtreporttype.Rows.Add("Order Summary By Item", "Order Summary By Item");
			dtreporttype.Rows.Add("Order Summary By Item & Varient", "Order Summary By Item & Varient");
			dtreporttype.Rows.Add("Order Summary By Item & City", "Order Summary By Item & City");
			dtreporttype.Rows.Add("Order Summary By Item_Varient & City", "Order Summary By Item_Varient & City");
			dtreporttype.Rows.Add("Order Summary By Customer", "Order Summary By Customer");
			dtreporttype.Rows.Add("Order Summary By Customer & Item", "Order Summary By Customer & Item");
			dtreporttype.Rows.Add("Order Summary By Customer & City", "Order Summary By Customer & City");
			dtreporttype.Rows.Add("Order Summary By Customer_Item & City", "Order Summary By Customer_Item & City");
			dtreporttype.Rows.Add("Order Summary By Customer & Varient", "Order Summary By Customer & Varient");
			dtreporttype.Rows.Add("Order Summary By Customer_Item & Varient", "Order Summary By Customer_Item & Varient");
			dtreporttype.Rows.Add("Order Summary By Customer & ReferenceParty", "Order Summary By Customer & ReferenceParty");
			dtreporttype.Rows.Add("Order Summary By Customer Item & ReferenceParty", "Order Summary By Customer Item & ReferenceParty");
			dtreporttype.Rows.Add("Order Summary By Customer Varient & ReferenceParty", "Order Summary By Customer Varient & ReferenceParty");
			dtreporttype.Rows.Add("Order Summary By Customer_Item_Varient & ReferenceParty", "Order Summary By Customer_Item_Varient & ReferenceParty");
			dtreporttype.Rows.Add("Order Summary By ReferenceParty", "Order Summary By ReferenceParty");
			dtreporttype.Rows.Add("Order Summary By ReferenceParty & Item", "Order Summary By ReferenceParty & Item");
			dtreporttype.Rows.Add("Order Summary By ReferenceParty & Varient", "Order Summary By ReferenceParty & Varient");
			dtreporttype.Rows.Add("Order Summary By ReferenceParty_Item & Varient", "Order Summary By ReferenceParty_Item & Varient");
			dtreporttype.Rows.Add("Order Summary By ReferenceParty & City", "Order Summary By ReferenceParty & City");
			DDL.BindDDL(dtreporttype, cmbReportType, "Id", "name", "Report Type", ZeroIndex: false);
			((UltraGridBase)cmbReportType).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)cmbParentCategory).Text = string.Empty;
			((Control)(object)cmbCategory).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemtype).Text = string.Empty;
			((Control)(object)cmbApproveStatus).Text = string.Empty;
			txtOrderNoFrom.Text = string.Empty;
			txtOrderNoTo.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Hide();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 0)
			{
				FillDetailDataGrid();
			}
			else if (tabControl2.SelectedIndex == 1)
			{
				FillMainGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailDataGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			obj.DocumentTypeId = 1852;
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbCategory.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtype.Value);
			obj.InventoryParentCategories = Conversion.ToInt(cmbParentCategory.Value);
			if (Conversion.ToString((object)((Control)(object)CmbOrderStatus).Text) != "All")
			{
				obj.Status = Conversion.ToString((object)((Control)(object)CmbOrderStatus).Text);
			}
			if (((Control)(object)cmbApproveStatus).Text == "Approve")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatus).Text == "UnApproved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatus).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			dtGrid = SaleOrder.SaleOrderSlipAndRegister(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("VisitedByName", typeof(string));
				dt.Columns.Add("ReferenceNo", typeof(string));
				dt.Columns.Add("ReferenceParty", typeof(string));
				dt.Columns.Add("ReferencePartyAddress", typeof(string));
				dt.Columns.Add("RefPartyCellNo", typeof(string));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("RefSalesMan", typeof(string));
				dt.Columns.Add("CommAmount", typeof(decimal));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("OrderDueDays", typeof(int));
				dt.Columns.Add("OrderDueDate", typeof(DateTime));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("OrderQtyHeader", typeof(decimal));
				dt.Columns.Add("OrderWeightHeader", typeof(decimal));
				dt.Columns.Add("OrderAmountHeader", typeof(decimal));
				dt.Columns.Add("DispatchQtyHeader", typeof(decimal));
				dt.Columns.Add("DispatchWeightHeader", typeof(decimal));
				dt.Columns.Add("DispatchAmountHeader", typeof(decimal));
				dt.Columns.Add("BalanceQtyHeader", typeof(decimal));
				dt.Columns.Add("BalanceWeightHeader", typeof(decimal));
				dt.Columns.Add("BalanceAmountHeader", typeof(decimal));
				dt.Columns.Add("OrderItemId", typeof(int));
				dt.Columns.Add("SaleGLAC", typeof(int));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemAttributeVarientId", typeof(int));
				dt.Columns.Add("ItemArrtibuteVarient", typeof(string));
				dt.Columns.Add("VarientEquivalent", typeof(decimal));
				dt.Columns.Add("JobLotId", typeof(int));
				dt.Columns.Add("JobLotDescription", typeof(string));
				dt.Columns.Add("ItemQty", typeof(decimal));
				dt.Columns.Add("DispatchQty", typeof(decimal));
				dt.Columns.Add("BalanceQty", typeof(decimal));
				dt.Columns.Add("ItemWeight", typeof(decimal));
				dt.Columns.Add("NetWeight", typeof(decimal));
				dt.Columns.Add("DispatchWeight", typeof(decimal));
				dt.Columns.Add("BalanceWeight", typeof(decimal));
				dt.Columns.Add("ItemRate", typeof(decimal));
				dt.Columns.Add("ItemAmount", typeof(decimal));
				dt.Columns.Add("ItemDiscountTypeId", typeof(int));
				dt.Columns.Add("ItemDiscountType", typeof(string));
				dt.Columns.Add("ItemDiscountRate", typeof(decimal));
				dt.Columns.Add("ItemDiscountAmount", typeof(decimal));
				dt.Columns.Add("ItemNetAmount", typeof(decimal));
				dt.Columns.Add("DispatchAmount", typeof(decimal));
				dt.Columns.Add("BalanceAmount", typeof(decimal));
				dt.Columns.Add("CityId", typeof(int));
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("DeliveryStartDate", typeof(DateTime));
				dt.Columns.Add("TransporterName", typeof(string));
				dt.Columns.Add("FrieghtAmountHeader", typeof(decimal));
				dt.Columns.Add("WagesAccount", typeof(string));
				dt.Columns.Add("WagesAmountHeader", typeof(decimal));
				dt.Columns.Add("Status", typeof(string));
				dt.Columns.Add("IsApproved", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUserName", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("CurrentGlBalance", typeof(double));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocNo"], row["DocDate"], row["CustomerName"], row["VisitedByName"], row["RefrenenceNo"], row["ReferencPartyName"], row["ReferencPartyAddress"], row["ReferencPartyCellNo"], row["CommissionAgent"], row["RefSalesMan"], row["CommissionAmount"], row["TermsDescription"], row["OrderDueDays"], row["OrderDueDate"], row["DeliveryTerm"], row["OrderQtyHeader"], row["OrderWeightHeader"], row["OrderAmountHeader"], row["DispatchQtyHeader"], row["DispatchWeightHeader"], row["DispatchAmountHeader"], row["BalanceQtyHeader"], row["BalanceWeightHeader"], row["BalanceAmountHeader"], row["OrderItemId"], row["SaleGLAC"], row["ItemCode"], row["ItemName"], row["ItemAttributeVarientId"], row["ItemArrtibuteVarient"], row["VarientEquivalent"], row["JobLotId"], row["JobLotDescription"], row["ItemQty"], row["DispatchQty"], row["BalanceQty"], row["ItemWeight"], row["NetWeight"], row["DispatchWeight"], row["BalanceWeight"], row["ItemRate"], row["ItemAmount"], row["ItemDiscountTypeId"], row["ItemDiscountType"], row["ItemDiscountRate"], row["ItemDiscountAmount"], row["ItemNetAmount"], row["DispatchAmount"], row["BalanceAmount"], row["CityId"], row["CityName"], row["DeliveryStartDate"], row["TransporterName"], row["FrieghtAmountHeader"], row["OtherWagesAccount"], row["OtherWagesHeader"], row["OrderStatus"], row["IsApproved"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"], row["RemarksHeader"], row["CurrentGlBalance"], row["NoOfAttachments"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdDetailDataSettings();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailDataSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "OrderQtyHeader", "OrderWeightHeader", "OrderAmountHeader", "DispatchQtyHeader", "DispatchWeightHeader", "DispatchAmountHeader", "BalanceQtyHeader", "BalanceWeightHeader", "BalanceAmountHeader",
				"SaleGLAC", "ItemDiscountTypeId", "ItemAttributeVarientId", "OrderItemId", "CityId", "JobLotId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 3, hiddenColumns);
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["JobLotDescription"].Caption = "JobLot";
			grd.RootTable.Columns["ItemArrtibuteVarient"].Caption = "Varient";
			grd.RootTable.Columns["VarientEquivalent"].Caption = "VarientUnit";
			grd.RootTable.Columns["ItemDiscountRate"].Caption = "DiscRate";
			grd.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["VisitedByName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["ReferenceNo"].Width = Constants.InventoryConstants.RecordNo + 30;
			grd.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["ReferencePartyAddress"].Width = Constants.InventoryConstants.Address;
			grd.RootTable.Columns["RefPartyCellNo"].Width = Constants.InventoryConstants.CellNo;
			grd.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["RefSalesMan"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grd.RootTable.Columns["OrderDueDays"].Width = Constants.InventoryConstants.DueDays;
			grd.RootTable.Columns["OrderDueDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grd.RootTable.Columns["OrderQtyHeader"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["OrderWeightHeader"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["OrderAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["DispatchQtyHeader"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["DispatchWeightHeader"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["DispatchAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["BalanceQtyHeader"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["BalanceWeightHeader"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["BalanceAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["SaleGLAC"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["ItemArrtibuteVarient"].Width = Constants.InventoryConstants.Variant;
			grd.RootTable.Columns["VarientEquivalent"].Width = Constants.InventoryConstants.VariantEquivalent;
			grd.RootTable.Columns["JobLotDescription"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["DispatchQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["BalanceQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["ItemWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["DispatchWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["BalanceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ItemDiscountTypeId"].Width = Constants.InventoryConstants.DiscountType;
			grd.RootTable.Columns["ItemDiscountType"].Width = Constants.InventoryConstants.DiscountType;
			grd.RootTable.Columns["ItemDiscountRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["ItemDiscountAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["ItemNetAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["DispatchAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["BalanceAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grd.RootTable.Columns["DeliveryStartDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["TransporterName"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["FrieghtAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["WagesAccount"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["WagesAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grd.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grd.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["ApprovedUserName"].Width = Constants.InventoryConstants.ApprovedByConstant + 10;
			grd.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grd.RootTable.Columns["CurrentGlBalance"].Position = grd.RootTable.Columns["CustomerName"].Position + 1;
			grd.RootTable.Columns["CurrentGlBalance"].FormatString = "#,#;(#,#);0";
			grd.RootTable.Columns["CurrentGlBalance"].TotalFormatString = "#,#;(#,#);0";
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].Caption = "Print";
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].Width = 60;
			grd.RootTable.Columns["Print"].Position = 0;
			grd.RootTable.Columns["NoOfAttachments"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalDetail()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				decimal OrderQty = default(decimal);
				decimal OrderWeight = default(decimal);
				decimal OrderAmount = default(decimal);
				decimal DispatchQty = default(decimal);
				decimal DispatchWeight = default(decimal);
				decimal DispatchAmount = default(decimal);
				decimal BalQty = default(decimal);
				decimal BalWeight = default(decimal);
				decimal BalAmount = default(decimal);
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Id != 0)
				{
					DataRow matchingRow = dtGrid.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["Id"]) == Id);
					if (matchingRow != null)
					{
						OrderQty = Conversion.ToDecimal(matchingRow["OrderQtyHeader"]);
						OrderWeight = Conversion.ToDecimal(matchingRow["OrderWeightHeader"]);
						OrderAmount = Conversion.ToDecimal(matchingRow["OrderAmountHeader"]);
						DispatchQty = Conversion.ToDecimal(matchingRow["DispatchQtyHeader"]);
						DispatchWeight = Conversion.ToDecimal(matchingRow["DispatchWeightHeader"]);
						DispatchAmount = Conversion.ToDecimal(matchingRow["DispatchAmountHeader"]);
						BalQty = Conversion.ToDecimal(matchingRow["BalanceQtyHeader"]);
						BalWeight = Conversion.ToDecimal(matchingRow["BalanceWeightHeader"]);
						BalAmount = Conversion.ToDecimal(matchingRow["BalanceAmountHeader"]);
					}
					txtOrderQtyHeader.Text = OrderQty.ToString(clsGlobalVariables.stringFormatsingle);
					txtQrderWeightHeader.Text = OrderWeight.ToString(clsGlobalVariables.stringFormatsingle);
					txtOrderAmountHeader.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
					txtDispatchQtyHeader.Text = DispatchQty.ToString(clsGlobalVariables.stringFormatsingle);
					txtDispatchWeightHeader.Text = DispatchWeight.ToString(clsGlobalVariables.stringFormatsingle);
					txtDispatchAmountHeader.Text = DispatchAmount.ToString(clsGlobalVariables.stringFormatsingle);
					txtBalanceQtyHeader.Text = BalQty.ToString(clsGlobalVariables.stringFormatsingle);
					txtBalanceWeightHeadrer.Text = BalWeight.ToString(clsGlobalVariables.stringFormatsingle);
					txtBalanceAmountHeader.Text = BalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else
			{
				txtOrderQtyHeader.Text = 0.ToString();
				txtQrderWeightHeader.Text = 0.ToString();
				txtOrderAmountHeader.Text = 0.ToString();
				txtDispatchQtyHeader.Text = 0.ToString();
				txtDispatchWeightHeader.Text = 0.ToString();
				txtDispatchAmountHeader.Text = 0.ToString();
				txtBalanceQtyHeader.Text = 0.ToString();
				txtBalanceWeightHeadrer.Text = 0.ToString();
				txtBalanceAmountHeader.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Print")
			{
				CommonServices.SaleOrderSlipConcrete(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillMainGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			obj.DocumentTypeId = 1852;
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			if (Conversion.ToString((object)((Control)(object)CmbOrderStatus).Text) != "All")
			{
				obj.Status = Conversion.ToString((object)((Control)(object)CmbOrderStatus).Text);
			}
			if (((Control)(object)cmbApproveStatus).Text == "Approve")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatus).Text == "UnApproved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatus).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			dtGrid = SaleOrder.SaleOrderSlipAndRegister(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("ExpiryDate", typeof(DateTime));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("RefrenenceNo", typeof(string));
				dt.Columns.Add("VisitedByName", typeof(string));
				dt.Columns.Add("ReferenceParty", typeof(string));
				dt.Columns.Add("RefPartyCellNo", typeof(string));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("RefSalesMan", typeof(string));
				dt.Columns.Add("CommAmount", typeof(decimal));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("OrderDueDays", typeof(int));
				dt.Columns.Add("OrderDueDate", typeof(DateTime));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("DeliveryStartDate", typeof(DateTime));
				dt.Columns.Add("TransporterName", typeof(string));
				dt.Columns.Add("FrieghtAmountHeader", typeof(decimal));
				dt.Columns.Add("WagesAccount", typeof(string));
				dt.Columns.Add("WagesAmountHeader", typeof(decimal));
				dt.Columns.Add("Status", typeof(string));
				dt.Columns.Add("IsApproved", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUserName", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("OrderQtyHeader", typeof(decimal));
				dt.Columns.Add("OrderWeightHeader", typeof(decimal));
				dt.Columns.Add("OrderAmountHeader", typeof(decimal));
				dt.Columns.Add("DispatchQtyHeader", typeof(decimal));
				dt.Columns.Add("DispatchWeightHeader", typeof(decimal));
				dt.Columns.Add("DispatchAmountHeader", typeof(decimal));
				dt.Columns.Add("BalanceQtyHeader", typeof(decimal));
				dt.Columns.Add("BalanceWeightHeader", typeof(decimal));
				dt.Columns.Add("BalanceAmountHeader", typeof(decimal));
				dt.Columns.Add("CurrentGlBalance", typeof(decimal));
				foreach (DataRow row in dtGrid.Rows)
				{
					if (!dt.AsEnumerable().Any((DataRow r) => r.Field<int>("Id") == Conversion.ToInt(row["Id"])))
					{
						dt.Rows.Add(row["Id"], row["DocNo"], row["DocDate"], row["OrderExpiryDate"], row["CustomerName"], row["RefrenenceNo"], row["VisitedByName"], row["ReferencPartyName"], row["ReferencPartyCellNo"], row["CommissionAgent"], row["RefSalesMan"], row["CommissionAmount"], row["TermsDescription"], row["OrderDueDays"], row["OrderDueDate"], row["DeliveryTerm"], row["DeliveryStartDate"], row["TransporterName"], row["FrieghtAmountHeader"], row["OtherWagesAccount"], row["OtherWagesHeader"], row["OrderStatus"], row["IsApproved"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"], row["RemarksHeader"], row["NoOfAttachments"], row["OrderQtyHeader"], row["OrderWeightHeader"], row["OrderAmountHeader"], row["DispatchQtyHeader"], row["DispatchWeightHeader"], row["DispatchAmountHeader"], row["BalanceQtyHeader"], row["BalanceWeightHeader"], row["BalanceAmountHeader"], row["CurrentGlBalance"]);
					}
				}
				grdMain.DataSource = dt;
				grdMain.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grdMain.ClearStructure();
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalMainDetail()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				decimal OrderQty = default(decimal);
				decimal OrderWeight = default(decimal);
				decimal OrderAmount = default(decimal);
				decimal DispatchQty = default(decimal);
				decimal DispatchWeight = default(decimal);
				decimal DispatchAmount = default(decimal);
				decimal BalQty = default(decimal);
				decimal BalWeight = default(decimal);
				decimal BalAmount = default(decimal);
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Id != 0)
				{
					DataRow matchingRow = dtGrid.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["Id"]) == Id);
					if (matchingRow != null)
					{
						OrderQty = Conversion.ToDecimal(matchingRow["OrderQtyHeader"]);
						OrderWeight = Conversion.ToDecimal(matchingRow["OrderWeightHeader"]);
						OrderAmount = Conversion.ToDecimal(matchingRow["OrderAmountHeader"]);
						DispatchQty = Conversion.ToDecimal(matchingRow["DispatchQtyHeader"]);
						DispatchWeight = Conversion.ToDecimal(matchingRow["DispatchWeightHeader"]);
						DispatchAmount = Conversion.ToDecimal(matchingRow["DispatchAmountHeader"]);
						BalQty = Conversion.ToDecimal(matchingRow["BalanceQtyHeader"]);
						BalWeight = Conversion.ToDecimal(matchingRow["BalanceWeightHeader"]);
						BalAmount = Conversion.ToDecimal(matchingRow["BalanceAmountHeader"]);
					}
					txtOrderQtyHeader.Text = OrderQty.ToString(clsGlobalVariables.stringFormatsingle);
					txtQrderWeightHeader.Text = OrderWeight.ToString(clsGlobalVariables.stringFormatsingle);
					txtOrderAmountHeader.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
					txtDispatchQtyHeader.Text = DispatchQty.ToString(clsGlobalVariables.stringFormatsingle);
					txtDispatchWeightHeader.Text = DispatchWeight.ToString(clsGlobalVariables.stringFormatsingle);
					txtDispatchAmountHeader.Text = DispatchAmount.ToString(clsGlobalVariables.stringFormatsingle);
					txtBalanceQtyHeader.Text = BalQty.ToString(clsGlobalVariables.stringFormatsingle);
					txtBalanceWeightHeadrer.Text = BalWeight.ToString(clsGlobalVariables.stringFormatsingle);
					txtBalanceAmountHeader.Text = BalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else
			{
				txtOrderQtyHeader.Text = 0.ToString();
				txtQrderWeightHeader.Text = 0.ToString();
				txtOrderAmountHeader.Text = 0.ToString();
				txtDispatchQtyHeader.Text = 0.ToString();
				txtDispatchWeightHeader.Text = 0.ToString();
				txtDispatchAmountHeader.Text = 0.ToString();
				txtBalanceQtyHeader.Text = 0.ToString();
				txtBalanceWeightHeadrer.Text = 0.ToString();
				txtBalanceAmountHeader.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_0bb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Expected O, but got Unknown
		try
		{
			grdMain.RootTable.Columns["Id"].Visible = false;
			grdMain.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdMain.RootTable.Columns["OrderQtyHeader"].Visible = false;
			grdMain.RootTable.Columns["OrderWeightHeader"].Visible = false;
			grdMain.RootTable.Columns["OrderAmountHeader"].Visible = false;
			grdMain.RootTable.Columns["DispatchQtyHeader"].Visible = false;
			grdMain.RootTable.Columns["DispatchWeightHeader"].Visible = false;
			grdMain.RootTable.Columns["DispatchAmountHeader"].Visible = false;
			grdMain.RootTable.Columns["BalanceQtyHeader"].Visible = false;
			grdMain.RootTable.Columns["BalanceWeightHeader"].Visible = false;
			grdMain.RootTable.Columns["BalanceAmountHeader"].Visible = false;
			grdMain.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
			grdMain.RootTable.Columns["Id"].EditType = (EditType)0;
			grdMain.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdMain.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdMain.RootTable.Columns["ExpiryDate"].EditType = (EditType)0;
			grdMain.RootTable.Columns["CustomerName"].EditType = (EditType)0;
			grdMain.RootTable.Columns["RefrenenceNo"].EditType = (EditType)0;
			grdMain.RootTable.Columns["VisitedByName"].EditType = (EditType)0;
			grdMain.RootTable.Columns["ReferenceParty"].EditType = (EditType)0;
			grdMain.RootTable.Columns["RefPartyCellNo"].EditType = (EditType)0;
			grdMain.RootTable.Columns["CommissionAgent"].EditType = (EditType)0;
			grdMain.RootTable.Columns["RefSalesMan"].EditType = (EditType)0;
			grdMain.RootTable.Columns["CommAmount"].EditType = (EditType)0;
			grdMain.RootTable.Columns["PaymentTerm"].EditType = (EditType)0;
			grdMain.RootTable.Columns["OrderDueDays"].EditType = (EditType)0;
			grdMain.RootTable.Columns["OrderDueDate"].EditType = (EditType)0;
			grdMain.RootTable.Columns["DeliveryTerm"].EditType = (EditType)0;
			grdMain.RootTable.Columns["DeliveryStartDate"].EditType = (EditType)0;
			grdMain.RootTable.Columns["TransporterName"].EditType = (EditType)0;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
			grdMain.RootTable.Columns["WagesAmountHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["Status"].EditType = (EditType)0;
			grdMain.RootTable.Columns["IsApproved"].EditType = (EditType)0;
			grdMain.RootTable.Columns["EntryDate"].EditType = (EditType)0;
			grdMain.RootTable.Columns["EntryUserName"].EditType = (EditType)0;
			grdMain.RootTable.Columns["ModifyDate"].EditType = (EditType)0;
			grdMain.RootTable.Columns["ModifyUserName"].EditType = (EditType)0;
			grdMain.RootTable.Columns["ApprovedDate"].EditType = (EditType)0;
			grdMain.RootTable.Columns["ApprovedUserName"].EditType = (EditType)0;
			grdMain.RootTable.Columns["RemarksHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			grdMain.RootTable.Columns["OrderQtyHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["OrderWeightHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["OrderAmountHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["DispatchQtyHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["DispatchWeightHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["DispatchAmountHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["BalanceQtyHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["BalanceWeightHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["BalanceAmountHeader"].EditType = (EditType)0;
			grdMain.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["CustomerName"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["RefrenenceNo"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["VisitedByName"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["ReferenceParty"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["RefPartyCellNo"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["CommissionAgent"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["RefSalesMan"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["CommAmount"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["PaymentTerm"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["OrderDueDays"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["OrderDueDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["DeliveryTerm"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["DeliveryStartDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["TransporterName"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["WagesAccount"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["WagesAmountHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["Status"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["IsApproved"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["EntryDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["EntryUserName"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["ModifyDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["ModifyUserName"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["ApprovedDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["ApprovedUserName"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["RemarksHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["NoOfAttachments"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["OrderQtyHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["OrderWeightHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["OrderAmountHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["DispatchQtyHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["DispatchWeightHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["DispatchAmountHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["BalanceQtyHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["BalanceWeightHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.Columns["BalanceAmountHeader"].FilterEditType = (FilterEditType)1;
			grdMain.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdMain.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					CommonServices.GridColumnSettings(column);
				}
			}
			grdMain.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdMain.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdMain.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grdMain.RootTable.Columns["RefrenenceNo"].Width = Constants.InventoryConstants.RecordNo + 30;
			grdMain.RootTable.Columns["VisitedByName"].Width = Constants.InventoryConstants.SupplierName;
			grdMain.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName;
			grdMain.RootTable.Columns["RefPartyCellNo"].Width = Constants.InventoryConstants.CellNo;
			grdMain.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.SupplierName;
			grdMain.RootTable.Columns["RefSalesMan"].Width = Constants.InventoryConstants.SupplierName;
			grdMain.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdMain.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdMain.RootTable.Columns["OrderDueDays"].Width = Constants.InventoryConstants.DueDays;
			grdMain.RootTable.Columns["OrderDueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdMain.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdMain.RootTable.Columns["DeliveryStartDate"].Width = Constants.InventoryConstants.DateConstant;
			grdMain.RootTable.Columns["TransporterName"].Width = Constants.InventoryConstants.AccountTitle;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdMain.RootTable.Columns["WagesAccount"].Width = Constants.InventoryConstants.AccountTitle;
			grdMain.RootTable.Columns["WagesAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdMain.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grdMain.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.StatusConstant;
			grdMain.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdMain.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdMain.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdMain.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdMain.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grdMain.RootTable.Columns["ApprovedUserName"].Width = Constants.InventoryConstants.ApprovedByConstant + 10;
			grdMain.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdMain.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdMain.RootTable.Columns["OrderQtyHeader"].Width = Constants.InventoryConstants.QtyConstant;
			grdMain.RootTable.Columns["OrderWeightHeader"].Width = Constants.InventoryConstants.WeightConstant;
			grdMain.RootTable.Columns["OrderAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdMain.RootTable.Columns["DispatchQtyHeader"].Width = Constants.InventoryConstants.QtyConstant;
			grdMain.RootTable.Columns["DispatchWeightHeader"].Width = Constants.InventoryConstants.WeightConstant;
			grdMain.RootTable.Columns["DispatchAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdMain.RootTable.Columns["BalanceQtyHeader"].Width = Constants.InventoryConstants.QtyConstant;
			grdMain.RootTable.Columns["BalanceWeightHeader"].Width = Constants.InventoryConstants.WeightConstant;
			grdMain.RootTable.Columns["BalanceAmountHeader"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdMain.RootTable.Columns["CurrentGlBalance"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdMain.RootTable.Columns["CurrentGlBalance"].Position = grdMain.RootTable.Columns["CustomerName"].Position + 1;
			grdMain.RootTable.Columns["CurrentGlBalance"].FormatString = "#,#;(#,#);0";
			grdMain.RootTable.Columns["CurrentGlBalance"].TotalFormatString = "#,#;(#,#);0";
			grdMain.RootTable.Columns.Remove("Complete");
			grdMain.RootTable.Columns.Remove("Cancel");
			grdMain.RootTable.Columns.Remove("ExpiryUpdate");
			grdMain.RootTable.Columns.Remove("Open");
			grdMain.RootTable.Columns.Remove("Print");
			BtnCompleteStatus.Visible = false;
			BtnCancelStatus.Visible = false;
			BtnOpenStatus.Visible = false;
			if (((Control)(object)cmbApproveStatus).Text == "Approve" && ((Control)(object)CmbOrderStatus).Text == "Open")
			{
				if (formright.CanChangeOrderStatusToComplete)
				{
					BtnCompleteStatus.Visible = true;
					grdMain.RootTable.Columns.Add("Complete");
					grdMain.RootTable.Columns["Complete"].Caption = "Complete";
					grdMain.RootTable.Columns["Complete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdMain.RootTable.Columns["Complete"].ButtonStyle = (ButtonStyle)5;
					grdMain.RootTable.Columns["Complete"].ButtonText = "Complete";
					grdMain.RootTable.Columns["Complete"].Width = 80;
				}
				if (formright.CanChangeOrderStatusToCancel)
				{
					BtnCancelStatus.Visible = true;
					grdMain.RootTable.Columns.Add("Cancel");
					grdMain.RootTable.Columns["Cancel"].Caption = "Cancel";
					grdMain.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdMain.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
					grdMain.RootTable.Columns["Cancel"].ButtonText = "Cancel";
					grdMain.RootTable.Columns["Cancel"].Width = 60;
				}
				if (formright.CanChangeOrderExpiryDate)
				{
					grdMain.AllowEdit = (InheritableBoolean)1;
					grdMain.RootTable.Columns["ExpiryDate"].EditType = (EditType)2;
					grdMain.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
					grdMain.RootTable.Columns.Add("ExpiryUpdate");
					grdMain.RootTable.Columns["ExpiryUpdate"].Key = "ExpiryUpdate";
					grdMain.RootTable.Columns["ExpiryUpdate"].Caption = "ExpiryUpdate";
					grdMain.RootTable.Columns["ExpiryUpdate"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdMain.RootTable.Columns["ExpiryUpdate"].ButtonStyle = (ButtonStyle)5;
					grdMain.RootTable.Columns["ExpiryUpdate"].ButtonText = "ExpiryUpdate";
					grdMain.RootTable.Columns["ExpiryUpdate"].Width = 100;
				}
			}
			if (((Control)(object)cmbApproveStatus).Text == "Approve" && (((Control)(object)CmbOrderStatus).Text == "Complete" || ((Control)(object)CmbOrderStatus).Text == "Cancel") && formright.CanChangeOrderStatusToOpen)
			{
				BtnOpenStatus.Visible = true;
				grdMain.RootTable.Columns.Add("Open");
				grdMain.RootTable.Columns["Open"].Key = "Open";
				grdMain.RootTable.Columns["Open"].Caption = "Open";
				grdMain.RootTable.Columns["Open"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdMain.RootTable.Columns["Open"].ButtonStyle = (ButtonStyle)5;
				grdMain.RootTable.Columns["Open"].ButtonText = "Open";
				grdMain.RootTable.Columns["Open"].Width = 50;
			}
			if (grdMain.RootTable.Columns.Contains("Open") || grdMain.RootTable.Columns.Contains("Cancel") || grdMain.RootTable.Columns.Contains("Complete"))
			{
				grdMain.RootTable.Columns.Add("Select");
				grdMain.RootTable.Columns["Select"].ActAsSelector = true;
				grdMain.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdMain.RootTable.Columns["Select"].Width = 40;
			}
			grdMain.RootTable.Columns.Add("Print");
			grdMain.RootTable.Columns["Print"].Caption = "Print";
			grdMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdMain.RootTable.Columns["Print"].ButtonText = "Print";
			grdMain.RootTable.Columns["Print"].Width = 60;
			if (grdMain.RootTable.Columns.Contains("Select"))
			{
				grdMain.RootTable.Columns["Select"].Position = grdMain.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdMain.RootTable.Columns.Contains("Print"))
			{
				grdMain.RootTable.Columns["Print"].Position = grdMain.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdMain.RootTable.Columns.Contains("ExpiryUpdate"))
			{
				grdMain.RootTable.Columns["ExpiryUpdate"].Position = grdMain.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdMain.RootTable.Columns.Contains("Complete"))
			{
				grdMain.RootTable.Columns["Complete"].Position = grdMain.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdMain.RootTable.Columns.Contains("Open"))
			{
				grdMain.RootTable.Columns["Open"].Position = grdMain.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdMain.RootTable.Columns.Contains("Cancel"))
			{
				grdMain.RootTable.Columns["Cancel"].Position = grdMain.RootTable.Columns["DocNo"].Position - 1;
			}
			grdMain.RootTable.Columns["NoOfAttachments"].Position = grdMain.RootTable.Columns["DocNo"].Position - 1;
			grdMain.FrozenColumns = grdMain.RootTable.Columns["DocNo"].Position - 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1852);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (e.Column.Key == "Print")
				{
					CommonServices.SaleOrderSlipConcrete(Id);
				}
				if (e.Column.Key == "Complete")
				{
					ChangeOrderStatus(Id, "Complete");
				}
				if (e.Column.Key == "Open")
				{
					ChangeOrderStatus(Id, "Open");
				}
				if (e.Column.Key == "Cancel")
				{
					ChangeOrderStatus(Id, "Cancel", Conversion.ToDouble(r.Cells["DispatchQtyHeader"].Value));
				}
				if (e.Column.Key == "ExpiryUpdate")
				{
					UpdateExpiryDate(Id, Conversion.ToDateTime(r.Cells["ExpiryDate"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalMainDetail();
			DetailGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdMain.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (item.Cells.Count > 0)
				{
					DataTable dtOrder = new DataTable();
					dtOrder.Columns.Add("Id", typeof(int));
					dtOrder.Columns.Add("DetailId", typeof(int));
					dtOrder.Columns.Add("OrderItemId", typeof(int));
					dtOrder.Columns.Add("SaleGLAC", typeof(int));
					dtOrder.Columns.Add("ItemCode", typeof(string));
					dtOrder.Columns.Add("ItemName", typeof(string));
					dtOrder.Columns.Add("ItemAttributeVarientId", typeof(int));
					dtOrder.Columns.Add("ItemArrtibuteVarient", typeof(string));
					dtOrder.Columns.Add("VarientEquivalent", typeof(decimal));
					dtOrder.Columns.Add("JobLotId", typeof(int));
					dtOrder.Columns.Add("JobLotDescription", typeof(string));
					dtOrder.Columns.Add("ItemQty", typeof(decimal));
					dtOrder.Columns.Add("DispatchQty", typeof(decimal));
					dtOrder.Columns.Add("BalanceQty", typeof(decimal));
					dtOrder.Columns.Add("ItemWeight", typeof(decimal));
					dtOrder.Columns.Add("NetWeight", typeof(decimal));
					dtOrder.Columns.Add("DispatchWeight", typeof(decimal));
					dtOrder.Columns.Add("BalanceWeight", typeof(decimal));
					dtOrder.Columns.Add("ItemRate", typeof(decimal));
					dtOrder.Columns.Add("ItemAmount", typeof(decimal));
					dtOrder.Columns.Add("ItemDiscountTypeId", typeof(int));
					dtOrder.Columns.Add("ItemDiscountType", typeof(string));
					dtOrder.Columns.Add("ItemDiscountRate", typeof(decimal));
					dtOrder.Columns.Add("ItemDiscountAmount", typeof(decimal));
					dtOrder.Columns.Add("ItemNetAmount", typeof(decimal));
					dtOrder.Columns.Add("DispatchAmount", typeof(decimal));
					dtOrder.Columns.Add("BalanceAmount", typeof(decimal));
					dtOrder.Columns.Add("CityId", typeof(int));
					dtOrder.Columns.Add("CityName", typeof(string));
					dtOrder.Columns.Add("RemarksDetail", typeof(string));
					dtOrder.Rows.Clear();
					DataTable resultRows = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dtOrder.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["DetailId"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["SaleGLAC"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemAttributeVarientId"], resultRows.Rows[i]["ItemArrtibuteVarient"], resultRows.Rows[i]["VarientEquivalent"], resultRows.Rows[i]["JobLotId"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["ItemQty"], resultRows.Rows[i]["DispatchQty"], resultRows.Rows[i]["BalanceQty"], resultRows.Rows[i]["ItemWeight"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["DispatchWeight"], resultRows.Rows[i]["BalanceWeight"], resultRows.Rows[i]["ItemRate"], resultRows.Rows[i]["ItemAmount"], resultRows.Rows[i]["ItemDiscountTypeId"], resultRows.Rows[i]["ItemDiscountType"], resultRows.Rows[i]["ItemDiscountRate"], resultRows.Rows[i]["ItemDiscountAmount"], resultRows.Rows[i]["ItemNetAmount"], resultRows.Rows[i]["DispatchAmount"], resultRows.Rows[i]["BalanceAmount"], resultRows.Rows[i]["CityId"], resultRows.Rows[i]["CityName"], resultRows.Rows[i]["RemarksDetail"]);
					}
					grdDetail.DataSource = dtOrder;
					grdDetail.RetrieveStructure();
					GridDetailSetting();
				}
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Expected O, but got Unknown
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["DetailId"].Visible = false;
			grdDetail.RootTable.Columns["SaleGLAC"].Visible = false;
			grdDetail.RootTable.Columns["ItemDiscountTypeId"].Visible = false;
			grdDetail.RootTable.Columns["ItemAttributeVarientId"].Visible = false;
			grdDetail.RootTable.Columns["OrderItemId"].Visible = false;
			grdDetail.RootTable.Columns["CityId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotDescription"].Caption = "JobLot";
			grdDetail.RootTable.Columns["ItemArrtibuteVarient"].Caption = "Varient";
			grdDetail.RootTable.Columns["VarientEquivalent"].Caption = "Varient Unit";
			grdDetail.RootTable.Columns["ItemDiscountRate"].Caption = "Disc Rate";
			grdDetail.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					CommonServices.GridColumnSettings(column);
				}
			}
			grdDetail.RootTable.Columns["SaleGLAC"].Width = Constants.InventoryConstants.AccountTitle;
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["ItemAttributeVarientId"].Width = Constants.InventoryConstants.Variant;
			grdDetail.RootTable.Columns["ItemArrtibuteVarient"].Width = Constants.InventoryConstants.Variant;
			grdDetail.RootTable.Columns["VarientEquivalent"].Width = Constants.InventoryConstants.VariantEquivalent;
			grdDetail.RootTable.Columns["JobLotId"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["JobLotDescription"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["DispatchQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["BalanceQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["ItemWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["DispatchWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["BalanceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ItemDiscountTypeId"].Width = Constants.InventoryConstants.DiscountType;
			grdDetail.RootTable.Columns["ItemDiscountType"].Width = Constants.InventoryConstants.DiscountType;
			grdDetail.RootTable.Columns["ItemDiscountRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["ItemDiscountAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdDetail.RootTable.Columns["ItemNetAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdDetail.RootTable.Columns["DispatchAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdDetail.RootTable.Columns["BalanceAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdDetail.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grdDetail.RootTable.Columns["RemarksDetail"].Width = Constants.InventoryConstants.RemarksConstants;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ChangeOrderStatus(int Id, string Status, double DispatchedQty = 0.0)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		try
		{
			if (Status == "Cancel" && DispatchedQty > 0.0)
			{
				MessageBox.Show("Record Not Cancel because Dispatch Qty greater than zero");
				btnReset_Click(null, null);
				return;
			}
			string Remarks = "";
			if (MessageBox.Show("Are you sure you want to change the status to '" + Status + "'?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				item.Id = Id;
				item.ReqType = Status;
				item.EntryUser = UserAccount.ID;
				item.Comments = Remarks;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateSaleOrderStatusById(val);
				MessageBox.Show("Status Updated To '" + Status + "' Successfully");
				FillMainGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ChangeOrderStatusOfGridSelectedRows(GridEX grd, string Status)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		try
		{
			if (Status == "Cancel")
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					if (Conversion.ToInt(r2.Cells["DispatchQtyHeader"].Value) > 0)
					{
						MessageBox.Show("Record Not Cancel because Dispatch Qty greater than zero in Row No " + (r2.RowIndex + 1));
						btnReset_Click(null, null);
						return;
					}
				}
			}
			string Remarks = "";
			if (MessageBox.Show("Are you sure you want to change the status to '" + Status + "' of Selected Rows?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList val = new ApprovalList
					{
						AllApprovalLists = new List<ApprovalList>()
					};
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.BranchesId = UserAccount.BranchesId;
					item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.ReqType = Status;
					item.EntryUser = UserAccount.ID;
					item.Comments = Remarks;
					val.AllApprovalLists.Add(item);
					SaleOrder.UpdateSaleOrderStatusById(val);
				}
				MessageBox.Show("Status Update To '" + Status + "' Successfully of Selected Rows");
				FillMainGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UpdateExpiryDate(int Id, DateTime PostData)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		try
		{
			string Remarks = "";
			if (MessageBox.Show("Are you sure to Update ExpiryDate?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				item.Id = Id;
				item.ReqType = "UpdateExpiryDate";
				item.Comments = Remarks;
				item.EntryUser = UserAccount.ID;
				item.PostDate = PostData;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateSaleOrderStatusById(val);
				MessageBox.Show("Update ExpiryDate Successfully");
				FillMainGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnOpenStatus_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdMain.GetCheckedRows().Count() > 0)
			{
				ChangeOrderStatusOfGridSelectedRows(grdMain, "Open");
				return;
			}
			throw new Exception("Please Check the Row First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelStatus_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdMain.GetCheckedRows().Count() > 0)
			{
				ChangeOrderStatusOfGridSelectedRows(grdMain, "Cancel");
				return;
			}
			throw new Exception("Please Check the Row First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCompleteStatus_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdMain.GetCheckedRows().Count() > 0)
			{
				ChangeOrderStatusOfGridSelectedRows(grdMain, "Complete");
				return;
			}
			throw new Exception("Please Check the Row First...");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdMain).Name);
			if (fs != null)
			{
				grdMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOrderNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void LoadPurchaseOrder_KeyDown(object sender, KeyEventArgs e)
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
					btnReset_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btngrnlod_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					BtnRegister_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					BtnRegister_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					printRegister_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdMain).Focus();
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
				((Control)(object)DataGridHistory).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void printRegister_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "1852-SaleOrderRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRegister_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "1852A-SaleOrder_Register.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShowSumm_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				GridSummaryFill();
				PrintButtonManage();
			}
			else
			{
				MessageBox.Show("Please Select Activity First...");
				cmbReportType.Focus();
			}
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
			((Control)(object)cmbCustomerSumm).Text = string.Empty;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((Control)(object)cmbDistrictSumm).Text = string.Empty;
			((Control)(object)CmbApprovalStatusSumm).Text = string.Empty;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			((Control)(object)CmbOrderStatusSumm).Text = string.Empty;
			txtFromDocNoSumm.Clear();
			txtToDocNoSumm.Clear();
			((UltraGridBase)cmbReportType).Rows[0].Activate();
			BtnShowSumm_Click(null, null);
			txtFromDateSumm.Focus();
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
			((Control)(object)cmbCustomerSumm).Text = string.Empty;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((Control)(object)cmbDistrictSumm).Text = string.Empty;
			((Control)(object)CmbApprovalStatusSumm).Text = string.Empty;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			((Control)(object)CmbOrderStatusSumm).Text = string.Empty;
			txtFromDocNoSumm.Clear();
			txtToDocNoSumm.Clear();
			((UltraGridBase)cmbReportType).Rows[0].Activate();
			BtnShowSumm_Click(null, null);
			txtFromDateSumm.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			obj.ParentCategoryId = Conversion.ToInt(cmbParentcategorySumm.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbCategorySumm.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtypeSumm.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbCustomerSumm.Value);
			obj.ItemId = Conversion.ToInt(cmbItemNameSumm.Value);
			obj.JobLotId = Conversion.ToInt(cmbJobLotSumm.Value);
			obj.CityId = Conversion.ToInt(cmbcitySumm.Value);
			obj.DistrictId = Conversion.ToInt(cmbDistrictSumm.Value);
			obj.ReportType = ((Control)(object)cmbReportType).Text;
			if (Conversion.ToString((object)((Control)(object)CmbOrderStatusSumm).Text) != "All")
			{
				obj.Status = Conversion.ToString((object)((Control)(object)CmbOrderStatusSumm).Text);
			}
			if (((Control)(object)CmbApprovalStatusSumm).Text == "Approve")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)CmbApprovalStatusSumm).Text == "UnApproved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)CmbApprovalStatusSumm).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			obj.ReferencePartyName = Conversion.ToString((object)((Control)(object)cmbRefPartyDetailSumm).Text);
			obj.CustomerGroupId = Conversion.ToInt(CmbPartyGroup.Value);
			obj.RefSaleManId = Conversion.ToInt(cmbsaleman.Value);
			string RefPartyIds = "";
			if (((Control)(object)CmbRegisteredRefParty).Text != null && ((Control)(object)CmbRegisteredRefParty).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbRegisteredRefParty).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtRegisteredRefParty.Select("name='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						RefPartyIds = RefPartyIds + Conversion.ToString(dr[0].ItemArray[0]) + ",";
					}
				}
				obj.RefPartyIds = RefPartyIds;
			}
			dtGridSummary = SaleOrder.SaleOrderSummaryRegister(obj);
			if (dtGridSummary.Rows.Count > 0)
			{
				if (((UltraGridBase)cmbReportType).ActiveRow != null)
				{
					if (((Control)(object)cmbReportType).Text == "Order Register")
					{
						((Control)(object)DataGridHistory).Name = "OrderRegister";
						dtOrderRegister.Rows.Clear();
						for (int i = 0; i < dtGridSummary.Rows.Count; i++)
						{
							dtOrderRegister.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i]["Id"]), Conversion.ToInt(dtGridSummary.Rows[i]["DocumentTypeId"]), Conversion.ToString(dtGridSummary.Rows[i]["DocumentTypeCode"]), Conversion.ToDateTime(dtGridSummary.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToInt(dtGridSummary.Rows[i]["DocNo"]), Conversion.ToString(dtGridSummary.Rows[i]["Customer"]), Conversion.ToString(dtGridSummary.Rows[i]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i]["RefPartyAddress"]), Conversion.ToString(dtGridSummary.Rows[i]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[i]["ItemName"]), Conversion.ToString(dtGridSummary.Rows[i]["JobLotCode"]), Conversion.ToString(dtGridSummary.Rows[i]["UOMCode"]), Conversion.ToDouble(dtGridSummary.Rows[i]["UomEquivalent"]), Conversion.ToDouble(dtGridSummary.Rows[i]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i]["PrcntOfTotal"]), Conversion.ToString(dtGridSummary.Rows[i]["CityName"]), Conversion.ToString(dtGridSummary.Rows[i]["BuildingStorey"]), Conversion.ToString(dtGridSummary.Rows[i]["BuildingHeight"]), Conversion.ToString(dtGridSummary.Rows[i]["BuildingArea"]));
						}
						DataGridHistory.DataSource = dtOrderRegister;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
						DataGridHistory.RootTable.Columns["Id"].Visible = false;
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By OrderNo & Customer")
					{
						((Control)(object)DataGridHistory).Name = "dtOrderSummaryByOrderNoAndCustomer";
						dtOrderSummaryByOrderNoAndCustomer.Rows.Clear();
						for (int j = 0; j < dtGridSummary.Rows.Count; j++)
						{
							dtOrderSummaryByOrderNoAndCustomer.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[j]["Id"]), Conversion.ToDateTime(dtGridSummary.Rows[j]["DocDate"]), Conversion.ToInt(dtGridSummary.Rows[j]["DocNo"]), Conversion.ToString(dtGridSummary.Rows[j]["Customer"]), Conversion.ToString(dtGridSummary.Rows[j]["ReferenceNo"]), Conversion.ToString(dtGridSummary.Rows[j]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[j]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[j]["RefPartyAddress"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["OrderQty"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["OrderWeight"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["OrderAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["DiscountAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["LabourAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["CarriageAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["CommissionAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["NetOrderAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["AvgRate"]), Conversion.ToDecimal(dtGridSummary.Rows[j]["PrcntOfTotal"]), Conversion.ToString(dtGridSummary.Rows[j]["BuildingStorey"]), Conversion.ToString(dtGridSummary.Rows[j]["BuildingHeight"]), Conversion.ToString(dtGridSummary.Rows[j]["BuildingArea"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByOrderNoAndCustomer;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By OrderNo Customer & SalesMan")
					{
						((Control)(object)DataGridHistory).Name = "dtOrderSummaryByOrderNoAndSalesMan";
						dtOrderSummaryByOrderNoAndSalesMan.Rows.Clear();
						for (int k = 0; k < dtGridSummary.Rows.Count; k++)
						{
							dtOrderSummaryByOrderNoAndSalesMan.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[k]["Id"]), Conversion.ToDateTime(dtGridSummary.Rows[k]["DocDate"]), Conversion.ToInt(dtGridSummary.Rows[k]["DocNo"]), Conversion.ToString(dtGridSummary.Rows[k]["CommissionAgent"]), Conversion.ToString(dtGridSummary.Rows[k]["Customer"]), Conversion.ToString(dtGridSummary.Rows[k]["ReferenceNo"]), Conversion.ToString(dtGridSummary.Rows[k]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[k]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[k]["RefPartyAddress"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["OrderQty"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["OrderWeight"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["OrderAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["DiscountAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["LabourAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["CarriageAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["CommissionAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["NetOrderAmount"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["AvgRate"]), Conversion.ToDecimal(dtGridSummary.Rows[k]["PrcntOfTotal"]), Conversion.ToString(dtGridSummary.Rows[k]["BuildingStorey"]), Conversion.ToString(dtGridSummary.Rows[k]["BuildingHeight"]), Conversion.ToString(dtGridSummary.Rows[k]["BuildingArea"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByOrderNoAndSalesMan;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Item")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByItem";
						dtOrderSummaryByItem.Rows.Clear();
						for (int l = 0; l < dtGridSummary.Rows.Count; l++)
						{
							dtOrderSummaryByItem.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[l]["Id"]), Conversion.ToString(dtGridSummary.Rows[l]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[l]["ItemName"]), Conversion.ToDouble(dtGridSummary.Rows[l]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[l]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[l]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[l]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[l]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[l]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[l]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[l]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[l]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[l]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[l]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Item & Varient")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByItem&Varient";
						dtOrderSummaryByItemandVarient.Rows.Clear();
						for (int m = 0; m < dtGridSummary.Rows.Count; m++)
						{
							dtOrderSummaryByItemandVarient.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[m]["Id"]), Conversion.ToString(dtGridSummary.Rows[m]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[m]["ItemName"]), Conversion.ToString(dtGridSummary.Rows[m]["UOMCode"]), Conversion.ToDouble(dtGridSummary.Rows[m]["UomEquivalent"]), Conversion.ToDouble(dtGridSummary.Rows[m]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[m]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[m]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[m]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[m]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[m]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[m]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[m]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[m]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[m]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[m]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByItemandVarient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Item & City")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByItemAndCity";
						dtOrderSummaryByItemandCity.Rows.Clear();
						for (int n = 0; n < dtGridSummary.Rows.Count; n++)
						{
							dtOrderSummaryByItemandCity.Rows.Add(dtGridSummary.Rows[n]["Id"], Conversion.ToString(dtGridSummary.Rows[n]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[n]["ItemName"]), Conversion.ToDouble(dtGridSummary.Rows[n]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[n]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[n]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[n]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[n]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[n]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[n]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[n]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[n]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[n]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[n]["PrcntOfTotal"]), Conversion.ToString(dtGridSummary.Rows[n]["CityName"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByItemandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Item_Varient & City")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByItem,Varient&City";
						dtOrderSummaryByItemVarientAndCity.Rows.Clear();
						for (int i2 = 0; i2 < dtGridSummary.Rows.Count; i2++)
						{
							dtOrderSummaryByItemVarientAndCity.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i2]["Id"]), Conversion.ToString(dtGridSummary.Rows[i2]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[i2]["ItemName"]), Conversion.ToString(dtGridSummary.Rows[i2]["UOMCode"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["UomEquivalent"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i2]["PrcntOfTotal"]), Conversion.ToString(dtGridSummary.Rows[i2]["CityName"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByItemVarientAndCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomer";
						dtOrderSummaryByCustomer.Rows.Clear();
						for (int i3 = 0; i3 < dtGridSummary.Rows.Count; i3++)
						{
							dtOrderSummaryByCustomer.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i3]["Id"]), Conversion.ToString(dtGridSummary.Rows[i3]["Customer"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i3]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomer;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer & Item")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerAndItem";
						dtOrderSummaryByCustomerandItem.Rows.Clear();
						for (int i4 = 0; i4 < dtGridSummary.Rows.Count; i4++)
						{
							dtOrderSummaryByCustomerandItem.Rows.Add(dtGridSummary.Rows[i4]["Id"], Conversion.ToString(dtGridSummary.Rows[i4]["Customer"]), Conversion.ToString(dtGridSummary.Rows[i4]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[i4]["ItemName"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i4]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomerandItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer & City")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomer&City";
						dtOrderSummaryByCustomerandCity.Rows.Clear();
						for (int i5 = 0; i5 < dtGridSummary.Rows.Count; i5++)
						{
							dtOrderSummaryByCustomerandCity.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i5]["Id"]), Conversion.ToString(dtGridSummary.Rows[i5]["Customer"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i5]["PrcntOfTotal"]), Conversion.ToString(dtGridSummary.Rows[i5]["CityName"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomerandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer_Item & City")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomer,Item&City";
						dtOrderSummaryByCustomerItemandCity.Rows.Clear();
						for (int i6 = 0; i6 < dtGridSummary.Rows.Count; i6++)
						{
							dtOrderSummaryByCustomerItemandCity.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i6]["Id"]), Conversion.ToString(dtGridSummary.Rows[i6]["Customer"]), Conversion.ToString(dtGridSummary.Rows[i6]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[i6]["ItemName"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i6]["PrcntOfTotal"]), Conversion.ToString(dtGridSummary.Rows[i6]["CityName"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomerItemandCity;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer & Varient")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomer&Varient";
						dtOrderSummaryByCustomerandVarient.Rows.Clear();
						for (int i7 = 0; i7 < dtGridSummary.Rows.Count; i7++)
						{
							dtOrderSummaryByCustomerandVarient.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i7]["Id"]), Conversion.ToString(dtGridSummary.Rows[i7]["Customer"]), Conversion.ToString(dtGridSummary.Rows[i7]["UOMCode"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["UomEquivalent"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i7]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomerandVarient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer_Item & Varient")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomer_ItemVarient";
						dtOrderSummaryByCustomerItemAndvarient.Rows.Clear();
						for (int i8 = 0; i8 < dtGridSummary.Rows.Count; i8++)
						{
							dtOrderSummaryByCustomerItemAndvarient.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i8]["Id"]), Conversion.ToString(dtGridSummary.Rows[i8]["Customer"]), Conversion.ToString(dtGridSummary.Rows[i8]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[i8]["ItemName"]), Conversion.ToString(dtGridSummary.Rows[i8]["UOMCode"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["UomEquivalent"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i8]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomerItemAndvarient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer & ReferenceParty")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerandReferenceParty";
						dtOrderSummaryByCustomerandReferenceParty.Rows.Clear();
						for (int i9 = 0; i9 < dtGridSummary.Rows.Count; i9++)
						{
							dtOrderSummaryByCustomerandReferenceParty.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i9]["Id"]), Conversion.ToString(dtGridSummary.Rows[i9]["Customer"]), Conversion.ToString(dtGridSummary.Rows[i9]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i9]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i9]["RefPartyAddress"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i9]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomerandReferenceParty;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer Item & ReferenceParty")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomer&Item&ReferenceParty";
						dtOrderSummaryByCustomerItemandReferenceParty.Rows.Clear();
						for (int i10 = 0; i10 < dtGridSummary.Rows.Count; i10++)
						{
							dtOrderSummaryByCustomerItemandReferenceParty.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i10]["Id"]), Conversion.ToString(dtGridSummary.Rows[i10]["Customer"]), Conversion.ToString(dtGridSummary.Rows[i10]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[i10]["ItemName"]), Conversion.ToString(dtGridSummary.Rows[i10]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i10]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i10]["RefPartyAddress"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i10]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomerItemandReferenceParty;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer Varient & ReferenceParty")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerVarient&ReferenceParty";
						dtOrderSummaryByCustomerVarientandReferenceParty.Rows.Clear();
						for (int i11 = 0; i11 < dtGridSummary.Rows.Count; i11++)
						{
							dtOrderSummaryByCustomerVarientandReferenceParty.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i11]["Id"]), Conversion.ToString(dtGridSummary.Rows[i11]["Customer"]), Conversion.ToString(dtGridSummary.Rows[i11]["UOMCode"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["UomEquivalent"]), Conversion.ToString(dtGridSummary.Rows[i11]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i11]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i11]["RefPartyAddress"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i11]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomerVarientandReferenceParty;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer_Item_Varient & ReferenceParty")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomer_Item_Varient&ReferenceParty";
						dtOrderSummaryByCustomerItemVarientandReferenceParty.Rows.Clear();
						for (int i12 = 0; i12 < dtGridSummary.Rows.Count; i12++)
						{
							dtOrderSummaryByCustomerItemVarientandReferenceParty.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i12]["Id"]), Conversion.ToString(dtGridSummary.Rows[i12]["Customer"]), Conversion.ToString(dtGridSummary.Rows[i12]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[i12]["ItemName"]), Conversion.ToString(dtGridSummary.Rows[i12]["UOMCode"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["UomEquivalent"]), Conversion.ToString(dtGridSummary.Rows[i12]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i12]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i12]["RefPartyAddress"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i12]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByCustomerItemVarientandReferenceParty;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByReferenceParty";
						dtOrderSummaryByReferenceParty.Rows.Clear();
						for (int i13 = 0; i13 < dtGridSummary.Rows.Count; i13++)
						{
							dtOrderSummaryByReferenceParty.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i13]["Id"]), Conversion.ToString(dtGridSummary.Rows[i13]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i13]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i13]["RefPartyAddress"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i13]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByReferenceParty;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty & Item")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByReferenceParty&Item";
						dtOrderSummaryByReferencePartyAndItem.Rows.Clear();
						for (int i14 = 0; i14 < dtGridSummary.Rows.Count; i14++)
						{
							dtOrderSummaryByReferencePartyAndItem.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i14]["Id"]), Conversion.ToString(dtGridSummary.Rows[i14]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i14]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i14]["RefPartyAddress"]), Conversion.ToString(dtGridSummary.Rows[i14]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[i14]["ItemName"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i14]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByReferencePartyAndItem;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty & Varient")
					{
						((Control)(object)DataGridHistory).Name = "Order Summary By ReferenceParty & Varient";
						dtOrderSummaryByReferencePartyAndVarient.Rows.Clear();
						for (int i15 = 0; i15 < dtGridSummary.Rows.Count; i15++)
						{
							dtOrderSummaryByReferencePartyAndVarient.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i15]["Id"]), Conversion.ToString(dtGridSummary.Rows[i15]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i15]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i15]["RefPartyAddress"]), Conversion.ToString(dtGridSummary.Rows[i15]["UOMCode"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["UomEquivalent"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i15]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByReferencePartyAndVarient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty_Item & Varient")
					{
						((Control)(object)DataGridHistory).Name = "Order Summary By ReferenceParty_Item & Varient";
						dtOrderSummaryByReferencePartyItemAndVarient.Rows.Clear();
						for (int i16 = 0; i16 < dtGridSummary.Rows.Count; i16++)
						{
							dtOrderSummaryByReferencePartyItemAndVarient.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i16]["Id"]), Conversion.ToString(dtGridSummary.Rows[i16]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i16]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i16]["RefPartyAddress"]), Conversion.ToString(dtGridSummary.Rows[i16]["ItemCode"]), Conversion.ToString(dtGridSummary.Rows[i16]["ItemName"]), Conversion.ToString(dtGridSummary.Rows[i16]["UOMCode"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["UomEquivalent"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i16]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByReferencePartyItemAndVarient;
						DataGridHistory.RetrieveStructure();
						DataGridHistorySetting();
					}
					else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty & City")
					{
						((Control)(object)DataGridHistory).Name = "OrderSummaryByReferenceParty&City";
						dtOrderSummaryByReferencePartyAndCity.Rows.Clear();
						for (int i17 = 0; i17 < dtGridSummary.Rows.Count; i17++)
						{
							dtOrderSummaryByReferencePartyAndCity.Rows.Add(Conversion.ToInt(dtGridSummary.Rows[i17]["Id"]), Conversion.ToString(dtGridSummary.Rows[i17]["CityName"]), Conversion.ToString(dtGridSummary.Rows[i17]["ReferencePartyDetail"]), Conversion.ToString(dtGridSummary.Rows[i17]["RefPartyCellNo"]), Conversion.ToString(dtGridSummary.Rows[i17]["RefPartyAddress"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["OrderQty"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["DispatchQty"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["BalQty"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["OrderWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["DispatchWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["BalWeight"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["OrderAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["DispatchAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["BalAmount"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["AvgRate"]), Conversion.ToDouble(dtGridSummary.Rows[i17]["PrcntOfTotal"]));
						}
						DataGridHistory.DataSource = dtOrderSummaryByReferencePartyAndCity;
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
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					CommonServices.GridColumnSettings(column);
				}
			}
			if (((Control)(object)cmbReportType).Text == "Order Summary By OrderNo & Customer" || ((Control)(object)cmbReportType).Text == "Order Summary By OrderNo Customer & SalesMan")
			{
				DataGridHistory.RootTable.Columns["Id"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
				DataGridHistory.RootTable.Columns["DocNo"].Caption = "Order No";
				DataGridHistory.RootTable.Columns["PercentOfTotal"].Visible = false;
				DataGridHistory.RootTable.Columns["AvgRate"].Visible = false;
			}
			else
			{
				DataGridHistory.RootTable.Columns["Id"].Visible = false;
			}
			_ = ((Control)(object)cmbReportType).Text == "Order Register";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl2.SelectedIndex == 1)
		{
			txtFromDateSumm.Focus();
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "DocNo")
			{
				CommonServices.SaleOrderSlipConcrete(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
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
		try
		{
			if (btnPrintSumm.Text == "1852_01-OrderRegister")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGridSummary, "1852_01-OrderRegister.rpt");
			}
			else if (btnPrintSumm.Text == "1852_02-OrderSummaryByItem")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtGridSummary, "1852_02-OrderSummaryByItem.rpt");
			}
			else if (btnPrintSumm.Text == "1852_03-OrderSummaryByItem&Varient")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtGridSummary, "1852_03-OrderSummaryByItem&Varient.rpt");
			}
			else if (btnPrintSumm.Text == "1852_04-OrderSummaryByItem&City")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtGridSummary, "1852_04-OrderSummaryByItem&City.rpt");
			}
			else if (btnPrintSumm.Text == "1852_05-OrderSummaryByItem_Varient&City")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtGridSummary, "1852_05-OrderSummaryByItem_Varient&City.rpt");
			}
			else if (btnPrintSumm.Text == "1852_06-OrderSummaryByCustomer")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtGridSummary, "1852_06-OrderSummaryByCustomer.rpt");
			}
			else if (btnPrintSumm.Text == "1852_07-OrderSummaryByCustomerItem")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtGridSummary, "1852_07-OrderSummaryByCustomerItem.rpt");
			}
			else if (btnPrintSumm.Text == "1852_08-OrderSummaryByCustomer&City")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtGridSummary, "1852_08-OrderSummaryByCustomer&City.rpt");
			}
			else if (btnPrintSumm.Text == "1852_09-OrderSummaryByCustomer_Item&City")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtGridSummary, "1852_09-OrderSummaryByCustomer_Item&City.rpt");
			}
			else if (btnPrintSumm.Text == "1852_10-OrderSummaryByCustomer&Varient")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtGridSummary, "1852_10-OrderSummaryByCustomer&Varient.rpt");
			}
			else if (btnPrintSumm.Text == "1852_11-OrderSummarByCustomer_ItemVarient")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtGridSummary, "1852_11-OrderSummaryByCustomer_ItemVarient.rpt");
			}
			else if (btnPrintSumm.Text == "1852_12-OrderSummaryByCustomer&ReferenceParty")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val12 = new Reporting();
				val12.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val12.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val12.ShowReportWithDataTable(dtGridSummary, "1852_12-OrderSummaryByCustomer&ReferenceParty.rpt");
			}
			else if (btnPrintSumm.Text == "1852_13-OrderSummaryByCustomerItem&ReferenceParty")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val13 = new Reporting();
				val13.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val13.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val13.ShowReportWithDataTable(dtGridSummary, "1852_13-OrderSummaryByCustomerItem&ReferenceParty.rpt");
			}
			else if (btnPrintSumm.Text == "1852_14-OrderSummaryByCustomerVarient&ReferenceParty")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val14 = new Reporting();
				val14.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val14.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val14.ShowReportWithDataTable(dtGridSummary, "1852_14-OrderSummaryByCustomerVarient&ReferenceParty.rpt");
			}
			else if (btnPrintSumm.Text == "1852_15-OrderSummaryByCustomer_Item_Varient&ReferenceParty")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val15 = new Reporting();
				val15.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val15.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val15.ShowReportWithDataTable(dtGridSummary, "1852_15-OrderSummaryByCustomer_Item_Varient&ReferenceParty.rpt");
			}
			else if (btnPrintSumm.Text == "1852_16-OrderSummaryByReferenceParty")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val16 = new Reporting();
				val16.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val16.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val16.ShowReportWithDataTable(dtGridSummary, "1852_16-OrderSummaryByReferenceParty.rpt");
			}
			else if (btnPrintSumm.Text == "1852_17-OrderSummaryByReferenceParty&Item")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val17 = new Reporting();
				val17.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val17.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val17.ShowReportWithDataTable(dtGridSummary, "1852_17-OrderSummaryByReferenceParty&Item.rpt");
			}
			else if (btnPrintSumm.Text == "1852_18-OrderSummaryByReferenceParty&Varient")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val18 = new Reporting();
				val18.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val18.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val18.ShowReportWithDataTable(dtGridSummary, "1852_18-OrderSummaryByReferenceParty&Varient.rpt");
			}
			else if (btnPrintSumm.Text == "1852_19-OrderSummaryByReferenceParty_Item&Varient")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val19 = new Reporting();
				val19.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val19.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val19.ShowReportWithDataTable(dtGridSummary, "1852_19-OrderSummaryByReferenceParty_Item&Varient.rpt");
			}
			else if (btnPrintSumm.Text == "1852_20-OrderSummaryByReferenceParty&City")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val20 = new Reporting();
				val20.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val20.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val20.ShowReportWithDataTable(dtGridSummary, "1852_20-OrderSummaryByReferenceParty&City.rpt");
			}
			else if (btnPrintSumm.Text == "1852_21-dtOrderSummaryByOrderNoAndCustomer")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val21 = new Reporting();
				val21.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val21.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val21.ShowReportWithDataTable(dtGridSummary, "1852_21-dtOrderSummaryByOrderNoAndCustomer.rpt");
			}
			else if (btnPrintSumm.Text == "1852_22-OrderSummaryByOrderNoAndSalesMan")
			{
				if (dtGridSummary.Rows.Count == 0 || dtGridSummary == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val22 = new Reporting();
				val22.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val22.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val22.ShowReportWithDataTable(dtGridSummary, "1852_22-OrderSummaryByOrderNoAndSalesMan.rpt");
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
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				btnPrintSumm.Visible = true;
				if (((Control)(object)cmbReportType).Text == "Order Register")
				{
					btnPrintSumm.Text = "1852_01-OrderRegister";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Item")
				{
					btnPrintSumm.Text = "1852_02-OrderSummaryByItem";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Item & Varient")
				{
					btnPrintSumm.Text = "1852_03-OrderSummaryByItem&Varient";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Item & City")
				{
					btnPrintSumm.Text = "1852_04-OrderSummaryByItem&City";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Item_Varient & City")
				{
					btnPrintSumm.Text = "1852_05-OrderSummaryByItem_Varient&City";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer")
				{
					btnPrintSumm.Text = "1852_06-OrderSummaryByCustomer";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer Item")
				{
					btnPrintSumm.Text = "1852_07-OrderSummaryByCustomerItem";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer & City")
				{
					btnPrintSumm.Text = "1852_08-OrderSummaryByCustomer&City";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer_Item & City")
				{
					btnPrintSumm.Text = "1852_09-OrderSummaryByCustomer_Item&City";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer & Varient")
				{
					btnPrintSumm.Text = "1852_10-OrderSummaryByCustomer&Varient";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer_Item & Varient")
				{
					btnPrintSumm.Text = "1852_11-OrderSummarByCustomer_ItemVarient";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer & ReferenceParty")
				{
					btnPrintSumm.Text = "1852_12-OrderSummaryByCustomer&ReferenceParty";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer Item & ReferenceParty")
				{
					btnPrintSumm.Text = "1852_13-OrderSummaryByCustomerItem&ReferenceParty";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer Varient & ReferenceParty")
				{
					btnPrintSumm.Text = "1852_14-OrderSummaryByCustomerVarient&ReferenceParty";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By Customer_Item_Varient & ReferenceParty")
				{
					btnPrintSumm.Text = "1852_15-OrderSummaryByCustomer_Item_Varient&ReferenceParty";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty")
				{
					btnPrintSumm.Text = "1852_16-OrderSummaryByReferenceParty";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty & Item")
				{
					btnPrintSumm.Text = "1852_17-OrderSummaryByReferenceParty&Item";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty & Varient")
				{
					btnPrintSumm.Text = "1852_18-OrderSummaryByReferenceParty&Varient";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty_Item & Varient")
				{
					btnPrintSumm.Text = "1852_19-OrderSummaryByReferenceParty_Item&Varient";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By ReferenceParty & City")
				{
					btnPrintSumm.Text = "1852_20-OrderSummaryByReferenceParty&City";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By OrderNo & Customer")
				{
					btnPrintSumm.Text = "1852_21-dtOrderSummaryByOrderNoAndCustomer";
				}
				else if (((Control)(object)cmbReportType).Text == "Order Summary By OrderNo Customer & SalesMan")
				{
					btnPrintSumm.Text = "1852_22-OrderSummaryByOrderNoAndSalesMan";
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
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Expected O, but got Unknown
		//IL_0927: Unknown result type (might be due to invalid IL or missing references)
		//IL_0931: Expected O, but got Unknown
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Expected O, but got Unknown
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Expected O, but got Unknown
		//IL_0969: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Expected O, but got Unknown
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Expected O, but got Unknown
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09aa: Expected O, but got Unknown
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Expected O, but got Unknown
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Expected O, but got Unknown
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Expected O, but got Unknown
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Expected O, but got Unknown
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Expected O, but got Unknown
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Expected O, but got Unknown
		//IL_0bf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Expected O, but got Unknown
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Expected O, but got Unknown
		//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Expected O, but got Unknown
		//IL_0c3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Expected O, but got Unknown
		//IL_0c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Expected O, but got Unknown
		//IL_0c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Expected O, but got Unknown
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Expected O, but got Unknown
		//IL_0c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Expected O, but got Unknown
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Expected O, but got Unknown
		//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1a: Expected O, but got Unknown
		//IL_0d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Expected O, but got Unknown
		//IL_0d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7d: Expected O, but got Unknown
		//IL_0d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d88: Expected O, but got Unknown
		//IL_0d89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d93: Expected O, but got Unknown
		//IL_4843: Unknown result type (might be due to invalid IL or missing references)
		//IL_484d: Expected O, but got Unknown
		//IL_4ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cb2: Expected O, but got Unknown
		//IL_4cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce0: Expected O, but got Unknown
		//IL_58b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58bb: Expected O, but got Unknown
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
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
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
		Appearance appearance233 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance278 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRegister = new System.Windows.Forms.ToolStripButton();
		this.printRegister = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbperemeter = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbApproveStatus = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbOrderStatus = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbItemtype = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbCategory = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbParentCategory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.CmbSupplier = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label22 = new System.Windows.Forms.Label();
		this.BtnCompleteStatus = new System.Windows.Forms.Button();
		this.BtnCancelStatus = new System.Windows.Forms.Button();
		this.BtnOpenStatus = new System.Windows.Forms.Button();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdMain = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtBalanceAmountHeader = new System.Windows.Forms.TextBox();
		this.txtBalanceWeightHeadrer = new System.Windows.Forms.TextBox();
		this.txtBalanceQtyHeader = new System.Windows.Forms.TextBox();
		this.txtDispatchAmountHeader = new System.Windows.Forms.TextBox();
		this.txtDispatchWeightHeader = new System.Windows.Forms.TextBox();
		this.txtDispatchQtyHeader = new System.Windows.Forms.TextBox();
		this.txtOrderAmountHeader = new System.Windows.Forms.TextBox();
		this.txtQrderWeightHeader = new System.Windows.Forms.TextBox();
		this.txtOrderQtyHeader = new System.Windows.Forms.TextBox();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbDateTypeSumm = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.CmbOrderStatusSumm = new UltraCombo();
		this.CmbApprovalStatusSumm = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.cmbItemtypeSumm = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.cmbRefPartyDetailSumm = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbCustomerSumm = new UltraCombo();
		this.cmbDistrictSumm = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbcitySumm = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.cmbReportType = new UltraCombo();
		this.cmbItemNameSumm = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.cmbJobLotSumm = new UltraCombo();
		this.txtFromDateSumm = new System.Windows.Forms.DateTimePicker();
		this.label32 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtToDateSumm = new System.Windows.Forms.DateTimePicker();
		this.txtFromDocNoSumm = new System.Windows.Forms.TextBox();
		this.txtToDocNoSumm = new System.Windows.Forms.TextBox();
		this.BtnShowSumm = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbParentcategorySumm = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbCategorySumm = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewSummary = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSumm = new System.Windows.Forms.ToolStripButton();
		this.CmbRegisteredRefParty = new UltraCombo();
		this.cmbsaleman = new UltraCombo();
		this.CmbPartyGroup = new UltraCombo();
		this.cmbsaleman1 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdMain).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatusSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovalStatusSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefPartyDetailSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRegisteredRefParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsaleman).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyGroup).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1200, 50);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripSeparator6, this.btnNew, this.BtnRegister, this.printRegister, this.btnRefresh, this.toolStripSeparator2, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1200, 25);
		this.toolStrip1.TabIndex = 27;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnReset_Click);
		this.BtnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRegister.Name = "BtnRegister";
		this.BtnRegister.Size = new System.Drawing.Size(128, 22);
		this.BtnRegister.Text = "1852A-Register";
		this.BtnRegister.Click += new System.EventHandler(BtnRegister_Click);
		this.printRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.printRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printRegister.Name = "printRegister";
		this.printRegister.Size = new System.Drawing.Size(119, 22);
		this.printRegister.Text = "1852-Register";
		this.printRegister.Click += new System.EventHandler(printRegister_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 27);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(169, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Sale Order Register";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatus);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderStatus);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtype);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCategory);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentCategory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtOrderNoTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.btngrnlod);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.FromDate);
		this.groupBox1.Controls.Add(this.txtOrderNoFrom);
		this.groupBox1.Controls.Add(this.Todate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, -1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(755, 133);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(91, 16);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 22);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(60, 15);
		this.label15.TabIndex = 592;
		this.label15.Text = "Date Type";
		this.cmbApproveStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbApproveStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Location = new System.Drawing.Point(555, 44);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Name = "cmbApproveStatus";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).TabIndex = 10;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.label13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label13.Location = new System.Drawing.Point(493, 50);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(64, 15);
		this.label13.TabIndex = 142;
		this.label13.Text = "        Status";
		this.CmbOrderStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Location = new System.Drawing.Point(555, 73);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Name = "CmbOrderStatus";
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).TabIndex = 11;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(493, 79);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(66, 15);
		this.label14.TabIndex = 141;
		this.label14.Text = "Ord. Status";
		this.cmbItemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance249;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance250).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance250).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance254).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance255).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance255;
		((AppearanceBase)appearance257).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Location = new System.Drawing.Point(320, 73);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Name = "cmbItemtype";
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Size = new System.Drawing.Size(172, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).TabIndex = 7;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(230, 79);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 124;
		this.label5.Text = "Item Type";
		this.cmbCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.cmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance262;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance263).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance263).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance265).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance268).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance269).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance269;
		((AppearanceBase)appearance270).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Location = new System.Drawing.Point(320, 44);
		((System.Windows.Forms.Control)(object)this.cmbCategory).Name = "cmbCategory";
		((System.Windows.Forms.Control)(object)this.cmbCategory).Size = new System.Drawing.Size(172, 26);
		((System.Windows.Forms.Control)(object)this.cmbCategory).TabIndex = 6;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(230, 50);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 123;
		this.label8.Text = "Item Category";
		this.cmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Location = new System.Drawing.Point(320, 16);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Name = "cmbParentCategory";
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Size = new System.Drawing.Size(172, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(230, 22);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(91, 15);
		this.label6.TabIndex = 122;
		this.label6.Text = "Parent Category";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(230, 108);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 116;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance236;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance241).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(320, 102);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(172, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 8;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 50);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 79);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "To Date";
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(157, 104);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(70, 23);
		this.txtOrderNoTo.TabIndex = 4;
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoTo_KeyPress);
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance215).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance216).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(555, 16);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 9;
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(688, 102);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(61, 26);
		this.btngrnlod.TabIndex = 12;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(493, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Customer";
		this.FromDate.CustomFormat = "dd-MM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(91, 46);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(136, 23);
		this.FromDate.TabIndex = 1;
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(91, 104);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(64, 23);
		this.txtOrderNoFrom.TabIndex = 3;
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.Todate.CustomFormat = "dd-MM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(91, 75);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(136, 23);
		this.Todate.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(5, 108);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 28;
		this.label4.Text = "No From && To";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.BtnCompleteStatus);
		this.panel3.Controls.Add(this.BtnCancelStatus);
		this.panel3.Controls.Add(this.BtnOpenStatus);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1186, 26);
		this.panel3.TabIndex = 31;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1137, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.Teal;
		this.label22.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(3, 3);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(372, 17);
		this.label22.TabIndex = 24;
		this.label22.Text = "Click The Button To Change Status of Selected Rows";
		this.BtnCompleteStatus.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnCompleteStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnCompleteStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCompleteStatus.ForeColor = System.Drawing.Color.White;
		this.BtnCompleteStatus.Location = new System.Drawing.Point(530, 1);
		this.BtnCompleteStatus.Name = "BtnCompleteStatus";
		this.BtnCompleteStatus.Size = new System.Drawing.Size(85, 23);
		this.BtnCompleteStatus.TabIndex = 12;
		this.BtnCompleteStatus.Text = "Complete";
		this.BtnCompleteStatus.UseVisualStyleBackColor = false;
		this.BtnCompleteStatus.Click += new System.EventHandler(BtnCompleteStatus_Click);
		this.BtnCancelStatus.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnCancelStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnCancelStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelStatus.ForeColor = System.Drawing.Color.White;
		this.BtnCancelStatus.Location = new System.Drawing.Point(458, 1);
		this.BtnCancelStatus.Name = "BtnCancelStatus";
		this.BtnCancelStatus.Size = new System.Drawing.Size(68, 23);
		this.BtnCancelStatus.TabIndex = 12;
		this.BtnCancelStatus.Text = "Cancel";
		this.BtnCancelStatus.UseVisualStyleBackColor = false;
		this.BtnCancelStatus.Click += new System.EventHandler(BtnCancelStatus_Click);
		this.BtnOpenStatus.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnOpenStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnOpenStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnOpenStatus.ForeColor = System.Drawing.Color.White;
		this.BtnOpenStatus.Location = new System.Drawing.Point(390, 1);
		this.BtnOpenStatus.Name = "BtnOpenStatus";
		this.BtnOpenStatus.Size = new System.Drawing.Size(64, 23);
		this.BtnOpenStatus.TabIndex = 12;
		this.BtnOpenStatus.Text = "Open";
		this.BtnOpenStatus.UseVisualStyleBackColor = false;
		this.BtnOpenStatus.Click += new System.EventHandler(BtnOpenStatus_Click);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1214, 664);
		this.tabControl1.TabIndex = 34;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1206, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail Register";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(3, 187);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1200, 445);
		this.tabControl2.TabIndex = 2;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1192, 416);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail Data";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1186, 384);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 34;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1186, 26);
		this.panel5.TabIndex = 33;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1138, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar2.TabIndex = 10;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage4.Controls.Add(this.panel4);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdMain);
		this.tabPage4.Controls.Add(this.panel3);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1192, 416);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Main Detail Data";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 294);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1186, 119);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 35;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 271);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1186, 23);
		this.panel4.TabIndex = 34;
		this.grdMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdMain).BackColor = System.Drawing.Color.White;
		grdMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMain.DesignTimeLayout = grdMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMain).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdMain.FilterMode = (FilterMode)2;
		this.grdMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdMain).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grdMain).Name = "grdMain";
		((ScrollableBase)this.grdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMain).Size = new System.Drawing.Size(1186, 242);
		((System.Windows.Forms.Control)(object)this.grdMain).TabIndex = 32;
		this.grdMain.TotalRow = (InheritableBoolean)1;
		this.grdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMain).VisualStyle = (VisualStyle)4;
		this.grdMain.ColumnButtonClick += new ColumnActionEventHandler(grdMain_ColumnButtonClick);
		this.grdMain.SelectionChanged += new System.EventHandler(grdMain_SelectionChanged);
		this.grdMain.LinkClicked += new ColumnActionEventHandler(grdMain_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox3);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 53);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1200, 134);
		this.panel2.TabIndex = 1;
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add(this.txtBalanceAmountHeader);
		this.groupBox3.Controls.Add(this.txtBalanceWeightHeadrer);
		this.groupBox3.Controls.Add(this.txtBalanceQtyHeader);
		this.groupBox3.Controls.Add(this.txtDispatchAmountHeader);
		this.groupBox3.Controls.Add(this.txtDispatchWeightHeader);
		this.groupBox3.Controls.Add(this.txtDispatchQtyHeader);
		this.groupBox3.Controls.Add(this.txtOrderAmountHeader);
		this.groupBox3.Controls.Add(this.txtQrderWeightHeader);
		this.groupBox3.Controls.Add(this.txtOrderQtyHeader);
		this.groupBox3.Enabled = false;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(759, -1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(407, 133);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Total Of Selected Row";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(3, 79);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(54, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Dispatch";
		this.label12.BackColor = System.Drawing.Color.Silver;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(290, 22);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(116, 20);
		this.label12.TabIndex = 0;
		this.label12.Text = "Amount";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label11.BackColor = System.Drawing.Color.Silver;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(171, 22);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(116, 20);
		this.label11.TabIndex = 0;
		this.label11.Text = "Weight";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label10.BackColor = System.Drawing.Color.Silver;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(59, 22);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(109, 20);
		this.label10.TabIndex = 0;
		this.label10.Text = "Qty";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(3, 50);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(50, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Ordered";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(3, 108);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(48, 15);
		this.label25.TabIndex = 0;
		this.label25.Text = "Balance";
		this.txtBalanceAmountHeader.Enabled = false;
		this.txtBalanceAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceAmountHeader.Location = new System.Drawing.Point(290, 104);
		this.txtBalanceAmountHeader.Name = "txtBalanceAmountHeader";
		this.txtBalanceAmountHeader.ShortcutsEnabled = false;
		this.txtBalanceAmountHeader.Size = new System.Drawing.Size(116, 23);
		this.txtBalanceAmountHeader.TabIndex = 8;
		this.txtBalanceWeightHeadrer.Enabled = false;
		this.txtBalanceWeightHeadrer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeightHeadrer.Location = new System.Drawing.Point(171, 104);
		this.txtBalanceWeightHeadrer.Name = "txtBalanceWeightHeadrer";
		this.txtBalanceWeightHeadrer.ShortcutsEnabled = false;
		this.txtBalanceWeightHeadrer.Size = new System.Drawing.Size(116, 23);
		this.txtBalanceWeightHeadrer.TabIndex = 5;
		this.txtBalanceQtyHeader.Enabled = false;
		this.txtBalanceQtyHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceQtyHeader.Location = new System.Drawing.Point(59, 104);
		this.txtBalanceQtyHeader.Name = "txtBalanceQtyHeader";
		this.txtBalanceQtyHeader.ShortcutsEnabled = false;
		this.txtBalanceQtyHeader.Size = new System.Drawing.Size(109, 23);
		this.txtBalanceQtyHeader.TabIndex = 2;
		this.txtDispatchAmountHeader.Enabled = false;
		this.txtDispatchAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDispatchAmountHeader.Location = new System.Drawing.Point(290, 75);
		this.txtDispatchAmountHeader.Name = "txtDispatchAmountHeader";
		this.txtDispatchAmountHeader.ShortcutsEnabled = false;
		this.txtDispatchAmountHeader.Size = new System.Drawing.Size(116, 23);
		this.txtDispatchAmountHeader.TabIndex = 7;
		this.txtDispatchWeightHeader.Enabled = false;
		this.txtDispatchWeightHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDispatchWeightHeader.Location = new System.Drawing.Point(171, 75);
		this.txtDispatchWeightHeader.Name = "txtDispatchWeightHeader";
		this.txtDispatchWeightHeader.ShortcutsEnabled = false;
		this.txtDispatchWeightHeader.Size = new System.Drawing.Size(116, 23);
		this.txtDispatchWeightHeader.TabIndex = 4;
		this.txtDispatchQtyHeader.Enabled = false;
		this.txtDispatchQtyHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDispatchQtyHeader.Location = new System.Drawing.Point(59, 75);
		this.txtDispatchQtyHeader.Name = "txtDispatchQtyHeader";
		this.txtDispatchQtyHeader.ShortcutsEnabled = false;
		this.txtDispatchQtyHeader.Size = new System.Drawing.Size(109, 23);
		this.txtDispatchQtyHeader.TabIndex = 1;
		this.txtOrderAmountHeader.Enabled = false;
		this.txtOrderAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderAmountHeader.Location = new System.Drawing.Point(290, 46);
		this.txtOrderAmountHeader.Name = "txtOrderAmountHeader";
		this.txtOrderAmountHeader.ShortcutsEnabled = false;
		this.txtOrderAmountHeader.Size = new System.Drawing.Size(116, 23);
		this.txtOrderAmountHeader.TabIndex = 6;
		this.txtQrderWeightHeader.Enabled = false;
		this.txtQrderWeightHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQrderWeightHeader.Location = new System.Drawing.Point(171, 46);
		this.txtQrderWeightHeader.Name = "txtQrderWeightHeader";
		this.txtQrderWeightHeader.ShortcutsEnabled = false;
		this.txtQrderWeightHeader.Size = new System.Drawing.Size(116, 23);
		this.txtQrderWeightHeader.TabIndex = 3;
		this.txtOrderQtyHeader.Enabled = false;
		this.txtOrderQtyHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderQtyHeader.Location = new System.Drawing.Point(59, 46);
		this.txtOrderQtyHeader.Name = "txtOrderQtyHeader";
		this.txtOrderQtyHeader.ShortcutsEnabled = false;
		this.txtOrderQtyHeader.Size = new System.Drawing.Size(109, 23);
		this.txtOrderQtyHeader.TabIndex = 0;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1206, 635);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Summary Register";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.AlternatingColors = true;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		this.DataGridHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 223);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1200, 409);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 198);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1200, 25);
		this.panel9.TabIndex = 7;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1153, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar5.TabIndex = 25;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 33);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1200, 165);
		this.panel7.TabIndex = 5;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsaleman);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyGroup);
		this.groupBox2.Controls.Add(this.cmbsaleman1);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderStatusSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDistrictSumm);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcitySumm);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemNameSumm);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLotSumm);
		this.groupBox2.Controls.Add(this.txtFromDateSumm);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.txtToDateSumm);
		this.groupBox2.Controls.Add(this.txtFromDocNoSumm);
		this.groupBox2.Controls.Add(this.txtToDocNoSumm);
		this.groupBox2.Controls.Add(this.BtnShowSumm);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategorySumm);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 4);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1191, 159);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Criteria's";
		this.CmbDateTypeSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbDateTypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateTypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateTypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateTypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Location = new System.Drawing.Point(93, 16);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Name = "CmbDateTypeSumm";
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).TabIndex = 0;
		this.CmbDateTypeSumm.ValueChanged += new System.EventHandler(CmbDateTypeSumm_ValueChanged);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(2, 22);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(60, 15);
		this.label40.TabIndex = 594;
		this.label40.Text = "Date Type";
		this.CmbOrderStatusSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderStatusSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderStatusSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderStatusSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance222;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance228).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance229).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance230).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderStatusSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatusSumm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderStatusSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatusSumm).Location = new System.Drawing.Point(862, 102);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatusSumm).Name = "CmbOrderStatusSumm";
		((System.Windows.Forms.Control)(object)this.CmbOrderStatusSumm).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatusSumm).TabIndex = 19;
		this.CmbApprovalStatusSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbApprovalStatusSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbApprovalStatusSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApprovalStatusSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance212;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance267).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance278).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbApprovalStatusSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).Location = new System.Drawing.Point(862, 73);
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).Name = "CmbApprovalStatusSumm";
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).TabIndex = 18;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(793, 108);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(66, 15);
		this.label28.TabIndex = 162;
		this.label28.Text = "Ord. Status";
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
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Location = new System.Drawing.Point(288, 44);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Name = "cmbItemtypeSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).TabIndex = 7;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.label26.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label26.Location = new System.Drawing.Point(793, 79);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(64, 15);
		this.label26.TabIndex = 163;
		this.label26.Text = "        Status";
		this.cmbRefPartyDetailSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbRefPartyDetailSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRefPartyDetailSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRefPartyDetailSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRefPartyDetailSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Location = new System.Drawing.Point(537, 102);
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Name = "cmbRefPartyDetailSumm";
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).TabIndex = 14;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(459, 79);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(58, 15);
		this.label16.TabIndex = 123;
		this.label16.Text = "Customer";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(459, 108);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(54, 15);
		this.label19.TabIndex = 158;
		this.label19.Text = "Ref Party";
		this.cmbCustomerSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomerSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Location = new System.Drawing.Point(537, 73);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Name = "cmbCustomerSumm";
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).TabIndex = 13;
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
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Location = new System.Drawing.Point(288, 130);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Name = "cmbDistrictSumm";
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).TabIndex = 10;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(459, 22);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(30, 15);
		this.label30.TabIndex = 149;
		this.label30.Text = "City ";
		this.cmbcitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbcitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Location = new System.Drawing.Point(537, 16);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Name = "cmbcitySumm";
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).TabIndex = 11;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(233, 79);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(35, 15);
		this.label17.TabIndex = 125;
		this.label17.Text = "Item ";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(233, 136);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(45, 15);
		this.label31.TabIndex = 151;
		this.label31.Text = "District";
		this.cmbReportType.AllowNull = (DefaultableBoolean)1;
		this.cmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Location = new System.Drawing.Point(862, 44);
		((System.Windows.Forms.Control)(object)this.cmbReportType).Name = "cmbReportType";
		((System.Windows.Forms.Control)(object)this.cmbReportType).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportType).TabIndex = 17;
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
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Location = new System.Drawing.Point(288, 73);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Name = "cmbItemNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).TabIndex = 8;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(793, 50);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(70, 15);
		this.label18.TabIndex = 155;
		this.label18.Text = "Report Type";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(2, 50);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(63, 15);
		this.label29.TabIndex = 127;
		this.label29.Text = "From Date";
		this.cmbJobLotSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance288;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLotSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Location = new System.Drawing.Point(288, 102);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Name = "cmbJobLotSumm";
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).TabIndex = 9;
		this.txtFromDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtFromDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateSumm.Location = new System.Drawing.Point(93, 46);
		this.txtFromDateSumm.Name = "txtFromDateSumm";
		this.txtFromDateSumm.Size = new System.Drawing.Size(135, 23);
		this.txtFromDateSumm.TabIndex = 1;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(233, 108);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(46, 15);
		this.label32.TabIndex = 153;
		this.label32.Text = "Job Lot";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(2, 108);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(70, 15);
		this.label21.TabIndex = 129;
		this.label21.Text = "From,To No";
		this.txtToDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtToDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateSumm.Location = new System.Drawing.Point(93, 75);
		this.txtToDateSumm.Name = "txtToDateSumm";
		this.txtToDateSumm.Size = new System.Drawing.Size(135, 23);
		this.txtToDateSumm.TabIndex = 2;
		this.txtFromDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoSumm.Location = new System.Drawing.Point(93, 104);
		this.txtFromDocNoSumm.Name = "txtFromDocNoSumm";
		this.txtFromDocNoSumm.Size = new System.Drawing.Size(67, 23);
		this.txtFromDocNoSumm.TabIndex = 3;
		this.txtToDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoSumm.Location = new System.Drawing.Point(161, 104);
		this.txtToDocNoSumm.Name = "txtToDocNoSumm";
		this.txtToDocNoSumm.Size = new System.Drawing.Size(67, 23);
		this.txtToDocNoSumm.TabIndex = 4;
		this.BtnShowSumm.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowSumm.FlatAppearance.BorderSize = 0;
		this.BtnShowSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowSumm.ForeColor = System.Drawing.Color.White;
		this.BtnShowSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowSumm.Location = new System.Drawing.Point(1053, 130);
		this.BtnShowSumm.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowSumm.Name = "BtnShowSumm";
		this.BtnShowSumm.Size = new System.Drawing.Size(57, 26);
		this.BtnShowSumm.TabIndex = 20;
		this.BtnShowSumm.Text = "Show";
		this.BtnShowSumm.UseVisualStyleBackColor = false;
		this.BtnShowSumm.Click += new System.EventHandler(BtnShowSumm_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(2, 136);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(91, 15);
		this.label20.TabIndex = 138;
		this.label20.Text = "Parent Category";
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
		this.cmbParentcategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Location = new System.Drawing.Point(93, 130);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Name = "cmbParentcategorySumm";
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).TabIndex = 5;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(233, 22);
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
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Location = new System.Drawing.Point(288, 16);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Name = "CmbCategorySumm";
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).TabIndex = 6;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(233, 50);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(32, 15);
		this.label33.TabIndex = 138;
		this.label33.Text = "Type";
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1200, 30);
		this.panel8.TabIndex = 6;
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNewSummary, this.btnRefreshSummary, this.btnPrintSumm });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1200, 30);
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
		this.btnPrintSumm.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSumm.Name = "btnPrintSumm";
		this.btnPrintSumm.Size = new System.Drawing.Size(61, 27);
		this.btnPrintSumm.Text = "Print";
		this.btnPrintSumm.Click += new System.EventHandler(btnPrintSumm_Click);
		this.CmbRegisteredRefParty.AllowNull = (DefaultableBoolean)1;
		this.CmbRegisteredRefParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRegisteredRefParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRegisteredRefParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).Location = new System.Drawing.Point(537, 130);
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).Name = "CmbRegisteredRefParty";
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).TabIndex = 15;
		this.cmbsaleman.AllowNull = (DefaultableBoolean)1;
		this.cmbsaleman.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsaleman.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsaleman.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Appearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsaleman).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance275;
		((SpecialBoxBase)((UltraGridBase)this.cmbsaleman).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance276).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance276).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance281).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance282).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance283).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsaleman).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsaleman.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsaleman).Location = new System.Drawing.Point(862, 16);
		((System.Windows.Forms.Control)(object)this.cmbsaleman).Name = "cmbsaleman";
		((System.Windows.Forms.Control)(object)this.cmbsaleman).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.cmbsaleman).TabIndex = 16;
		this.CmbPartyGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).Location = new System.Drawing.Point(537, 44);
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).Name = "CmbPartyGroup";
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).TabIndex = 12;
		this.cmbsaleman1.AutoSize = true;
		this.cmbsaleman1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsaleman1.Location = new System.Drawing.Point(793, 22);
		this.cmbsaleman1.Name = "cmbsaleman1";
		this.cmbsaleman1.Size = new System.Drawing.Size(56, 15);
		this.cmbsaleman1.TabIndex = 602;
		this.cmbsaleman1.Text = "Sale Man";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(459, 136);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(74, 15);
		this.label34.TabIndex = 601;
		this.label34.Text = "Sales Offices";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(459, 50);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(70, 15);
		this.label35.TabIndex = 600;
		this.label35.Text = "Party Group";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(2, 79);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(48, 15);
		this.label36.TabIndex = 127;
		this.label36.Text = "To Date";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1214, 664);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "SaleOrderRegister";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadPurchaseOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdMain).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatusSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovalStatusSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefPartyDetailSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRegisteredRefParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsaleman).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyGroup).EndInit();
		base.ResumeLayout(false);
	}
}

}
