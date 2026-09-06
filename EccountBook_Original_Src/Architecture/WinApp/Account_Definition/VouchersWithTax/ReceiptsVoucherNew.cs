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
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Special_Rights.Configurations;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition.VouchersWithTax;

public class ReceiptsVoucherNew : Form
{
	private bool AfterCloseup;

	private bool InventoryRelatedAccountsShowInVouchers;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool BranchFeature;

	private bool ShiftTabPressed;

	private bool MultiCurrencyFeatureVisibilty;

	private bool MultiCurrencyGridWorking;

	public bool UpdateMode;

	private bool AutoRemarksForPaymentThroughBank;

	private bool IsBookingOffice;

	private double BaseRate;

	private int BaseCurrency;

	private int[] AryColWidth;

	private int updateDetailIndex = -1;

	private int SubsdiaryAccountDatasourceLength;

	private int RecId;

	private int VoucherHeadId;

	private int ScreenId;

	public int DocumentTypeId;

	private string DocTypeIds = "3,4";

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int DefaultDiscountAccountId;

	private DataTable dtCurrencies = new DataTable();

	private DataTable dtAccount = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private DataTable dtSubsidory = new DataTable();

	private DataTable dtPaymetnType = new DataTable();

	private DataTable dtVoucherDetailMultiCurrency = new DataTable();

	private LoginNew log = new LoginNew();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private DataTable table = new DataTable();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private ToolStripButton btnNew;

	private ToolStripButton btnEdit;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnD;

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

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private TabPage CrvTab;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private new ToolStripButton Update;

	private ToolStripButton Save;

	private ToolStripButton Print;

	private ToolStripButton toolStripButton10;

	private ToolStripButton btnSmsTemplete;

	private Label PaymentVoucherLable;

	private Label label14;

	private GroupBox groupBox2;

	private Label label2;

	private Label label15;

	private DateTimePicker voucherdatetime;

	private Label label16;

	private GroupBox groupboxdetail;

	private TextBox txtremarks;

	private TextBox txtamount;

	private Label label20;

	private Label lblRemarks;

	private Label label11;

	private Button Add;

	private Panel panel8;

	private Panel panel11;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbDebitAccount;

	private TextBox txtvoucherno;

	internal UltraCombo combactitle;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label1;

	private BackgroundWorker backgroundWorker1;

	private Timer timer1;

	private TextBox txtPayTitle;

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

	private CheckBox ChkBoxWthHolding;

	internal UltraCombo CmbAgainstAc;

	private Label label13;

	private Label lblPayTitle;

	private DateTimePicker CheqDate;

	private Label lblCheqDate;

	private TextBox txtTaxPercent;

	private Label label21;

	private Label lblJoblot;

	internal UltraCombo CmbJobLot;

	internal UltraCombo combvtype;

	internal UltraCombo CmbPaymentType;

	private ToolStripButton btnDelete;

	private ToolStripButton btnRefresh;

	private TextBox CmbCheqNo;

	private Label lblbalancedetail;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo CmbTaxType;

	private Label label23;

	private Label label12;

	private ToolStripButton btnPrint102Old;

	private TabPage BrvTab;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX BankVoucherDetailGrid;

	internal GridEX BankVoucherGrid;

	internal UltraCombo CmbSubsidiaryAccount;

	private Label lblSubsidiaryaccount;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private ToolStripButton btnShortCutKey;

	private Label lblFcyAmount;

	private TextBox txtFcyAmount;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

	private TextBox txtExchangeRate;

	private Label lblExchangeRate;

	internal UltraCombo CmbProjectId;

	private Label label5;

	private ToolStripButton BtnPaymentAndReceiptVoucherSlip;

	private CheckBox ChkPrint3;

	private CheckBox ChkPrint2;

	private ToolStripButton btnSpecialRights;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewHistoryBrv;

	private ToolStripButton btnLoadAllFromBrvTab;

	private Panel panel15;

	private Label label6;

	private Panel panel17;

	private Panel panel16;

	private GroupBox groupBox3;

	private Button btnShowHistoryBrv;

	private Label label18;

	private Label label17;

	private Label label7;

	private Label label10;

	private Label label19;

	internal UltraCombo cmbApproveStatusBrv;

	private TextBox txtToDocNoBrvHistory;

	private TextBox txtFromDocNoBrvHistory;

	private DateTimePicker ToDateBrvHistory;

	private DateTimePicker FromDateBrvHistory;

	private Panel panel18;

	private Panel panel19;

	private Label label22;

	private Label label24;

	internal UltraCombo CmbAccountTitleBrvHistory;

	private ToolStripButton btnRefreshHistoryBrv;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNewCrvHistory;

	private ToolStripButton btnRefreshCrvHistory;

	private ToolStripButton btnLoadAll;

	private Panel panel13;

	private Label label25;

	private Panel panel12;

	private GroupBox groupBox1;

	private Label label26;

	internal UltraCombo cmbAccountTitleCrv;

	private Button btnShowCrv;

	private Label label28;

	private Label label29;

	private Label label30;

	private Label label31;

	private Label label32;

	internal UltraCombo cmbApprovedStatusCrv;

	private TextBox txtToDocNoCrv;

	private TextBox txtFromDocNoCrv;

	private DateTimePicker ToDateCrvHistory;

	private DateTimePicker FromDateCrvHistory;

	private Panel panel9;

	private Panel panel14;

	private Label label35;

	private Panel panel20;

	private GroupBox VouchersInfoBoxCash;

	private Label label42;

	private Label lblUnApprovedVouchers;

	private Label lblTotalVouchers;

	private Label label45;

	private Label lblApprovedVouchers;

	private Label label47;

	private GroupBox VoucherInfoBoxBank;

	private Label label36;

	private Label lblUnApprovedVouchersBpv;

	private Label lblTotalVouchersBpv;

	private Label label38;

	private Label lblApprovedVouchersBpv;

	private Label label37;

	private Panel panel6;

	private Panel panel21;

	private Panel panel22;

	private RadioButton rdcrvapproved;

	private RadioButton rdcrvmodify;

	private RadioButton rdcrvdoc;

	private RadioButton rdcrventry;

	private RadioButton rdbrvapproved;

	private RadioButton rdbrvmodify;

	private RadioButton rdbrvdoc;

	private RadioButton rdbrventry;

	internal UltraCombo cmbBranchName;

	private Label label41;

	private TextBox txtremarksmain;

	private Label labelSubsidiaryBal;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel PaymentTypePanel;

	private Panel CreditAcPanel;

	private Panel SubsidiaryAcPanel;

	private Panel JobLotPanel;

	private Panel RemarksPanel;

	private Panel AmountPanel;

	private Panel BranchNamePanel;

	private Panel CostCenterPanel;

	internal UltraCombo CmbCostCenter;

	private Label label39;

	private Panel ButtonsPanel;

	private Panel FcyAmountPanel;

	private TextBox txtFcyAmountDetail;

	private Label label43;

	private Panel ReferenceAccountPanel;

	private RadioButton rdBtnRefAcByCode;

	private RadioButton rdBtnRefAcByTitle;

	internal UltraCombo CmbReferenceAccount;

	private Label label46;

	private RadioButton rdbtnAccountCode;

	private RadioButton rdbtnAccountName;

	private TextBox txtDiscountAmount;

	private Label label50;

	private Label label51;

	private TextBox txtDiscPercent;

	internal UltraCombo CmbDiscountAccount;

	private Label label52;

	private CheckBox chkCustomAccounts;

	private Label labelLocationType;

	internal UltraCombo cmbLocationType;

	private Panel ChequeDetailPanel;

	internal GridEX grdMultiCurrency;

	private Label lblDrAcGlCurrency;

	private TextBox txtBaseFcyCode;

	private Label lblBaseFcyCode;

	private TextBox txtBaseFcyExchangeRate;

	private Label lblBaseFcyExchangeRate;

	private Panel panel26;

	internal UltraCombo cmbTcyCodeDetail;

	private Label label54;

	private Panel panel25;

	private TextBox txtTcyExchangeRateDetail;

	private Label label53;

	private ToolStripButton btnCreateUpdateMultiCurrency;

