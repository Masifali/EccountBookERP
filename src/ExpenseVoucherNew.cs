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
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using FilterEditType = Janus.Windows.GridEX.FilterEditType;
using Reporting = System.Object;
using FcYBankChargesTypes = System.Object;
using SpecialSreenRightsUserWise = System.Object;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
using UserAccount = global::Architecture.Model.UserAccount;
using VoucherHead = global::Architecture.Model.Accounts.VoucherHead;
using DMSAttachments = global::Architecture.BLL.DMSAttachments;
using tblUserRights = global::Architecture.Model.tblUserRights;
using CheqBookHeader = global::Architecture.BLL.Accounts.CheqBookHeader;
using TaxScheduleMain = global::Architecture.Model.Inventory.TaxScheduleMain;
using PdcInventory = global::Architecture.Model.Accounts.PdcInventory;
using MultiCurrency = global::Architecture.Model.MultiCurrency;
using BankBalanceManual = global::Architecture.BLL.BankBalanceManual;
using BranchesAllocationToUser = global::Architecture.BLL.BranchesAllocationToUser;
using SupCustBankDetail = global::Architecture.Model.Inventory.SupCustBankDetail;
using Projects = global::Architecture.BLL.Projects;
using UsersWithCostCenter = global::Architecture.BLL.Inventory.UsersWithCostCenter;
using AccountsPrematurePaymentsReceipts = global::Architecture.BLL.Accounts.AccountsPrematurePaymentsReceipts;
using AccountsReconciliationHeader = global::Architecture.BLL.Accounts.AccountsReconciliationHeader;
using BankReconciliation = global::Architecture.BLL.Accounts.BankReconciliation;
using City = global::Architecture.Model.City;
using Country = global::Architecture.Model.Country;
using ChartofAccount = global::Architecture.BLL.ChartofAccountBLL;
using pdcTransaction = global::Architecture.Model.BankSchema.pdcTransaction;
using LicenseContext = OfficeOpenXml.LicenseContext;
using SortOrder = Janus.Windows.GridEX.SortOrder;
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using SreenRightsUserWise = global::Architecture.Model.SreenRightsUserWise;
using TaxesTypes = global::Architecture.Model.Inventory.TaxesTypes;
using COAAllocation = global::Architecture.Model.Accounts.COAAllocation;

using MultiCurrencyBLL = global::Architecture.BLL.MultiCurrency;
using AccountsBudgetHeader = global::Architecture.Model.Accounts.AccountsBudgetHeader;
namespace Architecture.WinApp.Account_Definition.VouchersWithTax
{

public class ExpenseVoucherNew : Form
{
	private DataTable dtVoucherDetailMultiCurrency = new DataTable();

	private DataTable dtCurrencies = new DataTable();

	private DataTable dtAccount = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtGridCostCenterDetail = new DataTable();

	private DataTable dtAllSubCostCenters = new DataTable();

	private int RecId;

	private double BaseRate;

	private int BaseCurrency;

	private double DebitAmtGet;

	private int updateDetailIndex = -1;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private double AmountLimitForExpenseVoucher;

	private int SubsdiaryAccountDatasourceLength;

	private bool AfterCloseup;

	private bool BranchFeature;

	private bool shiftTabPressed;

	private bool CheqBookEnabled;

	public bool UpdateMode;

	private bool MultiCurrencyGridWorking;

	private bool PreventNegativeBalanceEntry;

	private bool DisplayWarningforNegativeBalance;

	private bool DisableBothNegativeBalanceRestrictions;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool InventoryRelatedAccountsShowInVouchers;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private ToolStripButton btnNew;

	private ToolStripButton btnEdit;

	private ToolStripButton btnS;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnDte;

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

	private TabPage History;

	private GroupBox groupBox2;

	private Label label2;

	private Label label15;

	private DateTimePicker datVoucherDate;

	private TextBox txtComments;

	private TextBox txtDebitAmount;

	private Label label20;

	private Label lblRemarks;

	private Label label11;

	private Button Add;

	private Panel panel8;

	private Panel panel11;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbRefAccountId;

	private TextBox txtVoucherCode;

	internal UltraCombo CmbAccountId;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label1;

	private BackgroundWorker backgroundWorker1;

	private Timer timer1;

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

	internal GridEX grdDetail;

	private Label lblJobLot;

	internal UltraCombo CmbJobLotId;

	private Label lblbalancedetail;

	private CtrlGrdBar ctrlGrdBar3;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	internal UltraCombo CmbSubsidiaryAccount;

	private Label lblSubsidiaryaccount;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

	private TextBox txtExchangeRate;

	private Label lblExchangeRate;

	private Label lblFcyAmount;

	private TextBox txtFcyAmount;

	internal UltraCombo CmbProjectId;

	private Label label5;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnLoadAll;

	private Panel panel15;

	private Label label7;

	private Panel panel16;

	private GroupBox VoucherInfoBox;

	private Label label36;

	private Label lblUnApprovedVouchers;

	private Label lblTotalVouchers;

	private Label label38;

	private Label lblApprovedVouchers;

	private Label label37;

	private GroupBox groupBox3;

	private Label label24;

	internal UltraCombo CmbAccountTitleHistory;

	private Button btnShowHistory;

	private Label label10;

	private Label label14;

	private Label label23;

	private Label label25;

	private Label label26;

	internal UltraCombo cmbApproveStatus;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel12;

	private Panel panel9;

	private Label label28;

	private Panel panel13;

	private RadioButton rdexpapproved;

	private RadioButton rdexpmodify;

	private RadioButton rdexpdocdate;

	private RadioButton rdexpentry;

	private TabControl tabControl2;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel6;

	internal GridEX grdCostCenter;

	internal GridEX grdCostCenterDetail;

	private Panel panel14;

	private Label label8;

	internal GridEX grdCostCenterDetailSummary;

	private Panel panel18;

	internal UltraCombo cmbBranchName;

	private Label labelbranchname;

	private TextBox txtRemarks;

	private Panel panel21;

	private Panel panel22;

	private Label label13;

	private Panel panel19;

	private Panel panel20;

	private Label label12;

	private Panel panel5;

	private Label label3;

	internal UltraCombo CmbCostCenter;

	private Label labelCostCenter;

	private Label labelSubsidiaryBal;

	private Panel ChequeDetailPanel;

	private DateTimePicker datChequeDate;

	private Label lblCheqDate;

	private TextBox txtPayTitle;

	private Label lblCheqNo;

	private Label lblPayTitle;

	internal UltraCombo CmbChequeNo;

	private TextBox txtbalance;

	private Label label4;

	private Panel PanelSubsidiary;

	private Panel PanelCostCenter;

	private Panel PanelComments;

	private Panel PanelBranchName;

	private Panel PanelDebitaccount;

	private Panel PanelAddButton;

	private GroupBox groupboxdetail;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel PanelAmount;

	private Panel PanelJobLot;

	private Panel FcyAmountDetailPanel;

	private TextBox txtFcyAmountDetail;

	private Label label6;

	private Panel ReferenceAccountPanel;

	internal UltraCombo CmbReferenceAccount;

	private Label label46;

	private Panel panel2;

	private Panel panel17;

	private Panel panel4;

	private CheckBox chkCustomAccounts;

	private Label labelLocationType;

	internal UltraCombo cmbLocationType;

	private Label ExpenseVoucherLable;

	private CheckBox chkformat2;

	private CheckBox ChkBox;

	private Panel panel3;

	private ToolStrip btnPrintFormat2;

	private ToolStripButton New;

	private ToolStripButton btnRefresh;

	private new ToolStripButton Update;

	private ToolStripButton Save;

	private ToolStripButton toolStripButton10;

	private ToolStripButton btnDelete;

	private ToolStripButton Print;

	private ToolStripButton btnPrintFormatII;

	private ToolStripButton BtnCostCenterform;

	private ToolStripButton btnShortCutKey;

	private Panel panel23;

	private Panel panel24;

	private Panel MultiCurrencyPanel;

	private Panel panel25;

	private Label label9;

	internal GridEX grdMultiCurrency;

	private TextBox txtBaseFcyCode;

	private Label lblBaseFcyCode;

	private TextBox txtBaseFcyExchangeRate;

	private Label lblBaseFcyExchangeRate;

	private Label lblCrAcGlCurrency;

	private Panel panel26;

	internal UltraCombo cmbTcyCodeDetail;

	private Label label54;

	private Panel panel27;

	private TextBox txtTcyExchangeRateDetail;

	private Label label53;

	private ToolStripButton btnCreateUpdateMultiCurrency;

