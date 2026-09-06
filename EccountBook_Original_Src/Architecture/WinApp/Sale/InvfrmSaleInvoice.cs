using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.ModernHistories;
using Architecture.WinApp.PartyProcessing;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Sale;

public class InvfrmSaleInvoice : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtGrid = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtComm = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtGridHistory = new DataTable();

	private DataTable dtGridDetailHistory = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtCommType = new DataTable();

	private DataTable dtCommDrAc = new DataTable();

	public DataTable dtBranch = new DataTable();

	private int Id;

	private int VoucherHeadId;

	private bool Approved;

	private string ScreenName = "InvfrmSaleInvoice";

	private int DocumentTypeId = 95;

	private string ItemIdsForComm = "";

	private double LedgerBalance;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int BaseCurrencyId;

	private double BaseCurrencyRate;

	private string CheckReservedNotReservedOnInsertOnly = "";

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool HasMultiCurrencyFeature;

	private bool BranchFeature;

	private bool hasSaleOrder;

	private bool IsCommgrdLock;

	private bool BranchImplemented;

	private bool EnableItemWiseComm;

	private bool CommissionDebitToExpenses;

	private bool FreightDebitToExpenses;

	private bool CommissionPolicyIsActive;

	private bool CommissionPolicyForCommissionAgentIsActive;

	private bool RateCutAndRateCutAmountAddOnInvoice;

	private bool CreditAmountInItemSaleGL;

	private bool IsStockReservedPerParty;

	private int DefaultDaysToLessFromHistoryFromDate;

	private static bool PCalculateByPercent = false;

	private static bool PCalculateByAmount = true;

	private frmLoadGDN LoadGDN;

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

	private ToolStripButton btnAttachment;

	private ToolStripButton toolStripButton3;

	private Label label20;

	private Label label26;

	private Label label11;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	private Label label7;

	private TextBox txtrefno;

	private Label label12;

	private TextBox txtbillno;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private CtrlGrdBar ctrlGrdBar1;

	private Timer timer1;

	internal GridEX grd;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	internal GridEX grdFreight;

	private TextBox txtBillAmount;

	private Label label3;

	internal UltraCombo cmbsuppliername;

	internal UltraCombo cmbcommagent;

	private Label labelRemarks;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Panel panel13;

	private ToolStripButton toolStripButton1;

	private ToolStrip toolStrip2;

	private TextBox txtDueDays;

	private Label label22;

	private DateTimePicker DueDate;

	private CheckBox ChkBok;

	private ToolStripButton btnDelete;

	internal UltraCombo cmbcommtype;

	internal UltraCombo cmbcommuom;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnSlipFormat2;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdDetail;

	private CheckBox ChkPrintSlip;

	internal UltraCombo CmbCommissionDebitAc;

	private Label lblDebitAccount;

	private RichTextBox txtcommremarks;

	private Panel panel7;

	private Panel panel9;

	private Panel panel4;

	private Panel panel10;

	private Panel panel20;

	private Panel panel21;

	private Label label2;

	private Panel panel22;

	private Panel panel23;

	private Label label5;

	private Panel panel15;

	private Panel panel18;

	internal GridEX grdComm;

	private Panel panel19;

	private Label label6;

	private Panel panel16;

	private Panel panel17;

	private Label label1;

	private Panel panel8;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar6;

	private CtrlGrdBar ctrlGrdBar7;

	private CtrlGrdBar ctrlGrdBar5;

	private CtrlGrdBar ctrlGrdBar4;

	private Panel panel5;

	private Label label9;

	private Label label10;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel26;

	private Label label14;

	private Panel panel6;

	internal GridEX grdHistory;

	private Panel panel24;

	private Label label13;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label53;

	private Label label54;

	private Label label55;

	private Label label56;

	private Label label57;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Label label15;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private TextBox txtRemarks;

	private Label label43;

	private TextBox txtExchangeRate;

	private TextBox txtFcyAmount;

	private Label label44;

	internal UltraCombo cmbCurrency;

	private Label label45;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnshortcutkeys;

	private TextBox txtBranchSrNo;

	internal UltraCombo cmbBranchName;

	private Label label4;

	private TextBox txtLedgerBalance;

	private TextBox txtLedgerBalanceAfterBill;

	private Label label23;

	private Label label18;

	private Label label16;

	private Button btnRecordsUpdate;

	private Label labelLocationType;

	internal UltraCombo cmbLocationType;

	private CheckBox chkCustomAccounts;

	private ToolStripButton BtnItemSlip;

	private CheckBox ChkOpenADOForm;

	private LinkLabel AdvanceDoFormLink;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private TabPage tabPaymentDetail;

	private Panel panel3;

	private Label label24;

	private CtrlGrdBar ctrlGrdBar8;

	internal GridEX grdPaymentDetail;

	public InvfrmSaleInvoice(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
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
			base.AutoScrollMinSize = new Size(1100, 800);
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
			dtCommType.Columns.Add("Id");
			dtCommType.Columns.Add("CommissionType");
			dtGrid.Columns.Add("OtherCategoryId");
			dtGrid.Columns.Add("CategoryII");
			dtGrid.Columns.Add("RefRefDocumentTypeId");
			dtGrid.Columns.Add("RefRefDocIdNo");
			dtGrid.Columns.Add("RefDocSubId");
			dtGrid.Columns.Add("PurchaseGLAC");
			dtGrid.Columns.Add("Id");
			dtGrid.Columns.Add("InvGdnId");
			dtGrid.Columns.Add("ItemId");
			dtGrid.Columns.Add("ItemCode");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("BrandItemId");
			dtGrid.Columns.Add("BrandItemCode");
			dtGrid.Columns.Add("BrandItemName");
			dtGrid.Columns.Add("CropYear");
			dtGrid.Columns.Add("ItemUomId");
			dtGrid.Columns.Add("UOMCodeItem");
			dtGrid.Columns.Add("PackEquivalent");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("Warehouse", typeof(string));
			dtGrid.Columns.Add("PackingTypeId");
			dtGrid.Columns.Add("SaleOrderId");
			dtGrid.Columns.Add("SaleOrder");
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("AddLsWt", typeof(double));
			dtGrid.Columns.Add("EBWPerUnit", typeof(double));
			dtGrid.Columns.Add("EmptyBagsDeduction", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("StockWeight", typeof(double));
			dtGrid.Columns.Add("ItemRateWOExp", typeof(double));
			dtGrid.Columns.Add("PackingAddLess", typeof(double));
			dtGrid.Columns.Add("ItemRate", typeof(double));
			dtGrid.Columns.Add("OrderItemRateUOMId");
			dtGrid.Columns.Add("RateUOM");
			dtGrid.Columns.Add("RateEquivalent", typeof(double));
			dtGrid.Columns.Add("RateCut", typeof(double));
			dtGrid.Columns.Add("RateCutAmount", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("FcyAmount", typeof(double));
			dtGrid.Columns.Add("ItemDiscount", typeof(double));
			dtGrid.Columns.Add("DiscAmount", typeof(double));
			dtGrid.Columns.Add("JobLotId");
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("CommOnSale", typeof(bool));
			dtGrid.Columns.Add("CommPercent", typeof(decimal));
			dtGrid.Columns.Add("BranchId", typeof(int));
			dtGrid.Columns.Add("BranchName", typeof(string));
			dtGrid.Columns.Add("CostCenterId", typeof(int));
			dtGrid.Columns.Add("SaleOrderPaymentTermId", typeof(int));
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
			dtFreight.Columns.Add("InvGdnId");
			dtFreight.Columns.Add("GlAccountId");
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
			dtFreight.Columns.Add("Remarks");
			dtComm.Columns.Add("Id");
			dtComm.Columns.Add("OrderId");
			dtComm.Columns.Add("OrderNo");
			dtComm.Columns.Add("InvoiceId");
			dtComm.Columns.Add("CommissionAgentId");
			dtComm.Columns.Add("CommType");
			dtComm.Columns.Add("CommRate", typeof(double));
			dtComm.Columns.Add("CommUom");
			dtComm.Columns.Add("CommissionAmount", typeof(double));
			dtComm.Columns.Add("DebitAccountId");
			dtComm.Columns.Add("Remarks");
			dtComm.Columns.Add("GlAccountId");
			dtPaymentTerm.Columns.Add("SaleOrderId", typeof(int));
			dtPaymentTerm.Columns.Add("SaleOrderNo", typeof(int));
			dtPaymentTerm.Columns.Add("ItemAmount", typeof(double));
			dtPaymentTerm.Columns.Add("Expense", typeof(double));
			dtPaymentTerm.Columns.Add("Freight", typeof(double));
			dtPaymentTerm.Columns.Add("PartyAddLessAmount", typeof(double));
			dtPaymentTerm.Columns.Add("Commission", typeof(double));
			dtPaymentTerm.Columns.Add("NetBillAmount", typeof(double));
			dtPaymentTerm.Columns.Add("PaymentTerm", typeof(int));
			dtPaymentTerm.Columns.Add("DueDays", typeof(int));
			dtPaymentTerm.Columns.Add("DueDate", typeof(DateTime));
			dtPaymentTerm.Columns.Add("%OfTotal", typeof(decimal));
			dtPaymentTerm.Columns.Add("Amount", typeof(decimal));
			dtPaymentTerm.Columns.Add("Remarks", typeof(string));
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
			ScreenName = base.Name;
			BranchFeature = CommonServices.GetERPFeatureById(11);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			GetConfigurationsFromGlobal();
			DataTable locationType = VoucherHead.GetLocationType();
			string documentNoData = GenerateDocumentNo();
			DataTable currencydata = CurrencyDbCall();
			HistoryCombosBranchDbCall();
			DataTable historyComboData = HistoryComboDbCall(ValidateBranch: false);
			string branchSrNo = "";
			if (BranchFeature)
			{
				branchSrNo = GenerateBranchSrNo();
			}
			OtherItemsdtFillDbCall();
			return new
			{
				LocationTypeData = locationType,
				documentNoData = documentNoData,
				HistoryComboData = historyComboData,
				currencydata = currencydata,
				BranchSrNo = branchSrNo
			};
		}).ContinueWith(t =>
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
				btnPrint.Enabled = formright.DoHavePrintRights;
				toolStripButton1.Enabled = formright.DoHavePrintRights;
				btnSlipFormat2.Enabled = formright.DoHavePrintRights;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Visible = formright.DoHaveCanDelete;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar3, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar4, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar5, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar6, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar7, formright);
				if (Id == 0)
				{
					txtdocno.Text = result.documentNoData;
					txtBranchSrNo.Text = result.BranchSrNo;
				}
				LocationTypeBind(result.LocationTypeData);
				CurrencyBind(result.currencydata);
				SupplierDtFillFromGlobal();
				BindSupplierName();
				AccountlstDtFillFromGlobal();
				CommissionDebitAccountBindFromGlobal();
				CommissionTypeFill();
				CommissionUOMFill();
				MultiCurrencyFeature();
				OtherItemsdtFillDbCall();
				ImplementConfiguration();
				BindGrids();
				tabControl1.TabPages.Add("Modren_History");
				InvoiceHistory value = new InvoiceHistory(UserAccount)
				{
					InvoiceType = 2,
					DocumentTypeId = DocumentTypeId,
					RequestFromScreen = "Sale Invoice",
					TopLevel = false,
					Visible = true,
					FormBorderStyle = FormBorderStyle.None,
					Dock = DockStyle.Fill
				};
				tabControl1.TabPages[2].Controls.Add(value);
				HistoryCombosBranchBind();
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				DocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
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
			DataTable dt = InvSaleInvoice.GetBranchsAllocatedToUserFromSaleInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, DocumentTypeId);
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
			return InvSaleInvoice.AllComboBindAgainstSaleInvoice(obj);
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
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dt, "Id", "ReferenceName", "Customer Name", AllColumns: false, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			RateCutAndRateCutAmountAddOnInvoice = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("RateCutAndRateCutAmountAddOnInvoice"));
			FreightDebitToExpenses = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DebitAmountChargetoExpenseAcFreightGrid"));
			CommissionDebitToExpenses = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DebitAmountChargetoExpenseAcOfCommission"));
			IsCommgrdLock = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CommissionEditableOnInvoice"));
			EnableItemWiseComm = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("EnableItemWiseCommOnSale"));
			CommissionPolicyIsActive = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CommissionPolicyIsActive"));
			IsStockReservedPerParty = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsStockReservedPerParty"));
			CommissionPolicyForCommissionAgentIsActive = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CommissionPolicyForCommissionAgentIsActive"));
			BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleInvoiceBranchWise"));
			CreditAmountInItemSaleGL = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CreditAmountInItemSaleGL"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			BaseCurrencyId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
			BaseCurrencyRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BaseCurrencyRate"));
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
			txtBranchSrNo.Visible = BranchFeature && BranchImplemented;
			txtdocno.Width = ((BranchFeature && BranchImplemented) ? 80 : 137);
			((Control)(object)CmbCommissionDebitAc).Visible = false;
			lblDebitAccount.Visible = false;
			txtcommremarks.Height = 78;
			if (CommissionDebitToExpenses)
			{
				((Control)(object)CmbCommissionDebitAc).Visible = true;
				lblDebitAccount.Visible = true;
				txtcommremarks.Height = 51;
			}
			if (Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				cmbCurrency.Value = BaseCurrencyId;
			}
			if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
			{
				txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			((Control)(object)cmbBranchName).Enabled = !BranchImplemented;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string GenerateDocumentNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private string GenerateBranchSrNo()
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
			return Conversion.ToString((object)InvSaleInvoice.GenerateInvSaleInvoiceCodeBranch(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
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
					where r.PartyTypeId == 2
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
			InfragisticsHelper.BindAndRetainSelection(cmbsuppliername, dtSupplier, "Id", "CompanyName", "Customer Name", AllColumns: true, Conversion.ToInt(cmbsuppliername.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbcommagent, dtSupplier, "Id", "CompanyName", "Commission Agent", AllColumns: true, Conversion.ToInt(cmbcommagent.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			DataTable dataTable = dtSupplier;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)cmbcommagent).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbcommagent).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			dtCommType.Rows.Add(1, "Flat");
			dtCommType.Rows.Add(2, "Percent");
			dtCommType.Rows.Add(3, "Comm Weight");
			DDL.BindDDL(dtCommType, cmbcommtype, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionDebitAccountBindFromGlobal()
	{
		try
		{
			dtCommDrAc.Clear();
			if (!CommissionDebitToExpenses)
			{
				return;
			}
			if (SubsidiaryAccountAllownOnVouchers)
			{
				List<getGlobalSupplierCustomer> lst = (from r in clsGlobalVariables.globalSupplierCustomer.AsEnumerable()
					where r.PartyTypeId == 2
					select r).ToList();
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtCommDrAc.Rows.Add(Conversion.ToInt((object)lst[i].Id), Conversion.ToString((object)lst[i].CompanyName), Conversion.ToString((object)lst[i].PartyCode));
					}
				}
			}
			else
			{
				List<getGlobalAllAccountsWithCustomGroup> obj = clsGlobalVariables.AllAccountsWithCustomGroupId;
				if (obj != null && obj.Count > 0)
				{
					for (int j = 0; j < obj.Count; j++)
					{
						dtCommDrAc.Rows.Add(Conversion.ToInt((object)obj[j].ChartOfAccountId), Conversion.ToString((object)obj[j].AccountTitle), Conversion.ToString((object)obj[j].AccountCode));
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCommissionDebitAc, dtCommDrAc, "Id", "AccountTitle", "Debit Account", AllColumns: true, Conversion.ToInt(CmbCommissionDebitAc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			List<getGlobalVendorsAndCustomersForTransporter> lst = clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters;
			dtAccountlst.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtAccountlst.Rows.Add(lst[i].GlAccountId, lst[i].Id, lst[i].CompanyName);
					}
				}
				return;
			}
			if (FreightDebitToExpenses)
			{
				int[] excludedIds2 = new int[3] { 2, 15, 22 };
				IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => !excludedIds2.Contains(r.AccountTypeId));
				HashSet<int> existingIds = new HashSet<int>();
				{
					foreach (getGlobalAllAccountsWithCustomGroup acc in enumerable)
					{
						int id = Conversion.ToInt((object)acc.ChartOfAccountId);
						if (!existingIds.Contains(id))
						{
							dtAccountlst.Rows.Add(acc.ChartOfAccountId, 0, acc.AccountTitle);
						}
						existingIds.Add(id);
					}
					return;
				}
			}
			int[] excludedIds = new int[9] { 2, 11, 12, 13, 14, 15, 20, 21, 22 };
			IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable2 = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => !excludedIds.Contains(r.AccountTypeId));
			HashSet<int> existingIds2 = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc2 in enumerable2)
			{
				int id2 = Conversion.ToInt((object)acc2.ChartOfAccountId);
				if (!existingIds2.Contains(id2))
				{
					dtAccountlst.Rows.Add(acc2.ChartOfAccountId, 0, acc2.AccountTitle);
				}
				existingIds2.Add(id2);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable CurrencyDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return MultiCurrency.GetAll(new MultiCurrency
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

	private void CurrencyBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbCurrency, dt, "Id", "CurrencyCode", "Currency", AllColumns: true, Conversion.ToInt(cmbCurrency.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			cmbCurrency.Value = BaseCurrencyId;
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

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, DocumentTypeId);
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
				txtRemarks.Location = new Point(1004, 55);
				labelRemarks.Location = new Point(1004, 28);
				txtRemarks.Size = new Size(243, 53);
				labelRemarks.Size = new Size(243, 23);
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				label43.Visible = false;
				label44.Visible = false;
				label45.Visible = false;
				txtRemarks.Location = new Point(794, 55);
				labelRemarks.Location = new Point(794, 28);
				txtRemarks.Size = new Size(453, 53);
				labelRemarks.Size = new Size(453, 23);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0 || Conversion.ToDouble((object)txtExchangeRate.Text) != 0.0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrencyId)
			{
				DataTable dt = VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = DocumentTypeId.ToString(),
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
				txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
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

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			CheckReservedNotReservedOnInsertOnly = "";
			ChkOpenADOForm.Visible = false;
			ChkOpenADOForm.Checked = false;
			AdvanceDoFormLink.Visible = false;
			Id = 0;
			Approved = false;
			ItemIdsForComm = string.Empty;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			chkCustomAccounts.Checked = false;
			DocDate.Focus();
			((Control)(object)cmbsuppliername).Enabled = true;
			((Control)(object)cmbsuppliername).Text = string.Empty;
			txtbillno.Text = string.Empty;
			txtrefno.Text = string.Empty;
			txtDueDays.Text = string.Empty;
			txtLedgerBalance.Text = "0";
			txtBillAmount.Text = "0";
			txtLedgerBalanceAfterBill.Text = "0";
			txtSupplierGLId.Text = string.Empty;
			dtGrid.Rows.Clear();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtFreight.Rows.Clear();
			dtComm.Rows.Clear();
			dtPaymentTerm.Rows.Clear();
			BindGrids();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			txtdocno.Text = GenerateDocumentNo();
			txtBranchSrNo.Text = GenerateBranchSrNo();
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "MultiCurrency", "AllCities", "CropYear", "JobLot", "InvPackingType" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			LocationTypeBind(VoucherHead.GetLocationType());
			SupplierDtFillFromGlobal();
			BindSupplierName();
			CommissionDebitAccountBindFromGlobal();
			AccountlstDtFillFromGlobal();
			CurrencyBind(CurrencyDbCall());
			OtherItemsdtFillDbCall();
			ImplementConfiguration();
			GridComboBind();
			if (CommissionDebitToExpenses)
			{
				grdComm.RootTable.Columns["DebitAccountId"].Visible = true;
				grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)4;
			}
			else
			{
				grdComm.RootTable.Columns["DebitAccountId"].Visible = false;
				grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmSaleInvoice_FormClosing(object sender, FormClosingEventArgs e)
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

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbLocationType).DataSource != null && (((UltraGridBase)cmbLocationType).ActiveRow == null || Conversion.ToInt(cmbLocationType.Value) == 0))
		{
			MessageBox.Show("Location Type Field is Required");
			cmbLocationType.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == null || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo  field is Required");
			txtdocno.Focus();
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
		if (((UltraGridBase)cmbsuppliername).ActiveRow == null)
		{
			MessageBox.Show("Customer Name field is Required");
			cmbsuppliername.Focus();
			return false;
		}
		return true;
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
				AddRowInGLGrid();
				grdGLedger.DataSource = dtGrdGL;
				grdGLedger.RetrieveStructure();
				gridGLSettings();
			}
			if (dtInvExp != null && dtInvExp.Columns.Count > 0)
			{
				AddRowInvExpGrid();
				grdInvExp.DataSource = dtInvExp;
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
			}
			if (dtFreight != null && dtFreight.Columns.Count > 0)
			{
				AddRowInFreightGrid();
				grdFreight.DataSource = dtFreight;
				grdFreight.RetrieveStructure();
				grdFreightSettings();
			}
			if (dtComm != null && dtComm.Columns.Count > 0)
			{
				AddRowInCommissionGrid();
				grdComm.DataSource = dtComm;
				grdComm.RetrieveStructure();
				grdCommissionSettings();
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

	private void AddRowInFreightGrid()
	{
		try
		{
			dtFreight.Rows.Add(0, 0, 0, 0, 0, "");
			grdFreight.DataSource = dtFreight;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreightSettings()
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.RootTable.Columns["GlAccountId"].Visible = false;
			grdFreight.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["InvGdnId"].Visible = false;
			if (!FreightDebitToExpenses)
			{
				grdFreight.RootTable.Columns["Debit"].Visible = false;
			}
			else
			{
				grdFreight.RootTable.Columns["Debit"].Visible = true;
			}
			grdFreight.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item2 in (JanusCollectionBase)grdFreight.RootTable.Columns)
			{
				item2.HeaderAlignment = (TextAlignment)2;
			}
			grdFreight.RootTable.Columns["Transporter"].Width = Constants.InventoryConstants.StringConstant;
			grdFreight.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.FreightConstant;
			grdFreight.RootTable.Columns["Debit"].Width = Constants.InventoryConstants.AmountConstant;
			grdFreight.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SupplierCustomerId", "AccountTitle");
			}
			else
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			}
			grdFreight.RootTable.Columns.Add("Delete");
			grdFreight.RootTable.Columns["Delete"].Key = "Delete";
			grdFreight.RootTable.Columns["Delete"].Caption = "X";
			grdFreight.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Delete"].ButtonText = "X";
			grdFreight.RootTable.Columns["Delete"].Width = 20;
			grdFreight.RootTable.Columns.Add("Add");
			grdFreight.RootTable.Columns["Add"].Key = "Add";
			grdFreight.RootTable.Columns["Add"].Caption = "+";
			grdFreight.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Add"].ButtonText = "+";
			grdFreight.RootTable.Columns["Add"].Width = 20;
			grdFreight.RootTable.Columns["Add"].Position = 0;
			grdFreight.RootTable.Columns["Delete"].Position = 1;
			grdFreight.FrozenColumns = 2;
			ctrlGrdBar4_Load(null, null);
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				GridEXRow[] rows = grdFreight.GetRows();
				foreach (GridEXRow item in rows)
				{
					int selectedAcId = Conversion.ToInt(item.Cells["GlAccountId"].Value);
					if (selectedAcId != 0)
					{
						DataRow[] matchingRows = dtAccountlst.Select("Id=" + selectedAcId);
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["Transporter"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			SupCustIdUpdateforFrieghtGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdFreight.CurrentRow;
				dtFreight.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
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
			FreightProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void grdFreight_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Freight" || e.Column.Key == "Debit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
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
			if (e.Column.Key == "Transporter")
			{
				Conversion.ToInt(item.Cells["InvGdnId"].Value);
				_ = 0;
				SupCustIdUpdateforFrieghtGrid();
			}
			FreightProportion();
			BillAmount();
			grdFreight.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdComm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdComm.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["OrderId"].Value) > 0)
				{
					if (grdComm.GetRows().Length == 1)
					{
						MessageBox.Show("You Can not Delete Row Because The Above Entry Is From Sale Order");
						return;
					}
					dtComm.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtComm.Rows.Count == 0)
					{
						AddRowInCommissionGrid();
					}
				}
				else
				{
					dtComm.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtComm.Rows.Count == 0)
					{
						AddRowInCommissionGrid();
					}
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInCommissionGrid();
			}
			grdComm.UpdateData();
			PropotionateCommissiongrid();
			CommissionAmountCalculateInCaseofPolicy();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PropotionateCommissiongrid()
	{
		try
		{
			if (!CommissionDebitToExpenses && !CommissionPolicyIsActive && !CommissionPolicyForCommissionAgentIsActive)
			{
				double TotalItemAmount = 0.0;
				double TotalItemAmountForCommOnSale = 0.0;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow row2 in rows)
				{
					if (EnableItemWiseComm)
					{
						TotalItemAmount += Conversion.ToDouble(row2.Cells["ItemAmount"].Value);
						row2.BeginEdit();
						row2.Cells["Commission"].Value = 0;
						row2.EndEdit();
					}
					else
					{
						TotalItemAmount += Conversion.ToDouble(row2.Cells["ItemAmount"].Value);
						row2.BeginEdit();
						row2.Cells["Commission"].Value = 0;
						row2.EndEdit();
					}
				}
				rows = grdComm.GetRows();
				foreach (GridEXRow row in rows)
				{
					int ComgrdOrderId = Conversion.ToInt(row.Cells["OrderId"].Value);
					double OrderItemAmount = Conversion.ToDouble((object)GridEX_Helper.GetColumnSumWithWhere(grd, "ItemAmount", "SaleOrderId", Conversion.ToString((object)ComgrdOrderId)));
					Dictionary<string, string> conditions = new Dictionary<string, string>
					{
						{
							"SaleOrderId",
							ComgrdOrderId.ToString()
						},
						{ "CommOnSale", "true" }
					};
					GridEX_Helper.GetColumnSumWithWhere(grd, "Amount", conditions);
					GridEXRow[] rows2 = grd.GetRows();
					foreach (GridEXRow item in rows2)
					{
						if (EnableItemWiseComm)
						{
							if (!Conversion.ToBool(item.Cells["CommOnSale"].Value))
							{
								continue;
							}
							int saleOrderId = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
							if (ComgrdOrderId > 0)
							{
								if (Conversion.ToInt(row.Cells["OrderId"].Value) == saleOrderId)
								{
									double PreCommission = Conversion.ToDouble(item.Cells["Commission"].Value);
									double itemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
									double NetBillWeight = Conversion.ToDouble(item.Cells["NetBillWeight"].Value);
									item.BeginEdit();
									if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Percent")
									{
										double percent = Conversion.ToDouble(row.Cells["CommRate"].Value);
										double commission = itemAmount * percent / 100.0;
										item.Cells["Commission"].Value = commission + PreCommission;
									}
									else if (row.Cells["CommType"].Text.ToString() == "Comm Weight")
									{
										double commissionRate = Conversion.ToDouble(row.Cells["CommRate"].Value);
										double commissionUom = Conversion.ToDouble((object)row.Cells["CommUom"].Text);
										double commissionAmount = NetBillWeight / commissionUom * commissionRate;
										item.Cells["Commission"].Value = commissionAmount + PreCommission;
									}
									else if (row.Cells["CommType"].Text.ToString() == "Flat")
									{
										double rowCommission = Conversion.ToDouble(row.Cells["CommRate"].Value) / TotalItemAmount * itemAmount;
										item.Cells["Commission"].Value = rowCommission + PreCommission;
									}
									item.EndEdit();
								}
							}
							else
							{
								double PreCommission2 = Conversion.ToDouble(item.Cells["Commission"].Value);
								double itemAmount2 = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
								double NetBillWeight2 = Conversion.ToDouble(item.Cells["NetBillWeight"].Value);
								item.BeginEdit();
								if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Percent")
								{
									double percent2 = Conversion.ToDouble(row.Cells["CommRate"].Value);
									double commission2 = itemAmount2 * percent2 / 100.0;
									item.Cells["Commission"].Value = commission2 + PreCommission2;
								}
								else if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Comm Weight")
								{
									double commissionRate2 = Conversion.ToDouble(row.Cells["CommRate"].Value);
									double commissionUom2 = Conversion.ToDouble((object)row.Cells["CommUom"].Text);
									double commissionAmount2 = NetBillWeight2 / commissionUom2 * commissionRate2;
									item.Cells["Commission"].Value = commissionAmount2 + PreCommission2;
								}
								else if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Flat")
								{
									double rowCommission2 = Conversion.ToDouble(row.Cells["CommRate"].Value) / TotalItemAmountForCommOnSale * itemAmount2;
									item.Cells["Commission"].Value = rowCommission2 + PreCommission2;
								}
								item.EndEdit();
							}
							continue;
						}
						int saleOrderId2 = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
						if (ComgrdOrderId > 0)
						{
							if (Conversion.ToInt(row.Cells["OrderId"].Value) == saleOrderId2)
							{
								double PreCommission3 = Conversion.ToDouble(item.Cells["Commission"].Value);
								double itemAmount3 = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
								double NetBillWeight3 = Conversion.ToDouble(item.Cells["NetBillWeight"].Value);
								item.BeginEdit();
								if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Percent")
								{
									double percent3 = Conversion.ToDouble(row.Cells["CommRate"].Value);
									double commission3 = itemAmount3 * percent3 / 100.0;
									item.Cells["Commission"].Value = commission3 + PreCommission3;
								}
								else if (row.Cells["CommType"].Text.ToString() == "Comm Weight")
								{
									double commissionRate3 = Conversion.ToDouble(row.Cells["CommRate"].Value);
									double commissionUom3 = Conversion.ToDouble((object)row.Cells["CommUom"].Text);
									double commissionAmount3 = NetBillWeight3 / commissionUom3 * commissionRate3;
									item.Cells["Commission"].Value = commissionAmount3 + PreCommission3;
								}
								else if (row.Cells["CommType"].Text.ToString() == "Flat")
								{
									double rowCommission3 = Conversion.ToDouble(row.Cells["CommRate"].Value) / OrderItemAmount * itemAmount3;
									item.Cells["Commission"].Value = rowCommission3 + PreCommission3;
								}
								item.EndEdit();
							}
						}
						else
						{
							double PreCommission4 = Conversion.ToDouble(item.Cells["Commission"].Value);
							double itemAmount4 = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
							double NetBillWeight4 = Conversion.ToDouble(item.Cells["NetBillWeight"].Value);
							item.BeginEdit();
							if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Percent")
							{
								double percent4 = Conversion.ToDouble(row.Cells["CommRate"].Value);
								double commission4 = itemAmount4 * percent4 / 100.0;
								item.Cells["Commission"].Value = commission4 + PreCommission4;
							}
							else if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Comm Weight")
							{
								double commissionRate4 = Conversion.ToDouble(row.Cells["CommRate"].Value);
								double commissionUom4 = Conversion.ToDouble((object)row.Cells["CommUom"].Text);
								double commissionAmount4 = NetBillWeight4 / commissionUom4 * commissionRate4;
								item.Cells["Commission"].Value = commissionAmount4 + PreCommission4;
							}
							else if (Conversion.ToString((object)row.Cells["CommType"].Text) == "Flat")
							{
								double rowCommission4 = Conversion.ToDouble(row.Cells["CommRate"].Value) / TotalItemAmount * itemAmount4;
								item.Cells["Commission"].Value = rowCommission4 + PreCommission4;
							}
							item.EndEdit();
						}
					}
				}
			}
			else if (CommissionDebitToExpenses && !CommissionPolicyIsActive && !CommissionPolicyForCommissionAgentIsActive)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Commission"].Value = 0;
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateCommissionGrid()
	{
		try
		{
			if (grdComm.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			GridEXRow[] rows = grdComm.GetRows();
			foreach (GridEXRow item in rows)
			{
				int orderId = Conversion.ToInt(item.Cells["OrderId"].Value);
				double totalAmount = 0.0;
				double totalAmountForCommOnSale = 0.0;
				double totalWeight = 0.0;
				double totalWeightForCommOnSale = 0.0;
				GridEXRow[] rows2 = grd.GetRows();
				foreach (GridEXRow row in rows2)
				{
					if (EnableItemWiseComm)
					{
						if (!Conversion.ToBool(row.Cells["CommOnSale"].Value))
						{
							continue;
						}
						if (orderId > 0)
						{
							if (Conversion.ToInt(row.Cells["SaleOrderId"].Value) == orderId)
							{
								totalAmountForCommOnSale += Conversion.ToDouble(row.Cells["ItemAmount"].Value);
								totalWeightForCommOnSale += Conversion.ToDouble(row.Cells["NetBillWeight"].Value);
							}
						}
						else
						{
							totalAmountForCommOnSale += Conversion.ToDouble(row.Cells["ItemAmount"].Value);
							totalWeightForCommOnSale += Conversion.ToDouble(row.Cells["NetBillWeight"].Value);
						}
					}
					else if (orderId > 0)
					{
						if (Conversion.ToInt(row.Cells["SaleOrderId"].Value) == orderId)
						{
							totalAmount += Conversion.ToDouble(row.Cells["ItemAmount"].Value);
							totalWeight += Conversion.ToDouble(row.Cells["NetBillWeight"].Value);
						}
					}
					else
					{
						totalAmount += Conversion.ToDouble(row.Cells["ItemAmount"].Value);
						totalWeight += Conversion.ToDouble(row.Cells["NetBillWeight"].Value);
					}
				}
				item.BeginEdit();
				if (Conversion.ToString((object)item.Cells["CommType"].Text) == "Percent")
				{
					if (EnableItemWiseComm)
					{
						double commissionRate = Conversion.ToDouble(item.Cells["CommRate"].Value);
						double commissionAmount = totalAmountForCommOnSale * commissionRate / 100.0;
						if (CommissionDebitToExpenses)
						{
							item.Cells["CommissionAmount"].Value = Math.Round(commissionAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						}
						else
						{
							item.Cells["CommissionAmount"].Value = commissionAmount;
						}
					}
					else
					{
						double commissionRate2 = Conversion.ToDouble(item.Cells["CommRate"].Value);
						double commissionAmount2 = totalAmount * commissionRate2 / 100.0;
						if (CommissionDebitToExpenses)
						{
							item.Cells["CommissionAmount"].Value = Math.Round(commissionAmount2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						}
						else
						{
							item.Cells["CommissionAmount"].Value = commissionAmount2;
						}
					}
				}
				else if (item.Cells["CommType"].Text.ToString() == "Comm Weight")
				{
					if (EnableItemWiseComm)
					{
						double commissionRate3 = Conversion.ToDouble(item.Cells["CommRate"].Value);
						double commissionUom = Conversion.ToDouble((object)item.Cells["CommUom"].Text);
						double commissionAmount3 = totalWeightForCommOnSale / commissionUom * commissionRate3;
						if (CommissionDebitToExpenses)
						{
							item.Cells["CommissionAmount"].Value = Math.Round(commissionAmount3, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						}
						else
						{
							item.Cells["CommissionAmount"].Value = commissionAmount3;
						}
					}
					else
					{
						double commissionRate4 = Conversion.ToDouble(item.Cells["CommRate"].Value);
						double commissionUom2 = Conversion.ToDouble((object)item.Cells["CommUom"].Text);
						double commissionAmount4 = totalWeight / commissionUom2 * commissionRate4;
						if (CommissionDebitToExpenses)
						{
							item.Cells["CommissionAmount"].Value = Math.Round(commissionAmount4, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						}
						else
						{
							item.Cells["CommissionAmount"].Value = commissionAmount4;
						}
					}
				}
				else if (item.Cells["CommType"].Text.ToString() == "Flat")
				{
					double commissionAmount5 = Conversion.ToDouble(item.Cells["CommRate"].Value);
					if (EnableItemWiseComm)
					{
						if (CommissionDebitToExpenses)
						{
							item.Cells["CommissionAmount"].Value = Math.Round(commissionAmount5, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						}
						else
						{
							item.Cells["CommissionAmount"].Value = commissionAmount5;
						}
					}
					else if (CommissionDebitToExpenses)
					{
						item.Cells["CommissionAmount"].Value = Math.Round(commissionAmount5, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
					else
					{
						item.Cells["CommissionAmount"].Value = commissionAmount5;
					}
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdComm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdComm.UpdateData();
			if (e.Column.Key == "CommRate")
			{
				if (Conversion.ToString((object)grdComm.CurrentRow.Cells["CommType"].Text) == "Percent")
				{
					double TotalPercentage2 = 0.0;
					GridEXRow[] rows = grdComm.GetRows();
					foreach (GridEXRow item2 in rows)
					{
						TotalPercentage2 += Conversion.ToDouble(item2.Cells["CommRate"].Value);
					}
					if (TotalPercentage2 > 100.0)
					{
						double Percent2 = Conversion.ToDouble(grdComm.CurrentRow.Cells["CommRate"].Value);
						double j = TotalPercentage2 - 100.0;
						grdComm.CurrentRow.Cells["CommRate"].Value = Percent2 - j;
						MessageBox.Show("TotalPercentage Can not be Greater than 100...");
						grdComm.UpdateData();
					}
				}
				CalculateCommissionGrid();
			}
			if (e.Column.Key == "CommType")
			{
				if (Conversion.ToString((object)grdComm.CurrentRow.Cells["CommType"].Text) == "Flat" || Conversion.ToString((object)grdComm.CurrentRow.Cells["CommType"].Text) == "Percent")
				{
					grdComm.RootTable.Columns["CommUom"].Visible = false;
					if (Conversion.ToString((object)grdComm.CurrentRow.Cells["CommType"].Text) == "Percent")
					{
						double TotalPercentage = 0.0;
						GridEXRow[] rows = grdComm.GetRows();
						foreach (GridEXRow item in rows)
						{
							TotalPercentage += Conversion.ToDouble(item.Cells["CommRate"].Value);
						}
						if (TotalPercentage > 100.0)
						{
							double Percent = Conversion.ToDouble(grdComm.CurrentRow.Cells["CommRate"].Value);
							double i = TotalPercentage - 100.0;
							grdComm.CurrentRow.Cells["CommRate"].Value = Percent - i;
							MessageBox.Show("TotalPercentage Can not be Greater than 100...");
							grdComm.UpdateData();
						}
					}
				}
				else
				{
					grdComm.RootTable.Columns["CommUom"].Visible = true;
				}
				CalculateCommissionGrid();
			}
			if (e.Column.Key == "OrderNo")
			{
				GetOrderIdAgainstOrderNo();
				CalculateCommissionGrid();
				GetOrderIdAgainstOrderNo();
			}
			if (e.Column.Key == "DebitAccountId")
			{
				SupCustIdUpdateforCommissionGrid();
			}
			grdComm.UpdateData();
			PropotionateCommissiongrid();
			CommissionAmountCalculateInCaseofPolicy();
			BillAmount();
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInCommissionGrid()
	{
		try
		{
			dtComm.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, 0);
			grdComm.DataSource = dtComm;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCommissionSettings()
	{
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdComm.RootTable.Columns["Id"].EditType = (EditType)0;
			grdComm.RootTable.Columns["InvoiceId"].EditType = (EditType)0;
			grdComm.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommissionAgentId"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommType"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommRate"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommUom"].EditType = (EditType)0;
			grdComm.RootTable.Columns["CommissionAmount"].EditType = (EditType)0;
			grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)0;
			grdComm.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdComm.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdComm.RootTable.Columns["Id"].Visible = false;
			grdComm.RootTable.Columns["GlAccountId"].Visible = false;
			grdComm.RootTable.Columns["InvoiceId"].Visible = false;
			grdComm.RootTable.Columns["OrderId"].Visible = false;
			grdComm.RootTable.Columns["DebitAccountId"].Visible = false;
			grdComm.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdComm.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdComm.RootTable.Columns["CommissionAmount"].FormatString = "#,##0.####";
			grdComm.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.####";
			grdComm.RootTable.Columns["CommRate"].AggregateFunction = (AggregateFunction)3;
			grdComm.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdComm.RootTable.Columns["CommRate"].FormatString = "#,##0.###";
			grdComm.RootTable.Columns["CommRate"].TotalFormatString = "#,##0.###";
			grdComm.RootTable.Columns["CommissionAgentId"].ColumnType = (ColumnType)0;
			grdComm.RootTable.Columns["CommissionAgentId"].HasValueList = true;
			grdComm.RootTable.Columns["CommissionAgentId"].LimitToList = true;
			grdComm.RootTable.Columns["CommissionAgentId"].Caption = "Commission Agent";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdComm.RootTable.Columns["CommissionAgentId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SupplierCustomerId", "AccountTitle");
			}
			else
			{
				grdComm.RootTable.Columns["CommissionAgentId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			}
			DataTable dtCommUom = (DataTable)((UltraGridBase)cmbcommuom).DataSource;
			if (dtCommUom != null && dtCommUom != null && dtCommUom.Rows.Count > 0)
			{
				grdComm.RootTable.Columns["CommUom"].ColumnType = (ColumnType)0;
				grdComm.RootTable.Columns["CommUom"].HasValueList = true;
				grdComm.RootTable.Columns["CommUom"].LimitToList = false;
				grdComm.RootTable.Columns["CommUom"].ValueList.PopulateValueList((IEnumerable)dtCommUom.DefaultView, "Id", "type");
			}
			grdComm.RootTable.Columns["CommType"].ColumnType = (ColumnType)0;
			grdComm.RootTable.Columns["CommType"].HasValueList = true;
			grdComm.RootTable.Columns["CommType"].LimitToList = true;
			grdComm.RootTable.Columns["CommType"].ValueList.PopulateValueList((IEnumerable)dtCommType.DefaultView, "Id", "CommissionType");
			grdComm.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item2 in (JanusCollectionBase)grdComm.RootTable.Columns)
			{
				item2.HeaderAlignment = (TextAlignment)2;
			}
			grdComm.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DoNoConstant;
			grdComm.RootTable.Columns["CommissionAgentId"].Width = Constants.InventoryConstants.AccountTitle;
			grdComm.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.ProductionStage;
			grdComm.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdComm.RootTable.Columns["CommUom"].Width = Constants.InventoryConstants.UomConstant;
			grdComm.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdComm.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			CommissionDebitAccountBind();
			CommGridLock();
			ctrlGrdBar5_Load(null, null);
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				GridEXRow[] rows = grdComm.GetRows();
				foreach (GridEXRow item in rows)
				{
					int selectedAcId = Conversion.ToInt(item.Cells["GlAccountId"].Value);
					if (selectedAcId != 0)
					{
						DataRow[] matchingRows = dtAccountlst.Select("Id=" + selectedAcId);
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["DebitAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			SupCustIdUpdateforCommissionGrid();
			if (CommissionPolicyIsActive || CommissionPolicyForCommissionAgentIsActive)
			{
				grdComm.RootTable.Columns["OrderNo"].Visible = false;
				grdComm.RootTable.Columns["CommType"].Visible = false;
				grdComm.RootTable.Columns["CommUom"].Visible = false;
				grdComm.RootTable.Columns["CommRate"].Visible = false;
				grdComm.RootTable.Columns["CommissionAmount"].EditType = (EditType)0;
				grdComm.RootTable.Columns["CommissionAgentId"].EditType = (EditType)0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupCustIdUpdateforCommissionGrid()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdComm.UpdateData();
			GridEXRow r = grdComm.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				GridEXRow[] rows = grdComm.GetRows();
				foreach (GridEXRow item in rows)
				{
					string selectedAcId = Conversion.ToString((object)item.Cells["DebitAccountId"].Text);
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
			grdComm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionDebitAccountBind()
	{
		try
		{
			if (CommissionDebitToExpenses)
			{
				DataTable dtCommDrAc = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,11,12,13,14,20,21");
				grdComm.RootTable.Columns["DebitAccountId"].ColumnType = (ColumnType)0;
				grdComm.RootTable.Columns["DebitAccountId"].HasValueList = true;
				grdComm.RootTable.Columns["DebitAccountId"].LimitToList = true;
				grdComm.RootTable.Columns["DebitAccountId"].Caption = "Dr Account";
				if (SubsidiaryAccountAllownOnVouchers)
				{
					grdComm.RootTable.Columns["DebitAccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "SupplierCustomerId", "AccountTitle");
				}
				else
				{
					grdComm.RootTable.Columns["DebitAccountId"].ValueList.PopulateValueList((IEnumerable)dtCommDrAc.DefaultView, "Id", "AccountTitle");
				}
				grdComm.RootTable.Columns["DebitAccountId"].Width = 130;
				grdComm.RootTable.Columns["DebitAccountId"].Visible = true;
				grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)4;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommGridLock()
	{
		try
		{
			if (IsCommgrdLock)
			{
				if (!CommissionPolicyIsActive && !CommissionPolicyForCommissionAgentIsActive)
				{
					grdComm.RootTable.Columns.Add("Delete");
					grdComm.RootTable.Columns["Delete"].Key = "Delete";
					grdComm.RootTable.Columns["Delete"].Caption = "X";
					grdComm.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdComm.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
					grdComm.RootTable.Columns["Delete"].ButtonText = "X";
					grdComm.RootTable.Columns["Delete"].Visible = true;
					grdComm.RootTable.Columns.Add("Add");
					grdComm.RootTable.Columns["Add"].Key = "Add";
					grdComm.RootTable.Columns["Add"].Caption = "+";
					grdComm.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grdComm.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
					grdComm.RootTable.Columns["Add"].ButtonText = "+";
					grdComm.RootTable.Columns["Add"].Visible = true;
					grdComm.RootTable.Columns["Add"].Width = 20;
					grdComm.RootTable.Columns["Delete"].Width = 20;
					grdComm.RootTable.Columns["Add"].Position = 0;
					grdComm.RootTable.Columns["Delete"].Position = 1;
					grdComm.FrozenColumns = 2;
				}
				grdComm.RootTable.Columns["DebitAccountId"].EditType = (EditType)4;
				grdComm.RootTable.Columns["CommissionAgentId"].EditType = (EditType)4;
				grdComm.RootTable.Columns["CommType"].EditType = (EditType)4;
				grdComm.RootTable.Columns["CommRate"].EditType = (EditType)1;
				grdComm.RootTable.Columns["CommUom"].EditType = (EditType)4;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetOrderIdAgainstOrderNo()
	{
		try
		{
			int DocNo = Conversion.ToInt(grdComm.CurrentRow.Cells["OrderNo"].Value);
			int OrderId = 0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["SaleOrder"].Value) == DocNo)
				{
					OrderId = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
					break;
				}
			}
			if (OrderId > 0)
			{
				grd.UpdateData();
				grdComm.CurrentRow.Cells["OrderId"].Value = OrderId;
				grd.UpdateData();
			}
			else
			{
				grd.UpdateData();
				grdComm.CurrentRow.Cells["OrderId"].Value = 0;
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCommGridOrderNo()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdComm.CurrentRow;
			if (r == null || (int)r.RowType != 0 || Conversion.ToInt(r.Cells["OrderId"].Value) != 0)
			{
				return;
			}
			var list = (from GridEXRow row in grd.GetRows()
				where Conversion.ToInt(row.Cells["SaleOrderId"].Value) > 0
				select new
				{
					OrderId = Conversion.ToInt(row.Cells["SaleOrder"].Value)
				}).Distinct().ToList();
			DataTable dtOrderNo = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"OrderNo",
						typeof(int)
					}
				}
			};
			foreach (var order in list)
			{
				dtOrderNo.Rows.Add(order.OrderId, order.OrderId);
			}
			if (dtOrderNo.Rows.Count > 0)
			{
				GridEXColumn obj = grdComm.RootTable.Columns["OrderNo"];
				obj.ColumnType = (ColumnType)0;
				obj.EditType = (EditType)4;
				obj.HasValueList = true;
				obj.LimitToList = true;
				obj.ValueList.PopulateValueList((IEnumerable)dtOrderNo.DefaultView, "OrderNo", "OrderNo");
				obj.Width = 70;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdComm_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "CommRate" || e.Column.Key == "CommissionAmount") && !CommonServices.IsNumeric(e))
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
				if (!SubsidiaryAccountAllownOnVouchers && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
				{
					MessageBox.Show("Customer Account Not select");
					item.Cells["AccountId"].Value = 0;
					return;
				}
				SupCustIdUpdateforGLGrid();
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

	private void gridGLSettings()
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
			ctrlGrdBar7_Load(null, null);
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				GridEXRow[] rows = grdGLedger.GetRows();
				foreach (GridEXRow item in rows)
				{
					int selectedAcId = Conversion.ToInt(item.Cells["GlAccountId"].Value);
					if (selectedAcId != 0)
					{
						DataRow[] matchingRows = dtAccountlst.Select("Id=" + selectedAcId);
						if (matchingRows.Length != 0)
						{
							item.BeginEdit();
							item.Cells["AccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
							item.EndEdit();
						}
					}
				}
			}
			SupCustIdUpdateforGLGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void button1_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			try
			{
				grdInvExp.UpdateData();
				GridEXRow item = grdInvExp.CurrentRow;
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate2 = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					double Amount = num * Rate2;
					item.Cells["Amount"].Value = Amount;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			try
			{
				grdInvExp.UpdateData();
				BillAmount();
				ExpProportion();
				grdFreight.UpdateData();
				GridEXRow items = grdFreight.CurrentRow;
				if (items != null && (int)items.RowType == 0 && Conversion.ToDouble((object)items.Cells["Debit"].Value.ToString()) > 0.0)
				{
					items.Cells["Freight"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
				if (items != null && (int)items.RowType == 0 && Conversion.ToDouble((object)items.Cells["Freight"].Value.ToString()) > 0.0)
				{
					items.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
				grdFreight.UpdateData();
				BillAmount();
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
			}
			try
			{
				grdGLedger.UpdateData();
				GridEXRow Item = grdGLedger.CurrentRow;
				if (Item != null && (int)Item.RowType == 0 && Item.Cells["Qty"].Value.ToString() != string.Empty && Item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double Qty = Conversion.ToDouble((object)Item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)Item.Cells["Rate"].Value.ToString());
					Item.Cells["Credit"].Value = Qty * Rate;
					Item.Cells["Debit"].Value = 0;
					Item.Cells["Percentage"].Value = 0;
				}
				if (Item != null && (int)Item.RowType == 0 && Item.Cells["Percentage"].Value.ToString() != string.Empty)
				{
					double num2 = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)Item.Cells["Percentage"].Value.ToString());
					double TotlaPercentage = num2 / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						Item.Cells["Credit"].Value = Math.Round(TotlaPercentage);
						Item.Cells["Debit"].Value = 0;
					}
					else
					{
						Item.Cells["Debit"].Value = Math.Round(TotlaPercentage);
						Item.Cells["Credit"].Value = 0;
					}
					Item.Cells["Qty"].Value = 0;
					Item.Cells["Rate"].Value = 0;
				}
				if (Item != null && (int)Item.RowType == 0 && Conversion.ToDouble((object)Item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					Item.Cells["Credit"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
				if (Item != null && (int)Item.RowType == 0 && Conversion.ToDouble((object)Item.Cells["Credit"].Value.ToString()) > 0.0)
				{
					Item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
				grdGLedger.UpdateData();
				BillAmount();
				LedgerProportion();
			}
			catch (Exception ex3)
			{
				MessageBox.Show(ex3.Message);
			}
		}
		catch (Exception ex4)
		{
			MessageBox.Show(ex4.Message);
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
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
				{
					if (item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
					{
						double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
						double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
						double Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						item.Cells["Amount"].Value = Amount;
					}
				}
				else if (e.Column.Key == "Amount" && item.Cells["Amount"].Value.ToString() != string.Empty)
				{
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
					double Amount2 = Math.Round(Conversion.ToDouble(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Amount"].Value = Amount2;
				}
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

	private void grdInvExpSettings()
	{
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdInvExp.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdInvExp.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdInvExp.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdInvExp.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInvExp.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdInvExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExp.RootTable.Columns["Amount"].EditType = (EditType)1;
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
			ctrlGrdBar6_Load(null, null);
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
			List<string> hiddenColumns = new List<string> { "SaleOrderId" };
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
			List<string> NotTotalColumns = new List<string> { "ItemAmount", "Expense", "Freight", "PartyAddLessAmount", "Commission", "NetBillAmount" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPaymentDetail, 2, 3, hiddenColumns, editableColumns, null, ColumnSetting: true, BracketsForamt: false, NotTotalColumns);
			grdPaymentDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant + 25;
			grdPaymentDetail.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["PartyAddLessAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["Commission"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentDetail.RootTable.Columns["NetBillAmount"].Width = Constants.InventoryConstants.AmountConstant + 25;
			grdPaymentDetail.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant + 50;
			grdPaymentDetail.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdPaymentDetail.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdPaymentDetail.RootTable.Columns["%OfTotal"].Width = Constants.InventoryConstants.ExRateConstant + 20;
			grdPaymentDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant + 20;
			grdPaymentDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["ItemAmount"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["Expense"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["Freight"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["PartyAddLessAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["PartyAddLessAmount"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["Commission"].FormatString = "#,##0.###";
			grdPaymentDetail.RootTable.Columns["NetBillAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentDetail.RootTable.Columns["NetBillAmount"].FormatString = "#,##0.###";
			GridEX_Helper.AddButton(grdPaymentDetail, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdPaymentDetail, "Add", "+", 20, 1);
			ctrlGrdBar8_Load(null, null);
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
			int orderId = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
			int count = dtPaymentTerm.AsEnumerable().Count((DataRow r) => r.Field<int?>("SaleOrderId") == orderId);
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
				double BillAmount = Conversion.ToDouble(item.Cells["NetBillAmount"].Value);
				double sumOfAmount = Conversion.ToDouble(dtPaymentTerm.Compute("Sum(Amount)", $"SaleOrderId = '{orderId}'"));
				double sumOfPercent = Conversion.ToDouble(dtPaymentTerm.Compute("Sum([%OfTotal])", $"SaleOrderId = '{orderId}'"));
				double Rem = BillAmount - sumOfAmount;
				double RemPercent = 100.0 - sumOfPercent;
				if (Rem > 0.0 && RemPercent > 0.0)
				{
					DataRow dr = dtPaymentTerm.Rows[item.RowIndex];
					dtPaymentTerm.Rows.Add(dr.ItemArray);
					dtPaymentTerm.Rows[dtPaymentTerm.Rows.Count - 1]["Amount"] = Rem;
					dtPaymentTerm.Rows[dtPaymentTerm.Rows.Count - 1]["%OfTotal"] = RemPercent;
				}
				else
				{
					MessageBox.Show("Can't break further because percent Or Amount is Equal To Order Bill Amount:" + BillAmount.ToString("#,##0.###") + ".\n If you want to break further first change percent or amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPaymentDetail.UpdateData();
			GridEXRow item = grdPaymentDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (grd.GetRows().Count() > 0)
				{
					decimal billAmount = Conversion.ToDecimal(item.Cells["NetBillAmount"].Value);
					int SaleOrderId = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
					Conversion.ToInt(item.Cells["SaleOrderNo"].Value);
					List<GridEXRow> source = (from GridEXRow r in grdPaymentDetail.GetRows()
						where (int)r.RowType == 0 && Conversion.ToInt(r.Cells["SaleOrderId"].Value) == SaleOrderId
						select r).ToList();
					decimal totalPercentExceptCurrent = source.Where((GridEXRow r) => r != item).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["%OfTotal"].Value));
					decimal totalAmountExceptCurrent = source.Where((GridEXRow r) => r != item).Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Amount"].Value));
					decimal currentPercent = Conversion.ToDecimal(item.Cells["%OfTotal"].Value);
					decimal currentAmount = Conversion.ToDecimal(item.Cells["Amount"].Value);
					if (e.Column.Key == "%OfTotal")
					{
						if (totalPercentExceptCurrent + currentPercent > 100.01m)
						{
							MessageBox.Show("Total % cannot exceed 100 for this order");
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
							decimal Rem = billAmount - totalAmountExceptCurrent;
							currentAmount = ((Rem < 0m) ? 0m : Rem);
							item.Cells["Amount"].Value = BaseCurrencyRate;
							item.Cells["%OfTotal"].Value = 0;
						}
						currentPercent = currentAmount * 100m / billAmount;
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
							throw new Exception("Due Date Can't less Than DocDate");
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
			int orderId = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
			int count = dtPaymentTerm.AsEnumerable().Count((DataRow r) => r.Field<int?>("SaleOrderId") == orderId);
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
				else if (grdPaymentDetail.CurrentColumn.Key == "Add")
				{
					double BillAmount = Conversion.ToDouble(item.Cells["NetBillAmount"].Value);
					double sumOfAmount = Conversion.ToDouble(dtPaymentTerm.Compute("Sum(Amount)", $"SaleOrderId = '{orderId}'"));
					double sumOfPercent = Conversion.ToDouble(dtPaymentTerm.Compute("Sum([%OfTotal])", $"SaleOrderId = '{orderId}'"));
					double Rem = BillAmount - sumOfAmount;
					double RemPercent = 100.0 - sumOfPercent;
					if (Rem > 0.0 && RemPercent > 0.0)
					{
						DataRow dr = dtPaymentTerm.Rows[item.RowIndex];
						dtPaymentTerm.Rows.Add(dr.ItemArray);
						dtPaymentTerm.Rows[dtPaymentTerm.Rows.Count - 1]["Amount"] = Rem;
						dtPaymentTerm.Rows[dtPaymentTerm.Rows.Count - 1]["%OfTotal"] = RemPercent;
					}
					else
					{
						MessageBox.Show("Can't break further because percent Or Amount is Equal To Order Bill Amount:" + BillAmount.ToString("#,##0.###") + ".\n If you want to break further first change percent or amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			else if (e.Control && e.KeyCode == Keys.D)
			{
				double BillAmount2 = Conversion.ToDouble(item.Cells["NetBillAmount"].Value);
				double sumOfAmount2 = Conversion.ToDouble(dtPaymentTerm.Compute("Sum(Amount)", $"SaleOrderId = '{orderId}'"));
				double sumOfPercent2 = Conversion.ToDouble(dtPaymentTerm.Compute("Sum([%OfTotal])", $"SaleOrderId = '{orderId}'"));
				double Rem2 = BillAmount2 - sumOfAmount2;
				double RemPercent2 = 100.0 - sumOfPercent2;
				if (Rem2 > 0.0 && RemPercent2 > 0.0)
				{
					DataRow dr2 = dtPaymentTerm.Rows[item.RowIndex];
					dtPaymentTerm.Rows.Add(dr2.ItemArray);
					dtPaymentTerm.Rows[dtPaymentTerm.Rows.Count - 1]["Amount"] = Rem2;
					dtPaymentTerm.Rows[dtPaymentTerm.Rows.Count - 1]["%OfTotal"] = RemPercent2;
				}
				else
				{
					MessageBox.Show("Can't break further because percent Or Amount is Equal To Order Bill Amount:" + BillAmount2.ToString("#,##0.###") + ".\n If you want to break further first change percent or amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
					OrderId = Conversion.ToInt(row.Cells["SaleOrderId"].Value),
					OrderNo = Conversion.ToInt(row.Cells["SaleOrder"].Value),
					PaymentTermId = Conversion.ToInt(row.Cells["SaleOrderPaymentTermId"].Value)
				} into x
				where x.OrderId > 0
				select x).Distinct().ToList();
			if (!distinctOrders.Any())
			{
				return;
			}
			HashSet<int> existingOrderIds = (from GridEXRow r in grdPaymentDetail.GetRows()
				select Conversion.ToInt(r.Cells["SaleOrderId"].Value)).ToHashSet();
			DataTable dt = InvSaleInvoice.SaleOrderPaymentTermDetailBySoIds(string.Join(",", distinctOrders.Select(x => x.OrderId)));
			Dictionary<int, List<DataRow>> dbLookup = new Dictionary<int, List<DataRow>>();
			if (dt != null)
			{
				foreach (DataRow row2 in dt.Rows)
				{
					int id = Conversion.ToInt(row2["SaleOrderId"]);
					if (!dbLookup.TryGetValue(id, out var list))
					{
						list = (dbLookup[id] = new List<DataRow>());
					}
					list.Add(row2);
				}
			}
			foreach (var order in distinctOrders)
			{
				if (existingOrderIds.Contains(order.OrderId))
				{
					continue;
				}
				if (dbLookup.TryGetValue(order.OrderId, out var rows))
				{
					foreach (DataRow dr in rows)
					{
						AddPaymentRow(order.OrderId, Conversion.ToInt(dr["SaleOrderNo"]), Conversion.ToInt(dr["PaymentTermsId"]), Conversion.ToInt(dr["DueDays"]), Conversion.ToDateTime(dr["DueDate"]), Conversion.ToDecimal(dr["PrcntOfTotal"]));
					}
				}
				else
				{
					AddPaymentRow(order.OrderId, order.OrderNo, order.PaymentTermId, 0, DateTime.Now);
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

	private void AddPaymentRow(int saleOrderId, int saleOrderNo, int paymentTermsId, int dueDays, DateTime dueDate, decimal PrcntOfTotal = 0m)
	{
		dtPaymentTerm.Rows.Add(saleOrderId, saleOrderNo, 0, 0, 0, 0, 0, 0, paymentTermsId, dueDays, dueDate, PrcntOfTotal, 0, "");
	}

	private void CalculateOrderWiseAmountForPaymentDetail()
	{
		try
		{
			var distinctOrders = (from GridEXRow r in grd.GetRows()
				where Conversion.ToInt(r.Cells["SaleOrderId"].Value) > 0
				group r by new
				{
					OrderId = Conversion.ToInt(r.Cells["SaleOrderId"].Value),
					OrderNo = Conversion.ToInt(r.Cells["SaleOrder"].Value)
				} into g
				select new
				{
					OrderId = g.Key.OrderId,
					OrderNo = g.Key.OrderNo,
					TotalItemAmount = g.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["ItemAmount"].Value))
				}).ToList();
			int orderCount = distinctOrders.Count;
			if (orderCount == 0)
			{
				return;
			}
			int partyId = Conversion.ToInt(cmbsuppliername.Value);
			decimal journalDebit = default(decimal);
			decimal journalCredit = default(decimal);
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["AccountId"].Value) > 0)
				{
					journalDebit += Conversion.ToDecimal(r2.Cells["Debit"].Value);
					journalCredit += Conversion.ToDecimal(r2.Cells["Credit"].Value);
				}
			}
			decimal glDifference = journalCredit - journalDebit;
			int transporterId = (SubsidiaryAccountAllownOnVouchers ? partyId : CommonServices.GetGlAccountIdBySupplierCustomerId(partyId));
			IEnumerable<GridEXRow> source = from GridEXRow r in grdFreight.GetDataRows()
				where (int)r.RowType == 0 && Conversion.ToInt(r.Cells["Transporter"].Value) == transporterId
				select r;
			decimal freightCredit = source.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Freight"].Value));
			decimal freightDebit = source.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["Debit"].Value));
			decimal transDifference = Math.Round(freightCredit - freightDebit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			decimal totalExpense = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdInvExp, "Amount"));
			var gridLookup = (from r in dtGrid.AsEnumerable()
				group r by Conversion.ToInt(r["SaleOrderId"])).ToDictionary((IGrouping<int, DataRow> g) => g.Key, (IGrouping<int, DataRow> g) => new
			{
				ItemAmount = g.Sum((DataRow r) => Conversion.ToDecimal(r["ItemAmount"])),
				Expense = g.Sum((DataRow r) => Conversion.ToDecimal(r["Expense"])),
				Freight = g.Sum((DataRow r) => Conversion.ToDecimal(r["Freights"])),
				Commission = g.Sum((DataRow r) => Conversion.ToDecimal(r["Commission"]))
			});
			Dictionary<int, decimal> commissionLookup = (from r in (from GridEXRow r in grdComm.GetRows()
					where Conversion.ToInt(r.Cells["CommissionAgentId"].Value) == partyId
					select r).ToList()
				group r by Conversion.ToInt(r.Cells["OrderId"].Value)).ToDictionary((IGrouping<int, GridEXRow> g) => g.Key, (IGrouping<int, GridEXRow> g) => g.Sum((GridEXRow r) => Conversion.ToDecimal(r.Cells["CommissionAmount"].Value)));
			decimal globalCommission = (commissionLookup.ContainsKey(0) ? commissionLookup[0] : 0m);
			foreach (var order in distinctOrders)
			{
				gridLookup.TryGetValue(order.OrderId, out var sums);
				decimal perOrderJournal = default(decimal);
				decimal perOrderExpense = ((totalExpense != 0m) ? (totalExpense / (decimal)orderCount) : 0m);
				decimal TotalItemAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "ItemAmount"));
				decimal OrderItemAmount = order.TotalItemAmount;
				decimal num = Math.Abs(transDifference) / TotalItemAmount * OrderItemAmount;
				perOrderJournal = Math.Abs(glDifference) / TotalItemAmount * OrderItemAmount;
				decimal itemAmount = sums?.ItemAmount ?? 0m;
				decimal propExpense = sums?.Expense ?? 0m;
				_ = sums?.Freight;
				_ = sums?.Commission;
				decimal orderCommission = (commissionLookup.ContainsKey(order.OrderId) ? commissionLookup[order.OrderId] : 0m);
				if (globalCommission > 0m)
				{
					orderCommission += globalCommission / (decimal)orderCount;
				}
				decimal expenseAmount = ((propExpense > 0m) ? propExpense : perOrderExpense);
				decimal freightAmount = num;
				decimal netAmount = itemAmount + expenseAmount - orderCommission;
				netAmount = ((transDifference > 0m) ? (netAmount - freightAmount) : (netAmount + Math.Abs(freightAmount)));
				netAmount = ((glDifference > 0m) ? (netAmount + perOrderJournal) : (netAmount - Math.Abs(perOrderJournal)));
				DataRow[] array = dtPaymentTerm.Select($"SaleOrderId = {order.OrderId}");
				foreach (DataRow row in array)
				{
					row.SetField("ItemAmount", itemAmount);
					row.SetField("PartyAddLessAmount", perOrderJournal);
					row.SetField("Freight", freightAmount);
					row.SetField("Expense", expenseAmount);
					row.SetField("Commission", orderCommission);
					row.SetField("NetBillAmount", netAmount);
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

	private void PaymentAmountReCalculate()
	{
		try
		{
			foreach (IGrouping<int, GridEXRow> group in from r in (from GridEXRow r in grdPaymentDetail.GetRows()
					where (int)r.RowType == 0
					select r).ToList()
				group r by Conversion.ToInt(r.Cells["SaleOrderId"].Value))
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

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if ((e.Column.Key == "RateCut" || e.Column.Key == "ItemRateWOExp" || e.Column.Key == "RateEquivalent") && item != null && (int)item.RowType == 0)
			{
				string obj = item.Cells["NetBillWeight"].Value.ToString();
				double RateCut = Conversion.ToDouble(item.Cells["RateCut"].Value);
				double Rate = Conversion.ToDouble(item.Cells["ItemRateWOExp"].Value);
				if (Conversion.ToDouble((object)RateCut) > 0.0)
				{
					item.Cells["ItemRate"].Value = Rate - Math.Abs(RateCut);
				}
				else
				{
					item.Cells["ItemRate"].Value = Rate + Math.Abs(RateCut);
				}
				double EquivalentPoRate = Conversion.ToDouble((object)item.Cells["RateEquivalent"].Text);
				double RateCutAmount = Conversion.ToDouble((object)obj) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)RateCut);
				RateCutAmount = Math.Round(RateCutAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["RateCutAmount"].Value = RateCutAmount;
				double ItemAmount = Math.Round(Conversion.ToDouble((object)obj) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble(item.Cells["ItemRate"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["ItemAmount"].Value = ItemAmount;
				double Expense = Conversion.ToDouble(item.Cells["Expense"].Value);
				double Commission = Conversion.ToDouble(item.Cells["Commission"].Value);
				List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
				if (lst.Count > 0)
				{
					if (!bool.Parse(lst[0].ConfigKey))
					{
						item.Cells["BillAmount"].Value = ItemAmount;
					}
					else
					{
						item.Cells["BillAmount"].Value = Math.Round(ItemAmount + Expense - Commission, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
				}
				else
				{
					item.Cells["BillAmount"].Value = Math.Round(ItemAmount + Expense - Commission, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
			}
			grd.UpdateData();
			CommissionAmountCalculateInCaseofPolicy();
			txtExchangeRate_TextChanged(null, null);
			CalculateItemDiscount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PAckingAddless()
	{
	}

	private void grdSettings()
	{
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["CostCenterId"].Visible = false;
			grd.RootTable.Columns["OtherCategoryId"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grd.RootTable.Columns["ItemRate"].Visible = false;
			grd.RootTable.Columns["PackingAddLess"].Visible = false;
			grd.RootTable.Columns["RefRefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefRefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefRefDocIdNo"].Visible = false;
			grd.RootTable.Columns["RefDocSubId"].Visible = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["InvGdnId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["BrandItemId"].Visible = false;
			grd.RootTable.Columns["ItemUomId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["SaleOrderId"].Visible = false;
			grd.RootTable.Columns["OrderItemRateUOMId"].Visible = false;
			grd.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["Journal"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Visible = false;
			grd.RootTable.Columns["ItemDiscount"].Visible = false;
			grd.RootTable.Columns["DiscAmount"].Visible = false;
			grd.RootTable.Columns["SaleOrderPaymentTermId"].Visible = false;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["SaleOrder"].Width = Constants.InventoryConstants.DoNoConstant;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DoNoConstant;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["UOMCodeItem"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["AddLsWt"].Width = Constants.InventoryConstants.AddLessConstant;
			grd.RootTable.Columns["EBWPerUnit"].Width = Constants.InventoryConstants.EbWt;
			grd.RootTable.Columns["EmptyBagsDeduction"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["RateCut"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["RateCutAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Freights"].Width = Constants.InventoryConstants.FreightConstant;
			grd.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Commission"].Width = Constants.InventoryConstants.StatusConstant;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["BranchId"].EditType = (EditType)0;
			grd.RootTable.Columns["CostCenterId"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchName"].EditType = (EditType)0;
			grd.RootTable.Columns["RefRefDocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["RefRefDocIdNo"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocSubId"].EditType = (EditType)0;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["InvGdnId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderItemRateUOMId"].EditType = (EditType)0;
			grd.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["Journal"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["BrandItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["BrandItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grd.RootTable.Columns["UOMCodeItem"].EditType = (EditType)0;
			grd.RootTable.Columns["PackEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrder"].EditType = (EditType)0;
			grd.RootTable.Columns["EBWPerUnit"].EditType = (EditType)0;
			grd.RootTable.Columns["EmptyBagsDeduction"].EditType = (EditType)0;
			grd.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["AddLsWt"].EditType = (EditType)0;
			grd.RootTable.Columns["NetBillWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grd.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemDiscount"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["BillAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["Freights"].EditType = (EditType)0;
			grd.RootTable.Columns["Expense"].EditType = (EditType)0;
			grd.RootTable.Columns["Commission"].EditType = (EditType)0;
			grd.RootTable.Columns["RateCutAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["CommPercent"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemRateWOExp"].EditType = (EditType)0;
			grd.RootTable.Columns["CommOnSale"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemDiscount"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingAddLess"].EditType = (EditType)0;
			grd.RootTable.Columns["OtherCategoryId"].EditType = (EditType)0;
			grd.RootTable.Columns["CategoryII"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderPaymentTermId"].EditType = (EditType)0;
			grd.RootTable.Columns["EBWPerUnit"].Caption = "EbW Per Unit";
			grd.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grd.RootTable.Columns["DiscAmount"].Caption = "Item Discount Amount";
			grd.RootTable.Columns["ItemRateWOExp"].Caption = "Item Rate";
			grd.RootTable.Columns["ItemRate"].Caption = "Net Rate";
			grd.RootTable.Columns["UOMCodeItem"].Caption = "Pack UOM";
			grd.RootTable.Columns["SaleOrder"].Position = 0;
			grd.RootTable.Columns["RateCut"].EditType = (EditType)(RateCutAndRateCutAmountAddOnInvoice ? 1 : 0);
			grd.RootTable.Columns["RateCut"].Visible = RateCutAndRateCutAmountAddOnInvoice;
			grd.RootTable.Columns["RateCutAmount"].Visible = RateCutAndRateCutAmountAddOnInvoice;
			grd.RootTable.Columns["CommPercent"].Visible = CommissionPolicyIsActive || CommissionPolicyForCommissionAgentIsActive;
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grd.RootTable.Columns["CommOnSale"].Visible = EnableItemWiseComm;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["CommPercent"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CommPercent"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CommPercent"].FormatString = "#,##0.##";
			grd.RootTable.Columns["CommPercent"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EBWPerUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EBWPerUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EBWPerUnit"].FormatString = "#,##0.###";
			grd.RootTable.Columns["EBWPerUnit"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["EmptyBagsDeduction"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EmptyBagsDeduction"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EmptyBagsDeduction"].FormatString = "#,##0.##";
			grd.RootTable.Columns["EmptyBagsDeduction"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["AddLsWt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLsWt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLsWt"].FormatString = "#,##0.##";
			grd.RootTable.Columns["AddLsWt"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemRateWOExp"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRateWOExp"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["PackingAddLess"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PackingAddLess"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freights"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Freights"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Expense"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Commission"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemDiscount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemDiscount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemDiscount"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemDiscount"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["DiscAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DiscAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DiscAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DiscAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			ctrlGrdBar1_Load(null, null);
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
			if ((e.Column.Key == "RateCut" || e.Column.Key == "GpNo") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void GridComboBind()
	{
		try
		{
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = "0,0";
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
			grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdFreight.RootTable.Columns["Transporter"].Width = 120;
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			DataTable dt = CommonServices.StaticColumnsService("GetCommissionUom");
			if (dt != null && dt.Rows.Count > 0)
			{
				grdComm.RootTable.Columns["CommUom"].ColumnType = (ColumnType)0;
				grdComm.RootTable.Columns["CommUom"].HasValueList = true;
				grdComm.RootTable.Columns["CommUom"].LimitToList = true;
				grdComm.RootTable.Columns["CommUom"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "type");
			}
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
			Id = Conversion.ToInt((object)ID);
			InvSaleInvoice invSI = InvSaleInvoice.GetByID(Id);
			txtdocno.Text = invSI.DocNo.ToString();
			txtBranchSrNo.Text = Conversion.ToString((object)invSI.BranchSrNo);
			DocDate.Value = invSI.DocDate;
			tabControl1.SelectedIndex = 0;
			cmbsuppliername.Value = invSI.SupplierCustomerId;
			if (CommissionDebitToExpenses)
			{
				CmbCommissionDebitAc.Value = invSI.CommissionDebitAcGLId;
			}
			((Control)(object)cmbsuppliername).Enabled = false;
			txtrefno.Text = Conversion.ToString((object)invSI.SupplierReferenceNo);
			txtbillno.Text = invSI.ManualBillNo;
			txtDueDays.Text = Conversion.ToString((object)invSI.DueDays);
			DueDate.Value = Conversion.ToDateTime((object)invSI.DueDate);
			cmbcommagent.Value = invSI.CommissionAgentId;
			txtcommrate.Text = invSI.CommRate.ToString();
			txtcommamount.Text = invSI.CommAmount.ToString();
			((Control)(object)cmbcommtype).Text = Conversion.ToString((object)invSI.CommissionType);
			((Control)(object)cmbcommuom).Text = invSI.UomScheduleIdCmRate;
			txtcommremarks.Text = Conversion.ToString((object)invSI.CommissionRemarks);
			txtRemarks.Text = Conversion.ToString((object)invSI.OtherRemarks);
			txtBillAmount.Text = invSI.BillAmount.ToString();
			if (HasMultiCurrencyFeature)
			{
				cmbCurrency.Value = invSI.CurrencyId;
				txtExchangeRate.Text = Conversion.ToString((object)invSI.ExchangeRate);
			}
			else
			{
				cmbCurrency.Value = BaseCurrencyId;
				txtExchangeRate.Text = Conversion.ToString((object)BaseCurrencyRate);
			}
			txtFcyAmount.Text = Conversion.ToString((object)invSI.FcyAmount);
			Approved = invSI.IsApproved;
			if (invSI.LocationTypeId > 0)
			{
				cmbLocationType.Value = Conversion.ToInt((object)invSI.LocationTypeId);
			}
			else
			{
				cmbLocationType.Value = ((((UltraGridBase)cmbLocationType).DataSource != null) ? 1 : 0);
			}
			AttachmentsValues = invSI.AttachmentsValues;
			CustomAttachmentsValues = invSI.CustomAttachmentsValues;
			chkCustomAccounts.Checked = Conversion.ToBool((object)invSI.CustomAccounts);
			hasSaleOrder = invSI.InvSaleInvoiceDetaillist != null && invSI.InvSaleInvoiceDetaillist.Any((InvSaleInvoiceDetail d) => d.SaleOrderId > 0);
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int n = 0; n < invSI.InvSaleInvoiceDetaillist.Count; n++)
			{
				dtGrid.Rows.Add(invSI.InvSaleInvoiceDetaillist[n].OtherCategoryId, invSI.InvSaleInvoiceDetaillist[n].OtherCategory, invSI.InvSaleInvoiceDetaillist[n].RefRefDocumentTypeId, invSI.InvSaleInvoiceDetaillist[n].RefRefDocIdNo, invSI.InvSaleInvoiceDetaillist[n].RefDocSubId, invSI.InvSaleInvoiceDetaillist[n].SaleGLAC, invSI.InvSaleInvoiceDetaillist[n].InvGdnDetailId, invSI.InvSaleInvoiceDetaillist[n].InvGdnId, invSI.InvSaleInvoiceDetaillist[n].ItemId, invSI.InvSaleInvoiceDetaillist[n].ItemCode, invSI.InvSaleInvoiceDetaillist[n].ItemName, invSI.InvSaleInvoiceDetaillist[n].BrandItemId, invSI.InvSaleInvoiceDetaillist[n].BrandItemCode, invSI.InvSaleInvoiceDetaillist[n].BrandItemName, invSI.InvSaleInvoiceDetaillist[n].CropYear, invSI.InvSaleInvoiceDetaillist[n].ItemUOMId, invSI.InvSaleInvoiceDetaillist[n].UOMCodeItem, invSI.InvSaleInvoiceDetaillist[n].PackEquivalent, invSI.InvSaleInvoiceDetaillist[n].ItemQty, invSI.InvSaleInvoiceDetaillist[n].WarehouseId, invSI.InvSaleInvoiceDetaillist[n].WareHouseName, invSI.InvSaleInvoiceDetaillist[n].PackingTypeId, invSI.InvSaleInvoiceDetaillist[n].SaleOrderId, invSI.InvSaleInvoiceDetaillist[n].SaleOrder, invSI.InvSaleInvoiceDetaillist[n].GrossWeight, invSI.InvSaleInvoiceDetaillist[n].AdLsWeight, invSI.InvSaleInvoiceDetaillist[n].EBWeight, invSI.InvSaleInvoiceDetaillist[n].EBTotalWt, invSI.InvSaleInvoiceDetaillist[n].NetBillWeight, invSI.InvSaleInvoiceDetaillist[n].NetStockWeight, Conversion.ToDecimal((object)invSI.InvSaleInvoiceDetaillist[n].ItemRateWOExp), Conversion.ToDecimal((object)invSI.InvSaleInvoiceDetaillist[n].PackingAddLess), invSI.InvSaleInvoiceDetaillist[n].ItemRate, invSI.InvSaleInvoiceDetaillist[n].UomScheduleIdRate, invSI.InvSaleInvoiceDetaillist[n].UOMCodeRate, invSI.InvSaleInvoiceDetaillist[n].RateUOM, invSI.InvSaleInvoiceDetaillist[n].RateCut, invSI.InvSaleInvoiceDetaillist[n].RateCutAmount, invSI.InvSaleInvoiceDetaillist[n].ItemAmount, invSI.InvSaleInvoiceDetaillist[n].FcyAmount, invSI.InvSaleInvoiceDetaillist[n].ItemDiscount, invSI.InvSaleInvoiceDetaillist[n].ItemDiscountAmount, invSI.InvSaleInvoiceDetaillist[n].JobLotId, invSI.InvSaleInvoiceDetaillist[n].BillAmount, invSI.InvSaleInvoiceDetaillist[n].FreightAmount, invSI.InvSaleInvoiceDetaillist[n].ExpenseAmount, invSI.InvSaleInvoiceDetaillist[n].JournalAmount, invSI.InvSaleInvoiceDetaillist[n].CommissionAmount, invSI.InvSaleInvoiceDetaillist[n].VehicleNo, invSI.InvSaleInvoiceDetaillist[n].GpNo, invSI.InvSaleInvoiceDetaillist[n].CommOnSale, 0, invSI.InvSaleInvoiceDetaillist[n].BranchId, invSI.InvSaleInvoiceDetaillist[n].BranchName, invSI.InvSaleInvoiceDetaillist[n].CostCenterId, invSI.InvSaleInvoiceDetaillist[n].PaymentTermId);
				ItemIdsForComm = ItemIdsForComm + "," + invSI.InvSaleInvoiceDetaillist[n].ItemId;
			}
			dtInvExp.Rows.Clear();
			for (int m = 0; m < invSI.InvSaleInvoiceExpenseList.Count; m++)
			{
				dtInvExp.Rows.Add(invSI.InvSaleInvoiceExpenseList[m].InvRevExpItemId, invSI.InvSaleInvoiceExpenseList[m].Qty, invSI.InvSaleInvoiceExpenseList[m].Rate, invSI.InvSaleInvoiceExpenseList[m].Amount, invSI.InvSaleInvoiceExpenseList[m].CustomRemarks);
			}
			dtGrdGL.Rows.Clear();
			for (int l = 0; l < invSI.InvSaleInvoiceJournalList.Count; l++)
			{
				dtGrdGL.Rows.Add(invSI.InvSaleInvoiceJournalList[l].TransporterSupCustId, invSI.InvSaleInvoiceJournalList[l].ChartofAccountId, invSI.InvSaleInvoiceJournalList[l].JvRemarks, invSI.InvSaleInvoiceJournalList[l].JvPrcnt, invSI.InvSaleInvoiceJournalList[l].JvQty, invSI.InvSaleInvoiceJournalList[l].JvRate, invSI.InvSaleInvoiceJournalList[l].JvDebit, invSI.InvSaleInvoiceJournalList[l].JvCredit);
			}
			dtComm.Rows.Clear();
			for (int k = 0; k < invSI.InvSaleInvoiceCommission.Count; k++)
			{
				dtComm.Rows.Add(invSI.InvSaleInvoiceCommission[k].Id, invSI.InvSaleInvoiceCommission[k].SaleOrderId, invSI.InvSaleInvoiceCommission[k].OrderNo, invSI.InvSaleInvoiceCommission[k].InvSaleInvoiceId, invSI.InvSaleInvoiceCommission[k].CommissionAgentId, invSI.InvSaleInvoiceCommission[k].CommType, invSI.InvSaleInvoiceCommission[k].Rate, invSI.InvSaleInvoiceCommission[k].RateUom, invSI.InvSaleInvoiceCommission[k].CommAmount, invSI.InvSaleInvoiceCommission[k].CommDebitSupCustId, invSI.InvSaleInvoiceCommission[k].Remarks, invSI.InvSaleInvoiceCommission[k].DebitAccountId);
			}
			dtFreight.Rows.Clear();
			for (int j = 0; j < invSI.InvSaleInvoiceFreightList.Count; j++)
			{
				dtFreight.Rows.Add(invSI.InvSaleInvoiceFreightList[j].InvGdnId, invSI.InvSaleInvoiceFreightList[j].TansporterId, invSI.InvSaleInvoiceFreightList[j].TransporterSupCustId, invSI.InvSaleInvoiceFreightList[j].FreightAmount, invSI.InvSaleInvoiceFreightList[j].Debit, invSI.InvSaleInvoiceFreightList[j].Remarks);
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
			if (dtFreight.Rows.Count == 0)
			{
				AddRowInFreightGrid();
			}
			grdFreight.DataSource = dtFreight;
			grdFreight.RetrieveStructure();
			if (dtComm.Rows.Count == 0)
			{
				AddRowInCommissionGrid();
			}
			grdComm.DataSource = dtComm;
			grdComm.RetrieveStructure();
			grdCommissionSettings();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdFreightSettings();
			grdInvExpSettings();
			gridGLSettings();
			dtPaymentTerm.Rows.Clear();
			bool SystemGenerated = false;
			SystemGenerated = invSI.SaleInvoicePaymentTermsDetailslist.Count == 0 || invSI.SaleInvoicePaymentTermsDetailslist.Any((SaleInvoicePaymentTermsDetail r) => r.SystemGeneratedRow);
			if (Conversion.ToBool((object)SystemGenerated))
			{
				var distinctOrders = (from GridEXRow row in grd.GetRows()
					select new
					{
						OrderId = Conversion.ToInt(row.Cells["SaleOrderId"].Value),
						OrderNo = Conversion.ToInt(row.Cells["SaleOrder"].Value),
						PaymentTermsId = Conversion.ToInt(row.Cells["SaleOrderPaymentTermId"].Value)
					} into x
					where x.OrderId > 0
					select x).Distinct().ToList();
				if (distinctOrders.Any())
				{
					HashSet<int> existingOrderIds = (from GridEXRow r in grdPaymentDetail.GetRows()
						select Conversion.ToInt(r.Cells["SaleOrderId"].Value)).ToHashSet();
					foreach (var order in distinctOrders)
					{
						if (!existingOrderIds.Contains(order.OrderId))
						{
							AddPaymentRow(order.OrderId, order.OrderNo, order.PaymentTermsId, 0, DateTime.Now);
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < invSI.SaleInvoicePaymentTermsDetailslist.Count; i++)
				{
					dtPaymentTerm.Rows.Add(invSI.SaleInvoicePaymentTermsDetailslist[i].SaleOrderId, invSI.SaleInvoicePaymentTermsDetailslist[i].SaleOrderNo, 0, 0, 0, 0, 0, 0, invSI.SaleInvoicePaymentTermsDetailslist[i].PaymentTermId, invSI.SaleInvoicePaymentTermsDetailslist[i].DueDays, invSI.SaleInvoicePaymentTermsDetailslist[i].DueDate, invSI.SaleInvoicePaymentTermsDetailslist[i].PrcntOfTotal, invSI.SaleInvoicePaymentTermsDetailslist[i].Amount, invSI.SaleInvoicePaymentTermsDetailslist[i].PaymentRemarks);
				}
			}
			CalculateOrderWiseAmountForPaymentDetail();
			grdPaymentDetail.RetrieveStructure();
			gridPaymentTermSetting();
			ExpProportion();
			LedgerProportion();
			FreightProportion();
			CommissionProportion();
			CalculateCommissionGrid();
			PropotionateCommissiongrid();
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
			BindCommGridOrderNo();
			tabControl1.SelectedIndex = 0;
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			BillProportion();
			CommissionAmountCalculateInCaseofPolicy();
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
			if (Id == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Expected O, but got Unknown
		//IL_1165: Unknown result type (might be due to invalid IL or missing references)
		//IL_116c: Expected O, but got Unknown
		//IL_1309: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Expected O, but got Unknown
		//IL_1541: Unknown result type (might be due to invalid IL or missing references)
		//IL_1548: Expected O, but got Unknown
		//IL_172a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Expected O, but got Unknown
		//IL_205b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2062: Expected O, but got Unknown
		//IL_1e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7b: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			InvSaleInvoice saleInvoice = new InvSaleInvoice();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				saleInvoice.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
			FreightProportion();
			ExpProportion();
			CommissionProportion();
			double FreightDebit = 0.0;
			double FreightCredit = 0.0;
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r8 in rows)
			{
				FreightCredit += Conversion.ToDouble(r8.Cells["Freight"].Value);
				FreightDebit += Conversion.ToDouble(r8.Cells["Debit"].Value);
				if (Conversion.ToDouble(r8.Cells["Freight"].Value) > 0.0 && Conversion.ToInt(r8.Cells["Transporter"].Value) == 0)
				{
					MessageBox.Show("Please Select an Account Against Freight First");
					return;
				}
			}
			if (FreightDebitToExpenses && FreightCredit != FreightDebit)
			{
				MessageBox.Show("Freight Debit Amount not equal to Credit Amount Please Check");
				return;
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r9 in rows)
			{
				if ((Conversion.ToDouble(r9.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r9.Cells["Debit"].Value) > 0) && Conversion.ToInt(r9.Cells["AccountId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Account Against JL First");
					return;
				}
				if (Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(r9.Cells["AccountId"].Value))
				{
					r9.Cells["AccountId"].Value = 0;
					MessageBox.Show("Customer Account Not select In Party Add Less Grid row#" + (r9.RowIndex + 1));
					return;
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				if (Conversion.ToDouble(r7.Cells["Amount"].Value) > 0.0 || Conversion.ToInt(r7.Cells["ItemId"].Value) > 0)
				{
					if (Conversion.ToInt(r7.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Please Select an Item Against Expense First");
						return;
					}
					if (Conversion.ToDouble(r7.Cells["Amount"].Value) == 0.0)
					{
						MessageBox.Show("Amount Field is Required Against Expense");
						return;
					}
				}
			}
			int Comm = Conversion.ToInt(grdComm.CurrentRow.Cells["CommissionAgentId"].Value);
			string CommType = Conversion.ToString(grdComm.CurrentRow.Cells["CommType"].Value);
			bool IsCommsame = true;
			double CommissionAmt = 0.0;
			rows = grdComm.GetRows();
			foreach (GridEXRow item3 in rows)
			{
				CommissionAmt += Conversion.ToDouble(item3.Cells["CommissionAmount"].Value);
				if (Conversion.ToDouble(item3.Cells["CommissionAmount"].Value) > 0.0)
				{
					if (Conversion.ToInt(item3.Cells["CommissionAgentId"].Value) == 0)
					{
						MessageBox.Show("Please Select Commission Agent In Commission Grid");
						return;
					}
					if (Conversion.ToInt(item3.Cells["CommissionAgentId"].Value) != Comm || Conversion.ToString(item3.Cells["CommType"].Value) != CommType)
					{
						IsCommsame = false;
					}
					if (CommissionDebitToExpenses && Conversion.ToInt(item3.Cells["DebitAccountId"].Value) == 0)
					{
						MessageBox.Show("Debit Account Feild Required In Commission Grid ...");
						return;
					}
				}
			}
			if (IsCommsame && CommissionAmt > 0.0)
			{
				saleInvoice.CommissionAgentId = Conversion.ToInt(grdComm.CurrentRow.Cells["CommissionAgentId"].Value);
				if (!CommissionPolicyIsActive && !CommissionPolicyForCommissionAgentIsActive)
				{
					saleInvoice.CommissionDebitAcGLId = Conversion.ToInt(grdComm.CurrentRow.Cells["DebitAccountId"].Value);
				}
				else
				{
					saleInvoice.CommissionDebitAcGLId = 0;
				}
				saleInvoice.CommissionType = Conversion.ToString(grdComm.CurrentRow.Cells["CommType"].Value);
				saleInvoice.CommRate = Conversion.ToDouble((object)grdComm.GetTotal(grdComm.RootTable.Columns["CommRate"], (AggregateFunction)3).ToString());
				saleInvoice.CommAmount = Conversion.ToDouble((object)grdComm.GetTotal(grdComm.RootTable.Columns["CommissionAmount"], (AggregateFunction)2).ToString());
				saleInvoice.CommissionRemarks = Conversion.ToString(grdComm.CurrentRow.Cells["Remarks"].Value);
			}
			saleInvoice.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			saleInvoice.DocDate = DocDate.Value;
			saleInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			saleInvoice.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
			saleInvoice.DocumentTypeId = DocumentTypeId;
			saleInvoice.ManualBillNo = txtbillno.Text.Trim();
			saleInvoice.RemarksHeader = txtRemarks.Text.Trim();
			saleInvoice.OtherRemarks = saleInvoice.RemarksHeader;
			saleInvoice.SupplierCustomerId = Conversion.ToInt(cmbsuppliername.Value);
			saleInvoice.SupplierInvoiceDate = DateTime.Now;
			saleInvoice.SupplierInvoiceNo = Conversion.ToInt((object)txtdocno.Text);
			saleInvoice.SupplierReferenceNo = Conversion.ToString((object)txtrefno.Text.Trim());
			saleInvoice.DueDays = Conversion.ToInt((object)txtDueDays.Text.Trim());
			saleInvoice.DueDate = DueDate.Value;
			saleInvoice.UomScheduleIdCmRate = ((Control)(object)cmbcommuom).Text.Trim();
			saleInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			saleInvoice.OrganizationId = UserAccount.OrganizationId;
			saleInvoice.CompanyId = UserAccount.CompanyId;
			saleInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			saleInvoice.EntryUser = UserAccount.ID;
			saleInvoice.EntryDate = DateTime.Now;
			saleInvoice.ModifyDate = DateTime.Now;
			saleInvoice.ModifyUser = UserAccount.ID;
			saleInvoice.ScreenName = Conversion.ToString((object)base.Name);
			saleInvoice.CustomAccounts = Conversion.ToBool((object)chkCustomAccounts.Checked);
			saleInvoice.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			saleInvoice.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			saleInvoice.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			saleInvoice.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
			saleInvoice.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			rows = grd.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				InvSaleInvoiceDetail Pd = new InvSaleInvoiceDetail();
				Pd.LineId = r6.RowIndex + 1;
				Pd.CropYear = Conversion.ToString(r6.Cells["CropYear"].Value);
				if (Pd.CropYear == string.Empty)
				{
					MessageBox.Show("CropYear Field is Required");
					return;
				}
				Pd.GrossWeight = Conversion.ToDouble(r6.Cells["GrossWeight"].Value);
				Pd.AdLsWeight = Conversion.ToDouble(r6.Cells["AddLsWt"].Value);
				Pd.RefRefDocumentTypeId = Conversion.ToInt(r6.Cells["RefRefDocumentTypeId"].Value);
				Pd.RefRefDocIdNo = Conversion.ToInt(r6.Cells["RefRefDocIdNo"].Value);
				Pd.RefDocSubId = Conversion.ToInt(r6.Cells["RefDocSubId"].Value);
				Pd.SaleOrderId = Conversion.ToInt(r6.Cells["SaleOrderId"].Value);
				Pd.SaleOrder = Conversion.ToInt(r6.Cells["SaleOrder"].Value);
				Pd.UOMCodeItem = Conversion.ToString(r6.Cells["UOMCodeItem"].Value);
				if (Pd.SaleOrderId == 0)
				{
					MessageBox.Show("SaleOrderId Field Required");
					return;
				}
				Pd.InvGdnDetailId = Conversion.ToInt(r6.Cells["Id"].Value);
				if (Pd.InvGdnDetailId == 0)
				{
					MessageBox.Show("InvGdnDetailId not found");
					return;
				}
				Pd.InvGdnId = Conversion.ToInt(r6.Cells["InvGdnId"].Value);
				if (Pd.InvGdnId == 0)
				{
					MessageBox.Show("InvGdnId not found");
					return;
				}
				Pd.ItemId = Conversion.ToInt(r6.Cells["ItemId"].Value);
				Pd.BrandItemId = Conversion.ToInt(r6.Cells["BrandItemId"].Value);
				if (Pd.ItemId == 0)
				{
					MessageBox.Show("Item Required");
					return;
				}
				Pd.ItemQty = Conversion.ToDouble(r6.Cells["ItemQty"].Value);
				if (Pd.ItemQty == 0.0)
				{
					MessageBox.Show("ItemQty must be greater than zero");
					return;
				}
				Pd.ItemUOMId = Conversion.ToInt(r6.Cells["ItemUomId"].Value);
				if (Pd.ItemUOMId == 0)
				{
					MessageBox.Show("ItemUOMId Field Required");
					return;
				}
				Pd.JobLotId = Conversion.ToInt(r6.Cells["JobLotId"].Value);
				if (Pd.JobLotId == 0)
				{
					MessageBox.Show("JobLot Required");
					return;
				}
				Pd.NetBillWeight = Conversion.ToDouble(r6.Cells["NetBillWeight"].Value);
				if (Pd.NetBillWeight == 0.0)
				{
					MessageBox.Show("NetBillWeight Field Required");
					return;
				}
				Pd.NetStockWeight = Conversion.ToDouble(r6.Cells["StockWeight"].Value);
				if (Pd.NetStockWeight == 0.0)
				{
					MessageBox.Show("NetStockWeight Field Required");
					return;
				}
				Pd.EBTotalWt = Conversion.ToDouble(r6.Cells["EmptyBagsDeduction"].Value);
				Pd.EBWeight = Conversion.ToDouble(r6.Cells["EBWPerUnit"].Value);
				Pd.PackingTypeId = Conversion.ToInt(r6.Cells["PackingTypeId"].Value);
				if (Pd.PackingTypeId == 0)
				{
					MessageBox.Show("PackingType Required");
					return;
				}
				Pd.ItemRateWOExp = Conversion.ToDecimal(r6.Cells["ItemRateWOExp"].Value);
				if (Pd.ItemRateWOExp == 0m)
				{
					MessageBox.Show("ItemRate Field Required");
					return;
				}
				Pd.PackingAddLess = Conversion.ToDecimal(r6.Cells["PackingAddLess"].Value);
				Pd.ItemRate = Conversion.ToDouble(r6.Cells["ItemRate"].Value);
				if (Pd.ItemRate == 0.0)
				{
					MessageBox.Show("ItemNetRate Field Required");
					return;
				}
				Pd.RateUOM = Conversion.ToDouble(r6.Cells["RateEquivalent"].Value);
				Pd.UomScheduleIdRate = Conversion.ToInt(r6.Cells["OrderItemRateUOMId"].Value);
				if (Pd.UomScheduleIdRate == 0)
				{
					DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r6.Cells["ItemId"].Value));
					if (dt.Rows.Count > 0)
					{
						DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt(r6.Cells["RateEquivalent"].Value) + "'");
						if (dr.Length == 0)
						{
							MessageBox.Show("this RateUom not define please check");
							return;
						}
						Pd.UomScheduleIdRate = Conversion.ToInt(dr[0].ItemArray[0]);
					}
				}
				if (Pd.UomScheduleIdRate == 0)
				{
					MessageBox.Show("Rate UOM Required");
					return;
				}
				Pd.RateCut = Conversion.ToDouble(r6.Cells["RateCut"].Value);
				Pd.RateCutAmount = Conversion.ToDouble(r6.Cells["RateCutAmount"].Value);
				Pd.ItemAmount = Math.Round(Conversion.ToDouble(r6.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				Pd.ItemDiscount = Conversion.ToDouble(r6.Cells["ItemDiscount"].Value);
				Pd.ItemDiscountAmount = Conversion.ToDouble(r6.Cells["DiscAmount"].Value);
				if (Pd.ItemAmount == 0.0)
				{
					MessageBox.Show("ItemAmount Field Required");
					return;
				}
				Pd.WarehouseId = Conversion.ToInt(r6.Cells["WarehouseId"].Value);
				Pd.WareHouseName = Conversion.ToString(r6.Cells["Warehouse"].Value);
				if (Pd.WarehouseId == 0)
				{
					MessageBox.Show("Warehouse Required");
					return;
				}
				Pd.BillAmount = Math.Round(Conversion.ToDouble(r6.Cells["BillAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (!FreightDebitToExpenses)
				{
					Pd.FreightAmount = Conversion.ToDouble(r6.Cells["Freights"].Value);
				}
				Pd.JournalAmount = Conversion.ToDouble(r6.Cells["Journal"].Value);
				Pd.ExpenseAmount = Conversion.ToDouble(r6.Cells["Expense"].Value);
				Pd.CommOnSale = Conversion.ToBool(r6.Cells["CommOnSale"].Value);
				if (!CommissionDebitToExpenses)
				{
					Pd.CommissionAmount = Conversion.ToDouble(r6.Cells["Commission"].Value);
				}
				Pd.GpDate = DateTime.Now;
				Pd.GpNo = Conversion.ToInt(r6.Cells["GpNo"].Value);
				Pd.BranchId = Conversion.ToInt(r6.Cells["BranchId"].Value);
				Pd.VehicleNo = Conversion.ToString(r6.Cells["VehicleNo"].Value);
				Pd.CostCenterId = Conversion.ToInt(r6.Cells["CostCenterId"].Value);
				saleInvoice.InvSaleInvoiceDetaillist.Add(Pd);
			}
			saleInvoice.InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
			rows = grdFreight.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToInt((object)r5.Cells["Transporter"].Value.ToString()) != 0)
				{
					InvSaleInvoiceFreight PF = new InvSaleInvoiceFreight();
					PF.InvGdnId = Conversion.ToInt(r5.Cells["InvGdnId"].Value);
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PF.TansporterId = Conversion.ToInt(r5.Cells["GlAccountId"].Value);
						PF.TransporterSupCustId = Conversion.ToInt(r5.Cells["Transporter"].Value);
					}
					else
					{
						PF.TansporterId = Conversion.ToInt(r5.Cells["GlAccountId"].Value);
						PF.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PF.FreightAmount = Conversion.ToInt(r5.Cells["Freight"].Value);
					PF.Debit = Conversion.ToInt(r5.Cells["Debit"].Value);
					PF.Remarks = Conversion.ToString(r5.Cells["Remarks"].Value);
					if (PF.Remarks == null || PF.Remarks == string.Empty)
					{
						throw new Exception("Remarks Field Requried in Freight Grid");
					}
					saleInvoice.InvSaleInvoiceFreightList.Add(PF);
				}
			}
			saleInvoice.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
					PE.InvRevExpItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r4.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r4.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDouble(r4.Cells["Amount"].Value);
					PE.CustomRemarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					PE.Remarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					if (PE.Qty > 0.0 && PE.Rate > 0.0)
					{
						PE.Remarks = "ItemName : " + Conversion.ToString((object)r4.Cells["ItemId"].Text.Trim()) + "  " + $"Qty: {PE.Qty} " + $"Rate: {PE.Rate} " + PE.Remarks;
					}
					saleInvoice.InvSaleInvoiceExpenseList.Add(PE);
				}
			}
			saleInvoice.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["AccountId"].Value.ToString()) != 0 && (Conversion.ToDouble(r3.Cells["Debit"].Value) > 0.0 || Conversion.ToDouble(r3.Cells["Credit"].Value) > 0.0))
				{
					InvSaleInvoiceJournal PJ2 = new InvSaleInvoiceJournal();
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PJ2.ChartofAccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
						PJ2.TransporterSupCustId = Conversion.ToInt(r3.Cells["AccountId"].Value);
					}
					else
					{
						PJ2.ChartofAccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
						PJ2.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PJ2.JvRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					PJ2.JvPrcnt = Conversion.ToDouble(r3.Cells["Percentage"].Value);
					PJ2.JvQty = Conversion.ToDouble(r3.Cells["Qty"].Value);
					PJ2.JvRate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					PJ2.JvDebit = Conversion.ToDouble(r3.Cells["Debit"].Value);
					PJ2.JvCredit = Conversion.ToDouble(r3.Cells["Credit"].Value);
					saleInvoice.InvSaleInvoiceJournalList.Add(PJ2);
				}
			}
			saleInvoice.InvSaleInvoiceCommission = new List<InvSaleInvoiceCommission>();
			rows = grdComm.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["CommissionAgentId"].Value.ToString()) > 0 && Conversion.ToDouble(r2.Cells["CommissionAmount"].Value) > 0.0)
				{
					InvSaleInvoiceCommission PJ = new InvSaleInvoiceCommission();
					PJ.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					PJ.SaleOrderId = Conversion.ToInt(r2.Cells["OrderId"].Value);
					PJ.InvSaleInvoiceId = Conversion.ToInt(r2.Cells["InvoiceId"].Value);
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PJ.CommDebitSupCustId = Conversion.ToInt(r2.Cells["DebitAccountId"].Value);
						PJ.DebitAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
					}
					else
					{
						PJ.CommDebitSupCustId = 0;
						PJ.DebitAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
					}
					PJ.CommissionAgentId = Conversion.ToInt(r2.Cells["CommissionAgentId"].Value);
					if (Conversion.ToString(r2.Cells["CommType"].Value) == "Percent")
					{
						PJ.CommType = 2.0;
					}
					if (Conversion.ToString(r2.Cells["CommType"].Value) == "Flat")
					{
						PJ.CommType = 1.0;
					}
					if (Conversion.ToString(r2.Cells["CommType"].Value) == "Comm Weight")
					{
						PJ.CommType = 3.0;
					}
					if (Conversion.ToDouble(r2.Cells["CommType"].Value) > 0.0)
					{
						PJ.CommType = Conversion.ToDouble(r2.Cells["CommType"].Value);
					}
					PJ.Rate = Conversion.ToDouble(r2.Cells["CommRate"].Value);
					PJ.RateUom = Conversion.ToDouble(r2.Cells["CommUom"].Value);
					PJ.CommAmount = Conversion.ToDouble(r2.Cells["CommissionAmount"].Value);
					PJ.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					if (CommissionPolicyIsActive || CommissionPolicyForCommissionAgentIsActive)
					{
						PJ.Rate = PJ.CommAmount;
						PJ.CommType = 1.0;
						PJ.RateUom = 0.0;
						PJ.SaleOrderId = 0;
					}
					saleInvoice.InvSaleInvoiceCommission.Add(PJ);
				}
			}
			decimal Pamount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentDetail, "Amount"));
			decimal PaymentDetailAmount = default(decimal);
			decimal PtScheduleDiff = Conversion.ToDecimal((object)saleInvoice.BillAmount) - Pamount;
			if (Pamount > 0m)
			{
				GridEXRow[] dataRows = grdPaymentDetail.GetDataRows();
				if (dataRows.Any((GridEXRow r) => Conversion.ToDecimal(r.Cells["%OfTotal"].Value) > 0m || Conversion.ToDecimal(r.Cells["Amount"].Value) > 0m))
				{
					int rowNo2 = 1;
					rows = dataRows;
					foreach (GridEXRow obj in rows)
					{
						decimal percent = Conversion.ToDecimal(obj.Cells["%OfTotal"].Value);
						decimal amount = Conversion.ToDecimal(obj.Cells["Amount"].Value);
						if (percent <= 0m || amount <= 0m)
						{
							MessageBox.Show($"Payment Detail:All rows must have both % and Amount when any row contains a value. Error at row# {rowNo2}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							tabControl2.SelectedIndex = 1;
							((Control)(object)grdPaymentDetail).Focus();
							return;
						}
						rowNo2++;
					}
				}
				if ((from r in (from GridEXRow r in grdPaymentDetail.GetRows()
						where (int)r.RowType == 0
						select r).ToList()
					group r by Conversion.ToInt(r.Cells["SaleOrderId"].Value)).Any((IGrouping<int, GridEXRow> g) => g.Count() == 1))
				{
					if (PCalculateByAmount)
					{
						PCalculateByAmount = false;
						PCalculateByPercent = true;
					}
					PaymentAmountReCalculate();
				}
				rows = grdPaymentDetail.GetDataRows();
				foreach (GridEXRow obj2 in rows)
				{
					int paymentTermId = Conversion.ToInt(obj2.Cells["PaymentTerm"].Value);
					int dueDays = Conversion.ToInt(obj2.Cells["DueDays"].Value);
					int rowNo = obj2.RowIndex + 1;
					if (paymentTermId <= 0)
					{
						MessageBox.Show($"Payment Term Required in row# {rowNo}", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPaymentDetail).Focus();
						return;
					}
					if (paymentTermId == 2 && dueDays <= 0)
					{
						MessageBox.Show($"Due Days Required in case of Credit in row# {rowNo}", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPaymentDetail).Focus();
						return;
					}
				}
				foreach (int soId in (from r in grdPaymentDetail.GetDataRows()
					select Conversion.ToInt(r.Cells["SaleOrderId"].Value)).Distinct())
				{
					decimal percentOfTotalOrderWise = Conversion.ToDecimal(dtPaymentTerm.Compute("Sum([%OfTotal])", $"SaleOrderId = '{soId}'"));
					percentOfTotalOrderWise = Math.Round(percentOfTotalOrderWise, 4);
					int soNo = Conversion.ToInt(dtPaymentTerm.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt(r["SaleOrderId"]) == soId)?["SaleOrderNo"]);
					if (Math.Abs(100m - percentOfTotalOrderWise) > 0.05m)
					{
						MessageBox.Show($"Percent Of SaleOrder:{soNo} not near to 100. Please Check!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPaymentDetail).Focus();
						return;
					}
				}
				rows = grdPaymentDetail.GetDataRows();
				foreach (GridEXRow item in rows)
				{
					decimal dueamount = Conversion.ToDecimal(item.Cells["Amount"].Value);
					decimal paymentdueamount = ((dataRows.Count() > 1 && PtScheduleDiff <= 1m) ? (dueamount + PtScheduleDiff) : dueamount);
					SaleInvoicePaymentTermsDetail pd = new SaleInvoicePaymentTermsDetail();
					pd.SaleOrderId = Conversion.ToInt(item.Cells["SaleOrderId"].Value);
					pd.SaleOrderNo = Conversion.ToInt(item.Cells["SaleOrderNo"].Value);
					pd.PaymentTermId = Conversion.ToInt(item.Cells["PaymentTerm"].Value);
					pd.TermsDescription = Conversion.ToString((object)item.Cells["PaymentTerm"].Text);
					pd.DueDays = Conversion.ToInt(item.Cells["DueDays"].Value);
					pd.PrcntOfTotal = Conversion.ToDecimal(item.Cells["%OfTotal"].Value);
					pd.Amount = ((dataRows.Count() == 1) ? Conversion.ToDecimal((object)saleInvoice.BillAmount) : paymentdueamount);
					pd.DueDate = Conversion.ToDateTime(item.Cells["DueDate"].Value);
					pd.PaymentRemarks = Conversion.ToString(item.Cells["Remarks"].Value);
					pd.SystemGeneratedRow = false;
					PtScheduleDiff = default(decimal);
					PaymentDetailAmount += pd.Amount;
					saleInvoice.SaleInvoicePaymentTermsDetailslist.Add(pd);
				}
			}
			else
			{
				int dueDays2 = Conversion.ToInt((object)txtDueDays.Text);
				if (dueDays2 < 0)
				{
					dueDays2 = 0;
				}
				HashSet<int> seenOrders = new HashSet<int>();
				rows = grdPaymentDetail.GetDataRows();
				foreach (GridEXRow item2 in rows)
				{
					int soId2 = Conversion.ToInt(item2.Cells["SaleOrderId"].Value);
					if (seenOrders.Add(soId2))
					{
						SaleInvoicePaymentTermsDetail pd2 = new SaleInvoicePaymentTermsDetail();
						pd2.SaleOrderId = soId2;
						pd2.SaleOrderNo = Conversion.ToInt(item2.Cells["SaleOrderNo"].Value);
						pd2.PaymentTermId = Conversion.ToInt(item2.Cells["PaymentTerm"].Value);
						pd2.DueDays = dueDays2;
						pd2.DueDate = saleInvoice.DocDate.AddDays(dueDays2);
						pd2.PrcntOfTotal = 100m;
						pd2.Amount = Conversion.ToDecimal(item2.Cells["NetBillAmount"].Value);
						pd2.SystemGeneratedRow = true;
						PaymentDetailAmount += pd2.Amount;
						saleInvoice.SaleInvoicePaymentTermsDetailslist.Add(pd2);
					}
				}
			}
			if (Math.Abs(PaymentDetailAmount - Conversion.ToDecimal((object)saleInvoice.BillAmount)) > 0.55m)
			{
				MessageBox.Show("Payment Detail Amount:" + PaymentDetailAmount.ToString("#,##0.####") + " Not Equal to Bill Amount:" + saleInvoice.BillAmount.ToString("#,##0.####"), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				tabControl2.SelectedIndex = 2;
				((Control)(object)grdPaymentDetail).Focus();
				return;
			}
			saleInvoice.AttachmentsList = new List<DMSAttachments>();
			saleInvoice.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvSaleInvoice>(saleInvoice, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			saleInvoice.AttachmentsValues = AttachmentsValues;
			saleInvoice.CustomAttachmentsValues = CustomAttachmentsValues;
			int sucess = InvSaleInvoice.Save(saleInvoice);
			if (Id == 0)
			{
				MessageBox.Show("Record saved Successfully" + saleInvoice.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update Successfully" + saleInvoice.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(Id, base.Name, DocumentTypeId, 0);
				}
			}
			int SupCustId = Conversion.ToInt(cmbsuppliername.Value);
			DateTime Date = Conversion.ToDateTime((object)DocDate.Value);
			Reset();
			if (saleInvoice.Id == 0 && ChkOpenADOForm.Checked)
			{
				OpenLinkedFormOnInsert(sucess);
			}
			if (ChkBok.Checked)
			{
				Id = sucess;
				VoucherHeadIdGet();
				Id = 0;
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId);
			}
			if (ChkPrintSlip.Checked)
			{
				CommonServices.SaleInvoicetSlip_301(sucess, SupCustId, 0, Date, Date);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenLinkedFormOnInsert(int SaleId)
	{
		try
		{
			AdvanceDOForm(SaleId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AdvanceDOForm(int SaleId)
	{
		try
		{
			FormHelper.CanOpenForm("AdvanceDeliveryOrderPP", delegate
			{
				AdvanceDeliveryOrderPP advanceDeliveryOrderPP = new AdvanceDeliveryOrderPP(UserAccount);
				advanceDeliveryOrderPP.Show();
				if (SaleId > 0)
				{
					advanceDeliveryOrderPP.GetSaleInvoiceDataAgainstId(SaleId);
				}
				advanceDeliveryOrderPP.BringToFront();
				bool formClosed = false;
				advanceDeliveryOrderPP.FormClosed += delegate
				{
					formClosed = true;
				};
				while (!formClosed)
				{
					Application.DoEvents();
				}
			}, UserAccount);
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
				throw new Exception("Record cannot be  Delete because Record has approved");
			}
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = DocumentTypeId,
						Id = Id,
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			DocDate.Focus();
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ResetHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetHistory()
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			grdDetail.DataSource = null;
			drdocdate.Checked = true;
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
			HistoryCombosBranchDbCall();
			HistoryCombosBranchBind();
			HistoryComboBind(HistoryComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				HistoryComboBind(HistoryComboDbCall());
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

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FillHistoryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillHistoryGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.DocumentTypeId = DocumentTypeId;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			PI.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
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
				PI.BranchesIds = BranchIds;
				DataTable dthistory = InvSaleInvoice.FormHistory(PI);
				if (dthistory.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("VoucherHeadId", typeof(int));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("DueDate", typeof(DateTime));
					dt.Columns.Add("BranchSrNo", typeof(int));
					dt.Columns.Add("BranchName");
					dt.Columns.Add("ManualBillNo");
					dt.Columns.Add("CustomerId", typeof(int));
					dt.Columns.Add("CustomerName");
					dt.Columns.Add("CommAgent");
					dt.Columns.Add("CommType");
					dt.Columns.Add("CommRate", typeof(double));
					dt.Columns.Add("CommAmount", typeof(double));
					dt.Columns.Add("CommRemarks");
					dt.Columns.Add("BillAmount", typeof(double));
					dt.Columns.Add("EntryUser");
					dt.Columns.Add("EntryDate", typeof(DateTime));
					dt.Columns.Add("ModifyUser");
					dt.Columns.Add("ModifyDate", typeof(DateTime));
					dt.Columns.Add("ApprovedUser", typeof(string));
					dt.Columns.Add("ApprovedDate", typeof(DateTime));
					dt.Columns.Add("Remarks");
					dt.Columns.Add("NoOfAttachments");
					for (int i = 0; i < dthistory.Rows.Count; i++)
					{
						dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["BranchSrNo"], dthistory.Rows[i]["BranchName"], dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["SupplierCustomerId"], dthistory.Rows[i]["CustomerName"], dthistory.Rows[i]["CommissionAgent"], dthistory.Rows[i]["CommissionType"], dthistory.Rows[i]["CommRate"], Conversion.ToDouble(dthistory.Rows[i]["CommAmount"]), dthistory.Rows[i]["CommissionRemarks"], Conversion.ToDouble(dthistory.Rows[i]["BillAmount"]), dthistory.Rows[i]["UserName"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUserName"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["ApprovedUserName"], dthistory.Rows[i]["ApprovedDate"], dthistory.Rows[i]["RemarksHeader"], dthistory.Rows[i]["NoOfAttachments"]);
					}
					grdHistory.DataSource = dt;
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
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["CustomerId"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["CommType"].Visible = false;
			grdHistory.RootTable.Columns["CommRemarks"].Visible = false;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.AccountTitle;
			grdHistory.RootTable.Columns["CommAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			grdHistory.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.GpType;
			grdHistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["CommRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ApprovedByConstant;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdHistory.RootTable.Columns["BranchSrNo"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "Slip";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "Slip";
			grdHistory.RootTable.Columns["View"].Width = 40;
			grdHistory.RootTable.Columns["View"].Position = 2;
			grdHistory.RootTable.Columns.Add("Print_303");
			grdHistory.RootTable.Columns["Print_303"].Caption = "303_Print";
			grdHistory.RootTable.Columns["Print_303"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print_303"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print_303"].ButtonText = "303_Print";
			grdHistory.RootTable.Columns["Print_303"].Width = 65;
			grdHistory.RootTable.Columns["Print_303"].Position = 3;
			grdHistory.RootTable.Columns.Add("Print_303A");
			grdHistory.RootTable.Columns["Print_303A"].Caption = "Print_303A";
			grdHistory.RootTable.Columns["Print_303A"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print_303A"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print_303A"].ButtonText = "Print_303A";
			grdHistory.RootTable.Columns["Print_303A"].Width = 65;
			grdHistory.RootTable.Columns["Print_303A"].Position = 4;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 65;
			grdHistory.RootTable.Columns["Voucher"].Position = 5;
			GridEX_Helper.AddButton(grdHistory, "AddAttachment", "Add Attachment", 120);
			grdHistory.FrozenColumns = 6;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar3_Load(null, null);
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
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					Id = id;
					ReadById(Id);
				}
				else if (e.Column.Key == "View")
				{
					int recId = Conversion.ToInt(r.Cells["Id"].Value);
					int SupCustId = Conversion.ToInt(r.Cells["CustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					CommonServices.SaleInvoicetSlip_301(recId, SupCustId, 0, Date, Date);
				}
				else if (e.Column.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(r.Cells["VoucherHeadId"].Value), DocumentTypeId);
				}
				else if (e.Column.Key == "Print_303")
				{
					CommonServices.SaleInvoicetSlip_303(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
				}
				else if (e.Column.Key == "Print_303A")
				{
					CommonServices.SaleInvoiceItemSlip_303A(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, id, DocumentTypeId, base.Name);
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				GridEXRow item = grdHistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailGrdByHeadId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			InvSaleInvoice obj = InvSaleInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				DataTable dtdetail = new DataTable();
				dtdetail.Columns.Add("CategoryII");
				dtdetail.Columns.Add("OrderNo");
				dtdetail.Columns.Add("ItemCode");
				dtdetail.Columns.Add("ItemName");
				dtdetail.Columns.Add("BrandItemCode");
				dtdetail.Columns.Add("BrandItemName");
				dtdetail.Columns.Add("CropYear");
				dtdetail.Columns.Add("ItemUOM");
				dtdetail.Columns.Add("ItemQty", typeof(double));
				dtdetail.Columns.Add("WareHouseName");
				dtdetail.Columns.Add("PackType");
				dtdetail.Columns.Add("SaleOrder");
				dtdetail.Columns.Add("GrossWeight", typeof(double));
				dtdetail.Columns.Add("AddLsWt", typeof(double));
				dtdetail.Columns.Add("EBTotalWt", typeof(double));
				dtdetail.Columns.Add("WeightCutTotal", typeof(double));
				dtdetail.Columns.Add("NetBillWeight", typeof(double));
				dtdetail.Columns.Add("NetStockWeight", typeof(double));
				dtdetail.Columns.Add("ItemRateWOExp", typeof(double));
				dtdetail.Columns.Add("PackingAddLess", typeof(double));
				dtdetail.Columns.Add("ItemRate", typeof(double));
				dtdetail.Columns.Add("RateUOM");
				dtdetail.Columns.Add("RateCut", typeof(double));
				dtdetail.Columns.Add("RateCutAmount", typeof(double));
				dtdetail.Columns.Add("ItemAmount", typeof(double));
				dtdetail.Columns.Add("FcyAmount", typeof(double));
				dtdetail.Columns.Add("JobLot");
				dtdetail.Columns.Add("CommissionAmount", typeof(double));
				dtdetail.Columns.Add("ExpenseAmount", typeof(double));
				dtdetail.Columns.Add("FreightAmount", typeof(double));
				dtdetail.Columns.Add("BillAmount", typeof(double));
				dtdetail.Columns.Add("GpNo");
				dtdetail.Columns.Add("VehicleNo");
				dtdetail.Columns.Add("BranchName");
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.InvSaleInvoiceDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(obj.InvSaleInvoiceDetaillist[i].OtherCategory, obj.InvSaleInvoiceDetaillist[i].SaleOrder, obj.InvSaleInvoiceDetaillist[i].ItemCode, obj.InvSaleInvoiceDetaillist[i].ItemName, obj.InvSaleInvoiceDetaillist[i].BrandItemCode, obj.InvSaleInvoiceDetaillist[i].BrandItemName, obj.InvSaleInvoiceDetaillist[i].CropYear, obj.InvSaleInvoiceDetaillist[i].UOMCodeItem, obj.InvSaleInvoiceDetaillist[i].ItemQty, obj.InvSaleInvoiceDetaillist[i].WareHouseName, obj.InvSaleInvoiceDetaillist[i].PackTypeDesc, obj.InvSaleInvoiceDetaillist[i].SaleOrder, obj.InvSaleInvoiceDetaillist[i].GrossWeight, obj.InvSaleInvoiceDetaillist[i].AdLsWeight, obj.InvSaleInvoiceDetaillist[i].EBTotalWt, obj.InvSaleInvoiceDetaillist[i].WeightCutTotal, obj.InvSaleInvoiceDetaillist[i].NetBillWeight, obj.InvSaleInvoiceDetaillist[i].NetStockWeight, obj.InvSaleInvoiceDetaillist[i].ItemRateWOExp, obj.InvSaleInvoiceDetaillist[i].PackingAddLess, obj.InvSaleInvoiceDetaillist[i].ItemRate, obj.InvSaleInvoiceDetaillist[i].UOMCodeRate, obj.InvSaleInvoiceDetaillist[i].RateCut, obj.InvSaleInvoiceDetaillist[i].RateCutAmount, obj.InvSaleInvoiceDetaillist[i].ItemAmount, obj.InvSaleInvoiceDetaillist[i].FcyAmount, obj.InvSaleInvoiceDetaillist[i].JobLotDescription, obj.InvSaleInvoiceDetaillist[i].CommissionAmount, obj.InvSaleInvoiceDetaillist[i].ExpenseAmount, obj.InvSaleInvoiceDetaillist[i].FreightAmount, obj.InvSaleInvoiceDetaillist[i].BillAmount, obj.InvSaleInvoiceDetaillist[i].GpNo, obj.InvSaleInvoiceDetaillist[i].VehicleNo, obj.InvSaleInvoiceDetaillist[i].BranchName);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailGridSetting()
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.RootTable.Columns["ItemRateWOExp"].Caption = "Item Rate";
			grdDetail.RootTable.Columns["ItemRate"].Caption = "Item Net Rate";
			grdDetail.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature && !BranchImplemented;
			grdDetail.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["BrandItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["BrandItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdDetail.RootTable.Columns["ItemUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["SaleOrder"].Width = Constants.InventoryConstants.DoNoConstant;
			grdDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["AddLsWt"].Width = Constants.InventoryConstants.AddLessConstant;
			grdDetail.RootTable.Columns["EBTotalWt"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["WeightCutTotal"].Width = Constants.InventoryConstants.WtCut;
			grdDetail.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRateWOExp"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["PackingAddLess"].Width = Constants.InventoryConstants.AddLessConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateCut"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateCutAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ExpenseAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["FreightAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdDetail.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["ItemRateWOExp"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRateWOExp"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PackingAddLess"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PackingAddLess"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PackingAddLess"].FormatString = "0,0";
			grdDetail.RootTable.Columns["PackingAddLess"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetStockWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CommissionAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EBTotalWt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EBTotalWt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EBTotalWt"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EBTotalWt"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExpenseAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FreightAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLsWt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLsWt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLsWt"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLsWt"].TotalFormatString = "#,##0.##";
			ctrlGrdBar2_Load(null, null);
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
			LoadGDN = new frmLoadGDN(UserAccount);
			LoadGDN.ShowDialog();
			LoadGDN.BringToFront();
			LoadInGridDetail();
			BindCommGridOrderNo();
			CommissionAmountCalculateInCaseofPolicy();
			LoadPaymentDetail();
			CalculateOrderWiseAmountForPaymentDetail();
			PaymentAmountReCalculate();
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
			DataTable invGdn = InvSaleInvoice.GdnLoadForSaleInvoice(IDs);
			if (invGdn.Rows.Count <= 0)
			{
				return;
			}
			if (((UltraGridBase)cmbsuppliername).ActiveRow != null && Conversion.ToInt(invGdn.Rows[0]["SupplierCustomerId"]) != Conversion.ToInt(cmbsuppliername.Value))
			{
				throw new Exception("You Can't Load GDN Of Different Customer At Same Time");
			}
			if (btnSave.Enabled && btnSave.Visible)
			{
				string s = (Conversion.ToBool(invGdn.Rows[0]["IsStockReserved"]) ? "Reserved" : "Not Reserved");
				if (CheckReservedNotReservedOnInsertOnly == "")
				{
					CheckReservedNotReservedOnInsertOnly = s;
				}
				else if (CheckReservedNotReservedOnInsertOnly != s)
				{
					throw new Exception("You Can't Load GDN Of Different Reserved Status! Already Loaded rows of:" + CheckReservedNotReservedOnInsertOnly + " Status");
				}
				if (CheckReservedNotReservedOnInsertOnly == "Reserved" && IsStockReservedPerParty)
				{
					ChkOpenADOForm.Visible = true;
					AdvanceDoFormLink.Visible = true;
					ChkOpenADOForm.Checked = true;
				}
				else
				{
					ChkOpenADOForm.Visible = false;
					AdvanceDoFormLink.Visible = false;
					ChkOpenADOForm.Checked = false;
				}
			}
			cmbsuppliername.Value = invGdn.Rows[0]["SupplierCustomerId"];
			((Control)(object)cmbsuppliername).Enabled = false;
			dtComm.Clear();
			for (int j = 0; j < invGdn.Rows.Count; j++)
			{
				int saleOrderId = Conversion.ToInt((object)invGdn.Rows[j]["SaleOrderId"].ToString());
				bool orderExists = false;
				foreach (DataRow row in dtComm.Rows)
				{
					int orderId = Conversion.ToInt((object)row["OrderId"].ToString());
					if (saleOrderId == orderId)
					{
						orderExists = true;
						break;
					}
				}
				if (!orderExists)
				{
					if (Conversion.ToInt(invGdn.Rows[j]["BrokerAgentSupCustId"]) != 0)
					{
						dtComm.Rows.Add(0, invGdn.Rows[j]["SaleOrderId"], invGdn.Rows[j]["SaleOrder"], 0, invGdn.Rows[j]["BrokerAgentSupCustId"], invGdn.Rows[j]["CommissionType"].ToString(), invGdn.Rows[j]["CommRate"].ToString(), invGdn.Rows[j]["UomScheduleIdCmRate"].ToString(), invGdn.Rows[j]["CommAmount"].ToString(), invGdn.Rows[j]["CommissionRemarks"].ToString(), 0);
					}
					if (Conversion.ToInt(invGdn.Rows[j]["OtherCommissionAgentId"]) != 0)
					{
						dtComm.Rows.Add(0, invGdn.Rows[j]["SaleOrderId"], invGdn.Rows[j]["SaleOrder"], 0, invGdn.Rows[j]["OtherCommissionAgentId"], invGdn.Rows[j]["OtherCommissionType"].ToString(), invGdn.Rows[j]["OtherCommissionRate"].ToString(), invGdn.Rows[j]["OtherCommissionUom"].ToString(), invGdn.Rows[j]["OtherCommissionAmount"].ToString(), invGdn.Rows[j]["OtherCommissionRemarks"].ToString(), 0);
					}
				}
			}
			txtRemarks.Text = invGdn.Rows[0]["RemarksHeader"].ToString();
			txtDueDays.Text = invGdn.Rows[0]["OrderDueDays"].ToString();
			DueDate.Value = Conversion.ToDateTime(invGdn.Rows[0]["OrderDueDate"]);
			DueDateGenerate();
			bool flag = false;
			for (int i = 0; i < invGdn.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int k = 0; k < rows.Length; k++)
					{
						if (Conversion.ToInt(rows[k].Cells["Id"].Value) == Conversion.ToInt(invGdn.Rows[i]["Id"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtGrid.Rows.Add(invGdn.Rows[i]["OtherCategoryId"], invGdn.Rows[i]["OtherCategory"], invGdn.Rows[i]["RefDocumentTypeId"], invGdn.Rows[i]["RefDocIdNo"], invGdn.Rows[i]["RefDocSubIdNo"], invGdn.Rows[i]["PurchaseGLAC"], invGdn.Rows[i]["Id"], invGdn.Rows[i]["invGdnId"], invGdn.Rows[i]["ItemId"], invGdn.Rows[i]["ItemCode"], invGdn.Rows[i]["ItemName"], invGdn.Rows[i]["BrandItemId"], invGdn.Rows[i]["BrandItemCode"], invGdn.Rows[i]["BrandItemName"], invGdn.Rows[i]["CropYear"], invGdn.Rows[i]["ItemUomId"], invGdn.Rows[i]["UOMCodeItem"], invGdn.Rows[i]["PackEquivalent"], invGdn.Rows[i]["ItemQty"], invGdn.Rows[i]["WarehouseId"], invGdn.Rows[i]["WareHouseName"], invGdn.Rows[i]["PackingTypeId"], invGdn.Rows[i]["SaleOrderId"], invGdn.Rows[i]["SaleOrder"], invGdn.Rows[i]["GrossWeight"], invGdn.Rows[i]["AdLsWeight"], invGdn.Rows[i]["EBWPerUnit"], invGdn.Rows[i]["EBWTotal"], invGdn.Rows[i]["NetBillWeight"], invGdn.Rows[i]["StockWeight"], invGdn.Rows[i]["OrderItemRate"], invGdn.Rows[i]["PackingAddLess"], Conversion.ToDouble(invGdn.Rows[i]["OrderItemRate"]), invGdn.Rows[i]["OrderItemRateUOMId"], invGdn.Rows[i]["RateUom"], invGdn.Rows[i]["EquivalentPoRate"], 0, 0, Math.Round(Conversion.ToDouble(invGdn.Rows[i]["ItemAmount"]), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), Math.Round(Conversion.ToDouble(invGdn.Rows[i]["ItemAmount"]), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), invGdn.Rows[i]["ItemDiscount"], 0, invGdn.Rows[i]["JobLotId"], 0, 0, 0, 0, 0, invGdn.Rows[i]["VehicleNo"], invGdn.Rows[i]["GpNo"], invGdn.Rows[i]["CommOnSale"], 0, invGdn.Rows[i]["BranchId"], invGdn.Rows[i]["BranchName"], invGdn.Rows[i]["CostCenterId"], invGdn.Rows[i]["PaymentTermsId"]);
					ItemIdsForComm = ItemIdsForComm + "," + Conversion.ToString(invGdn.Rows[i]["ItemId"]);
				}
				flag = false;
			}
			hasSaleOrder = invGdn.AsEnumerable().Any((DataRow r) => r.Field<int>("SaleOrderId") > 0);
			PAckingAddless();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LoadFreightData(string ID)
	{
		try
		{
			DataTable item = InvSaleInvoice.GetTransporterAndFreightFromGdn(ID);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			dtFreight.Clear();
			for (int i = 0; i < item.Rows.Count; i++)
			{
				if (grdFreight.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdFreight.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["InvGdnId"].Value) == Conversion.ToInt(item.Rows[i]["MainId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag && Conversion.ToInt(item.Rows[i]["CarriageAmount"]) > 0)
				{
					string Remarks = "";
					if (Conversion.ToInt(item.Rows[i]["GpNo"]) > 0)
					{
						Remarks = Remarks + "GpNo: " + Conversion.ToString(item.Rows[i]["GpNo"]);
					}
					if (Conversion.ToString(item.Rows[i]["VehicleNo"]) != string.Empty)
					{
						Remarks = Remarks + "   VehicleNo:  " + Conversion.ToString(item.Rows[i]["VehicleNo"]);
					}
					if (Conversion.ToString(item.Rows[i]["BiltyNo"]) != string.Empty)
					{
						Remarks = Remarks + "   BiltyNo: " + Conversion.ToString(item.Rows[i]["BiltyNo"]);
					}
					if (SubsidiaryAccountAllownOnVouchers)
					{
						dtFreight.Rows.Add(item.Rows[i]["MainId"], item.Rows[i]["Transporter"], item.Rows[i]["TransporterSupCustId"], item.Rows[i]["CarriageAmount"], 0, Remarks);
					}
					else
					{
						dtFreight.Rows.Add(item.Rows[i]["MainId"], item.Rows[i]["Transporter"], 0, item.Rows[i]["CarriageAmount"], 0, Remarks);
					}
				}
				flag = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void LoadExpensesFromGdn(string ID)
	{
		try
		{
			DataTable item = InvSaleInvoice.GetExpensesFromGdn(ID);
			if (item.Rows.Count > 0)
			{
				if (Conversion.ToDouble(dtInvExp.Compute("Sum(Amount)", string.Empty)) == 0.0)
				{
					dtInvExp.Rows.Clear();
				}
				for (int i = 0; i < item.Rows.Count; i++)
				{
					dtInvExp.Rows.Add(item.Rows[i]["ItemId"], item.Rows[i]["Qty"], item.Rows[i]["BagPrice"], Conversion.ToDouble(item.Rows[i]["Qty"]) * Conversion.ToDouble(item.Rows[i]["BagPrice"]), Conversion.ToString(item.Rows[i]["Remarks"]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void LoadExpData(string ID)
	{
		try
		{
			DataTable item = InvSaleInvoice.GetItemQtyAndItemRateFromSO(ID);
			if (item.Rows.Count > 0)
			{
				if (Conversion.ToDouble(dtInvExp.Compute("Sum(Amount)", string.Empty)) == 0.0)
				{
					dtInvExp.Rows.Clear();
				}
				for (int i = 0; i < item.Rows.Count; i++)
				{
					dtInvExp.Rows.Add(0, item.Rows[i]["ItemQty"], item.Rows[i]["BagPrice"], Conversion.ToDouble(item.Rows[i]["ItemQty"]) * Conversion.ToDouble(item.Rows[i]["BagPrice"]), 0);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			string GdnIds = "";
			for (int i = 0; i < LoadGDN.dtGDNS.Rows.Count; i++)
			{
				GdnIds = GdnIds + "," + int.Parse(LoadGDN.dtGDNS.Rows[i][0].ToString());
			}
			LoadDataDetailGridAgainstGP(GdnIds);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			LoadFreightData(GdnIds);
			if (dtFreight.Rows.Count == 0 || dtFreight == null)
			{
				AddRowInFreightGrid();
			}
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			if (dtComm.Rows.Count == 0 || dtComm == null)
			{
				AddRowInCommissionGrid();
			}
			grdComm.RetrieveStructure();
			grdCommissionSettings();
			LoadExpensesFromGdn(GdnIds);
			if (dtInvExp.Rows.Count == 0)
			{
				LoadExpData(GdnIds);
			}
			if (dtInvExp.Rows.Count == 0 || dtInvExp == null)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			ExpProportion();
			FreightProportion();
			CommissionProportion();
			TotalCommissionAmount();
			CalculateCommissionGrid();
			PropotionateCommissiongrid();
			LedgerProportion();
			BillProportion();
			BillAmount();
			GdnIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)cmbcommtype).Text == "Flat")
				{
					double CommRate = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					txtcommamount.Text = Math.Round(CommRate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)cmbcommtype).Text == "Percent" || ((Control)(object)cmbcommtype).Text == "Percentage")
				{
					double CommRate3 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate3 / 100.0;
						txtcommamount.Text = Math.Round(CommAmt2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)cmbcommtype).Text == "Comm Weight")
				{
					double CommRate2 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
					if (Weight != null)
					{
						double num = Conversion.ToDouble((object)Weight.ToString());
						double UOM = Conversion.ToDouble((object)((Control)(object)cmbcommuom).Text.Trim());
						double CommAmt = num / UOM * CommRate2;
						txtcommamount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
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

	private void cmbcommtype_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void cmbcommuom_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void CalculateItemDiscount()
	{
		try
		{
			grd.UpdateData();
			if (grd.CurrentRow.Cells.Count > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					double ItemPercentage = 0.0;
					double ItemDiscount = 0.0;
					double ItemAmount = 0.0;
					ItemPercentage = Conversion.ToDouble((object)item.Cells["ItemDiscount"].Text);
					ItemAmount = Conversion.ToDouble((object)item.Cells["ItemAmount"].Text);
					if (ItemPercentage > 0.0)
					{
						ItemDiscount = ItemAmount * ItemPercentage / 100.0;
						item.Cells["DiscAmount"].Text = ItemDiscount.ToString();
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

	private void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double ItemAmount = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
			Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "FcyAmount"));
			double ExpAmount = GridEX_Helper.GetColumnSum(grdInvExp, "Amount");
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			double TransporterCredit = 0.0;
			double TransporterDebit = 0.0;
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["AccountId"].Value) > 0)
				{
					JournalDebit += Conversion.ToDouble(r2.Cells["Debit"].Value);
					JournalCredit += Conversion.ToDouble(r2.Cells["Credit"].Value);
				}
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					if (cmbsuppliername.Value.ToString() == r.Cells["Transporter"].Value.ToString())
					{
						TransporterDebit += Conversion.ToDouble((object)r.Cells["Debit"].Value.ToString());
						TransporterCredit += Conversion.ToDouble((object)r.Cells["Freight"].Value.ToString());
					}
				}
				else if (txtSupplierGLId.Text.ToString() == r.Cells["Transporter"].Value.ToString())
				{
					TransporterDebit += Conversion.ToDouble((object)r.Cells["Debit"].Value.ToString());
					TransporterCredit += Conversion.ToDouble((object)r.Cells["Freight"].Value.ToString());
				}
			}
			BillAmount = ItemAmount + ExpAmount;
			double TransDifference = Math.Round(TransporterCredit - TransporterDebit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			BillAmount = ((!(TransDifference > 0.0)) ? (BillAmount + Math.Abs(TransDifference)) : (BillAmount - Math.Abs(TransDifference)));
			double GlDifference = Math.Round(JournalCredit - JournalDebit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			double DiffGl = Math.Abs(GlDifference);
			BillAmount = ((!(GlDifference < 0.0)) ? (BillAmount + GlDifference) : (BillAmount - DiffGl));
			double PartyCommAmount = 0.0;
			rows = grdComm.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(item.Cells["CommissionAgentId"].Value))
				{
					PartyCommAmount += Conversion.ToDouble((object)item.Cells["CommissionAmount"].Text);
				}
			}
			BillAmount -= PartyCommAmount;
			txtBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			txtLedgerBalanceAfterBill.Text = Math.Round(LedgerBalance + BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatboth);
			if (grd.RowCount > 0)
			{
				txtFcyAmount.Text = (BillAmount / Conversion.ToDouble((object)txtExchangeRate.Text)).ToString("#,##0.####");
			}
			else
			{
				txtFcyAmount.Text = "0";
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
			if (!CreditAmountInItemSaleGL)
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
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj3 in rows)
				{
					obj3.BeginEdit();
					obj3.Cells["Expense"].Value = 0;
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
				double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
				double CreditForFreight = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString());
				if (CreditForFreight > 0.0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
						obj.Cells["Freights"].Value = CreditForFreight / NetWeight * Weight;
					}
				}
				else
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						obj2.Cells["Freights"].Value = 0;
					}
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
		try
		{
			double TotalAmount = Conversion.ToDouble((object)txtcommamount.Text);
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdGLedger.GetTotal(grdGLedger.RootTable.Columns["Credit"], (AggregateFunction)2).ToString()) + TotalAmount;
			double DebitForFreight = Conversion.ToDouble((object)grdGLedger.GetTotal(grdGLedger.RootTable.Columns["Debit"], (AggregateFunction)2).ToString());
			if (DebitForFreight > CreditForFreight)
			{
				double TotalDiffernce = DebitForFreight - CreditForFreight;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
					obj.Cells["Journal"].Value = TotalDiffernce / NetWeight * Weight;
				}
			}
			else
			{
				if (DebitForFreight > CreditForFreight)
				{
					double TotalDiffernce3 = DebitForFreight - CreditForFreight;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						obj2.Cells["Journal"].Value = TotalDiffernce3;
					}
				}
				if (DebitForFreight == CreditForFreight)
				{
					double TotalDiffernce2 = CreditForFreight - DebitForFreight;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj3 in rows)
					{
						obj3.BeginEdit();
						obj3.Cells["Journal"].Value = TotalDiffernce2;
					}
				}
				if (DebitForFreight < CreditForFreight)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj4 in rows)
					{
						obj4.BeginEdit();
						obj4.Cells["Journal"].Value = 0;
					}
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				double ItemAmount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
				double Expense = Conversion.ToDouble(r.Cells["Expense"].Value);
				double Commission = Conversion.ToDouble(r.Cells["Commission"].Value);
				double Freights = Conversion.ToDouble(r.Cells["Freights"].Value);
				if (CreditAmountInItemSaleGL)
				{
					r.Cells["BillAmount"].Value = Math.Round(ItemAmount - Commission - Freights, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				else
				{
					r.Cells["BillAmount"].Value = Math.Round(ItemAmount + Expense - Commission - Freights, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionProportion()
	{
	}

	private void CommissionAmountCalculateInCaseofPolicy()
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		try
		{
			if (CommissionPolicyIsActive && CommissionPolicyForCommissionAgentIsActive)
			{
				throw new Exception("Configuration 'CommissionPolicyIsActive' and 'CommissionPolicyForCommissionAgentIsActive' can't be be true on the same time,please disable on of them");
			}
			GridEXRow[] rows;
			if (CommissionPolicyIsActive && grd.RowCount > 0)
			{
				double TotalCommAmount2 = 0.0;
				double CommPercent2 = 0.0;
				int CustomerIncentivePartyId2 = 0;
				if (Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString()) > 0.0)
				{
					DataTable dt2 = InvSaleInvoice.GetCommissionRateByCustomerAndItemIdSFromCommPolicy(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						CustomerId = Conversion.ToInt(cmbsuppliername.Value),
						EffectedDate = Conversion.ToDateTime((object)DocDate.Value),
						ItemIds = ItemIdsForComm,
						PolicyTypeId = 1
					});
					if (dt2.Rows.Count > 0)
					{
						CustomerIncentivePartyId2 = Conversion.ToInt(dt2.Rows[0]["CustomerIncentivePartyId"]);
						rows = grd.GetRows();
						foreach (GridEXRow r3 in rows)
						{
							r3.BeginEdit();
							CommPercent2 = (from row in dt2.AsEnumerable()
								where row.Field<int>("ItemId") == Conversion.ToInt(r3.Cells["ItemId"].Value)
								select row).Sum((DataRow row) => Conversion.ToDouble(row["CommissionRate"]));
							double CommAmount2 = Conversion.ToDouble(r3.Cells["ItemAmount"].Value) * CommPercent2 / 100.0;
							TotalCommAmount2 += CommAmount2;
							r3.Cells["Commission"].Value = CommAmount2;
							r3.Cells["CommPercent"].Value = CommPercent2;
							r3.EndEdit();
						}
					}
					else
					{
						rows = grd.GetRows();
						foreach (GridEXRow obj in rows)
						{
							obj.BeginEdit();
							obj.Cells["Commission"].Value = 0;
							obj.Cells["CommPercent"].Value = 0;
							obj.EndEdit();
						}
					}
				}
				rows = grdComm.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["CommissionAmount"].Value = TotalCommAmount2;
					obj2.Cells["CommissionAgentId"].Value = CustomerIncentivePartyId2;
					obj2.EndEdit();
				}
			}
			if (!CommissionPolicyForCommissionAgentIsActive || grd.RowCount <= 0)
			{
				return;
			}
			rows = grdComm.GetRows();
			foreach (GridEXRow rrr in rows)
			{
				double TotalCommAmount = 0.0;
				double CommPercent = 0.0;
				int CustomerIncentivePartyId = 0;
				if (Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString()) > 0.0)
				{
					DataTable dt = InvSaleInvoice.GetCommissionRateByCustomerAndItemIdSFromCommPolicy(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						CustomerId = Conversion.ToInt(rrr.Cells["CommissionAgentId"].Value),
						EffectedDate = Conversion.ToDateTime((object)DocDate.Value),
						ItemIds = ItemIdsForComm,
						PolicyTypeId = 2
					});
					if (dt.Rows.Count > 0)
					{
						CustomerIncentivePartyId = Conversion.ToInt(dt.Rows[0]["CustomerIncentivePartyId"]);
						foreach (GridEXRow r2 in from r in grd.GetRows()
							where Conversion.ToInt(r.Cells["SaleOrderId"].Value) == Conversion.ToInt(rrr.Cells["OrderId"].Value)
							select r)
						{
							r2.BeginEdit();
							CommPercent = (from row in dt.AsEnumerable()
								where row.Field<int>("ItemId") == Conversion.ToInt(r2.Cells["ItemId"].Value)
								select row).Sum((DataRow row) => Conversion.ToDouble(row["CommissionRate"]));
							double CommAmount = Conversion.ToDouble(r2.Cells["ItemAmount"].Value) * CommPercent / 100.0;
							TotalCommAmount += CommAmount;
							r2.Cells["Commission"].Value = CommAmount;
							r2.Cells["CommPercent"].Value = CommPercent;
							r2.EndEdit();
						}
					}
					else
					{
						foreach (GridEXRow item in from r in grd.GetRows()
							where Conversion.ToInt(r.Cells["SaleOrderId"].Value) == Conversion.ToInt(rrr.Cells["OrderId"].Value)
							select r)
						{
							item.BeginEdit();
							item.Cells["Commission"].Value = 0;
							item.Cells["CommPercent"].Value = 0;
							item.EndEdit();
						}
					}
				}
				foreach (GridEXRow item2 in from r in grdComm.GetRows()
					where Conversion.ToInt(r.Cells["OrderId"].Value) == Conversion.ToInt(rrr.Cells["OrderId"].Value)
					select r)
				{
					item2.BeginEdit();
					item2.Cells["CommissionAmount"].Value = TotalCommAmount;
					item2.Cells["CommissionAgentId"].Value = CustomerIncentivePartyId;
					item2.EndEdit();
				}
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dtGrid);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
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

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdFreight).Name);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdComm).Name);
			if (fs != null)
			{
				grdComm.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInvExp).Name);
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

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGLedger).Name);
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

	private void ctrlGrdBar8_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdPaymentDetail, dtPaymentTerm);
			}
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDateGenerate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDateGenerate()
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

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DueDateGenerate();
			cmbsuppliername_ValueChanged(null, null);
			CommissionAmountCalculateInCaseofPolicy();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReportVoucher103()
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReportVoucher103();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlipFormat2_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleInvoicetSlip_303(Id, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			if (Id == 0)
			{
				throw new Exception("PrintId not found...");
			}
			int SupCustId = Conversion.ToInt(cmbsuppliername.Value);
			DateTime Date = Conversion.ToDateTime((object)DocDate.Value);
			CommonServices.SaleInvoicetSlip_301(Id, SupCustId, 0, Date, Date);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void BtnItemSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleInvoiceItemSlip_303A(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void cmbcommagent_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
			CommissionProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcommtype_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			BillAmount();
			CommissionProportion();
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

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			btnRecordsUpdate.Visible = false;
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
					cmbsuppliername.Focus();
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.S)
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
				else if (e.Control && e.Shift && e.KeyCode == Keys.Delete)
				{
					if (btnDelete.Visible && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbsuppliername.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdFreight).Focus();
					}
					else if (((Control)(object)grdFreight).Focused)
					{
						((Control)(object)grdGLedger).Focus();
					}
					else if (((Control)(object)grdGLedger).Focused)
					{
						((Control)(object)grdComm).Focus();
					}
					else if (((Control)(object)grdComm).Focused)
					{
						((Control)(object)grdInvExp).Focus();
					}
					else
					{
						((Control)(object)grd).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbsuppliername.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					toolStripButton1_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					BtnItemSlip_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btnSlipFormat2_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3))
				{
					btnPrint_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.L)
				{
					toolStripButton3_Click_1(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDetail).Focus();
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
				((Control)(object)grdHistory).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnNewHistory_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
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
			dt.Rows.Add("Ctrl+S", "For Save in form tab and for show data in history tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+F5", "For Focus on Customer Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+P", "For 301 Print");
			dt.Rows.Add("Alt+1", "For Print Slip 303A");
			dt.Rows.Add("Alt+2", "For Print Slip 303");
			dt.Rows.Add("Alt+3", "For Print Voucher 103");
			dt.Rows.Add("Ctrl+L", "For Load GDN");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Customer Name in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
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

	private void grdFreight_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFreight.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.KeyCode == Keys.F1)
			{
				if (grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Transporter")
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
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtGrdGL.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
					dtFreight.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
				}
				if (grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Add")
				{
					AddRowInFreightGrid();
				}
				grdFreight.UpdateData();
				FreightProportion();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					dtFreight.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
					grdFreight.UpdateData();
					FreightProportion();
					BillAmount();
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInFreightGrid();
				grdFreight.UpdateData();
				FreightProportion();
				BillAmount();
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
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdGLedger.CurrentColumn != null && grdGLedger.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtGrdGL.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
				if (grdGLedger.CurrentColumn != null && grdGLedger.CurrentColumn.Key == "Add")
				{
					AddRowInGLGrid();
				}
				grdGLedger.UpdateData();
				BillAmount();
			}
			else if (e.KeyCode == Keys.F1)
			{
				if (grdGLedger.CurrentColumn != null && grdGLedger.CurrentColumn.Key == "AccountId")
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
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInGLGrid();
				grdGLedger.UpdateData();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				dtGrdGL.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				grdGLedger.UpdateData();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdComm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdComm.CurrentRow;
			if (r == null || (int)r.RowType != 0 || (e.Control && e.KeyCode == Keys.Space))
			{
				return;
			}
			if (e.KeyCode == Keys.F1)
			{
				if (grdComm.CurrentColumn != null && grdComm.CurrentColumn.Key == "CommissionAgentId" && !CommissionPolicyIsActive && !CommissionPolicyForCommissionAgentIsActive)
				{
					if (SubsidiaryAccountAllownOnVouchers)
					{
						GrdPopUp obj = new GrdPopUp(dtAccountlst, "SuppliercustomerId", "AccountTitle");
						obj.StartPosition = FormStartPosition.CenterScreen;
						obj.ShowDialog();
						obj.BringToFront();
						r.Cells["CommissionAgentId"].Value = obj.ReturnId;
					}
					else
					{
						GrdPopUp obj2 = new GrdPopUp(dtSupplier, "Id", "CompanyName");
						obj2.StartPosition = FormStartPosition.CenterScreen;
						obj2.ShowDialog();
						obj2.BringToFront();
						r.Cells["CommissionAgentId"].Value = obj2.ReturnId;
					}
				}
			}
			else if ((!e.Control || e.KeyCode != Keys.D) && e.Control)
			{
				_ = e.KeyCode;
				_ = 46;
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
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInvExp.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdInvExp.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtInvExp.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
				}
				if (grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
			}
			else if (e.KeyCode == Keys.F1)
			{
				if (grdInvExp.CurrentColumn.Key == "ItemId")
				{
					GrdPopUp obj = new GrdPopUp(dtOtherItem, "Id", "OtherItemName");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["ItemId"].Value = obj.ReturnId;
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				dtInvExp.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				ExpProportion();
				BillAmount();
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
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					Id = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(Id);
				}
				else if (grdHistory.CurrentColumn.Key == "View")
				{
					int recId = Conversion.ToInt(r.Cells["Id"].Value);
					int SupCustId = Conversion.ToInt(r.Cells["CustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					CommonServices.SaleInvoicetSlip_301(recId, SupCustId, 0, Date, Date);
				}
				else if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(r.Cells["VoucherHeadId"].Value), DocumentTypeId);
				}
				else if (grdHistory.CurrentColumn.Key == "Print_303")
				{
					CommonServices.SaleInvoicetSlip_303(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
				}
				else if (grdHistory.CurrentColumn.Key == "Print_303A")
				{
					CommonServices.SaleInvoiceItemSlip_303A(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights)
				{
					Reset();
					Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
					ReadById(Id);
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				int recId2 = Conversion.ToInt(r.Cells["Id"].Value);
				int SupCustId2 = Conversion.ToInt(r.Cells["CustomerId"].Value);
				DateTime Date2 = Conversion.ToDateTime(r.Cells["DocDate"].Value);
				CommonServices.SaleInvoicetSlip_301(recId2, SupCustId2, 0, Date2, Date2);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool AutoUpdatedRecords(int RecUpdateId)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Expected O, but got Unknown
		//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f72: Expected O, but got Unknown
		//IL_110f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1116: Expected O, but got Unknown
		//IL_12f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ff: Expected O, but got Unknown
		//IL_14e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e7: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return false;
			}
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return false;
			}
			InvSaleInvoice saleInvoice = new InvSaleInvoice();
			saleInvoice.AutoUpdate = 1;
			saleInvoice.Id = RecUpdateId;
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
			FreightProportion();
			ExpProportion();
			CommissionProportion();
			double FreightDebit = 0.0;
			double FreightCredit = 0.0;
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r8 in rows)
			{
				FreightCredit += Conversion.ToDouble(r8.Cells["Freight"].Value);
				FreightDebit += Conversion.ToDouble(r8.Cells["Debit"].Value);
				if (Conversion.ToDouble(r8.Cells["Freight"].Value) > 0.0 && Conversion.ToInt(r8.Cells["Transporter"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against Freight First");
				}
			}
			if (FreightDebitToExpenses && FreightCredit != FreightDebit)
			{
				throw new Exception("Freight Debit Amount not equal to Credit Amount Please Check");
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				if ((Conversion.ToDouble(r7.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r7.Cells["Debit"].Value) > 0) && Conversion.ToInt(r7.Cells["AccountId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against JL First");
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["Amount"].Value) > 0.0 || Conversion.ToInt(r6.Cells["ItemId"].Value) > 0)
				{
					if (Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
					{
						throw new Exception("Please Select an Item Against Expense First");
					}
					if (Conversion.ToDouble(r6.Cells["Amount"].Value) == 0.0)
					{
						throw new Exception("Amount Field is Required Against Expense");
					}
				}
			}
			int Comm = Conversion.ToInt(grdComm.CurrentRow.Cells["CommissionAgentId"].Value);
			string CommType = Conversion.ToString(grdComm.CurrentRow.Cells["CommType"].Value);
			bool IsCommsame = true;
			double CommissionAmt = 0.0;
			rows = grdComm.GetRows();
			foreach (GridEXRow item in rows)
			{
				CommissionAmt += Conversion.ToDouble(item.Cells["CommissionAmount"].Value);
				if (Conversion.ToDouble(item.Cells["CommissionAmount"].Value) > 0.0)
				{
					if (Conversion.ToInt(item.Cells["CommissionAgentId"].Value) == 0)
					{
						throw new Exception("Please Select Commission Agent In Commission Grid");
					}
					if (Conversion.ToInt(item.Cells["CommissionAgentId"].Value) != Comm || Conversion.ToString(item.Cells["CommType"].Value) != CommType)
					{
						IsCommsame = false;
					}
					if (CommissionDebitToExpenses && Conversion.ToInt(item.Cells["DebitAccountId"].Value) == 0)
					{
						throw new Exception("Debit Account Feild Required In Commission Grid ...");
					}
				}
			}
			if (IsCommsame && CommissionAmt > 0.0)
			{
				saleInvoice.CommissionAgentId = Conversion.ToInt(grdComm.CurrentRow.Cells["CommissionAgentId"].Value);
				if (!CommissionPolicyIsActive && !CommissionPolicyForCommissionAgentIsActive)
				{
					saleInvoice.CommissionDebitAcGLId = Conversion.ToInt(grdComm.CurrentRow.Cells["DebitAccountId"].Value);
				}
				else
				{
					saleInvoice.CommissionDebitAcGLId = 0;
				}
				saleInvoice.CommissionType = Conversion.ToString(grdComm.CurrentRow.Cells["CommType"].Value);
				saleInvoice.CommRate = Conversion.ToDouble((object)grdComm.GetTotal(grdComm.RootTable.Columns["CommRate"], (AggregateFunction)3).ToString());
				saleInvoice.CommAmount = Conversion.ToDouble((object)grdComm.GetTotal(grdComm.RootTable.Columns["CommissionAmount"], (AggregateFunction)2).ToString());
				saleInvoice.CommissionRemarks = Conversion.ToString(grdComm.CurrentRow.Cells["Remarks"].Value);
			}
			saleInvoice.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			saleInvoice.DocDate = DocDate.Value;
			saleInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			saleInvoice.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
			saleInvoice.DocumentTypeId = DocumentTypeId;
			saleInvoice.ManualBillNo = txtbillno.Text.Trim();
			saleInvoice.RemarksHeader = txtRemarks.Text.Trim();
			saleInvoice.OtherRemarks = saleInvoice.RemarksHeader;
			saleInvoice.SupplierCustomerId = Conversion.ToInt(cmbsuppliername.Value);
			saleInvoice.SupplierInvoiceDate = DateTime.Now;
			saleInvoice.SupplierInvoiceNo = Conversion.ToInt((object)txtdocno.Text);
			saleInvoice.SupplierReferenceNo = Conversion.ToString((object)txtrefno.Text.Trim());
			saleInvoice.DueDays = Conversion.ToInt((object)txtDueDays.Text.Trim());
			saleInvoice.DueDate = DueDate.Value;
			saleInvoice.UomScheduleIdCmRate = ((Control)(object)cmbcommuom).Text.Trim();
			saleInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			saleInvoice.OrganizationId = UserAccount.OrganizationId;
			saleInvoice.CompanyId = UserAccount.CompanyId;
			saleInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			saleInvoice.EntryUser = UserAccount.ID;
			saleInvoice.EntryDate = DateTime.Now;
			saleInvoice.ModifyDate = DateTime.Now;
			saleInvoice.ModifyUser = UserAccount.ID;
			saleInvoice.ScreenName = Conversion.ToString((object)base.Name);
			saleInvoice.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			saleInvoice.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			saleInvoice.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			saleInvoice.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			rows = grd.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				InvSaleInvoiceDetail Pd = new InvSaleInvoiceDetail();
				Pd.LineId = r5.RowIndex + 1;
				Pd.CropYear = Conversion.ToString(r5.Cells["CropYear"].Value);
				if (Pd.CropYear == string.Empty)
				{
					throw new Exception("CropYear Field is Required");
				}
				Pd.GrossWeight = Conversion.ToDouble(r5.Cells["GrossWeight"].Value);
				Pd.AdLsWeight = Conversion.ToDouble(r5.Cells["AddLsWt"].Value);
				Pd.RefRefDocumentTypeId = Conversion.ToInt(r5.Cells["RefRefDocumentTypeId"].Value);
				Pd.RefRefDocIdNo = Conversion.ToInt(r5.Cells["RefRefDocIdNo"].Value);
				Pd.RefDocSubId = Conversion.ToInt(r5.Cells["RefDocSubId"].Value);
				Pd.SaleOrderId = Conversion.ToInt(r5.Cells["SaleOrderId"].Value);
				Pd.SaleOrder = Conversion.ToInt(r5.Cells["SaleOrder"].Value);
				Pd.UOMCodeItem = Conversion.ToString(r5.Cells["UOMCodeItem"].Value);
				if (Pd.SaleOrderId == 0)
				{
					throw new Exception("SaleOrderId Field Required");
				}
				Pd.InvGdnDetailId = Conversion.ToInt(r5.Cells["Id"].Value);
				if (Pd.InvGdnDetailId == 0)
				{
					throw new Exception("InvGdnDetailId not found");
				}
				Pd.InvGdnId = Conversion.ToInt(r5.Cells["InvGdnId"].Value);
				if (Pd.InvGdnId == 0)
				{
					throw new Exception("InvGdnId not found");
				}
				Pd.ItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
				Pd.BrandItemId = Conversion.ToInt(r5.Cells["BrandItemId"].Value);
				if (Pd.ItemId == 0)
				{
					throw new Exception("Item Required");
				}
				Pd.ItemQty = Conversion.ToDouble(r5.Cells["ItemQty"].Value);
				if (Pd.ItemQty == 0.0)
				{
					throw new Exception("ItemQty mustbe greater than zero");
				}
				Pd.ItemUOMId = Conversion.ToInt(r5.Cells["ItemUomId"].Value);
				if (Pd.ItemUOMId == 0)
				{
					throw new Exception("ItemUOMId Field Required");
				}
				Pd.JobLotId = Conversion.ToInt(r5.Cells["JobLotId"].Value);
				if (Pd.JobLotId == 0)
				{
					throw new Exception("JobLot Required");
				}
				Pd.NetBillWeight = Conversion.ToDouble(r5.Cells["NetBillWeight"].Value);
				if (Pd.NetBillWeight == 0.0)
				{
					throw new Exception("NetBillWeight Field Required");
				}
				Pd.NetStockWeight = Conversion.ToDouble(r5.Cells["StockWeight"].Value);
				if (Pd.NetStockWeight == 0.0)
				{
					throw new Exception("NetStockWeight Field Required");
				}
				Pd.EBTotalWt = Conversion.ToDouble(r5.Cells["EmptyBagsDeduction"].Value);
				Pd.EBWeight = Conversion.ToDouble(r5.Cells["EBWPerUnit"].Value);
				Pd.PackingTypeId = Conversion.ToInt(r5.Cells["PackingTypeId"].Value);
				if (Pd.PackingTypeId == 0)
				{
					throw new Exception("PackingType Required");
				}
				Pd.ItemRateWOExp = Conversion.ToDecimal(r5.Cells["ItemRateWOExp"].Value);
				if (Pd.ItemRateWOExp == 0m)
				{
					throw new Exception("ItemRate Field Required");
				}
				Pd.PackingAddLess = Conversion.ToDecimal(r5.Cells["PackingAddLess"].Value);
				Pd.ItemRate = Conversion.ToDouble(r5.Cells["ItemRate"].Value);
				if (Pd.ItemRate == 0.0)
				{
					throw new Exception("ItemNetRate Field Required");
				}
				Pd.UomScheduleIdRate = Conversion.ToInt(r5.Cells["OrderItemRateUOMId"].Value);
				if (Pd.UomScheduleIdRate == 0)
				{
					throw new Exception("Rate UOM Required");
				}
				Pd.RateUOM = Conversion.ToInt(r5.Cells["RateEquivalent"].Value);
				Pd.RateCut = Conversion.ToDouble(r5.Cells["RateCut"].Value);
				Pd.RateCutAmount = Conversion.ToDouble(r5.Cells["RateCutAmount"].Value);
				Pd.ItemAmount = Math.Round(Conversion.ToDouble(r5.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				Pd.ItemDiscount = Conversion.ToDouble(r5.Cells["ItemDiscount"].Value);
				Pd.ItemDiscountAmount = Conversion.ToDouble(r5.Cells["DiscAmount"].Value);
				if (Pd.ItemAmount == 0.0)
				{
					throw new Exception("ItemAmount Field Required");
				}
				Pd.WarehouseId = Conversion.ToInt(r5.Cells["WarehouseId"].Value);
				if (Pd.WarehouseId == 0)
				{
					throw new Exception("Warehouse Required");
				}
				Pd.BillAmount = Math.Round(Conversion.ToDouble(r5.Cells["BillAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (!FreightDebitToExpenses)
				{
					Pd.FreightAmount = Conversion.ToDouble(r5.Cells["Freights"].Value);
				}
				Pd.JournalAmount = Conversion.ToDouble(r5.Cells["Journal"].Value);
				Pd.ExpenseAmount = Conversion.ToDouble(r5.Cells["Expense"].Value);
				Pd.CommOnSale = Conversion.ToBool(r5.Cells["CommOnSale"].Value);
				if (!CommissionDebitToExpenses)
				{
					Pd.CommissionAmount = Conversion.ToDouble(r5.Cells["Commission"].Value);
				}
				Pd.GpDate = DateTime.Now;
				Pd.GpNo = Conversion.ToInt(r5.Cells["GpNo"].Value);
				Pd.BranchId = Conversion.ToInt(r5.Cells["BranchId"].Value);
				Pd.VehicleNo = Conversion.ToString(r5.Cells["VehicleNo"].Value);
				Pd.CostCenterId = Conversion.ToInt(r5.Cells["CostCenterId"].Value);
				saleInvoice.InvSaleInvoiceDetaillist.Add(Pd);
			}
			saleInvoice.InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
			rows = grdFreight.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["Transporter"].Value.ToString()) != 0)
				{
					InvSaleInvoiceFreight PF = new InvSaleInvoiceFreight();
					PF.InvGdnId = Conversion.ToInt(r4.Cells["InvGdnId"].Value);
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PF.TansporterId = Conversion.ToInt(r4.Cells["GlAccountId"].Value);
						PF.TransporterSupCustId = Conversion.ToInt(r4.Cells["Transporter"].Value);
					}
					else
					{
						PF.TansporterId = Conversion.ToInt(r4.Cells["GlAccountId"].Value);
						PF.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PF.FreightAmount = Conversion.ToInt(r4.Cells["Freight"].Value);
					PF.Debit = Conversion.ToInt(r4.Cells["Debit"].Value);
					PF.Remarks = Conversion.ToString(r4.Cells["Remarks"].Value);
					if (PF.Remarks == null || PF.Remarks == string.Empty)
					{
						throw new Exception("Remarks Field Requried in Freight Grid");
					}
					saleInvoice.InvSaleInvoiceFreightList.Add(PF);
				}
			}
			saleInvoice.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
					PE.InvRevExpItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r3.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDouble(r3.Cells["Amount"].Value);
					PE.CustomRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					PE.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					if (PE.Qty > 0.0 && PE.Rate > 0.0)
					{
						PE.Remarks = "ItemName : " + Conversion.ToString((object)r3.Cells["ItemId"].Text.Trim()) + "  " + $"Qty: {PE.Qty} " + $"Rate: {PE.Rate} " + PE.Remarks;
					}
					saleInvoice.InvSaleInvoiceExpenseList.Add(PE);
				}
			}
			saleInvoice.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["AccountId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceJournal PJ2 = new InvSaleInvoiceJournal();
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PJ2.ChartofAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
						PJ2.TransporterSupCustId = Conversion.ToInt(r2.Cells["AccountId"].Value);
					}
					else
					{
						PJ2.ChartofAccountId = Conversion.ToInt(r2.Cells["GlAccountId"].Value);
						PJ2.TransporterSupCustId = Conversion.ToInt((object)0);
					}
					PJ2.JvRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					PJ2.JvPrcnt = Conversion.ToDouble(r2.Cells["Percentage"].Value);
					PJ2.JvQty = Conversion.ToDouble(r2.Cells["Qty"].Value);
					PJ2.JvRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
					PJ2.JvDebit = Conversion.ToDouble(r2.Cells["Debit"].Value);
					PJ2.JvCredit = Conversion.ToDouble(r2.Cells["Credit"].Value);
					saleInvoice.InvSaleInvoiceJournalList.Add(PJ2);
				}
			}
			saleInvoice.InvSaleInvoiceCommission = new List<InvSaleInvoiceCommission>();
			rows = grdComm.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["CommissionAgentId"].Value.ToString()) != 0 && Conversion.ToDouble(r.Cells["CommissionAmount"].Value) > 0.0)
				{
					InvSaleInvoiceCommission PJ = new InvSaleInvoiceCommission();
					PJ.Id = Conversion.ToInt(r.Cells["Id"].Value);
					PJ.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					PJ.InvSaleInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					if (SubsidiaryAccountAllownOnVouchers)
					{
						PJ.CommDebitSupCustId = Conversion.ToInt(r.Cells["DebitAccountId"].Value);
						PJ.DebitAccountId = Conversion.ToInt(r.Cells["GlAccountId"].Value);
					}
					else
					{
						PJ.CommDebitSupCustId = 0;
						PJ.DebitAccountId = Conversion.ToInt(r.Cells["GlAccountId"].Value);
					}
					PJ.CommissionAgentId = Conversion.ToInt(r.Cells["CommissionAgentId"].Value);
					if (Conversion.ToString(r.Cells["CommType"].Value) == "Percent")
					{
						PJ.CommType = 2.0;
					}
					if (Conversion.ToString(r.Cells["CommType"].Value) == "Flat")
					{
						PJ.CommType = 1.0;
					}
					if (Conversion.ToString(r.Cells["CommType"].Value) == "Comm Weight")
					{
						PJ.CommType = 3.0;
					}
					if (Conversion.ToDouble(r.Cells["CommType"].Value) > 0.0)
					{
						PJ.CommType = Conversion.ToDouble(r.Cells["CommType"].Value);
					}
					PJ.Rate = Conversion.ToDouble(r.Cells["CommRate"].Value);
					PJ.RateUom = Conversion.ToDouble(r.Cells["CommUom"].Value);
					PJ.CommAmount = Conversion.ToDouble(r.Cells["CommissionAmount"].Value);
					PJ.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					if (CommissionPolicyIsActive || CommissionPolicyForCommissionAgentIsActive)
					{
						PJ.Rate = PJ.CommAmount;
						PJ.CommType = 1.0;
						PJ.RateUom = 0.0;
						PJ.SaleOrderId = 0;
					}
					saleInvoice.InvSaleInvoiceCommission.Add(PJ);
				}
			}
			InvSaleInvoice.Save(saleInvoice);
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
		return true;
	}

	private void btnRecordsUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = InvSaleInvoice.GetSaleInvoiceIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, DocumentTypeId);
			if (dtIds != null && dtIds.Rows.Count > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				bool ShouldStop = false;
				for (int i = 0; i < dtIds.Rows.Count; i++)
				{
					int RecId = Conversion.ToInt(dtIds.Rows[i]["Id"]);
					ReadById(RecId);
					ShouldStop = AutoUpdatedRecords(RecId);
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

	private void ChkOpenADOForm_Click(object sender, EventArgs e)
	{
		FormHelper.CanOpenForm("AdvanceDeliveryOrderPP", delegate
		{
			AdvanceDeliveryOrderPP advanceDeliveryOrderPP = new AdvanceDeliveryOrderPP(UserAccount);
			advanceDeliveryOrderPP.Show();
			advanceDeliveryOrderPP.BringToFront();
		}, UserAccount);
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
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
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Expected O, but got Unknown
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Expected O, but got Unknown
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Expected O, but got Unknown
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Expected O, but got Unknown
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Expected O, but got Unknown
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Expected O, but got Unknown
		//IL_093e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0948: Expected O, but got Unknown
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e1: Expected O, but got Unknown
		//IL_10ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f8: Expected O, but got Unknown
		//IL_142b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1435: Expected O, but got Unknown
		//IL_1442: Unknown result type (might be due to invalid IL or missing references)
		//IL_144c: Expected O, but got Unknown
		//IL_187f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1889: Expected O, but got Unknown
		//IL_1896: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a0: Expected O, but got Unknown
		//IL_18ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b7: Expected O, but got Unknown
		//IL_1bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfa: Expected O, but got Unknown
		//IL_1c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c11: Expected O, but got Unknown
		//IL_1c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c28: Expected O, but got Unknown
		//IL_1fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fee: Expected O, but got Unknown
		//IL_1ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2005: Expected O, but got Unknown
		//IL_2012: Unknown result type (might be due to invalid IL or missing references)
		//IL_201c: Expected O, but got Unknown
		//IL_235f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2369: Expected O, but got Unknown
		//IL_2376: Unknown result type (might be due to invalid IL or missing references)
		//IL_2380: Expected O, but got Unknown
		//IL_238d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2397: Expected O, but got Unknown
		//IL_6b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b8d: Expected O, but got Unknown
		//IL_6bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bbb: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdComm_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		Appearance appearance72 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.InvfrmSaleInvoice));
		Appearance appearance51 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance41 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.tabPaymentDetail = new System.Windows.Forms.TabPage();
		this.grdPaymentDetail = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.ctrlGrdBar8 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.grdGLedger = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdComm = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdFreight = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.txtLedgerBalance = new System.Windows.Forms.TextBox();
		this.txtLedgerBalanceAfterBill = new System.Windows.Forms.TextBox();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbCommissionDebitAc = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.lblDebitAccount = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtcommremarks = new System.Windows.Forms.RichTextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.cmbcommtype = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbcommuom = new UltraCombo();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtrefno = new System.Windows.Forms.TextBox();
		this.labelRemarks = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.cmbcommagent = new UltraCombo();
		this.cmbsuppliername = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.AdvanceDoFormLink = new System.Windows.Forms.LinkLabel();
		this.ChkOpenADOForm = new System.Windows.Forms.CheckBox();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.BtnItemSlip = new System.Windows.Forms.ToolStripButton();
		this.btnSlipFormat2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.cmbLocationType = new UltraCombo();
		this.ChkPrintSlip = new System.Windows.Forms.CheckBox();
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.label10 = new System.Windows.Forms.Label();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel24 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel26 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label13 = new System.Windows.Forms.Label();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.tabPaymentDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetail).BeginInit();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel21.SuspendLayout();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel23.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdComm).BeginInit();
		this.panel19.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.panel17.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel24.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel13.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1266, 706);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1266, 706);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 8;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1258, 677);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel7.AutoScroll = true;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel4);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.panel2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1252, 671);
		this.panel7.TabIndex = 4;
		this.panel9.Controls.Add(this.tabControl2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 417);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1252, 254);
		this.panel9.TabIndex = 3;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabPaymentDetail);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1252, 254);
		this.tabControl2.TabIndex = 3;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.panel5);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1244, 225);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1238, 198);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).Tag = "GrdDetail";
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1238, 21);
		this.panel5.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1199, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(3, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(74, 14);
		this.label9.TabIndex = 43;
		this.label9.Text = "Detail  Grid";
		this.tabPaymentDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentDetail);
		this.tabPaymentDetail.Controls.Add(this.panel3);
		this.tabPaymentDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentDetail.Name = "tabPaymentDetail";
		this.tabPaymentDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabPaymentDetail.Size = new System.Drawing.Size(1244, 225);
		this.tabPaymentDetail.TabIndex = 1;
		this.tabPaymentDetail.Text = "Payment Detail";
		this.tabPaymentDetail.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPaymentDetail).BackColor = System.Drawing.Color.White;
		grdPaymentDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentDetail.DesignTimeLayout = grdPaymentDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Name = "grdPaymentDetail";
		((ScrollableBase)this.grdPaymentDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Size = new System.Drawing.Size(1238, 197);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).TabIndex = 6;
		this.grdPaymentDetail.TotalRow = (InheritableBoolean)1;
		this.grdPaymentDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentDetail).VisualStyle = (VisualStyle)4;
		this.grdPaymentDetail.CellUpdated += new ColumnActionEventHandler(grdPaymentDetail_CellUpdated);
		this.grdPaymentDetail.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentDetail_KeyDown);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label24);
		this.panel3.Controls.Add(this.ctrlGrdBar8);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1238, 22);
		this.panel3.TabIndex = 1;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(3, 4);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(128, 14);
		this.label24.TabIndex = 40;
		this.label24.Text = "Payment Detail Grid";
		this.ctrlGrdBar8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar8.FormName = this;
		this.ctrlGrdBar8.Location = new System.Drawing.Point(1199, 1);
		this.ctrlGrdBar8.MyGrid = this.grdPaymentDetail;
		this.ctrlGrdBar8.Name = "ctrlGrdBar8";
		this.ctrlGrdBar8.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar8.TabIndex = 101;
		this.ctrlGrdBar8.Load += new System.EventHandler(ctrlGrdBar8_Load);
		this.panel4.Controls.Add(this.panel10);
		this.panel4.Controls.Add(this.panel15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 143);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1252, 274);
		this.panel4.TabIndex = 2;
		this.panel10.Controls.Add(this.panel20);
		this.panel10.Controls.Add(this.panel22);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(603, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(649, 274);
		this.panel10.TabIndex = 1;
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel20.Controls.Add(this.panel21);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel20.Location = new System.Drawing.Point(0, 140);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(649, 134);
		this.panel20.TabIndex = 1;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(649, 113);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Tag = "GrdExpenses";
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.ctrlGrdBar6);
		this.panel21.Controls.Add(this.label5);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(649, 21);
		this.panel21.TabIndex = 0;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(609, -2);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar6.MyGrid = this.grdInvExp;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar6.TabIndex = 43;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(135, 14);
		this.label5.TabIndex = 42;
		this.label5.Text = "Other Item Expenses";
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.panel22.Controls.Add(this.panel23);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(649, 140);
		this.panel22.TabIndex = 0;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(649, 118);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Tag = "GrdPartyAddLess";
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlGrdBar7);
		this.panel23.Controls.Add(this.label2);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(649, 22);
		this.panel23.TabIndex = 0;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(608, -2);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdGLedger;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar7.TabIndex = 42;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(102, 14);
		this.label2.TabIndex = 41;
		this.label2.Text = "Party Add/Less";
		this.panel15.Controls.Add(this.panel18);
		this.panel15.Controls.Add(this.panel16);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(603, 274);
		this.panel15.TabIndex = 0;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdComm);
		this.panel18.Controls.Add(this.panel19);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 140);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(603, 134);
		this.panel18.TabIndex = 1;
		((ScrollableBase)this.grdComm).BackColor = System.Drawing.Color.White;
		grdComm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdComm.DesignTimeLayout = grdComm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdComm).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdComm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdComm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdComm).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdComm).Name = "grdComm";
		((ScrollableBase)this.grdComm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdComm).Size = new System.Drawing.Size(603, 113);
		((System.Windows.Forms.Control)(object)this.grdComm).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdComm).Tag = "GrdCommission";
		this.grdComm.TotalRow = (InheritableBoolean)1;
		this.grdComm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdComm).VisualStyle = (VisualStyle)4;
		this.grdComm.CellUpdated += new ColumnActionEventHandler(grdComm_CellUpdated);
		this.grdComm.UpdatingCell += new UpdatingCellEventHandler(grdComm_UpdatingCell);
		this.grdComm.ColumnButtonClick += new ColumnActionEventHandler(grdComm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdComm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdComm_KeyDown);
		this.panel19.BackColor = System.Drawing.Color.Teal;
		this.panel19.Controls.Add(this.ctrlGrdBar5);
		this.panel19.Controls.Add(this.label6);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(603, 21);
		this.panel19.TabIndex = 0;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(562, -1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.grdComm;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar5.TabIndex = 44;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(107, 14);
		this.label6.TabIndex = 43;
		this.label6.Text = "Commission Grid";
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.panel16.Controls.Add(this.panel17);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(603, 140);
		this.panel16.TabIndex = 0;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(603, 118);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdFreight).Tag = "GrdFreight";
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.UpdatingCell += new UpdatingCellEventHandler(grdFreight_UpdatingCell);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreight).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ctrlGrdBar4);
		this.panel17.Controls.Add(this.label1);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(603, 22);
		this.panel17.TabIndex = 0;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(563, -2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdFreight;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar4.TabIndex = 21;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(107, 14);
		this.label1.TabIndex = 40;
		this.label1.Text = "Transporter Grid";
		this.panel8.Controls.Add(this.txtBranchSrNo);
		this.panel8.Controls.Add(this.txtbillno);
		this.panel8.Controls.Add(this.txtDueDays);
		this.panel8.Controls.Add(this.txtLedgerBalance);
		this.panel8.Controls.Add(this.txtLedgerBalanceAfterBill);
		this.panel8.Controls.Add(this.txtBillAmount);
		this.panel8.Controls.Add(this.label43);
		this.panel8.Controls.Add(this.txtExchangeRate);
		this.panel8.Controls.Add(this.txtFcyAmount);
		this.panel8.Controls.Add(this.label44);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.panel8.Controls.Add(this.label45);
		this.panel8.Controls.Add(this.label8);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc);
		this.panel8.Controls.Add(this.label17);
		this.panel8.Controls.Add(this.lblDebitAccount);
		this.panel8.Controls.Add(this.txtdocno);
		this.panel8.Controls.Add(this.txtcommremarks);
		this.panel8.Controls.Add(this.DocDate);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommtype);
		this.panel8.Controls.Add(this.label12);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommuom);
		this.panel8.Controls.Add(this.txtRemarks);
		this.panel8.Controls.Add(this.txtrefno);
		this.panel8.Controls.Add(this.labelRemarks);
		this.panel8.Controls.Add(this.txtCommAgentId);
		this.panel8.Controls.Add(this.label7);
		this.panel8.Controls.Add(this.txtSupplierGLId);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsuppliername);
		this.panel8.Controls.Add(this.label20);
		this.panel8.Controls.Add(this.label23);
		this.panel8.Controls.Add(this.label22);
		this.panel8.Controls.Add(this.label26);
		this.panel8.Controls.Add(this.label18);
		this.panel8.Controls.Add(this.DueDate);
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.label11);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add(this.label25);
		this.panel8.Controls.Add(this.label21);
		this.panel8.Controls.Add(this.txtcommamount);
		this.panel8.Controls.Add(this.txtcommrate);
		this.panel8.Controls.Add(this.label19);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 55);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1252, 88);
		this.panel8.TabIndex = 0;
		this.txtBranchSrNo.BackColor = System.Drawing.Color.White;
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtBranchSrNo.Location = new System.Drawing.Point(196, 3);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.ReadOnly = true;
		this.txtBranchSrNo.Size = new System.Drawing.Size(57, 22);
		this.txtBranchSrNo.TabIndex = 1;
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(429, 59);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(108, 23);
		this.txtbillno.TabIndex = 7;
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(429, 3);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(108, 23);
		this.txtDueDays.TabIndex = 5;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.txtDueDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDays_KeyPress);
		this.txtLedgerBalance.BackColor = System.Drawing.SystemColors.Menu;
		this.txtLedgerBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLedgerBalance.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtLedgerBalance.Location = new System.Drawing.Point(633, 2);
		this.txtLedgerBalance.Name = "txtLedgerBalance";
		this.txtLedgerBalance.ReadOnly = true;
		this.txtLedgerBalance.Size = new System.Drawing.Size(158, 24);
		this.txtLedgerBalance.TabIndex = 8;
		this.txtLedgerBalance.TabStop = false;
		this.txtLedgerBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLedgerBalanceAfterBill.BackColor = System.Drawing.SystemColors.Menu;
		this.txtLedgerBalanceAfterBill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLedgerBalanceAfterBill.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtLedgerBalanceAfterBill.Location = new System.Drawing.Point(633, 58);
		this.txtLedgerBalanceAfterBill.Name = "txtLedgerBalanceAfterBill";
		this.txtLedgerBalanceAfterBill.ReadOnly = true;
		this.txtLedgerBalanceAfterBill.Size = new System.Drawing.Size(158, 24);
		this.txtLedgerBalanceAfterBill.TabIndex = 10;
		this.txtLedgerBalanceAfterBill.TabStop = false;
		this.txtLedgerBalanceAfterBill.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtBillAmount.Location = new System.Drawing.Point(633, 28);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(158, 29);
		this.txtBillAmount.TabIndex = 9;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillAmount.TextChanged += new System.EventHandler(txtBillAmount_TextChanged);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(790, 61);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(84, 19);
		this.label43.TabIndex = 87;
		this.label43.Text = "Fcy Amount";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(876, 31);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(126, 23);
		this.txtExchangeRate.TabIndex = 12;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtFcyAmount.Location = new System.Drawing.Point(876, 58);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(126, 25);
		this.txtFcyAmount.TabIndex = 13;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(790, 35);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(84, 15);
		this.label44.TabIndex = 86;
		this.label44.Text = "Exchange Rate";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(876, 1);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(126, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(790, 7);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(55, 15);
		this.label45.TabIndex = 85;
		this.label45.Text = "Fcy Code";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(365, 63);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(66, 15);
		this.label8.TabIndex = 40;
		this.label8.Text = "Manual Bill";
		this.CmbCommissionDebitAc.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionDebitAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionDebitAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionDebitAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionDebitAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionDebitAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).Location = new System.Drawing.Point(1036, 204);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).Name = "CmbCommissionDebitAc";
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).Size = new System.Drawing.Size(103, 24);
		((System.Windows.Forms.Control)(object)this.CmbCommissionDebitAc).TabIndex = 39;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(3, 7);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(105, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Br / Inv No && Date";
		this.lblDebitAccount.AutoSize = true;
		this.lblDebitAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDebitAccount.Location = new System.Drawing.Point(973, 209);
		this.lblDebitAccount.Name = "lblDebitAccount";
		this.lblDebitAccount.Size = new System.Drawing.Size(53, 15);
		this.lblDebitAccount.TabIndex = 38;
		this.lblDebitAccount.Text = "Debit Ac";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.txtdocno.Location = new System.Drawing.Point(115, 3);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(80, 22);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.txtcommremarks.Location = new System.Drawing.Point(713, 287);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(248, 50);
		this.txtcommremarks.TabIndex = 15;
		this.txtcommremarks.Text = "";
		this.txtcommremarks.Visible = false;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(254, 3);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(108, 22);
		this.DocDate.TabIndex = 2;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.cmbcommtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(713, 231);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(93, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TextChanged += new System.EventHandler(cmbcommtype_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Leave += new System.EventHandler(cmbcommtype_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(3, 63);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(97, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Customer Ref No";
		this.cmbcommuom.AllowNull = (DefaultableBoolean)1;
		this.cmbcommuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Location = new System.Drawing.Point(713, 258);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Name = "cmbcommuom";
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Size = new System.Drawing.Size(93, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).TextChanged += new System.EventHandler(cmbcommuom_Leave);
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(1004, 31);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(243, 53);
		this.txtRemarks.TabIndex = 14;
		this.txtrefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrefno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrefno.Location = new System.Drawing.Point(115, 59);
		this.txtrefno.Name = "txtrefno";
		this.txtrefno.Size = new System.Drawing.Size(247, 23);
		this.txtrefno.TabIndex = 4;
		this.labelRemarks.BackColor = System.Drawing.Color.LightGray;
		this.labelRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.labelRemarks.Location = new System.Drawing.Point(1004, 4);
		this.labelRemarks.Name = "labelRemarks";
		this.labelRemarks.Size = new System.Drawing.Size(243, 23);
		this.labelRemarks.TabIndex = 20;
		this.labelRemarks.Text = "Remarks";
		this.labelRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1036, 231);
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(31, 22);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(3, 35);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(93, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Customer Name";
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1036, 256);
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(31, 22);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(713, 204);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(248, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Leave += new System.EventHandler(cmbcommagent_Leave);
		this.cmbsuppliername.AllowNull = (DefaultableBoolean)1;
		this.cmbsuppliername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsuppliername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsuppliername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance88).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsuppliername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Location = new System.Drawing.Point(115, 29);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Name = "cmbsuppliername";
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).TabIndex = 3;
		this.cmbsuppliername.ValueChanged += new System.EventHandler(cmbsuppliername_ValueChanged);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(627, 209);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.label20.Visible = false;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(365, 35);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(57, 15);
		this.label23.TabIndex = 36;
		this.label23.Text = "Due Date";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(365, 7);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(58, 15);
		this.label22.TabIndex = 36;
		this.label22.Text = "Due Days";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(810, 263);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label26.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(539, 63);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(96, 15);
		this.label18.TabIndex = 18;
		this.label18.Text = "After Bill Balance";
		this.DueDate.CustomFormat = "dd-MMM-yy";
		this.DueDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(429, 31);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(108, 22);
		this.DueDate.TabIndex = 6;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(539, 32);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(93, 20);
		this.label16.TabIndex = 18;
		this.label16.Text = "Bill Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(627, 236);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(71, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.label11.Visible = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(539, 7);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(87, 15);
		this.label3.TabIndex = 18;
		this.label3.Text = "Ledger Balance";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(649, 287);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(52, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label25.Visible = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(812, 236);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(30, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Rate";
		this.label21.Visible = false;
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(865, 260);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(96, 21);
		this.txtcommamount.TabIndex = 14;
		this.txtcommamount.Visible = false;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.Location = new System.Drawing.Point(865, 233);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(96, 21);
		this.txtcommrate.TabIndex = 12;
		this.txtcommrate.Visible = false;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(627, 263);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(74, 15);
		this.label19.TabIndex = 14;
		this.label19.Text = "Comm UOM";
		this.label19.Visible = false;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.AdvanceDoFormLink);
		this.panel2.Controls.Add(this.ChkOpenADOForm);
		this.panel2.Controls.Add(this.chkCustomAccounts);
		this.panel2.Controls.Add(this.labelLocationType);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel2.Controls.Add(this.ChkPrintSlip);
		this.panel2.Controls.Add(this.btnRecordsUpdate);
		this.panel2.Controls.Add(this.ChkBok);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1252, 55);
		this.panel2.TabIndex = 0;
		this.AdvanceDoFormLink.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.AdvanceDoFormLink.AutoSize = true;
		this.AdvanceDoFormLink.Location = new System.Drawing.Point(740, 32);
		this.AdvanceDoFormLink.Name = "AdvanceDoFormLink";
		this.AdvanceDoFormLink.Size = new System.Drawing.Size(120, 16);
		this.AdvanceDoFormLink.TabIndex = 514;
		this.AdvanceDoFormLink.TabStop = true;
		this.AdvanceDoFormLink.Text = "AdvanceDO Form";
		this.AdvanceDoFormLink.Click += new System.EventHandler(ChkOpenADOForm_Click);
		this.ChkOpenADOForm.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkOpenADOForm.AutoSize = true;
		this.ChkOpenADOForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.ChkOpenADOForm.ForeColor = System.Drawing.Color.Blue;
		this.ChkOpenADOForm.Location = new System.Drawing.Point(725, 35);
		this.ChkOpenADOForm.Name = "ChkOpenADOForm";
		this.ChkOpenADOForm.Size = new System.Drawing.Size(15, 14);
		this.ChkOpenADOForm.TabIndex = 513;
		this.ChkOpenADOForm.UseVisualStyleBackColor = true;
		this.chkCustomAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(862, 31);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(153, 20);
		this.chkCustomAccounts.TabIndex = 512;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(240, 34);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 510;
		this.labelLocationType.Text = "Location Type";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[17]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator5, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator4, this.btnAttachment, this.toolStripSeparator3, this.toolStripButton1,
			this.BtnItemSlip, this.btnSlipFormat2, this.toolStripSeparator2, this.btnPrint, this.toolStripButton3, this.toolStripSeparator1, this.btnshortcutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1252, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = (System.Drawing.Image)resources.GetObject("btnDelete.Image");
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = (System.Drawing.Image)resources.GetObject("btnAttachment.Image");
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(85, 24);
		this.toolStripButton1.Text = "301-Slip";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.BtnItemSlip.Image = (System.Drawing.Image)resources.GetObject("BtnItemSlip.Image");
		this.BtnItemSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnItemSlip.Name = "BtnItemSlip";
		this.BtnItemSlip.Size = new System.Drawing.Size(129, 24);
		this.BtnItemSlip.Text = "303A-Item Slip";
		this.BtnItemSlip.Click += new System.EventHandler(BtnItemSlip_Click);
		this.btnSlipFormat2.Image = (System.Drawing.Image)resources.GetObject("btnSlipFormat2.Image");
		this.btnSlipFormat2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlipFormat2.Name = "btnSlipFormat2";
		this.btnSlipFormat2.Size = new System.Drawing.Size(85, 24);
		this.btnSlipFormat2.Text = "303-Slip";
		this.btnSlipFormat2.Click += new System.EventHandler(btnSlipFormat2_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(115, 24);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(92, 24);
		this.toolStripButton3.Text = "Load &Gdn";
		this.toolStripButton3.Click += new System.EventHandler(toolStripButton3_Click_1);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = (System.Drawing.Image)resources.GetObject("btnshortcutkeys.Image");
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(329, 28);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 509;
		this.ChkPrintSlip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintSlip.AutoSize = true;
		this.ChkPrintSlip.Checked = true;
		this.ChkPrintSlip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintSlip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintSlip.Location = new System.Drawing.Point(1030, 31);
		this.ChkPrintSlip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintSlip.Name = "ChkPrintSlip";
		this.ChkPrintSlip.Size = new System.Drawing.Size(90, 20);
		this.ChkPrintSlip.TabIndex = 37;
		this.ChkPrintSlip.Text = "Print Slip";
		this.ChkPrintSlip.UseVisualStyleBackColor = true;
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(122, 30);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(75, 22);
		this.btnRecordsUpdate.TabIndex = 43;
		this.btnRecordsUpdate.Text = "Records Update";
		this.btnRecordsUpdate.UseVisualStyleBackColor = false;
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnRecordsUpdate_Click);
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(1125, 31);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(124, 20);
		this.ChkBok.TabIndex = 18;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(3, 32);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(108, 19);
		this.label10.TabIndex = 42;
		this.label10.Text = "Sale Invoice";
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabHistory.Controls.Add(this.panel24);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Controls.Add(this.panel26);
		this.tabHistory.Controls.Add(this.panel13);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1258, 677);
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
		this.grdHistory.FrozenColumns = 4;
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 146);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1252, 295);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel24.BackColor = System.Drawing.Color.Teal;
		this.panel24.Controls.Add(this.label15);
		this.panel24.Controls.Add(this.ctrlGrdBar3);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.ForeColor = System.Drawing.Color.White;
		this.panel24.Location = new System.Drawing.Point(3, 122);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1252, 24);
		this.panel24.TabIndex = 51;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(5, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(36, 14);
		this.label15.TabIndex = 42;
		this.label15.Text = "Main";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1212, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 22);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel6.Controls.Add(this.groupBox2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 51);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1252, 71);
		this.panel6.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.label55);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1155, 66);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(3, 33);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 89;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(3, 16);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 88;
		this.label4.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1048, 37);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1048, 17);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(967, 17);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(967, 37);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(902, 33);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(664, 16);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(58, 15);
		this.label53.TabIndex = 87;
		this.label53.Text = "Customer";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(583, 16);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(64, 15);
		this.label54.TabIndex = 86;
		this.label54.Text = "To Doc No";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(502, 16);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(79, 15);
		this.label55.TabIndex = 85;
		this.label55.Text = "From Doc No";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(357, 16);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(48, 15);
		this.label56.TabIndex = 84;
		this.label56.Text = "To Date";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(213, 16);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(667, 33);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(586, 35);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(505, 35);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(360, 35);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(216, 35);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel26.BackColor = System.Drawing.Color.Teal;
		this.panel26.Controls.Add(this.label14);
		this.panel26.Controls.Add(this.ctrlGrdBar2);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel26.ForeColor = System.Drawing.Color.White;
		this.panel26.Location = new System.Drawing.Point(3, 441);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1252, 25);
		this.panel26.TabIndex = 49;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(5, 5);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(239, 14);
		this.label14.TabIndex = 42;
		this.label14.Text = "Invoice Detail Of Above Selected Row";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1213, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 466);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1252, 208);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 2;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.toolStrip2);
		this.panel13.Controls.Add(this.label13);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1252, 48);
		this.panel13.TabIndex = 27;
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1252, 25);
		this.toolStrip2.TabIndex = 22;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = (System.Drawing.Image)resources.GetObject("btnNewHistory.Image");
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = (System.Drawing.Image)resources.GetObject("btnRefreshHistory.Image");
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(1, 27);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(171, 19);
		this.label13.TabIndex = 42;
		this.label13.Text = "Sale Invoice History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1266, 706);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Name = "InvfrmSaleInvoice";
		this.Text = "Sale Invoice";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmSaleInvoice_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabPaymentDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel22.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdComm).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionDebitAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.tabHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