	public ReceiptsVoucherNew(UserAccount puserAccount)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void Insert()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Expected O, but got Unknown
		//IL_12f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fe: Expected O, but got Unknown
		//IL_15f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fb: Expected O, but got Unknown
		//IL_14bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c3: Expected O, but got Unknown
		//IL_175e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1765: Expected O, but got Unknown
		//IL_0cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd0: Expected O, but got Unknown
		//IL_198a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1991: Expected O, but got Unknown
		//IL_18d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18de: Expected O, but got Unknown
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
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				vh.Id = RecId;
			}
			if (ChkBoxWthHolding.Checked)
			{
				if (((UltraGridBase)CmbTaxType).ActiveRow == null || Conversion.ToInt(CmbTaxType.Value) == 0)
				{
					MessageBox.Show("TaxType Account Field is Required");
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
					MessageBox.Show("Withholding Credit Account Field is Required");
					CmbAgainstAc.Focus();
					return;
				}
				if (((UltraGridBase)CmbWithHoldingAc).ActiveRow == null || Conversion.ToInt(CmbWithHoldingAc.Value) == 0)
				{
					MessageBox.Show("Withholding Debit Account Field is Required");
					CmbWithHoldingAc.Focus();
					return;
				}
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
				if (Conversion.ToInt(CmbDiscountAccount.Value) > 0)
				{
					appliedOptions.Add("Discount");
				}
				string optionsText = string.Join(", ", appliedOptions);
				throw new Exception("There are multiple accounts in detail, so you can't apply: " + optionsText + ".\nYou can only apply these to a single account in detail.");
			}
			if (ChkBoxWthHolding.Checked && Conversion.ToInt(dtDetail.Rows[0]["AccountId"]) != Conversion.ToInt(CmbAgainstAc.Value) && MessageBox.Show("WHT A/C not match with detail Credit A/C. Are You Sure To Proceed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			vh.BaseDocumentTypeId = 2;
			vh.Type = (VoucherType)1;
			vh.DocumentTypeId = Conversion.ToInt(combvtype.Value);
			vh.ProjectId = Conversion.ToInt((object)CmbProjectId.Value.ToString());
			vh.VoucherCode = Conversion.ToInt((object)txtvoucherno.Text.Trim());
			vh.VoucherDate = voucherdatetime.Value;
			vh.Remarks = txtremarksmain.Text.Trim();
			vh.RemarksOtherLingo = "";
			vh.RefAccountId = Conversion.ToInt(CmbDebitAccount.Value);
			vh.AgainstAccountId = Conversion.ToInt(CmbWithHoldingAc.Value);
			vh.RefDocNoId = Conversion.ToInt(CmbAgainstAc.Value);
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.BranchId = UserAccount.BranchesId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.EntryUser = UserAccount.ID;
			vh.ModifyUser = UserAccount.ID;
			vh.MultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
			vh.ExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			vh.FcAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
			vh.CustomAccounts = chkCustomAccounts.Checked;
			if (ChkBoxWthHolding.Checked)
			{
				vh.IncludeWHT = true;
			}
			else
			{
				vh.IncludeWHT = false;
			}
			vh.voucherDetailList = new List<VoucherDetail>();
			if (grd.RowCount > 0)
			{
				string AcTitle = "";
				int LineId = 1;
				double VoucherAmount = 0.0;
				rows2 = grd.GetRows();
				foreach (GridEXRow r in rows2)
				{
					string RemarksAuto = "";
					VoucherDetail vd = new VoucherDetail();
					int SubsidiaryAccountId = Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
					AcTitle = Conversion.ToString((object)r.Cells["AccountId"].Text);
					vd.SubsidiaryAgainstTypeId = Conversion.ToInt(r.Cells["SubsidiaryAccountTypeId"].Value);
					if (SubsidiaryAccountId > 0)
					{
						vd.SubsidiaryAgainstAccountId = SubsidiaryAccountId;
					}
					if (vh.DocumentTypeId == 4)
					{
						AutoRemarksForPaymentThroughBank = false;
						if (AutoRemarksForPaymentThroughBank)
						{
							RemarksAuto = vh.Remarks + "  CHEQUE NO: " + vh.ChequeNo + "   RECEIVED FROM   " + AcTitle + "     DEPOSITED INTO   " + ((Control)(object)CmbDebitAccount).Text.Trim();
							vd.Comments = RemarksAuto;
							if (count == 1)
							{
								vh.Remarks = RemarksAuto;
							}
						}
						else if (vh.Remarks != null && vh.Remarks != string.Empty)
						{
							vd.Comments = vh.Remarks;
						}
						else
						{
							vd.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
						}
					}
					else if (vh.Remarks != null && vh.Remarks != string.Empty)
					{
						vd.Comments = vh.Remarks;
					}
					else
					{
						vd.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
					}
					vd.LineId = LineId;
					vd.SortNo = r.RowIndex + 1;
					vd.PaymentTypeId = Conversion.ToInt(r.Cells["PaymentType"].Value);
					vd.PaymentType = Conversion.ToString((object)r.Cells["PaymentType"].Text);
					vd.AccountId = Conversion.ToInt(CmbDebitAccount.Value);
					vd.AccountTitle = ((Control)(object)CmbDebitAccount).Text;
					vd.AgainstAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					vd.AgainstAccount = AcTitle;
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.DebitAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd.IsTaxable = "False";
					if (vh.DocumentTypeId != 3)
					{
						vd.InvoiceNoRefId = vh.CheqId;
						vd.DCheqDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
						vd.CheqNoDetail = Conversion.ToString(r.Cells["ChequeNo"].Value);
						vd.PayeeTitle = Conversion.ToString(r.Cells["PayTitle"].Value);
						if (string.IsNullOrWhiteSpace(vh.ChequeNo))
						{
							vh.ChequeDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
							vh.ChequeNo = Conversion.ToString(r.Cells["ChequeNo"].Value);
							vh.PayTitle = Conversion.ToString(r.Cells["PayTitle"].Value);
						}
					}
					vd.BranchesId = (BranchFeature ? Conversion.ToInt(r.Cells["BranchId"].Value) : UserAccount.BranchesId);
					vd.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
					vd.ReferenceAccountId = Conversion.ToInt(r.Cells["ReferenceAccountId"].Value);
					vd.DMultiCurrencyId = Conversion.ToInt(r.Cells["TcyCodeId"].Value);
					vd.DExchangeCurrencyRate = Conversion.ToDouble(r.Cells["TcyExchangeRate"].Value);
					vd.DCurrencyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
					if (MultiCurrencyGridWorking)
					{
						vd.ThirdCurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value);
						double CreditRowExchangeRate = (from x in dtVoucherDetailMultiCurrency.AsEnumerable()
							where Conversion.ToInt(x["GlCurrencyId"]) == vd.ThirdCurrencyId
							select Conversion.ToDouble(x["ExchangeRate"])).FirstOrDefault();
						if (CreditRowExchangeRate <= 0.0)
						{
							throw new Exception("Exchange rate not found or invalid for selected currency.");
						}
						vd.ThirdCurrencyFcyExchangeRate = CreditRowExchangeRate;
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
					vd.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
					VoucherAmount += vd.DebitAmount;
					if (count == 1 && vh.Remarks == string.Empty)
					{
						vh.Remarks = vd.Comments;
					}
					vh.voucherDetailList.Add(vd);
					LineId++;
					VoucherDetail vd2 = new VoucherDetail();
					vd2.SubsidiaryTypeId = Conversion.ToInt(r.Cells["SubsidiaryAccountTypeId"].Value);
					if (SubsidiaryAccountId > 0)
					{
						vd2.SupplierCustomerId = ((vd2.SubsidiaryTypeId == 1) ? SubsidiaryAccountId : 0);
						vd2.EmployeeId = ((vd2.SubsidiaryTypeId == 2) ? SubsidiaryAccountId : 0);
						vd2.SubsidiaryAccountId = SubsidiaryAccountId;
					}
					if (vh.DocumentTypeId == 4)
					{
						if (AutoRemarksForPaymentThroughBank)
						{
							vd2.Comments = RemarksAuto;
						}
						else
						{
							vd2.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
						}
					}
					else
					{
						vd2.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
					}
					vd2.LineId = LineId;
					vd2.SortNo = r.RowIndex + 1;
					vd2.PaymentTypeId = Conversion.ToInt(r.Cells["PaymentType"].Value);
					vd2.PaymentType = Conversion.ToString((object)r.Cells["PaymentType"].Text);
					vd2.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					vd2.AccountTitle = AcTitle;
					vd2.AgainstAccountId = Conversion.ToInt(CmbDebitAccount.Value);
					vd2.AgainstAccount = Conversion.ToString((object)((Control)(object)CmbDebitAccount).Text);
					vd2.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd2.CreditAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd2.IsTaxable = "False";
					if (vh.DocumentTypeId != 3)
					{
						vd2.InvoiceNoRefId = vh.CheqId;
						vd2.DCheqDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
						vd2.CheqNoDetail = Conversion.ToString(r.Cells["ChequeNo"].Value);
						vd2.PayeeTitle = Conversion.ToString(r.Cells["PayTitle"].Value);
						if (!string.IsNullOrWhiteSpace(vh.ChequeNo))
						{
							vh.ChequeDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
							vh.ChequeNo = Conversion.ToString(r.Cells["ChequeNo"].Value);
							vh.PayTitle = Conversion.ToString(r.Cells["PayTitle"].Value);
						}
					}
					vd2.DMultiCurrencyId = Conversion.ToInt(r.Cells["TcyCodeId"].Value);
					vd2.DExchangeCurrencyRate = Conversion.ToDouble(r.Cells["TcyExchangeRate"].Value);
					vd2.DCurrencyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
					vd2.BranchesId = (BranchFeature ? Conversion.ToInt(r.Cells["BranchId"].Value) : UserAccount.BranchesId);
					vd2.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
					vd2.ReferenceAccountId = Conversion.ToInt(r.Cells["ReferenceAccountId"].Value);
					vd2.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
					if (MultiCurrencyGridWorking)
					{
						vd2.ThirdCurrencyId = Conversion.ToInt(r.Cells["GlCurrencyId"].Value);
						if (vd2.ThirdCurrencyId == 0)
						{
							throw new Exception("Currency Not set against selected account");
						}
						double exchangeRate = 0.0;
						exchangeRate = ((vd2.DMultiCurrencyId != vd2.ThirdCurrencyId) ? (from x in dtVoucherDetailMultiCurrency.AsEnumerable()
							where Conversion.ToInt(x["GlCurrencyId"]) == vd2.ThirdCurrencyId
							select Conversion.ToDouble(x["ExchangeRate"])).FirstOrDefault() : vd2.DExchangeCurrencyRate);
						if (exchangeRate <= 0.0)
						{
							throw new Exception("Exchange rate not found or invalid for selected currency.");
						}
						vd2.ThirdCurrencyFcyExchangeRate = exchangeRate;
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
					VoucherDetail vd4 = new VoucherDetail();
					vd4.LineId = LineId + 1;
					vd4.AccountId = vh.AgainstAccountId;
					vd4.AgainstAccountId = vh.RefDocNoId;
					vd4.TaxTypeId = Conversion.ToInt(CmbTaxType.Value);
					vd4.IsTaxable = "True";
					double grossAmount = Conversion.ToDouble((object)txtValue.Text.Trim()) + Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
					double netAmount = Conversion.ToDouble((object)txtValue.Text.Trim());
					vd4.Comments = "Received after WHT deduction: Gross " + grossAmount.ToString("#,#") + ", WHT " + Math.Round(Conversion.ToDouble((object)txtTaxAmount.Text), 2) + ", Net " + netAmount + " @ " + txtTaxPercent.Text.Trim().ToString() + "%";
					vd4.TaxPrcnt = Conversion.ToDouble((object)txtTaxPercent.Text.Trim());
					vd4.TaxesTotalAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
					vd4.DebitAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
					double FcyAmount2 = ((vh.ExchangeCurrencyRate > 0.0) ? (vd4.DebitAmount / vh.ExchangeCurrencyRate) : 0.0);
					FcyAmount2 = (vd4.DCurrencyAmount = Math.Round(FcyAmount2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount));
					vh.voucherDetailList.Add(vd4);
					VoucherDetail vd6 = new VoucherDetail();
					vd6.LineId = LineId + 1;
					vd6.AccountId = vh.RefDocNoId;
					vd6.AgainstAccountId = vh.AgainstAccountId;
					vd6.TaxTypeId = Conversion.ToInt(CmbTaxType.Value);
					vd6.IsTaxable = "True";
					vd6.Comments = "Tax Name    " + ((Control)(object)CmbTaxType).Text + "   Tax %   " + txtTaxPercent.Text.Trim();
					vd6.TaxPrcnt = Conversion.ToDouble((object)txtTaxPercent.Text.Trim());
					vd6.TaxesTotalAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
					vd6.CreditAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
					vd6.DCurrencyAmount = FcyAmount2;
					vd6.Comments = vd4.Comments;
					vh.voucherDetailList.Add(vd6);
					VoucherAmount += vd4.DebitAmount;
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
					double FcyAmount = ((vh.ExchangeCurrencyRate > 0.0) ? (vd3.DebitAmount / vh.ExchangeCurrencyRate) : 0.0);
					FcyAmount = Math.Round(FcyAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount);
					double originalAmount = Conversion.ToDouble((object)txtValue.Text) + Conversion.ToDouble((object)txtDiscountAmount.Text);
					vd3.DCurrencyAmount = FcyAmount;
					vd3.Comments = "Original receivable amount was " + originalAmount + ". Discount of " + txtDiscountAmount.Text.Trim() + " given upon mutual agreement. Final amount received: " + txtValue.Text.Trim() + ".";
					vh.voucherDetailList.Add(vd3);
					VoucherDetail vd5 = new VoucherDetail();
					vd5.LineId = LineId + 1;
					vd5.AccountId = Conversion.ToInt(CmbDiscountAccount.Value);
					vd5.AgainstAccountId = vh.voucherDetailList[0].AccountId;
					vd5.DiscountPercent = Conversion.ToDouble((object)txtDiscPercent.Text.Trim());
					vd5.DiscountAmount = Conversion.ToDouble((object)txtDiscountAmount.Text.Trim());
					vd5.CreditAmount = Conversion.ToDouble((object)txtDiscountAmount.Text.Trim());
					vd5.DCurrencyAmount = FcyAmount;
					vd5.Comments = vd3.Comments;
					vh.voucherDetailList.Add(vd5);
					VoucherAmount += vd3.DebitAmount;
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
				vh.VoucherAmount = VoucherAmount;
				vh.AttachmentsList = new List<DMSAttachments>();
				vh.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<VoucherHead>(vh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				if (string.IsNullOrEmpty(AttachmentsValues) || AT.lst.Count > 0)
				{
					AttachmentsValues = "";
				}
				vh.AttachmentsValues = AttachmentsValues;
				vh.CustomAttachmentsValues = CustomAttachmentsValues;
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
				grd.ClearStructure();
				if (ChkPrint1.Checked)
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(success);
				}
				else if (ChkPrint2.Checked)
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(success, vh.DocumentTypeId);
				}
				else if (ChkPrint3.Checked)
				{
					CommonServices.PaymentAndReceiptVoucherSlip(success, vh.DocumentTypeId);
				}
			}
			else
			{
				MessageBox.Show("please enter record in grid");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Add_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbPaymentType).ActiveRow == null)
			{
				MessageBox.Show("PaymentType Field Required");
				CmbPaymentType.Focus();
				return;
			}
			if (Conversion.ToInt(combactitle.Value) == 0)
			{
				MessageBox.Show("Credit Account Field Required");
				combactitle.Focus();
				return;
			}
			if (MultiCurrencyGridWorking && Conversion.ToInt(combactitle.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)combactitle).SelectedRow.Cells[5].Value) == 0)
			{
				MessageBox.Show("Credit Account Currency not set Please Check");
				combactitle.Focus();
				return;
			}
			if (((UltraGridBase)CmbJobLot).ActiveRow == null)
			{
				MessageBox.Show("Job/Lot Field Required");
				CmbJobLot.Focus();
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
			if (txtamount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtamount.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Amount Field Required");
				txtamount.Focus();
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
			if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountDatasourceLength > 0 && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow == null)
			{
				MessageBox.Show("Subsidiary Account Title Field Is Required");
				CmbSubsidiaryAccount.Focus();
				return;
			}
			if (BranchFeature && (((UltraGridBase)cmbBranchName).ActiveRow == null || Conversion.ToInt(cmbBranchName.Value) == 0))
			{
				MessageBox.Show("BranchName Field is Required");
				cmbBranchName.Focus();
				return;
			}
			table.Rows.Add(Conversion.ToInt(CmbPaymentType.Value), ((UltraDropDownBase)combactitle).SelectedRow.Cells[2].Value, combactitle.Value, MultiCurrencyGridWorking ? Conversion.ToInt(((UltraDropDownBase)combactitle).SelectedRow.Cells[5].Value) : 0, MultiCurrencyGridWorking ? Conversion.ToString(((UltraDropDownBase)combactitle).SelectedRow.Cells[6].Value) : "", (!SubsidiaryAccountAllownOnVouchers) ? ((object)0) : ((SubsdiaryAccountDatasourceLength > 0) ? CmbSubsidiaryAccount.Value : combactitle.Value), (!SubsidiaryAccountAllownOnVouchers) ? "" : ((SubsdiaryAccountDatasourceLength > 0) ? ((Control)(object)CmbSubsidiaryAccount).Text : ((Control)(object)combactitle).Text), (!((Control)(object)CmbSubsidiaryAccount).Visible) ? ((object)0) : ((SubsdiaryAccountDatasourceLength > 0) ? ((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[2].Value : ((object)4)), CmbJobLot.Value, txtremarks.Text, cmbTcyCodeDetail.Value, ((Control)(object)cmbTcyCodeDetail).Text, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), txtFcyAmountDetail.Text, txtamount.Text, CmbReferenceAccount.Value, (Conversion.ToInt(CmbReferenceAccount.Value) > 0) ? Conversion.ToString(((UltraDropDownBase)CmbReferenceAccount).SelectedRow.Cells[1].Value) : "", (Conversion.ToInt(cmbBranchName.Value) > 0) ? Conversion.ToInt(cmbBranchName.Value) : 0, Conversion.ToString((object)((Control)(object)cmbBranchName).Text), (Conversion.ToInt(CmbCostCenter.Value) > 0) ? Conversion.ToInt(CmbCostCenter.Value) : 0, Conversion.ToDateTime((object)CheqDate.Value), Conversion.ToString((object)CmbCheqNo.Text), Conversion.ToString((object)txtPayTitle.Text));
			lblbalancedetail.Visible = false;
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			combactitle.Focus();
			Total();
			txtamount.Clear();
			txtFcyAmountDetail.Text = string.Empty;
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
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private bool FormValidation()
	{
		if (Conversion.ToInt(CmbProjectId.Value) == 0)
		{
			MessageBox.Show("Cost Center Field is Required");
			CmbProjectId.Focus();
			return false;
		}
		if (((UltraGridBase)combvtype).ActiveRow == null)
		{
			MessageBox.Show("Voucher Type Field is Required");
			combvtype.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLocationType).ActiveRow == null || Conversion.ToInt(cmbLocationType.Value) == 0)
		{
			MessageBox.Show("Location Type Field is Required");
			cmbLocationType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDebitAccount).ActiveRow == null || Conversion.ToInt(CmbDebitAccount.Value) == 0)
		{
			MessageBox.Show("Debit Account Field is Required");
			CmbDebitAccount.Focus();
			return false;
		}
		if (MultiCurrencyGridWorking && Conversion.ToInt(CmbDebitAccount.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value) == 0)
		{
			MessageBox.Show("Debit Account Currency not set Please Check");
			CmbDebitAccount.Focus();
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

	private void AcfrmPaymentVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			int StandardHeight = 625;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, StandardHeight);
			string ScreenName = Conversion.ToString(base.Tag);
			if (ScreenName == "frmCashReceiptVoucherTax")
			{
				DocumentTypeId = 3;
			}
			if (ScreenName == "frmBankReceiptVoucherTax")
			{
				DocumentTypeId = 4;
			}
			ScreenId = CommonServices.GetScreenIdByName(base.Name);
			MultiCurrencyFeature();
			if (DocumentTypeId == 3)
			{
				ChequeDetailPanel.Visible = false;
				DocTypeIds = "3";
				PaymentVoucherLable.Text = "Cash Receipt Voucher";
				ScreenName = "frmCashReceiptVoucherTax";
			}
			if (DocumentTypeId == 4)
			{
				PaymentVoucherLable.Text = "Bank Receipt Voucher";
				DocTypeIds = "4";
				ScreenName = "frmBankReceiptVoucherTax";
			}
			if (DocumentTypeId == 0)
			{
				ScreenName = base.Name;
			}
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			Save.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
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
			ctrlGrdBar4.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			AutoRemarksForPaymentThroughBank = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AutoRemarksForPaymentThroughBank"));
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			BranchFeature = CommonServices.GetERPFeatureById(17);
			if (table.Columns.Count == 0)
			{
				table.Columns.Add("PaymentType");
				table.Columns.Add("AccountCode");
				table.Columns.Add("AccountId", typeof(int));
				table.Columns.Add("GlCurrencyId", typeof(int));
				table.Columns.Add("GlCurrency");
				table.Columns.Add("SubsidiaryAccountId", typeof(int));
				table.Columns.Add("SubsidiaryAccount");
				table.Columns.Add("SubsidiaryAccountTypeId", typeof(int));
				table.Columns.Add("JobLotId", typeof(int));
				table.Columns.Add("Remarks");
				table.Columns.Add("TcyCodeId", typeof(int));
				table.Columns.Add("TcyCode");
				table.Columns.Add("TcyExchangeRate", typeof(double));
				table.Columns.Add("FcyAmount", typeof(double));
				table.Columns.Add("Amount", typeof(double));
				table.Columns.Add("ReferenceAccountId", typeof(int));
				table.Columns.Add("ReferenceAccount");
				table.Columns.Add("BranchId", typeof(int));
				table.Columns.Add("BranchName");
				table.Columns.Add("CostCenterId", typeof(int));
				table.Columns.Add("ChequeDate", typeof(DateTime));
				table.Columns.Add("ChequeNo", typeof(string));
				table.Columns.Add("PayTitle", typeof(string));
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
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
			dtPaymetnType.Columns.Add("Id");
			dtPaymetnType.Columns.Add("PaymentType");
			if (!MultiCurrencyGridWorking)
			{
				btnCreateUpdateMultiCurrency.Visible = false;
				((Control)(object)grdMultiCurrency).Visible = false;
				lblDrAcGlCurrency.Visible = false;
				lblBaseFcyCode.Visible = false;
				txtBaseFcyCode.Visible = false;
				lblBaseFcyExchangeRate.Visible = false;
				txtBaseFcyExchangeRate.Visible = false;
				panel21.Height = 105;
			}
			else
			{
				panel21.Height = 160;
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
			BranchesFill();
			CurrencyFill();
			PaymentTypeFill();
			CombProjectFill();
			VoucherNofill();
			combojoblotfill();
			AccountsComboBind();
			BindTaxTypes();
			LocationType();
			DetailAccountFill();
			BindReferenceAccounts();
			BindDiscountAccounts();
			lblbalancedetail.Visible = false;
			Save.Visible = true;
			Update.Visible = false;
			cmbdoctype.SelectedIndex = 0;
			TypeFill();
			((UltraGridBase)combvtype).Rows[0].Activate();
			combvtype.Focus();
			((UltraGridBase)CmbPaymentType).Rows[0].Activate();
			ChkPrint1.Checked = true;
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			if (DocTypeIds == "3")
			{
				tabControl1.TabPages.Remove(BrvTab);
			}
			else if (DocTypeIds == "4")
			{
				tabControl1.TabPages.Remove(CrvTab);
			}
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				GetSubsidoryAccounts();
				BindSubsidiaryAccount();
				SubsidiaryAcPanel.Visible = true;
				labelSubsidiaryBal.Visible = true;
				lblSubsidiaryaccount.Visible = true;
				((Control)(object)CmbSubsidiaryAccount).Visible = true;
			}
			else
			{
				SubsidiaryAcPanel.Visible = false;
				labelSubsidiaryBal.Visible = false;
				lblSubsidiaryaccount.Visible = false;
				((Control)(object)CmbSubsidiaryAccount).Visible = false;
				RemarksPanel.Size = new Size(451, 47);
				txtremarks.Size = new Size(451, 23);
			}
			if (BranchFeature)
			{
				BranchNamePanel.Visible = true;
			}
			else
			{
				BranchNamePanel.Visible = false;
			}
			ApplicationsAllocateToCompany obj = clsGlobalVariables.GetApplicationsAllocateToCompanyList.Find((ApplicationsAllocateToCompany row) => row.AppId == 5 && row.CompanyId == UserAccount.CompanyId);
			IsBookingOffice = obj != null && obj.IsActive;
			if (!IsBookingOffice)
			{
				CostCenterPanel.Visible = false;
			}
			label41.Visible = BranchFeature;
			((Control)(object)cmbBranchName).Visible = BranchFeature;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			DefaultConfigurations();
			SpecialRightsImplement();
			CostCenterFill();
			StatusFillForCrv();
			ComboBindForCrvHistory();
			FromDateCrvHistory.Value = DateTime.Now;
			ToDateCrvHistory.Value = DateTime.Now;
			StatusFillForBrv();
			ComboBindForBrvHistory();
			FromDateBrvHistory.Value = DateTime.Now;
			ToDateBrvHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void ComboBindForCrvHistory()
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
				DocumentTypeIds = 3.ToString()
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
			DDL.BindDDL(dtAccount, cmbAccountTitleCrv, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFillForCrv()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Not Apporved");
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApprovedStatusCrv, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApprovedStatusCrv).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindForBrvHistory()
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
				DocumentTypeIds = 4.ToString()
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
			DDL.BindDDL(dtAccount, CmbAccountTitleBrvHistory, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFillForBrv()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Not Apporved");
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, cmbApproveStatusBrv, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApproveStatusBrv).Rows[0].Activate();
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
			List<ConfigrationsAllocation> dfDiscAcId = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "DefaultDiscountAccount").ToList();
			if (dfDiscAcId.Count > 0)
			{
				DefaultDiscountAccountId = Conversion.ToInt((object)dfDiscAcId[0].ConfigKey);
				if (DefaultDiscountAccountId > 0)
				{
					CmbDiscountAccount.Value = DefaultDiscountAccountId;
				}
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
				DDL.BindDDL(dtJobLot, CmbJobLot, "Id", "JobLotDescription", "Job Lot", ZeroIndex: false);
				((UltraGridBase)CmbJobLot).DisplayLayout.Bands[0].Columns[1].Width = 250;
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			if (DocumentTypeId == 3)
			{
				dt.Rows.Add(3, "Cash Receipt Voucher");
			}
			if (DocumentTypeId == 4)
			{
				dt.Rows.Add(4, "Bank Receipt Voucher");
			}
			if (DocumentTypeId == 0)
			{
				dt.Rows.Add(3, "Cash Receipt Voucher");
				dt.Rows.Add(4, "Bank Receipt Voucher");
			}
			DDL.BindDDL(dt, combvtype, "Id", "Name", "Voucher Type", ZeroIndex: false);
			((UltraGridBase)combvtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)combvtype).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PaymentTypeFill()
	{
		try
		{
			dtPaymetnType = lookUp.GetDataPaymentTypeForReceipts();
			if (dtPaymetnType.Rows.Count > 0)
			{
				DDL.BindDDL(dtPaymetnType, CmbPaymentType, "Id", "PaymentType", "PaymentType", ZeroIndex: false);
				((UltraGridBase)CmbPaymentType).DisplayLayout.Bands[0].Columns[1].Width = 100;
				((UltraGridBase)CmbPaymentType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AccountTitleFill()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(CmbDebitAccount.Value);
			DataTable dtCoa = COAAllocation.GetDetailAccountByDocumentTypeId(new COAAllocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = Conversion.ToInt(combvtype.Value)
			});
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
				DDL.BindDDL(dt, CmbDebitAccount, "Id", "AccountTitle", "Debit Account", ZeroIndex: true);
				((UltraGridBase)CmbDebitAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbDebitAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				CommonServices.SetComboValue(CmbDebitAccount, "Id", id, dt);
				if (Conversion.ToInt(combvtype.Value) == 3 && Save.Visible && Save.Enabled)
				{
					((UltraGridBase)CmbDebitAccount).Rows[1].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void VoucherNofill()
	{
		try
		{
			txtvoucherno.Text = Conversion.ToString((object)CommonServices.GenerateVoucherCode(Convert.ToInt32(combvtype.Value)));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = voucherdatetime.Value,
				RefAccountId = Conversion.ToInt(CmbDebitAccount.Value)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailAccountFill()
	{
		try
		{
			int accountId = Conversion.ToInt(combactitle.Value);
			HashSet<int> excludedTypeSet = new HashSet<int>((!InventoryRelatedAccountsShowInVouchers) ? new int[10] { 2, 4, 11, 12, 13, 14, 15, 19, 20, 21 } : new int[2] { 2, 15 });
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
			string headerText = (rdbtnAccountName.Checked ? "Credit Account" : "Account Code");
			InfragisticsHelper.BindAndRetainSelection(combactitle, dtAccount, "Id", displayField, headerText, AllColumns: true, accountId, insertDefaultRow: false, null, null, ActivateRow: false);
			if (accountId > 0)
			{
				combactitle_Leave(null, null);
			}
			if (dtAccount.Rows.Count > 0)
			{
				((UltraGridBase)combactitle).DisplayLayout.Bands[0].Columns[5].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = voucherdatetime.Value,
				RefAccountId = Conversion.ToInt(combactitle.Value)
			});
			if (detail.Rows.Count > 0)
			{
				double ClosingBalance = Conversion.ToDouble(detail.Rows[0]["Balance"]);
				lblbalancedetail.Text = ClosingBalance.ToString("#,#;(#,#);0");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void combactitle_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(combactitle.Value) > 0)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				if (((UltraGridBase)combactitle).ActiveRow != null && ((Control)(object)combactitle).Text != string.Empty)
				{
					int selectedAcId = Conversion.ToInt(combactitle.Value);
					if (selectedAcId != 0)
					{
						DataTable matchingTable = dtSubsidory.Clone();
						int SubsidiaryIdInCaseOfUpdateDetail = 0;
						if (((Control)(object)CmbSubsidiaryAccount).Text != string.Empty)
						{
							SubsidiaryIdInCaseOfUpdateDetail = Conversion.ToInt(CmbSubsidiaryAccount.Value);
						}
						foreach (DataRow row in dtSubsidory.Rows)
						{
							if (Conversion.ToInt(row["AccountId"]) == selectedAcId)
							{
								matchingTable.ImportRow(row);
							}
						}
						if (matchingTable.Rows.Count > 0)
						{
							SubsdiaryAccountDatasourceLength = matchingTable.Rows.Count;
							DDL.BindDDL(matchingTable, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account", ZeroIndex: false);
							((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
							((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
							CommonServices.SetComboValue(CmbSubsidiaryAccount, "Id", SubsidiaryIdInCaseOfUpdateDetail, matchingTable);
						}
						else
						{
							((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
							((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
							SubsdiaryAccountDatasourceLength = 0;
						}
					}
					else
					{
						((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
						((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
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
			bool isNotType3 = Conversion.ToInt(combvtype.Value) != 3;
			ChequeDetailPanel.Visible = isNotType3;
			if (grd.RowCount > 0 && table.Rows.Count > 0)
			{
				grd.RootTable.Columns["ChequeDate"].Visible = isNotType3;
				grd.RootTable.Columns["ChequeNo"].Visible = isNotType3;
				grd.RootTable.Columns["PayTitle"].Visible = isNotType3;
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
			if (Conversion.ToInt(CmbDebitAccount.Value) > 0)
			{
				txtbalance.Visible = true;
				AccountCurrentBalance();
				DebitAccountCurrencyCheck();
			}
			else
			{
				txtbalance.Visible = false;
				txtbalance.Text = "0";
			}
			CreateUpdateMultiCurrencyGridInfo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void DebitAccountCurrencyCheck()
	{
		try
		{
			if (!MultiCurrencyGridWorking)
			{
				return;
			}
			int CurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value);
			string CurrencyCode = Conversion.ToString(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[3].Value);
			lblDrAcGlCurrency.Text = ((CurrencyId > 0) ? ("Debit Ac GLcy:\r\n" + CurrencyCode) : "Debit Ac GLcy:");
			if (CurrencyId == 0)
			{
				if (RecId == 0)
				{
					((Control)(object)CmbDebitAccount).Text = "";
					CmbDebitAccount.Focus();
				}
				throw new Exception("Debit Account Currency Not set. Please Check!");
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
			if (MultiCurrencyGridWorking && Conversion.ToInt(((UltraDropDownBase)combactitle).SelectedRow.Cells[5].Value) == 0 && updateDetailIndex == -1)
			{
				((Control)(object)combactitle).Text = "";
				combactitle.Focus();
				throw new Exception("Detail Account Currency Not set. Please Check!");
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
			grd.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			grd.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			grd.RootTable.Columns["SubsidiaryAccountTypeId"].Visible = false;
			grd.RootTable.Columns["ReferenceAccountId"].Visible = false;
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["GlCurrencyId"].Visible = false;
			grd.RootTable.Columns["TcyCodeId"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grd.RootTable.Columns["CostCenterId"].Visible = IsBookingOffice;
			grd.RootTable.Columns["GlCurrency"].Visible = MultiCurrencyGridWorking;
			bool isNotType3 = Conversion.ToInt(combvtype.Value) != 3;
			grd.RootTable.Columns["ChequeDate"].Visible = isNotType3;
			grd.RootTable.Columns["ChequeNo"].Visible = isNotType3;
			grd.RootTable.Columns["PayTitle"].Visible = isNotType3;
			grd.RootTable.Columns["BranchId"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchName"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccount"].EditType = (EditType)0;
			grd.RootTable.Columns["SubsidiaryAccountTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferenceAccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferenceAccount"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyCodeId"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyCode"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyExchangeRate"].EditType = (EditType)0;
			grd.RootTable.Columns["GlCurrencyId"].EditType = (EditType)0;
			grd.RootTable.Columns["GlCurrency"].EditType = (EditType)0;
			grd.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			CommonServices.GridWrappingAndColumnSettings(grd, 2);
			grd.RootTable.Columns["AccountId"].Width = 250;
			grd.RootTable.Columns["AccountCode"].Width = 90;
			grd.RootTable.Columns["SubsidiaryAccount"].Width = 190;
			grd.RootTable.Columns["Remarks"].Width = 293;
			grd.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 15;
			grd.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grd.RootTable.Columns["TcyExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 20;
			grd.RootTable.Columns["Amount"].Width = 115;
			grd.RootTable.Columns["FcyAmount"].Width = 115;
			grd.RootTable.Columns["FcyAmount"].Caption = "Tcy Amount";
			grd.RootTable.Columns["JobLotId"].Width = 150;
			grd.RootTable.Columns["ReferenceAccount"].Width = 150;
			grd.RootTable.Columns["CostCenterId"].Width = 130;
			grd.RootTable.Columns["BranchName"].Width = 130;
			grd.RootTable.Columns["ChequeDate"].Width = 90;
			grd.RootTable.Columns["ChequeNo"].Width = 110;
			grd.RootTable.Columns["PayTitle"].Width = 130;
			grd.RootTable.Columns["Amount"].Caption = "Credit Amount";
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			grd.RootTable.Columns["PaymentType"].Caption = "PaymentType";
			grd.RootTable.Columns["PaymentType"].ValueList.PopulateValueList((IEnumerable)dtPaymetnType.DefaultView, "Id", "PaymentType");
			grd.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["AccountId"].HasValueList = true;
			grd.RootTable.Columns["AccountId"].LimitToList = true;
			grd.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].Caption = "Account Title";
			grd.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccount.DefaultView, "Id", "AccountTitle");
			grd.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobLotId"].HasValueList = true;
			grd.RootTable.Columns["JobLotId"].LimitToList = true;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grd.RootTable.Columns["JobLotId"].Caption = "JobLot";
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.ClearStructure();
			new VoucherDetail();
			RecId = Conversion.ToInt((object)ID);
			VoucherHead vh = VoucherHead.GetByID(RecId);
			if (vh != null)
			{
				Save.Visible = false;
				Update.Visible = true;
				UpdateMode = true;
				tabControl1.SelectedIndex = 0;
				if (vh.DocumentTypeId == 3)
				{
					((Control)(object)combvtype).Text = "Cash Receipt Voucher";
				}
				else
				{
					((Control)(object)combvtype).Text = "Bank Receipt Voucher";
				}
				combvtype_Leave(null, null);
				voucherdatetime.Value = vh.VoucherDate;
				combcreditac_Leave(null, null);
				CmbDebitAccount.Value = vh.RefAccountId;
				CmbProjectId.Value = vh.ProjectId;
				txtremarksmain.Text = Conversion.ToString((object)vh.Remarks);
				txtvoucherno.Text = Conversion.ToString((object)vh.VoucherCode);
				txtValue.Text = Conversion.ToString((object)vh.VoucherAmount);
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
				table.Clear();
				for (int i = 0; i < vh.voucherDetailList.Count; i++)
				{
					cmbLocationType.Value = ((vh.voucherDetailList.Count > 0) ? Conversion.ToInt((object)vh.voucherDetailList[0].LocationTypeId) : 0);
					if (vh.voucherDetailList[i].IsTaxable == "False" && vh.voucherDetailList[i].AccountId != Convert.ToInt32(CmbDebitAccount.Value))
					{
						table.Rows.Add(vh.voucherDetailList[i].PaymentTypeId, vh.voucherDetailList[i].AccountCode, vh.voucherDetailList[i].AccountId, vh.voucherDetailList[i].ThirdCurrencyId, vh.voucherDetailList[i].GlcyCode, (vh.voucherDetailList[i].SubsidiaryTypeId == 1) ? vh.voucherDetailList[i].SupplierCustomerId : ((vh.voucherDetailList[i].SubsidiaryTypeId == 2) ? vh.voucherDetailList[i].EmployeeId : ((vh.voucherDetailList[i].SubsidiaryTypeId == 3) ? vh.voucherDetailList[i].SubsidiaryAccountId : ((vh.voucherDetailList[i].SubsidiaryTypeId == 4) ? vh.voucherDetailList[i].SubsidiaryAccountId : 0))), vh.voucherDetailList[i].SubsidiaryAccountTitle, vh.voucherDetailList[i].SubsidiaryTypeId, vh.voucherDetailList[i].JobLotId, vh.voucherDetailList[i].Comments, vh.voucherDetailList[i].DMultiCurrencyId, vh.voucherDetailList[i].TcyCode, vh.voucherDetailList[i].DExchangeCurrencyRate, vh.voucherDetailList[i].DCurrencyAmount, vh.voucherDetailList[i].CreditAmount, vh.voucherDetailList[i].ReferenceAccountId, vh.voucherDetailList[i].ReferenceAccount, vh.voucherDetailList[i].BranchesId, vh.voucherDetailList[i].BranchName, vh.voucherDetailList[i].CostCenterId, vh.voucherDetailList[i].DCheqDate, vh.voucherDetailList[i].CheqNoDetail, vh.voucherDetailList[i].PayeeTitle);
					}
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				Total();
				if (MultiCurrencyGridWorking && vh.voucherDetailList != null && vh.voucherDetailList.Count > 0)
				{
					txtBaseFcyCode.Tag = vh.voucherDetailList[0].BaseFcyId;
					txtBaseFcyCode.Text = Conversion.ToString((object)vh.voucherDetailList[0].BaseFcyCode);
					txtBaseFcyExchangeRate.Text = Conversion.ToString((object)vh.voucherDetailList[0].BaseFcyExchangeRate);
				}
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
				voucherdatetime.Focus();
			}
			else
			{
				Reset();
			}
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
			SubsdiaryAccountDatasourceLength = 0;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			RecId = 0;
			updateDetailIndex = -1;
			VoucherHeadId = 0;
			table.Rows.Clear();
			UpdateMode = false;
			txtFcyAmount.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtamount.Text = string.Empty;
			txtPayTitle.Text = string.Empty;
			txtremarksmain.Text = string.Empty;
			((Control)(object)CmbAgainstAc).Text = string.Empty;
			((Control)(object)CmbTaxType).Text = string.Empty;
			txtDiscountAmount.Text = string.Empty;
			txtDiscPercent.Text = string.Empty;
			txtValue.Clear();
			txtTaxAmount.Clear();
			txtTotalAmount.Clear();
			((UltraGridBase)CmbPaymentType).Rows[0].Activate();
			grd.DataSource = table;
			((Control)(object)combactitle).Text = string.Empty;
			lblbalancedetail.Visible = false;
			lblbalancedetail.Text = string.Empty;
			CmbCheqNo.Text = string.Empty;
			Save.Visible = true;
			CmbDebitAccount.Focus();
			VoucherNofill();
			UpdateMode = false;
			Save.Visible = true;
			Update.Visible = false;
			Total();
			lblDrAcGlCurrency.Text = "Debit Ac GLcy:";
			dtVoucherDetailMultiCurrency.Rows.Clear();
			grdMultiCurrency.DataSource = dtVoucherDetailMultiCurrency;
			cmbCurrency_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
		txtbalance.Visible = false;
	}

	private void Total()
	{
		try
		{
			double taxAmount = 0.0;
			double TotalAmt = 0.0;
			double Amount = 0.0;
			double DiscountAmount = 0.0;
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					Amount += Conversion.ToDouble(r.Cells["Amount"].Value);
				}
			}
			if (Amount > 0.0)
			{
				txtValue.Text = Amount.ToString(clsGlobalVariables.stringFormatsingle);
				taxAmount = Conversion.ToDouble((object)txtTaxAmount.Text);
				DiscountAmount = Conversion.ToDouble((object)txtDiscountAmount.Text);
				TotalAmt = Amount + taxAmount + DiscountAmount;
				txtTotalAmount.Text = TotalAmt.ToString(clsGlobalVariables.stringFormatsingle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (updateDetailIndex >= 0)
				{
					throw new Exception("Reset the detail first to delete");
				}
				item.Delete();
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
			MessageBox.Show(ex.Message);
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

	private void HistoryFillCrv(int NoofRecords = 0)
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
			vh.Ids = "3";
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (!vh.PostState)
			{
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (rdcrvdoc.Checked)
			{
				if (FromDateCrvHistory.Checked)
				{
					vh.FromDate = FromDateCrvHistory.Value;
				}
				if (ToDateCrvHistory.Checked)
				{
					vh.ToDate = ToDateCrvHistory.Value;
				}
			}
			else if (rdcrventry.Checked)
			{
				if (FromDateCrvHistory.Checked)
				{
					vh.EntryDateFrom = FromDateCrvHistory.Value;
				}
				if (ToDateCrvHistory.Checked)
				{
					vh.EntryDateTo = ToDateCrvHistory.Value;
				}
			}
			else if (rdcrvmodify.Checked)
			{
				if (FromDateCrvHistory.Checked)
				{
					vh.ModifyDateFrom = FromDateCrvHistory.Value;
				}
				if (ToDateCrvHistory.Checked)
				{
					vh.ModifyDateTo = ToDateCrvHistory.Value;
				}
			}
			else if (rdcrvapproved.Checked)
			{
				if (FromDateCrvHistory.Checked)
				{
					vh.ApprovedDateFrom = FromDateCrvHistory.Value;
				}
				if (ToDateCrvHistory.Checked)
				{
					vh.ApprovedDateTo = ToDateCrvHistory.Value;
				}
			}
			vh.FromDocNo = Conversion.ToInt((object)txtFromDocNoCrv.Text);
			vh.ToDocNo = Conversion.ToInt((object)txtToDocNoCrv.Text);
			vh.AccountId = Conversion.ToInt(cmbAccountTitleCrv.Value);
			if (((Control)(object)cmbApprovedStatusCrv).Text == "Approved")
			{
				vh.IsApproved = true;
			}
			if (((Control)(object)cmbApprovedStatusCrv).Text == "Not Apporved")
			{
				vh.IsApproved = false;
			}
			if (((Control)(object)cmbApprovedStatusCrv).Text == "All")
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
					dtCashVoucher.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeCode"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["Remarks"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["NoOfAttachments"]);
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

	private void HistoryFillBrv(int NoofRecords = 0)
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
			vh.Ids = "4";
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (rdbrvdoc.Checked)
			{
				if (FromDateBrvHistory.Checked)
				{
					vh.FromDate = FromDateBrvHistory.Value;
				}
				if (ToDateBrvHistory.Checked)
				{
					vh.ToDate = ToDateBrvHistory.Value;
				}
			}
			else if (rdbrventry.Checked)
			{
				if (FromDateBrvHistory.Checked)
				{
					vh.EntryDateFrom = FromDateBrvHistory.Value;
				}
				if (ToDateBrvHistory.Checked)
				{
					vh.EntryDateTo = ToDateBrvHistory.Value;
				}
			}
			else if (rdbrvmodify.Checked)
			{
				if (FromDateBrvHistory.Checked)
				{
					vh.ModifyDateFrom = FromDateBrvHistory.Value;
				}
				if (ToDateBrvHistory.Checked)
				{
					vh.ModifyDateTo = ToDateBrvHistory.Value;
				}
			}
			else if (rdbrvapproved.Checked)
			{
				if (FromDateBrvHistory.Checked)
				{
					vh.ApprovedDateFrom = FromDateBrvHistory.Value;
				}
				if (ToDateBrvHistory.Checked)
				{
					vh.ApprovedDateTo = ToDateBrvHistory.Value;
				}
			}
			vh.FromDocNo = Conversion.ToInt((object)txtFromDocNoBrvHistory.Text);
			vh.ToDocNo = Conversion.ToInt((object)txtToDocNoBrvHistory.Text);
			vh.AccountId = Conversion.ToInt(CmbAccountTitleBrvHistory.Value);
			if (((Control)(object)cmbApproveStatusBrv).Text == "Approved")
			{
				vh.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatusBrv).Text == "Not Apporved")
			{
				vh.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatusBrv).Text == "All")
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
					dtBankVoucher.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeCode"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["Remarks"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["NoOfAttachments"]);
				}
				VoucherInfoBoxBank.Visible = true;
				lblTotalVouchersBpv.Text = Conversion.ToInt(lst.Rows[0]["TotalVouchers"]).ToString();
				lblApprovedVouchersBpv.Text = Conversion.ToInt(lst.Rows[0]["TotalApprovedVoucher"]).ToString();
				lblUnApprovedVouchersBpv.Text = Conversion.ToInt(lst.Rows[0]["TotalUnApprovedVoucher"]).ToString();
				BankVoucherGrid.DataSource = dtBankVoucher;
				BankVoucherGrid.RetrieveStructure();
				BankGridSettings();
			}
			else
			{
				VoucherInfoBoxBank.Visible = false;
				BankVoucherGrid.ClearStructure();
				BankVoucherDetailGrid.ClearStructure();
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
			DataGridHistory.RootTable.Columns["FcyCode"].Visible = false;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Visible = false;
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
			DataGridHistory.RootTable.Columns["VoucherAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			DataGridHistory.RootTable.Columns["VoucherCode"].Caption = "V.No";
			DataGridHistory.RootTable.Columns["DocumentType"].Caption = "V.Type";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			DataGridHistory.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			DataGridHistory.RootTable.Columns["VoucherCode"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentTypeCode;
			DataGridHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 20;
			DataGridHistory.RootTable.Columns["VoucherAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
			DataGridHistory.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 100;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 20;
			DataGridHistory.RootTable.Columns["Attachment"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
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
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("AddAttachment");
			DataGridHistory.RootTable.Columns["AddAttachment"].Caption = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonText = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].Width = 110;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.RootTable.Columns["Print_102"].Position = 2;
			DataGridHistory.RootTable.Columns["PrintNew"].Position = 3;
			DataGridHistory.RootTable.Columns["DocumentType"].Position = 4;
			DataGridHistory.FrozenColumns = 5;
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
			BankVoucherGrid.RootTable.Columns["Id"].Visible = false;
			BankVoucherGrid.RootTable.Columns["DocumentTypeId"].Visible = false;
			BankVoucherGrid.RootTable.Columns["FcyCode"].Visible = false;
			BankVoucherGrid.RootTable.Columns["ExchangeRate"].Visible = false;
			BankVoucherGrid.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			BankVoucherGrid.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			BankVoucherGrid.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			BankVoucherGrid.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			BankVoucherGrid.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			BankVoucherGrid.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			BankVoucherGrid.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			BankVoucherGrid.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			BankVoucherGrid.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			BankVoucherGrid.RootTable.Columns["VoucherAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			BankVoucherGrid.RootTable.Columns["VoucherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridEX_Helper.GridWrappingAndColumnSettings(BankVoucherGrid, 2, 3);
			BankVoucherGrid.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			BankVoucherGrid.RootTable.Columns["DocumentType"].Caption = "V.Type";
			BankVoucherGrid.RootTable.Columns["VoucherCode"].Caption = "V.No";
			BankVoucherGrid.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			BankVoucherGrid.RootTable.Columns["VoucherCode"].Width = Constants.InventoryConstants.DocNoConstant;
			BankVoucherGrid.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentTypeCode;
			BankVoucherGrid.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 20;
			BankVoucherGrid.RootTable.Columns["VoucherAmount"].Width = Constants.InventoryConstants.AmountConstant;
			BankVoucherGrid.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			BankVoucherGrid.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
			BankVoucherGrid.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			BankVoucherGrid.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 100;
			BankVoucherGrid.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			BankVoucherGrid.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			BankVoucherGrid.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			BankVoucherGrid.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			BankVoucherGrid.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			BankVoucherGrid.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 20;
			BankVoucherGrid.RootTable.Columns["Attachment"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			BankVoucherGrid.RootTable.Columns["EntryDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			BankVoucherGrid.RootTable.Columns["ModifyDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			BankVoucherGrid.RootTable.Columns["ApprovedDate"].FormatString = "yyyy-MM-dd HH:mm:ss";
			BankVoucherGrid.RootTable.Columns.Add("Print");
			BankVoucherGrid.RootTable.Columns["Print"].Caption = "Print";
			BankVoucherGrid.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherGrid.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			BankVoucherGrid.RootTable.Columns["Print"].ButtonText = "Print";
			BankVoucherGrid.RootTable.Columns["Print"].Width = 40;
			BankVoucherGrid.RootTable.Columns.Add("Print_102");
			BankVoucherGrid.RootTable.Columns["Print_102"].Caption = "Print-II";
			BankVoucherGrid.RootTable.Columns["Print_102"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherGrid.RootTable.Columns["Print_102"].ButtonStyle = (ButtonStyle)5;
			BankVoucherGrid.RootTable.Columns["Print_102"].ButtonText = "Print-II";
			BankVoucherGrid.RootTable.Columns["Print_102"].Width = 55;
			BankVoucherGrid.RootTable.Columns.Add("PrintNew");
			BankVoucherGrid.RootTable.Columns["PrintNew"].Caption = "Print-III";
			BankVoucherGrid.RootTable.Columns["PrintNew"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherGrid.RootTable.Columns["PrintNew"].ButtonStyle = (ButtonStyle)5;
			BankVoucherGrid.RootTable.Columns["PrintNew"].ButtonText = "Print-III";
			BankVoucherGrid.RootTable.Columns["PrintNew"].Width = 60;
			BankVoucherGrid.RootTable.Columns.Add("Edit");
			BankVoucherGrid.RootTable.Columns["Edit"].Caption = "Edit";
			BankVoucherGrid.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherGrid.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			BankVoucherGrid.RootTable.Columns["Edit"].ButtonText = "Edit";
			BankVoucherGrid.RootTable.Columns["Edit"].Width = 40;
			BankVoucherGrid.RootTable.Columns.Add("AddAttachment");
			BankVoucherGrid.RootTable.Columns["AddAttachment"].Caption = "Add Attachments";
			BankVoucherGrid.RootTable.Columns["AddAttachment"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			BankVoucherGrid.RootTable.Columns["AddAttachment"].ButtonStyle = (ButtonStyle)5;
			BankVoucherGrid.RootTable.Columns["AddAttachment"].ButtonText = "Add Attachments";
			BankVoucherGrid.RootTable.Columns["AddAttachment"].Width = 110;
			BankVoucherGrid.RootTable.Columns["Edit"].Position = 0;
			BankVoucherGrid.RootTable.Columns["Print"].Position = 1;
			BankVoucherGrid.RootTable.Columns["Print_102"].Position = 2;
			BankVoucherGrid.RootTable.Columns["PrintNew"].Position = 3;
			BankVoucherGrid.RootTable.Columns["DocumentType"].Position = 4;
			BankVoucherGrid.FrozenColumns = 5;
			BankVoucherGrid.RootTable.Columns["Remarks"].Width = 350;
			BankVoucherGrid.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			else if (e.Column.Key == "Print")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(ID);
			}
			else if (e.Column.Key == "Print_102")
			{
				CommonServices.AcRptPaymentReceiptsVoucherSlip_102(ID);
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
				table.Columns.Add("GlCurrency");
				table.Columns.Add("SubsidiaryAccount");
				table.Columns.Add("JobLot");
				table.Columns.Add("Remarks");
				table.Columns.Add("TcyCode");
				table.Columns.Add("TcyExchangeRate", typeof(double));
				table.Columns.Add("DebitAmount", typeof(double));
				table.Columns.Add("DebitFcyAmount", typeof(double));
				table.Columns.Add("CreditAmount", typeof(double));
				table.Columns.Add("CreditFcyAmount", typeof(double));
				table.Columns.Add("ReferenceAccount");
				table.Columns.Add("BranchName");
				table.Columns.Add("CostCenter");
				table.Rows.Clear();
				List<VoucherDetail> sortedVoucherDetailList = byID.voucherDetailList.OrderBy((VoucherDetail v) => v.Id).ToList();
				for (int i = 0; i < sortedVoucherDetailList.Count; i++)
				{
					table.Rows.Add(sortedVoucherDetailList[i].AccountCode, sortedVoucherDetailList[i].AccountTitle, sortedVoucherDetailList[i].GlcyCode, sortedVoucherDetailList[i].SubsidiaryAccountTitle, sortedVoucherDetailList[i].JobLotDescription, sortedVoucherDetailList[i].Comments, sortedVoucherDetailList[i].TcyCode, sortedVoucherDetailList[i].DExchangeCurrencyRate, sortedVoucherDetailList[i].DebitAmount, (sortedVoucherDetailList[i].DebitAmount > 0.0) ? sortedVoucherDetailList[i].DCurrencyAmount : 0.0, sortedVoucherDetailList[i].CreditAmount, (sortedVoucherDetailList[i].CreditAmount > 0.0) ? sortedVoucherDetailList[i].DCurrencyAmount : 0.0, sortedVoucherDetailList[i].ReferenceAccount, sortedVoucherDetailList[i].BranchName, sortedVoucherDetailList[i].CostCenterName);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature;
				grdDetail.RootTable.Columns["CostCenter"].Visible = IsBookingOffice;
				grdDetail.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
				grdDetail.RootTable.Columns["DebitFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
				grdDetail.RootTable.Columns["CreditFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
				grdDetail.RootTable.Columns["GlCurrency"].Visible = MultiCurrencyGridWorking;
				GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3);
				grdDetail.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
				grdDetail.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
				grdDetail.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 15;
				grdDetail.RootTable.Columns["SubsidiaryAccount"].Width = Constants.InventoryConstants.AccountTitle;
				grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
				grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 100;
				grdDetail.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant;
				grdDetail.RootTable.Columns["TcyExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
				grdDetail.RootTable.Columns["DebitAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["DebitFcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["CreditAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["CreditFcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["ReferenceAccount"].Width = Constants.InventoryConstants.AccountTitle;
				grdDetail.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
				grdDetail.RootTable.Columns["CostCenter"].Width = Constants.InventoryConstants.StringConstant;
				grdDetail.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdDetail.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				grdDetail.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdDetail.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
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
			}
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.TabPages.Count == 2)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					if (tabControl1.SelectedTab.Name == "CrvTab")
					{
						FromDateCrvHistory.Focus();
					}
					else
					{
						FromDateBrvHistory.Focus();
					}
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				FromDateCrvHistory.Focus();
			}
			else if (tabControl1.SelectedIndex == 2)
			{
				FromDateBrvHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtvoucherno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnSmsTemplete_Click(object sender, EventArgs e)
	{
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
					double DiscountAmount = Conversion.ToDouble((object)txtDiscountAmount.Text);
					double taxamount = Math.Round((Conversion.ToDouble((object)txtValue.Text) + DiscountAmount) / (100.0 - Conversion.ToDouble((object)txtTaxPercent.Text)) * 100.0 * Conversion.ToDouble((object)txtTaxPercent.Text) / 100.0, 2);
					txtTaxAmount.Text = taxamount.ToString();
					double ValueAmt = Conversion.ToDouble((object)txtValue.Text);
					txtTotalAmount.Text = (taxamount + ValueAmt).ToString(clsGlobalVariables.stringFormatsingle);
				}
				((Control)(object)CmbAgainstAc).Enabled = true;
				((Control)(object)CmbWithHoldingAc).Enabled = true;
			}
			else
			{
				((Control)(object)CmbAgainstAc).Enabled = false;
				((Control)(object)CmbWithHoldingAc).Enabled = false;
				((Control)(object)CmbWithHoldingAc).Text = string.Empty;
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			checkBox1_CheckedChanged(null, null);
			txtDiscPrctAndAmount_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_CellEdited(object sender, ColumnActionEventArgs e)
	{
		try
		{
			Total();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			BranchesFill();
			combojoblotfill();
			AccountsComboBind();
			DetailAccountFill();
			BindReferenceAccounts();
			BindDiscountAccounts();
			GetSubsidoryAccounts();
			BindSubsidiaryAccount();
			BindTaxTypes();
			CostCenterFill();
			DefaultConfigurations();
			SpecialRightsImplement();
			grdCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReceiptsVoucherNew_KeyDown(object sender, KeyEventArgs e)
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
						if (tabControl1.SelectedTab.Name == "CrvTab")
						{
							btnShowCrv_Click(null, null);
						}
						else
						{
							btnShowHistoryBrv_Click(null, null);
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
					btnShowCrv_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					btnShowHistoryBrv_Click(null, null);
				}
			}
			if (Update.Visible && Update.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				Update_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
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
						if (tabControl1.SelectedTab.Name == "CrvTab")
						{
							btnNewCrvHistory_Click(null, null);
						}
						else
						{
							btnNewHistoryBrv_Click(null, null);
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnNewCrvHistory_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					btnNewHistoryBrv_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.TabPages.Count == 2)
				{
					if (tabControl1.SelectedIndex == 1)
					{
						tabControl1.SelectedIndex = 0;
						combvtype.Focus();
					}
					else
					{
						tabControl1.SelectedIndex = 1;
						if (tabControl1.SelectedTab.Name == "CrvTab")
						{
							FromDateCrvHistory.Focus();
						}
						else
						{
							FromDateBrvHistory.Focus();
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					tabControl1.SelectedIndex = 1;
					FromDateCrvHistory.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 2;
					FromDateBrvHistory.Focus();
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					tabControl1.SelectedIndex = 0;
					combvtype.Focus();
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
						if (tabControl1.SelectedTab.Name == "CrvTab")
						{
							btnRefreshCrvHistory_Click(null, null);
						}
						else
						{
							btnRefreshHistoryBrv_Click(null, null);
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					btnRefresh_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnRefreshCrvHistory_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					btnRefreshHistoryBrv_Click(null, null);
				}
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 76;
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
			{
				Print_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formright.DoHavePrintRights)
			{
				btnPrint102Old_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3) && formright.DoHavePrintRights)
			{
				BtnPaymentAndReceiptVoucherSlip_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				combvtype.Focus();
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
						((Control)(object)grd).Focus();
					}
					else if (tabControl1.SelectedIndex == 1)
					{
						if (tabControl1.SelectedTab.Name == "CrvTab")
						{
							((Control)(object)DataGridHistory).Focus();
						}
						else
						{
							((Control)(object)BankVoucherGrid).Focus();
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					((Control)(object)BankVoucherGrid).Focus();
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
						if (tabControl1.SelectedTab.Name == "CrvTab")
						{
							FromDateCrvHistory.Focus();
						}
						else
						{
							FromDateBrvHistory.Focus();
						}
					}
				}
				else if (tabControl1.SelectedIndex == 0)
				{
					CmbPaymentType.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateCrvHistory.Focus();
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					FromDateBrvHistory.Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
			{
				GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					Conversion.ToInt((object)checkedRows[i].Cells["Id"].Value.ToString());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
	}

	private void CmbTaxType_Leave(object sender, EventArgs e)
	{
		try
		{
			checkBox1_CheckedChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text) == 0.0)
			{
				txtTcyExchangeRateDetail.Focus();
			}
			CommonServices.UpdateRelatedFieldsCalculation(txtamount, txtFcyAmountDetail, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtremarks_Enter(object sender, EventArgs e)
	{
		try
		{
			txtremarks.SelectAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPrint102Old_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AcRptPaymentReceiptsVoucherSlip_102(RecId, DocumentTypeId);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)BankVoucherGrid).Name);
			if (fs != null)
			{
				BankVoucherGrid.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BankVoucherGrid_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int ID = Conversion.ToInt((object)BankVoucherGrid.CurrentRow.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt(BankVoucherGrid.CurrentRow.Cells["DocumentTypeId"].Value);
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
				CommonServices.AcRptPaymentReceiptsVoucherSlip_102(ID);
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

	private void VoucherDetailBRVByHeaderId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (BankVoucherGrid.CurrentRow == null || (int)BankVoucherGrid.CurrentRow.RowType != 0 || BankVoucherGrid.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			GridEXRow item = BankVoucherGrid.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				VoucherHead vh = VoucherHead.GetByID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
				DataTable table = new DataTable();
				table.Columns.Add("AccountTitle");
				table.Columns.Add("GlCurrency");
				table.Columns.Add("SubsidiaryAccount");
				table.Columns.Add("JobLot");
				table.Columns.Add("Remarks");
				table.Columns.Add("TcyCode");
				table.Columns.Add("TcyExchangeRate", typeof(double));
				table.Columns.Add("DebitAmount", typeof(double));
				table.Columns.Add("DebitFcyAmount", typeof(double));
				table.Columns.Add("CreditAmount", typeof(double));
				table.Columns.Add("CreditFcyAmount", typeof(double));
				table.Columns.Add("ReferenceAccount");
				table.Columns.Add("BranchName");
				table.Columns.Add("CostCenter");
				table.Columns.Add("ChequeDate", typeof(DateTime));
				table.Columns.Add("ChequeNo", typeof(string));
				table.Columns.Add("PayTitle", typeof(string));
				table.Rows.Clear();
				for (int i = 0; i < vh.voucherDetailList.Count; i++)
				{
					table.Rows.Add(vh.voucherDetailList[i].AccountTitle, vh.voucherDetailList[i].GlcyCode, vh.voucherDetailList[i].SubsidiaryAccountTitle, vh.voucherDetailList[i].JobLotDescription, vh.voucherDetailList[i].Comments, vh.voucherDetailList[i].TcyCode, vh.voucherDetailList[i].DExchangeCurrencyRate, vh.voucherDetailList[i].DebitAmount, (vh.voucherDetailList[i].DebitAmount > 0.0) ? vh.voucherDetailList[i].DCurrencyAmount : 0.0, vh.voucherDetailList[i].CreditAmount, (vh.voucherDetailList[i].CreditAmount > 0.0) ? vh.voucherDetailList[i].DCurrencyAmount : 0.0, vh.voucherDetailList[i].ReferenceAccount, vh.voucherDetailList[i].BranchName, vh.voucherDetailList[i].CostCenterName, vh.voucherDetailList[i].DCheqDate, vh.voucherDetailList[i].CheqNoDetail, vh.voucherDetailList[i].PayeeTitle);
				}
				BankVoucherDetailGrid.DataSource = table;
				BankVoucherDetailGrid.RetrieveStructure();
				BankVoucherDetailGrid.RootTable.Columns["BranchName"].Visible = BranchFeature;
				BankVoucherDetailGrid.RootTable.Columns["CostCenter"].Visible = IsBookingOffice;
				BankVoucherDetailGrid.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
				BankVoucherDetailGrid.RootTable.Columns["DebitFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
				BankVoucherDetailGrid.RootTable.Columns["CreditFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
				BankVoucherDetailGrid.RootTable.Columns["TcyCode"].Visible = MultiCurrencyGridWorking;
				GridEX_Helper.GridWrappingAndColumnSettings(BankVoucherDetailGrid, 2, 3);
				BankVoucherDetailGrid.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 50;
				BankVoucherDetailGrid.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 10;
				BankVoucherDetailGrid.RootTable.Columns["SubsidiaryAccount"].Width = Constants.InventoryConstants.AccountTitle;
				BankVoucherDetailGrid.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
				BankVoucherDetailGrid.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
				BankVoucherDetailGrid.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant;
				BankVoucherDetailGrid.RootTable.Columns["TcyExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 20;
				BankVoucherDetailGrid.RootTable.Columns["CreditAmount"].Width = Constants.InventoryConstants.AmountConstant;
				BankVoucherDetailGrid.RootTable.Columns["CreditFcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
				BankVoucherDetailGrid.RootTable.Columns["DebitAmount"].Width = Constants.InventoryConstants.AmountConstant;
				BankVoucherDetailGrid.RootTable.Columns["DebitFcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
				BankVoucherDetailGrid.RootTable.Columns["ReferenceAccount"].Width = Constants.InventoryConstants.AccountTitle;
				BankVoucherDetailGrid.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
				BankVoucherDetailGrid.RootTable.Columns["CostCenter"].Width = Constants.InventoryConstants.StringConstant;
				BankVoucherDetailGrid.RootTable.Columns["ChequeDate"].Width = Constants.InventoryConstants.DateConstant + 15;
				BankVoucherDetailGrid.RootTable.Columns["ChequeNo"].Width = Constants.InventoryConstants.StringConstant - 20;
				BankVoucherDetailGrid.RootTable.Columns["PayTitle"].Width = Constants.InventoryConstants.StringConstant;
				BankVoucherDetailGrid.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
				BankVoucherDetailGrid.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
				BankVoucherDetailGrid.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				BankVoucherDetailGrid.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				BankVoucherDetailGrid.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
				BankVoucherDetailGrid.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
				BankVoucherDetailGrid.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				BankVoucherDetailGrid.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				BankVoucherDetailGrid.RootTable.Columns["DebitFcyAmount"].AggregateFunction = (AggregateFunction)2;
				BankVoucherDetailGrid.RootTable.Columns["DebitFcyAmount"].TextAlignment = (TextAlignment)3;
				BankVoucherDetailGrid.RootTable.Columns["DebitFcyAmount"].FormatString = "#,##0.###";
				BankVoucherDetailGrid.RootTable.Columns["DebitFcyAmount"].TotalFormatString = "#,##0.###";
				BankVoucherDetailGrid.RootTable.Columns["CreditFcyAmount"].AggregateFunction = (AggregateFunction)2;
				BankVoucherDetailGrid.RootTable.Columns["CreditFcyAmount"].TextAlignment = (TextAlignment)3;
				BankVoucherDetailGrid.RootTable.Columns["CreditFcyAmount"].FormatString = "#,##0.###";
				BankVoucherDetailGrid.RootTable.Columns["CreditFcyAmount"].TotalFormatString = "#,##0.###";
				BankVoucherDetailGrid.RootTable.Columns["CreditAmount"].Caption = "Credit";
				BankVoucherDetailGrid.RootTable.Columns["DebitAmount"].Caption = "Debit";
				BankVoucherDetailGrid.RootTable.Columns["DebitFcyAmount"].Caption = "Fcy Debit";
				BankVoucherDetailGrid.RootTable.Columns["CreditFcyAmount"].Caption = "Fcy Credit";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankVoucherGrid_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = BankVoucherGrid.CurrentRow;
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

	private void BankVoucherGrid_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			int ID = Conversion.ToInt((object)BankVoucherGrid.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(ID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAllFromBrvTab_Click(object sender, EventArgs e)
	{
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

	private void BankVoucherGrid_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			VoucherDetailBRVByHeaderId();
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
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)CmbPaymentType).Text = item.Cells["PaymentType"].Value.ToString();
				combactitle.Value = Conversion.ToInt(item.Cells["AccountId"].Value);
				BindSubsidiaryAccount();
				if (SubsdiaryAccountDatasourceLength == 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("SubsidiaryAccount");
					dt.Columns.Add("TypeId");
					dt.Columns.Add("AccountId");
					dt.Columns.Add("AccountTitle");
					dt.Rows.Add(Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value), item.Cells["SubsidiaryAccount"].Text, 4, Conversion.ToInt(item.Cells["AccountId"].Value), item.Cells["AccountId"].Text);
					DDL.BindDDL(dt, CmbSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				CmbSubsidiaryAccount.Value = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				txtremarks.Text = item.Cells["Remarks"].Value.ToString();
				txtFcyAmountDetail.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString("#,##0.###");
				txtamount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString("#,##0.###");
				cmbBranchName.Value = Conversion.ToInt(item.Cells["BranchId"].Value);
				if (Conversion.ToInt(item.Cells["ReferenceAccountId"].Value) > 0)
				{
					CmbReferenceAccount.Value = Conversion.ToInt(item.Cells["ReferenceAccountId"].Value);
				}
				if (Conversion.ToInt(item.Cells["CostCenterId"].Value) > 0)
				{
					CmbCostCenter.Value = Conversion.ToInt(item.Cells["CostCenterId"].Value);
				}
				cmbTcyCodeDetail.Value = Conversion.ToInt(item.Cells["TcyCodeId"].Value);
				txtTcyExchangeRateDetail.Text = Conversion.ToDouble(item.Cells["TcyExchangeRate"].Value).ToString("#,##0.###");
				CheqDate.Value = Conversion.ToDateTime(item.Cells["ChequeDate"].Value);
				CmbCheqNo.Text = Conversion.ToString(item.Cells["ChequeNo"].Value);
				txtPayTitle.Text = Conversion.ToString(item.Cells["PayTitle"].Value);
				CmbPaymentType.Focus();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
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
			if (((UltraGridBase)CmbPaymentType).ActiveRow == null)
			{
				MessageBox.Show("PaymentType Field Required");
				CmbPaymentType.Focus();
				return;
			}
			if (((UltraGridBase)combactitle).ActiveRow == null)
			{
				MessageBox.Show("Account Title Field Required");
				combactitle.Focus();
				return;
			}
			if (MultiCurrencyGridWorking && Conversion.ToInt(combactitle.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)combactitle).SelectedRow.Cells[5].Value) == 0)
			{
				MessageBox.Show("Credit Account Currency not set Please Check");
				combactitle.Focus();
				return;
			}
			if (((UltraGridBase)CmbJobLot).ActiveRow == null)
			{
				MessageBox.Show("Job/Lot Field Required");
				CmbJobLot.Focus();
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
			if (txtamount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtamount.Text.Trim()) == 0.0)
			{
				MessageBox.Show("Amount Field Required");
				txtamount.Focus();
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
			if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountDatasourceLength > 0 && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow == null)
			{
				MessageBox.Show("Subsidiary Account Title Field Required");
				CmbSubsidiaryAccount.Focus();
				return;
			}
			if (BranchFeature && (((UltraGridBase)cmbBranchName).ActiveRow == null || Conversion.ToInt(cmbBranchName.Value) == 0))
			{
				MessageBox.Show("BranchName Field is Required");
				cmbBranchName.Focus();
				return;
			}
			table.Rows[updateDetailIndex]["PaymentType"] = Conversion.ToInt(CmbPaymentType.Value);
			table.Rows[updateDetailIndex]["AccountId"] = combactitle.Value;
			table.Rows[updateDetailIndex]["AccountCode"] = ((UltraDropDownBase)combactitle).SelectedRow.Cells[2].Value;
			table.Rows[updateDetailIndex]["GlCurrencyId"] = Conversion.ToInt(((UltraDropDownBase)combactitle).SelectedRow.Cells[5].Value);
			table.Rows[updateDetailIndex]["GlCurrency"] = ((UltraDropDownBase)combactitle).SelectedRow.Cells[6].Value;
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
					table.Rows[updateDetailIndex]["SubsidiaryAccountId"] = combactitle.Value;
					table.Rows[updateDetailIndex]["SubsidiaryAccount"] = ((Control)(object)combactitle).Text;
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
			table.Rows[updateDetailIndex]["Remarks"] = txtremarks.Text;
			table.Rows[updateDetailIndex]["TcyCodeId"] = cmbTcyCodeDetail.Value;
			table.Rows[updateDetailIndex]["TcyCode"] = ((Control)(object)cmbTcyCodeDetail).Text;
			table.Rows[updateDetailIndex]["TcyExchangeRate"] = txtTcyExchangeRateDetail.Text;
			table.Rows[updateDetailIndex]["FcyAmount"] = txtFcyAmountDetail.Text;
			table.Rows[updateDetailIndex]["Amount"] = txtamount.Text;
			table.Rows[updateDetailIndex]["ReferenceAccountId"] = Conversion.ToInt(CmbReferenceAccount.Value);
			table.Rows[updateDetailIndex]["ReferenceAccount"] = ((Conversion.ToInt(CmbReferenceAccount.Value) > 0) ? Conversion.ToString(((UltraDropDownBase)CmbReferenceAccount).SelectedRow.Cells[1].Value) : "");
			table.Rows[updateDetailIndex]["BranchId"] = Conversion.ToInt(cmbBranchName.Value);
			table.Rows[updateDetailIndex]["BranchName"] = ((Control)(object)cmbBranchName).Text;
			table.Rows[updateDetailIndex]["CostCenterId"] = Conversion.ToInt(CmbCostCenter.Value);
			table.Rows[updateDetailIndex]["ChequeDate"] = Conversion.ToDateTime((object)CheqDate.Value);
			table.Rows[updateDetailIndex]["ChequeNo"] = Conversion.ToString((object)CmbCheqNo.Text);
			table.Rows[updateDetailIndex]["PayTitle"] = Conversion.ToString((object)txtPayTitle.Text);
			grd.DataSource = table;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			Total();
			combactitle.Focus();
			updateDetailIndex = -1;
			((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
			((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
			txtamount.Clear();
			txtFcyAmountDetail.Text = string.Empty;
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			((UltraGridBase)CmbSubsidiaryAccount).DataSource = null;
			((Control)(object)CmbSubsidiaryAccount).Text = string.Empty;
			txtFcyAmountDetail.Clear();
			txtamount.Clear();
			updateDetailIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReceiptsVoucherNew_FormClosing(object sender, FormClosingEventArgs e)
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

	private void CmbSubsidiaryAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!AfterCloseup && ((UltraGridBase)CmbSubsidiaryAccount).ActiveRow != null && Conversion.ToInt(CmbSubsidiaryAccount.Value) != 0)
			{
				int GLOfSubSidory = Conversion.ToInt(((UltraDropDownBase)CmbSubsidiaryAccount).SelectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					combactitle.Value = GLOfSubSidory;
				}
			}
			AfterCloseup = false;
			if (Conversion.ToInt(combactitle.Value) > 0 && Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
			{
				labelSubsidiaryBal.Visible = true;
				labelSubsidiaryBal.Text = "0";
				double ClosingBalance = CommonServices.GetSubsiadiaryBalance(Conversion.ToInt(combactitle.Value), Conversion.ToInt(CmbSubsidiaryAccount.Value), voucherdatetime.Value);
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
			if (Conversion.ToInt(combactitle.Value) > 0 && Conversion.ToInt(CmbSubsidiaryAccount.Value) > 0)
			{
				labelSubsidiaryBal.Visible = true;
				labelSubsidiaryBal.Text = "0";
				double ClosingBalance = CommonServices.GetSubsiadiaryBalance(Conversion.ToInt(combactitle.Value), Conversion.ToInt(CmbSubsidiaryAccount.Value), voucherdatetime.Value);
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab And For Show History in History Tabs");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print I");
			dt.Rows.Add("Alt+2", "For Print II");
			dt.Rows.Add("Alt+3", "For Print III");
			dt.Rows.Add("Ctrl+F5", "For Focus on Voucher Type");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Payment Type in Detail Box");
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
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
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
			else if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Delete")
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
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

	private void BankVoucherGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int ID = 0;
			int documentTypeId = 0;
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = BankVoucherGrid.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (DataGridHistory.GetRows().Count() > 0)
			{
				ID = Conversion.ToInt(item.Cells["Id"].Value);
				documentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(ID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (BankVoucherGrid.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(ID);
				}
				else if (BankVoucherGrid.CurrentColumn.Key == "Print")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(ID);
				}
				else if (BankVoucherGrid.CurrentColumn.Key == "Print_102")
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(ID);
				}
				else if (BankVoucherGrid.CurrentColumn.Key == "Attachment")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(ID, documentTypeId);
				}
				else if (BankVoucherGrid.CurrentColumn.Key == "AddAttachment")
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
			if (base.ActiveControl.Name == "ctrlGrdBar1")
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
			if (!ShiftTabPressed)
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

	private void Add_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
	}

	private void btnCancelDetail_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		try
		{
			if (e.Shift && e.KeyCode == Keys.Tab)
			{
				ShiftTabPressed = true;
			}
			else
			{
				ShiftTabPressed = false;
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
				if (Conversion.ToInt(combvtype.Value) == 0)
				{
					MessageBox.Show("Please Select Voucher Type First");
					combvtype.Focus();
					return;
				}
				obj.DocumentTypeIds = Conversion.ToString(combvtype.Value);
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
			if (Conversion.ToInt(combvtype.Value) > 0)
			{
				CommonServices.PaymentAndReceiptVoucherSlip(RecId, Conversion.ToInt(combvtype.Value));
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

	private void btnRefreshHistoryBrv_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindForBrvHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistoryBrv_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateBrvHistory.Value = DateTime.Now;
			ToDateBrvHistory.Value = DateTime.Now;
			txtFromDocNoBrvHistory.Text = string.Empty;
			txtToDocNoBrvHistory.Text = string.Empty;
			((Control)(object)CmbAccountTitleBrvHistory).Text = string.Empty;
			((UltraGridBase)cmbApproveStatusBrv).Rows[0].Activate();
			VoucherInfoBoxBank.Visible = false;
			BankVoucherGrid.ClearStructure();
			BankVoucherDetailGrid.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistoryBrv_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFillBrv();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewCrvHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateCrvHistory.Value = DateTime.Now;
			ToDateCrvHistory.Value = DateTime.Now;
			txtFromDocNoCrv.Text = string.Empty;
			txtToDocNoCrv.Text = string.Empty;
			((Control)(object)cmbAccountTitleCrv).Text = string.Empty;
			((UltraGridBase)cmbApprovedStatusCrv).Rows[0].Activate();
			VouchersInfoBoxCash.Visible = false;
			DataGridHistory.ClearStructure();
			grdDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshCrvHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindForCrvHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowCrv_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFillCrv();
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
					combactitle.Value = GLOfSubSidory;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowCrv_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name != null && base.ActiveControl.Name == "rdcrvmodify")
			{
				FromDateCrvHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistoryBrv_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name != null && base.ActiveControl.Name == "BankVoucherGrid")
			{
				FromDateBrvHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoCrv_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoCrv_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFromDocNoBrvHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoBrvHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void rdbtnAccountName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtAccount.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(combactitle.Value) > 0)
				{
					Id = Conversion.ToInt(combactitle.Value);
				}
				if (rdbtnAccountName.Checked)
				{
					DDL.BindDDL(dtAccount, combactitle, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtAccount, combactitle, "Id", "AccountCode", "Account Code", ZeroIndex: true);
				}
				if (Id > 0)
				{
					combactitle.Value = Id;
					combactitle.Focus();
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

	private void txtDiscPrctAndAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtValue.Text) == 0.0)
			{
				CmbPaymentType.Focus();
			}
			CommonServices.UpdateRelatedFieldsCalculation(txtDiscountAmount, txtDiscPercent, Conversion.ToDouble((object)txtValue.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 100, "", 3);
			checkBox1_CheckedChanged(null, null);
			Total();
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			if (((UltraDropDownBase)CmbDebitAccount).SelectedRow != null && ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value != null)
			{
				int headerCurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value);
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
			if (((UltraDropDownBase)CmbDebitAccount).SelectedRow != null && ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value != null)
			{
				requiredCurrencyIds.Add(Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value));
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
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Expected O, but got Unknown
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Expected O, but got Unknown
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Expected O, but got Unknown
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Expected O, but got Unknown
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Expected O, but got Unknown
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba3: Expected O, but got Unknown
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Expected O, but got Unknown
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Expected O, but got Unknown
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d71: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Expected O, but got Unknown
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Expected O, but got Unknown
		//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8c: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Expected O, but got Unknown
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_107e: Expected O, but got Unknown
		//IL_1114: Unknown result type (might be due to invalid IL or missing references)
		//IL_111e: Expected O, but got Unknown
		//IL_1161: Unknown result type (might be due to invalid IL or missing references)
		//IL_116b: Expected O, but got Unknown
		//IL_1e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1c: Expected O, but got Unknown
		//IL_1e29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e33: Expected O, but got Unknown
		//IL_1e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4a: Expected O, but got Unknown
		//IL_1e57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e61: Expected O, but got Unknown
		//IL_1e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e78: Expected O, but got Unknown
		//IL_a723: Unknown result type (might be due to invalid IL or missing references)
		//IL_a72d: Expected O, but got Unknown
		//IL_a751: Unknown result type (might be due to invalid IL or missing references)
		//IL_a75b: Expected O, but got Unknown
		//IL_ce98: Unknown result type (might be due to invalid IL or missing references)
		//IL_cea2: Expected O, but got Unknown
		//IL_cec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_ced0: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
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
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
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
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance Appearance1 = new Appearance();
		Appearance Appearance5 = new Appearance();
		Appearance Appearance6 = new Appearance();
		Appearance Appearance7 = new Appearance();
		Appearance Appearance8 = new Appearance();
		Appearance Appearance9 = new Appearance();
		Appearance Appearance10 = new Appearance();
		Appearance Appearance11 = new Appearance();
		Appearance Appearance12 = new Appearance();
		Appearance Appearance2 = new Appearance();
		Appearance Appearance3 = new Appearance();
		Appearance Appearance4 = new Appearance();
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
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance165 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance230 = new Appearance();
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
		GridEXLayout BankVoucherDetailGrid_DesignTimeLayout = new GridEXLayout();
		GridEXLayout BankVoucherGrid_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdMultiCurrency_DesignTimeLayout = new GridEXLayout();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnEdit = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnD = new System.Windows.Forms.ToolStripButton();
		this.btnCopy = new System.Windows.Forms.ToolStripButton();
		this.btnPaste = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel22 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.txtDiscountAmount = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.txtDiscPercent = new System.Windows.Forms.TextBox();
		this.CmbDiscountAccount = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbWithHoldingAc = new UltraCombo();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbTaxType = new UltraCombo();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.txtValue = new System.Windows.Forms.TextBox();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.ChkBoxWthHolding = new System.Windows.Forms.CheckBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupboxdetail = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.PaymentTypePanel = new System.Windows.Forms.Panel();
		this.CmbPaymentType = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CreditAcPanel = new System.Windows.Forms.Panel();
		this.rdbtnAccountCode = new System.Windows.Forms.RadioButton();
		this.rdbtnAccountName = new System.Windows.Forms.RadioButton();
		this.combactitle = new UltraCombo();
		this.lblbalancedetail = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.SubsidiaryAcPanel = new System.Windows.Forms.Panel();
		this.labelSubsidiaryBal = new System.Windows.Forms.Label();
		this.CmbSubsidiaryAccount = new UltraCombo();
		this.lblSubsidiaryaccount = new System.Windows.Forms.Label();
		this.JobLotPanel = new System.Windows.Forms.Panel();
		this.CmbJobLot = new UltraCombo();
		this.lblJoblot = new System.Windows.Forms.Label();
		this.RemarksPanel = new System.Windows.Forms.Panel();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.lblRemarks = new System.Windows.Forms.Label();
		this.FcyAmountPanel = new System.Windows.Forms.Panel();
		this.txtFcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.AmountPanel = new System.Windows.Forms.Panel();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.ReferenceAccountPanel = new System.Windows.Forms.Panel();
		this.rdBtnRefAcByCode = new System.Windows.Forms.RadioButton();
		this.rdBtnRefAcByTitle = new System.Windows.Forms.RadioButton();
		this.CmbReferenceAccount = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.BranchNamePanel = new System.Windows.Forms.Panel();
		this.cmbBranchName = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.CostCenterPanel = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.label39 = new System.Windows.Forms.Label();
		this.ChequeDetailPanel = new System.Windows.Forms.Panel();
		this.CheqDate = new System.Windows.Forms.DateTimePicker();
		this.lblCheqDate = new System.Windows.Forms.Label();
		this.CmbCheqNo = new System.Windows.Forms.TextBox();
		this.txtPayTitle = new System.Windows.Forms.TextBox();
		this.lblCheqNo = new System.Windows.Forms.Label();
		this.lblPayTitle = new System.Windows.Forms.Label();
		this.ButtonsPanel = new System.Windows.Forms.Panel();
		this.Add = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.CmbProjectId = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.cmbCurrency = new UltraCombo();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.combvtype = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbAgainstAc = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtremarksmain = new System.Windows.Forms.TextBox();
		this.txtbalance = new System.Windows.Forms.Label();
		this.CmbDebitAccount = new UltraCombo();
		this.txtvoucherno = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.voucherdatetime = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
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
		this.btnSmsTemplete = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnPrint102Old = new System.Windows.Forms.ToolStripButton();
		this.BtnPaymentAndReceiptVoucherSlip = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnSpecialRights = new System.Windows.Forms.ToolStripButton();
		this.CrvTab = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label35 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.rdcrvapproved = new System.Windows.Forms.RadioButton();
		this.rdcrvmodify = new System.Windows.Forms.RadioButton();
		this.rdcrvdoc = new System.Windows.Forms.RadioButton();
		this.rdcrventry = new System.Windows.Forms.RadioButton();
		this.VouchersInfoBoxCash = new System.Windows.Forms.GroupBox();
		this.label42 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchers = new System.Windows.Forms.Label();
		this.lblTotalVouchers = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.lblApprovedVouchers = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label26 = new System.Windows.Forms.Label();
		this.cmbAccountTitleCrv = new UltraCombo();
		this.btnShowCrv = new System.Windows.Forms.Button();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.cmbApprovedStatusCrv = new UltraCombo();
		this.txtToDocNoCrv = new System.Windows.Forms.TextBox();
		this.txtFromDocNoCrv = new System.Windows.Forms.TextBox();
		this.ToDateCrvHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateCrvHistory = new System.Windows.Forms.DateTimePicker();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNewCrvHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshCrvHistory = new System.Windows.Forms.ToolStripButton();
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
		this.BrvTab = new System.Windows.Forms.TabPage();
		this.panel19 = new System.Windows.Forms.Panel();
		this.BankVoucherDetailGrid = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.BankVoucherGrid = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.VoucherInfoBoxBank = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchersBpv = new System.Windows.Forms.Label();
		this.lblTotalVouchersBpv = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.lblApprovedVouchersBpv = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdbrvapproved = new System.Windows.Forms.RadioButton();
		this.rdbrvmodify = new System.Windows.Forms.RadioButton();
		this.rdbrvdoc = new System.Windows.Forms.RadioButton();
		this.rdbrventry = new System.Windows.Forms.RadioButton();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbAccountTitleBrvHistory = new UltraCombo();
		this.btnShowHistoryBrv = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbApproveStatusBrv = new UltraCombo();
		this.txtToDocNoBrvHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoBrvHistory = new System.Windows.Forms.TextBox();
		this.ToDateBrvHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateBrvHistory = new System.Windows.Forms.DateTimePicker();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistoryBrv = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistoryBrv = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAllFromBrvTab = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.grdMultiCurrency = new GridEX();
		this.lblDrAcGlCurrency = new System.Windows.Forms.Label();
		this.txtBaseFcyCode = new System.Windows.Forms.TextBox();
		this.lblBaseFcyCode = new System.Windows.Forms.Label();
		this.txtBaseFcyExchangeRate = new System.Windows.Forms.TextBox();
		this.lblBaseFcyExchangeRate = new System.Windows.Forms.Label();
		this.panel26 = new System.Windows.Forms.Panel();
		this.cmbTcyCodeDetail = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.txtTcyExchangeRateDetail = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.btnCreateUpdateMultiCurrency = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWithHoldingAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		this.panel2.SuspendLayout();
		this.groupboxdetail.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.PaymentTypePanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentType).BeginInit();
		this.CreditAcPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combactitle).BeginInit();
		this.SubsidiaryAcPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).BeginInit();
		this.JobLotPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		this.RemarksPanel.SuspendLayout();
		this.FcyAmountPanel.SuspendLayout();
		this.AmountPanel.SuspendLayout();
		this.ReferenceAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).BeginInit();
		this.BranchNamePanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		this.CostCenterPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		this.ChequeDetailPanel.SuspendLayout();
		this.ButtonsPanel.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combvtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgainstAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.CrvTab.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.VouchersInfoBoxCash.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitleCrv).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusCrv).BeginInit();
		this.panel13.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.BrvTab.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.BankVoucherDetailGrid).BeginInit();
		this.panel18.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.BankVoucherGrid).BeginInit();
		this.panel16.SuspendLayout();
		this.VoucherInfoBoxBank.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleBrvHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatusBrv).BeginInit();
		this.panel15.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).BeginInit();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTcyCodeDetail).BeginInit();
		this.panel25.SuspendLayout();
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
			this.btnNew, this.btnEdit, this.btnSave, this.btnPrint, this.toolStripSeparator, this.btnD, this.btnCopy, this.btnPaste, this.toolStripSeparator1, this.toolStripButton1,
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
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 24);
		this.btnPrint.Text = "&Print";
		this.btnD.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnD.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnD.Name = "btnD";
		this.btnD.Size = new System.Drawing.Size(64, 24);
		this.btnD.Text = "&Delete";
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
		this.panel1.Size = new System.Drawing.Size(1452, 684);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.CrvTab);
		this.tabControl1.Controls.Add(this.BrvTab);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1452, 684);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.panel6);
		this.Form.Controls.Add(this.panel2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(1444, 658);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel22);
		this.panel6.Controls.Add(this.panel21);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 311);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1440, 345);
		this.panel6.TabIndex = 1;
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1440, 185);
		this.panel22.TabIndex = 1;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1440, 185);
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
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.grdMultiCurrency);
		this.panel21.Controls.Add(this.txtDiscountAmount);
		this.panel21.Controls.Add(this.label50);
		this.panel21.Controls.Add(this.label51);
		this.panel21.Controls.Add(this.txtDiscPercent);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountAccount);
		this.panel21.Controls.Add(this.label52);
		this.panel21.Controls.Add(this.label23);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc);
		this.panel21.Controls.Add(this.txtTaxAmount);
		this.panel21.Controls.Add(this.label12);
		this.panel21.Controls.Add(this.label9);
		this.panel21.Controls.Add(this.label8);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.panel21.Controls.Add(this.txtTotalAmount);
		this.panel21.Controls.Add(this.txtValue);
		this.panel21.Controls.Add(this.txtTaxPercent);
		this.panel21.Controls.Add(this.ChkBoxWthHolding);
		this.panel21.Controls.Add(this.label4);
		this.panel21.Controls.Add(this.label21);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel21.Location = new System.Drawing.Point(0, 185);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1440, 160);
		this.panel21.TabIndex = 3;
		this.txtDiscountAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtDiscountAmount.BackColor = System.Drawing.Color.White;
		this.txtDiscountAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscountAmount.Location = new System.Drawing.Point(1296, 55);
		this.txtDiscountAmount.Name = "txtDiscountAmount";
		this.txtDiscountAmount.Size = new System.Drawing.Size(142, 23);
		this.txtDiscountAmount.TabIndex = 5;
		this.txtDiscountAmount.Tag = "DiscAmount";
		this.txtDiscountAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountAmount.TextChanged += new System.EventHandler(txtDiscPrctAndAmount_TextChanged);
		this.txtDiscountAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtamount_KeyPress);
		this.label50.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(1215, 51);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(75, 30);
		this.label50.TabIndex = 51;
		this.label50.Text = "Discount Amount";
		this.label51.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(773, 59);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(125, 15);
		this.label51.TabIndex = 52;
		this.label51.Text = "Discount Account (Cr)";
		this.txtDiscPercent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtDiscPercent.BackColor = System.Drawing.Color.White;
		this.txtDiscPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscPercent.Location = new System.Drawing.Point(1140, 55);
		this.txtDiscPercent.Name = "txtDiscPercent";
		this.txtDiscPercent.Size = new System.Drawing.Size(72, 23);
		this.txtDiscPercent.TabIndex = 4;
		this.txtDiscPercent.Tag = "DiscPercnt";
		this.txtDiscPercent.TextChanged += new System.EventHandler(txtDiscPrctAndAmount_TextChanged);
		this.txtDiscPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtamount_KeyPress);
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
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Location = new System.Drawing.Point(902, 54);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Name = "CmbDiscountAccount";
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Size = new System.Drawing.Size(191, 24);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).TabIndex = 3;
		this.label52.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label52.AutoSize = true;
		this.label52.Location = new System.Drawing.Point(1094, 60);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(39, 13);
		this.label52.TabIndex = 53;
		this.label52.Text = "Disc %";
		this.label23.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(573, 32);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(50, 15);
		this.label23.TabIndex = 40;
		this.label23.Text = "TaxType";
		this.CmbWithHoldingAc.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbWithHoldingAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWithHoldingAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWithHoldingAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWithHoldingAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Location = new System.Drawing.Point(902, 29);
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Name = "CmbWithHoldingAc";
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Size = new System.Drawing.Size(191, 24);
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).TabIndex = 2;
		this.txtTaxAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtTaxAmount.BackColor = System.Drawing.Color.White;
		this.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(1296, 30);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(142, 23);
		this.txtTaxAmount.TabIndex = 8;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(773, 34);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(83, 15);
		this.label12.TabIndex = 1;
		this.label12.Text = "WHT Account";
		this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(1215, 83);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(80, 15);
		this.label9.TabIndex = 14;
		this.label9.Text = "Total Amount";
		this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1216, 34);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 15);
		this.label8.TabIndex = 12;
		this.label8.Text = "Tax Amount";
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance178;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance183).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance184).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(627, 29);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(92, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_Leave);
		this.txtTotalAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(1296, 79);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(142, 23);
		this.txtTotalAmount.TabIndex = 6;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtValue.BackColor = System.Drawing.Color.White;
		this.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtValue.Location = new System.Drawing.Point(1296, 4);
		this.txtValue.Name = "txtValue";
		this.txtValue.ReadOnly = true;
		this.txtValue.Size = new System.Drawing.Size(142, 23);
		this.txtValue.TabIndex = 9;
		this.txtValue.TabStop = false;
		this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtValue.TextChanged += new System.EventHandler(textBox1_TextChanged);
		this.txtTaxPercent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(1140, 30);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(72, 23);
		this.txtTaxPercent.TabIndex = 7;
		this.txtTaxPercent.TabStop = false;
		this.ChkBoxWthHolding.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxWthHolding.AutoSize = true;
		this.ChkBoxWthHolding.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxWthHolding.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBoxWthHolding.Location = new System.Drawing.Point(721, 32);
		this.ChkBoxWthHolding.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxWthHolding.Name = "ChkBoxWthHolding";
		this.ChkBoxWthHolding.Size = new System.Drawing.Size(54, 19);
		this.ChkBoxWthHolding.TabIndex = 5;
		this.ChkBoxWthHolding.TabStop = false;
		this.ChkBoxWthHolding.Text = "WHT";
		this.ChkBoxWthHolding.UseVisualStyleBackColor = true;
		this.ChkBoxWthHolding.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1216, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(51, 15);
		this.label4.TabIndex = 10;
		this.label4.Text = "Amount";
		this.label21.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1094, 34);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(38, 15);
		this.label21.TabIndex = 33;
		this.label21.Text = "Tax %";
		this.panel2.Controls.Add(this.groupboxdetail);
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1440, 309);
		this.panel2.TabIndex = 0;
		this.groupboxdetail.BackColor = System.Drawing.Color.White;
		this.groupboxdetail.Controls.Add(this.flowLayoutPanel1);
		this.groupboxdetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupboxdetail.Location = new System.Drawing.Point(4, 194);
		this.groupboxdetail.Name = "groupboxdetail";
		this.groupboxdetail.Size = new System.Drawing.Size(1206, 112);
		this.groupboxdetail.TabIndex = 1;
		this.groupboxdetail.TabStop = false;
		this.groupboxdetail.Text = "Detail";
		this.flowLayoutPanel1.Controls.Add(this.PaymentTypePanel);
		this.flowLayoutPanel1.Controls.Add(this.CreditAcPanel);
		this.flowLayoutPanel1.Controls.Add(this.SubsidiaryAcPanel);
		this.flowLayoutPanel1.Controls.Add(this.JobLotPanel);
		this.flowLayoutPanel1.Controls.Add(this.RemarksPanel);
		this.flowLayoutPanel1.Controls.Add(this.panel26);
		this.flowLayoutPanel1.Controls.Add(this.panel25);
		this.flowLayoutPanel1.Controls.Add(this.FcyAmountPanel);
		this.flowLayoutPanel1.Controls.Add(this.AmountPanel);
		this.flowLayoutPanel1.Controls.Add(this.ReferenceAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.BranchNamePanel);
		this.flowLayoutPanel1.Controls.Add(this.CostCenterPanel);
		this.flowLayoutPanel1.Controls.Add(this.ChequeDetailPanel);
		this.flowLayoutPanel1.Controls.Add(this.ButtonsPanel);
		this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 13);
		this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(1200, 97);
		this.flowLayoutPanel1.TabIndex = 0;
		this.PaymentTypePanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentType);
		this.PaymentTypePanel.Controls.Add(this.label3);
		this.PaymentTypePanel.Location = new System.Drawing.Point(0, 0);
		this.PaymentTypePanel.Margin = new System.Windows.Forms.Padding(0);
		this.PaymentTypePanel.Name = "PaymentTypePanel";
		this.PaymentTypePanel.Size = new System.Drawing.Size(96, 47);
		this.PaymentTypePanel.TabIndex = 0;
		this.CmbPaymentType.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance227;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Location = new System.Drawing.Point(2, 21);
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Name = "CmbPaymentType";
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1, 2);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(79, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "PaymentType";
		this.CreditAcPanel.Controls.Add(this.rdbtnAccountCode);
		this.CreditAcPanel.Controls.Add(this.rdbtnAccountName);
		this.CreditAcPanel.Controls.Add((System.Windows.Forms.Control)(object)this.combactitle);
		this.CreditAcPanel.Controls.Add(this.lblbalancedetail);
		this.CreditAcPanel.Controls.Add(this.label11);
		this.CreditAcPanel.Location = new System.Drawing.Point(96, 0);
		this.CreditAcPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CreditAcPanel.Name = "CreditAcPanel";
		this.CreditAcPanel.Size = new System.Drawing.Size(256, 47);
		this.CreditAcPanel.TabIndex = 1;
		this.rdbtnAccountCode.AutoSize = true;
		this.rdbtnAccountCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnAccountCode.Location = new System.Drawing.Point(103, 1);
		this.rdbtnAccountCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnAccountCode.Name = "rdbtnAccountCode";
		this.rdbtnAccountCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnAccountCode.TabIndex = 133;
		this.rdbtnAccountCode.Text = "Code";
		this.rdbtnAccountCode.UseVisualStyleBackColor = true;
		this.rdbtnAccountName.AutoSize = true;
		this.rdbtnAccountName.Checked = true;
		this.rdbtnAccountName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnAccountName.Location = new System.Drawing.Point(56, 1);
		this.rdbtnAccountName.Margin = new System.Windows.Forms.Padding(0);
		this.rdbtnAccountName.Name = "rdbtnAccountName";
		this.rdbtnAccountName.Size = new System.Drawing.Size(48, 19);
		this.rdbtnAccountName.TabIndex = 132;
		this.rdbtnAccountName.TabStop = true;
		this.rdbtnAccountName.Text = "Title";
		this.rdbtnAccountName.UseVisualStyleBackColor = true;
		this.rdbtnAccountName.CheckedChanged += new System.EventHandler(rdbtnAccountName_CheckedChanged);
		this.combactitle.AllowNull = (DefaultableBoolean)1;
		this.combactitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.combactitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combactitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combactitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.combactitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combactitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combactitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combactitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance125;
		((SpecialBoxBase)((UltraGridBase)this.combactitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combactitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.combactitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combactitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.combactitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combactitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.combactitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combactitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combactitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combactitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combactitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combactitle).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.combactitle).Name = "combactitle";
		((System.Windows.Forms.Control)(object)this.combactitle).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.combactitle).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combactitle).Leave += new System.EventHandler(combactitle_Leave);
		this.lblbalancedetail.AutoSize = true;
		this.lblbalancedetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.lblbalancedetail.ForeColor = System.Drawing.Color.Red;
		this.lblbalancedetail.Location = new System.Drawing.Point(152, 4);
		this.lblbalancedetail.Name = "lblbalancedetail";
		this.lblbalancedetail.Size = new System.Drawing.Size(48, 14);
		this.lblbalancedetail.TabIndex = 34;
		this.lblbalancedetail.Text = "label8";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(2, 3);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(55, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Credit Ac";
		this.SubsidiaryAcPanel.Controls.Add(this.labelSubsidiaryBal);
		this.SubsidiaryAcPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount);
		this.SubsidiaryAcPanel.Controls.Add(this.lblSubsidiaryaccount);
		this.SubsidiaryAcPanel.Location = new System.Drawing.Point(352, 0);
		this.SubsidiaryAcPanel.Margin = new System.Windows.Forms.Padding(0);
		this.SubsidiaryAcPanel.Name = "SubsidiaryAcPanel";
		this.SubsidiaryAcPanel.Size = new System.Drawing.Size(171, 47);
		this.SubsidiaryAcPanel.TabIndex = 2;
		this.labelSubsidiaryBal.AutoSize = true;
		this.labelSubsidiaryBal.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelSubsidiaryBal.Location = new System.Drawing.Point(83, 2);
		this.labelSubsidiaryBal.Name = "labelSubsidiaryBal";
		this.labelSubsidiaryBal.Size = new System.Drawing.Size(48, 14);
		this.labelSubsidiaryBal.TabIndex = 595;
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
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Name = "CmbSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).TabIndex = 0;
		this.CmbSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbSubsidiaryAccount_AfterCloseUp);
		this.CmbSubsidiaryAccount.ValueChanged += new System.EventHandler(CmbSubsidiaryAccount_Valuechanged);
		((System.Windows.Forms.Control)(object)this.CmbSubsidiaryAccount).Leave += new System.EventHandler(CmbSubsidiaryAccount_Leave);
		this.lblSubsidiaryaccount.AutoSize = true;
		this.lblSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubsidiaryaccount.Location = new System.Drawing.Point(0, 2);
		this.lblSubsidiaryaccount.Name = "lblSubsidiaryaccount";
		this.lblSubsidiaryaccount.Size = new System.Drawing.Size(84, 15);
		this.lblSubsidiaryaccount.TabIndex = 40;
		this.lblSubsidiaryaccount.Text = "Subsidiary A/c";
		this.JobLotPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.JobLotPanel.Controls.Add(this.lblJoblot);
		this.JobLotPanel.Location = new System.Drawing.Point(523, 0);
		this.JobLotPanel.Margin = new System.Windows.Forms.Padding(0);
		this.JobLotPanel.Name = "JobLotPanel";
		this.JobLotPanel.Size = new System.Drawing.Size(138, 47);
		this.JobLotPanel.TabIndex = 3;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(137, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 0;
		this.lblJoblot.AutoSize = true;
		this.lblJoblot.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJoblot.Location = new System.Drawing.Point(3, 2);
		this.lblJoblot.Name = "lblJoblot";
		this.lblJoblot.Size = new System.Drawing.Size(48, 15);
		this.lblJoblot.TabIndex = 12;
		this.lblJoblot.Text = "Job/Lot";
		this.RemarksPanel.Controls.Add(this.txtremarks);
		this.RemarksPanel.Controls.Add(this.lblRemarks);
		this.RemarksPanel.Location = new System.Drawing.Point(661, 0);
		this.RemarksPanel.Margin = new System.Windows.Forms.Padding(0);
		this.RemarksPanel.Name = "RemarksPanel";
		this.RemarksPanel.Size = new System.Drawing.Size(280, 47);
		this.RemarksPanel.TabIndex = 4;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.txtremarks.Location = new System.Drawing.Point(0, 23);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(280, 23);
		this.txtremarks.TabIndex = 0;
		this.txtremarks.Enter += new System.EventHandler(txtremarks_Enter);
		this.lblRemarks.AutoSize = true;
		this.lblRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRemarks.Location = new System.Drawing.Point(0, 3);
		this.lblRemarks.Name = "lblRemarks";
		this.lblRemarks.Size = new System.Drawing.Size(52, 15);
		this.lblRemarks.TabIndex = 2;
		this.lblRemarks.Text = "Remarks";
		this.FcyAmountPanel.Controls.Add(this.txtFcyAmountDetail);
		this.FcyAmountPanel.Controls.Add(this.label43);
		this.FcyAmountPanel.Location = new System.Drawing.Point(0, 47);
		this.FcyAmountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.FcyAmountPanel.Name = "FcyAmountPanel";
		this.FcyAmountPanel.Size = new System.Drawing.Size(96, 47);
		this.FcyAmountPanel.TabIndex = 7;
		this.txtFcyAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmountDetail.Location = new System.Drawing.Point(0, 23);
		this.txtFcyAmountDetail.Name = "txtFcyAmountDetail";
		this.txtFcyAmountDetail.ShortcutsEnabled = false;
		this.txtFcyAmountDetail.Size = new System.Drawing.Size(96, 23);
		this.txtFcyAmountDetail.TabIndex = 0;
		this.txtFcyAmountDetail.Tag = "FcyAmount";
		this.txtFcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountDetail.TextChanged += new System.EventHandler(txtFcyAmountDetail_TextChanged);
		this.txtFcyAmountDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtamount_KeyPress);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(1, 4);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(72, 15);
		this.label43.TabIndex = 4;
		this.label43.Text = "Tcy Amount";
		this.AmountPanel.Controls.Add(this.txtamount);
		this.AmountPanel.Controls.Add(this.label20);
		this.AmountPanel.Location = new System.Drawing.Point(96, 47);
		this.AmountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.AmountPanel.Name = "AmountPanel";
		this.AmountPanel.Size = new System.Drawing.Size(118, 47);
		this.AmountPanel.TabIndex = 8;
		this.txtamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtamount.Location = new System.Drawing.Point(0, 23);
		this.txtamount.Name = "txtamount";
		this.txtamount.ShortcutsEnabled = false;
		this.txtamount.Size = new System.Drawing.Size(117, 23);
		this.txtamount.TabIndex = 0;
		this.txtamount.Tag = "CrAmount";
		this.txtamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtamount.TextChanged += new System.EventHandler(txtamount_TextChanged);
		this.txtamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtamount_KeyPress);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1, 4);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(85, 15);
		this.label20.TabIndex = 4;
		this.label20.Text = "Credit Amount";
		this.ReferenceAccountPanel.Controls.Add(this.rdBtnRefAcByCode);
		this.ReferenceAccountPanel.Controls.Add(this.rdBtnRefAcByTitle);
		this.ReferenceAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceAccount);
		this.ReferenceAccountPanel.Controls.Add(this.label46);
		this.ReferenceAccountPanel.Location = new System.Drawing.Point(214, 47);
		this.ReferenceAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ReferenceAccountPanel.Name = "ReferenceAccountPanel";
		this.ReferenceAccountPanel.Size = new System.Drawing.Size(175, 47);
		this.ReferenceAccountPanel.TabIndex = 9;
		this.rdBtnRefAcByCode.AutoSize = true;
		this.rdBtnRefAcByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdBtnRefAcByCode.Location = new System.Drawing.Point(121, 1);
		this.rdBtnRefAcByCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdBtnRefAcByCode.Name = "rdBtnRefAcByCode";
		this.rdBtnRefAcByCode.Size = new System.Drawing.Size(52, 19);
		this.rdBtnRefAcByCode.TabIndex = 133;
		this.rdBtnRefAcByCode.Text = "Code";
		this.rdBtnRefAcByCode.UseVisualStyleBackColor = true;
		this.rdBtnRefAcByTitle.AutoSize = true;
		this.rdBtnRefAcByTitle.Checked = true;
		this.rdBtnRefAcByTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdBtnRefAcByTitle.Location = new System.Drawing.Point(77, 1);
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
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbReferenceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Name = "CmbReferenceAccount";
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).TabIndex = 0;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1, 2);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(76, 15);
		this.label46.TabIndex = 38;
		this.label46.Text = "Reference Ac";
		this.BranchNamePanel.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.BranchNamePanel.Controls.Add(this.label41);
		this.BranchNamePanel.Location = new System.Drawing.Point(389, 47);
		this.BranchNamePanel.Margin = new System.Windows.Forms.Padding(0);
		this.BranchNamePanel.Name = "BranchNamePanel";
		this.BranchNamePanel.Size = new System.Drawing.Size(164, 47);
		this.BranchNamePanel.TabIndex = 10;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(164, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(0, 2);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(79, 15);
		this.label41.TabIndex = 594;
		this.label41.Text = "Branch Name";
		this.CostCenterPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.CostCenterPanel.Controls.Add(this.label39);
		this.CostCenterPanel.Location = new System.Drawing.Point(553, 47);
		this.CostCenterPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CostCenterPanel.Name = "CostCenterPanel";
		this.CostCenterPanel.Size = new System.Drawing.Size(147, 47);
		this.CostCenterPanel.TabIndex = 11;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance117).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 0;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(0, 2);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(67, 15);
		this.label39.TabIndex = 594;
		this.label39.Text = "Cost Center";
		this.ChequeDetailPanel.Controls.Add(this.CheqDate);
		this.ChequeDetailPanel.Controls.Add(this.lblCheqDate);
		this.ChequeDetailPanel.Controls.Add(this.CmbCheqNo);
		this.ChequeDetailPanel.Controls.Add(this.txtPayTitle);
		this.ChequeDetailPanel.Controls.Add(this.lblCheqNo);
		this.ChequeDetailPanel.Controls.Add(this.lblPayTitle);
		this.ChequeDetailPanel.Location = new System.Drawing.Point(700, 47);
		this.ChequeDetailPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ChequeDetailPanel.Name = "ChequeDetailPanel";
		this.ChequeDetailPanel.Size = new System.Drawing.Size(415, 47);
		this.ChequeDetailPanel.TabIndex = 12;
		this.CheqDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CheqDate.CustomFormat = "dd-MMM-yy";
		this.CheqDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CheqDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.CheqDate.Location = new System.Drawing.Point(3, 23);
		this.CheqDate.Name = "CheqDate";
		this.CheqDate.Size = new System.Drawing.Size(120, 23);
		this.CheqDate.TabIndex = 0;
		this.lblCheqDate.AutoSize = true;
		this.lblCheqDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCheqDate.Location = new System.Drawing.Point(3, 2);
		this.lblCheqDate.Name = "lblCheqDate";
		this.lblCheqDate.Size = new System.Drawing.Size(75, 15);
		this.lblCheqDate.TabIndex = 33;
		this.lblCheqDate.Text = "Cheque Date";
		this.CmbCheqNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.CmbCheqNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCheqNo.Location = new System.Drawing.Point(124, 23);
		this.CmbCheqNo.Name = "CmbCheqNo";
		this.CmbCheqNo.Size = new System.Drawing.Size(113, 23);
		this.CmbCheqNo.TabIndex = 1;
		this.txtPayTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPayTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPayTitle.Location = new System.Drawing.Point(238, 23);
		this.txtPayTitle.Name = "txtPayTitle";
		this.txtPayTitle.Size = new System.Drawing.Size(175, 23);
		this.txtPayTitle.TabIndex = 2;
		this.lblCheqNo.AutoSize = true;
		this.lblCheqNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCheqNo.Location = new System.Drawing.Point(129, 2);
		this.lblCheqNo.Name = "lblCheqNo";
		this.lblCheqNo.Size = new System.Drawing.Size(66, 15);
		this.lblCheqNo.TabIndex = 0;
		this.lblCheqNo.Text = "Cheque No";
		this.lblPayTitle.AutoSize = true;
		this.lblPayTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPayTitle.Location = new System.Drawing.Point(239, 2);
		this.lblPayTitle.Name = "lblPayTitle";
		this.lblPayTitle.Size = new System.Drawing.Size(49, 15);
		this.lblPayTitle.TabIndex = 35;
		this.lblPayTitle.Text = "PayTitle";
		this.ButtonsPanel.Controls.Add(this.Add);
		this.ButtonsPanel.Controls.Add(this.btnUpdateDetail);
		this.ButtonsPanel.Controls.Add(this.btnCancelDetail);
		this.ButtonsPanel.Location = new System.Drawing.Point(1115, 47);
		this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ButtonsPanel.Name = "ButtonsPanel";
		this.ButtonsPanel.Size = new System.Drawing.Size(70, 47);
		this.ButtonsPanel.TabIndex = 13;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(2, 21);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(39, 25);
		this.Add.TabIndex = 0;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click_1);
		this.Add.Leave += new System.EventHandler(Add_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(4, 21);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(60, 25);
		this.btnUpdateDetail.TabIndex = 1;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(2, -1);
		this.btnCancelDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(62, 25);
		this.btnCancelDetail.TabIndex = 1;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(btnCancelDetail_Leave);
		this.btnCancelDetail.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(btnCancelDetail_PreviewKeyDown);
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.txtBaseFcyCode);
		this.groupBox2.Controls.Add(this.lblBaseFcyCode);
		this.groupBox2.Controls.Add(this.txtBaseFcyExchangeRate);
		this.groupBox2.Controls.Add(this.lblBaseFcyExchangeRate);
		this.groupBox2.Controls.Add(this.lblDrAcGlCurrency);
		this.groupBox2.Controls.Add(this.chkCustomAccounts);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProjectId);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.lblExchangeRate);
		this.groupBox2.Controls.Add(this.lblFcyAmount);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox2.Controls.Add(this.lblFcyCode);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combvtype);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAgainstAc);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.txtremarksmain);
		this.groupBox2.Controls.Add(this.txtbalance);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccount);
		this.groupBox2.Controls.Add(this.txtvoucherno);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.voucherdatetime);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 65);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1206, 127);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCustomAccounts.Location = new System.Drawing.Point(721, 98);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(120, 19);
		this.chkCustomAccounts.TabIndex = 10;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.CmbProjectId.AllowNull = (DefaultableBoolean)1;
		this.CmbProjectId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProjectId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProjectId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance220).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProjectId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Location = new System.Drawing.Point(100, 13);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Name = "CmbProjectId";
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Size = new System.Drawing.Size(232, 24);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(44, 15);
		this.label5.TabIndex = 566;
		this.label5.Text = "Project";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(807, 41);
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
		this.lblExchangeRate.Location = new System.Drawing.Point(722, 37);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(82, 30);
		this.lblExchangeRate.TabIndex = 52;
		this.lblExchangeRate.Text = "Tcy Exchange \r\nRate";
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFcyAmount.Location = new System.Drawing.Point(722, 71);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(72, 15);
		this.lblFcyAmount.TabIndex = 50;
		this.lblFcyAmount.Text = "Tcy Amount";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(807, 67);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(111, 23);
		this.txtFcyAmount.TabIndex = 9;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance201;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance209).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(807, 12);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(722, 18);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 48;
		this.lblFcyCode.Text = "Tcy Code";
		this.combvtype.AllowNull = (DefaultableBoolean)1;
		this.combvtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combvtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combvtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)Appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combvtype).DisplayLayout.Appearance = (AppearanceBase)(object)Appearance1;
		((UltraGridBase)this.combvtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combvtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)Appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)Appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)Appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combvtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)Appearance5;
		((AppearanceBase)Appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)Appearance6;
		((SpecialBoxBase)((UltraGridBase)this.combvtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)Appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)Appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)Appearance7;
		((UltraGridBase)this.combvtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combvtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)Appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)Appearance8;
		((AppearanceBase)Appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)Appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)Appearance9;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)Appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)Appearance10;
		((AppearanceBase)Appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)Appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)Appearance11;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)Appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)Appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)Appearance12;
		((AppearanceBase)Appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combvtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)Appearance2;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)Appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)Appearance3;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)Appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combvtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)Appearance4;
		((UltraGridBase)this.combvtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combvtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combvtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combvtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combvtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combvtype).Location = new System.Drawing.Point(100, 38);
		((System.Windows.Forms.Control)(object)this.combvtype).Name = "combvtype";
		((System.Windows.Forms.Control)(object)this.combvtype).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.combvtype).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combvtype).Leave += new System.EventHandler(combvtype_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(6, 44);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(79, 15);
		this.label14.TabIndex = 6;
		this.label14.Text = "Voucher Type";
		this.CmbAgainstAc.AllowNull = (DefaultableBoolean)1;
		this.CmbAgainstAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAgainstAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAgainstAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAgainstAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Location = new System.Drawing.Point(441, 12);
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Name = "CmbAgainstAc";
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Size = new System.Drawing.Size(276, 26);
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).TabIndex = 5;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(375, 18);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(64, 15);
		this.label13.TabIndex = 32;
		this.label13.Text = "Against Ac";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(373, 43);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "Remarks";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtremarksmain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksmain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksmain.Location = new System.Drawing.Point(441, 39);
		this.txtremarksmain.Multiline = true;
		this.txtremarksmain.Name = "txtremarksmain";
		this.txtremarksmain.Size = new System.Drawing.Size(276, 78);
		this.txtremarksmain.TabIndex = 6;
		this.txtbalance.AutoSize = true;
		this.txtbalance.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbalance.ForeColor = System.Drawing.Color.Red;
		this.txtbalance.Location = new System.Drawing.Point(336, 98);
		this.txtbalance.Name = "txtbalance";
		this.txtbalance.Size = new System.Drawing.Size(48, 14);
		this.txtbalance.TabIndex = 30;
		this.txtbalance.Text = "label8";
		this.CmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance240;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Location = new System.Drawing.Point(100, 91);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Name = "CmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Leave += new System.EventHandler(combcreditac_Leave);
		this.txtvoucherno.BackColor = System.Drawing.Color.White;
		this.txtvoucherno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvoucherno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvoucherno.Location = new System.Drawing.Point(100, 66);
		this.txtvoucherno.Name = "txtvoucherno";
		this.txtvoucherno.ReadOnly = true;
		this.txtvoucherno.Size = new System.Drawing.Size(86, 23);
		this.txtvoucherno.TabIndex = 2;
		this.txtvoucherno.Leave += new System.EventHandler(txtvoucherno_TextChanged);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 97);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "Debit Account";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(6, 70);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(70, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Voucher No";
		this.voucherdatetime.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.voucherdatetime.CustomFormat = "dd-MMM-yy";
		this.voucherdatetime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.voucherdatetime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.voucherdatetime.Location = new System.Drawing.Point(231, 66);
		this.voucherdatetime.Name = "voucherdatetime";
		this.voucherdatetime.Size = new System.Drawing.Size(101, 23);
		this.voucherdatetime.TabIndex = 3;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(191, 70);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(33, 15);
		this.label16.TabIndex = 2;
		this.label16.Text = "Date";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.labelLocationType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.PaymentVoucherLable);
		this.panel4.Controls.Add(this.ChkPrint3);
		this.panel4.Controls.Add(this.ChkPrint2);
		this.panel4.Controls.Add(this.ChkPrint1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1440, 32);
		this.panel4.TabIndex = 0;
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(247, 9);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 504;
		this.labelLocationType.Text = "Location Type";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(336, 3);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 503;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabStop = false;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1394, 5);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 21);
		this.ctrlGrdBar1.TabIndex = 19;
		this.ctrlGrdBar1.TabStop = false;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.PaymentVoucherLable.AutoSize = true;
		this.PaymentVoucherLable.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PaymentVoucherLable.ForeColor = System.Drawing.Color.White;
		this.PaymentVoucherLable.Location = new System.Drawing.Point(3, 6);
		this.PaymentVoucherLable.Name = "PaymentVoucherLable";
		this.PaymentVoucherLable.Size = new System.Drawing.Size(176, 19);
		this.PaymentVoucherLable.TabIndex = 0;
		this.PaymentVoucherLable.Text = "RECEIPTS VOUCHER";
		this.ChkPrint3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint3.AutoSize = true;
		this.ChkPrint3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint3.ForeColor = System.Drawing.Color.White;
		this.ChkPrint3.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint3.Location = new System.Drawing.Point(1274, 6);
		this.ChkPrint3.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint3.Name = "ChkPrint3";
		this.ChkPrint3.Size = new System.Drawing.Size(110, 19);
		this.ChkPrint3.TabIndex = 12;
		this.ChkPrint3.TabStop = false;
		this.ChkPrint3.Text = "Print Preview III";
		this.ChkPrint3.UseVisualStyleBackColor = true;
		this.ChkPrint2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint2.AutoSize = true;
		this.ChkPrint2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint2.ForeColor = System.Drawing.Color.White;
		this.ChkPrint2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint2.Location = new System.Drawing.Point(1170, 6);
		this.ChkPrint2.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint2.Name = "ChkPrint2";
		this.ChkPrint2.Size = new System.Drawing.Size(106, 19);
		this.ChkPrint2.TabIndex = 12;
		this.ChkPrint2.TabStop = false;
		this.ChkPrint2.Text = "Print Preview II";
		this.ChkPrint2.UseVisualStyleBackColor = true;
		this.ChkPrint1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint1.AutoSize = true;
		this.ChkPrint1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint1.ForeColor = System.Drawing.Color.White;
		this.ChkPrint1.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkPrint1.Location = new System.Drawing.Point(1064, 6);
		this.ChkPrint1.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrint1.Name = "ChkPrint1";
		this.ChkPrint1.Size = new System.Drawing.Size(95, 19);
		this.ChkPrint1.TabIndex = 12;
		this.ChkPrint1.TabStop = false;
		this.ChkPrint1.Text = "Print Preview";
		this.ChkPrint1.UseVisualStyleBackColor = true;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1440, 30);
		this.panel3.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.New, this.btnRefresh, this.Update, this.Save, this.toolStripButton10, this.btnSmsTemplete, this.btnDelete, this.Print, this.btnPrint102Old, this.BtnPaymentAndReceiptVoucherSlip,
			this.btnShortCutKey, this.btnSpecialRights, this.btnCreateUpdateMultiCurrency
		});
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1440, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(59, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.Update.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Update.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.Update.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Update.Name = "Update";
		this.Update.Size = new System.Drawing.Size(78, 24);
		this.Update.Text = "&Update";
		this.Update.Click += new System.EventHandler(Update_Click);
		this.Save.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(64, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.toolStripButton10.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton10.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton10.Name = "toolStripButton10";
		this.toolStripButton10.Size = new System.Drawing.Size(109, 24);
		this.toolStripButton10.Text = "&Attachment";
		this.toolStripButton10.Click += new System.EventHandler(toolStripButton10_Click);
		this.btnSmsTemplete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSmsTemplete.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.btnSmsTemplete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSmsTemplete.Name = "btnSmsTemplete";
		this.btnSmsTemplete.Size = new System.Drawing.Size(60, 24);
		this.btnSmsTemplete.Text = "&SMS";
		this.btnSmsTemplete.Visible = false;
		this.btnSmsTemplete.Click += new System.EventHandler(btnSmsTemplete_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "102-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnPrint102Old.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint102Old.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint102Old.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint102Old.Name = "btnPrint102Old";
		this.btnPrint102Old.Size = new System.Drawing.Size(109, 24);
		this.btnPrint102Old.Text = "102-Print-II";
		this.btnPrint102Old.Click += new System.EventHandler(btnPrint102Old_Click);
		this.BtnPaymentAndReceiptVoucherSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPaymentAndReceiptVoucherSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPaymentAndReceiptVoucherSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPaymentAndReceiptVoucherSlip.Name = "BtnPaymentAndReceiptVoucherSlip";
		this.BtnPaymentAndReceiptVoucherSlip.Size = new System.Drawing.Size(77, 24);
		this.BtnPaymentAndReceiptVoucherSlip.Text = "102-III";
		this.BtnPaymentAndReceiptVoucherSlip.Click += new System.EventHandler(BtnPaymentAndReceiptVoucherSlip_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnSpecialRights.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSpecialRights.Image = Architecture.WinApp.Properties.Resources.RightsIcon;
		this.btnSpecialRights.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSpecialRights.Name = "btnSpecialRights";
		this.btnSpecialRights.Size = new System.Drawing.Size(123, 24);
		this.btnSpecialRights.Text = "Special Rights";
		this.btnSpecialRights.Click += new System.EventHandler(btnSpecialRights_Click);
		this.CrvTab.Controls.Add(this.panel8);
		this.CrvTab.Location = new System.Drawing.Point(4, 4);
		this.CrvTab.Margin = new System.Windows.Forms.Padding(2);
		this.CrvTab.Name = "CrvTab";
		this.CrvTab.Padding = new System.Windows.Forms.Padding(2);
		this.CrvTab.Size = new System.Drawing.Size(1217, 627);
		this.CrvTab.TabIndex = 1;
		this.CrvTab.Text = "CRV History";
		this.CrvTab.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1213, 623);
		this.panel8.TabIndex = 0;
		this.panel11.Controls.Add(this.panel10);
		this.panel11.Controls.Add(this.panel7);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1213, 623);
		this.panel11.TabIndex = 2;
		this.panel10.Controls.Add(this.panel20);
		this.panel10.Controls.Add(this.panel14);
		this.panel10.Controls.Add(this.panel9);
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.toolStrip4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1213, 623);
		this.panel10.TabIndex = 45;
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel20.Location = new System.Drawing.Point(0, 429);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1213, 194);
		this.panel20.TabIndex = 55;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1213, 194);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label35);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 404);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1213, 25);
		this.panel14.TabIndex = 54;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(3, 3);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(242, 19);
		this.label35.TabIndex = 26;
		this.label35.Text = "Detail of above selected row";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 134);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1213, 270);
		this.panel9.TabIndex = 53;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1213, 270);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 44;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel12.Controls.Add(this.rdcrvapproved);
		this.panel12.Controls.Add(this.rdcrvmodify);
		this.panel12.Controls.Add(this.rdcrvdoc);
		this.panel12.Controls.Add(this.rdcrventry);
		this.panel12.Controls.Add(this.VouchersInfoBoxCash);
		this.panel12.Controls.Add(this.groupBox1);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 56);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1213, 78);
		this.panel12.TabIndex = 52;
		this.rdcrvapproved.AutoSize = true;
		this.rdcrvapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdcrvapproved.Location = new System.Drawing.Point(961, 53);
		this.rdcrvapproved.Name = "rdcrvapproved";
		this.rdcrvapproved.Size = new System.Drawing.Size(105, 19);
		this.rdcrvapproved.TabIndex = 9;
		this.rdcrvapproved.Text = "Approved Date";
		this.rdcrvapproved.UseVisualStyleBackColor = true;
		this.rdcrvmodify.AutoSize = true;
		this.rdcrvmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdcrvmodify.Location = new System.Drawing.Point(961, 33);
		this.rdcrvmodify.Name = "rdcrvmodify";
		this.rdcrvmodify.Size = new System.Drawing.Size(91, 19);
		this.rdcrvmodify.TabIndex = 8;
		this.rdcrvmodify.Text = "Modify Date";
		this.rdcrvmodify.UseVisualStyleBackColor = true;
		this.rdcrvdoc.AutoSize = true;
		this.rdcrvdoc.Checked = true;
		this.rdcrvdoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdcrvdoc.Location = new System.Drawing.Point(880, 33);
		this.rdcrvdoc.Name = "rdcrvdoc";
		this.rdcrvdoc.Size = new System.Drawing.Size(75, 19);
		this.rdcrvdoc.TabIndex = 6;
		this.rdcrvdoc.TabStop = true;
		this.rdcrvdoc.Text = "Doc Date";
		this.rdcrvdoc.UseVisualStyleBackColor = true;
		this.rdcrventry.AutoSize = true;
		this.rdcrventry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdcrventry.Location = new System.Drawing.Point(880, 53);
		this.rdcrventry.Name = "rdcrventry";
		this.rdcrventry.Size = new System.Drawing.Size(80, 19);
		this.rdcrventry.TabIndex = 7;
		this.rdcrventry.Text = "Entry Date";
		this.rdcrventry.UseVisualStyleBackColor = true;
		this.VouchersInfoBoxCash.Controls.Add(this.label42);
		this.VouchersInfoBoxCash.Controls.Add(this.lblUnApprovedVouchers);
		this.VouchersInfoBoxCash.Controls.Add(this.lblTotalVouchers);
		this.VouchersInfoBoxCash.Controls.Add(this.label45);
		this.VouchersInfoBoxCash.Controls.Add(this.lblApprovedVouchers);
		this.VouchersInfoBoxCash.Controls.Add(this.label47);
		this.VouchersInfoBoxCash.Location = new System.Drawing.Point(1068, 4);
		this.VouchersInfoBoxCash.Name = "VouchersInfoBoxCash";
		this.VouchersInfoBoxCash.Size = new System.Drawing.Size(242, 69);
		this.VouchersInfoBoxCash.TabIndex = 9;
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
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitleCrv);
		this.groupBox1.Controls.Add(this.btnShowCrv);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCrv);
		this.groupBox1.Controls.Add(this.txtToDocNoCrv);
		this.groupBox1.Controls.Add(this.txtFromDocNoCrv);
		this.groupBox1.Controls.Add(this.ToDateCrvHistory);
		this.groupBox1.Controls.Add(this.FromDateCrvHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1060, 71);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(482, 22);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(78, 15);
		this.label26.TabIndex = 147;
		this.label26.Text = "Account Title";
		this.cmbAccountTitleCrv.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitleCrv.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitleCrv.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitleCrv.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance163;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitleCrv).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCrv).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitleCrv.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCrv).Location = new System.Drawing.Point(482, 39);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCrv).Name = "cmbAccountTitleCrv";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCrv).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitleCrv).TabIndex = 4;
		this.btnShowCrv.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowCrv.FlatAppearance.BorderSize = 0;
		this.btnShowCrv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowCrv.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowCrv.ForeColor = System.Drawing.Color.White;
		this.btnShowCrv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowCrv.Location = new System.Drawing.Point(818, 39);
		this.btnShowCrv.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowCrv.Name = "btnShowCrv";
		this.btnShowCrv.Size = new System.Drawing.Size(53, 26);
		this.btnShowCrv.TabIndex = 10;
		this.btnShowCrv.Text = "Show";
		this.btnShowCrv.UseVisualStyleBackColor = false;
		this.btnShowCrv.Click += new System.EventHandler(btnShowCrv_Click);
		this.btnShowCrv.Leave += new System.EventHandler(btnShowCrv_Leave);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(696, 22);
		this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(95, 15);
		this.label28.TabIndex = 142;
		this.label28.Text = "Approved Status";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(378, 22);
		this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(64, 15);
		this.label29.TabIndex = 141;
		this.label29.Text = "To Doc No";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(274, 22);
		this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(79, 15);
		this.label30.TabIndex = 140;
		this.label30.Text = "From Doc No";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(140, 22);
		this.label31.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(48, 15);
		this.label31.TabIndex = 139;
		this.label31.Text = "To Date";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(4, 23);
		this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(63, 15);
		this.label32.TabIndex = 13;
		this.label32.Text = "From Date";
		this.cmbApprovedStatusCrv.AllowNull = (DefaultableBoolean)1;
		this.cmbApprovedStatusCrv.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApprovedStatusCrv.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApprovedStatusCrv.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApprovedStatusCrv).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCrv).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApprovedStatusCrv.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCrv).Location = new System.Drawing.Point(696, 39);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCrv).Name = "cmbApprovedStatusCrv";
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCrv).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusCrv).TabIndex = 5;
		this.txtToDocNoCrv.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoCrv.Location = new System.Drawing.Point(378, 41);
		this.txtToDocNoCrv.Name = "txtToDocNoCrv";
		this.txtToDocNoCrv.ShortcutsEnabled = false;
		this.txtToDocNoCrv.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoCrv.TabIndex = 3;
		this.txtToDocNoCrv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoCrv_KeyPress);
		this.txtFromDocNoCrv.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoCrv.Location = new System.Drawing.Point(274, 41);
		this.txtFromDocNoCrv.Name = "txtFromDocNoCrv";
		this.txtFromDocNoCrv.ShortcutsEnabled = false;
		this.txtFromDocNoCrv.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoCrv.TabIndex = 2;
		this.txtFromDocNoCrv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoCrv_KeyPress);
		this.ToDateCrvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateCrvHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateCrvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateCrvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateCrvHistory.Location = new System.Drawing.Point(140, 41);
		this.ToDateCrvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateCrvHistory.Name = "ToDateCrvHistory";
		this.ToDateCrvHistory.ShowCheckBox = true;
		this.ToDateCrvHistory.Size = new System.Drawing.Size(130, 23);
		this.ToDateCrvHistory.TabIndex = 1;
		this.FromDateCrvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateCrvHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateCrvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateCrvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateCrvHistory.Location = new System.Drawing.Point(4, 41);
		this.FromDateCrvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateCrvHistory.Name = "FromDateCrvHistory";
		this.FromDateCrvHistory.ShowCheckBox = true;
		this.FromDateCrvHistory.Size = new System.Drawing.Size(130, 23);
		this.FromDateCrvHistory.TabIndex = 0;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label25);
		this.panel13.Controls.Add(this.ctrlGrdBar3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 27);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1213, 29);
		this.panel13.TabIndex = 51;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(4, 4);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(249, 19);
		this.label25.TabIndex = 25;
		this.label25.Text = "Cash Receipt Voucher History";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1171, 4);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewCrvHistory, this.btnRefreshCrvHistory, this.btnLoadAll });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1213, 27);
		this.toolStrip4.TabIndex = 50;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNewCrvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewCrvHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewCrvHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewCrvHistory.Name = "btnNewCrvHistory";
		this.btnNewCrvHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewCrvHistory.Text = "&Reset";
		this.btnNewCrvHistory.Click += new System.EventHandler(btnNewCrvHistory_Click);
		this.btnRefreshCrvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshCrvHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshCrvHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshCrvHistory.Name = "btnRefreshCrvHistory";
		this.btnRefreshCrvHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshCrvHistory.Text = "Refresh";
		this.btnRefreshCrvHistory.Click += new System.EventHandler(btnRefreshCrvHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.btnLoadAll.Image = (System.Drawing.Image)resources.GetObject("btnLoadAll.Image");
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(84, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
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
		this.BrvTab.Controls.Add(this.panel19);
		this.BrvTab.Controls.Add(this.panel18);
		this.BrvTab.Controls.Add(this.panel17);
		this.BrvTab.Controls.Add(this.panel16);
		this.BrvTab.Controls.Add(this.panel15);
		this.BrvTab.Controls.Add(this.toolStrip3);
		this.BrvTab.Location = new System.Drawing.Point(4, 4);
		this.BrvTab.Name = "BrvTab";
		this.BrvTab.Size = new System.Drawing.Size(1217, 627);
		this.BrvTab.TabIndex = 2;
		this.BrvTab.Text = "BRV History";
		this.BrvTab.UseVisualStyleBackColor = true;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.BankVoucherDetailGrid);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 417);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1217, 210);
		this.panel19.TabIndex = 54;
		this.BankVoucherDetailGrid.AllowEdit = (InheritableBoolean)2;
		BankVoucherDetailGrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.BankVoucherDetailGrid.DesignTimeLayout = BankVoucherDetailGrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.BankVoucherDetailGrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.BankVoucherDetailGrid.DynamicFiltering = true;
		this.BankVoucherDetailGrid.FilterMode = (FilterMode)2;
		this.BankVoucherDetailGrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.BankVoucherDetailGrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.BankVoucherDetailGrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.BankVoucherDetailGrid).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.BankVoucherDetailGrid).Name = "BankVoucherDetailGrid";
		((ScrollableBase)this.BankVoucherDetailGrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.BankVoucherDetailGrid).Size = new System.Drawing.Size(1217, 210);
		((System.Windows.Forms.Control)(object)this.BankVoucherDetailGrid).TabIndex = 48;
		this.BankVoucherDetailGrid.TotalRow = (InheritableBoolean)1;
		this.BankVoucherDetailGrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.BankVoucherDetailGrid).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label22);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 391);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1217, 26);
		this.panel18.TabIndex = 53;
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.Teal;
		this.label22.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(2, 4);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(242, 19);
		this.label22.TabIndex = 54;
		this.label22.Text = "Detail of above selected row";
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.BankVoucherGrid);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 133);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1217, 258);
		this.panel17.TabIndex = 52;
		this.BankVoucherGrid.AllowEdit = (InheritableBoolean)2;
		this.BankVoucherGrid.DefaultFilterRowComparison = (FilterConditionOperator)7;
		BankVoucherGrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.BankVoucherGrid.DesignTimeLayout = BankVoucherGrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.BankVoucherGrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.BankVoucherGrid.DynamicFiltering = true;
		this.BankVoucherGrid.FilterMode = (FilterMode)2;
		this.BankVoucherGrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.BankVoucherGrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.BankVoucherGrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.BankVoucherGrid).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.BankVoucherGrid).Name = "BankVoucherGrid";
		((ScrollableBase)this.BankVoucherGrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.BankVoucherGrid).Size = new System.Drawing.Size(1217, 258);
		((System.Windows.Forms.Control)(object)this.BankVoucherGrid).TabIndex = 47;
		this.BankVoucherGrid.TotalRow = (InheritableBoolean)1;
		this.BankVoucherGrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.BankVoucherGrid).VisualStyle = (VisualStyle)4;
		this.BankVoucherGrid.ColumnButtonClick += new ColumnActionEventHandler(BankVoucherGrid_ColumnButtonClick);
		this.BankVoucherGrid.SelectionChanged += new System.EventHandler(BankVoucherGrid_SelectionChanged);
		this.BankVoucherGrid.LinkClicked += new ColumnActionEventHandler(BankVoucherGrid_LinkClicked);
		((System.Windows.Forms.Control)(object)this.BankVoucherGrid).DoubleClick += new System.EventHandler(BankVoucherGrid_DoubleClick);
		((System.Windows.Forms.Control)(object)this.BankVoucherGrid).KeyDown += new System.Windows.Forms.KeyEventHandler(BankVoucherGrid_KeyDown);
		this.panel16.Controls.Add(this.VoucherInfoBoxBank);
		this.panel16.Controls.Add(this.groupBox3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 55);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1217, 78);
		this.panel16.TabIndex = 51;
		this.VoucherInfoBoxBank.Controls.Add(this.label36);
		this.VoucherInfoBoxBank.Controls.Add(this.lblUnApprovedVouchersBpv);
		this.VoucherInfoBoxBank.Controls.Add(this.lblTotalVouchersBpv);
		this.VoucherInfoBoxBank.Controls.Add(this.label38);
		this.VoucherInfoBoxBank.Controls.Add(this.lblApprovedVouchersBpv);
		this.VoucherInfoBoxBank.Controls.Add(this.label37);
		this.VoucherInfoBoxBank.Location = new System.Drawing.Point(1069, 5);
		this.VoucherInfoBoxBank.Name = "VoucherInfoBoxBank";
		this.VoucherInfoBoxBank.Size = new System.Drawing.Size(242, 69);
		this.VoucherInfoBoxBank.TabIndex = 8;
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
		this.lblUnApprovedVouchersBpv.Location = new System.Drawing.Point(154, 47);
		this.lblUnApprovedVouchersBpv.Name = "lblUnApprovedVouchersBpv";
		this.lblUnApprovedVouchersBpv.Size = new System.Drawing.Size(80, 17);
		this.lblUnApprovedVouchersBpv.TabIndex = 6;
		this.lblUnApprovedVouchersBpv.Text = "0";
		this.lblUnApprovedVouchersBpv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTotalVouchersBpv.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalVouchersBpv.Location = new System.Drawing.Point(154, 11);
		this.lblTotalVouchersBpv.Name = "lblTotalVouchersBpv";
		this.lblTotalVouchersBpv.Size = new System.Drawing.Size(80, 17);
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
		this.lblApprovedVouchersBpv.Location = new System.Drawing.Point(154, 28);
		this.lblApprovedVouchersBpv.Name = "lblApprovedVouchersBpv";
		this.lblApprovedVouchersBpv.Size = new System.Drawing.Size(80, 17);
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
		this.groupBox3.Controls.Add(this.rdbrvapproved);
		this.groupBox3.Controls.Add(this.rdbrvmodify);
		this.groupBox3.Controls.Add(this.rdbrvdoc);
		this.groupBox3.Controls.Add(this.rdbrventry);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitleBrvHistory);
		this.groupBox3.Controls.Add(this.btnShowHistoryBrv);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatusBrv);
		this.groupBox3.Controls.Add(this.txtToDocNoBrvHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoBrvHistory);
		this.groupBox3.Controls.Add(this.ToDateBrvHistory);
		this.groupBox3.Controls.Add(this.FromDateBrvHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1064, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdbrvapproved.AutoSize = true;
		this.rdbrvapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbrvapproved.Location = new System.Drawing.Point(952, 48);
		this.rdbrvapproved.Name = "rdbrvapproved";
		this.rdbrvapproved.Size = new System.Drawing.Size(105, 19);
		this.rdbrvapproved.TabIndex = 9;
		this.rdbrvapproved.Text = "Approved Date";
		this.rdbrvapproved.UseVisualStyleBackColor = true;
		this.rdbrvmodify.AutoSize = true;
		this.rdbrvmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbrvmodify.Location = new System.Drawing.Point(952, 28);
		this.rdbrvmodify.Name = "rdbrvmodify";
		this.rdbrvmodify.Size = new System.Drawing.Size(91, 19);
		this.rdbrvmodify.TabIndex = 8;
		this.rdbrvmodify.Text = "Modify Date";
		this.rdbrvmodify.UseVisualStyleBackColor = true;
		this.rdbrvdoc.AutoSize = true;
		this.rdbrvdoc.Checked = true;
		this.rdbrvdoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbrvdoc.Location = new System.Drawing.Point(871, 28);
		this.rdbrvdoc.Name = "rdbrvdoc";
		this.rdbrvdoc.Size = new System.Drawing.Size(75, 19);
		this.rdbrvdoc.TabIndex = 6;
		this.rdbrvdoc.TabStop = true;
		this.rdbrvdoc.Text = "Doc Date";
		this.rdbrvdoc.UseVisualStyleBackColor = true;
		this.rdbrventry.AutoSize = true;
		this.rdbrventry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbrventry.Location = new System.Drawing.Point(871, 48);
		this.rdbrventry.Name = "rdbrventry";
		this.rdbrventry.Size = new System.Drawing.Size(80, 19);
		this.rdbrventry.TabIndex = 7;
		this.rdbrventry.Text = "Entry Date";
		this.rdbrventry.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(476, 22);
		this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(78, 15);
		this.label24.TabIndex = 147;
		this.label24.Text = "Account Title";
		this.CmbAccountTitleBrvHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitleBrvHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitleBrvHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitleBrvHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitleBrvHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBrvHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitleBrvHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBrvHistory).Location = new System.Drawing.Point(476, 39);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBrvHistory).Name = "CmbAccountTitleBrvHistory";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBrvHistory).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleBrvHistory).TabIndex = 4;
		this.btnShowHistoryBrv.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowHistoryBrv.FlatAppearance.BorderSize = 0;
		this.btnShowHistoryBrv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistoryBrv.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistoryBrv.ForeColor = System.Drawing.Color.White;
		this.btnShowHistoryBrv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowHistoryBrv.Location = new System.Drawing.Point(814, 39);
		this.btnShowHistoryBrv.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistoryBrv.Name = "btnShowHistoryBrv";
		this.btnShowHistoryBrv.Size = new System.Drawing.Size(53, 26);
		this.btnShowHistoryBrv.TabIndex = 10;
		this.btnShowHistoryBrv.Text = "Show";
		this.btnShowHistoryBrv.UseVisualStyleBackColor = false;
		this.btnShowHistoryBrv.Click += new System.EventHandler(btnShowHistoryBrv_Click);
		this.btnShowHistoryBrv.Leave += new System.EventHandler(btnShowHistoryBrv_Leave);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(694, 22);
		this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(95, 15);
		this.label18.TabIndex = 142;
		this.label18.Text = "Approved Status";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(374, 22);
		this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(64, 15);
		this.label17.TabIndex = 141;
		this.label17.Text = "To Doc No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(273, 22);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 140;
		this.label7.Text = "From Doc No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(139, 22);
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
		this.cmbApproveStatusBrv.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatusBrv.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatusBrv.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatusBrv.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatusBrv).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBrv).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApproveStatusBrv.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBrv).Location = new System.Drawing.Point(694, 39);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBrv).Name = "cmbApproveStatusBrv";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBrv).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusBrv).TabIndex = 5;
		this.txtToDocNoBrvHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoBrvHistory.Location = new System.Drawing.Point(374, 41);
		this.txtToDocNoBrvHistory.Name = "txtToDocNoBrvHistory";
		this.txtToDocNoBrvHistory.ShortcutsEnabled = false;
		this.txtToDocNoBrvHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoBrvHistory.TabIndex = 3;
		this.txtToDocNoBrvHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoBrvHistory_KeyPress);
		this.txtFromDocNoBrvHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoBrvHistory.Location = new System.Drawing.Point(273, 41);
		this.txtFromDocNoBrvHistory.Name = "txtFromDocNoBrvHistory";
		this.txtFromDocNoBrvHistory.ShortcutsEnabled = false;
		this.txtFromDocNoBrvHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoBrvHistory.TabIndex = 2;
		this.txtFromDocNoBrvHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoBrvHistory_KeyPress);
		this.ToDateBrvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateBrvHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateBrvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateBrvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateBrvHistory.Location = new System.Drawing.Point(139, 41);
		this.ToDateBrvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateBrvHistory.Name = "ToDateBrvHistory";
		this.ToDateBrvHistory.ShowCheckBox = true;
		this.ToDateBrvHistory.Size = new System.Drawing.Size(130, 23);
		this.ToDateBrvHistory.TabIndex = 1;
		this.FromDateBrvHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateBrvHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateBrvHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateBrvHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateBrvHistory.Location = new System.Drawing.Point(5, 41);
		this.FromDateBrvHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateBrvHistory.Name = "FromDateBrvHistory";
		this.FromDateBrvHistory.ShowCheckBox = true;
		this.FromDateBrvHistory.Size = new System.Drawing.Size(130, 23);
		this.FromDateBrvHistory.TabIndex = 0;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label6);
		this.panel15.Controls.Add(this.ctrlGrdBar4);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 27);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1217, 28);
		this.panel15.TabIndex = 50;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(5, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(251, 19);
		this.label6.TabIndex = 22;
		this.label6.Text = "Bank Receipt Voucher History";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1171, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.BankVoucherGrid;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar4.TabIndex = 24;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistoryBrv, this.btnRefreshHistoryBrv, this.btnLoadAllFromBrvTab });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1217, 27);
		this.toolStrip3.TabIndex = 49;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistoryBrv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistoryBrv.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistoryBrv.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistoryBrv.Name = "btnNewHistoryBrv";
		this.btnNewHistoryBrv.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistoryBrv.Text = "&Reset";
		this.btnNewHistoryBrv.Click += new System.EventHandler(btnNewHistoryBrv_Click);
		this.btnRefreshHistoryBrv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistoryBrv.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistoryBrv.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistoryBrv.Name = "btnRefreshHistoryBrv";
		this.btnRefreshHistoryBrv.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistoryBrv.Text = "Refresh";
		this.btnRefreshHistoryBrv.Click += new System.EventHandler(btnRefreshHistoryBrv_Click);
		this.btnLoadAllFromBrvTab.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.btnLoadAllFromBrvTab.Image = (System.Drawing.Image)resources.GetObject("btnLoadAllFromBrvTab.Image");
		this.btnLoadAllFromBrvTab.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllFromBrvTab.Name = "btnLoadAllFromBrvTab";
		this.btnLoadAllFromBrvTab.Size = new System.Drawing.Size(84, 24);
		this.btnLoadAllFromBrvTab.Text = "LoadAll";
		this.btnLoadAllFromBrvTab.Visible = false;
		this.btnLoadAllFromBrvTab.Click += new System.EventHandler(btnLoadAllFromBrvTab_Click);
		((ScrollableBase)this.grdMultiCurrency).BackColor = System.Drawing.Color.White;
		grdMultiCurrency_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMultiCurrency.DesignTimeLayout = grdMultiCurrency_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Dock = System.Windows.Forms.DockStyle.Left;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMultiCurrency.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Name = "grdMultiCurrency";
		((ScrollableBase)this.grdMultiCurrency).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Size = new System.Drawing.Size(338, 160);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabIndex = 60;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabStop = false;
		this.grdMultiCurrency.TotalRow = (InheritableBoolean)1;
		this.grdMultiCurrency.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMultiCurrency).VisualStyle = (VisualStyle)4;
		this.grdMultiCurrency.SelectionChanged += new System.EventHandler(grdMultiCurrency_SelectionChanged);
		this.lblDrAcGlCurrency.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblDrAcGlCurrency.Location = new System.Drawing.Point(334, 66);
		this.lblDrAcGlCurrency.Name = "lblDrAcGlCurrency";
		this.lblDrAcGlCurrency.Size = new System.Drawing.Size(95, 27);
		this.lblDrAcGlCurrency.TabIndex = 572;
		this.lblDrAcGlCurrency.Text = "Debit Ac GLcy:";
		this.txtBaseFcyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyCode.Enabled = false;
		this.txtBaseFcyCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyCode.Location = new System.Drawing.Point(925, 34);
		this.txtBaseFcyCode.Name = "txtBaseFcyCode";
		this.txtBaseFcyCode.ShortcutsEnabled = false;
		this.txtBaseFcyCode.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyCode.TabIndex = 575;
		this.txtBaseFcyCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyCode.AutoSize = true;
		this.lblBaseFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyCode.Location = new System.Drawing.Point(925, 15);
		this.lblBaseFcyCode.Name = "lblBaseFcyCode";
		this.lblBaseFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblBaseFcyCode.TabIndex = 576;
		this.lblBaseFcyCode.Text = "Fcy Code";
		this.txtBaseFcyExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyExchangeRate.Location = new System.Drawing.Point(924, 78);
		this.txtBaseFcyExchangeRate.Name = "txtBaseFcyExchangeRate";
		this.txtBaseFcyExchangeRate.ShortcutsEnabled = false;
		this.txtBaseFcyExchangeRate.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyExchangeRate.TabIndex = 573;
		this.txtBaseFcyExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyExchangeRate.AutoSize = true;
		this.lblBaseFcyExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyExchangeRate.Location = new System.Drawing.Point(924, 60);
		this.lblBaseFcyExchangeRate.Name = "lblBaseFcyExchangeRate";
		this.lblBaseFcyExchangeRate.Size = new System.Drawing.Size(105, 15);
		this.lblBaseFcyExchangeRate.TabIndex = 574;
		this.lblBaseFcyExchangeRate.Text = "Fcy Exchange Rate";
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail);
		this.panel26.Controls.Add(this.label54);
		this.panel26.Location = new System.Drawing.Point(941, 0);
		this.panel26.Margin = new System.Windows.Forms.Padding(0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(112, 47);
		this.panel26.TabIndex = 5;
		this.cmbTcyCodeDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbTcyCodeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTcyCodeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTcyCodeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance149;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance167).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
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
		this.label54.Location = new System.Drawing.Point(2, 2);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(55, 15);
		this.label54.TabIndex = 12;
		this.label54.Text = "Tcy Code";
		this.panel25.Controls.Add(this.txtTcyExchangeRateDetail);
		this.panel25.Controls.Add(this.label53);
		this.panel25.Location = new System.Drawing.Point(1053, 0);
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
		this.txtTcyExchangeRateDetail.Leave += new System.EventHandler(txtTcyExchangeRateDetail_Leave);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(1, 3);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(105, 15);
		this.label53.TabIndex = 4;
		this.label53.Text = "Tcy Exchange Rate";
		this.btnCreateUpdateMultiCurrency.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCreateUpdateMultiCurrency.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCreateUpdateMultiCurrency.Image = (System.Drawing.Image)resources.GetObject("btnCreateUpdateMultiCurrency.Image");
		this.btnCreateUpdateMultiCurrency.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCreateUpdateMultiCurrency.Name = "btnCreateUpdateMultiCurrency";
		this.btnCreateUpdateMultiCurrency.Size = new System.Drawing.Size(127, 24);
		this.btnCreateUpdateMultiCurrency.Text = "MultiCurrencyInfo";
		this.btnCreateUpdateMultiCurrency.Click += new System.EventHandler(btnCreateUpdateMultiCurrency_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1452, 684);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ReceiptsVoucherNew";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ReceiptsVoucherNew_FormClosing);
		base.Load += new System.EventHandler(AcfrmPaymentVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ReceiptsVoucherNew_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWithHoldingAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupboxdetail.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		this.PaymentTypePanel.ResumeLayout(false);
		this.PaymentTypePanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentType).EndInit();
		this.CreditAcPanel.ResumeLayout(false);
		this.CreditAcPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combactitle).EndInit();
		this.SubsidiaryAcPanel.ResumeLayout(false);
		this.SubsidiaryAcPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSubsidiaryAccount).EndInit();
		this.JobLotPanel.ResumeLayout(false);
		this.JobLotPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		this.RemarksPanel.ResumeLayout(false);
		this.RemarksPanel.PerformLayout();
		this.FcyAmountPanel.ResumeLayout(false);
		this.FcyAmountPanel.PerformLayout();
		this.AmountPanel.ResumeLayout(false);
		this.AmountPanel.PerformLayout();
		this.ReferenceAccountPanel.ResumeLayout(false);
		this.ReferenceAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).EndInit();
		this.BranchNamePanel.ResumeLayout(false);
		this.BranchNamePanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		this.CostCenterPanel.ResumeLayout(false);
		this.CostCenterPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		this.ChequeDetailPanel.ResumeLayout(false);
		this.ChequeDetailPanel.PerformLayout();
		this.ButtonsPanel.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combvtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgainstAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.CrvTab.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel20.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.VouchersInfoBoxCash.ResumeLayout(false);
		this.VouchersInfoBoxCash.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitleCrv).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusCrv).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.BrvTab.ResumeLayout(false);
		this.BrvTab.PerformLayout();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.BankVoucherDetailGrid).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.BankVoucherGrid).EndInit();
		this.panel16.ResumeLayout(false);
		this.VoucherInfoBoxBank.ResumeLayout(false);
		this.VoucherInfoBoxBank.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleBrvHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatusBrv).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).EndInit();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTcyCodeDetail).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		base.ResumeLayout(false);
	}
}
