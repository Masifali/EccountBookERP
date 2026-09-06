using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Account;
using Architecture.BLL.Accounts;
using Architecture.BLL.ChequePrinting;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.ChequePrinting;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Special_Rights.Configurations;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition.VouchersWithTax;

public class PaymentVoucherNew : Form
{
	private bool AutoRemarksIncludeHeaderRemarks;

	private bool AutoRemarksIncludeChqDateAndNum;

	private bool AutoRemarksIncludeChqNumber;

	private bool AutoRemarksIncludePayeeTitle;

	private bool AutoRemarksIncludeDetailRemarks;

	private bool AfterCloup;

	private bool ChequePrintingEnable;

	private bool BranchFeature;

	private bool isMouseClick;

	private bool InventoryRelatedAccountsShowInVouchers;

	private bool PreventNegativeBalanceEntry;

	private bool DisplayWarningforNegativeBalance;

	private bool DisableBothNegativeBalanceRestrictions;

	private bool ChequeBookEnableStatus;

	private bool ChequeBookCompulsoryOnBpv;

	private bool ChequeBookSerialWiseOnBpv;

	private bool AutoRemarksForPaymentThroughBank;

	private bool shiftTabPressed;

	private bool MultiCurrencyFeatureVisibilty;

	private bool MultiCurrencyGridWorking;

	public bool UpdateMode;

	private bool SubAccountType;

	private bool IsBookingOffice;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool FormInitialized;

	private int BaseCurrency;

	private double BaseRate;

	private int SubsdiaryAccountDatasourceLength;

	private int CheqNoValidatingInUpdate;

	private int RecId;

	private int CheqId;

	private int ScreenId;

	private int updateDetailIndex = -1;

	public int DocumentTypeId;

	private string DocTypeIds = "1,2";

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int DefaultSrbAccountId;

	private int DefaultDiscountAccountId;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	public DataTable dtCheq = new DataTable();

	private DataTable dtCurrencies = new DataTable();

	private DataTable dtAccount = new DataTable();

	private DataTable dtSubsidory = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private DataTable dtPaymetnType = new DataTable();

	private DataTable dtChequeType = new DataTable();

	private DataTable dtPayeeTitle = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtVoucherDetailMultiCurrency = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private ToolStripButton btn;

	private ToolStripButton btnE;

	private ToolStripButton btns;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton bt;

	private ToolStripButton btnCopy;

	private ToolStripButton btnPaste;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip1;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage Form;

	private Panel ParentPanelOfMainDetail;

	private Panel panel4;

	private Panel panel3;

	private TabPage Cpv;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private new ToolStripButton Update;

	private ToolStripButton Save;

	private ToolStripButton Print;

	private ToolStripButton toolStripButton10;

	private Label PaymentVoucherLable;

	private Label label14;

	private GroupBox groupBox2;

	private Label label2;

	private Label label15;

	private DateTimePicker voucherdatetime;

	private Label label16;

	private GroupBox groupboxdetail;

	private TextBox txtamount;

	private Label label20;

	private Label lblRemarks;

	private Label label11;

	private Button Add;

	private Panel panel8;

	private Panel panel11;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbCreditAccount;

	private TextBox txtvoucherno;

	internal UltraCombo CmbAccountDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label1;

	private BackgroundWorker backgroundWorker1;

	private Timer timer1;

	private Label lblCheqNo;

	private Panel panel10;

	internal GridEX DataGridHistory;

	private Panel panel7;

	private Button button1;

	private Label label34;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label27;

	private Label label33;

	private ComboBox cmbdoctype;

	private CheckBox ChkPrint1;

	private Label label3;

	internal GridEX grdDetail;

	private Label txtbalance;

	private TextBox txtTotalAmount;

	private Label label9;

	private TextBox txtTaxAmount;

	private Label label8;

	private TextBox txtValue;

	private Label label4;

	internal UltraCombo CmbWithHoldingAc;

	private Label label12;

	private CheckBox ChkBoxWthHolding;

	internal UltraCombo CmbAgainstAc;

	private Label label13;

	private Label lblPayTitle;

	private DateTimePicker CheqDate;

	private Label lblCheqDate;

	internal UltraCombo CmbCheqNo;

	private TextBox txtTaxPercent;

	private Label label21;

	private Label lblJoblot;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbPaymentType;

	internal UltraCombo CmbVoucherType;

	private ToolStripButton btnDelete;

	private ToolStripButton btnRefresh;

	private TextBox txtremarks;

	private Label lblbalancedetail;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo CmbTaxType;

	private Label label23;

	private ToolStripButton btnPaymentByInvoiceVoucher;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private ToolStripButton btnPrintOld;

	private TabPage Bpv;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX BankVoucherHistoryGrid;

	internal GridEX GridBankDetail;

	private Panel panel14;

	internal UltraCombo CmbSubsidiaryAccount;

	private Label lblSubsidiaryaccount;

	private CheckBox ChkPrint2;

	private ToolStripButton btnShortCutKey;

	private TextBox txtExchangeRate;

	private Label lblExchangeRate;

	private Label lblFcyAmount;

	private TextBox txtFcyAmount;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

	internal UltraCombo CmbProjectId;

	private Label label5;

	private ToolStripButton BtnPaymentAndReceiptVoucherSlip;

	private ToolStripButton btnSpecialRights;

	private CheckBox ChkPrint3;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNewCpvHistory;

	private ToolStripButton btnRefreshCpvHistory;

	private ToolStripButton toolStripButton3;

	private Panel panel13;

	private Label label25;

	private Panel panel12;

	private GroupBox groupBox1;

	private Label label26;

	internal UltraCombo cmbAccountTitleCpv;

	private Button btnShowCpv;

	private Label label28;

	private Label label29;

	private Label label30;

	private Label label31;

	private Label label32;

	internal UltraCombo cmbApprovedStatusCpv;

	private TextBox txtToDocNoCpv;

	private TextBox txtFromDocNoCpv;

	private DateTimePicker ToDateCpvHistory;

	private DateTimePicker FromDateCpvHistory;

	private Panel panel2;

	private Panel panel9;

	private Panel panel19;

	private Panel panel18;

	private Label label35;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewHistoryBpv;

	private ToolStripButton btnRefreshHistoryBpv;

	private ToolStripButton btnLoadAllFromBrvTab;

	private Panel panel15;

	private Label label6;

	private Panel panel20;

	private Panel panel5;

	private GroupBox groupBox3;

	private Label label24;

	internal UltraCombo CmbAccountTitleBpvHistory;

	private Button btnShowHistoryBpv;

	private Label label18;

	private Label label17;

	private Label label7;

	private Label label10;

	private Label label19;

	internal UltraCombo cmbApproveStatusBpv;

	private TextBox txtToDocNoBpvHistory;

	private TextBox txtFromDocNoBpvHistory;

	private DateTimePicker ToDateBpvHistory;

	private DateTimePicker FromDateBpvHistory;

	private Panel panel16;

	private Panel panel22;

	private Panel panel21;

	private Label label22;

	private Label label38;

	private Label label37;

	private Label label36;

	private GroupBox VoucherInfoBoxBank;

	private Label lblUnApprovedVouchersBpv;

	private Label lblTotalVouchersBpv;

	private Label lblApprovedVouchersBpv;

	private GroupBox VouchersInfoBoxCash;

	private Label label42;

	private Label lblUnApprovedVouchers;

	private Label lblTotalVouchers;

	private Label label45;

	private Label lblApprovedVouchers;

	private Label label47;

	private Panel panel6;

	private Panel panel23;

	private Panel panel17;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private RadioButton rdapproveddate;

	private RadioButton rdbpvapprovdate;

	private RadioButton rdbpvmodifydate;

	private RadioButton rdbpvdocdate;

	private RadioButton rdbpventrydate;

	internal UltraCombo txtPayTitle;

	internal UltraCombo CmbFinancialInstrument;

	private Label label39;

	private Label labelChequeType;

	internal UltraCombo cmbChequeType;

	private CheckBox cbPrintOnSave;

	internal UltraCombo cmbBranchName;

	private Label label41;

	private TextBox txtremarksmain;

	private Label labelSubsidiaryBal;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel PaymentTypePanel;

	private Panel DebitAccountPanel;

	private Panel SubsidiaryAcPanel;

	private Panel JobLotPanel;

	private Panel RemarksPanel;

	private Panel DebitAmountPanel;

	private Panel FinancialInstrumentPanel;

	private Panel ChequeDatePanel;

	private Panel ChequeNoPanel;

	private Panel PayeeTitlePanel;

	private Panel ChequeTypePanel;

	private Panel CostCenterPanel;

	internal UltraCombo CmbCostCenter;

	private Label label40;

	private Panel BranchPanel;

	private Panel panel24;

	private CheckBox chkCustomAccounts;

	private Panel FcyAmountPanel;

	private TextBox txtFcyAmountDetail;

	private Label label43;

	private Panel ReferenceAccountPanel;

	internal UltraCombo CmbReferenceAccount;

	private Label label46;

	private TextBox txtSrbTaxAmount;

	private Label label44;

	private Label label48;

	private TextBox txtSrbTaxPercent;

	internal UltraCombo CmbSrbTaxAccount;

	private Label label49;

	private TextBox txtDiscountAmount;

	private Label label50;

	private Label label51;

	private TextBox txtDiscPercent;

	internal UltraCombo CmbDiscountAccount;

	private Label label52;

	private RadioButton rdBtnRefAcByCode;

	private RadioButton rdBtnRefAcByTitle;

	private RadioButton rdbtnAccountCode;

	private RadioButton rdbtnAccountName;

	internal UltraCombo cmbLocationType;

	private Label labelLocationType;

	private RadioButton radioButton1;

	private RadioButton RadExcluded;

	internal GridEX grdMultiCurrency;

	private ToolStripButton btnCreateUpdateMultiCurrency;

	private Panel panel25;

	private TextBox txtTcyExchangeRateDetail;

	private Label label53;

	private Panel panel26;

	internal UltraCombo cmbTcyCodeDetail;

	private Label label54;

	private TextBox txtBaseFcyExchangeRate;

	private Label lblBaseFcyExchangeRate;

	private TextBox txtBaseFcyCode;

	private Label lblBaseFcyCode;

	private Label lblCrAcGlCurrency;