	public ExpenseVoucherNew(UserAccount puserAccount)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void AcfrmPaymentVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 600);
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			Save.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			Update.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			CheqBookEnabled = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheqBook Enabled"));
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "AmountLimitForExpenseVoucher");
			AmountLimitForExpenseVoucher = Conversion.ToDouble((object)((obj != null) ? obj.ConfigKey : null));
			MultiCurrencyGridWorking = CommonServices.GetERPFeatureById(6);
			BranchFeature = CommonServices.GetERPFeatureById(17);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				PanelSubsidiary.Visible = true;
				PanelComments.Width = 180;
				txtComments.Width = 179;
			}
			else
			{
				PanelSubsidiary.Visible = false;
				PanelComments.Width = 384;
				txtComments.Width = 382;
			}
			PanelBranchName.Visible = BranchFeature;
			if (table.Columns.Count == 0)
			{
				table.Columns.Add("AccountCode");
				table.Columns.Add("AccountId");
				table.Columns.Add("GlCurrencyId", typeof(int));
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
				table.Columns.Add("LineId", typeof(int));
				table.Columns.Add("CostCenterId", typeof(int));
				table.Columns.Add("CostCenterName");
				table.Columns.Add("BranchId", typeof(int));
				table.Columns.Add("BranchName");
				table.Columns.Add("ChequeId", typeof(int));
				table.Columns.Add("ChequeDate", typeof(DateTime));
				table.Columns.Add("ChequeNo", typeof(string));
				table.Columns.Add("PayTitle", typeof(string));
			}
			grd.DataSource = table;
			dtGridCostCenterDetail.Columns.Add("Id", typeof(int));
			dtGridCostCenterDetail.Columns.Add("LineId", typeof(int));
			dtGridCostCenterDetail.Columns.Add("ParentId", typeof(int));
			dtGridCostCenterDetail.Columns.Add("DebitAccount", typeof(string));
			dtGridCostCenterDetail.Columns.Add("CostCenterId", typeof(int));
			dtGridCostCenterDetail.Columns.Add("CostCenter", typeof(string));
			dtGridCostCenterDetail.Columns.Add("Percent", typeof(double));
			dtGridCostCenterDetail.Columns.Add("Amount", typeof(double));
			grdCostCenterDetail.DataSource = dtGridCostCenterDetail;
			grdCostCenterDetail.RetrieveStructure();
			grdCostCenterDetailSetting();
			if (!MultiCurrencyGridWorking)
			{
				((Control)(object)grdMultiCurrency).Visible = false;
				lblCrAcGlCurrency.Visible = false;
				lblBaseFcyCode.Visible = false;
				txtBaseFcyCode.Visible = false;
				lblBaseFcyExchangeRate.Visible = false;
				txtBaseFcyExchangeRate.Visible = false;
				MultiCurrencyPanel.Visible = false;
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
				dtAccount.Columns.Add("AccountTypeId");
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
			GetAllSubCostCenters();
			if (UserAccount.AppId == 5 && dtAllSubCostCenters.Rows.Count > 0)
			{
				((UltraGridBase)CmbCostCenter).Rows[0].Activate();
			}
			GridCostCenterFill();
			LocationType();
			CurrencyFill();
			BranchesFill();
			AccountTitleFill();
			VoucherNofill();
			BindReferenceAccounts();
			CombProjectFill();
			combojoblotfill();
			DetailAccountFill();
			lblbalancedetail.Visible = false;
			Save.Visible = true;
			Update.Visible = false;
			cmbdoctype.SelectedIndex = 0;
			datVoucherDate.Focus();
			ChkBox.Checked = true;
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			DefaultConfigurations();
			BtnCostCenterform.Visible = false;
			StatusFillForHistory();
			ComboBindForHistory();
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
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
			DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GetLocationType();
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

	private void combojoblotfill()
	{
		try
		{
			int jobLotId = Conversion.ToInt(CmbJobLotId.Value);
			dtJobLot = CommonServices.JobLotGetAllService();
			if (dtJobLot.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtJobLot, CmbJobLotId, "Id", "JobLotDescription", "JobLotDescription", true);
			if (jobLotId > 0)
			{
				if (dtJobLot.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == jobLotId))
				{
					CmbJobLotId.Value = jobLotId;
				}
				else
				{
					((Control)(object)CmbJobLotId).Text = string.Empty;
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
		try
		{
			int AccountId = Conversion.ToInt(CmbRefAccountId.Value);
			DataTable dtRefAccount = new DataTable();
			dtRefAccount.Columns.Add("Id", typeof(int));
			dtRefAccount.Columns.Add("AccountTitle");
			dtRefAccount.Columns.Add("AccountCode");
			dtRefAccount.Columns.Add("AccountTypeId");
			dtRefAccount.Columns.Add("ParentAccountTitle");
			dtRefAccount.Columns.Add("AccountClass");
			dtRefAccount.Columns.Add("CurrencyId");
			dtRefAccount.Columns.Add("CurrencyCode");
			DataTable dt = CommonServices.Accounts_GetAccountTitleByAccountTypeIds("2,15");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtRefAccount.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["AccountCode"], dt.Rows[i]["AccountTypeId"], dt.Rows[i]["ParentAccountTitle"], dt.Rows[i]["AccountClass"], dt.Rows[i]["CurrencyId"], dt.Rows[i]["CurrencyCode"]);
				}
			}
			if (dtRefAccount.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtRefAccount, CmbRefAccountId, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
			((UltraGridBase)CmbRefAccountId).DisplayLayout.Bands[0].Columns[1].Width = 350;
			((UltraGridBase)CmbRefAccountId).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			((UltraGridBase)CmbRefAccountId).DisplayLayout.Bands[0].Columns[6].Hidden = true;
			if (AccountId > 0)
			{
				if (dtRefAccount.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == AccountId))
				{
					CmbRefAccountId.Value = AccountId;
					combcreditac_Leave(null, null);
				}
				else
				{
					((Control)(object)CmbRefAccountId).Text = string.Empty;
				}
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
			DataTable dtA = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtA.Rows.Count > 0)
			{
				int[] AccountTypes = new int[5] { 3, 5, 6, 7, 8 };
				string[] columns = new string[5] { "Id", "AccountTitle", "AccountCode", "ParentAccountTitle", "AccountClass" };
				DataTable dt = CommonServices.FilterDataTableByAccountTypes(dtA, AccountTypes, includeTypes: true, columns);
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbReferenceAccount, "Id", "AccountTitle", "Reference Account", ZeroIndex: false);
					((UltraGridBase)CmbReferenceAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
					CommonServices.SetComboValue(CmbReferenceAccount, "Id", Id, dt);
				}
				else
				{
					((Control)(object)CmbReferenceAccount).Text = string.Empty;
					((UltraGridBase)CmbReferenceAccount).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbReferenceAccount).Text = string.Empty;
				((UltraGridBase)CmbReferenceAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VoucherNofill()
	{
		try
		{
			txtVoucherCode.Text = Conversion.ToString((object)CommonServices.GenerateVoucherCode(26));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailAccountFill()
	{
		try
		{
			int AccountId = Conversion.ToInt(CmbAccountId.Value);
			dtAccount.Clear();
			List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
			if (allAccounts == null)
			{
				return;
			}
			IEnumerable<getGlobalAllAccountsWithCustomGroup> filteredAccounts = allAccounts.AsEnumerable();
			int[] include = ((!InventoryRelatedAccountsShowInVouchers) ? new int[7] { 11, 13, 14, 20, 21, 22, 23 } : new int[8] { 11, 12, 13, 14, 20, 21, 22, 23 });
			int[] array = include;
			if (array != null && array.Length != 0)
			{
				filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => include.Contains(a.AccountTypeId));
			}
			HashSet<int> seenIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup account in filteredAccounts)
			{
				int id = Conversion.ToInt((object)account.ChartOfAccountId);
				if (seenIds.Add(id))
				{
					dtAccount.Rows.Add(id, account.AccountTitle, account.AccountCode, account.AccountTypeId, account.ParentAccountTitle, account.AccountClassName, account.CurrencyId, account.CurrencyCode);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbAccountId, dtAccount, "Id", "AccountTitle", "Account Title", AllColumns: true, AccountId, insertDefaultRow: false, null, null, ActivateRow: false);
			if (AccountId > 0)
			{
				combactitle_Leave(null, null);
			}
			if (dtAccount.Rows.Count > 0)
			{
				((UltraGridBase)CmbAccountId).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbAccountId).DisplayLayout.Bands[0].Columns[6].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountCurrentBalance()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		try
		{
			DataTable detail = global::Architecture.BLL.Accounts.VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = datVoucherDate.Value,
				RefAccountId = Conversion.ToInt(CmbRefAccountId.Value)
			});
			if (detail.Rows.Count > 0)
			{
				double ClosingBalance = Conversion.ToDouble(detail.Rows[0]["Balance"]);
				txtbalance.Text = ClosingBalance.ToString("#,#;(#,#);0");
			}
			else
			{
				txtbalance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheqNoFill()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			if (!CheqBookEnabled)
			{
				return;
			}
			((Control)(object)CmbChequeNo).Text = string.Empty;
			DataTable dtCheq = global::Architecture.BLL.Accounts.CheqBookHeader.OutstandingCheqNo(new global::Architecture.Model.Accounts.CheqBookHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BankId = Conversion.ToInt(CmbRefAccountId.Value)
			});
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
					DDL.BindDDL(dt, CmbChequeNo, "Id", "CheqNo", "CheqNo", ZeroIndex: false);
				}
				CmbChequeNo.LimitToList = true;
			}
			else
			{
				((Control)(object)CmbChequeNo).Text = string.Empty;
				((UltraGridBase)CmbChequeNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailAccountCurrentBalance()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		try
		{
			DataTable detail = global::Architecture.BLL.Accounts.VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = datVoucherDate.Value,
				RefAccountId = Conversion.ToInt(CmbAccountId.Value)
			});
			if (detail.Rows.Count > 0)
			{
				double ClosingBalance = Conversion.ToDouble(detail.Rows[0]["Balance"]);
				lblbalancedetail.Text = ClosingBalance.ToString("#,#;(#,#);0");
			}
			else
			{
				lblbalancedetail.Text = "0";
			}
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
			if (((UltraGridBase)CmbAccountId).ActiveRow != null && ((Control)(object)CmbAccountId).Text != string.Empty)
			{
				DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.BindSubsidiaryAccount(UserAccount.CompanyId, Conversion.ToInt(CmbAccountId.Value), "3");
				if (dt.Rows.Count > 0)
				{
					SubsdiaryAccountDatasourceLength = dt.Rows.Count;
					DDL.BindDDL(dt, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					if (dt.Rows.Count == 1)
					{
						((UltraGridBase)CmbSubsidiaryAccount).Rows[0].Activate();
					}
					else
					{
						((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
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

	private void combactitle_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbAccountId.Value) > 0)
			{
				DetailAccountCurrencyCheck();
			}
			lblbalancedetail.Visible = true;
			DetailAccountCurrentBalance();
			BindSubsidiaryAccount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFillForHistory()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Not Apporved");
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApproveStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindForHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherReports.LedgerByJobLotDropDownAndLists(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.OrganizationId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = 26.ToString()
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
			DDL.BindDDL(dtAccount, CmbAccountTitleHistory, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			int CurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells[6].Value);
			string CurrencyCode = Conversion.ToString(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells[7].Value);
			lblCrAcGlCurrency.Text = ((CurrencyId > 0) ? ("Credit Ac GLcy:\r\n" + CurrencyCode) : "Credit Ac GLcy:");
			if (CurrencyId == 0)
			{
				if (RecId == 0)
				{
					((Control)(object)CmbRefAccountId).Text = "";
					CmbRefAccountId.Focus();
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
			if (MultiCurrencyGridWorking && Conversion.ToInt(((UltraDropDownBase)CmbAccountId).SelectedRow.Cells[6].Value) == 0 && updateDetailIndex == -1)
			{
				((Control)(object)CmbAccountId).Text = "";
				CmbAccountId.Focus();
				throw new Exception("Detail Account Currency Not set. Please Check!");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void combcreditac_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbRefAccountId.Value) > 0)
			{
				CreditAccountCurrencyCheck();
			}
			AccountCurrentBalance();
			CmbAccountId_ValueChanged(null, null);
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

	private void DefaultConfigurations()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbJobLotId.Value = JobId;
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int Currency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				BaseCurrency = Currency;
				cmbCurrency.Value = BaseCurrency;
				cmbCurrency_Leave(null, null);
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				double BaseCurrencyRate = (BaseRate = Conversion.ToDouble((object)BaseRatelist[0].ConfigKey.ToString()));
				txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
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

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				return;
			}
			cmbTcyCodeDetail.Value = Conversion.ToInt(cmbCurrency.Value);
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "26",
					Ids = Conversion.ToString(cmbCurrency.Value)
				});
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

	private void CurrencyFill()
	{
		try
		{
			dtCurrencies = MultiCurrencyBLL.GetMultiCurrencywithExchangeRate(UserAccount.CompanyId);
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
			throw new Exception(ex.Message);
		}
	}

	private void GetAllSubCostCenters()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCostCenter.Value);
			dtAllSubCostCenters = Projects.GetSubCostCenters(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, UserAccount.AppId, 0);
			if (dtAllSubCostCenters.Rows.Count > 0)
			{
				DDL.BindDDL(dtAllSubCostCenters, CmbCostCenter, "Id", "CostCenterName", "CostCenterName", ZeroIndex: false);
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[1].Width = 180;
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCostCenter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (Id > 0)
				{
					if (dtAllSubCostCenters.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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

	private void CmbSubsidiaryAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!AfterCloseup && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && Conversion.ToInt(CmbSubsidiaryAccount.Value) != 0)
			{
				int GLOfSubSidory = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbAccountId.Value = GLOfSubSidory;
				}
			}
			AfterCloseup = false;
			if (Conversion.ToInt(CmbAccountId.Value) > 0 && Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
			{
				labelSubsidiaryBal.Visible = true;
				labelSubsidiaryBal.Text = "0";
				double ClosingBalance = CommonServices.GetSubsiadiaryBalance(Conversion.ToInt(CmbAccountId.Value), Conversion.ToInt(CmbSubsidiaryAccount.Value), datVoucherDate.Value);
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

	private void CmbSubsidiaryAccount_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbAccountId.Value) > 0 && Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
			{
				labelSubsidiaryBal.Visible = true;
				labelSubsidiaryBal.Text = "0";
				double ClosingBalance = CommonServices.GetSubsiadiaryBalance(Conversion.ToInt(CmbAccountId.Value), Conversion.ToInt(CmbSubsidiaryAccount.Value), datVoucherDate.Value);
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

	private void CmbSubsidiaryAccount_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloseup = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbAccountId.Value = GLOfSubSidory;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				int LineId = 0;
				if (item != null && (int)item.RowType == 0)
				{
					LineId = Conversion.ToInt(item.Cells["LineId"].Value);
					item.Delete();
				}
				grd.UpdateData();
				if (MultiCurrencyGridWorking)
				{
					CreateUpdateMultiCurrencyGridInfo();
				}
				CalculateTotalInformation();
				DeleteCostCenterDetail(LineId);
				SummaryForCostCenterBreakUp();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSettings()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.ColumnAutoResize = false;
			grd.AllowDelete = (InheritableBoolean)2;
			grd.AllowEdit = (InheritableBoolean)2;
			grd.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			int TypeId = 0;
			if (((UltraGridBase)CmbRefAccountId).ActiveRow != null && Conversion.ToInt(CmbRefAccountId.Value) > 0)
			{
				TypeId = Conversion.ToInt(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells["AccountTypeId"].Value);
			}
			grd.RootTable.Columns["ChequeDate"].Visible = TypeId == 15;
			grd.RootTable.Columns["ChequeNo"].Visible = TypeId == 15;
			grd.RootTable.Columns["PayTitle"].Visible = TypeId == 15;
			grd.RootTable.Columns["CostCenterId"].Visible = false;
			grd.RootTable.Columns["ChequeId"].Visible = false;
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["GlCurrencyId"].Visible = false;
			grd.RootTable.Columns["TcyCodeId"].Visible = false;
			grd.RootTable.Columns["GlCurrency"].Visible = MultiCurrencyGridWorking;
			grd.RootTable.Columns["ReferenceAccountId"].Visible = false;
			grd.RootTable.Columns["LineId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountTypeId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["GlCurrencyId"].EditType = (EditType)0;
			grd.RootTable.Columns["GlCurrency"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyCodeId"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyCode"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyExchangeRate"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchId"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchName"].EditType = (EditType)0;
			grd.RootTable.Columns["CostCenterId"].EditType = (EditType)0;
			grd.RootTable.Columns["CostCenterName"].EditType = (EditType)0;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["ChequeDate"].EditType = (EditType)0;
			grd.RootTable.Columns["ChequeNo"].EditType = (EditType)0;
			grd.RootTable.Columns["PayTitle"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferenceAccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferenceAccount"].EditType = (EditType)0;
			grd.RootTable.Columns["PayTitle"].EditType = (EditType)0;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			grd.RootTable.Columns["LineId"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["AccountId"].HasValueList = true;
			grd.RootTable.Columns["AccountId"].LimitToList = true;
			grd.RootTable.Columns["AccountId"].Caption = "Account Title";
			grd.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccount.DefaultView, "Id", "AccountTitle");
			grd.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobLotId"].HasValueList = true;
			grd.RootTable.Columns["JobLotId"].LimitToList = true;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grd.RootTable.Columns["JobLotId"].Caption = "JobLot";
			grd.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtJobLot.DefaultView, "Id", "JobLotDescription");
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccount"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].Width = 80;
			grd.RootTable.Columns["AccountId"].Width = (SubsidiaryAccountAllownOnVouchers ? 150 : 200);
			grd.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 15;
			grd.RootTable.Columns["SubsidiaryAccount"].Width = 130;
			grd.RootTable.Columns["JobLotId"].Width = (SubsidiaryAccountAllownOnVouchers ? 90 : 105);
			grd.RootTable.Columns["Remarks"].Width = (BranchFeature ? 120 : 180);
			grd.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grd.RootTable.Columns["TcyExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
			grd.RootTable.Columns["ReferenceAccount"].Width = 120;
			grd.RootTable.Columns["FcyAmount"].Width = 90;
			grd.RootTable.Columns["Amount"].Width = 90;
			grd.RootTable.Columns["BranchName"].Width = 100;
			grd.RootTable.Columns["ChequeDate"].Width = 90;
			grd.RootTable.Columns["ChequeNo"].Width = 110;
			grd.RootTable.Columns["PayTitle"].Width = 130;
			grd.RootTable.Columns["Amount"].Caption = "Debit Amount";
			grd.RootTable.Columns["FcyAmount"].Caption = "Tcy Amount";
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["TcyExchangeRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TcyExchangeRate"].FormatString = "#,##0.###";
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			ctrlGrdBar1_Load(null, null);
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
			GridEXRow item = grd.CurrentRow;
			if (item == null)
			{
				return;
			}
			updateDetailIndex = item.RowIndex;
			CmbAccountId.Value = item.Cells["AccountId"].Value.ToString();
			BindSubsidiaryAccount();
			if (SubsdiaryAccountDatasourceLength == 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("SubsidiaryAccount");
				dt.Columns.Add("TypeId");
				dt.Rows.Add(Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value), item.Cells["SubsidiaryAccount"].Text, 4);
				DDL.BindDDL(dt, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
				((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
			CmbSubsidiaryAccount.Value = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
			CmbJobLotId.Value = item.Cells["JobLotId"].Value.ToString();
			txtComments.Text = item.Cells["Remarks"].Text.ToString();
			cmbTcyCodeDetail.Value = Conversion.ToInt(item.Cells["TcyCodeId"].Value);
			txtTcyExchangeRateDetail.Text = Conversion.ToDouble(item.Cells["TcyExchangeRate"].Value).ToString("#,##0.###");
			txtFcyAmountDetail.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString("#,##0.###");
			txtDebitAmount.Text = item.Cells["Amount"].Text.ToString();
			if (Conversion.ToInt(item.Cells["ReferenceAccountId"].Value) > 0)
			{
				CmbReferenceAccount.Value = Conversion.ToInt(item.Cells["ReferenceAccountId"].Value);
			}
			cmbBranchName.Value = Conversion.ToInt(item.Cells["BranchId"].Value);
			CmbCostCenter.Value = Conversion.ToInt(item.Cells["CostCenterId"].Value);
			((Control)(object)CmbCostCenter).Text = ((Conversion.ToInt(item.Cells["CostCenterId"].Value) > 0) ? Conversion.ToString(item.Cells["CostCenterName"].Value) : "");
			CmbChequeNo.Value = Conversion.ToInt(item.Cells["ChequeId"].Value);
			datChequeDate.Value = Conversion.ToDateTime(item.Cells["ChequeDate"].Value);
			((Control)(object)CmbChequeNo).Text = Conversion.ToString(item.Cells["ChequeNo"].Value);
			txtPayTitle.Text = Conversion.ToString(item.Cells["PayTitle"].Value);
			CmbAccountId.Focus();
			Add.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelUpdateDetial.Visible = true;
			EnumerableRowCollection<DataRow> rows = from row in dtGridCostCenterDetail.AsEnumerable()
				where row.Field<int>("LineId") == Conversion.ToInt(item.Cells["LineId"].Value)
				select row;
			DataTable CostCenterDetail = (rows.Any() ? rows.CopyToDataTable() : null);
			if (CostCenterDetail != null && CostCenterDetail.Rows.Count > 0)
			{
				GridEXRow[] dataRows = grdCostCenter.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					DataRow singleRow = CostCenterDetail.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("CostCenterId") == Conversion.ToInt(r.Cells["Id"].Value));
					if (singleRow != null)
					{
						r.BeginEdit();
						r.IsChecked = true;
						r.Cells["Prcnt"].Value = Conversion.ToDouble(singleRow["Percent"]);
						r.Cells["Amount"].Value = Conversion.ToDouble(singleRow["Amount"]);
						r.EndEdit();
					}
					else
					{
						r.BeginEdit();
						r.IsChecked = false;
						r.Cells["Prcnt"].Value = 0;
						r.Cells["Amount"].Value = 0;
						r.EndEdit();
					}
				}
				grdCostCenter.UpdateData();
			}
			else
			{
				grdCostCenter.ClearStructure();
				GridCostCenterFill();
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
			if (((UltraGridBase)CmbAccountId).ActiveRow == null || Conversion.ToInt(CmbAccountId.Value) == 0)
			{
				MessageBox.Show("Account Title Field Required");
				CmbAccountId.Focus();
				return;
			}
			if (MultiCurrencyGridWorking && Conversion.ToInt(CmbAccountId.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbAccountId).SelectedRow.Cells[6].Value) == 0)
			{
				MessageBox.Show("Debit Account Currency not set Please Check");
				CmbAccountId.Focus();
				return;
			}
			if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountDatasourceLength > 0 && (((UltraGridBase)CmbSubsidiaryAccount).ActiveRow == null || Conversion.ToInt(CmbSubsidiaryAccount.Value) == 0))
			{
				MessageBox.Show("Subsidiary Account Title Field Required");
				CmbSubsidiaryAccount.Focus();
				return;
			}
			if (((UltraGridBase)CmbJobLotId).ActiveRow == null || Conversion.ToInt(CmbJobLotId.Value) == 0)
			{
				MessageBox.Show("Job/Lot Field Required");
				CmbJobLotId.Focus();
				return;
			}
			if (Conversion.ToInt(cmbTcyCodeDetail.Value) == 0)
			{
				MessageBox.Show("Tcy Code Field is Required");
				cmbTcyCodeDetail.Focus();
				return;
			}
			if (txtTcyExchangeRateDetail.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Tcy Exchange Rate Field is Required");
				txtTcyExchangeRateDetail.Focus();
				return;
			}
			if (Conversion.ToDouble((object)txtDebitAmount.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Amount Field Required");
				txtDebitAmount.Focus();
				return;
			}
			if (grdCostCenter.GetCheckedRows().Count() > 1)
			{
				double TotalPrcnt = 0.0;
				double Amount = 0.0;
				GridEXRow[] checkedRows = grdCostCenter.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					TotalPrcnt += Conversion.ToDouble(item.Cells["Prcnt"].Value);
					Amount += Conversion.ToDouble(item.Cells["Amount"].Value);
				}
				if (Math.Round(TotalPrcnt, 0) != 100.0 || Amount != Conversion.ToDouble((object)txtDebitAmount.Text))
				{
					throw new Exception("Percent not equal to 100 Or Amount not Equal To Debit Amount");
				}
			}
			if (MultiCurrencyGridWorking)
			{
				if (Conversion.ToInt(txtBaseFcyCode.Tag) == 0)
				{
					MessageBox.Show("Base Fcy Code not set. Please Contact with ERP Team!");
					txtBaseFcyExchangeRate.Focus();
					return;
				}
				if (txtBaseFcyExchangeRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtBaseFcyExchangeRate.Text.Trim()) == 0.0)
				{
					MessageBox.Show("Base Fcy Exchange Rate Field is Required");
					txtBaseFcyExchangeRate.Focus();
					return;
				}
			}
			if (BranchFeature && (((UltraGridBase)cmbBranchName).ActiveRow == null || Conversion.ToInt(cmbBranchName.Value) == 0))
			{
				MessageBox.Show("BranchName Field is Required");
				cmbBranchName.Focus();
				return;
			}
			table.Rows[updateDetailIndex]["AccountCode"] = ((UltraDropDownBase)CmbAccountId).SelectedRow.Cells[2].Value;
			table.Rows[updateDetailIndex]["AccountId"] = CmbAccountId.Value;
			table.Rows[updateDetailIndex]["GlCurrencyId"] = Conversion.ToInt(((UltraDropDownBase)CmbAccountId).SelectedRow.Cells[6].Value);
			table.Rows[updateDetailIndex]["GlCurrency"] = ((UltraDropDownBase)CmbAccountId).SelectedRow.Cells[7].Value;
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
					table.Rows[updateDetailIndex]["SubsidiaryAccountId"] = CmbAccountId.Value;
					table.Rows[updateDetailIndex]["SubsidiaryAccount"] = ((Control)(object)CmbAccountId).Text;
					table.Rows[updateDetailIndex]["SubsidiaryAccountTypeId"] = 4;
				}
			}
			else
			{
				table.Rows[updateDetailIndex]["SubsidiaryAccountId"] = 0;
				table.Rows[updateDetailIndex]["SubsidiaryAccount"] = "";
				table.Rows[updateDetailIndex]["SubsidiaryAccountTypeId"] = 0;
			}
			table.Rows[updateDetailIndex]["JobLotId"] = CmbJobLotId.Value;
			table.Rows[updateDetailIndex]["Remarks"] = txtComments.Text.Trim();
			table.Rows[updateDetailIndex]["TcyCodeId"] = cmbTcyCodeDetail.Value;
			table.Rows[updateDetailIndex]["TcyCode"] = ((Control)(object)cmbTcyCodeDetail).Text;
			table.Rows[updateDetailIndex]["TcyExchangeRate"] = txtTcyExchangeRateDetail.Text;
			table.Rows[updateDetailIndex]["FcyAmount"] = txtFcyAmountDetail.Text;
			table.Rows[updateDetailIndex]["Amount"] = txtDebitAmount.Text.Trim();
			BindSubsidiaryAccount();
			txtExchangeRate_TextChanged(null, null);
			table.Rows[updateDetailIndex]["ReferenceAccountId"] = Conversion.ToInt(CmbReferenceAccount.Value);
			table.Rows[updateDetailIndex]["ReferenceAccount"] = ((Conversion.ToInt(CmbReferenceAccount.Value) > 0) ? Conversion.ToString(((UltraDropDownBase)CmbReferenceAccount).SelectedRow.Cells[1].Value) : "");
			int lineId = Conversion.ToInt(table.Rows[updateDetailIndex]["LineId"]);
			table.Rows[updateDetailIndex]["BranchId"] = Conversion.ToInt(cmbBranchName.Value);
			table.Rows[updateDetailIndex]["BranchName"] = ((Control)(object)cmbBranchName).Text;
			if (grdCostCenter.GetCheckedRows().Count() > 1)
			{
				table.Rows[updateDetailIndex]["CostCenterId"] = 0;
				table.Rows[updateDetailIndex]["CostCenterName"] = "";
			}
			else
			{
				table.Rows[updateDetailIndex]["CostCenterId"] = Conversion.ToInt(CmbCostCenter.Value);
				table.Rows[updateDetailIndex]["CostCenterName"] = ((Control)(object)CmbCostCenter).Text;
			}
			table.Rows[updateDetailIndex]["ChequeId"] = Conversion.ToInt(CmbChequeNo.Value);
			table.Rows[updateDetailIndex]["ChequeDate"] = Conversion.ToDateTime((object)datChequeDate.Value);
			table.Rows[updateDetailIndex]["ChequeNo"] = Conversion.ToString((object)((Control)(object)CmbChequeNo).Text);
			table.Rows[updateDetailIndex]["PayTitle"] = Conversion.ToString((object)txtPayTitle.Text);
			if ((from rrr in grdCostCenterDetail.GetDataRows()
				where Conversion.ToInt(rrr.Cells["LineId"].Value) == lineId
				select rrr).Count() == 0)
			{
				InsertCostCenterBreakUp(lineId);
			}
			else
			{
				UpdateCostCenterBreakUp(lineId);
			}
			SummaryForCostCenterBreakUp();
			GridCostCenterFill();
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			CmbAccountId.Focus();
			txtComments.Clear();
			txtFcyAmountDetail.Clear();
			txtDebitAmount.Clear();
			((Control)(object)CmbAccountId).Text = string.Empty;
			txtFcyAmountDetail.AccessibleName = "";
			txtDebitAmount.AccessibleName = "";
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

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		Add.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		CmbAccountId.Focus();
		txtComments.Clear();
		txtDebitAmount.Clear();
		((Control)(object)CmbAccountId).Text = string.Empty;
		GridCostCenterFill();
	}

	private void Add_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdCostCenterDetailSummary")
			{
				CmbAccountId.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!shiftTabPressed)
			{
				CmbAccountId.Focus();
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

	private void btnCancelUpdateDetial_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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

	private void UpdateCostCenterGrid(int LineId)
	{
		try
		{
			GridEXRow[] dataRows = grdCostCenterDetail.GetDataRows();
			GridEXRow[] costCenterRows = grdCostCenter.GetDataRows();
			foreach (GridEXRow detailRow in dataRows.Where((GridEXRow rr) => Conversion.ToInt(rr.Cells["LineId"].Value) == LineId).ToList())
			{
				int parentId = Conversion.ToInt(detailRow.Cells["ParentId"].Value);
				if (parentId <= 0)
				{
					continue;
				}
				{
					foreach (GridEXRow item in costCenterRows.Where((GridEXRow rm) => Conversion.ToInt(rm.Cells["ParentCostCenterId"].Value) == parentId).ToList())
					{
						item.IsChecked = true;
						item.Cells["Prct"].Value = Conversion.ToDouble(detailRow.Cells["Percent"].Value);
						item.Cells["Amount"].Value = Conversion.ToDouble(detailRow.Cells["Amount"].Value);
					}
					break;
				}
			}
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
			if (((UltraGridBase)CmbAccountId).ActiveRow == null || Conversion.ToInt(CmbAccountId.Value) == 0)
			{
				MessageBox.Show("Debit Account Title Field Required");
				CmbAccountId.Focus();
				return;
			}
			if (MultiCurrencyGridWorking && Conversion.ToInt(CmbAccountId.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbAccountId).SelectedRow.Cells[6].Value) == 0)
			{
				MessageBox.Show("Debit Account Currency not set Please Check");
				CmbAccountId.Focus();
				return;
			}
			if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountDatasourceLength > 0 && (((UltraGridBase)CmbSubsidiaryAccount).ActiveRow == null || Conversion.ToInt(CmbSubsidiaryAccount.Value) == 0))
			{
				MessageBox.Show("Subsidiary Account Title Field Required");
				CmbSubsidiaryAccount.Focus();
				return;
			}
			if (((UltraGridBase)CmbJobLotId).ActiveRow == null || Conversion.ToInt(CmbJobLotId.Value) == 0)
			{
				MessageBox.Show("Job/Lot Field Required");
				CmbJobLotId.Focus();
				return;
			}
			if (Conversion.ToInt(cmbTcyCodeDetail.Value) == 0)
			{
				MessageBox.Show("Tcy Code Field is Required");
				cmbTcyCodeDetail.Focus();
				return;
			}
			if (txtTcyExchangeRateDetail.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Tcy Exchange Rate Field is Required");
				txtTcyExchangeRateDetail.Focus();
				return;
			}
			if (Conversion.ToDouble((object)txtDebitAmount.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Amount Field Required");
				txtDebitAmount.Focus();
				return;
			}
			if (grdCostCenter.GetCheckedRows().Count() > 1)
			{
				double TotalPrcnt = 0.0;
				double Amount = 0.0;
				GridEXRow[] checkedRows = grdCostCenter.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					TotalPrcnt += Conversion.ToDouble(item.Cells["Prcnt"].Value);
					Amount += Conversion.ToDouble(item.Cells["Amount"].Value);
				}
				if (Math.Round(TotalPrcnt, 0) != 100.0 || Amount != Conversion.ToDouble((object)txtDebitAmount.Text))
				{
					throw new Exception("Percent not equal to 100 Or Amount not Equal To Debit Amount");
				}
			}
			if (BranchFeature && (((UltraGridBase)cmbBranchName).ActiveRow == null || Conversion.ToInt(cmbBranchName.Value) == 0))
			{
				MessageBox.Show("BranchName Field is Required");
				cmbBranchName.Focus();
				return;
			}
			if (MultiCurrencyGridWorking)
			{
				if (Conversion.ToInt(txtBaseFcyCode.Tag) == 0)
				{
					MessageBox.Show("Base Fcy Code not set. Please Contact with ERP Team!");
					txtBaseFcyExchangeRate.Focus();
					return;
				}
				if (txtBaseFcyExchangeRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtBaseFcyExchangeRate.Text.Trim()) == 0.0)
				{
					MessageBox.Show("Base Fcy Exchange Rate Field is Required");
					txtBaseFcyExchangeRate.Focus();
					return;
				}
			}
			int LineId = 1;
			if (grd.GetRows().Count() > 0)
			{
				LineId = Conversion.ToInt((object)grd.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["LineId"].Value)));
				LineId++;
			}
			table.Rows.Add(((UltraDropDownBase)CmbAccountId).SelectedRow.Cells[2].Value, CmbAccountId.Value, Conversion.ToInt(((UltraDropDownBase)CmbAccountId).SelectedRow.Cells[6].Value), Conversion.ToString(((UltraDropDownBase)CmbAccountId).SelectedRow.Cells[7].Value), (!SubsidiaryAccountAllownOnVouchers) ? ((object)0) : ((SubsdiaryAccountDatasourceLength > 0) ? CmbSubsidiaryAccount.Value : CmbAccountId.Value), (!SubsidiaryAccountAllownOnVouchers) ? "" : ((SubsdiaryAccountDatasourceLength > 0) ? ((Control)(object)CmbSubsidiaryAccount).Text : ((Control)(object)CmbAccountId).Text), (!((Control)(object)CmbSubsidiaryAccount).Visible) ? ((object)0) : ((SubsdiaryAccountDatasourceLength > 0) ? ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value : ((object)4)), CmbJobLotId.Value, txtComments.Text, cmbTcyCodeDetail.Value, ((Control)(object)cmbTcyCodeDetail).Text, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), Conversion.ToDouble((object)txtFcyAmountDetail.Text), txtDebitAmount.Text, Conversion.ToInt(CmbReferenceAccount.Value), (Conversion.ToInt(CmbReferenceAccount.Value) > 0) ? Conversion.ToString(((UltraDropDownBase)CmbReferenceAccount).SelectedRow.Cells[1].Value) : "", LineId, (grdCostCenter.GetCheckedRows().Count() <= 1) ? Conversion.ToInt(CmbCostCenter.Value) : 0, (grdCostCenter.GetCheckedRows().Count() > 1) ? "" : Conversion.ToString((object)((Control)(object)CmbCostCenter).Text), (Conversion.ToInt(cmbBranchName.Value) > 0) ? Conversion.ToInt(cmbBranchName.Value) : 0, Conversion.ToString((object)((Control)(object)cmbBranchName).Text), Conversion.ToInt(CmbChequeNo.Value), Conversion.ToDateTime((object)datChequeDate.Value), Conversion.ToString((object)((Control)(object)CmbChequeNo).Text), Conversion.ToString((object)txtPayTitle.Text));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			BindSubsidiaryAccount();
			CalculateTotalInformation();
			InsertCostCenterBreakUp(LineId);
			GridCostCenterFill();
			SummaryForCostCenterBreakUp();
			((Control)(object)CmbAccountId).Text = string.Empty;
			txtFcyAmountDetail.Clear();
			txtComments.Clear();
			txtDebitAmount.Clear();
			CmbAccountId.Focus();
			txtFcyAmountDetail.AccessibleName = "";
			txtDebitAmount.AccessibleName = "";
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

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdCostCenterDetailSetting()
	{
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdCostCenterDetail.RootTable.Columns["ParentId"].Visible = false;
			grdCostCenterDetail.RootTable.Columns["Id"].Visible = false;
			grdCostCenterDetail.RootTable.Columns["CostCenterId"].Visible = false;
			grdCostCenterDetail.RootTable.Columns["Percent"].TextAlignment = (TextAlignment)3;
			grdCostCenterDetail.RootTable.Columns["Percent"].FormatString = "#,##0.###";
			grdCostCenterDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdCostCenterDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdCostCenterDetail.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			grdCostCenterDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
			grdCostCenterDetail.ColumnAutoResize = true;
			grdCostCenterDetail.RootTable.Columns["LineId"].Width = 40;
			grdCostCenterDetail.RootTable.Columns["CostCenter"].Width = 150;
			grdCostCenterDetail.RootTable.Columns["Percent"].Width = 60;
			grdCostCenterDetail.RootTable.Columns["Amount"].Width = 90;
			grdCostCenterDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdCostCenterDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteCostCenterDetail(int LineId)
	{
		try
		{
			if (LineId <= 0)
			{
				return;
			}
			foreach (DataRow row2 in (from row in dtGridCostCenterDetail.AsEnumerable()
				where row.Field<int>("LineId") == LineId
				select row).ToList())
			{
				dtGridCostCenterDetail.Rows.Remove(row2);
			}
			((Control)(object)grdCostCenterDetail).Refresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SummaryForCostCenterBreakUp()
	{
		try
		{
			if (dtGridCostCenterDetail.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("CostCenter", typeof(string));
				dt.Columns.Add("Amount", typeof(double));
				List<int> uniqueLineIds = (from row in dtGridCostCenterDetail.AsEnumerable()
					select row.Field<int>("CostCenterId")).Distinct().ToList();
				int i;
				for (i = 0; i < uniqueLineIds.Count; i++)
				{
					DataRow row2 = dtGridCostCenterDetail.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<int>("CostCenterId") == uniqueLineIds[i]);
					double Amount = 0.0;
					GridEXRow[] rows = grdCostCenterDetail.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToInt(item.Cells["CostCenterId"].Value) == uniqueLineIds[i])
						{
							Amount += Conversion.ToDouble(item.Cells["Amount"].Value);
						}
					}
					dt.Rows.Add(row2["CostCenter"], Amount);
				}
				grdCostCenterDetailSummary.DataSource = dt;
				grdCostCenterDetailSummary.RetrieveStructure();
				grdCostCenterDetailSummary.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdCostCenterDetailSummary.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdCostCenterDetailSummary.RootTable.Columns["Amount"].FormatString = "#,##0.###";
				grdCostCenterDetailSummary.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
				grdCostCenterDetailSummary.RootTable.Columns["CostCenter"].Width = 190;
				grdCostCenterDetailSummary.RootTable.Columns["Amount"].Width = 100;
			}
			else
			{
				grdCostCenterDetailSummary.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridCostCenterFill()
	{
		try
		{
			grdCostCenter.ClearStructure();
			if (dtAllSubCostCenters.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("CostCenter", typeof(string));
				dtGrid.Columns.Add("ParentCostCenterId", typeof(int));
				dtGrid.Columns.Add("ParentCostCenter", typeof(string));
				dtGrid.Columns.Add("Prcnt", typeof(double));
				dtGrid.Columns.Add("Amount", typeof(double));
				for (int i = 0; i < dtAllSubCostCenters.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dtAllSubCostCenters.Rows[i]["Id"], dtAllSubCostCenters.Rows[i]["CostCenterName"], dtAllSubCostCenters.Rows[i]["ParentCostCenterId"], dtAllSubCostCenters.Rows[i]["parentCostCenterName"], 0, 0);
				}
				grdCostCenter.DataSource = dtGrid;
				grdCostCenter.RetrieveStructure();
				grdCostCenter.RootTable.Columns["ParentCostCenterId"].Visible = false;
				grdCostCenter.RootTable.Columns["Id"].Visible = false;
				grdCostCenter.RootTable.Groups.Add("ParentCostCenter");
				grdCostCenter.RootTable.Columns["ParentCostCenter"].Visible = false;
				grdCostCenter.GroupByBoxVisible = true;
				grdCostCenter.RootTable.Columns["Id"].EditType = (EditType)0;
				grdCostCenter.RootTable.Columns["ParentCostCenterId"].EditType = (EditType)0;
				grdCostCenter.RootTable.Columns["ParentCostCenter"].EditType = (EditType)0;
				grdCostCenter.RootTable.Columns["CostCenter"].EditType = (EditType)0;
				grdCostCenter.RootTable.Columns.Add("Select");
				grdCostCenter.RootTable.Columns["Select"].ActAsSelector = true;
				grdCostCenter.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdCostCenter.RootTable.Columns["Select"].Width = 30;
				grdCostCenter.RootTable.Columns["Select"].Position = 0;
				grdCostCenter.RootTable.Columns["Prcnt"].AggregateFunction = (AggregateFunction)2;
				grdCostCenter.RootTable.Columns["Prcnt"].TextAlignment = (TextAlignment)3;
				grdCostCenter.RootTable.Columns["Prcnt"].FormatString = "#,##0.###";
				grdCostCenter.RootTable.Columns["Prcnt"].TotalFormatString = "#,##0.###";
				grdCostCenter.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdCostCenter.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdCostCenter.RootTable.Columns["Amount"].FormatString = "#,##0.###";
				grdCostCenter.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
				grdCostCenter.RootTable.Columns["CostCenter"].Width = 100;
				grdCostCenter.RootTable.Columns["Prcnt"].Width = 50;
				grdCostCenter.RootTable.Columns["Amount"].Width = 100;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCostCenter_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdCostCenter.UpdateData();
			GridEXRow row = grdCostCenter.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				double TotalPrcnt = 0.0;
				double TotalAmount = 0.0;
				double DebitAmount = 0.0;
				if (Conversion.ToDouble((object)txtDebitAmount.Text) > 0.0)
				{
					DebitAmount = Conversion.ToDouble((object)txtDebitAmount.Text);
				}
				if (e.Column.Key == "Prcnt")
				{
					GridEXRow[] checkedRows = grdCostCenter.GetCheckedRows();
					foreach (GridEXRow item2 in checkedRows)
					{
						TotalPrcnt += Conversion.ToDouble(item2.Cells["Prcnt"].Value);
					}
					double prcnt = Conversion.ToDouble(row.Cells["Prcnt"].Value);
					if (prcnt > 100.0 || TotalPrcnt > 100.0)
					{
						row.Cells["Prcnt"].Value = 0;
						prcnt = 0.0;
						MessageBox.Show("Total Percentage Can't greater than 100", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					double amount = DebitAmount * (prcnt / 100.0);
					row.Cells["Amount"].Value = amount;
				}
				if (e.Column.Key == "Amount")
				{
					GridEXRow[] checkedRows = grdCostCenter.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						TotalAmount += Conversion.ToDouble(item.Cells["Amount"].Value);
					}
					double Amount = Conversion.ToDouble(row.Cells["Amount"].Value);
					if (Amount > DebitAmount || TotalAmount > DebitAmount)
					{
						row.Cells["Amount"].Value = 0;
						Amount = 0.0;
						MessageBox.Show($"Total Amount Can't greater than DebitAmount {DebitAmount}", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					double Prcnt = ((DebitAmount > 0.0) ? (Amount / DebitAmount * 100.0) : 0.0);
					row.Cells["Prcnt"].Value = Math.Round(Prcnt, 3);
				}
				row.IsChecked = Conversion.ToDouble(row.Cells["Prcnt"].Value) > 0.0;
			}
			grdCostCenter.UpdateData();
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
			grdMultiCurrency.RootTable.Columns["LocalCurrency"].Width = Constants.InventoryConstants.FcyConstant + 20;
			grdMultiCurrency.RootTable.Columns["To"].Width = 35;
			grdMultiCurrency.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 15;
			grdMultiCurrency.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Expected O, but got Unknown
		//IL_1183: Unknown result type (might be due to invalid IL or missing references)
		//IL_118a: Expected O, but got Unknown
		//IL_0bf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfe: Expected O, but got Unknown
		//IL_13af: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b6: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
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
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (RecId > 0)
			{
				vh.Id = RecId;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r.Cells["AccountId"].Value) == 0)
				{
					MessageBox.Show("Please Select Account Title First");
					return;
				}
			}
			vh.Type = (VoucherType)0;
			vh.DocumentTypeId = 26;
			vh.VoucherCode = Conversion.ToInt((object)txtVoucherCode.Text.Trim());
			vh.ProjectId = Conversion.ToInt((object)CmbProjectId.Value.ToString());
			vh.VoucherDate = datVoucherDate.Value;
			vh.RefAccountId = Conversion.ToInt(CmbRefAccountId.Value);
			vh.AgainstAccountId = Conversion.ToInt(CmbRefAccountId.Value);
			vh.RefDocNoId = Conversion.ToInt(CmbRefAccountId.Value);
			vh.Remarks = txtRemarks.Text.Trim();
			vh.IncludeWHT = false;
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.EntryUser = UserAccount.ID;
			vh.ModifyUser = UserAccount.ID;
			vh.MultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
			vh.ExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			vh.FcAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
			vh.CustomAccounts = chkCustomAccounts.Checked;
			vh.voucherDetailList = new List<VoucherDetail>();
			if (grd.RowCount > 0)
			{
				double DebitAmount = 0.0;
				double VoucherAmount = 0.0;
				int LineId = 1;
				rows = grd.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.SubsidiaryTypeId = Conversion.ToInt(r2.Cells["SubsidiaryAccountTypeId"].Value);
					int SubsidiaryAccountId = Conversion.ToInt(r2.Cells["SubsidiaryAccountId"].Value);
					if (SubsidiaryAccountId > 0)
					{
						vd.SupplierCustomerId = ((vd.SubsidiaryTypeId == 1) ? SubsidiaryAccountId : 0);
						vd.EmployeeId = ((vd.SubsidiaryTypeId == 2) ? SubsidiaryAccountId : 0);
						vd.SubsidiaryAccountId = SubsidiaryAccountId;
					}
					vd.AccountId = Conversion.ToInt(r2.Cells["AccountId"].Value);
					List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Apply On Actual Expenses").ToList();
					if (lst.Count > 0 && bool.Parse(lst[0].ConfigKey.ToString()))
					{
						DataTable dt = global::Architecture.BLL.Accounts.AccountsBudgetHeader.BudgetBalances(new ReportsParameters
						{
							DocDate = vh.VoucherDate,
							AccountId = vd.AccountId
						});
						if (dt.Rows.Count > 0 && dt != null)
						{
							GridEXRow[] rows2 = grd.GetRows();
							foreach (GridEXRow x2 in rows2)
							{
								if (Conversion.ToInt(dt.Rows[0]["AccountId"]) == Conversion.ToInt((object)vd.AccountId))
								{
									DebitAmount += Conversion.ToDouble(x2.Cells["Amount"].Value);
								}
							}
							if (Conversion.ToInt(dt.Rows[0]["AccountId"]) == Conversion.ToInt((object)vd.AccountId))
							{
								List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "MonthlyBudget").ToList();
								if (list.Count > 0)
								{
									double BalanceAmount = Conversion.ToDouble(dt.Rows[0]["DebitAmount"]);
									double TotalUtilizeAmount = 0.0;
									if (Save.Visible && Save.Enabled)
									{
										TotalUtilizeAmount = DebitAmount;
									}
									else
									{
										TotalUtilizeAmount = DebitAmount;
										BalanceAmount += DebitAmtGet;
									}
									double DiffAmount = Math.Round(BalanceAmount) - Math.Round(TotalUtilizeAmount);
									if (list[0].ConfigKey.ToString() == "Stop" && TotalUtilizeAmount > BalanceAmount)
									{
										throw new Exception("Accumulated Monthly Budget For " + r2.Cells["AccountId"].Text.ToString() + " Up is" + BalanceAmount + " It will exceed by " + DiffAmount);
									}
									if (list[0].ConfigKey.ToString() == "Warn" && TotalUtilizeAmount > BalanceAmount)
									{
										MessageBox.Show("Accumulated Monthly Budget For " + r2.Cells["AccountId"].Text.ToString() + " Up is" + BalanceAmount + " It will exceed by " + DiffAmount, "Confirm", MessageBoxButtons.OK);
									}
								}
							}
						}
					}
					vd.LineId = LineId;
					vd.SortNo = Conversion.ToInt(r2.Cells["LineId"].Value);
					vd.AgainstAccountId = Conversion.ToInt(CmbRefAccountId.Value);
					vd.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
					vd.Comments = Conversion.ToString(r2.Cells["Remarks"].Value);
					vd.DebitAmount = Conversion.ToDouble(r2.Cells["Amount"].Value);
					if (((UltraGridBase)CmbRefAccountId).ActiveRow != null && vd.AgainstAccountId > 0 && Conversion.ToInt(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells["AccountTypeId"].Value) == 15)
					{
						vd.InvoiceNoRefId = Conversion.ToInt(r2.Cells["ChequeId"].Value);
						vd.DCheqDate = Conversion.ToDateTime(r2.Cells["ChequeDate"].Value);
						vd.CheqNoDetail = Conversion.ToString(r2.Cells["ChequeNo"].Value);
						vd.PayeeTitle = Conversion.ToString(r2.Cells["PayTitle"].Value);
						if (string.IsNullOrWhiteSpace(vh.ChequeNo))
						{
							vh.CheqId = Conversion.ToInt(r2.Cells["ChequeId"].Value);
							vh.ChequeDate = Conversion.ToDateTime(r2.Cells["ChequeDate"].Value);
							vh.ChequeNo = Conversion.ToString(r2.Cells["ChequeNo"].Value);
							vh.PayTitle = Conversion.ToString(r2.Cells["PayTitle"].Value);
						}
					}
					vd.IsTaxable = "False";
					vd.CostCenterId = Conversion.ToInt(r2.Cells["CostCenterId"].Value);
					vd.ReferenceAccountId = Conversion.ToInt(r2.Cells["ReferenceAccountId"].Value);
					vd.DMultiCurrencyId = Conversion.ToInt(r2.Cells["TcyCodeId"].Value);
					vd.DExchangeCurrencyRate = Conversion.ToDouble(r2.Cells["TcyExchangeRate"].Value);
					vd.DCurrencyAmount = Conversion.ToDouble(r2.Cells["FcyAmount"].Value);
					vd.BranchesId = (BranchFeature ? Conversion.ToInt(r2.Cells["BranchId"].Value) : UserAccount.BranchesId);
					if (vd.DCurrencyAmount == 0.0)
					{
						throw new Exception("Fcy Amount Not Found In Detail Grid at Row#" + (r2.RowIndex + 1));
					}
					vd.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
					if (MultiCurrencyGridWorking)
					{
						vd.ThirdCurrencyId = Conversion.ToInt(r2.Cells["GlCurrencyId"].Value);
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
					vh.voucherDetailList.Add(vd);
					LineId++;
					VoucherDetail vd2 = new VoucherDetail();
					vd2.LineId = LineId;
					vd2.SortNo = Conversion.ToInt(r2.Cells["LineId"].Value);
					vd2.AccountId = Conversion.ToInt(CmbRefAccountId.Value);
					vd2.AgainstAccountId = Conversion.ToInt(r2.Cells["AccountId"].Value);
					vd2.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
					vd2.Comments = Conversion.ToString(r2.Cells["Remarks"].Value);
					vd2.CreditAmount = Conversion.ToDouble(r2.Cells["Amount"].Value);
					vd2.InvoiceNoRefId = vh.CheqId;
					vd2.CheqNoDetail = vh.ChequeNo;
					vd2.DCheqDate = vh.ChequeDate;
					vd2.PayeeTitle = vh.PayTitle;
					vd2.IsTaxable = "False";
					vd2.CostCenterId = Conversion.ToInt(r2.Cells["CostCenterId"].Value);
					vd2.ReferenceAccountId = Conversion.ToInt(r2.Cells["ReferenceAccountId"].Value);
					vd2.BranchesId = (BranchFeature ? Conversion.ToInt(r2.Cells["BranchId"].Value) : UserAccount.BranchesId);
					vd2.DMultiCurrencyId = Conversion.ToInt(r2.Cells["TcyCodeId"].Value);
					vd2.DExchangeCurrencyRate = Conversion.ToDouble(r2.Cells["TcyExchangeRate"].Value);
					vd2.DCurrencyAmount = Conversion.ToDouble(r2.Cells["FcyAmount"].Value);
					if (vd2.DCurrencyAmount == 0.0)
					{
						throw new Exception("Fcy Amount Not Found In Detail Grid at Row#" + (r2.RowIndex + 1));
					}
					if (((UltraGridBase)CmbRefAccountId).ActiveRow != null && vd2.AccountId > 0 && Conversion.ToInt(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells["AccountTypeId"].Value) == 15)
					{
						vd2.InvoiceNoRefId = Conversion.ToInt(r2.Cells["ChequeId"].Value);
						vd2.DCheqDate = Conversion.ToDateTime(r2.Cells["ChequeDate"].Value);
						vd2.CheqNoDetail = Conversion.ToString(r2.Cells["ChequeNo"].Value);
						vd2.PayeeTitle = Conversion.ToString(r2.Cells["PayTitle"].Value);
					}
					VoucherAmount += vd2.CreditAmount;
					vd2.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
					if (MultiCurrencyGridWorking)
					{
						vd2.ThirdCurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells["CurrencyId"].Value);
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
				if (AmountLimitForExpenseVoucher > 0.0 && VoucherAmount > AmountLimitForExpenseVoucher)
				{
					throw new Exception($"Total VoucherAmount {VoucherAmount} can not be grater than Amount Limit {AmountLimitForExpenseVoucher}");
				}
				vh.VoucherAmount = VoucherAmount;
				if (MultiCurrencyGridWorking)
				{
					rows = grdMultiCurrency.GetDataRows();
					foreach (GridEXRow item2 in rows)
					{
						VoucherDetailMultiCurrency d2 = new VoucherDetailMultiCurrency();
						d2.Id = Conversion.ToInt(item2.Cells["Id"].Value);
						d2.LocalCurrencyId = Conversion.ToInt(item2.Cells["LocalCurencyId"].Value);
						d2.GlCurrencyId = Conversion.ToInt(item2.Cells["GlCurrencyId"].Value);
						d2.ExchangeRate = Conversion.ToDouble(item2.Cells["ExchangeRate"].Value);
						vh.VoucherDetailMultiCurrencyList.Add(d2);
					}
				}
				vh.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				vh.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				if (string.IsNullOrEmpty(AttachmentsValues) || AT.lst.Count > 0)
				{
					AttachmentsValues = "";
				}
				FormHelper.UpdateAttachmentsForObject<VoucherHead>(vh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				vh.AttachmentsValues = AttachmentsValues;
				vh.CustomAttachmentsValues = CustomAttachmentsValues;
				if (!DisableBothNegativeBalanceRestrictions)
				{
					double ClosingBalance = CommonServices.GetAccountBalance(vh.RefAccountId, Conversion.ToDateTime((object)datVoucherDate.Value));
					ClosingBalance = Math.Round(ClosingBalance);
					if (VoucherAmount > ClosingBalance)
					{
						if (PreventNegativeBalanceEntry)
						{
							MessageBox.Show("Debit Amount cannot be greater than Account Balance.\n" + $"Account '{Conversion.ToString((object)((Control)(object)CmbRefAccountId).Text)}' Balance is {ClosingBalance} and Total Debit Amount is {VoucherAmount}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
						if (DisplayWarningforNegativeBalance && MessageBox.Show($"Debit Amount cannot be greater than Account Balance.\nAccount '{Conversion.ToString((object)((Control)(object)CmbRefAccountId).Text)}' Balance is {ClosingBalance} and Total Debit Amount is {VoucherAmount}.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
						{
							return;
						}
					}
				}
				vh.VoucherAmount = VoucherAmount;
				if (!global::Architecture.BLL.Accounts.VoucherHead.VoucherExistWithSameAmountInSameDate(vh))
				{
					return;
				}
				vh.costCenterDetailList = new List<voucherCostCenterDetail>();
				rows = grdCostCenterDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					voucherCostCenterDetail d = new voucherCostCenterDetail();
					d.Id = Conversion.ToInt(item.Cells["Id"].Value);
					d.SortNo = Conversion.ToInt(item.Cells["LineId"].Value);
					d.CostCenterId = Conversion.ToInt(item.Cells["CostCenterId"].Value);
					d.costPrcent = Conversion.ToDecimal(item.Cells["Percent"].Value);
					d.costAmount = Conversion.ToDouble(item.Cells["Amount"].Value);
					vh.costCenterDetailList.Add(d);
				}
				foreach (VoucherDetail line in vh.voucherDetailList)
				{
					if (line.DebitAmount > 0.0 && vh.costCenterDetailList.Any((voucherCostCenterDetail b) => b.SortNo == line.SortNo))
					{
						double num = vh.costCenterDetailList.Where((voucherCostCenterDetail b) => b.SortNo == line.SortNo).Sum((voucherCostCenterDetail b) => b.costAmount);
						if (Math.Round(vh.costCenterDetailList.Where((voucherCostCenterDetail b) => b.SortNo == line.SortNo).Sum((voucherCostCenterDetail b) => b.costPrcent), 0) != 100m)
						{
							throw new Exception("Cost Center BreakUp Percent of LineId" + line.LineId + " not equal to 100");
						}
						if (num != line.DebitAmount)
						{
							throw new Exception("Cost Center BreakUp Amount and Detail Amount of LineId" + line.LineId + " not Match");
						}
					}
				}
				int success = global::Architecture.BLL.Accounts.VoucherHead.Save(vh);
				if (RecId == 0)
				{
					MessageBox.Show("Voucher Save Successfully...[" + vh.VoucherCode + "]");
				}
				else
				{
					MessageBox.Show("Voucher Update Successfully...[" + vh.VoucherCode + "]");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 26, 0);
					}
				}
				Reset();
				RecId = 0;
				grd.ClearStructure();
				if (ChkBox.Checked)
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt((object)success));
				}
				else if (chkformat2.Checked)
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(success, 26);
				}
			}
			else
			{
				MessageBox.Show("Grid Fields Required");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InsertCostCenterBreakUp(int LineId)
	{
		try
		{
			if (grdCostCenter.GetCheckedRows().Count() > 1)
			{
				GridEXRow[] checkedRows = grdCostCenter.GetCheckedRows();
				foreach (GridEXRow row in checkedRows)
				{
					dtGridCostCenterDetail.Rows.Add(0, LineId, Conversion.ToInt(row.Cells["ParentCostCenterId"].Value), Conversion.ToString((object)((Control)(object)CmbAccountId).Text), Conversion.ToInt(row.Cells["Id"].Value), Conversion.ToString(row.Cells["CostCenter"].Value), Conversion.ToDouble(row.Cells["Prcnt"].Value), Conversion.ToDouble(row.Cells["Amount"].Value));
				}
			}
			else if (Conversion.ToInt(CmbCostCenter.Value) > 0)
			{
				dtGridCostCenterDetail.Rows.Add(0, LineId, Conversion.ToInt(((UltraDropDownBase)CmbCostCenter).SelectedRow.Cells[2].Value), Conversion.ToString((object)((Control)(object)CmbAccountId).Text), Conversion.ToInt(CmbCostCenter.Value), Conversion.ToString((object)((Control)(object)CmbCostCenter).Text), 100, Conversion.ToDouble((object)txtDebitAmount.Text));
			}
			grdCostCenterDetail.DataSource = dtGridCostCenterDetail;
			grdCostCenterDetail.RetrieveStructure();
			grdCostCenterDetailSetting();
			SummaryForCostCenterBreakUp();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateCostCenterBreakUp(int LineId)
	{
		try
		{
			if (grdCostCenter.GetCheckedRows().Count() == 0 && Conversion.ToInt(CmbCostCenter.Value) == 0)
			{
				if (dtGridCostCenterDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("LineId") == LineId))
				{
					DeleteCostCenterDetail(LineId);
					SummaryForCostCenterBreakUp();
				}
			}
			else if (grdCostCenter.GetCheckedRows().Count() > 1)
			{
				if (!dtGridCostCenterDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("LineId") == LineId))
				{
					return;
				}
				int num = dtGridCostCenterDetail.AsEnumerable().Count((DataRow row) => row.Field<int>("LineId") == LineId);
				int CostCenterCount = grdCostCenter.GetCheckedRows().Count();
				if (num != CostCenterCount)
				{
					DeleteCostCenterDetail(LineId);
					InsertCostCenterBreakUp(LineId);
					SummaryForCostCenterBreakUp();
					return;
				}
				foreach (DataRow row5 in ((DataTable)grdCostCenter.DataSource).Rows)
				{
					int CostCenterId = row5.Field<int>("Id");
					string CostCenter = row5.Field<string>("CostCenter");
					int ParentCostCenterId = row5.Field<int>("ParentCostCenterId");
					double Percent = row5.Field<double>("Prcnt");
					double amount = row5.Field<double>("Amount");
					DataRow[] array = dtGridCostCenterDetail.Select($"CostCenter = '{CostCenter}' AND LineId ='{LineId}'");
					foreach (DataRow row3 in array)
					{
						row3.SetField("LineId", LineId);
						row3.SetField("ParentId", ParentCostCenterId);
						row3.SetField("DebitAccount", ((Control)(object)CmbAccountId).Text);
						row3.SetField("CostCenterId", CostCenterId);
						row3.SetField("CostCenter", CostCenter);
						row3.SetField("Percent", Percent);
						row3.SetField("Amount", amount);
					}
				}
				grdCostCenterDetail.DataSource = dtGridCostCenterDetail;
				grdCostCenterDetail.RetrieveStructure();
				grdCostCenterDetailSetting();
				SummaryForCostCenterBreakUp();
			}
			else
			{
				if (Conversion.ToInt(CmbCostCenter.Value) <= 0 || !dtGridCostCenterDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("LineId") == LineId))
				{
					return;
				}
				if (dtGridCostCenterDetail.AsEnumerable().Count((DataRow row) => row.Field<int>("LineId") == LineId) != 1)
				{
					DeleteCostCenterDetail(LineId);
					InsertCostCenterBreakUp(LineId);
					SummaryForCostCenterBreakUp();
					return;
				}
				int CostCenterId2 = Conversion.ToInt(CmbCostCenter.Value);
				string CostCenter2 = Conversion.ToString((object)((Control)(object)CmbCostCenter).Text);
				int ParentCostCenterId2 = Conversion.ToInt(((UltraDropDownBase)CmbCostCenter).SelectedRow.Cells[2].Value);
				double Percent2 = 100.0;
				double amount2 = Conversion.ToDouble((object)txtDebitAmount.Text);
				DataRow[] array = dtGridCostCenterDetail.Select($"LineId ='{LineId}'");
				foreach (DataRow row4 in array)
				{
					row4.SetField("LineId", LineId);
					row4.SetField("ParentId", ParentCostCenterId2);
					row4.SetField("DebitAccount", ((Control)(object)CmbAccountId).Text);
					row4.SetField("CostCenterId", CostCenterId2);
					row4.SetField("CostCenter", CostCenter2);
					row4.SetField("Percent", Percent2);
					row4.SetField("Amount", amount2);
				}
				grdCostCenterDetail.DataSource = dtGridCostCenterDetail;
				grdCostCenterDetail.RetrieveStructure();
				grdCostCenterDetailSetting();
				SummaryForCostCenterBreakUp();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void Update_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not Update  " + RecId);
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		try
		{
			RecId = Conversion.ToInt((object)ID);
			VoucherHead vh = global::Architecture.BLL.Accounts.VoucherHead.GetByID(RecId);
			if (vh != null)
			{
				Save.Visible = false;
				Update.Visible = true;
				tabControl1.SelectedIndex = 0;
				datVoucherDate.Value = vh.VoucherDate;
				combcreditac_Leave(null, null);
				CmbRefAccountId.Value = vh.RefAccountId;
				txtPayTitle.Text = Conversion.ToString((object)vh.PayTitle);
				((Control)(object)CmbChequeNo).Text = Conversion.ToString((object)vh.ChequeNo);
				datChequeDate.Value = Conversion.ToDateTime((object)vh.ChequeDate);
				txtRemarks.Text = Conversion.ToString((object)vh.Remarks);
				txtVoucherCode.Text = Conversion.ToString((object)vh.VoucherCode);
				cmbCurrency.Value = vh.MultiCurrencyId;
				cmbCurrency_Leave(null, null);
				chkCustomAccounts.Checked = vh.CustomAccounts;
				txtExchangeRate.Text = vh.ExchangeCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtFcyAmount.Text = vh.FcAmount.ToString("#,##0.###");
				if (vh.MultiCurrencyId == 0 || vh.ExchangeCurrencyRate == 0.0)
				{
					DefaultConfigurations();
				}
				AttachmentsValues = vh.AttachmentsValues;
				CustomAttachmentsValues = vh.CustomAttachmentsValues;
				table.Clear();
				table.Columns.Clear();
				if (table.Columns.Count == 0)
				{
					table.Columns.Add("AccountCode");
					table.Columns.Add("AccountId");
					table.Columns.Add("GlCurrencyId", typeof(int));
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
					table.Columns.Add("LineId", typeof(int));
					table.Columns.Add("CostCenterId", typeof(int));
					table.Columns.Add("CostCenterName");
					table.Columns.Add("BranchId", typeof(int));
					table.Columns.Add("BranchName");
					table.Columns.Add("ChequeId", typeof(int));
					table.Columns.Add("ChequeDate", typeof(DateTime));
					table.Columns.Add("ChequeNo", typeof(string));
					table.Columns.Add("PayTitle", typeof(string));
				}
				for (int j = 0; j < vh.voucherDetailList.Count; j++)
				{
					cmbLocationType.Value = ((vh.voucherDetailList.Count > 0) ? Conversion.ToInt((object)vh.voucherDetailList[0].LocationTypeId) : 0);
					List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Apply On Actual Expenses").ToList();
					if (lst.Count > 0 && bool.Parse(lst[0].ConfigKey.ToString()))
					{
						DataTable dt = global::Architecture.BLL.Accounts.AccountsBudgetHeader.BudgetBalances(new ReportsParameters
						{
							DocDate = vh.VoucherDate,
							AccountId = vh.voucherDetailList[j].AccountId
						});
						if (dt.Rows.Count > 0 && dt != null)
						{
							DebitAmtGet += vh.voucherDetailList[j].DebitAmount;
						}
					}
					if (vh.voucherDetailList[j].IsTaxable == "False" && vh.voucherDetailList[j].AccountId != Convert.ToInt32(CmbRefAccountId.Value))
					{
						table.Rows.Add(vh.voucherDetailList[j].AccountCode, vh.voucherDetailList[j].AccountId, vh.voucherDetailList[j].ThirdCurrencyId, vh.voucherDetailList[j].GlcyCode, (vh.voucherDetailList[j].SubsidiaryTypeId == 1) ? vh.voucherDetailList[j].SupplierCustomerId : ((vh.voucherDetailList[j].SubsidiaryTypeId == 2) ? vh.voucherDetailList[j].EmployeeId : ((vh.voucherDetailList[j].SubsidiaryTypeId == 3) ? vh.voucherDetailList[j].SubsidiaryAccountId : ((vh.voucherDetailList[j].SubsidiaryTypeId == 4) ? vh.voucherDetailList[j].SubsidiaryAccountId : 0))), vh.voucherDetailList[j].SubsidiaryAccountTitle, vh.voucherDetailList[j].SubsidiaryTypeId, vh.voucherDetailList[j].JobLotId, vh.voucherDetailList[j].Comments, vh.voucherDetailList[j].DMultiCurrencyId, vh.voucherDetailList[j].TcyCode, vh.voucherDetailList[j].DExchangeCurrencyRate, vh.voucherDetailList[j].DCurrencyAmount, vh.voucherDetailList[j].DebitAmount, vh.voucherDetailList[j].ReferenceAccountId, vh.voucherDetailList[j].ReferenceAccount, vh.voucherDetailList[j].SortNo, vh.voucherDetailList[j].CostCenterId, vh.voucherDetailList[j].CostCenterName, vh.voucherDetailList[j].BranchesId, vh.voucherDetailList[j].BranchName, vh.voucherDetailList[j].InvoiceNoRefId, vh.voucherDetailList[j].DCheqDate, vh.voucherDetailList[j].CheqNoDetail, vh.voucherDetailList[j].PayeeTitle);
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
				dtGridCostCenterDetail.Rows.Clear();
				if (vh.costCenterDetailList != null && vh.costCenterDetailList.Count > 0)
				{
					for (int i = 0; i < vh.costCenterDetailList.Count; i++)
					{
						dtGridCostCenterDetail.Rows.Add(vh.costCenterDetailList[i].Id, vh.costCenterDetailList[i].SortNo, vh.costCenterDetailList[i].ParentCostCenterId, vh.costCenterDetailList[i].AccountTitle, vh.costCenterDetailList[i].CostCenterId, vh.costCenterDetailList[i].CostCenterName, vh.costCenterDetailList[i].costPrcent, vh.costCenterDetailList[i].costAmount);
					}
				}
				grdCostCenterDetail.DataSource = dtGridCostCenterDetail;
				grdCostCenterDetail.RetrieveStructure();
				grdCostCenterDetailSetting();
				SummaryForCostCenterBreakUp();
				dtVoucherDetailMultiCurrency.Rows.Clear();
				foreach (VoucherDetailMultiCurrency item in vh.VoucherDetailMultiCurrencyList)
				{
					dtVoucherDetailMultiCurrency.Rows.Add(item.Id, item.LocalCurrencyId, item.LcyCurrencyCode, "To", item.GlCurrencyId, item.GlFcyCurrencyCode, item.ExchangeRate);
				}
				grdMultiCurrency.DataSource = dtVoucherDetailMultiCurrency;
				grdMultiCurrency.RetrieveStructure();
				GrdMultiCurrencySetting();
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbLocationType).ActiveRow == null || Conversion.ToInt(cmbLocationType.Value) == 0)
		{
			MessageBox.Show("Location Type Field is Required");
			cmbLocationType.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbProjectId.Value) == 0)
		{
			MessageBox.Show("Cost Center Field is Required");
			CmbProjectId.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbRefAccountId.Value) == 0)
		{
			MessageBox.Show("Credit Account Field is Required");
			CmbRefAccountId.Focus();
			return false;
		}
		if (MultiCurrencyGridWorking && Conversion.ToInt(CmbRefAccountId.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells["CurrencyId"].Value) == 0)
		{
			MessageBox.Show("Credit Account Currency not set Please Check");
			CmbRefAccountId.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbCurrency.Value) == 0)
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
		return true;
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			SubsdiaryAccountDatasourceLength = 0;
			CustomAttachmentsValues = "";
			AttachmentsValues = "";
			RecId = 0;
			table.Rows.Clear();
			UpdateMode = false;
			DebitAmtGet = 0.0;
			txtComments.Text = string.Empty;
			txtDebitAmount.Text = string.Empty;
			txtPayTitle.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)CmbAccountId).Text = string.Empty;
			((Control)(object)CmbChequeNo).Text = string.Empty;
			Save.Visible = true;
			datVoucherDate.Focus();
			VoucherNofill();
			Save.Visible = true;
			Update.Visible = false;
			combcreditac_Leave(null, null);
			grd.ClearStructure();
			lblbalancedetail.Visible = false;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtFcyAmount.Text = string.Empty;
			dtGridCostCenterDetail.Rows.Clear();
			grdCostCenterDetail.DataSource = dtGridCostCenterDetail;
			grdCostCenterDetailSummary.ClearStructure();
			lblCrAcGlCurrency.Text = "Credit Ac GLcy:";
			dtVoucherDetailMultiCurrency.Rows.Clear();
			grdMultiCurrency.DataSource = dtVoucherDetailMultiCurrency;
			GridCostCenterFill();
			DefaultConfigurations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
		((Control)(object)CmbRefAccountId).Text = string.Empty;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			LocationType();
			BranchesFill();
			AccountTitleFill();
			combojoblotfill();
			DetailAccountFill();
			BindReferenceAccounts();
			DefaultConfigurations();
			GetAllSubCostCenters();
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "CheqBook Enabled");
			CheqBookEnabled = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseVoucher_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbAccountTitleHistory).Text = string.Empty;
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
			VoucherInfoBox.Visible = false;
			DataGridHistory.ClearStructure();
			grdDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindForHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name != null && base.ActiveControl.Name == "DataGridHistory")
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		HistoryFill();
	}

	private void HistoryFill()
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
			vh.Ids = "26";
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (rdexpdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdexpentry.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdexpmodify.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdexpapproved.Checked)
			{
				if (FromDateHistory.Checked)
				{
					vh.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					vh.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			vh.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			vh.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			vh.AccountId = Conversion.ToInt(CmbAccountTitleHistory.Value);
			if (((Control)(object)cmbApproveStatus).Text == "Approved")
			{
				vh.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatus).Text == "Not Apporved")
			{
				vh.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatus).Text == "All")
			{
				vh.ApprovedFilter = "All";
			}
			DataTable lst = global::Architecture.BLL.Accounts.VoucherHead.VoucherFormHistory(vh);
			if (lst.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id");
				dtHistory.Columns.Add("AccountTypeId", typeof(int));
				dtHistory.Columns.Add("VoucherCode", typeof(int));
				dtHistory.Columns.Add("DocumentTypeId");
				dtHistory.Columns.Add("DocumentTypeCode", typeof(string));
				dtHistory.Columns.Add("VoucherDate", typeof(DateTime));
				dtHistory.Columns.Add("ManualBillNo");
				dtHistory.Columns.Add("AccountTitle");
				dtHistory.Columns.Add("AgainstAccount");
				dtHistory.Columns.Add("Remarks");
				dtHistory.Columns.Add("VoucherAmount", typeof(double));
				dtHistory.Columns.Add("FcyCode");
				dtHistory.Columns.Add("ExchangeRate", typeof(double));
				dtHistory.Columns.Add("FcyAmount", typeof(double));
				dtHistory.Columns.Add("EntryUser");
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser");
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("ApprovedUser");
				dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
				dtHistory.Columns.Add("CheqNo");
				dtHistory.Columns.Add("Attachment");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtHistory.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["AccountTypeId"], lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["ManualBillNo"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["AgainstAccount"], lst.Rows[i]["Remarks"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistory;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
				VoucherInfoBox.Visible = true;
				lblTotalVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalVouchers"]).ToString();
				lblApprovedVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalApprovedVoucher"]).ToString();
				lblUnApprovedVouchers.Text = Conversion.ToInt(lst.Rows[0]["TotalUnApprovedVoucher"]).ToString();
			}
			else
			{
				VoucherInfoBox.Visible = false;
				grdDetail.ClearStructure();
				DataGridHistory.ClearStructure();
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
			DataGridHistory.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["AccountTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
			DataGridHistory.RootTable.Columns["AgainstAccount"].Visible = false;
			DataGridHistory.RootTable.Columns["FcyCode"].Visible = false;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Visible = false;
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["VoucherAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			DataGridHistory.RootTable.Columns["VoucherCode"].Caption = "V.No";
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Caption = "V.Type";
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["VoucherCode"].Width = 50;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Width = 50;
			DataGridHistory.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			DataGridHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 20;
			DataGridHistory.RootTable.Columns["AgainstAccount"].Width = Constants.InventoryConstants.AccountTitle + 20;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			DataGridHistory.RootTable.Columns["VoucherAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["CheqNo"].Width = Constants.InventoryConstants.StringConstant - 40;
			DataGridHistory.RootTable.Columns["Attachment"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 15;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("AddAttachment");
			DataGridHistory.RootTable.Columns["AddAttachment"].Caption = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonText = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].Width = 110;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns["Edit"].Position = 1;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Position = 2;
			DataGridHistory.FrozenColumns = 2;
			DataGridHistory.RootTable.Columns["Remarks"].Width = 350;
			ctrlGrdBar2_Load(null, null);
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
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Edit")
				{
					Reset();
					int ID = Conversion.ToInt(item.Cells["Id"].Value);
					ReadById(ID);
				}
				else if (e.Column.Key == "Print")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Print")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Print_102")
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Conversion.ToInt(item.Cells["Id"].Value), 26, base.Name);
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

	private void VoucherDetailByHeaderId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			VoucherHead vh = global::Architecture.BLL.Accounts.VoucherHead.GetByID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
			DataTable table = new DataTable();
			table.Columns.Add("AccountCode");
			table.Columns.Add("AccountTitle");
			table.Columns.Add("GlCurrency");
			table.Columns.Add("SubsidiaryAccount");
			table.Columns.Add("Job/Lot");
			table.Columns.Add("Remarks");
			table.Columns.Add("TcyCode", typeof(string));
			table.Columns.Add("TcyExchangeRate", typeof(double));
			table.Columns.Add("TcyDebit", typeof(double));
			table.Columns.Add("DebitAmount", typeof(double));
			table.Columns.Add("TcyCredit", typeof(double));
			table.Columns.Add("CreditAmount", typeof(double));
			table.Columns.Add("ReferenceAccount", typeof(string));
			table.Columns.Add("BranchName");
			table.Columns.Add("ChequeDate", typeof(DateTime));
			table.Columns.Add("ChequeNo", typeof(string));
			table.Columns.Add("PayTitle", typeof(string));
			table.Rows.Clear();
			for (int i = 0; i < vh.voucherDetailList.Count; i++)
			{
				if (vh.voucherDetailList[i].DebitAmount > 0.0 || vh.voucherDetailList[i].CreditAmount > 0.0)
				{
					table.Rows.Add(vh.voucherDetailList[i].AccountCode, vh.voucherDetailList[i].AccountTitle, vh.voucherDetailList[i].GlcyCode, vh.voucherDetailList[i].SubsidiaryAccountTitle, vh.voucherDetailList[i].JobLotDescription, vh.voucherDetailList[i].Comments, vh.voucherDetailList[i].TcyCode, vh.voucherDetailList[i].DExchangeCurrencyRate, (vh.voucherDetailList[i].DebitAmount > 0.0) ? vh.voucherDetailList[i].DCurrencyAmount : 0.0, vh.voucherDetailList[i].DebitAmount, (vh.voucherDetailList[i].CreditAmount > 0.0) ? vh.voucherDetailList[i].DCurrencyAmount : 0.0, vh.voucherDetailList[i].CreditAmount, vh.voucherDetailList[i].ReferenceAccount, vh.voucherDetailList[i].BranchName, vh.voucherDetailList[i].DCheqDate, vh.voucherDetailList[i].CheqNoDetail, vh.voucherDetailList[i].PayeeTitle);
				}
			}
			grdDetail.DataSource = table;
			grdDetail.RetrieveStructure();
			int TypeId = Conversion.ToInt(item.Cells["AccountTypeId"].Value);
			grdDetail.RootTable.Columns["ChequeDate"].Visible = TypeId == 15;
			grdDetail.RootTable.Columns["ChequeNo"].Visible = TypeId == 15;
			grdDetail.RootTable.Columns["PayTitle"].Visible = TypeId == 15;
			grdDetail.RootTable.Columns["CreditAmount"].Caption = "Credit";
			grdDetail.RootTable.Columns["DebitAmount"].Caption = "Debit";
			grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grdDetail.RootTable.Columns["GlCurrency"].Visible = MultiCurrencyGridWorking;
			grdDetail.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 3, 3);
			grdDetail.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
			grdDetail.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 20;
			grdDetail.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 15;
			grdDetail.RootTable.Columns["SubsidiaryAccount"].Width = Constants.InventoryConstants.AccountTitle + 20;
			grdDetail.RootTable.Columns["Job/Lot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 30;
			grdDetail.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grdDetail.RootTable.Columns["TcyExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["TcyDebit"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["DebitAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["TcyCredit"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["CreditAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ReferenceAccount"].Width = Constants.InventoryConstants.AccountTitle + 20;
			grdDetail.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdDetail.RootTable.Columns["ChequeDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdDetail.RootTable.Columns["ChequeNo"].Width = Constants.InventoryConstants.StringConstant - 40;
			grdDetail.RootTable.Columns["PayTitle"].Width = Constants.InventoryConstants.StringConstant - 30;
			grdDetail.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TcyExchangeRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TcyExchangeRate"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TcyDebit"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TcyDebit"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TcyDebit"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TcyDebit"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TcyCredit"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TcyCredit"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TcyCredit"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TcyCredit"].TotalFormatString = "#,##0.###";
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 26);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
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

	private void btnPrintFormatII_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.AcRptPaymentReceiptsVoucherSlip_102(RecId, 26);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtvoucherno_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtDebitAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.UpdateRelatedFieldsCalculation(txtDebitAmount, txtFcyAmountDetail, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChangeAmountsOfCostCenter()
	{
		try
		{
			double DebitAmount = Conversion.ToDouble((object)txtDebitAmount.Text);
			GridEXRow[] checkedRows = grdCostCenter.GetCheckedRows();
			foreach (GridEXRow obj in checkedRows)
			{
				double prcnt = Conversion.ToDouble(obj.Cells["Prcnt"].Value);
				double a = DebitAmount * (prcnt / 100.0);
				obj.BeginEdit();
				obj.Cells["Amount"].Value = a;
				obj.EndEdit();
			}
			grdCostCenter.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void txtDebitAmount_Leave(object sender, EventArgs e)
	{
		try
		{
			ChangeAmountsOfCostCenter();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
			{
				GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
				}
			}
			if (e.Control && e.KeyCode == Keys.S)
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
					btnShowHistory_Click(null, null);
				}
			}
			if (Update.Visible && Update.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				Update_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnNewHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					datVoucherDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnRefresh_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnRefreshHistory_Click(null, null);
				}
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
			{
				Print_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formright.DoHavePrintRights)
			{
				btnPrintFormatII_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				datVoucherDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				toolStripButton10_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					CmbAccountId.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateHistory.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save on form tab and for show data in history tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print I");
			dt.Rows.Add("Alt+2", "For Print II");
			dt.Rows.Add("Ctrl+F5", "For Focus on Voucher Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
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
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				int LineId = 0;
				if (item != null && (int)item.RowType == 0)
				{
					LineId = Conversion.ToInt(item.Cells["LineId"].Value);
					item.Delete();
				}
				grd.UpdateData();
				if (MultiCurrencyGridWorking)
				{
					CreateUpdateMultiCurrencyGridInfo();
				}
				CalculateTotalInformation();
				DeleteCostCenterDetail(LineId);
				SummaryForCostCenterBreakUp();
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
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(ID);
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

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void GridEX_Amount_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '.')
		{
			e.Handled = true;
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

	private void txtDebitAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnCostCenterform_Click(object sender, EventArgs e)
	{
		try
		{
			new frmCostCenter(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAccountId_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			int TypeId = 0;
			int Id = Conversion.ToInt(CmbRefAccountId.Value);
			if (((UltraGridBase)CmbRefAccountId).ActiveRow != null && Id > 0)
			{
				TypeId = Conversion.ToInt(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells["AccountTypeId"].Value);
				if (TypeId == 15)
				{
					CheqNoFill();
				}
				else
				{
					((Control)(object)CmbChequeNo).Text = string.Empty;
					((UltraGridBase)CmbChequeNo).DataSource = null;
				}
				ChequeDetailPanel.Visible = TypeId == 15;
			}
			else
			{
				((Control)(object)CmbChequeNo).Text = string.Empty;
				((UltraGridBase)CmbChequeNo).DataSource = null;
			}
			CmbChequeNo.LimitToList = CheqBookEnabled;
			if (grd.RowCount > 0 && table.Rows.Count > 0)
			{
				grd.RootTable.Columns["ChequeDate"].Visible = TypeId == 15;
				grd.RootTable.Columns["ChequeNo"].Visible = TypeId == 15;
				grd.RootTable.Columns["PayTitle"].Visible = TypeId == 15;
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
			CommonServices.UpdateRelatedFieldsCalculation(txtDebitAmount, txtFcyAmountDetail, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			if (((UltraDropDownBase)CmbRefAccountId).SelectedRow != null && ((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells[6].Value != null)
			{
				int headerCurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells[6].Value);
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
			if (((UltraDropDownBase)CmbRefAccountId).SelectedRow != null && ((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells[6].Value != null)
			{
				requiredCurrencyIds.Add(Conversion.ToInt(((UltraDropDownBase)CmbRefAccountId).SelectedRow.Cells[6].Value));
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
		txtDebitAmount_TextChanged(null, null);
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
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Expected O, but got Unknown
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Expected O, but got Unknown
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Expected O, but got Unknown
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Expected O, but got Unknown
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Expected O, but got Unknown
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Expected O, but got Unknown
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Expected O, but got Unknown
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Expected O, but got Unknown
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Expected O, but got Unknown
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Expected O, but got Unknown
		//IL_090d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0917: Expected O, but got Unknown
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Expected O, but got Unknown
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0959: Expected O, but got Unknown
		//IL_09a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Expected O, but got Unknown
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a09: Expected O, but got Unknown
		//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Expected O, but got Unknown
		//IL_0b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Expected O, but got Unknown
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Expected O, but got Unknown
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0e: Expected O, but got Unknown
		//IL_1cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc8: Expected O, but got Unknown
		//IL_2398: Unknown result type (might be due to invalid IL or missing references)
		//IL_23a2: Expected O, but got Unknown
		//IL_23af: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b9: Expected O, but got Unknown
		//IL_23c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d0: Expected O, but got Unknown
		//IL_23dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_23e7: Expected O, but got Unknown
		//IL_901a: Unknown result type (might be due to invalid IL or missing references)
		//IL_9024: Expected O, but got Unknown
		//IL_9048: Unknown result type (might be due to invalid IL or missing references)
		//IL_9052: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.ExpenseVoucherNew));
		GridEXLayout grdCostCenterDetailSummary_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdCostCenter_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdMultiCurrency_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdCostCenterDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance100 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
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
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
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
		Appearance appearance168 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance93 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnEdit = new System.Windows.Forms.ToolStripButton();
		this.btnS = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnDte = new System.Windows.Forms.ToolStripButton();
		this.btnCopy = new System.Windows.Forms.ToolStripButton();
		this.btnPaste = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.grdCostCenterDetailSummary = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdCostCenter = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.MultiCurrencyPanel = new System.Windows.Forms.Panel();
		this.grdMultiCurrency = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label12 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdCostCenterDetail = new GridEX();
		this.label8 = new System.Windows.Forms.Label();
		this.panel23 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.lblCrAcGlCurrency = new System.Windows.Forms.Label();
		this.txtBaseFcyCode = new System.Windows.Forms.TextBox();
		this.lblBaseFcyCode = new System.Windows.Forms.Label();
		this.txtBaseFcyExchangeRate = new System.Windows.Forms.TextBox();
		this.lblBaseFcyExchangeRate = new System.Windows.Forms.Label();
		this.CmbProjectId = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.cmbCurrency = new UltraCombo();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.txtVoucherCode = new System.Windows.Forms.TextBox();
		this.txtbalance = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.datVoucherDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbRefAccountId = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.groupboxdetail = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.PanelDebitaccount = new System.Windows.Forms.Panel();
		this.CmbAccountId = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.lblbalancedetail = new System.Windows.Forms.Label();
		this.PanelSubsidiary = new System.Windows.Forms.Panel();
		this.labelSubsidiaryBal = new System.Windows.Forms.Label();
		this.CmbSubsidiaryAccount = new UltraCombo();
		this.lblSubsidiaryaccount = new System.Windows.Forms.Label();
		this.PanelJobLot = new System.Windows.Forms.Panel();
		this.CmbJobLotId = new UltraCombo();
		this.lblJobLot = new System.Windows.Forms.Label();
		this.PanelComments = new System.Windows.Forms.Panel();
		this.lblRemarks = new System.Windows.Forms.Label();
		this.txtComments = new System.Windows.Forms.TextBox();
		this.panel26 = new System.Windows.Forms.Panel();
		this.cmbTcyCodeDetail = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.panel27 = new System.Windows.Forms.Panel();
		this.txtTcyExchangeRateDetail = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.FcyAmountDetailPanel = new System.Windows.Forms.Panel();
		this.txtFcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.PanelAmount = new System.Windows.Forms.Panel();
		this.txtDebitAmount = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.ReferenceAccountPanel = new System.Windows.Forms.Panel();
		this.CmbReferenceAccount = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.PanelCostCenter = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.labelCostCenter = new System.Windows.Forms.Label();
		this.PanelBranchName = new System.Windows.Forms.Panel();
		this.cmbBranchName = new UltraCombo();
		this.labelbranchname = new System.Windows.Forms.Label();
		this.ChequeDetailPanel = new System.Windows.Forms.Panel();
		this.datChequeDate = new System.Windows.Forms.DateTimePicker();
		this.lblCheqDate = new System.Windows.Forms.Label();
		this.txtPayTitle = new System.Windows.Forms.TextBox();
		this.lblCheqNo = new System.Windows.Forms.Label();
		this.lblPayTitle = new System.Windows.Forms.Label();
		this.CmbChequeNo = new UltraCombo();
		this.PanelAddButton = new System.Windows.Forms.Panel();
		this.Add = new System.Windows.Forms.Button();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.ExpenseVoucherLable = new System.Windows.Forms.Label();
		this.chkformat2 = new System.Windows.Forms.CheckBox();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnPrintFormat2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.Update = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnPrintFormatII = new System.Windows.Forms.ToolStripButton();
		this.BtnCostCenterform = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.History = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.VoucherInfoBox = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchers = new System.Windows.Forms.Label();
		this.lblTotalVouchers = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.lblApprovedVouchers = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdexpapproved = new System.Windows.Forms.RadioButton();
		this.rdexpmodify = new System.Windows.Forms.RadioButton();
		this.rdexpdocdate = new System.Windows.Forms.RadioButton();
		this.rdexpentry = new System.Windows.Forms.RadioButton();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbAccountTitleHistory = new UltraCombo();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.cmbApproveStatus = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.button1 = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.cmbdoctype = new System.Windows.Forms.ComboBox();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.btnCreateUpdateMultiCurrency = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel24.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCostCenterDetailSummary).BeginInit();
		this.panel22.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCostCenter).BeginInit();
		this.panel5.SuspendLayout();
		this.MultiCurrencyPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).BeginInit();
		this.panel25.SuspendLayout();
		this.panel17.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel20.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCostCenterDetail).BeginInit();
		this.panel23.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefAccountId).BeginInit();
		this.groupboxdetail.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.PanelDebitaccount.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountId).BeginInit();
		this.PanelSubsidiary.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).BeginInit();
		this.PanelJobLot.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotId).BeginInit();
		this.PanelComments.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTcyCodeDetail).BeginInit();
		this.panel27.SuspendLayout();
		this.FcyAmountDetailPanel.SuspendLayout();
		this.PanelAmount.SuspendLayout();
		this.ReferenceAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).BeginInit();
		this.PanelCostCenter.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		this.PanelBranchName.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		this.ChequeDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).BeginInit();
		this.PanelAddButton.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.panel3.SuspendLayout();
		this.btnPrintFormat2.SuspendLayout();
		this.History.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.VoucherInfoBox.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).BeginInit();
		this.panel15.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel7.SuspendLayout();
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
			this.btnNew, this.btnEdit, this.btnS, this.btnPrint, this.toolStripSeparator, this.btnDte, this.btnCopy, this.btnPaste, this.toolStripSeparator1, this.toolStripButton1,
			this.toolStripButton2, this.helpToolStripButton
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1830, 27);
		this.toolStrip1.TabIndex = 46;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnEdit.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.Size = new System.Drawing.Size(51, 24);
		this.btnEdit.Text = "&Edit";
		this.btnS.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnS.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnS.Name = "btnS";
		this.btnS.Size = new System.Drawing.Size(55, 24);
		this.btnS.Text = "&Save";
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 24);
		this.btnPrint.Text = "&Print";
		this.btnDte.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDte.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDte.Name = "btnDte";
		this.btnDte.Size = new System.Drawing.Size(64, 24);
		this.btnDte.Text = "&Delete";
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
		this.panel1.Size = new System.Drawing.Size(1426, 663);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1426, 663);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.panel2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(1418, 634);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel24);
		this.panel2.Controls.Add(this.panel17);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1414, 630);
		this.panel2.TabIndex = 0;
		this.panel24.Controls.Add(this.panel21);
		this.panel24.Controls.Add(this.panel6);
		this.panel24.Controls.Add(this.MultiCurrencyPanel);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel24.Location = new System.Drawing.Point(1038, 57);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(286, 573);
		this.panel24.TabIndex = 8;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.grdCostCenterDetailSummary);
		this.panel21.Controls.Add(this.panel22);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 344);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(286, 229);
		this.panel21.TabIndex = 2;
		this.grdCostCenterDetailSummary.AllowEdit = (InheritableBoolean)2;
		grdCostCenterDetailSummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCostCenterDetailSummary.DesignTimeLayout = grdCostCenterDetailSummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetailSummary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdCostCenterDetailSummary.DynamicFiltering = true;
		this.grdCostCenterDetailSummary.FilterMode = (FilterMode)2;
		this.grdCostCenterDetailSummary.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetailSummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCostCenterDetailSummary.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetailSummary).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetailSummary).Name = "grdCostCenterDetailSummary";
		((ScrollableBase)this.grdCostCenterDetailSummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetailSummary).Size = new System.Drawing.Size(286, 208);
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetailSummary).TabIndex = 0;
		this.grdCostCenterDetailSummary.TotalRow = (InheritableBoolean)1;
		this.grdCostCenterDetailSummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCostCenterDetailSummary).VisualStyle = (VisualStyle)4;
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.label13);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Margin = new System.Windows.Forms.Padding(2);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(286, 21);
		this.panel22.TabIndex = 1;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 2);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(161, 17);
		this.label13.TabIndex = 0;
		this.label13.Text = "Cost Center Summary";
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdCostCenter);
		this.panel6.Controls.Add(this.panel5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 161);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(286, 183);
		this.panel6.TabIndex = 7;
		((ScrollableBase)this.grdCostCenter).BackColor = System.Drawing.Color.White;
		grdCostCenter_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCostCenter.DesignTimeLayout = grdCostCenter_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCostCenter).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCostCenter).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCostCenter.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCostCenter).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdCostCenter).Name = "grdCostCenter";
		((ScrollableBase)this.grdCostCenter).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCostCenter).Size = new System.Drawing.Size(286, 161);
		((System.Windows.Forms.Control)(object)this.grdCostCenter).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdCostCenter).TabStop = false;
		this.grdCostCenter.TotalRow = (InheritableBoolean)1;
		this.grdCostCenter.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCostCenter).VisualStyle = (VisualStyle)4;
		this.grdCostCenter.CellUpdated += new ColumnActionEventHandler(grdCostCenter_CellUpdated);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(286, 22);
		this.panel5.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(4, 2);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(171, 17);
		this.label3.TabIndex = 0;
		this.label3.Text = "Apply Multi Cost Center";
		this.MultiCurrencyPanel.Controls.Add((System.Windows.Forms.Control)(object)this.grdMultiCurrency);
		this.MultiCurrencyPanel.Controls.Add(this.panel25);
		this.MultiCurrencyPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.MultiCurrencyPanel.Location = new System.Drawing.Point(0, 0);
		this.MultiCurrencyPanel.Name = "MultiCurrencyPanel";
		this.MultiCurrencyPanel.Size = new System.Drawing.Size(286, 161);
		this.MultiCurrencyPanel.TabIndex = 0;
		((ScrollableBase)this.grdMultiCurrency).BackColor = System.Drawing.Color.White;
		grdMultiCurrency_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMultiCurrency.DesignTimeLayout = grdMultiCurrency_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMultiCurrency.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Name = "grdMultiCurrency";
		((ScrollableBase)this.grdMultiCurrency).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Size = new System.Drawing.Size(286, 139);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabIndex = 60;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabStop = false;
		this.grdMultiCurrency.TotalRow = (InheritableBoolean)1;
		this.grdMultiCurrency.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMultiCurrency).VisualStyle = (VisualStyle)4;
		this.grdMultiCurrency.SelectionChanged += new System.EventHandler(grdMultiCurrency_SelectionChanged);
		this.panel25.Controls.Add(this.label9);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(286, 22);
		this.panel25.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.Teal;
		this.label9.Location = new System.Drawing.Point(2, 1);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(130, 17);
		this.label9.TabIndex = 1;
		this.label9.Text = "Multi GL Currency";
		this.panel17.Controls.Add(this.tabControl2);
		this.panel17.Controls.Add(this.panel23);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(0, 57);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1038, 573);
		this.panel17.TabIndex = 1;
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 214);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1038, 359);
		this.tabControl2.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel19);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1030, 330);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(3, 3);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1024, 324);
		this.panel19.TabIndex = 1;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1024, 303);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.EditingCell += new EditingCellEventHandler(grd_EditingCell);
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar1);
		this.panel20.Controls.Add(this.label12);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Margin = new System.Windows.Forms.Padding(2);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1024, 21);
		this.panel20.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(980, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 21);
		this.ctrlGrdBar1.TabIndex = 19;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(1, 2);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(121, 17);
		this.label12.TabIndex = 0;
		this.label12.Text = "VOUCHER Detail";
		this.tabPage2.Controls.Add(this.panel14);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1030, 330);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Cost Center Breakup";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add(this.panel18);
		this.panel14.Controls.Add(this.label8);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(3, 3);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1024, 324);
		this.panel14.TabIndex = 46;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdCostCenterDetail);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 22);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1024, 302);
		this.panel18.TabIndex = 46;
		this.grdCostCenterDetail.AllowEdit = (InheritableBoolean)2;
		grdCostCenterDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCostCenterDetail.DesignTimeLayout = grdCostCenterDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdCostCenterDetail.DynamicFiltering = true;
		this.grdCostCenterDetail.FilterMode = (FilterMode)2;
		this.grdCostCenterDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCostCenterDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetail).Name = "grdCostCenterDetail";
		((ScrollableBase)this.grdCostCenterDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetail).Size = new System.Drawing.Size(1024, 302);
		((System.Windows.Forms.Control)(object)this.grdCostCenterDetail).TabIndex = 45;
		this.grdCostCenterDetail.TotalRow = (InheritableBoolean)1;
		this.grdCostCenterDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCostCenterDetail).VisualStyle = (VisualStyle)4;
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Dock = System.Windows.Forms.DockStyle.Top;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(0, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(1024, 22);
		this.label8.TabIndex = 13;
		this.label8.Text = "Cost Center BreakUp";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel23.Controls.Add(this.groupBox2);
		this.panel23.Controls.Add(this.groupboxdetail);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1038, 214);
		this.panel23.TabIndex = 0;
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.lblCrAcGlCurrency);
		this.groupBox2.Controls.Add(this.txtBaseFcyCode);
		this.groupBox2.Controls.Add(this.lblBaseFcyCode);
		this.groupBox2.Controls.Add(this.txtBaseFcyExchangeRate);
		this.groupBox2.Controls.Add(this.lblBaseFcyExchangeRate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProjectId);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.lblExchangeRate);
		this.groupBox2.Controls.Add(this.lblFcyAmount);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox2.Controls.Add(this.lblFcyCode);
		this.groupBox2.Controls.Add(this.txtVoucherCode);
		this.groupBox2.Controls.Add(this.txtbalance);
		this.groupBox2.Controls.Add(this.txtRemarks);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.datVoucherDate);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefAccountId);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1027, 93);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.lblCrAcGlCurrency.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblCrAcGlCurrency.Location = new System.Drawing.Point(909, 64);
		this.lblCrAcGlCurrency.Name = "lblCrAcGlCurrency";
		this.lblCrAcGlCurrency.Size = new System.Drawing.Size(95, 27);
		this.lblCrAcGlCurrency.TabIndex = 575;
		this.lblCrAcGlCurrency.Text = "Credit Ac GLcy:";
		this.txtBaseFcyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyCode.Enabled = false;
		this.txtBaseFcyCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyCode.Location = new System.Drawing.Point(910, 15);
		this.txtBaseFcyCode.Name = "txtBaseFcyCode";
		this.txtBaseFcyCode.ShortcutsEnabled = false;
		this.txtBaseFcyCode.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyCode.TabIndex = 573;
		this.txtBaseFcyCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyCode.AutoSize = true;
		this.lblBaseFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyCode.Location = new System.Drawing.Point(833, 19);
		this.lblBaseFcyCode.Name = "lblBaseFcyCode";
		this.lblBaseFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblBaseFcyCode.TabIndex = 574;
		this.lblBaseFcyCode.Text = "Fcy Code";
		this.txtBaseFcyExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyExchangeRate.Location = new System.Drawing.Point(910, 41);
		this.txtBaseFcyExchangeRate.Name = "txtBaseFcyExchangeRate";
		this.txtBaseFcyExchangeRate.ShortcutsEnabled = false;
		this.txtBaseFcyExchangeRate.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyExchangeRate.TabIndex = 571;
		this.txtBaseFcyExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyExchangeRate.AutoSize = true;
		this.lblBaseFcyExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyExchangeRate.Location = new System.Drawing.Point(833, 37);
		this.lblBaseFcyExchangeRate.Name = "lblBaseFcyExchangeRate";
		this.lblBaseFcyExchangeRate.Size = new System.Drawing.Size(82, 30);
		this.lblBaseFcyExchangeRate.TabIndex = 572;
		this.lblBaseFcyExchangeRate.Text = "Fcy Exchange \r\nRate";
		this.CmbProjectId.AllowNull = (DefaultableBoolean)1;
		this.CmbProjectId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProjectId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProjectId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProjectId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Location = new System.Drawing.Point(100, 13);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Name = "CmbProjectId";
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(44, 15);
		this.label5.TabIndex = 566;
		this.label5.Text = "Project";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(705, 41);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(124, 23);
		this.txtExchangeRate.TabIndex = 7;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.lblExchangeRate.AutoSize = true;
		this.lblExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblExchangeRate.Location = new System.Drawing.Point(623, 37);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(82, 30);
		this.lblExchangeRate.TabIndex = 42;
		this.lblExchangeRate.Text = "Tcy Exchange \r\nRate";
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFcyAmount.Location = new System.Drawing.Point(623, 69);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(72, 15);
		this.lblFcyAmount.TabIndex = 43;
		this.lblFcyAmount.Text = "Tcy Amount";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(705, 65);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(124, 23);
		this.txtFcyAmount.TabIndex = 8;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(705, 13);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(623, 19);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 39;
		this.lblFcyCode.Text = "Tcy Code";
		this.txtVoucherCode.BackColor = System.Drawing.Color.White;
		this.txtVoucherCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoucherCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVoucherCode.Location = new System.Drawing.Point(100, 41);
		this.txtVoucherCode.Name = "txtVoucherCode";
		this.txtVoucherCode.ReadOnly = true;
		this.txtVoucherCode.Size = new System.Drawing.Size(72, 23);
		this.txtVoucherCode.TabIndex = 1;
		this.txtVoucherCode.TabStop = false;
		this.txtVoucherCode.Leave += new System.EventHandler(txtvoucherno_TextChanged);
		this.txtbalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbalance.Enabled = false;
		this.txtbalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbalance.Location = new System.Drawing.Point(388, 41);
		this.txtbalance.Name = "txtbalance";
		this.txtbalance.Size = new System.Drawing.Size(231, 23);
		this.txtbalance.TabIndex = 5;
		this.txtbalance.Text = "0";
		this.txtbalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(100, 65);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(519, 23);
		this.txtRemarks.TabIndex = 3;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 45);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(98, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Voucher No,Date";
		this.datVoucherDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datVoucherDate.CustomFormat = "dd-MMM-yy";
		this.datVoucherDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datVoucherDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datVoucherDate.Location = new System.Drawing.Point(173, 41);
		this.datVoucherDate.Name = "datVoucherDate";
		this.datVoucherDate.Size = new System.Drawing.Size(123, 22);
		this.datVoucherDate.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 68);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(60, 17);
		this.label1.TabIndex = 6;
		this.label1.Text = "Remarks";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbRefAccountId.AllowNull = (DefaultableBoolean)1;
		this.CmbRefAccountId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefAccountId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefAccountId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefAccountId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Location = new System.Drawing.Point(388, 13);
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Name = "CmbRefAccountId";
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).TabIndex = 4;
		this.CmbRefAccountId.ValueChanged += new System.EventHandler(CmbAccountId_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Leave += new System.EventHandler(combcreditac_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(302, 45);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 4;
		this.label4.Text = "Balance";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(302, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "Credit Account";
		this.groupboxdetail.BackColor = System.Drawing.Color.White;
		this.groupboxdetail.Controls.Add(this.flowLayoutPanel1);
		this.groupboxdetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupboxdetail.Location = new System.Drawing.Point(3, 99);
		this.groupboxdetail.Name = "groupboxdetail";
		this.groupboxdetail.Size = new System.Drawing.Size(1027, 114);
		this.groupboxdetail.TabIndex = 1;
		this.groupboxdetail.TabStop = false;
		this.groupboxdetail.Text = "Detail";
		this.flowLayoutPanel1.Controls.Add(this.PanelDebitaccount);
		this.flowLayoutPanel1.Controls.Add(this.PanelSubsidiary);
		this.flowLayoutPanel1.Controls.Add(this.PanelJobLot);
		this.flowLayoutPanel1.Controls.Add(this.PanelComments);
		this.flowLayoutPanel1.Controls.Add(this.panel26);
		this.flowLayoutPanel1.Controls.Add(this.panel27);
		this.flowLayoutPanel1.Controls.Add(this.FcyAmountDetailPanel);
		this.flowLayoutPanel1.Controls.Add(this.PanelAmount);
		this.flowLayoutPanel1.Controls.Add(this.ReferenceAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.PanelCostCenter);
		this.flowLayoutPanel1.Controls.Add(this.PanelBranchName);
		this.flowLayoutPanel1.Controls.Add(this.ChequeDetailPanel);
		this.flowLayoutPanel1.Controls.Add(this.PanelAddButton);
		this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 15);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(1019, 94);
		this.flowLayoutPanel1.TabIndex = 603;
		this.PanelDebitaccount.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountId);
		this.PanelDebitaccount.Controls.Add(this.label11);
		this.PanelDebitaccount.Controls.Add(this.lblbalancedetail);
		this.PanelDebitaccount.Location = new System.Drawing.Point(0, 0);
		this.PanelDebitaccount.Margin = new System.Windows.Forms.Padding(0);
		this.PanelDebitaccount.Name = "PanelDebitaccount";
		this.PanelDebitaccount.Size = new System.Drawing.Size(210, 47);
		this.PanelDebitaccount.TabIndex = 0;
		this.CmbAccountId.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountId).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountId).Location = new System.Drawing.Point(1, 20);
		((System.Windows.Forms.Control)(object)this.CmbAccountId).Name = "CmbAccountId";
		((System.Windows.Forms.Control)(object)this.CmbAccountId).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountId).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbAccountId).Leave += new System.EventHandler(combactitle_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1, 3);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(84, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Debit Account";
		this.lblbalancedetail.AutoSize = true;
		this.lblbalancedetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblbalancedetail.ForeColor = System.Drawing.Color.Red;
		this.lblbalancedetail.Location = new System.Drawing.Point(95, 3);
		this.lblbalancedetail.Name = "lblbalancedetail";
		this.lblbalancedetail.Size = new System.Drawing.Size(48, 14);
		this.lblbalancedetail.TabIndex = 38;
		this.lblbalancedetail.Text = "label8";
		this.PanelSubsidiary.Controls.Add(this.labelSubsidiaryBal);
		this.PanelSubsidiary.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount);
		this.PanelSubsidiary.Controls.Add(this.lblSubsidiaryaccount);
		this.PanelSubsidiary.Location = new System.Drawing.Point(210, 0);
		this.PanelSubsidiary.Margin = new System.Windows.Forms.Padding(0);
		this.PanelSubsidiary.Name = "PanelSubsidiary";
		this.PanelSubsidiary.Size = new System.Drawing.Size(204, 47);
		this.PanelSubsidiary.TabIndex = 1;
		this.labelSubsidiaryBal.AutoSize = true;
		this.labelSubsidiaryBal.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelSubsidiaryBal.Location = new System.Drawing.Point(108, 3);
		this.labelSubsidiaryBal.Name = "labelSubsidiaryBal";
		this.labelSubsidiaryBal.Size = new System.Drawing.Size(48, 14);
		this.labelSubsidiaryBal.TabIndex = 596;
		this.labelSubsidiaryBal.Text = "label8";
		this.labelSubsidiaryBal.Visible = false;
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
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Location = new System.Drawing.Point(1, 20);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Name = "CmbSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).TabIndex = 0;
		this.CmbSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbSubsidiaryAccount_AfterCloseUp);
		this.CmbSubsidiaryAccount.ValueChanged += new System.EventHandler(CmbSubsidiaryAccount_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Leave += new System.EventHandler(CmbSubsidiaryAccount_Leave);
		this.lblSubsidiaryaccount.AutoSize = true;
		this.lblSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryaccount.Location = new System.Drawing.Point(1, 3);
		this.lblSubsidiaryaccount.Name = "lblSubsidiaryaccount";
		this.lblSubsidiaryaccount.Size = new System.Drawing.Size(110, 15);
		this.lblSubsidiaryaccount.TabIndex = 42;
		this.lblSubsidiaryaccount.Text = "Subsidiary Account";
		this.PanelJobLot.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotId);
		this.PanelJobLot.Controls.Add(this.lblJobLot);
		this.PanelJobLot.Location = new System.Drawing.Point(414, 0);
		this.PanelJobLot.Margin = new System.Windows.Forms.Padding(0);
		this.PanelJobLot.Name = "PanelJobLot";
		this.PanelJobLot.Size = new System.Drawing.Size(128, 47);
		this.PanelJobLot.TabIndex = 2;
		this.CmbJobLotId.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotId).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotId).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.CmbJobLotId).Name = "CmbJobLotId";
		((System.Windows.Forms.Control)(object)this.CmbJobLotId).Size = new System.Drawing.Size(128, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotId).TabIndex = 0;
		this.lblJobLot.AutoSize = true;
		this.lblJobLot.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJobLot.Location = new System.Drawing.Point(1, 3);
		this.lblJobLot.Name = "lblJobLot";
		this.lblJobLot.Size = new System.Drawing.Size(48, 15);
		this.lblJobLot.TabIndex = 12;
		this.lblJobLot.Text = "Job/Lot";
		this.PanelComments.Controls.Add(this.lblRemarks);
		this.PanelComments.Controls.Add(this.txtComments);
		this.PanelComments.Location = new System.Drawing.Point(542, 0);
		this.PanelComments.Margin = new System.Windows.Forms.Padding(0);
		this.PanelComments.Name = "PanelComments";
		this.PanelComments.Size = new System.Drawing.Size(180, 47);
		this.PanelComments.TabIndex = 3;
		this.lblRemarks.AutoSize = true;
		this.lblRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRemarks.Location = new System.Drawing.Point(1, 5);
		this.lblRemarks.Name = "lblRemarks";
		this.lblRemarks.Size = new System.Drawing.Size(52, 15);
		this.lblRemarks.TabIndex = 2;
		this.lblRemarks.Text = "Remarks";
		this.txtComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtComments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtComments.Location = new System.Drawing.Point(1, 22);
		this.txtComments.Name = "txtComments";
		this.txtComments.Size = new System.Drawing.Size(179, 23);
		this.txtComments.TabIndex = 0;
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail);
		this.panel26.Controls.Add(this.label54);
		this.panel26.Location = new System.Drawing.Point(722, 0);
		this.panel26.Margin = new System.Windows.Forms.Padding(0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(89, 47);
		this.panel26.TabIndex = 4;
		this.cmbTcyCodeDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbTcyCodeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTcyCodeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTcyCodeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTcyCodeDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Name = "cmbTcyCodeDetail";
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Size = new System.Drawing.Size(86, 26);
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).TabIndex = 3;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(0, 2);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(55, 15);
		this.label54.TabIndex = 12;
		this.label54.Text = "Tcy Code";
		this.panel27.Controls.Add(this.txtTcyExchangeRateDetail);
		this.panel27.Controls.Add(this.label53);
		this.panel27.Location = new System.Drawing.Point(811, 0);
		this.panel27.Margin = new System.Windows.Forms.Padding(0);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(105, 47);
		this.panel27.TabIndex = 5;
		this.txtTcyExchangeRateDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTcyExchangeRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTcyExchangeRateDetail.Location = new System.Drawing.Point(2, 22);
		this.txtTcyExchangeRateDetail.Name = "txtTcyExchangeRateDetail";
		this.txtTcyExchangeRateDetail.ShortcutsEnabled = false;
		this.txtTcyExchangeRateDetail.Size = new System.Drawing.Size(101, 23);
		this.txtTcyExchangeRateDetail.TabIndex = 5;
		this.txtTcyExchangeRateDetail.Tag = "FcyAmount";
		this.txtTcyExchangeRateDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTcyExchangeRateDetail.TextChanged += new System.EventHandler(txtTcyExchangeRateDetail_TextChanged);
		this.txtTcyExchangeRateDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtTcyExchangeRateDetail.Leave += new System.EventHandler(txtTcyExchangeRateDetail_Leave);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(-1, 3);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(105, 15);
		this.label53.TabIndex = 4;
		this.label53.Text = "Tcy Exchange Rate";
		this.FcyAmountDetailPanel.Controls.Add(this.txtFcyAmountDetail);
		this.FcyAmountDetailPanel.Controls.Add(this.label6);
		this.FcyAmountDetailPanel.Location = new System.Drawing.Point(916, 0);
		this.FcyAmountDetailPanel.Margin = new System.Windows.Forms.Padding(0);
		this.FcyAmountDetailPanel.Name = "FcyAmountDetailPanel";
		this.FcyAmountDetailPanel.Size = new System.Drawing.Size(96, 45);
		this.FcyAmountDetailPanel.TabIndex = 6;
		this.txtFcyAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountDetail.Location = new System.Drawing.Point(1, 22);
		this.txtFcyAmountDetail.Name = "txtFcyAmountDetail";
		this.txtFcyAmountDetail.ShortcutsEnabled = false;
		this.txtFcyAmountDetail.Size = new System.Drawing.Size(95, 23);
		this.txtFcyAmountDetail.TabIndex = 0;
		this.txtFcyAmountDetail.Tag = "FcyAmount";
		this.txtFcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountDetail.TextChanged += new System.EventHandler(txtFcyAmountDetail_TextChanged);
		this.txtFcyAmountDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDebitAmount_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(1, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(72, 15);
		this.label6.TabIndex = 4;
		this.label6.Text = "Tcy Amount";
		this.PanelAmount.Controls.Add(this.txtDebitAmount);
		this.PanelAmount.Controls.Add(this.label20);
		this.PanelAmount.Location = new System.Drawing.Point(0, 47);
		this.PanelAmount.Margin = new System.Windows.Forms.Padding(0);
		this.PanelAmount.Name = "PanelAmount";
		this.PanelAmount.Size = new System.Drawing.Size(110, 47);
		this.PanelAmount.TabIndex = 7;
		this.txtDebitAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDebitAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDebitAmount.Location = new System.Drawing.Point(1, 22);
		this.txtDebitAmount.Name = "txtDebitAmount";
		this.txtDebitAmount.ShortcutsEnabled = false;
		this.txtDebitAmount.Size = new System.Drawing.Size(108, 23);
		this.txtDebitAmount.TabIndex = 0;
		this.txtDebitAmount.Tag = "DebitAmount";
		this.txtDebitAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDebitAmount.TextChanged += new System.EventHandler(txtDebitAmount_TextChanged);
		this.txtDebitAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDebitAmount_KeyPress);
		this.txtDebitAmount.Leave += new System.EventHandler(txtDebitAmount_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(3, 4);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(83, 15);
		this.label20.TabIndex = 4;
		this.label20.Text = "Debit Amount";
		this.ReferenceAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceAccount);
		this.ReferenceAccountPanel.Controls.Add(this.label46);
		this.ReferenceAccountPanel.Location = new System.Drawing.Point(110, 47);
		this.ReferenceAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ReferenceAccountPanel.Name = "ReferenceAccountPanel";
		this.ReferenceAccountPanel.Size = new System.Drawing.Size(155, 47);
		this.ReferenceAccountPanel.TabIndex = 8;
		this.CmbReferenceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbReferenceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Location = new System.Drawing.Point(1, 19);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Name = "CmbReferenceAccount";
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).TabIndex = 2;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(2, 2);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(107, 15);
		this.label46.TabIndex = 38;
		this.label46.Text = "Reference Account";
		this.PanelCostCenter.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.PanelCostCenter.Controls.Add(this.labelCostCenter);
		this.PanelCostCenter.Location = new System.Drawing.Point(265, 47);
		this.PanelCostCenter.Margin = new System.Windows.Forms.Padding(0);
		this.PanelCostCenter.Name = "PanelCostCenter";
		this.PanelCostCenter.Size = new System.Drawing.Size(125, 45);
		this.PanelCostCenter.TabIndex = 9;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(1, 18);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Leave += new System.EventHandler(combactitle_Leave);
		this.labelCostCenter.AutoSize = true;
		this.labelCostCenter.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelCostCenter.Location = new System.Drawing.Point(1, 2);
		this.labelCostCenter.Name = "labelCostCenter";
		this.labelCostCenter.Size = new System.Drawing.Size(67, 15);
		this.labelCostCenter.TabIndex = 4;
		this.labelCostCenter.Text = "Cost Center";
		this.PanelBranchName.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.PanelBranchName.Controls.Add(this.labelbranchname);
		this.PanelBranchName.Location = new System.Drawing.Point(390, 47);
		this.PanelBranchName.Margin = new System.Windows.Forms.Padding(0);
		this.PanelBranchName.Name = "PanelBranchName";
		this.PanelBranchName.Size = new System.Drawing.Size(150, 45);
		this.PanelBranchName.TabIndex = 10;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(1, 18);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		this.labelbranchname.AutoSize = true;
		this.labelbranchname.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelbranchname.Location = new System.Drawing.Point(2, 1);
		this.labelbranchname.Name = "labelbranchname";
		this.labelbranchname.Size = new System.Drawing.Size(79, 15);
		this.labelbranchname.TabIndex = 594;
		this.labelbranchname.Text = "Branch Name";
		this.ChequeDetailPanel.Controls.Add(this.datChequeDate);
		this.ChequeDetailPanel.Controls.Add(this.lblCheqDate);
		this.ChequeDetailPanel.Controls.Add(this.txtPayTitle);
		this.ChequeDetailPanel.Controls.Add(this.lblCheqNo);
		this.ChequeDetailPanel.Controls.Add(this.lblPayTitle);
		this.ChequeDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeNo);
		this.ChequeDetailPanel.Location = new System.Drawing.Point(540, 47);
		this.ChequeDetailPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ChequeDetailPanel.Name = "ChequeDetailPanel";
		this.ChequeDetailPanel.Size = new System.Drawing.Size(405, 45);
		this.ChequeDetailPanel.TabIndex = 11;
		this.datChequeDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datChequeDate.CustomFormat = "dd-MMM-yy";
		this.datChequeDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datChequeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datChequeDate.Location = new System.Drawing.Point(1, 19);
		this.datChequeDate.Name = "datChequeDate";
		this.datChequeDate.Size = new System.Drawing.Size(107, 22);
		this.datChequeDate.TabIndex = 0;
		this.lblCheqDate.AutoSize = true;
		this.lblCheqDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCheqDate.Location = new System.Drawing.Point(1, 2);
		this.lblCheqDate.Name = "lblCheqDate";
		this.lblCheqDate.Size = new System.Drawing.Size(75, 15);
		this.lblCheqDate.TabIndex = 33;
		this.lblCheqDate.Text = "Cheque Date";
		this.txtPayTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPayTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPayTitle.Location = new System.Drawing.Point(223, 18);
		this.txtPayTitle.Name = "txtPayTitle";
		this.txtPayTitle.Size = new System.Drawing.Size(182, 23);
		this.txtPayTitle.TabIndex = 2;
		this.lblCheqNo.AutoSize = true;
		this.lblCheqNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCheqNo.Location = new System.Drawing.Point(108, 2);
		this.lblCheqNo.Name = "lblCheqNo";
		this.lblCheqNo.Size = new System.Drawing.Size(66, 15);
		this.lblCheqNo.TabIndex = 0;
		this.lblCheqNo.Text = "Cheque No";
		this.lblPayTitle.AutoSize = true;
		this.lblPayTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPayTitle.Location = new System.Drawing.Point(220, 1);
		this.lblPayTitle.Name = "lblPayTitle";
		this.lblPayTitle.Size = new System.Drawing.Size(49, 15);
		this.lblPayTitle.TabIndex = 35;
		this.lblPayTitle.Text = "PayTitle";
		this.CmbChequeNo.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Location = new System.Drawing.Point(109, 17);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Name = "CmbChequeNo";
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Size = new System.Drawing.Size(113, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).TabIndex = 1;
		this.PanelAddButton.Controls.Add(this.Add);
		this.PanelAddButton.Controls.Add(this.btnCancelUpdateDetial);
		this.PanelAddButton.Controls.Add(this.btnUpdateDetail);
		this.PanelAddButton.Location = new System.Drawing.Point(945, 47);
		this.PanelAddButton.Margin = new System.Windows.Forms.Padding(0);
		this.PanelAddButton.Name = "PanelAddButton";
		this.PanelAddButton.Size = new System.Drawing.Size(67, 47);
		this.PanelAddButton.TabIndex = 12;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(1, 22);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(36, 23);
		this.Add.TabIndex = 0;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click_1);
		this.Add.Leave += new System.EventHandler(Add_Leave);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1, 0);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(61, 22);
		this.btnCancelUpdateDetial.TabIndex = 2;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnCancelUpdateDetial.Leave += new System.EventHandler(btnCancelUpdateDetial_Leave);
		this.btnCancelUpdateDetial.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(btnCancelUpdateDetial_PreviewKeyDown);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1, 23);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(61, 21);
		this.btnUpdateDetail.TabIndex = 1;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.chkCustomAccounts);
		this.panel4.Controls.Add(this.labelLocationType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel4.Controls.Add(this.ExpenseVoucherLable);
		this.panel4.Controls.Add(this.chkformat2);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1414, 27);
		this.panel4.TabIndex = 0;
		this.chkCustomAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(994, 6);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(137, 17);
		this.chkCustomAccounts.TabIndex = 509;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(318, 6);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 508;
		this.labelLocationType.Text = "Location Type";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(406, 0);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 507;
		this.ExpenseVoucherLable.AutoSize = true;
		this.ExpenseVoucherLable.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpenseVoucherLable.ForeColor = System.Drawing.Color.White;
		this.ExpenseVoucherLable.Location = new System.Drawing.Point(3, 4);
		this.ExpenseVoucherLable.Name = "ExpenseVoucherLable";
		this.ExpenseVoucherLable.Size = new System.Drawing.Size(168, 19);
		this.ExpenseVoucherLable.TabIndex = 0;
		this.ExpenseVoucherLable.Text = "EXPENSE VOUCHER";
		this.chkformat2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkformat2.AutoSize = true;
		this.chkformat2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkformat2.ForeColor = System.Drawing.Color.White;
		this.chkformat2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.chkformat2.Location = new System.Drawing.Point(1255, 4);
		this.chkformat2.Margin = new System.Windows.Forms.Padding(2);
		this.chkformat2.Name = "chkformat2";
		this.chkformat2.Size = new System.Drawing.Size(157, 20);
		this.chkformat2.TabIndex = 12;
		this.chkformat2.Text = "Preview Format II";
		this.chkformat2.UseVisualStyleBackColor = true;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBox.Location = new System.Drawing.Point(1132, 4);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 11;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.btnPrintFormat2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1414, 30);
		this.panel3.TabIndex = 0;
		this.btnPrintFormat2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintFormat2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btnPrintFormat2.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.New, this.btnRefresh, this.Update, this.Save, this.toolStripButton10, this.btnDelete, this.Print, this.btnPrintFormatII, this.BtnCostCenterform, this.btnShortCutKey,
			this.btnCreateUpdateMultiCurrency
		});
		this.btnPrintFormat2.Location = new System.Drawing.Point(0, 0);
		this.btnPrintFormat2.Name = "btnPrintFormat2";
		this.btnPrintFormat2.Size = new System.Drawing.Size(1414, 27);
		this.btnPrintFormat2.TabIndex = 0;
		this.btnPrintFormat2.Text = "toolStrip2";
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
		this.Print.Text = "102-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnPrintFormatII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintFormatII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintFormatII.Name = "btnPrintFormatII";
		this.btnPrintFormatII.Size = new System.Drawing.Size(107, 24);
		this.btnPrintFormatII.Text = "102-Print II";
		this.btnPrintFormatII.Click += new System.EventHandler(btnPrintFormatII_Click);
		this.BtnCostCenterform.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnCostCenterform.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnCostCenterform.Name = "BtnCostCenterform";
		this.BtnCostCenterform.Size = new System.Drawing.Size(110, 24);
		this.BtnCostCenterform.Text = "Cost Center";
		this.BtnCostCenterform.Click += new System.EventHandler(BtnCostCenterform_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.History.Controls.Add(this.panel8);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Margin = new System.Windows.Forms.Padding(2);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(2);
		this.History.Size = new System.Drawing.Size(1418, 634);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1414, 630);
		this.panel8.TabIndex = 0;
		this.panel11.Controls.Add(this.panel10);
		this.panel11.Controls.Add(this.panel7);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1414, 630);
		this.panel11.TabIndex = 2;
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.panel9);
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel16);
		this.panel10.Controls.Add(this.panel15);
		this.panel10.Controls.Add(this.toolStrip3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1414, 630);
		this.panel10.TabIndex = 45;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 403);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1414, 227);
		this.panel13.TabIndex = 56;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1414, 227);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label28);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 378);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1414, 25);
		this.panel9.TabIndex = 55;
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.Teal;
		this.label28.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(4, 3);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(201, 17);
		this.label28.TabIndex = 55;
		this.label28.Text = "Detail of above selected row";
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 130);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1414, 248);
		this.panel12.TabIndex = 54;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1414, 248);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 44;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel16.Controls.Add(this.VoucherInfoBox);
		this.panel16.Controls.Add(this.groupBox3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 53);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1414, 77);
		this.panel16.TabIndex = 53;
		this.VoucherInfoBox.Controls.Add(this.label36);
		this.VoucherInfoBox.Controls.Add(this.lblUnApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.lblTotalVouchers);
		this.VoucherInfoBox.Controls.Add(this.label38);
		this.VoucherInfoBox.Controls.Add(this.lblApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.label37);
		this.VoucherInfoBox.Location = new System.Drawing.Point(922, 3);
		this.VoucherInfoBox.Name = "VoucherInfoBox";
		this.VoucherInfoBox.Size = new System.Drawing.Size(242, 72);
		this.VoucherInfoBox.TabIndex = 8;
		this.VoucherInfoBox.TabStop = false;
		this.VoucherInfoBox.Visible = false;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(6, 11);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(96, 17);
		this.label36.TabIndex = 1;
		this.label36.Text = "Total Vouchers";
		this.lblUnApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUnApprovedVouchers.Location = new System.Drawing.Point(153, 47);
		this.lblUnApprovedVouchers.Name = "lblUnApprovedVouchers";
		this.lblUnApprovedVouchers.Size = new System.Drawing.Size(80, 17);
		this.lblUnApprovedVouchers.TabIndex = 6;
		this.lblUnApprovedVouchers.Text = "0";
		this.lblUnApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalVouchers.Location = new System.Drawing.Point(153, 11);
		this.lblTotalVouchers.Name = "lblTotalVouchers";
		this.lblTotalVouchers.Size = new System.Drawing.Size(80, 17);
		this.lblTotalVouchers.TabIndex = 4;
		this.lblTotalVouchers.Text = "0";
		this.lblTotalVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(6, 47);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(144, 17);
		this.label38.TabIndex = 3;
		this.label38.Text = "UnApproved Vouchers";
		this.lblApprovedVouchers.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedVouchers.Location = new System.Drawing.Point(153, 28);
		this.lblApprovedVouchers.Name = "lblApprovedVouchers";
		this.lblApprovedVouchers.Size = new System.Drawing.Size(80, 17);
		this.lblApprovedVouchers.TabIndex = 5;
		this.lblApprovedVouchers.Text = "0";
		this.lblApprovedVouchers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(6, 28);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(127, 17);
		this.label37.TabIndex = 2;
		this.label37.Text = "Approved Vouchers";
		this.groupBox3.Controls.Add(this.rdexpapproved);
		this.groupBox3.Controls.Add(this.rdexpmodify);
		this.groupBox3.Controls.Add(this.rdexpdocdate);
		this.groupBox3.Controls.Add(this.rdexpentry);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.label23);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatus);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(917, 72);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdexpapproved.AutoSize = true;
		this.rdexpapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdexpapproved.Location = new System.Drawing.Point(813, 36);
		this.rdexpapproved.Name = "rdexpapproved";
		this.rdexpapproved.Size = new System.Drawing.Size(105, 19);
		this.rdexpapproved.TabIndex = 9;
		this.rdexpapproved.Text = "Approved Date";
		this.rdexpapproved.UseVisualStyleBackColor = true;
		this.rdexpmodify.AutoSize = true;
		this.rdexpmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdexpmodify.Location = new System.Drawing.Point(813, 16);
		this.rdexpmodify.Name = "rdexpmodify";
		this.rdexpmodify.Size = new System.Drawing.Size(91, 19);
		this.rdexpmodify.TabIndex = 8;
		this.rdexpmodify.Text = "Modify Date";
		this.rdexpmodify.UseVisualStyleBackColor = true;
		this.rdexpdocdate.AutoSize = true;
		this.rdexpdocdate.Checked = true;
		this.rdexpdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdexpdocdate.Location = new System.Drawing.Point(732, 16);
		this.rdexpdocdate.Name = "rdexpdocdate";
		this.rdexpdocdate.Size = new System.Drawing.Size(75, 19);
		this.rdexpdocdate.TabIndex = 6;
		this.rdexpdocdate.TabStop = true;
		this.rdexpdocdate.Text = "Doc Date";
		this.rdexpdocdate.UseVisualStyleBackColor = true;
		this.rdexpentry.AutoSize = true;
		this.rdexpentry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdexpentry.Location = new System.Drawing.Point(732, 36);
		this.rdexpentry.Name = "rdexpentry";
		this.rdexpentry.Size = new System.Drawing.Size(80, 19);
		this.rdexpentry.TabIndex = 7;
		this.rdexpentry.Text = "Entry Date";
		this.rdexpentry.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(397, 19);
		this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(78, 15);
		this.label24.TabIndex = 147;
		this.label24.Text = "Account Title";
		this.CmbAccountTitleHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitleHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitleHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitleHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitleHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Location = new System.Drawing.Point(493, 13);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Name = "CmbAccountTitleHistory";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).TabIndex = 4;
		this.btnShowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowHistory.FlatAppearance.BorderSize = 0;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowHistory.Location = new System.Drawing.Point(656, 41);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(71, 26);
		this.btnShowHistory.TabIndex = 10;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.btnShowHistory.Leave += new System.EventHandler(btnShowHistory_Leave);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(397, 47);
		this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(95, 15);
		this.label10.TabIndex = 142;
		this.label10.Text = "Approved Status";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(213, 47);
		this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(64, 15);
		this.label14.TabIndex = 141;
		this.label14.Text = "To Doc No";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(213, 19);
		this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(79, 15);
		this.label23.TabIndex = 140;
		this.label23.Text = "From Doc No";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(4, 47);
		this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(48, 15);
		this.label25.TabIndex = 139;
		this.label25.Text = "To Date";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(4, 19);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(63, 15);
		this.label26.TabIndex = 13;
		this.label26.Text = "From Date";
		this.cmbApproveStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApproveStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Location = new System.Drawing.Point(493, 41);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Name = "cmbApproveStatus";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Size = new System.Drawing.Size(158, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).TabIndex = 5;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(294, 43);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(294, 15);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(70, 43);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(140, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(70, 15);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(140, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label7);
		this.panel15.Controls.Add(this.ctrlGrdBar3);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 27);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1414, 26);
		this.panel15.TabIndex = 52;
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(5, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(211, 19);
		this.label7.TabIndex = 22;
		this.label7.Text = "Expense Voucher History";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1373, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1414, 27);
		this.toolStrip3.TabIndex = 51;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.btnLoadAll.Image = (System.Drawing.Image)resources.GetObject("btnLoadAll.Image");
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(84, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
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
		this.button1.Location = new System.Drawing.Point(736, 8);
		this.button1.Margin = new System.Windows.Forms.Padding(2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(47, 23);
		this.button1.TabIndex = 113;
		this.button1.TabStop = false;
		this.button1.Text = "Show";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Visible = false;
		this.button1.Click += new System.EventHandler(button1_Click);
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
		this.btnCreateUpdateMultiCurrency.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCreateUpdateMultiCurrency.Image = (System.Drawing.Image)resources.GetObject("btnCreateUpdateMultiCurrency.Image");
		this.btnCreateUpdateMultiCurrency.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCreateUpdateMultiCurrency.Name = "btnCreateUpdateMultiCurrency";
		this.btnCreateUpdateMultiCurrency.Size = new System.Drawing.Size(127, 24);
		this.btnCreateUpdateMultiCurrency.Text = "MultiCurrencyInfo";
		this.btnCreateUpdateMultiCurrency.Click += new System.EventHandler(btnCreateUpdateMultiCurrency_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1426, 663);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ExpenseVoucherNew";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExpenseVoucher_FormClosing);
		base.Load += new System.EventHandler(AcfrmPaymentVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExpenseVoucher_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCostCenterDetailSummary).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCostCenter).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.MultiCurrencyPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCostCenterDetail).EndInit();
		this.panel23.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefAccountId).EndInit();
		this.groupboxdetail.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		this.PanelDebitaccount.ResumeLayout(false);
		this.PanelDebitaccount.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountId).EndInit();
		this.PanelSubsidiary.ResumeLayout(false);
		this.PanelSubsidiary.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).EndInit();
		this.PanelJobLot.ResumeLayout(false);
		this.PanelJobLot.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotId).EndInit();
		this.PanelComments.ResumeLayout(false);
		this.PanelComments.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTcyCodeDetail).EndInit();
		this.panel27.ResumeLayout(false);
		this.panel27.PerformLayout();
		this.FcyAmountDetailPanel.ResumeLayout(false);
		this.FcyAmountDetailPanel.PerformLayout();
		this.PanelAmount.ResumeLayout(false);
		this.PanelAmount.PerformLayout();
		this.ReferenceAccountPanel.ResumeLayout(false);
		this.ReferenceAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).EndInit();
		this.PanelCostCenter.ResumeLayout(false);
		this.PanelCostCenter.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		this.PanelBranchName.ResumeLayout(false);
		this.PanelBranchName.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		this.ChequeDetailPanel.ResumeLayout(false);
		this.ChequeDetailPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).EndInit();
		this.PanelAddButton.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.btnPrintFormat2.ResumeLayout(false);
		this.btnPrintFormat2.PerformLayout();
		this.History.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.VoucherInfoBox.ResumeLayout(false);
		this.VoucherInfoBox.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		base.ResumeLayout(false);
	}
}

}