using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Special_Rights.Configurations;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Purchase;

public class InvfrmPurchaseInvoice : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtEmptyBags = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtGridHistory = new DataTable();

	private DataTable dtGridDetailHistory = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtCrAccountForEmptyBag = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtEmpType = new DataTable();

	private CancellationTokenSource _cts;

	private string ScreenName = "InvfrmPurchaseInvoice";

	private bool FormInitialized;

	private int DocumentTypeId = 56;

	private int ScreenId;

	private int VoucherHeadId;

	public int RecId;

	private bool Approved;

	private int POID;

	private int InvoiceTypeId;

	public static int SupplierId = 0;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool HasMultiCurrencyFeature;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool BranchFeature;

	private int BaseCurrencyId;

	private double BaseCurrencyRate;

	private bool BranchImplemented;

	private bool WagesAmountCalculateOnQty;

	private bool ContractWagesChargetoProduct;

	private bool ExpenseGridsInTabOrinFront;

	private bool WeightAddLessOnPurchaseInvoice;

	private bool RateEditableOnPurchaseInvoice_InGatePurchase;

	private bool FreightDebitToExpenses;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool ValidateGrnAndInvoiceDateWithGpDate;

	private bool HideColumnsFromDetailGridSpecialRight;

	private static bool PCalculateByPercent = false;

	private static bool PCalculateByAmount = true;

	private DateTime GrnDate = DateTime.Now;

	private frmLoadGRN LoadGRN;

	private List<InvPurchaseInvoiceDetail> lstRemoveRecord = new List<InvPurchaseInvoiceDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel2;

	private TabPage tabHistory;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnAttachment;

	private ToolStripButton BtnLoader;

	private Panel panel4;

	private Panel panel5;

	private Label label20;

	private Label label26;

	private Label label11;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	private Label label7;

	private Label label8;

	private Label label6;

	private TextBox txtbillno;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel PartyAddLessAndEmptyBagGridParentPanel;

	internal GridEX grd;

	private Panel TransporterAndOtherItemExpenseParentPanel;

	private Panel OtherItemExpensePanel;

	internal GridEX grdInvExp;

	private Panel TransporterGridPanel;

	internal GridEX grdFreight;

	private TextBox txtBillAmount;

	private Label label3;

	internal UltraCombo cmbstockprty;

	internal UltraCombo cmbsuppliername;

	internal UltraCombo cmbcommagent;

	private Label label4;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Label label9;

	private TextBox txtDeliveryTerm;

	private Label label15;

	private ToolStripButton btnSlipDetail;

	private CheckBox ChkBokVoucher;

	private Panel panel14;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton btnFrmRefresh;

	private ToolStripSeparator toolStripSeparator1;

	private TextBox txtDueDays;

	private Label label22;

	private DateTimePicker DueDate;

	private ToolStripButton btnDelete;

	private ToolStripButton btn104Voucher;

	internal UltraCombo cmbcommtype;

	internal UltraCombo cmbcommuom;

	private Panel EmptyBagGridPanel;

	internal GridEX grdEmptyBags;

	private Panel PartyAddLessGridPanel;

	internal GridEX grdGLedger;

	internal GridEX grdDetail;

	internal UltraCombo CmbBrokeryType;

	internal UltraCombo CmbBrokeryRateUom;

	internal UltraCombo CmbBrokeryAc;

	private Label label14;

	private Label label12;

	private Label label10;

	private Label label2;

	private TextBox txtBrokeryRate;

	private Label label1;

	private TextBox txtBrokeryAmount;

	private ToolStripButton btnSlip220A;

	private CheckBox ChkBoxParty;

	private ToolStripButton btn220bSummary;

	private Label label23;

	private TextBox txtGrnNo;

	private CheckBox ChkBoxItemPrint;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel17;

	private Label label27;

	private Panel panel11;

	private Label label24;

	private Panel panel19;

	private Label label29;

	private Panel panel18;

	private Label label28;

	private Label label30;

	private TextBox txtRefParty;

	private Label label31;

	private Panel panel21;

	private Label label32;

	internal UltraCombo CmbPaymentTerm;

	private Label label33;

	private Panel TabControlPanelForExpenseGrids;

	private TabControl tabControl2;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel7;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private RadioButton rdTemplate2;

	private RadioButton rdTemplate1;

	private CtrlGrdBar ctrlGrdBar4;

	private CtrlGrdBar ctrlGrdBar5;

	private CtrlGrdBar ctrlGrdBar6;

	private CtrlGrdBar ctrlGrdBar7;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton BtnSpecialRights;

	private Label label34;

	private TextBox txtcommremarks;

	private TextBox txtWagesAmountHeader;

	private Label label35;

	private Panel panel3;

	private GroupBox groupBox3;

	private Button btnshow;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel9;

	private Panel panel10;

	private Panel panel12;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private TextBox txtBranchCode;

	private TextBox txtRemarks;

	internal UltraCombo cmbBranchName;

	private Label label13;

	private CheckBox chkSummary;

	private DateTimePicker datSupplierInvoiceDate;

	private Label labelLocationType;

	internal UltraCombo cmbLocationType;

	private CheckBox chkCustomAccounts;

	internal UltraCombo CmbFreightAccount;

	private Label label5;

	private TextBox txtFreightDeduction;

	private Label label16;

	private ToolStripButton btnShortcutKeys;

	private CtrlGrdBar ctrlGrdBar8;

	private TabControl tabControl3;

	private TabPage tabDetail;

	private TabPage tabPaymentSchedule;

	private Panel panel8;

	private Label label18;

	private CtrlGrdBar ctrlGrdBar9;

	internal GridEX grdPaymentDetail;

	public InvfrmPurchaseInvoice(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		grdPaymentDetail.CellUpdated += new ColumnActionEventHandler(grdPaymentDetail_CellUpdated);
		grdPaymentDetail.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentDetail_ColumnButtonClick);
		((Control)(object)grdPaymentDetail).KeyDown += grdPaymentDetail_KeyDown;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1350, 800);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("SupplierCustomerId");
			dtAccountlst.Columns.Add("AccountTitle");
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			dtEmpType.Columns.Add("Id", typeof(int));
			dtEmpType.Columns.Add("Type", typeof(string));
			dtEmpType.Rows.Add(1, "Normal");
			dtEmpType.Rows.Add(2, "Purchase Against Weight");
			dtEmpType.Rows.Add(3, "Free of Cost");
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("InvGrnDetailId", typeof(int));
			dtGrid.Columns.Add("InvGrnId", typeof(int));
			dtGrid.Columns.Add("GrnNo", typeof(int));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("WareHouseName");
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("CropYear");
			dtGrid.Columns.Add("ItemUomId", typeof(int));
			dtGrid.Columns.Add("UOMCodeItem");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("PackingTypeId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrder", typeof(int));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("EbTotal", typeof(double));
			dtGrid.Columns.Add("EbPurAgainstWeight", typeof(double));
			dtGrid.Columns.Add("WtCutTotal", typeof(double));
			dtGrid.Columns.Add("AdLsWeight", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("StockWeight", typeof(double));
			dtGrid.Columns.Add("OrderItemRate", typeof(double));
			dtGrid.Columns.Add("OrderItemRateUOMId", typeof(int));
			dtGrid.Columns.Add("RateUom");
			dtGrid.Columns.Add("EquivalentPoRate", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("RateCut", typeof(double));
			dtGrid.Columns.Add("RateCutAmount", typeof(double));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("LabReportRef");
			dtGrid.Columns.Add("GpNo");
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("GpDate");
			dtGrid.Columns.Add("WagesAmount", typeof(double));
			dtGrid.Columns.Add("BranchId", typeof(int));
			dtGrid.Columns.Add("BranchName", typeof(string));
			dtGrid.Columns.Add("EbPurAgainstWeightAmount", typeof(double));
			dtGrid.Columns.Add("FreightDeduction", typeof(double));
			dtGrdGL.Columns.Add("InvGrnId");
			dtGrdGL.Columns.Add("FreightId");
			dtGrdGL.Columns.Add("FreightAcId");
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage", typeof(double));
			dtGrdGL.Columns.Add("Qty", typeof(double));
			dtGrdGL.Columns.Add("Rate", typeof(double));
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			dtGrdGL.Columns.Add("FreightAmount", typeof(double));
			dtGrdGL.Columns.Add("GlAccountId", typeof(int));
			dtGrdGL.Columns.Add("RateCutQty", typeof(double));
			dtGrdGL.Columns.Add("RateCut", typeof(double));
			dtGrdGL.Columns.Add("RowType", typeof(int));
			dtInvExp.Columns.Add("Id", typeof(int));
			dtInvExp.Columns.Add("GrnId", typeof(int));
			dtInvExp.Columns.Add("GrnNo", typeof(int));
			dtInvExp.Columns.Add("PurchaseOrderId", typeof(int));
			dtInvExp.Columns.Add("PurchaseOrderSupplierExpId", typeof(int));
			dtInvExp.Columns.Add("SupplierDispatchId", typeof(int));
			dtInvExp.Columns.Add("SupplierDispatchExpenseId", typeof(int));
			dtInvExp.Columns.Add("ItemId", typeof(int));
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			dtFreight.Columns.Add("OrderId");
			dtFreight.Columns.Add("InvGrnId");
			dtFreight.Columns.Add("FreightId");
			dtFreight.Columns.Add("FreightAcId");
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("Percentage", typeof(double));
			dtFreight.Columns.Add("Qty", typeof(double));
			dtFreight.Columns.Add("Rate", typeof(double));
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
			dtFreight.Columns.Add("FreightAmount", typeof(double));
			dtFreight.Columns.Add("Remarks");
			dtFreight.Columns.Add("GlAccountId", typeof(int));
			dtEmptyBags.Columns.Add("OrderId");
			dtEmptyBags.Columns.Add("TypeId");
			dtEmptyBags.Columns.Add("Type");
			dtEmptyBags.Columns.Add("InvGrnId");
			dtEmptyBags.Columns.Add("ItemId");
			dtEmptyBags.Columns.Add("ItemName");
			dtEmptyBags.Columns.Add("ItemConditionId");
			dtEmptyBags.Columns.Add("ItemCondition");
			dtEmptyBags.Columns.Add("PurchaseQty", typeof(double));
			dtEmptyBags.Columns.Add("Rate", typeof(double));
			dtEmptyBags.Columns.Add("Amount", typeof(double));
			dtEmptyBags.Columns.Add("Remarks");
			dtEmptyBags.Columns.Add("CreditAccountId");
			dtPaymentTerm.Columns.Add("PurchaseOrderId", typeof(int));
			dtPaymentTerm.Columns.Add("PurchaseOrderNo", typeof(int));
			dtPaymentTerm.Columns.Add("ItemAmount", typeof(double));
			dtPaymentTerm.Columns.Add("Freight", typeof(double));
			dtPaymentTerm.Columns.Add("Expense", typeof(double));
			dtPaymentTerm.Columns.Add("PartyAddLessAmount", typeof(double));
			dtPaymentTerm.Columns.Add("EmptyBagAmount", typeof(double));
			dtPaymentTerm.Columns.Add("FreightDeduction", typeof(double));
			dtPaymentTerm.Columns.Add("Commission", typeof(double));
			dtPaymentTerm.Columns.Add("BrokeryAmount", typeof(double));
			dtPaymentTerm.Columns.Add("NetBillAmount", typeof(double));
			dtPaymentTerm.Columns.Add("PaymentTerm", typeof(int));
			dtPaymentTerm.Columns.Add("DueDays", typeof(int));
			dtPaymentTerm.Columns.Add("DueDate", typeof(DateTime));
			dtPaymentTerm.Columns.Add("%OfTotal", typeof(decimal));
			dtPaymentTerm.Columns.Add("Amount", typeof(decimal));
			dtPaymentTerm.Columns.Add("Remarks", typeof(string));
			FormInitialized = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		_cts?.Cancel();
		_cts?.Dispose();
		_cts = new CancellationTokenSource();
		CancellationToken token = _cts.Token;
		Task.Run(delegate
		{
			token.ThrowIfCancellationRequested();
			ScreenName = base.Name;
			BranchFeature = CommonServices.GetERPFeatureById(11);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			token.ThrowIfCancellationRequested();
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			ScreenId = CommonServices.GetScreenIdByName(ScreenName);
			GetConfigurationsFromGlobal();
			token.ThrowIfCancellationRequested();
			HistoryCombosBranchDbCall();
			DataTable historyComboData = HistoryComboDbCall(ValidateBranch: false);
			token.ThrowIfCancellationRequested();
			DataTable locationType = VoucherHead.GetLocationType();
			string documentNoData = Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCode(DocumentTypeId));
			string branchSrNo = "";
			if (BranchFeature)
			{
				branchSrNo = GenerateBranchSrNo();
			}
			DataTable dueTermServiceBind = CommonServices.GetDueTermServiceBind();
			token.ThrowIfCancellationRequested();
			DataTable specialRightsData = SpecialRightsDbCall();
			token.ThrowIfCancellationRequested();
			OtherItemsdtFillDbCall();
			return new
			{
				HistoryComboData = historyComboData,
				LocationTypeData = locationType,
				documentNoData = documentNoData,
				BranchSrNo = branchSrNo,
				PTdata = dueTermServiceBind,
				SpecialRightsData = specialRightsData
			};
		}, token).ContinueWith(t =>
		{
			if (!token.IsCancellationRequested && !t.IsCanceled)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					int num = tabControl1.Height;
					int num2 = Screen.PrimaryScreen.Bounds.Height;
					int num3 = 10;
					tabControl1.Location = new Point(0, num2 - num - num3);
					var result = t.Result;
					btnSave.Enabled = formright.DoHaveSaveRight;
					btnUpdate.Enabled = formright.DoHaveUpdateRights;
					btnDelete.Enabled = formright.DoHaveCanDelete;
					btnPrint.Enabled = formright.DoHavePrintRights;
					btn104Voucher.Enabled = formright.DoHavePrintRights;
					btnSlipDetail.Enabled = formright.DoHavePrintRights;
					btn220bSummary.Enabled = formright.DoHavePrintRights;
					btnSlip220A.Enabled = formright.DoHavePrintRights;
					ctrlGrdBar8.mGridSaveLayouts.Visible = false;
					if (RecId == 0)
					{
						txtdocno.Text = result.documentNoData;
						txtBranchCode.Text = result.BranchSrNo;
					}
					LocationTypeBind(result.LocationTypeData);
					SpecialRightsImplement(result.SpecialRightsData);
					SupplierDtFillFromGlobal();
					BindSupplierName();
					PaymentTermBind(result.PTdata);
					AccountlstDtFillFromGlobal();
					CommissionTypeFill();
					CommissionUOMFill();
					FreightDeductionAccountFill();
					OtherItemsdtFillDbCall();
					ImplementConfiguration();
					ChangeTemplate(ExpenseGridsInTabOrinFront);
					BindGrids();
					HistoryCombosBranchBind();
					HistoryComboBind(result.HistoryComboData);
					FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					ToDateHistory.Value = DateTime.Now;
					FormInitialized = true;
					DocDate.Focus();
				}
			}
		}, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void FreightDeductionAccountFill()
	{
		try
		{
			DataTable dtfreight = new DataTable();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtfreight.Columns.Add("Id", typeof(int));
				dtfreight.Columns.Add("AccountTitle", typeof(string));
				dtfreight.Columns.Add("PartyCode", typeof(string));
				dtfreight.Columns.Add("SupplierCustomerId", typeof(int));
				List<getGlobalVendorsAndCustomersForTransporter> lst = clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters;
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtfreight.Rows.Add(lst[i].GlAccountId, lst[i].CompanyName, lst[i].PartyCode, lst[i].Id);
					}
				}
			}
			else
			{
				dtfreight = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, new int[3] { 2, 15, 22 });
			}
			if (SubsidiaryAccountAllownOnVouchers)
			{
				if (dtfreight.Rows.Count > 0)
				{
					InfragisticsHelper.BindAndRetainSelection(CmbFreightAccount, dtfreight, "SupplierCustomerId", "AccountTitle", "Transporter Name", AllColumns: true, Conversion.ToInt(CmbFreightAccount.Value), insertDefaultRow: true, null, null, ActivateRow: false);
					if (dtfreight.Rows.Count > 0)
					{
						((UltraGridBase)CmbFreightAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					}
				}
			}
			else
			{
				InfragisticsHelper.BindAndRetainSelection(CmbFreightAccount, dtfreight, "Id", "AccountTitle", "Freight Deduction Account", AllColumns: true, Conversion.ToInt(CmbFreightAccount.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryCombosBranchDbCall()
	{
		try
		{
			dtBranch.Clear();
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
				return;
			}
			DataTable dt = InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, DocumentTypeId);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosBranchBind()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)cmbBranchName).Text);
			((UltraGridBase)cmbBranchName).DataSource = null;
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

	private DataTable HistoryComboDbCall(bool ValidateBranch = true)
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
			obj.DocumentTypeIds = DocumentTypeId.ToString();
			obj.Activity = "Supplier";
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
			return InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(obj);
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
			InfragisticsHelper.BindAndRetainSelection(cmbSupplierNameHistory, dt, "Id", "ReferenceName", "Supplier Name", AllColumns: false, Conversion.ToInt(cmbSupplierNameHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			ContractWagesChargetoProduct = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ContractWagesChargetoProduct"));
			WagesAmountCalculateOnQty = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesAmountCalculateOnQty"));
			FreightDebitToExpenses = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DebitAmountChargetoExpenseAcFreightGridPurchase"));
			RateEditableOnPurchaseInvoice_InGatePurchase = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("RateEditableOnPurchaseInvoice_InGatePurchase"));
			WeightAddLessOnPurchaseInvoice = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightAddLessOnPurchaseInvoice"));
			BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PurchaseInvoiceBranchWise"));
			BaseCurrencyId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
			BaseCurrencyRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BaseCurrencyRate"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ValidateGrnAndInvoiceDateWithGpDate = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ValidateGrnAndInvoiceDateWithGpDate"));
			DocDate.Enabled = !ValidateGrnAndInvoiceDateWithGpDate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImplementConfiguration()
	{
		try
		{
			txtBranchCode.Visible = BranchFeature && BranchImplemented;
			txtdocno.Width = ((BranchFeature && BranchImplemented) ? 80 : 137);
			((Control)(object)cmbBranchName).Enabled = !BranchImplemented;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string GenerateBranchSrNo()
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
			return Conversion.ToString((object)InvPurchaseInvoice.GenerateInvPurchaseInvoiceCodeBranch(new InvPurchaseInvoice
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

	private void LocationTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbLocationType, dt, "Id", "Location", "Location Type", AllColumns: true, Conversion.ToInt(cmbLocationType.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbLocationType).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			List<getGlobalSupplierCustomer> obj = (from r in clsGlobalVariables.globalSupplierCustomer.AsEnumerable()
				where r.CustomerGroupId != 7
				select r).ToList();
			dtSupplier.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				List<getGlobalSupplierCustomer> lst = (from r in obj.AsEnumerable()
					where r.PartyTypeId == 1
					select r).ToList();
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
			InfragisticsHelper.BindAndRetainSelection(cmbsuppliername, dtSupplier, "Id", "CompanyName", "Customer Name", AllColumns: true, Conversion.ToInt(cmbsuppliername.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbcommagent, dtSupplier, "Id", "CompanyName", "Commission Agent", AllColumns: true, Conversion.ToInt(cmbcommagent.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBrokeryAc, dtSupplier, "Id", "CompanyName", "Broker Ac", AllColumns: true, Conversion.ToInt(CmbBrokeryAc.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			DataTable dataTable = dtSupplier;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)cmbcommagent).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbcommagent).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbBrokeryAc).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbBrokeryAc).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PaymentTermBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTerm, dt, "Id", "TermsDescription", "Payment Term", AllColumns: false, Conversion.ToInt(CmbPaymentTerm.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbPaymentTerm).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AccountlstDtFillFromGlobal()
	{
		try
		{
			dtAccountlst.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				List<getGlobalVendorsAndCustomersForTransporter> lst = clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters;
				if (lst != null && lst.Count > 0)
				{
					foreach (getGlobalVendorsAndCustomersForTransporter item in lst)
					{
						dtAccountlst.Rows.Add(item.GlAccountId, item.Id, item.CompanyName);
					}
				}
			}
			else
			{
				int[] excludedIds = ((!FreightDebitToExpenses) ? new int[9] { 2, 11, 12, 13, 14, 15, 20, 21, 22 } : new int[3] { 2, 15, 22 });
				IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup acc) => !excludedIds.Contains(acc.AccountTypeId));
				HashSet<int> existingIds = new HashSet<int>();
				foreach (getGlobalAllAccountsWithCustomGroup acc2 in enumerable)
				{
					int id = Conversion.ToInt((object)acc2.ChartOfAccountId);
					if (existingIds.Add(id))
					{
						dtAccountlst.Rows.Add(acc2.ChartOfAccountId, 0, acc2.AccountTitle);
					}
				}
			}
			dtCrAccountForEmptyBag = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[2] { 8, 10 });
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

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = CommonServices.StaticColumnsService("GetCommissionUom");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbcommuom, "Id", "type", "UOM", ZeroIndex: false);
				DDL.BindDDL(dt, CmbBrokeryRateUom, "Id", "type", "UOM", ZeroIndex: false);
			}
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			DDL.BindDDL(dt, cmbcommtype, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			DDL.BindDDL(dt, CmbBrokeryType, "Id", "CommissionType", "BrokeryType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable SpecialRightsDbCall()
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
			return SreenRightsUserWise.SpecialRightsByScreenIdAndUserId(new SreenRightsUserWise
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				ScreenId = ScreenId,
				UserId = UserAccount.ID
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SpecialRightsImplement(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 1)
				{
					ChkBoxItemPrint.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
				else if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 2)
				{
					ChkBokVoucher.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
				else if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 7)
				{
					ChkBoxParty.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
				else if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 5)
				{
					rdTemplate1.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
				else if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 6)
				{
					rdTemplate2.Checked = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
				else if (Conversion.ToInt(dt.Rows[i]["RightId"]) == 10)
				{
					HideColumnsFromDetailGridSpecialRight = Conversion.ToBool(dt.Rows[i]["IsActive"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSpecialRights_Click(object sender, EventArgs e)
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

	private void rdTemplate1_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rdTemplate1.Checked)
			{
				ExpenseGridsInTabOrinFront = false;
			}
			else
			{
				ExpenseGridsInTabOrinFront = true;
			}
			ChangeTemplate(ExpenseGridsInTabOrinFront);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChangeTemplate(bool Type)
	{
		try
		{
			if (Type)
			{
				TransporterAndOtherItemExpenseParentPanel.Visible = false;
				PartyAddLessAndEmptyBagGridParentPanel.Visible = false;
				TabControlPanelForExpenseGrids.Visible = true;
				TransporterGridPanel.Dock = DockStyle.Fill;
				tabPage1.Controls.Add(TransporterGridPanel);
				OtherItemExpensePanel.Dock = DockStyle.Fill;
				tabPage2.Controls.Add(OtherItemExpensePanel);
				PartyAddLessGridPanel.Dock = DockStyle.Fill;
				tabPage3.Controls.Add(PartyAddLessGridPanel);
				EmptyBagGridPanel.Dock = DockStyle.Fill;
				tabPage4.Controls.Add(EmptyBagGridPanel);
			}
			else
			{
				TabControlPanelForExpenseGrids.Visible = false;
				TransporterAndOtherItemExpenseParentPanel.Visible = true;
				PartyAddLessAndEmptyBagGridParentPanel.Visible = true;
				TransporterAndOtherItemExpenseParentPanel.Controls.Add(OtherItemExpensePanel);
				OtherItemExpensePanel.Dock = DockStyle.Left;
				TransporterAndOtherItemExpenseParentPanel.Controls.Add(TransporterGridPanel);
				TransporterGridPanel.Dock = DockStyle.Left;
				PartyAddLessAndEmptyBagGridParentPanel.Controls.Add(EmptyBagGridPanel);
				EmptyBagGridPanel.Dock = DockStyle.Left;
				PartyAddLessAndEmptyBagGridParentPanel.Controls.Add(PartyAddLessGridPanel);
				PartyAddLessGridPanel.Dock = DockStyle.Left;
				((Control)(object)grd).Size = new Size(1310, 150);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrids()
	{
		try
		{
			if (dtGrid != null && dtGrid.Columns.Count > 0)
			{
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
			}
			if (dtGrdGL != null && dtGrdGL.Columns.Count > 0)
			{
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				grdGLedger.DataSource = dtGrdGL;
				grdGLedger.RetrieveStructure();
				gridGLSettings();
			}
			if (dtInvExp != null && dtInvExp.Columns.Count > 0)
			{
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.DataSource = dtInvExp;
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
			}
			if (dtFreight != null && dtFreight.Columns.Count > 0)
			{
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
				grdFreight.DataSource = dtFreight;
				grdFreight.RetrieveStructure();
				grdFreightSettings();
			}
			if (dtEmptyBags != null && dtEmptyBags.Columns.Count > 0)
			{
				grdEmptyBags.DataSource = dtEmptyBags;
				grdEmptyBags.RetrieveStructure();
				grdEmptyBagsSettings();
			}
			if (dtPaymentTerm != null && dtPaymentTerm.Columns.Count > 0)
			{
				grdPaymentDetail.DataSource = dtPaymentTerm;
				grdPaymentDetail.RetrieveStructure();
				gridPaymentTermSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridcombo()
	{
		try
		{
			if (grdEmptyBags.RowCount > 0)
			{
				EBComboBind();
			}
			if (grdInvExp.RowCount > 0)
			{
				ExpComboBind();
			}
			if (grdGLedger.RowCount > 0)
			{
				GlComboBind();
			}
			if (grdFreight.RowCount > 0)
			{
				FreightComboBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCommonSetting(GridEX grd)
	{
		try
		{
			grd.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grd.RootTable.Columns["AdLsWeight"].Caption = "Less Weight";
			grd.RootTable.Columns["UOMCodeItem"].Caption = "Pack UOM";
			grd.RootTable.Columns["PurchaseOrder"].Caption = "Order No";
			grd.RootTable.Columns["EquivalentPoRate"].Caption = "Rate UOM";
			grd.RootTable.Columns["OrderItemRate"].Caption = "Item Rate";
			grd.RootTable.Columns["LabReportRef"].Caption = "Lab No";
			grd.RootTable.Columns["EbPurAgainstWeightAmount"].Caption = "Empty Bags (Charge to Product)";
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grd.RootTable.Columns["PurchaseOrder"].Visible = GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "PurchaseOrder");
			grd.RootTable.Columns["GrnNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["UOMCodeItem"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["PurchaseOrder"].Width = Constants.InventoryConstants.OrderNo;
			grd.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["EbTotal"].Width = Constants.InventoryConstants.EbWt;
			grd.RootTable.Columns["EbPurAgainstWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["WtCutTotal"].Width = Constants.InventoryConstants.WtCut;
			grd.RootTable.Columns["AdLsWeight"].Width = Constants.InventoryConstants.AddLessConstant;
			grd.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["OrderItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["EquivalentPoRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["RateCut"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateCutAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Freights"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Journal"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Commission"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["LabReportRef"].Width = Constants.InventoryConstants.ReferenceParty;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["WagesAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["EbPurAgainstWeightAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
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
			((Control)(object)CmbPaymentTerm).Enabled = true;
			List<string> hiddenColumns = new List<string>
			{
				"ItemId", "ItemUomId", "WarehouseId", "PackingTypeId", "PurchaseOrderId", "OrderItemRateUOMId", "JobLotId", "InvGrnId", "Id", "InvGrnDetailId",
				"Journal", "EquivalentPoRate", "BranchId"
			};
			List<string> hiddenColumnsExtra = new List<string>
			{
				"Id", "InvGrnId", "InvGrnDetailId", "WarehouseId", "WareHouseName", "ItemId", "ItemUomId", "PackingTypeId", "PurchaseOrderId", "PurchaseOrder",
				"EbPurAgainstWeight", "WtCutTotal", "StockWeight", "OrderItemRateUOMId", "ItemAmount", "JobLotId", "Journal", "EquivalentPoRate", "RateCut", "RateCutAmount",
				"Freights", "Expense", "Commission", "GpNo", "LabReportRef", "GpDate", "WagesAmount", "BranchId", "FreightDeduction"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"RateCut",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 2, HideColumnsFromDetailGridSpecialRight ? hiddenColumnsExtra : hiddenColumns, editableColumns);
			grdCommonSetting(grd);
			if (InvoiceTypeId == 1)
			{
				grd.RootTable.Columns["PurchaseOrder"].Visible = true;
				grd.RootTable.Columns["RateUom"].Visible = true;
				grd.RootTable.Columns["EquivalentPoRate"].Visible = false;
				grd.RootTable.Columns["OrderItemRate"].EditType = (EditType)0;
				grd.RootTable.Columns["EquivalentPoRate"].EditType = (EditType)0;
			}
			else if (InvoiceTypeId == 2)
			{
				grd.RootTable.Columns["RateUom"].Visible = false;
				grd.RootTable.Columns["EquivalentPoRate"].Visible = true;
				grd.RootTable.Columns["OrderItemRate"].EditType = (EditType)1;
				grd.RootTable.Columns["EquivalentPoRate"].EditType = (EditType)4;
			}
			else if (InvoiceTypeId == 3 || InvoiceTypeId == 4)
			{
				if (!RateEditableOnPurchaseInvoice_InGatePurchase)
				{
					grd.RootTable.Columns["RateUom"].Visible = true;
					grd.RootTable.Columns["EquivalentPoRate"].Visible = false;
					grd.RootTable.Columns["OrderItemRate"].EditType = (EditType)0;
					grd.RootTable.Columns["EquivalentPoRate"].EditType = (EditType)0;
				}
				else
				{
					grd.RootTable.Columns["RateUom"].Visible = false;
					grd.RootTable.Columns["EquivalentPoRate"].Visible = true;
					grd.RootTable.Columns["OrderItemRate"].EditType = (EditType)1;
					grd.RootTable.Columns["EquivalentPoRate"].EditType = (EditType)1;
				}
			}
			if (!WeightAddLessOnPurchaseInvoice)
			{
				grd.RootTable.Columns["AdLsWeight"].EditType = (EditType)0;
			}
			else
			{
				grd.RootTable.Columns["AdLsWeight"].EditType = (EditType)1;
			}
			((Control)(object)grd).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if ((e.Column.Key == "OrderItemRate" || e.Column.Key == "RateCut" || e.Column.Key == "RateCutForExcessWeight" || e.Column.Key == "EquivalentPoRate" || e.Column.Key == "AdLsWeight") && item != null && (int)item.RowType == 0)
			{
				string RateCut = item.Cells["RateCut"].Value.ToString();
				string GrossWeight = item.Cells["GrossWeight"].Value.ToString();
				string EbTotal = item.Cells["EbTotal"].Value.ToString();
				string WtCutTotal = item.Cells["WtCutTotal"].Value.ToString();
				double AdLsWeight = Conversion.ToDouble(item.Cells["AdLsWeight"].Value);
				double EbPurAgainstWeight = Conversion.ToDouble(item.Cells["EbPurAgainstWeight"].Value);
				double NetBillWeight = 0.0;
				NetBillWeight = ((InvoiceTypeId == 2) ? (Conversion.ToDouble((object)GrossWeight) - Math.Abs(AdLsWeight)) : ((!(AdLsWeight > 0.0)) ? (Conversion.ToDouble((object)GrossWeight) - Conversion.ToDouble((object)EbTotal) + EbPurAgainstWeight - Conversion.ToDouble((object)WtCutTotal) + Math.Abs(AdLsWeight)) : (Conversion.ToDouble((object)GrossWeight) - Conversion.ToDouble((object)EbTotal) + EbPurAgainstWeight - Conversion.ToDouble((object)WtCutTotal) - Math.Abs(AdLsWeight))));
				item.Cells["NetBillWeight"].Value = NetBillWeight.ToString();
				string EquivalentPoRate = item.Cells["EquivalentPoRate"].Text.ToString();
				double RateCutAmount = Conversion.ToDouble((object)NetBillWeight) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)RateCut);
				item.Cells["RateCutAmount"].Value = RateCutAmount;
				string ItemRate = item.Cells["OrderItemRate"].Value.ToString();
				double ItemAmount = Conversion.ToDouble((object)item.Cells["NetBillWeight"].Value.ToString()) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)ItemRate);
				item.Cells["ItemAmount"].Value = Math.Round(ItemAmount - RateCutAmount, 0, MidpointRounding.AwayFromZero);
			}
			grd.UpdateData();
			CalculateEbPurAgainstWeightAmountBasedOnBalQty();
			BillAmount();
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
			if ((e.Column.Key == "AdLsWeight" || e.Column.Key == "OrderItemRate" || e.Column.Key == "EquivalentPoRate" || e.Column.Key == "RateCut") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
			if (e.Column.Key == "RateCutForExcessWeight" && !CommonServices.IsPositiveNumeric(e))
			{
				throw new Exception("Please Type Only Positive Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void FreightComboBind()
	{
		try
		{
			string ValueMem = (SubsidiaryAccountAllownOnVouchers ? "SuppliercustomerId" : "Id");
			GridEX_Helper.ComboBind(grdFreight.RootTable.Columns["Transporter"], dtAccountlst, ValueMem, "AccountTitle", "Charge To Product", 200);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0);
		grdFreight.DataSource = dtFreight;
	}

	private void grdFreightSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "GlAccountId", "OrderId", "InvGrnId", "FreightId", "FreightAcId", "FreightAmount" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Percentage",
					(EditType)1
				},
				{
					"Qty",
					(EditType)1
				},
				{
					"Debit",
					(EditType)1
				},
				{
					"Freight",
					(EditType)1
				},
				{
					"Rate",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdFreight, 1, 3, hiddenColumns, editableColumns);
			grdFreight.RootTable.Columns["Debit"].Visible = FreightDebitToExpenses;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Transporter"].Caption = "AccountTitle";
			grdFreight.RootTable.Columns["Percentage"].Caption = "%";
			FreightComboBind();
			grdFreight.RootTable.Columns["Transporter"].Width = 150;
			grdFreight.RootTable.Columns["Remarks"].Width = 120;
			grdFreight.RootTable.Columns["Freight"].Width = 80;
			grdFreight.RootTable.Columns["Percentage"].Width = 50;
			GridEX_Helper.AddButton(grdFreight, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdFreight, "Add", "+", 20, 1);
			grdFreight.FrozenColumns = 2;
			SupCustIdUpdateforFrieghtGrid();
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdFreight.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0)
					{
						throw new Exception("Record cannot be deleted because this record againt grn");
					}
					dtFreight.Rows[item.RowIndex].Delete();
					item.Delete();
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInFreightGrid();
			}
			grdFreight.UpdateData();
			BillAmount();
			FreightProportion();
			WagesAmountProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Freight"].Value = amount;
				item.Cells["Percentage"].Value = 0;
				if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0 && amount != Conversion.ToDouble((object)item.Cells["FreightAmount"].Value.ToString()))
				{
					double Credit = Conversion.ToDouble((object)item.Cells["FreightAmount"].Value.ToString());
					item.Cells["Freight"].Value = Credit;
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
					MessageBox.Show("you cannot change CreditAmount against Grn");
					return;
				}
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				if (Math.Abs(Conversion.ToInt(item.Cells["Percentage"].Value)) > 100)
				{
					item.Cells["Percentage"].Value = 0;
					throw new Exception("Percentage mustbe less than 100");
				}
				if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0)
				{
					double Credit2 = Conversion.ToDouble((object)item.Cells["FreightAmount"].Value.ToString());
					item.Cells["Freight"].Value = Credit2;
					item.Cells["Percentage"].Value = 0;
					MessageBox.Show("you cannot change CreditAmount against Grn");
					return;
				}
				double columnSum = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = columnSum / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Freight"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Transporter")
			{
				if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0 && Conversion.ToInt(item.Cells["FreightId"].Value) > 0 && Conversion.ToInt(item.Cells["Transporter"].Value) != Conversion.ToInt(item.Cells["FreightAcId"].Value))
				{
					item.Cells["Transporter"].Value = item.Cells["FreightAcId"].Value;
					MessageBox.Show("Account not change against grn transaction");
					return;
				}
				SupCustIdUpdateforFrieghtGrid();
			}
			if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Freight"].Value = Math.Round(Conversion.ToDouble(item.Cells["Freight"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0 && Conversion.ToInt(item.Cells["FreightId"].Value) > 0)
				{
					double Credit3 = Math.Round(Conversion.ToDouble(item.Cells["FreightAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Freight"].Value = Credit3;
					MessageBox.Show("you cannot change CreditAmount against Grn");
					return;
				}
			}
			if (FreightDebitToExpenses)
			{
				if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
				{
					item.Cells["Freight"].Value = Math.Round(Conversion.ToDouble(item.Cells["Freight"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
					{
						item.Cells["Freight"].Value = 0;
						MessageBox.Show("Debit Side is aleady added");
					}
				}
				if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
				{
					item.Cells["Debit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDouble((object)item.Cells["Freight"].Value.ToString()) > 0.0)
					{
						item.Cells["Debit"].Value = 0;
						MessageBox.Show("Credit Side is aleady added");
					}
				}
			}
			grdFreight.UpdateData();
			FreightProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Freight" || e.Column.Key == "Debit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void SupCustIdUpdateforFrieghtGrid()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow r = grdFreight.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				GridEXRow[] rows = grdFreight.GetRows();
				foreach (GridEXRow item in rows)
				{
					string selectedAcId = Conversion.ToString((object)item.Cells["Transporter"].Text);
					if (selectedAcId != string.Empty && selectedAcId != "0")
					{
						DataRow[] matchingRows = dtAccountlst.Select("AccountTitle='" + selectedAcId + "'");
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			grdFreight.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdInvExp.RootTable.Columns["ItemId"], dtOtherItem, "Id", "OtherItemName", "Other ItemName", 200);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void grdInvExpSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PurchaseOrderId", "PurchaseOrderSupplierExpId", "GrnId", "SupplierDispatchId", "SupplierDispatchExpenseId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"Amount",
					(EditType)1
				},
				{
					"Rate",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdInvExp, 1, 3, hiddenColumns, editableColumns);
			ExpComboBind();
			GridEX_Helper.AddButton(grdInvExp, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdInvExp, "Add", "+", 20, 1);
			grdInvExp.FrozenColumns = 2;
			ctrlGrdBar5_Load(null, null);
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
			BillAmount();
			ExpProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					double Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Amount"].Value = Amount;
				}
			}
			else if (e.Column.Key == "Amount" && item != null && (int)item.RowType == 0 && item.Cells["Amount"].Value.ToString() != string.Empty)
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				double Amount2 = Math.Round(Conversion.ToDouble(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Amount"].Value = Amount2;
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

	private void GlComboBind()
	{
		try
		{
			string ValueMem = (SubsidiaryAccountAllownOnVouchers ? "SuppliercustomerId" : "Id");
			GridEX_Helper.ComboBind(grdGLedger.RootTable.Columns["AccountId"], dtAccountlst, ValueMem, "AccountTitle", "Supplier Add/Less", 200);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, 0, 0, 0, "", 0, 0, 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	private void gridGLSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "GlAccountId", "InvGrnId", "FreightId", "FreightAcId", "FreightAmount", "RateCutQty", "RateCut", "RowType" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Percentage",
					(EditType)1
				},
				{
					"Qty",
					(EditType)1
				},
				{
					"Debit",
					(EditType)1
				},
				{
					"Credit",
					(EditType)1
				},
				{
					"Rate",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdGLedger, 1, 3, hiddenColumns, editableColumns);
			GlComboBind();
			GridEX_Helper.AddButton(grdGLedger, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdGLedger, "Add", "+", 20, 1);
			grdGLedger.FrozenColumns = 2;
			SupCustIdUpdateforGLGrid();
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdGLedger.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0)
					{
						throw new Exception("Record cannot be deleted because this record againt grn");
					}
					dtGrdGL.Rows[item.RowIndex].Delete();
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGLedger.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int RowType = Conversion.ToInt(item.Cells["RowType"].Value);
				if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					double amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Credit"].Value = amount;
					item.Cells["Debit"].Value = 0;
					item.Cells["Percentage"].Value = 0;
					if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0 && RowType != 1 && amount != Conversion.ToDouble((object)item.Cells["FreightAmount"].Value.ToString()))
					{
						double Credit = Conversion.ToDouble((object)item.Cells["FreightAmount"].Value.ToString());
						double RateCutQty = Conversion.ToDouble((object)item.Cells["RateCutQty"].Value.ToString());
						double RateCut = Conversion.ToDouble((object)item.Cells["RateCut"].Value.ToString());
						item.Cells["Credit"].Value = Credit;
						item.Cells["Qty"].Value = RateCutQty;
						item.Cells["Rate"].Value = RateCut;
						MessageBox.Show("you cannot change CreditAmount against Grn");
						return;
					}
				}
				if (e.Column.Key == "Percentage" && item.Cells["Percentage"].Value.ToString() != string.Empty)
				{
					if (Math.Abs(Conversion.ToInt(item.Cells["Percentage"].Value)) > 100)
					{
						item.Cells["Percentage"].Value = 0;
						throw new Exception("Percentage mustbe less than 100");
					}
					if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0)
					{
						double Credit2 = Conversion.ToDouble((object)item.Cells["FreightAmount"].Value.ToString());
						double RateCutQty2 = Conversion.ToDouble((object)item.Cells["RateCutQty"].Value.ToString());
						double RateCut2 = Conversion.ToDouble((object)item.Cells["RateCut"].Value.ToString());
						item.Cells["Credit"].Value = Credit2;
						item.Cells["Qty"].Value = RateCutQty2;
						item.Cells["Rate"].Value = RateCut2;
						item.Cells["Percentage"].Value = 0;
						MessageBox.Show("you cannot change CreditAmount against Grn");
						return;
					}
					double columnSum = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
					double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
					double TotlaPercentage = columnSum / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Credit"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						item.Cells["Debit"].Value = 0;
					}
					else
					{
						item.Cells["Debit"].Value = Math.Round(Math.Abs(TotlaPercentage), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						item.Cells["Credit"].Value = 0;
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
				if (e.Column.Key == "Credit")
				{
					if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0)
					{
						double Credit3 = Conversion.ToDouble((object)item.Cells["FreightAmount"].Value.ToString());
						double RateCutQty3 = Conversion.ToDouble((object)item.Cells["RateCutQty"].Value.ToString());
						double RateCut3 = Conversion.ToDouble((object)item.Cells["RateCut"].Value.ToString());
						item.Cells["Credit"].Value = Credit3;
						item.Cells["Qty"].Value = RateCutQty3;
						item.Cells["Rate"].Value = RateCut3;
						MessageBox.Show("you cannot change CreditAmount against Grn");
						return;
					}
					item.Cells["Credit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
					{
						item.Cells["Credit"].Value = 0;
						MessageBox.Show("Debit Side is aleady added");
					}
				}
				if (e.Column.Key == "Debit")
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
					if (SubsidiaryAccountAllownOnVouchers)
					{
						if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0 && RowType != 1 && Conversion.ToInt(item.Cells["AccountId"].Value) != Conversion.ToInt(item.Cells["FreightAcId"].Value))
						{
							item.Cells["AccountId"].Value = item.Cells["FreightAcId"].Value;
							MessageBox.Show("Account not change against grn transaction");
							return;
						}
						if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(item.Cells["GlAccountId"].Value))
						{
							MessageBox.Show("Supplier Account Not select");
							item.Cells["GlAccountId"].Value = 0;
							return;
						}
					}
					else
					{
						if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0 && RowType != 1 && Conversion.ToInt(item.Cells["AccountId"].Value) != Conversion.ToInt(item.Cells["FreightAcId"].Value))
						{
							item.Cells["AccountId"].Value = item.Cells["FreightAcId"].Value;
							MessageBox.Show("Account not change against grn transaction");
							return;
						}
						if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
						{
							MessageBox.Show("Supplier Account Not select");
							item.Cells["AccountId"].Value = 0;
							return;
						}
					}
					SupCustIdUpdateforGLGrid();
				}
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

	private void grdGLedger_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Credit" || e.Column.Key == "Debit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void SupCustIdUpdateforGLGrid()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow r = grdGLedger.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow item in rows)
				{
					string selectedAcId = Conversion.ToString((object)item.Cells["AccountId"].Text);
					if (selectedAcId != "0" && selectedAcId != string.Empty)
					{
						DataRow[] matchingRows = dtAccountlst.Select("AccountTitle='" + selectedAcId + "'");
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EBComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdEmptyBags.RootTable.Columns["TypeId"], dtEmpType, "Id", "Type", "Type", 100);
			GridEX_Helper.ComboBind(grdEmptyBags.RootTable.Columns["CreditAccountId"], dtCrAccountForEmptyBag, "Id", "AccountTitle", "Credit Title", 180);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdEmptyBagsSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "OrderId", "Type", "ItemId", "InvGrnId", "ItemConditionId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Rate",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdEmptyBags, 1, 3, hiddenColumns, editableColumns);
			grdEmptyBags.RootTable.Columns["ItemName"].Width = 180;
			grdEmptyBags.RootTable.Columns["Remarks"].Width = 150;
			grdEmptyBags.RootTable.Columns["CreditAccountId"].Width = 180;
			EBComboBind();
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdEmptyBags.UpdateData();
			GridEXRow item = grdEmptyBags.CurrentRow;
			if (e.Column.Key == "Rate" && item != null && (int)item.RowType == 0 && item.Cells["PurchaseQty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["PurchaseQty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Amount"].Value = Amount;
			}
			grdEmptyBags.UpdateData();
			CalculateEbPurAgainstWeightAmountBasedOnBalQty();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmtForEmptyBagGrid()
	{
		try
		{
			if ((from r in grdEmptyBags.GetDataRows()
				where (Conversion.ToInt(r.Cells["TypeId"].Value) == 2 || Conversion.ToInt(r.Cells["TypeId"].Value) == 3) && Conversion.ToDouble(r.Cells["CreditAccountId"].Value) > 0.0
				select r).ToList().Any())
			{
				ResetEbPurAgainstWeightAmount();
				return;
			}
			double totalQty = grdEmptyBags.GetDataRows().Where(delegate(GridEXRow r)
			{
				GridEXCell obj3 = r.Cells["TypeId"];
				if (Conversion.ToInt((obj3 != null) ? obj3.Value : null) != 2)
				{
					GridEXCell obj4 = r.Cells["TypeId"];
					return Conversion.ToInt((obj4 != null) ? obj4.Value : null) == 3;
				}
				return true;
			}).Sum(delegate(GridEXRow r)
			{
				GridEXCell obj2 = r.Cells["PurchaseQty"];
				return Conversion.ToDouble((obj2 != null) ? obj2.Value : null);
			});
			if (totalQty <= 0.0)
			{
				ResetEbPurAgainstWeightAmount();
				return;
			}
			double remainingQty = totalQty;
			double totalAmount = 0.0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double ebTotal = Convert.ToDouble(obj.Cells["EbTotal"].Value);
				double qty = Convert.ToDouble(obj.Cells["ItemQty"].Value);
				double rate2 = Convert.ToDouble(obj.Cells["OrderItemRate"].Value);
				double rateUom = Convert.ToDouble(obj.Cells["EquivalentPoRate"].Value);
				double num = ((qty > 0.0) ? (ebTotal / qty) : 0.0);
				double utilizedQty = Math.Min(qty, remainingQty);
				double ebWt = num * utilizedQty;
				double ebAmount = ((rateUom > 0.0) ? (ebWt / rateUom * rate2) : 0.0);
				obj.Cells["EbPurAgainstWeightAmount"].Value = ebAmount;
				totalAmount += ebAmount;
				remainingQty -= utilizedQty;
				obj.EndEdit();
				if (remainingQty <= 0.0)
				{
					break;
				}
			}
			if (totalAmount > 0.0)
			{
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow row in rows)
				{
					if (Conversion.ToInt(row.Cells["TypeId"].Value) == 2 || Conversion.ToInt(row.Cells["TypeId"].Value) == 3)
					{
						row.BeginEdit();
						double purchaseQty = Conversion.ToDouble(row.Cells["PurchaseQty"].Value);
						double rate = ((purchaseQty > 0.0) ? (totalAmount / purchaseQty) : 0.0);
						row.Cells["Amount"].Value = totalAmount;
						row.Cells["Rate"].Value = rate;
						row.EndEdit();
					}
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool CalculateEbPurAgainstWeightAmountBasedOnBalQty()
	{
		try
		{
			if ((from r in grdEmptyBags.GetDataRows()
				where (Conversion.ToInt(r.Cells["TypeId"].Value) == 2 || Conversion.ToInt(r.Cells["TypeId"].Value) == 3) && Conversion.ToDouble(r.Cells["CreditAccountId"].Value) > 0.0
				select r).ToList().Any())
			{
				ResetEbPurAgainstWeightAmount();
				return false;
			}
			double totalAmount = grdEmptyBags.GetDataRows().Where(delegate(GridEXRow r)
			{
				GridEXCell obj5 = r.Cells["TypeId"];
				if (Conversion.ToInt((obj5 != null) ? obj5.Value : null) != 2)
				{
					GridEXCell obj6 = r.Cells["TypeId"];
					return Conversion.ToInt((obj6 != null) ? obj6.Value : null) == 3;
				}
				return true;
			}).Sum(delegate(GridEXRow r)
			{
				GridEXCell obj4 = r.Cells["Amount"];
				return Conversion.ToDouble((obj4 != null) ? obj4.Value : null);
			});
			double totalQty = grdEmptyBags.GetDataRows().Where(delegate(GridEXRow r)
			{
				GridEXCell obj2 = r.Cells["TypeId"];
				if (Conversion.ToInt((obj2 != null) ? obj2.Value : null) != 2)
				{
					GridEXCell obj3 = r.Cells["TypeId"];
					return Conversion.ToInt((obj3 != null) ? obj3.Value : null) == 3;
				}
				return true;
			}).Sum(delegate(GridEXRow r)
			{
				GridEXCell obj = r.Cells["PurchaseQty"];
				return Conversion.ToDouble((obj != null) ? obj.Value : null);
			});
			if (totalQty <= 0.0 || totalAmount <= 0.0)
			{
				ResetEbPurAgainstWeightAmount();
				return false;
			}
			double balQty = totalQty;
			double balAmount = totalAmount;
			double amountPerUnit = totalAmount / totalQty;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				double utilizedQty = Math.Min(Convert.ToDouble(item.Cells["ItemQty"].Value), balQty);
				double ebAmount = amountPerUnit * utilizedQty;
				item.Cells["EbPurAgainstWeightAmount"].Value = ebAmount;
				balQty -= utilizedQty;
				balAmount -= ebAmount;
				item.EndEdit();
				if (balAmount <= 0.0)
				{
					break;
				}
			}
			grd.UpdateData();
			return true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ResetEbPurAgainstWeightAmount()
	{
		GridEXRow[] rows = grd.GetRows();
		foreach (GridEXRow obj in rows)
		{
			obj.BeginEdit();
			obj.Cells["EbPurAgainstWeightAmount"].Value = 0;
			obj.EndEdit();
		}
		grd.UpdateData();
	}

	private void grdEmptyBags_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "PurchaseQty" || e.Column.Key == "Rate" || e.Column.Key == "Amount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void GridPaymentTermRefresh()
	{
		try
		{
			DataTable dtTerm = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalPaymentTerm, (InvDueTerms x) => x.Id, (InvDueTerms x) => x.TermsDescription);
			if (dtTerm != null && dtTerm.Rows.Count > 0)
			{
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].ColumnType = (ColumnType)0;
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].HasValueList = true;
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].LimitToList = true;
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].Caption = "Payment Term";
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].ValueList.PopulateValueList((IEnumerable)dtTerm.DefaultView, "Id", "Description");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridPaymentTermSetting()
	{
		try
		{
			GridPaymentTermRefresh();
			List<string> hiddenColumns = new List<string> { "PurchaseOrderId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"PaymentTerm",
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
					"Amount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				},
				{
					"DueDate",
					(EditType)3
				}
			};
			List<string> NotTotalColumns = new List<string> { "ItemAmount", "Expense", "Freight", "PartyAddLessAmount", "Commission", "BrokeryAmount", "FreightDeduction", "EmptyBagAmount", "NetBillAmount" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPaymentDetail, 2, 3, hiddenColumns, editableColumns, null, ColumnSetting: true, BracketsForamt: false, NotTotalColumns);
			grdPaymentDetail.RootTable.Columns["FreightDeduction"].Caption = "- Freight Deduction";
			grdPaymentDetail.RootTable.Columns["BrokeryAmount"].Caption = "- Brokery Amount";
			grdPaymentDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant + 25;
			grdPaymentDetail.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["PartyAddLessAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["EmptyBagAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["BrokeryAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["FreightDeduction"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["Commission"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["NetBillAmount"].Width = Constants.InventoryConstants.AmountConstant + 25;
			grdPaymentDetail.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant + 50;
			grdPaymentDetail.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdPaymentDetail.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdPaymentDetail.RootTable.Columns["%OfTotal"].Width = Constants.InventoryConstants.ExRateConstant + 20;
			grdPaymentDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant + 20;
			grdPaymentDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["Expense"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["Freight"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["PartyAddLessAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["PartyAddLessAmount"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["Commission"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["BrokeryAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["BrokeryAmount"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["EmptyBagAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["EmptyBagAmount"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["FreightDeduction"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["FreightDeduction"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["NetBillAmount"].TextAlignment = (TextAlignment)3;
			GridEX_Helper.AddButton(grdPaymentDetail, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdPaymentDetail, "Add", "+", 20, 1);
			ctrlGrdBar9_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int orderId = Conversion.ToInt(item.Cells["PurchaseOrderId"].Value);
			int count = dtPaymentTerm.AsEnumerable().Count((DataRow r) => r.Field<int?>("PurchaseOrderId") == orderId);
			if (e.Column.Key == "Delete")
			{
				if (count > 1)
				{
					dtPaymentTerm.Rows[item.RowIndex].Delete();
					item.Delete();
				}
			}
			else if (e.Column.Key == "Add")
			{
				decimal billAmount = Conversion.ToDecimal(item.Cells["NetBillAmount"].Value);
				EnumerableRowCollection<DataRow> allRows = dtPaymentTerm.AsEnumerable();
				EnumerableRowCollection<DataRow> obj = ((orderId > 0) ? allRows.Where((DataRow r) => Conversion.ToInt(r["PurchaseOrderId"]) == orderId) : allRows);
				decimal sumOfAmount = default(decimal);
				decimal sumOfPercent = default(decimal);
				foreach (DataRow r2 in obj)
				{
					sumOfAmount += Conversion.ToDecimal(r2["Amount"]);
					sumOfPercent += Conversion.ToDecimal(r2["%OfTotal"]);
				}
				decimal remainingAmount = billAmount - sumOfAmount;
				decimal remainingPercent = 100m - sumOfPercent;
				if (remainingAmount > 0m && remainingPercent > 0m)
				{
					DataRow sourceRow = dtPaymentTerm.Rows[item.RowIndex];
					DataRow newRow = dtPaymentTerm.NewRow();
					newRow.ItemArray = sourceRow.ItemArray;
					newRow["Amount"] = remainingAmount;
					newRow["%OfTotal"] = remainingPercent;
					dtPaymentTerm.Rows.Add(newRow);
				}
				else
				{
					MessageBox.Show($"Can't break further because Amount/Percent already reached Bill Amount: {billAmount:#,##0.###}\n" + "If you want to break further, first adjust existing rows.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			grdPaymentDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPaymentDetail.UpdateData();
			GridEXRow item = grdPaymentDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (grd.GetRows().Count() > 0)
				{
					decimal billAmount = Conversion.ToDecimal(item.Cells["NetBillAmount"].Value);
					int orderId = Conversion.ToInt(item.Cells["PurchaseOrderId"].Value);
					IEnumerable<GridEXRow> allRows = from GridEXRow r in grdPaymentDetail.GetRows()
						where (int)r.RowType == 0
						select r;
					List<GridEXRow> list = ((orderId > 0) ? allRows.Where((GridEXRow r) => Conversion.ToInt(r.Cells["PurchaseOrderId"].Value) == orderId) : allRows).ToList();
					decimal totalPercentExceptCurrent = default(decimal);
					decimal totalAmountExceptCurrent = default(decimal);
					foreach (GridEXRow r2 in list)
					{
						if (r2 != item)
						{
							totalPercentExceptCurrent += Conversion.ToDecimal(r2.Cells["%OfTotal"].Value);
							totalAmountExceptCurrent += Conversion.ToDecimal(r2.Cells["Amount"].Value);
						}
					}
					decimal currentPercent = Conversion.ToDecimal(item.Cells["%OfTotal"].Value);
					decimal currentAmount = Conversion.ToDecimal(item.Cells["Amount"].Value);
					decimal billAmountSafe = ((billAmount == 0m) ? 1m : billAmount);
					if (e.Column.Key == "%OfTotal")
					{
						if (totalPercentExceptCurrent + currentPercent > 100.01m)
						{
							MessageBox.Show("Total % cannot exceed 100");
							item.BeginEdit();
							item.Cells["%OfTotal"].Value = 0;
							item.Cells["Amount"].Value = 0;
							item.EndEdit();
							return;
						}
						currentAmount = billAmount * currentPercent / 100m;
						item.Cells["Amount"].Value = Math.Round(currentAmount, 4);
						PCalculateByPercent = true;
						PCalculateByAmount = false;
					}
					if (e.Column.Key == "Amount")
					{
						if (totalAmountExceptCurrent + currentAmount > billAmount)
						{
							decimal remaining = billAmount - totalAmountExceptCurrent;
							currentAmount = ((remaining < 0m) ? 0m : remaining);
							item.Cells["Amount"].Value = currentAmount;
							item.Cells["%OfTotal"].Value = 0;
						}
						currentPercent = currentAmount * 100m / billAmountSafe;
						item.Cells["%OfTotal"].Value = Math.Round(currentPercent, 8);
						PCalculateByAmount = true;
						PCalculateByPercent = false;
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
							MessageBox.Show("Due Date can't be less than Doc Date");
							return;
						}
						int days = (date.Date - DocDate.Value.Date).Days;
						item.Cells["DueDays"].Value = days;
					}
				}
				grdPaymentDetail.UpdateData();
				return;
			}
			throw new Exception("No Detail Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPaymentDetail.CurrentColumn == null)
			{
				return;
			}
			int orderId = Conversion.ToInt(item.Cells["PurchaseOrderId"].Value);
			int count = dtPaymentTerm.AsEnumerable().Count((DataRow r) => r.Field<int?>("PurchaseOrderId") == orderId);
			if (e.Control && e.KeyCode == Keys.Space && grdPaymentDetail.CurrentColumn != null)
			{
				if (grdPaymentDetail.CurrentColumn.Key == "Delete")
				{
					if (count > 1)
					{
						dtPaymentTerm.Rows[item.RowIndex].Delete();
						item.Delete();
					}
					grdPaymentDetail.UpdateData();
				}
				else
				{
					if (!(grdPaymentDetail.CurrentColumn.Key == "Add"))
					{
						return;
					}
					decimal billAmount = Conversion.ToDecimal(item.Cells["NetBillAmount"].Value);
					EnumerableRowCollection<DataRow> allRows = dtPaymentTerm.AsEnumerable();
					EnumerableRowCollection<DataRow> obj = ((orderId > 0) ? allRows.Where((DataRow r) => Conversion.ToInt(r["PurchaseOrderId"]) == orderId) : allRows);
					decimal sumOfAmount = default(decimal);
					decimal sumOfPercent = default(decimal);
					foreach (DataRow r2 in obj)
					{
						sumOfAmount += Conversion.ToDecimal(r2["Amount"]);
						sumOfPercent += Conversion.ToDecimal(r2["%OfTotal"]);
					}
					decimal remainingAmount = billAmount - sumOfAmount;
					decimal remainingPercent = 100m - sumOfPercent;
					if (remainingAmount > 0m && remainingPercent > 0m)
					{
						DataRow sourceRow = dtPaymentTerm.Rows[item.RowIndex];
						DataRow newRow = dtPaymentTerm.NewRow();
						newRow.ItemArray = sourceRow.ItemArray;
						newRow["Amount"] = remainingAmount;
						newRow["%OfTotal"] = remainingPercent;
						dtPaymentTerm.Rows.Add(newRow);
					}
					else
					{
						MessageBox.Show($"Can't break further because Amount/Percent already reached Bill Amount: {billAmount:#,##0.###}\n" + "If you want to break further, first adjust existing rows.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					grdPaymentDetail.UpdateData();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (count > 1)
				{
					dtPaymentTerm.Rows[item.RowIndex].Delete();
					item.Delete();
				}
				grdPaymentDetail.UpdateData();
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.D)
				{
					return;
				}
				decimal billAmount2 = Conversion.ToDecimal(item.Cells["NetBillAmount"].Value);
				EnumerableRowCollection<DataRow> allRows2 = dtPaymentTerm.AsEnumerable();
				EnumerableRowCollection<DataRow> obj2 = ((orderId > 0) ? allRows2.Where((DataRow r) => Conversion.ToInt(r["PurchaseOrderId"]) == orderId) : allRows2);
				decimal sumOfAmount2 = default(decimal);
				decimal sumOfPercent2 = default(decimal);
				foreach (DataRow r3 in obj2)
				{
					sumOfAmount2 += Conversion.ToDecimal(r3["Amount"]);
					sumOfPercent2 += Conversion.ToDecimal(r3["%OfTotal"]);
				}
				decimal remainingAmount2 = billAmount2 - sumOfAmount2;
				decimal remainingPercent2 = 100m - sumOfPercent2;
				if (remainingAmount2 > 0m && remainingPercent2 > 0m)
				{
					DataRow sourceRow2 = dtPaymentTerm.Rows[item.RowIndex];
					DataRow newRow2 = dtPaymentTerm.NewRow();
					newRow2.ItemArray = sourceRow2.ItemArray;
					newRow2["Amount"] = remainingAmount2;
					newRow2["%OfTotal"] = remainingPercent2;
					dtPaymentTerm.Rows.Add(newRow2);
				}
				else
				{
					MessageBox.Show($"Can't break further because Amount/Percent already reached Bill Amount: {billAmount2:#,##0.###}\n" + "If you want to break further, first adjust existing rows.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadPaymentDetail()
	{
		try
		{
			var distinctOrders = (from GridEXRow row in grd.GetRows()
				select new
				{
					OrderId = Conversion.ToInt(row.Cells["PurchaseOrderId"].Value),
					OrderNo = Conversion.ToInt(row.Cells["PurchaseOrder"].Value)
				} into x
				where x.OrderId > 0
				select x).Distinct().ToList();
			if (!distinctOrders.Any())
			{
				if (dtPaymentTerm.Rows.Count == 0)
				{
					AddPaymentRow(0, 0, Conversion.ToInt(CmbPaymentTerm.Value), 0, DateTime.Now);
				}
				grdPaymentDetail.DataSource = dtPaymentTerm;
				grdPaymentDetail.RetrieveStructure();
				gridPaymentTermSetting();
				return;
			}
			HashSet<int> existingOrderIds = (from GridEXRow r in grdPaymentDetail.GetRows()
				select Conversion.ToInt(r.Cells["PurchaseOrderId"].Value)).ToHashSet();
			DataTable dt = InvPurchaseInvoice.PaymentTermDetailByPoIds(string.Join(",", distinctOrders.Select(x => x.OrderId)));
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					int soId = Conversion.ToInt(dr["PurchaseOrderId"]);
					if (!existingOrderIds.Contains(soId))
					{
						AddPaymentRow(soId, Conversion.ToInt(dr["PurchaseOrderNo"]), Conversion.ToInt(dr["PaymentTermsId"]), Conversion.ToInt(dr["DueDays"]), Conversion.ToDateTime(dr["DueDate"]), Conversion.ToDecimal(dr["PrcntOfTotal"]));
					}
				}
			}
			else
			{
				foreach (var order in distinctOrders)
				{
					if (!existingOrderIds.Contains(order.OrderId))
					{
						AddPaymentRow(order.OrderId, order.OrderNo, Conversion.ToInt(CmbPaymentTerm.Value), 0, DateTime.Now);
					}
				}
			}
			grdPaymentDetail.DataSource = dtPaymentTerm;
			grdPaymentDetail.RetrieveStructure();
			gridPaymentTermSetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddPaymentRow(int OrderId, int OrderNo, int paymentTermsId, int dueDays, DateTime dueDate, decimal PrcntOfTotal = 0m)
	{
		dtPaymentTerm.Rows.Add(OrderId, OrderNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, paymentTermsId, dueDays, dueDate, PrcntOfTotal, 0, "");
	}

	private void CalculateOrderWiseAmountForPaymentDetail()
	{
		try
		{
			var distinctOrders = (from GridEXRow r in grd.GetRows()
				where Conversion.ToInt(r.Cells["PurchaseOrderId"].Value) > 0
				select r into row
				group row by new
				{
					OrderId = Conversion.ToInt(row.Cells["PurchaseOrderId"].Value),
					OrderNo = Conversion.ToInt(row.Cells["PurchaseOrder"].Value)
				} into g
				select new
				{
					OrderId = g.Key.OrderId,
					OrderNo = g.Key.OrderNo,
					TotalItemAmount = g.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["ItemAmount"].Value))
				}).ToList();
			if (distinctOrders.Count == 0 && grd.GetRows().Count() > 0)
			{
				HandleNoOrderPaymentCalculation();
				grdPaymentDetail.DataSource = dtPaymentTerm;
				grdPaymentDetail.UpdateData();
				return;
			}
			decimal journalDebit = default(decimal);
			decimal journalCredit = default(decimal);
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["AccountId"].Value) > 0)
				{
					journalDebit += Conversion.ToDecimal((object)r2.Cells["Debit"].Value.ToString());
					journalCredit += Conversion.ToDecimal((object)r2.Cells["Credit"].Value.ToString());
				}
			}
			int partyId = Conversion.ToInt(cmbsuppliername.Value);
			(decimal credit, decimal debit) freightAmounts = GetFreightAmounts(partyId);
			decimal freightCredit = freightAmounts.credit;
			decimal freightDebit = freightAmounts.debit;
			decimal totalExpense = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdInvExp, "Amount"));
			decimal brokeryAmount = GetBrokeryAmount();
			decimal emptyBagsAmount = GetEmptyBagsAmount();
			decimal freightDeduction = GetFreightDeduction(partyId);
			decimal commission = GetCommission(partyId);
			decimal glDifference = journalDebit - journalCredit;
			decimal transDifference = freightCredit - freightDebit;
			var gridLookup = (from r in dtGrid.AsEnumerable()
				group r by Conversion.ToInt(r["PurchaseOrderId"])).ToDictionary((IGrouping<int, DataRow> g) => g.Key, (IGrouping<int, DataRow> g) => new
			{
				ItemAmount = g.Sum((DataRow r) => Conversion.ToDecimal(r["ItemAmount"])),
				Expense = g.Sum((DataRow r) => Conversion.ToDecimal(r["Expense"])),
				Freights = g.Sum((DataRow r) => Conversion.ToDecimal(r["Freights"])),
				Commission = g.Sum((DataRow r) => Conversion.ToDecimal(r["Commission"]))
			});
			Dictionary<int, List<DataRow>> paymentLookup = (from r in dtPaymentTerm.AsEnumerable()
				group r by Conversion.ToInt(r["PurchaseOrderId"])).ToDictionary((IGrouping<int, DataRow> g) => g.Key, (IGrouping<int, DataRow> g) => g.ToList());
			foreach (var order in distinctOrders)
			{
				gridLookup.TryGetValue(order.OrderId, out var sums);
				decimal itemAmount = sums?.ItemAmount ?? 0m;
				decimal proportionedExpense = sums?.Expense ?? 0m;
				_ = sums?.Freights;
				_ = sums?.Commission;
				decimal TotalItemAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemAmount"));
				decimal perOrderJournal = Divide(glDifference, order.TotalItemAmount, TotalItemAmount);
				decimal perOrderFreight = Divide(transDifference, order.TotalItemAmount, TotalItemAmount);
				decimal perOrderExpense = Divide(totalExpense, order.TotalItemAmount, TotalItemAmount);
				decimal num = Divide(commission, order.TotalItemAmount, TotalItemAmount);
				decimal perOrderFreightDeduction = Divide(freightDeduction, order.TotalItemAmount, TotalItemAmount);
				decimal perOrderBrokery = Divide(brokeryAmount, order.TotalItemAmount, TotalItemAmount);
				decimal perOrderEmptyBag = Divide(emptyBagsAmount, order.TotalItemAmount, TotalItemAmount);
				decimal expenseAmount = ((proportionedExpense > 0m) ? proportionedExpense : perOrderExpense);
				decimal freight = perOrderFreight;
				decimal commissionAmount = num;
				decimal netBillAmount = itemAmount + expenseAmount + perOrderJournal + freight + commissionAmount + perOrderEmptyBag - perOrderBrokery - perOrderFreightDeduction;
				if (!paymentLookup.TryGetValue(order.OrderId, out var paymentRows))
				{
					continue;
				}
				foreach (DataRow item in paymentRows)
				{
					item.BeginEdit();
					item.SetField("ItemAmount", itemAmount);
					item.SetField("Expense", expenseAmount);
					item.SetField("Freight", freight);
					item.SetField("PartyAddLessAmount", perOrderJournal);
					item.SetField("EmptyBagAmount", perOrderEmptyBag);
					item.SetField("FreightDeduction", perOrderFreightDeduction);
					item.SetField("Commission", commissionAmount);
					item.SetField("BrokeryAmount", perOrderBrokery);
					item.SetField("NetBillAmount", netBillAmount);
					item.EndEdit();
				}
			}
			grdPaymentDetail.DataSource = dtPaymentTerm;
			grdPaymentDetail.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception("[CalculateOrderWiseAmountForPaymentDetail] " + ex.Message, ex);
		}
	}

	private void HandleNoOrderPaymentCalculation()
	{
		decimal journalDebit = default(decimal);
		decimal journalCredit = default(decimal);
		GridEXRow[] rows = grdGLedger.GetRows();
		foreach (GridEXRow r in rows)
		{
			if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0)
			{
				journalDebit += Conversion.ToDecimal((object)r.Cells["Debit"].Value.ToString());
				journalCredit += Conversion.ToDecimal((object)r.Cells["Credit"].Value.ToString());
			}
		}
		decimal glDifference = journalDebit - journalCredit;
		int partyId = Conversion.ToInt(cmbsuppliername.Value);
		(decimal credit, decimal debit) freightAmounts = GetFreightAmounts(partyId);
		decimal freightCredit = freightAmounts.credit;
		decimal freightDebit = freightAmounts.debit;
		decimal transDifference = freightCredit - freightDebit;
		decimal totalExpense = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdInvExp, "Amount"));
		decimal brokeryAmount = GetBrokeryAmount();
		decimal emptyBagsAmount = GetEmptyBagsAmount();
		decimal freightDeduction = GetFreightDeduction(partyId);
		decimal commission = GetCommission(partyId);
		decimal itemAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemAmount"));
		decimal netBillAmount = itemAmount + totalExpense + glDifference + transDifference + commission + emptyBagsAmount - brokeryAmount - freightDeduction;
		if (dtPaymentTerm.Rows.Count == 0)
		{
			AddPaymentRow(0, 0, Conversion.ToInt(CmbPaymentTerm.Value), 0, DateTime.Today);
		}
		foreach (DataRow row in dtPaymentTerm.Rows)
		{
			row.BeginEdit();
			row.SetField("ItemAmount", itemAmount);
			row.SetField("Expense", totalExpense);
			row.SetField("Freight", transDifference);
			row.SetField("PartyAddLessAmount", glDifference);
			row.SetField("EmptyBagAmount", emptyBagsAmount);
			row.SetField("FreightDeduction", freightDeduction);
			row.SetField("Commission", commission);
			row.SetField("BrokeryAmount", brokeryAmount);
			row.SetField("NetBillAmount", netBillAmount);
			row.EndEdit();
		}
	}

	private static decimal Divide(decimal value, decimal orderAmount, decimal TotalItemAmount)
	{
		if (!(value != 0m) || !(TotalItemAmount > 0m))
		{
			return 0m;
		}
		return value / TotalItemAmount * orderAmount;
	}

	private (decimal credit, decimal debit) GetFreightAmounts(int partyId)
	{
		IEnumerable<GridEXRow> source = from GridEXRow r in grdFreight.GetDataRows()
			where (int)r.RowType == 0
			select r;
		int filterKey = (SubsidiaryAccountAllownOnVouchers ? partyId : CommonServices.GetGlAccountIdBySupplierCustomerId(partyId));
		List<GridEXRow> source2 = source.Where((GridEXRow r) => Conversion.ToInt(r.Cells["Transporter"].Value) == filterKey).ToList();
		decimal credit = source2.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Freight"].Value));
		decimal debit = source2.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Debit"].Value));
		return (credit: credit, debit: debit);
	}

	private decimal GetBrokeryAmount()
	{
		if (((UltraGridBase)CmbBrokeryAc).ActiveRow == null)
		{
			return 0m;
		}
		decimal amount = Conversion.ToDecimal((object)txtBrokeryAmount.Text.Trim());
		if (!(amount > 0m))
		{
			return 0m;
		}
		return amount;
	}

	private decimal GetEmptyBagsAmount()
	{
		return (from GridEXRow r in grdEmptyBags.GetRows()
			where (int)r.RowType == 0 && Conversion.ToInt(r.Cells["TypeId"].Value) == 1 && Conversion.ToDecimal(r.Cells["Amount"].Value) > 0m
			select r).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Amount"].Value));
	}

	private decimal GetFreightDeduction(int partyId)
	{
		if (!(SubsidiaryAccountAllownOnVouchers ? (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(CmbFreightAccount.Value)) : (Conversion.ToString((object)txtSupplierGLId.Text) == Conversion.ToString(CmbFreightAccount.Value))))
		{
			return 0m;
		}
		return Conversion.ToDecimal((object)txtFreightDeduction.Text);
	}

	private decimal GetCommission(int partyId)
	{
		if (Conversion.ToInt(cmbcommagent.Value) <= 0)
		{
			return 0m;
		}
		if (Conversion.ToInt(cmbsuppliername.Value) != Conversion.ToInt(cmbcommagent.Value))
		{
			return 0m;
		}
		return Conversion.ToDecimal((object)txtcommamount.Text);
	}

	private void PaymentAmountReCalculate()
	{
		try
		{
			foreach (IGrouping<int, GridEXRow> group in from r in (from GridEXRow r in grdPaymentDetail.GetRows()
					where (int)r.RowType == 0
					select r).ToList()
				group r by Conversion.ToInt(r.Cells["PurchaseOrderId"].Value))
			{
				group.Count();
				if (!PCalculateByPercent)
				{
					continue;
				}
				foreach (GridEXRow item in group)
				{
					decimal totalAmount = Conversion.ToDecimal(item.Cells["NetBillAmount"].Value);
					decimal percent = Conversion.ToDecimal(item.Cells["%OfTotal"].Value);
					if (percent < 0m)
					{
						percent = default(decimal);
					}
					decimal amount = percent * totalAmount / 100m;
					amount = Math.Round(amount, 4, MidpointRounding.AwayFromZero);
					item.BeginEdit();
					item.Cells["Amount"].Value = amount;
					item.EndEdit();
				}
			}
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
				"AccountsWithCustomGroupId", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "MultiCurrency", "AllCities", "CropYear", "JobLot", "InvPackingType",
				"PaymentTerm"
			};
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			CommissionUOMFill();
			LocationTypeBind(VoucherHead.GetLocationType());
			SupplierDtFillFromGlobal();
			BindSupplierName();
			AccountlstDtFillFromGlobal();
			PaymentTermBind(CommonServices.GetDueTermServiceBind());
			FreightDeductionAccountFill();
			OtherItemsdtFillDbCall();
			ImplementConfiguration();
			BindGridcombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchaseInvoice_FormClosing(object sender, FormClosingEventArgs e)
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

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		_cts?.Cancel();
		_cts?.Dispose();
		base.OnFormClosing(e);
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			lstRemoveRecord.Clear();
			RecId = 0;
			POID = 0;
			VoucherHeadId = 0;
			Approved = false;
			chkCustomAccounts.Checked = false;
			GrnDate = DateTime.Now;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			DocDate.Focus();
			((Control)(object)cmbsuppliername).Enabled = true;
			((Control)(object)cmbsuppliername).Text = string.Empty;
			txtFreightDeduction.Text = string.Empty;
			((Control)(object)cmbcommagent).Text = string.Empty;
			txtWagesAmountHeader.Text = string.Empty;
			((Control)(object)cmbcommtype).Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)cmbcommuom).Text = string.Empty;
			txtcommremarks.Text = string.Empty;
			((Control)(object)CmbBrokeryAc).Text = string.Empty;
			((Control)(object)CmbBrokeryType).Text = string.Empty;
			((Control)(object)CmbBrokeryRateUom).Text = string.Empty;
			txtBrokeryRate.Text = string.Empty;
			txtBrokeryAmount.Text = string.Empty;
			txtbillno.Text = string.Empty;
			txtDeliveryTerm.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtDueDays.Text = string.Empty;
			txtGrnNo.Text = string.Empty;
			txtRefParty.Text = string.Empty;
			DueDate.Value = DateTime.Now;
			((Control)(object)CmbPaymentTerm).Text = string.Empty;
			((Control)(object)CmbPaymentTerm).Enabled = true;
			txtBillAmount.Text = "0";
			dtGrid.Rows.Clear();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtFreight.Rows.Clear();
			dtEmptyBags.Rows.Clear();
			dtPaymentTerm.Rows.Clear();
			BindGrids();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			txtdocno.Text = Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCode(DocumentTypeId));
			txtBranchCode.Text = GenerateBranchSrNo();
			cmbsuppliername.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvPurchaseInvoiceDetail Pd, GridEXRow r)
	{
		Pd.PurchaseOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
		Pd.PurchaseOrder = Conversion.ToInt(r.Cells["PurchaseOrder"].Value);
		Pd.InvGrnDetailId = Conversion.ToInt(r.Cells["InvGrnDetailId"].Value);
		Pd.InvGrnId = Conversion.ToInt(r.Cells["InvGrnId"].Value);
		Pd.GrnNo = Conversion.ToInt(r.Cells["GrnNo"].Value);
		Pd.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		Pd.WareHouseName = Conversion.ToString(r.Cells["WareHouseName"].Value);
		Pd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		Pd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		Pd.CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
		Pd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		Pd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
		Pd.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
		Pd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
		Pd.EBTotalWt = Conversion.ToDouble(r.Cells["EbTotal"].Value);
		Pd.WeightCutTotal = Conversion.ToDouble(r.Cells["WtCutTotal"].Value);
		Pd.AdLsWeight = Conversion.ToDouble(r.Cells["AdLsWeight"].Value);
		Pd.NetBillWeight = Conversion.ToDouble(r.Cells["NetBillWeight"].Value);
		Pd.NetStockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
		Pd.ItemCgsRate = 0.0;
		Pd.ItemRate = Conversion.ToDouble(r.Cells["OrderItemRate"].Value);
		Pd.ItemUOMId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
		Pd.ItemAmount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
		Pd.ItemAmount = Math.Round(Pd.ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		Pd.UomScheduleIdRate = Conversion.ToInt(r.Cells["OrderItemRateUOMId"].Value);
		if (InvoiceTypeId != 1)
		{
			if (InvoiceTypeId == 2)
			{
				DataTable dt2 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
				if (dt2.Rows.Count > 0)
				{
					DataRow[] dr = dt2.Select("Equivalent='" + Conversion.ToInt(r.Cells["EquivalentPoRate"].Value) + "'");
					if (dr.Length == 0)
					{
						MessageBox.Show("this RateUom not define please check");
						return;
					}
					Pd.UomScheduleIdRate = Conversion.ToInt(dr[0].ItemArray[0]);
				}
			}
			else if (InvoiceTypeId == 3 || InvoiceTypeId == 4)
			{
				DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
				if (dt.Rows.Count > 0)
				{
					DataRow[] dr2 = dt.Select("Equivalent='" + Conversion.ToInt(r.Cells["EquivalentPoRate"].Value) + "'");
					if (dr2.Length == 0)
					{
						MessageBox.Show("RateUom not define against this item " + Conversion.ToString(r.Cells["ItemName"].Value));
						return;
					}
					Pd.UomScheduleIdRate = Conversion.ToInt(dr2[0].ItemArray[0]);
				}
			}
		}
		Pd.RateCut = Conversion.ToDouble(r.Cells["RateCut"].Value);
		Pd.EbPurAgainstWeight = Conversion.ToDouble(r.Cells["EbPurAgainstWeight"].Value);
		Pd.EbPurAgainstWeightAmount = Conversion.ToDouble(r.Cells["EbPurAgainstWeightAmount"].Value);
		Pd.RateCutAmount = Conversion.ToDouble(r.Cells["RateCutAmount"].Value);
		if (!FreightDebitToExpenses)
		{
			Pd.FreightAmount = Conversion.ToDouble(r.Cells["Freights"].Value);
		}
		Pd.JournalAmount = Conversion.ToDouble(r.Cells["Journal"].Value);
		Pd.ExpenseAmount = Conversion.ToDouble(r.Cells["Expense"].Value);
		Pd.CommissionAmount = Conversion.ToDouble(r.Cells["Commission"].Value);
		Pd.LabAnalisysNo = Conversion.ToString(r.Cells["LabReportRef"].Value);
		Pd.GpNo = Conversion.ToInt(r.Cells["GpNo"].Value);
		Pd.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
		Pd.GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
		Pd.WagesAmount = Conversion.ToDouble(r.Cells["WagesAmount"].Value);
		Pd.FreightDeduction = Conversion.ToDouble(r.Cells["FreightDeduction"].Value);
		Pd.BranchId = Conversion.ToInt(r.Cells["BranchId"].Value);
		Pd.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
	}

	private void FilldtDetailFromListCommonForReadById(InvPurchaseInvoice invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.invPurchaseInvoiceDetailList == null || invGrn.invPurchaseInvoiceDetailList.Count == 0)
		{
			return;
		}
		foreach (InvPurchaseInvoiceDetail detail in invGrn.invPurchaseInvoiceDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id,
				detail.InvGrnDetailId,
				detail.InvGrnId,
				detail.GrnNo,
				detail.WarehouseId,
				detail.WareHouseName,
				detail.ItemId,
				detail.ItemName,
				detail.CropYear,
				detail.ItemUOMId,
				detail.UOMCodeItem,
				detail.ItemQty,
				detail.PackingTypeId,
				detail.PurchaseOrderId,
				detail.PurchaseOrder,
				detail.GrossWeight,
				detail.EBTotalWt,
				detail.EbPurAgainstWeight,
				detail.WeightCutTotal,
				detail.AdLsWeight,
				detail.NetBillWeight,
				detail.NetStockWeight,
				detail.ItemRate,
				detail.UomScheduleIdRate,
				detail.RateUom,
				detail.EquivalentPoRate,
				detail.ItemAmount,
				detail.JobLotId,
				detail.RateCut,
				detail.RateCutAmount,
				detail.BillAmount,
				detail.FreightAmount,
				detail.ExpenseAmount,
				detail.JournalAmount,
				detail.CommissionAmount,
				detail.LabAnalisysNo,
				detail.GpNo,
				detail.VehicleNo,
				Conversion.ToDateTime((object)detail.GpDate).ToShortDateString(),
				detail.WagesAmount,
				detail.BranchId,
				detail.BranchName,
				detail.EbPurAgainstWeightAmount,
				detail.FreightDeduction
			};
			if (AddExtraColumns)
			{
				rowValues.Add(detail.JobLotDescription);
				rowValues.Add(detail.PackTypeDesc);
			}
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void Insert()
	{
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_0a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9d: Expected O, but got Unknown
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d27: Expected O, but got Unknown
		//IL_0efd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f04: Expected O, but got Unknown
		//IL_14c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c8: Expected O, but got Unknown
		//IL_12af: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b6: Expected O, but got Unknown
		//IL_1cf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9e: Expected O, but got Unknown
		//IL_1b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c59: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "DocNo", FormHelper.ValidType.String),
				((Control)(object)cmbsuppliername, "Supplier Name", FormHelper.ValidType.String)
			};
			if (((UltraGridBase)cmbLocationType).DataSource != null)
			{
				controlsToValidate.Add(((Control)(object)cmbLocationType, "Location Type", FormHelper.ValidType.String));
			}
			if (!FormHelper.ValidateControls(controlsToValidate) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			if (Conversion.ToDecimal((object)txtFreightDeduction.Text) > 0m && (Conversion.ToInt(CmbFreightAccount.Value) == 0 || ((UltraGridBase)CmbFreightAccount).ActiveRow == null))
			{
				MessageBox.Show("Freight Deduction Debit Account Field Is Required");
				CmbFreightAccount.Focus();
				return;
			}
			if (RecId == 0 && ValidateGrnAndInvoiceDateWithGpDate && Conversion.ToDateTime((object)DocDate.Value).Date != GrnDate.Date)
			{
				throw new Exception("Invoice Date Should be Equal to GrnDate");
			}
			bool EmptyBagPurAgWtPropotionate = CalculateEbPurAgainstWeightAmountBasedOnBalQty();
			TotalCommissionAmount();
			ExpProportion();
			FreightProportion();
			FreightDeductionProportion();
			WagesAmountProportion();
			CommissionProportion();
			BillProportion();
			BillAmount();
			double TotalFreightCredit = 0.0;
			double TotalFreightDebit = 0.0;
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r8 in rows)
			{
				TotalFreightCredit += Conversion.ToDouble(r8.Cells["Freight"].Value);
				TotalFreightDebit += Conversion.ToDouble(r8.Cells["Debit"].Value);
				if (!(Conversion.ToDouble(r8.Cells["Freight"].Value) > 0.0))
				{
					continue;
				}
				if (Conversion.ToInt(r8.Cells["Transporter"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against Freight First");
				}
				if (Conversion.ToDouble(r8.Cells["Freight"].Value) > 0.0)
				{
					string FrRemarks = Conversion.ToString(r8.Cells["Remarks"].Value);
					if (FrRemarks == string.Empty || FrRemarks == null)
					{
						((Control)(object)grdFreight).Focus();
						throw new Exception("Grid Charge to Product Remarks required Please Check");
					}
				}
			}
			if (FreightDebitToExpenses && (TotalFreightCredit > 0.0 || TotalFreightDebit > 0.0) && TotalFreightCredit != TotalFreightDebit)
			{
				MessageBox.Show("Freight Debit Amount not equal to Credit Amount Please Check");
				return;
			}
			if (!FreightDebitToExpenses && TotalFreightDebit > 0.0)
			{
				MessageBox.Show("Freight Debit Row Remove first");
				return;
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r9 in rows)
			{
				if ((Conversion.ToDouble(r9.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r9.Cells["Debit"].Value) > 0) && Conversion.ToInt(r9.Cells["AccountId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against JL First");
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r10 in rows)
			{
				if (Conversion.ToDouble(r10.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r10.Cells["ItemId"].Value) == 0)
				{
					throw new Exception("Please Select an Item Against Expense First");
				}
			}
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				if (Conversion.ToInt(r7.Cells["ItemId"].Value) > 0 && Conversion.ToDouble(r7.Cells["PurchaseQty"].Value) > 0.0 && Conversion.ToDouble(r7.Cells["Amount"].Value) == 0.0)
				{
					throw new Exception("Packing Material Amount Required");
				}
			}
			if (Conversion.ToDouble((object)txtcommamount.Text) > 0.0 && Conversion.ToInt(cmbcommagent.Value) == 0)
			{
				throw new Exception("Please Select Commission Agent Account First");
			}
			InvPurchaseInvoice purchaseInvoice = new InvPurchaseInvoice();
			purchaseInvoice.OrganizationId = UserAccount.OrganizationId;
			purchaseInvoice.CompanyId = UserAccount.CompanyId;
			purchaseInvoice.BranchesId = UserAccount.BranchesId;
			purchaseInvoice.ProjectsId = UserAccount.BranchesId;
			purchaseInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			purchaseInvoice.DocumentTypeId = DocumentTypeId;
			purchaseInvoice.ScreenName = Conversion.ToString((object)ScreenName);
			purchaseInvoice.EntryDate = DateTime.Now;
			purchaseInvoice.ModifyDate = DateTime.Now;
			purchaseInvoice.PostDate = DateTime.Now;
			purchaseInvoice.EntryUser = UserAccount.ID;
			purchaseInvoice.ModifyUser = UserAccount.ID;
			purchaseInvoice.Id = RecId;
			purchaseInvoice.DocDate = DocDate.Value;
			purchaseInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			purchaseInvoice.BranchSrNo = Conversion.ToInt((object)txtBranchCode.Text);
			purchaseInvoice.SupplierCustomerId = Conversion.ToInt(cmbsuppliername.Value);
			purchaseInvoice.ManualBillNo = txtbillno.Text.Trim();
			purchaseInvoice.RemarksHeader = txtRemarks.Text.Trim();
			purchaseInvoice.OtherRemarks = txtRemarks.Text.Trim();
			purchaseInvoice.CustomAccounts = Conversion.ToBool((object)chkCustomAccounts.Checked);
			purchaseInvoice.CommissionAgentId = Conversion.ToInt(cmbcommagent.Value);
			purchaseInvoice.CommissionType = ((Control)(object)cmbcommtype).Text.Trim();
			purchaseInvoice.CommRate = Conversion.ToDouble((object)txtcommrate.Text);
			purchaseInvoice.UomScheduleIdCmRate = ((Control)(object)cmbcommuom).Text.Trim();
			purchaseInvoice.CommAmount = Conversion.ToDouble((object)txtcommamount.Text);
			if (txtcommremarks.Text.Trim() == string.Empty)
			{
				purchaseInvoice.CommissionRemarks = " CommRate:   " + purchaseInvoice.CommRate + "  CommType: " + purchaseInvoice.CommissionType;
			}
			else
			{
				purchaseInvoice.CommissionRemarks = txtcommremarks.Text.Trim();
			}
			purchaseInvoice.BrokerAgentId = Conversion.ToInt(CmbBrokeryAc.Value);
			purchaseInvoice.BrokeryType = ((Control)(object)CmbBrokeryType).Text.Trim();
			purchaseInvoice.BrokeryRate = Conversion.ToDouble((object)txtBrokeryRate.Text);
			purchaseInvoice.BrokeryUom = Conversion.ToDouble((object)((Control)(object)CmbBrokeryRateUom).Text);
			purchaseInvoice.BrokeryAmount = Conversion.ToDouble((object)txtBrokeryAmount.Text);
			if (purchaseInvoice.BrokerAgentId > 0 && purchaseInvoice.BrokeryAmount > 0.0 && purchaseInvoice.BrokerAgentId == purchaseInvoice.SupplierCustomerId)
			{
				throw new Exception("The broker and party account cannot be equal");
			}
			purchaseInvoice.StockPartyId = Conversion.ToInt(cmbstockprty.Value);
			purchaseInvoice.WagesAmount = Conversion.ToDouble((object)txtWagesAmountHeader.Text.Trim());
			purchaseInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			purchaseInvoice.SupplierInvoiceDate = datSupplierInvoiceDate.Value;
			purchaseInvoice.DueDate = DueDate.Value;
			purchaseInvoice.DueDays = Conversion.ToInt((object)txtDueDays.Text.Trim());
			purchaseInvoice.DocumentTypeSrNo = Conversion.ToInt((object)txtGrnNo.Text.Trim());
			purchaseInvoice.DeliveryTerm = Conversion.ToString((object)txtDeliveryTerm.Text.Trim());
			purchaseInvoice.PaymentTermsId = Conversion.ToInt(CmbPaymentTerm.Value);
			purchaseInvoice.ReferencePartyName = Conversion.ToString((object)txtRefParty.Text.Trim());
			purchaseInvoice.InvoiceTypeId = InvoiceTypeId;
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				purchaseInvoice.TransportAccountId = ((Conversion.ToDouble((object)txtFreightDeduction.Text) > 0.0) ? Conversion.ToInt(CmbFreightAccount.Value) : 0);
				purchaseInvoice.TransporterCreditPartyId = 0;
			}
			else
			{
				purchaseInvoice.TransporterCreditPartyId = ((Conversion.ToDouble((object)txtFreightDeduction.Text) > 0.0) ? Conversion.ToInt(((UltraDropDownBase)CmbFreightAccount).SelectedRow.Cells[3].Value) : 0);
				purchaseInvoice.TransportAccountId = ((Conversion.ToDouble((object)txtFreightDeduction.Text) > 0.0) ? Conversion.ToInt(((UltraDropDownBase)CmbFreightAccount).SelectedRow.Cells[1].Value) : 0);
			}
			purchaseInvoice.FreightAmount = Conversion.ToDouble((object)txtFreightDeduction.Text);
			purchaseInvoice.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			purchaseInvoice.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
			purchaseInvoice.InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
			purchaseInvoice.InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
			purchaseInvoice.InvPurchaseInvoiceEmptyBagslist = new List<InvPurchaseInvoiceEmptyBags>();
			if (lstRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int j = 0; j < lstRemoveRecord.Count; j++)
				{
					purchaseInvoice.invPurchaseInvoiceDetailList.Add(lstRemoveRecord[j]);
				}
			}
			rows = grd.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
				Pd.Id = ((RecId != 0) ? Conversion.ToInt(r6.Cells["Id"].Value) : 0);
				Pd.ActionTypeId = ((Pd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(Pd, r6);
				FormHelper.ValidateField(Pd.InvGrnDetailId, "InvGrnDetailId", r6.RowIndex);
				FormHelper.ValidateField(Pd.InvGrnId, "InvGrnId", r6.RowIndex);
				FormHelper.ValidateField(Pd.WarehouseId, "WareHouse", r6.RowIndex);
				FormHelper.ValidateField(Pd.ItemId, "Item", r6.RowIndex);
				FormHelper.ValidateField(Pd.CropYear, "CropYear", r6.RowIndex);
				FormHelper.ValidateField(Pd.PackingTypeId, "PackingType", r6.RowIndex);
				FormHelper.ValidateField(Pd.JobLotId, "JobLot", r6.RowIndex);
				FormHelper.ValidateField(Pd.ItemUOMId, "ItemUom", r6.RowIndex);
				FormHelper.ValidateField(Pd.ItemQty, "ItemQty", r6.RowIndex);
				FormHelper.ValidateField(Pd.GrossWeight, "GrossWeight", r6.RowIndex);
				FormHelper.ValidateField(Pd.NetBillWeight, "NetBillWeight", r6.RowIndex);
				FormHelper.ValidateField(Pd.NetStockWeight, "NetStockWeight", r6.RowIndex);
				FormHelper.ValidateField(Pd.ItemRate, "Item Rate", r6.RowIndex);
				FormHelper.ValidateField(Pd.ItemAmount, "ItemAmount", r6.RowIndex);
				purchaseInvoice.invPurchaseInvoiceDetailList.Add(Pd);
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToInt((object)r5.Cells["Transporter"].Value.ToString()) != 0)
				{
					InvPurchaseInvoiceFreight PF = new InvPurchaseInvoiceFreight();
					PF.PurchaseOrderId = Conversion.ToInt(r5.Cells["OrderId"].Value);
					PF.TansporterId = Conversion.ToInt(r5.Cells["GlAccountId"].Value);
					PF.InvGrnId = Conversion.ToInt(r5.Cells["InvGrnId"].Value);
					PF.FreightId = Conversion.ToInt(r5.Cells["FreightId"].Value);
					PF.SupplierCustomerId = Conversion.ToInt(r5.Cells["Transporter"].Value);
					PF.Percentage = Conversion.ToDouble(r5.Cells["Percentage"].Value);
					PF.FrQty = Conversion.ToDouble(r5.Cells["Qty"].Value);
					PF.FrRate = Conversion.ToDouble(r5.Cells["Rate"].Value);
					PF.FreightAmount = Conversion.ToDouble(r5.Cells["Freight"].Value);
					PF.Debit = Conversion.ToDouble(r5.Cells["Debit"].Value);
					PF.Remarks = Conversion.ToString(r5.Cells["Remarks"].Value);
					purchaseInvoice.InvPurchaseInvoiceFreightList.Add(PF);
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvPurchaseInvoiceExpense PE = new InvPurchaseInvoiceExpense();
					PE.Id = Conversion.ToInt(r4.Cells["Id"].Value);
					PE.GrnId = Conversion.ToInt(r4.Cells["GrnId"].Value);
					PE.GrnNo = Conversion.ToInt(r4.Cells["GrnNo"].Value);
					PE.PurchaseOrderId = Conversion.ToInt(r4.Cells["PurchaseOrderId"].Value);
					PE.PurchaseOrderSupplierExpId = Conversion.ToInt(r4.Cells["PurchaseOrderSupplierExpId"].Value);
					PE.SupplierDispatchId = Conversion.ToInt(r4.Cells["SupplierDispatchId"].Value);
					PE.SupplierDispatchExpenseId = Conversion.ToInt(r4.Cells["SupplierDispatchExpenseId"].Value);
					PE.InvRevExpItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToDouble(r4.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r4.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDouble(r4.Cells["Amount"].Value);
					PE.CustomRemarks = Conversion.ToString((object)r4.Cells["Remarks"].Text.Trim());
					PE.Remarks = Conversion.ToString((object)r4.Cells["Remarks"].Text.Trim());
					if (PE.Qty > 0.0 && PE.Rate > 0.0)
					{
						PE.Remarks = "ItemName : " + Conversion.ToString((object)r4.Cells["ItemId"].Text.Trim()) + "  " + $"Qty: {PE.Qty} " + $"Rate: {PE.Rate} " + PE.Remarks;
					}
					purchaseInvoice.InvPurchaseInvoiceExpList.Add(PE);
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				int AccountId = Conversion.ToInt(r3.Cells["AccountId"].Value);
				int rowType = Conversion.ToInt(r3.Cells["RowType"].Value);
				bool Flag = false;
				if (rowType == 1 && AccountId == 0 && Conversion.ToDouble(r3.Cells["Credit"].Value) > 0.0)
				{
					((Control)(object)grdGLedger).Focus();
					if (rdTemplate2.Checked)
					{
						tabControl2.SelectedIndex = 0;
					}
					throw new Exception("Excess Weight Row in Supplier/Add Less Grid must have Account!");
				}
				if (rowType == 1)
				{
					Flag = true;
				}
				else if (AccountId > 0 && (Conversion.ToDouble(r3.Cells["Debit"].Value) > 0.0 || Conversion.ToDouble(r3.Cells["Credit"].Value) > 0.0))
				{
					Flag = true;
				}
				if (Flag)
				{
					InvPurchaseInvoiceJournal PJ = new InvPurchaseInvoiceJournal();
					PJ.ChartofAccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
					PJ.SupplierCustomerId = Conversion.ToInt(r3.Cells["AccountId"].Value);
					PJ.JvRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					if (PJ.JvRemarks == string.Empty)
					{
						PJ.JvRemarks = Conversion.ToString((object)r3.Cells["AccountId"].Text);
					}
					PJ.JvPrcnt = Conversion.ToDouble(r3.Cells["Percentage"].Value);
					PJ.JvQty = Conversion.ToDouble(r3.Cells["Qty"].Value);
					PJ.JvRate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					PJ.JvDebit = Conversion.ToDouble(r3.Cells["Debit"].Value);
					PJ.JvCredit = Conversion.ToDouble(r3.Cells["Credit"].Value);
					PJ.InvGrnId = Conversion.ToInt(r3.Cells["InvGrnId"].Value);
					PJ.FreightId = Conversion.ToInt(r3.Cells["FreightId"].Value);
					PJ.RowType = Conversion.ToInt(r3.Cells["RowType"].Value);
					purchaseInvoice.InvPurchaseInvoicejournalList.Add(PJ);
				}
			}
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) == 0)
				{
					continue;
				}
				InvPurchaseInvoiceEmptyBags PEB = new InvPurchaseInvoiceEmptyBags();
				PEB.PurchaseOrderId = Conversion.ToInt(r2.Cells["OrderId"].Value);
				PEB.TypeId = Conversion.ToInt(r2.Cells["TypeId"].Value);
				if (PEB.TypeId > 0)
				{
					if (PEB.TypeId == 1)
					{
						PEB.CreditAccountId = 0;
					}
					else if (PEB.TypeId == 2 || PEB.TypeId == 3)
					{
						PEB.CreditAccountId = Conversion.ToInt(r2.Cells["CreditAccountId"].Value);
						if (PEB.CreditAccountId <= 0 && !EmptyBagPurAgWtPropotionate)
						{
							MessageBox.Show("Credit Account Required in Empty Bag Grid in Row#" + (r2.RowIndex + 1), "Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							if (rdTemplate1.Checked)
							{
								((Control)(object)grdEmptyBags).Focus();
							}
							else if (rdTemplate2.Checked)
							{
								tabControl2.SelectedIndex = 3;
								((Control)(object)grdEmptyBags).Focus();
							}
							return;
						}
					}
				}
				PEB.InvGrnId = Conversion.ToInt(r2.Cells["InvGrnId"].Value);
				PEB.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				PEB.ItemConditionId = Conversion.ToInt(r2.Cells["ItemConditionId"].Value);
				PEB.ItemCondition = Conversion.ToString(r2.Cells["ItemCondition"].Value);
				if (PEB.ItemConditionId == 0)
				{
					MessageBox.Show("Item Condition Required in Empty Bag Grid in Row#" + (r2.RowIndex + 1), "Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				PEB.PurchaseQty = Conversion.ToDouble(r2.Cells["PurchaseQty"].Value);
				PEB.Rate = Conversion.ToDouble(r2.Cells["Rate"].Value);
				PEB.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
				if (PEB.TypeId == 1 && PEB.Amount == 0.0)
				{
					throw new Exception("Packing Material Amount Required");
				}
				PEB.CustomRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
				PEB.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
				if (PEB.PurchaseQty > 0.0 && PEB.Rate > 0.0)
				{
					PEB.Remarks = "ItemName : " + Conversion.ToString((object)r2.Cells["ItemId"].Text.Trim()) + "  " + $"Qty: {PEB.PurchaseQty} " + $"Rate: {PEB.Rate} " + PEB.Remarks;
				}
				purchaseInvoice.InvPurchaseInvoiceEmptyBagslist.Add(PEB);
			}
			decimal num = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentDetail, "Amount"));
			decimal paymentDetailAmount = default(decimal);
			List<GridEXRow> dataRows = grdPaymentDetail.GetDataRows().ToList();
			if (num > 0m)
			{
				if (dataRows.Any((GridEXRow r) => Conversion.ToDecimal(r.Cells["%OfTotal"].Value) > 0m || Conversion.ToDecimal(r.Cells["Amount"].Value) > 0m))
				{
					for (int i = 0; i < dataRows.Count; i++)
					{
						decimal num2 = Conversion.ToDecimal(dataRows[i].Cells["%OfTotal"].Value);
						decimal amount = Conversion.ToDecimal(dataRows[i].Cells["Amount"].Value);
						if (num2 <= 0m || amount <= 0m)
						{
							ShowPaymentDetailError($"Payment detail: All rows must have both % and Amount. Error at row# {i + 1}");
							return;
						}
					}
				}
				if ((from r in dataRows
					group r by Conversion.ToInt(r.Cells["PurchaseOrderId"].Value)).Any((IGrouping<int, GridEXRow> g) => g.Count() == 1))
				{
					if (PCalculateByAmount)
					{
						PCalculateByAmount = false;
						PCalculateByPercent = true;
					}
					PaymentAmountReCalculate();
				}
				foreach (GridEXRow item in dataRows)
				{
					int paymentTermId = Conversion.ToInt(item.Cells["PaymentTerm"].Value);
					int dueDays = Conversion.ToInt(item.Cells["DueDays"].Value);
					int rowNo = item.RowIndex + 1;
					if (paymentTermId <= 0)
					{
						ShowPaymentDetailError($"Payment Term Required in row# {rowNo}");
						return;
					}
					if (paymentTermId == 2 && dueDays <= 0)
					{
						ShowPaymentDetailError($"Due Days Required in case of Credit in row# {rowNo}");
						return;
					}
				}
				foreach (var order in from r in dtPaymentTerm.AsEnumerable()
					group r by Conversion.ToInt(r["PurchaseOrderId"]) into g
					select new
					{
						PurchaseOrderId = g.Key,
						PercentSum = Math.Round(g.Sum((DataRow r) => Conversion.ToDecimal(r["%OfTotal"])), 4),
						PurchaseOrderNo = Conversion.ToInt(g.First()["PurchaseOrderNo"])
					})
				{
					if (Math.Abs(100m - order.PercentSum) > 0.05m)
					{
						ShowPaymentDetailError($"Percent Of SaleOrder:{order.PurchaseOrderNo} not near to 100. Please Check!");
						return;
					}
				}
				foreach (GridEXRow row in dataRows)
				{
					InvPurchaseInvoicePaymentTermsDetail pd = new InvPurchaseInvoicePaymentTermsDetail
					{
						PurchaseOrderId = Conversion.ToInt(row.Cells["PurchaseOrderId"].Value),
						PurchaseOrderNo = Conversion.ToInt(row.Cells["PurchaseOrderNo"].Value),
						PaymentTermId = Conversion.ToInt(row.Cells["PaymentTerm"].Value),
						DueDays = Conversion.ToInt(row.Cells["DueDays"].Value),
						PrcntOfTotal = Conversion.ToDecimal(row.Cells["%OfTotal"].Value),
						Amount = Conversion.ToDecimal(row.Cells["Amount"].Value),
						DueDate = Conversion.ToDateTime(row.Cells["DueDate"].Value),
						PaymentRemarks = Conversion.ToString(row.Cells["Remarks"].Value),
						SystemGeneratedRow = false
					};
					paymentDetailAmount += pd.Amount;
					purchaseInvoice.InvPurchaseInvoicePaymentTermsDetailList.Add(pd);
				}
			}
			else
			{
				int dueDays2 = Math.Max(0, Conversion.ToInt((object)txtDueDays.Text));
				HashSet<int> seenOrders = new HashSet<int>();
				foreach (GridEXRow row2 in dataRows)
				{
					int poId = Conversion.ToInt(row2.Cells["PurchaseOrderId"].Value);
					if (seenOrders.Add(poId))
					{
						InvPurchaseInvoicePaymentTermsDetail pd2 = new InvPurchaseInvoicePaymentTermsDetail
						{
							PurchaseOrderId = poId,
							PurchaseOrderNo = Conversion.ToInt(row2.Cells["PurchaseOrderNo"].Value),
							PaymentTermId = Conversion.ToInt(row2.Cells["PaymentTerm"].Value),
							DueDays = dueDays2,
							DueDate = purchaseInvoice.DocDate.AddDays(dueDays2),
							PrcntOfTotal = 100m,
							Amount = Conversion.ToDecimal(row2.Cells["NetBillAmount"].Value),
							SystemGeneratedRow = true
						};
						paymentDetailAmount += pd2.Amount;
						purchaseInvoice.InvPurchaseInvoicePaymentTermsDetailList.Add(pd2);
					}
				}
			}
			if (Math.Abs(paymentDetailAmount - Conversion.ToDecimal((object)purchaseInvoice.BillAmount)) > 0.99m)
			{
				ShowPaymentDetailError($"Payment Detail Amount:{paymentDetailAmount:#,##0.####} Not Equal to Bill Amount:{purchaseInvoice.BillAmount:#,##0.####}");
				return;
			}
			purchaseInvoice.AttachmentsList = new List<DMSAttachments>();
			purchaseInvoice.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvPurchaseInvoice>(purchaseInvoice, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			purchaseInvoice.AttachmentsValues = ((AttachmentsValues != "") ? AttachmentsValues : purchaseInvoice.AttachmentsValues);
			purchaseInvoice.CustomAttachmentsValues = CustomAttachmentsValues;
			int sucess = InvPurchaseInvoice.Save(purchaseInvoice);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully [" + purchaseInvoice.DocNo + "] ");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, ScreenName);
			}
			else
			{
				MessageBox.Show("Record Saved Successfully [" + purchaseInvoice.DocNo + "] ");
			}
			Reset();
			if (ChkBoxParty.Checked)
			{
				RecId = sucess;
				CommonServices.PurchaseInvoicettSlip_220A(RecId);
				RecId = 0;
			}
			if (ChkBoxItemPrint.Checked)
			{
				RecId = sucess;
				GeneratereportSlip(RecId, "220-InvRptPurchaseBillSupplierRiceSlip.rpt");
				RecId = 0;
			}
			if (chkSummary.Checked)
			{
				RecId = sucess;
				GeneratereportSlip(RecId, "220B-InvRptPurchaseBillSupplierRiceSummary.rpt");
				RecId = 0;
			}
			if (ChkBokVoucher.Checked)
			{
				RecId = sucess;
				VoucherHeadIdGet();
				RecId = 0;
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ShowPaymentDetailError(string message)
	{
		MessageBox.Show(message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		tabControl3.SelectedIndex = 1;
		((Control)(object)grdPaymentDetail).Focus();
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			if (RecId == 0)
			{
				throw new Exception("Record Not Update because Record Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			RecId = ID;
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(RecId);
			if (invPI == null)
			{
				return;
			}
			List<InvPurchaseInvoiceDetail> invPurchaseInvoiceDetailList = invPI.invPurchaseInvoiceDetailList;
			if (invPurchaseInvoiceDetailList == null || invPurchaseInvoiceDetailList.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocno.Text = invPI.DocNo.ToString();
			DocDate.Value = invPI.DocDate;
			cmbsuppliername.Value = invPI.SupplierCustomerId;
			((Control)(object)cmbsuppliername).Enabled = false;
			cmbstockprty.Value = invPI.StockPartyId;
			txtbillno.Text = invPI.ManualBillNo;
			if (invPI.CommissionAgentId > 0)
			{
				cmbcommagent.Value = invPI.CommissionAgentId;
			}
			((Control)(object)cmbcommtype).Text = Conversion.ToString((object)invPI.CommissionType);
			txtcommrate.Text = invPI.CommRate.ToString();
			((Control)(object)cmbcommuom).Text = invPI.UomScheduleIdCmRate;
			txtcommamount.Text = invPI.CommAmount.ToString();
			txtcommremarks.Text = Conversion.ToString((object)invPI.CommissionRemarks);
			if (invPI.BrokerAgentId > 0)
			{
				CmbBrokeryAc.Value = invPI.BrokerAgentId;
			}
			((Control)(object)CmbBrokeryType).Text = Conversion.ToString((object)invPI.BrokeryType);
			txtBrokeryRate.Text = invPI.BrokeryRate.ToString();
			txtBrokeryAmount.Text = Conversion.ToString((object)invPI.BrokeryAmount);
			((Control)(object)CmbBrokeryRateUom).Text = invPI.BrokeryUom.ToString();
			txtWagesAmountHeader.Text = Conversion.ToString((object)invPI.WagesAmount);
			txtRemarks.Text = Conversion.ToString((object)invPI.OtherRemarks);
			txtRefParty.Text = Conversion.ToString((object)invPI.ReferencePartyName);
			txtDeliveryTerm.Text = Conversion.ToString((object)invPI.DeliveryTerm);
			if (Conversion.ToInt((object)invPI.PaymentTermsId) > 0)
			{
				CmbPaymentTerm.Value = Conversion.ToInt((object)invPI.PaymentTermsId);
			}
			if (Conversion.ToInt((object)invPI.TransportAccountId) > 0)
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					CmbFreightAccount.Value = Conversion.ToInt((object)invPI.TransporterCreditPartyId);
				}
				else
				{
					CmbFreightAccount.Value = Conversion.ToInt((object)invPI.TransportAccountId);
				}
			}
			txtFreightDeduction.Text = Conversion.ToDouble((object)invPI.FreightAmount).ToString();
			txtBillAmount.Text = invPI.BillAmount.ToString();
			DueDate.Value = invPI.DueDate;
			txtDueDays.Text = Conversion.ToString((object)invPI.DueDays);
			txtGrnNo.Text = Conversion.ToString((object)invPI.DocumentTypeSrNo);
			AttachmentsValues = invPI.AttachmentsValues;
			CustomAttachmentsValues = invPI.CustomAttachmentsValues;
			Approved = invPI.IsApproved;
			InvoiceTypeId = invPI.InvoiceTypeId;
			datSupplierInvoiceDate.Value = invPI.SupplierInvoiceDate;
			chkCustomAccounts.Checked = Conversion.ToBool((object)invPI.CustomAccounts);
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			FilldtDetailFromListCommonForReadById(invPI, dtGrid);
			string GrnIds = string.Join(",", (from row in dtGrid?.AsEnumerable()
				select row.Field<int>("InvGrnId")));
			POID = (from row in dtGrid.AsEnumerable()
				select row.Field<int>("PurchaseOrderId")).FirstOrDefault();
			POID = invPI.invPurchaseInvoiceDetailList[0].PurchaseOrderId;
			int LocationTypeId = invPI.invPurchaseInvoiceDetailList[0].LocationTypeId;
			if (LocationTypeId > 0)
			{
				cmbLocationType.Value = LocationTypeId;
			}
			DataTable WagesData = InvPurchaseInvoice.GetWagesAmountByRefDocumentTypeIdandRefIds(UserAccount.OrganizationId, UserAccount.CompanyId, 46, GrnIds);
			if (WagesData.Rows.Count > 0)
			{
				txtWagesAmountHeader.Text = Conversion.ToString(WagesData.Rows[0]["WagesAmount"]);
			}
			dtInvExp.Rows.Clear();
			for (int l = 0; l < invPI.InvPurchaseInvoiceExpList.Count; l++)
			{
				dtInvExp.Rows.Add(invPI.InvPurchaseInvoiceExpList[l].Id, invPI.InvPurchaseInvoiceExpList[l].GrnId, invPI.InvPurchaseInvoiceExpList[l].GrnNo, invPI.InvPurchaseInvoiceExpList[l].PurchaseOrderId, invPI.InvPurchaseInvoiceExpList[l].PurchaseOrderSupplierExpId, invPI.InvPurchaseInvoiceExpList[l].SupplierDispatchId, invPI.InvPurchaseInvoiceExpList[l].SupplierDispatchExpenseId, invPI.InvPurchaseInvoiceExpList[l].InvRevExpItemId, invPI.InvPurchaseInvoiceExpList[l].Qty, invPI.InvPurchaseInvoiceExpList[l].Rate, invPI.InvPurchaseInvoiceExpList[l].Amount, invPI.InvPurchaseInvoiceExpList[l].CustomRemarks);
			}
			dtGrdGL.Rows.Clear();
			for (int k = 0; k < invPI.InvPurchaseInvoicejournalList.Count; k++)
			{
				dtGrdGL.Rows.Add(invPI.InvPurchaseInvoicejournalList[k].InvGrnId, invPI.InvPurchaseInvoicejournalList[k].FreightId, invPI.InvPurchaseInvoicejournalList[k].SupplierCustomerId, invPI.InvPurchaseInvoicejournalList[k].SupplierCustomerId, invPI.InvPurchaseInvoicejournalList[k].JvRemarks, invPI.InvPurchaseInvoicejournalList[k].JvPrcnt, invPI.InvPurchaseInvoicejournalList[k].JvQty, invPI.InvPurchaseInvoicejournalList[k].JvRate, invPI.InvPurchaseInvoicejournalList[k].JvDebit, invPI.InvPurchaseInvoicejournalList[k].JvCredit, invPI.InvPurchaseInvoicejournalList[k].JvCredit, invPI.InvPurchaseInvoicejournalList[k].ChartofAccountId, invPI.InvPurchaseInvoicejournalList[k].JvQty, invPI.InvPurchaseInvoicejournalList[k].JvRate, invPI.InvPurchaseInvoicejournalList[k].RowType);
			}
			dtFreight.Rows.Clear();
			for (int j = 0; j < invPI.InvPurchaseInvoiceFreightList.Count; j++)
			{
				dtFreight.Rows.Add(invPI.InvPurchaseInvoiceFreightList[j].PurchaseOrderId, invPI.InvPurchaseInvoiceFreightList[j].InvGrnId, invPI.InvPurchaseInvoiceFreightList[j].FreightId, invPI.InvPurchaseInvoiceFreightList[j].SupplierCustomerId, invPI.InvPurchaseInvoiceFreightList[j].SupplierCustomerId, invPI.InvPurchaseInvoiceFreightList[j].Percentage, invPI.InvPurchaseInvoiceFreightList[j].FrQty, invPI.InvPurchaseInvoiceFreightList[j].FrRate, Math.Round(invPI.InvPurchaseInvoiceFreightList[j].FreightAmount), invPI.InvPurchaseInvoiceFreightList[j].Debit, invPI.InvPurchaseInvoiceFreightList[j].FreightAmount, invPI.InvPurchaseInvoiceFreightList[j].Remarks, invPI.InvPurchaseInvoiceFreightList[j].TansporterId);
			}
			dtEmptyBags.Rows.Clear();
			foreach (InvPurchaseInvoiceEmptyBags bag in invPI.InvPurchaseInvoiceEmptyBagslist)
			{
				DataRow newRow = dtEmptyBags.NewRow();
				newRow["OrderId"] = bag.PurchaseOrderId;
				newRow["TypeId"] = bag.TypeId;
				newRow["Type"] = bag.EmptyBagsType;
				newRow["InvGrnId"] = bag.InvGrnId;
				newRow["ItemId"] = bag.ItemId;
				newRow["ItemName"] = bag.ItemName;
				newRow["ItemConditionId"] = bag.ItemConditionId;
				newRow["ItemCondition"] = bag.ItemCondition;
				newRow["PurchaseQty"] = bag.PurchaseQty;
				newRow["Rate"] = bag.Rate;
				newRow["Amount"] = bag.Amount;
				newRow["Remarks"] = bag.CustomRemarks;
				newRow["CreditAccountId"] = bag.CreditAccountId;
				dtEmptyBags.Rows.Add(newRow);
			}
			dtPaymentTerm.Rows.Clear();
			bool SystemGenerated = false;
			SystemGenerated = invPI.InvPurchaseInvoicePaymentTermsDetailList.Count == 0 || invPI.InvPurchaseInvoicePaymentTermsDetailList.Any((InvPurchaseInvoicePaymentTermsDetail r) => r.SystemGeneratedRow);
			if (Conversion.ToBool((object)SystemGenerated))
			{
				var list = (from GridEXRow row in grd.GetRows()
					select new
					{
						OrderId = Conversion.ToInt(row.Cells["PurchaseOrderId"].Value),
						OrderNo = Conversion.ToInt(row.Cells["PurchaseOrder"].Value),
						PaymentTermsId = Conversion.ToInt(CmbPaymentTerm.Value)
					} into x
					where x.OrderId > 0
					select x).Distinct().ToList();
				if (!list.Any())
				{
					AddPaymentRow(0, 0, Conversion.ToInt(CmbPaymentTerm.Value), 0, DateTime.Now);
					grdPaymentDetail.DataSource = dtPaymentTerm;
					grdPaymentDetail.RetrieveStructure();
					gridPaymentTermSetting();
				}
				HashSet<int> existingOrderIds = (from GridEXRow r in grdPaymentDetail.GetRows()
					select Conversion.ToInt(r.Cells["PurchaseOrderId"].Value)).ToHashSet();
				foreach (var order in list)
				{
					if (!existingOrderIds.Contains(order.OrderId))
					{
						AddPaymentRow(order.OrderId, order.OrderNo, order.PaymentTermsId, 0, DateTime.Now);
					}
				}
			}
			else
			{
				for (int i = 0; i < invPI.InvPurchaseInvoicePaymentTermsDetailList.Count; i++)
				{
					dtPaymentTerm.Rows.Add(invPI.InvPurchaseInvoicePaymentTermsDetailList[i].PurchaseOrderId, invPI.InvPurchaseInvoicePaymentTermsDetailList[i].PurchaseOrderNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, invPI.InvPurchaseInvoicePaymentTermsDetailList[i].PaymentTermId, invPI.InvPurchaseInvoicePaymentTermsDetailList[i].DueDays, invPI.InvPurchaseInvoicePaymentTermsDetailList[i].DueDate, invPI.InvPurchaseInvoicePaymentTermsDetailList[i].PrcntOfTotal, invPI.InvPurchaseInvoicePaymentTermsDetailList[i].Amount, invPI.InvPurchaseInvoicePaymentTermsDetailList[i].PaymentRemarks);
				}
			}
			CalculateOrderWiseAmountForPaymentDetail();
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
			FreightProportion();
			FreightDeductionProportion();
			WagesAmountProportion();
			ExpProportion();
			LedgerProportion();
			CommissionProportion();
			TotalBrokeryAmount();
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = DocumentTypeId,
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FreightDeductionProportion()
	{
		try
		{
			double NetWeight = ((grd.RowCount > 0) ? Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString()) : 0.0);
			double FreightDeduction = Conversion.ToDouble((object)txtFreightDeduction.Text);
			FreightDeduction = Math.Round(FreightDeduction, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			if (FreightDeduction > 0.0 && NetWeight > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble(obj.Cells["NetBillWeight"].Value);
					obj.Cells["FreightDeduction"].Value = ((NetWeight > 0.0) ? (FreightDeduction / NetWeight * Weight) : 0.0);
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FreightDeduction"].Value = 0;
				}
			}
			BillProportion();
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
			double TotalAmount = GridEX_Helper.GetColumnSum(grdInvExp, "Amount");
			double ItemQty = GridEX_Helper.GetColumnSum(grd, "ItemQty");
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Qty = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
					double Exp = TotalAmount / ItemQty * Qty;
					obj.Cells["Expense"].Value = Exp;
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
			if (!FreightDebitToExpenses)
			{
				GridEXRow[] allRows = grd.GetRows();
				IEnumerable<IGrouping<int, GridEXRow>> rowsByGrn = ((grd.RowCount > 0) ? (from r in allRows
					where r.Cells["InvGrnId"].Value != null
					group r by Conversion.ToInt(r.Cells["InvGrnId"].Value)) : null);
				if (rowsByGrn != null)
				{
					foreach (IGrouping<int, GridEXRow> grnGroup in rowsByGrn)
					{
						int grnId = grnGroup.Key;
						double totalWeight = grnGroup.Sum((GridEXRow r) => Conversion.ToDouble(r.Cells["NetBillWeight"].Value));
						double totalFreight = (from f in grdFreight.GetRows()
							where Conversion.ToInt(f.Cells["InvGrnId"].Value) == grnId
							select f).Sum((GridEXRow f) => Conversion.ToDouble(f.Cells["Freight"].Value));
						totalFreight = Math.Round(totalFreight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						foreach (GridEXRow r3 in grnGroup)
						{
							r3.BeginEdit();
							double weight2 = Conversion.ToDouble(r3.Cells["NetBillWeight"].Value);
							if (totalFreight > 0.0 && totalWeight > 0.0)
							{
								r3.Cells["Freights"].Value = totalFreight / totalWeight * weight2;
							}
							else
							{
								r3.Cells["Freights"].Value = 0;
							}
						}
					}
				}
				double freightForAll = (from f in grdFreight.GetRows()
					where Conversion.ToInt(f.Cells["InvGrnId"].Value) == 0
					select f).Sum((GridEXRow f) => Conversion.ToDouble(f.Cells["Freight"].Value));
				freightForAll = Math.Round(freightForAll, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (freightForAll > 0.0)
				{
					double totalWeightAll = allRows.Sum((GridEXRow r) => Conversion.ToDouble(r.Cells["NetBillWeight"].Value));
					GridEXRow[] array = allRows;
					foreach (GridEXRow r2 in array)
					{
						r2.BeginEdit();
						double weight = Conversion.ToDouble(r2.Cells["NetBillWeight"].Value);
						double additionalFreight = ((totalWeightAll > 0.0) ? (freightForAll / totalWeightAll * weight) : 0.0);
						r2.Cells["Freights"].Value = Conversion.ToDouble(r2.Cells["Freights"].Value) + additionalFreight;
						r2.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] array = grd.GetRows();
				foreach (GridEXRow obj in array)
				{
					obj.BeginEdit();
					obj.Cells["Freights"].Value = 0;
					obj.EndEdit();
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Freight Distribution Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void WagesAmountProportion()
	{
		try
		{
			double WagesAmount = Conversion.ToDouble((object)txtWagesAmountHeader.Text);
			if (WagesAmount > 0.0)
			{
				double ItemQty = GridEX_Helper.GetColumnSum(grd, "ItemQty");
				double NetWeight = GridEX_Helper.GetColumnSum(grd, "NetBillWeight");
				if (WagesAmountCalculateOnQty)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double qty = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
						obj.Cells["WagesAmount"].Value = WagesAmount / ItemQty * qty;
						obj.EndEdit();
					}
				}
				else
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						double Weight = Conversion.ToDouble((object)obj2.Cells["NetBillWeight"].Value.ToString());
						obj2.Cells["WagesAmount"].Value = WagesAmount / NetWeight * Weight;
						obj2.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj3 in rows)
				{
					obj3.BeginEdit();
					obj3.Cells["WagesAmount"].Value = 0;
					obj3.EndEdit();
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
				double ItemAmount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
				double Expense = Conversion.ToDouble(r.Cells["Expense"].Value);
				double Freights = Conversion.ToDouble(r.Cells["Freights"].Value);
				double Journal = Conversion.ToDouble(r.Cells["Journal"].Value);
				double Commission = Conversion.ToDouble(r.Cells["Commission"].Value);
				double WagesAmount = Conversion.ToDouble(r.Cells["WagesAmount"].Value);
				double FreightDeduction = Conversion.ToDouble(r.Cells["FreightDeduction"].Value);
				double EbPurAgainstWeightAmount = Conversion.ToDouble(r.Cells["EbPurAgainstWeightAmount"].Value);
				if (ContractWagesChargetoProduct)
				{
					r.BeginEdit();
					r.Cells["BillAmount"].Value = Math.Round(ItemAmount + Expense + Freights - FreightDeduction + Journal + Commission + WagesAmount - EbPurAgainstWeightAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					r.EndEdit();
				}
				else
				{
					r.BeginEdit();
					r.Cells["BillAmount"].Value = Math.Round(ItemAmount + Expense + Freights - FreightDeduction + Journal + Commission - EbPurAgainstWeightAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					r.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionProportion()
	{
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			Conversion.ToDecimal((object)txtcommrate.Text);
			decimal NetWeight = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "NetBillWeight"));
			decimal TotalItemAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemAmount"));
			if (TotalAmount > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (((Control)(object)cmbcommtype).Text == "Percent" || ((Control)(object)cmbcommtype).Text == "Flat")
					{
						decimal ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
						r.Cells["Commission"].Value = TotalAmount / TotalItemAmount * ItemAmount;
						r.EndEdit();
					}
					else
					{
						decimal Weight = Conversion.ToDecimal(r.Cells["NetBillWeight"].Value);
						r.Cells["Commission"].Value = TotalAmount / NetWeight * Weight;
						r.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Commission"].Value = 0;
					obj.EndEdit();
				}
			}
			BillProportion();
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
			double ItemAmount = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
			double ExpAmount = GridEX_Helper.GetColumnSum(grdInvExp, "Amount");
			double BillAmount = 0.0;
			double EmptyBagsAmount = 0.0;
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			double TransporterDebit = 0.0;
			double TransporterCredit = 0.0;
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt(cmbsuppliername.Value) > 0 && Conversion.ToInt((object)r3.Cells["AccountId"].Value.ToString()) > 0)
				{
					JournalDebit += Conversion.ToDouble((object)r3.Cells["Debit"].Value.ToString());
					JournalCredit += Conversion.ToDouble((object)r3.Cells["Credit"].Value.ToString());
				}
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["Transporter"].Value) <= 0)
				{
					continue;
				}
				if (SubsidiaryAccountAllownOnVouchers)
				{
					if (cmbsuppliername.Value.ToString() == r2.Cells["Transporter"].Value.ToString())
					{
						TransporterDebit += Conversion.ToDouble((object)r2.Cells["Debit"].Value.ToString());
						TransporterCredit += Conversion.ToDouble((object)r2.Cells["Freight"].Value.ToString());
					}
				}
				else if (txtSupplierGLId.Text.ToString() == r2.Cells["Transporter"].Value.ToString())
				{
					TransporterDebit += Conversion.ToDouble((object)r2.Cells["Debit"].Value.ToString());
					TransporterCredit += Conversion.ToDouble((object)r2.Cells["Freight"].Value.ToString());
				}
			}
			if (grdEmptyBags.GetRows().Count() > 0)
			{
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r.Cells["TypeId"].Value) == 1)
					{
						EmptyBagsAmount += Conversion.ToDouble((object)r.Cells["Amount"].Value.ToString());
					}
				}
			}
			BillAmount = ItemAmount + ExpAmount + EmptyBagsAmount;
			BillAmount = BillAmount + JournalDebit - JournalCredit;
			BillAmount = BillAmount + TransporterCredit - TransporterDebit;
			if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(cmbcommagent.Value))
			{
				if (txtcommrate.Text != string.Empty)
				{
					BillAmount += Conversion.ToDouble((object)txtcommamount.Text.Trim());
				}
				else
				{
					txtcommamount.Text = "0";
				}
			}
			if (((UltraGridBase)CmbBrokeryAc).ActiveRow != null)
			{
				if (Conversion.ToDouble((object)txtBrokeryAmount.Text) > 0.0)
				{
					BillAmount -= Conversion.ToDouble((object)txtBrokeryAmount.Text.Trim());
				}
				else
				{
					txtBrokeryAmount.Text = "0";
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			double FreightDeduction = Conversion.ToDouble((object)txtFreightDeduction.Text);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(CmbFreightAccount.Value))
				{
					BillAmount -= FreightDeduction;
				}
			}
			else if (Conversion.ToString((object)txtSupplierGLId.Text) == Conversion.ToString(CmbFreightAccount.Value))
			{
				BillAmount -= FreightDeduction;
			}
			txtBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWagesAmountHeader_TextChanged(object sender, EventArgs e)
	{
		try
		{
			WagesAmountProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcommagent_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void cmbcommtype_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void cmbcommuom_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void CmbBrokeryType_TextChanged(object sender, EventArgs e)
	{
		TotalBrokeryAmount();
	}

	private void txtBrokeryRate_TextChanged(object sender, EventArgs e)
	{
		TotalBrokeryAmount();
	}

	private void CmbBrokeryRateUom_TextChanged(object sender, EventArgs e)
	{
		TotalBrokeryAmount();
	}

	private void CmbBrokeryAc_Leave(object sender, EventArgs e)
	{
		TotalBrokeryAmount();
	}

	private void cmbsuppliername_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtSupplier.Rows.Count > 0 && Conversion.ToInt(cmbsuppliername.Value) > 0)
			{
				DataRow[] dr = dtSupplier.Select("Id = " + Conversion.ToInt(cmbsuppliername.Value));
				txtSupplierGLId.Text = Conversion.ToString(dr[0]["GlAccountId"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RateCutAmountCalculations()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				if (Conversion.ToDouble(item.Cells["RateCut"].Value) != 0.0)
				{
					string RateCut = item.Cells["RateCut"].Value.ToString();
					string NetBillWeight = item.Cells["NetBillWeight"].Value.ToString();
					string EquivalentPoRate = item.Cells["EquivalentPoRate"].Text.ToString();
					double RateCutAmount = Conversion.ToDouble((object)NetBillWeight) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)RateCut);
					item.Cells["RateCutAmount"].Value = RateCutAmount;
					string ItemRate = item.Cells["OrderItemRate"].Value.ToString();
					double ItemAmount = Conversion.ToDouble((object)NetBillWeight) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)ItemRate);
					item.Cells["ItemAmount"].Value = ItemAmount - RateCutAmount;
				}
				item.EndEdit();
			}
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			double CommRate = 0.0;
			if (!string.IsNullOrWhiteSpace(txtcommrate.Text))
			{
				CommRate = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				string commType = ((Control)(object)cmbcommtype).Text;
				double CommAmt = 0.0;
				switch (commType)
				{
				case "Flat":
					CommAmt = CommRate;
					break;
				case "Percent":
				{
					double total = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
					if (total != 0.0)
					{
						CommAmt = Conversion.ToDouble((object)total) * CommRate / 100.0;
					}
					break;
				}
				case "Comm Weight":
				{
					double weightVal = GridEX_Helper.GetColumnSum(grd, "NetBillWeight");
					if (weightVal != 0.0)
					{
						double num = Conversion.ToDouble((object)weightVal);
						double UOM = Conversion.ToDouble((object)((Control)(object)cmbcommuom).Text.Trim());
						CommAmt = num / UOM * CommRate;
					}
					break;
				}
				}
				txtcommamount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtcommamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalBrokeryAmount()
	{
		try
		{
			if (string.IsNullOrWhiteSpace(txtBrokeryRate.Text))
			{
				txtBrokeryAmount.Text = "0";
				BillAmount();
				return;
			}
			if (!double.TryParse(txtBrokeryRate.Text.Trim(), out var commRate))
			{
				txtBrokeryAmount.Text = "0";
				BillAmount();
				return;
			}
			string brokeryType = ((Control)(object)CmbBrokeryType).Text;
			double commAmt = 0.0;
			switch (brokeryType)
			{
			case "Flat":
				commAmt = commRate;
				break;
			case "Percent":
			{
				double total = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
				if (total != 0.0)
				{
					commAmt = Conversion.ToDouble((object)total) * commRate / 100.0;
				}
				break;
			}
			case "Comm Weight":
			{
				double weightVal = GridEX_Helper.GetColumnSum(grd, "NetBillWeight");
				if (weightVal != 0.0)
				{
					double num = Conversion.ToDouble((object)weightVal);
					if (!double.TryParse(((Control)(object)CmbBrokeryRateUom).Text.Trim(), out var uom) || uom == 0.0)
					{
						uom = 1.0;
					}
					commAmt = num / uom * commRate;
				}
				break;
			}
			}
			txtBrokeryAmount.Text = Math.Round(commAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcommtype_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		CommissionProportion();
		BillAmount();
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		CommissionProportion();
		BillAmount();
	}

	private void cmbcommuom_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		CommissionProportion();
		BillAmount();
	}

	private void txtFreightDeduction_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FreightDeductionProportion();
			BillAmount();
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

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			grdHistory.ClearStructure();
			grdDetail.ClearStructure();
			drdocdate.Checked = true;
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
			HistoryCombosBranchDbCall();
			HistoryCombosBranchBind();
			HistoryComboBind(HistoryComboDbCall());
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
			HistoryComboBind(HistoryComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
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

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
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
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
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
				DataTable dthistory = InvPurchaseInvoice.FormHistory(obj);
				if (dthistory.Rows.Count > 0)
				{
					if (dtGridHistory.Columns.Count == 0)
					{
						dtGridHistory.Columns.Add("Id", typeof(int));
						dtGridHistory.Columns.Add("VoucherHeadId", typeof(int));
						dtGridHistory.Columns.Add("DocumentTypeId", typeof(int));
						dtGridHistory.Columns.Add("BranchName", typeof(string));
						dtGridHistory.Columns.Add("BranchSrNo", typeof(int));
						dtGridHistory.Columns.Add("OrderNo", typeof(int));
						dtGridHistory.Columns.Add("DocNo", typeof(int));
						dtGridHistory.Columns.Add("DocDate", typeof(DateTime));
						dtGridHistory.Columns.Add("PurchaseAgainst", typeof(string));
						dtGridHistory.Columns.Add("DueDays", typeof(int));
						dtGridHistory.Columns.Add("DueDate", typeof(DateTime));
						dtGridHistory.Columns.Add("ManualBillNo", typeof(string));
						dtGridHistory.Columns.Add("SupplierName", typeof(string));
						dtGridHistory.Columns.Add("RefParty", typeof(string));
						dtGridHistory.Columns.Add("CommAgent", typeof(string));
						dtGridHistory.Columns.Add("CommType", typeof(string));
						dtGridHistory.Columns.Add("CommRate", typeof(double));
						dtGridHistory.Columns.Add("CommAmount", typeof(double));
						dtGridHistory.Columns.Add("CommRemarks", typeof(string));
						dtGridHistory.Columns.Add("BillAmount", typeof(double));
						dtGridHistory.Columns.Add("VehicleNos", typeof(string));
						dtGridHistory.Columns.Add("ApprovedStatus", typeof(string));
						dtGridHistory.Columns.Add("EntryUser", typeof(string));
						dtGridHistory.Columns.Add("EntryDate", typeof(DateTime));
						dtGridHistory.Columns.Add("ModifyUser", typeof(string));
						dtGridHistory.Columns.Add("ModifyDate", typeof(DateTime));
						dtGridHistory.Columns.Add("ApprovedUser", typeof(string));
						dtGridHistory.Columns.Add("ApprovedDate", typeof(DateTime));
						dtGridHistory.Columns.Add("NoOfAttachments", typeof(int));
						dtGridHistory.Columns.Add("Remarks", typeof(string));
					}
					dtGridHistory.Clear();
					for (int i = 0; i < dthistory.Rows.Count; i++)
					{
						dtGridHistory.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocumentTypeId"], dthistory.Rows[i]["BranchName"], dthistory.Rows[i]["BranchSrNo"], dthistory.Rows[i]["OrderNo"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["PurchaseAgainst"], dthistory.Rows[i]["DueDays"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["SupplierName"], dthistory.Rows[i]["ReferencePartyName"], dthistory.Rows[i]["CommissionAgent"], dthistory.Rows[i]["CommissionType"], dthistory.Rows[i]["CommRate"], dthistory.Rows[i]["CommAmount"], dthistory.Rows[i]["CommissionRemarks"], dthistory.Rows[i]["BillAmount"], dthistory.Rows[i]["VehicleNos"], dthistory.Rows[i]["ApprovedStatus"], dthistory.Rows[i]["EntryUser"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUser"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["ApprovedUser"], dthistory.Rows[i]["PostDate"], dthistory.Rows[i]["NoOfAttachments"], dthistory.Rows[i]["RemarksHeader"]);
					}
					grdHistory.DataSource = dtGridHistory;
					grdHistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					dtGridHistory.Clear();
					grdHistory.DataSource = dtGridHistory;
					dtGridDetailHistory.Clear();
					grdDetail.DataSource = dtGridDetailHistory;
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
			List<string> hiddenCols = new List<string> { "Id", "VoucherHeadId", "DocumentTypeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 2, 2, hiddenCols);
			grdHistory.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["BranchSrNo"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["PurchaseAgainst"].Width = 100;
			grdHistory.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["RefParty"].Width = Constants.InventoryConstants.ReferenceParty;
			grdHistory.RootTable.Columns["CommAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			grdHistory.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.DiscountType;
			grdHistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["CommRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["ApprovedStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["VehicleNos"].Width = 125;
			GridEX_Helper.AddButton(grdHistory, "View", "Item Slip", 65, 0);
			GridEX_Helper.AddButton(grdHistory, "PartySlip", "PartySlip", 65, 1);
			GridEX_Helper.AddButton(grdHistory, "Voucher", "Voucher", 60, 2);
			GridEX_Helper.AddButton(grdHistory, "SummaryReport", "SummaryReport", 90, 3);
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 40, 4);
			GridEX_Helper.AddButton(grdHistory, "AddAttachment", "Add Attachment", 120);
			grdHistory.FrozenColumns = 5;
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
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					RecId = CurrId;
					ReadById(RecId);
				}
				else if (e.Column.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Convert.ToInt32(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), DocumentTypeId);
				}
				else if (e.Column.Key == "View")
				{
					GeneratereportSlip(CurrId, "220-InvRptPurchaseBillSupplierRiceSlip.rpt");
				}
				else if (e.Column.Key == "PartySlip")
				{
					CommonServices.PurchaseInvoicettSlip_220A(CurrId);
				}
				else if (e.Column.Key == "SummaryReport")
				{
					GeneratereportSlip(CurrId, "220B-InvRptPurchaseBillSupplierRiceSummary.rpt");
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, CurrId, DocumentTypeId, base.Name);
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

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "GrnNo")
			{
				InvFrmGRN obj = new InvFrmGRN(UserAccount);
				if (clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					if (!bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "InvFrmGRN").ToList()[0].Value.ToString()))
					{
						MessageBox.Show("Please Check Screen Rights");
						return;
					}
					obj.Show();
					obj.ReadById(Conversion.ToInt(grdHistory.CurrentRow.Cells[3].Value));
				}
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, DocumentTypeId);
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
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				DetailGridBind(CurrId);
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind(int CurrId)
	{
		try
		{
			InvPurchaseInvoice invGrn = InvPurchaseInvoice.GetByID(CurrId);
			if (invGrn == null)
			{
				dtGridDetailHistory.Clear();
				grdDetail.DataSource = dtGridDetailHistory;
				return;
			}
			if (dtGridDetailHistory.Columns.Count == 0)
			{
				dtGridDetailHistory = dtGrid.Clone();
				dtGridDetailHistory.Columns.Add("JobLot");
				dtGridDetailHistory.Columns.Add("PackingType");
			}
			dtGridDetailHistory.Clear();
			FilldtDetailFromListCommonForReadById(invGrn, dtGridDetailHistory, AddExtraColumns: true);
			grdDetail.DataSource = dtGridDetailHistory;
			grdDetail.RetrieveStructure();
			grdHistoryDetailSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryDetailSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"ItemId", "ItemUomId", "WarehouseId", "PackingTypeId", "PurchaseOrderId", "OrderItemRateUOMId", "JobLotId", "InvGrnId", "Id", "InvGrnDetailId",
				"Journal", "EquivalentPoRate", "EbPurAgainstWeightAmount", "BranchId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3, hiddenColumns);
			grdCommonSetting(grdDetail);
			grdDetail.RootTable.Columns["JobLot"].Position = grdDetail.RootTable.Columns["JobLotId"].Position + 1;
			grdDetail.RootTable.Columns["PackingType"].Position = grdDetail.RootTable.Columns["PackingTypeId"].Position + 1;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
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
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnFrmRefresh_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnRefreshHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						btnSave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnshow_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnUpdate_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.F5)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					DocDate.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					cmbBranchName.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F10 && tabControl1.SelectedIndex == 0)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
			{
				btnSlipDetail_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formright.DoHavePrintRights)
			{
				btnSlip220A_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3) && formright.DoHavePrintRights)
			{
				btn220bSummary_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4) && formright.DoHavePrintRights)
			{
				btnPrint_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5) && formright.DoHavePrintRights)
			{
				btn104Voucher_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L && tabControl1.SelectedIndex == 0)
			{
				BtnLoader_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
			if (e.Control && e.KeyCode == Keys.Right && tabControl1.SelectedIndex == 0)
			{
				if (rdTemplate1.Checked)
				{
					if (Conversion.ToString(base.ActiveControl.Tag) == "GrdFreight")
					{
						((Control)(object)grdInvExp).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "GrdExpense")
					{
						((Control)(object)grdGLedger).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "GrdPartyAddLess")
					{
						((Control)(object)grdEmptyBags).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "GrdEmptyBags")
					{
						((Control)(object)grd).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "GrdDetail")
					{
						((Control)(object)grdFreight).Focus();
					}
				}
				if (rdTemplate2.Checked)
				{
					if (Conversion.ToString(base.ActiveControl.Tag) == "GrdPartyAddLess")
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdFreight).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "GrdFreight")
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdInvExp).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "GrdExpense")
					{
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdEmptyBags).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "GrdEmptyBags")
					{
						((Control)(object)grd).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "GrdDetail")
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdGLedger).Focus();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					txtRefParty.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateHistory.Focus();
				}
			}
			if (e.Control && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
			{
				if (rdTemplate1.Checked)
				{
					rdTemplate2.Checked = true;
				}
				else
				{
					rdTemplate1.Checked = true;
				}
				if (rdTemplate1.Checked)
				{
					ExpenseGridsInTabOrinFront = false;
				}
				else
				{
					ExpenseGridsInTabOrinFront = true;
				}
				ChangeTemplate(ExpenseGridsInTabOrinFront);
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 0 && btnDelete.Visible && btnDelete.Enabled)
			{
				btnDelete_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "GrdFreight")
				{
					GridEXRow item3 = grdFreight.CurrentRow;
					if (item3 != null && (int)item3.RowType == 0)
					{
						if (Conversion.ToInt(item3.Cells["InvGrnId"].Value) > 0)
						{
							throw new Exception("Record cannot be deleted because this record against grn");
						}
						dtFreight.Rows[item3.RowIndex].Delete();
						item3.Delete();
					}
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
					grdFreight.UpdateData();
					BillAmount();
					FreightProportion();
				}
				if (Conversion.ToString(base.ActiveControl.Tag) == "GrdExpense")
				{
					GridEXRow item2 = grdInvExp.CurrentRow;
					dtInvExp.Rows[item2.RowIndex].Delete();
					if (item2 != null && (int)item2.RowType == 0)
					{
						item2.Delete();
					}
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
					grdInvExp.UpdateData();
					BillAmount();
					ExpProportion();
				}
				if (Conversion.ToString(base.ActiveControl.Tag) == "GrdPartyAddLess")
				{
					GridEXRow item = grdGLedger.CurrentRow;
					if (item != null && (int)item.RowType == 0)
					{
						if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0)
						{
							throw new Exception("Record cannot be deleted because this record against grn");
						}
						dtGrdGL.Rows[item.RowIndex].Delete();
						item.Delete();
					}
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
					grdGLedger.UpdateData();
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "GrdFreight")
				{
					AddRowInFreightGrid();
				}
				if (Conversion.ToString(base.ActiveControl.Tag) == "GrdExpense")
				{
					AddRowInvExpGrid();
				}
				if (Conversion.ToString(base.ActiveControl.Tag) == "GrdPartyAddLess")
				{
					AddRowInGLGrid();
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
			dt.Rows.Add("Ctrl+S", "Save (Form) Or Show Record (History)");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+T", "For Transfer Tab");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date(Form) Or Branch Name (History)");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+L", "For Load Records Of GRN");
			dt.Rows.Add("ALT+1", "For 220 Slip");
			dt.Rows.Add("ALT+2", "For 220A Slip");
			dt.Rows.Add("ALT+3", "For 220B Slip");
			dt.Rows.Add("ALT+4", "For 103 Slip");
			dt.Rows.Add("ALT+5", "For 104 Slip");
			dt.Rows.Add("Ctrl+2", "Template Change");
			dt.Rows.Add("Ctrl+Delete", "Freight,Expense,PartyAddLess Grids delete row ");
			dt.Rows.Add("Ctrl+D", "Freight,Expense,PartyAddLess Grids Add new row ");
			dt.Rows.Add("Ctrl+Right", "To Toggle between form grids");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On detail grid (Form) Or Main Grid (History)");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date From Date (history)");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete Record in update case");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFreight.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Transporter" && e.KeyCode == Keys.F1)
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["Transporter"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlst, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["Transporter"].Value = obj2.ReturnId;
				}
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
		try
		{
			GridEXRow r = grdInvExp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "ItemId" && e.KeyCode == Keys.F1)
			{
				GrdPopUp obj = new GrdPopUp(dtOtherItem, "Id", "OtherItemName");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				r.Cells["ItemId"].Value = obj.ReturnId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdGLedger_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGLedger.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdGLedger.CurrentColumn == null || !(grdGLedger.CurrentColumn.Key == "AccountId"))
			{
				return;
			}
			if (e.KeyCode == Keys.F1)
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["AccountId"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlst, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["AccountId"].Value = obj2.ReturnId;
				}
			}
			else if (e.KeyCode == Keys.Up)
			{
				int rowIndex = -1;
				if (r.RowIndex != 0)
				{
					rowIndex = r.RowIndex - 1;
				}
				if (Conversion.ToInt(grdGLedger.GetRow(rowIndex).Cells["RowType"].Value) == 1)
				{
					grdGLedger.RootTable.Columns["Percentage"].EditType = (EditType)0;
					grdGLedger.RootTable.Columns["Qty"].EditType = (EditType)0;
					grdGLedger.RootTable.Columns["Debit"].EditType = (EditType)0;
					grdGLedger.RootTable.Columns["Credit"].EditType = (EditType)0;
				}
				else
				{
					grdGLedger.RootTable.Columns["Percentage"].EditType = (EditType)1;
					grdGLedger.RootTable.Columns["Qty"].EditType = (EditType)1;
					grdGLedger.RootTable.Columns["Debit"].EditType = (EditType)1;
					grdGLedger.RootTable.Columns["Credit"].EditType = (EditType)1;
				}
				grdGLedger.UpdateData();
			}
			else if (e.KeyCode == Keys.Down)
			{
				int rowIndex2 = -1;
				if (r.RowIndex != grdGLedger.GetRows().Count() - 1)
				{
					rowIndex2 = r.RowIndex + 1;
				}
				if (Conversion.ToInt(grdGLedger.GetRow(rowIndex2).Cells["RowType"].Value) == 1)
				{
					grdGLedger.RootTable.Columns["Percentage"].EditType = (EditType)0;
					grdGLedger.RootTable.Columns["Qty"].EditType = (EditType)0;
					grdGLedger.RootTable.Columns["Debit"].EditType = (EditType)0;
					grdGLedger.RootTable.Columns["Credit"].EditType = (EditType)0;
				}
				else
				{
					grdGLedger.RootTable.Columns["Percentage"].EditType = (EditType)1;
					grdGLedger.RootTable.Columns["Qty"].EditType = (EditType)1;
					grdGLedger.RootTable.Columns["Debit"].EditType = (EditType)1;
					grdGLedger.RootTable.Columns["Credit"].EditType = (EditType)1;
				}
				grdGLedger.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
			int CurrId = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space && grdHistory.CurrentColumn != null)
			{
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					Reset();
					RecId = CurrId;
					ReadById(RecId);
				}
				if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Convert.ToInt32(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), DocumentTypeId);
				}
				if (grdHistory.CurrentColumn.Key == "View")
				{
					GeneratereportSlip(CurrId, "220-InvRptPurchaseBillSupplierRiceSlip.rpt");
				}
				if (grdHistory.CurrentColumn.Key == "PartySlip")
				{
					CommonServices.PurchaseInvoicettSlip_220A(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "SummaryReport")
				{
					GeneratereportSlip(CurrId, "220B-InvRptPurchaseBillSupplierRiceSummary.rpt");
				}
				if (grdHistory.CurrentColumn.Key == "GrnNo")
				{
					InvFrmGRN obj = new InvFrmGRN(UserAccount);
					if (clsGlobalVariables.ScreenViewReights.Count > 0)
					{
						if (!bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "InvFrmGRN").ToList()[0].Value.ToString()))
						{
							MessageBox.Show("Please Check Screen Rights");
							return;
						}
						obj.Show();
						obj.ReadById(Conversion.ToInt(grdHistory.CurrentRow.Cells[3].Value));
					}
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				Reset();
				RecId = CurrId;
				ReadById(RecId);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grd, dtGrid);
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
			FileStream fs = CommonServices.GetGridLayout(ScreenName, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(ScreenName, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(ScreenName, ((Control)(object)grdFreight).Name);
			if (fs != null)
			{
				grdFreight.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(ScreenName, ((Control)(object)grdInvExp).Name);
			if (fs != null)
			{
				grdInvExp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(ScreenName, ((Control)(object)grdGLedger).Name);
			if (fs != null)
			{
				grdGLedger.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(ScreenName, ((Control)(object)grdEmptyBags).Name);
			if (fs != null)
			{
				grdEmptyBags.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar9_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdPaymentDetail, dtGrid);
			}
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

	private DataTable GetGrnIdAndInvoiceTypeId(int GrnNo)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		DataTable dt = new DataTable();
		try
		{
			dt = InvGrn.GetGRNIdByDocNo(new InvGrn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 46,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocNo = GrnNo
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dt;
	}

	private void txtGrnNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)txtGrnNo.Text.Trim()) > 0)
			{
				DataTable dt = GetGrnIdAndInvoiceTypeId(Conversion.ToInt((object)txtGrnNo.Text.Trim()));
				if (dt.Rows.Count <= 0)
				{
					throw new Exception("Record Not Found For Loader");
				}
				int GrnId = Conversion.ToInt(dt.Rows[0]["Id"]);
				int InvoiecTypeId = Conversion.ToInt(dt.Rows[0]["RefDocumentTypeId"]);
				switch (InvoiecTypeId)
				{
				case 41:
					InvoiecTypeId = 1;
					break;
				case 105:
					InvoiecTypeId = 2;
					break;
				case 106:
					InvoiecTypeId = 3;
					break;
				case 217:
					InvoiecTypeId = 4;
					break;
				}
				LoadInGridDetail(GrnId, InvoiecTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoader_Click(object sender, EventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled)
			{
				LoadGRN = new frmLoadGRN(UserAccount);
				LoadGRN.DocumentTypeId = 46;
				LoadGRN.ShowDialog();
				LoadGRN.BringToFront();
				LoadInGridDetail();
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

	private void LoadInGridDetail(int GrnIdByDocNo = 0, int InvoicetypeIdbygrndocno = 0)
	{
		try
		{
			InvoiceTypeId = 1;
			dtGrid.Rows.Clear();
			string GrnIds = "";
			if (GrnIdByDocNo > 0)
			{
				InvoiceTypeId = InvoicetypeIdbygrndocno;
				GrnIds = "," + Conversion.ToInt((object)GrnIdByDocNo);
			}
			else
			{
				for (int i = 0; i < LoadGRN.dtGRNS.Rows.Count; i++)
				{
					GrnIds = GrnIds + "," + Conversion.ToInt((object)LoadGRN.dtGRNS.Rows[i][0].ToString());
					InvoiceTypeId = Conversion.ToInt((object)LoadGRN.dtGRNS.Rows[i][1].ToString());
				}
			}
			DataTable WagesData = InvPurchaseInvoice.GetWagesAmountByRefDocumentTypeIdandRefIds(UserAccount.OrganizationId, UserAccount.CompanyId, 46, GrnIds);
			if (WagesData.Rows.Count > 0)
			{
				txtWagesAmountHeader.Text = Conversion.ToString(WagesData.Rows[0]["WagesAmount"]);
			}
			if (GrnIds != null && !(GrnIds == string.Empty))
			{
				LoadDataDetailGridAgainstGP(GrnIds);
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				dtFreight.Rows.Clear();
				dtGrdGL.Rows.Clear();
				LoadFreightData(GrnIds);
				LoadGlGridDataRateCutInCaseOfAccessWeightReceived(GrnIds);
				LoadPurchaseOrderExpensesChargToProductData(POID);
				if (dtFreight.Rows.Count == 0 || dtFreight == null)
				{
					AddRowInFreightGrid();
				}
				grdFreight.RetrieveStructure();
				grdFreightSettings();
				if (dtGrdGL.Rows.Count == 0 || dtGrdGL == null)
				{
					AddRowInGLGrid();
				}
				grdGLedger.RetrieveStructure();
				gridGLSettings();
				dtInvExp.Clear();
				LoadExpData(GrnIds);
				if (dtInvExp.Rows.Count == 0 || dtInvExp == null)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.RetrieveStructure();
				dtEmptyBags.Clear();
				LoadEmptyBagsData(GrnIds);
				grdEmptyBags.RetrieveStructure();
				grdEmptyBagsSettings();
				TotalCommissionAmount();
				TotalBrokeryAmount();
				grdInvExpSettings();
				FreightProportion();
				FreightDeductionProportion();
				WagesAmountProportion();
				ExpProportion();
				LedgerProportion();
				CommissionProportion();
				BillProportion();
				LoadPaymentDetail();
				BillAmount();
				TotalAmtForEmptyBagGrid();
				GrnIds = "";
				CalculateEbPurAgainstWeightAmountBasedOnBalQty();
				CalculateOrderWiseAmountForPaymentDetail();
				PaymentAmountReCalculate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailGridAgainstGP(string IDs)
	{
		try
		{
			DataTable invGrn = InvPurchaseInvoice.GrnLoadForPurchaseInvoice(IDs);
			if (invGrn.Rows.Count > 0)
			{
				cmbsuppliername.Value = invGrn.Rows[0]["SupplierCustomerId"];
				((Control)(object)cmbsuppliername).Enabled = false;
				cmbcommagent.Value = invGrn.Rows[0]["BrokerAgentSupCustId"];
				if (Conversion.ToInt(cmbcommagent.Value) == 0)
				{
					cmbcommagent.Value = invGrn.Rows[0]["SupplierCustomerId"];
				}
				((Control)(object)cmbcommtype).Text = string.Empty;
				((Control)(object)cmbcommtype).Text = invGrn.Rows[0]["CommissionType"].ToString();
				txtcommrate.Text = invGrn.Rows[0]["CommRate"].ToString();
				((Control)(object)cmbcommuom).Text = string.Empty;
				((Control)(object)cmbcommuom).Text = invGrn.Rows[0]["UomScheduleIdCmRate"].ToString();
				txtcommamount.Text = invGrn.Rows[0]["CommAmount"].ToString();
				txtcommremarks.Text = invGrn.Rows[0]["CommissionRemarks"].ToString();
				txtRemarks.Text = invGrn.Rows[0]["RemarksHeader"].ToString();
				CmbBrokeryAc.Value = invGrn.Rows[0]["BrokerAgentId"];
				((Control)(object)CmbBrokeryType).Text = Conversion.ToString(invGrn.Rows[0]["BrokeryType"]);
				((Control)(object)CmbBrokeryRateUom).Text = Conversion.ToString(invGrn.Rows[0]["BrokeryUom"]);
				txtBrokeryRate.Text = Conversion.ToString(invGrn.Rows[0]["BrokeryRate"]);
				txtBrokeryAmount.Text = Conversion.ToString(invGrn.Rows[0]["BrokeryAmount"]);
				if (Conversion.ToInt(invGrn.Rows[0]["TransporterId"]) > 0)
				{
					CmbFreightAccount.Value = Conversion.ToInt(invGrn.Rows[0]["TransporterId"]);
				}
				if (Conversion.ToInt(invGrn.Rows[0]["PaymentTermsId"]) > 0)
				{
					CmbPaymentTerm.Value = Conversion.ToInt(invGrn.Rows[0]["PaymentTermsId"]);
				}
				txtDeliveryTerm.Text = invGrn.Rows[0]["DeliveryTerm"].ToString();
				txtDueDays.Text = invGrn.Rows[0]["OrderDueDays"].ToString();
				if (ValidateGrnAndInvoiceDateWithGpDate)
				{
					GrnDate = Conversion.ToDateTime(invGrn.Rows[0]["DocDate"]);
					DocDate.Value = GrnDate;
				}
				for (int i = 0; i < invGrn.Rows.Count; i++)
				{
					double RateUom = 0.0;
					RateUom = ((InvoiceTypeId != 2) ? Conversion.ToDouble(invGrn.Rows[i]["EquivalentPoRate"]) : 40.0);
					dtGrid.Rows.Add(0, invGrn.Rows[i]["InvGrnDetailId"], invGrn.Rows[i]["InvGrnId"], invGrn.Rows[i]["DocNo"], invGrn.Rows[i]["WarehouseId"], invGrn.Rows[i]["WareHouseName"], invGrn.Rows[i]["ItemId"], invGrn.Rows[i]["ItemName"], invGrn.Rows[i]["CropYear"], invGrn.Rows[i]["ItemUOMId"], invGrn.Rows[i]["UOMCodeItem"], invGrn.Rows[i]["ItemQty"], invGrn.Rows[i]["PackingTypeId"], invGrn.Rows[i]["PurchaseOrderId"], invGrn.Rows[i]["PurchaseOrder"], invGrn.Rows[i]["GrossWeight"], invGrn.Rows[i]["EBTotalWt"], invGrn.Rows[i]["EbPurAgainstWeight"], invGrn.Rows[i]["WeightCutTotal"], invGrn.Rows[i]["AdLsWeight"], invGrn.Rows[i]["NetBillWeight"], invGrn.Rows[i]["NetStockWeight"], invGrn.Rows[i]["ItemRate"], invGrn.Rows[i]["UomScheduleIdRate"], invGrn.Rows[i]["RateUom"], RateUom, Math.Round(Conversion.ToDouble(invGrn.Rows[i]["ItemAmount"]), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), invGrn.Rows[i]["JobLotId"], invGrn.Rows[i]["DeductionRate"], 0, 0, 0, 0, 0, 0, invGrn.Rows[i]["LabAnalisysNo"], invGrn.Rows[i]["GpNo"], invGrn.Rows[i]["VehicleNo"], Conversion.ToDateTime(invGrn.Rows[i]["GpDate"]).ToShortDateString(), 0, invGrn.Rows[i]["BranchId"], invGrn.Rows[i]["BranchName"]);
					POID = Conversion.ToInt(grd.CurrentRow.Cells["PurchaseOrderId"].Value);
				}
				RateCutAmountCalculations();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LoadFreightData(string IDs)
	{
		try
		{
			string deliveryTerm = txtDeliveryTerm.Text;
			DataTable items = InvPurchaseInvoice.InvGrn_GetTransporterAndFreight(IDs);
			if (items.Rows.Count == 0)
			{
				return;
			}
			int supplierGlId = Conversion.ToInt((object)txtSupplierGLId.Text);
			int supplierId = Conversion.ToInt(cmbsuppliername.Value);
			if (supplierGlId == 0 && supplierId > 0)
			{
				DataRow[] supplierRows = dtSupplier.Select("Id = " + supplierId);
				if (supplierRows.Length != 0)
				{
					supplierGlId = Conversion.ToInt(supplierRows[0]["GlAccountId"]);
					txtSupplierGLId.Text = supplierGlId.ToString();
				}
			}
			double totalChargeToParty = 0.0;
			double totalAdvanceByParty = 0.0;
			double totalAdvanceByFactory = 0.0;
			double totalPaid = 0.0;
			bool isLoadDelivery = deliveryTerm == "Load" || deliveryTerm == "Load & PartyWeight" || deliveryTerm == "Load & FactoryWeight";
			foreach (DataRow row in items.Rows)
			{
				string remarks = string.Empty;
				int gpNo = Conversion.ToInt(row["GpNo"]);
				if (gpNo > 0)
				{
					remarks += $"GpNo: {gpNo}";
				}
				string vehicleNo = Conversion.ToString(row["VehicleNo"]);
				if (!string.IsNullOrWhiteSpace(vehicleNo))
				{
					remarks = remarks + "   VehicleNo: " + vehicleNo;
				}
				string biltyNo = Conversion.ToString(row["BiltyNo"]);
				if (!string.IsNullOrWhiteSpace(biltyNo))
				{
					remarks = remarks + "   BiltyNo: " + biltyNo;
				}
				string partyname = Conversion.ToString((object)((Control)(object)cmbsuppliername).Text.Trim());
				if (!string.IsNullOrWhiteSpace(partyname))
				{
					remarks = remarks + "   PartyName: " + partyname;
				}
				int mainId = Conversion.ToInt(row["MainId"]);
				int freightId = Conversion.ToInt(row["FreightId"]);
				int transporterId = (SubsidiaryAccountAllownOnVouchers ? Conversion.ToInt(row["TransporterSupCustId"]) : Conversion.ToInt(row["Transporter"]));
				_ = row["CarriageAmount"];
				double num = Conversion.ToDouble(row["CarriageAmount"]);
				double advanceByFactory = Conversion.ToDouble(row["TotalAdvanceByFactoryAmount"]);
				double advanceByParty = Conversion.ToDouble(row["TotalAdanceByPartyAmount"]);
				double totalAmountWithAdvanceByFactory = num + advanceByFactory;
				if (num > 0.0)
				{
					if (SubsidiaryAccountAllownOnVouchers)
					{
						if (isLoadDelivery)
						{
							dtFreight.Rows.Add(POID, mainId, freightId, transporterId, transporterId, 0, 0, 0, totalAmountWithAdvanceByFactory, 0, totalAmountWithAdvanceByFactory, remarks, row["Transporter"]);
						}
						else if (freightId > 0)
						{
							if (transporterId != supplierGlId)
							{
								dtGrdGL.Rows.Add(mainId, freightId, transporterId, transporterId, remarks, 0, 0, 0, 0, totalAmountWithAdvanceByFactory, totalAmountWithAdvanceByFactory, row["Transporter"]);
							}
						}
						else
						{
							dtGrdGL.Rows.Add(mainId, freightId, transporterId, transporterId, remarks, 0, 0, 0, 0, totalAmountWithAdvanceByFactory, totalAmountWithAdvanceByFactory, row["Transporter"]);
						}
					}
					else if (isLoadDelivery)
					{
						dtFreight.Rows.Add(POID, mainId, freightId, transporterId, transporterId, 0, 0, 0, totalAmountWithAdvanceByFactory, 0, totalAmountWithAdvanceByFactory, remarks);
					}
					else if (freightId > 0)
					{
						if (transporterId != supplierGlId)
						{
							dtGrdGL.Rows.Add(mainId, freightId, transporterId, transporterId, remarks, 0, 0, 0, 0, totalAmountWithAdvanceByFactory, totalAmountWithAdvanceByFactory);
						}
					}
					else
					{
						dtGrdGL.Rows.Add(mainId, freightId, transporterId, transporterId, remarks, 0, 0, 0, 0, totalAmountWithAdvanceByFactory, totalAmountWithAdvanceByFactory);
					}
				}
				if (SubsidiaryAccountAllownOnVouchers)
				{
					if (isLoadDelivery && advanceByParty > 0.0)
					{
						dtFreight.Rows.Add(POID, row["MainId"], row["FreightId"], supplierId, supplierId, 0, 0, 0, advanceByParty, 0, advanceByParty, remarks + " Advance Paid By Party", supplierGlId);
					}
				}
				else if (isLoadDelivery && advanceByParty > 0.0)
				{
					dtFreight.Rows.Add(POID, row["MainId"], row["FreightId"], supplierGlId, supplierGlId, 0, 0, 0, advanceByParty, 0, advanceByParty, remarks + " Advance Paid By Party", 0);
				}
				totalChargeToParty += Conversion.ToDouble(row["TotalChargeToPartyAmount"]);
				totalAdvanceByParty += advanceByParty;
				totalAdvanceByFactory += advanceByFactory;
				totalPaid += Conversion.ToDouble(row["TotalPaidAmount"]);
			}
			double totalSum = totalChargeToParty;
			if (!isLoadDelivery && totalSum > 0.0)
			{
				txtFreightDeduction.Text = totalSum.ToString("#,##0.##");
				CmbFreightAccount.Value = supplierGlId;
			}
			else if (isLoadDelivery && totalChargeToParty > 0.0)
			{
				txtFreightDeduction.Text = totalChargeToParty.ToString("#,##0.##");
				CmbFreightAccount.Value = supplierGlId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadGlGridDataRateCutInCaseOfAccessWeightReceived(string IDs)
	{
		try
		{
			_ = txtDeliveryTerm.Text;
			DataTable items = InvPurchaseInvoice.GetDataRateCutInCaseOfAccessWeightReceived(UserAccount.OrganizationId, UserAccount.CompanyId, IDs);
			if (items.Rows.Count == 0)
			{
				return;
			}
			foreach (DataRow row in items.Rows)
			{
				if (Conversion.ToInt(row["RateCutAmount"]) > 0)
				{
					string remarks = string.Empty;
					int gpNo = Conversion.ToInt(row["GpNo"]);
					if (gpNo > 0)
					{
						remarks += $"GpNo: {gpNo}";
					}
					string vehicleNo = Conversion.ToString(row["VehicleNo"]);
					if (!string.IsNullOrWhiteSpace(vehicleNo))
					{
						remarks = remarks + "   VehicleNo: " + vehicleNo;
					}
					string biltyNo = Conversion.ToString(row["BiltyNo"]);
					if (!string.IsNullOrWhiteSpace(biltyNo))
					{
						remarks = remarks + "   BiltyNo: " + biltyNo;
					}
					object mainId = row["MainId"];
					object glAccountId = row["GlAccountId"];
					object qty = row["Qty"];
					object rateCut = row["RateCut"];
					object rateCutAmt = row["RateCutAmount"];
					object subGlId = (SubsidiaryAccountAllownOnVouchers ? glAccountId : ((object)0));
					dtGrdGL.Rows.Add(mainId, 0, glAccountId, glAccountId, remarks, 0, qty, rateCut, 0, rateCutAmt, rateCutAmt, subGlId, qty, rateCut, 1);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadPurchaseOrderExpensesChargToProductData(int PurchaseOrderId)
	{
		try
		{
			double itemAmount = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
			double itemQty = GridEX_Helper.GetColumnSum(grd, "ItemQty");
			DataTable items = InvPurchaseInvoice.GetPurchaseOrderExpensesChargToProdut(PurchaseOrderId, (string)null);
			if (items.Rows.Count == 0)
			{
				return;
			}
			foreach (DataRow row in items.Rows)
			{
				int accountId = Conversion.ToInt(row["AccountId"]);
				if (accountId > 0)
				{
					double percentage = Conversion.ToDouble(row["Percentage"]);
					double rate = Conversion.ToDouble(row["Rate"]);
					double amount;
					double qtyUsed;
					if (percentage > 0.0 && itemAmount > 0.0)
					{
						amount = Math.Round(itemAmount * percentage / 100.0);
						qtyUsed = Conversion.ToDouble(row["Qty"]);
					}
					else
					{
						amount = Math.Round(itemQty * rate);
						qtyUsed = itemQty;
					}
					dtFreight.Rows.Add(row["PurchaseOrderId"], 0, 0, 0, accountId, percentage, qtyUsed, rate, amount, 0, amount, row["Remarks"], 0);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadEmptyBagsData(string IDs)
	{
		try
		{
			DataTable item = InvPurchaseInvoice.GetEmptyBagsFromGrn(IDs, 0);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow dr in item.Rows)
			{
				if (Conversion.ToDouble(dr["PurchaseQty"]) > 0.0)
				{
					DataRow newRow = dtEmptyBags.NewRow();
					newRow["OrderId"] = dr["PurchaseOrderId"];
					newRow["TypeId"] = dr["TypeId"];
					newRow["Type"] = dr["EmptyBagsType"];
					newRow["InvGrnId"] = dr["InvGrnId"];
					newRow["ItemId"] = dr["ItemId"];
					newRow["ItemName"] = dr["ItemName"];
					newRow["ItemConditionId"] = dr["ItemConditionId"];
					newRow["ItemCondition"] = dr["ItemCondition"];
					newRow["PurchaseQty"] = dr["PurchaseQty"];
					newRow["Rate"] = dr["Rate"];
					double qty = Conversion.ToDouble(dr["PurchaseQty"]);
					double rate = Conversion.ToDouble(dr["Rate"]);
					newRow["Amount"] = qty * rate;
					newRow["Remarks"] = dr["Remarks"];
					newRow["CreditAccountId"] = 0;
					dtEmptyBags.Rows.Add(newRow);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LoadExpData(string IDs)
	{
		try
		{
			DataTable item = InvPurchaseInvoice.GetExpenseDetailFromPOAndPreBill(IDs);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < item.Rows.Count; i++)
			{
				if (Conversion.ToInt(item.Rows[i]["Rate"]) > 0)
				{
					dtInvExp.Rows.Add(0, item.Rows[i]["GrnId"], item.Rows[i]["GrnNo"], item.Rows[i]["PoId"], 0, item.Rows[i]["SupplierDispatchId"], item.Rows[i]["SupplierDispatchExpenseId"], item.Rows[i]["OtherItemId"], item.Rows[i]["Qty"], item.Rows[i]["Rate"], Conversion.ToDouble(item.Rows[i]["Qty"]) * Conversion.ToDouble(item.Rows[i]["Rate"]), item.Rows[i]["Remarks"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GenerateReportVoucher104()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (VoucherHeadId == 0)
			{
				throw new Exception("VoucherId Not Found");
			}
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherSlipForInventoryReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				ProjectsId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				Id = VoucherHeadId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
			}
			else
			{
				new Reporting().ShowReportWithDataTable(dt, "104-AcRptGeneralJournalAcAndInventoryDetailSlip.rpt");
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
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId);
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
			GeneratereportSlip(RecId, "220-InvRptPurchaseBillSupplierRiceSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratereportSlip(int PrintId, string ReportName)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("Record Id Not Found");
				return;
			}
			DataTable dt = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = PrintId;
			dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprtForItem(obj.Id), "InvRptPurchaseItemBillOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, ReportName, lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip220A_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseInvoicettSlip_220A(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn220bSummary_Click(object sender, EventArgs e)
	{
		try
		{
			GeneratereportSlip(RecId, "220B-InvRptPurchaseBillSupplierRiceSummary.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn104Voucher_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReportVoucher104();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		DuedaysCalculates();
	}

	private void DuedaysCalculates()
	{
		try
		{
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				DueDate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				DueDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DuedaysCalculates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPaymentTerm_Leave(object sender, EventArgs e)
	{
		try
		{
			txtDueDays.Enabled = true;
			DueDate.Enabled = true;
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 1 || Conversion.ToInt(CmbPaymentTerm.Value) == 3)
			{
				txtDueDays.Text = "";
				txtDueDays.Enabled = false;
				DueDate.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGrnNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcommrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrokeryRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFreightDeduction_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	private void CmbFreightAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtFreightDeduction.Text) > 0.0)
			{
				FreightDeductionProportion();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutKeys_Click(object sender, EventArgs e)
	{
		MakeShortCutKeys();
	}

	private void grdGLedger_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currRow = grdGLedger.CurrentRow;
			if (currRow != null && (int)currRow.RowType == 0)
			{
				if (Conversion.ToInt(currRow.Cells["RowType"].Value) == 1)
				{
					grdGLedger.RootTable.Columns["Percentage"].EditType = (EditType)0;
					grdGLedger.RootTable.Columns["Qty"].EditType = (EditType)0;
					grdGLedger.RootTable.Columns["Debit"].EditType = (EditType)0;
					grdGLedger.RootTable.Columns["Credit"].EditType = (EditType)0;
				}
				else
				{
					grdGLedger.RootTable.Columns["Percentage"].EditType = (EditType)1;
					grdGLedger.RootTable.Columns["Qty"].EditType = (EditType)1;
					grdGLedger.RootTable.Columns["Debit"].EditType = (EditType)1;
					grdGLedger.RootTable.Columns["Credit"].EditType = (EditType)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBillAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateOrderWiseAmountForPaymentDetail();
			PaymentAmountReCalculate();
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
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Expected O, but got Unknown
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Expected O, but got Unknown
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Expected O, but got Unknown
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Expected O, but got Unknown
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Expected O, but got Unknown
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Expected O, but got Unknown
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Expected O, but got Unknown
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Expected O, but got Unknown
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Expected O, but got Unknown
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Expected O, but got Unknown
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Expected O, but got Unknown
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Expected O, but got Unknown
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_0922: Expected O, but got Unknown
		//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Expected O, but got Unknown
		//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Expected O, but got Unknown
		//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Expected O, but got Unknown
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Expected O, but got Unknown
		//IL_1367: Unknown result type (might be due to invalid IL or missing references)
		//IL_1371: Expected O, but got Unknown
		//IL_137e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1388: Expected O, but got Unknown
		//IL_1e07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e11: Expected O, but got Unknown
		//IL_1e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e28: Expected O, but got Unknown
		//IL_2167: Unknown result type (might be due to invalid IL or missing references)
		//IL_2171: Expected O, but got Unknown
		//IL_217e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2188: Expected O, but got Unknown
		//IL_2195: Unknown result type (might be due to invalid IL or missing references)
		//IL_219f: Expected O, but got Unknown
		//IL_2592: Unknown result type (might be due to invalid IL or missing references)
		//IL_259c: Expected O, but got Unknown
		//IL_25a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b3: Expected O, but got Unknown
		//IL_25c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ca: Expected O, but got Unknown
		//IL_2922: Unknown result type (might be due to invalid IL or missing references)
		//IL_292c: Expected O, but got Unknown
		//IL_2939: Unknown result type (might be due to invalid IL or missing references)
		//IL_2943: Expected O, but got Unknown
		//IL_2950: Unknown result type (might be due to invalid IL or missing references)
		//IL_295a: Expected O, but got Unknown
		//IL_8ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_8add: Expected O, but got Unknown
		//IL_8b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_8b0b: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
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
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
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
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.InvfrmPurchaseInvoice));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.ctrlGrdBar8 = new grid.User_Conrtols.CtrlGrdBar();
		this.label30 = new System.Windows.Forms.Label();
		this.tabPaymentSchedule = new System.Windows.Forms.TabPage();
		this.grdPaymentDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.ctrlGrdBar9 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.TabControlPanelForExpenseGrids = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.PartyAddLessAndEmptyBagGridParentPanel = new System.Windows.Forms.Panel();
		this.EmptyBagGridPanel = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdEmptyBags = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.PartyAddLessGridPanel = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdGLedger = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.TransporterAndOtherItemExpenseParentPanel = new System.Windows.Forms.Panel();
		this.OtherItemExpensePanel = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdInvExp = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.TransporterGridPanel = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdFreight = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.cmbsuppliername = new UltraCombo();
		this.CmbFreightAccount = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtFreightDeduction = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtGrnNo = new System.Windows.Forms.TextBox();
		this.datSupplierInvoiceDate = new System.Windows.Forms.DateTimePicker();
		this.rdTemplate2 = new System.Windows.Forms.RadioButton();
		this.rdTemplate1 = new System.Windows.Forms.RadioButton();
		this.CmbPaymentTerm = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtcommremarks = new System.Windows.Forms.TextBox();
		this.txtWagesAmountHeader = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtRefParty = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbBrokeryType = new UltraCombo();
		this.cmbcommtype = new UltraCombo();
		this.CmbBrokeryRateUom = new UltraCombo();
		this.cmbcommuom = new UltraCombo();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.txtDeliveryTerm = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.CmbBrokeryAc = new UltraCombo();
		this.cmbcommagent = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbstockprty = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtBrokeryRate = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtBrokeryAmount = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtBranchCode = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnFrmRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlipDetail = new System.Windows.Forms.ToolStripButton();
		this.btnSlip220A = new System.Windows.Forms.ToolStripButton();
		this.btn220bSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btn104Voucher = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoader = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.BtnSpecialRights = new System.Windows.Forms.ToolStripButton();
		this.label34 = new System.Windows.Forms.Label();
		this.ChkBokVoucher = new System.Windows.Forms.CheckBox();
		this.ChkBoxParty = new System.Windows.Forms.CheckBox();
		this.ChkBoxItemPrint = new System.Windows.Forms.CheckBox();
		this.chkSummary = new System.Windows.Forms.CheckBox();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label32 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.tabPaymentSchedule.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.TabControlPanelForExpenseGrids.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.PartyAddLessAndEmptyBagGridParentPanel.SuspendLayout();
		this.EmptyBagGridPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel19.SuspendLayout();
		this.PartyAddLessGridPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel18.SuspendLayout();
		this.TransporterAndOtherItemExpenseParentPanel.SuspendLayout();
		this.OtherItemExpensePanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel17.SuspendLayout();
		this.TransporterGridPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel21.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel14.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1327, 777);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1327, 777);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 8;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel5);
		this.tabForm.Controls.Add(this.panel4);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1319, 748);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.TabControlPanelForExpenseGrids);
		this.panel5.Controls.Add(this.PartyAddLessAndEmptyBagGridParentPanel);
		this.panel5.Controls.Add(this.TransporterAndOtherItemExpenseParentPanel);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 209);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1313, 536);
		this.panel5.TabIndex = 3;
		this.panel7.Controls.Add(this.tabControl3);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 492);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1313, 44);
		this.panel7.TabIndex = 21;
		this.tabControl3.Controls.Add(this.tabDetail);
		this.tabControl3.Controls.Add(this.tabPaymentSchedule);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl3.Location = new System.Drawing.Point(0, 0);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1313, 44);
		this.tabControl3.TabIndex = 26;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.ctrlGrdBar8);
		this.tabDetail.Controls.Add(this.label30);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1305, 15);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1299, 0);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).Tag = "GrdDetail";
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		this.ctrlGrdBar8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar8.FormName = this;
		this.ctrlGrdBar8.Location = new System.Drawing.Point(1264, 4);
		this.ctrlGrdBar8.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar8.MyGrid = this.grd;
		this.ctrlGrdBar8.Name = "ctrlGrdBar8";
		this.ctrlGrdBar8.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar8.TabIndex = 25;
		this.ctrlGrdBar8.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label30.BackColor = System.Drawing.Color.Teal;
		this.label30.Dock = System.Windows.Forms.DockStyle.Top;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(3, 3);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(1299, 22);
		this.label30.TabIndex = 0;
		this.label30.Text = "Detail Grid";
		this.tabPaymentSchedule.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentDetail);
		this.tabPaymentSchedule.Controls.Add(this.panel8);
		this.tabPaymentSchedule.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentSchedule.Name = "tabPaymentSchedule";
		this.tabPaymentSchedule.Padding = new System.Windows.Forms.Padding(3);
		this.tabPaymentSchedule.Size = new System.Drawing.Size(1305, 15);
		this.tabPaymentSchedule.TabIndex = 1;
		this.tabPaymentSchedule.Text = "Payment Schedule";
		this.tabPaymentSchedule.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPaymentDetail).BackColor = System.Drawing.Color.White;
		grdPaymentDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentDetail.DesignTimeLayout = grdPaymentDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Name = "grdPaymentDetail";
		((ScrollableBase)this.grdPaymentDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Size = new System.Drawing.Size(1299, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).TabIndex = 7;
		this.grdPaymentDetail.TotalRow = (InheritableBoolean)1;
		this.grdPaymentDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentDetail).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label18);
		this.panel8.Controls.Add(this.ctrlGrdBar9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1299, 22);
		this.panel8.TabIndex = 2;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(128, 14);
		this.label18.TabIndex = 40;
		this.label18.Text = "Payment Detail Grid";
		this.ctrlGrdBar9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar9.FormName = this;
		this.ctrlGrdBar9.Location = new System.Drawing.Point(1260, 1);
		this.ctrlGrdBar9.MyGrid = this.grdPaymentDetail;
		this.ctrlGrdBar9.Name = "ctrlGrdBar9";
		this.ctrlGrdBar9.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar9.TabIndex = 101;
		this.ctrlGrdBar9.Load += new System.EventHandler(ctrlGrdBar9_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1382, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.TabControlPanelForExpenseGrids.Controls.Add(this.tabControl2);
		this.TabControlPanelForExpenseGrids.Dock = System.Windows.Forms.DockStyle.Top;
		this.TabControlPanelForExpenseGrids.Location = new System.Drawing.Point(0, 320);
		this.TabControlPanelForExpenseGrids.Name = "TabControlPanelForExpenseGrids";
		this.TabControlPanelForExpenseGrids.Size = new System.Drawing.Size(1313, 172);
		this.TabControlPanelForExpenseGrids.TabIndex = 5;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1313, 172);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1305, 143);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Supplier Add/Less";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1305, 143);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Debit To Product";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1305, 143);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Other Expense";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(1305, 143);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "Empty Bag Purchase";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.PartyAddLessAndEmptyBagGridParentPanel.Controls.Add(this.EmptyBagGridPanel);
		this.PartyAddLessAndEmptyBagGridParentPanel.Controls.Add(this.PartyAddLessGridPanel);
		this.PartyAddLessAndEmptyBagGridParentPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.PartyAddLessAndEmptyBagGridParentPanel.Location = new System.Drawing.Point(0, 156);
		this.PartyAddLessAndEmptyBagGridParentPanel.Name = "PartyAddLessAndEmptyBagGridParentPanel";
		this.PartyAddLessAndEmptyBagGridParentPanel.Size = new System.Drawing.Size(1313, 164);
		this.PartyAddLessAndEmptyBagGridParentPanel.TabIndex = 3;
		this.EmptyBagGridPanel.Controls.Add(this.ctrlGrdBar7);
		this.EmptyBagGridPanel.Controls.Add(this.panel19);
		this.EmptyBagGridPanel.Controls.Add(this.label29);
		this.EmptyBagGridPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.EmptyBagGridPanel.Location = new System.Drawing.Point(641, 0);
		this.EmptyBagGridPanel.Name = "EmptyBagGridPanel";
		this.EmptyBagGridPanel.Size = new System.Drawing.Size(669, 164);
		this.EmptyBagGridPanel.TabIndex = 3;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(627, 0);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdEmptyBags;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 24;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(669, 143);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Tag = "GrdEmptyBags";
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.CellUpdated += new ColumnActionEventHandler(grdEmptyBags_CellUpdated);
		this.grdEmptyBags.UpdatingCell += new UpdatingCellEventHandler(grdEmptyBags_UpdatingCell);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 21);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(669, 143);
		this.panel19.TabIndex = 4;
		this.label29.BackColor = System.Drawing.Color.Teal;
		this.label29.Dock = System.Windows.Forms.DockStyle.Top;
		this.label29.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(0, 0);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(669, 21);
		this.label29.TabIndex = 3;
		this.label29.Text = "Empty Bag Purchase (Debit to P.M Item)";
		this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.PartyAddLessGridPanel.Controls.Add(this.ctrlGrdBar6);
		this.PartyAddLessGridPanel.Controls.Add(this.panel18);
		this.PartyAddLessGridPanel.Controls.Add(this.label28);
		this.PartyAddLessGridPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.PartyAddLessGridPanel.Location = new System.Drawing.Point(0, 0);
		this.PartyAddLessGridPanel.Name = "PartyAddLessGridPanel";
		this.PartyAddLessGridPanel.Size = new System.Drawing.Size(641, 164);
		this.PartyAddLessGridPanel.TabIndex = 2;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(596, 0);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar6.MyGrid = this.grdGLedger;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar6.TabIndex = 23;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(641, 143);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Tag = "GrdPartyAddLess";
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Click += new System.EventHandler(grdGLedger_Click);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 21);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(641, 143);
		this.panel18.TabIndex = 4;
		this.label28.BackColor = System.Drawing.Color.Teal;
		this.label28.Dock = System.Windows.Forms.DockStyle.Top;
		this.label28.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(0, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(641, 21);
		this.label28.TabIndex = 3;
		this.label28.Text = "Supplier Add/Less (Amount Debit or credit to Selected A/c Against Supplier";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.TransporterAndOtherItemExpenseParentPanel.Controls.Add(this.OtherItemExpensePanel);
		this.TransporterAndOtherItemExpenseParentPanel.Controls.Add(this.TransporterGridPanel);
		this.TransporterAndOtherItemExpenseParentPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.TransporterAndOtherItemExpenseParentPanel.Location = new System.Drawing.Point(0, 0);
		this.TransporterAndOtherItemExpenseParentPanel.Name = "TransporterAndOtherItemExpenseParentPanel";
		this.TransporterAndOtherItemExpenseParentPanel.Size = new System.Drawing.Size(1313, 156);
		this.TransporterAndOtherItemExpenseParentPanel.TabIndex = 2;
		this.OtherItemExpensePanel.Controls.Add(this.ctrlGrdBar5);
		this.OtherItemExpensePanel.Controls.Add(this.panel17);
		this.OtherItemExpensePanel.Controls.Add(this.label27);
		this.OtherItemExpensePanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.OtherItemExpensePanel.Location = new System.Drawing.Point(641, 0);
		this.OtherItemExpensePanel.Name = "OtherItemExpensePanel";
		this.OtherItemExpensePanel.Size = new System.Drawing.Size(669, 156);
		this.OtherItemExpensePanel.TabIndex = 1;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(627, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.grdInvExp;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar5.TabIndex = 22;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		this.grdInvExp.ColumnAutoResize = true;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(669, 135);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Tag = "GrdExpense";
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 21);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(669, 135);
		this.panel17.TabIndex = 3;
		this.label27.BackColor = System.Drawing.Color.Teal;
		this.label27.Dock = System.Windows.Forms.DockStyle.Top;
		this.label27.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(0, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(669, 21);
		this.label27.TabIndex = 2;
		this.label27.Text = "Other Expense (Credit To Supplier & Debit To Product)";
		this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.TransporterGridPanel.Controls.Add(this.ctrlGrdBar4);
		this.TransporterGridPanel.Controls.Add(this.panel11);
		this.TransporterGridPanel.Controls.Add(this.label24);
		this.TransporterGridPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.TransporterGridPanel.Location = new System.Drawing.Point(0, 0);
		this.TransporterGridPanel.Name = "TransporterGridPanel";
		this.TransporterGridPanel.Size = new System.Drawing.Size(641, 156);
		this.TransporterGridPanel.TabIndex = 0;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(596, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdFreight;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 21;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(641, 135);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdFreight).Tag = "GrdFreight";
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.UpdatingCell += new UpdatingCellEventHandler(grdFreight_UpdatingCell);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreight).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 21);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(641, 135);
		this.panel11.TabIndex = 2;
		this.label24.BackColor = System.Drawing.Color.Teal;
		this.label24.Dock = System.Windows.Forms.DockStyle.Top;
		this.label24.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(0, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(641, 21);
		this.label24.TabIndex = 1;
		this.label24.Text = "Amount Credit to selected A/c and Debit to Product";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsuppliername);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFreightAccount);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtFreightDeduction);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.txtGrnNo);
		this.panel4.Controls.Add(this.datSupplierInvoiceDate);
		this.panel4.Controls.Add(this.rdTemplate2);
		this.panel4.Controls.Add(this.rdTemplate1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.txtcommremarks);
		this.panel4.Controls.Add(this.txtWagesAmountHeader);
		this.panel4.Controls.Add(this.label35);
		this.panel4.Controls.Add(this.txtRefParty);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommtype);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommuom);
		this.panel4.Controls.Add(this.txtDueDays);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.DueDate);
		this.panel4.Controls.Add(this.txtDeliveryTerm);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.txtCommAgentId);
		this.panel4.Controls.Add(this.txtSupplierGLId);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryAc);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbstockprty);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.txtBrokeryRate);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtcommrate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.txtBrokeryAmount);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtcommamount);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtBillAmount);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtbillno);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.txtBranchCode);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 61);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1313, 148);
		this.panel4.TabIndex = 0;
		this.cmbsuppliername.AllowNull = (DefaultableBoolean)1;
		this.cmbsuppliername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsuppliername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsuppliername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsuppliername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Location = new System.Drawing.Point(87, 35);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Name = "cmbsuppliername";
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).TabIndex = 3;
		this.cmbsuppliername.ValueChanged += new System.EventHandler(cmbsuppliername_ValueChanged);
		this.CmbFreightAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbFreightAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFreightAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFreightAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFreightAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFreightAccount).Font = new System.Drawing.Font("Verdana", 9f);
		this.CmbFreightAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFreightAccount).Location = new System.Drawing.Point(413, 63);
		((System.Windows.Forms.Control)(object)this.CmbFreightAccount).Name = "CmbFreightAccount";
		((System.Windows.Forms.Control)(object)this.CmbFreightAccount).Size = new System.Drawing.Size(177, 25);
		((System.Windows.Forms.Control)(object)this.CmbFreightAccount).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbFreightAccount).Leave += new System.EventHandler(CmbFreightAccount_Leave);
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(345, 60);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 30);
		this.label5.TabIndex = 44;
		this.label5.Text = "Freight Ac (Dr)";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtFreightDeduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightDeduction.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtFreightDeduction.Location = new System.Drawing.Point(413, 92);
		this.txtFreightDeduction.Name = "txtFreightDeduction";
		this.txtFreightDeduction.Size = new System.Drawing.Size(177, 22);
		this.txtFreightDeduction.TabIndex = 13;
		this.txtFreightDeduction.TextChanged += new System.EventHandler(txtFreightDeduction_TextChanged);
		this.txtFreightDeduction.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightDeduction_KeyPress);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(346, 88);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(63, 30);
		this.label16.TabIndex = 43;
		this.label16.Text = "Freight \r\nDeduction";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtGrnNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrnNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrnNo.Location = new System.Drawing.Point(527, 37);
		this.txtGrnNo.Name = "txtGrnNo";
		this.txtGrnNo.Size = new System.Drawing.Size(63, 23);
		this.txtGrnNo.TabIndex = 11;
		this.txtGrnNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNo_KeyPress);
		this.txtGrnNo.Leave += new System.EventHandler(txtGrnNo_Leave);
		this.datSupplierInvoiceDate.CustomFormat = "dd-MMM-yy";
		this.datSupplierInvoiceDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datSupplierInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datSupplierInvoiceDate.Location = new System.Drawing.Point(221, 64);
		this.datSupplierInvoiceDate.Name = "datSupplierInvoiceDate";
		this.datSupplierInvoiceDate.Size = new System.Drawing.Size(122, 23);
		this.datSupplierInvoiceDate.TabIndex = 5;
		this.rdTemplate2.AutoSize = true;
		this.rdTemplate2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdTemplate2.Location = new System.Drawing.Point(1233, 124);
		this.rdTemplate2.Name = "rdTemplate2";
		this.rdTemplate2.Size = new System.Drawing.Size(85, 19);
		this.rdTemplate2.TabIndex = 29;
		this.rdTemplate2.Text = "Template II";
		this.rdTemplate2.UseVisualStyleBackColor = true;
		this.rdTemplate1.AutoSize = true;
		this.rdTemplate1.Checked = true;
		this.rdTemplate1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdTemplate1.Location = new System.Drawing.Point(1233, 103);
		this.rdTemplate1.Name = "rdTemplate1";
		this.rdTemplate1.Size = new System.Drawing.Size(81, 19);
		this.rdTemplate1.TabIndex = 28;
		this.rdTemplate1.TabStop = true;
		this.rdTemplate1.Text = "Template I";
		this.rdTemplate1.UseVisualStyleBackColor = true;
		this.rdTemplate1.CheckedChanged += new System.EventHandler(rdTemplate1_CheckedChanged);
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(87, 90);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Leave += new System.EventHandler(CmbPaymentTerm_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(4, 96);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(84, 15);
		this.label33.TabIndex = 34;
		this.label33.Text = "Payment Term";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(87, 119);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(503, 23);
		this.txtRemarks.TabIndex = 7;
		this.txtcommremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommremarks.Location = new System.Drawing.Point(673, 92);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(229, 23);
		this.txtcommremarks.TabIndex = 19;
		this.txtWagesAmountHeader.BackColor = System.Drawing.Color.WhiteSmoke;
		this.txtWagesAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesAmountHeader.Enabled = false;
		this.txtWagesAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWagesAmountHeader.Location = new System.Drawing.Point(673, 119);
		this.txtWagesAmountHeader.Name = "txtWagesAmountHeader";
		this.txtWagesAmountHeader.Size = new System.Drawing.Size(229, 23);
		this.txtWagesAmountHeader.TabIndex = 20;
		this.txtWagesAmountHeader.TextChanged += new System.EventHandler(txtWagesAmountHeader_TextChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(592, 122);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(78, 17);
		this.label35.TabIndex = 33;
		this.label35.Text = "Wages Amt";
		this.txtRefParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefParty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefParty.Location = new System.Drawing.Point(87, 64);
		this.txtRefParty.Name = "txtRefParty";
		this.txtRefParty.Size = new System.Drawing.Size(133, 23);
		this.txtRefParty.TabIndex = 4;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(4, 60);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(86, 30);
		this.label31.TabIndex = 33;
		this.label31.Text = "Ref Party && Supp Invoice Date";
		this.CmbBrokeryType.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrokeryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Location = new System.Drawing.Point(999, 35);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Name = "CmbBrokeryType";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TabIndex = 22;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TextChanged += new System.EventHandler(CmbBrokeryType_TextChanged);
		this.cmbcommtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(673, 35);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TextChanged += new System.EventHandler(cmbcommtype_TextChanged);
		this.CmbBrokeryRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Location = new System.Drawing.Point(999, 62);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Name = "CmbBrokeryRateUom";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TabIndex = 24;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TextChanged += new System.EventHandler(CmbBrokeryRateUom_TextChanged);
		this.cmbcommuom.AllowNull = (DefaultableBoolean)1;
		this.cmbcommuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Location = new System.Drawing.Point(673, 62);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Name = "cmbcommuom";
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).TextChanged += new System.EventHandler(cmbcommuom_TextChanged);
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDueDays.Location = new System.Drawing.Point(413, 9);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(70, 23);
		this.txtDueDays.TabIndex = 8;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(345, 13);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(58, 15);
		this.label22.TabIndex = 32;
		this.label22.Text = "Due Days";
		this.DueDate.CustomFormat = "dd-MMM-yy";
		this.DueDate.Enabled = false;
		this.DueDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(485, 9);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(105, 22);
		this.DueDate.TabIndex = 9;
		this.txtDeliveryTerm.BackColor = System.Drawing.Color.White;
		this.txtDeliveryTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(999, 92);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.ReadOnly = true;
		this.txtDeliveryTerm.Size = new System.Drawing.Size(231, 23);
		this.txtDeliveryTerm.TabIndex = 26;
		this.txtDeliveryTerm.TabStop = false;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(906, 96);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(80, 15);
		this.label15.TabIndex = 28;
		this.label15.Text = "Delivery Term";
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1358, 11);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(44, 26);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1358, 40);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(44, 26);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.CmbBrokeryAc.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance143;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance149).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrokeryAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Location = new System.Drawing.Point(999, 7);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Name = "CmbBrokeryAc";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).TabIndex = 21;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Leave += new System.EventHandler(CmbBrokeryAc_Leave);
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(673, 7);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Leave += new System.EventHandler(cmbcommagent_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(906, 13);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(82, 15);
		this.label14.TabIndex = 3;
		this.label14.Text = "Brokery Agent";
		this.cmbstockprty.AllowNull = (DefaultableBoolean)1;
		this.cmbstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Location = new System.Drawing.Point(1351, 99);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Name = "cmbstockprty";
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Size = new System.Drawing.Size(52, 24);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Visible = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1090, 68);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(51, 15);
		this.label12.TabIndex = 8;
		this.label12.Text = "Amount";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(592, 13);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(906, 41);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(75, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "Brokery Type";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(764, 68);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(592, 41);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(71, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1087, 41);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(73, 15);
		this.label2.TabIndex = 8;
		this.label2.Text = "Brokery Rate";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(592, 96);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(52, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(761, 41);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Comm Rate";
		this.txtBrokeryRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBrokeryRate.Location = new System.Drawing.Point(1160, 37);
		this.txtBrokeryRate.Name = "txtBrokeryRate";
		this.txtBrokeryRate.Size = new System.Drawing.Size(70, 23);
		this.txtBrokeryRate.TabIndex = 23;
		this.txtBrokeryRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrokeryRate.TextChanged += new System.EventHandler(txtBrokeryRate_TextChanged);
		this.txtBrokeryRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrokeryRate_KeyPress);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 41);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Supplier Name";
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.Location = new System.Drawing.Point(832, 37);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(70, 23);
		this.txtcommrate.TabIndex = 16;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(906, 68);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(61, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Rate UOM";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 123);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 15);
		this.label4.TabIndex = 20;
		this.label4.Text = "Remarks";
		this.txtBrokeryAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtBrokeryAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBrokeryAmount.Location = new System.Drawing.Point(1160, 64);
		this.txtBrokeryAmount.Name = "txtBrokeryAmount";
		this.txtBrokeryAmount.ReadOnly = true;
		this.txtBrokeryAmount.Size = new System.Drawing.Size(70, 23);
		this.txtBrokeryAmount.TabIndex = 25;
		this.txtBrokeryAmount.TabStop = false;
		this.txtBrokeryAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(592, 68);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(61, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Rate UOM";
		this.txtcommamount.BackColor = System.Drawing.SystemColors.Control;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommamount.Location = new System.Drawing.Point(832, 64);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.Size = new System.Drawing.Size(70, 23);
		this.txtcommamount.TabIndex = 18;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1348, 74);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.txtBillAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(999, 117);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(231, 27);
		this.txtBillAmount.TabIndex = 27;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillAmount.TextChanged += new System.EventHandler(txtBillAmount_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(484, 41);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(45, 15);
		this.label23.TabIndex = 0;
		this.label23.Text = "Grn No";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(345, 41);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(42, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Bill No";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(906, 120);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(93, 20);
		this.label3.TabIndex = 17;
		this.label3.Text = "Bill Amount";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbillno.Location = new System.Drawing.Point(413, 37);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(70, 23);
		this.txtbillno.TabIndex = 10;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(221, 9);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(122, 23);
		this.DocDate.TabIndex = 2;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtBranchCode.BackColor = System.Drawing.Color.White;
		this.txtBranchCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchCode.Location = new System.Drawing.Point(158, 9);
		this.txtBranchCode.Name = "txtBranchCode";
		this.txtBranchCode.ReadOnly = true;
		this.txtBranchCode.Size = new System.Drawing.Size(62, 23);
		this.txtBranchCode.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(87, 9);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(70, 23);
		this.txtdocno.TabIndex = 0;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(4, 13);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No,Date";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.chkCustomAccounts);
		this.panel2.Controls.Add(this.labelLocationType);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.label34);
		this.panel2.Controls.Add(this.ChkBokVoucher);
		this.panel2.Controls.Add(this.ChkBoxParty);
		this.panel2.Controls.Add(this.ChkBoxItemPrint);
		this.panel2.Controls.Add(this.chkSummary);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1313, 58);
		this.panel2.TabIndex = 0;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(473, 34);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(120, 19);
		this.chkCustomAccounts.TabIndex = 511;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(168, 36);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 510;
		this.labelLocationType.Text = "Location Type";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(257, 30);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 509;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[18]
		{
			this.btnNew, this.btnFrmRefresh, this.toolStripSeparator1, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.toolStripSeparator3, this.btnSlipDetail,
			this.btnSlip220A, this.btn220bSummary, this.btnPrint, this.btn104Voucher, this.toolStripSeparator4, this.BtnLoader, this.btnShortcutKeys, this.BtnSpecialRights
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1313, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
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
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnSlipDetail.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlipDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlipDetail.Name = "btnSlipDetail";
		this.btnSlipDetail.Size = new System.Drawing.Size(122, 24);
		this.btnSlipDetail.Text = "220-&SlipDetail";
		this.btnSlipDetail.Click += new System.EventHandler(btnSlipDetail_Click);
		this.btnSlip220A.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip220A.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip220A.Name = "btnSlip220A";
		this.btnSlip220A.Size = new System.Drawing.Size(134, 24);
		this.btnSlip220A.Text = "220A-Party Slip";
		this.btnSlip220A.Click += new System.EventHandler(btnSlip220A_Click);
		this.btn220bSummary.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn220bSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn220bSummary.Name = "btn220bSummary";
		this.btn220bSummary.Size = new System.Drawing.Size(130, 24);
		this.btn220bSummary.Text = "220B-Summary";
		this.btn220bSummary.Click += new System.EventHandler(btn220bSummary_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(115, 24);
		this.btnPrint.Text = "103-&Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btn104Voucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn104Voucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn104Voucher.Name = "btn104Voucher";
		this.btn104Voucher.Size = new System.Drawing.Size(115, 24);
		this.btn104Voucher.Text = "104-Voucher";
		this.btn104Voucher.Click += new System.EventHandler(btn104Voucher_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.BtnLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoader.Name = "BtnLoader";
		this.BtnLoader.Size = new System.Drawing.Size(73, 24);
		this.BtnLoader.Text = "Load &GRN";
		this.BtnLoader.Click += new System.EventHandler(BtnLoader_Click);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.BtnSpecialRights.Image = Architecture.WinApp.Properties.Resources.RightsIcon;
		this.BtnSpecialRights.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSpecialRights.Name = "BtnSpecialRights";
		this.BtnSpecialRights.Size = new System.Drawing.Size(123, 24);
		this.BtnSpecialRights.Text = "Special Rights";
		this.BtnSpecialRights.Click += new System.EventHandler(BtnSpecialRights_Click);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(3, 32);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(192, 19);
		this.label34.TabIndex = 0;
		this.label34.Text = "Purchase Invoice         ";
		this.ChkBokVoucher.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBokVoucher.AutoSize = true;
		this.ChkBokVoucher.Checked = true;
		this.ChkBokVoucher.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBokVoucher.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBokVoucher.ForeColor = System.Drawing.Color.White;
		this.ChkBokVoucher.Location = new System.Drawing.Point(774, 33);
		this.ChkBokVoucher.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBokVoucher.Name = "ChkBokVoucher";
		this.ChkBokVoucher.Size = new System.Drawing.Size(80, 18);
		this.ChkBokVoucher.TabIndex = 28;
		this.ChkBokVoucher.Text = "Voucher";
		this.ChkBokVoucher.UseVisualStyleBackColor = true;
		this.ChkBoxParty.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxParty.AutoSize = true;
		this.ChkBoxParty.Checked = true;
		this.ChkBoxParty.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBoxParty.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxParty.ForeColor = System.Drawing.Color.White;
		this.ChkBoxParty.Location = new System.Drawing.Point(864, 33);
		this.ChkBoxParty.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxParty.Name = "ChkBoxParty";
		this.ChkBoxParty.Size = new System.Drawing.Size(140, 18);
		this.ChkBoxParty.TabIndex = 30;
		this.ChkBoxParty.Text = "220A-Party Print";
		this.ChkBoxParty.UseVisualStyleBackColor = true;
		this.ChkBoxItemPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxItemPrint.AutoSize = true;
		this.ChkBoxItemPrint.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxItemPrint.ForeColor = System.Drawing.Color.White;
		this.ChkBoxItemPrint.Location = new System.Drawing.Point(1009, 33);
		this.ChkBoxItemPrint.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxItemPrint.Name = "ChkBoxItemPrint";
		this.ChkBoxItemPrint.Size = new System.Drawing.Size(126, 18);
		this.ChkBoxItemPrint.TabIndex = 29;
		this.ChkBoxItemPrint.Text = "220-Item Print";
		this.ChkBoxItemPrint.UseVisualStyleBackColor = true;
		this.chkSummary.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkSummary.AutoSize = true;
		this.chkSummary.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSummary.ForeColor = System.Drawing.Color.White;
		this.chkSummary.Location = new System.Drawing.Point(1143, 33);
		this.chkSummary.Margin = new System.Windows.Forms.Padding(2);
		this.chkSummary.Name = "chkSummary";
		this.chkSummary.Size = new System.Drawing.Size(167, 18);
		this.chkSummary.TabIndex = 31;
		this.chkSummary.Text = "220B-Summary Print";
		this.chkSummary.UseVisualStyleBackColor = true;
		this.tabHistory.Controls.Add(this.panel10);
		this.tabHistory.Controls.Add(this.panel9);
		this.tabHistory.Controls.Add(this.panel3);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1319, 748);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel21);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 428);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1313, 317);
		this.panel10.TabIndex = 53;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 25);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1313, 292);
		this.panel12.TabIndex = 47;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1313, 292);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel21.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel21.Controls.Add(this.ctrlGrdBar3);
		this.panel21.Controls.Add(this.label32);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1313, 25);
		this.panel21.TabIndex = 46;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1273, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 2);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(355, 19);
		this.label32.TabIndex = 24;
		this.label32.Text = "Detail Information Of Above Selected Row";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 134);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1313, 294);
		this.panel9.TabIndex = 52;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		this.grdHistory.HeaderFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(165, 191, 225);
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1313, 294);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel3.Controls.Add(this.groupBox3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 59);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1313, 75);
		this.panel3.TabIndex = 0;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label59);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1258, 69);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(4, 36);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 19);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 145;
		this.label13.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1085, 45);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1085, 25);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1004, 25);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1004, 45);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(927, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(72, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(684, 19);
		this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(86, 15);
		this.label59.TabIndex = 142;
		this.label59.Text = "Supplier Name";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(583, 19);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(64, 15);
		this.label60.TabIndex = 141;
		this.label60.Text = "To Doc No";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(481, 19);
		this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(79, 15);
		this.label61.TabIndex = 140;
		this.label61.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(348, 19);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(210, 19);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(684, 36);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 5;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(583, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(481, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(348, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(210, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 1;
		this.panel14.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 31);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1313, 28);
		this.panel14.TabIndex = 29;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1273, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(8, 4);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(254, 19);
		this.label9.TabIndex = 24;
		this.label9.Text = "PURCHASE INVOICE HISTORY";
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1313, 28);
		this.panel6.TabIndex = 28;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefreshHistory, this.btnNewHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1313, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1327, 777);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "InvfrmPurchaseInvoice";
		this.Text = "EccountBook ERP ";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmPurchaseInvoice_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl3.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.tabPaymentSchedule.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.TabControlPanelForExpenseGrids.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.PartyAddLessAndEmptyBagGridParentPanel.ResumeLayout(false);
		this.EmptyBagGridPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel19.ResumeLayout(false);
		this.PartyAddLessGridPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel18.ResumeLayout(false);
		this.TransporterAndOtherItemExpenseParentPanel.ResumeLayout(false);
		this.OtherItemExpensePanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel17.ResumeLayout(false);
		this.TransporterGridPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
