// using Architecture.Reports;
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
using Architecture.BLL.Reports.Export;
using Architecture.BLL.SDT;
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

public class frmExportContractIII : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtOtherItemDetail = new DataTable();

	private DataTable dtPmDetail = new DataTable();

	private DataTable dtPaymnentDetail = new DataTable();

	private DataTable dtOtherChargesDetail = new DataTable();

	private DataTable dtcol = new DataTable();

	private DataTable dtSalesMan = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtConsignee = new DataTable();

	private DataTable dtCustomerOrderNo = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtBrand = new DataTable();

	private DataTable dtpcktype = new DataTable();

	private DataTable dtLastSavedRecordHeader = new DataTable();

	private DataTable dtLastSavedRecordByItemId = new DataTable();

	private DataTable dtPMItem = new DataTable();

	private DataTable dtBrandForPMItem = new DataTable();

	private bool FormInitialized;

	private string ScreenName = "frmExportContractIII";

	private int DocumentTypeId = 223;

	private int RecId;

	private int updateDetailIndex = -1;

	private int updateDetailIndexOther = -1;

	private int updateDetailIndexPaymentDetail = -1;

	private int updateDetailIndexPmDetail = -1;

	private int updateDetailIndexOtherCharges = -1;

	private int ItemIdOfPreviousRate;

	private string CustomAttachmentsValues = "";

	private string AttachmentsValues = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool AllItemsBindonExportContract;

	private bool ItemSearchWithNameOrCode;

	private bool PMGridInfoShouldNotCompulsoryOnContract;

	private bool AllowExportMultiCompanies;

	private bool ShipmentDocumentsFeature;

	private string SpecialInstructionsForExportContract = "";

	private List<ExImLcOrderPackingDetail> lstRemoveRecord = new List<ExImLcOrderPackingDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton Print;

	private Panel panel17;

	private ToolStrip toolStrip3;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnDefinePaymentTerm;

	internal GridEX grdotheritems;

	private Button btnCancelInDetail;

	private Button btnUpdateInDetail;

	private Button BtnAddInDetail;

	private Label label16;

	private TextBox txtQtyMTonInDetail;

	private Label label13;

	internal UltraCombo CmbRateUomInDetail;

	private Label label10;

	internal UltraCombo CmbPackingTypeInDetail;

	private Label label2;

	internal UltraCombo CmbOuterUomInDetail;

	private Label label1;

	internal UltraCombo CmbItemNameInDetail;

	internal UltraCombo CmbFCYCode;

	private Label label22;

	private Label label20;

	private Label label9;

	private Label label6;

	private Label label8;

	private Label label21;

	private Label label3;

	private Label label4;

	private Label label28;

	internal UltraCombo CmbExporterbank;

	internal UltraCombo CmbImporterBank;

	internal UltraCombo CmbDestinationPort;

	internal UltraCombo CmbLoadingPort;

	internal UltraCombo CmbDeliveryTerm;

	private DateTimePicker txtCustomerOrderDate;

	private DateTimePicker txtDocDate;

	internal UltraCombo CmbNotifyParty1;

	internal UltraCombo CmbCustomer;

	internal UltraCombo CmbSalePerson;

	private TextBox txtMTon;

	private TextBox txtFCYAmount;

	private Label label29;

	private DateTimePicker txtShipmentDate;

	private Label label27;

	private TextBox txtLotRefNo;

	private Label label26;

	private Label label32;

	private TextBox txtNoOfContainers;

	private Label label12;

	private TextBox txtOuterQtyInDetail;

	private Label label23;

	private TextBox txtRateInDetail;

	private Label label25;

	private TextBox txtAmountInDetail;

	private Label label33;

	private TextBox txtRemarksInDetail;

	private Label label34;

	private ToolStripButton BtnRefresh;

	private ToolStripButton BtnDeliveryTermLookUp;

	private Label label14;

	private Label label5;

	internal UltraCombo cmbStatus;

	private Label label15;

	private Label label18;

	private Label label17;

	private Label label19;

	private Panel panel5;

	private TabControl tabControl2;

	private TabPage tabOtherItems;

	private TabPage tabContractDetail;

	internal UltraCombo CmbCropYearInDetail;

	private Label label24;

	private Panel panel7;

	private Panel panel6;

	internal GridEX grdDetails;

	private Panel panel9;

	private Panel panel8;

	internal UltraCombo CmbItemOtherDetail;

	private Label label31;

	private TextBox txtQtyOtherDetail;

	private Label label35;

	private TextBox txtRateOtherDetail;

	private Label label36;

	private Label label37;

	private TextBox txtAmountOtherDetail;

	private Label label38;

	private TextBox txtRemarksOtherDetail;

	private Button btnCancelOtherDetail;

	private Button btnUpdateOtherDetail;

	private Button btnaddOtherDetail;

	private ToolStripButton btnSeaPortDefine;

	private CtrlGrdBar ctrlGrdBar1;

	private TabPage tabPaymentTerm;

	private Panel panel10;

	private Panel panel11;

	private Panel panel12;

	private Label label54;

	internal UltraCombo CmbPaymentTermPT;

	internal GridEX grdPaymentTerm;

	private Panel panel14;

	private TextBox txtPercntOfTotalPT;

	private Label label53;

	private Label label55;

	private TextBox txtlFcyAmountPT;

	private TabPage tabPackingMaterialDetail;

	private Button btnCancelPayment;

	private Button btnUpdatePayment;

	private Button btnAddPayment;

	private Panel panel15;

	private Panel panel19;

	private Panel panel21;

	private Panel panel20;

	private Panel panel16;

	private Label label56;

	private Label label57;

	internal UltraCombo CmbItemPmDetail;

	private Label label64;

	private Label label63;

	private Label label62;

	private Label label61;

	private Label label60;

	private Label label58;

	internal UltraCombo CmbBrandPackingTypePmDetail;

	private TextBox txtInnerQtyPmDetail;

	private TextBox txtAmountPmDetail;

	private TextBox txtRatePmDetail;

	private TextBox txtRemarksPmDetail;

	private Button btnPackingCancel;

	private Button btnPackingUpdate;

	private Button btnPackingAdd;

	private Panel panel22;

	private Panel panel24;

	private Panel panel25;

	private Panel panel27;

	internal GridEX grdPackingMaterialDetail;

	private Panel panel28;

	private Label label66;

	private Label label40;

	private TextBox txtExchangeRate;

	private Label label41;

	private DateTimePicker datShipmentStartDate;

	private Label label42;

	private TextBox txtDeliveryDays;

	private Label label43;

	private DateTimePicker ApprovalDate;

	private Label label44;

	private Panel panel23;

	private Label label45;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private RadioButton rdPackingItemCode;

	private RadioButton rdPackingItemName;

	private ToolStripButton btnShortCutKey;

	private Panel panel26;

	private Label label46;

	internal UltraCombo CmbBrandPmDetail;

	private Label label47;

	private RadioButton radBrandName;

	private RadioButton rabBrandName;

	private Panel panel30;

	private TextBox txtCommodityDetailInDetail;

	private Label label48;

	internal UltraCombo CmbComditiyDetailByItemCusInDetail;

	private Label label39;

	private ToolStripButton BtnCommodityDetailForm;

	private GroupBox groupBox1;

	private TextBox txtDeliveryRemarks;

	private Label label51;

	private TextBox txtApprovalRemarks;

	private TextBox txtProductSpecification;

	private Label label50;

	private TextBox txtRemarksPT;

	private Label label30;

	internal GridEX grddetailhistory;

	private Label label52;

	private Panel panel13;

	private Label label59;

	internal GridEX DataGridHistory;

	private Panel panel29;

	private Label label65;

	private Panel panel18;

	private GroupBox groupBox5;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label67;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

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

	private ToolStripButton BtnSaveAs;

	private ToolStripButton BtnDelete;

	private CheckBox ChkPreview;

	internal UltraCombo CmbExportCompany;

	private Label labelExportCompany;

	internal UltraCombo CmbFarmingNTradeInDetail;

	private Label label69;

	private DateTimePicker txtPMScheduleDate;

	private Label label72;

	private DateTimePicker txtInspectionScheduleDate;

	private Label label71;

	private DateTimePicker txtProductionScheduleDate;

	private Label label70;

	private DateTimePicker txtFactoryLoadingDate;

	private Label label68;

	private TabControl tabControl3;

	private TabPage tabDetailH;

	private TabPage tabPaymentDetailH;

	internal GridEX grdPaymentHistory;

	private Panel panel31;

	private Label label74;

	private TabPage tabPmH;

	internal GridEX grdPMHistory;

	private Panel panel32;

	private Label label75;

	private TabPage tabOtherItemH;

	internal GridEX grdOtherItemHistory;

	private Panel panel33;

	private Label label76;

	private TextBox txtBrandInnerQtyPmDetail;

	private TextBox txtBrandOuterQtyPmDetail;

	internal UltraCombo CmbBrandUomPmDetail;

	private Label label77;

	private Label label79;

	private Label label78;

	private TextBox txtOuterQtyPmDetail;

	internal UltraCombo CmbUomPmDetail;

	private Label label80;

	private Label label81;

	private ToolStripButton BtnContractPmDetailForm;

	private Label labelDCG;

	internal UltraCombo CmbDocumentCustomGroup;

	private ToolStripButton BtnPrintII;

	private Label label82;

	private Label label84;

	private Label label83;

	private Label label88;

	private Label label93;

	private Label label92;

	private Label label94;

	private Label label97;

	private Label label96;

	private Label label99;

	private Label label98;

	private Label label102;

	private Label label101;

	private Label label100;

	private Label label104;

	private Label label103;

	private Label label106;

	private Label label105;

	private Label label107;

	private Label label110;

	private Label label109;

	private Label label108;

	private Label label86;

	internal UltraCombo CmbNotifyParty3;

	internal UltraCombo CmbNotifyParty2;

	private Label label49;

	private Label label7;

	private TextBox txtSpecialInstructions;

	private TextBox txtCommodityDetail;

	internal UltraCombo CmbCategoryInDetail;

	private Panel panel34;

	private RadioButton RadCategory;

	private RadioButton RadType;

	private Label label85;

	internal UltraCombo CmbBrandInDetail;

	internal UltraCombo CmbPmItemInDetail;

	private Panel panel35;

	private RadioButton RadPmItemNameInDetail;

	private RadioButton RadPmItemCodeInDetail;

	private Label label113;

	private Label label112;

	private Label label95;

	internal UltraCombo CmbConsignee;

	private Panel panel36;

	private RadioButton RadBrandNameInDetail;

	private RadioButton RadBrandCodeInDetail;

	internal UltraCombo CmbPmDetailByItemCusInDetail;

	private TextBox txtPmDetailInDetail;

	private Label label115;

	private Label label114;

	internal UltraCombo CmbInnerUomInDetail;

	private Label label119;

	private TextBox txtPalletQtyInDetail;

	private TextBox txtQtyInnerInDetail;

	private Label label120;

	private Label label116;

	private RadioButton RadItemCodeOtherDetail;

	private RadioButton RadItemNameOtherDetail;

	private LinkLabel AdvanceDoFormLink;

	private CheckBox chkOpenContractSchedule;

	private Button btnDefineBankLocal;

	private Button btnDefineBankForeign;

	private ToolStripButton BtnPackingTypeDefine;

	private TabPage tabOtherCharges;

	internal GridEX grdOtherChargesDetail;

	private Panel panel37;

	private CtrlGrdBar ctrlgrdOtherChargesDetail;

	private Panel panel38;

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

	private TabPage tabOtherchargesHistory;

	internal GridEX grdOtherChargesHistory;

	private Panel panel39;

	private Label label87;

	private ToolStripButton btnGenerateInvoiceNos;

	internal UltraCombo CmbContractNo;

	private TextBox txtCustomerOrderNo;

	private Label label11;

	public frmExportContractIII(UserAccount user)
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
			base.AutoScrollMinSize = new Size(1250, 800);
			ScreenName = Conversion.ToString((object)ScreenName);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtSalesMan = dtSupplier.Clone();
			dtConsignee = dtSupplier.Clone();
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
			dtpcktype.Columns.Add("Id", typeof(int));
			dtpcktype.Columns.Add("Description");
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("CropYear");
			dtBrandForPMItem.Columns.Add("Id", typeof(int));
			dtBrandForPMItem.Columns.Add("ItemName");
			dtBrandForPMItem.Columns.Add("ItemCode");
			dtBrandForPMItem.Columns.Add("PackingTypeId", typeof(int));
			dtBrandForPMItem.Columns.Add("PackingType", typeof(string));
			dtBrandForPMItem.Columns.Add("PackUomId", typeof(int));
			dtBrandForPMItem.Columns.Add("PackUom", typeof(string));
			dtBrandForPMItem.Columns.Add("OuterQty", typeof(double));
			dtBrandForPMItem.Columns.Add("InnerQty", typeof(double));
			dtBrandForPMItem.Columns.Add("M_Ton", typeof(double));
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("PackTypeId", typeof(int));
			dtdetail.Columns.Add("PmItemId", typeof(int));
			dtdetail.Columns.Add("PmItemCode");
			dtdetail.Columns.Add("PmItemName");
			dtdetail.Columns.Add("BrandId", typeof(int));
			dtdetail.Columns.Add("BrandCode");
			dtdetail.Columns.Add("BrandName");
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("OuterUomId", typeof(int));
			dtdetail.Columns.Add("OuterUom");
			dtdetail.Columns.Add("OuterEquivalent", typeof(double));
			dtdetail.Columns.Add("OuterQtyEquivalent", typeof(double));
			dtdetail.Columns.Add("OuterQty", typeof(double));
			dtdetail.Columns.Add("InnerQty", typeof(double));
			dtdetail.Columns.Add("InnerUomId", typeof(int));
			dtdetail.Columns.Add("InnerUom");
			dtdetail.Columns.Add("InnerEquivalent", typeof(double));
			dtdetail.Columns.Add("PalletQty", typeof(double));
			dtdetail.Columns.Add("Rate", typeof(double));
			dtdetail.Columns.Add("RateUOMId");
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateEquivalent", typeof(double));
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("OtherItemAmount", typeof(double));
			dtdetail.Columns.Add("CommodityDetail");
			dtdetail.Columns.Add("PackingDetail");
			dtdetail.Columns.Add("ExImFarmingNTradeId", typeof(int));
			dtdetail.Columns.Add("FarmingNTrade");
			dtdetail.Columns.Add("ExImFarmingTypeId", typeof(int));
			dtdetail.Columns.Add("ExImTradeTypeId", typeof(int));
			dtdetail.Columns.Add("Remarks");
			dtOtherItemDetail.Columns.Add("ItemId");
			dtOtherItemDetail.Columns.Add("ItemName");
			dtOtherItemDetail.Columns.Add("Qty", typeof(double));
			dtOtherItemDetail.Columns.Add("Rate", typeof(double));
			dtOtherItemDetail.Columns.Add("Amount", typeof(double));
			dtOtherItemDetail.Columns.Add("Remarks");
			dtPaymnentDetail.Columns.Add("PaymentTermId");
			dtPaymnentDetail.Columns.Add("PaymentTerm");
			dtPaymnentDetail.Columns.Add("%ofTotal", typeof(double));
			dtPaymnentDetail.Columns.Add("FcyAmount", typeof(double));
			dtPaymnentDetail.Columns.Add("Remarks");
			dtPmDetail.Columns.Add("Id", typeof(int));
			dtPmDetail.Columns.Add("BrandId", typeof(int));
			dtPmDetail.Columns.Add("BrandCode");
			dtPmDetail.Columns.Add("BrandName");
			dtPmDetail.Columns.Add("BrandUomId", typeof(int));
			dtPmDetail.Columns.Add("BrandUom");
			dtPmDetail.Columns.Add("PackingTypeId", typeof(int));
			dtPmDetail.Columns.Add("PackingType");
			dtPmDetail.Columns.Add("BrandOuterQty", typeof(double));
			dtPmDetail.Columns.Add("BrandInnerQty", typeof(double));
			dtPmDetail.Columns.Add("PmItemId", typeof(int));
			dtPmDetail.Columns.Add("PmItemCode");
			dtPmDetail.Columns.Add("PmItemName");
			dtPmDetail.Columns.Add("PmItemUomId", typeof(int));
			dtPmDetail.Columns.Add("PmItemUom");
			dtPmDetail.Columns.Add("PmItemOuterQty", typeof(double));
			dtPmDetail.Columns.Add("PmItemInnerQty", typeof(double));
			dtPmDetail.Columns.Add("Rate", typeof(double));
			dtPmDetail.Columns.Add("Amount", typeof(double));
			dtPmDetail.Columns.Add("Remarks");
			dtOtherChargesDetail.Columns.Add("Id", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItemId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItem");
			dtOtherChargesDetail.Columns.Add("AddAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("LessAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("Remarks");
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
			ShipmentDocumentsFeature = CommonServices.GetERPFeatureById(12);
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable historyComboData = HistoryComboDbCall();
			DataTable exportcompanyData = new DataTable();
			if (AllowExportMultiCompanies)
			{
				exportcompanyData = ExImLcOrder.GetExportCompaniesByCompany(UserAccount.OrganizationId, UserAccount.CompanyId);
			}
			List<Bank> bankData = BanksDbCall();
			DataTable portData = PortsDbCall();
			DataTable deliverytermData = ExImDeliveryTerm.Getall();
			DataTable multiCurrencywithExchangeRate = MultiCurrency.GetMultiCurrencywithExchangeRate(UserAccount.CompanyId);
			DataTable packingTypeData = PackingTypeDbCall();
			DataTable farmingNTrade = ExImLcOrder.GetFarmingNTrade();
			DataTable paymentTermData = PaymentTermsDbCall();
			dtLastSavedRecordHeader = ExImLcOrder.LastSavedRecord(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId);
			DataTable exportchargesData = ExportCharges.ComboBind(UserAccount.OrganizationId, UserAccount.CompanyId);
			DataTable customerOrderNoData = CustomerOrderNoDbCall();
			GetConfigurationsFromGlobal();
			return new
			{
				HistoryComboData = historyComboData,
				DeliverytermData = deliverytermData,
				CurrencyData = multiCurrencywithExchangeRate,
				ExportcompanyData = exportcompanyData,
				FarmingData = farmingNTrade,
				PackingTypeData = packingTypeData,
				BankData = bankData,
				PortData = portData,
				PaymentTermData = paymentTermData,
				ExportchargesData = exportchargesData,
				CustomerOrderNoData = customerOrderNoData
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
				BtnSaveAs.Enabled = formrights.DoHaveSaveRight;
				btnUpdate.Enabled = formrights.DoHaveUpdateRights;
				BtnDelete.Enabled = formrights.DoHaveCanDelete;
				Print.Enabled = formrights.DoHavePrintRights;
				ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
				ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
				ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
				labelDCG.Visible = ShipmentDocumentsFeature;
				((Control)(object)CmbDocumentCustomGroup).Visible = ShipmentDocumentsFeature;
				if (ItemSearchWithNameOrCode)
				{
					rdSearchByCode.Checked = true;
					rdPackingItemCode.Checked = true;
					rabBrandName.Checked = false;
					RadBrandCodeInDetail.Checked = true;
				}
				else
				{
					rabBrandName.Checked = true;
					rdSearchByName.Checked = true;
					rdPackingItemName.Checked = true;
					RadBrandNameInDetail.Checked = true;
				}
				if (RecId == 0)
				{
					CustomerOrderNoBind(result.CustomerOrderNoData);
				}
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
				SalesPersonBind();
				BindSupplierName();
				ConsigneeAgainstCustomerBind(Conversion.ToInt(CmbCustomer.Value));
				ImporterAndExportBankBind(result.BankData);
				PortsBind(result.PortData);
				DeliveryTermBind(result.DeliverytermData);
				MultiCurrencyBind(result.CurrencyData);
				StatusBind();
				if (ShipmentDocumentsFeature)
				{
					DocumentcustomGroupBind(Conversion.ToInt(CmbCustomer.Value));
				}
				ItemCategoryOrTypeBind();
				ItemDtsFillFromGlobal();
				ItemNameBind();
				PackingTypeDetailBind(result.PackingTypeData);
				PmItemInDetailBind();
				BrandDtFillFromGlobal();
				BrandNameBind();
				CropDtFillFromGlobalAndBind();
				ItemCommodityDetailBind(ItemCommodityDetailDbCall());
				FarmingNTradeBind(result.FarmingData);
				PaymentTermsBind(result.PaymentTermData);
				OtherItemsBind();
				PackingItemBind();
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				ChargesNameChargeDetailBind(result.ExportchargesData);
				BindGrids();
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				btnsave.Visible = true;
				btnUpdate.Visible = false;
				BtnDelete.Visible = false;
				BtnSaveAs.Visible = false;
				txtDocDate.Focus();
				tabControl2.SelectedIndex = 0;
				btnUpdatePayment.Visible = false;
				btnCancelPayment.Visible = false;
				btnPackingUpdate.Visible = false;
				btnPackingCancel.Visible = false;
				LastSavedRecordBind();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void LastSavedRecordBind()
	{
		try
		{
			if (Conversion.ToInt(CmbSalePerson.Value) == 0 && dtLastSavedRecordHeader.Rows.Count > 0)
			{
				CmbSalePerson.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["SalesPersonId"]);
				CmbFCYCode.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["FcurrencyId"]);
				CmbCropYearInDetail.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["CropYearId"]);
				CmbPackingTypeInDetail.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["InvPackingMaterialTypeId"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.GetDataForDropDownFromExportContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "Customer"
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
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dt, "Id", "name", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			AllItemsBindonExportContract = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllItemsBindonExportContract"));
			PMGridInfoShouldNotCompulsoryOnContract = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PMGridInfoShouldNotCompulsoryOnContract"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
			SpecialInstructionsForExportContract = Conversion.ToString((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SpecialInstructionsForExportContract"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Default Crop Year"));
			if (cropYearId != 0)
			{
				CmbCropYearInDetail.Value = cropYearId;
			}
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Paking Type"));
			if (packingTypeId != 0)
			{
				CmbPackingTypeInDetail.Value = packingTypeId;
			}
			if (!string.IsNullOrWhiteSpace(SpecialInstructionsForExportContract) && string.IsNullOrWhiteSpace(txtSpecialInstructions.Text))
			{
				txtSpecialInstructions.Text = SpecialInstructionsForExportContract;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private DataTable CustomerOrderNoDbCall(string ContractNo = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			return GenerateExportInvoiceNos.GetFinalInvoiceNosForExportContract(new GenerateExportInvoiceNos
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinalInvoiceNo = ContractNo,
				DocumentTypeId = DocumentTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomerOrderNoBind(DataTable dt)
	{
		try
		{
			dtCustomerOrderNo.Clear();
			if (dt.Rows.Count > 0)
			{
				if (dtCustomerOrderNo.Columns.Count == 0)
				{
					dtCustomerOrderNo.Columns.Add("Id", typeof(int));
					dtCustomerOrderNo.Columns.Add("CustomerOrderNo");
				}
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtCustomerOrderNo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["FinalInvoiceNo"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbContractNo, dtCustomerOrderNo, "Id", "CustomerOrderNo", "Customer Order No", AllColumns: true, Conversion.ToInt(CmbContractNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void SalesPersonBind()
	{
		InfragisticsHelper.BindAndRetainSelection(CmbSalePerson, dtSalesMan, "Id", "CompanyName", "Sales Person", AllColumns: true, Conversion.ToInt(CmbSalePerson.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		if (dtSalesMan.Rows.Count > 0)
		{
			((UltraGridBase)CmbSalePerson).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			((UltraGridBase)CmbSalePerson).DisplayLayout.Bands[0].Columns[4].Hidden = true;
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSalesMan.Clear();
			dtSupplier.Clear();
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer;
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				bool isSupplier = item.CustomerGroupId == 7 && !item.IsSubSupCust;
				bool isSalesman = item.CustomerGroupId == 9;
				if (isSupplier || isSalesman)
				{
					object[] rowData = new object[7] { item.Id, item.CompanyName, item.PartyCode, item.GlAccountId, item.CityId, item.CityName, item.MobilePersonal };
					if (isSupplier)
					{
						dtSupplier.Rows.Add(rowData);
					}
					if (isSalesman)
					{
						dtSalesMan.Rows.Add(rowData);
					}
				}
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
		InfragisticsHelper.BindAndRetainSelection(CmbExporterbank, dtHome, "Id", "BranchName", "Exporter Bank", AllColumns: true, Conversion.ToInt(CmbExporterbank.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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

	private void MultiCurrencyBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbFCYCode, dt, "Id", "CurrencyCode", "Fcy Code", AllColumns: true, Conversion.ToInt(CmbFCYCode.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusBind()
	{
		try
		{
			DataTable dtstatus = new DataTable();
			dtstatus.Columns.Add("Id");
			dtstatus.Columns.Add("Status");
			dtstatus.Rows.Add(1, "Open");
			dtstatus.Rows.Add(2, "Complete");
			dtstatus.Rows.Add(3, "Cancel");
			DDL.BindDDL(dtstatus, cmbStatus, "Id", "Status", "Order Status", ZeroIndex: true);
			((UltraGridBase)cmbStatus).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentcustomGroupBind(int CustomerId)
	{
		try
		{
			DataTable lst = ClientCustomGroup.GetCustomGroupAllocatedToCustomerBySupplierCustomerId(UserAccount.OrganizationId, UserAccount.CompanyId, CustomerId);
			InfragisticsHelper.BindAndRetainSelection(CmbDocumentCustomGroup, lst, "CustomGroupId", "CustomGroup", "Custom Group", AllColumns: true, Conversion.ToInt(CmbDocumentCustomGroup.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomer_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemCommodityDetailBind(ItemCommodityDetailDbCall());
			if (ShipmentDocumentsFeature)
			{
				DocumentcustomGroupBind(Conversion.ToInt(CmbCustomer.Value));
			}
			ConsigneeAgainstCustomerBind(Conversion.ToInt(CmbCustomer.Value));
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
			if (((UltraGridBase)CmbDeliveryTerm).ActiveRow != null && Conversion.ToInt(CmbDeliveryTerm.Value) != 0)
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

	private void CmbFCYCode_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbFCYCode.Value);
			double Rate = 0.0;
			if (((UltraGridBase)CmbFCYCode).ActiveRow != null && Id > 0)
			{
				Rate = Conversion.ToDouble(((UltraDropDownBase)CmbFCYCode).SelectedRow.Cells[3].Value);
			}
			if (string.IsNullOrEmpty(txtExchangeRate.Text) || Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
			{
				txtExchangeRate.Text = Rate.ToString("#,##.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategoryOrTypeBind()
	{
		try
		{
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			IEnumerable<object> dtItemAll = ((!AllItemsBindonExportContract) ? allItems.Where((getGlobalAllItems item) => item.ItemClassId == 8) : allItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId != 14 && item.ItemTypeOfTypeId != 17));
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description", typeof(string));
			dt.Rows.Clear();
			if (RadCategory.Checked)
			{
				foreach (var category in (from row in dtItemAll
					where !string.IsNullOrEmpty(row.ItemCategory) && !dt.Rows.Cast<DataRow>().Any((DataRow r) => r["Description"].ToString() == row.ItemCategory)
					select new
					{
						Id = (object)Conversion.ToInt(row.ItemCategoryId),
						Description = (object)row.ItemCategory
					}).Distinct().ToList())
				{
					dt.Rows.Add((dynamic)category.Id, (dynamic)category.Description);
				}
			}
			else if (RadType.Checked)
			{
				foreach (var type in (from row in dtItemAll
					where !string.IsNullOrEmpty(row.ItemType) && !dt.Rows.Cast<DataRow>().Any((DataRow r) => r["Description"].ToString() == row.ItemType)
					select new
					{
						Id = (object)Conversion.ToInt(row.ItemTypeId),
						Description = (object)row.ItemType
					}).Distinct().ToList())
				{
					dt.Rows.Add((dynamic)type.Id, (dynamic)type.Description);
				}
			}
			int ID = Conversion.ToInt(CmbCategoryInDetail.Value);
			string DisplayName = (RadCategory.Checked ? "Item Category" : "Item Type");
			InfragisticsHelper.BindAndRetainSelection(CmbCategoryInDetail, dt, "Id", "Description", DisplayName, AllColumns: true, ID, insertDefaultRow: false, null, null, ActivateRow: false);
			((UltraGridBase)CmbCategoryInDetail).DisplayLayout.Bands[0].Columns[1].Width = 300;
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
				int ItemTypeId = Conversion.ToInt(row.ItemTypeId);
				int ItemCategoryId = Conversion.ToInt(row.ItemCategoryId);
				if (CategoryOrTypeId == 0 || (RadCategory.Checked && CategoryOrTypeId == ItemCategoryId) || (RadType.Checked && CategoryOrTypeId == ItemTypeId))
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

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemNameInDetail.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemNameInDetail, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemNameInDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemNameInDetail).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItemNameInDetail).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)CmbItemNameInDetail).DisplayLayout.Bands[0].Columns[8].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable PackingTypeDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ExImPackMaterilaType.Getall(new ExImPackMaterilaType
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

	private void PackingTypeDetailBind(DataTable lst)
	{
		try
		{
			dtpcktype.Clear();
			if (lst.Rows.Count > 0)
			{
				foreach (DataRow dr in lst.Rows)
				{
					dtpcktype.Rows.Add(dr["Id"], dr["Description"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbPackingTypeInDetail, dtpcktype, "Id", "Description", "Packing Type", AllColumns: true, Conversion.ToInt(CmbPackingTypeInDetail.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PmItemInDetailBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbPmItemInDetail.Value);
			string ValueMember = (RadPmItemNameInDetail.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (RadPmItemNameInDetail.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbPmItemInDetail, dtPMItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
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

	private void BrandNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbBrandInDetail.Value);
			string ValueMember = (RadBrandNameInDetail.Checked ? "BrandName" : "BrandCode");
			string DisplayMember = (RadBrandNameInDetail.Checked ? "Brand Name" : "Brand Code");
			InfragisticsHelper.BindAndRetainSelection(CmbBrandInDetail, dtBrand, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CropDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalCropYear> lst = clsGlobalVariables.globalCropYear;
			DatatableHelper.PopulateDataTable(dtCrop, lst, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			int Id = Conversion.ToInt(CmbCropYearInDetail.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCropYearInDetail, dtCrop, "Id", "CropYear", "Crop Year", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom(int detailId = 0)
	{
		try
		{
			string packUom = ((Control)(object)CmbOuterUomInDetail).Text?.Trim();
			string InnerUom = ((Control)(object)CmbInnerUomInDetail).Text?.Trim();
			string rateUom = ((Control)(object)CmbRateUomInDetail).Text?.Trim();
			((Control)(object)CmbOuterUomInDetail).Text = string.Empty;
			((Control)(object)CmbInnerUomInDetail).Text = string.Empty;
			((Control)(object)CmbRateUomInDetail).Text = string.Empty;
			int itemId = Conversion.ToInt(CmbItemNameInDetail.Value);
			if (itemId == 0)
			{
				return;
			}
			DataTable dtUom = CommonServices.GetUomScheduleByItemIdForExport(itemId);
			if (dtUom.Rows.Count == 0)
			{
				((Control)(object)CmbOuterUomInDetail).Text = string.Empty;
				((UltraGridBase)CmbOuterUomInDetail).DataSource = null;
				((Control)(object)CmbInnerUomInDetail).Text = string.Empty;
				((UltraGridBase)CmbInnerUomInDetail).DataSource = null;
				((Control)(object)CmbRateUomInDetail).Text = string.Empty;
				((UltraGridBase)CmbRateUomInDetail).DataSource = null;
				return;
			}
			DDL.BindDDL(dtUom, CmbOuterUomInDetail, "Id", "UOMCode", "Outer Uom", ZeroIndex: true);
			if (!string.IsNullOrEmpty(packUom) && dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == packUom))
			{
				((Control)(object)CmbOuterUomInDetail).Text = packUom;
			}
			DDL.BindDDL(dtUom, CmbInnerUomInDetail, "Id", "UOMCode", "Inner Uom", ZeroIndex: true);
			if (!string.IsNullOrEmpty(InnerUom) && dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == InnerUom))
			{
				((Control)(object)CmbInnerUomInDetail).Text = InnerUom;
			}
			DDL.BindDDL(dtUom, CmbRateUomInDetail, "Id", "UOMCode", "Rate Uom", ZeroIndex: true);
			((UltraGridBase)CmbRateUomInDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			if (!string.IsNullOrEmpty(rateUom) && dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == rateUom))
			{
				((Control)(object)CmbRateUomInDetail).Text = rateUom;
			}
			if (detailId != 0)
			{
				return;
			}
			string selectedRateText = ((Control)(object)CmbRateUomInDetail).Text?.ToUpper();
			if (!string.IsNullOrEmpty(rateUom) && (!(selectedRateText != "1000KG") || !(selectedRateText != "1000") || Conversion.ToInt(CmbRateUomInDetail.Value) != 0))
			{
				return;
			}
			int baseRateUomId = CommonServices.GetBaseRateUomId(dtUom);
			if (baseRateUomId > 0)
			{
				CmbRateUomInDetail.Value = baseRateUomId;
				return;
			}
			DataRow equivalentRow = dtUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<double>("Equivalent") == 1000.0);
			if (equivalentRow != null)
			{
				int rateUomId = Conversion.ToInt(equivalentRow["Id"]);
				if (rateUomId > 0)
				{
					CmbRateUomInDetail.Value = rateUomId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ItemCommodityDetailDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			return CommodityDetailItemCustomerWise.GetRemarks(new CommodityDetailItemCustomerWise
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(CmbItemNameInDetail.Value)
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
			InfragisticsHelper.BindAndRetainSelection(CmbComditiyDetailByItemCusInDetail, dtCommo, "Id", "Remarks", "Commodity Detail Remarks", AllColumns: true, Conversion.ToInt(CmbComditiyDetailByItemCusInDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbPmDetailByItemCusInDetail, dtPmDetail, "Id", "Remarks", "Pm Detail Remarks", AllColumns: true, Conversion.ToInt(CmbPmDetailByItemCusInDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FarmingNTradeBind(DataTable lst)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbFarmingNTradeInDetail, lst, "Id", "FarmingNTrade", "Farming N Trade", AllColumns: true, Conversion.ToInt(CmbFarmingNTradeInDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCategoryInDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemDtsFillFromGlobal(Conversion.ToInt(CmbCategoryInDetail.Value));
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadCategory_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemCategoryOrTypeBind();
			ItemDtsFillFromGlobal(Conversion.ToInt(CmbCategoryInDetail.Value));
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave_1(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbRateUomInDetail.Value) == 0)
			{
				dtLastSavedRecordByItemId = ExImLcOrder.LastSavedRecordByItemId(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbItemNameInDetail.Value));
			}
			bindRateUomAndItemPackUom();
			ItemCommodityDetailBind(ItemCommodityDetailDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadPmItemNameInDetail_CheckedChanged(object sender, EventArgs e)
	{
		PmItemInDetailBind();
	}

	private void RadBrandNameInDetail_CheckedChanged(object sender, EventArgs e)
	{
		BrandNameBind();
	}

	private void cmbcomditiyDetailByItemCus_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string remarks = Conversion.ToString((object)((Control)(object)CmbComditiyDetailByItemCusInDetail).Text);
			if (string.IsNullOrWhiteSpace(txtCommodityDetailInDetail.Text))
			{
				txtCommodityDetailInDetail.Text = remarks;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPmDetailByItemCus_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string remarks = Conversion.ToString((object)((Control)(object)CmbPmDetailByItemCusInDetail).Text);
			if (string.IsNullOrWhiteSpace(txtPmDetailInDetail.Text))
			{
				txtPmDetailInDetail.Text = remarks;
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
			int Id = Conversion.ToInt(CmbItemOtherDetail.Value);
			string ValueMember = (RadItemNameOtherDetail.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (RadItemNameOtherDetail.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemOtherDetail, dtPMItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void RadItemNameOtherDetail_CheckedChanged(object sender, EventArgs e)
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

	private void FilldtBrandForPMItem()
	{
		try
		{
			if (grdDetails.RowCount <= 0)
			{
				return;
			}
			dtBrandForPMItem.Clear();
			Dictionary<(int, int, int), (double, double)> accumulatedData = new Dictionary<(int, int, int), (double, double)>();
			Dictionary<(int, int, int), GridEXRow> rowCache = new Dictionary<(int, int, int), GridEXRow>();
			GridEXRow[] rows = grdDetails.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				int itemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				int packTypeId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
				int packSizeId = Conversion.ToInt(r2.Cells["OuterUomId"].Value);
				double noOfBags = Conversion.ToDouble(r2.Cells["OuterQty"].Value);
				double mTon = Conversion.ToDouble(r2.Cells["Qty/M.Ton"].Value);
				(int, int, int) key = (itemId, packTypeId, packSizeId);
				if (!accumulatedData.ContainsKey(key))
				{
					accumulatedData[key] = (noOfBags, mTon);
					rowCache[key] = r2;
				}
				else
				{
					accumulatedData[key] = (accumulatedData[key].Item1 + noOfBags, accumulatedData[key].Item2 + mTon);
				}
			}
			foreach (KeyValuePair<(int, int, int), (double, double)> entry in accumulatedData)
			{
				GridEXRow r = rowCache[entry.Key];
				dtBrandForPMItem.Rows.Add(entry.Key.Item1, Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToString(r.Cells["ItemCode"].Value), entry.Key.Item2, Conversion.ToString((object)r.Cells["PackTypeId"].Text), entry.Key.Item3, Conversion.ToString(r.Cells["OuterUomId"].Value), entry.Value.Item1, entry.Value.Item1 * Conversion.ToDouble(r.Cells["OuterQtyEquivalent"].Value), entry.Value.Item2);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BrandItemsBind()
	{
		try
		{
			FilldtBrandForPMItem();
			int Id = Conversion.ToInt(CmbBrandPmDetail.Value);
			string ValueMember = (rabBrandName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rabBrandName.Checked ? "Brand Name" : "Brand Code");
			InfragisticsHelper.BindAndRetainSelection(CmbBrandPmDetail, dtBrandForPMItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingItemBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemPmDetail.Value);
			string ValueMember = (rdPackingItemName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdPackingItemName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemPmDetail, dtPMItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			PmItemUomFromGlobalBind(Id);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PmItemUomFromGlobalBind(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)CmbUomPmDetail).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			InfragisticsHelper.BindAndRetainSelection(CmbUomPmDetail, dtUOM, "Id", "UOMCode", "Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void rdPackingItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			PackingItemBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rabBrandName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			BrandItemsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBrandForPmEntry_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbBrandPmDetail).ActiveRow != null && Conversion.ToInt(CmbBrandPmDetail.Value) > 0)
			{
				int BrandUomId = Conversion.ToInt(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["PackUomId"].Value);
				string BrandUom = Conversion.ToString(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["PackUom"].Value);
				int PackingTypeId = Conversion.ToInt(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["PackingTypeId"].Value);
				string PackingType = Conversion.ToString(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["PackingType"].Value);
				double BrandOuterQty = Conversion.ToDouble(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["OuterQty"].Value);
				double BrandInnerQty = Conversion.ToDouble(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["InnerQty"].Value);
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("BrandUomId", typeof(int));
				dtUom.Columns.Add("BrandUom", typeof(string));
				dtUom.Rows.Add(BrandUomId, BrandUom);
				DDL.BindDDL(dtUom, CmbBrandUomPmDetail, "BrandUomId", "BrandUom", "BrandUom", ZeroIndex: false);
				((UltraGridBase)CmbBrandUomPmDetail).Rows[0].Activate();
				DataTable dtPackingType = new DataTable();
				dtPackingType.Columns.Add("PackingTypeId", typeof(int));
				dtPackingType.Columns.Add("PackingType", typeof(string));
				dtPackingType.Rows.Add(PackingTypeId, PackingType);
				DDL.BindDDL(dtPackingType, CmbBrandPackingTypePmDetail, "PackingTypeId", "PackingType", "PackingType", ZeroIndex: false);
				((UltraGridBase)CmbBrandPackingTypePmDetail).Rows[0].Activate();
				txtBrandOuterQtyPmDetail.Text = BrandOuterQty.ToString("#,##.##");
				txtBrandInnerQtyPmDetail.Text = BrandInnerQty.ToString("#,##.##");
			}
			else
			{
				((UltraGridBase)CmbBrandUomPmDetail).DataSource = null;
				((Control)(object)CmbBrandUomPmDetail).Text = "";
				((UltraGridBase)CmbBrandPackingTypePmDetail).DataSource = null;
				((Control)(object)CmbBrandPackingTypePmDetail).Text = "";
				txtBrandOuterQtyPmDetail.Text = 0.ToString("#,##.##");
				txtBrandInnerQtyPmDetail.Text = 0.ToString("#,##.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbPMItem_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(CmbItemPmDetail.Value);
			if (ItemId > 0 && ItemId != ItemIdOfPreviousRate)
			{
				GetPmItemRate(ItemId);
			}
			PmItemUomFromGlobalBind(ItemId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPmItemRate(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			double dt = InventoryStockEvalautionDetail.GetLastRateByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = ItemId
			});
			if (dt > 0.0)
			{
				txtRatePmDetail.Text = dt.ToString("#,##.##");
				ItemIdOfPreviousRate = ItemId;
			}
			else
			{
				ItemIdOfPreviousRate = 0;
				txtRatePmDetail.Text = 0.ToString("#,##.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTermPT, dt, "Id", "lcOrderTerm", " Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTermPT.Value), insertDefaultRow: true, null, null, ActivateRow: true, 1);
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
			if (Conversion.ToInt(CmbPaymentTermPT.Value) != 0)
			{
				string Desc = Conversion.ToString(((UltraDropDownBase)CmbPaymentTermPT).SelectedRow.Cells[2].Value);
				if (Desc != null && Desc != string.Empty)
				{
					txtRemarksPT.Text = Desc;
				}
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PmGridOuterQtyValidation()
	{
		try
		{
			GridEXRow[] dataRows = grdPackingMaterialDetail.GetDataRows();
			if (dataRows.Count() <= 0)
			{
				return;
			}
			HashSet<(int, int, int)> uniqueIds = new HashSet<(int, int, int)>();
			Dictionary<(int, int, int), double> brandOuterQtyDict = (from row in dtBrandForPMItem.AsEnumerable()
				group row by (BrandId: Conversion.ToInt((object)row.Field<int>("Id")), BrandUomId: Conversion.ToInt((object)row.Field<int>("PackUomId")), PackingTypeId: Conversion.ToInt((object)row.Field<int>("PackingTypeId")))).ToDictionary((IGrouping<(int BrandId, int BrandUomId, int PackingTypeId), DataRow> group) => group.Key, (IGrouping<(int BrandId, int BrandUomId, int PackingTypeId), DataRow> group) => group.Sum((DataRow row) => Conversion.ToDouble((object)row.Field<double>("OuterQty"))));
			GridEXRow[] array = dataRows;
			foreach (GridEXRow row2 in array)
			{
				int selectedBrandId = Conversion.ToInt(row2.Cells["BrandId"].Value);
				int selectedBrandUomId = Conversion.ToInt(row2.Cells["BrandUomId"].Value);
				int selectedPackingTypeId = Conversion.ToInt(row2.Cells["PackingTypeId"].Value);
				if (!uniqueIds.Contains((selectedBrandId, selectedBrandUomId, selectedPackingTypeId)))
				{
					if (!brandOuterQtyDict.TryGetValue((selectedBrandId, selectedBrandUomId, selectedPackingTypeId), out var brandOuterQty))
					{
						string selectedBrand2 = Conversion.ToString(row2.Cells["BrandName"].Value);
						string selectedBrandUom2 = Conversion.ToString(row2.Cells["BrandUom"].Value);
						string selectedPackingType2 = Conversion.ToString(row2.Cells["PackingType"].Value);
						throw new Exception("Brand outer quantity for the selected combination (Brand Item '" + selectedBrand2 + "' with UOM '" + selectedBrandUom2 + "' and PackingType '" + selectedPackingType2 + "') could not be found.");
					}
					double gridOuterQtyPm = (from GridEXRow r in grdPackingMaterialDetail.GetRows()
						where Conversion.ToInt(r.Cells["BrandId"].Value) == selectedBrandId && Conversion.ToInt(r.Cells["BrandUomId"].Value) == selectedBrandUomId && Conversion.ToInt(r.Cells["PackingTypeId"].Value) == selectedPackingTypeId
						select r).Sum((GridEXRow r) => Conversion.ToDouble(r.Cells["PmItemOuterQty"].Value));
					if (gridOuterQtyPm > brandOuterQty)
					{
						double difference = gridOuterQtyPm - brandOuterQty;
						string selectedBrand = Conversion.ToString(row2.Cells["BrandName"].Value);
						string selectedBrandUom = Conversion.ToString(row2.Cells["BrandUom"].Value);
						string selectedPackingType = Conversion.ToString(row2.Cells["PackingType"].Value);
						throw new Exception("PmOuterQty can't be greater than Brand OuterQty.\n" + $"Brand OuterQty is {brandOuterQty}\n" + $"PmOuterQty is {gridOuterQtyPm}\n" + $"Difference is {difference}\n" + "For Brand Item '" + selectedBrand + "' with UOM '" + selectedBrandUom + "' and PackingType '" + selectedPackingType + "'");
					}
				}
				uniqueIds.Add((selectedBrandId, selectedBrandUomId, selectedPackingTypeId));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PmGridInnerQtyValidation()
	{
		try
		{
			GridEXRow[] dataRows = grdPackingMaterialDetail.GetDataRows();
			if (dataRows.Count() <= 0)
			{
				return;
			}
			HashSet<(int, int, int)> uniqueIds = new HashSet<(int, int, int)>();
			Dictionary<(int, int, int), double> brandOuterQtyDict = (from row in dtBrandForPMItem.AsEnumerable()
				group row by (BrandId: Conversion.ToInt((object)row.Field<int>("Id")), BrandUomId: Conversion.ToInt((object)row.Field<int>("PackUomId")), PackingTypeId: Conversion.ToInt((object)row.Field<int>("PackingTypeId")))).ToDictionary((IGrouping<(int BrandId, int BrandUomId, int PackingTypeId), DataRow> group) => group.Key, (IGrouping<(int BrandId, int BrandUomId, int PackingTypeId), DataRow> group) => group.Sum((DataRow row) => Conversion.ToDouble((object)row.Field<double>("InnerQty"))));
			GridEXRow[] array = dataRows;
			foreach (GridEXRow row2 in array)
			{
				int selectedBrandId = Conversion.ToInt(row2.Cells["BrandId"].Value);
				int selectedBrandUomId = Conversion.ToInt(row2.Cells["BrandUomId"].Value);
				int selectedPackingTypeId = Conversion.ToInt(row2.Cells["PackingTypeId"].Value);
				if (!uniqueIds.Contains((selectedBrandId, selectedBrandUomId, selectedPackingTypeId)))
				{
					if (!brandOuterQtyDict.TryGetValue((selectedBrandId, selectedBrandUomId, selectedPackingTypeId), out var brandOuterQty))
					{
						string selectedBrand2 = Conversion.ToString(row2.Cells["BrandName"].Value);
						string selectedBrandUom2 = Conversion.ToString(row2.Cells["BrandUom"].Value);
						string selectedPackingType2 = Conversion.ToString(row2.Cells["PackingType"].Value);
						throw new Exception("Brand Inner quantity for the selected combination (Brand Item '" + selectedBrand2 + "' with UOM '" + selectedBrandUom2 + "' and PackingType '" + selectedPackingType2 + "') could not be found.");
					}
					double gridOuterQtyPm = (from GridEXRow r in grdPackingMaterialDetail.GetRows()
						where Conversion.ToInt(r.Cells["BrandId"].Value) == selectedBrandId && Conversion.ToInt(r.Cells["BrandUomId"].Value) == selectedBrandUomId && Conversion.ToInt(r.Cells["PackingTypeId"].Value) == selectedPackingTypeId
						select r).Sum((GridEXRow r) => Conversion.ToDouble(r.Cells["PmItemInnerQty"].Value));
					if (gridOuterQtyPm > brandOuterQty)
					{
						double difference = gridOuterQtyPm - brandOuterQty;
						string selectedBrand = Conversion.ToString(row2.Cells["BrandName"].Value);
						string selectedBrandUom = Conversion.ToString(row2.Cells["BrandUom"].Value);
						string selectedPackingType = Conversion.ToString(row2.Cells["PackingType"].Value);
						throw new Exception("PmInnerQty can't be greater than Brand InnerQty.\n" + $"Brand InnerQty is {brandOuterQty}\n" + $"PmInnerQty is {gridOuterQtyPm}\n" + $"Difference is {difference}\n" + "For Brand Item '" + selectedBrand + "' with UOM '" + selectedBrandUom + "' and PackingType '" + selectedPackingType + "'");
					}
				}
				uniqueIds.Add((selectedBrandId, selectedBrandUomId, selectedPackingTypeId));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool DetailFormValidation()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbItemNameInDetail, "Item", FormHelper.ValidType.INT),
			((Control)(object)CmbPackingTypeInDetail, "Pack Type", FormHelper.ValidType.INT),
			((Control)(object)CmbCropYearInDetail, "Crop Year", FormHelper.ValidType.INT),
			(txtQtyMTonInDetail, "Qty/M.Ton", FormHelper.ValidType.Double),
			((Control)(object)CmbOuterUomInDetail, "Outer Uom", FormHelper.ValidType.INT),
			(txtOuterQtyInDetail, "Outer Qty", FormHelper.ValidType.Double),
			(txtRateInDetail, "Rate", FormHelper.ValidType.Double),
			((Control)(object)CmbRateUomInDetail, "Rate UOM", FormHelper.ValidType.INT),
			(txtAmountInDetail, "Amount", FormHelper.ValidType.Double)
		}))
		{
			return false;
		}
		return true;
	}

	private bool OtherItemFormValidation()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbItemOtherDetail, "Item", FormHelper.ValidType.INT),
			(txtQtyOtherDetail, "Qty", FormHelper.ValidType.Double),
			(txtRateOtherDetail, "Rate", FormHelper.ValidType.Double),
			(txtAmountOtherDetail, "Amount", FormHelper.ValidType.Double)
		}))
		{
			return false;
		}
		return true;
	}

	private bool PackingMaterialFormValidation()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbBrandPmDetail, "Brand Item", FormHelper.ValidType.INT),
			((Control)(object)CmbBrandUomPmDetail, "Brand Uom", FormHelper.ValidType.INT),
			((Control)(object)CmbBrandPackingTypePmDetail, "Item Type", FormHelper.ValidType.INT),
			(txtBrandOuterQtyPmDetail, "Brand Outer Qty", FormHelper.ValidType.Double),
			(txtBrandInnerQtyPmDetail, "Brand Inner Qty", FormHelper.ValidType.Double),
			((Control)(object)CmbItemPmDetail, "Item", FormHelper.ValidType.INT),
			((Control)(object)CmbUomPmDetail, "Pm Uom", FormHelper.ValidType.INT),
			(txtRatePmDetail, "Rate", FormHelper.ValidType.Double),
			(txtAmountPmDetail, "Amount", FormHelper.ValidType.Double)
		}))
		{
			return false;
		}
		if (Conversion.ToDouble((object)txtInnerQtyPmDetail.Text.Trim()) == 0.0 && Conversion.ToDouble((object)txtOuterQtyPmDetail.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Pm OuterQty or Pm InnerQty is required");
			txtOuterQtyPmDetail.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtOuterQtyPmDetail.Text.Trim()) != 0.0 && Conversion.ToDouble((object)txtInnerQtyPmDetail.Text.Trim()) != 0.0)
		{
			MessageBox.Show("You can either add the PM OuterQty or the PM InnerQty at a time, not both");
			txtInnerQtyPmDetail.Text = "";
			CalculatePackingAmount(Conversion.ToDouble((object)txtOuterQtyPmDetail.Text.Trim()));
			txtInnerQtyPmDetail.Focus();
			return false;
		}
		return true;
	}

	private bool PaymentFormValidation()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbPaymentTermPT, "Payment Term", FormHelper.ValidType.INT),
			(txtPercntOfTotalPT, "% of Total", FormHelper.ValidType.Double),
			(txtlFcyAmountPT, "Fcy Amount", FormHelper.ValidType.Double)
		}))
		{
			return false;
		}
		return true;
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

	private void BindGrids()
	{
		try
		{
			if (dtdetail != null && dtdetail.Columns.Count > 0)
			{
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
			}
			if (dtOtherItemDetail != null && dtOtherItemDetail.Columns.Count > 0)
			{
				grdotheritems.DataSource = dtOtherItemDetail;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems(grdotheritems);
			}
			if (dtPaymnentDetail != null && dtPaymnentDetail.Columns.Count > 0)
			{
				grdPaymentTerm.DataSource = dtPaymnentDetail;
				grdPaymentTerm.RetrieveStructure();
				grdSettingsPaymentTerm(grdPaymentTerm);
			}
			if (dtPmDetail != null && dtPmDetail.Columns.Count > 0)
			{
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				grdPackingMaterialDetail.RetrieveStructure();
				grdSettingsPM(grdPackingMaterialDetail);
			}
			if (dtOtherChargesDetail != null && dtOtherChargesDetail.Columns.Count > 0)
			{
				grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
				grdOtherChargesDetail.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddtoGrid()
	{
		try
		{
			if (DetailFormValidation())
			{
				int PmItemId = Conversion.ToInt(CmbPmItemInDetail.Value);
				string PmItemName = "";
				string PmItemCode = "";
				if (((UltraGridBase)CmbPmItemInDetail).ActiveRow != null && PmItemId > 0)
				{
					PmItemName = Conversion.ToString(((UltraDropDownBase)CmbPmItemInDetail).SelectedRow.Cells["ItemName"].Value);
					PmItemCode = Conversion.ToString(((UltraDropDownBase)CmbPmItemInDetail).SelectedRow.Cells["ItemCode"].Value);
				}
				int BrandId = Conversion.ToInt(CmbBrandInDetail.Value);
				string BrandName = "";
				string BrandCode = "";
				if (((UltraGridBase)CmbBrandInDetail).ActiveRow != null && BrandId > 0)
				{
					BrandName = Conversion.ToString(((UltraDropDownBase)CmbBrandInDetail).SelectedRow.Cells["BrandName"].Value);
					BrandCode = Conversion.ToString(((UltraDropDownBase)CmbBrandInDetail).SelectedRow.Cells["BrandCode"].Value);
				}
				int FarmingNTradeID = 0;
				string FarmingNTrade = "";
				int ExImFarmingTypeId = 0;
				int ExImTradeTypeId = 0;
				if (((UltraGridBase)CmbFarmingNTradeInDetail).ActiveRow != null && Conversion.ToInt(CmbFarmingNTradeInDetail.Value) > 0)
				{
					FarmingNTradeID = Conversion.ToInt(CmbFarmingNTradeInDetail.Value);
					FarmingNTrade = Conversion.ToString(((UltraDropDownBase)CmbFarmingNTradeInDetail).SelectedRow.Cells["FarmingNTrade"].Value);
					ExImFarmingTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFarmingNTradeInDetail).SelectedRow.Cells["ExImFarmingTypeId"].Value);
					ExImTradeTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFarmingNTradeInDetail).SelectedRow.Cells["ExImTradeTypeId"].Value);
				}
				dtdetail.Rows.Add(0, CmbItemNameInDetail.Value, ((UltraDropDownBase)CmbItemNameInDetail).SelectedRow.Cells["ItemCode"].Value, ((UltraDropDownBase)CmbItemNameInDetail).SelectedRow.Cells["ItemName"].Value, CmbPackingTypeInDetail.Value, PmItemId, PmItemCode, PmItemName, BrandId, BrandCode, BrandName, CmbCropYearInDetail.Value, Conversion.ToDouble((object)txtQtyMTonInDetail.Text), CmbOuterUomInDetail.Value, ((Control)(object)CmbOuterUomInDetail).Text, ((UltraDropDownBase)CmbOuterUomInDetail).SelectedRow.Cells["Equivalent"].Value, ((UltraDropDownBase)CmbOuterUomInDetail).SelectedRow.Cells["QtyEquivalent"].Value, Conversion.ToDouble((object)txtOuterQtyInDetail.Text), Conversion.ToDouble((object)txtQtyInnerInDetail.Text), CmbInnerUomInDetail.Value, ((Control)(object)CmbInnerUomInDetail).Text, ((UltraDropDownBase)CmbInnerUomInDetail).SelectedRow.Cells["Equivalent"].Value, Conversion.ToDouble((object)txtPalletQtyInDetail.Text), Conversion.ToDouble((object)txtRateInDetail.Text), CmbRateUomInDetail.Value, Conversion.ToString(((UltraDropDownBase)CmbRateUomInDetail).SelectedRow.Cells[1].Value), Conversion.ToString(((UltraDropDownBase)CmbRateUomInDetail).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtAmountInDetail.Text), 0, txtCommodityDetailInDetail.Text, txtPmDetailInDetail.Text, FarmingNTradeID, FarmingNTrade, ExImFarmingTypeId, ExImTradeTypeId, txtRemarksInDetail.Text);
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
				ResetDetail();
				CmbItemNameInDetail.Focus();
				InsterAmountandQtyMtonInHeader();
				ProportionateOtherItemAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		AddtoGrid();
	}

	private void grdDetails_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetails.CurrentRow;
			if (item != null)
			{
				((Control)(object)CmbCategoryInDetail).Text = "";
				ItemDtsFillFromGlobal();
				ItemNameBind();
				updateDetailIndex = item.RowIndex;
				CmbItemNameInDetail.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(item.Cells["Id"].Value));
				CmbPackingTypeInDetail.Value = item.Cells["PackTypeId"].Value;
				CmbPmItemInDetail.Value = item.Cells["PmItemId"].Value;
				CmbBrandInDetail.Value = item.Cells["BrandId"].Value;
				CmbCropYearInDetail.Value = item.Cells["CropYearId"].Value;
				txtQtyMTonInDetail.Text = item.Cells["Qty/M.Ton"].Value.ToString();
				CmbOuterUomInDetail.Value = item.Cells["OuterUomId"].Value;
				txtOuterQtyInDetail.Text = item.Cells["OuterQty"].Value.ToString();
				txtQtyInnerInDetail.Text = item.Cells["InnerQty"].Value.ToString();
				CmbInnerUomInDetail.Value = item.Cells["InnerUomId"].Value;
				txtPalletQtyInDetail.Text = item.Cells["PalletQty"].Value.ToString();
				txtRateInDetail.Text = item.Cells["Rate"].Value.ToString();
				CmbRateUomInDetail.Value = item.Cells["RateUOMId"].Value;
				txtAmountInDetail.Text = item.Cells["Amount"].Value.ToString();
				txtCommodityDetailInDetail.Text = item.Cells["CommodityDetail"].Value.ToString();
				txtPmDetailInDetail.Text = item.Cells["PackingDetail"].Value.ToString();
				if (Conversion.ToInt(item.Cells["ExImFarmingNTradeId"].Value) > 0)
				{
					CmbFarmingNTradeInDetail.Value = item.Cells["ExImFarmingNTradeId"].Value;
				}
				txtRemarksInDetail.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddInDetail.Visible = false;
				btnUpdateInDetail.Visible = true;
				btnCancelInDetail.Visible = true;
				CmbItemNameInDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetail();
	}

	private void grdComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdDetails.RootTable.Columns["CropYearId"], dtCrop, "Id", "CropYear", "Crop Year");
			GridEX_Helper.ComboBind(grdDetails.RootTable.Columns["PackTypeId"], dtpcktype, "Id", "Description", "Packing Type");
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
				"Id", "ItemId", "PmItemId", "BrandId", "OuterUomId", "OuterEquivalent", "OuterQtyEquivalent", "InnerUomId", "InnerEquivalent", "RateUOMId",
				"ExImFarmingNTradeId", "ExImFarmingTypeId", "ExImTradeTypeId"
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
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetails, 2, 4, hiddenColumns, editableColumns);
			grdDetails.RootTable.Columns["ItemName"].Width = 200;
			grdDetails.RootTable.Columns["PackTypeId"].Width = 120;
			grdComboBind();
			GridEX_Helper.AddButton(grdDetails, "Delete", "X", 20, 1);
			grdDetails.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetails.CurrentRow;
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

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				int PmItemId = Conversion.ToInt(CmbPmItemInDetail.Value);
				string PmItemName = "";
				string PmItemCode = "";
				if (((UltraGridBase)CmbPmItemInDetail).ActiveRow != null && PmItemId > 0)
				{
					PmItemName = Conversion.ToString(((UltraDropDownBase)CmbPmItemInDetail).SelectedRow.Cells["ItemName"].Value);
					PmItemCode = Conversion.ToString(((UltraDropDownBase)CmbPmItemInDetail).SelectedRow.Cells["ItemCode"].Value);
				}
				int BrandId = Conversion.ToInt(CmbBrandInDetail.Value);
				string BrandName = "";
				string BrandCode = "";
				if (((UltraGridBase)CmbBrandInDetail).ActiveRow != null && BrandId > 0)
				{
					BrandName = Conversion.ToString(((UltraDropDownBase)CmbBrandInDetail).SelectedRow.Cells["BrandName"].Value);
					BrandCode = Conversion.ToString(((UltraDropDownBase)CmbBrandInDetail).SelectedRow.Cells["BrandCode"].Value);
				}
				int FarmingNTradeID = Conversion.ToInt(CmbFarmingNTradeInDetail.Value);
				string FarmingNTrade = "";
				int ExImFarmingTypeId = 0;
				int ExImTradeTypeId = 0;
				if (((UltraGridBase)CmbFarmingNTradeInDetail).ActiveRow != null && FarmingNTradeID > 0)
				{
					FarmingNTrade = Conversion.ToString(((UltraDropDownBase)CmbFarmingNTradeInDetail).SelectedRow.Cells["FarmingNTrade"].Value);
					ExImFarmingTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFarmingNTradeInDetail).SelectedRow.Cells["ExImFarmingTypeId"].Value);
					ExImTradeTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFarmingNTradeInDetail).SelectedRow.Cells["ExImTradeTypeId"].Value);
				}
				dtdetail.Rows[updateDetailIndex]["ItemId"] = CmbItemNameInDetail.Value;
				dtdetail.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)CmbItemNameInDetail).SelectedRow.Cells["ItemCode"].Value;
				dtdetail.Rows[updateDetailIndex]["ItemName"] = ((UltraDropDownBase)CmbItemNameInDetail).SelectedRow.Cells["ItemName"].Value;
				dtdetail.Rows[updateDetailIndex]["PackTypeId"] = CmbPackingTypeInDetail.Value;
				dtdetail.Rows[updateDetailIndex]["PmItemId"] = PmItemId;
				dtdetail.Rows[updateDetailIndex]["PmItemCode"] = PmItemCode;
				dtdetail.Rows[updateDetailIndex]["PmItemName"] = PmItemName;
				dtdetail.Rows[updateDetailIndex]["BrandId"] = BrandId;
				dtdetail.Rows[updateDetailIndex]["BrandCode"] = BrandCode;
				dtdetail.Rows[updateDetailIndex]["BrandName"] = BrandName;
				dtdetail.Rows[updateDetailIndex]["CropYearId"] = CmbCropYearInDetail.Value;
				dtdetail.Rows[updateDetailIndex]["Qty/M.Ton"] = txtQtyMTonInDetail.Text;
				dtdetail.Rows[updateDetailIndex]["OuterUomId"] = CmbOuterUomInDetail.Value;
				dtdetail.Rows[updateDetailIndex]["OuterUom"] = ((Control)(object)CmbOuterUomInDetail).Text;
				dtdetail.Rows[updateDetailIndex]["OuterEquivalent"] = ((UltraDropDownBase)CmbOuterUomInDetail).SelectedRow.Cells["Equivalent"].Value;
				dtdetail.Rows[updateDetailIndex]["OuterQtyEquivalent"] = ((UltraDropDownBase)CmbOuterUomInDetail).SelectedRow.Cells["QtyEquivalent"].Value;
				dtdetail.Rows[updateDetailIndex]["OuterQty"] = txtOuterQtyInDetail.Text;
				dtdetail.Rows[updateDetailIndex]["InnerQty"] = txtQtyInnerInDetail.Text;
				dtdetail.Rows[updateDetailIndex]["InnerUomId"] = CmbInnerUomInDetail.Value;
				dtdetail.Rows[updateDetailIndex]["InnerUom"] = ((Control)(object)CmbInnerUomInDetail).Text;
				dtdetail.Rows[updateDetailIndex]["InnerEquivalent"] = ((UltraDropDownBase)CmbInnerUomInDetail).SelectedRow.Cells["Equivalent"].Value;
				dtdetail.Rows[updateDetailIndex]["PalletQty"] = txtPalletQtyInDetail.Text;
				dtdetail.Rows[updateDetailIndex]["Rate"] = txtRateInDetail.Text;
				dtdetail.Rows[updateDetailIndex]["RateUOMId"] = CmbRateUomInDetail.Value;
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)CmbRateUomInDetail).Text;
				dtdetail.Rows[updateDetailIndex]["RateEquivalent"] = ((UltraDropDownBase)CmbRateUomInDetail).SelectedRow.Cells["Equivalent"].Value;
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmountInDetail.Text;
				dtdetail.Rows[updateDetailIndex]["CommodityDetail"] = txtCommodityDetailInDetail.Text;
				dtdetail.Rows[updateDetailIndex]["PackingDetail"] = txtPmDetailInDetail.Text;
				dtdetail.Rows[updateDetailIndex]["ExImFarmingNTradeId"] = FarmingNTradeID;
				dtdetail.Rows[updateDetailIndex]["FarmingNTrade"] = FarmingNTrade;
				dtdetail.Rows[updateDetailIndex]["ExImFarmingTypeId"] = ExImFarmingTypeId;
				dtdetail.Rows[updateDetailIndex]["ExImTradeTypeId"] = ExImTradeTypeId;
				dtdetail.Rows[updateDetailIndex]["Remarks"] = txtRemarksInDetail.Text;
				grdDetails.DataSource = dtdetail;
				ResetDetail();
				InsterAmountandQtyMtonInHeader();
				ProportionateOtherItemAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ExImLcOrderPackingDetail vd = new ExImLcOrderPackingDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					FillDetailListCommonForInsertAndDelete(vd, r);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtdetail.Rows[r.RowIndex].Delete();
					r.Delete();
					grdDetails.UpdateData();
				}
				else
				{
					r.Delete();
					grdDetails.UpdateData();
				}
				InsterAmountandQtyMtonInHeader();
				ProportionateOtherItemAmount();
				return;
			}
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentTerm(GridEX grdP)
	{
		try
		{
			grdP.RootTable.Columns["PaymentTermId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdP, 2, 3);
			if (((Control)(object)grdP).Name == "grdPaymentTerm")
			{
				GridEX_Helper.AddButton(grdP, "Delete", "X", 20, 1);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddPayment_Click(object sender, EventArgs e)
	{
		try
		{
			if (!PaymentFormValidation())
			{
				return;
			}
			double prcntOfTotal = Conversion.ToDouble((object)txtPercntOfTotalPT.Text);
			if (prcntOfTotal > 100.0)
			{
				MessageBox.Show("Percent can't be Greater than 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtPercntOfTotalPT.Focus();
				return;
			}
			if (grdPaymentTerm.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPaymentTerm.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					if (Conversion.ToInt(rows[i].Cells["PaymentTermId"].Value) == Conversion.ToInt(CmbPaymentTermPT.Value))
					{
						MessageBox.Show("You Can't Add Same Payment Term", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						CmbPaymentTermPT.Focus();
						return;
					}
				}
				double TotalPaymentPrcnt = Conversion.ToDouble(grdPaymentTerm.GetTotal(grdPaymentTerm.RootTable.Columns["%ofTotal"], (AggregateFunction)2));
				if (TotalPaymentPrcnt + prcntOfTotal > 100.0)
				{
					MessageBox.Show("Toatl Percent can't be Greater than 100%.Remaing " + (100.0 - TotalPaymentPrcnt), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					txtPercntOfTotalPT.Text = (100.0 - TotalPaymentPrcnt).ToString();
					return;
				}
			}
			dtPaymnentDetail.Rows.Add(CmbPaymentTermPT.Value, ((Control)(object)CmbPaymentTermPT).Text, txtPercntOfTotalPT.Text, txtlFcyAmountPT.Text, txtRemarksPT.Text);
			grdPaymentTerm.DataSource = dtPaymnentDetail;
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm(grdPaymentTerm);
			ResetPaymentDetail();
			CmbPaymentTermPT.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdatePayment_Click(object sender, EventArgs e)
	{
		try
		{
			if (!PaymentFormValidation())
			{
				return;
			}
			double prcntOfTotal = Conversion.ToDouble((object)txtPercntOfTotalPT.Text);
			if (prcntOfTotal > 100.0)
			{
				MessageBox.Show("Percent can't be Greater than 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtPercntOfTotalPT.Focus();
				return;
			}
			if (grdPaymentTerm.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (item.RowIndex != updateDetailIndexPaymentDetail && Conversion.ToInt(item.Cells["PaymentTermId"].Value) == Conversion.ToInt(CmbPaymentTermPT.Value))
					{
						MessageBox.Show("You Can't Add Same Payment Term Twice", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				GridEXRow curr = grdPaymentTerm.GetRow(updateDetailIndexPaymentDetail);
				double num = Conversion.ToDouble(grdPaymentTerm.GetTotal(grdPaymentTerm.RootTable.Columns["%ofTotal"], (AggregateFunction)2));
				double currentRowPrcnt = Conversion.ToDouble(curr.Cells["%ofTotal"].Value);
				double t = Math.Abs(num - currentRowPrcnt);
				if (t + prcntOfTotal > 100.0)
				{
					MessageBox.Show("Toatl Percent can't be Greater than 100%.Remaing " + (100.0 - t), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					txtPercntOfTotalPT.Text = (100.0 - t).ToString();
					return;
				}
			}
			dtPaymnentDetail.Rows[updateDetailIndexPaymentDetail]["PaymentTermId"] = CmbPaymentTermPT.Value;
			dtPaymnentDetail.Rows[updateDetailIndexPaymentDetail]["PaymentTerm"] = ((Control)(object)CmbPaymentTermPT).Text;
			dtPaymnentDetail.Rows[updateDetailIndexPaymentDetail]["%ofTotal"] = txtPercntOfTotalPT.Text;
			dtPaymnentDetail.Rows[updateDetailIndexPaymentDetail]["FcyAmount"] = txtlFcyAmountPT.Text;
			dtPaymnentDetail.Rows[updateDetailIndexPaymentDetail]["Remarks"] = txtRemarksPT.Text;
			grdPaymentTerm.DataSource = dtPaymnentDetail;
			btnAddPayment.Visible = true;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
			ResetPaymentDetail();
			CmbPaymentTermPT.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelPayment_Click(object sender, EventArgs e)
	{
		ResetPaymentDetail();
	}

	private void btnAddPayment_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdPaymentTerm")
			{
				CmbPaymentTermPT.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelPayment_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl?.Name == "grdPaymentTerm")
			{
				CmbPaymentTermPT.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item != null)
			{
				updateDetailIndexPaymentDetail = item.RowIndex;
				CmbPaymentTermPT.Value = item.Cells["PaymentTermId"].Value.ToString();
				txtPercntOfTotalPT.Text = Conversion.ToDouble(item.Cells["%ofTotal"].Value).ToString("#,##0.###");
				txtlFcyAmountPT.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtRemarksPT.Text = Conversion.ToString(item.Cells["Remarks"].Value).ToString();
				btnAddPayment.Visible = false;
				btnUpdatePayment.Visible = true;
				btnCancelPayment.Visible = true;
				CmbPaymentTermPT.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndexPaymentDetail != -1)
				{
					throw new Exception("Reset the Detail first...");
				}
				dtPaymnentDetail.Rows[item.RowIndex].Delete();
				item.Delete();
			}
			grdPaymentTerm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ValidatePmQty(bool Update = false)
	{
		try
		{
			double brandOuterQty = Conversion.ToDouble((object)txtBrandOuterQtyPmDetail.Text);
			double brandInnerQty = Conversion.ToDouble((object)txtBrandInnerQtyPmDetail.Text);
			int selectedBrandId = Conversion.ToInt(CmbBrandPmDetail.Value);
			int selectedBrandUomId = Conversion.ToInt(CmbBrandUomPmDetail.Value);
			int selectedPackingTypeId = Conversion.ToInt(CmbBrandPackingTypePmDetail.Value);
			double currentOuterQtyPm = Conversion.ToDouble((object)txtOuterQtyPmDetail.Text);
			double currentInnerQtyPm = Conversion.ToDouble((object)txtInnerQtyPmDetail.Text);
			double gridOuterQtyPm = (from GridEXRow row in grdPackingMaterialDetail.GetRows()
				where Conversion.ToInt(row.Cells["BrandId"].Value) == selectedBrandId && Conversion.ToInt(row.Cells["BrandUomId"].Value) == selectedBrandUomId && Conversion.ToInt(row.Cells["PackingTypeId"].Value) == selectedPackingTypeId && (!Update || row.RowIndex != updateDetailIndexPmDetail)
				select row).Sum((GridEXRow row) => Conversion.ToDouble(row.Cells["PmItemOuterQty"].Value));
			double totalOuterQtyPm = gridOuterQtyPm + currentOuterQtyPm;
			if (Conversion.ToDouble((object)txtOuterQtyPmDetail.Text) > 0.0 && totalOuterQtyPm > brandOuterQty)
			{
				double difference = totalOuterQtyPm - brandOuterQty;
				if (gridOuterQtyPm > 0.0)
				{
					throw new Exception("PmOuterQty can't be greater than Brand OuterQty.\n" + $"Brand OuterQty is {brandOuterQty}\n" + $"PmOuterQty is {currentOuterQtyPm}\n" + $"PmOuterQty in Grid is {gridOuterQtyPm}\n" + $"Difference is {difference}");
				}
				throw new Exception("PmOuterQty can't be greater than Brand OuterQty.\n" + $"Brand OuterQty is {brandOuterQty}\n" + $"PmOuterQty is {currentOuterQtyPm}\n" + $"Difference is {difference}");
			}
			double gridInnerQtyPm = (from GridEXRow row in grdPackingMaterialDetail.GetRows()
				where Conversion.ToInt(row.Cells["BrandId"].Value) == selectedBrandId && Conversion.ToInt(row.Cells["BrandUomId"].Value) == selectedBrandUomId && Conversion.ToInt(row.Cells["PackingTypeId"].Value) == selectedPackingTypeId && (!Update || row.RowIndex != updateDetailIndexPmDetail)
				select row).Sum((GridEXRow row) => Conversion.ToDouble(row.Cells["PmItemInnerQty"].Value));
			double totalInnerQtyPm = gridInnerQtyPm + currentInnerQtyPm;
			if (Conversion.ToDouble((object)txtInnerQtyPmDetail.Text) > 0.0 && totalInnerQtyPm > brandInnerQty)
			{
				double difference2 = totalInnerQtyPm - brandInnerQty;
				if (gridInnerQtyPm > 0.0)
				{
					throw new Exception("PmInnerQty can't be greater than Brand InnerQty.\n" + $"Brand InnerQty is {brandInnerQty}\n" + $"PmInnerQty is {currentInnerQtyPm}\n" + $"PmInnerQty in Grid is {gridInnerQtyPm}\n" + $"Difference is {difference2}");
				}
				throw new Exception("PmInnerQty can't be greater than Brand InnerQty.\n" + $"Brand InnerQty is {brandInnerQty}\n" + $"PmInnerQty is {currentInnerQtyPm}\n" + $"Difference is {difference2}");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdSettingsPM(GridEX grdPM)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PmItemUomId", "PmItemId", "BrandId", "BrandUomId", "PackingTypeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPM, 2, 3, hiddenColumns);
			if (((Control)(object)grdPM).Name == "grdPackingMaterialDetail")
			{
				GridEX_Helper.AddButton(grdPM, "Delete", "X", 20, 1);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (PackingMaterialFormValidation())
			{
				ValidatePmQty();
				dtPmDetail.Rows.Add(0, Conversion.ToInt(CmbBrandPmDetail.Value), Conversion.ToString(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbBrandUomPmDetail.Value), Conversion.ToString((object)((Control)(object)CmbBrandUomPmDetail).Text), Conversion.ToInt(CmbBrandPackingTypePmDetail.Value), Conversion.ToString((object)((Control)(object)CmbBrandPackingTypePmDetail).Text), Conversion.ToDouble((object)txtBrandOuterQtyPmDetail.Text), Conversion.ToDouble((object)txtBrandInnerQtyPmDetail.Text), Conversion.ToInt(CmbItemPmDetail.Value), Conversion.ToString(((UltraDropDownBase)CmbItemPmDetail).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemPmDetail).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbUomPmDetail.Value), Conversion.ToString((object)((Control)(object)CmbUomPmDetail).Text), Conversion.ToDouble((object)txtOuterQtyPmDetail.Text), Conversion.ToDouble((object)txtInnerQtyPmDetail.Text), Conversion.ToDouble((object)txtRatePmDetail.Text), Conversion.ToDouble((object)txtAmountPmDetail.Text), Conversion.ToString((object)txtRemarksPmDetail.Text));
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				grdPackingMaterialDetail.RetrieveStructure();
				grdSettingsPM(grdPackingMaterialDetail);
				ResetPmDetail();
				CmbBrandPmDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPackingUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (PackingMaterialFormValidation())
			{
				ValidatePmQty(Update: true);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandId"] = Conversion.ToInt(CmbBrandPmDetail.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandCode"] = Conversion.ToString(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["ItemCode"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandName"] = Conversion.ToString(((UltraDropDownBase)CmbBrandPmDetail).SelectedRow.Cells["ItemName"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandUomId"] = Conversion.ToInt(CmbBrandUomPmDetail.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandUom"] = Conversion.ToString((object)((Control)(object)CmbBrandUomPmDetail).Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PackingTypeId"] = Conversion.ToInt(CmbBrandPackingTypePmDetail.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PackingType"] = Conversion.ToString((object)((Control)(object)CmbBrandPackingTypePmDetail).Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandOuterQty"] = Conversion.ToDouble((object)txtBrandOuterQtyPmDetail.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandInnerQty"] = Conversion.ToDouble((object)txtBrandInnerQtyPmDetail.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemId"] = Conversion.ToInt(CmbItemPmDetail.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemPmDetail).SelectedRow.Cells["ItemCode"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItemPmDetail).SelectedRow.Cells["ItemName"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemUomId"] = Conversion.ToInt(CmbUomPmDetail.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemUom"] = Conversion.ToString((object)((Control)(object)CmbUomPmDetail).Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemOuterQty"] = Conversion.ToDouble((object)txtOuterQtyPmDetail.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemInnerQty"] = Conversion.ToDouble((object)txtInnerQtyPmDetail.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["Rate"] = Conversion.ToDouble((object)txtRatePmDetail.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["Amount"] = Conversion.ToDouble((object)txtAmountPmDetail.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["Remarks"] = Conversion.ToString((object)txtRemarksPmDetail.Text);
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				btnPackingAdd.Visible = true;
				btnPackingUpdate.Visible = false;
				btnPackingCancel.Visible = false;
				ResetPmDetail();
				CmbItemPmDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPackingCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ResetPmDetail();
			btnPackingAdd.Visible = true;
			btnPackingUpdate.Visible = false;
			btnPackingCancel.Visible = false;
			updateDetailIndexPmDetail = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingAdd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdPackingItemName")
			{
				CmbItemPmDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingCancel_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdPackingItemName")
			{
				CmbItemPmDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterialDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			updateDetailIndexPmDetail = item.RowIndex;
			RowEnumerator enumerator = ((UltraGridBase)CmbBrandPmDetail).Rows.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					UltraGridRow row = enumerator.Current;
					if (Conversion.ToInt(row.Cells["Id"].Value) == Conversion.ToInt(item.Cells["BrandId"].Value) && Conversion.ToInt(row.Cells["PackUomId"].Value) == Conversion.ToInt(item.Cells["BrandUomId"].Value) && Conversion.ToInt(row.Cells["PackingTypeId"].Value) == Conversion.ToInt(item.Cells["PackingTypeId"].Value))
					{
						((UltraDropDownBase)CmbBrandPmDetail).SelectedRow = row;
						break;
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
			cmbBrandForPmEntry_Leave(null, null);
			CmbItemPmDetail.Value = Conversion.ToInt(item.Cells["PmItemId"].Value);
			CmbPMItem_Leave(null, null);
			CmbUomPmDetail.Value = Conversion.ToInt(item.Cells["PmItemUomId"].Value);
			txtOuterQtyPmDetail.Text = Conversion.ToDouble(item.Cells["PmItemOuterQty"].Value).ToString("#,##0.###");
			txtInnerQtyPmDetail.Text = Conversion.ToDouble(item.Cells["PmItemInnerQty"].Value).ToString("#,##0.###");
			txtRatePmDetail.Text = Conversion.ToDouble(item.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
			txtAmountPmDetail.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
			txtRemarksPmDetail.Text = item.Cells["Remarks"].Value.ToString();
			btnPackingAdd.Visible = false;
			btnPackingUpdate.Visible = true;
			btnPackingCancel.Visible = true;
			CmbBrandPmDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterialDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndexPmDetail != -1)
				{
					throw new Exception("Reset  the Detail first...");
				}
				dtPmDetail.Rows[item.RowIndex].Delete();
				item.Delete();
			}
			grdPackingMaterialDetail.UpdateData();
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
				dtOtherItemDetail.Rows.Add(CmbItemOtherDetail.Value, ((Control)(object)CmbItemOtherDetail).Text, Conversion.ToDouble((object)txtQtyOtherDetail.Text.Trim()), Conversion.ToDouble((object)txtRateOtherDetail.Text.Trim()), Conversion.ToDouble((object)txtAmountOtherDetail.Text.Trim()), txtRemarksOtherDetail.Text.Trim());
				grdotheritems.DataSource = dtOtherItemDetail;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems(grdotheritems);
				ResetOtherItems();
				InsterAmountandQtyMtonInHeader();
				CmbItemOtherDetail.Focus();
				ProportionateOtherItemAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdotheritems.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdotheritems_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdotheritems.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndexOther != -1)
					{
						throw new Exception("Reset  the Detail first...");
					}
					dtOtherItemDetail.Rows[item.RowIndex].Delete();
					item.Delete();
				}
				grdotheritems.UpdateData();
				ProportionateOtherItemAmount();
				InsterAmountandQtyMtonInHeader();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsOtherItems(GridEX grdOI)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "ItemId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdOI, 2, 3, hiddenColumns);
			if (((Control)(object)grdOI).Name == "grdotheritems")
			{
				GridEX_Helper.AddButton(grdOI, "Delete", "X", 20, 1);
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
				updateDetailIndexOther = item.RowIndex;
				CmbItemOtherDetail.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)CmbItemOtherDetail).Text = item.Cells["ItemName"].Text.ToString();
				txtQtyOtherDetail.Text = item.Cells["Qty"].Value.ToString();
				txtRateOtherDetail.Text = item.Cells["Rate"].Value.ToString();
				txtAmountOtherDetail.Text = item.Cells["Amount"].Value.ToString();
				txtRemarksOtherDetail.Text = item.Cells["Remarks"].Value.ToString();
				btnaddOtherDetail.Visible = false;
				btnUpdateOtherDetail.Visible = true;
				btnCancelOtherDetail.Visible = true;
				CmbItemOtherDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoCancel_Click(object sender, EventArgs e)
	{
		btnaddOtherDetail.Visible = true;
		btnUpdateOtherDetail.Visible = false;
		btnCancelOtherDetail.Visible = false;
		ResetOtherItems();
		updateDetailIndexOther = -1;
	}

	private void grdotheritems_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdotheritems.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndexOther != -1)
				{
					throw new Exception("Reset  the Detail first...");
				}
				dtOtherItemDetail.Rows[item.RowIndex].Delete();
				item.Delete();
			}
			grdotheritems.UpdateData();
			InsterAmountandQtyMtonInHeader();
			ProportionateOtherItemAmount();
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
				dtOtherItemDetail.Rows[updateDetailIndexOther]["ItemId"] = CmbItemOtherDetail.Value;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["ItemName"] = ((Control)(object)CmbItemOtherDetail).Text;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["Qty"] = txtQtyOtherDetail.Text;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["Rate"] = txtRateOtherDetail.Text;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["Amount"] = txtAmountOtherDetail.Text;
				dtOtherItemDetail.Rows[updateDetailIndexOther]["Remarks"] = txtRemarksOtherDetail.Text;
				grdotheritems.DataSource = dtOtherItemDetail;
				btnaddOtherDetail.Visible = true;
				btnUpdateOtherDetail.Visible = false;
				btnCancelOtherDetail.Visible = false;
				ResetOtherItems();
				CmbItemOtherDetail.Focus();
				InsterAmountandQtyMtonInHeader();
				ProportionateOtherItemAmount();
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
				dtOtherChargesDetail.Rows.Add(0, Conversion.ToInt(CmbChargesNameChargeDetail.Value), Conversion.ToString((object)((Control)(object)CmbChargesNameChargeDetail).Text), Conversion.ToDouble((object)txtAddAmountChargeDetail.Text), Conversion.ToDouble((object)txtLessAmountChargeDetail.Text), txtRemarksChargeDetail.Text.Trim());
				grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
				grdOtherChargesDetail.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesDetail);
				ResetOtherChargesDetail();
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
			List<string> hiddenColumns = new List<string> { "Id", "ChargesItemId" };
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
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CheckBomHeaderIds(DataTable dtDetail, DataTable dtPmDetail)
	{
		try
		{
			HashSet<int> DetailIds = (from row in dtDetail.AsEnumerable()
				where row.Field<int>("ItemId") > 0
				select row.Field<int>("ItemId")).ToHashSet();
			HashSet<int> PmIds = (from row in dtPmDetail.AsEnumerable()
				where row.Field<int>("BrandId") > 0
				select row.Field<int>("BrandId")).ToHashSet();
			List<int> missingInDetailIds = PmIds.Except(DetailIds).ToList();
			if (missingInDetailIds.Any())
			{
				List<string> missingBrandNames = (from row in dtPmDetail.AsEnumerable()
					where missingInDetailIds.Contains(row.Field<int>("BrandId"))
					select row.Field<string>("BrandName")).ToList();
				throw new Exception("Brand Items '" + string.Join(", ", missingBrandNames) + "' found in Packing Material Grid but not in Detail Grid.");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Expected O, but got Unknown
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Expected O, but got Unknown
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae2: Expected O, but got Unknown
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Expected O, but got Unknown
		//IL_0f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f59: Expected O, but got Unknown
		try
		{
			if (grdDetails.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbContractNo, "Export Contract No", FormHelper.ValidType.INT),
				((Control)(object)CmbSalePerson, "Sale Person", FormHelper.ValidType.INT),
				((Control)(object)CmbCustomer, "Customer", FormHelper.ValidType.INT),
				(txtCustomerOrderDate, "Order Date", FormHelper.ValidType.DateTime),
				(txtMTon, "M.Ton", FormHelper.ValidType.Double),
				(txtNoOfContainers, "No. of Containers", FormHelper.ValidType.Double),
				((Control)(object)CmbFCYCode, "FCY Code", FormHelper.ValidType.INT),
				(txtExchangeRate, "Exchange Rate", FormHelper.ValidType.Double),
				(txtFCYAmount, "FCY Amount", FormHelper.ValidType.Double),
				(txtDeliveryDays, "Delivery Days", FormHelper.ValidType.INT),
				((Control)(object)CmbDeliveryTerm, "Delivery Term", FormHelper.ValidType.INT),
				((Control)(object)CmbLoadingPort, "Loading Port", FormHelper.ValidType.INT),
				((Control)(object)CmbDestinationPort, "Destination Port", FormHelper.ValidType.INT)
			};
			if (AllowExportMultiCompanies)
			{
				controlsToValidate.Insert(0, ((Control)(object)CmbExportCompany, "Export Company", FormHelper.ValidType.INT));
			}
			if (!FormHelper.ValidateControls(controlsToValidate) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			double columnSum = GridEX_Helper.GetColumnSum(grdDetails, "Amount");
			double Qty = GridEX_Helper.GetColumnSum(grdDetails, "Qty/M.Ton");
			double OtherAmount = GridEX_Helper.GetColumnSum(grdotheritems, "Amount");
			double QtyMton = Conversion.ToDouble((object)txtMTon.Text.Trim());
			double AmountM = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			if (Conversion.ToDouble((object)columnSum) + Conversion.ToDouble((object)OtherAmount) != AmountM || Conversion.ToDouble((object)Qty) != QtyMton)
			{
				txtFCYAmount.Focus();
				throw new Exception("Please check FcAmount or NetWeight");
			}
			if (GridEX_Helper.GetColumnSum(grdPaymentTerm, "%ofTotal") < 100.0)
			{
				MessageBox.Show("Payment Percent Not Equal To 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				tabControl2.SelectedIndex = 1;
				CmbPaymentTermPT.Focus();
				return;
			}
			CheckBomHeaderIds(dtdetail, dtPmDetail);
			ExImLcOrder obj = new ExImLcOrder();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.ExportCompanyId = Conversion.ToInt(CmbExportCompany.Value);
			obj.Id = RecId;
			obj.LcOrderDate = Conversion.ToDateTime((object)txtDocDate.Value);
			obj.SalesPersonId = Conversion.ToInt(CmbSalePerson.Value);
			obj.SupCustId = Conversion.ToInt(CmbCustomer.Value);
			obj.ConsigneeId = Conversion.ToInt(CmbConsignee.Value);
			obj.LcOrderNo = Conversion.ToString((object)((Control)(object)CmbContractNo).Text.Trim());
			obj.CustomerOrderNo = Conversion.ToString((object)txtCustomerOrderNo.Text.Trim());
			obj.SalesContratDate = Conversion.ToDateTime((object)txtCustomerOrderDate.Value);
			obj.NotifyPartyId = Conversion.ToInt(CmbNotifyParty1.Value);
			obj.NotifyParty2Id = Conversion.ToInt(CmbNotifyParty2.Value);
			obj.NotifyParty3Id = Conversion.ToInt(CmbNotifyParty3.Value);
			obj.ImporterBankId = Conversion.ToInt(CmbImporterBank.Value);
			obj.ExporterBankId = Conversion.ToInt(CmbExporterbank.Value);
			obj.RemarksHeader = Conversion.ToString((object)txtSpecialInstructions.Text.Trim());
			obj.ShipmentStartDate = datShipmentStartDate.Value;
			obj.DeliveryDays = Conversion.ToInt((object)txtDeliveryDays.Text);
			obj.LastShipmentDate = Conversion.ToDateTime((object)txtShipmentDate.Value);
			obj.LotReference = Conversion.ToString((object)txtLotRefNo.Text.Trim());
			obj.LoadingPortId = Conversion.ToInt(CmbLoadingPort.Value);
			obj.DestinationPortId = Conversion.ToInt(CmbDestinationPort.Value);
			obj.CustomerApprovalDate = ApprovalDate.Value;
			obj.CustomerApprovalRemarks = txtApprovalRemarks.Text;
			obj.DeliveryTermId = Conversion.ToInt(CmbDeliveryTerm.Value);
			obj.DeliveryRemarks = Conversion.ToString((object)txtDeliveryRemarks.Text.Trim());
			obj.ProductSpecification = Conversion.ToString((object)txtProductSpecification.Text.Trim());
			obj.CommodityDetial = Conversion.ToString((object)txtCommodityDetail.Text.Trim());
			obj.NetWeightKgs = Conversion.ToDouble((object)txtMTon.Text.Trim());
			obj.NoOfContainers = ((Conversion.ToDouble((object)txtNoOfContainers.Text.Trim()) > 0.0) ? Conversion.ToDouble((object)txtNoOfContainers.Text.Trim()) : ((obj.NetWeightKgs > 0.0) ? Math.Round(obj.NetWeightKgs / 25.0, 3) : 0.0));
			obj.FcurrencyId = Conversion.ToInt(CmbFCYCode.Value);
			obj.ConversionRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			obj.FCurrencyAmount = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			obj.Status = ((Control)(object)cmbStatus).Text;
			obj.TotalAmountWithAdls = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			obj.FactoryLoadingDate = txtFactoryLoadingDate.Value;
			obj.CustomGroupId = Conversion.ToInt(CmbDocumentCustomGroup.Value);
			obj.LcOrderDetail = new List<ExImLcOrderPackingDetail>();
			obj.ExImLcOrderOtherItemslist = new List<ExImLcOrderOtherItems>();
			obj.ExImLcOrderPaymentTermsDetailslist = new List<ExImLcOrderPaymentTermsDetail>();
			obj.ExImLcContractPackingMaterialDetailslist = new List<ExImLcContractPackingMaterialDetail>();
			obj.ExImLcOrderOtherChargesDetailList = new List<ExImLcOrderOtherChargesDetail>();
			if (lstRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					obj.LcOrderDetail.Add(lstRemoveRecord[i]);
				}
			}
			string RemarksFromDtail = "";
			GridEXRow[] rows = grdDetails.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				ExImLcOrderPackingDetail vd5 = new ExImLcOrderPackingDetail();
				vd5.Id = ((RecId != 0) ? Conversion.ToInt(r5.Cells["Id"].Value) : 0);
				vd5.ActionTypeId = ((vd5.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd5, r5);
				FormHelper.ValidateField(vd5.ExImItemId, "Item", r5.RowIndex);
				FormHelper.ValidateField(vd5.InvPackingMaterialTypeId, "PackingType", r5.RowIndex);
				FormHelper.ValidateField(vd5.PmItemId, "Pm Item", r5.RowIndex);
				FormHelper.ValidateField(vd5.ItemBrandId, "Brand", r5.RowIndex);
				FormHelper.ValidateField(vd5.CropYearId, "CropYear", r5.RowIndex);
				FormHelper.ValidateField(vd5.NetWeight, "M.Ton", r5.RowIndex);
				FormHelper.ValidateField(vd5.UOMScheduleIdOuter, "Outer Uom", r5.RowIndex);
				FormHelper.ValidateField(vd5.RatePrice, "Rate", r5.RowIndex);
				FormHelper.ValidateField(vd5.UOMScheduleIdRate, "Rate Uom", r5.RowIndex);
				FormHelper.ValidateField(vd5.TotalAmount, "Amount", r5.RowIndex);
				if (string.IsNullOrWhiteSpace(obj.RemarksHeader))
				{
					RemarksFromDtail = vd5.Other_Description;
				}
				obj.LcOrderDetail.Add(vd5);
			}
			if (obj.RemarksHeader == "")
			{
				obj.RemarksHeader = RemarksFromDtail;
			}
			if (grdotheritems.RowCount > 0)
			{
				rows = grdotheritems.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					ExImLcOrderOtherItems vd4 = new ExImLcOrderOtherItems();
					vd4.otherItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
					vd4.oItemQty = Conversion.ToDecimal(r4.Cells["Qty"].Value);
					vd4.oItemRate = Conversion.ToDouble(r4.Cells["Rate"].Value);
					vd4.oItemAmount = Conversion.ToDecimal(r4.Cells["Amount"].Value);
					vd4.OtherItemRemarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					obj.ExImLcOrderOtherItemslist.Add(vd4);
				}
			}
			if (grdPaymentTerm.RowCount > 0)
			{
				rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					ExImLcOrderPaymentTermsDetail vd3 = new ExImLcOrderPaymentTermsDetail();
					vd3.PaymentTermId = Conversion.ToInt(r3.Cells["PaymentTermId"].Value);
					vd3.PrcntOfTotal = Conversion.ToDouble(r3.Cells["%ofTotal"].Value);
					vd3.FcyAmount = Conversion.ToDouble(r3.Cells["FcyAmount"].Value);
					vd3.Remarks = Conversion.ToString((object)r3.Cells["Remarks"].Text);
					obj.ExImLcOrderPaymentTermsDetailslist.Add(vd3);
				}
				obj.PaymentTermId = obj.ExImLcOrderPaymentTermsDetailslist[0].PaymentTermId;
				if (grdPackingMaterialDetail.RowCount > 0)
				{
					rows = grdPackingMaterialDetail.GetRows();
					foreach (GridEXRow r2 in rows)
					{
						ExImLcContractPackingMaterialDetail vd2 = new ExImLcContractPackingMaterialDetail();
						vd2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
						vd2.BrandId = Conversion.ToInt(r2.Cells["BrandId"].Value);
						vd2.BrandUomId = Conversion.ToInt(r2.Cells["BrandUomId"].Value);
						vd2.PackingTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
						vd2.BrandOuterQty = Conversion.ToDouble(r2.Cells["BrandOuterQty"].Value);
						vd2.BrandInnerQty = Conversion.ToDouble(r2.Cells["BrandInnerQty"].Value);
						vd2.ItemId = Conversion.ToInt(r2.Cells["PmItemId"].Value);
						vd2.ItemUomId = Conversion.ToInt(r2.Cells["PmItemUomId"].Value);
						vd2.Qty = Conversion.ToDecimal(r2.Cells["PmItemOuterQty"].Value);
						vd2.InnerQty = Conversion.ToDouble(r2.Cells["PmItemInnerQty"].Value);
						vd2.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
						vd2.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
						vd2.pmRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						FormHelper.ValidateField(vd2.BrandId, "Brand Item", r2.RowIndex, "Packing Material Grid");
						FormHelper.ValidateField(vd2.BrandUomId, "Brand Uom", r2.RowIndex, "Packing Material Grid");
						FormHelper.ValidateField(vd2.PackingTypeId, "Brand PackingType", r2.RowIndex, "Packing Material Grid");
						FormHelper.ValidateField(vd2.BrandOuterQty, "BrandOuterQty", r2.RowIndex, "Packing Material Grid");
						FormHelper.ValidateField(vd2.ItemId, "PmItem", r2.RowIndex, "Packing Material Grid");
						FormHelper.ValidateField(vd2.ItemUomId, "PmItemUom", r2.RowIndex, "Packing Material Grid");
						if (vd2.Qty == 0m && vd2.InnerQty == 0.0)
						{
							throw new Exception($"PmItemOuterQty or PmItemInnerQty is required in Packing Material Grid in Row No {r2.RowIndex + 1}");
						}
						FormHelper.ValidateField(vd2.Rate, "Rate", r2.RowIndex, "Packing Material Grid");
						FormHelper.ValidateField(vd2.Amount, "Amount", r2.RowIndex, "Packing Material Grid");
						obj.ExImLcContractPackingMaterialDetailslist.Add(vd2);
					}
				}
				else if (!PMGridInfoShouldNotCompulsoryOnContract)
				{
					throw new Exception("Packing Material Grid record not found");
				}
				FilldtBrandForPMItem();
				PmGridOuterQtyValidation();
				PmGridInnerQtyValidation();
				rows = grdOtherChargesDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImLcOrderOtherChargesDetail vd = new ExImLcOrderOtherChargesDetail();
					vd.ChargesItemId = Conversion.ToInt(r.Cells["ChargesItemId"].Value);
					vd.AddAmount = Conversion.ToDouble(r.Cells["AddAmount"].Value);
					vd.LessAmount = Conversion.ToDouble(r.Cells["LessAmount"].Value);
					vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					FormHelper.ValidateField(vd.ChargesItemId, "Charges Item", r.RowIndex, "Other Charges");
					if (vd.AddAmount == 0.0 && vd.LessAmount == 0.0)
					{
						throw new Exception($"AddAmount Or Less Amount is required in Other Charges at row No: {r.RowIndex + 1}");
					}
					obj.ExImLcOrderOtherChargesDetailList.Add(vd);
				}
				obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<ExImLcOrder>(obj, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
				obj.AttachmentsValues = AttachmentsValues;
				obj.CustomAttachmentsValues = CustomAttachmentsValues;
				int success = ExImLcOrder.Save(obj);
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully");
				}
				else
				{
					MessageBox.Show("Record Update Successfully");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, ScreenName);
				}
				Reset();
				if (ChkPreview.Checked)
				{
					GenerateReport(success);
				}
				if (chkOpenContractSchedule.Checked)
				{
					FormHelper.CanOpenForm("FrmExportSalesContractSchedule", delegate
					{
						frmSaleContractSchedule obj2 = new frmSaleContractSchedule(UserAccount);
						obj2.rdAll.Checked = true;
						obj2.ActionIdForContractGridBid = 0;
						obj2.Show();
						obj2.grdPendingContractByContractId(success);
						obj2.BringToFront();
					}, UserAccount);
				}
				return;
			}
			throw new Exception("Payment Term Grid record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnSaveAs_Click(object sender, EventArgs e)
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
	}

	private void FillDetailListCommonForInsertAndDelete(ExImLcOrderPackingDetail vd, GridEXRow r)
	{
		vd.ExImItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.InvPackingMaterialTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
		vd.ExImPackMaterialTypeName = Conversion.ToString((object)r.Cells["PackTypeId"].Text);
		vd.PmItemId = Conversion.ToInt(r.Cells["PmItemId"].Value);
		vd.PmItemCode = Conversion.ToString(r.Cells["PmItemCode"].Value);
		vd.PmItemName = Conversion.ToString(r.Cells["PmItemName"].Value);
		vd.ItemBrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
		vd.BrandCode = Conversion.ToString(r.Cells["BrandCode"].Value);
		vd.BrandName = Conversion.ToString(r.Cells["BrandName"].Value);
		vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
		vd.CropYear = Conversion.ToString((object)r.Cells["CropYearId"].Text);
		vd.NetWeight = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
		vd.OuterQty = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
		vd.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["OuterUomId"].Value);
		vd.PackUom = Conversion.ToString(r.Cells["OuterUom"].Value);
		vd.UOMScheduleIdOuterName = Conversion.ToDouble(r.Cells["OuterEquivalent"].Value);
		vd.QtyEquivalent = Conversion.ToDouble(r.Cells["OuterEquivalent"].Value);
		vd.NoOfUnit = Conversion.ToDouble(r.Cells["OuterQty"].Value);
		vd.InnerQty = Conversion.ToDouble(r.Cells["InnerQty"].Value);
		vd.UOMScheduleIdInner = Conversion.ToInt(r.Cells["InnerUomId"].Value);
		vd.InnerUom = Conversion.ToString(r.Cells["InnerUom"].Value);
		vd.UOMScheduleIdInnerName = Conversion.ToDouble(r.Cells["InnerEquivalent"].Value);
		vd.PalletQty = Conversion.ToDouble(r.Cells["PalletQty"].Value);
		vd.RatePrice = Conversion.ToDouble(r.Cells["Rate"].Value);
		vd.UOMScheduleIdRate = Conversion.ToInt(r.Cells["RateUOMId"].Value);
		vd.RateUom = Conversion.ToString(r.Cells["RateUOM"].Value);
		vd.UOMScheduleIdRateName = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
		vd.TotalAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
		vd.CommodityDetail = Conversion.ToString(r.Cells["CommodityDetail"].Value);
		vd.PackingDetail = Conversion.ToString(r.Cells["PackingDetail"].Value);
		vd.ExImFarmingNTradeId = Conversion.ToInt(r.Cells["ExImFarmingNTradeId"].Value);
		vd.ExImTradeTypeId = Conversion.ToInt(r.Cells["ExImTradeTypeId"].Value);
		vd.ExImFarmingTypeId = Conversion.ToInt(r.Cells["ExImFarmingTypeId"].Value);
		vd.Other_Description = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			RecId = ID;
			ExImLcOrder Obj = ExImLcOrder.GetByID(ID);
			if (Obj != null)
			{
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				BtnSaveAs.Visible = false;
				if (Obj.ExportCompanyId > 0)
				{
					CmbExportCompany.Value = Obj.ExportCompanyId;
				}
				txtDocDate.Value = Conversion.ToDateTime((object)Obj.LcOrderDate);
				CmbSalePerson.Value = Obj.SalesPersonId;
				CmbCustomer.Value = Obj.SupCustId;
				if (Obj.ConsigneeId > 0)
				{
					CmbConsignee.Value = Obj.ConsigneeId;
				}
				CustomerOrderNoBind(CustomerOrderNoDbCall(Obj.LcOrderNo));
				((Control)(object)CmbContractNo).Text = Conversion.ToString((object)Obj.LcOrderNo);
				txtCustomerOrderNo.Text = Obj.CustomerOrderNo;
				txtCustomerOrderDate.Value = Conversion.ToDateTime((object)Obj.SalesContratDate);
				if (Obj.NotifyPartyId > 0)
				{
					CmbNotifyParty1.Value = Obj.NotifyPartyId;
				}
				if (Obj.NotifyParty2Id > 0)
				{
					CmbNotifyParty2.Value = Obj.NotifyParty2Id;
				}
				if (Obj.NotifyParty3Id > 0)
				{
					CmbNotifyParty3.Value = Obj.NotifyParty3Id;
				}
				if (Obj.ImporterBankId > 0)
				{
					CmbImporterBank.Value = Obj.ImporterBankId;
				}
				if (Obj.ExporterBankId > 0)
				{
					CmbExporterbank.Value = Obj.ExporterBankId;
				}
				txtSpecialInstructions.Text = Conversion.ToString((object)Obj.RemarksHeader);
				datShipmentStartDate.Value = Conversion.ToDateTime((object)Obj.ShipmentStartDate);
				txtDeliveryDays.Text = Obj.DeliveryDays.ToString();
				txtShipmentDate.Value = Conversion.ToDateTime((object)Obj.LastShipmentDate);
				txtLotRefNo.Text = Conversion.ToString((object)Obj.LotReference);
				CmbLoadingPort.Value = Obj.LoadingPortId;
				CmbDestinationPort.Value = Obj.DestinationPortId;
				ApprovalDate.Value = Conversion.ToDateTime((object)Obj.CustomerApprovalDate);
				txtApprovalRemarks.Text = Obj.CustomerApprovalRemarks;
				CmbDeliveryTerm.Value = Obj.DeliveryTermId;
				txtDeliveryRemarks.Text = Conversion.ToString((object)Obj.DeliveryRemarks);
				txtProductSpecification.Text = Conversion.ToString((object)Obj.ProductSpecification);
				txtCommodityDetail.Text = Conversion.ToString((object)Obj.CommodityDetial);
				txtMTon.Text = Conversion.ToString((object)Obj.NetWeightKgs);
				txtNoOfContainers.Text = Conversion.ToString((object)Obj.NoOfContainers);
				CmbFCYCode.Value = Obj.FcurrencyId;
				txtExchangeRate.Text = Obj.ConversionRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtFCYAmount.Text = Obj.FCurrencyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				((Control)(object)cmbStatus).Text = Obj.Status;
				txtFactoryLoadingDate.Value = Conversion.ToDateTime((object)Obj.FactoryLoadingDate);
				CmbDocumentCustomGroup.Value = Obj.CustomGroupId;
				AttachmentsValues = Obj.AttachmentsValues;
				CustomAttachmentsValues = Obj.CustomAttachmentsValues;
				dtdetail.Rows.Clear();
				FillDetailFromListCommonForReadById(Obj, dtdetail);
				dtOtherItemDetail.Rows.Clear();
				FillOtherItemDetailFromListCommonForReadById(Obj, dtOtherItemDetail);
				dtPaymnentDetail.Rows.Clear();
				FillPaymentDetailFromListCommonForReadById(Obj, dtPaymnentDetail);
				dtPmDetail.Rows.Clear();
				FillPmDetailFromListCommonForReadById(Obj, dtPmDetail);
				dtOtherChargesDetail.Rows.Clear();
				FillOtherChargesDetailFromListCommonForReadById(Obj, dtOtherChargesDetail);
				BindGrids();
				FilldtBrandForPMItem();
				BrandItemsBind();
				FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
				if (ShipmentDocumentsFeature)
				{
					DocumentcustomGroupBind(Obj.SupCustId);
				}
				ProportionateOtherItemAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(ExImLcOrder invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.LcOrderDetail == null || invGrn.LcOrderDetail.Count == 0)
		{
			return;
		}
		foreach (ExImLcOrderPackingDetail detail in invGrn.LcOrderDetail)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id,
				detail.ExImItemId,
				detail.ItemCode,
				detail.ItemName,
				detail.InvPackingMaterialTypeId,
				detail.PmItemId,
				detail.PmItemCode,
				detail.PmItemName,
				detail.ItemBrandId,
				detail.BrandCode,
				detail.BrandName,
				detail.CropYearId,
				detail.NetWeight / 1000.0,
				detail.UOMScheduleIdOuter,
				detail.PackUom,
				detail.UOMScheduleIdOuterName,
				detail.QtyEquivalent,
				detail.NoOfUnit,
				detail.InnerQty,
				detail.UOMScheduleIdInner,
				detail.InnerUom,
				detail.UOMScheduleIdInnerName,
				detail.PalletQty,
				detail.RatePrice,
				detail.UOMScheduleIdRate,
				detail.RateUom,
				detail.UOMScheduleIdRateName,
				detail.TotalAmount,
				0,
				detail.CommodityDetail,
				detail.PackingDetail,
				detail.ExImFarmingNTradeId,
				detail.FarmingNTrade,
				detail.ExImFarmingTypeId,
				detail.ExImTradeTypeId,
				detail.Other_Description
			};
			if (AddExtraColumns)
			{
				rowValues.Add(detail.ExImPackMaterialTypeName);
				rowValues.Add(detail.CropYear);
			}
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillPmDetailFromListCommonForReadById(ExImLcOrder invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.ExImLcContractPackingMaterialDetailslist : null) == null || invGrn.ExImLcContractPackingMaterialDetailslist.Count == 0)
		{
			return;
		}
		foreach (ExImLcContractPackingMaterialDetail detail in invGrn.ExImLcContractPackingMaterialDetailslist)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.BrandId, detail.BrandCode, detail.BrandName, detail.BrandUomId, detail.BrandUomCode, detail.PackingTypeId, detail.PackingType, detail.BrandOuterQty, detail.BrandInnerQty,
				detail.ItemId, detail.ItemCode, detail.ItemName, detail.ItemUomId, detail.ItemUomCode, detail.Qty, detail.InnerQty, detail.Rate, detail.Amount, detail.pmRemarks
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillOtherItemDetailFromListCommonForReadById(ExImLcOrder invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.ExImLcOrderOtherItemslist : null) == null || invGrn.ExImLcOrderOtherItemslist.Count == 0)
		{
			return;
		}
		foreach (ExImLcOrderOtherItems detail in invGrn.ExImLcOrderOtherItemslist)
		{
			List<object> rowValues = new List<object> { detail.otherItemId, detail.ItemName, detail.oItemQty, detail.oItemRate, detail.oItemAmount, detail.OtherItemRemarks };
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillPaymentDetailFromListCommonForReadById(ExImLcOrder invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.ExImLcOrderPaymentTermsDetailslist : null) == null || invGrn.ExImLcOrderPaymentTermsDetailslist.Count == 0)
		{
			return;
		}
		foreach (ExImLcOrderPaymentTermsDetail detail in invGrn.ExImLcOrderPaymentTermsDetailslist)
		{
			List<object> rowValues = new List<object> { detail.PaymentTermId, detail.LcOrderTerm, detail.PrcntOfTotal, detail.FcyAmount, detail.Remarks };
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillOtherChargesDetailFromListCommonForReadById(ExImLcOrder invGrn, DataTable dt)
	{
		if (invGrn == null)
		{
			return;
		}
		List<ExImLcOrderOtherChargesDetail> details = invGrn.ExImLcOrderOtherChargesDetailList;
		if (details == null || !details.Any())
		{
			return;
		}
		foreach (ExImLcOrderOtherChargesDetail detail in details)
		{
			List<object> rowValues = new List<object> { detail.Id, detail.ChargesItemId, detail.ChargesItemName, detail.AddAmount, detail.LessAmount, detail.Remarks };
			dt.Rows.Add(rowValues.ToArray());
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

	private void frmExportContractIII_FormClosing(object sender, FormClosingEventArgs e)
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
			PMGridInfoShouldNotCompulsoryOnContract = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PMGridInfoShouldNotCompulsoryOnContract"));
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "AllItems", "MultiCurrency", "CropYear", "JobLot", "InvPackingType", "AllBrands" };
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
			CustomerOrderNoBind(CustomerOrderNoDbCall());
			SalesPersonBind();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			ConsigneeAgainstCustomerBind(Conversion.ToInt(CmbCustomer.Value));
			ImporterAndExportBankBind(BanksDbCall());
			PortsBind(PortsDbCall());
			DeliveryTermBind(ExImDeliveryTerm.Getall());
			MultiCurrencyBind(MultiCurrency.GetMultiCurrencywithExchangeRate(UserAccount.CompanyId));
			if (ShipmentDocumentsFeature)
			{
				DocumentcustomGroupBind(Conversion.ToInt(CmbCustomer.Value));
			}
			ItemCategoryOrTypeBind();
			ItemDtsFillFromGlobal();
			ItemNameBind();
			PackingTypeDetailBind(PackingTypeDbCall());
			PmItemInDetailBind();
			BrandDtFillFromGlobal();
			BrandNameBind();
			CropDtFillFromGlobalAndBind();
			ItemCommodityDetailBind(ItemCommodityDetailDbCall());
			FarmingNTradeBind(ExImLcOrder.GetFarmingNTrade());
			PaymentTermsBind(PaymentTermsDbCall());
			OtherItemsBind();
			PackingItemBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			LastSavedRecordBind();
			ChargesNameChargeDetailBind(ExportCharges.ComboBind(UserAccount.OrganizationId, UserAccount.CompanyId));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnDelete.Visible = false;
			BtnSaveAs.Visible = false;
			RecId = 0;
			((Control)(object)CmbSalePerson).Text = string.Empty;
			((Control)(object)CmbCustomer).Text = string.Empty;
			((Control)(object)CmbConsignee).Text = string.Empty;
			((Control)(object)CmbContractNo).Text = string.Empty;
			((Control)(object)CmbNotifyParty1).Text = string.Empty;
			((Control)(object)CmbNotifyParty2).Text = string.Empty;
			((Control)(object)CmbNotifyParty3).Text = string.Empty;
			((Control)(object)CmbImporterBank).Text = string.Empty;
			((Control)(object)CmbExporterbank).Text = string.Empty;
			txtCustomerOrderNo.Text = string.Empty;
			txtDeliveryDays.Text = string.Empty;
			if (txtDocDate.Value.Date > DateTime.Now)
			{
				datShipmentStartDate.Value = txtDocDate.Value;
				txtShipmentDate.Value = txtDocDate.Value;
			}
			else
			{
				datShipmentStartDate.Value = DateTime.Now;
				txtShipmentDate.Value = DateTime.Now;
			}
			txtLotRefNo.Text = string.Empty;
			((Control)(object)CmbLoadingPort).Text = string.Empty;
			((Control)(object)CmbDestinationPort).Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			((Control)(object)CmbDeliveryTerm).Text = string.Empty;
			txtApprovalRemarks.Text = string.Empty;
			txtDeliveryRemarks.Text = string.Empty;
			txtNoOfContainers.Text = string.Empty;
			((Control)(object)CmbFCYCode).Text = string.Empty;
			txtFCYAmount.Text = string.Empty;
			((Control)(object)cmbStatus).Text = string.Empty;
			((Control)(object)CmbDocumentCustomGroup).Text = string.Empty;
			txtProductSpecification.Text = string.Empty;
			txtCommodityDetail.Text = string.Empty;
			txtSpecialInstructions.Text = string.Empty;
			txtMTon.Text = string.Empty;
			ResetDetail();
			dtdetail.Rows.Clear();
			ResetPaymentDetail();
			dtPaymnentDetail.Rows.Clear();
			ResetOtherItems();
			dtOtherItemDetail.Clear();
			dtPmDetail.Rows.Clear();
			dtBrandForPMItem.Clear();
			ResetPmDetail();
			ResetOtherChargesDetail();
			dtOtherChargesDetail.Clear();
			BindGrids();
			dtLastSavedRecordHeader = ExImLcOrder.LastSavedRecord(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			CustomerOrderNoBind(CustomerOrderNoDbCall());
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			tabControl2.SelectedIndex = 0;
			txtDocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbCategoryInDetail).Text = string.Empty;
			((Control)(object)CmbItemNameInDetail).Text = string.Empty;
			((Control)(object)CmbPackingTypeInDetail).Text = string.Empty;
			((Control)(object)CmbPmItemInDetail).Text = string.Empty;
			((Control)(object)CmbBrandInDetail).Text = string.Empty;
			((Control)(object)CmbCropYearInDetail).Text = string.Empty;
			txtQtyMTonInDetail.Text = string.Empty;
			((Control)(object)CmbOuterUomInDetail).Text = string.Empty;
			txtOuterQtyInDetail.Text = string.Empty;
			((Control)(object)CmbInnerUomInDetail).Text = string.Empty;
			txtQtyInnerInDetail.Text = string.Empty;
			txtPalletQtyInDetail.Text = string.Empty;
			((Control)(object)CmbRateUomInDetail).Text = string.Empty;
			txtRateInDetail.Text = string.Empty;
			txtAmountInDetail.Text = string.Empty;
			((Control)(object)CmbComditiyDetailByItemCusInDetail).Text = string.Empty;
			((Control)(object)CmbPmDetailByItemCusInDetail).Text = string.Empty;
			txtRemarksInDetail.Text = string.Empty;
			((Control)(object)CmbFarmingNTradeInDetail).Text = string.Empty;
			BtnAddInDetail.Visible = true;
			btnUpdateInDetail.Visible = false;
			btnCancelInDetail.Visible = false;
			CmbItemNameInDetail.Focus();
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
			updateDetailIndexPaymentDetail = -1;
			((Control)(object)CmbPaymentTermPT).Text = string.Empty;
			txtRemarksPT.Text = string.Empty;
			txtPercntOfTotalPT.Text = string.Empty;
			txtlFcyAmountPT.Text = string.Empty;
			btnAddPayment.Visible = true;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetPmDetail()
	{
		try
		{
			updateDetailIndexPmDetail = -1;
			((Control)(object)CmbBrandPmDetail).Text = string.Empty;
			((UltraGridBase)CmbBrandUomPmDetail).DataSource = null;
			((Control)(object)CmbBrandUomPmDetail).Text = "";
			((UltraGridBase)CmbBrandPackingTypePmDetail).DataSource = null;
			((Control)(object)CmbBrandPackingTypePmDetail).Text = "";
			txtBrandOuterQtyPmDetail.Text = 0.ToString("#,##.##");
			txtBrandInnerQtyPmDetail.Text = 0.ToString("#,##.##");
			((Control)(object)CmbItemPmDetail).Text = string.Empty;
			((Control)(object)CmbUomPmDetail).Text = string.Empty;
			txtOuterQtyPmDetail.Text = string.Empty;
			txtInnerQtyPmDetail.Text = string.Empty;
			txtRatePmDetail.Text = string.Empty;
			txtAmountPmDetail.Text = string.Empty;
			txtRemarksPmDetail.Text = string.Empty;
			CmbBrandPmDetail.Focus();
			btnPackingAdd.Visible = true;
			btnPackingUpdate.Visible = false;
			btnPackingCancel.Visible = false;
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
			updateDetailIndexOther = -1;
			((Control)(object)CmbItemOtherDetail).Text = string.Empty;
			txtQtyOtherDetail.Text = string.Empty;
			txtRateOtherDetail.Text = string.Empty;
			txtAmountOtherDetail.Text = string.Empty;
			txtRemarksOtherDetail.Text = string.Empty;
			btnaddOtherDetail.Visible = true;
			btnUpdateOtherDetail.Visible = false;
			btnCancelOtherDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void txtSpecialInstructions_Enter(object sender, EventArgs e)
	{
	}

	private void txtLotRefNo_Enter(object sender, EventArgs e)
	{
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
			grddetailhistory.DataSource = null;
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
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = DocumentTypeId.ToString();
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			obj.ApprovedFilter = "All";
			DataTable dt = ExImLcOrder.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				if (dtcol.Columns.Count == 0)
				{
					dtcol.Columns.Add("Id", typeof(int));
					dtcol.Columns.Add("ContractNo", typeof(string));
					dtcol.Columns.Add("ContractDate", typeof(DateTime));
					dtcol.Columns.Add("CustomerName", typeof(string));
					dtcol.Columns.Add("CustomerOrderNo", typeof(string));
					dtcol.Columns.Add("CustomerOrderDate", typeof(DateTime));
					dtcol.Columns.Add("LotReference", typeof(string));
					dtcol.Columns.Add("M.Tons", typeof(double));
					dtcol.Columns.Add("NoOfContainers", typeof(int));
					dtcol.Columns.Add("FcyCode", typeof(string));
					dtcol.Columns.Add("FcyAmount", typeof(double));
					dtcol.Columns.Add("LastShipmentDate", typeof(DateTime));
					dtcol.Columns.Add("NotifyParty", typeof(string));
					dtcol.Columns.Add("DeliveryTerm", typeof(string));
					dtcol.Columns.Add("PaymentTerm", typeof(string));
					dtcol.Columns.Add("LoadingPort", typeof(string));
					dtcol.Columns.Add("DestinationPort", typeof(string));
					dtcol.Columns.Add("Commodity_Description", typeof(string));
					dtcol.Columns.Add("EntryUser");
					dtcol.Columns.Add("EntryDate", typeof(DateTime));
					dtcol.Columns.Add("ModifyUser");
					dtcol.Columns.Add("ModifyDate", typeof(DateTime));
					dtcol.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LcOrderNo"], Conversion.ToDateTime(dt.Rows[i]["SalesContratDate"]).ToShortDateString(), dt.Rows[i]["CustomerName"], dt.Rows[i]["CustomerOrderNo"], Conversion.ToDateTime(dt.Rows[i]["LcOrderDate"]).ToShortDateString(), dt.Rows[i]["LotReference"], dt.Rows[i]["NetKgs"], dt.Rows[i]["NoOfContr"], dt.Rows[i]["CurrencyCode"], dt.Rows[i]["FCurrencyAmount"], Conversion.ToDateTime(dt.Rows[i]["LastShipmentDate"]).ToShortDateString(), dt.Rows[i]["NotifyParty"], dt.Rows[i]["DeliveryTerm"], dt.Rows[i]["PaymentTerm"], dt.Rows[i]["LoadingPort"], dt.Rows[i]["PortName"], dt.Rows[i]["Commodity_Description"], dt.Rows[i]["EntryUserName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["ModifyUserName"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
				grddetailhistory.ClearStructure();
				grdPaymentHistory.ClearStructure();
				grdPMHistory.ClearStructure();
				grdOtherItemHistory.ClearStructure();
				grdOtherChargesHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["Commodity_Description"].Width = 300;
			GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(DataGridHistory, "SaveAs", "SaveAs", 60, 1);
			GridEX_Helper.AddButton(DataGridHistory, "Print", "Print", 50, 2);
			GridEX_Helper.AddButton(DataGridHistory, "PrintII", "Print II", 50, 3);
			GridEX_Helper.AddButton(DataGridHistory, "PrintIII", "Print III", 50, 4);
			GridEX_Helper.AddButton(DataGridHistory, "Print501A", "Print 501A", 80, 5);
			GridEX_Helper.AddButton(DataGridHistory, "Print523", "Print 523", 80, 6);
			DataGridHistory.FrozenColumns = 6;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Print")
				{
					GenerateReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Print501A")
				{
					CommonServices.SaleContractReportsSlipNew501A(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "PrintII")
				{
					CommonServices.SaleContractReportsSlipNew501(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "PrintIII")
				{
					CommonServices.SaleContractReportsSlipNew501B(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Edit")
				{
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
				}
				else if (e.Column.Key == "SaveAs")
				{
					HistoryGridSaveAsButtonClick(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Print523")
				{
					CommonServices.SalesContractExportSlip523(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSaveAsButtonClick(int CurrId)
	{
		ReadById(CurrId);
		RecId = 0;
		CustomerOrderNoBind(CustomerOrderNoDbCall());
		datShipmentStartDate.Value = DateTime.Now;
		txtDocDate.Value = DateTime.Now;
		txtCustomerOrderDate.Value = DateTime.Now;
		ApprovalDate.Value = DateTime.Now;
		BtnSaveAs.Visible = true;
		btnsave.Visible = false;
		btnUpdate.Visible = false;
		BtnDelete.Visible = false;
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
			ExImLcOrder Obj = ExImLcOrder.GetByID(Id);
			if (Obj == null || Obj.LcOrderDetail.Count == 0)
			{
				grddetailhistory.ClearStructure();
				grdPaymentHistory.ClearStructure();
				grdPMHistory.ClearStructure();
				grdOtherItemHistory.ClearStructure();
				grdOtherChargesHistory.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtdetail.Clone();
			dtDetailHistory.Columns.Add("PackingType");
			dtDetailHistory.Columns.Add("CropYear");
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory, AddExtraColumns: true);
			grddetailhistory.DataSource = dtDetailHistory;
			grddetailhistory.RetrieveStructure();
			grddetailhistorySettings();
			DataTable dtPaymentH = dtPaymnentDetail.Clone();
			FillPaymentDetailFromListCommonForReadById(Obj, dtPaymentH);
			grdPaymentHistory.DataSource = dtPaymentH;
			grdPaymentHistory.RetrieveStructure();
			grdSettingsPaymentTerm(grdPaymentHistory);
			DataTable dtPmH = dtPmDetail.Clone();
			FillPmDetailFromListCommonForReadById(Obj, dtPmH);
			grdPMHistory.DataSource = dtPmH;
			grdPMHistory.RetrieveStructure();
			grdSettingsPM(grdPMHistory);
			DataTable dtOtherH = dtOtherItemDetail.Clone();
			FillOtherItemDetailFromListCommonForReadById(Obj, dtOtherH);
			grdOtherItemHistory.DataSource = dtOtherH;
			grdOtherItemHistory.RetrieveStructure();
			grdSettingsOtherItems(grdOtherItemHistory);
			DataTable dtOCH = dtOtherChargesDetail.Clone();
			FillOtherChargesDetailFromListCommonForReadById(Obj, dtOCH);
			grdOtherChargesHistory.DataSource = dtOCH;
			grdOtherChargesHistory.RetrieveStructure();
			grdSettingsOtherChargesDetail(grdOtherChargesHistory);
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
			grddetailhistory.RootTable.Columns["Qty/M.Ton"].AggregateFunction = (AggregateFunction)2;
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ItemId", "PmItemId", "BrandId", "OuterUomId", "OuterEquivalent", "OuterQtyEquivalent", "InnerUomId", "InnerEquivalent", "RateUOMId",
				"ExImFarmingNTradeId", "ExImFarmingTypeId", "ExImTradeTypeId", "CropYearId", "PackTypeId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grddetailhistory, 2, 3, hiddenColumns);
			grddetailhistory.RootTable.Columns["ItemName"].Width = 200;
			grddetailhistory.RootTable.Columns["CropYear"].Position = grddetailhistory.RootTable.Columns["CropYearId"].Position + 1;
			grddetailhistory.RootTable.Columns["PackingType"].Position = grddetailhistory.RootTable.Columns["PackTypeId"].Position + 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int id)
	{
		try
		{
			CommonServices.ExportSalesContractExport501WithSubReport(id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GenerateReport(RecId);
				return;
			}
			throw new Exception("No Record found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintII_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleContractReportsSlipNew501A(RecId);
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

	private void btnDeliveryTerm_Click(object sender, EventArgs e)
	{
		try
		{
			new DeliveryTerms(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPaymentTerms_Click(object sender, EventArgs e)
	{
		try
		{
			new PaymentTerms(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		new DefineMultiCurrency(UserAccount).Show();
	}

	private void btnSeaPortDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new SeaPortsDefine(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingMaterialRequirement_Click(object sender, EventArgs e)
	{
		try
		{
			new PackingMaterialRequirement(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCommodityDetailForm_Click(object sender, EventArgs e)
	{
		try
		{
			new CommodityDetailItemCustomerWise(UserAccount).Show();
			btnRefresh_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnContractPmDetailForm_Click(object sender, EventArgs e)
	{
		try
		{
			new frmExportSalesContractPmDetail(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AdvanceDoFormLink_Click(object sender, EventArgs e)
	{
		FormHelper.CanOpenForm("FrmExportSalesContractSchedule", delegate
		{
			frmSaleContractSchedule obj = new frmSaleContractSchedule(UserAccount);
			obj.Show();
			obj.BringToFront();
		}, UserAccount);
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

	private void BtnPackingTypeDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineExportPackingType(UserAccount).Show();
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
			new frmGenerateExportContractNos(UserAccount).Show();
			CustomerOrderNoBind(CustomerOrderNoDbCall());
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
			FromDateHistory.Focus();
			tabControl3.SelectedIndex = 0;
		}
		else
		{
			txtDocDate.Focus();
		}
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 2)
			{
				FilldtBrandForPMItem();
				BrandItemsBind();
				CmbBrandPmDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmExportContractIII_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					tabControl2.SelectedIndex = 0;
					txtDocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && RecId > 0)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					Print_Click(null, null);
				}
				if (e.Control && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					Print_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					btnDeliveryTerm_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F2)
				{
					btnPaymentTerms_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F3)
				{
					btnSeaPortDefine_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						CmbItemNameInDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						CmbPaymentTermPT.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						CmbItemPmDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 3)
					{
						CmbItemOtherDetail.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grdDetails).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						((Control)(object)grdPaymentTerm).Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						((Control)(object)grdPackingMaterialDetail).Focus();
					}
					else if (tabControl2.SelectedIndex == 3)
					{
						((Control)(object)grdotheritems).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						CmbPaymentTermPT.Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 2;
						CmbItemPmDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						tabControl2.SelectedIndex = 3;
						CmbItemOtherDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 3)
					{
						tabControl2.SelectedIndex = 0;
						CmbItemNameInDetail.Focus();
					}
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)grddetailhistory).Focus();
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

	private void grdDetails_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdDetails.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdDetails_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grdDetails.CurrentColumn != null && grdDetails.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdPaymentTerm_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (!(grdPaymentTerm.CurrentColumn.Key == "Delete"))
				{
					throw new Exception("Reset  the Detail first...");
				}
				if (updateDetailIndexPaymentDetail == -1)
				{
					dtPaymnentDetail.Rows[item.RowIndex].Delete();
					item.Delete();
				}
				grdPaymentTerm.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdPackingMaterialDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdPackingMaterialDetail_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdPackingMaterialDetail.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndexPmDetail != -1)
					{
						throw new Exception("Reset  the Detail first...");
					}
					dtPmDetail.Rows[item.RowIndex].Delete();
					item.Delete();
				}
				grdPackingMaterialDetail.UpdateData();
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
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights)
				{
					Reset();
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
					btnsave.Visible = false;
					btnUpdate.Visible = true;
				}
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					Reset();
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
				}
				if (DataGridHistory.CurrentColumn.Key == "SaveAs")
				{
					HistoryGridSaveAsButtonClick(Conversion.ToInt(r.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					if (formrights.DoHavePrintRights)
					{
						GenerateReport(Conversion.ToInt(r.Cells["Id"].Value));
					}
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print501A")
				{
					if (formrights.DoHavePrintRights)
					{
						CommonServices.SaleContractReportsSlipNew501A(Conversion.ToInt(r.Cells["Id"].Value));
					}
				}
				else if (DataGridHistory.CurrentColumn.Key == "PrintII")
				{
					if (formrights.DoHavePrintRights)
					{
						CommonServices.SaleContractReportsSlipNew501(Conversion.ToInt(r.Cells["Id"].Value));
					}
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print523")
				{
					if (formrights.DoHavePrintRights)
					{
						DataTable dt = new DataTable();
						dt = ExportPdfReport.ExpRptSalesContractExportRegister_523(new ReportsParameters
						{
							OrganizationId = UserAccount.OrganizationId,
							CompanyId = UserAccount.CompanyId,
							ExImLcOrderId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
						});
						if (dt == null || dt.Rows.Count == 0)
						{
							MessageBox.Show("Not Record Found For Display");
							return;
						}
						Reporting val = new Reporting();
						val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
						val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
						val.ShowReportWithDataTable(dt, "523-ExpRptSalesContractWiseInvoiceRegister.rpt");
					}
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), DocumentTypeId);
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
			dt.Rows.Add("Ctrl+F1", "For Open Delivery Term Popup");
			dt.Rows.Add("Ctrl+F2", "For Open Payterm Popup");
			dt.Rows.Add("Ctrl+F3", "For Open Sea Ports Popup");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Grids");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Column in Detail for Entry");
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

	private void txtNoOfBags_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InnerQtyCalculate()
	{
		try
		{
			double innerUom = 0.0;
			double mTon = 0.0;
			if (((UltraGridBase)CmbInnerUomInDetail).ActiveRow != null && ((UltraDropDownBase)CmbInnerUomInDetail).SelectedRow != null)
			{
				innerUom = Conversion.ToDouble(((UltraDropDownBase)CmbInnerUomInDetail).SelectedRow.Cells[2].Value);
			}
			mTon = Conversion.ToDouble((object)txtQtyMTonInDetail.Text);
			double noOfBags = ((mTon > 0.0 && innerUom > 0.0) ? (mTon * 1000.0 / innerUom) : 0.0);
			txtQtyInnerInDetail.Text = Math.Round(noOfBags).ToString("#,##0.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			double MTon = 0.0;
			double NoOfBags = 0.0;
			double OuterUom = 0.0;
			MTon = Conversion.ToDouble((object)txtQtyMTonInDetail.Text);
			NoOfBags = Conversion.ToDouble((object)txtOuterQtyInDetail.Text);
			if (((UltraGridBase)CmbOuterUomInDetail).ActiveRow == null)
			{
				return;
			}
			OuterUom = Conversion.ToDouble(((UltraDropDownBase)CmbOuterUomInDetail).SelectedRow.Cells[2].Value);
			Conversion.ToDouble(((UltraDropDownBase)CmbOuterUomInDetail).SelectedRow.Cells[3].Value);
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "MTon")
			{
				if (MTon > 0.0 && OuterUom > 0.0)
				{
					double noofbags = MTon * 1000.0 / OuterUom;
					txtOuterQtyInDetail.Text = Math.Round(noofbags).ToString("#,##0.##");
				}
				else
				{
					txtOuterQtyInDetail.Text = "0";
				}
			}
			else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "NoofBags")
			{
				if (NoOfBags > 0.0 && OuterUom > 0.0)
				{
					MTon = NoOfBags * OuterUom / 1000.0;
					txtQtyMTonInDetail.Text = MTon.ToString("#,#.######");
				}
				else
				{
					txtQtyMTonInDetail.Text = "0";
				}
			}
			else if (MTon > 0.0 && OuterUom > 0.0)
			{
				double noofbags2 = MTon * 1000.0 / OuterUom;
				txtOuterQtyInDetail.Text = Math.Round(noofbags2).ToString("#,##0.##");
			}
			InnerQtyCalculate();
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
			double NetWeight = 0.0;
			txtAmountInDetail.Text = 0.ToString();
			if (((UltraGridBase)CmbRateUomInDetail).ActiveRow != null && Conversion.ToInt(CmbRateUomInDetail.Value) > 0)
			{
				ItemRate = Conversion.ToDouble((object)txtRateInDetail.Text);
				ItemRateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomInDetail).SelectedRow.Cells[2].Value);
				NetWeight = Conversion.ToDouble((object)txtQtyMTonInDetail.Text);
				if (ItemRate > 0.0 && ItemRateUom > 0.0 && NetWeight > 0.0)
				{
					double weightkgs = NetWeight * 1000.0;
					double amount = ItemRate / ItemRateUom * weightkgs;
					txtAmountInDetail.Text = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
			}
			else
			{
				txtAmountInDetail.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void combitempck_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void txtCostMTon_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void InsterAmountandQtyMtonInHeader()
	{
		try
		{
			double OtherAmount = 0.0;
			object total = grdDetails.GetTotal(grdDetails.RootTable.Columns["Amount"], (AggregateFunction)2);
			if (grdotheritems.GetRows().Length != 0)
			{
				OtherAmount = Conversion.ToDouble(grdotheritems.GetTotal(grdotheritems.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			object Qty = grdDetails.GetTotal(grdDetails.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2);
			txtMTon.Text = Qty.ToString();
			double TotalAmount = Conversion.ToDouble(total) + Conversion.ToDouble((object)OtherAmount);
			txtFCYAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			if (TotalAmount > 0.0 && grdPaymentTerm.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double FcyAmount = Conversion.ToDouble(obj.Cells["%OfTotal"].Value) * TotalAmount / 100.0;
					obj.Cells["FcyAmount"].Value = FcyAmount;
					obj.EndEdit();
				}
			}
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
			double Qty = Conversion.ToDouble((object)txtQtyOtherDetail.Text.Trim());
			double Rate = Conversion.ToDouble((object)txtRateOtherDetail.Text.Trim());
			if (Qty > 0.0 && Rate > 0.0)
			{
				txtAmountOtherDetail.Text = (Qty * Rate).ToString();
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
		OtherItemsCalculations();
	}

	private void ProportionateOtherItemAmount()
	{
		try
		{
			if (grdDetails.RowCount <= 0)
			{
				return;
			}
			double ContractAmount = Conversion.ToDouble(grdDetails.GetTotal(grdDetails.RootTable.Columns["Amount"], (AggregateFunction)2));
			double OtherItemAmount = ((grdotheritems.RowCount > 0) ? Conversion.ToDouble(grdotheritems.GetTotal(grdotheritems.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
			if (OtherItemAmount > 0.0)
			{
				GridEXRow[] rows = grdDetails.GetRows();
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
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["OtherItemAmount"].Value = 0;
				}
			}
			grdDetails.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PercntOfTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double prcnt = Conversion.ToDouble((object)txtPercntOfTotalPT.Text);
			double fcyAmount = Conversion.ToDouble((object)txtFCYAmount.Text);
			if (prcnt > 100.0)
			{
				txtPercntOfTotalPT.Text = "0";
				txtlFcyAmountPT.Text = "0";
				MessageBox.Show("%age Can't be Greater Than 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (fcyAmount > 0.0)
			{
				double amount = prcnt * fcyAmount / 100.0;
				txtlFcyAmountPT.Text = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtlFcyAmountPT.Text = "0";
				MessageBox.Show("Fcy Amount is Zero", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtIPackingtemQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtInnerQtyPmDetail.Text);
			CalculatePackingAmount(Qty);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingItemRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double Qty = 0.0;
			Qty = ((!(Conversion.ToDouble((object)txtOuterQtyPmDetail.Text) > 0.0)) ? Conversion.ToDouble((object)txtInnerQtyPmDetail.Text) : Conversion.ToDouble((object)txtOuterQtyPmDetail.Text));
			CalculatePackingAmount(Qty);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculatePackingAmount(double Qty)
	{
		try
		{
			if (Qty > 0.0 && Conversion.ToDouble((object)txtRatePmDetail.Text) > 0.0)
			{
				double amount = Qty * Conversion.ToDouble((object)txtRatePmDetail.Text);
				txtAmountPmDetail.Text = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtAmountPmDetail.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOuterQtyPm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtOuterQtyPmDetail.Text);
			CalculatePackingAmount(Qty);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInnerUomInDetail_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			InnerQtyCalculate();
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, DataGridHistory, dtcol);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdOtherChargesDetail, dtOtherChargesDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtShipmentDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtShipmentDate.Value.Date < datShipmentStartDate.Value.Date)
			{
				txtShipmentDate.Value = datShipmentStartDate.Value;
				throw new Exception("Shipment End Date Can't Be Less Than Start Date");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void datOrderDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			ApprovalDate.Value = txtCustomerOrderDate.Value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datShipmentStartDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (datShipmentStartDate.Value.Date < txtDocDate.Value.Date)
			{
				datShipmentStartDate.Value = txtDocDate.Value;
				throw new Exception("Shipment Start Date Can't Be Less Than Doc Date");
			}
			CalculateShipmentDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateShipmentDate()
	{
		try
		{
			if (Conversion.ToInt((object)txtDeliveryDays.Text) == 0)
			{
				txtShipmentDate.Value = datShipmentStartDate.Value;
			}
			else
			{
				txtShipmentDate.Value = datShipmentStartDate.Value.AddDays(Conversion.ToDouble((object)txtDeliveryDays.Text.Trim()));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtDeliveryDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateShipmentDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtDocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (datShipmentStartDate.Value.Date < txtDocDate.Value.Date)
			{
				datShipmentStartDate.Value = txtDocDate.Value;
				throw new Exception("Shipment Start Date Can't Be Less Than Doc Date");
			}
			CalculateShipmentDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtFactoryLoadingDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = CommonServices.GetAllLcOrderSchedulingPolicy();
			if (dt != null && dt.Rows.Count > 0)
			{
				DateTime LoadingDate = Conversion.ToDateTime((object)txtFactoryLoadingDate.Value);
				int ProductionInterval = Conversion.ToInt(dt.Rows[0]["ProductionInterval"]);
				int InspectionInterval = Conversion.ToInt(dt.Rows[0]["InspectionInterval"]);
				int PackMaterialInterval = Conversion.ToInt(dt.Rows[0]["PackMaterialInterval"]);
				txtProductionScheduleDate.Value = LoadingDate.AddDays(-ProductionInterval);
				txtInspectionScheduleDate.Value = LoadingDate.AddDays(-InspectionInterval);
				txtPMScheduleDate.Value = LoadingDate.AddDays(-PackMaterialInterval);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PercntOfTotal_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyMTon_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtNoOfBags_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtCostMTon_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtPackingItemRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDeliveryDays_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtNoOfContainers_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtOuterQtyPm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtInnerQtyPm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrandInnerQtyPm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrandOuterQtyPm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddAmountChargeDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtLessAmountChargeDetail_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Expected O, but got Unknown
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Expected O, but got Unknown
		//IL_1032: Unknown result type (might be due to invalid IL or missing references)
		//IL_103c: Expected O, but got Unknown
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1094: Expected O, but got Unknown
		//IL_113a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1144: Expected O, but got Unknown
		//IL_1171: Unknown result type (might be due to invalid IL or missing references)
		//IL_117b: Expected O, but got Unknown
		//IL_117c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1186: Expected O, but got Unknown
		//IL_1192: Unknown result type (might be due to invalid IL or missing references)
		//IL_119c: Expected O, but got Unknown
		//IL_1242: Unknown result type (might be due to invalid IL or missing references)
		//IL_124c: Expected O, but got Unknown
		//IL_124d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1257: Expected O, but got Unknown
		//IL_12a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12af: Expected O, but got Unknown
		//IL_12bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c5: Expected O, but got Unknown
		//IL_131e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1328: Expected O, but got Unknown
		//IL_13b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c2: Expected O, but got Unknown
		//IL_1426: Unknown result type (might be due to invalid IL or missing references)
		//IL_1430: Expected O, but got Unknown
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_1451: Expected O, but got Unknown
		//IL_14f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1501: Expected O, but got Unknown
		//IL_1502: Unknown result type (might be due to invalid IL or missing references)
		//IL_150c: Expected O, but got Unknown
		//IL_1518: Unknown result type (might be due to invalid IL or missing references)
		//IL_1522: Expected O, but got Unknown
		//IL_1539: Unknown result type (might be due to invalid IL or missing references)
		//IL_1543: Expected O, but got Unknown
		//IL_15c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d2: Expected O, but got Unknown
		//IL_1683: Unknown result type (might be due to invalid IL or missing references)
		//IL_168d: Expected O, but got Unknown
		//IL_16a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ae: Expected O, but got Unknown
		//IL_1712: Unknown result type (might be due to invalid IL or missing references)
		//IL_171c: Expected O, but got Unknown
		//IL_1780: Unknown result type (might be due to invalid IL or missing references)
		//IL_178a: Expected O, but got Unknown
		//IL_178b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Expected O, but got Unknown
		//IL_1796: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a0: Expected O, but got Unknown
		//IL_17a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ab: Expected O, but got Unknown
		//IL_17ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f8: Expected O, but got Unknown
		//IL_17f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Expected O, but got Unknown
		//IL_180f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1819: Expected O, but got Unknown
		//IL_1830: Unknown result type (might be due to invalid IL or missing references)
		//IL_183a: Expected O, but got Unknown
		//IL_1a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a55: Expected O, but got Unknown
		//IL_1a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a60: Expected O, but got Unknown
		//IL_1a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a81: Expected O, but got Unknown
		//IL_1a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa2: Expected O, but got Unknown
		//IL_1aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab8: Expected O, but got Unknown
		//IL_1ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac3: Expected O, but got Unknown
		//IL_1ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae4: Expected O, but got Unknown
		//IL_1b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b47: Expected O, but got Unknown
		//IL_1c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c65: Expected O, but got Unknown
		//IL_1c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c86: Expected O, but got Unknown
		//IL_1ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb2: Expected O, but got Unknown
		//IL_1cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cde: Expected O, but got Unknown
		//IL_1d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0a: Expected O, but got Unknown
		//IL_1d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d36: Expected O, but got Unknown
		//IL_1df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfc: Expected O, but got Unknown
		//IL_28be: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c8: Expected O, but got Unknown
		//IL_82a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_82b0: Expected O, but got Unknown
		//IL_95b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_95bc: Expected O, but got Unknown
		//IL_c097: Unknown result type (might be due to invalid IL or missing references)
		//IL_c0a1: Expected O, but got Unknown
		//IL_d200: Unknown result type (might be due to invalid IL or missing references)
		//IL_d20a: Expected O, but got Unknown
		//IL_16b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b29: Expected O, but got Unknown
		//IL_16b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b57: Expected O, but got Unknown
		GridEXLayout grdDetails_DesignTimeLayout = new GridEXLayout();
		Appearance appearance301 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
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
		Appearance appearance334 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance412 = new Appearance();
		Appearance appearance423 = new Appearance();
		Appearance appearance434 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance345 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance387 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
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
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance404 = new Appearance();
		Appearance appearance405 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance413 = new Appearance();
		Appearance appearance414 = new Appearance();
		Appearance appearance415 = new Appearance();
		GridEXLayout grdPaymentTerm_DesignTimeLayout = new GridEXLayout();
		Appearance appearance429 = new Appearance();
		Appearance appearance430 = new Appearance();
		Appearance appearance431 = new Appearance();
		Appearance appearance432 = new Appearance();
		Appearance appearance433 = new Appearance();
		Appearance appearance435 = new Appearance();
		Appearance appearance436 = new Appearance();
		Appearance appearance437 = new Appearance();
		Appearance appearance438 = new Appearance();
		Appearance appearance439 = new Appearance();
		Appearance appearance440 = new Appearance();
		Appearance appearance441 = new Appearance();
		GridEXLayout grdPackingMaterialDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance442 = new Appearance();
		Appearance appearance443 = new Appearance();
		Appearance appearance444 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance416 = new Appearance();
		Appearance appearance417 = new Appearance();
		Appearance appearance418 = new Appearance();
		Appearance appearance419 = new Appearance();
		Appearance appearance420 = new Appearance();
		Appearance appearance421 = new Appearance();
		Appearance appearance422 = new Appearance();
		Appearance appearance424 = new Appearance();
		Appearance appearance425 = new Appearance();
		Appearance appearance426 = new Appearance();
		Appearance appearance427 = new Appearance();
		Appearance appearance428 = new Appearance();
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
		Appearance appearance78 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
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
		Appearance appearance391 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance394 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance396 = new Appearance();
		Appearance appearance397 = new Appearance();
		Appearance appearance398 = new Appearance();
		Appearance appearance399 = new Appearance();
		Appearance appearance401 = new Appearance();
		Appearance appearance402 = new Appearance();
		Appearance appearance403 = new Appearance();
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
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
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
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance225 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.frmExportContractIII));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grddetailhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPMHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherItemHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherChargesHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance384 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance390 = new Appearance();
		Appearance appearance400 = new Appearance();
		Appearance appearance406 = new Appearance();
		Appearance appearance407 = new Appearance();
		Appearance appearance408 = new Appearance();
		Appearance appearance409 = new Appearance();
		Appearance appearance410 = new Appearance();
		Appearance appearance411 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabContractDetail = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetails = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.CmbInnerUomInDetail = new UltraCombo();
		this.label119 = new System.Windows.Forms.Label();
		this.CmbBrandInDetail = new UltraCombo();
		this.CmbPmItemInDetail = new UltraCombo();
		this.panel36 = new System.Windows.Forms.Panel();
		this.RadBrandNameInDetail = new System.Windows.Forms.RadioButton();
		this.RadBrandCodeInDetail = new System.Windows.Forms.RadioButton();
		this.panel35 = new System.Windows.Forms.Panel();
		this.RadPmItemNameInDetail = new System.Windows.Forms.RadioButton();
		this.RadPmItemCodeInDetail = new System.Windows.Forms.RadioButton();
		this.label113 = new System.Windows.Forms.Label();
		this.CmbCategoryInDetail = new UltraCombo();
		this.label112 = new System.Windows.Forms.Label();
		this.panel34 = new System.Windows.Forms.Panel();
		this.RadCategory = new System.Windows.Forms.RadioButton();
		this.RadType = new System.Windows.Forms.RadioButton();
		this.label85 = new System.Windows.Forms.Label();
		this.label107 = new System.Windows.Forms.Label();
		this.label106 = new System.Windows.Forms.Label();
		this.label105 = new System.Windows.Forms.Label();
		this.label104 = new System.Windows.Forms.Label();
		this.label103 = new System.Windows.Forms.Label();
		this.label102 = new System.Windows.Forms.Label();
		this.label101 = new System.Windows.Forms.Label();
		this.label100 = new System.Windows.Forms.Label();
		this.BtnAddInDetail = new System.Windows.Forms.Button();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.CmbFarmingNTradeInDetail = new UltraCombo();
		this.label69 = new System.Windows.Forms.Label();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.panel23 = new System.Windows.Forms.Panel();
		this.label45 = new System.Windows.Forms.Label();
		this.CmbPmDetailByItemCusInDetail = new UltraCombo();
		this.CmbComditiyDetailByItemCusInDetail = new UltraCombo();
		this.txtPmDetailInDetail = new System.Windows.Forms.TextBox();
		this.CmbItemNameInDetail = new UltraCombo();
		this.txtCommodityDetailInDetail = new System.Windows.Forms.TextBox();
		this.txtRemarksInDetail = new System.Windows.Forms.TextBox();
		this.label115 = new System.Windows.Forms.Label();
		this.btnCancelInDetail = new System.Windows.Forms.Button();
		this.label48 = new System.Windows.Forms.Label();
		this.txtQtyMTonInDetail = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label114 = new System.Windows.Forms.Label();
		this.txtRateInDetail = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnUpdateInDetail = new System.Windows.Forms.Button();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbPackingTypeInDetail = new UltraCombo();
		this.CmbRateUomInDetail = new UltraCombo();
		this.txtPalletQtyInDetail = new System.Windows.Forms.TextBox();
		this.txtQtyInnerInDetail = new System.Windows.Forms.TextBox();
		this.txtOuterQtyInDetail = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label120 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label116 = new System.Windows.Forms.Label();
		this.CmbOuterUomInDetail = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbCropYearInDetail = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtAmountInDetail = new System.Windows.Forms.TextBox();
		this.tabPaymentTerm = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel22 = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.grdPaymentTerm = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label110 = new System.Windows.Forms.Label();
		this.label109 = new System.Windows.Forms.Label();
		this.label108 = new System.Windows.Forms.Label();
		this.btnCancelPayment = new System.Windows.Forms.Button();
		this.btnUpdatePayment = new System.Windows.Forms.Button();
		this.btnAddPayment = new System.Windows.Forms.Button();
		this.label55 = new System.Windows.Forms.Label();
		this.txtRemarksPT = new System.Windows.Forms.TextBox();
		this.txtlFcyAmountPT = new System.Windows.Forms.TextBox();
		this.txtPercntOfTotalPT = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbPaymentTermPT = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label57 = new System.Windows.Forms.Label();
		this.tabPackingMaterialDetail = new System.Windows.Forms.TabPage();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.panel25 = new System.Windows.Forms.Panel();
		this.panel27 = new System.Windows.Forms.Panel();
		this.grdPackingMaterialDetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.btnPackingAdd = new System.Windows.Forms.Button();
		this.CmbBrandPmDetail = new UltraCombo();
		this.panel30 = new System.Windows.Forms.Panel();
		this.radBrandName = new System.Windows.Forms.RadioButton();
		this.label47 = new System.Windows.Forms.Label();
		this.rabBrandName = new System.Windows.Forms.RadioButton();
		this.rdPackingItemCode = new System.Windows.Forms.RadioButton();
		this.rdPackingItemName = new System.Windows.Forms.RadioButton();
		this.btnPackingCancel = new System.Windows.Forms.Button();
		this.btnPackingUpdate = new System.Windows.Forms.Button();
		this.txtRemarksPmDetail = new System.Windows.Forms.TextBox();
		this.txtAmountPmDetail = new System.Windows.Forms.TextBox();
		this.txtRatePmDetail = new System.Windows.Forms.TextBox();
		this.txtBrandInnerQtyPmDetail = new System.Windows.Forms.TextBox();
		this.txtBrandOuterQtyPmDetail = new System.Windows.Forms.TextBox();
		this.txtOuterQtyPmDetail = new System.Windows.Forms.TextBox();
		this.txtInnerQtyPmDetail = new System.Windows.Forms.TextBox();
		this.CmbUomPmDetail = new UltraCombo();
		this.CmbBrandUomPmDetail = new UltraCombo();
		this.label80 = new System.Windows.Forms.Label();
		this.CmbBrandPackingTypePmDetail = new UltraCombo();
		this.label77 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.CmbItemPmDetail = new UltraCombo();
		this.label78 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label56 = new System.Windows.Forms.Label();
		this.tabOtherItems = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdotheritems = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.RadItemCodeOtherDetail = new System.Windows.Forms.RadioButton();
		this.RadItemNameOtherDetail = new System.Windows.Forms.RadioButton();
		this.panel26 = new System.Windows.Forms.Panel();
		this.label46 = new System.Windows.Forms.Label();
		this.btnCancelOtherDetail = new System.Windows.Forms.Button();
		this.btnUpdateOtherDetail = new System.Windows.Forms.Button();
		this.btnaddOtherDetail = new System.Windows.Forms.Button();
		this.label38 = new System.Windows.Forms.Label();
		this.txtRemarksOtherDetail = new System.Windows.Forms.TextBox();
		this.txtQtyOtherDetail = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtRateOtherDetail = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtAmountOtherDetail = new System.Windows.Forms.TextBox();
		this.CmbItemOtherDetail = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.tabOtherCharges = new System.Windows.Forms.TabPage();
		this.grdOtherChargesDetail = new GridEX();
		this.panel37 = new System.Windows.Forms.Panel();
		this.ctrlgrdOtherChargesDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.panel38 = new System.Windows.Forms.Panel();
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
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbNotifyParty2 = new UltraCombo();
		this.CmbNotifyParty1 = new UltraCombo();
		this.CmbConsignee = new UltraCombo();
		this.CmbContractNo = new UltraCombo();
		this.btnDefineBankLocal = new System.Windows.Forms.Button();
		this.btnDefineBankForeign = new System.Windows.Forms.Button();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.txtFCYAmount = new System.Windows.Forms.TextBox();
		this.txtMTon = new System.Windows.Forms.TextBox();
		this.txtNoOfContainers = new System.Windows.Forms.TextBox();
		this.CmbFCYCode = new UltraCombo();
		this.cmbStatus = new UltraCombo();
		this.txtDeliveryDays = new System.Windows.Forms.TextBox();
		this.CmbDeliveryTerm = new UltraCombo();
		this.txtSpecialInstructions = new System.Windows.Forms.TextBox();
		this.txtCommodityDetail = new System.Windows.Forms.TextBox();
		this.CmbExporterbank = new UltraCombo();
		this.label99 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.label97 = new System.Windows.Forms.Label();
		this.label96 = new System.Windows.Forms.Label();
		this.label94 = new System.Windows.Forms.Label();
		this.label93 = new System.Windows.Forms.Label();
		this.label92 = new System.Windows.Forms.Label();
		this.label88 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.label83 = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label95 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.txtPMScheduleDate = new System.Windows.Forms.DateTimePicker();
		this.labelDCG = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.txtInspectionScheduleDate = new System.Windows.Forms.DateTimePicker();
		this.label71 = new System.Windows.Forms.Label();
		this.txtProductionScheduleDate = new System.Windows.Forms.DateTimePicker();
		this.label70 = new System.Windows.Forms.Label();
		this.txtFactoryLoadingDate = new System.Windows.Forms.DateTimePicker();
		this.label68 = new System.Windows.Forms.Label();
		this.datShipmentStartDate = new System.Windows.Forms.DateTimePicker();
		this.label41 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtCustomerOrderNo = new System.Windows.Forms.TextBox();
		this.txtProductSpecification = new System.Windows.Forms.TextBox();
		this.txtLotRefNo = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.ApprovalDate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.txtCustomerOrderDate = new System.Windows.Forms.DateTimePicker();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.CmbNotifyParty3 = new UltraCombo();
		this.CmbCustomer = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbSalePerson = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txtShipmentDate = new System.Windows.Forms.DateTimePicker();
		this.CmbImporterBank = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbDocumentCustomGroup = new UltraCombo();
		this.CmbLoadingPort = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.CmbDestinationPort = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.panel28 = new System.Windows.Forms.Panel();
		this.AdvanceDoFormLink = new System.Windows.Forms.LinkLabel();
		this.chkOpenContractSchedule = new System.Windows.Forms.CheckBox();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.label66 = new System.Windows.Forms.Label();
		this.CmbExportCompany = new UltraCombo();
		this.labelExportCompany = new System.Windows.Forms.Label();
		this.txtApprovalRemarks = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtDeliveryRemarks = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintII = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateInvoiceNos = new System.Windows.Forms.ToolStripButton();
		this.BtnPackingTypeDefine = new System.Windows.Forms.ToolStripButton();
		this.BtnDeliveryTermLookUp = new System.Windows.Forms.ToolStripButton();
		this.btnDefinePaymentTerm = new System.Windows.Forms.ToolStripButton();
		this.btnSeaPortDefine = new System.Windows.Forms.ToolStripButton();
		this.BtnContractPmDetailForm = new System.Windows.Forms.ToolStripButton();
		this.BtnCommodityDetailForm = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabDetailH = new System.Windows.Forms.TabPage();
		this.grddetailhistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.tabPaymentDetailH = new System.Windows.Forms.TabPage();
		this.grdPaymentHistory = new GridEX();
		this.panel31 = new System.Windows.Forms.Panel();
		this.label74 = new System.Windows.Forms.Label();
		this.tabPmH = new System.Windows.Forms.TabPage();
		this.grdPMHistory = new GridEX();
		this.panel32 = new System.Windows.Forms.Panel();
		this.label75 = new System.Windows.Forms.Label();
		this.tabOtherItemH = new System.Windows.Forms.TabPage();
		this.grdOtherItemHistory = new GridEX();
		this.panel33 = new System.Windows.Forms.Panel();
		this.label76 = new System.Windows.Forms.Label();
		this.tabOtherchargesHistory = new System.Windows.Forms.TabPage();
		this.grdOtherChargesHistory = new GridEX();
		this.panel39 = new System.Windows.Forms.Panel();
		this.label87 = new System.Windows.Forms.Label();
		this.panel29 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel18 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label67 = new System.Windows.Forms.Label();
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
		this.panel17 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabContractDetail.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetails).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInnerUomInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPmItemInDetail).BeginInit();
		this.panel36.SuspendLayout();
		this.panel35.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCategoryInDetail).BeginInit();
		this.panel34.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingNTradeInDetail).BeginInit();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPmDetailByItemCusInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbComditiyDetailByItemCusInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOuterUomInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearInDetail).BeginInit();
		this.tabPaymentTerm.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPT).BeginInit();
		this.panel11.SuspendLayout();
		this.tabPackingMaterialDetail.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel21.SuspendLayout();
		this.panel25.SuspendLayout();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialDetail).BeginInit();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandPmDetail).BeginInit();
		this.panel30.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUomPmDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPmDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandPackingTypePmDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemPmDetail).BeginInit();
		this.panel16.SuspendLayout();
		this.tabOtherItems.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel8.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOtherDetail).BeginInit();
		this.tabOtherCharges.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetail).BeginInit();
		this.panel37.SuspendLayout();
		this.panel38.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesNameChargeDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbConsignee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFCYCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterbank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).BeginInit();
		this.panel28.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportCompany).BeginInit();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailhistory).BeginInit();
		this.panel13.SuspendLayout();
		this.tabPaymentDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentHistory).BeginInit();
		this.panel31.SuspendLayout();
		this.tabPmH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPMHistory).BeginInit();
		this.panel32.SuspendLayout();
		this.tabOtherItemH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherItemHistory).BeginInit();
		this.panel33.SuspendLayout();
		this.tabOtherchargesHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesHistory).BeginInit();
		this.panel39.SuspendLayout();
		this.panel29.SuspendLayout();
		this.panel18.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1234, 761);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1234, 761);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1226, 732);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1220, 726);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 397);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1220, 329);
		this.panel5.TabIndex = 2;
		this.tabControl2.Controls.Add(this.tabContractDetail);
		this.tabControl2.Controls.Add(this.tabPaymentTerm);
		this.tabControl2.Controls.Add(this.tabPackingMaterialDetail);
		this.tabControl2.Controls.Add(this.tabOtherItems);
		this.tabControl2.Controls.Add(this.tabOtherCharges);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1220, 329);
		this.tabControl2.TabIndex = 0;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabContractDetail.Controls.Add(this.panel7);
		this.tabContractDetail.Controls.Add(this.panel6);
		this.tabContractDetail.Location = new System.Drawing.Point(4, 25);
		this.tabContractDetail.Name = "tabContractDetail";
		this.tabContractDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabContractDetail.Size = new System.Drawing.Size(1212, 300);
		this.tabContractDetail.TabIndex = 1;
		this.tabContractDetail.Text = "Contract Detail";
		this.tabContractDetail.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetails);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 182);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1206, 115);
		this.panel7.TabIndex = 491;
		this.grdDetails.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetails).BackColor = System.Drawing.Color.White;
		grdDetails_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetails.DesignTimeLayout = grdDetails_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetails).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetails.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetails).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetails.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetails).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetails).Name = "grdDetails";
		((ScrollableBase)this.grdDetails).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetails).Size = new System.Drawing.Size(1206, 115);
		((System.Windows.Forms.Control)(object)this.grdDetails).TabIndex = 0;
		this.grdDetails.TotalRow = (InheritableBoolean)1;
		this.grdDetails.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetails).VisualStyle = (VisualStyle)4;
		this.grdDetails.ColumnButtonClick += new ColumnActionEventHandler(grdDetails_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetails).DoubleClick += new System.EventHandler(grdDetails_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetails).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetails_KeyDown);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail);
		this.panel6.Controls.Add(this.label119);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandInDetail);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPmItemInDetail);
		this.panel6.Controls.Add(this.panel36);
		this.panel6.Controls.Add(this.panel35);
		this.panel6.Controls.Add(this.label113);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategoryInDetail);
		this.panel6.Controls.Add(this.label112);
		this.panel6.Controls.Add(this.panel34);
		this.panel6.Controls.Add(this.label85);
		this.panel6.Controls.Add(this.label107);
		this.panel6.Controls.Add(this.label106);
		this.panel6.Controls.Add(this.label105);
		this.panel6.Controls.Add(this.label104);
		this.panel6.Controls.Add(this.label103);
		this.panel6.Controls.Add(this.label102);
		this.panel6.Controls.Add(this.label101);
		this.panel6.Controls.Add(this.label100);
		this.panel6.Controls.Add(this.BtnAddInDetail);
		this.panel6.Controls.Add(this.rdSearchByCode);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFarmingNTradeInDetail);
		this.panel6.Controls.Add(this.label69);
		this.panel6.Controls.Add(this.rdSearchByName);
		this.panel6.Controls.Add(this.panel23);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusInDetail);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbComditiyDetailByItemCusInDetail);
		this.panel6.Controls.Add(this.txtPmDetailInDetail);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameInDetail);
		this.panel6.Controls.Add(this.txtCommodityDetailInDetail);
		this.panel6.Controls.Add(this.txtRemarksInDetail);
		this.panel6.Controls.Add(this.label115);
		this.panel6.Controls.Add(this.btnCancelInDetail);
		this.panel6.Controls.Add(this.label48);
		this.panel6.Controls.Add(this.txtQtyMTonInDetail);
		this.panel6.Controls.Add(this.label34);
		this.panel6.Controls.Add(this.label16);
		this.panel6.Controls.Add(this.label114);
		this.panel6.Controls.Add(this.txtRateInDetail);
		this.panel6.Controls.Add(this.label39);
		this.panel6.Controls.Add(this.label10);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Controls.Add(this.btnUpdateInDetail);
		this.panel6.Controls.Add(this.label25);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingTypeInDetail);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomInDetail);
		this.panel6.Controls.Add(this.txtPalletQtyInDetail);
		this.panel6.Controls.Add(this.txtQtyInnerInDetail);
		this.panel6.Controls.Add(this.txtOuterQtyInDetail);
		this.panel6.Controls.Add(this.label24);
		this.panel6.Controls.Add(this.label120);
		this.panel6.Controls.Add(this.label13);
		this.panel6.Controls.Add(this.label116);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearInDetail);
		this.panel6.Controls.Add(this.label33);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Controls.Add(this.txtAmountInDetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1206, 179);
		this.panel6.TabIndex = 0;
		this.CmbInnerUomInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbInnerUomInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInnerUomInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInnerUomInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance312).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance312).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance312).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance323).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance323;
		((SpecialBoxBase)((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance335).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance335).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance335).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance335;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance346).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance346;
		((AppearanceBase)appearance357).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance357).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance357;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance368;
		((AppearanceBase)appearance372).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance372).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance372;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance373).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance373).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance373).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance373).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance373;
		((AppearanceBase)appearance374).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance374;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance375).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance375).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance375;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance376).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance376;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInnerUomInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).Location = new System.Drawing.Point(5, 101);
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).Name = "CmbInnerUomInDetail";
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).Size = new System.Drawing.Size(88, 26);
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).TabIndex = 9;
		this.CmbInnerUomInDetail.ValueChanged += new System.EventHandler(CmbInnerUomInDetail_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).Leave += new System.EventHandler(CmbInnerUomInDetail_ValueChanged);
		this.label119.AutoSize = true;
		this.label119.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label119.Location = new System.Drawing.Point(5, 83);
		this.label119.Name = "label119";
		this.label119.Size = new System.Drawing.Size(66, 15);
		this.label119.TabIndex = 588;
		this.label119.Text = "Inner UOM";
		this.CmbBrandInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance210;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance216).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandInDetail).Location = new System.Drawing.Point(711, 54);
		((System.Windows.Forms.Control)(object)this.CmbBrandInDetail).Name = "CmbBrandInDetail";
		((System.Windows.Forms.Control)(object)this.CmbBrandInDetail).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandInDetail).TabIndex = 4;
		this.CmbPmItemInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPmItemInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPmItemInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPmItemInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance296).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance296).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance296;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance297;
		((SpecialBoxBase)((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance298).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance298).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance299;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance303).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance304).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance304).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance304).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance304;
		((AppearanceBase)appearance305).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPmItemInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPmItemInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPmItemInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPmItemInDetail).Location = new System.Drawing.Point(530, 54);
		((System.Windows.Forms.Control)(object)this.CmbPmItemInDetail).Name = "CmbPmItemInDetail";
		((System.Windows.Forms.Control)(object)this.CmbPmItemInDetail).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbPmItemInDetail).TabIndex = 3;
		this.panel36.Controls.Add(this.RadBrandNameInDetail);
		this.panel36.Controls.Add(this.RadBrandCodeInDetail);
		this.panel36.Location = new System.Drawing.Point(745, 28);
		this.panel36.Name = "panel36";
		this.panel36.Size = new System.Drawing.Size(121, 26);
		this.panel36.TabIndex = 586;
		this.RadBrandNameInDetail.AutoSize = true;
		this.RadBrandNameInDetail.Checked = true;
		this.RadBrandNameInDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBrandNameInDetail.Location = new System.Drawing.Point(4, 4);
		this.RadBrandNameInDetail.Name = "RadBrandNameInDetail";
		this.RadBrandNameInDetail.Size = new System.Drawing.Size(57, 19);
		this.RadBrandNameInDetail.TabIndex = 35;
		this.RadBrandNameInDetail.TabStop = true;
		this.RadBrandNameInDetail.Text = "Name";
		this.RadBrandNameInDetail.UseVisualStyleBackColor = true;
		this.RadBrandNameInDetail.CheckedChanged += new System.EventHandler(RadBrandNameInDetail_CheckedChanged);
		this.RadBrandCodeInDetail.AutoSize = true;
		this.RadBrandCodeInDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBrandCodeInDetail.Location = new System.Drawing.Point(63, 4);
		this.RadBrandCodeInDetail.Name = "RadBrandCodeInDetail";
		this.RadBrandCodeInDetail.Size = new System.Drawing.Size(52, 19);
		this.RadBrandCodeInDetail.TabIndex = 36;
		this.RadBrandCodeInDetail.Text = "Code";
		this.RadBrandCodeInDetail.UseVisualStyleBackColor = true;
		this.panel35.Controls.Add(this.RadPmItemNameInDetail);
		this.panel35.Controls.Add(this.RadPmItemCodeInDetail);
		this.panel35.Location = new System.Drawing.Point(586, 28);
		this.panel35.Name = "panel35";
		this.panel35.Size = new System.Drawing.Size(121, 26);
		this.panel35.TabIndex = 586;
		this.RadPmItemNameInDetail.AutoSize = true;
		this.RadPmItemNameInDetail.Checked = true;
		this.RadPmItemNameInDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadPmItemNameInDetail.Location = new System.Drawing.Point(4, 4);
		this.RadPmItemNameInDetail.Name = "RadPmItemNameInDetail";
		this.RadPmItemNameInDetail.Size = new System.Drawing.Size(57, 19);
		this.RadPmItemNameInDetail.TabIndex = 35;
		this.RadPmItemNameInDetail.TabStop = true;
		this.RadPmItemNameInDetail.Text = "Name";
		this.RadPmItemNameInDetail.UseVisualStyleBackColor = true;
		this.RadPmItemNameInDetail.CheckedChanged += new System.EventHandler(RadPmItemNameInDetail_CheckedChanged);
		this.RadPmItemCodeInDetail.AutoSize = true;
		this.RadPmItemCodeInDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadPmItemCodeInDetail.Location = new System.Drawing.Point(63, 4);
		this.RadPmItemCodeInDetail.Name = "RadPmItemCodeInDetail";
		this.RadPmItemCodeInDetail.Size = new System.Drawing.Size(52, 19);
		this.RadPmItemCodeInDetail.TabIndex = 36;
		this.RadPmItemCodeInDetail.Text = "Code";
		this.RadPmItemCodeInDetail.UseVisualStyleBackColor = true;
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label113.Location = new System.Drawing.Point(711, 34);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(38, 15);
		this.label113.TabIndex = 584;
		this.label113.Text = "Brand";
		this.CmbCategoryInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCategoryInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategoryInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategoryInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance309).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance309).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance309;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance310;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance311).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance311).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance311;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance313;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance314).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance314;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance315;
		((AppearanceBase)appearance316).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance316).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance317).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance317).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance317).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance317;
		((AppearanceBase)appearance318).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance319).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategoryInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategoryInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategoryInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategoryInDetail).Location = new System.Drawing.Point(5, 54);
		((System.Windows.Forms.Control)(object)this.CmbCategoryInDetail).Name = "CmbCategoryInDetail";
		((System.Windows.Forms.Control)(object)this.CmbCategoryInDetail).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategoryInDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCategoryInDetail).Leave += new System.EventHandler(CmbCategoryInDetail_Leave);
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label112.Location = new System.Drawing.Point(530, 34);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(53, 15);
		this.label112.TabIndex = 584;
		this.label112.Text = "PM Item";
		this.panel34.Controls.Add(this.RadCategory);
		this.panel34.Controls.Add(this.RadType);
		this.panel34.Location = new System.Drawing.Point(36, 28);
		this.panel34.Name = "panel34";
		this.panel34.Size = new System.Drawing.Size(136, 26);
		this.panel34.TabIndex = 586;
		this.RadCategory.AutoSize = true;
		this.RadCategory.Checked = true;
		this.RadCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCategory.Location = new System.Drawing.Point(4, 4);
		this.RadCategory.Name = "RadCategory";
		this.RadCategory.Size = new System.Drawing.Size(72, 19);
		this.RadCategory.TabIndex = 35;
		this.RadCategory.TabStop = true;
		this.RadCategory.Text = "Category";
		this.RadCategory.UseVisualStyleBackColor = true;
		this.RadCategory.CheckedChanged += new System.EventHandler(RadCategory_CheckedChanged);
		this.RadType.AutoSize = true;
		this.RadType.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadType.Location = new System.Drawing.Point(80, 4);
		this.RadType.Name = "RadType";
		this.RadType.Size = new System.Drawing.Size(50, 19);
		this.RadType.TabIndex = 36;
		this.RadType.Text = "Type";
		this.RadType.UseVisualStyleBackColor = true;
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.Location = new System.Drawing.Point(5, 34);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(32, 15);
		this.label85.TabIndex = 584;
		this.label85.Text = "Item";
		this.label107.AutoSize = true;
		this.label107.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label107.ForeColor = System.Drawing.Color.Red;
		this.label107.Location = new System.Drawing.Point(954, 33);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(14, 17);
		this.label107.TabIndex = 583;
		this.label107.Text = "*";
		this.label106.AutoSize = true;
		this.label106.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label106.ForeColor = System.Drawing.Color.Red;
		this.label106.Location = new System.Drawing.Point(429, 82);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(14, 17);
		this.label106.TabIndex = 582;
		this.label106.Text = "*";
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label105.ForeColor = System.Drawing.Color.Red;
		this.label105.Location = new System.Drawing.Point(339, 82);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(14, 17);
		this.label105.TabIndex = 581;
		this.label105.Text = "*";
		this.label104.AutoSize = true;
		this.label104.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label104.ForeColor = System.Drawing.Color.Red;
		this.label104.Location = new System.Drawing.Point(1188, 33);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(14, 17);
		this.label104.TabIndex = 580;
		this.label104.Text = "*";
		this.label103.AutoSize = true;
		this.label103.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label103.ForeColor = System.Drawing.Color.Red;
		this.label103.Location = new System.Drawing.Point(1110, 33);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(14, 17);
		this.label103.TabIndex = 579;
		this.label103.Text = "*";
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.ForeColor = System.Drawing.Color.Red;
		this.label102.Location = new System.Drawing.Point(1021, 33);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(14, 17);
		this.label102.TabIndex = 578;
		this.label102.Text = "*";
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label101.ForeColor = System.Drawing.Color.Red;
		this.label101.Location = new System.Drawing.Point(516, 33);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(14, 17);
		this.label101.TabIndex = 577;
		this.label101.Text = "*";
		this.label100.AutoSize = true;
		this.label100.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label100.ForeColor = System.Drawing.Color.Red;
		this.label100.Location = new System.Drawing.Point(365, 33);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(14, 17);
		this.label100.TabIndex = 576;
		this.label100.Text = "*";
		this.BtnAddInDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnAddInDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddInDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddInDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddInDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddInDetail.Location = new System.Drawing.Point(1072, 148);
		this.BtnAddInDetail.Name = "BtnAddInDetail";
		this.BtnAddInDetail.Size = new System.Drawing.Size(36, 26);
		this.BtnAddInDetail.TabIndex = 21;
		this.BtnAddInDetail.Text = "+";
		this.BtnAddInDetail.UseVisualStyleBackColor = false;
		this.BtnAddInDetail.Click += new System.EventHandler(btnplus_Click);
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(263, 32);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 492;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.CmbFarmingNTradeInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbFarmingNTradeInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFarmingNTradeInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFarmingNTradeInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFarmingNTradeInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTradeInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFarmingNTradeInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTradeInDetail).Location = new System.Drawing.Point(289, 148);
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTradeInDetail).Name = "CmbFarmingNTradeInDetail";
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTradeInDetail).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTradeInDetail).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTradeInDetail).TextChanged += new System.EventHandler(CmbDeliveryTerm_TextChanged);
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.Black;
		this.label69.Location = new System.Drawing.Point(289, 130);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(95, 15);
		this.label69.TabIndex = 499;
		this.label69.Text = "Farming N Trade";
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(207, 32);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 491;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.label45);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Margin = new System.Windows.Forms.Padding(2);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1206, 26);
		this.panel23.TabIndex = 490;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.White;
		this.label45.Location = new System.Drawing.Point(7, 5);
		this.label45.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(111, 17);
		this.label45.TabIndex = 0;
		this.label45.Text = "Contract Detail";
		this.CmbPmDetailByItemCusInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPmDetailByItemCusInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPmDetailByItemCusInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPmDetailByItemCusInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance334).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance334).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance334).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance378).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance378;
		((AppearanceBase)appearance389).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance389).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance389;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance412).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance412;
		((AppearanceBase)appearance423).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance423).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance423;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance434).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance434).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance434).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance434).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance434).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance434;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPmDetailByItemCusInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPmDetailByItemCusInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusInDetail).Location = new System.Drawing.Point(973, 101);
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusInDetail).Name = "CmbPmDetailByItemCusInDetail";
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusInDetail).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusInDetail).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusInDetail).TextChanged += new System.EventHandler(CmbPmDetailByItemCus_TextChanged);
		this.CmbComditiyDetailByItemCusInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbComditiyDetailByItemCusInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbComditiyDetailByItemCusInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbComditiyDetailByItemCusInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance336).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance337).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance337).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance337).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance338).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance338;
		((SpecialBoxBase)((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance339).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance339).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance339).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance340).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance340;
		((AppearanceBase)appearance341).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance341).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance342;
		((AppearanceBase)appearance343).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance343).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance344).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance344).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance344).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance344).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance344;
		((AppearanceBase)appearance345).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance347).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance347;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance348).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance348;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbComditiyDetailByItemCusInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbComditiyDetailByItemCusInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbComditiyDetailByItemCusInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbComditiyDetailByItemCusInDetail).Location = new System.Drawing.Point(555, 101);
		((System.Windows.Forms.Control)(object)this.CmbComditiyDetailByItemCusInDetail).Name = "CmbComditiyDetailByItemCusInDetail";
		((System.Windows.Forms.Control)(object)this.CmbComditiyDetailByItemCusInDetail).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbComditiyDetailByItemCusInDetail).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.CmbComditiyDetailByItemCusInDetail).TextChanged += new System.EventHandler(cmbcomditiyDetailByItemCus_TextChanged);
		this.txtPmDetailInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPmDetailInDetail.Location = new System.Drawing.Point(5, 150);
		this.txtPmDetailInDetail.Name = "txtPmDetailInDetail";
		this.txtPmDetailInDetail.Size = new System.Drawing.Size(282, 23);
		this.txtPmDetailInDetail.TabIndex = 18;
		this.CmbItemNameInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance377).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance377).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance382).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance382).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance382).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance382).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance382;
		((AppearanceBase)appearance383).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance383;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance379).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance379).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance379).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance379).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance379;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance380).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance380).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance380;
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance381).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance381;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance387).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance387;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameInDetail).Location = new System.Drawing.Point(177, 54);
		((System.Windows.Forms.Control)(object)this.CmbItemNameInDetail).Name = "CmbItemNameInDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemNameInDetail).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameInDetail).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbItemNameInDetail).Leave += new System.EventHandler(combitem_Leave_1);
		this.txtCommodityDetailInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommodityDetailInDetail.Location = new System.Drawing.Point(771, 103);
		this.txtCommodityDetailInDetail.Name = "txtCommodityDetailInDetail";
		this.txtCommodityDetailInDetail.Size = new System.Drawing.Size(201, 23);
		this.txtCommodityDetailInDetail.TabIndex = 16;
		this.txtRemarksInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksInDetail.Location = new System.Drawing.Point(505, 150);
		this.txtRemarksInDetail.Name = "txtRemarksInDetail";
		this.txtRemarksInDetail.Size = new System.Drawing.Size(564, 23);
		this.txtRemarksInDetail.TabIndex = 20;
		this.label115.AutoSize = true;
		this.label115.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label115.Location = new System.Drawing.Point(5, 130);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(59, 15);
		this.label115.TabIndex = 489;
		this.label115.Text = "Pm Detail";
		this.btnCancelInDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelInDetail.FlatAppearance.BorderSize = 0;
		this.btnCancelInDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelInDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelInDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelInDetail.Location = new System.Drawing.Point(1140, 148);
		this.btnCancelInDetail.Name = "btnCancelInDetail";
		this.btnCancelInDetail.Size = new System.Drawing.Size(62, 26);
		this.btnCancelInDetail.TabIndex = 23;
		this.btnCancelInDetail.Text = "Cancel";
		this.btnCancelInDetail.UseVisualStyleBackColor = false;
		this.btnCancelInDetail.Visible = false;
		this.btnCancelInDetail.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(771, 83);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(104, 15);
		this.label48.TabIndex = 489;
		this.label48.Text = "Commodity Detail";
		this.txtQtyMTonInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyMTonInDetail.Location = new System.Drawing.Point(973, 56);
		this.txtQtyMTonInDetail.Name = "txtQtyMTonInDetail";
		this.txtQtyMTonInDetail.Size = new System.Drawing.Size(65, 23);
		this.txtQtyMTonInDetail.TabIndex = 6;
		this.txtQtyMTonInDetail.Tag = "MTon";
		this.txtQtyMTonInDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyMTonInDetail.TextChanged += new System.EventHandler(txtQtyMTon_TextChanged);
		this.txtQtyMTonInDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyMTon_KeyPress);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(505, 130);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(52, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Remarks";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(977, 34);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(41, 15);
		this.label16.TabIndex = 483;
		this.label16.Text = "M.Ton";
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label114.Location = new System.Drawing.Point(973, 83);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(103, 15);
		this.label114.TabIndex = 470;
		this.label114.Text = "Pm Detail By Item";
		this.txtRateInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateInDetail.Location = new System.Drawing.Point(273, 103);
		this.txtRateInDetail.Name = "txtRateInDetail";
		this.txtRateInDetail.Size = new System.Drawing.Size(80, 23);
		this.txtRateInDetail.TabIndex = 12;
		this.txtRateInDetail.Text = "0";
		this.txtRateInDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateInDetail.TextChanged += new System.EventHandler(txtCostMTon_TextChanged);
		this.txtRateInDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCostMTon_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(555, 83);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(144, 15);
		this.label39.TabIndex = 470;
		this.label39.Text = "Comoodity Detail By Item";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(384, 34);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(77, 15);
		this.label10.TabIndex = 476;
		this.label10.Text = "Packing Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(177, 34);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(35, 15);
		this.label1.TabIndex = 470;
		this.label1.Text = "Item ";
		this.btnUpdateInDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateInDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateInDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateInDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateInDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateInDetail.Location = new System.Drawing.Point(1072, 148);
		this.btnUpdateInDetail.Name = "btnUpdateInDetail";
		this.btnUpdateInDetail.Size = new System.Drawing.Size(66, 26);
		this.btnUpdateInDetail.TabIndex = 22;
		this.btnUpdateInDetail.Text = "Update";
		this.btnUpdateInDetail.UseVisualStyleBackColor = false;
		this.btnUpdateInDetail.Visible = false;
		this.btnUpdateInDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(273, 83);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(30, 15);
		this.label25.TabIndex = 487;
		this.label25.Text = "Rate";
		this.CmbPackingTypeInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingTypeInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingTypeInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingTypeInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance362).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance362).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance362).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance362;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance363).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance363).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance363;
		((AppearanceBase)appearance364).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance364).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance364;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance365).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance365;
		((AppearanceBase)appearance366).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance366).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance366;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance367).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance367).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance367).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance367).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance367;
		((AppearanceBase)appearance369).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance369;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance370).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance370).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance370;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance371).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance371;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingTypeInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingTypeInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeInDetail).Location = new System.Drawing.Point(384, 54);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeInDetail).Name = "CmbPackingTypeInDetail";
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeInDetail).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeInDetail).TabIndex = 2;
		this.CmbRateUomInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance268).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUomInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomInDetail).Location = new System.Drawing.Point(354, 101);
		((System.Windows.Forms.Control)(object)this.CmbRateUomInDetail).Name = "CmbRateUomInDetail";
		((System.Windows.Forms.Control)(object)this.CmbRateUomInDetail).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomInDetail).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbRateUomInDetail).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.txtPalletQtyInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPalletQtyInDetail.Location = new System.Drawing.Point(177, 103);
		this.txtPalletQtyInDetail.Name = "txtPalletQtyInDetail";
		this.txtPalletQtyInDetail.Size = new System.Drawing.Size(95, 23);
		this.txtPalletQtyInDetail.TabIndex = 11;
		this.txtPalletQtyInDetail.Tag = "NoofBags";
		this.txtPalletQtyInDetail.Text = "0";
		this.txtPalletQtyInDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPalletQtyInDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfBags_KeyPress);
		this.txtQtyInnerInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyInnerInDetail.Location = new System.Drawing.Point(95, 103);
		this.txtQtyInnerInDetail.Name = "txtQtyInnerInDetail";
		this.txtQtyInnerInDetail.Size = new System.Drawing.Size(80, 23);
		this.txtQtyInnerInDetail.TabIndex = 10;
		this.txtQtyInnerInDetail.Tag = "NoofBags";
		this.txtQtyInnerInDetail.Text = "0";
		this.txtQtyInnerInDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyInnerInDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfBags_KeyPress);
		this.txtOuterQtyInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOuterQtyInDetail.Location = new System.Drawing.Point(1125, 56);
		this.txtOuterQtyInDetail.Name = "txtOuterQtyInDetail";
		this.txtOuterQtyInDetail.Size = new System.Drawing.Size(77, 23);
		this.txtOuterQtyInDetail.TabIndex = 8;
		this.txtOuterQtyInDetail.Tag = "NoofBags";
		this.txtOuterQtyInDetail.Text = "0";
		this.txtOuterQtyInDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOuterQtyInDetail.TextChanged += new System.EventHandler(txtNoOfBags_TextChanged);
		this.txtOuterQtyInDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfBags_KeyPress);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(872, 34);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(54, 15);
		this.label24.TabIndex = 476;
		this.label24.Text = "CropYear";
		this.label120.AutoSize = true;
		this.label120.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label120.Location = new System.Drawing.Point(177, 83);
		this.label120.Name = "label120";
		this.label120.Size = new System.Drawing.Size(58, 15);
		this.label120.TabIndex = 485;
		this.label120.Text = "Pallet Qty";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(355, 83);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(61, 15);
		this.label13.TabIndex = 479;
		this.label13.Text = "Rate UOM";
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label116.Location = new System.Drawing.Point(95, 83);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(57, 15);
		this.label116.TabIndex = 485;
		this.label116.Text = "Qty Inner";
		this.CmbOuterUomInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbOuterUomInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOuterUomInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOuterUomInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance349).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance349;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance350).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance350).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance350).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance350).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance350;
		((AppearanceBase)appearance351).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance351;
		((SpecialBoxBase)((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance352).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance352).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance352).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance352).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance352;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance353).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance353).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance353;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance354).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance354;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance355).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance355;
		((AppearanceBase)appearance356).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance356).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance356;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance358).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance358).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance358).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance358).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance358).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance358;
		((AppearanceBase)appearance359).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance359;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance360).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance360;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance361).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance361;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOuterUomInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).Location = new System.Drawing.Point(1039, 54);
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).Name = "CmbOuterUomInDetail";
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).TextChanged += new System.EventHandler(combitempck_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(1125, 34);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(59, 15);
		this.label23.TabIndex = 485;
		this.label23.Text = "Qty Outer";
		this.CmbCropYearInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance404).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance404).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance404).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance404).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance404;
		((AppearanceBase)appearance405).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance405;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance196).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance413).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance413;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance414).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance414).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance414;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance415).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance415;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearInDetail).Location = new System.Drawing.Point(872, 54);
		((System.Windows.Forms.Control)(object)this.CmbCropYearInDetail).Name = "CmbCropYearInDetail";
		((System.Windows.Forms.Control)(object)this.CmbCropYearInDetail).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearInDetail).TabIndex = 5;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(444, 83);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(51, 15);
		this.label33.TabIndex = 485;
		this.label33.Text = "Amount";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1041, 34);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 474;
		this.label2.Text = "Outer UOM";
		this.txtAmountInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountInDetail.Location = new System.Drawing.Point(444, 103);
		this.txtAmountInDetail.Name = "txtAmountInDetail";
		this.txtAmountInDetail.ReadOnly = true;
		this.txtAmountInDetail.Size = new System.Drawing.Size(110, 23);
		this.txtAmountInDetail.TabIndex = 14;
		this.txtAmountInDetail.Text = "0";
		this.txtAmountInDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.tabPaymentTerm.Controls.Add(this.panel10);
		this.tabPaymentTerm.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentTerm.Margin = new System.Windows.Forms.Padding(2);
		this.tabPaymentTerm.Name = "tabPaymentTerm";
		this.tabPaymentTerm.Size = new System.Drawing.Size(1212, 300);
		this.tabPaymentTerm.TabIndex = 2;
		this.tabPaymentTerm.Text = "PaymentDetail";
		this.tabPaymentTerm.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1212, 300);
		this.panel10.TabIndex = 0;
		this.panel12.BackColor = System.Drawing.Color.White;
		this.panel12.Controls.Add(this.panel22);
		this.panel12.Controls.Add(this.panel14);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 26);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1212, 274);
		this.panel12.TabIndex = 1;
		this.panel22.Controls.Add(this.panel24);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(0, 63);
		this.panel22.Margin = new System.Windows.Forms.Padding(2);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1212, 211);
		this.panel22.TabIndex = 504;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerm);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Margin = new System.Windows.Forms.Padding(2);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1212, 211);
		this.panel24.TabIndex = 1;
		this.grdPaymentTerm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentTerm).BackColor = System.Drawing.Color.White;
		this.grdPaymentTerm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentTerm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerm.DesignTimeLayout = grdPaymentTerm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentTerm.FilterMode = (FilterMode)2;
		this.grdPaymentTerm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Name = "grdPaymentTerm";
		((ScrollableBase)this.grdPaymentTerm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Size = new System.Drawing.Size(1212, 211);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).TabIndex = 0;
		this.grdPaymentTerm.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTerm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTerm).VisualStyle = (VisualStyle)4;
		this.grdPaymentTerm.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTerm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).DoubleClick += new System.EventHandler(grdPaymentTerm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTerm_KeyDown);
		this.panel14.Controls.Add(this.label110);
		this.panel14.Controls.Add(this.label109);
		this.panel14.Controls.Add(this.label108);
		this.panel14.Controls.Add(this.btnCancelPayment);
		this.panel14.Controls.Add(this.btnUpdatePayment);
		this.panel14.Controls.Add(this.btnAddPayment);
		this.panel14.Controls.Add(this.label55);
		this.panel14.Controls.Add(this.txtRemarksPT);
		this.panel14.Controls.Add(this.txtlFcyAmountPT);
		this.panel14.Controls.Add(this.txtPercntOfTotalPT);
		this.panel14.Controls.Add(this.label53);
		this.panel14.Controls.Add(this.label54);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermPT);
		this.panel14.Controls.Add(this.label30);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Margin = new System.Windows.Forms.Padding(2);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1212, 63);
		this.panel14.TabIndex = 0;
		this.label110.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label110.ForeColor = System.Drawing.Color.Red;
		this.label110.Location = new System.Drawing.Point(404, 9);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(10, 10);
		this.label110.TabIndex = 588;
		this.label110.Text = "*";
		this.label109.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label109.ForeColor = System.Drawing.Color.Red;
		this.label109.Location = new System.Drawing.Point(268, 9);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(10, 10);
		this.label109.TabIndex = 587;
		this.label109.Text = "*";
		this.label108.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label108.ForeColor = System.Drawing.Color.Red;
		this.label108.Location = new System.Drawing.Point(168, 9);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(10, 10);
		this.label108.TabIndex = 586;
		this.label108.Text = "*";
		this.btnCancelPayment.BackColor = System.Drawing.Color.Teal;
		this.btnCancelPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelPayment.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelPayment.ForeColor = System.Drawing.Color.White;
		this.btnCancelPayment.Location = new System.Drawing.Point(1087, 28);
		this.btnCancelPayment.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelPayment.Name = "btnCancelPayment";
		this.btnCancelPayment.Size = new System.Drawing.Size(69, 24);
		this.btnCancelPayment.TabIndex = 6;
		this.btnCancelPayment.Text = "Cancel";
		this.btnCancelPayment.UseVisualStyleBackColor = false;
		this.btnCancelPayment.Click += new System.EventHandler(btnCancelPayment_Click);
		this.btnCancelPayment.Leave += new System.EventHandler(btnCancelPayment_Leave);
		this.btnUpdatePayment.BackColor = System.Drawing.Color.Teal;
		this.btnUpdatePayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdatePayment.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdatePayment.ForeColor = System.Drawing.Color.White;
		this.btnUpdatePayment.Location = new System.Drawing.Point(1018, 28);
		this.btnUpdatePayment.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdatePayment.Name = "btnUpdatePayment";
		this.btnUpdatePayment.Size = new System.Drawing.Size(70, 24);
		this.btnUpdatePayment.TabIndex = 5;
		this.btnUpdatePayment.Text = "Update";
		this.btnUpdatePayment.UseVisualStyleBackColor = false;
		this.btnUpdatePayment.Click += new System.EventHandler(btnUpdatePayment_Click);
		this.btnAddPayment.BackColor = System.Drawing.Color.Teal;
		this.btnAddPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddPayment.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddPayment.ForeColor = System.Drawing.Color.White;
		this.btnAddPayment.Location = new System.Drawing.Point(988, 28);
		this.btnAddPayment.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddPayment.Name = "btnAddPayment";
		this.btnAddPayment.Size = new System.Drawing.Size(31, 24);
		this.btnAddPayment.TabIndex = 4;
		this.btnAddPayment.Text = "+";
		this.btnAddPayment.UseVisualStyleBackColor = false;
		this.btnAddPayment.Click += new System.EventHandler(btnAddPayment_Click);
		this.btnAddPayment.Leave += new System.EventHandler(btnAddPayment_Leave);
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(285, 7);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(72, 15);
		this.label55.TabIndex = 562;
		this.label55.Text = "Fcy Amount";
		this.txtRemarksPT.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksPT.Location = new System.Drawing.Point(416, 28);
		this.txtRemarksPT.Name = "txtRemarksPT";
		this.txtRemarksPT.Size = new System.Drawing.Size(570, 23);
		this.txtRemarksPT.TabIndex = 3;
		this.txtlFcyAmountPT.Enabled = false;
		this.txtlFcyAmountPT.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtlFcyAmountPT.Location = new System.Drawing.Point(283, 28);
		this.txtlFcyAmountPT.Name = "txtlFcyAmountPT";
		this.txtlFcyAmountPT.Size = new System.Drawing.Size(131, 23);
		this.txtlFcyAmountPT.TabIndex = 2;
		this.txtlFcyAmountPT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPercntOfTotalPT.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPercntOfTotalPT.Location = new System.Drawing.Point(181, 28);
		this.txtPercntOfTotalPT.Name = "txtPercntOfTotalPT";
		this.txtPercntOfTotalPT.Size = new System.Drawing.Size(100, 23);
		this.txtPercntOfTotalPT.TabIndex = 1;
		this.txtPercntOfTotalPT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPercntOfTotalPT.TextChanged += new System.EventHandler(PercntOfTotal_TextChanged);
		this.txtPercntOfTotalPT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PercntOfTotal_KeyPress);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(191, 7);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(62, 15);
		this.label53.TabIndex = 503;
		this.label53.Text = "% Of Total";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(7, 7);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(84, 15);
		this.label54.TabIndex = 502;
		this.label54.Text = "Payment Term";
		this.CmbPaymentTermPT.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermPT.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermPT.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermPT.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance429).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance429).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Appearance = (AppearanceBase)(object)appearance429;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance430).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance430).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance430).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance430).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance430;
		((AppearanceBase)appearance431).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance431;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance432).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance432).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance432).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance432).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance432;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance433).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance433).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance433;
		((AppearanceBase)appearance435).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance435).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance435;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance436).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance436;
		((AppearanceBase)appearance437).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance437).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance437;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance438).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance438).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance438).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance438).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance438).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance438;
		((AppearanceBase)appearance439).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance439;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance440).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance440).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance440;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance441).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance441;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermPT).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPT).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermPT.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPT).Location = new System.Drawing.Point(7, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPT).Name = "CmbPaymentTermPT";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPT).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPT).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPT).TextChanged += new System.EventHandler(CmbPaymentTermDetail_TextChanged);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(420, 7);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(57, 15);
		this.label30.TabIndex = 570;
		this.label30.Text = "Remarsks";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label57);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1212, 26);
		this.panel11.TabIndex = 0;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.White;
		this.label57.Location = new System.Drawing.Point(3, 4);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(135, 19);
		this.label57.TabIndex = 561;
		this.label57.Text = "Payment Detail";
		this.tabPackingMaterialDetail.Controls.Add(this.panel15);
		this.tabPackingMaterialDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPackingMaterialDetail.Margin = new System.Windows.Forms.Padding(2);
		this.tabPackingMaterialDetail.Name = "tabPackingMaterialDetail";
		this.tabPackingMaterialDetail.Size = new System.Drawing.Size(1212, 300);
		this.tabPackingMaterialDetail.TabIndex = 3;
		this.tabPackingMaterialDetail.Text = "Packing Material Detail";
		this.tabPackingMaterialDetail.UseVisualStyleBackColor = true;
		this.panel15.Controls.Add(this.panel19);
		this.panel15.Controls.Add(this.panel16);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Margin = new System.Windows.Forms.Padding(2);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1212, 300);
		this.panel15.TabIndex = 0;
		this.panel19.Controls.Add(this.panel21);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 26);
		this.panel19.Margin = new System.Windows.Forms.Padding(2);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1212, 274);
		this.panel19.TabIndex = 1;
		this.panel21.Controls.Add(this.panel25);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 102);
		this.panel21.Margin = new System.Windows.Forms.Padding(2);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1212, 172);
		this.panel21.TabIndex = 1;
		this.panel25.Controls.Add(this.panel27);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Margin = new System.Windows.Forms.Padding(2);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1212, 172);
		this.panel25.TabIndex = 0;
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel27.Location = new System.Drawing.Point(0, 0);
		this.panel27.Margin = new System.Windows.Forms.Padding(2);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1212, 172);
		this.panel27.TabIndex = 1;
		this.grdPackingMaterialDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPackingMaterialDetail).BackColor = System.Drawing.Color.White;
		this.grdPackingMaterialDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPackingMaterialDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterialDetail.DesignTimeLayout = grdPackingMaterialDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingMaterialDetail.FilterMode = (FilterMode)2;
		this.grdPackingMaterialDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterialDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Name = "grdPackingMaterialDetail";
		((ScrollableBase)this.grdPackingMaterialDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Size = new System.Drawing.Size(1212, 172);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).TabIndex = 0;
		this.grdPackingMaterialDetail.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterialDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterialDetail).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterialDetail.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterialDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).DoubleClick += new System.EventHandler(grdPackingMaterialDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPackingMaterialDetail_KeyDown);
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add(this.btnPackingAdd);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandPmDetail);
		this.panel20.Controls.Add(this.panel30);
		this.panel20.Controls.Add(this.rdPackingItemCode);
		this.panel20.Controls.Add(this.rdPackingItemName);
		this.panel20.Controls.Add(this.btnPackingCancel);
		this.panel20.Controls.Add(this.btnPackingUpdate);
		this.panel20.Controls.Add(this.txtRemarksPmDetail);
		this.panel20.Controls.Add(this.txtAmountPmDetail);
		this.panel20.Controls.Add(this.txtRatePmDetail);
		this.panel20.Controls.Add(this.txtBrandInnerQtyPmDetail);
		this.panel20.Controls.Add(this.txtBrandOuterQtyPmDetail);
		this.panel20.Controls.Add(this.txtOuterQtyPmDetail);
		this.panel20.Controls.Add(this.txtInnerQtyPmDetail);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUomPmDetail);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandUomPmDetail);
		this.panel20.Controls.Add(this.label80);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePmDetail);
		this.panel20.Controls.Add(this.label77);
		this.panel20.Controls.Add(this.label79);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemPmDetail);
		this.panel20.Controls.Add(this.label78);
		this.panel20.Controls.Add(this.label81);
		this.panel20.Controls.Add(this.label64);
		this.panel20.Controls.Add(this.label63);
		this.panel20.Controls.Add(this.label62);
		this.panel20.Controls.Add(this.label61);
		this.panel20.Controls.Add(this.label60);
		this.panel20.Controls.Add(this.label58);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Margin = new System.Windows.Forms.Padding(2);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1212, 102);
		this.panel20.TabIndex = 0;
		this.btnPackingAdd.BackColor = System.Drawing.Color.Teal;
		this.btnPackingAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPackingAdd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPackingAdd.ForeColor = System.Drawing.Color.White;
		this.btnPackingAdd.Location = new System.Drawing.Point(995, 76);
		this.btnPackingAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnPackingAdd.Name = "btnPackingAdd";
		this.btnPackingAdd.Size = new System.Drawing.Size(34, 23);
		this.btnPackingAdd.TabIndex = 14;
		this.btnPackingAdd.Text = "+";
		this.btnPackingAdd.UseVisualStyleBackColor = false;
		this.btnPackingAdd.Click += new System.EventHandler(btnPackingAdd_Click);
		this.btnPackingAdd.Leave += new System.EventHandler(btnPackingAdd_Leave);
		this.CmbBrandPmDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandPmDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandPmDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandPmDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandPmDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandPmDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandPmDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandPmDetail).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.CmbBrandPmDetail).Name = "CmbBrandPmDetail";
		((System.Windows.Forms.Control)(object)this.CmbBrandPmDetail).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandPmDetail).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbBrandPmDetail).Leave += new System.EventHandler(cmbBrandForPmEntry_Leave);
		this.panel30.Controls.Add(this.radBrandName);
		this.panel30.Controls.Add(this.label47);
		this.panel30.Controls.Add(this.rabBrandName);
		this.panel30.Location = new System.Drawing.Point(4, 3);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(183, 24);
		this.panel30.TabIndex = 0;
		this.radBrandName.AutoSize = true;
		this.radBrandName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radBrandName.Location = new System.Drawing.Point(99, 4);
		this.radBrandName.Name = "radBrandName";
		this.radBrandName.Size = new System.Drawing.Size(52, 19);
		this.radBrandName.TabIndex = 0;
		this.radBrandName.Text = "Code";
		this.radBrandName.UseVisualStyleBackColor = true;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(3, 6);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(38, 15);
		this.label47.TabIndex = 560;
		this.label47.Text = "Brand";
		this.rabBrandName.AutoSize = true;
		this.rabBrandName.Checked = true;
		this.rabBrandName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rabBrandName.Location = new System.Drawing.Point(43, 4);
		this.rabBrandName.Name = "rabBrandName";
		this.rabBrandName.Size = new System.Drawing.Size(57, 19);
		this.rabBrandName.TabIndex = 574;
		this.rabBrandName.TabStop = true;
		this.rabBrandName.Text = "Name";
		this.rabBrandName.UseVisualStyleBackColor = true;
		this.rabBrandName.CheckedChanged += new System.EventHandler(rabBrandName_CheckedChanged);
		this.rdPackingItemCode.AutoSize = true;
		this.rdPackingItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdPackingItemCode.Location = new System.Drawing.Point(800, 6);
		this.rdPackingItemCode.Name = "rdPackingItemCode";
		this.rdPackingItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdPackingItemCode.TabIndex = 575;
		this.rdPackingItemCode.Text = "Code";
		this.rdPackingItemCode.UseVisualStyleBackColor = true;
		this.rdPackingItemName.AutoSize = true;
		this.rdPackingItemName.Checked = true;
		this.rdPackingItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdPackingItemName.Location = new System.Drawing.Point(747, 6);
		this.rdPackingItemName.Name = "rdPackingItemName";
		this.rdPackingItemName.Size = new System.Drawing.Size(57, 19);
		this.rdPackingItemName.TabIndex = 574;
		this.rdPackingItemName.TabStop = true;
		this.rdPackingItemName.Text = "Name";
		this.rdPackingItemName.UseVisualStyleBackColor = true;
		this.rdPackingItemName.CheckedChanged += new System.EventHandler(rdPackingItemName_CheckedChanged);
		this.btnPackingCancel.BackColor = System.Drawing.Color.Teal;
		this.btnPackingCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPackingCancel.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPackingCancel.ForeColor = System.Drawing.Color.White;
		this.btnPackingCancel.Location = new System.Drawing.Point(1063, 76);
		this.btnPackingCancel.Margin = new System.Windows.Forms.Padding(2);
		this.btnPackingCancel.Name = "btnPackingCancel";
		this.btnPackingCancel.Size = new System.Drawing.Size(66, 23);
		this.btnPackingCancel.TabIndex = 15;
		this.btnPackingCancel.Text = "Cancel";
		this.btnPackingCancel.UseVisualStyleBackColor = false;
		this.btnPackingCancel.Click += new System.EventHandler(btnPackingCancel_Click);
		this.btnPackingCancel.Leave += new System.EventHandler(btnPackingCancel_Leave);
		this.btnPackingUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnPackingUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPackingUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPackingUpdate.ForeColor = System.Drawing.Color.White;
		this.btnPackingUpdate.Location = new System.Drawing.Point(995, 76);
		this.btnPackingUpdate.Margin = new System.Windows.Forms.Padding(2);
		this.btnPackingUpdate.Name = "btnPackingUpdate";
		this.btnPackingUpdate.Size = new System.Drawing.Size(67, 23);
		this.btnPackingUpdate.TabIndex = 13;
		this.btnPackingUpdate.Text = "Update";
		this.btnPackingUpdate.UseVisualStyleBackColor = false;
		this.btnPackingUpdate.Click += new System.EventHandler(btnPackingUpdate_Click);
		this.txtRemarksPmDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksPmDetail.Location = new System.Drawing.Point(220, 76);
		this.txtRemarksPmDetail.Name = "txtRemarksPmDetail";
		this.txtRemarksPmDetail.Size = new System.Drawing.Size(773, 23);
		this.txtRemarksPmDetail.TabIndex = 12;
		this.txtAmountPmDetail.Enabled = false;
		this.txtAmountPmDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountPmDetail.Location = new System.Drawing.Point(109, 76);
		this.txtAmountPmDetail.Name = "txtAmountPmDetail";
		this.txtAmountPmDetail.Size = new System.Drawing.Size(109, 23);
		this.txtAmountPmDetail.TabIndex = 11;
		this.txtAmountPmDetail.Text = "0";
		this.txtAmountPmDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRatePmDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRatePmDetail.Location = new System.Drawing.Point(3, 76);
		this.txtRatePmDetail.Name = "txtRatePmDetail";
		this.txtRatePmDetail.Size = new System.Drawing.Size(104, 23);
		this.txtRatePmDetail.TabIndex = 10;
		this.txtRatePmDetail.Text = "0";
		this.txtRatePmDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRatePmDetail.TextChanged += new System.EventHandler(txtPackingItemRate_TextChanged);
		this.txtRatePmDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingItemRate_KeyPress);
		this.txtBrandInnerQtyPmDetail.Enabled = false;
		this.txtBrandInnerQtyPmDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrandInnerQtyPmDetail.Location = new System.Drawing.Point(578, 31);
		this.txtBrandInnerQtyPmDetail.Name = "txtBrandInnerQtyPmDetail";
		this.txtBrandInnerQtyPmDetail.Size = new System.Drawing.Size(111, 23);
		this.txtBrandInnerQtyPmDetail.TabIndex = 5;
		this.txtBrandInnerQtyPmDetail.Text = "0";
		this.txtBrandInnerQtyPmDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrandInnerQtyPmDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrandInnerQtyPm_KeyPress);
		this.txtBrandOuterQtyPmDetail.Enabled = false;
		this.txtBrandOuterQtyPmDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrandOuterQtyPmDetail.Location = new System.Drawing.Point(465, 31);
		this.txtBrandOuterQtyPmDetail.Name = "txtBrandOuterQtyPmDetail";
		this.txtBrandOuterQtyPmDetail.Size = new System.Drawing.Size(111, 23);
		this.txtBrandOuterQtyPmDetail.TabIndex = 4;
		this.txtBrandOuterQtyPmDetail.Text = "0";
		this.txtBrandOuterQtyPmDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrandOuterQtyPmDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrandOuterQtyPm_KeyPress);
		this.txtOuterQtyPmDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOuterQtyPmDetail.Location = new System.Drawing.Point(995, 31);
		this.txtOuterQtyPmDetail.Name = "txtOuterQtyPmDetail";
		this.txtOuterQtyPmDetail.Size = new System.Drawing.Size(104, 23);
		this.txtOuterQtyPmDetail.TabIndex = 8;
		this.txtOuterQtyPmDetail.Text = "0";
		this.txtOuterQtyPmDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOuterQtyPmDetail.TextChanged += new System.EventHandler(txtOuterQtyPm_TextChanged);
		this.txtOuterQtyPmDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOuterQtyPm_KeyPress);
		this.txtInnerQtyPmDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInnerQtyPmDetail.Location = new System.Drawing.Point(1100, 31);
		this.txtInnerQtyPmDetail.Name = "txtInnerQtyPmDetail";
		this.txtInnerQtyPmDetail.Size = new System.Drawing.Size(104, 23);
		this.txtInnerQtyPmDetail.TabIndex = 9;
		this.txtInnerQtyPmDetail.Text = "0";
		this.txtInnerQtyPmDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInnerQtyPmDetail.TextChanged += new System.EventHandler(txtIPackingtemQty_TextChanged);
		this.txtInnerQtyPmDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInnerQtyPm_KeyPress);
		this.CmbUomPmDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbUomPmDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUomPmDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUomPmDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance442).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance442).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance442;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance443).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance443).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance443).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance443).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance443;
		((AppearanceBase)appearance444).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance444;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUomPmDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUomPmDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUomPmDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUomPmDetail).Location = new System.Drawing.Point(896, 29);
		((System.Windows.Forms.Control)(object)this.CmbUomPmDetail).Name = "CmbUomPmDetail";
		((System.Windows.Forms.Control)(object)this.CmbUomPmDetail).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.CmbUomPmDetail).TabIndex = 7;
		this.CmbBrandUomPmDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandUomPmDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandUomPmDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandUomPmDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance241;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance247).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandUomPmDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPmDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPmDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandUomPmDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPmDetail).Location = new System.Drawing.Point(220, 29);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPmDetail).Name = "CmbBrandUomPmDetail";
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPmDetail).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPmDetail).TabIndex = 2;
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.Location = new System.Drawing.Point(898, 8);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(82, 15);
		this.label80.TabIndex = 566;
		this.label80.Text = "PM Item Uom";
		this.CmbBrandPackingTypePmDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandPackingTypePmDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandPackingTypePmDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandPackingTypePmDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandPackingTypePmDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePmDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandPackingTypePmDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePmDetail).Location = new System.Drawing.Point(321, 29);
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePmDetail).Name = "CmbBrandPackingTypePmDetail";
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePmDetail).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePmDetail).TabIndex = 3;
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(222, 8);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(67, 15);
		this.label77.TabIndex = 566;
		this.label77.Text = "Brand Uom";
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.Location = new System.Drawing.Point(578, 8);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(91, 15);
		this.label79.TabIndex = 565;
		this.label79.Text = "Brand Inner Qty";
		this.CmbItemPmDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemPmDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemPmDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemPmDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemPmDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemPmDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemPmDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemPmDetail).Location = new System.Drawing.Point(692, 29);
		((System.Windows.Forms.Control)(object)this.CmbItemPmDetail).Name = "CmbItemPmDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemPmDetail).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemPmDetail).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbItemPmDetail).Leave += new System.EventHandler(CmbPMItem_Leave);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(465, 8);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(93, 15);
		this.label78.TabIndex = 565;
		this.label78.Text = "Brand Outer Qty";
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.Location = new System.Drawing.Point(1100, 8);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(81, 15);
		this.label81.TabIndex = 565;
		this.label81.Text = "PM Inner  Qty";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(323, 8);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(111, 15);
		this.label64.TabIndex = 566;
		this.label64.Text = "Brand Packing Type\r\n";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(993, 8);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(83, 15);
		this.label63.TabIndex = 565;
		this.label63.Text = "PM Outer  Qty";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(3, 59);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(58, 15);
		this.label62.TabIndex = 564;
		this.label62.Text = "Item Rate";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(109, 59);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(51, 15);
		this.label61.TabIndex = 563;
		this.label61.Text = "Amount";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(220, 59);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(52, 15);
		this.label60.TabIndex = 562;
		this.label60.Text = "Remarks";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(692, 8);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(53, 15);
		this.label58.TabIndex = 560;
		this.label58.Text = "PM Item";
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label56);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Margin = new System.Windows.Forms.Padding(2);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1212, 26);
		this.panel16.TabIndex = 0;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.White;
		this.label56.Location = new System.Drawing.Point(3, 4);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(200, 19);
		this.label56.TabIndex = 560;
		this.label56.Text = "Packing Material Detail";
		this.tabOtherItems.Controls.Add(this.panel9);
		this.tabOtherItems.Controls.Add(this.panel8);
		this.tabOtherItems.Location = new System.Drawing.Point(4, 25);
		this.tabOtherItems.Name = "tabOtherItems";
		this.tabOtherItems.Padding = new System.Windows.Forms.Padding(3);
		this.tabOtherItems.Size = new System.Drawing.Size(1212, 300);
		this.tabOtherItems.TabIndex = 0;
		this.tabOtherItems.Text = "Other Items";
		this.tabOtherItems.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 84);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1206, 213);
		this.panel9.TabIndex = 2;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1206, 213);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 0;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		this.grdotheritems.ColumnButtonClick += new ColumnActionEventHandler(grdotheritems_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).KeyDown += new System.Windows.Forms.KeyEventHandler(grdotheritems_KeyDown);
		this.panel8.Controls.Add(this.RadItemCodeOtherDetail);
		this.panel8.Controls.Add(this.RadItemNameOtherDetail);
		this.panel8.Controls.Add(this.panel26);
		this.panel8.Controls.Add(this.btnCancelOtherDetail);
		this.panel8.Controls.Add(this.btnUpdateOtherDetail);
		this.panel8.Controls.Add(this.btnaddOtherDetail);
		this.panel8.Controls.Add(this.label38);
		this.panel8.Controls.Add(this.txtRemarksOtherDetail);
		this.panel8.Controls.Add(this.txtQtyOtherDetail);
		this.panel8.Controls.Add(this.label35);
		this.panel8.Controls.Add(this.txtRateOtherDetail);
		this.panel8.Controls.Add(this.label36);
		this.panel8.Controls.Add(this.label37);
		this.panel8.Controls.Add(this.txtAmountOtherDetail);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemOtherDetail);
		this.panel8.Controls.Add(this.label31);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1206, 81);
		this.panel8.TabIndex = 0;
		this.RadItemCodeOtherDetail.AutoSize = true;
		this.RadItemCodeOtherDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemCodeOtherDetail.Location = new System.Drawing.Point(97, 28);
		this.RadItemCodeOtherDetail.Name = "RadItemCodeOtherDetail";
		this.RadItemCodeOtherDetail.Size = new System.Drawing.Size(52, 19);
		this.RadItemCodeOtherDetail.TabIndex = 498;
		this.RadItemCodeOtherDetail.Text = "Code";
		this.RadItemCodeOtherDetail.UseVisualStyleBackColor = true;
		this.RadItemNameOtherDetail.AutoSize = true;
		this.RadItemNameOtherDetail.Checked = true;
		this.RadItemNameOtherDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemNameOtherDetail.Location = new System.Drawing.Point(41, 28);
		this.RadItemNameOtherDetail.Name = "RadItemNameOtherDetail";
		this.RadItemNameOtherDetail.Size = new System.Drawing.Size(57, 19);
		this.RadItemNameOtherDetail.TabIndex = 497;
		this.RadItemNameOtherDetail.TabStop = true;
		this.RadItemNameOtherDetail.Text = "Name";
		this.RadItemNameOtherDetail.UseVisualStyleBackColor = true;
		this.RadItemNameOtherDetail.CheckedChanged += new System.EventHandler(RadItemNameOtherDetail_CheckedChanged);
		this.panel26.BackColor = System.Drawing.Color.Teal;
		this.panel26.Controls.Add(this.label46);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel26.Location = new System.Drawing.Point(0, 0);
		this.panel26.Margin = new System.Windows.Forms.Padding(2);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1206, 26);
		this.panel26.TabIndex = 496;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.White;
		this.label46.Location = new System.Drawing.Point(7, 5);
		this.label46.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(93, 17);
		this.label46.TabIndex = 0;
		this.label46.Text = "Other Items";
		this.btnCancelOtherDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelOtherDetail.FlatAppearance.BorderSize = 0;
		this.btnCancelOtherDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelOtherDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelOtherDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelOtherDetail.Location = new System.Drawing.Point(1084, 49);
		this.btnCancelOtherDetail.Name = "btnCancelOtherDetail";
		this.btnCancelOtherDetail.Size = new System.Drawing.Size(62, 26);
		this.btnCancelOtherDetail.TabIndex = 7;
		this.btnCancelOtherDetail.Text = "Cancel";
		this.btnCancelOtherDetail.UseVisualStyleBackColor = false;
		this.btnCancelOtherDetail.Visible = false;
		this.btnCancelOtherDetail.Click += new System.EventHandler(btnoCancel_Click);
		this.btnUpdateOtherDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateOtherDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateOtherDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateOtherDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateOtherDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateOtherDetail.Location = new System.Drawing.Point(1018, 49);
		this.btnUpdateOtherDetail.Name = "btnUpdateOtherDetail";
		this.btnUpdateOtherDetail.Size = new System.Drawing.Size(65, 26);
		this.btnUpdateOtherDetail.TabIndex = 6;
		this.btnUpdateOtherDetail.Text = "Update";
		this.btnUpdateOtherDetail.UseVisualStyleBackColor = false;
		this.btnUpdateOtherDetail.Visible = false;
		this.btnUpdateOtherDetail.Click += new System.EventHandler(btnoUpdate_Click);
		this.btnaddOtherDetail.BackColor = System.Drawing.Color.Teal;
		this.btnaddOtherDetail.FlatAppearance.BorderSize = 0;
		this.btnaddOtherDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnaddOtherDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnaddOtherDetail.ForeColor = System.Drawing.Color.White;
		this.btnaddOtherDetail.Location = new System.Drawing.Point(992, 49);
		this.btnaddOtherDetail.Name = "btnaddOtherDetail";
		this.btnaddOtherDetail.Size = new System.Drawing.Size(25, 26);
		this.btnaddOtherDetail.TabIndex = 5;
		this.btnaddOtherDetail.Text = "+";
		this.btnaddOtherDetail.UseVisualStyleBackColor = false;
		this.btnaddOtherDetail.Click += new System.EventHandler(btnoadd_Click);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(546, 31);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(52, 15);
		this.label38.TabIndex = 495;
		this.label38.Text = "Remarks";
		this.txtRemarksOtherDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksOtherDetail.Location = new System.Drawing.Point(545, 51);
		this.txtRemarksOtherDetail.Name = "txtRemarksOtherDetail";
		this.txtRemarksOtherDetail.Size = new System.Drawing.Size(443, 23);
		this.txtRemarksOtherDetail.TabIndex = 4;
		this.txtQtyOtherDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyOtherDetail.Location = new System.Drawing.Point(257, 51);
		this.txtQtyOtherDetail.Name = "txtQtyOtherDetail";
		this.txtQtyOtherDetail.Size = new System.Drawing.Size(101, 23);
		this.txtQtyOtherDetail.TabIndex = 1;
		this.txtQtyOtherDetail.Text = "0";
		this.txtQtyOtherDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyOtherDetail.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(264, 30);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(26, 15);
		this.label35.TabIndex = 491;
		this.label35.Text = "Qty";
		this.txtRateOtherDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateOtherDetail.Location = new System.Drawing.Point(360, 51);
		this.txtRateOtherDetail.Name = "txtRateOtherDetail";
		this.txtRateOtherDetail.Size = new System.Drawing.Size(80, 23);
		this.txtRateOtherDetail.TabIndex = 2;
		this.txtRateOtherDetail.Text = "0";
		this.txtRateOtherDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateOtherDetail.TextChanged += new System.EventHandler(txtoRate_TextChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(360, 30);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(30, 15);
		this.label36.TabIndex = 493;
		this.label36.Text = "Rate";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(442, 30);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(51, 15);
		this.label37.TabIndex = 492;
		this.label37.Text = "Amount";
		this.txtAmountOtherDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountOtherDetail.Location = new System.Drawing.Point(442, 51);
		this.txtAmountOtherDetail.Name = "txtAmountOtherDetail";
		this.txtAmountOtherDetail.ReadOnly = true;
		this.txtAmountOtherDetail.Size = new System.Drawing.Size(101, 23);
		this.txtAmountOtherDetail.TabIndex = 3;
		this.txtAmountOtherDetail.Text = "0";
		this.txtAmountOtherDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbItemOtherDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemOtherDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemOtherDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemOtherDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance416).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance416).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance416;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance417).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance417).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance417).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance417).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance417;
		((AppearanceBase)appearance418).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance418;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance419).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance419).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance419).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance419).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance419;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance420).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance420).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance420;
		((AppearanceBase)appearance421).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance421).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance421;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance422).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance422;
		((AppearanceBase)appearance424).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance424).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance424;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance425).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance425).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance425).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance425).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance425).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance425;
		((AppearanceBase)appearance426).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance426;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance427).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance427).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance427;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance428).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance428;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemOtherDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemOtherDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemOtherDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemOtherDetail).Location = new System.Drawing.Point(9, 49);
		((System.Windows.Forms.Control)(object)this.CmbItemOtherDetail).Name = "CmbItemOtherDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemOtherDetail).Size = new System.Drawing.Size(245, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemOtherDetail).TabIndex = 0;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(10, 29);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(67, 15);
		this.label31.TabIndex = 472;
		this.label31.Text = "Item Name";
		this.tabOtherCharges.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail);
		this.tabOtherCharges.Controls.Add(this.panel37);
		this.tabOtherCharges.Controls.Add(this.panel38);
		this.tabOtherCharges.Location = new System.Drawing.Point(4, 25);
		this.tabOtherCharges.Name = "tabOtherCharges";
		this.tabOtherCharges.Size = new System.Drawing.Size(1212, 300);
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
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Size = new System.Drawing.Size(1212, 223);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).TabIndex = 4;
		this.grdOtherChargesDetail.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesDetail).VisualStyle = (VisualStyle)4;
		this.grdOtherChargesDetail.ColumnButtonClick += new ColumnActionEventHandler(grdOtherChargesDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).DoubleClick += new System.EventHandler(grdOtherChargesDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOtherChargesDetail_KeyDown);
		this.panel37.BackColor = System.Drawing.Color.Teal;
		this.panel37.Controls.Add(this.ctrlgrdOtherChargesDetail);
		this.panel37.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel37.Location = new System.Drawing.Point(0, 54);
		this.panel37.Name = "panel37";
		this.panel37.Size = new System.Drawing.Size(1212, 23);
		this.panel37.TabIndex = 5;
		this.ctrlgrdOtherChargesDetail.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdOtherChargesDetail.FormName = null;
		this.ctrlgrdOtherChargesDetail.Location = new System.Drawing.Point(1170, 0);
		this.ctrlgrdOtherChargesDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdOtherChargesDetail.MyGrid = this.grdOtherChargesDetail;
		this.ctrlgrdOtherChargesDetail.Name = "ctrlgrdOtherChargesDetail";
		this.ctrlgrdOtherChargesDetail.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdOtherChargesDetail.TabIndex = 27;
		this.ctrlgrdOtherChargesDetail.Load += new System.EventHandler(ctrlgrdOtherChargesDetail_Load);
		this.panel38.Controls.Add(this.BtnAddChargeDetail);
		this.panel38.Controls.Add(this.label130);
		this.panel38.Controls.Add(this.txtRemarksChargeDetail);
		this.panel38.Controls.Add(this.label134);
		this.panel38.Controls.Add(this.label131);
		this.panel38.Controls.Add(this.label132);
		this.panel38.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesNameChargeDetail);
		this.panel38.Controls.Add(this.label136);
		this.panel38.Controls.Add(this.BtnCancelChargeDetail);
		this.panel38.Controls.Add(this.BtnUpdateChargeDetail);
		this.panel38.Controls.Add(this.label133);
		this.panel38.Controls.Add(this.label140);
		this.panel38.Controls.Add(this.txtLessAmountChargeDetail);
		this.panel38.Controls.Add(this.txtAddAmountChargeDetail);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1212, 54);
		this.panel38.TabIndex = 3;
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
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.panel28);
		this.panel4.Controls.Add(this.txtApprovalRemarks);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Controls.Add(this.label50);
		this.panel4.Controls.Add(this.txtDeliveryRemarks);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1220, 368);
		this.panel4.TabIndex = 1;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbNotifyParty2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbNotifyParty1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbConsignee);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractNo);
		this.groupBox1.Controls.Add(this.btnDefineBankLocal);
		this.groupBox1.Controls.Add(this.btnDefineBankForeign);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.txtFCYAmount);
		this.groupBox1.Controls.Add(this.txtMTon);
		this.groupBox1.Controls.Add(this.txtNoOfContainers);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFCYCode);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.groupBox1.Controls.Add(this.txtDeliveryDays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox1.Controls.Add(this.txtSpecialInstructions);
		this.groupBox1.Controls.Add(this.txtCommodityDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExporterbank);
		this.groupBox1.Controls.Add(this.label99);
		this.groupBox1.Controls.Add(this.label98);
		this.groupBox1.Controls.Add(this.label97);
		this.groupBox1.Controls.Add(this.label96);
		this.groupBox1.Controls.Add(this.label94);
		this.groupBox1.Controls.Add(this.label93);
		this.groupBox1.Controls.Add(this.label92);
		this.groupBox1.Controls.Add(this.label88);
		this.groupBox1.Controls.Add(this.label86);
		this.groupBox1.Controls.Add(this.label84);
		this.groupBox1.Controls.Add(this.label83);
		this.groupBox1.Controls.Add(this.label82);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label95);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.txtPMScheduleDate);
		this.groupBox1.Controls.Add(this.labelDCG);
		this.groupBox1.Controls.Add(this.label72);
		this.groupBox1.Controls.Add(this.txtInspectionScheduleDate);
		this.groupBox1.Controls.Add(this.label71);
		this.groupBox1.Controls.Add(this.txtProductionScheduleDate);
		this.groupBox1.Controls.Add(this.label70);
		this.groupBox1.Controls.Add(this.txtFactoryLoadingDate);
		this.groupBox1.Controls.Add(this.label68);
		this.groupBox1.Controls.Add(this.datShipmentStartDate);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtCustomerOrderNo);
		this.groupBox1.Controls.Add(this.txtProductSpecification);
		this.groupBox1.Controls.Add(this.txtLotRefNo);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.ApprovalDate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.txtCustomerOrderDate);
		this.groupBox1.Controls.Add(this.txtDocDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbNotifyParty3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalePerson);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtShipmentDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbImporterBank);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentCustomGroup);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingPort);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDestinationPort);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 28);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1208, 336);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.CmbNotifyParty2.AllowNull = (DefaultableBoolean)1;
		this.CmbNotifyParty2.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbNotifyParty2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbNotifyParty2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance265).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbNotifyParty2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbNotifyParty2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).Location = new System.Drawing.Point(93, 173);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).Name = "CmbNotifyParty2";
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).Size = new System.Drawing.Size(333, 26);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty2).TabIndex = 8;
		this.CmbNotifyParty1.AllowNull = (DefaultableBoolean)1;
		this.CmbNotifyParty1.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbNotifyParty1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbNotifyParty1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance270).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance270).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance271;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance272).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance276).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance277).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance277).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance277).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance277).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance278).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbNotifyParty1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbNotifyParty1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).Location = new System.Drawing.Point(93, 146);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).Name = "CmbNotifyParty1";
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).Size = new System.Drawing.Size(333, 26);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty1).TabIndex = 7;
		this.CmbConsignee.AllowNull = (DefaultableBoolean)1;
		this.CmbConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbConsignee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Location = new System.Drawing.Point(93, 92);
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Name = "CmbConsignee";
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Size = new System.Drawing.Size(333, 26);
		((System.Windows.Forms.Control)(object)this.CmbConsignee).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbConsignee).Leave += new System.EventHandler(CmbCustomer_Leave);
		this.CmbContractNo.AllowNull = (DefaultableBoolean)1;
		this.CmbContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance391).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance391).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance391;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance392).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance392).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance392).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance392).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance392;
		((AppearanceBase)appearance393).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance393;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance394).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance394).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance394).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance394;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance395).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance395).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance395;
		((AppearanceBase)appearance396).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance396).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance396;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance397).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance397;
		((AppearanceBase)appearance398).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance398).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance398;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance399).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance399).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance399).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance399).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance399).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance399;
		((AppearanceBase)appearance401).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance401;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance402).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance402).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance402;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance403).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance403;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Location = new System.Drawing.Point(93, 10);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Name = "CmbContractNo";
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).TabIndex = 0;
		this.btnDefineBankLocal.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineBankLocal.Location = new System.Drawing.Point(400, 254);
		this.btnDefineBankLocal.Name = "btnDefineBankLocal";
		this.btnDefineBankLocal.Size = new System.Drawing.Size(26, 26);
		this.btnDefineBankLocal.TabIndex = 13;
		this.btnDefineBankLocal.TabStop = false;
		this.btnDefineBankLocal.UseVisualStyleBackColor = true;
		this.btnDefineBankLocal.Click += new System.EventHandler(btnDefineBankLocal_Click);
		this.btnDefineBankForeign.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineBankForeign.Location = new System.Drawing.Point(400, 227);
		this.btnDefineBankForeign.Name = "btnDefineBankForeign";
		this.btnDefineBankForeign.Size = new System.Drawing.Size(26, 26);
		this.btnDefineBankForeign.TabIndex = 11;
		this.btnDefineBankForeign.TabStop = false;
		this.btnDefineBankForeign.UseVisualStyleBackColor = true;
		this.btnDefineBankForeign.Click += new System.EventHandler(btnDefineBankForeign_Click);
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(774, 67);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(151, 23);
		this.txtExchangeRate.TabIndex = 27;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFCYAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFCYAmount.Location = new System.Drawing.Point(774, 94);
		this.txtFCYAmount.Name = "txtFCYAmount";
		this.txtFCYAmount.ReadOnly = true;
		this.txtFCYAmount.Size = new System.Drawing.Size(151, 23);
		this.txtFCYAmount.TabIndex = 28;
		this.txtFCYAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMTon.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMTon.Location = new System.Drawing.Point(520, 256);
		this.txtMTon.Name = "txtMTon";
		this.txtMTon.ReadOnly = true;
		this.txtMTon.Size = new System.Drawing.Size(150, 23);
		this.txtMTon.TabIndex = 24;
		this.txtNoOfContainers.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainers.Location = new System.Drawing.Point(774, 14);
		this.txtNoOfContainers.Name = "txtNoOfContainers";
		this.txtNoOfContainers.Size = new System.Drawing.Size(151, 23);
		this.txtNoOfContainers.TabIndex = 25;
		this.txtNoOfContainers.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfContainers_KeyPress);
		this.CmbFCYCode.AllowNull = (DefaultableBoolean)1;
		this.CmbFCYCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFCYCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFCYCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFCYCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Location = new System.Drawing.Point(774, 38);
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Name = "CmbFCYCode";
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).TabIndex = 26;
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Leave += new System.EventHandler(CmbFCYCode_TextChanged);
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(774, 119);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 29;
		this.txtDeliveryDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryDays.Location = new System.Drawing.Point(520, 40);
		this.txtDeliveryDays.Name = "txtDeliveryDays";
		this.txtDeliveryDays.Size = new System.Drawing.Size(150, 23);
		this.txtDeliveryDays.TabIndex = 17;
		this.txtDeliveryDays.TextChanged += new System.EventHandler(txtDeliveryDays_TextChanged);
		this.txtDeliveryDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeliveryDays_KeyPress);
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance282).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(520, 227);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 23;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TextChanged += new System.EventHandler(CmbDeliveryTerm_TextChanged);
		this.txtSpecialInstructions.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSpecialInstructions.Location = new System.Drawing.Point(928, 38);
		this.txtSpecialInstructions.Multiline = true;
		this.txtSpecialInstructions.Name = "txtSpecialInstructions";
		this.txtSpecialInstructions.Size = new System.Drawing.Size(277, 294);
		this.txtSpecialInstructions.TabIndex = 35;
		this.txtSpecialInstructions.Leave += new System.EventHandler(txtSpecialInstructions_Enter);
		this.txtCommodityDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommodityDetail.Location = new System.Drawing.Point(93, 309);
		this.txtCommodityDetail.Name = "txtCommodityDetail";
		this.txtCommodityDetail.Size = new System.Drawing.Size(832, 23);
		this.txtCommodityDetail.TabIndex = 15;
		this.CmbExporterbank.AllowNull = (DefaultableBoolean)1;
		this.CmbExporterbank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExporterbank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExporterbank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExporterbank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Location = new System.Drawing.Point(93, 254);
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Name = "CmbExporterbank";
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Size = new System.Drawing.Size(307, 26);
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).TabIndex = 12;
		this.label99.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label99.ForeColor = System.Drawing.Color.Red;
		this.label99.Location = new System.Drawing.Point(508, 181);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(10, 10);
		this.label99.TabIndex = 585;
		this.label99.Text = "*";
		this.label98.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.ForeColor = System.Drawing.Color.Red;
		this.label98.Location = new System.Drawing.Point(508, 154);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(10, 10);
		this.label98.TabIndex = 584;
		this.label98.Text = "*";
		this.label97.AutoSize = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label97.ForeColor = System.Drawing.Color.Red;
		this.label97.Location = new System.Drawing.Point(508, 43);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(14, 17);
		this.label97.TabIndex = 583;
		this.label97.Text = "*";
		this.label96.AutoSize = true;
		this.label96.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label96.ForeColor = System.Drawing.Color.Red;
		this.label96.Location = new System.Drawing.Point(508, 232);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(14, 17);
		this.label96.TabIndex = 582;
		this.label96.Text = "*";
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.ForeColor = System.Drawing.Color.Red;
		this.label94.Location = new System.Drawing.Point(754, 97);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(14, 17);
		this.label94.TabIndex = 580;
		this.label94.Text = "*";
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label93.ForeColor = System.Drawing.Color.Red;
		this.label93.Location = new System.Drawing.Point(754, 70);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(14, 17);
		this.label93.TabIndex = 579;
		this.label93.Text = "*";
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.Red;
		this.label92.Location = new System.Drawing.Point(754, 43);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(14, 17);
		this.label92.TabIndex = 578;
		this.label92.Text = "*";
		this.label88.AutoSize = true;
		this.label88.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label88.ForeColor = System.Drawing.Color.Red;
		this.label88.Location = new System.Drawing.Point(506, 259);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(14, 17);
		this.label88.TabIndex = 577;
		this.label88.Text = "*";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.ForeColor = System.Drawing.Color.Red;
		this.label86.Location = new System.Drawing.Point(79, 124);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(14, 17);
		this.label86.TabIndex = 575;
		this.label86.Text = "*";
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.ForeColor = System.Drawing.Color.Red;
		this.label84.Location = new System.Drawing.Point(79, 70);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(14, 17);
		this.label84.TabIndex = 573;
		this.label84.Text = "*";
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.ForeColor = System.Drawing.Color.Red;
		this.label83.Location = new System.Drawing.Point(79, 43);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(14, 17);
		this.label83.TabIndex = 572;
		this.label83.Text = "*";
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.ForeColor = System.Drawing.Color.Red;
		this.label82.Location = new System.Drawing.Point(79, 17);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(14, 17);
		this.label82.TabIndex = 571;
		this.label82.Text = "*";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(674, 71);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(84, 15);
		this.label40.TabIndex = 562;
		this.label40.Text = "Exchange Rate";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 287);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(79, 15);
		this.label12.TabIndex = 497;
		this.label12.Text = "Specification ";
		this.label95.AutoSize = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.Location = new System.Drawing.Point(2, 98);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(62, 15);
		this.label95.TabIndex = 459;
		this.label95.Text = "Consignee";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(2, 71);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 15);
		this.label6.TabIndex = 459;
		this.label6.Text = "Customer";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(431, 44);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(79, 15);
		this.label42.TabIndex = 566;
		this.label42.Text = "Delivery Days";
		this.txtPMScheduleDate.CustomFormat = "dd-MMM-yyyy";
		this.txtPMScheduleDate.Enabled = false;
		this.txtPMScheduleDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPMScheduleDate.Location = new System.Drawing.Point(774, 202);
		this.txtPMScheduleDate.Name = "txtPMScheduleDate";
		this.txtPMScheduleDate.Size = new System.Drawing.Size(151, 23);
		this.txtPMScheduleDate.TabIndex = 32;
		this.labelDCG.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.labelDCG.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.labelDCG.Location = new System.Drawing.Point(674, 258);
		this.labelDCG.Name = "labelDCG";
		this.labelDCG.Size = new System.Drawing.Size(96, 18);
		this.labelDCG.TabIndex = 564;
		this.labelDCG.Text = "Document Custom Group";
		this.labelDCG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label72.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label72.Location = new System.Drawing.Point(674, 204);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(96, 18);
		this.label72.TabIndex = 564;
		this.label72.Text = "PM Schedule";
		this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtInspectionScheduleDate.CustomFormat = "dd-MMM-yyyy";
		this.txtInspectionScheduleDate.Enabled = false;
		this.txtInspectionScheduleDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectionScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtInspectionScheduleDate.Location = new System.Drawing.Point(774, 229);
		this.txtInspectionScheduleDate.Name = "txtInspectionScheduleDate";
		this.txtInspectionScheduleDate.Size = new System.Drawing.Size(151, 23);
		this.txtInspectionScheduleDate.TabIndex = 33;
		this.label71.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label71.Location = new System.Drawing.Point(674, 225);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(96, 30);
		this.label71.TabIndex = 564;
		this.label71.Text = "Inspection Schedule";
		this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtProductionScheduleDate.CustomFormat = "dd-MMM-yyyy";
		this.txtProductionScheduleDate.Enabled = false;
		this.txtProductionScheduleDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtProductionScheduleDate.Location = new System.Drawing.Point(774, 175);
		this.txtProductionScheduleDate.Name = "txtProductionScheduleDate";
		this.txtProductionScheduleDate.Size = new System.Drawing.Size(151, 23);
		this.txtProductionScheduleDate.TabIndex = 31;
		this.label70.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label70.Location = new System.Drawing.Point(674, 171);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(96, 30);
		this.label70.TabIndex = 564;
		this.label70.Text = "Production Schedule";
		this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtFactoryLoadingDate.CustomFormat = "dd-MMM-yyyy";
		this.txtFactoryLoadingDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryLoadingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFactoryLoadingDate.Location = new System.Drawing.Point(774, 148);
		this.txtFactoryLoadingDate.Name = "txtFactoryLoadingDate";
		this.txtFactoryLoadingDate.Size = new System.Drawing.Size(151, 23);
		this.txtFactoryLoadingDate.TabIndex = 30;
		this.txtFactoryLoadingDate.ValueChanged += new System.EventHandler(txtFactoryLoadingDate_ValueChanged);
		this.label68.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label68.Location = new System.Drawing.Point(674, 150);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(96, 18);
		this.label68.TabIndex = 564;
		this.label68.Text = "Factory Loading";
		this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.datShipmentStartDate.CustomFormat = "MMM-yyyy";
		this.datShipmentStartDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datShipmentStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datShipmentStartDate.Location = new System.Drawing.Point(520, 14);
		this.datShipmentStartDate.Name = "datShipmentStartDate";
		this.datShipmentStartDate.Size = new System.Drawing.Size(150, 23);
		this.datShipmentStartDate.TabIndex = 16;
		this.datShipmentStartDate.ValueChanged += new System.EventHandler(datShipmentStartDate_ValueChanged);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(431, 18);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(90, 15);
		this.label41.TabIndex = 564;
		this.label41.Text = "Ship. Start Date";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(431, 98);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(66, 15);
		this.label26.TabIndex = 489;
		this.label26.Text = "Lot Ref No.";
		this.label51.BackColor = System.Drawing.Color.Gainsboro;
		this.label51.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(928, 14);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(277, 23);
		this.label51.TabIndex = 460;
		this.label51.Text = "Special Instructions";
		this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.BackColor = System.Drawing.Color.Transparent;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 305);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(79, 31);
		this.label8.TabIndex = 460;
		this.label8.Text = "Commodity Detail";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(674, 98);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(74, 15);
		this.label21.TabIndex = 461;
		this.label21.Text = "FCY Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(221, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(80, 15);
		this.label3.TabIndex = 463;
		this.label3.Text = "Contract Date";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(674, 18);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(63, 15);
		this.label32.TabIndex = 495;
		this.label32.Text = "Containers";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(431, 260);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(41, 15);
		this.label22.TabIndex = 457;
		this.label22.Text = "M.Ton";
		this.txtCustomerOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerOrderNo.Location = new System.Drawing.Point(93, 121);
		this.txtCustomerOrderNo.Name = "txtCustomerOrderNo";
		this.txtCustomerOrderNo.Size = new System.Drawing.Size(125, 23);
		this.txtCustomerOrderNo.TabIndex = 5;
		this.txtProductSpecification.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductSpecification.Location = new System.Drawing.Point(93, 283);
		this.txtProductSpecification.Name = "txtProductSpecification";
		this.txtProductSpecification.Size = new System.Drawing.Size(832, 23);
		this.txtProductSpecification.TabIndex = 14;
		this.txtLotRefNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLotRefNo.Location = new System.Drawing.Point(520, 92);
		this.txtLotRefNo.Multiline = true;
		this.txtLotRefNo.Name = "txtLotRefNo";
		this.txtLotRefNo.Size = new System.Drawing.Size(150, 51);
		this.txtLotRefNo.TabIndex = 19;
		this.txtLotRefNo.TabIndexChanged += new System.EventHandler(txtLotRefNo_Enter);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(674, 44);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(55, 15);
		this.label20.TabIndex = 458;
		this.label20.Text = "Fcy Code";
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(221, 117);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(80, 30);
		this.label11.TabIndex = 491;
		this.label11.Text = "Customer Order Date";
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(2, 117);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(68, 30);
		this.label27.TabIndex = 491;
		this.label27.Text = "Customer Order No";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(2, 44);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(73, 15);
		this.label28.TabIndex = 465;
		this.label28.Text = "Sales Person";
		this.ApprovalDate.CustomFormat = "dd-MMM-yyyy";
		this.ApprovalDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovalDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ApprovalDate.Location = new System.Drawing.Point(520, 202);
		this.ApprovalDate.Name = "ApprovalDate";
		this.ApprovalDate.Size = new System.Drawing.Size(150, 23);
		this.ApprovalDate.TabIndex = 22;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(2, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 464;
		this.label4.Text = "Contract No";
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.White;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(674, 125);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(88, 15);
		this.label15.TabIndex = 503;
		this.label15.Text = "Contract Status";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.White;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(431, 206);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(83, 15);
		this.label43.TabIndex = 568;
		this.label43.Text = "Approval Date";
		this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtCustomerOrderDate.CustomFormat = "dd-MMM-yyyy";
		this.txtCustomerOrderDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtCustomerOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtCustomerOrderDate.Location = new System.Drawing.Point(301, 121);
		this.txtCustomerOrderDate.Name = "txtCustomerOrderDate";
		this.txtCustomerOrderDate.Size = new System.Drawing.Size(125, 22);
		this.txtCustomerOrderDate.TabIndex = 6;
		this.txtCustomerOrderDate.ValueChanged += new System.EventHandler(datOrderDate_ValueChanged);
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(301, 14);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(125, 22);
		this.txtDocDate.TabIndex = 1;
		this.txtDocDate.ValueChanged += new System.EventHandler(txtDocDate_ValueChanged);
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
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty3).Location = new System.Drawing.Point(93, 200);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty3).Name = "CmbNotifyParty3";
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty3).Size = new System.Drawing.Size(333, 26);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty3).TabIndex = 9;
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance321).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance322).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance322).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance322;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance324;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance325).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance325).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance325).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance325;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance326).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance326;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance327).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance328;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance329).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance330).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance330).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance330).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance330).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance330;
		((AppearanceBase)appearance331).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance332).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance333;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(93, 65);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(333, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Leave += new System.EventHandler(CmbCustomer_Leave);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(2, 206);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(80, 15);
		this.label49.TabIndex = 466;
		this.label49.Text = "Notify Party 3";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(2, 179);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(80, 15);
		this.label7.TabIndex = 466;
		this.label7.Text = "Notify Party 2";
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
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Location = new System.Drawing.Point(93, 38);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Name = "CmbSalePerson";
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Size = new System.Drawing.Size(333, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(2, 152);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(78, 15);
		this.label9.TabIndex = 466;
		this.label9.Text = "Notify Party 1";
		this.txtShipmentDate.CustomFormat = "dd-MMM-yyyy";
		this.txtShipmentDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShipmentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtShipmentDate.Location = new System.Drawing.Point(520, 67);
		this.txtShipmentDate.Name = "txtShipmentDate";
		this.txtShipmentDate.Size = new System.Drawing.Size(150, 23);
		this.txtShipmentDate.TabIndex = 18;
		this.txtShipmentDate.ValueChanged += new System.EventHandler(txtShipmentDate_ValueChanged);
		this.CmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.CmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Location = new System.Drawing.Point(93, 227);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Name = "CmbImporterBank";
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Size = new System.Drawing.Size(307, 26);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).TabIndex = 10;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(431, 71);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(83, 15);
		this.label29.TabIndex = 493;
		this.label29.Text = "Ship.Last Date";
		this.CmbDocumentCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocumentCustomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentCustomGroup).Location = new System.Drawing.Point(774, 254);
		((System.Windows.Forms.Control)(object)this.CmbDocumentCustomGroup).Name = "CmbDocumentCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbDocumentCustomGroup).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentCustomGroup).TabIndex = 34;
		this.CmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance254;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Location = new System.Drawing.Point(520, 146);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Name = "CmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).TabIndex = 20;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(2, 233);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(83, 15);
		this.label18.TabIndex = 505;
		this.label18.Text = "Importer Bank";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(2, 260);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(80, 15);
		this.label19.TabIndex = 506;
		this.label19.Text = "Exporter Bank";
		this.CmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.CmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Location = new System.Drawing.Point(520, 173);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Name = "CmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).TabIndex = 21;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(431, 233);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(80, 15);
		this.label5.TabIndex = 499;
		this.label5.Text = "Delivery Term";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(431, 152);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(75, 15);
		this.label14.TabIndex = 501;
		this.label14.Text = "Loading Port";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(431, 179);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(68, 15);
		this.label17.TabIndex = 504;
		this.label17.Text = "Destination";
		this.panel28.BackColor = System.Drawing.Color.Teal;
		this.panel28.Controls.Add(this.AdvanceDoFormLink);
		this.panel28.Controls.Add(this.chkOpenContractSchedule);
		this.panel28.Controls.Add(this.ChkPreview);
		this.panel28.Controls.Add(this.label66);
		this.panel28.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExportCompany);
		this.panel28.Controls.Add(this.labelExportCompany);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel28.Location = new System.Drawing.Point(0, 0);
		this.panel28.Margin = new System.Windows.Forms.Padding(2);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1220, 26);
		this.panel28.TabIndex = 1;
		this.AdvanceDoFormLink.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.AdvanceDoFormLink.AutoSize = true;
		this.AdvanceDoFormLink.Location = new System.Drawing.Point(994, 4);
		this.AdvanceDoFormLink.Name = "AdvanceDoFormLink";
		this.AdvanceDoFormLink.Size = new System.Drawing.Size(130, 16);
		this.AdvanceDoFormLink.TabIndex = 563;
		this.AdvanceDoFormLink.TabStop = true;
		this.AdvanceDoFormLink.Text = "Contract Schedule";
		this.AdvanceDoFormLink.Click += new System.EventHandler(AdvanceDoFormLink_Click);
		this.chkOpenContractSchedule.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkOpenContractSchedule.AutoSize = true;
		this.chkOpenContractSchedule.Checked = true;
		this.chkOpenContractSchedule.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkOpenContractSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.chkOpenContractSchedule.ForeColor = System.Drawing.Color.Blue;
		this.chkOpenContractSchedule.Location = new System.Drawing.Point(979, 7);
		this.chkOpenContractSchedule.Name = "chkOpenContractSchedule";
		this.chkOpenContractSchedule.Size = new System.Drawing.Size(15, 14);
		this.chkOpenContractSchedule.TabIndex = 562;
		this.chkOpenContractSchedule.UseVisualStyleBackColor = true;
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(1133, 3);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPreview.TabIndex = 1;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.White;
		this.label66.Location = new System.Drawing.Point(3, 3);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(186, 19);
		this.label66.TabIndex = 561;
		this.label66.Text = "Export Sales Contract";
		this.CmbExportCompany.AllowNull = (DefaultableBoolean)1;
		this.CmbExportCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExportCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExportCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance286).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance286).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance286;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance287;
		((SpecialBoxBase)((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance288).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance288).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance291).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance293).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance294).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance294).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance294).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExportCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance294;
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
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).Location = new System.Drawing.Point(319, 0);
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).Name = "CmbExportCompany";
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).Size = new System.Drawing.Size(259, 26);
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbExportCompany).TextChanged += new System.EventHandler(CmbDeliveryTerm_TextChanged);
		this.labelExportCompany.AutoSize = true;
		this.labelExportCompany.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelExportCompany.ForeColor = System.Drawing.Color.White;
		this.labelExportCompany.Location = new System.Drawing.Point(224, 6);
		this.labelExportCompany.Name = "labelExportCompany";
		this.labelExportCompany.Size = new System.Drawing.Size(98, 15);
		this.labelExportCompany.TabIndex = 499;
		this.labelExportCompany.Text = "Export Company";
		this.txtApprovalRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovalRemarks.Location = new System.Drawing.Point(1167, 370);
		this.txtApprovalRemarks.Name = "txtApprovalRemarks";
		this.txtApprovalRemarks.Size = new System.Drawing.Size(42, 23);
		this.txtApprovalRemarks.TabIndex = 20;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1074, 374);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(52, 15);
		this.label44.TabIndex = 570;
		this.label44.Text = "Remarks";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(1074, 401);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(52, 15);
		this.label50.TabIndex = 570;
		this.label50.Text = "Remarks";
		this.txtDeliveryRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryRemarks.Location = new System.Drawing.Point(1167, 397);
		this.txtDeliveryRemarks.Name = "txtDeliveryRemarks";
		this.txtDeliveryRemarks.Size = new System.Drawing.Size(42, 23);
		this.txtDeliveryRemarks.TabIndex = 22;
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1220, 29);
		this.panel3.TabIndex = 0;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[17]
		{
			this.btnnew, this.BtnRefresh, this.BtnSaveAs, this.btnsave, this.btnUpdate, this.BtnDelete, this.btnAttachment, this.BtnPrintII, this.Print, this.btnGenerateInvoiceNos,
			this.BtnPackingTypeDefine, this.BtnDeliveryTermLookUp, this.btnDefinePaymentTerm, this.btnSeaPortDefine, this.BtnContractPmDetailForm, this.BtnCommodityDetailForm, this.btnShortCutKey
		});
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolstrip.Size = new System.Drawing.Size(1220, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveAs.Image = (System.Drawing.Image)resources.GetObject("BtnSaveAs.Image");
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(85, 24);
		this.BtnSaveAs.Text = "&Save As";
		this.BtnSaveAs.Visible = false;
		this.BtnSaveAs.Click += new System.EventHandler(BtnSaveAs_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(73, 24);
		this.BtnDelete.Text = "Delete";
		this.BtnDelete.Visible = false;
		this.BtnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.BtnPrintII.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintII.Name = "BtnPrintII";
		this.BtnPrintII.Size = new System.Drawing.Size(101, 24);
		this.BtnPrintII.Text = "523A-Print";
		this.BtnPrintII.Click += new System.EventHandler(BtnPrintII_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "501-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnGenerateInvoiceNos.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateInvoiceNos.Name = "btnGenerateInvoiceNos";
		this.btnGenerateInvoiceNos.Size = new System.Drawing.Size(163, 24);
		this.btnGenerateInvoiceNos.Text = "Generate Contract Nos";
		this.btnGenerateInvoiceNos.Click += new System.EventHandler(btnGenerateInvoiceNos_Click);
		this.BtnPackingTypeDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPackingTypeDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPackingTypeDefine.Name = "BtnPackingTypeDefine";
		this.BtnPackingTypeDefine.Size = new System.Drawing.Size(98, 24);
		this.BtnPackingTypeDefine.Text = "Packing Type";
		this.BtnPackingTypeDefine.Click += new System.EventHandler(BtnPackingTypeDefine_Click);
		this.BtnDeliveryTermLookUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDeliveryTermLookUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDeliveryTermLookUp.Name = "BtnDeliveryTermLookUp";
		this.BtnDeliveryTermLookUp.Size = new System.Drawing.Size(100, 24);
		this.BtnDeliveryTermLookUp.Text = "Delivery Term";
		this.BtnDeliveryTermLookUp.Click += new System.EventHandler(btnDeliveryTerm_Click);
		this.btnDefinePaymentTerm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefinePaymentTerm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefinePaymentTerm.Name = "btnDefinePaymentTerm";
		this.btnDefinePaymentTerm.Size = new System.Drawing.Size(105, 24);
		this.btnDefinePaymentTerm.Text = "Payment Term";
		this.btnDefinePaymentTerm.Click += new System.EventHandler(btnPaymentTerms_Click);
		this.btnSeaPortDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSeaPortDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSeaPortDefine.Name = "btnSeaPortDefine";
		this.btnSeaPortDefine.Size = new System.Drawing.Size(68, 24);
		this.btnSeaPortDefine.Text = "Sea Port";
		this.btnSeaPortDefine.Click += new System.EventHandler(btnSeaPortDefine_Click);
		this.BtnContractPmDetailForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnContractPmDetailForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnContractPmDetailForm.Image = (System.Drawing.Image)resources.GetObject("BtnContractPmDetailForm.Image");
		this.BtnContractPmDetailForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnContractPmDetailForm.Name = "BtnContractPmDetailForm";
		this.BtnContractPmDetailForm.Size = new System.Drawing.Size(172, 20);
		this.BtnContractPmDetailForm.Text = "Contract Pm Detail Form";
		this.BtnContractPmDetailForm.Click += new System.EventHandler(BtnContractPmDetailForm_Click);
		this.BtnCommodityDetailForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnCommodityDetailForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCommodityDetailForm.Image = (System.Drawing.Image)resources.GetObject("BtnCommodityDetailForm.Image");
		this.BtnCommodityDetailForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnCommodityDetailForm.Name = "BtnCommodityDetailForm";
		this.BtnCommodityDetailForm.Size = new System.Drawing.Size(162, 20);
		this.BtnCommodityDetailForm.Text = "Commodity Detail Form";
		this.BtnCommodityDetailForm.Click += new System.EventHandler(BtnCommodityDetailForm_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.tabControl3);
		this.tabPage2.Controls.Add(this.panel29);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1226, 732);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 152);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1220, 320);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.tabControl3.Controls.Add(this.tabDetailH);
		this.tabControl3.Controls.Add(this.tabPaymentDetailH);
		this.tabControl3.Controls.Add(this.tabPmH);
		this.tabControl3.Controls.Add(this.tabOtherItemH);
		this.tabControl3.Controls.Add(this.tabOtherchargesHistory);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(3, 472);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1220, 257);
		this.tabControl3.TabIndex = 349;
		this.tabDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailhistory);
		this.tabDetailH.Controls.Add(this.panel13);
		this.tabDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabDetailH.Name = "tabDetailH";
		this.tabDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetailH.Size = new System.Drawing.Size(1212, 228);
		this.tabDetailH.TabIndex = 0;
		this.tabDetailH.Text = "Detail";
		this.tabDetailH.UseVisualStyleBackColor = true;
		this.grddetailhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grddetailhistory).BackColor = System.Drawing.Color.White;
		this.grddetailhistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grddetailhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailhistory.DesignTimeLayout = grddetailhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailhistory.FilterMode = (FilterMode)2;
		this.grddetailhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailhistory.GroupByBoxVisible = false;
		this.grddetailhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Name = "grddetailhistory";
		((ScrollableBase)this.grddetailhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Size = new System.Drawing.Size(1206, 197);
		((System.Windows.Forms.Control)(object)this.grddetailhistory).TabIndex = 2;
		this.grddetailhistory.TotalRow = (InheritableBoolean)1;
		this.grddetailhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailhistory).VisualStyle = (VisualStyle)4;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label59);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1206, 25);
		this.panel13.TabIndex = 346;
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
		this.tabPaymentDetailH.Size = new System.Drawing.Size(1212, 228);
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
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Size = new System.Drawing.Size(1206, 197);
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).TabIndex = 347;
		this.grdPaymentHistory.TotalRow = (InheritableBoolean)1;
		this.grdPaymentHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentHistory).VisualStyle = (VisualStyle)4;
		this.panel31.BackColor = System.Drawing.Color.Teal;
		this.panel31.Controls.Add(this.label74);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel31.Location = new System.Drawing.Point(3, 3);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1206, 25);
		this.panel31.TabIndex = 348;
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label74.ForeColor = System.Drawing.Color.White;
		this.label74.Location = new System.Drawing.Point(5, 5);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(301, 17);
		this.label74.TabIndex = 562;
		this.label74.Text = "Payement Detail of the selected above row";
		this.tabPmH.Controls.Add((System.Windows.Forms.Control)(object)this.grdPMHistory);
		this.tabPmH.Controls.Add(this.panel32);
		this.tabPmH.Location = new System.Drawing.Point(4, 25);
		this.tabPmH.Name = "tabPmH";
		this.tabPmH.Size = new System.Drawing.Size(1212, 228);
		this.tabPmH.TabIndex = 2;
		this.tabPmH.Text = "Packing Material";
		this.tabPmH.UseVisualStyleBackColor = true;
		this.grdPMHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPMHistory).BackColor = System.Drawing.Color.White;
		this.grdPMHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPMHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPMHistory.DesignTimeLayout = grdPMHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPMHistory.FilterMode = (FilterMode)2;
		this.grdPMHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPMHistory.GroupByBoxVisible = false;
		this.grdPMHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Name = "grdPMHistory";
		((ScrollableBase)this.grdPMHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPMHistory).Size = new System.Drawing.Size(1212, 203);
		((System.Windows.Forms.Control)(object)this.grdPMHistory).TabIndex = 347;
		this.grdPMHistory.TotalRow = (InheritableBoolean)1;
		this.grdPMHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPMHistory).VisualStyle = (VisualStyle)4;
		this.panel32.BackColor = System.Drawing.Color.Teal;
		this.panel32.Controls.Add(this.label75);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(0, 0);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1212, 25);
		this.panel32.TabIndex = 348;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label75.ForeColor = System.Drawing.Color.White;
		this.label75.Location = new System.Drawing.Point(5, 5);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(343, 17);
		this.label75.TabIndex = 562;
		this.label75.Text = "Packing Material Detail of the selected above row";
		this.tabOtherItemH.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherItemHistory);
		this.tabOtherItemH.Controls.Add(this.panel33);
		this.tabOtherItemH.Location = new System.Drawing.Point(4, 25);
		this.tabOtherItemH.Name = "tabOtherItemH";
		this.tabOtherItemH.Size = new System.Drawing.Size(1212, 228);
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
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).Size = new System.Drawing.Size(1212, 203);
		((System.Windows.Forms.Control)(object)this.grdOtherItemHistory).TabIndex = 347;
		this.grdOtherItemHistory.TotalRow = (InheritableBoolean)1;
		this.grdOtherItemHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherItemHistory).VisualStyle = (VisualStyle)4;
		this.panel33.BackColor = System.Drawing.Color.Teal;
		this.panel33.Controls.Add(this.label76);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel33.Location = new System.Drawing.Point(0, 0);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(1212, 25);
		this.panel33.TabIndex = 348;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label76.ForeColor = System.Drawing.Color.White;
		this.label76.Location = new System.Drawing.Point(5, 5);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(310, 17);
		this.label76.TabIndex = 562;
		this.label76.Text = "Other Item Detail of the selected above row";
		this.tabOtherchargesHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory);
		this.tabOtherchargesHistory.Controls.Add(this.panel39);
		this.tabOtherchargesHistory.Location = new System.Drawing.Point(4, 25);
		this.tabOtherchargesHistory.Name = "tabOtherchargesHistory";
		this.tabOtherchargesHistory.Size = new System.Drawing.Size(1212, 228);
		this.tabOtherchargesHistory.TabIndex = 4;
		this.tabOtherchargesHistory.Text = "Other Charges";
		this.tabOtherchargesHistory.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).Size = new System.Drawing.Size(1212, 203);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).TabIndex = 349;
		this.grdOtherChargesHistory.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesHistory).VisualStyle = (VisualStyle)4;
		this.panel39.BackColor = System.Drawing.Color.Teal;
		this.panel39.Controls.Add(this.label87);
		this.panel39.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel39.Location = new System.Drawing.Point(0, 0);
		this.panel39.Name = "panel39";
		this.panel39.Size = new System.Drawing.Size(1212, 25);
		this.panel39.TabIndex = 350;
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label87.ForeColor = System.Drawing.Color.White;
		this.label87.Location = new System.Drawing.Point(5, 5);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(333, 17);
		this.label87.TabIndex = 562;
		this.label87.Text = "Other Charges Detail of the selected above row";
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.label65);
		this.panel29.Controls.Add(this.ctrlGrdBar1);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(3, 128);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1220, 24);
		this.panel29.TabIndex = 348;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.White;
		this.label65.Location = new System.Drawing.Point(4, 2);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(142, 19);
		this.label65.TabIndex = 562;
		this.label65.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1169, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(48, 23);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel18.Controls.Add(this.groupBox5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 56);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1220, 72);
		this.panel18.TabIndex = 0;
		this.groupBox5.Controls.Add(this.rdapproveddate);
		this.groupBox5.Controls.Add(this.btnShowHistory);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.label67);
		this.groupBox5.Controls.Add(this.drdocdate);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.label73);
		this.groupBox5.Controls.Add(this.label89);
		this.groupBox5.Controls.Add(this.label90);
		this.groupBox5.Controls.Add(this.label91);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox5.Controls.Add(this.ToDocNoHistory);
		this.groupBox5.Controls.Add(this.FromDocNoHistory);
		this.groupBox5.Controls.Add(this.ToDateHistory);
		this.groupBox5.Controls.Add(this.FromDateHistory);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(4, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(923, 71);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(668, 40);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(373, 20);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(58, 15);
		this.label67.TabIndex = 87;
		this.label67.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(735, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(735, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 46);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(64, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(213, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(79, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 46);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance384).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance384;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance385).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance385).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance385).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance385).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance385;
		((AppearanceBase)appearance386).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance386;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance388).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance388).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance388).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance388;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance390).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance390).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance390;
		((AppearanceBase)appearance400).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance400).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance400;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance406).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance406;
		((AppearanceBase)appearance407).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance407).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance407;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance408).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance408).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance408).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance408).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance408).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance408;
		((AppearanceBase)appearance409).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance409;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance410).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance410).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance410;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance411).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance411;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(434, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 42);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(294, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 42);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label52);
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 3);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1220, 53);
		this.panel17.TabIndex = 0;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(5, 30);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(249, 19);
		this.label52.TabIndex = 562;
		this.label52.Text = "Export Sales Contract History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(1220, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1234, 761);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmExportContractIII";
		this.Text = "Export Contract";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmExportContractIII_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportContractIII_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabContractDetail.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetails).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInnerUomInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPmItemInDetail).EndInit();
		this.panel36.ResumeLayout(false);
		this.panel36.PerformLayout();
		this.panel35.ResumeLayout(false);
		this.panel35.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCategoryInDetail).EndInit();
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingNTradeInDetail).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPmDetailByItemCusInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbComditiyDetailByItemCusInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOuterUomInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearInDetail).EndInit();
		this.tabPaymentTerm.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPT).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tabPackingMaterialDetail.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel25.ResumeLayout(false);
		this.panel27.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialDetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandPmDetail).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUomPmDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPmDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandPackingTypePmDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemPmDetail).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.tabOtherItems.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOtherDetail).EndInit();
		this.tabOtherCharges.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetail).EndInit();
		this.panel37.ResumeLayout(false);
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesNameChargeDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbConsignee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFCYCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterbank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).EndInit();
		this.panel28.ResumeLayout(false);
		this.panel28.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportCompany).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetailhistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.tabPaymentDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentHistory).EndInit();
		this.panel31.ResumeLayout(false);
		this.panel31.PerformLayout();
		this.tabPmH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPMHistory).EndInit();
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		this.tabOtherItemH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherItemHistory).EndInit();
		this.panel33.ResumeLayout(false);
		this.panel33.PerformLayout();
		this.tabOtherchargesHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesHistory).EndInit();
		this.panel39.ResumeLayout(false);
		this.panel39.PerformLayout();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
