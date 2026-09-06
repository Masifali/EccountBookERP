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
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition.VouchersWithTax;

public class ContraVoucher : Form
{
	public bool UpdateMode;

	private bool AutoRemarksForPaymentThroughBank;

	private bool MultiCurrencyFeatureVisibilty;

	private bool AutoRemarksIncludeHeaderRemarks;

	private bool AutoRemarksIncludeChqDateAndNum;

	private bool AutoRemarksIncludeChqNumber;

	private bool AutoRemarksIncludePayeeTitle;

	private bool AutoRemarksIncludeDetailRemarks;

	private bool BranchFeature;

	private bool PreventNegativeBalanceEntry;

	private bool DisplayWarningforNegativeBalance;

	private bool DisableBothNegativeBalanceRestrictions;

	private bool IsBookingOffice;

	private int BaseCurrency;

	private int RecId;

	private int UpdateDetailIndex = -1;

	private double BaseRate;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtAccount = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private DataTable dtcoalst = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtCurrencies = new DataTable();

	private DataTable dtVoucherDetailMultiCurrency = new DataTable();

	private bool FormInitialized;

	private bool CheqBookEnabled;

	private bool MultiCurrencyGridWorking;

	private Dictionary<string, int> ChequNosInGridFromChequebook = new Dictionary<string, int>();

	public UserAccount UserAccount = new UserAccount();

	private DataTable table = new DataTable();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private ToolStripButton btnNew;

	private ToolStripButton btnEdit;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnDe;

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

	private TabPage History;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private new ToolStripButton Update;

	private ToolStripButton Save;

	private ToolStripButton Print;

	private ToolStripButton toolStripButton10;

	private Label PaymentVoucherLable;

	private GroupBox groupBox2;

	private Label label2;

	private Label label15;

	private DateTimePicker voucherdatetime;

	private GroupBox groupboxdetail;

	private TextBox txtremarks;

	private TextBox txtamount;

	private Label lblDebitAmount;

	private Label label19;

	private Label label11;

	private Button Add;

	private Panel panel6;

	private Panel panel8;

	private Panel panel11;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbCreditAccount;

	private TextBox txtvoucherno;

	internal UltraCombo CmbDebitAccount;

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

	private CheckBox ChkBox;

	private Label label3;

	private ComboBox CmbPaymentType;

	internal GridEX grdDetail;

	internal UltraCombo CmbAgainstAc;

	private Label label13;

	private Label label22;

	internal UltraCombo CmbJobLot;

	private ToolStripButton btnDelete;

	private TextBox txtTaxPercent;

	private Label label21;

	internal UltraCombo CmbWithHoldingAc;

	private Label label12;

	private CheckBox ChkBoxWthHolding;

	private TextBox txtTotalAmount;

	private Label label9;

	private TextBox txtTaxAmount;

	private Label label8;

	private TextBox txtValue;

	private Label label4;

	private ToolStripButton btnRefresh;

	private Label lblbalancedetail;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnShortCutKey;

	private Label lblFcyAmount;

	private TextBox txtFcyAmount;

	private TextBox txtExchangeRate;

	private Label lblExchangeRate;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

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

	private Panel panel9;

	private Panel panel12;

	private Panel panel13;

	private Panel panel18;

	private Label label28;

	private RadioButton rdconapproved;

	private RadioButton rdconmodify;

	private RadioButton rdcondoc;

	private RadioButton rdconentry;

	internal UltraCombo cmbBranchName;

	private Label labelbranchname;

	private TextBox txtremarksmain;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel DebitAccountPanel;

	private Panel JobLotPanel;

	private Panel RemarksPanel;

	private Panel DebitAmountPanel;

	private Panel CostCenterPanel;

	internal UltraCombo CmbCostCenter;

	private Label label39;

	private Panel BranchPanel;

	private Panel panel14;

	private RadioButton rdbtnAccountCode;

	private RadioButton rdbtnAccountName;

	private Panel panel5;

	private TextBox txtFcyAmountDetail;

	private Label label29;

	private Panel ReferenceAccountPanel;

	internal UltraCombo CmbReferenceAccount;

	private Label label30;

	private RadioButton rdBtnRefAcByCode;

	private RadioButton rdBtnRefAcByTitle;

	private CheckBox chkCustomAccounts;

	private Panel ChequeDetailPanel;

	private DateTimePicker CheqDate;

	private Label lblCheqDate;

	private TextBox txtPayTitle;

	private Label lblCheqNo;

	private Label lblPayTitle;

	private TextBox txtbalance;

	private Label label6;

	internal UltraCombo CmbCheqNo;

	private Label labelLocationType;

	internal UltraCombo cmbLocationType;

	private Panel panel26;

	internal UltraCombo cmbTcyCodeDetail;

	private Label label54;

	private Panel panel25;

	private TextBox txtTcyExchangeRateDetail;

	private Label label53;

	private TextBox txtBaseFcyCode;

	private Label lblBaseFcyCode;

	private TextBox txtBaseFcyExchangeRate;

	private Label lblBaseFcyExchangeRate;

	private Label lblCrAcGlCurrency;

	private Panel panel17;

	internal GridEX grdMultiCurrency;

