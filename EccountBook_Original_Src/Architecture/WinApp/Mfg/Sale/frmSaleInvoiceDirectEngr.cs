using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Mfg;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Mfg.Sale;

public class frmSaleInvoiceDirectEngr : Form
{
	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects FormRights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtTerms = new DataTable();

	private DataTable dtWarehouse = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtTaxType = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtCastingType = new DataTable();

	private DataTable dtProductionStage = new DataTable();

	private DataTable dtTransporter = new DataTable();

	private DataTable dthistory = new DataTable();

	private DataTable dtSampleModal = new DataTable();

	private int VoucherHeadId;

	private int RecId;

	private int SaleOrderId;

	private int BaseCurrency;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int OutwardFreightAccountId;

	private double BaseRate;

	private bool HasMultiCurrencyFeature;

	private bool TaxPercntEditableOrNot;

	private int SupplierId;

	private bool FreightDebitToExpenses;

	private int updateDetailIndex = -1;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool BranchFeature;

	private bool BranchImplemented;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "";

	private double LedgerBalance;

	private frmLoadSaleOrderEngr LoadSo;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel2;

	private TabPage tabHistory;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private ToolStripButton toolStripButton3;

	private Label label23;

	private Panel panel4;

	private Panel panel5;

	private Label label20;

	private ComboBox cmbcommtype;

	private Label label26;

	private Label label11;

	private TextBox txtcommremarks;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private ComboBox cmbcommuom;

	private Label label19;

	private TextBox txtcommamount;

	private Label label7;

	private Label label2;

	private Label label8;

	private Label label6;

	private TextBox txtbillno;

	private Label label17;

	private TextBox txtdocno;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grd;

	internal GridEX grdGLedger;

	private TextBox txtBillAmount;

	private Label label3;

	internal UltraCombo cmbstockprty;

	internal UltraCombo cmbrefprty;

	internal UltraCombo cmbsuppliername;

	internal UltraCombo cmbcommagent;

	private Label label4;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Label label9;

	private ToolStripButton btnSaleInvoice1;

	private CheckBox ChkBok;

	private Panel panel14;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnFrmRefresh;

	private Label label12;

	internal UltraCombo CmbTaxAccount;

	private TextBox txtDueDays;

	private DateTimePicker DueDate;

	private Panel panel10;

	internal GridEX grdDetail;

	private ToolStripButton btnSaleinvoice2;

	private ToolStripButton btnDelete;

	private GroupBox groupBox1;

	internal UltraCombo CmbPaymentTerm;

	private Label label1;

	internal UltraCombo CmbDeliveryTerm;

	private Label label5;

	private Label label28;

	private TextBox txtRefrenceNo;

	private GroupBox groupBox2;

	internal UltraCombo CmbCity;

	private Label label53;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private TextBox txtgatepassno;

	private Button btnAdd;

	private Label label50;

	private TextBox txtNetRate;

	private Label label49;

	private TextBox txtAddLessRate;

	private Label label48;

	private TextBox txtvehicleno;

	private Label label47;

	internal UltraCombo CmbWarehouse;

	internal UltraCombo CmbRateUom;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbItem;

	private DateTimePicker txtgpdate;

	private Label label42;

	private TextBox txtRate;

	private TextBox txtGrossWeight;

	private Label label39;

	private TextBox txtItemQty;

	private Label label38;

	private Label label35;

	private TextBox txtNetBillWeight;

	private Label label36;

	private Label label32;

	private Label label33;

	private TextBox txtAddLss;

	private Label label31;

	private Label label30;

	private Label label10;

	private Label label14;

	private TextBox txtAmount;

	private Label label15;

	private Panel DetailPanel;

	internal UltraCombo CmbTaxType;

	private Label lblTotalAmount;

	private TextBox txtTaxPercent;

	private TextBox txtTotalAmount;

	private Label lblTaxPercent;

	private TextBox txtTaxAmount;

	private Label lblTaxTypes;

	private Label lblTaxAmount;

	private Panel panel7;

	private Panel panel18;

	private Label label24;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label27;

	private CheckBox ChkVoucherPreview;

	private TextBox txtDiscountAmount;

	private Label label41;

	private TextBox txtDiscPercent;

	private Label label40;

	internal UltraCombo CmbDiscountType;

	private Label label37;

	private Label label43;

	private TextBox txtExchangeRate;

	private TextBox txtFcyAmount;

	private Label label44;

	internal UltraCombo cmbCurrency;

	private Label label45;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnshortcutkeys;

	private ToolStripSeparator toolStripSeparator7;

	private TextBox txtRemarks;

	private Panel panel8;

	private GroupBox groupBox4;

	private Button btnShow;

	internal UltraCombo CmbPaymentTermHistory;

	private Label label52;

	internal UltraCombo CmbCustomerHistory;

	private Label label18;

	private TextBox ToDocNoHistory;

	private Label label46;

	private TextBox FromDocNoHistory;

	private Label label22;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label54;

	private ToolStripButton btnResetHistory;

	private ToolStripButton BtnRefreshHistory;

	private Label label55;

	internal UltraCombo CmbFreightAcCr;

	private Label label56;

	internal UltraCombo cmbFreightAcDr;

	private TextBox txtFreightRemarks;

	private Label label57;

	private TextBox txtFreightAmount;

	private Label label58;

	private TabControl tabControl2;

	private TabPage DetailGrid;

	private TabPage tabPaymentDetail;

	private TabPage tabExpenses;

	private TabPage tabPage2;

	private Panel panel28;

	private Panel panel29;

	internal GridEX grdPaymentTerm;

	private Panel panel17;

	private CtrlGrdBar ctrlGrdBar4;

	private Panel panel16;

	private Panel panel13;

	internal GridEX grdInvExp;

	private Panel panel21;

	private CtrlGrdBar ctrlGrdBar5;

	private CtrlGrdBar ctrlGrdBar3;

	private DateTimePicker DocDate;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label60;

	internal UltraCombo CmbDiscountAccount;

	private Label label61;

	private TextBox txtDiscountAmountHeader;

	private Label label62;

	private TextBox txtCustomerCity;

	internal UltraCombo CmbVariantDescription;

	private Label label76;

	internal UltraCombo CmbCategory;

	private Label label64;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private TextBox txtremarksdetail;

	private Label label65;

	internal UltraCombo cmbBranchName;

	private Label label66;

	private Button BtnDefineVariant;

	internal UltraCombo CmbCastingTypeDetail;

	private Label label77;

	internal UltraCombo CmbWarehouseForUpdateInGrid;

	private Label label51;

	private Button BtnUpdateInGridDetail;

	private TextBox txtCustomerCellNo;

	private Label label13;

	private TextBox txtBranchSrNo;

	private TextBox txtLedgerBalance;

	private TextBox txtLedgerBalanceAfterBill;

	private Label label63;

	private Label label67;

	private ToolStripButton btnCity;

	private ToolStripButton BtnDefineItem;

	internal UltraCombo CmbSampleModal;

	private Label label68;

	internal UltraCombo CmbProductionStage;

	private Label label16;

	private TextBox lblBalance;

	private Label label29;

	private Button btnRecordsUpdate;

