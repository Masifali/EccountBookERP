using System;
using System.Collections.Generic;
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
using Architecture.WebHelper.Helper;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Reports;

public class frmSaleOrderHistory : Form
{
	private WaitForm waitForm = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable grdlst = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtSalesRegister = new DataTable();

	private DataTable dtSalesSummaryByItem = new DataTable();

	private DataTable dtSalesSummaryByItemandCity = new DataTable();

	private DataTable dtSalesSummaryByItemandPackSize = new DataTable();

	private DataTable dtSalesSummaryByItemNPackSizeCity = new DataTable();

	private DataTable dtSalesSummaryByCustomer = new DataTable();

	private DataTable dtSalesSummaryByCustomerandItem = new DataTable();

	private DataTable dtSalesSummaryByCustomerandCity = new DataTable();

	private DataTable dtSalesSummaryByCustomerandPackSize = new DataTable();

	private DataTable dtSalesSummaryByCustomerItemandCity = new DataTable();

	private DataTable dtSalesSummaryByCustomerandReferenceParty = new DataTable();

	private DataTable dtSalesSummaryByCustomerItemandReferenceParty = new DataTable();

	private DataTable dtSalesSummaryByReferenceParty = new DataTable();

	private DataTable dtSalesSummaryByReferencePartyAndCity = new DataTable();

	private DataTable dtSalesSummaryByReferencePartyAndItem = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtGrid = new DataTable();

	private double ToleranceForOrderCompletion;

	public DateTime FromDate;

	public DateTime To_Date;

	public int SupplierId;

	public bool updateFromDate = true;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private Label label23;

	internal UltraCombo CmbSupplier;

	private Label label1;

	internal UltraCombo CmbItem;

	private Label label2;

	private Label label33;

	private Label label4;

	private TextBox txtSrFrom;

	private TextBox txtsrTo;

	internal GridEX grdsaleorder;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private Button btnshow;

	internal UltraCombo CmbParentCategory;

	private Label label7;

	internal UltraCombo CmbItemType;

	private Label label9;

	internal UltraCombo CmbItemCategory;

	private Label label8;

	private Label label10;

	private Label label11;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel5;

	private Label label6;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label12;

	internal GridEX GrdDetail;

	private CtrlGrdBar ctrlGrdBar4;

	private Label label13;

	internal GridEX GrdMain;

	private CheckBox chkincludeDo;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private Panel panel6;

	private TabPage tabSummary;

	private Panel panel9;

	internal GridEX DataGridHistory;

	private Label label26;

	private Panel panel7;

	internal UltraCombo cmbPackingtypeSumm;

	private Label label29;

	internal UltraCombo cmbPackUomSumm;

	private Label label28;

	internal UltraCombo cmbCropSumm;

	private Label label27;

	internal UltraCombo cmbItemtypeSumm;

	private Label label16;

	internal UltraCombo CmbCategorySumm;

	private Label label17;

	internal UltraCombo cmbParentcategorySumm;

	private Label label18;

	private Button BtnShowSumm;

	private TextBox txtToDocNoSumm;

	private TextBox txtFromDocNoSumm;

	private DateTimePicker txtToDateSumm;

	private Label label21;

	private DateTimePicker txtFromDateSumm;

	private Label label22;

	internal UltraCombo cmbItemNameSumm;

	private Label label24;

	internal UltraCombo cmbCustomerSumm;

	private Label label25;

	private Panel panel8;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefreshSummary;

	private ToolStripButton btnPrintSumm;

	internal UltraCombo cmbDistrictSumm;

	private Label label31;

	internal UltraCombo cmbcitySumm;

	private Label label30;

	internal UltraCombo cmbActivitySumm;

	private Label label14;

	internal UltraCombo cmbJobLotSumm;

	private Label label32;

	private CtrlGrdBar ctrlGrdBar5;

	private CheckBox ChkDoApprovedIncludSummary;

	public DateTimePicker fromdate;

	public DateTimePicker ToDate;

	public UltraCombo CmbIsApproved;

	public UltraCombo CmbOrderStatus;

	private CtrlGrdBar ctrlGrdBar6;

	private CtrlGrdBar ctrlGrdBar7;

	private ToolStripButton btnPrint271A;

	public UltraCombo cmbRefPartyDetail;

	private Label label15;

	internal UltraCombo cmbRefPartyDetailSumm;

	private Label label19;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private CheckBox chkSkipZero;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnNewSummary;

	private ToolStripButton btnShortCutKeys;

	private ToolStripButton btnShortCutKeySumamryTab;

	private ToolStripButton btnCompleteOrders;

	private ToolStripButton btnCancelOrders;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo cmbBranchName;

	private Label label48;

	internal UltraCombo cmbBranchNameSumm;

	private Label label5;

	internal UltraCombo ultraCombo1;

	private Label label3;

	private ToolStripButton btnAutoCompleteOrder;

	internal UltraCombo CmbCostCenter;

	private Label lblCostCenter;

	public UltraCombo CmbStatusSummary;

	public UltraCombo CmbApprovedStatusSummary;

	private Label label34;

	private Label label20;

	private ToolStripButton btnOpenOrders;

	public UltraCombo CmbBookingPerson;

	private Label label35;

	internal UltraCombo cmbBookingPersonSumm;

	private Label label36;