	private ToolStripButton btnCreateUpdateMultiCurrency;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	public ContraVoucher(UserAccount puserAccount)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private bool validationDetai()
	{
		if (((UltraGridBase)CmbDebitAccount).ActiveRow == null || Conversion.ToInt(CmbDebitAccount.Value) == 0)
		{
			MessageBox.Show("Debit Account Field Required");
			CmbDebitAccount.Focus();
			return false;
		}
		if (MultiCurrencyGridWorking && Conversion.ToInt(CmbDebitAccount.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[6].Value) == 0)
		{
			MessageBox.Show("Debit Account Currency not set Please Check");
			CmbDebitAccount.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field Required");
			CmbJobLot.Focus();
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
		if (Conversion.ToDouble((object)txtamount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount Field Required");
			txtamount.Focus();
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

	private void Add_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (validationDetai())
			{
				table.Rows.Add(CmbPaymentType.Text, ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[3].Value, ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value, CmbDebitAccount.Value, MultiCurrencyGridWorking ? Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[6].Value) : 0, MultiCurrencyGridWorking ? Conversion.ToString(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[7].Value) : "", CmbJobLot.Value, txtremarks.Text, cmbTcyCodeDetail.Value, ((Control)(object)cmbTcyCodeDetail).Text, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), Conversion.ToDouble((object)txtFcyAmountDetail.Text), txtamount.Text, Conversion.ToInt(CmbReferenceAccount.Value), (Conversion.ToInt(cmbBranchName.Value) > 0) ? Conversion.ToInt(cmbBranchName.Value) : 0, (Conversion.ToInt(CmbCostCenter.Value) > 0) ? Conversion.ToInt(CmbCostCenter.Value) : 0, Conversion.ToInt(CmbCheqNo.Value), Conversion.ToDateTime((object)CheqDate.Value), Conversion.ToString((object)((Control)(object)CmbCheqNo).Text), Conversion.ToString((object)txtPayTitle.Text));
				lblbalancedetail.Visible = false;
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				CmbDebitAccount.Focus();
				txtremarks.Clear();
				txtamount.Clear();
				txtFcyAmountDetail.Clear();
				((Control)(object)CmbDebitAccount).Text = string.Empty;
				CalculateTotalInformation();
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

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (validationDetai())
			{
				if (Conversion.CheckDateTimeNull(CheqDate.Value))
				{
					CheqDate.Value = DateTime.Now;
				}
				table.Rows[UpdateDetailIndex]["PaymentType"] = Conversion.ToInt((object)CmbPaymentType.Text);
				table.Rows[UpdateDetailIndex]["AccountTypeId"] = ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[3].Value;
				table.Rows[UpdateDetailIndex]["AccountCode"] = ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value;
				table.Rows[UpdateDetailIndex]["AccountId"] = CmbDebitAccount.Value;
				table.Rows[UpdateDetailIndex]["GlCurrencyId"] = ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[6].Value;
				table.Rows[UpdateDetailIndex]["GlCurrency"] = ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[7].Value;
				table.Rows[UpdateDetailIndex]["JobLotId"] = CmbJobLot.Value;
				table.Rows[UpdateDetailIndex]["Remarks"] = txtremarks.Text;
				table.Rows[UpdateDetailIndex]["TcyCodeId"] = cmbTcyCodeDetail.Value;
				table.Rows[UpdateDetailIndex]["TcyCode"] = ((Control)(object)cmbTcyCodeDetail).Text;
				table.Rows[UpdateDetailIndex]["TcyExchangeRate"] = txtTcyExchangeRateDetail.Text;
				table.Rows[UpdateDetailIndex]["FcyAmount"] = txtFcyAmountDetail.Text;
				table.Rows[UpdateDetailIndex]["Amount"] = txtamount.Text;
				table.Rows[UpdateDetailIndex]["ReferenceAccountId"] = Conversion.ToInt(CmbReferenceAccount.Value);
				table.Rows[UpdateDetailIndex]["BranchId"] = Conversion.ToInt(cmbBranchName.Value);
				table.Rows[UpdateDetailIndex]["CostCenterId"] = Conversion.ToInt(CmbCostCenter.Value);
				table.Rows[UpdateDetailIndex]["ChequeId"] = CmbCheqNo.Value;
				table.Rows[UpdateDetailIndex]["ChequeDate"] = Conversion.ToDateTime((object)CheqDate.Value).ToString("dd-MMM-yy");
				table.Rows[UpdateDetailIndex]["ChequeNo"] = ((Control)(object)CmbCheqNo).Text;
				table.Rows[UpdateDetailIndex]["PayTitle"] = txtPayTitle.Text;
				grd.DataSource = table;
				Add.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				txtremarks.Clear();
				txtamount.Clear();
				txtFcyAmountDetail.Clear();
				((Control)(object)CmbDebitAccount).Text = string.Empty;
				((Control)(object)CmbCheqNo).Text = string.Empty;
				CalculateTotalInformation();
				CmbDebitAccount.Focus();
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

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			UpdateDetailIndex = -1;
			txtremarks.Clear();
			txtamount.Clear();
			txtFcyAmountDetail.Clear();
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			CmbDebitAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_13e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ee: Expected O, but got Unknown
		//IL_1331: Unknown result type (might be due to invalid IL or missing references)
		//IL_1338: Expected O, but got Unknown
		//IL_0dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e05: Expected O, but got Unknown
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
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (RecId > 0)
			{
				vh.Id = RecId;
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
			vh.BaseDocumentTypeId = 3;
			vh.Type = (VoucherType)3;
			vh.DocumentTypeId = 10;
			vh.VoucherCode = Conversion.ToInt((object)txtvoucherno.Text.Trim());
			vh.ProjectId = Conversion.ToInt((object)CmbProjectId.Value.ToString());
			vh.VoucherDate = voucherdatetime.Value;
			vh.RefAccountId = Conversion.ToInt(CmbCreditAccount.Value);
			vh.AgainstAccountId = Conversion.ToInt(CmbCreditAccount.Value);
			vh.Remarks = txtremarksmain.Text.Trim();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.EntryUser = UserAccount.ID;
			vh.ModifyUser = UserAccount.ID;
			vh.IncludeWHT = false;
			vh.MultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
			vh.ExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			vh.FcAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
			vh.CustomAccounts = chkCustomAccounts.Checked;
			vh.voucherDetailList = new List<VoucherDetail>();
			if (grd.RowCount > 0)
			{
				string AcTitle = "";
				int LineId = 1;
				double VoucherAmt = 0.0;
				rows2 = grd.GetRows();
				foreach (GridEXRow r in rows2)
				{
					string RemarksAuto = "";
					string Remarks = "";
					VoucherDetail vd = new VoucherDetail();
					if (AutoRemarksForPaymentThroughBank)
					{
						for (int i = 0; i < dtcoalst.Rows.Count; i++)
						{
							if (Conversion.ToInt(dtcoalst.Rows[i]["Id"]) == Conversion.ToInt(r.Cells["AccountId"].Value))
							{
								AcTitle = Conversion.ToString(dtcoalst.Rows[i]["AccountTitle"]);
							}
						}
					}
					vd.LineId = LineId;
					vd.SortNo = r.RowIndex + 1;
					vd.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					vd.AgainstAccountId = Conversion.ToInt(CmbCreditAccount.Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					if (((UltraGridBase)CmbCreditAccount).ActiveRow != null && Conversion.ToInt(CmbCreditAccount.Value) != 0 && Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells["AccountTypeId"].Value) == 15)
					{
						vd.InvoiceNoRefId = Conversion.ToInt(r.Cells["ChequeId"].Value);
						vd.DCheqDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
						vd.CheqNoDetail = Conversion.ToString(r.Cells["ChequeNo"].Value);
						vd.PayeeTitle = Conversion.ToString(r.Cells["PayTitle"].Value);
						if (string.IsNullOrWhiteSpace(vh.ChequeNo) || vh.CheqId == 0)
						{
							vh.CheqId = Conversion.ToInt(r.Cells["ChequeId"].Value);
							vh.ChequeDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
							vh.ChequeNo = Conversion.ToString(r.Cells["ChequeNo"].Value);
							vh.PayTitle = Conversion.ToString(r.Cells["PayTitle"].Value);
						}
					}
					if (AutoRemarksForPaymentThroughBank)
					{
						RemarksAuto = ((AutoRemarksIncludeChqDateAndNum && AutoRemarksIncludePayeeTitle) ? (" CHEQUE DATE: " + Conversion.ToDateTime((object)vd.DCheqDate).ToShortDateString() + " CHEQUE NO: " + vd.CheqNoDetail + " PayTitle: " + vd.PayeeTitle + " Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " TRANSFER TO: " + AcTitle) : (AutoRemarksIncludeChqDateAndNum ? (" CHEQUE DATE: " + Conversion.ToDateTime((object)vd.DCheqDate).ToShortDateString() + " CHEQUE NO: " + vd.CheqNoDetail + " Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " TRANSFER TO: " + AcTitle) : ((AutoRemarksIncludeChqNumber && AutoRemarksIncludePayeeTitle) ? (" CHEQUE NO: " + vd.CheqNoDetail + " PayTitle: " + vd.PayeeTitle + " Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " TRANSFER TO: " + AcTitle) : ((!AutoRemarksIncludePayeeTitle) ? (" Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " TRANSFER TO: " + AcTitle) : (" PayTitle: " + vd.PayeeTitle + " Credit Account: " + ((Control)(object)CmbCreditAccount).Text.Trim() + " TRANSFER TO: " + AcTitle)))));
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
							vh.RemarksOtherLingo = ((vh.Remarks != "") ? vh.Remarks : null);
							vh.Remarks = RemarksAuto;
						}
					}
					else if (vd.CheqNoDetail != string.Empty)
					{
						Remarks = "CHEQUE NO: " + vd.CheqNoDetail + ", " + Conversion.ToString(r.Cells["Remarks"].Value);
						vd.Comments = Remarks;
						vd.CommentsOtherLingo = Conversion.ToString(r.Cells["Remarks"].Value);
					}
					else
					{
						vd.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.CommentsOtherLingo = Conversion.ToString(r.Cells["Remarks"].Value);
					}
					vd.DebitAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
					if (vd.DebitAmount == 0.0)
					{
						throw new Exception("Amount is Required in Row# " + (r.RowIndex + 1));
					}
					VoucherAmt += vd.DebitAmount;
					vd.IsTaxable = "False";
					vd.DMultiCurrencyId = Conversion.ToInt(r.Cells["TcyCodeId"].Value);
					vd.DExchangeCurrencyRate = Conversion.ToDouble(r.Cells["TcyExchangeRate"].Value);
					vd.DCurrencyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
					vd.BranchesId = (BranchFeature ? Conversion.ToInt(r.Cells["BranchId"].Value) : UserAccount.BranchesId);
					vd.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
					vd.ReferenceAccountId = Conversion.ToInt(r.Cells["ReferenceAccountId"].Value);
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
					vd.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
					vh.voucherDetailList.Add(vd);
					LineId++;
					VoucherDetail vd2 = new VoucherDetail();
					vd2.LineId = LineId;
					vd2.SortNo = r.RowIndex + 1;
					vd2.AccountId = Conversion.ToInt(CmbCreditAccount.Value);
					vd2.AgainstAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					vh.AgainstAccountId = vd2.AgainstAccountId;
					vd2.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					if (AutoRemarksForPaymentThroughBank)
					{
						vd2.Comments = RemarksAuto;
						vd2.CommentsOtherLingo = Conversion.ToString(r.Cells["Remarks"].Value);
					}
					else if (count == 1 && vh.Remarks.Trim() != null && vh.Remarks.Trim() != string.Empty)
					{
						vd2.Comments = vh.Remarks.Trim();
						vd2.CommentsOtherLingo = vd.CommentsOtherLingo;
					}
					else
					{
						vd2.Comments = vd.Comments;
						vd2.CommentsOtherLingo = Conversion.ToString(r.Cells["Remarks"].Value);
					}
					vd2.CreditAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd2.IsTaxable = "False";
					vd2.DMultiCurrencyId = Conversion.ToInt(r.Cells["TcyCodeId"].Value);
					vd2.DExchangeCurrencyRate = Conversion.ToDouble(r.Cells["TcyExchangeRate"].Value);
					vd2.DCurrencyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
					vd2.BranchesId = (BranchFeature ? Conversion.ToInt(r.Cells["BranchId"].Value) : UserAccount.BranchesId);
					vd2.ReferenceAccountId = Conversion.ToInt(r.Cells["ReferenceAccountId"].Value);
					if (count == 1 && vh.Remarks == string.Empty)
					{
						vh.Remarks = vd2.Comments;
					}
					vd2.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
					if (MultiCurrencyGridWorking)
					{
						vd2.ThirdCurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[6].Value);
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
				if (string.IsNullOrEmpty(AttachmentsValues) || AT.lst.Count > 0)
				{
					AttachmentsValues = "";
				}
				vh.AttachmentsValues = AttachmentsValues;
				vh.CustomAttachmentsValues = CustomAttachmentsValues;
				if (!DisableBothNegativeBalanceRestrictions)
				{
					double ClosingBalance = CommonServices.GetAccountBalance(vh.RefAccountId, Conversion.ToDateTime((object)voucherdatetime.Value));
					ClosingBalance = Math.Round(ClosingBalance);
					if (VoucherAmt > ClosingBalance)
					{
						if (PreventNegativeBalanceEntry)
						{
							MessageBox.Show($"Debit Amount cannot be greater than Account Balance.\nAccount '{Conversion.ToString((object)((Control)(object)CmbCreditAccount).Text)}' Balance is {ClosingBalance} and Total Debit Amount is {VoucherAmt}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
						if (DisplayWarningforNegativeBalance && MessageBox.Show($"Debit Amount cannot be greater than Account Balance.\nAccount '{Conversion.ToString((object)((Control)(object)CmbCreditAccount).Text)}' Balance is {ClosingBalance} and Total Debit Amount is {VoucherAmt}.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
						{
							return;
						}
					}
				}
				if (VoucherHead.VoucherExistWithSameAmountInSameDate(vh))
				{
					vh.VoucherAmount = VoucherAmt;
					int success = VoucherHead.Save(vh);
					if (RecId == 0)
					{
						MessageBox.Show("Voucher Save Successfully...[" + vh.VoucherCode + "]");
					}
					else
					{
						MessageBox.Show("Voucher Update Successfully...[" + vh.VoucherCode + "]");
						FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, vh.DocumentTypeId, base.Name);
					}
					Reset();
					if (ChkBox.Checked)
					{
						CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(success);
					}
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

	private bool FormValidation()
	{
		if (Conversion.ToInt(CmbProjectId.Value) == 0)
		{
			MessageBox.Show("Cost Center Field is Required");
			CmbProjectId.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAccount).ActiveRow == null || Conversion.ToInt(CmbCreditAccount.Value) == 0)
		{
			MessageBox.Show("Credit Account Field is Required");
			CmbCreditAccount.Focus();
			return false;
		}
		if (MultiCurrencyGridWorking && Conversion.ToInt(CmbCreditAccount.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[6].Value) == 0)
		{
			MessageBox.Show("Credit Account Currency not set Please Check");
			CmbCreditAccount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLocationType).ActiveRow == null || Conversion.ToInt(cmbLocationType.Value) == 0)
		{
			MessageBox.Show("Location Type Field is Required");
			cmbLocationType.Focus();
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
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 600);
			string ScreenName = Conversion.ToString(base.Tag);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			Save.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			Update.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			txtamount.Tag = "DebitAmount";
			txtFcyAmountDetail.Tag = "FcyAmount";
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
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "CheqBook Enabled");
			CheqBookEnabled = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			MultiCurrencyGridWorking = CommonServices.GetERPFeatureById(6);
			if (table.Columns.Count == 0)
			{
				table.Columns.Add("PaymentType");
				table.Columns.Add("AccountTypeId", typeof(int));
				table.Columns.Add("AccountCode");
				table.Columns.Add("AccountId");
				table.Columns.Add("GlCurrencyId", typeof(int));
				table.Columns.Add("GlCurrency");
				table.Columns.Add("JobLotId");
				table.Columns.Add("Remarks");
				table.Columns.Add("TcyCodeId", typeof(int));
				table.Columns.Add("TcyCode");
				table.Columns.Add("TcyExchangeRate", typeof(double));
				table.Columns.Add("FcyAmount", typeof(double));
				table.Columns.Add("Amount", typeof(double));
				table.Columns.Add("ReferenceAccountId", typeof(int));
				table.Columns.Add("BranchId", typeof(int));
				table.Columns.Add("CostCenterId", typeof(int));
				table.Columns.Add("ChequeId", typeof(int));
				table.Columns.Add("ChequeDate", typeof(DateTime));
				table.Columns.Add("ChequeNo", typeof(string));
				table.Columns.Add("PayTitle", typeof(string));
			}
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
				panel17.Visible = false;
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
				dtAccount.Columns.Add("AccountTypeId", typeof(int));
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
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchesFill();
			LocationType();
			CombProjectFill();
			VoucherNofill();
			CurrencyFill();
			MultiCurrencyFeature();
			combojoblotfill();
			AccountsComboBind();
			DetailAccounts();
			BindReferenceAccounts();
			CostCenterFill();
			Save.Visible = true;
			Update.Visible = false;
			cmbdoctype.SelectedIndex = 0;
			CmbPaymentType.SelectedIndex = 0;
			ChkBox.Checked = true;
			voucherdatetime.Focus();
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			DefaultConfigurations();
			if (!BranchFeature)
			{
				BranchPanel.Visible = false;
				labelbranchname.Visible = false;
				((Control)(object)cmbBranchName).Visible = false;
			}
			ApplicationsAllocateToCompany obj2 = clsGlobalVariables.GetApplicationsAllocateToCompanyList.Find((ApplicationsAllocateToCompany row) => row.AppId == 5 && row.CompanyId == UserAccount.CompanyId);
			IsBookingOffice = obj2 != null && obj2.IsActive;
			if (!IsBookingOffice)
			{
				CostCenterPanel.Visible = false;
			}
			if (!IsBookingOffice && !BranchFeature)
			{
				RemarksPanel.Size = new Size(282, 47);
				txtremarks.Size = new Size(282, 23);
			}
			else
			{
				RemarksPanel.Size = new Size(320, 47);
				txtremarks.Size = new Size(320, 23);
			}
			StatusFillForHistory();
			ComboBindForHistory();
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			voucherdatetime.Focus();
			FormInitialized = true;
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
				DocumentTypeIds = 10.ToString()
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

	private void MultiCurrencyFeature()
	{
		try
		{
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
			throw new Exception(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		try
		{
			dtJobLot.Rows.Clear();
			int id = Conversion.ToInt(CmbJobLot.Value);
			dtJobLot = CommonServices.JobLotGetAllService();
			if (dtJobLot.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtJobLot, CmbJobLot, "Id", "JobLotDescription", "JobLot", true);
			if (id > 0)
			{
				if (dtJobLot.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbJobLot.Value = RecId;
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

	private void VoucherNofill()
	{
		try
		{
			txtvoucherno.Text = Conversion.ToString((object)CommonServices.GenerateVoucherCode(10));
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
			dtcoalst.Rows.Clear();
			int AccountId = Conversion.ToInt(CmbCreditAccount.Value);
			dtcoalst = CommonServices.Accounts_GetAccountTitleByAccountTypeIds("2,15");
			if (dtcoalst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("AccountCode");
			dt.Columns.Add("AccountTypeId", typeof(int));
			dt.Columns.Add("ParentAccountTitle");
			dt.Columns.Add("AccountClass");
			dt.Columns.Add("CurrencyId");
			dt.Columns.Add("CurrencyCode");
			for (int i = 0; i < dtcoalst.Rows.Count; i++)
			{
				dt.Rows.Add(dtcoalst.Rows[i]["Id"], dtcoalst.Rows[i]["AccountTitle"], dtcoalst.Rows[i]["AccountCode"], dtcoalst.Rows[i]["AccountTypeId"], dtcoalst.Rows[i]["ParentAccountTitle"], dtcoalst.Rows[i]["AccountClass"], dtcoalst.Rows[i]["CurrencyId"], dtcoalst.Rows[i]["CurrencyCode"]);
			}
			DDL.BindDDL(dt, CmbCreditAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			((UltraGridBase)CmbCreditAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			((UltraGridBase)CmbCreditAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			((UltraGridBase)CmbCreditAccount).DisplayLayout.Bands[0].Columns[6].Hidden = true;
			if (AccountId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == AccountId))
				{
					CmbCreditAccount.Value = AccountId;
					combcreditac_Leave(null, null);
				}
				else
				{
					((Control)(object)CmbCreditAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailAccounts()
	{
		try
		{
			dtAccount.Rows.Clear();
			int id = Conversion.ToInt(CmbDebitAccount.Value);
			if (dtcoalst.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcoalst.Rows.Count; i++)
			{
				dtAccount.Rows.Add(dtcoalst.Rows[i]["Id"], dtcoalst.Rows[i]["AccountTitle"], dtcoalst.Rows[i]["AccountCode"], dtcoalst.Rows[i]["AccountTypeId"], dtcoalst.Rows[i]["ParentAccountTitle"], dtcoalst.Rows[i]["AccountClass"], dtcoalst.Rows[i]["CurrencyId"], dtcoalst.Rows[i]["CurrencyCode"]);
			}
			if (dtAccount.Rows.Count > 0)
			{
				if (rdbtnAccountName.Checked)
				{
					DDL.BindDDL(dtAccount, CmbDebitAccount, "Id", "AccountTitle", "Debit Account", ZeroIndex: true);
				}
				else if (rdbtnAccountCode.Checked)
				{
					DDL.BindDDL(dtAccount, CmbDebitAccount, "Id", "AccountCode", "Debit Ac Code", ZeroIndex: true);
				}
				((UltraGridBase)CmbDebitAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbDebitAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbDebitAccount).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				CommonServices.SetComboValue(CmbDebitAccount, "Id", id, dtAccount);
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
					if (rdBtnRefAcByTitle.Checked)
					{
						DDL.BindDDL(dt, CmbReferenceAccount, "Id", "AccountTitle", "Reference Account", ZeroIndex: false);
					}
					else
					{
						DDL.BindDDL(dt, CmbReferenceAccount, "Id", "AccountCode", "Reference Ac Code", ZeroIndex: false);
					}
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
				RefAccountId = Conversion.ToInt(CmbCreditAccount.Value)
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
				RefAccountId = Conversion.ToInt(CmbDebitAccount.Value)
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

	private void combactitle_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbDebitAccount.Value) > 0)
			{
				DetailAccountCurrencyCheck();
			}
			lblbalancedetail.Visible = true;
			DetailAccountCurrentBalance();
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
			dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
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

	private void ChequeBookBind()
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
			((Control)(object)CmbCheqNo).Text = string.Empty;
			DataTable dtCheq = CheqBookHeader.OutstandingCheqNo(new CheqBookHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BankId = Conversion.ToInt(CmbCreditAccount.Value)
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
					DDL.BindDDL(dt, CmbCheqNo, "Id", "CheqNo", "CheqNo", ZeroIndex: false);
					((UltraGridBase)CmbCheqNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			int CurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[6].Value);
			string CurrencyCode = Conversion.ToString(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[7].Value);
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
			if (MultiCurrencyGridWorking && Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[6].Value) == 0 && UpdateDetailIndex == -1)
			{
				((Control)(object)CmbDebitAccount).Text = "";
				CmbDebitAccount.Focus();
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
			BaseCurrencyCheck();
			if (Conversion.ToInt(CmbCreditAccount.Value) > 0)
			{
				CreditAccountCurrencyCheck();
			}
			AccountCurrentBalance();
			combcreditac_TextChanged(null, null);
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

	private void grdCombosFill()
	{
		try
		{
			grd.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["AccountId"].HasValueList = true;
			grd.RootTable.Columns["AccountId"].LimitToList = true;
			grd.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].Caption = "Account Title";
			grd.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccount.DefaultView, "Id", "AccountTitle");
			grd.RootTable.Columns["BranchId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["BranchId"].HasValueList = true;
			grd.RootTable.Columns["BranchId"].LimitToList = true;
			grd.RootTable.Columns["BranchId"].EditType = (EditType)4;
			grd.RootTable.Columns["BranchId"].Caption = "Branch Name";
			grd.RootTable.Columns["BranchId"].ValueList.PopulateValueList((IEnumerable)dtBranch.DefaultView, "BranchId", "BranchName");
			grd.RootTable.Columns["BranchId"].Visible = BranchFeature;
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
			DataTable dtRef = (DataTable)((UltraGridBase)CmbReferenceAccount).DataSource;
			if (dtRef != null && dtRef.Rows.Count > 0)
			{
				grd.RootTable.Columns["ReferenceAccountId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["ReferenceAccountId"].HasValueList = true;
				grd.RootTable.Columns["ReferenceAccountId"].LimitToList = true;
				grd.RootTable.Columns["ReferenceAccountId"].EditType = (EditType)4;
				grd.RootTable.Columns["ReferenceAccountId"].Caption = "Reference Account";
				grd.RootTable.Columns["ReferenceAccountId"].ValueList.PopulateValueList((IEnumerable)dtRef.DefaultView, "Id", "AccountTitle");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdCombosFill();
			grd.RootTable.Columns["ChequeId"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["GlCurrencyId"].EditType = (EditType)0;
			grd.RootTable.Columns["GlCurrency"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyCodeId"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyCode"].EditType = (EditType)0;
			grd.RootTable.Columns["TcyExchangeRate"].EditType = (EditType)0;
			grd.RootTable.Columns["CostCenterId"].Visible = IsBookingOffice;
			grd.RootTable.Columns["ChequeId"].Visible = false;
			grd.RootTable.Columns["PaymentType"].Visible = false;
			grd.RootTable.Columns["AccountTypeId"].Visible = false;
			grd.RootTable.Columns["TcyCodeId"].Visible = false;
			grd.RootTable.Columns["GlCurrencyId"].Visible = false;
			grd.RootTable.Columns["GlCurrency"].Visible = MultiCurrencyGridWorking;
			grd.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Amount"].Caption = "Debit Amount";
			grd.RootTable.Columns["FcyAmount"].Caption = "Tcy Amount";
			grd.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["AccountId"].Width = 200;
			grd.RootTable.Columns["ReferenceAccountId"].Width = 200;
			grd.RootTable.Columns["Remarks"].Width = 250;
			grd.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 10;
			grd.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant + 10;
			grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["TcyExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
			grd.RootTable.Columns["TcyExchangeRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TcyExchangeRate"].FormatString = "#,##0.###";
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
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
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
			Reset();
			RecId = Conversion.ToInt((object)ID);
			VoucherHead vh = VoucherHead.GetByID(RecId);
			if (vh != null)
			{
				Save.Visible = false;
				Update.Visible = true;
				tabControl1.SelectedIndex = 0;
				voucherdatetime.Value = vh.VoucherDate;
				CmbCreditAccount.Value = vh.RefAccountId;
				chkCustomAccounts.Checked = vh.CustomAccounts;
				combcreditac_Leave(null, null);
				txtPayTitle.Text = Conversion.ToString((object)vh.PayTitle);
				TextBox textBox = txtremarksmain;
				string text2 = (txtremarksmain.Text = (AutoRemarksForPaymentThroughBank ? vh.RemarksOtherLingo : vh.Remarks));
				textBox.Text = text2;
				txtvoucherno.Text = Conversion.ToString((object)vh.VoucherCode);
				txtValue.Text = Conversion.ToString((object)vh.VoucherAmount);
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
				table.Clear();
				for (int i = 0; i < vh.voucherDetailList.Count; i++)
				{
					cmbLocationType.Value = ((vh.voucherDetailList.Count > 0) ? Conversion.ToInt((object)vh.voucherDetailList[0].LocationTypeId) : 0);
					if (vh.voucherDetailList[i].DebitAmount > 0.0)
					{
						table.Rows.Add("", vh.voucherDetailList[i].AccountTypeId, vh.voucherDetailList[i].AccountCode, vh.voucherDetailList[i].AccountId, vh.voucherDetailList[i].ThirdCurrencyId, vh.voucherDetailList[i].GlcyCode, vh.voucherDetailList[i].JobLotId, AutoRemarksForPaymentThroughBank ? vh.voucherDetailList[i].CommentsOtherLingo : ((vh.voucherDetailList[i].CommentsOtherLingo != null) ? vh.voucherDetailList[i].CommentsOtherLingo : vh.voucherDetailList[i].Comments), vh.voucherDetailList[i].DMultiCurrencyId, vh.voucherDetailList[i].TcyCode, vh.voucherDetailList[i].DExchangeCurrencyRate, vh.voucherDetailList[i].DCurrencyAmount, vh.voucherDetailList[i].DebitAmount, vh.voucherDetailList[i].ReferenceAccountId, vh.voucherDetailList[i].BranchesId, vh.voucherDetailList[i].CostCenterId, vh.voucherDetailList[i].InvoiceNoRefId, vh.voucherDetailList[i].DCheqDate, vh.voucherDetailList[i].CheqNoDetail, vh.voucherDetailList[i].PayeeTitle);
						if (vh.voucherDetailList[i].InvoiceNoRefId > 0 && !ChequNosInGridFromChequebook.ContainsKey(vh.voucherDetailList[i].CheqNoDetail))
						{
							ChequNosInGridFromChequebook.Add(vh.voucherDetailList[i].CheqNoDetail, vh.voucherDetailList[i].InvoiceNoRefId);
						}
					}
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				if (MultiCurrencyGridWorking && vh.voucherDetailList != null && vh.voucherDetailList.Count > 0)
				{
					txtBaseFcyCode.Tag = vh.voucherDetailList[0].BaseFcyId;
					txtBaseFcyCode.Text = Conversion.ToString((object)vh.voucherDetailList[0].BaseFcyCode);
					txtBaseFcyExchangeRate.Text = Conversion.ToString((object)vh.voucherDetailList[0].BaseFcyExchangeRate);
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			UpdateDetailIndex = -1;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			ChequNosInGridFromChequebook.Clear();
			table.Rows.Clear();
			UpdateMode = false;
			txtremarks.Text = string.Empty;
			txtamount.Text = string.Empty;
			txtPayTitle.Text = string.Empty;
			txtremarksmain.Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			txtValue.Clear();
			txtTaxAmount.Clear();
			txtTotalAmount.Clear();
			CmbPaymentType.SelectedIndex = 0;
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			((Control)(object)CmbCheqNo).Text = string.Empty;
			voucherdatetime.Focus();
			VoucherNofill();
			Save.Visible = true;
			Update.Visible = false;
			UpdateMode = false;
			grd.DataSource = null;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			txtbalance.Text = "0";
			lblbalancedetail.Visible = false;
			lblbalancedetail.Text = "0";
			lblCrAcGlCurrency.Text = "Credit Ac GLcy:";
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
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Delete")
			{
				item.Delete();
				grd.UpdateData();
				if (MultiCurrencyGridWorking)
				{
					CreateUpdateMultiCurrencyGridInfo();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill(int NoofRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.UserId = UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.AppId = UserAccount.AppId;
			vh.Ids = "10";
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = UserAccount.ID;
			}
			if (rdcondoc.Checked)
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
			else if (rdconentry.Checked)
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
			else if (rdconmodify.Checked)
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
			else if (rdconapproved.Checked)
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
			DataTable lst = VoucherHead.VoucherFormHistory(vh);
			if (lst.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id");
				dtHistory.Columns.Add("VoucherCode", typeof(int));
				dtHistory.Columns.Add("DocumentTypeId");
				dtHistory.Columns.Add("DocumentTypeCode", typeof(string));
				dtHistory.Columns.Add("VoucherDate", typeof(DateTime));
				dtHistory.Columns.Add("ManualBillNo");
				dtHistory.Columns.Add("AccountTypeId", typeof(int));
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
					dtHistory.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["ManualBillNo"], lst.Rows[i]["AccountTypeId"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["AgainstAccount"], lst.Rows[i]["Remarks"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["NoOfAttachments"]);
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["AccountTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
			DataGridHistory.RootTable.Columns["FcyCode"].Visible = false;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Visible = false;
			DataGridHistory.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			DataGridHistory.RootTable.Columns["AccountTitle"].Caption = "Credit Account";
			DataGridHistory.RootTable.Columns["AgainstAccount"].Caption = "Debit Account";
			DataGridHistory.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["VoucherAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["VoucherAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["VoucherCode"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
			DataGridHistory.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			DataGridHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 50;
			DataGridHistory.RootTable.Columns["AgainstAccount"].Width = Constants.InventoryConstants.AccountTitle + 30;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			DataGridHistory.RootTable.Columns["VoucherAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["CheqNo"].Width = Constants.InventoryConstants.StringConstant;
			DataGridHistory.RootTable.Columns["Attachment"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DataGridHistory.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			DataGridHistory.RootTable.Columns["VoucherCode"].Caption = "V.No";
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Caption = "V.Type";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "yyyy-MM-dd HH:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "yyyy-MM-dd HH:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "yyyy-MM-dd HH:mm tt";
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
			DataGridHistory.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
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
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "is Minus balance Allowed");
			PreventNegativeBalanceEntry = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DisplayWarningforNegativeBalance");
			DisplayWarningforNegativeBalance = Conversion.ToBool((object)((obj2 != null) ? obj2.ConfigKey : null));
			ConfigrationsAllocation obj3 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DisableBothNegativeBalanceRestrictions");
			DisableBothNegativeBalanceRestrictions = Conversion.ToBool((object)((obj3 != null) ? obj3.ConfigKey : null));
			ConfigrationsAllocation obj4 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "CheqBook Enabled");
			CheqBookEnabled = Conversion.ToBool((object)((obj4 != null) ? obj4.ConfigKey : null));
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
			if (e.Column.Key == "Edit")
			{
				int ID = int.Parse(DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(ID);
			}
			else if (e.Column.Key == "Print")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 10, base.Name);
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
				table.Columns.Add("GlCurrency");
				table.Columns.Add("Remarks");
				table.Columns.Add("TcyCode");
				table.Columns.Add("TcyExchangeRate", typeof(double));
				table.Columns.Add("DebitFcyAmount", typeof(double));
				table.Columns.Add("DebitAmount", typeof(double));
				table.Columns.Add("CreditFcyAmount", typeof(double));
				table.Columns.Add("CreditAmount", typeof(double));
				table.Columns.Add("ReferenceAccount");
				table.Columns.Add("BranchName");
				table.Columns.Add("CostCenter");
				table.Columns.Add("ChequeDate", typeof(DateTime));
				table.Columns.Add("ChequeNo", typeof(string));
				table.Columns.Add("PayTitle", typeof(string));
				table.Rows.Clear();
				List<VoucherDetail> sortingvoucherlist = byID.voucherDetailList.OrderBy((VoucherDetail v) => v.Id).ToList();
				for (int i = 0; i < sortingvoucherlist.Count; i++)
				{
					table.Rows.Add(sortingvoucherlist[i].AccountCode, sortingvoucherlist[i].AccountTitle, sortingvoucherlist[i].GlcyCode, sortingvoucherlist[i].Comments, sortingvoucherlist[i].TcyCode, sortingvoucherlist[i].DExchangeCurrencyRate, (sortingvoucherlist[i].DebitAmount > 0.0) ? sortingvoucherlist[i].DCurrencyAmount : 0.0, sortingvoucherlist[i].DebitAmount, (sortingvoucherlist[i].CreditAmount > 0.0) ? sortingvoucherlist[i].DCurrencyAmount : 0.0, sortingvoucherlist[i].CreditAmount, sortingvoucherlist[i].ReferenceAccount, sortingvoucherlist[i].BranchName, sortingvoucherlist[i].CostCenterName, sortingvoucherlist[i].DCheqDate, sortingvoucherlist[i].CheqNoDetail, sortingvoucherlist[i].PayeeTitle);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				int TypeId = Conversion.ToInt(item.Cells["AccountTypeId"].Value);
				grdDetail.RootTable.Columns["ChequeDate"].Visible = TypeId == 15;
				grdDetail.RootTable.Columns["ChequeNo"].Visible = TypeId == 15;
				grdDetail.RootTable.Columns["PayTitle"].Visible = TypeId == 15;
				grdDetail.RootTable.Columns["GlCurrency"].Visible = MultiCurrencyGridWorking;
				GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3);
				grdDetail.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
				grdDetail.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 50;
				grdDetail.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 10;
				grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
				grdDetail.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant;
				grdDetail.RootTable.Columns["TcyExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
				grdDetail.RootTable.Columns["DebitFcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["DebitAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["CreditFcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["CreditAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["ReferenceAccount"].Width = Constants.InventoryConstants.AccountTitle;
				grdDetail.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
				grdDetail.RootTable.Columns["CostCenter"].Width = Constants.InventoryConstants.StringConstant;
				grdDetail.RootTable.Columns["ChequeDate"].Width = Constants.InventoryConstants.DateConstant + 10;
				grdDetail.RootTable.Columns["ChequeNo"].Width = Constants.InventoryConstants.StringConstant;
				grdDetail.RootTable.Columns["PayTitle"].Width = Constants.InventoryConstants.StringConstant;
				grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature;
				grdDetail.RootTable.Columns["CostCenter"].Visible = IsBookingOffice;
				grdDetail.RootTable.Columns["DebitFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
				grdDetail.RootTable.Columns["CreditFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
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
				grdDetail.RootTable.Columns["DebitFcyAmount"].Caption = "Tcy Debit";
				grdDetail.RootTable.Columns["DebitAmount"].Caption = "Debit";
				grdDetail.RootTable.Columns["CreditFcyAmount"].Caption = "Tcy Credit";
				grdDetail.RootTable.Columns["CreditAmount"].Caption = "Credit";
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
		_ = tabControl1.SelectedIndex;
		_ = 1;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		HistoryFill(50);
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 10);
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			CurrencyFill();
			combojoblotfill();
			AccountsComboBind();
			DetailAccounts();
			DefaultConfigurations();
			grdCombosFill();
			LocationType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContraVoucher_KeyDown(object sender, KeyEventArgs e)
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
			if (Update.Visible && Update.Enabled && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
			{
				Update_Click(null, null);
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
					voucherdatetime.Focus();
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
			if (e.Control && e.KeyCode == Keys.P)
			{
				Print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				voucherdatetime.Focus();
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
					CmbDebitAccount.Focus();
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

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			int ID = int.Parse(DataGridHistory.CurrentRow.Cells[0].Value.ToString());
			ReadById(ID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcreditac_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int TypeId = 0;
			int Id = Conversion.ToInt(CmbCreditAccount.Value);
			if (((UltraGridBase)CmbCreditAccount).ActiveRow != null && Id > 0)
			{
				TypeId = Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells["AccountTypeId"].Value);
				if (TypeId == 15)
				{
					ChequeBookBind();
				}
				else
				{
					((Control)(object)CmbCheqNo).Text = string.Empty;
					((UltraGridBase)CmbCheqNo).DataSource = null;
				}
				ChequeDetailPanel.Visible = TypeId == 15;
			}
			else
			{
				((Control)(object)CmbCheqNo).Text = string.Empty;
				((UltraGridBase)CmbCheqNo).DataSource = null;
			}
			CmbCheqNo.LimitToList = CheqBookEnabled;
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

	private void ContraVoucher_FormClosing(object sender, FormClosingEventArgs e)
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

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null)
			{
				return;
			}
			UpdateDetailIndex = item.RowIndex;
			CmbPaymentType.Text = item.Cells["PaymentType"].Value.ToString();
			CmbDebitAccount.Value = Conversion.ToInt(item.Cells["AccountId"].Value);
			CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
			txtremarks.Text = item.Cells["Remarks"].Value.ToString();
			cmbTcyCodeDetail.Value = Conversion.ToInt(item.Cells["TcyCodeId"].Value);
			txtTcyExchangeRateDetail.Text = Conversion.ToDouble(item.Cells["TcyExchangeRate"].Value).ToString("#,##0.###");
			txtFcyAmountDetail.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString("#,##0.###");
			txtamount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString("#,##0.###");
			if (Conversion.ToInt(item.Cells["ReferenceAccountId"].Value) > 0)
			{
				CmbReferenceAccount.Value = Conversion.ToInt(item.Cells["ReferenceAccountId"].Value);
			}
			cmbBranchName.Value = Conversion.ToInt(item.Cells["BranchId"].Value);
			if (Conversion.ToInt(item.Cells["CostCenterId"].Value) > 0)
			{
				CmbCostCenter.Value = Conversion.ToInt(item.Cells["CostCenterId"].Value);
			}
			if (Conversion.ToInt(item.Cells["ChequeId"].Value) > 0 && Conversion.ToString(item.Cells["ChequeNo"].Value) != string.Empty)
			{
				if (Save.Visible && Save.Enabled)
				{
					CmbCheqNo.Value = Conversion.ToInt(item.Cells["ChequeId"].Value);
					Conversion.ToInt(item.Cells["ChequeId"].Value);
				}
				else
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("ChequeId");
					dt.Columns.Add("ChequeNo");
					dt.Rows.Add(Conversion.ToInt(item.Cells["ChequeId"].Value), item.Cells["ChequeNo"].Value);
					DDL.BindDDL(dt, CmbCheqNo, "ChequeId", "ChequeNo", "Cheque_number", ZeroIndex: false);
					((UltraGridBase)CmbCheqNo).Rows[0].Activate();
					Conversion.ToInt(item.Cells["ChequeId"].Value);
				}
			}
			CheqDate.Value = Conversion.ToDateTime(item.Cells["ChequeDate"].Value);
			txtPayTitle.Text = Conversion.ToString(item.Cells["PayTitle"].Value);
			CmbDebitAccount.Focus();
			Add.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelDetail.Visible = true;
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

	private void Add_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar1")
			{
				CmbDebitAccount.Focus();
			}
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
			grd.UpdateData();
			if (e.Column.Key == "AccountId")
			{
				string AccountCode = "";
				int AccountTypeId = 0;
				int accountId = Conversion.ToInt(grd.CurrentRow.Cells["AccountId"].Value);
				for (int i = 0; i < dtAccount.Rows.Count; i++)
				{
					if (accountId == Conversion.ToInt(dtAccount.Rows[i]["Id"]))
					{
						AccountCode = dtAccount.Rows[i]["AccountCode"].ToString();
						AccountTypeId = Conversion.ToInt(dtAccount.Rows[i]["AccountTypeId"]);
						break;
					}
				}
				grd.CurrentRow.Cells["AccountCode"].Value = AccountCode;
				grd.CurrentRow.Cells["AccountTypeId"].Value = AccountTypeId;
				grd.CurrentRow.Cells["ChequeNo"].Value = "";
				grd.CurrentRow.Cells["ChequeId"].Value = 0;
			}
			if (e.Column.Key == "Amount")
			{
				double amt = Conversion.ToDouble(grd.CurrentRow.Cells["Amount"].Value);
				double ExRate2 = Conversion.ToDouble((object)txtExchangeRate.Text);
				if (ExRate2 == 0.0)
				{
					grd.CurrentRow.Cells["FcyAmount"].Value = 0;
				}
				else
				{
					double Fcy2 = amt / ExRate2;
					Fcy2 = Math.Round(Fcy2, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					grd.CurrentRow.Cells["FcyAmount"].Value = Fcy2;
				}
			}
			if (e.Column.Key == "FcyAmount")
			{
				double Fcy = Conversion.ToDouble(grd.CurrentRow.Cells["FcyAmount"].Value);
				double ExRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				if (ExRate == 0.0)
				{
					grd.CurrentRow.Cells["Amount"].Value = 0;
				}
				else
				{
					double amount = Fcy * ExRate;
					amount = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount);
					grd.CurrentRow.Cells["Amount"].Value = amount;
				}
			}
			if (e.Column.Key == "ChequeNo")
			{
				int ChequeId = 0;
				string ChequeNo = Conversion.ToString(grd.CurrentRow.Cells["ChequeNo"].Value);
				KeyValuePair<string, int> match = ChequNosInGridFromChequebook.FirstOrDefault((KeyValuePair<string, int> kv) => kv.Key == ChequeNo);
				if (!string.IsNullOrWhiteSpace(match.Key))
				{
					ChequeId = match.Value;
				}
				if (CheqBookEnabled)
				{
					grd.CurrentRow.Cells["ChequeId"].Value = ChequeId;
					if (ChequeId == 0)
					{
						grd.CurrentRow.Cells["ChequeNo"].Value = "";
						MessageBox.Show("You cannot write a Cheque No when Cheque Book status is on. Please press F1 to select a Cheque No.");
					}
				}
				else if (((UltraGridBase)CmbCreditAccount).ActiveRow != null && Conversion.ToInt(CmbCreditAccount.Value) != 0 && Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells["AccountTypeId"].Value) != 15)
				{
					grd.CurrentRow.Cells["ChequeId"].Value = ChequeId;
					if (ChequeId == 0)
					{
						grd.CurrentRow.Cells["ChequeNo"].Value = "";
						MessageBox.Show("You can add a Cheque No only for Bank Account.");
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
			dt.Rows.Add("Ctrl+S", "For Save in form tab and for show history in history tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Voucher Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid in form tab and to focus on history grid in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Debit Account in Detail Box of form tab and From Date in history tab");
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
				ReadById(ID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(ID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(ID);
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn != null && grd.CurrentColumn.Key == "Delete")
			{
				item.Delete();
				grd.UpdateData();
				if (MultiCurrencyGridWorking)
				{
					CreateUpdateMultiCurrencyGridInfo();
				}
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string Name = "";
			if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "ChequeNo" || grd.CurrentColumn.Key == "ChequeNo"))
			{
				if (!CheqBookEnabled)
				{
					throw new Exception("Cheque Book Status Is Off,you can Write Cheque no Manually");
				}
				if (((UltraGridBase)CmbCreditAccount).ActiveRow != null && Conversion.ToInt(CmbCreditAccount.Value) != 0 && Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells["AccountTypeId"].Value) != 15)
				{
					throw new Exception("You can add chequeNo only For Bank Account");
				}
				GrdPopUp grdPopUp = new GrdPopUp(CheqBookHeader.OutstandingCheqNo(new CheqBookHeader
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BankId = Conversion.ToInt(CmbCreditAccount.Value)
				}), "Id", "CheqNo");
				grdPopUp.StartPosition = FormStartPosition.CenterScreen;
				grdPopUp.ShowDialog();
				grdPopUp.BringToFront();
				Id = Conversion.ToInt((object)grdPopUp.ReturnId);
				Name = Conversion.ToString((object)grdPopUp.ReturnName);
				item.Cells["ChequeId"].Value = Id;
				item.Cells["ChequeNo"].Value = Name;
				if (Id > 0 && !ChequNosInGridFromChequebook.ContainsKey(Name))
				{
					ChequNosInGridFromChequebook.Add(Name, Id);
				}
			}
			grd.UpdateData();
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
				DataTable dt = VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "10",
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
				txtExchangeRate.Text = BaseRate.ToString();
				txtTcyExchangeRateDetail.Text = BaseRate.ToString();
			}
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

	private void btnShowHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "DataGridHistory")
			{
				FromDateHistory.Focus();
			}
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
				if (Conversion.ToInt(CmbDebitAccount.Value) > 0)
				{
					Id = Conversion.ToInt(CmbDebitAccount.Value);
				}
				if (rdbtnAccountName.Checked)
				{
					DDL.BindDDL(dtAccount, CmbDebitAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtAccount, CmbDebitAccount, "Id", "AccountCode", "Account Code", ZeroIndex: true);
				}
				if (Id > 0)
				{
					CmbDebitAccount.Value = Id;
					CmbDebitAccount.Focus();
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
			if (Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text) == 0.0)
			{
				txtTcyExchangeRateDetail.Focus();
			}
			CommonServices.UpdateRelatedFieldsCalculation(txtamount, txtFcyAmountDetail, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
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
			if (((UltraDropDownBase)CmbCreditAccount).SelectedRow != null && ((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[6].Value != null)
			{
				int headerCurrencyId = Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[6].Value);
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
			if (((UltraDropDownBase)CmbCreditAccount).SelectedRow != null && ((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[6].Value != null)
			{
				requiredCurrencyIds.Add(Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[6].Value));
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
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Expected O, but got Unknown
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Expected O, but got Unknown
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Expected O, but got Unknown
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Expected O, but got Unknown
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Expected O, but got Unknown
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Expected O, but got Unknown
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Expected O, but got Unknown
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Expected O, but got Unknown
		//IL_0891: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Expected O, but got Unknown
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Expected O, but got Unknown
		//IL_0936: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Expected O, but got Unknown
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Expected O, but got Unknown
		//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Expected O, but got Unknown
		//IL_0ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acc: Expected O, but got Unknown
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af8: Expected O, but got Unknown
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b66: Expected O, but got Unknown
		//IL_0b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b71: Expected O, but got Unknown
		//IL_1811: Unknown result type (might be due to invalid IL or missing references)
		//IL_181b: Expected O, but got Unknown
		//IL_1828: Unknown result type (might be due to invalid IL or missing references)
		//IL_1832: Expected O, but got Unknown
		//IL_183f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1849: Expected O, but got Unknown
		//IL_1856: Unknown result type (might be due to invalid IL or missing references)
		//IL_1860: Expected O, but got Unknown
		//IL_8ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_8ed8: Expected O, but got Unknown
		//IL_8efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_8f06: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.ContraVoucher));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdMultiCurrency_DesignTimeLayout = new GridEXLayout();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
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
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
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
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnEdit = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnDe = new System.Windows.Forms.ToolStripButton();
		this.btnCopy = new System.Windows.Forms.ToolStripButton();
		this.btnPaste = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdMultiCurrency = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.lblCrAcGlCurrency = new System.Windows.Forms.Label();
		this.txtBaseFcyCode = new System.Windows.Forms.TextBox();
		this.lblBaseFcyCode = new System.Windows.Forms.Label();
		this.txtBaseFcyExchangeRate = new System.Windows.Forms.TextBox();
		this.lblBaseFcyExchangeRate = new System.Windows.Forms.Label();
		this.CmbProjectId = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.txtbalance = new System.Windows.Forms.TextBox();
		this.txtremarksmain = new System.Windows.Forms.TextBox();
		this.cmbCurrency = new UltraCombo();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.txtvoucherno = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbCreditAccount = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.voucherdatetime = new System.Windows.Forms.DateTimePicker();
		this.groupboxdetail = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.DebitAccountPanel = new System.Windows.Forms.Panel();
		this.rdbtnAccountCode = new System.Windows.Forms.RadioButton();
		this.rdbtnAccountName = new System.Windows.Forms.RadioButton();
		this.CmbDebitAccount = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.lblbalancedetail = new System.Windows.Forms.Label();
		this.JobLotPanel = new System.Windows.Forms.Panel();
		this.CmbJobLot = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.RemarksPanel = new System.Windows.Forms.Panel();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.panel26 = new System.Windows.Forms.Panel();
		this.cmbTcyCodeDetail = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.txtTcyExchangeRateDetail = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.txtFcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.DebitAmountPanel = new System.Windows.Forms.Panel();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.lblDebitAmount = new System.Windows.Forms.Label();
		this.ReferenceAccountPanel = new System.Windows.Forms.Panel();
		this.rdBtnRefAcByCode = new System.Windows.Forms.RadioButton();
		this.rdBtnRefAcByTitle = new System.Windows.Forms.RadioButton();
		this.CmbReferenceAccount = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.BranchPanel = new System.Windows.Forms.Panel();
		this.cmbBranchName = new UltraCombo();
		this.labelbranchname = new System.Windows.Forms.Label();
		this.CostCenterPanel = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.label39 = new System.Windows.Forms.Label();
		this.ChequeDetailPanel = new System.Windows.Forms.Panel();
		this.CheqDate = new System.Windows.Forms.DateTimePicker();
		this.lblCheqDate = new System.Windows.Forms.Label();
		this.txtPayTitle = new System.Windows.Forms.TextBox();
		this.lblCheqNo = new System.Windows.Forms.Label();
		this.lblPayTitle = new System.Windows.Forms.Label();
		this.CmbCheqNo = new UltraCombo();
		this.panel14 = new System.Windows.Forms.Panel();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.Add = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbWithHoldingAc = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.ChkBoxWthHolding = new System.Windows.Forms.CheckBox();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtValue = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.PaymentVoucherLable = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.Update = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnCreateUpdateMultiCurrency = new System.Windows.Forms.ToolStripButton();
		this.CmbAgainstAc = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbPaymentType = new System.Windows.Forms.ComboBox();
		this.History = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
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
		this.CmbAccountTitleHistory = new UltraCombo();
		this.cmbApproveStatus = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.rdconapproved = new System.Windows.Forms.RadioButton();
		this.rdconmodify = new System.Windows.Forms.RadioButton();
		this.rdcondoc = new System.Windows.Forms.RadioButton();
		this.rdconentry = new System.Windows.Forms.RadioButton();
		this.label24 = new System.Windows.Forms.Label();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel7 = new System.Windows.Forms.Panel();
		this.button1 = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.cmbdoctype = new System.Windows.Forms.ComboBox();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		this.groupboxdetail.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.DebitAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).BeginInit();
		this.JobLotPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		this.RemarksPanel.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTcyCodeDetail).BeginInit();
		this.panel25.SuspendLayout();
		this.panel5.SuspendLayout();
		this.DebitAmountPanel.SuspendLayout();
		this.ReferenceAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).BeginInit();
		this.BranchPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		this.CostCenterPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		this.ChequeDetailPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqNo).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWithHoldingAc).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAgainstAc).BeginInit();
		this.History.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel18.SuspendLayout();
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
			this.btnNew, this.btnEdit, this.btnSave, this.btnPrint, this.toolStripSeparator, this.btnDe, this.btnCopy, this.btnPaste, this.toolStripSeparator1, this.toolStripButton1,
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
		this.btnDe.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDe.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDe.Name = "btnDe";
		this.btnDe.Size = new System.Drawing.Size(64, 24);
		this.btnDe.Text = "&Delete";
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
		this.panel1.Size = new System.Drawing.Size(1327, 690);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1327, 690);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.panel6);
		this.Form.Controls.Add(this.panel2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(1319, 664);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Controls.Add(this.panel17);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 279);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1315, 383);
		this.panel6.TabIndex = 1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1315, 238);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellEdited += new ColumnActionEventHandler(grd_CellEdited);
		this.grd.EditingCell += new EditingCellEventHandler(grd_EditingCell);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdMultiCurrency);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel17.Location = new System.Drawing.Point(0, 238);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1315, 145);
		this.panel17.TabIndex = 1;
		((ScrollableBase)this.grdMultiCurrency).BackColor = System.Drawing.Color.White;
		grdMultiCurrency_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMultiCurrency.DesignTimeLayout = grdMultiCurrency_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Dock = System.Windows.Forms.DockStyle.Left;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMultiCurrency.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Name = "grdMultiCurrency";
		((ScrollableBase)this.grdMultiCurrency).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Size = new System.Drawing.Size(338, 145);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabIndex = 60;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabStop = false;
		this.grdMultiCurrency.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMultiCurrency).VisualStyle = (VisualStyle)4;
		this.grdMultiCurrency.SelectionChanged += new System.EventHandler(grdMultiCurrency_SelectionChanged);
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Controls.Add(this.groupboxdetail);
		this.panel2.Controls.Add(this.txtTaxPercent);
		this.panel2.Controls.Add(this.label21);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc);
		this.panel2.Controls.Add(this.label12);
		this.panel2.Controls.Add(this.ChkBoxWthHolding);
		this.panel2.Controls.Add(this.txtTotalAmount);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.txtTaxAmount);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.txtValue);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAgainstAc);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.CmbPaymentType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1315, 277);
		this.panel2.TabIndex = 0;
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.lblCrAcGlCurrency);
		this.groupBox2.Controls.Add(this.txtBaseFcyCode);
		this.groupBox2.Controls.Add(this.lblBaseFcyCode);
		this.groupBox2.Controls.Add(this.txtBaseFcyExchangeRate);
		this.groupBox2.Controls.Add(this.lblBaseFcyExchangeRate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProjectId);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.lblFcyAmount);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.lblExchangeRate);
		this.groupBox2.Controls.Add(this.txtbalance);
		this.groupBox2.Controls.Add(this.txtremarksmain);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox2.Controls.Add(this.lblFcyCode);
		this.groupBox2.Controls.Add(this.txtvoucherno);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.voucherdatetime);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 64);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1184, 96);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.lblCrAcGlCurrency.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblCrAcGlCurrency.Location = new System.Drawing.Point(1064, 19);
		this.lblCrAcGlCurrency.Name = "lblCrAcGlCurrency";
		this.lblCrAcGlCurrency.Size = new System.Drawing.Size(95, 27);
		this.lblCrAcGlCurrency.TabIndex = 572;
		this.lblCrAcGlCurrency.Text = "Credit Ac GLcy:";
		this.txtBaseFcyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyCode.Enabled = false;
		this.txtBaseFcyCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyCode.Location = new System.Drawing.Point(948, 16);
		this.txtBaseFcyCode.Name = "txtBaseFcyCode";
		this.txtBaseFcyCode.ShortcutsEnabled = false;
		this.txtBaseFcyCode.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyCode.TabIndex = 573;
		this.txtBaseFcyCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyCode.AutoSize = true;
		this.lblBaseFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyCode.Location = new System.Drawing.Point(869, 20);
		this.lblBaseFcyCode.Name = "lblBaseFcyCode";
		this.lblBaseFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblBaseFcyCode.TabIndex = 574;
		this.lblBaseFcyCode.Text = "Fcy Code";
		this.txtBaseFcyExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyExchangeRate.Location = new System.Drawing.Point(948, 42);
		this.txtBaseFcyExchangeRate.Name = "txtBaseFcyExchangeRate";
		this.txtBaseFcyExchangeRate.ShortcutsEnabled = false;
		this.txtBaseFcyExchangeRate.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyExchangeRate.TabIndex = 571;
		this.txtBaseFcyExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyExchangeRate.AutoSize = true;
		this.lblBaseFcyExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyExchangeRate.Location = new System.Drawing.Point(869, 38);
		this.lblBaseFcyExchangeRate.Name = "lblBaseFcyExchangeRate";
		this.lblBaseFcyExchangeRate.Size = new System.Drawing.Size(82, 30);
		this.lblBaseFcyExchangeRate.TabIndex = 572;
		this.lblBaseFcyExchangeRate.Text = "Fcy Exchange \r\nRate";
		this.CmbProjectId.AllowNull = (DefaultableBoolean)1;
		this.CmbProjectId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProjectId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProjectId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProjectId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProjectId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Location = new System.Drawing.Point(94, 14);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Name = "CmbProjectId";
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(44, 15);
		this.label5.TabIndex = 564;
		this.label5.Text = "Project";
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyAmount.Location = new System.Drawing.Point(653, 72);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(72, 15);
		this.lblFcyAmount.TabIndex = 562;
		this.lblFcyAmount.Text = "Tcy Amount";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(738, 68);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(128, 23);
		this.txtFcyAmount.TabIndex = 8;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(738, 42);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(128, 23);
		this.txtExchangeRate.TabIndex = 7;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.lblExchangeRate.AutoSize = true;
		this.lblExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblExchangeRate.Location = new System.Drawing.Point(653, 38);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(82, 30);
		this.lblExchangeRate.TabIndex = 561;
		this.lblExchangeRate.Text = "Tcy Exchange \r\nRate";
		this.txtbalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbalance.Enabled = false;
		this.txtbalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbalance.Location = new System.Drawing.Point(417, 42);
		this.txtbalance.Name = "txtbalance";
		this.txtbalance.Size = new System.Drawing.Size(232, 23);
		this.txtbalance.TabIndex = 5;
		this.txtbalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtremarksmain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksmain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksmain.Location = new System.Drawing.Point(94, 68);
		this.txtremarksmain.Name = "txtremarksmain";
		this.txtremarksmain.Size = new System.Drawing.Size(555, 23);
		this.txtremarksmain.TabIndex = 3;
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(738, 14);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(128, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(653, 20);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 560;
		this.lblFcyCode.Text = "Tcy Code";
		this.txtvoucherno.BackColor = System.Drawing.Color.White;
		this.txtvoucherno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvoucherno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvoucherno.Location = new System.Drawing.Point(94, 42);
		this.txtvoucherno.Name = "txtvoucherno";
		this.txtvoucherno.ReadOnly = true;
		this.txtvoucherno.Size = new System.Drawing.Size(104, 23);
		this.txtvoucherno.TabIndex = 1;
		this.txtvoucherno.TabStop = false;
		this.txtvoucherno.Leave += new System.EventHandler(txtvoucherno_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 46);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(70, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Voucher No";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 72);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "Remarks";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(417, 14);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TextChanged += new System.EventHandler(combcreditac_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Leave += new System.EventHandler(combcreditac_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(329, 46);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(71, 15);
		this.label6.TabIndex = 4;
		this.label6.Text = "Account Bal";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(329, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "Credit Account";
		this.voucherdatetime.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.voucherdatetime.CustomFormat = "dd-MMM-yy";
		this.voucherdatetime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.voucherdatetime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.voucherdatetime.Location = new System.Drawing.Point(200, 42);
		this.voucherdatetime.Name = "voucherdatetime";
		this.voucherdatetime.Size = new System.Drawing.Size(126, 23);
		this.voucherdatetime.TabIndex = 2;
		this.groupboxdetail.BackColor = System.Drawing.Color.White;
		this.groupboxdetail.Controls.Add(this.flowLayoutPanel1);
		this.groupboxdetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupboxdetail.Location = new System.Drawing.Point(2, 161);
		this.groupboxdetail.Name = "groupboxdetail";
		this.groupboxdetail.Size = new System.Drawing.Size(1185, 112);
		this.groupboxdetail.TabIndex = 1;
		this.groupboxdetail.TabStop = false;
		this.groupboxdetail.Text = "Detail";
		this.flowLayoutPanel1.Controls.Add(this.DebitAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.JobLotPanel);
		this.flowLayoutPanel1.Controls.Add(this.RemarksPanel);
		this.flowLayoutPanel1.Controls.Add(this.panel26);
		this.flowLayoutPanel1.Controls.Add(this.panel25);
		this.flowLayoutPanel1.Controls.Add(this.panel5);
		this.flowLayoutPanel1.Controls.Add(this.DebitAmountPanel);
		this.flowLayoutPanel1.Controls.Add(this.ReferenceAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.BranchPanel);
		this.flowLayoutPanel1.Controls.Add(this.CostCenterPanel);
		this.flowLayoutPanel1.Controls.Add(this.ChequeDetailPanel);
		this.flowLayoutPanel1.Controls.Add(this.panel14);
		this.flowLayoutPanel1.Location = new System.Drawing.Point(1, 13);
		this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(1167, 95);
		this.flowLayoutPanel1.TabIndex = 0;
		this.DebitAccountPanel.Controls.Add(this.rdbtnAccountCode);
		this.DebitAccountPanel.Controls.Add(this.rdbtnAccountName);
		this.DebitAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccount);
		this.DebitAccountPanel.Controls.Add(this.label11);
		this.DebitAccountPanel.Controls.Add(this.lblbalancedetail);
		this.DebitAccountPanel.Location = new System.Drawing.Point(0, 0);
		this.DebitAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitAccountPanel.Name = "DebitAccountPanel";
		this.DebitAccountPanel.Size = new System.Drawing.Size(258, 47);
		this.DebitAccountPanel.TabIndex = 0;
		this.rdbtnAccountCode.AutoSize = true;
		this.rdbtnAccountCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnAccountCode.Location = new System.Drawing.Point(104, 1);
		this.rdbtnAccountCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnAccountCode.Name = "rdbtnAccountCode";
		this.rdbtnAccountCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnAccountCode.TabIndex = 2;
		this.rdbtnAccountCode.Text = "Code";
		this.rdbtnAccountCode.UseVisualStyleBackColor = true;
		this.rdbtnAccountName.AutoSize = true;
		this.rdbtnAccountName.Checked = true;
		this.rdbtnAccountName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnAccountName.Location = new System.Drawing.Point(57, 1);
		this.rdbtnAccountName.Margin = new System.Windows.Forms.Padding(0);
		this.rdbtnAccountName.Name = "rdbtnAccountName";
		this.rdbtnAccountName.Size = new System.Drawing.Size(48, 19);
		this.rdbtnAccountName.TabIndex = 1;
		this.rdbtnAccountName.TabStop = true;
		this.rdbtnAccountName.Text = "Title";
		this.rdbtnAccountName.UseVisualStyleBackColor = true;
		this.rdbtnAccountName.CheckedChanged += new System.EventHandler(rdbtnAccountName_CheckedChanged);
		this.CmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Name = "CmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Leave += new System.EventHandler(combactitle_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(4, 3);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(53, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Debit Ac";
		this.lblbalancedetail.AutoSize = true;
		this.lblbalancedetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblbalancedetail.Location = new System.Drawing.Point(153, 4);
		this.lblbalancedetail.Name = "lblbalancedetail";
		this.lblbalancedetail.Size = new System.Drawing.Size(48, 14);
		this.lblbalancedetail.TabIndex = 3;
		this.lblbalancedetail.Text = "label8";
		this.JobLotPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.JobLotPanel.Controls.Add(this.label22);
		this.JobLotPanel.Location = new System.Drawing.Point(258, 0);
		this.JobLotPanel.Margin = new System.Windows.Forms.Padding(0);
		this.JobLotPanel.Name = "JobLotPanel";
		this.JobLotPanel.Size = new System.Drawing.Size(147, 47);
		this.JobLotPanel.TabIndex = 1;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 0;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(1, 2);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(48, 15);
		this.label22.TabIndex = 12;
		this.label22.Text = "Job/Lot";
		this.RemarksPanel.Controls.Add(this.txtremarks);
		this.RemarksPanel.Controls.Add(this.label19);
		this.RemarksPanel.Location = new System.Drawing.Point(405, 0);
		this.RemarksPanel.Margin = new System.Windows.Forms.Padding(0);
		this.RemarksPanel.Name = "RemarksPanel";
		this.RemarksPanel.Size = new System.Drawing.Size(268, 47);
		this.RemarksPanel.TabIndex = 2;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(0, 22);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(267, 23);
		this.txtremarks.TabIndex = 2;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1, 3);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(52, 15);
		this.label19.TabIndex = 2;
		this.label19.Text = "Remarks";
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail);
		this.panel26.Controls.Add(this.label54);
		this.panel26.Location = new System.Drawing.Point(673, 0);
		this.panel26.Margin = new System.Windows.Forms.Padding(0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(112, 47);
		this.panel26.TabIndex = 3;
		this.cmbTcyCodeDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbTcyCodeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTcyCodeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTcyCodeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
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
		this.panel25.Location = new System.Drawing.Point(785, 0);
		this.panel25.Margin = new System.Windows.Forms.Padding(0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(120, 47);
		this.panel25.TabIndex = 4;
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
		this.panel5.Controls.Add(this.txtFcyAmountDetail);
		this.panel5.Controls.Add(this.label29);
		this.panel5.Location = new System.Drawing.Point(905, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(123, 47);
		this.panel5.TabIndex = 5;
		this.txtFcyAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountDetail.Location = new System.Drawing.Point(1, 22);
		this.txtFcyAmountDetail.Name = "txtFcyAmountDetail";
		this.txtFcyAmountDetail.ShortcutsEnabled = false;
		this.txtFcyAmountDetail.Size = new System.Drawing.Size(122, 23);
		this.txtFcyAmountDetail.TabIndex = 3;
		this.txtFcyAmountDetail.Tag = "FcyAmount";
		this.txtFcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountDetail.TextChanged += new System.EventHandler(txtFcyAmountDetail_TextChanged);
		this.txtFcyAmountDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtamount_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(1, 3);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(72, 15);
		this.label29.TabIndex = 4;
		this.label29.Text = "Tcy Amount";
		this.DebitAmountPanel.Controls.Add(this.txtamount);
		this.DebitAmountPanel.Controls.Add(this.lblDebitAmount);
		this.DebitAmountPanel.Location = new System.Drawing.Point(1028, 0);
		this.DebitAmountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitAmountPanel.Name = "DebitAmountPanel";
		this.DebitAmountPanel.Size = new System.Drawing.Size(123, 47);
		this.DebitAmountPanel.TabIndex = 6;
		this.txtamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtamount.Location = new System.Drawing.Point(1, 22);
		this.txtamount.Name = "txtamount";
		this.txtamount.ShortcutsEnabled = false;
		this.txtamount.Size = new System.Drawing.Size(122, 23);
		this.txtamount.TabIndex = 4;
		this.txtamount.Tag = "DebitAmount";
		this.txtamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtamount.TextChanged += new System.EventHandler(txtamount_TextChanged);
		this.txtamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtamount_KeyPress);
		this.lblDebitAmount.AutoSize = true;
		this.lblDebitAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDebitAmount.Location = new System.Drawing.Point(1, 3);
		this.lblDebitAmount.Name = "lblDebitAmount";
		this.lblDebitAmount.Size = new System.Drawing.Size(83, 15);
		this.lblDebitAmount.TabIndex = 4;
		this.lblDebitAmount.Text = "Debit Amount";
		this.ReferenceAccountPanel.Controls.Add(this.rdBtnRefAcByCode);
		this.ReferenceAccountPanel.Controls.Add(this.rdBtnRefAcByTitle);
		this.ReferenceAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceAccount);
		this.ReferenceAccountPanel.Controls.Add(this.label30);
		this.ReferenceAccountPanel.Location = new System.Drawing.Point(0, 47);
		this.ReferenceAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ReferenceAccountPanel.Name = "ReferenceAccountPanel";
		this.ReferenceAccountPanel.Size = new System.Drawing.Size(186, 47);
		this.ReferenceAccountPanel.TabIndex = 7;
		this.rdBtnRefAcByCode.AutoSize = true;
		this.rdBtnRefAcByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdBtnRefAcByCode.Location = new System.Drawing.Point(123, 2);
		this.rdBtnRefAcByCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdBtnRefAcByCode.Name = "rdBtnRefAcByCode";
		this.rdBtnRefAcByCode.Size = new System.Drawing.Size(52, 19);
		this.rdBtnRefAcByCode.TabIndex = 7;
		this.rdBtnRefAcByCode.Text = "Code";
		this.rdBtnRefAcByCode.UseVisualStyleBackColor = true;
		this.rdBtnRefAcByTitle.AutoSize = true;
		this.rdBtnRefAcByTitle.Checked = true;
		this.rdBtnRefAcByTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdBtnRefAcByTitle.Location = new System.Drawing.Point(79, 2);
		this.rdBtnRefAcByTitle.Margin = new System.Windows.Forms.Padding(0);
		this.rdBtnRefAcByTitle.Name = "rdBtnRefAcByTitle";
		this.rdBtnRefAcByTitle.Size = new System.Drawing.Size(48, 19);
		this.rdBtnRefAcByTitle.TabIndex = 6;
		this.rdBtnRefAcByTitle.TabStop = true;
		this.rdBtnRefAcByTitle.Text = "Title";
		this.rdBtnRefAcByTitle.UseVisualStyleBackColor = true;
		this.rdBtnRefAcByTitle.CheckedChanged += new System.EventHandler(rdBtnRefAcByTitle_CheckedChanged);
		this.CmbReferenceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Name = "CmbReferenceAccount";
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceAccount).TabIndex = 5;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(1, 4);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(76, 15);
		this.label30.TabIndex = 596;
		this.label30.Text = "Reference Ac";
		this.BranchPanel.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.BranchPanel.Controls.Add(this.labelbranchname);
		this.BranchPanel.Location = new System.Drawing.Point(186, 47);
		this.BranchPanel.Margin = new System.Windows.Forms.Padding(0);
		this.BranchPanel.Name = "BranchPanel";
		this.BranchPanel.Size = new System.Drawing.Size(201, 47);
		this.BranchPanel.TabIndex = 8;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
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
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 4;
		this.labelbranchname.AutoSize = true;
		this.labelbranchname.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelbranchname.Location = new System.Drawing.Point(2, 4);
		this.labelbranchname.Name = "labelbranchname";
		this.labelbranchname.Size = new System.Drawing.Size(79, 15);
		this.labelbranchname.TabIndex = 596;
		this.labelbranchname.Text = "Branch Name";
		this.CostCenterPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.CostCenterPanel.Controls.Add(this.label39);
		this.CostCenterPanel.Location = new System.Drawing.Point(387, 47);
		this.CostCenterPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CostCenterPanel.Name = "CostCenterPanel";
		this.CostCenterPanel.Size = new System.Drawing.Size(166, 47);
		this.CostCenterPanel.TabIndex = 9;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 4;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1, 2);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(67, 15);
		this.label39.TabIndex = 596;
		this.label39.Text = "Cost Center";
		this.ChequeDetailPanel.Controls.Add(this.CheqDate);
		this.ChequeDetailPanel.Controls.Add(this.lblCheqDate);
		this.ChequeDetailPanel.Controls.Add(this.txtPayTitle);
		this.ChequeDetailPanel.Controls.Add(this.lblCheqNo);
		this.ChequeDetailPanel.Controls.Add(this.lblPayTitle);
		this.ChequeDetailPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCheqNo);
		this.ChequeDetailPanel.Location = new System.Drawing.Point(553, 47);
		this.ChequeDetailPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ChequeDetailPanel.Name = "ChequeDetailPanel";
		this.ChequeDetailPanel.Size = new System.Drawing.Size(517, 47);
		this.ChequeDetailPanel.TabIndex = 10;
		this.CheqDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CheqDate.CustomFormat = "dd-MMM-yy";
		this.CheqDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CheqDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.CheqDate.Location = new System.Drawing.Point(2, 23);
		this.CheqDate.Name = "CheqDate";
		this.CheqDate.Size = new System.Drawing.Size(125, 23);
		this.CheqDate.TabIndex = 0;
		this.lblCheqDate.AutoSize = true;
		this.lblCheqDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCheqDate.Location = new System.Drawing.Point(2, 2);
		this.lblCheqDate.Name = "lblCheqDate";
		this.lblCheqDate.Size = new System.Drawing.Size(75, 15);
		this.lblCheqDate.TabIndex = 33;
		this.lblCheqDate.Text = "Cheque Date";
		this.txtPayTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPayTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPayTitle.Location = new System.Drawing.Point(241, 22);
		this.txtPayTitle.Name = "txtPayTitle";
		this.txtPayTitle.Size = new System.Drawing.Size(276, 23);
		this.txtPayTitle.TabIndex = 2;
		this.lblCheqNo.AutoSize = true;
		this.lblCheqNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCheqNo.Location = new System.Drawing.Point(128, 2);
		this.lblCheqNo.Name = "lblCheqNo";
		this.lblCheqNo.Size = new System.Drawing.Size(66, 15);
		this.lblCheqNo.TabIndex = 0;
		this.lblCheqNo.Text = "Cheque No";
		this.lblPayTitle.AutoSize = true;
		this.lblPayTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPayTitle.Location = new System.Drawing.Point(242, 2);
		this.lblPayTitle.Name = "lblPayTitle";
		this.lblPayTitle.Size = new System.Drawing.Size(49, 15);
		this.lblPayTitle.TabIndex = 35;
		this.lblPayTitle.Text = "PayTitle";
		this.CmbCheqNo.AllowNull = (DefaultableBoolean)1;
		this.CmbCheqNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCheqNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCheqNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCheqNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCheqNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Location = new System.Drawing.Point(128, 21);
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Name = "CmbCheqNo";
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.CmbCheqNo).TabIndex = 1;
		this.panel14.Controls.Add(this.btnCancelDetail);
		this.panel14.Controls.Add(this.Add);
		this.panel14.Controls.Add(this.btnUpdateDetail);
		this.panel14.Location = new System.Drawing.Point(1070, 47);
		this.panel14.Margin = new System.Windows.Forms.Padding(0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(81, 47);
		this.panel14.TabIndex = 11;
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(0, -1);
		this.btnCancelDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(66, 25);
		this.btnCancelDetail.TabIndex = 2;
		this.btnCancelDetail.Text = "Clear";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(0, 21);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(26, 25);
		this.Add.TabIndex = 0;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click_1);
		this.Add.Leave += new System.EventHandler(Add_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1, 20);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 25);
		this.btnUpdateDetail.TabIndex = 1;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtTaxPercent.Location = new System.Drawing.Point(1063, 396);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(72, 20);
		this.txtTaxPercent.TabIndex = 45;
		this.txtTaxPercent.TabStop = false;
		this.txtTaxPercent.Visible = false;
		this.label21.AutoSize = true;
		this.label21.Location = new System.Drawing.Point(1013, 399);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(36, 13);
		this.label21.TabIndex = 44;
		this.label21.Text = "Tax %";
		this.label21.Visible = false;
		this.CmbWithHoldingAc.AllowNull = (DefaultableBoolean)1;
		this.CmbWithHoldingAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWithHoldingAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWithHoldingAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWithHoldingAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWithHoldingAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Location = new System.Drawing.Point(903, 421);
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Name = "CmbWithHoldingAc";
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Size = new System.Drawing.Size(232, 24);
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).TabIndex = 36;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbWithHoldingAc).Visible = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(802, 426);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(87, 15);
		this.label12.TabIndex = 43;
		this.label12.Text = "WithHoldingAc";
		this.label12.Visible = false;
		this.ChkBoxWthHolding.AutoSize = true;
		this.ChkBoxWthHolding.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBoxWthHolding.Location = new System.Drawing.Point(903, 396);
		this.ChkBoxWthHolding.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxWthHolding.Name = "ChkBoxWthHolding";
		this.ChkBoxWthHolding.Size = new System.Drawing.Size(102, 17);
		this.ChkBoxWthHolding.TabIndex = 35;
		this.ChkBoxWthHolding.TabStop = false;
		this.ChkBoxWthHolding.Text = "WithHoldingTax";
		this.ChkBoxWthHolding.UseVisualStyleBackColor = true;
		this.ChkBoxWthHolding.Visible = false;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(1241, 442);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(142, 21);
		this.txtTotalAmount.TabIndex = 41;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.Visible = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(1148, 445);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(79, 15);
		this.label9.TabIndex = 42;
		this.label9.Text = "Total Amount";
		this.label9.Visible = false;
		this.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(1241, 417);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(142, 21);
		this.txtTaxAmount.TabIndex = 39;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1148, 420);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 15);
		this.label8.TabIndex = 40;
		this.label8.Text = "Tax Amount";
		this.label8.Visible = false;
		this.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtValue.Location = new System.Drawing.Point(1241, 392);
		this.txtValue.Name = "txtValue";
		this.txtValue.ReadOnly = true;
		this.txtValue.Size = new System.Drawing.Size(142, 21);
		this.txtValue.TabIndex = 37;
		this.txtValue.TabStop = false;
		this.txtValue.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1148, 395);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(49, 15);
		this.label4.TabIndex = 38;
		this.label4.Text = "Amount";
		this.label4.Visible = false;
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.chkCustomAccounts);
		this.panel4.Controls.Add(this.labelLocationType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.PaymentVoucherLable);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1315, 32);
		this.panel4.TabIndex = 0;
		this.chkCustomAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(1048, 7);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(122, 19);
		this.chkCustomAccounts.TabIndex = 10;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(343, 8);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 506;
		this.labelLocationType.Text = "Location Type";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(432, 2);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 505;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabStop = false;
		this.PaymentVoucherLable.AutoSize = true;
		this.PaymentVoucherLable.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PaymentVoucherLable.ForeColor = System.Drawing.Color.White;
		this.PaymentVoucherLable.Location = new System.Drawing.Point(5, 6);
		this.PaymentVoucherLable.Name = "PaymentVoucherLable";
		this.PaymentVoucherLable.Size = new System.Drawing.Size(321, 19);
		this.PaymentVoucherLable.TabIndex = 0;
		this.PaymentVoucherLable.Text = "Intra-Company Fund Transfer Voucher";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBox.Location = new System.Drawing.Point(1170, 7);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(102, 19);
		this.ChkBox.TabIndex = 9;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(945, 374);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(81, 15);
		this.label3.TabIndex = 8;
		this.label3.Text = "PaymentType";
		this.label3.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1315, 30);
		this.panel3.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.New, this.btnRefresh, this.Update, this.Save, this.toolStripButton10, this.btnDelete, this.Print, this.btnShortCutKey, this.btnCreateUpdateMultiCurrency });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1315, 27);
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
		this.Print.Text = "102-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnCreateUpdateMultiCurrency.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCreateUpdateMultiCurrency.Image = (System.Drawing.Image)resources.GetObject("btnCreateUpdateMultiCurrency.Image");
		this.btnCreateUpdateMultiCurrency.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCreateUpdateMultiCurrency.Name = "btnCreateUpdateMultiCurrency";
		this.btnCreateUpdateMultiCurrency.Size = new System.Drawing.Size(127, 24);
		this.btnCreateUpdateMultiCurrency.Text = "MultiCurrencyInfo";
		this.btnCreateUpdateMultiCurrency.Click += new System.EventHandler(btnCreateUpdateMultiCurrency_Click);
		this.CmbAgainstAc.AllowNull = (DefaultableBoolean)1;
		this.CmbAgainstAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAgainstAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAgainstAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAgainstAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAgainstAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Location = new System.Drawing.Point(1241, 365);
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Name = "CmbAgainstAc";
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Size = new System.Drawing.Size(142, 24);
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbAgainstAc).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1148, 374);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(63, 15);
		this.label13.TabIndex = 32;
		this.label13.Text = "Against Ac";
		this.label13.Visible = false;
		this.CmbPaymentType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.CmbPaymentType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.CmbPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.CmbPaymentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentType.FormattingEnabled = true;
		this.CmbPaymentType.Items.AddRange(new object[2] { "Regular", "Advance" });
		this.CmbPaymentType.Location = new System.Drawing.Point(1031, 370);
		this.CmbPaymentType.Margin = new System.Windows.Forms.Padding(2);
		this.CmbPaymentType.Name = "CmbPaymentType";
		this.CmbPaymentType.Size = new System.Drawing.Size(104, 25);
		this.CmbPaymentType.TabIndex = 0;
		this.CmbPaymentType.TabStop = false;
		this.CmbPaymentType.Visible = false;
		this.History.Controls.Add(this.panel8);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Margin = new System.Windows.Forms.Padding(2);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(2);
		this.History.Size = new System.Drawing.Size(1319, 664);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1315, 660);
		this.panel8.TabIndex = 0;
		this.panel11.Controls.Add(this.panel10);
		this.panel11.Controls.Add(this.panel7);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1315, 660);
		this.panel11.TabIndex = 2;
		this.panel10.Controls.Add(this.panel9);
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel16);
		this.panel10.Controls.Add(this.panel15);
		this.panel10.Controls.Add(this.toolStrip3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1315, 660);
		this.panel10.TabIndex = 45;
		this.panel9.Controls.Add(this.panel13);
		this.panel9.Controls.Add(this.panel18);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 373);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1315, 287);
		this.panel9.TabIndex = 54;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 26);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1315, 261);
		this.panel13.TabIndex = 55;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		this.grdDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1315, 261);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label28);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1315, 26);
		this.panel18.TabIndex = 54;
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.Teal;
		this.label28.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(2, 4);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(242, 19);
		this.label28.TabIndex = 54;
		this.label28.Text = "Detail of above selected row";
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 133);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1315, 240);
		this.panel12.TabIndex = 53;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1315, 240);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel16.Controls.Add(this.VoucherInfoBox);
		this.panel16.Controls.Add(this.groupBox3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 55);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1315, 78);
		this.panel16.TabIndex = 0;
		this.VoucherInfoBox.Controls.Add(this.label36);
		this.VoucherInfoBox.Controls.Add(this.lblUnApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.lblTotalVouchers);
		this.VoucherInfoBox.Controls.Add(this.label38);
		this.VoucherInfoBox.Controls.Add(this.lblApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.label37);
		this.VoucherInfoBox.Location = new System.Drawing.Point(896, 5);
		this.VoucherInfoBox.Name = "VoucherInfoBox";
		this.VoucherInfoBox.Size = new System.Drawing.Size(242, 69);
		this.VoucherInfoBox.TabIndex = 1;
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
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatus);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Controls.Add(this.rdconapproved);
		this.groupBox3.Controls.Add(this.rdconmodify);
		this.groupBox3.Controls.Add(this.rdcondoc);
		this.groupBox3.Controls.Add(this.rdconentry);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.label23);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(891, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.CmbAccountTitleHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitleHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitleHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitleHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitleHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Location = new System.Drawing.Point(484, 12);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Name = "CmbAccountTitleHistory";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).TabIndex = 4;
		this.cmbApproveStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApproveStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Location = new System.Drawing.Point(484, 40);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Name = "cmbApproveStatus";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).TabIndex = 5;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(283, 42);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(283, 14);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(68, 42);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(132, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(68, 14);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(132, 23);
		this.FromDateHistory.TabIndex = 0;
		this.rdconapproved.AutoSize = true;
		this.rdconapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdconapproved.Location = new System.Drawing.Point(786, 35);
		this.rdconapproved.Name = "rdconapproved";
		this.rdconapproved.Size = new System.Drawing.Size(105, 19);
		this.rdconapproved.TabIndex = 10;
		this.rdconapproved.Text = "Approved Date";
		this.rdconapproved.UseVisualStyleBackColor = true;
		this.rdconmodify.AutoSize = true;
		this.rdconmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdconmodify.Location = new System.Drawing.Point(786, 15);
		this.rdconmodify.Name = "rdconmodify";
		this.rdconmodify.Size = new System.Drawing.Size(91, 19);
		this.rdconmodify.TabIndex = 9;
		this.rdconmodify.Text = "Modify Date";
		this.rdconmodify.UseVisualStyleBackColor = true;
		this.rdcondoc.AutoSize = true;
		this.rdcondoc.Checked = true;
		this.rdcondoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdcondoc.Location = new System.Drawing.Point(705, 15);
		this.rdcondoc.Name = "rdcondoc";
		this.rdcondoc.Size = new System.Drawing.Size(75, 19);
		this.rdcondoc.TabIndex = 7;
		this.rdcondoc.TabStop = true;
		this.rdcondoc.Text = "Doc Date";
		this.rdcondoc.UseVisualStyleBackColor = true;
		this.rdconentry.AutoSize = true;
		this.rdconentry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdconentry.Location = new System.Drawing.Point(705, 35);
		this.rdconentry.Name = "rdconentry";
		this.rdconentry.Size = new System.Drawing.Size(80, 19);
		this.rdconentry.TabIndex = 8;
		this.rdconentry.Text = "Entry Date";
		this.rdconentry.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(388, 18);
		this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(78, 15);
		this.label24.TabIndex = 147;
		this.label24.Text = "Account Title";
		this.btnShowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowHistory.FlatAppearance.BorderSize = 0;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowHistory.Location = new System.Drawing.Point(647, 40);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(53, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.btnShowHistory.Leave += new System.EventHandler(btnShowHistory_Leave);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(388, 46);
		this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(95, 15);
		this.label10.TabIndex = 142;
		this.label10.Text = "Approved Status";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(204, 46);
		this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(64, 15);
		this.label14.TabIndex = 141;
		this.label14.Text = "To Doc No";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(204, 18);
		this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(79, 15);
		this.label23.TabIndex = 140;
		this.label23.Text = "From Doc No";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(5, 46);
		this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(48, 15);
		this.label25.TabIndex = 139;
		this.label25.Text = "To Date";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(5, 18);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(63, 15);
		this.label26.TabIndex = 13;
		this.label26.Text = "From Date";
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label7);
		this.panel15.Controls.Add(this.ctrlGrdBar3);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 27);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1315, 28);
		this.panel15.TabIndex = 51;
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(5, 4);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(384, 19);
		this.label7.TabIndex = 22;
		this.label7.Text = "Intra-Company Fund Transfer Voucher History";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1315, 27);
		this.toolStrip3.TabIndex = 50;
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
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1273, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 19;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1275, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1327, 690);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ContraVoucher";
		this.Text = "Contra Voucher";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ContraVoucher_FormClosing);
		base.Load += new System.EventHandler(AcfrmPaymentVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ContraVoucher_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		this.groupboxdetail.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		this.DebitAccountPanel.ResumeLayout(false);
		this.DebitAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).EndInit();
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
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.DebitAmountPanel.ResumeLayout(false);
		this.DebitAmountPanel.PerformLayout();
		this.ReferenceAccountPanel.ResumeLayout(false);
		this.ReferenceAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceAccount).EndInit();
		this.BranchPanel.ResumeLayout(false);
		this.BranchPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		this.CostCenterPanel.ResumeLayout(false);
		this.CostCenterPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		this.ChequeDetailPanel.ResumeLayout(false);
		this.ChequeDetailPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCheqNo).EndInit();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.CmbWithHoldingAc).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAgainstAc).EndInit();
		this.History.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
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
