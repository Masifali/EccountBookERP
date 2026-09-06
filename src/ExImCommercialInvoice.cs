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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
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

public class ExImCommercialInvoice : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private int contractidInupdateCase;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool UpdateMode;

	private int updateDetailIndex = -1;

	private int updateDetailIndexOtherItem = -1;

	private int updateDetailIndexPT = -1;

	private int updateDetailIndexCommissionInfo = -1;

	private int updateDetailIndexOtherCharges = -1;

	private int InvoiceUpdateFlag;

	private bool FlagFin;

	private bool AutoGenInvoiceNo;

	private bool RateAddLessOnCommercialInvoice;

	private bool AllowExportMultiCompanies;

	private bool SaleCommissionTabFeatureWise;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool FormInitialized;

	private DataTable dtHistoryForLayout = new DataTable();

	private DataTable dtdetail4update = new DataTable();

	private DataTable dtdetail = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtOtherChargesDetail = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtInvoice = new DataTable();

	private DataTable dtFiList = new DataTable();

	private DataTable dtCommission = new DataTable();

	private LoadSalesContractForInvoice Loader;

	private List<ExImInvoicePackingDetail> LstRemoveRecordDetail = new List<ExImInvoicePackingDetail>();

	private IContainer components;

	private Panel panel4;

	private Panel panel5;

	private Panel panel1;

	private Panel panel7;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel8;

	private GroupBox HeaderGropBox;

	private Label label28;

	internal UltraCombo cmbSupCust;

	private Label label21;

	private Label label3;

	private DateTimePicker txtDocDate;

	private TextBox txtdocno;

	private Label label5;

	private Label label1;

	private Label label43;

	internal UltraCombo cmbproject;

	private Label label18;

	internal UltraCombo cmbbranches;

	private Label label13;

	internal GridEX grdDetail;

	private Panel panel10;

	private Label label23;

	private Panel panel12;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TabPage tabPage2;

	private Panel panel14;

	private Panel panel16;

	private ToolStrip toolStrip3;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo cmbFCurrencr;

	internal UltraCombo cmbPaymentTerm;

	internal UltraCombo combranches;

	internal UltraCombo comproject;

	internal UltraCombo ultraCombo3;

	internal UltraCombo CmbLCContractor;

	internal UltraCombo ultraCombo2;

	internal UltraCombo ultraCombo1;

	internal UltraCombo cmbCarierType;

	internal UltraCombo cmbExporterBank;

	internal UltraCombo cmbImporterBank;

	internal UltraCombo cmbEFormNo;

	internal UltraCombo cmbdeliveryterm;

	private ToolStrip miniToolStrip;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton toolStripButton4;

	private ToolStripButton toolStripButton5;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip2;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label4;

	internal UltraCombo cmbNotifyParty2;

	private Label label10;

	internal UltraCombo cmbNotifyParty1;

	private Label label8;

	private Label label7;

	private TextBox txtLotRef;

	private Label label9;

	private Label label11;

	private Label label12;

	internal UltraCombo cmbexporterBankNew;

	internal UltraCombo cmbimporterBankNew;

	internal UltraCombo cmbPaymentTermsNew;

	internal UltraCombo cmbdeliverytermnew;

	private Label label24;

	private Label label19;

	private Label label26;

	private Label label56;

	internal UltraCombo cmbDestinationPort;

	internal UltraCombo cmbLoadingPort;

	private Label label16;

	private Label label41;

	private Label label47;

	internal UltraCombo cmbcareiertype;

	private Label label22;

	private TextBox txtfcyAmount;

	private Label label25;

	private Label label27;

	internal UltraCombo cmbfcycode;

	private TextBox txtLocalAmt;

	private Label label42;

	private TextBox txtExRate;

	private Label label44;

	private Label label45;

	private DateTimePicker txtIformDate;

	private TextBox txtIformNo;

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

	private TextBox txtcertificateOfOrigion;

	private TextBox txtcertificate;

	private Label label65;

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

	internal UltraCombo CmbCreditAccount;

	private Label lblCreditAc;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel22;

	private Panel panel24;

	private Panel panel23;

	private Panel panel17;

	private Panel panel11;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private GroupBox GroupDetail;

	private Label label72;

	internal UltraCombo CmbCropYear;

	private TextBox txtpackingdetail;

	private Label label39;

	private TextBox txtitemdetail;

	private Label label34;

	private TextBox txtCostMTon;

	private Label label29;

	private TextBox txtNoOfBags;

	private Label label30;

	internal UltraCombo combitem;

	private Label label32;

	internal UltraCombo combpcktype;

	private Label label33;

	private TextBox txtQtyMTon;

	private Button btnCancel;

	private Label label35;

	private Button btnGridUpdate;

	private Button btnAddinGrid;

	internal UltraCombo CmbPackUom;

	private Label label36;

	private TextBox txtAmount;

	private Label label37;

	private Label label38;

	internal UltraCombo CmbRateUom;

	private Panel panel9;

	private TabPage tabPage4;

	private Panel panel19;

	internal GridEX grdotheritems;

	private Panel panel18;

	private Button btnoCancel;

	private Button btnoUpdate;

	private Button btnoadd;

	private Label label14;

	private TextBox txtoRemarks;

	private TextBox txtoQty;

	private Label label15;

	private TextBox txtoRate;

	private Label label63;

	private Label label70;

	private TextBox txtoAmount;

	internal UltraCombo CmbOtherItem;

	private Label label71;

	private TabPage tabPage5;

	private Panel panel21;

	internal GridEX grdpaymentdetail;

	private Panel panel20;

	private Label label82;

	private Label label81;

	private Label label80;

	private Label label79;

	private Button btndcancel;

	private Button btndupdate;

	private Button btndadd;

	private TextBox dtxtdueday;

	private Label label78;

	private TextBox dtxtfcyamount;

	private Label label77;

	internal UltraCombo dcmbpaymentterm;

	private Label label76;

	private TextBox txtBalanceFI;

	private Label label83;

	private TextBox dtxtpercentoftotal;

	private Label label74;

	internal UltraCombo CmbFinInstruments;

	private Label label75;

	private Label label85;

	private Label label84;

	private Label label51;

	private Label label17;

	private Label label2;

	private Panel panel2;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar4;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKey;

	internal UltraCombo cmbConsignee;

	private Label label86;

	private ToolStripButton BtnLoad;

	internal UltraCombo CmbContractSchedule;

	private Label label6;

	private ToolStripButton btnGenerateInvoiceNos;

	private ToolStripButton btnItemDetailLookUp;

	private Label label52;

	private Label label58;

	private TextBox txtPaymentRemarksDetail;

	private TextBox txtPaymentRemarks;

	private TextBox txtDeliveryRemarks;

	private Label label87;

	private Label label88;

	private TextBox cmbInvoiceNo;

	internal UltraCombo CmbHSCode;

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

	private Label label93;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label94;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbExportCompany;

	private Label labelExportCompany;

	internal UltraCombo CmbFarmingNTrade;

	private Label label95;

	private Label label98;

	private Label label97;

	private Label label96;

	private Label label59;

	private Label label100;

	private Label label99;

	private Label label103;

	private Label label102;

	private Label label101;

	private Label label105;

	private Label label104;

	private Label label48;

	internal UltraCombo CmbInvoiceNoHistory;

	private GroupBox groupBox1;

	private RadioButton rdAll;

	private RadioButton rdReferred;

	private RadioButton rdNotReferred;

	private TextBox txtCustomerContractNos;

	private Label label106;

	private ToolStripButton BtnPrintPL;

	private TextBox txtBalanceInvoiceAmtPaymentGrid;

	private Label label107;

	private TabPage tabPage6;

	internal GridEX grdCommissionInfo;

	private Panel panel13;

	private Label label108;

	private Panel panel25;

	private Label label109;

	internal UltraCombo CmbSalePerson;

	private TextBox txtCommRate;

	private TextBox txtCommissionAmount;

	internal UltraCombo cmbCommissionType;

	private Label label110;

	private Label label111;

	private Label label112;

	private Label label113;

	private Label label114;

	private TextBox txtCommissionExchangeRate;

	private TextBox txtCommissionLcyAmount;

	private Label label115;

	private TextBox txtCommRateUom;

	private TextBox txtCommissionRemarks;

	private Button btnAddCommission;

	private Button btnCancelCommission;

	private Button btnUpdateCommision;

	private Label label116;

	private CtrlGrdBar ctrlGrdBar5;

	private ToolStripButton btnPrint501B;

	private TabPage tabOtherCharges;

	private Panel panel26;

	private Button BtnAddChargeDetail;

	private Label label130;

	private TextBox txtRemarksChargeDetail;

	private Label label134;

	private Label label131;

	private Label label132;

	internal UltraCombo CmbChargesNameChargeDetail;

	private Label label136;

	private Button BtnCancelChargeDetail;

	private Button BtnUpdateChargeDetail;

	private Label label133;

	private Label label140;

	private TextBox txtLessAmountChargeDetail;

	private TextBox txtAddAmountChargeDetail;

	private Panel panel27;

	private CtrlGrdBar ctrlgrdOtherChargesDetail;

	internal GridEX grdOtherChargesDetail;

	private ToolStripButton btnExportCharges;

	private TextBox txtSubContractExpiryDays;

	private Label label117;

	public ExImCommercialInvoice(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ImProformaInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			InitializeEssentialComponents();
			Task.Run(() => LoadNonEssentialDataAsync());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeEssentialComponents()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			ConfigureRights();
			ConfigureControls();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigureRights()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 700);
			cmbInvoiceNo.Enabled = true;
			lblCreditAc.Visible = false;
			((Control)(object)CmbCreditAccount).Visible = false;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FlagFin = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("FinancialisActiveonCommercialInvoice"));
			lblCreditAc.Visible = true;
			((Control)(object)CmbCreditAccount).Visible = true;
			AutoGenInvoiceNo = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("CommercialInvoicePrefix"));
			RateAddLessOnCommercialInvoice = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("RateAddLessOnCommercialInvoice"));
			if (AutoGenInvoiceNo)
			{
				cmbInvoiceNo.Enabled = false;
				GenerateInvoiceNo();
			}
			else
			{
				cmbInvoiceNo.Enabled = true;
			}
			formrights = CommonServices.SetRightsValueInRightsObject("EximInvoice");
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			btnPrint.Enabled = formrights.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			AllowExportMultiCompanies = CommonServices.GetERPFeatureById(9);
			SaleCommissionTabFeatureWise = CommonServices.GetERPFeatureById(15);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigureControls()
	{
		try
		{
			dtInvoice.Columns.Add("Id", typeof(int));
			dtInvoice.Columns.Add("InvoiceNo", typeof(string));
			dtInvoice.Columns.Add("MainId", typeof(int));
			dtdetail.Columns.Add("Id");
			dtdetail.Columns.Add("ContractId", typeof(int));
			dtdetail.Columns.Add("ContractDetailId", typeof(int));
			dtdetail.Columns.Add("ContractNo", typeof(string));
			dtdetail.Columns.Add("ContractDate", typeof(DateTime));
			dtdetail.Columns.Add("ContractNoInvoiceWise", typeof(string));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("PackTypeId", typeof(int));
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("JobLotId", typeof(int));
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("PackSizeId", typeof(int));
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("NoOfBags", typeof(double));
			dtdetail.Columns.Add("RateWithoutAddLess", typeof(double));
			dtdetail.Columns.Add("RateAddLess", typeof(double));
			dtdetail.Columns.Add("Cost/M.Ton", typeof(double));
			dtdetail.Columns.Add("ContractRate", typeof(double));
			dtdetail.Columns.Add("RateUOMId", typeof(int));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateEquivalent", typeof(double));
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("ContractScheduleId", typeof(int));
			dtdetail.Columns.Add("ContractSchedule");
			dtdetail.Columns.Add("HSCode");
			dtdetail.Columns.Add("ItemDetail");
			dtdetail.Columns.Add("PackingDetail");
			dtdetail.Columns.Add("OtherItemAmount", typeof(double));
			dtdetail.Columns.Add("AddLessAmount", typeof(double));
			dtdetail.Columns.Add("FlagToUpdateScheduleCombo");
			dtdetail.Columns.Add("ExImFarmingNTradeId", typeof(int));
			dtdetail.Columns.Add("FarmingNTrade");
			dtdetail.Columns.Add("ExImFarmingTypeId", typeof(int));
			dtdetail.Columns.Add("ExImTradeTypeId", typeof(int));
			dtdetail.Columns.Add("ContractScheduleDetailId", typeof(int));
			dtdetail.Columns.Add("ScheduleFCL", typeof(double));
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			dtOtherItem.Columns.Add("ItemId");
			dtOtherItem.Columns.Add("ItemName");
			dtOtherItem.Columns.Add("Qty", typeof(double));
			dtOtherItem.Columns.Add("Rate", typeof(double));
			dtOtherItem.Columns.Add("Amount", typeof(double));
			dtOtherItem.Columns.Add("Remarks");
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			dtPaymentTerm.Columns.Add("Id");
			dtPaymentTerm.Columns.Add("PaymentTermId");
			dtPaymentTerm.Columns.Add("PaymentTerm");
			dtPaymentTerm.Columns.Add("DocumentTypeId");
			dtPaymentTerm.Columns.Add("ExImEFormRegistrationId");
			dtPaymentTerm.Columns.Add("FinancialInstrumentNo");
			dtPaymentTerm.Columns.Add("%OfTotal", typeof(double));
			dtPaymentTerm.Columns.Add("FcyAmount", typeof(double));
			dtPaymentTerm.Columns.Add("DueDays");
			dtPaymentTerm.Columns.Add("Remarks");
			grdpaymentdetail.DataSource = dtPaymentTerm;
			grdpaymentdetail.RetrieveStructure();
			grdSettingsPaymentTerm();
			dtCommission.Columns.Add("Id", typeof(int));
			dtCommission.Columns.Add("SalesPersonId", typeof(int));
			dtCommission.Columns.Add("SalesPerson", typeof(string));
			dtCommission.Columns.Add("CommissionTypeId", typeof(int));
			dtCommission.Columns.Add("CommissionType", typeof(string));
			dtCommission.Columns.Add("Rate", typeof(double));
			dtCommission.Columns.Add("RateUom", typeof(string));
			dtCommission.Columns.Add("FcyAmount", typeof(double));
			dtCommission.Columns.Add("ExchangeRate", typeof(double));
			dtCommission.Columns.Add("LcyAmount", typeof(double));
			dtCommission.Columns.Add("Remarks", typeof(string));
			grdCommissionInfo.DataSource = dtCommission;
			grdCommissionInfo.RetrieveStructure();
			dtOtherChargesDetail.Columns.Add("Id", typeof(int));
			dtOtherChargesDetail.Columns.Add("ContractId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ContractOtherChargesDetailId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItemId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItem");
			dtOtherChargesDetail.Columns.Add("AddAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("LessAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("Remarks");
			grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
			grdOtherChargesDetail.RetrieveStructure();
			grdSettingsOtherChargesDetail(grdOtherChargesDetail);
			tabControl2.SelectedIndex = 0;
			btnAddinGrid.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			ChkPrintPreview.Checked = true;
			txtDocDate.Focus();
			if (AllowExportMultiCompanies)
			{
				((Control)(object)CmbExportCompany).Visible = true;
				labelExportCompany.Visible = true;
				ExportCompaniesByCompanyIdBind();
			}
			else
			{
				((Control)(object)CmbExportCompany).Visible = false;
				labelExportCompany.Visible = false;
			}
			if (!SaleCommissionTabFeatureWise)
			{
				tabControl2.TabPages.Remove(tabPage6);
			}
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async Task LoadNonEssentialDataAsync()
	{
		try
		{
			await Task.Run(delegate
			{
				BindData();
			});
			Invoke((Action)delegate
			{
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindData()
	{
		try
		{
			branchesGetAll();
			projectGetAll();
			generateCode();
			BindCommercialInvoiceNofromlookup();
			CreditGlAccountFill();
			SupplierCustomerFromContracts();
			CustomerGetAll();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			CarierType();
			BindOtherItems();
			MultiCurrencyfill();
			CropYearFill();
			JobLotFill();
			ItemPackingTypeFill();
			ItemSpecificationsBind();
			FarmingNTradeBind();
			OtherChargesBind();
			if (SaleCommissionTabFeatureWise)
			{
				salesPersonFill();
				CommissionTypeFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FarmingNTradeBind()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int id = Conversion.ToInt(CmbFarmingNTrade.Value);
				DataTable farmingNTrade = ExImLcOrder.GetFarmingNTrade();
				if (farmingNTrade.Rows.Count > 0)
				{
					DDL.BindDDL(farmingNTrade, CmbFarmingNTrade, "Id", "FarmingNTrade", "Farming N Trade ", ZeroIndex: true);
					((UltraGridBase)CmbFarmingNTrade).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbFarmingNTrade).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					if (id > 0)
					{
						if (farmingNTrade.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
						{
							CmbFarmingNTrade.Value = id;
						}
						else
						{
							((Control)(object)CmbFarmingNTrade).Text = string.Empty;
						}
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportCompaniesByCompanyIdBind()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int id = Conversion.ToInt(CmbExportCompany.Value);
				DataTable exportCompaniesByCompany = ExImLcOrder.GetExportCompaniesByCompany(UserAccount.OrganizationId, UserAccount.CompanyId);
				if (exportCompaniesByCompany.Rows.Count > 0)
				{
					DDL.BindDDL(exportCompaniesByCompany, CmbExportCompany, "Id", "CompName", "Export Company ", ZeroIndex: true);
					if (id > 0)
					{
						if (exportCompaniesByCompany.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
						{
							CmbExportCompany.Value = id;
						}
						else
						{
							((Control)(object)CmbExportCompany).Text = string.Empty;
						}
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RunOnUiThread(Action action)
	{
		if (base.InvokeRequired)
		{
			Invoke(action);
		}
		else
		{
			action();
		}
	}

	private void branchesGetAll()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable dataTable = CommonServices.BrancheServiceBind();
				if (dataTable.Rows.Count > 0)
				{
					DDL.BindDDLNew(dataTable, cmbbranches, "Id", "BranchName", "Branch Name", false);
					((UltraGridBase)cmbbranches).Rows[0].Activate();
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void projectGetAll()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable dataTable = CommonServices.ProjectServiceBind();
				if (dataTable.Rows.Count > 0)
				{
					DDL.BindDDLNew(dataTable, cmbproject, "Id", "ProjectName", "Project Name", false);
					((UltraGridBase)cmbproject).Rows[0].Activate();
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void generateCode()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Expected O, but got Unknown
				int num = ExImInvoice.GenerateCode(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = UserAccount.BranchesId,
					DocumentTypeId = 204,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (num > 0)
				{
					txtdocno.Text = num.ToString();
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCommercialInvoiceNofromlookup()
	{
	}

	private void SupplierCustomerFromContracts()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Expected O, but got Unknown
				int id = Conversion.ToInt(cmbSupCust.Value);
				DataTable dataTable = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						"Name"
					}
				};
				DataTable dataForDropDownFromExportContract = ExImLcOrder.GetDataForDropDownFromExportContract(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Activity = "Customer"
				});
				if (dataForDropDownFromExportContract.Rows.Count > 0)
				{
					for (int i = 0; i < dataForDropDownFromExportContract.Rows.Count; i++)
					{
						dataTable.Rows.Add(dataForDropDownFromExportContract.Rows[i]["Id"], dataForDropDownFromExportContract.Rows[i]["name"]);
					}
				}
				DDL.BindDDL(dataTable, cmbSupCust, "Id", "Name", "Customer Name", ZeroIndex: true);
				((UltraGridBase)cmbSupCust).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (id > 0)
				{
					if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbSupCust.Value = id;
					}
					else
					{
						((Control)(object)cmbSupCust).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerGetAll()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Expected O, but got Unknown
				int id1 = Conversion.ToInt(cmbNotifyParty1.Value);
				int id2 = Conversion.ToInt(cmbNotifyParty2.Value);
				DataTable dataTable = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				DDL.BindDDLNew(dataTable, cmbNotifyParty1, "Id", "CompanyName", "Notify Party1", true);
				DDL.BindDDLNew(dataTable, cmbNotifyParty2, "Id", "CompanyName", "Notify Party2", true);
				((UltraGridBase)cmbNotifyParty1).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbNotifyParty2).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (id1 > 0)
				{
					if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id1))
					{
						cmbNotifyParty1.Value = id1;
					}
					else
					{
						((Control)(object)cmbNotifyParty1).Text = string.Empty;
					}
				}
				if (id2 > 0)
				{
					if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id2))
					{
						cmbNotifyParty2.Value = id2;
					}
					else
					{
						((Control)(object)cmbNotifyParty2).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int id = Conversion.ToInt(cmbdeliveryterm.Value);
				DataTable dataTable = ExImDeliveryTerm.Getall();
				DDL.BindDDL(dataTable, cmbdeliverytermnew, "Id", "Code", "DeliveryTerms", ZeroIndex: true);
				if (id > 0)
				{
					if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbdeliverytermnew.Value = id;
					}
					else
					{
						((Control)(object)cmbdeliverytermnew).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Expected O, but got Unknown
				int id = Conversion.ToInt(cmbPaymentTermsNew.Value);
				int id2 = Conversion.ToInt(dcmbpaymentterm.Value);
				DataTable dataTable = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (dataTable.Rows.Count > 0)
				{
					DataTable dataTable2 = new DataTable
					{
						Columns = 
						{
							{
								"Id",
								typeof(int)
							},
							{
								"lcOrderTerm",
								typeof(string)
							},
							{
								"Description",
								typeof(string)
							}
						}
					};
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						dataTable2.Rows.Add(Conversion.ToInt(dataTable.Rows[i]["Id"]), Conversion.ToString(dataTable.Rows[i]["lcOrderTerm"]), Conversion.ToString(dataTable.Rows[i]["lcOrderTermDesc"]));
					}
					DDL.BindDDL(dataTable2, cmbPaymentTermsNew, "Id", "lcOrderTerm", "Payment Term", ZeroIndex: true);
					DDL.BindDDL(dataTable2, dcmbpaymentterm, "Id", "lcOrderTerm", "Payment Term", ZeroIndex: true);
					if (dataTable2.Rows.Count > 0)
					{
						if (id > 0)
						{
							if (dataTable2.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
							{
								cmbPaymentTermsNew.Value = id;
							}
							else
							{
								((Control)(object)cmbPaymentTermsNew).Text = string.Empty;
							}
						}
						if (id2 > 0)
						{
							if (dataTable2.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id2))
							{
								dcmbpaymentterm.Value = id2;
							}
							else
							{
								((Control)(object)dcmbpaymentterm).Text = string.Empty;
							}
						}
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingPortFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Expected O, but got Unknown
				int LoadId = Conversion.ToInt(cmbLoadingPort.Value);
				int DestId = Conversion.ToInt(cmbDestinationPort.Value);
				DataTable dataTable = SeaPorts.Getall(new SeaPorts
				{
					CompanyId = UserAccount.CompanyId,
					OrganizationId = UserAccount.OrganizationId
				});
				DataTable dataTable2 = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						"PortName"
					}
				};
				DataTable dataTable3 = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						"PortName"
					}
				};
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						if (Conversion.ToString(dataTable.Rows[i]["PortType"]) == "Loading")
						{
							dataTable2.Rows.Add(dataTable.Rows[i]["Id"], dataTable.Rows[i]["PortName"]);
						}
						else if (Conversion.ToString(dataTable.Rows[i]["PortType"]) == "Destination")
						{
							dataTable3.Rows.Add(dataTable.Rows[i]["Id"], dataTable.Rows[i]["PortName"]);
						}
					}
				}
				DDL.BindDDL(dataTable2, cmbLoadingPort, "Id", "PortName", "Port Name", ZeroIndex: true);
				DDL.BindDDL(dataTable3, cmbDestinationPort, "Id", "PortName", "Port Name", ZeroIndex: true);
				if (LoadId > 0)
				{
					if (dataTable2.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == LoadId))
					{
						cmbLoadingPort.Value = LoadId;
					}
					else
					{
						((Control)(object)cmbLoadingPort).Text = string.Empty;
					}
				}
				if (DestId > 0)
				{
					if (dataTable3.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DestId))
					{
						cmbDestinationPort.Value = DestId;
					}
					else
					{
						((Control)(object)cmbDestinationPort).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImporterandExportBankFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Expected O, but got Unknown
				int id = Conversion.ToInt(cmbimporterBankNew.Value);
				int id2 = Conversion.ToInt(cmbexporterBankNew.Value);
				Bank val = new Bank
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				};
				DataTable dataTable = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						"BranchName"
					}
				};
				DataTable dataTable2 = new DataTable
				{
					Columns = 
					{
						{
							"Id",
							typeof(int)
						},
						"BranchName"
					}
				};
				List<Bank> all = Bank.GetAll(val);
				if (all.Count > 0)
				{
					for (int i = 0; i < all.Count; i++)
					{
						if (all[i].IsHomeland == "Home Country")
						{
							dataTable.Rows.Add(all[i].Id, all[i].BranchName);
						}
						if (all[i].IsHomeland == "Foreign Country")
						{
							dataTable2.Rows.Add(all[i].Id, all[i].BranchName);
						}
					}
				}
				DDL.BindDDL(dataTable, cmbexporterBankNew, "Id", "BranchName", "ExporterBank", ZeroIndex: true);
				DDL.BindDDL(dataTable2, cmbimporterBankNew, "Id", "BranchName", "ImporterBank", ZeroIndex: true);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					((UltraGridBase)cmbexporterBankNew).Rows[1].Activate();
				}
				if (id > 0)
				{
					if (dataTable2.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbimporterBankNew.Value = id;
					}
					else
					{
						((Control)(object)cmbimporterBankNew).Text = string.Empty;
					}
				}
				if (id2 > 0)
				{
					if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id2))
					{
						cmbexporterBankNew.Value = id2;
					}
					else
					{
						((Control)(object)cmbexporterBankNew).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CarierType()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable dt = new DataTable
				{
					Columns = { "Id", "Type" },
					Rows = 
					{
						new object[2] { 1, "By Sea" },
						new object[2] { 2, "By Air" },
						new object[2] { 3, "By Road" }
					}
				};
				DDL.BindDDL(dt, cmbcareiertype, "Id", "Type", "Carier type", ZeroIndex: true);
				((UltraGridBase)cmbcareiertype).Rows[1].Activate();
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void salesPersonFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int num = Conversion.ToInt(CmbSalePerson.Value);
				DataTable supplierustomerByCustomerGroupId = CommonServices.GetSupplierustomerByCustomerGroupId("9");
				DDL.BindDDL(supplierustomerByCustomerGroupId, CmbSalePerson, "Id", "CompanyName", "Sales Person", ZeroIndex: true);
				((UltraGridBase)CmbSalePerson).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (supplierustomerByCustomerGroupId.Rows.Count > 0 && num > 0)
				{
					CommonServices.SetComboValue(CmbSalePerson, "Id", num, supplierustomerByCustomerGroupId, ActivateRow: true);
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable dt = new DataTable
				{
					Columns = { "Id", "CommissionType" },
					Rows = 
					{
						new object[2] { 1, "Fixed Commission Amount" },
						new object[2] { 2, "Commission % of Value" },
						new object[2] { 3, "Commission By Weight Kg" }
					}
				};
				DDL.BindDDL(dt, cmbCommissionType, "Id", "CommissionType", "Commission Type", ZeroIndex: true);
				((UltraGridBase)cmbCommissionType).Rows[2].Activate();
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindOtherItems()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int id = Conversion.ToInt(CmbOtherItem.Value);
				DataTable itemByItemTypeId = CommonServices.GetItemByItemTypeId("14");
				if (itemByItemTypeId.Rows.Count > 0)
				{
					DDL.BindDDLNew(itemByItemTypeId, CmbOtherItem, "Id", "ItemName", "Item Name", true);
					((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
					if (id > 0)
					{
						if (itemByItemTypeId.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
						{
							CmbOtherItem.Value = id;
						}
						else
						{
							((Control)(object)CmbOtherItem).Text = string.Empty;
						}
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyfill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Expected O, but got Unknown
				int id = Conversion.ToInt(cmbfcycode.Value);
				DataTable all = MultiCurrency.GetAll(new MultiCurrency
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				DDL.BindDDLNew(all, cmbfcycode, "Id", "CurrencyName", "CurrencyName", true);
				if (id > 0)
				{
					if (all.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbfcycode.Value = id;
					}
					else
					{
						((Control)(object)cmbfcycode).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int id = Conversion.ToInt(CmbCropYear.Value);
				DataTable dataTable = CommonServices.CropYearGetAllService();
				DDL.BindDDLNew(dataTable, CmbCropYear, "Id", "CropYear", "CropYear", true);
				if (id > 0)
				{
					if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbCropYear.Value = id;
					}
					else
					{
						((Control)(object)CmbCropYear).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int id = Conversion.ToInt(CmbJobLot.Value);
				DataTable dataTable = CommonServices.JobLotGetAllService();
				DDL.BindDDLNew(dataTable, CmbJobLot, "Id", "JobLotDescription", "Job Lot", true);
				if (id > 0)
				{
					if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbJobLot.Value = id;
					}
					else
					{
						((Control)(object)CmbJobLot).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemPackingTypeFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Expected O, but got Unknown
				int id = Conversion.ToInt(combpcktype.Value);
				DataTable dataTable = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				DDL.BindDDLNew(dataTable, combpcktype, "Id", "Description", "Description", true);
				if (id > 0)
				{
					if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						combpcktype.Value = id;
					}
					else
					{
						((Control)(object)combpcktype).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherChargesBind()
	{
		try
		{
			RunOnUiThread(delegate
			{
				DataTable source = ExportCharges.ComboBind(UserAccount.OrganizationId, UserAccount.CompanyId);
				InfragisticsHelper.BindAndRetainSelection(CmbChargesNameChargeDetail, source, "ExportChargesID", "ExportChargesName", "Export Charges", AllColumns: true, Conversion.ToInt(CmbChargesNameChargeDetail.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemSpecificationsBind()
	{
	}

	private void GenerateInvoiceNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			string Code = ExImInvoice.GenerateInvoiceNo(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 204,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code != string.Empty)
			{
				cmbInvoiceNo.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HSCodeBind()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbHSCode.Value);
			DataTable lst = CommodityDetailItemCustomerWise.GetRemarks(new CommodityDetailItemCustomerWise
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(combitem.Value),
				SupplierCustomerId = Conversion.ToInt(cmbSupCust.Value)
			});
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(lst, CmbHSCode, "Id", "HSCode", "HSCode", false);
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbHSCode.Value = id;
				}
				else
				{
					((Control)(object)CmbHSCode).Text = string.Empty;
				}
			}
			((UltraGridBase)CmbHSCode).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLcContractNo_Leave(object sender, EventArgs e)
	{
	}

	private void OrderStatus()
	{
		try
		{
			DataTable dtstatus = new DataTable();
			dtstatus.Columns.Add("Id");
			dtstatus.Columns.Add("Status");
			dtstatus.Rows.Add(1, "Open");
			dtstatus.Rows.Add(2, "Complete");
			dtstatus.Rows.Add(3, "Cancel");
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
			if (Conversion.ToInt(dcmbpaymentterm.Value) != 0)
			{
				string Desc = Conversion.ToString(((UltraDropDownBase)dcmbpaymentterm).SelectedRow.Cells[2].Value);
				if (Desc != null && Desc != string.Empty)
				{
					txtPaymentRemarksDetail.Text = Desc;
				}
			}
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
			if (Conversion.ToInt(cmbdeliverytermnew.Value) != 0)
			{
				string Desc = Conversion.ToString(((UltraDropDownBase)cmbdeliverytermnew).SelectedRow.Cells[2].Value);
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

	private void CmbPaymentTerms_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbPaymentTermsNew.Value) != 0)
			{
				string Desc = Conversion.ToString(((UltraDropDownBase)cmbPaymentTermsNew).SelectedRow.Cells[2].Value);
				if (Desc != null && Desc != string.Empty)
				{
					txtPaymentRemarks.Text = Desc;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom();
			HSCodeBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		try
		{
			string packUom = ((Control)(object)CmbPackUom).Text;
			string RateUom = ((Control)(object)CmbRateUom).Text;
			DataTable dtUOM = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(combitem.Value));
			DDL.BindDDL(dtUOM, CmbPackUom, "Id", "UOMCode", "Pack Uom", ZeroIndex: true);
			DDL.BindDDL(dtUOM, CmbRateUom, "Id", "UOMCode", "Rate Uom", ZeroIndex: true);
			if (packUom != null && packUom != string.Empty)
			{
				if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == packUom))
				{
					((Control)(object)CmbPackUom).Text = packUom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			if (RateUom != null && RateUom != string.Empty)
			{
				if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == RateUom))
				{
					((Control)(object)CmbRateUom).Text = RateUom;
				}
				else
				{
					((Control)(object)CmbRateUom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindConsigneeAgainstCustomer()
	{
		try
		{
			int id = Conversion.ToInt(cmbConsignee.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CompanyName");
			if (Conversion.ToInt(cmbSupCust.Value) > 0)
			{
				DataTable dt2 = CommonServices.GetSupplierustomerByCustomerGroupId(null, Conversion.ToInt(cmbSupCust.Value));
				if (dt2.Rows.Count > 0)
				{
					for (int i = 0; i < dt2.Rows.Count; i++)
					{
						dt.Rows.Add(dt2.Rows[i]["Id"], dt2.Rows[i]["CompanyName"]);
					}
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbConsignee, "Id", "CompanyName", "Consignee", ZeroIndex: true);
				((UltraGridBase)cmbConsignee).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbConsignee.Value = id;
				}
				else
				{
					((Control)(object)cmbConsignee).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindFinancialInstrument()
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbFinInstruments.Value);
			dtFiList.Rows.Clear();
			if (dtFiList.Columns.Count == 0)
			{
				dtFiList.Columns.Add("Id", typeof(int));
				dtFiList.Columns.Add("EFormNo");
				dtFiList.Columns.Add("PaymenttermId");
				dtFiList.Columns.Add("DocumnetTypeId");
			}
			if (Conversion.ToInt(cmbSupCust.Value) > 0)
			{
				DataTable dt = ExImEFormRegistration.GetFINoForInvoice(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Conversion.ToInt(cmbSupCust.Value)
				});
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtFiList.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["EFormNo"], dt.Rows[i]["PaymenttermId"], dt.Rows[i]["DocumentTypeId"]);
					}
					DDL.BindDDL(dtFiList, CmbFinInstruments, "Id", "EFormNo", "FI No", ZeroIndex: true);
					((UltraGridBase)CmbFinInstruments).DisplayLayout.Bands[0].Columns[1].Width = 400;
				}
			}
			if (id > 0)
			{
				if (dtFiList.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbFinInstruments.Value = id;
				}
				else
				{
					((Control)(object)CmbFinInstruments).Text = string.Empty;
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
			if (((UltraGridBase)CmbFinInstruments).ActiveRow != null && Conversion.ToInt(CmbFinInstruments.Value) > 0)
			{
				dcmbpaymentterm.Value = Conversion.ToInt(((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells[2].Value);
				GetFinancialInstrumentsBalance();
			}
			else
			{
				txtBalanceFI.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CreditGlAccountFill()
	{
		try
		{
			RunOnUiThread(delegate
			{
				int id = Conversion.ToInt(CmbCreditAccount.Value);
				DataTable dataTable = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,10");
				DDL.BindDDL(dataTable, CmbCreditAccount, "Id", "AccountTitle", "AccountTitle", ZeroIndex: true);
				if (id > 0)
				{
					if (dataTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbCreditAccount.Value = id;
					}
					else
					{
						((Control)(object)CmbCreditAccount).Text = string.Empty;
					}
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailFormReset()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)CmbFarmingNTrade).Text = string.Empty;
			((Control)(object)combpcktype).Text = string.Empty;
			txtQtyMTon.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtCostMTon.Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtitemdetail.Text = string.Empty;
			txtpackingdetail.Text = string.Empty;
			((Control)(object)CmbHSCode).Text = "";
			btnAddinGrid.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			combitem.ReadOnly = false;
			tabControl2.SelectedIndex = 0;
			((Control)(object)CmbContractSchedule).Text = string.Empty;
			((UltraGridBase)CmbContractSchedule).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormReset()
	{
		AT = new Attachment();
		Attachment.dtAttachments.Rows.Clear();
		LstRemoveRecordDetail.Clear();
		AttachmentsValues = "";
		CustomAttachmentsValues = "";
		btnsave.Visible = true;
		btnupdate.Visible = false;
		UpdateMode = false;
		RecId = 0;
		contractidInupdateCase = 0;
		generateCode();
		if (AutoGenInvoiceNo)
		{
			cmbInvoiceNo.Enabled = false;
			GenerateInvoiceNo();
		}
		else
		{
			cmbInvoiceNo.Enabled = true;
		}
		((UltraGridBase)cmbbranches).Rows[0].Activate();
		((UltraGridBase)cmbproject).Rows[0].Activate();
		((UltraGridBase)cmbcareiertype).Rows[1].Activate();
		txtDocDate.Value = DateTime.Now;
		cmbSupCust.Value = 0;
		cmbSupCust_Leave(null, null);
		cmbConsignee.Value = 0;
		cmbNotifyParty1.Value = 0;
		cmbNotifyParty2.Value = 0;
		cmbPaymentTerm.Value = 0;
		cmbimporterBankNew.Value = 0;
		cmbdeliverytermnew.Value = 0;
		cmbLoadingPort.Value = 0;
		cmbDestinationPort.Value = 0;
		txtcertificate.Text = string.Empty;
		txtcertificateOfOrigion.Text = string.Empty;
		txtSubContractExpiryDays.Text = "0";
		txtLotRef.Text = string.Empty;
		cmbexporterBankNew.Value = 0;
		cmbfcycode.Value = 0;
		txtfcyAmount.Text = string.Empty;
		txtExRate.Text = string.Empty;
		txtLocalAmt.Text = string.Empty;
		txtGrossWeight.Text = string.Empty;
		txtNetWeight.Text = string.Empty;
		txtNoOfContainer.Text = string.Empty;
		txtIformNo.Text = string.Empty;
		txtaddlesscommnets.Text = string.Empty;
		txtaddlessamount.Text = string.Empty;
		txtTotalNetAmount.Text = string.Empty;
		txtCustomerContractNos.Text = string.Empty;
		dtdetail4update.Rows.Clear();
		dtdetail.Rows.Clear();
		grdDetail.ClearStructure();
		dtPaymentTerm.Rows.Clear();
		grdpaymentdetail.ClearStructure();
		dtOtherItem.Rows.Clear();
		grdotheritems.ClearStructure();
		dtCommission.Rows.Clear();
		grdCommissionInfo.DataSource = null;
		grdDetail.DataSource = dtdetail;
		grdDetail.RetrieveStructure();
		grdSettings();
		grdotheritems.DataSource = dtOtherItem;
		grdotheritems.RetrieveStructure();
		grdSettingsOtherItems();
		grdpaymentdetail.DataSource = dtPaymentTerm;
		grdpaymentdetail.RetrieveStructure();
		grdSettingsPaymentTerm();
		grdpaymentdetail.DataSource = dtPaymentTerm;
		grdpaymentdetail.RetrieveStructure();
		grdSettingsPaymentTerm();
		grdCommissionInfo.DataSource = dtCommission;
		grdCommissionInfo.RetrieveStructure();
		grdSettingsCommissionInfo();
		ResetOtherChargesDetail();
		dtOtherChargesDetail.Rows.Clear();
		grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
		grdOtherChargesDetail.RetrieveStructure();
		grdSettingsOtherChargesDetail(grdOtherChargesDetail);
		updateDetailIndexPT = -1;
		updateDetailIndexCommissionInfo = -1;
		((Control)(object)combitem).Text = string.Empty;
		((UltraGridBase)combitem).DataSource = null;
		txtDocDate.Value = DateTime.Now;
		txtIformDate.Value = DateTime.Now;
		txtDocDate.Focus();
		DetailFormReset();
		ResePaymentDetails();
		btnAddCommission.Enabled = true;
		btnUpdateCommision.Enabled = true;
		ResetCommissionDetail();
	}

	private void ResetOtherItems()
	{
		try
		{
			updateDetailIndexOtherItem = -1;
			((Control)(object)CmbOtherItem).Text = string.Empty;
			txtoQty.Text = string.Empty;
			txtoRate.Text = string.Empty;
			txtoAmount.Text = string.Empty;
			txtoRemarks.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResePaymentDetails()
	{
		try
		{
			updateDetailIndexPT = -1;
			dcmbpaymentterm.Value = 0;
			CmbFinInstruments.Value = 0;
			dtxtpercentoftotal.Text = Conversion.ToInt((object)0).ToString();
			dtxtfcyamount.Text = Conversion.ToInt((object)0).ToString();
			dtxtdueday.Text = Conversion.ToInt((object)0).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImCommercialInvoice_FormClosing(object sender, FormClosingEventArgs e)
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
			BindCommercialInvoiceNofromlookup();
			CreditGlAccountFill();
			SupplierCustomerFromContracts();
			CustomerGetAll();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			MultiCurrencyfill();
			ImporterandExportBankFill();
			CarierType();
			BindOtherItems();
			ItemPackingTypeFill();
			ItemSpecificationsBind();
			CropYearFill();
			JobLotFill();
			HSCodeBind();
			OtherChargesBind();
			if (SaleCommissionTabFeatureWise)
			{
				salesPersonFill();
			}
			AutoGenInvoiceNo = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("CommercialInvoicePrefix"));
			if (AutoGenInvoiceNo)
			{
				cmbInvoiceNo.Enabled = false;
				GenerateInvoiceNo();
			}
			else
			{
				cmbInvoiceNo.Enabled = true;
			}
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
				CmbOtherItem.Focus();
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
				CmbOtherItem.Focus();
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
				dcmbpaymentterm.Focus();
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
				dcmbpaymentterm.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "tabControl1")
			{
				combpcktype.Focus();
			}
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
			DetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			MessageBox.Show("Sorry Cannot Add New Record Only Update Detail Record : Thank You", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getTotalnetWeightFromGrid()
	{
		try
		{
			object MTonTotal = grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2);
			double Amount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
			Conversion.ToDouble(MTonTotal);
			txtNetWeight.Text = Conversion.ToDouble(MTonTotal).ToString("#,##0.#####");
			txtfcyAmount.Text = Amount.ToString("#,##0.###");
			PaymentGridBalanceAmountFromInvoiceAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGridUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (!DetailFormValidation())
			{
				return;
			}
			int FarmingNTradeID = 0;
			string FarmingNTrade = "";
			int ExImFarmingTypeId = 0;
			int ExImTradeTypeId = 0;
			if (((UltraGridBase)CmbFarmingNTrade).ActiveRow != null && Conversion.ToInt(CmbFarmingNTrade.Value) > 0)
			{
				FarmingNTradeID = Conversion.ToInt(CmbFarmingNTrade.Value);
				FarmingNTrade = Conversion.ToString(((UltraDropDownBase)CmbFarmingNTrade).SelectedRow.Cells["FarmingNTrade"].Value);
				ExImFarmingTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFarmingNTrade).SelectedRow.Cells["ExImFarmingTypeId"].Value);
				ExImTradeTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFarmingNTrade).SelectedRow.Cells["ExImTradeTypeId"].Value);
			}
			_ = DateTime.Now;
			double ScheduleFCL = 0.0;
			if (Conversion.ToInt(grdDetail.CurrentRow.Cells["ContractScheduleDetailId"].Value) == 0)
			{
				if (((UltraGridBase)CmbContractSchedule).ActiveRow != null && Conversion.ToInt(CmbContractSchedule.Value) > 0)
				{
					Conversion.ToInt(CmbContractSchedule.Value);
					Conversion.ToDateTime((object)((Control)(object)CmbContractSchedule).Text);
					Conversion.ToString(((UltraDropDownBase)CmbContractSchedule).SelectedRow.Cells[3].Value);
					ScheduleFCL = Conversion.ToDouble(((UltraDropDownBase)CmbContractSchedule).SelectedRow.Cells[2].Value);
				}
				dtdetail.Rows[updateDetailIndex]["ContractScheduleId"] = CmbContractSchedule.Value;
				dtdetail.Rows[updateDetailIndex]["ContractSchedule"] = Conversion.ToDateTime((object)((Control)(object)CmbContractSchedule).Text).ToShortDateString();
				dtdetail.Rows[updateDetailIndex]["ContractNoInvoiceWise"] = Conversion.ToString(((UltraDropDownBase)CmbContractSchedule).SelectedRow.Cells[3].Value);
				dtdetail.Rows[updateDetailIndex]["ScheduleFCL"] = ScheduleFCL;
			}
			dtdetail.Rows[updateDetailIndex]["ItemId"] = combitem.Value;
			dtdetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)combitem).Text;
			dtdetail.Rows[updateDetailIndex]["PackTypeId"] = combpcktype.Value;
			dtdetail.Rows[updateDetailIndex]["PackType"] = ((Control)(object)combpcktype).Text;
			dtdetail.Rows[updateDetailIndex]["Qty/M.Ton"] = txtQtyMTon.Text;
			dtdetail.Rows[updateDetailIndex]["PackSizeId"] = CmbPackUom.Value;
			dtdetail.Rows[updateDetailIndex]["PackSize"] = ((Control)(object)CmbPackUom).Text;
			dtdetail.Rows[updateDetailIndex]["CropYearId"] = CmbCropYear.Value;
			dtdetail.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropYear).Text;
			dtdetail.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
			dtdetail.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text;
			dtdetail.Rows[updateDetailIndex]["NoOfBags"] = txtNoOfBags.Text;
			dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)CmbRateUom).Text;
			dtdetail.Rows[updateDetailIndex]["Cost/M.Ton"] = txtCostMTon.Text;
			dtdetail.Rows[updateDetailIndex]["RateUOMId"] = CmbRateUom.Value;
			dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)CmbRateUom).Text;
			dtdetail.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells["Equivalent"].Value);
			dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
			if (Conversion.ToString((object)((Control)(object)CmbHSCode).Text) == "..... Select Any Value .....")
			{
				dtdetail.Rows[updateDetailIndex]["HSCode"] = "";
			}
			else
			{
				dtdetail.Rows[updateDetailIndex]["HSCode"] = Conversion.ToString((object)((Control)(object)CmbHSCode).Text);
			}
			dtdetail.Rows[updateDetailIndex]["ItemDetail"] = txtitemdetail.Text;
			dtdetail.Rows[updateDetailIndex]["PackingDetail"] = txtpackingdetail.Text;
			dtdetail.Rows[updateDetailIndex]["ExImFarmingNTradeId"] = FarmingNTradeID;
			dtdetail.Rows[updateDetailIndex]["FarmingNTrade"] = FarmingNTrade;
			dtdetail.Rows[updateDetailIndex]["ExImFarmingTypeId"] = ExImFarmingTypeId;
			dtdetail.Rows[updateDetailIndex]["ExImTradeTypeId"] = ExImTradeTypeId;
			grdDetail.DataSource = dtdetail;
			getTotalnetWeightFromGrid();
			ProportionateOtherItemAmount();
			btnAddinGrid.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			DetailFormReset();
			CalculateLocalAmount();
			combpcktype.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateNoOfContainersFromSchedule()
	{
		try
		{
			if (grdDetail.GetRows().Count() <= 0)
			{
				return;
			}
			double TotalScheduleFCL = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["ContractScheduleId"].Value) > 0)
				{
					TotalScheduleFCL += Conversion.ToDouble(item.Cells["ScheduleFCL"].Value);
				}
			}
			if (Conversion.ToDouble((object)txtNoOfContainer.Text) == 0.0 && TotalScheduleFCL > 0.0)
			{
				txtNoOfContainer.Text = TotalScheduleFCL.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddinGrid.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			DetailFormReset();
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
			List<string> ExcludeFromSum = new List<string> { "Id", "Cost/M.Ton" };
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ContractId", "ContractScheduleId", "ContractDetailId", "ItemId", "PackTypeId", "PackSizeId", "JobLotId", "RateUOMId", "CropYearId",
				"ContractNo", "ContractDate", "RateEquivalent", "FlagToUpdateScheduleCombo", "ExImTradeTypeId", "ExImFarmingTypeId", "ExImFarmingNTradeId", "ScheduleFCL", "ContractSchedule", "ContractScheduleDetailId"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"CompanyName",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3, hiddenColumns, editableColumns, null, ColumnSetting: true, BracketsForamt: false, ExcludeFromSum);
			grdDetail.RootTable.Columns["ContractRate"].Visible = FlagFin;
			grdDetail.RootTable.Columns["ContractNoInvoiceWise"].Width = Constants.ExportConstants.ContractNoConstant;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants - 55;
			grdDetail.RootTable.Columns["PackType"].Width = Constants.ExportConstants.DocumentStatus + 5;
			grdDetail.RootTable.Columns["CropYear"].Width = Constants.ExportConstants.CropYearConstant - 10;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.ExportConstants.JobLotConstant + 15;
			grdDetail.RootTable.Columns["Qty/M.Ton"].Width = Constants.ExportConstants.QtyConstant;
			grdDetail.RootTable.Columns["PackSize"].Width = Constants.ExportConstants.UomConstant;
			grdDetail.RootTable.Columns["NoOfBags"].Width = Constants.ExportConstants.QtyConstant;
			grdDetail.RootTable.Columns["RateWithoutAddLess"].Width = Constants.ExportConstants.AddLessConstant + 10;
			grdDetail.RootTable.Columns["RateAddLess"].Width = Constants.ExportConstants.AddLessConstant;
			grdDetail.RootTable.Columns["Cost/M.Ton"].Width = Constants.ExportConstants.FclConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.ExportConstants.ExRateConstant - 10;
			grdDetail.RootTable.Columns["AddLessAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdDetail.RootTable.Columns["Amount"].Width = Constants.ExportConstants.AmountConstant;
			grdDetail.RootTable.Columns["HSCode"].Width = Constants.ExportConstants.HsCode;
			grdDetail.RootTable.Columns["ItemDetail"].Width = Constants.ExportConstants.CommodityDetail;
			grdDetail.RootTable.Columns["PackingDetail"].Width = Constants.ExportConstants.CommodityDetail;
			grdDetail.RootTable.Columns["OtherItemAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdDetail.RootTable.Columns["FarmingNTrade"].Width = Constants.ExportConstants.HsCode + 10;
			grdDetail.RootTable.Columns["AddLessAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLessAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLessAmount"].FormatString = "#,##0.####";
			grdDetail.RootTable.Columns["AddLessAmount"].TotalFormatString = "#,##0.####";
			GridEX_Helper.AddButton(grdDetail, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdDetail, "Add", "+", 20, 1);
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
			if (RateAddLessOnCommercialInvoice)
			{
				grdDetail.RootTable.Columns["RateAddLess"].EditType = (EditType)1;
				grdDetail.RootTable.Columns["RateWithoutAddLess"].Visible = true;
				grdDetail.RootTable.Columns["RateAddLess"].Visible = true;
			}
			else
			{
				grdDetail.RootTable.Columns["RateWithoutAddLess"].Visible = false;
				grdDetail.RootTable.Columns["RateAddLess"].Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Reset the Detail First");
				}
				if (grdDetail.GetRows().Count() > 1)
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ExImInvoicePackingDetail vd = new ExImInvoicePackingDetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
						vd.ContractDetailId = Conversion.ToInt(r.Cells["ContractDetailId"].Value);
						vd.ContractNoShipmentWise = Conversion.ToString(r.Cells["ContractNoInvoiceWise"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
						vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd.PackingMaterialTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
						vd.MTon = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
						vd.NetWeight = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
						vd.OuterQtyUomId = Conversion.ToInt(r.Cells["PackSizeId"].Value);
						vd.OuterQty = Conversion.ToDouble(r.Cells["NoOfBags"].Value);
						vd.InnerQtyUomId = Conversion.ToInt(r.Cells["PackSizeId"].Value);
						vd.InnerQty = Conversion.ToInt(r.Cells["NoOfBags"].Value);
						vd.RatePrice = Conversion.ToDouble(r.Cells["Cost/M.Ton"].Value);
						vd.RateUomId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
						vd.FcAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
						vd.ContractScheduleId = Conversion.ToInt(r.Cells["ContractScheduleId"].Value);
						vd.ContractScheduleDetailId = Conversion.ToInt(r.Cells["ContractScheduleDetailId"].Value);
						vd.HsCode = Conversion.ToString((object)r.Cells["HSCode"].Text);
						vd.ItemDescriptionManual = Conversion.ToString((object)r.Cells["ItemDetail"].Text);
						vd.ExImFarmingNTradeId = Conversion.ToInt(r.Cells["ExImFarmingNTradeId"].Value);
						vd.ExImTradeTypeId = Conversion.ToInt(r.Cells["ExImTradeTypeId"].Value);
						vd.ExImFarmingTypeId = Conversion.ToInt(r.Cells["ExImFarmingTypeId"].Value);
						vd.OuterPackDescription = Conversion.ToString((object)r.Cells["PackingDetail"].Text);
						vd.ActionTypeId = 3;
						LstRemoveRecordDetail.Add(vd);
						dtdetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
					else
					{
						r.Delete();
					}
				}
			}
			else if (e.Column.Key == "Add")
			{
				DataRow dr = dtdetail.Rows[r.RowIndex];
				dtdetail.Rows.Add(dr.ItemArray);
				dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
				grdDetail.DataSource = dtdetail;
			}
			grdDetail.UpdateData();
			getTotalnetWeightFromGrid();
			ProportionateOtherItemAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetContractScheduleByContractId(int ContractId, bool flag = false, int ScheduleId = 0, DateTime? LoadingDate = null, double Containers = 0.0, string ScheduleCode = null)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbContractSchedule).Text = string.Empty;
			DataTable dt = ExImLcOrder.GetContractScheduleByContractId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = ContractId,
				ExImInvoiceId = RecId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbContractSchedule, "Id", "AttentiveLoadingDate", "Contract Schedule", ZeroIndex: false);
				((UltraGridBase)CmbContractSchedule).DisplayLayout.Bands[0].Columns[1].Width = 130;
			}
			else
			{
				((UltraGridBase)CmbContractSchedule).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null)
			{
				return;
			}
			updateDetailIndex = item.RowIndex;
			combitem.Value = item.Cells["ItemId"].Value.ToString();
			bindRateUomAndItemPackUom();
			HSCodeBind();
			combpcktype.Value = item.Cells["PackTypeId"].Value;
			CmbPackUom.Value = item.Cells["PackSizeId"].Value;
			CmbCropYear.Value = item.Cells["CropYearId"].Value;
			((Control)(object)CmbCropYear).Text = item.Cells["CropYear"].Text;
			CmbJobLot.Value = item.Cells["JobLotId"].Value;
			txtQtyMTon.Text = item.Cells["Qty/M.Ton"].Value.ToString();
			CmbRateUom.Value = item.Cells["RateUOMId"].Value;
			txtNoOfBags.Text = item.Cells["NoOfBags"].Value.ToString();
			txtCostMTon.Text = item.Cells["Cost/M.Ton"].Value.ToString();
			txtAmount.Text = item.Cells["Amount"].Value.ToString();
			txtitemdetail.Text = item.Cells["ItemDetail"].Value.ToString();
			((Control)(object)CmbHSCode).Text = item.Cells["HSCode"].Value.ToString();
			CmbFarmingNTrade.Value = item.Cells["ExImFarmingNTradeId"].Value;
			txtpackingdetail.Text = item.Cells["PackingDetail"].Value.ToString();
			Conversion.ToBool(item.Cells["FlagToUpdateScheduleCombo"].Value);
			int ContractId = Conversion.ToInt(item.Cells["ContractId"].Value);
			int ContractScheduleId = Conversion.ToInt(item.Cells["ContractScheduleId"].Value);
			int num = Conversion.ToInt(item.Cells["ContractScheduleDetailId"].Value);
			string ScheduleCode = Conversion.ToString(item.Cells["ContractNoInvoiceWise"].Value);
			Conversion.ToDateTime((object)Conversion.ToDateTime(item.Cells["ContractSchedule"].Value).ToShortDateString());
			if (num == 0)
			{
				GetContractScheduleByContractId(ContractId);
				((Control)(object)CmbContractSchedule).Enabled = true;
			}
			else
			{
				((Control)(object)CmbContractSchedule).Enabled = false;
				if (ContractScheduleId > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("ScheduleCode", typeof(string));
					dt.Rows.Add(ContractScheduleId, ScheduleCode);
					DDL.BindDDL(dt, CmbContractSchedule, "Id", "ScheduleCode", "Contract Schedule", ZeroIndex: false);
					CmbContractSchedule.Value = ContractScheduleId;
				}
			}
			btnAddinGrid.Visible = false;
			btnGridUpdate.Visible = true;
			btnCancel.Visible = true;
			if (InvoiceUpdateFlag == 0 || !FlagFin)
			{
				txtCostMTon.ReadOnly = true;
			}
			else
			{
				txtCostMTon.ReadOnly = false;
			}
			combpcktype.Focus();
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
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grdDetail.UpdateData();
				r.BeginEdit();
				double RateAddLess = 0.0;
				double ItemRate = 0.0;
				double ItemRateUom = 0.0;
				double Qty = 0.0;
				Qty = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
				double num = Conversion.ToDouble(r.Cells["RateWithoutAddLess"].Value);
				RateAddLess = Conversion.ToDouble(r.Cells["RateAddLess"].Value);
				ItemRateUom = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
				ItemRate = num + RateAddLess;
				r.Cells["Cost/M.Ton"].Value = ItemRate;
				if (ItemRate > 0.0 && ItemRateUom > 0.0 && Qty > 0.0)
				{
					double weightkgs = Qty * 1000.0;
					double amount = ItemRate / ItemRateUom * weightkgs;
					r.Cells["Amount"].Value = amount;
				}
				r.EndEdit();
				grdDetail.UpdateData();
				getTotalnetWeightFromGrid();
				ProportionateOtherItemAmount();
				CalculateLocalAmount();
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
				dtOtherItem.Rows.Add(CmbOtherItem.Value, ((Control)(object)CmbOtherItem).Text, Conversion.ToDouble((object)txtoQty.Text.Trim()), Conversion.ToString((object)txtoRate.Text), Conversion.ToDouble((object)txtoAmount.Text.Trim()), txtoRemarks.Text.Trim());
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				ResetOtherItems();
				TotalAmountCalculate();
				ProportionateOtherItemAmount();
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
				CmbOtherItem.Value = item.Cells["ItemId"].Value.ToString();
				txtoQty.Text = item.Cells["Qty"].Value.ToString();
				txtoRate.Text = item.Cells["Rate"].Value.ToString();
				txtoAmount.Text = item.Cells["Amount"].Value.ToString();
				txtoRemarks.Text = item.Cells["Remarks"].Value.ToString();
				btnoadd.Visible = false;
				btnoUpdate.Visible = true;
				btnoCancel.Visible = true;
				CmbOtherItem.Focus();
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
				dtOtherItem.Rows[updateDetailIndexOtherItem]["ItemId"] = CmbOtherItem.Value;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["ItemName"] = ((Control)(object)CmbOtherItem).Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Qty"] = txtoQty.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Rate"] = txtoRate.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Amount"] = txtoAmount.Text;
				dtOtherItem.Rows[updateDetailIndexOtherItem]["Remarks"] = txtoRemarks.Text;
				grdotheritems.DataSource = dtOtherItem;
				btnoadd.Visible = true;
				btnoUpdate.Visible = false;
				btnoCancel.Visible = false;
				ResetOtherItems();
				txtoQty.Focus();
				TotalAmountCalculate();
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
		btnoadd.Visible = true;
		btnoUpdate.Visible = false;
		btnoCancel.Visible = false;
		ResetOtherItems();
		updateDetailIndexOtherItem = -1;
	}

	private void grdSettingsOtherItems()
	{
		try
		{
			grdotheritems.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdotheritems.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdotheritems.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
			grdotheritems.RootTable.Columns["ItemName"].Width = 350;
			grdotheritems.RootTable.Columns["Remarks"].Width = 350;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentGridBalanceAmountFromInvoiceAmount()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtTotalNetAmount.Text);
			double PaymentAmount = GridEX_Helper.GetColumnSum(grdpaymentdetail, "FcyAmount");
			txtBalanceInvoiceAmtPaymentGrid.Text = (num - PaymentAmount).ToString(clsGlobalVariables.stringFormatsingleForFcy);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndadd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!PaymentDetailFormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdpaymentdetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(CmbFinInstruments.Value) > 0 && Conversion.ToInt(item.Cells["ExImEFormRegistrationId"].Value) == Conversion.ToInt(CmbFinInstruments.Value) && Conversion.ToInt(item.Cells["DocumentTypeId"].Value) == Conversion.ToInt(((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells[3].Value))
				{
					throw new Exception("Financial Instrument No already add in Grid Please Check");
				}
			}
			dtPaymentTerm.Rows.Add(0, dcmbpaymentterm.Value, ((Control)(object)dcmbpaymentterm).Text, (Conversion.ToInt(CmbFinInstruments.Value) > 0) ? ((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells[3].Value : ((object)0), CmbFinInstruments.Value, (Conversion.ToInt(CmbFinInstruments.Value) > 0) ? ((Control)(object)CmbFinInstruments).Text : "", Conversion.ToDouble((object)dtxtpercentoftotal.Text.Trim()), Conversion.ToDouble((object)dtxtfcyamount.Text.Trim()), Conversion.ToDouble((object)dtxtdueday.Text.Trim()), Conversion.ToString((object)txtPaymentRemarksDetail.Text.Trim()));
			grdpaymentdetail.DataSource = dtPaymentTerm;
			grdpaymentdetail.RetrieveStructure();
			grdSettingsPaymentTerm();
			ResePaymentDetails();
			dcmbpaymentterm.Focus();
			PaymentGridBalanceAmountFromInvoiceAmount();
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
			if (!PaymentDetailFormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdpaymentdetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (((UltraGridBase)CmbFinInstruments).ActiveRow != null && Conversion.ToInt(CmbFinInstruments.Value) > 0 && updateDetailIndexPT != Conversion.ToInt((object)item.RowIndex) && Conversion.ToInt(item.Cells["ExImEFormRegistrationId"].Value) == Conversion.ToInt(CmbFinInstruments.Value) && Conversion.ToInt(item.Cells["DocumentTypeId"].Value) == Conversion.ToInt(((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells["DocumnetTypeId"].Value))
				{
					throw new Exception("Financial Instrument No already add in Grid Please Check");
				}
			}
			dtPaymentTerm.Rows[updateDetailIndexPT]["PaymentTermId"] = dcmbpaymentterm.Value;
			dtPaymentTerm.Rows[updateDetailIndexPT]["PaymentTerm"] = ((Control)(object)dcmbpaymentterm).Text;
			dtPaymentTerm.Rows[updateDetailIndexPT]["DocumentTypeId"] = ((Conversion.ToInt(CmbFinInstruments.Value) > 0) ? ((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells["DocumnetTypeId"].Value : ((object)0));
			dtPaymentTerm.Rows[updateDetailIndexPT]["ExImEFormRegistrationId"] = CmbFinInstruments.Value;
			dtPaymentTerm.Rows[updateDetailIndexPT]["FinancialInstrumentNo"] = ((Conversion.ToInt(CmbFinInstruments.Value) > 0) ? ((Control)(object)CmbFinInstruments).Text : "");
			dtPaymentTerm.Rows[updateDetailIndexPT]["%OfTotal"] = dtxtpercentoftotal.Text.Trim();
			dtPaymentTerm.Rows[updateDetailIndexPT]["FcyAmount"] = dtxtfcyamount.Text.Trim();
			dtPaymentTerm.Rows[updateDetailIndexPT]["DueDays"] = dtxtdueday.Text;
			dtPaymentTerm.Rows[updateDetailIndexPT]["Remarks"] = txtPaymentRemarksDetail.Text;
			grdpaymentdetail.DataSource = dtPaymentTerm;
			btndadd.Visible = true;
			btndupdate.Visible = false;
			btndcancel.Visible = false;
			ResePaymentDetails();
			dcmbpaymentterm.Focus();
			PaymentGridBalanceAmountFromInvoiceAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndcancel_Click(object sender, EventArgs e)
	{
		btndadd.Visible = true;
		btndupdate.Visible = false;
		btndcancel.Visible = false;
		ResePaymentDetails();
	}

	private void grdpaymentdetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdpaymentdetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndexPT = item.RowIndex;
				dcmbpaymentterm.Value = item.Cells["PaymentTermId"].Value.ToString();
				CmbFinInstruments.Value = item.Cells["ExImEFormRegistrationId"].Value.ToString();
				dtxtpercentoftotal.Text = item.Cells["%OfTotal"].Value.ToString();
				dtxtfcyamount.Text = item.Cells["FcyAmount"].Value.ToString();
				dtxtdueday.Text = item.Cells["DueDays"].Value.ToString();
				txtPaymentRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
				btndadd.Visible = false;
				btndupdate.Visible = true;
				btndcancel.Visible = true;
				dcmbpaymentterm.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentTerm()
	{
		try
		{
			grdpaymentdetail.RootTable.Columns["%OfTotal"].AggregateFunction = (AggregateFunction)2;
			grdpaymentdetail.RootTable.Columns["%OfTotal"].TextAlignment = (TextAlignment)3;
			grdpaymentdetail.RootTable.Columns["%OfTotal"].FormatString = "#,#.##";
			grdpaymentdetail.RootTable.Columns["%OfTotal"].TotalFormatString = "#,#.##";
			grdpaymentdetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdpaymentdetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdpaymentdetail.RootTable.Columns["ExImEFormRegistrationId"].Visible = false;
			grdpaymentdetail.RootTable.Columns["PaymentTermId"].Visible = false;
			grdpaymentdetail.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdpaymentdetail.RootTable.Columns["Id"].Visible = false;
			grdpaymentdetail.RootTable.Columns["FinancialInstrumentNo"].Width = 350;
			grdpaymentdetail.RootTable.Columns["PaymentTerm"].Width = 350;
			grdpaymentdetail.RootTable.Columns.Add("Delete");
			grdpaymentdetail.RootTable.Columns["Delete"].Caption = "X";
			grdpaymentdetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdpaymentdetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdpaymentdetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdpaymentdetail.RootTable.Columns["Delete"].Width = 20;
			grdpaymentdetail.RootTable.Columns["Delete"].Position = 0;
			grdpaymentdetail.FrozenColumns = 1;
			ctrlGrdBar4_Load(null, null);
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
			GridEXRow r = grdpaymentdetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				if (updateDetailIndexPT != -1)
				{
					throw new Exception("Reset Detail First...");
				}
				r.Delete();
			}
			PaymentGridBalanceAmountFromInvoiceAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool CommissionDetailFormValidation()
	{
		if (Conversion.ToInt(CmbSalePerson.Value) == 0)
		{
			MessageBox.Show("Sales Person field is required");
			CmbSalePerson.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbCommissionType.Value) == 0)
		{
			MessageBox.Show("Commission Type field is required");
			cmbCommissionType.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommRate.Text) == 0.0)
		{
			MessageBox.Show("Commission Rate Field is Required");
			txtCommRate.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbCommissionType.Value) == 3 && txtCommRateUom.Text == "")
		{
			MessageBox.Show("Commission Rate Uom Field is Required");
			txtCommRateUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommissionAmount.Text.Trim()) == 0.0 || txtCommissionAmount.Text == "")
		{
			MessageBox.Show("Commission Fcy Amount Field is Required");
			txtCommissionAmount.Focus();
			return false;
		}
		return true;
	}

	private void btnAddCommission_Click(object sender, EventArgs e)
	{
		try
		{
			if (!CommissionDetailFormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdCommissionInfo.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(CmbSalePerson.Value) > 0 && Conversion.ToInt(item.Cells["SalesPersonId"].Value) == Conversion.ToInt(CmbSalePerson.Value))
				{
					throw new Exception("Sale person already add in Grid Please select another sale person!");
				}
			}
			dtCommission.Rows.Add(0, CmbSalePerson.Value, ((Control)(object)CmbSalePerson).Text, Conversion.ToInt(cmbCommissionType.Value), ((Control)(object)cmbCommissionType).Text, Conversion.ToDouble((object)txtCommRate.Text.Trim()), txtCommRateUom.Text.Trim(), Conversion.ToDouble((object)txtCommissionAmount.Text.Trim()), Conversion.ToDouble((object)txtCommissionExchangeRate.Text.Trim()), Conversion.ToDouble((object)txtCommissionLcyAmount.Text.Trim()), Conversion.ToString((object)txtCommissionRemarks.Text.Trim()));
			grdCommissionInfo.DataSource = dtCommission;
			grdCommissionInfo.RetrieveStructure();
			grdSettingsCommissionInfo();
			ResetCommissionDetail();
			CmbSalePerson.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void ResetCommissionDetail()
	{
		try
		{
			updateDetailIndexCommissionInfo = -1;
			CmbSalePerson.Value = 0;
			cmbCommissionType.Value = 0;
			txtCommRate.Text = Conversion.ToInt((object)0).ToString();
			txtCommRateUom.Text = Conversion.ToInt((object)0).ToString();
			txtCommRateUom.Tag = "";
			txtCommissionAmount.Text = Conversion.ToInt((object)0).ToString();
			txtCommissionLcyAmount.Text = Conversion.ToInt((object)0).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdateCommision_Click(object sender, EventArgs e)
	{
		try
		{
			if (!CommissionDetailFormValidation())
			{
				return;
			}
			GridEXRow[] rows = grdCommissionInfo.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (((UltraGridBase)CmbSalePerson).ActiveRow != null && Conversion.ToInt(CmbSalePerson.Value) > 0 && updateDetailIndexCommissionInfo != Conversion.ToInt((object)item.RowIndex) && Conversion.ToInt(item.Cells["SalesPersonId"].Value) == Conversion.ToInt(CmbSalePerson.Value))
				{
					throw new Exception("Sales person already add in Grid Please selected another SalesPerson!");
				}
			}
			dtCommission.Rows[updateDetailIndexCommissionInfo]["SalesPersonId"] = Conversion.ToInt(CmbSalePerson.Value);
			dtCommission.Rows[updateDetailIndexCommissionInfo]["SalesPerson"] = ((Control)(object)CmbSalePerson).Text;
			dtCommission.Rows[updateDetailIndexCommissionInfo]["CommissionTypeId"] = Conversion.ToInt(cmbCommissionType.Value);
			dtCommission.Rows[updateDetailIndexCommissionInfo]["CommissionType"] = ((Control)(object)cmbCommissionType).Text;
			dtCommission.Rows[updateDetailIndexCommissionInfo]["Rate"] = Conversion.ToDouble((object)txtCommRate.Text);
			dtCommission.Rows[updateDetailIndexCommissionInfo]["RateUom"] = txtCommRateUom.Text.Trim();
			dtCommission.Rows[updateDetailIndexCommissionInfo]["FcyAmount"] = txtCommissionAmount.Text.Trim();
			dtCommission.Rows[updateDetailIndexCommissionInfo]["ExchangeRate"] = Conversion.ToDouble((object)txtCommissionExchangeRate.Text.Trim());
			dtCommission.Rows[updateDetailIndexCommissionInfo]["LcyAmount"] = txtCommissionLcyAmount.Text.Trim();
			dtCommission.Rows[updateDetailIndexCommissionInfo]["Remarks"] = txtCommissionRemarks.Text;
			grdCommissionInfo.DataSource = dtCommission;
			btnAddCommission.Visible = true;
			btnUpdateCommision.Visible = false;
			btnCancelCommission.Visible = false;
			ResetCommissionDetail();
			CmbSalePerson.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelCommission_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddCommission.Visible = true;
			btnUpdateCommision.Visible = false;
			btnCancelCommission.Visible = false;
			ResetCommissionDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsCommissionInfo()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SalesPersonId", "CommissionTypeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdCommissionInfo, 2, 3, hiddenColumns);
			grdCommissionInfo.RootTable.Columns["SalesPerson"].Width = Constants.ExportConstants.PartyName + 20;
			grdCommissionInfo.RootTable.Columns["CommissionType"].Width = 150;
			grdCommissionInfo.RootTable.Columns["Rate"].Width = Constants.ExportConstants.ExRateConstant + 10;
			grdCommissionInfo.RootTable.Columns["RateUom"].Width = Constants.ExportConstants.UomConstant + 20;
			grdCommissionInfo.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdCommissionInfo.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant + 20;
			grdCommissionInfo.RootTable.Columns["LcyAmount"].Width = Constants.ExportConstants.LcyAmountConstant;
			grdCommissionInfo.RootTable.Columns["Remarks"].Width = Constants.ExportConstants.RemarksConstants;
			grdCommissionInfo.RootTable.Columns.Add("Delete");
			grdCommissionInfo.RootTable.Columns["Delete"].Caption = "X";
			grdCommissionInfo.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdCommissionInfo.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdCommissionInfo.RootTable.Columns["Delete"].ButtonText = "X";
			grdCommissionInfo.RootTable.Columns["Delete"].Width = 20;
			grdCommissionInfo.RootTable.Columns["Delete"].Position = 0;
			grdCommissionInfo.FrozenColumns = 1;
			((Control)(object)grdCommissionInfo).Refresh();
			ctrlGrdBar5_Load(null, null);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdCommissionInfo, dtCommission);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdCommissionInfo_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdCommissionInfo.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				if (updateDetailIndexCommissionInfo != -1)
				{
					throw new Exception("Reset Detail First...");
				}
				if (btnAddCommission.Enabled)
				{
					r.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCommissionInfo_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdCommissionInfo.CurrentRow;
			if (item != null)
			{
				updateDetailIndexCommissionInfo = item.RowIndex;
				CmbSalePerson.Value = Conversion.ToInt(item.Cells["SalesPersonId"].Value);
				cmbCommissionType.Value = Conversion.ToInt(item.Cells["CommissionTypeId"].Value);
				txtCommRate.Text = Conversion.ToDouble(item.Cells["Rate"].Value).ToString();
				txtCommRateUom.Text = item.Cells["RateUom"].Value.ToString();
				txtCommissionAmount.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString("#,##0.###");
				txtCommissionExchangeRate.Text = Conversion.ToDouble(item.Cells["ExchangeRate"].Value).ToString("#,##0.###");
				txtCommissionLcyAmount.Text = Conversion.ToDouble(item.Cells["LcyAmount"].Value).ToString("#,##0.###");
				txtCommissionRemarks.Text = item.Cells["Remarks"].Value.ToString();
				btnAddCommission.Visible = false;
				btnUpdateCommision.Visible = true;
				btnCancelCommission.Visible = true;
				CmbSalePerson.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateCommissionAmount()
	{
		try
		{
			if (Conversion.ToInt(cmbCommissionType.Value) > 0 && Conversion.ToDouble((object)txtCommRate.Text.Trim()) > 0.0 && Conversion.ToDouble((object)txtfcyAmount.Text.Trim()) > 0.0)
			{
				double GridAmount = 0.0;
				double GridRate = 0.0;
				double CommRate = Conversion.ToDouble((object)txtCommRate.Text.Trim());
				double FcyAmount = Conversion.ToDouble((object)txtfcyAmount.Text.Trim());
				double NetWeightKgs = Conversion.ToDouble((object)txtNetWeight.Text.Trim()) * 1000.0;
				double CommAmt = 0.0;
				GridEXRow[] rows = grdCommissionInfo.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (updateDetailIndexCommissionInfo != item.RowIndex)
					{
						GridAmount += Conversion.ToDouble(item.Cells["FcyAmount"].Value);
						GridRate += Conversion.ToDouble(item.Cells["Rate"].Value);
					}
				}
				if (Conversion.ToInt(cmbCommissionType.Value) == 1)
				{
					if (GridAmount + CommRate > FcyAmount)
					{
						double rem = FcyAmount - GridAmount;
						CommRate = ((rem >= 0.0) ? rem : 0.0);
					}
					CommAmt = CommRate;
				}
				else if (Conversion.ToInt(cmbCommissionType.Value) == 2)
				{
					if (CommRate + GridRate > 99.0)
					{
						CommAmt = FcyAmount - GridAmount;
						CommRate = CommAmt * 100.0 / FcyAmount;
						txtCommRate.Text = CommRate.ToString("#,##0.####");
					}
					CommAmt = FcyAmount * CommRate / 100.0;
					if (CommAmt + GridAmount > FcyAmount)
					{
						double rem2 = FcyAmount - GridAmount;
						CommAmt = ((!(rem2 > 0.0)) ? 0.0 : (rem2 * CommRate / 100.0));
					}
				}
				else if (Conversion.ToInt(cmbCommissionType.Value) == 3)
				{
					if (NetWeightKgs > 0.0)
					{
						double UOM = Conversion.ToDouble((object)txtCommRateUom.Text.Trim());
						CommAmt = NetWeightKgs / UOM * CommRate;
						if (CommAmt + GridAmount > FcyAmount)
						{
							double rem3 = FcyAmount - GridAmount;
							if (rem3 > 0.0)
							{
								CommAmt = rem3;
								CommRate = CommAmt * 100.0 / FcyAmount;
								txtCommRate.Text = CommRate.ToString("#,##0.####");
								CommAmt = NetWeightKgs / UOM * CommRate;
							}
							else
							{
								CommAmt = 0.0;
							}
						}
					}
					else
					{
						CommAmt = 0.0;
					}
				}
				txtCommissionAmount.Text = CommAmt.ToString("#,##0.####");
				double ExchangeRate = Conversion.ToDouble((object)txtCommissionExchangeRate.Text);
				double LcyAmount = ((ExchangeRate > 0.0) ? (ExchangeRate * CommAmt) : 0.0);
				txtCommissionLcyAmount.Text = LcyAmount.ToString("#,##0.####");
			}
			else
			{
				txtCommissionAmount.Text = "0";
				txtCommissionLcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbCommissionType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(cmbCommissionType.Value);
			if (Id > 0 && Id == 1)
			{
				txtCommRateUom.Text = "Lump Sum";
				txtCommRateUom.Enabled = false;
			}
			else if (Id > 0 && Id == 2)
			{
				txtCommRateUom.Text = "%";
				txtCommRateUom.Enabled = false;
			}
			else if (Id > 0 && Id == 3)
			{
				if (txtCommRateUom.Tag == "")
				{
					txtCommRateUom.Text = "1000";
				}
				txtCommRateUom.Enabled = true;
			}
			else
			{
				txtCommRateUom.Text = "";
				txtCommRateUom.Enabled = true;
			}
			CalculateCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCommRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommRateUom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			txtCommRateUom.Tag = txtCommRateUom.Text;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommissionAmount_TextChanged(object sender, EventArgs e)
	{
	}

	private void RecalculateCommissionGridLocalAmount()
	{
		try
		{
			GridEXRow[] rows = grdCommissionInfo.GetRows();
			foreach (GridEXRow item in rows)
			{
				double fcyAmount = Conversion.ToDouble(item.Cells["FcyAmount"].Value);
				double ExchangeRate = Conversion.ToDouble((object)txtExRate.Text);
				item.BeginEdit();
				if (fcyAmount > 0.0 && ExchangeRate > 0.0)
				{
					double LocalAmount = fcyAmount * ExchangeRate;
					item.Cells["ExchangeRate"].Value = ExchangeRate;
					item.Cells["LcyAmount"].Value = LocalAmount;
				}
				else
				{
					item.Cells["ExchangeRate"].Value = 0;
					item.Cells["LcyAmount"].Value = 0;
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlgrdOtherChargesDetail_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdOtherChargesDetail, dtOtherChargesDetail);
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

	private bool formvalidation()
	{
		if (AllowExportMultiCompanies && (((UltraGridBase)CmbExportCompany).ActiveRow == null || Conversion.ToInt(CmbExportCompany.Value) == 0))
		{
			MessageBox.Show("Export Company Is required");
			CmbExportCompany.Focus();
			return false;
		}
		if (txtdocno.Text == "" || Conversion.ToInt((object)txtdocno.Text) == 0)
		{
			MessageBox.Show("Doc No Is Required");
			txtdocno.Focus();
			return false;
		}
		if (Conversion.ToString((object)cmbInvoiceNo.Text) == string.Empty || txtdocno.Text == "")
		{
			MessageBox.Show("Invoice No Is Required");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbSupCust.Value) == 0)
		{
			MessageBox.Show("Customer Is Required");
			cmbSupCust.Focus();
			return false;
		}
		if (FlagFin && Conversion.ToInt(CmbCreditAccount.Value) == 0 && MessageBox.Show("Are you sure to Credit Value charge to Sale Account?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
		{
			MessageBox.Show("Please select CreditAc");
			CmbCreditAccount.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbdeliverytermnew.Value) == 0)
		{
			MessageBox.Show("Delivery Term Is Required");
			cmbdeliverytermnew.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbLoadingPort.Value) == 0)
		{
			MessageBox.Show("Loading Port Is Required");
			cmbLoadingPort.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbDestinationPort.Value) == 0)
		{
			MessageBox.Show("Destination Port  Is Required");
			cmbDestinationPort.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbcareiertype.Value) == 0)
		{
			MessageBox.Show("Carrier Type  Is Required");
			cmbcareiertype.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbfcycode.Value) == 0)
		{
			MessageBox.Show("Fcy Code Is Required");
			cmbfcycode.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfcyAmount.Text) == 0.0)
		{
			MessageBox.Show("Fcy Amount  Is Required");
			txtfcyAmount.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtNoOfContainer.Text) == 0)
		{
			MessageBox.Show("No.of Containers Field Is Required");
			txtNoOfContainer.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtNetWeight.Text) > 0.0))
		{
			MessageBox.Show("Net Weight  Is Required");
			txtNetWeight.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtGrossWeight.Text) >= Conversion.ToDouble((object)txtNetWeight.Text)))
		{
			MessageBox.Show("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
			txtGrossWeight.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtExRate.Text) > 0.0))
		{
			MessageBox.Show("ExchangeRate Field is Required");
			txtExRate.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtTotalNetAmount.Text) > 0.0))
		{
			MessageBox.Show("Total Amount  Is Required");
			txtTotalNetAmount.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		if (Conversion.ToInt(combitem.Value) == 0)
		{
			MessageBox.Show("Item field required");
			combitem.Focus();
			return false;
		}
		if (((UltraGridBase)combpcktype).ActiveRow == null || Conversion.ToInt(combpcktype.Value) == 0)
		{
			MessageBox.Show("Pack Type field required");
			combpcktype.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year field required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot field required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack uom field required");
			CmbPackUom.Focus();
			return false;
		}
		if (txtQtyMTon.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtQtyMTon.Text) > 0.0))
		{
			MessageBox.Show("Qty/M.Ton field required");
			txtQtyMTon.Focus();
			return false;
		}
		if (txtNoOfBags.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtNoOfBags.Text.Trim()) > 0.0))
		{
			MessageBox.Show("No of Bags field required");
			txtNoOfBags.Focus();
			return false;
		}
		if (txtCostMTon.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtCostMTon.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Cost/M.Ton field required");
			txtCostMTon.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
		{
			MessageBox.Show("Rate UOM field required");
			CmbRateUom.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtAmount.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Amount field required");
			txtAmount.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbContractSchedule.Value) <= 0)
		{
			MessageBox.Show("Contract Schedule field required");
			CmbContractSchedule.Focus();
			return false;
		}
		if (txtTotalNetAmount.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Total Net Amount  field required");
			txtTotalNetAmount.Focus();
			return false;
		}
		return true;
	}

	private bool OtherItemFormValidation()
	{
		if (Conversion.ToInt(CmbOtherItem.Value) == 0)
		{
			MessageBox.Show("Item field required");
			CmbOtherItem.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtoRate.Focus();
			return false;
		}
		return true;
	}

	private bool PaymentDetailFormValidation()
	{
		if (Conversion.ToInt(dcmbpaymentterm.Value) == 0)
		{
			MessageBox.Show("Payment Term field required");
			dcmbpaymentterm.Focus();
			return false;
		}
		if (Conversion.ToInt(dcmbpaymentterm.Value) == 1 && (((UltraGridBase)CmbFinInstruments).ActiveRow == null || Conversion.ToInt(CmbFinInstruments.Value) == 0))
		{
			throw new Exception("Financial Instrument field is required against Advance");
		}
		if (Conversion.ToDouble((object)dtxtpercentoftotal.Text.Trim()) == 0.0 || dtxtpercentoftotal.Text == "")
		{
			MessageBox.Show("Percent Field Required");
			dtxtpercentoftotal.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)dtxtfcyamount.Text.Trim()) == 0.0 || dtxtfcyamount.Text == "")
		{
			MessageBox.Show("Fcy Amount  Field Required");
			dtxtfcyamount.Focus();
			return false;
		}
		return true;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_0fb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb7: Expected O, but got Unknown
		//IL_0e9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea5: Expected O, but got Unknown
		//IL_12cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d3: Expected O, but got Unknown
		//IL_1188: Unknown result type (might be due to invalid IL or missing references)
		//IL_118f: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() <= 0)
			{
				throw new Exception("Grid Record not found");
			}
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			object MTonTotal = grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2);
			txtNetWeight.Text = Conversion.ToDouble(MTonTotal).ToString("#,##0.###");
			if (Conversion.ToDouble((object)txtaddlessamount.Text) > 0.0 && grdOtherChargesDetail.RowCount == 0 && MessageBox.Show("Are you sure to proceed because add less amount not zero but Charges grid don't have any record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				tabControl2.SelectedIndex = 4;
				CmbChargesNameChargeDetail.Focus();
				return;
			}
			ExImInvoice PIH = new ExImInvoice();
			if (!formvalidation())
			{
				return;
			}
			if (RecId > 0)
			{
				PIH.Id = RecId;
			}
			PIH.OrganizationId = UserAccount.OrganizationId;
			PIH.CompanyId = UserAccount.CompanyId;
			PIH.BranchesId = UserAccount.BranchesId;
			PIH.IsApproved = false;
			PIH.EntryDate = DateTime.Now;
			PIH.EntryUser = UserAccount.ID;
			PIH.ModifyDate = DateTime.Now;
			PIH.ModifyUser = UserAccount.ID;
			PIH.ApprovedDate = DateTime.Now;
			PIH.DocumentTypeId = 204;
			PIH.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PIH.ProjectsId = Conversion.ToInt(cmbproject.Value);
			PIH.DocCode = Conversion.ToInt((object)txtdocno.Text);
			PIH.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			PIH.InvoiceNo = Conversion.ToString((object)cmbInvoiceNo.Text.Trim());
			PIH.SupplierCustomerId = Conversion.ToInt(cmbSupCust.Value);
			PIH.ConsigneeId = Conversion.ToInt(cmbConsignee.Value);
			PIH.NotifyParty1 = Conversion.ToInt(cmbNotifyParty1.Value);
			PIH.NotifyParty2 = Conversion.ToInt(cmbNotifyParty2.Value);
			PIH.LotNoRef = Conversion.ToString((object)txtLotRef.Text);
			PIH.EFormNo = Conversion.ToString((object)txtIformNo.Text.Trim());
			PIH.EFormDate = Conversion.ToDateTime((object)txtIformDate.Value);
			PIH.FcurrencyId = Conversion.ToInt(cmbfcycode.Value);
			PIH.ConversionRate = Conversion.ToDouble((object)txtExRate.Text.Trim());
			PIH.FCurrencyAmount = Conversion.ToDouble((object)txtfcyAmount.Text.Trim());
			PIH.EquivalentAmount = Conversion.ToDouble((object)txtLocalAmt.Text.Trim());
			PIH.AddLessAmount = Conversion.ToDouble((object)txtaddlessamount.Text.Trim());
			PIH.TotalAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim());
			PIH.AddLessComments = Conversion.ToString((object)txtaddlesscommnets.Text);
			PIH.DeliveryTermId = Conversion.ToInt(cmbdeliverytermnew.Value);
			PIH.PaymentRemarks = Conversion.ToString((object)txtPaymentRemarks.Text);
			PIH.DeliveryRemarks = Conversion.ToString((object)txtDeliveryRemarks.Text);
			PIH.ImporterBankId = Conversion.ToInt(cmbimporterBankNew.Value);
			PIH.ExporteBankId = Conversion.ToInt(cmbexporterBankNew.Value);
			PIH.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			PIH.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			PIH.CarierType = Conversion.ToString((object)((Control)(object)cmbcareiertype).Text);
			PIH.GrossMton = Conversion.ToDecimal((object)txtGrossWeight.Text.Trim());
			PIH.GrossWeight = Conversion.ToDouble((object)(PIH.GrossMton * 1000m));
			PIH.ExportCompanyId = Conversion.ToInt(CmbExportCompany.Value);
			PIH.NoOfContainers = Conversion.ToInt((object)txtNoOfContainer.Text.Trim());
			PIH.Certificate1 = Conversion.ToString((object)txtcertificate.Text.Trim());
			PIH.Certificate2 = Conversion.ToString((object)txtcertificateOfOrigion.Text.Trim());
			PIH.CreditAccountId = Conversion.ToInt(CmbCreditAccount.Value);
			PIH.CustomerContractNos = Conversion.ToString((object)txtCustomerContractNos.Text);
			PIH.SubContractExpiryDays = Conversion.ToInt((object)txtSubContractExpiryDays.Text);
			PIH.ExImInvoiceDetail = new List<ExImInvoicePackingDetail>();
			PIH.ExImInvoiceOtherItemslist = new List<ExImInvoiceOtherItems>();
			PIH.ExImInvoicePaymentTermsDetail = new List<ExImInvoicePaymentTermsDetail>();
			PIH.ExImInvoiceSaleManCommissionList = new List<ExImInvoiceSaleManCommission>();
			PIH.ExImInvoiceOtherChargesDetailList = new List<ExImInvoiceOtherChargesDetail>();
			double TotalFcAmount = 0.0;
			if (RecId > 0 && LstRemoveRecordDetail.Count > 0)
			{
				for (int i = 0; i < LstRemoveRecordDetail.Count; i++)
				{
					PIH.ExImInvoiceDetail.Add(LstRemoveRecordDetail[i]);
				}
			}
			double MtonPerContainer = Conversion.ToDouble((object)txtNetWeight.Text) / Conversion.ToDouble((object)txtNoOfContainer.Text);
			string ContractIds = "";
			string ContractDetailIds = "";
			string ContractNos = "";
			string ContractDates = "";
			string ContractScheduleIds = "";
			string ContractScheduleNos = "";
			if (grdDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r5 in rows)
				{
					ExImInvoicePackingDetail vd3 = new ExImInvoicePackingDetail();
					vd3.Id = Conversion.ToInt(r5.Cells["Id"].Value);
					vd3.ContractId = Conversion.ToInt(r5.Cells["ContractId"].Value);
					if (vd3.ContractId == 0)
					{
						throw new Exception("ContractId required in Row#" + (r5.RowIndex + 1));
					}
					vd3.ContractDetailId = Conversion.ToInt(r5.Cells["ContractDetailId"].Value);
					ContractIds = ContractIds + "," + Conversion.ToString(r5.Cells["ContractId"].Value);
					ContractDetailIds = ContractDetailIds + "," + Conversion.ToString(r5.Cells["ContractDetailId"].Value);
					ContractNos = ContractNos + "," + Conversion.ToString(r5.Cells["ContractNo"].Value);
					ContractDates = ContractDates + "," + Conversion.ToDateTime(r5.Cells["ContractDate"].Value).ToShortDateString();
					ContractScheduleIds = ContractScheduleIds + "," + Conversion.ToString(r5.Cells["ContractScheduleId"].Value);
					ContractScheduleNos = ContractScheduleNos + "," + Conversion.ToString(r5.Cells["ContractNoInvoiceWise"].Value);
					if (vd3.ContractDetailId == 0)
					{
						throw new Exception("ContractDetailId required in Row#" + (r5.RowIndex + 1));
					}
					vd3.ContractNoShipmentWise = Conversion.ToString(r5.Cells["ContractNoInvoiceWise"].Value);
					vd3.ContractScheduleDetailId = Conversion.ToInt(r5.Cells["ContractScheduleDetailId"].Value);
					vd3.ItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
					vd3.CropYearId = Conversion.ToInt(r5.Cells["CropYearId"].Value);
					vd3.JobLotId = Conversion.ToInt(r5.Cells["JobLotId"].Value);
					vd3.PackingMaterialTypeId = Conversion.ToInt(r5.Cells["PackTypeId"].Value);
					vd3.MTon = Conversion.ToDouble(r5.Cells["Qty/M.Ton"].Value);
					vd3.NetWeight = vd3.MTon * 1000.0;
					vd3.OuterQtyUomId = Conversion.ToInt(r5.Cells["PackSizeId"].Value);
					vd3.OuterQty = Conversion.ToDouble(r5.Cells["NoOfBags"].Value);
					vd3.InnerQtyUomId = Conversion.ToInt(r5.Cells["PackSizeId"].Value);
					vd3.InnerQty = Conversion.ToInt(r5.Cells["NoOfBags"].Value);
					vd3.RateWithoutAddLess = Conversion.ToDouble(r5.Cells["RateWithoutAddLess"].Value);
					vd3.RateAddLess = Conversion.ToDouble(r5.Cells["RateAddLess"].Value);
					vd3.RatePrice = Conversion.ToDouble(r5.Cells["Cost/M.Ton"].Value);
					vd3.RateUomId = Conversion.ToInt(r5.Cells["RateUOMId"].Value);
					vd3.AddLessAmount = Conversion.ToDouble(r5.Cells["AddLessAmount"].Value);
					vd3.FcAmount = Conversion.ToDouble(r5.Cells["Amount"].Value);
					vd3.OtherAmount = Conversion.ToDouble(r5.Cells["OtherItemAmount"].Value);
					vd3.ContractScheduleId = Conversion.ToInt(r5.Cells["ContractScheduleId"].Value);
					if (vd3.ContractScheduleId == 0)
					{
						throw new Exception("Contract Schedule required in Row#" + (r5.RowIndex + 1));
					}
					TotalFcAmount += vd3.FcAmount;
					vd3.HsCode = Conversion.ToString((object)r5.Cells["HSCode"].Text);
					vd3.ItemDescriptionManual = Conversion.ToString((object)r5.Cells["ItemDetail"].Text);
					vd3.OuterPackDescription = Conversion.ToString((object)r5.Cells["PackingDetail"].Text);
					vd3.ExImFarmingNTradeId = Conversion.ToInt(r5.Cells["ExImFarmingNTradeId"].Value);
					vd3.ExImTradeTypeId = Conversion.ToInt(r5.Cells["ExImTradeTypeId"].Value);
					vd3.ExImFarmingTypeId = Conversion.ToInt(r5.Cells["ExImFarmingTypeId"].Value);
					vd3.ActionTypeId = ((vd3.Id <= 0) ? 1 : 2);
					PIH.NetWeight += vd3.NetWeight;
					PIH.NetMton += Conversion.ToDecimal((object)vd3.MTon);
					vd3.NoofContainers = vd3.MTon / MtonPerContainer;
					PIH.ExImInvoiceDetail.Add(vd3);
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
				double OtherAmount = 0.0;
				double UtilizeAmount = 0.0;
				if (grdotheritems.RowCount > 0)
				{
					rows = grdotheritems.GetRows();
					foreach (GridEXRow r4 in rows)
					{
						if (Conversion.ToDecimal(r4.Cells["Qty"].Value) > 0m)
						{
							ExImInvoiceOtherItems vd2 = new ExImInvoiceOtherItems();
							vd2.otherItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
							vd2.oItemQty = Conversion.ToDecimal(r4.Cells["Qty"].Value);
							vd2.oItemRate = Conversion.ToDouble(r4.Cells["Rate"].Value);
							vd2.oItemAmount = Conversion.ToDecimal(r4.Cells["Amount"].Value);
							OtherAmount += Conversion.ToDouble((object)vd2.oItemAmount);
							vd2.OtherItemRemarks = Conversion.ToString(r4.Cells["Remarks"].Value);
							PIH.ExImInvoiceOtherItemslist.Add(vd2);
						}
					}
				}
				if (grdpaymentdetail.RowCount > 0)
				{
					rows = grdpaymentdetail.GetRows();
					foreach (GridEXRow r3 in rows)
					{
						ExImInvoicePaymentTermsDetail vdp2 = new ExImInvoicePaymentTermsDetail();
						vdp2.DocumentTypeId = Conversion.ToInt(r3.Cells["DocumentTypeId"].Value);
						vdp2.PaymentTermId = Conversion.ToInt(r3.Cells["PaymentTermId"].Value);
						vdp2.ExImEFormRegistrationId = Conversion.ToInt(r3.Cells["ExImEFormRegistrationId"].Value);
						if (vdp2.PaymentTermId == 1 && vdp2.ExImEFormRegistrationId == 0)
						{
							throw new Exception("Financial Instrument number field is required against advance in payment term detail...");
						}
						vdp2.FinancialInstrumentNo = Conversion.ToString((object)r3.Cells["FinancialInstrumentNo"].Text);
						vdp2.PrcntOfTotal = Conversion.ToDouble(r3.Cells["%OfTotal"].Value);
						vdp2.DueDays = Conversion.ToInt(r3.Cells["DueDays"].Value);
						vdp2.FcyAmount = Conversion.ToDouble(r3.Cells["FcyAmount"].Value);
						vdp2.PaymentRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
						UtilizeAmount += vdp2.FcyAmount;
						vdp2.SortNo = r3.RowIndex + 1;
						PIH.ExImInvoicePaymentTermsDetail.Add(vdp2);
					}
					PIH.PaymentTermId = PIH.ExImInvoicePaymentTermsDetail[0].PaymentTermId;
					if (grdCommissionInfo.RowCount > 0)
					{
						rows = grdCommissionInfo.GetRows();
						foreach (GridEXRow r2 in rows)
						{
							ExImInvoiceSaleManCommission vdp = new ExImInvoiceSaleManCommission();
							vdp.ActionTypeId = 1;
							vdp.SalesPersonId = Conversion.ToInt(r2.Cells["SalesPersonId"].Value);
							vdp.commissionTypeId = Conversion.ToInt(r2.Cells["CommissionTypeId"].Value);
							vdp.commissionRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
							vdp.rateUom = Conversion.ToString((object)r2.Cells["RateUom"].Text);
							vdp.fcyAmount = Conversion.ToDouble(r2.Cells["FcyAmount"].Value);
							vdp.exchangeRate = PIH.ConversionRate;
							vdp.lcyAmount = Conversion.ToDouble(r2.Cells["LcyAmount"].Value);
							vdp.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
							PIH.ExImInvoiceSaleManCommissionList.Add(vdp);
						}
					}
					rows = grdOtherChargesDetail.GetRows();
					foreach (GridEXRow r in rows)
					{
						ExImInvoiceOtherChargesDetail vd = new ExImInvoiceOtherChargesDetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
						vd.ContractOtherChargesDetailId = Conversion.ToInt(r.Cells["ContractOtherChargesDetailId"].Value);
						vd.ChargesItemId = Conversion.ToInt(r.Cells["ChargesItemId"].Value);
						vd.AddAmount = Conversion.ToDouble(r.Cells["AddAmount"].Value);
						vd.LessAmount = Conversion.ToDouble(r.Cells["LessAmount"].Value);
						vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						FormHelper.ValidateField(vd.ChargesItemId, "Charges Item", r.RowIndex, "Other Charges");
						if (vd.AddAmount == 0.0 && vd.LessAmount == 0.0)
						{
							throw new Exception($"AddAmount Or Less Amount is required in Other Charges at row No: {r.RowIndex + 1}");
						}
						PIH.ExImInvoiceOtherChargesDetailList.Add(vd);
					}
					PIH.TotalAmount = TotalFcAmount + PIH.AddLessAmount + OtherAmount;
					if (grdpaymentdetail.GetRows().Count() > 0 && Math.Abs(Math.Round(PIH.TotalAmount, 0) - Math.Round(UtilizeAmount, 0)) >= 1.0)
					{
						throw new Exception("Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check");
					}
					PIH.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
					PIH.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
					FormHelper.UpdateAttachmentsForObject<ExImInvoice>(PIH, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
					PIH.AttachmentsValues = ((AttachmentsValues != "") ? AttachmentsValues : PIH.AttachmentsValues);
					PIH.CustomAttachmentsValues = CustomAttachmentsValues;
					int success = ExImInvoice.Save(PIH);
					if (RecId == 0)
					{
						MessageBox.Show("Save SuccessFully");
					}
					if (RecId > 0)
					{
						MessageBox.Show("Update SuccessFully");
						FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, PIH.DocumentTypeId, base.Name);
					}
					FormReset();
					if (ChkPrintPreview.Checked)
					{
						InvoiceSlip521(success);
					}
					return;
				}
				tabControl2.SelectedIndex = 2;
				dcmbpaymentterm.Focus();
				throw new Exception("Payment Detail Not Found. Please Check! ");
			}
			tabControl2.SelectedIndex = 0;
			throw new Exception("Detail Not Found. Please Check! ");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			btnsave_Click(null, null);
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

	public void ReadById(int Id)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		try
		{
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("Name");
			DataTable dtOItem = new DataTable();
			dtOItem.Columns.Add("Id", typeof(int));
			dtOItem.Columns.Add("Name");
			dtOItem.Columns.Add("Rate", typeof(double));
			LstRemoveRecordDetail.Clear();
			btnsave.Visible = false;
			btnupdate.Visible = true;
			RecId = Id;
			ExImInvoice PIH = new ExImInvoice();
			PIH = ExImInvoice.GetByID(RecId);
			if (PIH != null)
			{
				tabControl1.SelectedIndex = 0;
				cmbbranches.Value = PIH.BranchesId;
				cmbproject.Value = PIH.ProjectsId;
				txtdocno.Text = PIH.DocCode.ToString();
				txtDocDate.Value = Conversion.ToDateTime((object)PIH.DocDate);
				cmbInvoiceNo.Text = PIH.InvoiceNo.ToString();
				cmbSupCust.Value = PIH.SupplierCustomerId;
				cmbSupCust_Leave(null, null);
				cmbConsignee.Value = PIH.ConsigneeId;
				cmbNotifyParty1.Value = PIH.NotifyParty1;
				cmbNotifyParty2.Value = PIH.NotifyParty2;
				txtLotRef.Text = PIH.LotNoRef;
				cmbimporterBankNew.Value = PIH.ImporterBankId;
				cmbexporterBankNew.Value = PIH.ExporteBankId;
				cmbdeliverytermnew.Value = PIH.DeliveryTermId;
				cmbLoadingPort.Value = PIH.LoadingPortId;
				cmbDestinationPort.Value = PIH.DestinationPortId;
				((Control)(object)cmbcareiertype).Text = PIH.CarierType;
				cmbfcycode.Value = PIH.FcurrencyId;
				txtfcyAmount.Text = PIH.FCurrencyAmount.ToString("#,##0.####");
				txtLocalAmt.Text = PIH.EquivalentAmount.ToString("#,##0.####");
				CmbExportCompany.Value = PIH.ExportCompanyId;
				txtGrossWeight.Text = ((PIH.GrossMton > 0m) ? PIH.GrossMton.ToString("#,##0.####") : (PIH.GrossWeight / 1000.0).ToString("#,##0.####"));
				txtNetWeight.Text = ((PIH.NetMton > 0m) ? PIH.NetMton.ToString("#,##0.####") : (PIH.NetWeight / 1000.0).ToString("#,##0.####"));
				txtNoOfContainer.Text = PIH.NoOfContainers.ToString();
				txtIformNo.Text = PIH.EFormNo;
				txtIformDate.Value = PIH.EFormDate;
				txtcertificate.Text = PIH.Certificate1;
				txtcertificateOfOrigion.Text = PIH.Certificate2;
				txtPaymentRemarks.Text = PIH.PaymentRemarks;
				txtDeliveryRemarks.Text = PIH.DeliveryRemarks;
				txtaddlesscommnets.Text = PIH.AddLessComments;
				txtaddlessamount.Text = PIH.AddLessAmount.ToString();
				txtTotalNetAmount.Text = PIH.TotalAmount.ToString("#,##0.####");
				CmbCreditAccount.Value = PIH.CreditAccountId;
				txtCustomerContractNos.Text = Conversion.ToString((object)PIH.CustomerContractNos);
				txtSubContractExpiryDays.Text = Conversion.ToString((object)PIH.SubContractExpiryDays);
				AttachmentsValues = PIH.AttachmentsValues;
				CustomAttachmentsValues = PIH.CustomAttachmentsValues;
				if (PIH.ExportVoucherId > 0)
				{
					btnAddCommission.Enabled = false;
					btnUpdateCommision.Enabled = false;
				}
				dtdetail.Rows.Clear();
				LstRemoveRecordDetail.Clear();
				int j;
				for (j = 0; j < PIH.ExImInvoiceDetail.Count; j++)
				{
					if (!dtItem.AsEnumerable().Any((DataRow dr) => (int)dr["Id"] == Conversion.ToInt((object)PIH.ExImInvoiceDetail[j].ItemId)))
					{
						dtItem.Rows.Add(PIH.ExImInvoiceDetail[j].ItemId, PIH.ExImInvoiceDetail[j].ItemName);
					}
					dtdetail.Rows.Add(PIH.ExImInvoiceDetail[j].Id, PIH.ExImInvoiceDetail[j].ContractId, PIH.ExImInvoiceDetail[j].ContractDetailId, PIH.ExImInvoiceDetail[j].PriRefNo, PIH.ExImInvoiceDetail[j].ProformaDocDate, PIH.ExImInvoiceDetail[j].ContractNoShipmentWise, PIH.ExImInvoiceDetail[j].ItemId, PIH.ExImInvoiceDetail[j].ItemName, PIH.ExImInvoiceDetail[j].PackingMaterialTypeId, PIH.ExImInvoiceDetail[j].PackMaterilaType, PIH.ExImInvoiceDetail[j].CropYearId, PIH.ExImInvoiceDetail[j].CropYear, PIH.ExImInvoiceDetail[j].JobLotId, PIH.ExImInvoiceDetail[j].JobLotDescription, PIH.ExImInvoiceDetail[j].MTon, PIH.ExImInvoiceDetail[j].OuterQtyUomId, PIH.ExImInvoiceDetail[j].OuterUOM, PIH.ExImInvoiceDetail[j].OuterQty, (PIH.ExImInvoiceDetail[j].RateWithoutAddLess > 0.0) ? PIH.ExImInvoiceDetail[j].RateWithoutAddLess : PIH.ExImInvoiceDetail[j].RatePrice, PIH.ExImInvoiceDetail[j].RateAddLess, PIH.ExImInvoiceDetail[j].RatePrice, PIH.ExImInvoiceDetail[j].ContractRate, PIH.ExImInvoiceDetail[j].RateUomId, PIH.ExImInvoiceDetail[j].RateUOM, PIH.ExImInvoiceDetail[j].RateEquivalent, PIH.ExImInvoiceDetail[j].FcAmount, PIH.ExImInvoiceDetail[j].ContractScheduleId, PIH.ExImInvoiceDetail[j].AttentiveLoadingDate.ToShortDateString(), PIH.ExImInvoiceDetail[j].HsCode, PIH.ExImInvoiceDetail[j].ItemDescriptionManual, PIH.ExImInvoiceDetail[j].OuterPackDescription, PIH.ExImInvoiceDetail[j].OtherAmount, PIH.ExImInvoiceDetail[j].AddLessAmount, true, PIH.ExImInvoiceDetail[j].ExImFarmingNTradeId, PIH.ExImInvoiceDetail[j].FarmingNTrade, PIH.ExImInvoiceDetail[j].ExImFarmingTypeId, PIH.ExImInvoiceDetail[j].ExImTradeTypeId, PIH.ExImInvoiceDetail[j].ContractScheduleDetailId, PIH.ExImInvoiceDetail[j].ScheduleFCL);
				}
				InvoiceUpdateFlag = PIH.ExImInvoiceDetail[0].ContractType;
				if (dtItem.Rows.Count > 0)
				{
					DDL.BindDDL(dtItem, combitem, "Id", "Name", "Item", ZeroIndex: false);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				dtOtherItem.Rows.Clear();
				int i;
				for (i = 0; i < PIH.ExImInvoiceOtherItemslist.Count; i++)
				{
					if (!dtOItem.AsEnumerable().Any((DataRow dr) => (int)dr["Id"] == PIH.ExImInvoiceOtherItemslist[i].otherItemId))
					{
						dtOItem.Rows.Add(PIH.ExImInvoiceOtherItemslist[i].otherItemId, PIH.ExImInvoiceOtherItemslist[i].ItemName, PIH.ExImInvoiceOtherItemslist[i].oItemRate);
					}
					dtOtherItem.Rows.Add(PIH.ExImInvoiceOtherItemslist[i].otherItemId, PIH.ExImInvoiceOtherItemslist[i].ItemName, PIH.ExImInvoiceOtherItemslist[i].oItemQty, PIH.ExImInvoiceOtherItemslist[i].oItemRate, PIH.ExImInvoiceOtherItemslist[i].oItemAmount, PIH.ExImInvoiceOtherItemslist[i].OtherItemRemarks);
				}
				if (dtOItem.Rows.Count > 0 && PIH.ForwardingId > 0)
				{
					DDL.BindDDL(dtOItem, CmbOtherItem, "Id", "Name", "Item Name", ZeroIndex: false);
				}
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				dtPaymentTerm.Rows.Clear();
				for (int l = 0; l < PIH.ExImInvoicePaymentTermsDetail.Count; l++)
				{
					dtPaymentTerm.Rows.Add(PIH.ExImInvoicePaymentTermsDetail[l].Id, PIH.ExImInvoicePaymentTermsDetail[l].PaymentTermId, PIH.ExImInvoicePaymentTermsDetail[l].PaymentTerm, PIH.ExImInvoicePaymentTermsDetail[l].DocumentTypeId, PIH.ExImInvoicePaymentTermsDetail[l].ExImEFormRegistrationId, PIH.ExImInvoicePaymentTermsDetail[l].FinancialInstrumentNo, PIH.ExImInvoicePaymentTermsDetail[l].PrcntOfTotal, PIH.ExImInvoicePaymentTermsDetail[l].FcyAmount, PIH.ExImInvoicePaymentTermsDetail[l].DueDays, PIH.ExImInvoicePaymentTermsDetail[l].PaymentRemarks);
				}
				grdpaymentdetail.DataSource = dtPaymentTerm;
				grdpaymentdetail.RetrieveStructure();
				grdSettingsPaymentTerm();
				dtCommission.Rows.Clear();
				for (int k = 0; k < PIH.ExImInvoiceSaleManCommissionList.Count; k++)
				{
					dtCommission.Rows.Add(PIH.ExImInvoiceSaleManCommissionList[k].exImInvoiceSaleManCommissionId, PIH.ExImInvoiceSaleManCommissionList[k].SalesPersonId, PIH.ExImInvoiceSaleManCommissionList[k].SalesPerson, PIH.ExImInvoiceSaleManCommissionList[k].commissionTypeId, PIH.ExImInvoiceSaleManCommissionList[k].CommissionType, PIH.ExImInvoiceSaleManCommissionList[k].commissionRate, PIH.ExImInvoiceSaleManCommissionList[k].rateUom, PIH.ExImInvoiceSaleManCommissionList[k].fcyAmount, PIH.ExImInvoiceSaleManCommissionList[k].exchangeRate, PIH.ExImInvoiceSaleManCommissionList[k].lcyAmount, PIH.ExImInvoiceSaleManCommissionList[k].Remarks);
				}
				grdCommissionInfo.DataSource = dtCommission;
				grdCommissionInfo.RetrieveStructure();
				grdSettingsCommissionInfo();
				dtOtherChargesDetail.Rows.Clear();
				FillOtherChargesDetailFromListCommonForReadById(PIH, dtOtherChargesDetail);
				grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
				grdOtherChargesDetail.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesDetail);
				UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetail, txtaddlessamount);
				if (grdOtherChargesDetail.RowCount == 0)
				{
					txtaddlessamount.Text = PIH.AddLessAmount.ToString();
				}
				ProportionateOtherItemAmount();
				DetailFormReset();
				UpdateMode = true;
				txtDocDate.Focus();
				txtExRate.Text = PIH.ConversionRate.ToString("#,##0.####");
				ProportionateAddLessAmount();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				PaymentGridBalanceAmountFromInvoiceAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BindFinancialInstrument();
	}

	private void FillDetailFromListCommonForReadById(ExImInvoice invGrn, DataTable dt)
	{
		if (invGrn == null || invGrn.ExImInvoiceDetail == null || !invGrn.ExImInvoiceDetail.Any())
		{
			return;
		}
		foreach (ExImInvoicePackingDetail detail in invGrn.ExImInvoiceDetail)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id,
				detail.ContractId,
				detail.ContractDetailId,
				detail.PriRefNo,
				detail.ProformaDocDate,
				detail.ContractNoShipmentWise,
				detail.ItemId,
				detail.ItemName,
				detail.PackingMaterialTypeId,
				detail.PackMaterilaType,
				detail.CropYearId,
				detail.CropYear,
				detail.JobLotId,
				detail.JobLotDescription,
				detail.MTon,
				detail.OuterQtyUomId,
				detail.OuterUOM,
				detail.OuterQty,
				(detail.RateWithoutAddLess > 0.0) ? detail.RateWithoutAddLess : detail.RatePrice,
				detail.RateAddLess,
				detail.RatePrice,
				detail.ContractRate,
				detail.RateUomId,
				detail.RateUOM,
				detail.RateEquivalent,
				detail.FcAmount,
				detail.ContractScheduleId,
				detail.AttentiveLoadingDate.ToShortDateString(),
				detail.HsCode,
				detail.ItemDescriptionManual,
				detail.OuterPackDescription,
				detail.OtherAmount,
				detail.AddLessAmount,
				true,
				detail.ExImFarmingNTradeId,
				detail.FarmingNTrade,
				detail.ExImFarmingTypeId,
				detail.ExImTradeTypeId,
				detail.ContractScheduleDetailId,
				detail.ScheduleFCL
			};
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
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dtInvoice = new DataTable();
			dtInvoice.Columns.Add("Id", typeof(int));
			dtInvoice.Columns.Add("Invoice", typeof(string));
			DataTable dt = ExImLcOrder.GetDataForDropDownFromExportInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Invoice")
					{
						dtInvoice.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
			DDL.BindDDL(dtInvoice, CmbInvoiceNoHistory, "Id", "Invoice", "Party", ZeroIndex: false);
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistoryForLayout.Rows.Clear();
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.Ids = "204";
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
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("InvoiceNo");
				dt.Columns.Add("DocCode", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("ContractNos");
				dt.Columns.Add("NoOfContainers", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("Add/Less", typeof(double));
				dt.Columns.Add("TotalAmount", typeof(double));
				dt.Columns.Add("JobLots");
				dt.Columns.Add("LoadingPort");
				dt.Columns.Add("DestinationPort");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments");
				dtHistoryForLayout.Clone();
				for (int i = 0; i < dtgrd.Rows.Count; i++)
				{
					dt.Rows.Add(dtgrd.Rows[i]["Id"], dtgrd.Rows[i]["InvoiceNo"], dtgrd.Rows[i]["DocCode"], Conversion.ToDateTime(dtgrd.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtgrd.Rows[i]["Customer"], dtgrd.Rows[i]["ContractNos"], dtgrd.Rows[i]["NoOfContainers"], dtgrd.Rows[i]["GrossWeight"], dtgrd.Rows[i]["NetWeight"], dtgrd.Rows[i]["FCurrencyAmount"], dtgrd.Rows[i]["AddLessAmount"], dtgrd.Rows[i]["TotalAmount"], dtgrd.Rows[i]["JobLots"], dtgrd.Rows[i]["LoadingPort"], dtgrd.Rows[i]["DestinationPort"], dtgrd.Rows[i]["EntryUserName"], dtgrd.Rows[i]["EntryDate"], dtgrd.Rows[i]["ModifyUserName"], dtgrd.Rows[i]["ModifyDate"], dtgrd.Rows[i]["NoOfAttachments"]);
				}
				dtHistoryForLayout = dt.Copy();
				DataGridHistory.DataSource = dt;
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
			List<string> hiddenColumns = new List<string> { "Id", "InvoiceId", "GatePassId" };
			List<string> ExcludeC = new List<string> { "InvoiceNetWeight", "InvoiceGrossWeight", "FactoryWeight" };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3, hiddenColumns, null, null, ColumnSetting: true, BracketsForamt: false, ExcludeC);
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			DataGridHistory.RootTable.Columns["DocCode"].Width = Constants.ExportConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["ContractNos"].Width = Constants.ExportConstants.StringConstant;
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
			GridEX_Helper.AddButton(DataGridHistory, "AddAttachment", "Add Attachment", 105);
			((Control)(object)DataGridHistory).Refresh();
			ctrlGrdBar1_Load(null, null);
			if (formrights.DoHavePrintRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Slip", "521-Slip", 70, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
				GridEX_Helper.AddButton(DataGridHistory, "PrintPackingList", "Print Packing List", 110, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
				GridEX_Helper.AddButton(DataGridHistory, "DoPrintByInvoice", "Do Print", 60, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
				GridEX_Helper.AddButton(DataGridHistory, "GpPrint", "Gp Print", 60, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
			}
			if (formrights.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 40, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
			}
			DataGridHistory.FrozenColumns = 4;
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
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 204);
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
					InvoiceSlip521(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Edit")
				{
					ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				else if (e.Column.Key == "PrintPackingList")
				{
					CommonServices.ExportPackingList529(Id);
				}
				else if (e.Column.Key == "DoPrintByInvoice")
				{
					CrystalReportPrint_Helper.DeliveryOrderPrintByInvoicesId(Id, 84);
				}
				else if (e.Column.Key == "GpPrint")
				{
					CrystalReportPrint_Helper.OutwardGatePassPrintByInvoicesIds_295(null, Id);
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, 204, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
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
				return;
			}
			DataTable dtDetailHistory = dtdetail.Clone();
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory);
			gridDetailHistory.DataSource = dtDetailHistory;
			gridDetailHistory.RetrieveStructure();
			grddetailhistorySettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ContractId", "ContractScheduleId", "ContractDetailId", "ItemId", "PackTypeId", "PackSizeId", "JobLotId", "RateUOMId", "CropYearId",
				"ContractNo", "ContractDate", "RateEquivalent", "FlagToUpdateScheduleCombo", "ExImTradeTypeId", "ExImFarmingTypeId", "ExImFarmingNTradeId", "ScheduleFCL", "ContractSchedule", "ContractScheduleDetailId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(gridDetailHistory, 2, 3, hiddenColumns);
			gridDetailHistory.RootTable.Columns["ContractRate"].Visible = FlagFin;
			gridDetailHistory.RootTable.Columns["ContractNoInvoiceWise"].Width = Constants.ExportConstants.ContractNoConstant;
			gridDetailHistory.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants - 55;
			gridDetailHistory.RootTable.Columns["PackType"].Width = Constants.ExportConstants.DocumentStatus + 5;
			gridDetailHistory.RootTable.Columns["CropYear"].Width = Constants.ExportConstants.CropYearConstant - 10;
			gridDetailHistory.RootTable.Columns["JobLot"].Width = Constants.ExportConstants.JobLotConstant + 15;
			gridDetailHistory.RootTable.Columns["Qty/M.Ton"].Width = Constants.ExportConstants.QtyConstant;
			gridDetailHistory.RootTable.Columns["PackSize"].Width = Constants.ExportConstants.UomConstant;
			gridDetailHistory.RootTable.Columns["NoOfBags"].Width = Constants.ExportConstants.QtyConstant;
			gridDetailHistory.RootTable.Columns["RateWithoutAddLess"].Width = Constants.ExportConstants.AddLessConstant + 10;
			gridDetailHistory.RootTable.Columns["RateAddLess"].Width = Constants.ExportConstants.AddLessConstant;
			gridDetailHistory.RootTable.Columns["Cost/M.Ton"].Width = Constants.ExportConstants.FclConstant;
			gridDetailHistory.RootTable.Columns["RateUOM"].Width = Constants.ExportConstants.ExRateConstant - 10;
			gridDetailHistory.RootTable.Columns["Amount"].Width = Constants.ExportConstants.AmountConstant;
			gridDetailHistory.RootTable.Columns["HSCode"].Width = Constants.ExportConstants.HsCode;
			gridDetailHistory.RootTable.Columns["ItemDetail"].Width = Constants.ExportConstants.CommodityDetail;
			gridDetailHistory.RootTable.Columns["PackingDetail"].Width = Constants.ExportConstants.CommodityDetail;
			gridDetailHistory.RootTable.Columns["OtherItemAmount"].Width = Constants.ExportConstants.AmountConstant;
			gridDetailHistory.RootTable.Columns["FarmingNTrade"].Width = Constants.ExportConstants.HsCode + 10;
			if (RateAddLessOnCommercialInvoice)
			{
				gridDetailHistory.RootTable.Columns["RateAddLess"].EditType = (EditType)1;
				gridDetailHistory.RootTable.Columns["RateWithoutAddLess"].Visible = true;
				gridDetailHistory.RootTable.Columns["RateAddLess"].Visible = true;
			}
			else
			{
				gridDetailHistory.RootTable.Columns["RateWithoutAddLess"].Visible = false;
				gridDetailHistory.RootTable.Columns["RateAddLess"].Visible = false;
			}
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

	private void dtxtfcyamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtTotalNetAmount.Text) > 0.0 && Conversion.ToDouble((object)dtxtfcyamount.Text) > 0.0)
			{
				double TotalAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim());
				double Percent = Conversion.ToDouble((object)dtxtfcyamount.Text.Trim()) / TotalAmount * 100.0;
				dtxtpercentoftotal.Text = Percent.ToString();
				if (Conversion.ToDouble((object)dtxtpercentoftotal.Text) > 100.0)
				{
					dtxtpercentoftotal.Text = "0";
					throw new Exception("Percent cannot be greater than 100 Please check");
				}
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

	private void CmbOtherItem_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (btnupdate.Visible && btnupdate.Enabled)
			{
				DataTable dt = (DataTable)((UltraGridBase)CmbOtherItem).DataSource;
				if (((UltraGridBase)CmbOtherItem).ActiveRow != null && dt.Columns.Count == 3)
				{
					txtoRate.Text = Conversion.ToString(((UltraDropDownBase)CmbOtherItem).SelectedRow.Cells[2].Value);
				}
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
			TotalAmountCalculate();
			ProportionateAddLessAmount();
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

	private void PaymentDetailPercentCalculate()
	{
		try
		{
			if (Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim()) > 0.0 && Conversion.ToDouble((object)dtxtpercentoftotal.Text) > 0.0)
			{
				if (Conversion.ToDouble((object)dtxtpercentoftotal.Text) > 100.0)
				{
					dtxtpercentoftotal.Text = "0";
					throw new Exception("Percent cannot be greater than 100 Please check");
				}
				double num = Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim());
				double Percent = Conversion.ToDouble((object)dtxtpercentoftotal.Text.Trim());
				double FcAmount = num * Percent / 100.0;
				dtxtfcyamount.Text = FcAmount.ToString("#,##.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetFinancialInstrumentsBalance()
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
				DocumentTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells[3].Value),
				Id = Conversion.ToInt(((UltraDropDownBase)CmbFinInstruments).SelectedRow.Cells["Id"].Value)
			});
			txtBalanceFI.Text = Balance.ToString("#,#0.###");
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
			double Qty = Conversion.ToDouble((object)txtoQty.Text.Trim());
			double Rate = Conversion.ToDouble((object)txtoRate.Text.Trim());
			if (Qty > 0.0 && Rate > 0.0)
			{
				txtoAmount.Text = (Qty * Rate).ToString();
			}
			else
			{
				txtoAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProportionateAddLessAmount()
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2).ToString());
			double addLessAmount = Conversion.ToDouble((object)txtaddlessamount.Text);
			if (addLessAmount != 0.0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble(obj.Cells["Qty/M.Ton"].Value);
					obj.Cells["AddLessAmount"].Value = addLessAmount / NetWeight * Weight;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["AddLessAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmountCalculate()
	{
		try
		{
			object total = grdotheritems.GetTotal(grdotheritems.RootTable.Columns["Amount"], (AggregateFunction)2);
			double AddlessAmount = Conversion.ToDouble((object)txtaddlessamount.Text);
			double FcyAmount = Conversion.ToDouble((object)txtfcyAmount.Text);
			double OtherAmount = Conversion.ToDouble(total);
			if (FcyAmount > 0.0)
			{
				txtTotalNetAmount.Text = (FcyAmount + AddlessAmount + OtherAmount).ToString("#,#.###");
			}
			if (Conversion.ToDouble((object)txtTotalNetAmount.Text) > 0.0 && grdpaymentdetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdpaymentdetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double TotalAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text);
					double Amount = Conversion.ToDouble(obj.Cells["%OfTotal"].Value) * TotalAmount / 100.0;
					obj.Cells["FcyAmount"].Value = Amount;
					obj.EndEdit();
				}
			}
			CalculateLocalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateLocalAmount()
	{
		try
		{
			double fcyAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text);
			double ExchangeRate = Conversion.ToDouble((object)txtExRate.Text);
			if (fcyAmount > 0.0 && ExchangeRate > 0.0)
			{
				txtLocalAmt.Text = (fcyAmount * ExchangeRate).ToString("#,#0.###");
			}
			else
			{
				txtLocalAmt.Text = "0";
			}
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
			txtCommissionExchangeRate.Text = txtExRate.Text;
			CalculateLocalAmount();
			RecalculateCommissionGridLocalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtaddlesscommnets_TextChanged(object sender, EventArgs e)
	{
	}

	private void CalculateWeight()
	{
		try
		{
			double MTon = 0.0;
			double OuterUom = 0.0;
			txtNoOfBags.Text = 0.ToString();
			if (((UltraGridBase)CmbPackUom).ActiveRow != null)
			{
				MTon = Conversion.ToDouble((object)txtQtyMTon.Text);
				OuterUom = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				if (MTon > 0.0 && OuterUom > 0.0)
				{
					double noofbags = MTon * 1000.0 / OuterUom;
					txtNoOfBags.Text = Math.Round(noofbags).ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculatAmount()
	{
		try
		{
			double ItemRate = 0.0;
			double ItemRateUom = 0.0;
			double Qty = 0.0;
			txtAmount.Text = 0.ToString();
			if (((UltraGridBase)CmbRateUom).ActiveRow != null)
			{
				ItemRate = Conversion.ToDouble((object)txtCostMTon.Text);
				ItemRateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				Qty = Conversion.ToDouble((object)txtQtyMTon.Text);
				if (ItemRate > 0.0 && ItemRateUom > 0.0 && Qty > 0.0)
				{
					double weightkgs = Qty * 1000.0;
					double amount = ItemRate / ItemRateUom * weightkgs;
					txtAmount.Text = amount.ToString("#,#0.###");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitempck_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCostMTon_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
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
			BindFinancialInstrument();
			bindConsigneeAgainstCustomer();
			HSCodeBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceSlip521(int PrintId)
	{
		try
		{
			CommonServices.CommercialInvoiceSlip521(PrintId);
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
			InvoiceSlip521(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCostMTon_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dtdetail);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtHistoryForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdotheritems, dtOtherItem);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdpaymentdetail, dtPaymentTerm);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnLoad_Click(object sender, EventArgs e)
	{
		try
		{
			Loader = new LoadSalesContractForInvoice(UserAccount);
			Loader.DocumentTypeId = 202;
			if (btnupdate.Visible && btnupdate.Enabled)
			{
				HashSet<int> uniqueScheduleIds = new HashSet<int>();
				GridEXRow[] dataRows = grdDetail.GetDataRows();
				for (int i = 0; i < dataRows.Length; i++)
				{
					int scheduleId = Conversion.ToInt(dataRows[i].Cells["ContractScheduleId"].Value);
					uniqueScheduleIds.Add(scheduleId);
				}
				string scheduleIds = string.Join(",", uniqueScheduleIds);
				Loader.ScheduleIds = scheduleIds;
			}
			else
			{
				Loader.ScheduleIds = "";
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
		//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		try
		{
			DataTable contract = Loader.LoaderData;
			if (contract.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtItem = new DataTable();
			if (RecId > 0)
			{
				dtItem = (DataTable)((UltraGridBase)combitem).DataSource;
			}
			else
			{
				dtItem.Columns.Add("Id", typeof(int));
				dtItem.Columns.Add("Name");
			}
			if (grdDetail.GetRows().Count() > 0)
			{
				if (Conversion.ToInt(contract.Rows[0]["ScheduleDetailId"]) == 0)
				{
					if (Conversion.ToInt(contract.Rows[0]["SupCustId"]) != Conversion.ToInt(cmbSupCust.Value) || Conversion.ToInt(contract.Rows[0]["FcurrencyId"]) != Conversion.ToInt(cmbfcycode.Value))
					{
						throw new Exception("Already Loaded Row's Have Different Customer And Currency. So you Can't Load Rows Of Different Customer or Currency!");
					}
				}
				else if (Conversion.ToInt(contract.Rows[0]["SupCustId"]) != Conversion.ToInt(cmbSupCust.Value))
				{
					throw new Exception("Already Loaded Row's Have Different Customer. So you Can't Load Rows Of Different Customer!");
				}
			}
			((Control)(object)cmbSupCust).Enabled = false;
			((Control)(object)cmbfcycode).Enabled = false;
			cmbSupCust.Value = Conversion.ToInt(contract.Rows[0]["SupCustId"]);
			cmbSupCust_Leave(null, null);
			cmbNotifyParty1.Value = Conversion.ToInt(contract.Rows[0]["NotifyPartyId"]);
			cmbexporterBankNew.Value = Conversion.ToInt(contract.Rows[0]["ExporterBankId"]);
			cmbfcycode.Value = Conversion.ToInt(contract.Rows[0]["FcurrencyId"]);
			cmbdeliverytermnew.Value = Conversion.ToInt(contract.Rows[0]["DeliveryTermId"]);
			cmbLoadingPort.Value = Conversion.ToInt(contract.Rows[0]["LoadingPortId"]);
			cmbDestinationPort.Value = Conversion.ToInt(contract.Rows[0]["DestinationPortId"]);
			CmbExportCompany.Value = Conversion.ToInt(contract.Rows[0]["ExportCompanyId"]);
			int totalContainers = (from row in contract.AsEnumerable()
				group row by row.Field<int>("ScheduleId")).Sum((IGrouping<int, DataRow> g) => g.First().Field<int>("NoOfContainers"));
			txtNoOfContainer.Text = totalContainers.ToString();
			string contractNoList = string.Join(", ", (from row in contract.AsEnumerable()
				select row.Field<string>("CustomerContractNo") into c
				where !string.IsNullOrWhiteSpace(c)
				select c).Distinct());
			txtCustomerContractNos.Text = contractNoList;
			int i;
			for (i = 0; i < contract.Rows.Count; i++)
			{
				bool flag = false;
				if (grdDetail.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdDetail.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToInt(contract.Rows[i]["ScheduleDetailId"]) == 0)
						{
							if (Conversion.ToInt(item.Cells["ContractDetailId"].Value) == Conversion.ToInt(contract.Rows[i]["ContractDetailId"]))
							{
								flag = true;
								break;
							}
						}
						else if (Conversion.ToInt(item.Cells["ContractScheduleDetailId"].Value) == Conversion.ToInt(contract.Rows[i]["ScheduleDetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					if (!dtItem.AsEnumerable().Any((DataRow dr) => (int)dr["Id"] == Conversion.ToInt(contract.Rows[i]["ExImItemId"])))
					{
						dtItem.Rows.Add(contract.Rows[i]["ExImItemId"], contract.Rows[i]["ItemName"]);
					}
					dtdetail.Rows.Add(0, contract.Rows[i]["Id"], contract.Rows[i]["ContractDetailId"], contract.Rows[i]["ProformaNo"], contract.Rows[i]["ProformaDate"], contract.Rows[i]["ScheduleCode"], contract.Rows[i]["ExImItemId"], contract.Rows[i]["ItemName"], contract.Rows[i]["InvPackingMaterialTypeId"], contract.Rows[i]["PackingTYpe"], contract.Rows[i]["CropYearId"], contract.Rows[i]["CropYear"], contract.Rows[i]["JobLotId"], contract.Rows[i]["JobLotDescription"], Conversion.ToDouble(contract.Rows[i]["BalMTon"]), contract.Rows[i]["PackUomId"], contract.Rows[i]["PackUom"], contract.Rows[i]["BalNoofBags"], contract.Rows[i]["RatePrice"], 0, contract.Rows[i]["RatePrice"], contract.Rows[i]["RatePrice"], contract.Rows[i]["RateUomId"], contract.Rows[i]["RateUom"], contract.Rows[i]["RateEquivalent"], contract.Rows[i]["BalShipAmount"], contract.Rows[i]["ScheduleId"], "", "", contract.Rows[i]["CommodityDetail"], "", 0, 0, false, contract.Rows[i]["ExImFarmingNTradeId"], contract.Rows[i]["FarmingNTrade"], contract.Rows[i]["ExImFarmingTypeId"], contract.Rows[i]["ExImTradeTypeId"], contract.Rows[i]["ScheduleDetailId"]);
				}
				flag = false;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, combitem, "Id", "Name", "Item", ZeroIndex: false);
			}
			string distinctIds = string.Join(",", (from row in contract.AsEnumerable()
				select row.Field<int>("Id")).Distinct());
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Ids = distinctIds
			};
			dtOtherItem.Rows.Clear();
			DataTable dt = ExImLcOrder.GetOtherItemsFromLcOrder(val);
			if (dt.Rows.Count > 0)
			{
				for (int k = 0; k < dt.Rows.Count; k++)
				{
					dtOtherItem.Rows.Add(dt.Rows[k]["otherItemId"], dt.Rows[k]["ItemName"], dt.Rows[k]["oItemQty"], dt.Rows[k]["oItemRate"], dt.Rows[k]["oItemAmount"], dt.Rows[k]["OtherItemRemarks"]);
				}
			}
			ProportionateOtherItemAmount();
			dtPaymentTerm.Rows.Clear();
			DataTable dtPay = ExImLcOrder.LcOrderPaymentTermsDetailByLcOrderIds(distinctIds);
			if (dtPay.Rows.Count > 0)
			{
				for (int j = 0; j < dtPay.Rows.Count; j++)
				{
					dtPaymentTerm.Rows.Add(0, dtPay.Rows[j]["PaymentTermId"], dtPay.Rows[j]["LcOrderTerm"], 0, 0, "", dtPay.Rows[j]["PrcntOfTotal"], dtPay.Rows[j]["FcyAmount"], dtPay.Rows[j]["DueDays"], dtPay.Rows[j]["Remarks"]);
				}
			}
			grdpaymentdetail.DataSource = dtPaymentTerm;
			grdpaymentdetail.RetrieveStructure();
			grdSettingsPaymentTerm();
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			FillOtherchargesDetailByContractIdsDBCall(distinctIds);
			grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
			grdOtherChargesDetail.RetrieveStructure();
			grdSettingsOtherChargesDetail(grdOtherChargesDetail);
			UpdateAddLessAmountInHeaderFromChargesGrid(grdOtherChargesDetail, txtaddlessamount);
			txtExRate.Text = Conversion.ToDouble(contract.Rows[0]["ExchangeRate"]).ToString();
			getTotalnetWeightFromGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void ProportionateOtherItemAmount()
	{
		try
		{
			double ContractAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
			double OtherItemAmount = Conversion.ToDouble(grdotheritems.GetTotal(grdotheritems.RootTable.Columns["Amount"], (AggregateFunction)2));
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
				if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && UpdateMode)
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
						((Control)(object)grdpaymentdetail).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						combpcktype.Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						CmbOtherItem.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						dcmbpaymentterm.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						CmbOtherItem.Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 2;
						dcmbpaymentterm.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						tabControl2.SelectedIndex = 0;
						combitem.Focus();
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
		catch (Exception)
		{
			throw;
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				GridEXRow r = grdDetail.CurrentRow;
				if (r == null || (int)r.RowType != 0 || grdDetail.CurrentColumn == null)
				{
					return;
				}
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndex != -1)
					{
						throw new Exception("Reset the Detail First");
					}
					if (grdDetail.GetRows().Count() > 1)
					{
						if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
						{
							if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
							{
								return;
							}
							ExImInvoicePackingDetail vd = new ExImInvoicePackingDetail();
							vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
							vd.ContractId = Conversion.ToInt(r.Cells["ContractId"].Value);
							vd.ContractDetailId = Conversion.ToInt(r.Cells["ContractDetailId"].Value);
							vd.ContractNoShipmentWise = Conversion.ToString(r.Cells["ContractNoInvoiceWise"].Value);
							vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
							vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
							vd.PackingMaterialTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
							vd.MTon = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
							vd.NetWeight = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
							vd.OuterQtyUomId = Conversion.ToInt(r.Cells["PackSizeId"].Value);
							vd.OuterQty = Conversion.ToDouble(r.Cells["NoOfBags"].Value);
							vd.InnerQtyUomId = Conversion.ToInt(r.Cells["PackSizeId"].Value);
							vd.InnerQty = Conversion.ToInt(r.Cells["NoOfBags"].Value);
							vd.RatePrice = Conversion.ToDouble(r.Cells["Cost/M.Ton"].Value);
							vd.RateUomId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
							vd.FcAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
							vd.ContractScheduleId = Conversion.ToInt(r.Cells["ContractScheduleId"].Value);
							vd.HsCode = Conversion.ToString((object)r.Cells["HSCode"].Text);
							vd.ItemDescriptionManual = Conversion.ToString((object)r.Cells["ItemDetail"].Text);
							vd.ExImFarmingNTradeId = Conversion.ToInt(r.Cells["ExImFarmingNTradeId"].Value);
							vd.ExImTradeTypeId = Conversion.ToInt(r.Cells["ExImTradeTypeId"].Value);
							vd.ExImFarmingTypeId = Conversion.ToInt(r.Cells["ExImFarmingTypeId"].Value);
							vd.OuterPackDescription = Conversion.ToString((object)r.Cells["PackingDetail"].Text);
							vd.ActionTypeId = 3;
							LstRemoveRecordDetail.Add(vd);
							dtdetail.Rows[r.RowIndex].Delete();
							r.Delete();
						}
						else
						{
							r.Delete();
						}
					}
					getTotalnetWeightFromGrid();
					grdDetail.UpdateData();
				}
				else if (grdDetail.CurrentColumn.Key == "Add")
				{
					DataRow dr = dtdetail.Rows[r.RowIndex];
					dtdetail.Rows.Add(dr.ItemArray);
					dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
					grdDetail.DataSource = dtdetail;
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
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdotheritems_DoubleClick(null, null);
				}
				else if (e.Control)
				{
					_ = e.KeyCode;
					_ = 32;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpaymentdetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdpaymentdetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdpaymentdetail_DoubleClick(null, null);
				}
				else if (e.Control)
				{
					_ = e.KeyCode;
					_ = 32;
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
					ReadById(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Slip" && formrights.DoHavePrintRights)
				{
					InvoiceSlip521(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "PrintPackingList")
				{
					CommonServices.ExportPackingList529(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "GpPrint")
				{
					CrystalReportPrint_Helper.OutwardGatePassPrintByInvoicesIds_295(null, Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Attachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 204);
				}
				if (DataGridHistory.CurrentColumn.Key == "Attachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 204);
				}
				if (DataGridHistory.CurrentColumn.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, 204, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void BtnPrintPL_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExportPackingList529(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint501B_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExportSalesContractByInvoiceIdExport501B(0, RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnExportCharges_Click(object sender, EventArgs e)
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
		//IL_0c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Expected O, but got Unknown
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5c: Expected O, but got Unknown
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c67: Expected O, but got Unknown
		//IL_0c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c72: Expected O, but got Unknown
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7d: Expected O, but got Unknown
		//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c88: Expected O, but got Unknown
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Expected O, but got Unknown
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9e: Expected O, but got Unknown
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Expected O, but got Unknown
		//IL_0cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb4: Expected O, but got Unknown
		//IL_0cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbf: Expected O, but got Unknown
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cca: Expected O, but got Unknown
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Expected O, but got Unknown
		//IL_0cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce0: Expected O, but got Unknown
		//IL_0ce2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ceb: Expected O, but got Unknown
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf6: Expected O, but got Unknown
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d01: Expected O, but got Unknown
		//IL_0d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0c: Expected O, but got Unknown
		//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d17: Expected O, but got Unknown
		//IL_0d19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Expected O, but got Unknown
		//IL_0d24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2d: Expected O, but got Unknown
		//IL_0d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d38: Expected O, but got Unknown
		//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d43: Expected O, but got Unknown
		//IL_0d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4e: Expected O, but got Unknown
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d59: Expected O, but got Unknown
		//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d64: Expected O, but got Unknown
		//IL_0d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6f: Expected O, but got Unknown
		//IL_0d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7a: Expected O, but got Unknown
		//IL_0d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d85: Expected O, but got Unknown
		//IL_0d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d90: Expected O, but got Unknown
		//IL_0d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9b: Expected O, but got Unknown
		//IL_0d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Expected O, but got Unknown
		//IL_0da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db1: Expected O, but got Unknown
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbc: Expected O, but got Unknown
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc7: Expected O, but got Unknown
		//IL_0dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd2: Expected O, but got Unknown
		//IL_0dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddd: Expected O, but got Unknown
		//IL_0ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de8: Expected O, but got Unknown
		//IL_0dea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Expected O, but got Unknown
		//IL_0df5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfe: Expected O, but got Unknown
		//IL_0e00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e09: Expected O, but got Unknown
		//IL_0e0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e14: Expected O, but got Unknown
		//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1f: Expected O, but got Unknown
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2a: Expected O, but got Unknown
		//IL_0e2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e35: Expected O, but got Unknown
		//IL_0e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e40: Expected O, but got Unknown
		//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4b: Expected O, but got Unknown
		//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e56: Expected O, but got Unknown
		//IL_0e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e61: Expected O, but got Unknown
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6c: Expected O, but got Unknown
		//IL_0e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e77: Expected O, but got Unknown
		//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e82: Expected O, but got Unknown
		//IL_0e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8d: Expected O, but got Unknown
		//IL_0e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e98: Expected O, but got Unknown
		//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea3: Expected O, but got Unknown
		//IL_0ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eae: Expected O, but got Unknown
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb9: Expected O, but got Unknown
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec4: Expected O, but got Unknown
		//IL_0ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecf: Expected O, but got Unknown
		//IL_0ed1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eda: Expected O, but got Unknown
		//IL_0edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee5: Expected O, but got Unknown
		//IL_0ee7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef0: Expected O, but got Unknown
		//IL_0ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efb: Expected O, but got Unknown
		//IL_0efd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f06: Expected O, but got Unknown
		//IL_0f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f11: Expected O, but got Unknown
		//IL_0f13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1c: Expected O, but got Unknown
		//IL_0f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f27: Expected O, but got Unknown
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f32: Expected O, but got Unknown
		//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3d: Expected O, but got Unknown
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f48: Expected O, but got Unknown
		//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f53: Expected O, but got Unknown
		//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5e: Expected O, but got Unknown
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f69: Expected O, but got Unknown
		//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Expected O, but got Unknown
		//IL_0f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Expected O, but got Unknown
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8a: Expected O, but got Unknown
		//IL_0f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f95: Expected O, but got Unknown
		//IL_0f97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa0: Expected O, but got Unknown
		//IL_0fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fab: Expected O, but got Unknown
		//IL_0fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb6: Expected O, but got Unknown
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc1: Expected O, but got Unknown
		//IL_0fc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcc: Expected O, but got Unknown
		//IL_0fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd7: Expected O, but got Unknown
		//IL_0fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe2: Expected O, but got Unknown
		//IL_0fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fed: Expected O, but got Unknown
		//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff8: Expected O, but got Unknown
		//IL_0ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1003: Expected O, but got Unknown
		//IL_1005: Unknown result type (might be due to invalid IL or missing references)
		//IL_100e: Expected O, but got Unknown
		//IL_1010: Unknown result type (might be due to invalid IL or missing references)
		//IL_1019: Expected O, but got Unknown
		//IL_101b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Expected O, but got Unknown
		//IL_1026: Unknown result type (might be due to invalid IL or missing references)
		//IL_102f: Expected O, but got Unknown
		//IL_1031: Unknown result type (might be due to invalid IL or missing references)
		//IL_103a: Expected O, but got Unknown
		//IL_103c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1045: Expected O, but got Unknown
		//IL_1047: Unknown result type (might be due to invalid IL or missing references)
		//IL_1050: Expected O, but got Unknown
		//IL_1052: Unknown result type (might be due to invalid IL or missing references)
		//IL_105b: Expected O, but got Unknown
		//IL_105d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1066: Expected O, but got Unknown
		//IL_1068: Unknown result type (might be due to invalid IL or missing references)
		//IL_1071: Expected O, but got Unknown
		//IL_1073: Unknown result type (might be due to invalid IL or missing references)
		//IL_107c: Expected O, but got Unknown
		//IL_107e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1087: Expected O, but got Unknown
		//IL_1089: Unknown result type (might be due to invalid IL or missing references)
		//IL_1092: Expected O, but got Unknown
		//IL_1094: Unknown result type (might be due to invalid IL or missing references)
		//IL_109d: Expected O, but got Unknown
		//IL_109f: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Expected O, but got Unknown
		//IL_10aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b3: Expected O, but got Unknown
		//IL_10b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10be: Expected O, but got Unknown
		//IL_10c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c9: Expected O, but got Unknown
		//IL_10cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d4: Expected O, but got Unknown
		//IL_10d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10df: Expected O, but got Unknown
		//IL_10e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ea: Expected O, but got Unknown
		//IL_10ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f5: Expected O, but got Unknown
		//IL_10f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1100: Expected O, but got Unknown
		//IL_1102: Unknown result type (might be due to invalid IL or missing references)
		//IL_110b: Expected O, but got Unknown
		//IL_110d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1116: Expected O, but got Unknown
		//IL_1118: Unknown result type (might be due to invalid IL or missing references)
		//IL_1121: Expected O, but got Unknown
		//IL_1123: Unknown result type (might be due to invalid IL or missing references)
		//IL_112c: Expected O, but got Unknown
		//IL_112e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1137: Expected O, but got Unknown
		//IL_1139: Unknown result type (might be due to invalid IL or missing references)
		//IL_1142: Expected O, but got Unknown
		//IL_1144: Unknown result type (might be due to invalid IL or missing references)
		//IL_114d: Expected O, but got Unknown
		//IL_114f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1158: Expected O, but got Unknown
		//IL_115a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1163: Expected O, but got Unknown
		//IL_1165: Unknown result type (might be due to invalid IL or missing references)
		//IL_116e: Expected O, but got Unknown
		//IL_1170: Unknown result type (might be due to invalid IL or missing references)
		//IL_1179: Expected O, but got Unknown
		//IL_117b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1184: Expected O, but got Unknown
		//IL_1186: Unknown result type (might be due to invalid IL or missing references)
		//IL_118f: Expected O, but got Unknown
		//IL_1191: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Expected O, but got Unknown
		//IL_119c: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a5: Expected O, but got Unknown
		//IL_11a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b0: Expected O, but got Unknown
		//IL_11b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bb: Expected O, but got Unknown
		//IL_11bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c6: Expected O, but got Unknown
		//IL_11c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d1: Expected O, but got Unknown
		//IL_11d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11dc: Expected O, but got Unknown
		//IL_11de: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e7: Expected O, but got Unknown
		//IL_11e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f2: Expected O, but got Unknown
		//IL_11f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fd: Expected O, but got Unknown
		//IL_11ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1208: Expected O, but got Unknown
		//IL_120a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1213: Expected O, but got Unknown
		//IL_1215: Unknown result type (might be due to invalid IL or missing references)
		//IL_121e: Expected O, but got Unknown
		//IL_1220: Unknown result type (might be due to invalid IL or missing references)
		//IL_1229: Expected O, but got Unknown
		//IL_122b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1234: Expected O, but got Unknown
		//IL_1236: Unknown result type (might be due to invalid IL or missing references)
		//IL_123f: Expected O, but got Unknown
		//IL_1241: Unknown result type (might be due to invalid IL or missing references)
		//IL_124a: Expected O, but got Unknown
		//IL_124c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1255: Expected O, but got Unknown
		//IL_1257: Unknown result type (might be due to invalid IL or missing references)
		//IL_1260: Expected O, but got Unknown
		//IL_1262: Unknown result type (might be due to invalid IL or missing references)
		//IL_126b: Expected O, but got Unknown
		//IL_126d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1276: Expected O, but got Unknown
		//IL_1278: Unknown result type (might be due to invalid IL or missing references)
		//IL_1281: Expected O, but got Unknown
		//IL_1283: Unknown result type (might be due to invalid IL or missing references)
		//IL_128c: Expected O, but got Unknown
		//IL_128e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1297: Expected O, but got Unknown
		//IL_1299: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a2: Expected O, but got Unknown
		//IL_12a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ad: Expected O, but got Unknown
		//IL_12af: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b8: Expected O, but got Unknown
		//IL_12ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c3: Expected O, but got Unknown
		//IL_12c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ce: Expected O, but got Unknown
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d9: Expected O, but got Unknown
		//IL_12db: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e4: Expected O, but got Unknown
		//IL_12e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ef: Expected O, but got Unknown
		//IL_12f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fa: Expected O, but got Unknown
		//IL_12fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1305: Expected O, but got Unknown
		//IL_1307: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Expected O, but got Unknown
		//IL_1312: Unknown result type (might be due to invalid IL or missing references)
		//IL_131b: Expected O, but got Unknown
		//IL_131d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1326: Expected O, but got Unknown
		//IL_1328: Unknown result type (might be due to invalid IL or missing references)
		//IL_1331: Expected O, but got Unknown
		//IL_1333: Unknown result type (might be due to invalid IL or missing references)
		//IL_133c: Expected O, but got Unknown
		//IL_133e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1347: Expected O, but got Unknown
		//IL_1349: Unknown result type (might be due to invalid IL or missing references)
		//IL_1352: Expected O, but got Unknown
		//IL_1354: Unknown result type (might be due to invalid IL or missing references)
		//IL_135d: Expected O, but got Unknown
		//IL_135f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1368: Expected O, but got Unknown
		//IL_136a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1373: Expected O, but got Unknown
		//IL_1375: Unknown result type (might be due to invalid IL or missing references)
		//IL_137e: Expected O, but got Unknown
		//IL_1380: Unknown result type (might be due to invalid IL or missing references)
		//IL_1389: Expected O, but got Unknown
		//IL_138b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1394: Expected O, but got Unknown
		//IL_1396: Unknown result type (might be due to invalid IL or missing references)
		//IL_139f: Expected O, but got Unknown
		//IL_13a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13aa: Expected O, but got Unknown
		//IL_13ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b5: Expected O, but got Unknown
		//IL_13b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c0: Expected O, but got Unknown
		//IL_13c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cb: Expected O, but got Unknown
		//IL_13cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d6: Expected O, but got Unknown
		//IL_13d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e1: Expected O, but got Unknown
		//IL_13e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ec: Expected O, but got Unknown
		//IL_13ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f7: Expected O, but got Unknown
		//IL_13f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1402: Expected O, but got Unknown
		//IL_1404: Unknown result type (might be due to invalid IL or missing references)
		//IL_140d: Expected O, but got Unknown
		//IL_140f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1418: Expected O, but got Unknown
		//IL_1489: Unknown result type (might be due to invalid IL or missing references)
		//IL_1493: Expected O, but got Unknown
		//IL_1539: Unknown result type (might be due to invalid IL or missing references)
		//IL_1543: Expected O, but got Unknown
		//IL_155a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1564: Expected O, but got Unknown
		//IL_1565: Unknown result type (might be due to invalid IL or missing references)
		//IL_156f: Expected O, but got Unknown
		//IL_1591: Unknown result type (might be due to invalid IL or missing references)
		//IL_159b: Expected O, but got Unknown
		//IL_159c: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a6: Expected O, but got Unknown
		//IL_160a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1614: Expected O, but got Unknown
		//IL_1620: Unknown result type (might be due to invalid IL or missing references)
		//IL_162a: Expected O, but got Unknown
		//IL_1662: Unknown result type (might be due to invalid IL or missing references)
		//IL_166c: Expected O, but got Unknown
		//IL_1699: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a3: Expected O, but got Unknown
		//IL_16d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16da: Expected O, but got Unknown
		//IL_16f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fb: Expected O, but got Unknown
		//IL_17a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ab: Expected O, but got Unknown
		//IL_17f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Expected O, but got Unknown
		//IL_1825: Unknown result type (might be due to invalid IL or missing references)
		//IL_182f: Expected O, but got Unknown
		//IL_18d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18df: Expected O, but got Unknown
		//IL_1964: Unknown result type (might be due to invalid IL or missing references)
		//IL_196e: Expected O, but got Unknown
		//IL_19a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b0: Expected O, but got Unknown
		//IL_19e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Expected O, but got Unknown
		//IL_1a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a60: Expected O, but got Unknown
		//IL_1b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b10: Expected O, but got Unknown
		//IL_1b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3c: Expected O, but got Unknown
		//IL_1b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9f: Expected O, but got Unknown
		//IL_1bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c02: Expected O, but got Unknown
		//IL_1c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c18: Expected O, but got Unknown
		//IL_1c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c86: Expected O, but got Unknown
		//IL_1cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc8: Expected O, but got Unknown
		//IL_1d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d20: Expected O, but got Unknown
		//IL_1d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d99: Expected O, but got Unknown
		//IL_1d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da4: Expected O, but got Unknown
		//IL_1dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc5: Expected O, but got Unknown
		//IL_1de7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df1: Expected O, but got Unknown
		//IL_1e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3e: Expected O, but got Unknown
		//IL_1e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6a: Expected O, but got Unknown
		//IL_1e6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e75: Expected O, but got Unknown
		//IL_1e97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea1: Expected O, but got Unknown
		//IL_1f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7d: Expected O, but got Unknown
		//IL_2002: Unknown result type (might be due to invalid IL or missing references)
		//IL_200c: Expected O, but got Unknown
		//IL_205a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2064: Expected O, but got Unknown
		//IL_20f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fe: Expected O, but got Unknown
		//IL_210a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2114: Expected O, but got Unknown
		//IL_2115: Unknown result type (might be due to invalid IL or missing references)
		//IL_211f: Expected O, but got Unknown
		//IL_2120: Unknown result type (might be due to invalid IL or missing references)
		//IL_212a: Expected O, but got Unknown
		//IL_212b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2135: Expected O, but got Unknown
		//IL_2136: Unknown result type (might be due to invalid IL or missing references)
		//IL_2140: Expected O, but got Unknown
		//IL_2141: Unknown result type (might be due to invalid IL or missing references)
		//IL_214b: Expected O, but got Unknown
		//IL_214c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2156: Expected O, but got Unknown
		//IL_2157: Unknown result type (might be due to invalid IL or missing references)
		//IL_2161: Expected O, but got Unknown
		//IL_2162: Unknown result type (might be due to invalid IL or missing references)
		//IL_216c: Expected O, but got Unknown
		//IL_216d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2177: Expected O, but got Unknown
		//IL_2178: Unknown result type (might be due to invalid IL or missing references)
		//IL_2182: Expected O, but got Unknown
		//IL_2183: Unknown result type (might be due to invalid IL or missing references)
		//IL_218d: Expected O, but got Unknown
		//IL_218e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2198: Expected O, but got Unknown
		//IL_2d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d37: Expected O, but got Unknown
		//IL_2d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d4e: Expected O, but got Unknown
		//IL_8029: Unknown result type (might be due to invalid IL or missing references)
		//IL_8033: Expected O, but got Unknown
		//IL_9a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_9a27: Expected O, but got Unknown
		//IL_b20f: Unknown result type (might be due to invalid IL or missing references)
		//IL_b219: Expected O, but got Unknown
		//IL_14580: Unknown result type (might be due to invalid IL or missing references)
		//IL_1458a: Expected O, but got Unknown
		//IL_145ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_145b8: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance550 = new Appearance();
		Appearance appearance551 = new Appearance();
		Appearance appearance552 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance425 = new Appearance();
		Appearance appearance426 = new Appearance();
		Appearance appearance427 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance411 = new Appearance();
		Appearance appearance413 = new Appearance();
		Appearance appearance414 = new Appearance();
		Appearance appearance415 = new Appearance();
		Appearance appearance416 = new Appearance();
		Appearance appearance417 = new Appearance();
		Appearance appearance418 = new Appearance();
		Appearance appearance419 = new Appearance();
		Appearance appearance420 = new Appearance();
		Appearance appearance421 = new Appearance();
		Appearance appearance422 = new Appearance();
		Appearance appearance424 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance387 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance391 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance394 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance396 = new Appearance();
		Appearance appearance397 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance384 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance522 = new Appearance();
		Appearance appearance523 = new Appearance();
		Appearance appearance521 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance345 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance358 = new Appearance();
		GridEXLayout grdpaymentdetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance478 = new Appearance();
		Appearance appearance480 = new Appearance();
		Appearance appearance481 = new Appearance();
		Appearance appearance482 = new Appearance();
		Appearance appearance483 = new Appearance();
		Appearance appearance484 = new Appearance();
		GridEXLayout grdCommissionInfo_DesignTimeLayout = new GridEXLayout();
		Appearance appearance368 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance390 = new Appearance();
		Appearance appearance401 = new Appearance();
		Appearance appearance412 = new Appearance();
		Appearance appearance423 = new Appearance();
		Appearance appearance511 = new Appearance();
		Appearance appearance515 = new Appearance();
		Appearance appearance516 = new Appearance();
		Appearance appearance517 = new Appearance();
		Appearance appearance518 = new Appearance();
		Appearance appearance519 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance445 = new Appearance();
		Appearance appearance509 = new Appearance();
		Appearance appearance520 = new Appearance();
		Appearance appearance531 = new Appearance();
		Appearance appearance542 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance434 = new Appearance();
		Appearance appearance479 = new Appearance();
		Appearance appearance490 = new Appearance();
		Appearance appearance524 = new Appearance();
		Appearance appearance525 = new Appearance();
		Appearance appearance526 = new Appearance();
		Appearance appearance507 = new Appearance();
		Appearance appearance527 = new Appearance();
		Appearance appearance528 = new Appearance();
		Appearance appearance529 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance485 = new Appearance();
		Appearance appearance486 = new Appearance();
		Appearance appearance487 = new Appearance();
		Appearance appearance488 = new Appearance();
		Appearance appearance489 = new Appearance();
		Appearance appearance491 = new Appearance();
		Appearance appearance492 = new Appearance();
		Appearance appearance493 = new Appearance();
		Appearance appearance494 = new Appearance();
		Appearance appearance495 = new Appearance();
		Appearance appearance496 = new Appearance();
		Appearance appearance497 = new Appearance();
		Appearance appearance537 = new Appearance();
		Appearance appearance538 = new Appearance();
		Appearance appearance539 = new Appearance();
		Appearance appearance540 = new Appearance();
		Appearance appearance541 = new Appearance();
		Appearance appearance543 = new Appearance();
		Appearance appearance544 = new Appearance();
		Appearance appearance545 = new Appearance();
		Appearance appearance546 = new Appearance();
		Appearance appearance547 = new Appearance();
		Appearance appearance548 = new Appearance();
		Appearance appearance549 = new Appearance();
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
		Appearance appearance465 = new Appearance();
		Appearance appearance466 = new Appearance();
		Appearance appearance467 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance428 = new Appearance();
		Appearance appearance429 = new Appearance();
		Appearance appearance430 = new Appearance();
		Appearance appearance431 = new Appearance();
		Appearance appearance432 = new Appearance();
		Appearance appearance433 = new Appearance();
		Appearance appearance435 = new Appearance();
		Appearance appearance436 = new Appearance();
		Appearance appearance437 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
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
		Appearance appearance530 = new Appearance();
		Appearance appearance532 = new Appearance();
		Appearance appearance533 = new Appearance();
		Appearance appearance534 = new Appearance();
		Appearance appearance535 = new Appearance();
		Appearance appearance536 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance398 = new Appearance();
		Appearance appearance399 = new Appearance();
		Appearance appearance400 = new Appearance();
		Appearance appearance402 = new Appearance();
		Appearance appearance403 = new Appearance();
		Appearance appearance404 = new Appearance();
		Appearance appearance405 = new Appearance();
		Appearance appearance406 = new Appearance();
		Appearance appearance407 = new Appearance();
		Appearance appearance408 = new Appearance();
		Appearance appearance409 = new Appearance();
		Appearance appearance410 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.ExImCommercialInvoice));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance501 = new Appearance();
		Appearance appearance505 = new Appearance();
		Appearance appearance506 = new Appearance();
		Appearance appearance508 = new Appearance();
		Appearance appearance510 = new Appearance();
		Appearance appearance498 = new Appearance();
		Appearance appearance514 = new Appearance();
		Appearance appearance499 = new Appearance();
		Appearance appearance500 = new Appearance();
		Appearance appearance502 = new Appearance();
		Appearance appearance503 = new Appearance();
		Appearance appearance504 = new Appearance();
		Appearance appearance452 = new Appearance();
		Appearance appearance453 = new Appearance();
		Appearance appearance454 = new Appearance();
		Appearance appearance455 = new Appearance();
		Appearance appearance456 = new Appearance();
		Appearance appearance458 = new Appearance();
		Appearance appearance459 = new Appearance();
		Appearance appearance460 = new Appearance();
		Appearance appearance461 = new Appearance();
		Appearance appearance462 = new Appearance();
		Appearance appearance463 = new Appearance();
		Appearance appearance464 = new Appearance();
		GridEXLayout gridDetailHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance446 = new Appearance();
		Appearance appearance457 = new Appearance();
		Appearance appearance468 = new Appearance();
		Appearance appearance512 = new Appearance();
		Appearance appearance513 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance438 = new Appearance();
		Appearance appearance439 = new Appearance();
		Appearance appearance440 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance469 = new Appearance();
		Appearance appearance470 = new Appearance();
		Appearance appearance471 = new Appearance();
		Appearance appearance472 = new Appearance();
		Appearance appearance473 = new Appearance();
		Appearance appearance474 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance475 = new Appearance();
		Appearance appearance476 = new Appearance();
		Appearance appearance477 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance441 = new Appearance();
		Appearance appearance442 = new Appearance();
		Appearance appearance443 = new Appearance();
		Appearance appearance444 = new Appearance();
		Appearance appearance447 = new Appearance();
		Appearance appearance448 = new Appearance();
		Appearance appearance449 = new Appearance();
		Appearance appearance450 = new Appearance();
		Appearance appearance451 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
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
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance258 = new Appearance();
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
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance333 = new Appearance();
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
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel22 = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label85 = new System.Windows.Forms.Label();
		this.GroupDetail = new System.Windows.Forms.GroupBox();
		this.label105 = new System.Windows.Forms.Label();
		this.label104 = new System.Windows.Forms.Label();
		this.label103 = new System.Windows.Forms.Label();
		this.label102 = new System.Windows.Forms.Label();
		this.label101 = new System.Windows.Forms.Label();
		this.label100 = new System.Windows.Forms.Label();
		this.label99 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.label97 = new System.Windows.Forms.Label();
		this.label96 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.CmbFarmingNTrade = new UltraCombo();
		this.label95 = new System.Windows.Forms.Label();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.CmbHSCode = new UltraCombo();
		this.CmbContractSchedule = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label94 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.CmbCropYear = new UltraCombo();
		this.txtpackingdetail = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.txtitemdetail = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtCostMTon = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.combitem = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.combpcktype = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.txtQtyMTon = new System.Windows.Forms.TextBox();
		this.btnCancel = new System.Windows.Forms.Button();
		this.label35 = new System.Windows.Forms.Label();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.CmbPackUom = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbRateUom = new UltraCombo();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label84 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdotheritems = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.btnoCancel = new System.Windows.Forms.Button();
		this.CmbOtherItem = new UltraCombo();
		this.btnoUpdate = new System.Windows.Forms.Button();
		this.label71 = new System.Windows.Forms.Label();
		this.btnoadd = new System.Windows.Forms.Button();
		this.txtoAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.txtoRemarks = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.txtoQty = new System.Windows.Forms.TextBox();
		this.txtoRate = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdpaymentdetail = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.btndadd = new System.Windows.Forms.Button();
		this.label58 = new System.Windows.Forms.Label();
		this.txtPaymentRemarksDetail = new System.Windows.Forms.TextBox();
		this.label82 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.CmbFinInstruments = new UltraCombo();
		this.label80 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.dcmbpaymentterm = new UltraCombo();
		this.label76 = new System.Windows.Forms.Label();
		this.dtxtpercentoftotal = new System.Windows.Forms.TextBox();
		this.btndcancel = new System.Windows.Forms.Button();
		this.label83 = new System.Windows.Forms.Label();
		this.btndupdate = new System.Windows.Forms.Button();
		this.txtBalanceFI = new System.Windows.Forms.TextBox();
		this.dtxtdueday = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.dtxtfcyamount = new System.Windows.Forms.TextBox();
		this.txtBalanceInvoiceAmtPaymentGrid = new System.Windows.Forms.TextBox();
		this.label107 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdCommissionInfo = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.label116 = new System.Windows.Forms.Label();
		this.txtCommissionRemarks = new System.Windows.Forms.TextBox();
		this.btnAddCommission = new System.Windows.Forms.Button();
		this.btnCancelCommission = new System.Windows.Forms.Button();
		this.txtCommRateUom = new System.Windows.Forms.TextBox();
		this.txtCommissionLcyAmount = new System.Windows.Forms.TextBox();
		this.label115 = new System.Windows.Forms.Label();
		this.label114 = new System.Windows.Forms.Label();
		this.txtCommissionExchangeRate = new System.Windows.Forms.TextBox();
		this.txtCommRate = new System.Windows.Forms.TextBox();
		this.txtCommissionAmount = new System.Windows.Forms.TextBox();
		this.cmbCommissionType = new UltraCombo();
		this.label110 = new System.Windows.Forms.Label();
		this.label111 = new System.Windows.Forms.Label();
		this.label112 = new System.Windows.Forms.Label();
		this.label113 = new System.Windows.Forms.Label();
		this.label109 = new System.Windows.Forms.Label();
		this.CmbSalePerson = new UltraCombo();
		this.btnUpdateCommision = new System.Windows.Forms.Button();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label108 = new System.Windows.Forms.Label();
		this.tabOtherCharges = new System.Windows.Forms.TabPage();
		this.grdOtherChargesDetail = new GridEX();
		this.panel27 = new System.Windows.Forms.Panel();
		this.ctrlgrdOtherChargesDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.panel26 = new System.Windows.Forms.Panel();
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
		this.panel11 = new System.Windows.Forms.Panel();
		this.txtPaymentRemarks = new System.Windows.Forms.TextBox();
		this.HeaderGropBox = new System.Windows.Forms.GroupBox();
		this.txtCustomerContractNos = new System.Windows.Forms.TextBox();
		this.label106 = new System.Windows.Forms.Label();
		this.txtDeliveryRemarks = new System.Windows.Forms.TextBox();
		this.label87 = new System.Windows.Forms.Label();
		this.cmbInvoiceNo = new System.Windows.Forms.TextBox();
		this.cmbConsignee = new UltraCombo();
		this.label86 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbCreditAccount = new UltraCombo();
		this.lblCreditAc = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.txtTotalNetAmount = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.cmbdeliverytermnew = new UltraCombo();
		this.txtaddlessamount = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbLoadingPort = new UltraCombo();
		this.label61 = new System.Windows.Forms.Label();
		this.cmbDestinationPort = new UltraCombo();
		this.txtcertificateOfOrigion = new System.Windows.Forms.TextBox();
		this.label56 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtaddlesscommnets = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtLotRef = new System.Windows.Forms.TextBox();
		this.cmbcareiertype = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtcertificate = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.cmbimporterBankNew = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.cmbNotifyParty2 = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.txtIformDate = new System.Windows.Forms.DateTimePicker();
		this.txtIformNo = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.txtLocalAmt = new System.Windows.Forms.TextBox();
		this.txtExRate = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtfcyAmount = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.cmbfcycode = new UltraCombo();
		this.cmbexporterBankNew = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.cmbNotifyParty1 = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.cmbSupCust = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbbranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label88 = new System.Windows.Forms.Label();
		this.cmbproject = new UltraCombo();
		this.cmbPaymentTermsNew = new UltraCombo();
		this.label51 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
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
		this.BtnPrintPL = new System.Windows.Forms.ToolStripButton();
		this.BtnLoad = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateInvoiceNos = new System.Windows.Forms.ToolStripButton();
		this.btnItemDetailLookUp = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnPrint501B = new System.Windows.Forms.ToolStripButton();
		this.btnExportCharges = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
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
		this.panel14 = new System.Windows.Forms.Panel();
		this.label93 = new System.Windows.Forms.Label();
		this.gridDetailHistory = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbFCurrencr = new UltraCombo();
		this.cmbPaymentTerm = new UltraCombo();
		this.combranches = new UltraCombo();
		this.comproject = new UltraCombo();
		this.ultraCombo3 = new UltraCombo();
		this.CmbLCContractor = new UltraCombo();
		this.ultraCombo2 = new UltraCombo();
		this.ultraCombo1 = new UltraCombo();
		this.cmbCarierType = new UltraCombo();
		this.cmbExporterBank = new UltraCombo();
		this.cmbImporterBank = new UltraCombo();
		this.cmbEFormNo = new UltraCombo();
		this.cmbdeliveryterm = new UltraCombo();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.label117 = new System.Windows.Forms.Label();
		this.txtSubContractExpiryDays = new System.Windows.Forms.TextBox();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel17.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel23.SuspendLayout();
		this.GroupDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingNTrade).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHSCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractSchedule).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		this.panel9.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).BeginInit();
		this.panel18.SuspendLayout();
		this.tabPage5.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).BeginInit();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstruments).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dcmbpaymentterm).BeginInit();
		this.panel20.SuspendLayout();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCommissionInfo).BeginInit();
		this.panel25.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).BeginInit();
		this.panel13.SuspendLayout();
		this.tabOtherCharges.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetail).BeginInit();
		this.panel27.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesNameChargeDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.HeaderGropBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbConsignee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliverytermnew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcareiertype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterBankNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfcycode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbexporterBankNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupCust).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTermsNew).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportCompany).BeginInit();
		this.panel12.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1184, 711);
		this.panel4.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.tabControl1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1184, 711);
		this.panel7.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 711);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel17);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 57);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1170, 622);
		this.panel8.TabIndex = 3;
		this.panel17.AutoScroll = true;
		this.panel17.Controls.Add(this.tabControl2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 279);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1170, 343);
		this.panel17.TabIndex = 4;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Controls.Add(this.tabPage6);
		this.tabControl2.Controls.Add(this.tabOtherCharges);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1170, 343);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel22);
		this.tabPage3.Controls.Add(this.GroupDetail);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1162, 314);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Invoice Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel22.Controls.Add(this.panel24);
		this.panel22.Controls.Add(this.panel23);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(3, 141);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1156, 170);
		this.panel22.TabIndex = 5;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 24);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1156, 146);
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1156, 146);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlGrdBar2);
		this.panel23.Controls.Add(this.label85);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1156, 24);
		this.panel23.TabIndex = 1;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1117, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label85.ForeColor = System.Drawing.Color.White;
		this.label85.Location = new System.Drawing.Point(3, 2);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(49, 20);
		this.label85.TabIndex = 27;
		this.label85.Text = "Detail";
		this.GroupDetail.Controls.Add(this.label105);
		this.GroupDetail.Controls.Add(this.label104);
		this.GroupDetail.Controls.Add(this.label103);
		this.GroupDetail.Controls.Add(this.label102);
		this.GroupDetail.Controls.Add(this.label101);
		this.GroupDetail.Controls.Add(this.label100);
		this.GroupDetail.Controls.Add(this.label99);
		this.GroupDetail.Controls.Add(this.label98);
		this.GroupDetail.Controls.Add(this.label97);
		this.GroupDetail.Controls.Add(this.label96);
		this.GroupDetail.Controls.Add(this.label59);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade);
		this.GroupDetail.Controls.Add(this.label95);
		this.GroupDetail.Controls.Add(this.btnAddinGrid);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbHSCode);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractSchedule);
		this.GroupDetail.Controls.Add(this.label6);
		this.GroupDetail.Controls.Add(this.label94);
		this.GroupDetail.Controls.Add(this.label72);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.GroupDetail.Controls.Add(this.txtpackingdetail);
		this.GroupDetail.Controls.Add(this.label39);
		this.GroupDetail.Controls.Add(this.label52);
		this.GroupDetail.Controls.Add(this.txtitemdetail);
		this.GroupDetail.Controls.Add(this.label34);
		this.GroupDetail.Controls.Add(this.txtCostMTon);
		this.GroupDetail.Controls.Add(this.label29);
		this.GroupDetail.Controls.Add(this.txtNoOfBags);
		this.GroupDetail.Controls.Add(this.label30);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.GroupDetail.Controls.Add(this.label32);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.combpcktype);
		this.GroupDetail.Controls.Add(this.label33);
		this.GroupDetail.Controls.Add(this.txtQtyMTon);
		this.GroupDetail.Controls.Add(this.btnCancel);
		this.GroupDetail.Controls.Add(this.label35);
		this.GroupDetail.Controls.Add(this.btnGridUpdate);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.GroupDetail.Controls.Add(this.label36);
		this.GroupDetail.Controls.Add(this.txtAmount);
		this.GroupDetail.Controls.Add(this.label37);
		this.GroupDetail.Controls.Add(this.label38);
		this.GroupDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.GroupDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.GroupDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.GroupDetail.Location = new System.Drawing.Point(3, 26);
		this.GroupDetail.Name = "GroupDetail";
		this.GroupDetail.Size = new System.Drawing.Size(1156, 115);
		this.GroupDetail.TabIndex = 0;
		this.GroupDetail.TabStop = false;
		this.GroupDetail.Text = "Detail";
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label105.ForeColor = System.Drawing.Color.Red;
		this.label105.Location = new System.Drawing.Point(106, 68);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(14, 17);
		this.label105.TabIndex = 605;
		this.label105.Text = "*";
		this.label104.AutoSize = true;
		this.label104.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label104.ForeColor = System.Drawing.Color.Red;
		this.label104.Location = new System.Drawing.Point(248, 68);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(14, 17);
		this.label104.TabIndex = 604;
		this.label104.Text = "*";
		this.label103.AutoSize = true;
		this.label103.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label103.ForeColor = System.Drawing.Color.Red;
		this.label103.Location = new System.Drawing.Point(1138, 22);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(14, 17);
		this.label103.TabIndex = 603;
		this.label103.Text = "*";
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.ForeColor = System.Drawing.Color.Red;
		this.label102.Location = new System.Drawing.Point(1033, 22);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(14, 17);
		this.label102.TabIndex = 602;
		this.label102.Text = "*";
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label101.ForeColor = System.Drawing.Color.Red;
		this.label101.Location = new System.Drawing.Point(950, 22);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(14, 17);
		this.label101.TabIndex = 601;
		this.label101.Text = "*";
		this.label100.AutoSize = true;
		this.label100.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label100.ForeColor = System.Drawing.Color.Red;
		this.label100.Location = new System.Drawing.Point(868, 22);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(14, 17);
		this.label100.TabIndex = 600;
		this.label100.Text = "*";
		this.label99.AutoSize = true;
		this.label99.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label99.ForeColor = System.Drawing.Color.Red;
		this.label99.Location = new System.Drawing.Point(752, 22);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(14, 17);
		this.label99.TabIndex = 599;
		this.label99.Text = "*";
		this.label98.AutoSize = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.ForeColor = System.Drawing.Color.Red;
		this.label98.Location = new System.Drawing.Point(651, 22);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(14, 17);
		this.label98.TabIndex = 598;
		this.label98.Text = "*";
		this.label97.AutoSize = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label97.ForeColor = System.Drawing.Color.Red;
		this.label97.Location = new System.Drawing.Point(497, 22);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(14, 17);
		this.label97.TabIndex = 597;
		this.label97.Text = "*";
		this.label96.AutoSize = true;
		this.label96.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label96.ForeColor = System.Drawing.Color.Red;
		this.label96.Location = new System.Drawing.Point(393, 22);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(14, 17);
		this.label96.TabIndex = 596;
		this.label96.Text = "*";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.Red;
		this.label59.Location = new System.Drawing.Point(248, 22);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(14, 17);
		this.label59.TabIndex = 595;
		this.label59.Text = "*";
		this.CmbFarmingNTrade.AllowNull = (DefaultableBoolean)1;
		this.CmbFarmingNTrade.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFarmingNTrade.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFarmingNTrade.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFarmingNTrade.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Location = new System.Drawing.Point(628, 83);
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Name = "CmbFarmingNTrade";
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Size = new System.Drawing.Size(186, 26);
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).TabIndex = 13;
		this.label95.AutoSize = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.ForeColor = System.Drawing.Color.Black;
		this.label95.Location = new System.Drawing.Point(628, 64);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(95, 15);
		this.label95.TabIndex = 501;
		this.label95.Text = "Farming N Trade";
		this.btnAddinGrid.BackColor = System.Drawing.Color.Teal;
		this.btnAddinGrid.FlatAppearance.BorderSize = 0;
		this.btnAddinGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddinGrid.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.ForeColor = System.Drawing.Color.White;
		this.btnAddinGrid.Location = new System.Drawing.Point(1016, 85);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(31, 26);
		this.btnAddinGrid.TabIndex = 15;
		this.btnAddinGrid.Text = "+";
		this.btnAddinGrid.UseVisualStyleBackColor = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.CmbHSCode.AllowNull = (DefaultableBoolean)1;
		this.CmbHSCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbHSCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbHSCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance550).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance550).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance550;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance551).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance551).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance551).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance551).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbHSCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance551;
		((AppearanceBase)appearance552).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance552;
		((SpecialBoxBase)((UltraGridBase)this.CmbHSCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance224).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance235).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbHSCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbHSCode).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbHSCode).Location = new System.Drawing.Point(268, 86);
		((System.Windows.Forms.Control)(object)this.CmbHSCode).Name = "CmbHSCode";
		((System.Windows.Forms.Control)(object)this.CmbHSCode).Size = new System.Drawing.Size(143, 25);
		((System.Windows.Forms.Control)(object)this.CmbHSCode).TabIndex = 11;
		this.CmbContractSchedule.AllowNull = (DefaultableBoolean)1;
		this.CmbContractSchedule.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractSchedule.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractSchedule.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Appearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractSchedule).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractSchedule).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance203).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance204).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractSchedule).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractSchedule).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbContractSchedule).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractSchedule.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractSchedule).Location = new System.Drawing.Point(122, 86);
		((System.Windows.Forms.Control)(object)this.CmbContractSchedule).Name = "CmbContractSchedule";
		((System.Windows.Forms.Control)(object)this.CmbContractSchedule).Size = new System.Drawing.Size(143, 25);
		((System.Windows.Forms.Control)(object)this.CmbContractSchedule).TabIndex = 10;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(122, 67);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(104, 15);
		this.label6.TabIndex = 497;
		this.label6.Text = "Contract Schedule";
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.Location = new System.Drawing.Point(517, 21);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(46, 15);
		this.label94.TabIndex = 493;
		this.label94.Text = "Job Lot";
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(413, 21);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(54, 15);
		this.label72.TabIndex = 493;
		this.label72.Text = "CropYear";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance425).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance425).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance425;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance426).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance426).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance426).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance426).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance426;
		((AppearanceBase)appearance427).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance427;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance336).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance336).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance336).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance337).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance338).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance338).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance338;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance339;
		((AppearanceBase)appearance340).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance340).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance340;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance341).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance341).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance341).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance341).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance341).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance341;
		((AppearanceBase)appearance342).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance342;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance343).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance343).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance344;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(517, 39);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(150, 25);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance411).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance411).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance411;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance413).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance413).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance413).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance413).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance413;
		((AppearanceBase)appearance414).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance414;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance415).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance415).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance415).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance415).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance415;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance416).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance416).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance416;
		((AppearanceBase)appearance417).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance417).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance417;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance418).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance418;
		((AppearanceBase)appearance419).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance419).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance419;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance420).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance420).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance420).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance420).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance420).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance420;
		((AppearanceBase)appearance421).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance421;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance422).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance422).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance422;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance424).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance424;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(413, 39);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(101, 25);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 2;
		this.txtpackingdetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpackingdetail.Location = new System.Drawing.Point(817, 87);
		this.txtpackingdetail.Name = "txtpackingdetail";
		this.txtpackingdetail.Size = new System.Drawing.Size(195, 22);
		this.txtpackingdetail.TabIndex = 14;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(817, 67);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(83, 15);
		this.label39.TabIndex = 491;
		this.label39.Text = "Packing Detail";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(268, 67);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(53, 15);
		this.label52.TabIndex = 489;
		this.label52.Text = "HS Code";
		this.txtitemdetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtitemdetail.Location = new System.Drawing.Point(413, 87);
		this.txtitemdetail.Name = "txtitemdetail";
		this.txtitemdetail.Size = new System.Drawing.Size(212, 22);
		this.txtitemdetail.TabIndex = 12;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(413, 67);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(66, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Item Detail";
		this.txtCostMTon.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCostMTon.Location = new System.Drawing.Point(963, 40);
		this.txtCostMTon.Name = "txtCostMTon";
		this.txtCostMTon.ReadOnly = true;
		this.txtCostMTon.Size = new System.Drawing.Size(84, 22);
		this.txtCostMTon.TabIndex = 7;
		this.txtCostMTon.Text = "0";
		this.txtCostMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCostMTon.TextChanged += new System.EventHandler(txtCostMTon_TextChanged);
		this.txtCostMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCostMTon_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(963, 21);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(69, 15);
		this.label29.TabIndex = 487;
		this.label29.Text = "Cost/M.Ton";
		this.txtNoOfBags.BackColor = System.Drawing.Color.White;
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(888, 40);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.ReadOnly = true;
		this.txtNoOfBags.Size = new System.Drawing.Size(73, 22);
		this.txtNoOfBags.TabIndex = 6;
		this.txtNoOfBags.Text = "0";
		this.txtNoOfBags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(888, 21);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(67, 15);
		this.label30.TabIndex = 485;
		this.label30.Text = "No Of Bags";
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance372).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance372).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance372;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance373).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance373).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance373).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance373;
		((AppearanceBase)appearance374).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance374;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance375).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance375).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance375).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance375).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance375;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance376).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance376).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance376;
		((AppearanceBase)appearance377).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance377).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance378;
		((AppearanceBase)appearance380).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance380).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance380;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance381).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance381).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance381).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance381).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance381;
		((AppearanceBase)appearance6).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(3, 39);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(262, 25);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(3, 21);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(67, 15);
		this.label32.TabIndex = 470;
		this.label32.Text = "Item Name";
		this.combpcktype.AllowNull = (DefaultableBoolean)1;
		this.combpcktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpcktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpcktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance385).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance385).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpcktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance385;
		((UltraGridBase)this.combpcktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance386).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance386).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance386).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance386).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance386;
		((AppearanceBase)appearance387).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance387;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance388).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance388).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance388).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance388;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance389).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance389).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance389;
		((AppearanceBase)appearance391).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance391).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance391;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance392).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance392;
		((AppearanceBase)appearance393).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance393).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance393;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance394).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance394).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance394).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance394).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance394;
		((AppearanceBase)appearance395).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance395;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance396).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance396).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance396;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance397).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance397;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpcktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpcktype).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpcktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpcktype).Location = new System.Drawing.Point(268, 39);
		((System.Windows.Forms.Control)(object)this.combpcktype).Name = "combpcktype";
		((System.Windows.Forms.Control)(object)this.combpcktype).Size = new System.Drawing.Size(143, 25);
		((System.Windows.Forms.Control)(object)this.combpcktype).TabIndex = 1;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(268, 21);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(77, 15);
		this.label33.TabIndex = 476;
		this.label33.Text = "Packing Type";
		this.txtQtyMTon.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyMTon.Location = new System.Drawing.Point(669, 40);
		this.txtQtyMTon.Name = "txtQtyMTon";
		this.txtQtyMTon.Size = new System.Drawing.Size(97, 22);
		this.txtQtyMTon.TabIndex = 4;
		this.txtQtyMTon.Text = "0";
		this.txtQtyMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyMTon.TextChanged += new System.EventHandler(txtQtyMTon_TextChanged);
		this.txtQtyMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyMTon_KeyPress);
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderSize = 0;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(1085, 85);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(67, 26);
		this.btnCancel.TabIndex = 16;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnCancel.Leave += new System.EventHandler(btnCancel_Leave);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(667, 21);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(65, 15);
		this.label35.TabIndex = 483;
		this.label35.Text = "Qty/M.Ton";
		this.btnGridUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
		this.btnGridUpdate.FlatAppearance.BorderSize = 0;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.ForeColor = System.Drawing.Color.White;
		this.btnGridUpdate.Location = new System.Drawing.Point(1016, 85);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(67, 26);
		this.btnGridUpdate.TabIndex = 15;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = false;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance279).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance279).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance290).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance290;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance382).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance382).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance382).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance382).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance382;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance383).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance383).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance383;
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance384).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance384;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance179).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(767, 39);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(117, 25);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(combitempck_TextChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(764, 21);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(57, 15);
		this.label36.TabIndex = 474;
		this.label36.Text = "Pack Size";
		this.txtAmount.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(3, 87);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(117, 22);
		this.txtAmount.TabIndex = 9;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(3, 67);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(51, 15);
		this.label37.TabIndex = 485;
		this.label37.Text = "Amount";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(1050, 21);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(61, 15);
		this.label38.TabIndex = 479;
		this.label38.Text = "Cost UOM";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance522).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance522).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance522;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance523).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance523).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance523).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance523).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance523;
		((AppearanceBase)appearance521).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance521;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(1050, 39);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(103, 25);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label84);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1156, 23);
		this.panel9.TabIndex = 0;
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.ForeColor = System.Drawing.Color.White;
		this.label84.Location = new System.Drawing.Point(3, 2);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(95, 16);
		this.label84.TabIndex = 26;
		this.label84.Text = "Invoice Detail";
		this.tabPage4.Controls.Add(this.panel2);
		this.tabPage4.Controls.Add(this.panel19);
		this.tabPage4.Controls.Add(this.panel18);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1162, 314);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Items";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 78);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1156, 233);
		this.panel2.TabIndex = 2;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdotheritems).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1156, 233);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 0;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).KeyDown += new System.Windows.Forms.KeyEventHandler(grdotheritems_KeyDown);
		this.panel19.Controls.Add(this.btnoCancel);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherItem);
		this.panel19.Controls.Add(this.btnoUpdate);
		this.panel19.Controls.Add(this.label71);
		this.panel19.Controls.Add(this.btnoadd);
		this.panel19.Controls.Add(this.txtoAmount);
		this.panel19.Controls.Add(this.label14);
		this.panel19.Controls.Add(this.label70);
		this.panel19.Controls.Add(this.txtoRemarks);
		this.panel19.Controls.Add(this.label63);
		this.panel19.Controls.Add(this.txtoQty);
		this.panel19.Controls.Add(this.txtoRate);
		this.panel19.Controls.Add(this.label15);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(3, 26);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1156, 52);
		this.panel19.TabIndex = 1;
		this.btnoCancel.BackColor = System.Drawing.Color.Teal;
		this.btnoCancel.FlatAppearance.BorderSize = 0;
		this.btnoCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoCancel.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnoCancel.ForeColor = System.Drawing.Color.White;
		this.btnoCancel.Location = new System.Drawing.Point(968, 24);
		this.btnoCancel.Name = "btnoCancel";
		this.btnoCancel.Size = new System.Drawing.Size(70, 24);
		this.btnoCancel.TabIndex = 7;
		this.btnoCancel.Text = "Cancel";
		this.btnoCancel.UseVisualStyleBackColor = false;
		this.btnoCancel.Visible = false;
		this.btnoCancel.Click += new System.EventHandler(btnoCancel_Click);
		this.btnoCancel.Leave += new System.EventHandler(btnoCancel_Leave);
		this.CmbOtherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance345).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance345).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance347).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance347).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance347).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance347;
		((AppearanceBase)appearance348).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance348;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance349).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance349).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance349).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance349;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance350).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance350).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance350;
		((AppearanceBase)appearance351).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance351).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance351;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance352).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance352;
		((AppearanceBase)appearance353).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance353).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance353;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance354).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance354).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance354).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance354).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance354;
		((AppearanceBase)appearance355).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance355;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance356).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance356).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance356;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance358).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance358;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Location = new System.Drawing.Point(5, 24);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Name = "CmbOtherItem";
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Size = new System.Drawing.Size(198, 25);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TextChanged += new System.EventHandler(CmbOtherItem_TextChanged);
		this.btnoUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnoUpdate.FlatAppearance.BorderSize = 0;
		this.btnoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoUpdate.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnoUpdate.ForeColor = System.Drawing.Color.White;
		this.btnoUpdate.Location = new System.Drawing.Point(899, 24);
		this.btnoUpdate.Name = "btnoUpdate";
		this.btnoUpdate.Size = new System.Drawing.Size(67, 24);
		this.btnoUpdate.TabIndex = 6;
		this.btnoUpdate.Text = "Update";
		this.btnoUpdate.UseVisualStyleBackColor = false;
		this.btnoUpdate.Visible = false;
		this.btnoUpdate.Click += new System.EventHandler(btnoUpdate_Click);
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(8, 5);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(67, 15);
		this.label71.TabIndex = 504;
		this.label71.Text = "Item Name";
		this.btnoadd.BackColor = System.Drawing.Color.Teal;
		this.btnoadd.FlatAppearance.BorderSize = 0;
		this.btnoadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoadd.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnoadd.ForeColor = System.Drawing.Color.White;
		this.btnoadd.Location = new System.Drawing.Point(861, 24);
		this.btnoadd.Name = "btnoadd";
		this.btnoadd.Size = new System.Drawing.Size(36, 24);
		this.btnoadd.TabIndex = 5;
		this.btnoadd.Text = "+";
		this.btnoadd.UseVisualStyleBackColor = false;
		this.btnoadd.Click += new System.EventHandler(btnoadd_Click);
		this.btnoadd.Leave += new System.EventHandler(btnoadd_Leave);
		this.txtoAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoAmount.Location = new System.Drawing.Point(392, 25);
		this.txtoAmount.Name = "txtoAmount";
		this.txtoAmount.ReadOnly = true;
		this.txtoAmount.Size = new System.Drawing.Size(101, 22);
		this.txtoAmount.TabIndex = 3;
		this.txtoAmount.Text = "0";
		this.txtoAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(499, 5);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(52, 15);
		this.label14.TabIndex = 508;
		this.label14.Text = "Remarks";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(393, 5);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(51, 15);
		this.label70.TabIndex = 506;
		this.label70.Text = "Amount";
		this.txtoRemarks.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRemarks.Location = new System.Drawing.Point(494, 25);
		this.txtoRemarks.Name = "txtoRemarks";
		this.txtoRemarks.Size = new System.Drawing.Size(364, 22);
		this.txtoRemarks.TabIndex = 4;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(308, 5);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(30, 15);
		this.label63.TabIndex = 507;
		this.label63.Text = "Rate";
		this.txtoQty.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoQty.Location = new System.Drawing.Point(205, 25);
		this.txtoQty.Name = "txtoQty";
		this.txtoQty.Size = new System.Drawing.Size(101, 22);
		this.txtoQty.TabIndex = 1;
		this.txtoQty.Text = "0";
		this.txtoQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtoQty.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.txtoRate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRate.Location = new System.Drawing.Point(307, 25);
		this.txtoRate.Name = "txtoRate";
		this.txtoRate.Size = new System.Drawing.Size(84, 22);
		this.txtoRate.TabIndex = 2;
		this.txtoRate.Text = "0";
		this.txtoRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtoRate.TextChanged += new System.EventHandler(txtoRate_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(208, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(26, 15);
		this.label15.TabIndex = 505;
		this.label15.Text = "Qty";
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar3);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 3);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1156, 23);
		this.panel18.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1116, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdotheritems;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar3.TabIndex = 26;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.tabPage5.Controls.Add(this.panel6);
		this.tabPage5.Controls.Add(this.panel21);
		this.tabPage5.Controls.Add(this.panel20);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(1162, 314);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Payment Detail";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdpaymentdetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 120);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1156, 191);
		this.panel6.TabIndex = 2;
		this.grdpaymentdetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdpaymentdetail).BackColor = System.Drawing.Color.White;
		grdpaymentdetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdpaymentdetail.DesignTimeLayout = grdpaymentdetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdpaymentdetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Name = "grdpaymentdetail";
		((ScrollableBase)this.grdpaymentdetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdpaymentdetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdpaymentdetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Size = new System.Drawing.Size(1156, 191);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).TabIndex = 0;
		this.grdpaymentdetail.TotalRow = (InheritableBoolean)1;
		this.grdpaymentdetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdpaymentdetail).VisualStyle = (VisualStyle)4;
		this.grdpaymentdetail.ColumnButtonClick += new ColumnActionEventHandler(grdpaymentdetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).DoubleClick += new System.EventHandler(grdpaymentdetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdpaymentdetail_KeyDown);
		this.panel21.Controls.Add(this.btndadd);
		this.panel21.Controls.Add(this.label58);
		this.panel21.Controls.Add(this.txtPaymentRemarksDetail);
		this.panel21.Controls.Add(this.label82);
		this.panel21.Controls.Add(this.label75);
		this.panel21.Controls.Add(this.label81);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinInstruments);
		this.panel21.Controls.Add(this.label80);
		this.panel21.Controls.Add(this.label74);
		this.panel21.Controls.Add(this.label79);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.dcmbpaymentterm);
		this.panel21.Controls.Add(this.label76);
		this.panel21.Controls.Add(this.dtxtpercentoftotal);
		this.panel21.Controls.Add(this.btndcancel);
		this.panel21.Controls.Add(this.label83);
		this.panel21.Controls.Add(this.btndupdate);
		this.panel21.Controls.Add(this.txtBalanceFI);
		this.panel21.Controls.Add(this.dtxtdueday);
		this.panel21.Controls.Add(this.label78);
		this.panel21.Controls.Add(this.label77);
		this.panel21.Controls.Add(this.dtxtfcyamount);
		this.panel21.Controls.Add(this.txtBalanceInvoiceAmtPaymentGrid);
		this.panel21.Controls.Add(this.label107);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(3, 26);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1156, 94);
		this.panel21.TabIndex = 0;
		this.btndadd.BackColor = System.Drawing.Color.Teal;
		this.btndadd.FlatAppearance.BorderSize = 0;
		this.btndadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btndadd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btndadd.ForeColor = System.Drawing.Color.White;
		this.btndadd.Location = new System.Drawing.Point(878, 68);
		this.btndadd.Name = "btndadd";
		this.btndadd.Size = new System.Drawing.Size(31, 22);
		this.btndadd.TabIndex = 7;
		this.btndadd.Text = "+";
		this.btndadd.UseVisualStyleBackColor = false;
		this.btndadd.Click += new System.EventHandler(btndadd_Click);
		this.btndadd.Leave += new System.EventHandler(btndadd_Leave);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(6, 51);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(52, 15);
		this.label58.TabIndex = 593;
		this.label58.Text = "Remarks";
		this.txtPaymentRemarksDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPaymentRemarksDetail.Location = new System.Drawing.Point(6, 68);
		this.txtPaymentRemarksDetail.Name = "txtPaymentRemarksDetail";
		this.txtPaymentRemarksDetail.Size = new System.Drawing.Size(869, 22);
		this.txtPaymentRemarksDetail.TabIndex = 6;
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.ForeColor = System.Drawing.Color.Red;
		this.label82.Location = new System.Drawing.Point(873, 5);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(14, 17);
		this.label82.TabIndex = 591;
		this.label82.Text = "*";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(288, 6);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(140, 15);
		this.label75.TabIndex = 508;
		this.label75.Text = "Financial Instruments No";
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.Red;
		this.label81.Location = new System.Drawing.Point(791, 5);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(14, 17);
		this.label81.TabIndex = 590;
		this.label81.Text = "*";
		this.CmbFinInstruments.AllowNull = (DefaultableBoolean)1;
		this.CmbFinInstruments.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFinInstruments.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinInstruments.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance359).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance359).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Appearance = (AppearanceBase)(object)appearance359;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance360).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance360).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance360).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstruments).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance360;
		((AppearanceBase)appearance361).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance361;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinInstruments).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance362).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance362).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance362).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance362;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance363).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance363).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance363;
		((AppearanceBase)appearance364).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance364).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance364;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance365).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance365;
		((AppearanceBase)appearance366).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance366).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance366;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance367).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance367).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance367).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance367).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance367;
		((AppearanceBase)appearance369).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance369;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance370).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance370).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance370;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance371).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance371;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinInstruments).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).Location = new System.Drawing.Point(288, 23);
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).Name = "CmbFinInstruments";
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).Size = new System.Drawing.Size(300, 25);
		((System.Windows.Forms.Control)(object)this.CmbFinInstruments).TabIndex = 1;
		this.CmbFinInstruments.ValueChanged += new System.EventHandler(dcmbfino_ValueChanged);
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.ForeColor = System.Drawing.Color.Red;
		this.label80.Location = new System.Drawing.Point(101, 5);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(14, 17);
		this.label80.TabIndex = 589;
		this.label80.Text = "*";
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.Location = new System.Drawing.Point(723, 6);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(60, 15);
		this.label74.TabIndex = 509;
		this.label74.Text = "% of Total";
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.ForeColor = System.Drawing.Color.Red;
		this.label79.Location = new System.Drawing.Point(434, 5);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(14, 17);
		this.label79.TabIndex = 588;
		this.label79.Text = "*";
		this.dcmbpaymentterm.AllowNull = (DefaultableBoolean)1;
		this.dcmbpaymentterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.dcmbpaymentterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.dcmbpaymentterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance312).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance312).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance312).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance323).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance323;
		((SpecialBoxBase)((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance335).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance335).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance335).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance335;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance346).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance346;
		((AppearanceBase)appearance357).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance357).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance357;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance478).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance478;
		((AppearanceBase)appearance480).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance480).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance480;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance481).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance481).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance481).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance481).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance481).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance481;
		((AppearanceBase)appearance482).TextHAlignAsString = "Left";
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance482;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance483).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance483).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance483;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance484).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance484;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.dcmbpaymentterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).Location = new System.Drawing.Point(6, 23);
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).Name = "dcmbpaymentterm";
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).Size = new System.Drawing.Size(280, 25);
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.dcmbpaymentterm).TextChanged += new System.EventHandler(CmbPaymentTermDetail_TextChanged);
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(6, 6);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(84, 15);
		this.label76.TabIndex = 511;
		this.label76.Text = "Payment Term";
		this.dtxtpercentoftotal.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtxtpercentoftotal.Location = new System.Drawing.Point(723, 24);
		this.dtxtpercentoftotal.Name = "dtxtpercentoftotal";
		this.dtxtpercentoftotal.Size = new System.Drawing.Size(80, 22);
		this.dtxtpercentoftotal.TabIndex = 3;
		this.dtxtpercentoftotal.Text = "0";
		this.dtxtpercentoftotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.dtxtpercentoftotal.Leave += new System.EventHandler(dtxtpercentoftotal_TextChanged);
		this.btndcancel.BackColor = System.Drawing.Color.Teal;
		this.btndcancel.FlatAppearance.BorderSize = 0;
		this.btndcancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btndcancel.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btndcancel.ForeColor = System.Drawing.Color.White;
		this.btndcancel.Location = new System.Drawing.Point(941, 68);
		this.btndcancel.Name = "btndcancel";
		this.btndcancel.Size = new System.Drawing.Size(55, 22);
		this.btndcancel.TabIndex = 9;
		this.btndcancel.Text = "Cancel";
		this.btndcancel.UseVisualStyleBackColor = false;
		this.btndcancel.Visible = false;
		this.btndcancel.Click += new System.EventHandler(btndcancel_Click);
		this.btndcancel.Leave += new System.EventHandler(btndcancel_Leave);
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(590, 6);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(48, 15);
		this.label83.TabIndex = 509;
		this.label83.Text = "Balance";
		this.btndupdate.BackColor = System.Drawing.Color.Teal;
		this.btndupdate.FlatAppearance.BorderSize = 0;
		this.btndupdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btndupdate.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btndupdate.ForeColor = System.Drawing.Color.White;
		this.btndupdate.Location = new System.Drawing.Point(878, 68);
		this.btndupdate.Name = "btndupdate";
		this.btndupdate.Size = new System.Drawing.Size(60, 22);
		this.btndupdate.TabIndex = 8;
		this.btndupdate.Text = "Update";
		this.btndupdate.UseVisualStyleBackColor = false;
		this.btndupdate.Visible = false;
		this.btndupdate.Click += new System.EventHandler(btndupdate_Click);
		this.txtBalanceFI.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceFI.Location = new System.Drawing.Point(590, 24);
		this.txtBalanceFI.Name = "txtBalanceFI";
		this.txtBalanceFI.ReadOnly = true;
		this.txtBalanceFI.Size = new System.Drawing.Size(130, 22);
		this.txtBalanceFI.TabIndex = 2;
		this.txtBalanceFI.TabStop = false;
		this.txtBalanceFI.Text = "0";
		this.txtBalanceFI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalanceFI.Leave += new System.EventHandler(dtxtpercentoftotal_TextChanged);
		this.dtxtdueday.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtxtdueday.Location = new System.Drawing.Point(936, 24);
		this.dtxtdueday.Name = "dtxtdueday";
		this.dtxtdueday.Size = new System.Drawing.Size(60, 22);
		this.dtxtdueday.TabIndex = 5;
		this.dtxtdueday.Text = "0";
		this.dtxtdueday.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(936, 6);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(58, 15);
		this.label78.TabIndex = 515;
		this.label78.Text = "Due Days";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(803, 6);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(72, 15);
		this.label77.TabIndex = 513;
		this.label77.Text = "Fcy Amount";
		this.dtxtfcyamount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtxtfcyamount.Location = new System.Drawing.Point(804, 24);
		this.dtxtfcyamount.Name = "dtxtfcyamount";
		this.dtxtfcyamount.Size = new System.Drawing.Size(130, 22);
		this.dtxtfcyamount.TabIndex = 4;
		this.dtxtfcyamount.Text = "0";
		this.dtxtfcyamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.dtxtfcyamount.Leave += new System.EventHandler(dtxtfcyamount_TextChanged);
		this.txtBalanceInvoiceAmtPaymentGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceInvoiceAmtPaymentGrid.Enabled = false;
		this.txtBalanceInvoiceAmtPaymentGrid.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceInvoiceAmtPaymentGrid.Location = new System.Drawing.Point(1018, 29);
		this.txtBalanceInvoiceAmtPaymentGrid.Name = "txtBalanceInvoiceAmtPaymentGrid";
		this.txtBalanceInvoiceAmtPaymentGrid.Size = new System.Drawing.Size(134, 22);
		this.txtBalanceInvoiceAmtPaymentGrid.TabIndex = 30;
		this.label107.BackColor = System.Drawing.Color.LightGray;
		this.label107.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label107.Location = new System.Drawing.Point(1018, 5);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(134, 22);
		this.label107.TabIndex = 577;
		this.label107.Text = "Balance";
		this.label107.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 3);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1156, 23);
		this.panel20.TabIndex = 0;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1116, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdpaymentdetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar4.TabIndex = 27;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdCommissionInfo);
		this.tabPage6.Controls.Add(this.panel25);
		this.tabPage6.Controls.Add(this.panel13);
		this.tabPage6.Location = new System.Drawing.Point(4, 25);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(1162, 314);
		this.tabPage6.TabIndex = 3;
		this.tabPage6.Text = "Commission Info";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.grdCommissionInfo.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdCommissionInfo).BackColor = System.Drawing.Color.White;
		grdCommissionInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCommissionInfo.DesignTimeLayout = grdCommissionInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCommissionInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Location = new System.Drawing.Point(0, 73);
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Name = "grdCommissionInfo";
		((ScrollableBase)this.grdCommissionInfo).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdCommissionInfo).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdCommissionInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).Size = new System.Drawing.Size(1162, 241);
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).TabIndex = 1;
		this.grdCommissionInfo.TotalRow = (InheritableBoolean)1;
		this.grdCommissionInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCommissionInfo).VisualStyle = (VisualStyle)4;
		this.grdCommissionInfo.ColumnButtonClick += new ColumnActionEventHandler(grdCommissionInfo_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdCommissionInfo).DoubleClick += new System.EventHandler(grdCommissionInfo_DoubleClick);
		this.panel25.Controls.Add(this.label116);
		this.panel25.Controls.Add(this.txtCommissionRemarks);
		this.panel25.Controls.Add(this.btnAddCommission);
		this.panel25.Controls.Add(this.btnCancelCommission);
		this.panel25.Controls.Add(this.txtCommRateUom);
		this.panel25.Controls.Add(this.txtCommissionLcyAmount);
		this.panel25.Controls.Add(this.label115);
		this.panel25.Controls.Add(this.label114);
		this.panel25.Controls.Add(this.txtCommissionExchangeRate);
		this.panel25.Controls.Add(this.txtCommRate);
		this.panel25.Controls.Add(this.txtCommissionAmount);
		this.panel25.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommissionType);
		this.panel25.Controls.Add(this.label110);
		this.panel25.Controls.Add(this.label111);
		this.panel25.Controls.Add(this.label112);
		this.panel25.Controls.Add(this.label113);
		this.panel25.Controls.Add(this.label109);
		this.panel25.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalePerson);
		this.panel25.Controls.Add(this.btnUpdateCommision);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 21);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1162, 52);
		this.panel25.TabIndex = 2;
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label116.Location = new System.Drawing.Point(789, 5);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(52, 15);
		this.label116.TabIndex = 604;
		this.label116.Text = "Remarks";
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(792, 25);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(250, 23);
		this.txtCommissionRemarks.TabIndex = 7;
		this.btnAddCommission.BackColor = System.Drawing.Color.Teal;
		this.btnAddCommission.FlatAppearance.BorderSize = 0;
		this.btnAddCommission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddCommission.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddCommission.ForeColor = System.Drawing.Color.White;
		this.btnAddCommission.Location = new System.Drawing.Point(1045, 25);
		this.btnAddCommission.Name = "btnAddCommission";
		this.btnAddCommission.Size = new System.Drawing.Size(31, 22);
		this.btnAddCommission.TabIndex = 8;
		this.btnAddCommission.Text = "+";
		this.btnAddCommission.UseVisualStyleBackColor = false;
		this.btnAddCommission.Click += new System.EventHandler(btnAddCommission_Click);
		this.btnCancelCommission.BackColor = System.Drawing.Color.Teal;
		this.btnCancelCommission.FlatAppearance.BorderSize = 0;
		this.btnCancelCommission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelCommission.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelCommission.ForeColor = System.Drawing.Color.White;
		this.btnCancelCommission.Location = new System.Drawing.Point(1104, 25);
		this.btnCancelCommission.Name = "btnCancelCommission";
		this.btnCancelCommission.Size = new System.Drawing.Size(55, 22);
		this.btnCancelCommission.TabIndex = 10;
		this.btnCancelCommission.Text = "Cancel";
		this.btnCancelCommission.UseVisualStyleBackColor = false;
		this.btnCancelCommission.Visible = false;
		this.btnCancelCommission.Click += new System.EventHandler(btnCancelCommission_Click);
		this.txtCommRateUom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRateUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommRateUom.Location = new System.Drawing.Point(390, 25);
		this.txtCommRateUom.Name = "txtCommRateUom";
		this.txtCommRateUom.Size = new System.Drawing.Size(87, 23);
		this.txtCommRateUom.TabIndex = 3;
		this.txtCommRateUom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommRateUom.TextChanged += new System.EventHandler(txtCommRateUom_TextChanged);
		this.txtCommRateUom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommRateUom_KeyPress);
		this.txtCommissionLcyAmount.BackColor = System.Drawing.Color.White;
		this.txtCommissionLcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionLcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionLcyAmount.Location = new System.Drawing.Point(684, 25);
		this.txtCommissionLcyAmount.Name = "txtCommissionLcyAmount";
		this.txtCommissionLcyAmount.ReadOnly = true;
		this.txtCommissionLcyAmount.Size = new System.Drawing.Size(107, 23);
		this.txtCommissionLcyAmount.TabIndex = 6;
		this.txtCommissionLcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label115.AutoSize = true;
		this.label115.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label115.Location = new System.Drawing.Point(684, 5);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(72, 15);
		this.label115.TabIndex = 598;
		this.label115.Text = "Lcy Amount";
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label114.Location = new System.Drawing.Point(580, 5);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(84, 15);
		this.label114.TabIndex = 597;
		this.label114.Text = "Exchange Rate";
		this.txtCommissionExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionExchangeRate.Enabled = false;
		this.txtCommissionExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionExchangeRate.Location = new System.Drawing.Point(580, 25);
		this.txtCommissionExchangeRate.Name = "txtCommissionExchangeRate";
		this.txtCommissionExchangeRate.Size = new System.Drawing.Size(103, 23);
		this.txtCommissionExchangeRate.TabIndex = 5;
		this.txtCommissionExchangeRate.Text = "0";
		this.txtCommissionExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommRate.Location = new System.Drawing.Point(312, 25);
		this.txtCommRate.Name = "txtCommRate";
		this.txtCommRate.Size = new System.Drawing.Size(77, 23);
		this.txtCommRate.TabIndex = 2;
		this.txtCommRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommRate.TextChanged += new System.EventHandler(txtCommRate_TextChanged);
		this.txtCommRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommRate_KeyPress);
		this.txtCommissionAmount.BackColor = System.Drawing.Color.White;
		this.txtCommissionAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionAmount.Location = new System.Drawing.Point(478, 25);
		this.txtCommissionAmount.Name = "txtCommissionAmount";
		this.txtCommissionAmount.ReadOnly = true;
		this.txtCommissionAmount.Size = new System.Drawing.Size(101, 23);
		this.txtCommissionAmount.TabIndex = 4;
		this.txtCommissionAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommissionAmount.TextChanged += new System.EventHandler(txtCommissionAmount_TextChanged);
		this.cmbCommissionType.AllowNull = (DefaultableBoolean)1;
		this.cmbCommissionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommissionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommissionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance368).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance368;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance379).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance379).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance379).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance379).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance379;
		((AppearanceBase)appearance390).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance390;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance401).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance401).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance401).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance401).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance401;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance412).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance412).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance412;
		((AppearanceBase)appearance423).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance423).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance423;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance511).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance511;
		((AppearanceBase)appearance515).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance515).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance515;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance516).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance516).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance516).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance516).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance516).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance516;
		((AppearanceBase)appearance517).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance517;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance518).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance518).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance518;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance519).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance519;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommissionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCommissionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).Location = new System.Drawing.Point(189, 23);
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).Name = "cmbCommissionType";
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbCommissionType).TextChanged += new System.EventHandler(cmbCommissionType_TextChanged);
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label110.Location = new System.Drawing.Point(387, 5);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(61, 15);
		this.label110.TabIndex = 475;
		this.label110.Text = "Rate UOM";
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label111.Location = new System.Drawing.Point(189, 5);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(101, 15);
		this.label111.TabIndex = 473;
		this.label111.Text = "Commission Type";
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label112.Location = new System.Drawing.Point(478, 5);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(72, 15);
		this.label112.TabIndex = 468;
		this.label112.Text = "Fcy Amount";
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label113.Location = new System.Drawing.Point(313, 5);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(30, 15);
		this.label113.TabIndex = 469;
		this.label113.Text = "Rate";
		this.label109.AutoSize = true;
		this.label109.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label109.Location = new System.Drawing.Point(3, 5);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(73, 15);
		this.label109.TabIndex = 467;
		this.label109.Text = "Sales Person";
		this.CmbSalePerson.AllowNull = (DefaultableBoolean)1;
		this.CmbSalePerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalePerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalePerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalePerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Location = new System.Drawing.Point(3, 23);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Name = "CmbSalePerson";
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).TabIndex = 0;
		this.btnUpdateCommision.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateCommision.FlatAppearance.BorderSize = 0;
		this.btnUpdateCommision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateCommision.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateCommision.ForeColor = System.Drawing.Color.White;
		this.btnUpdateCommision.Location = new System.Drawing.Point(1043, 25);
		this.btnUpdateCommision.Name = "btnUpdateCommision";
		this.btnUpdateCommision.Size = new System.Drawing.Size(60, 22);
		this.btnUpdateCommision.TabIndex = 9;
		this.btnUpdateCommision.Text = "Update";
		this.btnUpdateCommision.UseVisualStyleBackColor = false;
		this.btnUpdateCommision.Visible = false;
		this.btnUpdateCommision.Click += new System.EventHandler(btnUpdateCommision_Click);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar5);
		this.panel13.Controls.Add(this.label108);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1162, 21);
		this.panel13.TabIndex = 0;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1122, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar5.MyGrid = this.grdCommissionInfo;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar5.TabIndex = 28;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label108.AutoSize = true;
		this.label108.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label108.ForeColor = System.Drawing.Color.White;
		this.label108.Location = new System.Drawing.Point(3, 3);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(304, 16);
		this.label108.TabIndex = 27;
		this.label108.Text = "Commission Agent / Sales Person Information";
		this.tabOtherCharges.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail);
		this.tabOtherCharges.Controls.Add(this.panel27);
		this.tabOtherCharges.Controls.Add(this.panel26);
		this.tabOtherCharges.Location = new System.Drawing.Point(4, 25);
		this.tabOtherCharges.Name = "tabOtherCharges";
		this.tabOtherCharges.Size = new System.Drawing.Size(1162, 314);
		this.tabOtherCharges.TabIndex = 4;
		this.tabOtherCharges.Text = "Other Charges";
		this.tabOtherCharges.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Size = new System.Drawing.Size(1162, 237);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).TabIndex = 4;
		this.grdOtherChargesDetail.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesDetail).VisualStyle = (VisualStyle)4;
		this.grdOtherChargesDetail.ColumnButtonClick += new ColumnActionEventHandler(grdOtherChargesDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).DoubleClick += new System.EventHandler(grdOtherChargesDetail_DoubleClick);
		this.panel27.BackColor = System.Drawing.Color.Teal;
		this.panel27.Controls.Add(this.ctrlgrdOtherChargesDetail);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel27.Location = new System.Drawing.Point(0, 54);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1162, 23);
		this.panel27.TabIndex = 3;
		this.ctrlgrdOtherChargesDetail.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdOtherChargesDetail.FormName = this;
		this.ctrlgrdOtherChargesDetail.Location = new System.Drawing.Point(1120, 0);
		this.ctrlgrdOtherChargesDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdOtherChargesDetail.MyGrid = this.grdOtherChargesDetail;
		this.ctrlgrdOtherChargesDetail.Name = "ctrlgrdOtherChargesDetail";
		this.ctrlgrdOtherChargesDetail.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdOtherChargesDetail.TabIndex = 27;
		this.ctrlgrdOtherChargesDetail.Load += new System.EventHandler(ctrlgrdOtherChargesDetail_Load);
		this.panel26.Controls.Add(this.BtnAddChargeDetail);
		this.panel26.Controls.Add(this.label130);
		this.panel26.Controls.Add(this.txtRemarksChargeDetail);
		this.panel26.Controls.Add(this.label134);
		this.panel26.Controls.Add(this.label131);
		this.panel26.Controls.Add(this.label132);
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetail);
		this.panel26.Controls.Add(this.label136);
		this.panel26.Controls.Add(this.BtnCancelChargeDetail);
		this.panel26.Controls.Add(this.BtnUpdateChargeDetail);
		this.panel26.Controls.Add(this.label133);
		this.panel26.Controls.Add(this.label140);
		this.panel26.Controls.Add(this.txtLessAmountChargeDetail);
		this.panel26.Controls.Add(this.txtAddAmountChargeDetail);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel26.Location = new System.Drawing.Point(0, 0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1162, 54);
		this.panel26.TabIndex = 1;
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
		this.txtAddAmountChargeDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddAmountChargeDetail.Location = new System.Drawing.Point(215, 26);
		this.txtAddAmountChargeDetail.Name = "txtAddAmountChargeDetail";
		this.txtAddAmountChargeDetail.Size = new System.Drawing.Size(109, 22);
		this.txtAddAmountChargeDetail.TabIndex = 1;
		this.txtAddAmountChargeDetail.Text = "0";
		this.txtAddAmountChargeDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel11.Controls.Add(this.txtPaymentRemarks);
		this.panel11.Controls.Add(this.HeaderGropBox);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranches);
		this.panel11.Controls.Add(this.label13);
		this.panel11.Controls.Add(this.label18);
		this.panel11.Controls.Add(this.label88);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew);
		this.panel11.Controls.Add(this.label51);
		this.panel11.Controls.Add(this.label11);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1170, 279);
		this.panel11.TabIndex = 3;
		this.txtPaymentRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPaymentRemarks.Location = new System.Drawing.Point(348, 358);
		this.txtPaymentRemarks.Name = "txtPaymentRemarks";
		this.txtPaymentRemarks.Size = new System.Drawing.Size(272, 23);
		this.txtPaymentRemarks.TabIndex = 20;
		this.HeaderGropBox.Controls.Add(this.txtSubContractExpiryDays);
		this.HeaderGropBox.Controls.Add(this.label117);
		this.HeaderGropBox.Controls.Add(this.txtCustomerContractNos);
		this.HeaderGropBox.Controls.Add(this.label106);
		this.HeaderGropBox.Controls.Add(this.txtDeliveryRemarks);
		this.HeaderGropBox.Controls.Add(this.label87);
		this.HeaderGropBox.Controls.Add(this.cmbInvoiceNo);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbConsignee);
		this.HeaderGropBox.Controls.Add(this.label86);
		this.HeaderGropBox.Controls.Add(this.label17);
		this.HeaderGropBox.Controls.Add(this.label2);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.HeaderGropBox.Controls.Add(this.lblCreditAc);
		this.HeaderGropBox.Controls.Add(this.label65);
		this.HeaderGropBox.Controls.Add(this.label1);
		this.HeaderGropBox.Controls.Add(this.label43);
		this.HeaderGropBox.Controls.Add(this.txtTotalNetAmount);
		this.HeaderGropBox.Controls.Add(this.label12);
		this.HeaderGropBox.Controls.Add(this.label45);
		this.HeaderGropBox.Controls.Add(this.label66);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew);
		this.HeaderGropBox.Controls.Add(this.txtaddlessamount);
		this.HeaderGropBox.Controls.Add(this.label60);
		this.HeaderGropBox.Controls.Add(this.label9);
		this.HeaderGropBox.Controls.Add(this.label25);
		this.HeaderGropBox.Controls.Add(this.label42);
		this.HeaderGropBox.Controls.Add(this.label64);
		this.HeaderGropBox.Controls.Add(this.label62);
		this.HeaderGropBox.Controls.Add(this.label16);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.HeaderGropBox.Controls.Add(this.label61);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.HeaderGropBox.Controls.Add(this.txtcertificateOfOrigion);
		this.HeaderGropBox.Controls.Add(this.label56);
		this.HeaderGropBox.Controls.Add(this.label40);
		this.HeaderGropBox.Controls.Add(this.label26);
		this.HeaderGropBox.Controls.Add(this.txtaddlesscommnets);
		this.HeaderGropBox.Controls.Add(this.label19);
		this.HeaderGropBox.Controls.Add(this.label10);
		this.HeaderGropBox.Controls.Add(this.label22);
		this.HeaderGropBox.Controls.Add(this.txtLotRef);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcareiertype);
		this.HeaderGropBox.Controls.Add(this.label31);
		this.HeaderGropBox.Controls.Add(this.label41);
		this.HeaderGropBox.Controls.Add(this.txtcertificate);
		this.HeaderGropBox.Controls.Add(this.label57);
		this.HeaderGropBox.Controls.Add(this.txtNoOfContainer);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbimporterBankNew);
		this.HeaderGropBox.Controls.Add(this.label55);
		this.HeaderGropBox.Controls.Add(this.label7);
		this.HeaderGropBox.Controls.Add(this.txtNetWeight);
		this.HeaderGropBox.Controls.Add(this.label54);
		this.HeaderGropBox.Controls.Add(this.txtGrossWeight);
		this.HeaderGropBox.Controls.Add(this.label53);
		this.HeaderGropBox.Controls.Add(this.label49);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNotifyParty2);
		this.HeaderGropBox.Controls.Add(this.label50);
		this.HeaderGropBox.Controls.Add(this.txtIformDate);
		this.HeaderGropBox.Controls.Add(this.txtIformNo);
		this.HeaderGropBox.Controls.Add(this.label46);
		this.HeaderGropBox.Controls.Add(this.txtLocalAmt);
		this.HeaderGropBox.Controls.Add(this.txtExRate);
		this.HeaderGropBox.Controls.Add(this.label44);
		this.HeaderGropBox.Controls.Add(this.label47);
		this.HeaderGropBox.Controls.Add(this.txtfcyAmount);
		this.HeaderGropBox.Controls.Add(this.label27);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbfcycode);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbexporterBankNew);
		this.HeaderGropBox.Controls.Add(this.label24);
		this.HeaderGropBox.Controls.Add(this.label67);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNotifyParty1);
		this.HeaderGropBox.Controls.Add(this.label8);
		this.HeaderGropBox.Controls.Add(this.label4);
		this.HeaderGropBox.Controls.Add(this.label28);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupCust);
		this.HeaderGropBox.Controls.Add(this.label21);
		this.HeaderGropBox.Controls.Add(this.label3);
		this.HeaderGropBox.Controls.Add(this.label68);
		this.HeaderGropBox.Controls.Add(this.txtDocDate);
		this.HeaderGropBox.Controls.Add(this.txtdocno);
		this.HeaderGropBox.Controls.Add(this.label5);
		this.HeaderGropBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.HeaderGropBox.Location = new System.Drawing.Point(4, 2);
		this.HeaderGropBox.Name = "HeaderGropBox";
		this.HeaderGropBox.Size = new System.Drawing.Size(1158, 273);
		this.HeaderGropBox.TabIndex = 0;
		this.HeaderGropBox.TabStop = false;
		this.HeaderGropBox.Text = "Main Header";
		this.txtCustomerContractNos.Enabled = false;
		this.txtCustomerContractNos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerContractNos.Location = new System.Drawing.Point(881, 41);
		this.txtCustomerContractNos.Name = "txtCustomerContractNos";
		this.txtCustomerContractNos.Size = new System.Drawing.Size(272, 23);
		this.txtCustomerContractNos.TabIndex = 32;
		this.label106.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label106.Location = new System.Drawing.Point(813, 42);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(87, 30);
		this.label106.TabIndex = 596;
		this.label106.Text = "Customer Contract Nos ";
		this.txtDeliveryRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryRemarks.Location = new System.Drawing.Point(881, 16);
		this.txtDeliveryRemarks.Name = "txtDeliveryRemarks";
		this.txtDeliveryRemarks.Size = new System.Drawing.Size(272, 23);
		this.txtDeliveryRemarks.TabIndex = 22;
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.Location = new System.Drawing.Point(815, 20);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(52, 15);
		this.label87.TabIndex = 573;
		this.label87.Text = "Remarks";
		this.cmbInvoiceNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.Location = new System.Drawing.Point(105, 44);
		this.cmbInvoiceNo.Name = "cmbInvoiceNo";
		this.cmbInvoiceNo.Size = new System.Drawing.Size(207, 23);
		this.cmbInvoiceNo.TabIndex = 2;
		this.cmbConsignee.AllowNull = (DefaultableBoolean)1;
		this.cmbConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbConsignee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Location = new System.Drawing.Point(105, 99);
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Name = "cmbConsignee";
		((System.Windows.Forms.Control)(object)this.cmbConsignee).Size = new System.Drawing.Size(207, 25);
		((System.Windows.Forms.Control)(object)this.cmbConsignee).TabIndex = 4;
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label86.Location = new System.Drawing.Point(6, 104);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(62, 15);
		this.label86.TabIndex = 594;
		this.label86.Text = "Consignee";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(398, 47);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 17);
		this.label17.TabIndex = 591;
		this.label17.Text = "*";
		this.label17.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(318, 48);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(55, 15);
		this.label2.TabIndex = 590;
		this.label2.Text = "G.D Date";
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance334).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance334).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance334).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance445).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance445).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance445;
		((AppearanceBase)appearance509).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance509).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance509;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance520).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance520;
		((AppearanceBase)appearance531).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance531).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance531;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance542).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance542).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance542).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance542).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance542).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance542;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(649, 212);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 29;
		this.lblCreditAc.AutoSize = true;
		this.lblCreditAc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblCreditAc.Location = new System.Drawing.Point(544, 216);
		this.lblCreditAc.Name = "lblCreditAc";
		this.lblCreditAc.Size = new System.Drawing.Size(60, 15);
		this.lblCreditAc.TabIndex = 589;
		this.lblCreditAc.Text = "Credit A/c";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Red;
		this.label65.Location = new System.Drawing.Point(90, 245);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(14, 17);
		this.label65.TabIndex = 586;
		this.label65.Text = "*";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(398, 131);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 43;
		this.label1.Text = "*";
		this.label1.Visible = false;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(398, 159);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 42;
		this.label43.Text = "*";
		this.label43.Visible = false;
		this.txtTotalNetAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalNetAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalNetAmount.Location = new System.Drawing.Point(417, 214);
		this.txtTotalNetAmount.Name = "txtTotalNetAmount";
		this.txtTotalNetAmount.ReadOnly = true;
		this.txtTotalNetAmount.Size = new System.Drawing.Size(122, 22);
		this.txtTotalNetAmount.TabIndex = 17;
		this.txtTotalNetAmount.TabStop = false;
		this.txtTotalNetAmount.Text = "0";
		this.txtTotalNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(544, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(80, 15);
		this.label12.TabIndex = 526;
		this.label12.Text = "Delivery Term";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(398, 217);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 550;
		this.label45.Text = "*";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(318, 218);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(80, 15);
		this.label66.TabIndex = 585;
		this.label66.Text = "Total Amount";
		this.cmbdeliverytermnew.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliverytermnew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliverytermnew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliverytermnew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance434).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance434).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance434;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance479).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance479).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance479).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance479).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance479;
		((AppearanceBase)appearance490).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance490;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance524).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance524).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance524).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance524).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance524;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance525).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance525).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance525;
		((AppearanceBase)appearance526).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance526).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance526;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance507).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance507;
		((AppearanceBase)appearance527).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance527).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance527;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance528).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance528).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance528).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance528).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance528).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance528;
		((AppearanceBase)appearance529).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance529;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdeliverytermnew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Location = new System.Drawing.Point(649, 15);
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Name = "cmbdeliverytermnew";
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).TabIndex = 21;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).TextChanged += new System.EventHandler(CmbDeliveryTerm_TextChanged);
		this.txtaddlessamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlessamount.Enabled = false;
		this.txtaddlessamount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddlessamount.Location = new System.Drawing.Point(417, 185);
		this.txtaddlessamount.Name = "txtaddlessamount";
		this.txtaddlessamount.Size = new System.Drawing.Size(122, 22);
		this.txtaddlessamount.TabIndex = 16;
		this.txtaddlessamount.Text = "0";
		this.txtaddlessamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtaddlessamount.TextChanged += new System.EventHandler(txtaddlessamount_TextChanged);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(398, 75);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 573;
		this.label60.Text = "*";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(633, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(14, 17);
		this.label9.TabIndex = 528;
		this.label9.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(318, 132);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(74, 15);
		this.label25.TabIndex = 538;
		this.label25.Text = "Fcy-Amount";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(318, 160);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(82, 15);
		this.label42.TabIndex = 549;
		this.label42.Text = "Local Amount";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(318, 189);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(80, 15);
		this.label64.TabIndex = 582;
		this.label64.Text = "Add Less Amt";
		this.label62.BackColor = System.Drawing.Color.LightGray;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(816, 73);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(337, 25);
		this.label62.TabIndex = 577;
		this.label62.Text = "Certificate Of Origin";
		this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(544, 76);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(93, 15);
		this.label16.TabIndex = 533;
		this.label16.Text = "Destination Port";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance485).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance485).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance485;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance486).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance486).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance486).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance486).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance486;
		((AppearanceBase)appearance487).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance487;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance488).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance488).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance488).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance488).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance488;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance489).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance489).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance489;
		((AppearanceBase)appearance491).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance491).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance491;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance492).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance492;
		((AppearanceBase)appearance493).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance493).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance493;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance494).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance494).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance494).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance494).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance494).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance494;
		((AppearanceBase)appearance495).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance495;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance496).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance496).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance496;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance497).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance497;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(649, 43);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 23;
		this.label61.BackColor = System.Drawing.Color.LightGray;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(816, 156);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(337, 25);
		this.label61.TabIndex = 576;
		this.label61.Text = "Certificate -1";
		this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance537).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance537).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance537;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance538).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance538).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance538).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance538).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance538;
		((AppearanceBase)appearance539).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance539;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance540).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance540).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance540).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance540).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance540;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance541).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance541).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance541;
		((AppearanceBase)appearance543).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance543).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance543;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance544).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance544;
		((AppearanceBase)appearance545).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance545).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance545;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance546).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance546).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance546).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance546).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance546).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance546;
		((AppearanceBase)appearance547).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance547;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance548).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance548).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance548;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance549).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance549;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(649, 71);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 24;
		this.txtcertificateOfOrigion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcertificateOfOrigion.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcertificateOfOrigion.Location = new System.Drawing.Point(816, 101);
		this.txtcertificateOfOrigion.Multiline = true;
		this.txtcertificateOfOrigion.Name = "txtcertificateOfOrigion";
		this.txtcertificateOfOrigion.Size = new System.Drawing.Size(337, 53);
		this.txtcertificateOfOrigion.TabIndex = 30;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(544, 48);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(75, 15);
		this.label56.TabIndex = 534;
		this.label56.Text = "Loading Port";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(318, 246);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(99, 15);
		this.label40.TabIndex = 580;
		this.label40.Text = "Ad/Ls Comments";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(633, 47);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(14, 17);
		this.label26.TabIndex = 535;
		this.label26.Text = "*";
		this.txtaddlesscommnets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlesscommnets.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddlesscommnets.Location = new System.Drawing.Point(417, 242);
		this.txtaddlesscommnets.Name = "txtaddlesscommnets";
		this.txtaddlesscommnets.Size = new System.Drawing.Size(395, 22);
		this.txtaddlesscommnets.TabIndex = 18;
		this.txtaddlesscommnets.TextChanged += new System.EventHandler(txtaddlesscommnets_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(633, 75);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 536;
		this.label19.Text = "*";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(6, 160);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(80, 15);
		this.label10.TabIndex = 120;
		this.label10.Text = "Notify Party 2";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(544, 104);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(69, 15);
		this.label22.TabIndex = 542;
		this.label22.Text = "Carrier Type";
		this.txtLotRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLotRef.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLotRef.Location = new System.Drawing.Point(105, 242);
		this.txtLotRef.Name = "txtLotRef";
		this.txtLotRef.Size = new System.Drawing.Size(207, 22);
		this.txtLotRef.TabIndex = 9;
		this.cmbcareiertype.AllowNull = (DefaultableBoolean)1;
		this.cmbcareiertype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcareiertype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcareiertype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance138;
		((SpecialBoxBase)((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcareiertype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Location = new System.Drawing.Point(649, 99);
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Name = "cmbcareiertype";
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).TabIndex = 25;
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(6, 189);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(83, 15);
		this.label31.TabIndex = 524;
		this.label31.Text = "Importer Bank";
		this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(633, 103);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 545;
		this.label41.Text = "*";
		this.txtcertificate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcertificate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcertificate.Location = new System.Drawing.Point(816, 184);
		this.txtcertificate.Multiline = true;
		this.txtcertificate.Name = "txtcertificate";
		this.txtcertificate.Size = new System.Drawing.Size(337, 54);
		this.txtcertificate.TabIndex = 31;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(544, 132);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(93, 15);
		this.label57.TabIndex = 552;
		this.label57.Text = "No.Of Container";
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainer.Location = new System.Drawing.Point(649, 128);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(163, 22);
		this.txtNoOfContainer.TabIndex = 26;
		this.txtNoOfContainer.Text = "0";
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbimporterBankNew.AllowNull = (DefaultableBoolean)1;
		this.cmbimporterBankNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbimporterBankNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbimporterBankNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance465).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance465).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance465;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance466).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance466).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance466).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance466).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance466;
		((AppearanceBase)appearance467).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance467;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbimporterBankNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Location = new System.Drawing.Point(105, 184);
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Name = "cmbimporterBankNew";
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Size = new System.Drawing.Size(207, 25);
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).TabIndex = 7;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(544, 190);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(67, 15);
		this.label55.TabIndex = 554;
		this.label55.Text = "Net MTon's";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(6, 246);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 57;
		this.label7.Text = "Lot Reference";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(649, 186);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(163, 22);
		this.txtNetWeight.TabIndex = 28;
		this.txtNetWeight.Text = "0";
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(544, 161);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(77, 15);
		this.label54.TabIndex = 553;
		this.label54.Text = "Gross MTon's";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(649, 157);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(163, 22);
		this.txtGrossWeight.TabIndex = 27;
		this.txtGrossWeight.Text = "0";
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(633, 131);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 570;
		this.label53.Text = "*";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(633, 189);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 560;
		this.label49.Text = "*";
		this.cmbNotifyParty2.AllowNull = (DefaultableBoolean)1;
		this.cmbNotifyParty2.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNotifyParty2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNotifyParty2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance428).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance428).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance428;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance429).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance429).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance429).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance429).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance429;
		((AppearanceBase)appearance430).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance430;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance431).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance431).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance431).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance431).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance431;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance432).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance432).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance432;
		((AppearanceBase)appearance433).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance433).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance433;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance435).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance435;
		((AppearanceBase)appearance436).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance436).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance436;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance437).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance437).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance437).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance437).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance437).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance437;
		((AppearanceBase)appearance239).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbNotifyParty2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Location = new System.Drawing.Point(105, 155);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Name = "cmbNotifyParty2";
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Size = new System.Drawing.Size(207, 25);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).TabIndex = 6;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(633, 160);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 569;
		this.label50.Text = "*";
		this.txtIformDate.CustomFormat = "dd-MMM-yy";
		this.txtIformDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIformDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtIformDate.Location = new System.Drawing.Point(417, 44);
		this.txtIformDate.Name = "txtIformDate";
		this.txtIformDate.Size = new System.Drawing.Size(122, 22);
		this.txtIformDate.TabIndex = 11;
		this.txtIformNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtIformNo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIformNo.Location = new System.Drawing.Point(417, 16);
		this.txtIformNo.Name = "txtIformNo";
		this.txtIformNo.Size = new System.Drawing.Size(122, 22);
		this.txtIformNo.TabIndex = 10;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(318, 20);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(46, 15);
		this.label46.TabIndex = 561;
		this.label46.Text = "G.D No";
		this.txtLocalAmt.BackColor = System.Drawing.Color.White;
		this.txtLocalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmt.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLocalAmt.Location = new System.Drawing.Point(417, 156);
		this.txtLocalAmt.Name = "txtLocalAmt";
		this.txtLocalAmt.ReadOnly = true;
		this.txtLocalAmt.Size = new System.Drawing.Size(122, 22);
		this.txtLocalAmt.TabIndex = 15;
		this.txtLocalAmt.TabStop = false;
		this.txtLocalAmt.Text = "0";
		this.txtLocalAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExRate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExRate.Location = new System.Drawing.Point(417, 100);
		this.txtExRate.Name = "txtExRate";
		this.txtExRate.Size = new System.Drawing.Size(122, 22);
		this.txtExRate.TabIndex = 13;
		this.txtExRate.Text = "0";
		this.txtExRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExRate.TextChanged += new System.EventHandler(txtExRate_TextChanged);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(318, 104);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(84, 15);
		this.label44.TabIndex = 546;
		this.label44.Text = "Exchange Rate";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(398, 103);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 543;
		this.label47.Text = "*";
		this.txtfcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfcyAmount.Location = new System.Drawing.Point(417, 128);
		this.txtfcyAmount.Name = "txtfcyAmount";
		this.txtfcyAmount.ReadOnly = true;
		this.txtfcyAmount.Size = new System.Drawing.Size(122, 22);
		this.txtfcyAmount.TabIndex = 14;
		this.txtfcyAmount.Text = "0";
		this.txtfcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcyAmount.TextChanged += new System.EventHandler(txtfcyAmount_TextChanged);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(318, 76);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(55, 15);
		this.label27.TabIndex = 537;
		this.label27.Text = "Fcy Code";
		this.cmbfcycode.AllowNull = (DefaultableBoolean)1;
		this.cmbfcycode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbfcycode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbfcycode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbfcycode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Location = new System.Drawing.Point(417, 71);
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Name = "cmbfcycode";
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Size = new System.Drawing.Size(122, 25);
		((System.Windows.Forms.Control)(object)this.cmbfcycode).TabIndex = 12;
		this.cmbexporterBankNew.AllowNull = (DefaultableBoolean)1;
		this.cmbexporterBankNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbexporterBankNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbexporterBankNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance530).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance530).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance530;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance532).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance532).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance532).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance532).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance532;
		((AppearanceBase)appearance533).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance533;
		((SpecialBoxBase)((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance534).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance534).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance534).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance534).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance534;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance535).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance535).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance535;
		((AppearanceBase)appearance536).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance536).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance536;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbexporterBankNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Location = new System.Drawing.Point(105, 213);
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Name = "cmbexporterBankNew";
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Size = new System.Drawing.Size(207, 25);
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).TabIndex = 8;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(6, 218);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(80, 15);
		this.label24.TabIndex = 525;
		this.label24.Text = "Exporter Bank";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(6, 48);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(65, 15);
		this.label67.TabIndex = 57;
		this.label67.Text = "Invoice No";
		this.cmbNotifyParty1.AllowNull = (DefaultableBoolean)1;
		this.cmbNotifyParty1.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNotifyParty1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNotifyParty1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance151;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbNotifyParty1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Location = new System.Drawing.Point(105, 127);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Name = "cmbNotifyParty1";
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Size = new System.Drawing.Size(207, 25);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).TabIndex = 5;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(6, 132);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 117;
		this.label8.Text = "Notify Party 1";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(90, 75);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(14, 17);
		this.label4.TabIndex = 115;
		this.label4.Text = "*";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(90, 75);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(14, 17);
		this.label28.TabIndex = 111;
		this.label28.Text = "*";
		this.cmbSupCust.AllowNull = (DefaultableBoolean)1;
		this.cmbSupCust.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupCust.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupCust.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance398).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance398).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Appearance = (AppearanceBase)(object)appearance398;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance399).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance399).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance399).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance399).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance399;
		((AppearanceBase)appearance400).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance400;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance402).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance402).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance402).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance402).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance402;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance403).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance403).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance403;
		((AppearanceBase)appearance404).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance404).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance404;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance405).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance405;
		((AppearanceBase)appearance406).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance406).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance406;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance407).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance407).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance407).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance407).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance407).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance407;
		((AppearanceBase)appearance408).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance408;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance409).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance409).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance409;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance410).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance410;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupCust.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Location = new System.Drawing.Point(105, 71);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Name = "cmbSupCust";
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Size = new System.Drawing.Size(207, 25);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Leave += new System.EventHandler(cmbSupCust_Leave);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(6, 76);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(58, 15);
		this.label21.TabIndex = 78;
		this.label21.Text = "Customer";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(90, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 0;
		this.label3.Text = "*";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Red;
		this.label68.Location = new System.Drawing.Point(90, 47);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(14, 17);
		this.label68.TabIndex = 50;
		this.label68.Text = "*";
		this.txtDocDate.CustomFormat = "dd-MMM-yy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(183, 16);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(129, 22);
		this.txtDocDate.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(105, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(73, 22);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(6, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 49;
		this.label5.Text = "Doc No && Date";
		this.cmbbranches.AllowNull = (DefaultableBoolean)1;
		this.cmbbranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbbranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbbranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbbranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranches).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbbranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranches).Location = new System.Drawing.Point(116, 299);
		((System.Windows.Forms.Control)(object)this.cmbbranches).Name = "cmbbranches";
		((System.Windows.Forms.Control)(object)this.cmbbranches).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbbranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label13.Location = new System.Drawing.Point(31, 305);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 15);
		this.label13.TabIndex = 40;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label18.Location = new System.Drawing.Point(31, 328);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(46, 15);
		this.label18.TabIndex = 41;
		this.label18.Text = "Project";
		this.label18.Visible = false;
		this.label88.AutoSize = true;
		this.label88.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label88.Location = new System.Drawing.Point(283, 362);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(52, 15);
		this.label88.TabIndex = 574;
		this.label88.Text = "Remarks";
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbproject).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(116, 329);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbproject).Visible = false;
		this.cmbPaymentTermsNew.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTermsNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTermsNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTermsNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPaymentTermsNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Location = new System.Drawing.Point(116, 357);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Name = "cmbPaymentTermsNew";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).TextChanged += new System.EventHandler(CmbPaymentTerms_TextChanged);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.Red;
		this.label51.Location = new System.Drawing.Point(100, 361);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(14, 17);
		this.label51.TabIndex = 592;
		this.label51.Text = "*";
		this.label51.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(11, 362);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(84, 15);
		this.label11.TabIndex = 527;
		this.label11.Text = "Payment Term";
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
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
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
		this.ChkPrintPreview.Location = new System.Drawing.Point(1082, 2);
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
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.btnNew, this.btnRefresh, this.btnsave, this.btnupdate, this.btnAttachment, this.btnPrint, this.BtnPrintPL, this.BtnLoad, this.btnGenerateInvoiceNos, this.btnItemDetailLookUp,
			this.btnShortCutKey, this.btnPrint501B, this.btnExportCharges
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
		this.BtnPrintPL.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintPL.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintPL.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintPL.Name = "BtnPrintPL";
		this.BtnPrintPL.Size = new System.Drawing.Size(92, 24);
		this.BtnPrintPL.Text = "Print-529";
		this.BtnPrintPL.Click += new System.EventHandler(BtnPrintPL_Click);
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
		this.btnItemDetailLookUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnItemDetailLookUp.Name = "btnItemDetailLookUp";
		this.btnItemDetailLookUp.Size = new System.Drawing.Size(133, 24);
		this.btnItemDetailLookUp.Text = "ItemDetail_LookUp";
		this.btnItemDetailLookUp.Click += new System.EventHandler(btnItemDetailLookUp_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnPrint501B.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint501B.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint501B.Name = "btnPrint501B";
		this.btnPrint501B.Size = new System.Drawing.Size(163, 24);
		this.btnPrint501B.Text = "Contract Print-501B";
		this.btnPrint501B.Click += new System.EventHandler(btnPrint501B_Click);
		this.btnExportCharges.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnExportCharges.Name = "btnExportCharges";
		this.btnExportCharges.Size = new System.Drawing.Size(111, 20);
		this.btnExportCharges.Text = "Export Charges";
		this.btnExportCharges.Click += new System.EventHandler(btnExportCharges_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Controls.Add(this.panel14);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetailHistory);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1176, 682);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1170, 244);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label92);
		this.panel15.Controls.Add(this.ctrlGrdBar1);
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
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1122, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
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
		((AppearanceBase)appearance501).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance501).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance501;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance505).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance505).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance505).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance505).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance505;
		((AppearanceBase)appearance506).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance506;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance508).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance508).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance508).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance508).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance508;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance510).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance510).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance510;
		((AppearanceBase)appearance498).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance498).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance498;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance514).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance514;
		((AppearanceBase)appearance499).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance499).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance499;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance500).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance500).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance500).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance500).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance500).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance500;
		((AppearanceBase)appearance502).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance502;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance503).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance503).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance503;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance504).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance504;
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
		((AppearanceBase)appearance452).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance452).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance452;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance453).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance453).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance453).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance453).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance453;
		((AppearanceBase)appearance454).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance454;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance455).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance455).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance455).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance455).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance455;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance456).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance456).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance456;
		((AppearanceBase)appearance458).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance458).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance458;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance459).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance459;
		((AppearanceBase)appearance460).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance460).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance460;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance461).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance461).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance461).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance461).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance461).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance461;
		((AppearanceBase)appearance462).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance462;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance463).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance463).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance463;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance464).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance464;
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
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label93);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel14.Location = new System.Drawing.Point(3, 408);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1170, 22);
		this.panel14.TabIndex = 2;
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label93.ForeColor = System.Drawing.Color.White;
		this.label93.Location = new System.Drawing.Point(3, 3);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(234, 17);
		this.label93.TabIndex = 1;
		this.label93.Text = "Detail of the Selected above Row";
		this.gridDetailHistory.AllowEdit = (InheritableBoolean)2;
		gridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridDetailHistory.DesignTimeLayout = gridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.gridDetailHistory.DynamicFiltering = true;
		this.gridDetailHistory.FilterMode = (FilterMode)2;
		this.gridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Location = new System.Drawing.Point(3, 430);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Name = "gridDetailHistory";
		((ScrollableBase)this.gridDetailHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetailHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.gridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Size = new System.Drawing.Size(1170, 249);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).TabIndex = 1;
		this.gridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.gridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 711);
		this.panel1.TabIndex = 1;
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance446).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance446).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance446;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance457).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance457).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance457).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance457).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance457;
		((AppearanceBase)appearance468).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance468;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance512).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance512).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance512).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance512).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance512;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance513).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance513).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance513;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance438).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance438;
		((AppearanceBase)appearance439).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance439).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance439;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance440).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance440).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance440).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance440).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance440).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance440;
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
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance469).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance469).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance469).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance469).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance469;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance470).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance470).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance470;
		((AppearanceBase)appearance471).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance471).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance471;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance472).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance472;
		((AppearanceBase)appearance473).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance473).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance473;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance474).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance474).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance474).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance474).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance474).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance474;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(111, 210);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 7;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance475).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance475).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance475;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance476).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance476).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance476).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance476).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance476;
		((AppearanceBase)appearance477).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance477;
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
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance441).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance441;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance442).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance442).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance442;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance443).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance443;
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
		((AppearanceBase)appearance444).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance444).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance444;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance447).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance447).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance447).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance447).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance447;
		((AppearanceBase)appearance448).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance448;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance449).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance449).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance449).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance449).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance449;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance450).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance450).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance450;
		((AppearanceBase)appearance451).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance451).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance451;
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
		this.CmbLCContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbLCContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLCContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLCContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance285).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance286;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance289).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance291).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance291).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance291).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance291;
		((AppearanceBase)appearance292).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.CmbLCContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Location = new System.Drawing.Point(111, 120);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Name = "CmbLCContractor";
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).TabIndex = 4;
		this.ultraCombo2.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo2.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance244;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance250).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance251).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance254;
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
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance296).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance296).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance296;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance297;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance298).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance298).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance299;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance303).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance304).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance304).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance304).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance304;
		((AppearanceBase)appearance255).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(385, 67);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 10;
		this.cmbCarierType.AllowNull = (DefaultableBoolean)1;
		this.cmbCarierType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCarierType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCarierType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance306).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance306).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance307).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance307;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance308).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance308).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance308).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance309).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance309;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance313).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance313).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance314).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance314).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance314).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance315).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance316).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbCarierType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Location = new System.Drawing.Point(385, 96);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Name = "cmbCarierType";
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).TabIndex = 11;
		this.cmbExporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbExporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance319).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance319).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance319).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance319;
		((AppearanceBase)appearance320).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance320;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbExporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Location = new System.Drawing.Point(111, 180);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Name = "cmbExporterBank";
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).TabIndex = 6;
		this.cmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance321).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance322).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance322).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance322;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance324;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance325).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance325).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance325).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance325;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance326).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance326;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance327).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance328;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance329).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance330).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance330).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance330).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance330).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance330;
		((AppearanceBase)appearance331).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance332).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance333;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Location = new System.Drawing.Point(111, 151);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Name = "cmbImporterBank";
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).TabIndex = 5;
		this.cmbEFormNo.AllowNull = (DefaultableBoolean)1;
		this.cmbEFormNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEFormNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEFormNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance260).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance260).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance260;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance261;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance262).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance262).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance266).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance267).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance267).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance267).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance267;
		((AppearanceBase)appearance269).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbEFormNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Location = new System.Drawing.Point(662, 164);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Name = "cmbEFormNo";
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Size = new System.Drawing.Size(164, 22);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).TabIndex = 0;
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance274;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance275).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance275).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance280).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance281).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance281).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance281).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(385, 6);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 8;
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
		this.label117.AutoSize = true;
		this.label117.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label117.Location = new System.Drawing.Point(816, 244);
		this.label117.Name = "label117";
		this.label117.Size = new System.Drawing.Size(140, 15);
		this.label117.TabIndex = 597;
		this.label117.Text = "Sub Contract Expiry Days";
		this.txtSubContractExpiryDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSubContractExpiryDays.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubContractExpiryDays.Location = new System.Drawing.Point(1019, 240);
		this.txtSubContractExpiryDays.Name = "txtSubContractExpiryDays";
		this.txtSubContractExpiryDays.Size = new System.Drawing.Size(133, 22);
		this.txtSubContractExpiryDays.TabIndex = 32;
		this.txtSubContractExpiryDays.Text = "0";
		this.txtSubContractExpiryDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 711);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ExImCommercialInvoice";
		this.Text = "Commercial Invoice";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExImCommercialInvoice_FormClosing);
		base.Load += new System.EventHandler(ImProformaInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ImProformaInvoice_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.GroupDetail.ResumeLayout(false);
		this.GroupDetail.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingNTrade).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHSCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractSchedule).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).EndInit();
		this.panel18.ResumeLayout(false);
		this.tabPage5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinInstruments).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dcmbpaymentterm).EndInit();
		this.panel20.ResumeLayout(false);
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCommissionInfo).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.tabOtherCharges.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetail).EndInit();
		this.panel27.ResumeLayout(false);
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesNameChargeDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.HeaderGropBox.ResumeLayout(false);
		this.HeaderGropBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbConsignee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliverytermnew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcareiertype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterBankNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfcycode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbexporterBankNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupCust).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTermsNew).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportCompany).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
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
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).EndInit();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
