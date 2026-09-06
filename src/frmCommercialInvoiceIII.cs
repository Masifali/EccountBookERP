using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Main;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
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
namespace Architecture.WinApp.Export
{

public class frmCommercialInvoiceIII : Form
{
	private DataTable dtViewAttachments = new DataTable();

	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtPaymnentDetail = new DataTable();

	private DataTable dtOtherChargesDetail = new DataTable();

	private DataTable dtPackingListDetail = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtdetailCustom = new DataTable();

	private DataTable dtPaymnentDetailCustom = new DataTable();

	private DataTable dtOtherChargesDetailCustom = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtInvoiceNo = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtjob = new DataTable();

	private DataTable dtpcktype = new DataTable();

	private DataTable dtBrand = new DataTable();

	private DataTable dtFinancialInstruments = new DataTable();

	private DataTable dtPMItem = new DataTable();

	private DataTable dtBrandForPMItem = new DataTable();

	private bool FormInitialized;

	private string ScreenName = "frmCommercialInvoiceIII";

	private int DocumentTypeId = 204;

	private int RecId;

	private bool UpdateMode;

	private int updateDetailIndexPT = -1;

	private int updateDetailIndexOtherCharges = -1;

	private int updateDetailIndexPackList = -1;

	private int updateDetailIndexOtherItem = -1;

	private int updateDetailIndexOtherChargesCustom = -1;

	private int updateDetailIndexPTCustom = -1;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool AllItemsBindonExportContract;

	private bool ItemSearchWithNameOrCode;

	private bool RateAddLessOnCommercialInvoice;

	private bool AllowExportMultiCompanies;

	private LoadSalesContractForInvoice Loader;

	private List<ExImInvoicePackingDetail> LstRemoveRecordDetail = new List<ExImInvoicePackingDetail>();

	private List<ExImInvoicePackingDetailCustom> LstRemoveRecordDetailCustom = new List<ExImInvoicePackingDetailCustom>();

	private IContainer components;

	private Panel panel4;

	private Panel panel5;

	private Panel panel1;

	private Panel panel7;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private GroupBox HeaderGropBox;

	private Label label28;

	internal UltraCombo CmbCustomer;

	private Label label21;

	private Label label3;

	private DateTimePicker txtDocDate;

	private TextBox txtdocno;

	private Label label5;

	private Label label1;

	private Label label43;

	internal GridEX grdDetail;

	private Panel panel10;

	private Label label23;

	private Panel panel12;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TabPage tabPage2;

	private Panel panel16;

	private ToolStrip toolStrip3;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo cmbFCurrencr;

	internal UltraCombo combranches;

	internal UltraCombo comproject;

	internal UltraCombo ultraCombo3;

	internal UltraCombo ultraCombo2;

	internal UltraCombo ultraCombo1;

	internal UltraCombo cmbEFormNo;

	private ToolStrip miniToolStrip;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton toolStripButton4;

	private ToolStripButton toolStripButton5;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip2;

	private CtrlGrdBar ctrlDataGridHistory;

	private Label label4;

	internal UltraCombo CmbNotifyParty2;

	private Label label10;

	internal UltraCombo CmbNotifyParty1;

	private Label label8;

	private Label label7;

	private TextBox txtLotRefHeader;

	private Label label9;

	private Label label12;

	internal UltraCombo CmbExporterBank;

	internal UltraCombo CmbImporterBank;

	internal UltraCombo CmbDeliveryTerm;

	private Label label24;

	private Label label26;

	private Label label56;

	internal UltraCombo CmbDestinationPort;

	internal UltraCombo CmbLoadingPort;

	private Label label16;

	private Label label41;

	private Label label47;

	internal UltraCombo CmbCareierType;

	private Label label22;

	private TextBox txtFcyAmount;

	private Label label25;

	private Label label27;

	internal UltraCombo CmbFcyCode;

	private TextBox txtLocalAmt;

	private Label label42;

	private TextBox txtExRate;

	private Label label44;

	private Label label45;

	private DateTimePicker txtGdDate;

	private TextBox txtGdNo;

	private Label label49;

	private Label label46;

	private TextBox txtGrossWeight;

	private Label label54;

	private TextBox txtNetWeight;

	private Label label55;

	private TextBox txtNoOfContainer;

	private Label label57;

	private Label label53;

	private Label label50;

	private Label label31;

	private Label label60;

	private Label label62;

	private Label label61;

	private TextBox txtCertificateOfOrigion;

	private TextBox txtCertificate1;

	private TextBox txtTotalNetAmount;

	private Label label66;

	private TextBox txtaddlessamount;

	private Label label64;

	private Label label40;

	private TextBox txtaddlesscommnets;

	private Label label67;

	private Label label68;

	internal GridEX gridDetailHistory;

	private CheckBox ChkPrintPreview;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private CtrlGrdBar ctrlgrdDetail;

	private Panel panel22;

	private Panel panel24;

	private Panel panel23;

	private Panel panel17;

	private Panel panel11;

	private TabControl tabControl2;

	private TabPage tabItemDetail;

	private TabPage tabOtherItem;

	private Panel panel19;

	internal GridEX grdotheritems;

	private Panel panel18;

	private Button BtnCancelOIDetail;

	private Button BtnUpdateOIDetail;

	private Button BtnAddOIDetail;

	private Label label14;

	private TextBox txtRemarksOIDetail;

	private TextBox txtQtyOIDetail;

	private Label label15;

	private TextBox txtRateOIDetail;

	private Label label63;

	private Label label70;

	private TextBox txtAmountOIDetail;

	internal UltraCombo CmbItemOIDetail;

	private Label label71;

	private TabPage tabPaymentDetail;

	private Panel panel21;

	internal GridEX grdPaymentTermDetail;

	private Panel panel20;

	private Label label82;

	private Label label81;

	private Label label80;

	private Label label79;

	private Button BtnCancelPTDetail;

	private Button BtnUpdatePTDetail;

	private Button BtnAddPTDetail;

	private TextBox txtDueDaysPTDetail;

	private Label label78;

	private TextBox txtFcyAmountPTDetail;

	private Label label77;

	internal UltraCombo CmbPaymentTermPTDetail;

	private Label label76;

	private TextBox txtBalanceFIPTDetail;

	private Label label83;

	private TextBox txtPrcntOfTotalPTDetail;

	private Label label74;

	internal UltraCombo CmbFinInstrumentsPTDetail;

	private Label label75;

	private Label label17;

	private Label label2;

	private CtrlGrdBar ctrlgrdotheritems;

	private CtrlGrdBar ctrlgrdPaymentTermDetail;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKey;

	internal UltraCombo CmbConsignee;

	private Label label86;

	private ToolStripButton BtnLoad;

	private ToolStripButton btnGenerateInvoiceNos;

	private ToolStripButton btnItemDetailLookUp;

	private Label label58;

	private TextBox txtRemarksPTDetail;

	private TextBox txtDeliveryRemarks;

	private Label label87;

	private Panel panel15;

	private Label label69;

	private Panel panel3;

	internal GridEX DataGridHistory;

	private Label label92;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label20;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	internal UltraCombo CmbExportCompany;

	private Label labelExportCompany;

	private Label label48;

	internal UltraCombo CmbInvoiceNoHistory;

	private GroupBox groupBox1;

	private RadioButton rdAll;

	private RadioButton rdReferred;

	private RadioButton rdNotReferred;

	private TextBox txtCustomerContractsNos;

	private TextBox txtDocumentaryCreditNo;

	private Label label107;

	private Label label95;

	private DateTimePicker txtDocumentaryCreditNoDate;

	private Label label114;

	internal UltraCombo CmbNotifyParty3;

	private Label label108;

	private TextBox txtTcpRegNo;

	private Label label113;

	private Label label110;

	internal UltraCombo CmbContinent;

	internal UltraCombo CmbImporterCountry;

	internal UltraCombo CmbOriginCountry;

	private Label label111;

	private Label label115;

	private Label label109;

	private TextBox txtCustomerContractsDates;

	private Label label120;

	private TextBox txtRemarks2;

	private TextBox txtRemarks1;

	private Label label118;

	private Label label117;

	private Label label116;

	private TabPage tabChargesDetail;

	private Label label129;

	internal UltraCombo CmbPlaceOfDelivery;

	private TabPage tabPackingList;

	internal GridEX grdOtherChargesDetail;

	private Panel panel6;

	private CtrlGrdBar ctrlgrdOtherChargesDetail;

	private Panel panel13;

	private Label label130;

	private TextBox txtRemarksChargeDetail;

	private Label label134;

	private Label label131;

	private Label label132;

	internal UltraCombo CmbChargesNameChargeDetail;

	private Label label136;

	private Button BtnCancelChargeDetail;

	private Button BtnUpdateChargeDetail;

	private Button BtnAddChargeDetail;

	private Label label133;

	private Label label140;

	private TextBox txtLessAmountChargeDetail;

	private TextBox txtAddAmountChargeDetail;

	internal GridEX grdPackListDetail;

	private Panel panel25;

	private CtrlGrdBar ctrlgrdPackListDetail;

	private Panel panel26;

	private Label label141;

	private TextBox txtRemarksPackListDetail;

	private Label label145;

	internal UltraCombo CmbContainerNoPackListDetail;

	private Label label148;

	private Button BtnCancelPackListDetail;

	private Button BtnUpdatePackListDetail;

	private Button BtnAddPackListDetail;

	internal UltraCombo CmbPmDetailByItemCusPackListDetail;

	internal UltraCombo CmbCommodityByItemCusPackListDetail;

	private TextBox txtPmDetailPackListDetail;

	private TextBox txtCommodityPackListDetail;

	private Label label135;

	private Label label137;

	private Label label138;

	private Label label139;

	private DateTimePicker txtPackageDatePackListDetail;

	private Label label142;

	internal UltraCombo CmbInspectionNoPackListDetail;

	private Label label144;

	private TextBox txtLotNoPackListDetail;

	private Label label146;

	private DateTimePicker txtExpiryDatePackListDetail;

	private Label label143;

	private TextBox txtNetWeightPackListDetail;

	private Label label150;

	private TextBox txtGrossWeightPackListDetail;

	private Label label149;

	private TextBox txtOuterQtyPackListDetail;

	private Label label147;

	private TextBox txtSubLotNoPackListDetail;

	private Label label151;

	private TextBox txtOtherRemarks2;

	private Label label155;

	private TextBox txtOtherRemarks1;

	private Label label154;

	internal UltraCombo CmbItemFromInvDetailPackListDetail;

	private Label label84;

	private Label label160;

	private Label label159;

	private Label label158;

	private Label label157;

	private Label label156;

	private Label label85;

	private Label label29;

	private Label label18;

	private Label label33;

	private Label label32;

	private Label label30;

	private Label label13;

	private Label label6;

	private RadioButton RadItemCodeOIDetail;

	private RadioButton RadItemNameOIDetail;

	private RadioButton RadItemCodePackListDetail;

	private RadioButton RadItemNamePackListDetail;

	private Label label11;

	private TabControl tabControl3;

	private TabPage tabDetailH;

	private Panel panel2;

	private Label label59;

	private TabPage tabPaymentDetailH;

	internal GridEX grdPaymentHistory;

	private Panel panel31;

	private Label label19;

	private TabPage tabOtherChargesH;

	internal GridEX grdOtherChargesHistory;

	private Panel panel8;

	private Label label36;

	private TabPage tabPmH;

	internal GridEX grdPackListHistory;

	private Panel panel32;

	private Label label34;

	private TabPage tabOtherItemH;

	internal GridEX grdOtherItemHistory;

	private Panel panel33;

	private Label label35;

	private Button btnDefineBankLocal;

	private Button btnDefineBankForeign;

	private ToolStripButton BtnExportCharges;

	private TabControl tabControl4;

	private TabPage tabInvoiceDetailll;

	private TabPage tabCustomDetail;

	private TabControl tabControl5;

	private TabPage tabItemDetailCustom;

	private Panel panel9;

	private Panel panel14;

	internal GridEX grdDetailCustom;

	private Panel panel27;

	private CtrlGrdBar ctrlgrdDetailCustom;

	private TabPage tabChargesDetailCustom;

	internal GridEX grdOtherChargesDetailCustom;

	private Panel panel28;

	private CtrlGrdBar ctrlgrdOtherChargesDetailCustom;

	private Panel panel29;

	private Button BtnAddChargeDetailCustom;

	private Label label37;

	private TextBox txtRemarksChargeDetailCustom;

	private Label label38;

	private Label label39;

	private Label label51;

	internal UltraCombo CmbChargesNameChargeDetailCustom;

	private Label label52;

	private Button BtnCancelChargeDetailCustom;

	private Button BtnUpdateChargeDetailCustom;

	private Label label65;

	private Label label72;

	private TextBox txtLessAmountChargeDetailCustom;

	private TextBox txtAddAmountChargeDetailCustom;

	private TabPage tabPaymentDetailCustom;

	internal GridEX grdPaymentTermDetailCustom;

	private Panel panel30;

	private CtrlGrdBar ctrlgrdPaymentTermDetailCustom;

	private Panel panel34;

	private Button BtnAddPTDetailCustom;

	private Label label88;

	private TextBox txtRemarksPTDetailCustom;

	private Label label93;

	private Label label94;

	private Label label96;

	internal UltraCombo CmbFinInstrumentsPTDetailCustom;

	private Label label97;

	private Label label98;

	private Label label99;

	internal UltraCombo CmbPaymentTermPTDetailCustom;

	private Label label100;

	private TextBox txtPrcntOfTotalPTDetailCustom;

	private Button BtnCancelPTDetailCustom;

	private Label label101;

	private Button BtnUpdatePTDetailCustom;

	private TextBox txtBalanceFIPTDetailCustom;

	private TextBox txtDueDaysPTDetailCustom;

	private Label label102;

	private Label label103;

	private TextBox txtFcyAmountPTDetailCustom;

	private TabControl tabControl6;

	private TabPage tabInvoiceDetailH;

	private TabPage tabCustomDetailH;

	private TabControl tabControl7;

	private TabPage tabDetailCustomH;

	internal GridEX gridDetailCustomHistory;

	private Panel panel35;

	private Label label104;

	private TabPage tabPaymentDetailCustomH;

	internal GridEX grdPaymentCustomHistory;

	private Panel panel36;

	private Label label105;

	private TabPage tabOtherChargesCustomH;

	internal GridEX grdOtherChargesCustomHistory;

	private Panel panel37;

	private Label label106;

	private GroupBox groupBox3;

	private TextBox txtFcyAmountHeaderCustom;

	private Label label112;

	private Label label119;

	private TextBox txtAddLessHeaderCustom;

	private TextBox txtLocalAmountHeaderCustom;

	private TextBox txtTotalFcyAmountHeaderCustom;

	private Label label121;

	private Label label122;

	private ToolStripButton btnGenerateInvoiceNo;

	internal UltraCombo cmbInvoiceNo;

	private ToolStripButton btnPackingList;

	private ToolStripButton BtnPrint521A;

	private ToolStripButton BtnPrint521B;

	private ToolStripButton BtnPrint521C;

	private ToolStripButton BtnPrint521D;

	public frmCommercialInvoiceIII(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 800);
			ScreenName = Conversion.ToString((object)ScreenName);
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtInvoiceNo.Columns.Add("Id", typeof(int));
			dtInvoiceNo.Columns.Add("InvoiceNo");
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtcity.Columns.Add("Id", typeof(int));
			dtcity.Columns.Add("CityName");
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("InventoryParentCategoriesId");
			dtItem.Columns.Add("ItemCategoryId");
			dtItem.Columns.Add("ItemCategory");
			dtItem.Columns.Add("ItemTypeId");
			dtItem.Columns.Add("ItemType");
			dtItem.Columns.Add("ProductionStageId");
			dtItem.Columns.Add("ProductionStage");
			dtBrand.Columns.Add("Id", typeof(int));
			dtBrand.Columns.Add("BrandName");
			dtBrand.Columns.Add("BrandCode");
			dtPMItem.Columns.Add("Id", typeof(int));
			dtPMItem.Columns.Add("ItemName");
			dtPMItem.Columns.Add("ItemCode");
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("CropYear");
			dtjob.Columns.Add("Id", typeof(int));
			dtjob.Columns.Add("JobLotDescription");
			dtpcktype.Columns.Add("Id", typeof(int));
			dtpcktype.Columns.Add("Description");
			dtBrandForPMItem.Columns.Add("Id", typeof(int));
			dtBrandForPMItem.Columns.Add("ItemCode");
			dtBrandForPMItem.Columns.Add("ItemName");
			dtBrandForPMItem.Columns.Add("PackingTypeId", typeof(int));
			dtBrandForPMItem.Columns.Add("PackingType", typeof(string));
			dtBrandForPMItem.Columns.Add("PackUomId", typeof(int));
			dtBrandForPMItem.Columns.Add("PackUom", typeof(string));
			dtBrandForPMItem.Columns.Add("OuterQty", typeof(double));
			dtBrandForPMItem.Columns.Add("InnerQty", typeof(double));
			dtBrandForPMItem.Columns.Add("M_Ton", typeof(double));
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("ContractId", typeof(int));
			dtdetail.Columns.Add("ContractDetailId", typeof(int));
			dtdetail.Columns.Add("ContractNo", typeof(string));
			dtdetail.Columns.Add("ContractDate", typeof(DateTime));
			dtdetail.Columns.Add("ContractNoInvoiceWise", typeof(string));
			dtdetail.Columns.Add("ContractScheduleId", typeof(int));
			dtdetail.Columns.Add("ContractSchedule", typeof(string));
			dtdetail.Columns.Add("ContractScheduleDetailId", typeof(int));
			dtdetail.Columns.Add("ItemCategoryId", typeof(int));
			dtdetail.Columns.Add("ItemCategory");
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("JobLotId", typeof(int));
			dtdetail.Columns.Add("PackingTypeId", typeof(int));
			dtdetail.Columns.Add("PmItemId", typeof(int));
			dtdetail.Columns.Add("PmItemCode");
			dtdetail.Columns.Add("PmItemName");
			dtdetail.Columns.Add("BrandId", typeof(int));
			dtdetail.Columns.Add("BrandCode");
			dtdetail.Columns.Add("BrandName");
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("OuterUomId", typeof(int));
			dtdetail.Columns.Add("OuterUom");
			dtdetail.Columns.Add("OuterEquivalent", typeof(double));
			dtdetail.Columns.Add("OuterQtyEquivalent", typeof(double));
			dtdetail.Columns.Add("OuterQty", typeof(double));
			dtdetail.Columns.Add("InnerUomId", typeof(int));
			dtdetail.Columns.Add("InnerUom");
			dtdetail.Columns.Add("InnerEquivalent", typeof(double));
			dtdetail.Columns.Add("InnerQty", typeof(double));
			dtdetail.Columns.Add("PalletQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("RateWithoutAddLess", typeof(double));
			dtdetail.Columns.Add("RateAddLess", typeof(double));
			dtdetail.Columns.Add("NetRate", typeof(double));
			dtdetail.Columns.Add("ContractRate", typeof(double));
			dtdetail.Columns.Add("RateUOMId", typeof(int));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateEquivalent", typeof(double));
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("LabInspectionId", typeof(int));
			dtdetail.Columns.Add("BuyerLotNo");
			dtdetail.Columns.Add("ProductCode");
			dtdetail.Columns.Add("BuyerHSCode");
			dtdetail.Columns.Add("HSCode");
			dtdetail.Columns.Add("CommodityDetail");
			dtdetail.Columns.Add("PackingDetail");
			dtdetail.Columns.Add("Remarks");
			dtdetail.Columns.Add("OtherItemAmount", typeof(double));
			dtdetail.Columns.Add("ScheduleFCL", typeof(double));
			dtPaymnentDetail.Columns.Add("Id");
			dtPaymnentDetail.Columns.Add("PaymentTermId");
			dtPaymnentDetail.Columns.Add("PaymentTerm");
			dtPaymnentDetail.Columns.Add("DocumentTypeId");
			dtPaymnentDetail.Columns.Add("ExImEFormRegistrationId");
			dtPaymnentDetail.Columns.Add("FinancialInstrumentNo");
			dtPaymnentDetail.Columns.Add("%OfTotal", typeof(double));
			dtPaymnentDetail.Columns.Add("FcyAmount", typeof(double));
			dtPaymnentDetail.Columns.Add("DueDays");
			dtPaymnentDetail.Columns.Add("Remarks");
			dtOtherChargesDetail.Columns.Add("Id", typeof(int));
			dtOtherChargesDetail.Columns.Add("ContractId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ContractOtherChargesDetailId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItemId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItem");
			dtOtherChargesDetail.Columns.Add("AddAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("LessAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("Remarks");
			dtPackingListDetail.Columns.Add("Id", typeof(int));
			dtPackingListDetail.Columns.Add("ContainerId", typeof(int));
			dtPackingListDetail.Columns.Add("ContainerNo", typeof(string));
			dtPackingListDetail.Columns.Add("ItemId", typeof(int));
			dtPackingListDetail.Columns.Add("ItemCode", typeof(string));
			dtPackingListDetail.Columns.Add("ItemName", typeof(string));
			dtPackingListDetail.Columns.Add("ItemCommodityDetail", typeof(string));
			dtPackingListDetail.Columns.Add("ItemPmDetail", typeof(string));
			dtPackingListDetail.Columns.Add("PackageDate", typeof(DateTime));
			dtPackingListDetail.Columns.Add("ExpiryDate", typeof(DateTime));
			dtPackingListDetail.Columns.Add("LotNo");
			dtPackingListDetail.Columns.Add("LabInspectionId", typeof(int));
			dtPackingListDetail.Columns.Add("LabInspectionNo", typeof(string));
			dtPackingListDetail.Columns.Add("SubLotNo");
			dtPackingListDetail.Columns.Add("OuterQty", typeof(double));
			dtPackingListDetail.Columns.Add("NetWeight", typeof(double));
			dtPackingListDetail.Columns.Add("GrossWeight", typeof(double));
			dtPackingListDetail.Columns.Add("Remarks", typeof(string));
			dtOtherItem.Columns.Add("ItemId");
			dtOtherItem.Columns.Add("ItemName");
			dtOtherItem.Columns.Add("Qty", typeof(double));
			dtOtherItem.Columns.Add("Rate", typeof(double));
			dtOtherItem.Columns.Add("Amount", typeof(double));
			dtOtherItem.Columns.Add("Remarks");
			dtdetailCustom = dtdetail.Clone();
			dtOtherChargesDetailCustom = dtOtherChargesDetail.Clone();
			dtOtherChargesDetailCustom.Columns.Remove("ContractId");
			dtOtherChargesDetailCustom.Columns.Remove("ContractOtherChargesDetailId");
			dtPaymnentDetailCustom = dtPaymnentDetail.Clone();
			tabControl2.TabPages.Remove(tabPackingList);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			AllowExportMultiCompanies = CommonServices.GetERPFeatureById(9);
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable historyComboData = HistoryComboDbCall();
			DataTable exportcompanyData = new DataTable();
			if (AllowExportMultiCompanies)
			{
				exportcompanyData = ExImLcOrder.GetExportCompaniesByCompany(UserAccount.OrganizationId, UserAccount.CompanyId);
			}
			string documentCode = GetDocumentCode();
			DataTable invoiceNoData = InvoiceNoDbCall();
			List<Bank> bankData = BanksDbCall();
			DataTable portData = PortsDbCall();
			DataTable deliverytermData = ExImDeliveryTerm.Getall();
			DataTable continentData = Continent.ComboBind();
			DataTable countryData = CountryDtDbCall();
			DataTable multiCurrencywithExchangeRate = MultiCurrency.GetMultiCurrencywithExchangeRate(UserAccount.CompanyId);
			PackingTypeDtFillDbCall();
			DataTable paymentTermData = PaymentTermsDbCall();
			DataTable exportchargesData = ExportCharges.ComboBind(UserAccount.OrganizationId, UserAccount.CompanyId);
			DataTable containerNoFromShipingLineBookingDetailByInvoiceId = ExImInvoice.GetContainerNoFromShipingLineBookingDetailByInvoiceId(UserAccount.OrganizationId, UserAccount.CompanyId, RecId, 0);
			GetConfigurationsFromGlobal();
			return new
			{
				HistoryComboData = historyComboData,
				ExportcompanyData = exportcompanyData,
				documentNoData = documentCode,
				InvoiceNoData = invoiceNoData,
				BankData = bankData,
				PortData = portData,
				DeliverytermData = deliverytermData,
				ContinentData = continentData,
				CountryData = countryData,
				CurrencyData = multiCurrencywithExchangeRate,
				PaymentTermData = paymentTermData,
				ExportchargesData = exportchargesData,
				ContainerData = containerNoFromShipingLineBookingDetailByInvoiceId
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnsave.Enabled = formrights.DoHaveSaveRight;
				btnupdate.Enabled = formrights.DoHaveUpdateRights;
				btnPrint.Enabled = formrights.DoHavePrintRights;
				ctrlDataGridHistory.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
				ctrlDataGridHistory.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
				ctrlDataGridHistory.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlDataGridHistory.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlDataGridHistory.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
				ctrlDataGridHistory.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
				ctrlgrdDetail.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
				ctrlgrdDetail.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
				ctrlgrdDetail.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlgrdDetail.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlgrdDetail.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
				ctrlgrdDetail.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
				ctrlgrdotheritems.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
				ctrlgrdotheritems.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
				ctrlgrdotheritems.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlgrdotheritems.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlgrdotheritems.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
				ctrlgrdotheritems.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
				if (ItemSearchWithNameOrCode)
				{
					RadItemCodePackListDetail.Checked = true;
					RadItemCodeOIDetail.Checked = true;
				}
				else
				{
					RadItemCodePackListDetail.Checked = true;
					RadItemCodeOIDetail.Checked = true;
				}
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				InvoiceNoBind(result.InvoiceNoData);
				if (AllowExportMultiCompanies)
				{
					((Control)(object)CmbExportCompany).Visible = true;
					labelExportCompany.Visible = true;
					ExportCompaniesBind(result.ExportcompanyData);
				}
				else
				{
					((Control)(object)CmbExportCompany).Visible = false;
					labelExportCompany.Visible = false;
				}
				SupplierDtFillFromGlobal();
				BindSupplierName();
				ConsigneeAgainstCustomerBind(Conversion.ToInt(CmbCustomer.Value));
				ImporterAndExportBankBind(result.BankData);
				PortsBind(result.PortData);
				CarrierTypeBind();
				DeliveryTermBind(result.DeliverytermData);
				ContinentBind(result.ContinentData);
				OriginAndImportCountryBind(result.CountryData);
				CityDtFillFromGlobalAndBind();
				MultiCurrencyBind(result.CurrencyData);
				ItemDtsFillFromGlobal();
				CropJobDtFillFromGlobalAndBind();
				BrandDtFillFromGlobal();
				PaymentTermsBind(PaymentTermsDbCall());
				FinancialInstrumentDtFillDbCall(Conversion.ToInt(CmbCustomer.Value));
				FinancialInstrumentBind();
				ChargesNameChargeDetailBind(result.ExportchargesData);
				ContainerNoFromCROByInvoiceIdBind(result.ContainerData);
				ItemNamePackListBind();
				OtherItemsBind();
				BindGrids();
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				tabControl2.SelectedIndex = 0;
				ChkPrintPreview.Checked = true;
				txtDocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.GetDataForDropDownFromExportInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("name", typeof(string));
			DataTable dtInvoice = dtCustomer.Clone();
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					string Activity = Conversion.ToString(dr["ActivityType"]);
					if (Activity == "Customer")
					{
						dtCustomer.Rows.Add(dr["Id"], dr["name"]);
					}
					else if (Activity == "Invoice")
					{
						dtInvoice.Rows.Add(dr["Id"], dr["name"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dtCustomer, "Id", "name", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNoHistory, dtInvoice, "Id", "name", "Invoice No", AllColumns: true, Conversion.ToInt(CmbInvoiceNoHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable InvoiceNoDbCall(string InvoiceNo = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			return GenerateExportInvoiceNos.GetFinalInvoiceNosForCommercialInvoice(new GenerateExportInvoiceNos
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinalInvoiceNo = InvoiceNo
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InvoiceNoBind(DataTable dt)
	{
		try
		{
			dtInvoiceNo.Clear();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtInvoiceNo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["FinalInvoiceNo"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbInvoiceNo, dtInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", AllColumns: true, Conversion.ToInt(cmbInvoiceNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			RateAddLessOnCommercialInvoice = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("RateAddLessOnCommercialInvoice"));
			AllItemsBindonExportContract = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllItemsBindonExportContract"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtdocno.Text = documentData;
	}

	private string GetDocumentCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)ExImInvoice.GenerateCode(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ExportCompaniesBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbExportCompany, dt, "Id", "CompName", "Export Company", AllColumns: true, Conversion.ToInt(CmbExportCompany.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			foreach (getGlobalAllSupplierCustomer item in clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 7 && !r.IsSubSupCust))
			{
				dtSupplier.Rows.Add(Conversion.ToInt((object)item.Id), Conversion.ToString((object)item.CompanyName), Conversion.ToString((object)item.PartyCode), Conversion.ToInt((object)item.GlAccountId), Conversion.ToInt((object)item.CityId), Conversion.ToString((object)item.CityName), Conversion.ToString((object)item.MobilePersonal));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCustomer, dtSupplier, "Id", "CompanyName", "SupplierName", AllColumns: true, Conversion.ToInt(CmbCustomer.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)CmbCustomer).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbCustomer).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConsigneeAgainstCustomerBind(int CustomerId)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CompanyName");
			dt.Columns.Add("PartyCode");
			if (CustomerId > 0)
			{
				IEnumerable<getGlobalAllSupplierCustomer> lst = clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.ParentsSupCustId == CustomerId && r.IsSubSupCust);
				if (lst != null)
				{
					foreach (getGlobalAllSupplierCustomer dr in lst)
					{
						dt.Rows.Add(Conversion.ToInt((object)dr.Id), Conversion.ToString((object)dr.CompanyName), Conversion.ToString((object)dr.PartyCode));
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbConsignee, dt, "Id", "CompanyName", "Consignee", AllColumns: true, Conversion.ToInt(CmbConsignee.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbNotifyParty1, dt, "Id", "CompanyName", "Notify Party 1", AllColumns: true, Conversion.ToInt(CmbNotifyParty1.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbNotifyParty2, dt, "Id", "CompanyName", "Notify Party 2", AllColumns: true, Conversion.ToInt(CmbNotifyParty2.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbNotifyParty3, dt, "Id", "CompanyName", "Notify Party 3", AllColumns: true, Conversion.ToInt(CmbNotifyParty3.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private List<Bank> BanksDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return Bank.GetAll(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ImporterAndExportBankBind(List<Bank> lst)
	{
		DataTable dtForeign = new DataTable();
		dtForeign.Columns.Add("Id", typeof(int));
		dtForeign.Columns.Add("BranchName");
		dtForeign.Columns.Add("BankIBANNo");
		DataTable dtHome = dtForeign.Clone();
		if (lst.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < lst.Count; i++)
		{
			if (lst[i].IsHomeland == "Foreign Country")
			{
				dtForeign.Rows.Add(lst[i].Id, lst[i].BranchName, lst[i].BankIBANNo);
			}
			else if (lst[i].IsHomeland == "Home Country")
			{
				dtHome.Rows.Add(lst[i].Id, lst[i].BranchName, lst[i].BankIBANNo);
			}
		}
		InfragisticsHelper.BindAndRetainSelection(CmbImporterBank, dtForeign, "Id", "BranchName", "Importer Bank", AllColumns: true, Conversion.ToInt(CmbImporterBank.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		InfragisticsHelper.BindAndRetainSelection(CmbExporterBank, dtHome, "Id", "BranchName", "Exporter Bank", AllColumns: true, Conversion.ToInt(CmbExporterBank.Value), insertDefaultRow: false, null, null, ActivateRow: false);
	}

	private DataTable PortsDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PortsBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbLoadingPort, dt, "Id", "PortName", "Loading Port", AllColumns: false, Conversion.ToInt(CmbLoadingPort.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDestinationPort, dt, "Id", "PortName", "Destination Port", AllColumns: false, Conversion.ToInt(CmbDestinationPort.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CarrierTypeBind()
	{
		try
		{
			DataTable dtcariertype = new DataTable();
			dtcariertype.Columns.Add("Id");
			dtcariertype.Columns.Add("Type");
			dtcariertype.Rows.Add(1, "By Sea");
			dtcariertype.Rows.Add(2, "By Air");
			dtcariertype.Rows.Add(3, "By Road");
			InfragisticsHelper.BindAndRetainSelection(CmbCareierType, dtcariertype, "Id", "Type", "Fcy Carrier Type", AllColumns: true, Conversion.ToInt(CmbCareierType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTerm, dt, "Id", "Code", "Delivery Term", AllColumns: true, Conversion.ToInt(CmbDeliveryTerm.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContinentBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbContinent, dt, "ContinentID", "ContinentName", "Continent Name", AllColumns: true, Conversion.ToInt(CmbContinent.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable CountryDtDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return country.GetAll(new Country
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OriginAndImportCountryBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbOriginCountry, dt, "Id", "Description", "Origin", AllColumns: false, Conversion.ToInt(CmbOriginCountry.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbImporterCountry, dt, "Id", "Description", "Importer Country", AllColumns: false, Conversion.ToInt(CmbImporterCountry.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalAllCity> lst = clsGlobalVariables.globalAllCities;
			DatatableHelper.PopulateDataTable(dtcity, lst, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			int Id = Conversion.ToInt(CmbPlaceOfDelivery.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPlaceOfDelivery, dtcity, "Id", "CityName", "City Name", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbFcyCode, dt, "Id", "CurrencyCode", "Fcy Code", AllColumns: true, Conversion.ToInt(CmbFcyCode.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbSupCust_Leave(object sender, EventArgs e)
	{
		try
		{
			int CustomerId = Conversion.ToInt(CmbCustomer.Value);
			FinancialInstrumentDtFillDbCall(CustomerId);
			FinancialInstrumentBind();
			if (((UltraGridBase)CmbFinInstrumentsPTDetail).ActiveRow != null)
			{
				FinancialInstrumentsBalanceBindWithDbCall();
			}
			ConsigneeAgainstCustomerBind(CustomerId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryTerm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbDeliveryTerm.Value) != 0)
			{
				string Desc = Conversion.ToString(((UltraDropDownBase)CmbDeliveryTerm).SelectedRow.Cells[2].Value);
				if (Desc != null && Desc != string.Empty)
				{
					txtDeliveryRemarks.Text = Desc;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal(int CategoryOrTypeId = 0)
	{
		try
		{
			dtItem.Clear();
			dtPMItem.Clear();
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			IEnumerable<object> dtItemAll = ((!AllItemsBindonExportContract) ? allItems.Where((getGlobalAllItems item) => item.ItemClassId == 8) : allItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId != 14 && item.ItemTypeOfTypeId != 17));
			foreach (dynamic row in dtItemAll)
			{
				int ItemCategoryId = Conversion.ToInt(row.ItemCategoryId);
				if (CategoryOrTypeId == 0 || CategoryOrTypeId == ItemCategoryId)
				{
					dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.InventoryParentCategoriesId, row.ItemCategoryId, row.ItemCategory, row.ItemTypeId, row.ItemType, row.ItemProductionStageId, row.productionStageName);
				}
			}
			foreach (getGlobalAllItems item2 in allItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId == 14))
			{
				dtPMItem.Rows.Add(item2.Id, item2.ItemName, item2.ItemCode);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private void CropJobDtFillFromGlobalAndBind()
	{
		try
		{
			dtjob.Clear();
			dtCrop.Clear();
			List<GlobalCropYear> lstCrop = clsGlobalVariables.globalCropYear;
			DatatableHelper.PopulateDataTable(dtCrop, lstCrop, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			List<GetJobLotGlIdsandName> lst = clsGlobalVariables.globalJobLot;
			DatatableHelper.PopulateDataTable(dtjob, lst, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackingTypeDtFillDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			dtpcktype.Clear();
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow dr in lst.Rows)
			{
				dtpcktype.Rows.Add(dr["Id"], dr["Description"]);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BrandDtFillFromGlobal()
	{
		try
		{
			dtBrand.Clear();
			List<getGlobalAllBrands> allItems = clsGlobalVariables.getGlobalAllBrands;
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			foreach (getGlobalAllBrands item in allItems)
			{
				dtBrand.Rows.Add(item.Id, item.BrandName, item.BrandCode);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private DataTable PaymentTermsDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PaymentTermsBind(DataTable dtterm)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("lcOrderTerm", typeof(string));
			dt.Columns.Add("Description", typeof(string));
			if (dtterm.Rows.Count > 0)
			{
				foreach (DataRow dr in dtterm.Rows)
				{
					dt.Rows.Add(Conversion.ToInt(dr["Id"]), Conversion.ToString(dr["lcOrderTerm"]), Conversion.ToString(dr["lcOrderTermDesc"]));
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTermPTDetail, dt, "Id", "lcOrderTerm", " Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTermPTDetail.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTermPTDetailCustom, dt, "Id", "lcOrderTerm", " Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTermPTDetailCustom.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FinancialInstrumentDtFillDbCall(int CustomerId)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		try
		{
			dtFinancialInstruments.Rows.Clear();
			if (dtFinancialInstruments.Columns.Count == 0)
			{
				dtFinancialInstruments.Columns.Add("Id", typeof(int));
				dtFinancialInstruments.Columns.Add("EFormNo");
				dtFinancialInstruments.Columns.Add("PaymenttermId");
				dtFinancialInstruments.Columns.Add("DocumnetTypeId");
			}
			if (CustomerId == 0)
			{
				return;
			}
			DataTable dt = ExImEFormRegistration.GetFINoForInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = CustomerId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtFinancialInstruments.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["EFormNo"], dt.Rows[i]["PaymenttermId"], dt.Rows[i]["DocumentTypeId"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FinancialInstrumentBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbFinInstrumentsPTDetail, dtFinancialInstruments, "Id", "EFormNo", "FI No", AllColumns: true, Conversion.ToInt(CmbFinInstrumentsPTDetail.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbFinInstrumentsPTDetailCustom, dtFinancialInstruments, "Id", "EFormNo", "FI No", AllColumns: true, Conversion.ToInt(CmbFinInstrumentsPTDetailCustom.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPaymentTermDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbPaymentTermPTDetail.Value) != 0)
			{
				string Desc = Conversion.ToString(((UltraDropDownBase)CmbPaymentTermPTDetail).SelectedRow.Cells[2].Value);
				if (Desc != null && Desc != string.Empty)
				{
					txtRemarksPTDetail.Text = Desc;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPaymentTermDetailCustom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbPaymentTermPTDetailCustom.Value) != 0)
			{
				string Desc = Conversion.ToString(((UltraDropDownBase)CmbPaymentTermPTDetailCustom).SelectedRow.Cells[2].Value);
				if (Desc != null && Desc != string.Empty)
				{
					txtRemarksPTDetailCustom.Text = Desc;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dcmbfino_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbFinInstrumentsPTDetail).ActiveRow != null && Conversion.ToInt(CmbFinInstrumentsPTDetail.Value) > 0)
			{
				CmbPaymentTermPTDetail.Value = Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPTDetail).SelectedRow.Cells[2].Value);
				FinancialInstrumentsBalanceBindWithDbCall();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbFinInstrumentsPTDetailCustom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbFinInstrumentsPTDetailCustom).ActiveRow != null && Conversion.ToInt(CmbFinInstrumentsPTDetailCustom.Value) > 0)
			{
				CmbPaymentTermPTDetailCustom.Value = Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPTDetailCustom).SelectedRow.Cells[2].Value);
				CustomFinancialInstrumentsBalanceBindWithDbCall();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChargesNameChargeDetailBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbChargesNameChargeDetail, dt, "ExportChargesID", "ExportChargesName", " Export Charges", AllColumns: true, Conversion.ToInt(CmbChargesNameChargeDetail.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbChargesNameChargeDetailCustom, dt, "ExportChargesID", "ExportChargesName", " Export Charges", AllColumns: true, Conversion.ToInt(CmbChargesNameChargeDetailCustom.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContainerNoFromCROByInvoiceIdBind(DataTable dt)
	{
		try
		{
			DataTable dtCont = new DataTable();
			dtCont.Columns.Add("Id", typeof(int));
			dtCont.Columns.Add("ContainerNo", typeof(string));
			dtCont.Clear();
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					dtCont.Rows.Add(dr["ContainerId"], dr["Container"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbContainerNoPackListDetail, dtCont, "Id", "ContainerNo", "Container No", AllColumns: true, Conversion.ToInt(CmbContainerNoPackListDetail.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FilldtBrandForPMItemdtForItemPackList()
	{
		dtBrandForPMItem.Clear();
		if (grdDetail.RowCount == 0)
		{
			return;
		}
		try
		{
			Dictionary<(int, int, int), (double, double)> accumulatedData = new Dictionary<(int, int, int), (double, double)>();
			Dictionary<(int, int, int), GridEXRow> rowCache = new Dictionary<(int, int, int), GridEXRow>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow row2 in rows)
			{
				int itemId = Conversion.ToInt(row2.Cells["ItemId"].Value);
				int packTypeId = Conversion.ToInt(row2.Cells["PackingTypeId"].Value);
				int packSizeId = Conversion.ToInt(row2.Cells["OuterUomId"].Value);
				double noOfBags = Conversion.ToDouble(row2.Cells["OuterQty"].Value);
				double mTon = Conversion.ToDouble(row2.Cells["Qty/M.Ton"].Value);
				(int, int, int) key2 = (itemId, packTypeId, packSizeId);
				if (accumulatedData.TryGetValue(key2, out var totals2))
				{
					accumulatedData[key2] = (totals2.Item1 + noOfBags, totals2.Item2 + mTon);
					continue;
				}
				accumulatedData[key2] = (noOfBags, mTon);
				rowCache[key2] = row2;
			}
			foreach (KeyValuePair<(int, int, int), (double, double)> kvp in accumulatedData)
			{
				(int, int, int) key = kvp.Key;
				(double, double) totals = kvp.Value;
				GridEXRow row = rowCache[key];
				double outerQtyEqv = Conversion.ToDouble(row.Cells["OuterQtyEquivalent"].Value);
				double totalEquivQty = totals.Item1 * outerQtyEqv;
				dtBrandForPMItem.Rows.Add(key.Item1, row.Cells["ItemCode"].Value?.ToString() ?? "", row.Cells["ItemName"].Value?.ToString() ?? "", key.Item2, row.Cells["PackingTypeId"].Text ?? "", key.Item3, row.Cells["OuterUomId"].Value?.ToString() ?? "", totals.Item1, totalEquivQty, totals.Item2);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemNamePackListBind()
	{
		try
		{
			FilldtBrandForPMItemdtForItemPackList();
			int Id = Conversion.ToInt(CmbItemFromInvDetailPackListDetail.Value);
			string ValueMember = ((!RadItemCodePackListDetail.Checked) ? "ItemName" : "ItemCode");
			string DisplayMember = ((!RadItemCodePackListDetail.Checked) ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemFromInvDetailPackListDetail, dtBrandForPMItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			CmbItemFromInvDetailPackListDetail_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ItemCommodityDetailDbCall(int ItemId, int SupplierCustomerId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		try
		{
			return CommodityDetailItemCustomerWise.GetRemarks(new CommodityDetailItemCustomerWise
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = ItemId,
				SupplierCustomerId = SupplierCustomerId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemCommodityDetailBind(DataTable lst)
	{
		try
		{
			DataTable dtCommo = new DataTable();
			dtCommo.Columns.Add("Id", typeof(int));
			dtCommo.Columns.Add("Remarks", typeof(string));
			DataTable dtPmDetail = dtCommo.Clone();
			if (lst.Rows.Count > 0)
			{
				foreach (DataRow dr in lst.Rows)
				{
					if (Conversion.ToInt(dr["DetailTypeId"]) == 2)
					{
						dtPmDetail.Rows.Add(dr["Id"], dr["Remarks"]);
					}
					else
					{
						dtCommo.Rows.Add(dr["Id"], dr["Remarks"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCommodityByItemCusPackListDetail, dtCommo, "Id", "Remarks", "Commodity Detail Remarks", AllColumns: true, Conversion.ToInt(CmbCommodityByItemCusPackListDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbPmDetailByItemCusPackListDetail, dtPmDetail, "Id", "Remarks", "Pm Detail Remarks", AllColumns: true, Conversion.ToInt(CmbPmDetailByItemCusPackListDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ThirdPartyNoDbCall(int ItemId)
	{
		try
		{
			return InvLabPreProductionExportLotInspectionHeader.LabPreProductionExportLotInspectionHeader_GetByItemId(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, ItemId, 0);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ThirdPartyNoBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbInspectionNoPackListDetail, dt, "Id", "InspectionNo", "Inspection No", AllColumns: true, Conversion.ToInt(CmbInspectionNoPackListDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemFromInvDetailPackListDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemFromInvDetailPackListDetail.Value);
			DataTable dtCo = new DataTable();
			DataTable dtThird = new DataTable();
			if (((UltraGridBase)CmbItemFromInvDetailPackListDetail).ActiveRow != null && Id > 0)
			{
				dtCo = ItemCommodityDetailDbCall(Id);
				dtThird = ThirdPartyNoDbCall(Id);
			}
			ItemCommodityDetailBind(dtCo);
			ThirdPartyNoBind(dtThird);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCommodityByItemCusPackListDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int num = Conversion.ToInt(CmbCommodityByItemCusPackListDetail.Value);
			string selectedText = Conversion.ToString((object)((Control)(object)CmbCommodityByItemCusPackListDetail).Text);
			if (num != 0 && !string.IsNullOrWhiteSpace(selectedText) && string.IsNullOrWhiteSpace(txtCommodityPackListDetail.Text))
			{
				txtCommodityPackListDetail.Text = selectedText;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPmDetailByItemCusPackListDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int num = Conversion.ToInt(CmbPmDetailByItemCusPackListDetail.Value);
			string selectedText = Conversion.ToString((object)((Control)(object)CmbPmDetailByItemCusPackListDetail).Text);
			if (num != 0 && !string.IsNullOrWhiteSpace(selectedText) && string.IsNullOrWhiteSpace(txtPmDetailPackListDetail.Text))
			{
				txtPmDetailPackListDetail.Text = selectedText;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemOIDetail.Value);
			string ValueMember = (RadItemNameOIDetail.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (RadItemNameOIDetail.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemOIDetail, dtPMItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void RadItemNameOIDetail_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			OtherItemsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool PaymentDetailFormValidation()
	{
		List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbPaymentTermPTDetail, "Payment Term", FormHelper.ValidType.INT),
			(txtPrcntOfTotalPTDetail, "Percent of Total", FormHelper.ValidType.Double),
			(txtFcyAmountPTDetail, "FCY Amount", FormHelper.ValidType.Double)
		};
		if (Conversion.ToInt(CmbPaymentTermPTDetail.Value) == 1)
		{
			controlsToValidate.Insert(1, ((Control)(object)CmbFinInstrumentsPTDetail, "Financial Instrument", FormHelper.ValidType.String));
		}
		return FormHelper.ValidateControls(controlsToValidate);
	}

	private bool OtherChargesFormValidation()
	{
		if (((UltraGridBase)CmbChargesNameChargeDetail).ActiveRow == null || string.IsNullOrWhiteSpace(((Control)(object)CmbChargesNameChargeDetail).Text) || Conversion.ToInt(CmbChargesNameChargeDetail.Value) == 0)
		{
			MessageBox.Show("Charges Item field is required");
			CmbChargesNameChargeDetail.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtLessAmountChargeDetail.Text) == 0.0 && Conversion.ToDouble((object)txtAddAmountChargeDetail.Text) == 0.0)
		{
			MessageBox.Show("Add Or Less Amount is required");
			txtLessAmountChargeDetail.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtLessAmountChargeDetail.Text) > 0.0 && Conversion.ToDouble((object)txtAddAmountChargeDetail.Text) > 0.0)
		{
			MessageBox.Show("You Can Only Enter 'Add Or Less Amount'");
			txtLessAmountChargeDetail.Focus();
			return false;
		}
		return true;
	}

	private bool PackingListFormValidation()
	{
		return FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbContainerNoPackListDetail, "Container No", FormHelper.ValidType.INT),
			((Control)(object)CmbItemFromInvDetailPackListDetail, "Item", FormHelper.ValidType.INT),
			(txtCommodityPackListDetail, "Commodity Detail", FormHelper.ValidType.String),
			(txtPmDetailPackListDetail, "Pm Detail", FormHelper.ValidType.String),
			(txtPackageDatePackListDetail, "Package Date", FormHelper.ValidType.DateTime),
			(txtExpiryDatePackListDetail, "Expiry Date", FormHelper.ValidType.DateTime),
			(txtOuterQtyPackListDetail, "Qty", FormHelper.ValidType.Double),
			(txtNetWeightPackListDetail, "Net Weight", FormHelper.ValidType.Double),
			(txtGrossWeightPackListDetail, "Gross Weight", FormHelper.ValidType.Double)
		});
	}

	private bool OtherItemFormValidation()
	{
		return FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbItemOIDetail, "Item", FormHelper.ValidType.INT),
			(txtQtyOIDetail, "Qty", FormHelper.ValidType.Double),
			(txtRateOIDetail, "Rate", FormHelper.ValidType.Double)
		});
	}

	private bool OtherChargesCustomFormValidation()
	{
		if (((UltraGridBase)CmbChargesNameChargeDetailCustom).ActiveRow == null || string.IsNullOrWhiteSpace(((Control)(object)CmbChargesNameChargeDetailCustom).Text) || Conversion.ToInt(CmbChargesNameChargeDetailCustom.Value) == 0)
		{
			MessageBox.Show("Charges Item field is required");
			CmbChargesNameChargeDetailCustom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtLessAmountChargeDetailCustom.Text) == 0.0 && Conversion.ToDouble((object)txtAddAmountChargeDetailCustom.Text) == 0.0)
		{
			MessageBox.Show("Add Or Less Amount is required");
			txtLessAmountChargeDetailCustom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtLessAmountChargeDetailCustom.Text) > 0.0 && Conversion.ToDouble((object)txtAddAmountChargeDetailCustom.Text) > 0.0)
		{
			MessageBox.Show("You Can Only Enter 'Add Or Less Amount'");
			txtLessAmountChargeDetailCustom.Focus();
			return false;
		}
		return true;
	}

	private bool PaymentDetailCustomFormValidation()
	{
		List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbPaymentTermPTDetailCustom, "Payment Term", FormHelper.ValidType.INT),
			(txtPrcntOfTotalPTDetailCustom, "Percent of Total", FormHelper.ValidType.Double),
			(txtFcyAmountPTDetailCustom, "FCY Amount", FormHelper.ValidType.Double)
		};
		if (Conversion.ToInt(CmbPaymentTermPTDetailCustom.Value) == 1)
		{
			controlsToValidate.Insert(1, ((Control)(object)CmbFinInstrumentsPTDetailCustom, "Financial Instrument", FormHelper.ValidType.String));
		}
		return FormHelper.ValidateControls(controlsToValidate);
	}

	private void BindGrids()
	{
		try
		{
			if (dtdetail != null && dtdetail.Columns.Count > 0)
			{
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
			}
			if (dtPaymnentDetail != null && dtPaymnentDetail.Columns.Count > 0)
			{
				grdPaymentTermDetail.DataSource = dtPaymnentDetail;
				grdPaymentTermDetail.RetrieveStructure();
				grdSettingsPaymentTerm(grdPaymentTermDetail);
			}
			if (dtOtherChargesDetail != null && dtOtherChargesDetail.Columns.Count > 0)
			{
				grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
				grdOtherChargesDetail.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesDetail);
			}
			if (dtPackingListDetail != null && dtPackingListDetail.Columns.Count > 0)
			{
				grdPackListDetail.DataSource = dtPackingListDetail;
				grdPackListDetail.RetrieveStructure();
				grdSettingsPackListDetaill(grdPackListDetail);
			}
			if (dtOtherItem != null && dtOtherItem.Columns.Count > 0)
			{
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems(grdotheritems);
			}
			if (dtdetailCustom != null && dtdetailCustom.Columns.Count > 0)
			{
				grdDetailCustom.DataSource = dtdetailCustom;
				grdDetailCustom.RetrieveStructure();
				grdSettingsDetailCustom();
			}
			if (dtPaymnentDetailCustom != null && dtPaymnentDetailCustom.Columns.Count > 0)
			{
				grdPaymentTermDetailCustom.DataSource = dtPaymnentDetailCustom;
				grdPaymentTermDetailCustom.RetrieveStructure();
				grdSettingsPaymentTerm(grdPaymentTermDetailCustom);
			}
			if (dtOtherChargesDetailCustom != null && dtOtherChargesDetailCustom.Columns.Count > 0)
			{
				grdOtherChargesDetailCustom.DataSource = dtOtherChargesDetailCustom;
				grdOtherChargesDetailCustom.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesDetailCustom);
			}
			UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetail, txtaddlessamount);
			UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: false);
			CalculateLocalAmount(isCustom: false);
			ProportionateOtherItemAmount();
			UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetailCustom, txtAddLessHeaderCustom);
			UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: true);
			CalculateLocalAmount(isCustom: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailCommonSetting(GridEX grid)
	{
		try
		{
			grid.RootTable.Columns["ContractNoInvoiceWise"].Width = Constants.ExportConstants.ContractNoConstant;
			grid.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants;
			grid.RootTable.Columns["PackingTypeId"].Width = Constants.ExportConstants.PackingTypeConstant;
			grid.RootTable.Columns["CropYearId"].Width = Constants.ExportConstants.CropYearConstant;
			grid.RootTable.Columns["JobLotId"].Width = Constants.ExportConstants.JobLotConstant;
			grid.RootTable.Columns["Qty/M.Ton"].Width = Constants.ExportConstants.QtyConstant;
			grid.RootTable.Columns["OuterQty"].Width = Constants.ExportConstants.QtyConstant;
			grid.RootTable.Columns["RateWithoutAddLess"].Width = Constants.ExportConstants.ExRateConstant + 10;
			grid.RootTable.Columns["RateAddLess"].Width = Constants.ExportConstants.AddLessConstant;
			grid.RootTable.Columns["NetRate"].Width = Constants.ExportConstants.ExRateConstant;
			grid.RootTable.Columns["ContractRate"].Width = Constants.ExportConstants.ExRateConstant;
			grid.RootTable.Columns["RateUOM"].Width = Constants.ExportConstants.UomConstant;
			grid.RootTable.Columns["Amount"].Width = Constants.ExportConstants.AmountConstant;
			grid.RootTable.Columns["HSCode"].Width = Constants.ExportConstants.HsCode;
			grid.RootTable.Columns["CommodityDetail"].Width = Constants.ExportConstants.CommodityDetail;
			grid.RootTable.Columns["PackingDetail"].Width = Constants.ExportConstants.CommodityDetail;
			grid.RootTable.Columns["OtherItemAmount"].Width = Constants.ExportConstants.AmountConstant;
			grid.RootTable.Columns["ScheduleFCL"].Width = Constants.ExportConstants.FclConstant;
			grid.RootTable.Columns["RateWithoutAddLess"].Visible = RateAddLessOnCommercialInvoice;
			grid.RootTable.Columns["RateAddLess"].Visible = RateAddLessOnCommercialInvoice;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdComboBind(GridEX grid)
	{
		try
		{
			GridEX_Helper.ComboBind(grid.RootTable.Columns["CropYearId"], dtCrop, "Id", "CropYear", "Crop Year");
			GridEX_Helper.ComboBind(grid.RootTable.Columns["PackingTypeId"], dtpcktype, "Id", "Description", "Packing Type");
			GridEX_Helper.ComboBind(grid.RootTable.Columns["JobLotId"], dtjob, "Id", "JobLotDescription", "Job Lot");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ContractId", "ContractDetailId", "ContractNoInvoiceWise", "ContractScheduleId", "ContractSchedule", "ContractScheduleDetailId", "ItemCategoryId", "ItemId", "PmItemId",
				"BrandId", "OuterUomId", "OuterEquivalent", "OuterQtyEquivalent", "InnerUomId", "InnerEquivalent", "RateUOMId", "RateEquivalent"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"CommodityDetail",
					(EditType)1
				},
				{
					"PackingDetail",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				},
				{
					"Qty/M.Ton",
					(EditType)1
				},
				{
					"OuterQty",
					(EditType)1
				},
				{
					"InnerQty",
					(EditType)1
				},
				{
					"PalletQty",
					(EditType)1
				},
				{
					"BuyerLotNo",
					(EditType)1
				},
				{
					"ProductCode",
					(EditType)1
				},
				{
					"BuyerHSCode",
					(EditType)1
				},
				{
					"HSCode",
					(EditType)1
				},
				{
					"GrossWeight",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3, hiddenColumns, editableColumns);
			grdDetailCommonSetting(grdDetail);
			grdComboBind(grdDetail);
			grdDetail.RootTable.Columns["RateAddLess"].EditType = (EditType)(RateAddLessOnCommercialInvoice ? 1 : 0);
			GridEX_Helper.AddButton(grdDetail, "Delete", "X", 20, 0);
			grdDetail.FrozenColumns = 2;
			ctrlgrdDetail_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			grdDetail.UpdateData();
			string Key = e.Column.Key;
			switch (Key)
			{
			case "Qty/M.Ton":
			case "OuterUomId":
			case "OuterUom":
			case "RateAddLess":
			case "RateUOMId":
			case "RateEquivalent":
			{
				r.BeginEdit();
				double QtyMton2 = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
				double OuterEquivalent2 = Conversion.ToDouble(r.Cells["OuterEquivalent"].Value);
				double RateWithoutAddLess = Conversion.ToDouble(r.Cells["RateWithoutAddLess"].Value);
				double RateAddLess = Conversion.ToDouble(r.Cells["RateAddLess"].Value);
				double RateEquivalent2 = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
				double NetRate = Conversion.ToDouble(r.Cells["NetRate"].Value);
				switch (Key)
				{
				case "Qty/M.Ton":
				case "OuterUomId":
				case "OuterUom":
				{
					double OuterQty2 = ((OuterEquivalent2 > 0.0) ? (QtyMton2 * 1000.0 / OuterEquivalent2) : 0.0);
					r.Cells["OuterQty"].Value = OuterQty2;
					double innerUom2 = Conversion.ToDouble(r.Cells["InnerEquivalent"].Value);
					double InnerQty2 = ((QtyMton2 > 0.0 && innerUom2 > 0.0) ? (QtyMton2 * 1000.0 / innerUom2) : 0.0);
					r.Cells["InnerQty"].Value = Math.Round(InnerQty2);
					break;
				}
				case "RateAddLess":
				case "RateUOMId":
				case "RateEquivalent":
					NetRate = RateWithoutAddLess + RateAddLess;
					r.Cells["NetRate"].Value = NetRate;
					break;
				}
				double weightKgs2 = QtyMton2 * 1000.0;
				double amount2 = NetRate / RateEquivalent2 * weightKgs2;
				r.Cells["Amount"].Value = amount2;
				r.EndEdit();
				break;
			}
			case "OuterQty":
			{
				r.BeginEdit();
				double OuterQty = Conversion.ToDouble(r.Cells["OuterQty"].Value);
				double OuterEquivalent = Conversion.ToDouble(r.Cells["OuterEquivalent"].Value);
				double RateEquivalent = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
				double num = Conversion.ToDouble(r.Cells["NetRate"].Value);
				double QtyMton = ((OuterEquivalent > 0.0) ? (OuterQty * OuterEquivalent / 1000.0) : 0.0);
				r.Cells["Qty/M.Ton"].Value = QtyMton;
				double innerUom = Conversion.ToDouble(r.Cells["InnerEquivalent"].Value);
				double InnerQty = ((QtyMton > 0.0 && innerUom > 0.0) ? (QtyMton * 1000.0 / innerUom) : 0.0);
				r.Cells["InnerQty"].Value = Math.Round(InnerQty);
				double weightKgs = QtyMton * 1000.0;
				double amount = num / RateEquivalent * weightKgs;
				r.Cells["Amount"].Value = amount;
				r.EndEdit();
				break;
			}
			}
			grdDetail.UpdateData();
			UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: false);
			CalculateLocalAmount(isCustom: false);
			ProportionateOtherItemAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ExImInvoicePackingDetail vd = new ExImInvoicePackingDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				FillDetailListCommonForInsertAndDelete(vd, r);
				vd.ActionTypeId = 3;
				LstRemoveRecordDetail.Add(vd);
				dtdetail.Rows[r.RowIndex].Delete();
				r.Delete();
				grdDetail.UpdateData();
			}
			else
			{
				r.Delete();
				grdDetail.UpdateData();
			}
			UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: false);
			ProportionateOtherItemAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool IsDuplicateFinancialInstrument(bool updateMode = false)
	{
		if (Conversion.ToInt(CmbFinInstrumentsPTDetail.Value) <= 0)
		{
			return false;
		}
		int currentId = Conversion.ToInt(CmbFinInstrumentsPTDetail.Value);
		UltraGridRow selectedRow = ((UltraDropDownBase)CmbFinInstrumentsPTDetail).SelectedRow;
		int currentDocType = Conversion.ToInt((selectedRow != null) ? selectedRow.Cells["DocumnetTypeId"].Value : null);
		GridEXRow[] rows = grdPaymentTermDetail.GetRows();
		foreach (GridEXRow item in rows)
		{
			int rowIndex = item.RowIndex;
			if (!updateMode || rowIndex != updateDetailIndexPT)
			{
				int existingId = Conversion.ToInt(item.Cells["ExImEFormRegistrationId"].Value);
				int existingDocType = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				if (existingId == currentId && existingDocType == currentDocType)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void btndadd_Click(object sender, EventArgs e)
	{
		try
		{
			if (PaymentDetailFormValidation())
			{
				if (IsDuplicateFinancialInstrument())
				{
					throw new Exception("Financial Instrument No already added in the grid. Please check.");
				}
				dtPaymnentDetail.Rows.Add(0, CmbPaymentTermPTDetail.Value, ((Control)(object)CmbPaymentTermPTDetail).Text, (Conversion.ToInt(CmbFinInstrumentsPTDetail.Value) > 0) ? ((UltraDropDownBase)CmbFinInstrumentsPTDetail).SelectedRow.Cells[3].Value : ((object)0), CmbFinInstrumentsPTDetail.Value, (Conversion.ToInt(CmbFinInstrumentsPTDetail.Value) > 0) ? ((Control)(object)CmbFinInstrumentsPTDetail).Text : "", Conversion.ToDouble((object)txtPrcntOfTotalPTDetail.Text.Trim()), Conversion.ToDouble((object)txtFcyAmountPTDetail.Text.Trim()), Conversion.ToDouble((object)txtDueDaysPTDetail.Text.Trim()), Conversion.ToString((object)txtRemarksPTDetail.Text.Trim()));
				grdPaymentTermDetail.DataSource = dtPaymnentDetail;
				grdPaymentTermDetail.RetrieveStructure();
				grdSettingsPaymentTerm(grdPaymentTermDetail);
				ResetPaymentDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void btndupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (PaymentDetailFormValidation())
			{
				if (IsDuplicateFinancialInstrument(updateMode: true))
				{
					throw new Exception("Financial Instrument No already added in the grid. Please check.");
				}
				dtPaymnentDetail.Rows[updateDetailIndexPT]["PaymentTermId"] = CmbPaymentTermPTDetail.Value;
				dtPaymnentDetail.Rows[updateDetailIndexPT]["PaymentTerm"] = ((Control)(object)CmbPaymentTermPTDetail).Text;
				dtPaymnentDetail.Rows[updateDetailIndexPT]["DocumentTypeId"] = ((Conversion.ToInt(CmbFinInstrumentsPTDetail.Value) > 0) ? ((UltraDropDownBase)CmbFinInstrumentsPTDetail).SelectedRow.Cells["DocumnetTypeId"].Value : ((object)0));
				dtPaymnentDetail.Rows[updateDetailIndexPT]["ExImEFormRegistrationId"] = CmbFinInstrumentsPTDetail.Value;
				dtPaymnentDetail.Rows[updateDetailIndexPT]["FinancialInstrumentNo"] = ((Conversion.ToInt(CmbFinInstrumentsPTDetail.Value) > 0) ? ((Control)(object)CmbFinInstrumentsPTDetail).Text : "");
				dtPaymnentDetail.Rows[updateDetailIndexPT]["%OfTotal"] = txtPrcntOfTotalPTDetail.Text.Trim();
				dtPaymnentDetail.Rows[updateDetailIndexPT]["FcyAmount"] = Conversion.ToDouble((object)txtFcyAmountPTDetail.Text.Trim());
				dtPaymnentDetail.Rows[updateDetailIndexPT]["DueDays"] = txtDueDaysPTDetail.Text;
				dtPaymnentDetail.Rows[updateDetailIndexPT]["Remarks"] = txtRemarksPTDetail.Text;
				grdPaymentTermDetail.DataSource = dtPaymnentDetail;
				ResetPaymentDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndcancel_Click(object sender, EventArgs e)
	{
		ResetPaymentDetail();
	}

	private void grdpaymentdetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPaymentTermDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndexPT = item.RowIndex;
				CmbPaymentTermPTDetail.Value = item.Cells["PaymentTermId"].Value.ToString();
				CmbFinInstrumentsPTDetail.Value = item.Cells["ExImEFormRegistrationId"].Value.ToString();
				txtPrcntOfTotalPTDetail.Text = item.Cells["%OfTotal"].Value.ToString();
				txtFcyAmountPTDetail.Text = item.Cells["FcyAmount"].Value.ToString();
				txtDueDaysPTDetail.Text = item.Cells["DueDays"].Value.ToString();
				txtRemarksPTDetail.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddPTDetail.Visible = false;
				BtnUpdatePTDetail.Visible = true;
				BtnCancelPTDetail.Visible = true;
				CmbPaymentTermPTDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentTerm(GridEX grid)
	{
		try
		{
			grid.RootTable.Columns["Id"].Visible = false;
			grid.RootTable.Columns["PaymentTermId"].Visible = false;
			grid.RootTable.Columns["ExImEFormRegistrationId"].Visible = false;
			grid.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3);
			grid.RootTable.Columns["FinancialInstrumentNo"].Width = 350;
			grid.RootTable.Columns["PaymentTerm"].Width = 350;
			if (((Control)(object)grid).Name != "grdPaymentHistory" && ((Control)(object)grid).Name != "grdPaymentCustomHistory")
			{
				GridEX_Helper.AddButton(grid, "Delete", "X", 20, 1);
				grid.FrozenColumns = 1;
			}
			if (((Control)(object)grid).Name == "grdPaymentTermDetail")
			{
				ctrlgrdPaymentTermDetail_Load(null, null);
			}
			if (((Control)(object)grid).Name == "grdPaymentTermDetailCustom")
			{
				ctrlgrdPaymentTermDetailCustom_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpaymentdetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPaymentTermDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeletePTDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletePTDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndexPT != -1)
				{
					throw new Exception("Reset Detail First...");
				}
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddChargeDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherChargesFormValidation())
			{
				dtOtherChargesDetail.Rows.Add(0, 0, 0, Conversion.ToInt(CmbChargesNameChargeDetail.Value), Conversion.ToString((object)((Control)(object)CmbChargesNameChargeDetail).Text), Conversion.ToDouble((object)txtAddAmountChargeDetail.Text), Conversion.ToDouble((object)txtLessAmountChargeDetail.Text), txtRemarksChargeDetail.Text.Trim());
				grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
				grdOtherChargesDetail.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesDetail);
				ResetOtherChargesDetail();
				UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetail, txtaddlessamount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateChargeDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherChargesFormValidation())
			{
				dtOtherChargesDetail.Rows[updateDetailIndexOtherCharges]["ChargesItemId"] = CmbChargesNameChargeDetail.Value;
				dtOtherChargesDetail.Rows[updateDetailIndexOtherCharges]["ChargesItem"] = ((Control)(object)CmbChargesNameChargeDetail).Text;
				dtOtherChargesDetail.Rows[updateDetailIndexOtherCharges]["AddAmount"] = txtAddAmountChargeDetail.Text;
				dtOtherChargesDetail.Rows[updateDetailIndexOtherCharges]["LessAmount"] = txtLessAmountChargeDetail.Text;
				dtOtherChargesDetail.Rows[updateDetailIndexOtherCharges]["Remarks"] = txtRemarksChargeDetail.Text;
				grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
				ResetOtherChargesDetail();
				UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetail, txtaddlessamount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelChargeDetail_Click(object sender, EventArgs e)
	{
		ResetOtherChargesDetail();
	}

	private void grdSettingsOtherChargesDetail(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ContractId", "ContractOtherChargesDetailId", "ChargesItemId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3, hiddenColumns);
			grid.RootTable.Columns["ChargesItem"].Width = 350;
			grid.RootTable.Columns["Remarks"].Width = 350;
			if (((Control)(object)grid).Name != "grdOtherChargesHistory" && ((Control)(object)grid).Name != "grdOtherChargesCustomHistory")
			{
				GridEX_Helper.AddButton(grid, "Delete", "X", 20, 1);
			}
			if (((Control)(object)grid).Name == "grdOtherChargesDetail")
			{
				ctrlgrdOtherChargesDetail_Load(null, null);
			}
			if (((Control)(object)grid).Name == "grdOtherChargesDetailCustom")
			{
				ctrlgrdOtherChargesDetailCustom_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherChargesDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdOtherChargesDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOtherCharges = item.RowIndex;
				CmbChargesNameChargeDetail.Value = item.Cells["ChargesItemId"].Value.ToString();
				txtAddAmountChargeDetail.Text = item.Cells["AddAmount"].Value.ToString();
				txtLessAmountChargeDetail.Text = item.Cells["LessAmount"].Value.ToString();
				txtRemarksChargeDetail.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddChargeDetail.Visible = false;
				BtnUpdateChargeDetail.Visible = true;
				BtnCancelChargeDetail.Visible = true;
				CmbChargesNameChargeDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherChargesDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOtherChargesDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeleteOtherChargesDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOtherChargesDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndexOtherCharges != -1)
				{
					throw new Exception("Reset Detail First...");
				}
				r.Delete();
				UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetail, txtaddlessamount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ValidatePackListWithDetailGrid()
	{
		try
		{
			if (Conversion.ToDouble((object)txtGrossWeightPackListDetail.Text) < Conversion.ToDouble((object)txtNetWeightPackListDetail.Text))
			{
				throw new Exception("Gross Weight Cannot be less than Net Weight");
			}
			int ItemId = Conversion.ToInt(CmbItemFromInvDetailPackListDetail.Value);
			double CurrQty = Conversion.ToDouble((object)txtOuterQtyPackListDetail.Text.Trim());
			double CurrNetWeight = Conversion.ToDouble((object)txtNetWeightPackListDetail.Text.Trim());
			Conversion.ToDouble((object)txtGrossWeightPackListDetail.Text.Trim());
			double DetailGridQtyMton = 0.0;
			double DetailGridQty = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow row2 in rows)
			{
				if (Conversion.ToInt(row2.Cells["ItemId"].Value) == ItemId)
				{
					DetailGridQty += Conversion.ToDouble(row2.Cells["OuterQty"].Value);
					DetailGridQtyMton += Conversion.ToDouble(row2.Cells["Qty/M.Ton"].Value);
				}
			}
			double PackListGridQty = 0.0;
			double PackListGridNetWeight = 0.0;
			rows = grdPackListDetail.GetRows();
			foreach (GridEXRow row in rows)
			{
				if (Conversion.ToInt(row.Cells["ItemId"].Value) == ItemId && row.RowIndex != updateDetailIndexPackList)
				{
					PackListGridQty += Conversion.ToDouble(row.Cells["OuterQty"].Value);
					PackListGridNetWeight += Conversion.ToDouble(row.Cells["NetWeight"].Value);
				}
			}
			if (PackListGridQty + CurrQty > DetailGridQty)
			{
				throw new Exception($"Total Qty{PackListGridQty + CurrQty} can not Greater than Detail Grid Qty {DetailGridQty} for This Item");
			}
			if (CurrNetWeight + PackListGridNetWeight > DetailGridQtyMton * 1000.0)
			{
				throw new Exception($"Total NetWeight{CurrNetWeight + PackListGridNetWeight} can not Greater than Detail Grid Weight{DetailGridQtyMton * 1000.0} for This Item");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnAddPackListDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (PackingListFormValidation())
			{
				ValidatePackListWithDetailGrid();
				dtPackingListDetail.Rows.Add(0, Conversion.ToInt(CmbContainerNoPackListDetail.Value), Conversion.ToString((object)((Control)(object)CmbContainerNoPackListDetail).Text), Conversion.ToInt(CmbItemFromInvDetailPackListDetail.Value), Conversion.ToString(((UltraDropDownBase)CmbItemFromInvDetailPackListDetail).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemFromInvDetailPackListDetail).SelectedRow.Cells["ItemName"].Value), Conversion.ToString(CmbCommodityByItemCusPackListDetail.Value), Conversion.ToString(CmbPmDetailByItemCusPackListDetail.Value), Conversion.ToDateTime((object)txtPackageDatePackListDetail.Value), Conversion.ToDateTime((object)txtExpiryDatePackListDetail.Value), Conversion.ToString((object)txtLotNoPackListDetail.Text), Conversion.ToInt(CmbInspectionNoPackListDetail.Value), Conversion.ToString((object)((Control)(object)CmbInspectionNoPackListDetail).Text), Conversion.ToString((object)txtSubLotNoPackListDetail.Text), Conversion.ToDouble((object)txtOuterQtyPackListDetail.Text.Trim()), Conversion.ToDouble((object)txtNetWeightPackListDetail.Text.Trim()), Conversion.ToDouble((object)txtGrossWeightPackListDetail.Text.Trim()), txtRemarksPackListDetail.Text.Trim());
				grdPackListDetail.DataSource = dtPackingListDetail;
				grdPackListDetail.RetrieveStructure();
				grdSettingsPackListDetaill(grdPackListDetail);
				ResetPackListDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdatePackListDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (PackingListFormValidation())
			{
				ValidatePackListWithDetailGrid();
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ContainerId"] = Conversion.ToInt(CmbContainerNoPackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ContainerNo"] = Conversion.ToString((object)((Control)(object)CmbContainerNoPackListDetail).Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemId"] = Conversion.ToInt(CmbItemFromInvDetailPackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemFromInvDetailPackListDetail).SelectedRow.Cells["ItemCode"].Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItemFromInvDetailPackListDetail).SelectedRow.Cells["ItemName"].Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemCommodityDetail"] = Conversion.ToString((object)txtCommodityPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemPmDetail"] = Conversion.ToString((object)txtPmDetailPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["PackageDate"] = Conversion.ToString((object)txtPackageDatePackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ExpiryDate"] = Conversion.ToString((object)txtExpiryDatePackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["LotNo"] = Conversion.ToString((object)txtLotNoPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["LabInspectionId"] = Conversion.ToInt(CmbInspectionNoPackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["LabInspectionNo"] = Conversion.ToString((object)((Control)(object)CmbInspectionNoPackListDetail).Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["SubLotNo"] = Conversion.ToString((object)txtSubLotNoPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["OuterQty"] = Convert.ToDouble(txtOuterQtyPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["NetWeight"] = Convert.ToDouble(txtNetWeightPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["GrossWeight"] = Convert.ToDouble(txtGrossWeightPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["Remarks"] = Conversion.ToString((object)txtRemarksPackListDetail.Text);
				grdPackListDetail.DataSource = dtPackingListDetail;
				ResetPackListDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelPackListDetail_Click(object sender, EventArgs e)
	{
		ResetPackListDetail();
	}

	private void grdSettingsPackListDetaill(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ContainerId", "ItemId", "LabInspectionId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3, hiddenColumns);
			grid.RootTable.Columns["Remarks"].Width = 350;
			if (((Control)(object)grid).Name == "grdPackListDetail")
			{
				GridEX_Helper.AddButton(grid, "Delete", "X", 20, 1);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackListDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPackListDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndexPackList = item.RowIndex;
				CmbContainerNoPackListDetail.Value = item.Cells["ContainerId"].Value;
				CmbItemFromInvDetailPackListDetail.Value = item.Cells["ItemId"].Value;
				CmbItemFromInvDetailPackListDetail_Leave(null, null);
				txtCommodityPackListDetail.Text = item.Cells["ItemCommodityDetail"].Value.ToString();
				txtPmDetailPackListDetail.Text = item.Cells["ItemPmDetail"].Value.ToString();
				txtPackageDatePackListDetail.Value = Convert.ToDateTime(item.Cells["PackageDate"].Value);
				txtExpiryDatePackListDetail.Value = Convert.ToDateTime(item.Cells["ExpiryDate"].Value);
				txtLotNoPackListDetail.Text = item.Cells["LotNo"].Value.ToString();
				CmbInspectionNoPackListDetail.Value = item.Cells["LabInspectionId"].Value;
				txtSubLotNoPackListDetail.Text = item.Cells["SubLotNo"].Value.ToString();
				txtOuterQtyPackListDetail.Text = item.Cells["OuterQty"].Value.ToString();
				txtNetWeightPackListDetail.Text = item.Cells["NetWeight"].Value.ToString();
				txtGrossWeightPackListDetail.Text = item.Cells["GrossWeight"].Value.ToString();
				txtRemarksPackListDetail.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddPackListDetail.Visible = false;
				BtnUpdatePackListDetail.Visible = true;
				BtnCancelPackListDetail.Visible = true;
				CmbContainerNoPackListDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackListDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPackListDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeletePackListDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletePackListDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndexPackList != -1)
				{
					throw new Exception("Reset Detail First...");
				}
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoadd_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows.Add(CmbItemOIDetail.Value, ((Control)(object)CmbItemOIDetail).Text, Conversion.ToDouble((object)txtQtyOIDetail.Text.Trim()), Conversion.ToString((object)txtRateOIDetail.Text), Conversion.ToDouble((object)txtAmountOIDetail.Text.Trim()), txtRemarksOIDetail.Text.Trim());
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems(grdotheritems);
				ResetOtherItems();
				UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: false);
				ProportionateOtherItemAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows[updateDetailIndexOtherItem]["ItemId"] = CmbItemOIDetail.Value;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["ItemName"] = ((Control)(object)CmbItemOIDetail).Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Qty"] = txtQtyOIDetail.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Rate"] = txtRateOIDetail.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Amount"] = txtAmountOIDetail.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Remarks"] = txtRemarksOIDetail.Text;
				grdotheritems.DataSource = dtOtherItem;
				ResetOtherItems();
				UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: false);
				ProportionateOtherItemAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoCancel_Click(object sender, EventArgs e)
	{
		ResetOtherItems();
	}

	private void grdSettingsOtherItems(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "ItemId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3, hiddenColumns);
			grid.RootTable.Columns["ItemName"].Width = 350;
			grid.RootTable.Columns["Remarks"].Width = 350;
			if (((Control)(object)grid).Name == "grdotheritems")
			{
				GridEX_Helper.AddButton(grid, "Delete", "X", 20, 1);
				ctrlgrdotheritems_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOtherItem = item.RowIndex;
				CmbItemOIDetail.Value = item.Cells["ItemId"].Value.ToString();
				txtQtyOIDetail.Text = item.Cells["Qty"].Value.ToString();
				txtRateOIDetail.Text = item.Cells["Rate"].Value.ToString();
				txtAmountOIDetail.Text = item.Cells["Amount"].Value.ToString();
				txtRemarksOIDetail.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddOIDetail.Visible = false;
				BtnUpdateOIDetail.Visible = true;
				BtnCancelOIDetail.Visible = true;
				CmbItemOIDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdotheritems.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeleteOIDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOIDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndexOtherItem != -1)
				{
					throw new Exception("Reset Detail First...");
				}
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailCustomRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ExImInvoicePackingDetailCustom vd = new ExImInvoicePackingDetailCustom();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				FillDetailListCommonForInsertAndDelete(vd, r);
				vd.ActionTypeId = 3;
				LstRemoveRecordDetailCustom.Add(vd);
				dtdetailCustom.Rows[r.RowIndex].Delete();
				r.Delete();
				grdDetailCustom.UpdateData();
			}
			else
			{
				r.Delete();
				grdDetailCustom.UpdateData();
			}
			UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsDetailCustom()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ContractId", "ContractDetailId", "ContractNoInvoiceWise", "ContractScheduleId", "ContractSchedule", "ContractScheduleDetailId", "ItemCategoryId", "ItemId", "PmItemId",
				"BrandId", "OuterUomId", "OuterEquivalent", "OuterQtyEquivalent", "InnerUomId", "InnerEquivalent", "RateUOMId", "RateEquivalent"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"CommodityDetail",
					(EditType)1
				},
				{
					"PackingDetail",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				},
				{
					"Qty/M.Ton",
					(EditType)1
				},
				{
					"OuterQty",
					(EditType)1
				},
				{
					"InnerQty",
					(EditType)1
				},
				{
					"PalletQty",
					(EditType)1
				},
				{
					"BuyerLotNo",
					(EditType)1
				},
				{
					"ProductCode",
					(EditType)1
				},
				{
					"BuyerHSCode",
					(EditType)1
				},
				{
					"RateAddLess",
					(EditType)1
				},
				{
					"HSCode",
					(EditType)1
				},
				{
					"GrossWeight",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetailCustom, 2, 3, hiddenColumns, editableColumns);
			grdDetailCommonSetting(grdDetailCustom);
			grdDetailCustom.RootTable.Columns["RateAddLess"].Visible = true;
			grdDetailCustom.RootTable.Columns["RateWithoutAddLess"].Visible = true;
			grdComboBind(grdDetailCustom);
			GridEX_Helper.AddButton(grdDetailCustom, "Delete", "X", 20, 0);
			grdDetailCustom.FrozenColumns = 2;
			ctrlgrdDetailCustom_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailCustom_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetailCustom.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteDetailCustomRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailCustom_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetailCustom.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			grdDetailCustom.UpdateData();
			string Key = e.Column.Key;
			switch (Key)
			{
			case "Qty/M.Ton":
			case "OuterUomId":
			case "OuterUom":
			case "RateAddLess":
			case "RateUOMId":
			case "RateEquivalent":
			{
				r.BeginEdit();
				double QtyMton2 = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
				double OuterEquivalent2 = Conversion.ToDouble(r.Cells["OuterEquivalent"].Value);
				double RateWithoutAddLess = Conversion.ToDouble(r.Cells["RateWithoutAddLess"].Value);
				double RateAddLess = Conversion.ToDouble(r.Cells["RateAddLess"].Value);
				double RateEquivalent2 = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
				double NetRate = Conversion.ToDouble(r.Cells["NetRate"].Value);
				switch (Key)
				{
				case "Qty/M.Ton":
				case "OuterUomId":
				case "OuterUom":
				{
					double OuterQty2 = ((OuterEquivalent2 > 0.0) ? (QtyMton2 * 1000.0 / OuterEquivalent2) : 0.0);
					r.Cells["OuterQty"].Value = OuterQty2;
					break;
				}
				case "RateAddLess":
				case "RateUOMId":
				case "RateEquivalent":
					NetRate = RateWithoutAddLess + RateAddLess;
					r.Cells["NetRate"].Value = NetRate;
					break;
				}
				double weightKgs2 = QtyMton2 * 1000.0;
				double amount2 = NetRate / RateEquivalent2 * weightKgs2;
				r.Cells["Amount"].Value = amount2;
				r.EndEdit();
				break;
			}
			case "OuterQty":
			{
				r.BeginEdit();
				double OuterQty = Conversion.ToDouble(r.Cells["OuterQty"].Value);
				double OuterEquivalent = Conversion.ToDouble(r.Cells["OuterEquivalent"].Value);
				double RateEquivalent = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
				double num = Conversion.ToDouble(r.Cells["NetRate"].Value);
				double QtyMton = ((OuterEquivalent > 0.0) ? (OuterQty * OuterEquivalent / 1000.0) : 0.0);
				r.Cells["Qty/M.Ton"].Value = QtyMton;
				double weightKgs = QtyMton * 1000.0;
				double amount = num / RateEquivalent * weightKgs;
				r.Cells["Amount"].Value = amount;
				r.EndEdit();
				break;
			}
			}
			grdDetailCustom.UpdateData();
			UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: true);
			CalculateLocalAmount(isCustom: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool IsDuplicateFinancialInstrumentCustom(bool updateMode = false)
	{
		if (Conversion.ToInt(CmbFinInstrumentsPTDetailCustom.Value) <= 0)
		{
			return false;
		}
		int currentId = Conversion.ToInt(CmbFinInstrumentsPTDetailCustom.Value);
		UltraGridRow selectedRow = ((UltraDropDownBase)CmbFinInstrumentsPTDetailCustom).SelectedRow;
		int currentDocType = Conversion.ToInt((selectedRow != null) ? selectedRow.Cells["DocumnetTypeId"].Value : null);
		GridEXRow[] rows = grdPaymentTermDetailCustom.GetRows();
		foreach (GridEXRow item in rows)
		{
			int rowIndex = item.RowIndex;
			if (!updateMode || rowIndex != updateDetailIndexPTCustom)
			{
				int existingId = Conversion.ToInt(item.Cells["ExImEFormRegistrationId"].Value);
				int existingDocType = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				if (existingId == currentId && existingDocType == currentDocType)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void BtnAddPTDetailCustom_Click(object sender, EventArgs e)
	{
		try
		{
			if (PaymentDetailCustomFormValidation())
			{
				if (IsDuplicateFinancialInstrumentCustom())
				{
					throw new Exception("Financial Instrument No already added in the grid. Please check.");
				}
				dtPaymnentDetailCustom.Rows.Add(0, CmbPaymentTermPTDetailCustom.Value, ((Control)(object)CmbPaymentTermPTDetailCustom).Text, (Conversion.ToInt(CmbFinInstrumentsPTDetailCustom.Value) > 0) ? ((UltraDropDownBase)CmbFinInstrumentsPTDetailCustom).SelectedRow.Cells[3].Value : ((object)0), CmbFinInstrumentsPTDetailCustom.Value, (Conversion.ToInt(CmbFinInstrumentsPTDetailCustom.Value) > 0) ? ((Control)(object)CmbFinInstrumentsPTDetailCustom).Text : "", Conversion.ToDouble((object)txtPrcntOfTotalPTDetailCustom.Text.Trim()), Conversion.ToDouble((object)txtFcyAmountPTDetailCustom.Text.Trim()), Conversion.ToDouble((object)txtDueDaysPTDetailCustom.Text.Trim()), Conversion.ToString((object)txtRemarksPTDetailCustom.Text.Trim()));
				grdPaymentTermDetailCustom.DataSource = dtPaymnentDetailCustom;
				grdPaymentTermDetailCustom.RetrieveStructure();
				grdSettingsPaymentTerm(grdPaymentTermDetailCustom);
				ResetPaymentDetailCustom();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void BtnUpdatePTDetailCustom_Click(object sender, EventArgs e)
	{
		try
		{
			if (PaymentDetailCustomFormValidation())
			{
				if (IsDuplicateFinancialInstrumentCustom(updateMode: true))
				{
					throw new Exception("Financial Instrument No already added in the grid. Please check.");
				}
				dtPaymnentDetailCustom.Rows[updateDetailIndexPTCustom]["PaymentTermId"] = CmbPaymentTermPTDetailCustom.Value;
				dtPaymnentDetailCustom.Rows[updateDetailIndexPTCustom]["PaymentTerm"] = ((Control)(object)CmbPaymentTermPTDetailCustom).Text;
				dtPaymnentDetailCustom.Rows[updateDetailIndexPTCustom]["DocumentTypeId"] = ((Conversion.ToInt(CmbFinInstrumentsPTDetailCustom.Value) > 0) ? ((UltraDropDownBase)CmbFinInstrumentsPTDetailCustom).SelectedRow.Cells["DocumnetTypeId"].Value : ((object)0));
				dtPaymnentDetailCustom.Rows[updateDetailIndexPTCustom]["ExImEFormRegistrationId"] = CmbFinInstrumentsPTDetailCustom.Value;
				dtPaymnentDetailCustom.Rows[updateDetailIndexPTCustom]["FinancialInstrumentNo"] = ((Conversion.ToInt(CmbFinInstrumentsPTDetailCustom.Value) > 0) ? ((Control)(object)CmbFinInstrumentsPTDetailCustom).Text : "");
				dtPaymnentDetailCustom.Rows[updateDetailIndexPTCustom]["%OfTotal"] = txtPrcntOfTotalPTDetailCustom.Text.Trim();
				dtPaymnentDetailCustom.Rows[updateDetailIndexPTCustom]["FcyAmount"] = txtFcyAmountPTDetailCustom.Text.Trim();
				dtPaymnentDetailCustom.Rows[updateDetailIndexPTCustom]["DueDays"] = txtDueDaysPTDetailCustom.Text;
				dtPaymnentDetailCustom.Rows[updateDetailIndexPTCustom]["Remarks"] = txtRemarksPTDetailCustom.Text;
				grdPaymentTermDetailCustom.DataSource = dtPaymnentDetailCustom;
				ResetPaymentDetailCustom();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelPTDetailCustom_Click(object sender, EventArgs e)
	{
		ResetPaymentDetailCustom();
	}

	private void grdPaymentTermDetailCustom_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPaymentTermDetailCustom.CurrentRow;
			if (item != null)
			{
				updateDetailIndexPTCustom = item.RowIndex;
				CmbPaymentTermPTDetailCustom.Value = item.Cells["PaymentTermId"].Value.ToString();
				CmbFinInstrumentsPTDetailCustom.Value = item.Cells["ExImEFormRegistrationId"].Value.ToString();
				txtPrcntOfTotalPTDetailCustom.Text = item.Cells["%OfTotal"].Value.ToString();
				txtFcyAmountPTDetailCustom.Text = item.Cells["FcyAmount"].Value.ToString();
				txtDueDaysPTDetailCustom.Text = item.Cells["DueDays"].Value.ToString();
				txtRemarksPTDetailCustom.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddPTDetailCustom.Visible = false;
				BtnUpdatePTDetailCustom.Visible = true;
				BtnCancelPTDetailCustom.Visible = true;
				CmbPaymentTermPTDetailCustom.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTermDetailCustom_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPaymentTermDetailCustom.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeletePTDetailCustomRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletePTDetailCustomRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndexPTCustom != -1)
				{
					throw new Exception("Reset DetailCustom First...");
				}
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddChargeDetailCustom_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherChargesCustomFormValidation())
			{
				dtOtherChargesDetailCustom.Rows.Add(0, Conversion.ToInt(CmbChargesNameChargeDetailCustom.Value), Conversion.ToString((object)((Control)(object)CmbChargesNameChargeDetailCustom).Text), Conversion.ToDouble((object)txtAddAmountChargeDetailCustom.Text), Conversion.ToDouble((object)txtLessAmountChargeDetailCustom.Text), txtRemarksChargeDetailCustom.Text.Trim());
				grdOtherChargesDetailCustom.DataSource = dtOtherChargesDetailCustom;
				grdOtherChargesDetailCustom.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesDetailCustom);
				ResetOtherChargesDetailCustom();
				UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetailCustom, txtAddLessHeaderCustom);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateChargeDetailCustom_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherChargesCustomFormValidation())
			{
				dtOtherChargesDetailCustom.Rows[updateDetailIndexOtherChargesCustom]["ChargesItemId"] = CmbChargesNameChargeDetailCustom.Value;
				dtOtherChargesDetailCustom.Rows[updateDetailIndexOtherChargesCustom]["ChargesItem"] = ((Control)(object)CmbChargesNameChargeDetailCustom).Text;
				dtOtherChargesDetailCustom.Rows[updateDetailIndexOtherChargesCustom]["AddAmount"] = txtAddAmountChargeDetailCustom.Text;
				dtOtherChargesDetailCustom.Rows[updateDetailIndexOtherChargesCustom]["LessAmount"] = txtLessAmountChargeDetailCustom.Text;
				dtOtherChargesDetailCustom.Rows[updateDetailIndexOtherChargesCustom]["Remarks"] = txtRemarksChargeDetailCustom.Text;
				grdOtherChargesDetailCustom.DataSource = dtOtherChargesDetailCustom;
				ResetOtherChargesDetailCustom();
				UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetailCustom, txtAddLessHeaderCustom);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelChargeDetailCustom_Click(object sender, EventArgs e)
	{
		ResetOtherChargesDetailCustom();
	}

	private void grdOtherChargesDetailCustom_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdOtherChargesDetailCustom.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOtherChargesCustom = item.RowIndex;
				CmbChargesNameChargeDetailCustom.Value = item.Cells["ChargesItemId"].Value.ToString();
				txtAddAmountChargeDetailCustom.Text = item.Cells["AddAmount"].Value.ToString();
				txtLessAmountChargeDetailCustom.Text = item.Cells["LessAmount"].Value.ToString();
				txtRemarksChargeDetailCustom.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddChargeDetailCustom.Visible = false;
				BtnUpdateChargeDetailCustom.Visible = true;
				BtnCancelChargeDetailCustom.Visible = true;
				CmbChargesNameChargeDetailCustom.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherChargesDetailCustom_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOtherChargesDetailCustom.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeleteOtherChargesDetailCustomRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOtherChargesDetailCustomRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndexOtherChargesCustom != -1)
				{
					throw new Exception("Reset DetailCustom First...");
				}
				r.Delete();
				UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetailCustom, txtAddLessHeaderCustom);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CheckItemIds(DataTable dtDetail, DataTable dtPackListDetail)
	{
		try
		{
			var detailSummary = (from row in dtDetail.AsEnumerable()
				group row by row.Field<int>("ItemId")).ToDictionary((IGrouping<int, DataRow> g) => g.Key, (IGrouping<int, DataRow> g) => new
			{
				ItemName = g.First().Field<string>("ItemName"),
				TotalQty = g.Sum((DataRow r) => r.Field<double>("OuterQty")),
				TotalWeightKg = g.Sum((DataRow r) => r.Field<double>("Qty/M.Ton")) * 1000.0,
				TotalGrossWeight = g.Sum((DataRow r) => r.Field<double>("GrossWeight"))
			});
			var dictionary = (from row in dtPackListDetail.AsEnumerable()
				group row by row.Field<int>("ItemId")).ToDictionary((IGrouping<int, DataRow> g) => g.Key, (IGrouping<int, DataRow> g) => new
			{
				ItemName = g.First().Field<string>("ItemName"),
				TotalQty = g.Sum((DataRow r) => r.Field<double>("OuterQty")),
				TotalNetWeight = g.Sum((DataRow r) => r.Field<double>("NetWeight")),
				TotalGrossWeight = g.Sum((DataRow r) => r.Field<double>("GrossWeight"))
			});
			List<string> errors = new List<string>();
			foreach (var pack in dictionary)
			{
				int itemId = pack.Key;
				var packData = pack.Value;
				if (!detailSummary.ContainsKey(itemId))
				{
					errors.Add("Item '" + packData.ItemName + "' exists in PackList but not in Detail.");
					continue;
				}
				var detailData = detailSummary[itemId];
				if (packData.TotalQty != detailData.TotalQty)
				{
					errors.Add($"Total Qty for '{packData.ItemName}' in PackList ({packData.TotalQty}) not equal to Detail Qty ({detailData.TotalQty}).");
				}
				if (packData.TotalNetWeight != detailData.TotalWeightKg)
				{
					errors.Add($"Total Net Weight for '{packData.ItemName}' in PackList ({packData.TotalNetWeight} kg) not equal to Detail ({detailData.TotalWeightKg} kg).");
				}
				if (packData.TotalGrossWeight != detailData.TotalGrossWeight)
				{
					errors.Add($"Total Gross Weight for '{packData.ItemName}' in PackList ({packData.TotalGrossWeight} kg) not equal to Detail ({detailData.TotalGrossWeight} kg).");
				}
			}
			if (errors.Any())
			{
				throw new Exception(string.Join("\n", errors));
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Validation failed: " + ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Expected O, but got Unknown
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Expected O, but got Unknown
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e60: Expected O, but got Unknown
		//IL_10da: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e1: Expected O, but got Unknown
		//IL_12d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12df: Expected O, but got Unknown
		//IL_14e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e7: Expected O, but got Unknown
		//IL_1673: Unknown result type (might be due to invalid IL or missing references)
		//IL_167a: Expected O, but got Unknown
		//IL_17a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a7: Expected O, but got Unknown
		//IL_1b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b79: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				tabControl4.SelectedIndex = 0;
				tabControl2.SelectedIndex = 0;
				throw new Exception("Item Detail Record Not Found");
			}
			if (grdPaymentTermDetail.RowCount == 0)
			{
				tabControl4.SelectedIndex = 0;
				tabControl2.SelectedIndex = 1;
				CmbPaymentTermPTDetail.Focus();
				throw new Exception("Payment Detail Not Found. Please Check! ");
			}
			if (grdDetailCustom.GetRows().Count() == 0)
			{
				tabControl4.SelectedIndex = 1;
				tabControl5.SelectedIndex = 0;
				throw new Exception("Custom Item Detail Record Not Found");
			}
			if (grdPaymentTermDetailCustom.RowCount == 0)
			{
				tabControl4.SelectedIndex = 1;
				tabControl5.SelectedIndex = 1;
				CmbPaymentTermPTDetailCustom.Focus();
				throw new Exception("Custom Payment Detail Not Found. Please Check! ");
			}
			List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)cmbInvoiceNo, "Invoice No", FormHelper.ValidType.String),
				(txtDocumentaryCreditNo, "Documentary Credit No", FormHelper.ValidType.String),
				((Control)(object)CmbCustomer, "Customer", FormHelper.ValidType.INT),
				((Control)(object)CmbConsignee, "Consignee", FormHelper.ValidType.INT),
				((Control)(object)CmbLoadingPort, "Loading Port", FormHelper.ValidType.INT),
				((Control)(object)CmbDestinationPort, "Destination Port", FormHelper.ValidType.INT),
				((Control)(object)CmbCareierType, "Carrier Type", FormHelper.ValidType.INT),
				((Control)(object)CmbDeliveryTerm, "Delivery Term", FormHelper.ValidType.INT),
				((Control)(object)CmbContinent, "Continent", FormHelper.ValidType.INT),
				((Control)(object)CmbOriginCountry, "Origin", FormHelper.ValidType.INT),
				((Control)(object)CmbImporterCountry, "Importer Country", FormHelper.ValidType.INT),
				((Control)(object)CmbPlaceOfDelivery, "Delivery Place", FormHelper.ValidType.INT),
				((Control)(object)CmbFcyCode, "FCY Code", FormHelper.ValidType.INT),
				(txtExRate, "Exchange Rate", FormHelper.ValidType.Double),
				(txtFcyAmount, "FCY Amount", FormHelper.ValidType.Double),
				(txtLocalAmt, "Local Amount", FormHelper.ValidType.Double),
				(txtTotalNetAmount, "Total Amount", FormHelper.ValidType.Double),
				(txtNoOfContainer, "No Of Container", FormHelper.ValidType.Double),
				(txtGrossWeight, "Gross MTon", FormHelper.ValidType.Double),
				(txtNetWeight, "Net MTon", FormHelper.ValidType.Double)
			};
			if (AllowExportMultiCompanies)
			{
				controlsToValidate.Insert(0, ((Control)(object)CmbExportCompany, "Export Company", FormHelper.ValidType.INT));
			}
			if (!FormHelper.ValidateControls(controlsToValidate))
			{
				return;
			}
			UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetail, txtaddlessamount);
			UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: false);
			double num = Conversion.ToDouble((object)txtGrossWeight.Text);
			double netWeight = Conversion.ToDouble((object)txtNetWeight.Text);
			if (num < netWeight)
			{
				MessageBox.Show("Gross Weight must be equal to or greater than Net Weight. Thank you.");
				txtGrossWeight.Focus();
				return;
			}
			if (GridEX_Helper.GetColumnSum(grdPaymentTermDetail, "%ofTotal") != 100.0)
			{
				MessageBox.Show("Payment Percent Not Equal To 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				tabControl4.SelectedIndex = 0;
				tabControl2.SelectedIndex = 1;
				CmbPaymentTermPTDetail.Focus();
				return;
			}
			if (GridEX_Helper.GetColumnSum(grdPaymentTermDetailCustom, "%ofTotal") != 100.0)
			{
				MessageBox.Show("Payment Percent Not Equal To 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				tabControl4.SelectedIndex = 1;
				tabControl5.SelectedIndex = 1;
				CmbPaymentTermPTDetailCustom.Focus();
				return;
			}
			CheckItemIds(dtdetail, dtPackingListDetail);
			if (!FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			ExImInvoice PIH = new ExImInvoice();
			PIH.OrganizationId = UserAccount.OrganizationId;
			PIH.CompanyId = UserAccount.CompanyId;
			PIH.BranchesId = UserAccount.BranchesId;
			PIH.ProjectsId = UserAccount.BranchesId;
			PIH.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PIH.DocumentTypeId = DocumentTypeId;
			PIH.EntryDate = DateTime.Now;
			PIH.ModifyDate = DateTime.Now;
			PIH.EntryUser = UserAccount.ID;
			PIH.ModifyUser = UserAccount.ID;
			PIH.ApprovedDate = DateTime.Now;
			PIH.IsApproved = false;
			PIH.ExportCompanyId = Conversion.ToInt(CmbExportCompany.Value);
			PIH.Id = RecId;
			PIH.DocCode = Conversion.ToInt((object)txtdocno.Text);
			PIH.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			PIH.InvoiceNo = Conversion.ToString((object)((Control)(object)cmbInvoiceNo).Text);
			PIH.DocumentaryCreditNo = Conversion.ToString((object)txtDocumentaryCreditNo.Text.Trim());
			PIH.DocumentaryCreditNoIssueDate = Conversion.ToDateTime((object)txtDocumentaryCreditNoDate.Value);
			PIH.SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value);
			PIH.ConsigneeId = Conversion.ToInt(CmbConsignee.Value);
			PIH.CustomerContractNos = Conversion.ToString((object)txtCustomerContractsNos.Text);
			PIH.ContractDates = Conversion.ToString((object)txtCustomerContractsDates.Text);
			PIH.NotifyParty1 = Conversion.ToInt(CmbNotifyParty1.Value);
			PIH.NotifyParty2 = Conversion.ToInt(CmbNotifyParty2.Value);
			PIH.NotifyParty3 = Conversion.ToInt(CmbNotifyParty3.Value);
			PIH.ImporterBankId = Conversion.ToInt(CmbImporterBank.Value);
			PIH.ExporteBankId = Conversion.ToInt(CmbExporterBank.Value);
			PIH.LotNoRef = Conversion.ToString((object)txtLotRefHeader.Text);
			PIH.EFormNo = Conversion.ToString((object)txtGdNo.Text.Trim());
			PIH.EFormDate = Conversion.ToDateTime((object)txtGdDate.Value);
			PIH.LoadingPortId = Conversion.ToInt(CmbLoadingPort.Value);
			PIH.DestinationPortId = Conversion.ToInt(CmbDestinationPort.Value);
			PIH.CarierType = Conversion.ToString((object)((Control)(object)CmbCareierType).Text);
			PIH.DeliveryTermId = Conversion.ToInt(CmbDeliveryTerm.Value);
			PIH.DeliveryRemarks = Conversion.ToString((object)txtDeliveryRemarks.Text);
			PIH.TCPRegNo = Conversion.ToString((object)txtTcpRegNo.Text);
			PIH.ContinentId = Conversion.ToInt(CmbContinent.Value);
			PIH.OriginCountryId = Conversion.ToInt(CmbOriginCountry.Value);
			PIH.ImporterCountryId = Conversion.ToInt(CmbImporterCountry.Value);
			PIH.PlaceOfDeliveryId = Conversion.ToInt(CmbPlaceOfDelivery.Value);
			PIH.FcurrencyId = Conversion.ToInt(CmbFcyCode.Value);
			PIH.ConversionRate = Conversion.ToDouble((object)txtExRate.Text.Trim());
			PIH.FCurrencyAmount = Conversion.ToDouble((object)txtFcyAmount.Text.Trim());
			PIH.EquivalentAmount = Conversion.ToDouble((object)txtLocalAmt.Text.Trim());
			PIH.AddLessAmount = Conversion.ToDouble((object)txtaddlessamount.Text.Trim());
			PIH.AddLessComments = Conversion.ToString((object)txtaddlesscommnets.Text);
			PIH.TotalAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim());
			PIH.NoOfContainers = Conversion.ToInt((object)txtNoOfContainer.Text.Trim());
			PIH.GrossMton = Conversion.ToDecimal((object)txtGrossWeight.Text.Trim());
			PIH.GrossWeight = Conversion.ToDouble((object)(PIH.GrossMton * 1000m));
			PIH.NetMton = Conversion.ToDecimal((object)txtNetWeight.Text);
			PIH.NetWeight = Conversion.ToDouble((object)(PIH.NetMton * 1000m));
			PIH.Certificate1 = Conversion.ToString((object)txtCertificate1.Text.Trim());
			PIH.Certificate2 = Conversion.ToString((object)txtCertificateOfOrigion.Text.Trim());
			PIH.Remarks1 = Conversion.ToString((object)txtRemarks1.Text);
			PIH.Remarks2 = Conversion.ToString((object)txtRemarks2.Text);
			PIH.OtherRemarks1 = Conversion.ToString((object)txtOtherRemarks1.Text);
			PIH.OtherRemarks2 = Conversion.ToString((object)txtOtherRemarks2.Text);
			PIH.ExImInvoiceDetail = new List<ExImInvoicePackingDetail>();
			PIH.ExImInvoiceDetailCustom = new List<ExImInvoicePackingDetailCustom>();
			PIH.ExImInvoicePaymentTermsDetail = new List<ExImInvoicePaymentTermsDetail>();
			PIH.ExImInvoicePaymentTermsDetailCustom = new List<ExImInvoicePaymentTermsDetailCustom>();
			PIH.ExImInvoiceOtherChargesDetailList = new List<ExImInvoiceOtherChargesDetail>();
			PIH.ExImInvoiceOtherChargesDetailCustomList = new List<ExImInvoiceOtherChargesDetailCustom>();
			PIH.ExImInvoicePackingListDetailList = new List<ExImInvoicePackingListDetail>();
			PIH.ExImInvoiceOtherItemslist = new List<ExImInvoiceOtherItems>();
			double TotalFcAmount = 0.0;
			double MtonPerContainer = Conversion.ToDouble((object)txtNetWeight.Text) / Conversion.ToDouble((object)txtNoOfContainer.Text);
			string ContractIds = "";
			string ContractDetailIds = "";
			string ContractNos = "";
			string ContractDates = "";
			string ContractScheduleIds = "";
			string ContractScheduleNos = "";
			double UtilizeAmountPT = 0.0;
			double UtilizeAmountPTCustm = 0.0;
			if (LstRemoveRecordDetail.Count > 0 && RecId > 0)
			{
				for (int j = 0; j < LstRemoveRecordDetail.Count; j++)
				{
					PIH.ExImInvoiceDetail.Add(LstRemoveRecordDetail[j]);
				}
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r8 in rows)
			{
				ExImInvoicePackingDetail vd6 = new ExImInvoicePackingDetail();
				vd6.Id = ((RecId != 0) ? Conversion.ToInt(r8.Cells["Id"].Value) : 0);
				vd6.ActionTypeId = ((vd6.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd6, r8);
				FormHelper.ValidateField(vd6.ContractId, "Contract No", r8.RowIndex);
				FormHelper.ValidateField(vd6.ContractScheduleId, "Contract Schedule No", r8.RowIndex);
				FormHelper.ValidateField(vd6.ItemId, "Item", r8.RowIndex);
				FormHelper.ValidateField(vd6.CropYearId, "CropYear", r8.RowIndex);
				FormHelper.ValidateField(vd6.PackingMaterialTypeId, "PackingType", r8.RowIndex);
				FormHelper.ValidateField(vd6.MTon, "Qty/M.Ton", r8.RowIndex);
				FormHelper.ValidateField(vd6.OuterQtyUomId, "Outer Uom", r8.RowIndex);
				FormHelper.ValidateField(vd6.OuterQty, "OuterQty", r8.RowIndex);
				FormHelper.ValidateField(vd6.RatePrice, "Rate", r8.RowIndex);
				FormHelper.ValidateField(vd6.RateUomId, "Rate Uom", r8.RowIndex);
				FormHelper.ValidateField(vd6.FcAmount, "Amount", r8.RowIndex);
				if (vd6.GrossWeight > vd6.NetWeight * 1.4)
				{
					double allowedGross2 = vd6.NetWeight * 1.4;
					MessageBox.Show($"Gross Weight '{vd6.GrossWeight}' cannot exceed 40% above the Net Weight '{vd6.NetWeight}'. " + $"Allowed maximum is '{allowedGross2:F2}'. (Detail Grid Row No: {r8.RowIndex + 1})");
					return;
				}
				ContractIds = ContractIds + "," + Conversion.ToString(r8.Cells["ContractId"].Value);
				ContractDetailIds = ContractDetailIds + "," + Conversion.ToString(r8.Cells["ContractDetailId"].Value);
				ContractNos = ContractNos + "," + Conversion.ToString(r8.Cells["ContractNo"].Value);
				ContractDates = ContractDates + "," + Conversion.ToDateTime(r8.Cells["ContractDate"].Value).ToShortDateString();
				ContractScheduleIds = ContractScheduleIds + "," + Conversion.ToString(r8.Cells["ContractScheduleId"].Value);
				ContractScheduleNos = ContractScheduleNos + "," + Conversion.ToString(r8.Cells["ContractNoInvoiceWise"].Value);
				TotalFcAmount += vd6.FcAmount;
				vd6.NoofContainers = vd6.MTon / MtonPerContainer;
				PIH.ExImInvoiceDetail.Add(vd6);
			}
			if (LstRemoveRecordDetailCustom.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < LstRemoveRecordDetailCustom.Count; i++)
				{
					PIH.ExImInvoiceDetailCustom.Add(LstRemoveRecordDetailCustom[i]);
				}
			}
			rows = grdDetailCustom.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				ExImInvoicePackingDetailCustom vd5 = new ExImInvoicePackingDetailCustom();
				vd5.Id = ((RecId != 0) ? Conversion.ToInt(r7.Cells["Id"].Value) : 0);
				vd5.ActionTypeId = ((vd5.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd5, r7);
				FormHelper.ValidateField(vd5.ContractId, "Contract No", r7.RowIndex, "Detail Custom Grid");
				FormHelper.ValidateField(vd5.ContractScheduleId, "Contract Schedule No", r7.RowIndex, "Detail Custom Grid");
				FormHelper.ValidateField(vd5.ItemId, "Item", r7.RowIndex, "Detail Custom Grid");
				FormHelper.ValidateField(vd5.CropYearId, "CropYear", r7.RowIndex, "Detail Custom Grid");
				FormHelper.ValidateField(vd5.PackingMaterialTypeId, "PackingType", r7.RowIndex, "Detail Custom Grid");
				FormHelper.ValidateField(vd5.MTon, "Qty/M.Ton", r7.RowIndex, "Detail Custom Grid");
				FormHelper.ValidateField(vd5.OuterQtyUomId, "Outer Uom", r7.RowIndex, "Detail Custom Grid");
				FormHelper.ValidateField(vd5.OuterQty, "OuterQty", r7.RowIndex, "Detail Custom Grid");
				if (vd5.GrossWeight > vd5.NetWeight * 1.4)
				{
					double allowedGross = vd5.NetWeight * 1.4;
					MessageBox.Show($"Gross Weight '{vd5.GrossWeight}' cannot exceed 40% above the Net Weight '{vd5.NetWeight}'. " + $"Allowed maximum is '{allowedGross:F2}'. (Custom Detail Grid Row No: {r7.RowIndex + 1})");
					return;
				}
				FormHelper.ValidateField(vd5.RatePrice, "Rate", r7.RowIndex, "Detail Custom Grid");
				FormHelper.ValidateField(vd5.RateUomId, "Rate Uom", r7.RowIndex, "Detail Custom Grid");
				FormHelper.ValidateField(vd5.FcAmount, "Amount", r7.RowIndex, "Detail Custom Grid");
				PIH.ExImInvoiceDetailCustom.Add(vd5);
			}
			rows = grdPaymentTermDetail.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				ExImInvoicePaymentTermsDetail vdp2 = new ExImInvoicePaymentTermsDetail();
				vdp2.DocumentTypeId = Conversion.ToInt(r6.Cells["DocumentTypeId"].Value);
				vdp2.PaymentTermId = Conversion.ToInt(r6.Cells["PaymentTermId"].Value);
				vdp2.ExImEFormRegistrationId = Conversion.ToInt(r6.Cells["ExImEFormRegistrationId"].Value);
				vdp2.FinancialInstrumentNo = Conversion.ToString((object)r6.Cells["FinancialInstrumentNo"].Text);
				vdp2.PrcntOfTotal = Conversion.ToDouble(r6.Cells["%OfTotal"].Value);
				vdp2.DueDays = Conversion.ToInt(r6.Cells["DueDays"].Value);
				vdp2.FcyAmount = Conversion.ToDouble(r6.Cells["FcyAmount"].Value);
				vdp2.PaymentRemarks = Conversion.ToString(r6.Cells["Remarks"].Value);
				FormHelper.ValidateField(vdp2.PaymentTermId, "Payment Term", r6.RowIndex, "Payment Detail");
				if (vdp2.PaymentTermId == 1)
				{
					FormHelper.ValidateField(vdp2.ExImEFormRegistrationId, "Financial Instrument number", r6.RowIndex, "Payment Detail");
				}
				FormHelper.ValidateField(vdp2.FcyAmount, "FcyAmount", r6.RowIndex, "Payment Detail");
				UtilizeAmountPT += vdp2.FcyAmount;
				vdp2.SortNo = r6.RowIndex + 1;
				PIH.ExImInvoicePaymentTermsDetail.Add(vdp2);
			}
			if (Math.Round(PIH.TotalAmount, 0) != Math.Round(UtilizeAmountPT, 0))
			{
				throw new Exception("Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check");
			}
			rows = grdPaymentTermDetailCustom.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				ExImInvoicePaymentTermsDetailCustom vdp = new ExImInvoicePaymentTermsDetailCustom();
				vdp.DocumentTypeId = Conversion.ToInt(r5.Cells["DocumentTypeId"].Value);
				vdp.PaymentTermId = Conversion.ToInt(r5.Cells["PaymentTermId"].Value);
				vdp.ExImEFormRegistrationId = Conversion.ToInt(r5.Cells["ExImEFormRegistrationId"].Value);
				vdp.FinancialInstrumentNo = Conversion.ToString((object)r5.Cells["FinancialInstrumentNo"].Text);
				vdp.PrcntOfTotal = Conversion.ToDouble(r5.Cells["%OfTotal"].Value);
				vdp.DueDays = Conversion.ToInt(r5.Cells["DueDays"].Value);
				vdp.FcyAmount = Conversion.ToDouble(r5.Cells["FcyAmount"].Value);
				vdp.PaymentRemarks = Conversion.ToString(r5.Cells["Remarks"].Value);
				FormHelper.ValidateField(vdp.PaymentTermId, "Payment Term", r5.RowIndex, "Payment DetailCustom");
				if (vdp.PaymentTermId == 1)
				{
					FormHelper.ValidateField(vdp.ExImEFormRegistrationId, "Financial Instrument number", r5.RowIndex, "Payment DetailCustom");
				}
				FormHelper.ValidateField(vdp.FcyAmount, "FcyAmount", r5.RowIndex, "Payment DetailCustom");
				UtilizeAmountPTCustm += vdp.FcyAmount;
				vdp.SortNo = r5.RowIndex + 1;
				PIH.ExImInvoicePaymentTermsDetailCustom.Add(vdp);
			}
			if (Math.Round(Conversion.ToDouble((object)txtTotalFcyAmountHeaderCustom.Text), 0) != Math.Round(UtilizeAmountPTCustm, 0))
			{
				throw new Exception("Total Invoice Amount and Total Payment Utilize Amount Not equal in Custom Please Check");
			}
			rows = grdOtherChargesDetail.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				ExImInvoiceOtherChargesDetail vd4 = new ExImInvoiceOtherChargesDetail();
				vd4.Id = Conversion.ToInt(r4.Cells["Id"].Value);
				vd4.ContractId = Conversion.ToInt(r4.Cells["ContractId"].Value);
				vd4.ContractOtherChargesDetailId = Conversion.ToInt(r4.Cells["ContractOtherChargesDetailId"].Value);
				vd4.ChargesItemId = Conversion.ToInt(r4.Cells["ChargesItemId"].Value);
				vd4.AddAmount = Conversion.ToDouble(r4.Cells["AddAmount"].Value);
				vd4.LessAmount = Conversion.ToDouble(r4.Cells["LessAmount"].Value);
				vd4.Remarks = Conversion.ToString(r4.Cells["Remarks"].Value);
				FormHelper.ValidateField(vd4.ChargesItemId, "Charges Item", r4.RowIndex, "Other Charges");
				if (vd4.AddAmount == 0.0 && vd4.LessAmount == 0.0)
				{
					throw new Exception($"AddAmount Or Less Amount is required in Other Charges at row No: {r4.RowIndex + 1}");
				}
				PIH.ExImInvoiceOtherChargesDetailList.Add(vd4);
			}
			rows = grdOtherChargesDetailCustom.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				ExImInvoiceOtherChargesDetailCustom vd3 = new ExImInvoiceOtherChargesDetailCustom();
				vd3.ChargesItemId = Conversion.ToInt(r3.Cells["ChargesItemId"].Value);
				vd3.AddAmount = Conversion.ToDouble(r3.Cells["AddAmount"].Value);
				vd3.LessAmount = Conversion.ToDouble(r3.Cells["LessAmount"].Value);
				vd3.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
				FormHelper.ValidateField(vd3.ChargesItemId, "Charges Item", r3.RowIndex, "Other Charges Custom");
				if (vd3.AddAmount == 0.0 && vd3.LessAmount == 0.0)
				{
					throw new Exception($"AddAmount Or Less Amount is required in Other Charges Custom at row No: {r3.RowIndex + 1}");
				}
				PIH.ExImInvoiceOtherChargesDetailCustomList.Add(vd3);
			}
			rows = grdPackListDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ExImInvoicePackingListDetail vd2 = new ExImInvoicePackingListDetail();
				vd2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				vd2.ContainerId = Conversion.ToInt(r2.Cells["ContainerId"].Value);
				vd2.ContainerNo = Conversion.ToString(r2.Cells["ContainerNo"].Value);
				vd2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				vd2.ItemName = Conversion.ToString(r2.Cells["ItemName"].Value);
				vd2.ItemCode = Conversion.ToString(r2.Cells["ItemCode"].Value);
				vd2.CommodityDetail = Conversion.ToString(r2.Cells["ItemCommodityDetail"].Value);
				vd2.PmDetail = Conversion.ToString(r2.Cells["ItemPmDetail"].Value);
				vd2.PackageDate = Conversion.ToDateTime(r2.Cells["PackageDate"].Value);
				vd2.ExpiryDate = Conversion.ToDateTime(r2.Cells["ExpiryDate"].Value);
				vd2.LotNo = Conversion.ToString(r2.Cells["LotNo"].Value);
				vd2.ThirdPartyAnalysisId = Conversion.ToInt(r2.Cells["LabInspectionId"].Value);
				vd2.ThirdPartyAnalysisNo = Conversion.ToString(r2.Cells["LabInspectionNo"].Value);
				vd2.SubLotNo = Conversion.ToString(r2.Cells["SubLotNo"].Value);
				vd2.OuterQty = Conversion.ToDouble(r2.Cells["OuterQty"].Value);
				vd2.NetWeight = Conversion.ToDouble(r2.Cells["NetWeight"].Value);
				vd2.GrossWeight = Conversion.ToDouble(r2.Cells["GrossWeight"].Value);
				vd2.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
				FormHelper.ValidateField(vd2.ContainerId, "Container", r2.RowIndex, "Packing List");
				FormHelper.ValidateField(vd2.ItemId, "Item", r2.RowIndex, "Packing List");
				FormHelper.ValidateField(vd2.CommodityDetail, "Commodity Detail", r2.RowIndex, "Packing List");
				FormHelper.ValidateField(vd2.PmDetail, "Pm Detail", r2.RowIndex, "Packing List");
				FormHelper.ValidateField(vd2.PackageDate, "Package Date", r2.RowIndex, "Packing List");
				FormHelper.ValidateField(vd2.ExpiryDate, "Expiry Date", r2.RowIndex, "Packing List");
				FormHelper.ValidateField(vd2.OuterQty, "Outer Qty", r2.RowIndex, "Packing List");
				FormHelper.ValidateField(vd2.NetWeight, "Net Weight", r2.RowIndex, "Packing List");
				FormHelper.ValidateField(vd2.GrossWeight, "Gross Weight", r2.RowIndex, "Packing List");
				PIH.ExImInvoicePackingListDetailList.Add(vd2);
			}
			rows = grdotheritems.GetRows();
			foreach (GridEXRow r in rows)
			{
				ExImInvoiceOtherItems vd = new ExImInvoiceOtherItems();
				vd.otherItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.oItemQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
				vd.oItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
				vd.oItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
				vd.OtherItemRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
				FormHelper.ValidateField(vd.otherItemId, "Item Name", r.RowIndex, "Other Item");
				FormHelper.ValidateField(vd.oItemQty, "Qty", r.RowIndex, "Other Item");
				FormHelper.ValidateField(vd.oItemRate, "Rate", r.RowIndex, "Other Item");
				FormHelper.ValidateField(vd.oItemAmount, "Amount", r.RowIndex, "Other Item");
				PIH.ExImInvoiceOtherItemslist.Add(vd);
			}
			if (!string.IsNullOrEmpty(ContractIds))
			{
				string[] distinctContractIds = ContractIds.Split(',').Distinct().ToArray();
				if (distinctContractIds.Length == 1)
				{
					PIH.ContractIds = distinctContractIds[0];
				}
				else
				{
					PIH.ContractIds = string.Join(",", distinctContractIds).TrimStart(',');
				}
			}
			if (!string.IsNullOrEmpty(ContractDetailIds))
			{
				string[] distinctContractDetailIds = ContractDetailIds.Split(',').Distinct().ToArray();
				if (distinctContractDetailIds.Length == 1)
				{
					PIH.ContractDetailIds = distinctContractDetailIds[0];
				}
				else
				{
					PIH.ContractDetailIds = string.Join(",", distinctContractDetailIds).TrimStart(',');
				}
			}
			if (!string.IsNullOrEmpty(ContractNos))
			{
				string[] distinctContractNos = ContractNos.Split(',').Distinct().ToArray();
				if (distinctContractNos.Length == 1)
				{
					PIH.ContractNos = distinctContractNos[0];
				}
				else
				{
					PIH.ContractNos = string.Join(",", distinctContractNos).TrimStart(',');
				}
			}
			if (!string.IsNullOrEmpty(ContractDates))
			{
				string[] distinctContractDates = ContractDates.Split(',').Distinct().ToArray();
				if (distinctContractDates.Length == 1)
				{
					PIH.ContractDates = distinctContractDates[0];
				}
				else
				{
					PIH.ContractDates = string.Join(",", distinctContractDates).TrimStart(',');
				}
			}
			if (!string.IsNullOrEmpty(ContractScheduleIds))
			{
				string[] distinctContractScheduleIds = ContractScheduleIds.Split(',').Distinct().ToArray();
				if (distinctContractScheduleIds.Length == 1)
				{
					PIH.ContractScheduleIds = distinctContractScheduleIds[0];
				}
				else
				{
					PIH.ContractScheduleIds = string.Join(",", distinctContractScheduleIds).TrimStart(',');
				}
			}
			if (!string.IsNullOrEmpty(ContractScheduleNos))
			{
				string[] distinctScheduleNos = ContractScheduleNos.Split(',').Distinct().ToArray();
				if (distinctScheduleNos.Length == 1)
				{
					PIH.ContractScheduleNos = distinctScheduleNos[0];
				}
				else
				{
					PIH.ContractScheduleNos = string.Join(",", distinctScheduleNos).TrimStart(',');
				}
			}
			PIH.PaymentTermId = PIH.ExImInvoicePaymentTermsDetail[0].PaymentTermId;
			double AmountOtherItemGrid = GridEX_Helper.GetColumnSum(grdotheritems, "Amount");
			PIH.TotalAmount = TotalFcAmount + PIH.AddLessAmount + AmountOtherItemGrid;
			PIH.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			PIH.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExImInvoice>(PIH, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			PIH.AttachmentsValues = AttachmentsValues;
			PIH.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = ExImInvoice.Save(PIH);
			if (RecId == 0)
			{
				MessageBox.Show("Save SuccessFully");
			}
			if (RecId > 0)
			{
				MessageBox.Show("Update SuccessFully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, PIH.DocumentTypeId, ScreenName);
			}
			FormReset();
			if (ChkPrintPreview.Checked)
			{
				CommonServices.CommercialInvoiceSlip521(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Rec Id not found...");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(dynamic vd, GridEXRow r)
	{
		vd.ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
		vd.ContractDetailId = Conversion.ToInt(r.Cells["ContractDetailId"].Value);
		vd.PriRefNo = Conversion.ToString(r.Cells["ContractNo"].Value);
		vd.ProformaDocDate = Conversion.ToDateTime(r.Cells["ContractDate"].Value);
		vd.ContractNoShipmentWise = Conversion.ToString(r.Cells["ContractNoInvoiceWise"].Value);
		vd.ContractScheduleId = Conversion.ToInt(r.Cells["ContractScheduleId"].Value);
		vd.ContractScheduleNo = Conversion.ToString(r.Cells["ContractSchedule"].Value);
		vd.ContractScheduleDetailId = Conversion.ToInt(r.Cells["ContractScheduleDetailId"].Value);
		vd.ItemCategoryId = Conversion.ToInt(r.Cells["ItemCategoryId"].Value);
		vd.ItemCategory = Conversion.ToString(r.Cells["ItemCategory"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
		vd.CropYear = Conversion.ToString((object)r.Cells["CropYearId"].Text);
		vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		vd.JobLotDescription = Conversion.ToString((object)r.Cells["JobLotId"].Text);
		vd.PackingMaterialTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
		vd.PackMaterilaType = Conversion.ToString((object)r.Cells["PackingTypeId"].Text);
		vd.PmItemId = Conversion.ToInt(r.Cells["PmItemId"].Value);
		vd.PmItemCode = Conversion.ToString(r.Cells["PmItemCode"].Value);
		vd.PmItemName = Conversion.ToString(r.Cells["PmItemName"].Value);
		vd.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
		vd.BrandCode = Conversion.ToString(r.Cells["BrandCode"].Value);
		vd.BrandName = Conversion.ToString(r.Cells["BrandName"].Value);
		vd.MTon = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
		vd.NetWeight = vd.MTon * 1000;
		vd.OuterQtyUomId = Conversion.ToInt(r.Cells["OuterUomId"].Value);
		vd.OuterUOM = Conversion.ToString(r.Cells["OuterUom"].Value);
		vd.OuterEquivalent = Conversion.ToDouble(r.Cells["OuterEquivalent"].Value);
		vd.OuterQtyEquivalent = Conversion.ToDouble(r.Cells["OuterQtyEquivalent"].Value);
		vd.OuterQty = Conversion.ToDouble(r.Cells["OuterQty"].Value);
		vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
		vd.InnerQtyUomId = Conversion.ToInt(r.Cells["InnerUomId"].Value);
		vd.InnerUOM = Conversion.ToString(r.Cells["InnerUom"].Value);
		vd.InnerQty = Conversion.ToDouble(r.Cells["InnerQty"].Value);
		vd.InnerEquivalent = Conversion.ToDouble(r.Cells["InnerEquivalent"].Value);
		vd.PalletQty = Conversion.ToDouble(r.Cells["PalletQty"].Value);
		vd.RateWithoutAddLess = Conversion.ToDouble(r.Cells["RateWithoutAddLess"].Value);
		vd.RateAddLess = Conversion.ToDouble(r.Cells["RateAddLess"].Value);
		vd.RatePrice = Conversion.ToDouble(r.Cells["NetRate"].Value);
		vd.ContractRate = Conversion.ToDouble(r.Cells["ContractRate"].Value);
		vd.RateUomId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
		vd.RateUOM = Conversion.ToString(r.Cells["RateUOM"].Value);
		vd.RateEquivalent = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
		vd.FcAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
		vd.LabInspectionId = Conversion.ToInt(r.Cells["LabInspectionId"].Value);
		vd.BuyerLotNo = Conversion.ToString((object)r.Cells["BuyerLotNo"].Text);
		vd.ProductCode = Conversion.ToString((object)r.Cells["ProductCode"].Text);
		vd.BuyerHSCode = Conversion.ToString((object)r.Cells["BuyerHSCode"].Text);
		vd.HsCode = Conversion.ToString((object)r.Cells["HSCode"].Text);
		vd.ItemCommodityDetail = Conversion.ToString((object)r.Cells["CommodityDetail"].Text);
		vd.OuterPackDescription = Conversion.ToString((object)r.Cells["PackingDetail"].Text);
		vd.ItemDescriptionManual = Conversion.ToString((object)r.Cells["PackingDetail"].Text);
		vd.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
		vd.OtherAmount = Conversion.ToDouble((object)r.Cells["OtherItemAmount"].Text);
		vd.ScheduleFCL = Conversion.ToDouble((object)r.Cells["ScheduleFCL"].Text);
		if (vd.GrossWeight == 0)
		{
			vd.GrossWeight = vd.NetWeight;
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			FormReset();
			RecId = ID;
			ExImInvoice Obj = ExImInvoice.GetByID(ID);
			if (Obj != null)
			{
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnupdate.Visible = true;
				UpdateMode = true;
				txtDocDate.Focus();
				CmbExportCompany.Value = Obj.ExportCompanyId;
				txtdocno.Text = Conversion.ToString((object)Obj.DocCode);
				txtDocDate.Value = Conversion.ToDateTime((object)Obj.DocDate);
				InvoiceNoBind(InvoiceNoDbCall(Obj.InvoiceNo));
				((Control)(object)cmbInvoiceNo).Text = Conversion.ToString((object)Obj.InvoiceNo);
				txtDocumentaryCreditNo.Text = Conversion.ToString((object)Obj.DocumentaryCreditNo);
				txtDocumentaryCreditNoDate.Value = Conversion.ToDateTime((object)Obj.DocumentaryCreditNoIssueDate);
				CmbCustomer.Value = Obj.SupplierCustomerId;
				cmbSupCust_Leave(null, null);
				if (Obj.ConsigneeId > 0)
				{
					CmbConsignee.Value = Obj.ConsigneeId;
				}
				if (!string.IsNullOrWhiteSpace(Obj.CustomerContractNos))
				{
					txtCustomerContractsNos.Text = Obj.CustomerContractNos;
				}
				if (!string.IsNullOrWhiteSpace(Obj.ContractDates))
				{
					txtCustomerContractsDates.Text = Obj.ContractDates;
				}
				if (Obj.NotifyParty1 > 0)
				{
					CmbNotifyParty1.Value = Obj.NotifyParty1;
				}
				if (Obj.NotifyParty2 > 0)
				{
					CmbNotifyParty2.Value = Obj.NotifyParty2;
				}
				if (Obj.NotifyParty3 > 0)
				{
					CmbNotifyParty3.Value = Obj.NotifyParty3;
				}
				if (Obj.ImporterBankId > 0)
				{
					CmbImporterBank.Value = Obj.ImporterBankId;
				}
				if (Obj.ExporteBankId > 0)
				{
					CmbExporterBank.Value = Obj.ExporteBankId;
				}
				if (!string.IsNullOrWhiteSpace(Obj.LotNoRef))
				{
					txtLotRefHeader.Text = Obj.LotNoRef;
				}
				if (!string.IsNullOrWhiteSpace(Obj.GDNo))
				{
					txtGdNo.Text = Obj.GDNo;
				}
				if (Obj.GDDate.HasValue)
				{
					txtGdDate.Value = Conversion.ToDateTime((object)Obj.GDDate);
				}
				if (Obj.LoadingPortId > 0)
				{
					CmbLoadingPort.Value = Obj.LoadingPortId;
				}
				if (Obj.DestinationPortId > 0)
				{
					CmbDestinationPort.Value = Obj.DestinationPortId;
				}
				if (!string.IsNullOrWhiteSpace(Obj.CarierType))
				{
					((Control)(object)CmbCareierType).Text = Obj.CarierType;
				}
				if (Obj.DeliveryTermId > 0)
				{
					CmbDeliveryTerm.Value = Obj.DeliveryTermId;
				}
				if (!string.IsNullOrWhiteSpace(Obj.DeliveryRemarks))
				{
					txtDeliveryRemarks.Text = Obj.DeliveryRemarks;
				}
				if (!string.IsNullOrWhiteSpace(Obj.TCPRegNo))
				{
					txtTcpRegNo.Text = Obj.TCPRegNo;
				}
				if (Obj.ContinentId > 0)
				{
					CmbContinent.Value = Obj.ContinentId;
				}
				if (Obj.OriginCountryId > 0)
				{
					CmbOriginCountry.Value = Obj.OriginCountryId;
				}
				if (Obj.ImporterCountryId > 0)
				{
					CmbImporterCountry.Value = Obj.ImporterCountryId;
				}
				if (Obj.PlaceOfDeliveryId > 0)
				{
					CmbPlaceOfDelivery.Value = Obj.PlaceOfDeliveryId;
				}
				if (Obj.FcurrencyId > 0)
				{
					CmbFcyCode.Value = Obj.FcurrencyId;
				}
				txtFcyAmount.Text = Obj.FCurrencyAmount.ToString("#,##0.####");
				txtExRate.Text = Obj.ConversionRate.ToString("#,##0.####");
				txtLocalAmt.Text = Obj.EquivalentAmount.ToString("#,##0.####");
				txtaddlesscommnets.Text = Obj.AddLessComments;
				txtaddlessamount.Text = Obj.AddLessAmount.ToString();
				txtTotalNetAmount.Text = Obj.TotalAmount.ToString("#,##0.####");
				txtNoOfContainer.Text = Obj.NoOfContainers.ToString();
				txtGrossWeight.Text = ((Obj.GrossMton > 0m) ? Obj.GrossMton.ToString("#,##0.####") : (Obj.GrossWeight / 1000.0).ToString("#,##0.####"));
				txtNetWeight.Text = ((Obj.NetMton > 0m) ? Obj.NetMton.ToString("#,##0.####") : (Obj.NetWeight / 1000.0).ToString("#,##0.####"));
				txtCertificate1.Text = Obj.Certificate1;
				txtCertificateOfOrigion.Text = Obj.Certificate2;
				txtRemarks1.Text = Obj.Remarks1;
				txtRemarks2.Text = Obj.Remarks2;
				txtOtherRemarks1.Text = Obj.OtherRemarks1;
				txtOtherRemarks2.Text = Obj.OtherRemarks2;
				AttachmentsValues = Obj.AttachmentsValues;
				CustomAttachmentsValues = Obj.CustomAttachmentsValues;
				ContainerNoFromCROByInvoiceIdBind(ExImInvoice.GetContainerNoFromShipingLineBookingDetailByInvoiceId(UserAccount.OrganizationId, UserAccount.CompanyId, RecId, 0));
				dtdetail.Rows.Clear();
				FillDetailFromListCommonForReadById(Obj, dtdetail);
				dtPaymnentDetail.Rows.Clear();
				FillPaymentDetailFromListCommonForReadById(Obj, dtPaymnentDetail);
				dtOtherChargesDetail.Rows.Clear();
				FillOtherChargesDetailFromListCommonForReadById(Obj, dtOtherChargesDetail);
				dtOtherItem.Rows.Clear();
				FillOtherItemDetailFromListCommonForReadById(Obj, dtOtherItem);
				dtdetailCustom.Rows.Clear();
				FillDetailFromListCommonForReadById(Obj, dtdetailCustom, AddExtraColumns: false, IsCustom: true);
				dtPaymnentDetailCustom.Rows.Clear();
				FillPaymentDetailFromListCommonForReadById(Obj, dtPaymnentDetailCustom, IsCustom: true);
				dtOtherChargesDetailCustom.Rows.Clear();
				FillOtherChargesDetailFromListCommonForReadById(Obj, dtOtherChargesDetailCustom, IsCustom: true);
				FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
				BindGrids();
				ItemNamePackListBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(ExImInvoice invGrn, DataTable dt, bool AddExtraColumns = false, bool IsCustom = false)
	{
		if (invGrn == null)
		{
			return;
		}
		IEnumerable<object> details = ((!IsCustom) ? invGrn.ExImInvoiceDetail?.Cast<object>() : invGrn.ExImInvoiceDetailCustom?.Cast<object>());
		if (details == null || !details.Any())
		{
			return;
		}
		foreach (dynamic detail in details)
		{
			List<object> list = new List<object>();
			list.Add(detail.Id);
			list.Add(detail.ContractId);
			list.Add(detail.ContractDetailId);
			list.Add(detail.PriRefNo);
			list.Add(detail.ProformaDocDate);
			list.Add(detail.ContractNoShipmentWise);
			list.Add(detail.ContractScheduleId);
			list.Add(detail.ContractScheduleNo);
			list.Add(detail.ContractScheduleDetailId);
			list.Add(detail.ItemCategoryId);
			list.Add(detail.ItemCategory);
			list.Add(detail.ItemId);
			list.Add(detail.ItemCode);
			list.Add(detail.ItemName);
			list.Add(detail.CropYearId);
			list.Add(detail.JobLotId);
			list.Add(detail.PackingMaterialTypeId);
			list.Add(detail.PmItemId);
			list.Add(detail.PmItemCode);
			list.Add(detail.PmItemName);
			list.Add(detail.BrandId);
			list.Add(detail.BrandCode);
			list.Add(detail.BrandName);
			list.Add(detail.NetWeight / 1000);
			list.Add(detail.OuterQtyUomId);
			list.Add(detail.OuterUOM);
			list.Add(detail.OuterEquivalent);
			list.Add(detail.OuterQtyEquivalent);
			list.Add(detail.OuterQty);
			list.Add(detail.InnerQtyUomId);
			list.Add(detail.InnerUOM);
			list.Add(detail.InnerEquivalent);
			list.Add(detail.InnerQty);
			list.Add(detail.PalletQty);
			list.Add(detail.GrossWeight);
			list.Add(detail.RateWithoutAddLess);
			list.Add(detail.RateAddLess);
			list.Add(detail.RatePrice);
			list.Add(detail.ContractRate);
			list.Add(detail.RateUomId);
			list.Add(detail.RateUOM);
			list.Add(detail.RateEquivalent);
			list.Add(detail.FcAmount);
			list.Add(detail.LabInspectionId);
			list.Add(detail.BuyerLotNo);
			list.Add(detail.ProductCode);
			list.Add(detail.BuyerHSCode);
			list.Add(detail.HsCode);
			list.Add(detail.ItemCommodityDetail);
			list.Add(detail.OuterPackDescription);
			list.Add(detail.Remarks);
			list.Add(detail.OtherAmount);
			list.Add(detail.ScheduleFCL);
			List<object> rowValues = list;
			if (AddExtraColumns)
			{
				rowValues.Add(detail.CropYear);
				rowValues.Add(detail.JobLotDescription);
				rowValues.Add(detail.PackMaterilaType);
			}
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillPaymentDetailFromListCommonForReadById(ExImInvoice invGrn, DataTable dt, bool IsCustom = false)
	{
		if (invGrn == null)
		{
			return;
		}
		IEnumerable<object> details = ((!IsCustom) ? invGrn.ExImInvoicePaymentTermsDetail?.Cast<object>() : invGrn.ExImInvoicePaymentTermsDetailCustom?.Cast<object>());
		if (details == null || !details.Any())
		{
			return;
		}
		foreach (dynamic detail in details)
		{
			List<object> list = new List<object>();
			list.Add(detail.Id);
			list.Add(detail.PaymentTermId);
			list.Add(detail.PaymentTerm);
			list.Add(detail.DocumentTypeId);
			list.Add(detail.ExImEFormRegistrationId);
			list.Add(detail.FinancialInstrumentNo);
			list.Add(detail.PrcntOfTotal);
			list.Add(detail.FcyAmount);
			list.Add(detail.DueDays);
			list.Add(detail.PaymentRemarks);
			List<object> rowValues = list;
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillOtherChargesDetailFromListCommonForReadById(ExImInvoice invGrn, DataTable dt, bool IsCustom = false)
	{
		if (invGrn == null)
		{
			return;
		}
		IEnumerable<object> details = ((!IsCustom) ? invGrn.ExImInvoiceOtherChargesDetailList?.Cast<object>() : invGrn.ExImInvoiceOtherChargesDetailCustomList?.Cast<object>());
		if (details == null || !details.Any())
		{
			return;
		}
		foreach (dynamic detail in details)
		{
			if (IsCustom)
			{
				List<object> list = new List<object>();
				list.Add(detail.Id);
				list.Add(detail.ChargesItemId);
				list.Add(detail.ChargesItemName);
				list.Add(detail.AddAmount);
				list.Add(detail.LessAmount);
				list.Add(detail.Remarks);
				List<object> rowValues = list;
				dt.Rows.Add(rowValues.ToArray());
			}
			else
			{
				List<object> list = new List<object>();
				list.Add(detail.Id);
				list.Add(detail.ContractId);
				list.Add(detail.ContractOtherChargesDetailId);
				list.Add(detail.ChargesItemId);
				list.Add(detail.ChargesItemName);
				list.Add(detail.AddAmount);
				list.Add(detail.LessAmount);
				list.Add(detail.Remarks);
				List<object> rowValues2 = list;
				dt.Rows.Add(rowValues2.ToArray());
			}
		}
	}

	private void FillPackingListDetailFromListCommonForReadById(ExImInvoice invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.ExImInvoicePackingListDetailList : null) == null || invGrn.ExImInvoicePackingListDetailList.Count == 0)
		{
			return;
		}
		foreach (ExImInvoicePackingListDetail detail in invGrn.ExImInvoicePackingListDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.ContainerId, detail.ContainerNo, detail.ItemId, detail.ItemCode, detail.ItemName, detail.CommodityDetail, detail.PmDetail, detail.PackageDate, detail.ExpiryDate,
				detail.LotNo, detail.ThirdPartyAnalysisId, detail.ThirdPartyAnalysisNo, detail.SubLotNo, detail.OuterQty, detail.NetWeight, detail.GrossWeight, detail.Remarks
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillOtherItemDetailFromListCommonForReadById(ExImInvoice invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.ExImInvoiceOtherItemslist : null) == null || invGrn.ExImInvoiceOtherItemslist.Count == 0)
		{
			return;
		}
		foreach (ExImInvoiceOtherItems detail in invGrn.ExImInvoiceOtherItemslist)
		{
			List<object> rowValues = new List<object> { detail.otherItemId, detail.ItemName, detail.oItemQty, detail.oItemRate, detail.oItemAmount, detail.OtherItemRemarks };
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FormReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			LstRemoveRecordDetail.Clear();
			LstRemoveRecordDetailCustom.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			btnsave.Visible = true;
			btnupdate.Visible = false;
			RecId = 0;
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			txtDocumentaryCreditNo.Text = string.Empty;
			((Control)(object)CmbCustomer).Text = string.Empty;
			cmbSupCust_Leave(null, null);
			((Control)(object)CmbConsignee).Text = string.Empty;
			txtCustomerContractsNos.Text = string.Empty;
			txtCustomerContractsDates.Text = string.Empty;
			((Control)(object)CmbNotifyParty1).Text = string.Empty;
			((Control)(object)CmbNotifyParty2).Text = string.Empty;
			((Control)(object)CmbNotifyParty3).Text = string.Empty;
			((Control)(object)CmbImporterBank).Text = string.Empty;
			((Control)(object)CmbExporterBank).Text = string.Empty;
			txtLotRefHeader.Text = string.Empty;
			txtGdNo.Text = string.Empty;
			((Control)(object)CmbLoadingPort).Text = string.Empty;
			((Control)(object)CmbDestinationPort).Text = string.Empty;
			((UltraGridBase)CmbCareierType).Rows[1].Activate();
			((Control)(object)CmbDeliveryTerm).Text = string.Empty;
			txtDeliveryRemarks.Text = string.Empty;
			txtTcpRegNo.Text = string.Empty;
			((Control)(object)CmbContinent).Text = string.Empty;
			((Control)(object)CmbOriginCountry).Text = string.Empty;
			((Control)(object)CmbImporterCountry).Text = string.Empty;
			((Control)(object)CmbPlaceOfDelivery).Text = string.Empty;
			((Control)(object)CmbFcyCode).Text = string.Empty;
			txtExRate.Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			txtLocalAmt.Text = string.Empty;
			txtaddlessamount.Text = string.Empty;
			txtaddlesscommnets.Text = string.Empty;
			txtNoOfContainer.Text = string.Empty;
			txtTotalNetAmount.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtCertificate1.Text = string.Empty;
			txtCertificateOfOrigion.Text = string.Empty;
			txtRemarks1.Text = string.Empty;
			txtRemarks2.Text = string.Empty;
			txtOtherRemarks1.Text = string.Empty;
			txtOtherRemarks2.Text = string.Empty;
			dtdetail.Rows.Clear();
			ResetPaymentDetail();
			dtPaymnentDetail.Rows.Clear();
			ResetOtherChargesDetail();
			dtOtherChargesDetail.Rows.Clear();
			dtPackingListDetail.Rows.Clear();
			dtBrandForPMItem.Clear();
			((UltraGridBase)CmbItemFromInvDetailPackListDetail).DataSource = null;
			ResetPackListDetail();
			ResetOtherItems();
			dtOtherItem.Clear();
			dtdetailCustom.Rows.Clear();
			ResetPaymentDetailCustom();
			dtPaymnentDetailCustom.Rows.Clear();
			ResetOtherChargesDetailCustom();
			dtOtherChargesDetailCustom.Rows.Clear();
			BindGrids();
			UpdateDocumentNoUI(GetDocumentCode());
			InvoiceNoBind(InvoiceNoDbCall());
			tabControl4.SelectedIndex = 0;
			tabControl2.SelectedIndex = 0;
			txtDocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetPaymentDetail()
	{
		try
		{
			updateDetailIndexPT = -1;
			((Control)(object)CmbPaymentTermPTDetail).Text = string.Empty;
			((Control)(object)CmbFinInstrumentsPTDetail).Text = string.Empty;
			txtPrcntOfTotalPTDetail.Text = string.Empty;
			txtFcyAmountPTDetail.Text = string.Empty;
			txtDueDaysPTDetail.Text = string.Empty;
			txtRemarksPTDetail.Text = string.Empty;
			BtnAddPTDetail.Visible = true;
			BtnUpdatePTDetail.Visible = false;
			BtnCancelPTDetail.Visible = false;
			CmbPaymentTermPTDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetOtherChargesDetail()
	{
		try
		{
			updateDetailIndexOtherCharges = -1;
			((Control)(object)CmbChargesNameChargeDetail).Text = string.Empty;
			txtAddAmountChargeDetail.Text = string.Empty;
			txtLessAmountChargeDetail.Text = string.Empty;
			txtRemarksChargeDetail.Text = string.Empty;
			BtnAddChargeDetail.Visible = true;
			BtnUpdateChargeDetail.Visible = false;
			BtnCancelChargeDetail.Visible = false;
			CmbChargesNameChargeDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetPackListDetail()
	{
		try
		{
			updateDetailIndexPackList = -1;
			((Control)(object)CmbContainerNoPackListDetail).Text = string.Empty;
			((Control)(object)CmbItemFromInvDetailPackListDetail).Text = string.Empty;
			((Control)(object)CmbCommodityByItemCusPackListDetail).Text = string.Empty;
			((UltraGridBase)CmbCommodityByItemCusPackListDetail).DataSource = null;
			txtCommodityPackListDetail.Text = string.Empty;
			((Control)(object)CmbPmDetailByItemCusPackListDetail).Text = string.Empty;
			((UltraGridBase)CmbPmDetailByItemCusPackListDetail).DataSource = null;
			txtPmDetailPackListDetail.Text = string.Empty;
			txtLotNoPackListDetail.Text = string.Empty;
			((Control)(object)CmbInspectionNoPackListDetail).Text = string.Empty;
			txtSubLotNoPackListDetail.Text = string.Empty;
			txtOuterQtyPackListDetail.Text = string.Empty;
			txtNetWeightPackListDetail.Text = string.Empty;
			txtGrossWeightPackListDetail.Text = string.Empty;
			txtRemarksPackListDetail.Text = string.Empty;
			BtnAddPackListDetail.Visible = true;
			BtnUpdatePackListDetail.Visible = false;
			BtnCancelPackListDetail.Visible = false;
			CmbContainerNoPackListDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetOtherItems()
	{
		try
		{
			updateDetailIndexOtherItem = -1;
			((Control)(object)CmbItemOIDetail).Text = string.Empty;
			txtQtyOIDetail.Text = string.Empty;
			txtRateOIDetail.Text = string.Empty;
			txtAmountOIDetail.Text = string.Empty;
			txtRemarksOIDetail.Text = string.Empty;
			BtnAddOIDetail.Visible = true;
			BtnUpdateOIDetail.Visible = false;
			BtnCancelOIDetail.Visible = false;
			CmbItemOIDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetOtherChargesDetailCustom()
	{
		try
		{
			updateDetailIndexOtherChargesCustom = -1;
			((Control)(object)CmbChargesNameChargeDetailCustom).Text = string.Empty;
			txtAddAmountChargeDetailCustom.Text = string.Empty;
			txtLessAmountChargeDetailCustom.Text = string.Empty;
			txtRemarksChargeDetailCustom.Text = string.Empty;
			BtnAddChargeDetailCustom.Visible = true;
			BtnUpdateChargeDetailCustom.Visible = false;
			BtnCancelChargeDetailCustom.Visible = false;
			CmbChargesNameChargeDetailCustom.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetPaymentDetailCustom()
	{
		try
		{
			updateDetailIndexPTCustom = -1;
			((Control)(object)CmbPaymentTermPTDetailCustom).Text = string.Empty;
			((Control)(object)CmbFinInstrumentsPTDetailCustom).Text = string.Empty;
			txtPrcntOfTotalPTDetailCustom.Text = string.Empty;
			txtFcyAmountPTDetailCustom.Text = string.Empty;
			txtDueDaysPTDetailCustom.Text = string.Empty;
			txtRemarksPTDetailCustom.Text = string.Empty;
			BtnAddPTDetailCustom.Visible = true;
			BtnUpdatePTDetailCustom.Visible = false;
			BtnCancelPTDetailCustom.Visible = false;
			CmbPaymentTermPTDetailCustom.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmCommercialInvoiceIII_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "AllCities", "CropYear", "JobLot", "InvPackingType", "AllBrands" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			if (AllowExportMultiCompanies)
			{
				((Control)(object)CmbExportCompany).Visible = true;
				labelExportCompany.Visible = true;
				ExportCompaniesBind(ExImLcOrder.GetExportCompaniesByCompany(UserAccount.OrganizationId, UserAccount.CompanyId));
			}
			else
			{
				((Control)(object)CmbExportCompany).Visible = false;
				labelExportCompany.Visible = false;
			}
			InvoiceNoBind(InvoiceNoDbCall());
			SupplierDtFillFromGlobal();
			BindSupplierName();
			ConsigneeAgainstCustomerBind(Conversion.ToInt(CmbCustomer.Value));
			ImporterAndExportBankBind(BanksDbCall());
			PortsBind(PortsDbCall());
			DeliveryTermBind(ExImDeliveryTerm.Getall());
			ContinentBind(Continent.ComboBind());
			OriginAndImportCountryBind(CountryDtDbCall());
			CityDtFillFromGlobalAndBind();
			MultiCurrencyBind(MultiCurrency.GetMultiCurrencywithExchangeRate(UserAccount.CompanyId));
			ItemDtsFillFromGlobal();
			CropJobDtFillFromGlobalAndBind();
			BrandDtFillFromGlobal();
			PaymentTermsBind(PaymentTermsDbCall());
			FinancialInstrumentDtFillDbCall(Conversion.ToInt(CmbCustomer.Value));
			FinancialInstrumentBind();
			ChargesNameChargeDetailBind(ExportCharges.ComboBind(UserAccount.OrganizationId, UserAccount.CompanyId));
			ContainerNoFromCROByInvoiceIdBind(ExImInvoice.GetContainerNoFromShipingLineBookingDetailByInvoiceId(UserAccount.OrganizationId, UserAccount.CompanyId, RecId, 0));
			ItemNamePackListBind();
			OtherItemsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoadd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdotheritems")
			{
				CmbItemOIDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoCancel_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdotheritems")
			{
				CmbItemOIDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndadd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdpaymentdetail")
			{
				CmbPaymentTermPTDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndcancel_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdpaymentdetail")
			{
				CmbPaymentTermPTDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRemarks2_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "")
			{
				tabControl4.SelectedIndex = 0;
				tabControl2.SelectedIndex = 0;
				((Control)(object)grdDetail).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCertificate1_Enter(object sender, EventArgs e)
	{
	}

	private void txtCertificateOfOrigion_Enter(object sender, EventArgs e)
	{
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			txtDocDate.Focus();
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			gridDetailHistory.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(HistoryComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.Ids = DocumentTypeId.ToString();
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			Obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			Obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			Obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			Obj.Id = Conversion.ToInt(CmbInvoiceNoHistory.Value);
			if (rdNotReferred.Checked)
			{
				Obj.ActionId = 2;
			}
			else if (rdReferred.Checked)
			{
				Obj.ActionId = 1;
			}
			else if (rdAll.Checked)
			{
				Obj.ActionId = 0;
			}
			Obj.ApprovedFilter = "All";
			DataTable dtgrd = ExImInvoice.FormHistory(Obj);
			if (dtgrd.Rows.Count > 0)
			{
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id");
					dtHistoryGrid.Columns.Add("InvoiceNo");
					dtHistoryGrid.Columns.Add("DocCode", typeof(int));
					dtHistoryGrid.Columns.Add("DocDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("CustomerName");
					dtHistoryGrid.Columns.Add("NoOfContainers", typeof(double));
					dtHistoryGrid.Columns.Add("GrossWeight", typeof(double));
					dtHistoryGrid.Columns.Add("NetWeight", typeof(double));
					dtHistoryGrid.Columns.Add("FcyAmount", typeof(double));
					dtHistoryGrid.Columns.Add("Add/Less", typeof(double));
					dtHistoryGrid.Columns.Add("TotalAmount", typeof(double));
					dtHistoryGrid.Columns.Add("LoadingPort");
					dtHistoryGrid.Columns.Add("DestinationPort");
					dtHistoryGrid.Columns.Add("EntryUser");
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUser");
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("NoOfAttachments");
				}
				dtHistoryGrid.Clear();
				for (int i = 0; i < dtgrd.Rows.Count; i++)
				{
					dtHistoryGrid.Rows.Add(dtgrd.Rows[i]["Id"], dtgrd.Rows[i]["InvoiceNo"], dtgrd.Rows[i]["DocCode"], Conversion.ToDateTime(dtgrd.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtgrd.Rows[i]["Customer"], dtgrd.Rows[i]["NoOfContainers"], dtgrd.Rows[i]["GrossWeight"], dtgrd.Rows[i]["NetWeight"], dtgrd.Rows[i]["FCurrencyAmount"], dtgrd.Rows[i]["AddLessAmount"], dtgrd.Rows[i]["TotalAmount"], dtgrd.Rows[i]["LoadingPort"], dtgrd.Rows[i]["DestinationPort"], dtgrd.Rows[i]["EntryUserName"], dtgrd.Rows[i]["EntryDate"], dtgrd.Rows[i]["ModifyUserName"], dtgrd.Rows[i]["ModifyDate"], dtgrd.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistoryGrid;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
				gridDetailHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			DataGridHistory.RootTable.Columns["DocCode"].Width = Constants.ExportConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfContainers;
			DataGridHistory.RootTable.Columns["GrossWeight"].Width = Constants.ExportConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["Add/Less"].Width = Constants.ExportConstants.AddLessConstant;
			DataGridHistory.RootTable.Columns["TotalAmount"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["LoadingPort"].Width = Constants.ExportConstants.PortConstant;
			DataGridHistory.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.ModifyUserConstants;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants - 12;
			GridEX_Helper.AddButton(DataGridHistory, "AddAttachment", "AddAttachment", 105);
			ctrlDataGridHistory_Load(null, null);
			if (formrights.DoHavePrintRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Slip", "Slip", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
				GridEX_Helper.AddButton(DataGridHistory, "PrintA", "PrintA", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
				GridEX_Helper.AddButton(DataGridHistory, "PrintB", "PrintB", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
				GridEX_Helper.AddButton(DataGridHistory, "PrintC", "PrintC", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
				GridEX_Helper.AddButton(DataGridHistory, "PrintD", "PrintD", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
			}
			if (formrights.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
			}
			DataGridHistory.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				ReadById(Conversion.ToInt(r.Cells["Id"].Value));
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
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Slip")
				{
					CommonServices.CommercialInvoiceSlip521(Id);
				}
				else if (e.Column.Key == "Edit")
				{
					ReadById(Id);
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, DocumentTypeId, ScreenName);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
				else if (e.Column.Key == "PrintA")
				{
					CommonServices.CommercialInvoiceSlipWithName(Id, "521_A_ExportInvoiceSlip");
				}
				else if (e.Column.Key == "PrintB")
				{
					CommonServices.CommercialInvoiceSlipWithName(Id, "521_B_ExportInvoiceSlip");
				}
				else if (e.Column.Key == "PrintC")
				{
					CommonServices.CommercialInvoiceSlipWithName(Id, "521_C_ExportInvoiceSlip");
				}
				else if (e.Column.Key == "PrintD")
				{
					CommonServices.CommercialInvoiceSlipWithName(Id, "521_D_ExportInvoiceSlip");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			ExImInvoice Obj = ExImInvoice.GetByID(Id);
			if (Obj == null || Obj.ExImInvoiceDetail.Count == 0)
			{
				gridDetailHistory.ClearStructure();
				grdPaymentHistory.ClearStructure();
				grdOtherChargesHistory.ClearStructure();
				grdPackListHistory.ClearStructure();
				grdOtherItemHistory.ClearStructure();
				gridDetailCustomHistory.ClearStructure();
				grdPaymentCustomHistory.ClearStructure();
				grdOtherChargesCustomHistory.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtdetail.Clone();
			dtDetailHistory.Columns.Add("CropYear");
			dtDetailHistory.Columns.Add("JobLot");
			dtDetailHistory.Columns.Add("PackingType");
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory, AddExtraColumns: true);
			gridDetailHistory.DataSource = dtDetailHistory;
			gridDetailHistory.RetrieveStructure();
			grddetailhistorySettings(gridDetailHistory);
			DataTable dtPaymentH = dtPaymnentDetail.Clone();
			FillPaymentDetailFromListCommonForReadById(Obj, dtPaymentH);
			grdPaymentHistory.DataSource = dtPaymentH;
			grdPaymentHistory.RetrieveStructure();
			grdSettingsPaymentTerm(grdPaymentHistory);
			DataTable dtOCH = dtOtherChargesDetail.Clone();
			FillOtherChargesDetailFromListCommonForReadById(Obj, dtOCH);
			grdOtherChargesHistory.DataSource = dtOCH;
			grdOtherChargesHistory.RetrieveStructure();
			grdSettingsOtherChargesDetail(grdOtherChargesHistory);
			DataTable dtPmH = dtPackingListDetail.Clone();
			FillPackingListDetailFromListCommonForReadById(Obj, dtPmH);
			grdPackListHistory.DataSource = dtPmH;
			grdPackListHistory.RetrieveStructure();
			grdSettingsPackListDetaill(grdPackListHistory);
			DataTable dtOtherH = dtOtherItem.Clone();
			FillOtherItemDetailFromListCommonForReadById(Obj, dtOtherH);
			grdOtherItemHistory.DataSource = dtOtherH;
			grdOtherItemHistory.RetrieveStructure();
			grdSettingsOtherItems(grdOtherItemHistory);
			DataTable dtDetailCustomHistory = dtdetailCustom.Clone();
			dtDetailCustomHistory.Columns.Add("CropYear");
			dtDetailCustomHistory.Columns.Add("JobLot");
			dtDetailCustomHistory.Columns.Add("PackingType");
			FillDetailFromListCommonForReadById(Obj, dtDetailCustomHistory, AddExtraColumns: true, IsCustom: true);
			gridDetailCustomHistory.DataSource = dtDetailCustomHistory;
			gridDetailCustomHistory.RetrieveStructure();
			grddetailhistorySettings(gridDetailCustomHistory);
			DataTable dtPaymentCustomH = dtPaymnentDetailCustom.Clone();
			FillPaymentDetailFromListCommonForReadById(Obj, dtPaymentCustomH, IsCustom: true);
			grdPaymentCustomHistory.DataSource = dtPaymentCustomH;
			grdPaymentCustomHistory.RetrieveStructure();
			grdSettingsPaymentTerm(grdPaymentCustomHistory);
			DataTable dtOcCustomH = dtOtherChargesDetailCustom.Clone();
			FillOtherChargesDetailFromListCommonForReadById(Obj, dtOcCustomH, IsCustom: true);
			grdOtherChargesCustomHistory.DataSource = dtOcCustomH;
			grdOtherChargesCustomHistory.RetrieveStructure();
			grdSettingsOtherChargesDetail(grdOtherChargesCustomHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ContractId", "ContractDetailId", "ContractNoInvoiceWise", "ContractScheduleId", "ContractSchedule", "ContractScheduleDetailId", "ItemCategoryId", "ItemId", "PmItemId",
				"BrandId", "OuterUomId", "OuterEquivalent", "OuterQtyEquivalent", "InnerUomId", "InnerEquivalent", "RateUOMId", "RateEquivalent", "CropYearId", "JobLotId",
				"PackingTypeId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3, hiddenColumns);
			grdDetailCommonSetting(grid);
			grid.RootTable.Columns["CropYear"].Position = grid.RootTable.Columns["CropYearId"].Position + 1;
			grid.RootTable.Columns["JobLot"].Position = grid.RootTable.Columns["JobLotId"].Position + 1;
			grid.RootTable.Columns["PackingType"].Position = grid.RootTable.Columns["PackingTypeId"].Position + 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateAddLessAmountInHeaderFromChargesGrid(GridEX grid, TextBox AddLessText)
	{
		try
		{
			double columnSum = GridEX_Helper.GetColumnSum(grid, "AddAmount");
			double LessAmount = GridEX_Helper.GetColumnSum(grid, "LessAmount");
			AddLessText.Text = (columnSum - LessAmount).ToString("#,#.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateHeaderWeightAndAmountFromDetailGrid(bool isCustom)
	{
		try
		{
			GridEX grid = (isCustom ? grdDetailCustom : grdDetail);
			double mTonTotal = GridEX_Helper.GetColumnSum(grid, "Qty/M.Ton");
			double GrossmTonTotal = GridEX_Helper.GetColumnSum(grid, "GrossWeight");
			double amount = GridEX_Helper.GetColumnSum(grid, "Amount");
			double otherAmount = GridEX_Helper.GetColumnSum(grdotheritems, "Amount");
			double addlessAmount = Conversion.ToDouble((object)(isCustom ? txtAddLessHeaderCustom.Text : txtaddlessamount.Text));
			double netTotalAmount = amount + addlessAmount + (isCustom ? 0.0 : otherAmount);
			if (isCustom)
			{
				txtFcyAmountHeaderCustom.Text = amount.ToString("#,##0.###");
				txtTotalFcyAmountHeaderCustom.Text = netTotalAmount.ToString("#,#.###");
			}
			else
			{
				txtNetWeight.Text = mTonTotal.ToString("#,##0.#####");
				txtGrossWeight.Text = ((GrossmTonTotal > 0.0) ? (GrossmTonTotal / 1000.0).ToString("#,##0.#####") : "0");
				txtFcyAmount.Text = amount.ToString("#,##0.###");
				txtTotalNetAmount.Text = netTotalAmount.ToString("#,#.###");
			}
			if (!isCustom && netTotalAmount > 0.0)
			{
				UpdatePaymentTerms(grdPaymentTermDetail, netTotalAmount);
			}
			else if (isCustom && netTotalAmount > 0.0)
			{
				UpdatePaymentTerms(grdPaymentTermDetailCustom, netTotalAmount);
			}
			CalculateLocalAmount(isCustom);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdatePaymentTerms(GridEX grid, double netTotalAmount)
	{
		GridEXRow[] rows = grid.GetRows();
		foreach (GridEXRow obj in rows)
		{
			obj.BeginEdit();
			double percent = Conversion.ToDouble(obj.Cells["%OfTotal"].Value);
			obj.Cells["FcyAmount"].Value = percent * netTotalAmount / 100.0;
			obj.EndEdit();
		}
	}

	private void CalculateLocalAmount(bool isCustom)
	{
		try
		{
			double exchangeRate = Conversion.ToDouble((object)txtExRate.Text);
			double fcyAmount = Conversion.ToDouble((object)(isCustom ? txtTotalFcyAmountHeaderCustom.Text : txtTotalNetAmount.Text));
			double localAmount = ((fcyAmount > 0.0 && exchangeRate > 0.0) ? (fcyAmount * exchangeRate) : 0.0);
			if (isCustom)
			{
				txtLocalAmountHeaderCustom.Text = localAmount.ToString("0,0");
			}
			else
			{
				txtLocalAmt.Text = localAmount.ToString("0,0");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtaddlessamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfcyAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtaddlessamount_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateLocalAmount(isCustom: false);
			CalculateLocalAmount(isCustom: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLessHeaderCustom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			UpdateHeaderWeightAndAmountFromDetailGrid(isCustom: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FinancialInstrumentsBalanceBindWithDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		try
		{
			decimal Balance = ExImEFormRegistration.GetFinancialInstrumentsBalance(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPTDetail).SelectedRow.Cells[3].Value),
				Id = Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPTDetail).SelectedRow.Cells["Id"].Value)
			});
			txtBalanceFIPTDetail.Text = Balance.ToString("0,0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtxtpercentoftotal_TextChanged(object sender, EventArgs e)
	{
		PaymentDetailPercentCalculate();
	}

	private void PaymentDetailPercentCalculate()
	{
		try
		{
			if (Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim()) > 0.0 && Conversion.ToDouble((object)txtPrcntOfTotalPTDetail.Text) > 0.0)
			{
				if (Conversion.ToDouble((object)txtPrcntOfTotalPTDetail.Text) > 100.0)
				{
					txtPrcntOfTotalPTDetail.Text = "0";
					throw new Exception("Percent cannot be greater than 100 Please check");
				}
				double num = Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim());
				double Percent = Conversion.ToDouble((object)txtPrcntOfTotalPTDetail.Text.Trim());
				double FcAmount = num * Percent / 100.0;
				txtFcyAmountPTDetail.Text = FcAmount.ToString("#,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtxtfcyamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtTotalNetAmount.Text) > 0.0 && Conversion.ToDouble((object)txtFcyAmountPTDetail.Text) > 0.0)
			{
				double TotalAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim());
				double Percent = Conversion.ToDouble((object)txtFcyAmountPTDetail.Text.Trim()) / TotalAmount * 100.0;
				txtPrcntOfTotalPTDetail.Text = Percent.ToString();
				if (Conversion.ToDouble((object)txtPrcntOfTotalPTDetail.Text) > 100.0)
				{
					txtPrcntOfTotalPTDetail.Text = "0";
					throw new Exception("Percent cannot be greater than 100 Please check");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomFinancialInstrumentsBalanceBindWithDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		try
		{
			decimal Balance = ExImEFormRegistration.GetFinancialInstrumentsBalance(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPTDetailCustom).SelectedRow.Cells[3].Value),
				Id = Conversion.ToInt(((UltraDropDownBase)CmbFinInstrumentsPTDetailCustom).SelectedRow.Cells["Id"].Value)
			});
			txtBalanceFIPTDetailCustom.Text = Balance.ToString("0,0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPrcntOfTotalPTDetailCustom_TextChanged(object sender, EventArgs e)
	{
		PaymentDetailCustomPercentCalculate();
	}

	private void txtFcyAmountPTDetailCustom_TextChanged(object sender, EventArgs e)
	{
	}

	private void PaymentDetailCustomPercentCalculate()
	{
		try
		{
			if (Conversion.ToDouble((object)txtTotalFcyAmountHeaderCustom.Text.Trim()) > 0.0 && Conversion.ToDouble((object)txtPrcntOfTotalPTDetailCustom.Text) > 0.0)
			{
				if (Conversion.ToDouble((object)txtPrcntOfTotalPTDetailCustom.Text) > 100.0)
				{
					txtPrcntOfTotalPTDetailCustom.Text = "0";
					throw new Exception("Percent cannot be greater than 100 Please check");
				}
				double num = Conversion.ToDouble((object)txtTotalFcyAmountHeaderCustom.Text.Trim());
				double Percent = Conversion.ToDouble((object)txtPrcntOfTotalPTDetailCustom.Text.Trim());
				double FcAmount = num * Percent / 100.0;
				txtFcyAmountPTDetailCustom.Text = FcAmount.ToString("#,#.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOtherItem_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (btnupdate.Visible && btnupdate.Enabled && ((UltraGridBase)CmbItemOIDetail).ActiveRow != null)
			{
				txtRateOIDetail.Text = Conversion.ToString(((UltraDropDownBase)CmbItemOIDetail).SelectedRow.Cells[2].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtoQty_TextChanged(object sender, EventArgs e)
	{
		OtherItemsCalculations();
	}

	private void txtoRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			OtherItemsCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsCalculations()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtQtyOIDetail.Text.Trim());
			double Rate = Conversion.ToDouble((object)txtRateOIDetail.Text.Trim());
			if (Qty > 0.0 && Rate > 0.0)
			{
				txtAmountOIDetail.Text = (Qty * Rate).ToString();
			}
			else
			{
				txtAmountOIDetail.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProportionateOtherItemAmount()
	{
		try
		{
			double ContractAmount = GridEX_Helper.GetColumnSum(grdDetail, "Amount");
			double OtherItemAmount = GridEX_Helper.GetColumnSum(grdotheritems, "Amount");
			if (OtherItemAmount > 0.0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double amount = Conversion.ToDouble(obj.Cells["Amount"].Value);
					double propAmount = OtherItemAmount / ContractAmount * amount;
					obj.Cells["OtherItemAmount"].Value = propAmount;
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["OtherItemAmount"].Value = 0;
				}
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommercialInvoiceSlip521(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint521A_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommercialInvoiceSlipWithName(RecId, "521_A_ExportInvoiceSlip");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint521B_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommercialInvoiceSlipWithName(RecId, "521_B_ExportInvoiceSlip");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint521C_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommercialInvoiceSlipWithName(RecId, "521_C_ExportInvoiceSlip");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint521D_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommercialInvoiceSlipWithName(RecId, "521_D_ExportInvoiceSlip");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlgrdDetail_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdDetail, dtdetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlgrdPaymentTermDetail_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdPaymentTermDetail, dtPaymnentDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlgrdOtherChargesDetail_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdOtherChargesDetail, dtOtherChargesDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlgrdPackListDetail_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdPackListDetail, dtPackingListDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlgrdotheritems_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdotheritems, dtOtherItem);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlgrdDetailCustom_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdDetailCustom, dtdetailCustom);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlgrdOtherChargesDetailCustom_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdOtherChargesDetailCustom, dtOtherChargesDetailCustom);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlgrdPaymentTermDetailCustom_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdPaymentTermDetailCustom, dtPaymnentDetailCustom);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlDataGridHistory_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, DataGridHistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoad_Click(object sender, EventArgs e)
	{
		try
		{
			Loader = new LoadSalesContractForInvoice(UserAccount)
			{
				DocumentTypeId = 223
			};
			if (btnupdate.Visible && btnupdate.Enabled)
			{
				HashSet<int> uniqueScheduleIds = new HashSet<int>(from r in grdDetail.GetDataRows()
					select Conversion.ToInt(r.Cells["ContractScheduleId"].Value));
				Loader.ScheduleIds = string.Join(",", uniqueScheduleIds);
			}
			else
			{
				Loader.ScheduleIds = string.Empty;
			}
			Loader.ShowDialog();
			Loader.BringToFront();
			BindLoaderData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindLoaderData()
	{
		try
		{
			DataTable contract = Loader.LoaderData;
			if (contract.Rows.Count > 0)
			{
				BindAndValidateHeaderDataFromLoader(contract);
				FillDetaildtsFromLoaderData(contract);
				string distinctIds = string.Join(",", (from row in contract.AsEnumerable()
					select row.Field<int>("Id")).Distinct());
				BindPaymentTermDtByContractIdsDBCall(distinctIds);
				BindOtherItemDtByContractIdsDBCall(distinctIds);
				FillOtherchargesDetailByContractIdsDBCall(distinctIds);
				BindGrids();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindAndValidateHeaderDataFromLoader(DataTable dataTable)
	{
		try
		{
			DataRow dr = dataTable.Rows[0];
			if (grdDetail.GetRows().Count() > 0)
			{
				if (Conversion.ToInt(dr["ScheduleDetailId"]) == 0)
				{
					if (Conversion.ToInt(dr["SupCustId"]) != Conversion.ToInt(CmbCustomer.Value) || Conversion.ToInt(dr["FcurrencyId"]) != Conversion.ToInt(CmbFcyCode.Value))
					{
						throw new Exception("Already Loaded Row's Have Different Customer And Currency. So you Can't Load Rows Of Different Customer or Currency!");
					}
				}
				else if (Conversion.ToInt(dr["SupCustId"]) != Conversion.ToInt(CmbCustomer.Value))
				{
					throw new Exception("Already Loaded Row's Have Different Customer. So you Can't Load Rows Of Different Customer!");
				}
			}
			((Control)(object)CmbCustomer).Enabled = false;
			((Control)(object)CmbFcyCode).Enabled = false;
			CmbExportCompany.Value = Conversion.ToInt(dr["ExportCompanyId"]);
			CmbCustomer.Value = Conversion.ToInt(dr["SupCustId"]);
			cmbSupCust_Leave(null, null);
			int ConsigneeId = Conversion.ToInt(dr["ConsigneeId"]);
			if (ConsigneeId > 0)
			{
				CmbConsignee.Value = ConsigneeId;
			}
			int notifyParty1Id = Conversion.ToInt(dr["NotifyPartyId"]);
			if (notifyParty1Id > 0)
			{
				CmbNotifyParty1.Value = notifyParty1Id;
			}
			int notifyParty2Id = Conversion.ToInt(dr["NotifyParty2Id"]);
			if (notifyParty2Id > 0)
			{
				CmbNotifyParty2.Value = notifyParty2Id;
			}
			int notifyParty3Id = Conversion.ToInt(dr["NotifyParty3Id"]);
			if (notifyParty3Id > 0)
			{
				CmbNotifyParty3.Value = notifyParty3Id;
			}
			int exporterBankId = Conversion.ToInt(dr["ExporterBankId"]);
			if (exporterBankId > 0)
			{
				CmbExporterBank.Value = exporterBankId;
			}
			int importerBankId = Conversion.ToInt(dr["ImporterBankId"]);
			if (importerBankId > 0)
			{
				CmbImporterBank.Value = importerBankId;
			}
			int loadingPortId = Conversion.ToInt(dr["LoadingPortId"]);
			if (loadingPortId > 0)
			{
				CmbLoadingPort.Value = loadingPortId;
			}
			int destinationPortId = Conversion.ToInt(dr["DestinationPortId"]);
			if (destinationPortId > 0)
			{
				CmbDestinationPort.Value = destinationPortId;
			}
			int deliveryTermId = Conversion.ToInt(dr["DeliveryTermId"]);
			if (deliveryTermId > 0)
			{
				CmbDeliveryTerm.Value = deliveryTermId;
			}
			int fcurrencyId = Conversion.ToInt(dr["FcurrencyId"]);
			if (fcurrencyId > 0)
			{
				CmbFcyCode.Value = fcurrencyId;
			}
			int totalContainers = (from row in dataTable.AsEnumerable()
				group row by row.Field<int>("ScheduleId")).Sum((IGrouping<int, DataRow> g) => g.First().Field<int>("NoOfContainers"));
			txtNoOfContainer.Text = totalContainers.ToString();
			string contractNoList = string.Join(", ", (from row in dataTable.AsEnumerable()
				select row.Field<string>("CustomerContractNo") into c
				where !string.IsNullOrWhiteSpace(c)
				select c).Distinct());
			txtCustomerContractsNos.Text = contractNoList;
			txtExRate.Text = Conversion.ToDouble(dr["ExchangeRate"]).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillDetaildtsFromLoaderData(DataTable contract)
	{
		try
		{
			HashSet<int> existingContractDetailIds = new HashSet<int>();
			HashSet<int> existingScheduleDetailIds = new HashSet<int>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow obj in rows)
			{
				int scheduleDetailId2 = Conversion.ToInt(obj.Cells["ContractScheduleDetailId"].Value);
				int contractDetailId2 = Conversion.ToInt(obj.Cells["ContractDetailId"].Value);
				if (scheduleDetailId2 > 0)
				{
					existingScheduleDetailIds.Add(scheduleDetailId2);
				}
				else
				{
					existingContractDetailIds.Add(contractDetailId2);
				}
			}
			foreach (DataRow row in contract.Rows)
			{
				int scheduleDetailId = Conversion.ToInt(row["ScheduleDetailId"]);
				int contractDetailId = Conversion.ToInt(row["ContractDetailId"]);
				if (!((scheduleDetailId > 0) ? existingScheduleDetailIds.Contains(scheduleDetailId) : existingContractDetailIds.Contains(contractDetailId)))
				{
					double innerUom = Conversion.ToDouble(row["InnerEquivalent"]);
					double BalMTon = Conversion.ToDouble(row["BalMTon"]);
					double InnerQty = ((BalMTon > 0.0 && innerUom > 0.0) ? (BalMTon * 1000.0 / innerUom) : 0.0);
					InnerQty = Math.Round(InnerQty);
					object[] values = new object[52]
					{
						0,
						row["Id"],
						row["ContractDetailId"],
						row["ProformaNo"],
						row["ProformaDate"],
						row["ScheduleCode"],
						row["ScheduleId"],
						row["ScheduleCode"],
						row["ScheduleDetailId"],
						row["ItemCategoryId"],
						row["ItemCategory"],
						row["ExImItemId"],
						row["ItemCode"],
						row["ItemName"],
						row["CropYearId"],
						row["JobLotId"],
						row["InvPackingMaterialTypeId"],
						row["PmItemId"],
						row["PmItemCode"],
						row["PmItemName"],
						row["ItemBrandId"],
						row["BrandCode"],
						row["BrandName"],
						Conversion.ToDouble(row["BalMTon"]),
						row["PackUomId"],
						row["PackUom"],
						row["OuterEquivalent"],
						row["OuterQtyEquivalent"],
						row["BalNoofBags"],
						row["InnerUomId"],
						row["InnerUom"],
						row["InnerEquivalent"],
						InnerQty,
						row["PalletQty"],
						0,
						row["RatePrice"],
						0,
						row["RatePrice"],
						row["RatePrice"],
						row["RateUomId"],
						row["RateUom"],
						row["RateEquivalent"],
						row["BalShipAmount"],
						0,
						"",
						"",
						"",
						"",
						row["CommodityDetail"],
						row["PackingDetail"],
						"",
						0
					};
					dtdetail.Rows.Add(values);
					dtdetailCustom.Rows.Add(values);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillOtherchargesDetailByContractIdsDBCall(string distinctIds)
	{
		try
		{
			DataTable contract = ExImLcOrder.ExImLcOrderOtherChargesDetailByLcOrderIds(distinctIds);
			if (contract != null && contract.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> existingContractDetailIds = new HashSet<int>();
			GridEXRow[] rows = grdOtherChargesDetail.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				int ContractDetailId = Conversion.ToInt(rows[i].Cells["ContractOtherChargesDetailId"].Value);
				existingContractDetailIds.Add(ContractDetailId);
			}
			foreach (DataRow row in contract.Rows)
			{
				int ContractOtherChargesDetailId = Conversion.ToInt(row["Id"]);
				if (existingContractDetailIds.Add(ContractOtherChargesDetailId))
				{
					object[] values = new object[8]
					{
						0,
						row["ExImLcOrderId"],
						row["Id"],
						row["ChargesItemId"],
						row["ChargesItemName"],
						row["AddAmount"],
						row["LessAmount"],
						row["Remarks"]
					};
					dtOtherChargesDetail.Rows.Add(values);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindPaymentTermDtByContractIdsDBCall(string distinctIds)
	{
		try
		{
			dtPaymnentDetail.Rows.Clear();
			dtPaymnentDetailCustom.Rows.Clear();
			foreach (DataRow row in ExImLcOrder.LcOrderPaymentTermsDetailByLcOrderIds(distinctIds).Rows)
			{
				object[] values = new object[10]
				{
					0,
					row["PaymentTermId"],
					row["LcOrderTerm"],
					0,
					0,
					"",
					row["PrcntOfTotal"],
					row["FcyAmount"],
					row["DueDays"],
					row["Remarks"]
				};
				dtPaymnentDetail.Rows.Add(values);
				dtPaymnentDetailCustom.Rows.Add(values);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindOtherItemDtByContractIdsDBCall(string ids)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Ids = ids
			};
			dtOtherItem.Clear();
			foreach (DataRow row in ExImLcOrder.GetOtherItemsFromLcOrder(val).Rows)
			{
				dtOtherItem.Rows.Add(row["otherItemId"], row["ItemName"], row["oItemQty"], row["oItemRate"], row["oItemAmount"], row["OtherItemRemarks"]);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnGenerateInvoiceNos_Click(object sender, EventArgs e)
	{
		try
		{
			new GenerateExportInvoiceNos(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnItemDetailLookUp_Click(object sender, EventArgs e)
	{
		try
		{
			new CommodityDetailItemCustomerWise(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnExportCharges_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineExportCharges(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineBankForeign_Click(object sender, EventArgs e)
	{
		try
		{
			OpenBankDefinition(1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenBankDefinition(int Id)
	{
		try
		{
			AcfrmDefineBank acfrmDefineBank = new AcfrmDefineBank(UserAccount);
			acfrmDefineBank.RequestedByOtherDocument = true;
			acfrmDefineBank.CountryTypeId = Id;
			acfrmDefineBank.BringToFront();
			acfrmDefineBank.Show();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnDefineBankLocal_Click(object sender, EventArgs e)
	{
		try
		{
			OpenBankDefinition(2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateInvoiceNo_Click(object sender, EventArgs e)
	{
		try
		{
			new GenerateExportInvoiceNos(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingList_Click(object sender, EventArgs e)
	{
		try
		{
			FormHelper.CanOpenForm("frmExportInvoicePackingList", delegate
			{
				frmExportInvoicePackingList obj = new frmExportInvoicePackingList(UserAccount);
				obj.Show();
				obj.BringToFront();
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddAmountChargeDetail_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtLessAmountChargeDetail_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtOuterQtyPackListDetail_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtNetWeightPackListDetail_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtGrossWeightPackListDetail_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtAddAmountChargeDetailCustom_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtLessAmountChargeDetailCustom_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtPrcntOfTotalPTDetailCustom_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtFcyAmountPTDetailCustom_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtDueDaysPTDetailCustom_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl2.SelectedIndex == 3)
		{
			ItemNamePackListBind();
		}
	}

	private void ImProformaInvoice_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtDocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grdDetail).Focus();
						tabControl2.SelectedIndex = 0;
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						((Control)(object)grdotheritems).Focus();
						tabControl2.SelectedIndex = 1;
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						((Control)(object)grdPaymentTermDetail).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grdDetail).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						CmbItemOIDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						CmbPaymentTermPTDetail.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						CmbItemOIDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 2;
						CmbPaymentTermPTDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdDetail).Focus();
					}
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)gridDetailHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)gridDetailHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Tabs");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Selected Tab Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Entry of Detail Selected Tab");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEX grid = grdDetail;
			GridEXRow row = grid.CurrentRow;
			if (row == null || (int)row.RowType != 0 || grid.CurrentColumn == null)
			{
				return;
			}
			string key = grid.CurrentColumn.Key;
			if (e.Control && e.KeyCode == Keys.Space && key == "Delete")
			{
				DeleteDetailRow(row);
			}
			else
			{
				if (e.KeyCode != Keys.F1 || key == null)
				{
					return;
				}
				switch (key.Length)
				{
				default:
					return;
				case 10:
				{
					char c = key[0];
					if ((uint)c <= 73u)
					{
						switch (c)
						{
						default:
							return;
						case 'C':
							if (key == "CropYearId")
							{
								ShowPopupAndSetValue(row, dtCrop, "Id", "CropYear", key);
							}
							return;
						case 'I':
							break;
						}
						if (!(key == "InnerUomId"))
						{
							return;
						}
						goto IL_039c;
					}
					if (c != 'O')
					{
						if (c != 'P' || (!(key == "PmItemName") && !(key == "PmItemCode")))
						{
							return;
						}
						goto IL_03ad;
					}
					if (!(key == "OuterUomId"))
					{
						return;
					}
					goto IL_038b;
				}
				case 8:
				{
					char c = key[0];
					if ((uint)c <= 74u)
					{
						switch (c)
						{
						default:
							return;
						case 'J':
							if (key == "JobLotId")
							{
								ShowPopupAndSetValue(row, dtjob, "Id", "JobLotDescription", key);
							}
							return;
						case 'I':
							break;
						}
						if (!(key == "InnerUom"))
						{
							return;
						}
						goto IL_039c;
					}
					if (c != 'O')
					{
						if (c != 'P' || !(key == "PmItemId"))
						{
							return;
						}
						goto IL_03ad;
					}
					if (!(key == "OuterUom"))
					{
						return;
					}
					goto IL_038b;
				}
				case 13:
					switch (key[7])
					{
					case 'T':
						if (key == "PackingTypeId")
						{
							ShowPopupAndSetValue(row, dtpcktype, "Id", "Description", key);
						}
						break;
					case 'D':
						if (key == "PackingDetail")
						{
							HandleCommodityOrPackingDetail(row, 2, "PackingDetail");
						}
						break;
					}
					return;
				case 15:
				{
					char c = key[0];
					if ((uint)c <= 73u)
					{
						switch (c)
						{
						default:
							return;
						case 'I':
							break;
						case 'C':
							if (key == "CommodityDetail")
							{
								HandleCommodityOrPackingDetail(row, 1, "CommodityDetail");
							}
							return;
						}
						if (!(key == "InnerEquivalent"))
						{
							return;
						}
						goto IL_039c;
					}
					switch (c)
					{
					default:
						return;
					case 'O':
						break;
					case 'L':
						if (key == "LabInspectionId")
						{
							int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
							if (itemId <= 0)
							{
								MessageBox.Show("Please Select an Item First");
								return;
							}
							DataTable dt = ThirdPartyNoDbCall(itemId);
							ShowPopupAndSetValue(row, dt, "Id", "InspectionNo", key);
						}
						return;
					}
					if (!(key == "OuterEquivalent"))
					{
						return;
					}
					goto IL_038b;
				}
				case 9:
					switch (key[5])
					{
					default:
						return;
					case 'N':
						if (!(key == "BrandName"))
						{
							return;
						}
						break;
					case 'C':
						if (!(key == "BrandCode"))
						{
							return;
						}
						break;
					}
					break;
				case 7:
					if (!(key == "BrandId"))
					{
						return;
					}
					break;
				case 11:
				case 12:
				case 14:
					return;
					IL_039c:
					HandleUomSelection(row, "Inner");
					return;
					IL_03ad:
					HandleItemSelection(row, dtPMItem, "PmItem");
					return;
					IL_038b:
					HandleUomSelection(row, "Outer");
					return;
				}
				HandleItemSelection(row, dtBrand, "Brand");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailCustom_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEX grid = grdDetailCustom;
			GridEXRow row = grid.CurrentRow;
			if (row == null || (int)row.RowType != 0 || grid.CurrentColumn == null)
			{
				return;
			}
			string key = grid.CurrentColumn.Key;
			if (e.Control && e.KeyCode == Keys.Space && key == "Delete")
			{
				DeleteDetailCustomRow(row);
			}
			else
			{
				if (e.KeyCode != Keys.F1 || key == null)
				{
					return;
				}
				switch (key.Length)
				{
				default:
					return;
				case 10:
				{
					char c = key[0];
					if ((uint)c <= 73u)
					{
						switch (c)
						{
						default:
							return;
						case 'C':
							if (key == "CropYearId")
							{
								ShowPopupAndSetValue(row, dtCrop, "Id", "CropYear", key);
							}
							return;
						case 'I':
							break;
						}
						if (!(key == "InnerUomId"))
						{
							return;
						}
						goto IL_039c;
					}
					if (c != 'O')
					{
						if (c != 'P' || (!(key == "PmItemName") && !(key == "PmItemCode")))
						{
							return;
						}
						goto IL_03ad;
					}
					if (!(key == "OuterUomId"))
					{
						return;
					}
					goto IL_038b;
				}
				case 8:
				{
					char c = key[0];
					if ((uint)c <= 74u)
					{
						switch (c)
						{
						default:
							return;
						case 'J':
							if (key == "JobLotId")
							{
								ShowPopupAndSetValue(row, dtjob, "Id", "JobLotDescription", key);
							}
							return;
						case 'I':
							break;
						}
						if (!(key == "InnerUom"))
						{
							return;
						}
						goto IL_039c;
					}
					if (c != 'O')
					{
						if (c != 'P' || !(key == "PmItemId"))
						{
							return;
						}
						goto IL_03ad;
					}
					if (!(key == "OuterUom"))
					{
						return;
					}
					goto IL_038b;
				}
				case 13:
					switch (key[7])
					{
					case 'T':
						if (key == "PackingTypeId")
						{
							ShowPopupAndSetValue(row, dtpcktype, "Id", "Description", key);
						}
						break;
					case 'D':
						if (key == "PackingDetail")
						{
							HandleCommodityOrPackingDetail(row, 2, "PackingDetail");
						}
						break;
					}
					return;
				case 15:
				{
					char c = key[0];
					if ((uint)c <= 73u)
					{
						switch (c)
						{
						default:
							return;
						case 'I':
							break;
						case 'C':
							if (key == "CommodityDetail")
							{
								HandleCommodityOrPackingDetail(row, 1, "CommodityDetail");
							}
							return;
						}
						if (!(key == "InnerEquivalent"))
						{
							return;
						}
						goto IL_039c;
					}
					switch (c)
					{
					default:
						return;
					case 'O':
						break;
					case 'L':
						if (key == "LabInspectionId")
						{
							int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
							if (itemId <= 0)
							{
								MessageBox.Show("Please Select an Item First");
								return;
							}
							DataTable dt = ThirdPartyNoDbCall(itemId);
							ShowPopupAndSetValue(row, dt, "Id", "InspectionNo", key);
						}
						return;
					}
					if (!(key == "OuterEquivalent"))
					{
						return;
					}
					goto IL_038b;
				}
				case 9:
					switch (key[5])
					{
					default:
						return;
					case 'N':
						if (!(key == "BrandName"))
						{
							return;
						}
						break;
					case 'C':
						if (!(key == "BrandCode"))
						{
							return;
						}
						break;
					}
					break;
				case 7:
					if (!(key == "BrandId"))
					{
						return;
					}
					break;
				case 11:
				case 12:
				case 14:
					return;
					IL_039c:
					HandleUomSelection(row, "Inner");
					return;
					IL_03ad:
					HandleItemSelection(row, dtPMItem, "PmItem");
					return;
					IL_038b:
					HandleUomSelection(row, "Outer");
					return;
				}
				HandleItemSelection(row, dtBrand, "Brand");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ValidateRowAgainstReference(GridEXRow row, DataTable referenceTable, string matchColumn1, string matchColumn2, Dictionary<string, object> resetColumns)
	{
		if (string.IsNullOrWhiteSpace(matchColumn1))
		{
			return;
		}
		object obj;
		switch (matchColumn1)
		{
		default:
			obj = matchColumn1;
			break;
		case "ItemId":
		case "OuterUomId":
		case "InnerUomId":
			obj = "Id";
			break;
		}
		string refCol1 = (string)obj;
		int val1 = Conversion.ToInt(row.Cells[matchColumn1].Value);
		bool exists = false;
		if (string.IsNullOrWhiteSpace(matchColumn2))
		{
			exists = referenceTable.AsEnumerable().Any((DataRow r) => r[refCol1] != DBNull.Value && Conversion.ToInt(r[refCol1]) == val1);
		}
		else
		{
			string refCol2 = ((matchColumn2 == "ItemId") ? "Id" : matchColumn2);
			int val2 = Conversion.ToInt(row.Cells[matchColumn2].Value);
			exists = referenceTable.AsEnumerable().Any((DataRow r) => r[refCol1] != DBNull.Value && r[refCol2] != DBNull.Value && Conversion.ToInt(r[refCol1]) == val1 && Conversion.ToInt(r[refCol2]) == val2);
		}
		if (exists)
		{
			return;
		}
		foreach (KeyValuePair<string, object> kvp in resetColumns)
		{
			row.Cells[kvp.Key].Value = kvp.Value;
		}
	}

	private void ShowPopupAndSetValue(GridEXRow row, DataTable dataSource, string idField, string displayField, string columnKey, string columnName = null)
	{
		using GrdPopUp popup = new GrdPopUp(dataSource, idField, displayField);
		popup.StartPosition = FormStartPosition.CenterScreen;
		popup.ShowDialog();
		row.Cells[columnKey].Value = Conversion.ToInt((object)popup.ReturnId);
		if (!string.IsNullOrWhiteSpace(columnName))
		{
			row.Cells[columnName].Value = Conversion.ToString((object)popup.ReturnName);
		}
	}

	private void HandleUomSelection(GridEXRow row, string prefix)
	{
		int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
		if (itemId <= 0)
		{
			MessageBox.Show("Please Select an Item First");
			return;
		}
		DataTable dtUom = CommonServices.dtUomFromGloablUomScheduleByItemId(itemId);
		using GrdPopUp popup = new GrdPopUp(dtUom, "Id", "UomCode");
		popup.StartPosition = FormStartPosition.CenterScreen;
		popup.ShowDialog();
		int uomId = Conversion.ToInt((object)popup.ReturnId);
		string uomName = Conversion.ToString((object)popup.ReturnName);
		row.Cells[prefix + "UomId"].Value = uomId;
		row.Cells[prefix + "Uom"].Value = uomName;
		if (uomId > 0)
		{
			DataRow found = dtUom.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<int>("Id") == uomId);
			double equivalent = ((found != null) ? Conversion.ToDouble(found["Equivalent"]) : 0.0);
			row.Cells[prefix + "Equivalent"].Value = equivalent;
			double qtyMton = Conversion.ToDouble(row.Cells["Qty/M.Ton"].Value);
			double qty = ((equivalent > 0.0) ? (qtyMton * 1000.0 / equivalent) : 0.0);
			row.Cells[prefix + "Qty"].Value = qty;
			if (prefix == "Outer")
			{
				double num = Conversion.ToDouble(row.Cells["NetRate"].Value);
				double rateEq = Conversion.ToDouble(row.Cells["RateEquivalent"].Value);
				double amount = num / rateEq * qtyMton * 1000.0;
				row.Cells["Amount"].Value = amount;
			}
		}
	}

	private void HandleItemSelection(GridEXRow row, DataTable dt, string prefix)
	{
		string ValuName = ((prefix == "PmItem") ? "Item" : prefix);
		using GrdPopUp popup = new GrdPopUp(dt, "Id", ValuName + "Name");
		popup.StartPosition = FormStartPosition.CenterScreen;
		popup.ShowDialog();
		int uomId = Conversion.ToInt((object)popup.ReturnId);
		string uomName = Conversion.ToString((object)popup.ReturnName);
		row.Cells[prefix + "Id"].Value = uomId;
		row.Cells[prefix + "Name"].Value = uomName;
		if (uomId > 0)
		{
			DataRow found = dt.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<int>("Id") == uomId);
			string Code = ((found != null) ? Conversion.ToString(found[ValuName + "Code"]) : "");
			row.Cells[prefix + "Code"].Value = Code;
		}
	}

	private void HandleCommodityOrPackingDetail(GridEXRow row, int detailTypeId, string columnKey)
	{
		int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
		if (itemId <= 0)
		{
			MessageBox.Show("Please Select an Item First");
			return;
		}
		DataRow[] filteredRows = ItemCommodityDetailDbCall(itemId).Select($"DetailTypeId={detailTypeId}");
		if (filteredRows.Length != 0)
		{
			using (GrdPopUp popup = new GrdPopUp(filteredRows.CopyToDataTable(), "Id", "Remarks"))
			{
				popup.StartPosition = FormStartPosition.CenterScreen;
				popup.ShowDialog();
				row.Cells[columnKey].Value = Conversion.ToString((object)popup.ReturnName);
				return;
			}
		}
		MessageBox.Show("No Record Found");
	}

	private void grdpaymentdetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEX grid = grdPaymentTermDetail;
			GridEXRow r = grid.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdpaymentdetail_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grid.CurrentColumn != null && grid.CurrentColumn.Key == "Delete")
				{
					DeletePTDetailRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTermDetailCustom_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEX grid = grdPaymentTermDetailCustom;
			GridEXRow r = grid.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdPaymentTermDetailCustom_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grid.CurrentColumn != null && grid.CurrentColumn.Key == "Delete")
				{
					DeletePTDetailCustomRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherChargesDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEX grid = grdOtherChargesDetail;
			GridEXRow r = grid.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdOtherChargesDetail_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grid.CurrentColumn != null && grid.CurrentColumn.Key == "Delete")
				{
					DeleteOtherChargesDetailRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherChargesDetailCustom_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEX grid = grdOtherChargesDetailCustom;
			GridEXRow r = grid.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdOtherChargesDetailCustom_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grid.CurrentColumn != null && grid.CurrentColumn.Key == "Delete")
				{
					DeleteOtherChargesDetailCustomRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackListDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEX grid = grdPackListDetail;
			GridEXRow r = grid.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdPackListDetail_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grid.CurrentColumn != null && grid.CurrentColumn.Key == "Delete")
				{
					DeletePackListDetailRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEX grid = grdotheritems;
			GridEXRow r = grid.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdotheritems_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grid.CurrentColumn != null && grid.CurrentColumn.Key == "Delete")
				{
					DeleteOIDetailRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights)
				{
					ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "Slip" && formrights.DoHavePrintRights)
				{
					CommonServices.CommercialInvoiceSlip521(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "Attachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
				if (DataGridHistory.CurrentColumn.Key == "Attachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
				if (DataGridHistory.CurrentColumn.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, DocumentTypeId, ScreenName);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
				else if (DataGridHistory.CurrentColumn.Key == "PrintA")
				{
					CommonServices.CommercialInvoiceSlipWithName(Id, "521_A_ExportInvoiceSlip");
				}
				else if (DataGridHistory.CurrentColumn.Key == "PrintB")
				{
					CommonServices.CommercialInvoiceSlipWithName(Id, "521_B_ExportInvoiceSlip");
				}
				else if (DataGridHistory.CurrentColumn.Key == "PrintC")
				{
					CommonServices.CommercialInvoiceSlipWithName(Id, "521_C_ExportInvoiceSlip");
				}
				else if (DataGridHistory.CurrentColumn.Key == "PrintD")
				{
					CommonServices.CommercialInvoiceSlipWithName(Id, "521_D_ExportInvoiceSlip");
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
		//IL_0d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Expected O, but got Unknown
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2e: Expected O, but got Unknown
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Expected O, but got Unknown
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d44: Expected O, but got Unknown
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4f: Expected O, but got Unknown
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5a: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d65: Expected O, but got Unknown
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d70: Expected O, but got Unknown
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7b: Expected O, but got Unknown
		//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Expected O, but got Unknown
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Expected O, but got Unknown
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9c: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da7: Expected O, but got Unknown
		//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db2: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbd: Expected O, but got Unknown
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc8: Expected O, but got Unknown
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd3: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dde: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de9: Expected O, but got Unknown
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df4: Expected O, but got Unknown
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dff: Expected O, but got Unknown
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0a: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e15: Expected O, but got Unknown
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e20: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2b: Expected O, but got Unknown
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e36: Expected O, but got Unknown
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e41: Expected O, but got Unknown
		//IL_0e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4c: Expected O, but got Unknown
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e57: Expected O, but got Unknown
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e62: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6d: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e78: Expected O, but got Unknown
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e83: Expected O, but got Unknown
		//IL_0e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8e: Expected O, but got Unknown
		//IL_0e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e99: Expected O, but got Unknown
		//IL_0e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea4: Expected O, but got Unknown
		//IL_0ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eaf: Expected O, but got Unknown
		//IL_0eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eba: Expected O, but got Unknown
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec5: Expected O, but got Unknown
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed0: Expected O, but got Unknown
		//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edb: Expected O, but got Unknown
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee6: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef1: Expected O, but got Unknown
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efc: Expected O, but got Unknown
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f07: Expected O, but got Unknown
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Expected O, but got Unknown
		//IL_0f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1d: Expected O, but got Unknown
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f28: Expected O, but got Unknown
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f33: Expected O, but got Unknown
		//IL_0f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3e: Expected O, but got Unknown
		//IL_0f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f49: Expected O, but got Unknown
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f54: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5f: Expected O, but got Unknown
		//IL_0f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Expected O, but got Unknown
		//IL_0f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f75: Expected O, but got Unknown
		//IL_0f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f80: Expected O, but got Unknown
		//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8b: Expected O, but got Unknown
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f96: Expected O, but got Unknown
		//IL_0f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa1: Expected O, but got Unknown
		//IL_0fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fac: Expected O, but got Unknown
		//IL_0fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb7: Expected O, but got Unknown
		//IL_0fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc2: Expected O, but got Unknown
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcd: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd8: Expected O, but got Unknown
		//IL_0fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe3: Expected O, but got Unknown
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fee: Expected O, but got Unknown
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff9: Expected O, but got Unknown
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1004: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_100f: Expected O, but got Unknown
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_101a: Expected O, but got Unknown
		//IL_101c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1025: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1030: Expected O, but got Unknown
		//IL_1032: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Expected O, but got Unknown
		//IL_103d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1046: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1051: Expected O, but got Unknown
		//IL_1053: Unknown result type (might be due to invalid IL or missing references)
		//IL_105c: Expected O, but got Unknown
		//IL_105e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1067: Expected O, but got Unknown
		//IL_1069: Unknown result type (might be due to invalid IL or missing references)
		//IL_1072: Expected O, but got Unknown
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_107d: Expected O, but got Unknown
		//IL_107f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1088: Expected O, but got Unknown
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1093: Expected O, but got Unknown
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109e: Expected O, but got Unknown
		//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a9: Expected O, but got Unknown
		//IL_10ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b4: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bf: Expected O, but got Unknown
		//IL_10c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ca: Expected O, but got Unknown
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d5: Expected O, but got Unknown
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1165: Expected O, but got Unknown
		//IL_1187: Unknown result type (might be due to invalid IL or missing references)
		//IL_1191: Expected O, but got Unknown
		//IL_11f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ff: Expected O, but got Unknown
		//IL_1221: Unknown result type (might be due to invalid IL or missing references)
		//IL_122b: Expected O, but got Unknown
		//IL_12a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12af: Expected O, but got Unknown
		//IL_1313: Unknown result type (might be due to invalid IL or missing references)
		//IL_131d: Expected O, but got Unknown
		//IL_1376: Unknown result type (might be due to invalid IL or missing references)
		//IL_1380: Expected O, but got Unknown
		//IL_143c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1446: Expected O, but got Unknown
		//IL_1452: Unknown result type (might be due to invalid IL or missing references)
		//IL_145c: Expected O, but got Unknown
		//IL_145d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1467: Expected O, but got Unknown
		//IL_1468: Unknown result type (might be due to invalid IL or missing references)
		//IL_1472: Expected O, but got Unknown
		//IL_1518: Unknown result type (might be due to invalid IL or missing references)
		//IL_1522: Expected O, but got Unknown
		//IL_1591: Unknown result type (might be due to invalid IL or missing references)
		//IL_159b: Expected O, but got Unknown
		//IL_1615: Unknown result type (might be due to invalid IL or missing references)
		//IL_161f: Expected O, but got Unknown
		//IL_16c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cf: Expected O, but got Unknown
		//IL_16f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fb: Expected O, but got Unknown
		//IL_175f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1769: Expected O, but got Unknown
		//IL_178b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Expected O, but got Unknown
		//IL_180f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1819: Expected O, but got Unknown
		//IL_187d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1887: Expected O, but got Unknown
		//IL_18eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f5: Expected O, but got Unknown
		//IL_1990: Unknown result type (might be due to invalid IL or missing references)
		//IL_199a: Expected O, but got Unknown
		//IL_19bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c6: Expected O, but got Unknown
		//IL_1a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8c: Expected O, but got Unknown
		//IL_1ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ace: Expected O, but got Unknown
		//IL_1acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad9: Expected O, but got Unknown
		//IL_1ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aef: Expected O, but got Unknown
		//IL_1b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1b: Expected O, but got Unknown
		//IL_1b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b47: Expected O, but got Unknown
		//IL_1c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4f: Expected O, but got Unknown
		//IL_1c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7b: Expected O, but got Unknown
		//IL_1cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd3: Expected O, but got Unknown
		//IL_1cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cde: Expected O, but got Unknown
		//IL_1d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2b: Expected O, but got Unknown
		//IL_1d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d41: Expected O, but got Unknown
		//IL_1d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6d: Expected O, but got Unknown
		//IL_1da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1daf: Expected O, but got Unknown
		//IL_1dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e07: Expected O, but got Unknown
		//IL_1f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0f: Expected O, but got Unknown
		//IL_1f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f46: Expected O, but got Unknown
		//IL_1f68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f72: Expected O, but got Unknown
		//IL_1f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9e: Expected O, but got Unknown
		//IL_1fc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fca: Expected O, but got Unknown
		//IL_1fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff6: Expected O, but got Unknown
		//IL_202e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2038: Expected O, but got Unknown
		//IL_205a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2064: Expected O, but got Unknown
		//IL_2086: Unknown result type (might be due to invalid IL or missing references)
		//IL_2090: Expected O, but got Unknown
		//IL_212b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2135: Expected O, but got Unknown
		//IL_2183: Unknown result type (might be due to invalid IL or missing references)
		//IL_218d: Expected O, but got Unknown
		//IL_2212: Unknown result type (might be due to invalid IL or missing references)
		//IL_221c: Expected O, but got Unknown
		//IL_221d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2227: Expected O, but got Unknown
		//IL_2228: Unknown result type (might be due to invalid IL or missing references)
		//IL_2232: Expected O, but got Unknown
		//IL_2233: Unknown result type (might be due to invalid IL or missing references)
		//IL_223d: Expected O, but got Unknown
		//IL_223e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2248: Expected O, but got Unknown
		//IL_2249: Unknown result type (might be due to invalid IL or missing references)
		//IL_2253: Expected O, but got Unknown
		//IL_2254: Unknown result type (might be due to invalid IL or missing references)
		//IL_225e: Expected O, but got Unknown
		//IL_2f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f86: Expected O, but got Unknown
		//IL_2f93: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9d: Expected O, but got Unknown
		//IL_3285: Unknown result type (might be due to invalid IL or missing references)
		//IL_328f: Expected O, but got Unknown
		//IL_4b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b4d: Expected O, but got Unknown
		//IL_5bac: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bb6: Expected O, but got Unknown
		//IL_8d02: Unknown result type (might be due to invalid IL or missing references)
		//IL_8d0c: Expected O, but got Unknown
		//IL_a30b: Unknown result type (might be due to invalid IL or missing references)
		//IL_a315: Expected O, but got Unknown
		//IL_a322: Unknown result type (might be due to invalid IL or missing references)
		//IL_a32c: Expected O, but got Unknown
		//IL_a606: Unknown result type (might be due to invalid IL or missing references)
		//IL_a610: Expected O, but got Unknown
		//IL_beda: Unknown result type (might be due to invalid IL or missing references)
		//IL_bee4: Expected O, but got Unknown
		//IL_16d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d4d: Expected O, but got Unknown
		//IL_16d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d7b: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTermDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance436 = new Appearance();
		Appearance appearance447 = new Appearance();
		Appearance appearance458 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		GridEXLayout grdOtherChargesDetail_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdPackListDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance168 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance437 = new Appearance();
		Appearance appearance438 = new Appearance();
		Appearance appearance439 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
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
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance431 = new Appearance();
		Appearance appearance433 = new Appearance();
		Appearance appearance434 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance247 = new Appearance();
		GridEXLayout grdDetailCustom_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTermDetailCustom_DesignTimeLayout = new GridEXLayout();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance401 = new Appearance();
		Appearance appearance402 = new Appearance();
		Appearance appearance403 = new Appearance();
		Appearance appearance404 = new Appearance();
		Appearance appearance405 = new Appearance();
		Appearance appearance406 = new Appearance();
		Appearance appearance407 = new Appearance();
		Appearance appearance409 = new Appearance();
		Appearance appearance410 = new Appearance();
		Appearance appearance411 = new Appearance();
		Appearance appearance412 = new Appearance();
		Appearance appearance413 = new Appearance();
		Appearance appearance414 = new Appearance();
		Appearance appearance415 = new Appearance();
		Appearance appearance416 = new Appearance();
		GridEXLayout grdOtherChargesDetailCustom_DesignTimeLayout = new GridEXLayout();
		Appearance appearance417 = new Appearance();
		Appearance appearance418 = new Appearance();
		Appearance appearance420 = new Appearance();
		Appearance appearance421 = new Appearance();
		Appearance appearance422 = new Appearance();
		Appearance appearance423 = new Appearance();
		Appearance appearance424 = new Appearance();
		Appearance appearance425 = new Appearance();
		Appearance appearance426 = new Appearance();
		Appearance appearance427 = new Appearance();
		Appearance appearance428 = new Appearance();
		Appearance appearance429 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
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
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance387 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance390 = new Appearance();
		Appearance appearance391 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance394 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance396 = new Appearance();
		Appearance appearance397 = new Appearance();
		Appearance appearance398 = new Appearance();
		Appearance appearance399 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance345 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance453 = new Appearance();
		Appearance appearance454 = new Appearance();
		Appearance appearance455 = new Appearance();
		Appearance appearance456 = new Appearance();
		Appearance appearance457 = new Appearance();
		Appearance appearance459 = new Appearance();
		Appearance appearance460 = new Appearance();
		Appearance appearance461 = new Appearance();
		Appearance appearance462 = new Appearance();
		Appearance appearance463 = new Appearance();
		Appearance appearance464 = new Appearance();
		Appearance appearance465 = new Appearance();
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
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance333 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance440 = new Appearance();
		Appearance appearance441 = new Appearance();
		Appearance appearance442 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance443 = new Appearance();
		Appearance appearance444 = new Appearance();
		Appearance appearance445 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance446 = new Appearance();
		Appearance appearance448 = new Appearance();
		Appearance appearance449 = new Appearance();
		Appearance appearance450 = new Appearance();
		Appearance appearance451 = new Appearance();
		Appearance appearance452 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance466 = new Appearance();
		Appearance appearance467 = new Appearance();
		Appearance appearance468 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance183 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout gridDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherChargesHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackListHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherItemHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout gridDetailCustomHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentCustomHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherChargesCustomHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance348 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance400 = new Appearance();
		Appearance appearance408 = new Appearance();
		Appearance appearance419 = new Appearance();
		Appearance appearance430 = new Appearance();
		Appearance appearance432 = new Appearance();
		Appearance appearance435 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance384 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.frmCommercialInvoiceIII));
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl4 = new System.Windows.Forms.TabControl();
		this.tabInvoiceDetailll = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabItemDetail = new System.Windows.Forms.TabPage();
		this.panel22 = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.ctrlgrdDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPaymentDetail = new System.Windows.Forms.TabPage();
		this.grdPaymentTermDetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlgrdPaymentTermDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.panel21 = new System.Windows.Forms.Panel();
		this.BtnAddPTDetail = new System.Windows.Forms.Button();
		this.label58 = new System.Windows.Forms.Label();
		this.txtRemarksPTDetail = new System.Windows.Forms.TextBox();
		this.label82 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.CmbFinInstrumentsPTDetail = new UltraCombo();
		this.label80 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.CmbPaymentTermPTDetail = new UltraCombo();
		this.label76 = new System.Windows.Forms.Label();
		this.txtPrcntOfTotalPTDetail = new System.Windows.Forms.TextBox();
		this.BtnCancelPTDetail = new System.Windows.Forms.Button();
		this.label83 = new System.Windows.Forms.Label();
		this.BtnUpdatePTDetail = new System.Windows.Forms.Button();
		this.txtBalanceFIPTDetail = new System.Windows.Forms.TextBox();
		this.txtDueDaysPTDetail = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFcyAmountPTDetail = new System.Windows.Forms.TextBox();
		this.tabChargesDetail = new System.Windows.Forms.TabPage();
		this.grdOtherChargesDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlgrdOtherChargesDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.panel13 = new System.Windows.Forms.Panel();
		this.BtnAddChargeDetail = new System.Windows.Forms.Button();
		this.label130 = new System.Windows.Forms.Label();
		this.txtRemarksChargeDetail = new System.Windows.Forms.TextBox();
		this.label134 = new System.Windows.Forms.Label();
		this.label131 = new System.Windows.Forms.Label();
		this.label132 = new System.Windows.Forms.Label();
		this.CmbChargesNameChargeDetail = new UltraCombo();
		this.label136 = new System.Windows.Forms.Label();
		this.BtnCancelChargeDetail = new System.Windows.Forms.Button();
		this.BtnUpdateChargeDetail = new System.Windows.Forms.Button();
		this.label133 = new System.Windows.Forms.Label();
		this.label140 = new System.Windows.Forms.Label();
		this.txtLessAmountChargeDetail = new System.Windows.Forms.TextBox();
		this.txtAddAmountChargeDetail = new System.Windows.Forms.TextBox();
		this.tabPackingList = new System.Windows.Forms.TabPage();
		this.grdPackListDetail = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.ctrlgrdPackListDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.panel26 = new System.Windows.Forms.Panel();
		this.RadItemCodePackListDetail = new System.Windows.Forms.RadioButton();
		this.RadItemNamePackListDetail = new System.Windows.Forms.RadioButton();
		this.label160 = new System.Windows.Forms.Label();
		this.label159 = new System.Windows.Forms.Label();
		this.label158 = new System.Windows.Forms.Label();
		this.BtnAddPackListDetail = new System.Windows.Forms.Button();
		this.txtSubLotNoPackListDetail = new System.Windows.Forms.TextBox();
		this.label151 = new System.Windows.Forms.Label();
		this.txtNetWeightPackListDetail = new System.Windows.Forms.TextBox();
		this.label150 = new System.Windows.Forms.Label();
		this.txtGrossWeightPackListDetail = new System.Windows.Forms.TextBox();
		this.label149 = new System.Windows.Forms.Label();
		this.txtOuterQtyPackListDetail = new System.Windows.Forms.TextBox();
		this.label147 = new System.Windows.Forms.Label();
		this.CmbInspectionNoPackListDetail = new UltraCombo();
		this.label144 = new System.Windows.Forms.Label();
		this.CmbPmDetailByItemCusPackListDetail = new UltraCombo();
		this.CmbItemFromInvDetailPackListDetail = new UltraCombo();
		this.CmbCommodityByItemCusPackListDetail = new UltraCombo();
		this.txtPmDetailPackListDetail = new System.Windows.Forms.TextBox();
		this.txtCommodityPackListDetail = new System.Windows.Forms.TextBox();
		this.label135 = new System.Windows.Forms.Label();
		this.label137 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.label138 = new System.Windows.Forms.Label();
		this.label139 = new System.Windows.Forms.Label();
		this.label141 = new System.Windows.Forms.Label();
		this.txtRemarksPackListDetail = new System.Windows.Forms.TextBox();
		this.label157 = new System.Windows.Forms.Label();
		this.label156 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label85 = new System.Windows.Forms.Label();
		this.label145 = new System.Windows.Forms.Label();
		this.CmbContainerNoPackListDetail = new UltraCombo();
		this.txtLotNoPackListDetail = new System.Windows.Forms.TextBox();
		this.label148 = new System.Windows.Forms.Label();
		this.BtnCancelPackListDetail = new System.Windows.Forms.Button();
		this.BtnUpdatePackListDetail = new System.Windows.Forms.Button();
		this.label146 = new System.Windows.Forms.Label();
		this.txtExpiryDatePackListDetail = new System.Windows.Forms.DateTimePicker();
		this.label143 = new System.Windows.Forms.Label();
		this.txtPackageDatePackListDetail = new System.Windows.Forms.DateTimePicker();
		this.label142 = new System.Windows.Forms.Label();
		this.tabOtherItem = new System.Windows.Forms.TabPage();
		this.grdotheritems = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlgrdotheritems = new grid.User_Conrtols.CtrlGrdBar();
		this.panel19 = new System.Windows.Forms.Panel();
		this.RadItemCodeOIDetail = new System.Windows.Forms.RadioButton();
		this.RadItemNameOIDetail = new System.Windows.Forms.RadioButton();
		this.label11 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.BtnAddOIDetail = new System.Windows.Forms.Button();
		this.BtnCancelOIDetail = new System.Windows.Forms.Button();
		this.CmbItemOIDetail = new UltraCombo();
		this.BtnUpdateOIDetail = new System.Windows.Forms.Button();
		this.label71 = new System.Windows.Forms.Label();
		this.txtAmountOIDetail = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.txtRemarksOIDetail = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.txtQtyOIDetail = new System.Windows.Forms.TextBox();
		this.txtRateOIDetail = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.tabCustomDetail = new System.Windows.Forms.TabPage();
		this.tabControl5 = new System.Windows.Forms.TabControl();
		this.tabItemDetailCustom = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdDetailCustom = new GridEX();
		this.panel27 = new System.Windows.Forms.Panel();
		this.ctrlgrdDetailCustom = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPaymentDetailCustom = new System.Windows.Forms.TabPage();
		this.grdPaymentTermDetailCustom = new GridEX();
		this.panel30 = new System.Windows.Forms.Panel();
		this.ctrlgrdPaymentTermDetailCustom = new grid.User_Conrtols.CtrlGrdBar();
		this.panel34 = new System.Windows.Forms.Panel();
		this.BtnAddPTDetailCustom = new System.Windows.Forms.Button();
		this.label88 = new System.Windows.Forms.Label();
		this.txtRemarksPTDetailCustom = new System.Windows.Forms.TextBox();
		this.label93 = new System.Windows.Forms.Label();
		this.label94 = new System.Windows.Forms.Label();
		this.label96 = new System.Windows.Forms.Label();
		this.CmbFinInstrumentsPTDetailCustom = new UltraCombo();
		this.label97 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.label99 = new System.Windows.Forms.Label();
		this.CmbPaymentTermPTDetailCustom = new UltraCombo();
		this.label100 = new System.Windows.Forms.Label();
		this.txtPrcntOfTotalPTDetailCustom = new System.Windows.Forms.TextBox();
		this.BtnCancelPTDetailCustom = new System.Windows.Forms.Button();
		this.label101 = new System.Windows.Forms.Label();
		this.BtnUpdatePTDetailCustom = new System.Windows.Forms.Button();
		this.txtBalanceFIPTDetailCustom = new System.Windows.Forms.TextBox();
		this.txtDueDaysPTDetailCustom = new System.Windows.Forms.TextBox();
		this.label102 = new System.Windows.Forms.Label();
		this.label103 = new System.Windows.Forms.Label();
		this.txtFcyAmountPTDetailCustom = new System.Windows.Forms.TextBox();
		this.tabChargesDetailCustom = new System.Windows.Forms.TabPage();
		this.grdOtherChargesDetailCustom = new GridEX();
		this.panel28 = new System.Windows.Forms.Panel();
		this.ctrlgrdOtherChargesDetailCustom = new grid.User_Conrtols.CtrlGrdBar();
		this.panel29 = new System.Windows.Forms.Panel();
		this.BtnAddChargeDetailCustom = new System.Windows.Forms.Button();
		this.label37 = new System.Windows.Forms.Label();
		this.txtRemarksChargeDetailCustom = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.CmbChargesNameChargeDetailCustom = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.BtnCancelChargeDetailCustom = new System.Windows.Forms.Button();
		this.BtnUpdateChargeDetailCustom = new System.Windows.Forms.Button();
		this.label65 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.txtLessAmountChargeDetailCustom = new System.Windows.Forms.TextBox();
		this.txtAddAmountChargeDetailCustom = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.HeaderGropBox = new System.Windows.Forms.GroupBox();
		this.cmbInvoiceNo = new UltraCombo();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtFcyAmountHeaderCustom = new System.Windows.Forms.TextBox();
		this.txtAddLessHeaderCustom = new System.Windows.Forms.TextBox();
		this.txtLocalAmountHeaderCustom = new System.Windows.Forms.TextBox();
		this.txtTotalFcyAmountHeaderCustom = new System.Windows.Forms.TextBox();
		this.label112 = new System.Windows.Forms.Label();
		this.label119 = new System.Windows.Forms.Label();
		this.label121 = new System.Windows.Forms.Label();
		this.label122 = new System.Windows.Forms.Label();
		this.btnDefineBankLocal = new System.Windows.Forms.Button();
		this.btnDefineBankForeign = new System.Windows.Forms.Button();
		this.txtaddlesscommnets = new System.Windows.Forms.TextBox();
		this.txtLocalAmt = new System.Windows.Forms.TextBox();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.CmbFcyCode = new UltraCombo();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.txtaddlessamount = new System.Windows.Forms.TextBox();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.CmbPlaceOfDelivery = new UltraCombo();
		this.txtTotalNetAmount = new System.Windows.Forms.TextBox();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtExRate = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.CmbImporterCountry = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.txtLotRefHeader = new System.Windows.Forms.TextBox();
		this.CmbContinent = new UltraCombo();
		this.CmbCareierType = new UltraCombo();
		this.txtGdDate = new System.Windows.Forms.DateTimePicker();
		this.CmbDestinationPort = new UltraCombo();
		this.label116 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtCustomerContractsNos = new System.Windows.Forms.TextBox();
		this.CmbNotifyParty3 = new UltraCombo();
		this.label108 = new System.Windows.Forms.Label();
		this.txtCustomerContractsDates = new System.Windows.Forms.TextBox();
		this.txtDeliveryRemarks = new System.Windows.Forms.TextBox();
		this.CmbConsignee = new UltraCombo();
		this.label87 = new System.Windows.Forms.Label();
		this.label120 = new System.Windows.Forms.Label();
		this.label114 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.txtTcpRegNo = new System.Windows.Forms.TextBox();
		this.txtDocumentaryCreditNo = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label113 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label107 = new System.Windows.Forms.Label();
		this.txtRemarks2 = new System.Windows.Forms.TextBox();
		this.txtRemarks1 = new System.Windows.Forms.TextBox();
		this.label118 = new System.Windows.Forms.Label();
		this.txtCertificate1 = new System.Windows.Forms.TextBox();
		this.label117 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtCertificateOfOrigion = new System.Windows.Forms.TextBox();
		this.CmbImporterBank = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label95 = new System.Windows.Forms.Label();
		this.CmbOriginCountry = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label109 = new System.Windows.Forms.Label();
		this.txtGdNo = new System.Windows.Forms.TextBox();
		this.label129 = new System.Windows.Forms.Label();
		this.label115 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label110 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.CmbExporterBank = new UltraCombo();
		this.label68 = new System.Windows.Forms.Label();
		this.txtDocumentaryCreditNoDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label111 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.CmbLoadingPort = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbCustomer = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbNotifyParty2 = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.CmbNotifyParty1 = new UltraCombo();
		this.label56 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.txtOtherRemarks2 = new System.Windows.Forms.TextBox();
		this.label155 = new System.Windows.Forms.Label();
		this.txtOtherRemarks1 = new System.Windows.Forms.TextBox();
		this.label154 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.CmbExportCompany = new UltraCombo();
		this.labelExportCompany = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint521A = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint521B = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint521C = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint521D = new System.Windows.Forms.ToolStripButton();
		this.BtnLoad = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateInvoiceNos = new System.Windows.Forms.ToolStripButton();
		this.BtnExportCharges = new System.Windows.Forms.ToolStripButton();
		this.btnItemDetailLookUp = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateInvoiceNo = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnPackingList = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.tabControl6 = new System.Windows.Forms.TabControl();
		this.tabInvoiceDetailH = new System.Windows.Forms.TabPage();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabDetailH = new System.Windows.Forms.TabPage();
		this.gridDetailHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.tabPaymentDetailH = new System.Windows.Forms.TabPage();
		this.grdPaymentHistory = new GridEX();
		this.panel31 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.tabOtherChargesH = new System.Windows.Forms.TabPage();
		this.grdOtherChargesHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label36 = new System.Windows.Forms.Label();
		this.tabPmH = new System.Windows.Forms.TabPage();
		this.grdPackListHistory = new GridEX();
		this.panel32 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.tabOtherItemH = new System.Windows.Forms.TabPage();
		this.grdOtherItemHistory = new GridEX();
		this.panel33 = new System.Windows.Forms.Panel();
		this.label35 = new System.Windows.Forms.Label();
		this.tabCustomDetailH = new System.Windows.Forms.TabPage();
		this.tabControl7 = new System.Windows.Forms.TabControl();
		this.tabDetailCustomH = new System.Windows.Forms.TabPage();
		this.gridDetailCustomHistory = new GridEX();
		this.panel35 = new System.Windows.Forms.Panel();
		this.label104 = new System.Windows.Forms.Label();
		this.tabPaymentDetailCustomH = new System.Windows.Forms.TabPage();
		this.grdPaymentCustomHistory = new GridEX();
		this.panel36 = new System.Windows.Forms.Panel();
		this.label105 = new System.Windows.Forms.Label();
		this.tabOtherChargesCustomH = new System.Windows.Forms.TabPage();
		this.grdOtherChargesCustomHistory = new GridEX();
		this.panel37 = new System.Windows.Forms.Panel();
		this.label106 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.ctrlDataGridHistory = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.rdReferred = new System.Windows.Forms.RadioButton();
		this.rdNotReferred = new System.Windows.Forms.RadioButton();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label48 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label73 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbFCurrencr = new UltraCombo();
		this.combranches = new UltraCombo();
		this.comproject = new UltraCombo();
		this.ultraCombo3 = new UltraCombo();
		this.ultraCombo2 = new UltraCombo();
		this.ultraCombo1 = new UltraCombo();
		this.cmbEFormNo = new UltraCombo();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl4.SuspendLayout();
		this.tabInvoiceDetailll.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabItemDetail.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel23.SuspendLayout();
		this.tabPaymentDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermDetail).BeginInit();
		this.panel20.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstrumentsPTDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPTDetail).BeginInit();
		this.tabChargesDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesNameChargeDetail).BeginInit();
		this.tabPackingList.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackListDetail).BeginInit();
		this.panel25.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionNoPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPmDetailByItemCusPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemFromInvDetailPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommodityByItemCusPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerNoPackListDetail).BeginInit();
		this.tabOtherItem.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel18.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOIDetail).BeginInit();
		this.tabCustomDetail.SuspendLayout();
		this.tabControl5.SuspendLayout();
		this.tabItemDetailCustom.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailCustom).BeginInit();
		this.panel27.SuspendLayout();
		this.tabPaymentDetailCustom.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermDetailCustom).BeginInit();
		this.panel30.SuspendLayout();
		this.panel34.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstrumentsPTDetailCustom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPTDetailCustom).BeginInit();
		this.tabChargesDetailCustom.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetailCustom).BeginInit();
		this.panel28.SuspendLayout();
		this.panel29.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesNameChargeDetailCustom).BeginInit();
		this.panel11.SuspendLayout();
		this.HeaderGropBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlaceOfDelivery).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterCountry).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContinent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCareierType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbConsignee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOriginCountry).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty1).BeginInit();
		this.panel17.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportCompany).BeginInit();
		this.panel12.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.tabControl6.SuspendLayout();
		this.tabInvoiceDetailH.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.tabPaymentDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentHistory).BeginInit();
		this.panel31.SuspendLayout();
		this.tabOtherChargesH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.tabPmH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackListHistory).BeginInit();
		this.panel32.SuspendLayout();
		this.tabOtherItemH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherItemHistory).BeginInit();
		this.panel33.SuspendLayout();
		this.tabCustomDetailH.SuspendLayout();
		this.tabControl7.SuspendLayout();
		this.tabDetailCustomH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailCustomHistory).BeginInit();
		this.panel35.SuspendLayout();
		this.tabPaymentDetailCustomH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentCustomHistory).BeginInit();
		this.panel36.SuspendLayout();
		this.tabOtherChargesCustomH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesCustomHistory).BeginInit();
		this.panel37.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1184, 761);
		this.panel4.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.tabControl1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1184, 761);
		this.panel7.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 761);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.tabControl4);
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Controls.Add(this.panel17);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 732);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabControl4.Controls.Add(this.tabInvoiceDetailll);
		this.tabControl4.Controls.Add(this.tabCustomDetail);
		this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl4.Location = new System.Drawing.Point(3, 437);
		this.tabControl4.Name = "tabControl4";
		this.tabControl4.SelectedIndex = 0;
		this.tabControl4.Size = new System.Drawing.Size(1170, 240);
		this.tabControl4.TabIndex = 4;
		this.tabInvoiceDetailll.Controls.Add(this.tabControl2);
		this.tabInvoiceDetailll.Location = new System.Drawing.Point(4, 25);
		this.tabInvoiceDetailll.Name = "tabInvoiceDetailll";
		this.tabInvoiceDetailll.Padding = new System.Windows.Forms.Padding(3);
		this.tabInvoiceDetailll.Size = new System.Drawing.Size(1162, 211);
		this.tabInvoiceDetailll.TabIndex = 0;
		this.tabInvoiceDetailll.Text = "Invoice Detail";
		this.tabInvoiceDetailll.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabItemDetail);
		this.tabControl2.Controls.Add(this.tabPaymentDetail);
		this.tabControl2.Controls.Add(this.tabChargesDetail);
		this.tabControl2.Controls.Add(this.tabPackingList);
		this.tabControl2.Controls.Add(this.tabOtherItem);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(3, 3);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1156, 205);
		this.tabControl2.TabIndex = 0;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabItemDetail.Controls.Add(this.panel22);
		this.tabItemDetail.Location = new System.Drawing.Point(4, 25);
		this.tabItemDetail.Name = "tabItemDetail";
		this.tabItemDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabItemDetail.Size = new System.Drawing.Size(1148, 176);
		this.tabItemDetail.TabIndex = 0;
		this.tabItemDetail.Text = "Item Detail";
		this.tabItemDetail.UseVisualStyleBackColor = true;
		this.panel22.Controls.Add(this.panel24);
		this.panel22.Controls.Add(this.panel23);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(3, 3);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1142, 170);
		this.panel22.TabIndex = 5;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 24);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1142, 146);
		this.panel24.TabIndex = 2;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1142, 146);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlgrdDetail);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1142, 24);
		this.panel23.TabIndex = 1;
		this.ctrlgrdDetail.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdDetail.FormName = this;
		this.ctrlgrdDetail.Location = new System.Drawing.Point(1103, 1);
		this.ctrlgrdDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdDetail.MyGrid = this.grdDetail;
		this.ctrlgrdDetail.Name = "ctrlgrdDetail";
		this.ctrlgrdDetail.Size = new System.Drawing.Size(38, 21);
		this.ctrlgrdDetail.TabIndex = 5;
		this.ctrlgrdDetail.Load += new System.EventHandler(ctrlgrdDetail_Load);
		this.tabPaymentDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail);
		this.tabPaymentDetail.Controls.Add(this.panel20);
		this.tabPaymentDetail.Controls.Add(this.panel21);
		this.tabPaymentDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentDetail.Name = "tabPaymentDetail";
		this.tabPaymentDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabPaymentDetail.Size = new System.Drawing.Size(1148, 176);
		this.tabPaymentDetail.TabIndex = 2;
		this.tabPaymentDetail.Text = "Payment Detail";
		this.tabPaymentDetail.UseVisualStyleBackColor = true;
		this.grdPaymentTermDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentTermDetail).BackColor = System.Drawing.Color.White;
		grdPaymentTermDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTermDetail.DesignTimeLayout = grdPaymentTermDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTermDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Location = new System.Drawing.Point(3, 83);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Name = "grdPaymentTermDetail";
		((ScrollableBase)this.grdPaymentTermDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdPaymentTermDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdPaymentTermDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).Size = new System.Drawing.Size(1142, 90);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).TabIndex = 1;
		this.grdPaymentTermDetail.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTermDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTermDetail).VisualStyle = (VisualStyle)4;
		this.grdPaymentTermDetail.ColumnButtonClick += new ColumnActionEventHandler(grdpaymentdetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).DoubleClick += new System.EventHandler(grdpaymentdetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdpaymentdetail_KeyDown);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlgrdPaymentTermDetail);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 60);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1142, 23);
		this.panel20.TabIndex = 0;
		this.ctrlgrdPaymentTermDetail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdPaymentTermDetail.FormName = this;
		this.ctrlgrdPaymentTermDetail.Location = new System.Drawing.Point(1099, 1);
		this.ctrlgrdPaymentTermDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdPaymentTermDetail.MyGrid = this.grdPaymentTermDetail;
		this.ctrlgrdPaymentTermDetail.Name = "ctrlgrdPaymentTermDetail";
		this.ctrlgrdPaymentTermDetail.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdPaymentTermDetail.TabIndex = 27;
		this.ctrlgrdPaymentTermDetail.Load += new System.EventHandler(ctrlgrdPaymentTermDetail_Load);
		this.panel21.Controls.Add(this.BtnAddPTDetail);
		this.panel21.Controls.Add(this.label58);
		this.panel21.Controls.Add(this.txtRemarksPTDetail);
		this.panel21.Controls.Add(this.label82);
		this.panel21.Controls.Add(this.label75);
		this.panel21.Controls.Add(this.label81);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetail);
		this.panel21.Controls.Add(this.label80);
		this.panel21.Controls.Add(this.label74);
		this.panel21.Controls.Add(this.label79);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetail);
		this.panel21.Controls.Add(this.label76);
		this.panel21.Controls.Add(this.txtPrcntOfTotalPTDetail);
		this.panel21.Controls.Add(this.BtnCancelPTDetail);
		this.panel21.Controls.Add(this.label83);
		this.panel21.Controls.Add(this.BtnUpdatePTDetail);
		this.panel21.Controls.Add(this.txtBalanceFIPTDetail);
		this.panel21.Controls.Add(this.txtDueDaysPTDetail);
		this.panel21.Controls.Add(this.label78);
		this.panel21.Controls.Add(this.label77);
		this.panel21.Controls.Add(this.txtFcyAmountPTDetail);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(3, 3);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1142, 57);
		this.panel21.TabIndex = 0;
		this.BtnAddPTDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnAddPTDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddPTDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddPTDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddPTDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddPTDetail.Location = new System.Drawing.Point(1075, 30);
		this.BtnAddPTDetail.Name = "BtnAddPTDetail";
		this.BtnAddPTDetail.Size = new System.Drawing.Size(31, 23);
		this.BtnAddPTDetail.TabIndex = 7;
		this.BtnAddPTDetail.Text = "+";
		this.BtnAddPTDetail.UseVisualStyleBackColor = false;
		this.BtnAddPTDetail.Click += new System.EventHandler(btndadd_Click);
		this.BtnAddPTDetail.Leave += new System.EventHandler(btndadd_Leave);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(812, 10);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(52, 15);
		this.label58.TabIndex = 593;
		this.label58.Text = "Remarks";
		this.txtRemarksPTDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksPTDetail.Location = new System.Drawing.Point(812, 30);
		this.txtRemarksPTDetail.Name = "txtRemarksPTDetail";
		this.txtRemarksPTDetail.Size = new System.Drawing.Size(261, 22);
		this.txtRemarksPTDetail.TabIndex = 6;
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.ForeColor = System.Drawing.Color.Red;
		this.label82.Location = new System.Drawing.Point(673, 9);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(14, 17);
		this.label82.TabIndex = 591;
		this.label82.Text = "*";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(187, 10);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(140, 15);
		this.label75.TabIndex = 508;
		this.label75.Text = "Financial Instruments No";
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.Red;
		this.label81.Location = new System.Drawing.Point(589, 9);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(14, 17);
		this.label81.TabIndex = 590;
		this.label81.Text = "*";
		this.CmbFinInstrumentsPTDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbFinInstrumentsPTDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFinInstrumentsPTDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinInstrumentsPTDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance181;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance292).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance359).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance359).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance359;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance436).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance436;
		((AppearanceBase)appearance447).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance447).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance447;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance458).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance458).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance458).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance458).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance458).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance458;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinInstrumentsPTDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetail).Location = new System.Drawing.Point(187, 29);
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetail).Name = "CmbFinInstrumentsPTDetail";
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetail).Size = new System.Drawing.Size(182, 25);
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetail).TabIndex = 1;
		this.CmbFinInstrumentsPTDetail.ValueChanged += new System.EventHandler(dcmbfino_ValueChanged);
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.ForeColor = System.Drawing.Color.Red;
		this.label80.Location = new System.Drawing.Point(101, 9);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(14, 17);
		this.label80.TabIndex = 589;
		this.label80.Text = "*";
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.Location = new System.Drawing.Point(521, 10);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(60, 15);
		this.label74.TabIndex = 509;
		this.label74.Text = "% of Total";
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.ForeColor = System.Drawing.Color.Red;
		this.label79.Location = new System.Drawing.Point(333, 9);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(14, 17);
		this.label79.TabIndex = 588;
		this.label79.Text = "*";
		this.CmbPaymentTermPTDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermPTDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermPTDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermPTDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermPTDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetail).Location = new System.Drawing.Point(4, 29);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetail).Name = "CmbPaymentTermPTDetail";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetail).Size = new System.Drawing.Size(182, 25);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetail).TextChanged += new System.EventHandler(CmbPaymentTermDetail_TextChanged);
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(4, 10);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(84, 15);
		this.label76.TabIndex = 511;
		this.label76.Text = "Payment Term";
		this.txtPrcntOfTotalPTDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPrcntOfTotalPTDetail.Location = new System.Drawing.Point(521, 30);
		this.txtPrcntOfTotalPTDetail.Name = "txtPrcntOfTotalPTDetail";
		this.txtPrcntOfTotalPTDetail.Size = new System.Drawing.Size(82, 22);
		this.txtPrcntOfTotalPTDetail.TabIndex = 3;
		this.txtPrcntOfTotalPTDetail.Text = "0";
		this.txtPrcntOfTotalPTDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPrcntOfTotalPTDetail.Leave += new System.EventHandler(dtxtpercentoftotal_TextChanged);
		this.BtnCancelPTDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelPTDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelPTDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelPTDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelPTDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelPTDetail.Location = new System.Drawing.Point(1075, 4);
		this.BtnCancelPTDetail.Name = "BtnCancelPTDetail";
		this.BtnCancelPTDetail.Size = new System.Drawing.Size(63, 23);
		this.BtnCancelPTDetail.TabIndex = 9;
		this.BtnCancelPTDetail.Text = "Cancel";
		this.BtnCancelPTDetail.UseVisualStyleBackColor = false;
		this.BtnCancelPTDetail.Visible = false;
		this.BtnCancelPTDetail.Click += new System.EventHandler(btndcancel_Click);
		this.BtnCancelPTDetail.Leave += new System.EventHandler(btndcancel_Leave);
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(370, 10);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(48, 15);
		this.label83.TabIndex = 509;
		this.label83.Text = "Balance";
		this.BtnUpdatePTDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdatePTDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdatePTDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdatePTDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdatePTDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdatePTDetail.Location = new System.Drawing.Point(1075, 30);
		this.BtnUpdatePTDetail.Name = "BtnUpdatePTDetail";
		this.BtnUpdatePTDetail.Size = new System.Drawing.Size(63, 23);
		this.BtnUpdatePTDetail.TabIndex = 8;
		this.BtnUpdatePTDetail.Text = "Update";
		this.BtnUpdatePTDetail.UseVisualStyleBackColor = false;
		this.BtnUpdatePTDetail.Visible = false;
		this.BtnUpdatePTDetail.Click += new System.EventHandler(btndupdate_Click);
		this.txtBalanceFIPTDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceFIPTDetail.Location = new System.Drawing.Point(370, 30);
		this.txtBalanceFIPTDetail.Name = "txtBalanceFIPTDetail";
		this.txtBalanceFIPTDetail.ReadOnly = true;
		this.txtBalanceFIPTDetail.Size = new System.Drawing.Size(150, 22);
		this.txtBalanceFIPTDetail.TabIndex = 2;
		this.txtBalanceFIPTDetail.TabStop = false;
		this.txtBalanceFIPTDetail.Text = "0";
		this.txtBalanceFIPTDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalanceFIPTDetail.Leave += new System.EventHandler(dtxtpercentoftotal_TextChanged);
		this.txtDueDaysPTDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDaysPTDetail.Location = new System.Drawing.Point(755, 30);
		this.txtDueDaysPTDetail.Name = "txtDueDaysPTDetail";
		this.txtDueDaysPTDetail.Size = new System.Drawing.Size(56, 22);
		this.txtDueDaysPTDetail.TabIndex = 5;
		this.txtDueDaysPTDetail.Text = "0";
		this.txtDueDaysPTDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(753, 10);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(58, 15);
		this.label78.TabIndex = 515;
		this.label78.Text = "Due Days";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(603, 10);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(72, 15);
		this.label77.TabIndex = 513;
		this.label77.Text = "Fcy Amount";
		this.txtFcyAmountPTDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountPTDetail.Location = new System.Drawing.Point(604, 30);
		this.txtFcyAmountPTDetail.Name = "txtFcyAmountPTDetail";
		this.txtFcyAmountPTDetail.Size = new System.Drawing.Size(150, 22);
		this.txtFcyAmountPTDetail.TabIndex = 4;
		this.txtFcyAmountPTDetail.Text = "0";
		this.txtFcyAmountPTDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountPTDetail.Leave += new System.EventHandler(dtxtfcyamount_TextChanged);
		this.tabChargesDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail);
		this.tabChargesDetail.Controls.Add(this.panel6);
		this.tabChargesDetail.Controls.Add(this.panel13);
		this.tabChargesDetail.Location = new System.Drawing.Point(4, 25);
		this.tabChargesDetail.Name = "tabChargesDetail";
		this.tabChargesDetail.Size = new System.Drawing.Size(1148, 176);
		this.tabChargesDetail.TabIndex = 3;
		this.tabChargesDetail.Text = "Other Charges Detail";
		this.tabChargesDetail.UseVisualStyleBackColor = true;
		this.grdOtherChargesDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOtherChargesDetail).BackColor = System.Drawing.Color.White;
		grdOtherChargesDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherChargesDetail.DesignTimeLayout = grdOtherChargesDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherChargesDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Location = new System.Drawing.Point(0, 77);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Name = "grdOtherChargesDetail";
		((ScrollableBase)this.grdOtherChargesDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdOtherChargesDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdOtherChargesDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Size = new System.Drawing.Size(1148, 99);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).TabIndex = 1;
		this.grdOtherChargesDetail.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesDetail).VisualStyle = (VisualStyle)4;
		this.grdOtherChargesDetail.ColumnHeaderClick += new ColumnActionEventHandler(grdOtherChargesDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).DoubleClick += new System.EventHandler(grdOtherChargesDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOtherChargesDetail_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlgrdOtherChargesDetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 54);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1148, 23);
		this.panel6.TabIndex = 2;
		this.ctrlgrdOtherChargesDetail.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdOtherChargesDetail.FormName = this;
		this.ctrlgrdOtherChargesDetail.Location = new System.Drawing.Point(1106, 0);
		this.ctrlgrdOtherChargesDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdOtherChargesDetail.MyGrid = this.grdOtherChargesDetail;
		this.ctrlgrdOtherChargesDetail.Name = "ctrlgrdOtherChargesDetail";
		this.ctrlgrdOtherChargesDetail.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdOtherChargesDetail.TabIndex = 27;
		this.ctrlgrdOtherChargesDetail.Load += new System.EventHandler(ctrlgrdOtherChargesDetail_Load);
		this.panel13.Controls.Add(this.BtnAddChargeDetail);
		this.panel13.Controls.Add(this.label130);
		this.panel13.Controls.Add(this.txtRemarksChargeDetail);
		this.panel13.Controls.Add(this.label134);
		this.panel13.Controls.Add(this.label131);
		this.panel13.Controls.Add(this.label132);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetail);
		this.panel13.Controls.Add(this.label136);
		this.panel13.Controls.Add(this.BtnCancelChargeDetail);
		this.panel13.Controls.Add(this.BtnUpdateChargeDetail);
		this.panel13.Controls.Add(this.label133);
		this.panel13.Controls.Add(this.label140);
		this.panel13.Controls.Add(this.txtLessAmountChargeDetail);
		this.panel13.Controls.Add(this.txtAddAmountChargeDetail);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1148, 54);
		this.panel13.TabIndex = 0;
		this.BtnAddChargeDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnAddChargeDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddChargeDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddChargeDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddChargeDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddChargeDetail.Location = new System.Drawing.Point(1080, 26);
		this.BtnAddChargeDetail.Name = "BtnAddChargeDetail";
		this.BtnAddChargeDetail.Size = new System.Drawing.Size(31, 23);
		this.BtnAddChargeDetail.TabIndex = 4;
		this.BtnAddChargeDetail.Text = "+";
		this.BtnAddChargeDetail.UseVisualStyleBackColor = false;
		this.BtnAddChargeDetail.Click += new System.EventHandler(BtnAddChargeDetail_Click);
		this.label130.AutoSize = true;
		this.label130.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label130.Location = new System.Drawing.Point(435, 8);
		this.label130.Name = "label130";
		this.label130.Size = new System.Drawing.Size(52, 15);
		this.label130.TabIndex = 593;
		this.label130.Text = "Remarks";
		this.txtRemarksChargeDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksChargeDetail.Location = new System.Drawing.Point(435, 26);
		this.txtRemarksChargeDetail.Name = "txtRemarksChargeDetail";
		this.txtRemarksChargeDetail.Size = new System.Drawing.Size(642, 22);
		this.txtRemarksChargeDetail.TabIndex = 3;
		this.label134.AutoSize = true;
		this.label134.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label134.ForeColor = System.Drawing.Color.Red;
		this.label134.Location = new System.Drawing.Point(399, 7);
		this.label134.Name = "label134";
		this.label134.Size = new System.Drawing.Size(14, 17);
		this.label134.TabIndex = 591;
		this.label134.Text = "*";
		this.label131.AutoSize = true;
		this.label131.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label131.ForeColor = System.Drawing.Color.Red;
		this.label131.Location = new System.Drawing.Point(289, 7);
		this.label131.Name = "label131";
		this.label131.Size = new System.Drawing.Size(14, 17);
		this.label131.TabIndex = 591;
		this.label131.Text = "*";
		this.label132.AutoSize = true;
		this.label132.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label132.Location = new System.Drawing.Point(3, 8);
		this.label132.Name = "label132";
		this.label132.Size = new System.Drawing.Size(70, 15);
		this.label132.TabIndex = 508;
		this.label132.Text = "Charges List";
		this.CmbChargesNameChargeDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesNameChargeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesNameChargeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesNameChargeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesNameChargeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetail).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetail).Name = "CmbChargesNameChargeDetail";
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetail).Size = new System.Drawing.Size(211, 25);
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetail).TabIndex = 0;
		this.label136.AutoSize = true;
		this.label136.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label136.ForeColor = System.Drawing.Color.Red;
		this.label136.Location = new System.Drawing.Point(72, 8);
		this.label136.Name = "label136";
		this.label136.Size = new System.Drawing.Size(14, 17);
		this.label136.TabIndex = 588;
		this.label136.Text = "*";
		this.BtnCancelChargeDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelChargeDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelChargeDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelChargeDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelChargeDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelChargeDetail.Location = new System.Drawing.Point(1080, 2);
		this.BtnCancelChargeDetail.Name = "BtnCancelChargeDetail";
		this.BtnCancelChargeDetail.Size = new System.Drawing.Size(63, 23);
		this.BtnCancelChargeDetail.TabIndex = 6;
		this.BtnCancelChargeDetail.Text = "Cancel";
		this.BtnCancelChargeDetail.UseVisualStyleBackColor = false;
		this.BtnCancelChargeDetail.Visible = false;
		this.BtnCancelChargeDetail.Click += new System.EventHandler(BtnCancelChargeDetail_Click);
		this.BtnUpdateChargeDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateChargeDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdateChargeDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateChargeDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateChargeDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateChargeDetail.Location = new System.Drawing.Point(1080, 26);
		this.BtnUpdateChargeDetail.Name = "BtnUpdateChargeDetail";
		this.BtnUpdateChargeDetail.Size = new System.Drawing.Size(63, 23);
		this.BtnUpdateChargeDetail.TabIndex = 5;
		this.BtnUpdateChargeDetail.Text = "Update";
		this.BtnUpdateChargeDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateChargeDetail.Visible = false;
		this.BtnUpdateChargeDetail.Click += new System.EventHandler(BtnUpdateChargeDetail_Click);
		this.label133.AutoSize = true;
		this.label133.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label133.Location = new System.Drawing.Point(324, 8);
		this.label133.Name = "label133";
		this.label133.Size = new System.Drawing.Size(76, 15);
		this.label133.TabIndex = 513;
		this.label133.Text = "Less Amount";
		this.label140.AutoSize = true;
		this.label140.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label140.Location = new System.Drawing.Point(214, 8);
		this.label140.Name = "label140";
		this.label140.Size = new System.Drawing.Size(76, 15);
		this.label140.TabIndex = 513;
		this.label140.Text = "Add Amount";
		this.txtLessAmountChargeDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLessAmountChargeDetail.Location = new System.Drawing.Point(325, 26);
		this.txtLessAmountChargeDetail.Name = "txtLessAmountChargeDetail";
		this.txtLessAmountChargeDetail.Size = new System.Drawing.Size(109, 22);
		this.txtLessAmountChargeDetail.TabIndex = 2;
		this.txtLessAmountChargeDetail.Text = "0";
		this.txtLessAmountChargeDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLessAmountChargeDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLessAmountChargeDetail_KeyPress);
		this.txtAddAmountChargeDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddAmountChargeDetail.Location = new System.Drawing.Point(215, 26);
		this.txtAddAmountChargeDetail.Name = "txtAddAmountChargeDetail";
		this.txtAddAmountChargeDetail.Size = new System.Drawing.Size(109, 22);
		this.txtAddAmountChargeDetail.TabIndex = 1;
		this.txtAddAmountChargeDetail.Text = "0";
		this.txtAddAmountChargeDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddAmountChargeDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddAmountChargeDetail_KeyPress);
		this.tabPackingList.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackListDetail);
		this.tabPackingList.Controls.Add(this.panel25);
		this.tabPackingList.Controls.Add(this.panel26);
		this.tabPackingList.Location = new System.Drawing.Point(4, 25);
		this.tabPackingList.Name = "tabPackingList";
		this.tabPackingList.Size = new System.Drawing.Size(1148, 176);
		this.tabPackingList.TabIndex = 4;
		this.tabPackingList.Text = "Packing List";
		this.tabPackingList.UseVisualStyleBackColor = true;
		this.grdPackListDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPackListDetail).BackColor = System.Drawing.Color.White;
		grdPackListDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackListDetail.DesignTimeLayout = grdPackListDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackListDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Location = new System.Drawing.Point(0, 118);
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Name = "grdPackListDetail";
		((ScrollableBase)this.grdPackListDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdPackListDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdPackListDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Size = new System.Drawing.Size(1148, 58);
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).TabIndex = 1;
		this.grdPackListDetail.TotalRow = (InheritableBoolean)1;
		this.grdPackListDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackListDetail).VisualStyle = (VisualStyle)4;
		this.grdPackListDetail.ColumnButtonClick += new ColumnActionEventHandler(grdPackListDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).DoubleClick += new System.EventHandler(grdPackListDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPackListDetail_KeyDown);
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.ctrlgrdPackListDetail);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 95);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1148, 23);
		this.panel25.TabIndex = 2;
		this.ctrlgrdPackListDetail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdPackListDetail.FormName = this;
		this.ctrlgrdPackListDetail.Location = new System.Drawing.Point(1106, 1);
		this.ctrlgrdPackListDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdPackListDetail.MyGrid = this.grdPackListDetail;
		this.ctrlgrdPackListDetail.Name = "ctrlgrdPackListDetail";
		this.ctrlgrdPackListDetail.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdPackListDetail.TabIndex = 27;
		this.ctrlgrdPackListDetail.Load += new System.EventHandler(ctrlgrdPackListDetail_Load);
		this.panel26.Controls.Add(this.RadItemCodePackListDetail);
		this.panel26.Controls.Add(this.RadItemNamePackListDetail);
		this.panel26.Controls.Add(this.label160);
		this.panel26.Controls.Add(this.label159);
		this.panel26.Controls.Add(this.label158);
		this.panel26.Controls.Add(this.BtnAddPackListDetail);
		this.panel26.Controls.Add(this.txtSubLotNoPackListDetail);
		this.panel26.Controls.Add(this.label151);
		this.panel26.Controls.Add(this.txtNetWeightPackListDetail);
		this.panel26.Controls.Add(this.label150);
		this.panel26.Controls.Add(this.txtGrossWeightPackListDetail);
		this.panel26.Controls.Add(this.label149);
		this.panel26.Controls.Add(this.txtOuterQtyPackListDetail);
		this.panel26.Controls.Add(this.label147);
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail);
		this.panel26.Controls.Add(this.label144);
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail);
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail);
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail);
		this.panel26.Controls.Add(this.txtPmDetailPackListDetail);
		this.panel26.Controls.Add(this.txtCommodityPackListDetail);
		this.panel26.Controls.Add(this.label135);
		this.panel26.Controls.Add(this.label137);
		this.panel26.Controls.Add(this.label84);
		this.panel26.Controls.Add(this.label138);
		this.panel26.Controls.Add(this.label139);
		this.panel26.Controls.Add(this.label141);
		this.panel26.Controls.Add(this.txtRemarksPackListDetail);
		this.panel26.Controls.Add(this.label157);
		this.panel26.Controls.Add(this.label156);
		this.panel26.Controls.Add(this.label29);
		this.panel26.Controls.Add(this.label18);
		this.panel26.Controls.Add(this.label85);
		this.panel26.Controls.Add(this.label145);
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail);
		this.panel26.Controls.Add(this.txtLotNoPackListDetail);
		this.panel26.Controls.Add(this.label148);
		this.panel26.Controls.Add(this.BtnCancelPackListDetail);
		this.panel26.Controls.Add(this.BtnUpdatePackListDetail);
		this.panel26.Controls.Add(this.label146);
		this.panel26.Controls.Add(this.txtExpiryDatePackListDetail);
		this.panel26.Controls.Add(this.label143);
		this.panel26.Controls.Add(this.txtPackageDatePackListDetail);
		this.panel26.Controls.Add(this.label142);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel26.Location = new System.Drawing.Point(0, 0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1148, 95);
		this.panel26.TabIndex = 0;
		this.RadItemCodePackListDetail.AutoSize = true;
		this.RadItemCodePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemCodePackListDetail.Location = new System.Drawing.Point(235, 3);
		this.RadItemCodePackListDetail.Name = "RadItemCodePackListDetail";
		this.RadItemCodePackListDetail.Size = new System.Drawing.Size(52, 19);
		this.RadItemCodePackListDetail.TabIndex = 623;
		this.RadItemCodePackListDetail.Text = "Code";
		this.RadItemCodePackListDetail.UseVisualStyleBackColor = true;
		this.RadItemNamePackListDetail.AutoSize = true;
		this.RadItemNamePackListDetail.Checked = true;
		this.RadItemNamePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemNamePackListDetail.Location = new System.Drawing.Point(179, 3);
		this.RadItemNamePackListDetail.Name = "RadItemNamePackListDetail";
		this.RadItemNamePackListDetail.Size = new System.Drawing.Size(57, 19);
		this.RadItemNamePackListDetail.TabIndex = 622;
		this.RadItemNamePackListDetail.TabStop = true;
		this.RadItemNamePackListDetail.Text = "Name";
		this.RadItemNamePackListDetail.UseVisualStyleBackColor = true;
		this.label160.AutoSize = true;
		this.label160.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label160.ForeColor = System.Drawing.Color.Red;
		this.label160.Location = new System.Drawing.Point(808, 49);
		this.label160.Name = "label160";
		this.label160.Size = new System.Drawing.Size(14, 17);
		this.label160.TabIndex = 621;
		this.label160.Text = "*";
		this.label159.AutoSize = true;
		this.label159.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label159.ForeColor = System.Drawing.Color.Red;
		this.label159.Location = new System.Drawing.Point(697, 49);
		this.label159.Name = "label159";
		this.label159.Size = new System.Drawing.Size(14, 17);
		this.label159.TabIndex = 620;
		this.label159.Text = "*";
		this.label158.AutoSize = true;
		this.label158.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label158.ForeColor = System.Drawing.Color.Red;
		this.label158.Location = new System.Drawing.Point(597, 49);
		this.label158.Name = "label158";
		this.label158.Size = new System.Drawing.Size(14, 17);
		this.label158.TabIndex = 589;
		this.label158.Text = "*";
		this.BtnAddPackListDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnAddPackListDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddPackListDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddPackListDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddPackListDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddPackListDetail.Location = new System.Drawing.Point(1023, 67);
		this.BtnAddPackListDetail.Name = "BtnAddPackListDetail";
		this.BtnAddPackListDetail.Size = new System.Drawing.Size(31, 22);
		this.BtnAddPackListDetail.TabIndex = 15;
		this.BtnAddPackListDetail.Text = "+";
		this.BtnAddPackListDetail.UseVisualStyleBackColor = false;
		this.BtnAddPackListDetail.Click += new System.EventHandler(BtnAddPackListDetail_Click);
		this.txtSubLotNoPackListDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSubLotNoPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubLotNoPackListDetail.Location = new System.Drawing.Point(433, 67);
		this.txtSubLotNoPackListDetail.Name = "txtSubLotNoPackListDetail";
		this.txtSubLotNoPackListDetail.Size = new System.Drawing.Size(106, 22);
		this.txtSubLotNoPackListDetail.TabIndex = 10;
		this.label151.AutoSize = true;
		this.label151.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label151.Location = new System.Drawing.Point(433, 50);
		this.label151.Name = "label151";
		this.label151.Size = new System.Drawing.Size(67, 15);
		this.label151.TabIndex = 619;
		this.label151.Text = "Sub Lot No";
		this.txtNetWeightPackListDetail.BackColor = System.Drawing.Color.White;
		this.txtNetWeightPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightPackListDetail.Location = new System.Drawing.Point(614, 67);
		this.txtNetWeightPackListDetail.Name = "txtNetWeightPackListDetail";
		this.txtNetWeightPackListDetail.Size = new System.Drawing.Size(105, 22);
		this.txtNetWeightPackListDetail.TabIndex = 12;
		this.txtNetWeightPackListDetail.Text = "0";
		this.txtNetWeightPackListDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeightPackListDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetWeightPackListDetail_KeyPress);
		this.label150.AutoSize = true;
		this.label150.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label150.Location = new System.Drawing.Point(614, 50);
		this.label150.Name = "label150";
		this.label150.Size = new System.Drawing.Size(68, 15);
		this.label150.TabIndex = 617;
		this.label150.Text = "Net Weight";
		this.txtGrossWeightPackListDetail.BackColor = System.Drawing.Color.White;
		this.txtGrossWeightPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeightPackListDetail.Location = new System.Drawing.Point(721, 67);
		this.txtGrossWeightPackListDetail.Name = "txtGrossWeightPackListDetail";
		this.txtGrossWeightPackListDetail.Size = new System.Drawing.Size(105, 22);
		this.txtGrossWeightPackListDetail.TabIndex = 13;
		this.txtGrossWeightPackListDetail.Text = "0";
		this.txtGrossWeightPackListDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeightPackListDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeightPackListDetail_KeyPress);
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(721, 50);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(78, 15);
		this.label149.TabIndex = 617;
		this.label149.Text = "Gross Weight";
		this.txtOuterQtyPackListDetail.BackColor = System.Drawing.Color.White;
		this.txtOuterQtyPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOuterQtyPackListDetail.Location = new System.Drawing.Point(540, 67);
		this.txtOuterQtyPackListDetail.Name = "txtOuterQtyPackListDetail";
		this.txtOuterQtyPackListDetail.Size = new System.Drawing.Size(73, 22);
		this.txtOuterQtyPackListDetail.TabIndex = 11;
		this.txtOuterQtyPackListDetail.Text = "0";
		this.txtOuterQtyPackListDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOuterQtyPackListDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOuterQtyPackListDetail_KeyPress);
		this.label147.AutoSize = true;
		this.label147.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label147.Location = new System.Drawing.Point(540, 50);
		this.label147.Name = "label147";
		this.label147.Size = new System.Drawing.Size(59, 15);
		this.label147.TabIndex = 617;
		this.label147.Text = "Outer Qty";
		this.CmbInspectionNoPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbInspectionNoPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInspectionNoPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInspectionNoPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance175;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInspectionNoPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Location = new System.Drawing.Point(332, 65);
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Name = "CmbInspectionNoPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).TabIndex = 9;
		this.label144.AutoSize = true;
		this.label144.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label144.Location = new System.Drawing.Point(332, 50);
		this.label144.Name = "label144";
		this.label144.Size = new System.Drawing.Size(82, 15);
		this.label144.TabIndex = 615;
		this.label144.Text = "Inspection No";
		this.CmbPmDetailByItemCusPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPmDetailByItemCusPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPmDetailByItemCusPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPmDetailByItemCusPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance234;
		((SpecialBoxBase)((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance258).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance259).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance263).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPmDetailByItemCusPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Location = new System.Drawing.Point(734, 22);
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Name = "CmbPmDetailByItemCusPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Leave += new System.EventHandler(CmbPmDetailByItemCusPackListDetail_Leave);
		this.CmbItemFromInvDetailPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemFromInvDetailPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemFromInvDetailPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemFromInvDetailPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance437).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance437).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance437;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance438).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance438).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance438).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance438).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance438;
		((AppearanceBase)appearance439).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance439;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemFromInvDetailPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Location = new System.Drawing.Point(146, 22);
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Name = "CmbItemFromInvDetailPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Leave += new System.EventHandler(CmbItemFromInvDetailPackListDetail_Leave);
		this.CmbCommodityByItemCusPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCommodityByItemCusPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommodityByItemCusPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommodityByItemCusPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommodityByItemCusPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Location = new System.Drawing.Point(307, 22);
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Name = "CmbCommodityByItemCusPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Leave += new System.EventHandler(CmbCommodityByItemCusPackListDetail_Leave);
		this.txtPmDetailPackListDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPmDetailPackListDetail.Location = new System.Drawing.Point(935, 24);
		this.txtPmDetailPackListDetail.Name = "txtPmDetailPackListDetail";
		this.txtPmDetailPackListDetail.Size = new System.Drawing.Size(211, 23);
		this.txtPmDetailPackListDetail.TabIndex = 5;
		this.txtCommodityPackListDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommodityPackListDetail.Location = new System.Drawing.Point(508, 24);
		this.txtCommodityPackListDetail.Name = "txtCommodityPackListDetail";
		this.txtCommodityPackListDetail.Size = new System.Drawing.Size(225, 23);
		this.txtCommodityPackListDetail.TabIndex = 3;
		this.label135.AutoSize = true;
		this.label135.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label135.Location = new System.Drawing.Point(935, 5);
		this.label135.Name = "label135";
		this.label135.Size = new System.Drawing.Size(59, 15);
		this.label135.TabIndex = 600;
		this.label135.Text = "Pm Detail";
		this.label137.AutoSize = true;
		this.label137.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label137.Location = new System.Drawing.Point(508, 5);
		this.label137.Name = "label137";
		this.label137.Size = new System.Drawing.Size(104, 15);
		this.label137.TabIndex = 601;
		this.label137.Text = "Commodity Detail";
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.Location = new System.Drawing.Point(146, 5);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(32, 15);
		this.label84.TabIndex = 599;
		this.label84.Text = "Item";
		this.label138.AutoSize = true;
		this.label138.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label138.Location = new System.Drawing.Point(734, 5);
		this.label138.Name = "label138";
		this.label138.Size = new System.Drawing.Size(103, 15);
		this.label138.TabIndex = 598;
		this.label138.Text = "Pm Detail By Item";
		this.label139.AutoSize = true;
		this.label139.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label139.Location = new System.Drawing.Point(307, 5);
		this.label139.Name = "label139";
		this.label139.Size = new System.Drawing.Size(144, 15);
		this.label139.TabIndex = 599;
		this.label139.Text = "Comoodity Detail By Item";
		this.label141.AutoSize = true;
		this.label141.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label141.Location = new System.Drawing.Point(827, 50);
		this.label141.Name = "label141";
		this.label141.Size = new System.Drawing.Size(52, 15);
		this.label141.TabIndex = 593;
		this.label141.Text = "Remarks";
		this.txtRemarksPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksPackListDetail.Location = new System.Drawing.Point(827, 67);
		this.txtRemarksPackListDetail.Name = "txtRemarksPackListDetail";
		this.txtRemarksPackListDetail.Size = new System.Drawing.Size(194, 22);
		this.txtRemarksPackListDetail.TabIndex = 14;
		this.label157.AutoSize = true;
		this.label157.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label157.ForeColor = System.Drawing.Color.Red;
		this.label157.Location = new System.Drawing.Point(179, 49);
		this.label157.Name = "label157";
		this.label157.Size = new System.Drawing.Size(14, 17);
		this.label157.TabIndex = 589;
		this.label157.Text = "*";
		this.label156.AutoSize = true;
		this.label156.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label156.ForeColor = System.Drawing.Color.Red;
		this.label156.Location = new System.Drawing.Point(87, 49);
		this.label156.Name = "label156";
		this.label156.Size = new System.Drawing.Size(14, 17);
		this.label156.TabIndex = 589;
		this.label156.Text = "*";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(1000, 4);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(14, 17);
		this.label29.TabIndex = 589;
		this.label29.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(623, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 589;
		this.label18.Text = "*";
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.ForeColor = System.Drawing.Color.Red;
		this.label85.Location = new System.Drawing.Point(285, 4);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(14, 17);
		this.label85.TabIndex = 589;
		this.label85.Text = "*";
		this.label145.AutoSize = true;
		this.label145.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label145.ForeColor = System.Drawing.Color.Red;
		this.label145.Location = new System.Drawing.Point(82, 4);
		this.label145.Name = "label145";
		this.label145.Size = new System.Drawing.Size(14, 17);
		this.label145.TabIndex = 589;
		this.label145.Text = "*";
		this.CmbContainerNoPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbContainerNoPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContainerNoPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContainerNoPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).Location = new System.Drawing.Point(5, 23);
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).Name = "CmbContainerNoPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).Size = new System.Drawing.Size(140, 25);
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).TabIndex = 0;
		this.txtLotNoPackListDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLotNoPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLotNoPackListDetail.Location = new System.Drawing.Point(225, 67);
		this.txtLotNoPackListDetail.Name = "txtLotNoPackListDetail";
		this.txtLotNoPackListDetail.Size = new System.Drawing.Size(106, 22);
		this.txtLotNoPackListDetail.TabIndex = 8;
		this.label148.AutoSize = true;
		this.label148.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label148.Location = new System.Drawing.Point(5, 5);
		this.label148.Name = "label148";
		this.label148.Size = new System.Drawing.Size(77, 15);
		this.label148.TabIndex = 511;
		this.label148.Text = "Container No";
		this.BtnCancelPackListDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelPackListDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelPackListDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelPackListDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelPackListDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelPackListDetail.Location = new System.Drawing.Point(1088, 67);
		this.BtnCancelPackListDetail.Name = "BtnCancelPackListDetail";
		this.BtnCancelPackListDetail.Size = new System.Drawing.Size(57, 22);
		this.BtnCancelPackListDetail.TabIndex = 17;
		this.BtnCancelPackListDetail.Text = "Cancel";
		this.BtnCancelPackListDetail.UseVisualStyleBackColor = false;
		this.BtnCancelPackListDetail.Visible = false;
		this.BtnCancelPackListDetail.Click += new System.EventHandler(BtnCancelPackListDetail_Click);
		this.BtnUpdatePackListDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdatePackListDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdatePackListDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdatePackListDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdatePackListDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdatePackListDetail.Location = new System.Drawing.Point(1023, 67);
		this.BtnUpdatePackListDetail.Name = "BtnUpdatePackListDetail";
		this.BtnUpdatePackListDetail.Size = new System.Drawing.Size(63, 22);
		this.BtnUpdatePackListDetail.TabIndex = 16;
		this.BtnUpdatePackListDetail.Text = "Update";
		this.BtnUpdatePackListDetail.UseVisualStyleBackColor = false;
		this.BtnUpdatePackListDetail.Visible = false;
		this.BtnUpdatePackListDetail.Click += new System.EventHandler(BtnUpdatePackListDetail_Click);
		this.label146.AutoSize = true;
		this.label146.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label146.Location = new System.Drawing.Point(225, 50);
		this.label146.Name = "label146";
		this.label146.Size = new System.Drawing.Size(43, 15);
		this.label146.TabIndex = 57;
		this.label146.Text = "Lot No";
		this.txtExpiryDatePackListDetail.CustomFormat = "dd-MMM-yy";
		this.txtExpiryDatePackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpiryDatePackListDetail.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtExpiryDatePackListDetail.Location = new System.Drawing.Point(115, 67);
		this.txtExpiryDatePackListDetail.Name = "txtExpiryDatePackListDetail";
		this.txtExpiryDatePackListDetail.Size = new System.Drawing.Size(109, 22);
		this.txtExpiryDatePackListDetail.TabIndex = 7;
		this.label143.AutoSize = true;
		this.label143.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label143.Location = new System.Drawing.Point(112, 50);
		this.label143.Name = "label143";
		this.label143.Size = new System.Drawing.Size(67, 15);
		this.label143.TabIndex = 590;
		this.label143.Text = "Expiry Date";
		this.txtPackageDatePackListDetail.CustomFormat = "dd-MMM-yy";
		this.txtPackageDatePackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackageDatePackListDetail.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPackageDatePackListDetail.Location = new System.Drawing.Point(5, 67);
		this.txtPackageDatePackListDetail.Name = "txtPackageDatePackListDetail";
		this.txtPackageDatePackListDetail.Size = new System.Drawing.Size(109, 22);
		this.txtPackageDatePackListDetail.TabIndex = 6;
		this.label142.AutoSize = true;
		this.label142.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label142.Location = new System.Drawing.Point(5, 50);
		this.label142.Name = "label142";
		this.label142.Size = new System.Drawing.Size(79, 15);
		this.label142.TabIndex = 590;
		this.label142.Text = "Package Date";
		this.tabOtherItem.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.tabOtherItem.Controls.Add(this.panel18);
		this.tabOtherItem.Controls.Add(this.panel19);
		this.tabOtherItem.Location = new System.Drawing.Point(4, 25);
		this.tabOtherItem.Name = "tabOtherItem";
		this.tabOtherItem.Padding = new System.Windows.Forms.Padding(3);
		this.tabOtherItem.Size = new System.Drawing.Size(1148, 176);
		this.tabOtherItem.TabIndex = 1;
		this.tabOtherItem.Text = "Other Items";
		this.tabOtherItem.UseVisualStyleBackColor = true;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(3, 79);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdotheritems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1142, 94);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 1;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		this.grdotheritems.ColumnButtonClick += new ColumnActionEventHandler(grdotheritems_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).KeyDown += new System.Windows.Forms.KeyEventHandler(grdotheritems_KeyDown);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlgrdotheritems);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 56);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1142, 23);
		this.panel18.TabIndex = 0;
		this.ctrlgrdotheritems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdotheritems.FormName = this;
		this.ctrlgrdotheritems.Location = new System.Drawing.Point(1099, 1);
		this.ctrlgrdotheritems.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdotheritems.MyGrid = this.grdotheritems;
		this.ctrlgrdotheritems.Name = "ctrlgrdotheritems";
		this.ctrlgrdotheritems.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdotheritems.TabIndex = 26;
		this.ctrlgrdotheritems.Load += new System.EventHandler(ctrlgrdotheritems_Load);
		this.panel19.Controls.Add(this.RadItemCodeOIDetail);
		this.panel19.Controls.Add(this.RadItemNameOIDetail);
		this.panel19.Controls.Add(this.label11);
		this.panel19.Controls.Add(this.label33);
		this.panel19.Controls.Add(this.label32);
		this.panel19.Controls.Add(this.label30);
		this.panel19.Controls.Add(this.BtnAddOIDetail);
		this.panel19.Controls.Add(this.BtnCancelOIDetail);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemOIDetail);
		this.panel19.Controls.Add(this.BtnUpdateOIDetail);
		this.panel19.Controls.Add(this.label71);
		this.panel19.Controls.Add(this.txtAmountOIDetail);
		this.panel19.Controls.Add(this.label14);
		this.panel19.Controls.Add(this.label70);
		this.panel19.Controls.Add(this.txtRemarksOIDetail);
		this.panel19.Controls.Add(this.label63);
		this.panel19.Controls.Add(this.txtQtyOIDetail);
		this.panel19.Controls.Add(this.txtRateOIDetail);
		this.panel19.Controls.Add(this.label15);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(3, 3);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1142, 53);
		this.panel19.TabIndex = 0;
		this.RadItemCodeOIDetail.AutoSize = true;
		this.RadItemCodeOIDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemCodeOIDetail.Location = new System.Drawing.Point(91, 5);
		this.RadItemCodeOIDetail.Name = "RadItemCodeOIDetail";
		this.RadItemCodeOIDetail.Size = new System.Drawing.Size(52, 19);
		this.RadItemCodeOIDetail.TabIndex = 548;
		this.RadItemCodeOIDetail.Text = "Code";
		this.RadItemCodeOIDetail.UseVisualStyleBackColor = true;
		this.RadItemNameOIDetail.AutoSize = true;
		this.RadItemNameOIDetail.Checked = true;
		this.RadItemNameOIDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemNameOIDetail.Location = new System.Drawing.Point(35, 5);
		this.RadItemNameOIDetail.Name = "RadItemNameOIDetail";
		this.RadItemNameOIDetail.Size = new System.Drawing.Size(57, 19);
		this.RadItemNameOIDetail.TabIndex = 547;
		this.RadItemNameOIDetail.TabStop = true;
		this.RadItemNameOIDetail.Text = "Name";
		this.RadItemNameOIDetail.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Red;
		this.label11.Location = new System.Drawing.Point(500, 8);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(14, 17);
		this.label11.TabIndex = 546;
		this.label11.Text = "*";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(391, 6);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(14, 17);
		this.label33.TabIndex = 546;
		this.label33.Text = "*";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(290, 6);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(14, 17);
		this.label32.TabIndex = 546;
		this.label32.Text = "*";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(141, 6);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(14, 17);
		this.label30.TabIndex = 546;
		this.label30.Text = "*";
		this.BtnAddOIDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnAddOIDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddOIDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddOIDetail.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.BtnAddOIDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddOIDetail.Location = new System.Drawing.Point(1074, 27);
		this.BtnAddOIDetail.Name = "BtnAddOIDetail";
		this.BtnAddOIDetail.Size = new System.Drawing.Size(36, 23);
		this.BtnAddOIDetail.TabIndex = 5;
		this.BtnAddOIDetail.Text = "+";
		this.BtnAddOIDetail.UseVisualStyleBackColor = false;
		this.BtnAddOIDetail.Click += new System.EventHandler(btnoadd_Click);
		this.BtnAddOIDetail.Leave += new System.EventHandler(btnoadd_Leave);
		this.BtnCancelOIDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelOIDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelOIDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelOIDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.BtnCancelOIDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelOIDetail.Location = new System.Drawing.Point(1074, 3);
		this.BtnCancelOIDetail.Name = "BtnCancelOIDetail";
		this.BtnCancelOIDetail.Size = new System.Drawing.Size(63, 23);
		this.BtnCancelOIDetail.TabIndex = 7;
		this.BtnCancelOIDetail.Text = "Cancel";
		this.BtnCancelOIDetail.UseVisualStyleBackColor = false;
		this.BtnCancelOIDetail.Visible = false;
		this.BtnCancelOIDetail.Click += new System.EventHandler(btnoCancel_Click);
		this.BtnCancelOIDetail.Leave += new System.EventHandler(btnoCancel_Leave);
		this.CmbItemOIDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemOIDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemOIDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemOIDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance431).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance431).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance431;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance433).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance433).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance433).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance433).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance433;
		((AppearanceBase)appearance434).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance434;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance242).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance243).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemOIDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemOIDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbItemOIDetail).Location = new System.Drawing.Point(5, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemOIDetail).Name = "CmbItemOIDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemOIDetail).Size = new System.Drawing.Size(253, 25);
		((System.Windows.Forms.Control)(object)this.CmbItemOIDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemOIDetail).TextChanged += new System.EventHandler(CmbOtherItem_TextChanged);
		this.BtnUpdateOIDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateOIDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdateOIDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateOIDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateOIDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateOIDetail.Location = new System.Drawing.Point(1074, 27);
		this.BtnUpdateOIDetail.Name = "BtnUpdateOIDetail";
		this.BtnUpdateOIDetail.Size = new System.Drawing.Size(63, 23);
		this.BtnUpdateOIDetail.TabIndex = 6;
		this.BtnUpdateOIDetail.Text = "Update";
		this.BtnUpdateOIDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateOIDetail.Visible = false;
		this.BtnUpdateOIDetail.Click += new System.EventHandler(btnoUpdate_Click);
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(5, 7);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(32, 15);
		this.label71.TabIndex = 504;
		this.label71.Text = "Item";
		this.txtAmountOIDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountOIDetail.Location = new System.Drawing.Point(447, 27);
		this.txtAmountOIDetail.Name = "txtAmountOIDetail";
		this.txtAmountOIDetail.ReadOnly = true;
		this.txtAmountOIDetail.Size = new System.Drawing.Size(101, 22);
		this.txtAmountOIDetail.TabIndex = 3;
		this.txtAmountOIDetail.Text = "0";
		this.txtAmountOIDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(554, 7);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(52, 15);
		this.label14.TabIndex = 508;
		this.label14.Text = "Remarks";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(448, 7);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(51, 15);
		this.label70.TabIndex = 506;
		this.label70.Text = "Amount";
		this.txtRemarksOIDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksOIDetail.Location = new System.Drawing.Point(549, 27);
		this.txtRemarksOIDetail.Name = "txtRemarksOIDetail";
		this.txtRemarksOIDetail.Size = new System.Drawing.Size(522, 22);
		this.txtRemarksOIDetail.TabIndex = 4;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(363, 7);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(30, 15);
		this.label63.TabIndex = 507;
		this.label63.Text = "Rate";
		this.txtQtyOIDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyOIDetail.Location = new System.Drawing.Point(260, 27);
		this.txtQtyOIDetail.Name = "txtQtyOIDetail";
		this.txtQtyOIDetail.Size = new System.Drawing.Size(101, 22);
		this.txtQtyOIDetail.TabIndex = 1;
		this.txtQtyOIDetail.Text = "0";
		this.txtQtyOIDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyOIDetail.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.txtRateOIDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateOIDetail.Location = new System.Drawing.Point(362, 27);
		this.txtRateOIDetail.Name = "txtRateOIDetail";
		this.txtRateOIDetail.Size = new System.Drawing.Size(84, 22);
		this.txtRateOIDetail.TabIndex = 2;
		this.txtRateOIDetail.Text = "0";
		this.txtRateOIDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateOIDetail.TextChanged += new System.EventHandler(txtoRate_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(263, 7);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(26, 15);
		this.label15.TabIndex = 505;
		this.label15.Text = "Qty";
		this.tabCustomDetail.Controls.Add(this.tabControl5);
		this.tabCustomDetail.Location = new System.Drawing.Point(4, 25);
		this.tabCustomDetail.Name = "tabCustomDetail";
		this.tabCustomDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabCustomDetail.Size = new System.Drawing.Size(1162, 211);
		this.tabCustomDetail.TabIndex = 1;
		this.tabCustomDetail.Text = "Custom Detail";
		this.tabCustomDetail.UseVisualStyleBackColor = true;
		this.tabControl5.Controls.Add(this.tabItemDetailCustom);
		this.tabControl5.Controls.Add(this.tabPaymentDetailCustom);
		this.tabControl5.Controls.Add(this.tabChargesDetailCustom);
		this.tabControl5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl5.Location = new System.Drawing.Point(3, 3);
		this.tabControl5.Name = "tabControl5";
		this.tabControl5.SelectedIndex = 0;
		this.tabControl5.Size = new System.Drawing.Size(1156, 205);
		this.tabControl5.TabIndex = 7;
		this.tabItemDetailCustom.Controls.Add(this.panel9);
		this.tabItemDetailCustom.Location = new System.Drawing.Point(4, 25);
		this.tabItemDetailCustom.Name = "tabItemDetailCustom";
		this.tabItemDetailCustom.Padding = new System.Windows.Forms.Padding(3);
		this.tabItemDetailCustom.Size = new System.Drawing.Size(1148, 176);
		this.tabItemDetailCustom.TabIndex = 0;
		this.tabItemDetailCustom.Text = "Item Detail";
		this.tabItemDetailCustom.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.panel14);
		this.panel9.Controls.Add(this.panel27);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1142, 170);
		this.panel9.TabIndex = 5;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailCustom);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 24);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1142, 146);
		this.panel14.TabIndex = 2;
		this.grdDetailCustom.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetailCustom).BackColor = System.Drawing.Color.White;
		grdDetailCustom_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailCustom.DesignTimeLayout = grdDetailCustom_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailCustom).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetailCustom).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailCustom.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetailCustom).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailCustom).Name = "grdDetailCustom";
		((ScrollableBase)this.grdDetailCustom).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdDetailCustom).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdDetailCustom).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailCustom).Size = new System.Drawing.Size(1142, 146);
		((System.Windows.Forms.Control)(object)this.grdDetailCustom).TabIndex = 0;
		this.grdDetailCustom.TotalRow = (InheritableBoolean)1;
		this.grdDetailCustom.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailCustom).VisualStyle = (VisualStyle)4;
		this.grdDetailCustom.CellUpdated += new ColumnActionEventHandler(grdDetailCustom_CellUpdated);
		this.grdDetailCustom.ColumnButtonClick += new ColumnActionEventHandler(grdDetailCustom_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetailCustom).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetailCustom_KeyDown);
		this.panel27.BackColor = System.Drawing.Color.Teal;
		this.panel27.Controls.Add(this.ctrlgrdDetailCustom);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel27.Location = new System.Drawing.Point(0, 0);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1142, 24);
		this.panel27.TabIndex = 1;
		this.ctrlgrdDetailCustom.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdDetailCustom.FormName = this;
		this.ctrlgrdDetailCustom.Location = new System.Drawing.Point(1103, 1);
		this.ctrlgrdDetailCustom.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdDetailCustom.MyGrid = this.grdDetailCustom;
		this.ctrlgrdDetailCustom.Name = "ctrlgrdDetailCustom";
		this.ctrlgrdDetailCustom.Size = new System.Drawing.Size(38, 21);
		this.ctrlgrdDetailCustom.TabIndex = 5;
		this.ctrlgrdDetailCustom.Load += new System.EventHandler(ctrlgrdDetailCustom_Load);
		this.tabPaymentDetailCustom.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTermDetailCustom);
		this.tabPaymentDetailCustom.Controls.Add(this.panel30);
		this.tabPaymentDetailCustom.Controls.Add(this.panel34);
		this.tabPaymentDetailCustom.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentDetailCustom.Name = "tabPaymentDetailCustom";
		this.tabPaymentDetailCustom.Size = new System.Drawing.Size(1148, 179);
		this.tabPaymentDetailCustom.TabIndex = 4;
		this.tabPaymentDetailCustom.Text = "Payment Detail";
		this.tabPaymentDetailCustom.UseVisualStyleBackColor = true;
		this.grdPaymentTermDetailCustom.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentTermDetailCustom).BackColor = System.Drawing.Color.White;
		grdPaymentTermDetailCustom_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTermDetailCustom.DesignTimeLayout = grdPaymentTermDetailCustom_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetailCustom).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetailCustom).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTermDetailCustom.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetailCustom).Location = new System.Drawing.Point(0, 80);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetailCustom).Name = "grdPaymentTermDetailCustom";
		((ScrollableBase)this.grdPaymentTermDetailCustom).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdPaymentTermDetailCustom).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdPaymentTermDetailCustom).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetailCustom).Size = new System.Drawing.Size(1148, 99);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetailCustom).TabIndex = 11;
		this.grdPaymentTermDetailCustom.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTermDetailCustom.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTermDetailCustom).VisualStyle = (VisualStyle)4;
		this.grdPaymentTermDetailCustom.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTermDetailCustom_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetailCustom).DoubleClick += new System.EventHandler(grdPaymentTermDetailCustom_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermDetailCustom).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTermDetailCustom_KeyDown);
		this.panel30.BackColor = System.Drawing.Color.Teal;
		this.panel30.Controls.Add(this.ctrlgrdPaymentTermDetailCustom);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 57);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1148, 23);
		this.panel30.TabIndex = 9;
		this.ctrlgrdPaymentTermDetailCustom.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdPaymentTermDetailCustom.FormName = this;
		this.ctrlgrdPaymentTermDetailCustom.Location = new System.Drawing.Point(1104, 1);
		this.ctrlgrdPaymentTermDetailCustom.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdPaymentTermDetailCustom.MyGrid = this.grdPaymentTermDetailCustom;
		this.ctrlgrdPaymentTermDetailCustom.Name = "ctrlgrdPaymentTermDetailCustom";
		this.ctrlgrdPaymentTermDetailCustom.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdPaymentTermDetailCustom.TabIndex = 27;
		this.ctrlgrdPaymentTermDetailCustom.Load += new System.EventHandler(ctrlgrdPaymentTermDetailCustom_Load);
		this.panel34.Controls.Add(this.BtnAddPTDetailCustom);
		this.panel34.Controls.Add(this.label88);
		this.panel34.Controls.Add(this.txtRemarksPTDetailCustom);
		this.panel34.Controls.Add(this.label93);
		this.panel34.Controls.Add(this.label94);
		this.panel34.Controls.Add(this.label96);
		this.panel34.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetailCustom);
		this.panel34.Controls.Add(this.label97);
		this.panel34.Controls.Add(this.label98);
		this.panel34.Controls.Add(this.label99);
		this.panel34.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetailCustom);
		this.panel34.Controls.Add(this.label100);
		this.panel34.Controls.Add(this.txtPrcntOfTotalPTDetailCustom);
		this.panel34.Controls.Add(this.BtnCancelPTDetailCustom);
		this.panel34.Controls.Add(this.label101);
		this.panel34.Controls.Add(this.BtnUpdatePTDetailCustom);
		this.panel34.Controls.Add(this.txtBalanceFIPTDetailCustom);
		this.panel34.Controls.Add(this.txtDueDaysPTDetailCustom);
		this.panel34.Controls.Add(this.label102);
		this.panel34.Controls.Add(this.label103);
		this.panel34.Controls.Add(this.txtFcyAmountPTDetailCustom);
		this.panel34.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel34.Location = new System.Drawing.Point(0, 0);
		this.panel34.Name = "panel34";
		this.panel34.Size = new System.Drawing.Size(1148, 57);
		this.panel34.TabIndex = 10;
		this.BtnAddPTDetailCustom.BackColor = System.Drawing.Color.Teal;
		this.BtnAddPTDetailCustom.FlatAppearance.BorderSize = 0;
		this.BtnAddPTDetailCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddPTDetailCustom.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddPTDetailCustom.ForeColor = System.Drawing.Color.White;
		this.BtnAddPTDetailCustom.Location = new System.Drawing.Point(1075, 30);
		this.BtnAddPTDetailCustom.Name = "BtnAddPTDetailCustom";
		this.BtnAddPTDetailCustom.Size = new System.Drawing.Size(31, 23);
		this.BtnAddPTDetailCustom.TabIndex = 7;
		this.BtnAddPTDetailCustom.Text = "+";
		this.BtnAddPTDetailCustom.UseVisualStyleBackColor = false;
		this.BtnAddPTDetailCustom.Click += new System.EventHandler(BtnAddPTDetailCustom_Click);
		this.label88.AutoSize = true;
		this.label88.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label88.Location = new System.Drawing.Point(812, 10);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(52, 15);
		this.label88.TabIndex = 593;
		this.label88.Text = "Remarks";
		this.txtRemarksPTDetailCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksPTDetailCustom.Location = new System.Drawing.Point(812, 30);
		this.txtRemarksPTDetailCustom.Name = "txtRemarksPTDetailCustom";
		this.txtRemarksPTDetailCustom.Size = new System.Drawing.Size(261, 22);
		this.txtRemarksPTDetailCustom.TabIndex = 6;
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label93.ForeColor = System.Drawing.Color.Red;
		this.label93.Location = new System.Drawing.Point(673, 9);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(14, 17);
		this.label93.TabIndex = 591;
		this.label93.Text = "*";
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.Location = new System.Drawing.Point(187, 10);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(140, 15);
		this.label94.TabIndex = 508;
		this.label94.Text = "Financial Instruments No";
		this.label96.AutoSize = true;
		this.label96.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label96.ForeColor = System.Drawing.Color.Red;
		this.label96.Location = new System.Drawing.Point(589, 9);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(14, 17);
		this.label96.TabIndex = 590;
		this.label96.Text = "*";
		this.CmbFinInstrumentsPTDetailCustom.AllowNull = (DefaultableBoolean)1;
		this.CmbFinInstrumentsPTDetailCustom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFinInstrumentsPTDetailCustom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinInstrumentsPTDetailCustom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance250;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance255;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance256).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance401).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance401;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance402).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance402).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance402;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance403).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance403;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinInstrumentsPTDetailCustom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetailCustom).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetailCustom).Location = new System.Drawing.Point(187, 29);
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetailCustom).Name = "CmbFinInstrumentsPTDetailCustom";
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetailCustom).Size = new System.Drawing.Size(182, 25);
		((System.Windows.Forms.Control)(object)this.CmbFinInstrumentsPTDetailCustom).TabIndex = 1;
		this.CmbFinInstrumentsPTDetailCustom.ValueChanged += new System.EventHandler(CmbFinInstrumentsPTDetailCustom_ValueChanged);
		this.label97.AutoSize = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label97.ForeColor = System.Drawing.Color.Red;
		this.label97.Location = new System.Drawing.Point(101, 9);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(14, 17);
		this.label97.TabIndex = 589;
		this.label97.Text = "*";
		this.label98.AutoSize = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.Location = new System.Drawing.Point(521, 10);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(60, 15);
		this.label98.TabIndex = 509;
		this.label98.Text = "% of Total";
		this.label99.AutoSize = true;
		this.label99.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label99.ForeColor = System.Drawing.Color.Red;
		this.label99.Location = new System.Drawing.Point(333, 9);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(14, 17);
		this.label99.TabIndex = 588;
		this.label99.Text = "*";
		this.CmbPaymentTermPTDetailCustom.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermPTDetailCustom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermPTDetailCustom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermPTDetailCustom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance404).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance404).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance404;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance405).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance405).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance405).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance405).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance405;
		((AppearanceBase)appearance406).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance406;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance407).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance407).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance407).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance407).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance407;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance409).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance409).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance409;
		((AppearanceBase)appearance410).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance410).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance410;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance411).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance411;
		((AppearanceBase)appearance412).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance412).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance412;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance413).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance413).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance413).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance413).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance413).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance413;
		((AppearanceBase)appearance414).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance414;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance415).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance415).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance415;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance416).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance416;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermPTDetailCustom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetailCustom).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetailCustom).Location = new System.Drawing.Point(4, 29);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetailCustom).Name = "CmbPaymentTermPTDetailCustom";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetailCustom).Size = new System.Drawing.Size(182, 25);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetailCustom).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPTDetailCustom).TextChanged += new System.EventHandler(CmbPaymentTermDetailCustom_TextChanged);
		this.label100.AutoSize = true;
		this.label100.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label100.Location = new System.Drawing.Point(4, 10);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(84, 15);
		this.label100.TabIndex = 511;
		this.label100.Text = "Payment Term";
		this.txtPrcntOfTotalPTDetailCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPrcntOfTotalPTDetailCustom.Location = new System.Drawing.Point(521, 30);
		this.txtPrcntOfTotalPTDetailCustom.Name = "txtPrcntOfTotalPTDetailCustom";
		this.txtPrcntOfTotalPTDetailCustom.Size = new System.Drawing.Size(82, 22);
		this.txtPrcntOfTotalPTDetailCustom.TabIndex = 3;
		this.txtPrcntOfTotalPTDetailCustom.Text = "0";
		this.txtPrcntOfTotalPTDetailCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPrcntOfTotalPTDetailCustom.TextChanged += new System.EventHandler(txtPrcntOfTotalPTDetailCustom_TextChanged);
		this.txtPrcntOfTotalPTDetailCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPrcntOfTotalPTDetailCustom_KeyPress);
		this.BtnCancelPTDetailCustom.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelPTDetailCustom.FlatAppearance.BorderSize = 0;
		this.BtnCancelPTDetailCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelPTDetailCustom.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelPTDetailCustom.ForeColor = System.Drawing.Color.White;
		this.BtnCancelPTDetailCustom.Location = new System.Drawing.Point(1075, 4);
		this.BtnCancelPTDetailCustom.Name = "BtnCancelPTDetailCustom";
		this.BtnCancelPTDetailCustom.Size = new System.Drawing.Size(63, 23);
		this.BtnCancelPTDetailCustom.TabIndex = 9;
		this.BtnCancelPTDetailCustom.Text = "Cancel";
		this.BtnCancelPTDetailCustom.UseVisualStyleBackColor = false;
		this.BtnCancelPTDetailCustom.Visible = false;
		this.BtnCancelPTDetailCustom.Click += new System.EventHandler(BtnCancelPTDetailCustom_Click);
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label101.Location = new System.Drawing.Point(370, 10);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(48, 15);
		this.label101.TabIndex = 509;
		this.label101.Text = "Balance";
		this.BtnUpdatePTDetailCustom.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdatePTDetailCustom.FlatAppearance.BorderSize = 0;
		this.BtnUpdatePTDetailCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdatePTDetailCustom.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdatePTDetailCustom.ForeColor = System.Drawing.Color.White;
		this.BtnUpdatePTDetailCustom.Location = new System.Drawing.Point(1075, 30);
		this.BtnUpdatePTDetailCustom.Name = "BtnUpdatePTDetailCustom";
		this.BtnUpdatePTDetailCustom.Size = new System.Drawing.Size(63, 23);
		this.BtnUpdatePTDetailCustom.TabIndex = 8;
		this.BtnUpdatePTDetailCustom.Text = "Update";
		this.BtnUpdatePTDetailCustom.UseVisualStyleBackColor = false;
		this.BtnUpdatePTDetailCustom.Visible = false;
		this.BtnUpdatePTDetailCustom.Click += new System.EventHandler(BtnUpdatePTDetailCustom_Click);
		this.txtBalanceFIPTDetailCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceFIPTDetailCustom.Location = new System.Drawing.Point(370, 30);
		this.txtBalanceFIPTDetailCustom.Name = "txtBalanceFIPTDetailCustom";
		this.txtBalanceFIPTDetailCustom.ReadOnly = true;
		this.txtBalanceFIPTDetailCustom.Size = new System.Drawing.Size(150, 22);
		this.txtBalanceFIPTDetailCustom.TabIndex = 2;
		this.txtBalanceFIPTDetailCustom.TabStop = false;
		this.txtBalanceFIPTDetailCustom.Text = "0";
		this.txtBalanceFIPTDetailCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDueDaysPTDetailCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDaysPTDetailCustom.Location = new System.Drawing.Point(755, 30);
		this.txtDueDaysPTDetailCustom.Name = "txtDueDaysPTDetailCustom";
		this.txtDueDaysPTDetailCustom.Size = new System.Drawing.Size(56, 22);
		this.txtDueDaysPTDetailCustom.TabIndex = 5;
		this.txtDueDaysPTDetailCustom.Text = "0";
		this.txtDueDaysPTDetailCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDueDaysPTDetailCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDaysPTDetailCustom_KeyPress);
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.Location = new System.Drawing.Point(753, 10);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(58, 15);
		this.label102.TabIndex = 515;
		this.label102.Text = "Due Days";
		this.label103.AutoSize = true;
		this.label103.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label103.Location = new System.Drawing.Point(603, 10);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(72, 15);
		this.label103.TabIndex = 513;
		this.label103.Text = "Fcy Amount";
		this.txtFcyAmountPTDetailCustom.Enabled = false;
		this.txtFcyAmountPTDetailCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountPTDetailCustom.Location = new System.Drawing.Point(604, 30);
		this.txtFcyAmountPTDetailCustom.Name = "txtFcyAmountPTDetailCustom";
		this.txtFcyAmountPTDetailCustom.Size = new System.Drawing.Size(150, 22);
		this.txtFcyAmountPTDetailCustom.TabIndex = 4;
		this.txtFcyAmountPTDetailCustom.Text = "0";
		this.txtFcyAmountPTDetailCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountPTDetailCustom.TextChanged += new System.EventHandler(txtFcyAmountPTDetailCustom_TextChanged);
		this.txtFcyAmountPTDetailCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyAmountPTDetailCustom_KeyPress);
		this.tabChargesDetailCustom.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesDetailCustom);
		this.tabChargesDetailCustom.Controls.Add(this.panel28);
		this.tabChargesDetailCustom.Controls.Add(this.panel29);
		this.tabChargesDetailCustom.Location = new System.Drawing.Point(4, 25);
		this.tabChargesDetailCustom.Name = "tabChargesDetailCustom";
		this.tabChargesDetailCustom.Size = new System.Drawing.Size(1148, 176);
		this.tabChargesDetailCustom.TabIndex = 3;
		this.tabChargesDetailCustom.Text = "Other Charges Detail";
		this.tabChargesDetailCustom.UseVisualStyleBackColor = true;
		this.grdOtherChargesDetailCustom.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOtherChargesDetailCustom).BackColor = System.Drawing.Color.White;
		grdOtherChargesDetailCustom_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherChargesDetailCustom.DesignTimeLayout = grdOtherChargesDetailCustom_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetailCustom).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetailCustom).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherChargesDetailCustom.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetailCustom).Location = new System.Drawing.Point(0, 77);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetailCustom).Name = "grdOtherChargesDetailCustom";
		((ScrollableBase)this.grdOtherChargesDetailCustom).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdOtherChargesDetailCustom).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdOtherChargesDetailCustom).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetailCustom).Size = new System.Drawing.Size(1148, 99);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetailCustom).TabIndex = 1;
		this.grdOtherChargesDetailCustom.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesDetailCustom.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesDetailCustom).VisualStyle = (VisualStyle)4;
		this.grdOtherChargesDetailCustom.ColumnButtonClick += new ColumnActionEventHandler(grdOtherChargesDetailCustom_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetailCustom).DoubleClick += new System.EventHandler(grdOtherChargesDetailCustom_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetailCustom).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOtherChargesDetailCustom_KeyDown);
		this.panel28.BackColor = System.Drawing.Color.Teal;
		this.panel28.Controls.Add(this.ctrlgrdOtherChargesDetailCustom);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel28.Location = new System.Drawing.Point(0, 54);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1148, 23);
		this.panel28.TabIndex = 2;
		this.ctrlgrdOtherChargesDetailCustom.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdOtherChargesDetailCustom.FormName = this;
		this.ctrlgrdOtherChargesDetailCustom.Location = new System.Drawing.Point(1106, 0);
		this.ctrlgrdOtherChargesDetailCustom.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdOtherChargesDetailCustom.MyGrid = this.grdOtherChargesDetailCustom;
		this.ctrlgrdOtherChargesDetailCustom.Name = "ctrlgrdOtherChargesDetailCustom";
		this.ctrlgrdOtherChargesDetailCustom.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdOtherChargesDetailCustom.TabIndex = 27;
		this.ctrlgrdOtherChargesDetailCustom.Load += new System.EventHandler(ctrlgrdOtherChargesDetailCustom_Load);
		this.panel29.Controls.Add(this.BtnAddChargeDetailCustom);
		this.panel29.Controls.Add(this.label37);
		this.panel29.Controls.Add(this.txtRemarksChargeDetailCustom);
		this.panel29.Controls.Add(this.label38);
		this.panel29.Controls.Add(this.label39);
		this.panel29.Controls.Add(this.label51);
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetailCustom);
		this.panel29.Controls.Add(this.label52);
		this.panel29.Controls.Add(this.BtnCancelChargeDetailCustom);
		this.panel29.Controls.Add(this.BtnUpdateChargeDetailCustom);
		this.panel29.Controls.Add(this.label65);
		this.panel29.Controls.Add(this.label72);
		this.panel29.Controls.Add(this.txtLessAmountChargeDetailCustom);
		this.panel29.Controls.Add(this.txtAddAmountChargeDetailCustom);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(0, 0);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1148, 54);
		this.panel29.TabIndex = 0;
		this.BtnAddChargeDetailCustom.BackColor = System.Drawing.Color.Teal;
		this.BtnAddChargeDetailCustom.FlatAppearance.BorderSize = 0;
		this.BtnAddChargeDetailCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddChargeDetailCustom.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddChargeDetailCustom.ForeColor = System.Drawing.Color.White;
		this.BtnAddChargeDetailCustom.Location = new System.Drawing.Point(1082, 26);
		this.BtnAddChargeDetailCustom.Name = "BtnAddChargeDetailCustom";
		this.BtnAddChargeDetailCustom.Size = new System.Drawing.Size(31, 23);
		this.BtnAddChargeDetailCustom.TabIndex = 4;
		this.BtnAddChargeDetailCustom.Text = "+";
		this.BtnAddChargeDetailCustom.UseVisualStyleBackColor = false;
		this.BtnAddChargeDetailCustom.Click += new System.EventHandler(BtnAddChargeDetailCustom_Click);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(435, 8);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(52, 15);
		this.label37.TabIndex = 593;
		this.label37.Text = "Remarks";
		this.txtRemarksChargeDetailCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksChargeDetailCustom.Location = new System.Drawing.Point(435, 26);
		this.txtRemarksChargeDetailCustom.Name = "txtRemarksChargeDetailCustom";
		this.txtRemarksChargeDetailCustom.Size = new System.Drawing.Size(645, 22);
		this.txtRemarksChargeDetailCustom.TabIndex = 3;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(399, 7);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(14, 17);
		this.label38.TabIndex = 591;
		this.label38.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(289, 7);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(14, 17);
		this.label39.TabIndex = 591;
		this.label39.Text = "*";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(3, 8);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(70, 15);
		this.label51.TabIndex = 508;
		this.label51.Text = "Charges List";
		this.CmbChargesNameChargeDetailCustom.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesNameChargeDetailCustom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesNameChargeDetailCustom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesNameChargeDetailCustom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance417).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance417).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance417;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance418).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance418).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance418).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance418).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance418;
		((AppearanceBase)appearance420).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance420;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance421).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance421).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance421).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance421).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance421;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance422).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance422).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance422;
		((AppearanceBase)appearance423).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance423).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance423;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance424).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance424;
		((AppearanceBase)appearance425).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance425).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance425;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance426).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance426).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance426).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance426).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance426).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance426;
		((AppearanceBase)appearance427).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance427;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance428).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance428).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance428;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance429).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance429;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesNameChargeDetailCustom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetailCustom).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetailCustom).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetailCustom).Name = "CmbChargesNameChargeDetailCustom";
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetailCustom).Size = new System.Drawing.Size(211, 25);
		((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetailCustom).TabIndex = 0;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(72, 8);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 17);
		this.label52.TabIndex = 588;
		this.label52.Text = "*";
		this.BtnCancelChargeDetailCustom.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelChargeDetailCustom.FlatAppearance.BorderSize = 0;
		this.BtnCancelChargeDetailCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelChargeDetailCustom.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelChargeDetailCustom.ForeColor = System.Drawing.Color.White;
		this.BtnCancelChargeDetailCustom.Location = new System.Drawing.Point(1082, 2);
		this.BtnCancelChargeDetailCustom.Name = "BtnCancelChargeDetailCustom";
		this.BtnCancelChargeDetailCustom.Size = new System.Drawing.Size(63, 23);
		this.BtnCancelChargeDetailCustom.TabIndex = 6;
		this.BtnCancelChargeDetailCustom.Text = "Cancel";
		this.BtnCancelChargeDetailCustom.UseVisualStyleBackColor = false;
		this.BtnCancelChargeDetailCustom.Visible = false;
		this.BtnCancelChargeDetailCustom.Click += new System.EventHandler(BtnCancelChargeDetailCustom_Click);
		this.BtnUpdateChargeDetailCustom.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateChargeDetailCustom.FlatAppearance.BorderSize = 0;
		this.BtnUpdateChargeDetailCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateChargeDetailCustom.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateChargeDetailCustom.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateChargeDetailCustom.Location = new System.Drawing.Point(1082, 26);
		this.BtnUpdateChargeDetailCustom.Name = "BtnUpdateChargeDetailCustom";
		this.BtnUpdateChargeDetailCustom.Size = new System.Drawing.Size(63, 23);
		this.BtnUpdateChargeDetailCustom.TabIndex = 5;
		this.BtnUpdateChargeDetailCustom.Text = "Update";
		this.BtnUpdateChargeDetailCustom.UseVisualStyleBackColor = false;
		this.BtnUpdateChargeDetailCustom.Visible = false;
		this.BtnUpdateChargeDetailCustom.Click += new System.EventHandler(BtnUpdateChargeDetailCustom_Click);
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(324, 8);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(76, 15);
		this.label65.TabIndex = 513;
		this.label65.Text = "Less Amount";
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(214, 8);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(76, 15);
		this.label72.TabIndex = 513;
		this.label72.Text = "Add Amount";
		this.txtLessAmountChargeDetailCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLessAmountChargeDetailCustom.Location = new System.Drawing.Point(325, 26);
		this.txtLessAmountChargeDetailCustom.Name = "txtLessAmountChargeDetailCustom";
		this.txtLessAmountChargeDetailCustom.Size = new System.Drawing.Size(109, 22);
		this.txtLessAmountChargeDetailCustom.TabIndex = 2;
		this.txtLessAmountChargeDetailCustom.Text = "0";
		this.txtLessAmountChargeDetailCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLessAmountChargeDetailCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLessAmountChargeDetailCustom_KeyPress);
		this.txtAddAmountChargeDetailCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddAmountChargeDetailCustom.Location = new System.Drawing.Point(215, 26);
		this.txtAddAmountChargeDetailCustom.Name = "txtAddAmountChargeDetailCustom";
		this.txtAddAmountChargeDetailCustom.Size = new System.Drawing.Size(109, 22);
		this.txtAddAmountChargeDetailCustom.TabIndex = 1;
		this.txtAddAmountChargeDetailCustom.Text = "0";
		this.txtAddAmountChargeDetailCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddAmountChargeDetailCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddAmountChargeDetailCustom_KeyPress);
		this.panel11.Controls.Add(this.HeaderGropBox);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 57);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1170, 380);
		this.panel11.TabIndex = 3;
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.HeaderGropBox.Controls.Add(this.groupBox3);
		this.HeaderGropBox.Controls.Add(this.btnDefineBankLocal);
		this.HeaderGropBox.Controls.Add(this.btnDefineBankForeign);
		this.HeaderGropBox.Controls.Add(this.txtaddlesscommnets);
		this.HeaderGropBox.Controls.Add(this.txtLocalAmt);
		this.HeaderGropBox.Controls.Add(this.txtNoOfContainer);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyCode);
		this.HeaderGropBox.Controls.Add(this.txtGrossWeight);
		this.HeaderGropBox.Controls.Add(this.txtaddlessamount);
		this.HeaderGropBox.Controls.Add(this.txtNetWeight);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlaceOfDelivery);
		this.HeaderGropBox.Controls.Add(this.txtTotalNetAmount);
		this.HeaderGropBox.Controls.Add(this.txtFcyAmount);
		this.HeaderGropBox.Controls.Add(this.txtExRate);
		this.HeaderGropBox.Controls.Add(this.label43);
		this.HeaderGropBox.Controls.Add(this.label1);
		this.HeaderGropBox.Controls.Add(this.label45);
		this.HeaderGropBox.Controls.Add(this.label47);
		this.HeaderGropBox.Controls.Add(this.label13);
		this.HeaderGropBox.Controls.Add(this.label60);
		this.HeaderGropBox.Controls.Add(this.label50);
		this.HeaderGropBox.Controls.Add(this.label49);
		this.HeaderGropBox.Controls.Add(this.label53);
		this.HeaderGropBox.Controls.Add(this.label9);
		this.HeaderGropBox.Controls.Add(this.label42);
		this.HeaderGropBox.Controls.Add(this.label27);
		this.HeaderGropBox.Controls.Add(this.label66);
		this.HeaderGropBox.Controls.Add(this.label64);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbImporterCountry);
		this.HeaderGropBox.Controls.Add(this.label44);
		this.HeaderGropBox.Controls.Add(this.label25);
		this.HeaderGropBox.Controls.Add(this.label6);
		this.HeaderGropBox.Controls.Add(this.label40);
		this.HeaderGropBox.Controls.Add(this.txtLotRefHeader);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContinent);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCareierType);
		this.HeaderGropBox.Controls.Add(this.txtGdDate);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDestinationPort);
		this.HeaderGropBox.Controls.Add(this.label116);
		this.HeaderGropBox.Controls.Add(this.txtdocno);
		this.HeaderGropBox.Controls.Add(this.txtCustomerContractsNos);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbNotifyParty3);
		this.HeaderGropBox.Controls.Add(this.label108);
		this.HeaderGropBox.Controls.Add(this.txtCustomerContractsDates);
		this.HeaderGropBox.Controls.Add(this.txtDeliveryRemarks);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbConsignee);
		this.HeaderGropBox.Controls.Add(this.label87);
		this.HeaderGropBox.Controls.Add(this.label120);
		this.HeaderGropBox.Controls.Add(this.label114);
		this.HeaderGropBox.Controls.Add(this.label86);
		this.HeaderGropBox.Controls.Add(this.txtTcpRegNo);
		this.HeaderGropBox.Controls.Add(this.txtDocumentaryCreditNo);
		this.HeaderGropBox.Controls.Add(this.label31);
		this.HeaderGropBox.Controls.Add(this.label17);
		this.HeaderGropBox.Controls.Add(this.label113);
		this.HeaderGropBox.Controls.Add(this.label7);
		this.HeaderGropBox.Controls.Add(this.label107);
		this.HeaderGropBox.Controls.Add(this.txtRemarks2);
		this.HeaderGropBox.Controls.Add(this.txtRemarks1);
		this.HeaderGropBox.Controls.Add(this.label118);
		this.HeaderGropBox.Controls.Add(this.txtCertificate1);
		this.HeaderGropBox.Controls.Add(this.label117);
		this.HeaderGropBox.Controls.Add(this.label61);
		this.HeaderGropBox.Controls.Add(this.label57);
		this.HeaderGropBox.Controls.Add(this.label62);
		this.HeaderGropBox.Controls.Add(this.label55);
		this.HeaderGropBox.Controls.Add(this.label2);
		this.HeaderGropBox.Controls.Add(this.label54);
		this.HeaderGropBox.Controls.Add(this.txtCertificateOfOrigion);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbImporterBank);
		this.HeaderGropBox.Controls.Add(this.label10);
		this.HeaderGropBox.Controls.Add(this.label67);
		this.HeaderGropBox.Controls.Add(this.label95);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOriginCountry);
		this.HeaderGropBox.Controls.Add(this.label12);
		this.HeaderGropBox.Controls.Add(this.label109);
		this.HeaderGropBox.Controls.Add(this.txtGdNo);
		this.HeaderGropBox.Controls.Add(this.label129);
		this.HeaderGropBox.Controls.Add(this.label115);
		this.HeaderGropBox.Controls.Add(this.label3);
		this.HeaderGropBox.Controls.Add(this.label110);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExporterBank);
		this.HeaderGropBox.Controls.Add(this.label68);
		this.HeaderGropBox.Controls.Add(this.txtDocumentaryCreditNoDate);
		this.HeaderGropBox.Controls.Add(this.label16);
		this.HeaderGropBox.Controls.Add(this.label24);
		this.HeaderGropBox.Controls.Add(this.label111);
		this.HeaderGropBox.Controls.Add(this.txtDocDate);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingPort);
		this.HeaderGropBox.Controls.Add(this.label5);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.HeaderGropBox.Controls.Add(this.label41);
		this.HeaderGropBox.Controls.Add(this.label22);
		this.HeaderGropBox.Controls.Add(this.label21);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbNotifyParty2);
		this.HeaderGropBox.Controls.Add(this.label28);
		this.HeaderGropBox.Controls.Add(this.label26);
		this.HeaderGropBox.Controls.Add(this.label4);
		this.HeaderGropBox.Controls.Add(this.label8);
		this.HeaderGropBox.Controls.Add(this.label46);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbNotifyParty1);
		this.HeaderGropBox.Controls.Add(this.label56);
		this.HeaderGropBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.HeaderGropBox.Location = new System.Drawing.Point(4, 2);
		this.HeaderGropBox.Name = "HeaderGropBox";
		this.HeaderGropBox.Size = new System.Drawing.Size(1163, 377);
		this.HeaderGropBox.TabIndex = 0;
		this.HeaderGropBox.TabStop = false;
		this.HeaderGropBox.Text = "Main Header";
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance361).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance361).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance361;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance362).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance362).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance362).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance362;
		((AppearanceBase)appearance363).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance363;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance364).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance364).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance364).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance364).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance364;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance365).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance365).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance365;
		((AppearanceBase)appearance366).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance366).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance366;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance367;
		((AppearanceBase)appearance368).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance368).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance368;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance369).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance369).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance369).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance369).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance369).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance369;
		((AppearanceBase)appearance371).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance371;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance372).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance372).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance372;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance373;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(115, 37);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 2;
		this.groupBox3.Controls.Add(this.txtFcyAmountHeaderCustom);
		this.groupBox3.Controls.Add(this.txtAddLessHeaderCustom);
		this.groupBox3.Controls.Add(this.txtLocalAmountHeaderCustom);
		this.groupBox3.Controls.Add(this.txtTotalFcyAmountHeaderCustom);
		this.groupBox3.Controls.Add(this.label112);
		this.groupBox3.Controls.Add(this.label119);
		this.groupBox3.Controls.Add(this.label121);
		this.groupBox3.Controls.Add(this.label122);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(881, 247);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(280, 125);
		this.groupBox3.TabIndex = 601;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Custom Values";
		this.txtFcyAmountHeaderCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountHeaderCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountHeaderCustom.Location = new System.Drawing.Point(106, 15);
		this.txtFcyAmountHeaderCustom.Name = "txtFcyAmountHeaderCustom";
		this.txtFcyAmountHeaderCustom.ReadOnly = true;
		this.txtFcyAmountHeaderCustom.Size = new System.Drawing.Size(172, 22);
		this.txtFcyAmountHeaderCustom.TabIndex = 29;
		this.txtFcyAmountHeaderCustom.Text = "0";
		this.txtFcyAmountHeaderCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountHeaderCustom.TextChanged += new System.EventHandler(txtAddLessHeaderCustom_TextChanged);
		this.txtAddLessHeaderCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessHeaderCustom.Enabled = false;
		this.txtAddLessHeaderCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLessHeaderCustom.Location = new System.Drawing.Point(106, 41);
		this.txtAddLessHeaderCustom.Name = "txtAddLessHeaderCustom";
		this.txtAddLessHeaderCustom.Size = new System.Drawing.Size(172, 22);
		this.txtAddLessHeaderCustom.TabIndex = 30;
		this.txtAddLessHeaderCustom.Text = "0";
		this.txtAddLessHeaderCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessHeaderCustom.TextChanged += new System.EventHandler(txtAddLessHeaderCustom_TextChanged);
		this.txtLocalAmountHeaderCustom.BackColor = System.Drawing.Color.White;
		this.txtLocalAmountHeaderCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmountHeaderCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLocalAmountHeaderCustom.Location = new System.Drawing.Point(106, 93);
		this.txtLocalAmountHeaderCustom.Name = "txtLocalAmountHeaderCustom";
		this.txtLocalAmountHeaderCustom.ReadOnly = true;
		this.txtLocalAmountHeaderCustom.Size = new System.Drawing.Size(172, 22);
		this.txtLocalAmountHeaderCustom.TabIndex = 33;
		this.txtLocalAmountHeaderCustom.TabStop = false;
		this.txtLocalAmountHeaderCustom.Text = "0";
		this.txtLocalAmountHeaderCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalFcyAmountHeaderCustom.BackColor = System.Drawing.Color.White;
		this.txtTotalFcyAmountHeaderCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFcyAmountHeaderCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFcyAmountHeaderCustom.Location = new System.Drawing.Point(106, 67);
		this.txtTotalFcyAmountHeaderCustom.Name = "txtTotalFcyAmountHeaderCustom";
		this.txtTotalFcyAmountHeaderCustom.ReadOnly = true;
		this.txtTotalFcyAmountHeaderCustom.Size = new System.Drawing.Size(172, 22);
		this.txtTotalFcyAmountHeaderCustom.TabIndex = 32;
		this.txtTotalFcyAmountHeaderCustom.TabStop = false;
		this.txtTotalFcyAmountHeaderCustom.Text = "0";
		this.txtTotalFcyAmountHeaderCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label112.Location = new System.Drawing.Point(4, 19);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(74, 15);
		this.label112.TabIndex = 538;
		this.label112.Text = "Fcy-Amount";
		this.label119.AutoSize = true;
		this.label119.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label119.Location = new System.Drawing.Point(4, 45);
		this.label119.Name = "label119";
		this.label119.Size = new System.Drawing.Size(80, 15);
		this.label119.TabIndex = 582;
		this.label119.Text = "Add Less Amt";
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label121.Location = new System.Drawing.Point(4, 71);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(101, 15);
		this.label121.TabIndex = 585;
		this.label121.Text = "Total Fcy Amount";
		this.label122.AutoSize = true;
		this.label122.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label122.Location = new System.Drawing.Point(4, 97);
		this.label122.Name = "label122";
		this.label122.Size = new System.Drawing.Size(82, 15);
		this.label122.TabIndex = 549;
		this.label122.Text = "Local Amount";
		this.btnDefineBankLocal.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineBankLocal.Location = new System.Drawing.Point(324, 296);
		this.btnDefineBankLocal.Name = "btnDefineBankLocal";
		this.btnDefineBankLocal.Size = new System.Drawing.Size(26, 26);
		this.btnDefineBankLocal.TabIndex = 600;
		this.btnDefineBankLocal.TabStop = false;
		this.btnDefineBankLocal.UseVisualStyleBackColor = true;
		this.btnDefineBankLocal.Click += new System.EventHandler(btnDefineBankLocal_Click);
		this.btnDefineBankForeign.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineBankForeign.Location = new System.Drawing.Point(324, 270);
		this.btnDefineBankForeign.Name = "btnDefineBankForeign";
		this.btnDefineBankForeign.Size = new System.Drawing.Size(26, 26);
		this.btnDefineBankForeign.TabIndex = 599;
		this.btnDefineBankForeign.TabStop = false;
		this.btnDefineBankForeign.UseVisualStyleBackColor = true;
		this.btnDefineBankForeign.Click += new System.EventHandler(btnDefineBankForeign_Click);
		this.txtaddlesscommnets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlesscommnets.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddlesscommnets.Location = new System.Drawing.Point(734, 142);
		this.txtaddlesscommnets.Multiline = true;
		this.txtaddlesscommnets.Name = "txtaddlesscommnets";
		this.txtaddlesscommnets.Size = new System.Drawing.Size(145, 50);
		this.txtaddlesscommnets.TabIndex = 31;
		this.txtLocalAmt.BackColor = System.Drawing.Color.White;
		this.txtLocalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmt.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLocalAmt.Location = new System.Drawing.Point(734, 221);
		this.txtLocalAmt.Name = "txtLocalAmt";
		this.txtLocalAmt.ReadOnly = true;
		this.txtLocalAmt.Size = new System.Drawing.Size(145, 22);
		this.txtLocalAmt.TabIndex = 33;
		this.txtLocalAmt.TabStop = false;
		this.txtLocalAmt.Text = "0";
		this.txtLocalAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainer.Location = new System.Drawing.Point(734, 247);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(145, 22);
		this.txtNoOfContainer.TabIndex = 34;
		this.txtNoOfContainer.Text = "0";
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbFcyCode.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFcyCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Location = new System.Drawing.Point(734, 38);
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Name = "CmbFcyCode";
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Size = new System.Drawing.Size(145, 25);
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).TabIndex = 27;
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Enabled = false;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(734, 274);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(145, 22);
		this.txtGrossWeight.TabIndex = 35;
		this.txtGrossWeight.Text = "0";
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtaddlessamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlessamount.Enabled = false;
		this.txtaddlessamount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddlessamount.Location = new System.Drawing.Point(734, 117);
		this.txtaddlessamount.Name = "txtaddlessamount";
		this.txtaddlessamount.Size = new System.Drawing.Size(145, 22);
		this.txtaddlessamount.TabIndex = 30;
		this.txtaddlessamount.Text = "0";
		this.txtaddlessamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtaddlessamount.TextChanged += new System.EventHandler(txtaddlessamount_TextChanged);
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(734, 300);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(145, 22);
		this.txtNetWeight.TabIndex = 36;
		this.txtNetWeight.Text = "0";
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbPlaceOfDelivery.AllowNull = (DefaultableBoolean)1;
		this.CmbPlaceOfDelivery.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlaceOfDelivery.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlaceOfDelivery.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance316).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Appearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance317).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance317).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance317;
		((AppearanceBase)appearance318).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance318;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance319).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance319).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance319).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance320).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance320;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance321).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance322;
		((AppearanceBase)appearance323).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance323).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance323;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance324).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance324).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance324).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance324).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance324;
		((AppearanceBase)appearance325).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance325;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance327).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlaceOfDelivery).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlaceOfDelivery).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlaceOfDelivery.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlaceOfDelivery).Location = new System.Drawing.Point(734, 12);
		((System.Windows.Forms.Control)(object)this.CmbPlaceOfDelivery).Name = "CmbPlaceOfDelivery";
		((System.Windows.Forms.Control)(object)this.CmbPlaceOfDelivery).Size = new System.Drawing.Size(145, 25);
		((System.Windows.Forms.Control)(object)this.CmbPlaceOfDelivery).TabIndex = 26;
		this.txtTotalNetAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalNetAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalNetAmount.Location = new System.Drawing.Point(734, 195);
		this.txtTotalNetAmount.Name = "txtTotalNetAmount";
		this.txtTotalNetAmount.ReadOnly = true;
		this.txtTotalNetAmount.Size = new System.Drawing.Size(145, 22);
		this.txtTotalNetAmount.TabIndex = 32;
		this.txtTotalNetAmount.TabStop = false;
		this.txtTotalNetAmount.Text = "0";
		this.txtTotalNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(734, 91);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ReadOnly = true;
		this.txtFcyAmount.Size = new System.Drawing.Size(145, 22);
		this.txtFcyAmount.TabIndex = 29;
		this.txtFcyAmount.Text = "0";
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.TextChanged += new System.EventHandler(txtfcyAmount_TextChanged);
		this.txtExRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExRate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExRate.Location = new System.Drawing.Point(734, 66);
		this.txtExRate.Name = "txtExRate";
		this.txtExRate.Size = new System.Drawing.Size(145, 22);
		this.txtExRate.TabIndex = 28;
		this.txtExRate.Text = "0";
		this.txtExRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExRate.TextChanged += new System.EventHandler(txtExRate_TextChanged);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(724, 224);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 42;
		this.label43.Text = "*";
		this.label43.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(724, 94);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 43;
		this.label1.Text = "*";
		this.label1.Visible = false;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(724, 198);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 550;
		this.label45.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(724, 69);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 543;
		this.label47.Text = "*";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(724, 16);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 17);
		this.label13.TabIndex = 598;
		this.label13.Text = "*";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(724, 42);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 573;
		this.label60.Text = "*";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(724, 277);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 569;
		this.label50.Text = "*";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(724, 303);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 560;
		this.label49.Text = "*";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(724, 250);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 570;
		this.label53.Text = "*";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(427, 170);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(14, 17);
		this.label9.TabIndex = 528;
		this.label9.Text = "*";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(627, 225);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(82, 15);
		this.label42.TabIndex = 549;
		this.label42.Text = "Local Amount";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(627, 43);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(55, 15);
		this.label27.TabIndex = 537;
		this.label27.Text = "Fcy Code";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(627, 199);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(101, 15);
		this.label66.TabIndex = 585;
		this.label66.Text = "Total Fcy Amount";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(627, 121);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(80, 15);
		this.label64.TabIndex = 582;
		this.label64.Text = "Add Less Amt";
		this.CmbImporterCountry.AllowNull = (DefaultableBoolean)1;
		this.CmbImporterCountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbImporterCountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbImporterCountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance387).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance387).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance387;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance388).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance388).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance388).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterCountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance388;
		((AppearanceBase)appearance390).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance390;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterCountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance391).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance391).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance391).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance391).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance391;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance392).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance392).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance392;
		((AppearanceBase)appearance393).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance393).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance393;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance394;
		((AppearanceBase)appearance395).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance395).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance395;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance396).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance396).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance396).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance396).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance396).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance396;
		((AppearanceBase)appearance397).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance397;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance398).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance398).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance398;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance399).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance399;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbImporterCountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbImporterCountry).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbImporterCountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbImporterCountry).Location = new System.Drawing.Point(444, 297);
		((System.Windows.Forms.Control)(object)this.CmbImporterCountry).Name = "CmbImporterCountry";
		((System.Windows.Forms.Control)(object)this.CmbImporterCountry).Size = new System.Drawing.Size(179, 25);
		((System.Windows.Forms.Control)(object)this.CmbImporterCountry).TabIndex = 25;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(627, 70);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(84, 15);
		this.label44.TabIndex = 546;
		this.label44.Text = "Exchange Rate";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(627, 95);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(74, 15);
		this.label25.TabIndex = 538;
		this.label25.Text = "Fcy-Amount";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(427, 306);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 17);
		this.label6.TabIndex = 545;
		this.label6.Text = "*";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(627, 147);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(99, 15);
		this.label40.TabIndex = 580;
		this.label40.Text = "Ad/Ls Comments";
		this.txtLotRefHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLotRefHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLotRefHeader.Location = new System.Drawing.Point(444, 13);
		this.txtLotRefHeader.Name = "txtLotRefHeader";
		this.txtLotRefHeader.Size = new System.Drawing.Size(179, 22);
		this.txtLotRefHeader.TabIndex = 14;
		this.CmbContinent.AllowNull = (DefaultableBoolean)1;
		this.CmbContinent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContinent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContinent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbContinent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContinent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContinent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContinent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance138;
		((SpecialBoxBase)((UltraGridBase)this.CmbContinent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContinent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbContinent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContinent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContinent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbContinent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContinent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContinent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContinent).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContinent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContinent).Location = new System.Drawing.Point(444, 245);
		((System.Windows.Forms.Control)(object)this.CmbContinent).Name = "CmbContinent";
		((System.Windows.Forms.Control)(object)this.CmbContinent).Size = new System.Drawing.Size(179, 25);
		((System.Windows.Forms.Control)(object)this.CmbContinent).TabIndex = 23;
		this.CmbCareierType.AllowNull = (DefaultableBoolean)1;
		this.CmbCareierType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCareierType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCareierType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance342).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance342;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance343).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance343).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance343).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance343).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCareierType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance343;
		((AppearanceBase)appearance344).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance344;
		((SpecialBoxBase)((UltraGridBase)this.CmbCareierType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance345).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance345).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance345).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance345).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance346).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance346;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance347).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance347;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance349;
		((AppearanceBase)appearance350).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance350).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance350;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance351).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance351).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance351).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance351).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance351).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance351;
		((AppearanceBase)appearance352).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance352;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance353).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance353).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance353;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance354;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCareierType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCareierType).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCareierType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCareierType).Location = new System.Drawing.Point(444, 140);
		((System.Windows.Forms.Control)(object)this.CmbCareierType).Name = "CmbCareierType";
		((System.Windows.Forms.Control)(object)this.CmbCareierType).Size = new System.Drawing.Size(179, 25);
		((System.Windows.Forms.Control)(object)this.CmbCareierType).TabIndex = 19;
		this.txtGdDate.CustomFormat = "dd-MMM-yy";
		this.txtGdDate.Enabled = false;
		this.txtGdDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGdDate.Location = new System.Drawing.Point(444, 64);
		this.txtGdDate.Name = "txtGdDate";
		this.txtGdDate.Size = new System.Drawing.Size(179, 22);
		this.txtGdDate.TabIndex = 16;
		this.CmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.CmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance453).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance453).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance453;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance454).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance454).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance454).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance454).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance454;
		((AppearanceBase)appearance455).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance455;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance456).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance456).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance456).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance456).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance456;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance457).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance457).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance457;
		((AppearanceBase)appearance459).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance459).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance459;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance460).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance460;
		((AppearanceBase)appearance461).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance461).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance461;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance462).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance462).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance462).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance462).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance462).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance462;
		((AppearanceBase)appearance463).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance463;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance464).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance464).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance464;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance465).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance465;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Location = new System.Drawing.Point(444, 114);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Name = "CmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Size = new System.Drawing.Size(179, 25);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).TabIndex = 18;
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label116.ForeColor = System.Drawing.Color.Red;
		this.label116.Location = new System.Drawing.Point(427, 119);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(14, 17);
		this.label116.TabIndex = 545;
		this.label116.Text = "*";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(115, 13);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(125, 22);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtCustomerContractsNos.Enabled = false;
		this.txtCustomerContractsNos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerContractsNos.Location = new System.Drawing.Point(115, 141);
		this.txtCustomerContractsNos.Name = "txtCustomerContractsNos";
		this.txtCustomerContractsNos.Size = new System.Drawing.Size(235, 23);
		this.txtCustomerContractsNos.TabIndex = 7;
		this.CmbNotifyParty3.AllowNull = (DefaultableBoolean)1;
		this.CmbNotifyParty3.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbNotifyParty3.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbNotifyParty3.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbNotifyParty3).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty3).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbNotifyParty3.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty3).Location = new System.Drawing.Point(115, 244);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty3).Name = "CmbNotifyParty3";
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty3).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty3).TabIndex = 11;
		this.label108.AutoSize = true;
		this.label108.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label108.Location = new System.Drawing.Point(5, 250);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(80, 15);
		this.label108.TabIndex = 596;
		this.label108.Text = "Notify Party 3";
		this.txtCustomerContractsDates.Enabled = false;
		this.txtCustomerContractsDates.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerContractsDates.Location = new System.Drawing.Point(115, 167);
		this.txtCustomerContractsDates.Name = "txtCustomerContractsDates";
		this.txtCustomerContractsDates.Size = new System.Drawing.Size(235, 23);
		this.txtCustomerContractsDates.TabIndex = 8;
		this.txtDeliveryRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryRemarks.Location = new System.Drawing.Point(444, 193);
		this.txtDeliveryRemarks.Name = "txtDeliveryRemarks";
		this.txtDeliveryRemarks.Size = new System.Drawing.Size(179, 23);
		this.txtDeliveryRemarks.TabIndex = 21;
		this.CmbConsignee.AllowNull = (DefaultableBoolean)1;
		this.CmbConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance186;
		((SpecialBoxBase)((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance194).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbConsignee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Location = new System.Drawing.Point(115, 114);
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Name = "CmbConsignee";
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Size = new System.Drawing.Size(235, 25);
		((System.Windows.Forms.Control)(object)this.CmbConsignee).TabIndex = 6;
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.Location = new System.Drawing.Point(352, 197);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(52, 15);
		this.label87.TabIndex = 573;
		this.label87.Text = "Remarks";
		this.label120.AutoSize = true;
		this.label120.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label120.Location = new System.Drawing.Point(5, 171);
		this.label120.Name = "label120";
		this.label120.Size = new System.Drawing.Size(115, 15);
		this.label120.TabIndex = 579;
		this.label120.Text = "Sales Contract Dates";
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label114.Location = new System.Drawing.Point(5, 145);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(106, 15);
		this.label114.TabIndex = 579;
		this.label114.Text = "Sales Contract Nos";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label86.Location = new System.Drawing.Point(5, 119);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(97, 15);
		this.label86.TabIndex = 594;
		this.label86.Text = "Consignee Name";
		this.txtTcpRegNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTcpRegNo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTcpRegNo.Location = new System.Drawing.Point(444, 219);
		this.txtTcpRegNo.Name = "txtTcpRegNo";
		this.txtTcpRegNo.Size = new System.Drawing.Size(179, 22);
		this.txtTcpRegNo.TabIndex = 22;
		this.txtDocumentaryCreditNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocumentaryCreditNo.Location = new System.Drawing.Point(115, 64);
		this.txtDocumentaryCreditNo.Name = "txtDocumentaryCreditNo";
		this.txtDocumentaryCreditNo.Size = new System.Drawing.Size(125, 23);
		this.txtDocumentaryCreditNo.TabIndex = 3;
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(5, 276);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(83, 15);
		this.label31.TabIndex = 524;
		this.label31.Text = "Importer Bank";
		this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(427, 67);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 17);
		this.label17.TabIndex = 591;
		this.label17.Text = "*";
		this.label17.Visible = false;
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label113.Location = new System.Drawing.Point(352, 223);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(71, 15);
		this.label113.TabIndex = 57;
		this.label113.Text = "TCP REG No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(352, 17);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 57;
		this.label7.Text = "Lot Reference";
		this.label107.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label107.Location = new System.Drawing.Point(5, 60);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(99, 31);
		this.label107.TabIndex = 57;
		this.label107.Text = "Documentory Credit No && Date";
		this.txtRemarks2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks2.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks2.Location = new System.Drawing.Point(115, 349);
		this.txtRemarks2.Name = "txtRemarks2";
		this.txtRemarks2.Size = new System.Drawing.Size(764, 22);
		this.txtRemarks2.TabIndex = 40;
		this.txtRemarks2.Leave += new System.EventHandler(txtRemarks2_Leave);
		this.txtRemarks1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks1.Location = new System.Drawing.Point(115, 325);
		this.txtRemarks1.Name = "txtRemarks1";
		this.txtRemarks1.Size = new System.Drawing.Size(764, 22);
		this.txtRemarks1.TabIndex = 39;
		this.label118.BackColor = System.Drawing.Color.Gainsboro;
		this.label118.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label118.Location = new System.Drawing.Point(5, 349);
		this.label118.Name = "label118";
		this.label118.Size = new System.Drawing.Size(107, 22);
		this.label118.TabIndex = 576;
		this.label118.Text = "Remarks 2";
		this.label118.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtCertificate1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCertificate1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCertificate1.Location = new System.Drawing.Point(881, 40);
		this.txtCertificate1.Multiline = true;
		this.txtCertificate1.Name = "txtCertificate1";
		this.txtCertificate1.Size = new System.Drawing.Size(278, 98);
		this.txtCertificate1.TabIndex = 37;
		this.txtCertificate1.Leave += new System.EventHandler(txtCertificate1_Enter);
		this.label117.BackColor = System.Drawing.Color.Gainsboro;
		this.label117.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label117.Location = new System.Drawing.Point(5, 325);
		this.label117.Name = "label117";
		this.label117.Size = new System.Drawing.Size(107, 22);
		this.label117.TabIndex = 576;
		this.label117.Text = "Remarks 1";
		this.label117.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label61.BackColor = System.Drawing.Color.Gainsboro;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(881, 15);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(278, 22);
		this.label61.TabIndex = 576;
		this.label61.Text = "Certificate -1";
		this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(627, 251);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(93, 15);
		this.label57.TabIndex = 552;
		this.label57.Text = "No.Of Container";
		this.label62.BackColor = System.Drawing.Color.Gainsboro;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(881, 141);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(278, 22);
		this.label62.TabIndex = 577;
		this.label62.Text = "Certificate Of Origin";
		this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(627, 304);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(67, 15);
		this.label55.TabIndex = 554;
		this.label55.Text = "Net MTon's";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(352, 68);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(55, 15);
		this.label2.TabIndex = 590;
		this.label2.Text = "G.D Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(627, 278);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(77, 15);
		this.label54.TabIndex = 553;
		this.label54.Text = "Gross MTon's";
		this.txtCertificateOfOrigion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCertificateOfOrigion.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCertificateOfOrigion.Location = new System.Drawing.Point(881, 166);
		this.txtCertificateOfOrigion.Multiline = true;
		this.txtCertificateOfOrigion.Name = "txtCertificateOfOrigion";
		this.txtCertificateOfOrigion.Size = new System.Drawing.Size(278, 77);
		this.txtCertificateOfOrigion.TabIndex = 38;
		this.txtCertificateOfOrigion.Leave += new System.EventHandler(txtCertificateOfOrigion_Enter);
		this.CmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.CmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Location = new System.Drawing.Point(115, 271);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Name = "CmbImporterBank";
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Size = new System.Drawing.Size(210, 25);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).TabIndex = 12;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(5, 223);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(80, 15);
		this.label10.TabIndex = 120;
		this.label10.Text = "Notify Party 2";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(5, 43);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(65, 15);
		this.label67.TabIndex = 57;
		this.label67.Text = "Invoice No";
		this.label95.AutoSize = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.ForeColor = System.Drawing.Color.Red;
		this.label95.Location = new System.Drawing.Point(103, 67);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(14, 17);
		this.label95.TabIndex = 50;
		this.label95.Text = "*";
		this.CmbOriginCountry.AllowNull = (DefaultableBoolean)1;
		this.CmbOriginCountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOriginCountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOriginCountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance330).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance330).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance330).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOriginCountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance330;
		((AppearanceBase)appearance331).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance331;
		((SpecialBoxBase)((UltraGridBase)this.CmbOriginCountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance332).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance332).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance332).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance333).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance333;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance334).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance335;
		((AppearanceBase)appearance336).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance336).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance338).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance338).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance338).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance338).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance338).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance338;
		((AppearanceBase)appearance339).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance340).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance340;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance341).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOriginCountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOriginCountry).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOriginCountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOriginCountry).Location = new System.Drawing.Point(444, 271);
		((System.Windows.Forms.Control)(object)this.CmbOriginCountry).Name = "CmbOriginCountry";
		((System.Windows.Forms.Control)(object)this.CmbOriginCountry).Size = new System.Drawing.Size(179, 25);
		((System.Windows.Forms.Control)(object)this.CmbOriginCountry).TabIndex = 24;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(352, 171);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(80, 15);
		this.label12.TabIndex = 526;
		this.label12.Text = "Delivery Term";
		this.label109.AutoSize = true;
		this.label109.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label109.Location = new System.Drawing.Point(352, 276);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(40, 15);
		this.label109.TabIndex = 542;
		this.label109.Text = "Origin";
		this.txtGdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGdNo.Enabled = false;
		this.txtGdNo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdNo.Location = new System.Drawing.Point(444, 39);
		this.txtGdNo.Name = "txtGdNo";
		this.txtGdNo.Size = new System.Drawing.Size(179, 22);
		this.txtGdNo.TabIndex = 15;
		this.label129.AutoSize = true;
		this.label129.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label129.Location = new System.Drawing.Point(627, 17);
		this.label129.Name = "label129";
		this.label129.Size = new System.Drawing.Size(81, 15);
		this.label129.TabIndex = 542;
		this.label129.Text = "Delivery Place";
		this.label115.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label115.Location = new System.Drawing.Point(352, 294);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(79, 31);
		this.label115.TabIndex = 542;
		this.label115.Text = "Importer  Country\r\n";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(103, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 0;
		this.label3.Text = "*";
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label110.ForeColor = System.Drawing.Color.Red;
		this.label110.Location = new System.Drawing.Point(427, 275);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(14, 17);
		this.label110.TabIndex = 545;
		this.label110.Text = "*";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance326).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance326).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance326).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance326;
		((AppearanceBase)appearance337).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance337;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance440).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance440).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance440).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance440).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance440;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance441).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance441).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance441;
		((AppearanceBase)appearance442).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance442).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance442;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance357).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance357;
		((AppearanceBase)appearance443).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance443).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance443;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance444).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance444).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance444).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance444).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance444).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance444;
		((AppearanceBase)appearance445).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance445;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(444, 166);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(179, 25);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TextChanged += new System.EventHandler(CmbDeliveryTerm_TextChanged);
		this.CmbExporterBank.AllowNull = (DefaultableBoolean)1;
		this.CmbExporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance446).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance446).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance446;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance448).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance448).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance448).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance448).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance448;
		((AppearanceBase)appearance449).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance449;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance450).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance450).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance450).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance450).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance450;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance451).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance451).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance451;
		((AppearanceBase)appearance452).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance452).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance452;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExporterBank).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExporterBank).Location = new System.Drawing.Point(115, 297);
		((System.Windows.Forms.Control)(object)this.CmbExporterBank).Name = "CmbExporterBank";
		((System.Windows.Forms.Control)(object)this.CmbExporterBank).Size = new System.Drawing.Size(210, 25);
		((System.Windows.Forms.Control)(object)this.CmbExporterBank).TabIndex = 13;
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Red;
		this.label68.Location = new System.Drawing.Point(103, 42);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(14, 17);
		this.label68.TabIndex = 50;
		this.label68.Text = "*";
		this.txtDocumentaryCreditNoDate.CustomFormat = "dd-MMM-yy";
		this.txtDocumentaryCreditNoDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocumentaryCreditNoDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocumentaryCreditNoDate.Location = new System.Drawing.Point(241, 64);
		this.txtDocumentaryCreditNoDate.Name = "txtDocumentaryCreditNoDate";
		this.txtDocumentaryCreditNoDate.Size = new System.Drawing.Size(109, 22);
		this.txtDocumentaryCreditNoDate.TabIndex = 4;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(352, 111);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(79, 31);
		this.label16.TabIndex = 533;
		this.label16.Text = "Destination Port";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(5, 302);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(80, 15);
		this.label24.TabIndex = 525;
		this.label24.Text = "Exporter Bank";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label111.Location = new System.Drawing.Point(352, 250);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(59, 15);
		this.label111.TabIndex = 542;
		this.label111.Text = "Continent";
		this.txtDocDate.CustomFormat = "dd-MMM-yy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(242, 13);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(109, 22);
		this.txtDocDate.TabIndex = 1;
		this.CmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance466).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance466).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance466;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance467).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance467).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance467).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance467).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance467;
		((AppearanceBase)appearance468).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance468;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Location = new System.Drawing.Point(444, 88);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Name = "CmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Size = new System.Drawing.Size(179, 25);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).TabIndex = 17;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(5, 17);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 49;
		this.label5.Text = "Doc No";
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance262;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance264).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance264).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance266).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance267;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance268).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance269).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance269;
		((AppearanceBase)appearance270).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(115, 88);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(235, 25);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Leave += new System.EventHandler(cmbSupCust_Leave);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(427, 144);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 545;
		this.label41.Text = "*";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(352, 145);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(69, 15);
		this.label22.TabIndex = 542;
		this.label22.Text = "Carrier Type";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(5, 93);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(93, 15);
		this.label21.TabIndex = 78;
		this.label21.Text = "Customer Name";
		this.CmbNotifyParty2.AllowNull = (DefaultableBoolean)1;
		this.CmbNotifyParty2.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbNotifyParty2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbNotifyParty2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance276).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance276).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance277;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance278).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance280).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance283).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance284).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance284).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance284).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance284;
		((AppearanceBase)appearance197).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbNotifyParty2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).Location = new System.Drawing.Point(115, 218);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).Name = "CmbNotifyParty2";
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).Size = new System.Drawing.Size(235, 25);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).TabIndex = 10;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(102, 92);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(14, 17);
		this.label28.TabIndex = 111;
		this.label28.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(427, 92);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(14, 17);
		this.label26.TabIndex = 535;
		this.label26.Text = "*";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(102, 92);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(14, 17);
		this.label4.TabIndex = 115;
		this.label4.Text = "*";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(5, 197);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 117;
		this.label8.Text = "Notify Party 1";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(352, 43);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(46, 15);
		this.label46.TabIndex = 561;
		this.label46.Text = "G.D No";
		this.CmbNotifyParty1.AllowNull = (DefaultableBoolean)1;
		this.CmbNotifyParty1.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbNotifyParty1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbNotifyParty1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance151;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance173).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbNotifyParty1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).Location = new System.Drawing.Point(115, 192);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).Name = "CmbNotifyParty1";
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).Size = new System.Drawing.Size(235, 25);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).TabIndex = 9;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(352, 93);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(75, 15);
		this.label56.TabIndex = 534;
		this.label56.Text = "Loading Port";
		this.panel17.AutoScroll = true;
		this.panel17.Controls.Add(this.txtOtherRemarks2);
		this.panel17.Controls.Add(this.label155);
		this.panel17.Controls.Add(this.txtOtherRemarks1);
		this.panel17.Controls.Add(this.label154);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel17.Location = new System.Drawing.Point(3, 677);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1170, 52);
		this.panel17.TabIndex = 1;
		this.txtOtherRemarks2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherRemarks2.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherRemarks2.Location = new System.Drawing.Point(119, 26);
		this.txtOtherRemarks2.Name = "txtOtherRemarks2";
		this.txtOtherRemarks2.Size = new System.Drawing.Size(1045, 22);
		this.txtOtherRemarks2.TabIndex = 1;
		this.label155.AutoSize = true;
		this.label155.BackColor = System.Drawing.Color.Gainsboro;
		this.label155.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label155.Location = new System.Drawing.Point(4, 28);
		this.label155.Name = "label155";
		this.label155.Size = new System.Drawing.Size(114, 19);
		this.label155.TabIndex = 576;
		this.label155.Text = "Other Remarks 2";
		this.label155.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtOtherRemarks1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherRemarks1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherRemarks1.Location = new System.Drawing.Point(119, 3);
		this.txtOtherRemarks1.Name = "txtOtherRemarks1";
		this.txtOtherRemarks1.Size = new System.Drawing.Size(1045, 22);
		this.txtOtherRemarks1.TabIndex = 0;
		this.label154.AutoSize = true;
		this.label154.BackColor = System.Drawing.Color.Gainsboro;
		this.label154.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label154.Location = new System.Drawing.Point(4, 5);
		this.label154.Name = "label154";
		this.label154.Size = new System.Drawing.Size(112, 19);
		this.label154.TabIndex = 576;
		this.label154.Text = "Other Remarks 1";
		this.label154.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExportCompany);
		this.panel10.Controls.Add(this.labelExportCompany);
		this.panel10.Controls.Add(this.label23);
		this.panel10.Controls.Add(this.ChkPrintPreview);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1170, 27);
		this.panel10.TabIndex = 1;
		this.CmbExportCompany.AllowNull = (DefaultableBoolean)1;
		this.CmbExportCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExportCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExportCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance374).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance374;
		((AppearanceBase)appearance375).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance375).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance375;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance376).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance376).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance376).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance376).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance376).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance376;
		((AppearanceBase)appearance179).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExportCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).Location = new System.Drawing.Point(420, 0);
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).Name = "CmbExportCompany";
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).TabIndex = 500;
		this.labelExportCompany.AutoSize = true;
		this.labelExportCompany.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelExportCompany.ForeColor = System.Drawing.Color.White;
		this.labelExportCompany.Location = new System.Drawing.Point(320, 6);
		this.labelExportCompany.Name = "labelExportCompany";
		this.labelExportCompany.Size = new System.Drawing.Size(98, 15);
		this.labelExportCompany.TabIndex = 501;
		this.labelExportCompany.Text = "Export Company";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(169, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Commercial Invoice";
		this.ChkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreview.Location = new System.Drawing.Point(1083, 2);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPrintPreview.TabIndex = 32;
		this.ChkPrintPreview.Text = "Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.panel12.Controls.Add(this.toolStrip4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1170, 27);
		this.panel12.TabIndex = 0;
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[17]
		{
			this.btnNew, this.btnRefresh, this.btnsave, this.btnupdate, this.btnAttachment, this.btnPrint, this.BtnPrint521A, this.BtnPrint521B, this.BtnPrint521C, this.BtnPrint521D,
			this.BtnLoad, this.btnGenerateInvoiceNos, this.BtnExportCharges, this.btnItemDetailLookUp, this.btnGenerateInvoiceNo, this.btnShortCutKey, this.btnPackingList
		});
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip4.TabIndex = 4;
		this.toolStrip4.Text = "toolStrip4";
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
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "Print-521";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.BtnPrint521A.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint521A.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint521A.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint521A.Name = "BtnPrint521A";
		this.BtnPrint521A.Size = new System.Drawing.Size(108, 24);
		this.BtnPrint521A.Text = "Print-521-A";
		this.BtnPrint521A.Click += new System.EventHandler(BtnPrint521A_Click);
		this.BtnPrint521B.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint521B.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint521B.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint521B.Name = "BtnPrint521B";
		this.BtnPrint521B.Size = new System.Drawing.Size(107, 24);
		this.BtnPrint521B.Text = "Print-521-B";
		this.BtnPrint521B.Click += new System.EventHandler(BtnPrint521B_Click);
		this.BtnPrint521C.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint521C.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint521C.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint521C.Name = "BtnPrint521C";
		this.BtnPrint521C.Size = new System.Drawing.Size(108, 24);
		this.BtnPrint521C.Text = "Print-521-C";
		this.BtnPrint521C.Click += new System.EventHandler(BtnPrint521C_Click);
		this.BtnPrint521D.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint521D.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint521D.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint521D.Name = "BtnPrint521D";
		this.BtnPrint521D.Size = new System.Drawing.Size(108, 24);
		this.BtnPrint521D.Text = "Print-521-D";
		this.BtnPrint521D.Click += new System.EventHandler(BtnPrint521D_Click);
		this.BtnLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnLoad.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoad.Image = (System.Drawing.Image)resources.GetObject("BtnLoad.Image");
		this.BtnLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoad.Name = "BtnLoad";
		this.BtnLoad.Size = new System.Drawing.Size(145, 24);
		this.BtnLoad.Text = "Load Sales Contract";
		this.BtnLoad.Click += new System.EventHandler(BtnLoad_Click);
		this.btnGenerateInvoiceNos.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateInvoiceNos.Name = "btnGenerateInvoiceNos";
		this.btnGenerateInvoiceNos.Size = new System.Drawing.Size(153, 24);
		this.btnGenerateInvoiceNos.Text = "Generate Invoice Nos";
		this.btnGenerateInvoiceNos.Visible = false;
		this.btnGenerateInvoiceNos.Click += new System.EventHandler(btnGenerateInvoiceNos_Click);
		this.BtnExportCharges.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnExportCharges.Name = "BtnExportCharges";
		this.BtnExportCharges.Size = new System.Drawing.Size(111, 24);
		this.BtnExportCharges.Text = "Export Charges";
		this.BtnExportCharges.Click += new System.EventHandler(BtnExportCharges_Click);
		this.btnItemDetailLookUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnItemDetailLookUp.Name = "btnItemDetailLookUp";
		this.btnItemDetailLookUp.Size = new System.Drawing.Size(133, 20);
		this.btnItemDetailLookUp.Text = "ItemDetail_LookUp";
		this.btnItemDetailLookUp.Click += new System.EventHandler(btnItemDetailLookUp_Click);
		this.btnGenerateInvoiceNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerateInvoiceNo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateInvoiceNo.Name = "btnGenerateInvoiceNo";
		this.btnGenerateInvoiceNo.Size = new System.Drawing.Size(126, 20);
		this.btnGenerateInvoiceNo.Text = "&Generate Inv Nos";
		this.btnGenerateInvoiceNo.Click += new System.EventHandler(btnGenerateInvoiceNo_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnPackingList.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPackingList.Name = "btnPackingList";
		this.btnPackingList.Size = new System.Drawing.Size(89, 20);
		this.btnPackingList.Text = "Packing List";
		this.btnPackingList.Click += new System.EventHandler(btnPackingList_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.tabControl6);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1176, 732);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 164);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1170, 297);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.tabControl6.Controls.Add(this.tabInvoiceDetailH);
		this.tabControl6.Controls.Add(this.tabCustomDetailH);
		this.tabControl6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl6.Location = new System.Drawing.Point(3, 461);
		this.tabControl6.Name = "tabControl6";
		this.tabControl6.SelectedIndex = 0;
		this.tabControl6.Size = new System.Drawing.Size(1170, 268);
		this.tabControl6.TabIndex = 351;
		this.tabInvoiceDetailH.Controls.Add(this.tabControl3);
		this.tabInvoiceDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabInvoiceDetailH.Name = "tabInvoiceDetailH";
		this.tabInvoiceDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabInvoiceDetailH.Size = new System.Drawing.Size(1162, 239);
		this.tabInvoiceDetailH.TabIndex = 0;
		this.tabInvoiceDetailH.Text = "Invoice Detail";
		this.tabInvoiceDetailH.UseVisualStyleBackColor = true;
		this.tabControl3.Controls.Add(this.tabDetailH);
		this.tabControl3.Controls.Add(this.tabPaymentDetailH);
		this.tabControl3.Controls.Add(this.tabOtherChargesH);
		this.tabControl3.Controls.Add(this.tabPmH);
		this.tabControl3.Controls.Add(this.tabOtherItemH);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl3.Location = new System.Drawing.Point(3, 3);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1156, 233);
		this.tabControl3.TabIndex = 350;
		this.tabDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetailHistory);
		this.tabDetailH.Controls.Add(this.panel2);
		this.tabDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabDetailH.Name = "tabDetailH";
		this.tabDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetailH.Size = new System.Drawing.Size(1148, 204);
		this.tabDetailH.TabIndex = 0;
		this.tabDetailH.Text = "Item Detail";
		this.tabDetailH.UseVisualStyleBackColor = true;
		this.gridDetailHistory.AllowEdit = (InheritableBoolean)2;
		gridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridDetailHistory.DesignTimeLayout = gridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridDetailHistory.DynamicFiltering = true;
		this.gridDetailHistory.FilterMode = (FilterMode)2;
		this.gridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Name = "gridDetailHistory";
		((ScrollableBase)this.gridDetailHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetailHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.gridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Size = new System.Drawing.Size(1142, 173);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).TabIndex = 1;
		this.gridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.gridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label59);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1142, 25);
		this.panel2.TabIndex = 346;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(5, 5);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(228, 17);
		this.label59.TabIndex = 562;
		this.label59.Text = "Detail of the selected above row";
		this.tabPaymentDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentHistory);
		this.tabPaymentDetailH.Controls.Add(this.panel31);
		this.tabPaymentDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentDetailH.Name = "tabPaymentDetailH";
		this.tabPaymentDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabPaymentDetailH.Size = new System.Drawing.Size(1148, 204);
		this.tabPaymentDetailH.TabIndex = 1;
		this.tabPaymentDetailH.Text = "Payment Detail";
		this.tabPaymentDetailH.UseVisualStyleBackColor = true;
		this.grdPaymentHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentHistory).BackColor = System.Drawing.Color.White;
		this.grdPaymentHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentHistory.DesignTimeLayout = grdPaymentHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentHistory.FilterMode = (FilterMode)2;
		this.grdPaymentHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentHistory.GroupByBoxVisible = false;
		this.grdPaymentHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Name = "grdPaymentHistory";
		((ScrollableBase)this.grdPaymentHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Size = new System.Drawing.Size(1142, 173);
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).TabIndex = 347;
		this.grdPaymentHistory.TotalRow = (InheritableBoolean)1;
		this.grdPaymentHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentHistory).VisualStyle = (VisualStyle)4;
		this.panel31.BackColor = System.Drawing.Color.Teal;
		this.panel31.Controls.Add(this.label19);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel31.Location = new System.Drawing.Point(3, 3);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1142, 25);
		this.panel31.TabIndex = 348;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(5, 5);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(301, 17);
		this.label19.TabIndex = 562;
		this.label19.Text = "Payement Detail of the selected above row";
		this.tabOtherChargesH.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory);
		this.tabOtherChargesH.Controls.Add(this.panel8);
		this.tabOtherChargesH.Location = new System.Drawing.Point(4, 25);
		this.tabOtherChargesH.Name = "tabOtherChargesH";
		this.tabOtherChargesH.Size = new System.Drawing.Size(1148, 204);
		this.tabOtherChargesH.TabIndex = 4;
		this.tabOtherChargesH.Text = "Other Charges";
		this.tabOtherChargesH.UseVisualStyleBackColor = true;
		this.grdOtherChargesHistory.AllowEdit = (InheritableBoolean)2;
		grdOtherChargesHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherChargesHistory.DesignTimeLayout = grdOtherChargesHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOtherChargesHistory.DynamicFiltering = true;
		this.grdOtherChargesHistory.FilterMode = (FilterMode)2;
		this.grdOtherChargesHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherChargesHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Name = "grdOtherChargesHistory";
		((ScrollableBase)this.grdOtherChargesHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdOtherChargesHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.grdOtherChargesHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Size = new System.Drawing.Size(1148, 179);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).TabIndex = 347;
		this.grdOtherChargesHistory.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesHistory).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label36);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1148, 25);
		this.panel8.TabIndex = 348;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(5, 5);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(333, 17);
		this.label36.TabIndex = 562;
		this.label36.Text = "Other Charges Detail of the selected above row";
		this.tabPmH.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackListHistory);
		this.tabPmH.Controls.Add(this.panel32);
		this.tabPmH.Location = new System.Drawing.Point(4, 25);
		this.tabPmH.Name = "tabPmH";
		this.tabPmH.Size = new System.Drawing.Size(1148, 204);
		this.tabPmH.TabIndex = 2;
		this.tabPmH.Text = "Packing List";
		this.tabPmH.UseVisualStyleBackColor = true;
		this.grdPackListHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPackListHistory).BackColor = System.Drawing.Color.White;
		this.grdPackListHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPackListHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackListHistory.DesignTimeLayout = grdPackListHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackListHistory.FilterMode = (FilterMode)2;
		this.grdPackListHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackListHistory.GroupByBoxVisible = false;
		this.grdPackListHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Name = "grdPackListHistory";
		((ScrollableBase)this.grdPackListHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Size = new System.Drawing.Size(1148, 179);
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).TabIndex = 347;
		this.grdPackListHistory.TotalRow = (InheritableBoolean)1;
		this.grdPackListHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackListHistory).VisualStyle = (VisualStyle)4;
		this.panel32.BackColor = System.Drawing.Color.Teal;
		this.panel32.Controls.Add(this.label34);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(0, 0);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1148, 25);
		this.panel32.TabIndex = 348;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(5, 5);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(313, 17);
		this.label34.TabIndex = 562;
		this.label34.Text = "Packing List Detail of the selected above row";
		this.tabOtherItemH.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherItemHistory);
		this.tabOtherItemH.Controls.Add(this.panel33);
		this.tabOtherItemH.Location = new System.Drawing.Point(4, 25);
		this.tabOtherItemH.Name = "tabOtherItemH";
		this.tabOtherItemH.Size = new System.Drawing.Size(1148, 204);
		this.tabOtherItemH.TabIndex = 3;
		this.tabOtherItemH.Text = "Other Item";
		this.tabOtherItemH.UseVisualStyleBackColor = true;
		this.grdOtherItemHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOtherItemHistory).BackColor = System.Drawing.Color.White;
		this.grdOtherItemHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdOtherItemHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherItemHistory.DesignTimeLayout = grdOtherItemHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOtherItemHistory.FilterMode = (FilterMode)2;
		this.grdOtherItemHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherItemHistory.GroupByBoxVisible = false;
		this.grdOtherItemHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Name = "grdOtherItemHistory";
		((ScrollableBase)this.grdOtherItemHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Size = new System.Drawing.Size(1148, 179);
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).TabIndex = 347;
		this.grdOtherItemHistory.TotalRow = (InheritableBoolean)1;
		this.grdOtherItemHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherItemHistory).VisualStyle = (VisualStyle)4;
		this.panel33.BackColor = System.Drawing.Color.Teal;
		this.panel33.Controls.Add(this.label35);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel33.Location = new System.Drawing.Point(0, 0);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(1148, 25);
		this.panel33.TabIndex = 348;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(5, 5);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(310, 17);
		this.label35.TabIndex = 562;
		this.label35.Text = "Other Item Detail of the selected above row";
		this.tabCustomDetailH.Controls.Add(this.tabControl7);
		this.tabCustomDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabCustomDetailH.Name = "tabCustomDetailH";
		this.tabCustomDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabCustomDetailH.Size = new System.Drawing.Size(1162, 239);
		this.tabCustomDetailH.TabIndex = 1;
		this.tabCustomDetailH.Text = "Custom Detail";
		this.tabCustomDetailH.UseVisualStyleBackColor = true;
		this.tabControl7.Controls.Add(this.tabDetailCustomH);
		this.tabControl7.Controls.Add(this.tabPaymentDetailCustomH);
		this.tabControl7.Controls.Add(this.tabOtherChargesCustomH);
		this.tabControl7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl7.Location = new System.Drawing.Point(3, 3);
		this.tabControl7.Name = "tabControl7";
		this.tabControl7.SelectedIndex = 0;
		this.tabControl7.Size = new System.Drawing.Size(1156, 233);
		this.tabControl7.TabIndex = 352;
		this.tabDetailCustomH.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetailCustomHistory);
		this.tabDetailCustomH.Controls.Add(this.panel35);
		this.tabDetailCustomH.Location = new System.Drawing.Point(4, 25);
		this.tabDetailCustomH.Name = "tabDetailCustomH";
		this.tabDetailCustomH.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetailCustomH.Size = new System.Drawing.Size(1148, 204);
		this.tabDetailCustomH.TabIndex = 0;
		this.tabDetailCustomH.Text = "Item Detail";
		this.tabDetailCustomH.UseVisualStyleBackColor = true;
		this.gridDetailCustomHistory.AllowEdit = (InheritableBoolean)2;
		gridDetailCustomHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridDetailCustomHistory.DesignTimeLayout = gridDetailCustomHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridDetailCustomHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridDetailCustomHistory.DynamicFiltering = true;
		this.gridDetailCustomHistory.FilterMode = (FilterMode)2;
		this.gridDetailCustomHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetailCustomHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetailCustomHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridDetailCustomHistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.gridDetailCustomHistory).Name = "gridDetailCustomHistory";
		((ScrollableBase)this.gridDetailCustomHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetailCustomHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.gridDetailCustomHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetailCustomHistory).Size = new System.Drawing.Size(1142, 173);
		((System.Windows.Forms.Control)(object)this.gridDetailCustomHistory).TabIndex = 1;
		this.gridDetailCustomHistory.TotalRow = (InheritableBoolean)1;
		this.gridDetailCustomHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetailCustomHistory).VisualStyle = (VisualStyle)4;
		this.panel35.BackColor = System.Drawing.Color.Teal;
		this.panel35.Controls.Add(this.label104);
		this.panel35.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel35.Location = new System.Drawing.Point(3, 3);
		this.panel35.Name = "panel35";
		this.panel35.Size = new System.Drawing.Size(1142, 25);
		this.panel35.TabIndex = 346;
		this.label104.AutoSize = true;
		this.label104.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label104.ForeColor = System.Drawing.Color.White;
		this.label104.Location = new System.Drawing.Point(5, 5);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(228, 17);
		this.label104.TabIndex = 562;
		this.label104.Text = "Detail of the selected above row";
		this.tabPaymentDetailCustomH.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentCustomHistory);
		this.tabPaymentDetailCustomH.Controls.Add(this.panel36);
		this.tabPaymentDetailCustomH.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentDetailCustomH.Name = "tabPaymentDetailCustomH";
		this.tabPaymentDetailCustomH.Padding = new System.Windows.Forms.Padding(3);
		this.tabPaymentDetailCustomH.Size = new System.Drawing.Size(1148, 207);
		this.tabPaymentDetailCustomH.TabIndex = 1;
		this.tabPaymentDetailCustomH.Text = "Payment Detail";
		this.tabPaymentDetailCustomH.UseVisualStyleBackColor = true;
		this.grdPaymentCustomHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentCustomHistory).BackColor = System.Drawing.Color.White;
		this.grdPaymentCustomHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentCustomHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentCustomHistory.DesignTimeLayout = grdPaymentCustomHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentCustomHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentCustomHistory.FilterMode = (FilterMode)2;
		this.grdPaymentCustomHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPaymentCustomHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentCustomHistory.GroupByBoxVisible = false;
		this.grdPaymentCustomHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPaymentCustomHistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdPaymentCustomHistory).Name = "grdPaymentCustomHistory";
		((ScrollableBase)this.grdPaymentCustomHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentCustomHistory).Size = new System.Drawing.Size(1142, 176);
		((System.Windows.Forms.Control)(object)this.grdPaymentCustomHistory).TabIndex = 347;
		this.grdPaymentCustomHistory.TotalRow = (InheritableBoolean)1;
		this.grdPaymentCustomHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentCustomHistory).VisualStyle = (VisualStyle)4;
		this.panel36.BackColor = System.Drawing.Color.Teal;
		this.panel36.Controls.Add(this.label105);
		this.panel36.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel36.Location = new System.Drawing.Point(3, 3);
		this.panel36.Name = "panel36";
		this.panel36.Size = new System.Drawing.Size(1142, 25);
		this.panel36.TabIndex = 348;
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label105.ForeColor = System.Drawing.Color.White;
		this.label105.Location = new System.Drawing.Point(5, 5);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(301, 17);
		this.label105.TabIndex = 562;
		this.label105.Text = "Payement Detail of the selected above row";
		this.tabOtherChargesCustomH.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesCustomHistory);
		this.tabOtherChargesCustomH.Controls.Add(this.panel37);
		this.tabOtherChargesCustomH.Location = new System.Drawing.Point(4, 25);
		this.tabOtherChargesCustomH.Name = "tabOtherChargesCustomH";
		this.tabOtherChargesCustomH.Size = new System.Drawing.Size(1148, 207);
		this.tabOtherChargesCustomH.TabIndex = 4;
		this.tabOtherChargesCustomH.Text = "Other Charges";
		this.tabOtherChargesCustomH.UseVisualStyleBackColor = true;
		this.grdOtherChargesCustomHistory.AllowEdit = (InheritableBoolean)2;
		grdOtherChargesCustomHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherChargesCustomHistory.DesignTimeLayout = grdOtherChargesCustomHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesCustomHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOtherChargesCustomHistory.DynamicFiltering = true;
		this.grdOtherChargesCustomHistory.FilterMode = (FilterMode)2;
		this.grdOtherChargesCustomHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesCustomHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherChargesCustomHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesCustomHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesCustomHistory).Name = "grdOtherChargesCustomHistory";
		((ScrollableBase)this.grdOtherChargesCustomHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdOtherChargesCustomHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.grdOtherChargesCustomHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherChargesCustomHistory).Size = new System.Drawing.Size(1148, 182);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesCustomHistory).TabIndex = 347;
		this.grdOtherChargesCustomHistory.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesCustomHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesCustomHistory).VisualStyle = (VisualStyle)4;
		this.panel37.BackColor = System.Drawing.Color.Teal;
		this.panel37.Controls.Add(this.label106);
		this.panel37.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel37.Location = new System.Drawing.Point(0, 0);
		this.panel37.Name = "panel37";
		this.panel37.Size = new System.Drawing.Size(1148, 25);
		this.panel37.TabIndex = 348;
		this.label106.AutoSize = true;
		this.label106.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label106.ForeColor = System.Drawing.Color.White;
		this.label106.Location = new System.Drawing.Point(5, 5);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(333, 17);
		this.label106.TabIndex = 562;
		this.label106.Text = "Other Charges Detail of the selected above row";
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label92);
		this.panel15.Controls.Add(this.ctrlDataGridHistory);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 136);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1170, 28);
		this.panel15.TabIndex = 47;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(2, 3);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(142, 19);
		this.label92.TabIndex = 1;
		this.label92.Text = "Filtered Records";
		this.ctrlDataGridHistory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlDataGridHistory.FormName = this;
		this.ctrlDataGridHistory.Location = new System.Drawing.Point(1122, 1);
		this.ctrlDataGridHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlDataGridHistory.MyGrid = this.DataGridHistory;
		this.ctrlDataGridHistory.Name = "ctrlDataGridHistory";
		this.ctrlDataGridHistory.Size = new System.Drawing.Size(45, 25);
		this.ctrlDataGridHistory.TabIndex = 45;
		this.ctrlDataGridHistory.Load += new System.EventHandler(ctrlDataGridHistory_Load);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1170, 81);
		this.panel3.TabIndex = 46;
		this.groupBox2.Controls.Add(this.groupBox1);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.label73);
		this.groupBox2.Controls.Add(this.label89);
		this.groupBox2.Controls.Add(this.label90);
		this.groupBox2.Controls.Add(this.label91);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(963, 75);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.groupBox1.Controls.Add(this.rdAll);
		this.groupBox1.Controls.Add(this.rdReferred);
		this.groupBox1.Controls.Add(this.rdNotReferred);
		this.groupBox1.Location = new System.Drawing.Point(860, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(96, 67);
		this.groupBox1.TabIndex = 11;
		this.groupBox1.TabStop = false;
		this.rdAll.AutoSize = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.Location = new System.Drawing.Point(6, 48);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(39, 19);
		this.rdAll.TabIndex = 2;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.rdReferred.AutoSize = true;
		this.rdReferred.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdReferred.Location = new System.Drawing.Point(5, 29);
		this.rdReferred.Name = "rdReferred";
		this.rdReferred.Size = new System.Drawing.Size(69, 19);
		this.rdReferred.TabIndex = 1;
		this.rdReferred.Text = "Referred";
		this.rdReferred.UseVisualStyleBackColor = true;
		this.rdNotReferred.AutoSize = true;
		this.rdNotReferred.Checked = true;
		this.rdNotReferred.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdNotReferred.Location = new System.Drawing.Point(5, 11);
		this.rdNotReferred.Name = "rdNotReferred";
		this.rdNotReferred.Size = new System.Drawing.Size(92, 19);
		this.rdNotReferred.TabIndex = 0;
		this.rdNotReferred.TabStop = true;
		this.rdNotReferred.Text = "Not Referred";
		this.rdNotReferred.UseVisualStyleBackColor = true;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(754, 58);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(689, 44);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.btnShowHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(373, 50);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(53, 15);
		this.label48.TabIndex = 89;
		this.label48.Text = "Invoice#";
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance348).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance348).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance348;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance355).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance355).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance355).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance355).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance355;
		((AppearanceBase)appearance356).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance356;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance358).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance358).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance358).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance358).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance358;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance360).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance360;
		((AppearanceBase)appearance370).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance370).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance370;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance400).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance400;
		((AppearanceBase)appearance408).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance408).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance408;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance419).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance419).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance419).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance419).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance419).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance419;
		((AppearanceBase)appearance430).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance430;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance432).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance432).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance432;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance435).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance435;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(434, 44);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 5;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(373, 21);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(58, 15);
		this.label20.TabIndex = 87;
		this.label20.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(754, 10);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(754, 26);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 50);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(64, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(213, 21);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(79, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 50);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 21);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance300).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance300).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance300).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance301).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance301;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance302).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance302).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance302).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance302;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance303).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance303;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance305).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance307).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance307).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance308).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance308).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance308).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance308;
		((AppearanceBase)appearance309).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance311;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(434, 15);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 46);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(294, 17);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 46);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 17);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(754, 42);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label69);
		this.panel16.Controls.Add(this.toolStrip3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1170, 52);
		this.panel16.TabIndex = 0;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(2, 29);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(190, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Export Invoice History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.btnRefreshHistory, this.toolStripSeparator1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip3.TabIndex = 52;
		this.toolStrip3.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 761);
		this.panel1.TabIndex = 1;
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance304).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance304).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance304;
		((AppearanceBase)appearance315).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance315;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance381).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance381).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance381).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance381;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance389).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance389).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance389;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance286).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance287).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbFCurrencr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Location = new System.Drawing.Point(385, 129);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Name = "cmbFCurrencr";
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).TabIndex = 12;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance38;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1307, 237);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance313).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance313).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance314).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance314).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance314).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance288).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1404, 237);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.ultraCombo3.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo3.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo3.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo3.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance294).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance294).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance295).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance295;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance296).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance296).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance296).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance297;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo3.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Location = new System.Drawing.Point(111, 91);
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Name = "ultraCombo3";
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo3).TabIndex = 3;
		this.ultraCombo2.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo2.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance202;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance208).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Location = new System.Drawing.Point(385, 37);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Name = "ultraCombo2";
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).TabIndex = 9;
		this.ultraCombo1.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance377).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance377).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance378).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance378).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance378).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance378;
		((AppearanceBase)appearance379).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance379;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance380).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance380).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance380).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance380).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance380;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance382).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance382).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance382;
		((AppearanceBase)appearance383).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance383).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance383;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance384;
		((AppearanceBase)appearance385).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance385).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance385;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance386).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance386).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance386).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance386).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance386).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance386;
		((AppearanceBase)appearance213).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(385, 67);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 10;
		this.cmbEFormNo.AllowNull = (DefaultableBoolean)1;
		this.cmbEFormNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEFormNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEFormNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance219;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance224).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance227).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbEFormNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Location = new System.Drawing.Point(662, 164);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Name = "cmbEFormNo";
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Size = new System.Drawing.Size(164, 22);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).TabIndex = 0;
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(346, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(280, 27);
		this.miniToolStrip.TabIndex = 0;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(35, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.toolStripSeparator, this.toolStripButton4, this.toolStripButton5, this.helpToolStripButton });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(280, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip1";
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton2.Text = "&Save";
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(69, 24);
		this.toolStripButton3.Text = "&Update";
		this.toolStripButton3.Visible = false;
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton4.Text = "&Attachment";
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton5.Text = "&SMS";
		this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(24, 24);
		this.helpToolStripButton.Text = "He&lp";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 761);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmCommercialInvoiceIII";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmCommercialInvoiceIII_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ImProformaInvoice_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl4.ResumeLayout(false);
		this.tabInvoiceDetailll.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabItemDetail.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel23.ResumeLayout(false);
		this.tabPaymentDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermDetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstrumentsPTDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPTDetail).EndInit();
		this.tabChargesDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesNameChargeDetail).EndInit();
		this.tabPackingList.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackListDetail).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionNoPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPmDetailByItemCusPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemFromInvDetailPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommodityByItemCusPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerNoPackListDetail).EndInit();
		this.tabOtherItem.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOIDetail).EndInit();
		this.tabCustomDetail.ResumeLayout(false);
		this.tabControl5.ResumeLayout(false);
		this.tabItemDetailCustom.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailCustom).EndInit();
		this.panel27.ResumeLayout(false);
		this.tabPaymentDetailCustom.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermDetailCustom).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstrumentsPTDetailCustom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPTDetailCustom).EndInit();
		this.tabChargesDetailCustom.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetailCustom).EndInit();
		this.panel28.ResumeLayout(false);
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesNameChargeDetailCustom).EndInit();
		this.panel11.ResumeLayout(false);
		this.HeaderGropBox.ResumeLayout(false);
		this.HeaderGropBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlaceOfDelivery).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterCountry).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContinent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCareierType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbConsignee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOriginCountry).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty1).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportCompany).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.tabControl6.ResumeLayout(false);
		this.tabInvoiceDetailH.ResumeLayout(false);
		this.tabControl3.ResumeLayout(false);
		this.tabDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tabPaymentDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentHistory).EndInit();
		this.panel31.ResumeLayout(false);
		this.panel31.PerformLayout();
		this.tabOtherChargesH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabPmH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackListHistory).EndInit();
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		this.tabOtherItemH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherItemHistory).EndInit();
		this.panel33.ResumeLayout(false);
		this.panel33.PerformLayout();
		this.tabCustomDetailH.ResumeLayout(false);
		this.tabControl7.ResumeLayout(false);
		this.tabDetailCustomH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridDetailCustomHistory).EndInit();
		this.panel35.ResumeLayout(false);
		this.panel35.PerformLayout();
		this.tabPaymentDetailCustomH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentCustomHistory).EndInit();
		this.panel36.ResumeLayout(false);
		this.panel36.PerformLayout();
		this.tabOtherChargesCustomH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesCustomHistory).EndInit();
		this.panel37.ResumeLayout(false);
		this.panel37.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
