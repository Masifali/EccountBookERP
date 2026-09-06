using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.CommissionAgent;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.CommissionAgent;
using Architecture.WinApp.CommissionAgent.FromWise_Helper_Methods;
using Architecture.WinApp.CommissionAgent.Helper_Methods;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.Cmagt
{

public class frmCommissionAgentTradeBillAgainstGdn : Form
{
	private class CommissionConfig
	{
		public GridEX SourceGrid { get; set; }

		public string OrderIdColumn { get; set; }

		public string AmountColumn { get; set; }

		public string WeightColumn { get; set; }

		public string TargetColumn { get; set; }

		public string RateColumn { get; set; }

		public string UomColumn { get; set; }

		public string TypeColumn { get; set; }

		public string CalcColumn { get; set; }

		public decimal TotalAmount { get; set; }

		public Func<int, decimal> OrderAmountFunc { get; set; }
	}

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtDetail = new DataTable();

	private DataTable dtPurchaseOtherExpense = new DataTable();

	private DataTable dtPurchasePmExpense = new DataTable();

	private DataTable dtSaleOtherExpense = new DataTable();

	private DataTable dtSalePaymentDetail = new DataTable();

	private DataTable dtPurchasePaymentDetail = new DataTable();

	private DataTable dtSalePmExpense = new DataTable();

	private DataTable dtSaleBrokery = new DataTable();

	private DataTable dtSaleCommission = new DataTable();

	private DataTable dtPurchaseBrokery = new DataTable();

	private DataTable dtPurchaseCommission = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtBuyer = new DataTable();

	private DataTable dtBindCombo = new DataTable();

	private DataTable dtCommType = new DataTable();

	private DataTable dtCommUom = new DataTable();

	private DataTable dtViewsData = new DataTable();

	private DataTable dtBaseDateType = new DataTable();

	private DataTable dtEbGridPackingTypes = new DataTable();

	private DataTable dtPmItemForPurchase = new DataTable();

	private DataTable dtPmItemForSale = new DataTable();

	private int RecId;

	private int VoucherHeadId;

	private int updateRowIndex = -1;

	private int DocumentTypeId = 1056;

	private string ScreenName = "frmCommissionAgentTradeBillAgainstGdn";

	private int LocalBaseCurrency;

	private int ForeignBaseCurrency;

	private double LocalBaseCurrencyRate;

	private double ForeignBaseCurrencyRate;

	private decimal PurchaseSaleFreightDiffTolerancePercent;

	private bool FormInitialized;

	private bool ItemSearchWithNameOrCode;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool DisableFreightFieldsOnBillFromGdn;

	private bool EnableSaleAddLessWeightOnBillFromGdn;

	private int CustomGroupForWHTAccounts;

	private int CustomGroupForWHTAccountsSale;

	private int DefaultWhtAccountPurchaseIdForCommissionAgentPortal;

	private int DefaultWhtAccountSaleIdForCommissionAgentPortal;

	private int DefaultTaxTypePurchaseeIdForCommissionAgentPortal;

	private int DefaultTaxTypeSaleIdForCommissionAgentPortal;

	private static bool CalculateByEbUnitPur = true;

	private static bool CalculateByEbTotalPur = false;

	private static bool CalculateByEbUnitSale = true;

	private static bool CalculateByEbTotalSale = false;

	private UserAccount UserAccount = new UserAccount();

	private List<InvCommAgentTradeBillDetail> lstRemoveRecord = new List<InvCommAgentTradeBillDetail>();

	private DataSet LoadedDataSetToGetExpenseAndEbDetail = new DataSet();

	private CommissionConfig[] _commissionConfigs;

	private IContainer components;

	private Panel MainPanel;

	private Panel ToolstripPanel;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnAttachment;

	private Label label18;

	private DateTimePicker txtDocDate;

	private Label label3;

	private Label label8;

	private Label label2;

	private TextBox TxtDocNo;

	internal UltraCombo CmbSupplier;

	internal UltraCombo CmbCustomer;

	private TextBox txtMainRemarks;

	internal UltraCombo CmbDeliveryTermPurchase;

	private TextBox txtDueDaysPurchase;

	internal UltraCombo CmbPaymentTermPurchase;

	private Label label15;

	private Label label12;

	private DateTimePicker txtDueDatePurchase;

	internal UltraCombo CmbPackUom;

	private Label label20;

	internal UltraCombo CmbPackType;

	private Label label19;

	internal UltraCombo CmbCropYear;

	private Label label11;

	internal UltraCombo CmbItemName;

	private Label label10;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel PanelDetailInfo;

	private Label label25;

	private TextBox txtItemAmountPurchase;

	private Label label24;

	private TextBox txtSaleNetRate;

	private Label label23;

	private TextBox txtPurchaseNetRate;

	private Label label22;

	private TextBox txtGrossWeightPurchase;

	private Label label21;

	private TextBox txtQuantity;

	private Panel PanelMainInfo;

	private Panel PanelFormHeading;

	private TabPage tabPage2;

	private TextBox txtDetailRemarks;

	private Label label28;

	private Label label26;

	private TextBox txtItemAmountSale;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Button btnAddDetail;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel4;

	private ToolStrip toolStrip2;

	private ToolStripButton btnGrnFormHistory;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX GrdHistoryDetail;

	internal GridEX grd;

	private TextBox txtRefDocNo;

	private Label label32;

	private TextBox txtVehicleNo;

	private Label label27;

	private Label label31;

	private TextBox txtBillWeightPurchase;

	private TextBox txtPurchaseBillAmount;

	private TextBox txtSaleBillAmount;

	private Label label33;

	private Label label36;

	private TextBox txtEBTotalPurchase;

	private Label label37;

	private TextBox txtEBUnitPurchase;

	private Label label29;

	internal UltraCombo CmbRateUomPurchase;

	internal UltraCombo CmbTradingAccount;

	private Label label38;

	private ToolStripButton btnPrint;

	private CheckBox ChkPreview1056;

	private ToolStripButton btnPrint103;

	private ToolStripButton btnPurchasePrint;

	private ToolStripButton btnSalePrint;

	internal GridEX grdSalePmExpenseSale;

	private Label label39;

	private TextBox txtEBTotalSale;

	private Label label40;

	private TextBox txtEBUnitSale;

	private Label label41;

	private TextBox txtBillWeightSale;

	private TabControl tabControl2;

	private TabPage tabPurchaseExpense;

	private TabPage tabPurchasePmExpense;

	internal GridEX grdPurchasePmExpense;

	private Label label34;

	private TabPage tabSaleExpense;

	private TabPage tabSalePmExpense;

	private GroupBox GbDetailInfo;

	private GroupBox GbMain;

	private Label label35;

	private CtrlGrdBar ctrlGrdBar3;

	private TextBox txtAddlessWtSales;

	private Label label9;

	private Label label43;

	private Panel panel13;

	private Panel panel14;

	private Label label44;

	private Panel panel20;

	private TextBox txtOrderNoTo;

	private TextBox txtOrderNoFrom;

	private Label label46;

	internal UltraCombo cmbperemeter;

	private Label label47;

	private Button btnshowHistory;

	private DateTimePicker ToDate;

	private DateTimePicker fromdate;

	private Label label48;

	internal UltraCombo cmbtradAcHistory;

	private Label label49;

	private ToolStripButton btnNewHistory;

	internal UltraCombo cmbcusthistory;

	private Label label51;

	internal UltraCombo cmbsupplierhistory;

	private Label label50;

	private Label label52;

	private TextBox txtProfitLoss;

	internal GridEX grdPurchaseOtherExpenses;

	private Label label53;

	internal GridEX grdSaleOtherExpense;

	private Label label54;

	internal GridEX GrdHistory;

	private Panel panel11;

	private Label label42;

	private Panel panel9;

	internal UltraCombo CmbDeliveryTermSale;

	private Label label55;

	private TabPage tabCommissionAndBrokery;

	private Label label66;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private ToolStripButton BtnDelete;

	private GroupBox GbPurchaseSide;

	private GroupBox GbSaleSide;

	internal UltraCombo CmbPaymentTermSale;

	private DateTimePicker txtDueDateSale;

	private Label label67;

	private TextBox txtDueDaysSale;

	private Label label16;

	private Label label1;

	private TextBox txtBranchSrNo;

	private TextBox txtGrossWeightSale;

	private Label label17;

	private TabPage tabDetail;

	private Label label30;

	private GroupBox GbWbComparison;

	private TextBox txtNetWeightDifference;

	private TextBox txtSupplierWeight;

	private TextBox txtSecondWtDifference;

	private TextBox txtSupplierSecondWeight;

	private Label label80;

	private Label label82;

	private TextBox txtFirstWtDifference;

	private Label label83;

	private TextBox txtSupplierFirstWeight;

	private Label label84;

	private Label label85;

	private Label label86;

	private TextBox txtBuyerWeight;

	private TextBox txtBuyerSecondWeight;

	private TextBox txtBuyerFirstWeight;

	private TextBox txtAddlessWtPurchase;

	private Label label69;

	internal UltraCombo CmbRateUomSale;

	private Label label70;

	private TextBox txtTotalAmountSale;

	private TextBox txtTaxPercntSale;

	private Label label71;

	private Label label72;

	private TextBox txtTaxAmountSale;

	private Label label73;

	private Label label74;

	internal UltraCombo CmbTaxNameSale;

	private TextBox txtTotalAmountPurchase;

	private TextBox txtTaxPercntPurchase;

	private Label label75;

	private Label label76;

	private TextBox txtTaxAmountPurchase;

	private Label label77;

	private Label label78;

	internal UltraCombo CmbTaxNamePurchase;

	private Label label79;

	private TextBox txtbiltyno;

	private TabPage tabPaymentDetail;

	private Panel panel2;

	internal GridEX grdPaymentDetailSale;

	private Label label87;

	private Panel panel1;

	internal GridEX grdPaymentDetailPurchase;

	private Label label81;

	private Label label88;

	internal UltraCombo CmbBranch;

	private Label label89;

	internal UltraCombo CmbCompanyName;

	private ToolStripButton btnLoadGdn;

	private ToolStripButton BtnShortCutkeys;

	private TabControl tabControl3;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Panel panel3;

	internal GridEX GrdHistoryPaymentDetailSale;

	private Label label91;

	private Panel panel5;

	internal GridEX GrdHistoryPaymentDetailPurchase;

	private Label label92;

	private TabPage tabPage5;

	internal GridEX GrdHistoryPurchaseExpenseDetail;

	private Label label93;

	private TabPage tabPage6;

	private Panel panel6;

	internal GridEX GrdHistoryPurchaseFreightDetail;

	private Label label94;

	private TabPage tabPage7;

	internal GridEX GrdHistorySaleExpenseDetail;

	private Label label95;

	private TabPage tabPage8;

	private Panel panel7;

	internal GridEX GrdHistorySalePmDetail;

	private Label label96;

	private TabPage tabPage9;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private RadioButton drdocdate;

	private Label lblebTerm;

	internal UltraCombo CmbEmptyBagTerm;

	private Label label45;

	internal UltraCombo CmbEmptyBagTermPurchase;

	private Panel panel15;

	private Panel panel18;

	internal GridEX grdSaleBrokery;

	private Label label100;

	private Panel panel19;

	internal GridEX grdPurchaseBrokery;

	private Label label101;

	private Panel panel12;

	private Panel panel16;

	internal GridEX grdSaleCommission;

	private Label label98;

	private Panel panel17;

	internal GridEX grdPurchaseCommission;

	private Label label99;

	private Panel panel21;

	private Panel panel22;

	internal GridEX GrdHistorySaleBrokeryDetail;

	private Label label102;

	private Panel panel23;

	internal GridEX GrdHistoryPurchaseBrokeryDetail;

	private Label label103;

	private Panel panel24;

	private Panel panel25;

	internal GridEX GrdHistorySaleCommissionDetail;

	private Label label104;

	private Panel panel26;

	internal GridEX GrdHistoryPurchaseCommissionDetail;

	private Label label105;

	internal UltraCombo CmbTaxAccountPurchase;

	internal UltraCombo CmbTaxAccountSale;

	private Label label4;

	private Label label5;

	private ToolStripButton BtnPrintII;

	private CheckBox ChkPreview1056A;

	private CheckBox ChkPreview105602;

	private CheckBox ChkPreview105601;

	internal UltraCombo CmbFreightAccountPurchase;

	private Label label6;

	private TextBox txtTotalFreightPurchase;

	private TextBox txtFreightAddLessPurchase;

	private Label label13;

	private TextBox txtFreightAmountPurchase;

	private Label label7;

	private Label label14;

	private TextBox txtFreightRemarksPurchase;

	private Label label56;

	private TextBox txtFreightRemarksSale;

	private Label label57;

	private TextBox txtTotalFreightSale;

	internal UltraCombo CmbFreightAccountSale;

	private TextBox txtFreightAddLessSale;

	private Label label58;

	private TextBox txtFreightAmountSale;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label62;

	private TextBox txtBillWeightGrn;

	private TextBox txtBillWeightDiff;

	private TextBox txtBillWeightGdn;

	private Panel panel10;

	private RadioButton RadBusinessNameSale;

	private RadioButton RadNickNameSale;

	private RadioButton radPartyCodeSale;

	private Panel panel8;

	private RadioButton RadBusinessNamePurchase;

	private RadioButton RadNickNamePurchase;

	private RadioButton radPartyCodePurchase;

	internal UltraCombo CmbWhtAccountPurchase;

	private Label label65;

	private Label label64;

	private Label label63;

	private TextBox txtWhtTaxAmountPurchase;

	internal UltraCombo CmbWhtTaxTypePurchase;

	private Label label90;

	private Label label97;

	private TextBox txtWhtTaxPercentPurchase;

	private Label label106;

	private Label label107;

	private Label label108;

	private TextBox txtwhtTaxAmountSale;

	private Label label109;

	internal UltraCombo CmbWhtTaxTypeSale;

	private TextBox txtWhtTaxPercentSale;

	private Label label110;

	private Label label111;

	internal UltraCombo CmbWhtAccountSale;

	private Label label112;

	private Label label68;

	private TextBox txtPurchaseAvgNetRate;

	private TextBox txtDiffAvgNetRate;

	private TextBox txtSaleAvgNetRate;

	private Label label114;

	private TextBox txtSaleRateWithoutAddLess;

	private Label label113;

	private TextBox txtPurchaseRateWithoutAddLess;

	private TextBox txtSaleRateAddLess;

	private Label label116;

	private Label label115;

	private TextBox txtPurchaseRateAddLess;

	private ToolStripButton BtnSalePrintII;

	private CheckBox ChkPreview118;

	private CheckBox ChkPreview105603;

	public event Action FormInitializedAction;

	public frmCommissionAgentTradeBillAgainstGdn(UserAccount pUser)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InitializeComponentCustom()
	{
		try
		{
			ScreenName = Conversion.ToString((object)base.Name);
			TradeBill_Helper.InitializeDetailtable(dtDetail);
			TradeBill_Helper.InitializeFreightDetailTable(dtPurchasePmExpense);
			TradeBill_Helper.InitializePurchaseExpenseDetailtable(dtPurchaseOtherExpense);
			TradeBill_Helper.InitializeSaleExpenseDetailtable(dtSaleOtherExpense);
			TradeBill_Helper.InitializeSalePaymentDetailTable(dtSalePaymentDetail);
			TradeBill_Helper.InitializePurchasePaymentDetailTable(dtPurchasePaymentDetail);
			TradeBill_Helper.InitializeExpensePackingMaterialTable(dtSalePmExpense);
			TradeBill_Helper.InitializePurchaseCommissionDetailTable(dtPurchaseCommission);
			TradeBill_Helper.InitializePurchaseBrokeryDetailTable(dtPurchaseBrokery);
			TradeBill_Helper.InitializeSaleCommissionDetailTable(dtSaleCommission);
			TradeBill_Helper.InitializeSaleBrokeryDetailTable(dtSaleBrokery);
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
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			dtViewsData = AllComboServices_Views();
			DataTable historyComboData = HistoryComboDbCall();
			string documentNoData = DocumentNoDbCall();
			string branchNoData = BranchSrNoDbCall();
			DataTable companyData = CompanyDropdownData();
			OtherItemsdtFillDbCall();
			dtPmItemForPurchase = CommonServices.GetPackingMaterialItemsAllocateToFlow();
			dtPmItemForSale = CommonServices.GetPackingMaterialItemsAllocateToFlow(2);
			DataTable taxTypesData = DatatableHelper.TaxTypesDbCall(UserAccount);
			return new
			{
				HistoryComboData = historyComboData,
				DocumentNoData = documentNoData,
				BranchNoData = branchNoData,
				CompanyData = companyData,
				TaxTypesData = taxTypesData
			};
		}).ContinueWith(t =>
		{
			if (!base.IsDisposed)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					var result = t.Result;
					btnsave.Enabled = formright.DoHaveSaveRight;
					btnUpdate.Enabled = formright.DoHaveUpdateRights;
					BtnDelete.Enabled = formright.DoHaveCanDelete;
					btnPrint.Enabled = formright.DoHavePrintRights;
					GetConfigurationsFromGlobalandBind();
					if (RecId == 0)
					{
						TxtDocNo.Text = result.DocumentNoData;
						txtBranchSrNo.Text = Conversion.ToString((object)result.BranchNoData);
					}
					CompanyBind(result.CompanyData);
					BranchesFill();
					CmbCompanyName.Value = UserAccount.CompanyId;
					CmbBranch.Value = UserAccount.BranchesId;
					TradingAccountsBindFromGlobal();
					TaxAccountsBindFromGlobal();
					PurchaseTaxTypesBind(result.TaxTypesData);
					CommonBindings.SupplierDtFillFromGlobal(dtSupplier, RadBusinessNamePurchase.Checked);
					SupplierBind();
					CommonBindings.SupplierDtFillFromGlobal(dtBuyer, RadBusinessNamePurchase.Checked);
					BuyerBind();
					PaymentTermsFill();
					decimal num = Conversion.ToDecimal((object)txtFreightAmountSale.Text);
					decimal num2 = Conversion.ToDecimal((object)txtFreightAmountPurchase.Text);
					DeliveryTermFill();
					txtFreightAmountSale.Text = num.ToString("#,##.##");
					txtFreightAmountPurchase.Text = num2.ToString("#,##.##");
					CommonBindings.ItemdtFillFromGlobal(dtItem);
					ItemNameBind();
					CropDtFillFromGlobalAndBind();
					PackingTypeDtFillFromGlobalAndBind();
					BindViewCombos();
					BindGrids();
					GetCommissionAgentConfigurationsFromGlobalandBind();
					ParameterFill();
					HistoryComboBind(result.HistoryComboData);
					fromdate.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					ToDate.Value = DateTime.Now;
					FormInitialized = true;
					this.FormInitializedAction?.Invoke();
					txtDocDate.Focus();
					txtBuyerWeight.ReadOnly = false;
					txtBuyerWeight.Enabled = true;
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void frmCommissionAgentOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1280, 950);
			txtBillWeightGdn.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable CompanyDropdownData()
	{
		try
		{
			return CommonServices.CompanyServiceBind();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CompanyBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCompanyName, dt, "Id", "CompName", "Company Name", AllColumns: false, Conversion.ToInt(CmbCompanyName.Value));
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
			InfragisticsHelper.BindAndRetainSelection(CmbBranch, dtBranch, "BranchId", "BranchName", "Branch Name", AllColumns: true, Conversion.ToInt(CmbBranch.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalandBind()
	{
		try
		{
			LocalBaseCurrency = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
			LocalBaseCurrencyRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BaseCurrencyRate"));
			ForeignBaseCurrency = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ForeignBaseCurrency"));
			ForeignBaseCurrencyRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("FcyBaseCurrencyRate"));
			PurchaseSaleFreightDiffTolerancePercent = Conversion.ToDecimal((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PurchaseSaleFreightDiffTolerancePercent"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			CustomGroupForWHTAccounts = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CustomGroupForWHTAccounts"));
			CustomGroupForWHTAccountsSale = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CustomGroupForWHTAccountsSale"));
			DefaultWhtAccountPurchaseIdForCommissionAgentPortal = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWhtAccountPurchaseIdForCommissionAgentPortal"));
			DefaultWhtAccountSaleIdForCommissionAgentPortal = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWhtAccountSaleIdForCommissionAgentPortal"));
			DefaultTaxTypeSaleIdForCommissionAgentPortal = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultTaxTypeSaleIdForCommissionAgentPortal"));
			DefaultTaxTypePurchaseeIdForCommissionAgentPortal = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultTaxTypePurchaseeIdForCommissionAgentPortal"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
			DisableFreightFieldsOnBillFromGdn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DisableFreightFieldsOnBillFromGdn"));
			EnableSaleAddLessWeightOnBillFromGdn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("EnableSaleAddLessWeightOnBillFromGdn"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCommissionAgentConfigurationsFromGlobalandBind()
	{
		try
		{
			int tradingAccountId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultTradingAccountIdForCommissionAgentPortal"));
			if (tradingAccountId > 0)
			{
				CmbTradingAccount.Value = tradingAccountId;
			}
			int taxAccountId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultTaxAccountIdForCommissionAgentPortal"));
			if (taxAccountId > 0)
			{
				CmbTaxAccountPurchase.Value = taxAccountId;
				CmbTaxAccountSale.Value = taxAccountId;
			}
			int paymentTermId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultPaymentTermIdForCommissionAgentPortal"));
			if (paymentTermId > 0)
			{
				CmbPaymentTermSale.Value = paymentTermId;
				CmbPaymentTermPurchase.Value = paymentTermId;
			}
			int deliveryTermId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDeliveryTermIdForCommissionAgentPortal"));
			if (deliveryTermId > 0)
			{
				if (Conversion.ToInt(CmbDeliveryTermPurchase.Value) == 0)
				{
					CmbDeliveryTermPurchase.Value = deliveryTermId;
				}
				if (Conversion.ToInt(CmbDeliveryTermSale.Value) == 0)
				{
					CmbDeliveryTermSale.Value = deliveryTermId;
				}
			}
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultCropYearIdForCommissionAgentPortal"));
			if (cropYearId > 0)
			{
				CmbCropYear.Value = cropYearId;
			}
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultPackingTypeIdForCommissionAgentPortal"));
			if (packingTypeId > 0)
			{
				CmbPackType.Value = packingTypeId;
			}
			if (DefaultWhtAccountPurchaseIdForCommissionAgentPortal > 0)
			{
				CmbWhtAccountPurchase.Value = DefaultWhtAccountPurchaseIdForCommissionAgentPortal;
			}
			if (DefaultWhtAccountSaleIdForCommissionAgentPortal > 0)
			{
				CmbWhtAccountSale.Value = DefaultWhtAccountSaleIdForCommissionAgentPortal;
			}
			if (DefaultTaxTypePurchaseeIdForCommissionAgentPortal > 0)
			{
				CmbWhtTaxTypePurchase.Value = DefaultTaxTypePurchaseeIdForCommissionAgentPortal;
			}
			if (DefaultTaxTypeSaleIdForCommissionAgentPortal > 0)
			{
				CmbWhtTaxTypeSale.Value = DefaultTaxTypeSaleIdForCommissionAgentPortal;
			}
			bool hasGdn = GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "GdnId");
			txtBillWeightGdn.Enabled = !hasGdn || EnableSaleAddLessWeightOnBillFromGdn;
			if (grd.RowCount > 0)
			{
				grd.RootTable.Columns["SaleAddLess"].EditType = (EditType)(EnableSaleAddLessWeightOnBillFromGdn ? 1 : 0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return InvCommAgentTradeBill.GenerateCode(new InvCommAgentTradeBill
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private string BranchSrNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return InvCommAgentTradeBill.GenerateBranchSrCode(new InvCommAgentTradeBill
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TradingAccountsBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 10 });
			InfragisticsHelper.BindAndRetainSelection(CmbTradingAccount, dt, "Id", "AccountTitle", "Trading Account", AllColumns: true, Conversion.ToInt(CmbTradingAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaxAccountsBindFromGlobal()
	{
		try
		{
			DataTable dt1 = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 19 });
			InfragisticsHelper.BindAndRetainSelection(CmbTaxAccountPurchase, dt1, "Id", "AccountTitle", "Tax Account", AllColumns: true, Conversion.ToInt(CmbTaxAccountPurchase.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbTaxAccountSale, dt1, "Id", "AccountTitle", "Tax Account", AllColumns: true, Conversion.ToInt(CmbTaxAccountSale.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			DataTable dtForPurchase = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, null, null, null, null, null, null, new int[1] { CustomGroupForWHTAccounts });
			InfragisticsHelper.BindAndRetainSelection(CmbWhtAccountPurchase, dtForPurchase, "Id", "AccountTitle", "WHT Credit Account", AllColumns: true, Conversion.ToInt(CmbWhtAccountPurchase.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			DataTable dtForsale = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, null, null, null, null, null, null, new int[1] { CustomGroupForWHTAccountsSale });
			InfragisticsHelper.BindAndRetainSelection(CmbWhtAccountSale, dtForsale, "Id", "AccountTitle", "WHT Debit Account", AllColumns: true, Conversion.ToInt(CmbWhtAccountSale.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseTaxTypesBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbWhtTaxTypePurchase, dt, "Id", "TaxName", "Tax Type", AllColumns: true, Conversion.ToInt(CmbWhtTaxTypePurchase.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleTaxTypesBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomerTaxSchedule.TaxSchedule_GetBySupplierCustomer(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value),
				DocDate = txtDocDate.Value
			});
			InfragisticsHelper.BindAndRetainSelection(CmbWhtTaxTypeSale, dt, "Id", "TaxName", "Tax Type", AllColumns: true, Conversion.ToInt(CmbWhtTaxTypeSale.Value));
			CmbWhtTaxTypeSale_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierBind()
	{
		CommonBindings.SupplierBind(CmbSupplier, dtSupplier, "Supplier Name", radPartyCodePurchase);
		CommonBindings.SupplierBind(CmbFreightAccountPurchase, dtSupplier, "Freight Account (Dr)");
	}

	private void BuyerBind()
	{
		CommonBindings.SupplierBind(CmbCustomer, dtBuyer, "Buyer Name", radPartyCodeSale);
		CommonBindings.SupplierBind(CmbFreightAccountSale, dtBuyer, "Freight Account (Cr)");
		CmbCustomer_TextChanged(null, null);
	}

	private void PaymentTermsFill()
	{
		try
		{
			DataTable dtTerm = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalPaymentTerm, (InvDueTerms x) => x.Id, (InvDueTerms x) => x.TermsDescription);
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTermPurchase, dtTerm, "Id", "Description", "Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTermPurchase.Value));
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTermSale, dtTerm, "Id", "Description", "Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTermSale.Value));
			if (dtTerm != null && dtTerm.Rows.Count > 0)
			{
				CmbPaymentTermPurchase_ValueChanged(null, null);
				CmbPaymentTermSale_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			DataTable dtTerm = DeliveryTerm.FormHistory();
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTermPurchase, dtTerm, "Id", "Description", "Delivery Term", AllColumns: false, Conversion.ToInt(CmbDeliveryTermPurchase.Value));
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTermSale, dtTerm, "Id", "Description", "Delivery Term", AllColumns: false, Conversion.ToInt(CmbDeliveryTermSale.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplier_Leave(object sender, EventArgs e)
	{
		try
		{
			CheckSupplierAndFreightGlAccount();
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
			CheckCustomerAndPMSaleGlAccount();
			ExpenseProportionForSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemName.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", "ItemName", "Item Name", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
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
			DataTable dtCrop = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalCropYear, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			int Id = Conversion.ToInt(CmbCropYear.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCropYear, dtCrop, "Id", "Description", "Crop Year", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackingTypeDtFillFromGlobalAndBind()
	{
		try
		{
			DataTable dtPackType = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalInvPackingType, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			int Id = Conversion.ToInt(CmbPackType.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPackType, dtPackType, "Id", "Description", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TaxTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbTaxNamePurchase, dt, "Id", "TaxType", "Tax Name", AllColumns: true, Conversion.ToInt(CmbTaxNamePurchase.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbTaxNameSale, dt, "Id", "TaxType", "Tax Name", AllColumns: true, Conversion.ToInt(CmbTaxNameSale.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (updateRowIndex > -1 && grd.CurrentRow != null)
			{
				if (Conversion.ToInt(grd.CurrentRow.Cells["PurchaseTaxNameId"].Value) > 0)
				{
					CmbTaxNamePurchase.Value = Conversion.ToInt(grd.CurrentRow.Cells["PurchaseTaxNameId"].Value);
				}
				if (Conversion.ToInt(grd.CurrentRow.Cells["SaleTaxNameId"].Value) > 0)
				{
					CmbTaxNameSale.Value = Conversion.ToInt(grd.CurrentRow.Cells["SaleTaxNameId"].Value);
				}
			}
			CmbTaxNamePurchase_Leave(null, null);
			CmbTaxNameSale_Leave(null, null);
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
			if (dtItem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItemName.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemName.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtItem, CmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtItem, CmbItemName, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItemName.Value = ItemId;
				CmbItemName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = 0;
			if (((UltraGridBase)CmbItemName).ActiveRow != null)
			{
				ItemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value);
				TaxTypeBind(DatatableHelper.TaxTypeDbCallByItemId(ItemId, txtDocDate.Value));
			}
			else
			{
				((Control)(object)CmbTaxNamePurchase).Text = string.Empty;
				((UltraGridBase)CmbTaxNamePurchase).DataSource = null;
				txtTaxPercntPurchase.Clear();
				txtTaxAmountPurchase.Clear();
				((Control)(object)CmbTaxNameSale).Text = string.Empty;
				((UltraGridBase)CmbTaxNameSale).DataSource = null;
				txtTaxPercntSale.Clear();
				txtTaxAmountSale.Clear();
			}
			CommonBindings.ItemUomFromGlobalBind(ItemId, CmbPackUom, CmbRateUomPurchase);
			CommonBindings.ItemUomFromGlobalBind(ItemId, null, CmbRateUomSale);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxNamePurchase_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmountPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxNameSale_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmountSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable AllComboServices_Views()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return purchaseOrderMaster.AllComboServices_FromViews(new ReportsParameters
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

	private void BindViewCombos()
	{
		try
		{
			dtEbGridPackingTypes.Rows.Clear();
			dtBaseDateType.Rows.Clear();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dtEbGridPackingTypes = dt.Clone();
			dtBaseDateType = dt.Clone();
			dtCommType = dt.Clone();
			dtCommUom = dt.Clone();
			for (int i = 0; i < dtViewsData.Rows.Count; i++)
			{
				switch (Conversion.ToString(dtViewsData.Rows[i]["Activity"]))
				{
				case "AllocatedPackingType":
					dtEbGridPackingTypes.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
					break;
				case "EmptyBagTypes":
					dt.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
					break;
				case "PaymentBaseDate":
					dtBaseDateType.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
					break;
				case "CommissionType":
					dtCommType.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
					break;
				case "CommissionRateUom":
					dtCommUom.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
					break;
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbEmptyBagTerm, dt, "Id", "Name", "Empty Bag term", AllColumns: true, Conversion.ToInt(CmbEmptyBagTerm.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbEmptyBagTermPurchase, dt, "Id", "Name", "Empty Bag term", AllColumns: true, Conversion.ToInt(CmbEmptyBagTermPurchase.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OtherItemsdtFillDbCall()
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

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			ResetForm();
			FormResetDBCallsAndDefault();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetForm()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			updateRowIndex = -1;
			EnableAndDisableFields(flag: true);
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			btnAddDetail.Visible = true;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnDelete.Visible = false;
			txtBuyerWeight.ReadOnly = false;
			txtBuyerWeight.Enabled = true;
			TxtDocNo.Text = "0";
			txtBranchSrNo.Text = "0";
			txtBuyerFirstWeight.Text = string.Empty;
			txtBuyerSecondWeight.Text = string.Empty;
			txtSupplierFirstWeight.Text = string.Empty;
			txtSupplierSecondWeight.Text = string.Empty;
			txtBillWeightGrn.Text = string.Empty;
			txtBillWeightGdn.Text = string.Empty;
			txtPurchaseBillAmount.Clear();
			txtSaleBillAmount.Clear();
			txtProfitLoss.Clear();
			txtMainRemarks.Clear();
			txtFreightAmountPurchase.Text = "0";
			txtFreightAddLessPurchase.Text = "0";
			txtTotalFreightPurchase.Text = "0";
			txtFreightRemarksPurchase.Text = string.Empty;
			CmbFreightAccountPurchase.Value = null;
			txtFreightAmountSale.Text = "0";
			txtFreightAddLessSale.Text = "0";
			txtTotalFreightSale.Text = "0";
			txtFreightRemarksSale.Text = string.Empty;
			CmbFreightAccountSale.Value = null;
			txtWhtTaxPercentPurchase.Text = "0";
			txtWhtTaxAmountPurchase.Text = "0";
			CmbWhtAccountPurchase.Value = null;
			CmbWhtTaxTypePurchase.Value = null;
			txtWhtTaxPercentSale.Text = "0";
			txtwhtTaxAmountSale.Text = "0";
			CmbWhtAccountSale.Value = null;
			CmbWhtTaxTypeSale.Value = null;
			DetailFormReset();
			PanelDetailInfo.Visible = true;
			dtDetail.Rows.Clear();
			dtPurchaseOtherExpense.Rows.Clear();
			dtSaleOtherExpense.Rows.Clear();
			dtSalePmExpense.Rows.Clear();
			dtPurchasePmExpense.Rows.Clear();
			dtPurchaseCommission.Rows.Clear();
			dtPurchaseBrokery.Rows.Clear();
			dtSaleCommission.Rows.Clear();
			dtSaleBrokery.Rows.Clear();
			dtPurchasePaymentDetail.Rows.Clear();
			dtSalePaymentDetail.Rows.Clear();
			BindGrids();
			txtDocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormResetDBCallsAndDefault()
	{
		try
		{
			TxtDocNo.Text = DocumentNoDbCall();
			txtBranchSrNo.Text = BranchSrNoDbCall();
			GetConfigurationsFromGlobalandBind();
			GetCommissionAgentConfigurationsFromGlobalandBind();
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
			txtQuantity.Clear();
			txtGrossWeightPurchase.Clear();
			txtGrossWeightSale.Clear();
			txtEBUnitPurchase.Clear();
			txtEBTotalPurchase.Clear();
			txtBillWeightPurchase.Clear();
			txtEBUnitSale.Clear();
			txtEBTotalSale.Clear();
			txtAddlessWtSales.Clear();
			txtBillWeightSale.Clear();
			txtPurchaseRateWithoutAddLess.Clear();
			txtPurchaseNetRate.Clear();
			txtSaleRateWithoutAddLess.Clear();
			txtSaleNetRate.Clear();
			txtItemAmountPurchase.Clear();
			txtItemAmountSale.Clear();
			txtVehicleNo.Clear();
			txtRefDocNo.Clear();
			txtDetailRemarks.Clear();
			CmbItemName.Focus();
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
			RefreshFormFields();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefreshFormFields()
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "SupplierCustomerLists", "PaymentTerm", "CropYear", "InvPackingType", "AllItems", "UomSchedule" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobalandBind();
			TradingAccountsBindFromGlobal();
			TaxAccountsBindFromGlobal();
			PurchaseTaxTypesBind(DatatableHelper.TaxTypesDbCall(UserAccount));
			CommonBindings.SupplierDtFillFromGlobal(dtSupplier, RadBusinessNamePurchase.Checked);
			SupplierBind();
			CommonBindings.SupplierDtFillFromGlobal(dtBuyer, RadBusinessNamePurchase.Checked);
			BuyerBind();
			PaymentTermsFill();
			DeliveryTermFill();
			CropDtFillFromGlobalAndBind();
			PackingTypeDtFillFromGlobalAndBind();
			CommonBindings.ItemdtFillFromGlobal(dtItem);
			ItemNameBind();
			dtViewsData = AllComboServices_Views();
			BindViewCombos();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionAgentTradeBill_162_FormClosing(object sender, FormClosingEventArgs e)
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

	private void CheckSupplierAndFreightGlAccount()
	{
		try
		{
			bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
			if (!(grdPurchasePmExpense.GetRows().Count() > 0 && Conversion.ToInt(CmbSupplier.Value) > 0 && isPurchaseDelivered))
			{
				return;
			}
			int glId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString(CmbSupplier.Value)));
			GridEXRow[] rows = grdPurchasePmExpense.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["AccountId"].Value) == glId)
				{
					((Control)(object)CmbSupplier).Text = string.Empty;
					MessageBox.Show("Freight Account And Supplier Gl Account Can't be Same. First Delete Freight Grid Row#" + (item.RowIndex + 1) + " Or Change the Account in Freight Grid", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionAgentAccoutValidation()
	{
		try
		{
			if (((UltraGridBase)CmbSupplier).ActiveRow != null && ((UltraGridBase)CmbCustomer).ActiveRow != null && Conversion.ToInt(CmbSupplier.Value) > 0 && Conversion.ToInt(CmbCustomer.Value) > 0 && Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbCustomer.Value))
			{
				CmbCustomer.Value = 0;
				CmbCustomer.Focus();
				tabControl2.SelectedIndex = 4;
				throw new Exception("Supplier Ac and Customer  Ac cannot be same");
			}
			CommissionAndBrokeryGridValidation(grdPurchaseCommission, "CommissionAgentId", "CommissionAmount", "In Purchase Commission Grid: Commission Agent cannot be the same as the Debit Account", 5);
			CommissionAndBrokeryGridValidation(grdSaleCommission, "CommissionAgentId", "CommissionAmount", "In Sale Commission Grid: Commission Agent cannot be the same as the Debit Account", 5);
			CommissionAndBrokeryGridValidation(grdPurchaseBrokery, "BrokeryAgentId", "BrokeryAmount", "In Purchase Brokery Grid: Broker cannot be the same as the Debit Account", 5);
			CommissionAndBrokeryGridValidation(grdSaleBrokery, "BrokeryAgentId", "BrokeryAmount", "In Sale Brokery Grid: Broker cannot be the same as the Debit Account", 5);
			SupplierBillAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (TxtDocNo.Text.Trim() == "" || TxtDocNo.Text.Trim() == "0" || TxtDocNo.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			TxtDocNo.Focus();
			return false;
		}
		if (txtBranchSrNo.Text.Trim() == "" || txtBranchSrNo.Text.Trim() == "0" || txtBranchSrNo.Text.Trim() == null)
		{
			MessageBox.Show("Branch Doc No Field is Required");
			txtBranchSrNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbTradingAccount).ActiveRow == null || Conversion.ToInt(CmbTradingAccount.Value) == 0)
		{
			MessageBox.Show("TradingAccount Field is Required");
			CmbTradingAccount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSupplier).ActiveRow == null || Conversion.ToInt(CmbSupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			CmbSupplier.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTermPurchase).ActiveRow == null || Conversion.ToInt(CmbDeliveryTermPurchase.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			CmbDeliveryTermPurchase.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentDetailPurchase, "Amount")) == 0m)
		{
			if (((UltraGridBase)CmbPaymentTermPurchase).ActiveRow == null || Conversion.ToInt(CmbPaymentTermPurchase.Value) == 0)
			{
				MessageBox.Show("Payment Term Field is Required");
				CmbPaymentTermPurchase.Focus();
				return false;
			}
			if (((Control)(object)CmbPaymentTermPurchase).Text == "Credit" && Conversion.ToInt((object)txtDueDaysPurchase.Text.Trim()) == 0)
			{
				MessageBox.Show("Due Days Field Required");
				txtDueDaysPurchase.Focus();
				return false;
			}
		}
		if (((UltraGridBase)CmbCustomer).ActiveRow == null || Conversion.ToInt(CmbCustomer.Value) == 0)
		{
			MessageBox.Show("Customer Field is Required");
			CmbCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTermSale).ActiveRow == null || Conversion.ToInt(CmbDeliveryTermSale.Value) == 0)
		{
			MessageBox.Show("Delivery Term Sale Field is Required");
			CmbDeliveryTermSale.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentDetailSale, "Amount")) == 0m)
		{
			if (((UltraGridBase)CmbPaymentTermSale).ActiveRow == null || Conversion.ToInt(CmbPaymentTermSale.Value) == 0)
			{
				MessageBox.Show("Payment Term Sale Field is Required");
				CmbPaymentTermSale.Focus();
				return false;
			}
			if (((Control)(object)CmbPaymentTermSale).Text == "Credit" && Conversion.ToInt((object)txtDueDaysSale.Text.Trim()) == 0)
			{
				MessageBox.Show("Due Days Sale Field Required");
				txtDueDaysSale.Focus();
				return false;
			}
		}
		if (Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbCustomer.Value))
		{
			CmbCustomer.Value = 0;
			CmbCustomer.Focus();
			MessageBox.Show("Supplier Ac and Customer  Ac cannot be same");
			return false;
		}
		if (Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "PurchaseTaxAmount")) > 0m && (((UltraGridBase)CmbTaxAccountPurchase).ActiveRow == null || Conversion.ToInt(CmbTaxAccountPurchase.Value) == 0))
		{
			MessageBox.Show("Purchase Tax Account field is required");
			CmbTaxAccountPurchase.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "SaleTaxAmount")) > 0m && (((UltraGridBase)CmbTaxAccountSale).ActiveRow == null || Conversion.ToInt(CmbTaxAccountSale.Value) == 0))
		{
			MessageBox.Show("Sale Tax Account field is required");
			CmbTaxAccountSale.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtBuyerWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Buyer Wb NetWeight Should Be Greater Than 0");
			txtBuyerWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtSupplierWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Supplier Wb NetWeight Should Be Greater Than 0");
			txtSupplierWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPurchaseBillAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Purchase Bill Amount Should Be Greater Than 0");
			txtPurchaseBillAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtSaleBillAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Sale Bill Amount Should Be Greater Than 0");
			txtSaleBillAmount.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("Item Field Required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field Required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackType).ActiveRow == null || Conversion.ToInt(CmbPackType.Value) == 0)
		{
			MessageBox.Show("Pack Type Field Required");
			CmbPackType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack Uom Field Required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtQuantity.Text.Trim()) == 0)
		{
			MessageBox.Show("Quantity Field Required");
			txtQuantity.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGrossWeightPurchase.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Purchase Field is Required");
			txtGrossWeightPurchase.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtBillWeightPurchase.Text.Trim()) == 0.0)
		{
			MessageBox.Show(" Purchase Bill Weight Field is Required");
			txtGrossWeightPurchase.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGrossWeightSale.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Sale Field is Required");
			txtGrossWeightSale.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtBillWeightSale.Text.Trim()) == 0.0)
		{
			MessageBox.Show(" Sale Bill Weight Field is Required");
			txtGrossWeightPurchase.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPurchaseNetRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate Purchase Field is Required");
			txtPurchaseNetRate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUomPurchase).ActiveRow == null || Conversion.ToInt(CmbRateUomPurchase.Value) == 0)
		{
			MessageBox.Show("RateUom Field is Required");
			CmbRateUomPurchase.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtSaleNetRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate Sale Field is Required");
			txtSaleNetRate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUomSale).ActiveRow == null || Conversion.ToInt(CmbRateUomSale.Value) == 0)
		{
			MessageBox.Show("RateUom Sale Field is Required");
			CmbRateUomSale.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemAmountPurchase.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Purchase Amount Field is Required");
			txtItemAmountPurchase.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemAmountSale.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Sale Amount Field is Required");
			txtItemAmountSale.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtVehicleNo.Text.Trim()) == string.Empty)
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtVehicleNo.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtbiltyno.Text.Trim()) == string.Empty)
		{
			MessageBox.Show("Bilty No Field is Required");
			txtbiltyno.Focus();
			return false;
		}
		return true;
	}

	private void CheckCustomerAndPMSaleGlAccount()
	{
		try
		{
			bool isSaleXFactory = FormHelper.DeliveryTermIsLoad(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text));
			if (!(grdSalePmExpenseSale.GetRows().Count() > 0 && Conversion.ToInt(CmbCustomer.Value) > 0 && isSaleXFactory))
			{
				return;
			}
			int glId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString(CmbCustomer.Value)));
			GridEXRow[] rows = grdSalePmExpenseSale.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				if (Conversion.ToInt(rows[i].Cells["AccountId"].Value) == glId)
				{
					((Control)(object)CmbCustomer).Text = string.Empty;
					MessageBox.Show("PM Sale Account And Customer Gl Account Can't be Same", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplier_TextChanged(object sender, EventArgs e)
	{
		CommissionAgentAccoutValidation();
	}

	private void CmbCustomer_TextChanged(object sender, EventArgs e)
	{
		CommissionAgentAccoutValidation();
		SaleTaxTypesBind();
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtDetail;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				grdSettings(grd);
			}
			DataTable dataTable2 = dtPurchaseOtherExpense;
			if (dataTable2 != null && dataTable2.Columns.Count > 0)
			{
				if (dtPurchaseOtherExpense.Rows.Count == 0)
				{
					AddRowsInPurchaseExpenseGrid(dtPurchaseOtherExpense);
				}
				grdPurchaseOtherExpenses.DataSource = dtPurchaseOtherExpense;
				grdPurchaseOtherExpenses.RetrieveStructure();
				grdPurchaseOtherExpensesSettings(grdPurchaseOtherExpenses);
			}
			DataTable dataTable3 = dtSaleOtherExpense;
			if (dataTable3 != null && dataTable3.Columns.Count > 0)
			{
				if (dtSaleOtherExpense.Rows.Count == 0)
				{
					AddRowsInSaleExpenseGrid(dtSaleOtherExpense);
				}
				grdSaleOtherExpense.DataSource = dtSaleOtherExpense;
				grdSaleOtherExpense.RetrieveStructure();
				grdSaleOtherExpenseSettings(grdSaleOtherExpense);
			}
			DataTable dataTable4 = dtPurchasePmExpense;
			if (dataTable4 != null && dataTable4.Columns.Count > 0)
			{
				if (dtPurchasePmExpense.Rows.Count == 0)
				{
					AddRowInPurchasePmExpenseGrid();
				}
				grdPurchasePmExpense.RetrieveStructure();
				grdPurchasePmExpenseSettings(grdPurchasePmExpense);
			}
			DataTable dataTable5 = dtSalePmExpense;
			if (dataTable5 != null && dataTable5.Columns.Count > 0)
			{
				if (dtSalePmExpense.Rows.Count == 0)
				{
					AddRowInSalePmExpenseGrid();
				}
				grdSalePmExpenseSale.RetrieveStructure();
				grdSalePmExpenseSettings(grdSalePmExpenseSale);
			}
			DataTable dataTable6 = dtPurchasePaymentDetail;
			if (dataTable6 != null && dataTable6.Columns.Count > 0)
			{
				if (dtPurchasePaymentDetail.Rows.Count == 0)
				{
					AddRowInPurchasePaymentGrid();
				}
				grdPaymentDetailPurchase.RetrieveStructure();
				grdPaymentDetailPurchaseSetting(grdPaymentDetailPurchase);
			}
			DataTable dataTable7 = dtSalePaymentDetail;
			if (dataTable7 != null && dataTable7.Columns.Count > 0)
			{
				if (dtSalePaymentDetail.Rows.Count == 0)
				{
					AddRowInSalePaymentGrid();
				}
				grdPaymentDetailSale.RetrieveStructure();
				grdPaymentDetailSaleSetting(grdPaymentDetailSale);
			}
			DataTable dataTable8 = dtPurchaseCommission;
			if (dataTable8 != null && dataTable8.Columns.Count > 0)
			{
				if (dtPurchaseCommission.Rows.Count == 0)
				{
					AddRowInPurchaseCommissionGrid();
				}
				grdPurchaseCommission.RetrieveStructure();
				grdPurchaseCommissionDetailSetting(grdPurchaseCommission);
			}
			DataTable dataTable9 = dtPurchaseBrokery;
			if (dataTable9 != null && dataTable9.Columns.Count > 0)
			{
				if (dtPurchaseBrokery.Rows.Count == 0)
				{
					AddRowInPurchaseBrokeryGrid();
				}
				grdPurchaseBrokery.RetrieveStructure();
				grdPurchaseBrokeryDetailSetting(grdPurchaseBrokery);
			}
			DataTable dataTable10 = dtSaleCommission;
			if (dataTable10 != null && dataTable10.Columns.Count > 0)
			{
				if (dtSaleCommission.Rows.Count == 0)
				{
					AddRowInSaleCommissionGrid();
				}
				grdSaleCommission.RetrieveStructure();
				grdSaleCommissionDetailSetting(grdSaleCommission);
			}
			DataTable dataTable11 = dtSaleBrokery;
			if (dataTable11 != null && dataTable11.Columns.Count > 0)
			{
				if (dtSaleBrokery.Rows.Count == 0)
				{
					AddRowInSaleBrokeryGrid();
				}
				grdSaleBrokery.RetrieveStructure();
				grdSaleBrokeryDetailSetting(grdSaleBrokery);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddEmptyBagTermInPurchaseSaleGrid(GridEX gridEX)
	{
		GridEXRow[] dataRows = gridEX.GetDataRows();
		foreach (GridEXRow obj in dataRows)
		{
			Conversion.ToInt(obj.Cells["PackingTypeId"].Value);
			Conversion.ToInt(obj.Cells["PmItemId"].Value);
			obj.BeginEdit();
			obj.Cells["EmptyBagTermId"].Value = Conversion.ToInt(CmbEmptyBagTerm.Value);
			obj.Cells["EmptyBagTerm"].Value = Conversion.ToString((object)((Control)(object)CmbEmptyBagTerm).Text);
			obj.EndEdit();
		}
	}

	private void FillDetailRow(DataRow dr)
	{
		dr["ItemId"] = CmbItemName.Value;
		dr["ItemCode"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value;
		dr["ItemName"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value;
		dr["CropYearId"] = CmbCropYear.Value;
		dr["CropYear"] = ((Control)(object)CmbCropYear).Text.Trim();
		dr["PackTypeId"] = CmbPackType.Value;
		dr["PackTypeDesc"] = ((Control)(object)CmbPackType).Text.Trim();
		dr["UomId"] = CmbPackUom.Value;
		dr["UomCode"] = ((Control)(object)CmbPackUom).Text.Trim();
		dr["PackEquivalent"] = ((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value;
		dr["ItemQty"] = Conversion.ToDouble((object)txtQuantity.Text.Trim());
		dr["RefDocNo"] = txtRefDocNo.Text;
		dr["VehicleNo"] = txtVehicleNo.Text;
		dr["BiltyNo"] = txtbiltyno.Text;
		dr["PurchaseGrossWeight"] = Conversion.ToDouble((object)txtGrossWeightPurchase.Text);
		dr["PurchaseEbCut"] = Conversion.ToDouble((object)txtEBUnitPurchase.Text);
		dr["PurchaseEbCutTotal"] = Conversion.ToDouble((object)txtEBTotalPurchase.Text);
		dr["PurchaseAddLessWeight"] = Conversion.ToDouble((object)txtAddlessWtPurchase.Text);
		dr["PurchaseBillWeight"] = Conversion.ToDouble((object)txtBillWeightPurchase.Text);
		dr["PurchaseRateWithoutAddLess"] = Conversion.ToDouble((object)txtPurchaseRateWithoutAddLess.Text);
		dr["PurchaseRateAddLess"] = Conversion.ToDouble((object)txtPurchaseRateAddLess.Text);
		dr["PurchaseNetRate"] = Conversion.ToDouble((object)txtPurchaseNetRate.Text);
		dr["PurchaseRateUomId"] = Conversion.ToInt(CmbRateUomPurchase.Value);
		dr["PurchaseRateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUomPurchase).Text);
		dr["PurchaseRateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomPurchase).SelectedRow.Cells[2].Value);
		dr["PurchaseAmount"] = Conversion.ToDouble((object)txtItemAmountPurchase.Text);
		dr["PurchaseTaxNameId"] = Conversion.ToInt(CmbTaxNamePurchase.Value);
		dr["PurchaseTaxName"] = Conversion.ToString((object)((Control)(object)CmbTaxNamePurchase).Text);
		dr["PurchaseTax%"] = Conversion.ToDouble((object)txtTaxPercntPurchase.Text);
		dr["PurchaseTaxAmount"] = Conversion.ToDouble((object)txtTaxAmountPurchase.Text);
		dr["PurchaseNetAmount"] = Conversion.ToDouble((object)txtTotalAmountPurchase.Text);
		dr["SaleGrossWeight"] = Conversion.ToDouble((object)txtGrossWeightSale.Text);
		dr["SaleEbCut"] = Conversion.ToDouble((object)txtEBUnitSale.Text);
		dr["SaleEbCutTotal"] = Conversion.ToDouble((object)txtEBTotalSale.Text);
		dr["SaleAddLess"] = Conversion.ToDouble((object)txtAddlessWtSales.Text);
		dr["SaleBillWeight"] = Conversion.ToDouble((object)txtBillWeightSale.Text);
		dr["SaleRateWithoutAddLess"] = Conversion.ToDouble((object)txtSaleRateWithoutAddLess.Text);
		dr["SaleRateAddLess"] = Conversion.ToDouble((object)txtSaleRateAddLess.Text);
		dr["SaleNetRate"] = Conversion.ToDouble((object)txtSaleNetRate.Text);
		dr["SaleRateUomId"] = Conversion.ToInt(CmbRateUomSale.Value);
		dr["SaleRateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUomSale).Text);
		dr["SaleRateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomSale).SelectedRow.Cells[2].Value);
		dr["SaleAmount"] = Conversion.ToDouble((object)txtItemAmountSale.Text);
		dr["SaleTaxNameId"] = Conversion.ToInt(CmbTaxNameSale.Value);
		dr["SaleTaxName"] = Conversion.ToString((object)((Control)(object)CmbTaxNameSale).Text);
		dr["SaleTax%"] = Conversion.ToDouble((object)txtTaxPercntSale.Text);
		dr["SaleTaxAmount"] = Conversion.ToDecimal((object)txtTaxAmountSale.Text);
		dr["SaleNetAmount"] = Conversion.ToDouble((object)txtTotalAmountSale.Text);
		dr["RemarksDetail"] = Conversion.ToString((object)txtDetailRemarks.Text);
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				DataRow dr = dtDetail.NewRow();
				dr["Id"] = 0;
				FillDetailRow(dr);
				dtDetail.Rows.Add(dr);
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				grdSettings(grd);
				DetailFormReset();
				ExpenseProportionForPurchase();
				ExpenseProportionForSale();
				FreightProportion();
				InitCommissionConfigs();
				CalculateCommissionGrid(GetPurchaseCommissionConfig());
				CalculateCommissionGrid(GetPurchaseBrokeryConfig());
				CalculateCommissionGrid(GetSaleCommissionConfig());
				CalculateCommissionGrid(GetSaleBrokeryConfig());
				ProportionateCommissionAndBrokery();
				SupplierBillAmount();
				CustomerBillAmount();
				PLAmountProportion();
				CalculateHeaderRates();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && Conversion.ToInt(item.Cells["GdnId"].Value) <= 0)
			{
				updateRowIndex = item.RowIndex;
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				CmbItemName_Leave(null, null);
				CmbCropYear.Value = item.Cells["CropYearId"].Text.ToString();
				CmbPackType.Value = item.Cells["PackTypeId"].Value.ToString();
				CmbPackUom.Value = item.Cells["UomId"].Value.ToString();
				txtQuantity.Text = item.Cells["ItemQty"].Value.ToString();
				txtVehicleNo.Text = item.Cells["VehicleNo"].Text.ToString();
				txtRefDocNo.Text = item.Cells["RefDocNo"].Text.ToString();
				txtGrossWeightPurchase.Text = Conversion.ToDouble(item.Cells["PurchaseGrossWeight"].Value).ToString("#,##0.###");
				txtEBUnitPurchase.Text = Conversion.ToDouble(item.Cells["PurchaseEbCut"].Value).ToString("#,##0.###");
				txtEBTotalPurchase.Text = Conversion.ToDouble(item.Cells["PurchaseEbCutTotal"].Value).ToString("#,##0.###");
				txtAddlessWtPurchase.Text = Conversion.ToDouble(item.Cells["PurchaseAddLessWeight"].Value).ToString("#,##0.###");
				txtBillWeightPurchase.Text = Conversion.ToDouble(item.Cells["PurchaseBillWeight"].Value).ToString("#,##0.###");
				CmbRateUomPurchase.Value = item.Cells["PurchaseRateUomId"].Value.ToString();
				txtPurchaseRateWithoutAddLess.Text = Conversion.ToDouble(item.Cells["PurchaseRateWithoutAddLess"].Value).ToString("#,##0.###");
				txtPurchaseRateAddLess.Text = Conversion.ToDouble(item.Cells["PurchaseRateAddLess"].Value).ToString("#,##0.###");
				txtPurchaseNetRate.Text = Conversion.ToDouble(item.Cells["PurchaseNetRate"].Value).ToString("#,##0.###");
				if (Conversion.ToInt(grd.CurrentRow.Cells["PurchaseTaxNameId"].Value) > 0)
				{
					CmbTaxNamePurchase.Value = item.Cells["PurchaseTaxNameId"].Value.ToString();
				}
				txtTaxPercntPurchase.Text = Conversion.ToDouble(item.Cells["PurchaseTax%"].Value).ToString("#,##0.###");
				txtTaxAmountPurchase.Text = Conversion.ToDouble(item.Cells["PurchaseTaxAmount"].Value).ToString("#,##0.###");
				txtItemAmountPurchase.Text = Conversion.ToDouble(item.Cells["PurchaseAmount"].Value).ToString("#,##0.###");
				txtGrossWeightSale.Text = Conversion.ToDouble(item.Cells["SaleGrossWeight"].Value).ToString("#,##0.###");
				txtEBUnitSale.Text = Conversion.ToDouble(item.Cells["SaleEbCut"].Value).ToString("#,##0.###");
				txtEBTotalSale.Text = Conversion.ToDouble(item.Cells["SaleEbCutTotal"].Value).ToString("#,##0.###");
				txtAddlessWtSales.Text = Conversion.ToDouble(item.Cells["SaleAddLess"].Value).ToString("#,##0.###");
				txtBillWeightSale.Text = Conversion.ToDouble(item.Cells["SaleBillWeight"].Value).ToString("#,##0.###");
				CmbRateUomSale.Value = item.Cells["SaleRateUomId"].Value.ToString();
				txtSaleRateWithoutAddLess.Text = Conversion.ToDouble(item.Cells["SaleRateWithoutAddLess"].Value).ToString("#,##0.###");
				txtSaleRateAddLess.Text = Conversion.ToDouble(item.Cells["SaleRateAddLess"].Value).ToString("#,##0.###");
				txtSaleNetRate.Text = Conversion.ToDouble(item.Cells["SaleNetRate"].Value).ToString("#,##0.###");
				if (Conversion.ToInt(grd.CurrentRow.Cells["SaleTaxNameId"].Value) > 0)
				{
					CmbTaxNameSale.Value = item.Cells["SaleTaxNameId"].Value.ToString();
				}
				txtTaxPercntSale.Text = Conversion.ToDouble(item.Cells["SaleTax%"].Value).ToString("#,##0.###");
				txtTaxAmountSale.Text = Conversion.ToDouble(item.Cells["SaleTaxAmount"].Value).ToString("#,##0.###");
				txtItemAmountSale.Text = Conversion.ToDouble(item.Cells["SaleAmount"].Value).ToString("#,##0.###");
				txtDetailRemarks.Text = item.Cells["RemarksDetail"].Text.ToString();
				btnAddDetail.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				CmbItemName.Focus();
				ExpenseProportionForPurchase();
				ExpenseProportionForSale();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings(GridEX gridEX)
	{
		try
		{
			if (gridEX == null)
			{
				return;
			}
			TradeBill_Helper.DetailGridCommonSetting(gridEX);
			DataTable dt = (DataTable)gridEX.DataSource;
			bool hasGdn = false;
			if (dt != null)
			{
				hasGdn = dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["GdnId"]) > 0);
			}
			if (((Control)(object)gridEX).Name == "grd")
			{
				if (!hasGdn)
				{
					GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
					grd.FrozenColumns = 1;
					gridEX.RootTable.Columns["PurchaseAddLessWeight"].EditType = (EditType)1;
					gridEX.RootTable.Columns["SaleAddLess"].EditType = (EditType)1;
				}
				else
				{
					PanelDetailInfo.Visible = false;
					gridEX.RootTable.Columns["PurchaseAddLessWeight"].EditType = (EditType)1;
					gridEX.RootTable.Columns["SaleAddLess"].EditType = (EditType)(EnableSaleAddLessWeightOnBillFromGdn ? 1 : 0);
					gridEX.RootTable.Columns["RemarksDetail"].EditType = (EditType)1;
					gridEX.RootTable.Columns["RefDocNo"].EditType = (EditType)1;
				}
				if (!dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["PoId"]) > 0) && hasGdn)
				{
					gridEX.RootTable.Columns["PurchaseRateWithoutAddLess"].EditType = (EditType)1;
				}
				if (!dt.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["SoId"]) > 0) && hasGdn)
				{
					gridEX.RootTable.Columns["SaleRateWithoutAddLess"].EditType = (EditType)1;
				}
				ctrlGrdBar1_Load(null, null);
			}
			gridEX.RootTable.Columns["PurchaseRateAddLess"].EditType = (EditType)1;
			gridEX.RootTable.Columns["SaleRateAddLess"].EditType = (EditType)1;
			gridEX.RootTable.Columns["GdnNo"].Visible = hasGdn;
			gridEX.RootTable.Columns["GrnNo"].Visible = hasGdn;
			gridEX.RootTable.Columns["PoNo"].Visible = hasGdn;
			gridEX.RootTable.Columns["SoNo"].Visible = hasGdn;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			DetailFormReset();
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
			if (FormValidationDetail() && updateRowIndex >= 0 && updateRowIndex < dtDetail.Rows.Count)
			{
				DataRow dr = dtDetail.Rows[updateRowIndex];
				FillDetailRow(dr);
				grd.DataSource = dtDetail;
				((Control)(object)grd).Refresh();
				DetailFormReset();
				btnAddDetail.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				ExpenseProportionForPurchase();
				ExpenseProportionForSale();
				FreightProportion();
				InitCommissionConfigs();
				CalculateCommissionGrid(GetPurchaseCommissionConfig());
				CalculateCommissionGrid(GetPurchaseBrokeryConfig());
				CalculateCommissionGrid(GetSaleCommissionConfig());
				CalculateCommissionGrid(GetSaleBrokeryConfig());
				ProportionateCommissionAndBrokery();
				SupplierBillAmount();
				CustomerBillAmount();
				PLAmountProportion();
				CalculateHeaderRates();
			}
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
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null && e.Column.Key == "Delete")
			{
				DeleteDetailGridRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailGridRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				InvCommAgentTradeBillDetail detail = new InvCommAgentTradeBillDetail();
				detail.Id = Id;
				detail.ActionTypeId = 3;
				FillDetailListCommonForInsertAndDelete(detail, r);
				lstRemoveRecord.Add(detail);
				dtDetail.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			grd.UpdateData();
			PLAmountProportion();
			CalculateHeaderRates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				string col = e.Column.Key;
				if (col.StartsWith("Purchase"))
				{
					CalculateRowAmountsPurchase(r);
					grd.UpdateData();
					ExpenseProportionForPurchase();
					FreightProportion();
					InitCommissionConfigs();
					CalculateCommissionGrid(GetPurchaseCommissionConfig());
					CalculateCommissionGrid(GetPurchaseBrokeryConfig());
					ProportionateCommissionAndBrokery();
					SupplierBillAmount();
					PLAmountProportion();
					CalculateHeaderRates();
				}
				else if (col.StartsWith("Sale"))
				{
					CalculateRowAmountsSale(r);
					grd.UpdateData();
					ExpenseProportionForSale();
					InitCommissionConfigs();
					CalculateCommissionGrid(GetSaleCommissionConfig());
					CalculateCommissionGrid(GetSaleBrokeryConfig());
					ProportionateCommissionAndBrokery();
					CustomerBillAmount();
					PLAmountProportion();
					CalculateHeaderRates();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateRowAmountsPurchase(GridEXRow r)
	{
		decimal num = Conversion.ToDecimal(r.Cells["PurchaseGrossWeight"].Value);
		decimal ebTotal = Conversion.ToDecimal(r.Cells["PurchaseEbCutTotal"].Value);
		decimal addLess = Conversion.ToDecimal(r.Cells["PurchaseAddLessWeight"].Value);
		decimal num2 = Conversion.ToDecimal(r.Cells["PurchaseRateWithoutAddLess"].Value);
		decimal PurchaseRateAddLess = Conversion.ToDecimal(r.Cells["PurchaseRateAddLess"].Value);
		decimal PurchaseNetRate = num2 + PurchaseRateAddLess;
		r.Cells["PurchaseNetRate"].Value = PurchaseNetRate;
		decimal rateEq = Conversion.ToDecimal(r.Cells["PurchaseRateEquivalent"].Value);
		decimal freight = Conversion.ToDecimal(r.Cells["PurchaseFreight"].Value);
		decimal freightLess = Conversion.ToDecimal(r.Cells["PurchaseFreightLess"].Value);
		decimal expenses = Conversion.ToDecimal(r.Cells["PurchaseExpenses"].Value);
		decimal commission = Conversion.ToDecimal(r.Cells["PurchaseCommission"].Value);
		decimal brokery = Conversion.ToDecimal(r.Cells["PurchaseBrokery"].Value);
		decimal netWeight = num - ebTotal + addLess;
		r.Cells["PurchaseBillWeight"].Value = netWeight;
		decimal amount = ((rateEq > 0m) ? (netWeight / rateEq * PurchaseNetRate) : 0m);
		amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		r.Cells["PurchaseAmount"].Value = amount;
		decimal taxAmount = ApplyTax(r, "PurchaseAmount", "PurchaseTax%", "PurchaseTaxAmount", null);
		decimal netAmount = amount + taxAmount + freight - freightLess + expenses + commission + brokery;
		netAmount = Math.Round(netAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		r.Cells["PurchaseNetAmount"].Value = netAmount;
	}

	private void CalculateRowAmountsSale(GridEXRow r)
	{
		decimal num = Conversion.ToDecimal(r.Cells["SaleGrossWeight"].Value);
		decimal ebTotal = Conversion.ToDecimal(r.Cells["SaleEbCutTotal"].Value);
		decimal addLess = Conversion.ToDecimal(r.Cells["SaleAddLess"].Value);
		decimal num2 = Conversion.ToDecimal(r.Cells["SaleRateWithoutAddLess"].Value);
		decimal SaleRateAddLess = Conversion.ToDecimal(r.Cells["SaleRateAddLess"].Value);
		decimal SaleNetRate = num2 + SaleRateAddLess;
		r.Cells["SaleNetRate"].Value = SaleNetRate;
		decimal rateEq = Conversion.ToDecimal(r.Cells["SaleRateEquivalent"].Value);
		decimal expenses = Conversion.ToDecimal(r.Cells["SaleExpenses"].Value);
		decimal commission = Conversion.ToDecimal(r.Cells["SaleCommission"].Value);
		decimal brokery = Conversion.ToDecimal(r.Cells["SaleBrokery"].Value);
		decimal netWeight = num - ebTotal + addLess;
		r.Cells["SaleBillWeight"].Value = netWeight;
		decimal amount = ((rateEq > 0m) ? (netWeight / rateEq * SaleNetRate) : 0m);
		amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		r.Cells["SaleAmount"].Value = amount;
		decimal taxAmount = ApplyTax(r, "SaleAmount", "SaleTax%", "SaleTaxAmount", null);
		decimal netAmount = amount + taxAmount - expenses - commission - brokery;
		netAmount = Math.Round(netAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		r.Cells["SaleNetAmount"].Value = netAmount;
	}

	private decimal ApplyTax(GridEXRow row, string amountCol, string taxPctCol, string taxAmtCol, string totalCol)
	{
		decimal obj = ((row.Cells[amountCol].Value != DBNull.Value) ? Conversion.ToDecimal(row.Cells[amountCol].Value) : 0m);
		decimal taxPercent = ((row.Cells[taxPctCol].Value != DBNull.Value) ? Conversion.ToDecimal(row.Cells[taxPctCol].Value) : 0m);
		decimal taxAmount = obj * taxPercent / 100m;
		decimal totalAmount = obj + taxAmount;
		taxAmount = Math.Round(taxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		row.Cells[taxAmtCol].Value = taxAmount;
		if (!string.IsNullOrEmpty(totalCol))
		{
			row.Cells[totalCol].Value = Math.Round(totalAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		}
		return taxAmount;
	}

	private void RecalculatePurchase(GridEXRow r)
	{
		CalculateRowAmountsPurchase(r);
		grd.UpdateData();
		ExpenseProportionForPurchase();
		FreightProportion();
		InitCommissionConfigs();
		CalculateCommissionGrid(GetPurchaseCommissionConfig());
		CalculateCommissionGrid(GetPurchaseBrokeryConfig());
		ProportionateCommissionAndBrokery();
		SupplierBillAmount();
		PLAmountProportion();
		CalculateHeaderRates();
	}

	private void RecalculateSale(GridEXRow r)
	{
		CalculateRowAmountsSale(r);
		grd.UpdateData();
		ExpenseProportionForSale();
		InitCommissionConfigs();
		CalculateCommissionGrid(GetSaleCommissionConfig());
		CalculateCommissionGrid(GetSaleBrokeryConfig());
		ProportionateCommissionAndBrokery();
		CustomerBillAmount();
		PLAmountProportion();
		CalculateHeaderRates();
	}

	private decimal GetUomEquivalent(DataTable dt, int id)
	{
		DataRow row = dt.AsEnumerable().FirstOrDefault((DataRow x) => x.Field<int>("Id") == id);
		if (row == null)
		{
			return 0m;
		}
		return Conversion.ToDecimal(row["Equivalent"]);
	}

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			string col = grd.CurrentColumn.Key;
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (col == "Delete")
				{
					DeleteDetailGridRow(r);
				}
				else if (col == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailGridRow(r);
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				HandleF1Actions(r);
			}
			((Control)(object)grd).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HandleF1Actions(GridEXRow row)
	{
		string col = grd.CurrentColumn.Key;
		int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
		if (itemId == 0)
		{
			throw new Exception("Item not found in current row");
		}
		if (col.StartsWith("PurchaseRateUom"))
		{
			if (Conversion.ToInt(row.Cells["PoId"].Value) == 0 && Conversion.ToInt(row.Cells["GdnId"].Value) != 0)
			{
				DataTable dtUom = CommonServices.dtUomFromGloablUomScheduleByItemId(itemId);
				GrdPopUp obj = new GrdPopUp(dtUom, "Id", "UOMCode", "Equivalent");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				int id = Conversion.ToInt((object)obj.ReturnId);
				if (id > 0)
				{
					row.Cells["PurchaseRateUomId"].Value = id;
					row.Cells["PurchaseRateUom"].Value = obj.ReturnName;
					row.Cells["PurchaseRateEquivalent"].Value = GetUomEquivalent(dtUom, id);
					RecalculatePurchase(row);
				}
			}
		}
		else if (col.StartsWith("SaleRateUom"))
		{
			if (Conversion.ToInt(row.Cells["SoId"].Value) == 0 && Conversion.ToInt(row.Cells["GdnId"].Value) != 0)
			{
				DataTable dtUom2 = CommonServices.dtUomFromGloablUomScheduleByItemId(itemId);
				GrdPopUp obj2 = new GrdPopUp(dtUom2, "Id", "UOMCode", "Equivalent");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				int id2 = Conversion.ToInt((object)obj2.ReturnId);
				if (id2 > 0)
				{
					row.Cells["SaleRateUomId"].Value = id2;
					row.Cells["SaleRateUom"].Value = obj2.ReturnName;
					row.Cells["SaleRateEquivalent"].Value = GetUomEquivalent(dtUom2, id2);
					RecalculateSale(row);
				}
			}
		}
		else
		{
			if (!col.Contains("TaxName") || (Conversion.ToInt(row.Cells["PoId"].Value) != 0 && Conversion.ToInt(row.Cells["SoId"].Value) != 0) || Conversion.ToInt(row.Cells["GdnId"].Value) == 0)
			{
				return;
			}
			GrdPopUp obj3 = new GrdPopUp(DatatableHelper.TaxTypeDbCallByItemId(itemId, txtDocDate.Value), "Id", "TaxType", "TaxPrcnt");
			obj3.StartPosition = FormStartPosition.CenterScreen;
			obj3.ShowDialog();
			int id3 = Conversion.ToInt((object)obj3.ReturnId);
			string prefix = (col.StartsWith("Purchase") ? "Purchase" : "Sale");
			if (prefix == "Purchase")
			{
				if (Conversion.ToInt(row.Cells["PoId"].Value) != 0 || Conversion.ToInt(row.Cells["GdnId"].Value) == 0)
				{
					return;
				}
			}
			else if (Conversion.ToInt(row.Cells["SoId"].Value) != 0 || Conversion.ToInt(row.Cells["GdnId"].Value) == 0)
			{
				return;
			}
			if (id3 > 0)
			{
				row.Cells[prefix + "TaxNameId"].Value = id3;
				row.Cells[prefix + "TaxName"].Value = obj3.ReturnName;
				row.Cells[prefix + "Tax%"].Value = Conversion.ToDecimal((object)obj3.ReturnCode);
			}
			else
			{
				row.Cells[prefix + "TaxNameId"].Value = 0;
				row.Cells[prefix + "TaxName"].Value = "";
				row.Cells[prefix + "Tax%"].Value = 0;
			}
			if (prefix == "Purchase")
			{
				RecalculatePurchase(row);
			}
			else
			{
				RecalculateSale(row);
			}
		}
	}

	private void AddRowsInPurchaseExpenseGrid(DataTable dt)
	{
		try
		{
			foreach (DataRow sourceRow in dtOtherItem.Rows)
			{
				DataRow newRow = dt.NewRow();
				newRow["ItemId"] = sourceRow["Id"];
				newRow["ItemName"] = sourceRow["OtherItemName"];
				dt.Rows.Add(newRow);
			}
			if (dt.TableName == "dtPurchaseOtherExpense")
			{
				grdPurchaseOtherExpenses.DataSource = dt;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseOtherExpenses_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPurchaseOtherExpenses.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			double TotalAmount = GridEX_Helper.GetColumnSum(grd, "PurchaseNetAmount");
			if (e.Column.Key == "%OfTotal")
			{
				double prcnt = Conversion.ToDouble(item.Cells["%ofTotal"].Value);
				double usedPrcnt = GridEX_Helper.GetColumnSumWithoutIndex(grdPurchaseOtherExpenses, "%ofTotal", item.RowIndex);
				double remainingPrcnt = 100.0 - usedPrcnt;
				if (prcnt > remainingPrcnt)
				{
					MessageBox.Show($"%of Total Can't be Greater than Remaining: {remainingPrcnt}%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					prcnt = remainingPrcnt;
					item.Cells["%ofTotal"].Value = prcnt;
				}
				double amount = TotalAmount * prcnt / 100.0;
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Amount"].Value = Math.Round(amount, 4);
			}
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (Conversion.ToDouble(item.Cells["Qty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0)
				{
					double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
					double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
					item.Cells["Amount"].Value = Qty * Rate;
				}
				grdPurchaseOtherExpenses.UpdateData();
			}
			if (e.Column.Key == "Amount")
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				grdPurchaseOtherExpenses.UpdateData();
			}
			ExpenseProportionForPurchase();
			SupplierBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseOtherExpensesSettings(GridEX gridEX)
	{
		try
		{
			if (gridEX == null)
			{
				return;
			}
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>();
			if (((Control)(object)gridEX).Name == "grdPurchaseOtherExpenses")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"%OfTotal",
						(EditType)1
					},
					{
						"Qty",
						(EditType)1
					},
					{
						"Rate",
						(EditType)1
					},
					{
						"Amount",
						(EditType)1
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			TradeBill_Helper.PurchaseExpenseGridCommonSetting(gridEX, editableColumns);
			DataTable obj = (DataTable)gridEX.DataSource;
			bool hasGrn = false;
			foreach (DataRow row in obj.Rows)
			{
				if (Conversion.ToInt(row["GrnId"]) > 0)
				{
					hasGrn = true;
					break;
				}
			}
			gridEX.RootTable.Columns["GrnNo"].Visible = hasGrn;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPurchasePmExpenseGrid()
	{
		try
		{
			DataRow row = dtPurchasePmExpense.NewRow();
			dtPurchasePmExpense.Rows.Add(row);
			grdPurchasePmExpense.DataSource = dtPurchasePmExpense;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchasePmExpenseGridComboBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, new int[9] { 2, 11, 12, 13, 14, 15, 20, 21, 22 });
			GridEX_Helper.ComboBind(grdPurchasePmExpense.RootTable.Columns["AccountId"], dt, "Id", "AccountTitle", "Account");
			GridEX_Helper.ComboBind(grdPurchasePmExpense.RootTable.Columns["PackingTypeId"], dtEbGridPackingTypes, "Id", "Name", "Packing Type");
			GridEX_Helper.ComboBind(grdPurchasePmExpense.RootTable.Columns["PmItemId"], dtPmItemForPurchase, "ItemId", "ItemName", "Empty Bag Item");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchasePmExpenseSettings(GridEX gridEX)
	{
		try
		{
			if (gridEX == null)
			{
				return;
			}
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>();
			if (((Control)(object)gridEX).Name == "grdPurchasePmExpense")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"AccountId",
						(EditType)4
					},
					{
						"PackingTypeId",
						(EditType)4
					},
					{
						"PmItemId",
						(EditType)4
					},
					{
						"Qty",
						(EditType)1
					},
					{
						"Rate",
						(EditType)1
					},
					{
						"Amount",
						(EditType)1
					},
					{
						"Debit",
						(EditType)1
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			TradeBill_Helper.FreightGridCommonSetting(gridEX, editableColumns);
			if (((Control)(object)gridEX).Name == "grdPurchasePmExpense")
			{
				PurchasePmExpenseGridComboBind();
				GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
				GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
				gridEX.FrozenColumns = 2;
			}
			DataTable dt = gridEX.DataSource as DataTable;
			bool hasGrn = false;
			if (dt != null)
			{
				foreach (DataRow r in dt.Rows)
				{
					if (r["GrnId"] != DBNull.Value && (int)r["GrnId"] > 0)
					{
						hasGrn = true;
						break;
					}
				}
			}
			if (gridEX.RootTable.Columns.Contains("GrnNo"))
			{
				gridEX.RootTable.Columns["GrnNo"].Visible = hasGrn;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchasePmExpense_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPurchasePmExpense.UpdateData();
			GridEXRow item = grdPurchasePmExpense.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && Conversion.ToDouble(item.Cells["Qty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0)
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Amount"].Value = Qty * Rate;
			}
			if (e.Column.Key == "Amount")
			{
				if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					item.Cells["Amount"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
				else
				{
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			if (e.Column.Key == "Debit")
			{
				if (Conversion.ToDouble(item.Cells["Credit"].Value) > 0.0)
				{
					item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
			}
			else if (e.Column.Key == "AccountId")
			{
				bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
				if (Conversion.ToInt(CmbSupplier.Value) > 0 && isPurchaseDelivered)
				{
					int glId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString(CmbSupplier.Value)));
					if (Conversion.ToInt(item.Cells["AccountId"].Value) == glId)
					{
						item.Cells["AccountId"].Value = 0;
						MessageBox.Show("Freight Account And Supplier Gl Account Can't be Same", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
			}
			grdPurchasePmExpense.UpdateData();
			FreightProportion();
			ExpenseProportionForSale();
			SupplierBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchasePmExpense_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPurchasePmExpense.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeletePurchasePmExpenseRow(item);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInPurchasePmExpenseGrid();
				}
				grdPurchasePmExpense.UpdateData();
				FreightProportion();
				SupplierBillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletePurchasePmExpenseRow(GridEXRow item)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				dtPurchasePmExpense.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtPurchasePmExpense.Rows.Count == 0)
				{
					AddRowInPurchasePmExpenseGrid();
				}
				grdPurchasePmExpense.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbEmptyBagTermPurchase_Leave(object sender, EventArgs e)
	{
		try
		{
			AddEmptyBagTermInPurchaseSaleGrid(grdPurchasePmExpense);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowsInSaleExpenseGrid(DataTable dt)
	{
		try
		{
			foreach (DataRow sourceRow in dtOtherItem.Rows)
			{
				DataRow newRow = dt.NewRow();
				newRow["ItemId"] = sourceRow["Id"];
				newRow["ItemName"] = sourceRow["OtherItemName"];
				dt.Rows.Add(newRow);
			}
			if (dt.TableName == "dtSaleOtherExpense")
			{
				grdSaleOtherExpense.DataSource = dt;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleOtherExpense_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdSaleOtherExpense.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			double TotalAmount = GridEX_Helper.GetColumnSum(grd, "SaleNetAmount");
			if (e.Column.Key == "%OfTotal")
			{
				double prcnt = Conversion.ToDouble(item.Cells["%ofTotal"].Value);
				double usedPrcnt = GridEX_Helper.GetColumnSumWithoutIndex(grdSaleOtherExpense, "%ofTotal", item.RowIndex);
				double remainingPrcnt = 100.0 - usedPrcnt;
				if (prcnt > remainingPrcnt)
				{
					MessageBox.Show($"%of Total Can't be Greater than Remaining: {remainingPrcnt}%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					prcnt = remainingPrcnt;
					item.Cells["%ofTotal"].Value = prcnt;
				}
				double amount = TotalAmount * prcnt / 100.0;
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Amount"].Value = Math.Round(amount, 4);
			}
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && Conversion.ToDouble(item.Cells["Qty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0)
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Amount"].Value = Qty * Rate;
			}
			if (e.Column.Key == "Amount")
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdSaleOtherExpense.UpdateData();
			ExpenseProportionForSale();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleOtherExpenseSettings(GridEX gridEX)
	{
		try
		{
			if (gridEX == null)
			{
				return;
			}
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>();
			if (((Control)(object)gridEX).Name == "grdSaleOtherExpense")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"%OfTotal",
						(EditType)1
					},
					{
						"Qty",
						(EditType)1
					},
					{
						"Rate",
						(EditType)1
					},
					{
						"Amount",
						(EditType)1
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			TradeBill_Helper.SaleExpenseGridCommonSetting(gridEX, editableColumns);
			DataTable dt = (DataTable)gridEX.DataSource;
			bool hasGrn = false;
			if (dt != null)
			{
				foreach (DataRow row in dt.Rows)
				{
					if (Conversion.ToInt(row["GdnId"]) > 0)
					{
						hasGrn = true;
						break;
					}
				}
			}
			gridEX.RootTable.Columns["GdnNo"].Visible = hasGrn;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInSalePmExpenseGrid()
	{
		try
		{
			DataRow row = dtSalePmExpense.NewRow();
			dtSalePmExpense.Rows.Add(row);
			grdSalePmExpenseSale.DataSource = dtSalePmExpense;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PmGridCombBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds();
			GridEX_Helper.ComboBind(grdSalePmExpenseSale.RootTable.Columns["AccountId"], dt, "Id", "AccountTitle", "Account");
			GridEX_Helper.ComboBind(grdSalePmExpenseSale.RootTable.Columns["PackingTypeId"], dtEbGridPackingTypes, "Id", "Name", "Packing Type");
			GridEX_Helper.ComboBind(grdSalePmExpenseSale.RootTable.Columns["PmItemId"], dtPmItemForSale, "ItemId", "ItemName", "Empty Bag Item");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSalePmExpenseSettings(GridEX gridEX)
	{
		try
		{
			if (gridEX == null)
			{
				return;
			}
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>();
			if (((Control)(object)gridEX).Name == "grdSalePmExpenseSale")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"AccountId",
						(EditType)4
					},
					{
						"PackingTypeId",
						(EditType)4
					},
					{
						"PmItemId",
						(EditType)4
					},
					{
						"Qty",
						(EditType)1
					},
					{
						"Rate",
						(EditType)1
					},
					{
						"Amount",
						(EditType)1
					},
					{
						"Debit",
						(EditType)1
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			TradeBill_Helper.PackingMaterialGridCommonSetting(gridEX, editableColumns);
			if (((Control)(object)gridEX).Name == "grdSalePmExpenseSale")
			{
				PmGridCombBind();
				GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
				GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
				gridEX.FrozenColumns = 2;
			}
			DataTable dt = (DataTable)gridEX.DataSource;
			bool hasGrn = false;
			if (dt != null)
			{
				foreach (DataRow row in dt.Rows)
				{
					if (Conversion.ToInt(row["GdnId"]) > 0)
					{
						hasGrn = true;
						break;
					}
				}
			}
			gridEX.RootTable.Columns["GdnNo"].Visible = hasGrn;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSalePmExpense_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdSalePmExpenseSale.UpdateData();
			GridEXRow item = grdSalePmExpenseSale.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					item.Cells["Amount"].Value = Qty * Rate;
				}
				grdSalePmExpenseSale.UpdateData();
			}
			else if (e.Column.Key == "Amount")
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
				grdSalePmExpenseSale.UpdateData();
			}
			else if (e.Column.Key == "AccountId")
			{
				bool isSaleXFactory = FormHelper.DeliveryTermIsLoad(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text));
				if (item != null && (int)item.RowType == 0 && Conversion.ToInt(CmbCustomer.Value) > 0 && isSaleXFactory)
				{
					int glId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString(CmbCustomer.Value)));
					if (Conversion.ToInt(item.Cells["AccountId"].Value) == glId)
					{
						item.Cells["AccountId"].Value = 0;
						MessageBox.Show("PM Sale Expense Account And Customer Gl Account Can't be Same", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				grdSalePmExpenseSale.UpdateData();
			}
			ExpenseProportionForSale();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSalePmExpense_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdSalePmExpenseSale.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteSalePmExpenseRow(item);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInSalePmExpenseGrid();
				}
				grdSalePmExpenseSale.UpdateData();
				ExpenseProportionForSale();
				CustomerBillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbEmptyBagTerm_Leave(object sender, EventArgs e)
	{
		try
		{
			AddEmptyBagTermInPurchaseSaleGrid(grdSalePmExpenseSale);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteSalePmExpenseRow(GridEXRow item)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				dtSalePmExpense.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtSalePmExpense.Rows.Count == 0)
				{
					AddRowInSalePmExpenseGrid();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPurchasePaymentGrid()
	{
		try
		{
			dtPurchasePaymentDetail.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0);
			grdPaymentDetailPurchase.DataSource = dtPurchasePaymentDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPurchasePaymentComboBind()
	{
		try
		{
			DataTable dt = (DataTable)((UltraGridBase)CmbPaymentTermPurchase).DataSource;
			if (dt != null && dt.Rows.Count > 0)
			{
				GridEX_Helper.ComboBind(grdPaymentDetailPurchase.RootTable.Columns["PaymentTermId"], dt, "Id", "Description", "Payment Term", 100);
			}
			GridEX_Helper.ComboBind(grdPaymentDetailPurchase.RootTable.Columns["BaseDateTypeId"], dtBaseDateType, "Id", "Name", "Base Date Type", 100);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetailPurchaseSetting(GridEX gridEX)
	{
		try
		{
			if (gridEX != null)
			{
				Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
				{
					{
						"PaymentTermId",
						(EditType)4
					},
					{
						"DueDays",
						(EditType)1
					},
					{
						"%OfTotal",
						(EditType)1
					},
					{
						"BaseDateTypeId",
						(EditType)4
					},
					{
						"DueDate",
						(EditType)3
					}
				};
				TradeBill_Helper.PurchasePaymentGridCommonSetting(gridEX, editableColumns);
				if (((Control)(object)gridEX).Name == "grdPaymentDetailPurchase")
				{
					GridPurchasePaymentComboBind();
					GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					gridEX.FrozenColumns = 2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetailPurchase_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetailPurchase.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				int poId = Conversion.ToInt(item.Cells["PoId"].Value);
				if (poId > 0 && grdPaymentDetailPurchase.GetDataRows().Cast<GridEXRow>().Count((GridEXRow r) => Conversion.ToInt(r.Cells["PoId"].Value) == poId) <= 1)
				{
					MessageBox.Show("At least one payment row must exist per Purchase Order.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				dtPurchasePaymentDetail.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtPurchasePaymentDetail.Rows.Count == 0)
				{
					AddRowInPurchasePaymentGrid();
				}
			}
			else if (e.Column.Key == "Add")
			{
				AddRowInPurchasePaymentGrid();
			}
			grdPaymentDetailPurchase.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetailPurchase_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetailPurchase.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grd.GetRows().Count() > 0)
			{
				bool num = grd.GetDataRows().Cast<GridEXRow>().Any((GridEXRow r) => Conversion.ToInt(r.Cells["GrnId"].Value) > 0);
				decimal TotalAmount = Conversion.ToDecimal((object)txtPurchaseBillAmount.Text);
				if (num)
				{
					int poId = Conversion.ToInt(item.Cells["PoId"].Value);
					TotalAmount = (from r in dtDetail.AsEnumerable()
						where Conversion.ToInt(r["PoId"]) == poId
						select r).Sum((DataRow r) => Conversion.ToDecimal(r["PurchaseAmount"]));
				}
				int currentPoId = Conversion.ToInt(item.Cells["PoId"].Value);
				if (e.Column.Key == "%OfTotal")
				{
					decimal prcnt = Conversion.ToDecimal(item.Cells["%ofTotal"].Value);
					decimal usedPrcnt = (from GridEXRow r in grdPaymentDetailPurchase.GetDataRows()
						where r != item && Conversion.ToInt(r.Cells["PoId"].Value) == currentPoId
						select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["%ofTotal"].Value));
					decimal remainingPrcnt = 100m - usedPrcnt;
					if (prcnt > remainingPrcnt)
					{
						MessageBox.Show($"%of Total Can't be Greater than Remaining: {remainingPrcnt}%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						prcnt = remainingPrcnt;
						item.Cells["%ofTotal"].Value = prcnt;
					}
					decimal amount = TotalAmount * prcnt / 100m;
					item.Cells["Amount"].Value = Math.Round(amount, 4);
					grdPaymentDetailPurchase.UpdateData();
					CalculatePaymentDetailGrid(grdPaymentDetailPurchase);
				}
				else if (e.Column.Key == "Amount")
				{
					decimal amount2 = Conversion.ToDecimal(item.Cells["Amount"].Value);
					decimal usedAmount = (from GridEXRow r in grdPaymentDetailPurchase.GetDataRows()
						where r != item && Conversion.ToInt(r.Cells["PoId"].Value) == currentPoId
						select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Amount"].Value));
					decimal remainingAmount = TotalAmount - usedAmount;
					if (amount2 > remainingAmount)
					{
						MessageBox.Show($"Amount Can't be Greater than Remaining Amount: {remainingAmount}", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						amount2 = default(decimal);
						item.Cells["Amount"].Value = amount2;
					}
					decimal prcnt2 = ((TotalAmount > 0m) ? (amount2 * 100m / TotalAmount) : 0m);
					item.Cells["%ofTotal"].Value = Math.Round(prcnt2, 8);
				}
				else if (e.Column.Key == "DueDays")
				{
					int days = Conversion.ToInt(item.Cells["DueDays"].Value);
					item.Cells["DueDate"].Value = txtDocDate.Value.AddDays(days);
				}
				else if (e.Column.Key == "DueDate")
				{
					DateTime date = Conversion.ToDateTime(item.Cells["DueDate"].Value);
					if (date.Date < txtDocDate.Value.Date)
					{
						item.Cells["DueDate"].Value = txtDocDate.Value;
						throw new Exception("Due Date Can't be Less Than DocDate");
					}
					int days2 = (date.Date - txtDocDate.Value.Date).Days;
					item.Cells["DueDays"].Value = days2;
				}
			}
			grdPaymentDetailPurchase.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetailPurchase_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetailPurchase.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPaymentDetailPurchase.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space && grdPaymentDetailPurchase.CurrentColumn != null)
			{
				if (grdPaymentDetailPurchase.CurrentColumn.Key == "Delete")
				{
					int poId = Conversion.ToInt(item.Cells["PoId"].Value);
					if (poId > 0 && grdPaymentDetailPurchase.GetDataRows().Cast<GridEXRow>().Count((GridEXRow r) => Conversion.ToInt(r.Cells["PoId"].Value) == poId) <= 1)
					{
						MessageBox.Show("At least one payment row must exist per Purchase Order.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					dtPurchasePaymentDetail.Rows[item.RowIndex].Delete();
					item.Delete();
					if (dtPurchasePaymentDetail.Rows.Count == 0)
					{
						AddRowInPurchasePaymentGrid();
					}
				}
				else if (grdPaymentDetailPurchase.CurrentColumn.Key == "Add")
				{
					AddRowInPurchasePaymentGrid();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtPurchasePaymentDetail.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtPurchasePaymentDetail.Rows.Count == 0)
				{
					AddRowInPurchasePaymentGrid();
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPurchasePaymentGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInSalePaymentGrid()
	{
		try
		{
			dtSalePaymentDetail.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0);
			grdPaymentDetailSale.DataSource = dtSalePaymentDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSalePaymentComboBind()
	{
		try
		{
			DataTable dt = (DataTable)((UltraGridBase)CmbPaymentTermSale).DataSource;
			if (dt != null && dt.Rows.Count > 0)
			{
				GridEX_Helper.ComboBind(grdPaymentDetailSale.RootTable.Columns["PaymentTermId"], dt, "Id", "Description", "Payment Term", 100);
			}
			GridEX_Helper.ComboBind(grdPaymentDetailSale.RootTable.Columns["BaseDateTypeId"], dtBaseDateType, "Id", "Name", "Base Date Type", 100);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetailSaleSetting(GridEX gridEX)
	{
		try
		{
			if (gridEX != null)
			{
				Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
				{
					{
						"PaymentTermId",
						(EditType)4
					},
					{
						"DueDays",
						(EditType)1
					},
					{
						"%OfTotal",
						(EditType)1
					},
					{
						"BaseDateTypeId",
						(EditType)4
					},
					{
						"DueDate",
						(EditType)3
					}
				};
				TradeBill_Helper.SalePaymentGridCommonSetting(gridEX, editableColumns);
				if (((Control)(object)gridEX).Name == "grdPaymentDetailSale")
				{
					GridSalePaymentComboBind();
					GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					gridEX.FrozenColumns = 2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetailSale_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetailSale.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				int soId = Conversion.ToInt(item.Cells["SoId"].Value);
				if (soId > 0 && grdPaymentDetailSale.GetDataRows().Cast<GridEXRow>().Count((GridEXRow r) => Conversion.ToInt(r.Cells["SoId"].Value) == soId) <= 1)
				{
					MessageBox.Show("At least one payment row must exist per Sale Order.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				dtSalePaymentDetail.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtSalePaymentDetail.Rows.Count == 0)
				{
					AddRowInSalePaymentGrid();
				}
			}
			else if (e.Column.Key == "Add")
			{
				AddRowInSalePaymentGrid();
			}
			grdPaymentDetailSale.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetailSale_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetailSale.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grd.GetRows().Count() > 0)
			{
				bool num = grd.GetDataRows().Cast<GridEXRow>().Any((GridEXRow r) => Conversion.ToInt(r.Cells["GdnId"].Value) > 0);
				decimal TotalAmount = Conversion.ToDecimal((object)txtSaleBillAmount.Text);
				if (num)
				{
					int soId = Conversion.ToInt(item.Cells["SoId"].Value);
					TotalAmount = (from r in dtDetail.AsEnumerable()
						where Conversion.ToInt(r["SoId"]) == soId
						select r).Sum((DataRow r) => Conversion.ToDecimal(r["SaleAmount"]));
				}
				int currentSoId = Conversion.ToInt(item.Cells["SoId"].Value);
				if (e.Column.Key == "%OfTotal")
				{
					decimal prcnt = Conversion.ToDecimal(item.Cells["%ofTotal"].Value);
					decimal usedPrcnt = (from GridEXRow r in grdPaymentDetailSale.GetDataRows()
						where r != item && Conversion.ToInt(r.Cells["SoId"].Value) == currentSoId
						select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["%ofTotal"].Value));
					decimal remainingPrcnt = 100m - usedPrcnt;
					if (prcnt > remainingPrcnt)
					{
						MessageBox.Show($"%of Total Can't be Greater than Remaining: {remainingPrcnt}%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						prcnt = remainingPrcnt;
						item.Cells["%ofTotal"].Value = prcnt;
					}
					decimal amount = TotalAmount * prcnt / 100m;
					item.Cells["Amount"].Value = Math.Round(amount, 4);
					grdPaymentDetailSale.UpdateData();
					CalculatePaymentDetailGrid(grdPaymentDetailSale);
				}
				else if (e.Column.Key == "Amount")
				{
					decimal amount2 = Conversion.ToDecimal(item.Cells["Amount"].Value);
					decimal usedAmount = (from GridEXRow r in grdPaymentDetailSale.GetDataRows()
						where r != item && Conversion.ToInt(r.Cells["SoId"].Value) == currentSoId
						select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Amount"].Value));
					decimal remainingAmount = TotalAmount - usedAmount;
					if (amount2 > remainingAmount)
					{
						MessageBox.Show($"Amount Can't be Greater than Remaining Amount: {remainingAmount}", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						amount2 = default(decimal);
						item.Cells["Amount"].Value = amount2;
					}
					decimal prcnt2 = ((TotalAmount > 0m) ? (amount2 * 100m / TotalAmount) : 0m);
					item.Cells["%ofTotal"].Value = Math.Round(prcnt2, 8);
				}
				else if (e.Column.Key == "DueDays")
				{
					int days = Conversion.ToInt(item.Cells["DueDays"].Value);
					item.Cells["DueDate"].Value = txtDocDate.Value.AddDays(days);
				}
				else if (e.Column.Key == "DueDate")
				{
					DateTime date = Conversion.ToDateTime(item.Cells["DueDate"].Value);
					if (date.Date < txtDocDate.Value.Date)
					{
						item.Cells["DueDate"].Value = txtDocDate.Value;
						throw new Exception("Due Date Can't be Less Than DocDate");
					}
					int days2 = (date.Date - txtDocDate.Value.Date).Days;
					item.Cells["DueDays"].Value = days2;
				}
			}
			grdPaymentDetailSale.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetailSale_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetailSale.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPaymentDetailSale.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space && grdPaymentDetailSale.CurrentColumn != null)
			{
				if (grdPaymentDetailSale.CurrentColumn.Key == "Delete")
				{
					int soId = Conversion.ToInt(item.Cells["SoId"].Value);
					if (soId > 0 && grdPaymentDetailSale.GetDataRows().Cast<GridEXRow>().Count((GridEXRow r) => Conversion.ToInt(r.Cells["SoId"].Value) == soId) <= 1)
					{
						MessageBox.Show("At least one payment row must exist per Sale Order.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					dtSalePaymentDetail.Rows[item.RowIndex].Delete();
					item.Delete();
					if (dtSalePaymentDetail.Rows.Count == 0)
					{
						AddRowInSalePaymentGrid();
					}
				}
				else if (grdPaymentDetailSale.CurrentColumn.Key == "Add")
				{
					AddRowInSalePaymentGrid();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtSalePaymentDetail.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtSalePaymentDetail.Rows.Count == 0)
				{
					AddRowInSalePaymentGrid();
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInSalePaymentGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPurchaseCommissionGrid()
	{
		try
		{
			dtPurchaseCommission.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0);
			grdPurchaseCommission.DataSource = dtPurchaseCommission;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPurchaseCommissionComboBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, new int[3] { 2, 4, 15 }, null, new int[4] { 2, 3, 4, 5 });
			GridEX_Helper.ComboBind(grdPurchaseCommission.RootTable.Columns["CommissionAgentId"], dtSupplier, "Id", "CompanyName", "Commission Agent");
			GridEX_Helper.ComboBind(grdPurchaseCommission.RootTable.Columns["CommissionTypeId"], dtCommType, "Id", "Name", "Commission Type");
			GridEX_Helper.ComboBind(grdPurchaseCommission.RootTable.Columns["CommissionUomId"], dtCommUom, "Id", "Name", "Commission Uom");
			GridEX_Helper.ComboBind(grdPurchaseCommission.RootTable.Columns["DebitAccountId"], dt, "Id", "AccountTitle", "Commission Dr Account");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseCommissionDetailSetting(GridEX gridEX)
	{
		try
		{
			if (gridEX != null)
			{
				Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
				{
					{
						"CommissionAgentId",
						(EditType)4
					},
					{
						"CommissionTypeId",
						(EditType)4
					},
					{
						"CommissionUomId",
						(EditType)4
					},
					{
						"CommissionRate",
						(EditType)1
					},
					{
						"DebitAccountId",
						(EditType)4
					}
				};
				TradeBill_Helper.PurchaseCommissionGridCommonSetting(gridEX, editableColumns);
				if (((Control)(object)gridEX).Name == "grdPurchaseCommission")
				{
					GridPurchaseCommissionComboBind();
					GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					gridEX.FrozenColumns = 2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseCommission_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			HandleCommissionGridCellUpdated(GetPurchaseCommissionConfig(), e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseCommission_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPurchaseCommission.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["PoId"].Value) > 0)
				{
					MessageBox.Show("You Can not Delete Row Because The Entry Is From Purcahse Order");
					return;
				}
				dtPurchaseCommission.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtPurchaseCommission.Rows.Count == 0)
				{
					AddRowInPurchaseCommissionGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPurchaseCommissionGrid();
			}
			grdPurchaseCommission.UpdateData();
			ProportionateCommissionAndBrokery();
			SupplierBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseCommission_Click(object sender, EventArgs e)
	{
		try
		{
			PurchaseSaleCommissionGridsColumnsEdit(grdPurchaseCommission, "PoId");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseCommission_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			PurchaseSaleCommissionGridsColumnsEdit(grdPurchaseCommission, "PoId");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseCommission_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPurchaseCommission.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || !e.Control || e.KeyCode != Keys.Space || grdPurchaseCommission.CurrentColumn == null)
			{
				return;
			}
			if (grdPurchaseCommission.CurrentColumn.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["PoId"].Value) > 0)
				{
					MessageBox.Show("You Can not Delete Row Because The Entry Is From Purchase Order");
					return;
				}
				dtPurchaseCommission.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtPurchaseCommission.Rows.Count == 0)
				{
					AddRowInPurchaseCommissionGrid();
				}
				grdPurchaseCommission.UpdateData();
			}
			if (grdPurchaseCommission.CurrentColumn.Key == "Add")
			{
				AddRowInPurchaseCommissionGrid();
				grdPurchaseCommission.UpdateData();
			}
			ProportionateCommissionAndBrokery();
			SupplierBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInSaleCommissionGrid()
	{
		try
		{
			dtSaleCommission.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0);
			grdSaleCommission.DataSource = dtSaleCommission;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSaleCommissionComboBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, new int[3] { 2, 4, 15 }, null, new int[4] { 2, 3, 4, 5 });
			GridEX_Helper.ComboBind(grdSaleCommission.RootTable.Columns["CommissionAgentId"], dtSupplier, "Id", "CompanyName", "Commission Agent");
			GridEX_Helper.ComboBind(grdSaleCommission.RootTable.Columns["CommissionTypeId"], dtCommType, "Id", "Name", "Commission Type");
			GridEX_Helper.ComboBind(grdSaleCommission.RootTable.Columns["CommissionUomId"], dtCommUom, "Id", "Name", "Commission Uom");
			GridEX_Helper.ComboBind(grdSaleCommission.RootTable.Columns["DebitAccountId"], dt, "Id", "AccountTitle", "Commission Dr Account");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleCommissionDetailSetting(GridEX gridEX)
	{
		try
		{
			if (gridEX != null)
			{
				Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
				{
					{
						"CommissionAgentId",
						(EditType)4
					},
					{
						"CommissionTypeId",
						(EditType)4
					},
					{
						"CommissionUomId",
						(EditType)4
					},
					{
						"CommissionRate",
						(EditType)1
					},
					{
						"DebitAccountId",
						(EditType)4
					}
				};
				TradeBill_Helper.SaleCommissionGridCommonSetting(gridEX, editableColumns);
				if (((Control)(object)gridEX).Name == "grdSaleCommission")
				{
					GridSaleCommissionComboBind();
					GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					gridEX.FrozenColumns = 2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleCommission_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			HandleCommissionGridCellUpdated(GetSaleCommissionConfig(), e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleCommission_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSaleCommission.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["SoId"].Value) > 0)
				{
					MessageBox.Show("You Can not Delete Row Because The Entry Is From Sale Order");
					return;
				}
				dtSaleCommission.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtSaleCommission.Rows.Count == 0)
				{
					AddRowInSaleCommissionGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInSaleCommissionGrid();
			}
			grdSaleCommission.UpdateData();
			ProportionateCommissionAndBrokery();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleCommission_Click(object sender, EventArgs e)
	{
		try
		{
			PurchaseSaleCommissionGridsColumnsEdit(grdSaleCommission, "SoId");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleCommission_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			PurchaseSaleCommissionGridsColumnsEdit(grdSaleCommission, "SoId");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleCommission_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSaleCommission.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || !e.Control || e.KeyCode != Keys.Space || grdSaleCommission.CurrentColumn == null)
			{
				return;
			}
			if (grdSaleCommission.CurrentColumn.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["SoId"].Value) > 0)
				{
					MessageBox.Show("You Can not Delete Row Because The Entry Is From Sale Order");
					return;
				}
				dtSaleCommission.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtSaleCommission.Rows.Count == 0)
				{
					AddRowInSaleCommissionGrid();
				}
				grdSaleCommission.UpdateData();
			}
			if (grdSaleCommission.CurrentColumn.Key == "Add")
			{
				AddRowInSaleCommissionGrid();
				grdSaleCommission.UpdateData();
			}
			ProportionateCommissionAndBrokery();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPurchaseBrokeryGrid()
	{
		try
		{
			dtPurchaseBrokery.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0);
			grdPurchaseBrokery.DataSource = dtPurchaseBrokery;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPurchaseBrokeryComboBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, new int[3] { 2, 4, 15 }, null, new int[4] { 2, 3, 4, 5 });
			GridEX_Helper.ComboBind(grdPurchaseBrokery.RootTable.Columns["BrokeryAgentId"], dtSupplier, "Id", "CompanyName", "Brorker Agent");
			GridEX_Helper.ComboBind(grdPurchaseBrokery.RootTable.Columns["BrokeryTypeId"], dtCommType, "Id", "Name", "Brokery Type");
			GridEX_Helper.ComboBind(grdPurchaseBrokery.RootTable.Columns["BrokeryUomId"], dtCommUom, "Id", "Name", "Brokery Uom");
			GridEX_Helper.ComboBind(grdPurchaseBrokery.RootTable.Columns["DebitAccountId"], dt, "Id", "AccountTitle", "Broker Dr Account");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrokeryDetailSetting(GridEX gridEX)
	{
		try
		{
			if (gridEX != null)
			{
				Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
				{
					{
						"BrokeryAgentId",
						(EditType)4
					},
					{
						"BrokeryTypeId",
						(EditType)4
					},
					{
						"BrokeryUomId",
						(EditType)4
					},
					{
						"BrokeryRate",
						(EditType)1
					},
					{
						"DebitAccountId",
						(EditType)4
					}
				};
				TradeBill_Helper.PurchaseBrokeryGridCommonSetting(gridEX, editableColumns);
				if (((Control)(object)gridEX).Name == "grdPurchaseBrokery")
				{
					GridPurchaseBrokeryComboBind();
					GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					gridEX.FrozenColumns = 2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrokery_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			HandleCommissionGridCellUpdated(GetPurchaseBrokeryConfig(), e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrokery_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPurchaseBrokery.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["PoId"].Value) > 0)
				{
					MessageBox.Show("You Can not Delete Row Because The Entry Is From Purcahse Order");
					return;
				}
				dtPurchaseBrokery.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtPurchaseBrokery.Rows.Count == 0)
				{
					AddRowInPurchaseBrokeryGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPurchaseBrokeryGrid();
			}
			grdPurchaseBrokery.UpdateData();
			ProportionateCommissionAndBrokery();
			SupplierBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrokery_Click(object sender, EventArgs e)
	{
		try
		{
			PurchaseSaleBrokeryGridsColumnsEdit(grdPurchaseBrokery, "PoId");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrokery_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			PurchaseSaleBrokeryGridsColumnsEdit(grdPurchaseBrokery, "PoId");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrokery_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPurchaseBrokery.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || !e.Control || e.KeyCode != Keys.Space || grdPurchaseBrokery.CurrentColumn == null)
			{
				return;
			}
			if (grdPurchaseBrokery.CurrentColumn.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["PoId"].Value) > 0)
				{
					MessageBox.Show("You Can not Delete Row Because The Entry Is From Purchase Order");
					return;
				}
				dtPurchaseBrokery.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtPurchaseBrokery.Rows.Count == 0)
				{
					AddRowInPurchaseBrokeryGrid();
				}
				grdPurchaseBrokery.UpdateData();
			}
			if (grdPurchaseBrokery.CurrentColumn.Key == "Add")
			{
				AddRowInPurchaseBrokeryGrid();
				grdPurchaseBrokery.UpdateData();
			}
			ProportionateCommissionAndBrokery();
			SupplierBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInSaleBrokeryGrid()
	{
		try
		{
			dtSaleBrokery.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0);
			grdSaleBrokery.DataSource = dtSaleBrokery;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSaleBrokeryComboBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, new int[3] { 2, 4, 15 }, null, new int[4] { 2, 3, 4, 5 });
			GridEX_Helper.ComboBind(grdSaleBrokery.RootTable.Columns["BrokeryAgentId"], dtSupplier, "Id", "CompanyName", "Brorker Agent");
			GridEX_Helper.ComboBind(grdSaleBrokery.RootTable.Columns["BrokeryTypeId"], dtCommType, "Id", "Name", "Brokery Type");
			GridEX_Helper.ComboBind(grdSaleBrokery.RootTable.Columns["BrokeryUomId"], dtCommUom, "Id", "Name", "Brokery Uom");
			GridEX_Helper.ComboBind(grdSaleBrokery.RootTable.Columns["DebitAccountId"], dt, "Id", "AccountTitle", "Broker Dr Account");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleBrokeryDetailSetting(GridEX gridEX)
	{
		try
		{
			if (gridEX != null)
			{
				Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
				{
					{
						"BrokeryAgentId",
						(EditType)4
					},
					{
						"BrokeryTypeId",
						(EditType)4
					},
					{
						"BrokeryUomId",
						(EditType)4
					},
					{
						"BrokeryRate",
						(EditType)1
					},
					{
						"DebitAccountId",
						(EditType)4
					}
				};
				TradeBill_Helper.SaleBrokeryGridCommonSetting(gridEX, editableColumns);
				if (((Control)(object)gridEX).Name == "grdSaleBrokery")
				{
					GridSaleBrokeryComboBind();
					GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 0);
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					gridEX.FrozenColumns = 2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleBrokery_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			HandleCommissionGridCellUpdated(GetSaleBrokeryConfig(), e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleBrokery_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSaleBrokery.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["SoId"].Value) > 0)
				{
					MessageBox.Show("You Can not Delete Row Because The Entry Is From Sale Order");
					return;
				}
				dtSaleBrokery.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtSaleBrokery.Rows.Count == 0)
				{
					AddRowInSaleBrokeryGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInSaleBrokeryGrid();
			}
			grdSaleBrokery.UpdateData();
			ProportionateCommissionAndBrokery();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleBrokery_Click(object sender, EventArgs e)
	{
		try
		{
			PurchaseSaleBrokeryGridsColumnsEdit(grdSaleBrokery, "SoId");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleBrokery_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			PurchaseSaleBrokeryGridsColumnsEdit(grdSaleBrokery, "SoId");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleBrokery_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSaleBrokery.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || !e.Control || e.KeyCode != Keys.Space || grdSaleBrokery.CurrentColumn == null)
			{
				return;
			}
			if (grdSaleBrokery.CurrentColumn.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["SoId"].Value) > 0)
				{
					MessageBox.Show("You Can not Delete Row Because The Entry Is From Sale Order");
					return;
				}
				dtSaleBrokery.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtSaleBrokery.Rows.Count == 0)
				{
					AddRowInSaleBrokeryGrid();
				}
				grdSaleBrokery.UpdateData();
			}
			if (grdSaleBrokery.CurrentColumn.Key == "Add")
			{
				AddRowInSaleBrokeryGrid();
				grdSaleBrokery.UpdateData();
			}
			ProportionateCommissionAndBrokery();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvCommAgentTradeBillDetail d, GridEXRow r)
	{
		try
		{
			d.gdnBuyerDispatchMasterId = Conversion.ToInt(r.Cells["GdnId"].Value);
			d.gdnBuyerDispatchDetailId = Conversion.ToInt(r.Cells["GdnDetailId"].Value);
			d.grnSupplierLoadingMasterId = Conversion.ToInt(r.Cells["GrnId"].Value);
			d.grnSupplierLoadingDetailId = Conversion.ToInt(r.Cells["GrnDetailId"].Value);
			d.purchaseOrderMasterId = Conversion.ToInt(r.Cells["PoId"].Value);
			d.purchaseOrderDetailId = Conversion.ToInt(r.Cells["PoDetailId"].Value);
			d.saleOrderMasterId = Conversion.ToInt(r.Cells["SoId"].Value);
			d.saleOrderDetailId = Conversion.ToInt(r.Cells["SoDetailId"].Value);
			d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			d.ItemName = Conversion.ToString((object)r.Cells["ItemName"].Text);
			d.Crop = Conversion.ToInt(r.Cells["CropYearId"].Value);
			d.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
			d.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
			d.PackTypeDesc = Conversion.ToString((object)r.Cells["PackTypeDesc"].Text);
			d.PackUomId = Conversion.ToInt(r.Cells["UomId"].Value);
			d.PackUom = Conversion.ToString((object)r.Cells["UomCode"].Text);
			d.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
			d.RefDocNo = Conversion.ToString(r.Cells["RefDocNo"].Value);
			d.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
			d.BiltyNo = Conversion.ToString(r.Cells["BiltyNo"].Value);
			d.GrossWeight = Conversion.ToDecimal(r.Cells["PurchaseGrossWeight"].Value);
			d.EbCut = Conversion.ToDouble(r.Cells["PurchaseEbCut"].Value);
			d.EbCutTotal = Conversion.ToDouble(r.Cells["PurchaseEbCutTotal"].Value);
			d.AddLessPurchase = Conversion.ToDecimal(r.Cells["PurchaseAddLessWeight"].Value);
			d.BillWeight = Conversion.ToDecimal(r.Cells["PurchaseBillWeight"].Value);
			d.PurchaseRateWithoutAddLess = Conversion.ToDecimal(r.Cells["PurchaseRateWithoutAddLess"].Value);
			d.PurchaseRateAddLess = Conversion.ToDecimal(r.Cells["PurchaseRateAddLess"].Value);
			d.RatePurchase = Conversion.ToDecimal(r.Cells["PurchaseNetRate"].Value);
			d.RateUomId = Conversion.ToInt(r.Cells["PurchaseRateUomId"].Value);
			d.ItemAmount = Conversion.ToDouble(r.Cells["PurchaseAmount"].Value);
			d.PurchaseExpenses = Conversion.ToDouble(r.Cells["PurchaseExpenses"].Value);
			d.PurchaseFreight = Conversion.ToDecimal(r.Cells["PurchaseFreight"].Value);
			d.PurchaseBrokery = Conversion.ToDecimal(r.Cells["PurchaseBrokery"].Value);
			d.PurchaseCommission = Conversion.ToDecimal(r.Cells["PurchaseCommission"].Value);
			d.PurchaseFreightLess = Conversion.ToDouble(r.Cells["PurchaseFreightLess"].Value);
			d.TaxNameIdPurchase = Conversion.ToInt(r.Cells["PurchaseTaxNameId"].Value);
			d.TaxNamePurchase = Conversion.ToString(r.Cells["PurchaseTaxName"].Value);
			d.TaxPercentPurchase = Conversion.ToDecimal(r.Cells["PurchaseTax%"].Value);
			d.TaxAmountPurchase = Conversion.ToDecimal(r.Cells["PurchaseTaxAmount"].Value);
			d.PurchaseNetAmount = Conversion.ToDouble(r.Cells["PurchaseNetAmount"].Value);
			d.SaleGrossWeight = Conversion.ToDecimal(r.Cells["SaleGrossWeight"].Value);
			d.SaleEbCut = Conversion.ToDouble(r.Cells["SaleEbCut"].Value);
			d.SaleEbCutTotal = Conversion.ToDouble(r.Cells["SaleEbCutTotal"].Value);
			d.AddLessSale = Conversion.ToDecimal(r.Cells["SaleAddLess"].Value);
			d.SaleBillWeight = Conversion.ToDecimal(r.Cells["SaleBillWeight"].Value);
			d.SaleRateWithoutAddLess = Conversion.ToDecimal(r.Cells["SaleRateWithoutAddLess"].Value);
			d.SaleRateAddLess = Conversion.ToDecimal(r.Cells["SaleRateAddLess"].Value);
			d.RateSale = Conversion.ToDecimal(r.Cells["SaleNetRate"].Value);
			d.SaleRateUomId = Conversion.ToInt(r.Cells["SaleRateUomId"].Value);
			d.SaleAmount = Conversion.ToDouble(r.Cells["SaleAmount"].Value);
			d.SaleExpenses = Conversion.ToDouble(r.Cells["SaleExpenses"].Value);
			d.SaleCommission = Conversion.ToDecimal(r.Cells["SaleCommission"].Value);
			d.SaleBrokery = Conversion.ToDecimal(r.Cells["SaleBrokery"].Value);
			d.TaxNameIdSale = Conversion.ToInt(r.Cells["SaleTaxNameId"].Value);
			d.TaxNameSale = Conversion.ToString(r.Cells["SaleTaxName"].Value);
			d.TaxPercentSale = Conversion.ToDecimal(r.Cells["SaleTax%"].Value);
			d.TaxAmountSale = Conversion.ToDecimal(r.Cells["SaleTaxAmount"].Value);
			d.SaleNetAmount = Conversion.ToDouble(r.Cells["SaleNetAmount"].Value);
			d.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text.Trim());
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddCommissionOrBrokery(InvCommAgentTradeBill obj, GridEX grid, string agentIdColumn, string amountColumn, string commissionTypeIdColumn, string uomIdColumn, string rateColumn, int agentTypeId, int entrySideId, string orderIdColumn)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow item in dataRows)
		{
			decimal commAmount = Conversion.ToDecimal(item.Cells[amountColumn].Value);
			int agentId = Conversion.ToInt(item.Cells[agentIdColumn].Value);
			string agentText = Conversion.ToString((object)item.Cells[agentIdColumn].Text);
			if (agentId > 0 && !(commAmount <= 0m))
			{
				CommisionAgentBillCommissionDetail comm = new CommisionAgentBillCommissionDetail();
				if (entrySideId == 1)
				{
					comm.purchaseOrderMasterId = Conversion.ToInt(item.Cells[orderIdColumn].Value);
				}
				else
				{
					comm.saleOrderMasterId = Conversion.ToInt(item.Cells[orderIdColumn].Value);
				}
				comm.commissionAgentId = agentId;
				comm.CommissionAgentName = agentText;
				comm.commissionTypeId = Conversion.ToInt(item.Cells[commissionTypeIdColumn].Value);
				comm.CommissionType = Conversion.ToString((object)item.Cells[commissionTypeIdColumn].Text);
				if (comm.commissionTypeId == 0)
				{
					throw new Exception("Commission Type not found");
				}
				comm.rateUomId = Conversion.ToInt(item.Cells[uomIdColumn].Value);
				comm.RateUom = Conversion.ToInt((object)item.Cells[uomIdColumn].Text);
				comm.commissionRate = Conversion.ToDecimal(item.Cells[rateColumn].Value);
				if (comm.commissionRate == 0m)
				{
					throw new Exception("Rate not found");
				}
				comm.commissionAmount = commAmount;
				comm.debitAccountId = Conversion.ToInt(item.Cells["DebitAccountId"].Value);
				comm.AccountTitle = Conversion.ToString((object)item.Cells["DebitAccountId"].Text);
				comm.agentTypeId = agentTypeId;
				comm.EntrySideId = entrySideId;
				string agentLabel = ((agentTypeId == 1) ? "Agent" : "Broker");
				List<string> parts = new List<string> { agentLabel + ":" + agentText };
				parts.Add("CommissionType:" + comm.CommissionType);
				if (comm.commissionTypeId == 3)
				{
					parts.Add($"UOM:{comm.RateUom}");
				}
				parts.Add($"Rate:{comm.commissionRate}");
				parts.Add($"Amount:{comm.commissionAmount}");
				if (comm.debitAccountId > 0)
				{
					parts.Add("Debit Account:" + comm.AccountTitle);
				}
				comm.commissionRemarks = "[" + string.Join(", ", parts) + "]";
				obj.CommisionAgentBillCommissionDetailList.Add(comm);
			}
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ValidateFreight(decimal net, decimal amount, decimal addLess, int accId, string type)
	{
		if (net > 0m || amount > 0m)
		{
			if (accId <= 0)
			{
				throw new Exception(type + " Freight Account is required.");
			}
			if (amount < 0m)
			{
				throw new Exception(type + " Freight Amount cannot be negative.");
			}
			if (Math.Abs(addLess) > amount)
			{
				throw new Exception(type + " Freight Add/Less is not logical.");
			}
			if (Math.Abs(net - (amount + addLess)) > 0.01m)
			{
				throw new Exception(type + " Freight Net Amount mismatch.");
			}
			if (net == 0m)
			{
				throw new Exception(type + " Freight Net Amount cannot be zero.");
			}
		}
	}

	private void ValidateTax(decimal percent, decimal amount, int accId, int taxTypeId, string side)
	{
		if (percent > 0m || amount > 0m || taxTypeId > 0)
		{
			if (amount <= 0m)
			{
				throw new Exception(side + " Tax amount must be greater than zero.");
			}
			if (percent <= 0m)
			{
				throw new Exception(side + " Tax percent must be greater than zero.");
			}
			if (accId <= 0)
			{
				throw new Exception(side + " Tax account is required.");
			}
			if (taxTypeId <= 0)
			{
				throw new Exception(side + " Tax type is required.");
			}
		}
	}

	private void Insert()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Expected O, but got Unknown
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Expected O, but got Unknown
		//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Expected O, but got Unknown
		//IL_0b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b58: Expected O, but got Unknown
		//IL_1041: Unknown result type (might be due to invalid IL or missing references)
		//IL_1048: Expected O, but got Unknown
		//IL_1347: Unknown result type (might be due to invalid IL or missing references)
		//IL_134e: Expected O, but got Unknown
		//IL_1695: Unknown result type (might be due to invalid IL or missing references)
		//IL_169c: Expected O, but got Unknown
		//IL_1c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7f: Expected O, but got Unknown
		//IL_2764: Unknown result type (might be due to invalid IL or missing references)
		//IL_276b: Expected O, but got Unknown
		//IL_247b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2482: Expected O, but got Unknown
		//IL_2d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7f: Expected O, but got Unknown
		//IL_2a6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a74: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
			}
			else
			{
				if (!FormValidation())
				{
					return;
				}
				CommissionAgentAccoutValidation();
				if (!FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				InvCommAgentTradeBill obj = new InvCommAgentTradeBill();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.BranchesId = UserAccount.BranchesId;
				obj.ProjectsId = UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.DocumentTypeId = DocumentTypeId;
				obj.Id = RecId;
				obj.EnteryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.ApprovedDate = DateTime.Now;
				obj.EnteryUserId = UserAccount.ID;
				obj.ModifyUserId = UserAccount.ID;
				obj.ApprovalUserId = UserAccount.ID;
				obj.IsApproved = false;
				obj.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
				obj.DocNo = Convert.ToInt32(TxtDocNo.Text.Trim());
				obj.BranchSrNo = Convert.ToInt32(txtBranchSrNo.Text.Trim());
				obj.TradingGlAccountId = Conversion.ToInt(CmbTradingAccount.Value);
				obj.RemarksHeader = txtMainRemarks.Text.Trim();
				obj.SupplierId = Conversion.ToInt(CmbSupplier.Value);
				obj.DeliveryTermPurchaseId = Conversion.ToInt(CmbDeliveryTermPurchase.Value);
				obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text);
				obj.PaymentTermId = Conversion.ToInt(CmbPaymentTermPurchase.Value);
				obj.DueDays = Conversion.ToInt((object)txtDueDaysPurchase.Text);
				obj.DueDate = Conversion.ToDateTime((object)txtDueDatePurchase.Value);
				obj.PurchaseTaxAccountId = Conversion.ToInt(CmbTaxAccountPurchase.Value);
				obj.PurchaseBillAmount = Conversion.ToDouble((object)txtPurchaseBillAmount.Text);
				obj.CustomerId = Conversion.ToInt(CmbCustomer.Value);
				obj.DeliveryTermSaleId = Conversion.ToInt(CmbDeliveryTermSale.Value);
				obj.DeliveryTermSale = Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text);
				obj.SalePaymentTermId = Conversion.ToInt(CmbPaymentTermSale.Value);
				obj.SaleDueDays = Conversion.ToInt((object)txtDueDaysSale.Text);
				obj.SaleDueDate = Conversion.ToDateTime((object)txtDueDateSale.Value);
				obj.SaleTaxAccountId = Conversion.ToInt(CmbTaxAccountSale.Value);
				obj.SaleBillAmount = Conversion.ToDouble((object)txtSaleBillAmount.Text);
				obj.SupplierFirstWeight = Conversion.ToDecimal((object)txtSupplierFirstWeight.Text);
				obj.SupplierSecondWeight = Conversion.ToDecimal((object)txtSupplierSecondWeight.Text);
				obj.SupplierNetWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text);
				obj.SupplierBillWeight = Conversion.ToDecimal((object)txtBillWeightGrn.Text);
				obj.BuyerBillWeight = Conversion.ToDecimal((object)txtBillWeightGdn.Text);
				obj.BuyerFirstWeight = Conversion.ToDecimal((object)txtBuyerFirstWeight.Text);
				obj.BuyerSecondWeight = Conversion.ToDecimal((object)txtBuyerSecondWeight.Text);
				obj.BuyerNetWeight = Conversion.ToDecimal((object)txtBuyerWeight.Text);
				obj.PLAmount = Conversion.ToDouble((object)txtProfitLoss.Text);
				obj.ScreenName = base.Name;
				bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
				bool isSaleDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text));
				bool isSaleXFactory = FormHelper.DeliveryTermIsLoad(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text));
				decimal purchaseNet = Conversion.ToDecimal((object)txtTotalFreightPurchase.Text);
				decimal purchaseAmount = Conversion.ToDecimal((object)txtFreightAmountPurchase.Text);
				decimal purchaseAddLess = Conversion.ToDecimal((object)txtFreightAddLessPurchase.Text);
				int purchaseAccId = Conversion.ToInt(CmbFreightAccountPurchase.Value);
				ValidateFreight(purchaseNet, purchaseAmount, purchaseAddLess, purchaseAccId, "Purchase");
				decimal saleNet = Conversion.ToDecimal((object)txtTotalFreightSale.Text);
				decimal saleAmount = Conversion.ToDecimal((object)txtFreightAmountSale.Text);
				decimal saleAddLess = Conversion.ToDecimal((object)txtFreightAddLessSale.Text);
				int saleAccId = Conversion.ToInt(CmbFreightAccountSale.Value);
				ValidateFreight(saleNet, saleAmount, saleAddLess, saleAccId, "Sale");
				if (purchaseNet > 0m && saleNet > 0m && PurchaseSaleFreightDiffTolerancePercent > 0m)
				{
					decimal toleranceAmount = purchaseNet * PurchaseSaleFreightDiffTolerancePercent / 100m;
					if (Math.Abs(purchaseNet - saleNet) > toleranceAmount)
					{
						throw new Exception("Freight difference exceeds allowed tolerance. \n" + $"Purchase: {purchaseNet}, Sale: {saleNet}, \n" + $"Tolerance: {PurchaseSaleFreightDiffTolerancePercent}%.");
					}
				}
				int purchaseFreightAccountGlId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString((object)purchaseAccId)));
				int saleFreightAccountGlId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString((object)saleAccId)));
				if (purchaseFreightAccountGlId > 0 && obj.TradingGlAccountId > 0 && purchaseFreightAccountGlId == obj.TradingGlAccountId)
				{
					CmbFreightAccountPurchase.Value = 0;
					CmbFreightAccountPurchase.Focus();
					throw new Exception("Purchase Freight Account cannot be same as Trading Account.");
				}
				if (saleFreightAccountGlId > 0 && obj.TradingGlAccountId > 0 && saleFreightAccountGlId == obj.TradingGlAccountId)
				{
					CmbFreightAccountSale.Value = 0;
					CmbFreightAccountSale.Focus();
					throw new Exception("Sale Freight Account cannot be same as Trading Account.");
				}
				int grnId = Conversion.ToInt((object)GridEX_Helper.GetFirstOrDefaultValueOfColumn(grd, "GrnId"));
				int gdnId = Conversion.ToInt((object)GridEX_Helper.GetFirstOrDefaultValueOfColumn(grd, "GdnId"));
				if (isPurchaseDelivered && purchaseNet > 0m && purchaseAccId > 0)
				{
					CommisionAgentBillFreightDetail comm = new CommisionAgentBillFreightDetail();
					comm.Id = 0;
					comm.EntrySideId = 1;
					comm.grnSupplierLoadingMasterId = grnId;
					comm.gdnBuyerDispatchMasterId = 0;
					comm.AccountId = purchaseAccId;
					comm.Amount = purchaseAmount;
					comm.AddLessAmount = purchaseAddLess;
					comm.NetAmount = purchaseNet;
					comm.Remarks = Conversion.ToString((object)txtFreightRemarksPurchase.Text);
					obj.CommisionAgentBillFreightDetailList.Add(comm);
				}
				if (isSaleDelivered && saleNet > 0m && saleAccId > 0)
				{
					CommisionAgentBillFreightDetail comm2 = new CommisionAgentBillFreightDetail();
					comm2.Id = 0;
					comm2.EntrySideId = 2;
					comm2.grnSupplierLoadingMasterId = 0;
					comm2.gdnBuyerDispatchMasterId = gdnId;
					comm2.AccountId = saleAccId;
					comm2.Amount = saleAmount;
					comm2.AddLessAmount = saleAddLess;
					comm2.NetAmount = saleNet;
					comm2.Remarks = Conversion.ToString((object)txtFreightRemarksSale.Text);
					obj.CommisionAgentBillFreightDetailList.Add(comm2);
				}
				decimal purchaseTaxPercent = Conversion.ToDecimal((object)txtWhtTaxPercentPurchase.Text);
				decimal purchaseTaxAmount = Conversion.ToDecimal((object)txtWhtTaxAmountPurchase.Text);
				int purchaseTaxAccId = Conversion.ToInt(CmbWhtAccountPurchase.Value);
				int purchaseTaxTypeId = Conversion.ToInt(CmbWhtTaxTypePurchase.Value);
				Conversion.ToString((object)((Control)(object)CmbWhtTaxTypePurchase).Text);
				decimal saleTaxPercent = Conversion.ToDecimal((object)txtWhtTaxPercentSale.Text);
				decimal saleTaxAmount = Conversion.ToDecimal((object)txtwhtTaxAmountSale.Text);
				int saleTaxAccId = Conversion.ToInt(CmbWhtAccountSale.Value);
				int saleTaxTypeId = Conversion.ToInt(CmbWhtTaxTypeSale.Value);
				Conversion.ToString((object)((Control)(object)CmbWhtTaxTypeSale).Text);
				ValidateTax(purchaseTaxPercent, purchaseTaxAmount, purchaseTaxAccId, purchaseTaxTypeId, "Purchase");
				ValidateTax(saleTaxPercent, saleTaxAmount, saleTaxAccId, saleTaxTypeId, "Sale");
				int supplierAccountId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString((object)obj.SupplierId)));
				int customerAccountId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString((object)obj.CustomerId)));
				if (purchaseTaxAccId > 0 && supplierAccountId > 0 && purchaseTaxAccId == supplierAccountId)
				{
					CmbWhtAccountPurchase.Value = 0;
					CmbWhtAccountPurchase.Focus();
					throw new Exception("Purchase WHT/TAX Account cannot be the same as Supplier Account.");
				}
				if (saleTaxAccId > 0 && customerAccountId > 0 && saleTaxAccId == customerAccountId)
				{
					CmbWhtAccountSale.Value = 0;
					CmbWhtAccountSale.Focus();
					throw new Exception("Sales WHT/TAX Account cannot be the same as Customer Account.");
				}
				if (purchaseTaxAmount > 0m && purchaseTaxAccId > 0 && purchaseTaxTypeId > 0)
				{
					CommisionAgentBillTaxDetail tax2 = new CommisionAgentBillTaxDetail();
					tax2.Id = 0;
					tax2.EntrySideId = 1;
					tax2.EntrySide = "Purchase";
					tax2.TaxAccountId = purchaseTaxAccId;
					tax2.TaxAccountTitle = Conversion.ToString((object)((Control)(object)CmbWhtAccountPurchase).Text);
					tax2.TaxTypeId = purchaseTaxTypeId;
					tax2.TaxTypeName = Conversion.ToString((object)((Control)(object)CmbWhtTaxTypePurchase).Text);
					tax2.TaxPercantage = purchaseTaxPercent;
					tax2.TaxAmount = purchaseTaxAmount;
					tax2.Remarks = "";
					tax2.sortNo = 1;
					obj.CommisionAgentBillTaxDetailList.Add(tax2);
				}
				if (saleTaxAmount > 0m && saleTaxAccId > 0 && saleTaxTypeId > 0)
				{
					CommisionAgentBillTaxDetail tax = new CommisionAgentBillTaxDetail();
					tax.Id = 0;
					tax.EntrySideId = 2;
					tax.EntrySide = "Sale";
					tax.TaxAccountId = saleTaxAccId;
					tax.TaxAccountTitle = Conversion.ToString((object)((Control)(object)CmbWhtAccountSale).Text);
					tax.TaxTypeId = saleTaxTypeId;
					tax.TaxTypeName = Conversion.ToString((object)((Control)(object)CmbWhtTaxTypeSale).Text);
					tax.TaxPercantage = saleTaxPercent;
					tax.TaxAmount = saleTaxAmount;
					tax.Remarks = "";
					tax.sortNo = 2;
					obj.CommisionAgentBillTaxDetailList.Add(tax);
				}
				if (lstRemoveRecord.Count > 0)
				{
					for (int i = 0; i < lstRemoveRecord.Count; i++)
					{
						obj.InvCommAgentTradeBillDetailslist.Add(lstRemoveRecord[i]);
					}
				}
				decimal grossWeightPurchase = default(decimal);
				decimal grossWeightSale = default(decimal);
				decimal BillWeightPurchase = default(decimal);
				decimal BillWeightSale = default(decimal);
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r5 in rows)
				{
					InvCommAgentTradeBillDetail d = new InvCommAgentTradeBillDetail();
					d.Id = ((RecId > 0) ? Conversion.ToInt(r5.Cells["Id"].Value) : 0);
					d.ActionTypeId = ((d.Id <= 0) ? 1 : 2);
					FillDetailListCommonForInsertAndDelete(d, r5);
					FormHelper.ValidateField(d.ItemId, "Item Name", r5.RowIndex);
					FormHelper.ValidateField(d.Crop, "Crop Year", r5.RowIndex);
					FormHelper.ValidateField(d.PackingTypeId, "Packing Type", r5.RowIndex);
					FormHelper.ValidateField(d.PackUomId, "Pack Uom", r5.RowIndex);
					FormHelper.ValidateField(d.Qty, "Loading Qty", r5.RowIndex);
					FormHelper.ValidateField(d.VehicleNo, "Vehicle No", r5.RowIndex);
					FormHelper.ValidateField(d.BiltyNo, "Bilty No", r5.RowIndex);
					FormHelper.ValidateField(d.GrossWeight, "Purchase Gross Weight", r5.RowIndex);
					FormHelper.ValidateField(d.BillWeight, "Purchase Net Bill Weight", r5.RowIndex);
					FormHelper.ValidateField(d.RatePurchase, "Purchase Rate", r5.RowIndex);
					FormHelper.ValidateField(d.RateUomId, "Purchase Rate UOM", r5.RowIndex);
					FormHelper.ValidateField(d.ItemAmount, "Purchase Amount", r5.RowIndex);
					FormHelper.ValidateField(d.SaleGrossWeight, "Sale Gross Weight", r5.RowIndex);
					FormHelper.ValidateField(d.SaleBillWeight, "Sale Bill Weight", r5.RowIndex);
					FormHelper.ValidateField(d.RateSale, "Sale Rate", r5.RowIndex);
					FormHelper.ValidateField(d.SaleRateUomId, "Sale Rate UOM", r5.RowIndex);
					FormHelper.ValidateField(d.SaleAmount, "Sale Amount", r5.RowIndex);
					grossWeightPurchase += d.GrossWeight;
					grossWeightSale += d.SaleGrossWeight;
					BillWeightPurchase += d.BillWeight;
					BillWeightSale += d.SaleBillWeight;
					obj.InvCommAgentTradeBillDetailslist.Add(d);
				}
				if (obj.SupplierNetWeight != grossWeightPurchase)
				{
					txtGrossWeightPurchase.Focus();
					throw new Exception("Supplier Net Weight:" + obj.SupplierNetWeight.ToString("#,##0.###") + " not equal to detail Purchase gross weight:" + grossWeightPurchase.ToString("#,###0.###") + ". please Check!");
				}
				if (obj.BuyerNetWeight != grossWeightSale)
				{
					txtGrossWeightSale.Focus();
					throw new Exception("Buyer Net Weight:" + obj.BuyerNetWeight.ToString("#,##0.###") + " not equal to detail Sale gross weight:" + grossWeightSale.ToString("#,###0.###") + ". please Check!");
				}
				if (obj.SupplierBillWeight != BillWeightPurchase)
				{
					txtBillWeightGrn.Focus();
					throw new Exception("Supplier BillWeight:" + obj.SupplierNetWeight.ToString("#,##0.###") + " not equal to detail Purchase Bill weight:" + BillWeightPurchase.ToString("#,###0.###") + ". please Check!");
				}
				if (obj.BuyerBillWeight != BillWeightSale)
				{
					txtBillWeightGdn.Focus();
					throw new Exception("Buyer BillWeight:" + obj.BuyerBillWeight.ToString("#,##0.###") + " not equal to detail Sale Bill weight:" + BillWeightSale.ToString("#,###0.###") + ". please Check!");
				}
				rows = grdPurchaseOtherExpenses.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					if (Conversion.ToInt(r4.Cells["ItemId"].Value) > 0 && Conversion.ToInt(r4.Cells["Amount"].Value) > 0)
					{
						CommisionAgentBillPurchaseExpense PE2 = new CommisionAgentBillPurchaseExpense();
						PE2.grnSupplierLoadingMasterId = Conversion.ToInt(r4.Cells["GrnId"].Value);
						PE2.InvExpItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
						PE2.OtherItemName = Conversion.ToString(r4.Cells["ItemName"].Value);
						PE2.Qty = Conversion.ToDouble(r4.Cells["Qty"].Value);
						PE2.Rate = Conversion.ToDouble(r4.Cells["Rate"].Value);
						PE2.Amount = Conversion.ToDouble(r4.Cells["Amount"].Value);
						PE2.Remarks = Conversion.ToString((object)r4.Cells["Remarks"].Text.Trim());
						if ((PE2.Remarks == "0" || PE2.Remarks == string.Empty || PE2.Remarks == null) && Conversion.ToDouble(r4.Cells["Amount"].Value) > 0.0)
						{
							PE2.Remarks = $"[ItemName:{PE2.OtherItemName}, Qty:{PE2.Qty}, Rate:{PE2.Rate}, Amount:{PE2.Amount}]";
						}
						obj.InvCommAgentTradePurchaseExpList.Add(PE2);
					}
					else if (Conversion.ToInt(r4.Cells["ItemId"].Value) > 0 && Conversion.ToInt(r4.Cells["Amount"].Value) < 0)
					{
						MessageBox.Show("Amount Can't be Less than 0 in Purchase Expense Grid in Row#" + (r4.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPurchaseOtherExpenses).Focus();
						return;
					}
				}
				if (obj.InvCommAgentTradePurchaseExpList != null && obj.InvCommAgentTradePurchaseExpList.Count > 0)
				{
					obj.PurchaseExpenseDetailDescription = string.Join(", ", obj.InvCommAgentTradePurchaseExpList.Select((CommisionAgentBillPurchaseExpense x) => $"[ItemName:{x.OtherItemName}, Qty:{x.Qty}, Rate:{x.Rate}, Amount:{x.Amount}]"));
				}
				else
				{
					obj.PurchaseExpenseDetailDescription = string.Empty;
				}
				rows = grdSaleOtherExpense.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (Conversion.ToInt(r3.Cells["ItemId"].Value) > 0 && Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0)
					{
						CommisionAgentBillSaleExpense SE2 = new CommisionAgentBillSaleExpense();
						SE2.gdnBuyerDispatchMasterId = Conversion.ToInt(r3.Cells["GdnId"].Value);
						SE2.InvExpItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
						SE2.OtherItemName = Conversion.ToString(r3.Cells["ItemName"].Value);
						SE2.Qty = Conversion.ToDouble(r3.Cells["Qty"].Value);
						SE2.Rate = Conversion.ToDouble(r3.Cells["Rate"].Value);
						SE2.Amount = Conversion.ToDouble(r3.Cells["Amount"].Value);
						SE2.Remarks = Conversion.ToString((object)r3.Cells["Remarks"].Text.Trim());
						if ((SE2.Remarks == "0" || SE2.Remarks == string.Empty || SE2.Remarks == null) && Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0)
						{
							SE2.Remarks = $"[ItemName:{SE2.OtherItemName}, Qty:{SE2.Qty}, Rate:{SE2.Rate}, Amount:{SE2.Amount}]";
						}
						obj.InvCommAgentTradeSaleExpList.Add(SE2);
					}
					else if (Conversion.ToInt(r3.Cells["ItemId"].Value) > 0 && Conversion.ToInt(r3.Cells["Amount"].Value) < 0)
					{
						MessageBox.Show("Amount Can't be Less than 0 in Sale Expense Grid in Row#" + (r3.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdSaleOtherExpense).Focus();
						return;
					}
				}
				if (obj.InvCommAgentTradeSaleExpList != null && obj.InvCommAgentTradeSaleExpList.Count > 0)
				{
					obj.SaleExpenseDetailDescription = string.Join(", ", obj.InvCommAgentTradeSaleExpList.Select((CommisionAgentBillSaleExpense x) => $"[ItemName:{x.OtherItemName}, Qty:{x.Qty}, Rate:{x.Rate}, Amount:{x.Amount}]"));
				}
				else
				{
					obj.PurchaseExpenseDetailDescription = string.Empty;
				}
				rows = grdSalePmExpenseSale.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToDouble(r2.Cells["Amount"].Value) > 0.0)
					{
						if (isSaleXFactory && Conversion.ToInt(r2.Cells["AccountId"].Value) == customerAccountId)
						{
							MessageBox.Show("PM Sale Expense Account And Customer Gl Account Can't be Same in Row#" + (r2.RowIndex + 1), "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							tabControl2.SelectedIndex = 3;
							((Control)(object)grdSalePmExpenseSale).Focus();
							return;
						}
						CommisionAgentBillSaleExpenseCreditToReleventAc SE = new CommisionAgentBillSaleExpenseCreditToReleventAc();
						SE.gdnBuyerDispatchMasterId = Conversion.ToInt(r2.Cells["GdnId"].Value);
						SE.AccountId = Conversion.ToInt(r2.Cells["AccountId"].Value);
						SE.AccountTitle = Conversion.ToString((object)r2.Cells["AccountId"].Text);
						if (Conversion.ToInt(r2.Cells["AccountId"].Value) == 0)
						{
							MessageBox.Show("Account is Required in Sale PM Expense Grid in Row#" + (r2.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							tabControl2.SelectedIndex = 3;
							((Control)(object)grdSalePmExpenseSale).Focus();
							return;
						}
						SE.PackingTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
						SE.PackingType = Conversion.ToString((object)r2.Cells["PackingTypeId"].Text);
						SE.PmItemId = Conversion.ToInt(r2.Cells["PmItemId"].Value);
						SE.PmItemName = Conversion.ToString((object)r2.Cells["PmItemId"].Text);
						SE.EBWeightDeductionTermId = Conversion.ToInt(r2.Cells["EmptyBagTermId"].Value);
						SE.EBWeightDeductionTerm = Conversion.ToString((object)r2.Cells["EmptyBagTerm"].Text);
						SE.Qty = Conversion.ToDouble(r2.Cells["Qty"].Value);
						SE.Rate = Conversion.ToDouble(r2.Cells["Rate"].Value);
						SE.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
						SE.Remarks = Conversion.ToString((object)r2.Cells["Remarks"].Text.Trim());
						if (SE.Remarks == "0" || SE.Remarks == string.Empty || SE.Remarks == null)
						{
							List<string> parts2 = new List<string> { "Account:" + SE.AccountTitle };
							if (SE.PackingTypeId > 0)
							{
								parts2.Add("PackingType:" + SE.PackingType);
							}
							if (SE.PmItemId > 0)
							{
								parts2.Add("PmItem:" + SE.PmItemName);
							}
							if (SE.EBWeightDeductionTermId > 0)
							{
								parts2.Add("EmptyBagTerm:" + SE.EBWeightDeductionTerm);
							}
							if (SE.Qty > 0.0)
							{
								parts2.Add($"Qty:{SE.Qty}");
							}
							if (SE.Rate > 0.0)
							{
								parts2.Add($"Rate:{SE.Rate}");
							}
							if (SE.Amount > 0.0)
							{
								parts2.Add($"Amount:{SE.Amount}");
							}
							SE.Remarks = "[" + string.Join(", ", parts2) + "]";
						}
						obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList.Add(SE);
					}
					else if (Conversion.ToInt(r2.Cells["AccountId"].Value) > 0 && Conversion.ToInt(r2.Cells["Amount"].Value) < 0)
					{
						MessageBox.Show("Amount Can't be Less than 0 in Sale PM Expense Grid in Row#" + (r2.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdSalePmExpenseSale).Focus();
						return;
					}
				}
				if (obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList != null && obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList.Count > 0)
				{
					obj.SaleExpenseCreditToReleventAccountDetailDescription = string.Join(", ", obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList.Select((CommisionAgentBillSaleExpenseCreditToReleventAc x) => $"[Account:{x.AccountTitle}, Qty:{x.Qty}, Rate:{x.Rate}, Amount:{x.Amount}\n" + "PackingType:" + x.PackingType + ", PmItem:" + x.PmItemName + ", EmptyBagTerm:" + x.EBWeightDeductionTerm + "]"));
				}
				else
				{
					obj.SaleExpenseCreditToReleventAccountDetailDescription = string.Empty;
				}
				rows = grdPurchasePmExpense.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0)
					{
						if (isPurchaseDelivered)
						{
							if (Conversion.ToInt(r.Cells["AccountId"].Value) == supplierAccountId)
							{
								MessageBox.Show($"Freight Account And Supplier Gl Account Can't be Same in Row# {r.RowIndex + 1} " + "\nBecause Purchase Delivery Term is " + Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text), "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								tabControl2.SelectedIndex = 2;
								((Control)(object)grdPurchasePmExpense).Focus();
								return;
							}
						}
						else if (Conversion.ToInt(r.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value))
						{
							MessageBox.Show($"Freight Account And TradingAccount Can't be Same in Row# {r.RowIndex + 1} " + "\nBecause Purchase Delivery Term is " + Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text), "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							tabControl2.SelectedIndex = 2;
							((Control)(object)grdPurchasePmExpense).Focus();
							return;
						}
						CommisionAgentBillPurchaseFreightExpense PE = new CommisionAgentBillPurchaseFreightExpense();
						PE.grnSupplierLoadingMasterId = Conversion.ToInt(r.Cells["GrnId"].Value);
						PE.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
						PE.AccountTitle = Conversion.ToString((object)r.Cells["AccountId"].Text);
						if (Conversion.ToInt(r.Cells["AccountId"].Value) == 0)
						{
							MessageBox.Show("Account is Required in Purchase PM Expense Grid in Row#" + (r.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							tabControl2.SelectedIndex = 2;
							((Control)(object)grdSalePmExpenseSale).Focus();
							return;
						}
						PE.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						PE.PackingType = Conversion.ToString((object)r.Cells["PackingTypeId"].Text);
						PE.PmItemId = Conversion.ToInt(r.Cells["PmItemId"].Value);
						PE.PmItemName = Conversion.ToString((object)r.Cells["PmItemId"].Text);
						PE.EBWeightDeductionTermId = Conversion.ToInt(r.Cells["EmptyBagTermId"].Value);
						PE.EBWeightDeductionTerm = Conversion.ToString((object)r.Cells["EmptyBagTerm"].Text);
						PE.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text.Trim());
						if (PE.Remarks == "0" || PE.Remarks == string.Empty || PE.Remarks == null)
						{
							List<string> parts = new List<string> { "Account:" + PE.AccountTitle };
							if (PE.PackingTypeId > 0)
							{
								parts.Add("PackingType:" + PE.PackingType);
							}
							if (PE.PmItemId > 0)
							{
								parts.Add("PmItem:" + PE.PmItemName);
							}
							if (PE.EBWeightDeductionTermId > 0)
							{
								parts.Add("EmptyBagTerm:" + PE.EBWeightDeductionTerm);
							}
							if (PE.Qty > 0m)
							{
								parts.Add($"Qty:{PE.Qty}");
							}
							if (PE.Rate > 0m)
							{
								parts.Add($"Rate:{PE.Rate}");
							}
							if (PE.Amount > 0m)
							{
								parts.Add($"Amount:{PE.Amount}");
							}
							PE.Remarks = "[" + string.Join(", ", parts) + "]";
						}
						obj.InvCommAgentTradeFreightExpList.Add(PE);
					}
					else if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0 && Conversion.ToInt(r.Cells["Amount"].Value) < 0)
					{
						MessageBox.Show("Amount Can't be Less than 0 in Purchase Pm Expense Grid in Row#" + (r.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdPurchasePmExpense).Focus();
						return;
					}
				}
				if (obj.InvCommAgentTradeFreightExpList != null && obj.InvCommAgentTradeFreightExpList.Count > 0)
				{
					obj.PurchaseFreightExpenseDetailDescription = string.Join(", ", obj.InvCommAgentTradeFreightExpList.Select((CommisionAgentBillPurchaseFreightExpense x) => $"[Account:{x.AccountTitle}, Qty:{x.Qty}, Rate:{x.Rate}, Amount:{x.Amount}\n" + "PackingType:" + x.PackingType + ", PmItem:" + x.PmItemName + ", EmptyBagTerm:" + x.EBWeightDeductionTerm + "]"));
				}
				else
				{
					obj.PurchaseFreightExpenseDetailDescription = string.Empty;
				}
				AddCommissionOrBrokery(obj, grdPurchaseCommission, "CommissionAgentId", "CommissionAmount", "CommissionTypeId", "CommissionUomId", "CommissionRate", 1, 1, "PoId");
				AddCommissionOrBrokery(obj, grdSaleCommission, "CommissionAgentId", "CommissionAmount", "CommissionTypeId", "CommissionUomId", "CommissionRate", 1, 2, "SoId");
				AddCommissionOrBrokery(obj, grdPurchaseBrokery, "BrokeryAgentId", "BrokeryAmount", "BrokeryTypeId", "BrokeryUomId", "BrokeryRate", 2, 1, "PoId");
				AddCommissionOrBrokery(obj, grdSaleBrokery, "BrokeryAgentId", "BrokeryAmount", "BrokeryTypeId", "BrokeryUomId", "BrokeryRate", 2, 2, "SoId");
				obj.PurchaseCommisionDetailDescription = ((obj.CommisionAgentBillCommissionDetailList != null && obj.CommisionAgentBillCommissionDetailList.Count > 0) ? string.Join(", ", from x in obj.CommisionAgentBillCommissionDetailList
					where x.EntrySideId == 1 && x.agentTypeId == 1
					select string.Format("[Agent:{0}, Type:{1}, {2}Rate:{3}, Amount:{4}{5}]", x.CommissionAgentName, x.CommissionType, (x.commissionTypeId == 3) ? $"Uom:{x.RateUom}, " : "", x.commissionRate, x.commissionAmount, (x.debitAccountId > 0) ? (", DebitAccount:" + x.AccountTitle) : "")) : string.Empty);
				obj.SaleCommisionDetailDescription = ((obj.CommisionAgentBillCommissionDetailList != null && obj.CommisionAgentBillCommissionDetailList.Count > 0) ? string.Join(", ", from x in obj.CommisionAgentBillCommissionDetailList
					where x.EntrySideId == 2 && x.agentTypeId == 1
					select string.Format("[Agent:{0}, Type:{1}, {2}Rate:{3}, Amount:{4}{5}]", x.CommissionAgentName, x.CommissionType, (x.commissionTypeId == 3) ? $"Uom:{x.RateUom}, " : "", x.commissionRate, x.commissionAmount, (x.debitAccountId > 0) ? (", DebitAccount:" + x.AccountTitle) : "")) : string.Empty);
				obj.PurchaseBrokeryDetailDescription = ((obj.CommisionAgentBillCommissionDetailList != null && obj.CommisionAgentBillCommissionDetailList.Count > 0) ? string.Join(", ", from x in obj.CommisionAgentBillCommissionDetailList
					where x.EntrySideId == 1 && x.agentTypeId == 2
					select string.Format("[Broker:{0}, Type:{1}, {2}Rate:{3}, Amount:{4}{5}]", x.CommissionAgentName, x.CommissionType, (x.commissionTypeId == 3) ? $"Uom:{x.RateUom}, " : "", x.commissionRate, x.commissionAmount, (x.debitAccountId > 0) ? (", DebitAccount:" + x.AccountTitle) : "")) : string.Empty);
				obj.SaleBrokeryDetailDescription = ((obj.CommisionAgentBillCommissionDetailList != null && obj.CommisionAgentBillCommissionDetailList.Count > 0) ? string.Join(", ", from x in obj.CommisionAgentBillCommissionDetailList
					where x.EntrySideId == 2 && x.agentTypeId == 2
					select string.Format("[Broker:{0}, Type:{1}, {2}Rate:{3}, Amount:{4}{5}]", x.CommissionAgentName, x.CommissionType, (x.commissionTypeId == 3) ? $"Uom:{x.RateUom}, " : "", x.commissionRate, x.commissionAmount, (x.debitAccountId > 0) ? (", DebitAccount:" + x.AccountTitle) : "")) : string.Empty);
				obj.CommisionAgentBillPaymentDetailList = new List<CommisionAgentBillPaymentDetail>();
				decimal Pamount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentDetailPurchase, "Amount"));
				decimal PaymentDetailAmount = default(decimal);
				decimal PtPurScheduleDiff = Conversion.ToDecimal((object)obj.PurchaseBillAmount) - Pamount;
				decimal PrcntOfTotal = default(decimal);
				if (Pamount > 0m)
				{
					int ptcount2 = grdPaymentDetailPurchase.GetDataRows().Count();
					rows = grdPaymentDetailPurchase.GetDataRows();
					foreach (GridEXRow item2 in rows)
					{
						decimal dueamount2 = Conversion.ToDecimal(item2.Cells["Amount"].Value);
						decimal paymentdueamount2 = ((ptcount2 > 1 && PtPurScheduleDiff <= 1m) ? (dueamount2 + PtPurScheduleDiff) : dueamount2);
						CommisionAgentBillPaymentDetail pd2 = new CommisionAgentBillPaymentDetail();
						pd2.EntrySideId = 1;
						pd2.purchaseOrderMasterId = Conversion.ToInt(item2.Cells["PoId"].Value);
						pd2.PurchaseOrderNo = Conversion.ToInt(item2.Cells["PoNo"].Value);
						pd2.PaymentTermId = Conversion.ToInt(item2.Cells["PaymentTermId"].Value);
						pd2.PaymentTerm = Conversion.ToString((object)item2.Cells["PaymentTermId"].Text);
						pd2.DueDays = Conversion.ToInt(item2.Cells["DueDays"].Value);
						pd2.pctOfTotal = Conversion.ToDecimal(item2.Cells["%OfTotal"].Value);
						if (pd2.pctOfTotal == 0m)
						{
							throw new Exception("%Of Total not found");
						}
						pd2.dueAmount = ((ptcount2 == 1) ? Conversion.ToDecimal((object)obj.PurchaseBillAmount) : paymentdueamount2);
						pd2.BaseDueDateTypeId = Conversion.ToInt(item2.Cells["BaseDateTypeId"].Value);
						pd2.DueDate = ((pd2.BaseDueDateTypeId == 1) ? new DateTime?(obj.DocDate.AddDays(pd2.DueDays)) : ((pd2.BaseDueDateTypeId == 4) ? new DateTime?(Conversion.ToDateTime(item2.Cells["DueDate"].Value)) : null));
						if (Conversion.ToDouble(dtPurchasePaymentDetail.Compute("Sum([%OfTotal])", $"PoId = '{pd2.purchaseOrderMasterId}'")) != 100.0)
						{
							throw new Exception($"Percent Of Purcahse Order:{pd2.PurchaseOrderNo} not equal to 100. Please Check!");
						}
						PrcntOfTotal += pd2.pctOfTotal;
						PaymentDetailAmount += pd2.dueAmount;
						PtPurScheduleDiff = default(decimal);
						if (pd2.PaymentTermId <= 0)
						{
							MessageBox.Show("Payment Term Required in row#" + (item2.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							tabControl2.SelectedIndex = 6;
							((Control)(object)grdPaymentDetailPurchase).Focus();
							return;
						}
						if (pd2.PaymentTermId == 2 && pd2.DueDays <= 0)
						{
							MessageBox.Show("Due Days Required In case Of Credit row in row#" + (item2.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							tabControl2.SelectedIndex = 6;
							((Control)(object)grdPaymentDetailPurchase).Focus();
							return;
						}
						obj.CommisionAgentBillPaymentDetailList.Add(pd2);
					}
				}
				else
				{
					CommisionAgentBillPaymentDetail psD2 = new CommisionAgentBillPaymentDetail();
					psD2.EntrySideId = 1;
					psD2.PaymentTermId = Conversion.ToInt(CmbPaymentTermPurchase.Value);
					psD2.PaymentTerm = Conversion.ToString((object)((Control)(object)CmbPaymentTermPurchase).Text);
					psD2.DueDays = Conversion.ToInt((object)txtDueDaysPurchase.Text);
					psD2.BaseDueDateTypeId = 1;
					psD2.DueDate = obj.DocDate.AddDays(psD2.DueDays);
					psD2.DueDate = obj.DocDate.AddDays(psD2.DueDays);
					psD2.pctOfTotal = 100m;
					psD2.dueAmount = Conversion.ToDecimal((object)obj.PurchaseBillAmount);
					PaymentDetailAmount = psD2.dueAmount;
					PrcntOfTotal = 100m;
					obj.CommisionAgentBillPaymentDetailList.Add(psD2);
				}
				if (Math.Abs(Conversion.ToDecimal((object)obj.PurchaseBillAmount) - PaymentDetailAmount) > 0.3m)
				{
					MessageBox.Show("Purchase Payment Detail Amount" + PaymentDetailAmount.ToString("#,##0.####") + " Not Equal to Total Supplier Amount" + obj.PurchaseBillAmount.ToString("#,##0.####"), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					tabControl2.SelectedIndex = 6;
					((Control)(object)grdPaymentDetailPurchase).Focus();
					return;
				}
				PrcntOfTotal = Math.Round(PrcntOfTotal, 4);
				if (Math.Abs(100m - PrcntOfTotal) > 0.01m)
				{
					MessageBox.Show("Payment Detail Total% not near to 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					tabControl2.SelectedIndex = 6;
					((Control)(object)grdPaymentDetailPurchase).Focus();
					return;
				}
				obj.PurchasePaymentDetailDescription = ((obj.CommisionAgentBillPaymentDetailList != null && obj.CommisionAgentBillPaymentDetailList.Count > 0) ? string.Join(", ", from x in obj.CommisionAgentBillPaymentDetailList
					where x.EntrySideId == 1
					select $"[Payment Term:{x.PaymentTerm}, Days:{x.DueDays}, %OfTotal:{x.pctOfTotal} ,Amount:{x.dueAmount}, BaseDateType:{x.BaseDateType}]") : string.Empty);
				decimal Samount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentDetailSale, "Amount"));
				decimal PaymentDetailAmountSale = default(decimal);
				decimal PrcntOfTotalSale = default(decimal);
				decimal PtScheduleDiff = Conversion.ToDecimal((object)obj.SaleBillAmount) - Samount;
				if (Samount > 0m)
				{
					int ptcount = grdPaymentDetailSale.GetDataRows().Count();
					rows = grdPaymentDetailSale.GetDataRows();
					foreach (GridEXRow item in rows)
					{
						decimal dueamount = Conversion.ToDecimal(item.Cells["Amount"].Value);
						decimal paymentdueamount = ((ptcount > 1 && PtScheduleDiff <= 1m) ? (dueamount + PtScheduleDiff) : dueamount);
						CommisionAgentBillPaymentDetail pd = new CommisionAgentBillPaymentDetail();
						pd.EntrySideId = 2;
						pd.saleOrderMasterId = Conversion.ToInt(item.Cells["SoId"].Value);
						pd.SaleOrderNo = Conversion.ToInt(item.Cells["SoNo"].Value);
						pd.PaymentTermId = Conversion.ToInt(item.Cells["PaymentTermId"].Value);
						pd.PaymentTerm = Conversion.ToString((object)item.Cells["PaymentTermId"].Text);
						pd.DueDays = Conversion.ToInt(item.Cells["DueDays"].Value);
						pd.pctOfTotal = Conversion.ToDecimal(item.Cells["%OfTotal"].Value);
						if (pd.pctOfTotal == 0m)
						{
							throw new Exception("%Of Total not found");
						}
						pd.dueAmount = ((ptcount == 1) ? Conversion.ToDecimal((object)obj.SaleBillAmount) : paymentdueamount);
						pd.BaseDueDateTypeId = Conversion.ToInt(item.Cells["BaseDateTypeId"].Value);
						pd.BaseDateType = Conversion.ToString((object)item.Cells["BaseDateTypeId"].Text);
						pd.DueDate = ((pd.BaseDueDateTypeId == 1) ? new DateTime?(obj.DocDate.AddDays(pd.DueDays)) : ((pd.BaseDueDateTypeId == 4) ? new DateTime?(Conversion.ToDateTime(item.Cells["DueDate"].Value)) : null));
						if (Conversion.ToDouble(dtSalePaymentDetail.Compute("Sum([%OfTotal])", $"SoId = '{pd.saleOrderMasterId}'")) != 100.0)
						{
							throw new Exception($"Percent Of Sale Order:{pd.SaleOrderNo} not equal to 100. Please Check!");
						}
						PrcntOfTotalSale += pd.pctOfTotal;
						PaymentDetailAmountSale += pd.dueAmount;
						PtScheduleDiff = default(decimal);
						if (pd.PaymentTermId <= 0)
						{
							MessageBox.Show("Payment Term Required in row#" + (item.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							tabControl2.SelectedIndex = 6;
							((Control)(object)grdPaymentDetailSale).Focus();
							return;
						}
						if (pd.PaymentTermId == 2 && pd.DueDays <= 0)
						{
							MessageBox.Show("Due Days Required In case Of Credit row in row#" + (item.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							tabControl2.SelectedIndex = 6;
							((Control)(object)grdPaymentDetailSale).Focus();
							return;
						}
						obj.CommisionAgentBillPaymentDetailList.Add(pd);
					}
				}
				else
				{
					CommisionAgentBillPaymentDetail psD = new CommisionAgentBillPaymentDetail();
					psD.EntrySideId = 2;
					psD.PaymentTermId = Conversion.ToInt(CmbPaymentTermSale.Value);
					psD.PaymentTerm = Conversion.ToString((object)((Control)(object)CmbPaymentTermSale).Text);
					psD.DueDays = Conversion.ToInt((object)txtDueDaysSale.Text);
					psD.BaseDueDateTypeId = 1;
					psD.DueDate = obj.DocDate.AddDays(psD.DueDays);
					psD.DueDate = obj.DocDate.AddDays(psD.DueDays);
					psD.pctOfTotal = 100m;
					psD.dueAmount = Conversion.ToDecimal((object)obj.SaleBillAmount);
					PaymentDetailAmountSale = psD.dueAmount;
					PrcntOfTotalSale = 100m;
					obj.CommisionAgentBillPaymentDetailList.Add(psD);
				}
				if (Math.Abs(Conversion.ToDecimal((object)obj.SaleBillAmount) - PaymentDetailAmountSale) > 0.3m)
				{
					MessageBox.Show("Sale Payment Detail Amount" + PaymentDetailAmountSale.ToString("#,##0.####") + " Not Equal to Total Customer Amount" + obj.SaleBillAmount.ToString("#,##0.####"), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					tabControl2.SelectedIndex = 6;
					((Control)(object)grdPaymentDetailSale).Focus();
					return;
				}
				PrcntOfTotalSale = Math.Round(PrcntOfTotalSale, 4);
				if (Math.Abs(100m - PrcntOfTotalSale) > 0.01m)
				{
					MessageBox.Show("Sale Payment Detail Total% not near to 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					tabControl2.SelectedIndex = 6;
					((Control)(object)grdPaymentDetailSale).Focus();
					return;
				}
				obj.SalePaymentDetailDescription = ((obj.CommisionAgentBillPaymentDetailList != null && obj.CommisionAgentBillPaymentDetailList.Count > 0) ? string.Join(", ", from x in obj.CommisionAgentBillPaymentDetailList
					where x.EntrySideId == 2
					select $"[Payment Term:{x.PaymentTerm}, Days:{x.DueDays}, %OfTotal:{x.pctOfTotal} ,Amount:{x.dueAmount}, BaseDateType:{x.BaseDateType}]") : string.Empty);
				obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvCommAgentTradeBill>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = InvCommAgentTradeBill.Save(obj);
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully [ " + obj.DocNo + " ] ");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, obj.DocumentTypeId, 0);
					}
				}
				else
				{
					MessageBox.Show("Record Save Successfully  [ " + obj.DocNo + " ] ");
				}
				ResetForm();
				FormResetDBCallsAndDefault();
				if (ChkPreview1056.Checked)
				{
					OpenSlip(success);
				}
				if (ChkPreview1056A.Checked)
				{
					OpenSlip(success, 2);
				}
				if (ChkPreview105601.Checked)
				{
					OpenSlip(success, 3);
				}
				if (ChkPreview105602.Checked)
				{
					OpenSlip(success, 4);
				}
				if (ChkPreview105603.Checked)
				{
					OpenSlip(success, 5);
				}
				if (ChkPreview118.Checked)
				{
					OpenVoucher(success, DocumentTypeId);
				}
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			ResetForm();
			RecId = Conversion.ToInt((object)ID);
			InvCommAgentTradeBill obj = InvCommAgentTradeBill.GetByID(RecId);
			if (obj == null)
			{
				return;
			}
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			BtnDelete.Visible = true;
			tabControl1.SelectedIndex = 0;
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, DocumentTypeId);
			TxtDocNo.Text = obj.DocNo.ToString();
			txtBranchSrNo.Text = Conversion.ToString((object)obj.BranchSrNo);
			txtDocDate.Value = obj.DocDate;
			CmbTradingAccount.Value = obj.TradingGlAccountId;
			txtMainRemarks.Text = obj.RemarksHeader;
			CmbSupplier.Value = obj.SupplierId;
			((Control)(object)CmbDeliveryTermPurchase).Text = obj.DeliveryTerm;
			if (obj.PurchaseTaxAccountId > 0)
			{
				CmbTaxAccountPurchase.Value = obj.PurchaseTaxAccountId;
			}
			txtPurchaseBillAmount.Text = obj.PurchaseBillAmount.ToString(clsGlobalVariables.stringFormatboth);
			txtSupplierFirstWeight.Text = obj.SupplierFirstWeight.ToString("#,##0.###");
			txtSupplierSecondWeight.Text = obj.SupplierSecondWeight.ToString("#,##0.###");
			txtSupplierWeight.Text = obj.SupplierNetWeight.ToString("#,##0.###");
			txtBillWeightGrn.Text = obj.SupplierBillWeight.ToString("#,##0.###");
			CmbCustomer.Value = obj.CustomerId;
			((Control)(object)CmbDeliveryTermSale).Text = obj.DeliveryTermSale;
			if (obj.SaleTaxAccountId > 0)
			{
				CmbTaxAccountSale.Value = obj.SaleTaxAccountId;
			}
			txtSaleBillAmount.Text = obj.SaleBillAmount.ToString(clsGlobalVariables.stringFormatboth);
			txtBuyerFirstWeight.Text = obj.BuyerFirstWeight.ToString("#,##0.###");
			txtBuyerSecondWeight.Text = obj.BuyerSecondWeight.ToString("#,##0.###");
			txtBuyerWeight.Text = obj.BuyerNetWeight.ToString("#,##0.###");
			txtBillWeightGdn.Text = obj.BuyerBillWeight.ToString("#,##0.###");
			CmbCompanyName.Value = obj.CompanyId;
			CmbBranch.Value = obj.BranchesId;
			txtProfitLoss.Text = obj.PLAmount.ToString();
			dtDetail.Rows.Clear();
			TradeBill_Helper.FillDetailtableFromListCommonForReadById(obj, dtDetail);
			bool hasGdnLinked = ((obj == null) ? null : obj.InvCommAgentTradeBillDetailslist?.Any((InvCommAgentTradeBillDetail x) => x.gdnBuyerDispatchMasterId > 0)).GetValueOrDefault();
			EnableAndDisableFields(!hasGdnLinked);
			dtPurchaseOtherExpense.Rows.Clear();
			AddRowsInPurchaseExpenseGrid(dtPurchaseOtherExpense);
			TradeBill_Helper.FillPurchaseExpenseTableFromListCommonForReadById(obj, dtPurchaseOtherExpense);
			dtPurchasePmExpense.Rows.Clear();
			TradeBill_Helper.FillPurchaseFreightTableFromListCommonForReadById(obj, dtPurchasePmExpense);
			dtSaleOtherExpense.Rows.Clear();
			AddRowsInSaleExpenseGrid(dtSaleOtherExpense);
			TradeBill_Helper.FillSaleExpenseTableFromListCommonForReadById(obj, dtSaleOtherExpense);
			dtSalePmExpense.Rows.Clear();
			TradeBill_Helper.FillSalePmTableFromListCommonForReadById(obj, dtSalePmExpense);
			dtPurchaseCommission.Rows.Clear();
			TradeBill_Helper.FillPurchaseCommissionTableFromListCommonForReadById(obj, dtPurchaseCommission);
			dtPurchaseBrokery.Rows.Clear();
			TradeBill_Helper.FillPurchaseBrokeryTableFromListCommonForReadById(obj, dtPurchaseBrokery);
			dtSaleCommission.Rows.Clear();
			TradeBill_Helper.FillSaleCommissionTableFromListCommonForReadById(obj, dtSaleCommission);
			dtSaleBrokery.Rows.Clear();
			TradeBill_Helper.FillSaleBrokeryTableFromListCommonForReadById(obj, dtSaleBrokery);
			dtPurchasePaymentDetail.Rows.Clear();
			TradeBill_Helper.FillPurchasePaymentTableFromListCommonForReadById(obj, dtPurchasePaymentDetail);
			dtSalePaymentDetail.Rows.Clear();
			TradeBill_Helper.FillSalePaymentTableFromListCommonForReadById(obj, dtSalePaymentDetail);
			BindGrids();
			if (obj.CommisionAgentBillFreightDetailList != null && obj.CommisionAgentBillFreightDetailList.Count > 0)
			{
				CommisionAgentBillFreightDetail purchase = obj.CommisionAgentBillFreightDetailList.FirstOrDefault((CommisionAgentBillFreightDetail x) => x.EntrySideId == 1);
				if (purchase != null)
				{
					txtFreightAmountPurchase.Text = purchase.Amount.ToString("#,##0.###");
					txtFreightAddLessPurchase.Text = purchase.AddLessAmount.ToString("#,##0.###");
					txtTotalFreightPurchase.Text = purchase.NetAmount.ToString("#,##0.###");
					txtFreightRemarksPurchase.Text = purchase.Remarks ?? "";
					CmbFreightAccountPurchase.Value = purchase.AccountId;
				}
				CommisionAgentBillFreightDetail sale2 = obj.CommisionAgentBillFreightDetailList.FirstOrDefault((CommisionAgentBillFreightDetail x) => x.EntrySideId == 2);
				if (sale2 != null)
				{
					txtFreightAmountSale.Text = sale2.Amount.ToString("#,##0.###");
					txtFreightAddLessSale.Text = sale2.AddLessAmount.ToString("#,##0.###");
					txtTotalFreightSale.Text = sale2.NetAmount.ToString("#,##0.###");
					txtFreightRemarksSale.Text = sale2.Remarks ?? "";
					CmbFreightAccountSale.Value = sale2.AccountId;
				}
			}
			if (obj.CommisionAgentBillTaxDetailList != null && obj.CommisionAgentBillTaxDetailList.Any())
			{
				List<CommisionAgentBillTaxDetail> purchaseTaxes = obj.CommisionAgentBillTaxDetailList.Where((CommisionAgentBillTaxDetail x) => x.EntrySideId == 1).ToList();
				if (purchaseTaxes.Any())
				{
					CommisionAgentBillTaxDetail purchase2 = purchaseTaxes.FirstOrDefault();
					if (purchase2 != null)
					{
						txtWhtTaxPercentPurchase.Text = purchase2.TaxPercantage.ToString("#,##0.###");
						txtWhtTaxAmountPurchase.Text = purchase2.TaxAmount.ToString("#,##0.###");
						CmbWhtAccountPurchase.Value = purchase2.TaxAccountId;
						CmbWhtTaxTypePurchase.Value = purchase2.TaxTypeId;
					}
				}
				List<CommisionAgentBillTaxDetail> saleTaxes = obj.CommisionAgentBillTaxDetailList.Where((CommisionAgentBillTaxDetail x) => x.EntrySideId == 2).ToList();
				if (saleTaxes.Any())
				{
					CommisionAgentBillTaxDetail sale = saleTaxes.FirstOrDefault();
					if (sale != null)
					{
						txtWhtTaxPercentSale.Text = sale.TaxPercantage.ToString("#,##0.###");
						txtwhtTaxAmountSale.Text = sale.TaxAmount.ToString("#,##0.###");
						CmbWhtAccountSale.Value = sale.TaxAccountId;
						CmbWhtTaxTypeSale.Value = sale.TaxTypeId;
					}
				}
			}
			ExpenseProportionForPurchase();
			ExpenseProportionForSale();
			FreightProportion();
			InitCommissionConfigs();
			CalculateCommissionGrid(GetPurchaseCommissionConfig());
			CalculateCommissionGrid(GetPurchaseBrokeryConfig());
			CalculateCommissionGrid(GetSaleCommissionConfig());
			CalculateCommissionGrid(GetSaleBrokeryConfig());
			ProportionateCommissionAndBrokery();
			SupplierBillAmount();
			CustomerBillAmount();
			PLAmountProportion();
			CalculateHeaderRates();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			CmbSupplier.Focus();
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				InvCommAgentTradeBill.DeleteByID(UserAccount.ID, RecId);
				MessageBox.Show("Delete Record Seccessfully");
				ResetForm();
				FormResetDBCallsAndDefault();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistory).Name);
			if (fs != null)
			{
				GrdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistoryDetail).Name);
			if (fs != null)
			{
				GrdHistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void ParameterFill()
	{
		try
		{
			InfragisticsHelper.BindComboDateType(cmbperemeter);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (fromdate.Checked)
				{
					obj.FromDate = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ToDate = ToDate.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (fromdate.Checked)
				{
					obj.EntryDateFrom = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.EntryDateTo = ToDate.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (fromdate.Checked)
				{
					obj.ModifyDateFrom = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ModifyDateTo = ToDate.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text);
			obj.GlAccountId = Conversion.ToInt(cmbtradAcHistory.Value);
			obj.SupplierId = Conversion.ToInt(cmbsupplierhistory.Value);
			obj.CustomerId = Conversion.ToInt(cmbcusthistory.Value);
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dtHistory = InvCommAgentTradeBill.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("BranchSrNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("Supplier");
				dt.Columns.Add("Customer");
				dt.Columns.Add("RefDocNos");
				dt.Columns.Add("VehicleNos");
				dt.Columns.Add("PurDeliveryTerm");
				dt.Columns.Add("SaleDeliveryTerm");
				dt.Columns.Add("PurchaseBillAmount", typeof(double));
				dt.Columns.Add("SaleBillAmount", typeof(double));
				dt.Columns.Add("PLAmount", typeof(double));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["DocNo"], dtHistory.Rows[i]["BranchSrNo"], Conversion.ToDateTime(dtHistory.Rows[i]["DocDate"]).ToShortDateString(), dtHistory.Rows[i]["SupplierName"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["RefDocNos"], dtHistory.Rows[i]["VehicleNos"], dtHistory.Rows[i]["DeliveryTerm"], dtHistory.Rows[i]["DeliveryTermSale"], dtHistory.Rows[i]["PurchaseBillAmount"], dtHistory.Rows[i]["SaleBillAmount"], dtHistory.Rows[i]["PLAmount"], dtHistory.Rows[i]["RemarksHeader"], dtHistory.Rows[i]["EntryUserName"], dtHistory.Rows[i]["EnteryDate"], dtHistory.Rows[i]["ModifyUserName"], dtHistory.Rows[i]["ModifyDate"], dtHistory.Rows[i]["NoOfAttachments"]);
				}
				GrdHistory.DataSource = dt;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.ClearStructure();
				GrdHistoryDetail.ClearStructure();
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
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistory, 3, 3);
			GrdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			GrdHistory.RootTable.Columns["Supplier"].Width = Constants.InventoryConstants.SupplierName + 20;
			GrdHistory.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName + 20;
			GrdHistory.RootTable.Columns["RefDocNos"].Width = Constants.InventoryConstants.DocNoConstant + 30;
			GrdHistory.RootTable.Columns["VehicleNos"].Width = Constants.InventoryConstants.VehicleNoConstant + 30;
			GrdHistory.RootTable.Columns["PurDeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdHistory.RootTable.Columns["SaleDeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["SaleBillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["PLAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			GrdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GrdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			GrdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GrdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["SaleBillAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["SaleBillAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["SaleBillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["SaleBillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["PLAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["PLAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["PLAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["PLAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridEX_Helper.AddButton(GrdHistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(GrdHistory, "Print", "1056 Slip", 70, 1);
			GridEX_Helper.AddButton(GrdHistory, "IncomeSlip", "Income Slip", 85, 2);
			GridEX_Helper.AddButton(GrdHistory, "PurchaseSlip", "Purchase Slip", 90, 3);
			GridEX_Helper.AddButton(GrdHistory, "SaleSlip", "Sale Slip", 70, 4);
			GridEX_Helper.AddButton(GrdHistory, "SaleSlipII", "Sale Slip II", 80, 4);
			GridEX_Helper.AddButton(GrdHistory, "Voucher", "Voucher", 65, 5);
			GrdHistory.FrozenColumns = 6;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && GrdHistory.CurrentColumn != null && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && GrdHistory.CurrentColumn != null)
			{
				int CurrId = Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
				else if (e.Column.Key == "Print")
				{
					OpenSlip(CurrId);
				}
				else if (e.Column.Key == "IncomeSlip")
				{
					OpenSlip(CurrId, 2);
				}
				else if (e.Column.Key == "PurchaseSlip")
				{
					OpenSlip(CurrId, 3);
				}
				else if (e.Column.Key == "SaleSlip")
				{
					OpenSlip(CurrId, 4);
				}
				else if (e.Column.Key == "SaleSlipII")
				{
					OpenSlip(CurrId, 5);
				}
				else if (e.Column.Key == "Voucher")
				{
					OpenVoucher(CurrId, DocumentTypeId);
				}
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
				fromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				fromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				fromdate.Value = first;
				ToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				fromdate.Value = first2;
				ToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				ToDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ParameterFill();
			txtOrderNoFrom.Text = string.Empty;
			txtOrderNoTo.Text = string.Empty;
			((Control)(object)cmbtradAcHistory).Text = string.Empty;
			((Control)(object)cmbsupplierhistory).Text = string.Empty;
			((Control)(object)cmbcusthistory).Text = string.Empty;
			cmbperemeter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtBindCombo.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = InvCommAgentTradeBill.GetDataForDropDownFromCommOrder(obj);
			return dtBindCombo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtAccount = new DataTable();
			dtAccount.Columns.Add("Id");
			dtAccount.Columns.Add("name");
			DataTable dtsupplier = dtAccount.Clone();
			DataTable dtcustomer = dtAccount.Clone();
			if (dtcombo.Rows.Count > 0)
			{
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "TradingAccount")
					{
						dtAccount.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Supplier")
					{
						dtsupplier.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Customer")
					{
						dtcustomer.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
				}
				if (dtAccount.Rows.Count > 0)
				{
					DDL.BindDDL(dtAccount, cmbtradAcHistory, "Id", "name", "Trading Account", ZeroIndex: false);
				}
				if (dtsupplier.Rows.Count > 0)
				{
					DDL.BindDDL(dtsupplier, cmbsupplierhistory, "Id", "name", "Supplier", ZeroIndex: false);
				}
				if (dtcustomer.Rows.Count > 0)
				{
					DDL.BindDDL(dtcustomer, cmbcusthistory, "Id", "name", "Customer", ZeroIndex: false);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbtradAcHistory, dtAccount, "Id", "Name", "Trading Account", AllColumns: true, Conversion.ToInt(cmbtradAcHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbsupplierhistory, dtsupplier, "Id", "Name", "Supplier Name", AllColumns: true, Conversion.ToInt(cmbsupplierhistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbcusthistory, dtcustomer, "Id", "Name", "Customer Name", AllColumns: true, Conversion.ToInt(cmbcusthistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			((UltraGridBase)cmbtradAcHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbsupplierhistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbcusthistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "GrdHistory")
			{
				cmbperemeter.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				ReadById(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				InvCommAgentTradeBill byID = InvCommAgentTradeBill.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
				DataTable dthistorydetail = dtDetail.Clone();
				TradeBill_Helper.FillDetailtableFromListCommonForReadById(byID, dthistorydetail);
				GrdHistoryDetail.DataSource = dthistorydetail;
				GrdHistoryDetail.RetrieveStructure();
				grdSettings(GrdHistoryDetail);
				ctrlGrdBar3_Load(null, null);
				DataTable dtPExp = dtPurchaseOtherExpense.Clone();
				AddRowsInPurchaseExpenseGrid(dtPExp);
				TradeBill_Helper.FillPurchaseExpenseTableFromListCommonForReadById(byID, dtPExp);
				GrdHistoryPurchaseExpenseDetail.DataSource = dtPExp;
				GrdHistoryPurchaseExpenseDetail.RetrieveStructure();
				grdPurchaseOtherExpensesSettings(GrdHistoryPurchaseExpenseDetail);
				DataTable dtSExp = dtSaleOtherExpense.Clone();
				AddRowsInSaleExpenseGrid(dtSExp);
				TradeBill_Helper.FillSaleExpenseTableFromListCommonForReadById(byID, dtSExp);
				GrdHistorySaleExpenseDetail.DataSource = dtSExp;
				GrdHistorySaleExpenseDetail.RetrieveStructure();
				grdSaleOtherExpenseSettings(GrdHistorySaleExpenseDetail);
				DataTable dtPFreight = dtPurchasePmExpense.Clone();
				dtPFreight.Columns.Add("AccountTitle", typeof(string));
				dtPFreight.Columns.Add("PackingType", typeof(string));
				dtPFreight.Columns.Add("PmItemName", typeof(string));
				TradeBill_Helper.FillPurchaseFreightTableFromListCommonForReadById(byID, dtPFreight, AddExtraColumns: true);
				GrdHistoryPurchaseFreightDetail.DataSource = dtPFreight;
				GrdHistoryPurchaseFreightDetail.RetrieveStructure();
				grdPurchasePmExpenseSettings(GrdHistoryPurchaseFreightDetail);
				GrdHistoryPurchaseFreightDetail.RootTable.Columns["AccountId"].Visible = false;
				GrdHistoryPurchaseFreightDetail.RootTable.Columns["PackingTypeId"].Visible = false;
				GrdHistoryPurchaseFreightDetail.RootTable.Columns["PmItemId"].Visible = false;
				DataTable dtPm = dtSalePmExpense.Clone();
				dtPm.Columns.Add("AccountTitle", typeof(string));
				dtPm.Columns.Add("PackingType", typeof(string));
				dtPm.Columns.Add("PmItemName", typeof(string));
				TradeBill_Helper.FillSalePmTableFromListCommonForReadById(byID, dtPm, AddExtraColumns: true);
				GrdHistorySalePmDetail.DataSource = dtPm;
				GrdHistorySalePmDetail.RetrieveStructure();
				grdSalePmExpenseSettings(GrdHistorySalePmDetail);
				GrdHistorySalePmDetail.RootTable.Columns["AccountId"].Visible = false;
				GrdHistorySalePmDetail.RootTable.Columns["PackingTypeId"].Visible = false;
				GrdHistorySalePmDetail.RootTable.Columns["PmItemId"].Visible = false;
				DataTable dtPc = dtPurchaseCommission.Clone();
				dtPc.Columns.Add("CommissionAgent", typeof(string));
				dtPc.Columns.Add("CommissionType", typeof(string));
				dtPc.Columns.Add("Uom", typeof(string));
				dtPc.Columns.Add("DebitAccount", typeof(string));
				TradeBill_Helper.FillPurchaseCommissionTableFromListCommonForReadById(byID, dtPc, AddExtraColumns: true);
				GrdHistoryPurchaseCommissionDetail.DataSource = dtPc;
				GrdHistoryPurchaseCommissionDetail.RetrieveStructure();
				grdPurchaseCommissionDetailSetting(GrdHistoryPurchaseCommissionDetail);
				GrdHistoryPurchaseCommissionDetail.RootTable.Columns["CommissionAgentId"].Visible = false;
				GrdHistoryPurchaseCommissionDetail.RootTable.Columns["CommissionTypeId"].Visible = false;
				GrdHistoryPurchaseCommissionDetail.RootTable.Columns["CommissionUomId"].Visible = false;
				GrdHistoryPurchaseCommissionDetail.RootTable.Columns["DebitAccountId"].Visible = false;
				DataTable dtPB = dtPurchaseBrokery.Clone();
				dtPB.Columns.Add("BrokeryAgent", typeof(string));
				dtPB.Columns.Add("BrokeryType", typeof(string));
				dtPB.Columns.Add("Uom", typeof(string));
				dtPB.Columns.Add("DebitAccount", typeof(string));
				TradeBill_Helper.FillPurchaseBrokeryTableFromListCommonForReadById(byID, dtPB, AddExtraColumns: true);
				GrdHistoryPurchaseBrokeryDetail.DataSource = dtPB;
				GrdHistoryPurchaseBrokeryDetail.RetrieveStructure();
				grdPurchaseBrokeryDetailSetting(GrdHistoryPurchaseBrokeryDetail);
				GrdHistoryPurchaseBrokeryDetail.RootTable.Columns["BrokeryAgentId"].Visible = false;
				GrdHistoryPurchaseBrokeryDetail.RootTable.Columns["BrokeryTypeId"].Visible = false;
				GrdHistoryPurchaseBrokeryDetail.RootTable.Columns["BrokeryUomId"].Visible = false;
				GrdHistoryPurchaseBrokeryDetail.RootTable.Columns["DebitAccountId"].Visible = false;
				DataTable dtSc = dtSaleCommission.Clone();
				dtSc.Columns.Add("CommissionAgent", typeof(string));
				dtSc.Columns.Add("CommissionType", typeof(string));
				dtSc.Columns.Add("Uom", typeof(string));
				dtSc.Columns.Add("DebitAccount", typeof(string));
				TradeBill_Helper.FillSaleCommissionTableFromListCommonForReadById(byID, dtSc, AddExtraColumns: true);
				GrdHistorySaleCommissionDetail.DataSource = dtSc;
				GrdHistorySaleCommissionDetail.RetrieveStructure();
				grdSaleCommissionDetailSetting(GrdHistorySaleCommissionDetail);
				GrdHistorySaleCommissionDetail.RootTable.Columns["CommissionAgentId"].Visible = false;
				GrdHistorySaleCommissionDetail.RootTable.Columns["CommissionTypeId"].Visible = false;
				GrdHistorySaleCommissionDetail.RootTable.Columns["CommissionUomId"].Visible = false;
				GrdHistorySaleCommissionDetail.RootTable.Columns["DebitAccountId"].Visible = false;
				DataTable dtSB = dtSaleBrokery.Clone();
				dtSB.Columns.Add("BrokeryAgent", typeof(string));
				dtSB.Columns.Add("BrokeryType", typeof(string));
				dtSB.Columns.Add("Uom", typeof(string));
				dtSB.Columns.Add("DebitAccount", typeof(string));
				TradeBill_Helper.FillSaleBrokeryTableFromListCommonForReadById(byID, dtSB, AddExtraColumns: true);
				GrdHistorySaleBrokeryDetail.DataSource = dtSB;
				GrdHistorySaleBrokeryDetail.RetrieveStructure();
				grdSaleBrokeryDetailSetting(GrdHistorySaleBrokeryDetail);
				GrdHistorySaleBrokeryDetail.RootTable.Columns["BrokeryAgentId"].Visible = false;
				GrdHistorySaleBrokeryDetail.RootTable.Columns["BrokeryTypeId"].Visible = false;
				GrdHistorySaleBrokeryDetail.RootTable.Columns["BrokeryUomId"].Visible = false;
				GrdHistorySaleBrokeryDetail.RootTable.Columns["DebitAccountId"].Visible = false;
				DataTable dtPP = dtPurchasePaymentDetail.Clone();
				dtPP.Columns.Add("PaymentTerm", typeof(string));
				dtPP.Columns.Add("BaseDateType", typeof(string));
				TradeBill_Helper.FillPurchasePaymentTableFromListCommonForReadById(byID, dtPP, AddExtraColumns: true);
				GrdHistoryPaymentDetailPurchase.DataSource = dtPP;
				GrdHistoryPaymentDetailPurchase.RetrieveStructure();
				grdPaymentDetailPurchaseSetting(GrdHistoryPaymentDetailPurchase);
				GrdHistoryPaymentDetailPurchase.RootTable.Columns["PaymentTermId"].Visible = false;
				GrdHistoryPaymentDetailPurchase.RootTable.Columns["BaseDateTypeId"].Visible = false;
				DataTable dtSP = dtSalePaymentDetail.Clone();
				dtSP.Columns.Add("PaymentTerm", typeof(string));
				dtSP.Columns.Add("BaseDateType", typeof(string));
				TradeBill_Helper.FillSalePaymentTableFromListCommonForReadById(byID, dtSP, AddExtraColumns: true);
				GrdHistoryPaymentDetailSale.DataSource = dtSP;
				GrdHistoryPaymentDetailSale.RetrieveStructure();
				grdPaymentDetailSaleSetting(GrdHistoryPaymentDetailSale);
				GrdHistoryPaymentDetailSale.RootTable.Columns["PaymentTermId"].Visible = false;
				GrdHistoryPaymentDetailSale.RootTable.Columns["BaseDateTypeId"].Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnGrnFormHistory_Click(object sender, EventArgs e)
	{
		try
		{
			new CommissionAgentTrade_Register(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateBuyerNetWeight()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtBuyerFirstWeight.Text);
			decimal TareWeight = Conversion.ToDecimal((object)txtBuyerSecondWeight.Text);
			decimal NetWeight = Math.Abs(num - TareWeight);
			txtBuyerWeight.Text = NetWeight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateSupplierNetWeight()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtSupplierFirstWeight.Text);
			decimal TareWeight = Conversion.ToDecimal((object)txtSupplierSecondWeight.Text);
			decimal NetWeight = Math.Abs(num - TareWeight);
			txtSupplierWeight.Text = NetWeight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateLoadWeightDifference()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtBuyerFirstWeight.Text);
			decimal supplierLoadWeight = Conversion.ToDecimal((object)txtSupplierFirstWeight.Text);
			decimal diffWeight = Math.Abs(num - supplierLoadWeight);
			txtFirstWtDifference.Text = diffWeight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTareWeightDifference()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtBuyerSecondWeight.Text);
			decimal supplierWeight = Conversion.ToDecimal((object)txtSupplierSecondWeight.Text);
			decimal diffWeight = Math.Abs(num - supplierWeight);
			txtSecondWtDifference.Text = diffWeight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateNetWeightDifference()
	{
		try
		{
			decimal num = Conversion.ToDecimal((object)txtBuyerWeight.Text);
			decimal supplierWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text);
			decimal diffWeight = Math.Abs(num - supplierWeight);
			txtNetWeightDifference.Text = diffWeight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBuyerFirstWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtBuyerFirstWeight.Text = Conversion.ToDecimal((object)txtBuyerFirstWeight.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBuyerFirstWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBuyerNetWeight();
			CalculateLoadWeightDifference();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBuyerSecondWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtBuyerFirstWeight.Text = Conversion.ToDecimal((object)txtBuyerFirstWeight.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBuyerSecondWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBuyerNetWeight();
			CalculateTareWeightDifference();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierFirstWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtSupplierFirstWeight.Text = Conversion.ToDecimal((object)txtSupplierFirstWeight.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierFirstWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateSupplierNetWeight();
			CalculateLoadWeightDifference();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierSecondWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			txtSupplierSecondWeight.Text = Conversion.ToDecimal((object)txtSupplierSecondWeight.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierSecondWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateSupplierNetWeight();
			CalculateTareWeightDifference();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBuyerWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetWeightDifference();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetWeightDifference();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateGrossWeight()
	{
		try
		{
			if (Conversion.ToDouble((object)txtQuantity.Text) > 0.0 && ((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToInt(CmbPackUom.Value) > 0)
			{
				double num = double.Parse(txtQuantity.Text);
				double PackUom = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				double Weight = num * PackUom;
				txtGrossWeightPurchase.Text = Weight.ToString("#,##0.##");
				txtGrossWeightSale.Text = Weight.ToString("#,##0.##");
			}
			else
			{
				txtGrossWeightPurchase.Text = 0.ToString();
				txtGrossWeightSale.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseEbCalculations()
	{
		try
		{
			double EBUnit = Conversion.ToDouble((object)txtEBUnitPurchase.Text);
			double EBTotal = Conversion.ToDouble((object)txtEBTotalPurchase.Text);
			double factor = Conversion.ToDouble((object)txtQuantity.Text);
			string activeTag = Convert.ToString(base.ActiveControl?.AccessibleName);
			double calcValue = 0.0;
			if (factor <= 0.0)
			{
				txtEBUnitPurchase.Text = "0";
				txtEBTotalPurchase.Text = "0";
			}
			else if (activeTag == Convert.ToString(txtEBUnitPurchase.AccessibleName))
			{
				calcValue = ((EBUnit > 0.0) ? (EBUnit * factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEBTotalPurchase.Text = calcValue.ToString("#,##0.###");
				CalculateByEbUnitPur = true;
				CalculateByEbTotalPur = false;
			}
			else if (activeTag == Convert.ToString(txtEBTotalPurchase.AccessibleName))
			{
				calcValue = ((EBTotal > 0.0) ? (EBTotal / factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEBUnitPurchase.Text = calcValue.ToString("#,##0.###");
				CalculateByEbUnitPur = false;
				CalculateByEbTotalPur = true;
			}
			else if (CalculateByEbTotalPur)
			{
				calcValue = ((EBTotal > 0.0) ? (EBTotal / factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEBUnitPurchase.Text = calcValue.ToString("#,##0.###");
			}
			else if (CalculateByEbUnitPur)
			{
				calcValue = ((EBUnit > 0.0) ? (EBUnit * factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEBTotalPurchase.Text = calcValue.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleEbCalculations()
	{
		try
		{
			double EBUnit = Conversion.ToDouble((object)txtEBUnitSale.Text);
			double EBTotal = Conversion.ToDouble((object)txtEBTotalSale.Text);
			double factor = Conversion.ToDouble((object)txtQuantity.Text);
			string activeTag = Convert.ToString(base.ActiveControl?.AccessibleName);
			double calcValue = 0.0;
			if (factor <= 0.0)
			{
				txtEBUnitSale.Text = "0";
				txtEBTotalSale.Text = "0";
			}
			else if (activeTag == Convert.ToString(txtEBUnitSale.AccessibleName))
			{
				calcValue = ((EBUnit > 0.0) ? (EBUnit * factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEBTotalSale.Text = calcValue.ToString("#,##0.###");
				CalculateByEbUnitSale = true;
				CalculateByEbTotalSale = false;
			}
			else if (activeTag == Convert.ToString(txtEBTotalSale.AccessibleName))
			{
				calcValue = ((EBTotal > 0.0) ? (EBTotal / factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEBUnitSale.Text = calcValue.ToString("#,##0.###");
				CalculateByEbUnitSale = false;
				CalculateByEbTotalSale = true;
			}
			else if (CalculateByEbTotalSale)
			{
				calcValue = ((EBTotal > 0.0) ? (EBTotal / factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEBUnitSale.Text = calcValue.ToString("#,##0.###");
			}
			else if (CalculateByEbUnitSale)
			{
				calcValue = ((EBUnit > 0.0) ? (EBUnit * factor) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtEBTotalSale.Text = calcValue.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateBillWeightSale()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtGrossWeightSale.Text);
			double EbWeightPurchase = Conversion.ToDouble((object)txtEBTotalSale.Text);
			double AddLess = Conversion.ToDouble((object)txtAddlessWtSales.Text);
			txtBillWeightSale.Text = (num - EbWeightPurchase + AddLess).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateBillWeightPurchase()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtGrossWeightPurchase.Text);
			double EbWeightPurchase = Conversion.ToDouble((object)txtEBTotalPurchase.Text);
			double AddLess = Conversion.ToDouble((object)txtAddlessWtPurchase.Text);
			txtBillWeightPurchase.Text = (num - EbWeightPurchase + AddLess).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculatePurchaseAmount()
	{
		try
		{
			if (Conversion.ToDouble((object)txtPurchaseNetRate.Text) > 0.0 && Conversion.ToDouble((object)txtBillWeightPurchase.Text) > 0.0 && ((UltraGridBase)CmbRateUomPurchase).ActiveRow != null && Conversion.ToInt(CmbRateUomPurchase.Value) > 0)
			{
				double num = double.Parse(txtBillWeightPurchase.Text);
				double purchaseRate = Conversion.ToDouble((object)txtPurchaseNetRate.Text);
				double RateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomPurchase).SelectedRow.Cells[2].Value);
				double PurAmount = num / RateUom * purchaseRate;
				txtItemAmountPurchase.Text = PurAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtItemAmountPurchase.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateSaleAmount()
	{
		try
		{
			if (Conversion.ToDouble((object)txtSaleNetRate.Text) > 0.0 && Conversion.ToDouble((object)txtBillWeightSale.Text) > 0.0 && ((UltraGridBase)CmbRateUomSale).ActiveRow != null && Conversion.ToInt(CmbRateUomSale.Value) > 0)
			{
				double num = double.Parse(txtBillWeightSale.Text);
				double SaleRate = Conversion.ToDouble((object)txtSaleNetRate.Text);
				double RateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomSale).SelectedRow.Cells[2].Value);
				double SaleAmount = num / RateUom * SaleRate;
				txtItemAmountSale.Text = SaleAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtItemAmountSale.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTaxAmountPurchase()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtItemAmountPurchase.Text);
			double TaxPercent = 0.0;
			double TaxAmt = 0.0;
			double TotalAmount = 0.0;
			if (((UltraGridBase)CmbTaxNamePurchase).ActiveRow != null)
			{
				TaxPercent = ((((UltraDropDownBase)CmbTaxNamePurchase).SelectedRow != null) ? Conversion.ToDouble(((UltraDropDownBase)CmbTaxNamePurchase).SelectedRow.Cells[2].Value) : 0.0);
			}
			TaxAmt = num * TaxPercent / 100.0;
			TotalAmount = num + TaxAmt;
			txtTaxPercntPurchase.Text = TaxPercent.ToString("#,##0.###");
			txtTaxAmountPurchase.Text = Math.Round(TaxAmt, 3, MidpointRounding.AwayFromZero).ToString("#,##0.###");
			txtTotalAmountPurchase.Text = Math.Round(TotalAmount, 3, MidpointRounding.AwayFromZero).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateTaxAmountSale()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtItemAmountSale.Text);
			double TaxPercent = 0.0;
			double TaxAmt = 0.0;
			double TotalAmount = 0.0;
			if (((UltraGridBase)CmbTaxNameSale).ActiveRow != null)
			{
				TaxPercent = ((Conversion.ToDouble((object)txtTaxPercntSale.Text) > 0.0) ? Conversion.ToDouble((object)txtTaxPercntSale.Text) : ((((UltraDropDownBase)CmbTaxNameSale).SelectedRow != null) ? Conversion.ToDouble(((UltraDropDownBase)CmbTaxNameSale).SelectedRow.Cells[2].Value) : 0.0));
			}
			TaxAmt = num * TaxPercent / 100.0;
			TotalAmount = num + TaxAmt;
			txtTaxPercntSale.Text = TaxPercent.ToString("#,##0.###");
			txtTaxAmountSale.Text = Math.Round(TaxAmt, 3, MidpointRounding.AwayFromZero).ToString("#,##0.###");
			txtTotalAmountSale.Text = Math.Round(TotalAmount, 3, MidpointRounding.AwayFromZero).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbPackUom_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQuantity_TextChanged(object sender, EventArgs e)
	{
		try
		{
			PurchaseEbCalculations();
			SaleEbCalculations();
			CalculateGrossWeight();
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
			CalculateBillWeightPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBPerUnit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			PurchaseEbCalculations();
			CalculateBillWeightPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBTotalPurchase_TextChanged(object sender, EventArgs e)
	{
		try
		{
			PurchaseEbCalculations();
			CalculateBillWeightPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAddlessWtPurchase_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSupplierRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			decimal Rate = default(decimal);
			decimal addLess = default(decimal);
			decimal netRate = default(decimal);
			decimal.TryParse(txtPurchaseRateWithoutAddLess.Text, out Rate);
			decimal.TryParse(txtPurchaseRateAddLess.Text, out addLess);
			netRate = Rate + addLess;
			txtPurchaseNetRate.Text = netRate.ToString("#,##0.###");
			CalculatePurchaseAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPurchaseRateAddLess_TextChanged(object sender, EventArgs e)
	{
		try
		{
			decimal saleRate = default(decimal);
			decimal addLess = default(decimal);
			decimal netRate = default(decimal);
			decimal.TryParse(txtPurchaseRateWithoutAddLess.Text, out saleRate);
			decimal.TryParse(txtPurchaseRateAddLess.Text, out addLess);
			netRate = saleRate + addLess;
			txtPurchaseNetRate.Text = netRate.ToString("#,##0.###");
			CalculatePurchaseAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPurchaseNetRate_TextChanged(object sender, EventArgs e)
	{
		CalculatePurchaseAmount();
	}

	private void CmbRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculatePurchaseAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBillWeightPurchase_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculatePurchaseAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSaleGrossWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBPerUnitSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			SaleEbCalculations();
			CalculateBillWeightSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBTotalSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			SaleEbCalculations();
			CalculateBillWeightSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAddlessWtSales_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBillWeightSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateSaleAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtCustomerRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			decimal saleRate = default(decimal);
			decimal addLess = default(decimal);
			decimal netRate = default(decimal);
			decimal.TryParse(txtSaleRateWithoutAddLess.Text, out saleRate);
			decimal.TryParse(txtSaleRateAddLess.Text, out addLess);
			netRate = saleRate + addLess;
			txtSaleNetRate.Text = netRate.ToString("#,##0.###");
			CalculateSaleAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSaleRateAddLess_TextChanged(object sender, EventArgs e)
	{
		try
		{
			decimal saleRate = default(decimal);
			decimal addLess = default(decimal);
			decimal netRate = default(decimal);
			decimal.TryParse(txtSaleRateWithoutAddLess.Text, out saleRate);
			decimal.TryParse(txtSaleRateAddLess.Text, out addLess);
			netRate = saleRate + addLess;
			txtSaleNetRate.Text = netRate.ToString("#,##0.###");
			CalculateSaleAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSaleNetRate_TextChanged(object sender, EventArgs e)
	{
		CalculateSaleAmount();
	}

	private void CmbRateUomSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateSaleAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemAmountPurchase_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmountPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemAmountSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmountSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommPercent_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtBrokeryPercent_TextChanged(object sender, EventArgs e)
	{
	}

	private void CustomerBillAmount()
	{
		try
		{
			decimal SaleAmount = default(decimal);
			decimal TaxAmount = default(decimal);
			decimal FreightAmountPurchase = default(decimal);
			decimal ExpAmountSale = default(decimal);
			decimal PMAmountSale = default(decimal);
			int customerId = Conversion.ToInt(CmbCustomer.Value);
			int customerglId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString((object)customerId)));
			SaleAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "SaleAmount"));
			TaxAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "SaleTaxAmount"));
			if (((UltraGridBase)CmbCustomer).ActiveRow != null && Conversion.ToInt(CmbCustomer.Value) > 0 && grdPurchasePmExpense.RowCount > 0)
			{
				FreightAmountPurchase = (from GridEXRow row in grdPurchasePmExpense.GetRows()
					where Conversion.ToInt(row.Cells["AccountId"].Value) == customerglId
					select (decimal)row.Cells["Amount"].Value).Sum();
				if (FreightAmountPurchase != 0m)
				{
					SaleAmount -= FreightAmountPurchase;
				}
			}
			ExpAmountSale = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdSaleOtherExpense, "Amount"));
			if (ExpAmountSale != 0m)
			{
				SaleAmount += ExpAmountSale;
			}
			SaleAmount += TaxAmount;
			bool isSaleDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text));
			if (FormHelper.DeliveryTermIsLoad(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text)))
			{
				PMAmountSale = Conversion.ToDecimal(grdSalePmExpenseSale.GetTotal(grdSalePmExpenseSale.RootTable.Columns["Amount"], (AggregateFunction)2));
				SaleAmount += PMAmountSale;
			}
			else if (isSaleDelivered)
			{
				PMAmountSale = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSumWithWhere(grdSalePmExpenseSale, "Amount", "AccountId", customerglId.ToString(), CheckValue: true));
				if (PMAmountSale != 0m)
				{
					SaleAmount -= PMAmountSale;
				}
			}
			ApplyCommissionOrBrokery(grdPurchaseCommission, "CommissionAgentId", "DebitAccountId", "CommissionAmount", customerId, ref SaleAmount, isCustomer: true);
			ApplyCommissionOrBrokery(grdSaleCommission, "CommissionAgentId", "DebitAccountId", "CommissionAmount", customerId, ref SaleAmount, isCustomer: true);
			ApplyCommissionOrBrokery(grdPurchaseBrokery, "BrokeryAgentId", "DebitAccountId", "BrokeryAmount", customerId, ref SaleAmount, isCustomer: true);
			ApplyCommissionOrBrokery(grdSaleBrokery, "BrokeryAgentId", "DebitAccountId", "BrokeryAmount", customerId, ref SaleAmount, isCustomer: true);
			decimal saleFreight = Conversion.ToDecimal((object)txtTotalFreightSale.Text);
			if (isSaleDelivered)
			{
				SaleAmount -= saleFreight;
			}
			decimal WhtTaxAmount = default(decimal);
			decimal WhtTaxPrcnt = Conversion.ToDecimal((object)txtWhtTaxPercentSale.Text);
			if (WhtTaxPrcnt > 0m)
			{
				WhtTaxAmount = SaleAmount * WhtTaxPrcnt / 100m;
			}
			txtwhtTaxAmountSale.Text = WhtTaxAmount.ToString(clsGlobalVariables.stringFormatboth);
			SaleAmount -= WhtTaxAmount;
			txtSaleBillAmount.Text = SaleAmount.ToString(clsGlobalVariables.stringFormatboth);
			BillProportionForSale();
			CalculateProfitLoss();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierBillAmount()
	{
		try
		{
			bool isPurchaseXFactory = FormHelper.DeliveryTermIsLoad(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
			bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
			decimal PurchaseAmount = default(decimal);
			decimal ExpAmountPurchase = default(decimal);
			decimal TaxAmount = default(decimal);
			decimal FreightAmountPurchase = default(decimal);
			decimal PMAmountSale = default(decimal);
			int supplierId = Conversion.ToInt(CmbSupplier.Value);
			int supplierglId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString((object)supplierId)));
			PurchaseAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "PurchaseAmount"));
			TaxAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "PurchaseTaxAmount"));
			ExpAmountPurchase = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPurchaseOtherExpenses, "Amount"));
			PurchaseAmount += ExpAmountPurchase;
			PurchaseAmount += TaxAmount;
			if (((UltraGridBase)CmbSupplier).ActiveRow != null && Conversion.ToInt(CmbSupplier.Value) > 0 && grdPurchasePmExpense.RowCount > 0)
			{
				if (isPurchaseXFactory)
				{
					FreightAmountPurchase = (from GridEXRow row in grdPurchasePmExpense.GetRows()
						where Conversion.ToInt(row.Cells["AccountId"].Value) == supplierglId
						select (decimal)row.Cells["Amount"].Value).Sum();
					PurchaseAmount += FreightAmountPurchase;
				}
				else if (isPurchaseDelivered)
				{
					FreightAmountPurchase = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPurchasePmExpense, "Amount"));
					PurchaseAmount -= FreightAmountPurchase;
				}
			}
			if (((UltraGridBase)CmbSupplier).ActiveRow != null && Conversion.ToInt(CmbSupplier.Value) > 0 && grdSalePmExpenseSale.RowCount > 0)
			{
				PMAmountSale = (from GridEXRow row in grdSalePmExpenseSale.GetRows()
					where Conversion.ToInt(row.Cells["AccountId"].Value) == supplierglId
					select (decimal)row.Cells["Amount"].Value).Sum();
				PurchaseAmount += PMAmountSale;
			}
			ApplyCommissionOrBrokery(grdPurchaseCommission, "CommissionAgentId", "DebitAccountId", "CommissionAmount", supplierId, ref PurchaseAmount);
			ApplyCommissionOrBrokery(grdSaleCommission, "CommissionAgentId", "DebitAccountId", "CommissionAmount", supplierId, ref PurchaseAmount);
			ApplyCommissionOrBrokery(grdPurchaseBrokery, "BrokeryAgentId", "DebitAccountId", "BrokeryAmount", supplierId, ref PurchaseAmount);
			ApplyCommissionOrBrokery(grdSaleBrokery, "BrokeryAgentId", "DebitAccountId", "BrokeryAmount", supplierId, ref PurchaseAmount);
			decimal purchaseFreight = Conversion.ToDecimal((object)txtTotalFreightPurchase.Text);
			if (isPurchaseDelivered)
			{
				PurchaseAmount -= purchaseFreight;
			}
			decimal WhtTaxAmount = default(decimal);
			decimal WhtTaxPrcnt = Conversion.ToDecimal((object)txtWhtTaxPercentPurchase.Text);
			if (WhtTaxPrcnt > 0m)
			{
				WhtTaxAmount = GetWeightForPurchaseTaxCalculations() * WhtTaxPrcnt;
			}
			txtWhtTaxAmountPurchase.Text = WhtTaxAmount.ToString(clsGlobalVariables.stringFormatboth);
			PurchaseAmount -= WhtTaxAmount;
			txtPurchaseBillAmount.Text = PurchaseAmount.ToString(clsGlobalVariables.stringFormatboth);
			BillProportionForPurchase();
			CalculateProfitLoss();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private decimal GetWeightForPurchaseTaxCalculations()
	{
		decimal totalWeight = default(decimal);
		GridEXRow[] dataRows = grd.GetDataRows();
		foreach (GridEXRow obj in dataRows)
		{
			decimal billWeight = Conversion.ToDecimal(obj.Cells["PurchaseBillWeight"].Value);
			decimal purchaseRateEquivalent = Conversion.ToDecimal(obj.Cells["PurchaseRateEquivalent"].Value);
			if (purchaseRateEquivalent > 0m)
			{
				totalWeight += billWeight / purchaseRateEquivalent;
			}
		}
		return totalWeight;
	}

	private void CalculateProfitLoss()
	{
		try
		{
			bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
			bool isSaleDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text));
			double TradingDebitAmount = 0.0;
			double TradingCreditAmount = 0.0;
			int tradingAccountGlId = Conversion.ToInt(CmbTradingAccount.Value);
			double FreightGrid = 0.0;
			double PmSaleGrid = 0.0;
			TradingDebitAmount += ((grd.GetRows().Count() > 0) ? Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["PurchaseAmount"], (AggregateFunction)2)) : 0.0);
			TradingCreditAmount += ((grd.GetRows().Count() > 0) ? Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["SaleAmount"], (AggregateFunction)2)) : 0.0);
			TradingDebitAmount += ((grdPurchaseOtherExpenses.GetRows().Count() > 0) ? Conversion.ToDouble(grdPurchaseOtherExpenses.GetTotal(grdPurchaseOtherExpenses.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
			if (!isPurchaseDelivered)
			{
				FreightGrid = GridEX_Helper.GetColumnSum(grdPurchasePmExpense, "Amount");
				TradingDebitAmount += FreightGrid;
			}
			else if (isPurchaseDelivered)
			{
				FreightGrid = GridEX_Helper.GetColumnSumWithWhere(grdPurchasePmExpense, "Amount", "AccountId", Conversion.ToString(CmbTradingAccount.Value), CheckValue: true);
				TradingCreditAmount += FreightGrid;
			}
			double PurchaseFreight = Conversion.ToDouble((object)txtTotalFreightPurchase.Text.Trim());
			double SaleFreight = Conversion.ToDouble((object)txtTotalFreightSale.Text.Trim());
			TradingDebitAmount += SaleFreight;
			TradingCreditAmount += PurchaseFreight;
			TradingCreditAmount += ((grdSaleOtherExpense.GetRows().Count() > 0) ? Conversion.ToDouble(grdSaleOtherExpense.GetTotal(grdSaleOtherExpense.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
			if (isSaleDelivered)
			{
				PmSaleGrid = GridEX_Helper.GetColumnSum(grdSalePmExpenseSale, "Amount");
				TradingDebitAmount += PmSaleGrid;
			}
			else if (!isSaleDelivered)
			{
				PmSaleGrid = GridEX_Helper.GetColumnSumWithWhere(grdSalePmExpenseSale, "Amount", "AccountId", Conversion.ToString(CmbTradingAccount.Value), CheckValue: true);
				TradingCreditAmount += PmSaleGrid;
			}
			ApplyCommissionOrBrokeryForPL(grdPurchaseCommission, "CommissionAgentId", "DebitAccountId", "CommissionAmount", tradingAccountGlId, ref TradingDebitAmount, ref TradingCreditAmount);
			ApplyCommissionOrBrokeryForPL(grdSaleCommission, "CommissionAgentId", "DebitAccountId", "CommissionAmount", tradingAccountGlId, ref TradingDebitAmount, ref TradingCreditAmount);
			ApplyCommissionOrBrokeryForPL(grdPurchaseBrokery, "BrokeryAgentId", "DebitAccountId", "BrokeryAmount", tradingAccountGlId, ref TradingDebitAmount, ref TradingCreditAmount);
			ApplyCommissionOrBrokeryForPL(grdSaleBrokery, "BrokeryAgentId", "DebitAccountId", "BrokeryAmount", tradingAccountGlId, ref TradingDebitAmount, ref TradingCreditAmount);
			double PLAmount = TradingDebitAmount - TradingCreditAmount;
			txtProfitLoss.Text = PLAmount.ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPurchaseBillAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculatePaymentDetailGrid(grdPaymentDetailPurchase);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSaleBillAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculatePaymentDetailGrid(grdPaymentDetailSale);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryTermSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FreightHandling(Conversion.ToDecimal((object)txtFreightAmountPurchase.Text), Conversion.ToDecimal((object)txtFreightAmountSale.Text));
			CustomerBillAmount();
			SupplierBillAmount();
			ExpenseProportionForPurchase();
			FreightProportion();
			ExpenseProportionForSale();
			BillProportionForSale();
			PLAmountProportion();
			CalculateHeaderRates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryTerms_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FreightHandling(Conversion.ToDecimal((object)txtFreightAmountPurchase.Text), Conversion.ToDecimal((object)txtFreightAmountSale.Text));
			CustomerBillAmount();
			SupplierBillAmount();
			ExpenseProportionForPurchase();
			FreightProportion();
			ExpenseProportionForSale();
			PLAmountProportion();
			CalculateHeaderRates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseProportionForPurchase()
	{
		try
		{
			double PurchaseAmount = ((grd.GetRows().Count() > 0) ? Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["PurchaseAmount"], (AggregateFunction)2)) : 0.0);
			double Expense = Conversion.ToDouble(grdPurchaseOtherExpenses.GetTotal(grdPurchaseOtherExpenses.RootTable.Columns["Amount"], (AggregateFunction)2));
			if (PurchaseAmount > 0.0)
			{
				double TotalDiffernce = Expense;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Amount = Conversion.ToDouble((object)obj.Cells["PurchaseAmount"].Value.ToString());
					obj.Cells["PurchaseExpenses"].Value = Math.Round(TotalDiffernce) / PurchaseAmount * Amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["PurchaseExpenses"].Value = 0;
				}
			}
			BillProportionForPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseProportionForSale()
	{
		try
		{
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			double SaleAmount = ((grd.GetRows().Count() > 0) ? Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["SaleAmount"], (AggregateFunction)2)) : 0.0);
			if (SaleAmount > 0.0)
			{
				double Expense = Conversion.ToDouble(grdSaleOtherExpense.GetTotal(grdSaleOtherExpense.RootTable.Columns["Amount"], (AggregateFunction)2));
				double PackingExpense = ((grdSalePmExpenseSale.GetRows().Count() > 0) ? Conversion.ToDouble(grdSalePmExpenseSale.GetTotal(grdSalePmExpenseSale.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
				if (((UltraGridBase)CmbTradingAccount).ActiveRow != null && Conversion.ToInt(CmbTradingAccount.Value) > 0)
				{
					PackingExpense = (from GridEXRow row in grdSalePmExpenseSale.GetRows()
						where Conversion.ToInt(row.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value)
						select (double)row.Cells["Amount"].Value).Sum();
				}
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					double TotalExpAmount = Expense + PackingExpense;
					TotalExpAmount = Math.Abs(TotalExpAmount);
					obj.BeginEdit();
					double Amount = Conversion.ToDouble(obj.Cells["SaleAmount"].Value);
					obj.Cells["SaleExpenses"].Value = TotalExpAmount / SaleAmount * Amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["SaleExpenses"].Value = 0;
				}
			}
			BillProportionForSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillProportionForPurchase()
	{
		try
		{
			bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
			if (!isPurchaseDelivered)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells["PurchaseNetAmount"].Value = Conversion.ToDouble(r.Cells["PurchaseAmount"].Value) + Conversion.ToDouble(r.Cells["PurchaseExpenses"].Value) + Conversion.ToDouble(r.Cells["PurchaseFreight"].Value) + Conversion.ToDouble(r.Cells["PurchaseCommission"].Value) + Conversion.ToDouble(r.Cells["PurchaseBrokery"].Value);
					r.EndEdit();
				}
			}
			else if (isPurchaseDelivered)
			{
				double Freight = 0.0;
				if (((UltraGridBase)CmbTradingAccount).ActiveRow != null && Conversion.ToInt(CmbTradingAccount.Value) > 0)
				{
					Freight = (from GridEXRow row in grdPurchasePmExpense.GetRows()
						where Conversion.ToInt(row.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value)
						select (double)row.Cells["Amount"].Value).Sum();
				}
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					r2.BeginEdit();
					r2.Cells["PurchaseNetAmount"].Value = Conversion.ToDouble(r2.Cells["PurchaseAmount"].Value) + Conversion.ToDouble(r2.Cells["PurchaseExpenses"].Value) - Freight + Conversion.ToDouble(r2.Cells["PurchaseCommission"].Value) + Conversion.ToDouble(r2.Cells["PurchaseBrokery"].Value);
					r2.EndEdit();
				}
			}
			PLAmountProportion();
			CalculateHeaderRates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillProportionForSale()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells["SaleNetAmount"].Value = Conversion.ToDouble(r.Cells["SaleAmount"].Value) + Conversion.ToDouble(r.Cells["SaleExpenses"].Value) + Conversion.ToDouble(r.Cells["SaleCommission"].Value) + Conversion.ToDouble(r.Cells["SaleBrokery"].Value);
					r.EndEdit();
				}
			}
			PLAmountProportion();
			CalculateHeaderRates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PLAmountProportion()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells["PLAmount"].Value = Conversion.ToDouble((object)r.Cells["PurchaseNetAmount"].Value.ToString()) - Conversion.ToDouble((object)r.Cells["SaleNetAmount"].Value.ToString());
					r.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["PLAmount"].Value = 0;
					obj.EndEdit();
				}
			}
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
			bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
			double NetWeight = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["PurchaseBillWeight"], (AggregateFunction)2));
			double Freight = Conversion.ToDouble(grdPurchasePmExpense.GetTotal(grdPurchasePmExpense.RootTable.Columns["Amount"], (AggregateFunction)2));
			if (Freight > 0.0)
			{
				if (!isPurchaseDelivered)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double Weight = Conversion.ToDouble((object)obj.Cells["PurchaseBillWeight"].Value.ToString());
						obj.Cells["PurchaseFreight"].Value = ((NetWeight > 0.0) ? (Freight / NetWeight * Weight) : 0.0);
						obj.EndEdit();
					}
					grd.RootTable.Columns["PurchaseFreight"].Visible = true;
					grd.RootTable.Columns["PurchaseFreightLess"].Visible = false;
				}
				else if (isPurchaseDelivered)
				{
					Freight = (from GridEXRow row in grdPurchasePmExpense.GetRows()
						where Conversion.ToInt(row.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value)
						select (double)row.Cells["Amount"].Value).Sum();
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						double Weight2 = Conversion.ToDouble((object)obj2.Cells["PurchaseBillWeight"].Value.ToString());
						obj2.Cells["PurchaseFreightLess"].Value = ((NetWeight > 0.0) ? (Freight / NetWeight * Weight2) : 0.0);
						obj2.EndEdit();
					}
					grd.RootTable.Columns["PurchaseFreight"].Visible = false;
					grd.RootTable.Columns["PurchaseFreightLess"].Visible = true;
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj3 in rows)
				{
					obj3.BeginEdit();
					obj3.Cells["PurchaseFreight"].Value = 0;
				}
			}
			BillProportionForPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalPurchaseFreight()
	{
		decimal bilty = Conversion.ToDecimal((object)txtFreightAmountPurchase.Text);
		decimal other = Conversion.ToDecimal((object)txtFreightAddLessPurchase.Text);
		txtTotalFreightPurchase.Text = (bilty + other).ToString("#,##0.###");
	}

	private void txtFreightAmountPurchase_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalPurchaseFreight();
	}

	private void txtFreightAddLessPurchase_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalPurchaseFreight();
	}

	private void txtTotalFreightPurchase_TextChanged(object sender, EventArgs e)
	{
		CustomerBillAmount();
		SupplierBillAmount();
		ExpenseProportionForPurchase();
		FreightProportion();
		ExpenseProportionForSale();
		PLAmountProportion();
	}

	private void CalculateTotalSaleFreight()
	{
		decimal bilty = Conversion.ToDecimal((object)txtFreightAmountSale.Text);
		decimal other = Conversion.ToDecimal((object)txtFreightAddLessSale.Text);
		txtTotalFreightSale.Text = (bilty + other).ToString("#,##0.###");
	}

	private void txtFreightAmountSale_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalSaleFreight();
	}

	private void txtFreightAddLessSale_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalSaleFreight();
	}

	private void txtTotalFreightSale_TextChanged(object sender, EventArgs e)
	{
		CustomerBillAmount();
		SupplierBillAmount();
		ExpenseProportionForPurchase();
		FreightProportion();
		ExpenseProportionForSale();
		PLAmountProportion();
	}

	private void txtFreightAmountPurchase_Leave(object sender, EventArgs e)
	{
		try
		{
			txtFreightAmountPurchase.Text = Conversion.ToDecimal((object)txtFreightAmountPurchase.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightAddLessPurchase_Leave(object sender, EventArgs e)
	{
		try
		{
			txtFreightAddLessPurchase.Text = Conversion.ToDecimal((object)txtFreightAddLessPurchase.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTotalFreightPurchase_Leave(object sender, EventArgs e)
	{
		try
		{
			txtTotalFreightPurchase.Text = Conversion.ToDecimal((object)txtTotalFreightPurchase.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightAmountSale_Leave(object sender, EventArgs e)
	{
		try
		{
			txtFreightAmountSale.Text = Conversion.ToDecimal((object)txtFreightAmountSale.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightAddLessSale_Leave(object sender, EventArgs e)
	{
		try
		{
			txtFreightAddLessSale.Text = Conversion.ToDecimal((object)txtFreightAddLessSale.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTotalFreightSale_Leave(object sender, EventArgs e)
	{
		try
		{
			txtTotalFreightSale.Text = Conversion.ToDecimal((object)txtTotalFreightSale.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBillWeightGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtBillWeightGrn, txtBillWeightGdn, txtBillWeightDiff);
	}

	private void txtBillWeightGdn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifference(txtBillWeightGrn, txtBillWeightGdn, txtBillWeightDiff);
	}

	private void txtBillWeightGdn_Leave(object sender, EventArgs e)
	{
		txtBillWeightGdn.Text = Conversion.ToDecimal((object)txtBillWeightGdn.Text).ToString("#,##0.####");
	}

	private void txtBillWeightGrn_Leave(object sender, EventArgs e)
	{
		txtBillWeightGrn.Text = Conversion.ToDecimal((object)txtBillWeightGrn.Text).ToString("#,##0.####");
	}

	private void CalculateHeaderRates()
	{
		try
		{
			double SaleAvgRate = GridEX_Helper.GetColumnAvg(grd, "SaleNetRate");
			double PurchaseAvgRate = GridEX_Helper.GetColumnAvg(grd, "PurchaseNetRate");
			txtSaleAvgNetRate.Text = SaleAvgRate.ToString("#,##0.###");
			txtPurchaseAvgNetRate.Text = PurchaseAvgRate.ToString("#,##0.###");
			FormHelper.SetDifference(txtSaleAvgNetRate, txtPurchaseAvgNetRate, txtDiffAvgNetRate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPaymentTermPurchase_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			txtDueDaysPurchase.Enabled = true;
			if (Conversion.ToInt(CmbPaymentTermPurchase.Value) == 1 || Conversion.ToInt(CmbPaymentTermPurchase.Value) == 3)
			{
				txtDueDaysPurchase.Text = "0";
				txtDueDaysPurchase.Enabled = false;
			}
			else if (Conversion.ToInt(CmbPaymentTermPurchase.Value) == 2 && Conversion.ToInt((object)txtDueDaysPurchase.Text) == 0)
			{
				txtDueDaysPurchase.Text = "2";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtDueDaysPurchase_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtDueDaysPurchase.Text.Trim() != string.Empty && txtDueDaysPurchase.Text != null)
			{
				txtDueDatePurchase.Value = txtDocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDaysPurchase.Text.Trim()));
			}
			else
			{
				txtDueDatePurchase.Value = txtDocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPaymentTermSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtDueDaysSale.Enabled = true;
			if (Conversion.ToInt(CmbPaymentTermSale.Value) == 1 || Conversion.ToInt(CmbPaymentTermSale.Value) == 3)
			{
				txtDueDaysSale.Text = "0";
				txtDueDaysSale.Enabled = false;
			}
			else if (Conversion.ToInt(CmbPaymentTermSale.Value) == 2 && Conversion.ToInt((object)txtDueDaysSale.Text) == 0)
			{
				txtDueDaysSale.Text = "2";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDueDaysSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtDueDaysSale.Text.Trim() != string.Empty && txtDueDaysSale.Text != null)
			{
				txtDueDateSale.Value = txtDocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDaysSale.Text));
			}
			else
			{
				txtDueDateSale.Value = txtDocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWhtAccountPurchase_Leave(object sender, EventArgs e)
	{
	}

	private void CmbWhtAccountSale_Leave(object sender, EventArgs e)
	{
	}

	private void CmbFreightAccountPurchase_Leave(object sender, EventArgs e)
	{
	}

	private void CmbFreightAccountSale_Leave(object sender, EventArgs e)
	{
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		OpenSlip(RecId);
	}

	private void BtnPrintII_Click(object sender, EventArgs e)
	{
		OpenSlip(RecId, 2);
	}

	private void btnPurchasePrint_Click(object sender, EventArgs e)
	{
		OpenSlip(RecId, 3);
	}

	private void btnSalePrint_Click(object sender, EventArgs e)
	{
		OpenSlip(RecId, 4);
	}

	private void btnPrint103_Click(object sender, EventArgs e)
	{
		try
		{
			if (VoucherHeadId == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			OpenVoucher(RecId, DocumentTypeId, VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenVoucher(int PrintId, int DocumentTypeId, int VoucherHeadId = 0)
	{
		try
		{
			if (VoucherHeadId == 0)
			{
				VoucherHeadId = CommonServices.VoucherHeadIdGet(PrintId, DocumentTypeId);
			}
			CommonServices.VoucherReport_118(VoucherHeadId, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenSlip(int PrintId, int ReportNo = 1)
	{
		try
		{
			if (formright.DoHavePrintRights)
			{
				switch (ReportNo)
				{
				case 1:
					CrystalReportPrint_Helper.CommissionAgentBillSlip_1056(PrintId);
					break;
				case 2:
					CrystalReportPrint_Helper.CommissionAgentBillIncomeSlip_1056A(PrintId);
					break;
				case 3:
					CrystalReportPrint_Helper.CommissionAgentBillPurchaseSlip_1056_01(RecId);
					break;
				case 4:
					CrystalReportPrint_Helper.CommissionAgentBillSaleSlip_1056_02(RecId);
					break;
				case 5:
					CrystalReportPrint_Helper.CommissionAgentBillSaleSlip_1056_03(RecId);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSalePrintII_Click(object sender, EventArgs e)
	{
		OpenSlip(RecId, 5);
	}

	private void CommissionAgentTradeBill_162_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					CmbSupplier.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					cmbperemeter.Focus();
				}
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
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && BtnDelete.Visible && BtnDelete.Enabled)
				{
					BtnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					CmbSupplier.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdPurchaseOtherExpenses).Focus();
					}
					else if (((Control)(object)grdPurchaseOtherExpenses).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPurchasePmExpense).Focus();
					}
					else if (((Control)(object)grdPurchasePmExpense).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdSaleOtherExpense).Focus();
					}
					else if (((Control)(object)grdSaleOtherExpense).Focused)
					{
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdSalePmExpenseSale).Focus();
					}
					else if (((Control)(object)grdSalePmExpenseSale).Focused)
					{
						tabControl2.SelectedIndex = 4;
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grd).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Left)
				{
					if (tabControl2.SelectedIndex == 4)
					{
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdSalePmExpenseSale).Focus();
					}
					else if (((Control)(object)grdSalePmExpenseSale).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdSaleOtherExpense).Focus();
					}
					else if (((Control)(object)grdSaleOtherExpense).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPurchasePmExpense).Focus();
					}
					else if (((Control)(object)grdPurchasePmExpense).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdPurchaseOtherExpenses).Focus();
					}
					else if (((Control)(object)grdPurchaseOtherExpenses).Focused)
					{
						tabControl2.SelectedIndex = 4;
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grd).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbItemName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnPrint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btnPurchasePrint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3))
				{
					btnSalePrint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4))
				{
					BtnPrintII_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.D5))
				{
					btnPrint103_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)GrdHistoryDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)GrdHistoryDetail).Focused)
				{
					((Control)(object)GrdHistory).Focus();
				}
				else
				{
					((Control)(object)GrdHistoryDetail).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)GrdHistory).Focus();
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip 168");
			dt.Rows.Add("Alt+2", "For Print Slip 170");
			dt.Rows.Add("Alt+3", "For Print Slip 171");
			dt.Rows.Add("Alt+4", "For Print Slip 172");
			dt.Rows.Add("Alt+5", "For Print Slip 1706");
			dt.Rows.Add("Alt+6", "For Print Voucher 118");
			dt.Rows.Add("Ctrl+F5", "For Focus on Supplier Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
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

	private void grdPurchasePmExpense_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPurchasePmExpense.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPurchasePmExpense.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space && grdPurchasePmExpense.CurrentColumn != null)
			{
				if (grdPurchasePmExpense.CurrentColumn.Key == "Delete")
				{
					DeletePurchasePmExpenseRow(item);
				}
				if (grdPurchasePmExpense.CurrentColumn.Key == "Add")
				{
					AddRowInPurchasePmExpenseGrid();
				}
				grdPurchasePmExpense.UpdateData();
				FreightProportion();
				SupplierBillAmount();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeletePurchasePmExpenseRow(item);
				grdPurchasePmExpense.UpdateData();
				FreightProportion();
				SupplierBillAmount();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPurchasePmExpenseGrid();
				grdPurchasePmExpense.UpdateData();
				FreightProportion();
				SupplierBillAmount();
			}
			_ = e.KeyCode;
			_ = 112;
			((Control)(object)grdPurchasePmExpense).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSalePmExpense_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdSalePmExpenseSale.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdSalePmExpenseSale.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdSalePmExpenseSale.CurrentColumn != null && grdSalePmExpenseSale.CurrentColumn.Key == "Delete")
				{
					DeleteSalePmExpenseRow(item);
				}
				if (grdSalePmExpenseSale.CurrentColumn != null && grdSalePmExpenseSale.CurrentColumn.Key == "Add")
				{
					AddRowInSalePmExpenseGrid();
				}
				grdSalePmExpenseSale.UpdateData();
				ExpenseProportionForSale();
				CustomerBillAmount();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteSalePmExpenseRow(item);
				grdSalePmExpenseSale.UpdateData();
				ExpenseProportionForSale();
				CustomerBillAmount();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInSalePmExpenseGrid();
				grdSalePmExpenseSale.UpdateData();
				ExpenseProportionForSale();
				CustomerBillAmount();
			}
			_ = e.KeyCode;
			_ = 112;
			((Control)(object)grdSalePmExpenseSale).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || GrdHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GrdHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (GrdHistory.CurrentColumn.Key == "Print")
				{
					OpenSlip(CurrId);
				}
				else if (GrdHistory.CurrentColumn.Key == "IncomeSlip")
				{
					OpenSlip(CurrId, 2);
				}
				else if (GrdHistory.CurrentColumn.Key == "PurchaseSlip")
				{
					OpenSlip(CurrId, 3);
				}
				else if (GrdHistory.CurrentColumn.Key == "SaleSlip")
				{
					OpenSlip(CurrId, 4);
				}
				else if (GrdHistory.CurrentColumn.Key == "Voucher")
				{
					OpenVoucher(CurrId, DocumentTypeId);
				}
				else if (GrdHistory.CurrentColumn.Key == "SaleSlipII")
				{
					OpenSlip(CurrId, 5);
				}
				if (GrdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				OpenSlip(CurrId);
			}
			((Control)(object)GrdHistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadGdn_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				using (frmPendingGdnLoader loadGdn = new frmPendingGdnLoader(UserAccount))
				{
					loadGdn.ShowDialog();
					loadGdn.BringToFront();
					LoadedDataSetToGetExpenseAndEbDetail = loadGdn.ds;
					LoadInGridDetail(loadGdn.dtLoader);
					return;
				}
			}
			throw new Exception("You Can't Load Because Entry Already Exist");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				if (grd.GetDataRows().Any())
				{
					GridEXRow existingRow = grd.GetDataRows().FirstOrDefault((GridEXRow r) => Conversion.ToInt(r.Cells["GdnId"].Value) > 0);
					if (existingRow != null)
					{
						int num = Conversion.ToInt(existingRow.Cells["GdnId"].Value);
						int incomingId = Conversion.ToInt(dt.Rows[0]["gdnBuyerDispatchMasterId"]);
						if (num != incomingId)
						{
							throw new Exception("Already loaded rows belong to a different Gdn. You cannot load rows from another Gdn.");
						}
					}
				}
				DataRow row = dt.Rows[0];
				StringBuilder warningRemarksBuilder = new StringBuilder();
				string header = Convert.ToString(row["warningRemarksHeader"]);
				if (!string.IsNullOrWhiteSpace(header))
				{
					warningRemarksBuilder.AppendLine("1. " + header);
				}
				int counter = (string.IsNullOrWhiteSpace(header) ? 1 : 2);
				foreach (DataRow row2 in dt.Rows)
				{
					string remark = Convert.ToString(row2["warningRemarks"]);
					if (!string.IsNullOrWhiteSpace(remark))
					{
						warningRemarksBuilder.AppendLine($"{counter}. {remark}");
						counter++;
					}
				}
				txtMainRemarks.Text = warningRemarksBuilder.ToString().Trim();
				CmbSupplier.Value = Conversion.ToInt(row["supplierId"]);
				CmbDeliveryTermPurchase.Value = Conversion.ToInt(row["GrnDeliveryTermId"]);
				txtSupplierFirstWeight.Text = Conversion.ToDouble(row["SupplierLoadWeight"]).ToString("#,##0.###");
				txtSupplierSecondWeight.Text = Conversion.ToDouble(row["SupplierTareWeight"]).ToString("#,##0.###");
				txtSupplierWeight.Text = Conversion.ToDouble(row["SupplierScaleNetWeight"]).ToString("#,##0.###");
				txtBillWeightGrn.Text = Conversion.ToDouble(row["GrnNetBillWeightSum"]).ToString("#,##0.###");
				CmbFreightAccountPurchase.Value = Conversion.ToInt(row["supplierId"]);
				txtBuyerWeight.ReadOnly = true;
				txtBuyerWeight.Enabled = false;
				CmbCustomer.Value = Conversion.ToInt(row["BuyerId"]);
				CmbDeliveryTermSale.Value = Conversion.ToInt(row["deliveryTermId"]);
				txtBuyerFirstWeight.Text = Conversion.ToDouble(row["BuyerLoadWeight"]).ToString("#,##0.###");
				txtBuyerSecondWeight.Text = Conversion.ToDouble(row["BuyerTareWeight"]).ToString("#,##0.###");
				txtBuyerWeight.Text = Conversion.ToDouble(row["BuyerScaleNetWeight"]).ToString("#,##0.###");
				txtBillWeightGdn.Text = Conversion.ToDouble(row["BillWeight"]).ToString("#,##0.###");
				CmbFreightAccountSale.Value = Conversion.ToInt(row["BuyerId"]);
				int purchaseTerm = Conversion.ToInt(row["GrnDeliveryTermId"]);
				int deliveryTermId = Conversion.ToInt(row["deliveryTermId"]);
				decimal purchaseFreight = Conversion.ToDecimal(row["GrnTotalFreight"]);
				decimal saleFreight = Conversion.ToDecimal(row["totalFreight"]);
				bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(purchaseTerm, "");
				bool isSaleDelivered = FormHelper.DeliveryTermIsPonch(deliveryTermId, "");
				if ((isPurchaseDelivered && isSaleDelivered) || (!isPurchaseDelivered && isSaleDelivered))
				{
					txtFreightRemarksSale.Text = Conversion.ToString(row["FreightRemarks"]);
				}
				FreightHandling(purchaseFreight, saleFreight);
				EnableAndDisableFields(flag: false);
				HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
					select Conversion.ToInt(r.Cells["GdnDetailId"].Value));
				foreach (DataRow dr in dt.Rows)
				{
					int DetailId = Conversion.ToInt(dr["DetailId"]);
					if (existingDetailIds.Add(DetailId))
					{
						DataRow newRow = dtDetail.NewRow();
						newRow["GdnId"] = Conversion.ToInt(dr["gdnBuyerDispatchMasterId"]);
						newRow["GdnDetailId"] = dr["DetailId"];
						newRow["GdnNo"] = dr["docNo"];
						newRow["GdnDate"] = dr["docDate"];
						newRow["GrnId"] = Conversion.ToInt(dr["grnSupplierLoadingMasterId"]);
						newRow["GrnDetailId"] = dr["grnSupplierLoadingDetailId"];
						newRow["GrnNo"] = dr["GrnNo"];
						newRow["GrnDate"] = dr["GrnDate"];
						newRow["PoId"] = Conversion.ToInt(dr["purchaseOrderMasterId"]);
						newRow["PoDetailId"] = dr["purchaseOrderDetailId"];
						newRow["PoNo"] = dr["PurchaseOrderNo"];
						newRow["PoDate"] = dr["PurchaseOrderDate"];
						newRow["SoId"] = Conversion.ToInt(dr["saleOrderMasterId"]);
						newRow["SoDetailId"] = dr["saleOrderDetailId"];
						newRow["SoNo"] = dr["SaleOrderNo"];
						newRow["SoDate"] = dr["SaleOrderDate"];
						int itemId = Conversion.ToInt(dr["itemId"]);
						newRow["ItemId"] = itemId;
						newRow["ItemName"] = ((itemId > 0) ? dr["ItemName"] : string.Empty);
						newRow["ItemCode"] = ((itemId > 0) ? dr["ItemCode"] : string.Empty);
						newRow["CropYearId"] = dr["cropYearId"];
						newRow["CropYear"] = dr["cropYear"];
						newRow["PackTypeId"] = dr["packingTypeId"];
						newRow["PackTypeDesc"] = dr["PackingType"];
						newRow["UomId"] = dr["packUomId"];
						newRow["UomCode"] = dr["PackUomCode"];
						newRow["PackEquivalent"] = dr["PackUomEquivalent"];
						newRow["ItemQty"] = dr["loadingQty"];
						newRow["RefDocNo"] = dr["BuyerRefDocNo"];
						newRow["VehicleNo"] = dr["vehicleNo"];
						newRow["BiltyNo"] = dr["biltyNo"];
						newRow["PurchaseGrossWeight"] = dr["GrnGrossWeight"];
						newRow["PurchaseEbCut"] = dr["GrnEbUnit"];
						newRow["PurchaseEbCutTotal"] = dr["GrnEbTotal"];
						newRow["PurchaseAddLessWeight"] = dr["GrnAddLessWeight"];
						newRow["PurchaseBillWeight"] = dr["GrnNetBillWeight"];
						newRow["PurchaseRateWithoutAddLess"] = dr["PurchaseRate"];
						newRow["PurchaseNetRate"] = dr["PurchaseRate"];
						newRow["PurchaseRateUomId"] = dr["PurchaseRateUomId"];
						newRow["PurchaseRateUom"] = dr["PurchaseRateUomCode"];
						newRow["PurchaseRateEquivalent"] = dr["PurchaseRateUomEquivalent"];
						newRow["PurchaseAmount"] = dr["PurchaseAmount"];
						newRow["PurchaseTaxNameId"] = dr["TaxNameIdPurchase"];
						newRow["PurchaseTaxName"] = dr["TaxNamePurchase"];
						newRow["PurchaseTax%"] = dr["TaxPercentPurchase"];
						newRow["PurchaseTaxAmount"] = dr["TaxAmountPurchase"];
						newRow["SaleGrossWeight"] = dr["wbGrossWeight"];
						newRow["SaleEbCut"] = dr["ebwPerUnit"];
						newRow["SaleEbCutTotal"] = dr["ebwTotal"];
						newRow["SaleAddLess"] = dr["addLessWeight"];
						newRow["SaleBillWeight"] = dr["netBillWeight"];
						newRow["SaleRateWithoutAddLess"] = dr["SaleRate"];
						newRow["SaleNetRate"] = dr["SaleRate"];
						newRow["SaleRateUomId"] = dr["SaleRateUomId"];
						newRow["SaleRateUom"] = dr["SaleRateUomCode"];
						newRow["SaleRateEquivalent"] = dr["SaleRateUomEquivalent"];
						newRow["SaleAmount"] = dr["SaleAmount"];
						newRow["SaleTaxNameId"] = dr["TaxNameIdSale"];
						newRow["SaleTaxName"] = dr["TaxNameSale"];
						newRow["SaleTax%"] = dr["TaxPercentSale"];
						newRow["SaleTaxAmount"] = dr["TaxAmountSale"];
						dtDetail.Rows.Add(newRow);
					}
				}
			}
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSettings(grd);
			PanelDetailInfo.Visible = false;
			CalculationsCallForDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightHandling(decimal purchaseFreight, decimal saleFreight)
	{
		bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
		bool isSaleDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text));
		txtFreightAmountPurchase.Text = "0";
		txtFreightAmountSale.Text = "0";
		if (isPurchaseDelivered && isSaleDelivered)
		{
			txtFreightAmountPurchase.Text = purchaseFreight.ToString("#,##0.###");
			txtFreightAmountSale.Text = saleFreight.ToString("#,##0.###");
		}
		else if (isPurchaseDelivered || isSaleDelivered)
		{
			if (isPurchaseDelivered && !isSaleDelivered)
			{
				txtFreightAmountPurchase.Text = purchaseFreight.ToString("#,##0.###");
			}
			else if (!isPurchaseDelivered && isSaleDelivered)
			{
				txtFreightAmountSale.Text = saleFreight.ToString("#,##0.###");
			}
		}
	}

	private void CalculationsCallForDetail()
	{
		try
		{
			CalculateEbTotalThenBillWeight();
			GetSaleExpensesOnBaseOfGdn();
			GetEmptyBagDetailOnBaseOfGdn();
			GetEmptyBagDetailOnBaseOfGrn();
			GetPurchaseExpensesOnBaseOfGrn();
			GetSalePaymentDetailOnBaseOfSaleOrder();
			GetPurchasePaymentDetailOnBaseOfPurchaseOrder();
			GetSaleCommissionAndBrokeryOnBaseOfSaleOrder();
			GetPurchaseCommissionAndBrokeryOnBaseOfPurchaseOrder();
			InitCommissionConfigs();
			CalculateCommissionGrid(GetPurchaseCommissionConfig());
			CalculateCommissionGrid(GetPurchaseBrokeryConfig());
			CalculateCommissionGrid(GetSaleCommissionConfig());
			CalculateCommissionGrid(GetSaleBrokeryConfig());
			ExpenseProportionForPurchase();
			ExpenseProportionForSale();
			FreightProportion();
			SupplierBillAmount();
			CustomerBillAmount();
			ProportionateCommissionAndBrokery();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateEbTotalThenBillWeight()
	{
		try
		{
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				Conversion.ToDecimal(obj.Cells["ItemQty"].Value);
				decimal GrossWeight = Conversion.ToDecimal(obj.Cells["PurchaseGrossWeight"].Value);
				decimal addLessWeight = Conversion.ToDecimal(obj.Cells["PurchaseAddLessWeight"].Value);
				Conversion.ToDecimal(obj.Cells["PurchaseEbCut"].Value);
				decimal PRate = Conversion.ToDecimal(obj.Cells["PurchaseNetRate"].Value);
				decimal PRateEQuivalent = Conversion.ToDecimal(obj.Cells["PurchaseRateEquivalent"].Value);
				decimal PTaxPct = Conversion.ToDecimal(obj.Cells["PurchaseTax%"].Value);
				decimal ebTotal = Conversion.ToDecimal(obj.Cells["PurchaseEbCutTotal"].Value);
				decimal SaleGrossWeight = Conversion.ToDecimal(obj.Cells["SaleGrossWeight"].Value);
				decimal SaleAddLessWeight = Conversion.ToDecimal(obj.Cells["SaleAddLess"].Value);
				Conversion.ToDecimal(obj.Cells["SaleEbCut"].Value);
				decimal SRate = Conversion.ToDecimal(obj.Cells["SaleNetRate"].Value);
				decimal SRateEQuivalent = Conversion.ToDecimal(obj.Cells["SaleRateEquivalent"].Value);
				decimal STaxPct = Conversion.ToDecimal(obj.Cells["SaleTax%"].Value);
				decimal SaleEbTotal = Conversion.ToDecimal(obj.Cells["SaleEbCutTotal"].Value);
				obj.BeginEdit();
				decimal pNetWeight = GrossWeight - ebTotal + addLessWeight;
				obj.Cells["PurchaseBillWeight"].Value = pNetWeight;
				decimal Pamount = ((PRateEQuivalent > 0m) ? (pNetWeight / PRateEQuivalent * PRate) : 0m);
				Pamount = Math.Round(Pamount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				obj.Cells["PurchaseAmount"].Value = Pamount;
				decimal PTaxAmount = Pamount * PTaxPct / 100m;
				obj.Cells["PurchaseTaxAmount"].Value = PTaxAmount;
				decimal sNetWeight = SaleGrossWeight - SaleEbTotal + SaleAddLessWeight;
				obj.Cells["SaleBillWeight"].Value = sNetWeight;
				decimal Samount = ((SRateEQuivalent > 0m) ? (sNetWeight / SRateEQuivalent * SRate) : 0m);
				Samount = Math.Round(Samount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				obj.Cells["SaleAmount"].Value = Samount;
				decimal STaxAmount = Samount * STaxPct / 100m;
				obj.Cells["SaleTaxAmount"].Value = STaxAmount;
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPurchaseExpensesOnBaseOfGrn()
	{
		try
		{
			DataTable expenseTable = LoadedDataSetToGetExpenseAndEbDetail?.Tables[4];
			if (expenseTable == null || expenseTable.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> grnIds = (from GridEXRow item in grd.GetDataRows()
				select Conversion.ToInt(item.Cells["GrnId"].Value)).ToHashSet();
			List<DataRow> filteredRows = (from r in expenseTable.AsEnumerable()
				where grnIds.Contains(Conversion.ToInt(r["grnSupplierLoadingMasterId"]))
				select r).ToList();
			if (!filteredRows.Any())
			{
				return;
			}
			Dictionary<int, List<DataRow>> invExpByItemId = (from r in dtPurchaseOtherExpense.AsEnumerable()
				group r by Conversion.ToInt(r["ItemId"])).ToDictionary((IGrouping<int, DataRow> g) => g.Key, (IGrouping<int, DataRow> g) => g.ToList());
			foreach (DataRow expRow in filteredRows)
			{
				int itemId = Conversion.ToInt(expRow["ItemId"]);
				int grnId = Conversion.ToInt(expRow["grnSupplierLoadingMasterId"]);
				if (!invExpByItemId.TryGetValue(itemId, out var matchedRows))
				{
					continue;
				}
				foreach (DataRow item in matchedRows)
				{
					item["GrnId"] = grnId;
					item["GrnNo"] = expRow["GrnNo"];
					item["Qty"] = expRow["Qty"];
					item["Rate"] = expRow["rate"];
					item["Amount"] = expRow["amount"];
					item["Remarks"] = expRow["remarks"];
				}
			}
			grdPurchaseOtherExpenses.DataSource = dtPurchaseOtherExpense;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetEmptyBagDetailOnBaseOfGrn()
	{
		try
		{
			DataTable EbTable = LoadedDataSetToGetExpenseAndEbDetail?.Tables[2];
			if (EbTable == null || EbTable.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> grnIds = (from GridEXRow item in grd.GetDataRows()
				select Conversion.ToInt(item.Cells["GrnId"].Value)).ToHashSet();
			List<DataRow> filteredRows = (from r in EbTable.AsEnumerable()
				where grnIds.Contains(Conversion.ToInt(r["grnSupplierLoadingMasterId"]))
				select r).ToList();
			if (!filteredRows.Any())
			{
				return;
			}
			if (!dtPurchasePmExpense.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["Amount"]) > 0m))
			{
				dtPurchasePmExpense.Rows.Clear();
			}
			HashSet<int> existingKeys = (from r in dtPurchasePmExpense.AsEnumerable()
				select Conversion.ToInt(r["GrnId"])).ToHashSet();
			decimal qty = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemQty"));
			foreach (DataRow row in filteredRows)
			{
				int grnId = Conversion.ToInt(row["grnSupplierLoadingMasterId"]);
				if (existingKeys.Add(grnId))
				{
					dtPurchasePmExpense.Rows.Add(grnId, row["GrnNo"], 0, row["PackingTypeId"], row["emptyBagPackingMaterialItemId"], qty, row["Rate"], qty * Conversion.ToDecimal(row["Rate"]), 0, "", row["EBWeightDeductionTermId"], row["EmptyBagTerm"]);
				}
			}
			grdPurchasePmExpense.DataSource = dtPurchasePmExpense;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetEmptyBagDetailOnBaseOfGdn()
	{
		try
		{
			DataTable EbTable = LoadedDataSetToGetExpenseAndEbDetail?.Tables[1];
			if (EbTable == null || EbTable.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> grnIds = (from GridEXRow item in grd.GetDataRows()
				select Conversion.ToInt(item.Cells["GdnId"].Value)).ToHashSet();
			List<DataRow> filteredRows = (from r in EbTable.AsEnumerable()
				where grnIds.Contains(Conversion.ToInt(r["gdnBuyerDispatchMasterId"]))
				select r).ToList();
			if (!filteredRows.Any())
			{
				return;
			}
			if (!dtSalePmExpense.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["Amount"]) > 0m))
			{
				dtSalePmExpense.Rows.Clear();
			}
			HashSet<int> existingKeys = (from r in dtSalePmExpense.AsEnumerable()
				select Conversion.ToInt(r["GdnId"])).ToHashSet();
			decimal qty = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemQty"));
			foreach (DataRow row in filteredRows)
			{
				int gdnId = Conversion.ToInt(row["gdnBuyerDispatchMasterId"]);
				if (existingKeys.Add(gdnId))
				{
					dtSalePmExpense.Rows.Add(gdnId, row["GdnNo"], 0, row["PackingTypeId"], row["emptyBagPackingMaterialItemId"], qty, row["Rate"], qty * Conversion.ToDecimal(row["Rate"]), 0, "", row["EBWeightDeductionTermId"], row["EmptyBagTerm"]);
				}
			}
			grdSalePmExpenseSale.DataSource = dtSalePmExpense;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSaleExpensesOnBaseOfGdn()
	{
		try
		{
			DataTable expenseTable = LoadedDataSetToGetExpenseAndEbDetail?.Tables[3];
			if (expenseTable == null || expenseTable.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> gdnIds = (from GridEXRow item in grd.GetDataRows()
				select Conversion.ToInt(item.Cells["GdnId"].Value)).ToHashSet();
			List<DataRow> filteredRows = (from r in expenseTable.AsEnumerable()
				where gdnIds.Contains(Conversion.ToInt(r["gdnBuyerDispatchMasterId"]))
				select r).ToList();
			if (!filteredRows.Any())
			{
				return;
			}
			Dictionary<int, List<DataRow>> invExpByItemId = (from r in dtSaleOtherExpense.AsEnumerable()
				group r by Conversion.ToInt(r["ItemId"])).ToDictionary((IGrouping<int, DataRow> g) => g.Key, (IGrouping<int, DataRow> g) => g.ToList());
			foreach (DataRow expRow in filteredRows)
			{
				int itemId = Conversion.ToInt(expRow["ItemId"]);
				int gdnId = Conversion.ToInt(expRow["gdnBuyerDispatchMasterId"]);
				if (!invExpByItemId.TryGetValue(itemId, out var matchedRows))
				{
					continue;
				}
				foreach (DataRow item in matchedRows)
				{
					item["GdnId"] = gdnId;
					item["GdnNo"] = expRow["GdnNo"];
					item["Qty"] = expRow["Qty"];
					item["Rate"] = expRow["rate"];
					item["Amount"] = expRow["amount"];
					item["Remarks"] = expRow["remarks"];
				}
			}
			grdSaleOtherExpense.DataSource = dtSaleOtherExpense;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSalePaymentDetailOnBaseOfSaleOrder()
	{
		try
		{
			DataTable table = LoadedDataSetToGetExpenseAndEbDetail?.Tables[6];
			if (table == null || table.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> soIds = (from GridEXRow item in grd.GetDataRows()
				select Conversion.ToInt(item.Cells["SoId"].Value)).ToHashSet();
			List<DataRow> filteredRows = (from r in table.AsEnumerable()
				where soIds.Contains(Conversion.ToInt(r["saleOrderMasterId"]))
				select r).ToList();
			if (!filteredRows.Any())
			{
				return;
			}
			if (!dtSalePaymentDetail.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["Amount"]) > 0m))
			{
				dtSalePaymentDetail.Rows.Clear();
			}
			HashSet<(int, int)> existingKeys = (from r in dtSalePaymentDetail.AsEnumerable()
				select (SoId: Conversion.ToInt(r["SoId"]), PaymentTermId: Conversion.ToInt(r["PaymentTermId"]))).ToHashSet();
			foreach (DataRow row in filteredRows)
			{
				int SoId = Conversion.ToInt(row["saleOrderMasterId"]);
				int PtmId = Conversion.ToInt(row["PaymentTermId"]);
				if (existingKeys.Add((SoId, PtmId)))
				{
					dtSalePaymentDetail.Rows.Add(0, SoId, row["SaleOrderNo"], PtmId, row["DueDays"], row["pctOfTotal"], row["dueAmount"], row["BaseDueDateTypeId"], row["DueDate"]);
				}
			}
			grdPaymentDetailSale.DataSource = dtSalePaymentDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPurchasePaymentDetailOnBaseOfPurchaseOrder()
	{
		try
		{
			DataTable table = LoadedDataSetToGetExpenseAndEbDetail?.Tables[8];
			if (table == null || table.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> poIds = (from GridEXRow item in grd.GetDataRows()
				select Conversion.ToInt(item.Cells["PoId"].Value)).ToHashSet();
			List<DataRow> filteredRows = (from r in table.AsEnumerable()
				where poIds.Contains(Conversion.ToInt(r["purchaseOrderMasterId"]))
				select r).ToList();
			if (!filteredRows.Any())
			{
				return;
			}
			if (!dtPurchasePaymentDetail.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["Amount"]) > 0m))
			{
				dtPurchasePaymentDetail.Rows.Clear();
			}
			HashSet<(int, int)> existingKeys = (from r in dtPurchasePaymentDetail.AsEnumerable()
				select (PoId: Conversion.ToInt(r["PoId"]), PaymentTermId: Conversion.ToInt(r["PaymentTermId"]))).ToHashSet();
			foreach (DataRow row in filteredRows)
			{
				int poId = Conversion.ToInt(row["purchaseOrderMasterId"]);
				int PtmId = Conversion.ToInt(row["PaymentTermId"]);
				if (existingKeys.Add((poId, PtmId)))
				{
					dtPurchasePaymentDetail.Rows.Add(0, poId, row["PurchaseOrderNo"], PtmId, row["DueDays"], row["pctOfTotal"], row["dueAmount"], row["BaseDueDateTypeId"], row["DueDate"]);
				}
			}
			grdPaymentDetailPurchase.DataSource = dtPurchasePaymentDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPurchaseCommissionAndBrokeryOnBaseOfPurchaseOrder()
	{
		try
		{
			DataTable table = LoadedDataSetToGetExpenseAndEbDetail?.Tables[7];
			if (table == null || table.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> poIds = (from GridEXRow item in grd.GetDataRows()
				select Conversion.ToInt(item.Cells["PoId"].Value)).ToHashSet();
			List<DataRow> filteredRows = (from r in table.AsEnumerable()
				where poIds.Contains(Conversion.ToInt(r["purchaseOrderMasterId"]))
				select r).ToList();
			if (!filteredRows.Any())
			{
				return;
			}
			List<DataRow> filteredRowsCommission = filteredRows.Where((DataRow r) => Conversion.ToInt(r["agentTypeId"]) == 1).ToList();
			List<DataRow> filteredRowsBrokery = filteredRows.Where((DataRow r) => Conversion.ToInt(r["agentTypeId"]) == 2).ToList();
			if (filteredRowsCommission.Any())
			{
				if (!dtPurchaseCommission.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["CommissionAmount"]) > 0m))
				{
					dtPurchaseCommission.Rows.Clear();
				}
				HashSet<int> existingKeys = (from r in dtPurchaseCommission.AsEnumerable()
					select Conversion.ToInt(r["PoId"])).ToHashSet();
				foreach (DataRow row2 in filteredRowsCommission)
				{
					int poId2 = Conversion.ToInt(row2["purchaseOrderMasterId"]);
					if (existingKeys.Add(poId2))
					{
						dtPurchaseCommission.Rows.Add(poId2, row2["PurchaseOrderNo"], row2["commissionAgentId"], row2["commissionTypeId"], row2["rateUomId"], row2["commissionRate"], row2["commissionAmount"], 0);
					}
				}
				grdPurchaseCommission.DataSource = dtPurchaseCommission;
			}
			if (!filteredRowsBrokery.Any())
			{
				return;
			}
			if (!dtPurchaseBrokery.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["BrokeryAmount"]) > 0m))
			{
				dtPurchaseBrokery.Rows.Clear();
			}
			HashSet<int> existingKeysInBrokery = (from r in dtPurchaseBrokery.AsEnumerable()
				select Conversion.ToInt(r["PoId"])).ToHashSet();
			foreach (DataRow row in filteredRowsBrokery)
			{
				int poId = Conversion.ToInt(row["purchaseOrderMasterId"]);
				if (existingKeysInBrokery.Add(poId))
				{
					dtPurchaseBrokery.Rows.Add(poId, row["PurchaseOrderNo"], row["commissionAgentId"], row["commissionTypeId"], row["rateUomId"], row["commissionRate"], row["commissionAmount"], 0);
				}
			}
			grdPurchaseBrokery.DataSource = dtPurchaseBrokery;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSaleCommissionAndBrokeryOnBaseOfSaleOrder()
	{
		try
		{
			DataTable table = LoadedDataSetToGetExpenseAndEbDetail?.Tables[5];
			if (table == null || table.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> soIds = (from GridEXRow item in grd.GetDataRows()
				select Conversion.ToInt(item.Cells["SoId"].Value)).ToHashSet();
			List<DataRow> filteredRows = (from r in table.AsEnumerable()
				where soIds.Contains(Conversion.ToInt(r["saleOrderMasterId"]))
				select r).ToList();
			if (!filteredRows.Any())
			{
				return;
			}
			List<DataRow> filteredRowsCommission = filteredRows.Where((DataRow r) => Conversion.ToInt(r["agentTypeId"]) == 1).ToList();
			List<DataRow> filteredRowsBrokery = filteredRows.Where((DataRow r) => Conversion.ToInt(r["agentTypeId"]) == 2).ToList();
			if (filteredRowsCommission.Any())
			{
				if (!dtSaleCommission.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["CommissionAmount"]) > 0m))
				{
					dtSaleCommission.Rows.Clear();
				}
				HashSet<int> existingKeys = (from r in dtSaleCommission.AsEnumerable()
					select Conversion.ToInt(r["SoId"])).ToHashSet();
				foreach (DataRow row2 in filteredRowsCommission)
				{
					int soId2 = Conversion.ToInt(row2["saleOrderMasterId"]);
					if (existingKeys.Add(soId2))
					{
						dtSaleCommission.Rows.Add(soId2, row2["SaleOrderNo"], row2["commissionAgentId"], row2["commissionTypeId"], row2["rateUomId"], row2["commissionRate"], row2["commissionAmount"], 0);
					}
				}
				grdSaleCommission.DataSource = dtSaleCommission;
			}
			if (!filteredRowsBrokery.Any())
			{
				return;
			}
			if (!dtSaleBrokery.AsEnumerable().Any((DataRow r) => Conversion.ToDecimal(r["BrokeryAmount"]) > 0m))
			{
				dtSaleBrokery.Rows.Clear();
			}
			HashSet<int> existingKeysInBrokery = (from r in dtSaleBrokery.AsEnumerable()
				select Conversion.ToInt(r["SoId"])).ToHashSet();
			foreach (DataRow row in filteredRowsBrokery)
			{
				int soId = Conversion.ToInt(row["saleOrderMasterId"]);
				if (existingKeysInBrokery.Add(soId))
				{
					dtSaleBrokery.Rows.Add(soId, row["SaleOrderNo"], row["commissionAgentId"], row["commissionTypeId"], row["rateUomId"], row["commissionRate"], row["commissionAmount"], 0);
				}
			}
			grdSaleBrokery.DataSource = dtSaleBrokery;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableAndDisableFields(bool flag)
	{
		((Control)(object)CmbDeliveryTermSale).Enabled = flag;
		((Control)(object)CmbCustomer).Enabled = flag;
		((Control)(object)CmbSupplier).Enabled = flag;
		((Control)(object)CmbDeliveryTermPurchase).Enabled = flag;
		((Control)(object)CmbEmptyBagTerm).Enabled = flag;
		((Control)(object)CmbEmptyBagTermPurchase).Enabled = flag;
		txtBuyerWeight.Enabled = flag;
		bool editableF = flag || !DisableFreightFieldsOnBillFromGdn;
		((Control)(object)CmbFreightAccountPurchase).Enabled = editableF;
		txtFreightAmountPurchase.Enabled = editableF;
		txtTotalFreightPurchase.Enabled = editableF;
		txtFreightAmountSale.Enabled = editableF;
		txtTotalFreightSale.Enabled = editableF;
		((Control)(object)CmbFreightAccountSale).Enabled = editableF;
		txtBillWeightGdn.Enabled = flag || EnableSaleAddLessWeightOnBillFromGdn;
	}

	private void PurchaseSaleCommissionGridsColumnsEdit(GridEX gridEX, string column)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = gridEX.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (Conversion.ToInt(r.Cells[column].Value) > 0)
				{
					gridEX.RootTable.Columns["CommissionAgentId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["CommissionTypeId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["CommissionUomId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["CommissionRate"].EditType = (EditType)0;
				}
				else
				{
					gridEX.RootTable.Columns["CommissionAgentId"].EditType = (EditType)4;
					gridEX.RootTable.Columns["CommissionTypeId"].EditType = (EditType)4;
					gridEX.RootTable.Columns["CommissionUomId"].EditType = (EditType)4;
					gridEX.RootTable.Columns["CommissionRate"].EditType = (EditType)1;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PurchaseSaleBrokeryGridsColumnsEdit(GridEX gridEX, string column)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = gridEX.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (Conversion.ToInt(r.Cells[column].Value) > 0)
				{
					gridEX.RootTable.Columns["BrokeryAgentId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["BrokeryTypeId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["BrokeryUomId"].EditType = (EditType)0;
					gridEX.RootTable.Columns["BrokeryRate"].EditType = (EditType)0;
				}
				else
				{
					gridEX.RootTable.Columns["BrokeryAgentId"].EditType = (EditType)4;
					gridEX.RootTable.Columns["BrokeryTypeId"].EditType = (EditType)4;
					gridEX.RootTable.Columns["BrokeryUomId"].EditType = (EditType)4;
					gridEX.RootTable.Columns["BrokeryRate"].EditType = (EditType)1;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculatePaymentDetailGrid(GridEX gridEX)
	{
		if (gridEX.RowCount == 0)
		{
			return;
		}
		bool isPurchase = ((Control)(object)gridEX).Name == "grdPaymentDetailPurchase";
		string orderColumn = (isPurchase ? "PoId" : "SoId");
		string amountColumn = (isPurchase ? "PurchaseAmount" : "SaleAmount");
		bool isCustomer = !isPurchase;
		int partyId = (isPurchase ? Conversion.ToInt(CmbSupplier.Value) : Conversion.ToInt(CmbCustomer.Value));
		int partyGlId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString((object)partyId)));
		Dictionary<int, decimal> orderTotals = (from GridEXRow r in grd.GetDataRows()
			where (int)r.RowType == 0
			group r by Conversion.ToInt(r.Cells[orderColumn].Value)).ToDictionary((IGrouping<int, GridEXRow> g) => g.Key, (IGrouping<int, GridEXRow> g) => g.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells[amountColumn].Value)));
		decimal grandTotal = orderTotals.Values.Sum();
		decimal baseAdjustment = default(decimal);
		if (isPurchase)
		{
			bool isPurchaseDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermPurchase.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermPurchase).Text));
			baseAdjustment += Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPurchaseOtherExpenses, "Amount"));
			baseAdjustment += Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "PurchaseTaxAmount"));
			if (partyId > 0 && grdPurchasePmExpense.RowCount > 0)
			{
				if (!isPurchaseDelivered)
				{
					decimal freightAmount = (from GridEXRow r in grdPurchasePmExpense.GetDataRows()
						where (int)r.RowType == 0 && Conversion.ToInt(r.Cells["AccountId"].Value) == partyGlId
						select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Amount"].Value));
					baseAdjustment += freightAmount;
				}
				else if (isPurchaseDelivered)
				{
					decimal freightAmount2 = (from GridEXRow r in grdPurchasePmExpense.GetDataRows()
						where (int)r.RowType == 0
						select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Amount"].Value));
					baseAdjustment -= freightAmount2;
				}
			}
			if (partyId > 0 && grdSalePmExpenseSale.RowCount > 0)
			{
				decimal pmAmount = (from GridEXRow r in grdSalePmExpenseSale.GetDataRows()
					where (int)r.RowType == 0 && Conversion.ToInt(r.Cells["AccountId"].Value) == partyGlId
					select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Amount"].Value));
				baseAdjustment += pmAmount;
			}
			decimal purchaseFreight = Conversion.ToDecimal((object)txtTotalFreightPurchase.Text);
			if (isPurchaseDelivered)
			{
				baseAdjustment -= purchaseFreight;
			}
			decimal WhtTaxAmount = Conversion.ToDecimal((object)txtWhtTaxAmountPurchase.Text);
			baseAdjustment -= WhtTaxAmount;
		}
		else
		{
			bool num = FormHelper.DeliveryTermIsLoad(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text));
			bool isSaleDelivered = FormHelper.DeliveryTermIsPonch(Conversion.ToInt(CmbDeliveryTermSale.Value), Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text));
			if (partyId > 0 && grdPurchasePmExpense.RowCount > 0)
			{
				decimal freightAmount3 = (from GridEXRow r in grdPurchasePmExpense.GetDataRows()
					where (int)r.RowType == 0 && Conversion.ToInt(r.Cells["AccountId"].Value) == partyGlId
					select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Amount"].Value));
				if (freightAmount3 != 0m)
				{
					baseAdjustment -= freightAmount3;
				}
			}
			decimal expAmountSale = Conversion.ToDecimal(grdSaleOtherExpense.GetTotal(grdSaleOtherExpense.RootTable.Columns["Amount"], (AggregateFunction)2));
			if (expAmountSale != 0m)
			{
				baseAdjustment += expAmountSale;
			}
			if (num)
			{
				baseAdjustment += Conversion.ToDecimal(grdSalePmExpenseSale.GetTotal(grdSalePmExpenseSale.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			else if (isSaleDelivered && partyId > 0)
			{
				decimal pmAmount2 = (from GridEXRow r in grdSalePmExpenseSale.GetDataRows()
					where (int)r.RowType == 0 && Conversion.ToInt(r.Cells["AccountId"].Value) == partyGlId
					select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Amount"].Value));
				if (pmAmount2 != 0m)
				{
					baseAdjustment -= pmAmount2;
				}
			}
			baseAdjustment += Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "SaleTaxAmount"));
			decimal saleFreight = Conversion.ToDecimal((object)txtTotalFreightSale.Text);
			if (isSaleDelivered)
			{
				baseAdjustment -= saleFreight;
			}
			decimal WhtTaxAmount2 = Conversion.ToDecimal((object)txtwhtTaxAmountSale.Text);
			baseAdjustment -= WhtTaxAmount2;
		}
		(GridEX, string, string)[] obj = new(GridEX, string, string)[4]
		{
			(grdPurchaseCommission, "CommissionAgentId", "CommissionAmount"),
			(grdPurchaseBrokery, "BrokeryAgentId", "BrokeryAmount"),
			(grdSaleCommission, "CommissionAgentId", "CommissionAmount"),
			(grdSaleBrokery, "BrokeryAgentId", "BrokeryAmount")
		};
		Dictionary<int, decimal> orderSpecificCommission = new Dictionary<int, decimal>();
		decimal noOrderCommission = default(decimal);
		(GridEX, string, string)[] array = obj;
		for (int i = 0; i < array.Length; i++)
		{
			(GridEX, string, string) tuple = array[i];
			GridEX commGrid = tuple.Item1;
			string agentCol = tuple.Item2;
			string amountCol = tuple.Item3;
			bool gridHasOrderCol = commGrid.RootTable.Columns[orderColumn] != null;
			if (partyId <= 0)
			{
				continue;
			}
			foreach (GridEXRow row in from GridEXRow r in commGrid.GetDataRows()
				where (int)r.RowType == 0
				select r)
			{
				int agentId = Conversion.ToInt(row.Cells[agentCol].Value);
				int debitAcId = Conversion.ToInt(row.Cells["DebitAccountId"].Value);
				decimal amount2 = Conversion.ToDecimal(row.Cells[amountCol].Value);
				if (amount2 <= 0m)
				{
					continue;
				}
				decimal contribution = default(decimal);
				if (isCustomer)
				{
					if (partyId == agentId)
					{
						contribution -= amount2;
					}
					if (debitAcId > 0 && partyGlId == debitAcId)
					{
						contribution += amount2;
					}
				}
				else
				{
					if (partyId == agentId)
					{
						contribution += amount2;
					}
					if (debitAcId > 0 && partyGlId == debitAcId)
					{
						contribution -= amount2;
					}
				}
				if (contribution == 0m)
				{
					continue;
				}
				int rowOrderId = (gridHasOrderCol ? Conversion.ToInt(row.Cells[orderColumn].Value) : 0);
				if (rowOrderId > 0)
				{
					if (!orderSpecificCommission.ContainsKey(rowOrderId))
					{
						orderSpecificCommission[rowOrderId] = 0m;
					}
					orderSpecificCommission[rowOrderId] += contribution;
				}
				else
				{
					noOrderCommission += contribution;
				}
			}
		}
		foreach (GridEXRow item in from GridEXRow r in gridEX.GetDataRows()
			where (int)r.RowType == 0
			select r)
		{
			int orderId = Conversion.ToInt(item.Cells[orderColumn].Value);
			decimal t;
			decimal baseForOrder = ((orderId <= 0) ? grandTotal : (orderTotals.TryGetValue(orderId, out t) ? t : 0m));
			decimal totalAmount = ((grandTotal != 0m) ? Math.Round((grandTotal + baseAdjustment) * baseForOrder / grandTotal, 4) : (baseForOrder + baseAdjustment));
			if (orderId > 0 && orderSpecificCommission.TryGetValue(orderId, out var specificComm))
			{
				totalAmount += specificComm;
			}
			if (grandTotal != 0m && noOrderCommission != 0m)
			{
				totalAmount += Math.Round(noOrderCommission * baseForOrder / grandTotal, 4);
			}
			else if (grandTotal == 0m)
			{
				totalAmount += noOrderCommission;
			}
			decimal percent = Conversion.ToDecimal(item.Cells["%OfTotal"].Value);
			decimal amount = ((totalAmount != 0m) ? Math.Round(totalAmount * percent / 100m, 4) : 0m);
			item.BeginEdit();
			item.Cells["Amount"].Value = amount;
			item.EndEdit();
		}
	}

	private void HandleCommissionGridCellUpdated(CommissionConfig config, ColumnActionEventArgs e)
	{
		try
		{
			config.SourceGrid.UpdateData();
			if ((e.Column.Key == config.RateColumn || e.Column.Key == config.TypeColumn) && Conversion.ToString((object)config.SourceGrid.CurrentRow.Cells[config.TypeColumn].Text) == "Percent")
			{
				EnforcePercentCap(config);
			}
			CalculateCommissionGrid(config);
			config.SourceGrid.UpdateData();
			ProportionateCommissionAndBrokery();
			SupplierBillAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnforcePercentCap(CommissionConfig config)
	{
		GridEXRow currentRow = config.SourceGrid.CurrentRow;
		if (currentRow == null || Conversion.ToString((object)currentRow.Cells[config.TypeColumn].Text) != "Percent")
		{
			return;
		}
		int currentOrderId = Conversion.ToInt(currentRow.Cells[config.OrderIdColumn].Value);
		decimal currentRate = Conversion.ToDecimal(currentRow.Cells[config.RateColumn].Value);
		if (currentOrderId > 0)
		{
			decimal totalPercentage = default(decimal);
			GridEXRow[] rows = config.SourceGrid.GetRows();
			foreach (GridEXRow item in rows)
			{
				bool num = Conversion.ToString((object)item.Cells[config.TypeColumn].Text) == "Percent";
				bool sameOrder = Conversion.ToInt(item.Cells[config.OrderIdColumn].Value) == currentOrderId;
				if (num && sameOrder)
				{
					totalPercentage += Conversion.ToDecimal(item.Cells[config.RateColumn].Value);
				}
			}
			if (totalPercentage > 100m)
			{
				decimal excess = totalPercentage - 100m;
				currentRow.Cells[config.RateColumn].Value = currentRate - excess;
				config.SourceGrid.UpdateData();
				MessageBox.Show($"Total Percentage for Order #{currentOrderId} cannot be greater than 100.");
			}
		}
		else if (currentRate > 100m)
		{
			currentRow.Cells[config.RateColumn].Value = 100m;
			config.SourceGrid.UpdateData();
			MessageBox.Show("Percentage cannot be greater than 100.");
		}
	}

	private void CalculateCommissionGrid(CommissionConfig config)
	{
		if (config.SourceGrid.RowCount == 0)
		{
			return;
		}
		GridEXRow[] rows = config.SourceGrid.GetRows();
		foreach (GridEXRow item in rows)
		{
			int orderId = Conversion.ToInt(item.Cells[config.OrderIdColumn].Value);
			decimal totalAmount = default(decimal);
			decimal totalWeight = default(decimal);
			GridEXRow[] rows2 = grd.GetRows();
			foreach (GridEXRow row in rows2)
			{
				if (orderId <= 0 || Conversion.ToInt(row.Cells[config.OrderIdColumn].Value) == orderId)
				{
					totalAmount += Conversion.ToDecimal(row.Cells[config.AmountColumn].Value);
					totalWeight += Conversion.ToDecimal(row.Cells[config.WeightColumn].Value);
				}
			}
			decimal rate = Conversion.ToDecimal(item.Cells[config.RateColumn].Value);
			decimal uom = Conversion.ToDecimal((object)item.Cells[config.UomColumn].Text);
			string type = Conversion.ToString((object)item.Cells[config.TypeColumn].Text);
			decimal commissionAmount = ((!(type == "Percent")) ? ((!(type == "Weight")) ? rate : ((uom != 0m) ? (totalWeight / uom * rate) : 0m)) : ((totalAmount != 0m) ? (totalAmount * rate / 100m) : 0m));
			item.BeginEdit();
			item.Cells[config.CalcColumn].Value = commissionAmount;
			item.EndEdit();
		}
	}

	private void ProportionateCommissionAndBrokery()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				obj.Cells["PurchaseCommission"].Value = 0;
				obj.Cells["PurchaseBrokery"].Value = 0;
				obj.Cells["SaleCommission"].Value = 0;
				obj.Cells["SaleBrokery"].Value = 0;
				obj.EndEdit();
			}
			if (_commissionConfigs != null)
			{
				CommissionConfig[] commissionConfigs = _commissionConfigs;
				foreach (CommissionConfig config in commissionConfigs)
				{
					ApplyProportionateValues(config);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApplyProportionateValues(CommissionConfig config)
	{
		GridEXRow[] rows = config.SourceGrid.GetRows();
		foreach (GridEXRow obj in rows)
		{
			int orderId = Conversion.ToInt(obj.Cells[config.OrderIdColumn].Value);
			decimal rate = Conversion.ToDecimal(obj.Cells[config.RateColumn].Value);
			decimal uom = Conversion.ToDecimal((object)obj.Cells[config.UomColumn].Text);
			string type = Conversion.ToString((object)obj.Cells[config.TypeColumn].Text);
			decimal orderTotal = ((orderId > 0) ? config.OrderAmountFunc(orderId) : config.TotalAmount);
			GridEXRow[] rows2 = grd.GetRows();
			foreach (GridEXRow detailRow in rows2)
			{
				int rowOrderId = Conversion.ToInt(detailRow.Cells[config.OrderIdColumn].Value);
				decimal amount = Conversion.ToDecimal(detailRow.Cells[config.AmountColumn].Value);
				decimal weight = Conversion.ToDecimal(detailRow.Cells[config.WeightColumn].Value);
				if (orderId <= 0 || rowOrderId == orderId)
				{
					decimal calculatedValue = ((type == "Percent") ? (amount * rate / 100m) : ((!(type == "Weight")) ? ((orderTotal != 0m) ? (rate / orderTotal * amount) : 0m) : ((uom != 0m) ? (weight / uom * rate) : 0m)));
					detailRow.BeginEdit();
					detailRow.Cells[config.TargetColumn].Value = Conversion.ToDecimal(detailRow.Cells[config.TargetColumn].Value) + calculatedValue;
					detailRow.EndEdit();
				}
			}
		}
	}

	private void CommissionAndBrokeryGridValidation(GridEX gridEX, string columnForCommissionAgentIdOrBrokerId, string AmountColumn, string message, int tabIndex)
	{
		try
		{
			List<GridEXRow> rows = gridEX.GetDataRows().Cast<GridEXRow>().ToList();
			for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
			{
				GridEXRow item = rows[rowIndex];
				if (!(Conversion.ToDouble(item.Cells[AmountColumn].Value) <= 0.0))
				{
					int commissionAgentOrBrokerId = Conversion.ToInt(item.Cells[columnForCommissionAgentIdOrBrokerId].Value);
					if (SupplierCustomer.GetSupplierCustomerIdbyGlAccountId(Conversion.ToInt(item.Cells["DebitAccountId"].Value)) == commissionAgentOrBrokerId)
					{
						tabControl2.SelectedIndex = tabIndex;
						((Control)(object)gridEX).Focus();
						throw new Exception($"Row {rowIndex + 1}: {message}");
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ApplyCommissionOrBrokery(GridEX grid, string agentIdColumn, string debitAccountIdColumn, string amountColumn, int partyId, ref decimal billAmount, bool isCustomer = false)
	{
		try
		{
			if (partyId <= 0)
			{
				return;
			}
			int partyGlId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString((object)partyId)));
			GridEXRow[] dataRows = grid.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				int agentId = Conversion.ToInt(obj.Cells[agentIdColumn].Value);
				int debitAccountId = Conversion.ToInt(obj.Cells[debitAccountIdColumn].Value);
				decimal amount = Conversion.ToDecimal(obj.Cells[amountColumn].Value);
				if (isCustomer)
				{
					if (partyId == agentId)
					{
						billAmount -= amount;
					}
					if (debitAccountId > 0 && partyGlId == debitAccountId)
					{
						billAmount += amount;
					}
				}
				else
				{
					if (partyId == agentId)
					{
						billAmount += amount;
					}
					if (debitAccountId > 0 && partyGlId == debitAccountId)
					{
						billAmount -= amount;
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ApplyCommissionOrBrokeryForPL(GridEX grid, string agentIdColumn, string debitAccountIdColumn, string amountColumn, int tradingAccountGlId, ref double tradingDebitAmount, ref double tradingCreditAmount)
	{
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow obj in dataRows)
		{
			int agentId = Conversion.ToInt(obj.Cells[agentIdColumn].Value);
			int debitAccountId = Conversion.ToInt(obj.Cells[debitAccountIdColumn].Value);
			double amount = Conversion.ToDouble(obj.Cells[amountColumn].Value);
			int agentGlId = Conversion.ToInt((object)DatatableHelper.GetFirstColumnValue(dtSupplier, "GlAccountId", "Id", Conversion.ToString((object)agentId)));
			if (agentGlId > 0 && tradingAccountGlId == agentGlId)
			{
				tradingCreditAmount += amount;
			}
			if (debitAccountId == 0 || tradingAccountGlId == debitAccountId)
			{
				tradingDebitAmount += amount;
			}
		}
	}

	private CommissionConfig GetPurchaseCommissionConfig()
	{
		return _commissionConfigs[0];
	}

	private CommissionConfig GetPurchaseBrokeryConfig()
	{
		return _commissionConfigs[1];
	}

	private CommissionConfig GetSaleCommissionConfig()
	{
		return _commissionConfigs[2];
	}

	private CommissionConfig GetSaleBrokeryConfig()
	{
		return _commissionConfigs[3];
	}

	private void InitCommissionConfigs()
	{
		decimal totalPurchaseAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "PurcahseAmount"));
		decimal totalSaleAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "SaleAmount"));
		_commissionConfigs = new CommissionConfig[4]
		{
			new CommissionConfig
			{
				SourceGrid = grdPurchaseCommission,
				OrderIdColumn = "PoId",
				AmountColumn = "PurchaseAmount",
				WeightColumn = "PurchaseBillWeight",
				TargetColumn = "PurchaseCommission",
				RateColumn = "CommissionRate",
				UomColumn = "CommissionUomId",
				TypeColumn = "CommissionTypeId",
				CalcColumn = "CommissionAmount",
				TotalAmount = totalPurchaseAmount,
				OrderAmountFunc = (int orderId) => Conversion.ToDecimal((object)GridEX_Helper.GetColumnSumWithWhere(grd, "PurchaseAmount", "PoId", Conversion.ToString((object)orderId)))
			},
			new CommissionConfig
			{
				SourceGrid = grdPurchaseBrokery,
				OrderIdColumn = "PoId",
				AmountColumn = "PurchaseAmount",
				WeightColumn = "PurchaseBillWeight",
				TargetColumn = "PurchaseBrokery",
				RateColumn = "BrokeryRate",
				UomColumn = "BrokeryUomId",
				TypeColumn = "BrokeryTypeId",
				CalcColumn = "BrokeryAmount",
				TotalAmount = totalPurchaseAmount,
				OrderAmountFunc = (int orderId) => Conversion.ToDecimal((object)GridEX_Helper.GetColumnSumWithWhere(grd, "PurchaseAmount", "PoId", Conversion.ToString((object)orderId)))
			},
			new CommissionConfig
			{
				SourceGrid = grdSaleCommission,
				OrderIdColumn = "SoId",
				AmountColumn = "SaleAmount",
				WeightColumn = "SaleBillWeight",
				TargetColumn = "SaleCommission",
				RateColumn = "CommissionRate",
				UomColumn = "CommissionUomId",
				TypeColumn = "CommissionTypeId",
				CalcColumn = "CommissionAmount",
				TotalAmount = totalSaleAmount,
				OrderAmountFunc = (int orderId) => Conversion.ToDecimal((object)GridEX_Helper.GetColumnSumWithWhere(grd, "SaleAmount", "SoId", Conversion.ToString((object)orderId)))
			},
			new CommissionConfig
			{
				SourceGrid = grdSaleBrokery,
				OrderIdColumn = "SoId",
				AmountColumn = "SaleAmount",
				WeightColumn = "SaleBillWeight",
				TargetColumn = "SaleBrokery",
				RateColumn = "BrokeryRate",
				UomColumn = "BrokeryUomId",
				TypeColumn = "BrokeryTypeId",
				CalcColumn = "BrokeryAmount",
				TotalAmount = totalSaleAmount,
				OrderAmountFunc = (int orderId) => Conversion.ToDecimal((object)GridEX_Helper.GetColumnSumWithWhere(grd, "SaleAmount", "SoId", Conversion.ToString((object)orderId)))
			}
		};
	}

	private void CmbWhtTaxTypePurchase_Leave(object sender, EventArgs e)
	{
		PurchaseTaxCalculation();
	}

	private void CmbWhtTaxTypeSale_Leave(object sender, EventArgs e)
	{
		SaleTaxCalculation();
	}

	private void txtDocDate_ValueChanged(object sender, EventArgs e)
	{
		PurchaseTaxCalculation();
		SaleTaxTypesBind();
	}

	private void PurchaseTaxCalculation()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int taxTypeId = Conversion.ToInt(CmbWhtTaxTypePurchase.Value);
			DataTable detail = TaxScheduleMain.ReadTaxSchedule(new TaxScheduleMain
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EffectedDate = txtDocDate.Value,
				TaxNameId = taxTypeId
			});
			double taxPercent = 0.0;
			if (detail.Rows.Count > 0)
			{
				taxPercent = Conversion.ToDouble(detail.Rows[0]["TaxPercent"]);
			}
			string percentText = taxPercent.ToString("#,##0.####");
			txtWhtTaxPercentPurchase.Text = percentText;
			SupplierBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error applying withholding tax: " + ex.Message);
		}
	}

	private void SaleTaxCalculation()
	{
		try
		{
			int taxTypeId = Conversion.ToInt(CmbWhtTaxTypeSale.Value);
			double taxPercent = 0.0;
			if (((UltraGridBase)CmbWhtTaxTypeSale).ActiveRow != null && taxTypeId > 0)
			{
				UltraGridRow selectedRow = ((UltraDropDownBase)CmbWhtTaxTypeSale).SelectedRow;
				taxPercent = Conversion.ToDouble((selectedRow != null) ? selectedRow.Cells["TaxPercent"].Value : null);
			}
			string percentText = taxPercent.ToString("#,##0.####");
			txtWhtTaxPercentSale.Text = percentText;
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error applying withholding tax: " + ex.Message);
		}
	}

	private void RadBusinessNamePurchase_CheckedChanged(object sender, EventArgs e)
	{
		CommonBindings.SupplierDtFillFromGlobal(dtSupplier, RadBusinessNamePurchase.Checked, radPartyCodePurchase);
		SupplierBind();
	}

	private void RadBusinessNameSale_CheckedChanged(object sender, EventArgs e)
	{
		CommonBindings.SupplierDtFillFromGlobal(dtBuyer, RadBusinessNameSale.Checked, radPartyCodeSale);
		BuyerBind();
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
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Expected O, but got Unknown
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddf: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Expected O, but got Unknown
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Expected O, but got Unknown
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e58: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Expected O, but got Unknown
		//IL_0e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea5: Expected O, but got Unknown
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec6: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef2: Expected O, but got Unknown
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f13: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Expected O, but got Unknown
		//IL_0f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f76: Expected O, but got Unknown
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Expected O, but got Unknown
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d6: Expected O, but got Unknown
		//IL_1103: Unknown result type (might be due to invalid IL or missing references)
		//IL_110d: Expected O, but got Unknown
		//IL_12d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12db: Expected O, but got Unknown
		//IL_1308: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Expected O, but got Unknown
		//IL_1376: Unknown result type (might be due to invalid IL or missing references)
		//IL_1380: Expected O, but got Unknown
		//IL_1381: Unknown result type (might be due to invalid IL or missing references)
		//IL_138b: Expected O, but got Unknown
		//IL_1397: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a1: Expected O, but got Unknown
		//IL_13ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f9: Expected O, but got Unknown
		//IL_1410: Unknown result type (might be due to invalid IL or missing references)
		//IL_141a: Expected O, but got Unknown
		//IL_159c: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a6: Expected O, but got Unknown
		//IL_15bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c7: Expected O, but got Unknown
		//IL_162b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1635: Expected O, but got Unknown
		//IL_1683: Unknown result type (might be due to invalid IL or missing references)
		//IL_168d: Expected O, but got Unknown
		//IL_168e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1698: Expected O, but got Unknown
		//IL_16c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cf: Expected O, but got Unknown
		//IL_16f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fb: Expected O, but got Unknown
		//IL_1707: Unknown result type (might be due to invalid IL or missing references)
		//IL_1711: Expected O, but got Unknown
		//IL_176a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1774: Expected O, but got Unknown
		//IL_1780: Unknown result type (might be due to invalid IL or missing references)
		//IL_178a: Expected O, but got Unknown
		//IL_189e: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a8: Expected O, but got Unknown
		//IL_18bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c9: Expected O, but got Unknown
		//IL_18f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1900: Expected O, but got Unknown
		//IL_1922: Unknown result type (might be due to invalid IL or missing references)
		//IL_192c: Expected O, but got Unknown
		//IL_1943: Unknown result type (might be due to invalid IL or missing references)
		//IL_194d: Expected O, but got Unknown
		//IL_196f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1979: Expected O, but got Unknown
		//IL_19a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b0: Expected O, but got Unknown
		//IL_19c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d1: Expected O, but got Unknown
		//IL_19f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fd: Expected O, but got Unknown
		//IL_1a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1e: Expected O, but got Unknown
		//IL_1a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4a: Expected O, but got Unknown
		//IL_1a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6b: Expected O, but got Unknown
		//IL_1ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ace: Expected O, but got Unknown
		//IL_1ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae4: Expected O, but got Unknown
		//IL_1b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1b: Expected O, but got Unknown
		//IL_1b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5d: Expected O, but got Unknown
		//IL_250d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2517: Expected O, but got Unknown
		//IL_2524: Unknown result type (might be due to invalid IL or missing references)
		//IL_252e: Expected O, but got Unknown
		//IL_282e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2838: Expected O, but got Unknown
		//IL_2fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc9: Expected O, but got Unknown
		//IL_2fd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe0: Expected O, but got Unknown
		//IL_3220: Unknown result type (might be due to invalid IL or missing references)
		//IL_322a: Expected O, but got Unknown
		//IL_357c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3586: Expected O, but got Unknown
		//IL_3593: Unknown result type (might be due to invalid IL or missing references)
		//IL_359d: Expected O, but got Unknown
		//IL_3d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d26: Expected O, but got Unknown
		//IL_3d33: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d3d: Expected O, but got Unknown
		//IL_3f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa2: Expected O, but got Unknown
		//IL_3faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fb9: Expected O, but got Unknown
		//IL_42a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_42ac: Expected O, but got Unknown
		//IL_42b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c3: Expected O, but got Unknown
		//IL_4524: Unknown result type (might be due to invalid IL or missing references)
		//IL_452e: Expected O, but got Unknown
		//IL_453b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4545: Expected O, but got Unknown
		//IL_483b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4845: Expected O, but got Unknown
		//IL_4852: Unknown result type (might be due to invalid IL or missing references)
		//IL_485c: Expected O, but got Unknown
		//IL_4a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a99: Expected O, but got Unknown
		//IL_4aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ab0: Expected O, but got Unknown
		//IL_1421e: Unknown result type (might be due to invalid IL or missing references)
		//IL_14228: Expected O, but got Unknown
		//IL_1424c: Unknown result type (might be due to invalid IL or missing references)
		//IL_14256: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPurchaseOtherExpenses_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		GridEXLayout grdPurchasePmExpense_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSaleOtherExpense_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSalePmExpenseSale_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdSaleBrokery_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPurchaseBrokery_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSaleCommission_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPurchaseCommission_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentDetailSale_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentDetailPurchase_DesignTimeLayout = new GridEXLayout();
		Appearance appearance265 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance308 = new Appearance();
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
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance345 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance372 = new Appearance();
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
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance333 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
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
		Appearance appearance278 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
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
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryPurchaseExpenseDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryPurchaseFreightDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistorySaleExpenseDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistorySalePmDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistorySaleBrokeryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryPurchaseBrokeryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistorySaleCommissionDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryPurchaseCommissionDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryPaymentDetailSale_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryPaymentDetailPurchase_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Cmagt.frmCommissionAgentTradeBillAgainstGdn));
		this.MainPanel = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label34 = new System.Windows.Forms.Label();
		this.tabPurchaseExpense = new System.Windows.Forms.TabPage();
		this.grdPurchaseOtherExpenses = new GridEX();
		this.label53 = new System.Windows.Forms.Label();
		this.tabPurchasePmExpense = new System.Windows.Forms.TabPage();
		this.label45 = new System.Windows.Forms.Label();
		this.CmbEmptyBagTermPurchase = new UltraCombo();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdPurchasePmExpense = new GridEX();
		this.label43 = new System.Windows.Forms.Label();
		this.tabSaleExpense = new System.Windows.Forms.TabPage();
		this.grdSaleOtherExpense = new GridEX();
		this.label54 = new System.Windows.Forms.Label();
		this.tabSalePmExpense = new System.Windows.Forms.TabPage();
		this.lblebTerm = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdSalePmExpenseSale = new GridEX();
		this.CmbEmptyBagTerm = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.tabCommissionAndBrokery = new System.Windows.Forms.TabPage();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdSaleBrokery = new GridEX();
		this.label100 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdPurchaseBrokery = new GridEX();
		this.label101 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdSaleCommission = new GridEX();
		this.label98 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdPurchaseCommission = new GridEX();
		this.label99 = new System.Windows.Forms.Label();
		this.tabPaymentDetail = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdPaymentDetailSale = new GridEX();
		this.label87 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdPaymentDetailPurchase = new GridEX();
		this.label81 = new System.Windows.Forms.Label();
		this.PanelDetailInfo = new System.Windows.Forms.Panel();
		this.GbDetailInfo = new System.Windows.Forms.GroupBox();
		this.txtTotalAmountSale = new System.Windows.Forms.TextBox();
		this.txtTaxPercntSale = new System.Windows.Forms.TextBox();
		this.txtTaxAmountSale = new System.Windows.Forms.TextBox();
		this.CmbTaxNameSale = new UltraCombo();
		this.txtGrossWeightSale = new System.Windows.Forms.TextBox();
		this.CmbRateUomSale = new UltraCombo();
		this.txtBillWeightSale = new System.Windows.Forms.TextBox();
		this.txtItemAmountSale = new System.Windows.Forms.TextBox();
		this.txtSaleRateAddLess = new System.Windows.Forms.TextBox();
		this.txtSaleRateWithoutAddLess = new System.Windows.Forms.TextBox();
		this.txtSaleNetRate = new System.Windows.Forms.TextBox();
		this.txtEBTotalSale = new System.Windows.Forms.TextBox();
		this.txtAddlessWtSales = new System.Windows.Forms.TextBox();
		this.txtEBUnitSale = new System.Windows.Forms.TextBox();
		this.txtbiltyno = new System.Windows.Forms.TextBox();
		this.txtDetailRemarks = new System.Windows.Forms.TextBox();
		this.txtRefDocNo = new System.Windows.Forms.TextBox();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.CmbCropYear = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.CmbPackType = new UltraCombo();
		this.txtQuantity = new System.Windows.Forms.TextBox();
		this.CmbPackUom = new UltraCombo();
		this.label79 = new System.Windows.Forms.Label();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.label28 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.txtTotalAmountPurchase = new System.Windows.Forms.TextBox();
		this.txtTaxPercntPurchase = new System.Windows.Forms.TextBox();
		this.label75 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.txtTaxAmountPurchase = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label78 = new System.Windows.Forms.Label();
		this.CmbTaxNamePurchase = new UltraCombo();
		this.label71 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.CmbRateUomPurchase = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.label70 = new System.Windows.Forms.Label();
		this.label116 = new System.Windows.Forms.Label();
		this.label114 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtGrossWeightPurchase = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtEBTotalPurchase = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label115 = new System.Windows.Forms.Label();
		this.label113 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtItemAmountPurchase = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtPurchaseRateAddLess = new System.Windows.Forms.TextBox();
		this.txtPurchaseRateWithoutAddLess = new System.Windows.Forms.TextBox();
		this.txtPurchaseNetRate = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtAddlessWtPurchase = new System.Windows.Forms.TextBox();
		this.label69 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.txtBillWeightPurchase = new System.Windows.Forms.TextBox();
		this.txtEBUnitPurchase = new System.Windows.Forms.TextBox();
		this.PanelMainInfo = new System.Windows.Forms.Panel();
		this.GbSaleSide = new System.Windows.Forms.GroupBox();
		this.txtWhtTaxPercentSale = new System.Windows.Forms.TextBox();
		this.txtwhtTaxAmountSale = new System.Windows.Forms.TextBox();
		this.label109 = new System.Windows.Forms.Label();
		this.CmbWhtTaxTypeSale = new UltraCombo();
		this.txtSaleBillAmount = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label110 = new System.Windows.Forms.Label();
		this.label111 = new System.Windows.Forms.Label();
		this.CmbWhtAccountSale = new UltraCombo();
		this.label112 = new System.Windows.Forms.Label();
		this.txtFreightRemarksSale = new System.Windows.Forms.TextBox();
		this.panel10 = new System.Windows.Forms.Panel();
		this.RadBusinessNameSale = new System.Windows.Forms.RadioButton();
		this.RadNickNameSale = new System.Windows.Forms.RadioButton();
		this.radPartyCodeSale = new System.Windows.Forms.RadioButton();
		this.label57 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtTotalFreightSale = new System.Windows.Forms.TextBox();
		this.CmbTaxAccountSale = new UltraCombo();
		this.CmbFreightAccountSale = new UltraCombo();
		this.txtFreightAddLessSale = new System.Windows.Forms.TextBox();
		this.CmbPaymentTermSale = new UltraCombo();
		this.label58 = new System.Windows.Forms.Label();
		this.txtDueDateSale = new System.Windows.Forms.DateTimePicker();
		this.txtFreightAmountSale = new System.Windows.Forms.TextBox();
		this.label67 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.txtDueDaysSale = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.CmbCustomer = new UltraCombo();
		this.label61 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbDeliveryTermSale = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.label107 = new System.Windows.Forms.Label();
		this.label108 = new System.Windows.Forms.Label();
		this.GbWbComparison = new System.Windows.Forms.GroupBox();
		this.txtPurchaseAvgNetRate = new System.Windows.Forms.TextBox();
		this.label68 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.txtDiffAvgNetRate = new System.Windows.Forms.TextBox();
		this.txtBillWeightGrn = new System.Windows.Forms.TextBox();
		this.txtSaleAvgNetRate = new System.Windows.Forms.TextBox();
		this.txtBillWeightDiff = new System.Windows.Forms.TextBox();
		this.txtBillWeightGdn = new System.Windows.Forms.TextBox();
		this.txtSupplierWeight = new System.Windows.Forms.TextBox();
		this.txtSupplierSecondWeight = new System.Windows.Forms.TextBox();
		this.label82 = new System.Windows.Forms.Label();
		this.label83 = new System.Windows.Forms.Label();
		this.txtSupplierFirstWeight = new System.Windows.Forms.TextBox();
		this.txtBuyerWeight = new System.Windows.Forms.TextBox();
		this.txtBuyerSecondWeight = new System.Windows.Forms.TextBox();
		this.txtBuyerFirstWeight = new System.Windows.Forms.TextBox();
		this.txtNetWeightDifference = new System.Windows.Forms.TextBox();
		this.txtSecondWtDifference = new System.Windows.Forms.TextBox();
		this.label80 = new System.Windows.Forms.Label();
		this.txtFirstWtDifference = new System.Windows.Forms.TextBox();
		this.label84 = new System.Windows.Forms.Label();
		this.label85 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.GbMain = new System.Windows.Forms.GroupBox();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.TxtDocNo = new System.Windows.Forms.TextBox();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtMainRemarks = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbTradingAccount = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.GbPurchaseSide = new System.Windows.Forms.GroupBox();
		this.CmbTaxAccountPurchase = new UltraCombo();
		this.txtWhtTaxPercentPurchase = new System.Windows.Forms.TextBox();
		this.txtWhtTaxAmountPurchase = new System.Windows.Forms.TextBox();
		this.txtFreightRemarksPurchase = new System.Windows.Forms.TextBox();
		this.label90 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.RadBusinessNamePurchase = new System.Windows.Forms.RadioButton();
		this.RadNickNamePurchase = new System.Windows.Forms.RadioButton();
		this.radPartyCodePurchase = new System.Windows.Forms.RadioButton();
		this.CmbWhtTaxTypePurchase = new UltraCombo();
		this.txtPurchaseBillAmount = new System.Windows.Forms.TextBox();
		this.label56 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtTotalFreightPurchase = new System.Windows.Forms.TextBox();
		this.label106 = new System.Windows.Forms.Label();
		this.label97 = new System.Windows.Forms.Label();
		this.txtFreightAddLessPurchase = new System.Windows.Forms.TextBox();
		this.CmbWhtAccountPurchase = new UltraCombo();
		this.CmbFreightAccountPurchase = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtFreightAmountPurchase = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.CmbDeliveryTermPurchase = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbPaymentTermPurchase = new UltraCombo();
		this.label64 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtDueDatePurchase = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDueDaysPurchase = new System.Windows.Forms.TextBox();
		this.PanelFormHeading = new System.Windows.Forms.Panel();
		this.txtProfitLoss = new System.Windows.Forms.TextBox();
		this.label88 = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.label89 = new System.Windows.Forms.Label();
		this.CmbCompanyName = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.ChkPreview118 = new System.Windows.Forms.CheckBox();
		this.ChkPreview105603 = new System.Windows.Forms.CheckBox();
		this.ChkPreview105602 = new System.Windows.Forms.CheckBox();
		this.ChkPreview1056A = new System.Windows.Forms.CheckBox();
		this.ChkPreview105601 = new System.Windows.Forms.CheckBox();
		this.ChkPreview1056 = new System.Windows.Forms.CheckBox();
		this.ToolstripPanel = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintII = new System.Windows.Forms.ToolStripButton();
		this.btnPurchasePrint = new System.Windows.Forms.ToolStripButton();
		this.btnSalePrint = new System.Windows.Forms.ToolStripButton();
		this.BtnSalePrintII = new System.Windows.Forms.ToolStripButton();
		this.btnPrint103 = new System.Windows.Forms.ToolStripButton();
		this.btnLoadGdn = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdHistory = new GridEX();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.GrdHistoryDetail = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label42 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.GrdHistoryPurchaseExpenseDetail = new GridEX();
		this.label93 = new System.Windows.Forms.Label();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.GrdHistoryPurchaseFreightDetail = new GridEX();
		this.label94 = new System.Windows.Forms.Label();
		this.tabPage7 = new System.Windows.Forms.TabPage();
		this.GrdHistorySaleExpenseDetail = new GridEX();
		this.label95 = new System.Windows.Forms.Label();
		this.tabPage8 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.GrdHistorySalePmDetail = new GridEX();
		this.label96 = new System.Windows.Forms.Label();
		this.tabPage9 = new System.Windows.Forms.TabPage();
		this.panel21 = new System.Windows.Forms.Panel();
		this.panel22 = new System.Windows.Forms.Panel();
		this.GrdHistorySaleBrokeryDetail = new GridEX();
		this.label102 = new System.Windows.Forms.Label();
		this.panel23 = new System.Windows.Forms.Panel();
		this.GrdHistoryPurchaseBrokeryDetail = new GridEX();
		this.label103 = new System.Windows.Forms.Label();
		this.panel24 = new System.Windows.Forms.Panel();
		this.panel25 = new System.Windows.Forms.Panel();
		this.GrdHistorySaleCommissionDetail = new GridEX();
		this.label104 = new System.Windows.Forms.Label();
		this.panel26 = new System.Windows.Forms.Panel();
		this.GrdHistoryPurchaseCommissionDetail = new GridEX();
		this.label105 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.GrdHistoryPaymentDetailSale = new GridEX();
		this.label91 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GrdHistoryPaymentDetailPurchase = new GridEX();
		this.label92 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label66 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel20 = new System.Windows.Forms.Panel();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.cmbcusthistory = new UltraCombo();
		this.label51 = new System.Windows.Forms.Label();
		this.cmbsupplierhistory = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.cmbperemeter = new UltraCombo();
		this.label47 = new System.Windows.Forms.Label();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label48 = new System.Windows.Forms.Label();
		this.cmbtradAcHistory = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label35 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnGrnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.MainPanel.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.tabPurchaseExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseOtherExpenses).BeginInit();
		this.tabPurchasePmExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEmptyBagTermPurchase).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPurchasePmExpense).BeginInit();
		this.tabSaleExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleOtherExpense).BeginInit();
		this.tabSalePmExpense.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSalePmExpenseSale).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEmptyBagTerm).BeginInit();
		this.tabCommissionAndBrokery.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleBrokery).BeginInit();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseBrokery).BeginInit();
		this.panel12.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleCommission).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseCommission).BeginInit();
		this.tabPaymentDetail.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetailSale).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetailPurchase).BeginInit();
		this.PanelDetailInfo.SuspendLayout();
		this.GbDetailInfo.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxNameSale).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomSale).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxNamePurchase).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomPurchase).BeginInit();
		this.PanelMainInfo.SuspendLayout();
		this.GbSaleSide.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWhtTaxTypeSale).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWhtAccountSale).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccountSale).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAccountSale).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermSale).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermSale).BeginInit();
		this.GbWbComparison.SuspendLayout();
		this.GbMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTradingAccount).BeginInit();
		this.GbPurchaseSide.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccountPurchase).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWhtTaxTypePurchase).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWhtAccountPurchase).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAccountPurchase).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermPurchase).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPurchase).BeginInit();
		this.PanelFormHeading.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyName).BeginInit();
		this.ToolstripPanel.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPurchaseExpenseDetail).BeginInit();
		this.tabPage6.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPurchaseFreightDetail).BeginInit();
		this.tabPage7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistorySaleExpenseDetail).BeginInit();
		this.tabPage8.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistorySalePmDetail).BeginInit();
		this.tabPage9.SuspendLayout();
		this.panel21.SuspendLayout();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistorySaleBrokeryDetail).BeginInit();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPurchaseBrokeryDetail).BeginInit();
		this.panel24.SuspendLayout();
		this.panel25.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistorySaleCommissionDetail).BeginInit();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPurchaseCommissionDetail).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPaymentDetailSale).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPaymentDetailPurchase).BeginInit();
		this.panel9.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcusthistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierhistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtradAcHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.MainPanel.Controls.Add(this.tabControl1);
		this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.MainPanel.Location = new System.Drawing.Point(0, 0);
		this.MainPanel.Name = "MainPanel";
		this.MainPanel.Size = new System.Drawing.Size(1284, 861);
		this.MainPanel.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1284, 861);
		this.tabControl1.TabIndex = 4;
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.PanelDetailInfo);
		this.tabPage1.Controls.Add(this.PanelMainInfo);
		this.tabPage1.Controls.Add(this.PanelFormHeading);
		this.tabPage1.Controls.Add(this.ToolstripPanel);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1276, 832);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabPurchaseExpense);
		this.tabControl2.Controls.Add(this.tabPurchasePmExpense);
		this.tabControl2.Controls.Add(this.tabSaleExpense);
		this.tabControl2.Controls.Add(this.tabSalePmExpense);
		this.tabControl2.Controls.Add(this.tabCommissionAndBrokery);
		this.tabControl2.Controls.Add(this.tabPaymentDetail);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(3, 580);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1270, 249);
		this.tabControl2.TabIndex = 540;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.ctrlGrdBar1);
		this.tabDetail.Controls.Add(this.label34);
		this.tabDetail.Location = new System.Drawing.Point(4, 26);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Size = new System.Drawing.Size(1262, 219);
		this.tabDetail.TabIndex = 5;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1262, 196);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1223, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 22);
		this.ctrlGrdBar1.TabIndex = 540;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label34.BackColor = System.Drawing.Color.Teal;
		this.label34.Dock = System.Windows.Forms.DockStyle.Top;
		this.label34.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(0, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(1262, 23);
		this.label34.TabIndex = 1;
		this.label34.Text = "Detail Grid";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPurchaseExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdPurchaseOtherExpenses);
		this.tabPurchaseExpense.Controls.Add(this.label53);
		this.tabPurchaseExpense.Location = new System.Drawing.Point(4, 26);
		this.tabPurchaseExpense.Name = "tabPurchaseExpense";
		this.tabPurchaseExpense.Padding = new System.Windows.Forms.Padding(3);
		this.tabPurchaseExpense.Size = new System.Drawing.Size(1217, 219);
		this.tabPurchaseExpense.TabIndex = 0;
		this.tabPurchaseExpense.Text = "Purchase Other Expense";
		this.tabPurchaseExpense.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPurchaseOtherExpenses).BackColor = System.Drawing.Color.White;
		grdPurchaseOtherExpenses_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPurchaseOtherExpenses.DesignTimeLayout = grdPurchaseOtherExpenses_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOtherExpenses).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOtherExpenses).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPurchaseOtherExpenses.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOtherExpenses).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grdPurchaseOtherExpenses).Name = "grdPurchaseOtherExpenses";
		((ScrollableBase)this.grdPurchaseOtherExpenses).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOtherExpenses).Size = new System.Drawing.Size(1211, 191);
		((System.Windows.Forms.Control)(object)this.grdPurchaseOtherExpenses).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOtherExpenses).TabStop = false;
		this.grdPurchaseOtherExpenses.TotalRow = (InheritableBoolean)1;
		this.grdPurchaseOtherExpenses.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPurchaseOtherExpenses).VisualStyle = (VisualStyle)4;
		this.grdPurchaseOtherExpenses.CellUpdated += new ColumnActionEventHandler(grdPurchaseOtherExpenses_CellUpdated);
		this.label53.BackColor = System.Drawing.Color.Teal;
		this.label53.Dock = System.Windows.Forms.DockStyle.Top;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(3, 3);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(1211, 22);
		this.label53.TabIndex = 3;
		this.label53.Text = "Credit To Supplier And Debit to Trading Account";
		this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPurchasePmExpense.Controls.Add(this.label45);
		this.tabPurchasePmExpense.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEmptyBagTermPurchase);
		this.tabPurchasePmExpense.Controls.Add(this.panel13);
		this.tabPurchasePmExpense.Controls.Add(this.label43);
		this.tabPurchasePmExpense.Location = new System.Drawing.Point(4, 26);
		this.tabPurchasePmExpense.Name = "tabPurchasePmExpense";
		this.tabPurchasePmExpense.Padding = new System.Windows.Forms.Padding(3);
		this.tabPurchasePmExpense.Size = new System.Drawing.Size(1217, 219);
		this.tabPurchasePmExpense.TabIndex = 1;
		this.tabPurchasePmExpense.Text = "Purchase Packing Material Expense";
		this.tabPurchasePmExpense.UseVisualStyleBackColor = true;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Black;
		this.label45.Location = new System.Drawing.Point(792, 7);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(94, 15);
		this.label45.TabIndex = 678;
		this.label45.Text = "Empty Bag Term";
		this.CmbEmptyBagTermPurchase.AllowNull = (DefaultableBoolean)1;
		this.CmbEmptyBagTermPurchase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEmptyBagTermPurchase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEmptyBagTermPurchase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance307).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance307).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance307).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance318).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance318;
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance329).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance340;
		((AppearanceBase)appearance351).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance351).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance351;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance362).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance362).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance362).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance362).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance362;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEmptyBagTermPurchase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTermPurchase).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbEmptyBagTermPurchase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTermPurchase).Location = new System.Drawing.Point(890, 3);
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTermPurchase).Name = "CmbEmptyBagTermPurchase";
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTermPurchase).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTermPurchase).TabIndex = 677;
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTermPurchase).Leave += new System.EventHandler(CmbEmptyBagTermPurchase_Leave);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdPurchasePmExpense);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 25);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1211, 191);
		this.panel13.TabIndex = 3;
		this.grdPurchasePmExpense.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdPurchasePmExpense).BackColor = System.Drawing.Color.White;
		grdPurchasePmExpense_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPurchasePmExpense.DesignTimeLayout = grdPurchasePmExpense_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPurchasePmExpense).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPurchasePmExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPurchasePmExpense.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPurchasePmExpense).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPurchasePmExpense).Name = "grdPurchasePmExpense";
		((ScrollableBase)this.grdPurchasePmExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPurchasePmExpense).Size = new System.Drawing.Size(1211, 191);
		((System.Windows.Forms.Control)(object)this.grdPurchasePmExpense).TabIndex = 1;
		this.grdPurchasePmExpense.TotalRow = (InheritableBoolean)1;
		this.grdPurchasePmExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPurchasePmExpense).VisualStyle = (VisualStyle)4;
		this.grdPurchasePmExpense.CellUpdated += new ColumnActionEventHandler(grdPurchasePmExpense_CellUpdated);
		this.grdPurchasePmExpense.ColumnButtonClick += new ColumnActionEventHandler(grdPurchasePmExpense_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPurchasePmExpense).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPurchasePmExpense_KeyDown);
		this.label43.BackColor = System.Drawing.Color.Teal;
		this.label43.Dock = System.Windows.Forms.DockStyle.Top;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(3, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(1211, 22);
		this.label43.TabIndex = 2;
		this.label43.Text = "Selected Account Credit , Debit Account On Base of Delivery term(Load=> Debit To Trading Account  && Ponch=> Debit To Supplier Account)";
		this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabSaleExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleOtherExpense);
		this.tabSaleExpense.Controls.Add(this.label54);
		this.tabSaleExpense.Location = new System.Drawing.Point(4, 26);
		this.tabSaleExpense.Name = "tabSaleExpense";
		this.tabSaleExpense.Size = new System.Drawing.Size(1217, 219);
		this.tabSaleExpense.TabIndex = 2;
		this.tabSaleExpense.Text = "Sale Other Expense";
		this.tabSaleExpense.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdSaleOtherExpense).BackColor = System.Drawing.Color.White;
		grdSaleOtherExpense_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSaleOtherExpense.DesignTimeLayout = grdSaleOtherExpense_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSaleOtherExpense).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdSaleOtherExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleOtherExpense.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSaleOtherExpense).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdSaleOtherExpense).Name = "grdSaleOtherExpense";
		((ScrollableBase)this.grdSaleOtherExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleOtherExpense).Size = new System.Drawing.Size(1217, 197);
		((System.Windows.Forms.Control)(object)this.grdSaleOtherExpense).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdSaleOtherExpense).TabStop = false;
		this.grdSaleOtherExpense.TotalRow = (InheritableBoolean)1;
		this.grdSaleOtherExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleOtherExpense).VisualStyle = (VisualStyle)4;
		this.grdSaleOtherExpense.CellUpdated += new ColumnActionEventHandler(grdSaleOtherExpense_CellUpdated);
		this.label54.BackColor = System.Drawing.Color.Teal;
		this.label54.Dock = System.Windows.Forms.DockStyle.Top;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(0, 0);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(1217, 22);
		this.label54.TabIndex = 3;
		this.label54.Text = "Debit To Customer And Credit to Trading Account";
		this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabSalePmExpense.Controls.Add(this.lblebTerm);
		this.tabSalePmExpense.Controls.Add(this.panel14);
		this.tabSalePmExpense.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm);
		this.tabSalePmExpense.Controls.Add(this.label44);
		this.tabSalePmExpense.Location = new System.Drawing.Point(4, 26);
		this.tabSalePmExpense.Name = "tabSalePmExpense";
		this.tabSalePmExpense.Size = new System.Drawing.Size(1217, 219);
		this.tabSalePmExpense.TabIndex = 3;
		this.tabSalePmExpense.Text = "Sale Packing Material Expense";
		this.tabSalePmExpense.UseVisualStyleBackColor = true;
		this.lblebTerm.AutoSize = true;
		this.lblebTerm.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblebTerm.ForeColor = System.Drawing.Color.Black;
		this.lblebTerm.Location = new System.Drawing.Point(799, 4);
		this.lblebTerm.Name = "lblebTerm";
		this.lblebTerm.Size = new System.Drawing.Size(94, 15);
		this.lblebTerm.TabIndex = 676;
		this.lblebTerm.Text = "Empty Bag Term";
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdSalePmExpenseSale);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 22);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1217, 197);
		this.panel14.TabIndex = 4;
		((ScrollableBase)this.grdSalePmExpenseSale).BackColor = System.Drawing.Color.White;
		grdSalePmExpenseSale_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSalePmExpenseSale.DesignTimeLayout = grdSalePmExpenseSale_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSalePmExpenseSale).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdSalePmExpenseSale).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSalePmExpenseSale.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSalePmExpenseSale).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSalePmExpenseSale).Name = "grdSalePmExpenseSale";
		((ScrollableBase)this.grdSalePmExpenseSale).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSalePmExpenseSale).Size = new System.Drawing.Size(1217, 197);
		((System.Windows.Forms.Control)(object)this.grdSalePmExpenseSale).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdSalePmExpenseSale).TabStop = false;
		this.grdSalePmExpenseSale.TotalRow = (InheritableBoolean)1;
		this.grdSalePmExpenseSale.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSalePmExpenseSale).VisualStyle = (VisualStyle)4;
		this.grdSalePmExpenseSale.CellUpdated += new ColumnActionEventHandler(grdSalePmExpense_CellUpdated);
		this.grdSalePmExpenseSale.ColumnButtonClick += new ColumnActionEventHandler(grdSalePmExpense_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdSalePmExpenseSale).KeyDown += new System.Windows.Forms.KeyEventHandler(grdSalePmExpense_KeyDown);
		this.CmbEmptyBagTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbEmptyBagTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEmptyBagTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEmptyBagTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEmptyBagTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbEmptyBagTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).Location = new System.Drawing.Point(897, 0);
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).Name = "CmbEmptyBagTerm";
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).TabIndex = 675;
		((System.Windows.Forms.Control)(object)this.CmbEmptyBagTerm).Leave += new System.EventHandler(CmbEmptyBagTerm_Leave);
		this.label44.BackColor = System.Drawing.Color.Teal;
		this.label44.Dock = System.Windows.Forms.DockStyle.Top;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.White;
		this.label44.Location = new System.Drawing.Point(0, 0);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(1217, 22);
		this.label44.TabIndex = 3;
		this.label44.Text = "Selected Account Credit , Debit Account On Base of Delivery term(Load=> Debit To Customer  && Ponch=> Debit To Trading Account)";
		this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabCommissionAndBrokery.AutoScroll = true;
		this.tabCommissionAndBrokery.Controls.Add(this.panel15);
		this.tabCommissionAndBrokery.Controls.Add(this.panel12);
		this.tabCommissionAndBrokery.Location = new System.Drawing.Point(4, 26);
		this.tabCommissionAndBrokery.Name = "tabCommissionAndBrokery";
		this.tabCommissionAndBrokery.Size = new System.Drawing.Size(1217, 219);
		this.tabCommissionAndBrokery.TabIndex = 4;
		this.tabCommissionAndBrokery.Text = "Commission & Brokery";
		this.tabCommissionAndBrokery.UseVisualStyleBackColor = true;
		this.panel15.Controls.Add(this.panel18);
		this.panel15.Controls.Add(this.panel19);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 150);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1217, 69);
		this.panel15.TabIndex = 543;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleBrokery);
		this.panel18.Controls.Add(this.label100);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel18.Location = new System.Drawing.Point(606, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(606, 69);
		this.panel18.TabIndex = 545;
		((ScrollableBase)this.grdSaleBrokery).BackColor = System.Drawing.Color.White;
		grdSaleBrokery_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSaleBrokery.DesignTimeLayout = grdSaleBrokery_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSaleBrokery).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdSaleBrokery).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleBrokery.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSaleBrokery).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdSaleBrokery).Name = "grdSaleBrokery";
		((ScrollableBase)this.grdSaleBrokery).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleBrokery).Size = new System.Drawing.Size(606, 47);
		((System.Windows.Forms.Control)(object)this.grdSaleBrokery).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdSaleBrokery).TabStop = false;
		this.grdSaleBrokery.TotalRow = (InheritableBoolean)1;
		this.grdSaleBrokery.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleBrokery).VisualStyle = (VisualStyle)4;
		this.grdSaleBrokery.CellUpdated += new ColumnActionEventHandler(grdSaleBrokery_CellUpdated);
		this.grdSaleBrokery.ColumnButtonClick += new ColumnActionEventHandler(grdSaleBrokery_ColumnButtonClick);
		this.grdSaleBrokery.SelectionChanged += new System.EventHandler(grdSaleBrokery_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdSaleBrokery).Click += new System.EventHandler(grdSaleBrokery_Click);
		((System.Windows.Forms.Control)(object)this.grdSaleBrokery).KeyDown += new System.Windows.Forms.KeyEventHandler(grdSaleBrokery_KeyDown);
		this.label100.BackColor = System.Drawing.Color.Teal;
		this.label100.Dock = System.Windows.Forms.DockStyle.Top;
		this.label100.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label100.ForeColor = System.Drawing.Color.White;
		this.label100.Location = new System.Drawing.Point(0, 0);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(606, 22);
		this.label100.TabIndex = 5;
		this.label100.Text = "Sale Brokery Detail";
		this.label100.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery);
		this.panel19.Controls.Add(this.label101);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(606, 69);
		this.panel19.TabIndex = 544;
		((ScrollableBase)this.grdPurchaseBrokery).BackColor = System.Drawing.Color.White;
		grdPurchaseBrokery_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPurchaseBrokery.DesignTimeLayout = grdPurchaseBrokery_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPurchaseBrokery.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery).Name = "grdPurchaseBrokery";
		((ScrollableBase)this.grdPurchaseBrokery).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery).Size = new System.Drawing.Size(606, 47);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery).TabStop = false;
		this.grdPurchaseBrokery.TotalRow = (InheritableBoolean)1;
		this.grdPurchaseBrokery.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPurchaseBrokery).VisualStyle = (VisualStyle)4;
		this.grdPurchaseBrokery.CellUpdated += new ColumnActionEventHandler(grdPurchaseBrokery_CellUpdated);
		this.grdPurchaseBrokery.ColumnButtonClick += new ColumnActionEventHandler(grdPurchaseBrokery_ColumnButtonClick);
		this.grdPurchaseBrokery.SelectionChanged += new System.EventHandler(grdPurchaseBrokery_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery).Click += new System.EventHandler(grdPurchaseBrokery_Click);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrokery).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPurchaseBrokery_KeyDown);
		this.label101.BackColor = System.Drawing.Color.Teal;
		this.label101.Dock = System.Windows.Forms.DockStyle.Top;
		this.label101.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label101.ForeColor = System.Drawing.Color.White;
		this.label101.Location = new System.Drawing.Point(0, 0);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(606, 22);
		this.label101.TabIndex = 5;
		this.label101.Text = "Purchase Brokery Detail";
		this.label101.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel12.Controls.Add(this.panel16);
		this.panel12.Controls.Add(this.panel17);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1217, 150);
		this.panel12.TabIndex = 542;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleCommission);
		this.panel16.Controls.Add(this.label98);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(606, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(606, 150);
		this.panel16.TabIndex = 545;
		((ScrollableBase)this.grdSaleCommission).BackColor = System.Drawing.Color.White;
		grdSaleCommission_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSaleCommission.DesignTimeLayout = grdSaleCommission_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSaleCommission).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdSaleCommission).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleCommission.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSaleCommission).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdSaleCommission).Name = "grdSaleCommission";
		((ScrollableBase)this.grdSaleCommission).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleCommission).Size = new System.Drawing.Size(606, 128);
		((System.Windows.Forms.Control)(object)this.grdSaleCommission).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdSaleCommission).TabStop = false;
		this.grdSaleCommission.TotalRow = (InheritableBoolean)1;
		this.grdSaleCommission.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleCommission).VisualStyle = (VisualStyle)4;
		this.grdSaleCommission.CellUpdated += new ColumnActionEventHandler(grdSaleCommission_CellUpdated);
		this.grdSaleCommission.ColumnButtonClick += new ColumnActionEventHandler(grdSaleCommission_ColumnButtonClick);
		this.grdSaleCommission.SelectionChanged += new System.EventHandler(grdSaleCommission_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdSaleCommission).Click += new System.EventHandler(grdSaleCommission_Click);
		((System.Windows.Forms.Control)(object)this.grdSaleCommission).KeyDown += new System.Windows.Forms.KeyEventHandler(grdSaleCommission_KeyDown);
		this.label98.BackColor = System.Drawing.Color.Teal;
		this.label98.Dock = System.Windows.Forms.DockStyle.Top;
		this.label98.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.ForeColor = System.Drawing.Color.White;
		this.label98.Location = new System.Drawing.Point(0, 0);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(606, 22);
		this.label98.TabIndex = 5;
		this.label98.Text = "Sale Commission Detail";
		this.label98.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdPurchaseCommission);
		this.panel17.Controls.Add(this.label99);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(606, 150);
		this.panel17.TabIndex = 544;
		((ScrollableBase)this.grdPurchaseCommission).BackColor = System.Drawing.Color.White;
		grdPurchaseCommission_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPurchaseCommission.DesignTimeLayout = grdPurchaseCommission_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPurchaseCommission).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPurchaseCommission).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPurchaseCommission.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPurchaseCommission).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPurchaseCommission).Name = "grdPurchaseCommission";
		((ScrollableBase)this.grdPurchaseCommission).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPurchaseCommission).Size = new System.Drawing.Size(606, 128);
		((System.Windows.Forms.Control)(object)this.grdPurchaseCommission).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdPurchaseCommission).TabStop = false;
		this.grdPurchaseCommission.TotalRow = (InheritableBoolean)1;
		this.grdPurchaseCommission.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPurchaseCommission).VisualStyle = (VisualStyle)4;
		this.grdPurchaseCommission.CellUpdated += new ColumnActionEventHandler(grdPurchaseCommission_CellUpdated);
		this.grdPurchaseCommission.ColumnButtonClick += new ColumnActionEventHandler(grdPurchaseCommission_ColumnButtonClick);
		this.grdPurchaseCommission.SelectionChanged += new System.EventHandler(grdPurchaseCommission_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdPurchaseCommission).Click += new System.EventHandler(grdPurchaseCommission_Click);
		((System.Windows.Forms.Control)(object)this.grdPurchaseCommission).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPurchaseCommission_KeyDown);
		this.label99.BackColor = System.Drawing.Color.Teal;
		this.label99.Dock = System.Windows.Forms.DockStyle.Top;
		this.label99.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label99.ForeColor = System.Drawing.Color.White;
		this.label99.Location = new System.Drawing.Point(0, 0);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(606, 22);
		this.label99.TabIndex = 5;
		this.label99.Text = "Purchase Commission Detail";
		this.label99.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPaymentDetail.Controls.Add(this.panel2);
		this.tabPaymentDetail.Controls.Add(this.panel1);
		this.tabPaymentDetail.Location = new System.Drawing.Point(4, 26);
		this.tabPaymentDetail.Name = "tabPaymentDetail";
		this.tabPaymentDetail.Size = new System.Drawing.Size(1217, 219);
		this.tabPaymentDetail.TabIndex = 6;
		this.tabPaymentDetail.Text = "Payment Detail";
		this.tabPaymentDetail.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentDetailSale);
		this.panel2.Controls.Add(this.label87);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(606, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(606, 219);
		this.panel2.TabIndex = 543;
		((ScrollableBase)this.grdPaymentDetailSale).BackColor = System.Drawing.Color.White;
		grdPaymentDetailSale_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentDetailSale.DesignTimeLayout = grdPaymentDetailSale_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailSale).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailSale).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentDetailSale.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailSale).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailSale).Name = "grdPaymentDetailSale";
		((ScrollableBase)this.grdPaymentDetailSale).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailSale).Size = new System.Drawing.Size(606, 197);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailSale).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailSale).TabStop = false;
		this.grdPaymentDetailSale.TotalRow = (InheritableBoolean)1;
		this.grdPaymentDetailSale.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentDetailSale).VisualStyle = (VisualStyle)4;
		this.grdPaymentDetailSale.CellUpdated += new ColumnActionEventHandler(grdPaymentDetailSale_CellUpdated);
		this.grdPaymentDetailSale.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentDetailSale_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailSale).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentDetailSale_KeyDown);
		this.label87.BackColor = System.Drawing.Color.Teal;
		this.label87.Dock = System.Windows.Forms.DockStyle.Top;
		this.label87.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.ForeColor = System.Drawing.Color.White;
		this.label87.Location = new System.Drawing.Point(0, 0);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(606, 22);
		this.label87.TabIndex = 5;
		this.label87.Text = "Buyer Payment Detail";
		this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentDetailPurchase);
		this.panel1.Controls.Add(this.label81);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(606, 219);
		this.panel1.TabIndex = 542;
		((ScrollableBase)this.grdPaymentDetailPurchase).BackColor = System.Drawing.Color.White;
		grdPaymentDetailPurchase_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentDetailPurchase.DesignTimeLayout = grdPaymentDetailPurchase_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailPurchase).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailPurchase).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentDetailPurchase.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailPurchase).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailPurchase).Name = "grdPaymentDetailPurchase";
		((ScrollableBase)this.grdPaymentDetailPurchase).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailPurchase).Size = new System.Drawing.Size(606, 197);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailPurchase).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailPurchase).TabStop = false;
		this.grdPaymentDetailPurchase.TotalRow = (InheritableBoolean)1;
		this.grdPaymentDetailPurchase.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentDetailPurchase).VisualStyle = (VisualStyle)4;
		this.grdPaymentDetailPurchase.CellUpdated += new ColumnActionEventHandler(grdPaymentDetailPurchase_CellUpdated);
		this.grdPaymentDetailPurchase.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentDetailPurchase_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetailPurchase).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentDetailPurchase_KeyDown);
		this.label81.BackColor = System.Drawing.Color.Teal;
		this.label81.Dock = System.Windows.Forms.DockStyle.Top;
		this.label81.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.White;
		this.label81.Location = new System.Drawing.Point(0, 0);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(606, 22);
		this.label81.TabIndex = 5;
		this.label81.Text = "Supplier Payment Detail";
		this.label81.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.PanelDetailInfo.Controls.Add(this.GbDetailInfo);
		this.PanelDetailInfo.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelDetailInfo.Location = new System.Drawing.Point(3, 398);
		this.PanelDetailInfo.Name = "PanelDetailInfo";
		this.PanelDetailInfo.Size = new System.Drawing.Size(1270, 182);
		this.PanelDetailInfo.TabIndex = 1;
		this.GbDetailInfo.Controls.Add(this.txtTotalAmountSale);
		this.GbDetailInfo.Controls.Add(this.txtTaxPercntSale);
		this.GbDetailInfo.Controls.Add(this.txtTaxAmountSale);
		this.GbDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxNameSale);
		this.GbDetailInfo.Controls.Add(this.txtGrossWeightSale);
		this.GbDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomSale);
		this.GbDetailInfo.Controls.Add(this.txtBillWeightSale);
		this.GbDetailInfo.Controls.Add(this.txtItemAmountSale);
		this.GbDetailInfo.Controls.Add(this.txtSaleRateAddLess);
		this.GbDetailInfo.Controls.Add(this.txtSaleRateWithoutAddLess);
		this.GbDetailInfo.Controls.Add(this.txtSaleNetRate);
		this.GbDetailInfo.Controls.Add(this.txtEBTotalSale);
		this.GbDetailInfo.Controls.Add(this.txtAddlessWtSales);
		this.GbDetailInfo.Controls.Add(this.txtEBUnitSale);
		this.GbDetailInfo.Controls.Add(this.txtbiltyno);
		this.GbDetailInfo.Controls.Add(this.txtDetailRemarks);
		this.GbDetailInfo.Controls.Add(this.txtRefDocNo);
		this.GbDetailInfo.Controls.Add(this.txtVehicleNo);
		this.GbDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.GbDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.GbDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackType);
		this.GbDetailInfo.Controls.Add(this.txtQuantity);
		this.GbDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.GbDetailInfo.Controls.Add(this.label79);
		this.GbDetailInfo.Controls.Add(this.btnAddDetail);
		this.GbDetailInfo.Controls.Add(this.label28);
		this.GbDetailInfo.Controls.Add(this.btnUpdateDetail);
		this.GbDetailInfo.Controls.Add(this.btnCancelDetail);
		this.GbDetailInfo.Controls.Add(this.txtTotalAmountPurchase);
		this.GbDetailInfo.Controls.Add(this.txtTaxPercntPurchase);
		this.GbDetailInfo.Controls.Add(this.label75);
		this.GbDetailInfo.Controls.Add(this.label76);
		this.GbDetailInfo.Controls.Add(this.txtTaxAmountPurchase);
		this.GbDetailInfo.Controls.Add(this.label32);
		this.GbDetailInfo.Controls.Add(this.label77);
		this.GbDetailInfo.Controls.Add(this.label27);
		this.GbDetailInfo.Controls.Add(this.label78);
		this.GbDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxNamePurchase);
		this.GbDetailInfo.Controls.Add(this.label71);
		this.GbDetailInfo.Controls.Add(this.label72);
		this.GbDetailInfo.Controls.Add(this.label73);
		this.GbDetailInfo.Controls.Add(this.label74);
		this.GbDetailInfo.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomPurchase);
		this.GbDetailInfo.Controls.Add(this.label17);
		this.GbDetailInfo.Controls.Add(this.rdSearchByCode);
		this.GbDetailInfo.Controls.Add(this.rdSearchByName);
		this.GbDetailInfo.Controls.Add(this.label70);
		this.GbDetailInfo.Controls.Add(this.label116);
		this.GbDetailInfo.Controls.Add(this.label114);
		this.GbDetailInfo.Controls.Add(this.label24);
		this.GbDetailInfo.Controls.Add(this.label29);
		this.GbDetailInfo.Controls.Add(this.label10);
		this.GbDetailInfo.Controls.Add(this.label19);
		this.GbDetailInfo.Controls.Add(this.txtGrossWeightPurchase);
		this.GbDetailInfo.Controls.Add(this.label26);
		this.GbDetailInfo.Controls.Add(this.txtEBTotalPurchase);
		this.GbDetailInfo.Controls.Add(this.label41);
		this.GbDetailInfo.Controls.Add(this.label39);
		this.GbDetailInfo.Controls.Add(this.label20);
		this.GbDetailInfo.Controls.Add(this.label11);
		this.GbDetailInfo.Controls.Add(this.label115);
		this.GbDetailInfo.Controls.Add(this.label113);
		this.GbDetailInfo.Controls.Add(this.label23);
		this.GbDetailInfo.Controls.Add(this.label25);
		this.GbDetailInfo.Controls.Add(this.txtItemAmountPurchase);
		this.GbDetailInfo.Controls.Add(this.label36);
		this.GbDetailInfo.Controls.Add(this.txtPurchaseRateAddLess);
		this.GbDetailInfo.Controls.Add(this.txtPurchaseRateWithoutAddLess);
		this.GbDetailInfo.Controls.Add(this.txtPurchaseNetRate);
		this.GbDetailInfo.Controls.Add(this.label31);
		this.GbDetailInfo.Controls.Add(this.label21);
		this.GbDetailInfo.Controls.Add(this.label37);
		this.GbDetailInfo.Controls.Add(this.txtAddlessWtPurchase);
		this.GbDetailInfo.Controls.Add(this.label69);
		this.GbDetailInfo.Controls.Add(this.label22);
		this.GbDetailInfo.Controls.Add(this.label9);
		this.GbDetailInfo.Controls.Add(this.label40);
		this.GbDetailInfo.Controls.Add(this.txtBillWeightPurchase);
		this.GbDetailInfo.Controls.Add(this.txtEBUnitPurchase);
		this.GbDetailInfo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.GbDetailInfo.ForeColor = System.Drawing.Color.Maroon;
		this.GbDetailInfo.Location = new System.Drawing.Point(1, 3);
		this.GbDetailInfo.Name = "GbDetailInfo";
		this.GbDetailInfo.Size = new System.Drawing.Size(1218, 177);
		this.GbDetailInfo.TabIndex = 0;
		this.GbDetailInfo.TabStop = false;
		this.GbDetailInfo.Text = "Detail";
		this.txtTotalAmountSale.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTotalAmountSale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmountSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmountSale.Location = new System.Drawing.Point(1024, 150);
		this.txtTotalAmountSale.Name = "txtTotalAmountSale";
		this.txtTotalAmountSale.ReadOnly = true;
		this.txtTotalAmountSale.Size = new System.Drawing.Size(110, 23);
		this.txtTotalAmountSale.TabIndex = 36;
		this.txtTotalAmountSale.TabStop = false;
		this.txtTotalAmountSale.Tag = "double";
		this.txtTotalAmountSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercntSale.Enabled = false;
		this.txtTaxPercntSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercntSale.Location = new System.Drawing.Point(877, 150);
		this.txtTaxPercntSale.Name = "txtTaxPercntSale";
		this.txtTaxPercntSale.ShortcutsEnabled = false;
		this.txtTaxPercntSale.Size = new System.Drawing.Size(55, 23);
		this.txtTaxPercntSale.TabIndex = 34;
		this.txtTaxPercntSale.Tag = "double";
		this.txtTaxPercntSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxAmountSale.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTaxAmountSale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmountSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmountSale.Location = new System.Drawing.Point(933, 150);
		this.txtTaxAmountSale.Name = "txtTaxAmountSale";
		this.txtTaxAmountSale.ReadOnly = true;
		this.txtTaxAmountSale.Size = new System.Drawing.Size(90, 23);
		this.txtTaxAmountSale.TabIndex = 35;
		this.txtTaxAmountSale.TabStop = false;
		this.txtTaxAmountSale.Tag = "double";
		this.txtTaxAmountSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbTaxNameSale.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxNameSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxNameSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxNameSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance305).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance305).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance305;
		((AppearanceBase)appearance306).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance306;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance308).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance308).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance308).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance266).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance269;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance270).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance271).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance271).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance271).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance271;
		((AppearanceBase)appearance272).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxNameSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxNameSale).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxNameSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxNameSale).Location = new System.Drawing.Point(776, 148);
		((System.Windows.Forms.Control)(object)this.CmbTaxNameSale).Name = "CmbTaxNameSale";
		((System.Windows.Forms.Control)(object)this.CmbTaxNameSale).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxNameSale).TabIndex = 33;
		((System.Windows.Forms.Control)(object)this.CmbTaxNameSale).Leave += new System.EventHandler(CmbTaxNameSale_Leave);
		this.txtGrossWeightSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeightSale.Location = new System.Drawing.Point(6, 150);
		this.txtGrossWeightSale.Name = "txtGrossWeightSale";
		this.txtGrossWeightSale.Size = new System.Drawing.Size(80, 23);
		this.txtGrossWeightSale.TabIndex = 23;
		this.txtGrossWeightSale.Tag = "double";
		this.txtGrossWeightSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeightSale.TextChanged += new System.EventHandler(txtSaleGrossWeight_TextChanged);
		this.CmbRateUomSale.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance276).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance276).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance277;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance321).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance321).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance321).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance322).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance322;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance323).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance323;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance324;
		((AppearanceBase)appearance325).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance325).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance325;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance326).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance326).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance326).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance326).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance326;
		((AppearanceBase)appearance327).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance328).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance330;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomSale).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUomSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomSale).Location = new System.Drawing.Point(582, 148);
		((System.Windows.Forms.Control)(object)this.CmbRateUomSale).Name = "CmbRateUomSale";
		((System.Windows.Forms.Control)(object)this.CmbRateUomSale).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomSale).TabIndex = 31;
		((System.Windows.Forms.Control)(object)this.CmbRateUomSale).TextChanged += new System.EventHandler(CmbRateUomSale_TextChanged);
		this.txtBillWeightSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightSale.Location = new System.Drawing.Point(285, 150);
		this.txtBillWeightSale.Name = "txtBillWeightSale";
		this.txtBillWeightSale.ReadOnly = true;
		this.txtBillWeightSale.Size = new System.Drawing.Size(80, 23);
		this.txtBillWeightSale.TabIndex = 27;
		this.txtBillWeightSale.TabStop = false;
		this.txtBillWeightSale.Tag = "double";
		this.txtBillWeightSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightSale.TextChanged += new System.EventHandler(txtBillWeightSale_TextChanged);
		this.txtItemAmountSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmountSale.Location = new System.Drawing.Point(674, 150);
		this.txtItemAmountSale.Name = "txtItemAmountSale";
		this.txtItemAmountSale.ReadOnly = true;
		this.txtItemAmountSale.Size = new System.Drawing.Size(100, 23);
		this.txtItemAmountSale.TabIndex = 32;
		this.txtItemAmountSale.TabStop = false;
		this.txtItemAmountSale.Tag = "double";
		this.txtItemAmountSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmountSale.TextChanged += new System.EventHandler(txtItemAmountSale_TextChanged);
		this.txtSaleRateAddLess.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleRateAddLess.Location = new System.Drawing.Point(442, 150);
		this.txtSaleRateAddLess.Name = "txtSaleRateAddLess";
		this.txtSaleRateAddLess.Size = new System.Drawing.Size(62, 23);
		this.txtSaleRateAddLess.TabIndex = 29;
		this.txtSaleRateAddLess.Tag = "double";
		this.txtSaleRateAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSaleRateAddLess.TextChanged += new System.EventHandler(txtSaleRateAddLess_TextChanged);
		this.txtSaleRateWithoutAddLess.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleRateWithoutAddLess.Location = new System.Drawing.Point(366, 150);
		this.txtSaleRateWithoutAddLess.Name = "txtSaleRateWithoutAddLess";
		this.txtSaleRateWithoutAddLess.Size = new System.Drawing.Size(75, 23);
		this.txtSaleRateWithoutAddLess.TabIndex = 28;
		this.txtSaleRateWithoutAddLess.Tag = "double";
		this.txtSaleRateWithoutAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSaleRateWithoutAddLess.TextChanged += new System.EventHandler(txtCustomerRate_TextChanged);
		this.txtSaleNetRate.BackColor = System.Drawing.SystemColors.Control;
		this.txtSaleNetRate.Enabled = false;
		this.txtSaleNetRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleNetRate.Location = new System.Drawing.Point(505, 150);
		this.txtSaleNetRate.Name = "txtSaleNetRate";
		this.txtSaleNetRate.Size = new System.Drawing.Size(75, 23);
		this.txtSaleNetRate.TabIndex = 30;
		this.txtSaleNetRate.Tag = "double";
		this.txtSaleNetRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSaleNetRate.TextChanged += new System.EventHandler(txtSaleNetRate_TextChanged);
		this.txtEBTotalSale.AccessibleName = "SaleEbTotal";
		this.txtEBTotalSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBTotalSale.Location = new System.Drawing.Point(138, 150);
		this.txtEBTotalSale.Name = "txtEBTotalSale";
		this.txtEBTotalSale.Size = new System.Drawing.Size(70, 23);
		this.txtEBTotalSale.TabIndex = 25;
		this.txtEBTotalSale.Tag = "double";
		this.txtEBTotalSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEBTotalSale.TextChanged += new System.EventHandler(txtEBTotalSale_TextChanged);
		this.txtAddlessWtSales.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddlessWtSales.Location = new System.Drawing.Point(209, 150);
		this.txtAddlessWtSales.Name = "txtAddlessWtSales";
		this.txtAddlessWtSales.Size = new System.Drawing.Size(75, 23);
		this.txtAddlessWtSales.TabIndex = 26;
		this.txtAddlessWtSales.Tag = "";
		this.txtAddlessWtSales.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddlessWtSales.TextChanged += new System.EventHandler(txtAddlessWtSales_TextChanged);
		this.txtEBUnitSale.AccessibleName = "SaleEbUnit";
		this.txtEBUnitSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBUnitSale.Location = new System.Drawing.Point(87, 150);
		this.txtEBUnitSale.Name = "txtEBUnitSale";
		this.txtEBUnitSale.Size = new System.Drawing.Size(50, 23);
		this.txtEBUnitSale.TabIndex = 24;
		this.txtEBUnitSale.Tag = "double";
		this.txtEBUnitSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEBUnitSale.TextChanged += new System.EventHandler(txtEBPerUnitSale_TextChanged);
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbiltyno.Location = new System.Drawing.Point(777, 31);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.Size = new System.Drawing.Size(89, 23);
		this.txtbiltyno.TabIndex = 7;
		this.txtDetailRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDetailRemarks.Location = new System.Drawing.Point(867, 31);
		this.txtDetailRemarks.Name = "txtDetailRemarks";
		this.txtDetailRemarks.Size = new System.Drawing.Size(347, 23);
		this.txtDetailRemarks.TabIndex = 8;
		this.txtRefDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefDocNo.Location = new System.Drawing.Point(620, 31);
		this.txtRefDocNo.Name = "txtRefDocNo";
		this.txtRefDocNo.Size = new System.Drawing.Size(66, 23);
		this.txtRefDocNo.TabIndex = 5;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(687, 31);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(89, 23);
		this.txtVehicleNo.TabIndex = 6;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance344).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance344;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance345).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance345).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance345).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance345).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance345;
		((AppearanceBase)appearance346).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance346;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance347).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance347).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance347).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance347;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance348).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance348).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance348;
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance349).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance349;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance350).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance350;
		((AppearanceBase)appearance352).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance352).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance352;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance353).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance353).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance353).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance353).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance353).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance353;
		((AppearanceBase)appearance354).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance354;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance355).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance355).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance355;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance356).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance356;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(207, 29);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 1;
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance357).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance357).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance357;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance358).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance358).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance358).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance358).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance358;
		((AppearanceBase)appearance359).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance359;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance360).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance360).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance360).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance360;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance361).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance361).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance361;
		((AppearanceBase)appearance363).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance363).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance363;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance364).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance364;
		((AppearanceBase)appearance365).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance365).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance365;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance366).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance366).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance366).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance366).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance366).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance366;
		((AppearanceBase)appearance367).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance367;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance368).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance368;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance369).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance369;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(6, 29);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.CmbPackType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance370).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance370).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance370;
		((UltraGridBase)this.CmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance371).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance371).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance371).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance371).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance371;
		((AppearanceBase)appearance372).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance372;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackType).Location = new System.Drawing.Point(309, 29);
		((System.Windows.Forms.Control)(object)this.CmbPackType).Name = "CmbPackType";
		((System.Windows.Forms.Control)(object)this.CmbPackType).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackType).TabIndex = 2;
		this.txtQuantity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQuantity.Location = new System.Drawing.Point(543, 31);
		this.txtQuantity.Name = "txtQuantity";
		this.txtQuantity.Size = new System.Drawing.Size(75, 23);
		this.txtQuantity.TabIndex = 4;
		this.txtQuantity.Tag = "double";
		this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQuantity.TextChanged += new System.EventHandler(txtQuantity_TextChanged);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(451, 29);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(CmbPackUom_Leave);
		this.label79.AutoSize = true;
		this.label79.BackColor = System.Drawing.Color.White;
		this.label79.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label79.ForeColor = System.Drawing.Color.Black;
		this.label79.Location = new System.Drawing.Point(781, 12);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(49, 15);
		this.label79.TabIndex = 674;
		this.label79.Text = "Bilty No";
		this.btnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnAddDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddDetail.Location = new System.Drawing.Point(1138, 149);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(33, 25);
		this.btnAddDetail.TabIndex = 37;
		this.btnAddDetail.Text = "+";
		this.btnAddDetail.UseVisualStyleBackColor = false;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Black;
		this.label28.Location = new System.Drawing.Point(867, 12);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(52, 15);
		this.label28.TabIndex = 516;
		this.label28.Text = "Remarks";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1138, 149);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 25);
		this.btnUpdateDetail.TabIndex = 38;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(1138, 124);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(71, 24);
		this.btnCancelDetail.TabIndex = 39;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.txtTotalAmountPurchase.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTotalAmountPurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmountPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmountPurchase.Location = new System.Drawing.Point(1024, 91);
		this.txtTotalAmountPurchase.Name = "txtTotalAmountPurchase";
		this.txtTotalAmountPurchase.ReadOnly = true;
		this.txtTotalAmountPurchase.Size = new System.Drawing.Size(110, 23);
		this.txtTotalAmountPurchase.TabIndex = 22;
		this.txtTotalAmountPurchase.TabStop = false;
		this.txtTotalAmountPurchase.Tag = "double";
		this.txtTotalAmountPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercntPurchase.Enabled = false;
		this.txtTaxPercntPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercntPurchase.Location = new System.Drawing.Point(877, 91);
		this.txtTaxPercntPurchase.Name = "txtTaxPercntPurchase";
		this.txtTaxPercntPurchase.ShortcutsEnabled = false;
		this.txtTaxPercntPurchase.Size = new System.Drawing.Size(55, 23);
		this.txtTaxPercntPurchase.TabIndex = 20;
		this.txtTaxPercntPurchase.Tag = "double";
		this.txtTaxPercntPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.ForeColor = System.Drawing.Color.Blue;
		this.label75.Location = new System.Drawing.Point(877, 66);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(35, 15);
		this.label75.TabIndex = 670;
		this.label75.Text = "Tax%";
		this.label75.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.ForeColor = System.Drawing.Color.Blue;
		this.label76.Location = new System.Drawing.Point(1028, 58);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(102, 30);
		this.label76.TabIndex = 672;
		this.label76.Text = "Total \r\nPurchase Amount";
		this.label76.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtTaxAmountPurchase.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTaxAmountPurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmountPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmountPurchase.Location = new System.Drawing.Point(933, 91);
		this.txtTaxAmountPurchase.Name = "txtTaxAmountPurchase";
		this.txtTaxAmountPurchase.ReadOnly = true;
		this.txtTaxAmountPurchase.Size = new System.Drawing.Size(90, 23);
		this.txtTaxAmountPurchase.TabIndex = 21;
		this.txtTaxAmountPurchase.TabStop = false;
		this.txtTaxAmountPurchase.Tag = "double";
		this.txtTaxAmountPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Black;
		this.label32.Location = new System.Drawing.Point(620, 12);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(68, 15);
		this.label32.TabIndex = 522;
		this.label32.Text = "Ref Doc No";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.ForeColor = System.Drawing.Color.Blue;
		this.label77.Location = new System.Drawing.Point(933, 58);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(51, 30);
		this.label77.TabIndex = 671;
		this.label77.Text = "Tax\r\nAmount";
		this.label77.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Black;
		this.label27.Location = new System.Drawing.Point(687, 12);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(64, 15);
		this.label27.TabIndex = 520;
		this.label27.Text = "Vehicle No";
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.Blue;
		this.label78.Location = new System.Drawing.Point(776, 66);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(60, 15);
		this.label78.TabIndex = 669;
		this.label78.Text = "Tax Name";
		this.label78.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbTaxNamePurchase.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxNamePurchase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxNamePurchase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxNamePurchase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance268).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxNamePurchase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxNamePurchase).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxNamePurchase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxNamePurchase).Location = new System.Drawing.Point(776, 89);
		((System.Windows.Forms.Control)(object)this.CmbTaxNamePurchase).Name = "CmbTaxNamePurchase";
		((System.Windows.Forms.Control)(object)this.CmbTaxNamePurchase).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxNamePurchase).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbTaxNamePurchase).Leave += new System.EventHandler(CmbTaxNamePurchase_Leave);
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.ForeColor = System.Drawing.Color.Red;
		this.label71.Location = new System.Drawing.Point(877, 126);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(35, 15);
		this.label71.TabIndex = 662;
		this.label71.Text = "Tax%";
		this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.ForeColor = System.Drawing.Color.Red;
		this.label72.Location = new System.Drawing.Point(1041, 118);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(76, 30);
		this.label72.TabIndex = 664;
		this.label72.Text = "Total \r\nSale Amount";
		this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.ForeColor = System.Drawing.Color.Red;
		this.label73.Location = new System.Drawing.Point(933, 118);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(51, 30);
		this.label73.TabIndex = 663;
		this.label73.Text = "Tax\r\nAmount";
		this.label73.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.ForeColor = System.Drawing.Color.Red;
		this.label74.Location = new System.Drawing.Point(776, 126);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(60, 15);
		this.label74.TabIndex = 661;
		this.label74.Text = "Tax Name";
		this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbRateUomPurchase.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomPurchase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomPurchase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomPurchase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance331).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance331).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance332).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance332).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance332).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance332;
		((AppearanceBase)appearance333).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance333;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance334).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance334).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance334).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance335).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance335;
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance336).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance338).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance338).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance338;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance339).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance339).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance339).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance339).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance339;
		((AppearanceBase)appearance341).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance342).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance342;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance343).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomPurchase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomPurchase).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUomPurchase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomPurchase).Location = new System.Drawing.Point(582, 89);
		((System.Windows.Forms.Control)(object)this.CmbRateUomPurchase).Name = "CmbRateUomPurchase";
		((System.Windows.Forms.Control)(object)this.CmbRateUomPurchase).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomPurchase).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbRateUomPurchase).TextChanged += new System.EventHandler(CmbRateUom_TextChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(23, 118);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(46, 30);
		this.label17.TabIndex = 541;
		this.label17.Text = "Gross\r\nWeight";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.ForeColor = System.Drawing.Color.Black;
		this.rdSearchByCode.Location = new System.Drawing.Point(98, 10);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 539;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.ForeColor = System.Drawing.Color.Black;
		this.rdSearchByName.Location = new System.Drawing.Point(39, 10);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 538;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.ForeColor = System.Drawing.Color.Red;
		this.label70.Location = new System.Drawing.Point(598, 126);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(59, 15);
		this.label70.TabIndex = 528;
		this.label70.Text = "Rate Uom";
		this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label116.ForeColor = System.Drawing.Color.Red;
		this.label116.Location = new System.Drawing.Point(442, 118);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(62, 30);
		this.label116.TabIndex = 509;
		this.label116.Text = "Rate \r\nAdd / Less";
		this.label116.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label114.ForeColor = System.Drawing.Color.Red;
		this.label114.Location = new System.Drawing.Point(388, 118);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(30, 30);
		this.label114.TabIndex = 509;
		this.label114.Text = "Sale\r\nRate";
		this.label114.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(516, 118);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 30);
		this.label24.TabIndex = 509;
		this.label24.Text = "Sale\r\nNet Rate";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Blue;
		this.label29.Location = new System.Drawing.Point(598, 66);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(59, 15);
		this.label29.TabIndex = 528;
		this.label29.Text = "Rate Uom";
		this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Black;
		this.label10.Location = new System.Drawing.Point(6, 12);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(35, 15);
		this.label10.TabIndex = 2;
		this.label10.Text = "Item ";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Black;
		this.label19.Location = new System.Drawing.Point(309, 12);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 499;
		this.label19.Text = "Pack Type";
		this.txtGrossWeightPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeightPurchase.Location = new System.Drawing.Point(6, 91);
		this.txtGrossWeightPurchase.Name = "txtGrossWeightPurchase";
		this.txtGrossWeightPurchase.Size = new System.Drawing.Size(80, 23);
		this.txtGrossWeightPurchase.TabIndex = 9;
		this.txtGrossWeightPurchase.Tag = "double";
		this.txtGrossWeightPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeightPurchase.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(699, 118);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 30);
		this.label26.TabIndex = 513;
		this.label26.Text = "Sale\r\nAmount";
		this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtEBTotalPurchase.AccessibleName = "PurcahseEbTotal";
		this.txtEBTotalPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBTotalPurchase.Location = new System.Drawing.Point(138, 91);
		this.txtEBTotalPurchase.Name = "txtEBTotalPurchase";
		this.txtEBTotalPurchase.Size = new System.Drawing.Size(70, 23);
		this.txtEBTotalPurchase.TabIndex = 11;
		this.txtEBTotalPurchase.Tag = "double";
		this.txtEBTotalPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEBTotalPurchase.TextChanged += new System.EventHandler(txtEBTotalPurchase_TextChanged);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(302, 118);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(46, 30);
		this.label41.TabIndex = 535;
		this.label41.Text = "Net Bill\r\nWeight";
		this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(149, 126);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(49, 15);
		this.label39.TabIndex = 537;
		this.label39.Text = "EB Total";
		this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Black;
		this.label20.Location = new System.Drawing.Point(451, 12);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(61, 15);
		this.label20.TabIndex = 501;
		this.label20.Text = "Pack Uom";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Black;
		this.label11.Location = new System.Drawing.Point(207, 12);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(57, 15);
		this.label11.TabIndex = 497;
		this.label11.Text = "Crop Year";
		this.label115.AutoSize = true;
		this.label115.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label115.ForeColor = System.Drawing.Color.Blue;
		this.label115.Location = new System.Drawing.Point(442, 58);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(62, 30);
		this.label115.TabIndex = 507;
		this.label115.Text = "Rate \r\nAdd / Less";
		this.label115.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label113.ForeColor = System.Drawing.Color.Blue;
		this.label113.Location = new System.Drawing.Point(376, 58);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(55, 30);
		this.label113.TabIndex = 507;
		this.label113.Text = "Purchase\r\nRate";
		this.label113.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Blue;
		this.label23.Location = new System.Drawing.Point(515, 58);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(55, 30);
		this.label23.TabIndex = 507;
		this.label23.Text = "Purchase\r\nNet Rate";
		this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Blue;
		this.label25.Location = new System.Drawing.Point(697, 58);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(55, 30);
		this.label25.TabIndex = 511;
		this.label25.Text = "Purchase\r\nAmount";
		this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtItemAmountPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmountPurchase.Location = new System.Drawing.Point(674, 91);
		this.txtItemAmountPurchase.Name = "txtItemAmountPurchase";
		this.txtItemAmountPurchase.ReadOnly = true;
		this.txtItemAmountPurchase.Size = new System.Drawing.Size(100, 23);
		this.txtItemAmountPurchase.TabIndex = 18;
		this.txtItemAmountPurchase.TabStop = false;
		this.txtItemAmountPurchase.Tag = "double";
		this.txtItemAmountPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmountPurchase.TextChanged += new System.EventHandler(txtItemAmountPurchase_TextChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Blue;
		this.label36.Location = new System.Drawing.Point(149, 66);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(49, 15);
		this.label36.TabIndex = 526;
		this.label36.Text = "EB Total";
		this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtPurchaseRateAddLess.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseRateAddLess.Location = new System.Drawing.Point(442, 91);
		this.txtPurchaseRateAddLess.Name = "txtPurchaseRateAddLess";
		this.txtPurchaseRateAddLess.Size = new System.Drawing.Size(62, 23);
		this.txtPurchaseRateAddLess.TabIndex = 15;
		this.txtPurchaseRateAddLess.Tag = "double";
		this.txtPurchaseRateAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPurchaseRateAddLess.TextChanged += new System.EventHandler(txtPurchaseRateAddLess_TextChanged);
		this.txtPurchaseRateWithoutAddLess.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseRateWithoutAddLess.Location = new System.Drawing.Point(366, 91);
		this.txtPurchaseRateWithoutAddLess.Name = "txtPurchaseRateWithoutAddLess";
		this.txtPurchaseRateWithoutAddLess.Size = new System.Drawing.Size(75, 23);
		this.txtPurchaseRateWithoutAddLess.TabIndex = 14;
		this.txtPurchaseRateWithoutAddLess.Tag = "double";
		this.txtPurchaseRateWithoutAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPurchaseRateWithoutAddLess.TextChanged += new System.EventHandler(txtSupplierRate_TextChanged);
		this.txtPurchaseNetRate.BackColor = System.Drawing.SystemColors.Control;
		this.txtPurchaseNetRate.Enabled = false;
		this.txtPurchaseNetRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseNetRate.Location = new System.Drawing.Point(505, 91);
		this.txtPurchaseNetRate.Name = "txtPurchaseNetRate";
		this.txtPurchaseNetRate.Size = new System.Drawing.Size(75, 23);
		this.txtPurchaseNetRate.TabIndex = 16;
		this.txtPurchaseNetRate.Tag = "double";
		this.txtPurchaseNetRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPurchaseNetRate.TextChanged += new System.EventHandler(txtPurchaseNetRate_TextChanged);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Blue;
		this.label31.Location = new System.Drawing.Point(302, 58);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(46, 30);
		this.label31.TabIndex = 518;
		this.label31.Text = "Net Bill\r\nWeight";
		this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Black;
		this.label21.Location = new System.Drawing.Point(543, 12);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(53, 15);
		this.label21.TabIndex = 503;
		this.label21.Text = "Quantity";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Blue;
		this.label37.Location = new System.Drawing.Point(89, 66);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(47, 15);
		this.label37.TabIndex = 525;
		this.label37.Text = "EB/Unit";
		this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtAddlessWtPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddlessWtPurchase.Location = new System.Drawing.Point(209, 91);
		this.txtAddlessWtPurchase.Name = "txtAddlessWtPurchase";
		this.txtAddlessWtPurchase.Size = new System.Drawing.Size(75, 23);
		this.txtAddlessWtPurchase.TabIndex = 12;
		this.txtAddlessWtPurchase.Tag = "";
		this.txtAddlessWtPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddlessWtPurchase.TextChanged += new System.EventHandler(txtAddlessWtPurchase_TextChanged);
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.Blue;
		this.label69.Location = new System.Drawing.Point(218, 58);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(56, 30);
		this.label69.TabIndex = 536;
		this.label69.Text = "Add/Less\r\nWeight";
		this.label69.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Blue;
		this.label22.Location = new System.Drawing.Point(23, 58);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(46, 30);
		this.label22.TabIndex = 505;
		this.label22.Text = "Gross\r\nWeight";
		this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(218, 118);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(56, 30);
		this.label9.TabIndex = 536;
		this.label9.Text = "Add/Less\r\nWeight";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(89, 126);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(47, 15);
		this.label40.TabIndex = 536;
		this.label40.Text = "EB/Unit";
		this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtBillWeightPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightPurchase.Location = new System.Drawing.Point(285, 91);
		this.txtBillWeightPurchase.Name = "txtBillWeightPurchase";
		this.txtBillWeightPurchase.ReadOnly = true;
		this.txtBillWeightPurchase.Size = new System.Drawing.Size(80, 23);
		this.txtBillWeightPurchase.TabIndex = 13;
		this.txtBillWeightPurchase.TabStop = false;
		this.txtBillWeightPurchase.Tag = "double";
		this.txtBillWeightPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightPurchase.TextChanged += new System.EventHandler(txtBillWeightPurchase_TextChanged);
		this.txtEBUnitPurchase.AccessibleName = "PurcahseEbUnit";
		this.txtEBUnitPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBUnitPurchase.Location = new System.Drawing.Point(87, 91);
		this.txtEBUnitPurchase.Name = "txtEBUnitPurchase";
		this.txtEBUnitPurchase.Size = new System.Drawing.Size(50, 23);
		this.txtEBUnitPurchase.TabIndex = 10;
		this.txtEBUnitPurchase.Tag = "double";
		this.txtEBUnitPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEBUnitPurchase.TextChanged += new System.EventHandler(txtEBPerUnit_TextChanged);
		this.PanelMainInfo.Controls.Add(this.GbSaleSide);
		this.PanelMainInfo.Controls.Add(this.GbWbComparison);
		this.PanelMainInfo.Controls.Add(this.GbMain);
		this.PanelMainInfo.Controls.Add(this.GbPurchaseSide);
		this.PanelMainInfo.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelMainInfo.Location = new System.Drawing.Point(3, 58);
		this.PanelMainInfo.Name = "PanelMainInfo";
		this.PanelMainInfo.Size = new System.Drawing.Size(1270, 340);
		this.PanelMainInfo.TabIndex = 0;
		this.GbSaleSide.Controls.Add(this.txtWhtTaxPercentSale);
		this.GbSaleSide.Controls.Add(this.txtwhtTaxAmountSale);
		this.GbSaleSide.Controls.Add(this.label109);
		this.GbSaleSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypeSale);
		this.GbSaleSide.Controls.Add(this.txtSaleBillAmount);
		this.GbSaleSide.Controls.Add(this.label33);
		this.GbSaleSide.Controls.Add(this.label110);
		this.GbSaleSide.Controls.Add(this.label111);
		this.GbSaleSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWhtAccountSale);
		this.GbSaleSide.Controls.Add(this.label112);
		this.GbSaleSide.Controls.Add(this.txtFreightRemarksSale);
		this.GbSaleSide.Controls.Add(this.panel10);
		this.GbSaleSide.Controls.Add(this.label57);
		this.GbSaleSide.Controls.Add(this.label5);
		this.GbSaleSide.Controls.Add(this.txtTotalFreightSale);
		this.GbSaleSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccountSale);
		this.GbSaleSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFreightAccountSale);
		this.GbSaleSide.Controls.Add(this.txtFreightAddLessSale);
		this.GbSaleSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermSale);
		this.GbSaleSide.Controls.Add(this.label58);
		this.GbSaleSide.Controls.Add(this.txtDueDateSale);
		this.GbSaleSide.Controls.Add(this.txtFreightAmountSale);
		this.GbSaleSide.Controls.Add(this.label67);
		this.GbSaleSide.Controls.Add(this.label59);
		this.GbSaleSide.Controls.Add(this.txtDueDaysSale);
		this.GbSaleSide.Controls.Add(this.label60);
		this.GbSaleSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.GbSaleSide.Controls.Add(this.label61);
		this.GbSaleSide.Controls.Add(this.label12);
		this.GbSaleSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale);
		this.GbSaleSide.Controls.Add(this.label55);
		this.GbSaleSide.Controls.Add(this.label107);
		this.GbSaleSide.Controls.Add(this.label108);
		this.GbSaleSide.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.GbSaleSide.ForeColor = System.Drawing.Color.Red;
		this.GbSaleSide.Location = new System.Drawing.Point(828, 3);
		this.GbSaleSide.Name = "GbSaleSide";
		this.GbSaleSide.Size = new System.Drawing.Size(392, 337);
		this.GbSaleSide.TabIndex = 3;
		this.GbSaleSide.TabStop = false;
		this.GbSaleSide.Text = "Sale Side";
		this.txtWhtTaxPercentSale.BackColor = System.Drawing.SystemColors.Control;
		this.txtWhtTaxPercentSale.Enabled = false;
		this.txtWhtTaxPercentSale.Location = new System.Drawing.Point(202, 279);
		this.txtWhtTaxPercentSale.Name = "txtWhtTaxPercentSale";
		this.txtWhtTaxPercentSale.ReadOnly = true;
		this.txtWhtTaxPercentSale.Size = new System.Drawing.Size(50, 22);
		this.txtWhtTaxPercentSale.TabIndex = 14;
		this.txtWhtTaxPercentSale.TabStop = false;
		this.txtWhtTaxPercentSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtwhtTaxAmountSale.BackColor = System.Drawing.SystemColors.Control;
		this.txtwhtTaxAmountSale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwhtTaxAmountSale.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwhtTaxAmountSale.Location = new System.Drawing.Point(298, 278);
		this.txtwhtTaxAmountSale.Name = "txtwhtTaxAmountSale";
		this.txtwhtTaxAmountSale.ReadOnly = true;
		this.txtwhtTaxAmountSale.Size = new System.Drawing.Size(89, 25);
		this.txtwhtTaxAmountSale.TabIndex = 15;
		this.txtwhtTaxAmountSale.TabStop = false;
		this.txtwhtTaxAmountSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label109.AutoSize = true;
		this.label109.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold);
		this.label109.ForeColor = System.Drawing.Color.Black;
		this.label109.Location = new System.Drawing.Point(251, 284);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(50, 13);
		this.label109.TabIndex = 688;
		this.label109.Text = "Amount";
		this.CmbWhtTaxTypeSale.AllowNull = (DefaultableBoolean)1;
		this.CmbWhtTaxTypeSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWhtTaxTypeSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWhtTaxTypeSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance309).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance309).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance309;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance310;
		((SpecialBoxBase)((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance311).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance311).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance311;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance312;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance314;
		((AppearanceBase)appearance315).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance315).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance316).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance316).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance316).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance316).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance316;
		((AppearanceBase)appearance317).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance319).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWhtTaxTypeSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypeSale).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWhtTaxTypeSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypeSale).Location = new System.Drawing.Point(77, 278);
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypeSale).Name = "CmbWhtTaxTypeSale";
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypeSale).Size = new System.Drawing.Size(90, 24);
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypeSale).TabIndex = 13;
		this.CmbWhtTaxTypeSale.ValueChanged += new System.EventHandler(CmbWhtTaxTypeSale_Leave);
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypeSale).Leave += new System.EventHandler(CmbWhtTaxTypeSale_Leave);
		this.txtSaleBillAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtSaleBillAmount.Location = new System.Drawing.Point(202, 306);
		this.txtSaleBillAmount.Name = "txtSaleBillAmount";
		this.txtSaleBillAmount.ReadOnly = true;
		this.txtSaleBillAmount.Size = new System.Drawing.Size(186, 25);
		this.txtSaleBillAmount.TabIndex = 16;
		this.txtSaleBillAmount.TabStop = false;
		this.txtSaleBillAmount.Tag = "double";
		this.txtSaleBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSaleBillAmount.TextChanged += new System.EventHandler(txtSaleBillAmount_TextChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 15f, System.Drawing.FontStyle.Bold);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(70, 303);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(125, 28);
		this.label33.TabIndex = 497;
		this.label33.Text = "Bill Amount";
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold);
		this.label110.ForeColor = System.Drawing.Color.Black;
		this.label110.Location = new System.Drawing.Point(171, 277);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(24, 26);
		this.label110.TabIndex = 689;
		this.label110.Text = "Tax\r\n%";
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold);
		this.label111.ForeColor = System.Drawing.Color.Black;
		this.label111.Location = new System.Drawing.Point(4, 284);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(52, 13);
		this.label111.TabIndex = 691;
		this.label111.Text = "Tax Type";
		this.CmbWhtAccountSale.AllowNull = (DefaultableBoolean)1;
		this.CmbWhtAccountSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWhtAccountSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWhtAccountSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance241;
		((SpecialBoxBase)((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance247).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWhtAccountSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountSale).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWhtAccountSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountSale).Location = new System.Drawing.Point(77, 248);
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountSale).Name = "CmbWhtAccountSale";
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountSale).Size = new System.Drawing.Size(310, 26);
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountSale).TabIndex = 12;
		this.CmbWhtAccountSale.ValueChanged += new System.EventHandler(CmbWhtAccountSale_Leave);
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountSale).Leave += new System.EventHandler(CmbWhtAccountSale_Leave);
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label112.ForeColor = System.Drawing.Color.Black;
		this.label112.Location = new System.Drawing.Point(4, 254);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(76, 15);
		this.label112.TabIndex = 686;
		this.label112.Text = "Account (Dr)";
		this.txtFreightRemarksSale.BackColor = System.Drawing.Color.White;
		this.txtFreightRemarksSale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightRemarksSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightRemarksSale.Location = new System.Drawing.Point(77, 207);
		this.txtFreightRemarksSale.Name = "txtFreightRemarksSale";
		this.txtFreightRemarksSale.Size = new System.Drawing.Size(310, 23);
		this.txtFreightRemarksSale.TabIndex = 11;
		this.panel10.Controls.Add(this.RadBusinessNameSale);
		this.panel10.Controls.Add(this.RadNickNameSale);
		this.panel10.Controls.Add(this.radPartyCodeSale);
		this.panel10.ForeColor = System.Drawing.Color.Black;
		this.panel10.Location = new System.Drawing.Point(111, 6);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(277, 22);
		this.panel10.TabIndex = 0;
		this.RadBusinessNameSale.AutoSize = true;
		this.RadBusinessNameSale.Checked = true;
		this.RadBusinessNameSale.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBusinessNameSale.Location = new System.Drawing.Point(2, 3);
		this.RadBusinessNameSale.Name = "RadBusinessNameSale";
		this.RadBusinessNameSale.Size = new System.Drawing.Size(123, 17);
		this.RadBusinessNameSale.TabIndex = 2;
		this.RadBusinessNameSale.TabStop = true;
		this.RadBusinessNameSale.Text = "Business Name";
		this.RadBusinessNameSale.UseVisualStyleBackColor = true;
		this.RadBusinessNameSale.CheckedChanged += new System.EventHandler(RadBusinessNameSale_CheckedChanged);
		this.RadNickNameSale.AutoSize = true;
		this.RadNickNameSale.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNickNameSale.Location = new System.Drawing.Point(126, 3);
		this.RadNickNameSale.Name = "RadNickNameSale";
		this.RadNickNameSale.Size = new System.Drawing.Size(94, 17);
		this.RadNickNameSale.TabIndex = 65;
		this.RadNickNameSale.Text = "Nick Name";
		this.RadNickNameSale.UseVisualStyleBackColor = true;
		this.RadNickNameSale.CheckedChanged += new System.EventHandler(RadBusinessNameSale_CheckedChanged);
		this.radPartyCodeSale.AutoSize = true;
		this.radPartyCodeSale.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radPartyCodeSale.Location = new System.Drawing.Point(221, 4);
		this.radPartyCodeSale.Name = "radPartyCodeSale";
		this.radPartyCodeSale.Size = new System.Drawing.Size(57, 17);
		this.radPartyCodeSale.TabIndex = 65;
		this.radPartyCodeSale.Text = "Code";
		this.radPartyCodeSale.UseVisualStyleBackColor = true;
		this.radPartyCodeSale.CheckedChanged += new System.EventHandler(RadBusinessNameSale_CheckedChanged);
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.ForeColor = System.Drawing.Color.Black;
		this.label57.Location = new System.Drawing.Point(5, 211);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(52, 15);
		this.label57.TabIndex = 678;
		this.label57.Text = "Remarks";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(5, 114);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(73, 15);
		this.label5.TabIndex = 533;
		this.label5.Text = "Tax Account";
		this.txtTotalFreightSale.BackColor = System.Drawing.SystemColors.Control;
		this.txtTotalFreightSale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFreightSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFreightSale.Location = new System.Drawing.Point(298, 180);
		this.txtTotalFreightSale.Name = "txtTotalFreightSale";
		this.txtTotalFreightSale.Size = new System.Drawing.Size(89, 23);
		this.txtTotalFreightSale.TabIndex = 10;
		this.txtTotalFreightSale.Tag = "double";
		this.txtTotalFreightSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalFreightSale.TextChanged += new System.EventHandler(txtTotalFreightSale_TextChanged);
		this.txtTotalFreightSale.Leave += new System.EventHandler(txtTotalFreightSale_Leave);
		this.CmbTaxAccountSale.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccountSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccountSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccountSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccountSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountSale).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccountSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountSale).Location = new System.Drawing.Point(111, 108);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountSale).Name = "CmbTaxAccountSale";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountSale).Size = new System.Drawing.Size(277, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountSale).TabIndex = 6;
		this.CmbFreightAccountSale.AllowNull = (DefaultableBoolean)1;
		this.CmbFreightAccountSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFreightAccountSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFreightAccountSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFreightAccountSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountSale).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFreightAccountSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountSale).Location = new System.Drawing.Point(77, 149);
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountSale).Name = "CmbFreightAccountSale";
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountSale).Size = new System.Drawing.Size(310, 26);
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountSale).TabIndex = 7;
		this.CmbFreightAccountSale.ValueChanged += new System.EventHandler(CmbFreightAccountSale_Leave);
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountSale).Leave += new System.EventHandler(CmbFreightAccountSale_Leave);
		this.txtFreightAddLessSale.BackColor = System.Drawing.Color.White;
		this.txtFreightAddLessSale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAddLessSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAddLessSale.Location = new System.Drawing.Point(200, 180);
		this.txtFreightAddLessSale.Name = "txtFreightAddLessSale";
		this.txtFreightAddLessSale.Size = new System.Drawing.Size(50, 23);
		this.txtFreightAddLessSale.TabIndex = 9;
		this.txtFreightAddLessSale.Tag = "double";
		this.txtFreightAddLessSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAddLessSale.TextChanged += new System.EventHandler(txtFreightAddLessSale_TextChanged);
		this.txtFreightAddLessSale.Leave += new System.EventHandler(txtFreightAddLessSale_Leave);
		this.CmbPaymentTermSale.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermSale).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermSale).Location = new System.Drawing.Point(227, 56);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermSale).Name = "CmbPaymentTermSale";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermSale).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermSale).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermSale).TextChanged += new System.EventHandler(CmbPaymentTermSale_TextChanged);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Black;
		this.label58.Location = new System.Drawing.Point(254, 176);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(44, 30);
		this.label58.TabIndex = 676;
		this.label58.Text = "Net\r\nFreight";
		this.txtDueDateSale.CustomFormat = "dd-MMM-yyyy";
		this.txtDueDateSale.Enabled = false;
		this.txtDueDateSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDateSale.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDueDateSale.Location = new System.Drawing.Point(227, 83);
		this.txtDueDateSale.Name = "txtDueDateSale";
		this.txtDueDateSale.Size = new System.Drawing.Size(160, 23);
		this.txtDueDateSale.TabIndex = 5;
		this.txtFreightAmountSale.BackColor = System.Drawing.Color.White;
		this.txtFreightAmountSale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmountSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAmountSale.Location = new System.Drawing.Point(77, 180);
		this.txtFreightAmountSale.Name = "txtFreightAmountSale";
		this.txtFreightAmountSale.Size = new System.Drawing.Size(90, 23);
		this.txtFreightAmountSale.TabIndex = 8;
		this.txtFreightAmountSale.Tag = "double";
		this.txtFreightAmountSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAmountSale.TextChanged += new System.EventHandler(txtFreightAmountSale_TextChanged);
		this.txtFreightAmountSale.Leave += new System.EventHandler(txtFreightAmountSale_Leave);
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.ForeColor = System.Drawing.Color.Black;
		this.label67.Location = new System.Drawing.Point(5, 87);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(98, 15);
		this.label67.TabIndex = 491;
		this.label67.Text = "Due Days && Date";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.Black;
		this.label59.Location = new System.Drawing.Point(169, 178);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(34, 26);
		this.label59.TabIndex = 675;
		this.label59.Text = "Add/\r\nLess";
		this.txtDueDaysSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDaysSale.Location = new System.Drawing.Point(111, 83);
		this.txtDueDaysSale.Name = "txtDueDaysSale";
		this.txtDueDaysSale.Size = new System.Drawing.Size(115, 23);
		this.txtDueDaysSale.TabIndex = 4;
		this.txtDueDaysSale.Tag = "int";
		this.txtDueDaysSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDueDaysSale.TextChanged += new System.EventHandler(txtDueDaysSale_TextChanged);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Black;
		this.label60.Location = new System.Drawing.Point(5, 155);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(74, 15);
		this.label60.TabIndex = 471;
		this.label60.Text = "Account (Cr)";
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(111, 29);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(276, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TextChanged += new System.EventHandler(CmbCustomer_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Leave += new System.EventHandler(CmbCustomer_Leave);
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Black;
		this.label61.Location = new System.Drawing.Point(5, 176);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(51, 30);
		this.label61.TabIndex = 677;
		this.label61.Text = "Freight\r\nAmount";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Black;
		this.label12.Location = new System.Drawing.Point(5, 35);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(93, 15);
		this.label12.TabIndex = 482;
		this.label12.Text = "Customer Name";
		this.CmbDeliveryTermSale.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTermSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTermSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTermSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTermSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).Location = new System.Drawing.Point(111, 56);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).Name = "CmbDeliveryTermSale";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).TextChanged += new System.EventHandler(CmbDeliveryTermSale_TextChanged);
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.ForeColor = System.Drawing.Color.Black;
		this.label55.Location = new System.Drawing.Point(5, 52);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(93, 32);
		this.label55.TabIndex = 486;
		this.label55.Text = "Delivery && Payment Term";
		this.label107.AutoSize = true;
		this.label107.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label107.ForeColor = System.Drawing.Color.Red;
		this.label107.Location = new System.Drawing.Point(5, 132);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(117, 14);
		this.label107.TabIndex = 471;
		this.label107.Text = "____Freight___";
		this.label108.AutoSize = true;
		this.label108.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label108.ForeColor = System.Drawing.Color.Red;
		this.label108.Location = new System.Drawing.Point(5, 230);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(102, 14);
		this.label108.TabIndex = 471;
		this.label108.Text = "____WHT___";
		this.GbWbComparison.Controls.Add(this.txtPurchaseAvgNetRate);
		this.GbWbComparison.Controls.Add(this.label68);
		this.GbWbComparison.Controls.Add(this.label62);
		this.GbWbComparison.Controls.Add(this.txtDiffAvgNetRate);
		this.GbWbComparison.Controls.Add(this.txtBillWeightGrn);
		this.GbWbComparison.Controls.Add(this.txtSaleAvgNetRate);
		this.GbWbComparison.Controls.Add(this.txtBillWeightDiff);
		this.GbWbComparison.Controls.Add(this.txtBillWeightGdn);
		this.GbWbComparison.Controls.Add(this.txtSupplierWeight);
		this.GbWbComparison.Controls.Add(this.txtSupplierSecondWeight);
		this.GbWbComparison.Controls.Add(this.label82);
		this.GbWbComparison.Controls.Add(this.label83);
		this.GbWbComparison.Controls.Add(this.txtSupplierFirstWeight);
		this.GbWbComparison.Controls.Add(this.txtBuyerWeight);
		this.GbWbComparison.Controls.Add(this.txtBuyerSecondWeight);
		this.GbWbComparison.Controls.Add(this.txtBuyerFirstWeight);
		this.GbWbComparison.Controls.Add(this.txtNetWeightDifference);
		this.GbWbComparison.Controls.Add(this.txtSecondWtDifference);
		this.GbWbComparison.Controls.Add(this.label80);
		this.GbWbComparison.Controls.Add(this.txtFirstWtDifference);
		this.GbWbComparison.Controls.Add(this.label84);
		this.GbWbComparison.Controls.Add(this.label85);
		this.GbWbComparison.Controls.Add(this.label86);
		this.GbWbComparison.Font = new System.Drawing.Font("Tahoma", 8.5f, System.Drawing.FontStyle.Bold);
		this.GbWbComparison.ForeColor = System.Drawing.Color.Maroon;
		this.GbWbComparison.Location = new System.Drawing.Point(397, 165);
		this.GbWbComparison.Name = "GbWbComparison";
		this.GbWbComparison.Size = new System.Drawing.Size(426, 175);
		this.GbWbComparison.TabIndex = 2;
		this.GbWbComparison.TabStop = false;
		this.GbWbComparison.Text = "Weigh-Bridge Weight Comparison";
		this.txtPurchaseAvgNetRate.BackColor = System.Drawing.SystemColors.Control;
		this.txtPurchaseAvgNetRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPurchaseAvgNetRate.Enabled = false;
		this.txtPurchaseAvgNetRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseAvgNetRate.Location = new System.Drawing.Point(86, 146);
		this.txtPurchaseAvgNetRate.Name = "txtPurchaseAvgNetRate";
		this.txtPurchaseAvgNetRate.Size = new System.Drawing.Size(110, 23);
		this.txtPurchaseAvgNetRate.TabIndex = 12;
		this.txtPurchaseAvgNetRate.Tag = "double";
		this.txtPurchaseAvgNetRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPurchaseAvgNetRate.TextChanged += new System.EventHandler(txtBillWeightGrn_TextChanged);
		this.txtPurchaseAvgNetRate.Leave += new System.EventHandler(txtBillWeightGrn_Leave);
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Black;
		this.label68.Location = new System.Drawing.Point(5, 150);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(82, 15);
		this.label68.TabIndex = 675;
		this.label68.Text = "Avg Net Rate";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.Black;
		this.label62.Location = new System.Drawing.Point(5, 114);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(68, 15);
		this.label62.TabIndex = 675;
		this.label62.Text = "Bill Weight";
		this.txtDiffAvgNetRate.BackColor = System.Drawing.SystemColors.Control;
		this.txtDiffAvgNetRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiffAvgNetRate.Enabled = false;
		this.txtDiffAvgNetRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiffAvgNetRate.Location = new System.Drawing.Point(309, 146);
		this.txtDiffAvgNetRate.Name = "txtDiffAvgNetRate";
		this.txtDiffAvgNetRate.Size = new System.Drawing.Size(110, 23);
		this.txtDiffAvgNetRate.TabIndex = 14;
		this.txtDiffAvgNetRate.Tag = "double";
		this.txtDiffAvgNetRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightGrn.BackColor = System.Drawing.Color.White;
		this.txtBillWeightGrn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillWeightGrn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightGrn.Location = new System.Drawing.Point(86, 110);
		this.txtBillWeightGrn.Name = "txtBillWeightGrn";
		this.txtBillWeightGrn.Size = new System.Drawing.Size(110, 23);
		this.txtBillWeightGrn.TabIndex = 3;
		this.txtBillWeightGrn.Tag = "double";
		this.txtBillWeightGrn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightGrn.TextChanged += new System.EventHandler(txtBillWeightGrn_TextChanged);
		this.txtBillWeightGrn.Leave += new System.EventHandler(txtBillWeightGrn_Leave);
		this.txtSaleAvgNetRate.BackColor = System.Drawing.SystemColors.Control;
		this.txtSaleAvgNetRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSaleAvgNetRate.Enabled = false;
		this.txtSaleAvgNetRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleAvgNetRate.Location = new System.Drawing.Point(198, 146);
		this.txtSaleAvgNetRate.Name = "txtSaleAvgNetRate";
		this.txtSaleAvgNetRate.Size = new System.Drawing.Size(110, 23);
		this.txtSaleAvgNetRate.TabIndex = 13;
		this.txtSaleAvgNetRate.Tag = "double";
		this.txtSaleAvgNetRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSaleAvgNetRate.TextChanged += new System.EventHandler(txtBillWeightGdn_TextChanged);
		this.txtSaleAvgNetRate.Leave += new System.EventHandler(txtBillWeightGdn_Leave);
		this.txtBillWeightDiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtBillWeightDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillWeightDiff.Enabled = false;
		this.txtBillWeightDiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightDiff.Location = new System.Drawing.Point(309, 110);
		this.txtBillWeightDiff.Name = "txtBillWeightDiff";
		this.txtBillWeightDiff.Size = new System.Drawing.Size(110, 23);
		this.txtBillWeightDiff.TabIndex = 11;
		this.txtBillWeightDiff.Tag = "double";
		this.txtBillWeightDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightGdn.BackColor = System.Drawing.Color.White;
		this.txtBillWeightGdn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillWeightGdn.Enabled = false;
		this.txtBillWeightGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightGdn.Location = new System.Drawing.Point(198, 110);
		this.txtBillWeightGdn.Name = "txtBillWeightGdn";
		this.txtBillWeightGdn.Size = new System.Drawing.Size(110, 23);
		this.txtBillWeightGdn.TabIndex = 7;
		this.txtBillWeightGdn.Tag = "double";
		this.txtBillWeightGdn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightGdn.TextChanged += new System.EventHandler(txtBillWeightGdn_TextChanged);
		this.txtBillWeightGdn.Leave += new System.EventHandler(txtBillWeightGdn_Leave);
		this.txtSupplierWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtSupplierWeight.ForeColor = System.Drawing.Color.Blue;
		this.txtSupplierWeight.Location = new System.Drawing.Point(86, 85);
		this.txtSupplierWeight.Name = "txtSupplierWeight";
		this.txtSupplierWeight.Size = new System.Drawing.Size(110, 24);
		this.txtSupplierWeight.TabIndex = 2;
		this.txtSupplierWeight.TabStop = false;
		this.txtSupplierWeight.Tag = "double";
		this.txtSupplierWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupplierWeight.TextChanged += new System.EventHandler(txtSupplierWeight_TextChanged);
		this.txtSupplierSecondWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierSecondWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtSupplierSecondWeight.ForeColor = System.Drawing.Color.Blue;
		this.txtSupplierSecondWeight.Location = new System.Drawing.Point(86, 60);
		this.txtSupplierSecondWeight.Name = "txtSupplierSecondWeight";
		this.txtSupplierSecondWeight.Size = new System.Drawing.Size(110, 24);
		this.txtSupplierSecondWeight.TabIndex = 1;
		this.txtSupplierSecondWeight.Tag = "double";
		this.txtSupplierSecondWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupplierSecondWeight.TextChanged += new System.EventHandler(txtSupplierSecondWeight_TextChanged);
		this.txtSupplierSecondWeight.Leave += new System.EventHandler(txtSupplierSecondWeight_Leave);
		this.label82.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label82.ForeColor = System.Drawing.Color.Red;
		this.label82.Location = new System.Drawing.Point(198, 16);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(110, 18);
		this.label82.TabIndex = 270;
		this.label82.Text = "Buyer";
		this.label82.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label83.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label83.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label83.ForeColor = System.Drawing.Color.Blue;
		this.label83.Location = new System.Drawing.Point(86, 16);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(110, 18);
		this.label83.TabIndex = 271;
		this.label83.Text = "Supplier";
		this.label83.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtSupplierFirstWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierFirstWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtSupplierFirstWeight.ForeColor = System.Drawing.Color.Blue;
		this.txtSupplierFirstWeight.Location = new System.Drawing.Point(86, 35);
		this.txtSupplierFirstWeight.Name = "txtSupplierFirstWeight";
		this.txtSupplierFirstWeight.Size = new System.Drawing.Size(110, 24);
		this.txtSupplierFirstWeight.TabIndex = 0;
		this.txtSupplierFirstWeight.Tag = "double";
		this.txtSupplierFirstWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupplierFirstWeight.TextChanged += new System.EventHandler(txtSupplierFirstWeight_TextChanged);
		this.txtSupplierFirstWeight.Leave += new System.EventHandler(txtSupplierFirstWeight_Leave);
		this.txtBuyerWeight.BackColor = System.Drawing.Color.White;
		this.txtBuyerWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBuyerWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtBuyerWeight.ForeColor = System.Drawing.Color.Red;
		this.txtBuyerWeight.Location = new System.Drawing.Point(198, 85);
		this.txtBuyerWeight.Name = "txtBuyerWeight";
		this.txtBuyerWeight.ReadOnly = true;
		this.txtBuyerWeight.Size = new System.Drawing.Size(110, 24);
		this.txtBuyerWeight.TabIndex = 6;
		this.txtBuyerWeight.TabStop = false;
		this.txtBuyerWeight.Tag = "double";
		this.txtBuyerWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBuyerWeight.TextChanged += new System.EventHandler(txtBuyerWeight_TextChanged);
		this.txtBuyerSecondWeight.BackColor = System.Drawing.Color.White;
		this.txtBuyerSecondWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBuyerSecondWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtBuyerSecondWeight.ForeColor = System.Drawing.Color.Red;
		this.txtBuyerSecondWeight.Location = new System.Drawing.Point(198, 60);
		this.txtBuyerSecondWeight.Name = "txtBuyerSecondWeight";
		this.txtBuyerSecondWeight.Size = new System.Drawing.Size(110, 24);
		this.txtBuyerSecondWeight.TabIndex = 5;
		this.txtBuyerSecondWeight.Tag = "double";
		this.txtBuyerSecondWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBuyerSecondWeight.TextChanged += new System.EventHandler(txtBuyerSecondWeight_TextChanged);
		this.txtBuyerSecondWeight.Leave += new System.EventHandler(txtBuyerSecondWeight_Leave);
		this.txtBuyerFirstWeight.BackColor = System.Drawing.Color.White;
		this.txtBuyerFirstWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBuyerFirstWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtBuyerFirstWeight.ForeColor = System.Drawing.Color.Red;
		this.txtBuyerFirstWeight.Location = new System.Drawing.Point(198, 35);
		this.txtBuyerFirstWeight.Name = "txtBuyerFirstWeight";
		this.txtBuyerFirstWeight.Size = new System.Drawing.Size(110, 24);
		this.txtBuyerFirstWeight.TabIndex = 4;
		this.txtBuyerFirstWeight.Tag = "double";
		this.txtBuyerFirstWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBuyerFirstWeight.TextChanged += new System.EventHandler(txtBuyerFirstWeight_TextChanged);
		this.txtBuyerFirstWeight.Leave += new System.EventHandler(txtBuyerFirstWeight_Leave);
		this.txtNetWeightDifference.BackColor = System.Drawing.SystemColors.Control;
		this.txtNetWeightDifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightDifference.Enabled = false;
		this.txtNetWeightDifference.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtNetWeightDifference.Location = new System.Drawing.Point(309, 85);
		this.txtNetWeightDifference.Name = "txtNetWeightDifference";
		this.txtNetWeightDifference.Size = new System.Drawing.Size(110, 24);
		this.txtNetWeightDifference.TabIndex = 10;
		this.txtNetWeightDifference.TabStop = false;
		this.txtNetWeightDifference.Tag = "double";
		this.txtNetWeightDifference.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSecondWtDifference.BackColor = System.Drawing.SystemColors.Control;
		this.txtSecondWtDifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSecondWtDifference.Enabled = false;
		this.txtSecondWtDifference.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtSecondWtDifference.Location = new System.Drawing.Point(309, 60);
		this.txtSecondWtDifference.Name = "txtSecondWtDifference";
		this.txtSecondWtDifference.Size = new System.Drawing.Size(110, 24);
		this.txtSecondWtDifference.TabIndex = 9;
		this.txtSecondWtDifference.TabStop = false;
		this.txtSecondWtDifference.Tag = "double";
		this.txtSecondWtDifference.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label80.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label80.ForeColor = System.Drawing.Color.Black;
		this.label80.Location = new System.Drawing.Point(309, 16);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(110, 18);
		this.label80.TabIndex = 269;
		this.label80.Text = "Differences";
		this.label80.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtFirstWtDifference.BackColor = System.Drawing.SystemColors.Control;
		this.txtFirstWtDifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFirstWtDifference.Enabled = false;
		this.txtFirstWtDifference.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtFirstWtDifference.Location = new System.Drawing.Point(309, 35);
		this.txtFirstWtDifference.Name = "txtFirstWtDifference";
		this.txtFirstWtDifference.Size = new System.Drawing.Size(110, 24);
		this.txtFirstWtDifference.TabIndex = 8;
		this.txtFirstWtDifference.TabStop = false;
		this.txtFirstWtDifference.Tag = "double";
		this.txtFirstWtDifference.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25f, System.Drawing.FontStyle.Bold);
		this.label84.ForeColor = System.Drawing.Color.Black;
		this.label84.Location = new System.Drawing.Point(4, 64);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(81, 17);
		this.label84.TabIndex = 268;
		this.label84.Text = "Tare Weight";
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25f, System.Drawing.FontStyle.Bold);
		this.label85.ForeColor = System.Drawing.Color.Black;
		this.label85.Location = new System.Drawing.Point(4, 39);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(85, 17);
		this.label85.TabIndex = 267;
		this.label85.Text = "Load Weight";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25f, System.Drawing.FontStyle.Bold);
		this.label86.ForeColor = System.Drawing.Color.Black;
		this.label86.Location = new System.Drawing.Point(4, 89);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(78, 17);
		this.label86.TabIndex = 266;
		this.label86.Text = "Net Weight";
		this.GbMain.Controls.Add(this.txtDocDate);
		this.GbMain.Controls.Add(this.TxtDocNo);
		this.GbMain.Controls.Add(this.txtBranchSrNo);
		this.GbMain.Controls.Add(this.label30);
		this.GbMain.Controls.Add(this.label3);
		this.GbMain.Controls.Add(this.txtMainRemarks);
		this.GbMain.Controls.Add(this.label15);
		this.GbMain.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTradingAccount);
		this.GbMain.Controls.Add(this.label38);
		this.GbMain.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GbMain.ForeColor = System.Drawing.Color.Maroon;
		this.GbMain.Location = new System.Drawing.Point(397, 3);
		this.GbMain.Name = "GbMain";
		this.GbMain.Size = new System.Drawing.Size(426, 157);
		this.GbMain.TabIndex = 0;
		this.GbMain.TabStop = false;
		this.GbMain.Text = "Main";
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(293, 21);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(130, 22);
		this.txtDocDate.TabIndex = 2;
		this.txtDocDate.ValueChanged += new System.EventHandler(txtDocDate_ValueChanged);
		this.txtDocDate.Leave += new System.EventHandler(txtDocDate_ValueChanged);
		this.TxtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TxtDocNo.Location = new System.Drawing.Point(97, 21);
		this.TxtDocNo.Name = "TxtDocNo";
		this.TxtDocNo.ReadOnly = true;
		this.TxtDocNo.Size = new System.Drawing.Size(65, 23);
		this.TxtDocNo.TabIndex = 0;
		this.TxtDocNo.TabStop = false;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchSrNo.Location = new System.Drawing.Point(163, 21);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.ReadOnly = true;
		this.txtBranchSrNo.Size = new System.Drawing.Size(70, 23);
		this.txtBranchSrNo.TabIndex = 1;
		this.txtBranchSrNo.TabStop = false;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Black;
		this.label30.Location = new System.Drawing.Point(235, 25);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(57, 15);
		this.label30.TabIndex = 464;
		this.label30.Text = "Doc Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(4, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(77, 30);
		this.label3.TabIndex = 464;
		this.label3.Text = "Doc  && \r\nBranch Sr No";
		this.txtMainRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMainRemarks.Location = new System.Drawing.Point(97, 74);
		this.txtMainRemarks.Multiline = true;
		this.txtMainRemarks.Name = "txtMainRemarks";
		this.txtMainRemarks.Size = new System.Drawing.Size(327, 77);
		this.txtMainRemarks.TabIndex = 4;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Black;
		this.label15.Location = new System.Drawing.Point(4, 76);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 485;
		this.label15.Text = "Remarks";
		this.CmbTradingAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTradingAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTradingAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTradingAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradingAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradingAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTradingAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).Location = new System.Drawing.Point(97, 46);
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).Name = "CmbTradingAccount";
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).Size = new System.Drawing.Size(327, 26);
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).TabIndex = 3;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Black;
		this.label38.Location = new System.Drawing.Point(4, 52);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(95, 15);
		this.label38.TabIndex = 530;
		this.label38.Text = "Trading Account";
		this.GbPurchaseSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccountPurchase);
		this.GbPurchaseSide.Controls.Add(this.txtWhtTaxPercentPurchase);
		this.GbPurchaseSide.Controls.Add(this.txtWhtTaxAmountPurchase);
		this.GbPurchaseSide.Controls.Add(this.txtFreightRemarksPurchase);
		this.GbPurchaseSide.Controls.Add(this.label90);
		this.GbPurchaseSide.Controls.Add(this.label16);
		this.GbPurchaseSide.Controls.Add(this.panel8);
		this.GbPurchaseSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypePurchase);
		this.GbPurchaseSide.Controls.Add(this.txtPurchaseBillAmount);
		this.GbPurchaseSide.Controls.Add(this.label56);
		this.GbPurchaseSide.Controls.Add(this.label4);
		this.GbPurchaseSide.Controls.Add(this.txtTotalFreightPurchase);
		this.GbPurchaseSide.Controls.Add(this.label106);
		this.GbPurchaseSide.Controls.Add(this.label97);
		this.GbPurchaseSide.Controls.Add(this.txtFreightAddLessPurchase);
		this.GbPurchaseSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWhtAccountPurchase);
		this.GbPurchaseSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFreightAccountPurchase);
		this.GbPurchaseSide.Controls.Add(this.label13);
		this.GbPurchaseSide.Controls.Add(this.label1);
		this.GbPurchaseSide.Controls.Add(this.txtFreightAmountPurchase);
		this.GbPurchaseSide.Controls.Add(this.label7);
		this.GbPurchaseSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.GbPurchaseSide.Controls.Add(this.label14);
		this.GbPurchaseSide.Controls.Add(this.label8);
		this.GbPurchaseSide.Controls.Add(this.label65);
		this.GbPurchaseSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTermPurchase);
		this.GbPurchaseSide.Controls.Add(this.label6);
		this.GbPurchaseSide.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermPurchase);
		this.GbPurchaseSide.Controls.Add(this.label64);
		this.GbPurchaseSide.Controls.Add(this.label63);
		this.GbPurchaseSide.Controls.Add(this.txtDueDatePurchase);
		this.GbPurchaseSide.Controls.Add(this.label2);
		this.GbPurchaseSide.Controls.Add(this.txtDueDaysPurchase);
		this.GbPurchaseSide.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.GbPurchaseSide.ForeColor = System.Drawing.Color.Blue;
		this.GbPurchaseSide.Location = new System.Drawing.Point(1, 3);
		this.GbPurchaseSide.Name = "GbPurchaseSide";
		this.GbPurchaseSide.Size = new System.Drawing.Size(392, 337);
		this.GbPurchaseSide.TabIndex = 1;
		this.GbPurchaseSide.TabStop = false;
		this.GbPurchaseSide.Text = "Purchase Side";
		this.CmbTaxAccountPurchase.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccountPurchase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccountPurchase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccountPurchase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccountPurchase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountPurchase).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccountPurchase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountPurchase).Location = new System.Drawing.Point(105, 108);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountPurchase).Name = "CmbTaxAccountPurchase";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountPurchase).Size = new System.Drawing.Size(282, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccountPurchase).TabIndex = 6;
		this.txtWhtTaxPercentPurchase.BackColor = System.Drawing.SystemColors.Control;
		this.txtWhtTaxPercentPurchase.Enabled = false;
		this.txtWhtTaxPercentPurchase.Location = new System.Drawing.Point(202, 280);
		this.txtWhtTaxPercentPurchase.Name = "txtWhtTaxPercentPurchase";
		this.txtWhtTaxPercentPurchase.ReadOnly = true;
		this.txtWhtTaxPercentPurchase.Size = new System.Drawing.Size(50, 22);
		this.txtWhtTaxPercentPurchase.TabIndex = 14;
		this.txtWhtTaxPercentPurchase.TabStop = false;
		this.txtWhtTaxPercentPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWhtTaxAmountPurchase.BackColor = System.Drawing.SystemColors.Control;
		this.txtWhtTaxAmountPurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWhtTaxAmountPurchase.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWhtTaxAmountPurchase.Location = new System.Drawing.Point(298, 278);
		this.txtWhtTaxAmountPurchase.Name = "txtWhtTaxAmountPurchase";
		this.txtWhtTaxAmountPurchase.ReadOnly = true;
		this.txtWhtTaxAmountPurchase.Size = new System.Drawing.Size(89, 25);
		this.txtWhtTaxAmountPurchase.TabIndex = 15;
		this.txtWhtTaxAmountPurchase.TabStop = false;
		this.txtWhtTaxAmountPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightRemarksPurchase.BackColor = System.Drawing.Color.White;
		this.txtFreightRemarksPurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightRemarksPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightRemarksPurchase.Location = new System.Drawing.Point(79, 207);
		this.txtFreightRemarksPurchase.Name = "txtFreightRemarksPurchase";
		this.txtFreightRemarksPurchase.Size = new System.Drawing.Size(308, 23);
		this.txtFreightRemarksPurchase.TabIndex = 11;
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold);
		this.label90.ForeColor = System.Drawing.Color.Black;
		this.label90.Location = new System.Drawing.Point(251, 284);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(50, 13);
		this.label90.TabIndex = 680;
		this.label90.Text = "Amount";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 15f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.Blue;
		this.label16.Location = new System.Drawing.Point(69, 303);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(125, 28);
		this.label16.TabIndex = 498;
		this.label16.Text = "Bill Amount";
		this.panel8.Controls.Add(this.RadBusinessNamePurchase);
		this.panel8.Controls.Add(this.RadNickNamePurchase);
		this.panel8.Controls.Add(this.radPartyCodePurchase);
		this.panel8.ForeColor = System.Drawing.Color.Black;
		this.panel8.Location = new System.Drawing.Point(105, 5);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(283, 22);
		this.panel8.TabIndex = 0;
		this.RadBusinessNamePurchase.AutoSize = true;
		this.RadBusinessNamePurchase.Checked = true;
		this.RadBusinessNamePurchase.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBusinessNamePurchase.Location = new System.Drawing.Point(3, 3);
		this.RadBusinessNamePurchase.Name = "RadBusinessNamePurchase";
		this.RadBusinessNamePurchase.Size = new System.Drawing.Size(123, 17);
		this.RadBusinessNamePurchase.TabIndex = 0;
		this.RadBusinessNamePurchase.TabStop = true;
		this.RadBusinessNamePurchase.Text = "Business Name";
		this.RadBusinessNamePurchase.UseVisualStyleBackColor = true;
		this.RadBusinessNamePurchase.CheckedChanged += new System.EventHandler(RadBusinessNamePurchase_CheckedChanged);
		this.RadNickNamePurchase.AutoSize = true;
		this.RadNickNamePurchase.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNickNamePurchase.Location = new System.Drawing.Point(126, 3);
		this.RadNickNamePurchase.Name = "RadNickNamePurchase";
		this.RadNickNamePurchase.Size = new System.Drawing.Size(94, 17);
		this.RadNickNamePurchase.TabIndex = 65;
		this.RadNickNamePurchase.Text = "Nick Name";
		this.RadNickNamePurchase.UseVisualStyleBackColor = true;
		this.RadNickNamePurchase.CheckedChanged += new System.EventHandler(RadBusinessNamePurchase_CheckedChanged);
		this.radPartyCodePurchase.AutoSize = true;
		this.radPartyCodePurchase.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radPartyCodePurchase.Location = new System.Drawing.Point(220, 4);
		this.radPartyCodePurchase.Name = "radPartyCodePurchase";
		this.radPartyCodePurchase.Size = new System.Drawing.Size(57, 17);
		this.radPartyCodePurchase.TabIndex = 65;
		this.radPartyCodePurchase.Text = "Code";
		this.radPartyCodePurchase.UseVisualStyleBackColor = true;
		this.radPartyCodePurchase.CheckedChanged += new System.EventHandler(RadBusinessNamePurchase_CheckedChanged);
		this.CmbWhtTaxTypePurchase.AllowNull = (DefaultableBoolean)1;
		this.CmbWhtTaxTypePurchase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWhtTaxTypePurchase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWhtTaxTypePurchase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance281).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance281;
		((SpecialBoxBase)((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance282).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance286).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance287).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWhtTaxTypePurchase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypePurchase).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWhtTaxTypePurchase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypePurchase).Location = new System.Drawing.Point(79, 277);
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypePurchase).Name = "CmbWhtTaxTypePurchase";
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypePurchase).Size = new System.Drawing.Size(88, 24);
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypePurchase).TabIndex = 13;
		this.CmbWhtTaxTypePurchase.ValueChanged += new System.EventHandler(CmbWhtTaxTypePurchase_Leave);
		((System.Windows.Forms.Control)(object)this.CmbWhtTaxTypePurchase).Leave += new System.EventHandler(CmbWhtTaxTypePurchase_Leave);
		this.txtPurchaseBillAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtPurchaseBillAmount.Location = new System.Drawing.Point(201, 306);
		this.txtPurchaseBillAmount.Name = "txtPurchaseBillAmount";
		this.txtPurchaseBillAmount.ReadOnly = true;
		this.txtPurchaseBillAmount.Size = new System.Drawing.Size(186, 25);
		this.txtPurchaseBillAmount.TabIndex = 16;
		this.txtPurchaseBillAmount.TabStop = false;
		this.txtPurchaseBillAmount.Tag = "double";
		this.txtPurchaseBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPurchaseBillAmount.TextChanged += new System.EventHandler(txtPurchaseBillAmount_TextChanged);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.ForeColor = System.Drawing.Color.Black;
		this.label56.Location = new System.Drawing.Point(6, 211);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(52, 15);
		this.label56.TabIndex = 678;
		this.label56.Text = "Remarks";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(6, 114);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(73, 15);
		this.label4.TabIndex = 532;
		this.label4.Text = "Tax Account";
		this.txtTotalFreightPurchase.BackColor = System.Drawing.SystemColors.Control;
		this.txtTotalFreightPurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFreightPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFreightPurchase.Location = new System.Drawing.Point(298, 180);
		this.txtTotalFreightPurchase.Name = "txtTotalFreightPurchase";
		this.txtTotalFreightPurchase.Size = new System.Drawing.Size(89, 23);
		this.txtTotalFreightPurchase.TabIndex = 10;
		this.txtTotalFreightPurchase.Tag = "double";
		this.txtTotalFreightPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalFreightPurchase.TextChanged += new System.EventHandler(txtTotalFreightPurchase_TextChanged);
		this.txtTotalFreightPurchase.Leave += new System.EventHandler(txtTotalFreightPurchase_Leave);
		this.label106.AutoSize = true;
		this.label106.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold);
		this.label106.ForeColor = System.Drawing.Color.Black;
		this.label106.Location = new System.Drawing.Point(171, 277);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(24, 26);
		this.label106.TabIndex = 681;
		this.label106.Text = "Tax\r\n%";
		this.label97.AutoSize = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold);
		this.label97.ForeColor = System.Drawing.Color.Black;
		this.label97.Location = new System.Drawing.Point(4, 282);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(52, 13);
		this.label97.TabIndex = 683;
		this.label97.Text = "Tax Type";
		this.txtFreightAddLessPurchase.BackColor = System.Drawing.Color.White;
		this.txtFreightAddLessPurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAddLessPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAddLessPurchase.Location = new System.Drawing.Point(202, 180);
		this.txtFreightAddLessPurchase.Name = "txtFreightAddLessPurchase";
		this.txtFreightAddLessPurchase.Size = new System.Drawing.Size(50, 23);
		this.txtFreightAddLessPurchase.TabIndex = 9;
		this.txtFreightAddLessPurchase.Tag = "double";
		this.txtFreightAddLessPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAddLessPurchase.TextChanged += new System.EventHandler(txtFreightAddLessPurchase_TextChanged);
		this.txtFreightAddLessPurchase.Leave += new System.EventHandler(txtFreightAddLessPurchase_Leave);
		this.CmbWhtAccountPurchase.AllowNull = (DefaultableBoolean)1;
		this.CmbWhtAccountPurchase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWhtAccountPurchase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWhtAccountPurchase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance293).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance293).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance294).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance294;
		((SpecialBoxBase)((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance295).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance295).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance295).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance296;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance299).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance300).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance300).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance300).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance300).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance302).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance302;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWhtAccountPurchase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountPurchase).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWhtAccountPurchase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountPurchase).Location = new System.Drawing.Point(79, 248);
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountPurchase).Name = "CmbWhtAccountPurchase";
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountPurchase).Size = new System.Drawing.Size(308, 26);
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountPurchase).TabIndex = 12;
		this.CmbWhtAccountPurchase.ValueChanged += new System.EventHandler(CmbWhtAccountPurchase_Leave);
		((System.Windows.Forms.Control)(object)this.CmbWhtAccountPurchase).Leave += new System.EventHandler(CmbWhtAccountPurchase_Leave);
		this.CmbFreightAccountPurchase.AllowNull = (DefaultableBoolean)1;
		this.CmbFreightAccountPurchase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFreightAccountPurchase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFreightAccountPurchase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance254;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFreightAccountPurchase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountPurchase).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFreightAccountPurchase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountPurchase).Location = new System.Drawing.Point(79, 149);
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountPurchase).Name = "CmbFreightAccountPurchase";
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountPurchase).Size = new System.Drawing.Size(308, 26);
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountPurchase).TabIndex = 7;
		this.CmbFreightAccountPurchase.ValueChanged += new System.EventHandler(CmbFreightAccountPurchase_Leave);
		((System.Windows.Forms.Control)(object)this.CmbFreightAccountPurchase).Leave += new System.EventHandler(CmbFreightAccountPurchase_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Black;
		this.label13.Location = new System.Drawing.Point(254, 176);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 30);
		this.label13.TabIndex = 676;
		this.label13.Text = "Net\r\nFreight";
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(6, 52);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 33);
		this.label1.TabIndex = 498;
		this.label1.Text = "Delivery && Payment Term";
		this.txtFreightAmountPurchase.BackColor = System.Drawing.Color.White;
		this.txtFreightAmountPurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmountPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAmountPurchase.Location = new System.Drawing.Point(79, 180);
		this.txtFreightAmountPurchase.Name = "txtFreightAmountPurchase";
		this.txtFreightAmountPurchase.Size = new System.Drawing.Size(90, 23);
		this.txtFreightAmountPurchase.TabIndex = 8;
		this.txtFreightAmountPurchase.Tag = "double";
		this.txtFreightAmountPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAmountPurchase.TextChanged += new System.EventHandler(txtFreightAmountPurchase_TextChanged);
		this.txtFreightAmountPurchase.Leave += new System.EventHandler(txtFreightAmountPurchase_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(171, 178);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(34, 26);
		this.label7.TabIndex = 675;
		this.label7.Text = "Add/\r\nLess";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(105, 28);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(282, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TextChanged += new System.EventHandler(CmbSupplier_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Leave += new System.EventHandler(CmbSupplier_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(6, 176);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 30);
		this.label14.TabIndex = 677;
		this.label14.Text = "Freight\r\nAmount";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(6, 34);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(86, 15);
		this.label8.TabIndex = 471;
		this.label8.Text = "Supplier Name";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Black;
		this.label65.Location = new System.Drawing.Point(4, 254);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(74, 15);
		this.label65.TabIndex = 471;
		this.label65.Text = "Account (Cr)";
		this.CmbDeliveryTermPurchase.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTermPurchase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTermPurchase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTermPurchase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTermPurchase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermPurchase).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTermPurchase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermPurchase).Location = new System.Drawing.Point(105, 55);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermPurchase).Name = "CmbDeliveryTermPurchase";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermPurchase).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermPurchase).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermPurchase).TextChanged += new System.EventHandler(CmbDeliveryTerms_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(6, 155);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(76, 15);
		this.label6.TabIndex = 471;
		this.label6.Text = "Account (Dr)";
		this.CmbPaymentTermPurchase.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermPurchase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermPurchase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermPurchase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermPurchase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPurchase).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermPurchase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPurchase).Location = new System.Drawing.Point(222, 55);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPurchase).Name = "CmbPaymentTermPurchase";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPurchase).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermPurchase).TabIndex = 3;
		this.CmbPaymentTermPurchase.ValueChanged += new System.EventHandler(CmbPaymentTermPurchase_ValueChanged);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.Blue;
		this.label64.Location = new System.Drawing.Point(7, 230);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(102, 14);
		this.label64.TabIndex = 471;
		this.label64.Text = "____WHT___";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.ForeColor = System.Drawing.Color.Blue;
		this.label63.Location = new System.Drawing.Point(4, 132);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(117, 14);
		this.label63.TabIndex = 471;
		this.label63.Text = "____Freight___";
		this.txtDueDatePurchase.CustomFormat = "dd-MMM-yyyy";
		this.txtDueDatePurchase.Enabled = false;
		this.txtDueDatePurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDatePurchase.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDueDatePurchase.Location = new System.Drawing.Point(222, 83);
		this.txtDueDatePurchase.Name = "txtDueDatePurchase";
		this.txtDueDatePurchase.Size = new System.Drawing.Size(165, 23);
		this.txtDueDatePurchase.TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(6, 87);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(98, 15);
		this.label2.TabIndex = 466;
		this.label2.Text = "Due Days && Date";
		this.txtDueDaysPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDaysPurchase.Location = new System.Drawing.Point(105, 83);
		this.txtDueDaysPurchase.Name = "txtDueDaysPurchase";
		this.txtDueDaysPurchase.Size = new System.Drawing.Size(115, 23);
		this.txtDueDaysPurchase.TabIndex = 4;
		this.txtDueDaysPurchase.Tag = "int";
		this.txtDueDaysPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDueDaysPurchase.TextChanged += new System.EventHandler(txtDueDaysPurchase_TextChanged);
		this.PanelFormHeading.BackColor = System.Drawing.Color.Teal;
		this.PanelFormHeading.Controls.Add(this.txtProfitLoss);
		this.PanelFormHeading.Controls.Add(this.label88);
		this.PanelFormHeading.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.PanelFormHeading.Controls.Add(this.label89);
		this.PanelFormHeading.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompanyName);
		this.PanelFormHeading.Controls.Add(this.label18);
		this.PanelFormHeading.Controls.Add(this.label52);
		this.PanelFormHeading.Controls.Add(this.ChkPreview118);
		this.PanelFormHeading.Controls.Add(this.ChkPreview105603);
		this.PanelFormHeading.Controls.Add(this.ChkPreview105602);
		this.PanelFormHeading.Controls.Add(this.ChkPreview1056A);
		this.PanelFormHeading.Controls.Add(this.ChkPreview105601);
		this.PanelFormHeading.Controls.Add(this.ChkPreview1056);
		this.PanelFormHeading.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelFormHeading.ForeColor = System.Drawing.Color.White;
		this.PanelFormHeading.Location = new System.Drawing.Point(3, 28);
		this.PanelFormHeading.Name = "PanelFormHeading";
		this.PanelFormHeading.Size = new System.Drawing.Size(1270, 30);
		this.PanelFormHeading.TabIndex = 2;
		this.txtProfitLoss.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtProfitLoss.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtProfitLoss.Location = new System.Drawing.Point(818, 3);
		this.txtProfitLoss.Name = "txtProfitLoss";
		this.txtProfitLoss.ReadOnly = true;
		this.txtProfitLoss.Size = new System.Drawing.Size(115, 25);
		this.txtProfitLoss.TabIndex = 2;
		this.txtProfitLoss.TabStop = false;
		this.txtProfitLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label88.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label88.AutoSize = true;
		this.label88.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label88.ForeColor = System.Drawing.Color.White;
		this.label88.Location = new System.Drawing.Point(521, 8);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(46, 15);
		this.label88.TabIndex = 534;
		this.label88.Text = "Branch";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(570, 2);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 1;
		this.label89.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.ForeColor = System.Drawing.Color.White;
		this.label89.Location = new System.Drawing.Point(298, 8);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(58, 15);
		this.label89.TabIndex = 532;
		this.label89.Text = "Company";
		this.CmbCompanyName.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbCompanyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompanyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompanyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCompanyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Location = new System.Drawing.Point(360, 2);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Name = "CmbCompanyName";
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).TabIndex = 0;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 5);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(244, 19);
		this.label18.TabIndex = 25;
		this.label18.Text = "Commission Agent Trade Bill";
		this.label52.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 10.25f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(734, 7);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(85, 17);
		this.label52.TabIndex = 497;
		this.label52.Text = "Profit/Loss";
		this.ChkPreview118.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview118.AutoSize = true;
		this.ChkPreview118.Font = new System.Drawing.Font("Verdana", 7.75f, System.Drawing.FontStyle.Bold);
		this.ChkPreview118.Location = new System.Drawing.Point(1199, 15);
		this.ChkPreview118.Name = "ChkPreview118";
		this.ChkPreview118.Size = new System.Drawing.Size(109, 17);
		this.ChkPreview118.TabIndex = 6;
		this.ChkPreview118.Text = "118-Voucher";
		this.ChkPreview118.UseVisualStyleBackColor = true;
		this.ChkPreview105603.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview105603.AutoSize = true;
		this.ChkPreview105603.Font = new System.Drawing.Font("Verdana", 7.75f, System.Drawing.FontStyle.Bold);
		this.ChkPreview105603.Location = new System.Drawing.Point(1199, 2);
		this.ChkPreview105603.Name = "ChkPreview105603";
		this.ChkPreview105603.Size = new System.Drawing.Size(82, 17);
		this.ChkPreview105603.TabIndex = 6;
		this.ChkPreview105603.Text = "1056_03";
		this.ChkPreview105603.UseVisualStyleBackColor = true;
		this.ChkPreview105602.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview105602.AutoSize = true;
		this.ChkPreview105602.Font = new System.Drawing.Font("Verdana", 7.75f, System.Drawing.FontStyle.Bold);
		this.ChkPreview105602.Location = new System.Drawing.Point(1061, 14);
		this.ChkPreview105602.Name = "ChkPreview105602";
		this.ChkPreview105602.Size = new System.Drawing.Size(140, 17);
		this.ChkPreview105602.TabIndex = 6;
		this.ChkPreview105602.Text = "1056_02-Preview";
		this.ChkPreview105602.UseVisualStyleBackColor = true;
		this.ChkPreview1056A.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview1056A.AutoSize = true;
		this.ChkPreview1056A.Font = new System.Drawing.Font("Verdana", 7.75f, System.Drawing.FontStyle.Bold);
		this.ChkPreview1056A.Location = new System.Drawing.Point(939, 15);
		this.ChkPreview1056A.Name = "ChkPreview1056A";
		this.ChkPreview1056A.Size = new System.Drawing.Size(125, 17);
		this.ChkPreview1056A.TabIndex = 4;
		this.ChkPreview1056A.Text = "1056A-Preview";
		this.ChkPreview1056A.UseVisualStyleBackColor = true;
		this.ChkPreview105601.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview105601.AutoSize = true;
		this.ChkPreview105601.Font = new System.Drawing.Font("Verdana", 7.75f, System.Drawing.FontStyle.Bold);
		this.ChkPreview105601.Location = new System.Drawing.Point(1061, 1);
		this.ChkPreview105601.Name = "ChkPreview105601";
		this.ChkPreview105601.Size = new System.Drawing.Size(140, 17);
		this.ChkPreview105601.TabIndex = 5;
		this.ChkPreview105601.Text = "1056_01-Preview";
		this.ChkPreview105601.UseVisualStyleBackColor = true;
		this.ChkPreview1056.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview1056.AutoSize = true;
		this.ChkPreview1056.Font = new System.Drawing.Font("Verdana", 7.75f, System.Drawing.FontStyle.Bold);
		this.ChkPreview1056.Location = new System.Drawing.Point(939, 2);
		this.ChkPreview1056.Name = "ChkPreview1056";
		this.ChkPreview1056.Size = new System.Drawing.Size(116, 17);
		this.ChkPreview1056.TabIndex = 3;
		this.ChkPreview1056.Text = "1056-Preview";
		this.ChkPreview1056.UseVisualStyleBackColor = true;
		this.ToolstripPanel.Controls.Add(this.toolstrip);
		this.ToolstripPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.ToolstripPanel.Location = new System.Drawing.Point(3, 3);
		this.ToolstripPanel.Name = "ToolstripPanel";
		this.ToolstripPanel.Size = new System.Drawing.Size(1270, 25);
		this.ToolstripPanel.TabIndex = 0;
		this.toolstrip.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 8.5f);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.BtnDelete, this.btnAttachment, this.btnLoadGdn, this.btnPrint, this.BtnPrintII, this.btnPurchasePrint,
			this.btnSalePrint, this.BtnSalePrintII, this.btnPrint103, this.BtnShortCutkeys
		});
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1270, 25);
		this.toolstrip.TabIndex = 6;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 22);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(79, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(62, 22);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(77, 22);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnDelete.Font = new System.Drawing.Font("Verdana", 9f);
		this.BtnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(72, 22);
		this.BtnDelete.Text = "Delete";
		this.BtnDelete.Visible = false;
		this.BtnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(103, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(90, 22);
		this.btnPrint.Text = "1056-Slip";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.BtnPrintII.Font = new System.Drawing.Font("Verdana", 9f);
		this.BtnPrintII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintII.Name = "BtnPrintII";
		this.BtnPrintII.Size = new System.Drawing.Size(149, 22);
		this.BtnPrintII.Text = "1056A-Income-Slip";
		this.BtnPrintII.Click += new System.EventHandler(BtnPrintII_Click);
		this.btnPurchasePrint.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnPurchasePrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPurchasePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchasePrint.Name = "btnPurchasePrint";
		this.btnPurchasePrint.Size = new System.Drawing.Size(176, 22);
		this.btnPurchasePrint.Text = "1056_01-Purchase Slip";
		this.btnPurchasePrint.Click += new System.EventHandler(btnPurchasePrint_Click);
		this.btnSalePrint.Font = new System.Drawing.Font("Verdana", 9f);
		this.btnSalePrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSalePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSalePrint.Name = "btnSalePrint";
		this.btnSalePrint.Size = new System.Drawing.Size(145, 22);
		this.btnSalePrint.Text = "1056_02-Sale Slip";
		this.btnSalePrint.Click += new System.EventHandler(btnSalePrint_Click);
		this.BtnSalePrintII.Font = new System.Drawing.Font("Verdana", 9f);
		this.BtnSalePrintII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnSalePrintII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSalePrintII.Name = "BtnSalePrintII";
		this.BtnSalePrintII.Size = new System.Drawing.Size(145, 22);
		this.BtnSalePrintII.Text = "1056_03-Sale Slip";
		this.BtnSalePrintII.Click += new System.EventHandler(BtnSalePrintII_Click);
		this.btnPrint103.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint103.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint103.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint103.Name = "btnPrint103";
		this.btnPrint103.Size = new System.Drawing.Size(115, 22);
		this.btnPrint103.Text = "118-Voucher";
		this.btnPrint103.Click += new System.EventHandler(btnPrint103_Click);
		this.btnLoadGdn.Font = new System.Drawing.Font("Verdana", 9.25f);
		this.btnLoadGdn.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoadGdn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGdn.Name = "btnLoadGdn";
		this.btnLoadGdn.Size = new System.Drawing.Size(92, 22);
		this.btnLoadGdn.Text = "Load Gdn";
		this.btnLoadGdn.Click += new System.EventHandler(btnLoadGdn_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = (System.Drawing.Image)resources.GetObject("BtnShortCutkeys.Image");
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.tabPage2.Controls.Add(this.tabControl3);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel20);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1231, 832);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(3, 141);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1225, 376);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 1;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.GrdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistory_KeyDown);
		this.tabControl3.Controls.Add(this.tabPage3);
		this.tabControl3.Controls.Add(this.tabPage5);
		this.tabControl3.Controls.Add(this.tabPage6);
		this.tabControl3.Controls.Add(this.tabPage7);
		this.tabControl3.Controls.Add(this.tabPage8);
		this.tabControl3.Controls.Add(this.tabPage9);
		this.tabControl3.Controls.Add(this.tabPage4);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl3.Location = new System.Drawing.Point(3, 517);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1225, 312);
		this.tabControl3.TabIndex = 541;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.tabPage3.Controls.Add(this.panel11);
		this.tabPage3.Location = new System.Drawing.Point(4, 26);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1217, 282);
		this.tabPage3.TabIndex = 5;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1217, 259);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 0;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label42);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.ForeColor = System.Drawing.Color.White;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1217, 23);
		this.panel11.TabIndex = 65;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1177, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GrdHistoryDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 26;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label42.ForeColor = System.Drawing.Color.White;
		this.label42.Location = new System.Drawing.Point(4, 5);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(191, 14);
		this.label42.TabIndex = 25;
		this.label42.Text = "Detail Of Above Selected Row";
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseExpenseDetail);
		this.tabPage5.Controls.Add(this.label93);
		this.tabPage5.Location = new System.Drawing.Point(4, 26);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(1217, 282);
		this.tabPage5.TabIndex = 0;
		this.tabPage5.Text = "Purchase Expense";
		this.tabPage5.UseVisualStyleBackColor = true;
		((ScrollableBase)this.GrdHistoryPurchaseExpenseDetail).BackColor = System.Drawing.Color.White;
		this.GrdHistoryPurchaseExpenseDetail.ColumnAutoResize = true;
		GrdHistoryPurchaseExpenseDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryPurchaseExpenseDetail.DesignTimeLayout = GrdHistoryPurchaseExpenseDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseExpenseDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseExpenseDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryPurchaseExpenseDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseExpenseDetail).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseExpenseDetail).Name = "GrdHistoryPurchaseExpenseDetail";
		((ScrollableBase)this.GrdHistoryPurchaseExpenseDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseExpenseDetail).Size = new System.Drawing.Size(1211, 254);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseExpenseDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseExpenseDetail).TabStop = false;
		this.GrdHistoryPurchaseExpenseDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryPurchaseExpenseDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryPurchaseExpenseDetail).VisualStyle = (VisualStyle)4;
		this.label93.BackColor = System.Drawing.Color.Teal;
		this.label93.Dock = System.Windows.Forms.DockStyle.Top;
		this.label93.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label93.ForeColor = System.Drawing.Color.White;
		this.label93.Location = new System.Drawing.Point(3, 3);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(1211, 22);
		this.label93.TabIndex = 3;
		this.label93.Text = "Credit To Supplier And Debit to Trading Account";
		this.label93.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage6.Controls.Add(this.panel6);
		this.tabPage6.Controls.Add(this.label94);
		this.tabPage6.Location = new System.Drawing.Point(4, 26);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(1217, 282);
		this.tabPage6.TabIndex = 1;
		this.tabPage6.Text = "Purchase Freight Expense";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseFreightDetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 25);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1211, 254);
		this.panel6.TabIndex = 3;
		this.GrdHistoryPurchaseFreightDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.GrdHistoryPurchaseFreightDetail).BackColor = System.Drawing.Color.White;
		GrdHistoryPurchaseFreightDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryPurchaseFreightDetail.DesignTimeLayout = GrdHistoryPurchaseFreightDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseFreightDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseFreightDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryPurchaseFreightDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseFreightDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseFreightDetail).Name = "GrdHistoryPurchaseFreightDetail";
		((ScrollableBase)this.GrdHistoryPurchaseFreightDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseFreightDetail).Size = new System.Drawing.Size(1211, 254);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseFreightDetail).TabIndex = 1;
		this.GrdHistoryPurchaseFreightDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryPurchaseFreightDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryPurchaseFreightDetail).VisualStyle = (VisualStyle)4;
		this.label94.BackColor = System.Drawing.Color.Teal;
		this.label94.Dock = System.Windows.Forms.DockStyle.Top;
		this.label94.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.ForeColor = System.Drawing.Color.White;
		this.label94.Location = new System.Drawing.Point(3, 3);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(1211, 22);
		this.label94.TabIndex = 2;
		this.label94.Text = "Selected Account Credit , Debit Account On Base of Delivery term(Load=> Debit To Trading Account  && Ponch=> Debit To Supplier Account)";
		this.label94.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage7.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistorySaleExpenseDetail);
		this.tabPage7.Controls.Add(this.label95);
		this.tabPage7.Location = new System.Drawing.Point(4, 26);
		this.tabPage7.Name = "tabPage7";
		this.tabPage7.Size = new System.Drawing.Size(1217, 282);
		this.tabPage7.TabIndex = 2;
		this.tabPage7.Text = "Sale Expense";
		this.tabPage7.UseVisualStyleBackColor = true;
		((ScrollableBase)this.GrdHistorySaleExpenseDetail).BackColor = System.Drawing.Color.White;
		this.GrdHistorySaleExpenseDetail.ColumnAutoResize = true;
		GrdHistorySaleExpenseDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistorySaleExpenseDetail.DesignTimeLayout = GrdHistorySaleExpenseDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleExpenseDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleExpenseDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistorySaleExpenseDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleExpenseDetail).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleExpenseDetail).Name = "GrdHistorySaleExpenseDetail";
		((ScrollableBase)this.GrdHistorySaleExpenseDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleExpenseDetail).Size = new System.Drawing.Size(1217, 260);
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleExpenseDetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleExpenseDetail).TabStop = false;
		this.GrdHistorySaleExpenseDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistorySaleExpenseDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistorySaleExpenseDetail).VisualStyle = (VisualStyle)4;
		this.label95.BackColor = System.Drawing.Color.Teal;
		this.label95.Dock = System.Windows.Forms.DockStyle.Top;
		this.label95.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.ForeColor = System.Drawing.Color.White;
		this.label95.Location = new System.Drawing.Point(0, 0);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(1217, 22);
		this.label95.TabIndex = 3;
		this.label95.Text = "Debit To Customer And Credit to Trading Account";
		this.label95.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage8.Controls.Add(this.panel7);
		this.tabPage8.Controls.Add(this.label96);
		this.tabPage8.Location = new System.Drawing.Point(4, 26);
		this.tabPage8.Name = "tabPage8";
		this.tabPage8.Size = new System.Drawing.Size(1217, 282);
		this.tabPage8.TabIndex = 3;
		this.tabPage8.Text = "Packing Material Sale Expense";
		this.tabPage8.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistorySalePmDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 22);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1217, 260);
		this.panel7.TabIndex = 4;
		((ScrollableBase)this.GrdHistorySalePmDetail).BackColor = System.Drawing.Color.White;
		GrdHistorySalePmDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistorySalePmDetail.DesignTimeLayout = GrdHistorySalePmDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistorySalePmDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistorySalePmDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistorySalePmDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistorySalePmDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistorySalePmDetail).Name = "GrdHistorySalePmDetail";
		((ScrollableBase)this.GrdHistorySalePmDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistorySalePmDetail).Size = new System.Drawing.Size(1217, 260);
		((System.Windows.Forms.Control)(object)this.GrdHistorySalePmDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.GrdHistorySalePmDetail).TabStop = false;
		this.GrdHistorySalePmDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistorySalePmDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistorySalePmDetail).VisualStyle = (VisualStyle)4;
		this.label96.BackColor = System.Drawing.Color.Teal;
		this.label96.Dock = System.Windows.Forms.DockStyle.Top;
		this.label96.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label96.ForeColor = System.Drawing.Color.White;
		this.label96.Location = new System.Drawing.Point(0, 0);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(1217, 22);
		this.label96.TabIndex = 3;
		this.label96.Text = "Selected Account Credit , Debit Account On Base of Delivery term(Load=> Debit To Customer  && Ponch=> Debit To Trading Account)";
		this.label96.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage9.Controls.Add(this.panel21);
		this.tabPage9.Controls.Add(this.panel24);
		this.tabPage9.Location = new System.Drawing.Point(4, 26);
		this.tabPage9.Name = "tabPage9";
		this.tabPage9.Size = new System.Drawing.Size(1217, 282);
		this.tabPage9.TabIndex = 4;
		this.tabPage9.Text = "Commission & Brokery";
		this.tabPage9.UseVisualStyleBackColor = true;
		this.panel21.Controls.Add(this.panel22);
		this.panel21.Controls.Add(this.panel23);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 149);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1217, 133);
		this.panel21.TabIndex = 545;
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistorySaleBrokeryDetail);
		this.panel22.Controls.Add(this.label102);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel22.Location = new System.Drawing.Point(606, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(606, 133);
		this.panel22.TabIndex = 545;
		((ScrollableBase)this.GrdHistorySaleBrokeryDetail).BackColor = System.Drawing.Color.White;
		GrdHistorySaleBrokeryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistorySaleBrokeryDetail.DesignTimeLayout = GrdHistorySaleBrokeryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleBrokeryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleBrokeryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistorySaleBrokeryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleBrokeryDetail).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleBrokeryDetail).Name = "GrdHistorySaleBrokeryDetail";
		((ScrollableBase)this.GrdHistorySaleBrokeryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleBrokeryDetail).Size = new System.Drawing.Size(606, 111);
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleBrokeryDetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleBrokeryDetail).TabStop = false;
		this.GrdHistorySaleBrokeryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistorySaleBrokeryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistorySaleBrokeryDetail).VisualStyle = (VisualStyle)4;
		this.label102.BackColor = System.Drawing.Color.Teal;
		this.label102.Dock = System.Windows.Forms.DockStyle.Top;
		this.label102.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.ForeColor = System.Drawing.Color.White;
		this.label102.Location = new System.Drawing.Point(0, 0);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(606, 22);
		this.label102.TabIndex = 5;
		this.label102.Text = "Sale Brokery Detail";
		this.label102.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseBrokeryDetail);
		this.panel23.Controls.Add(this.label103);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(606, 133);
		this.panel23.TabIndex = 544;
		((ScrollableBase)this.GrdHistoryPurchaseBrokeryDetail).BackColor = System.Drawing.Color.White;
		GrdHistoryPurchaseBrokeryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryPurchaseBrokeryDetail.DesignTimeLayout = GrdHistoryPurchaseBrokeryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseBrokeryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseBrokeryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryPurchaseBrokeryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseBrokeryDetail).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseBrokeryDetail).Name = "GrdHistoryPurchaseBrokeryDetail";
		((ScrollableBase)this.GrdHistoryPurchaseBrokeryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseBrokeryDetail).Size = new System.Drawing.Size(606, 111);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseBrokeryDetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseBrokeryDetail).TabStop = false;
		this.GrdHistoryPurchaseBrokeryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryPurchaseBrokeryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryPurchaseBrokeryDetail).VisualStyle = (VisualStyle)4;
		this.label103.BackColor = System.Drawing.Color.Teal;
		this.label103.Dock = System.Windows.Forms.DockStyle.Top;
		this.label103.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label103.ForeColor = System.Drawing.Color.White;
		this.label103.Location = new System.Drawing.Point(0, 0);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(606, 22);
		this.label103.TabIndex = 5;
		this.label103.Text = "Purchase Brokery Detail";
		this.label103.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel24.Controls.Add(this.panel25);
		this.panel24.Controls.Add(this.panel26);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1217, 149);
		this.panel24.TabIndex = 544;
		this.panel25.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistorySaleCommissionDetail);
		this.panel25.Controls.Add(this.label104);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel25.Location = new System.Drawing.Point(606, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(606, 149);
		this.panel25.TabIndex = 545;
		((ScrollableBase)this.GrdHistorySaleCommissionDetail).BackColor = System.Drawing.Color.White;
		GrdHistorySaleCommissionDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistorySaleCommissionDetail.DesignTimeLayout = GrdHistorySaleCommissionDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleCommissionDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleCommissionDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistorySaleCommissionDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleCommissionDetail).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleCommissionDetail).Name = "GrdHistorySaleCommissionDetail";
		((ScrollableBase)this.GrdHistorySaleCommissionDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleCommissionDetail).Size = new System.Drawing.Size(606, 127);
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleCommissionDetail).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.GrdHistorySaleCommissionDetail).TabStop = false;
		this.GrdHistorySaleCommissionDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistorySaleCommissionDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistorySaleCommissionDetail).VisualStyle = (VisualStyle)4;
		this.label104.BackColor = System.Drawing.Color.Teal;
		this.label104.Dock = System.Windows.Forms.DockStyle.Top;
		this.label104.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label104.ForeColor = System.Drawing.Color.White;
		this.label104.Location = new System.Drawing.Point(0, 0);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(606, 22);
		this.label104.TabIndex = 5;
		this.label104.Text = "Sale Commission Detail";
		this.label104.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseCommissionDetail);
		this.panel26.Controls.Add(this.label105);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel26.Location = new System.Drawing.Point(0, 0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(606, 149);
		this.panel26.TabIndex = 544;
		((ScrollableBase)this.GrdHistoryPurchaseCommissionDetail).BackColor = System.Drawing.Color.White;
		GrdHistoryPurchaseCommissionDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryPurchaseCommissionDetail.DesignTimeLayout = GrdHistoryPurchaseCommissionDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseCommissionDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseCommissionDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryPurchaseCommissionDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseCommissionDetail).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseCommissionDetail).Name = "GrdHistoryPurchaseCommissionDetail";
		((ScrollableBase)this.GrdHistoryPurchaseCommissionDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseCommissionDetail).Size = new System.Drawing.Size(606, 127);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseCommissionDetail).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPurchaseCommissionDetail).TabStop = false;
		this.GrdHistoryPurchaseCommissionDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryPurchaseCommissionDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryPurchaseCommissionDetail).VisualStyle = (VisualStyle)4;
		this.label105.BackColor = System.Drawing.Color.Teal;
		this.label105.Dock = System.Windows.Forms.DockStyle.Top;
		this.label105.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label105.ForeColor = System.Drawing.Color.White;
		this.label105.Location = new System.Drawing.Point(0, 0);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(606, 22);
		this.label105.TabIndex = 5;
		this.label105.Text = "Purchase Commission Detail";
		this.label105.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage4.Controls.Add(this.panel3);
		this.tabPage4.Controls.Add(this.panel5);
		this.tabPage4.Location = new System.Drawing.Point(4, 26);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(1217, 282);
		this.tabPage4.TabIndex = 6;
		this.tabPage4.Text = "Payment Detail";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailSale);
		this.panel3.Controls.Add(this.label91);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(606, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(606, 282);
		this.panel3.TabIndex = 543;
		((ScrollableBase)this.GrdHistoryPaymentDetailSale).BackColor = System.Drawing.Color.White;
		GrdHistoryPaymentDetailSale_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryPaymentDetailSale.DesignTimeLayout = GrdHistoryPaymentDetailSale_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailSale).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailSale).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryPaymentDetailSale.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailSale).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailSale).Name = "GrdHistoryPaymentDetailSale";
		((ScrollableBase)this.GrdHistoryPaymentDetailSale).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailSale).Size = new System.Drawing.Size(606, 260);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailSale).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailSale).TabStop = false;
		this.GrdHistoryPaymentDetailSale.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryPaymentDetailSale.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryPaymentDetailSale).VisualStyle = (VisualStyle)4;
		this.label91.BackColor = System.Drawing.Color.Teal;
		this.label91.Dock = System.Windows.Forms.DockStyle.Top;
		this.label91.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.ForeColor = System.Drawing.Color.White;
		this.label91.Location = new System.Drawing.Point(0, 0);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(606, 22);
		this.label91.TabIndex = 5;
		this.label91.Text = "Buyer Payment Detail";
		this.label91.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailPurchase);
		this.panel5.Controls.Add(this.label92);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(606, 282);
		this.panel5.TabIndex = 542;
		((ScrollableBase)this.GrdHistoryPaymentDetailPurchase).BackColor = System.Drawing.Color.White;
		GrdHistoryPaymentDetailPurchase_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryPaymentDetailPurchase.DesignTimeLayout = GrdHistoryPaymentDetailPurchase_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailPurchase).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailPurchase).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryPaymentDetailPurchase.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailPurchase).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailPurchase).Name = "GrdHistoryPaymentDetailPurchase";
		((ScrollableBase)this.GrdHistoryPaymentDetailPurchase).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailPurchase).Size = new System.Drawing.Size(606, 260);
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailPurchase).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.GrdHistoryPaymentDetailPurchase).TabStop = false;
		this.GrdHistoryPaymentDetailPurchase.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryPaymentDetailPurchase.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryPaymentDetailPurchase).VisualStyle = (VisualStyle)4;
		this.label92.BackColor = System.Drawing.Color.Teal;
		this.label92.Dock = System.Windows.Forms.DockStyle.Top;
		this.label92.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(0, 0);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(606, 22);
		this.label92.TabIndex = 5;
		this.label92.Text = "Supplier Payment Detail";
		this.label92.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label66);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.ForeColor = System.Drawing.Color.White;
		this.panel9.Location = new System.Drawing.Point(3, 114);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1225, 27);
		this.panel9.TabIndex = 64;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label66.ForeColor = System.Drawing.Color.White;
		this.label66.Location = new System.Drawing.Point(3, 4);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(132, 18);
		this.label66.TabIndex = 25;
		this.label66.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1185, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 24;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add(this.rdmodifydate);
		this.panel20.Controls.Add(this.rdentrydate);
		this.panel20.Controls.Add(this.drdocdate);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcusthistory);
		this.panel20.Controls.Add(this.label51);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplierhistory);
		this.panel20.Controls.Add(this.label50);
		this.panel20.Controls.Add(this.txtOrderNoTo);
		this.panel20.Controls.Add(this.txtOrderNoFrom);
		this.panel20.Controls.Add(this.label46);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel20.Controls.Add(this.label47);
		this.panel20.Controls.Add(this.btnshowHistory);
		this.panel20.Controls.Add(this.ToDate);
		this.panel20.Controls.Add(this.fromdate);
		this.panel20.Controls.Add(this.label48);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtradAcHistory);
		this.panel20.Controls.Add(this.label49);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 54);
		this.panel20.Margin = new System.Windows.Forms.Padding(2);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1225, 60);
		this.panel20.TabIndex = 0;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1017, 38);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 11;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1017, 21);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 10;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1016, 4);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 9;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.cmbcusthistory.AllowNull = (DefaultableBoolean)1;
		this.cmbcusthistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcusthistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcusthistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcusthistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbcusthistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcusthistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).Location = new System.Drawing.Point(717, 30);
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).Name = "cmbcusthistory";
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).TabIndex = 7;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(653, 36);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(58, 15);
		this.label51.TabIndex = 602;
		this.label51.Text = "Customer";
		this.cmbsupplierhistory.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplierhistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplierhistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplierhistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsupplierhistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).Location = new System.Drawing.Point(717, 3);
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).Name = "cmbsupplierhistory";
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).TabIndex = 6;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(653, 9);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(51, 15);
		this.label50.TabIndex = 600;
		this.label50.Text = "Supplier";
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(550, 5);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(100, 23);
		this.txtOrderNoTo.TabIndex = 4;
		this.txtOrderNoTo.Tag = "int";
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(450, 5);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(99, 23);
		this.txtOrderNoFrom.TabIndex = 3;
		this.txtOrderNoFrom.Tag = "int";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(351, 9);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(95, 15);
		this.label46.TabIndex = 597;
		this.label46.Text = "Doc No From,To";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(66, 3);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(282, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(5, 9);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(60, 15);
		this.label47.TabIndex = 594;
		this.label47.Text = "Date Type";
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(941, 30);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(58, 26);
		this.btnshowHistory.TabIndex = 8;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.btnshowHistory.Leave += new System.EventHandler(btnshowHistory_Leave);
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(208, 32);
		this.ToDate.Name = "ToDate";
		this.ToDate.ShowCheckBox = true;
		this.ToDate.Size = new System.Drawing.Size(140, 23);
		this.ToDate.TabIndex = 2;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(66, 32);
		this.fromdate.Name = "fromdate";
		this.fromdate.ShowCheckBox = true;
		this.fromdate.Size = new System.Drawing.Size(140, 23);
		this.fromdate.TabIndex = 1;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(5, 28);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(63, 30);
		this.label48.TabIndex = 146;
		this.label48.Text = "Date From\r\n&& To";
		this.cmbtradAcHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbtradAcHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtradAcHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtradAcHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtradAcHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).Location = new System.Drawing.Point(450, 30);
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).Name = "cmbtradAcHistory";
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).TabIndex = 5;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(351, 36);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(95, 15);
		this.label49.TabIndex = 144;
		this.label49.Text = "Trading Account";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label35);
		this.panel4.Controls.Add(this.toolStrip2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1225, 51);
		this.panel4.TabIndex = 0;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(4, 29);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(307, 19);
		this.label35.TabIndex = 25;
		this.label35.Text = "Commission Agent Trade Bill History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnGrnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1225, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(55, 24);
		this.btnNewHistory.Text = "New";
		this.btnNewHistory.ToolTipText = "Crtl_H";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnGrnFormHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGrnFormHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGrnFormHistory.Name = "btnGrnFormHistory";
		this.btnGrnFormHistory.Size = new System.Drawing.Size(73, 24);
		this.btnGrnFormHistory.Text = "Register";
		this.btnGrnFormHistory.ToolTipText = "Crtl_H";
		this.btnGrnFormHistory.Click += new System.EventHandler(btnGrnFormHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1284, 861);
		base.Controls.Add(this.MainPanel);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmCommissionAgentTradeBillAgainstGdn";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CommissionAgentTradeBill_162_FormClosing);
		base.Load += new System.EventHandler(frmCommissionAgentOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommissionAgentTradeBill_162_KeyDown);
		this.MainPanel.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.tabPurchaseExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseOtherExpenses).EndInit();
		this.tabPurchasePmExpense.ResumeLayout(false);
		this.tabPurchasePmExpense.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEmptyBagTermPurchase).EndInit();
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPurchasePmExpense).EndInit();
		this.tabSaleExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSaleOtherExpense).EndInit();
		this.tabSalePmExpense.ResumeLayout(false);
		this.tabSalePmExpense.PerformLayout();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSalePmExpenseSale).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEmptyBagTerm).EndInit();
		this.tabCommissionAndBrokery.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSaleBrokery).EndInit();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseBrokery).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSaleCommission).EndInit();
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseCommission).EndInit();
		this.tabPaymentDetail.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetailSale).EndInit();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetailPurchase).EndInit();
		this.PanelDetailInfo.ResumeLayout(false);
		this.GbDetailInfo.ResumeLayout(false);
		this.GbDetailInfo.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxNameSale).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomSale).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxNamePurchase).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomPurchase).EndInit();
		this.PanelMainInfo.ResumeLayout(false);
		this.GbSaleSide.ResumeLayout(false);
		this.GbSaleSide.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWhtTaxTypeSale).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWhtAccountSale).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccountSale).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAccountSale).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermSale).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermSale).EndInit();
		this.GbWbComparison.ResumeLayout(false);
		this.GbWbComparison.PerformLayout();
		this.GbMain.ResumeLayout(false);
		this.GbMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTradingAccount).EndInit();
		this.GbPurchaseSide.ResumeLayout(false);
		this.GbPurchaseSide.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccountPurchase).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWhtTaxTypePurchase).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWhtAccountPurchase).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAccountPurchase).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermPurchase).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermPurchase).EndInit();
		this.PanelFormHeading.ResumeLayout(false);
		this.PanelFormHeading.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyName).EndInit();
		this.ToolstripPanel.ResumeLayout(false);
		this.ToolstripPanel.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPurchaseExpenseDetail).EndInit();
		this.tabPage6.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPurchaseFreightDetail).EndInit();
		this.tabPage7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistorySaleExpenseDetail).EndInit();
		this.tabPage8.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistorySalePmDetail).EndInit();
		this.tabPage9.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistorySaleBrokeryDetail).EndInit();
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPurchaseBrokeryDetail).EndInit();
		this.panel24.ResumeLayout(false);
		this.panel25.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistorySaleCommissionDetail).EndInit();
		this.panel26.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPurchaseCommissionDetail).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPaymentDetailSale).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryPaymentDetailPurchase).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcusthistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierhistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtradAcHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