	public frmSaleInvoiceDirectEngr(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		InitializeComponentCustom();
		InitializeComponentMethod();
		UserAccount = user;
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1130, 650);
			ScreenName = Conversion.ToString((object)base.Name);
			dtCity.Columns.Add("Id", typeof(int));
			dtCity.Columns.Add("CityName", typeof(string));
			dtCastingType.Columns.Add("Id", typeof(int));
			dtCastingType.Columns.Add("CastingType", typeof(string));
			dtProductionStage.Columns.Add("Id", typeof(int));
			dtProductionStage.Columns.Add("ProductionStage", typeof(string));
			dtWarehouse.Columns.Add("Id", typeof(int));
			dtWarehouse.Columns.Add("WareHouseName", typeof(string));
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitem.Columns.Add("ItemCategoryId", typeof(int));
			dtitem.Columns.Add("itemModalId", typeof(int));
			dtitem.Columns.Add("FinishWeight", typeof(double));
			if (dtSampleModal.Columns.Count == 0)
			{
				dtSampleModal.Columns.AddRange(new DataColumn[5]
				{
					new DataColumn("Id", typeof(int)),
					new DataColumn("SampleNo"),
					new DataColumn("ItemModal"),
					new DataColumn("ItemCategoryId", typeof(int)),
					new DataColumn("ItemId", typeof(int))
				});
			}
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("AccountTitle");
			dtAccountlst.Columns.Add("SupplierCustomerId");
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtTransporter.Columns.Add("Id", typeof(int));
			dtTransporter.Columns.Add("AccountTitle", typeof(string));
			dtTransporter.Columns.Add("AccountCode", typeof(string));
			dtTransporter.Columns.Add("SupplierCustomerId", typeof(int));
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("OrderId", typeof(int));
			dtGrid.Columns.Add("OrderDetailId", typeof(int));
			dtGrid.Columns.Add("OrderNo", typeof(int));
			dtGrid.Columns.Add("OrderDate", typeof(DateTime));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemCode");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("PackUomId", typeof(int));
			dtGrid.Columns.Add("PackUom");
			dtGrid.Columns.Add("PackEquivalent", typeof(double));
			dtGrid.Columns.Add("VariantId", typeof(int));
			dtGrid.Columns.Add("VariantDescription", typeof(string));
			dtGrid.Columns.Add("CastingTypeId", typeof(int));
			dtGrid.Columns.Add("ProductionStageId", typeof(int));
			dtGrid.Columns.Add("Remarks", typeof(string));
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("NetWeight", typeof(double));
			dtGrid.Columns.Add("ItemRate", typeof(double));
			dtGrid.Columns.Add("RateUomId", typeof(int));
			dtGrid.Columns.Add("RateUom");
			dtGrid.Columns.Add("RateEquivalent", typeof(double));
			dtGrid.Columns.Add("AddLessRate", typeof(double));
			dtGrid.Columns.Add("NetRate", typeof(double));
			dtGrid.Columns.Add("DiscountType", typeof(int));
			dtGrid.Columns.Add("Discount%", typeof(double));
			dtGrid.Columns.Add("DiscountAmount", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("FcyAmount", typeof(double));
			dtGrid.Columns.Add("TaxTypeId", typeof(int));
			dtGrid.Columns.Add("Tax%", typeof(double));
			dtGrid.Columns.Add("TaxAmount", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("GpNo");
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("CityName");
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("GlAccountId");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage", typeof(double));
			dtGrdGL.Columns.Add("Qty", typeof(double));
			dtGrdGL.Columns.Add("Rate", typeof(double));
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			dtInvExp.Columns.Add("ItemId");
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			dtPaymentTerm.Columns.Add("PaymentTermId", typeof(int));
			dtPaymentTerm.Columns.Add("%ofTotal", typeof(double));
			dtPaymentTerm.Columns.Add("Amount", typeof(double));
			dtPaymentTerm.Columns.Add("DueDays", typeof(int));
			dtPaymentTerm.Columns.Add("DueDate", typeof(DateTime));
			dtPaymentTerm.Columns.Add("Remarks");
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			DocDate.Focus();
			ChkBok.Checked = true;
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
			FormRights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			BranchFeature = CommonServices.GetERPFeatureById(11);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "TaxPercentEditable");
			TaxPercntEditableOrNot = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DebitAmountChargetoExpenseAcFreightGrid");
			FreightDebitToExpenses = Conversion.ToBool((object)((obj2 != null) ? obj2.ConfigKey : null));
			ConfigrationsAllocation obj3 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "SaleInvoiceDirectBranchWise");
			BranchImplemented = Conversion.ToBool((object)((obj3 != null) ? obj3.ConfigKey : null));
			ConfigrationsAllocation obj4 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj4 != null) ? obj4.ConfigKey : null));
			ConfigrationsAllocation obj5 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "FreightOutwardAc");
			OutwardFreightAccountId = Conversion.ToInt((object)((obj5 != null) ? obj5.ConfigKey : null));
			string documentNoData = DocumentNoDBCall();
			string branchNoData = BranchSrNoDBCall();
			DataTable itemCategoryData = ItemCategoryDBCall();
			SampleModaldBCall();
			AllTaxTypeDBCall();
			int previousDiscountAcIdData = PreviousDiscountAcIdWithDbCall();
			OtherItemsDBcall();
			int previousSalesTaxAcIdData = PreviousSalesTaxAcWithDbCall();
			return new
			{
				documentNoData = documentNoData,
				branchNoData = branchNoData,
				ItemCategoryData = itemCategoryData,
				PreviousDiscountAcIdData = previousDiscountAcIdData,
				PreviousSalesTaxAcIdData = previousSalesTaxAcIdData
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
				btnSave.Enabled = FormRights.DoHaveSaveRight;
				btnPrint.Enabled = FormRights.DoHavePrintRights;
				btnUpdate.Enabled = FormRights.DoHaveUpdateRights;
				btnDelete.Enabled = FormRights.DoHaveCanDelete;
				ctrlGrdBar1.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
				ctrlGrdBar2.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
				ctrlGrdBar2.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
				ctrlGrdBar3.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
				ctrlGrdBar3.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
				ctrlGrdBar4.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
				ctrlGrdBar4.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
				ctrlGrdBar5.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
				ctrlGrdBar5.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
					UpdateBranchSrNoUI(result.branchNoData);
				}
				SupplierDtFillFromGlobal();
				BindSupplierName();
				FreightAccountDebitAndCreditDtFill();
				BindFreightAccountDebitAndCredit();
				BindTaxAccount();
				PaymentTermDtFillFromGlobal();
				BindPaymentTerm();
				BindDeliveryTerm();
				BindDiscountAccount();
				BindCurrency();
				WarehousesDtFillFromGlobal();
				BindWareHouse();
				ItemCategoryFill(result.ItemCategoryData);
				ItemModalBind();
				ItemDtFillFromGlobal();
				ItemDetailBind();
				CastingTypeDtFillFromGlobal();
				BindCastingType();
				BindProductionStage();
				BindDiscountType();
				BindTaxType();
				CityDtFillFromGlobal();
				AccountlstDtFillFromGlobal();
				BindCityName();
				int num = Conversion.ToInt((object)result.PreviousDiscountAcIdData);
				if (num > 0)
				{
					CmbDiscountAccount.Value = num;
				}
				int num2 = Conversion.ToInt((object)result.PreviousSalesTaxAcIdData);
				if (num2 > 0)
				{
					CmbTaxAccount.Value = num2;
				}
				if (dtGrid != null && dtGrid.Columns.Count > 0)
				{
					grd.DataSource = dtGrid;
					grd.RetrieveStructure();
					grdSettings();
				}
				if (dtGrdGL != null && dtGrdGL.Columns.Count > 0)
				{
					AddRowInGLGrid();
					grdGLedger.RetrieveStructure();
					gridGLSettings();
				}
				if (dtInvExp != null && dtInvExp.Columns.Count > 0)
				{
					AddRowInvExpGrid();
					grdInvExp.RetrieveStructure();
					grdInvExpSettings();
				}
				if (dtPaymentTerm != null && dtPaymentTerm.Columns.Count > 0)
				{
					grdPaymentTerm.DataSource = dtPaymentTerm;
					grdPaymentTerm.RetrieveStructure();
					grdSettingsPaymentTerm();
					AddPaymentRowsRowsInPaymentGrid();
				}
				HistoryCombosBranchFill();
				HistoryComboFill(ValidateBranch: false);
				((Control)(object)cmbBranchName).Enabled = BranchFeature && !BranchImplemented;
				txtBranchSrNo.Visible = BranchFeature && BranchImplemented;
				txtdocno.Width = ((BranchFeature && BranchImplemented) ? txtdocno.Width : (txtdocno.Width + txtBranchSrNo.Width));
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				if (OutwardFreightAccountId > 0 && ((DataTable)((UltraGridBase)CmbFreightAcCr).DataSource).AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OutwardFreightAccountId))
				{
					CmbFreightAcCr.Value = OutwardFreightAccountId;
				}
				MultiCurrencyFeature();
				ConfigurationDefault();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private string DocumentNoDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				DocumentTypeId = 1661,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtdocno.Text = documentData;
	}

	private string BranchSrNoDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)InvSaleInvoice.GenerateInvSaleInvoiceCodeBranch(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1661,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateBranchSrNoUI(string documentData)
	{
		txtBranchSrNo.Text = documentData;
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			List<getGlobalSupplierCustomer> obj = clsGlobalVariables.globalSupplierCustomer;
			dtSupplier.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				List<getGlobalSupplierCustomer> lst = obj.Where((getGlobalSupplierCustomer r) => r.PartyTypeId == 2).ToList();
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtSupplier.Rows.Add(Conversion.ToInt((object)lst[i].Id), Conversion.ToString((object)lst[i].CompanyName), Conversion.ToString((object)lst[i].PartyCode), Conversion.ToInt((object)lst[i].GlAccountId), Conversion.ToInt((object)lst[i].CityId), Conversion.ToString((object)lst[i].CityName), Conversion.ToString((object)lst[i].MobilePersonal));
					}
				}
			}
			else if (obj != null && obj.Count > 0)
			{
				for (int j = 0; j < obj.Count; j++)
				{
					dtSupplier.Rows.Add(Conversion.ToInt((object)obj[j].Id), Conversion.ToString((object)obj[j].CompanyName), Conversion.ToString((object)obj[j].PartyCode), Conversion.ToInt((object)obj[j].GlAccountId), Conversion.ToInt((object)obj[j].CityId), Conversion.ToString((object)obj[j].CityName), Conversion.ToString((object)obj[j].MobilePersonal));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			int SupId = Conversion.ToInt(cmbsuppliername.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, cmbsuppliername, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				if (SupId > 0)
				{
					CommonServices.SetComboValue(cmbsuppliername, "Id", SupId, dtSupplier);
				}
			}
			else
			{
				((Control)(object)cmbsuppliername).Text = string.Empty;
				((UltraGridBase)cmbsuppliername).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FreightAccountDebitAndCreditDtFill()
	{
		try
		{
			dtTransporter.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				CommonServices.GetVendorsAndCustomersForTransporter();
				List<getGlobalVendorsAndCustomersForTransporter> lst = clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters;
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtTransporter.Rows.Add(lst[i].GlAccountId, lst[i].CompanyName, lst[i].PartyCode, lst[i].Id);
					}
				}
				return;
			}
			int[] excludedIds = ((!FreightDebitToExpenses) ? new int[9] { 2, 11, 12, 13, 14, 15, 20, 21, 22 } : new int[3] { 2, 15, 22 });
			IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => !excludedIds.Contains(r.AccountTypeId));
			HashSet<int> existingIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc in enumerable)
			{
				int id = Conversion.ToInt((object)acc.ChartOfAccountId);
				if (!existingIds.Contains(id))
				{
					dtTransporter.Rows.Add(acc.ChartOfAccountId, acc.AccountTitle, acc.AccountCode, 0);
				}
				existingIds.Add(id);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindFreightAccountDebitAndCredit()
	{
		try
		{
			int Id = Conversion.ToInt(cmbFreightAcDr.Value);
			int Id2 = Conversion.ToInt(CmbFreightAcCr.Value);
			if (dtTransporter.Rows.Count > 0)
			{
				DDL.BindDDL(dtTransporter, cmbFreightAcDr, "Id", "AccountTitle", "Freight Ac Dr", ZeroIndex: true);
				((UltraGridBase)cmbFreightAcDr).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (Id > 0)
				{
					CommonServices.SetComboValue(cmbFreightAcDr, "Id", Id, dtTransporter);
				}
				DDL.BindDDL(dtTransporter, CmbFreightAcCr, "Id", "AccountTitle", "Freight Ac Cr", ZeroIndex: true);
				((UltraGridBase)CmbFreightAcCr).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (Id2 > 0)
				{
					CommonServices.SetComboValue(CmbFreightAcCr, "Id", Id2, dtTransporter);
				}
			}
			else
			{
				((UltraGridBase)cmbFreightAcDr).DataSource = null;
				((Control)(object)cmbFreightAcDr).Text = string.Empty;
				((UltraGridBase)CmbFreightAcCr).DataSource = null;
				((Control)(object)CmbFreightAcCr).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindTaxAccount()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTaxAccount.Value);
			((Control)(object)CmbTaxAccount).Text = string.Empty;
			((UltraGridBase)CmbTaxAccount).DataSource = null;
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[7] { 3, 6, 8, 16, 17, 18, 19 });
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTaxAccount, "Id", "AccountTitle", "Tax Account", ZeroIndex: false);
				if (Id > 0)
				{
					CommonServices.SetComboValue(CmbTaxAccount, "Id", Id, dt);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermDtFillFromGlobal()
	{
		try
		{
			List<InvDueTerms> lst = clsGlobalVariables.globalPaymentTerm;
			dtTerms.Rows.Clear();
			if (dtTerms.Columns.Count == 0)
			{
				dtTerms.Columns.Add("Id", typeof(int));
				dtTerms.Columns.Add("TermsDescription", typeof(string));
			}
			if (lst != null && lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					dtTerms.Rows.Add(lst[i].Id, lst[i].TermsDescription);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPaymentTerm()
	{
		try
		{
			int Id = Conversion.ToInt(CmbPaymentTerm.Value);
			if (dtTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerms, CmbPaymentTerm, "Id", "TermsDescription", "Payment Term", false);
				((UltraGridBase)CmbPaymentTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbPaymentTerm).Rows[1].Activate();
				if (Id > 0)
				{
					CommonServices.SetComboValue(CmbPaymentTerm, "Id", Id, dtTerms);
				}
			}
			else
			{
				((Control)(object)CmbPaymentTerm).Text = string.Empty;
				((UltraGridBase)CmbPaymentTerm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDeliveryTerm()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, CmbDeliveryTerm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)CmbDeliveryTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbDeliveryTerm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDiscountAccount()
	{
		try
		{
			int ID = Conversion.ToInt(CmbDiscountAccount.Value);
			((Control)(object)CmbDiscountAccount).Text = string.Empty;
			((UltraGridBase)CmbDiscountAccount).DataSource = null;
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 4 });
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDiscountAccount, "Id", "AccountTitle", "Discount Account", ZeroIndex: true);
				if (ID > 0)
				{
					CommonServices.SetComboValue(CmbDiscountAccount, "Id", ID, dt);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCurrency()
	{
		try
		{
			List<MultiCurrency> lst = clsGlobalVariables.globalMultiCurrency;
			int id = Conversion.ToInt(cmbCurrency.Value);
			((Control)(object)cmbCurrency).Text = string.Empty;
			if (lst != null && lst.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CurrencyCode", typeof(string));
				for (int i = 0; i < lst.Count; i++)
				{
					dt.Rows.Add(lst[i].Id, lst[i].CurrencyCode);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", ZeroIndex: true);
					((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					CommonServices.SetComboValue(cmbCurrency, "Id", id, dt);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WarehousesDtFillFromGlobal()
	{
		try
		{
			List<GlobalWarehousesWithBranch> lst = clsGlobalVariables.globalWarehousesWithBranches;
			dtWarehouse.Clear();
			if (lst != null && lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					dtWarehouse.Rows.Add(lst[i].WarehouseId, lst[i].WarehouseName);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindWareHouse()
	{
		try
		{
			int ID = Conversion.ToInt(CmbWarehouse.Value);
			int Id2 = Conversion.ToInt(CmbWarehouseForUpdateInGrid.Value);
			if (dtWarehouse.Rows.Count > 0)
			{
				DDL.BindDDL(dtWarehouse, CmbWarehouse, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
				CommonServices.SetComboValue(CmbWarehouse, "Id", ID, dtWarehouse);
				DDL.BindDDL(dtWarehouse, CmbWarehouseForUpdateInGrid, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
				CommonServices.SetComboValue(CmbWarehouseForUpdateInGrid, "Id", Id2, dtWarehouse);
			}
			else
			{
				((Control)(object)CmbWarehouse).Text = string.Empty;
				((UltraGridBase)CmbWarehouse).DataSource = null;
				((Control)(object)CmbWarehouseForUpdateInGrid).Text = string.Empty;
				((UltraGridBase)CmbWarehouseForUpdateInGrid).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ItemCategoryDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ItemCategory.Getall(new ItemCategory
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

	private void ItemCategoryFill(DataTable dtItemCategory)
	{
		try
		{
			int SupId = Conversion.ToInt(CmbCategory.Value);
			if (dtItemCategory.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemCategory, CmbCategory, "Id", "CategoryDescription", "ItemCategory", false);
				CommonServices.SetComboValue(CmbCategory, "Id", SupId, dtItemCategory);
			}
			else
			{
				((Control)(object)CmbCategory).Text = string.Empty;
				((UltraGridBase)CmbCategory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SampleModaldBCall()
	{
		try
		{
			dtSampleModal.Rows.Clear();
			DataTable dt = ItemModel.ItemModel_WithItemId(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtSampleModal.Rows.Add(dt.Rows[i]["itemModalId"], dt.Rows[i]["ItemSampleNo"], dt.Rows[i]["ItemModal"], dt.Rows[i]["ItemCategoryId"], dt.Rows[i]["ItemId"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemModalBind(int CategoryId = 0, int itemId = 0)
	{
		try
		{
			int Id = Conversion.ToInt(CmbSampleModal.Value);
			DataTable FilterItems = new DataTable();
			if (itemId > 0)
			{
				DataRow[] filteredRows = dtSampleModal.Select($"ItemId = {itemId}");
				FilterItems = (filteredRows.Any() ? filteredRows.CopyToDataTable() : new DataTable());
			}
			else if (CategoryId > 0)
			{
				DataRow[] filteredRows2 = dtSampleModal.Select($"ItemCategoryId = {CategoryId}");
				FilterItems = (filteredRows2.Any() ? filteredRows2.CopyToDataTable() : new DataTable());
			}
			else
			{
				FilterItems = dtSampleModal;
			}
			if (FilterItems.Rows.Count > 0)
			{
				DDL.BindDDL(FilterItems, CmbSampleModal, "Id", "SampleNo", "SampleNo", ZeroIndex: true);
				((UltraGridBase)CmbSampleModal).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbSampleModal).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbSampleModal).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbSampleModal).Rows[1].Activate();
				if (Id > 0)
				{
					if (FilterItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbSampleModal.Value = Id;
					}
					else
					{
						((Control)(object)CmbSampleModal).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbSampleModal).Text = string.Empty;
				((UltraGridBase)CmbSampleModal).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemDtFillFromGlobal()
	{
		try
		{
			List<getGlobalAllItems> dt = clsGlobalVariables.getGlobalAllItems;
			dtitem.Clear();
			if (dt != null && dt.Count > 0)
			{
				for (int i = 0; i < dt.Count; i++)
				{
					dtitem.Rows.Add(dt[i].Id, dt[i].ItemName, dt[i].ItemCode, dt[i].ItemCategoryId, dt[i].itemModalId, dt[i].FinishWeight);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailBind(int CategoryId = 0, int itemModalId = 0)
	{
		try
		{
			int Id = Conversion.ToInt(CmbItem.Value);
			DataTable FilterItems = new DataTable();
			if (itemModalId > 0)
			{
				DataRow[] filteredRows = dtitem.Select($"itemModalId = {itemModalId}");
				FilterItems = (filteredRows.Any() ? filteredRows.CopyToDataTable() : new DataTable());
			}
			else if (CategoryId > 0)
			{
				DataRow[] filteredRows2 = dtitem.Select($"ItemCategoryId = {CategoryId}");
				FilterItems = (filteredRows2.Any() ? filteredRows2.CopyToDataTable() : new DataTable());
			}
			else
			{
				FilterItems = dtitem;
			}
			if (FilterItems.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(FilterItems, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(FilterItems, CmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				if (Id > 0)
				{
					if (FilterItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbItem.Value = Id;
					}
					else
					{
						((UltraGridBase)CmbItem).Rows[1].Activate();
					}
				}
				else
				{
					((UltraGridBase)CmbItem).Rows[1].Activate();
				}
			}
			else
			{
				((Control)(object)CmbItem).Text = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindPackUomAndRateUom()
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			string Rateuom = ((Control)(object)CmbRateUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			DataTable dtUom = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(CmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUom, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				DDL.BindDDL(dtUom, CmbRateUom, "Id", "UOMCode", "RateUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 85;
				((UltraGridBase)CmbPackUom).Rows[0].Activate();
				((UltraGridBase)CmbRateUom).Rows[0].Activate();
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUom).Text = Packuom;
				}
				else
				{
					((UltraGridBase)CmbPackUom).Rows[0].Activate();
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)CmbRateUom).Text = Rateuom;
				}
				else
				{
					((UltraGridBase)CmbRateUom).Rows[0].Activate();
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
			if (TaxPercntEditableOrNot)
			{
				txtTaxPercent.Enabled = true;
			}
			else
			{
				txtTaxPercent.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindvarientunit(int ID)
	{
		try
		{
			string ItId = Conversion.ToString((object)((Control)(object)CmbVariantDescription).Text);
			DataTable dtunit = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ID);
			if (dtunit.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("VariantId", typeof(int));
				dt.Columns.Add("VariantDescription");
				dt.Clear();
				for (int i = 0; i < dtunit.Rows.Count; i++)
				{
					dt.Rows.Add(dtunit.Rows[i]["ItemAttributeVarientId"], dtunit.Rows[i]["VarientDescription"]);
				}
				DDL.BindDDL(dt, CmbVariantDescription, "VariantId", "VariantDescription", "VariantDescription", ZeroIndex: true);
				((UltraGridBase)CmbVariantDescription).Rows[1].Activate();
				if (ItId != null && ItId != string.Empty && ItId != "...Select Any Value...")
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("VariantDescription") == ItId))
					{
						((Control)(object)CmbVariantDescription).Text = ItId;
					}
					else
					{
						((UltraGridBase)CmbVariantDescription).Rows[1].Activate();
					}
				}
			}
			else
			{
				((Control)(object)CmbVariantDescription).Text = string.Empty;
				((UltraGridBase)CmbVariantDescription).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CastingTypeDtFillFromGlobal()
	{
		try
		{
			List<LookUps> lst = clsGlobalVariables.GetAllLookUps_Engr;
			dtCastingType.Clear();
			dtProductionStage.Clear();
			if (lst == null || lst.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i].LookUpTypeId == 1)
				{
					dtCastingType.Rows.Add(lst[i].Id, lst[i].LookupName);
				}
				if (lst[i].LookUpTypeId == 3)
				{
					dtProductionStage.Rows.Add(lst[i].Id, lst[i].LookupName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCastingType()
	{
		try
		{
			int DetailId = Conversion.ToInt(CmbCastingTypeDetail.Value);
			if (dtCastingType.Rows.Count > 0)
			{
				DDL.BindDDL(dtCastingType, CmbCastingTypeDetail, "Id", "CastingType", "Casting Type", ZeroIndex: true);
				((UltraGridBase)CmbCastingTypeDetail).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbCastingTypeDetail).Rows[1].Activate();
				if (DetailId > 0)
				{
					if (dtCastingType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DetailId))
					{
						CmbCastingTypeDetail.Value = DetailId;
					}
					else
					{
						((UltraGridBase)CmbCastingTypeDetail).Rows[1].Activate();
					}
				}
			}
			else
			{
				((Control)(object)CmbCastingTypeDetail).Text = string.Empty;
				((UltraGridBase)CmbCastingTypeDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindProductionStage()
	{
		try
		{
			int DetailId = Conversion.ToInt(CmbProductionStage.Value);
			if (dtProductionStage.Rows.Count > 0)
			{
				DDL.BindDDL(dtProductionStage, CmbProductionStage, "Id", "ProductionStage", "Production Stage", ZeroIndex: true);
				((UltraGridBase)CmbProductionStage).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbProductionStage).Rows[1].Activate();
				if (DetailId > 0)
				{
					if (dtProductionStage.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DetailId))
					{
						CmbProductionStage.Value = DetailId;
					}
					else
					{
						((UltraGridBase)CmbProductionStage).Rows[1].Activate();
					}
				}
			}
			else
			{
				((Control)(object)CmbProductionStage).Text = string.Empty;
				((UltraGridBase)CmbProductionStage).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDiscountType()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			DDL.BindDDL(dt, CmbDiscountType, "Id", "Name", "Discount Type", ZeroIndex: false);
			((UltraGridBase)CmbDiscountType).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllTaxTypeDBCall()
	{
		try
		{
			dtTaxType.Rows.Clear();
			dtTaxType = ItemTaxSchedule.GetAllTaxSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, DocDate.Value, 2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindTaxType()
	{
		try
		{
			int DetailId = Conversion.ToInt(CmbTaxType.Value);
			if (dtTaxType.Rows.Count > 0)
			{
				DDL.BindDDL(dtTaxType, CmbTaxType, "TaxNameId", "TaxName", "Tax Type", ZeroIndex: false);
				((UltraGridBase)CmbTaxType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbTaxType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (DetailId > 0)
				{
					if (dtTaxType.AsEnumerable().Any((DataRow row) => row.Field<int>("TaxNameId") == DetailId))
					{
						CmbTaxType.Value = DetailId;
					}
					else
					{
						((Control)(object)CmbTaxType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbTaxType).Text = string.Empty;
				((UltraGridBase)CmbTaxType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityDtFillFromGlobal()
	{
		try
		{
			List<GlobalAllCity> obj = clsGlobalVariables.globalAllCities;
			dtCity.Clear();
			if (obj != null && obj.Count > 0)
			{
				for (int i = 0; i < obj.Count; i++)
				{
					dtCity.Rows.Add(obj[i].Id, obj[i].CityName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCityName()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCity.Value);
			if (dtCity.Rows.Count > 0)
			{
				DDL.BindDDL(dtCity, CmbCity, "Id", "CityName", "City Name", ZeroIndex: true);
				((UltraGridBase)CmbCity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id > 0)
				{
					if (dtCity.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCity.Value = Id;
					}
					else
					{
						((Control)(object)CmbCity).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCity).Text = string.Empty;
				((UltraGridBase)CmbCity).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private int PreviousDiscountAcIdWithDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return InvSaleInvoice.GetLastDiscountAcId(new ReportsParameters
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

	private void BindCustomerCity()
	{
		try
		{
			txtCustomerCity.Text = string.Empty;
			txtCustomerCellNo.Text = string.Empty;
			int Id = Conversion.ToInt(cmbsuppliername.Value);
			if (((UltraGridBase)cmbsuppliername).ActiveRow == null || Id <= 0)
			{
				return;
			}
			string CustomerCellNo = Conversion.ToString(((UltraDropDownBase)cmbsuppliername).SelectedRow.Cells["MobileNo"].Value);
			txtCustomerCellNo.Text = CustomerCellNo;
			string CityName = Conversion.ToString(((UltraDropDownBase)cmbsuppliername).SelectedRow.Cells["CityName"].Value);
			txtCustomerCity.Text = CityName;
			if (!string.IsNullOrEmpty(CityName) && Conversion.ToInt(CmbCity.Value) == 0 && ((UltraGridBase)CmbCity).DataSource is DataTable dt && dt.Rows.Count > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("CityName") != null && row.Field<string>("CityName") == CityName))
				{
					((Control)(object)CmbCity).Text = CityName;
				}
				else
				{
					((Control)(object)CmbCity).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountlstDtFillFromGlobal()
	{
		try
		{
			dtAccountlst.Rows.Clear();
			List<getGlobalAllAccountsWithCustomGroup> dtcoa = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => r.AccountTypeId == 3 || r.AccountTypeId == 6 || r.AccountTypeId == 8 || r.AccountTypeId == 16 || r.AccountTypeId == 17 || r.AccountTypeId == 18 || r.AccountTypeId == 19).ToList();
			if (dtcoa == null || dtcoa.Count <= 0)
			{
				return;
			}
			HashSet<int> existingIds = new HashSet<int>();
			for (int i = 0; i < dtcoa.Count; i++)
			{
				int id = Conversion.ToInt((object)dtcoa[i].ChartOfAccountId);
				if (!existingIds.Contains(id))
				{
					dtAccountlst.Rows.Add(dtcoa[i].ChartOfAccountId, dtcoa[i].AccountTitle);
				}
				existingIds.Add(id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsDBcall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private int PreviousSalesTaxAcWithDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return InvSaleInvoice.GetLastSaleTaxAcId(new ReportsParameters
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

	private void FillItemFinishWeight(int Id)
	{
		try
		{
			double FinishWeight = 0.0;
			if (((UltraGridBase)CmbItem).ActiveRow != null && Id > 0)
			{
				FinishWeight = Conversion.ToDouble(((UltraDropDownBase)CmbItem).SelectedRow.Cells["FinishWeight"].Value);
			}
			txtGrossWeight.Text = FinishWeight.ToString("#,##.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGetDBCall(int RecordId)
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecordId, 1661);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(CmbItem.Value);
			ItemModalBind(0, ItemId);
			if (((UltraGridBase)CmbItem).ActiveRow != null && ItemId > 0)
			{
				CmbCategory.Value = Conversion.ToInt(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCategoryId"].Value);
				BindPackUomAndRateUom();
				bindvarientunit(Conversion.ToInt(CmbItem.Value));
				FillItemFinishWeight(Conversion.ToInt(CmbItem.Value));
				AvailableStockGetByItem();
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsuppliername_Leave(object sender, EventArgs e)
	{
		try
		{
			BindCustomerCity();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemDetailBind(Conversion.ToInt(CmbCategory.Value));
			ItemModalBind(Conversion.ToInt(CmbCategory.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItem.Value);
				}
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				if (ItemId > 0)
				{
					CmbItem.Value = ItemId;
					CmbItem.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSampleModal_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbSampleModal.Value);
			if (((UltraGridBase)CmbSampleModal).ActiveRow != null && Id > 0)
			{
				CmbCategory.Value = Conversion.ToInt(((UltraDropDownBase)CmbSampleModal).SelectedRow.Cells["ItemCategoryId"].Value);
			}
			ItemDetailBind(Conversion.ToInt(CmbCategory.Value), Conversion.ToInt(CmbSampleModal.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbCurrency).Visible = true;
				txtExchangeRate.Visible = true;
				txtFcyAmount.Visible = true;
				label43.Visible = true;
				label44.Visible = true;
				label45.Visible = true;
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				label43.Visible = false;
				label44.Visible = false;
				label45.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				if (Conversion.ToInt(CmbCity.Value) == 0)
				{
					CmbCity.Value = CityId;
				}
			}
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				if (Conversion.ToInt(CmbJobLot.Value) == 0)
				{
					CmbJobLot.Value = JobId;
				}
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int Currency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				BaseCurrency = Currency;
				if (Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					cmbCurrency.Value = BaseCurrency;
				}
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				double BaseCurrencyRate = (BaseRate = Conversion.ToDouble((object)BaseRatelist[0].ConfigKey.ToString()));
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
				{
					txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0 || Conversion.ToDouble((object)txtExchangeRate.Text) != 0.0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "1661",
					Ids = Conversion.ToString(cmbCurrency.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtExchangeRate.Text = Conversion.ToDouble(dt.Rows[0]["LastExchRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else
				{
					txtExchangeRate.Text = "0";
				}
			}
			else
			{
				txtExchangeRate.Text = BaseRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double amount = Conversion.ToDouble(obj.Cells["ItemAmount"].Value) / Conversion.ToDouble((object)txtExchangeRate.Text);
					obj.Cells["FcyAmount"].Value = amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate.Text = Conversion.ToDecimal((object)txtExchangeRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool FormValidation()
	{
		if (Conversion.ToInt((object)txtdocno.Text.Trim()) == 0)
		{
			MessageBox.Show("Doc No Name is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbsuppliername).ActiveRow == null || Conversion.ToInt(cmbsuppliername.Value) == 0)
		{
			MessageBox.Show("Customer Name is Required");
			cmbsuppliername.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentTerm).ActiveRow == null || Conversion.ToInt(CmbPaymentTerm.Value) == 0)
		{
			MessageBox.Show("Payment Term is Required");
			CmbPaymentTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && Conversion.ToInt((object)txtDueDays.Text) == 0)
		{
			MessageBox.Show("Due Days Field is Required");
			txtDueDays.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTerm).ActiveRow == null || string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text)))
		{
			MessageBox.Show("Delivery Term is Required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRate.Focus();
				return false;
			}
			if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtFcyAmount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtExchangeRate.Focus();
				return false;
			}
		}
		if (((UltraGridBase)CmbDeliveryTerm).ActiveRow == null || Conversion.ToInt(CmbDeliveryTerm.Value) == 0)
		{
			MessageBox.Show("Delivery Term is Required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtFreightAmount.Text) > 0.0) && (Conversion.ToInt(CmbFreightAcCr.Value) > 0 || Conversion.ToInt(cmbFreightAcDr.Value) > 0))
		{
			MessageBox.Show("Freight Amount Field is Required");
			txtFreightAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFreightAmount.Text) > 0.0 && Conversion.ToInt(CmbFreightAcCr.Value) <= 0)
		{
			MessageBox.Show("Freight/Ac Cr Field is Required");
			CmbFreightAcCr.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFreightAmount.Text) > 0.0 && Conversion.ToInt(cmbFreightAcDr.Value) <= 0)
		{
			MessageBox.Show("Freight/Ac Dr Field is Required");
			cmbFreightAcDr.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFreightAmount.Text) > 0.0 && txtFreightRemarks.Text.Trim() == "")
		{
			MessageBox.Show("Freight Remarks Field is Required");
			txtFreightRemarks.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetila()
	{
		if (((UltraGridBase)CmbWarehouse).ActiveRow == null || Conversion.ToInt(CmbWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			CmbWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItem).ActiveRow == null || Conversion.ToInt(CmbItem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Uom Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemQty.Text) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtItemQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Item Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required");
			CmbRateUom.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbDiscountType.Value) > 0 && Conversion.ToDouble((object)txtDiscPercent.Text) <= 0.0)
		{
			MessageBox.Show("Discount Rate Field is Required when Discount Type is Selected");
			txtDiscPercent.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbDiscountType.Value) != 0 && (txtDiscountAmount.Text.Trim() == "" || txtDiscountAmount.Text.Trim() == "0"))
		{
			MessageBox.Show("Discount Amount Field is Required when Discount Type is Selected");
			txtDiscountAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCity).ActiveRow == null || Conversion.ToInt(CmbCity.Value) == 0)
		{
			MessageBox.Show("CityName Field is Required");
			CmbCity.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			DetailPanel.Show();
			SupplierId = 0;
			VoucherHeadId = 0;
			RecId = 0;
			SaleOrderId = 0;
			((Control)(object)cmbsuppliername).Enabled = true;
			DocDate.Focus();
			((Control)(object)cmbcommagent).Text = string.Empty;
			cmbcommtype.Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			cmbcommuom.Text = string.Empty;
			txtcommremarks.Text = string.Empty;
			txtbillno.Text = string.Empty;
			((Control)(object)CmbFreightAcCr).Text = string.Empty;
			((Control)(object)cmbFreightAcDr).Text = string.Empty;
			txtFreightAmount.Text = string.Empty;
			txtFreightRemarks.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)cmbsuppliername).Text = string.Empty;
			dtGrdGL.Rows.Clear();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			if (grdGLedger.GetDataRows().Count() > 0)
			{
				gridGLSettings();
			}
			dtInvExp.Rows.Clear();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			if (grdInvExp.GetDataRows().Count() > 0)
			{
				grdInvExpSettings();
			}
			dtPaymentTerm.Rows.Clear();
			grdPaymentTerm.DataSource = dtPaymentTerm;
			grdPaymentTerm.RetrieveStructure();
			if (grdPaymentTerm.GetDataRows().Count() > 0)
			{
				grdSettingsPaymentTerm();
			}
			AddPaymentRowsRowsInPaymentGrid();
			dtGrid.Rows.Clear();
			grd.DataSource = dtGrid;
			if (grd.GetDataRows().Count() > 0)
			{
				grdSettings();
			}
			UpdateDocumentNoUI(DocumentNoDBCall());
			UpdateBranchSrNoUI(BranchSrNoDBCall());
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtCustomerCity.Text = string.Empty;
			txtCustomerCellNo.Text = string.Empty;
			cmbsuppliername.Focus();
			txtLedgerBalance.Text = "0";
			txtBillAmount.Text = "0";
			txtLedgerBalanceAfterBill.Text = "0";
			ConfigurationDefault();
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
		try
		{
			Reset();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFrmRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string>
			{
				"AccountsWithCustomGroupId", "ItemsWithCustomGroupId", "LookUpsEngr", "PaymentTerm", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "MultiCurrency",
				"AllCities", "CropYear", "JobLot", "InvPackingType"
			};
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			SupplierDtFillFromGlobal();
			BindSupplierName();
			FreightAccountDebitAndCreditDtFill();
			BindFreightAccountDebitAndCredit();
			BindTaxAccount();
			PaymentTermDtFillFromGlobal();
			BindPaymentTerm();
			BindDiscountAccount();
			BindCurrency();
			WarehousesDtFillFromGlobal();
			BindWareHouse();
			ItemCategoryFill(ItemCategoryDBCall());
			SampleModaldBCall();
			ItemModalBind();
			ItemDtFillFromGlobal();
			ItemDetailBind();
			CastingTypeDtFillFromGlobal();
			BindCastingType();
			BindProductionStage();
			AllTaxTypeDBCall();
			BindTaxType();
			CityDtFillFromGlobal();
			BindCityName();
			GridDetailCombo();
			AccountlstDtFillFromGlobal();
			grdGLCombBind();
			OtherItemsDBcall();
			grdInvExpCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void closeHistory_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void ResetDetail()
	{
		try
		{
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtItemQty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtAddLss.Text = string.Empty;
			txtNetBillWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtAddLessRate.Text = string.Empty;
			txtNetRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)CmbTaxType).Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtDiscPercent.Clear();
			txtDiscountAmount.Clear();
			txtgatepassno.Clear();
			txtvehicleno.Clear();
			txtTotalAmount.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleInvoiceDirectEngr_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "ctrlGrdBar2")
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Debit" || e.Column.Key == "Credit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdGLedger.CurrentRow;
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGLGrid();
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BillAmount();
	}

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Credit"].Value = Math.Round(Qty * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage")
			{
				double TotalPercentage = 0.0;
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow r in rows)
				{
					TotalPercentage += Conversion.ToDouble(r.Cells["Percentage"].Value);
				}
				if (TotalPercentage > 100.0)
				{
					double Percent = Conversion.ToDouble(grdGLedger.CurrentRow.Cells["Percentage"].Value);
					double i = TotalPercentage - 100.0;
					grdGLedger.CurrentRow.Cells["Percentage"].Value = Percent - i;
					MessageBox.Show("TotalPercentage Can not be Greater than 100...");
					return;
				}
				if (item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
					double TotlaPercentage = num / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Credit"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						item.Cells["Debit"].Value = 0;
					}
					else
					{
						item.Cells["Debit"].Value = Math.Abs(Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero));
						item.Cells["Credit"].Value = 0;
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			if (e.Column.Key == "Credit" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Credit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					item.Cells["Credit"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Debit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Credit"].Value.ToString()) > 0.0)
				{
					item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
			}
			if (e.Column.Key == "AccountId")
			{
				if (!SubsidiaryAccountAllownOnVouchers && Conversion.ToInt((object)txtSupplierGLId.Text) > 0 && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
				{
					MessageBox.Show("Customer Account Not select");
					item.Cells["AccountId"].Value = 0;
					return;
				}
				SupCustIdUpdateforGLGrid(item);
			}
			grdGLedger.UpdateData();
			BillAmount();
			LedgerProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGLedger.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdGLedger.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdGLedger.CurrentColumn.Key == "Delete")
				{
					dtGrdGL.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						item.Delete();
					}
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
				else if (grdGLedger.CurrentColumn.Key == "Add")
				{
					AddRowInGLGrid();
				}
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInGLGrid();
				BillAmount();
			}
			else if (e.KeyCode == Keys.F1 && grdGLedger.CurrentColumn != null && grdGLedger.CurrentColumn.Key == "AccountId")
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					item.Cells["AccountId"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlst, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					item.Cells["AccountId"].Value = obj2.ReturnId;
				}
				SupCustIdUpdateforGLGrid(item);
				grdGLedger.UpdateData();
			}
			((Control)(object)grdGLedger).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGLGrid()
	{
		try
		{
			dtGrdGL.Rows.Add(0, 0, "", 0, 0, 0, 0, 0);
			grdGLedger.DataSource = dtGrdGL;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLCombBind()
	{
		try
		{
			grdGLedger.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdGLedger.RootTable.Columns["GlAccountId"].Visible = false;
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SupplierCustomerId", "AccountTitle");
			}
			else
			{
				grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridGLSettings()
	{
		try
		{
			grdGLedger.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdGLedger.RootTable.Columns["GlAccountId"].Visible = false;
			grdGLCombBind();
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "X";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "X";
			grdGLedger.RootTable.Columns["Delete"].Width = 20;
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "+";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "+";
			grdGLedger.RootTable.Columns["Add"].Width = 20;
			grdGLedger.RootTable.Columns["Add"].Position = 0;
			grdGLedger.RootTable.Columns["Delete"].Position = 1;
			grdGLedger.FrozenColumns = 2;
			grdGLedger.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupCustIdUpdateforGLGrid(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			string selectedAcId = Conversion.ToString((object)r.Cells["AccountId"].Text);
			if (selectedAcId != "0" && selectedAcId != string.Empty)
			{
				DataRow[] matchingRows = dtAccountlst.Select("AccountTitle='" + selectedAcId + "'");
				if (matchingRows.Length != 0)
				{
					r.BeginEdit();
					r.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
					r.EndEdit();
				}
			}
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				if (Conversion.ToInt((object)txtSupplierGLId.Text) > 0 && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(r.Cells["GlAccountId"].Value))
				{
					MessageBox.Show("Selected Account Can not be Same As Supplier Account");
					r.BeginEdit();
					r.Cells["GlAccountId"].Value = 0;
					r.Cells["AccountId"].Value = 0;
					r.EndEdit();
				}
			}
			else if (Conversion.ToInt(cmbsuppliername.Value) > 0 && Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(r.Cells["AccountId"].Value))
			{
				MessageBox.Show("Selected Account Can not be Same As Supplier Account");
				r.BeginEdit();
				r.Cells["AccountId"].Value = 0;
				r.Cells["GlAccountId"].Value = 0;
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdInvExp.CurrentRow;
				dtInvExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInvExpGrid();
			}
			grdInvExp.UpdateData();
			ExpProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Amount"].Value = Amount;
			}
			grdInvExp.UpdateData();
			ExpProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		try
		{
			dtInvExp.Rows.Add(0, 0, 0, 0, "");
			grdInvExp.DataSource = dtInvExp;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpCombo()
	{
		try
		{
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExpCombo();
			grdInvExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExp.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = "#,##0.##";
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "X";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "X";
			grdInvExp.RootTable.Columns["Delete"].Width = 20;
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "+";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "+";
			grdInvExp.RootTable.Columns["Add"].Width = 20;
			grdInvExp.RootTable.Columns["Add"].Position = 0;
			grdInvExp.RootTable.Columns["Delete"].Position = 1;
			grdInvExp.FrozenColumns = 2;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Amount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdInvExp.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExp.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtInvExp.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
				}
				if (grdInvExp.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtInvExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
			}
			((Control)(object)grdInvExp).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddPaymentRowsRowsInPaymentGrid()
	{
		try
		{
			if (grdPaymentTerm.RowCount > 0)
			{
				foreach (DataRow r in dtTerms.Rows)
				{
					if (!dtPaymentTerm.AsEnumerable().Any((DataRow row) => row.Field<int>("PaymentTermId") == Conversion.ToInt(r["Id"])))
					{
						dtPaymentTerm.Rows.Add(r["Id"], 0, 0, 0, DateTime.Now, "");
					}
				}
				grdPaymentTerm.DataSource = dtPaymentTerm;
				return;
			}
			dtPaymentTerm.Clear();
			foreach (DataRow r2 in dtTerms.Rows)
			{
				dtPaymentTerm.Rows.Add(r2["Id"], 0, 0, 0, DateTime.Now, "");
			}
			grdPaymentTerm.DataSource = dtPaymentTerm;
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
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
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].ColumnType = (ColumnType)0;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].HasValueList = true;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].LimitToList = true;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].EditType = (EditType)4;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Caption = "Payment Term";
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].ValueList.PopulateValueList((IEnumerable)dtTerms.DefaultView, "Id", "TermsDescription");
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Width = 120;
			grdPaymentTerm.RootTable.Columns["DueDate"].EditType = (EditType)2;
			grdPaymentTerm.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yyyy";
			CommonServices.GridWrappingAndColumnSettings(grdPaymentTerm, 2, 3);
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Width = Constants.InventoryConstants.TermConstant;
			grdPaymentTerm.RootTable.Columns["%ofTotal"].Width = Constants.InventoryConstants.PercentOfTotal;
			grdPaymentTerm.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentTerm.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdPaymentTerm.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdPaymentTerm.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdPaymentTerm.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "%ofTotal" && grd.GetRows().Count() > 0)
				{
					double num = Conversion.ToDouble((object)txtBillAmount.Text);
					double prcnt2 = Conversion.ToDouble(item.Cells["%ofTotal"].Value);
					if (prcnt2 > 100.0)
					{
						MessageBox.Show("%of Total Can't Greater than 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["%ofTotal"].Value = 100;
						prcnt2 = 100.0;
					}
					double amount2 = num * prcnt2 / 100.0;
					item.Cells["Amount"].Value = Math.Round(amount2, 4);
				}
				if (e.Column.Key == "Amount")
				{
					if (grd.GetRows().Count() <= 0)
					{
						throw new Exception("No Detail Record Found");
					}
					double billAmount2 = Conversion.ToDouble((object)txtBillAmount.Text);
					double amount3 = Conversion.ToDouble(item.Cells["Amount"].Value);
					if (billAmount2 < amount3)
					{
						MessageBox.Show("Amount Cant be Greater than BillAmount", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["Amount"].Value = billAmount2;
						amount3 = billAmount2;
					}
					double prcnt3 = Math.Round(amount3 * 100.0 / billAmount2, 8);
					item.Cells["%ofTotal"].Value = prcnt3;
				}
				if (e.Column.Key == "DueDays")
				{
					int days2 = Conversion.ToInt(item.Cells["DueDays"].Value);
					item.Cells["DueDate"].Value = DocDate.Value.AddDays(days2);
				}
				if (e.Column.Key == "DueDate")
				{
					DateTime date = Conversion.ToDateTime(item.Cells["DueDate"].Value);
					if (date.Date < DocDate.Value.Date)
					{
						item.Cells["DueDate"].Value = DocDate.Value;
						throw new Exception("Due Date Can't less Than DocDate");
					}
					int days = (date.Date - DocDate.Value.Date).Days;
					item.Cells["DueDays"].Value = days;
				}
				if (e.Column.Key == "PaymentTermId")
				{
					if (Conversion.ToInt(item.Cells["PaymentTermId"].Value) == 3 && SaleOrderId > 0)
					{
						DataTable dt = SaleOrder.GetSaleOrderRemningAmountByOrderId(UserAccount.OrganizationId, UserAccount.CompanyId, SaleOrderId, RecId);
						if (dt != null)
						{
							item.Cells["Amount"].Value = Conversion.ToDouble(dt.Rows[0]["BalanceAmount"]);
							double billAmount = Conversion.ToDouble((object)txtBillAmount.Text);
							double amount = Conversion.ToDouble(item.Cells["Amount"].Value);
							if (billAmount < amount)
							{
								MessageBox.Show("Amount Cant be Greater than BillAmount", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								item.Cells["Amount"].Value = billAmount;
								amount = billAmount;
							}
							double prcnt = Math.Round(amount * 100.0 / billAmount, 8);
							item.Cells["%ofTotal"].Value = prcnt;
						}
					}
					else
					{
						item.Cells["Amount"].Value = 0.ToString();
					}
				}
			}
			grdPaymentTerm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "%ofTotal" || e.Column.Key == "Amount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdPaymentTerm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdPaymentTerm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				_ = grdPaymentTerm.CurrentColumn;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermAmountCalculateFromPercent()
	{
		try
		{
			double orderAmount = Conversion.ToDouble((object)txtBillAmount.Text);
			GridEXRow[] rows = grdPaymentTerm.GetRows();
			if (orderAmount > 0.0 && rows.Length != 0)
			{
				GridEXRow[] array = rows;
				foreach (GridEXRow obj in array)
				{
					obj.BeginEdit();
					double percent = Conversion.ToDouble(obj.Cells["%ofTotal"].Value);
					obj.Cells["Amount"].Value = ((percent > 0.0) ? Math.Round(orderAmount * percent / 100.0, 2, MidpointRounding.AwayFromZero) : 0.0);
					obj.EndEdit();
				}
				grdPaymentTerm.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetila())
			{
				dtGrid.Rows.Add(0, 0, 0, 0, DateTime.Now, CmbWarehouse.Value, CmbItem.Value, Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value), CmbPackUom.Value, ((Control)(object)CmbPackUom).Text, Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value), Conversion.ToInt(CmbVariantDescription.Value), Conversion.ToString((object)((Control)(object)CmbVariantDescription).Text), Conversion.ToInt(CmbCastingTypeDetail.Value), Conversion.ToInt(CmbProductionStage.Value), Conversion.ToString((object)txtremarksdetail.Text), Conversion.ToDouble((object)txtItemQty.Text), Conversion.ToDouble((object)txtNetBillWeight.Text), Conversion.ToDouble((object)txtRate.Text), CmbRateUom.Value, ((Control)(object)CmbRateUom).Text, Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtAddLessRate.Text), Conversion.ToDouble((object)txtNetRate.Text), Conversion.ToInt(CmbDiscountType.Value), Conversion.ToDouble((object)txtDiscPercent.Text), Conversion.ToDouble((object)txtDiscountAmount.Text), Conversion.ToDouble((object)txtAmount.Text), 0, CmbTaxType.Value, Conversion.ToDouble((object)txtTaxPercent.Text), Conversion.ToDouble((object)txtTaxAmount.Text), 0, 0, 0, txtgpdate.Value.ToShortDateString(), txtgatepassno.Text, txtvehicleno.Text, CmbCity.Value);
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				ExpProportion();
				FreightProportion();
				LedgerProportion();
				BillAmount();
				((Control)(object)CmbItem).Text = string.Empty;
				txtRate.Text = string.Empty;
				txtItemQty.Text = string.Empty;
				CmbItem.Focus();
				txtExchangeRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && Conversion.ToInt(r.Cells["OrderId"].Value) == 0)
			{
				updateDetailIndex = r.RowIndex;
				((Control)(object)CmbCategory).Text = "";
				CmbCategory_Leave(null, null);
				CmbWarehouse.Value = r.Cells["WareHouseId"].Value.ToString();
				CmbItem.Value = r.Cells["ItemId"].Value.ToString();
				CmbItem_Leave(null, null);
				CmbPackUom.Value = r.Cells["PackUomId"].Value.ToString();
				CmbVariantDescription.Value = Conversion.ToInt(r.Cells["VariantId"].Value);
				CmbCastingTypeDetail.Value = Conversion.ToInt(r.Cells["CastingTypeId"].Value);
				CmbProductionStage.Value = Conversion.ToInt(r.Cells["ProductionStageId"].Value);
				txtremarksdetail.Text = r.Cells["Remarks"].Value.ToString();
				txtItemQty.Text = r.Cells["ItemQty"].Value.ToString();
				txtRate.Text = Conversion.ToDouble(r.Cells["ItemRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				CmbRateUom.Value = r.Cells["RateUomId"].Value.ToString();
				txtAddLessRate.Text = Conversion.ToDouble(r.Cells["AddLessRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtNetRate.Text = Conversion.ToDouble(r.Cells["NetRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				CmbDiscountType.Value = r.Cells["DiscountType"].Value;
				txtDiscPercent.Text = Conversion.ToDouble(r.Cells["Discount%"].Value).ToString("#,##0.###");
				txtDiscountAmount.Text = Conversion.ToDouble(r.Cells["DiscountAmount"].Value).ToString("#,##0.###");
				txtAmount.Text = Conversion.ToDouble(r.Cells["ItemAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtgpdate.Text = r.Cells["GpDate"].Value.ToString();
				txtgatepassno.Text = r.Cells["GpNo"].Value.ToString();
				txtvehicleno.Text = r.Cells["VehicleNo"].Value.ToString();
				CmbTaxType.Value = r.Cells["TaxTypeId"].Value.ToString();
				txtTaxPercent.Text = r.Cells["Tax%"].Value.ToString();
				txtTaxAmount.Text = r.Cells["TaxAmount"].Value.ToString();
				((Control)(object)CmbCity).Text = r.Cells["CityName"].Text.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbWarehouse.Focus();
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
			if (FormValidationDetila())
			{
				dtGrid.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(CmbWarehouse.Value);
				dtGrid.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItem.Value);
				dtGrid.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value);
				dtGrid.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Value);
				dtGrid.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(CmbPackUom.Value);
				dtGrid.Rows[updateDetailIndex]["PackUom"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
				dtGrid.Rows[updateDetailIndex]["PackEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				dtGrid.Rows[updateDetailIndex]["VariantId"] = Conversion.ToInt(CmbVariantDescription.Value);
				dtGrid.Rows[updateDetailIndex]["VariantDescription"] = Conversion.ToString((object)((Control)(object)CmbVariantDescription).Text);
				dtGrid.Rows[updateDetailIndex]["CastingTypeId"] = Conversion.ToInt(CmbCastingTypeDetail.Value);
				dtGrid.Rows[updateDetailIndex]["ProductionStageId"] = Conversion.ToInt(CmbProductionStage.Value);
				dtGrid.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtremarksdetail.Text);
				dtGrid.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtItemQty.Text);
				dtGrid.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtRate.Text);
				dtGrid.Rows[updateDetailIndex]["RateUomId"] = Conversion.ToInt(CmbRateUom.Value);
				dtGrid.Rows[updateDetailIndex]["RateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUom).Text);
				dtGrid.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				dtGrid.Rows[updateDetailIndex]["AddLessRate"] = Conversion.ToDouble((object)txtAddLessRate.Text);
				dtGrid.Rows[updateDetailIndex]["NetRate"] = Conversion.ToDouble((object)txtNetRate.Text);
				dtGrid.Rows[updateDetailIndex]["DiscountType"] = Conversion.ToInt(CmbDiscountType.Value);
				dtGrid.Rows[updateDetailIndex]["Discount%"] = Conversion.ToDouble((object)txtDiscPercent.Text);
				dtGrid.Rows[updateDetailIndex]["DiscountAmount"] = Conversion.ToDouble((object)txtDiscountAmount.Text);
				dtGrid.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtAmount.Text);
				dtGrid.Rows[updateDetailIndex]["TaxTypeId"] = Conversion.ToInt(CmbTaxType.Value);
				dtGrid.Rows[updateDetailIndex]["Tax%"] = Conversion.ToDouble((object)txtTaxPercent.Text);
				dtGrid.Rows[updateDetailIndex]["TaxAmount"] = Conversion.ToDouble((object)txtTaxAmount.Text);
				dtGrid.Rows[updateDetailIndex]["GpDate"] = Conversion.ToDateTime((object)txtgpdate.Value);
				dtGrid.Rows[updateDetailIndex]["GpNo"] = Conversion.ToInt((object)txtgatepassno.Text);
				dtGrid.Rows[updateDetailIndex]["VehicleNo"] = Conversion.ToString((object)txtvehicleno.Text);
				dtGrid.Rows[updateDetailIndex]["CityName"] = Conversion.ToInt(CmbCity.Value);
				btnAdd.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				FreightProportion();
				BillAmount();
				ResetDetail();
				CmbItem.Focus();
				txtExchangeRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetail();
			CmbItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "ItemQty" || e.Column.Key == "AddLessRate" || e.Column.Key == "DiscountType" || e.Column.Key == "Discount%")
			{
				item.BeginEdit();
				double num = Conversion.ToDouble(item.Cells["ItemQty"].Value);
				Conversion.ToDouble(item.Cells["PackEquivalent"].Value);
				double ItemRate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
				double AddLessRate = Conversion.ToDouble(item.Cells["AddLessRate"].Value);
				double NetRate = ((AddLessRate != 0.0) ? (ItemRate + AddLessRate) : ItemRate);
				item.Cells["NetRate"].Value = NetRate;
				double EquivalentRate = Conversion.ToDouble(item.Cells["RateEquivalent"].Value);
				double Amount = num / EquivalentRate * NetRate;
				int DiscountType = Conversion.ToInt(item.Cells["DiscountType"].Value);
				double DiscPrcnt = Conversion.ToDouble(item.Cells["Discount%"].Value);
				double DiscountAmount = 0.0;
				switch (DiscountType)
				{
				case 1:
					DiscountAmount = ((DiscPrcnt > 0.0) ? DiscPrcnt : 0.0);
					break;
				case 2:
					if (DiscPrcnt > 100.0)
					{
						DiscPrcnt = 100.0;
						item.Cells["Discount%"].Value = 100;
					}
					DiscountAmount = ((DiscPrcnt > 0.0) ? (Amount * DiscPrcnt / 100.0) : 0.0);
					break;
				default:
					DiscountAmount = 0.0;
					item.Cells["Discount%"].Value = 0;
					break;
				}
				item.Cells["DiscountAmount"].Value = DiscountAmount;
				item.Cells["ItemAmount"].Value = Amount - DiscountAmount;
				item.EndEdit();
			}
			else if (e.Column.Key == "TaxTypeId")
			{
				int selectedTaxId = Conversion.ToInt(item.Cells["TaxTypeId"].Value);
				if (selectedTaxId == 0)
				{
					item.Cells["Tax%"].Value = 0;
				}
				else
				{
					DataRow[] matchingRows = dtTaxType.Select("TaxNameId = " + selectedTaxId);
					if (matchingRows.Length != 0)
					{
						item.BeginEdit();
						item.Cells["Tax%"].Value = Conversion.ToDouble(matchingRows[0]["TaxPercent"]);
						item.EndEdit();
					}
				}
			}
			else if (e.Column.Key == "Tax%" && Conversion.ToDouble(item.Cells["Tax%"].Value) == 0.0)
			{
				int selectedTaxId2 = Conversion.ToInt(item.Cells["TaxTypeId"].Value);
				DataRow[] matchingRows2 = dtTaxType.Select("TaxNameId = " + selectedTaxId2);
				if (matchingRows2.Length != 0)
				{
					item.BeginEdit();
					item.Cells["Tax%"].Value = Conversion.ToDouble(matchingRows2[0]["TaxPercent"]);
					item.EndEdit();
				}
			}
			int TaxType = Conversion.ToInt(item.Cells["TaxTypeId"].Value);
			double TaxPercent = Conversion.ToDouble(item.Cells["Tax%"].Value);
			double ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
			double TaxAmount = 0.0;
			if (TaxPercent > 100.0)
			{
				MessageBox.Show("Tax % Can't be greater than 100...");
				TaxPercent = 100.0;
				item.Cells["Tax%"].Value = TaxPercent;
			}
			if (ItemAmount > 0.0 && TaxPercent > 0.0 && TaxType > 0)
			{
				TaxAmount = ItemAmount * TaxPercent / 100.0;
				item.BeginEdit();
				item.Cells["TaxAmount"].Value = Math.Round(TaxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.EndEdit();
			}
			else
			{
				item.BeginEdit();
				item.Cells["Tax%"].Value = 0;
				item.Cells["TaxAmount"].Value = 0;
				item.EndEdit();
			}
			grd.UpdateData();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailCombo()
	{
		try
		{
			grd.RootTable.Columns["CityName"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CityName"].EditType = (EditType)5;
			grd.RootTable.Columns["CityName"].HasValueList = true;
			grd.RootTable.Columns["CityName"].LimitToList = true;
			grd.RootTable.Columns["CityName"].ValueList.PopulateValueList((IEnumerable)dtCity.DefaultView, "Id", "CityName");
			grd.RootTable.Columns["CastingTypeId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CastingTypeId"].EditType = (EditType)5;
			grd.RootTable.Columns["CastingTypeId"].HasValueList = true;
			grd.RootTable.Columns["CastingTypeId"].LimitToList = true;
			grd.RootTable.Columns["CastingTypeId"].Caption = "Casting Type";
			grd.RootTable.Columns["CastingTypeId"].ValueList.PopulateValueList((IEnumerable)dtCastingType.DefaultView, "Id", "CastingType");
			grd.RootTable.Columns["ProductionStageId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["ProductionStageId"].EditType = (EditType)5;
			grd.RootTable.Columns["ProductionStageId"].HasValueList = true;
			grd.RootTable.Columns["ProductionStageId"].LimitToList = true;
			grd.RootTable.Columns["ProductionStageId"].Caption = "Production Stage";
			grd.RootTable.Columns["ProductionStageId"].ValueList.PopulateValueList((IEnumerable)dtProductionStage.DefaultView, "Id", "ProductionStage");
			grd.RootTable.Columns["WarehouseId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WarehouseId"].HasValueList = true;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)5;
			grd.RootTable.Columns["WarehouseId"].LimitToList = true;
			grd.RootTable.Columns["WarehouseId"].Caption = "Ware House";
			grd.RootTable.Columns["WarehouseId"].ValueList.PopulateValueList((IEnumerable)dtWarehouse.DefaultView, "Id", "WareHouseName");
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			grd.RootTable.Columns["DiscountType"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["DiscountType"].HasValueList = true;
			grd.RootTable.Columns["DiscountType"].LimitToList = true;
			grd.RootTable.Columns["DiscountType"].EditType = (EditType)4;
			grd.RootTable.Columns["DiscountType"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "Name");
			grd.RootTable.Columns["DiscountType"].Width = 60;
			grd.RootTable.Columns["TaxTypeId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["TaxTypeId"].HasValueList = true;
			grd.RootTable.Columns["TaxTypeId"].LimitToList = true;
			grd.RootTable.Columns["TaxTypeId"].EditType = (EditType)(TaxPercntEditableOrNot ? 4 : 0);
			grd.RootTable.Columns["TaxTypeId"].Caption = "Tax Type";
			grd.RootTable.Columns["TaxTypeId"].ValueList.PopulateValueList((IEnumerable)dtTaxType.DefaultView, "TaxNameId", "TaxName");
			grd.RootTable.Columns["TaxTypeId"].Width = 60;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["VariantId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Visible = false;
			grd.RootTable.Columns["Freights"].Visible = false;
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grd.RootTable.Columns["PackUom"].Caption = "Uom";
			grd.RootTable.Columns["VariantDescription"].Caption = "Modal Description";
			grd.RootTable.Columns["ItemQty"].Caption = "Qty";
			grd.RootTable.Columns["NetWeight"].Caption = "Item FG Weight";
			grd.RootTable.Columns["Remarks"].Caption = "Specification/Remarks";
			GridEXRow row = grd.GetRow(0);
			int OrderId = ((row != null) ? Conversion.ToInt(row.Cells["OrderId"].Value) : 0);
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				CommonServices.GridColumnSettings(item, 3);
			}
			grd.RootTable.Columns["Tax%"].EditType = (EditType)(TaxPercntEditableOrNot ? 1 : 0);
			GridDetailCombo();
			grd.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grd.RootTable.Columns["OrderDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["WarehouseId"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["PackEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.VariantEquivalent;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["RateEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["AddLessRate"].Width = Constants.InventoryConstants.Address;
			grd.RootTable.Columns["NetRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["DiscountType"].Width = Constants.InventoryConstants.DiscountType;
			grd.RootTable.Columns["Discount%"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["DiscountAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["TaxTypeId"].Width = Constants.InventoryConstants.DiscountType;
			grd.RootTable.Columns["Tax%"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Freights"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grd.RootTable.Columns["ProductionStageId"].Width = Constants.InventoryConstants.ProductionStage;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grd.RootTable.Columns["AddLessRate"].EditType = (EditType)1;
			grd.RootTable.Columns["GpDate"].EditType = (EditType)2;
			grd.RootTable.Columns["GpNo"].EditType = (EditType)1;
			grd.RootTable.Columns["VehicleNo"].EditType = (EditType)1;
			grd.RootTable.Columns["Discount%"].EditType = (EditType)1;
			grd.RootTable.Columns["Remarks"].EditType = (EditType)1;
			if (OrderId > 0)
			{
				DetailPanel.Hide();
				grd.RootTable.Columns["OrderNo"].Visible = true;
				grd.RootTable.Columns["OrderDate"].Visible = true;
				grd.RootTable.Columns.Add("Add");
				grd.RootTable.Columns["Add"].Key = "Add";
				grd.RootTable.Columns["Add"].Caption = "+";
				grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Add"].ButtonText = "+";
				grd.RootTable.Columns["Add"].Width = 20;
			}
			else
			{
				grd.RootTable.Columns["OrderNo"].Visible = false;
				grd.RootTable.Columns["OrderDate"].Visible = false;
			}
			if (btnSave.Visible && btnSave.Enabled)
			{
				grd.RootTable.Columns.Add("Delete");
				grd.RootTable.Columns["Delete"].Key = "Delete";
				grd.RootTable.Columns["Delete"].Caption = "X";
				grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Delete"].ButtonText = "X";
				grd.RootTable.Columns["Delete"].Width = 20;
			}
			if (OrderId > 0 && btnSave.Visible && btnSave.Enabled)
			{
				grd.RootTable.Columns["Add"].Position = 0;
				grd.RootTable.Columns["Delete"].Position = 1;
				grd.FrozenColumns = 2;
			}
			else if (btnSave.Visible && btnSave.Enabled)
			{
				grd.RootTable.Columns["Delete"].Position = 0;
				grd.FrozenColumns = 1;
			}
			else if (OrderId > 0)
			{
				grd.RootTable.Columns["Add"].Position = 0;
				grd.FrozenColumns = 1;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		if (e.Column.Key == "GpNo")
		{
			if (!int.TryParse(e.Value.ToString(), out var _))
			{
				e.Cancel = true;
				MessageBox.Show("Please enter a valid number.");
			}
		}
		else if ((e.Column.Key == "Discount%" || e.Column.Key == "AddLessRate" || e.Column.Key == "ItemQty") && !CommonServices.IsNumeric(e))
		{
			throw new Exception("Please Type Only Numeric Value");
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Add")
			{
				GridEXRow r = grd.CurrentRow;
				DataRow dr = dtGrid.Rows[r.RowIndex];
				dtGrid.Rows.Add(dr.ItemArray);
				grd.DataSource = dtGrid;
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					dtGrid.Rows[dtGrid.Rows.Count - 1]["Id"] = 0;
				}
			}
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			if (grd.GetRows().Count() == 0)
			{
				DetailPanel.Show();
			}
			FreightProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool Insert(bool IsAutoUpdate = false, int RecUpdateId = 0)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Expected O, but got Unknown
		//IL_0fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb5: Expected O, but got Unknown
		//IL_1417: Unknown result type (might be due to invalid IL or missing references)
		//IL_141e: Expected O, but got Unknown
		//IL_126b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1272: Expected O, but got Unknown
		//IL_15e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ec: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return false;
			}
			InvSaleInvoice saleInvoice = new InvSaleInvoice();
			if (IsAutoUpdate)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return false;
				}
				saleInvoice.Id = RecUpdateId;
				saleInvoice.AutoUpdate = 1;
			}
			else if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return false;
				}
				saleInvoice.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return false;
			}
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r in rows)
			{
				if ((Conversion.ToDouble(r.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r.Cells["Debit"].Value) > 0) && Conversion.ToInt(r.Cells["AccountId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against JL First");
				}
			}
			rows = grd.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				if (Conversion.ToDouble(rows[i].Cells["TaxAmount"].Value) > 0.0 && (((Control)(object)CmbTaxAccount).Text.Trim() == string.Empty || ((Control)(object)CmbTaxAccount).Text.Trim() == "0"))
				{
					CmbTaxAccount.Focus();
					throw new Exception("Tax Account is Required");
				}
			}
			if (((UltraGridBase)CmbDiscountAccount).ActiveRow != null && Conversion.ToInt(CmbDiscountAccount.Value) > 0 && Conversion.ToDouble((object)txtDiscountAmountHeader.Text) == 0.0)
			{
				CmbDiscountAccount.Focus();
				throw new Exception("Discount Amount Required When Discount Account Is Present...");
			}
			if (Conversion.ToDouble((object)txtDiscountAmountHeader.Text) != 0.0 && (((UltraGridBase)CmbDiscountAccount).ActiveRow == null || Conversion.ToInt(CmbDiscountAccount.Value) == 0))
			{
				CmbDiscountAccount.Focus();
				throw new Exception("Discount Ac Required When Discount Amount Is Present...");
			}
			if (Conversion.ToDouble((object)txtDiscountAmountHeader.Text) >= Conversion.ToDouble((object)txtBillAmount.Text))
			{
				txtDiscountAmountHeader.Focus();
				throw new Exception("Discount Amount Can not be Greater than or Equal to Bill Amount");
			}
			PaymentTermAmountCalculateFromPercent();
			if (Conversion.ToDouble((object)txtBillAmount.Text) == 0.0)
			{
				throw new Exception("Bill Amount Required");
			}
			saleInvoice.TaxAccountId = Conversion.ToInt(CmbTaxAccount.Value);
			saleInvoice.BranchesId = UserAccount.BranchesId;
			saleInvoice.DocumentTypeId = 1661;
			saleInvoice.SupplierInvoiceDate = DateTime.Now;
			saleInvoice.SupplierInvoiceNo = Conversion.ToInt((object)txtdocno.Text);
			saleInvoice.OrganizationId = UserAccount.OrganizationId;
			saleInvoice.CompanyId = UserAccount.CompanyId;
			saleInvoice.EntryDate = DateTime.Now;
			saleInvoice.ModifyDate = DateTime.Now;
			saleInvoice.EntryUser = UserAccount.ID;
			saleInvoice.ScreenName = Conversion.ToString((object)base.Name);
			saleInvoice.ModifyUser = UserAccount.ID;
			saleInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			saleInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			saleInvoice.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
			saleInvoice.DocDate = DocDate.Value;
			saleInvoice.SupplierCustomerId = Conversion.ToInt(cmbsuppliername.Value);
			saleInvoice.SupplierReferenceNo = Conversion.ToString((object)txtRefrenceNo.Text);
			saleInvoice.ManualBillNo = txtbillno.Text.Trim();
			saleInvoice.PaymentTermId = Conversion.ToInt(CmbPaymentTerm.Value);
			saleInvoice.DueDays = Conversion.ToInt((object)txtDueDays.Text);
			saleInvoice.DueDate = Conversion.ToDateTime((object)DueDate.Value);
			saleInvoice.DeliveryTerm = ((Control)(object)CmbDeliveryTerm).Text;
			saleInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			saleInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			saleInvoice.RemarksHeader = txtRemarks.Text.Trim();
			saleInvoice.DiscountAmount = Conversion.ToDecimal((object)txtDiscountAmountHeader.Text);
			saleInvoice.DiscountAccountId = Conversion.ToInt(CmbDiscountAccount.Value);
			decimal TotalQty = Conversion.ToDecimal(dtGrid.Compute("Sum(ItemQty)", string.Empty));
			saleInvoice.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			saleInvoice.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			saleInvoice.InvoiceQty = TotalQty;
			saleInvoice.InvoiceWeight = TotalQty;
			saleInvoice.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				saleInvoice.TransporterId = Conversion.ToInt(CmbFreightAcCr.Value);
				saleInvoice.TransporterCreditPartyId = 0;
				saleInvoice.TransporterDebitGLId = Conversion.ToInt(cmbFreightAcDr.Value);
				saleInvoice.TransporterDebitPartyId = 0;
			}
			else
			{
				if (((UltraGridBase)CmbFreightAcCr).ActiveRow != null && Conversion.ToInt(CmbFreightAcCr.Value) > 0)
				{
					saleInvoice.TransporterId = Conversion.ToInt(((UltraDropDownBase)CmbFreightAcCr).SelectedRow.Cells[0].Value);
					saleInvoice.TransporterCreditPartyId = Conversion.ToInt(((UltraDropDownBase)CmbFreightAcCr).SelectedRow.Cells[1].Value);
				}
				if (((UltraGridBase)cmbFreightAcDr).ActiveRow != null && Conversion.ToInt(cmbFreightAcDr.Value) > 0)
				{
					saleInvoice.TransporterDebitGLId = Conversion.ToInt(((UltraDropDownBase)cmbFreightAcDr).SelectedRow.Cells[0].Value);
					saleInvoice.TransporterDebitPartyId = Conversion.ToInt(((UltraDropDownBase)cmbFreightAcDr).SelectedRow.Cells[1].Value);
				}
			}
			saleInvoice.FreightAmount = Conversion.ToDouble((object)txtFreightAmount.Text.Trim());
			saleInvoice.FreightRemark = txtFreightRemarks.Text.Trim();
			saleInvoice.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			if (grd.RowCount <= 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			rows = grd.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				InvSaleInvoiceDetail Sd = new InvSaleInvoiceDetail();
				Sd.Id = Conversion.ToInt(r3.Cells["Id"].Value);
				Sd.SaleOrderId = Conversion.ToInt(r3.Cells["OrderId"].Value);
				Sd.SaleOrderDetailId = Conversion.ToInt(r3.Cells["OrderDetailId"].Value);
				Sd.SaleOrder = Conversion.ToInt(r3.Cells["OrderNo"].Value);
				Sd.OrderDate = Conversion.ToDateTime(r3.Cells["OrderDate"].Value);
				Sd.WarehouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
				Sd.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				Sd.ItemName = Conversion.ToString(r3.Cells["ItemName"].Value);
				Sd.ItemUOMId = Conversion.ToInt(r3.Cells["PackUomId"].Value);
				Sd.ItemVariantId = Conversion.ToInt(r3.Cells["VariantId"].Value);
				Sd.VariantDescription = Conversion.ToString(r3.Cells["VariantDescription"].Value);
				Sd.CastingTypeId = Conversion.ToInt(r3.Cells["CastingTypeId"].Value);
				Sd.ProductionStageId = Conversion.ToInt(r3.Cells["ProductionStageId"].Value);
				Sd.ItemQty = Conversion.ToDouble(r3.Cells["ItemQty"].Value);
				Sd.WeightFinishGoods = Conversion.ToDouble(r3.Cells["NetWeight"].Value);
				Sd.RemarksDetail = Conversion.ToString(r3.Cells["Remarks"].Value);
				Sd.GrossWeight = Sd.ItemQty;
				Sd.AdLsWeight = 0.0;
				Sd.NetBillWeight = Sd.ItemQty;
				Sd.NetStockWeight = Sd.ItemQty;
				Sd.RateUOM = Conversion.ToDouble(r3.Cells["RateEquivalent"].Value);
				Sd.UomScheduleIdRate = Conversion.ToInt(r3.Cells["RateUomId"].Value);
				Sd.RateCut = Conversion.ToInt(r3.Cells["AddLessRate"].Value);
				Sd.ItemRate = Conversion.ToDouble(r3.Cells["NetRate"].Value);
				Sd.DiscountTypeId = Conversion.ToInt(r3.Cells["DiscountType"].Value);
				Sd.ItemDiscount = Conversion.ToDouble(r3.Cells["Discount%"].Value);
				Sd.ItemDiscountAmount = Conversion.ToDouble(r3.Cells["DiscountAmount"].Value);
				Sd.ItemAmount = Conversion.ToDouble(r3.Cells["ItemAmount"].Value);
				Sd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
				Sd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
				Sd.FcyAmount = Conversion.ToDecimal(r3.Cells["FcyAmount"].Value);
				Sd.TaxNameId = Conversion.ToInt(r3.Cells["TaxTypeId"].Value);
				Sd.TaxPercent = Conversion.ToDouble(r3.Cells["Tax%"].Value);
				Sd.TaxAmount = Math.Round(Conversion.ToDouble(r3.Cells["TaxAmount"].Value), 2);
				Sd.IsTaxable = ((Sd.TaxNameId > 0 && Sd.TaxPercent > 0.0 && Sd.TaxAmount > 0.0) ? "true" : "false");
				Sd.FreightAmount = Conversion.ToDouble(r3.Cells["Freights"].Value);
				Sd.JournalAmount = 0.0;
				Sd.ExpenseAmount = Conversion.ToDouble(r3.Cells["Expense"].Value);
				Sd.BillAmount = Conversion.ToDouble(r3.Cells["BillAmount"].Value);
				Sd.GpDate = Conversion.ToDateTime(r3.Cells["GpDate"].Value);
				Sd.DueDate = Conversion.ToDateTime((object)saleInvoice.DueDate);
				Sd.GpNo = Conversion.ToInt(r3.Cells["GpNo"].Value);
				Sd.VehicleNo = Conversion.ToString(r3.Cells["VehicleNo"].Value);
				Sd.CityId = Conversion.ToInt(r3.Cells["CityName"].Value);
				Sd.LineId = r3.RowIndex + 1;
				if (Sd.WarehouseId == 0)
				{
					throw new Exception("Warehouse Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.ItemId == 0)
				{
					throw new Exception("Item Required In Row #" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.ItemUOMId == 0)
				{
					throw new Exception("Pack Uom Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.CastingTypeId == 0)
				{
					throw new Exception("CastingType Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.ProductionStageId == 0)
				{
					throw new Exception("ProductionStage Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.ItemQty == 0.0)
				{
					throw new Exception("Item Qty Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.ItemRate == 0.0)
				{
					throw new Exception("Item Rate Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.UomScheduleIdRate == 0)
				{
					throw new Exception("Rate Uom Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.DiscountTypeId > 0 && Sd.ItemDiscount <= 0.0 && Sd.ItemDiscountAmount <= 0.0)
				{
					Sd.DiscountTypeId = 0;
				}
				if (Sd.DiscountTypeId > 0 && Sd.ItemDiscount > 0.0 && Sd.ItemDiscountAmount <= 0.0)
				{
					throw new Exception("Discount Amount Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.ItemAmount == 0.0)
				{
					throw new Exception("Item Amount Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.BillAmount == 0.0)
				{
					throw new Exception("Bill Amount Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				if (Sd.CityId == 0)
				{
					throw new Exception("City Name Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
				}
				saleInvoice.InvSaleInvoiceDetaillist.Add(Sd);
			}
			saleInvoice.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToInt((object)r5.Cells["AccountId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceJournal PJ = new InvSaleInvoiceJournal();
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PJ.ChartofAccountId = Conversion.ToInt(r5.Cells["GlAccountId"].Value);
						PJ.TransporterSupCustId = Conversion.ToInt(r5.Cells["AccountId"].Value);
					}
					else
					{
						PJ.ChartofAccountId = Conversion.ToInt(r5.Cells["GlAccountId"].Value);
						PJ.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PJ.JvRemarks = Conversion.ToString(r5.Cells["Remarks"].Value);
					PJ.JvPrcnt = Conversion.ToDouble(r5.Cells["Percentage"].Value);
					PJ.JvQty = Conversion.ToDouble(r5.Cells["Qty"].Value);
					PJ.JvRate = Conversion.ToDouble(r5.Cells["Rate"].Value);
					PJ.JvDebit = Conversion.ToDouble(r5.Cells["Debit"].Value);
					PJ.JvCredit = Conversion.ToDouble(r5.Cells["Credit"].Value);
					if (PJ.ChartofAccountId == 0)
					{
						throw new Exception("Account Required In Row#" + (r5.RowIndex + 1) + " In Party AddLess");
					}
					if (PJ.JvDebit == 0.0 && PJ.JvCredit == 0.0)
					{
						throw new Exception("Credit or Debit Amount Required In Row#" + (r5.RowIndex + 1) + " In Party AddLess");
					}
					saleInvoice.InvSaleInvoiceJournalList.Add(PJ);
				}
			}
			decimal PaymentDetailAmount = default(decimal);
			decimal PrcntOfTotal = default(decimal);
			saleInvoice.SaleInvoicePaymentTermsDetailslist = new List<SaleInvoicePaymentTermsDetail>();
			if (grdPaymentTerm.GetRows().Count() <= 0)
			{
				tabControl2.SelectedIndex = 1;
				((Control)(object)grdPaymentTerm).Focus();
				MessageBox.Show("Payment Detail Record Not Found", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			if (Conversion.ToDouble(dtPaymentTerm.Compute("Sum(Amount)", string.Empty)) > 0.0)
			{
				rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					if (Conversion.ToDecimal(r4.Cells["Amount"].Value) > 0m)
					{
						SaleInvoicePaymentTermsDetail obj = new SaleInvoicePaymentTermsDetail();
						obj.PaymentTermId = Conversion.ToInt(r4.Cells["PaymentTermId"].Value);
						obj.PrcntOfTotal = Conversion.ToDecimal(r4.Cells["%ofTotal"].Value);
						PrcntOfTotal += obj.PrcntOfTotal;
						obj.Amount = Conversion.ToDecimal(r4.Cells["Amount"].Value);
						PaymentDetailAmount += obj.Amount;
						obj.DueDays = Conversion.ToInt(r4.Cells["DueDays"].Value);
						obj.DueDate = Conversion.ToDateTime(r4.Cells["DueDate"].Value);
						obj.PaymentRemarks = Conversion.ToString(r4.Cells["Remarks"].Value);
						if (obj.PaymentTermId <= 0)
						{
							MessageBox.Show("Payment Term Required in row#" + (r4.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							((Control)(object)grdPaymentTerm).Focus();
							return false;
						}
						if (obj.PaymentTermId == 2 && obj.DueDays <= 0)
						{
							MessageBox.Show("Due Days Required In case Of Credit row in row#" + (r4.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							((Control)(object)grdPaymentTerm).Focus();
							return false;
						}
						saleInvoice.SaleInvoicePaymentTermsDetailslist.Add(obj);
					}
				}
			}
			else
			{
				SaleInvoicePaymentTermsDetail obj2 = new SaleInvoicePaymentTermsDetail();
				obj2.PaymentTermId = Conversion.ToInt(CmbPaymentTerm.Value);
				obj2.PrcntOfTotal = 100m;
				PrcntOfTotal = 100m;
				obj2.Amount = Conversion.ToDecimal((object)(100.0 * saleInvoice.BillAmount / 100.0));
				PaymentDetailAmount = Math.Round(obj2.Amount, 4);
				obj2.DueDays = Conversion.ToInt((object)txtDueDays.Text);
				obj2.DueDate = Conversion.ToDateTime((object)DueDate.Value);
				obj2.PaymentRemarks = "";
				saleInvoice.SaleInvoicePaymentTermsDetailslist.Add(obj2);
			}
			if (PrcntOfTotal != 100m)
			{
				MessageBox.Show("Payment Detail Total% not equal to 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				((Control)(object)grdPaymentTerm).Focus();
				return false;
			}
			if (PaymentDetailAmount != Conversion.ToDecimal((object)saleInvoice.BillAmount))
			{
				MessageBox.Show("Payment Detail Amount " + PaymentDetailAmount.ToString("#,##0.####") + " Not Equal to Total Invoice Amount " + saleInvoice.BillAmount.ToString("#,##0.####"), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				((Control)(object)grdPaymentTerm).Focus();
				return false;
			}
			saleInvoice.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0 && Conversion.ToDouble(r2.Cells["Amount"].Value) != 0.0)
				{
					InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
					PE.InvRevExpItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r2.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r2.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					saleInvoice.InvSaleInvoiceExpenseList.Add(PE);
				}
			}
			saleInvoice.AttachmentsList = new List<DMSAttachments>();
			saleInvoice.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvSaleInvoice>(saleInvoice, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			saleInvoice.AttachmentsValues = AttachmentsValues;
			saleInvoice.CustomAttachmentsValues = CustomAttachmentsValues;
			saleInvoice.InvSaleInvoiceCommission = new List<InvSaleInvoiceCommission>();
			int sucess = InvSaleInvoice.Save(saleInvoice);
			if (IsAutoUpdate)
			{
				Reset();
			}
			else
			{
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully [" + saleInvoice.DocNo + "]");
				}
				else
				{
					MessageBox.Show("Record Update Successfully [" + saleInvoice.DocNo + "]");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 1661, base.Name);
				}
				int SupCustId = Conversion.ToInt(cmbsuppliername.Value);
				DateTime Date = Conversion.ToDateTime((object)DocDate.Value);
				Reset();
				if (ChkBok.Checked)
				{
					CommonServices.SaleInvoicePrint1661(sucess, SupCustId, 0, Date, Date);
				}
				if (ChkVoucherPreview.Checked)
				{
					VoucherHeadIdGetDBCall(sucess);
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1661);
					VoucherHeadId = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return false;
		}
		return true;
	}

	private void btnSave_Click(object sender, EventArgs e)
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

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			SaleOrderId = 0;
			RecId = Conversion.ToInt((object)ID);
			InvSaleInvoice invSI = InvSaleInvoice.GetByID(RecId);
			txtdocno.Text = invSI.DocNo.ToString();
			txtBranchSrNo.Text = invSI.BranchSrNo.ToString();
			DocDate.Value = invSI.DocDate;
			tabControl1.SelectedIndex = 0;
			cmbsuppliername.Value = invSI.SupplierCustomerId;
			txtbillno.Text = invSI.ManualBillNo;
			((Control)(object)CmbDeliveryTerm).Text = invSI.DeliveryTerm;
			CmbPaymentTerm.Value = invSI.PaymentTermId;
			txtDueDays.Text = Conversion.ToString((object)invSI.DueDays);
			DueDate.Value = Conversion.ToDateTime((object)invSI.DueDate);
			if (invSI.TaxAccountId > 0)
			{
				CmbTaxAccount.Value = invSI.TaxAccountId;
			}
			if (invSI.TransporterId > 0)
			{
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					CmbFreightAcCr.Value = invSI.TransporterId;
				}
				else
				{
					CmbFreightAcCr.Value = invSI.TransporterCreditPartyId;
				}
			}
			if (invSI.TransporterDebitGLId > 0)
			{
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					cmbFreightAcDr.Value = invSI.TransporterDebitGLId;
				}
				else
				{
					cmbFreightAcDr.Value = invSI.TransporterDebitPartyId;
				}
			}
			txtFreightAmount.Text = invSI.FreightAmount.ToString("#,##0.###");
			txtFreightRemarks.Text = invSI.FreightRemark;
			txtBillAmount.Text = invSI.BillAmount.ToString("#,##0.####");
			cmbCurrency.Value = invSI.CurrencyId;
			txtFcyAmount.Text = invSI.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			txtRemarks.Text = invSI.RemarksHeader;
			txtDiscountAmountHeader.Text = invSI.DiscountAmount.ToString("#,##0.####");
			if (invSI.DiscountAccountId > 0)
			{
				CmbDiscountAccount.Value = invSI.DiscountAccountId;
			}
			if (invSI.CurrencyId > 0)
			{
				txtExchangeRate.Text = Conversion.ToString((object)invSI.ExchangeRate);
			}
			else if (invSI.CurrencyId == 0)
			{
				ConfigurationDefault();
			}
			AttachmentsValues = invSI.AttachmentsValues;
			CustomAttachmentsValues = invSI.CustomAttachmentsValues;
			VoucherHeadIdGetDBCall(RecId);
			dtGrid.Rows.Clear();
			for (int l = 0; l < invSI.InvSaleInvoiceDetaillist.Count; l++)
			{
				if (l == 0)
				{
					SaleOrderId = invSI.InvSaleInvoiceDetaillist[0].SaleOrderId;
				}
				dtGrid.Rows.Add(invSI.InvSaleInvoiceDetaillist[l].Id, invSI.InvSaleInvoiceDetaillist[l].SaleOrderId, invSI.InvSaleInvoiceDetaillist[l].SaleOrderDetailId, invSI.InvSaleInvoiceDetaillist[l].SaleOrder, invSI.InvSaleInvoiceDetaillist[l].OrderDate, invSI.InvSaleInvoiceDetaillist[l].WarehouseId, invSI.InvSaleInvoiceDetaillist[l].ItemId, invSI.InvSaleInvoiceDetaillist[l].ItemCode, invSI.InvSaleInvoiceDetaillist[l].ItemName, invSI.InvSaleInvoiceDetaillist[l].ItemUOMId, invSI.InvSaleInvoiceDetaillist[l].UOMCodeItem, invSI.InvSaleInvoiceDetaillist[l].PackEquivalent, invSI.InvSaleInvoiceDetaillist[l].ItemVariantId, invSI.InvSaleInvoiceDetaillist[l].VariantDescription, invSI.InvSaleInvoiceDetaillist[l].CastingTypeId, invSI.InvSaleInvoiceDetaillist[l].ProductionStageId, invSI.InvSaleInvoiceDetaillist[l].RemarksDetail, invSI.InvSaleInvoiceDetaillist[l].ItemQty, invSI.InvSaleInvoiceDetaillist[l].WeightFinishGoods, invSI.InvSaleInvoiceDetaillist[l].ItemRate - invSI.InvSaleInvoiceDetaillist[l].RateCut, invSI.InvSaleInvoiceDetaillist[l].UomScheduleIdRate, invSI.InvSaleInvoiceDetaillist[l].UOMCodeRate, invSI.InvSaleInvoiceDetaillist[l].RateUOM, invSI.InvSaleInvoiceDetaillist[l].RateCut, invSI.InvSaleInvoiceDetaillist[l].ItemRate, invSI.InvSaleInvoiceDetaillist[l].DiscountTypeId, invSI.InvSaleInvoiceDetaillist[l].ItemDiscount, invSI.InvSaleInvoiceDetaillist[l].ItemDiscountAmount, invSI.InvSaleInvoiceDetaillist[l].ItemAmount, invSI.InvSaleInvoiceDetaillist[l].FcyAmount, invSI.InvSaleInvoiceDetaillist[l].TaxNameId, invSI.InvSaleInvoiceDetaillist[l].TaxPercent, invSI.InvSaleInvoiceDetaillist[l].TaxAmount, invSI.InvSaleInvoiceDetaillist[l].ExpenseAmount, invSI.InvSaleInvoiceDetaillist[l].FreightAmount, invSI.InvSaleInvoiceDetaillist[l].BillAmount, invSI.InvSaleInvoiceDetaillist[l].GpDate.ToShortDateString(), invSI.InvSaleInvoiceDetaillist[l].GpNo, invSI.InvSaleInvoiceDetaillist[l].VehicleNo, invSI.InvSaleInvoiceDetaillist[l].CityId);
			}
			if (SaleOrderId > 0)
			{
				((Control)(object)cmbsuppliername).Enabled = false;
			}
			dtInvExp.Rows.Clear();
			for (int k = 0; k < invSI.InvSaleInvoiceExpenseList.Count; k++)
			{
				dtInvExp.Rows.Add(invSI.InvSaleInvoiceExpenseList[k].InvRevExpItemId, invSI.InvSaleInvoiceExpenseList[k].Qty, invSI.InvSaleInvoiceExpenseList[k].Rate, invSI.InvSaleInvoiceExpenseList[k].Amount, invSI.InvSaleInvoiceExpenseList[k].Remarks);
			}
			dtGrdGL.Rows.Clear();
			for (int j = 0; j < invSI.InvSaleInvoiceJournalList.Count; j++)
			{
				dtGrdGL.Rows.Add(SubsidiaryAccountAllownOnVouchers ? invSI.InvSaleInvoiceJournalList[j].TransporterSupCustId : invSI.InvSaleInvoiceJournalList[j].ChartofAccountId, invSI.InvSaleInvoiceJournalList[j].ChartofAccountId, invSI.InvSaleInvoiceJournalList[j].JvRemarks, invSI.InvSaleInvoiceJournalList[j].JvPrcnt, invSI.InvSaleInvoiceJournalList[j].JvQty, invSI.InvSaleInvoiceJournalList[j].JvRate, invSI.InvSaleInvoiceJournalList[j].JvDebit, invSI.InvSaleInvoiceJournalList[j].JvCredit);
			}
			dtPaymentTerm.Rows.Clear();
			for (int i = 0; i < invSI.SaleInvoicePaymentTermsDetailslist.Count; i++)
			{
				dtPaymentTerm.Rows.Add(invSI.SaleInvoicePaymentTermsDetailslist[i].PaymentTermId, invSI.SaleInvoicePaymentTermsDetailslist[i].PrcntOfTotal, invSI.SaleInvoicePaymentTermsDetailslist[i].Amount, invSI.SaleInvoicePaymentTermsDetailslist[i].DueDays, invSI.SaleInvoicePaymentTermsDetailslist[i].DueDate, invSI.SaleInvoicePaymentTermsDetailslist[i].PaymentRemarks);
			}
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			if (dtGrdGL.Rows.Count == 0)
			{
				AddRowInGLGrid();
			}
			grdGLedger.DataSource = dtGrdGL;
			grdGLedger.RetrieveStructure();
			grdPaymentTerm.DataSource = dtPaymentTerm;
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			AddPaymentRowsRowsInPaymentGrid();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdInvExpSettings();
			gridGLSettings();
			FreightProportion();
			ExpProportion();
			LedgerProportion();
			BillAmount();
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			tabControl1.SelectedIndex = 0;
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			txtExchangeRate_TextChanged(null, null);
			DocDate.Focus();
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 1661,
						Id = RecId,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("RecordId Not Found.....");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillAmount()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				double BillAmount = 0.0;
				Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double TaxAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["TaxAmount"], (AggregateFunction)2).ToString());
				double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				double JournalDebit = 0.0;
				double JournalCredit = 0.0;
				double TransporterCredit = 0.0;
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) > 0)
					{
						JournalDebit += Conversion.ToDouble((object)r.Cells["Debit"].Value.ToString());
						JournalCredit += Conversion.ToDouble((object)r.Cells["Credit"].Value.ToString());
					}
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt(CmbFreightAcCr.Value) > 0 && txtSupplierGLId.Text.ToString() == Conversion.ToString(CmbFreightAcCr.Value))
					{
						BillAmount -= Conversion.ToDouble((object)txtFreightAmount.Text);
					}
				}
				else if (Conversion.ToInt(CmbFreightAcCr.Value) > 0 && Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(CmbFreightAcCr.Value))
				{
					BillAmount -= Conversion.ToDouble((object)txtFreightAmount.Text);
				}
				if (((UltraGridBase)cmbFreightAcDr).ActiveRow != null && Conversion.ToInt(cmbFreightAcDr.Value) > 0)
				{
					if (!SubsidiaryAccountAllownOnVouchers)
					{
						if (txtSupplierGLId.Text.ToString() == Conversion.ToString(((UltraDropDownBase)cmbFreightAcDr).SelectedRow.Cells[0].Value))
						{
							BillAmount += Conversion.ToDouble((object)txtFreightAmount.Text);
						}
					}
					else if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(cmbFreightAcDr.Value))
					{
						BillAmount += Conversion.ToDouble((object)txtFreightAmount.Text);
					}
				}
				BillAmount = BillAmount + ItemAmount + TaxAmount + ExpAmount;
				double GlDifference = JournalCredit - JournalDebit;
				double DiffGl = Math.Abs(GlDifference);
				BillAmount = ((!(GlDifference < 0.0)) ? (BillAmount + GlDifference) : (BillAmount - DiffGl));
				double DiscAmountHeader = Conversion.ToDouble((object)txtDiscountAmountHeader.Text);
				BillAmount = BillAmount + TransporterCredit - DiscAmountHeader;
				txtBillAmount.Text = BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtLedgerBalanceAfterBill.Text = Math.Round(LedgerBalance + BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatboth);
				txtFcyAmount.Text = (BillAmount / Conversion.ToDouble((object)txtExchangeRate.Text)).ToString("#,##0.####");
			}
			else
			{
				txtFcyAmount.Text = "0";
			}
			BillProportion();
			PaymentTermAmountCalculateFromPercent();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double TotalQty = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Qty = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
					obj.Cells["Expense"].Value = TotalAmount / TotalQty * Qty;
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Expense"].Value = 0;
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightProportion()
	{
		try
		{
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)txtFreightAmount.Text);
			if (CreditForFreight > 0.0 && Conversion.ToInt(CmbFreightAcCr.Value) > 0 && Conversion.ToInt(cmbFreightAcDr.Value) <= 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
					double freight = Math.Round(CreditForFreight / NetWeight * Weight, 4);
					obj.Cells["Freights"].Value = freight;
				}
			}
			else if (!(CreditForFreight > 0.0) || Conversion.ToInt(cmbFreightAcDr.Value) > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Freights"].Value = 0;
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LedgerProportion()
	{
	}

	private void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				r.Cells["BillAmount"].Value = Conversion.ToDouble(r.Cells["ItemAmount"].Value) + Conversion.ToDouble(r.Cells["TaxAmount"].Value) + Conversion.ToDouble(r.Cells["Expense"].Value) + Conversion.ToDouble(r.Cells["Freights"].Value);
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsuppliername_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			txtLedgerBalance.Text = "0";
			int SupplierId = Conversion.ToInt(cmbsuppliername.Value);
			LedgerBalance = 0.0;
			if (dtSupplier.Rows.Count > 0 && ((UltraGridBase)cmbsuppliername).ActiveRow != null && SupplierId > 0)
			{
				int GLID = Conversion.ToInt(dtSupplier.Select("Id = " + SupplierId)[0]["GlAccountId"]);
				txtSupplierGLId.Text = Conversion.ToString((object)GLID);
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					DataTable Baldt = CommonServices.GetAccountsBalancesOpeningCurrentAndClosing(GLID, clsGlobalVariables.ActiveYr.Start_Period, DocDate.Value);
					if (Baldt.Rows.Count > 0)
					{
						LedgerBalance = Conversion.ToDouble(Baldt.Rows[0]["ClosingBalance"]);
						txtLedgerBalance.Text = Math.Round(LedgerBalance).ToString("#,#;(#,#);0");
					}
				}
				else
				{
					DataTable Baldt2 = CommonServices.GetSubsidiaryLedgerBalance(SupplierId, DocDate.Value);
					if (Baldt2.Rows.Count > 0)
					{
						LedgerBalance = Conversion.ToDouble(Baldt2.Rows[0]["Balance"]);
						txtLedgerBalance.Text = Math.Round(LedgerBalance).ToString("#,#;(#,#);0");
					}
				}
			}
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTaxTypeIdAndTaxPercent()
	{
		try
		{
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(Conversion.ToInt(CmbItem.Value), DocDate.Value);
			if (dt.Rows.Count > 0)
			{
				DataTable dtTax = new DataTable();
				dtTax.Columns.Add("Id");
				dtTax.Columns.Add("TaxType");
				dtTax.Columns.Add("TaxPrcnt");
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
				DDL.BindDDL(dtTax, CmbTaxType, "Id", "TaxType", "TaxType", ZeroIndex: false);
				((UltraGridBase)CmbTaxType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbTaxType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)CmbTaxType).Text = string.Empty;
				((UltraGridBase)CmbTaxType).DataSource = null;
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
			}
			if (updateDetailIndex > -1 && Conversion.ToInt(grd.CurrentRow.Cells["Tax%"].Value) > 0)
			{
				CmbTaxType.Value = Conversion.ToInt(grd.CurrentRow.Cells["Tax%"].Value);
			}
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateAmount()
	{
		try
		{
			double RateEquivalent = ((((UltraGridBase)CmbRateUom).ActiveRow != null) ? Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value) : 0.0);
			double Qty = Conversion.ToDouble((object)txtItemQty.Text);
			double Rate = Conversion.ToDouble((object)txtRate.Text);
			double AddLessRate = Conversion.ToDouble((object)txtAddLessRate.Text);
			double discountPrcnt = Conversion.ToDouble((object)txtDiscPercent.Text);
			double discountAmount = 0.0;
			if (Qty > 0.0 && Rate > 0.0 && RateEquivalent > 0.0)
			{
				double NetRate = ((AddLessRate != 0.0) ? (Rate + AddLessRate) : Rate);
				txtNetRate.Text = NetRate.ToString(clsGlobalVariables.DecimalRateFormate);
				double Amount = Qty / RateEquivalent * NetRate;
				if (Conversion.ToInt(CmbDiscountType.Value) == 1)
				{
					discountAmount = ((discountPrcnt > 0.0) ? discountPrcnt : 0.0);
				}
				else if (Conversion.ToInt(CmbDiscountType.Value) == 2)
				{
					if (discountPrcnt > 100.0)
					{
						MessageBox.Show("Discount Percent Can't Greater Than 100.", "WARNING!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						txtDiscPercent.Text = 100.ToString();
						discountPrcnt = 100.0;
					}
					discountAmount = ((discountPrcnt > 0.0) ? (Amount * discountPrcnt / 100.0) : 0.0);
				}
				else
				{
					discountAmount = 0.0;
					txtDiscPercent.Text = "0";
					txtDiscountAmount.Text = "0";
				}
				txtDiscountAmount.Text = discountAmount.ToString("#,##0.###");
				Amount -= discountAmount;
				txtAmount.Text = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtAmount.Text = "0";
				txtNetRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTaxAmount()
	{
		try
		{
			if (((UltraGridBase)CmbTaxType).ActiveRow != null)
			{
				double TaxPercent = 0.0;
				if (!(Conversion.ToDouble((object)txtTaxPercent.Text) > 0.0))
				{
					txtTaxPercent.Text = Conversion.ToDouble(((UltraDropDownBase)CmbTaxType).SelectedRow.Cells[2].Value).ToString("#,##0.###");
					TaxPercent = Conversion.ToDouble(((UltraDropDownBase)CmbTaxType).SelectedRow.Cells[2].Value);
				}
				else
				{
					TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
				}
				if (TaxPercent > 100.0)
				{
					MessageBox.Show("Tax % Can't be greater than 100...");
					TaxPercent = 100.0;
					txtTaxPercent.Text = TaxPercent.ToString();
				}
				double num = Conversion.ToDouble((object)txtAmount.Text);
				double TaxAmt = num * TaxPercent / 100.0;
				txtTaxAmount.Text = TaxAmt.ToString(clsGlobalVariables.stringFormatsingle);
				double TotalAmount = num + TaxAmt;
				txtTotalAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGrossWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLss_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtratecut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTaxType).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtRate.Text = Conversion.ToDouble((object)txtRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxAmount_TextChanged(object sender, EventArgs e)
	{
	}

	private void CmbTaxType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTaxType).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDiscountType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbFreightAcCr_Leave(object sender, EventArgs e)
	{
		try
		{
			int FrCrId = Conversion.ToInt(CmbFreightAcCr.Value);
			int FrDrId = Conversion.ToInt(cmbFreightAcDr.Value);
			if (FrCrId > 0 && FrDrId > 0 && FrDrId == FrCrId)
			{
				((Control)(object)CmbFreightAcCr).Text = string.Empty;
				MessageBox.Show("Freight Credit And Freight Debit Account Can't be same", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			FreightProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbFreightAcDr_Leave(object sender, EventArgs e)
	{
		try
		{
			int FrDrId = Conversion.ToInt(cmbFreightAcDr.Value);
			int FrCrId = Conversion.ToInt(CmbFreightAcCr.Value);
			if (FrDrId > 0 && FrDrId > 0 && FrDrId == FrCrId)
			{
				((Control)(object)cmbFreightAcDr).Text = string.Empty;
				MessageBox.Show("Freight Credit And Freight Debit Account Can't be same", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			FreightProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FreightProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscountAmountHeader_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtBillAmount.Text) > 0.0 && Conversion.ToDouble((object)txtDiscountAmountHeader.Text) >= Conversion.ToDouble((object)txtBillAmount.Text))
			{
				txtDiscountAmountHeader.Text = 0.ToString("#,##.###");
				MessageBox.Show("Discount Amount Can not be Greater than or Equal to Bill Amount");
			}
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetByItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		try
		{
			double CurrStock = InventoryStockEvalautionDetail.StockEvaluation_GetCurrStockByItem_Engr(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocDate = Conversion.ToDateTime((object)DocDate.Value),
				WarehouseId = Conversion.ToInt(CmbWarehouse.Value),
				ItemId = Conversion.ToInt(CmbItem.Value),
				CastingTypeId = Conversion.ToInt(CmbCastingTypeDetail.Value),
				ProductionStageId = Conversion.ToInt(CmbProductionStage.Value),
				VarientId = Conversion.ToInt(CmbVariantDescription.Value),
				stockUOM = Conversion.ToInt(CmbPackUom.Value)
			});
			if (CurrStock > 0.0)
			{
				lblBalance.Text = CurrStock.ToString();
			}
			else
			{
				lblBalance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbVariantDescription_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCastingTypeDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbProductionStage_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			SaleOrderId = 0;
			DataTable dt = LoadSo.dtLoader;
			if (dt.Rows.Count > 0)
			{
				DetailPanel.Hide();
				if (grd.GetRows().Count() > 0 && Conversion.ToInt(grd.GetRow(0).Cells["OrderId"].Value) != Conversion.ToInt(dt.Rows[0]["Id"]))
				{
					throw new Exception("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!");
				}
				((Control)(object)cmbsuppliername).Enabled = false;
				cmbsuppliername.Value = Conversion.ToInt(dt.Rows[0]["OrderSupCustId"]);
				txtRefrenceNo.Text = Conversion.ToString(dt.Rows[0]["SupplierRefNo"]);
				((Control)(object)CmbPaymentTerm).Text = Conversion.ToString(dt.Rows[0]["TermsDescription"]);
				txtDueDays.Text = Conversion.ToString(dt.Rows[0]["OrderDueDays"]);
				((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString(dt.Rows[0]["DeliveryTerm"]);
				txtRemarks.Text = Conversion.ToString(dt.Rows[0]["RemarksHeader"]);
				bool flag = false;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (grd.GetRows().Count() > 0)
					{
						GridEXRow[] rows = grd.GetRows();
						for (int j = 0; j < rows.Length; j++)
						{
							if (Conversion.ToInt(rows[j].Cells["OrderDetailId"].Value) == Conversion.ToInt(dt.Rows[i]["PoDId"]))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						dtGrid.Rows.Add(0, dt.Rows[i]["Id"], dt.Rows[i]["PoDId"], dt.Rows[i]["DocNo"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), 0, dt.Rows[i]["OrderItemId"], dt.Rows[i]["ItemCode"], dt.Rows[i]["ItemName"], dt.Rows[i]["PackUomId"], dt.Rows[i]["PackUom"], dt.Rows[i]["PackEquivalent"], dt.Rows[i]["ItemVariantId"], dt.Rows[i]["VarientDescription"], dt.Rows[i]["CastingTypeId"], 0, dt.Rows[i]["DetailRemarks"], dt.Rows[i]["BalQty"], dt.Rows[i]["WeightFinishGoods"], dt.Rows[i]["OrderItemRate"], dt.Rows[i]["RateUomId"], dt.Rows[i]["RateUom"], dt.Rows[i]["EquivalentRate"], 0, dt.Rows[i]["OrderItemRate"], dt.Rows[i]["DiscountTypeId"], dt.Rows[i]["discountPercent"], dt.Rows[i]["DiscountAmount"], dt.Rows[i]["ItemAmount"], 0, dt.Rows[i]["TaxNameId"], dt.Rows[i]["TaxPercent"], dt.Rows[i]["TaxAmount"], 0, 0, 0, DateTime.Now, 0, 0, dt.Rows[i]["CityId"]);
					}
					flag = false;
				}
				SaleOrderId = Conversion.ToInt(dt.Rows[0]["Id"]);
				LoadPaymentDetailBySaleOrderIds(Conversion.ToString(dt.Rows[0]["Id"]));
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdPaymentTerm.DataSource = dtPaymentTerm;
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			AddPaymentRowsRowsInPaymentGrid();
			ExpProportion();
			FreightProportion();
			LedgerProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadPaymentDetailBySaleOrderIds(string Ids)
	{
		try
		{
			DataTable item = InvSaleInvoice.SaleOrderPaymentTermDetailBySoIds(Ids);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			if (Conversion.ToDouble(dtPaymentTerm.Compute("Sum(Amount)", string.Empty)) == 0.0)
			{
				dtPaymentTerm.Rows.Clear();
				for (int i = 0; i < item.Rows.Count; i++)
				{
					dtPaymentTerm.Rows.Add(item.Rows[i]["PaymentTermId"], item.Rows[i]["PrcntOfTotal"], item.Rows[i]["Amount"], item.Rows[i]["DueDays"], item.Rows[i]["DueDate"], Conversion.ToString(item.Rows[i]["PaymentRemarks"]));
				}
				return;
			}
			foreach (DataRow paymentTermRow in dtPaymentTerm.Rows)
			{
				int paymentTermId = Conversion.ToInt(paymentTermRow["PaymentTermId"]);
				DataRow[] matchingItemRows = item.Select($"PaymentTermId = {paymentTermId}");
				if (matchingItemRows.Length != 0)
				{
					DataRow itemRow = matchingItemRows[0];
					paymentTermRow["%ofTotal"] = Convert.ToDouble(itemRow["PrcntOfTotal"]);
					paymentTermRow["Amount"] = Convert.ToDouble(itemRow["Amount"]);
					paymentTermRow["DueDays"] = Convert.ToInt32(itemRow["DueDays"]);
					paymentTermRow["DueDate"] = Convert.ToDateTime(itemRow["DueDate"]);
					paymentTermRow["Remarks"] = Convert.ToString(itemRow["PaymentRemarks"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton3_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				LoadSo = new frmLoadSaleOrderEngr(UserAccount);
				LoadSo.ShowDialog();
				LoadSo.BringToFront();
				LoadInGridDetail();
				return;
			}
			if (Conversion.ToInt(grd.GetRow(0).Cells["OrderId"].Value) > 0)
			{
				LoadSo = new frmLoadSaleOrderEngr(UserAccount);
				LoadSo.ShowDialog();
				LoadSo.BringToFront();
				LoadInGridDetail();
				return;
			}
			throw new Exception("You Can't Load Sale Order Because Direct Entry Already Exist");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryCombosBranchFill()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)cmbBranchName).Text);
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				DataTable dt = InvSaleInvoice.GetBranchsAllocatedToUserFromSaleInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 1661);
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
					}
				}
			}
			if (dtBranch.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: true);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryComboFill(bool ValidateBranch = true)
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
			obj.DocumentTypeIds = "1661";
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
			else
			{
				if (ValidateBranch)
				{
					cmbBranchName.Focus();
					throw new Exception("Select branch first");
				}
				obj.BranchesIds = UserAccount.BranchesId.ToString();
			}
			DataTable dt = InvSaleInvoice.AllComboBindAgainstSaleInvoice(obj);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dtPaymentTerm = new DataTable();
			dtPaymentTerm.Columns.Add("Id", typeof(int));
			dtPaymentTerm.Columns.Add("PaymentTerm", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "PaymentTerm")
				{
					dtPaymentTerm.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Customer", ZeroIndex: false);
			DDL.BindDDL(dtPaymentTerm, CmbPaymentTermHistory, "Id", "PaymentTerm", "Payment Term", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton4_Click(object sender, EventArgs e)
	{
		try
		{
			GetAll();
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
		}
	}

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters SI = new ReportsParameters();
			SI.OrganizationId = UserAccount.OrganizationId;
			SI.CompanyId = UserAccount.CompanyId;
			SI.DocumentTypeId = 1661;
			SI.CanViewAllRecord = FormRights.DoHaveCanViewAllRecordRights;
			SI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			SI.EntryUser = UserAccount.ID;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			SI.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			SI.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			SI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			SI.PaymenetTermId = Conversion.ToInt(CmbPaymentTermHistory.Value);
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
				SI.BranchesIds = BranchIds;
				DataTable dtGetAll = InvSaleInvoice.FormHistory(SI);
				if (dtGetAll.Rows.Count > 0)
				{
					if (dthistory.Columns.Count == 0)
					{
						dthistory.Columns.Clear();
						dthistory.Columns.Add("Id", typeof(int));
						dthistory.Columns.Add("VoucherHeadId", typeof(int));
						dthistory.Columns.Add("DocNo", typeof(int));
						dthistory.Columns.Add("DocDate", typeof(DateTime));
						dthistory.Columns.Add("DueDate", typeof(DateTime));
						dthistory.Columns.Add("ManualBillNo");
						dthistory.Columns.Add("SupplierCustomerId", typeof(int));
						dthistory.Columns.Add("CustomerName");
						dthistory.Columns.Add("BillAmount", typeof(double));
						dthistory.Columns.Add("EntryUser");
						dthistory.Columns.Add("EntryDate", typeof(DateTime));
						dthistory.Columns.Add("ModifyUser");
						dthistory.Columns.Add("ModifyDate", typeof(DateTime));
						dthistory.Columns.Add("ApprovedUser");
						dthistory.Columns.Add("ApprovedDate", typeof(DateTime));
						dthistory.Columns.Add("NoOfAttachments", typeof(int));
						dthistory.Columns.Add("Remarks");
					}
					dthistory.Clear();
					for (int i = 0; i < dtGetAll.Rows.Count; i++)
					{
						dthistory.Rows.Add(dtGetAll.Rows[i]["Id"], dtGetAll.Rows[i]["VoucherHeadId"], dtGetAll.Rows[i]["DocNo"], Conversion.ToDateTime(dtGetAll.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToDateTime(dtGetAll.Rows[i]["DueDate"]).ToShortDateString(), dtGetAll.Rows[i]["ManualBillNo"], dtGetAll.Rows[i]["SupplierCustomerId"], dtGetAll.Rows[i]["CustomerName"], dtGetAll.Rows[i]["BillAmount"], dtGetAll.Rows[i]["UserName"], dtGetAll.Rows[i]["EntryDate"], dtGetAll.Rows[i]["ModifyUserName"], dtGetAll.Rows[i]["ModifyDate"], dtGetAll.Rows[i]["ApprovedUserName"], dtGetAll.Rows[i]["ApprovedDate"], dtGetAll.Rows[i]["NoOfAttachments"], dtGetAll.Rows[i]["RemarksHeader"]);
					}
					grdHistory.DataSource = dthistory;
					grdHistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					grdHistory.ClearStructure();
					grdDetail.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
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
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdHistory.RootTable.Columns["ManualBillNo"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["CustomerName"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["BillAmount"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["EntryUser"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(grdHistory, 3, 3);
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ApprovedByConstant;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["Edit"].Width = 40;
			grdHistory.RootTable.Columns.Add("Print-1661");
			grdHistory.RootTable.Columns["Print-1661"].Caption = "Print-1661";
			grdHistory.RootTable.Columns["Print-1661"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print-1661"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print-1661"].ButtonText = "Print-1661";
			grdHistory.RootTable.Columns["Print-1661"].Position = 1;
			grdHistory.RootTable.Columns["Print-1661"].Width = 80;
			grdHistory.RootTable.Columns.Add("Print-1661A");
			grdHistory.RootTable.Columns["Print-1661A"].Caption = "Print-1661A";
			grdHistory.RootTable.Columns["Print-1661A"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print-1661A"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print-1661A"].ButtonText = "Print-1661A";
			grdHistory.RootTable.Columns["Print-1661A"].Position = 2;
			grdHistory.RootTable.Columns["Print-1661A"].Width = 80;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Position = 3;
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(id);
				}
				else if (e.Column.Key == "Print-1661")
				{
					int SupCustId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(item.Cells["DocDate"].Value);
					CommonServices.SaleInvoicePrint1661(id, SupCustId, 0, Date, Date);
				}
				else if (e.Column.Key == "Print-1661A")
				{
					GeneratePrint1661A(id);
				}
				else if (e.Column.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(item.Cells["VoucherHeadId"].Value), 1661);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1661);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow Row = grdHistory.CurrentRow;
			if (Row != null && (int)Row.RowType == 0 && Row.Cells.Count > 0)
			{
				BindDetailOfHeaderId(Conversion.ToInt(Row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDetailOfHeaderId(int Id)
	{
		try
		{
			InvSaleInvoice invSI = InvSaleInvoice.GetByID(Id);
			if (invSI.InvSaleInvoiceDetaillist != null && invSI.InvSaleInvoiceDetaillist.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderDetailId", typeof(int));
				dt.Columns.Add("OrderNo", typeof(int));
				dt.Columns.Add("OrderDate", typeof(DateTime));
				dt.Columns.Add("Warehouse", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackUomId", typeof(int));
				dt.Columns.Add("PackUom");
				dt.Columns.Add("PackEquivalent", typeof(double));
				dt.Columns.Add("VariantId", typeof(int));
				dt.Columns.Add("VariantDescription", typeof(string));
				dt.Columns.Add("CastingType", typeof(string));
				dt.Columns.Add("ProductionStage", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUomId", typeof(int));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("RateEquivalent", typeof(double));
				dt.Columns.Add("AddLessRate", typeof(double));
				dt.Columns.Add("NetRate", typeof(double));
				dt.Columns.Add("DiscountType", typeof(string));
				dt.Columns.Add("Discount%", typeof(double));
				dt.Columns.Add("DiscountAmount", typeof(double));
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("TaxType", typeof(string));
				dt.Columns.Add("Tax%", typeof(double));
				dt.Columns.Add("TaxAmount", typeof(double));
				dt.Columns.Add("Expense", typeof(double));
				dt.Columns.Add("Freights", typeof(double));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("GpNo");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("CityName");
				for (int i = 0; i < invSI.InvSaleInvoiceDetaillist.Count; i++)
				{
					dt.Rows.Add(invSI.InvSaleInvoiceDetaillist[i].Id, invSI.InvSaleInvoiceDetaillist[i].SaleOrderId, invSI.InvSaleInvoiceDetaillist[i].SaleOrderDetailId, invSI.InvSaleInvoiceDetaillist[i].SaleOrder, invSI.InvSaleInvoiceDetaillist[i].OrderDate, invSI.InvSaleInvoiceDetaillist[i].WareHouseName, invSI.InvSaleInvoiceDetaillist[i].ItemId, invSI.InvSaleInvoiceDetaillist[i].ItemCode, invSI.InvSaleInvoiceDetaillist[i].ItemName, invSI.InvSaleInvoiceDetaillist[i].ItemUOMId, invSI.InvSaleInvoiceDetaillist[i].UOMCodeItem, invSI.InvSaleInvoiceDetaillist[i].PackEquivalent, invSI.InvSaleInvoiceDetaillist[i].ItemVariantId, invSI.InvSaleInvoiceDetaillist[i].VariantDescription, invSI.InvSaleInvoiceDetaillist[i].CastingType, invSI.InvSaleInvoiceDetaillist[i].ProductionProcessStage, invSI.InvSaleInvoiceDetaillist[i].RemarksDetail, invSI.InvSaleInvoiceDetaillist[i].ItemQty, invSI.InvSaleInvoiceDetaillist[i].WeightFinishGoods, invSI.InvSaleInvoiceDetaillist[i].ItemRate - invSI.InvSaleInvoiceDetaillist[i].RateCut, invSI.InvSaleInvoiceDetaillist[i].UomScheduleIdRate, invSI.InvSaleInvoiceDetaillist[i].UOMCodeRate, invSI.InvSaleInvoiceDetaillist[i].RateUOM, invSI.InvSaleInvoiceDetaillist[i].RateCut, invSI.InvSaleInvoiceDetaillist[i].ItemRate, invSI.InvSaleInvoiceDetaillist[i].DiscountType, invSI.InvSaleInvoiceDetaillist[i].ItemDiscount, invSI.InvSaleInvoiceDetaillist[i].ItemDiscountAmount, invSI.InvSaleInvoiceDetaillist[i].ItemAmount, invSI.InvSaleInvoiceDetaillist[i].FcyAmount, invSI.InvSaleInvoiceDetaillist[i].TaxName, invSI.InvSaleInvoiceDetaillist[i].TaxPercent, invSI.InvSaleInvoiceDetaillist[i].TaxAmount, invSI.InvSaleInvoiceDetaillist[i].ExpenseAmount, invSI.InvSaleInvoiceDetaillist[i].FreightAmount, invSI.InvSaleInvoiceDetaillist[i].BillAmount, invSI.InvSaleInvoiceDetaillist[i].GpDate.ToShortDateString(), invSI.InvSaleInvoiceDetaillist[i].GpNo, invSI.InvSaleInvoiceDetaillist[i].VehicleNo, invSI.InvSaleInvoiceDetaillist[i].CityName);
					grdDetail.DataSource = dt;
					grdDetail.RetrieveStructure();
					grdDetailSettings();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSettings()
	{
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["VariantId"].Visible = false;
			grdDetail.RootTable.Columns["PackUomId"].Visible = false;
			grdDetail.RootTable.Columns["PackEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["RateUomId"].Visible = false;
			grdDetail.RootTable.Columns["Freights"].Visible = false;
			grdDetail.RootTable.Columns["RateEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdDetail.RootTable.Columns["PackUom"].Caption = "Uom";
			grdDetail.RootTable.Columns["VariantDescription"].Caption = "Modal Description";
			grdDetail.RootTable.Columns["ItemQty"].Caption = "Qty";
			grdDetail.RootTable.Columns["NetWeight"].Caption = "Item FG Weight";
			grdDetail.RootTable.Columns["Remarks"].Caption = "Specification/Remarks";
			GridEXRow row = grdDetail.GetRow(0);
			int OrderId = ((row != null) ? Conversion.ToInt(row.Cells["OrderId"].Value) : 0);
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				CommonServices.GridColumnSettings(item, 3);
			}
			grdDetail.RootTable.Columns["Tax%"].EditType = (EditType)(TaxPercntEditableOrNot ? 1 : 0);
			grdDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grdDetail.RootTable.Columns["OrderDate"].Width = Constants.InventoryConstants.DateConstant;
			grdDetail.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["PackEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.VariantEquivalent;
			grdDetail.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.CastingType;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["AddLessRate"].Width = Constants.InventoryConstants.Address;
			grdDetail.RootTable.Columns["NetRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["DiscountType"].Width = Constants.InventoryConstants.DiscountType;
			grdDetail.RootTable.Columns["Discount%"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["DiscountAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["TaxType"].Width = Constants.InventoryConstants.DiscountType;
			grdDetail.RootTable.Columns["Tax%"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Freights"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdDetail.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdDetail.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdDetail.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grdDetail.RootTable.Columns["ProductionStage"].Width = Constants.InventoryConstants.ProductionStage;
			grdDetail.RootTable.Columns["OrderNo"].Visible = OrderId > 0;
			grdDetail.RootTable.Columns["OrderDate"].Visible = OrderId > 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbPaymentTermHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			grdDetail.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosBranchFill();
			HistoryComboFill();
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
			GetAll();
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
			if (((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				HistoryComboFill();
				return;
			}
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((UltraGridBase)CmbCustomerHistory).DataSource = null;
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtGrid);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dthistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdPaymentTerm, dtPaymentTerm);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdInvExp, dtInvExp);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdGLedger, dtGrdGL);
			}
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
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1661);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlipDetail_Click(object sender, EventArgs e)
	{
		try
		{
			int SupCustId = Conversion.ToInt(cmbsuppliername.Value);
			DateTime Date = Conversion.ToDateTime((object)DocDate.Value);
			if (RecId > 0)
			{
				CommonServices.SaleInvoicePrint1661(RecId, SupCustId, 0, Date, Date);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratePrint1661A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 1661;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1661A_SaleInvoice_CustomerBillDirectSlip_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaleinvoice2_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GeneratePrint1661A(RecId);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				DueDate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				DueDate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDueDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0 && btnSave.Visible && btnSave.Enabled)
			{
				string ItemIds = "";
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					ItemIds = ItemIds + "," + r.Cells["ItemId"].Value;
				}
				DataTable dt = ChangedTaxPercentFromGridByDatechange(ItemIds);
				rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					double TaxPercent = 0.0;
					double ItemAmount = 0.0;
					double TaxAmount = 0.0;
					bool flag = false;
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						if (Conversion.ToInt(dt.Rows[i]["ItemId"]) == Conversion.ToInt(item.Cells["ItemId"].Value))
						{
							flag = true;
							item.Cells["TaxTypeId"].Value = Conversion.ToInt(dt.Rows[i]["TaxNameId"]);
							TaxPercent = Conversion.ToDouble(dt.Rows[i]["TaxPercent"]);
							item.Cells["Tax%"].Value = TaxPercent;
							ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
							Conversion.ToDouble(item.Cells["BillAmount"].Value);
							if (ItemAmount > 0.0 && TaxPercent > 0.0)
							{
								TaxAmount = ItemAmount * TaxPercent / 100.0;
								item.Cells["TaxAmount"].Value = TaxAmount;
							}
						}
					}
					if (!flag)
					{
						item.Cells["TaxTypeId"].Value = 0;
						item.Cells["Tax%"].Value = 0;
						item.Cells["TaxAmount"].Value = 0;
					}
					item.EndEdit();
				}
			}
			txtDueDays_TextChanged(null, null);
			cmbsuppliername_ValueChanged(null, null);
			BillAmount();
			AllTaxTypeDBCall();
			BindTaxType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ChangedTaxPercentFromGridByDatechange(string ItemIds)
	{
		DataTable dt = new DataTable();
		try
		{
			dt = CommonServices.GetTaxScheduleDetailbyItemIds(ItemIds, DocDate.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dt;
	}

	private void DueDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (DueDate.Value.Date < DocDate.Value.Date)
			{
				DueDate.Value = DocDate.Value;
			}
			int days = (int)(DueDate.Value.Date - DocDate.Value.Date).TotalDays;
			txtDueDays.Text = days.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbPaymentTerm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbPaymentTerm).Text == "Cash")
			{
				txtDueDays.Enabled = false;
				txtDueDays.Text = string.Empty;
				DueDate.Value = DocDate.Value;
			}
			else
			{
				txtDueDays.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscPercent_KeyPress(object sender, KeyPressEventArgs e)
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
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrossWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddLss_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtratecut_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDiscountAmountHeader_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			btnRecordsUpdate.Visible = false;
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.F)
			{
				btnRecordsUpdate.Visible = true;
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						btnSave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnFrmRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbsuppliername.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grd).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						((Control)(object)grdPaymentTerm).Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						((Control)(object)grdInvExp).Focus();
					}
					else if (tabControl2.SelectedIndex == 3)
					{
						((Control)(object)grdGLedger).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPaymentTerm).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdInvExp).Focus();
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdGLedger).Focus();
					}
					else if (tabControl2.SelectedIndex == 3)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grd).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbWarehouse.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					btnSlipDetail_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSlipDetail_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btnSaleinvoice2_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3))
				{
					btnPrint_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdDetail).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdDetail).Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip 1661");
			dt.Rows.Add("Alt+2", "For Print Slip 1661A");
			dt.Rows.Add("Alt+3", "For Print Voucher 103");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus Grids");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Customer Name in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Add")
				{
					DataRow dr = dtGrid.Rows[item.RowIndex];
					dtGrid.Rows.Add(dr.ItemArray);
					grd.DataSource = dtGrid;
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						dtGrid.Rows[dtGrid.Rows.Count - 1]["Id"] = 0;
					}
				}
				if (grd.CurrentColumn.Key == "Delete" && item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				if (grd.GetRows().Count() == 0)
				{
					DetailPanel.Show();
				}
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				DataRow dr2 = dtGrid.Rows[item.RowIndex];
				dtGrid.Rows.Add(dr2.ItemArray);
				grd.DataSource = dtGrid;
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					dtGrid.Rows[dtGrid.Rows.Count - 1]["Id"] = 0;
				}
				FreightProportion();
				txtExchangeRate_TextChanged(null, null);
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					item.Delete();
					if (grd.GetRows().Count() == 0)
					{
						DetailPanel.Show();
					}
					FreightProportion();
					txtExchangeRate_TextChanged(null, null);
					BillAmount();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (FormRights.DoHaveUpdateRights)
					{
						RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
						ReadById(RecId);
					}
				}
				else if (grdHistory.CurrentColumn.Key == "Print-1661")
				{
					int SupCustId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(item.Cells["DocDate"].Value);
					CommonServices.SaleInvoicePrint1661(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()), SupCustId, 0, Date, Date);
				}
				else if (grdHistory.CurrentColumn.Key == "Print-1661A")
				{
					GeneratePrint1661A(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
				}
				else if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(item.Cells["VoucherHeadId"].Value), 1661);
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value), 1661);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (FormRights.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && FormRights.DoHavePrintRights)
			{
				int SupCustId2 = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
				DateTime Date2 = Conversion.ToDateTime(item.Cells["DocDate"].Value);
				CommonServices.SaleInvoicePrint1661(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()), SupCustId2, 0, Date2, Date2);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineVariant_Click(object sender, EventArgs e)
	{
		try
		{
			ItemAttributeVarient_Engr itemAttributeVarient_Engr = new ItemAttributeVarient_Engr(UserAccount);
			itemAttributeVarient_Engr.CmbItem.Value = Conversion.ToInt(CmbItem.Value);
			itemAttributeVarient_Engr.Show();
			bindvarientunit(Conversion.ToInt(CmbItem.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCity_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineCity(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineItem_Click(object sender, EventArgs e)
	{
		try
		{
			new frmAddItem_Engr(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateInGridDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetDataRows().Count() > 0)
			{
				if (((UltraGridBase)CmbWarehouseForUpdateInGrid).ActiveRow == null || Conversion.ToInt(CmbWarehouseForUpdateInGrid.Value) == 0)
				{
					GridEXRow[] GridRows = grd.GetDataRows();
					if (Conversion.ToInt(GridRows[0].Cells["WarehouseId"].Value) == 0)
					{
						throw new Exception("Warehouse Field is Empty in Detail Grid Row No: 01");
					}
					Conversion.ToInt(GridRows[0].Cells["WarehouseId"].Value);
				}
				else if (((UltraGridBase)CmbWarehouseForUpdateInGrid).ActiveRow != null && Conversion.ToInt(CmbWarehouseForUpdateInGrid.Value) != 0)
				{
					int ContrId = Conversion.ToInt(CmbWarehouseForUpdateInGrid.Value);
					GridEXRow[] dataRows = grd.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WarehouseId"].Value = ContrId;
						obj.EndEdit();
					}
				}
				return;
			}
			throw new Exception("Detail Grid Record not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRecordsUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = InvSaleInvoice.GetSaleInvoiceIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, 1661);
			if (dtIds != null && dtIds.Rows.Count > 0)
			{
				bool ShouldStop = false;
				for (int i = 0; i < dtIds.Rows.Count; i++)
				{
					int RecId = Conversion.ToInt(dtIds.Rows[i]["Id"]);
					ReadById(RecId);
					ShouldStop = Insert(IsAutoUpdate: true, RecId);
					if (!ShouldStop)
					{
						break;
					}
				}
				if (ShouldStop)
				{
					MessageBox.Show("Records Update Successfully");
				}
				btnRecordsUpdate.Visible = false;
				return;
			}
			btnRecordsUpdate.Visible = false;
			throw new Exception("Ids Not Found For Update");
		}
		catch (Exception ex)
		{
			btnRecordsUpdate.Visible = false;
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
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1c: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Expected O, but got Unknown
		//IL_0c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c48: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Expected O, but got Unknown
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Expected O, but got Unknown
		//IL_0cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d03: Expected O, but got Unknown
		//IL_0d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d19: Expected O, but got Unknown
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d71: Expected O, but got Unknown
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e00: Expected O, but got Unknown
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0b: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Expected O, but got Unknown
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef2: Expected O, but got Unknown
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Expected O, but got Unknown
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Expected O, but got Unknown
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f97: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_103d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1047: Expected O, but got Unknown
		//IL_10ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b5: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c0: Expected O, but got Unknown
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ec: Expected O, but got Unknown
		//IL_11c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d3: Expected O, but got Unknown
		//IL_11d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11de: Expected O, but got Unknown
		//IL_11df: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e9: Expected O, but got Unknown
		//IL_131e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1328: Expected O, but got Unknown
		//IL_1360: Unknown result type (might be due to invalid IL or missing references)
		//IL_136a: Expected O, but got Unknown
		//IL_1381: Unknown result type (might be due to invalid IL or missing references)
		//IL_138b: Expected O, but got Unknown
		//IL_13ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d8: Expected O, but got Unknown
		//IL_13e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ee: Expected O, but got Unknown
		//IL_1c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5e: Expected O, but got Unknown
		//IL_1c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c75: Expected O, but got Unknown
		//IL_1c82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8c: Expected O, but got Unknown
		//IL_2621: Unknown result type (might be due to invalid IL or missing references)
		//IL_262b: Expected O, but got Unknown
		//IL_2638: Unknown result type (might be due to invalid IL or missing references)
		//IL_2642: Expected O, but got Unknown
		//IL_264f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2659: Expected O, but got Unknown
		//IL_29c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_29cb: Expected O, but got Unknown
		//IL_29d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e2: Expected O, but got Unknown
		//IL_29ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f9: Expected O, but got Unknown
		//IL_2c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c99: Expected O, but got Unknown
		//IL_2ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb0: Expected O, but got Unknown
		//IL_2cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc7: Expected O, but got Unknown
		//IL_ecfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_ed07: Expected O, but got Unknown
		//IL_ed2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_ed35: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance38 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		GridEXLayout grdPaymentTerm_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
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
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
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
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
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
		Appearance appearance238 = new Appearance();
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
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance333 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance202 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance225 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Mfg.Sale.frmSaleInvoiceDirectEngr));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.DetailGrid = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.CmbWarehouseForUpdateInGrid = new UltraCombo();
		this.label51 = new System.Windows.Forms.Label();
		this.BtnUpdateInGridDetail = new System.Windows.Forms.Button();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPaymentDetail = new System.Windows.Forms.TabPage();
		this.panel28 = new System.Windows.Forms.Panel();
		this.panel29 = new System.Windows.Forms.Panel();
		this.grdPaymentTerm = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabExpenses = new System.Windows.Forms.TabPage();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdGLedger = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.DetailPanel = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbProductionStage = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbSampleModal = new UltraCombo();
		this.label68 = new System.Windows.Forms.Label();
		this.CmbCastingTypeDetail = new UltraCombo();
		this.label77 = new System.Windows.Forms.Label();
		this.BtnDefineVariant = new System.Windows.Forms.Button();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.label65 = new System.Windows.Forms.Label();
		this.CmbCategory = new UltraCombo();
		this.label64 = new System.Windows.Forms.Label();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.label39 = new System.Windows.Forms.Label();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.CmbVariantDescription = new UltraCombo();
		this.label76 = new System.Windows.Forms.Label();
		this.txtDiscountAmount = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.txtDiscPercent = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.CmbDiscountType = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbTaxType = new UltraCombo();
		this.lblTotalAmount = new System.Windows.Forms.Label();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.lblTaxPercent = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.lblTaxTypes = new System.Windows.Forms.Label();
		this.lblTaxAmount = new System.Windows.Forms.Label();
		this.CmbCity = new UltraCombo();
		this.label53 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.lblBalance = new System.Windows.Forms.TextBox();
		this.txtNetRate = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.txtAddLessRate = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.CmbWarehouse = new UltraCombo();
		this.CmbRateUom = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.CmbItem = new UltraCombo();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.label42 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtAddLss = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.txtNetBillWeight = new System.Windows.Forms.TextBox();
		this.CmbJobLot = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbFreightAcDr = new UltraCombo();
		this.CmbDiscountAccount = new UltraCombo();
		this.txtDiscountAmountHeader = new System.Windows.Forms.TextBox();
		this.txtFreightRemarks = new System.Windows.Forms.TextBox();
		this.txtFreightAmount = new System.Windows.Forms.TextBox();
		this.txtLedgerBalance = new System.Windows.Forms.TextBox();
		this.txtLedgerBalanceAfterBill = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.CmbFreightAcCr = new UltraCombo();
		this.txtCustomerCellNo = new System.Windows.Forms.TextBox();
		this.txtCustomerCity = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtRefrenceNo = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbsuppliername = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.CmbTaxAccount = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.CmbPaymentTerm = new UltraCombo();
		this.cmbCurrency = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtcommremarks = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbcommuom = new System.Windows.Forms.ComboBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.cmbcommtype = new System.Windows.Forms.ComboBox();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbstockprty = new UltraCombo();
		this.cmbcommagent = new UltraCombo();
		this.cmbrefprty = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkVoucherPreview = new System.Windows.Forms.CheckBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnFrmRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSaleInvoice1 = new System.Windows.Forms.ToolStripButton();
		this.btnSaleinvoice2 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnDefineItem = new System.Windows.Forms.ToolStripButton();
		this.btnCity = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.label66 = new System.Windows.Forms.Label();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbPaymentTermHistory = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label54 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.DetailGrid.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseForUpdateInGrid).BeginInit();
		this.tabPaymentDetail.SuspendLayout();
		this.panel28.SuspendLayout();
		this.panel29.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).BeginInit();
		this.panel17.SuspendLayout();
		this.tabExpenses.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel21.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel18.SuspendLayout();
		this.DetailPanel.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleModal).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingTypeDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariantDescription).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFreightAcDr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAcCr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrefprty).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1141, 611);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1141, 611);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 8;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel5);
		this.tabForm.Controls.Add(this.panel4);
		this.tabForm.Controls.Add(this.panel3);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1133, 582);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Controls.Add(this.DetailPanel);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 239);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1127, 340);
		this.panel5.TabIndex = 3;
		this.tabControl2.Controls.Add(this.DetailGrid);
		this.tabControl2.Controls.Add(this.tabPaymentDetail);
		this.tabControl2.Controls.Add(this.tabExpenses);
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 160);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1127, 180);
		this.tabControl2.TabIndex = 43;
		this.DetailGrid.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.DetailGrid.Controls.Add(this.panel7);
		this.DetailGrid.Location = new System.Drawing.Point(4, 25);
		this.DetailGrid.Name = "DetailGrid";
		this.DetailGrid.Padding = new System.Windows.Forms.Padding(3);
		this.DetailGrid.Size = new System.Drawing.Size(1119, 151);
		this.DetailGrid.TabIndex = 0;
		this.DetailGrid.Text = "Detail";
		this.DetailGrid.UseVisualStyleBackColor = true;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1113, 119);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid);
		this.panel7.Controls.Add(this.label51);
		this.panel7.Controls.Add(this.BtnUpdateInGridDetail);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1113, 26);
		this.panel7.TabIndex = 42;
		this.CmbWarehouseForUpdateInGrid.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbWarehouseForUpdateInGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseForUpdateInGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseForUpdateInGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseForUpdateInGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Location = new System.Drawing.Point(816, 0);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Name = "CmbWarehouseForUpdateInGrid";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).TabIndex = 160;
		this.label51.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label51.AutoSize = true;
		this.label51.BackColor = System.Drawing.Color.Teal;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(684, 4);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(130, 19);
		this.label51.TabIndex = 162;
		this.label51.Text = "WareHouse Name";
		this.BtnUpdateInGridDetail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateInGridDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateInGridDetail.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateInGridDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateInGridDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateInGridDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateInGridDetail.Location = new System.Drawing.Point(998, 1);
		this.BtnUpdateInGridDetail.Name = "BtnUpdateInGridDetail";
		this.BtnUpdateInGridDetail.Size = new System.Drawing.Size(72, 23);
		this.BtnUpdateInGridDetail.TabIndex = 161;
		this.BtnUpdateInGridDetail.Text = "Update";
		this.BtnUpdateInGridDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateInGridDetail.Click += new System.EventHandler(BtnUpdateInGridDetail_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1074, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPaymentDetail.Controls.Add(this.panel28);
		this.tabPaymentDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentDetail.Name = "tabPaymentDetail";
		this.tabPaymentDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabPaymentDetail.Size = new System.Drawing.Size(1119, 151);
		this.tabPaymentDetail.TabIndex = 1;
		this.tabPaymentDetail.Text = "Payment Detail";
		this.tabPaymentDetail.UseVisualStyleBackColor = true;
		this.panel28.Controls.Add(this.panel29);
		this.panel28.Controls.Add(this.panel17);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel28.Location = new System.Drawing.Point(3, 3);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1113, 145);
		this.panel28.TabIndex = 2;
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerm);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel29.Location = new System.Drawing.Point(0, 22);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1113, 123);
		this.panel29.TabIndex = 1;
		((ScrollableBase)this.grdPaymentTerm).BackColor = System.Drawing.Color.White;
		this.grdPaymentTerm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentTerm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerm.DesignTimeLayout = grdPaymentTerm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Name = "grdPaymentTerm";
		((ScrollableBase)this.grdPaymentTerm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Size = new System.Drawing.Size(1113, 123);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).TabIndex = 3;
		this.grdPaymentTerm.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTerm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTerm).VisualStyle = (VisualStyle)4;
		this.grdPaymentTerm.CellUpdated += new ColumnActionEventHandler(grdPaymentTerm_CellUpdated);
		this.grdPaymentTerm.UpdatingCell += new UpdatingCellEventHandler(grdPaymentTerm_UpdatingCell);
		this.grdPaymentTerm.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTerm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTerm_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ctrlGrdBar3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1113, 22);
		this.panel17.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1074, 1);
		this.ctrlGrdBar3.MyGrid = this.grdPaymentTerm;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 101;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.tabExpenses.Controls.Add(this.panel16);
		this.tabExpenses.Location = new System.Drawing.Point(4, 25);
		this.tabExpenses.Name = "tabExpenses";
		this.tabExpenses.Size = new System.Drawing.Size(1119, 151);
		this.tabExpenses.TabIndex = 2;
		this.tabExpenses.Text = "Other Item Expenses";
		this.tabExpenses.UseVisualStyleBackColor = true;
		this.panel16.Controls.Add(this.panel13);
		this.panel16.Controls.Add(this.panel21);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1119, 151);
		this.panel16.TabIndex = 1;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 22);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1119, 129);
		this.panel13.TabIndex = 2;
		this.grdInvExp.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1119, 129);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.ctrlGrdBar4);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1119, 22);
		this.panel21.TabIndex = 1;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1079, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdInvExp;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 21;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Size = new System.Drawing.Size(1119, 151);
		this.tabPage2.TabIndex = 3;
		this.tabPage2.Text = "Party Add/Less";
		this.tabPage2.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(1119, 126);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1119, 25);
		this.panel18.TabIndex = 1;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1079, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(6);
		this.ctrlGrdBar5.MyGrid = this.grdGLedger;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar5.TabIndex = 102;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.DetailPanel.Controls.Add(this.groupBox2);
		this.DetailPanel.Controls.Add(this.txtAddLss);
		this.DetailPanel.Controls.Add(this.label30);
		this.DetailPanel.Controls.Add(this.label36);
		this.DetailPanel.Controls.Add(this.txtNetBillWeight);
		this.DetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.DetailPanel.Controls.Add(this.label33);
		this.DetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.DetailPanel.Location = new System.Drawing.Point(0, 0);
		this.DetailPanel.Name = "DetailPanel";
		this.DetailPanel.Size = new System.Drawing.Size(1127, 160);
		this.DetailPanel.TabIndex = 40;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionStage);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSampleModal);
		this.groupBox2.Controls.Add(this.label68);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail);
		this.groupBox2.Controls.Add(this.label77);
		this.groupBox2.Controls.Add(this.BtnDefineVariant);
		this.groupBox2.Controls.Add(this.txtremarksdetail);
		this.groupBox2.Controls.Add(this.label65);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategory);
		this.groupBox2.Controls.Add(this.label64);
		this.groupBox2.Controls.Add(this.rdbtnItemCode);
		this.groupBox2.Controls.Add(this.label39);
		this.groupBox2.Controls.Add(this.rdbtnItemName);
		this.groupBox2.Controls.Add(this.txtGrossWeight);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVariantDescription);
		this.groupBox2.Controls.Add(this.label76);
		this.groupBox2.Controls.Add(this.txtDiscountAmount);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.txtDiscPercent);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountType);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.groupBox2.Controls.Add(this.lblTotalAmount);
		this.groupBox2.Controls.Add(this.txtTaxPercent);
		this.groupBox2.Controls.Add(this.txtTotalAmount);
		this.groupBox2.Controls.Add(this.lblTaxPercent);
		this.groupBox2.Controls.Add(this.txtTaxAmount);
		this.groupBox2.Controls.Add(this.lblTaxTypes);
		this.groupBox2.Controls.Add(this.lblTaxAmount);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.txtgatepassno);
		this.groupBox2.Controls.Add(this.btnAdd);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.lblBalance);
		this.groupBox2.Controls.Add(this.txtNetRate);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Controls.Add(this.txtAddLessRate);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.txtvehicleno);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox2.Controls.Add(this.txtgpdate);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.txtRate);
		this.groupBox2.Controls.Add(this.txtItemQty);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtAmount);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1118, 156);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.CmbProductionStage.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionStage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionStage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionStage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance210;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance216).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionStage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionStage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionStage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionStage).Location = new System.Drawing.Point(4, 81);
		((System.Windows.Forms.Control)(object)this.CmbProductionStage).Name = "CmbProductionStage";
		((System.Windows.Forms.Control)(object)this.CmbProductionStage).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionStage).TabIndex = 8;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(4, 62);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(99, 15);
		this.label16.TabIndex = 605;
		this.label16.Text = "Production Stage";
		this.CmbSampleModal.AllowNull = (DefaultableBoolean)1;
		this.CmbSampleModal.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSampleModal.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSampleModal.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance271).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance271).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance293).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance304;
		((AppearanceBase)appearance315).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance315).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance326).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance326).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance326).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance326).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance326;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSampleModal.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Location = new System.Drawing.Point(317, 33);
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Name = "CmbSampleModal";
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Leave += new System.EventHandler(CmbSampleModal_Leave);
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label68.Location = new System.Drawing.Point(317, 15);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(84, 15);
		this.label68.TabIndex = 603;
		this.label68.Text = "Modal Sample";
		this.CmbCastingTypeDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCastingTypeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCastingTypeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCastingTypeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance168).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCastingTypeDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Location = new System.Drawing.Point(961, 33);
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Name = "CmbCastingTypeDetail";
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Size = new System.Drawing.Size(154, 26);
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Leave += new System.EventHandler(CmbCastingTypeDetail_Leave);
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label77.Location = new System.Drawing.Point(960, 15);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(74, 15);
		this.label77.TabIndex = 160;
		this.label77.Text = "Casting Type";
		this.BtnDefineVariant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineVariant.Location = new System.Drawing.Point(931, 33);
		this.BtnDefineVariant.Name = "BtnDefineVariant";
		this.BtnDefineVariant.Size = new System.Drawing.Size(29, 26);
		this.BtnDefineVariant.TabIndex = 6;
		this.BtnDefineVariant.TabStop = false;
		this.BtnDefineVariant.UseVisualStyleBackColor = true;
		this.BtnDefineVariant.Click += new System.EventHandler(BtnDefineVariant_Click);
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksdetail.Location = new System.Drawing.Point(171, 83);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(178, 23);
		this.txtremarksdetail.TabIndex = 9;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(171, 63);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(132, 15);
		this.label65.TabIndex = 158;
		this.label65.Text = "Specification / Remarks";
		this.CmbCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance254;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Location = new System.Drawing.Point(165, 33);
		((System.Windows.Forms.Control)(object)this.CmbCategory).Name = "CmbCategory";
		((System.Windows.Forms.Control)(object)this.CmbCategory).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategory).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Leave += new System.EventHandler(CmbCategory_Leave);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(165, 15);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(82, 15);
		this.label64.TabIndex = 153;
		this.label64.Text = "Item Category";
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(501, 13);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 156;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(430, 64);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(91, 15);
		this.label39.TabIndex = 119;
		this.label39.Text = "Item FG Weight";
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(448, 13);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 155;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Enabled = false;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(430, 83);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(104, 23);
		this.txtGrossWeight.TabIndex = 11;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.CmbVariantDescription.AllowNull = (DefaultableBoolean)1;
		this.CmbVariantDescription.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVariantDescription.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVariantDescription.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance227;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance228).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance229).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance230).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVariantDescription.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Location = new System.Drawing.Point(771, 33);
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Name = "CmbVariantDescription";
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Leave += new System.EventHandler(CmbVariantDescription_Leave);
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(771, 15);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(105, 15);
		this.label76.TabIndex = 154;
		this.label76.Text = "Modal Description";
		this.txtDiscountAmount.BackColor = System.Drawing.Color.White;
		this.txtDiscountAmount.Enabled = false;
		this.txtDiscountAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscountAmount.Location = new System.Drawing.Point(1014, 83);
		this.txtDiscountAmount.Name = "txtDiscountAmount";
		this.txtDiscountAmount.Size = new System.Drawing.Size(101, 23);
		this.txtDiscountAmount.TabIndex = 18;
		this.txtDiscountAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(1013, 64);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(77, 15);
		this.label41.TabIndex = 142;
		this.label41.Text = "Disc Amount";
		this.txtDiscPercent.BackColor = System.Drawing.Color.White;
		this.txtDiscPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscPercent.Location = new System.Drawing.Point(945, 83);
		this.txtDiscPercent.Name = "txtDiscPercent";
		this.txtDiscPercent.Size = new System.Drawing.Size(68, 23);
		this.txtDiscPercent.TabIndex = 17;
		this.txtDiscPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscPercent.TextChanged += new System.EventHandler(txtDiscPercent_TextChanged);
		this.txtDiscPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDiscPercent_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(947, 64);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(43, 15);
		this.label40.TabIndex = 140;
		this.label40.Text = "Disc %";
		this.CmbDiscountType.AllowNull = (DefaultableBoolean)1;
		this.CmbDiscountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance278).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDiscountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Location = new System.Drawing.Point(855, 81);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Name = "CmbDiscountType";
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Size = new System.Drawing.Size(88, 26);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).TextChanged += new System.EventHandler(CmbDiscountType_TextChanged);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(856, 64);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(83, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Discount Type";
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(117, 126);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_Leave);
		this.lblTotalAmount.AutoSize = true;
		this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalAmount.Location = new System.Drawing.Point(356, 109);
		this.lblTotalAmount.Name = "lblTotalAmount";
		this.lblTotalAmount.Size = new System.Drawing.Size(80, 15);
		this.lblTotalAmount.TabIndex = 136;
		this.lblTotalAmount.Text = "Total Amount";
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(198, 128);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(62, 23);
		this.txtTaxPercent.TabIndex = 21;
		this.txtTaxPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercent.TextChanged += new System.EventHandler(txtTaxPercent_TextChanged);
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Enabled = false;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(356, 128);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(122, 23);
		this.txtTotalAmount.TabIndex = 23;
		this.txtTotalAmount.Text = "0";
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblTaxPercent.AutoSize = true;
		this.lblTaxPercent.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxPercent.Location = new System.Drawing.Point(200, 109);
		this.lblTaxPercent.Name = "lblTaxPercent";
		this.lblTaxPercent.Size = new System.Drawing.Size(38, 15);
		this.lblTaxPercent.TabIndex = 134;
		this.lblTaxPercent.Text = "Tax %";
		this.txtTaxAmount.BackColor = System.Drawing.Color.White;
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(261, 128);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(94, 23);
		this.txtTaxAmount.TabIndex = 22;
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxAmount.TextChanged += new System.EventHandler(txtTaxAmount_TextChanged);
		this.lblTaxTypes.AutoSize = true;
		this.lblTaxTypes.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxTypes.Location = new System.Drawing.Point(116, 109);
		this.lblTaxTypes.Name = "lblTaxTypes";
		this.lblTaxTypes.Size = new System.Drawing.Size(53, 15);
		this.lblTaxTypes.TabIndex = 133;
		this.lblTaxTypes.Text = "Tax Type";
		this.lblTaxAmount.AutoSize = true;
		this.lblTaxAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxAmount.Location = new System.Drawing.Point(259, 109);
		this.lblTaxAmount.Name = "lblTaxAmount";
		this.lblTaxAmount.Size = new System.Drawing.Size(72, 15);
		this.lblTaxAmount.TabIndex = 135;
		this.lblTaxAmount.Text = "Tax Amount";
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(776, 126);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 27;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(776, 109);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(108, 15);
		this.label53.TabIndex = 128;
		this.label53.Text = "Delivery City Name";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1053, 127);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(62, 25);
		this.btnCancelUpdateDetial.TabIndex = 30;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgatepassno.Location = new System.Drawing.Point(593, 128);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(58, 23);
		this.txtgatepassno.TabIndex = 25;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(988, 127);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(29, 25);
		this.btnAdd.TabIndex = 28;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(776, 64);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(52, 15);
		this.label50.TabIndex = 125;
		this.label50.Text = "Net Rate";
		this.lblBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalance.Location = new System.Drawing.Point(619, 35);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.ReadOnly = true;
		this.lblBalance.Size = new System.Drawing.Size(81, 23);
		this.lblBalance.TabIndex = 15;
		this.lblBalance.TabStop = false;
		this.lblBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetRate.Location = new System.Drawing.Point(776, 83);
		this.txtNetRate.Name = "txtNetRate";
		this.txtNetRate.ReadOnly = true;
		this.txtNetRate.Size = new System.Drawing.Size(77, 23);
		this.txtNetRate.TabIndex = 15;
		this.txtNetRate.TabStop = false;
		this.txtNetRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(693, 64);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(82, 15);
		this.label49.TabIndex = 124;
		this.label49.Text = "Add/Less Rate";
		this.txtAddLessRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLessRate.Location = new System.Drawing.Point(693, 83);
		this.txtAddLessRate.Name = "txtAddLessRate";
		this.txtAddLessRate.Size = new System.Drawing.Size(82, 23);
		this.txtAddLessRate.TabIndex = 14;
		this.txtAddLessRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessRate.TextChanged += new System.EventHandler(txtratecut_TextChanged);
		this.txtAddLessRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtratecut_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(653, 109);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 123;
		this.label48.Text = "Vehicle No";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(652, 128);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(123, 23);
		this.txtvehicleno.TabIndex = 26;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(596, 109);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(32, 15);
		this.label47.TabIndex = 121;
		this.label47.Text = "GP #";
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(4, 33);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 0;
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(614, 81);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(CmbRateUom_TextChanged);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(701, 33);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(68, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance236;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(418, 33);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(CmbItem_Leave);
		this.txtgpdate.CustomFormat = "dd-MMM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(481, 128);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(111, 23);
		this.txtgpdate.TabIndex = 24;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(535, 64);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(58, 15);
		this.label42.TabIndex = 120;
		this.label42.Text = "Item Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(535, 83);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(77, 23);
		this.txtRate.TabIndex = 12;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtRate.Leave += new System.EventHandler(txtRate_Leave);
		this.txtItemQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(351, 83);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.Size = new System.Drawing.Size(77, 23);
		this.txtItemQty.TabIndex = 10;
		this.txtItemQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQty.TextChanged += new System.EventHandler(txtItemQty_TextChanged);
		this.txtItemQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemQty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(351, 64);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 118;
		this.label38.Text = "Item Qty";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(480, 109);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(50, 15);
		this.label35.TabIndex = 117;
		this.label35.Text = "GP Date";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(418, 15);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(35, 15);
		this.label32.TabIndex = 115;
		this.label32.Text = "Item ";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(4, 15);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 15);
		this.label31.TabIndex = 112;
		this.label31.Text = "WareHouse";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(619, 15);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(56, 15);
		this.label29.TabIndex = 110;
		this.label29.Text = "Bal Stock";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(699, 15);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(35, 15);
		this.label10.TabIndex = 110;
		this.label10.Text = "UOM";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(615, 64);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(61, 15);
		this.label14.TabIndex = 109;
		this.label14.Text = "Rate UOM";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(4, 128);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(111, 23);
		this.txtAmount.TabIndex = 19;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.TextChanged += new System.EventHandler(txtAmount_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(4, 109);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(79, 15);
		this.label15.TabIndex = 105;
		this.label15.Text = "Item Amount";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(988, 127);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 25);
		this.btnUpdateDetail.TabIndex = 29;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtAddLss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLss.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLss.Location = new System.Drawing.Point(1429, 40);
		this.txtAddLss.Name = "txtAddLss";
		this.txtAddLss.Size = new System.Drawing.Size(62, 23);
		this.txtAddLss.TabIndex = 6;
		this.txtAddLss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLss.Visible = false;
		this.txtAddLss.TextChanged += new System.EventHandler(txtAddLss_TextChanged);
		this.txtAddLss.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLss_KeyPress);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(1431, 19);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(56, 15);
		this.label30.TabIndex = 111;
		this.label30.Text = "Add/Less";
		this.label30.Visible = false;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(1223, 22);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(87, 15);
		this.label36.TabIndex = 116;
		this.label36.Text = "Net Bill Weight";
		this.label36.Visible = false;
		this.txtNetBillWeight.BackColor = System.Drawing.Color.White;
		this.txtNetBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeight.Enabled = false;
		this.txtNetBillWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetBillWeight.Location = new System.Drawing.Point(1224, 43);
		this.txtNetBillWeight.Name = "txtNetBillWeight";
		this.txtNetBillWeight.ReadOnly = true;
		this.txtNetBillWeight.Size = new System.Drawing.Size(86, 23);
		this.txtNetBillWeight.TabIndex = 7;
		this.txtNetBillWeight.TabStop = false;
		this.txtNetBillWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetBillWeight.Visible = false;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(1310, 39);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Visible = false;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(1310, 20);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 114;
		this.label33.Text = "Job Lot";
		this.label33.Visible = false;
		this.panel4.Controls.Add(this.txtCommAgentId);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtcommremarks);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.cmbcommuom);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtcommrate);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtcommamount);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtSupplierGLId);
		this.panel4.Controls.Add(this.cmbcommtype);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbstockprty);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbrefprty);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1127, 184);
		this.panel4.TabIndex = 0;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1230, 93);
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(47, 22);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFreightAcDr);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountAccount);
		this.groupBox1.Controls.Add(this.txtDiscountAmountHeader);
		this.groupBox1.Controls.Add(this.txtFreightRemarks);
		this.groupBox1.Controls.Add(this.txtFreightAmount);
		this.groupBox1.Controls.Add(this.txtLedgerBalance);
		this.groupBox1.Controls.Add(this.txtLedgerBalanceAfterBill);
		this.groupBox1.Controls.Add(this.label63);
		this.groupBox1.Controls.Add(this.label67);
		this.groupBox1.Controls.Add(this.txtBranchSrNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFreightAcCr);
		this.groupBox1.Controls.Add(this.txtCustomerCellNo);
		this.groupBox1.Controls.Add(this.txtCustomerCity);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtRefrenceNo);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsuppliername);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.txtbillno);
		this.groupBox1.Controls.Add(this.label62);
		this.groupBox1.Controls.Add(this.label60);
		this.groupBox1.Controls.Add(this.label61);
		this.groupBox1.Controls.Add(this.label58);
		this.groupBox1.Controls.Add(this.label57);
		this.groupBox1.Controls.Add(this.label56);
		this.groupBox1.Controls.Add(this.label55);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.txtDueDays);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.DueDate);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtBillAmount);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1118, 177);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.cmbFreightAcDr.AllowNull = (DefaultableBoolean)1;
		this.cmbFreightAcDr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFreightAcDr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFreightAcDr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFreightAcDr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFreightAcDr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Location = new System.Drawing.Point(674, 13);
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Name = "cmbFreightAcDr";
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.cmbFreightAcDr).Leave += new System.EventHandler(cmbFreightAcDr_Leave);
		this.CmbDiscountAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDiscountAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance269;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance270;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance272).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance276).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance277).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance277).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance277).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance277).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance172).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDiscountAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Location = new System.Drawing.Point(674, 95);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Name = "CmbDiscountAccount";
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).TabIndex = 18;
		this.txtDiscountAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscountAmountHeader.Location = new System.Drawing.Point(674, 124);
		this.txtDiscountAmountHeader.Name = "txtDiscountAmountHeader";
		this.txtDiscountAmountHeader.ShortcutsEnabled = false;
		this.txtDiscountAmountHeader.Size = new System.Drawing.Size(202, 23);
		this.txtDiscountAmountHeader.TabIndex = 19;
		this.txtDiscountAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountAmountHeader.TextChanged += new System.EventHandler(txtDiscountAmountHeader_TextChanged);
		this.txtDiscountAmountHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDiscountAmountHeader_KeyPress);
		this.txtFreightRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightRemarks.Location = new System.Drawing.Point(674, 43);
		this.txtFreightRemarks.Name = "txtFreightRemarks";
		this.txtFreightRemarks.ShortcutsEnabled = false;
		this.txtFreightRemarks.Size = new System.Drawing.Size(202, 23);
		this.txtFreightRemarks.TabIndex = 16;
		this.txtFreightAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAmount.Location = new System.Drawing.Point(674, 70);
		this.txtFreightAmount.Name = "txtFreightAmount";
		this.txtFreightAmount.ShortcutsEnabled = false;
		this.txtFreightAmount.Size = new System.Drawing.Size(202, 23);
		this.txtFreightAmount.TabIndex = 17;
		this.txtFreightAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAmount.TextChanged += new System.EventHandler(txtFreightAmount_TextChanged);
		this.txtLedgerBalance.BackColor = System.Drawing.SystemColors.Menu;
		this.txtLedgerBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLedgerBalance.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtLedgerBalance.Location = new System.Drawing.Point(973, 15);
		this.txtLedgerBalance.Name = "txtLedgerBalance";
		this.txtLedgerBalance.ReadOnly = true;
		this.txtLedgerBalance.Size = new System.Drawing.Size(143, 24);
		this.txtLedgerBalance.TabIndex = 20;
		this.txtLedgerBalance.TabStop = false;
		this.txtLedgerBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLedgerBalanceAfterBill.BackColor = System.Drawing.SystemColors.Menu;
		this.txtLedgerBalanceAfterBill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLedgerBalanceAfterBill.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtLedgerBalanceAfterBill.Location = new System.Drawing.Point(973, 69);
		this.txtLedgerBalanceAfterBill.Name = "txtLedgerBalanceAfterBill";
		this.txtLedgerBalanceAfterBill.ReadOnly = true;
		this.txtLedgerBalanceAfterBill.Size = new System.Drawing.Size(143, 24);
		this.txtLedgerBalanceAfterBill.TabIndex = 22;
		this.txtLedgerBalanceAfterBill.TabStop = false;
		this.txtLedgerBalanceAfterBill.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(881, 74);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(96, 15);
		this.label63.TabIndex = 109;
		this.label63.Text = "After Bill Balance";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(881, 20);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(87, 15);
		this.label67.TabIndex = 110;
		this.label67.Text = "Ledger Balance";
		this.txtBranchSrNo.BackColor = System.Drawing.Color.White;
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtBranchSrNo.Location = new System.Drawing.Point(144, 15);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.ReadOnly = true;
		this.txtBranchSrNo.Size = new System.Drawing.Size(49, 22);
		this.txtBranchSrNo.TabIndex = 1;
		this.CmbFreightAcCr.AllowNull = (DefaultableBoolean)1;
		this.CmbFreightAcCr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFreightAcCr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFreightAcCr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFreightAcCr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFreightAcCr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Location = new System.Drawing.Point(388, 122);
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Name = "CmbFreightAcCr";
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbFreightAcCr).Leave += new System.EventHandler(CmbFreightAcCr_Leave);
		this.txtCustomerCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerCellNo.Enabled = false;
		this.txtCustomerCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerCellNo.Location = new System.Drawing.Point(83, 97);
		this.txtCustomerCellNo.Name = "txtCustomerCellNo";
		this.txtCustomerCellNo.Size = new System.Drawing.Size(218, 23);
		this.txtCustomerCellNo.TabIndex = 5;
		this.txtCustomerCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerCity.Enabled = false;
		this.txtCustomerCity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerCity.Location = new System.Drawing.Point(83, 70);
		this.txtCustomerCity.Name = "txtCustomerCity";
		this.txtCustomerCity.Size = new System.Drawing.Size(218, 23);
		this.txtCustomerCity.TabIndex = 4;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(194, 15);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(107, 22);
		this.DocDate.TabIndex = 2;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_Leave);
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.txtRefrenceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefrenceNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefrenceNo.Location = new System.Drawing.Point(83, 124);
		this.txtRefrenceNo.Name = "txtRefrenceNo";
		this.txtRefrenceNo.Size = new System.Drawing.Size(218, 23);
		this.txtRefrenceNo.TabIndex = 6;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocno.Location = new System.Drawing.Point(83, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(60, 22);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.cmbsuppliername.AllowNull = (DefaultableBoolean)1;
		this.cmbsuppliername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsuppliername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsuppliername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsuppliername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Location = new System.Drawing.Point(83, 41);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Name = "cmbsuppliername";
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).TabIndex = 3;
		this.cmbsuppliername.ValueChanged += new System.EventHandler(cmbsuppliername_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Leave += new System.EventHandler(cmbsuppliername_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 101);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(80, 15);
		this.label13.TabIndex = 105;
		this.label13.Text = "Customer Cell";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(388, 15);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(181, 23);
		this.txtbillno.TabIndex = 8;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(4, 74);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(81, 15);
		this.label62.TabIndex = 105;
		this.label62.Text = "Customer City";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(574, 101);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(93, 15);
		this.label60.TabIndex = 104;
		this.label60.Text = "Discount A/c Dr";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(574, 128);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(102, 15);
		this.label61.TabIndex = 103;
		this.label61.Text = "Discount Amount";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(574, 47);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(92, 15);
		this.label58.TabIndex = 100;
		this.label58.Text = "Freight Remarks";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(574, 74);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(91, 15);
		this.label57.TabIndex = 98;
		this.label57.Text = "Freight Amount";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(574, 19);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(82, 15);
		this.label56.TabIndex = 96;
		this.label56.Text = "Freight A/c Dr";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(303, 128);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(80, 15);
		this.label55.TabIndex = 92;
		this.label55.Text = "Freight A/c Cr";
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance309).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance309).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance309;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance310;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance311).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance311).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance311;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance316).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance316).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance317).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance317).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance317).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance317;
		((AppearanceBase)appearance318).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance319).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(83, 148);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 7;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 154);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(73, 15);
		this.label12.TabIndex = 30;
		this.label12.Text = "Tax Account";
		this.txtRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(388, 150);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(488, 23);
		this.txtRemarks.TabIndex = 14;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(881, 152);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(84, 19);
		this.label43.TabIndex = 69;
		this.label43.Text = "Fcy Amount";
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(388, 70);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(55, 23);
		this.txtDueDays.TabIndex = 10;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.txtDueDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDays_KeyPress);
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(973, 124);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(143, 23);
		this.txtExchangeRate.TabIndex = 24;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.DueDate.CustomFormat = "dd-MMM-yy";
		this.DueDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(444, 70);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(125, 22);
		this.DueDate.TabIndex = 11;
		this.DueDate.ValueChanged += new System.EventHandler(DueDate_ValueChanged);
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(973, 148);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(143, 26);
		this.txtFcyAmount.TabIndex = 25;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(303, 47);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "Payment Term";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(881, 128);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(78, 15);
		this.label44.TabIndex = 68;
		this.label44.Text = "Echange Rate";
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance334).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance335).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance335).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance335).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance335;
		((AppearanceBase)appearance336).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance336;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(388, 41);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TextChanged += new System.EventHandler(CmbPaymentTerm_TextChanged);
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance296).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance296).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance296;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance297;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance298).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance298).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance299;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance301;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance302).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance302;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance303).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance303).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance303).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance303;
		((AppearanceBase)appearance305).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(973, 95);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 23;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(303, 74);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(86, 15);
		this.label24.TabIndex = 45;
		this.label24.Text = "Due Days,Date";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(881, 101);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(55, 15);
		this.label45.TabIndex = 64;
		this.label45.Text = "Fcy Code";
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.White;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(4, 128);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(78, 15);
		this.label28.TabIndex = 44;
		this.label28.Text = "Reference No";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance321).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance322).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance322).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance322;
		((AppearanceBase)appearance323).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance323;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance324).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance324).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance324;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance325).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance325;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance327).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance328;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance329).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance330).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance330).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance330).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance330).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance330;
		((AppearanceBase)appearance331).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance332).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance332;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance333;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(388, 95);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 12;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(303, 101);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(80, 15);
		this.label5.TabIndex = 38;
		this.label5.Text = "Delivery Term";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(4, 19);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No,Date";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 47);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(58, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Customer";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(303, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(76, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Manual Bill #";
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Enabled = false;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(973, 41);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.Size = new System.Drawing.Size(143, 27);
		this.txtBillAmount.TabIndex = 21;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(303, 154);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 15);
		this.label4.TabIndex = 20;
		this.label4.Text = "Remarks";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(881, 44);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(89, 20);
		this.label3.TabIndex = 17;
		this.label3.Text = "Bill Amount";
		this.txtcommremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommremarks.Location = new System.Drawing.Point(1478, 69);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(61, 21);
		this.txtcommremarks.TabIndex = 18;
		this.txtcommremarks.TabStop = false;
		this.txtcommremarks.Visible = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(1400, 72);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(57, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label25.Visible = false;
		this.cmbcommuom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbcommuom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cmbcommuom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbcommuom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommuom.FormattingEnabled = true;
		this.cmbcommuom.Items.AddRange(new object[7] { "1", "5", "25", "40", "50", "60", "100" });
		this.cmbcommuom.Location = new System.Drawing.Point(1478, 41);
		this.cmbcommuom.Name = "cmbcommuom";
		this.cmbcommuom.Size = new System.Drawing.Size(61, 23);
		this.cmbcommuom.TabIndex = 14;
		this.cmbcommuom.TabStop = false;
		this.cmbcommuom.Visible = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1281, 45);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Comm Rate";
		this.label21.Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1158, 72);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.Location = new System.Drawing.Point(1347, 41);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(48, 22);
		this.txtcommrate.TabIndex = 13;
		this.txtcommrate.TabStop = false;
		this.txtcommrate.Visible = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1400, 45);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(76, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Comm UOM";
		this.label19.Visible = false;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(1347, 68);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(48, 22);
		this.txtcommamount.TabIndex = 15;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1158, 45);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(56, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Ref Party";
		this.label2.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1230, 116);
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(47, 22);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.cmbcommtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbcommtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cmbcommtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbcommtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommtype.FormattingEnabled = true;
		this.cmbcommtype.Items.AddRange(new object[3] { "Flat", "Percentage", "Comm Weight" });
		this.cmbcommtype.Location = new System.Drawing.Point(1478, 14);
		this.cmbcommtype.Name = "cmbcommtype";
		this.cmbcommtype.Size = new System.Drawing.Size(61, 23);
		this.cmbcommtype.TabIndex = 12;
		this.cmbcommtype.TabStop = false;
		this.cmbcommtype.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(1281, 72);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(49, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label26.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1400, 18);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(73, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.label11.Visible = false;
		this.cmbstockprty.AllowNull = (DefaultableBoolean)1;
		this.cmbstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance158;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Location = new System.Drawing.Point(1230, 67);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Name = "cmbstockprty";
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Size = new System.Drawing.Size(47, 24);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Visible = false;
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance283).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance283).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance284;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance285).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance286;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance289).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance290).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance290).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance290).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance291).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(1347, 13);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(48, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Visible = false;
		this.cmbrefprty.AllowNull = (DefaultableBoolean)1;
		this.cmbrefprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbrefprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbrefprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance265).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbrefprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Location = new System.Drawing.Point(1230, 40);
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Name = "cmbrefprty";
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Size = new System.Drawing.Size(47, 24);
		((System.Windows.Forms.Control)(object)this.cmbrefprty).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1281, 18);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.label20.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.btnRecordsUpdate);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkVoucherPreview);
		this.panel3.Controls.Add(this.ChkBok);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 31);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1127, 24);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(0, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(130, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "SALE INVOICE ";
		this.ChkVoucherPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkVoucherPreview.AutoSize = true;
		this.ChkVoucherPreview.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkVoucherPreview.ForeColor = System.Drawing.Color.White;
		this.ChkVoucherPreview.Location = new System.Drawing.Point(999, 3);
		this.ChkVoucherPreview.Margin = new System.Windows.Forms.Padding(2);
		this.ChkVoucherPreview.Name = "ChkVoucherPreview";
		this.ChkVoucherPreview.Size = new System.Drawing.Size(72, 19);
		this.ChkVoucherPreview.TabIndex = 18;
		this.ChkVoucherPreview.TabStop = false;
		this.ChkVoucherPreview.Text = "Voucher";
		this.ChkVoucherPreview.UseVisualStyleBackColor = true;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(1075, 3);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(46, 19);
		this.ChkBok.TabIndex = 17;
		this.ChkBok.TabStop = false;
		this.ChkBok.Text = "Slip";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1127, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[20]
		{
			this.toolStripSeparator3, this.btnNew, this.btnFrmRefresh, this.toolStripSeparator2, this.btnSave, this.btnDelete, this.btnUpdate, this.toolStripSeparator4, this.btnAttachment, this.toolStripSeparator5,
			this.btnSaleInvoice1, this.btnSaleinvoice2, this.btnPrint, this.toolStripSeparator6, this.toolStripButton3, this.toolStripSeparator7, this.BtnDefineItem, this.btnCity, this.btnshortcutkeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1127, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnFrmRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnFrmRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFrmRefresh.Name = "btnFrmRefresh";
		this.btnFrmRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnFrmRefresh.Text = "&Refresh";
		this.btnFrmRefresh.Click += new System.EventHandler(btnFrmRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnSaleInvoice1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSaleInvoice1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaleInvoice1.Name = "btnSaleInvoice1";
		this.btnSaleInvoice1.Size = new System.Drawing.Size(100, 24);
		this.btnSaleInvoice1.Text = "1661-Print";
		this.btnSaleInvoice1.Click += new System.EventHandler(btnSlipDetail_Click);
		this.btnSaleinvoice2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSaleinvoice2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaleinvoice2.Name = "btnSaleinvoice2";
		this.btnSaleinvoice2.Size = new System.Drawing.Size(109, 24);
		this.btnSaleinvoice2.Text = "1661A-Print";
		this.btnSaleinvoice2.Click += new System.EventHandler(btnSaleinvoice2_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(115, 24);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(103, 24);
		this.toolStripButton3.Text = "Load Order";
		this.toolStripButton3.Click += new System.EventHandler(toolStripButton3_Click_1);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
		this.BtnDefineItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDefineItem.Name = "BtnDefineItem";
		this.BtnDefineItem.Size = new System.Drawing.Size(87, 24);
		this.BtnDefineItem.Text = "Define Item";
		this.BtnDefineItem.Click += new System.EventHandler(BtnDefineItem_Click);
		this.btnCity.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCity.Name = "btnCity";
		this.btnCity.Size = new System.Drawing.Size(83, 24);
		this.btnCity.Text = "Define City";
		this.btnCity.Click += new System.EventHandler(btnCity_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabHistory.Controls.Add(this.panel10);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabHistory.Controls.Add(this.panel8);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1133, 582);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 138);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1127, 281);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label27);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel10.Location = new System.Drawing.Point(3, 419);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1127, 18);
		this.panel10.TabIndex = 2;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(5, 2);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(187, 14);
		this.label27.TabIndex = 25;
		this.label27.Text = "Detail Of Selected Above row";
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel14.Location = new System.Drawing.Point(3, 111);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1127, 27);
		this.panel14.TabIndex = 29;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1086, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(5, 2);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(171, 19);
		this.label9.TabIndex = 24;
		this.label9.Text = "Sale Invoice History";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 437);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1127, 142);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 3;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel8.Controls.Add(this.groupBox4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 31);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1127, 80);
		this.panel8.TabIndex = 0;
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox4.Controls.Add(this.rdapproveddate);
		this.groupBox4.Controls.Add(this.label66);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.btnShow);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory);
		this.groupBox4.Controls.Add(this.label52);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label18);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.label46);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.label22);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Controls.Add(this.label54);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(7, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(993, 74);
		this.groupBox4.TabIndex = 2;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(86, 15);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(251, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(887, 39);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 11;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(6, 21);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(79, 15);
		this.label66.TabIndex = 90;
		this.label66.Text = "Branch Name";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(887, 19);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(806, 19);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 8;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(806, 39);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 9;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(742, 44);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(57, 24);
		this.btnShow.TabIndex = 7;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.CmbPaymentTermHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Location = new System.Drawing.Point(591, 43);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Name = "CmbPaymentTermHistory";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Size = new System.Drawing.Size(149, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).TabIndex = 6;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(511, 49);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(84, 15);
		this.label52.TabIndex = 14;
		this.label52.Text = "Payment Term";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance241;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance247).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(591, 15);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(511, 21);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(58, 15);
		this.label18.TabIndex = 12;
		this.label18.Text = "Customer";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(416, 45);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(91, 23);
		this.ToDocNoHistory.TabIndex = 4;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(340, 49);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(61, 15);
		this.label46.TabIndex = 10;
		this.label46.Text = "To DocNo";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(416, 17);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(91, 23);
		this.FromDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(340, 21);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(76, 15);
		this.label22.TabIndex = 8;
		this.label22.Text = "From DocNo";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(212, 45);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(125, 22);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(86, 45);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(125, 22);
		this.FromDateHistory.TabIndex = 1;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(6, 49);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(79, 15);
		this.label54.TabIndex = 5;
		this.label54.Text = "From,To Date";
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1127, 28);
		this.panel6.TabIndex = 28;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.BtnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1127, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(55, 24);
		this.btnResetHistory.Text = "New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(70, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(toolStripButton4_Click);
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(129, 2);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(75, 22);
		this.btnRecordsUpdate.TabIndex = 44;
		this.btnRecordsUpdate.Text = "Records Update";
		this.btnRecordsUpdate.UseVisualStyleBackColor = false;
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnRecordsUpdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1141, 611);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmSaleInvoiceDirectEngr";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmSaleInvoiceDirectEngr_FormClosed);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.DetailGrid.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseForUpdateInGrid).EndInit();
		this.tabPaymentDetail.ResumeLayout(false);
		this.panel28.ResumeLayout(false);
		this.panel29.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).EndInit();
		this.panel17.ResumeLayout(false);
		this.tabExpenses.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel21.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel18.ResumeLayout(false);
		this.DetailPanel.ResumeLayout(false);
		this.DetailPanel.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleModal).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingTypeDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariantDescription).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFreightAcDr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAcCr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrefprty).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