	public PaymentVoucherNew(UserAccount puserAccount)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
		txtremarks.Enter += txtremarks_Enter;
		txtremarks.MouseClick += txtremarks_MouseClick;
	}

	private void AcfrmPaymentVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 625);
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			((Control)(object)txtPayTitle).Visible = true;
			((Control)(object)txtPayTitle).Enabled = true;
			ScreenId = CommonServices.GetScreenIdByName(base.Name);
			CurrencyFill();
			MultiCurrencyFeature();
			string ScreenName = Conversion.ToString(base.Tag);
			if (ScreenName == "frmCashPaymentVoucherTax")
			{
				DocumentTypeId = 1;
			}
			if (ScreenName == "frmBankPaymentVoucherTax")
			{
				DocumentTypeId = 2;
			}
			if (DocumentTypeId == 1)
			{
				lblCheqDate.Visible = false;
				lblCheqNo.Visible = false;
				lblPayTitle.Visible = false;
				CheqDate.Visible = false;
				((Control)(object)CmbCheqNo).Visible = false;
				((Control)(object)txtPayTitle).Visible = false;
				DocTypeIds = "1";
				PaymentVoucherLable.Text = "Cash Payment Voucher";
				ScreenName = "frmCashPaymentVoucherTax";
			}
			if (DocumentTypeId == 2)
			{
				PaymentVoucherLable.Text = "Bank PaymentVoucher";
				DocTypeIds = "2";
				ScreenName = "frmBankPaymentVoucherTax";
			}
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			Save.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			Update.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("InventoryRelatedAccountsShowInVouchers"));
			BranchFeature = CommonServices.GetERPFeatureById(17);
			if (BranchFeature)
			{
				BranchPanel.Visible = true;
			}
			else
			{
				BranchPanel.Visible = false;
			}
			lblbalancedetail.Visible = false;
			table.Columns.Add("PaymentType");
			table.Columns.Add("AccountCode");
			table.Columns.Add("AccountId", typeof(int));
			table.Columns.Add("AccountTitle");
			table.Columns.Add("GlCurrency");
			table.Columns.Add("SubsidiaryAccountId");
			table.Columns.Add("SubsidiaryAccount");
			table.Columns.Add("SubsidiaryAccountTypeId");
			table.Columns.Add("JobLotId");
			table.Columns.Add("Remarks");
			table.Columns.Add("TcyCodeId", typeof(int));
			table.Columns.Add("TcyCode");
			table.Columns.Add("TcyExchangeRate", typeof(double));
			table.Columns.Add("FcyAmount", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("ReferenceAccountId", typeof(int));
			table.Columns.Add("ReferenceAccount", typeof(string));
			table.Columns.Add("FinancialInstrumentId", typeof(int));
			table.Columns.Add("FinancialInstrument", typeof(string));
			table.Columns.Add("ChequeDate", typeof(DateTime));
			table.Columns.Add("ChequeId");
			table.Columns.Add("ChequeNo");
			table.Columns.Add("PayeeTitle");
			table.Columns.Add("ChequeTypeId");
			table.Columns.Add("BranchId", typeof(int));
			table.Columns.Add("BranchName");
			table.Columns.Add("CostCenterId", typeof(int));
			table.Columns.Add("TaxAmount", typeof(double));
			table.Columns.Add("GlCurrencyId", typeof(int));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			if (!MultiCurrencyGridWorking)
			{
				btnCreateUpdateMultiCurrency.Visible = false;
				((Control)(object)grdMultiCurrency).Visible = false;
				lblCrAcGlCurrency.Visible = false;
				lblBaseFcyCode.Visible = false;
				txtBaseFcyCode.Visible = false;
				lblBaseFcyExchangeRate.Visible = false;
				txtBaseFcyExchangeRate.Visible = false;
			}
			dtVoucherDetailMultiCurrency.Columns.Add("Id", typeof(int));
			dtVoucherDetailMultiCurrency.Columns.Add("LocalCurencyId", typeof(int));
			dtVoucherDetailMultiCurrency.Columns.Add("LocalCurrency", typeof(string));
			dtVoucherDetailMultiCurrency.Columns.Add("To", typeof(string));
			dtVoucherDetailMultiCurrency.Columns.Add("GlCurrencyId", typeof(int));
			dtVoucherDetailMultiCurrency.Columns.Add("GlCurrency", typeof(string));
			dtVoucherDetailMultiCurrency.Columns.Add("ExchangeRate", typeof(double));
			grdMultiCurrency.DataSource = dtVoucherDetailMultiCurrency;
			grdMultiCurrency.RetrieveStructure();
			GrdMultiCurrencySetting();
			if (dtAccount.Columns.Count == 0)
			{
				dtAccount.Columns.Add("Id", typeof(int));
				dtAccount.Columns.Add("AccountTitle");
				dtAccount.Columns.Add("AccountCode");
				dtAccount.Columns.Add("ParentAccountTitle");
				dtAccount.Columns.Add("AccountClass");
				dtAccount.Columns.Add("CurrencyId");
				dtAccount.Columns.Add("CurrencyCode");
			}
			if (dtJobLot.Columns.Count == 0)
			{
				dtJobLot.Columns.Add("Id", typeof(int));
				dtJobLot.Columns.Add("JobLotDescription");
			}
			dtChequeType.Columns.Add("Id");
			dtChequeType.Columns.Add("CheqType");
			DocumentTypeFill();
			BranchesFill();
			GetFinancialInstrumentTypes();
			CombProjectFill();
			VoucherNofill();
			PaymentTypeFill();
			ChequeTypeFill();
			combojoblotfill();
			AccountsComboBind();
			DefaultConfigurations();
			DetailAccountFill();
			BindReferenceAccounts();
			BindRevenueBoardAccounts();
			BindDiscountAccounts();
			BindTaxTypes();
			CostCenterFill();
			LocationType();
			lblbalancedetail.Visible = false;
			CmbVoucherType.Focus();
			Save.Visible = true;
			Update.Visible = false;
			cmbdoctype.SelectedIndex = 0;
			((UltraGridBase)CmbVoucherType).Rows[0].Activate();
			((UltraGridBase)CmbPaymentType).Rows[0].Activate();
			ChkPrint1.Checked = true;
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			cbPrintOnSave.Checked = false;
			if (DocTypeIds == "1")
			{
				tabControl1.TabPages.Remove(Bpv);
			}
			else if (DocTypeIds == "2")
			{
				tabControl1.TabPages.Remove(Cpv);
			}
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				GetSubsidoryAccounts();
				BindSubsidiaryAccount();
				labelSubsidiaryBal.Visible = true;
				lblSubsidiaryaccount.Visible = true;
				((Control)(object)CmbSubsidiaryAccount).Visible = true;
				SubsidiaryAcPanel.Visible = true;
			}
			else
			{
				labelSubsidiaryBal.Visible = false;
				lblSubsidiaryaccount.Visible = false;
				((Control)(object)CmbSubsidiaryAccount).Visible = false;
				SubsidiaryAcPanel.Visible = false;
				RemarksPanel.Size = new Size(446, 47);
				txtremarks.Size = new Size(444, 23);
			}
			ApplicationsAllocateToCompany obj = clsGlobalVariables.GetApplicationsAllocateToCompanyList.Find((ApplicationsAllocateToCompany row) => row.AppId == 5 && row.CompanyId == UserAccount.CompanyId);
			IsBookingOffice = obj != null && obj.IsActive;
			if (!IsBookingOffice)
			{
				CostCenterPanel.Visible = false;
			}
			if ((IsBookingOffice || BranchFeature) && Conversion.ToInt(CmbVoucherType.Value) == 2)
			{
				flowLayoutPanel1.Height = 144;
				groupboxdetail.Height = 162;
				ParentPanelOfMainDetail.Height = 359;
			}
			else
			{
				flowLayoutPanel1.Height = 96;
				groupboxdetail.Height = 113;
				ParentPanelOfMainDetail.Height = 312;
			}
			SpecialRightsImplement();
			StatusFillForCpv();
			ComboBindForCpvHistory();
			FromDateCpvHistory.Value = DateTime.Now;
			ToDateCpvHistory.Value = DateTime.Now;
			StatusFillForBpv();
			ComboBindForBpvHistory();
			FromDateBpvHistory.Value = DateTime.Now;
			ToDateBpvHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				if (DocumentTypeId == 2 && ChequeBookSerialWiseOnBpv && Conversion.ToInt(CmbFinancialInstrument.Value) == 1)
				{
					CheckingChequeNoSerialWise();
				}
				table.Rows.Add(Conversion.ToInt(CmbPaymentType.Value), ((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[2].Value, CmbAccountDetail.Value, ((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[1].Value, MultiCurrencyGridWorking ? Conversion.ToString(((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[6].Value) : "", (!SubsidiaryAccountAllownOnVouchers) ? ((object)0) : ((SubsdiaryAccountDatasourceLength > 0) ? CmbSubsidiaryAccount.Value : CmbAccountDetail.Value), (!SubsidiaryAccountAllownOnVouchers) ? "" : ((SubsdiaryAccountDatasourceLength > 0) ? ((Control)(object)CmbSubsidiaryAccount).Text : ((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[1].Value), (!((Control)(object)CmbSubsidiaryAccount).Visible) ? ((object)0) : ((SubsdiaryAccountDatasourceLength > 0) ? ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value : ((object)4)), CmbJobLot.Value, txtremarks.Text, cmbTcyCodeDetail.Value, ((Control)(object)cmbTcyCodeDetail).Text, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), Conversion.ToDouble((object)txtFcyAmountDetail.Text), txtamount.Text.Trim(), Conversion.ToInt(CmbReferenceAccount.Value), (Conversion.ToInt(CmbReferenceAccount.Value) > 0) ? Conversion.ToString(((UltraDropDownBase)CmbReferenceAccount).SelectedRow.Cells[1].Value) : "", Conversion.ToInt(CmbFinancialInstrument.Value), Conversion.ToString((object)((Control)(object)CmbFinancialInstrument).Text), Conversion.ToDateTime((object)CheqDate.Value).ToString("dd-MMM-yy"), CmbCheqNo.Value, ((Control)(object)CmbCheqNo).Text, ((Control)(object)txtPayTitle).Text, (Conversion.ToInt(cmbChequeType.Value) > 0) ? Conversion.ToInt(cmbChequeType.Value) : 0, (Conversion.ToInt(cmbBranchName.Value) > 0) ? Conversion.ToInt(cmbBranchName.Value) : 0, Conversion.ToString((object)((Control)(object)cmbBranchName).Text), (Conversion.ToInt(CmbCostCenter.Value) > 0) ? Conversion.ToInt(CmbCostCenter.Value) : 0, 0, MultiCurrencyGridWorking ? Conversion.ToInt(((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[5].Value) : 0);
				lblbalancedetail.Visible = false;
				if (Conversion.ToInt(CmbCheqNo.Value) > 0)
				{
					((Control)(object)CmbVoucherType).Enabled = false;
					((Control)(object)CmbCreditAccount).Enabled = false;
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
				CalculateTotalInformation();
				if (grd.RowCount > 0 && Conversion.ToInt(CmbAgainstAc.Value) == 0)
				{
					int AcId = Conversion.ToInt(grd.GetRow(0).Cells["AccountId"].Value);
					CmbAgainstAc.Value = AcId;
				}
				if (MultiCurrencyGridWorking)
				{
					CreateUpdateMultiCurrencyGridInfo();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CheckingChequeNoSerialWise()
	{
		try
		{
			if (dtCheq.Rows.Count <= 0)
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				int minValue = dtCheq.AsEnumerable().Min((DataRow row) => Conversion.ToInt((object)row.Field<string>("CheqNo")));
				if (Conversion.ToInt((object)((Control)(object)CmbCheqNo).Text) != Conversion.ToInt((object)minValue))
				{
					throw new Exception("Please Insert Cheque No In Detail Grid Serial Wise");
				}
				return;
			}
			List<int> distinctChequeNos = (from row in table.AsEnumerable()
				select Conversion.ToInt((object)row.Field<string>("ChequeNo"))).Distinct().ToList();
			if (distinctChequeNos.Count == 1)
			{
				int minValue2 = (from row in dtCheq.AsEnumerable()
					where Conversion.ToInt((object)row.Field<string>("CheqNo")) != distinctChequeNos[0]
					select row).CopyToDataTable().AsEnumerable().Min((DataRow row) => Conversion.ToInt((object)row.Field<string>("CheqNo")));
				if (Conversion.ToInt((object)((Control)(object)CmbCheqNo).Text) > Conversion.ToInt((object)minValue2))
				{
					throw new Exception("Please Insert Cheque No In Detail Grid Serial Wise");
				}
				return;
			}
			int max = 0;
			int min = distinctChequeNos.Min();
			max = distinctChequeNos.Max();
			if (Conversion.ToInt((object)((Control)(object)CmbCheqNo).Text) < max && updateDetailIndex < 0)
			{
				throw new Exception("Please Insert Cheque No In Detail Grid Serial Wise! You Inserting " + ((Control)(object)CmbCheqNo).Text + " ChequeNo And in Grid " + max + " ChequeNo Is Present");
			}
			EnumerableRowCollection<DataRow> dt = dtCheq.AsEnumerable().Where(delegate(DataRow row)
			{
				string text = row.Field<string>("CheqNo");
				if (string.IsNullOrWhiteSpace(text))
				{
					return true;
				}
				if (!int.TryParse(text, out var result))
				{
					return true;
				}
				int num = Conversion.ToInt((object)min);
				int num2 = Conversion.ToInt((object)max);
				return result < num || result > num2;
			});
			int minValue3 = (dt.Any() ? dt.CopyToDataTable() : dtCheq.Clone()).AsEnumerable().Min((DataRow row) => Conversion.ToInt((object)row.Field<string>("CheqNo")));
			if (Conversion.ToInt((object)((Control)(object)CmbCheqNo).Text) >= min && Conversion.ToInt((object)((Control)(object)CmbCheqNo).Text) < max && updateDetailIndex < 0)
			{
				throw new Exception("Please Insert Cheque No In Detail Grid Serial Wise");
			}
			if (Conversion.ToInt((object)((Control)(object)CmbCheqNo).Text) > minValue3)
			{
				throw new Exception("Please Insert Cheque No In Detail Grid Serial Wise");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Expected O, but got Unknown
		//IL_1bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd0: Expected O, but got Unknown
		//IL_1e6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e73: Expected O, but got Unknown
		//IL_1d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d42: Expected O, but got Unknown
		//IL_208b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2092: Expected O, but got Unknown
		//IL_1f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8e: Expected O, but got Unknown
		//IL_2200: Unknown result type (might be due to invalid IL or missing references)
		//IL_2207: Expected O, but got Unknown
		//IL_2444: Unknown result type (might be due to invalid IL or missing references)
		//IL_244b: Expected O, but got Unknown
		//IL_238a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2391: Expected O, but got Unknown
		//IL_1490: Unknown result type (might be due to invalid IL or missing references)
		//IL_149a: Expected O, but got Unknown
		try
		{
			int count = grd.GetRows().Count();
			if (count == 0)
			{
				throw new Exception("Grid Record not found");
			}
			if (!FormValidation())
			{
				return;
			}
			if (MultiCurrencyGridWorking)
			{
				if (grdMultiCurrency.RowCount == 0)
				{
					throw new Exception("Multi Currency Grid record not found");
				}
				ValidateMultiCurrencyBeforeSave();
			}
			VoucherHead vh = new VoucherHead();
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				vh.Id = RecId;
			}
			if (ChkBoxWthHolding.Checked)
			{
				if (((UltraGridBase)CmbTaxType).ActiveRow == null || Conversion.ToInt(CmbTaxType.Value) == 0)
				{
					MessageBox.Show("TaxType Field is Required");
					CmbTaxType.Focus();
					return;
				}
				if (Conversion.ToDouble((object)txtTaxPercent.Text) == 0.0)
				{
					MessageBox.Show("Tax Percent Field is Required");
					txtTaxPercent.Focus();
					return;
				}
				if (((UltraGridBase)CmbAgainstAc).ActiveRow == null || Conversion.ToInt(CmbAgainstAc.Value) == 0)
				{
					MessageBox.Show("Withholding Debit Account Field is Required");
					CmbAgainstAc.Focus();
					return;
				}
				if (((UltraGridBase)CmbWithHoldingAc).ActiveRow == null || Conversion.ToInt(CmbWithHoldingAc.Value) == 0)
				{
					MessageBox.Show("Withholding Credit Account Field is Required");
					CmbWithHoldingAc.Focus();
					return;
				}
			}
			if (Conversion.ToInt(CmbSrbTaxAccount.Value) == 0 && Conversion.ToDouble((object)txtSrbTaxAmount.Text) > 0.0)
			{
				MessageBox.Show("SRB Account Field is Required");
				CmbSrbTaxAccount.Focus();
				return;
			}
			if (Conversion.ToInt(CmbDiscountAccount.Value) == 0 && Conversion.ToDouble((object)txtDiscountAmount.Text) > 0.0)
			{
				MessageBox.Show("Discount Account Field is Required");
				CmbDiscountAccount.Focus();
				return;
			}
			if (Conversion.ToDouble((object)txtDiscPercent.Text) == 0.0 && Conversion.ToDouble((object)txtDiscountAmount.Text) > 0.0)
			{
				MessageBox.Show("Disc% Field is Required");
				txtDiscPercent.Focus();
				return;
			}
			GridEXRow[] rows2 = grd.GetRows();
			foreach (GridEXRow r2 in rows2)
			{
				if (Conversion.ToDouble(r2.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r2.Cells["AccountId"].Value) == 0)
				{
					MessageBox.Show("Please Select Account Title First");
					return;
				}
			}
			DataTable dtDetail = (DataTable)grd.DataSource;
			if (((ChkBoxWthHolding.Checked && Conversion.ToDouble((object)txtTaxAmount.Text) > 0.0) || (Conversion.ToInt(CmbDiscountAccount.Value) > 0 && Conversion.ToDouble((object)txtDiscountAmount.Text) > 0.0)) && (from row in dtDetail.AsEnumerable()
				select row.Field<int>("AccountId")).Distinct().Count() > 1)
			{
				List<string> appliedOptions = new List<string>();
				if (ChkBoxWthHolding.Checked)
				{
					appliedOptions.Add("Withholding Tax");
				}
				if (Conversion.ToInt(CmbSrbTaxAccount.Value) > 0)
				{
					appliedOptions.Add("SRB Tax");
				}
				if (Conversion.ToInt(CmbDiscountAccount.Value) > 0)
				{
					appliedOptions.Add("Discount");
				}
				string optionsText = string.Join(", ", appliedOptions);
				throw new Exception("There are multiple accounts in detail, so you can't apply: " + optionsText + ".\nYou can only apply these to a single account in detail.");
			}
			if (ChkBoxWthHolding.Checked && Conversion.ToInt(dtDetail.Rows[0]["AccountId"]) != Conversion.ToInt(CmbAgainstAc.Value) && MessageBox.Show("WHT Debit A/C not match with detail Debit A/C. Are You Sure To Proceed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			vh.BaseDocumentTypeId = 1;
			vh.Type = (VoucherType)0;
			vh.DocumentTypeId = Convert.ToInt32(CmbVoucherType.Value);
			vh.ProjectId = Conversion.ToInt((object)CmbProjectId.Value.ToString());
			vh.VoucherCode = Conversion.ToInt((object)txtvoucherno.Text.Trim());
			vh.VoucherDate = voucherdatetime.Value;
			vh.Remarks = txtremarks.Text.Trim();
			vh.RefAccountId = Conversion.ToInt(CmbCreditAccount.Value);
			vh.AgainstAccountId = Conversion.ToInt(CmbWithHoldingAc.Value);
			vh.RefDocNoId = Conversion.ToInt(CmbAgainstAc.Value);
			vh.Remarks = txtremarksmain.Text;
			vh.PayTitle = ((Control)(object)txtPayTitle).Text.Trim();
			if (ChkBoxWthHolding.Checked)
			{
				vh.IncludeWHT = true;
			}
			else
			{
				vh.IncludeWHT = false;
			}
			vh.BranchId = UserAccount.BranchesId;
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.EntryUser = UserAccount.ID;
			vh.ModifyUser = UserAccount.ID;
			vh.MultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
			vh.ExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			vh.FcAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
			vh.CustomAccounts = chkCustomAccounts.Checked;
			vh.InclusiveTax = Conversion.ToBool((object)radioButton1.Checked);
			vh.voucherDetailList = new List<VoucherDetail>();
			int LineId = 1;
			double VoucherAmt = 0.0;
			rows2 = grd.GetRows();
			foreach (GridEXRow r in rows2)
			{
				string RemarksAuto = "";
				string Remarks = "";
				VoucherDetail vd = new VoucherDetail();
				vd.SubsidiaryTypeId = Conversion.ToInt(r.Cells["SubsidiaryAccountTypeId"].Value);
				int SubsidiaryAccountId = Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
				if (SubsidiaryAccountId > 0)
				{
					vd.SupplierCustomerId = ((vd.SubsidiaryTypeId == 1) ? SubsidiaryAccountId : 0);
					vd.EmployeeId = ((vd.SubsidiaryTypeId == 2) ? SubsidiaryAccountId : 0);
					vd.SubsidiaryAccountId = SubsidiaryAccountId;
				}
				if (vh.DocumentTypeId == 2)
				{
					if (count == 1)
					{
						vh.CheqId = Conversion.ToInt(r.Cells["ChequeId"].Value);
						vh.ChequeNo = Conversion.ToString(r.Cells["ChequeNo"].Value);
						vh.ChequeDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
						vh.PayTitle = Conversion.ToString(r.Cells["PayeeTitle"].Value);
					}
					if (AutoRemarksForPaymentThroughBank)
					{
						RemarksAuto = ((AutoRemarksIncludeChqDateAndNum && AutoRemarksIncludePayeeTitle) ? (" CHEQUE DATE: " + Conversion.ToDateTime(r.Cells["ChequeDate"].Value).ToShortDateString() + " CHEQUE NO: " + Conversion.ToString(r.Cells["ChequeNo"].Value) + " PayTitle: " + Conversion.ToString(r.Cells["PayeeTitle"].Value) + " Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " PAID TO: " + Conversion.ToString(r.Cells["AccountTitle"].Value)) : (AutoRemarksIncludeChqDateAndNum ? (" CHEQUE DATE: " + Conversion.ToDateTime(r.Cells["ChequeDate"].Value).ToShortDateString() + " CHEQUE NO: " + Conversion.ToString(r.Cells["ChequeNo"].Value) + " Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " PAID TO: " + Conversion.ToString(r.Cells["AccountTitle"].Value)) : ((AutoRemarksIncludeChqNumber && AutoRemarksIncludePayeeTitle) ? (" CHEQUE NO: " + Conversion.ToString(r.Cells["ChequeNo"].Value) + " PayTitle: " + Conversion.ToString(r.Cells["PayeeTitle"].Value) + " Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " PAID TO: " + Conversion.ToString(r.Cells["AccountTitle"].Value)) : ((!AutoRemarksIncludePayeeTitle) ? (" Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " PAID TO: " + Conversion.ToString(r.Cells["AccountTitle"].Value)) : (" PayTitle: " + Conversion.ToString(r.Cells["PayeeTitle"].Value) + " Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " PAID TO: " + Conversion.ToString(r.Cells["AccountTitle"].Value))))));
						bool containsSubstring = new string[4] { "CHEQUE DATE", "CHEQUE NO", "PayTitle", "Credit Account" }.Any((string substring) => Conversion.ToString((object)vh.Remarks).Contains(substring));
						if (AutoRemarksIncludeHeaderRemarks && AutoRemarksIncludeDetailRemarks)
						{
							RemarksAuto = (containsSubstring ? "" : vh.Remarks) + " " + RemarksAuto + " " + Conversion.ToString(r.Cells["Remarks"].Value);
						}
						else if (AutoRemarksIncludeHeaderRemarks)
						{
							RemarksAuto = (containsSubstring ? "" : vh.Remarks) + " " + RemarksAuto;
						}
						else if (AutoRemarksIncludeDetailRemarks)
						{
							RemarksAuto = RemarksAuto + " " + Conversion.ToString(r.Cells["Remarks"].Value);
						}
						vd.Comments = RemarksAuto;
						vd.CommentsOtherLingo = Conversion.ToString(r.Cells["Remarks"].Value);
						if (count == 1)
						{
							vh.RemarksOtherLingo = ((vh.Remarks != "") ? vh.Remarks : "");
							vh.Remarks = RemarksAuto;
						}
					}
					else if (Conversion.ToString(r.Cells["ChequeNo"].Value) != string.Empty)
					{
						Remarks = "Cheq/Ref#: " + Conversion.ToString(r.Cells["ChequeNo"].Value) + ", " + Conversion.ToString(r.Cells["Remarks"].Value);
						vd.Comments = Remarks;
						vd.CommentsOtherLingo = Conversion.ToString(r.Cells["Remarks"].Value);
					}
					else
					{
						vd.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.CommentsOtherLingo = Conversion.ToString(r.Cells["Remarks"].Value);
					}
				}
				else
				{
					vd.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
				}
				if (Conversion.ToInt(r.Cells["PaymentType"].Value) == 1)
				{
					vd.AdvanceAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
				}
				vd.LineId = LineId;
				vd.SortNo = r.RowIndex + 1;
				vd.PaymentTypeId = Conversion.ToInt(r.Cells["PaymentType"].Value);
				vd.PaymentType = Conversion.ToString((object)r.Cells["PaymentType"].Text);
				vd.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
				vd.AccountTitle = Conversion.ToString((object)r.Cells["AccountTitle"].Text);
				vd.AgainstAccountId = Conversion.ToInt(CmbCreditAccount.Value);
				vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				vd.ReferenceAccountId = Conversion.ToInt(r.Cells["ReferenceAccountId"].Value);
				vd.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
				vd.DebitAmount = (radioButton1.Checked ? (Conversion.ToDouble(r.Cells["Amount"].Value) - vd.TaxAmount) : Conversion.ToDouble(r.Cells["Amount"].Value));
				VoucherAmt += vd.DebitAmount;
				vd.IsTaxable = "False";
				vd.InvoiceNoRefId = Conversion.ToInt(r.Cells["ChequeId"].Value);
				vd.InstrumentTypeId = Conversion.ToInt(r.Cells["FinancialInstrumentId"].Value);
				vd.CheqNoDetail = Conversion.ToString(r.Cells["ChequeNo"].Value);
				vd.DCheqDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
				vd.PayeeTitle = Conversion.ToString(r.Cells["PayeeTitle"].Value);
				vd.ChequeTypeId = Conversion.ToInt(r.Cells["ChequeTypeId"].Value);
				vd.BranchesId = (BranchFeature ? Conversion.ToInt(r.Cells["BranchId"].Value) : UserAccount.BranchesId);
				vd.DMultiCurrencyId = Conversion.ToInt(r.Cells["TcyCodeId"].Value);
				vd.DExchangeCurrencyRate = Conversion.ToDouble(r.Cells["TcyExchangeRate"].Value);
				vd.DCurrencyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
				vd.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
				if (MultiCurrencyGridWorking)
				{
					vd.ThirdCurrencyId = Conversion.ToInt(r.Cells["GlCurrencyId"].Value);
					if (vd.ThirdCurrencyId == 0)
					{
						throw new Exception("Currency Not set against selected account");
					}
					double exchangeRate = 0.0;
					exchangeRate = ((vd.DMultiCurrencyId != vd.ThirdCurrencyId) ? (from x in dtVoucherDetailMultiCurrency.AsEnumerable()
						where Conversion.ToInt(x["GlCurrencyId"]) == vd.ThirdCurrencyId
						select Conversion.ToDouble(x["ExchangeRate"])).FirstOrDefault() : vd.DExchangeCurrencyRate);
					if (exchangeRate <= 0.0)
					{
						throw new Exception("Exchange rate not found or invalid for selected currency.");
					}
					vd.ThirdCurrencyFcyExchangeRate = exchangeRate;
					vd.ThirdCurrencyAmount = vd.DebitAmount / vd.ThirdCurrencyFcyExchangeRate;
					vd.BaseFcyId = Conversion.ToInt(txtBaseFcyCode.Tag);
					if (vd.DMultiCurrencyId == vd.BaseFcyId)
					{
						vd.BaseFcyExchangeRate = vd.DExchangeCurrencyRate;
					}
					else
					{
						vd.BaseFcyExchangeRate = Conversion.ToDouble((object)txtBaseFcyExchangeRate.Text);
					}
					vd.BaseFcyAmount = vd.DebitAmount / vd.BaseFcyExchangeRate;
				}
				if (count == 1 && vh.Remarks.Trim() == string.Empty)
				{
					vh.RemarksOtherLingo = ((vh.Remarks.Trim() != "") ? vh.Remarks.Trim() : null);
					vh.Remarks = vd.Comments;
				}
				if (RecId > 0)
				{
					vd.ActionId = 2;
				}
				vd.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
				vh.voucherDetailList.Add(vd);
				LineId++;
				VoucherDetail vd2 = new VoucherDetail();
				if (vh.DocumentTypeId == 2)
				{
					if (AutoRemarksForPaymentThroughBank)
					{
						vd2.Comments = RemarksAuto;
						vd2.CommentsOtherLingo = Conversion.ToString(r.Cells["Remarks"].Value);
					}
					else if (vh.Remarks.Trim() != null && vh.Remarks.Trim() != string.Empty)
					{
						vd2.Comments = vh.Remarks.Trim();
						vd2.CommentsOtherLingo = vd.CommentsOtherLingo;
					}
					else
					{
						vd2.Comments = vd.Comments;
						vd2.CommentsOtherLingo = Conversion.ToString(r.Cells["Remarks"].Value);
					}
				}
				else if (vh.Remarks.Trim() != null && vh.Remarks.Trim() != string.Empty)
				{
					vd2.Comments = vh.Remarks.Trim();
				}
				else
				{
					vd2.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
				}
				if (Conversion.ToInt(r.Cells["PaymentType"].Value) == 1)
				{
					vd2.AdvanceAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
				}
				vd2.SubsidiaryAgainstTypeId = Conversion.ToInt(r.Cells["SubsidiaryAccountTypeId"].Value);
				if (SubsidiaryAccountId > 0)
				{
					vd2.SubsidiaryAgainstAccountId = SubsidiaryAccountId;
				}
				vd2.LineId = LineId;
				vd2.SortNo = r.RowIndex + 1;
				vd2.PaymentTypeId = Conversion.ToInt(r.Cells["PaymentType"].Value);
				vd2.PaymentType = Conversion.ToString((object)r.Cells["PaymentType"].Text);
				vd2.AccountId = Conversion.ToInt(CmbCreditAccount.Value);
				vd2.AccountTitle = Conversion.ToString((object)((Control)(object)CmbCreditAccount).Text);
				vd2.AgainstAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
				vd2.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				vd2.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
				vd2.CreditAmount = (radioButton1.Checked ? (Conversion.ToDouble(r.Cells["Amount"].Value) - vd2.TaxAmount) : Conversion.ToDouble(r.Cells["Amount"].Value));
				vd2.DebitAmount = 0.0;
				vd2.ReferenceAccountId = Conversion.ToInt(r.Cells["ReferenceAccountId"].Value);
				vd2.IsTaxable = "False";
				vd2.InvoiceNoRefId = Conversion.ToInt(r.Cells["ChequeId"].Value);
				vd2.InstrumentTypeId = Conversion.ToInt(r.Cells["FinancialInstrumentId"].Value);
				vd2.ChequeTypeId = Conversion.ToInt(r.Cells["ChequeTypeId"].Value);
				vd2.BranchesId = (BranchFeature ? Conversion.ToInt(r.Cells["BranchId"].Value) : UserAccount.BranchesId);
				vd2.CheqNoDetail = Conversion.ToString(r.Cells["ChequeNo"].Value);
				vd2.DCheqDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
				vd2.PayeeTitle = Conversion.ToString(r.Cells["PayeeTitle"].Value);
				vd2.DMultiCurrencyId = Conversion.ToInt(r.Cells["TcyCodeId"].Value);
				vd2.DExchangeCurrencyRate = Conversion.ToDouble(r.Cells["TcyExchangeRate"].Value);
				vd2.DCurrencyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
				vd2.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
				vd2.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
				if (MultiCurrencyGridWorking)
				{
					vd2.ThirdCurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[2].Value);
					double CreditRowExchangeRate = (from x in dtVoucherDetailMultiCurrency.AsEnumerable()
						where Conversion.ToInt(x["GlCurrencyId"]) == vd2.ThirdCurrencyId
						select Conversion.ToDouble(x["ExchangeRate"])).FirstOrDefault();
					if (CreditRowExchangeRate <= 0.0)
					{
						throw new Exception("Exchange rate not found or invalid for selected currency.");
					}
					vd2.ThirdCurrencyFcyExchangeRate = CreditRowExchangeRate;
					vd2.ThirdCurrencyAmount = vd2.CreditAmount / vd2.ThirdCurrencyFcyExchangeRate;
					vd2.BaseFcyId = Conversion.ToInt(txtBaseFcyCode.Tag);
					if (vd2.DMultiCurrencyId == vd2.BaseFcyId)
					{
						vd2.BaseFcyExchangeRate = vd2.DExchangeCurrencyRate;
					}
					else
					{
						vd2.BaseFcyExchangeRate = Conversion.ToDouble((object)txtBaseFcyExchangeRate.Text);
					}
					vd2.BaseFcyAmount = vd2.CreditAmount / vd2.BaseFcyExchangeRate;
				}
				vh.voucherDetailList.Add(vd2);
				LineId++;
			}
			if (vh.IncludeWHT)
			{
				VoucherDetail vd5 = new VoucherDetail();
				vd5.LineId = LineId + 1;
				vd5.AccountId = vh.RefDocNoId;
				vd5.AgainstAccountId = vh.AgainstAccountId;
				vd5.TaxTypeId = Conversion.ToInt(CmbTaxType.Value);
				vd5.IsTaxable = "True";
				vd5.Comments = "Withholding Tax deducted under " + ((Control)(object)CmbTaxType).Text + " at " + txtTaxPercent.Text.Trim() + "% rate.";
				vd5.TaxPrcnt = Conversion.ToDouble((object)txtTaxPercent.Text.Trim());
				vd5.TaxesTotalAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
				vd5.WhtHolding = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
				vd5.DebitAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
				double FcyAmount3 = ((vh.ExchangeCurrencyRate > 0.0) ? (vd5.DebitAmount / vh.ExchangeCurrencyRate) : 0.0);
				FcyAmount3 = (vd5.DCurrencyAmount = Math.Round(FcyAmount3, clsGlobalVariables.DefaultNoofDecimalPointsForAmount));
				vh.voucherDetailList.Add(vd5);
				VoucherDetail vd8 = new VoucherDetail();
				vd8.LineId = LineId + 1;
				vd8.AccountId = vh.AgainstAccountId;
				vd8.AgainstAccountId = vh.RefDocNoId;
				vd8.TaxTypeId = Conversion.ToInt(CmbTaxType.Value);
				vd8.IsTaxable = "True";
				vd8.Comments = vd5.Comments;
				vd8.TaxPrcnt = Conversion.ToDouble((object)txtTaxPercent.Text.Trim());
				vd8.TaxesTotalAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
				vd8.WhtHolding = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
				vd8.CreditAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
				vd8.DCurrencyAmount = FcyAmount3;
				vh.voucherDetailList.Add(vd8);
				VoucherAmt += vd8.CreditAmount;
			}
			if (Conversion.ToInt(CmbSrbTaxAccount.Value) > 0 && Conversion.ToDouble((object)txtSrbTaxAmount.Text) > 0.0)
			{
				VoucherDetail vd4 = new VoucherDetail();
				vd4.LineId = LineId + 1;
				vd4.AccountId = vh.voucherDetailList[0].AccountId;
				vd4.AgainstAccountId = Conversion.ToInt(CmbSrbTaxAccount.Value);
				vd4.TaxesTotalAmount = Conversion.ToDouble((object)txtSrbTaxAmount.Text);
				vd4.SBRTaxAmount = Conversion.ToDouble((object)txtSrbTaxAmount.Text);
				vd4.IsTaxable = "True";
				vd4.Comments = "SBR Tax deducted under " + ((Control)(object)CmbSrbTaxAccount).Text.ToString();
				vd4.DebitAmount = Conversion.ToDouble((object)txtSrbTaxAmount.Text.Trim());
				double FcyAmount2 = ((vh.ExchangeCurrencyRate > 0.0) ? (vd4.DebitAmount / vh.ExchangeCurrencyRate) : 0.0);
				FcyAmount2 = (vd4.DCurrencyAmount = Math.Round(FcyAmount2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount));
				vh.voucherDetailList.Add(vd4);
				VoucherDetail vd7 = new VoucherDetail();
				vd7.LineId = LineId + 1;
				vd7.AccountId = Conversion.ToInt(CmbSrbTaxAccount.Value);
				vd7.AgainstAccountId = vh.voucherDetailList[0].AccountId;
				vd7.TaxesTotalAmount = Conversion.ToDouble((object)txtSrbTaxAmount.Text);
				vd7.SBRTaxAmount = Conversion.ToDouble((object)txtSrbTaxAmount.Text);
				vd7.IsTaxable = "True";
				vd7.Comments = vd4.Comments;
				vd7.CreditAmount = Conversion.ToDouble((object)txtSrbTaxAmount.Text.Trim());
				vd7.DCurrencyAmount = FcyAmount2;
				vh.voucherDetailList.Add(vd7);
				VoucherAmt += vd7.CreditAmount;
			}
			if (Conversion.ToInt(CmbDiscountAccount.Value) > 0 && Conversion.ToDouble((object)txtDiscountAmount.Text) > 0.0)
			{
				VoucherDetail vd3 = new VoucherDetail();
				vd3.LineId = LineId + 1;
				vd3.AccountId = vh.voucherDetailList[0].AccountId;
				vd3.AgainstAccountId = Conversion.ToInt(CmbDiscountAccount.Value);
				vd3.DiscountPercent = Conversion.ToDouble((object)txtDiscPercent.Text.Trim());
				vd3.DiscountAmount = Conversion.ToDouble((object)txtDiscountAmount.Text.Trim());
				vd3.DebitAmount = Conversion.ToDouble((object)txtDiscountAmount.Text.Trim());
				double originalAmount = Conversion.ToDouble((object)txtValue.Text) + Conversion.ToDouble((object)txtDiscountAmount.Text);
				vd3.Comments = "Original payable amount was " + originalAmount + ".Discount of " + txtDiscountAmount.Text.ToString() + " agreed upon mutual discussion. Final payment made: " + txtValue.Text.ToString();
				double FcyAmount = ((vh.ExchangeCurrencyRate > 0.0) ? (vd3.DebitAmount / vh.ExchangeCurrencyRate) : 0.0);
				FcyAmount = (vd3.DCurrencyAmount = Math.Round(FcyAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount));
				vh.voucherDetailList.Add(vd3);
				VoucherDetail vd6 = new VoucherDetail();
				vd6.LineId = LineId + 1;
				vd6.AccountId = Conversion.ToInt(CmbDiscountAccount.Value);
				vd6.AgainstAccountId = vh.voucherDetailList[0].AccountId;
				vd6.DiscountPercent = Conversion.ToDouble((object)txtDiscPercent.Text.Trim());
				vd6.DiscountAmount = Conversion.ToDouble((object)txtDiscountAmount.Text.Trim());
				vd6.CreditAmount = Conversion.ToDouble((object)txtDiscountAmount.Text.Trim());
				vd6.DCurrencyAmount = FcyAmount;
				vd6.Comments = vd3.Comments;
				vh.voucherDetailList.Add(vd6);
				VoucherAmt += vd6.CreditAmount;
			}
			var rows = (from x in ((DataTable)grd.DataSource).AsEnumerable().Select((DataRow row, int index) => new
				{
					Row = row,
					RowIndex = index
				})
				where x.Row.Field<int>("CostCenterId") > 0
				select new
				{
					RowIndex = x.RowIndex + 1,
					Amount = x.Row.Field<double>("Amount"),
					CostCenterId = x.Row.Field<int>("CostCenterId")
				}).ToList();
			vh.costCenterDetailList = new List<voucherCostCenterDetail>();
			if (rows.Count > 0)
			{
				foreach (var item2 in rows)
				{
					voucherCostCenterDetail d2 = new voucherCostCenterDetail();
					d2.Id = 0;
					d2.SortNo = Conversion.ToInt((object)item2.RowIndex);
					d2.CostCenterId = Conversion.ToInt((object)item2.CostCenterId);
					d2.costPrcent = 100m;
					d2.costAmount = Conversion.ToDouble((object)item2.Amount);
					vh.costCenterDetailList.Add(d2);
				}
			}
			if (MultiCurrencyGridWorking)
			{
				rows2 = grdMultiCurrency.GetDataRows();
				foreach (GridEXRow item in rows2)
				{
					VoucherDetailMultiCurrency d = new VoucherDetailMultiCurrency();
					d.Id = Conversion.ToInt(item.Cells["Id"].Value);
					d.LocalCurrencyId = Conversion.ToInt(item.Cells["LocalCurencyId"].Value);
					d.GlCurrencyId = Conversion.ToInt(item.Cells["GlCurrencyId"].Value);
					d.ExchangeRate = Conversion.ToDouble(item.Cells["ExchangeRate"].Value);
					vh.VoucherDetailMultiCurrencyList.Add(d);
				}
			}
			vh.AttachmentsList = new List<DMSAttachments>();
			vh.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<VoucherHead>(vh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			vh.AttachmentsValues = AttachmentsValues;
			vh.CustomAttachmentsValues = CustomAttachmentsValues;
			if (!DisableBothNegativeBalanceRestrictions)
			{
				double ClosingBalance = GetAccountBalance(vh.RefAccountId);
				ClosingBalance = Math.Round(ClosingBalance);
				string Acc = ((DocumentTypeId == 2) ? "Bank" : "Cash");
				if (VoucherAmt > ClosingBalance)
				{
					if (PreventNegativeBalanceEntry)
					{
						MessageBox.Show("Debit Amount cannot be greater than " + Acc + " Balance.\n" + $"{Acc} Balance is {ClosingBalance} and Total Debit Amount is {VoucherAmt}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					if (DisplayWarningforNegativeBalance && MessageBox.Show($"Debit Amount cannot be greater than {Acc} Balance.\n{Acc} Balance is {ClosingBalance} and Total Debit Amount is {VoucherAmt}.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
					{
						return;
					}
				}
			}
			if (!VoucherHead.VoucherExistWithSameAmountInSameDate(vh))
			{
				return;
			}
			vh.VoucherAmount = Conversion.ToDouble((object)txtTotalAmount.Text);
			HashSet<int> uniqueIds = new HashSet<int>();
			for (int i = 0; i < vh.voucherDetailList.Count; i++)
			{
				if (uniqueIds.Add(Conversion.ToInt((object)vh.voucherDetailList[i].AccountId)) && vh.voucherDetailList[i].DebitAmount > 0.0 && vh.voucherDetailList[i].TaxesTotalAmount == 0.0 && vh.voucherDetailList[i].SBRTaxAmount == 0.0 && vh.voucherDetailList[i].DiscountAmount == 0.0)
				{
					int organizationId = UserAccount.OrganizationId;
					int ComId = UserAccount.CompanyId;
					int AcId = vh.voucherDetailList[i].AccountId;
					DateTime ToDate = Conversion.ToDateTime((object)voucherdatetime.Value);
					double GlBalanceAmount = VoucherHead.GetGLBalanceByAccountTypeIdClassIdAndAccountId(organizationId, ComId, AcId, ToDate, 3, 3);
					if (GlBalanceAmount > 0.0 && MessageBox.Show($"The account '{vh.voucherDetailList[i].AccountTitle}' already has a debit ledger balance with an amount of {GlBalanceAmount}. Do you want to add the payment again?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
					{
						return;
					}
				}
			}
			int success = VoucherHead.Save(vh);
			if (RecId == 0)
			{
				MessageBox.Show("Voucher Save Successfully...[" + vh.VoucherCode + "]");
			}
			else
			{
				MessageBox.Show("Voucher Update Successfully...[" + vh.VoucherCode + "]");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, DocumentTypeId, 0);
				}
			}
			Reset();
			RecId = 0;
			grd.DataSource = null;
			if (ChkPrint1.Checked)
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(success);
			}
			else if (ChkPrint2.Checked)
			{
				CommonServices.AcRptPaymentReceiptsVoucherSlip_102(success, DocumentTypeId);
			}
			else if (ChkPrint3.Checked)
			{
				CommonServices.PaymentAndReceiptVoucherSlip(success, vh.DocumentTypeId);
			}
			if (ChequePrintingEnable && cbPrintOnSave.Checked)
			{
				new Architecture.WinApp.ChequePrinting.ChequePrinting(UserAccount).ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Save_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Update_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool FormValidation()
	{
		if (Conversion.ToInt(CmbProjectId.Value) == 0)
		{
			MessageBox.Show("Cost Center Field is Required");
			CmbProjectId.Focus();
			return false;
		}
		if (((UltraGridBase)CmbVoucherType).ActiveRow == null)
		{
			MessageBox.Show("Voucher Type Field is Required");
			CmbVoucherType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLocationType).ActiveRow == null || Conversion.ToInt(cmbLocationType.Value) == 0)
		{
			MessageBox.Show("Location Type Field is Required");
			cmbLocationType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAccount).ActiveRow == null || Conversion.ToInt(CmbCreditAccount.Value) == 0)
		{
			MessageBox.Show("Credit Account Field is Required");
			CmbCreditAccount.Focus();
			return false;
		}
		if (MultiCurrencyGridWorking && Conversion.ToInt(CmbCreditAccount.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[2].Value) == 0)
		{
			MessageBox.Show("Credit Account Currency not set Please Check");
			CmbCreditAccount.Focus();
			return false;
		}
		if (MultiCurrencyFeatureVisibilty)
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbCurrency.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRate.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtFcyAmount.Text) == 0.0)
			{
				MessageBox.Show("Fcy Amount Field is Required");
				txtFcyAmount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				return false;
			}
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbPaymentType).ActiveRow == null)
		{
			MessageBox.Show("PaymentType Field is Required");
			CmbPaymentType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAccount).ActiveRow == null || Conversion.ToInt(CmbCreditAccount.Value) == 0)
		{
			MessageBox.Show("Credit Account Field is Required");
			CmbCreditAccount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAccountDetail).ActiveRow == null || Conversion.ToInt(CmbAccountDetail.Value) == 0)
		{
			MessageBox.Show("Debit Account Field is Required");
			CmbAccountDetail.Focus();
			return false;
		}
		if (MultiCurrencyGridWorking && Conversion.ToInt(CmbAccountDetail.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[5].Value) == 0)
		{
			MessageBox.Show("Debit Account Currency not set Please Check");
			CmbAccountDetail.Focus();
			return false;
		}
		if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountDatasourceLength > 0 && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow == null)
		{
			MessageBox.Show("Subsidiary Account Field is Required");
			CmbSubsidiaryAccount.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbVoucherType.Value) == 2 && ((UltraGridBase)CmbFinancialInstrument).ActiveRow == null)
		{
			MessageBox.Show("Financial Instrument Field is Required");
			CmbFinancialInstrument.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbVoucherType.Value) == 2 && Conversion.ToInt(CmbFinancialInstrument.Value) == 1 && ChequeBookCompulsoryOnBpv && ((UltraGridBase)CmbCheqNo).ActiveRow == null)
		{
			MessageBox.Show("Cheque_number Field is Required");
			CmbCheqNo.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbTcyCodeDetail.Value) == 0)
		{
			MessageBox.Show("Tcy Code Field is Required");
			cmbTcyCodeDetail.Focus();
			return false;
		}
		if (txtTcyExchangeRateDetail.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Tcy Exchange Rate Field is Required");
			txtTcyExchangeRateDetail.Focus();
			return false;
		}
		if (txtamount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtamount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount Field is Required");
			txtamount.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbVoucherType.Value) == 2 && Conversion.ToInt(CmbFinancialInstrument.Value) == 1 && (((UltraGridBase)cmbChequeType).ActiveRow == null || Conversion.ToInt(cmbChequeType.Value) == 0))
		{
			MessageBox.Show("Cheque Type Field is Required");
			cmbChequeType.Focus();
			return false;
		}
		if (MultiCurrencyGridWorking)
		{
			if (Conversion.ToInt(txtBaseFcyCode.Tag) == 0)
			{
				MessageBox.Show("Base Fcy Code not set. Please Contact with ERP Team!");
				txtBaseFcyExchangeRate.Focus();
				return false;
			}
			if (txtBaseFcyExchangeRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtBaseFcyExchangeRate.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Base Fcy Exchange Rate Field is Required");
				txtBaseFcyExchangeRate.Focus();
				return false;
			}
		}
		if (BranchFeature && (((UltraGridBase)cmbBranchName).ActiveRow == null || Conversion.ToInt(cmbBranchName.Value) == 0))
		{
			MessageBox.Show("BranchName Field is Required");
			cmbBranchName.Focus();
			return false;
		}
		return true;
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			MultiCurrencyGridWorking = CommonServices.GetERPFeatureById(6);
			MultiCurrencyFeatureVisibilty = true;
			if (MultiCurrencyFeatureVisibilty)
			{
				lblFcyCode.Visible = true;
				((Control)(object)cmbCurrency).Visible = true;
				lblExchangeRate.Visible = true;
				txtExchangeRate.Visible = true;
				lblFcyAmount.Visible = true;
				txtFcyAmount.Visible = true;
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				lblFcyCode.Visible = false;
				lblExchangeRate.Visible = false;
				lblFcyAmount.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SpecialRightsImplement()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			DataTable dt = SreenRightsUserWise.SpecialRightsByScreenIdAndUserId(new SreenRightsUserWise
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				ScreenId = ScreenId,
				UserId = UserAccount.ID
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 2)
				{
					ChkPrint1.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 8)
				{
					ChkPrint2.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
				if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 9)
				{
					ChkPrint3.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				((UltraGridBase)CmbCostCenter).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			DataTable dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFillForCpv()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Not Apporved");
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApprovedStatusCpv, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApprovedStatusCpv).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindForCpvHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherReports.LedgerByJobLotDropDownAndLists(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.OrganizationId,
				DocumentTypeIds = 1.ToString(),
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID
			});
			DataTable dtAccount = new DataTable();
			dtAccount.Columns.Add("Id", typeof(int));
			dtAccount.Columns.Add("AccountTitle");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Account Header")
					{
						dtAccount.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtAccount, cmbAccountTitleCpv, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFillForBpv()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Not Apporved");
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApproveStatusBpv, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApproveStatusBpv).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindForBpvHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherReports.LedgerByJobLotDropDownAndLists(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.OrganizationId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = 2.ToString()
			});
			DataTable dtAccount = new DataTable();
			dtAccount.Columns.Add("Id", typeof(int));
			dtAccount.Columns.Add("AccountTitle");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Account Header")
					{
						dtAccount.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtAccount, CmbAccountTitleBpvHistory, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbProjectId, "Id", "ProjectName", "Cost Center", false);
				((UltraGridBase)CmbProjectId).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyFill()
	{
		try
		{
			dtCurrencies = MultiCurrency.GetMultiCurrencywithExchangeRate(UserAccount.CompanyId);
			InfragisticsHelper.BindAndRetainSelection(cmbCurrency, dtCurrencies, "Id", "CurrencyCode", "Tcy Code", AllColumns: true, Conversion.ToInt(cmbCurrency.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbTcyCodeDetail, dtCurrencies, "Id", "CurrencyCode", "Tcy Code", AllColumns: true, Conversion.ToInt(cmbTcyCodeDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtCurrencies.Rows.Count <= 0 || !MultiCurrencyGridWorking)
			{
				return;
			}
			DataRow BaseFcyCurrencyRow = dtCurrencies.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<int?>("BaseCurrencyId") == 2);
			double lastExchangeRate = 0.0;
			string FcyCode = "";
			int FcyId = 0;
			if (BaseFcyCurrencyRow != null)
			{
				FcyCode = BaseFcyCurrencyRow["CurrencyCode"]?.ToString() ?? "";
				FcyId = Conversion.ToInt(BaseFcyCurrencyRow["Id"]);
				if (BaseFcyCurrencyRow["ExchangeRate"] != DBNull.Value)
				{
					lastExchangeRate = Conversion.ToDouble(BaseFcyCurrencyRow["ExchangeRate"]);
				}
				txtBaseFcyCode.Text = FcyCode;
				txtBaseFcyCode.Tag = FcyId.ToString();
				txtBaseFcyExchangeRate.Text = lastExchangeRate.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationType()
	{
		try
		{
			DataTable dt = VoucherHead.GetLocationType();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbLocationType, "Id", "Location", "Location Type", ZeroIndex: true);
				((UltraGridBase)cmbLocationType).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbJobLot.Value);
			List<JobLot> lst = jobLot.GetList(new JobLot
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Count <= 0)
			{
				return;
			}
			dtJobLot.Rows.Clear();
			for (int i = 0; i < lst.Count; i++)
			{
				dtJobLot.Rows.Add(lst[i].Id, lst[i].JobLotDescription);
			}
			if (dtJobLot.Rows.Count > 0)
			{
				DDL.BindDDL(dtJobLot, CmbJobLot, "Id", "JobLotDescription", "JobLot", ZeroIndex: true);
			}
			if (id > 0)
			{
				if (dtJobLot.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbJobLot.Value = id;
				}
				else
				{
					((Control)(object)CmbJobLot).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFill()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbCreditAccount.Value);
			if (Save.Visible && Save.Enabled)
			{
				((Control)(object)CmbCreditAccount).Text = string.Empty;
			}
			COAAllocation coa = new COAAllocation();
			coa.OrganizationId = UserAccount.OrganizationId;
			coa.CompanyId = UserAccount.CompanyId;
			((Control)(object)cmbBranchName).Visible = BranchFeature;
			label41.Visible = BranchFeature;
			if (Conversion.ToInt(CmbVoucherType.Value) == 1)
			{
				lblCheqDate.Visible = false;
				CheqDate.Visible = false;
				ChequeDatePanel.Visible = false;
				lblCheqNo.Visible = false;
				((Control)(object)CmbCheqNo).Visible = false;
				ChequeNoPanel.Visible = false;
				lblPayTitle.Visible = false;
				((Control)(object)txtPayTitle).Visible = false;
				PayeeTitlePanel.Visible = false;
				label39.Visible = false;
				((Control)(object)CmbFinancialInstrument).Visible = false;
				FinancialInstrumentPanel.Visible = false;
				labelChequeType.Visible = false;
				((Control)(object)cmbChequeType).Visible = false;
				ChequeTypePanel.Visible = false;
				coa.DocumentTypeId = 1;
			}
			else
			{
				label39.Visible = true;
				((Control)(object)CmbFinancialInstrument).Visible = true;
				FinancialInstrumentPanel.Visible = true;
				lblCheqDate.Visible = true;
				CheqDate.Visible = true;
				ChequeDatePanel.Visible = true;
				lblCheqNo.Visible = true;
				((Control)(object)CmbCheqNo).Visible = true;
				ChequeNoPanel.Visible = true;
				lblPayTitle.Visible = true;
				((Control)(object)txtPayTitle).Visible = true;
				PayeeTitlePanel.Visible = true;
				labelChequeType.Visible = true;
				((Control)(object)cmbChequeType).Visible = true;
				ChequeTypePanel.Visible = true;
				coa.DocumentTypeId = 2;
			}
			if ((IsBookingOffice || BranchFeature) && Conversion.ToInt(CmbVoucherType.Value) == 2)
			{
				flowLayoutPanel1.Height = 144;
				groupboxdetail.Height = 162;
				ParentPanelOfMainDetail.Height = 359;
			}
			else
			{
				flowLayoutPanel1.Height = 96;
				groupboxdetail.Height = 113;
				ParentPanelOfMainDetail.Height = 312;
			}
			DataTable dtCoa = COAAllocation.GetDetailAccountByDocumentTypeId(coa);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("CurrencyId", typeof(int));
			dt.Columns.Add("CurrencyCode");
			for (int i = 0; i < dtCoa.Rows.Count; i++)
			{
				dt.Rows.Add(dtCoa.Rows[i]["Id"], dtCoa.Rows[i]["AccountTitle"], dtCoa.Rows[i]["CurrencyId"], dtCoa.Rows[i]["CurrencyCode"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCreditAccount, "Id", "AccountTitle", "Credit Account", ZeroIndex: true);
				((UltraGridBase)CmbCreditAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				CommonServices.SetComboValue(CmbCreditAccount, "Id", Id, dt);
			}
			if (Save.Visible && Save.Enabled && Conversion.ToInt(CmbVoucherType.Value) == 1)
			{
				((UltraGridBase)CmbCreditAccount).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherNofill()
	{
		try
		{
			if (Save.Visible && Save.Enabled)
			{
				if (Conversion.ToInt(CmbVoucherType.Value) == 1)
				{
					DocumentTypeId = 1;
				}
				else
				{
					DocumentTypeId = 2;
				}
				txtvoucherno.Text = Conversion.ToString((object)CommonServices.GenerateVoucherCode(DocumentTypeId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetFinancialInstrumentTypes()
	{
		try
		{
			DataTable dt = VoucherHead.GetInstrumentTypes();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbFinancialInstrument, "Id", "InstrumentType", "Financial Instrument", ZeroIndex: false);
				((UltraGridBase)CmbFinancialInstrument).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsComboBind()
	{
		try
		{
			int AgainstId = Conversion.ToInt(CmbAgainstAc.Value);
			int WhtId = Conversion.ToInt(CmbWithHoldingAc.Value);
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, new int[3] { 2, 11, 15 });
			InfragisticsHelper.BindAndRetainSelection(CmbAgainstAc, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, AgainstId, insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbWithHoldingAc, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, WhtId, insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SetComboValue(UltraCombo combo, int id, DataTable dt)
	{
		try
		{
			if (id > 0 && dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
			{
				combo.Value = id;
			}
			else
			{
				((Control)(object)combo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DocumentTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			if (DocumentTypeId == 1)
			{
				dt.Rows.Add(1, "Cash Payment Voucher");
				lblCheqDate.Visible = false;
				CheqDate.Visible = false;
				ChequeDatePanel.Visible = false;
				lblCheqNo.Visible = false;
				((Control)(object)CmbCheqNo).Visible = false;
				ChequeNoPanel.Visible = false;
				lblPayTitle.Visible = false;
				((Control)(object)txtPayTitle).Visible = false;
				PayeeTitlePanel.Visible = false;
				label39.Visible = false;
				((Control)(object)CmbFinancialInstrument).Visible = false;
				FinancialInstrumentPanel.Visible = false;
				labelChequeType.Visible = false;
				((Control)(object)cmbChequeType).Visible = false;
				ChequeTypePanel.Visible = false;
			}
			if (DocumentTypeId == 2)
			{
				dt.Rows.Add(2, "Bank Payment Voucher");
				label39.Visible = true;
				((Control)(object)CmbFinancialInstrument).Visible = true;
				FinancialInstrumentPanel.Visible = true;
				lblCheqDate.Visible = true;
				CheqDate.Visible = true;
				ChequeDatePanel.Visible = true;
				lblCheqNo.Visible = true;
				((Control)(object)CmbCheqNo).Visible = true;
				ChequeNoPanel.Visible = true;
				lblPayTitle.Visible = true;
				((Control)(object)txtPayTitle).Visible = true;
				PayeeTitlePanel.Visible = true;
				labelChequeType.Visible = true;
				((Control)(object)cmbChequeType).Visible = true;
				ChequeTypePanel.Visible = true;
			}
			if (DocumentTypeId == 0)
			{
				dt.Rows.Add(1, "Cash Payment Voucher");
				dt.Rows.Add(2, "Bank Payment Voucher");
				lblCheqDate.Visible = false;
				CheqDate.Visible = false;
				ChequeDatePanel.Visible = false;
				lblCheqNo.Visible = false;
				((Control)(object)CmbCheqNo).Visible = false;
				ChequeNoPanel.Visible = false;
				lblPayTitle.Visible = false;
				((Control)(object)txtPayTitle).Visible = false;
				PayeeTitlePanel.Visible = false;
				label39.Visible = false;
				((Control)(object)CmbFinancialInstrument).Visible = false;
				FinancialInstrumentPanel.Visible = false;
				labelChequeType.Visible = false;
				((Control)(object)cmbChequeType).Visible = false;
				ChequeTypePanel.Visible = false;
			}
			DDL.BindDDL(dt, CmbVoucherType, "Id", "Name", "Voucher Type", ZeroIndex: false);
			((UltraGridBase)CmbVoucherType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbVoucherType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTypeFill()
	{
		try
		{
			dtPaymetnType = lookUp.GetDataPaymentTypeForPayments();
			if (dtPaymetnType.Rows.Count > 0)
			{
				DDL.BindDDL(dtPaymetnType, CmbPaymentType, "Id", "PaymentType", "PaymentType", ZeroIndex: false);
				((UltraGridBase)CmbPaymentType).DisplayLayout.Bands[0].Columns[1].Width = 100;
				((UltraGridBase)CmbPaymentType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChequeTypeFill()
	{
		try
		{
			dtChequeType.Rows.Clear();
			dtChequeType = ChequeType.GetChequeType();
			if (dtChequeType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtChequeType, cmbChequeType, "id", "CheqType", "ChequeType", false);
				((UltraGridBase)cmbChequeType).DisplayLayout.Bands[0].Columns[1].Width = 100;
				((UltraGridBase)cmbChequeType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindReferenceAccounts()
	{
		try
		{
			int Id = Conversion.ToInt(CmbReferenceAccount.Value);
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[5] { 3, 5, 6, 7, 8 });
			string displayField = (rdBtnRefAcByTitle.Checked ? "AccountTitle" : "AccountCode");
			string headerText = (rdBtnRefAcByTitle.Checked ? "Reference Account" : "Reference Ac Code");
			InfragisticsHelper.BindAndRetainSelection(CmbReferenceAccount, dt, "Id", displayField, headerText, AllColumns: true, Id, insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindRevenueBoardAccounts()
	{
		try
		{
			int Id = Conversion.ToInt(CmbSrbTaxAccount.Value);
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[2] { 6, 8 });
			InfragisticsHelper.BindAndRetainSelection(CmbSrbTaxAccount, dt, "Id", "AccountTitle", "SRB Account", AllColumns: true, Id, insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindDiscountAccounts()
	{
		try
		{
			int Id = Conversion.ToInt(CmbDiscountAccount.Value);
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[5] { 11, 13, 20, 21, 22 });
			InfragisticsHelper.BindAndRetainSelection(CmbDiscountAccount, dt, "Id", "AccountTitle", "Discount Account", AllColumns: true, Id, insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DetailAccountFill()
	{
		try
		{
			int accountId = Conversion.ToInt(CmbAccountDetail.Value);
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation x) => x.ConfigDescription == "ExpenseAccountAllowOnPaymentVoucher");
			HashSet<int> excludedTypeSet = new HashSet<int>((!(((obj == null) ? null : obj.ConfigKey?.ToString().ToLower()) == "true")) ? ((!InventoryRelatedAccountsShowInVouchers) ? new int[9] { 2, 4, 11, 12, 13, 14, 15, 20, 21 } : new int[8] { 2, 11, 12, 13, 14, 15, 20, 21 }) : ((!InventoryRelatedAccountsShowInVouchers) ? new int[4] { 2, 4, 12, 15 } : new int[2] { 2, 15 }));
			bool excludePlNote = !InventoryRelatedAccountsShowInVouchers;
			List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
			if (allAccounts == null)
			{
				return;
			}
			HashSet<int> seenIds = new HashSet<int>();
			dtAccount.Rows.Clear();
			foreach (getGlobalAllAccountsWithCustomGroup account in allAccounts)
			{
				if (!excludedTypeSet.Contains(account.AccountTypeId) && (!excludePlNote || account.PLNoteId != 2))
				{
					int id = account.ChartOfAccountId;
					if (seenIds.Add(id))
					{
						dtAccount.Rows.Add(id, account.AccountTitle, account.AccountCode, account.ParentAccountTitle, account.AccountClassName, account.CurrencyId, account.CurrencyCode);
					}
				}
			}
			string displayField = (rdbtnAccountName.Checked ? "AccountTitle" : "AccountCode");
			string headerText = (rdbtnAccountName.Checked ? "Account Title" : "Account Code");
			InfragisticsHelper.BindAndRetainSelection(CmbAccountDetail, dtAccount, "Id", displayField, headerText, AllColumns: true, accountId, insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtAccount.Rows.Count > 0)
			{
				((UltraGridBase)CmbAccountDetail).DisplayLayout.Bands[0].Columns[5].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool ShouldIncludeAccount(int typeId, bool config, bool inventoryFlag)
	{
		HashSet<int> excludedDefault = new HashSet<int>
		{
			2, 4, 10, 11, 12, 13, 14, 15, 19, 20,
			21
		};
		HashSet<int> excludedIfInventory = new HashSet<int> { 2, 15 };
		HashSet<int> excludedIfNoInventory = new HashSet<int> { 2, 4, 10, 12, 15 };
		if (config)
		{
			if (!inventoryFlag)
			{
				return !excludedIfNoInventory.Contains(typeId);
			}
			return !excludedIfInventory.Contains(typeId);
		}
		return !excludedDefault.Contains(typeId);
	}

	private void AccountCurrentBalance()
	{
		try
		{
			txtbalance.Text = "0";
			double ClosingBalance = GetAccountBalance(Conversion.ToInt(CmbCreditAccount.Value));
			txtbalance.Text = ClosingBalance.ToString("#,#;(#,#);0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailAccountCurrentBalance()
	{
		try
		{
			lblbalancedetail.Text = "0";
			double ClosingBalance = GetAccountBalance(Conversion.ToInt(CmbAccountDetail.Value));
			lblbalancedetail.Text = ClosingBalance.ToString("#,#;(#,#);0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double GetAccountBalance(int AccountId)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		double ClosingBalance = 0.0;
		try
		{
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = voucherdatetime.Value,
				RefAccountId = AccountId
			});
			if (detail.Rows.Count > 0)
			{
				ClosingBalance = Conversion.ToDouble(detail.Rows[0]["Balance"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return ClosingBalance;
	}

	private void CheqNoFill()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			dtCheq.Rows.Clear();
			if (Conversion.ToInt(CmbVoucherType.Value) != 2 || !ChequeBookEnableStatus)
			{
				return;
			}
			((Control)(object)CmbCheqNo).Text = string.Empty;
			CheqBookHeader cheqBook = new CheqBookHeader();
			cheqBook.OrganizationId = UserAccount.OrganizationId;
			cheqBook.CompanyId = UserAccount.CompanyId;
			cheqBook.BankId = Conversion.ToInt(CmbCreditAccount.Value);
			dtCheq = CheqBookHeader.OutstandingCheqNo(cheqBook);
			if (dtCheq.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("CheqNo");
				dt.Clear();
				for (int i = 0; i < dtCheq.Rows.Count; i++)
				{
					dt.Rows.Add(dtCheq.Rows[i]["Id"], dtCheq.Rows[i]["CheqNo"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbCheqNo, "Id", "CheqNo", "CheqNo", ZeroIndex: false);
				}
			}
			else
			{
				((Control)(object)CmbCheqNo).Text = string.Empty;
				((UltraGridBase)CmbCheqNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindTaxTypes()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = TaxesTypes.GetForComboBind(new TaxesTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Type = 1
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTaxType, "Id", "TaxName", "TaxType", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combactitle_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbAccountDetail.Value) > 0)
			{
				lblbalancedetail.Visible = true;
				DetailAccountCurrentBalance();
				DetailAccountCurrencyCheck();
			}
			else
			{
				lblbalancedetail.Visible = false;
				lblbalancedetail.Text = "0";
			}
			BindSubsidiaryAccount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSubsidoryAccounts()
	{
		try
		{
			dtSubsidory = VoucherHead.BindSubsidiaryAccount(UserAccount.CompanyId, 0, (string)null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSubsidiaryAccount()
	{
		try
		{
			if (dtSubsidory.Rows.Count > 0)
			{
				if (((UltraGridBase)CmbAccountDetail).ActiveRow != null && ((Control)(object)CmbAccountDetail).Text != string.Empty)
				{
					int selectedAcId = Conversion.ToInt(CmbAccountDetail.Value);
					if (selectedAcId != 0)
					{
						DataTable matchingTable = dtSubsidory.Clone();
						foreach (DataRow row in dtSubsidory.Rows)
						{
							if (Conversion.ToInt(row["AccountId"]) == selectedAcId)
							{
								matchingTable.ImportRow(row);
							}
						}
						int SubsidiaryIdInCaseOfUpdateDetail = 0;
						if (((Control)(object)CmbSubsidiaryAccount).Text != string.Empty)
						{
							SubsidiaryIdInCaseOfUpdateDetail = Conversion.ToInt(CmbSubsidiaryAccount.Value);
						}
						if (matchingTable.Rows.Count > 0)
						{
							SubsdiaryAccountDatasourceLength = matchingTable.Rows.Count;
							DDL.BindDDL(matchingTable, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account", ZeroIndex: false);
							((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
							((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
							SetComboValue(CmbSubsidiaryAccount, SubsidiaryIdInCaseOfUpdateDetail, matchingTable);
						}
						else
						{
							((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
							((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
							SubsdiaryAccountDatasourceLength = 0;
						}
					}
					else
					{
						((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
						((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
						SubsdiaryAccountDatasourceLength = 0;
					}
				}
				else
				{
					SubsdiaryAccountDatasourceLength = dtSubsidory.Rows.Count;
					DDL.BindDDL(dtSubsidory, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				}
			}
			else
			{
				((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
				((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
				SubsdiaryAccountDatasourceLength = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combvtype_Leave(object sender, EventArgs e)
	{
		try
		{
			AccountTitleFill();
			if (Save.Visible && Save.Enabled)
			{
				VoucherNofill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcreditac_Leave(object sender, EventArgs e)
	{
		try
		{
			BaseCurrencyCheck();
			if (Conversion.ToInt(CmbCreditAccount.Value) > 0)
			{
				txtbalance.Visible = true;
				AccountCurrentBalance();
				CreditAccountCurrencyCheck();
			}
			else
			{
				txtbalance.Visible = false;
				txtbalance.Text = "0";
			}
			CheqNoFill();
			if (MultiCurrencyGridWorking)
			{
				CreateUpdateMultiCurrencyGridInfo();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BaseCurrencyCheck()
	{
		try
		{
			if (MultiCurrencyGridWorking && Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency")) == 0)
			{
				throw new Exception("Base Currency Not set in Configuration. Please Check!");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CreditAccountCurrencyCheck()
	{
		try
		{
			if (!MultiCurrencyGridWorking)
			{
				return;
			}
			int CurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[2].Value);
			string CurrencyCode = Conversion.ToString(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[3].Value);
			lblCrAcGlCurrency.Text = ((CurrencyId > 0) ? ("Credit Ac GLcy:\r\n" + CurrencyCode) : "Credit Ac GLcy:");
			if (CurrencyId == 0)
			{
				if (RecId == 0)
				{
					((Control)(object)CmbCreditAccount).Text = "";
					CmbCreditAccount.Focus();
				}
				throw new Exception("Credit Account Currency Not set. Please Check!");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DetailAccountCurrencyCheck()
	{
		try
		{
			if (MultiCurrencyGridWorking && Conversion.ToInt(((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[5].Value) == 0 && updateDetailIndex == -1)
			{
				((Control)(object)CmbAccountDetail).Text = "";
				CmbAccountDetail.Focus();
				throw new Exception("Detail Account Currency Not set. Please Check!");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdCombosFill()
	{
		try
		{
			grd.RootTable.Columns["PaymentType"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["PaymentType"].HasValueList = true;
			grd.RootTable.Columns["PaymentType"].LimitToList = true;
			grd.RootTable.Columns["PaymentType"].EditType = (EditType)4;
			grd.RootTable.Columns["PaymentType"].Caption = "Payment Type";
			grd.RootTable.Columns["PaymentType"].ValueList.PopulateValueList((IEnumerable)dtPaymetnType.DefaultView, "Id", "PaymentType");
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobLotId"].HasValueList = true;
			grd.RootTable.Columns["JobLotId"].LimitToList = true;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grd.RootTable.Columns["JobLotId"].Caption = "Job Lot";
			grd.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtJobLot.DefaultView, "Id", "JobLotDescription");
			DataTable dt = (DataTable)((UltraGridBase)CmbCostCenter).DataSource;
			if (dt != null && dt.Rows.Count > 0)
			{
				grd.RootTable.Columns["CostCenterId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["CostCenterId"].HasValueList = true;
				grd.RootTable.Columns["CostCenterId"].LimitToList = true;
				grd.RootTable.Columns["CostCenterId"].EditType = (EditType)4;
				grd.RootTable.Columns["CostCenterId"].Caption = "Cost Center";
				grd.RootTable.Columns["CostCenterId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "CostCenterName");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdCombosFill();
			CommonServices.GridWrappingAndColumnSettings(grd, 2);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["ReferenceAccountId"].Visible = false;
			grd.RootTable.Columns["GlCurrencyId"].Visible = false;
			grd.RootTable.Columns["TcyCodeId"].Visible = false;
			grd.RootTable.Columns["GlCurrency"].Visible = MultiCurrencyGridWorking;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grd.RootTable.Columns["CostCenterId"].Visible = IsBookingOffice;
			grd.RootTable.Columns["TaxAmount"].Visible = radioButton1.Checked;
			grd.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PaymentType"].Width = 90;
			grd.RootTable.Columns["AccountCode"].Width = 90;
			grd.RootTable.Columns["AccountTitle"].Width = 220;
			grd.RootTable.Columns["ReferenceAccount"].Width = 150;
			grd.RootTable.Columns["SubsidiaryAccount"].Width = 180;
			grd.RootTable.Columns["Remarks"].Width = 220;
			grd.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant;
			grd.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grd.RootTable.Columns["TcyExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
			grd.RootTable.Columns["FcyAmount"].Width = 110;
			grd.RootTable.Columns["FcyAmount"].Caption = "Tcy Amount";
			grd.RootTable.Columns["Amount"].Width = 110;
			grd.RootTable.Columns["Amount"].Caption = "Debit Amount";
			grd.RootTable.Columns["BranchName"].Width = 130;
			grd.RootTable.Columns["CostCenterId"].Width = 130;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountTypeId"].Visible = false;
			grd.RootTable.Columns["FinancialInstrumentId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			grd.RootTable.Columns["ChequeTypeId"].Visible = false;
			grd.RootTable.Columns["ChequeId"].Visible = false;
			if (Conversion.ToInt(CmbVoucherType.Value) == 1)
			{
				grd.ColumnAutoResize = true;
				grd.RootTable.Columns["FinancialInstrument"].Visible = false;
				grd.RootTable.Columns["ChequeDate"].Visible = false;
				grd.RootTable.Columns["ChequeNo"].Visible = false;
				grd.RootTable.Columns["PayeeTitle"].Visible = false;
			}
			else
			{
				grd.ColumnAutoResize = false;
				grd.RootTable.Columns["FinancialInstrument"].Visible = true;
				grd.RootTable.Columns["ChequeDate"].Visible = true;
				grd.RootTable.Columns["ChequeNo"].Visible = true;
				grd.RootTable.Columns["PayeeTitle"].Visible = true;
			}
			grd.RootTable.Columns["BranchId"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchName"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyCodeId"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyCode"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyExchangeRate"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccountTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccount"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferenceAccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferenceAccount"].EditType = (EditType)0;
			grd.RootTable.Columns["FinancialInstrumentId"].EditType = (EditType)0;
			grd.RootTable.Columns["FinancialInstrument"].EditType = (EditType)0;
			grd.RootTable.Columns["ChequeId"].EditType = (EditType)0;
			grd.RootTable.Columns["ChequeTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["ChequeNo"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grd.RootTable.Columns["ChequeDate"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["GlCurrencyId"].EditType = (EditType)0;
			grd.RootTable.Columns["GlCurrency"].EditType = (EditType)0;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["ChequeDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			((Control)(object)grd).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdMultiCurrencySetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "LocalCurencyId", "GlCurrencyId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"ExchangeRate",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdMultiCurrency, 2, 3, hiddenColumns, editableColumns);
			grdMultiCurrency.RootTable.Columns["LocalCurrency"].Width = Constants.InventoryConstants.FcyConstant + 25;
			grdMultiCurrency.RootTable.Columns["To"].Width = 50;
			grdMultiCurrency.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 25;
			grdMultiCurrency.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 20;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			grd.ClearStructure();
			RecId = Conversion.ToInt((object)ID);
			VoucherHead vh = VoucherHead.GetByID(RecId);
			if (vh != null)
			{
				Save.Visible = false;
				Update.Visible = true;
				UpdateMode = true;
				((Control)(object)CmbVoucherType).Enabled = false;
				tabControl1.SelectedIndex = 0;
				if (vh.DocumentTypeId == 1)
				{
					((Control)(object)CmbVoucherType).Text = "Cash Payment Voucher";
				}
				else
				{
					((Control)(object)CmbVoucherType).Text = "Bank Payment Voucher";
				}
				combvtype_Leave(null, null);
				voucherdatetime.Value = vh.VoucherDate;
				CmbCreditAccount.Value = vh.RefAccountId;
				combcreditac_Leave(null, null);
				txtremarksmain.Text = ((vh.DocumentTypeId != 2) ? vh.Remarks : (AutoRemarksForPaymentThroughBank ? vh.RemarksOtherLingo : ((vh.RemarksOtherLingo != null) ? vh.RemarksOtherLingo : vh.Remarks)));
				txtvoucherno.Text = Conversion.ToString((object)vh.VoucherCode);
				CmbProjectId.Value = vh.ProjectId;
				cmbCurrency.Value = vh.MultiCurrencyId;
				cmbCurrency_Leave(null, null);
				txtExchangeRate.Text = vh.ExchangeCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtFcyAmount.Text = vh.FcAmount.ToString("#,##0.###");
				if (vh.MultiCurrencyId == 0 || vh.ExchangeCurrencyRate == 0.0)
				{
					DefaultConfigurations();
				}
				AttachmentsValues = vh.AttachmentsValues;
				CustomAttachmentsValues = vh.CustomAttachmentsValues;
				chkCustomAccounts.Checked = vh.CustomAccounts;
				radioButton1.Checked = Conversion.ToBool((object)vh.InclusiveTax);
				table.Rows.Clear();
				for (int i = 0; i < vh.voucherDetailList.Count; i++)
				{
					cmbLocationType.Value = ((vh.voucherDetailList.Count > 0) ? Conversion.ToInt((object)vh.voucherDetailList[0].LocationTypeId) : 0);
					if (vh.voucherDetailList[i].IsTaxable == "False" && vh.voucherDetailList[i].AccountId != Convert.ToInt32(CmbCreditAccount.Value))
					{
						double DebitAmount = ((radioButton1.Checked && vh.voucherDetailList[i].DebitAmount > 0.0) ? (vh.voucherDetailList[i].DebitAmount + vh.voucherDetailList[i].TaxAmount) : vh.voucherDetailList[i].DebitAmount);
						table.Rows.Add(vh.voucherDetailList[i].PaymentTypeId, vh.voucherDetailList[i].AccountCode, vh.voucherDetailList[i].AccountId, vh.voucherDetailList[i].AccountTitle, vh.voucherDetailList[i].GlcyCode, (vh.voucherDetailList[i].SubsidiaryTypeId == 1) ? vh.voucherDetailList[i].SupplierCustomerId : ((vh.voucherDetailList[i].SubsidiaryTypeId == 2) ? vh.voucherDetailList[i].EmployeeId : ((vh.voucherDetailList[i].SubsidiaryTypeId == 3) ? vh.voucherDetailList[i].SubsidiaryAccountId : ((vh.voucherDetailList[i].SubsidiaryTypeId == 4) ? vh.voucherDetailList[i].SubsidiaryAccountId : 0))), vh.voucherDetailList[i].SubsidiaryAccountTitle, vh.voucherDetailList[i].SubsidiaryTypeId, vh.voucherDetailList[i].JobLotId, (vh.DocumentTypeId != 2) ? vh.voucherDetailList[i].Comments : (AutoRemarksForPaymentThroughBank ? vh.voucherDetailList[i].CommentsOtherLingo : ((vh.voucherDetailList[i].CommentsOtherLingo != null) ? vh.voucherDetailList[i].CommentsOtherLingo : vh.voucherDetailList[i].Comments)), vh.voucherDetailList[i].DMultiCurrencyId, vh.voucherDetailList[i].TcyCode, vh.voucherDetailList[i].DExchangeCurrencyRate, vh.voucherDetailList[i].DCurrencyAmount, DebitAmount, vh.voucherDetailList[i].ReferenceAccountId, vh.voucherDetailList[i].ReferenceAccount, vh.voucherDetailList[i].InstrumentTypeId, vh.voucherDetailList[i].InstrumentType, vh.voucherDetailList[i].DCheqDate, vh.voucherDetailList[i].InvoiceNoRefId, vh.voucherDetailList[i].CheqNoDetail, vh.voucherDetailList[i].PayeeTitle, vh.voucherDetailList[i].ChequeTypeId, vh.voucherDetailList[i].BranchesId, vh.voucherDetailList[i].BranchName, vh.voucherDetailList[i].CostCenterId, vh.voucherDetailList[i].TaxAmount, vh.voucherDetailList[i].ThirdCurrencyId);
					}
				}
				if (MultiCurrencyGridWorking && vh.voucherDetailList != null && vh.voucherDetailList.Count > 0)
				{
					txtBaseFcyCode.Tag = vh.voucherDetailList[0].BaseFcyId;
					txtBaseFcyCode.Text = Conversion.ToString((object)vh.voucherDetailList[0].BaseFcyCode);
					txtBaseFcyExchangeRate.Text = Conversion.ToString((object)vh.voucherDetailList[0].BaseFcyExchangeRate);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				Total();
				if (vh.voucherDetailList.Any((VoucherDetail x) => x.InvoiceNoRefId > 0))
				{
					((Control)(object)CmbVoucherType).Enabled = false;
					((Control)(object)CmbCreditAccount).Enabled = false;
				}
				dtVoucherDetailMultiCurrency.Rows.Clear();
				foreach (VoucherDetailMultiCurrency item in vh.VoucherDetailMultiCurrencyList)
				{
					dtVoucherDetailMultiCurrency.Rows.Add(item.Id, item.LocalCurrencyId, item.LcyCurrencyCode, "To", item.GlCurrencyId, item.GlFcyCurrencyCode, item.ExchangeRate);
				}
				grdMultiCurrency.DataSource = dtVoucherDetailMultiCurrency;
				grdMultiCurrency.RetrieveStructure();
				GrdMultiCurrencySetting();
				VoucherDetail whtTax = vh.voucherDetailList.Find((VoucherDetail x) => x.IsTaxable == "True" && x.TaxesTotalAmount > 0.0 && x.TaxTypeId > 0);
				if (whtTax != null)
				{
					ChkBoxWthHolding.Checked = true;
					CmbAgainstAc.Value = vh.RefDocNoId;
					CmbWithHoldingAc.Value = whtTax.AccountId;
					((Control)(object)CmbWithHoldingAc).Text = whtTax.AccountTitle;
					CmbTaxType.Value = whtTax.TaxTypeId;
					txtTaxPercent.Text = whtTax.TaxPrcnt.ToString();
					txtTaxAmount.Text = whtTax.TaxesTotalAmount.ToString();
				}
				else
				{
					txtTaxPercent.Text = "0";
					txtTaxAmount.Text = "0";
					ChkBoxWthHolding.Checked = false;
				}
				VoucherDetail SrbTax = vh.voucherDetailList.Find((VoucherDetail x) => x.SBRTaxAmount > 0.0 && x.CreditAmount > 0.0);
				if (SrbTax != null)
				{
					txtSrbTaxPercent.AccessibleName = "True";
					CmbSrbTaxAccount.Value = SrbTax.AccountId;
					txtSrbTaxAmount.Text = SrbTax.SBRTaxAmount.ToString();
				}
				else
				{
					CmbSrbTaxAccount.Value = DefaultSrbAccountId;
					txtSrbTaxAmount.Text = "0";
				}
				VoucherDetail DiscRow = vh.voucherDetailList.Find((VoucherDetail x) => x.DiscountAmount > 0.0 && x.CreditAmount > 0.0);
				if (DiscRow != null)
				{
					txtDiscPercent.AccessibleName = "True";
					CmbDiscountAccount.Value = DiscRow.AccountId;
					txtDiscPercent.Text = DiscRow.DiscountPercent.ToString();
					txtDiscountAmount.Text = DiscRow.DiscountAmount.ToString();
				}
				else
				{
					CmbDiscountAccount.Value = DefaultDiscountAccountId;
					txtDiscPercent.Text = "0";
					txtDiscountAmount.Text = "0";
				}
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				txtExchangeRate_TextChanged(null, null);
			}
			else
			{
				Reset();
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
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			SubsdiaryAccountDatasourceLength = 0;
			CheqNoValidatingInUpdate = 0;
			updateDetailIndex = -1;
			((Control)(object)CmbVoucherType).Enabled = true;
			((Control)(object)CmbCreditAccount).Enabled = true;
			table.Rows.Clear();
			UpdateMode = false;
			txtFcyAmount.Text = string.Empty;
			lblbalancedetail.Visible = false;
			lblbalancedetail.Text = "0";
			txtremarks.Text = string.Empty;
			txtamount.Text = string.Empty;
			((Control)(object)txtPayTitle).Text = string.Empty;
			((Control)(object)cmbChequeType).Text = string.Empty;
			txtremarksmain.Text = string.Empty;
			((Control)(object)CmbTaxType).Text = string.Empty;
			((Control)(object)CmbAgainstAc).Text = string.Empty;
			ChkBoxWthHolding.Checked = false;
			txtValue.Clear();
			txtTaxAmount.Clear();
			txtTotalAmount.Clear();
			((UltraGridBase)CmbPaymentType).Rows[0].Activate();
			((Control)(object)CmbAccountDetail).Text = string.Empty;
			((Control)(object)CmbCheqNo).Text = string.Empty;
			((Control)(object)CmbWithHoldingAc).Text = string.Empty;
			Save.Visible = true;
			CmbAccountDetail.Focus();
			VoucherNofill();
			Save.Visible = true;
			Update.Visible = false;
			combcreditac_Leave(null, null);
			grd.DataSource = null;
			ResetDetail();
			lblCrAcGlCurrency.Text = "Credit Ac GLcy:";
			dtVoucherDetailMultiCurrency.Rows.Clear();
			grdMultiCurrency.DataSource = dtVoucherDetailMultiCurrency;
			txtSrbTaxPercent.Text = string.Empty;
			txtSrbTaxAmount.Text = string.Empty;
			txtDiscPercent.Text = string.Empty;
			txtDiscountAmount.Text = string.Empty;
			cmbCurrency_Leave(null, null);
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
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			CmbAccountDetail.Focus();
			((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
			((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
			txtFcyAmountDetail.Clear();
			txtamount.Clear();
			((Control)(object)CmbCheqNo).Text = string.Empty;
			((Control)(object)txtPayTitle).Text = string.Empty;
			txtFcyAmountDetail.AccessibleName = "";
			txtamount.AccessibleName = "";
			Total();
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
			Reset();
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			txtbalance.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Total()
	{
		try
		{
			double taxRate = Conversion.ToDouble((object)txtTaxPercent.Text);
			double srbAmount = Conversion.ToDouble((object)txtSrbTaxAmount.Text);
			double discount = Conversion.ToDouble((object)txtDiscountAmount.Text);
			double taxAmount = 0.0;
			double TotalAmt = 0.0;
			double BaseAmount = 0.0;
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					BaseAmount += Conversion.ToDouble(r.Cells["Amount"].Value);
				}
			}
			if (BaseAmount > 0.0)
			{
				if (RadExcluded.Checked)
				{
					taxAmount = (BaseAmount + srbAmount + discount) / (100.0 - taxRate) * taxRate;
					TotalAmt = BaseAmount + taxAmount + srbAmount + discount;
				}
				else
				{
					double num = BaseAmount + srbAmount + discount;
					BaseAmount = num / 100.0 * (100.0 - taxRate);
					TotalAmt = num;
					taxAmount = TotalAmt - BaseAmount;
				}
				txtValue.Text = BaseAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtTaxAmount.Text = taxAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalAmount.Text = TotalAmt.ToString(clsGlobalVariables.stringFormatsingle);
				TaxAmountProportion();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (ChequeBookSerialWiseOnBpv)
				{
					int max = (from row in table.AsEnumerable()
						select Conversion.ToInt((object)row.Field<string>("ChequeNo"))).Distinct().ToList().Max();
					if (Conversion.ToInt(item.Cells["ChequeNo"].Value) != max)
					{
						throw new Exception("You can't Delete This Row Because This Entry Is Against Serial Wise Cheque No");
					}
					item.Delete();
				}
				else
				{
					item.Delete();
				}
			}
			if (Conversion.ToInt(CmbVoucherType.Value) == 2)
			{
				if (grd.GetRows().Length != 0)
				{
					int CountCheqid = 0;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (Conversion.ToInt(r.Cells["ChequeId"].Value) > 0)
						{
							CountCheqid += Conversion.ToInt(r.Cells["ChequeId"].Value);
						}
					}
					if (CountCheqid == 0)
					{
						((Control)(object)CmbCreditAccount).Enabled = true;
					}
				}
				else
				{
					((Control)(object)CmbCreditAccount).Enabled = true;
				}
			}
			Total();
			grd.UpdateData();
			if (MultiCurrencyGridWorking)
			{
				CreateUpdateMultiCurrencyGridInfo();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grd.UpdateData();
			Total();
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, table);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryFillCpv()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.AppId = UserAccount.AppId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.Ids = "1";
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateCpvHistory.Checked)
				{
					vh.FromDate = FromDateCpvHistory.Value;
				}
				if (ToDateCpvHistory.Checked)
				{
					vh.ToDate = ToDateCpvHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateCpvHistory.Checked)
				{
					vh.EntryDateFrom = FromDateCpvHistory.Value;
				}
				if (ToDateCpvHistory.Checked)
				{
					vh.EntryDateTo = ToDateCpvHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateCpvHistory.Checked)
				{
					vh.ModifyDateFrom = FromDateCpvHistory.Value;
				}
				if (ToDateCpvHistory.Checked)
				{
					vh.ModifyDateTo = ToDateCpvHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateCpvHistory.Checked)
				{
					vh.ApprovedDateFrom = FromDateCpvHistory.Value;
				}
				if (ToDateCpvHistory.Checked)
				{
					vh.ApprovedDateTo = ToDateCpvHistory.Value;
				}
			}
			vh.FromDocNo = Conversion.ToInt((object)txtFromDocNoCpv.Text);
			vh.ToDocNo = Conversion.ToInt((object)txtToDocNoCpv.Text);
			vh.AccountId = Conversion.ToInt(cmbAccountTitleCpv.Value);
			if (((Control)(object)cmbApprovedStatusCpv).Text == "Approved")
			{
				vh.IsApproved = true;
			}
			if (((Control)(object)cmbApprovedStatusCpv).Text == "Not Apporved")
			{
				vh.IsApproved = false;
			}
			if (((Control)(object)cmbApprovedStatusCpv).Text == "All")
			{
				vh.ApprovedFilter = "All";
			}
			DataTable lst = VoucherHead.VoucherFormHistory(vh);
			if (lst.Rows.Count > 0)
			{
				DataTable dtCashVoucher = new DataTable();
				dtCashVoucher.Columns.Add("Id");
				dtCashVoucher.Columns.Add("DocumentTypeId");
				dtCashVoucher.Columns.Add("VoucherDate", typeof(DateTime));
				dtCashVoucher.Columns.Add("VoucherCode", typeof(int));
				dtCashVoucher.Columns.Add("DocumentType");
				dtCashVoucher.Columns.Add("AccountTitle");
				dtCashVoucher.Columns.Add("VoucherAmount", typeof(double));
				dtCashVoucher.Columns.Add("FcyCode");
				dtCashVoucher.Columns.Add("ExchangeRate", typeof(double));
				dtCashVoucher.Columns.Add("FcyAmount", typeof(double));
				dtCashVoucher.Columns.Add("Remarks");
				dtCashVoucher.Columns.Add("EntryUser");
				dtCashVoucher.Columns.Add("EntryDate", typeof(DateTime));
				dtCashVoucher.Columns.Add("ModifyUser");
				dtCashVoucher.Columns.Add("ModifyDate", typeof(DateTime));
				dtCashVoucher.Columns.Add("ApprovedUser");
				dtCashVoucher.Columns.Add("ApprovedDate", typeof(DateTime));
				dtCashVoucher.Columns.Add("Attachment");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtCashVoucher.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToShortDateString(), lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeCode"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["Remarks"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["NoOfAttachments"]);
				}
				VouchersInfoBoxCash.Visible = true;
				lblTotalVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalVouchers"]).ToString();
				lblApprovedVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalApprovedVoucher"]).ToString();
				lblUnApprovedVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalUnApprovedVoucher"]).ToString();
				DataGridHistory.DataSource = dtCashVoucher;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				VouchersInfoBoxCash.Visible = false;
				grdDetail.ClearStructure();
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryFillBpv()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.AppId = UserAccount.AppId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.Ids = "2";
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (rdbpvdocdate.Checked)
			{
				if (FromDateBpvHistory.Checked)
				{
					vh.FromDate = FromDateBpvHistory.Value;
				}
				if (ToDateBpvHistory.Checked)
				{
					vh.ToDate = ToDateBpvHistory.Value;
				}
			}
			else if (rdbpventrydate.Checked)
			{
				if (FromDateBpvHistory.Checked)
				{
					vh.EntryDateFrom = FromDateBpvHistory.Value;
				}
				if (ToDateBpvHistory.Checked)
				{
					vh.EntryDateTo = ToDateBpvHistory.Value;
				}
			}
			else if (rdbpvmodifydate.Checked)
			{
				if (FromDateBpvHistory.Checked)
				{
					vh.ModifyDateFrom = FromDateBpvHistory.Value;
				}
				if (ToDateBpvHistory.Checked)
				{
					vh.ModifyDateTo = ToDateBpvHistory.Value;
				}
			}
			else if (rdbpvapprovdate.Checked)
			{
				if (FromDateBpvHistory.Checked)
				{
					vh.ApprovedDateFrom = FromDateBpvHistory.Value;
				}
				if (ToDateBpvHistory.Checked)
				{
					vh.ApprovedDateTo = ToDateBpvHistory.Value;
				}
			}
			vh.FromDocNo = Conversion.ToInt((object)txtFromDocNoBpvHistory.Text);
			vh.ToDocNo = Conversion.ToInt((object)txtToDocNoBpvHistory.Text);
			vh.AccountId = Conversion.ToInt(CmbAccountTitleBpvHistory.Value);
			if (((Control)(object)cmbApproveStatusBpv).Text == "Approved")
			{
				vh.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatusBpv).Text == "Not Apporved")
			{
				vh.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatusBpv).Text == "All")
			{
				vh.ApprovedFilter = "All";
			}
			DataTable lst = VoucherHead.VoucherFormHistory(vh);
			if (lst.Rows.Count > 0)
			{
				DataTable dtBankVoucher = new DataTable();
				dtBankVoucher.Columns.Add("Id");
				dtBankVoucher.Columns.Add("DocumentTypeId");
				dtBankVoucher.Columns.Add("VoucherDate", typeof(DateTime));
				dtBankVoucher.Columns.Add("VoucherCode", typeof(int));
				dtBankVoucher.Columns.Add("DocumentType");
				dtBankVoucher.Columns.Add("ChequeNo");
				dtBankVoucher.Columns.Add("AccountTitle");
				dtBankVoucher.Columns.Add("VoucherAmount", typeof(double));
				dtBankVoucher.Columns.Add("FcyCode");
				dtBankVoucher.Columns.Add("ExchangeRate", typeof(double));
				dtBankVoucher.Columns.Add("FcyAmount", typeof(double));
				dtBankVoucher.Columns.Add("Remarks");
				dtBankVoucher.Columns.Add("EntryUser");
				dtBankVoucher.Columns.Add("EntryDate", typeof(DateTime));
				dtBankVoucher.Columns.Add("ModifyUser");
				dtBankVoucher.Columns.Add("ModifyDate", typeof(DateTime));
				dtBankVoucher.Columns.Add("ApprovedUser");
				dtBankVoucher.Columns.Add("ApprovedDate", typeof(DateTime));
				dtBankVoucher.Columns.Add("Attachment");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtBankVoucher.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToShortDateString(), lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeCode"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["Remarks"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["NoOfAttachments"]);
				}
				VoucherInfoBoxBank.Visible = true;
				lblTotalVouchersBpv.Text = Conversion.ToInt(lst.Rows[0]["TotalVouchers"]).ToString();
				lblApprovedVouchersBpv.Text = Conversion.ToInt(lst.Rows[0]["TotalApprovedVoucher"]).ToString();
				lblUnApprovedVouchersBpv.Text = Conversion.ToInt(lst.Rows[0]["TotalUnApprovedVoucher"]).ToString();
				BankVoucherHistoryGrid.DataSource = dtBankVoucher;
				BankVoucherHistoryGrid.RetrieveStructure();
				BankGridSettings();
			}
			else
			{
				VoucherInfoBoxBank.Visible = false;
				BankVoucherHistoryGrid.ClearStructure();
				GridBankDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["FcyCode"].Visible = MultiCurrencyFeatureVisibilty;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Visible = MultiCurrencyFeatureVisibilty;
			DataGridHistory.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["VoucherAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			DataGridHistory.RootTable.Columns["VoucherCode"].Caption = "V.No";
			DataGridHistory.RootTable.Columns["DocumentType"].Caption = "V.Type";
			DataGridHistory.RootTable.Columns["VoucherDate"].Width = 90;
			DataGridHistory.RootTable.Columns["VoucherCode"].Width = 15;
			DataGridHistory.RootTable.Columns["DocumentType"].Width = 30;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("Print_102");
			DataGridHistory.RootTable.Columns["Print_102"].Caption = "Print-II";
			DataGridHistory.RootTable.Columns["Print_102"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print_102"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print_102"].ButtonText = "Print-II";
			DataGridHistory.RootTable.Columns["Print_102"].Width = 60;
			DataGridHistory.RootTable.Columns.Add("PrintNew");
			DataGridHistory.RootTable.Columns["PrintNew"].Caption = "Print-III";
			DataGridHistory.RootTable.Columns["PrintNew"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["PrintNew"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["PrintNew"].ButtonText = "Print-III";
			DataGridHistory.RootTable.Columns["PrintNew"].Width = 60;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("AddAttachment");
			DataGridHistory.RootTable.Columns["AddAttachment"].Caption = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonText = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].Width = 100;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.RootTable.Columns["Print_102"].Position = 2;
			DataGridHistory.RootTable.Columns["PrintNew"].Position = 3;
			DataGridHistory.RootTable.Columns["DocumentType"].Position = 4;
			DataGridHistory.FrozenColumns = 5;
			DataGridHistory.RootTable.Columns["Remarks"].Width = 350;
			DataGridHistory.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BankGridSettings()
	{
		try
		{
			BankVoucherHistoryGrid.RootTable.Columns["Id"].Visible = false;
			BankVoucherHistoryGrid.RootTable.Columns["DocumentTypeId"].Visible = false;
			BankVoucherHistoryGrid.RootTable.Columns["FcyCode"].Visible = MultiCurrencyFeatureVisibilty;
			BankVoucherHistoryGrid.RootTable.Columns["ExchangeRate"].Visible = MultiCurrencyFeatureVisibilty;
			BankVoucherHistoryGrid.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			BankVoucherHistoryGrid.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			BankVoucherHistoryGrid.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			BankVoucherHistoryGrid.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			BankVoucherHistoryGrid.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			BankVoucherHistoryGrid.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			BankVoucherHistoryGrid.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			BankVoucherHistoryGrid.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			BankVoucherHistoryGrid.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			BankVoucherHistoryGrid.RootTable.Columns["VoucherAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			BankVoucherHistoryGrid.RootTable.Columns["VoucherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			BankVoucherHistoryGrid.RootTable.Columns["EntryDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			BankVoucherHistoryGrid.RootTable.Columns["ModifyDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			BankVoucherHistoryGrid.RootTable.Columns["ApprovedDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			BankVoucherHistoryGrid.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			BankVoucherHistoryGrid.RootTable.Columns["VoucherCode"].Caption = "V.No";
			BankVoucherHistoryGrid.RootTable.Columns["DocumentType"].Caption = "V.Type";
			BankVoucherHistoryGrid.RootTable.Columns["VoucherDate"].Width = 90;
			BankVoucherHistoryGrid.RootTable.Columns["VoucherCode"].Width = 15;
			BankVoucherHistoryGrid.RootTable.Columns["DocumentType"].Width = 30;
			CommonServices.GridAutoAdjustmentNew(BankVoucherHistoryGrid);
			BankVoucherHistoryGrid.RootTable.Columns.Add("Print");
			BankVoucherHistoryGrid.RootTable.Columns["Print"].Caption = "Print";
			BankVoucherHistoryGrid.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherHistoryGrid.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			BankVoucherHistoryGrid.RootTable.Columns["Print"].ButtonText = "Print";
			BankVoucherHistoryGrid.RootTable.Columns["Print"].Width = 50;
			BankVoucherHistoryGrid.RootTable.Columns.Add("Print_102");
			BankVoucherHistoryGrid.RootTable.Columns["Print_102"].Caption = "Print-II";
			BankVoucherHistoryGrid.RootTable.Columns["Print_102"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherHistoryGrid.RootTable.Columns["Print_102"].ButtonStyle = (ButtonStyle)5;
			BankVoucherHistoryGrid.RootTable.Columns["Print_102"].ButtonText = "Print-II";
			BankVoucherHistoryGrid.RootTable.Columns["Print_102"].Width = 60;
			BankVoucherHistoryGrid.RootTable.Columns.Add("PrintNew");
			BankVoucherHistoryGrid.RootTable.Columns["PrintNew"].Caption = "Print-III";
			BankVoucherHistoryGrid.RootTable.Columns["PrintNew"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherHistoryGrid.RootTable.Columns["PrintNew"].ButtonStyle = (ButtonStyle)5;
			BankVoucherHistoryGrid.RootTable.Columns["PrintNew"].ButtonText = "Print-III";
			BankVoucherHistoryGrid.RootTable.Columns["PrintNew"].Width = 60;
			BankVoucherHistoryGrid.RootTable.Columns.Add("Edit");
			BankVoucherHistoryGrid.RootTable.Columns["Edit"].Caption = "Edit";
			BankVoucherHistoryGrid.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherHistoryGrid.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			BankVoucherHistoryGrid.RootTable.Columns["Edit"].ButtonText = "Edit";
			BankVoucherHistoryGrid.RootTable.Columns["Edit"].Width = 50;
			BankVoucherHistoryGrid.RootTable.Columns.Add("AddAttachment");
			BankVoucherHistoryGrid.RootTable.Columns["AddAttachment"].Caption = "Add Attachment";
			BankVoucherHistoryGrid.RootTable.Columns["AddAttachment"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherHistoryGrid.RootTable.Columns["AddAttachment"].ButtonStyle = (ButtonStyle)5;
			BankVoucherHistoryGrid.RootTable.Columns["AddAttachment"].ButtonText = "Add Attachment";
			BankVoucherHistoryGrid.RootTable.Columns["AddAttachment"].Width = 110;
			BankVoucherHistoryGrid.RootTable.Columns["Edit"].Position = 0;
			BankVoucherHistoryGrid.RootTable.Columns["Print"].Position = 1;
			BankVoucherHistoryGrid.RootTable.Columns["Print_102"].Position = 2;
			BankVoucherHistoryGrid.RootTable.Columns["PrintNew"].Position = 3;
			BankVoucherHistoryGrid.RootTable.Columns["DocumentType"].Position = 4;
			BankVoucherHistoryGrid.FrozenColumns = 5;
			BankVoucherHistoryGrid.RootTable.Columns["Remarks"].Width = 350;
			BankVoucherHistoryGrid.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DefaultConfigurations()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbJobLot.Value = JobId;
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				double BaseCurrencyRate = (BaseRate = Conversion.ToDouble((object)BaseRatelist[0].ConfigKey.ToString()));
				txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int Currency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				BaseCurrency = Currency;
				cmbCurrency.Value = BaseCurrency;
				cmbCurrency_Leave(null, null);
			}
			List<ConfigrationsAllocation> AutoRemarks = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksForPaymentThroughBank").ToList();
			if (AutoRemarks.Count > 0)
			{
				AutoRemarksForPaymentThroughBank = Conversion.ToBool((object)AutoRemarks[0].ConfigKey);
			}
			List<ConfigrationsAllocation> AutoRemarksIncludeHeader = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludeHeaderRemarks").ToList();
			if (AutoRemarksIncludeHeader.Count > 0)
			{
				AutoRemarksIncludeHeaderRemarks = Conversion.ToBool((object)AutoRemarksIncludeHeader[0].ConfigKey);
			}
			List<ConfigrationsAllocation> AutoRemarksIncludeChqDateNum = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludeChequeDateAndNumber").ToList();
			if (AutoRemarksIncludeChqDateNum.Count > 0)
			{
				AutoRemarksIncludeChqDateAndNum = Conversion.ToBool((object)AutoRemarksIncludeChqDateNum[0].ConfigKey);
			}
			List<ConfigrationsAllocation> AutoRemarksIncludeChqNum = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludeChequeNumber").ToList();
			if (AutoRemarksIncludeChqNum.Count > 0)
			{
				AutoRemarksIncludeChqNumber = Conversion.ToBool((object)AutoRemarksIncludeChqNum[0].ConfigKey);
			}
			List<ConfigrationsAllocation> PayeeTitle = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludePayeeTitle").ToList();
			if (PayeeTitle.Count > 0)
			{
				AutoRemarksIncludePayeeTitle = Conversion.ToBool((object)PayeeTitle[0].ConfigKey);
			}
			List<ConfigrationsAllocation> DetailRemarks = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludeDetailRemarks").ToList();
			if (DetailRemarks.Count > 0)
			{
				AutoRemarksIncludeDetailRemarks = Conversion.ToBool((object)DetailRemarks[0].ConfigKey);
			}
			List<ConfigrationsAllocation> ChqBookEnable = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CheqBook Enabled").ToList();
			if (ChqBookEnable.Count > 0)
			{
				ChequeBookEnableStatus = Conversion.ToBool((object)ChqBookEnable[0].ConfigKey);
			}
			List<ConfigrationsAllocation> ChqNoCompulsory = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "ChequeNoCompulsoryOnBpv").ToList();
			if (ChqNoCompulsory.Count > 0)
			{
				ChequeBookCompulsoryOnBpv = Conversion.ToBool((object)ChqNoCompulsory[0].ConfigKey);
			}
			List<ConfigrationsAllocation> ChqNoSerialWise = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Cheque Posting Serial Wise").ToList();
			if (ChqNoSerialWise.Count > 0)
			{
				ChequeBookSerialWiseOnBpv = Conversion.ToBool((object)ChqNoSerialWise[0].ConfigKey);
			}
			List<ConfigrationsAllocation> dfSRBAcId = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "DefaultRevenueBoardAccount").ToList();
			if (dfSRBAcId.Count > 0)
			{
				DefaultSrbAccountId = Conversion.ToInt((object)dfSRBAcId[0].ConfigKey);
				if (DefaultSrbAccountId > 0)
				{
					CmbSrbTaxAccount.Value = DefaultSrbAccountId;
				}
			}
			List<ConfigrationsAllocation> dfDiscAcId = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "DefaultDiscountAccount").ToList();
			if (dfDiscAcId.Count > 0)
			{
				DefaultDiscountAccountId = Conversion.ToInt((object)dfDiscAcId[0].ConfigKey);
				if (DefaultDiscountAccountId > 0)
				{
					CmbDiscountAccount.Value = DefaultDiscountAccountId;
				}
			}
			List<ConfigrationsAllocation> ChequePrinting = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "ChequePrintingEnable").ToList();
			if (ChequePrinting.Count > 0)
			{
				ChequePrintingEnable = Conversion.ToBool((object)ChequePrinting[0].ConfigKey);
			}
			if (!ChequeBookCompulsoryOnBpv)
			{
				CmbCheqNo.LimitToList = false;
			}
			cbPrintOnSave.Visible = ChequePrintingEnable;
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "is Minus balance Allowed");
			PreventNegativeBalanceEntry = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DisplayWarningforNegativeBalance");
			DisplayWarningforNegativeBalance = Conversion.ToBool((object)((obj2 != null) ? obj2.ConfigKey : null));
			ConfigrationsAllocation obj3 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DisableBothNegativeBalanceRestrictions");
			DisableBothNegativeBalanceRestrictions = Conversion.ToBool((object)((obj3 != null) ? obj3.ConfigKey : null));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton10_Click(object sender, EventArgs e)
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

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int ID = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "Edit")
			{
				Reset();
				ReadById(ID);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Print_102")
			{
				CommonServices.AcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), DocumentTypeId);
			}
			else if (e.Column.Key == "PrintNew")
			{
				CommonServices.PaymentAndReceiptVoucherSlip(ID, DocumentTypeId);
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, ID, DocumentTypeId, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherDetailByHeaderId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DataGridHistory.CurrentRow == null || (int)DataGridHistory.CurrentRow.RowType != 0 || DataGridHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				VoucherHead byID = VoucherHead.GetByID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
				DataTable table = new DataTable();
				table.Columns.Add("AccountCode");
				table.Columns.Add("AccountTitle");
				table.Columns.Add("SubsidiaryAccount");
				table.Columns.Add("Job/Lot");
				table.Columns.Add("Remarks");
				table.Columns.Add("DebitAmount", typeof(double));
				table.Columns.Add("CreditAmount", typeof(double));
				table.Columns.Add("DebitFcyAmount", typeof(double));
				table.Columns.Add("CreditFcyAmount", typeof(double));
				table.Columns.Add("ReferenceAccount");
				table.Columns.Add("BranchName");
				table.Columns.Add("CostCenter");
				table.Columns.Add("TaxAmount");
				table.Rows.Clear();
				List<VoucherDetail> sortingvoucherlist = byID.voucherDetailList.OrderBy((VoucherDetail x) => x.Id).ToList();
				for (int i = 0; i < sortingvoucherlist.Count; i++)
				{
					double DebitAmount = ((radioButton1.Checked && sortingvoucherlist[i].DebitAmount > 0.0) ? (sortingvoucherlist[i].DebitAmount + sortingvoucherlist[i].TaxAmount) : sortingvoucherlist[i].DebitAmount);
					double CreditAmount = ((radioButton1.Checked && sortingvoucherlist[i].CreditAmount > 0.0) ? (sortingvoucherlist[i].CreditAmount + sortingvoucherlist[i].TaxAmount) : sortingvoucherlist[i].CreditAmount);
					table.Rows.Add(sortingvoucherlist[i].AccountCode, sortingvoucherlist[i].AccountTitle, sortingvoucherlist[i].SubsidiaryAccountTitle, sortingvoucherlist[i].JobLotDescription, sortingvoucherlist[i].Comments, DebitAmount, CreditAmount, (sortingvoucherlist[i].DebitAmount > 0.0) ? sortingvoucherlist[i].DCurrencyAmount : 0.0, (sortingvoucherlist[i].CreditAmount > 0.0) ? sortingvoucherlist[i].DCurrencyAmount : 0.0, sortingvoucherlist[i].ReferenceAccount, sortingvoucherlist[i].BranchName, sortingvoucherlist[i].CostCenterName, sortingvoucherlist[i].TaxAmount);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				grdDetail.RootTable.Columns["BranchNAme"].Visible = BranchFeature;
				grdDetail.RootTable.Columns["CostCenter"].Visible = IsBookingOffice;
				grdDetail.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
				grdDetail.RootTable.Columns["DebitFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
				grdDetail.RootTable.Columns["CreditFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
				grdDetail.RootTable.Columns["TaxAmount"].Visible = GridEX_Helper.GetColumnSum(grdDetail, "TaxAmount") > 0.0;
				grdDetail.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdDetail.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdDetail.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdDetail.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdDetail.RootTable.Columns["DebitFcyAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["DebitFcyAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["DebitFcyAmount"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["DebitFcyAmount"].TotalFormatString = "#,##0.###";
				grdDetail.RootTable.Columns["CreditFcyAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["CreditFcyAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["CreditFcyAmount"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["CreditFcyAmount"].TotalFormatString = "#,##0.###";
				grdDetail.RootTable.Columns["CreditAmount"].Caption = "Credit";
				grdDetail.RootTable.Columns["DebitAmount"].Caption = "Debit";
				grdDetail.RootTable.Columns["DebitFcyAmount"].Caption = "Fcy Debit";
				grdDetail.RootTable.Columns["CreditFcyAmount"].Caption = "Fcy Credit";
				CommonServices.GridAutoAdjustmentNew(grdDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		RecId = 0;
		Close();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.TabPages.Count == 2)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					if (tabControl1.SelectedTab.Name == "Cpv")
					{
						FromDateCpvHistory.Focus();
					}
					else
					{
						FromDateBpvHistory.Focus();
					}
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				FromDateCpvHistory.Focus();
			}
			else if (tabControl1.SelectedIndex == 2)
			{
				FromDateBpvHistory.Focus();
			}
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
			GenerateReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport()
	{
		try
		{
			CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			if (ChkBoxWthHolding.Checked)
			{
				TaxScheduleMain taxSchedule = new TaxScheduleMain();
				taxSchedule.OrganizationId = UserAccount.OrganizationId;
				taxSchedule.CompanyId = UserAccount.CompanyId;
				taxSchedule.EffectedDate = voucherdatetime.Value;
				taxSchedule.TaxNameId = Conversion.ToInt(CmbTaxType.Value);
				if (TaxScheduleMain.ReadTaxSchedule(taxSchedule).Rows.Count == 0)
				{
					((Control)(object)CmbAgainstAc).Enabled = false;
					((Control)(object)CmbWithHoldingAc).Enabled = false;
					((Control)(object)CmbWithHoldingAc).Text = string.Empty;
					((Control)(object)CmbAgainstAc).Text = string.Empty;
					txtTaxPercent.Text = "0";
					txtTaxAmount.Text = "0";
				}
				else
				{
					DataTable detail = TaxScheduleMain.ReadTaxSchedule(taxSchedule);
					txtTaxPercent.Text = detail.Rows[0]["TaxPercent"].ToString();
					if (!UpdateMode)
					{
						CmbWithHoldingAc.Value = detail.Rows[0]["TaxGLAccountId"];
						((Control)(object)CmbWithHoldingAc).Text = detail.Rows[0]["AccountTitle"].ToString();
					}
					((Control)(object)CmbAgainstAc).Enabled = true;
					((Control)(object)CmbWithHoldingAc).Enabled = true;
					if (grd.RowCount > 0 && Conversion.ToInt(CmbAgainstAc.Value) == 0)
					{
						int AcId = Conversion.ToInt(grd.GetRow(0).Cells["AccountId"].Value);
						CmbAgainstAc.Value = AcId;
					}
				}
			}
			else
			{
				((Control)(object)CmbAgainstAc).Enabled = false;
				((Control)(object)CmbWithHoldingAc).Enabled = false;
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
				double Amt = Conversion.ToDouble((object)txtValue.Text);
				double Tax = Conversion.ToDouble((object)txtTaxAmount.Text);
				txtTotalAmount.Text = (Amt + Tax).ToString(clsGlobalVariables.stringFormatsingle);
			}
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			checkBox1_CheckedChanged(null, null);
			txtSrbTaxAmount_TextChanged(null, null);
			txtDiscountAmount_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellEdited(object sender, ColumnActionEventArgs e)
	{
		try
		{
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbVoucherType.Value) == 2)
			{
				ChequeBookEnableStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("CheqBook Enabled"));
			}
			List<string> allServices = new List<string> { "MultiCurrency" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			BranchesFill();
			combojoblotfill();
			LocationType();
			AccountsComboBind();
			DetailAccountFill();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				GetSubsidoryAccounts();
				BindSubsidiaryAccount();
			}
			CurrencyFill();
			AccountTitleFill();
			BindTaxTypes();
			DefaultConfigurations();
			SpecialRightsImplement();
			GetFinancialInstrumentTypes();
			CostCenterFill();
			grdCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void PaymentVoucherNew_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.TabPages.Count == 2)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						if (Save.Visible && Save.Enabled)
						{
							Save_Click(null, null);
						}
					}
					else if (tabControl1.SelectedIndex == 1)
					{
						if (tabControl1.SelectedTab.Name == "Cpv")
						{
							btnShowCpv_Click(null, null);
						}
						else
						{
							btnShowHistoryBpv_Click(null, null);
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					if (Save.Visible && Save.Enabled)
					{
						Save_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnShowCpv_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					btnShowHistoryBpv_Click(null, null);
				}
			}
			if (Update.Visible && Update.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				Update_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.TabPages.Count == 2)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						btnNew_Click(null, null);
					}
					else if (tabControl1.SelectedIndex == 1)
					{
						if (tabControl1.SelectedTab.Name == "Cpv")
						{
							btnNewCpvHistory_Click(null, null);
						}
						else
						{
							btnNewHistoryBpv_Click(null, null);
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnNewCpvHistory_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					btnNewHistoryBpv_Click(null, null);
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.TabPages.Count == 2)
				{
					if (tabControl1.SelectedIndex == 1)
					{
						tabControl1.SelectedIndex = 0;
						CmbVoucherType.Focus();
					}
					else
					{
						tabControl1.SelectedIndex = 1;
						if (tabControl1.SelectedTab.Name == "Cpv")
						{
							FromDateCpvHistory.Focus();
						}
						else
						{
							FromDateBpvHistory.Focus();
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					tabControl1.SelectedIndex = 1;
					FromDateCpvHistory.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 2;
					FromDateBpvHistory.Focus();
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					tabControl1.SelectedIndex = 0;
					CmbVoucherType.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.TabPages.Count == 2)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						btnRefresh_Click(null, null);
					}
					else if (tabControl1.SelectedIndex == 1)
					{
						if (tabControl1.SelectedTab.Name == "Cpv")
						{
							btnRefreshCpvHistory_Click(null, null);
						}
						else
						{
							btnRefreshHistoryBpv_Click(null, null);
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnRefreshCpvHistory_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					btnRefreshHistoryBpv_Click(null, null);
				}
			}
			if (e.Alt && e.KeyCode == Keys.F1)
			{
				btnPaymentByInvoiceVoucher_Click(null, null);
			}
			else if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
			{
				if (formright.DoHavePrintRights)
				{
					Print_Click(null, null);
				}
			}
			else if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formright.DoHavePrintRights)
			{
				btnPrintOld_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3) && formright.DoHavePrintRights)
			{
				BtnPaymentAndReceiptVoucherSlip_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbVoucherType.Focus();
				if (!((Control)(object)CmbVoucherType).Enabled)
				{
					CmbCreditAccount.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				toolStripButton10_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.TabPages.Count == 2)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						CmbTaxType.Focus();
					}
					else if (tabControl1.SelectedIndex == 1)
					{
						if (tabControl1.SelectedTab.Name == "Cpv")
						{
							((Control)(object)DataGridHistory).Focus();
						}
						else
						{
							((Control)(object)BankVoucherHistoryGrid).Focus();
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					CmbTaxType.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					((Control)(object)BankVoucherHistoryGrid).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.TabPages.Count == 2)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						CmbPaymentType.Focus();
					}
					else if (tabControl1.SelectedIndex == 1)
					{
						if (tabControl1.SelectedTab.Name == "Cpv")
						{
							FromDateCpvHistory.Focus();
						}
						else
						{
							FromDateBpvHistory.Focus();
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					CmbPaymentType.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateCpvHistory.Focus();
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					FromDateBpvHistory.Focus();
				}
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

	private void CmbTaxType_Leave(object sender, EventArgs e)
	{
		try
		{
			checkBox1_CheckedChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPaymentByInvoiceVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "PaymentByInvoiceVoucherNew").ToList();
				if (newList.Count > 0 && Conversion.ToBool((object)newList[0].Value))
				{
					new PaymentByInvoiceVoucherNew(UserAccount).Show();
				}
				else
				{
					MessageBox.Show("Please Check Screen Rights");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			int ID = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(ID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combvtype_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbVoucherType.Value) == 1)
			{
				((Control)(object)CmbCheqNo).Text = string.Empty;
				((UltraGridBase)CmbCheqNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text);
			_ = 0.0;
			CommonServices.UpdateRelatedFieldsCalculation(txtamount, txtFcyAmountDetail, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentVoucherNew_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			RecId = 0;
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			CheqId = 0;
			GridEXRow item = grd.CurrentRow;
			if (item == null)
			{
				return;
			}
			updateDetailIndex = item.RowIndex;
			((Control)(object)CmbPaymentType).Text = item.Cells["PaymentType"].Text.ToString();
			CmbAccountDetail.Value = Conversion.ToInt((object)item.Cells["AccountId"].Value.ToString());
			BindSubsidiaryAccount();
			if (SubsdiaryAccountDatasourceLength == 0)
			{
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("SubsidiaryAccount");
				dt2.Columns.Add("TypeId");
				dt2.Columns.Add("AccountId");
				dt2.Columns.Add("AccountTitle");
				dt2.Rows.Add(Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value), item.Cells["SubsidiaryAccount"].Text, 4, Conversion.ToInt(item.Cells["AccountId"].Value), item.Cells["AccountTitle"].Text);
				DDL.BindDDL(dt2, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			CmbSubsidiaryAccount.Value = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
			CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
			if (Conversion.ToInt(item.Cells["FinancialInstrumentId"].Value) > 0)
			{
				CmbFinancialInstrument.Value = Conversion.ToInt(item.Cells["FinancialInstrumentId"].Value);
			}
			if (Conversion.ToInt(item.Cells["ReferenceAccountId"].Value) > 0)
			{
				CmbReferenceAccount.Value = Conversion.ToInt(item.Cells["ReferenceAccountId"].Value);
			}
			CheqDate.Value = Conversion.ToDateTime(item.Cells["ChequeDate"].Value);
			if (Conversion.ToInt(item.Cells["ChequeId"].Value) > 0 && Conversion.ToString(item.Cells["ChequeNo"].Value) != string.Empty)
			{
				CheqNoValidatingInUpdate = Conversion.ToInt(item.Cells["ChequeNo"].Value);
				if (Save.Visible && Save.Enabled)
				{
					CmbCheqNo.Value = Conversion.ToInt(item.Cells["ChequeId"].Value);
					CheqId = Conversion.ToInt(item.Cells["ChequeId"].Value);
				}
				else
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("ChequeId");
					dt.Columns.Add("ChequeNo");
					dt.Rows.Add(Conversion.ToInt(item.Cells["ChequeId"].Value), item.Cells["ChequeNo"].Value);
					DDL.BindDDL(dt, CmbCheqNo, "ChequeId", "ChequeNo", "Cheque_number", ZeroIndex: false);
					((UltraGridBase)CmbCheqNo).Rows[0].Activate();
					CheqId = Conversion.ToInt(item.Cells["ChequeId"].Value);
				}
			}
			if (Conversion.CheckDateTimeNull(CheqDate.Value))
			{
				CheqDate.Value = DateTime.Now;
			}
			((Control)(object)txtPayTitle).Text = Conversion.ToString(item.Cells["PayeeTitle"].Value);
			cmbChequeType.Value = Conversion.ToInt(item.Cells["ChequeTypeId"].Value);
			cmbBranchName.Value = Conversion.ToInt(item.Cells["BranchId"].Value);
			txtremarks.Text = item.Cells["Remarks"].Value.ToString();
			cmbTcyCodeDetail.Value = Conversion.ToInt(item.Cells["TcyCodeId"].Value);
			txtTcyExchangeRateDetail.Text = Conversion.ToDouble(item.Cells["TcyExchangeRate"].Value).ToString("#,##0.###");
			txtFcyAmountDetail.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString("#,##0.###");
			txtamount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString();
			if (Conversion.ToInt(item.Cells["CostCenterId"].Value) > 0)
			{
				CmbCostCenter.Value = Conversion.ToInt(item.Cells["CostCenterId"].Value);
			}
			CmbPaymentType.Focus();
			Add.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelDetail.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (DocumentTypeId == 2 && ChequeBookSerialWiseOnBpv)
			{
				if (CheqNoValidatingInUpdate > 0 && Conversion.ToInt((object)((Control)(object)CmbCheqNo).Text) != CheqNoValidatingInUpdate)
				{
					int max = (from row in table.AsEnumerable()
						select Conversion.ToInt((object)row.Field<string>("ChequeNo"))).Distinct().ToList().Max();
					GridEXRow nextRow = grd.GetRow(updateDetailIndex + 1);
					if (nextRow != null && (int)nextRow.RowType == 0)
					{
						if (CheqNoValidatingInUpdate < Conversion.ToInt(nextRow.Cells["ChequeNo"].Value))
						{
							throw new Exception("You Cant Update Cheque No Serial To Other Serial No");
						}
					}
					else if (Conversion.ToInt((object)((Control)(object)CmbCheqNo).Text) > max)
					{
						throw new Exception("You Cant Update Cheque No Serial To Other Serial No");
					}
				}
				CheckingChequeNoSerialWise();
			}
			if (Conversion.CheckDateTimeNull(CheqDate.Value))
			{
				CheqDate.Value = DateTime.Now;
			}
			table.Rows[updateDetailIndex]["PaymentType"] = Conversion.ToInt(CmbPaymentType.Value);
			table.Rows[updateDetailIndex]["AccountCode"] = ((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[2].Value;
			table.Rows[updateDetailIndex]["AccountId"] = CmbAccountDetail.Value;
			table.Rows[updateDetailIndex]["AccountTitle"] = ((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[1].Value;
			table.Rows[updateDetailIndex]["GlCurrency"] = ((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[6].Value;
			if (((Control)(object)CmbSubsidiaryAccount).Visible)
			{
				if (((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && ((Control)(object)CmbSubsidiaryAccount).Text != string.Empty)
				{
					table.Rows[updateDetailIndex]["SubsidiaryAccountId"] = CmbSubsidiaryAccount.Value;
					table.Rows[updateDetailIndex]["SubsidiaryAccount"] = ((Control)(object)CmbSubsidiaryAccount).Text;
					table.Rows[updateDetailIndex]["SubsidiaryAccountTypeId"] = ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value;
				}
				else
				{
					table.Rows[updateDetailIndex]["SubsidiaryAccountId"] = CmbAccountDetail.Value;
					table.Rows[updateDetailIndex]["SubsidiaryAccount"] = ((Control)(object)CmbAccountDetail).Text;
					table.Rows[updateDetailIndex]["SubsidiaryAccountTypeId"] = 4;
				}
			}
			else
			{
				table.Rows[updateDetailIndex]["SubsidiaryAccountId"] = 0;
				table.Rows[updateDetailIndex]["SubsidiaryAccount"] = "";
				table.Rows[updateDetailIndex]["SubsidiaryAccountTypeId"] = 0;
			}
			table.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
			table.Rows[updateDetailIndex]["FinancialInstrumentId"] = Conversion.ToInt(CmbFinancialInstrument.Value);
			table.Rows[updateDetailIndex]["FinancialInstrument"] = ((Control)(object)CmbFinancialInstrument).Text;
			table.Rows[updateDetailIndex]["ReferenceAccountId"] = Conversion.ToInt(CmbReferenceAccount.Value);
			table.Rows[updateDetailIndex]["ReferenceAccount"] = ((Conversion.ToInt(CmbReferenceAccount.Value) > 0) ? Conversion.ToString(((UltraDropDownBase)CmbReferenceAccount).SelectedRow.Cells[1].Value) : "");
			table.Rows[updateDetailIndex]["ChequeDate"] = Conversion.ToDateTime((object)CheqDate.Value).ToString("dd-MMM-yy");
			table.Rows[updateDetailIndex]["ChequeId"] = CmbCheqNo.Value;
			table.Rows[updateDetailIndex]["ChequeNo"] = ((Control)(object)CmbCheqNo).Text;
			table.Rows[updateDetailIndex]["PayeeTitle"] = ((Control)(object)txtPayTitle).Text;
			table.Rows[updateDetailIndex]["ChequeTypeId"] = cmbChequeType.Value;
			table.Rows[updateDetailIndex]["Remarks"] = txtremarks.Text;
			table.Rows[updateDetailIndex]["TcyCodeId"] = cmbTcyCodeDetail.Value;
			table.Rows[updateDetailIndex]["TcyCode"] = ((Control)(object)cmbTcyCodeDetail).Text;
			table.Rows[updateDetailIndex]["TcyExchangeRate"] = txtTcyExchangeRateDetail.Text;
			table.Rows[updateDetailIndex]["Amount"] = txtamount.Text;
			table.Rows[updateDetailIndex]["FcyAmount"] = txtFcyAmountDetail.Text;
			table.Rows[updateDetailIndex]["BranchId"] = Conversion.ToInt(cmbBranchName.Value);
			table.Rows[updateDetailIndex]["BranchName"] = ((Control)(object)cmbBranchName).Text;
			table.Rows[updateDetailIndex]["CostCenterId"] = Conversion.ToInt(CmbCostCenter.Value);
			table.Rows[updateDetailIndex]["GlCurrencyId"] = Conversion.ToInt(((UltraDropDownBase)CmbAccountDetail).SelectedRow.Cells[5].Value);
			grd.DataSource = table;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			ResetDetail();
			txtExchangeRate_TextChanged(null, null);
			if (grd.RowCount > 0 && Conversion.ToInt(CmbAgainstAc.Value) == 0)
			{
				int AcId = Conversion.ToInt(grd.GetRow(0).Cells["AccountId"].Value);
				CmbAgainstAc.Value = AcId;
			}
			if (MultiCurrencyGridWorking)
			{
				CreateUpdateMultiCurrencyGridInfo();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtremarks_Enter(object sender, EventArgs e)
	{
		try
		{
			if (!isMouseClick)
			{
				txtremarks.SelectAll();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtremarks_MouseClick(object sender, MouseEventArgs e)
	{
		isMouseClick = true;
		try
		{
			if (e.Button == MouseButtons.Left)
			{
				int index = txtremarks.GetCharIndexFromPosition(e.Location);
				if (index >= 0 && index < txtremarks.Text.Length)
				{
					txtremarks.SelectionStart = index;
					txtremarks.SelectionLength = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPayTitle_Enter(object sender, EventArgs e)
	{
		((Control)(object)txtPayTitle).Select();
	}

	private void btnPrintOld_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AcRptPaymentReceiptsVoucherSlip_102(RecId, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankVoucherHistoryGrid_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int ID = Conversion.ToInt(BankVoucherHistoryGrid.CurrentRow.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(BankVoucherHistoryGrid.CurrentRow.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "Edit")
			{
				Reset();
				ReadById(ID);
			}
			else if (e.Column.Key == "Print")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(ID);
			}
			else if (e.Column.Key == "Print_102")
			{
				CommonServices.AcRptPaymentReceiptsVoucherSlip_102(ID, DocumentTypeId);
			}
			else if (e.Column.Key == "PrintNew")
			{
				CommonServices.PaymentAndReceiptVoucherSlip(ID, DocumentTypeId);
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, ID, DocumentTypeId, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void VoucherDetailBPVByHeaderId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (BankVoucherHistoryGrid.CurrentRow == null || (int)BankVoucherHistoryGrid.CurrentRow.RowType != 0 || BankVoucherHistoryGrid.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			GridEXRow item = BankVoucherHistoryGrid.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			VoucherHead vh = VoucherHead.GetByID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
			DataTable table = new DataTable();
			table.Columns.Add("AccountCode");
			table.Columns.Add("AccountTitle");
			table.Columns.Add("SubsidiaryAccount");
			table.Columns.Add("Job/Lot");
			table.Columns.Add("Remarks");
			table.Columns.Add("DebitAmount", typeof(double));
			table.Columns.Add("CreditAmount", typeof(double));
			table.Columns.Add("ChequeDate", typeof(string));
			table.Columns.Add("ChequeNo", typeof(string));
			table.Columns.Add("PayeeTitle");
			table.Columns.Add("DebitFcyAmount", typeof(double));
			table.Columns.Add("CreditFcyAmount", typeof(double));
			table.Columns.Add("ReferenceAccount");
			table.Columns.Add("BranchName");
			table.Columns.Add("CostCenter");
			table.Columns.Add("TaxAmount", typeof(double));
			table.Rows.Clear();
			for (int i = 0; i < vh.voucherDetailList.Count; i++)
			{
				if (vh.voucherDetailList[i].DebitAmount > 0.0 || vh.voucherDetailList[i].CreditAmount > 0.0)
				{
					string ChequeDate = (((object)vh.voucherDetailList[i].DCheqDate != null) ? Conversion.ToDateTime((object)vh.voucherDetailList[i].DCheqDate).ToShortDateString() : "");
					double DebitAmount = ((vh.voucherDetailList[i].IsTaxable == "False" && Conversion.ToBool((object)vh.InclusiveTax) && vh.voucherDetailList[i].DebitAmount > 0.0) ? (vh.voucherDetailList[i].DebitAmount + vh.voucherDetailList[i].TaxAmount) : vh.voucherDetailList[i].DebitAmount);
					double CreditAmount = ((vh.voucherDetailList[i].IsTaxable == "False" && Conversion.ToBool((object)vh.InclusiveTax) && vh.voucherDetailList[i].CreditAmount > 0.0) ? (vh.voucherDetailList[i].CreditAmount + vh.voucherDetailList[i].TaxAmount) : vh.voucherDetailList[i].CreditAmount);
					table.Rows.Add(vh.voucherDetailList[i].AccountCode, vh.voucherDetailList[i].AccountTitle, vh.voucherDetailList[i].SubsidiaryAccountTitle, vh.voucherDetailList[i].JobLotDescription, vh.voucherDetailList[i].Comments, DebitAmount, CreditAmount, ChequeDate, vh.voucherDetailList[i].CheqNoDetail, vh.voucherDetailList[i].PayeeTitle, (vh.voucherDetailList[i].DebitAmount > 0.0) ? vh.voucherDetailList[i].DCurrencyAmount : 0.0, (vh.voucherDetailList[i].CreditAmount > 0.0) ? vh.voucherDetailList[i].DCurrencyAmount : 0.0, vh.voucherDetailList[i].ReferenceAccount, vh.voucherDetailList[i].BranchName, vh.voucherDetailList[i].CostCenterName, vh.voucherDetailList[i].TaxAmount);
				}
			}
			GridBankDetail.DataSource = table;
			GridBankDetail.RetrieveStructure();
			GridBankDetail.RootTable.Columns["BranchName"].Visible = BranchFeature;
			GridBankDetail.RootTable.Columns["CostCenter"].Visible = IsBookingOffice;
			GridBankDetail.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			GridBankDetail.RootTable.Columns["DebitFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			GridBankDetail.RootTable.Columns["CreditFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			GridBankDetail.RootTable.Columns["TaxAmount"].Visible = GridEX_Helper.GetColumnSum(grdDetail, "TaxAmount") > 0.0;
			GridBankDetail.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			GridBankDetail.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			GridBankDetail.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridBankDetail.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridBankDetail.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			GridBankDetail.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			GridBankDetail.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridBankDetail.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridBankDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			GridBankDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			GridBankDetail.RootTable.Columns["TaxAmount"].FormatString = "#,##0.###";
			GridBankDetail.RootTable.Columns["TaxAmount"].TotalFormatString = "#,##0.###";
			GridBankDetail.RootTable.Columns["DebitFcyAmount"].AggregateFunction = (AggregateFunction)2;
			GridBankDetail.RootTable.Columns["DebitFcyAmount"].TextAlignment = (TextAlignment)3;
			GridBankDetail.RootTable.Columns["DebitFcyAmount"].FormatString = "#,##0.###";
			GridBankDetail.RootTable.Columns["DebitFcyAmount"].TotalFormatString = "#,##0.###";
			GridBankDetail.RootTable.Columns["CreditFcyAmount"].AggregateFunction = (AggregateFunction)2;
			GridBankDetail.RootTable.Columns["CreditFcyAmount"].TextAlignment = (TextAlignment)3;
			GridBankDetail.RootTable.Columns["CreditFcyAmount"].FormatString = "#,##0.###";
			GridBankDetail.RootTable.Columns["CreditFcyAmount"].TotalFormatString = "#,##0.###";
			GridBankDetail.RootTable.Columns["CreditAmount"].Caption = "Credit";
			GridBankDetail.RootTable.Columns["DebitAmount"].Caption = "Debit";
			GridBankDetail.RootTable.Columns["DebitFcyAmount"].Caption = "Fcy Debit";
			GridBankDetail.RootTable.Columns["CreditFcyAmount"].Caption = "Fcy Credit";
			CommonServices.GridAutoAdjustmentNew(GridBankDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankVoucherHistoryGrid_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = BankVoucherHistoryGrid.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)BankVoucherHistoryGrid).Name);
			if (fs != null)
			{
				BankVoucherHistoryGrid.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BankVoucherHistoryGrid_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			int ID = Conversion.ToInt(BankVoucherHistoryGrid.CurrentRow.Cells["Id"].Value);
			ReadById(ID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			VoucherDetailByHeaderId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankVoucherHistoryGrid_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			VoucherDetailBPVByHeaderId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!AfterCloup && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && Conversion.ToInt(CmbSubsidiaryAccount.Value) != 0)
			{
				int GLOfSubSidory = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbAccountDetail.Value = GLOfSubSidory;
				}
			}
			AfterCloup = false;
			if (Conversion.ToInt(CmbAccountDetail.Value) > 0 && Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
			{
				labelSubsidiaryBal.Visible = true;
				labelSubsidiaryBal.Text = "0";
				double ClosingBalance = CommonServices.GetSubsiadiaryBalance(Conversion.ToInt(CmbAccountDetail.Value), Conversion.ToInt(CmbSubsidiaryAccount.Value), voucherdatetime.Value);
				labelSubsidiaryBal.Text = ClosingBalance.ToString("#,#;(#,#);0");
			}
			else
			{
				labelSubsidiaryBal.Visible = false;
				labelSubsidiaryBal.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccount_Valuechanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbAccountDetail.Value) > 0 && Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
			{
				labelSubsidiaryBal.Visible = true;
				labelSubsidiaryBal.Text = "0";
				double ClosingBalance = CommonServices.GetSubsiadiaryBalance(Conversion.ToInt(CmbAccountDetail.Value), Conversion.ToInt(CmbSubsidiaryAccount.Value), voucherdatetime.Value);
				labelSubsidiaryBal.Text = ClosingBalance.ToString("#,#;(#,#);0");
			}
			else
			{
				labelSubsidiaryBal.Visible = false;
				labelSubsidiaryBal.Text = "0";
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
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Alt+F1", "For Open Payment By Invoice Voucher");
			dt.Rows.Add("Alt+1", "For Print I");
			dt.Rows.Add("Alt+2", "For Print II");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On TaxType");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Debit Account in Detail Box");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space || !(grd.CurrentColumn.Key == "Delete"))
				{
					return;
				}
				if (item != null && (int)item.RowType == 0)
				{
					if (ChequeBookSerialWiseOnBpv)
					{
						int max = (from row in table.AsEnumerable()
							select Conversion.ToInt((object)row.Field<string>("ChequeNo"))).Distinct().ToList().Max();
						if (Conversion.ToInt(item.Cells["ChequeNo"].Value) != max)
						{
							throw new Exception("You can't Delete This Row Beacuse This Entry Is Against Serial Wise Cheque No");
						}
						item.Delete();
					}
					else
					{
						item.Delete();
					}
				}
				if (Conversion.ToInt(CmbVoucherType.Value) == 2)
				{
					if (grd.GetRows().Length != 0)
					{
						int CountCheqid = 0;
						GridEXRow[] rows = grd.GetRows();
						foreach (GridEXRow r in rows)
						{
							if (Conversion.ToInt(r.Cells["ChequeId"].Value) > 0)
							{
								CountCheqid += Conversion.ToInt(r.Cells["ChequeId"].Value);
							}
						}
						if (CountCheqid == 0)
						{
							((Control)(object)CmbCreditAccount).Enabled = true;
						}
					}
					else
					{
						((Control)(object)CmbCreditAccount).Enabled = true;
					}
				}
				Total();
				grd.UpdateData();
				if (MultiCurrencyGridWorking)
				{
					CreateUpdateMultiCurrencyGridInfo();
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int ID = 0;
			int documentTypeId = 0;
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (DataGridHistory.GetRows().Count() > 0)
			{
				ID = Conversion.ToInt(item.Cells["Id"].Value);
				documentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			}
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(ID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(ID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(ID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print_102")
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(ID, documentTypeId);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Attachment")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(ID, documentTypeId);
				}
				else if (DataGridHistory.CurrentColumn.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, ID, documentTypeId, base.Name);
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

	private void BankVoucherHistoryGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int ID = 0;
			int documentTypeId = 0;
			GridEXRow item = BankVoucherHistoryGrid.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (BankVoucherHistoryGrid.GetRows().Count() > 0)
			{
				ID = Conversion.ToInt(item.Cells["Id"].Value);
				documentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			}
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(ID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (BankVoucherHistoryGrid.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(ID);
				}
				else if (BankVoucherHistoryGrid.CurrentColumn.Key == "Print")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(ID);
				}
				else if (BankVoucherHistoryGrid.CurrentColumn.Key == "Print_102")
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(ID);
				}
				else if (BankVoucherHistoryGrid.CurrentColumn.Key == "Attachment")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(ID, documentTypeId);
				}
				else if (BankVoucherHistoryGrid.CurrentColumn.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, ID, documentTypeId, base.Name);
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

	private void Add_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "ctrlGrdBar1")
			{
				CmbPaymentType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!shiftTabPressed)
			{
				CmbPaymentType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Amount" && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_EditingCell(object sender, EditingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Amount")
			{
				((Control)(object)grd).KeyPress += GridEX_Amount_KeyPress;
			}
			else
			{
				((Control)(object)grd).KeyPress -= GridEX_Amount_KeyPress;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridEX_Amount_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '.')
		{
			e.Handled = true;
		}
	}

	private void btnCancelDetail_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		try
		{
			if (e.Shift && e.KeyCode == Keys.Tab)
			{
				shiftTabPressed = true;
			}
			else
			{
				shiftTabPressed = false;
			}
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

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtTcyExchangeRateDetail.Text = txtExchangeRate.Text;
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				double TotalFcyAmount = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				txtFcyAmount.Text = TotalFcyAmount.ToString("#,##0.###");
			}
			else
			{
				txtFcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				return;
			}
			cmbTcyCodeDetail.Value = Conversion.ToInt(cmbCurrency.Value);
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				if (Conversion.ToInt(CmbVoucherType.Value) == 0)
				{
					MessageBox.Show("Please Select Voucher Type First");
					CmbVoucherType.Focus();
					return;
				}
				obj.DocumentTypeIds = Conversion.ToString(CmbVoucherType.Value);
				obj.Ids = Conversion.ToString(cmbCurrency.Value);
				DataTable dt = VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(obj);
				if (dt.Rows.Count > 0)
				{
					txtExchangeRate.Text = Conversion.ToDouble(dt.Rows[0]["LastExchRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
					txtTcyExchangeRateDetail.Text = Conversion.ToDouble(dt.Rows[0]["LastExchRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else
				{
					txtExchangeRate.Text = "0";
					txtTcyExchangeRateDetail.Text = "0";
				}
			}
			else
			{
				txtExchangeRate.Text = BaseRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtTcyExchangeRateDetail.Text = BaseRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPaymentAndReceiptVoucherSlip_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbVoucherType.Value) > 0)
			{
				CommonServices.PaymentAndReceiptVoucherSlip(RecId, Conversion.ToInt(CmbVoucherType.Value));
			}
			else
			{
				MessageBox.Show("Please Select Voucher type First");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSpecialRights_Click(object sender, EventArgs e)
	{
		try
		{
			SpecialSreenRightsUserWise obj = new SpecialSreenRightsUserWise(UserAccount);
			obj.ScreenId = ScreenId;
			((Control)(object)obj.cmbScreenName).Enabled = false;
			obj.UserId = Conversion.ToInt((object)UserAccount.ID);
			if (UserAccount.RoleName != "Admin")
			{
				((Control)(object)obj.cmbUserName).Enabled = false;
			}
			obj.Show();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewCpvHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateCpvHistory.Value = DateTime.Now;
			ToDateCpvHistory.Value = DateTime.Now;
			txtFromDocNoCpv.Text = string.Empty;
			txtToDocNoCpv.Text = string.Empty;
			((Control)(object)cmbAccountTitleCpv).Text = string.Empty;
			((UltraGridBase)cmbApprovedStatusCpv).Rows[0].Activate();
			VouchersInfoBoxCash.Visible = false;
			DataGridHistory.ClearStructure();
			grdDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshCpvHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindForCpvHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowCpv_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFillCpv();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistoryBpv_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateBpvHistory.Value = DateTime.Now;
			ToDateBpvHistory.Value = DateTime.Now;
			txtFromDocNoBpvHistory.Text = string.Empty;
			txtToDocNoBpvHistory.Text = string.Empty;
			((Control)(object)CmbAccountTitleBpvHistory).Text = string.Empty;
			((UltraGridBase)cmbApproveStatusBpv).Rows[0].Activate();
			VoucherInfoBoxBank.Visible = false;
			BankVoucherHistoryGrid.ClearStructure();
			GridBankDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistoryBpv_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindForBpvHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistoryBpv_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFillBpv();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSubsidiaryAccount_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloup = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbAccountDetail.Value = GLOfSubSidory;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoCpv_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoCpv_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFromDocNoBpvHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoBpvHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtremarks_Leave(object sender, EventArgs e)
	{
		try
		{
			isMouseClick = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdbtnAccountName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtAccount.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(CmbAccountDetail.Value) > 0)
				{
					Id = Conversion.ToInt(CmbAccountDetail.Value);
				}
				if (rdbtnAccountName.Checked)
				{
					DDL.BindDDL(dtAccount, CmbAccountDetail, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtAccount, CmbAccountDetail, "Id", "AccountCode", "Account Code", ZeroIndex: true);
				}
				if (Id > 0)
				{
					CmbAccountDetail.Value = Id;
					CmbAccountDetail.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdBtnRefAcByTitle_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = (DataTable)((UltraGridBase)CmbReferenceAccount).DataSource;
			if (dt != null && dt.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(CmbReferenceAccount.Value) > 0)
				{
					Id = Conversion.ToInt(CmbReferenceAccount.Value);
				}
				if (rdBtnRefAcByTitle.Checked)
				{
					DDL.BindDDL(dt, CmbReferenceAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				}
				else
				{
					DDL.BindDDL(dt, CmbReferenceAccount, "Id", "AccountCode", "Reference Ac Code", ZeroIndex: false);
				}
				if (Id > 0)
				{
					CmbReferenceAccount.Value = Id;
					CmbReferenceAccount.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyAmountDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text);
			_ = 0.0;
			CommonServices.UpdateRelatedFieldsCalculation(txtamount, txtFcyAmountDetail, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Amounts_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSrbTaxPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSrbTaxAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			checkBox1_CheckedChanged(null, null);
			Total();
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
			if (Conversion.ToDouble((object)txtDiscPercent.Text) > 0.0)
			{
				if (Conversion.ToDouble((object)txtValue.Text) == 0.0)
				{
					CmbPaymentType.Focus();
				}
				CommonServices.UpdateRelatedFieldsCalculation(txtDiscountAmount, txtDiscPercent, Conversion.ToDouble((object)txtValue.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 100, "", 3);
			}
			else
			{
				txtDiscountAmount.Text = "0";
			}
			checkBox1_CheckedChanged(null, null);
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscountAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtDiscountAmount.Text) > 0.0)
			{
				if (Conversion.ToDouble((object)txtValue.Text) == 0.0)
				{
					CmbPaymentType.Focus();
				}
				CommonServices.UpdateRelatedFieldsCalculation(txtDiscountAmount, txtDiscPercent, Conversion.ToDouble((object)txtValue.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 100, "", 3);
			}
			else
			{
				txtDiscPercent.Text = "0";
			}
			checkBox1_CheckedChanged(null, null);
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadExcluded_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			if (grd.RowCount > 0)
			{
				grd.RootTable.Columns["TaxAmount"].Visible = radioButton1.Checked;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaxAmountProportion()
	{
		try
		{
			if (!radioButton1.Checked)
			{
				ResetTaxAmounts(0.0);
				return;
			}
			double totalTaxAmount = Math.Round(Conversion.ToDouble((object)txtTaxAmount.Text), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			if (totalTaxAmount <= 0.0)
			{
				ResetTaxAmounts(0.0);
				return;
			}
			double totalDetailAmount = GridEX_Helper.GetColumnSum(grd, "Amount");
			if (totalDetailAmount <= 0.0)
			{
				ResetTaxAmounts(0.0);
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double amount = Conversion.ToDouble(obj.Cells["Amount"].Value);
				double taxAmount = totalTaxAmount / totalDetailAmount * amount;
				obj.Cells["TaxAmount"].Value = Math.Round(taxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Tax Distribution Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ResetTaxAmounts(double value)
	{
		GridEXRow[] rows = grd.GetRows();
		foreach (GridEXRow obj in rows)
		{
			obj.BeginEdit();
			obj.Cells["TaxAmount"].Value = value;
			obj.EndEdit();
		}
	}

	private void CreateUpdateMultiCurrencyGridInfo()
	{
		try
		{
			int baseCurrencyId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
			MultiCurrency obj = clsGlobalVariables.globalMultiCurrency.FirstOrDefault((MultiCurrency x) => x.Id == baseCurrencyId);
			string localFcyCode = ((obj != null) ? obj.CurrencyCode : null) ?? "";
			Dictionary<int, object[]> existingCurrencyRows = (from r in dtVoucherDetailMultiCurrency.AsEnumerable()
				where r["GlCurrencyId"] != DBNull.Value
				select r).ToDictionary((DataRow r) => Conversion.ToInt(r["GlCurrencyId"]), (DataRow r) => (object[])r.ItemArray.Clone());
			HashSet<int> requiredCurrencyIds = new HashSet<int>();
			if (grd.DataSource is DataTable dtDetail)
			{
				foreach (DataRow dr in dtDetail.Rows)
				{
					if (dr["GlCurrencyId"] != DBNull.Value && Conversion.ToInt(dr["GlCurrencyId"]) != 0)
					{
						requiredCurrencyIds.Add(Conversion.ToInt(dr["GlCurrencyId"]));
					}
				}
			}
			dtVoucherDetailMultiCurrency.Rows.Clear();
			if (((UltraDropDownBase)CmbCreditAccount).SelectedRow != null && ((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[2].Value != null)
			{
				int headerCurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[2].Value);
				requiredCurrencyIds.Add(headerCurrencyId);
			}
			foreach (int glCurrencyId in requiredCurrencyIds)
			{
				DataRow newRow = dtVoucherDetailMultiCurrency.NewRow();
				if (existingCurrencyRows.TryGetValue(glCurrencyId, out var oldValues))
				{
					newRow.ItemArray = (object[])oldValues.Clone();
				}
				else
				{
					if (dtCurrencies == null || dtCurrencies.Rows.Count == 0)
					{
						throw new Exception("Currencies data not loaded.");
					}
					DataRow currencyRow = dtCurrencies.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<int?>("Id") == glCurrencyId);
					string glFcyCode = "";
					double exchangeRate = 0.0;
					if (currencyRow != null)
					{
						glFcyCode = (currencyRow["CurrencyCode"] as string) ?? string.Empty;
						if (currencyRow["ExchangeRate"] != DBNull.Value)
						{
							exchangeRate = Convert.ToDouble(currencyRow["ExchangeRate"]);
						}
					}
					newRow["Id"] = 0;
					newRow["GlCurrencyId"] = glCurrencyId;
					newRow["GlCurrency"] = glFcyCode;
					newRow["ExchangeRate"] = exchangeRate;
				}
				newRow["LocalCurencyId"] = baseCurrencyId;
				newRow["LocalCurrency"] = localFcyCode;
				newRow["To"] = "To";
				if (glCurrencyId == baseCurrencyId)
				{
					newRow["GlCurrency"] = localFcyCode;
					newRow["ExchangeRate"] = 1;
				}
				dtVoucherDetailMultiCurrency.Rows.Add(newRow);
			}
			grdMultiCurrency.DataSource = dtVoucherDetailMultiCurrency;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ValidateMultiCurrencyBeforeSave()
	{
		try
		{
			HashSet<int> requiredCurrencyIds = new HashSet<int>();
			if (grd.DataSource is DataTable dtDetail)
			{
				foreach (DataRow dr in dtDetail.Rows)
				{
					if (dr["GlCurrencyId"] != DBNull.Value)
					{
						requiredCurrencyIds.Add(Conversion.ToInt(dr["GlCurrencyId"]));
					}
				}
			}
			if (((UltraDropDownBase)CmbCreditAccount).SelectedRow != null && ((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[2].Value != null)
			{
				requiredCurrencyIds.Add(Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[2].Value));
			}
			HashSet<int> gridCurrencyIds = (from r in dtVoucherDetailMultiCurrency.AsEnumerable()
				where r["GlCurrencyId"] != DBNull.Value
				select Conversion.ToInt(r["GlCurrencyId"])).ToHashSet();
			List<int> source = requiredCurrencyIds.Except(gridCurrencyIds).ToList();
			List<int> extraInGrid = gridCurrencyIds.Except(requiredCurrencyIds).ToList();
			if (source.Any() || extraInGrid.Any())
			{
				throw new Exception("Multi-currency information is mismatched with voucher details. Please refresh or recheck currency setup.\nBy Clicking Multi Currency Info Button");
			}
			ValidateExchangeRates();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ValidateExchangeRates()
	{
		try
		{
			foreach (DataRow row in dtVoucherDetailMultiCurrency.Rows)
			{
				if (row["ExchangeRate"] == DBNull.Value)
				{
					throw new Exception("Exchange rate cannot be empty in Multi Currency Grid.");
				}
				if (!decimal.TryParse(row["ExchangeRate"].ToString(), out var rate))
				{
					throw new Exception("Invalid exchange rate value.");
				}
				if (rate <= 0m)
				{
					throw new Exception("Exchange rate must be greater than zero in Multi Currency Grid.");
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnCreateUpdateMultiCurrency_Click(object sender, EventArgs e)
	{
		try
		{
			CreateUpdateMultiCurrencyGridInfo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMultiCurrency_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMultiCurrency.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int num = Conversion.ToInt(r.Cells["LocalCurencyId"].Value);
				int GlCurrencyId = Conversion.ToInt(r.Cells["GlCurrencyId"].Value);
				if (num == GlCurrencyId)
				{
					grdMultiCurrency.RootTable.Columns["ExchangeRate"].EditType = (EditType)0;
				}
				else
				{
					grdMultiCurrency.RootTable.Columns["ExchangeRate"].EditType = (EditType)1;
				}
			}
			grdMultiCurrency.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTcyExchangeRateDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			txtTcyExchangeRateDetail.Text = Conversion.ToDecimal((object)txtTcyExchangeRateDetail.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtTcyExchangeRateDetail_TextChanged(object sender, EventArgs e)
	{
		txtamount_TextChanged(null, null);
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_06fc: Expected O, but got Unknown
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Expected O, but got Unknown
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Expected O, but got Unknown
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
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Expected O, but got Unknown
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1c: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Expected O, but got Unknown
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cab: Expected O, but got Unknown
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Expected O, but got Unknown
		//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d87: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dea: Expected O, but got Unknown
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0b: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2c: Expected O, but got Unknown
		//IL_0e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4d: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6e: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef2: Expected O, but got Unknown
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Expected O, but got Unknown
		//IL_0f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4a: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Expected O, but got Unknown
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f97: Expected O, but got Unknown
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Expected O, but got Unknown
		//IL_110e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1118: Expected O, but got Unknown
		//IL_113a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1144: Expected O, but got Unknown
		//IL_11df: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e9: Expected O, but got Unknown
		//IL_122c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1236: Expected O, but got Unknown
		//IL_1329: Unknown result type (might be due to invalid IL or missing references)
		//IL_1333: Expected O, but got Unknown
		//IL_1355: Unknown result type (might be due to invalid IL or missing references)
		//IL_135f: Expected O, but got Unknown
		//IL_13fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1404: Expected O, but got Unknown
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_1451: Expected O, but got Unknown
		//IL_2217: Unknown result type (might be due to invalid IL or missing references)
		//IL_2221: Expected O, but got Unknown
		//IL_222e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2238: Expected O, but got Unknown
		//IL_2245: Unknown result type (might be due to invalid IL or missing references)
		//IL_224f: Expected O, but got Unknown
		//IL_225c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2266: Expected O, but got Unknown
		//IL_2273: Unknown result type (might be due to invalid IL or missing references)
		//IL_227d: Expected O, but got Unknown
		//IL_d0a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_d0b1: Expected O, but got Unknown
		//IL_d0d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_d0df: Expected O, but got Unknown
		//IL_f8b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_f8bd: Expected O, but got Unknown
		//IL_f8e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_f8eb: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.PaymentVoucherNew));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdMultiCurrency_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
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
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
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
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
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
		Appearance appearance132 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
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
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
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
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
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
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		GridEXLayout GridBankDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout BankVoucherHistoryGrid_DesignTimeLayout = new GridEXLayout();
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
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btn = new System.Windows.Forms.ToolStripButton();
		this.btnE = new System.Windows.Forms.ToolStripButton();
		this.btns = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.bt = new System.Windows.Forms.ToolStripButton();
		this.btnCopy = new System.Windows.Forms.ToolStripButton();
		this.btnPaste = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel23 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdMultiCurrency = new GridEX();
		this.radioButton1 = new System.Windows.Forms.RadioButton();
		this.RadExcluded = new System.Windows.Forms.RadioButton();
		this.txtDiscountAmount = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.txtDiscPercent = new System.Windows.Forms.TextBox();
		this.CmbDiscountAccount = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.txtSrbTaxAmount = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.txtSrbTaxPercent = new System.Windows.Forms.TextBox();
		this.CmbSrbTaxAccount = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.CmbTaxType = new UltraCombo();
		this.ChkBoxWthHolding = new System.Windows.Forms.CheckBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtValue = new System.Windows.Forms.TextBox();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.CmbWithHoldingAc = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.ParentPanelOfMainDetail = new System.Windows.Forms.Panel();
		this.groupboxdetail = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.PaymentTypePanel = new System.Windows.Forms.Panel();
		this.CmbPaymentType = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.DebitAccountPanel = new System.Windows.Forms.Panel();
		this.rdbtnAccountCode = new System.Windows.Forms.RadioButton();
		this.rdbtnAccountName = new System.Windows.Forms.RadioButton();
		this.CmbAccountDetail = new UltraCombo();
		this.lblbalancedetail = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.SubsidiaryAcPanel = new System.Windows.Forms.Panel();
		this.CmbSubsidiaryAccount = new UltraCombo();
		this.labelSubsidiaryBal = new System.Windows.Forms.Label();
		this.lblSubsidiaryaccount = new System.Windows.Forms.Label();
		this.JobLotPanel = new System.Windows.Forms.Panel();
		this.CmbJobLot = new UltraCombo();
		this.lblJoblot = new System.Windows.Forms.Label();
		this.RemarksPanel = new System.Windows.Forms.Panel();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.lblRemarks = new System.Windows.Forms.Label();
		this.panel26 = new System.Windows.Forms.Panel();
		this.cmbTcyCodeDetail = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.txtTcyExchangeRateDetail = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.FcyAmountPanel = new System.Windows.Forms.Panel();
		this.txtFcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.DebitAmountPanel = new System.Windows.Forms.Panel();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.ReferenceAccountPanel = new System.Windows.Forms.Panel();
		this.rdBtnRefAcByCode = new System.Windows.Forms.RadioButton();
		this.rdBtnRefAcByTitle = new System.Windows.Forms.RadioButton();
		this.CmbReferenceAccount = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.FinancialInstrumentPanel = new System.Windows.Forms.Panel();
		this.CmbFinancialInstrument = new UltraCombo();
		this.label39 = new System.Windows.Forms.Label();
		this.ChequeDatePanel = new System.Windows.Forms.Panel();
		this.CheqDate = new System.Windows.Forms.DateTimePicker();
		this.lblCheqDate = new System.Windows.Forms.Label();
		this.ChequeNoPanel = new System.Windows.Forms.Panel();
		this.CmbCheqNo = new UltraCombo();
		this.lblCheqNo = new System.Windows.Forms.Label();
		this.PayeeTitlePanel = new System.Windows.Forms.Panel();
		this.txtPayTitle = new UltraCombo();
		this.lblPayTitle = new System.Windows.Forms.Label();
		this.ChequeTypePanel = new System.Windows.Forms.Panel();
		this.cmbChequeType = new UltraCombo();
		this.labelChequeType = new System.Windows.Forms.Label();
		this.BranchPanel = new System.Windows.Forms.Panel();
		this.cmbBranchName = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.CostCenterPanel = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.panel24 = new System.Windows.Forms.Panel();
		this.Add = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtBaseFcyCode = new System.Windows.Forms.TextBox();
		this.lblBaseFcyCode = new System.Windows.Forms.Label();
		this.txtBaseFcyExchangeRate = new System.Windows.Forms.TextBox();
		this.lblBaseFcyExchangeRate = new System.Windows.Forms.Label();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.CmbProjectId = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.cmbCurrency = new UltraCombo();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.CmbVoucherType = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbAgainstAc = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtbalance = new System.Windows.Forms.Label();
		this.txtremarksmain = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbCreditAccount = new UltraCombo();
		this.txtvoucherno = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.voucherdatetime = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.cbPrintOnSave = new System.Windows.Forms.CheckBox();
		this.PaymentVoucherLable = new System.Windows.Forms.Label();
		this.ChkPrint3 = new System.Windows.Forms.CheckBox();
		this.ChkPrint2 = new System.Windows.Forms.CheckBox();
		this.ChkPrint1 = new System.Windows.Forms.CheckBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.Update = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnPrintOld = new System.Windows.Forms.ToolStripButton();
		this.BtnPaymentAndReceiptVoucherSlip = new System.Windows.Forms.ToolStripButton();
		this.btnPaymentByInvoiceVoucher = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnSpecialRights = new System.Windows.Forms.ToolStripButton();
		this.btnCreateUpdateMultiCurrency = new System.Windows.Forms.ToolStripButton();
		this.Cpv = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label35 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.VouchersInfoBoxCash = new System.Windows.Forms.GroupBox();
		this.label42 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchers = new System.Windows.Forms.Label();
		this.lblTotalVouchers = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.lblApprovedVouchers = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label26 = new System.Windows.Forms.Label();
		this.cmbAccountTitleCpv = new UltraCombo();
		this.btnShowCpv = new System.Windows.Forms.Button();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.cmbApprovedStatusCpv = new UltraCombo();
		this.txtToDocNoCpv = new System.Windows.Forms.TextBox();
		this.txtFromDocNoCpv = new System.Windows.Forms.TextBox();
		this.ToDateCpvHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateCpvHistory = new System.Windows.Forms.DateTimePicker();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNewCpvHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshCpvHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.panel7 = new System.Windows.Forms.Panel();
		this.button1 = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.cmbdoctype = new System.Windows.Forms.ComboBox();
		this.Bpv = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel22 = new System.Windows.Forms.Panel();
		this.GridBankDetail = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.BankVoucherHistoryGrid = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.rdbpvapprovdate = new System.Windows.Forms.RadioButton();
		this.rdbpvmodifydate = new System.Windows.Forms.RadioButton();
		this.rdbpvdocdate = new System.Windows.Forms.RadioButton();
		this.rdbpventrydate = new System.Windows.Forms.RadioButton();
		this.VoucherInfoBoxBank = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchersBpv = new System.Windows.Forms.Label();
		this.lblTotalVouchersBpv = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.lblApprovedVouchersBpv = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbAccountTitleBpvHistory = new UltraCombo();
		this.btnShowHistoryBpv = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbApproveStatusBpv = new UltraCombo();
		this.txtToDocNoBpvHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoBpvHistory = new System.Windows.Forms.TextBox();
		this.ToDateBpvHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateBpvHistory = new System.Windows.Forms.DateTimePicker();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistoryBpv = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistoryBpv = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAllFromBrvTab = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.lblCrAcGlCurrency = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSrbTaxAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWithHoldingAc).BeginInit();
		this.ParentPanelOfMainDetail.SuspendLayout();
		this.groupboxdetail.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.PaymentTypePanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentType).BeginInit();
		this.DebitAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountDetail).BeginInit();
		this.SubsidiaryAcPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).BeginInit();
		this.JobLotPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		this.RemarksPanel.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTcyCodeDetail).BeginInit();
		this.panel25.SuspendLayout();
		this.FcyAmountPanel.SuspendLayout();
		this.DebitAmountPanel.SuspendLayout();
		this.ReferenceAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).BeginInit();
		this.FinancialInstrumentPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinancialInstrument).BeginInit();
		this.ChequeDatePanel.SuspendLayout();
		this.ChequeNoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqNo).BeginInit();
		this.PayeeTitlePanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtPayTitle).BeginInit();
		this.ChequeTypePanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbChequeType).BeginInit();
		this.BranchPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		this.CostCenterPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		this.panel24.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVoucherType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgainstAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.Cpv.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel18.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.VouchersInfoBoxCash.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitleCpv).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusCpv).BeginInit();
		this.panel13.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.Bpv.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridBankDetail).BeginInit();
		this.panel21.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.BankVoucherHistoryGrid).BeginInit();
		this.panel5.SuspendLayout();
		this.VoucherInfoBoxBank.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleBpvHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatusBpv).BeginInit();
		this.panel15.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(718, 1);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1830, 27);
		this.miniToolStrip.TabIndex = 46;
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btn, this.btnE, this.btns, this.btnPrint, this.toolStripSeparator, this.bt, this.btnCopy, this.btnPaste, this.toolStripSeparator1, this.toolStripButton1,
			this.toolStripButton2, this.helpToolStripButton
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1830, 27);
		this.toolStrip1.TabIndex = 46;
		this.toolStrip1.Text = "toolStrip1";
		this.btn.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(55, 24);
		this.btn.Text = "&New";
		this.btnE.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnE.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnE.Name = "btnE";
		this.btnE.Size = new System.Drawing.Size(51, 24);
		this.btnE.Text = "&Edit";
		this.btns.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btns.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btns.Name = "btns";
		this.btns.Size = new System.Drawing.Size(55, 24);
		this.btns.Text = "&Save";
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 24);
		this.btnPrint.Text = "&Print";
		this.bt.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.bt.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.bt.Name = "bt";
		this.bt.Size = new System.Drawing.Size(64, 24);
		this.bt.Text = "&Delete";
		this.btnCopy.Image = Architecture.WinApp.Properties.Resources.icons8_copy_16;
		this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCopy.Name = "btnCopy";
		this.btnCopy.Size = new System.Drawing.Size(59, 24);
		this.btnCopy.Text = "&Copy";
		this.btnPaste.Image = (System.Drawing.Image)resources.GetObject("btnPaste.Image");
		this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPaste.Name = "btnPaste";
		this.btnPaste.Size = new System.Drawing.Size(59, 24);
		this.btnPaste.Text = "&Paste";
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton1.Text = "&Attachment";
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton2.Text = "&SMS";
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(56, 24);
		this.helpToolStripButton.Text = "He&lp";
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1553, 813);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.Cpv);
		this.tabControl1.Controls.Add(this.Bpv);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1553, 813);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.panel6);
		this.Form.Controls.Add(this.ParentPanelOfMainDetail);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(1545, 783);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel23);
		this.panel6.Controls.Add(this.panel17);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 361);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1541, 420);
		this.panel6.TabIndex = 1;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1541, 263);
		this.panel23.TabIndex = 1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1541, 263);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellEdited += new ColumnActionEventHandler(grd_CellEdited);
		this.grd.EditingCell += new EditingCellEventHandler(grd_EditingCell);
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdMultiCurrency);
		this.panel17.Controls.Add(this.radioButton1);
		this.panel17.Controls.Add(this.RadExcluded);
		this.panel17.Controls.Add(this.txtDiscountAmount);
		this.panel17.Controls.Add(this.label50);
		this.panel17.Controls.Add(this.label51);
		this.panel17.Controls.Add(this.txtDiscPercent);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountAccount);
		this.panel17.Controls.Add(this.label52);
		this.panel17.Controls.Add(this.txtSrbTaxAmount);
		this.panel17.Controls.Add(this.label44);
		this.panel17.Controls.Add(this.label48);
		this.panel17.Controls.Add(this.txtSrbTaxPercent);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSrbTaxAccount);
		this.panel17.Controls.Add(this.label49);
		this.panel17.Controls.Add(this.txtTotalAmount);
		this.panel17.Controls.Add(this.txtTaxAmount);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.panel17.Controls.Add(this.ChkBoxWthHolding);
		this.panel17.Controls.Add(this.label8);
		this.panel17.Controls.Add(this.label23);
		this.panel17.Controls.Add(this.label12);
		this.panel17.Controls.Add(this.txtValue);
		this.panel17.Controls.Add(this.txtTaxPercent);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc);
		this.panel17.Controls.Add(this.label9);
		this.panel17.Controls.Add(this.label4);
		this.panel17.Controls.Add(this.label21);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel17.Location = new System.Drawing.Point(0, 263);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1541, 157);
		this.panel17.TabIndex = 0;
		((ScrollableBase)this.grdMultiCurrency).BackColor = System.Drawing.Color.White;
		grdMultiCurrency_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMultiCurrency.DesignTimeLayout = grdMultiCurrency_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Dock = System.Windows.Forms.DockStyle.Left;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMultiCurrency.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Name = "grdMultiCurrency";
		((ScrollableBase)this.grdMultiCurrency).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Size = new System.Drawing.Size(338, 157);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabIndex = 59;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabStop = false;
		this.grdMultiCurrency.TotalRow = (InheritableBoolean)1;
		this.grdMultiCurrency.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMultiCurrency).VisualStyle = (VisualStyle)4;
		this.grdMultiCurrency.SelectionChanged += new System.EventHandler(grdMultiCurrency_SelectionChanged);
		this.radioButton1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.radioButton1.AutoSize = true;
		this.radioButton1.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButton1.Location = new System.Drawing.Point(1199, 8);
		this.radioButton1.Name = "radioButton1";
		this.radioButton1.Size = new System.Drawing.Size(110, 17);
		this.radioButton1.TabIndex = 57;
		this.radioButton1.Text = "Included Tax";
		this.radioButton1.UseVisualStyleBackColor = true;
		this.RadExcluded.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.RadExcluded.AutoSize = true;
		this.RadExcluded.Checked = true;
		this.RadExcluded.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadExcluded.Location = new System.Drawing.Point(1075, 8);
		this.RadExcluded.Name = "RadExcluded";
		this.RadExcluded.Size = new System.Drawing.Size(112, 17);
		this.RadExcluded.TabIndex = 58;
		this.RadExcluded.TabStop = true;
		this.RadExcluded.Text = "Excluded Tax";
		this.RadExcluded.UseVisualStyleBackColor = true;
		this.RadExcluded.CheckedChanged += new System.EventHandler(RadExcluded_CheckedChanged);
		this.txtDiscountAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtDiscountAmount.BackColor = System.Drawing.Color.White;
		this.txtDiscountAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscountAmount.Location = new System.Drawing.Point(1396, 56);
		this.txtDiscountAmount.Name = "txtDiscountAmount";
		this.txtDiscountAmount.Size = new System.Drawing.Size(142, 25);
		this.txtDiscountAmount.TabIndex = 8;
		this.txtDiscountAmount.Tag = "DiscAmount";
		this.txtDiscountAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountAmount.TextChanged += new System.EventHandler(txtDiscountAmount_TextChanged);
		this.txtDiscountAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Amounts_KeyPress);
		this.label50.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(1315, 53);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(75, 30);
		this.label50.TabIndex = 45;
		this.label50.Text = "Discount Amount";
		this.label51.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(873, 61);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(125, 15);
		this.label51.TabIndex = 46;
		this.label51.Text = "Discount Account (Cr)";
		this.txtDiscPercent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtDiscPercent.BackColor = System.Drawing.Color.White;
		this.txtDiscPercent.Location = new System.Drawing.Point(1240, 56);
		this.txtDiscPercent.Name = "txtDiscPercent";
		this.txtDiscPercent.Size = new System.Drawing.Size(72, 25);
		this.txtDiscPercent.TabIndex = 7;
		this.txtDiscPercent.Tag = "DiscPercnt";
		this.txtDiscPercent.TextChanged += new System.EventHandler(txtDiscPercent_TextChanged);
		this.txtDiscPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Amounts_KeyPress);
		this.CmbDiscountAccount.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbDiscountAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDiscountAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Location = new System.Drawing.Point(1002, 56);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Name = "CmbDiscountAccount";
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Size = new System.Drawing.Size(191, 24);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).TabIndex = 6;
		this.label52.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label52.AutoSize = true;
		this.label52.Location = new System.Drawing.Point(1194, 60);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(47, 17);
		this.label52.TabIndex = 47;
		this.label52.Text = "Disc %";
		this.txtSrbTaxAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtSrbTaxAmount.BackColor = System.Drawing.Color.White;
		this.txtSrbTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrbTaxAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrbTaxAmount.Location = new System.Drawing.Point(1396, 30);
		this.txtSrbTaxAmount.Name = "txtSrbTaxAmount";
		this.txtSrbTaxAmount.Size = new System.Drawing.Size(142, 25);
		this.txtSrbTaxAmount.TabIndex = 5;
		this.txtSrbTaxAmount.Tag = "SrbAmount";
		this.txtSrbTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSrbTaxAmount.TextChanged += new System.EventHandler(txtSrbTaxAmount_TextChanged);
		this.txtSrbTaxAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Amounts_KeyPress);
		this.label44.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1315, 35);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(75, 15);
		this.label44.TabIndex = 39;
		this.label44.Text = "SRB Amount";
		this.label48.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(874, 35);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(119, 15);
		this.label48.TabIndex = 40;
		this.label48.Text = "SRB Tax Account (Cr)";
		this.txtSrbTaxPercent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtSrbTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtSrbTaxPercent.Location = new System.Drawing.Point(1240, 30);
		this.txtSrbTaxPercent.Name = "txtSrbTaxPercent";
		this.txtSrbTaxPercent.Size = new System.Drawing.Size(72, 25);
		this.txtSrbTaxPercent.TabIndex = 4;
		this.txtSrbTaxPercent.Tag = "SRBPrcnt";
		this.txtSrbTaxPercent.Visible = false;
		this.txtSrbTaxPercent.TextChanged += new System.EventHandler(txtSrbTaxPercent_TextChanged);
		this.txtSrbTaxPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Amounts_KeyPress);
		this.CmbSrbTaxAccount.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbSrbTaxAccount).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbSrbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSrbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSrbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSrbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSrbTaxAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSrbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSrbTaxAccount).Location = new System.Drawing.Point(1002, 30);
		((System.Windows.Forms.Control)(object)this.CmbSrbTaxAccount).Name = "CmbSrbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbSrbTaxAccount).Size = new System.Drawing.Size(191, 24);
		((System.Windows.Forms.Control)(object)this.CmbSrbTaxAccount).TabIndex = 3;
		this.label49.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label49.AutoSize = true;
		this.label49.Location = new System.Drawing.Point(1194, 34);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(46, 17);
		this.label49.TabIndex = 41;
		this.label49.Text = "SRB %";
		this.label49.Visible = false;
		this.txtTotalAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(1396, 108);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(142, 25);
		this.txtTotalAmount.TabIndex = 13;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtTaxAmount.BackColor = System.Drawing.Color.White;
		this.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(1396, 82);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(142, 25);
		this.txtTaxAmount.TabIndex = 11;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance250;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance255).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance256).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance256).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance256).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance257).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(724, 82);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(92, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_Leave);
		this.ChkBoxWthHolding.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxWthHolding.AutoSize = true;
		this.ChkBoxWthHolding.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBoxWthHolding.Location = new System.Drawing.Point(821, 84);
		this.ChkBoxWthHolding.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxWthHolding.Name = "ChkBoxWthHolding";
		this.ChkBoxWthHolding.Size = new System.Drawing.Size(57, 21);
		this.ChkBoxWthHolding.TabIndex = 1;
		this.ChkBoxWthHolding.Text = "WHT";
		this.ChkBoxWthHolding.UseVisualStyleBackColor = true;
		this.ChkBoxWthHolding.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1315, 87);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(77, 15);
		this.label8.TabIndex = 12;
		this.label8.Text = "Wht Amount";
		this.label23.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(667, 87);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(50, 15);
		this.label23.TabIndex = 35;
		this.label23.Text = "TaxType";
		this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(874, 87);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(117, 15);
		this.label12.TabIndex = 32;
		this.label12.Text = "WHT Credit Account";
		this.txtValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtValue.BackColor = System.Drawing.Color.White;
		this.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtValue.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtValue.Location = new System.Drawing.Point(1396, 4);
		this.txtValue.Name = "txtValue";
		this.txtValue.ReadOnly = true;
		this.txtValue.Size = new System.Drawing.Size(142, 25);
		this.txtValue.TabIndex = 9;
		this.txtValue.TabStop = false;
		this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtValue.TextChanged += new System.EventHandler(textBox1_TextChanged);
		this.txtTaxPercent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Location = new System.Drawing.Point(1240, 82);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(72, 25);
		this.txtTaxPercent.TabIndex = 34;
		this.txtTaxPercent.TabStop = false;
		this.CmbWithHoldingAc.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbWithHoldingAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWithHoldingAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWithHoldingAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance117).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWithHoldingAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Location = new System.Drawing.Point(1002, 82);
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Name = "CmbWithHoldingAc";
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Size = new System.Drawing.Size(191, 24);
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).TabIndex = 2;
		this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(1315, 113);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(80, 15);
		this.label9.TabIndex = 14;
		this.label9.Text = "Total Amount";
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1315, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(51, 15);
		this.label4.TabIndex = 10;
		this.label4.Text = "Amount";
		this.label21.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label21.AutoSize = true;
		this.label21.Location = new System.Drawing.Point(1194, 86);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(43, 17);
		this.label21.TabIndex = 33;
		this.label21.Text = "Tax %";
		this.ParentPanelOfMainDetail.Controls.Add(this.groupboxdetail);
		this.ParentPanelOfMainDetail.Controls.Add(this.groupBox2);
		this.ParentPanelOfMainDetail.Controls.Add(this.panel4);
		this.ParentPanelOfMainDetail.Controls.Add(this.panel3);
		this.ParentPanelOfMainDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.ParentPanelOfMainDetail.Location = new System.Drawing.Point(2, 2);
		this.ParentPanelOfMainDetail.Margin = new System.Windows.Forms.Padding(2);
		this.ParentPanelOfMainDetail.Name = "ParentPanelOfMainDetail";
		this.ParentPanelOfMainDetail.Size = new System.Drawing.Size(1541, 359);
		this.ParentPanelOfMainDetail.TabIndex = 0;
		this.groupboxdetail.BackColor = System.Drawing.Color.White;
		this.groupboxdetail.Controls.Add(this.flowLayoutPanel1);
		this.groupboxdetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupboxdetail.Location = new System.Drawing.Point(2, 197);
		this.groupboxdetail.Name = "groupboxdetail";
		this.groupboxdetail.Size = new System.Drawing.Size(1172, 162);
		this.groupboxdetail.TabIndex = 6;
		this.groupboxdetail.TabStop = false;
		this.groupboxdetail.Text = "Detail";
		this.flowLayoutPanel1.Controls.Add(this.PaymentTypePanel);
		this.flowLayoutPanel1.Controls.Add(this.DebitAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.SubsidiaryAcPanel);
		this.flowLayoutPanel1.Controls.Add(this.JobLotPanel);
		this.flowLayoutPanel1.Controls.Add(this.RemarksPanel);
		this.flowLayoutPanel1.Controls.Add(this.panel26);
		this.flowLayoutPanel1.Controls.Add(this.panel25);
		this.flowLayoutPanel1.Controls.Add(this.FcyAmountPanel);
		this.flowLayoutPanel1.Controls.Add(this.DebitAmountPanel);
		this.flowLayoutPanel1.Controls.Add(this.ReferenceAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.FinancialInstrumentPanel);
		this.flowLayoutPanel1.Controls.Add(this.ChequeDatePanel);
		this.flowLayoutPanel1.Controls.Add(this.ChequeNoPanel);
		this.flowLayoutPanel1.Controls.Add(this.PayeeTitlePanel);
		this.flowLayoutPanel1.Controls.Add(this.ChequeTypePanel);
		this.flowLayoutPanel1.Controls.Add(this.BranchPanel);
		this.flowLayoutPanel1.Controls.Add(this.CostCenterPanel);
		this.flowLayoutPanel1.Controls.Add(this.panel24);
		this.flowLayoutPanel1.Location = new System.Drawing.Point(1, 15);
		this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(1165, 144);
		this.flowLayoutPanel1.TabIndex = 0;
		this.PaymentTypePanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentType);
		this.PaymentTypePanel.Controls.Add(this.label3);
		this.PaymentTypePanel.Location = new System.Drawing.Point(0, 0);
		this.PaymentTypePanel.Margin = new System.Windows.Forms.Padding(0);
		this.PaymentTypePanel.Name = "PaymentTypePanel";
		this.PaymentTypePanel.Size = new System.Drawing.Size(87, 47);
		this.PaymentTypePanel.TabIndex = 0;
		this.CmbPaymentType.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance297).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance298).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance298).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance298).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance299;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance300).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance300).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance301;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance303).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance304;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance305).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance306).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance306).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance306).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance307).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPaymentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Name = "CmbPaymentType";
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Size = new System.Drawing.Size(86, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 8;
		this.label3.Text = "Payment Type";
		this.DebitAccountPanel.Controls.Add(this.rdbtnAccountCode);
		this.DebitAccountPanel.Controls.Add(this.rdbtnAccountName);
		this.DebitAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountDetail);
		this.DebitAccountPanel.Controls.Add(this.lblbalancedetail);
		this.DebitAccountPanel.Controls.Add(this.label11);
		this.DebitAccountPanel.Location = new System.Drawing.Point(87, 0);
		this.DebitAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitAccountPanel.Name = "DebitAccountPanel";
		this.DebitAccountPanel.Size = new System.Drawing.Size(263, 47);
		this.DebitAccountPanel.TabIndex = 1;
		this.rdbtnAccountCode.AutoSize = true;
		this.rdbtnAccountCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnAccountCode.Location = new System.Drawing.Point(101, 2);
		this.rdbtnAccountCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnAccountCode.Name = "rdbtnAccountCode";
		this.rdbtnAccountCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnAccountCode.TabIndex = 135;
		this.rdbtnAccountCode.Text = "Code";
		this.rdbtnAccountCode.UseVisualStyleBackColor = true;
		this.rdbtnAccountName.AutoSize = true;
		this.rdbtnAccountName.Checked = true;
		this.rdbtnAccountName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnAccountName.Location = new System.Drawing.Point(54, 2);
		this.rdbtnAccountName.Margin = new System.Windows.Forms.Padding(0);
		this.rdbtnAccountName.Name = "rdbtnAccountName";
		this.rdbtnAccountName.Size = new System.Drawing.Size(48, 19);
		this.rdbtnAccountName.TabIndex = 134;
		this.rdbtnAccountName.TabStop = true;
		this.rdbtnAccountName.Text = "Title";
		this.rdbtnAccountName.UseVisualStyleBackColor = true;
		this.rdbtnAccountName.CheckedChanged += new System.EventHandler(rdbtnAccountName_CheckedChanged);
		this.CmbAccountDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAccountDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountDetail).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbAccountDetail).Name = "CmbAccountDetail";
		((System.Windows.Forms.Control)(object)this.CmbAccountDetail).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountDetail).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbAccountDetail).Leave += new System.EventHandler(combactitle_Leave);
		this.lblbalancedetail.AutoSize = true;
		this.lblbalancedetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblbalancedetail.ForeColor = System.Drawing.Color.Red;
		this.lblbalancedetail.Location = new System.Drawing.Point(150, 4);
		this.lblbalancedetail.Name = "lblbalancedetail";
		this.lblbalancedetail.Size = new System.Drawing.Size(48, 14);
		this.lblbalancedetail.TabIndex = 32;
		this.lblbalancedetail.Text = "label8";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(53, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Debit Ac";
		this.SubsidiaryAcPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount);
		this.SubsidiaryAcPanel.Controls.Add(this.labelSubsidiaryBal);
		this.SubsidiaryAcPanel.Controls.Add(this.lblSubsidiaryaccount);
		this.SubsidiaryAcPanel.Location = new System.Drawing.Point(350, 0);
		this.SubsidiaryAcPanel.Margin = new System.Windows.Forms.Padding(0);
		this.SubsidiaryAcPanel.Name = "SubsidiaryAcPanel";
		this.SubsidiaryAcPanel.Size = new System.Drawing.Size(168, 47);
		this.SubsidiaryAcPanel.TabIndex = 2;
		this.CmbSubsidiaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbSubsidiaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSubsidiaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSubsidiaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSubsidiaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Name = "CmbSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).TabIndex = 2;
		this.CmbSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbSubsidiaryAccount_AfterCloseUp);
		this.CmbSubsidiaryAccount.ValueChanged += new System.EventHandler(CmbSubsidiaryAccount_Valuechanged);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Leave += new System.EventHandler(CmbSubsidiaryAccount_Leave);
		this.labelSubsidiaryBal.AutoSize = true;
		this.labelSubsidiaryBal.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelSubsidiaryBal.Location = new System.Drawing.Point(84, 3);
		this.labelSubsidiaryBal.Name = "labelSubsidiaryBal";
		this.labelSubsidiaryBal.Size = new System.Drawing.Size(48, 14);
		this.labelSubsidiaryBal.TabIndex = 32;
		this.labelSubsidiaryBal.Text = "label8";
		this.labelSubsidiaryBal.Visible = false;
		this.lblSubsidiaryaccount.AutoSize = true;
		this.lblSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryaccount.Location = new System.Drawing.Point(0, 3);
		this.lblSubsidiaryaccount.Name = "lblSubsidiaryaccount";
		this.lblSubsidiaryaccount.Size = new System.Drawing.Size(84, 15);
		this.lblSubsidiaryaccount.TabIndex = 38;
		this.lblSubsidiaryaccount.Text = "Subsidiary A/c";
		this.JobLotPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.JobLotPanel.Controls.Add(this.lblJoblot);
		this.JobLotPanel.Location = new System.Drawing.Point(518, 0);
		this.JobLotPanel.Margin = new System.Windows.Forms.Padding(0);
		this.JobLotPanel.Name = "JobLotPanel";
		this.JobLotPanel.Size = new System.Drawing.Size(135, 47);
		this.JobLotPanel.TabIndex = 3;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		this.lblJoblot.AutoSize = true;
		this.lblJoblot.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJoblot.Location = new System.Drawing.Point(0, 2);
		this.lblJoblot.Name = "lblJoblot";
		this.lblJoblot.Size = new System.Drawing.Size(48, 15);
		this.lblJoblot.TabIndex = 12;
		this.lblJoblot.Text = "Job/Lot";
		this.RemarksPanel.Controls.Add(this.txtremarks);
		this.RemarksPanel.Controls.Add(this.lblRemarks);
		this.RemarksPanel.Location = new System.Drawing.Point(653, 0);
		this.RemarksPanel.Margin = new System.Windows.Forms.Padding(0);
		this.RemarksPanel.Name = "RemarksPanel";
		this.RemarksPanel.Size = new System.Drawing.Size(278, 47);
		this.RemarksPanel.TabIndex = 4;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(0, 22);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(278, 23);
		this.txtremarks.TabIndex = 4;
		this.txtremarks.MouseClick += new System.Windows.Forms.MouseEventHandler(txtremarks_MouseClick);
		this.txtremarks.Enter += new System.EventHandler(txtremarks_Enter);
		this.txtremarks.Leave += new System.EventHandler(txtremarks_Leave);
		this.lblRemarks.AutoSize = true;
		this.lblRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRemarks.Location = new System.Drawing.Point(0, 3);
		this.lblRemarks.Name = "lblRemarks";
		this.lblRemarks.Size = new System.Drawing.Size(52, 15);
		this.lblRemarks.TabIndex = 2;
		this.lblRemarks.Text = "Remarks";
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail);
		this.panel26.Controls.Add(this.label54);
		this.panel26.Location = new System.Drawing.Point(931, 0);
		this.panel26.Margin = new System.Windows.Forms.Padding(0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(112, 47);
		this.panel26.TabIndex = 5;
		this.cmbTcyCodeDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbTcyCodeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTcyCodeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTcyCodeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance239).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTcyCodeDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Name = "cmbTcyCodeDetail";
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).TabIndex = 3;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(0, 2);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(55, 15);
		this.label54.TabIndex = 12;
		this.label54.Text = "Tcy Code";
		this.panel25.Controls.Add(this.txtTcyExchangeRateDetail);
		this.panel25.Controls.Add(this.label53);
		this.panel25.Location = new System.Drawing.Point(1043, 0);
		this.panel25.Margin = new System.Windows.Forms.Padding(0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(120, 47);
		this.panel25.TabIndex = 6;
		this.txtTcyExchangeRateDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTcyExchangeRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTcyExchangeRateDetail.Location = new System.Drawing.Point(0, 22);
		this.txtTcyExchangeRateDetail.Name = "txtTcyExchangeRateDetail";
		this.txtTcyExchangeRateDetail.ShortcutsEnabled = false;
		this.txtTcyExchangeRateDetail.Size = new System.Drawing.Size(120, 23);
		this.txtTcyExchangeRateDetail.TabIndex = 5;
		this.txtTcyExchangeRateDetail.Tag = "FcyAmount";
		this.txtTcyExchangeRateDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTcyExchangeRateDetail.TextChanged += new System.EventHandler(txtTcyExchangeRateDetail_TextChanged);
		this.txtTcyExchangeRateDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtTcyExchangeRateDetail.Leave += new System.EventHandler(txtTcyExchangeRateDetail_Leave);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(1, 3);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(105, 15);
		this.label53.TabIndex = 4;
		this.label53.Text = "Tcy Exchange Rate";
		this.FcyAmountPanel.Controls.Add(this.txtFcyAmountDetail);
		this.FcyAmountPanel.Controls.Add(this.label43);
		this.FcyAmountPanel.Location = new System.Drawing.Point(0, 47);
		this.FcyAmountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.FcyAmountPanel.Name = "FcyAmountPanel";
		this.FcyAmountPanel.Size = new System.Drawing.Size(117, 47);
		this.FcyAmountPanel.TabIndex = 7;
		this.txtFcyAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmountDetail.Location = new System.Drawing.Point(0, 22);
		this.txtFcyAmountDetail.Name = "txtFcyAmountDetail";
		this.txtFcyAmountDetail.ShortcutsEnabled = false;
		this.txtFcyAmountDetail.Size = new System.Drawing.Size(117, 23);
		this.txtFcyAmountDetail.TabIndex = 5;
		this.txtFcyAmountDetail.Tag = "FcyAmount";
		this.txtFcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountDetail.TextChanged += new System.EventHandler(txtFcyAmountDetail_TextChanged);
		this.txtFcyAmountDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Amounts_KeyPress);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(1, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(72, 15);
		this.label43.TabIndex = 4;
		this.label43.Text = "Tcy Amount";
		this.DebitAmountPanel.Controls.Add(this.txtamount);
		this.DebitAmountPanel.Controls.Add(this.label20);
		this.DebitAmountPanel.Location = new System.Drawing.Point(117, 47);
		this.DebitAmountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitAmountPanel.Name = "DebitAmountPanel";
		this.DebitAmountPanel.Size = new System.Drawing.Size(117, 47);
		this.DebitAmountPanel.TabIndex = 8;
		this.txtamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtamount.Location = new System.Drawing.Point(1, 22);
		this.txtamount.Name = "txtamount";
		this.txtamount.ShortcutsEnabled = false;
		this.txtamount.Size = new System.Drawing.Size(114, 23);
		this.txtamount.TabIndex = 5;
		this.txtamount.Tag = "DebitAmount";
		this.txtamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtamount.TextChanged += new System.EventHandler(txtamount_TextChanged);
		this.txtamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtamount_KeyPress);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1, 3);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(83, 15);
		this.label20.TabIndex = 4;
		this.label20.Text = "Debit Amount";
		this.ReferenceAccountPanel.Controls.Add(this.rdBtnRefAcByCode);
		this.ReferenceAccountPanel.Controls.Add(this.rdBtnRefAcByTitle);
		this.ReferenceAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceAccount);
		this.ReferenceAccountPanel.Controls.Add(this.label46);
		this.ReferenceAccountPanel.Location = new System.Drawing.Point(234, 47);
		this.ReferenceAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ReferenceAccountPanel.Name = "ReferenceAccountPanel";
		this.ReferenceAccountPanel.Size = new System.Drawing.Size(170, 47);
		this.ReferenceAccountPanel.TabIndex = 9;
		this.rdBtnRefAcByCode.AutoSize = true;
		this.rdBtnRefAcByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdBtnRefAcByCode.Location = new System.Drawing.Point(119, 2);
		this.rdBtnRefAcByCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdBtnRefAcByCode.Name = "rdBtnRefAcByCode";
		this.rdBtnRefAcByCode.Size = new System.Drawing.Size(52, 19);
		this.rdBtnRefAcByCode.TabIndex = 133;
		this.rdBtnRefAcByCode.Text = "Code";
		this.rdBtnRefAcByCode.UseVisualStyleBackColor = true;
		this.rdBtnRefAcByTitle.AutoSize = true;
		this.rdBtnRefAcByTitle.Checked = true;
		this.rdBtnRefAcByTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdBtnRefAcByTitle.Location = new System.Drawing.Point(75, 2);
		this.rdBtnRefAcByTitle.Margin = new System.Windows.Forms.Padding(0);
		this.rdBtnRefAcByTitle.Name = "rdBtnRefAcByTitle";
		this.rdBtnRefAcByTitle.Size = new System.Drawing.Size(48, 19);
		this.rdBtnRefAcByTitle.TabIndex = 132;
		this.rdBtnRefAcByTitle.TabStop = true;
		this.rdBtnRefAcByTitle.Text = "Title";
		this.rdBtnRefAcByTitle.UseVisualStyleBackColor = true;
		this.rdBtnRefAcByTitle.CheckedChanged += new System.EventHandler(rdBtnRefAcByTitle_CheckedChanged);
		this.CmbReferenceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbReferenceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Location = new System.Drawing.Point(2, 21);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Name = "CmbReferenceAccount";
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).TabIndex = 2;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(4, 3);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(76, 15);
		this.label46.TabIndex = 38;
		this.label46.Text = "Reference Ac";
		this.FinancialInstrumentPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinancialInstrument);
		this.FinancialInstrumentPanel.Controls.Add(this.label39);
		this.FinancialInstrumentPanel.Location = new System.Drawing.Point(404, 47);
		this.FinancialInstrumentPanel.Margin = new System.Windows.Forms.Padding(0);
		this.FinancialInstrumentPanel.Name = "FinancialInstrumentPanel";
		this.FinancialInstrumentPanel.Size = new System.Drawing.Size(116, 47);
		this.FinancialInstrumentPanel.TabIndex = 10;
		this.CmbFinancialInstrument.AllowNull = (DefaultableBoolean)1;
		this.CmbFinancialInstrument.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFinancialInstrument.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinancialInstrument.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Appearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance246).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance246).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance248;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance265).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinancialInstrument).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinancialInstrument).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbFinancialInstrument.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFinancialInstrument).Location = new System.Drawing.Point(1, 19);
		((System.Windows.Forms.Control)(object)this.CmbFinancialInstrument).Name = "CmbFinancialInstrument";
		((System.Windows.Forms.Control)(object)this.CmbFinancialInstrument).Size = new System.Drawing.Size(116, 26);
		((System.Windows.Forms.Control)(object)this.CmbFinancialInstrument).TabIndex = 6;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1, 2);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(116, 15);
		this.label39.TabIndex = 39;
		this.label39.Text = "Financial Instrument";
		this.ChequeDatePanel.Controls.Add(this.CheqDate);
		this.ChequeDatePanel.Controls.Add(this.lblCheqDate);
		this.ChequeDatePanel.Location = new System.Drawing.Point(520, 47);
		this.ChequeDatePanel.Margin = new System.Windows.Forms.Padding(0);
		this.ChequeDatePanel.Name = "ChequeDatePanel";
		this.ChequeDatePanel.Size = new System.Drawing.Size(114, 47);
		this.ChequeDatePanel.TabIndex = 11;
		this.CheqDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CheqDate.CustomFormat = "dd-MMM-yy";
		this.CheqDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CheqDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.CheqDate.Location = new System.Drawing.Point(1, 21);
		this.CheqDate.Name = "CheqDate";
		this.CheqDate.Size = new System.Drawing.Size(113, 23);
		this.CheqDate.TabIndex = 7;
		this.lblCheqDate.AutoSize = true;
		this.lblCheqDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCheqDate.Location = new System.Drawing.Point(4, 3);
		this.lblCheqDate.Name = "lblCheqDate";
		this.lblCheqDate.Size = new System.Drawing.Size(75, 15);
		this.lblCheqDate.TabIndex = 33;
		this.lblCheqDate.Text = "Cheque Date";
		this.ChequeNoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCheqNo);
		this.ChequeNoPanel.Controls.Add(this.lblCheqNo);
		this.ChequeNoPanel.Location = new System.Drawing.Point(634, 47);
		this.ChequeNoPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ChequeNoPanel.Name = "ChequeNoPanel";
		this.ChequeNoPanel.Size = new System.Drawing.Size(130, 47);
		this.ChequeNoPanel.TabIndex = 12;
		this.CmbCheqNo.AllowNull = (DefaultableBoolean)1;
		this.CmbCheqNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCheqNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCheqNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCheqNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Location = new System.Drawing.Point(1, 19);
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Name = "CmbCheqNo";
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).TabIndex = 8;
		this.lblCheqNo.AutoSize = true;
		this.lblCheqNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCheqNo.Location = new System.Drawing.Point(0, 1);
		this.lblCheqNo.Name = "lblCheqNo";
		this.lblCheqNo.Size = new System.Drawing.Size(66, 15);
		this.lblCheqNo.TabIndex = 0;
		this.lblCheqNo.Text = "Cheque No";
		this.PayeeTitlePanel.Controls.Add((System.Windows.Forms.Control)(object)this.txtPayTitle);
		this.PayeeTitlePanel.Controls.Add(this.lblPayTitle);
		this.PayeeTitlePanel.Location = new System.Drawing.Point(764, 47);
		this.PayeeTitlePanel.Margin = new System.Windows.Forms.Padding(0);
		this.PayeeTitlePanel.Name = "PayeeTitlePanel";
		this.PayeeTitlePanel.Size = new System.Drawing.Size(158, 47);
		this.PayeeTitlePanel.TabIndex = 13;
		this.txtPayTitle.AllowNull = (DefaultableBoolean)1;
		this.txtPayTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtPayTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtPayTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtPayTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.txtPayTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtPayTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtPayTitle).Enabled = false;
		((System.Windows.Forms.Control)(object)this.txtPayTitle).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.txtPayTitle).Location = new System.Drawing.Point(1, 19);
		((System.Windows.Forms.Control)(object)this.txtPayTitle).Name = "txtPayTitle";
		((System.Windows.Forms.Control)(object)this.txtPayTitle).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.txtPayTitle).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.txtPayTitle).Enter += new System.EventHandler(txtPayTitle_Enter);
		this.lblPayTitle.AutoSize = true;
		this.lblPayTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPayTitle.Location = new System.Drawing.Point(0, 1);
		this.lblPayTitle.Name = "lblPayTitle";
		this.lblPayTitle.Size = new System.Drawing.Size(64, 15);
		this.lblPayTitle.TabIndex = 35;
		this.lblPayTitle.Text = "Payee Title";
		this.ChequeTypePanel.Controls.Add((System.Windows.Forms.Control)(object)this.cmbChequeType);
		this.ChequeTypePanel.Controls.Add(this.labelChequeType);
		this.ChequeTypePanel.Location = new System.Drawing.Point(922, 47);
		this.ChequeTypePanel.Margin = new System.Windows.Forms.Padding(0);
		this.ChequeTypePanel.Name = "ChequeTypePanel";
		this.ChequeTypePanel.Size = new System.Drawing.Size(98, 47);
		this.ChequeTypePanel.TabIndex = 14;
		this.cmbChequeType.AllowNull = (DefaultableBoolean)1;
		this.cmbChequeType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbChequeType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbChequeType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbChequeType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbChequeType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbChequeType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbChequeType).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.cmbChequeType).Location = new System.Drawing.Point(1, 19);
		((System.Windows.Forms.Control)(object)this.cmbChequeType).Name = "cmbChequeType";
		((System.Windows.Forms.Control)(object)this.cmbChequeType).Size = new System.Drawing.Size(98, 26);
		((System.Windows.Forms.Control)(object)this.cmbChequeType).TabIndex = 10;
		this.labelChequeType.AutoSize = true;
		this.labelChequeType.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelChequeType.Location = new System.Drawing.Point(1, 1);
		this.labelChequeType.Name = "labelChequeType";
		this.labelChequeType.Size = new System.Drawing.Size(75, 15);
		this.labelChequeType.TabIndex = 42;
		this.labelChequeType.Text = "Cheque Type";
		this.BranchPanel.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.BranchPanel.Controls.Add(this.label41);
		this.BranchPanel.Location = new System.Drawing.Point(1020, 47);
		this.BranchPanel.Margin = new System.Windows.Forms.Padding(0);
		this.BranchPanel.Name = "BranchPanel";
		this.BranchPanel.Size = new System.Drawing.Size(143, 47);
		this.BranchPanel.TabIndex = 15;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance138;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance198).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(1, 19);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 11;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(1, 1);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(79, 15);
		this.label41.TabIndex = 592;
		this.label41.Text = "Branch Name";
		this.CostCenterPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.CostCenterPanel.Controls.Add(this.label40);
		this.CostCenterPanel.Location = new System.Drawing.Point(0, 94);
		this.CostCenterPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CostCenterPanel.Name = "CostCenterPanel";
		this.CostCenterPanel.Size = new System.Drawing.Size(121, 47);
		this.CostCenterPanel.TabIndex = 16;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(1, 19);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 595;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(2, 1);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(67, 15);
		this.label40.TabIndex = 596;
		this.label40.Text = "Cost Center";
		this.panel24.Controls.Add(this.Add);
		this.panel24.Controls.Add(this.btnCancelDetail);
		this.panel24.Controls.Add(this.btnUpdateDetail);
		this.panel24.Location = new System.Drawing.Point(121, 94);
		this.panel24.Margin = new System.Windows.Forms.Padding(0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(117, 47);
		this.panel24.TabIndex = 17;
		this.Add.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(1, 19);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(26, 25);
		this.Add.TabIndex = 12;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click_1);
		this.Add.Leave += new System.EventHandler(Add_Leave);
		this.btnCancelDetail.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(59, 19);
		this.btnCancelDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(57, 25);
		this.btnCancelDetail.TabIndex = 13;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(btnCancelDetail_Leave);
		this.btnCancelDetail.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(btnCancelDetail_PreviewKeyDown);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1, 19);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(59, 25);
		this.btnUpdateDetail.TabIndex = 12;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.lblCrAcGlCurrency);
		this.groupBox2.Controls.Add(this.txtBaseFcyCode);
		this.groupBox2.Controls.Add(this.lblBaseFcyCode);
		this.groupBox2.Controls.Add(this.txtBaseFcyExchangeRate);
		this.groupBox2.Controls.Add(this.lblBaseFcyExchangeRate);
		this.groupBox2.Controls.Add(this.chkCustomAccounts);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProjectId);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.lblExchangeRate);
		this.groupBox2.Controls.Add(this.lblFcyAmount);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox2.Controls.Add(this.lblFcyCode);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVoucherType);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAgainstAc);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.txtbalance);
		this.groupBox2.Controls.Add(this.txtremarksmain);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.groupBox2.Controls.Add(this.txtvoucherno);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.voucherdatetime);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(1, 68);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1173, 128);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.txtBaseFcyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyCode.Enabled = false;
		this.txtBaseFcyCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyCode.Location = new System.Drawing.Point(1042, 30);
		this.txtBaseFcyCode.Name = "txtBaseFcyCode";
		this.txtBaseFcyCode.ShortcutsEnabled = false;
		this.txtBaseFcyCode.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyCode.TabIndex = 569;
		this.txtBaseFcyCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyCode.AutoSize = true;
		this.lblBaseFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyCode.Location = new System.Drawing.Point(1042, 11);
		this.lblBaseFcyCode.Name = "lblBaseFcyCode";
		this.lblBaseFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblBaseFcyCode.TabIndex = 570;
		this.lblBaseFcyCode.Text = "Fcy Code";
		this.txtBaseFcyExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyExchangeRate.Location = new System.Drawing.Point(1041, 74);
		this.txtBaseFcyExchangeRate.Name = "txtBaseFcyExchangeRate";
		this.txtBaseFcyExchangeRate.ShortcutsEnabled = false;
		this.txtBaseFcyExchangeRate.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyExchangeRate.TabIndex = 567;
		this.txtBaseFcyExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyExchangeRate.AutoSize = true;
		this.lblBaseFcyExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyExchangeRate.Location = new System.Drawing.Point(1041, 56);
		this.lblBaseFcyExchangeRate.Name = "lblBaseFcyExchangeRate";
		this.lblBaseFcyExchangeRate.Size = new System.Drawing.Size(105, 15);
		this.lblBaseFcyExchangeRate.TabIndex = 568;
		this.lblBaseFcyExchangeRate.Text = "Fcy Exchange Rate";
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCustomAccounts.Location = new System.Drawing.Point(915, 97);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(120, 19);
		this.chkCustomAccounts.TabIndex = 10;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.CmbProjectId.AllowNull = (DefaultableBoolean)1;
		this.CmbProjectId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProjectId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProjectId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance286;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance292).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance293).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance293).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance293).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance294).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProjectId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Location = new System.Drawing.Point(111, 11);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Name = "CmbProjectId";
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Size = new System.Drawing.Size(240, 24);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(14, 16);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 566;
		this.label5.Text = "Cost Center";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(924, 40);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(111, 23);
		this.txtExchangeRate.TabIndex = 8;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.lblExchangeRate.AutoSize = true;
		this.lblExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblExchangeRate.Location = new System.Drawing.Point(843, 36);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(79, 30);
		this.lblExchangeRate.TabIndex = 58;
		this.lblExchangeRate.Text = "Tcy Exchange\r\nRate";
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFcyAmount.Location = new System.Drawing.Point(843, 70);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(72, 15);
		this.lblFcyAmount.TabIndex = 57;
		this.lblFcyAmount.Text = "Tcy Amount";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(924, 66);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(111, 23);
		this.txtFcyAmount.TabIndex = 9;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance273;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance278).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance279).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance279).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance279).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance281).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(924, 10);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(843, 16);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 56;
		this.lblFcyCode.Text = "Tcy Code";
		this.CmbVoucherType.AllowNull = (DefaultableBoolean)1;
		this.CmbVoucherType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVoucherType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVoucherType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance311).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance311).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance311).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVoucherType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance312;
		((SpecialBoxBase)((UltraGridBase)this.CmbVoucherType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVoucherType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVoucherType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbVoucherType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVoucherType).Location = new System.Drawing.Point(111, 37);
		((System.Windows.Forms.Control)(object)this.CmbVoucherType).Name = "CmbVoucherType";
		((System.Windows.Forms.Control)(object)this.CmbVoucherType).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbVoucherType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbVoucherType).TextChanged += new System.EventHandler(combvtype_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbVoucherType).Leave += new System.EventHandler(combvtype_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(14, 43);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(79, 15);
		this.label14.TabIndex = 6;
		this.label14.Text = "Voucher Type";
		this.CmbAgainstAc.AllowNull = (DefaultableBoolean)1;
		this.CmbAgainstAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAgainstAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAgainstAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAgainstAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Location = new System.Drawing.Point(465, 10);
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Name = "CmbAgainstAc";
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Size = new System.Drawing.Size(373, 26);
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).TabIndex = 5;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(373, 16);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(84, 15);
		this.label13.TabIndex = 32;
		this.label13.Text = "WHT Debit Ac";
		this.txtbalance.AutoSize = true;
		this.txtbalance.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbalance.ForeColor = System.Drawing.Color.Red;
		this.txtbalance.Location = new System.Drawing.Point(354, 96);
		this.txtbalance.Name = "txtbalance";
		this.txtbalance.Size = new System.Drawing.Size(45, 17);
		this.txtbalance.TabIndex = 30;
		this.txtbalance.Text = "label8";
		this.txtremarksmain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksmain.Location = new System.Drawing.Point(465, 41);
		this.txtremarksmain.Multiline = true;
		this.txtremarksmain.Name = "txtremarksmain";
		this.txtremarksmain.Size = new System.Drawing.Size(373, 76);
		this.txtremarksmain.TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(373, 39);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 16);
		this.label1.TabIndex = 6;
		this.label1.Text = "Remarks";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(111, 91);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Leave += new System.EventHandler(combcreditac_Leave);
		this.txtvoucherno.BackColor = System.Drawing.Color.White;
		this.txtvoucherno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvoucherno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvoucherno.Location = new System.Drawing.Point(111, 66);
		this.txtvoucherno.Name = "txtvoucherno";
		this.txtvoucherno.ReadOnly = true;
		this.txtvoucherno.Size = new System.Drawing.Size(74, 23);
		this.txtvoucherno.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(14, 97);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "Credit Account";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(14, 70);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(70, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Voucher No";
		this.voucherdatetime.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.voucherdatetime.CustomFormat = "dd-MMM-yy";
		this.voucherdatetime.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.voucherdatetime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.voucherdatetime.Location = new System.Drawing.Point(229, 66);
		this.voucherdatetime.Name = "voucherdatetime";
		this.voucherdatetime.Size = new System.Drawing.Size(122, 23);
		this.voucherdatetime.TabIndex = 3;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(190, 70);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(32, 15);
		this.label16.TabIndex = 2;
		this.label16.Text = "Date";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.labelLocationType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel4.Controls.Add(this.cbPrintOnSave);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.PaymentVoucherLable);
		this.panel4.Controls.Add(this.ChkPrint3);
		this.panel4.Controls.Add(this.ChkPrint2);
		this.panel4.Controls.Add(this.ChkPrint1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1541, 36);
		this.panel4.TabIndex = 0;
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(255, 11);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 502;
		this.labelLocationType.Text = "Location Type";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(344, 5);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 501;
		this.cbPrintOnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.cbPrintOnSave.AutoSize = true;
		this.cbPrintOnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cbPrintOnSave.ForeColor = System.Drawing.Color.White;
		this.cbPrintOnSave.Location = new System.Drawing.Point(928, 6);
		this.cbPrintOnSave.Name = "cbPrintOnSave";
		this.cbPrintOnSave.Size = new System.Drawing.Size(183, 20);
		this.cbPrintOnSave.TabIndex = 43;
		this.cbPrintOnSave.Text = "Cheque Print On Save";
		this.cbPrintOnSave.UseVisualStyleBackColor = true;
		this.PaymentVoucherLable.AutoSize = true;
		this.PaymentVoucherLable.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PaymentVoucherLable.ForeColor = System.Drawing.Color.White;
		this.PaymentVoucherLable.Location = new System.Drawing.Point(7, 8);
		this.PaymentVoucherLable.Name = "PaymentVoucherLable";
		this.PaymentVoucherLable.Size = new System.Drawing.Size(175, 19);
		this.PaymentVoucherLable.TabIndex = 0;
		this.PaymentVoucherLable.Text = "PAYMENT VOUCHER";
		this.ChkPrint3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint3.AutoSize = true;
		this.ChkPrint3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint3.ForeColor = System.Drawing.Color.White;
		this.ChkPrint3.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint3.Location = new System.Drawing.Point(1363, 8);
		this.ChkPrint3.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint3.Name = "ChkPrint3";
		this.ChkPrint3.Size = new System.Drawing.Size(126, 19);
		this.ChkPrint3.TabIndex = 10;
		this.ChkPrint3.TabStop = false;
		this.ChkPrint3.Text = "Preview Format II";
		this.ChkPrint3.UseVisualStyleBackColor = true;
		this.ChkPrint2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint2.AutoSize = true;
		this.ChkPrint2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint2.ForeColor = System.Drawing.Color.White;
		this.ChkPrint2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint2.Location = new System.Drawing.Point(1236, 8);
		this.ChkPrint2.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint2.Name = "ChkPrint2";
		this.ChkPrint2.Size = new System.Drawing.Size(126, 19);
		this.ChkPrint2.TabIndex = 10;
		this.ChkPrint2.TabStop = false;
		this.ChkPrint2.Text = "Preview Format II";
		this.ChkPrint2.UseVisualStyleBackColor = true;
		this.ChkPrint1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint1.AutoSize = true;
		this.ChkPrint1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint1.ForeColor = System.Drawing.Color.White;
		this.ChkPrint1.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint1.Location = new System.Drawing.Point(1128, 8);
		this.ChkPrint1.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint1.Name = "ChkPrint1";
		this.ChkPrint1.Size = new System.Drawing.Size(102, 19);
		this.ChkPrint1.TabIndex = 9;
		this.ChkPrint1.TabStop = false;
		this.ChkPrint1.Text = "Print Preview";
		this.ChkPrint1.UseVisualStyleBackColor = true;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1541, 30);
		this.panel3.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.New, this.btnRefresh, this.Update, this.Save, this.toolStripButton10, this.btnDelete, this.Print, this.btnPrintOld, this.BtnPaymentAndReceiptVoucherSlip, this.btnPaymentByInvoiceVoucher,
			this.btnShortCutKey, this.btnSpecialRights, this.btnCreateUpdateMultiCurrency
		});
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1541, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(59, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.Update.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.Update.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Update.Name = "Update";
		this.Update.Size = new System.Drawing.Size(78, 24);
		this.Update.Text = "&Update";
		this.Update.Click += new System.EventHandler(Update_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(64, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.toolStripButton10.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton10.Name = "toolStripButton10";
		this.toolStripButton10.Size = new System.Drawing.Size(109, 24);
		this.toolStripButton10.Text = "&Attachment";
		this.toolStripButton10.Click += new System.EventHandler(toolStripButton10_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "102-&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnPrintOld.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintOld.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintOld.Name = "btnPrintOld";
		this.btnPrintOld.Size = new System.Drawing.Size(125, 24);
		this.btnPrintOld.Text = "102-Format-II";
		this.btnPrintOld.Click += new System.EventHandler(btnPrintOld_Click);
		this.BtnPaymentAndReceiptVoucherSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPaymentAndReceiptVoucherSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPaymentAndReceiptVoucherSlip.Name = "BtnPaymentAndReceiptVoucherSlip";
		this.BtnPaymentAndReceiptVoucherSlip.Size = new System.Drawing.Size(77, 24);
		this.BtnPaymentAndReceiptVoucherSlip.Text = "102-III";
		this.BtnPaymentAndReceiptVoucherSlip.Click += new System.EventHandler(BtnPaymentAndReceiptVoucherSlip_Click);
		this.btnPaymentByInvoiceVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPaymentByInvoiceVoucher.Name = "btnPaymentByInvoiceVoucher";
		this.btnPaymentByInvoiceVoucher.Size = new System.Drawing.Size(200, 24);
		this.btnPaymentByInvoiceVoucher.Text = "Payment By Invoice Voucher";
		this.btnPaymentByInvoiceVoucher.Visible = false;
		this.btnPaymentByInvoiceVoucher.Click += new System.EventHandler(btnPaymentByInvoiceVoucher_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnSpecialRights.Image = Architecture.WinApp.Properties.Resources.RightsIcon;
		this.btnSpecialRights.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSpecialRights.Name = "btnSpecialRights";
		this.btnSpecialRights.Size = new System.Drawing.Size(123, 24);
		this.btnSpecialRights.Text = "Special Rights";
		this.btnSpecialRights.Click += new System.EventHandler(btnSpecialRights_Click);
		this.btnCreateUpdateMultiCurrency.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCreateUpdateMultiCurrency.Image = (System.Drawing.Image)resources.GetObject("btnCreateUpdateMultiCurrency.Image");
		this.btnCreateUpdateMultiCurrency.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCreateUpdateMultiCurrency.Name = "btnCreateUpdateMultiCurrency";
		this.btnCreateUpdateMultiCurrency.Size = new System.Drawing.Size(127, 24);
		this.btnCreateUpdateMultiCurrency.Text = "MultiCurrencyInfo";
		this.btnCreateUpdateMultiCurrency.Click += new System.EventHandler(btnCreateUpdateMultiCurrency_Click);
		this.Cpv.Controls.Add(this.panel8);
		this.Cpv.Location = new System.Drawing.Point(4, 4);
		this.Cpv.Margin = new System.Windows.Forms.Padding(2);
		this.Cpv.Name = "Cpv";
		this.Cpv.Padding = new System.Windows.Forms.Padding(2);
		this.Cpv.Size = new System.Drawing.Size(1545, 783);
		this.Cpv.TabIndex = 1;
		this.Cpv.Text = "CPV History";
		this.Cpv.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1541, 779);
		this.panel8.TabIndex = 0;
		this.panel11.Controls.Add(this.panel10);
		this.panel11.Controls.Add(this.panel7);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1541, 779);
		this.panel11.TabIndex = 2;
		this.panel10.Controls.Add(this.panel9);
		this.panel10.Controls.Add(this.panel2);
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.toolStrip4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1541, 779);
		this.panel10.TabIndex = 45;
		this.panel9.Controls.Add(this.panel19);
		this.panel9.Controls.Add(this.panel18);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 333);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1541, 446);
		this.panel9.TabIndex = 55;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 25);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1541, 421);
		this.panel19.TabIndex = 56;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1541, 421);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label35);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1541, 25);
		this.panel18.TabIndex = 55;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(3, 3);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(242, 19);
		this.label35.TabIndex = 26;
		this.label35.Text = "Detail of above selected row";
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 135);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1541, 198);
		this.panel2.TabIndex = 54;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1541, 198);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 44;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel12.Controls.Add(this.rdapproveddate);
		this.panel12.Controls.Add(this.rdmodifydate);
		this.panel12.Controls.Add(this.drdocdate);
		this.panel12.Controls.Add(this.rdentrydate);
		this.panel12.Controls.Add(this.VouchersInfoBoxCash);
		this.panel12.Controls.Add(this.groupBox1);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 56);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1541, 79);
		this.panel12.TabIndex = 53;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(984, 50);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(984, 30);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(903, 30);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(903, 50);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.VouchersInfoBoxCash.Controls.Add(this.label42);
		this.VouchersInfoBoxCash.Controls.Add(this.lblUnApprovedVouchers);
		this.VouchersInfoBoxCash.Controls.Add(this.lblTotalVouchers);
		this.VouchersInfoBoxCash.Controls.Add(this.label45);
		this.VouchersInfoBoxCash.Controls.Add(this.lblApprovedVouchers);
		this.VouchersInfoBoxCash.Controls.Add(this.label47);
		this.VouchersInfoBoxCash.Location = new System.Drawing.Point(1090, 5);
		this.VouchersInfoBoxCash.Name = "VouchersInfoBoxCash";
		this.VouchersInfoBoxCash.Size = new System.Drawing.Size(229, 69);
		this.VouchersInfoBoxCash.TabIndex = 8;
		this.VouchersInfoBoxCash.TabStop = false;
		this.VouchersInfoBoxCash.Visible = false;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(6, 11);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(96, 17);
		this.label42.TabIndex = 1;
		this.label42.Text = "Total Vouchers";
		this.lblUnApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUnApprovedVouchers.Location = new System.Drawing.Point(158, 47);
		this.lblUnApprovedVouchers.Name = "lblUnApprovedVouchers";
		this.lblUnApprovedVouchers.Size = new System.Drawing.Size(67, 17);
		this.lblUnApprovedVouchers.TabIndex = 6;
		this.lblUnApprovedVouchers.Text = "0";
		this.lblUnApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalVouchers.Location = new System.Drawing.Point(158, 11);
		this.lblTotalVouchers.Name = "lblTotalVouchers";
		this.lblTotalVouchers.Size = new System.Drawing.Size(67, 17);
		this.lblTotalVouchers.TabIndex = 4;
		this.lblTotalVouchers.Text = "0";
		this.lblTotalVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(6, 47);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(144, 17);
		this.label45.TabIndex = 3;
		this.label45.Text = "UnApproved Vouchers";
		this.lblApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedVouchers.Location = new System.Drawing.Point(158, 28);
		this.lblApprovedVouchers.Name = "lblApprovedVouchers";
		this.lblApprovedVouchers.Size = new System.Drawing.Size(67, 17);
		this.lblApprovedVouchers.TabIndex = 5;
		this.lblApprovedVouchers.Text = "0";
		this.lblApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(6, 28);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(127, 17);
		this.label47.TabIndex = 2;
		this.label47.Text = "Approved Vouchers";
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv);
		this.groupBox1.Controls.Add(this.btnShowCpv);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv);
		this.groupBox1.Controls.Add(this.txtToDocNoCpv);
		this.groupBox1.Controls.Add(this.txtFromDocNoCpv);
		this.groupBox1.Controls.Add(this.ToDateCpvHistory);
		this.groupBox1.Controls.Add(this.FromDateCpvHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1081, 71);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(488, 19);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(78, 15);
		this.label26.TabIndex = 147;
		this.label26.Text = "Account Title";
		this.cmbAccountTitleCpv.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitleCpv.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitleCpv.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitleCpv.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance291).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance302).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance302).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance302).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitleCpv).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitleCpv.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).Location = new System.Drawing.Point(488, 36);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).Name = "cmbAccountTitleCpv";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCpv).TabIndex = 4;
		this.btnShowCpv.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowCpv.FlatAppearance.BorderSize = 0;
		this.btnShowCpv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowCpv.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowCpv.ForeColor = System.Drawing.Color.White;
		this.btnShowCpv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowCpv.Location = new System.Drawing.Point(831, 36);
		this.btnShowCpv.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowCpv.Name = "btnShowCpv";
		this.btnShowCpv.Size = new System.Drawing.Size(53, 26);
		this.btnShowCpv.TabIndex = 10;
		this.btnShowCpv.Text = "Show";
		this.btnShowCpv.UseVisualStyleBackColor = false;
		this.btnShowCpv.Click += new System.EventHandler(btnShowCpv_Click);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(708, 19);
		this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(95, 15);
		this.label28.TabIndex = 142;
		this.label28.Text = "Approved Status";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(382, 19);
		this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(64, 15);
		this.label29.TabIndex = 141;
		this.label29.Text = "To Doc No";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(278, 19);
		this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(79, 15);
		this.label30.TabIndex = 140;
		this.label30.Text = "From Doc No";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(144, 19);
		this.label31.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(48, 15);
		this.label31.TabIndex = 139;
		this.label31.Text = "To Date";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(5, 19);
		this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(63, 15);
		this.label32.TabIndex = 13;
		this.label32.Text = "From Date";
		this.cmbApprovedStatusCpv.AllowNull = (DefaultableBoolean)1;
		this.cmbApprovedStatusCpv.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApprovedStatusCpv.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApprovedStatusCpv.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApprovedStatusCpv).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApprovedStatusCpv.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).Location = new System.Drawing.Point(708, 36);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).Name = "cmbApprovedStatusCpv";
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCpv).TabIndex = 5;
		this.txtToDocNoCpv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoCpv.Location = new System.Drawing.Point(382, 38);
		this.txtToDocNoCpv.Name = "txtToDocNoCpv";
		this.txtToDocNoCpv.ShortcutsEnabled = false;
		this.txtToDocNoCpv.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoCpv.TabIndex = 3;
		this.txtToDocNoCpv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoCpv_KeyPress);
		this.txtFromDocNoCpv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoCpv.Location = new System.Drawing.Point(278, 38);
		this.txtFromDocNoCpv.Name = "txtFromDocNoCpv";
		this.txtFromDocNoCpv.ShortcutsEnabled = false;
		this.txtFromDocNoCpv.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoCpv.TabIndex = 2;
		this.txtFromDocNoCpv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoCpv_KeyPress);
		this.ToDateCpvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateCpvHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateCpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateCpvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateCpvHistory.Location = new System.Drawing.Point(144, 38);
		this.ToDateCpvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateCpvHistory.Name = "ToDateCpvHistory";
		this.ToDateCpvHistory.ShowCheckBox = true;
		this.ToDateCpvHistory.Size = new System.Drawing.Size(130, 23);
		this.ToDateCpvHistory.TabIndex = 1;
		this.FromDateCpvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateCpvHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateCpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateCpvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateCpvHistory.Location = new System.Drawing.Point(5, 38);
		this.FromDateCpvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateCpvHistory.Name = "FromDateCpvHistory";
		this.FromDateCpvHistory.ShowCheckBox = true;
		this.FromDateCpvHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDateCpvHistory.TabIndex = 0;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label25);
		this.panel13.Controls.Add(this.ctrlGrdBar3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 27);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1541, 29);
		this.panel13.TabIndex = 52;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(4, 4);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(259, 19);
		this.label25.TabIndex = 25;
		this.label25.Text = "Cash Payment Voucher History";
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewCpvHistory, this.btnRefreshCpvHistory, this.toolStripButton3 });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1541, 27);
		this.toolStrip4.TabIndex = 51;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNewCpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewCpvHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewCpvHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewCpvHistory.Name = "btnNewCpvHistory";
		this.btnNewCpvHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewCpvHistory.Text = "&Reset";
		this.btnNewCpvHistory.Click += new System.EventHandler(btnNewCpvHistory_Click);
		this.btnRefreshCpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshCpvHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshCpvHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshCpvHistory.Name = "btnRefreshCpvHistory";
		this.btnRefreshCpvHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshCpvHistory.Text = "Refresh";
		this.btnRefreshCpvHistory.Click += new System.EventHandler(btnRefreshCpvHistory_Click);
		this.toolStripButton3.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(84, 24);
		this.toolStripButton3.Text = "LoadAll";
		this.toolStripButton3.Visible = false;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.button1);
		this.panel7.Controls.Add(this.label34);
		this.panel7.Controls.Add(this.todate);
		this.panel7.Controls.Add(this.fromdate);
		this.panel7.Controls.Add(this.label27);
		this.panel7.Controls.Add(this.label33);
		this.panel7.Controls.Add(this.cmbdoctype);
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1162, 36);
		this.panel7.TabIndex = 44;
		this.button1.Location = new System.Drawing.Point(736, 8);
		this.button1.Margin = new System.Windows.Forms.Padding(2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(47, 23);
		this.button1.TabIndex = 113;
		this.button1.TabStop = false;
		this.button1.Text = "Show";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Visible = false;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(522, 12);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(93, 15);
		this.label34.TabIndex = 114;
		this.label34.Text = "Document Type";
		this.label34.Visible = false;
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yyyy";
		this.todate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(327, 9);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(177, 21);
		this.todate.TabIndex = 111;
		this.todate.TabStop = false;
		this.todate.Visible = false;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yyyy";
		this.fromdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(81, 9);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(177, 21);
		this.fromdate.TabIndex = 110;
		this.fromdate.TabStop = false;
		this.fromdate.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(8, 12);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(65, 15);
		this.label27.TabIndex = 116;
		this.label27.Text = "From Date";
		this.label27.Visible = false;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(268, 12);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(50, 15);
		this.label33.TabIndex = 115;
		this.label33.Text = "To Date";
		this.label33.Visible = false;
		this.cmbdoctype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbdoctype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cmbdoctype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbdoctype.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdoctype.FormattingEnabled = true;
		this.cmbdoctype.Items.AddRange(new object[2] { "CPV", "BPV" });
		this.cmbdoctype.Location = new System.Drawing.Point(626, 8);
		this.cmbdoctype.Name = "cmbdoctype";
		this.cmbdoctype.Size = new System.Drawing.Size(106, 23);
		this.cmbdoctype.TabIndex = 112;
		this.cmbdoctype.TabStop = false;
		this.cmbdoctype.Visible = false;
		this.Bpv.Controls.Add(this.panel14);
		this.Bpv.Location = new System.Drawing.Point(4, 4);
		this.Bpv.Name = "Bpv";
		this.Bpv.Size = new System.Drawing.Size(1545, 783);
		this.Bpv.TabIndex = 2;
		this.Bpv.Text = "BPV History ";
		this.Bpv.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add(this.panel16);
		this.panel14.Controls.Add(this.panel20);
		this.panel14.Controls.Add(this.panel5);
		this.panel14.Controls.Add(this.panel15);
		this.panel14.Controls.Add(this.toolStrip3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1545, 783);
		this.panel14.TabIndex = 55;
		this.panel16.Controls.Add(this.panel22);
		this.panel16.Controls.Add(this.panel21);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(0, 353);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1545, 430);
		this.panel16.TabIndex = 59;
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.GridBankDetail);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(0, 26);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1545, 404);
		this.panel22.TabIndex = 55;
		this.GridBankDetail.AllowEdit = (InheritableBoolean)2;
		this.GridBankDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridBankDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridBankDetail.DesignTimeLayout = GridBankDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridBankDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridBankDetail.DynamicFiltering = true;
		this.GridBankDetail.FilterMode = (FilterMode)2;
		this.GridBankDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridBankDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridBankDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridBankDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridBankDetail).Name = "GridBankDetail";
		((ScrollableBase)this.GridBankDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridBankDetail).Size = new System.Drawing.Size(1545, 404);
		((System.Windows.Forms.Control)(object)this.GridBankDetail).TabIndex = 52;
		this.GridBankDetail.TotalRow = (InheritableBoolean)1;
		this.GridBankDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridBankDetail).VisualStyle = (VisualStyle)4;
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.label22);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1545, 26);
		this.panel21.TabIndex = 54;
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.Teal;
		this.label22.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(2, 4);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(242, 19);
		this.label22.TabIndex = 54;
		this.label22.Text = "Detail of above selected row";
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.BankVoucherHistoryGrid);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 133);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1545, 220);
		this.panel20.TabIndex = 58;
		this.BankVoucherHistoryGrid.AllowEdit = (InheritableBoolean)2;
		this.BankVoucherHistoryGrid.DefaultFilterRowComparison = (FilterConditionOperator)7;
		BankVoucherHistoryGrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.BankVoucherHistoryGrid.DesignTimeLayout = BankVoucherHistoryGrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.BankVoucherHistoryGrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.BankVoucherHistoryGrid.DynamicFiltering = true;
		this.BankVoucherHistoryGrid.FilterMode = (FilterMode)2;
		this.BankVoucherHistoryGrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.BankVoucherHistoryGrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.BankVoucherHistoryGrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.BankVoucherHistoryGrid).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.BankVoucherHistoryGrid).Name = "BankVoucherHistoryGrid";
		((ScrollableBase)this.BankVoucherHistoryGrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.BankVoucherHistoryGrid).Size = new System.Drawing.Size(1545, 220);
		((System.Windows.Forms.Control)(object)this.BankVoucherHistoryGrid).TabIndex = 50;
		this.BankVoucherHistoryGrid.TotalRow = (InheritableBoolean)1;
		this.BankVoucherHistoryGrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.BankVoucherHistoryGrid).VisualStyle = (VisualStyle)4;
		this.BankVoucherHistoryGrid.ColumnButtonClick += new ColumnActionEventHandler(BankVoucherHistoryGrid_ColumnButtonClick);
		this.BankVoucherHistoryGrid.SelectionChanged += new System.EventHandler(BankVoucherHistoryGrid_SelectionChanged);
		this.BankVoucherHistoryGrid.LinkClicked += new ColumnActionEventHandler(BankVoucherHistoryGrid_LinkClicked);
		((System.Windows.Forms.Control)(object)this.BankVoucherHistoryGrid).DoubleClick += new System.EventHandler(BankVoucherHistoryGrid_DoubleClick);
		((System.Windows.Forms.Control)(object)this.BankVoucherHistoryGrid).KeyDown += new System.Windows.Forms.KeyEventHandler(BankVoucherHistoryGrid_KeyDown);
		this.panel5.Controls.Add(this.rdbpvapprovdate);
		this.panel5.Controls.Add(this.rdbpvmodifydate);
		this.panel5.Controls.Add(this.rdbpvdocdate);
		this.panel5.Controls.Add(this.rdbpventrydate);
		this.panel5.Controls.Add(this.VoucherInfoBoxBank);
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1545, 78);
		this.panel5.TabIndex = 57;
		this.rdbpvapprovdate.AutoSize = true;
		this.rdbpvapprovdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbpvapprovdate.Location = new System.Drawing.Point(979, 49);
		this.rdbpvapprovdate.Name = "rdbpvapprovdate";
		this.rdbpvapprovdate.Size = new System.Drawing.Size(105, 19);
		this.rdbpvapprovdate.TabIndex = 9;
		this.rdbpvapprovdate.Text = "Approved Date";
		this.rdbpvapprovdate.UseVisualStyleBackColor = true;
		this.rdbpvmodifydate.AutoSize = true;
		this.rdbpvmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbpvmodifydate.Location = new System.Drawing.Point(979, 29);
		this.rdbpvmodifydate.Name = "rdbpvmodifydate";
		this.rdbpvmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdbpvmodifydate.TabIndex = 8;
		this.rdbpvmodifydate.Text = "Modify Date";
		this.rdbpvmodifydate.UseVisualStyleBackColor = true;
		this.rdbpvdocdate.AutoSize = true;
		this.rdbpvdocdate.Checked = true;
		this.rdbpvdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbpvdocdate.Location = new System.Drawing.Point(898, 29);
		this.rdbpvdocdate.Name = "rdbpvdocdate";
		this.rdbpvdocdate.Size = new System.Drawing.Size(75, 19);
		this.rdbpvdocdate.TabIndex = 6;
		this.rdbpvdocdate.TabStop = true;
		this.rdbpvdocdate.Text = "Doc Date";
		this.rdbpvdocdate.UseVisualStyleBackColor = true;
		this.rdbpventrydate.AutoSize = true;
		this.rdbpventrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbpventrydate.Location = new System.Drawing.Point(898, 49);
		this.rdbpventrydate.Name = "rdbpventrydate";
		this.rdbpventrydate.Size = new System.Drawing.Size(80, 19);
		this.rdbpventrydate.TabIndex = 7;
		this.rdbpventrydate.Text = "Entry Date";
		this.rdbpventrydate.UseVisualStyleBackColor = true;
		this.VoucherInfoBoxBank.Controls.Add(this.label36);
		this.VoucherInfoBoxBank.Controls.Add(this.lblUnApprovedVouchersBpv);
		this.VoucherInfoBoxBank.Controls.Add(this.lblTotalVouchersBpv);
		this.VoucherInfoBoxBank.Controls.Add(this.label38);
		this.VoucherInfoBoxBank.Controls.Add(this.lblApprovedVouchersBpv);
		this.VoucherInfoBoxBank.Controls.Add(this.label37);
		this.VoucherInfoBoxBank.Location = new System.Drawing.Point(1087, 1);
		this.VoucherInfoBoxBank.Name = "VoucherInfoBoxBank";
		this.VoucherInfoBoxBank.Size = new System.Drawing.Size(238, 73);
		this.VoucherInfoBoxBank.TabIndex = 7;
		this.VoucherInfoBoxBank.TabStop = false;
		this.VoucherInfoBoxBank.Visible = false;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(6, 11);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(96, 17);
		this.label36.TabIndex = 1;
		this.label36.Text = "Total Vouchers";
		this.lblUnApprovedVouchersBpv.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUnApprovedVouchersBpv.Location = new System.Drawing.Point(158, 47);
		this.lblUnApprovedVouchersBpv.Name = "lblUnApprovedVouchersBpv";
		this.lblUnApprovedVouchersBpv.Size = new System.Drawing.Size(67, 17);
		this.lblUnApprovedVouchersBpv.TabIndex = 6;
		this.lblUnApprovedVouchersBpv.Text = "0";
		this.lblUnApprovedVouchersBpv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalVouchersBpv.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalVouchersBpv.Location = new System.Drawing.Point(158, 11);
		this.lblTotalVouchersBpv.Name = "lblTotalVouchersBpv";
		this.lblTotalVouchersBpv.Size = new System.Drawing.Size(67, 17);
		this.lblTotalVouchersBpv.TabIndex = 4;
		this.lblTotalVouchersBpv.Text = "0";
		this.lblTotalVouchersBpv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(6, 47);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(144, 17);
		this.label38.TabIndex = 3;
		this.label38.Text = "UnApproved Vouchers";
		this.lblApprovedVouchersBpv.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedVouchersBpv.Location = new System.Drawing.Point(158, 28);
		this.lblApprovedVouchersBpv.Name = "lblApprovedVouchersBpv";
		this.lblApprovedVouchersBpv.Size = new System.Drawing.Size(67, 17);
		this.lblApprovedVouchersBpv.TabIndex = 5;
		this.lblApprovedVouchersBpv.Text = "0";
		this.lblApprovedVouchersBpv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(6, 28);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(127, 17);
		this.label37.TabIndex = 2;
		this.label37.Text = "Approved Vouchers";
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitleBpvHistory);
		this.groupBox3.Controls.Add(this.btnShowHistoryBpv);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatusBpv);
		this.groupBox3.Controls.Add(this.txtToDocNoBpvHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoBpvHistory);
		this.groupBox3.Controls.Add(this.ToDateBpvHistory);
		this.groupBox3.Controls.Add(this.FromDateBpvHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1075, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(488, 22);
		this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(78, 15);
		this.label24.TabIndex = 147;
		this.label24.Text = "Account Title";
		this.CmbAccountTitleBpvHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitleBpvHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitleBpvHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitleBpvHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitleBpvHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBpvHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitleBpvHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBpvHistory).Location = new System.Drawing.Point(488, 39);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBpvHistory).Name = "CmbAccountTitleBpvHistory";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBpvHistory).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBpvHistory).TabIndex = 4;
		this.btnShowHistoryBpv.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowHistoryBpv.FlatAppearance.BorderSize = 0;
		this.btnShowHistoryBpv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistoryBpv.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistoryBpv.ForeColor = System.Drawing.Color.White;
		this.btnShowHistoryBpv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowHistoryBpv.Location = new System.Drawing.Point(829, 39);
		this.btnShowHistoryBpv.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistoryBpv.Name = "btnShowHistoryBpv";
		this.btnShowHistoryBpv.Size = new System.Drawing.Size(53, 26);
		this.btnShowHistoryBpv.TabIndex = 10;
		this.btnShowHistoryBpv.Text = "Show";
		this.btnShowHistoryBpv.UseVisualStyleBackColor = false;
		this.btnShowHistoryBpv.Click += new System.EventHandler(btnShowHistoryBpv_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(707, 22);
		this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(95, 15);
		this.label18.TabIndex = 142;
		this.label18.Text = "Approved Status";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(384, 22);
		this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(64, 15);
		this.label17.TabIndex = 141;
		this.label17.Text = "To Doc No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(281, 22);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 140;
		this.label7.Text = "From Doc No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(146, 22);
		this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(48, 15);
		this.label10.TabIndex = 139;
		this.label10.Text = "To Date";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(5, 23);
		this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(63, 15);
		this.label19.TabIndex = 13;
		this.label19.Text = "From Date";
		this.cmbApproveStatusBpv.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatusBpv.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatusBpv.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatusBpv.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatusBpv).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBpv).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApproveStatusBpv.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBpv).Location = new System.Drawing.Point(707, 39);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBpv).Name = "cmbApproveStatusBpv";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBpv).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBpv).TabIndex = 5;
		this.txtToDocNoBpvHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoBpvHistory.Location = new System.Drawing.Point(384, 41);
		this.txtToDocNoBpvHistory.Name = "txtToDocNoBpvHistory";
		this.txtToDocNoBpvHistory.ShortcutsEnabled = false;
		this.txtToDocNoBpvHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoBpvHistory.TabIndex = 3;
		this.txtToDocNoBpvHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoBpvHistory_KeyPress);
		this.txtFromDocNoBpvHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoBpvHistory.Location = new System.Drawing.Point(281, 41);
		this.txtFromDocNoBpvHistory.Name = "txtFromDocNoBpvHistory";
		this.txtFromDocNoBpvHistory.ShortcutsEnabled = false;
		this.txtFromDocNoBpvHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoBpvHistory.TabIndex = 2;
		this.txtFromDocNoBpvHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoBpvHistory_KeyPress);
		this.ToDateBpvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateBpvHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateBpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateBpvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateBpvHistory.Location = new System.Drawing.Point(146, 41);
		this.ToDateBpvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateBpvHistory.Name = "ToDateBpvHistory";
		this.ToDateBpvHistory.ShowCheckBox = true;
		this.ToDateBpvHistory.Size = new System.Drawing.Size(132, 23);
		this.ToDateBpvHistory.TabIndex = 1;
		this.FromDateBpvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateBpvHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateBpvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateBpvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateBpvHistory.Location = new System.Drawing.Point(5, 41);
		this.FromDateBpvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateBpvHistory.Name = "FromDateBpvHistory";
		this.FromDateBpvHistory.ShowCheckBox = true;
		this.FromDateBpvHistory.Size = new System.Drawing.Size(138, 23);
		this.FromDateBpvHistory.TabIndex = 0;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label6);
		this.panel15.Controls.Add(this.ctrlGrdBar4);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 27);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1545, 28);
		this.panel15.TabIndex = 56;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(5, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(261, 19);
		this.label6.TabIndex = 22;
		this.label6.Text = "Bank Payment Voucher History";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistoryBpv, this.btnRefreshHistoryBpv, this.btnLoadAllFromBrvTab });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1545, 27);
		this.toolStrip3.TabIndex = 55;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistoryBpv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistoryBpv.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistoryBpv.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistoryBpv.Name = "btnNewHistoryBpv";
		this.btnNewHistoryBpv.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistoryBpv.Text = "&Reset";
		this.btnNewHistoryBpv.Click += new System.EventHandler(btnNewHistoryBpv_Click);
		this.btnRefreshHistoryBpv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistoryBpv.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistoryBpv.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistoryBpv.Name = "btnRefreshHistoryBpv";
		this.btnRefreshHistoryBpv.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistoryBpv.Text = "Refresh";
		this.btnRefreshHistoryBpv.Click += new System.EventHandler(btnRefreshHistoryBpv_Click);
		this.btnLoadAllFromBrvTab.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.btnLoadAllFromBrvTab.Image = (System.Drawing.Image)resources.GetObject("btnLoadAllFromBrvTab.Image");
		this.btnLoadAllFromBrvTab.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllFromBrvTab.Name = "btnLoadAllFromBrvTab";
		this.btnLoadAllFromBrvTab.Size = new System.Drawing.Size(84, 24);
		this.btnLoadAllFromBrvTab.Text = "LoadAll";
		this.btnLoadAllFromBrvTab.Visible = false;
		this.lblCrAcGlCurrency.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblCrAcGlCurrency.Location = new System.Drawing.Point(352, 67);
		this.lblCrAcGlCurrency.Name = "lblCrAcGlCurrency";
		this.lblCrAcGlCurrency.Size = new System.Drawing.Size(95, 27);
		this.lblCrAcGlCurrency.TabIndex = 571;
		this.lblCrAcGlCurrency.Text = "Credit Ac GLcy:";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1496, 6);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar1.TabIndex = 19;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1496, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 23);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1113, 6);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar2.TabIndex = 20;
		this.ctrlGrdBar2.TabStop = false;
		this.ctrlGrdBar2.Visible = false;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1500, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.BankVoucherHistoryGrid;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(43, 23);
		this.ctrlGrdBar4.TabIndex = 51;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1553, 813);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PaymentVoucherNew";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PaymentVoucherNew_FormClosing);
		base.Load += new System.EventHandler(AcfrmPaymentVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PaymentVoucherNew_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSrbTaxAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWithHoldingAc).EndInit();
		this.ParentPanelOfMainDetail.ResumeLayout(false);
		this.groupboxdetail.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		this.PaymentTypePanel.ResumeLayout(false);
		this.PaymentTypePanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentType).EndInit();
		this.DebitAccountPanel.ResumeLayout(false);
		this.DebitAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountDetail).EndInit();
		this.SubsidiaryAcPanel.ResumeLayout(false);
		this.SubsidiaryAcPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).EndInit();
		this.JobLotPanel.ResumeLayout(false);
		this.JobLotPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		this.RemarksPanel.ResumeLayout(false);
		this.RemarksPanel.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTcyCodeDetail).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.FcyAmountPanel.ResumeLayout(false);
		this.FcyAmountPanel.PerformLayout();
		this.DebitAmountPanel.ResumeLayout(false);
		this.DebitAmountPanel.PerformLayout();
		this.ReferenceAccountPanel.ResumeLayout(false);
		this.ReferenceAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).EndInit();
		this.FinancialInstrumentPanel.ResumeLayout(false);
		this.FinancialInstrumentPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFinancialInstrument).EndInit();
		this.ChequeDatePanel.ResumeLayout(false);
		this.ChequeDatePanel.PerformLayout();
		this.ChequeNoPanel.ResumeLayout(false);
		this.ChequeNoPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqNo).EndInit();
		this.PayeeTitlePanel.ResumeLayout(false);
		this.PayeeTitlePanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtPayTitle).EndInit();
		this.ChequeTypePanel.ResumeLayout(false);
		this.ChequeTypePanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbChequeType).EndInit();
		this.BranchPanel.ResumeLayout(false);
		this.BranchPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		this.CostCenterPanel.ResumeLayout(false);
		this.CostCenterPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		this.panel24.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVoucherType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgainstAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.Cpv.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.VouchersInfoBoxCash.ResumeLayout(false);
		this.VouchersInfoBoxCash.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitleCpv).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusCpv).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.Bpv.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridBankDetail).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel20.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.BankVoucherHistoryGrid).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.VoucherInfoBoxBank.ResumeLayout(false);
		this.VoucherInfoBoxBank.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleBpvHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatusBpv).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
