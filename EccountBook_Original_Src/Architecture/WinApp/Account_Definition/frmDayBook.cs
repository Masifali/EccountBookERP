using System;
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
using Architecture.Model.Common;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class frmDayBook : Form
{
	private bool MultiCurrencyFeatureVisibilty;

	private int BaseCurrency;

	private double BaseRate;

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable table = new DataTable();

	private Attachment AT = new Attachment();

	private int RecId;

	private int TemporaryAccountId;

	private int updateRowIndex;

	private int UniqueSubCodeForOneEntry = 1;

	private bool ChequeBookEnableStatus;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool InventoryRelatedAccountsShowInVouchers;

	private DataTable dtSubsidiary = new DataTable();

	private bool SubsidiaryAccountAllowOnVouchers;

	private int SubsidiaryAccountDrDatasourceLength;

	private bool AfterCloseUpDrSubsidiary;

	private int SubsidiaryAccountCrDatasourceLength;

	private bool AfterCloseUpCrSubsidiary;

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

	private Panel panel13;

	private ToolStripButton btnUpdate;

	private DateTimePicker txtDocDate;

	private ToolStripButton btnRefresh;

	private Panel panel6;

	private Label label18;

	internal GridEX grdDetail;

	internal GridEX DataGridCreditHistory;

	private TextBox txtCrRemarks;

	private Label lblCrRemarks;

	internal GridEX DataGridDebitHistory;

	private Label label51;

	private TextBox txtDocNo;

	private Label label4;

	internal UltraCombo cmbCashAccount;

	private Button btnAddDetail;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo cmbCreditAccount;

	private Label lblcreditAccount;

	private TextBox txtClosing;

	private Label label2;

	private TextBox txtOpening;

	private Label label1;

	internal UltraCombo cmbDebitAccount;

	private Label label11;

	private TextBox txtAmount;

	private Label lblAmount;

	private TextBox txtChequeRef;

	private Label lblChequeRef;

	private TextBox txtDrRemarks;

	private Label lblDrRemarks;

	private ToolStripButton btnAttachment;

	private Button btnUpdateDetail;

	private Button btnCancelDetail;

	private Label label14;

	private Label label15;

	internal UltraCombo CmbCashAccountForHistory;

	private Button BtnShow;

	private Panel panel8;

	private Panel panel12;

	private Panel panel5;

	private Panel panel11;

	private Panel panel10;

	private ToolStrip toolStrip1;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkBoxPrintPreview;

	private ToolStripButton btnPrint144;

	private Label DrBalance;

	private Label CrBalance;

	private TextBox txtTotalDrAmount;

	private Label lblDrAmount;

	private TextBox txtTotalCrAmount;

	private Label lblCrTotal;

	private TextBox txtSubCode;

	private Label lblSubCode;

	internal UltraCombo CmbTemporaryAccount;

	private Label lblTemporaryAccount;

	internal UltraCombo CmbChequeNo;

	private Label lblChequeNo;

	private Panel panel9;

	private Label label6;

	private Panel panel7;

	private Label label5;

	private CheckBox chkZkipCashTransaction;

	private CheckBox chkformat2;

	private GroupBox groupBox2;

	private GroupBox MainBox;

	private Label label7;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

	private TextBox txtExchangeRate;

	private Label lblExchangeRate;

	private Label lblFcyAmount;

	private TextBox txtFcyAmount;

	private Label label3;

	private ToolStripButton btnShortCutKey;

	private Label label8;

	private TextBox txtVoucherRemarks;

	private Label lblDrSubsidiaryaccount;

	internal UltraCombo CmbCrSubsidiaryAccount;

	private Label lblCrSubsidiaryaccount;

	internal UltraCombo CmbDrSubsidiaryAccount;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel SubCodePanel;

	private Panel CreditAccountPanel;

	private Panel CreditSubsidiaryPanel;

	private Panel CrTotalAmountPanel;

	private Panel CrRemarksPanel;

	private Panel CheqNoPanel;

	private Panel AmountPanel;

	private Panel DebitAccountPanel;

	private Panel DrTotalAmountPanel;

	private Panel DebitSubsidiaryAccountPanel;

	private Panel DrRemarksPanel;

	private Panel ButtonsPanel;

	internal UltraCombo datVouchderDateForHistory;

	private ToolStripButton btnRefreshHistory;

	public frmDayBook(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			MultiCurrencyFeatureVisibilty = CommonServices.GetERPFeatureById(6);
			if (MultiCurrencyFeatureVisibilty)
			{
				lblFcyCode.Visible = true;
				((Control)(object)cmbCurrency).Visible = true;
				lblExchangeRate.Visible = true;
				txtExchangeRate.Visible = true;
				lblFcyAmount.Visible = true;
				txtFcyAmount.Visible = true;
				ChkBoxPrintPreview.Location = new Point(990, 56);
				chkformat2.Location = new Point(990, 79);
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				lblFcyCode.Visible = false;
				lblExchangeRate.Visible = false;
				lblFcyAmount.Visible = false;
				ChkBoxPrintPreview.Location = new Point(771, 56);
				chkformat2.Location = new Point(771, 79);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDayBook_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1350, 600);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			btnPrint144.Enabled = formrights.DoHavePrintRights;
			flowLayoutPanel1.Margin = new Padding(0, 0, 0, 0);
			ChequeBookEnableStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheqBook Enabled"));
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			GenerateCode();
			CashAccountFill();
			table.Columns.Add("SubCode");
			table.Columns.Add("AccountCode");
			table.Columns.Add("AccountId");
			table.Columns.Add("AccountTitle");
			table.Columns.Add("AgainstAccountId");
			table.Columns.Add("SubsidiaryAccountId");
			table.Columns.Add("SubsidiaryAccount");
			table.Columns.Add("SubsidiaryAccountTypeId");
			table.Columns.Add("Remarks");
			table.Columns.Add("CheqNo");
			table.Columns.Add("TotalAmountDr", typeof(double));
			table.Columns.Add("TotalAmountCr", typeof(double));
			table.Columns.Add("AmountDr", typeof(double));
			table.Columns.Add("AmountCr", typeof(double));
			table.Columns.Add("LineId");
			table.Columns.Add("CheqId");
			table.Columns.Add("DebitFcyAmount", typeof(double));
			table.Columns.Add("CreditFcyAmount", typeof(double));
			grdDetail.DataSource = table;
			txtSubCode.Text = Conversion.ToString((object)UniqueSubCodeForOneEntry);
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			CrBalance.Visible = false;
			DrBalance.Visible = false;
			txtDocDate.Focus();
			AccountsFill();
			CurrencyFill();
			MultiCurrencyFeature();
			if (ChequeBookEnableStatus)
			{
				lblChequeNo.Visible = true;
				((Control)(object)CmbChequeNo).Visible = true;
				lblChequeRef.Visible = false;
				txtChequeRef.Visible = false;
			}
			else
			{
				lblChequeNo.Visible = false;
				((Control)(object)CmbChequeNo).Visible = false;
				lblChequeRef.Visible = true;
				txtChequeRef.Visible = true;
			}
			if (TemporaryAccountId > 0)
			{
				lblTemporaryAccount.Visible = true;
				((Control)(object)CmbTemporaryAccount).Visible = true;
				MainBox.Size = new Size(1239, 138);
				txtVoucherRemarks.Size = new Size(319, 108);
				SubCodePanel.Visible = true;
				CrTotalAmountPanel.Visible = true;
				DrTotalAmountPanel.Visible = true;
			}
			else
			{
				lblTemporaryAccount.Visible = false;
				((Control)(object)CmbTemporaryAccount).Visible = false;
				MainBox.Size = new Size(1239, 122);
				txtVoucherRemarks.Size = new Size(319, 78);
				SubCodePanel.Visible = false;
				CrTotalAmountPanel.Visible = false;
				DrTotalAmountPanel.Visible = false;
			}
			SubsidiaryAccountAllowOnVouchers = CommonServices.GetERPFeatureById(4);
			if (SubsidiaryAccountAllowOnVouchers)
			{
				BindAllSubsidiaryaccounts();
				CreditSubsidiaryPanel.Visible = true;
				DebitSubsidiaryAccountPanel.Visible = true;
				lblCrSubsidiaryaccount.Visible = true;
				((Control)(object)CmbCrSubsidiaryAccount).Visible = true;
				lblDrSubsidiaryaccount.Visible = true;
				((Control)(object)CmbDrSubsidiaryAccount).Visible = true;
			}
			else
			{
				CreditSubsidiaryPanel.Visible = false;
				DebitSubsidiaryAccountPanel.Visible = false;
				lblCrSubsidiaryaccount.Visible = false;
				((Control)(object)CmbCrSubsidiaryAccount).Visible = false;
				lblDrSubsidiaryaccount.Visible = false;
				((Control)(object)CmbDrSubsidiaryAccount).Visible = false;
			}
			if (!SubsidiaryAccountAllowOnVouchers && TemporaryAccountId <= 0)
			{
				CreditAccountPanel.Size = new Size(290, 47);
				((Control)(object)cmbCreditAccount).Size = new Size(288, 26);
				CrRemarksPanel.Size = new Size(600, 47);
				txtCrRemarks.Size = new Size(595, 23);
				DrRemarksPanel.Size = new Size(600, 47);
				txtDrRemarks.Size = new Size(595, 23);
			}
			if (SubsidiaryAccountAllowOnVouchers && TemporaryAccountId <= 0)
			{
				CreditAccountPanel.Size = new Size(290, 47);
				((Control)(object)cmbCreditAccount).Size = new Size(288, 26);
			}
			GetVoucherDateDropdownForHistory();
			DefaultConfigurations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetVoucherDateDropdownForHistory()
	{
		try
		{
			DataTable dt = CommonServices.GetVoucherDateForDaybookDropdown(8);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, datVouchderDateForHistory, "Id", "VoucherDate", "Voucher Date", ZeroIndex: false);
				return;
			}
			((Control)(object)datVouchderDateForHistory).Text = string.Empty;
			((UltraGridBase)datVouchderDateForHistory).DataSource = null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GenerateCode()
	{
		try
		{
			txtDocNo.Text = Conversion.ToString((object)CommonServices.GenerateVoucherCode(8));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CashAccountFill()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 2 });
			InfragisticsHelper.BindAndRetainSelection(cmbCashAccount, dt, "Id", "AccountTitle", "Cash Account", AllColumns: true, Conversion.ToInt(cmbCashAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCashAccountForHistory, dt, "Id", "AccountTitle", "Cash Account", AllColumns: true, Conversion.ToInt(CmbCashAccountForHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbCurrency.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "FcyCode", true);
			((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbCurrency.Value = Id;
				}
				else
				{
					((Control)(object)cmbCurrency).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			TemporaryAccountId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DayBookAcTemporary"));
			int DebitId = Conversion.ToInt(cmbDebitAccount.Value);
			int CreditId = Conversion.ToInt(cmbCreditAccount.Value);
			int[] baseExclude = ((!InventoryRelatedAccountsShowInVouchers) ? new int[3]
			{
				4,
				12,
				(!MultiCurrencyFeatureVisibilty) ? 22 : 0
			} : new int[1] { (!MultiCurrencyFeatureVisibilty) ? 22 : 0 });
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, baseExclude, null, null, null, null, InventoryRelatedAccountsShowInVouchers ? null : new int[1] { 2 });
			InfragisticsHelper.BindAndRetainSelection(cmbDebitAccount, dt, "Id", "AccountTitle", "Debit Account", AllColumns: true, DebitId, insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbCreditAccount, dt, "Id", "AccountTitle", "Credit Account", AllColumns: true, CreditId, insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbTemporaryAccount, dt, "Id", "AccountTitle", "Temporary Account", AllColumns: true, Conversion.ToInt(CmbTemporaryAccount.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			if (TemporaryAccountId > 0)
			{
				CmbTemporaryAccount.Value = TemporaryAccountId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindAllSubsidiaryaccounts()
	{
		try
		{
			dtSubsidiary = VoucherHead.BindSubsidiaryAccount(UserAccount.CompanyId, 0, (string)null);
			if (dtSubsidiary.Rows.Count > 0)
			{
				DDL.BindDDL(dtSubsidiary, CmbCrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
				((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				DDL.BindDDL(dtSubsidiary, CmbDrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
				((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DefaultConfigurations()
	{
		try
		{
			int CurrencyId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
			double BaseCurrencyRate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BaseCurrencyRate"));
			if (CurrencyId > 0)
			{
				BaseCurrency = CurrencyId;
				cmbCurrency.Value = BaseCurrency;
			}
			if (BaseCurrencyRate > 0.0)
			{
				BaseRate = BaseCurrencyRate;
				txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNo.Text == string.Empty)
		{
			MessageBox.Show("DocNo Required");
			txtDocNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCashAccount).ActiveRow == null || Conversion.ToInt(cmbCashAccount.Value) == 0)
		{
			MessageBox.Show("Cash Account Required");
			cmbCashAccount.Focus();
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
		if (((UltraGridBase)cmbCreditAccount).ActiveRow == null || Conversion.ToInt(cmbCreditAccount.Value) == 0)
		{
			MessageBox.Show("Credit Account Field Required");
			cmbCreditAccount.Focus();
			return false;
		}
		if (SubsidiaryAccountAllowOnVouchers && SubsidiaryAccountCrDatasourceLength > 0 && ((UltraGridBase)CmbCrSubsidiaryAccount).ActiveRow == null)
		{
			MessageBox.Show("Credit Subsidiary Account Field Required");
			CmbCrSubsidiaryAccount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDebitAccount).ActiveRow == null || Conversion.ToInt(cmbDebitAccount.Value) == 0)
		{
			MessageBox.Show("Debit Account Field Required");
			cmbDebitAccount.Focus();
			return false;
		}
		if (SubsidiaryAccountAllowOnVouchers && SubsidiaryAccountDrDatasourceLength > 0 && ((UltraGridBase)CmbDrSubsidiaryAccount).ActiveRow == null)
		{
			MessageBox.Show("Debit Subsidiary Account Field Required");
			CmbDrSubsidiaryAccount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount Field Required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			double Amount = 0.0;
			double TotalCreditAmount = 0.0;
			double TotalDeditAmount = 0.0;
			double RemainingAmount = 0.0;
			if (txtTotalCrAmount.Text == string.Empty && txtTotalDrAmount.Text == string.Empty)
			{
				txtTotalCrAmount.Text = "0";
				txtTotalDrAmount.Text = "0";
			}
			if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0 && Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
			{
				MessageBox.Show("You Can't Enter Total Credit Amount And Total Debit Amount At Same Time");
				return;
			}
			if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
			{
				txtTotalDrAmount.Text = "0";
			}
			else if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
			{
				txtTotalCrAmount.Text = "0";
			}
			if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
			{
				if (Conversion.ToDouble((object)txtAmount.Text) > Conversion.ToDouble((object)txtTotalCrAmount.Text))
				{
					MessageBox.Show("Amount Shouldn't Greater Than Total Credit Amount");
					return;
				}
			}
			else if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0 && Conversion.ToDouble((object)txtAmount.Text) > Conversion.ToDouble((object)txtTotalDrAmount.Text))
			{
				MessageBox.Show("Amount Shouldn't Greater Than Total Debit Amount");
				return;
			}
			int UniqueIdToRecognizeCreditDebitRowInDetail = 1;
			if (table.Rows.Count > 0)
			{
				UniqueIdToRecognizeCreditDebitRowInDetail = Conversion.ToInt((object)grdDetail.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["LineId"].Value)));
				UniqueIdToRecognizeCreditDebitRowInDetail++;
				if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
				{
					if (Conversion.ToDouble((object)txtAmount.Text) > Conversion.ToDouble((object)txtTotalCrAmount.Text))
					{
						MessageBox.Show("Amount Shouldn't Greater Than Total Credit Amount");
						return;
					}
				}
				else if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0 && Conversion.ToDouble((object)txtAmount.Text) > Conversion.ToDouble((object)txtTotalDrAmount.Text))
				{
					MessageBox.Show("Amount Shouldn't Greater Than Total Debit Amount");
					return;
				}
				int CreditAccountId = 0;
				int DebitAccountId = 0;
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (Conversion.ToInt(table.Rows[i]["SubCode"]) == Conversion.ToInt((object)txtSubCode.Text))
					{
						if ((i + 1) % 2 != 0)
						{
							CreditAccountId = Conversion.ToInt(table.Rows[i]["AccountId"]);
						}
						else
						{
							DebitAccountId = Conversion.ToInt(table.Rows[i]["AccountId"]);
						}
						TotalCreditAmount = Conversion.ToDouble(table.Rows[i]["TotalAmountCr"]);
						TotalDeditAmount = Conversion.ToDouble(table.Rows[i]["TotalAmountDr"]);
						Amount += (double)Conversion.ToInt(table.Rows[i]["AmountCr"]);
					}
				}
				if (TotalCreditAmount > 0.0 && Amount != TotalCreditAmount)
				{
					RemainingAmount = TotalCreditAmount - Amount;
					if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
					{
						MessageBox.Show("You Can't Change The Credit Total Amount To Debit Total Amount");
						txtTotalCrAmount.Text = Conversion.ToString((object)TotalCreditAmount);
						return;
					}
					if (Conversion.ToDouble((object)txtTotalCrAmount.Text) != Conversion.ToDouble((object)TotalCreditAmount))
					{
						txtTotalCrAmount.Text = Conversion.ToString((object)TotalCreditAmount);
						MessageBox.Show("You Can't Change The Credit Total Amount");
						return;
					}
					if (Conversion.ToInt(cmbCreditAccount.Value) != CreditAccountId)
					{
						MessageBox.Show("You Can't Change The Credit Account");
						cmbCreditAccount.Value = CreditAccountId;
						return;
					}
					if (Conversion.ToDouble((object)txtAmount.Text) > RemainingAmount)
					{
						MessageBox.Show("Amount Shouldn't Greater Than The Remaining Amount: " + RemainingAmount + " Of Total Credit Amount");
						return;
					}
				}
				else if (TotalDeditAmount > 0.0 && Amount != TotalDeditAmount)
				{
					RemainingAmount = TotalDeditAmount - Amount;
					if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
					{
						txtTotalDrAmount.Text = Conversion.ToString((object)TotalDeditAmount);
						MessageBox.Show("You Can't Change The Debit Total Amount To Credit Total Amount");
						return;
					}
					if (Conversion.ToDouble((object)txtTotalCrAmount.Text) != TotalCreditAmount)
					{
						txtTotalDrAmount.Text = Conversion.ToString((object)TotalDeditAmount);
						MessageBox.Show("You Can't Change The Debit Total Amount");
						return;
					}
					if (Conversion.ToInt(cmbDebitAccount.Value) != DebitAccountId)
					{
						MessageBox.Show("You Can't Change The Debit Account");
						return;
					}
					if (Conversion.ToDouble((object)txtAmount.Text) > RemainingAmount)
					{
						MessageBox.Show("Amount Shouldn't Greater Than The Remaining Amount: " + RemainingAmount + " Of Total Debit Amount");
						return;
					}
				}
			}
			table.Rows.Add(Conversion.ToInt((object)txtSubCode.Text), ((UltraDropDownBase)cmbCreditAccount).SelectedRow.Cells[3].Value, cmbCreditAccount.Value, ((Control)(object)cmbCreditAccount).Text, cmbDebitAccount.Value, (!SubsidiaryAccountAllowOnVouchers) ? ((object)0) : ((SubsidiaryAccountCrDatasourceLength > 0) ? CmbCrSubsidiaryAccount.Value : cmbCreditAccount.Value), (!SubsidiaryAccountAllowOnVouchers) ? "" : ((SubsidiaryAccountCrDatasourceLength > 0) ? ((Control)(object)CmbCrSubsidiaryAccount).Text : ((Control)(object)cmbCreditAccount).Text), (!SubsidiaryAccountAllowOnVouchers) ? ((object)0) : ((SubsidiaryAccountCrDatasourceLength > 0) ? ((UltraDropDownBase)CmbCrSubsidiaryAccount).SelectedRow.Cells[2].Value : ((object)4)), txtCrRemarks.Text.Trim(), ChequeBookEnableStatus ? ((Control)(object)CmbChequeNo).Text : txtChequeRef.Text.Trim(), Conversion.ToDouble((object)txtTotalDrAmount.Text), Conversion.ToDouble((object)txtTotalCrAmount.Text), 0, txtAmount.Text.Trim(), UniqueIdToRecognizeCreditDebitRowInDetail, ChequeBookEnableStatus ? CmbChequeNo.Value : ((object)0), 0, (Conversion.ToDouble((object)txtExchangeRate.Text) > 0.0 && Conversion.ToDouble((object)txtAmount.Text) > 0.0) ? (Conversion.ToDouble((object)txtAmount.Text) / Conversion.ToDouble((object)txtExchangeRate.Text)) : 0.0);
			table.Rows.Add(Conversion.ToInt((object)txtSubCode.Text), ((UltraDropDownBase)cmbDebitAccount).SelectedRow.Cells[3].Value, cmbDebitAccount.Value, ((Control)(object)cmbDebitAccount).Text, cmbCreditAccount.Value, (!SubsidiaryAccountAllowOnVouchers) ? ((object)0) : ((SubsidiaryAccountDrDatasourceLength > 0) ? CmbDrSubsidiaryAccount.Value : cmbDebitAccount.Value), (!SubsidiaryAccountAllowOnVouchers) ? "" : ((SubsidiaryAccountDrDatasourceLength > 0) ? ((Control)(object)CmbDrSubsidiaryAccount).Text : ((Control)(object)cmbDebitAccount).Text), (!SubsidiaryAccountAllowOnVouchers) ? ((object)0) : ((SubsidiaryAccountDrDatasourceLength > 0) ? ((UltraDropDownBase)CmbDrSubsidiaryAccount).SelectedRow.Cells[2].Value : ((object)4)), txtDrRemarks.Text.Trim(), ChequeBookEnableStatus ? ((Control)(object)CmbChequeNo).Text : txtChequeRef.Text.Trim(), Conversion.ToDouble((object)txtTotalDrAmount.Text), Conversion.ToDouble((object)txtTotalCrAmount.Text), txtAmount.Text.Trim(), 0, UniqueIdToRecognizeCreditDebitRowInDetail + 1, ChequeBookEnableStatus ? CmbChequeNo.Value : ((object)0), (Conversion.ToDouble((object)txtExchangeRate.Text) > 0.0 && Conversion.ToDouble((object)txtAmount.Text) > 0.0) ? (Conversion.ToDouble((object)txtAmount.Text) / Conversion.ToDouble((object)txtExchangeRate.Text)) : 0.0, 0);
			if (Conversion.ToDouble((object)txtAmount.Text) == RemainingAmount)
			{
				txtTotalCrAmount.Text = "0";
				txtTotalDrAmount.Text = "0";
			}
			grdDetail.DataSource = table;
			grdDetail.RetrieveStructure();
			if (Conversion.ToDouble((object)txtTotalCrAmount.Text) == 0.0 && Conversion.ToDouble((object)txtTotalDrAmount.Text) == 0.0)
			{
				UniqueSubCodeForOneEntry = Conversion.ToInt((object)grdDetail.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["SubCode"].Value)));
				UniqueSubCodeForOneEntry++;
				((Control)(object)cmbCreditAccount).Text = string.Empty;
				((Control)(object)cmbDebitAccount).Text = string.Empty;
			}
			else if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
			{
				((Control)(object)cmbDebitAccount).Text = string.Empty;
			}
			else if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
			{
				((Control)(object)cmbCreditAccount).Text = string.Empty;
			}
			txtSubCode.Text = Conversion.ToString((object)UniqueSubCodeForOneEntry);
			gridsetting();
			ResetDetail();
			cmbCreditAccount.Focus();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grdDetail.AllowEdit = (InheritableBoolean)2;
			grdDetail.AllowDelete = (InheritableBoolean)2;
			grdDetail.RootTable.Columns["AccountId"].Visible = false;
			grdDetail.RootTable.Columns["LineId"].Visible = false;
			grdDetail.RootTable.Columns["CheqId"].Visible = false;
			grdDetail.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grdDetail.RootTable.Columns["SubsidiaryAccountTypeId"].Visible = false;
			grdDetail.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllowOnVouchers;
			grdDetail.RootTable.Columns["AgainstAccountId"].Visible = false;
			grdDetail.RootTable.Columns["DebitFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			grdDetail.RootTable.Columns["CreditFcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			grdDetail.RootTable.Columns["SubCode"].Visible = TemporaryAccountId > 0;
			grdDetail.RootTable.Columns["TotalAmountDr"].Visible = TemporaryAccountId > 0;
			grdDetail.RootTable.Columns["TotalAmountCr"].Visible = TemporaryAccountId > 0;
			grdDetail.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AmountDr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountDr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AmountCr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["AmountCr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["DebitFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DebitFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DebitFcyAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["DebitFcyAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["CreditFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CreditFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CreditFcyAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["CreditFcyAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Caption = "Delete";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdDetail.RootTable.Columns["Delete"].Width = 50;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0 || Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
			{
				MessageBox.Show("There Is A Stand Entry In Detail Record. Please Complete that Entry First");
			}
			else if (grdDetail.RowCount > 0)
			{
				VoucherHead vh = new VoucherHead();
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					vh.Id = RecId;
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				double TempDebit = 0.0;
				double TempCredit = 0.0;
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (Conversion.ToDouble(r3.Cells["AmountDr"].Value) > 0.0 && Conversion.ToInt(r3.Cells["AccountId"].Value) == 0)
					{
						MessageBox.Show("Please Select Debit Account Title First");
						return;
					}
					if (Conversion.ToInt(r3.Cells["AccountId"].Value) == Conversion.ToInt(CmbTemporaryAccount.Value))
					{
						TempDebit += (double)Conversion.ToInt(r3.Cells["AmountDr"].Value);
						TempCredit += (double)Conversion.ToInt(r3.Cells["AmountCr"].Value);
					}
				}
				if (TempDebit != TempCredit)
				{
					throw new Exception("Temporary Account Debit And Credit not equal.Debit is " + TempDebit + " and Credit is " + TempCredit);
				}
				rows = grdDetail.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToDouble(r2.Cells["AmountCr"].Value) > 0.0 && Conversion.ToInt(r2.Cells["AccountId"].Value) == 0)
					{
						MessageBox.Show("Please Select Credit Account Title First");
						return;
					}
				}
				vh.DocumentTypeId = 8;
				vh.DocumentTypeSrNo = 0;
				vh.VoucherCode = Conversion.ToInt((object)txtDocNo.Text.Trim());
				vh.VoucherDate = txtDocDate.Value;
				vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				vh.RefAccountId = Conversion.ToInt(cmbCashAccount.Value);
				vh.EntryDate = DateTime.Now;
				vh.EntryUser = UserAccount.ID;
				vh.ModifyDate = DateTime.Now;
				vh.ModifyUser = UserAccount.ID;
				vh.OrganizationId = UserAccount.OrganizationId;
				vh.CompanyId = UserAccount.CompanyId;
				vh.BranchId = UserAccount.BranchesId;
				vh.Remarks = txtVoucherRemarks.Text;
				vh.MultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
				vh.ExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				vh.FcAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
				vh.voucherDetailList = new List<VoucherDetail>();
				rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					VoucherDetail vd = new VoucherDetail();
					vd.SubNo = Conversion.ToInt(r.Cells["SubCode"].Value);
					vd.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					vd.AccountTitle = Conversion.ToString(r.Cells["AccountTitle"].Value);
					vd.AgainstAccountId = Conversion.ToInt(r.Cells["AgainstAccountId"].Value);
					vd.SubsidiaryTypeId = Conversion.ToInt(r.Cells["SubsidiaryAccountTypeId"].Value);
					int SubsidiaryAccountId = Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
					if (SubsidiaryAccountId > 0)
					{
						vd.SupplierCustomerId = ((vd.SubsidiaryTypeId == 1) ? SubsidiaryAccountId : 0);
						vd.EmployeeId = ((vd.SubsidiaryTypeId == 2) ? SubsidiaryAccountId : 0);
						vd.SubsidiaryAccountId = SubsidiaryAccountId;
					}
					vd.Comments = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.CheqNoDetail = Conversion.ToString(r.Cells["CheqNo"].Value);
					vd.InvoiceNoRefId = Conversion.ToInt(r.Cells["CheqId"].Value);
					vd.TotalDebitAmount = Conversion.ToDouble(r.Cells["TotalAmountDr"].Value);
					vd.TotalCreditAmount = Conversion.ToDouble(r.Cells["TotalAmountCr"].Value);
					vd.DebitAmount = Conversion.ToDouble(r.Cells["AmountDr"].Value);
					vd.CreditAmount = Conversion.ToDouble(r.Cells["AmountCr"].Value);
					vd.LineId = Conversion.ToInt(r.Cells["LineId"].Value);
					vd.DMultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
					vd.DExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
					vd.DCurrencyAmount = ((vd.DebitAmount > 0.0) ? Conversion.ToDouble(r.Cells["DebitFcyAmount"].Value) : Conversion.ToDouble(r.Cells["CreditFcyAmount"].Value));
					vh.voucherDetailList.Add(vd);
				}
				object Debit = grdDetail.GetTotal(grdDetail.RootTable.Columns["AmountDr"], (AggregateFunction)2);
				object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["AmountCr"], (AggregateFunction)2);
				double DebitAmount = Conversion.ToDouble(Debit);
				double CreditAmount = Conversion.ToDouble(total);
				if (CreditAmount != DebitAmount)
				{
					MessageBox.Show("Debit & Credit side Must Be equal");
					return;
				}
				vh.VoucherAmount = DebitAmount;
				vh.BillAmount = CreditAmount;
				if (string.IsNullOrEmpty(AttachmentsValues) || Attachment.dtAttachments.Rows.Count > 0)
				{
					AttachmentsValues = "";
				}
				vh.AttachmentsList = new List<DMSAttachments>();
				vh.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<VoucherHead>(vh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				vh.AttachmentsValues = AttachmentsValues;
				vh.CustomAttachmentsValues = CustomAttachmentsValues;
				if (!VoucherHead.VoucherExistWithSameAmountInSameDate(vh))
				{
					return;
				}
				int success = VoucherHead.Save(vh);
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully...[" + vh.VoucherCode + "]");
				}
				else
				{
					MessageBox.Show("Record Update Successfully...[" + vh.VoucherCode + "]");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 8, 0);
					}
				}
				Reset();
				if (ChkBoxPrintPreview.Checked)
				{
					DayBookSlip(success);
				}
				else if (chkformat2.Checked)
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(success, 8);
				}
			}
			else
			{
				MessageBox.Show("Grid Fields Required");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		if (RecId == 0)
		{
			throw new Exception("Record Id Not Found");
		}
		btnsave_Click(null, null);
	}

	private void frmDayBook_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtDocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					datVouchderDateForHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click_1(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint144_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					txtDocDate.Focus();
				}
				else
				{
					datVouchderDateForHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbCreditAccount.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right && tabControl1.SelectedIndex == 1)
			{
				if (((Control)(object)DataGridCreditHistory).Focused)
				{
					((Control)(object)DataGridDebitHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridCreditHistory).Focus();
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

	private void btnRefresh_Click_1(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			CurrencyFill();
			CashAccountFill();
			AccountsFill();
			DefaultConfigurations();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
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

	private void cmbCreditAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			double bal = CommonServices.GetCurrentGlBalance(Conversion.ToInt(cmbCreditAccount.Value), DateTime.Now);
			CrBalance.Visible = true;
			CrBalance.Text = bal.ToString(clsGlobalVariables.stringFormatboth);
			if (ChequeBookEnableStatus)
			{
				if (((UltraGridBase)cmbCreditAccount).ActiveRow != null && Conversion.ToInt(((UltraDropDownBase)cmbCreditAccount).SelectedRow.Cells[2].Value) == 15)
				{
					BindChequeNo();
				}
				else
				{
					((Control)(object)CmbChequeNo).Text = string.Empty;
					((UltraGridBase)CmbChequeNo).DataSource = null;
				}
			}
			if (((UltraGridBase)cmbCreditAccount).ActiveRow != null)
			{
				BindCreditSubsidiaryAccount();
			}
			else
			{
				BindAllSubsidiaryaccounts();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindChequeNo()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbChequeNo).Text = string.Empty;
			DataTable dtCheq = CheqBookHeader.OutstandingCheqNo(new CheqBookHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BankId = Conversion.ToInt(cmbCreditAccount.Value)
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
					((UltraGridBase)CmbChequeNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
			else
			{
				((Control)(object)CmbChequeNo).Text = string.Empty;
				((UltraGridBase)CmbChequeNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbDebitAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			double bal = CommonServices.GetCurrentGlBalance(Conversion.ToInt(cmbDebitAccount.Value), DateTime.Now);
			DrBalance.Visible = true;
			DrBalance.Text = bal.ToString(clsGlobalVariables.stringFormatboth);
			if (((UltraGridBase)cmbDebitAccount).ActiveRow != null)
			{
				BindDebitSubsidiaryAccount();
			}
			else
			{
				BindAllSubsidiaryaccounts();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbCashAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			double op = CommonServices.GetCurrentGlBalance(Conversion.ToInt(cmbCashAccount.Value), Conversion.ToDateTime((object)txtDocDate.Text).AddDays(-1.0));
			txtOpening.Text = op.ToString(clsGlobalVariables.stringFormatboth);
			double closing = CommonServices.GetCurrentGlBalance(Conversion.ToInt(cmbCashAccount.Value), Conversion.ToDateTime((object)txtDocDate.Text));
			txtClosing.Text = closing.ToString(clsGlobalVariables.stringFormatboth);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCrRemarks_Leave(object sender, EventArgs e)
	{
		try
		{
			txtDrRemarks.Text = txtCrRemarks.Text;
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

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			btnAddDetail.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelDetail.Visible = true;
			txtTotalCrAmount.Enabled = false;
			txtTotalDrAmount.Enabled = false;
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null)
			{
				return;
			}
			int UniqueId = Conversion.ToInt(item.Cells["LineId"].Value);
			if (UniqueId % 2 == 0)
			{
				DataRow[] CrObject = table.Select("LineId='" + Conversion.ToInt((object)(UniqueId - 1)) + "'");
				cmbCreditAccount.Value = CrObject[0].ItemArray[2];
				cmbCreditAccount_Leave(null, null);
				cmbDebitAccount.Value = CrObject[0].ItemArray[4];
				if (SubsidiaryAccountCrDatasourceLength == 0)
				{
					DataTable dt2 = new DataTable();
					dt2.Columns.Add("Id");
					dt2.Columns.Add("SubsidiaryAccount");
					dt2.Columns.Add("TypeId");
					dt2.Columns.Add("AccountId");
					dt2.Columns.Add("AccountTitle");
					dt2.Rows.Add(Conversion.ToInt(CrObject[0].ItemArray[5]), CrObject[0].ItemArray[6], 4, CrObject[0].ItemArray[2], CrObject[0].ItemArray[3]);
					DDL.BindDDL(dt2, CmbCrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (Conversion.ToInt(CrObject[0].ItemArray[5]) > 0)
				{
					CmbDrSubsidiaryAccount.Value = Conversion.ToInt(CrObject[0].ItemArray[5]);
				}
				if (ChequeBookEnableStatus)
				{
					((Control)(object)CmbChequeNo).Text = Conversion.ToString(CrObject[0].ItemArray[9]);
				}
				else
				{
					txtChequeRef.Text = Conversion.ToString(CrObject[0].ItemArray[9]);
				}
				txtCrRemarks.Text = Conversion.ToString(CrObject[0].ItemArray[8]);
				txtTotalCrAmount.Text = Conversion.ToString(CrObject[0].ItemArray[11]);
				txtAmount.Text = Conversion.ToString(CrObject[0].ItemArray[13]);
				txtSubCode.Text = Conversion.ToString((object)item.Cells["SubCode"].Text);
				cmbCreditAccount.Value = item.Cells["AgainstAccountId"].Text.ToString();
				cmbDebitAccount.Value = item.Cells["AccountId"].Text.ToString();
				cmbDebitAccount_Leave(null, null);
				if (SubsidiaryAccountDrDatasourceLength == 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("SubsidiaryAccount");
					dt.Columns.Add("TypeId");
					dt.Columns.Add("AccountId");
					dt.Columns.Add("AccountTitle");
					dt.Rows.Add(Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value), item.Cells["SubsidiaryAccount"].Text, 4, item.Cells["AccountId"].Text, item.Cells["AccountTitle"].Text);
					DDL.BindDDL(dt, CmbDrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value) > 0)
				{
					CmbDrSubsidiaryAccount.Value = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
				}
				if (ChequeBookEnableStatus)
				{
					((Control)(object)CmbChequeNo).Text = Conversion.ToString((object)item.Cells["CheqNo"].Text);
				}
				else
				{
					txtChequeRef.Text = Conversion.ToString((object)item.Cells["CheqNo"].Text);
				}
				txtDrRemarks.Text = Conversion.ToString((object)item.Cells["Remarks"].Text);
				txtTotalDrAmount.Text = Conversion.ToString((object)item.Cells["TotalAmountDr"].Text);
				txtAmount.Text = item.Cells["AmountDr"].Text.ToString();
			}
			else
			{
				DataRow[] DrObject = table.Select("LineId='" + Conversion.ToInt((object)(UniqueId + 1)) + "'");
				cmbDebitAccount.Value = DrObject[0].ItemArray[2];
				cmbDebitAccount_Leave(null, null);
				cmbCreditAccount.Value = DrObject[0].ItemArray[4];
				if (SubsidiaryAccountDrDatasourceLength == 0)
				{
					DataTable dt4 = new DataTable();
					dt4.Columns.Add("Id");
					dt4.Columns.Add("SubsidiaryAccount");
					dt4.Columns.Add("TypeId");
					dt4.Columns.Add("AccountId");
					dt4.Columns.Add("AccountTitle");
					dt4.Rows.Add(Conversion.ToInt(DrObject[0].ItemArray[5]), DrObject[0].ItemArray[6], 4, DrObject[0].ItemArray[2], DrObject[0].ItemArray[3]);
					DDL.BindDDL(dt4, CmbDrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (Conversion.ToInt(DrObject[0].ItemArray[5]) > 0)
				{
					CmbDrSubsidiaryAccount.Value = Conversion.ToInt(DrObject[0].ItemArray[5]);
				}
				txtDrRemarks.Text = Conversion.ToString(DrObject[0].ItemArray[8]);
				if (ChequeBookEnableStatus)
				{
					((Control)(object)CmbChequeNo).Text = Conversion.ToString(DrObject[0].ItemArray[9]);
				}
				else
				{
					txtChequeRef.Text = Conversion.ToString(DrObject[0].ItemArray[9]);
				}
				txtTotalDrAmount.Text = Conversion.ToString(DrObject[0].ItemArray[10]);
				txtAmount.Text = Conversion.ToString(DrObject[0].ItemArray[12]);
				txtSubCode.Text = Conversion.ToString((object)item.Cells["SubCode"].Text);
				cmbCreditAccount.Value = item.Cells["AccountId"].Text.ToString();
				cmbCreditAccount_Leave(null, null);
				cmbDebitAccount.Value = item.Cells["AgainstAccountId"].Text.ToString();
				if (SubsidiaryAccountCrDatasourceLength == 0)
				{
					DataTable dt3 = new DataTable();
					dt3.Columns.Add("Id");
					dt3.Columns.Add("SubsidiaryAccount");
					dt3.Columns.Add("TypeId");
					dt3.Columns.Add("AccountId");
					dt3.Columns.Add("AccountTitle");
					dt3.Rows.Add(Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value), item.Cells["SubsidiaryAccount"].Text, 4, item.Cells["AccountId"].Text, item.Cells["AccountTitle"].Text);
					DDL.BindDDL(dt3, CmbCrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value) > 0)
				{
					CmbCrSubsidiaryAccount.Value = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
				}
				if (ChequeBookEnableStatus)
				{
					((Control)(object)CmbChequeNo).Text = Conversion.ToString((object)item.Cells["CheqNo"].Text);
				}
				else
				{
					txtChequeRef.Text = Conversion.ToString((object)item.Cells["CheqNo"].Text);
				}
				txtCrRemarks.Text = Conversion.ToString((object)item.Cells["Remarks"].Text);
				txtTotalCrAmount.Text = Conversion.ToString((object)item.Cells["TotalAmountCr"].Text);
				txtAmount.Text = Conversion.ToString((object)item.Cells["AmountCr"].Text);
			}
			updateRowIndex = Conversion.ToInt(item.Cells["LineId"].Value);
			cmbCreditAccount.Focus();
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
			if (!FormValidationDetail())
			{
				return;
			}
			double RemainingAmount = 0.0;
			GridEXRow item = grdDetail.CurrentRow;
			int CurrentRowsubCode = Conversion.ToInt(item.Cells["SubCode"].Value);
			double CurrentRowAmount = 0.0;
			if (Conversion.ToDouble(item.Cells["AmountDr"].Value) > 0.0)
			{
				CurrentRowAmount = Conversion.ToDouble(item.Cells["AmountDr"].Value);
			}
			else if (Conversion.ToDouble(item.Cells["AmountCr"].Value) > 0.0)
			{
				CurrentRowAmount = Conversion.ToDouble(item.Cells["AmountCr"].Value);
			}
			DataRow[] SubCodeArray = table.Select("SubCode='" + Conversion.ToInt((object)CurrentRowsubCode) + "'");
			if (SubCodeArray.Length != 0)
			{
				double Amount = 0.0;
				double TotalCreditAmount = 0.0;
				double TotalDeditAmount = 0.0;
				if (txtTotalCrAmount.Text == string.Empty && txtTotalDrAmount.Text == string.Empty)
				{
					txtTotalCrAmount.Text = "0";
					txtTotalDrAmount.Text = "0";
				}
				if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0 && Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
				{
					MessageBox.Show("You Can't Enter Total Credit Amount And Total Debit Amount At Same Time");
					return;
				}
				if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
				{
					txtTotalDrAmount.Text = "0";
				}
				else if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
				{
					txtTotalCrAmount.Text = "0";
				}
				if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
				{
					if (Conversion.ToDouble((object)txtAmount.Text) > Conversion.ToDouble((object)txtTotalCrAmount.Text))
					{
						MessageBox.Show("Amount Shouldn't Greater Than Total Credit Amount");
						return;
					}
				}
				else if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0 && Conversion.ToDouble((object)txtAmount.Text) > Conversion.ToDouble((object)txtTotalDrAmount.Text))
				{
					MessageBox.Show("Amount Shouldn't Greater Than Total Debit Amount");
					return;
				}
				int CreditAccountId = 0;
				int DebitAccountId = 0;
				for (int k = 0; k < SubCodeArray.Length; k++)
				{
					if ((k + 1) % 2 != 0)
					{
						CreditAccountId = Conversion.ToInt(SubCodeArray[k].ItemArray[2]);
					}
					else
					{
						DebitAccountId = Conversion.ToInt(SubCodeArray[k].ItemArray[2]);
					}
					TotalDeditAmount = Conversion.ToDouble(SubCodeArray[k].ItemArray[10]);
					TotalCreditAmount = Conversion.ToDouble(SubCodeArray[k].ItemArray[11]);
					Amount += (double)Conversion.ToInt(SubCodeArray[k].ItemArray[13]);
				}
				if (TotalCreditAmount > 0.0 && Amount - CurrentRowAmount != TotalCreditAmount)
				{
					RemainingAmount = TotalCreditAmount - (Amount - CurrentRowAmount);
					if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
					{
						txtTotalCrAmount.Text = Conversion.ToString((object)TotalCreditAmount);
						txtTotalDrAmount.Text = "0";
						MessageBox.Show("You Can't Change The Credit Total Amount To Debit Total Amount");
						return;
					}
					if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > TotalCreditAmount)
					{
						txtTotalCrAmount.Text = Conversion.ToString((object)TotalCreditAmount);
						MessageBox.Show("You Can't Change The Credit Total Amount");
						return;
					}
					if (Conversion.ToInt(cmbCreditAccount.Value) != CreditAccountId)
					{
						MessageBox.Show("You Can't Change The Credit Account");
						return;
					}
					if (Conversion.ToDouble((object)txtAmount.Text) > RemainingAmount)
					{
						MessageBox.Show("Amount Shouldn't Greater Than The Remaining Amount: " + RemainingAmount + " Of Total Credit Amount");
						return;
					}
				}
				else if (TotalDeditAmount > 0.0 && Amount != TotalDeditAmount)
				{
					RemainingAmount = TotalDeditAmount - Amount;
					if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
					{
						txtTotalDrAmount.Text = Conversion.ToString((object)TotalDeditAmount);
						txtTotalCrAmount.Text = "0";
						MessageBox.Show("You Can't Change The Debit Total Amount To Credit Total Amount");
						return;
					}
					if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > TotalCreditAmount)
					{
						txtTotalDrAmount.Text = Conversion.ToString((object)TotalDeditAmount);
						MessageBox.Show("You Can't Change The Debit Total Amount");
						return;
					}
					if (Conversion.ToInt(cmbDebitAccount.Value) != DebitAccountId)
					{
						MessageBox.Show("You Can't Change The Debit Account");
						return;
					}
					if (Conversion.ToDouble((object)txtAmount.Text) > RemainingAmount)
					{
						MessageBox.Show("Amount Shouldn't Greater Than The Remaining Amount: " + RemainingAmount + " Of Total Debit Amount");
						return;
					}
				}
			}
			if (updateRowIndex > -1)
			{
				bool IfUpdateRowIdIsOfDebitRow = updateRowIndex % 2 == 0;
				object[] debitRowValues = new object[16]
				{
					Conversion.ToInt((object)txtSubCode.Text),
					Conversion.ToInt(((UltraDropDownBase)cmbDebitAccount).SelectedRow.Cells[3].Value),
					Conversion.ToInt(cmbDebitAccount.Value),
					((Control)(object)cmbDebitAccount).Text,
					Conversion.ToInt(cmbCreditAccount.Value),
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null
				};
				if (SubsidiaryAccountAllowOnVouchers)
				{
					if (((UltraGridBase)CmbDrSubsidiaryAccount).ActiveRow != null && ((Control)(object)CmbDrSubsidiaryAccount).Text != string.Empty)
					{
						debitRowValues[5] = CmbDrSubsidiaryAccount.Value;
						debitRowValues[6] = ((Control)(object)CmbDrSubsidiaryAccount).Text;
						debitRowValues[7] = ((UltraDropDownBase)CmbDrSubsidiaryAccount).SelectedRow.Cells[2].Value;
					}
					else
					{
						debitRowValues[5] = cmbDebitAccount.Value;
						debitRowValues[6] = ((Control)(object)cmbDebitAccount).Text;
						debitRowValues[7] = 4;
					}
				}
				else
				{
					debitRowValues[5] = 0;
					debitRowValues[6] = "";
					debitRowValues[7] = 0;
				}
				debitRowValues[8] = txtDrRemarks.Text;
				if (ChequeBookEnableStatus)
				{
					debitRowValues[15] = CmbChequeNo.Value;
					debitRowValues[9] = ((Control)(object)CmbChequeNo).Text;
				}
				else
				{
					debitRowValues[15] = 0;
					debitRowValues[9] = txtChequeRef.Text;
				}
				debitRowValues[10] = Conversion.ToDouble((object)txtTotalDrAmount.Text);
				debitRowValues[11] = Conversion.ToDouble((object)txtTotalCrAmount.Text);
				debitRowValues[12] = Conversion.ToDouble((object)txtAmount.Text);
				debitRowValues[13] = Conversion.ToDouble((object)0);
				debitRowValues[14] = Conversion.ToInt((object)(IfUpdateRowIdIsOfDebitRow ? updateRowIndex : (updateRowIndex + 1)));
				object[] creditRowValues = new object[16]
				{
					Conversion.ToInt((object)txtSubCode.Text),
					Conversion.ToInt(((UltraDropDownBase)cmbCreditAccount).SelectedRow.Cells[3].Value),
					Conversion.ToInt(cmbCreditAccount.Value),
					((Control)(object)cmbCreditAccount).Text,
					Conversion.ToInt(cmbDebitAccount.Value),
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null
				};
				if (SubsidiaryAccountAllowOnVouchers)
				{
					if (((UltraGridBase)CmbCrSubsidiaryAccount).ActiveRow != null && ((Control)(object)CmbCrSubsidiaryAccount).Text != string.Empty)
					{
						creditRowValues[5] = CmbCrSubsidiaryAccount.Value;
						creditRowValues[6] = ((Control)(object)CmbCrSubsidiaryAccount).Text;
						creditRowValues[7] = ((UltraDropDownBase)CmbCrSubsidiaryAccount).SelectedRow.Cells[2].Value;
					}
					else
					{
						creditRowValues[5] = cmbCreditAccount.Value;
						creditRowValues[6] = ((Control)(object)cmbCreditAccount).Text;
						creditRowValues[7] = 4;
					}
				}
				else
				{
					creditRowValues[5] = 0;
					creditRowValues[6] = "";
					creditRowValues[7] = 0;
				}
				creditRowValues[8] = txtCrRemarks.Text;
				if (ChequeBookEnableStatus)
				{
					creditRowValues[9] = ((Control)(object)CmbChequeNo).Text;
					creditRowValues[15] = CmbChequeNo.Value;
				}
				else
				{
					creditRowValues[9] = txtChequeRef.Text;
					creditRowValues[15] = 0;
				}
				creditRowValues[10] = Conversion.ToDouble((object)txtTotalDrAmount.Text);
				creditRowValues[11] = Conversion.ToDouble((object)txtTotalCrAmount.Text);
				creditRowValues[12] = Conversion.ToDouble((object)0);
				creditRowValues[13] = Conversion.ToDouble((object)txtAmount.Text);
				creditRowValues[14] = Conversion.ToInt((object)((!IfUpdateRowIdIsOfDebitRow) ? updateRowIndex : (updateRowIndex - 1)));
				int updateIndexForDebitRow = 0;
				int updateIndexForCreditRow = 0;
				for (int j = 0; j < table.Rows.Count; j++)
				{
					if (Conversion.ToInt(table.Rows[j]["LineId"]) == Conversion.ToInt(debitRowValues[14]))
					{
						updateIndexForDebitRow = j;
						break;
					}
				}
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (Conversion.ToInt(table.Rows[i]["LineId"]) == Conversion.ToInt(creditRowValues[14]))
					{
						updateIndexForCreditRow = i;
						break;
					}
				}
				table.Rows[updateIndexForDebitRow]["SubCode"] = debitRowValues[0];
				table.Rows[updateIndexForDebitRow]["AccountCode"] = debitRowValues[1];
				table.Rows[updateIndexForDebitRow]["AccountId"] = debitRowValues[2];
				table.Rows[updateIndexForDebitRow]["AccountTitle"] = debitRowValues[3];
				table.Rows[updateIndexForDebitRow]["AgainstAccountId"] = debitRowValues[4];
				table.Rows[updateIndexForDebitRow]["SubsidiaryAccountId"] = debitRowValues[5];
				table.Rows[updateIndexForDebitRow]["SubsidiaryAccount"] = debitRowValues[6];
				table.Rows[updateIndexForDebitRow]["SubsidiaryAccountTypeId"] = debitRowValues[7];
				table.Rows[updateIndexForDebitRow]["Remarks"] = debitRowValues[8];
				table.Rows[updateIndexForDebitRow]["CheqNo"] = debitRowValues[9];
				table.Rows[updateIndexForDebitRow]["TotalAmountDr"] = debitRowValues[10];
				table.Rows[updateIndexForDebitRow]["TotalAmountCr"] = debitRowValues[11];
				table.Rows[updateIndexForDebitRow]["AmountDr"] = debitRowValues[12];
				table.Rows[updateIndexForDebitRow]["AmountCr"] = debitRowValues[13];
				table.Rows[updateIndexForDebitRow]["LineId"] = debitRowValues[14];
				table.Rows[updateIndexForDebitRow]["CheqId"] = debitRowValues[15];
				table.Rows[updateIndexForCreditRow]["SubCode"] = creditRowValues[0];
				table.Rows[updateIndexForCreditRow]["AccountCode"] = creditRowValues[1];
				table.Rows[updateIndexForCreditRow]["AccountId"] = creditRowValues[2];
				table.Rows[updateIndexForCreditRow]["AccountTitle"] = creditRowValues[3];
				table.Rows[updateIndexForCreditRow]["AgainstAccountId"] = creditRowValues[4];
				table.Rows[updateIndexForCreditRow]["SubsidiaryAccountId"] = creditRowValues[5];
				table.Rows[updateIndexForCreditRow]["SubsidiaryAccount"] = creditRowValues[6];
				table.Rows[updateIndexForCreditRow]["SubsidiaryAccountTypeId"] = creditRowValues[7];
				table.Rows[updateIndexForCreditRow]["Remarks"] = creditRowValues[8];
				table.Rows[updateIndexForCreditRow]["CheqNo"] = creditRowValues[9];
				table.Rows[updateIndexForCreditRow]["TotalAmountDr"] = creditRowValues[10];
				table.Rows[updateIndexForCreditRow]["TotalAmountCr"] = creditRowValues[11];
				table.Rows[updateIndexForCreditRow]["AmountDr"] = creditRowValues[12];
				table.Rows[updateIndexForCreditRow]["AmountCr"] = creditRowValues[13];
				table.Rows[updateIndexForCreditRow]["LineId"] = creditRowValues[14];
				table.Rows[updateIndexForCreditRow]["CheqId"] = creditRowValues[15];
				updateRowIndex = -1;
			}
			if (Conversion.ToDouble((object)txtAmount.Text) == RemainingAmount)
			{
				txtTotalCrAmount.Text = "0";
				txtTotalDrAmount.Text = "0";
			}
			if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
			{
				txtSubCode.Text = Conversion.ToString((object)CurrentRowsubCode);
			}
			else if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
			{
				txtSubCode.Text = Conversion.ToString((object)CurrentRowsubCode);
			}
			else
			{
				txtSubCode.Text = Conversion.ToString((object)UniqueSubCodeForOneEntry);
			}
			if (Conversion.ToDouble((object)txtTotalCrAmount.Text) == 0.0 && Conversion.ToDouble((object)txtTotalDrAmount.Text) == 0.0)
			{
				((Control)(object)cmbCreditAccount).Text = string.Empty;
				((Control)(object)cmbDebitAccount).Text = string.Empty;
			}
			else if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
			{
				((Control)(object)cmbDebitAccount).Text = string.Empty;
			}
			else if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
			{
				((Control)(object)cmbCreditAccount).Text = string.Empty;
			}
			ResetDetail();
			txtTotalCrAmount.Enabled = true;
			txtTotalDrAmount.Enabled = true;
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			txtExchangeRate_TextChanged(null, null);
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
			txtTotalCrAmount.Enabled = true;
			txtTotalDrAmount.Enabled = true;
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			int num = Conversion.ToInt((object)txtSubCode.Text);
			txtSubCode.Text = Conversion.ToString((object)UniqueSubCodeForOneEntry);
			if (Conversion.ToDouble((object)txtTotalCrAmount.Text) > 0.0)
			{
				((Control)(object)cmbDebitAccount).Text = string.Empty;
			}
			else if (Conversion.ToDouble((object)txtTotalDrAmount.Text) > 0.0)
			{
				((Control)(object)cmbCreditAccount).Text = string.Empty;
			}
			if (num != UniqueSubCodeForOneEntry)
			{
				txtTotalCrAmount.Text = string.Empty;
				txtTotalDrAmount.Text = string.Empty;
			}
			ResetDetail();
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
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			UniqueSubCodeForOneEntry = 1;
			txtSubCode.Text = Conversion.ToString((object)UniqueSubCodeForOneEntry);
			txtDocNo.Text = string.Empty;
			txtDocDate.Value = DateTime.Now;
			((Control)(object)cmbCashAccount).Text = string.Empty;
			txtClosing.Clear();
			txtOpening.Clear();
			txtFcyAmount.Text = string.Empty;
			txtVoucherRemarks.Text = string.Empty;
			((Control)(object)cmbCreditAccount).Text = string.Empty;
			BindCreditSubsidiaryAccount();
			((Control)(object)cmbDebitAccount).Text = string.Empty;
			BindDebitSubsidiaryAccount();
			txtTotalCrAmount.Clear();
			txtTotalDrAmount.Clear();
			ResetDetail();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			txtDocDate.Focus();
			table.Rows.Clear();
			GenerateCode();
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
			txtCrRemarks.Clear();
			txtDrRemarks.Clear();
			txtChequeRef.Clear();
			((Control)(object)CmbChequeNo).Text = string.Empty;
			txtAmount.Clear();
			CrBalance.Text = string.Empty;
			DrBalance.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				HistoryFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill()
	{
		try
		{
			DataTable dt1 = new DataTable();
			dt1.Columns.Add("Id");
			dt1.Columns.Add("VoucherCode");
			dt1.Columns.Add("CashAccount");
			dt1.Columns.Add("AccountCode");
			dt1.Columns.Add("DetailAccount");
			dt1.Columns.Add("CreditAmount", typeof(double));
			dt1.Columns.Add("FcyAmount", typeof(double));
			dt1.Columns.Add("Comments");
			dt1.Columns.Add("CheqNoDetail");
			dt1.Columns.Add("EntryUserName");
			dt1.Columns.Add("EntryDate", typeof(DateTime));
			dt1.Columns.Add("ModifyUserName");
			dt1.Columns.Add("ModifyDate", typeof(DateTime));
			dt1.Columns.Add("NoOfAttachments", typeof(int));
			DataTable dt2 = new DataTable();
			dt2.Columns.Add("Id");
			dt2.Columns.Add("VoucherCode");
			dt2.Columns.Add("CashAccount");
			dt2.Columns.Add("AccountCode");
			dt2.Columns.Add("DetailAccount");
			dt2.Columns.Add("DebitAmount", typeof(double));
			dt2.Columns.Add("FcyAmount", typeof(double));
			dt2.Columns.Add("Comments");
			dt2.Columns.Add("CheqNoDetail");
			dt2.Columns.Add("EntryUserName");
			dt2.Columns.Add("EntryDate", typeof(DateTime));
			dt2.Columns.Add("ModifyUserName");
			dt2.Columns.Add("ModifyDate", typeof(DateTime));
			dt2.Columns.Add("NoOfAttachments", typeof(int));
			DataTable dtHistory = VoucherHead.DayBookHistory(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, Conversion.ToDateTime((object)((Control)(object)datVouchderDateForHistory).Text), Conversion.ToInt(CmbCashAccountForHistory.Value), 8);
			if (dtHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					if (Conversion.ToDouble(dtHistory.Rows[i]["CreditAmount"]) > 0.0)
					{
						dt1.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["VoucherCode"], dtHistory.Rows[i]["CashAccount"], dtHistory.Rows[i]["DeailAccountCode"], dtHistory.Rows[i]["DetailAccount"], dtHistory.Rows[i]["CreditAmount"], dtHistory.Rows[i]["DCurrencyAmount"], dtHistory.Rows[i]["Comments"], dtHistory.Rows[i]["CheqNoDetail"], dtHistory.Rows[i]["EntryUserName"], Conversion.ToDateTime(dtHistory.Rows[i]["EntryDate"]).ToShortDateString(), dtHistory.Rows[i]["ModifyUserName"], Conversion.ToDateTime(dtHistory.Rows[i]["ModifyDate"]).ToShortDateString(), dtHistory.Rows[i]["NoOfAttachments"]);
					}
					else if (Conversion.ToDouble(dtHistory.Rows[i]["DebitAmount"]) > 0.0)
					{
						dt2.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["VoucherCode"], dtHistory.Rows[i]["CashAccount"], dtHistory.Rows[i]["DeailAccountCode"], dtHistory.Rows[i]["DetailAccount"], dtHistory.Rows[i]["DebitAmount"], dtHistory.Rows[i]["DCurrencyAmount"], dtHistory.Rows[i]["Comments"], dtHistory.Rows[i]["CheqNoDetail"], dtHistory.Rows[i]["EntryUserName"], Conversion.ToDateTime(dtHistory.Rows[i]["EntryDate"]).ToShortDateString(), dtHistory.Rows[i]["ModifyUserName"], Conversion.ToDateTime(dtHistory.Rows[i]["ModifyDate"]).ToShortDateString(), dtHistory.Rows[i]["NoOfAttachments"]);
					}
				}
				DataGridCreditHistory.DataSource = dt1;
				DataGridCreditHistory.RetrieveStructure();
				DataGridCreditHistorySetting();
				DataGridDebitHistory.DataSource = dt2;
				DataGridDebitHistory.RetrieveStructure();
				DataGridDebitHistorySetting();
			}
			else
			{
				DataGridCreditHistory.ClearStructure();
				DataGridDebitHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridCreditHistorySetting()
	{
		try
		{
			DataGridCreditHistory.RootTable.Columns["Id"].Visible = false;
			DataGridCreditHistory.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridCreditHistory.RootTable.Columns["CreditAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridCreditHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridCreditHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridCreditHistory.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			DataGridCreditHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			DataGridCreditHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridCreditHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridCreditHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(DataGridCreditHistory);
			DataGridCreditHistory.RootTable.Columns.Add("Edit");
			DataGridCreditHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridCreditHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridCreditHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridCreditHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridCreditHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridCreditHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridCreditHistory.RootTable.Columns.Add("Print");
			DataGridCreditHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridCreditHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridCreditHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridCreditHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridCreditHistory.RootTable.Columns["Print"].Width = 50;
			DataGridCreditHistory.RootTable.Columns["Print"].Position = 1;
			DataGridCreditHistory.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridDebitHistorySetting()
	{
		try
		{
			DataGridDebitHistory.RootTable.Columns["Id"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["EntryUserName"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["ModifyUserName"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["EntryDate"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["ModifyDate"].Visible = false;
			DataGridDebitHistory.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			DataGridDebitHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridDebitHistory.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridDebitHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridDebitHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridDebitHistory.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			DataGridDebitHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.###";
			DataGridDebitHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridDebitHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(DataGridDebitHistory);
			DataGridDebitHistory.RootTable.Columns.Add("Edit");
			DataGridDebitHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridDebitHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridDebitHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridDebitHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridDebitHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridDebitHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridDebitHistory.RootTable.Columns.Add("Print");
			DataGridDebitHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridDebitHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridDebitHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridDebitHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridDebitHistory.RootTable.Columns["Print"].Position = 1;
			DataGridDebitHistory.RootTable.Columns["Print"].Width = 50;
			DataGridDebitHistory.FrozenColumns = 2;
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
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

	public void ReadById(int ID)
	{
		try
		{
			RecId = ID;
			VoucherHead Obj = VoucherHead.GetByID(ID);
			if (Obj != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				txtDocDate.Value = Obj.VoucherDate;
				txtDocNo.Text = Conversion.ToString((object)Obj.VoucherCode);
				cmbCashAccount.Value = Obj.RefAccountId;
				cmbCashAccount_Leave(null, null);
				txtVoucherRemarks.Text = Obj.Remarks;
				cmbCurrency.Value = Obj.MultiCurrencyId;
				txtExchangeRate.Text = Obj.ExchangeCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtFcyAmount.Text = Obj.FcAmount.ToString("#,##0.###");
				if (Obj.MultiCurrencyId == 0 || Obj.ExchangeCurrencyRate == 0.0)
				{
					DefaultConfigurations();
				}
				AttachmentsValues = Obj.AttachmentsValues;
				CustomAttachmentsValues = Obj.CustomAttachmentsValues;
				table.Rows.Clear();
				for (int i = 0; i < Obj.voucherDetailList.Count; i++)
				{
					table.Rows.Add(Obj.voucherDetailList[i].SubNo, Obj.voucherDetailList[i].AccountCode, Obj.voucherDetailList[i].AccountId, Obj.voucherDetailList[i].AccountTitle, Obj.voucherDetailList[i].AgainstAccountId, Obj.voucherDetailList[i].SubsidiaryAccountId, Obj.voucherDetailList[i].SubsidiaryAccountTitle, Obj.voucherDetailList[i].SubsidiaryTypeId, Obj.voucherDetailList[i].Comments, Obj.voucherDetailList[i].CheqNoDetail, Obj.voucherDetailList[i].TotalDebitAmount, Obj.voucherDetailList[i].TotalCreditAmount, Obj.voucherDetailList[i].DebitAmount, Obj.voucherDetailList[i].CreditAmount, Obj.voucherDetailList[i].LineId, Obj.voucherDetailList[i].InvoiceNoRefId, (Obj.voucherDetailList[i].DebitAmount > 0.0) ? Obj.voucherDetailList[i].DCurrencyAmount : 0.0, (Obj.voucherDetailList[i].CreditAmount > 0.0) ? Obj.voucherDetailList[i].DCurrencyAmount : 0.0);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				gridsetting();
				UniqueSubCodeForOneEntry = Conversion.ToInt((object)grdDetail.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["SubCode"].Value)));
				UniqueSubCodeForOneEntry++;
				txtSubCode.Text = Conversion.ToString((object)UniqueSubCodeForOneEntry);
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				txtExchangeRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridCreditHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				RecId = Conversion.ToInt(DataGridCreditHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				DayBookSlip(Conversion.ToInt(DataGridCreditHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridDebitHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				RecId = Conversion.ToInt(DataGridDebitHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				DayBookSlip(Conversion.ToInt(DataGridDebitHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DayBookSlip(int PrintId)
	{
		try
		{
			CommonServices.DayBookSlip(PrintId);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridCreditHistory).Name);
			if (fs != null)
			{
				DataGridCreditHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridDebitHistory).Name);
			if (fs != null)
			{
				DataGridDebitHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridCreditHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridCreditHistory.CurrentRow.Cells["Id"].Value), 8);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridDebitHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridDebitHistory.CurrentRow.Cells["Id"].Value), 8);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint144_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocDate = Conversion.ToDateTime(datVouchderDateForHistory.Value);
			obj.AccountId = Conversion.ToInt(CmbCashAccountForHistory.Value);
			if (chkZkipCashTransaction.Checked)
			{
				obj.ZeroBalanceType = 1;
			}
			DataTable dt = GeneralReprots.DayBookSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<double>("DebitAmount") > 0.0
				select r;
			DataTable dtDebit = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
			EnumerableRowCollection<DataRow> rowscr = from r in dt.AsEnumerable()
				where r.Field<double>("CreditAmount") > 0.0
				select r;
			DataTable obj2 = (rowscr.Any() ? rowscr.CopyToDataTable() : dt.Clone());
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject DayBookSubReport_DebitTransactions = new SubReportObject(dtDebit, "DayBookDebitSubReport.rpt");
			SubReportObject DayBookSubReport_CreditTransactions = new SubReportObject(obj2, "DayBookCreditSubReport.rpt");
			lstSubReports.Add(DayBookSubReport_DebitTransactions);
			lstSubReports.Add(DayBookSubReport_CreditTransactions);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "144-DayBookSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (MessageBox.Show("Are you sure! You Want To Delete The Record ?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow item = grdDetail.CurrentRow;
			int subCode = Conversion.ToInt(item.Cells["SubCode"].Value);
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			int UniqueId = Conversion.ToInt(item.Cells["LineId"].Value);
			if (UniqueId % 2 == 0)
			{
				for (int j = 0; j < table.Rows.Count; j++)
				{
					if (Conversion.ToInt(table.Rows[j]["LineId"]) == UniqueId - 1)
					{
						table.Rows[j].Delete();
						break;
					}
				}
				item.Delete();
				DataRow[] SubCodeArray = table.Select("SubCode='" + Conversion.ToInt((object)subCode) + "'");
				if (SubCodeArray.Length == 0 || Conversion.ToInt((object)txtSubCode.Text) == subCode)
				{
					return;
				}
				double amount = 0.0;
				for (int i = 0; i < SubCodeArray.Length; i++)
				{
					amount += Conversion.ToDouble(SubCodeArray[i].ItemArray[10]);
				}
				if (Conversion.ToDouble(SubCodeArray[0].ItemArray[7]) > 0.0)
				{
					GridEXRow[] rows = grdDetail.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (subCode == Conversion.ToInt(r.Cells["SubCode"].Value))
						{
							r.BeginEdit();
							r.Cells["TotalAmountDr"].Value = Conversion.ToString((object)amount);
							r.EndEdit();
						}
					}
					grdDetail.UpdateData();
				}
				else
				{
					if (!(Conversion.ToDouble(SubCodeArray[0].ItemArray[8]) > 0.0))
					{
						return;
					}
					GridEXRow[] rows = grdDetail.GetRows();
					foreach (GridEXRow r2 in rows)
					{
						if (subCode == Conversion.ToInt(r2.Cells["SubCode"].Value))
						{
							r2.BeginEdit();
							r2.Cells["TotalAmountCr"].Value = Conversion.ToString((object)amount);
							r2.EndEdit();
						}
					}
					grdDetail.UpdateData();
				}
				return;
			}
			for (int l = 0; l < table.Rows.Count; l++)
			{
				if (Conversion.ToInt(table.Rows[l]["LineId"]) == UniqueId + 1)
				{
					table.Rows[l].Delete();
					break;
				}
			}
			item.Delete();
			DataRow[] SubCodeArray2 = table.Select("SubCode='" + Conversion.ToInt((object)subCode) + "'");
			if (SubCodeArray2.Length == 0 || Conversion.ToInt((object)txtSubCode.Text) == subCode)
			{
				return;
			}
			double amount2 = 0.0;
			for (int k = 0; k < SubCodeArray2.Length; k++)
			{
				amount2 += Conversion.ToDouble(SubCodeArray2[k].ItemArray[10]);
			}
			if (Conversion.ToDouble(SubCodeArray2[0].ItemArray[7]) > 0.0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (subCode == Conversion.ToInt(r3.Cells["SubCode"].Value))
					{
						r3.BeginEdit();
						r3.Cells["TotalAmountDr"].Value = Conversion.ToString((object)amount2);
						r3.EndEdit();
					}
				}
				grdDetail.UpdateData();
			}
			else
			{
				if (!(Conversion.ToDouble(SubCodeArray2[0].ItemArray[8]) > 0.0))
				{
					return;
				}
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					if (subCode == Conversion.ToInt(r4.Cells["SubCode"].Value))
					{
						r4.BeginEdit();
						r4.Cells["TotalAmountCr"].Value = Conversion.ToString((object)amount2);
						r4.EndEdit();
					}
				}
				grdDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTotalCrAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtTotalDrAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTypingOnVouchers(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTotalCrAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTotalDrAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDayBook_FormClosing(object sender, FormClosingEventArgs e)
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
			dt.Rows.Add("Ctrl+P", "For Print in History");
			dt.Rows.Add("Ctrl+F5", "For Focus on Voucher Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowRight", "For Focus On History Grid's");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Credit Account in Detail Box");
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

	private void btnAddDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar2")
			{
				cmbCreditAccount.Focus();
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
			if (grdDetail.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double DebitAmount = Conversion.ToDouble(obj.Cells["AmountDr"].Value);
					double num = Conversion.ToDouble(obj.Cells["AmountCr"].Value);
					double DrFcyamount = DebitAmount / Conversion.ToDouble((object)txtExchangeRate.Text);
					double CrFcyamount = num / Conversion.ToDouble((object)txtExchangeRate.Text);
					obj.Cells["DebitFcyAmount"].Value = DrFcyamount;
					obj.Cells["CreditFcyAmount"].Value = CrFcyamount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["DebitFcyAmount"].Value = 0;
					obj2.Cells["CreditFcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
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
			if (grdDetail.GetRows().Count() > 0)
			{
				double TotalFcyAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["DebitFcyAmount"], (AggregateFunction)2));
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space || MessageBox.Show("Are you sure! You Want To Delete The Record ?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				int subCode = Conversion.ToInt(item.Cells["SubCode"].Value);
				if (!(grdDetail.CurrentColumn.Key == "Delete"))
				{
					return;
				}
				int UniqueId = Conversion.ToInt(item.Cells["LineId"].Value);
				if (UniqueId % 2 == 0)
				{
					for (int j = 0; j < table.Rows.Count; j++)
					{
						if (Conversion.ToInt(table.Rows[j]["LineId"]) == UniqueId - 1)
						{
							table.Rows[j].Delete();
							break;
						}
					}
					item.Delete();
					DataRow[] SubCodeArray = table.Select("SubCode='" + Conversion.ToInt((object)subCode) + "'");
					if (SubCodeArray.Length == 0 || Conversion.ToInt((object)txtSubCode.Text) == subCode)
					{
						return;
					}
					double amount = 0.0;
					for (int i = 0; i < SubCodeArray.Length; i++)
					{
						amount += Conversion.ToDouble(SubCodeArray[i].ItemArray[10]);
					}
					if (Conversion.ToDouble(SubCodeArray[0].ItemArray[7]) > 0.0)
					{
						GridEXRow[] rows = grdDetail.GetRows();
						foreach (GridEXRow r in rows)
						{
							if (subCode == Conversion.ToInt(r.Cells["SubCode"].Value))
							{
								r.BeginEdit();
								r.Cells["TotalAmountDr"].Value = Conversion.ToString((object)amount);
								r.EndEdit();
							}
						}
						grdDetail.UpdateData();
					}
					else
					{
						if (!(Conversion.ToDouble(SubCodeArray[0].ItemArray[8]) > 0.0))
						{
							return;
						}
						GridEXRow[] rows = grdDetail.GetRows();
						foreach (GridEXRow r2 in rows)
						{
							if (subCode == Conversion.ToInt(r2.Cells["SubCode"].Value))
							{
								r2.BeginEdit();
								r2.Cells["TotalAmountCr"].Value = Conversion.ToString((object)amount);
								r2.EndEdit();
							}
						}
						grdDetail.UpdateData();
					}
					return;
				}
				for (int l = 0; l < table.Rows.Count; l++)
				{
					if (Conversion.ToInt(table.Rows[l]["LineId"]) == UniqueId + 1)
					{
						table.Rows[l].Delete();
						break;
					}
				}
				item.Delete();
				DataRow[] SubCodeArray2 = table.Select("SubCode='" + Conversion.ToInt((object)subCode) + "'");
				if (SubCodeArray2.Length == 0 || Conversion.ToInt((object)txtSubCode.Text) == subCode)
				{
					return;
				}
				double amount2 = 0.0;
				for (int k = 0; k < SubCodeArray2.Length; k++)
				{
					amount2 += Conversion.ToDouble(SubCodeArray2[k].ItemArray[10]);
				}
				if (Conversion.ToDouble(SubCodeArray2[0].ItemArray[7]) > 0.0)
				{
					GridEXRow[] rows = grdDetail.GetRows();
					foreach (GridEXRow r3 in rows)
					{
						if (subCode == Conversion.ToInt(r3.Cells["SubCode"].Value))
						{
							r3.BeginEdit();
							r3.Cells["TotalAmountDr"].Value = Conversion.ToString((object)amount2);
							r3.EndEdit();
						}
					}
					grdDetail.UpdateData();
				}
				else
				{
					if (!(Conversion.ToDouble(SubCodeArray2[0].ItemArray[8]) > 0.0))
					{
						return;
					}
					GridEXRow[] rows = grdDetail.GetRows();
					foreach (GridEXRow r4 in rows)
					{
						if (subCode == Conversion.ToInt(r4.Cells["SubCode"].Value))
						{
							r4.BeginEdit();
							r4.Cells["TotalAmountCr"].Value = Conversion.ToString((object)amount2);
							r4.EndEdit();
						}
					}
					grdDetail.UpdateData();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridCreditHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DataGridCreditHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(id);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridCreditHistory.CurrentColumn.Key == "Print")
				{
					DayBookSlip(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (DataGridCreditHistory.CurrentColumn.Key == "Edit")
				{
					int id2 = Conversion.ToInt(item.Cells["Id"].Value);
					ReadById(id2);
				}
				else if (DataGridCreditHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 8);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridDebitHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DataGridDebitHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(id);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridDebitHistory.CurrentColumn.Key == "Print")
				{
					DayBookSlip(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (DataGridDebitHistory.CurrentColumn.Key == "Edit")
				{
					int id2 = Conversion.ToInt(item.Cells["Id"].Value);
					ReadById(id2);
				}
				else if (DataGridDebitHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 8);
				}
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
			if (base.ActiveControl.Name == "ctrlGrdBar2")
			{
				cmbCreditAccount.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "8",
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

	private void CmbCrSubsidiaryAccount_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			BindCreditParentAccount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindCreditParentAccount()
	{
		try
		{
			if (!AfterCloseUpCrSubsidiary && ((UltraGridBase)CmbCrSubsidiaryAccount).ActiveRow != null && updateRowIndex <= -1)
			{
				cmbCreditAccount.Value = Conversion.ToInt(((UltraDropDownBase)CmbCrSubsidiaryAccount).SelectedRow.Cells[4].Value);
				cmbCreditAccount_Leave(null, null);
			}
			AfterCloseUpCrSubsidiary = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbDrSubsidiaryAccount_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			BindDebitParentAccount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindDebitParentAccount()
	{
		try
		{
			if (!AfterCloseUpDrSubsidiary && ((UltraGridBase)CmbDrSubsidiaryAccount).ActiveRow != null && updateRowIndex <= -1)
			{
				cmbDebitAccount.Value = Conversion.ToInt(((UltraDropDownBase)CmbDrSubsidiaryAccount).SelectedRow.Cells[4].Value);
				cmbDebitAccount_Leave(null, null);
			}
			AfterCloseUpDrSubsidiary = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbCrSubsidiaryAccount_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbCrSubsidiaryAccount).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloseUpCrSubsidiary = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					cmbCreditAccount.Value = GLOfSubSidory;
					cmbCreditAccount_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDrSubsidiaryAccount_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbDrSubsidiaryAccount).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloseUpDrSubsidiary = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					cmbDebitAccount.Value = GLOfSubSidory;
					cmbDebitAccount_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCreditSubsidiaryAccount()
	{
		try
		{
			if (((UltraGridBase)cmbCreditAccount).ActiveRow != null && ((Control)(object)cmbCreditAccount).Text != string.Empty)
			{
				if (dtSubsidiary.Rows.Count > 0)
				{
					if (((UltraGridBase)cmbCreditAccount).ActiveRow != null && ((Control)(object)cmbCreditAccount).Text != string.Empty)
					{
						int selectedAcId = Conversion.ToInt(cmbCreditAccount.Value);
						if (selectedAcId != 0)
						{
							DataTable matchingTable = dtSubsidiary.Clone();
							foreach (DataRow row2 in dtSubsidiary.Rows)
							{
								if (Conversion.ToInt(row2["AccountId"]) == selectedAcId)
								{
									matchingTable.ImportRow(row2);
								}
							}
							int SubsidiaryIdInCaseOfUpdateDetail = 0;
							if (((Control)(object)CmbCrSubsidiaryAccount).Text != string.Empty)
							{
								SubsidiaryIdInCaseOfUpdateDetail = Conversion.ToInt(CmbCrSubsidiaryAccount.Value);
							}
							if (matchingTable.Rows.Count > 0)
							{
								SubsidiaryAccountCrDatasourceLength = matchingTable.Rows.Count;
								DDL.BindDDL(matchingTable, CmbCrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
								((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
								((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
								((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
								((Control)(object)CmbCrSubsidiaryAccount).Text = string.Empty;
								if (matchingTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SubsidiaryIdInCaseOfUpdateDetail))
								{
									CmbCrSubsidiaryAccount.Value = SubsidiaryIdInCaseOfUpdateDetail;
								}
								else
								{
									((Control)(object)CmbCrSubsidiaryAccount).Text = string.Empty;
								}
							}
							else
							{
								((UltraGridBase)CmbCrSubsidiaryAccount).DataSource = null;
								((Control)(object)CmbCrSubsidiaryAccount).Text = string.Empty;
								SubsidiaryAccountCrDatasourceLength = 0;
							}
						}
						else
						{
							((UltraGridBase)CmbCrSubsidiaryAccount).DataSource = null;
							((Control)(object)CmbCrSubsidiaryAccount).Text = string.Empty;
							SubsidiaryAccountCrDatasourceLength = 0;
						}
					}
					else
					{
						SubsidiaryAccountCrDatasourceLength = dtSubsidiary.Rows.Count;
						DDL.BindDDL(dtSubsidiary, CmbCrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
						((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
						((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					}
				}
				else
				{
					((UltraGridBase)CmbCrSubsidiaryAccount).DataSource = null;
					((Control)(object)CmbCrSubsidiaryAccount).Text = string.Empty;
					SubsidiaryAccountCrDatasourceLength = 0;
				}
			}
			else
			{
				((UltraGridBase)CmbCrSubsidiaryAccount).DataSource = null;
				((Control)(object)CmbCrSubsidiaryAccount).Text = string.Empty;
				SubsidiaryAccountCrDatasourceLength = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDebitSubsidiaryAccount()
	{
		try
		{
			if (((UltraGridBase)cmbDebitAccount).ActiveRow != null && ((Control)(object)cmbDebitAccount).Text != string.Empty)
			{
				if (dtSubsidiary.Rows.Count > 0)
				{
					if (((UltraGridBase)cmbDebitAccount).ActiveRow != null && ((Control)(object)cmbDebitAccount).Text != string.Empty)
					{
						int selectedAcId = Conversion.ToInt(cmbDebitAccount.Value);
						if (selectedAcId != 0)
						{
							DataTable matchingTable = dtSubsidiary.Clone();
							foreach (DataRow row2 in dtSubsidiary.Rows)
							{
								if (Conversion.ToInt(row2["AccountId"]) == selectedAcId)
								{
									matchingTable.ImportRow(row2);
								}
							}
							int SubsidiaryIdInCaseOfUpdateDetail = 0;
							if (((Control)(object)CmbDrSubsidiaryAccount).Text != string.Empty)
							{
								SubsidiaryIdInCaseOfUpdateDetail = Conversion.ToInt(CmbDrSubsidiaryAccount.Value);
							}
							if (matchingTable.Rows.Count > 0)
							{
								SubsidiaryAccountDrDatasourceLength = matchingTable.Rows.Count;
								DDL.BindDDL(matchingTable, CmbDrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
								((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
								((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
								((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
								((Control)(object)CmbDrSubsidiaryAccount).Text = string.Empty;
								if (matchingTable.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SubsidiaryIdInCaseOfUpdateDetail))
								{
									CmbDrSubsidiaryAccount.Value = SubsidiaryIdInCaseOfUpdateDetail;
								}
								else
								{
									((Control)(object)CmbDrSubsidiaryAccount).Text = string.Empty;
								}
							}
							else
							{
								((UltraGridBase)CmbDrSubsidiaryAccount).DataSource = null;
								((Control)(object)CmbDrSubsidiaryAccount).Text = string.Empty;
								SubsidiaryAccountDrDatasourceLength = 0;
							}
						}
						else
						{
							((UltraGridBase)CmbDrSubsidiaryAccount).DataSource = null;
							((Control)(object)CmbDrSubsidiaryAccount).Text = string.Empty;
							SubsidiaryAccountDrDatasourceLength = 0;
						}
					}
					else
					{
						SubsidiaryAccountDrDatasourceLength = dtSubsidiary.Rows.Count;
						DDL.BindDDL(dtSubsidiary, CmbDrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
						((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
						((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					}
				}
				else
				{
					((UltraGridBase)CmbDrSubsidiaryAccount).DataSource = null;
					((Control)(object)CmbDrSubsidiaryAccount).Text = string.Empty;
					SubsidiaryAccountDrDatasourceLength = 0;
				}
			}
			else
			{
				((Control)(object)CmbDrSubsidiaryAccount).Text = string.Empty;
				((UltraGridBase)CmbDrSubsidiaryAccount).DataSource = null;
				SubsidiaryAccountDrDatasourceLength = 0;
			}
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
			GetVoucherDateDropdownForHistory();
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
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Expected O, but got Unknown
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Expected O, but got Unknown
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Expected O, but got Unknown
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Expected O, but got Unknown
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Expected O, but got Unknown
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Expected O, but got Unknown
		//IL_0d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d48: Expected O, but got Unknown
		//IL_5b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b19: Expected O, but got Unknown
		//IL_5b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b30: Expected O, but got Unknown
		//IL_5e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e90: Expected O, but got Unknown
		//IL_5e9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ea7: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
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
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.frmDayBook));
		GridEXLayout DataGridDebitHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridCreditHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.SubCodePanel = new System.Windows.Forms.Panel();
		this.txtSubCode = new System.Windows.Forms.TextBox();
		this.lblSubCode = new System.Windows.Forms.Label();
		this.CreditAccountPanel = new System.Windows.Forms.Panel();
		this.cmbCreditAccount = new UltraCombo();
		this.lblcreditAccount = new System.Windows.Forms.Label();
		this.CrBalance = new System.Windows.Forms.Label();
		this.CreditSubsidiaryPanel = new System.Windows.Forms.Panel();
		this.CmbCrSubsidiaryAccount = new UltraCombo();
		this.lblCrSubsidiaryaccount = new System.Windows.Forms.Label();
		this.CrTotalAmountPanel = new System.Windows.Forms.Panel();
		this.txtTotalCrAmount = new System.Windows.Forms.TextBox();
		this.lblCrTotal = new System.Windows.Forms.Label();
		this.CrRemarksPanel = new System.Windows.Forms.Panel();
		this.txtCrRemarks = new System.Windows.Forms.TextBox();
		this.lblCrRemarks = new System.Windows.Forms.Label();
		this.CheqNoPanel = new System.Windows.Forms.Panel();
		this.CmbChequeNo = new UltraCombo();
		this.lblChequeNo = new System.Windows.Forms.Label();
		this.txtChequeRef = new System.Windows.Forms.TextBox();
		this.lblChequeRef = new System.Windows.Forms.Label();
		this.AmountPanel = new System.Windows.Forms.Panel();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.lblAmount = new System.Windows.Forms.Label();
		this.DebitAccountPanel = new System.Windows.Forms.Panel();
		this.cmbDebitAccount = new UltraCombo();
		this.DrBalance = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.DrTotalAmountPanel = new System.Windows.Forms.Panel();
		this.txtTotalDrAmount = new System.Windows.Forms.TextBox();
		this.lblDrAmount = new System.Windows.Forms.Label();
		this.DebitSubsidiaryAccountPanel = new System.Windows.Forms.Panel();
		this.lblDrSubsidiaryaccount = new System.Windows.Forms.Label();
		this.CmbDrSubsidiaryAccount = new UltraCombo();
		this.DrRemarksPanel = new System.Windows.Forms.Panel();
		this.txtDrRemarks = new System.Windows.Forms.TextBox();
		this.lblDrRemarks = new System.Windows.Forms.Label();
		this.ButtonsPanel = new System.Windows.Forms.Panel();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.MainBox = new System.Windows.Forms.GroupBox();
		this.txtVoucherRemarks = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.ChkBoxPrintPreview = new System.Windows.Forms.CheckBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.chkformat2 = new System.Windows.Forms.CheckBox();
		this.cmbCurrency = new UltraCombo();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.CmbTemporaryAccount = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.lblTemporaryAccount = new System.Windows.Forms.Label();
		this.cmbCashAccount = new UltraCombo();
		this.txtOpening = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtClosing = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label18 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.DataGridDebitHistory = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.DataGridCreditHistory = new GridEX();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.datVouchderDateForHistory = new UltraCombo();
		this.chkZkipCashTransaction = new System.Windows.Forms.CheckBox();
		this.BtnShow = new System.Windows.Forms.Button();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbCashAccountForHistory = new UltraCombo();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnPrint144 = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.SubCodePanel.SuspendLayout();
		this.CreditAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCreditAccount).BeginInit();
		this.CreditSubsidiaryPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrSubsidiaryAccount).BeginInit();
		this.CrTotalAmountPanel.SuspendLayout();
		this.CrRemarksPanel.SuspendLayout();
		this.CheqNoPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).BeginInit();
		this.AmountPanel.SuspendLayout();
		this.DebitAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDebitAccount).BeginInit();
		this.DrTotalAmountPanel.SuspendLayout();
		this.DebitSubsidiaryAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrSubsidiaryAccount).BeginInit();
		this.DrRemarksPanel.SuspendLayout();
		this.ButtonsPanel.SuspendLayout();
		this.MainBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTemporaryAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCashAccount).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridDebitHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridCreditHistory).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.datVouchderDateForHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCashAccountForHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1256, 563);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1256, 563);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1248, 537);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1242, 531);
		this.panel2.TabIndex = 0;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 323);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1242, 208);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.MainBox);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1242, 293);
		this.panel4.TabIndex = 1;
		this.groupBox2.Controls.Add(this.flowLayoutPanel1);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 173);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1239, 116);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.flowLayoutPanel1.Controls.Add(this.SubCodePanel);
		this.flowLayoutPanel1.Controls.Add(this.CreditAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.CreditSubsidiaryPanel);
		this.flowLayoutPanel1.Controls.Add(this.CrTotalAmountPanel);
		this.flowLayoutPanel1.Controls.Add(this.CrRemarksPanel);
		this.flowLayoutPanel1.Controls.Add(this.CheqNoPanel);
		this.flowLayoutPanel1.Controls.Add(this.AmountPanel);
		this.flowLayoutPanel1.Controls.Add(this.DebitAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.DrTotalAmountPanel);
		this.flowLayoutPanel1.Controls.Add(this.DebitSubsidiaryAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.DrRemarksPanel);
		this.flowLayoutPanel1.Controls.Add(this.ButtonsPanel);
		this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 16);
		this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(1212, 97);
		this.flowLayoutPanel1.TabIndex = 0;
		this.SubCodePanel.Controls.Add(this.txtSubCode);
		this.SubCodePanel.Controls.Add(this.lblSubCode);
		this.SubCodePanel.Location = new System.Drawing.Point(0, 0);
		this.SubCodePanel.Margin = new System.Windows.Forms.Padding(0);
		this.SubCodePanel.Name = "SubCodePanel";
		this.SubCodePanel.Size = new System.Drawing.Size(79, 47);
		this.SubCodePanel.TabIndex = 0;
		this.txtSubCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubCode.Location = new System.Drawing.Point(1, 23);
		this.txtSubCode.Name = "txtSubCode";
		this.txtSubCode.ReadOnly = true;
		this.txtSubCode.Size = new System.Drawing.Size(76, 23);
		this.txtSubCode.TabIndex = 0;
		this.txtSubCode.TabStop = false;
		this.lblSubCode.AutoSize = true;
		this.lblSubCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSubCode.Location = new System.Drawing.Point(1, 3);
		this.lblSubCode.Name = "lblSubCode";
		this.lblSubCode.Size = new System.Drawing.Size(58, 15);
		this.lblSubCode.TabIndex = 539;
		this.lblSubCode.Text = "Sub Code";
		this.CreditAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCreditAccount);
		this.CreditAccountPanel.Controls.Add(this.lblcreditAccount);
		this.CreditAccountPanel.Controls.Add(this.CrBalance);
		this.CreditAccountPanel.Location = new System.Drawing.Point(79, 0);
		this.CreditAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CreditAccountPanel.Name = "CreditAccountPanel";
		this.CreditAccountPanel.Size = new System.Drawing.Size(210, 47);
		this.CreditAccountPanel.TabIndex = 1;
		this.cmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCreditAccount).Location = new System.Drawing.Point(1, 20);
		((System.Windows.Forms.Control)(object)this.cmbCreditAccount).Name = "cmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.cmbCreditAccount).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.cmbCreditAccount).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbCreditAccount).Leave += new System.EventHandler(cmbCreditAccount_Leave);
		this.lblcreditAccount.AutoSize = true;
		this.lblcreditAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblcreditAccount.Location = new System.Drawing.Point(0, 2);
		this.lblcreditAccount.Name = "lblcreditAccount";
		this.lblcreditAccount.Size = new System.Drawing.Size(86, 15);
		this.lblcreditAccount.TabIndex = 522;
		this.lblcreditAccount.Text = "Credit Account";
		this.CrBalance.AutoSize = true;
		this.CrBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CrBalance.Location = new System.Drawing.Point(92, 2);
		this.CrBalance.Name = "CrBalance";
		this.CrBalance.Size = new System.Drawing.Size(59, 15);
		this.CrBalance.TabIndex = 544;
		this.CrBalance.Text = "CrBalance";
		this.CreditSubsidiaryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount);
		this.CreditSubsidiaryPanel.Controls.Add(this.lblCrSubsidiaryaccount);
		this.CreditSubsidiaryPanel.Location = new System.Drawing.Point(289, 0);
		this.CreditSubsidiaryPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CreditSubsidiaryPanel.Name = "CreditSubsidiaryPanel";
		this.CreditSubsidiaryPanel.Size = new System.Drawing.Size(172, 47);
		this.CreditSubsidiaryPanel.TabIndex = 2;
		this.CmbCrSubsidiaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCrSubsidiaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCrSubsidiaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCrSubsidiaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCrSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Name = "CmbCrSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).TabIndex = 551;
		this.CmbCrSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbCrSubsidiaryAccount_AfterCloseUp);
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Leave += new System.EventHandler(CmbCrSubsidiaryAccount_ValueChanged);
		this.lblCrSubsidiaryaccount.AutoSize = true;
		this.lblCrSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCrSubsidiaryaccount.Location = new System.Drawing.Point(1, 1);
		this.lblCrSubsidiaryaccount.Name = "lblCrSubsidiaryaccount";
		this.lblCrSubsidiaryaccount.Size = new System.Drawing.Size(144, 15);
		this.lblCrSubsidiaryaccount.TabIndex = 552;
		this.lblCrSubsidiaryaccount.Text = "Credit Subsidiary Account";
		this.CrTotalAmountPanel.Controls.Add(this.txtTotalCrAmount);
		this.CrTotalAmountPanel.Controls.Add(this.lblCrTotal);
		this.CrTotalAmountPanel.Location = new System.Drawing.Point(461, 0);
		this.CrTotalAmountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CrTotalAmountPanel.Name = "CrTotalAmountPanel";
		this.CrTotalAmountPanel.Size = new System.Drawing.Size(130, 46);
		this.CrTotalAmountPanel.TabIndex = 3;
		this.txtTotalCrAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalCrAmount.Location = new System.Drawing.Point(1, 21);
		this.txtTotalCrAmount.Name = "txtTotalCrAmount";
		this.txtTotalCrAmount.ShortcutsEnabled = false;
		this.txtTotalCrAmount.Size = new System.Drawing.Size(129, 23);
		this.txtTotalCrAmount.TabIndex = 2;
		this.txtTotalCrAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalCrAmount.TextChanged += new System.EventHandler(txtTotalCrAmount_TextChanged);
		this.txtTotalCrAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTotalCrAmount_KeyPress);
		this.lblCrTotal.AutoSize = true;
		this.lblCrTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCrTotal.Location = new System.Drawing.Point(4, 1);
		this.lblCrTotal.Name = "lblCrTotal";
		this.lblCrTotal.Size = new System.Drawing.Size(94, 15);
		this.lblCrTotal.TabIndex = 541;
		this.lblCrTotal.Text = "Cr Total Amount";
		this.CrRemarksPanel.Controls.Add(this.txtCrRemarks);
		this.CrRemarksPanel.Controls.Add(this.lblCrRemarks);
		this.CrRemarksPanel.Location = new System.Drawing.Point(591, 0);
		this.CrRemarksPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CrRemarksPanel.Name = "CrRemarksPanel";
		this.CrRemarksPanel.Size = new System.Drawing.Size(329, 47);
		this.CrRemarksPanel.TabIndex = 4;
		this.txtCrRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCrRemarks.Location = new System.Drawing.Point(1, 22);
		this.txtCrRemarks.Name = "txtCrRemarks";
		this.txtCrRemarks.Size = new System.Drawing.Size(328, 23);
		this.txtCrRemarks.TabIndex = 3;
		this.txtCrRemarks.Leave += new System.EventHandler(txtCrRemarks_Leave);
		this.lblCrRemarks.AutoSize = true;
		this.lblCrRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCrRemarks.Location = new System.Drawing.Point(1, 2);
		this.lblCrRemarks.Name = "lblCrRemarks";
		this.lblCrRemarks.Size = new System.Drawing.Size(86, 15);
		this.lblCrRemarks.TabIndex = 508;
		this.lblCrRemarks.Text = "Credit Remarks";
		this.CheqNoPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeNo);
		this.CheqNoPanel.Controls.Add(this.lblChequeNo);
		this.CheqNoPanel.Controls.Add(this.txtChequeRef);
		this.CheqNoPanel.Controls.Add(this.lblChequeRef);
		this.CheqNoPanel.Location = new System.Drawing.Point(920, 0);
		this.CheqNoPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CheqNoPanel.Name = "CheqNoPanel";
		this.CheqNoPanel.Size = new System.Drawing.Size(154, 47);
		this.CheqNoPanel.TabIndex = 5;
		this.CmbChequeNo.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Location = new System.Drawing.Point(1, 19);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Name = "CmbChequeNo";
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).TabIndex = 4;
		this.lblChequeNo.AutoSize = true;
		this.lblChequeNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblChequeNo.Location = new System.Drawing.Point(1, 1);
		this.lblChequeNo.Name = "lblChequeNo";
		this.lblChequeNo.Size = new System.Drawing.Size(66, 15);
		this.lblChequeNo.TabIndex = 549;
		this.lblChequeNo.Text = "Cheque No";
		this.txtChequeRef.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChequeRef.Location = new System.Drawing.Point(1, 21);
		this.txtChequeRef.Name = "txtChequeRef";
		this.txtChequeRef.Size = new System.Drawing.Size(149, 23);
		this.txtChequeRef.TabIndex = 9;
		this.lblChequeRef.AutoSize = true;
		this.lblChequeRef.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblChequeRef.Location = new System.Drawing.Point(1, 1);
		this.lblChequeRef.Name = "lblChequeRef";
		this.lblChequeRef.Size = new System.Drawing.Size(67, 15);
		this.lblChequeRef.TabIndex = 526;
		this.lblChequeRef.Text = "Cheque Ref";
		this.AmountPanel.Controls.Add(this.txtAmount);
		this.AmountPanel.Controls.Add(this.lblAmount);
		this.AmountPanel.Location = new System.Drawing.Point(1074, 0);
		this.AmountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.AmountPanel.Name = "AmountPanel";
		this.AmountPanel.Size = new System.Drawing.Size(133, 47);
		this.AmountPanel.TabIndex = 6;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(1, 22);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ShortcutsEnabled = false;
		this.txtAmount.Size = new System.Drawing.Size(130, 23);
		this.txtAmount.TabIndex = 5;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.TextChanged += new System.EventHandler(txtAmount_TextChanged);
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.lblAmount.AutoSize = true;
		this.lblAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmount.Location = new System.Drawing.Point(2, 2);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(51, 15);
		this.lblAmount.TabIndex = 528;
		this.lblAmount.Text = "Amount";
		this.DebitAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDebitAccount);
		this.DebitAccountPanel.Controls.Add(this.DrBalance);
		this.DebitAccountPanel.Controls.Add(this.label11);
		this.DebitAccountPanel.Location = new System.Drawing.Point(0, 47);
		this.DebitAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitAccountPanel.Name = "DebitAccountPanel";
		this.DebitAccountPanel.Size = new System.Drawing.Size(290, 47);
		this.DebitAccountPanel.TabIndex = 7;
		this.cmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).Location = new System.Drawing.Point(1, 20);
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).Name = "cmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).Size = new System.Drawing.Size(288, 26);
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbDebitAccount).Leave += new System.EventHandler(cmbDebitAccount_Leave);
		this.DrBalance.AutoSize = true;
		this.DrBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DrBalance.Location = new System.Drawing.Point(169, 2);
		this.DrBalance.Name = "DrBalance";
		this.DrBalance.Size = new System.Drawing.Size(61, 15);
		this.DrBalance.TabIndex = 545;
		this.DrBalance.Text = "DrBalance";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(2, 1);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(84, 15);
		this.label11.TabIndex = 530;
		this.label11.Text = "Debit Account";
		this.DrTotalAmountPanel.Controls.Add(this.txtTotalDrAmount);
		this.DrTotalAmountPanel.Controls.Add(this.lblDrAmount);
		this.DrTotalAmountPanel.Location = new System.Drawing.Point(290, 47);
		this.DrTotalAmountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrTotalAmountPanel.Name = "DrTotalAmountPanel";
		this.DrTotalAmountPanel.Size = new System.Drawing.Size(131, 47);
		this.DrTotalAmountPanel.TabIndex = 8;
		this.txtTotalDrAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalDrAmount.Location = new System.Drawing.Point(1, 23);
		this.txtTotalDrAmount.Name = "txtTotalDrAmount";
		this.txtTotalDrAmount.ShortcutsEnabled = false;
		this.txtTotalDrAmount.Size = new System.Drawing.Size(130, 23);
		this.txtTotalDrAmount.TabIndex = 7;
		this.txtTotalDrAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalDrAmount.TextChanged += new System.EventHandler(txtTotalDrAmount_TextChanged);
		this.txtTotalDrAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTotalDrAmount_KeyPress);
		this.lblDrAmount.AutoSize = true;
		this.lblDrAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDrAmount.Location = new System.Drawing.Point(4, 2);
		this.lblDrAmount.Name = "lblDrAmount";
		this.lblDrAmount.Size = new System.Drawing.Size(96, 15);
		this.lblDrAmount.TabIndex = 543;
		this.lblDrAmount.Text = "Dr Total Amount";
		this.DebitSubsidiaryAccountPanel.Controls.Add(this.lblDrSubsidiaryaccount);
		this.DebitSubsidiaryAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount);
		this.DebitSubsidiaryAccountPanel.Location = new System.Drawing.Point(421, 47);
		this.DebitSubsidiaryAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitSubsidiaryAccountPanel.Name = "DebitSubsidiaryAccountPanel";
		this.DebitSubsidiaryAccountPanel.Size = new System.Drawing.Size(172, 47);
		this.DebitSubsidiaryAccountPanel.TabIndex = 9;
		this.lblDrSubsidiaryaccount.AutoSize = true;
		this.lblDrSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDrSubsidiaryaccount.Location = new System.Drawing.Point(1, 1);
		this.lblDrSubsidiaryaccount.Name = "lblDrSubsidiaryaccount";
		this.lblDrSubsidiaryaccount.Size = new System.Drawing.Size(142, 15);
		this.lblDrSubsidiaryaccount.TabIndex = 553;
		this.lblDrSubsidiaryaccount.Text = "Debit Subsidiary Account";
		this.CmbDrSubsidiaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDrSubsidiaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDrSubsidiaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDrSubsidiaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDrSubsidiaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDrSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).Location = new System.Drawing.Point(1, 20);
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).Name = "CmbDrSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).TabIndex = 550;
		this.CmbDrSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbDrSubsidiaryAccount_AfterCloseUp);
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).Leave += new System.EventHandler(CmbDrSubsidiaryAccount_ValueChanged);
		this.DrRemarksPanel.Controls.Add(this.txtDrRemarks);
		this.DrRemarksPanel.Controls.Add(this.lblDrRemarks);
		this.DrRemarksPanel.Location = new System.Drawing.Point(593, 47);
		this.DrRemarksPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrRemarksPanel.Name = "DrRemarksPanel";
		this.DrRemarksPanel.Size = new System.Drawing.Size(329, 47);
		this.DrRemarksPanel.TabIndex = 10;
		this.txtDrRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDrRemarks.Location = new System.Drawing.Point(1, 23);
		this.txtDrRemarks.Name = "txtDrRemarks";
		this.txtDrRemarks.Size = new System.Drawing.Size(328, 23);
		this.txtDrRemarks.TabIndex = 8;
		this.lblDrRemarks.AutoSize = true;
		this.lblDrRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDrRemarks.Location = new System.Drawing.Point(1, 2);
		this.lblDrRemarks.Name = "lblDrRemarks";
		this.lblDrRemarks.Size = new System.Drawing.Size(84, 15);
		this.lblDrRemarks.TabIndex = 534;
		this.lblDrRemarks.Text = "Debit Remarks";
		this.ButtonsPanel.Controls.Add(this.btnAddDetail);
		this.ButtonsPanel.Controls.Add(this.btnUpdateDetail);
		this.ButtonsPanel.Controls.Add(this.btnCancelDetail);
		this.ButtonsPanel.Location = new System.Drawing.Point(922, 47);
		this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
		this.ButtonsPanel.Name = "ButtonsPanel";
		this.ButtonsPanel.Size = new System.Drawing.Size(138, 47);
		this.ButtonsPanel.TabIndex = 11;
		this.btnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddDetail.Location = new System.Drawing.Point(1, 19);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(46, 27);
		this.btnAddDetail.TabIndex = 9;
		this.btnAddDetail.Text = "+";
		this.btnAddDetail.UseVisualStyleBackColor = false;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.btnAddDetail.Leave += new System.EventHandler(btnAddDetail_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1, 19);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 27);
		this.btnUpdateDetail.TabIndex = 10;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(68, 19);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(66, 27);
		this.btnCancelDetail.TabIndex = 11;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(btnCancelDetail_Leave);
		this.MainBox.Controls.Add(this.txtVoucherRemarks);
		this.MainBox.Controls.Add(this.label8);
		this.MainBox.Controls.Add(this.label3);
		this.MainBox.Controls.Add(this.lblFcyAmount);
		this.MainBox.Controls.Add(this.txtFcyAmount);
		this.MainBox.Controls.Add(this.txtExchangeRate);
		this.MainBox.Controls.Add(this.ChkBoxPrintPreview);
		this.MainBox.Controls.Add(this.lblExchangeRate);
		this.MainBox.Controls.Add(this.chkformat2);
		this.MainBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.MainBox.Controls.Add(this.lblFcyCode);
		this.MainBox.Controls.Add(this.label7);
		this.MainBox.Controls.Add(this.txtDocDate);
		this.MainBox.Controls.Add(this.txtDocNo);
		this.MainBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTemporaryAccount);
		this.MainBox.Controls.Add(this.label4);
		this.MainBox.Controls.Add(this.lblTemporaryAccount);
		this.MainBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCashAccount);
		this.MainBox.Controls.Add(this.txtOpening);
		this.MainBox.Controls.Add(this.label1);
		this.MainBox.Controls.Add(this.label2);
		this.MainBox.Controls.Add(this.txtClosing);
		this.MainBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.MainBox.Location = new System.Drawing.Point(3, 35);
		this.MainBox.Name = "MainBox";
		this.MainBox.Size = new System.Drawing.Size(1239, 138);
		this.MainBox.TabIndex = 0;
		this.MainBox.TabStop = false;
		this.MainBox.Text = "Main";
		this.txtVoucherRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVoucherRemarks.Location = new System.Drawing.Point(447, 22);
		this.txtVoucherRemarks.Multiline = true;
		this.txtVoucherRemarks.Name = "txtVoucherRemarks";
		this.txtVoucherRemarks.Size = new System.Drawing.Size(319, 108);
		this.txtVoucherRemarks.TabIndex = 6;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(209, 26);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(32, 15);
		this.label8.TabIndex = 559;
		this.label8.Text = "Date";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(394, 26);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(52, 15);
		this.label3.TabIndex = 558;
		this.label3.Text = "Remarks";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFcyAmount.Location = new System.Drawing.Point(772, 80);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(72, 15);
		this.lblFcyAmount.TabIndex = 556;
		this.lblFcyAmount.Text = "Fcy Amount";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(856, 76);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(128, 23);
		this.txtFcyAmount.TabIndex = 9;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(856, 50);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(128, 23);
		this.txtExchangeRate.TabIndex = 8;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.ChkBoxPrintPreview.AutoSize = true;
		this.ChkBoxPrintPreview.Checked = true;
		this.ChkBoxPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBoxPrintPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrintPreview.Location = new System.Drawing.Point(990, 56);
		this.ChkBoxPrintPreview.Name = "ChkBoxPrintPreview";
		this.ChkBoxPrintPreview.Size = new System.Drawing.Size(95, 19);
		this.ChkBoxPrintPreview.TabIndex = 10;
		this.ChkBoxPrintPreview.Text = "Print Preview";
		this.ChkBoxPrintPreview.UseVisualStyleBackColor = true;
		this.lblExchangeRate.AutoSize = true;
		this.lblExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblExchangeRate.Location = new System.Drawing.Point(772, 54);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(84, 15);
		this.lblExchangeRate.TabIndex = 554;
		this.lblExchangeRate.Text = "Exchange Rate";
		this.chkformat2.AutoSize = true;
		this.chkformat2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkformat2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.chkformat2.Location = new System.Drawing.Point(990, 79);
		this.chkformat2.Margin = new System.Windows.Forms.Padding(2);
		this.chkformat2.Name = "chkformat2";
		this.chkformat2.Size = new System.Drawing.Size(119, 19);
		this.chkformat2.TabIndex = 11;
		this.chkformat2.Text = "Preview Format II";
		this.chkformat2.UseVisualStyleBackColor = true;
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
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
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(856, 20);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(128, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(772, 26);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 552;
		this.lblFcyCode.Text = "Fcy Code";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(12, 26);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(74, 15);
		this.label7.TabIndex = 511;
		this.label7.Text = "Vocher Code";
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(255, 22);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(135, 23);
		this.txtDocDate.TabIndex = 1;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(93, 22);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(116, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TabStop = false;
		this.CmbTemporaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTemporaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTemporaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTemporaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTemporaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTemporaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTemporaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTemporaryAccount).Location = new System.Drawing.Point(93, 104);
		((System.Windows.Forms.Control)(object)this.CmbTemporaryAccount).Name = "CmbTemporaryAccount";
		this.CmbTemporaryAccount.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbTemporaryAccount).Size = new System.Drawing.Size(297, 26);
		((System.Windows.Forms.Control)(object)this.CmbTemporaryAccount).TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(12, 54);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 15);
		this.label4.TabIndex = 516;
		this.label4.Text = "Cash Account";
		this.lblTemporaryAccount.AutoSize = true;
		this.lblTemporaryAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTemporaryAccount.Location = new System.Drawing.Point(12, 110);
		this.lblTemporaryAccount.Name = "lblTemporaryAccount";
		this.lblTemporaryAccount.Size = new System.Drawing.Size(81, 15);
		this.lblTemporaryAccount.TabIndex = 547;
		this.lblTemporaryAccount.Text = "Temporary Ac";
		this.cmbCashAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbCashAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCashAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCashAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCashAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.cmbCashAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCashAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCashAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Location = new System.Drawing.Point(93, 48);
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Name = "cmbCashAccount";
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Size = new System.Drawing.Size(297, 26);
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbCashAccount).Leave += new System.EventHandler(cmbCashAccount_Leave);
		this.txtOpening.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOpening.Location = new System.Drawing.Point(93, 77);
		this.txtOpening.Name = "txtOpening";
		this.txtOpening.ReadOnly = true;
		this.txtOpening.Size = new System.Drawing.Size(116, 23);
		this.txtOpening.TabIndex = 3;
		this.txtOpening.TabStop = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 81);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(56, 15);
		this.label1.TabIndex = 518;
		this.label1.Text = "Opening ";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(209, 81);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 15);
		this.label2.TabIndex = 520;
		this.label2.Text = "Closing";
		this.txtClosing.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtClosing.Location = new System.Drawing.Point(255, 77);
		this.txtClosing.Name = "txtClosing";
		this.txtClosing.ReadOnly = true;
		this.txtClosing.Size = new System.Drawing.Size(135, 23);
		this.txtClosing.TabIndex = 4;
		this.txtClosing.TabStop = false;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label18);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1242, 36);
		this.panel6.TabIndex = 504;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1201, 6);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 47;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.Teal;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Transparent;
		this.label18.Location = new System.Drawing.Point(7, 7);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(86, 19);
		this.label18.TabIndex = 25;
		this.label18.Text = "Day Book";
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1242, 30);
		this.panel3.TabIndex = 0;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnAttachment, this.btnShortCutKey });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1242, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click_1);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(62, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1248, 537);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.panel8);
		this.panel13.Controls.Add(this.panel12);
		this.panel13.Controls.Add(this.panel5);
		this.panel13.Controls.Add(this.panel11);
		this.panel13.Controls.Add(this.panel10);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1242, 531);
		this.panel13.TabIndex = 0;
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel8.Location = new System.Drawing.Point(627, 119);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(688, 412);
		this.panel8.TabIndex = 527;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridDebitHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(688, 382);
		this.panel9.TabIndex = 49;
		this.DataGridDebitHistory.AllowEdit = (InheritableBoolean)2;
		DataGridDebitHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridDebitHistory.DesignTimeLayout = DataGridDebitHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridDebitHistory.DynamicFiltering = true;
		this.DataGridDebitHistory.FilterMode = (FilterMode)2;
		this.DataGridDebitHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridDebitHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Name = "DataGridDebitHistory";
		((ScrollableBase)this.DataGridDebitHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).Size = new System.Drawing.Size(688, 382);
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).TabIndex = 4;
		this.DataGridDebitHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridDebitHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridDebitHistory).VisualStyle = (VisualStyle)4;
		this.DataGridDebitHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridDebitHistory_ColumnButtonClick);
		this.DataGridDebitHistory.LinkClicked += new ColumnActionEventHandler(DataGridDebitHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridDebitHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridDebitHistory_KeyDown);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(639, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridDebitHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 48;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(0, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(688, 30);
		this.label6.TabIndex = 5;
		this.label6.Text = "Debit Transactions";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel12.Controls.Add(this.panel7);
		this.panel12.Controls.Add(this.ctrlGrdBar3);
		this.panel12.Controls.Add(this.label5);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel12.Location = new System.Drawing.Point(0, 119);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(627, 412);
		this.panel12.TabIndex = 526;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridCreditHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 30);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(627, 382);
		this.panel7.TabIndex = 50;
		this.DataGridCreditHistory.AllowEdit = (InheritableBoolean)2;
		DataGridCreditHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridCreditHistory.DesignTimeLayout = DataGridCreditHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridCreditHistory.DynamicFiltering = true;
		this.DataGridCreditHistory.FilterMode = (FilterMode)2;
		this.DataGridCreditHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Name = "DataGridCreditHistory";
		((ScrollableBase)this.DataGridCreditHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).Size = new System.Drawing.Size(627, 382);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).TabIndex = 3;
		this.DataGridCreditHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridCreditHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridCreditHistory).VisualStyle = (VisualStyle)4;
		this.DataGridCreditHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridCreditHistory_ColumnButtonClick);
		this.DataGridCreditHistory.LinkClicked += new ColumnActionEventHandler(DataGridCreditHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridCreditHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridCreditHistory_KeyDown);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(583, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridCreditHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar3.TabIndex = 49;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(0, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(627, 30);
		this.label5.TabIndex = 4;
		this.label5.Text = "Credit Transactions";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory);
		this.panel5.Controls.Add(this.chkZkipCashTransaction);
		this.panel5.Controls.Add(this.BtnShow);
		this.panel5.Controls.Add(this.label14);
		this.panel5.Controls.Add(this.label15);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 62);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1242, 57);
		this.panel5.TabIndex = 524;
		this.datVouchderDateForHistory.AllowNull = (DefaultableBoolean)1;
		this.datVouchderDateForHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.datVouchderDateForHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.datVouchderDateForHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.datVouchderDateForHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datVouchderDateForHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).Location = new System.Drawing.Point(5, 26);
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).Name = "datVouchderDateForHistory";
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.datVouchderDateForHistory).TabIndex = 0;
		this.chkZkipCashTransaction.AutoSize = true;
		this.chkZkipCashTransaction.Location = new System.Drawing.Point(425, 6);
		this.chkZkipCashTransaction.Name = "chkZkipCashTransaction";
		this.chkZkipCashTransaction.Size = new System.Drawing.Size(262, 17);
		this.chkZkipCashTransaction.TabIndex = 3;
		this.chkZkipCashTransaction.Text = "Print Preview Without Cash Account Transactions";
		this.chkZkipCashTransaction.UseVisualStyleBackColor = true;
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(420, 26);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(58, 26);
		this.BtnShow.TabIndex = 2;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(5, 5);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(79, 15);
		this.label14.TabIndex = 465;
		this.label14.Text = "Voucher Date";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(187, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(80, 15);
		this.label15.TabIndex = 518;
		this.label15.Text = "Cash Account";
		this.CmbCashAccountForHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCashAccountForHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCashAccountForHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCashAccountForHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCashAccountForHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCashAccountForHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Location = new System.Drawing.Point(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Name = "CmbCashAccountForHistory";
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbCashAccountForHistory).TabIndex = 1;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label51);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 29);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1242, 33);
		this.panel11.TabIndex = 523;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(7, 7);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(175, 19);
		this.label51.TabIndex = 25;
		this.label51.Text = "DAY BOOK HISTORY";
		this.panel10.Controls.Add(this.toolStrip1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1242, 29);
		this.panel10.TabIndex = 522;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnPrint144, this.btnRefreshHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1242, 27);
		this.toolStrip1.TabIndex = 6;
		this.toolStrip1.Text = "toolStrip1";
		this.btnPrint144.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint144.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint144.Name = "btnPrint144";
		this.btnPrint144.Size = new System.Drawing.Size(92, 24);
		this.btnPrint144.Text = "144-Print";
		this.btnPrint144.Click += new System.EventHandler(btnPrint144_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1256, 563);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmDayBook";
		this.Text = "Day Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmDayBook_FormClosing);
		base.Load += new System.EventHandler(frmDayBook_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDayBook_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		this.SubCodePanel.ResumeLayout(false);
		this.SubCodePanel.PerformLayout();
		this.CreditAccountPanel.ResumeLayout(false);
		this.CreditAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCreditAccount).EndInit();
		this.CreditSubsidiaryPanel.ResumeLayout(false);
		this.CreditSubsidiaryPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrSubsidiaryAccount).EndInit();
		this.CrTotalAmountPanel.ResumeLayout(false);
		this.CrTotalAmountPanel.PerformLayout();
		this.CrRemarksPanel.ResumeLayout(false);
		this.CrRemarksPanel.PerformLayout();
		this.CheqNoPanel.ResumeLayout(false);
		this.CheqNoPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).EndInit();
		this.AmountPanel.ResumeLayout(false);
		this.AmountPanel.PerformLayout();
		this.DebitAccountPanel.ResumeLayout(false);
		this.DebitAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDebitAccount).EndInit();
		this.DrTotalAmountPanel.ResumeLayout(false);
		this.DrTotalAmountPanel.PerformLayout();
		this.DebitSubsidiaryAccountPanel.ResumeLayout(false);
		this.DebitSubsidiaryAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrSubsidiaryAccount).EndInit();
		this.DrRemarksPanel.ResumeLayout(false);
		this.DrRemarksPanel.PerformLayout();
		this.ButtonsPanel.ResumeLayout(false);
		this.MainBox.ResumeLayout(false);
		this.MainBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTemporaryAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCashAccount).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridDebitHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridCreditHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.datVouchderDateForHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCashAccountForHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