	public frmSaleOrderHistory(UserAccount user)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 800);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			waitForm.Show(this);
			ToleranceForOrderCompletion = Conversion.ToDouble((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ToleranceForOrderCompletion"));
			dtSalesRegister.Columns.Add("Id", typeof(int));
			dtSalesRegister.Columns.Add("DocumentTypeId", typeof(int));
			dtSalesRegister.Columns.Add("BranchName", typeof(string));
			dtSalesRegister.Columns.Add("DocType", typeof(string));
			dtSalesRegister.Columns.Add("DocDate", typeof(DateTime));
			dtSalesRegister.Columns.Add("DocNo", typeof(int));
			dtSalesRegister.Columns.Add("PartyName", typeof(string));
			dtSalesRegister.Columns.Add("BookingPerson", typeof(string));
			dtSalesRegister.Columns.Add("ReferenceParty", typeof(string));
			dtSalesRegister.Columns.Add("ItemCode", typeof(string));
			dtSalesRegister.Columns.Add("ItemName", typeof(string));
			dtSalesRegister.Columns.Add("PackUom", typeof(string));
			dtSalesRegister.Columns.Add("CropYear", typeof(string));
			dtSalesRegister.Columns.Add("JobLot", typeof(string));
			dtSalesRegister.Columns.Add("PackingType", typeof(string));
			dtSalesRegister.Columns.Add("OrderQty", typeof(double));
			dtSalesRegister.Columns.Add("DispatchedQty", typeof(double));
			dtSalesRegister.Columns.Add("BalQty", typeof(double));
			dtSalesRegister.Columns.Add("OrderWeight", typeof(double));
			dtSalesRegister.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesRegister.Columns.Add("BalWeight", typeof(double));
			dtSalesRegister.Columns.Add("OrderAmount", typeof(double));
			dtSalesRegister.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesRegister.Columns.Add("BalAmount", typeof(double));
			dtSalesRegister.Columns.Add("AvgRate", typeof(double));
			dtSalesRegister.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesRegister.Columns.Add("CityName", typeof(string));
			dtSalesSummaryByItem.Columns.Add("Id", typeof(int));
			dtSalesSummaryByItem.Columns.Add("ItemCode");
			dtSalesSummaryByItem.Columns.Add("ItemName");
			dtSalesSummaryByItem.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByItem.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByItem.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByItem.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByItem.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByItem.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByItem.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItem.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("Id", typeof(int));
			dtSalesSummaryByItemandCity.Columns.Add("ItemCode");
			dtSalesSummaryByItemandCity.Columns.Add("ItemName");
			dtSalesSummaryByItemandCity.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByItemandCity.Columns.Add("CityName");
			dtSalesSummaryByItemandPackSize.Columns.Add("Id", typeof(int));
			dtSalesSummaryByItemandPackSize.Columns.Add("ItemCode");
			dtSalesSummaryByItemandPackSize.Columns.Add("ItemName");
			dtSalesSummaryByItemandPackSize.Columns.Add("PackUom");
			dtSalesSummaryByItemandPackSize.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemandPackSize.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("Id", typeof(int));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("ItemCode");
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("ItemName");
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("PackUom");
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByItemNPackSizeCity.Columns.Add("CityName");
			dtSalesSummaryByCustomer.Columns.Add("Id", typeof(int));
			dtSalesSummaryByCustomer.Columns.Add("PartyName");
			dtSalesSummaryByCustomer.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomer.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("Id", typeof(int));
			dtSalesSummaryByCustomerandItem.Columns.Add("PartyName");
			dtSalesSummaryByCustomerandItem.Columns.Add("ItemCode");
			dtSalesSummaryByCustomerandItem.Columns.Add("ItemName");
			dtSalesSummaryByCustomerandItem.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandItem.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("Id", typeof(int));
			dtSalesSummaryByCustomerandCity.Columns.Add("PartyName");
			dtSalesSummaryByCustomerandCity.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerandCity.Columns.Add("CityName");
			dtSalesSummaryByCustomerItemandCity.Columns.Add("Id", typeof(int));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PartyName");
			dtSalesSummaryByCustomerItemandCity.Columns.Add("ItemCode");
			dtSalesSummaryByCustomerItemandCity.Columns.Add("ItemName");
			dtSalesSummaryByCustomerItemandCity.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerItemandCity.Columns.Add("CityName");
			dtSalesSummaryByCustomerandPackSize.Columns.Add("Id", typeof(int));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PartyName");
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PackUom");
			dtSalesSummaryByCustomerandPackSize.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandPackSize.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("Id", typeof(int));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("PartyName");
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("ReferenceParty");
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("PackUom");
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerandReferenceParty.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("Id", typeof(int));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("PartyName");
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("ReferenceParty");
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("Itemcode");
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("ItemName");
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("PackUom");
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByCustomerItemandReferenceParty.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("Id", typeof(int));
			dtSalesSummaryByReferenceParty.Columns.Add("ReferenceParty");
			dtSalesSummaryByReferenceParty.Columns.Add("PackUom");
			dtSalesSummaryByReferenceParty.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByReferenceParty.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("Id", typeof(int));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("CityName");
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("ReferenceParty");
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("PackUom");
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByReferencePartyAndCity.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("Id", typeof(int));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("ReferenceParty");
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("ItemCode");
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("ItemName");
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("PackUom");
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("OrderQty", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("DispatchedQty", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("BalQty", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("OrderWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("DispatchedWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("BalWeight", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("OrderAmount", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("BalAmount", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("AvgRate", typeof(double));
			dtSalesSummaryByReferencePartyAndItem.Columns.Add("PercentOfTotal", typeof(double));
			CostCenterFill();
			if (UserAccount.AppId == 5 && ((DataTable)((UltraGridBase)CmbCostCenter).DataSource).Rows.Count > 0)
			{
				((UltraGridBase)CmbCostCenter).Rows[0].Activate();
				((Control)(object)CmbCostCenter).Enabled = false;
			}
			AllDropDownBind(Conversion.ToInt(CmbCostCenter.Value));
			BranchesFill();
			ItemUOMFill();
			StatusFill();
			OrderStatusFill();
			ActivityFill();
			if (Conversion.CheckDateTimeNull(FromDate))
			{
				fromdate.Value = DateTime.Now.AddDays(-7.0);
				ToDate.Value = DateTime.Now;
				txtFromDateSumm.Value = DateTime.Now.AddDays(-7.0);
				txtToDateSumm.Value = DateTime.Now;
				if (((Control)(object)CmbOrderStatus).Text == string.Empty)
				{
					((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
				}
				if (((Control)(object)CmbStatusSummary).Text == string.Empty)
				{
					((UltraGridBase)CmbStatusSummary).Rows[0].Activate();
				}
				if (((Control)(object)CmbIsApproved).Text == string.Empty)
				{
					((UltraGridBase)CmbIsApproved).Rows[1].Activate();
				}
				if (((Control)(object)CmbApprovedStatusSummary).Text == string.Empty)
				{
					((UltraGridBase)CmbApprovedStatusSummary).Rows[1].Activate();
				}
				tabControl1.SelectedIndex = 1;
			}
			else
			{
				if (updateFromDate)
				{
					fromdate.Value = FromDate;
				}
				ToDate.Value = To_Date;
				if (SupplierId > 0)
				{
					CmbSupplier.Value = SupplierId;
				}
				txtFromDateSumm.Value = FromDate;
				txtToDateSumm.Value = To_Date;
				((UltraGridBase)CmbIsApproved).Rows[2].Activate();
				((UltraGridBase)CmbApprovedStatusSummary).Rows[2].Activate();
				gridHisory();
			}
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

	private void CostCenterFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCostCenter.Value);
			DataTable dt = Projects.GetSubCostCenters(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, UserAccount.AppId, 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCostCenter, "Id", "CostCenterName", "Cost Center", ZeroIndex: false);
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCostCenter.Value = Id;
					}
					else
					{
						((Control)(object)CmbCostCenter).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbCostCenter).DataSource = null;
				((Control)(object)CmbCostCenter).Text = "";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllDropDownBind(int CostCenterId = 0)
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
			obj.CostCenterId = CostCenterId;
			obj.DocumentTypeIds = "81";
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
			DataTable dtAll = SaleOrder.GetDataForDropDownFromSaleOrder(obj);
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Customer"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Item"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowswh = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "District"
					select r;
				DataTable dtDis = (rowswh.Any() ? rowswh.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsJob = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "JobLot"
					select r;
				DataTable dtJob = (rowsJob.Any() ? rowsJob.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsBookingPerson = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "BookingPerson"
					select r;
				DataTable dtBookingPerson = (rowsBookingPerson.Any() ? rowsBookingPerson.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsCity = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "City"
					select r;
				DataTable dtCity = (rowsCity.Any() ? rowsCity.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsCrp = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Crop"
					select r;
				DataTable dtCrop = (rowsCrp.Any() ? rowsCrp.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowspt = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "PackingType"
					select r;
				DataTable dtPackingType = (rowspt.Any() ? rowspt.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsPc = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ParentCategory"
					select r;
				DataTable dtParentCat = (rowsPc.Any() ? rowsPc.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsIc = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ItemCategory"
					select r;
				DataTable dtCategory = (rowsIc.Any() ? rowsIc.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsItmType = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ItemType"
					select r;
				DataTable dtItemType = (rowsItmType.Any() ? rowsItmType.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsRefParty = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "RefPartyDetail"
					select r;
				DataTable dtRefPartyDetail = (rowsRefParty.Any() ? rowsRefParty.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDL(dtParty, CmbSupplier, "Id", "ReferenceName", "Customer Name", ZeroIndex: true);
				((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtParty, cmbCustomerSumm, "Id", "ReferenceName", "Customer Name", ZeroIndex: true);
				((UltraGridBase)cmbCustomerSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbCustomerSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItem, CmbItem, "Id", "ReferenceName", "Item Name", ZeroIndex: true);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItem, cmbItemNameSumm, "Id", "ReferenceName", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItemNameSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbItemNameSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtDis, cmbDistrictSumm, "Id", "ReferenceName", "District", ZeroIndex: true);
				((UltraGridBase)cmbDistrictSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbDistrictSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtJob, cmbJobLotSumm, "Id", "ReferenceName", "Job Lot", ZeroIndex: true);
				((UltraGridBase)cmbJobLotSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbJobLotSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtBookingPerson, CmbBookingPerson, "Id", "ReferenceName", "Booking Person", ZeroIndex: true);
				((UltraGridBase)CmbBookingPerson).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbBookingPerson).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtBookingPerson, cmbBookingPersonSumm, "Id", "ReferenceName", "Booking Person", ZeroIndex: true);
				((UltraGridBase)cmbBookingPersonSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbBookingPersonSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCity, cmbcitySumm, "Id", "ReferenceName", "City Name", ZeroIndex: true);
				((UltraGridBase)cmbcitySumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbcitySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCity, ultraCombo1, "Id", "ReferenceName", "City Name", ZeroIndex: true);
				((UltraGridBase)ultraCombo1).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)ultraCombo1).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCrop, cmbCropSumm, "Id", "ReferenceName", "Crop Year", ZeroIndex: true);
				((UltraGridBase)cmbCropSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbCropSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtPackingType, cmbPackingtypeSumm, "Id", "ReferenceName", "Packing Type", ZeroIndex: true);
				((UltraGridBase)cmbPackingtypeSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbPackingtypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtParentCat, cmbParentcategorySumm, "Id", "ReferenceName", "Parent Category", ZeroIndex: true);
				((UltraGridBase)cmbParentcategorySumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbParentcategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtParentCat, CmbParentCategory, "Id", "ReferenceName", "Parent Category", ZeroIndex: true);
				((UltraGridBase)CmbParentCategory).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCategory, CmbItemCategory, "Id", "ReferenceName", "Item Category", ZeroIndex: true);
				((UltraGridBase)CmbItemCategory).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCategory, CmbCategorySumm, "Id", "ReferenceName", "Item Category", ZeroIndex: true);
				((UltraGridBase)CmbCategorySumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItemType, CmbItemType, "Id", "ReferenceName", "Item Type", ZeroIndex: true);
				((UltraGridBase)CmbItemType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItemType, cmbItemtypeSumm, "Id", "ReferenceName", "Item Type", ZeroIndex: true);
				((UltraGridBase)cmbItemtypeSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbItemtypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtRefPartyDetail, cmbRefPartyDetail, "Id", "ReferenceName", "Ref Party", ZeroIndex: true);
				((UltraGridBase)cmbRefPartyDetail).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbRefPartyDetail).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtRefPartyDetail, cmbRefPartyDetailSumm, "Id", "ReferenceName", "Ref Party", ZeroIndex: true);
				((UltraGridBase)cmbRefPartyDetailSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbRefPartyDetailSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			dtBranch = SaleOrder.GetBranchesAllocatedToUserFromSaleOrder(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 81);
			if (dtBranch.Rows.Count > 0)
			{
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
				DDL.BindDDL(dtBranch, cmbBranchNameSumm, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj2 = ((UltraGridBase)cmbBranchNameSumm).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbBranchNameSumm.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchNameSumm.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchNameSumm.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchNameSumm.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchNameSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchNameSumm).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			DDL.BindDDL(dt, cmbPackUomSumm, "Id", "UOM", "Pack UOM", ZeroIndex: false);
			((UltraGridBase)cmbPackUomSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void resetDetailRegister()
	{
		try
		{
			fromdate.Value = DateTime.Now.AddDays(-7.0);
			ToDate.Value = DateTime.Now;
			chkincludeDo.Checked = false;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbParentCategory).Text = string.Empty;
			((Control)(object)CmbItemCategory).Text = string.Empty;
			((Control)(object)CmbBookingPerson).Text = string.Empty;
			((Control)(object)cmbBookingPersonSumm).Text = string.Empty;
			((Control)(object)CmbItemType).Text = string.Empty;
			txtSrFrom.Clear();
			txtsrTo.Clear();
			((UltraGridBase)CmbIsApproved).Rows[0].Activate();
			((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
			fromdate.Focus();
			grdsaleorder.ClearStructure();
			GrdMain.ClearStructure();
			GrdDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		resetDetailRegister();
	}

	private void StatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "UnApprove");
			dt.Rows.Add(2, "Approve");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbIsApproved, "Id", "Status", "Status", ZeroIndex: false);
			DDL.BindDDL(dt, CmbApprovedStatusSummary, "Id", "Status", "Approved Status", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OrderStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Cancel");
			dt.Rows.Add(3, "Complete");
			DDL.BindDDL(dt, CmbOrderStatus, "Id", "Status", "Status", ZeroIndex: false);
			DDL.BindDDL(dt, CmbStatusSummary, "Id", "Status", "Status", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdsaleorder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdsaleorder.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdsaleorder.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			double DispatchQty = Conversion.ToDouble(r.Cells["DispatchQty"].Value);
			DateTime date = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
			if (grdsaleorder.CurrentColumn.Key == "IsApproved")
			{
				ApproveRecord(Id, grdsaleorder);
			}
			else if (grdsaleorder.CurrentColumn.Key == "Complete")
			{
				CompleteStatus(Id, grdsaleorder);
			}
			else if (grdsaleorder.CurrentColumn.Key == "Open")
			{
				OpenStatus(Id, grdsaleorder);
			}
			else if (grdsaleorder.CurrentColumn.Key == "Cancel")
			{
				CancelStatus(Id, DispatchQty, grdsaleorder);
			}
			else if (grdsaleorder.CurrentColumn.Key == "Update")
			{
				UpdateExpiryDate(Id, date, grdsaleorder);
			}
			else if (grdsaleorder.CurrentColumn.Key == "CustomerName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(r.Cells["OrderSupCustId"].Value)), fromdate.Value, ToDate.Value);
			}
			else if (grdsaleorder.CurrentColumn.Key == "ItemName")
			{
				Conversion.ToInt(r.Cells["SaleGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["OrderItemId"].Value), fromdate.Value, ToDate.Value);
			}
			else if (grdsaleorder.CurrentColumn.Key == "DocNo")
			{
				new SaleActivityDetails(UserAccount, Id).Show();
			}
			else if (grdsaleorder.CurrentColumn.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grdsaleorder.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdsaleorder).Name);
			if (fs != null)
			{
				grdsaleorder.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdMain).Name);
			if (fs != null)
			{
				GrdMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdDetail).Name);
			if (fs != null)
			{
				GrdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			val.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			val.RptPerameter("@FromId", grdsaleorder.CurrentRow.Cells[0].Value.ToString());
			val.RptPerameter("@ToId", (string)null);
			val.RptPerameter("@BranchesId", (string)null);
			val.RptPerameter("@ProjectsId", (string)null);
			val.RptPerameter("@FromDate", (string)null);
			val.RptPerameter("@Todate", (string)null);
			val.RptPerameter("@FromDocNo", (string)null);
			val.RptPerameter("@ToDocNo", (string)null);
			val.RptPerameter("@SupplierCustomerId", (string)null);
			val.RptPerameter("@ItemId", (string)null);
			val.RptPerameter("@IsApproved", (string)null);
			val.RptPerameter("@Status", (string)null);
			val.RptPerameter("@SaleOrderId", grdsaleorder.CurrentRow.Cells[0].Value.ToString());
			val.ShowReport("272-InvRptSaleOrderSlipWithDespatchDetail.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst.Rows.Count == 0 || grdlst == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(grdlst, "270-InvRptSaleOderRegister.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst.Rows.Count == 0 || grdlst == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(grdlst, "271-InvRptSalesOrderRegister.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void frmSaleOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl2.SelectedIndex == 0)
			{
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
				{
					print_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
				{
					toolStripButton2_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3))
				{
					btnPrint271A_Click(null, null);
				}
				if (e.Control && e.Alt)
				{
					MakeShortCutKeysForDetailTab();
				}
			}
			else if (tabControl2.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrintSummary_Click(null, null);
				}
				if (e.Control && e.Alt)
				{
					MakeShortCutKeysForSummaryTab();
				}
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					btnNewSummary_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					btnshow_Click_1(null, null);
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					BtnShowSumm_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					btnRefresh_Click(null, null);
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					btnRefreshSummary_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					tabControl2.SelectedIndex = 1;
					txtFromDateSumm.Focus();
				}
				else
				{
					tabControl2.SelectedIndex = 0;
					fromdate.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						((Control)(object)grdsaleorder).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
					{
						((Control)(object)GrdDetail).Focus();
					}
					else
					{
						((Control)(object)GrdMain).Focus();
					}
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					fromdate.Focus();
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					txtFromDateSumm.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveRecord(int Id, GridEX grid)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		try
		{
			string Remarks = "";
			GridEXRow row = grid.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Remarks = Conversion.ToString((object)row.Cells["ActionRemarks"].Text);
				if (Remarks == string.Empty)
				{
					MessageBox.Show("Action Remarks Required");
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Approve";
				item.Comments = Remarks;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CompleteStatus(int Id, GridEX grid)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		try
		{
			string Remarks = "";
			GridEXRow row = grid.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Remarks = Conversion.ToString((object)row.Cells["ActionRemarks"].Text);
				if (Remarks == string.Empty)
				{
					MessageBox.Show("Action Remarks Required");
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Complete Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Status";
				item.EntryUser = UserAccount.ID;
				item.Comments = Remarks;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Complete Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void OpenStatus(int Id, GridEX grid)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		try
		{
			string Remarks = "";
			GridEXRow row = grid.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Remarks = Conversion.ToString((object)row.Cells["ActionRemarks"].Text);
				if (Remarks == string.Empty)
				{
					MessageBox.Show("Action Remarks Required");
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Open Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Open";
				item.EntryUser = UserAccount.ID;
				item.Comments = Remarks;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Open Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CancelStatus(int Id, double dispatchQty, GridEX grid)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		try
		{
			if (dispatchQty > 0.0)
			{
				MessageBox.Show("Record Not Cancel because Dispatch Qty gratter than zero");
				resetDetailRegister();
				return;
			}
			string Remarks = "";
			GridEXRow row = grid.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Remarks = Conversion.ToString((object)row.Cells["ActionRemarks"].Text);
				if (Remarks == string.Empty)
				{
					MessageBox.Show("Action Remarks Required");
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Cancel Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Cancel";
				item.Comments = Remarks;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Cancel Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UpdateExpiryDate(int Id, DateTime postData, GridEX grid)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		try
		{
			string Remarks = "";
			GridEXRow row = grid.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Remarks = Conversion.ToString((object)row.Cells["ActionRemarks"].Text);
				if (Remarks == string.Empty)
				{
					MessageBox.Show("Action Remarks Required");
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Update ExpiryDate?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "UpdateExpiryDate";
				item.Comments = Remarks;
				item.EntryUser = UserAccount.ID;
				item.PostDate = postData;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Update ExpiryDate Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "IsApproved")
			{
				ApproveRecord(Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value), GrdMain);
			}
			else if (((Control)(object)CmbIsApproved).Text == "Approve")
			{
				if (e.Column.Key == "Complete")
				{
					CompleteStatus(Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value), GrdMain);
				}
				if (e.Column.Key == "Open")
				{
					OpenStatus(Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value), GrdMain);
				}
				else if (e.Column.Key == "Cancel")
				{
					CancelStatus(Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value), Conversion.ToDouble(GrdMain.CurrentRow.Cells["DispatchQty"].Value), GrdMain);
				}
				else if (e.Column.Key == "Update")
				{
					UpdateExpiryDate(Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value), Conversion.ToDateTime(GrdMain.CurrentRow.Cells["ExpiryDate"].Value), GrdMain);
				}
			}
			if (e.Column.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (GrdMain.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(GrdMain.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GrdMain.CurrentRow;
			if (e.Column.Key == "DocNo")
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				new SaleActivityDetails(UserAccount, id).Show();
			}
			else if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value)), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = GrdMain.CurrentRow;
			if (r == null || (int)r.RowType != 0 || GrdMain.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			double DispatchQty = Conversion.ToDouble(r.Cells["DispatchQty"].Value);
			DateTime date = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
			if (GrdMain.CurrentColumn.Key == "IsApproved")
			{
				ApproveRecord(Id, GrdMain);
			}
			else if (GrdMain.CurrentColumn.Key == "Complete")
			{
				CompleteStatus(Id, GrdMain);
			}
			else if (GrdMain.CurrentColumn.Key == "Open")
			{
				OpenStatus(Id, GrdMain);
			}
			else if (GrdMain.CurrentColumn.Key == "Cancel")
			{
				CancelStatus(Id, DispatchQty, GrdMain);
			}
			else if (GrdMain.CurrentColumn.Key == "Update")
			{
				UpdateExpiryDate(Id, date, GrdMain);
			}
			else if (GrdMain.CurrentColumn.Key == "CustomerName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(r.Cells["SupplierCustomerId"].Value)), fromdate.Value, ToDate.Value);
			}
			else if (GrdMain.CurrentColumn.Key == "DocNo")
			{
				new SaleActivityDetails(UserAccount, Id).Show();
			}
			else if (GrdMain.CurrentColumn.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (GrdMain.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(GrdMain.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdMain.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dtDetail = new DataTable();
				dtDetail.Columns.Add("ReferenceParty");
				dtDetail.Columns.Add("ItemId");
				dtDetail.Columns.Add("SaleGLAC");
				dtDetail.Columns.Add("ItemName");
				dtDetail.Columns.Add("CropYear");
				dtDetail.Columns.Add("PackingType");
				dtDetail.Columns.Add("PackUom");
				dtDetail.Columns.Add("ItemQty", typeof(double));
				dtDetail.Columns.Add("DispatchQty", typeof(double));
				dtDetail.Columns.Add("BalQty", typeof(double));
				dtDetail.Columns.Add("Weight", typeof(double));
				dtDetail.Columns.Add("DispatchWeight", typeof(double));
				dtDetail.Columns.Add("BalWeight", typeof(double));
				dtDetail.Columns.Add("ItemRate", typeof(double));
				dtDetail.Columns.Add("ItemAmount", typeof(double));
				dtDetail.Columns.Add("BagPrice");
				dtDetail.Columns.Add("BagWeight");
				dtDetail.Columns.Add("CityName");
				DataTable resultRows = (from row in grdlst.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dtDetail.Rows.Add(resultRows.Rows[i]["ReferencePartyNameDetail"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["SaleGLAC"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["Crop"], resultRows.Rows[i]["PackTypeDesc"], resultRows.Rows[i]["UOMCodeItm"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["DispatchQty"], resultRows.Rows[i]["BalQty"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["DispatchWeight"], resultRows.Rows[i]["BalWeight"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["BagPrice"], resultRows.Rows[i]["BagWeight"], resultRows.Rows[i]["CityArea"]);
				}
				GrdDetail.DataSource = dtDetail;
				GrdDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MainGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "SupplierCustomerId", "DispatchQty", "BalWeight" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"ExpiryDate",
				(EditType)2
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(GrdMain, 2, 3, hiddenColumns, editableColumns);
			GrdMain.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdMain.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdMain.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdMain.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.PackingType;
			GrdMain.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdMain.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdMain.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			GrdMain.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.SupplierName;
			GrdMain.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			GrdMain.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdMain.RootTable.Columns["OrderStatus"].Width = Constants.InventoryConstants.StatusConstant;
			GrdMain.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			GrdMain.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdMain.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdMain.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdMain.RootTable.Columns["DispatchQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdMain.RootTable.Columns["OrderAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdMain.RootTable.Columns["ExpiryDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdMain.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdMain.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			GrdMain.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdMain.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			GrdMain.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdMain.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			GrdMain.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdMain.RootTable.Columns["ActionRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdMain.RootTable.Columns["Id"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["DocumentType"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["DocDate"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["DocNo"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["CustomerName"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["CommissionAgent"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["CommRate"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["CommAmount"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["OrderStatus"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["DueDays"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["DueDate"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["PaymentTerm"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["DeliveryTerm"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["DispatchQty"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["OrderAmount"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["ExpiryDate"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["EntryDate"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["EntryUser"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["ModifyDate"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["ModifyUser"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["ApprovedDate"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["ApprovedUser"].EditType = (EditType)0;
			GrdMain.RootTable.Columns["Remarks"].EditType = (EditType)0;
			ctrlGrdBar6_Load(null, null);
			GrdMain.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			GrdMain.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			if (((Control)(object)CmbIsApproved).Text == "Approve" && ((Control)(object)CmbOrderStatus).Text == "Open")
			{
				if (formright.CanChangeOrderStatusToComplete)
				{
					GrdMain.RootTable.Columns.Add("Complete");
					GrdMain.RootTable.Columns["Complete"].Caption = "Complete";
					GrdMain.RootTable.Columns["Complete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					GrdMain.RootTable.Columns["Complete"].ButtonStyle = (ButtonStyle)5;
					GrdMain.RootTable.Columns["Complete"].ButtonText = "Complete";
					GrdMain.RootTable.Columns["Complete"].Width = 80;
					btnCompleteOrders.Visible = true;
				}
				else
				{
					btnCompleteOrders.Visible = false;
				}
				if (formright.CanChangeOrderStatusToCancel)
				{
					GrdMain.RootTable.Columns.Add("Cancel");
					GrdMain.RootTable.Columns["Cancel"].Caption = "Cancel";
					GrdMain.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					GrdMain.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
					GrdMain.RootTable.Columns["Cancel"].ButtonText = "Cancel";
					GrdMain.RootTable.Columns["Cancel"].Width = 60;
					btnCancelOrders.Visible = true;
				}
				else
				{
					btnCancelOrders.Visible = false;
				}
			}
			else
			{
				btnCancelOrders.Visible = false;
				btnCompleteOrders.Visible = false;
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApprove")
			{
				GrdMain.RootTable.Columns.Add("IsApproved");
				GrdMain.RootTable.Columns["IsApproved"].Caption = "IsApproved";
				GrdMain.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				GrdMain.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
				GrdMain.RootTable.Columns["IsApproved"].ButtonText = "IsApproved";
				GrdMain.RootTable.Columns["IsApproved"].Width = 60;
				GrdMain.RootTable.Columns["IsApproved"].Visible = false;
			}
			if (((Control)(object)CmbOrderStatus).Text == "Open" && ((Control)(object)CmbIsApproved).Text == "Approve" && formright.CanChangeOrderExpiryDate)
			{
				GrdMain.AllowEdit = (InheritableBoolean)1;
				GrdMain.RootTable.Columns["ExpiryDate"].EditType = (EditType)2;
				GrdMain.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
				GrdMain.RootTable.Columns.Add("Update");
				GrdMain.RootTable.Columns["Update"].Caption = "UpdateExpiryDate";
				GrdMain.RootTable.Columns["Update"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				GrdMain.RootTable.Columns["Update"].ButtonStyle = (ButtonStyle)5;
				GrdMain.RootTable.Columns["Update"].ButtonText = "UpdateExpiryDate";
				GrdMain.RootTable.Columns["Update"].Width = 120;
			}
			GrdMain.RootTable.Columns.Add("ApprovalDetail");
			GrdMain.RootTable.Columns["ApprovalDetail"].Caption = "Approval Detail";
			GrdMain.RootTable.Columns["ApprovalDetail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdMain.RootTable.Columns["ApprovalDetail"].ButtonStyle = (ButtonStyle)5;
			GrdMain.RootTable.Columns["ApprovalDetail"].ButtonText = "Approval Detail";
			GrdMain.RootTable.Columns["ApprovalDetail"].Width = 100;
			GrdMain.RootTable.Columns["ApprovalDetail"].Position = 0;
			if (((Control)(object)CmbIsApproved).Text == "Approve" && ((Control)(object)CmbOrderStatus).Text != "" && ((Control)(object)CmbOrderStatus).Text != "Open")
			{
				if (formright.CanChangeOrderStatusToOpen)
				{
					GrdMain.RootTable.Columns.Add("Open");
					GrdMain.RootTable.Columns["Open"].Caption = "Open";
					GrdMain.RootTable.Columns["Open"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					GrdMain.RootTable.Columns["Open"].ButtonStyle = (ButtonStyle)5;
					GrdMain.RootTable.Columns["Open"].ButtonText = "Open";
					GrdMain.RootTable.Columns["Open"].Width = 45;
					btnOpenOrders.Visible = true;
				}
				else
				{
					btnOpenOrders.Visible = false;
				}
			}
			else
			{
				btnOpenOrders.Visible = false;
			}
			if (((Control)(object)CmbIsApproved).Text == "Approve" && ((Control)(object)CmbOrderStatus).Text != "")
			{
				if (formright.CanChangeOrderStatusToComplete || formright.CanChangeOrderStatusToCancel)
				{
					GrdMain.RootTable.Columns.Add("Select");
					GrdMain.RootTable.Columns["Select"].ActAsSelector = true;
					GrdMain.RootTable.Columns["Select"].UseHeaderSelector = true;
					GrdMain.RootTable.Columns["Select"].Width = 40;
					GrdMain.RootTable.Columns["Select"].Position = 1;
					GrdMain.RootTable.Columns["ActionRemarks"].Position = 2;
					GrdMain.FrozenColumns = 3;
					btnAutoCompleteOrder.Visible = !(((Control)(object)CmbOrderStatus).Text == "Complete");
				}
				else
				{
					GrdMain.RootTable.Columns["ActionRemarks"].Position = 1;
					GrdMain.FrozenColumns = 2;
					btnAutoCompleteOrder.Visible = false;
				}
			}
			else
			{
				GrdMain.RootTable.Columns["ActionRemarks"].Position = 1;
				GrdMain.FrozenColumns = 2;
				btnAutoCompleteOrder.Visible = false;
			}
			if (GrdMain.RootTable.Columns.Contains("Complete") || GrdMain.RootTable.Columns.Contains("Cancel") || GrdMain.RootTable.Columns.Contains("Update") || GrdMain.RootTable.Columns.Contains("Open"))
			{
				GrdMain.RootTable.Columns["ActionRemarks"].EditType = (EditType)1;
			}
			else
			{
				GrdMain.RootTable.Columns["ActionRemarks"].EditType = (EditType)0;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdsaleorder.AllowEdit = (InheritableBoolean)1;
			grdsaleorder.RootTable.Columns["ReferencePartyDetail"].Caption = "Reference Party";
			grdsaleorder.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdsaleorder.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdsaleorder.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdsaleorder.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.PackingType;
			grdsaleorder.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdsaleorder.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdsaleorder.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.StringConstant;
			grdsaleorder.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.StringConstant;
			grdsaleorder.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.StringConstant;
			grdsaleorder.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdsaleorder.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdsaleorder.RootTable.Columns["OrderStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdsaleorder.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdsaleorder.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdsaleorder.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdsaleorder.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdsaleorder.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdsaleorder.RootTable.Columns["ReferencePartyDetail"].Width = Constants.InventoryConstants.StringConstant;
			grdsaleorder.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdsaleorder.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdsaleorder.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdsaleorder.RootTable.Columns["DispatchQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdsaleorder.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdsaleorder.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdsaleorder.RootTable.Columns["DispatchWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdsaleorder.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdsaleorder.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdsaleorder.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdsaleorder.RootTable.Columns["BagPrice"].Width = Constants.InventoryConstants.FclConstant;
			grdsaleorder.RootTable.Columns["BagWeight"].Width = 65;
			grdsaleorder.RootTable.Columns["ExpiryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdsaleorder.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdsaleorder.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdsaleorder.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdsaleorder.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdsaleorder.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdsaleorder.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdsaleorder.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdsaleorder.RootTable.Columns["ActionRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdsaleorder.RootTable.HeaderLines = 2;
			grdsaleorder.RootTable.Columns["DocumentType"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["DocDate"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["DocNo"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["CustomerName"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["ReferenceParty"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["CommissionAgent"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["CommRate"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["CommAmount"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["OrderStatus"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["DueDays"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["DueDate"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["SaleGLAC"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["PaymentTerm"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["DeliveryTerm"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["ReferencePartyDetail"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["CropYear"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["ItemQty"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["DispatchQty"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["BalQty"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["Weight"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["DispatchWeight"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["BalWeight"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["ItemRate"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["ItemAmount"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["BagPrice"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["BagWeight"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["ExpiryDate"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["EntryDate"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["EntryUser"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["Remarks"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.RootTable.Columns["ActionRemarks"].HeaderAlignment = (TextAlignment)2;
			grdsaleorder.AllowEdit = (InheritableBoolean)1;
			grdsaleorder.RootTable.Columns["Id"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DocumentType"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["CustomerName"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ReferenceParty"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["CommissionAgent"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["CommRate"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["CommAmount"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["OrderSupCustId"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["OrderStatus"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DueDays"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DueDate"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["OrderItemId"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["SaleGLAC"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["PaymentTerm"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DeliveryTerm"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ReferencePartyDetail"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DispatchQty"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DispatchWeight"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["BagPrice"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["BagWeight"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ExpiryDate"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["EntryDate"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["EntryUser"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ModifyDate"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ModifyUser"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ApprovedDate"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["ApprovedUser"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["DeliveryDays"].EditType = (EditType)0;
			grdsaleorder.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["DocumentTypeId"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["DocumentType"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdsaleorder.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["CustomerName"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["ReferenceParty"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["CommissionAgent"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["CommRate"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["CommAmount"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["OrderSupCustId"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["OrderStatus"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["OrderStatus"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["DueDays"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["DueDate"].FilterEditType = (FilterEditType)6;
			grdsaleorder.RootTable.Columns["OrderItemId"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["SaleGLAC"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["PaymentTerm"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["DeliveryTerm"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["ReferencePartyDetail"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["PackUom"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["ItemQty"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["DispatchQty"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["BalQty"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["DispatchWeight"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["BalWeight"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["ItemRate"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["ItemAmount"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["BagPrice"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["BagWeight"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
			grdsaleorder.RootTable.Columns["EntryDate"].FilterEditType = (FilterEditType)6;
			grdsaleorder.RootTable.Columns["EntryUser"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["ModifyDate"].FilterEditType = (FilterEditType)6;
			grdsaleorder.RootTable.Columns["ModifyUser"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["ApprovedDate"].FilterEditType = (FilterEditType)6;
			grdsaleorder.RootTable.Columns["ApprovedUser"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdsaleorder.RootTable.Columns["Id"].Visible = false;
			grdsaleorder.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdsaleorder.RootTable.Columns["OrderSupCustId"].Visible = false;
			grdsaleorder.RootTable.Columns["SaleGLAC"].Visible = false;
			grdsaleorder.RootTable.Columns["OrderItemId"].Visible = false;
			grdsaleorder.RootTable.Columns["ReferenceParty"].Visible = false;
			grdsaleorder.RootTable.GroupTotals = (GroupTotals)2;
			grdsaleorder.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grdsaleorder.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			grdsaleorder.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["DispatchQty"].FormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["DispatchWeight"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["DispatchWeight"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["DispatchWeight"].FormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["DispatchWeight"].TotalFormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["BalWeight"].FormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdsaleorder.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdsaleorder.RootTable.Columns["BagPrice"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["BagPrice"].FormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["BagWeight"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["BagWeight"].FormatString = "#,##0.##";
			ctrlGrdBar3_Load(null, null);
			if (((Control)(object)CmbIsApproved).Text == "Approve" && ((Control)(object)CmbOrderStatus).Text == "Open")
			{
				if (formright.CanChangeOrderStatusToComplete)
				{
					grdsaleorder.RootTable.Columns.Add("Complete");
					grdsaleorder.RootTable.Columns["Complete"].Caption = "Complete";
					grdsaleorder.RootTable.Columns["Complete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdsaleorder.RootTable.Columns["Complete"].ButtonStyle = (ButtonStyle)5;
					grdsaleorder.RootTable.Columns["Complete"].ButtonText = "Complete";
					grdsaleorder.RootTable.Columns["Complete"].Width = 80;
					btnCompleteOrders.Visible = true;
				}
				else
				{
					btnCompleteOrders.Visible = false;
				}
				if (formright.CanChangeOrderStatusToCancel)
				{
					grdsaleorder.RootTable.Columns.Add("Cancel");
					grdsaleorder.RootTable.Columns["Cancel"].Caption = "Cancel";
					grdsaleorder.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdsaleorder.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
					grdsaleorder.RootTable.Columns["Cancel"].ButtonText = "Cancel";
					grdsaleorder.RootTable.Columns["Cancel"].Width = 60;
					btnCancelOrders.Visible = true;
				}
				else
				{
					btnCancelOrders.Visible = false;
				}
			}
			else
			{
				btnCompleteOrders.Visible = false;
				btnCancelOrders.Visible = false;
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApprove")
			{
				grdsaleorder.RootTable.Columns.Add("IsApproved");
				grdsaleorder.RootTable.Columns["IsApproved"].Caption = "IsApproved";
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonText = "IsApproved";
				grdsaleorder.RootTable.Columns["IsApproved"].Width = 60;
				grdsaleorder.RootTable.Columns["IsApproved"].Visible = false;
			}
			if (((Control)(object)CmbOrderStatus).Text == "Open" && ((Control)(object)CmbIsApproved).Text == "Approve" && formright.CanChangeOrderExpiryDate)
			{
				grdsaleorder.AllowEdit = (InheritableBoolean)1;
				grdsaleorder.RootTable.Columns["ExpiryDate"].EditType = (EditType)2;
				grdsaleorder.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
				grdsaleorder.RootTable.Columns.Add("Update");
				grdsaleorder.RootTable.Columns["Update"].Caption = "UpdateExpiryDate";
				grdsaleorder.RootTable.Columns["Update"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["Update"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["Update"].ButtonText = "UpdateExpiryDate";
				grdsaleorder.RootTable.Columns["Update"].Width = 120;
			}
			if (((Control)(object)CmbIsApproved).Text == "Approve" && ((Control)(object)CmbOrderStatus).Text != "" && ((Control)(object)CmbOrderStatus).Text != "Open")
			{
				if (formright.CanChangeOrderStatusToOpen)
				{
					grdsaleorder.RootTable.Columns.Add("Open");
					grdsaleorder.RootTable.Columns["Open"].Caption = "Open";
					grdsaleorder.RootTable.Columns["Open"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdsaleorder.RootTable.Columns["Open"].ButtonStyle = (ButtonStyle)5;
					grdsaleorder.RootTable.Columns["Open"].ButtonText = "Open";
					grdsaleorder.RootTable.Columns["Open"].Width = 45;
					btnOpenOrders.Visible = true;
				}
				else
				{
					btnOpenOrders.Visible = false;
				}
			}
			else
			{
				btnOpenOrders.Visible = false;
			}
			grdsaleorder.RootTable.Columns.Add("ApprovalDetail");
			grdsaleorder.RootTable.Columns["ApprovalDetail"].Caption = "Approval Detail";
			grdsaleorder.RootTable.Columns["ApprovalDetail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdsaleorder.RootTable.Columns["ApprovalDetail"].ButtonStyle = (ButtonStyle)5;
			grdsaleorder.RootTable.Columns["ApprovalDetail"].ButtonText = "Approval Detail";
			grdsaleorder.RootTable.Columns["ApprovalDetail"].Width = 100;
			grdsaleorder.RootTable.Columns["ApprovalDetail"].Position = 0;
			if (((Control)(object)CmbIsApproved).Text == "Approve")
			{
				if (formright.CanChangeOrderStatusToComplete || formright.CanChangeOrderStatusToCancel || formright.CanChangeOrderStatusToOpen)
				{
					grdsaleorder.RootTable.Columns.Add("Select");
					grdsaleorder.RootTable.Columns["Select"].ActAsSelector = true;
					grdsaleorder.RootTable.Columns["Select"].UseHeaderSelector = true;
					grdsaleorder.RootTable.Columns["Select"].Width = 40;
					grdsaleorder.RootTable.Columns["Select"].Position = 1;
					grdsaleorder.RootTable.Columns["ActionRemarks"].Position = 2;
					grdsaleorder.FrozenColumns = 3;
				}
				else
				{
					grdsaleorder.RootTable.Columns["ActionRemarks"].Position = 1;
					grdsaleorder.FrozenColumns = 2;
				}
			}
			else
			{
				grdsaleorder.RootTable.Columns["ActionRemarks"].Position = 1;
				grdsaleorder.FrozenColumns = 2;
			}
			if (grdsaleorder.RootTable.Columns.Contains("Complete") || grdsaleorder.RootTable.Columns.Contains("Cancel") || grdsaleorder.RootTable.Columns.Contains("Update") || grdsaleorder.RootTable.Columns.Contains("Open"))
			{
				grdsaleorder.RootTable.Columns["ActionRemarks"].EditType = (EditType)1;
			}
			else
			{
				grdsaleorder.RootTable.Columns["ActionRemarks"].EditType = (EditType)0;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void gridHisory()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			grdlst.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.EntryUser = UserAccount.ID;
			if (updateFromDate)
			{
				obj.FromDate = fromdate.Value;
			}
			obj.ToDate = ToDate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtsrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
			obj.BookingPersonId = Conversion.ToInt(CmbBookingPerson.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
			obj.CityId = Conversion.ToInt(ultraCombo1.Value);
			obj.Status = Conversion.ToString((object)((Control)(object)CmbOrderStatus).Text);
			if (((Control)(object)CmbIsApproved).Text == "Approve")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApprove")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)CmbIsApproved).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			if (chkincludeDo.Checked)
			{
				obj.ActionId = 1;
			}
			obj.RefPartyId = Conversion.ToInt(cmbRefPartyDetail.Value);
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
				if (UserAccount.AppId == 5 && Conversion.ToInt(CmbCostCenter.Value) == 0)
				{
					throw new Exception("Cost Center Not Found");
				}
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				DataSet ds = SaleOrder.SaleOrderHistory(obj);
				grdlst = ds?.Tables[0];
				DataTable dataTable = grdlst;
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					ReportHelper.ReplaceCompLogoColumn(grdlst, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("DocumentTypeId", typeof(int));
					dt.Columns.Add("DocumentType", typeof(string));
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("CustomerName", typeof(string));
					dt.Columns.Add("BookingPerson", typeof(string));
					dt.Columns.Add("ReferencePartyDetail", typeof(string));
					dt.Columns.Add("CommissionAgent", typeof(string));
					dt.Columns.Add("CommRate", typeof(double));
					dt.Columns.Add("CommAmount", typeof(double));
					dt.Columns.Add("OrderSupCustId", typeof(int));
					dt.Columns.Add("DueDays", typeof(int));
					dt.Columns.Add("DueDate", typeof(DateTime));
					dt.Columns.Add("OrderItemId", typeof(int));
					dt.Columns.Add("SaleGLAC", typeof(int));
					dt.Columns.Add("PaymentTerm", typeof(string));
					dt.Columns.Add("DeliveryTerm", typeof(string));
					dt.Columns.Add("ItemName", typeof(string));
					dt.Columns.Add("CropYear", typeof(string));
					dt.Columns.Add("PackUom", typeof(string));
					dt.Columns.Add("ItemQty", typeof(double));
					dt.Columns.Add("DispatchQty", typeof(double));
					dt.Columns.Add("BalQty", typeof(double));
					dt.Columns.Add("Weight", typeof(double));
					dt.Columns.Add("DispatchWeight", typeof(double));
					dt.Columns.Add("BalWeight", typeof(double));
					dt.Columns.Add("ItemRate", typeof(double));
					dt.Columns.Add("ItemAmount", typeof(double));
					dt.Columns.Add("OrderStatus", typeof(string));
					dt.Columns.Add("BagPrice", typeof(double));
					dt.Columns.Add("BagWeight", typeof(double));
					dt.Columns.Add("DeliveryDays", typeof(int));
					dt.Columns.Add("ExpiryDate", typeof(DateTime));
					dt.Columns.Add("EntryDate", typeof(DateTime));
					dt.Columns.Add("EntryUser", typeof(string));
					dt.Columns.Add("ModifyDate", typeof(DateTime));
					dt.Columns.Add("ModifyUser", typeof(string));
					dt.Columns.Add("ApprovedDate", typeof(DateTime));
					dt.Columns.Add("ApprovedUser", typeof(string));
					dt.Columns.Add("ReferenceParty", typeof(string));
					dt.Columns.Add("Remarks", typeof(string));
					dt.Columns.Add("ActionRemarks", typeof(string));
					dt.Rows.Clear();
					for (int j = 0; j < grdlst.Rows.Count; j++)
					{
						dt.Rows.Add(grdlst.Rows[j]["Id"], grdlst.Rows[j]["DocumentTypeId"], grdlst.Rows[j]["DocumentTypeDescription"], Conversion.ToDateTime(grdlst.Rows[j]["DocDate"]).ToString("dd-MMM-yyyy"), grdlst.Rows[j]["DocNo"], grdlst.Rows[j]["SupplierName"], grdlst.Rows[j]["BookingPerson"], grdlst.Rows[j]["ReferencePartyNameDetail"], grdlst.Rows[j]["CompanyNameSpCAgent"], grdlst.Rows[j]["CommRate"], grdlst.Rows[j]["CommAmount"], grdlst.Rows[j]["OrderSupCustId"], grdlst.Rows[j]["OrderDueDays"], Conversion.ToDateTime(grdlst.Rows[j]["OrderDueDate"]).ToString("dd-MMM-yy"), grdlst.Rows[j]["OrderItemId"], grdlst.Rows[j]["SaleGLAC"], grdlst.Rows[j]["DeliveryTerm"], grdlst.Rows[j]["TermsDescription"], grdlst.Rows[j]["ItemName"], grdlst.Rows[j]["Crop"], grdlst.Rows[j]["UOMCodeItm"], grdlst.Rows[j]["OrderItemQty"], grdlst.Rows[j]["DispatchQty"], grdlst.Rows[j]["BalQty"], grdlst.Rows[j]["NetWeight"], grdlst.Rows[j]["DispatchWeight"], grdlst.Rows[j]["BalWeight"], grdlst.Rows[j]["OrderItemRate"], grdlst.Rows[j]["Amount"], grdlst.Rows[j]["OrderStatus"], grdlst.Rows[j]["BagPrice"], grdlst.Rows[j]["BagWeight"], grdlst.Rows[j]["DeliveryDays"], Conversion.ToDateTime(grdlst.Rows[j]["OrderExpiryDate"]).ToString("dd-MMM-yy"), grdlst.Rows[j]["EntryDate"], grdlst.Rows[j]["UserNameEusr"], grdlst.Rows[j]["ModifyDate"], Conversion.ToString(grdlst.Rows[j]["UserNameMusr"]), grdlst.Rows[j]["PostDate"], Conversion.ToString(grdlst.Rows[j]["UserNameAusr"]), grdlst.Rows[j]["ReferencePartyNameDetail"], grdlst.Rows[j]["RemarksHeader"], Conversion.ToString(grdlst.Rows[j]["ActionRemarks"]));
					}
					grdsaleorder.DataSource = dt;
					grdsaleorder.RetrieveStructure();
					GridSettings();
					DataTable dtMain = new DataTable();
					dtMain.Columns.Add("Id", typeof(string));
					dtMain.Columns.Add("DocumentTypeId", typeof(int));
					dtMain.Columns.Add("DocumentType", typeof(string));
					dtMain.Columns.Add("DocDate", typeof(DateTime));
					dtMain.Columns.Add("DocNo", typeof(int));
					dtMain.Columns.Add("SupplierCustomerId", typeof(int));
					dtMain.Columns.Add("CustomerName", typeof(string));
					dtMain.Columns.Add("BookingPerson", typeof(string));
					dtMain.Columns.Add("CommissionAgent", typeof(string));
					dtMain.Columns.Add("CommRate", typeof(double));
					dtMain.Columns.Add("CommAmount", typeof(double));
					dtMain.Columns.Add("OrderStatus", typeof(string));
					dtMain.Columns.Add("DueDays", typeof(int));
					dtMain.Columns.Add("DueDate", typeof(DateTime));
					dtMain.Columns.Add("PaymentTerm", typeof(string));
					dtMain.Columns.Add("DeliveryTerm", typeof(string));
					dtMain.Columns.Add("DispatchQty", typeof(double));
					dtMain.Columns.Add("OrderAmount", typeof(double));
					dtMain.Columns.Add("BalWeight", typeof(double));
					dtMain.Columns.Add("DeliveryDays", typeof(int));
					dtMain.Columns.Add("ExpiryDate", typeof(DateTime));
					dtMain.Columns.Add("EntryDate", typeof(DateTime));
					dtMain.Columns.Add("EntryUser", typeof(string));
					dtMain.Columns.Add("ModifyDate", typeof(DateTime));
					dtMain.Columns.Add("ModifyUser", typeof(string));
					dtMain.Columns.Add("ApprovedDate", typeof(DateTime));
					dtMain.Columns.Add("ApprovedUser", typeof(string));
					dtMain.Columns.Add("Remarks", typeof(string));
					dtMain.Columns.Add("ActionRemarks", typeof(string));
					int i;
					for (i = 0; i < grdlst.Rows.Count; i++)
					{
						bool num = dtMain.AsEnumerable().Any((DataRow row) => row.Field<string>("Id") == Conversion.ToString(grdlst.Rows[i]["Id"]));
						double totalAmount = (from row in grdlst.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(grdlst.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<double>("Amount"));
						double BalWeight = (from row in grdlst.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(grdlst.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<double>("BalWeight"));
						if (!num)
						{
							dtMain.Rows.Add(grdlst.Rows[i]["Id"], grdlst.Rows[i]["DocumentTypeId"], grdlst.Rows[i]["DocumentTypeDescription"], Conversion.ToDateTime(grdlst.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), grdlst.Rows[i]["DocNo"], grdlst.Rows[i]["OrderSupCustId"], grdlst.Rows[i]["SupplierName"], grdlst.Rows[i]["BookingPerson"], grdlst.Rows[i]["CompanyNameSpCAgent"], grdlst.Rows[i]["CommRate"], grdlst.Rows[i]["CommAmount"], grdlst.Rows[i]["OrderStatus"], grdlst.Rows[i]["OrderDueDays"], Conversion.ToDateTime(grdlst.Rows[i]["OrderDueDate"]).ToString("dd-MMM-yy"), grdlst.Rows[i]["TermsDescription"], grdlst.Rows[i]["DeliveryTerm"], grdlst.Rows[i]["DispatchQty"], totalAmount, BalWeight, grdlst.Rows[i]["DeliveryDays"], Conversion.ToDateTime(grdlst.Rows[i]["OrderExpiryDate"]).ToString("dd-MMM-yy"), Conversion.ToDateTime(grdlst.Rows[i]["EntryDate"]), grdlst.Rows[i]["UserNameEusr"], grdlst.Rows[i]["ModifyDate"], Conversion.ToString(grdlst.Rows[i]["UserNameMusr"]), grdlst.Rows[i]["PostDate"], Conversion.ToString(grdlst.Rows[i]["UserNameAusr"]), grdlst.Rows[i]["RemarksHeader"], Conversion.ToString(grdlst.Rows[i]["ActionRemarks"]));
						}
					}
					GrdMain.DataSource = dtMain;
					GrdMain.RetrieveStructure();
					MainGridSetting();
				}
				else
				{
					waitForm.Close();
					grdsaleorder.DataSource = null;
					GrdMain.ClearStructure();
					GrdDetail.ClearStructure();
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

	private void btnshow_Click_1(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			gridHisory();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void grdsaleorder_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "IsApproved")
			{
				ApproveRecord(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value), grdsaleorder);
			}
			if (e.Column.Key == "Complete")
			{
				CompleteStatus(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value), grdsaleorder);
			}
			else if (e.Column.Key == "Open")
			{
				OpenStatus(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value), grdsaleorder);
			}
			else if (e.Column.Key == "Cancel")
			{
				CancelStatus(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value), Conversion.ToDouble(grdsaleorder.CurrentRow.Cells["DispatchQty"].Value), grdsaleorder);
			}
			else if (e.Column.Key == "Update")
			{
				UpdateExpiryDate(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value), Conversion.ToDateTime(grdsaleorder.CurrentRow.Cells["ExpiryDate"].Value), grdsaleorder);
			}
			else if (e.Column.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grdsaleorder.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdsaleorder.CurrentRow;
			if (e.Column.Key == "DocNo")
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				if (UserAccount.RoleName == "Admin")
				{
					new SaleActivityDetails(UserAccount, id).Show();
				}
				else if (clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "SaleActivityDetails").ToList();
					if (newList.Count <= 0)
					{
						MessageBox.Show("you don't have Screen Rights");
						return;
					}
					if (!Conversion.ToBool((object)newList[0].Value.ToString()))
					{
						MessageBox.Show("you don't have Screen Rights");
						return;
					}
					new SaleActivityDetails(UserAccount, id).Show();
				}
			}
			if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(item.Cells["OrderSupCustId"].Value)), fromdate.Value, ToDate.Value);
			}
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["OrderItemId"].Value), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			GrdDetail.RootTable.Columns["ItemId"].Visible = false;
			GrdDetail.RootTable.Columns["SaleGLAC"].Visible = false;
			GrdDetail.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName;
			GrdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			GrdDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			GrdDetail.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			GrdDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			GrdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdDetail.RootTable.Columns["DispatchQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdDetail.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdDetail.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetail.RootTable.Columns["DispatchWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetail.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			GrdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetail.RootTable.Columns["BagPrice"].Width = Constants.InventoryConstants.FclConstant;
			GrdDetail.RootTable.Columns["BagWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetail.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.StatusConstant;
			GrdDetail.RootTable.HeaderLines = 2;
			GrdDetail.RootTable.Columns["ReferenceParty"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["ItemId"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["SaleGLAC"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["CropYear"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["PackingType"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["ItemQty"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["DispatchQty"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["BalQty"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["Weight"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["DispatchWeight"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["BalWeight"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["ItemRate"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["ItemAmount"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["BagPrice"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["BagWeight"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["CityName"].HeaderAlignment = (TextAlignment)2;
			GrdDetail.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			GrdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["DispatchQty"].FormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["BalQty"].FormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["DispatchWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["DispatchWeight"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["DispatchWeight"].FormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["DispatchWeight"].TotalFormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["BalWeight"].FormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.##";
			GrdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GrdDetail.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GrdDetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GrdDetail.CurrentColumn != null && GrdDetail.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["SaleGLAC"].Value);
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), fromdate.Value, ToDate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSrFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsrTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ActivityFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Order Register");
			dt.Rows.Add(2, "Order Summary By Item");
			dt.Rows.Add(3, "Order Summary By Item & Pack Size");
			dt.Rows.Add(4, "Order Summary By Item & City");
			dt.Rows.Add(5, "Order Summary By Item,Pack Size & City");
			dt.Rows.Add(6, "Order Summary By Customer");
			dt.Rows.Add(7, "Order Summary By Customer & Item");
			dt.Rows.Add(8, "Order Summary By Customer & Pack Size");
			dt.Rows.Add(9, "Order Summary By Customer & City");
			dt.Rows.Add(10, "Order Summary By Customer,Item & City");
			dt.Rows.Add(11, "Order Summary By Customer and ReferenceParty");
			dt.Rows.Add(12, "Order Summary By Customer & Item & ReferenceParty");
			dt.Rows.Add(13, "Order Summary By ReferenceParty");
			dt.Rows.Add(14, "Order Summary By ReferenceParty & City");
			dt.Rows.Add(15, "Order Summary By ReferenceParty & Item");
			DDL.BindDDL(dt, cmbActivitySumm, "Id", "Name", "Activity", ZeroIndex: false);
			((UltraGridBase)cmbActivitySumm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnRefreshSummary_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			AllDropDownBind();
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

	private void BtnShowSumm_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbActivitySumm).ActiveRow != null)
			{
				GridSummaryFill();
				PrintButtonManage();
			}
			else
			{
				MessageBox.Show("Please Select Activity First...");
				cmbActivitySumm.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummaryFill()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			waitForm.Show(this);
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.FromDate = Conversion.ToDateTime((object)txtFromDateSumm.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDateSumm.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoSumm.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDateSumm.Text);
			obj.ParentCategoryId = Conversion.ToInt(cmbParentcategorySumm.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbCategorySumm.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtypeSumm.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbCustomerSumm.Value);
			obj.BookingPersonId = Conversion.ToInt(cmbBookingPersonSumm.Value);
			obj.ItemId = Conversion.ToInt(cmbItemNameSumm.Value);
			obj.PackSizeFrom = Conversion.ToInt(cmbPackUomSumm.Value);
			obj.CropYear = ((Conversion.ToInt(cmbCropSumm.Value) > 0) ? ((Control)(object)cmbCropSumm).Text : null);
			obj.JobLotId = Conversion.ToInt(cmbJobLotSumm.Value);
			obj.PackingTypeId = Conversion.ToInt(cmbPackingtypeSumm.Value);
			obj.CityId = Conversion.ToInt(cmbcitySumm.Value);
			obj.DistrictId = Conversion.ToInt(cmbDistrictSumm.Value);
			obj.ReportType = ((Control)(object)cmbActivitySumm).Text;
			if (((Control)(object)CmbApprovedStatusSummary).Text == "Approve")
			{
				obj.IsApproved = true;
			}
			else if (((Control)(object)CmbApprovedStatusSummary).Text == "UnApprove")
			{
				obj.IsApproved = false;
			}
			else if (((Control)(object)CmbApprovedStatusSummary).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			if (ChkDoApprovedIncludSummary.Checked)
			{
				obj.ActionId = 1;
			}
			obj.RefPartyId = Conversion.ToInt(cmbRefPartyDetailSumm.Value);
			if (chkSkipZero.Checked)
			{
				obj.SkipZero = 1;
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchNameSumm).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchNameSumm).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				if (UserAccount.AppId == 5 && Conversion.ToInt(CmbCostCenter.Value) == 0)
				{
					throw new Exception("Cost Center Not Found");
				}
				obj.Status = Conversion.ToString((object)((Control)(object)CmbStatusSummary).Text);
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				dtGrid = SaleOrder.SaleOrderSummaryRegister(obj);
				if (dtGrid.Rows.Count > 0)
				{
					if (((UltraGridBase)cmbActivitySumm).ActiveRow != null)
					{
						if (((Control)(object)cmbActivitySumm).Text == "Order Register")
						{
							((Control)(object)DataGridHistory).Name = "OrderRegister";
							dtSalesRegister.Rows.Clear();
							for (int i = 0; i < dtGrid.Rows.Count; i++)
							{
								dtSalesRegister.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["DocumentTypeId"], dtGrid.Rows[i]["BranchName"], dtGrid.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), dtGrid.Rows[i]["DocNo"], dtGrid.Rows[i]["Customer"], dtGrid.Rows[i]["BookingPerson"], dtGrid.Rows[i]["ReferencePartyDetail"], dtGrid.Rows[i]["ItemCode"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["UOMCode"], dtGrid.Rows[i]["CropBatch"], dtGrid.Rows[i]["JobLotCode"], dtGrid.Rows[i]["PackTypeCode"], dtGrid.Rows[i]["OrderQty"], dtGrid.Rows[i]["DispatchQty"], dtGrid.Rows[i]["BalQty"], dtGrid.Rows[i]["OrderWeight"], dtGrid.Rows[i]["DispatchWeight"], dtGrid.Rows[i]["BalWeight"], dtGrid.Rows[i]["OrderAmount"], dtGrid.Rows[i]["DispatchAmount"], dtGrid.Rows[i]["BalAmount"], dtGrid.Rows[i]["AvgRate"], dtGrid.Rows[i]["PrcntOfTotal"], dtGrid.Rows[i]["CityName"]);
							}
							DataGridHistory.DataSource = dtSalesRegister;
							DataGridHistory.RetrieveStructure();
							DataGridHistorySetting();
							DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
							DataGridHistory.RootTable.Columns["DocType"].Width = Constants.InventoryConstants.DocNoConstant;
							DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
							DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
							DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["BookingPerson"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
							DataGridHistory.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
							DataGridHistory.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
							DataGridHistory.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
							DataGridHistory.RootTable.Columns["BranchName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["DocType"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["DocDate"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["DocNo"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PartyName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ReferenceParty"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemCode"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["CropYear"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["JobLot"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PackingType"].HeaderAlignment = (TextAlignment)2;
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByItem";
							dtSalesSummaryByItem.Rows.Clear();
							for (int j = 0; j < dtGrid.Rows.Count; j++)
							{
								dtSalesSummaryByItem.Rows.Add(dtGrid.Rows[j]["Id"], dtGrid.Rows[j]["ItemCode"], dtGrid.Rows[j]["ItemName"], dtGrid.Rows[j]["OrderQty"], dtGrid.Rows[j]["DispatchQty"], dtGrid.Rows[j]["BalQty"], dtGrid.Rows[j]["OrderWeight"], dtGrid.Rows[j]["DispatchWeight"], dtGrid.Rows[j]["BalWeight"], dtGrid.Rows[j]["OrderAmount"], dtGrid.Rows[j]["DispatchAmount"], dtGrid.Rows[j]["BalAmount"], dtGrid.Rows[j]["AvgRate"], dtGrid.Rows[j]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByItem;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & Pack Size")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByItemAndPackSize";
							dtSalesSummaryByItemandPackSize.Rows.Clear();
							for (int k = 0; k < dtGrid.Rows.Count; k++)
							{
								dtSalesSummaryByItemandPackSize.Rows.Add(dtGrid.Rows[k]["Id"], dtGrid.Rows[k]["ItemCode"], dtGrid.Rows[k]["ItemName"], dtGrid.Rows[k]["UOMCode"], dtGrid.Rows[k]["OrderQty"], dtGrid.Rows[k]["DispatchQty"], dtGrid.Rows[k]["BalQty"], dtGrid.Rows[k]["OrderWeight"], dtGrid.Rows[k]["DispatchWeight"], dtGrid.Rows[k]["BalWeight"], dtGrid.Rows[k]["OrderAmount"], dtGrid.Rows[k]["DispatchAmount"], dtGrid.Rows[k]["BalAmount"], dtGrid.Rows[k]["AvgRate"], dtGrid.Rows[k]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByItemandPackSize;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & City")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByItemAndCity";
							dtSalesSummaryByItemandCity.Rows.Clear();
							for (int l = 0; l < dtGrid.Rows.Count; l++)
							{
								dtSalesSummaryByItemandCity.Rows.Add(dtGrid.Rows[l]["Id"], dtGrid.Rows[l]["ItemCode"], dtGrid.Rows[l]["ItemName"], dtGrid.Rows[l]["OrderQty"], dtGrid.Rows[l]["DispatchQty"], dtGrid.Rows[l]["BalQty"], dtGrid.Rows[l]["OrderWeight"], dtGrid.Rows[l]["DispatchWeight"], dtGrid.Rows[l]["BalWeight"], dtGrid.Rows[l]["OrderAmount"], dtGrid.Rows[l]["DispatchAmount"], dtGrid.Rows[l]["BalAmount"], dtGrid.Rows[l]["AvgRate"], dtGrid.Rows[l]["PrcntOfTotal"], dtGrid.Rows[l]["CityName"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByItemandCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item,Pack Size & City")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByItemPackSizeAndCity";
							dtSalesSummaryByItemNPackSizeCity.Rows.Clear();
							for (int m = 0; m < dtGrid.Rows.Count; m++)
							{
								dtSalesSummaryByItemNPackSizeCity.Rows.Add(dtGrid.Rows[m]["Id"], dtGrid.Rows[m]["ItemCode"], dtGrid.Rows[m]["ItemName"], dtGrid.Rows[m]["UOMCode"], dtGrid.Rows[m]["OrderQty"], dtGrid.Rows[m]["DispatchQty"], dtGrid.Rows[m]["BalQty"], dtGrid.Rows[m]["OrderWeight"], dtGrid.Rows[m]["DispatchWeight"], dtGrid.Rows[m]["BalWeight"], dtGrid.Rows[m]["OrderAmount"], dtGrid.Rows[m]["DispatchAmount"], dtGrid.Rows[m]["BalAmount"], dtGrid.Rows[m]["AvgRate"], dtGrid.Rows[m]["PrcntOfTotal"], dtGrid.Rows[m]["CityName"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByItemNPackSizeCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
							DataGridHistory.RootTable.Columns["ItemCode"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomer";
							dtSalesSummaryByCustomer.Rows.Clear();
							for (int n = 0; n < dtGrid.Rows.Count; n++)
							{
								dtSalesSummaryByCustomer.Rows.Add(dtGrid.Rows[n]["Id"], dtGrid.Rows[n]["Customer"], dtGrid.Rows[n]["OrderQty"], dtGrid.Rows[n]["DispatchQty"], dtGrid.Rows[n]["BalQty"], dtGrid.Rows[n]["OrderWeight"], dtGrid.Rows[n]["DispatchWeight"], dtGrid.Rows[n]["BalWeight"], dtGrid.Rows[n]["OrderAmount"], dtGrid.Rows[n]["DispatchAmount"], dtGrid.Rows[n]["BalAmount"], dtGrid.Rows[n]["AvgRate"], dtGrid.Rows[n]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByCustomer;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["PartyName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Item")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerAndItem";
							dtSalesSummaryByCustomerandItem.Rows.Clear();
							for (int i2 = 0; i2 < dtGrid.Rows.Count; i2++)
							{
								dtSalesSummaryByCustomerandItem.Rows.Add(dtGrid.Rows[i2]["Id"], dtGrid.Rows[i2]["Customer"], dtGrid.Rows[i2]["ItemCode"], dtGrid.Rows[i2]["ItemName"], dtGrid.Rows[i2]["OrderQty"], dtGrid.Rows[i2]["DispatchQty"], dtGrid.Rows[i2]["BalQty"], dtGrid.Rows[i2]["OrderWeight"], dtGrid.Rows[i2]["DispatchWeight"], dtGrid.Rows[i2]["BalWeight"], dtGrid.Rows[i2]["OrderAmount"], dtGrid.Rows[i2]["DispatchAmount"], dtGrid.Rows[i2]["BalAmount"], dtGrid.Rows[i2]["AvgRate"], dtGrid.Rows[i2]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByCustomerandItem;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["PartyName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemCode"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & City")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerAndCity";
							dtSalesSummaryByCustomerandCity.Rows.Clear();
							for (int i3 = 0; i3 < dtGrid.Rows.Count; i3++)
							{
								dtSalesSummaryByCustomerandCity.Rows.Add(dtGrid.Rows[i3]["Id"], dtGrid.Rows[i3]["Customer"], dtGrid.Rows[i3]["OrderQty"], dtGrid.Rows[i3]["DispatchQty"], dtGrid.Rows[i3]["BalQty"], dtGrid.Rows[i3]["OrderWeight"], dtGrid.Rows[i3]["DispatchWeight"], dtGrid.Rows[i3]["BalWeight"], dtGrid.Rows[i3]["OrderAmount"], dtGrid.Rows[i3]["DispatchAmount"], dtGrid.Rows[i3]["BalAmount"], dtGrid.Rows[i3]["AvgRate"], dtGrid.Rows[i3]["PrcntOfTotal"], dtGrid.Rows[i3]["CityName"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByCustomerandCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["PartyName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer,Item & City")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerItemAndCity";
							dtSalesSummaryByCustomerItemandCity.Rows.Clear();
							for (int i4 = 0; i4 < dtGrid.Rows.Count; i4++)
							{
								dtSalesSummaryByCustomerItemandCity.Rows.Add(dtGrid.Rows[i4]["Id"], dtGrid.Rows[i4]["Customer"], dtGrid.Rows[i4]["ItemCode"], dtGrid.Rows[i4]["ItemName"], dtGrid.Rows[i4]["OrderQty"], dtGrid.Rows[i4]["DispatchQty"], dtGrid.Rows[i4]["BalQty"], dtGrid.Rows[i4]["OrderWeight"], dtGrid.Rows[i4]["DispatchWeight"], dtGrid.Rows[i4]["BalWeight"], dtGrid.Rows[i4]["OrderAmount"], dtGrid.Rows[i4]["DispatchAmount"], dtGrid.Rows[i4]["BalAmount"], dtGrid.Rows[i4]["AvgRate"], dtGrid.Rows[i4]["PrcntOfTotal"], dtGrid.Rows[i4]["CityName"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByCustomerItemandCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["PartyName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemCode"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Pack Size")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerAndPackSize";
							dtSalesSummaryByCustomerandPackSize.Rows.Clear();
							for (int i5 = 0; i5 < dtGrid.Rows.Count; i5++)
							{
								dtSalesSummaryByCustomerandPackSize.Rows.Add(dtGrid.Rows[i5]["Id"], dtGrid.Rows[i5]["Customer"], dtGrid.Rows[i5]["UOMCode"], dtGrid.Rows[i5]["OrderQty"], dtGrid.Rows[i5]["DispatchQty"], dtGrid.Rows[i5]["BalQty"], dtGrid.Rows[i5]["OrderWeight"], dtGrid.Rows[i5]["DispatchWeight"], dtGrid.Rows[i5]["BalWeight"], dtGrid.Rows[i5]["OrderAmount"], dtGrid.Rows[i5]["DispatchAmount"], dtGrid.Rows[i5]["BalAmount"], dtGrid.Rows[i5]["AvgRate"], dtGrid.Rows[i5]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByCustomerandPackSize;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
							DataGridHistory.RootTable.Columns["PartyName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer and ReferenceParty")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerandReferenceParty";
							dtSalesSummaryByCustomerandReferenceParty.Rows.Clear();
							for (int i6 = 0; i6 < dtGrid.Rows.Count; i6++)
							{
								dtSalesSummaryByCustomerandReferenceParty.Rows.Add(dtGrid.Rows[i6]["Id"], dtGrid.Rows[i6]["Customer"], dtGrid.Rows[i6]["ReferencePartyDetail"], dtGrid.Rows[i6]["UOMCode"], dtGrid.Rows[i6]["OrderQty"], dtGrid.Rows[i6]["DispatchQty"], dtGrid.Rows[i6]["BalQty"], dtGrid.Rows[i6]["OrderWeight"], dtGrid.Rows[i6]["DispatchWeight"], dtGrid.Rows[i6]["BalWeight"], dtGrid.Rows[i6]["OrderAmount"], dtGrid.Rows[i6]["DispatchAmount"], dtGrid.Rows[i6]["BalAmount"], dtGrid.Rows[i6]["AvgRate"], dtGrid.Rows[i6]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByCustomerandReferenceParty;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
							DataGridHistory.RootTable.Columns["PartyName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ReferenceParty"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Item & ReferenceParty")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByCustomerItemAndReferenceParty";
							dtSalesSummaryByCustomerItemandReferenceParty.Rows.Clear();
							for (int i7 = 0; i7 < dtGrid.Rows.Count; i7++)
							{
								dtSalesSummaryByCustomerItemandReferenceParty.Rows.Add(dtGrid.Rows[i7]["Id"], dtGrid.Rows[i7]["Customer"], dtGrid.Rows[i7]["ReferencePartyDetail"], dtGrid.Rows[i7]["itemcode"], dtGrid.Rows[i7]["ItemName"], dtGrid.Rows[i7]["UOMCode"], dtGrid.Rows[i7]["OrderQty"], dtGrid.Rows[i7]["DispatchQty"], dtGrid.Rows[i7]["BalQty"], dtGrid.Rows[i7]["OrderWeight"], dtGrid.Rows[i7]["DispatchWeight"], dtGrid.Rows[i7]["BalWeight"], dtGrid.Rows[i7]["OrderAmount"], dtGrid.Rows[i7]["DispatchAmount"], dtGrid.Rows[i7]["BalAmount"], dtGrid.Rows[i7]["AvgRate"], dtGrid.Rows[i7]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByCustomerItemandReferenceParty;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
							DataGridHistory.RootTable.Columns["PartyName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ReferenceParty"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemCode"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By ReferenceParty")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByReferenceParty";
							dtSalesSummaryByReferenceParty.Rows.Clear();
							for (int i8 = 0; i8 < dtGrid.Rows.Count; i8++)
							{
								dtSalesSummaryByReferenceParty.Rows.Add(dtGrid.Rows[i8]["Id"], dtGrid.Rows[i8]["ReferencePartyDetail"], dtGrid.Rows[i8]["UOMCode"], dtGrid.Rows[i8]["OrderQty"], dtGrid.Rows[i8]["DispatchQty"], dtGrid.Rows[i8]["BalQty"], dtGrid.Rows[i8]["OrderWeight"], dtGrid.Rows[i8]["DispatchWeight"], dtGrid.Rows[i8]["BalWeight"], dtGrid.Rows[i8]["OrderAmount"], dtGrid.Rows[i8]["DispatchAmount"], dtGrid.Rows[i8]["BalAmount"], dtGrid.Rows[i8]["AvgRate"], dtGrid.Rows[i8]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByReferenceParty;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
							DataGridHistory.RootTable.Columns["ReferenceParty"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By ReferenceParty & City")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByReferencePartyAndCity";
							dtSalesSummaryByReferencePartyAndCity.Rows.Clear();
							for (int i9 = 0; i9 < dtGrid.Rows.Count; i9++)
							{
								dtSalesSummaryByReferencePartyAndCity.Rows.Add(dtGrid.Rows[i9]["Id"], dtGrid.Rows[i9]["CityName"], dtGrid.Rows[i9]["ReferencePartyDetail"], dtGrid.Rows[i9]["UOMCode"], dtGrid.Rows[i9]["OrderQty"], dtGrid.Rows[i9]["DispatchQty"], dtGrid.Rows[i9]["BalQty"], dtGrid.Rows[i9]["OrderWeight"], dtGrid.Rows[i9]["DispatchWeight"], dtGrid.Rows[i9]["BalWeight"], dtGrid.Rows[i9]["OrderAmount"], dtGrid.Rows[i9]["DispatchAmount"], dtGrid.Rows[i9]["BalAmount"], dtGrid.Rows[i9]["AvgRate"], dtGrid.Rows[i9]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByReferencePartyAndCity;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.StatusConstant;
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
							DataGridHistory.RootTable.Columns["ReferenceParty"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By ReferenceParty & Item")
						{
							((Control)(object)DataGridHistory).Name = "OrderSummaryByReferencePartyAndItem";
							dtSalesSummaryByReferencePartyAndItem.Rows.Clear();
							for (int i10 = 0; i10 < dtGrid.Rows.Count; i10++)
							{
								dtSalesSummaryByReferencePartyAndItem.Rows.Add(dtGrid.Rows[i10]["Id"], dtGrid.Rows[i10]["ReferencePartyDetail"], dtGrid.Rows[i10]["itemcode"], dtGrid.Rows[i10]["ItemName"], dtGrid.Rows[i10]["UOMCode"], dtGrid.Rows[i10]["OrderQty"], dtGrid.Rows[i10]["DispatchQty"], dtGrid.Rows[i10]["BalQty"], dtGrid.Rows[i10]["OrderWeight"], dtGrid.Rows[i10]["DispatchWeight"], dtGrid.Rows[i10]["BalWeight"], dtGrid.Rows[i10]["OrderAmount"], dtGrid.Rows[i10]["DispatchAmount"], dtGrid.Rows[i10]["BalAmount"], dtGrid.Rows[i10]["AvgRate"], dtGrid.Rows[i10]["PrcntOfTotal"]);
							}
							DataGridHistory.DataSource = dtSalesSummaryByReferencePartyAndItem;
							DataGridHistory.RetrieveStructure();
							DataGridHistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName;
							DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
							DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
							DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
							DataGridHistory.RootTable.Columns["ReferenceParty"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemCode"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
							DataGridHistory.RootTable.Columns["PackUom"].HeaderAlignment = (TextAlignment)2;
							DataGridHistorySetting();
						}
					}
					else
					{
						DataGridHistory.DataSource = null;
					}
				}
				else
				{
					waitForm.Close();
					DataGridHistory.DataSource = null;
				}
				return;
			}
			cmbBranchNameSumm.Focus();
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

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["OrderQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["DispatchedQty"].Width = Constants.InventoryConstants.StatusConstant;
			DataGridHistory.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["OrderWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["DispatchedWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["OrderAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["DispatchedAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["AvgRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["PercentOfTotal"].Width = Constants.InventoryConstants.PercentOfTotal;
			DataGridHistory.RootTable.HeaderLines = 2;
			DataGridHistory.RootTable.Columns["OrderQty"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["DispatchedQty"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["BalQty"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["OrderWeight"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["DispatchedWeight"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["BalWeight"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["OrderAmount"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["DispatchedAmount"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["BalAmount"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["AvgRate"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["PercentOfTotal"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["PercentOfTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["PercentOfTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PercentOfTotal"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["PercentOfTotal"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["DisPatchedQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DisPatchedQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DisPatchedQty"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["DisPatchedQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BalQty"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OrderAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["OrderAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DisPatchedAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DisPatchedAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DisPatchedAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DisPatchedAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BalAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BalAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OrderWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["OrderWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DisPatchedWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DisPatchedWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DisPatchedWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DisPatchedWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BalWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AvgRate"].FormatString = "#,##0.###";
			if (((Control)(object)cmbActivitySumm).Text == "Order Register")
			{
				DataGridHistory.RootTable.Columns.Add("ApprovalDetail");
				DataGridHistory.RootTable.Columns["ApprovalDetail"].Caption = "Approval Detail";
				DataGridHistory.RootTable.Columns["ApprovalDetail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["ApprovalDetail"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["ApprovalDetail"].ButtonText = "Approval Detail";
				DataGridHistory.RootTable.Columns["ApprovalDetail"].Width = 100;
				DataGridHistory.RootTable.Columns["ApprovalDetail"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl2.SelectedIndex == 1)
		{
			txtFromDateSumm.Focus();
		}
		else if (tabControl2.SelectedIndex == 0)
		{
			fromdate.Focus();
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

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNewSummary_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDateSumm.Value = DateTime.Now.AddDays(-7.0);
			txtToDateSumm.Value = DateTime.Now;
			txtFromDocNoSumm.Clear();
			txtToDocNoSumm.Clear();
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((Control)(object)cmbItemtypeSumm).Text = string.Empty;
			((Control)(object)cmbCropSumm).Text = string.Empty;
			((Control)(object)cmbPackUomSumm).Text = string.Empty;
			((Control)(object)cmbPackingtypeSumm).Text = string.Empty;
			((Control)(object)cmbDistrictSumm).Text = string.Empty;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((Control)(object)cmbCustomerSumm).Text = string.Empty;
			((Control)(object)cmbRefPartyDetailSumm).Text = string.Empty;
			((UltraGridBase)cmbActivitySumm).Rows[0].Activate();
			txtFromDateSumm.Focus();
			DataGridHistory.ClearStructure();
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
			AllDropDownBind();
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

	private void btnPrintSummary_Click(object sender, EventArgs e)
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
			if (btnPrintSumm.Text == "347-OrderRegister")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtGrid, "347-OrderRegister.rpt");
			}
			else if (btnPrintSumm.Text == "348-OrderSummaryByItem&PackSize")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtGrid, "348-OrderSummaryByItem&PackSize.rpt");
			}
			else if (btnPrintSumm.Text == "349-OrderSummaryByItem,PackSize&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtGrid, "349-OrderSummaryByItem,PackSize&City.rpt");
			}
			else if (btnPrintSumm.Text == "350-OrderSummaryByCustomer&PackSize")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtGrid, "350-OrderSummaryByCustomer&PackSize.rpt");
			}
			else if (btnPrintSumm.Text == "351-OrderSummaryByItem")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtGrid, "351-OrderSummaryByItem.rpt");
			}
			else if (btnPrintSumm.Text == "352-OrderSummaryByItem&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtGrid, "352-OrderSummaryByItem&City.rpt");
			}
			else if (btnPrintSumm.Text == "353-OrderSummaryByCustomer")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtGrid, "353-OrderSummaryByCustomer.rpt");
			}
			else if (btnPrintSumm.Text == "354-OrderSummaryByCustomer&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtGrid, "354-OrderSummaryByCustomer&City.rpt");
			}
			else if (btnPrintSumm.Text == "355-OrderSummaryByCustomer&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtGrid, "355-OrderSummaryByCustomer&Item.rpt");
			}
			else if (btnPrintSumm.Text == "356-OrderSummaryByCustomer,Item&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtGrid, "356-OrderSummaryByCustomer,Item&City.rpt");
			}
			else if (btnPrintSumm.Text == "357-OrderSummaryByCustomer&ReferenceParty")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtGrid, "357-OrderSummaryByCustomerandReferenceParty.rpt");
			}
			else if (btnPrintSumm.Text == "358-OrderSummaryByCustomer&Item&ReferenceParty")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val12 = new Reporting();
				val12.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val12.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val12.ShowReportWithDataTable(dtGrid, "358-OrderSummaryByCustomer&Item&ReferenceParty.rpt");
			}
			else if (btnPrintSumm.Text == "359-OrderSummaryByReferenceParty")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val13 = new Reporting();
				val13.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val13.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val13.ShowReportWithDataTable(dtGrid, "359-OrderSummaryByReferenceParty.rpt");
			}
			else if (btnPrintSumm.Text == "360-OrderSummaryByReferenceParty&City")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val14 = new Reporting();
				val14.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val14.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val14.ShowReportWithDataTable(dtGrid, "360-OrderSummaryByReferenceParty&City.rpt");
			}
			else if (btnPrintSumm.Text == "361-OrderSummaryByReferenceParty&Item")
			{
				if (dtGrid.Rows.Count == 0 || dtGrid == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val15 = new Reporting();
				val15.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val15.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val15.ShowReportWithDataTable(dtGrid, "361-OrderSummaryByReferenceParty&Item.rpt");
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
			if (((UltraGridBase)cmbActivitySumm).ActiveRow != null)
			{
				btnPrintSumm.Visible = true;
				if (((Control)(object)cmbActivitySumm).Text == "Order Register")
				{
					btnPrintSumm.Text = "347-OrderRegister";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & Pack Size")
				{
					btnPrintSumm.Text = "348-OrderSummaryByItem&PackSize";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item,Pack Size & City")
				{
					btnPrintSumm.Text = "349-OrderSummaryByItem,PackSize&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Pack Size")
				{
					btnPrintSumm.Text = "350-OrderSummaryByCustomer&PackSize";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item")
				{
					btnPrintSumm.Text = "351-OrderSummaryByItem";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & City")
				{
					btnPrintSumm.Text = "352-OrderSummaryByItem&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer")
				{
					btnPrintSumm.Text = "353-OrderSummaryByCustomer";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Sales Summary By Customer & City")
				{
					btnPrintSumm.Text = "354-OrderSummaryByCustomer&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Item")
				{
					btnPrintSumm.Text = "355-OrderSummaryByCustomer&Item";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer,Item & City")
				{
					btnPrintSumm.Text = "356-OrderSummaryByCustomer,Item&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer and ReferenceParty")
				{
					btnPrintSumm.Text = "357-OrderSummaryByCustomer&ReferenceParty";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Item & ReferenceParty")
				{
					btnPrintSumm.Text = "358-OrderSummaryByCustomer&Item&ReferenceParty";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By ReferenceParty")
				{
					btnPrintSumm.Text = "359-OrderSummaryByReferenceParty";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By ReferenceParty & City")
				{
					btnPrintSumm.Text = "360-OrderSummaryByReferenceParty&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By ReferenceParty & Item")
				{
					btnPrintSumm.Text = "361-OrderSummaryByReferenceParty&Item";
				}
			}
			else
			{
				btnPrintSumm.Visible = false;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	private void frmSaleOrderHistory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnPrint271A_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst.Rows.Count == 0 || grdlst == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(grdlst, "271A-InvRptSalesOrderRegister.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
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
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null && DataGridHistory.CurrentColumn.Key == "ApprovalDetail")
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeysForDetailTab();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MakeShortCutKeysForDetailTab()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show History");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print 270");
			dt.Rows.Add("Alt+2", "For Print 271");
			dt.Rows.Add("Alt+3", "For Print 271A");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on From Date");
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

	private void btnShortCutKeySumamryTab_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeysForSummaryTab();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MakeShortCutKeysForSummaryTab()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show History");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on From Date");
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

	private void btnCompleteOrders_Click(object sender, EventArgs e)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		try
		{
			if (grdsaleorder.GetCheckedRows().Count() > 0 && GrdMain.GetCheckedRows().Count() > 0)
			{
				throw new Exception("Please Check Rows of any one grid at a time");
			}
			if (grdsaleorder.GetCheckedRows().Count() == 0 && GrdMain.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check Rows of any one grid To Proceed!");
			}
			RemarksPopUp remarksPopUp = new RemarksPopUp();
			remarksPopUp.StartPosition = FormStartPosition.CenterScreen;
			remarksPopUp.ShowDialog();
			string Remarks = remarksPopUp.txtRemarks.Text;
			if (Remarks == null || Remarks == string.Empty)
			{
				MessageBox.Show("Action Remarks Required");
			}
			else if (grdsaleorder.GetCheckedRows().Count() > 0)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdsaleorder.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.ReqType = "Status";
					item.EntryUser = UserAccount.ID;
					item.Comments = Remarks;
					obj.AllApprovalLists.Add(item);
				}
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(obj);
				MessageBox.Show("Orders Complete Successfully");
				gridHisory();
			}
			else if (GrdMain.GetCheckedRows().Count() > 0)
			{
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = GrdMain.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					ApprovalList item2 = new ApprovalList();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					item2.ReqType = "Status";
					item2.EntryUser = UserAccount.ID;
					item2.Comments = Remarks;
					obj2.AllApprovalLists.Add(item2);
				}
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(obj2);
				MessageBox.Show("Orders Complete Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelOrders_Click(object sender, EventArgs e)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		try
		{
			if (grdsaleorder.GetCheckedRows().Count() > 0 && GrdMain.GetCheckedRows().Count() > 0)
			{
				throw new Exception("Please Check Rows of any one grid at a time");
			}
			if (grdsaleorder.GetCheckedRows().Count() == 0 && GrdMain.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check Rows of any one grid To Proceed!");
			}
			RemarksPopUp remarksPopUp = new RemarksPopUp();
			remarksPopUp.StartPosition = FormStartPosition.CenterScreen;
			remarksPopUp.ShowDialog();
			string Remarks = remarksPopUp.txtRemarks.Text;
			if (Remarks == null || Remarks == string.Empty)
			{
				MessageBox.Show("Action Remarks Required");
			}
			else if (grdsaleorder.GetCheckedRows().Count() > 0)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdsaleorder.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.ReqType = "Cancel";
					item.EntryUser = UserAccount.ID;
					item.Comments = Remarks;
					obj.AllApprovalLists.Add(item);
				}
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(obj);
				MessageBox.Show("Orders Cancel Successfully");
				gridHisory();
			}
			else if (GrdMain.GetCheckedRows().Count() > 0)
			{
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = GrdMain.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					ApprovalList item2 = new ApprovalList();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					item2.ReqType = "Cancel";
					item2.EntryUser = UserAccount.ID;
					item2.Comments = Remarks;
					obj2.AllApprovalLists.Add(item2);
				}
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(obj2);
				MessageBox.Show("Orders Cancel Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchNameSumm_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchName).Text != string.Empty && ((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				AllDropDownBind();
				return;
			}
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((UltraGridBase)cmbParentcategorySumm).DataSource = null;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((UltraGridBase)CmbCategorySumm).DataSource = null;
			((Control)(object)cmbItemtypeSumm).Text = string.Empty;
			((UltraGridBase)cmbItemtypeSumm).DataSource = null;
			((Control)(object)cmbCropSumm).Text = string.Empty;
			((UltraGridBase)cmbCropSumm).DataSource = null;
			((Control)(object)cmbPackUomSumm).Text = string.Empty;
			((UltraGridBase)cmbPackUomSumm).DataSource = null;
			((Control)(object)cmbPackingtypeSumm).Text = string.Empty;
			((UltraGridBase)cmbPackingtypeSumm).DataSource = null;
			((Control)(object)cmbDistrictSumm).Text = string.Empty;
			((UltraGridBase)cmbDistrictSumm).DataSource = null;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((UltraGridBase)cmbcitySumm).DataSource = null;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			((UltraGridBase)cmbJobLotSumm).DataSource = null;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((UltraGridBase)cmbItemNameSumm).DataSource = null;
			((Control)(object)cmbCustomerSumm).Text = string.Empty;
			((UltraGridBase)cmbCustomerSumm).DataSource = null;
			((Control)(object)cmbRefPartyDetailSumm).Text = string.Empty;
			((UltraGridBase)cmbRefPartyDetailSumm).DataSource = null;
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
				AllDropDownBind();
				return;
			}
			((Control)(object)cmbRefPartyDetail).Text = string.Empty;
			((UltraGridBase)cmbRefPartyDetail).DataSource = null;
			((Control)(object)CmbParentCategory).Text = string.Empty;
			((UltraGridBase)CmbParentCategory).DataSource = null;
			((Control)(object)CmbItemCategory).Text = string.Empty;
			((UltraGridBase)CmbItemCategory).DataSource = null;
			((Control)(object)CmbBookingPerson).Text = string.Empty;
			((UltraGridBase)CmbBookingPerson).DataSource = null;
			((Control)(object)cmbBookingPersonSumm).Text = string.Empty;
			((UltraGridBase)cmbBookingPersonSumm).DataSource = null;
			((Control)(object)CmbItemType).Text = string.Empty;
			((UltraGridBase)CmbItemType).DataSource = null;
			((Control)(object)CmbItem).Text = string.Empty;
			((UltraGridBase)CmbItem).DataSource = null;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((UltraGridBase)CmbSupplier).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void fromdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (!Conversion.CheckDateTimeNull(fromdate.Value))
			{
				updateFromDate = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAutoCompleteOrder_Click(object sender, EventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		try
		{
			List<GridEXRow> MainRows = GrdMain.GetRows().ToList();
			if (MainRows.Count == 0)
			{
				throw new Exception("There is no record to Update Status");
			}
			if (MessageBox.Show("Are you sure to Auto Complete Order?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			List<GridEXRow> RowsLessThanEqualToToleranceWeight = MainRows.Where((GridEXRow row) => Conversion.ToDouble(row.Cells["BalWeight"].Value) <= ToleranceForOrderCompletion).ToList();
			if (RowsLessThanEqualToToleranceWeight.Count > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.ReportsParameterslist = new List<ReportsParameters>();
				foreach (GridEXRow item in RowsLessThanEqualToToleranceWeight)
				{
					ReportsParameters update = new ReportsParameters();
					update.OrganizationId = UserAccount.OrganizationId;
					update.CompanyId = UserAccount.CompanyId;
					update.OrderId = Conversion.ToInt(item.Cells["Id"].Value);
					update.DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
					update.NetWeight = 0.0;
					obj.ReportsParameterslist.Add(update);
				}
				SaleOrder.SaleOrderAutoComplete(obj);
				MessageBox.Show("Records Auto Complete Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				gridHisory();
				return;
			}
			throw new Exception("No record found for auto complete");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnOpen_Click(object sender, EventArgs e)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		try
		{
			if (grdsaleorder.GetCheckedRows().Count() > 0 && GrdMain.GetCheckedRows().Count() > 0)
			{
				throw new Exception("Please Check Rows of any one grid at a time");
			}
			if (grdsaleorder.GetCheckedRows().Count() == 0 && GrdMain.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check Rows of any one grid To Proceed!");
			}
			RemarksPopUp remarksPopUp = new RemarksPopUp();
			remarksPopUp.StartPosition = FormStartPosition.CenterScreen;
			remarksPopUp.ShowDialog();
			string Remarks = remarksPopUp.txtRemarks.Text;
			if (Remarks == null || Remarks == string.Empty)
			{
				MessageBox.Show("Action Remarks Required");
			}
			else if (grdsaleorder.GetCheckedRows().Count() > 0)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdsaleorder.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.ReqType = "Open";
					item.EntryUser = UserAccount.ID;
					item.Comments = Remarks;
					obj.AllApprovalLists.Add(item);
				}
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(obj);
				MessageBox.Show("Orders Open Successfully");
				gridHisory();
			}
			else if (GrdMain.GetCheckedRows().Count() > 0)
			{
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = GrdMain.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					ApprovalList item2 = new ApprovalList();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					item2.ReqType = "Open";
					item2.EntryUser = UserAccount.ID;
					item2.Comments = Remarks;
					obj2.AllApprovalLists.Add(item2);
				}
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(obj2);
				MessageBox.Show("Orders Open Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_FormattingRow(object sender, RowLoadEventArgs e)
	{
		GridFormattingRow(grdsaleorder, e);
	}

	private void GridFormattingRow(GridEX gridEX, RowLoadEventArgs e)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		if (gridEX.RowCount <= 0)
		{
			return;
		}
		GridEXColumn Closing = gridEX.RootTable.Columns["ExpiryDate"];
		GridEXColumn DeliveryDays = gridEX.RootTable.Columns["DeliveryDays"];
		if (Closing != null && DeliveryDays != null)
		{
			GridEXCell obj = e.Row.Cells[((JanusCollectionElement)Closing).Index];
			GridEXCell deliveryCell = e.Row.Cells[((JanusCollectionElement)DeliveryDays).Index];
			if (Conversion.ToDateTime(obj.Value).Date < DateTime.Now)
			{
				deliveryCell.FormatStyle = new GridEXFormatStyle();
				deliveryCell.FormatStyle.ForeColor = Color.Red;
				deliveryCell.FormatStyle.FontBold = (TriState)2;
			}
		}
	}

	private void GrdMain_FormattingRow(object sender, RowLoadEventArgs e)
	{
		GridFormattingRow(GrdMain, e);
	}

	private void DataGridHistory_FormattingRow(object sender, RowLoadEventArgs e)
	{
		if (DataGridHistory != null && DataGridHistory.RootTable != null && DataGridHistory.RootTable.Columns != null && DataGridHistory.RootTable.Columns.Contains("ExpiryDate"))
		{
			DataGridHistory.RootTable.Columns.Contains("DeliveryDays");
		}
		GridFormattingRow(DataGridHistory, e);
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
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Expected O, but got Unknown
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Expected O, but got Unknown
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Expected O, but got Unknown
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Expected O, but got Unknown
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Expected O, but got Unknown
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Expected O, but got Unknown
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Expected O, but got Unknown
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Expected O, but got Unknown
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Expected O, but got Unknown
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Expected O, but got Unknown
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Expected O, but got Unknown
		//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Expected O, but got Unknown
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Expected O, but got Unknown
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Expected O, but got Unknown
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Expected O, but got Unknown
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Expected O, but got Unknown
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Expected O, but got Unknown
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Expected O, but got Unknown
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Expected O, but got Unknown
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Expected O, but got Unknown
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Expected O, but got Unknown
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Expected O, but got Unknown
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6d: Expected O, but got Unknown
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Expected O, but got Unknown
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a83: Expected O, but got Unknown
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Expected O, but got Unknown
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Expected O, but got Unknown
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Expected O, but got Unknown
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Expected O, but got Unknown
		//IL_0ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Expected O, but got Unknown
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Expected O, but got Unknown
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Expected O, but got Unknown
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Expected O, but got Unknown
		//IL_0add: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae6: Expected O, but got Unknown
		//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af1: Expected O, but got Unknown
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Expected O, but got Unknown
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b07: Expected O, but got Unknown
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b12: Expected O, but got Unknown
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Expected O, but got Unknown
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Expected O, but got Unknown
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Expected O, but got Unknown
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Expected O, but got Unknown
		//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Expected O, but got Unknown
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Expected O, but got Unknown
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5e: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Expected O, but got Unknown
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Expected O, but got Unknown
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2c: Expected O, but got Unknown
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e58: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Expected O, but got Unknown
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e84: Expected O, but got Unknown
		//IL_0e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9a: Expected O, but got Unknown
		//IL_0eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Expected O, but got Unknown
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec6: Expected O, but got Unknown
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee7: Expected O, but got Unknown
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f08: Expected O, but got Unknown
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f13: Expected O, but got Unknown
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Expected O, but got Unknown
		//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8c: Expected O, but got Unknown
		//IL_0fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb8: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_0fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe4: Expected O, but got Unknown
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_107e: Expected O, but got Unknown
		//IL_1830: Unknown result type (might be due to invalid IL or missing references)
		//IL_183a: Expected O, but got Unknown
		//IL_1847: Unknown result type (might be due to invalid IL or missing references)
		//IL_1851: Expected O, but got Unknown
		//IL_185e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1868: Expected O, but got Unknown
		//IL_1ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1caa: Expected O, but got Unknown
		//IL_1e91: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9b: Expected O, but got Unknown
		//IL_1ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb2: Expected O, but got Unknown
		//IL_1ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee0: Expected O, but got Unknown
		//IL_665d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6667: Expected O, but got Unknown
		//IL_6674: Unknown result type (might be due to invalid IL or missing references)
		//IL_667e: Expected O, but got Unknown
		GridEXLayout grdsaleorder_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdMain_DesignTimeLayout = new GridEXLayout();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
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
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
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
		Appearance appearance293 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance306 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.frmSaleOrderHistory));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
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
		Appearance appearance333 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance345 = new Appearance();
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
		Appearance appearance346 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdsaleorder = new GridEX();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.GrdDetail = new GridEX();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.GrdMain = new GridEX();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbBookingPerson = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.ultraCombo1 = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbOrderStatus = new UltraCombo();
		this.CmbIsApproved = new UltraCombo();
		this.CmbItemType = new UltraCombo();
		this.CmbItemCategory = new UltraCombo();
		this.CmbParentCategory = new UltraCombo();
		this.cmbBranchName = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.cmbRefPartyDetail = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.chkincludeDo = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtSrFrom = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtsrTo = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint271A = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnOpenOrders = new System.Windows.Forms.ToolStripButton();
		this.btnCompleteOrders = new System.Windows.Forms.ToolStripButton();
		this.btnCancelOrders = new System.Windows.Forms.ToolStripButton();
		this.btnAutoCompleteOrder = new System.Windows.Forms.ToolStripButton();
		this.tabSummary = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.label26 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbBookingPersonSumm = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbStatusSummary = new UltraCombo();
		this.CmbApprovedStatusSummary = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.cmbBranchNameSumm = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.chkSkipZero = new System.Windows.Forms.CheckBox();
		this.cmbItemtypeSumm = new UltraCombo();
		this.cmbRefPartyDetailSumm = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbCustomerSumm = new UltraCombo();
		this.ChkDoApprovedIncludSummary = new System.Windows.Forms.CheckBox();
		this.label24 = new System.Windows.Forms.Label();
		this.cmbActivitySumm = new UltraCombo();
		this.cmbItemNameSumm = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.cmbJobLotSumm = new UltraCombo();
		this.txtFromDateSumm = new System.Windows.Forms.DateTimePicker();
		this.label32 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.cmbDistrictSumm = new UltraCombo();
		this.txtToDateSumm = new System.Windows.Forms.DateTimePicker();
		this.label31 = new System.Windows.Forms.Label();
		this.txtFromDocNoSumm = new System.Windows.Forms.TextBox();
		this.cmbcitySumm = new UltraCombo();
		this.txtToDocNoSumm = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.BtnShowSumm = new System.Windows.Forms.Button();
		this.cmbPackingtypeSumm = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.cmbParentcategorySumm = new UltraCombo();
		this.cmbPackUomSumm = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbCategorySumm = new UltraCombo();
		this.cmbCropSumm = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewSummary = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSumm = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeySumamryTab = new System.Windows.Forms.ToolStripButton();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.lblCostCenter = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.GrdMain).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingPerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefPartyDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabSummary.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBookingPersonSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatusSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovedStatusSummary).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefPartyDetailSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingtypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUomSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropSumm).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1353, 690);
		this.panel1.TabIndex = 0;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabSummary);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 28);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1353, 662);
		this.tabControl2.TabIndex = 5;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabDetail.Controls.Add(this.panel6);
		this.tabDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1345, 633);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail Register";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.tabControl1);
		this.panel6.Controls.Add(this.panel4);
		this.panel6.Controls.Add(this.panel2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1339, 627);
		this.panel6.TabIndex = 6;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 169);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1339, 458);
		this.tabControl1.TabIndex = 4;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.ctrlGrdBar3);
		this.tabPage1.Controls.Add(this.label6);
		this.tabPage1.Location = new System.Drawing.Point(4, 24);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1331, 430);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdsaleorder);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 29);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1325, 398);
		this.panel5.TabIndex = 25;
		this.grdsaleorder.AllowEdit = (InheritableBoolean)2;
		this.grdsaleorder.AlternatingColors = true;
		((ScrollableBase)this.grdsaleorder).BackColor = System.Drawing.Color.White;
		this.grdsaleorder.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdsaleorder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsaleorder.DesignTimeLayout = grdsaleorder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdsaleorder.FilterMode = (FilterMode)2;
		this.grdsaleorder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsaleorder.GroupTotals = (GroupTotals)2;
		this.grdsaleorder.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Name = "grdsaleorder";
		((ScrollableBase)this.grdsaleorder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Size = new System.Drawing.Size(1325, 398);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).TabIndex = 0;
		this.grdsaleorder.TotalRow = (InheritableBoolean)1;
		this.grdsaleorder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdsaleorder).VisualStyle = (VisualStyle)4;
		this.grdsaleorder.FormattingRow += new RowLoadEventHandler(grdsaleorder_FormattingRow);
		this.grdsaleorder.ColumnButtonClick += new ColumnActionEventHandler(grdsaleorder_ColumnButtonClick);
		this.grdsaleorder.LinkClicked += new ColumnActionEventHandler(grdsaleorder_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).KeyDown += new System.Windows.Forms.KeyEventHandler(grdsaleorder_KeyDown);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1287, 5);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdsaleorder;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(1325, 26);
		this.label6.TabIndex = 1;
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage2.Controls.Add(this.ctrlGrdBar7);
		this.tabPage2.Controls.Add(this.ctrlGrdBar6);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDetail);
		this.tabPage2.Controls.Add(this.label13);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdMain);
		this.tabPage2.Controls.Add(this.label12);
		this.tabPage2.Controls.Add(this.ctrlGrdBar4);
		this.tabPage2.Controls.Add(this.ctrlGrdBar2);
		this.tabPage2.Location = new System.Drawing.Point(4, 24);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1331, 430);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Brief";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1289, 244);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(6);
		this.ctrlGrdBar7.MyGrid = this.GrdDetail;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 32;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.GrdDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdDetail.AlternatingColors = true;
		((ScrollableBase)this.GrdDetail).BackColor = System.Drawing.Color.White;
		this.GrdDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GrdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDetail.DesignTimeLayout = GrdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdDetail.FilterMode = (FilterMode)2;
		this.GrdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDetail.GroupTotals = (GroupTotals)2;
		this.GrdDetail.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Location = new System.Drawing.Point(3, 265);
		((System.Windows.Forms.Control)(object)this.GrdDetail).Name = "GrdDetail";
		((ScrollableBase)this.GrdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Size = new System.Drawing.Size(1325, 162);
		((System.Windows.Forms.Control)(object)this.GrdDetail).TabIndex = 1;
		this.GrdDetail.TotalRow = (InheritableBoolean)1;
		this.GrdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDetail).VisualStyle = (VisualStyle)4;
		this.GrdDetail.LinkClicked += new ColumnActionEventHandler(GrdDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdDetail_KeyDown);
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1288, 5);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar6.MyGrid = this.GrdMain;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar6.TabIndex = 30;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.GrdMain.AllowEdit = (InheritableBoolean)2;
		this.GrdMain.AlternatingColors = true;
		((ScrollableBase)this.GrdMain).BackColor = System.Drawing.Color.White;
		this.GrdMain.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GrdMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdMain.DesignTimeLayout = GrdMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdMain).Dock = System.Windows.Forms.DockStyle.Top;
		this.GrdMain.FilterMode = (FilterMode)2;
		this.GrdMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdMain.GroupTotals = (GroupTotals)2;
		this.GrdMain.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GrdMain).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.GrdMain).Name = "GrdMain";
		((ScrollableBase)this.GrdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdMain).Size = new System.Drawing.Size(1325, 213);
		((System.Windows.Forms.Control)(object)this.GrdMain).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.GrdMain).Tag = "MasterGrid";
		this.GrdMain.TotalRow = (InheritableBoolean)1;
		this.GrdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdMain).VisualStyle = (VisualStyle)4;
		this.GrdMain.FormattingRow += new RowLoadEventHandler(GrdMain_FormattingRow);
		this.GrdMain.ColumnButtonClick += new ColumnActionEventHandler(GrdMain_ColumnButtonClick);
		this.GrdMain.SelectionChanged += new System.EventHandler(GrdMain_SelectionChanged);
		this.GrdMain.LinkClicked += new ColumnActionEventHandler(GrdMain_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdMain).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdMain_KeyDown);
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 242);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(1325, 23);
		this.label13.TabIndex = 27;
		this.label13.Text = "Detail Grid";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Dock = System.Windows.Forms.DockStyle.Top;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(3, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(1325, 26);
		this.label12.TabIndex = 2;
		this.label12.Text = "Main Grid";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar4.MyGrid = null;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(150, 150);
		this.ctrlGrdBar4.TabIndex = 33;
		this.ctrlGrdBar2.FormName = null;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(150, 150);
		this.ctrlGrdBar2.TabIndex = 31;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1339, 139);
		this.panel4.TabIndex = 1;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBookingPerson);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.ultraCombo1);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderStatus);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIsApproved);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRefPartyDetail);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.chkincludeDo);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.ToDate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtSrFrom);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtsrTo);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(5, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1151, 134);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter Criteria";
		this.CmbBookingPerson.AllowNull = (DefaultableBoolean)1;
		this.CmbBookingPerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBookingPerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBookingPerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance307).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance308).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance308).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance308;
		((AppearanceBase)appearance309).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance309;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance310).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance310).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance312;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance313;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance314).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance314;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance315).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance315).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance315).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance315).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance315;
		((AppearanceBase)appearance317).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBookingPerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Location = new System.Drawing.Point(717, 45);
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Name = "CmbBookingPerson";
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).TabIndex = 11;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(623, 51);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(90, 15);
		this.label35.TabIndex = 153;
		this.label35.Text = "Booking Person";
		this.ultraCombo1.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(717, 73);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 12;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(623, 79);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(62, 15);
		this.label3.TabIndex = 151;
		this.label3.Text = "City Name";
		this.CmbOrderStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance254;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Location = new System.Drawing.Point(956, 17);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Name = "CmbOrderStatus";
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).TabIndex = 14;
		this.CmbIsApproved.AllowNull = (DefaultableBoolean)1;
		this.CmbIsApproved.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIsApproved.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIsApproved.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Appearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance267;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance273).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbIsApproved.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Location = new System.Drawing.Point(717, 100);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Name = "CmbIsApproved";
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).TabIndex = 13;
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(412, 45);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 7;
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance268).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(412, 17);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 6;
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance281).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance281).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance282;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance284).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance284).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance285).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance288).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance289).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance289).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance289).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance290).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(97, 100);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 5;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(96, 17);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(4, 23);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(79, 15);
		this.label48.TabIndex = 149;
		this.label48.Text = "Branch Name";
		this.cmbRefPartyDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbRefPartyDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRefPartyDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRefPartyDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRefPartyDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRefPartyDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetail).Location = new System.Drawing.Point(717, 17);
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetail).Name = "cmbRefPartyDetail";
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetail).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetail).TabIndex = 10;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(317, 106);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 123;
		this.label1.Text = "Customer Name";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(623, 23);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(88, 15);
		this.label15.TabIndex = 143;
		this.label15.Text = "Ref Party Detail";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(412, 100);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 9;
		this.chkincludeDo.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
		this.chkincludeDo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkincludeDo.Location = new System.Drawing.Point(1085, 12);
		this.chkincludeDo.Name = "chkincludeDo";
		this.chkincludeDo.Size = new System.Drawing.Size(64, 64);
		this.chkincludeDo.TabIndex = 15;
		this.chkincludeDo.Text = "Include approved Do";
		this.chkincludeDo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.chkincludeDo.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(317, 79);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 125;
		this.label2.Text = "Item Name";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.label11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label11.Location = new System.Drawing.Point(623, 106);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(61, 15);
		this.label11.TabIndex = 141;
		this.label11.Text = "       Status";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance295).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance295).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance295;
		((AppearanceBase)appearance296).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance296;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance297).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance297).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance301).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance302).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance302).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance302).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance303).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(412, 73);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 8;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(4, 51);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(79, 15);
		this.label33.TabIndex = 127;
		this.label33.Text = "From,To Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(911, 23);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(40, 15);
		this.label10.TabIndex = 140;
		this.label10.Text = "Status";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(97, 47);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(107, 22);
		this.fromdate.TabIndex = 1;
		this.fromdate.ValueChanged += new System.EventHandler(fromdate_ValueChanged);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(317, 51);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(60, 15);
		this.label9.TabIndex = 138;
		this.label9.Text = "Item Type";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(205, 47);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(107, 22);
		this.ToDate.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 79);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 15);
		this.label4.TabIndex = 130;
		this.label4.Text = "So From && To";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(317, 23);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 138;
		this.label8.Text = "Item Category";
		this.txtSrFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrFrom.Location = new System.Drawing.Point(97, 75);
		this.txtSrFrom.Name = "txtSrFrom";
		this.txtSrFrom.Size = new System.Drawing.Size(107, 23);
		this.txtSrFrom.TabIndex = 3;
		this.txtSrFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSrFrom_KeyPress);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 106);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(91, 15);
		this.label7.TabIndex = 138;
		this.label7.Text = "Parent Category";
		this.txtsrTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsrTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsrTo.Location = new System.Drawing.Point(205, 75);
		this.txtsrTo.Name = "txtsrTo";
		this.txtsrTo.Size = new System.Drawing.Size(107, 23);
		this.txtsrTo.TabIndex = 4;
		this.txtsrTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsrTo_KeyPress);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1023, 45);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(57, 26);
		this.btnshow.TabIndex = 16;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click_1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1339, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnNew, this.btnRefresh, this.print, this.toolStripButton2, this.btnPrint271A, this.toolStripButton1, this.btnShortCutKeys, this.toolStripSeparator1, this.btnOpenOrders, this.btnCompleteOrders,
			this.btnCancelOrders, this.btnAutoCompleteOrder
		});
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1339, 30);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 27);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 27);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.print.Image = (System.Drawing.Image)resources.GetObject("print.Image");
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(152, 27);
		this.print.Text = "270-RegisterDetail";
		this.print.Click += new System.EventHandler(print_Click);
		this.toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(175, 27);
		this.toolStripButton2.Text = "271-RegisterSummery";
		this.toolStripButton2.Click += new System.EventHandler(toolStripButton2_Click);
		this.btnPrint271A.Image = (System.Drawing.Image)resources.GetObject("btnPrint271A.Image");
		this.btnPrint271A.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint271A.Name = "btnPrint271A";
		this.btnPrint271A.Size = new System.Drawing.Size(184, 27);
		this.btnPrint271A.Text = "271A-RegisterSummery";
		this.btnPrint271A.ToolTipText = "271A-RegisterSummery";
		this.btnPrint271A.Click += new System.EventHandler(btnPrint271A_Click);
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(184, 27);
		this.toolStripButton1.Text = "272-PrintWithGdnDetail";
		this.toolStripButton1.Visible = false;
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(126, 27);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
		this.btnOpenOrders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnOpenOrders.Image = (System.Drawing.Image)resources.GetObject("btnOpenOrders.Image");
		this.btnOpenOrders.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnOpenOrders.Name = "btnOpenOrders";
		this.btnOpenOrders.Size = new System.Drawing.Size(45, 27);
		this.btnOpenOrders.Text = "Open";
		this.btnOpenOrders.Visible = false;
		this.btnOpenOrders.Click += new System.EventHandler(btnOpen_Click);
		this.btnCompleteOrders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCompleteOrders.Image = (System.Drawing.Image)resources.GetObject("btnCompleteOrders.Image");
		this.btnCompleteOrders.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCompleteOrders.Name = "btnCompleteOrders";
		this.btnCompleteOrders.Size = new System.Drawing.Size(72, 27);
		this.btnCompleteOrders.Text = "Complete";
		this.btnCompleteOrders.Visible = false;
		this.btnCompleteOrders.Click += new System.EventHandler(btnCompleteOrders_Click);
		this.btnCancelOrders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCancelOrders.Image = (System.Drawing.Image)resources.GetObject("btnCancelOrders.Image");
		this.btnCancelOrders.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCancelOrders.Name = "btnCancelOrders";
		this.btnCancelOrders.Size = new System.Drawing.Size(55, 27);
		this.btnCancelOrders.Text = "Cancel";
		this.btnCancelOrders.Visible = false;
		this.btnCancelOrders.Click += new System.EventHandler(btnCancelOrders_Click);
		this.btnAutoCompleteOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnAutoCompleteOrder.Image = (System.Drawing.Image)resources.GetObject("btnAutoCompleteOrder.Image");
		this.btnAutoCompleteOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAutoCompleteOrder.Name = "btnAutoCompleteOrder";
		this.btnAutoCompleteOrder.Size = new System.Drawing.Size(149, 27);
		this.btnAutoCompleteOrder.Text = "Auto Complete Order";
		this.btnAutoCompleteOrder.Visible = false;
		this.btnAutoCompleteOrder.Click += new System.EventHandler(btnAutoCompleteOrder_Click);
		this.tabSummary.Controls.Add(this.panel9);
		this.tabSummary.Controls.Add(this.panel7);
		this.tabSummary.Controls.Add(this.panel8);
		this.tabSummary.Location = new System.Drawing.Point(4, 25);
		this.tabSummary.Name = "tabSummary";
		this.tabSummary.Padding = new System.Windows.Forms.Padding(3);
		this.tabSummary.Size = new System.Drawing.Size(1345, 633);
		this.tabSummary.TabIndex = 1;
		this.tabSummary.Text = "Summary";
		this.tabSummary.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.ctrlGrdBar5);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel9.Controls.Add(this.label26);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 230);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1339, 400);
		this.panel9.TabIndex = 4;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1292, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.AlternatingColors = true;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)8;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		this.DataGridHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1339, 374);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.FormattingRow += new RowLoadEventHandler(DataGridHistory_FormattingRow);
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.label26.BackColor = System.Drawing.Color.Teal;
		this.label26.Dock = System.Windows.Forms.DockStyle.Top;
		this.label26.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(0, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(1339, 26);
		this.label26.TabIndex = 3;
		this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 33);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1339, 197);
		this.panel7.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatusSummary);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbApprovedStatusSummary);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.chkSkipZero);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerSumm);
		this.groupBox2.Controls.Add(this.ChkDoApprovedIncludSummary);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivitySumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemNameSumm);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLotSumm);
		this.groupBox2.Controls.Add(this.txtFromDateSumm);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDistrictSumm);
		this.groupBox2.Controls.Add(this.txtToDateSumm);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.txtFromDocNoSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcitySumm);
		this.groupBox2.Controls.Add(this.txtToDocNoSumm);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.BtnShowSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingtypeSumm);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUomSumm);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategorySumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropSumm);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1330, 189);
		this.groupBox2.TabIndex = 159;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Criteria's";
		this.cmbBookingPersonSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbBookingPersonSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBookingPersonSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBookingPersonSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBookingPersonSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).Location = new System.Drawing.Point(624, 73);
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).Name = "cmbBookingPersonSumm";
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).TabIndex = 16;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(528, 79);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(90, 15);
		this.label36.TabIndex = 166;
		this.label36.Text = "Booking Person";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(528, 160);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(95, 15);
		this.label20.TabIndex = 164;
		this.label20.Text = "Approved Status";
		this.CmbStatusSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbStatusSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatusSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatusSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatusSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatusSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatusSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatusSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatusSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatusSummary).Location = new System.Drawing.Point(889, 19);
		((System.Windows.Forms.Control)(object)this.CmbStatusSummary).Name = "CmbStatusSummary";
		((System.Windows.Forms.Control)(object)this.CmbStatusSummary).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatusSummary).TabIndex = 20;
		this.CmbApprovedStatusSummary.AllowNull = (DefaultableBoolean)1;
		this.CmbApprovedStatusSummary.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbApprovedStatusSummary.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApprovedStatusSummary.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance241;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance247).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApprovedStatusSummary).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApprovedStatusSummary).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbApprovedStatusSummary.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApprovedStatusSummary).Location = new System.Drawing.Point(624, 154);
		((System.Windows.Forms.Control)(object)this.CmbApprovedStatusSummary).Name = "CmbApprovedStatusSummary";
		((System.Windows.Forms.Control)(object)this.CmbApprovedStatusSummary).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbApprovedStatusSummary).TabIndex = 19;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(845, 25);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(40, 15);
		this.label34.TabIndex = 163;
		this.label34.Text = "Status";
		this.cmbBranchNameSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance320).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance321).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance321).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance321).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance321;
		((AppearanceBase)appearance322).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance322;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance323).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance323).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance323).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance323;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance324;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance325).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance325;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance326;
		((AppearanceBase)appearance328).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance328).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance329).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance329).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance329).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance329;
		((AppearanceBase)appearance330).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance330;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance331).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance331).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Location = new System.Drawing.Point(112, 19);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Name = "cmbBranchNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Leave += new System.EventHandler(cmbBranchNameSumm_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(7, 25);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(79, 15);
		this.label5.TabIndex = 160;
		this.label5.Text = "Branch Name";
		this.chkSkipZero.AutoSize = true;
		this.chkSkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipZero.Location = new System.Drawing.Point(889, 50);
		this.chkSkipZero.Name = "chkSkipZero";
		this.chkSkipZero.Size = new System.Drawing.Size(76, 19);
		this.chkSkipZero.TabIndex = 21;
		this.chkSkipZero.Text = "Skip Zero";
		this.chkSkipZero.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Location = new System.Drawing.Point(112, 154);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Name = "cmbItemtypeSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).TabIndex = 7;
		this.cmbRefPartyDetailSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbRefPartyDetailSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRefPartyDetailSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRefPartyDetailSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance333).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance333;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance334).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance334).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance334).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance334;
		((AppearanceBase)appearance335).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance335;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance336).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance336).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance336).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance337).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance339).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance340;
		((AppearanceBase)appearance341).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance341).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance342).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance342).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance342).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance342).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance342;
		((AppearanceBase)appearance343).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance344).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance344;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance345).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRefPartyDetailSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Location = new System.Drawing.Point(624, 100);
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Name = "cmbRefPartyDetailSumm";
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).TabIndex = 17;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(528, 52);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(93, 15);
		this.label25.TabIndex = 123;
		this.label25.Text = "Customer Name";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(528, 106);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(88, 15);
		this.label19.TabIndex = 158;
		this.label19.Text = "Ref Party Detail";
		this.cmbCustomerSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomerSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Location = new System.Drawing.Point(624, 46);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Name = "cmbCustomerSumm";
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).TabIndex = 15;
		this.ChkDoApprovedIncludSummary.AutoSize = true;
		this.ChkDoApprovedIncludSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkDoApprovedIncludSummary.Location = new System.Drawing.Point(971, 50);
		this.ChkDoApprovedIncludSummary.Name = "ChkDoApprovedIncludSummary";
		this.ChkDoApprovedIncludSummary.Size = new System.Drawing.Size(140, 19);
		this.ChkDoApprovedIncludSummary.TabIndex = 22;
		this.ChkDoApprovedIncludSummary.Text = "Include Approved Do";
		this.ChkDoApprovedIncludSummary.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(528, 25);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 15);
		this.label24.TabIndex = 125;
		this.label24.Text = "Item Name";
		this.cmbActivitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbActivitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Location = new System.Drawing.Point(624, 127);
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Name = "cmbActivitySumm";
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).TabIndex = 18;
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
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Location = new System.Drawing.Point(624, 19);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Name = "cmbItemNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).TabIndex = 14;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(528, 133);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(70, 15);
		this.label14.TabIndex = 155;
		this.label14.Text = "Report Type";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(7, 52);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(91, 15);
		this.label22.TabIndex = 127;
		this.label22.Text = "From && To Date";
		this.cmbJobLotSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance346).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance346;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance347).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance347).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance347).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance347;
		((AppearanceBase)appearance348).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance348;
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
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Location = new System.Drawing.Point(414, 154);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Name = "cmbJobLotSumm";
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).TabIndex = 13;
		this.txtFromDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtFromDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateSumm.Location = new System.Drawing.Point(112, 48);
		this.txtFromDateSumm.Name = "txtFromDateSumm";
		this.txtFromDateSumm.Size = new System.Drawing.Size(107, 23);
		this.txtFromDateSumm.TabIndex = 1;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(335, 160);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(46, 15);
		this.label32.TabIndex = 153;
		this.label32.Text = "Job Lot";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(7, 79);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(104, 15);
		this.label21.TabIndex = 129;
		this.label21.Text = "From && To DocNo";
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
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Location = new System.Drawing.Point(414, 100);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Name = "cmbDistrictSumm";
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).TabIndex = 11;
		this.txtToDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtToDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateSumm.Location = new System.Drawing.Point(223, 48);
		this.txtToDateSumm.Name = "txtToDateSumm";
		this.txtToDateSumm.Size = new System.Drawing.Size(107, 23);
		this.txtToDateSumm.TabIndex = 2;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(335, 106);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(45, 15);
		this.label31.TabIndex = 151;
		this.label31.Text = "District";
		this.txtFromDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoSumm.Location = new System.Drawing.Point(112, 75);
		this.txtFromDocNoSumm.Name = "txtFromDocNoSumm";
		this.txtFromDocNoSumm.Size = new System.Drawing.Size(107, 23);
		this.txtFromDocNoSumm.TabIndex = 3;
		this.txtFromDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNo_KeyPress);
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
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Location = new System.Drawing.Point(414, 127);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Name = "cmbcitySumm";
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).TabIndex = 12;
		this.txtToDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoSumm.Location = new System.Drawing.Point(223, 75);
		this.txtToDocNoSumm.Name = "txtToDocNoSumm";
		this.txtToDocNoSumm.Size = new System.Drawing.Size(107, 23);
		this.txtToDocNoSumm.TabIndex = 4;
		this.txtToDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNo_KeyPress);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(335, 133);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(62, 15);
		this.label30.TabIndex = 149;
		this.label30.Text = "City Name";
		this.BtnShowSumm.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowSumm.FlatAppearance.BorderSize = 0;
		this.BtnShowSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowSumm.ForeColor = System.Drawing.Color.White;
		this.BtnShowSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowSumm.Location = new System.Drawing.Point(1046, 73);
		this.BtnShowSumm.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowSumm.Name = "BtnShowSumm";
		this.BtnShowSumm.Size = new System.Drawing.Size(57, 26);
		this.BtnShowSumm.TabIndex = 23;
		this.BtnShowSumm.Text = "Show";
		this.BtnShowSumm.UseVisualStyleBackColor = false;
		this.BtnShowSumm.Click += new System.EventHandler(BtnShowSumm_Click);
		this.cmbPackingtypeSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingtypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingtypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingtypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingtypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingtypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingtypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingtypeSumm).Location = new System.Drawing.Point(414, 73);
		((System.Windows.Forms.Control)(object)this.cmbPackingtypeSumm).Name = "cmbPackingtypeSumm";
		((System.Windows.Forms.Control)(object)this.cmbPackingtypeSumm).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingtypeSumm).TabIndex = 10;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(7, 106);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(91, 15);
		this.label18.TabIndex = 138;
		this.label18.Text = "Parent Category";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(335, 79);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(77, 15);
		this.label29.TabIndex = 147;
		this.label29.Text = "Packing Type";
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
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Location = new System.Drawing.Point(112, 100);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Name = "cmbParentcategorySumm";
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).TabIndex = 5;
		this.cmbPackUomSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUomSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUomSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUomSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUomSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUomSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackUomSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUomSumm).Location = new System.Drawing.Point(414, 46);
		((System.Windows.Forms.Control)(object)this.cmbPackUomSumm).Name = "cmbPackUomSumm";
		((System.Windows.Forms.Control)(object)this.cmbPackUomSumm).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackUomSumm).TabIndex = 9;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(7, 133);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(82, 15);
		this.label17.TabIndex = 138;
		this.label17.Text = "Item Category";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(335, 52);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(61, 15);
		this.label28.TabIndex = 145;
		this.label28.Text = "Pack Uom";
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
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Location = new System.Drawing.Point(112, 127);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Name = "CmbCategorySumm";
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).TabIndex = 6;
		this.cmbCropSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbCropSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).Location = new System.Drawing.Point(414, 19);
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).Name = "cmbCropSumm";
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).TabIndex = 8;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(7, 160);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 138;
		this.label16.Text = "Item Type";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(335, 25);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(57, 15);
		this.label27.TabIndex = 143;
		this.label27.Text = "Crop Year";
		this.panel8.Controls.Add(this.toolStrip1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1339, 30);
		this.panel8.TabIndex = 2;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNewSummary, this.btnRefreshSummary, this.btnPrintSumm, this.btnShortCutKeySumamryTab });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1339, 30);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewSummary.Image = (System.Drawing.Image)resources.GetObject("btnNewSummary.Image");
		this.btnNewSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewSummary.Name = "btnNewSummary";
		this.btnNewSummary.Size = new System.Drawing.Size(68, 27);
		this.btnNewSummary.Text = "&Reset";
		this.btnNewSummary.Click += new System.EventHandler(btnNewSummary_Click);
		this.btnRefreshSummary.Image = (System.Drawing.Image)resources.GetObject("btnRefreshSummary.Image");
		this.btnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummary.Name = "btnRefreshSummary";
		this.btnRefreshSummary.Size = new System.Drawing.Size(80, 27);
		this.btnRefreshSummary.Text = "&Refresh";
		this.btnRefreshSummary.Click += new System.EventHandler(btnRefreshSummary_Click);
		this.btnPrintSumm.Image = (System.Drawing.Image)resources.GetObject("btnPrintSumm.Image");
		this.btnPrintSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSumm.Name = "btnPrintSumm";
		this.btnPrintSumm.Size = new System.Drawing.Size(61, 27);
		this.btnPrintSumm.Text = "Print";
		this.btnPrintSumm.Click += new System.EventHandler(btnPrintSummary_Click);
		this.btnShortCutKeySumamryTab.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeySumamryTab.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeySumamryTab.Name = "btnShortCutKeySumamryTab";
		this.btnShortCutKeySumamryTab.Size = new System.Drawing.Size(126, 27);
		this.btnShortCutKeySumamryTab.Text = "ShortCut Keys";
		this.btnShortCutKeySumamryTab.Click += new System.EventHandler(btnShortCutKeySumamryTab_Click);
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.panel3.Controls.Add(this.lblCostCenter);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1353, 28);
		this.panel3.TabIndex = 1;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance283).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance283).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance283).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance294).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance305).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance316;
		((AppearanceBase)appearance327).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance327).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance338).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance338).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance338).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance338).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance338).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance338;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(255, 1);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(280, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 137;
		this.lblCostCenter.AutoSize = true;
		this.lblCostCenter.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCostCenter.ForeColor = System.Drawing.Color.White;
		this.lblCostCenter.Location = new System.Drawing.Point(178, 7);
		this.lblCostCenter.Name = "lblCostCenter";
		this.lblCostCenter.Size = new System.Drawing.Size(72, 15);
		this.lblCostCenter.TabIndex = 138;
		this.lblCostCenter.Text = "Cost Center";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(156, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Sale Order Report";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1353, 690);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmSaleOrderHistory";
		this.Text = "frmSaleOrderHistory";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSaleOrderHistory_FormClosing);
		base.Load += new System.EventHandler(frmSaleOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleOrderHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).EndInit();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.GrdMain).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingPerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefPartyDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabSummary.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBookingPersonSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatusSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovedStatusSummary).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefPartyDetailSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingtypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUomSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropSumm).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		base.ResumeLayout(false);
	}
}
