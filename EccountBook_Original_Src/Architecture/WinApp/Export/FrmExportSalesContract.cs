using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.MRP;
using Architecture.BLL.Reports.Export;
using Architecture.BLL.SDT;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class FrmExportSalesContract : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtPaymnent = new DataTable();

	private DataTable dtPmDetail = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtOtherChargesDetail = new DataTable();

	private DataTable dtLastSavedRecordHeader = new DataTable();

	private DataTable dtLastSavedRecordByItemId = new DataTable();

	private DataTable dtItemAndPMItemMap = new DataTable();

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int ItemIdOfPreviousRate;

	private int updateDetailIndex = -1;

	private int updateDetailIndexOther = -1;

	private int updateDetailIndexPaymentDetail = -1;

	private int updateDetailIndexPmDetail = -1;

	private int updateDetailIndexOtherCharges = -1;

	private bool FlagFin;

	private bool AllItemsBindonExportContract;

	private bool ItemSearchWithNameOrCode;

	private bool PMGridInfoShouldNotCompulsoryOnContract;

	private bool AllowExportMultiCompanies;

	private bool ShipmentDocumentsFeature;

	private string CustomAttachmentsValues = "";

	private string AttachmentsValues = "";

	private bool FormInitialized;

	public DataTable dtGPGRNS = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtPMItem = new DataTable();

	private DataTable dtBrandForPMItem = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtpcktype = new DataTable();

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

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Label label16;

	private TextBox txtQtyMTon;

	private Label label13;

	internal UltraCombo combrateuom;

	private Label label10;

	internal UltraCombo combpcktype;

	private Label label2;

	internal UltraCombo combitempck;

	private Label label1;

	internal UltraCombo combitem;

	private TextBox txtExportSoNo;

	internal UltraCombo CmbFCYCode;

	private Label label22;

	private Label label20;

	private Label label9;

	private Label label6;

	private Label label8;

	private Label label21;

	private Label label11;

	private Label label3;

	private Label label4;

	private Label label28;

	internal UltraCombo CmbExporterbank;

	internal UltraCombo CmbImporterBank;

	internal UltraCombo CmbDestinationPort;

	internal UltraCombo CmbLoadingPort;

	internal UltraCombo CmbPaymentTerms;

	internal UltraCombo CmbDeliveryTerm;

	private DateTimePicker datOrderDate;

	private DateTimePicker txtDocDate;

	internal UltraCombo CmbNotifyParty;

	internal UltraCombo CmbCustomer;

	internal UltraCombo CmbSalePerson;

	private TextBox txtMTon;

	private TextBox txtFCYAmount;

	private Label label29;

	private DateTimePicker txtShipmentDate;

	private TextBox txtBuyerOrderNo;

	private Label label27;

	private TextBox txtLotRefNo;

	private Label label26;

	private Label label32;

	private TextBox txtNoOfContainers;

	private Label label12;

	private TextBox txtNoOfBags;

	private Label label23;

	private TextBox txtCostMTon;

	private Label label25;

	private TextBox txtAmount;

	private Label label33;

	private TextBox txtRemarks;

	private Label label34;

	private ToolStripButton BtnRefresh;

	private ToolStripButton BtnDeliveryTermLookUp;

	private Label label14;

	private Label label7;

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

	internal UltraCombo CmbCropYear;

	private Label label24;

	private Panel panel7;

	private Panel panel6;

	internal GridEX grdDetails;

	private Panel panel9;

	private Panel panel8;

	internal UltraCombo CmbOtherItem;

	private Label label31;

	private TextBox txtoQty;

	private Label label35;

	private TextBox txtoRate;

	private Label label36;

	private Label label37;

	private TextBox txtoAmount;

	private Label label38;

	private TextBox txtoRemarks;

	private Button btnoCancel;

	private Button btnoUpdate;

	private Button btnoadd;

	private ToolStripButton btnSeaPortDefine;

	private CheckBox ChkOnCommission;

	private CtrlGrdBar ctrlGrdBar1;

	private TabPage tabPaymentTerm;

	private Panel panel10;

	private Panel panel11;

	private Panel panel12;

	private Label label54;

	internal UltraCombo CmbPaymentTermDetail;

	internal GridEX grdPaymentTerm;

	private Panel panel14;

	private TextBox PercntOfTotal;

	private Label label53;

	private Label label55;

	private TextBox txtPaymentDetailFcyAmount;

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

	internal UltraCombo CmbPMItem;

	private Label label64;

	private Label label63;

	private Label label62;

	private Label label61;

	private Label label60;

	private Label label58;

	internal UltraCombo CmbBrandPackingTypePm;

	private TextBox txtInnerQtyPm;

	private TextBox txtPackingAmount;

	private TextBox txtPackingItemRate;

	private TextBox txtPackingRemarks;

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

	internal UltraCombo cmbBrandForPmEntry;

	private Label label47;

	private RadioButton radBrandName;

	private RadioButton rabBrandName;

	private Panel panel30;

	private TextBox txtCommodityDetailIndetail;

	private Label label48;

	internal UltraCombo cmbcomditiyDetailByItemCus;

	private Label label39;

	private ToolStripButton BtnCommodityDetailForm;

	private GroupBox groupBox1;

	private RichTextBox txtCommodityDetail;

	private RichTextBox txtSpecialInstructions;

	private TextBox txtPaymentRemarks;

	private TextBox txtDeliveryRemarks;

	private Label label51;

	private TextBox txtApprovalRemarks;

	private TextBox txtProductSpecification;

	private Label label50;

	private Label label49;

	private TextBox txtPaymentRemarksDetail;

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

	internal UltraCombo CmbFarmingNTrade;

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

	private TextBox txtBrandInnerQtyPm;

	private TextBox txtBrandOuterQtyPm;

	internal UltraCombo CmbBrandUomPm;

	private Label label77;

	private Label label79;

	private Label label78;

	private TextBox txtOuterQtyPm;

	internal UltraCombo CmbUomPm;

	private Label label80;

	private Label label81;

	private ToolStripButton BtnContractPmDetailForm;

	private Label labelDCG;

	internal UltraCombo CmbDocumentCustomGroup;

	private ToolStripButton BtnPrintII;

	private Label label82;

	private Label label84;

	private Label label83;

	private Label label86;

	private Label label85;

	private Label label88;

	private Label label87;

	private Label label93;

	private Label label92;

	private Label label94;

	private Label label97;

	private Label label96;

	private Label label95;

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

	private Button btnAutoMap;

	private TabPage tabOtherCharges;

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

	private Panel panel37;

	private CtrlGrdBar ctrlgrdOtherChargesDetail;

	internal GridEX grdOtherChargesDetail;

	private TabPage tabOtherChargesHistory;

	private Panel panel39;

	private Label label111;

	internal GridEX grdOtherChargesHistory;

	public FrmExportSalesContract(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FrmExportSalesContract_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 700);
			ChkOnCommission.Visible = false;
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			BtnSaveAs.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			BtnDelete.Enabled = formrights.DoHaveCanDelete;
			Print.Enabled = formrights.DoHavePrintRights;
			AllowExportMultiCompanies = CommonServices.GetERPFeatureById(9);
			AllItemsBindonExportContract = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllItemsBindonExportContract"));
			PMGridInfoShouldNotCompulsoryOnContract = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PMGridInfoShouldNotCompulsoryOnContract"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			ShipmentDocumentsFeature = CommonServices.GetERPFeatureById(12);
			labelDCG.Visible = ShipmentDocumentsFeature;
			((Control)(object)CmbDocumentCustomGroup).Visible = ShipmentDocumentsFeature;
			if (ItemSearchWithNameOrCode)
			{
				rdSearchByCode.Checked = true;
				rdPackingItemCode.Checked = true;
			}
			else
			{
				rdSearchByName.Checked = true;
				rdPackingItemName.Checked = true;
			}
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtPMItem.Columns.Add("Id", typeof(int));
			dtPMItem.Columns.Add("ItemName");
			dtPMItem.Columns.Add("ItemCode");
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
			dtpcktype.Columns.Add("Id", typeof(int));
			dtpcktype.Columns.Add("Description");
			GenerateCode();
			SupplierFill();
			salesPersonFill();
			MultiCurrencyfill();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			StatusFill();
			ItemDetailFill();
			ItemPackingTypeFill();
			CropYearFill();
			ItemSpecificationsBind();
			OtherChargesBind();
			if (ShipmentDocumentsFeature)
			{
				DocumentcustomGroupBind();
			}
			BindOtherItems();
			PackingItemFill();
			FarmingNTradeBind();
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("PackTypeId", typeof(int));
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("PackSizeId", typeof(int));
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("QtyEquivalent", typeof(double));
			dtdetail.Columns.Add("NoOfBags", typeof(double));
			dtdetail.Columns.Add("Rate", typeof(double));
			dtdetail.Columns.Add("RateUOMId");
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("OtherItemAmount", typeof(double));
			dtdetail.Columns.Add("CommodityDetail");
			dtdetail.Columns.Add("ExImFarmingNTradeId", typeof(int));
			dtdetail.Columns.Add("FarmingNTrade");
			dtdetail.Columns.Add("ExImFarmingTypeId", typeof(int));
			dtdetail.Columns.Add("ExImTradeTypeId", typeof(int));
			dtdetail.Columns.Add("Remarks");
			grdDetails.DataSource = dtdetail;
			grdDetails.RetrieveStructure();
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
			dtPaymnent.Columns.Add("PaymentTermId");
			dtPaymnent.Columns.Add("PaymentTerm");
			dtPaymnent.Columns.Add("%ofTotal", typeof(double));
			dtPaymnent.Columns.Add("FcyAmount", typeof(double));
			dtPaymnent.Columns.Add("Remarks");
			grdPaymentTerm.DataSource = dtPaymnent;
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
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
			grdPackingMaterialDetail.DataSource = dtPmDetail;
			grdPackingMaterialDetail.RetrieveStructure();
			grdSettingsPM();
			dtOtherChargesDetail.Columns.Add("Id", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItemId", typeof(int));
			dtOtherChargesDetail.Columns.Add("ChargesItem");
			dtOtherChargesDetail.Columns.Add("AddAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("LessAmount", typeof(double));
			dtOtherChargesDetail.Columns.Add("Remarks");
			grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
			grdOtherChargesDetail.RetrieveStructure();
			grdSettingsOtherChargesDetail(grdOtherChargesDetail);
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
			dtLastSavedRecordHeader = ExImLcOrder.LastSavedRecord(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			if (Conversion.ToInt(CmbSalePerson.Value) == 0 && dtLastSavedRecordHeader.Rows.Count > 0)
			{
				CmbSalePerson.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["SalesPersonId"]);
				CmbFCYCode.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["FcurrencyId"]);
				CmbCropYear.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["CropYearId"]);
				combpcktype.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["InvPackingMaterialTypeId"]);
			}
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
			dtItemAndPMItemMap = ItemAndPMItemMap.ItemAndPMItemMap_GetByItemIds(UserAccount.CompanyId, UserAccount.OrganizationId, (string)null);
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int GenerateCode = ExImLcOrder.GenerateCode(new ExImLcOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 202,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (GenerateCode > 0)
			{
				txtExportSoNo.Text = GenerateCode.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int customerId = Conversion.ToInt(CmbCustomer.Value);
			int NotifyId = Conversion.ToInt(CmbNotifyParty.Value);
			DataTable dtSupplier = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DDL.BindDDLNew(dtSupplier, CmbCustomer, "Id", "CompanyName", "Customer", true);
			DDL.BindDDLNew(dtSupplier, CmbNotifyParty, "Id", "CompanyName", "NotifyParty", true);
			((UltraGridBase)CmbCustomer).DisplayLayout.Bands[0].Columns[1].Width = 300;
			((UltraGridBase)CmbNotifyParty).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			if (customerId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == customerId))
				{
					CmbCustomer.Value = customerId;
				}
				else
				{
					((Control)(object)CmbCustomer).Text = string.Empty;
				}
			}
			if (NotifyId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == NotifyId))
				{
					CmbNotifyParty.Value = customerId;
				}
				else
				{
					((Control)(object)CmbNotifyParty).Text = string.Empty;
				}
			}
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
			int id = Conversion.ToInt(CmbSalePerson.Value);
			DataTable dtSalePerson = CommonServices.GetSupplierustomerByCustomerGroupId("9");
			DDL.BindDDL(dtSalePerson, CmbSalePerson, "Id", "CompanyName", "Sales Person", ZeroIndex: true);
			((UltraGridBase)CmbSalePerson).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (dtSalePerson.Rows.Count > 0 && id > 0)
			{
				if (dtSalePerson.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbSalePerson.Value = id;
				}
				else
				{
					((Control)(object)CmbSalePerson).Text = string.Empty;
				}
			}
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
			int id = Conversion.ToInt(CmbDeliveryTerm.Value);
			DataTable dt = ExImDeliveryTerm.Getall();
			DDL.BindDDL(dt, CmbDeliveryTerm, "Id", "Code", "Delivery Term", ZeroIndex: true);
			if (dt.Rows.Count > 0 && id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbDeliveryTerm.Value = id;
				}
				else
				{
					((Control)(object)CmbDeliveryTerm).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int termId = Conversion.ToInt(CmbPaymentTerms.Value);
			int termIdDetail = Conversion.ToInt(CmbPaymentTermDetail.Value);
			DataTable dtDeliveryTerms = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDeliveryTerms.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("lcOrderTerm", typeof(string));
			dt.Columns.Add("Description", typeof(string));
			for (int i = 0; i < dtDeliveryTerms.Rows.Count; i++)
			{
				dt.Rows.Add(Conversion.ToInt(dtDeliveryTerms.Rows[i]["Id"]), Conversion.ToString(dtDeliveryTerms.Rows[i]["lcOrderTerm"]), Conversion.ToString(dtDeliveryTerms.Rows[i]["lcOrderTermDesc"]));
			}
			DDL.BindDDL(dt, CmbPaymentTerms, "Id", "lcOrderTerm", "Payment Term", ZeroIndex: true);
			DDL.BindDDL(dt, CmbPaymentTermDetail, "Id", "lcOrderTerm", "Payment Term", ZeroIndex: true);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			if (termId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == termId))
				{
					CmbPaymentTerms.Value = termId;
				}
				else
				{
					((Control)(object)CmbPaymentTerms).Text = string.Empty;
				}
			}
			if (termIdDetail > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == termIdDetail))
				{
					CmbPaymentTermDetail.Value = termIdDetail;
				}
				else
				{
					((Control)(object)CmbPaymentTermDetail).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingPortFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int loadId = Conversion.ToInt(CmbLoadingPort.Value);
			int destId = Conversion.ToInt(CmbDestinationPort.Value);
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DDL.BindDDLNew(dt, CmbLoadingPort, "Id", "PortName", "Loading Port", true);
			DDL.BindDDLNew(dt, CmbDestinationPort, "Id", "PortName", "Destination Port", true);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			if (loadId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == loadId))
				{
					CmbLoadingPort.Value = loadId;
				}
				else
				{
					((Control)(object)CmbLoadingPort).Text = string.Empty;
				}
			}
			if (destId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == destId))
				{
					CmbDestinationPort.Value = destId;
				}
				else
				{
					((Control)(object)CmbDestinationPort).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbFCYCode.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbFCYCode, "Id", "CurrencyCode", "Fcy Code", true);
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbFCYCode.Value = id;
				}
				else
				{
					((Control)(object)CmbFCYCode).Text = string.Empty;
				}
			}
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
			int id = Conversion.ToInt(CmbCropYear.Value);
			DataTable dt = CommonServices.CropYearGetAllService();
			DDL.BindDDLNew(dt, CmbCropYear, "Id", "CropYear", "Crop Year", true);
			if (dt.Rows.Count > 0 && id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbCropYear.Value = id;
				}
				else
				{
					((Control)(object)CmbCropYear).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImporterandExportBankFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int foreign = Conversion.ToInt(CmbImporterBank.Value);
			int home = Conversion.ToInt(CmbExporterbank.Value);
			List<Bank> lst = Bank.GetAll(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dtForeign = new DataTable();
			dtForeign.Columns.Add("Id", typeof(int));
			dtForeign.Columns.Add("BranchName");
			DataTable dtHome = new DataTable();
			dtHome.Columns.Add("Id", typeof(int));
			dtHome.Columns.Add("BranchName");
			if (lst.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i].IsHomeland == "Foreign Country")
				{
					dtForeign.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
				else if (lst[i].IsHomeland == "Home Country")
				{
					dtHome.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
			}
			DDL.BindDDL(dtForeign, CmbImporterBank, "Id", "BranchName", "Importer Bank", ZeroIndex: true);
			if (dtForeign.Rows.Count > 0 && foreign > 0)
			{
				if (dtForeign.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == foreign))
				{
					CmbImporterBank.Value = foreign;
				}
				else
				{
					((Control)(object)CmbImporterBank).Text = string.Empty;
				}
			}
			DDL.BindDDL(dtHome, CmbExporterbank, "Id", "BranchName", "Exporter Bank", ZeroIndex: true);
			if (dtHome.Rows.Count > 0 && home > 0)
			{
				if (dtHome.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == home))
				{
					CmbExporterbank.Value = home;
				}
				else
				{
					((Control)(object)CmbExporterbank).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			dtItem.Rows.Clear();
			int id = Conversion.ToInt(combitem.Value);
			DataTable dt = new DataTable();
			dt = ((!AllItemsBindonExportContract) ? Item.ReadAllForExportCombo(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			}) : CommonServices.ReadAllForComboTwoColumnsWithParentId());
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
			}
			if (rdSearchByName.Checked)
			{
				DDL.BindDDL(dtItem, combitem, "Id", "ItemName", "Item Name", ZeroIndex: true);
			}
			else
			{
				DDL.BindDDL(dtItem, combitem, "Id", "ItemCode", "Item Code", ZeroIndex: true);
			}
			((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			if (id > 0)
			{
				if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					combitem.Value = id;
				}
				else
				{
					((Control)(object)combitem).Text = string.Empty;
				}
			}
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
			int id = Conversion.ToInt(CmbOtherItem.Value);
			DataTable lst = CommonServices.GetItemByItemTypeId("14");
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(lst, CmbOtherItem, "Id", "ItemName", "Item Name", true);
			((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbOtherItem.Value = id;
				}
				else
				{
					((Control)(object)CmbOtherItem).Text = string.Empty;
				}
			}
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

	private void ItemPackingTypeFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(combpcktype.Value);
			DataTable lst = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			dtpcktype.Clear();
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dtpcktype.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["Description"]);
			}
			if (dtpcktype.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtpcktype, combpcktype, "Id", "Description", "Packing Type", ZeroIndex: true);
			if (id > 0)
			{
				if (dtpcktype.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					combpcktype.Value = id;
				}
				else
				{
					((Control)(object)combpcktype).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom(int DetailId = 0)
	{
		try
		{
			string Packuom = ((Control)(object)combitempck).Text;
			string Rateuom = ((Control)(object)combrateuom).Text;
			((Control)(object)combitempck).Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemIdForExport(Conversion.ToInt(combitem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, combitempck, "Id", "UOMCode", "PackUom", ZeroIndex: true);
				DDL.BindDDL(dtUom, combrateuom, "Id", "UOMCode", "RateUom", ZeroIndex: true);
				((UltraGridBase)combrateuom).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (Packuom != string.Empty)
				{
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
					{
						((Control)(object)combitempck).Text = Packuom;
					}
					else
					{
						((Control)(object)combitempck).Text = string.Empty;
					}
				}
				if (Rateuom != string.Empty)
				{
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
					{
						((Control)(object)combrateuom).Text = Rateuom;
					}
					else
					{
						((Control)(object)combrateuom).Text = string.Empty;
					}
				}
				if (DetailId != 0)
				{
					return;
				}
				if (Conversion.ToInt(combitem.Value) != 0 && (Rateuom != "1000" || Rateuom.ToUpper() != "1000KG" || Conversion.ToInt(combrateuom.Value) == 0))
				{
					int BaseRateUomId = CommonServices.GetBaseRateUomId(dtUom);
					if (BaseRateUomId > 0)
					{
						combrateuom.Value = BaseRateUomId;
					}
					else
					{
						DataRow foundRow = dtUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<double>("Equivalent") == 1000.0);
						int RateUomId = ((foundRow != null) ? Conversion.ToInt(foundRow["Id"]) : 0);
						if (RateUomId > 0)
						{
							combrateuom.Value = RateUomId;
						}
					}
				}
				if (((Control)(object)combrateuom).Text.ToUpper() != "1000KG" && ((Control)(object)combrateuom).Text != "1000")
				{
					((Control)(object)combrateuom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)combitempck).Text = string.Empty;
				((UltraGridBase)combitempck).DataSource = null;
				((Control)(object)combrateuom).Text = string.Empty;
				((UltraGridBase)combrateuom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemSpecificationsBind()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbcomditiyDetailByItemCus.Value);
			DataTable lst = CommodityDetailItemCustomerWise.GetRemarks(new CommodityDetailItemCustomerWise
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(combitem.Value),
				SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value)
			});
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(lst, cmbcomditiyDetailByItemCus, "Id", "Remarks", "Commodity Detail Remarks", true);
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbcomditiyDetailByItemCus.Value = id;
				}
				else
				{
					((Control)(object)cmbcomditiyDetailByItemCus).Text = string.Empty;
				}
			}
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
			DataTable dt = ExportCharges.ComboBind(UserAccount.OrganizationId, UserAccount.CompanyId);
			InfragisticsHelper.BindAndRetainSelection(CmbChargesNameChargeDetail, dt, "ExportChargesID", "ExportChargesName", " Export Charges", AllColumns: true, Conversion.ToInt(CmbChargesNameChargeDetail.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentcustomGroupBind()
	{
		try
		{
			int id = Conversion.ToInt(CmbDocumentCustomGroup.Value);
			DataTable lst = ClientCustomGroup.GetCustomGroupAllocatedToCustomerBySupplierCustomerId(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbCustomer.Value));
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, CmbDocumentCustomGroup, "CustomGroupId", "CustomGroup", "CustomGroup", true);
				if (id > 0)
				{
					if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("CustomGroupId") == id))
					{
						CmbDocumentCustomGroup.Value = id;
					}
					else
					{
						((Control)(object)CmbDocumentCustomGroup).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbDocumentCustomGroup).DataSource = null;
				((Control)(object)CmbDocumentCustomGroup).Text = string.Empty;
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
			int id = Conversion.ToInt(CmbFarmingNTrade.Value);
			DataTable lst = ExImLcOrder.GetFarmingNTrade();
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(lst, CmbFarmingNTrade, "Id", "FarmingNTrade", "Farming N Trade ", ZeroIndex: true);
			((UltraGridBase)CmbFarmingNTrade).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			((UltraGridBase)CmbFarmingNTrade).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbFarmingNTrade.Value = id;
				}
				else
				{
					((Control)(object)CmbFarmingNTrade).Text = string.Empty;
				}
			}
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
			int id = Conversion.ToInt(CmbExportCompany.Value);
			DataTable lst = ExImLcOrder.GetExportCompaniesByCompany(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(lst, CmbExportCompany, "Id", "CompName", "Export Company ", ZeroIndex: true);
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbExportCompany.Value = id;
				}
				else
				{
					((Control)(object)CmbExportCompany).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingItemFill()
	{
		try
		{
			int pmId = Conversion.ToInt(CmbPMItem.Value);
			dtPMItem.Rows.Clear();
			DataTable dt = CommonServices.GetItemByItemTypeId("14");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtPMItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
			}
			if (dtPMItem.Rows.Count <= 0)
			{
				return;
			}
			if (rdPackingItemName.Checked)
			{
				DDL.BindDDL(dtPMItem, CmbPMItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
			}
			else
			{
				DDL.BindDDL(dtPMItem, CmbPMItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
			}
			((UltraGridBase)CmbPMItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (pmId > 0)
			{
				if (dtPMItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == pmId))
				{
					CmbPMItem.Value = pmId;
				}
				else
				{
					((Control)(object)CmbPMItem).Text = string.Empty;
				}
			}
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
			if (Conversion.ToInt(combrateuom.Value) == 0)
			{
				dtLastSavedRecordByItemId = ExImLcOrder.LastSavedRecordByItemId(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(combitem.Value));
			}
			bindRateUomAndItemPackUom();
			ItemSpecificationsBind();
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
			ItemSpecificationsBind();
			if (ShipmentDocumentsFeature)
			{
				DocumentcustomGroupBind();
			}
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
			if (Conversion.ToInt(CmbPaymentTermDetail.Value) != 0)
			{
				string Desc = Conversion.ToString(((UltraDropDownBase)CmbPaymentTermDetail).SelectedRow.Cells[2].Value);
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

	private void CmbPaymentTerms_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbPaymentTerms.Value) != 0)
			{
				string Desc = Conversion.ToString(((UltraDropDownBase)CmbPaymentTerms).SelectedRow.Cells[2].Value);
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

	private void cmbcomditiyDetailByItemCus_Leave(object sender, EventArgs e)
	{
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtItem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(combitem.Value) > 0)
				{
					ItemId = Conversion.ToInt(combitem.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtItem, combitem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtItem, combitem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				combitem.Value = ItemId;
				combitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdPackingItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtPMItem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbPMItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbPMItem.Value);
				}
				if (rdPackingItemName.Checked)
				{
					DDL.BindDDL(dtPMItem, CmbPMItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtPMItem, CmbPMItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbPMItem.Value = ItemId;
				CmbPMItem.Focus();
			}
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
				int packSizeId = Conversion.ToInt(r2.Cells["PackSizeId"].Value);
				double noOfBags = Conversion.ToDouble(r2.Cells["NoOfBags"].Value);
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
				dtBrandForPMItem.Rows.Add(entry.Key.Item1, Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToString(r.Cells["ItemCode"].Value), entry.Key.Item2, Conversion.ToString(r.Cells["PackType"].Value), entry.Key.Item3, Conversion.ToString(r.Cells["PackSize"].Value), entry.Value.Item1, entry.Value.Item1 * Conversion.ToDouble(r.Cells["QtyEquivalent"].Value), entry.Value.Item2);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindBrandItemsFromOutPutGrid()
	{
		try
		{
			FilldtBrandForPMItem();
			int id = Conversion.ToInt(cmbBrandForPmEntry.Value);
			if (dtBrandForPMItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtBrandForPMItem, cmbBrandForPmEntry, "Id", "ItemName", "Brand Item", ZeroIndex: true);
				((UltraGridBase)cmbBrandForPmEntry).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbBrandForPmEntry).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				if (id > 0)
				{
					if (dtBrandForPMItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbBrandForPmEntry.Value = id;
					}
					else
					{
						((Control)(object)cmbBrandForPmEntry).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbBrandForPmEntry).DataSource = null;
				((Control)(object)cmbBrandForPmEntry).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcomditiyDetailByItemCus_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string remarks = Conversion.ToString((object)((Control)(object)cmbcomditiyDetailByItemCus).Text);
			txtCommodityDetailIndetail.Text = remarks;
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
			if (dtBrandForPMItem.Rows.Count > 0)
			{
				int ItemId = Conversion.ToInt(cmbBrandForPmEntry.Value);
				if (Conversion.ToInt(cmbBrandForPmEntry.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbBrandForPmEntry.Value);
				}
				if (radBrandName.Checked)
				{
					DDL.BindDDL(dtBrandForPMItem, cmbBrandForPmEntry, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtBrandForPMItem, cmbBrandForPmEntry, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbBrandForPmEntry.Value = ItemId;
				cmbBrandForPmEntry.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPMItem_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(CmbPMItem.Value);
			if (ItemId > 0 && ItemId != ItemIdOfPreviousRate)
			{
				GetPmItemRate(ItemId);
			}
			BindPmItemPackUom();
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
				txtPackingItemRate.Text = dt.ToString("#,##.##");
				ItemIdOfPreviousRate = ItemId;
			}
			else
			{
				ItemIdOfPreviousRate = 0;
				txtPackingItemRate.Text = 0.ToString("#,##.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPmItemPackUom()
	{
		try
		{
			string Packuom = ((Control)(object)CmbUomPm).Text;
			((Control)(object)CmbUomPm).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbPMItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbUomPm, "Id", "UOMCode", "PackUom", ZeroIndex: true);
				if (Packuom != string.Empty)
				{
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
					{
						((Control)(object)CmbUomPm).Text = Packuom;
					}
					else
					{
						((Control)(object)CmbUomPm).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbUomPm).Text = string.Empty;
				((UltraGridBase)CmbUomPm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (AllowExportMultiCompanies && (((UltraGridBase)CmbExportCompany).ActiveRow == null || Conversion.ToInt(CmbExportCompany.Value) == 0))
		{
			MessageBox.Show("Export Company Is required");
			CmbExportCompany.Focus();
			return false;
		}
		if (txtExportSoNo.Text.Trim() == string.Empty || txtExportSoNo.Text.Trim() == "0")
		{
			MessageBox.Show("Export So No Is required");
			txtExportSoNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbSalePerson.Value) == 0)
		{
			MessageBox.Show("Sale Person Is required");
			CmbSalePerson.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbCustomer.Value) == 0)
		{
			MessageBox.Show("Customer Is required");
			CmbCustomer.Focus();
			return false;
		}
		if (txtBuyerOrderNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("OrderNo Is required");
			txtBuyerOrderNo.Focus();
			return false;
		}
		if (datOrderDate.Text.Trim() == string.Empty)
		{
			MessageBox.Show("OrderDate Is required");
			datOrderDate.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtMTon.Text) > 0.0))
		{
			MessageBox.Show("M.Ton Must Be Greater Than 0.");
			txtMTon.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtNoOfContainers.Text) <= 0)
		{
			MessageBox.Show("No. of Containers Must Be Greater Than 0.");
			txtMTon.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbFCYCode.Value) == 0)
		{
			MessageBox.Show("Fcy Code Is required");
			CmbFCYCode.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
		{
			MessageBox.Show("Exchange Rate Is required");
			txtExchangeRate.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtFCYAmount.Text) > 0.0) || txtFCYAmount.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Fcy Amount Must Be Greater Than 0.");
			txtFCYAmount.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtDeliveryDays.Text) == 0)
		{
			MessageBox.Show("Delivery Days Is required");
			txtDeliveryDays.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbDeliveryTerm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Is required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbLoadingPort.Value) == 0)
		{
			MessageBox.Show("Loading Port Is required");
			CmbLoadingPort.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbDestinationPort.Value) == 0)
		{
			MessageBox.Show("Destination Port  Is required");
			CmbDestinationPort.Focus();
			return false;
		}
		return true;
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
		if (Conversion.ToInt(combitem.Value) == 0)
		{
			MessageBox.Show("Item field required");
			combitem.Focus();
			return false;
		}
		if (Conversion.ToInt(combpcktype.Value) == 0)
		{
			MessageBox.Show("Pack Type field required");
			combpcktype.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year field is required");
			CmbCropYear.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQtyMTon.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty/M.Ton field required");
			txtQtyMTon.Focus();
			return false;
		}
		if (Conversion.ToInt(combitempck.Value) == 0)
		{
			MessageBox.Show("Pack Size field required");
			combitempck.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNoOfBags.Text.Trim()) == 0.0)
		{
			MessageBox.Show("No of Bags field required");
			txtNoOfBags.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCostMTon.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtCostMTon.Focus();
			return false;
		}
		if (Conversion.ToInt(combrateuom.Value) == 0)
		{
			MessageBox.Show("Rate UOM field required");
			combrateuom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount field required");
			txtAmount.Focus();
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
		if (Conversion.ToDouble((object)txtoQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty field required");
			txtoQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtoRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount field required");
			txtoAmount.Focus();
			return false;
		}
		return true;
	}

	private bool PackingMaterialFormValidation()
	{
		if (Conversion.ToInt(cmbBrandForPmEntry.Value) == 0 || ((UltraGridBase)cmbBrandForPmEntry).ActiveRow == null)
		{
			MessageBox.Show("Brand Item Is required");
			cmbBrandForPmEntry.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbBrandUomPm.Value) == 0 || ((UltraGridBase)CmbBrandUomPm).ActiveRow == null)
		{
			MessageBox.Show("Brand Uom Is required");
			CmbBrandUomPm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbBrandPackingTypePm.Value) == 0 || ((UltraGridBase)CmbBrandPackingTypePm).ActiveRow == null)
		{
			MessageBox.Show("Item Type Is required");
			CmbBrandPackingTypePm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtBrandOuterQtyPm.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Brand Outer Qty field required");
			txtBrandOuterQtyPm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtBrandInnerQtyPm.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Brand Inner Qty field required");
			txtBrandInnerQtyPm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPMItem.Value) == 0 || ((UltraGridBase)CmbPMItem).ActiveRow == null)
		{
			MessageBox.Show("Item Is required");
			CmbPMItem.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbUomPm.Value) == 0 || ((UltraGridBase)CmbUomPm).ActiveRow == null)
		{
			MessageBox.Show("Pm Uom Is required");
			CmbUomPm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtInnerQtyPm.Text.Trim()) == 0.0 && Conversion.ToDouble((object)txtOuterQtyPm.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Pm OuterQty or Pm InnerQty is required");
			txtOuterQtyPm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtOuterQtyPm.Text.Trim()) != 0.0 && Conversion.ToDouble((object)txtInnerQtyPm.Text.Trim()) != 0.0)
		{
			MessageBox.Show("You can either add the PM OuterQty or the PM InnerQty at a time, not both");
			txtInnerQtyPm.Text = "";
			CalculatePackingAmount(Conversion.ToDouble((object)txtOuterQtyPm.Text.Trim()));
			txtInnerQtyPm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPackingItemRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtPackingItemRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPackingAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount field required");
			txtPackingAmount.Focus();
			return false;
		}
		return true;
	}

	private bool PaymentFormValidation()
	{
		if (Conversion.ToInt(CmbPaymentTermDetail.Value) == 0)
		{
			MessageBox.Show("Payment Term field required");
			CmbPaymentTermDetail.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)PercntOfTotal.Text.Trim()) == 0.0)
		{
			MessageBox.Show("% of Total field required");
			PercntOfTotal.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPaymentDetailFcyAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Fcy Amount field required");
			txtPaymentDetailFcyAmount.Focus();
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

	private void AddtoGrid()
	{
		try
		{
			if (DetailFormValidation())
			{
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
				dtdetail.Rows.Add(0, ((UltraDropDownBase)combitem).SelectedRow.Cells[2].Value, combitem.Value, ((UltraDropDownBase)combitem).SelectedRow.Cells[1].Value, combpcktype.Value, Conversion.ToString(((UltraDropDownBase)combpcktype).SelectedRow.Cells[1].Value), CmbCropYear.Value, ((Control)(object)CmbCropYear).Text, Conversion.ToDouble((object)txtQtyMTon.Text), combitempck.Value, ((Control)(object)combitempck).Text, ((UltraDropDownBase)combitempck).SelectedRow.Cells["QtyEquivalent"].Value, Conversion.ToDouble((object)txtNoOfBags.Text), Conversion.ToDouble((object)txtCostMTon.Text), combrateuom.Value, Conversion.ToString(((UltraDropDownBase)combrateuom).SelectedRow.Cells[1].Value), Conversion.ToDouble((object)txtAmount.Text), 0, txtCommodityDetailIndetail.Text, FarmingNTradeID, FarmingNTrade, ExImFarmingTypeId, ExImTradeTypeId, txtRemarks.Text);
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
				ResetDetail();
				combitem.Focus();
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
		try
		{
			AddtoGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetails.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				combitem.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(item.Cells["Id"].Value));
				combpcktype.Value = item.Cells["PackTypeId"].Value;
				combitempck.Value = item.Cells["PackSizeId"].Value;
				CmbCropYear.Value = item.Cells["CropYearId"].Value;
				((Control)(object)CmbCropYear).Text = item.Cells["CropYear"].Text;
				txtQtyMTon.Text = item.Cells["Qty/M.Ton"].Value.ToString();
				combrateuom.Value = item.Cells["RateUOMId"].Value;
				txtNoOfBags.Text = item.Cells["NoOfBags"].Value.ToString();
				txtCostMTon.Text = item.Cells["Rate"].Value.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Value.ToString();
				txtCommodityDetailIndetail.Text = item.Cells["CommodityDetail"].Value.ToString();
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				CmbFarmingNTrade.Value = item.Cells["ExImFarmingNTradeId"].Value;
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				combitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		ResetDetail();
	}

	private void grdSettings()
	{
		try
		{
			grdDetails.RootTable.Columns["Id"].Visible = false;
			grdDetails.RootTable.Columns["ItemId"].Visible = false;
			grdDetails.RootTable.Columns["CropYearId"].Visible = false;
			grdDetails.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetails.RootTable.Columns["PackSizeId"].Visible = false;
			grdDetails.RootTable.Columns["RateUOMId"].Visible = false;
			grdDetails.RootTable.Columns["ExImFarmingNTradeId"].Visible = false;
			grdDetails.RootTable.Columns["ExImTradeTypeId"].Visible = false;
			grdDetails.RootTable.Columns["ExImFarmingTypeId"].Visible = false;
			grdDetails.RootTable.Columns["QtyEquivalent"].Visible = false;
			grdDetails.RootTable.Columns["QtyEquivalent"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["QtyEquivalent"].FormatString = "0,0";
			grdDetails.RootTable.Columns["QtyEquivalent"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["Qty/M.Ton"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["Qty/M.Ton"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Qty/M.Ton"].FormatString = "0,0";
			grdDetails.RootTable.Columns["Qty/M.Ton"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["NoOfBags"].FormatString = "0,0";
			grdDetails.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetails.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetails.RootTable.Columns["OtherItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["OtherItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["OtherItemAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetails.RootTable.Columns["OtherItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetails.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grdDetails);
			grdDetails.RootTable.Columns.Add("Delete");
			grdDetails.RootTable.Columns["Delete"].Key = "Delete";
			grdDetails.RootTable.Columns["Delete"].Caption = "X";
			grdDetails.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetails.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetails.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetails.RootTable.Columns["Delete"].Width = 20;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdDetails.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Reset  the Detail first...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ExImLcOrderPackingDetail vd = new ExImLcOrderPackingDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ExImItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.InvPackingMaterialTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.NetWeight = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
					vd.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["PackSizeId"].Value);
					vd.UOMScheduleIdInner = Conversion.ToInt(r.Cells["PackSizeId"].Value);
					vd.NoOfUnit = Conversion.ToDouble(r.Cells["NoOfBags"].Value);
					vd.UOMScheduleIdRate = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					vd.OuterQty = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
					vd.InnerQty = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
					vd.NoOfUnit = Conversion.ToDouble(r.Cells["NoOfBags"].Value);
					vd.RatePrice = Conversion.ToDouble(r.Cells["Rate"].Value);
					vd.TotalAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd.CommodityDetail = Conversion.ToString(r.Cells["CommodityDetail"].Value);
					vd.ExImFarmingNTradeId = Conversion.ToInt(r.Cells["ExImFarmingNTradeId"].Value);
					vd.ExImTradeTypeId = Conversion.ToInt(r.Cells["ExImTradeTypeId"].Value);
					vd.ExImFarmingTypeId = Conversion.ToInt(r.Cells["ExImFarmingTypeId"].Value);
					vd.Other_Description = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtdetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
			}
			grdDetails.UpdateData();
			InsterAmountandQtyMtonInHeader();
			ProportionateOtherItemAmount();
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
				dtdetail.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)combitem).SelectedRow.Cells[2].Value;
				dtdetail.Rows[updateDetailIndex]["ItemId"] = combitem.Value;
				dtdetail.Rows[updateDetailIndex]["ItemName"] = ((UltraDropDownBase)combitem).SelectedRow.Cells[1].Value;
				dtdetail.Rows[updateDetailIndex]["PackTypeId"] = combpcktype.Value;
				dtdetail.Rows[updateDetailIndex]["PackType"] = ((Control)(object)combpcktype).Text;
				dtdetail.Rows[updateDetailIndex]["Qty/M.Ton"] = txtQtyMTon.Text;
				dtdetail.Rows[updateDetailIndex]["PackSizeId"] = combitempck.Value;
				dtdetail.Rows[updateDetailIndex]["PackSize"] = ((Control)(object)combitempck).Text;
				dtdetail.Rows[updateDetailIndex]["QtyEquivalent"] = ((UltraDropDownBase)combitempck).SelectedRow.Cells["QtyEquivalent"].Value;
				dtdetail.Rows[updateDetailIndex]["CropYearId"] = CmbCropYear.Value;
				dtdetail.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropYear).Text;
				dtdetail.Rows[updateDetailIndex]["NoOfBags"] = txtNoOfBags.Text;
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)combrateuom).Text;
				dtdetail.Rows[updateDetailIndex]["Rate"] = txtCostMTon.Text;
				dtdetail.Rows[updateDetailIndex]["RateUOMId"] = combrateuom.Value;
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)combrateuom).Text;
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
				dtdetail.Rows[updateDetailIndex]["CommodityDetail"] = txtCommodityDetailIndetail.Text;
				dtdetail.Rows[updateDetailIndex]["ExImFarmingNTradeId"] = FarmingNTradeID;
				dtdetail.Rows[updateDetailIndex]["FarmingNTrade"] = FarmingNTrade;
				dtdetail.Rows[updateDetailIndex]["ExImFarmingTypeId"] = ExImFarmingTypeId;
				dtdetail.Rows[updateDetailIndex]["ExImTradeTypeId"] = ExImTradeTypeId;
				dtdetail.Rows[updateDetailIndex]["Remarks"] = txtRemarks.Text;
				grdDetails.DataSource = dtdetail;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				combitem.Focus();
				InsterAmountandQtyMtonInHeader();
				ProportionateOtherItemAmount();
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
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Visible = false;
			grdPaymentTerm.RootTable.Columns["%ofTotal"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["%ofTotal"].FormatString = "#,##0.###";
			grdPaymentTerm.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerm.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdPaymentTerm.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdPaymentTerm);
			grdPaymentTerm.RootTable.Columns.Add("Delete");
			grdPaymentTerm.RootTable.Columns["Delete"].Key = "Delete";
			grdPaymentTerm.RootTable.Columns["Delete"].Caption = "X";
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonText = "X";
			grdPaymentTerm.RootTable.Columns["Delete"].Width = 20;
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
			double prcntOfTotal = Conversion.ToDouble((object)PercntOfTotal.Text);
			if (prcntOfTotal > 100.0)
			{
				MessageBox.Show("Percent can't be Greater than 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				PercntOfTotal.Focus();
				return;
			}
			if (grdPaymentTerm.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPaymentTerm.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					if (Conversion.ToInt(rows[i].Cells["PaymentTermId"].Value) == Conversion.ToInt(CmbPaymentTermDetail.Value))
					{
						MessageBox.Show("You Can't Add Same Payment Term", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						CmbPaymentTermDetail.Focus();
						return;
					}
				}
				double TotalPaymentPrcnt = Conversion.ToDouble(grdPaymentTerm.GetTotal(grdPaymentTerm.RootTable.Columns["%ofTotal"], (AggregateFunction)2));
				if (TotalPaymentPrcnt + prcntOfTotal > 100.0)
				{
					MessageBox.Show("Toatl Percent can't be Greater than 100%.Remaing " + (100.0 - TotalPaymentPrcnt), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					PercntOfTotal.Text = (100.0 - TotalPaymentPrcnt).ToString();
					return;
				}
			}
			dtPaymnent.Rows.Add(CmbPaymentTermDetail.Value, ((Control)(object)CmbPaymentTermDetail).Text, PercntOfTotal.Text, txtPaymentDetailFcyAmount.Text, txtPaymentRemarksDetail.Text);
			grdPaymentTerm.DataSource = dtPaymnent;
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			ResetPaymentDetail();
			CmbPaymentTermDetail.Focus();
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
			double prcntOfTotal = Conversion.ToDouble((object)PercntOfTotal.Text);
			if (prcntOfTotal > 100.0)
			{
				MessageBox.Show("Percent can't be Greater than 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				PercntOfTotal.Focus();
				return;
			}
			if (grdPaymentTerm.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (item.RowIndex != updateDetailIndexPaymentDetail && Conversion.ToInt(item.Cells["PaymentTermId"].Value) == Conversion.ToInt(CmbPaymentTermDetail.Value))
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
					PercntOfTotal.Text = (100.0 - t).ToString();
					return;
				}
			}
			dtPaymnent.Rows[updateDetailIndexPaymentDetail]["PaymentTermId"] = CmbPaymentTermDetail.Value;
			dtPaymnent.Rows[updateDetailIndexPaymentDetail]["PaymentTerm"] = ((Control)(object)CmbPaymentTermDetail).Text;
			dtPaymnent.Rows[updateDetailIndexPaymentDetail]["%ofTotal"] = PercntOfTotal.Text;
			dtPaymnent.Rows[updateDetailIndexPaymentDetail]["FcyAmount"] = txtPaymentDetailFcyAmount.Text;
			dtPaymnent.Rows[updateDetailIndexPaymentDetail]["Remarks"] = txtPaymentRemarksDetail.Text;
			grdPaymentTerm.DataSource = dtPaymnent;
			btnAddPayment.Visible = true;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
			ResetPaymentDetail();
			CmbPaymentTermDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelPayment_Click(object sender, EventArgs e)
	{
		try
		{
			ResetPaymentDetail();
			btnAddPayment.Visible = true;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
			updateDetailIndexPaymentDetail = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddPayment_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "grdPaymentTerm")
			{
				CmbPaymentTermDetail.Focus();
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
			if (base.ActiveControl.Name == "grdPaymentTerm")
			{
				CmbPaymentTermDetail.Focus();
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
				CmbPaymentTermDetail.Value = item.Cells["PaymentTermId"].Value.ToString();
				PercntOfTotal.Text = Conversion.ToDouble(item.Cells["%ofTotal"].Value).ToString("#,##0.###");
				txtPaymentDetailFcyAmount.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtPaymentRemarksDetail.Text = Conversion.ToString(item.Cells["Remarks"].Value).ToString();
				btnAddPayment.Visible = false;
				btnUpdatePayment.Visible = true;
				btnCancelPayment.Visible = true;
				CmbPaymentTermDetail.Focus();
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
					throw new Exception("Reset  the Detail first...");
				}
				dtPaymnent.Rows[item.RowIndex].Delete();
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
			double brandOuterQty = Conversion.ToDouble((object)txtBrandOuterQtyPm.Text);
			double brandInnerQty = Conversion.ToDouble((object)txtBrandInnerQtyPm.Text);
			int selectedBrandId = Conversion.ToInt(cmbBrandForPmEntry.Value);
			int selectedBrandUomId = Conversion.ToInt(CmbBrandUomPm.Value);
			int selectedPackingTypeId = Conversion.ToInt(CmbBrandPackingTypePm.Value);
			double currentOuterQtyPm = Conversion.ToDouble((object)txtOuterQtyPm.Text);
			double currentInnerQtyPm = Conversion.ToDouble((object)txtInnerQtyPm.Text);
			double gridOuterQtyPm = (from GridEXRow row in grdPackingMaterialDetail.GetRows()
				where Conversion.ToInt(row.Cells["BrandId"].Value) == selectedBrandId && Conversion.ToInt(row.Cells["BrandUomId"].Value) == selectedBrandUomId && Conversion.ToInt(row.Cells["PackingTypeId"].Value) == selectedPackingTypeId && (!Update || row.RowIndex != updateDetailIndexPmDetail)
				select row).Sum((GridEXRow row) => Conversion.ToDouble(row.Cells["PmItemOuterQty"].Value));
			double totalOuterQtyPm = gridOuterQtyPm + currentOuterQtyPm;
			if (Conversion.ToDouble((object)txtOuterQtyPm.Text) > 0.0 && totalOuterQtyPm > brandOuterQty)
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
			if (Conversion.ToDouble((object)txtInnerQtyPm.Text) > 0.0 && totalInnerQtyPm > brandInnerQty)
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

	private void grdSettingsPM()
	{
		try
		{
			grdPackingMaterialDetail.RootTable.Columns["Id"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["BrandId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["BrandUomId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PmItemId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PmItemId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PmItemUomId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["PmItemUomId"].Visible = false;
			grdPackingMaterialDetail.RootTable.Columns["BrandOuterQty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialDetail.RootTable.Columns["BrandOuterQty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["BrandOuterQty"].FormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["BrandOuterQty"].TotalFormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["BrandInnerQty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialDetail.RootTable.Columns["BrandInnerQty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["BrandInnerQty"].FormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["BrandInnerQty"].TotalFormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].FormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["PmItemOuterQty"].TotalFormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].FormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["PmItemInnerQty"].TotalFormatString = "#,##0.###";
			grdPackingMaterialDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPackingMaterialDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPackingMaterialDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdPackingMaterialDetail);
			grdPackingMaterialDetail.RootTable.Columns.Add("Delete");
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterialDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterialDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterialDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterialDetail.RootTable.Columns["Delete"].Width = 20;
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
				dtPmDetail.Rows.Add(0, Conversion.ToInt(cmbBrandForPmEntry.Value), Conversion.ToString(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbBrandUomPm.Value), Conversion.ToString((object)((Control)(object)CmbBrandUomPm).Text), Conversion.ToInt(CmbBrandPackingTypePm.Value), Conversion.ToString((object)((Control)(object)CmbBrandPackingTypePm).Text), Conversion.ToDouble((object)txtBrandOuterQtyPm.Text), Conversion.ToDouble((object)txtBrandInnerQtyPm.Text), Conversion.ToInt(CmbPMItem.Value), Conversion.ToString(((UltraDropDownBase)CmbPMItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbPMItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbUomPm.Value), Conversion.ToString((object)((Control)(object)CmbUomPm).Text), Conversion.ToDouble((object)txtOuterQtyPm.Text), Conversion.ToDouble((object)txtInnerQtyPm.Text), Conversion.ToDouble((object)txtPackingItemRate.Text), Conversion.ToDouble((object)txtPackingAmount.Text), Conversion.ToString((object)txtPackingRemarks.Text));
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				grdPackingMaterialDetail.RetrieveStructure();
				grdSettingsPM();
				ResetPmDetail();
				cmbBrandForPmEntry.Focus();
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
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandId"] = Conversion.ToInt(cmbBrandForPmEntry.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandCode"] = Conversion.ToString(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["ItemCode"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandName"] = Conversion.ToString(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["ItemName"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandUomId"] = Conversion.ToInt(CmbBrandUomPm.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandUom"] = Conversion.ToString((object)((Control)(object)CmbBrandUomPm).Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PackingTypeId"] = Conversion.ToInt(CmbBrandPackingTypePm.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PackingType"] = Conversion.ToString((object)((Control)(object)CmbBrandPackingTypePm).Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandOuterQty"] = Conversion.ToDouble((object)txtBrandOuterQtyPm.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["BrandInnerQty"] = Conversion.ToDouble((object)txtBrandInnerQtyPm.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemId"] = Conversion.ToInt(CmbPMItem.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbPMItem).SelectedRow.Cells["ItemCode"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemName"] = Conversion.ToString(((UltraDropDownBase)CmbPMItem).SelectedRow.Cells["ItemName"].Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemUomId"] = Conversion.ToInt(CmbUomPm.Value);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemUom"] = Conversion.ToString((object)((Control)(object)CmbUomPm).Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemOuterQty"] = Conversion.ToDouble((object)txtOuterQtyPm.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["PmItemInnerQty"] = Conversion.ToDouble((object)txtInnerQtyPm.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["Rate"] = Conversion.ToDouble((object)txtPackingItemRate.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["Amount"] = Conversion.ToDouble((object)txtPackingAmount.Text);
				dtPmDetail.Rows[updateDetailIndexPmDetail]["Remarks"] = Conversion.ToString((object)txtPackingRemarks.Text);
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				btnPackingAdd.Visible = true;
				btnPackingUpdate.Visible = false;
				btnPackingCancel.Visible = false;
				ResetPmDetail();
				CmbPMItem.Focus();
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
				CmbPMItem.Focus();
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
				CmbPMItem.Focus();
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
			RowEnumerator enumerator = ((UltraGridBase)cmbBrandForPmEntry).Rows.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					UltraGridRow row = enumerator.Current;
					if (Conversion.ToInt(row.Cells["Id"].Value) == Conversion.ToInt(item.Cells["BrandId"].Value) && Conversion.ToInt(row.Cells["PackUomId"].Value) == Conversion.ToInt(item.Cells["BrandUomId"].Value) && Conversion.ToInt(row.Cells["PackingTypeId"].Value) == Conversion.ToInt(item.Cells["PackingTypeId"].Value))
					{
						((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow = row;
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
			CmbPMItem.Value = Conversion.ToInt(item.Cells["PmItemId"].Value);
			CmbPMItem_Leave(null, null);
			CmbUomPm.Value = Conversion.ToInt(item.Cells["PmItemUomId"].Value);
			txtOuterQtyPm.Text = Conversion.ToDouble(item.Cells["PmItemOuterQty"].Value).ToString("#,##0.###");
			txtInnerQtyPm.Text = Conversion.ToDouble(item.Cells["PmItemInnerQty"].Value).ToString("#,##0.###");
			txtPackingItemRate.Text = Conversion.ToDouble(item.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
			txtPackingAmount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
			txtPackingRemarks.Text = item.Cells["Remarks"].Value.ToString();
			btnPackingAdd.Visible = false;
			btnPackingUpdate.Visible = true;
			btnPackingCancel.Visible = true;
			cmbBrandForPmEntry.Focus();
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

	private void cmbBrandForPmEntry_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbBrandForPmEntry).ActiveRow != null && Conversion.ToInt(cmbBrandForPmEntry.Value) > 0)
			{
				int BrandUomId = Conversion.ToInt(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["PackUomId"].Value);
				string BrandUom = Conversion.ToString(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["PackUom"].Value);
				int PackingTypeId = Conversion.ToInt(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["PackingTypeId"].Value);
				string PackingType = Conversion.ToString(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["PackingType"].Value);
				double BrandOuterQty = Conversion.ToDouble(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["OuterQty"].Value);
				double BrandInnerQty = Conversion.ToDouble(((UltraDropDownBase)cmbBrandForPmEntry).SelectedRow.Cells["InnerQty"].Value);
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("BrandUomId", typeof(int));
				dtUom.Columns.Add("BrandUom", typeof(string));
				dtUom.Rows.Add(BrandUomId, BrandUom);
				DDL.BindDDL(dtUom, CmbBrandUomPm, "BrandUomId", "BrandUom", "BrandUom", ZeroIndex: false);
				((UltraGridBase)CmbBrandUomPm).Rows[0].Activate();
				DataTable dtPackingType = new DataTable();
				dtPackingType.Columns.Add("PackingTypeId", typeof(int));
				dtPackingType.Columns.Add("PackingType", typeof(string));
				dtPackingType.Rows.Add(PackingTypeId, PackingType);
				DDL.BindDDL(dtPackingType, CmbBrandPackingTypePm, "PackingTypeId", "PackingType", "PackingType", ZeroIndex: false);
				((UltraGridBase)CmbBrandPackingTypePm).Rows[0].Activate();
				txtBrandOuterQtyPm.Text = BrandOuterQty.ToString("#,##.##");
				txtBrandInnerQtyPm.Text = BrandInnerQty.ToString("#,##.##");
			}
			else
			{
				((UltraGridBase)CmbBrandUomPm).DataSource = null;
				((Control)(object)CmbBrandUomPm).Text = "";
				((UltraGridBase)CmbBrandPackingTypePm).DataSource = null;
				((Control)(object)CmbBrandPackingTypePm).Text = "";
				txtBrandOuterQtyPm.Text = 0.ToString("#,##.##");
				txtBrandInnerQtyPm.Text = 0.ToString("#,##.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnoadd_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows.Add(CmbOtherItem.Value, ((Control)(object)CmbOtherItem).Text, Conversion.ToDouble((object)txtoQty.Text.Trim()), Conversion.ToDouble((object)txtoRate.Text.Trim()), Conversion.ToDouble((object)txtoAmount.Text.Trim()), txtoRemarks.Text.Trim());
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				ResetOtherItems();
				InsterAmountandQtyMtonInHeader();
				CmbOtherItem.Focus();
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
					dtOtherItem.Rows[item.RowIndex].Delete();
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

	private void grdSettingsOtherItems()
	{
		try
		{
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
			grdotheritems.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Qty"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdotheritems.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdotheritems.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grdotheritems);
			grdotheritems.RootTable.Columns.Add("Delete");
			grdotheritems.RootTable.Columns["Delete"].Key = "Delete";
			grdotheritems.RootTable.Columns["Delete"].Caption = "X";
			grdotheritems.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdotheritems.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdotheritems.RootTable.Columns["Delete"].ButtonText = "X";
			grdotheritems.RootTable.Columns["Delete"].Width = 20;
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
				CmbOtherItem.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)CmbOtherItem).Text = item.Cells["ItemName"].Text.ToString();
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

	private void btnoCancel_Click(object sender, EventArgs e)
	{
		btnoadd.Visible = true;
		btnoUpdate.Visible = false;
		btnoCancel.Visible = false;
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
				dtOtherItem.Rows[item.RowIndex].Delete();
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
				dtOtherItem.Rows[updateDetailIndexOther]["ItemId"] = CmbOtherItem.Value;
				dtOtherItem.Rows[updateDetailIndexOther]["ItemName"] = ((Control)(object)CmbOtherItem).Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Qty"] = txtoQty.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Rate"] = txtoRate.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Amount"] = txtoAmount.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Remarks"] = txtoRemarks.Text;
				grdotheritems.DataSource = dtOtherItem;
				btnoadd.Visible = true;
				btnoUpdate.Visible = false;
				btnoCancel.Visible = false;
				ResetOtherItems();
				CmbOtherItem.Focus();
				InsterAmountandQtyMtonInHeader();
				ProportionateOtherItemAmount();
			}
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
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c6: Expected O, but got Unknown
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac3: Expected O, but got Unknown
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Expected O, but got Unknown
		//IL_1036: Unknown result type (might be due to invalid IL or missing references)
		//IL_103d: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
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
			double OtherAmount = 0.0;
			object total = grdDetails.GetTotal(grdDetails.RootTable.Columns["Amount"], (AggregateFunction)2);
			if (grdotheritems.GetRows().Length != 0)
			{
				OtherAmount = Conversion.ToDouble(grdotheritems.GetTotal(grdotheritems.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			object Qty = grdDetails.GetTotal(grdDetails.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2);
			double QtyMton = Conversion.ToDouble((object)txtMTon.Text.Trim());
			double AmountM = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			if (Conversion.ToDouble(total) + Conversion.ToDouble((object)OtherAmount) != AmountM || Conversion.ToDouble(Qty) != QtyMton)
			{
				txtFCYAmount.Focus();
				throw new Exception("Please check FcAmount or NetWeight");
			}
			if (Conversion.ToDouble(grdPaymentTerm.GetTotal(grdPaymentTerm.RootTable.Columns["%ofTotal"], (AggregateFunction)2)) < 100.0)
			{
				MessageBox.Show("Payment Percent Not Equal To 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				tabControl2.SelectedIndex = 1;
				CmbPaymentTermDetail.Focus();
				return;
			}
			CheckBomHeaderIds(dtdetail, dtPmDetail);
			ExImLcOrder obj = new ExImLcOrder();
			if (RecId > 0)
			{
				obj.Id = RecId;
			}
			obj.Status = ((Control)(object)cmbStatus).Text;
			obj.LcOrderDocNo = Conversion.ToInt((object)txtExportSoNo.Text.Trim());
			obj.LcOrderDate = Conversion.ToDateTime((object)txtDocDate.Value);
			obj.SalesPersonId = Conversion.ToInt(CmbSalePerson.Value);
			obj.SupCustId = Conversion.ToInt(CmbCustomer.Value);
			obj.LcOrderNo = Conversion.ToString((object)txtBuyerOrderNo.Text.Trim());
			obj.SalesContratDate = Conversion.ToDateTime((object)datOrderDate.Value);
			obj.LotReference = Conversion.ToString((object)txtLotRefNo.Text.Trim());
			obj.NetWeightKgs = Conversion.ToDouble((object)txtMTon.Text.Trim());
			obj.NoOfContainers = Conversion.ToDouble((object)txtNoOfContainers.Text.Trim());
			obj.FcurrencyId = Conversion.ToInt(CmbFCYCode.Value);
			obj.ConversionRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			obj.FCurrencyAmount = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			obj.ShipmentStartDate = datShipmentStartDate.Value;
			obj.DeliveryDays = Conversion.ToInt((object)txtDeliveryDays.Text);
			obj.LastShipmentDate = Conversion.ToDateTime((object)txtShipmentDate.Value);
			obj.NotifyPartyId = Conversion.ToInt(CmbNotifyParty.Value);
			obj.DeliveryTermId = Conversion.ToInt(CmbDeliveryTerm.Value);
			obj.LoadingPortId = Conversion.ToInt(CmbLoadingPort.Value);
			obj.DestinationPortId = Conversion.ToInt(CmbDestinationPort.Value);
			obj.ImporterBankId = Conversion.ToInt(CmbImporterBank.Value);
			obj.ExporterBankId = Conversion.ToInt(CmbExporterbank.Value);
			obj.ExportCompanyId = Conversion.ToInt(CmbExportCompany.Value);
			obj.TotalAmountWithAdls = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			obj.DeliveryRemarks = Conversion.ToString((object)txtDeliveryRemarks.Text.Trim());
			obj.PaymentRemarks = Conversion.ToString((object)txtPaymentRemarks.Text.Trim());
			obj.CommodityDetial = Conversion.ToString((object)txtCommodityDetail.Text.Trim());
			obj.RemarksHeader = Conversion.ToString((object)txtSpecialInstructions.Text.Trim());
			obj.ProductSpecification = Conversion.ToString((object)txtProductSpecification.Text.Trim());
			obj.CustomerApprovalDate = ApprovalDate.Value;
			obj.CustomerApprovalRemarks = txtApprovalRemarks.Text;
			obj.FactoryLoadingDate = txtFactoryLoadingDate.Value;
			if (ChkOnCommission.Checked && FlagFin)
			{
				obj.ContractType = 1;
			}
			obj.DocumentTypeId = 202;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
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
			if (grdDetails.RowCount > 0)
			{
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow r5 in rows)
				{
					ExImLcOrderPackingDetail vd5 = new ExImLcOrderPackingDetail();
					vd5.Id = ((RecId != 0) ? Conversion.ToInt(r5.Cells["Id"].Value) : 0);
					vd5.ActionTypeId = ((vd5.Id <= 0) ? 1 : 2);
					vd5.ExImItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
					vd5.InvPackingMaterialTypeId = Conversion.ToInt(r5.Cells["PackTypeId"].Value);
					vd5.CropYearId = Conversion.ToInt(r5.Cells["CropYearId"].Value);
					vd5.NetWeight = Conversion.ToDouble(r5.Cells["Qty/M.Ton"].Value) * 1000.0;
					vd5.UOMScheduleIdOuter = Conversion.ToInt(r5.Cells["PackSizeId"].Value);
					vd5.UOMScheduleIdInner = Conversion.ToInt(r5.Cells["PackSizeId"].Value);
					vd5.NoOfUnit = Conversion.ToDouble(r5.Cells["NoOfBags"].Value);
					vd5.UOMScheduleIdRate = Conversion.ToInt(r5.Cells["RateUOMId"].Value);
					vd5.OuterQty = Conversion.ToDouble(r5.Cells["Qty/M.Ton"].Value);
					vd5.InnerQty = Conversion.ToDouble(r5.Cells["Qty/M.Ton"].Value);
					vd5.NoOfUnit = Conversion.ToDouble(r5.Cells["NoOfBags"].Value);
					vd5.RatePrice = Conversion.ToDouble(r5.Cells["Rate"].Value);
					vd5.TotalAmount = Conversion.ToDouble(r5.Cells["Amount"].Value);
					vd5.ExImFarmingNTradeId = Conversion.ToInt(r5.Cells["ExImFarmingNTradeId"].Value);
					vd5.ExImTradeTypeId = Conversion.ToInt(r5.Cells["ExImTradeTypeId"].Value);
					vd5.ExImFarmingTypeId = Conversion.ToInt(r5.Cells["ExImFarmingTypeId"].Value);
					vd5.CommodityDetail = Conversion.ToString(r5.Cells["CommodityDetail"].Value);
					if (RemarksFromDtail == "")
					{
						RemarksFromDtail = vd5.CommodityDetail;
					}
					vd5.Other_Description = Conversion.ToString(r5.Cells["Remarks"].Value);
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
							vd2.Id = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
							if (Conversion.ToInt(r2.Cells["BrandId"].Value) == 0)
							{
								throw new Exception($"Brand Item Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
							}
							if (Conversion.ToInt(r2.Cells["BrandUomId"].Value) == 0)
							{
								throw new Exception($"Brand Uom Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
							}
							if (Conversion.ToInt(r2.Cells["PackingTypeId"].Value) == 0)
							{
								throw new Exception($"Brand PackingType Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
							}
							if (Conversion.ToDouble(r2.Cells["BrandOuterQty"].Value) == 0.0)
							{
								throw new Exception($"BrandOuterQty Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
							}
							if (Conversion.ToInt(r2.Cells["PmItemId"].Value) == 0)
							{
								throw new Exception($"PmItem Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
							}
							if (Conversion.ToInt(r2.Cells["PmItemUomId"].Value) == 0)
							{
								throw new Exception($"PmItemUom Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
							}
							if (Conversion.ToInt(r2.Cells["PmItemOuterQty"].Value) == 0 && Conversion.ToInt(r2.Cells["PmItemInnerQty"].Value) == 0)
							{
								throw new Exception($"PmItemOuterQty or PmItemInnerQty Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
							}
							if (Conversion.ToDouble(r2.Cells["Rate"].Value) == 0.0)
							{
								throw new Exception($"Rate Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
							}
							if (Conversion.ToDouble(r2.Cells["Amount"].Value) == 0.0)
							{
								throw new Exception($"Amount Is Required In Packing Material Grid in Row No {r2.RowIndex + 1}");
							}
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
					obj.AttachmentsList = new List<DMSAttachments>();
					obj.DeleteAttachmentsList = new List<DMSAttachments>();
					FormHelper.UpdateAttachmentsForObject<ExImLcOrder>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
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
						FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 202, base.Name);
					}
					Reset();
					if (ChkPreview.Checked)
					{
						GenerateReport(success);
					}
					return;
				}
				throw new Exception("Payment Term Grid record not found");
			}
			throw new Exception("Detail Grid record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	public void ReadById(int ID, bool saveAs = false)
	{
		try
		{
			Reset();
			RecId = ID;
			ExImLcOrder Obj = ExImLcOrder.GetByID(ID);
			if (Obj != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				BtnSaveAs.Visible = false;
				tabControl1.SelectedIndex = 0;
				if (saveAs)
				{
					((UltraGridBase)cmbStatus).Rows[1].Activate();
				}
				else
				{
					((Control)(object)cmbStatus).Text = Obj.Status;
				}
				txtExportSoNo.Text = Conversion.ToString((object)Obj.LcOrderDocNo);
				txtDocDate.Value = Conversion.ToDateTime((object)Obj.LcOrderDate);
				CmbSalePerson.Value = Obj.SalesPersonId;
				CmbCustomer.Value = Obj.SupCustId;
				CmbNotifyParty.Value = Obj.NotifyPartyId;
				txtLotRefNo.Text = Conversion.ToString((object)Obj.LotReference);
				txtBuyerOrderNo.Text = Conversion.ToString((object)Obj.LcOrderNo);
				datOrderDate.Value = Conversion.ToDateTime((object)Obj.SalesContratDate);
				datShipmentStartDate.Value = Conversion.ToDateTime((object)Obj.ShipmentStartDate);
				txtShipmentDate.Value = Conversion.ToDateTime((object)Obj.LastShipmentDate);
				CmbDeliveryTerm.Value = Obj.DeliveryTermId;
				CmbLoadingPort.Value = Obj.LoadingPortId;
				CmbDestinationPort.Value = Obj.DestinationPortId;
				CmbImporterBank.Value = Obj.ImporterBankId;
				CmbExporterbank.Value = Obj.ExporterBankId;
				CmbExportCompany.Value = Obj.ExportCompanyId;
				txtNoOfContainers.Text = Conversion.ToString((object)Obj.NoOfContainers);
				CmbFCYCode.Value = Obj.FcurrencyId;
				txtFCYAmount.Text = Obj.FCurrencyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtMTon.Text = Conversion.ToString((object)Obj.NetWeightKgs);
				txtDeliveryRemarks.Text = Conversion.ToString((object)Obj.DeliveryRemarks);
				txtPaymentRemarks.Text = Conversion.ToString((object)Obj.PaymentRemarks);
				txtCommodityDetail.Text = Conversion.ToString((object)Obj.CommodityDetial);
				txtSpecialInstructions.Text = Conversion.ToString((object)Obj.RemarksHeader);
				txtProductSpecification.Text = Conversion.ToString((object)Obj.ProductSpecification);
				txtExchangeRate.Text = Obj.ConversionRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtDeliveryDays.Text = Obj.DeliveryDays.ToString();
				ApprovalDate.Value = Conversion.ToDateTime((object)Obj.CustomerApprovalDate);
				txtApprovalRemarks.Text = Obj.CustomerApprovalRemarks;
				txtFactoryLoadingDate.Value = Conversion.ToDateTime((object)Obj.FactoryLoadingDate);
				CustomAttachmentsValues = Obj.CustomAttachmentsValues;
				AttachmentsValues = Obj.AttachmentsValues;
				CmbDocumentCustomGroup.Value = Obj.CustomGroupId;
				if (Obj.ContractType == 1)
				{
					ChkOnCommission.Checked = true;
				}
				lstRemoveRecord.Clear();
				dtdetail.Rows.Clear();
				for (int l = 0; l < Obj.LcOrderDetail.Count; l++)
				{
					dtdetail.Rows.Add(Obj.LcOrderDetail[l].Id, Obj.LcOrderDetail[l].ItemCode, Obj.LcOrderDetail[l].ExImItemId, Obj.LcOrderDetail[l].ItemName, Obj.LcOrderDetail[l].InvPackingMaterialTypeId, Obj.LcOrderDetail[l].ExImPackMaterialTypeName, Obj.LcOrderDetail[l].CropYearId, Obj.LcOrderDetail[l].CropYear, Obj.LcOrderDetail[l].NetWeight / 1000.0, Obj.LcOrderDetail[l].UOMScheduleIdOuter, Obj.LcOrderDetail[l].PackUom, Obj.LcOrderDetail[l].QtyEquivalent, Obj.LcOrderDetail[l].NoOfUnit, Obj.LcOrderDetail[l].RatePrice, Obj.LcOrderDetail[l].UOMScheduleIdRate, Obj.LcOrderDetail[l].RateUom, Obj.LcOrderDetail[l].TotalAmount, 0, Obj.LcOrderDetail[l].CommodityDetail, Obj.LcOrderDetail[l].ExImFarmingNTradeId, Obj.LcOrderDetail[l].FarmingNTrade, Obj.LcOrderDetail[l].ExImFarmingTypeId, Obj.LcOrderDetail[l].ExImTradeTypeId, Obj.LcOrderDetail[l].Other_Description);
				}
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
				BindBrandItemsFromOutPutGrid();
				dtOtherItem.Rows.Clear();
				for (int k = 0; k < Obj.ExImLcOrderOtherItemslist.Count; k++)
				{
					dtOtherItem.Rows.Add(Obj.ExImLcOrderOtherItemslist[k].otherItemId, Obj.ExImLcOrderOtherItemslist[k].ItemName, Obj.ExImLcOrderOtherItemslist[k].oItemQty, Obj.ExImLcOrderOtherItemslist[k].oItemRate, Obj.ExImLcOrderOtherItemslist[k].oItemAmount, Obj.ExImLcOrderOtherItemslist[k].OtherItemRemarks);
				}
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				dtPaymnent.Rows.Clear();
				for (int j = 0; j < Obj.ExImLcOrderPaymentTermsDetailslist.Count; j++)
				{
					dtPaymnent.Rows.Add(Obj.ExImLcOrderPaymentTermsDetailslist[j].PaymentTermId, Obj.ExImLcOrderPaymentTermsDetailslist[j].LcOrderTerm, Obj.ExImLcOrderPaymentTermsDetailslist[j].PrcntOfTotal, Obj.ExImLcOrderPaymentTermsDetailslist[j].FcyAmount, Obj.ExImLcOrderPaymentTermsDetailslist[j].Remarks);
				}
				grdPaymentTerm.DataSource = dtPaymnent;
				grdPaymentTerm.RetrieveStructure();
				grdSettingsPaymentTerm();
				dtPmDetail.Rows.Clear();
				for (int i = 0; i < Obj.ExImLcContractPackingMaterialDetailslist.Count; i++)
				{
					dtPmDetail.Rows.Add(Obj.ExImLcContractPackingMaterialDetailslist[i].Id, Obj.ExImLcContractPackingMaterialDetailslist[i].BrandId, Obj.ExImLcContractPackingMaterialDetailslist[i].BrandCode, Obj.ExImLcContractPackingMaterialDetailslist[i].BrandName, Obj.ExImLcContractPackingMaterialDetailslist[i].BrandUomId, Obj.ExImLcContractPackingMaterialDetailslist[i].BrandUomCode, Obj.ExImLcContractPackingMaterialDetailslist[i].PackingTypeId, Obj.ExImLcContractPackingMaterialDetailslist[i].PackingType, Obj.ExImLcContractPackingMaterialDetailslist[i].BrandOuterQty, Obj.ExImLcContractPackingMaterialDetailslist[i].BrandInnerQty, Obj.ExImLcContractPackingMaterialDetailslist[i].ItemId, Obj.ExImLcContractPackingMaterialDetailslist[i].ItemCode, Obj.ExImLcContractPackingMaterialDetailslist[i].ItemName, Obj.ExImLcContractPackingMaterialDetailslist[i].ItemUomId, Obj.ExImLcContractPackingMaterialDetailslist[i].ItemUomCode, Obj.ExImLcContractPackingMaterialDetailslist[i].Qty, Obj.ExImLcContractPackingMaterialDetailslist[i].InnerQty, Obj.ExImLcContractPackingMaterialDetailslist[i].Rate, Obj.ExImLcContractPackingMaterialDetailslist[i].Amount, Obj.ExImLcContractPackingMaterialDetailslist[i].pmRemarks);
				}
				grdPackingMaterialDetail.DataSource = dtPmDetail;
				grdPackingMaterialDetail.RetrieveStructure();
				grdSettingsPM();
				dtOtherChargesDetail.Rows.Clear();
				FillOtherChargesDetailFromListCommonForReadById(Obj, dtOtherChargesDetail);
				grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
				grdOtherChargesDetail.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesDetail);
				ProportionateOtherItemAmount();
				if (!saveAs)
				{
					FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				}
				if (ShipmentDocumentsFeature)
				{
					DocumentcustomGroupBind();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillOtherChargesDetailFromListCommonForReadById(ExImLcOrder lcOrder, DataTable dt)
	{
		if (lcOrder == null)
		{
			return;
		}
		List<ExImLcOrderOtherChargesDetail> details = lcOrder.ExImLcOrderOtherChargesDetailList;
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

	private void txtLcOrderNo_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImLcOrder.GetIdByDocNo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				OrderNo = Conversion.ToString((object)txtExportSoNo.Text.Trim()),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0)
			{
				RecId = Conversion.ToInt(dt.Rows[0]["Id"]);
				ReadById(Conversion.ToInt((object)RecId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FrmExportSalesContract_FormClosing(object sender, FormClosingEventArgs e)
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierFill();
			salesPersonFill();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			MultiCurrencyfill();
			ItemDetailFill();
			ItemPackingTypeFill();
			PackingItemFill();
			BindOtherItems();
			CropYearFill();
			ItemSpecificationsBind();
			OtherChargesBind();
			if (ShipmentDocumentsFeature)
			{
				DocumentcustomGroupBind();
			}
			PMGridInfoShouldNotCompulsoryOnContract = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PMGridInfoShouldNotCompulsoryOnContract"));
			if (AllowExportMultiCompanies)
			{
				ExportCompaniesByCompanyIdBind();
			}
			dtItemAndPMItemMap = ItemAndPMItemMap.ItemAndPMItemMap_GetByItemIds(UserAccount.CompanyId, UserAccount.OrganizationId, (string)null);
			FarmingNTradeBind();
			if (Conversion.ToInt(CmbSalePerson.Value) == 0 && dtLastSavedRecordHeader.Rows.Count > 0)
			{
				CmbSalePerson.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["SalesPersonId"]);
				CmbFCYCode.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["FcurrencyId"]);
				CmbCropYear.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["CropYearId"]);
				combpcktype.Value = Conversion.ToInt(dtLastSavedRecordHeader.Rows[0]["InvPackingMaterialTypeId"]);
			}
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
			CustomAttachmentsValues = "";
			AttachmentsValues = "";
			RecId = 0;
			GenerateCode();
			ChkOnCommission.Checked = false;
			((Control)(object)CmbSalePerson).Text = string.Empty;
			txtPaymentRemarks.Text = string.Empty;
			txtDeliveryRemarks.Text = string.Empty;
			((Control)(object)CmbCustomer).Text = string.Empty;
			((Control)(object)CmbNotifyParty).Text = string.Empty;
			((Control)(object)CmbFCYCode).Text = string.Empty;
			txtFCYAmount.Text = string.Empty;
			txtSpecialInstructions.Text = string.Empty;
			txtMTon.Text = string.Empty;
			txtCommodityDetail.Text = string.Empty;
			((Control)(object)CmbDeliveryTerm).Text = string.Empty;
			((Control)(object)CmbPaymentTerms).Text = string.Empty;
			((Control)(object)CmbLoadingPort).Text = string.Empty;
			((Control)(object)CmbDestinationPort).Text = string.Empty;
			((Control)(object)CmbImporterBank).Text = string.Empty;
			((Control)(object)CmbExporterbank).Text = string.Empty;
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			((Control)(object)combpcktype).Text = string.Empty;
			txtQtyMTon.Text = string.Empty;
			txtBuyerOrderNo.Text = string.Empty;
			txtLotRefNo.Text = string.Empty;
			txtNoOfContainers.Text = string.Empty;
			txtProductSpecification.Text = string.Empty;
			((Control)(object)cmbStatus).Text = string.Empty;
			dtdetail.Rows.Clear();
			grdotheritems.ClearStructure();
			dtPaymnent.Rows.Clear();
			dtPmDetail.Rows.Clear();
			dtBrandForPMItem.Clear();
			((UltraGridBase)cmbBrandForPmEntry).DataSource = null;
			ResetPaymentDetail();
			ResetPmDetail();
			txtExchangeRate.Text = string.Empty;
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
			ResetOtherChargesDetail();
			dtOtherChargesDetail.Clear();
			grdOtherChargesDetail.DataSource = dtOtherChargesDetail;
			grdOtherChargesDetail.RetrieveStructure();
			grdSettingsOtherChargesDetail(grdOtherChargesDetail);
			txtApprovalRemarks.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnDelete.Visible = false;
			BtnSaveAs.Visible = false;
			btnAddPayment.Visible = true;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
			btnPackingAdd.Visible = true;
			btnPackingUpdate.Visible = false;
			btnPackingCancel.Visible = false;
			txtDocDate.Focus();
			tabControl2.SelectedIndex = 0;
			updateDetailIndex = -1;
			updateDetailIndexOther = -1;
			updateDetailIndexPmDetail = -1;
			updateDetailIndexPaymentDetail = -1;
			updateDetailIndexOtherCharges = -1;
			dtLastSavedRecordHeader = ExImLcOrder.LastSavedRecord(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
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
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combpcktype).Text = string.Empty;
			txtQtyMTon.Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			((Control)(object)CmbCropYear).Text = string.Empty;
			((Control)(object)cmbcomditiyDetailByItemCus).Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtCostMTon.Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)CmbFarmingNTrade).Text = string.Empty;
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
			((Control)(object)CmbPaymentTermDetail).Text = string.Empty;
			txtPaymentRemarksDetail.Text = string.Empty;
			PercntOfTotal.Text = string.Empty;
			txtPaymentDetailFcyAmount.Text = string.Empty;
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
			((Control)(object)cmbBrandForPmEntry).Text = string.Empty;
			((UltraGridBase)CmbBrandUomPm).DataSource = null;
			((Control)(object)CmbBrandUomPm).Text = "";
			((UltraGridBase)CmbBrandPackingTypePm).DataSource = null;
			((Control)(object)CmbBrandPackingTypePm).Text = "";
			txtBrandOuterQtyPm.Text = 0.ToString("#,##.##");
			txtBrandInnerQtyPm.Text = 0.ToString("#,##.##");
			((Control)(object)CmbPMItem).Text = string.Empty;
			((Control)(object)CmbUomPm).Text = string.Empty;
			txtOuterQtyPm.Text = string.Empty;
			txtInnerQtyPm.Text = string.Empty;
			txtPackingItemRate.Text = string.Empty;
			txtPackingAmount.Text = string.Empty;
			txtPackingRemarks.Text = string.Empty;
			cmbBrandForPmEntry.Focus();
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierFill();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			MultiCurrencyfill();
			ItemDetailFill();
			ItemPackingTypeFill();
			BindOtherItems();
			CropYearFill();
			ItemSpecificationsBind();
			if (ShipmentDocumentsFeature)
			{
				DocumentcustomGroupBind();
			}
			if (AllowExportMultiCompanies)
			{
				ExportCompaniesByCompanyIdBind();
			}
			FarmingNTradeBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			obj.DocumentTypeIds = "202";
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
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("OrderNo", typeof(string));
				dtcol.Columns.Add("OrderDate", typeof(DateTime));
				dtcol.Columns.Add("CustomerName", typeof(string));
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
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LcOrderDocNo"], Conversion.ToDateTime(dt.Rows[i]["SalesContratDate"]).ToShortDateString(), dt.Rows[i]["LcOrderNo"], Conversion.ToDateTime(dt.Rows[i]["LcOrderDate"]).ToShortDateString(), dt.Rows[i]["CustomerName"], dt.Rows[i]["LotReference"], dt.Rows[i]["NetKgs"], dt.Rows[i]["NoOfContr"], dt.Rows[i]["CurrencyCode"], dt.Rows[i]["FCurrencyAmount"], Conversion.ToDateTime(dt.Rows[i]["LastShipmentDate"]).ToShortDateString(), dt.Rows[i]["NotifyParty"], dt.Rows[i]["DeliveryTerm"], dt.Rows[i]["PaymentTerm"], dt.Rows[i]["LoadingPort"], dt.Rows[i]["PortName"], dt.Rows[i]["Commodity_Description"], dt.Rows[i]["EntryUserName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["ModifyUserName"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
				grddetailhistory.ClearStructure();
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
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 3, 2);
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["M.Tons"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["M.Tons"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["M.Tons"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["M.Tons"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			DataGridHistory.RootTable.Columns["Commodity_Description"].Width = 300;
			GridEX_Helper.AddButton(DataGridHistory, "Print", "Print", 50, 0);
			GridEX_Helper.AddButton(DataGridHistory, "Print501A", "Print501A", 65, 1);
			GridEX_Helper.AddButton(DataGridHistory, "PrintII", "PrintII", 50, 2);
			GridEX_Helper.AddButton(DataGridHistory, "Print523", "Print523", 60, 3);
			GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 40, 4);
			GridEX_Helper.AddButton(DataGridHistory, "SaveAs", "SaveAs", 65, 5);
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

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (int)r.RowType != 5 && r.Cells.Count > 0)
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
			if (Obj != null && Obj.LcOrderDetail.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackTypeId");
				dt.Columns.Add("PackType");
				dt.Columns.Add("CropYearId");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("Qty/M.Ton", typeof(double));
				dt.Columns.Add("PackSizeId");
				dt.Columns.Add("PackSize");
				dt.Columns.Add("NoOfBags", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("RateUOMId");
				dt.Columns.Add("RateUOM");
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("OtherItemAmount", typeof(double));
				dt.Columns.Add("CommodityDetail");
				dt.Columns.Add("FarmingNTrade");
				dt.Columns.Add("Remarks");
				dt.Clear();
				for (int l = 0; l < Obj.LcOrderDetail.Count; l++)
				{
					dt.Rows.Add(Obj.LcOrderDetail[l].Id, Obj.LcOrderDetail[l].ItemCode, Obj.LcOrderDetail[l].ExImItemId, Obj.LcOrderDetail[l].ItemName, Obj.LcOrderDetail[l].InvPackingMaterialTypeId, Obj.LcOrderDetail[l].ExImPackMaterialTypeName, Obj.LcOrderDetail[l].CropYearId, Obj.LcOrderDetail[l].CropYear, Obj.LcOrderDetail[l].NetWeight / 1000.0, Obj.LcOrderDetail[l].UOMScheduleIdOuter, Obj.LcOrderDetail[l].PackUom, Obj.LcOrderDetail[l].NoOfUnit, Obj.LcOrderDetail[l].RatePrice, Obj.LcOrderDetail[l].UOMScheduleIdRate, Obj.LcOrderDetail[l].RateUom, Obj.LcOrderDetail[l].TotalAmount, 0, Obj.LcOrderDetail[l].CommodityDetail, Obj.LcOrderDetail[l].FarmingNTrade, Obj.LcOrderDetail[l].Other_Description);
				}
				grddetailhistory.DataSource = dt;
				grddetailhistory.RetrieveStructure();
				grddetailhistorySettings();
				DataTable dtPaymentH = new DataTable();
				dtPaymentH.Columns.Add("PaymentTerm");
				dtPaymentH.Columns.Add("%ofTotal", typeof(double));
				dtPaymentH.Columns.Add("FcyAmount", typeof(double));
				dtPaymentH.Columns.Add("Remarks");
				dtPaymentH.Clear();
				for (int k = 0; k < Obj.ExImLcOrderPaymentTermsDetailslist.Count; k++)
				{
					dtPaymentH.Rows.Add(Obj.ExImLcOrderPaymentTermsDetailslist[k].LcOrderTerm, Obj.ExImLcOrderPaymentTermsDetailslist[k].PrcntOfTotal, Obj.ExImLcOrderPaymentTermsDetailslist[k].FcyAmount, Obj.ExImLcOrderPaymentTermsDetailslist[k].Remarks);
				}
				grdPaymentHistory.DataSource = dtPaymentH;
				grdPaymentHistory.RetrieveStructure();
				grdSettingsPaymentHistory();
				DataTable dtPmH = new DataTable();
				dtPmH.Columns.Add("Id", typeof(int));
				dtPmH.Columns.Add("BrandId", typeof(int));
				dtPmH.Columns.Add("BrandCode");
				dtPmH.Columns.Add("BrandName");
				dtPmH.Columns.Add("BrandUomId", typeof(int));
				dtPmH.Columns.Add("BrandUom");
				dtPmH.Columns.Add("PackingTypeId", typeof(int));
				dtPmH.Columns.Add("PackingType");
				dtPmH.Columns.Add("BrandOuterQty", typeof(double));
				dtPmH.Columns.Add("BrandInnerQty", typeof(double));
				dtPmH.Columns.Add("PmItemId", typeof(int));
				dtPmH.Columns.Add("PmItemCode");
				dtPmH.Columns.Add("PmItemName");
				dtPmH.Columns.Add("PmItemUomId", typeof(int));
				dtPmH.Columns.Add("PmItemUom");
				dtPmH.Columns.Add("PmItemOuterQty", typeof(double));
				dtPmH.Columns.Add("PmItemInnerQty", typeof(double));
				dtPmH.Columns.Add("Rate", typeof(double));
				dtPmH.Columns.Add("Amount", typeof(double));
				dtPmH.Columns.Add("Remarks");
				dtPmH.Clear();
				for (int j = 0; j < Obj.ExImLcContractPackingMaterialDetailslist.Count; j++)
				{
					dtPmH.Rows.Add(Obj.ExImLcContractPackingMaterialDetailslist[j].Id, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandId, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandCode, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandName, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandUomId, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandUomCode, Obj.ExImLcContractPackingMaterialDetailslist[j].PackingTypeId, Obj.ExImLcContractPackingMaterialDetailslist[j].PackingType, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandOuterQty, Obj.ExImLcContractPackingMaterialDetailslist[j].BrandInnerQty, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemId, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemCode, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemName, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemUomId, Obj.ExImLcContractPackingMaterialDetailslist[j].ItemUomCode, Obj.ExImLcContractPackingMaterialDetailslist[j].Qty, Obj.ExImLcContractPackingMaterialDetailslist[j].InnerQty, Obj.ExImLcContractPackingMaterialDetailslist[j].Rate, Obj.ExImLcContractPackingMaterialDetailslist[j].Amount, Obj.ExImLcContractPackingMaterialDetailslist[j].pmRemarks);
				}
				grdPMHistory.DataSource = dtPmH;
				grdPMHistory.RetrieveStructure();
				grdSettingsPMHistory();
				DataTable dtOtherH = new DataTable();
				dtOtherH.Columns.Add("ItemName");
				dtOtherH.Columns.Add("Qty", typeof(double));
				dtOtherH.Columns.Add("Rate", typeof(double));
				dtOtherH.Columns.Add("Amount", typeof(double));
				dtOtherH.Columns.Add("Remarks");
				dtOtherH.Clear();
				for (int i = 0; i < Obj.ExImLcOrderOtherItemslist.Count; i++)
				{
					dtOtherH.Rows.Add(Obj.ExImLcOrderOtherItemslist[i].ItemName, Obj.ExImLcOrderOtherItemslist[i].oItemQty, Obj.ExImLcOrderOtherItemslist[i].oItemRate, Obj.ExImLcOrderOtherItemslist[i].oItemAmount, Obj.ExImLcOrderOtherItemslist[i].OtherItemRemarks);
				}
				grdOtherItemHistory.DataSource = dtOtherH;
				grdOtherItemHistory.RetrieveStructure();
				grdSettingsOtherItemHistory();
				DataTable dtOCH = dtOtherChargesDetail.Clone();
				FillOtherChargesDetailFromListCommonForReadById(Obj, dtOCH);
				grdOtherChargesHistory.DataSource = dtOCH;
				grdOtherChargesHistory.RetrieveStructure();
				grdSettingsOtherChargesDetail(grdOtherChargesHistory);
			}
			else
			{
				grddetailhistory.ClearStructure();
				grdPaymentHistory.ClearStructure();
				grdPMHistory.ClearStructure();
				grdOtherItemHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentHistory()
	{
		try
		{
			grdPaymentHistory.RootTable.Columns["%ofTotal"].TextAlignment = (TextAlignment)3;
			grdPaymentHistory.RootTable.Columns["%ofTotal"].FormatString = "#,##0.###";
			grdPaymentHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdPaymentHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentHistory.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdPaymentHistory.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			GridEX_Helper.GridWrappingAndColumnSettings(grdPaymentHistory, 2, 2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPMHistory()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "BrandId", "BrandUomId", "PackingTypeId", "PmItemId", "PmItemUomId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPMHistory, 2, 2, hiddenColumns);
			grdPMHistory.RootTable.Columns["BrandOuterQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["BrandOuterQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["BrandOuterQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["BrandInnerQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["BrandInnerQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["BrandInnerQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["BrandInnerQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemOuterQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].FormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["PmItemInnerQty"].TotalFormatString = "#,##0.###";
			grdPMHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPMHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPMHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPMHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPMHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsOtherItemHistory()
	{
		try
		{
			grdOtherItemHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdOtherItemHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdOtherItemHistory.RootTable.Columns["Qty"].FormatString = "0,0";
			grdOtherItemHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdOtherItemHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOtherItemHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOtherItemHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdOtherItemHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdOtherItemHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOtherItemHistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridEX_Helper.GridWrappingAndColumnSettings(grdOtherItemHistory, 2, 2);
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
			grddetailhistory.RootTable.Columns["Qty/M.Ton"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["Qty/M.Ton"].FormatString = "0,0";
			grddetailhistory.RootTable.Columns["Qty/M.Ton"].TotalFormatString = "#,##0.##";
			grddetailhistory.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grddetailhistory.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["NoOfBags"].FormatString = "0,0";
			grddetailhistory.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grddetailhistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grddetailhistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grddetailhistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grddetailhistory.RootTable.Columns["OtherItemAmount"].AggregateFunction = (AggregateFunction)2;
			grddetailhistory.RootTable.Columns["OtherItemAmount"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["OtherItemAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grddetailhistory.RootTable.Columns["OtherItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grddetailhistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grddetailhistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			List<string> hiddenColumns = new List<string> { "Id", "ItemId", "CropYearId", "PackTypeId", "PackSizeId", "RateUOMId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grddetailhistory, 2, 2, hiddenColumns);
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
			HistoryCombosFill();
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

	private void HistoryCombosFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dt = ExImLcOrder.GetDataForDropDownFromExportContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "Customer"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				else if (e.Column.Key == "Edit")
				{
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
				}
				else if (e.Column.Key == "SaveAs")
				{
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId, saveAs: true);
					txtBuyerOrderNo.Text = "";
					datShipmentStartDate.Value = DateTime.Now;
					txtDocDate.Value = DateTime.Now;
					datOrderDate.Value = DateTime.Now;
					ApprovalDate.Value = DateTime.Now;
					BtnSaveAs.Visible = true;
					btnsave.Visible = false;
					btnUpdate.Visible = false;
					BtnDelete.Visible = false;
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

	private void GenerateReport(int id)
	{
		try
		{
			CommonServices.ExportSalesContractExport501(id);
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
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 202);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnImporterBank_Click(object sender, EventArgs e)
	{
		new AcfrmDefineBank(UserAccount).Show();
	}

	private void btnExporterBank_Click(object sender, EventArgs e)
	{
		new AcfrmDefineBank(UserAccount).Show();
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
					dtPaymnent.Rows[item.RowIndex].Delete();
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

	private void FrmExportSalesContract_KeyDown(object sender, KeyEventArgs e)
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
					toolStripButton1_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						combitem.Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						CmbPaymentTermDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						CmbPMItem.Focus();
					}
					else if (tabControl2.SelectedIndex == 3)
					{
						CmbOtherItem.Focus();
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
						CmbPaymentTermDetail.Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 2;
						CmbPMItem.Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						tabControl2.SelectedIndex = 3;
						CmbOtherItem.Focus();
					}
					else if (tabControl2.SelectedIndex == 3)
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdDetails.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetails_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdDetails.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndex != -1)
					{
						throw new Exception("Reset  the Detail first...");
					}
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ExImLcOrderPackingDetail vd = new ExImLcOrderPackingDetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.ExImItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.InvPackingMaterialTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
						vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
						vd.NetWeight = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
						vd.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["PackSizeId"].Value);
						vd.UOMScheduleIdInner = Conversion.ToInt(r.Cells["PackSizeId"].Value);
						vd.NoOfUnit = Conversion.ToDouble(r.Cells["NoOfBags"].Value);
						vd.UOMScheduleIdRate = Conversion.ToInt(r.Cells["RateUOMId"].Value);
						vd.OuterQty = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
						vd.InnerQty = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
						vd.NoOfUnit = Conversion.ToDouble(r.Cells["NoOfBags"].Value);
						vd.RatePrice = Conversion.ToDouble(r.Cells["Rate"].Value);
						vd.TotalAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
						vd.CommodityDetail = Conversion.ToString(r.Cells["CommodityDetail"].Value);
						vd.ExImFarmingNTradeId = Conversion.ToInt(r.Cells["ExImFarmingNTradeId"].Value);
						vd.ExImTradeTypeId = Conversion.ToInt(r.Cells["ExImTradeTypeId"].Value);
						vd.ExImFarmingTypeId = Conversion.ToInt(r.Cells["ExImFarmingTypeId"].Value);
						vd.Other_Description = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						dtdetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
					else
					{
						r.Delete();
					}
				}
				grdDetails.UpdateData();
				ProportionateOtherItemAmount();
				InsterAmountandQtyMtonInHeader();
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
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
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
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
				}
				if (DataGridHistory.CurrentColumn.Key == "SaveAs")
				{
					RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
					ReadById(RecId, saveAs: true);
					txtBuyerOrderNo.Text = "";
					BtnSaveAs.Visible = true;
					btnsave.Visible = false;
					btnUpdate.Visible = false;
					BtnDelete.Visible = false;
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
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 202);
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

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 2)
			{
				BindBrandItemsFromOutPutGrid();
				cmbBrandForPmEntry.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			double MTon = 0.0;
			double NoOfBags = 0.0;
			double OuterUom = 0.0;
			MTon = Conversion.ToDouble((object)txtQtyMTon.Text);
			NoOfBags = Conversion.ToDouble((object)txtNoOfBags.Text);
			if (((UltraGridBase)combitempck).ActiveRow != null)
			{
				OuterUom = Conversion.ToDouble(((UltraDropDownBase)combitempck).SelectedRow.Cells[2].Value);
				if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "MTon")
				{
					if (MTon > 0.0 && OuterUom > 0.0)
					{
						double noofbags = MTon * 1000.0 / OuterUom;
						txtNoOfBags.Text = Math.Round(noofbags).ToString("#,##0.##");
					}
					else
					{
						txtNoOfBags.Text = "0";
					}
				}
				else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "NoofBags")
				{
					if (NoOfBags > 0.0 && OuterUom > 0.0)
					{
						MTon = NoOfBags * OuterUom / 1000.0;
						txtQtyMTon.Text = MTon.ToString("#,#.######");
					}
					else
					{
						txtQtyMTon.Text = "0";
					}
				}
				else if (MTon > 0.0 && OuterUom > 0.0)
				{
					double noofbags2 = MTon * 1000.0 / OuterUom;
					txtNoOfBags.Text = Math.Round(noofbags2).ToString("#,##0.##");
				}
			}
			else
			{
				txtNoOfBags.Text = 0.ToString();
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
			double NetWeight = 0.0;
			txtAmount.Text = 0.ToString();
			if (((UltraGridBase)combrateuom).ActiveRow != null && Conversion.ToInt(combrateuom.Value) > 0)
			{
				ItemRate = Conversion.ToDouble((object)txtCostMTon.Text);
				ItemRateUom = Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value);
				NetWeight = Conversion.ToDouble((object)txtQtyMTon.Text);
				if (ItemRate > 0.0 && ItemRateUom > 0.0 && NetWeight > 0.0)
				{
					double weightkgs = NetWeight * 1000.0;
					double amount = ItemRate / ItemRateUom * weightkgs;
					txtAmount.Text = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
			}
			else
			{
				txtAmount.Text = 0.ToString();
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
			double prcnt = Conversion.ToDouble((object)PercntOfTotal.Text);
			double fcyAmount = Conversion.ToDouble((object)txtFCYAmount.Text);
			if (prcnt > 100.0)
			{
				PercntOfTotal.Text = "0";
				txtPaymentDetailFcyAmount.Text = "0";
				MessageBox.Show("%age Can't be Greater Than 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (fcyAmount > 0.0)
			{
				double amount = prcnt * fcyAmount / 100.0;
				txtPaymentDetailFcyAmount.Text = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtPaymentDetailFcyAmount.Text = "0";
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
			double Qty = Conversion.ToDouble((object)txtInnerQtyPm.Text);
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
			Qty = ((!(Conversion.ToDouble((object)txtOuterQtyPm.Text) > 0.0)) ? Conversion.ToDouble((object)txtInnerQtyPm.Text) : Conversion.ToDouble((object)txtOuterQtyPm.Text));
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
			if (Qty > 0.0 && Conversion.ToDouble((object)txtPackingItemRate.Text) > 0.0)
			{
				double amount = Qty * Conversion.ToDouble((object)txtPackingItemRate.Text);
				txtPackingAmount.Text = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtPackingAmount.Text = "0";
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
			double Qty = Conversion.ToDouble((object)txtOuterQtyPm.Text);
			CalculatePackingAmount(Qty);
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

	private void datOrderDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			ApprovalDate.Value = datOrderDate.Value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AutoFillGridFromMapping()
	{
		try
		{
			DataTable dataTable = dtItemAndPMItemMap;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				throw new Exception("There are no items to map with Packing Material");
			}
			DataTable dataTable2 = dtBrandForPMItem;
			if (dataTable2 != null && dataTable2.Rows.Count == 0)
			{
				throw new Exception("No Brand Items found");
			}
			EnumerableRowCollection<DataRow> itemMapRows = dtItemAndPMItemMap.AsEnumerable();
			EnumerableRowCollection<DataRow> existingDetails = dtPmDetail.AsEnumerable();
			foreach (DataRow item in dtBrandForPMItem.Rows)
			{
				int brandId = Conversion.ToInt(item["Id"]);
				foreach (DataRow row2 in itemMapRows.Where((DataRow row) => row.Field<int>("ItemId") == brandId).ToList())
				{
					int pmItemId = row2.Field<int>("PmItemId");
					if (!existingDetails.Any((DataRow r) => r.Field<int>("BrandId") == brandId && r.Field<int>("PmItemId") == pmItemId))
					{
						dtPmDetail.Rows.Add(0, brandId, item["ItemCode"], item["ItemName"], item["PackUomId"], item["PackUom"], item["PackingTypeId"], item["PackingType"], item["OuterQty"], item["InnerQty"], pmItemId, row2.Field<string>("PmItemCode"), row2.Field<string>("PmItemName"), row2.Field<int>("PmBaseUomId"), row2.Field<string>("PmBaseUomCode"), item["OuterQty"], 0.0, 0.0, 0.0, "");
					}
				}
			}
			grdPackingMaterialDetail.DataSource = dtPmDetail;
			grdPackingMaterialDetail.RetrieveStructure();
			grdSettingsPM();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error in AutoFillGridFromMapping: " + ex.Message);
		}
	}

	private void btnAutoMap_Click(object sender, EventArgs e)
	{
		try
		{
			AutoFillGridFromMapping();
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
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Expected O, but got Unknown
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Expected O, but got Unknown
		//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b54: Expected O, but got Unknown
		//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5f: Expected O, but got Unknown
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Expected O, but got Unknown
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Expected O, but got Unknown
		//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b80: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Expected O, but got Unknown
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cab: Expected O, but got Unknown
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Expected O, but got Unknown
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Expected O, but got Unknown
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Expected O, but got Unknown
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d92: Expected O, but got Unknown
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df5: Expected O, but got Unknown
		//IL_0e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8f: Expected O, but got Unknown
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efd: Expected O, but got Unknown
		//IL_0f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1e: Expected O, but got Unknown
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fce: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
		//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10aa: Expected O, but got Unknown
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_114f: Expected O, but got Unknown
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_1170: Expected O, but got Unknown
		//IL_11c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d3: Expected O, but got Unknown
		//IL_141b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1425: Expected O, but got Unknown
		//IL_14cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d5: Expected O, but got Unknown
		//IL_14ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f6: Expected O, but got Unknown
		//IL_14f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1501: Expected O, but got Unknown
		//IL_1502: Unknown result type (might be due to invalid IL or missing references)
		//IL_150c: Expected O, but got Unknown
		//IL_150d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1517: Expected O, but got Unknown
		//IL_152e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1538: Expected O, but got Unknown
		//IL_1544: Unknown result type (might be due to invalid IL or missing references)
		//IL_154e: Expected O, but got Unknown
		//IL_154f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1559: Expected O, but got Unknown
		//IL_1570: Unknown result type (might be due to invalid IL or missing references)
		//IL_157a: Expected O, but got Unknown
		//IL_157b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1585: Expected O, but got Unknown
		//IL_15c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d2: Expected O, but got Unknown
		//IL_15e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f3: Expected O, but got Unknown
		//IL_16e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f0: Expected O, but got Unknown
		//IL_1707: Unknown result type (might be due to invalid IL or missing references)
		//IL_1711: Expected O, but got Unknown
		//IL_1733: Unknown result type (might be due to invalid IL or missing references)
		//IL_173d: Expected O, but got Unknown
		//IL_175f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1769: Expected O, but got Unknown
		//IL_178b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Expected O, but got Unknown
		//IL_1846: Unknown result type (might be due to invalid IL or missing references)
		//IL_1850: Expected O, but got Unknown
		//IL_18eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f5: Expected O, but got Unknown
		//IL_22e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_22eb: Expected O, but got Unknown
		//IL_5b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b84: Expected O, but got Unknown
		//IL_6e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e84: Expected O, but got Unknown
		//IL_9a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_9a38: Expected O, but got Unknown
		//IL_aa22: Unknown result type (might be due to invalid IL or missing references)
		//IL_aa2c: Expected O, but got Unknown
		//IL_130b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_130ba: Expected O, but got Unknown
		//IL_130de: Unknown result type (might be due to invalid IL or missing references)
		//IL_130e8: Expected O, but got Unknown
		GridEXLayout grdDetails_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance293 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
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
		Appearance appearance191 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		GridEXLayout grdPaymentTerm_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.FrmExportSalesContract));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grddetailhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPMHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherItemHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance315 = new Appearance();
		GridEXLayout grdOtherChargesHistory_DesignTimeLayout = new GridEXLayout();
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
		this.label107 = new System.Windows.Forms.Label();
		this.label106 = new System.Windows.Forms.Label();
		this.label105 = new System.Windows.Forms.Label();
		this.label104 = new System.Windows.Forms.Label();
		this.label103 = new System.Windows.Forms.Label();
		this.label102 = new System.Windows.Forms.Label();
		this.label101 = new System.Windows.Forms.Label();
		this.label100 = new System.Windows.Forms.Label();
		this.btnplus = new System.Windows.Forms.Button();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.CmbFarmingNTrade = new UltraCombo();
		this.label69 = new System.Windows.Forms.Label();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.panel23 = new System.Windows.Forms.Panel();
		this.label45 = new System.Windows.Forms.Label();
		this.cmbcomditiyDetailByItemCus = new UltraCombo();
		this.combitem = new UltraCombo();
		this.txtCommodityDetailIndetail = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label48 = new System.Windows.Forms.Label();
		this.txtQtyMTon = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtCostMTon = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label25 = new System.Windows.Forms.Label();
		this.combpcktype = new UltraCombo();
		this.combrateuom = new UltraCombo();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.combitempck = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
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
		this.txtPaymentRemarksDetail = new System.Windows.Forms.TextBox();
		this.txtPaymentDetailFcyAmount = new System.Windows.Forms.TextBox();
		this.PercntOfTotal = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbPaymentTermDetail = new UltraCombo();
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
		this.cmbBrandForPmEntry = new UltraCombo();
		this.panel30 = new System.Windows.Forms.Panel();
		this.radBrandName = new System.Windows.Forms.RadioButton();
		this.label47 = new System.Windows.Forms.Label();
		this.rabBrandName = new System.Windows.Forms.RadioButton();
		this.rdPackingItemCode = new System.Windows.Forms.RadioButton();
		this.rdPackingItemName = new System.Windows.Forms.RadioButton();
		this.btnPackingCancel = new System.Windows.Forms.Button();
		this.btnPackingUpdate = new System.Windows.Forms.Button();
		this.txtPackingRemarks = new System.Windows.Forms.TextBox();
		this.txtPackingAmount = new System.Windows.Forms.TextBox();
		this.txtPackingItemRate = new System.Windows.Forms.TextBox();
		this.txtBrandInnerQtyPm = new System.Windows.Forms.TextBox();
		this.txtBrandOuterQtyPm = new System.Windows.Forms.TextBox();
		this.txtOuterQtyPm = new System.Windows.Forms.TextBox();
		this.txtInnerQtyPm = new System.Windows.Forms.TextBox();
		this.CmbUomPm = new UltraCombo();
		this.CmbBrandUomPm = new UltraCombo();
		this.label80 = new System.Windows.Forms.Label();
		this.CmbBrandPackingTypePm = new UltraCombo();
		this.label77 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.CmbPMItem = new UltraCombo();
		this.label78 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.btnAutoMap = new System.Windows.Forms.Button();
		this.label56 = new System.Windows.Forms.Label();
		this.tabOtherItems = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdotheritems = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel26 = new System.Windows.Forms.Panel();
		this.label46 = new System.Windows.Forms.Label();
		this.btnoCancel = new System.Windows.Forms.Button();
		this.btnoUpdate = new System.Windows.Forms.Button();
		this.btnoadd = new System.Windows.Forms.Button();
		this.label38 = new System.Windows.Forms.Label();
		this.txtoRemarks = new System.Windows.Forms.TextBox();
		this.txtoQty = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtoRate = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtoAmount = new System.Windows.Forms.TextBox();
		this.CmbOtherItem = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.tabOtherCharges = new System.Windows.Forms.TabPage();
		this.grdOtherChargesDetail = new GridEX();
		this.panel37 = new System.Windows.Forms.Panel();
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
		this.label99 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.label97 = new System.Windows.Forms.Label();
		this.label96 = new System.Windows.Forms.Label();
		this.label94 = new System.Windows.Forms.Label();
		this.label93 = new System.Windows.Forms.Label();
		this.label92 = new System.Windows.Forms.Label();
		this.label88 = new System.Windows.Forms.Label();
		this.label87 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.label85 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.label83 = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.txtExportSoNo = new System.Windows.Forms.TextBox();
		this.txtCommodityDetail = new System.Windows.Forms.RichTextBox();
		this.txtSpecialInstructions = new System.Windows.Forms.RichTextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtDeliveryDays = new System.Windows.Forms.TextBox();
		this.txtFCYAmount = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtMTon = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
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
		this.txtDeliveryRemarks = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtNoOfContainers = new System.Windows.Forms.TextBox();
		this.CmbFCYCode = new UltraCombo();
		this.txtApprovalRemarks = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtProductSpecification = new System.Windows.Forms.TextBox();
		this.txtLotRefNo = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.txtBuyerOrderNo = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.ApprovalDate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.cmbStatus = new UltraCombo();
		this.datOrderDate = new System.Windows.Forms.DateTimePicker();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.CmbNotifyParty = new UltraCombo();
		this.CmbCustomer = new UltraCombo();
		this.CmbDeliveryTerm = new UltraCombo();
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
		this.CmbExporterbank = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.panel28 = new System.Windows.Forms.Panel();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.label66 = new System.Windows.Forms.Label();
		this.CmbExportCompany = new UltraCombo();
		this.labelExportCompany = new System.Windows.Forms.Label();
		this.ChkOnCommission = new System.Windows.Forms.CheckBox();
		this.CmbPaymentTerms = new UltraCombo();
		this.label95 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.txtPaymentRemarks = new System.Windows.Forms.TextBox();
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
		this.panel29 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
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
		this.ctrlgrdOtherChargesDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabOtherChargesHistory = new System.Windows.Forms.TabPage();
		this.panel39 = new System.Windows.Forms.Panel();
		this.label111 = new System.Windows.Forms.Label();
		this.grdOtherChargesHistory = new GridEX();
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
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingNTrade).BeginInit();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcomditiyDetailByItemCus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		this.tabPaymentTerm.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.tabPackingMaterialDetail.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel21.SuspendLayout();
		this.panel25.SuspendLayout();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialDetail).BeginInit();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBrandForPmEntry).BeginInit();
		this.panel30.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUomPm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandPackingTypePm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPMItem).BeginInit();
		this.panel16.SuspendLayout();
		this.tabOtherItems.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel8.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).BeginInit();
		this.tabOtherCharges.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesDetail).BeginInit();
		this.panel37.SuspendLayout();
		this.panel38.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesNameChargeDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFCYCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterbank).BeginInit();
		this.panel28.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).BeginInit();
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
		this.panel29.SuspendLayout();
		this.panel18.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabOtherChargesHistory.SuspendLayout();
		this.panel39.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesHistory).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1234, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1234, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1226, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1220, 676);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 365);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1220, 311);
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
		this.tabControl2.Size = new System.Drawing.Size(1220, 311);
		this.tabControl2.TabIndex = 0;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabContractDetail.Controls.Add(this.panel7);
		this.tabContractDetail.Controls.Add(this.panel6);
		this.tabContractDetail.Location = new System.Drawing.Point(4, 25);
		this.tabContractDetail.Name = "tabContractDetail";
		this.tabContractDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabContractDetail.Size = new System.Drawing.Size(1212, 282);
		this.tabContractDetail.TabIndex = 1;
		this.tabContractDetail.Text = "Contract Detail";
		this.tabContractDetail.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetails);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 136);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1206, 143);
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
		((System.Windows.Forms.Control)(object)this.grdDetails).Size = new System.Drawing.Size(1206, 143);
		((System.Windows.Forms.Control)(object)this.grdDetails).TabIndex = 0;
		this.grdDetails.TotalRow = (InheritableBoolean)1;
		this.grdDetails.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetails).VisualStyle = (VisualStyle)4;
		this.grdDetails.ColumnButtonClick += new ColumnActionEventHandler(grdDetails_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetails).DoubleClick += new System.EventHandler(grdDetails_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetails).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetails_KeyDown);
		this.panel6.Controls.Add(this.label107);
		this.panel6.Controls.Add(this.label106);
		this.panel6.Controls.Add(this.label105);
		this.panel6.Controls.Add(this.label104);
		this.panel6.Controls.Add(this.label103);
		this.panel6.Controls.Add(this.label102);
		this.panel6.Controls.Add(this.label101);
		this.panel6.Controls.Add(this.label100);
		this.panel6.Controls.Add(this.btnplus);
		this.panel6.Controls.Add(this.rdSearchByCode);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade);
		this.panel6.Controls.Add(this.label69);
		this.panel6.Controls.Add(this.rdSearchByName);
		this.panel6.Controls.Add(this.panel23);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcomditiyDetailByItemCus);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.panel6.Controls.Add(this.txtCommodityDetailIndetail);
		this.panel6.Controls.Add(this.txtRemarks);
		this.panel6.Controls.Add(this.btnCancelUpdateDetial);
		this.panel6.Controls.Add(this.label48);
		this.panel6.Controls.Add(this.txtQtyMTon);
		this.panel6.Controls.Add(this.label34);
		this.panel6.Controls.Add(this.label16);
		this.panel6.Controls.Add(this.txtCostMTon);
		this.panel6.Controls.Add(this.label39);
		this.panel6.Controls.Add(this.label10);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Controls.Add(this.btnUpdateDetail);
		this.panel6.Controls.Add(this.label25);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.combpcktype);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.combrateuom);
		this.panel6.Controls.Add(this.txtNoOfBags);
		this.panel6.Controls.Add(this.label24);
		this.panel6.Controls.Add(this.label13);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.combitempck);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel6.Controls.Add(this.label33);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Controls.Add(this.txtAmount);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1206, 133);
		this.panel6.TabIndex = 0;
		this.label107.AutoSize = true;
		this.label107.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label107.ForeColor = System.Drawing.Color.Red;
		this.label107.Location = new System.Drawing.Point(544, 32);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(14, 17);
		this.label107.TabIndex = 583;
		this.label107.Text = "*";
		this.label106.AutoSize = true;
		this.label106.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label106.ForeColor = System.Drawing.Color.Red;
		this.label106.Location = new System.Drawing.Point(1019, 32);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(14, 17);
		this.label106.TabIndex = 582;
		this.label106.Text = "*";
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label105.ForeColor = System.Drawing.Color.Red;
		this.label105.Location = new System.Drawing.Point(925, 33);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(14, 17);
		this.label105.TabIndex = 581;
		this.label105.Text = "*";
		this.label104.AutoSize = true;
		this.label104.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label104.ForeColor = System.Drawing.Color.Red;
		this.label104.Location = new System.Drawing.Point(831, 34);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(14, 17);
		this.label104.TabIndex = 580;
		this.label104.Text = "*";
		this.label103.AutoSize = true;
		this.label103.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label103.ForeColor = System.Drawing.Color.Red;
		this.label103.Location = new System.Drawing.Point(738, 33);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(14, 17);
		this.label103.TabIndex = 579;
		this.label103.Text = "*";
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.ForeColor = System.Drawing.Color.Red;
		this.label102.Location = new System.Drawing.Point(647, 32);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(14, 17);
		this.label102.TabIndex = 578;
		this.label102.Text = "*";
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label101.ForeColor = System.Drawing.Color.Red;
		this.label101.Location = new System.Drawing.Point(432, 32);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(14, 17);
		this.label101.TabIndex = 577;
		this.label101.Text = "*";
		this.label100.AutoSize = true;
		this.label100.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label100.ForeColor = System.Drawing.Color.Red;
		this.label100.Location = new System.Drawing.Point(267, 32);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(14, 17);
		this.label100.TabIndex = 576;
		this.label100.Text = "*";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(1022, 101);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(36, 26);
		this.btnplus.TabIndex = 13;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(113, 31);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 492;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Location = new System.Drawing.Point(562, 101);
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Name = "CmbFarmingNTrade";
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).TextChanged += new System.EventHandler(CmbDeliveryTerm_TextChanged);
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.Black;
		this.label69.Location = new System.Drawing.Point(562, 82);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(95, 15);
		this.label69.TabIndex = 499;
		this.label69.Text = "Farming N Trade";
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(50, 31);
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
		this.cmbcomditiyDetailByItemCus.AllowNull = (DefaultableBoolean)1;
		this.cmbcomditiyDetailByItemCus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcomditiyDetailByItemCus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcomditiyDetailByItemCus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance283).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance283).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance283).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance294).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance305).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance316;
		((AppearanceBase)appearance327).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance327).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance338).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance338).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance338).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance338).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance338).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance338;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcomditiyDetailByItemCus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcomditiyDetailByItemCus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcomditiyDetailByItemCus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcomditiyDetailByItemCus).Location = new System.Drawing.Point(5, 101);
		((System.Windows.Forms.Control)(object)this.cmbcomditiyDetailByItemCus).Name = "cmbcomditiyDetailByItemCus";
		((System.Windows.Forms.Control)(object)this.cmbcomditiyDetailByItemCus).Size = new System.Drawing.Size(279, 26);
		((System.Windows.Forms.Control)(object)this.cmbcomditiyDetailByItemCus).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.cmbcomditiyDetailByItemCus).TextChanged += new System.EventHandler(cmbcomditiyDetailByItemCus_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbcomditiyDetailByItemCus).Leave += new System.EventHandler(cmbcomditiyDetailByItemCus_Leave);
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance298).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance298).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance298).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance299;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance295).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance295).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance295).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance296;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance303;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(5, 52);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(279, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave_1);
		this.txtCommodityDetailIndetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommodityDetailIndetail.Location = new System.Drawing.Point(287, 103);
		this.txtCommodityDetailIndetail.Name = "txtCommodityDetailIndetail";
		this.txtCommodityDetailIndetail.Size = new System.Drawing.Size(272, 23);
		this.txtCommodityDetailIndetail.TabIndex = 10;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(758, 103);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(260, 23);
		this.txtRemarks.TabIndex = 12;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1090, 101);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(62, 26);
		this.btnCancelUpdateDetial.TabIndex = 14;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(287, 82);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(104, 15);
		this.label48.TabIndex = 489;
		this.label48.Text = "Commodity Detail";
		this.txtQtyMTon.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyMTon.Location = new System.Drawing.Point(562, 54);
		this.txtQtyMTon.Name = "txtQtyMTon";
		this.txtQtyMTon.Size = new System.Drawing.Size(101, 23);
		this.txtQtyMTon.TabIndex = 3;
		this.txtQtyMTon.Tag = "MTon";
		this.txtQtyMTon.Text = "0";
		this.txtQtyMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyMTon.TextChanged += new System.EventHandler(txtQtyMTon_TextChanged);
		this.txtQtyMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyMTon_KeyPress);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(758, 82);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(52, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Remarks";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(566, 33);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(41, 15);
		this.label16.TabIndex = 483;
		this.label16.Text = "M.Ton";
		this.txtCostMTon.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCostMTon.Location = new System.Drawing.Point(849, 54);
		this.txtCostMTon.Name = "txtCostMTon";
		this.txtCostMTon.Size = new System.Drawing.Size(92, 23);
		this.txtCostMTon.TabIndex = 6;
		this.txtCostMTon.Text = "0";
		this.txtCostMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCostMTon.TextChanged += new System.EventHandler(txtCostMTon_TextChanged);
		this.txtCostMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCostMTon_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(5, 82);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(144, 15);
		this.label39.TabIndex = 470;
		this.label39.Text = "Comoodity Detail By Item";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(287, 33);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(77, 15);
		this.label10.TabIndex = 476;
		this.label10.Text = "Packing Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 33);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(35, 15);
		this.label1.TabIndex = 470;
		this.label1.Text = "Item ";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1022, 101);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(66, 26);
		this.btnUpdateDetail.TabIndex = 13;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(851, 33);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(30, 15);
		this.label25.TabIndex = 487;
		this.label25.Text = "Rate";
		this.combpcktype.AllowNull = (DefaultableBoolean)1;
		this.combpcktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpcktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpcktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpcktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.combpcktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance267;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance273).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpcktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpcktype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpcktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpcktype).Location = new System.Drawing.Point(287, 52);
		((System.Windows.Forms.Control)(object)this.combpcktype).Name = "combpcktype";
		((System.Windows.Forms.Control)(object)this.combpcktype).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.combpcktype).TabIndex = 1;
		this.combrateuom.AllowNull = (DefaultableBoolean)1;
		this.combrateuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrateuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrateuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrateuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.combrateuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance268).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrateuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrateuom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrateuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrateuom).Location = new System.Drawing.Point(944, 52);
		((System.Windows.Forms.Control)(object)this.combrateuom).Name = "combrateuom";
		((System.Windows.Forms.Control)(object)this.combrateuom).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.combrateuom).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.combrateuom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(758, 54);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(88, 23);
		this.txtNoOfBags.TabIndex = 5;
		this.txtNoOfBags.Tag = "NoofBags";
		this.txtNoOfBags.Text = "0";
		this.txtNoOfBags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfBags.TextChanged += new System.EventHandler(txtNoOfBags_TextChanged);
		this.txtNoOfBags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfBags_KeyPress);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(452, 33);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(54, 15);
		this.label24.TabIndex = 476;
		this.label24.Text = "CropYear";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(945, 33);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(61, 15);
		this.label13.TabIndex = 479;
		this.label13.Text = "Rate UOM";
		this.combitempck.AllowNull = (DefaultableBoolean)1;
		this.combitempck.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitempck.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitempck.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitempck).DisplayLayout.Appearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.combitempck).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitempck).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance281).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance281).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance282;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance284).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance284).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance285).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance288).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance289).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance289).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance289).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance290).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitempck).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitempck).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitempck.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitempck).Location = new System.Drawing.Point(665, 52);
		((System.Windows.Forms.Control)(object)this.combitempck).Name = "combitempck";
		((System.Windows.Forms.Control)(object)this.combitempck).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.combitempck).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.combitempck).TextChanged += new System.EventHandler(combitempck_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(758, 33);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(67, 15);
		this.label23.TabIndex = 485;
		this.label23.Text = "No Of Bags";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance308).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance308).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance308;
		((AppearanceBase)appearance309).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance309;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance196).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance317).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(452, 52);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(107, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 2;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(1036, 33);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(51, 15);
		this.label33.TabIndex = 485;
		this.label33.Text = "Amount";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(667, 33);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 15);
		this.label2.TabIndex = 474;
		this.label2.Text = "Pack UOM";
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(1038, 54);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(114, 23);
		this.txtAmount.TabIndex = 8;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.tabPaymentTerm.Controls.Add(this.panel10);
		this.tabPaymentTerm.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentTerm.Margin = new System.Windows.Forms.Padding(2);
		this.tabPaymentTerm.Name = "tabPaymentTerm";
		this.tabPaymentTerm.Size = new System.Drawing.Size(1212, 282);
		this.tabPaymentTerm.TabIndex = 2;
		this.tabPaymentTerm.Text = "PaymentDetail";
		this.tabPaymentTerm.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1212, 282);
		this.panel10.TabIndex = 0;
		this.panel12.BackColor = System.Drawing.Color.White;
		this.panel12.Controls.Add(this.panel22);
		this.panel12.Controls.Add(this.panel14);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 26);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1212, 256);
		this.panel12.TabIndex = 1;
		this.panel22.Controls.Add(this.panel24);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(0, 63);
		this.panel22.Margin = new System.Windows.Forms.Padding(2);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1212, 193);
		this.panel22.TabIndex = 504;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerm);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Margin = new System.Windows.Forms.Padding(2);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1212, 193);
		this.panel24.TabIndex = 1;
		this.grdPaymentTerm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentTerm).BackColor = System.Drawing.Color.White;
		this.grdPaymentTerm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentTerm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerm.DesignTimeLayout = grdPaymentTerm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentTerm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Name = "grdPaymentTerm";
		((ScrollableBase)this.grdPaymentTerm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Size = new System.Drawing.Size(1212, 193);
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
		this.panel14.Controls.Add(this.txtPaymentRemarksDetail);
		this.panel14.Controls.Add(this.txtPaymentDetailFcyAmount);
		this.panel14.Controls.Add(this.PercntOfTotal);
		this.panel14.Controls.Add(this.label53);
		this.panel14.Controls.Add(this.label54);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail);
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
		this.txtPaymentRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPaymentRemarksDetail.Location = new System.Drawing.Point(416, 28);
		this.txtPaymentRemarksDetail.Name = "txtPaymentRemarksDetail";
		this.txtPaymentRemarksDetail.Size = new System.Drawing.Size(570, 23);
		this.txtPaymentRemarksDetail.TabIndex = 3;
		this.txtPaymentDetailFcyAmount.Enabled = false;
		this.txtPaymentDetailFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPaymentDetailFcyAmount.Location = new System.Drawing.Point(283, 28);
		this.txtPaymentDetailFcyAmount.Name = "txtPaymentDetailFcyAmount";
		this.txtPaymentDetailFcyAmount.Size = new System.Drawing.Size(131, 23);
		this.txtPaymentDetailFcyAmount.TabIndex = 2;
		this.txtPaymentDetailFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.PercntOfTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.PercntOfTotal.Location = new System.Drawing.Point(181, 28);
		this.PercntOfTotal.Name = "PercntOfTotal";
		this.PercntOfTotal.Size = new System.Drawing.Size(100, 23);
		this.PercntOfTotal.TabIndex = 1;
		this.PercntOfTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.PercntOfTotal.TextChanged += new System.EventHandler(PercntOfTotal_TextChanged);
		this.PercntOfTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PercntOfTotal_KeyPress);
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
		this.CmbPaymentTermDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance333).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance333;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance334).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance334).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance334).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance334;
		((AppearanceBase)appearance335).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance335;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance336).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance336).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance336).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance337).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance339).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance340;
		((AppearanceBase)appearance341).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance341).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance342).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance342).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance342).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance342).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance342;
		((AppearanceBase)appearance343).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance344).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance344;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance345).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).Location = new System.Drawing.Point(7, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).Name = "CmbPaymentTermDetail";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).TextChanged += new System.EventHandler(CmbPaymentTermDetail_TextChanged);
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
		this.tabPackingMaterialDetail.Size = new System.Drawing.Size(1212, 282);
		this.tabPackingMaterialDetail.TabIndex = 3;
		this.tabPackingMaterialDetail.Text = "Packing Material (PMRP)";
		this.tabPackingMaterialDetail.UseVisualStyleBackColor = true;
		this.panel15.Controls.Add(this.panel19);
		this.panel15.Controls.Add(this.panel16);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Margin = new System.Windows.Forms.Padding(2);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1212, 282);
		this.panel15.TabIndex = 0;
		this.panel19.Controls.Add(this.panel21);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 26);
		this.panel19.Margin = new System.Windows.Forms.Padding(2);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1212, 256);
		this.panel19.TabIndex = 1;
		this.panel21.Controls.Add(this.panel25);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 102);
		this.panel21.Margin = new System.Windows.Forms.Padding(2);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1212, 154);
		this.panel21.TabIndex = 1;
		this.panel25.Controls.Add(this.panel27);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Margin = new System.Windows.Forms.Padding(2);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1212, 154);
		this.panel25.TabIndex = 0;
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel27.Location = new System.Drawing.Point(0, 0);
		this.panel27.Margin = new System.Windows.Forms.Padding(2);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1212, 154);
		this.panel27.TabIndex = 1;
		this.grdPackingMaterialDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPackingMaterialDetail).BackColor = System.Drawing.Color.White;
		this.grdPackingMaterialDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPackingMaterialDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterialDetail.DesignTimeLayout = grdPackingMaterialDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingMaterialDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterialDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Name = "grdPackingMaterialDetail";
		((ScrollableBase)this.grdPackingMaterialDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).Size = new System.Drawing.Size(1212, 154);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).TabIndex = 0;
		this.grdPackingMaterialDetail.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterialDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterialDetail).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterialDetail.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterialDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).DoubleClick += new System.EventHandler(grdPackingMaterialDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPackingMaterialDetail_KeyDown);
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add(this.btnPackingAdd);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry);
		this.panel20.Controls.Add(this.panel30);
		this.panel20.Controls.Add(this.rdPackingItemCode);
		this.panel20.Controls.Add(this.rdPackingItemName);
		this.panel20.Controls.Add(this.btnPackingCancel);
		this.panel20.Controls.Add(this.btnPackingUpdate);
		this.panel20.Controls.Add(this.txtPackingRemarks);
		this.panel20.Controls.Add(this.txtPackingAmount);
		this.panel20.Controls.Add(this.txtPackingItemRate);
		this.panel20.Controls.Add(this.txtBrandInnerQtyPm);
		this.panel20.Controls.Add(this.txtBrandOuterQtyPm);
		this.panel20.Controls.Add(this.txtOuterQtyPm);
		this.panel20.Controls.Add(this.txtInnerQtyPm);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUomPm);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandUomPm);
		this.panel20.Controls.Add(this.label80);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm);
		this.panel20.Controls.Add(this.label77);
		this.panel20.Controls.Add(this.label79);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPMItem);
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
		this.btnPackingAdd.TabIndex = 12;
		this.btnPackingAdd.Text = "+";
		this.btnPackingAdd.UseVisualStyleBackColor = false;
		this.btnPackingAdd.Click += new System.EventHandler(btnPackingAdd_Click);
		this.btnPackingAdd.Leave += new System.EventHandler(btnPackingAdd_Leave);
		this.cmbBrandForPmEntry.AllowNull = (DefaultableBoolean)1;
		this.cmbBrandForPmEntry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBrandForPmEntry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBrandForPmEntry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBrandForPmEntry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBrandForPmEntry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Name = "cmbBrandForPmEntry";
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBrandForPmEntry).Leave += new System.EventHandler(cmbBrandForPmEntry_Leave);
		this.panel30.Controls.Add(this.radBrandName);
		this.panel30.Controls.Add(this.label47);
		this.panel30.Controls.Add(this.rabBrandName);
		this.panel30.Location = new System.Drawing.Point(4, 3);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(183, 24);
		this.panel30.TabIndex = 576;
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
		this.btnPackingCancel.TabIndex = 14;
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
		this.txtPackingRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingRemarks.Location = new System.Drawing.Point(220, 76);
		this.txtPackingRemarks.Name = "txtPackingRemarks";
		this.txtPackingRemarks.Size = new System.Drawing.Size(773, 23);
		this.txtPackingRemarks.TabIndex = 11;
		this.txtPackingAmount.Enabled = false;
		this.txtPackingAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingAmount.Location = new System.Drawing.Point(109, 76);
		this.txtPackingAmount.Name = "txtPackingAmount";
		this.txtPackingAmount.Size = new System.Drawing.Size(109, 23);
		this.txtPackingAmount.TabIndex = 10;
		this.txtPackingAmount.Text = "0";
		this.txtPackingAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingItemRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingItemRate.Location = new System.Drawing.Point(3, 76);
		this.txtPackingItemRate.Name = "txtPackingItemRate";
		this.txtPackingItemRate.Size = new System.Drawing.Size(104, 23);
		this.txtPackingItemRate.TabIndex = 9;
		this.txtPackingItemRate.Text = "0";
		this.txtPackingItemRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingItemRate.TextChanged += new System.EventHandler(txtPackingItemRate_TextChanged);
		this.txtPackingItemRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingItemRate_KeyPress);
		this.txtBrandInnerQtyPm.Enabled = false;
		this.txtBrandInnerQtyPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrandInnerQtyPm.Location = new System.Drawing.Point(578, 31);
		this.txtBrandInnerQtyPm.Name = "txtBrandInnerQtyPm";
		this.txtBrandInnerQtyPm.Size = new System.Drawing.Size(111, 23);
		this.txtBrandInnerQtyPm.TabIndex = 4;
		this.txtBrandInnerQtyPm.Text = "0";
		this.txtBrandInnerQtyPm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrandInnerQtyPm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrandInnerQtyPm_KeyPress);
		this.txtBrandOuterQtyPm.Enabled = false;
		this.txtBrandOuterQtyPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrandOuterQtyPm.Location = new System.Drawing.Point(465, 31);
		this.txtBrandOuterQtyPm.Name = "txtBrandOuterQtyPm";
		this.txtBrandOuterQtyPm.Size = new System.Drawing.Size(111, 23);
		this.txtBrandOuterQtyPm.TabIndex = 3;
		this.txtBrandOuterQtyPm.Text = "0";
		this.txtBrandOuterQtyPm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrandOuterQtyPm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrandOuterQtyPm_KeyPress);
		this.txtOuterQtyPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOuterQtyPm.Location = new System.Drawing.Point(995, 31);
		this.txtOuterQtyPm.Name = "txtOuterQtyPm";
		this.txtOuterQtyPm.Size = new System.Drawing.Size(104, 23);
		this.txtOuterQtyPm.TabIndex = 7;
		this.txtOuterQtyPm.Text = "0";
		this.txtOuterQtyPm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOuterQtyPm.TextChanged += new System.EventHandler(txtOuterQtyPm_TextChanged);
		this.txtOuterQtyPm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOuterQtyPm_KeyPress);
		this.txtInnerQtyPm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInnerQtyPm.Location = new System.Drawing.Point(1100, 31);
		this.txtInnerQtyPm.Name = "txtInnerQtyPm";
		this.txtInnerQtyPm.Size = new System.Drawing.Size(104, 23);
		this.txtInnerQtyPm.TabIndex = 8;
		this.txtInnerQtyPm.Text = "0";
		this.txtInnerQtyPm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInnerQtyPm.TextChanged += new System.EventHandler(txtIPackingtemQty_TextChanged);
		this.txtInnerQtyPm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInnerQtyPm_KeyPress);
		this.CmbUomPm.AllowNull = (DefaultableBoolean)1;
		this.CmbUomPm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUomPm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUomPm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance346).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance346;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance347).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance347).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance347).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomPm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance347;
		((AppearanceBase)appearance348).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance348;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomPm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUomPm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUomPm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUomPm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUomPm).Location = new System.Drawing.Point(896, 29);
		((System.Windows.Forms.Control)(object)this.CmbUomPm).Name = "CmbUomPm";
		((System.Windows.Forms.Control)(object)this.CmbUomPm).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.CmbUomPm).TabIndex = 6;
		this.CmbBrandUomPm.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandUomPm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandUomPm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandUomPm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance241;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance247).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandUomPm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandUomPm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Location = new System.Drawing.Point(220, 29);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Name = "CmbBrandUomPm";
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPm).TabIndex = 1;
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.Location = new System.Drawing.Point(898, 8);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(82, 15);
		this.label80.TabIndex = 566;
		this.label80.Text = "PM Item Uom";
		this.CmbBrandPackingTypePm.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandPackingTypePm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandPackingTypePm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandPackingTypePm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandPackingTypePm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandPackingTypePm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).Location = new System.Drawing.Point(321, 29);
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).Name = "CmbBrandPackingTypePm";
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandPackingTypePm).TabIndex = 2;
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
		this.CmbPMItem.AllowNull = (DefaultableBoolean)1;
		this.CmbPMItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPMItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPMItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPMItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbPMItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPMItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPMItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPMItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPMItem).Location = new System.Drawing.Point(692, 29);
		((System.Windows.Forms.Control)(object)this.CmbPMItem).Name = "CmbPMItem";
		((System.Windows.Forms.Control)(object)this.CmbPMItem).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbPMItem).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPMItem).Leave += new System.EventHandler(CmbPMItem_Leave);
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
		this.panel16.Controls.Add(this.btnAutoMap);
		this.panel16.Controls.Add(this.label56);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Margin = new System.Windows.Forms.Padding(2);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1212, 26);
		this.panel16.TabIndex = 0;
		this.btnAutoMap.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnAutoMap.BackColor = System.Drawing.Color.White;
		this.btnAutoMap.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnAutoMap.ForeColor = System.Drawing.Color.Teal;
		this.btnAutoMap.Location = new System.Drawing.Point(1124, 2);
		this.btnAutoMap.Name = "btnAutoMap";
		this.btnAutoMap.Size = new System.Drawing.Size(86, 23);
		this.btnAutoMap.TabIndex = 562;
		this.btnAutoMap.Text = "Auto Map";
		this.btnAutoMap.UseVisualStyleBackColor = false;
		this.btnAutoMap.Click += new System.EventHandler(btnAutoMap_Click);
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
		this.tabOtherItems.Size = new System.Drawing.Size(1212, 282);
		this.tabOtherItems.TabIndex = 0;
		this.tabOtherItems.Text = "Other Items";
		this.tabOtherItems.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 84);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1206, 195);
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
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1206, 195);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 0;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		this.grdotheritems.ColumnButtonClick += new ColumnActionEventHandler(grdotheritems_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdotheritems).KeyDown += new System.Windows.Forms.KeyEventHandler(grdotheritems_KeyDown);
		this.panel8.Controls.Add(this.panel26);
		this.panel8.Controls.Add(this.btnoCancel);
		this.panel8.Controls.Add(this.btnoUpdate);
		this.panel8.Controls.Add(this.btnoadd);
		this.panel8.Controls.Add(this.label38);
		this.panel8.Controls.Add(this.txtoRemarks);
		this.panel8.Controls.Add(this.txtoQty);
		this.panel8.Controls.Add(this.label35);
		this.panel8.Controls.Add(this.txtoRate);
		this.panel8.Controls.Add(this.label36);
		this.panel8.Controls.Add(this.label37);
		this.panel8.Controls.Add(this.txtoAmount);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherItem);
		this.panel8.Controls.Add(this.label31);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1206, 81);
		this.panel8.TabIndex = 1;
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
		this.btnoCancel.BackColor = System.Drawing.Color.Teal;
		this.btnoCancel.FlatAppearance.BorderSize = 0;
		this.btnoCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoCancel.ForeColor = System.Drawing.Color.White;
		this.btnoCancel.Location = new System.Drawing.Point(1084, 49);
		this.btnoCancel.Name = "btnoCancel";
		this.btnoCancel.Size = new System.Drawing.Size(62, 26);
		this.btnoCancel.TabIndex = 7;
		this.btnoCancel.Text = "Cancel";
		this.btnoCancel.UseVisualStyleBackColor = false;
		this.btnoCancel.Visible = false;
		this.btnoCancel.Click += new System.EventHandler(btnoCancel_Click);
		this.btnoUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnoUpdate.FlatAppearance.BorderSize = 0;
		this.btnoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoUpdate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoUpdate.ForeColor = System.Drawing.Color.White;
		this.btnoUpdate.Location = new System.Drawing.Point(1018, 49);
		this.btnoUpdate.Name = "btnoUpdate";
		this.btnoUpdate.Size = new System.Drawing.Size(65, 26);
		this.btnoUpdate.TabIndex = 6;
		this.btnoUpdate.Text = "Update";
		this.btnoUpdate.UseVisualStyleBackColor = false;
		this.btnoUpdate.Visible = false;
		this.btnoUpdate.Click += new System.EventHandler(btnoUpdate_Click);
		this.btnoadd.BackColor = System.Drawing.Color.Teal;
		this.btnoadd.FlatAppearance.BorderSize = 0;
		this.btnoadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoadd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoadd.ForeColor = System.Drawing.Color.White;
		this.btnoadd.Location = new System.Drawing.Point(992, 49);
		this.btnoadd.Name = "btnoadd";
		this.btnoadd.Size = new System.Drawing.Size(25, 26);
		this.btnoadd.TabIndex = 5;
		this.btnoadd.Text = "+";
		this.btnoadd.UseVisualStyleBackColor = false;
		this.btnoadd.Click += new System.EventHandler(btnoadd_Click);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(546, 31);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(52, 15);
		this.label38.TabIndex = 495;
		this.label38.Text = "Remarks";
		this.txtoRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRemarks.Location = new System.Drawing.Point(545, 51);
		this.txtoRemarks.Name = "txtoRemarks";
		this.txtoRemarks.Size = new System.Drawing.Size(443, 23);
		this.txtoRemarks.TabIndex = 4;
		this.txtoQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoQty.Location = new System.Drawing.Point(257, 51);
		this.txtoQty.Name = "txtoQty";
		this.txtoQty.Size = new System.Drawing.Size(101, 23);
		this.txtoQty.TabIndex = 1;
		this.txtoQty.Text = "0";
		this.txtoQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtoQty.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(264, 30);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(26, 15);
		this.label35.TabIndex = 491;
		this.label35.Text = "Qty";
		this.txtoRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRate.Location = new System.Drawing.Point(360, 51);
		this.txtoRate.Name = "txtoRate";
		this.txtoRate.Size = new System.Drawing.Size(80, 23);
		this.txtoRate.TabIndex = 2;
		this.txtoRate.Text = "0";
		this.txtoRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtoRate.TextChanged += new System.EventHandler(txtoRate_TextChanged);
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
		this.txtoAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoAmount.Location = new System.Drawing.Point(442, 51);
		this.txtoAmount.Name = "txtoAmount";
		this.txtoAmount.ReadOnly = true;
		this.txtoAmount.Size = new System.Drawing.Size(101, 23);
		this.txtoAmount.TabIndex = 3;
		this.txtoAmount.Text = "0";
		this.txtoAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbOtherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance320).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance321).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance321).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance321).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance321;
		((AppearanceBase)appearance322).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance322;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance323).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance323).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance323).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance323;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance324;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance325).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance325;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance326;
		((AppearanceBase)appearance328).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance328).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance329).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance329).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance329).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance329;
		((AppearanceBase)appearance330).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance330;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance331).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance331).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOtherItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Location = new System.Drawing.Point(9, 49);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Name = "CmbOtherItem";
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Size = new System.Drawing.Size(245, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TabIndex = 0;
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
		this.tabOtherCharges.Size = new System.Drawing.Size(1212, 282);
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
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).Size = new System.Drawing.Size(1212, 205);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).TabIndex = 7;
		this.grdOtherChargesDetail.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesDetail).VisualStyle = (VisualStyle)4;
		this.grdOtherChargesDetail.ColumnButtonClick += new ColumnActionEventHandler(grdOtherChargesDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOtherChargesDetail).DoubleClick += new System.EventHandler(grdOtherChargesDetail_DoubleClick);
		this.panel37.BackColor = System.Drawing.Color.Teal;
		this.panel37.Controls.Add(this.ctrlgrdOtherChargesDetail);
		this.panel37.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel37.Location = new System.Drawing.Point(0, 54);
		this.panel37.Name = "panel37";
		this.panel37.Size = new System.Drawing.Size(1212, 23);
		this.panel37.TabIndex = 6;
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
		this.panel38.TabIndex = 4;
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
		this.panel4.Controls.Add(this.ChkOnCommission);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerms);
		this.panel4.Controls.Add(this.label95);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label49);
		this.panel4.Controls.Add(this.txtPaymentRemarks);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1220, 335);
		this.panel4.TabIndex = 1;
		this.groupBox1.Controls.Add(this.label99);
		this.groupBox1.Controls.Add(this.label98);
		this.groupBox1.Controls.Add(this.label97);
		this.groupBox1.Controls.Add(this.label96);
		this.groupBox1.Controls.Add(this.label94);
		this.groupBox1.Controls.Add(this.label93);
		this.groupBox1.Controls.Add(this.label92);
		this.groupBox1.Controls.Add(this.label88);
		this.groupBox1.Controls.Add(this.label87);
		this.groupBox1.Controls.Add(this.label86);
		this.groupBox1.Controls.Add(this.label85);
		this.groupBox1.Controls.Add(this.label84);
		this.groupBox1.Controls.Add(this.label83);
		this.groupBox1.Controls.Add(this.label82);
		this.groupBox1.Controls.Add(this.txtExportSoNo);
		this.groupBox1.Controls.Add(this.txtCommodityDetail);
		this.groupBox1.Controls.Add(this.txtSpecialInstructions);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtDeliveryDays);
		this.groupBox1.Controls.Add(this.txtFCYAmount);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtMTon);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label11);
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
		this.groupBox1.Controls.Add(this.txtDeliveryRemarks);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtNoOfContainers);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFCYCode);
		this.groupBox1.Controls.Add(this.txtApprovalRemarks);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtProductSpecification);
		this.groupBox1.Controls.Add(this.txtLotRefNo);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.txtBuyerOrderNo);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.ApprovalDate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.groupBox1.Controls.Add(this.datOrderDate);
		this.groupBox1.Controls.Add(this.txtDocDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbNotifyParty);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
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
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExporterbank);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 28);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1208, 302);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label99.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label99.ForeColor = System.Drawing.Color.Red;
		this.label99.Location = new System.Drawing.Point(300, 271);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(10, 10);
		this.label99.TabIndex = 585;
		this.label99.Text = "*";
		this.label98.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.ForeColor = System.Drawing.Color.Red;
		this.label98.Location = new System.Drawing.Point(300, 243);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(10, 10);
		this.label98.TabIndex = 584;
		this.label98.Text = "*";
		this.label97.AutoSize = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label97.ForeColor = System.Drawing.Color.Red;
		this.label97.Location = new System.Drawing.Point(79, 244);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(14, 17);
		this.label97.TabIndex = 583;
		this.label97.Text = "*";
		this.label96.AutoSize = true;
		this.label96.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label96.ForeColor = System.Drawing.Color.Red;
		this.label96.Location = new System.Drawing.Point(559, 43);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(14, 17);
		this.label96.TabIndex = 582;
		this.label96.Text = "*";
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.ForeColor = System.Drawing.Color.Red;
		this.label94.Location = new System.Drawing.Point(559, 187);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(14, 17);
		this.label94.TabIndex = 580;
		this.label94.Text = "*";
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label93.ForeColor = System.Drawing.Color.Red;
		this.label93.Location = new System.Drawing.Point(559, 160);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(14, 17);
		this.label93.TabIndex = 579;
		this.label93.Text = "*";
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.Red;
		this.label92.Location = new System.Drawing.Point(559, 133);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(14, 17);
		this.label92.TabIndex = 578;
		this.label92.Text = "*";
		this.label88.AutoSize = true;
		this.label88.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label88.ForeColor = System.Drawing.Color.Red;
		this.label88.Location = new System.Drawing.Point(559, 73);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(14, 17);
		this.label88.TabIndex = 577;
		this.label88.Text = "*";
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.ForeColor = System.Drawing.Color.Red;
		this.label87.Location = new System.Drawing.Point(559, 102);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(14, 17);
		this.label87.TabIndex = 576;
		this.label87.Text = "*";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.ForeColor = System.Drawing.Color.Red;
		this.label86.Location = new System.Drawing.Point(295, 103);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(14, 17);
		this.label86.TabIndex = 575;
		this.label86.Text = "*";
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.ForeColor = System.Drawing.Color.Red;
		this.label85.Location = new System.Drawing.Point(79, 102);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(14, 17);
		this.label85.TabIndex = 574;
		this.label85.Text = "*";
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.ForeColor = System.Drawing.Color.Red;
		this.label84.Location = new System.Drawing.Point(79, 76);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(14, 17);
		this.label84.TabIndex = 573;
		this.label84.Text = "*";
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.ForeColor = System.Drawing.Color.Red;
		this.label83.Location = new System.Drawing.Point(79, 47);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(14, 17);
		this.label83.TabIndex = 572;
		this.label83.Text = "*";
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.ForeColor = System.Drawing.Color.Red;
		this.label82.Location = new System.Drawing.Point(79, 18);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(14, 17);
		this.label82.TabIndex = 571;
		this.label82.Text = "*";
		this.txtExportSoNo.Enabled = false;
		this.txtExportSoNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExportSoNo.Location = new System.Drawing.Point(93, 17);
		this.txtExportSoNo.Name = "txtExportSoNo";
		this.txtExportSoNo.Size = new System.Drawing.Size(137, 23);
		this.txtExportSoNo.TabIndex = 0;
		this.txtCommodityDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommodityDetail.Location = new System.Drawing.Point(716, 100);
		this.txtCommodityDetail.Name = "txtCommodityDetail";
		this.txtCommodityDetail.Size = new System.Drawing.Size(328, 53);
		this.txtCommodityDetail.TabIndex = 28;
		this.txtCommodityDetail.Text = "";
		this.txtSpecialInstructions.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSpecialInstructions.Location = new System.Drawing.Point(716, 185);
		this.txtSpecialInstructions.Name = "txtSpecialInstructions";
		this.txtSpecialInstructions.Size = new System.Drawing.Size(328, 52);
		this.txtSpecialInstructions.TabIndex = 29;
		this.txtSpecialInstructions.Text = "";
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(573, 158);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(137, 23);
		this.txtExchangeRate.TabIndex = 24;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(479, 162);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(84, 15);
		this.label40.TabIndex = 562;
		this.label40.Text = "Exchange Rate";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(479, 246);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(79, 15);
		this.label12.TabIndex = 497;
		this.label12.Text = "Specification ";
		this.txtDeliveryDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryDays.Location = new System.Drawing.Point(93, 242);
		this.txtDeliveryDays.Name = "txtDeliveryDays";
		this.txtDeliveryDays.Size = new System.Drawing.Size(137, 23);
		this.txtDeliveryDays.TabIndex = 10;
		this.txtDeliveryDays.TextChanged += new System.EventHandler(txtDeliveryDays_TextChanged);
		this.txtDeliveryDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeliveryDays_KeyPress);
		this.txtFCYAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFCYAmount.Location = new System.Drawing.Point(573, 185);
		this.txtFCYAmount.Name = "txtFCYAmount";
		this.txtFCYAmount.ReadOnly = true;
		this.txtFCYAmount.Size = new System.Drawing.Size(137, 23);
		this.txtFCYAmount.TabIndex = 25;
		this.txtFCYAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(2, 76);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 15);
		this.label6.TabIndex = 459;
		this.label6.Text = "Customer";
		this.txtMTon.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMTon.Location = new System.Drawing.Point(573, 72);
		this.txtMTon.Name = "txtMTon";
		this.txtMTon.ReadOnly = true;
		this.txtMTon.Size = new System.Drawing.Size(137, 23);
		this.txtMTon.TabIndex = 21;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(2, 246);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(79, 15);
		this.label42.TabIndex = 566;
		this.label42.Text = "Delivery Days";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(232, 105);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(65, 15);
		this.label11.TabIndex = 462;
		this.label11.Text = "Order Date";
		this.txtPMScheduleDate.CustomFormat = "dd-MMM-yyyy";
		this.txtPMScheduleDate.Enabled = false;
		this.txtPMScheduleDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPMScheduleDate.Location = new System.Drawing.Point(1050, 214);
		this.txtPMScheduleDate.Name = "txtPMScheduleDate";
		this.txtPMScheduleDate.Size = new System.Drawing.Size(151, 23);
		this.txtPMScheduleDate.TabIndex = 33;
		this.labelDCG.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.labelDCG.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.labelDCG.Location = new System.Drawing.Point(1050, 242);
		this.labelDCG.Name = "labelDCG";
		this.labelDCG.Size = new System.Drawing.Size(151, 23);
		this.labelDCG.TabIndex = 564;
		this.labelDCG.Text = "Document Custom Group";
		this.labelDCG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label72.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label72.Location = new System.Drawing.Point(1050, 187);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(151, 23);
		this.label72.TabIndex = 564;
		this.label72.Text = "PM Schedule Date";
		this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtInspectionScheduleDate.CustomFormat = "dd-MMM-yyyy";
		this.txtInspectionScheduleDate.Enabled = false;
		this.txtInspectionScheduleDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectionScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtInspectionScheduleDate.Location = new System.Drawing.Point(1050, 158);
		this.txtInspectionScheduleDate.Name = "txtInspectionScheduleDate";
		this.txtInspectionScheduleDate.Size = new System.Drawing.Size(151, 23);
		this.txtInspectionScheduleDate.TabIndex = 32;
		this.label71.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label71.Location = new System.Drawing.Point(1050, 129);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(151, 23);
		this.label71.TabIndex = 564;
		this.label71.Text = "Inspection Schedule Date";
		this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtProductionScheduleDate.CustomFormat = "dd-MMM-yyyy";
		this.txtProductionScheduleDate.Enabled = false;
		this.txtProductionScheduleDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtProductionScheduleDate.Location = new System.Drawing.Point(1050, 101);
		this.txtProductionScheduleDate.Name = "txtProductionScheduleDate";
		this.txtProductionScheduleDate.Size = new System.Drawing.Size(151, 23);
		this.txtProductionScheduleDate.TabIndex = 31;
		this.label70.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label70.Location = new System.Drawing.Point(1050, 74);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(151, 23);
		this.label70.TabIndex = 564;
		this.label70.Text = "Production Schedule Date";
		this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtFactoryLoadingDate.CustomFormat = "dd-MMM-yyyy";
		this.txtFactoryLoadingDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryLoadingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFactoryLoadingDate.Location = new System.Drawing.Point(1050, 45);
		this.txtFactoryLoadingDate.Name = "txtFactoryLoadingDate";
		this.txtFactoryLoadingDate.Size = new System.Drawing.Size(151, 23);
		this.txtFactoryLoadingDate.TabIndex = 30;
		this.txtFactoryLoadingDate.ValueChanged += new System.EventHandler(txtFactoryLoadingDate_ValueChanged);
		this.label68.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label68.Location = new System.Drawing.Point(1050, 17);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(151, 23);
		this.label68.TabIndex = 564;
		this.label68.Text = "Factory Loading Date";
		this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.datShipmentStartDate.CustomFormat = "dd-MMM-yyyy";
		this.datShipmentStartDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datShipmentStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datShipmentStartDate.Location = new System.Drawing.Point(93, 214);
		this.datShipmentStartDate.Name = "datShipmentStartDate";
		this.datShipmentStartDate.Size = new System.Drawing.Size(137, 23);
		this.datShipmentStartDate.TabIndex = 9;
		this.datShipmentStartDate.ValueChanged += new System.EventHandler(datShipmentStartDate_ValueChanged);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(2, 218);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(90, 15);
		this.label41.TabIndex = 564;
		this.label41.Text = "Ship. Start Date";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(230, 218);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(66, 15);
		this.label26.TabIndex = 489;
		this.label26.Text = "Lot Ref No.";
		this.txtDeliveryRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryRemarks.Location = new System.Drawing.Point(782, 45);
		this.txtDeliveryRemarks.Name = "txtDeliveryRemarks";
		this.txtDeliveryRemarks.Size = new System.Drawing.Size(262, 23);
		this.txtDeliveryRemarks.TabIndex = 20;
		this.label51.BackColor = System.Drawing.Color.LightGray;
		this.label51.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(716, 158);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(328, 23);
		this.label51.TabIndex = 460;
		this.label51.Text = "Special Instructions";
		this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.BackColor = System.Drawing.Color.LightGray;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(716, 72);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(328, 23);
		this.label8.TabIndex = 460;
		this.label8.Text = "Commodity Detail";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(479, 189);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(74, 15);
		this.label21.TabIndex = 461;
		this.label21.Text = "FCY Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(232, 21);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(80, 15);
		this.label3.TabIndex = 463;
		this.label3.Text = "Contract Date";
		this.txtNoOfContainers.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainers.Location = new System.Drawing.Point(573, 100);
		this.txtNoOfContainers.Name = "txtNoOfContainers";
		this.txtNoOfContainers.Size = new System.Drawing.Size(137, 23);
		this.txtNoOfContainers.TabIndex = 22;
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
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Location = new System.Drawing.Point(573, 127);
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Name = "CmbFCYCode";
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Size = new System.Drawing.Size(137, 26);
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).TabIndex = 23;
		this.txtApprovalRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovalRemarks.Location = new System.Drawing.Point(782, 17);
		this.txtApprovalRemarks.Name = "txtApprovalRemarks";
		this.txtApprovalRemarks.Size = new System.Drawing.Size(262, 23);
		this.txtApprovalRemarks.TabIndex = 16;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(479, 104);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(63, 15);
		this.label32.TabIndex = 495;
		this.label32.Text = "Containers";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(483, 76);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(41, 15);
		this.label22.TabIndex = 457;
		this.label22.Text = "M.Ton";
		this.txtProductSpecification.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductSpecification.Location = new System.Drawing.Point(573, 242);
		this.txtProductSpecification.Name = "txtProductSpecification";
		this.txtProductSpecification.Size = new System.Drawing.Size(471, 23);
		this.txtProductSpecification.TabIndex = 27;
		this.txtLotRefNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLotRefNo.Location = new System.Drawing.Point(312, 214);
		this.txtLotRefNo.Name = "txtLotRefNo";
		this.txtLotRefNo.Size = new System.Drawing.Size(165, 23);
		this.txtLotRefNo.TabIndex = 12;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(479, 133);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(55, 15);
		this.label20.TabIndex = 458;
		this.label20.Text = "Fcy Code";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(716, 49);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(52, 15);
		this.label50.TabIndex = 570;
		this.label50.Text = "Remarks";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(716, 21);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(52, 15);
		this.label44.TabIndex = 570;
		this.label44.Text = "Remarks";
		this.txtBuyerOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBuyerOrderNo.Location = new System.Drawing.Point(93, 101);
		this.txtBuyerOrderNo.Name = "txtBuyerOrderNo";
		this.txtBuyerOrderNo.Size = new System.Drawing.Size(137, 23);
		this.txtBuyerOrderNo.TabIndex = 4;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(2, 99);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(61, 30);
		this.label27.TabIndex = 491;
		this.label27.Text = "Customer \r\nOrder No";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(2, 49);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(73, 15);
		this.label28.TabIndex = 465;
		this.label28.Text = "Sales Person";
		this.ApprovalDate.CustomFormat = "dd-MMM-yyyy";
		this.ApprovalDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ApprovalDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ApprovalDate.Location = new System.Drawing.Point(573, 17);
		this.ApprovalDate.Name = "ApprovalDate";
		this.ApprovalDate.Size = new System.Drawing.Size(137, 23);
		this.ApprovalDate.TabIndex = 15;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(2, 21);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 464;
		this.label4.Text = "Contract No";
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.White;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(479, 217);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(88, 15);
		this.label15.TabIndex = 503;
		this.label15.Text = "Contract Status";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.White;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(479, 21);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(83, 15);
		this.label43.TabIndex = 568;
		this.label43.Text = "Approval Date";
		this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(573, 211);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(137, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 26;
		this.datOrderDate.CustomFormat = "dd-MMM-yyyy";
		this.datOrderDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datOrderDate.Location = new System.Drawing.Point(312, 101);
		this.datOrderDate.Name = "datOrderDate";
		this.datOrderDate.Size = new System.Drawing.Size(165, 23);
		this.datOrderDate.TabIndex = 5;
		this.datOrderDate.ValueChanged += new System.EventHandler(datOrderDate_ValueChanged);
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(312, 17);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(165, 23);
		this.txtDocDate.TabIndex = 1;
		this.txtDocDate.ValueChanged += new System.EventHandler(txtDocDate_ValueChanged);
		this.CmbNotifyParty.AllowNull = (DefaultableBoolean)1;
		this.CmbNotifyParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbNotifyParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbNotifyParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbNotifyParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).Location = new System.Drawing.Point(93, 127);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).Name = "CmbNotifyParty";
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).Size = new System.Drawing.Size(384, 26);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).TabIndex = 6;
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(93, 72);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(384, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Leave += new System.EventHandler(CmbCustomer_Leave);
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
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(573, 43);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(137, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TextChanged += new System.EventHandler(CmbDeliveryTerm_TextChanged);
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
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Location = new System.Drawing.Point(93, 43);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Name = "CmbSalePerson";
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Size = new System.Drawing.Size(384, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(2, 133);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(70, 15);
		this.label9.TabIndex = 466;
		this.label9.Text = "Notify Party";
		this.txtShipmentDate.CustomFormat = "dd-MMM-yyyy";
		this.txtShipmentDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShipmentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtShipmentDate.Location = new System.Drawing.Point(93, 271);
		this.txtShipmentDate.Name = "txtShipmentDate";
		this.txtShipmentDate.Size = new System.Drawing.Size(137, 23);
		this.txtShipmentDate.TabIndex = 11;
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
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Location = new System.Drawing.Point(93, 156);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Name = "CmbImporterBank";
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Size = new System.Drawing.Size(384, 26);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).TabIndex = 7;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(2, 275);
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
		((System.Windows.Forms.Control)(object)this.CmbDocumentCustomGroup).Location = new System.Drawing.Point(1050, 269);
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
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Location = new System.Drawing.Point(312, 240);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Name = "CmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).TabIndex = 13;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(2, 162);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(83, 15);
		this.label18.TabIndex = 505;
		this.label18.Text = "Importer Bank";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(2, 191);
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
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Location = new System.Drawing.Point(312, 269);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Name = "CmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).TabIndex = 14;
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
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Location = new System.Drawing.Point(93, 185);
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Name = "CmbExporterbank";
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Size = new System.Drawing.Size(384, 26);
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).TabIndex = 8;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(479, 49);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(80, 15);
		this.label5.TabIndex = 499;
		this.label5.Text = "Delivery Term";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(230, 246);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(75, 15);
		this.label14.TabIndex = 501;
		this.label14.Text = "Loading Port";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(230, 275);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(68, 15);
		this.label17.TabIndex = 504;
		this.label17.Text = "Destination";
		this.panel28.BackColor = System.Drawing.Color.Teal;
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
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(1133, 1);
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
		this.ChkOnCommission.AutoSize = true;
		this.ChkOnCommission.Font = new System.Drawing.Font("Calibri", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkOnCommission.Location = new System.Drawing.Point(1046, 45);
		this.ChkOnCommission.Name = "ChkOnCommission";
		this.ChkOnCommission.Size = new System.Drawing.Size(112, 21);
		this.ChkOnCommission.TabIndex = 31;
		this.ChkOnCommission.Text = "On Commission";
		this.ChkOnCommission.UseVisualStyleBackColor = true;
		this.ChkOnCommission.Visible = false;
		this.CmbPaymentTerms.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerms.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerms.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerms.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerms.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Location = new System.Drawing.Point(100, 336);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Name = "CmbPaymentTerms";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Size = new System.Drawing.Size(137, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).TextChanged += new System.EventHandler(CmbPaymentTerms_TextChanged);
		this.label95.AutoSize = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.ForeColor = System.Drawing.Color.Red;
		this.label95.Location = new System.Drawing.Point(86, 338);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(14, 17);
		this.label95.TabIndex = 581;
		this.label95.Text = "*";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 342);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(84, 15);
		this.label7.TabIndex = 500;
		this.label7.Text = "Payment Term";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(243, 342);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(52, 15);
		this.label49.TabIndex = 570;
		this.label49.Text = "Remarks";
		this.txtPaymentRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPaymentRemarks.Location = new System.Drawing.Point(309, 338);
		this.txtPaymentRemarks.Name = "txtPaymentRemarks";
		this.txtPaymentRemarks.Size = new System.Drawing.Size(262, 23);
		this.txtPaymentRemarks.TabIndex = 18;
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1220, 30);
		this.panel3.TabIndex = 0;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.btnnew, this.BtnRefresh, this.BtnSaveAs, this.btnsave, this.btnUpdate, this.BtnDelete, this.btnAttachment, this.BtnPrintII, this.Print, this.BtnDeliveryTermLookUp,
			this.btnDefinePaymentTerm, this.btnSeaPortDefine, this.BtnContractPmDetailForm, this.BtnCommodityDetailForm, this.btnShortCutKey
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
		this.BtnRefresh.Click += new System.EventHandler(toolStripButton1_Click);
		this.BtnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveAs.Image = (System.Drawing.Image)resources.GetObject("BtnSaveAs.Image");
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(85, 24);
		this.BtnSaveAs.Text = "&Save As";
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
		this.BtnPrintII.Text = "501A-Print";
		this.BtnPrintII.Click += new System.EventHandler(BtnPrintII_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "501-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
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
		this.BtnContractPmDetailForm.Size = new System.Drawing.Size(172, 24);
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
		this.tabPage2.Size = new System.Drawing.Size(1226, 682);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1220, 270);
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
		this.tabControl3.Controls.Add(this.tabOtherChargesHistory);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(3, 422);
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
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance300).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance301).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance301).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance301;
		((AppearanceBase)appearance302).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance302;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance304).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance304).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance304).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance306).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance307).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance310;
		((AppearanceBase)appearance311).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance311).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance311;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance312).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance312).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance312).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance312).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance313).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance314;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance315;
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
		this.ctrlgrdOtherChargesDetail.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdOtherChargesDetail.FormName = this;
		this.ctrlgrdOtherChargesDetail.Location = new System.Drawing.Point(1170, 0);
		this.ctrlgrdOtherChargesDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdOtherChargesDetail.MyGrid = this.grdOtherChargesDetail;
		this.ctrlgrdOtherChargesDetail.Name = "ctrlgrdOtherChargesDetail";
		this.ctrlgrdOtherChargesDetail.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdOtherChargesDetail.TabIndex = 27;
		this.ctrlgrdOtherChargesDetail.Load += new System.EventHandler(ctrlgrdOtherChargesDetail_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1169, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(48, 23);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabOtherChargesHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory);
		this.tabOtherChargesHistory.Controls.Add(this.panel39);
		this.tabOtherChargesHistory.Location = new System.Drawing.Point(4, 25);
		this.tabOtherChargesHistory.Name = "tabOtherChargesHistory";
		this.tabOtherChargesHistory.Size = new System.Drawing.Size(1212, 228);
		this.tabOtherChargesHistory.TabIndex = 4;
		this.tabOtherChargesHistory.Text = "Other Charges";
		this.tabOtherChargesHistory.UseVisualStyleBackColor = true;
		this.panel39.BackColor = System.Drawing.Color.Teal;
		this.panel39.Controls.Add(this.label111);
		this.panel39.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel39.Location = new System.Drawing.Point(0, 0);
		this.panel39.Name = "panel39";
		this.panel39.Size = new System.Drawing.Size(1212, 25);
		this.panel39.TabIndex = 351;
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label111.ForeColor = System.Drawing.Color.White;
		this.label111.Location = new System.Drawing.Point(5, 5);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(333, 17);
		this.label111.TabIndex = 562;
		this.label111.Text = "Other Charges Detail of the selected above row";
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
		((System.Windows.Forms.Control)(object)this.grdOtherChargesHistory).TabIndex = 352;
		this.grdOtherChargesHistory.TotalRow = (InheritableBoolean)1;
		this.grdOtherChargesHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherChargesHistory).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1234, 711);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "FrmExportSalesContract";
		this.Text = "Export Contract";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FrmExportSalesContract_FormClosing);
		base.Load += new System.EventHandler(FrmExportSalesContract_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(FrmExportSalesContract_KeyDown);
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
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingNTrade).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcomditiyDetailByItemCus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		this.tabPaymentTerm.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermDetail).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.cmbBrandForPmEntry).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUomPm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandPackingTypePm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPMItem).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.tabOtherItems.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.CmbFCYCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterbank).EndInit();
		this.panel28.ResumeLayout(false);
		this.panel28.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExportCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).EndInit();
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
		this.tabOtherChargesHistory.ResumeLayout(false);
		this.panel39.ResumeLayout(false);
		this.panel39.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherChargesHistory).EndInit();
		base.ResumeLayout(false);
	}
}
