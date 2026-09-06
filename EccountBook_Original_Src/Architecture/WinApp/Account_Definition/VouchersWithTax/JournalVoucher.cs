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
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition.VouchersWithTax;

public class JournalVoucher : Form
{
	private bool shiftTabPressed;

	private bool BranchFeature;

	private bool AfterCloseupDrSubsidiary;

	private bool AfterCloseupcrSubsidiary;

	private bool MultiCurrencyGridWorking;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool UpdateMode;

	private bool IsBookingOffice;

	private bool InventoryRelatedAccountsShowInVouchers;

	private int BaseCurrency;

	private double BaseRate;

	private int SubsdiaryAccountCrDatasourceLength;

	private int SubsdiaryAccountDrDatasourceLength;

	private int updateRowIndex = -1;

	public int RecId;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	public bool _AutoAdjust;

	private DataTable table = new DataTable();

	private DataTable dtDrAc = new DataTable();

	private DataTable dtSubsidory = new DataTable();

	private DataTable dtCurrencies = new DataTable();

	private DataTable dtVoucherDetailMultiCurrency = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip2;

	private ToolStripButton New;

	private ToolStripButton btnUpdate;

	private ToolStripButton Save;

	private ToolStripButton btnAttachment;

	private Label label23;

	private Panel panel5;

	private Panel panel4;

	private BackgroundWorker backgroundWorker1;

	private TextBox txtdocno;

	private DateTimePicker DocDate;

	private Label label13;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip3;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnplus;

	internal UltraCombo CmbCreditAccount;

	internal UltraCombo CmbDebitAccount;

	private Label label11;

	private Label label6;

	private TextBox txtCrRmks;

	private TextBox txtDrRmks;

	private Label lblCrComments;

	private Label label9;

	private Label label8;

	private Label lblDrComments;

	private TextBox txtAmount;

	private TextBox txtCheq;

	private TextBox txtCredit;

	private TextBox txtDebit;

	internal GridEX grd;

	internal GridEX DataGridHistory;

	private ToolStripButton Print;

	private ToolStripButton btnRefresh;

	internal GridEX grdDetail;

	private CheckBox ChkBoxPrintPreview;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private Label label1;

	private Label label2;

	private Label label3;

	private Panel panel12;

	private Label label4;

	internal UltraCombo CmbCrSubsidiaryAccount;

	private Label lblCrSubsidiaryaccount;

	internal UltraCombo CmbDrSubsidiaryAccount;

	private Label lblDrSubsidiaryaccount;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo CmbDrJobLot;

	private Label label7;

	internal UltraCombo CmbCrJobLot;

	private Label label5;

	private ToolStripButton btnShortCutKey;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

	private TextBox txtExchangeRate;

	private Label lblExchangeRate;

	private Label lblFcyAmount;

	private TextBox txtFcyAmount;

	internal UltraCombo CmbProjectId;

	private Label label10;

	private Panel panel6;

	private Panel panel7;

	private GroupBox groupBox3;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	internal UltraCombo cmbApproveStatus;

	private Label label15;

	private Label label14;

	private Label label18;

	private Label label17;

	private Label label16;

	private Button btnshow;

	private Panel panel8;

	private Panel panel9;

	private Panel panel10;

	private Panel panel11;

	private ToolStripButton btnNewHistory;

	private GroupBox VoucherInfoBox;

	private Label label36;

	private Label lblUnApprovedVouchers;

	private Label lblTotalVouchers;

	private Label label38;

	private Label lblApprovedVouchers;

	private Label label37;

	private RadioButton rdentapproved;

	private RadioButton rdentmodify;

	private RadioButton rdentdoc;

	private RadioButton rdententry;

	private TextBox txtVoucherremarks;

	internal UltraCombo CmbDrBranch;

	private Label label12;

	internal UltraCombo CmbCrBranch;

	private Label label19;

	private Panel panelDetailButtons;

	private Panel panelChqRefAndAmount;

	private Label labelSubsidiaryCreditBal;

	private Label labelSubsidiaryDebitBal;

	private Label labelDrSubsidiary;

	private Label labelCrSubsidiary;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel DebitAccountPanel;

	private Panel DebitSubsidiaryPanel;

	private Panel DrCommentsPanel;

	private Panel DrJobLotPanel;

	private Panel DrBranchPanel;

	private Panel DrCostCenterPanel;

	internal UltraCombo CmbDrCostCenter;

	private Label label20;

	private Panel CreditAmountPanel;

	private Panel CrSubsidiaryPanel;

	private Panel CrCommentsPanel;

	private Panel CrJobLotPanel;

	private Panel CrBranchPanel;

	private Panel CrCostCenterPanel;

	internal UltraCombo CmbCrCostCenter;

	private Label label21;

	private Label labelLocationType;

	internal UltraCombo cmbLocationType;

	private Label label22;

	private TextBox txtFcyAmountDetail;

	private Label label24;

	private CheckBox chkCustomAccounts;

	private RadioButton rdSearchByAccountCode;

	private RadioButton rdSearchByAccountName;

	private RadioButton rdSearchByCrAccountCode;

	private RadioButton rdSearchByCrAccountName;

	private TextBox txtBaseFcyCode;

	private Label lblBaseFcyCode;

	private TextBox txtBaseFcyExchangeRate;

	private Label lblBaseFcyExchangeRate;

	private Panel panel13;

	internal GridEX grdMultiCurrency;

	private Panel panel14;

	private Panel panel15;

	private Panel panel16;

	private Panel panel25;

	private TextBox txtTcyExchangeRateDetail;

	private Label label53;

	private Panel panel26;

	internal UltraCombo cmbTcyCodeDetail;

	private Label label54;

	private ToolStripButton btnCreateUpdateMultiCurrency;

	private ToolStripButton btnSaveAs;

	public JournalVoucher(UserAccount user)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbDebitAccount).ActiveRow == null || Conversion.ToInt(CmbDebitAccount.Value) == 0)
		{
			MessageBox.Show("Debit Account Field is Required");
			CmbDebitAccount.Focus();
			return false;
		}
		if (MultiCurrencyGridWorking && Conversion.ToInt(CmbDebitAccount.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[5].Value) == 0)
		{
			MessageBox.Show("Debit Account Currency not set Please Check");
			CmbDebitAccount.Focus();
			return false;
		}
		if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountDrDatasourceLength > 0 && ((UltraGridBase)CmbDrSubsidiaryAccount).ActiveRow == null)
		{
			MessageBox.Show("Debit Subsidiary Account Field Required");
			CmbDrSubsidiaryAccount.Focus();
			return false;
		}
		if (txtDrRmks.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Debit Remarks Field is Required");
			txtDrRmks.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDrJobLot).ActiveRow == null || Conversion.ToInt(CmbDrJobLot.Value) == 0)
		{
			MessageBox.Show("Dr JobLot Field is Required");
			CmbDrJobLot.Focus();
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
		if (Conversion.ToDouble((object)txtAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount Field is Required");
			txtAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAccount).ActiveRow == null || Conversion.ToInt(CmbCreditAccount.Value) == 0)
		{
			MessageBox.Show("Credit Account Field is Required");
			CmbCreditAccount.Focus();
			return false;
		}
		if (MultiCurrencyGridWorking && Conversion.ToInt(CmbCreditAccount.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[5].Value) == 0)
		{
			MessageBox.Show("Credit Account Currency not set Please Check");
			CmbCreditAccount.Focus();
			return false;
		}
		if (SubsidiaryAccountAllownOnVouchers && SubsdiaryAccountCrDatasourceLength > 0 && ((UltraGridBase)CmbCrSubsidiaryAccount).ActiveRow == null)
		{
			MessageBox.Show("Credit Subsidiary Account Field Required");
			CmbCrSubsidiaryAccount.Focus();
			return false;
		}
		if (txtCrRmks.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Credit Remarks Field is Required");
			txtCrRmks.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCrJobLot).ActiveRow == null || Conversion.ToInt(CmbCrJobLot.Value) == 0)
		{
			MessageBox.Show("Cr JobLot Field is Required");
			CmbCrJobLot.Focus();
			return false;
		}
		if (BranchFeature && (((UltraGridBase)CmbDrBranch).ActiveRow == null || Conversion.ToInt(CmbDrBranch.Value) == 0))
		{
			MessageBox.Show("Dr Branch Field is Required");
			CmbDrBranch.Focus();
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
		if (BranchFeature && (((UltraGridBase)CmbCrBranch).ActiveRow == null || Conversion.ToInt(CmbCrBranch.Value) == 0))
		{
			MessageBox.Show("Cr Branch Field is Required");
			CmbCrBranch.Focus();
			return false;
		}
		return true;
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

	private void VoucherCode()
	{
		try
		{
			txtdocno.Text = Conversion.ToString((object)CommonServices.GenerateVoucherCode(5));
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
				DDL.BindDDL(dtBranch, CmbDrBranch, "BranchId", "BranchName", "Dr Branch", ZeroIndex: false);
				DDL.BindDDL(dtBranch, CmbCrBranch, "BranchId", "BranchName", "Cr Branch", ZeroIndex: false);
				CmbDrBranch.Value = UserAccount.BranchesId;
				CmbCrBranch.Value = UserAccount.BranchesId;
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

	private void DebitAccountTitleFill()
	{
		try
		{
			int DrAccountId = Conversion.ToInt(CmbDebitAccount.Value);
			int CrAccountId = Conversion.ToInt(CmbCreditAccount.Value);
			dtDrAc.Rows.Clear();
			List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
			if (allAccounts == null)
			{
				return;
			}
			IEnumerable<getGlobalAllAccountsWithCustomGroup> filteredAccounts = allAccounts.AsEnumerable();
			int[] exclude = (InventoryRelatedAccountsShowInVouchers ? new int[1] : new int[2] { 4, 12 });
			int[] array = exclude;
			if (array != null && array.Length != 0)
			{
				filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => !exclude.Contains(a.AccountTypeId));
			}
			if (!InventoryRelatedAccountsShowInVouchers)
			{
				filteredAccounts = filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => !new int[1] { 2 }.Contains(a.PLNoteId));
			}
			HashSet<int> seenIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup account in filteredAccounts)
			{
				if (dtDrAc.Columns.Count == 0)
				{
					dtDrAc.Columns.Add("Id", typeof(int));
					dtDrAc.Columns.Add("AccountTitle");
					dtDrAc.Columns.Add("AccountCode");
					dtDrAc.Columns.Add("ParentAccount");
					dtDrAc.Columns.Add("AccountClass");
					dtDrAc.Columns.Add("CurrencyId");
					dtDrAc.Columns.Add("CurrencyCode");
				}
				int id = Conversion.ToInt((object)account.ChartOfAccountId);
				if (seenIds.Add(id))
				{
					dtDrAc.Rows.Add(id, account.AccountTitle, account.AccountCode, account.ParentAccountTitle, account.AccountClassName, account.CurrencyId, account.CurrencyCode);
				}
			}
			string displayField = (rdSearchByAccountName.Checked ? "AccountTitle" : "AccountCode");
			string headerText = (rdSearchByAccountName.Checked ? "Debit Account" : "Account Code");
			string displayFieldCr = (rdSearchByCrAccountName.Checked ? "AccountTitle" : "AccountCode");
			string headerTextCr = (rdSearchByCrAccountName.Checked ? "Credit Account" : "Account Code");
			InfragisticsHelper.BindAndRetainSelection(CmbDebitAccount, dtDrAc, "Id", displayField, headerText, AllColumns: true, DrAccountId, insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCreditAccount, dtDrAc, "Id", displayFieldCr, headerTextCr, AllColumns: true, CrAccountId, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtDrAc.Rows.Count > 0)
			{
				((UltraGridBase)CmbDebitAccount).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbCreditAccount).DisplayLayout.Bands[0].Columns[5].Hidden = true;
			}
			if (DrAccountId > 0)
			{
				CmbDr_Leave(null, null);
			}
			if (CrAccountId > 0)
			{
				CmbCr_Leave(null, null);
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

	private void DebitAccountCurrencyCheck()
	{
		try
		{
			if (MultiCurrencyGridWorking && Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[5].Value) == 0 && updateRowIndex == -1)
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

	private void CreditAccountCurrencyCheck()
	{
		try
		{
			if (MultiCurrencyGridWorking && Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[5].Value) == 0 && updateRowIndex == -1)
			{
				((Control)(object)CmbCreditAccount).Text = "";
				CmbCreditAccount.Focus();
				throw new Exception("Detail Account Currency Not set. Please Check!");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbDr_Leave(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbDebitAccount.Value) > 0)
			{
				DebitAccountCurrencyCheck();
			}
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DocDate.Value,
				RefAccountId = Conversion.ToInt(CmbDebitAccount.Value)
			});
			if (detail.Rows.Count > 0)
			{
				double ClosingBalance = Math.Round(Conversion.ToDouble(detail.Rows[0]["Balance"]));
				txtDebit.Text = ClosingBalance.ToString(clsGlobalVariables.stringFormatboth) + ((ClosingBalance > 0.0) ? "Dr" : ((ClosingBalance < 0.0) ? "Cr" : ""));
				txtDebit.Visible = true;
			}
			else
			{
				txtDebit.Text = "0";
			}
			if (((UltraGridBase)CmbDebitAccount).ActiveRow != null)
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

	private void CmbCr_Leave(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbCreditAccount.Value) > 0)
			{
				CreditAccountCurrencyCheck();
			}
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DocDate.Value,
				RefAccountId = Conversion.ToInt(CmbCreditAccount.Value)
			});
			if (detail.Rows.Count > 0)
			{
				txtCredit.Visible = true;
				double Balance = Conversion.ToDouble(detail.Rows[0]["Balance"]);
				txtCredit.Text = Balance.ToString(clsGlobalVariables.stringFormatboth) + ((Balance > 0.0) ? "Dr" : ((Balance < 0.0) ? "Cr" : ""));
			}
			else
			{
				txtCredit.Text = "0";
			}
			if (((UltraGridBase)CmbCreditAccount).ActiveRow != null)
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
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

	private void CostCenterFill()
	{
		try
		{
			int CrId = Conversion.ToInt(CmbCrCostCenter.Value);
			int DrId = Conversion.ToInt(CmbDrCostCenter.Value);
			DataTable dt = Projects.GetSubCostCenters(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, UserAccount.AppId, 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDrCostCenter, "Id", "CostCenterName", "Cost Center", ZeroIndex: false);
				DDL.BindDDL(dt, CmbCrCostCenter, "Id", "CostCenterName", "Cost Center", ZeroIndex: false);
				((UltraGridBase)CmbDrCostCenter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbDrCostCenter).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbDrCostCenter).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCrCostCenter).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbCrCostCenter).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCrCostCenter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (DrId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DrId))
					{
						CmbDrCostCenter.Value = DrId;
					}
					else
					{
						((Control)(object)CmbDrCostCenter).Text = string.Empty;
					}
				}
				if (CrId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == CrId))
					{
						CmbCrCostCenter.Value = CrId;
					}
					else
					{
						((Control)(object)CmbCrCostCenter).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbDrCostCenter).DataSource = null;
				((Control)(object)CmbDrCostCenter).Text = "";
				((UltraGridBase)CmbCrCostCenter).DataSource = null;
				((Control)(object)CmbCrCostCenter).Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int DrjobLot = Conversion.ToInt(CmbDrJobLot.Value);
			int CrjobLot = Conversion.ToInt(CmbCrJobLot.Value);
			List<JobLot> lst = jobLot.GetList(new JobLot
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("JobLotDescription");
			if (lst.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < lst.Count; i++)
			{
				dt.Rows.Add(lst[i].Id, lst[i].JobLotDescription);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCrJobLot, "Id", "JobLotDescription", "JobLot", ZeroIndex: true);
				DDL.BindDDL(dt, CmbDrJobLot, "Id", "JobLotDescription", "JobLot", ZeroIndex: true);
			}
			if (DrjobLot > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DrjobLot))
				{
					CmbDrJobLot.Value = DrjobLot;
				}
				else
				{
					((Control)(object)CmbDrJobLot).Text = string.Empty;
				}
			}
			if (CrjobLot > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == CrjobLot))
				{
					CmbCrJobLot.Value = CrjobLot;
				}
				else
				{
					((Control)(object)CmbCrJobLot).Text = string.Empty;
				}
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
			BaseCurrencyCheck();
			if (!FormValidation() || (Conversion.ToInt(CmbDebitAccount.Value) == Conversion.ToInt(CmbCreditAccount.Value) && MessageBox.Show("Debit Account and Credit Account should not be same please check ... ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No))
			{
				return;
			}
			int UniqueIdToRecognizeCreditDebitRowInDetail = 1;
			if (table.Rows.Count > 0)
			{
				UniqueIdToRecognizeCreditDebitRowInDetail = Conversion.ToInt((object)grd.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["LineId"].Value)));
				UniqueIdToRecognizeCreditDebitRowInDetail++;
			}
			table.Rows.Add(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[2].Value, CmbCreditAccount.Value, ((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[1].Value, MultiCurrencyGridWorking ? Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[5].Value) : 0, MultiCurrencyGridWorking ? Conversion.ToString(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[6].Value) : "", CmbDebitAccount.Value, (!SubsidiaryAccountAllownOnVouchers) ? ((object)0) : ((SubsdiaryAccountCrDatasourceLength > 0) ? CmbCrSubsidiaryAccount.Value : CmbCreditAccount.Value), (!SubsidiaryAccountAllownOnVouchers) ? "" : ((SubsdiaryAccountCrDatasourceLength > 0) ? ((Control)(object)CmbCrSubsidiaryAccount).Text : ((Control)(object)CmbCreditAccount).Text), (!SubsidiaryAccountAllownOnVouchers) ? ((object)0) : ((SubsdiaryAccountCrDatasourceLength > 0) ? ((UltraDropDownBase)CmbCrSubsidiaryAccount).SelectedRow.Cells[2].Value : ((object)4)), txtCrRmks.Text.Trim(), CmbCrJobLot.Value, ((Control)(object)CmbCrJobLot).Text, txtCheq.Text.Trim(), cmbTcyCodeDetail.Value, ((Control)(object)cmbTcyCodeDetail).Text, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), Conversion.ToDouble((object)txtFcyAmountDetail.Text), 0, txtAmount.Text.Trim(), UniqueIdToRecognizeCreditDebitRowInDetail, BranchFeature ? Conversion.ToInt(CmbCrBranch.Value) : 0, BranchFeature ? Conversion.ToString((object)((Control)(object)CmbCrBranch).Text) : "", (Conversion.ToInt(CmbCrCostCenter.Value) > 0) ? Conversion.ToInt(CmbCrCostCenter.Value) : 0, (Conversion.ToInt(CmbCrCostCenter.Value) > 0) ? Conversion.ToString((object)((Control)(object)CmbCrCostCenter).Text) : "", ((UniqueIdToRecognizeCreditDebitRowInDetail - 1) % 2 == 0) ? (UniqueIdToRecognizeCreditDebitRowInDetail - 1 + 2) : (UniqueIdToRecognizeCreditDebitRowInDetail - 1));
			table.Rows.Add(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value, CmbDebitAccount.Value, ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[1].Value, MultiCurrencyGridWorking ? Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[5].Value) : 0, MultiCurrencyGridWorking ? Conversion.ToString(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[6].Value) : "", CmbCreditAccount.Value, (!SubsidiaryAccountAllownOnVouchers) ? ((object)0) : ((SubsdiaryAccountDrDatasourceLength > 0) ? CmbDrSubsidiaryAccount.Value : CmbDebitAccount.Value), (!SubsidiaryAccountAllownOnVouchers) ? "" : ((SubsdiaryAccountDrDatasourceLength > 0) ? ((Control)(object)CmbDrSubsidiaryAccount).Text : ((Control)(object)CmbDebitAccount).Text), (!SubsidiaryAccountAllownOnVouchers) ? ((object)0) : ((SubsdiaryAccountDrDatasourceLength > 0) ? ((UltraDropDownBase)CmbDrSubsidiaryAccount).SelectedRow.Cells[2].Value : ((object)4)), txtDrRmks.Text.Trim(), CmbDrJobLot.Value, ((Control)(object)CmbDrJobLot).Text, txtCheq.Text.Trim(), cmbTcyCodeDetail.Value, ((Control)(object)cmbTcyCodeDetail).Text, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), Conversion.ToDouble((object)txtFcyAmountDetail.Text), txtAmount.Text.Trim(), 0, UniqueIdToRecognizeCreditDebitRowInDetail + 1, BranchFeature ? Conversion.ToInt(CmbDrBranch.Value) : 0, BranchFeature ? Conversion.ToString((object)((Control)(object)CmbDrBranch).Text) : "", (Conversion.ToInt(CmbDrCostCenter.Value) > 0) ? Conversion.ToInt(CmbDrCostCenter.Value) : 0, (Conversion.ToInt(CmbDrCostCenter.Value) > 0) ? Conversion.ToString((object)((Control)(object)CmbDrCostCenter).Text) : "", ((++UniqueIdToRecognizeCreditDebitRowInDetail - 1) % 2 == 0) ? (UniqueIdToRecognizeCreditDebitRowInDetail - 1 + 2) : (UniqueIdToRecognizeCreditDebitRowInDetail - 1));
			table.DefaultView.Sort = "SortIndex ASC";
			grd.DataSource = table;
			grd.RetrieveStructure();
			grd.Refetch();
			gridsetting();
			txtDrRmks.Clear();
			txtCrRmks.Clear();
			txtAmount.Clear();
			txtFcyAmountDetail.Clear();
			txtCheq.Clear();
			CmbDebitAccount.Focus();
			updateRowIndex = -1;
			CalculateTotalInformation();
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

	private void gridsetting()
	{
		try
		{
			grd.AllowEdit = (InheritableBoolean)2;
			grd.AllowDelete = (InheritableBoolean)2;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["GlCurrencyId"].Visible = false;
			grd.RootTable.Columns["LineId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["TcyCodeId"].Visible = false;
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["CostCenterId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccountTypeId"].Visible = false;
			grd.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grd.RootTable.Columns["CostCenter"].Visible = IsBookingOffice;
			grd.RootTable.Columns["AgainstAccountId"].Visible = false;
			grd.RootTable.Columns["SortIndex"].Visible = false;
			grd.RootTable.Columns["GlCurrency"].Visible = MultiCurrencyGridWorking;
			List<string> excludeColumnsFromSum = new List<string> { "FcyAmount" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, null, null, null, ColumnSetting: true, BracketsForamt: false, excludeColumnsFromSum);
			grd.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode - 10;
			grd.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 50;
			grd.RootTable.Columns["SubsidiaryAccount"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 150;
			grd.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant + 15;
			grd.RootTable.Columns["CheqNo"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["AmountDr"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["AmountCr"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["CostCenter"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["GlCurrency"].Width = Constants.InventoryConstants.FcyConstant + 15;
			grd.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grd.RootTable.Columns["TcyExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant + 20;
			grd.RootTable.Columns["FcyAmount"].Caption = "Tcy Amount";
			grd.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AmountDr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["AmountDr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AmountCr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["AmountCr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = "#,##0.###";
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Caption = "Delete";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "Delete";
			grd.RootTable.Columns["Delete"].Width = 50;
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
			grdMultiCurrency.TotalRow = (InheritableBoolean)2;
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

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grd.CurrentRow == null || (int)grd.CurrentRow.RowType != 0 || grd.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			btnplus.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelDetail.Visible = true;
			updateRowIndex = Conversion.ToInt(grd.CurrentRow.Cells["LineId"].Value);
			int UniqueId = Conversion.ToInt(grd.CurrentRow.Cells["LineId"].Value);
			if (UniqueId == 0)
			{
				throw new Exception("Record Not Update because LineId not found");
			}
			if (UniqueId % 2 == 0)
			{
				CmbCreditAccount.Value = grd.CurrentRow.Cells["AgainstAccountId"].Text.ToString();
				CmbDebitAccount.Value = grd.CurrentRow.Cells["AccountId"].Text.ToString();
				CmbDr_Leave(null, null);
				if (SubsdiaryAccountDrDatasourceLength == 0)
				{
					DataTable dt2 = new DataTable();
					dt2.Columns.Add("Id");
					dt2.Columns.Add("SubsidiaryAccount");
					dt2.Columns.Add("TypeId");
					dt2.Columns.Add("AccountId");
					dt2.Columns.Add("AccountTitle");
					dt2.Rows.Add(Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccountId"].Value), grd.CurrentRow.Cells["SubsidiaryAccount"].Text, 4, grd.CurrentRow.Cells["AccountId"].Text, grd.CurrentRow.Cells["AccountTitle"].Text);
					DDL.BindDDL(dt2, CmbDrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccountId"].Value) > 0)
				{
					CmbDrSubsidiaryAccount.Value = Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccountId"].Value);
				}
				txtDrRmks.Text = grd.CurrentRow.Cells["Remarks"].Text.ToString();
				txtCheq.Text = grd.CurrentRow.Cells["CheqNo"].Text.ToString();
				cmbTcyCodeDetail.Value = Conversion.ToInt(grd.CurrentRow.Cells["TcyCodeId"].Value);
				txtTcyExchangeRateDetail.Text = Conversion.ToDouble(grd.CurrentRow.Cells["TcyExchangeRate"].Value).ToString("#,##0.###");
				txtAmount.Text = grd.CurrentRow.Cells["AmountDr"].Text.ToString();
				txtFcyAmountDetail.Text = Conversion.ToDouble(grd.CurrentRow.Cells["FcyAmount"].Value).ToString("#,##0.###");
				if (Conversion.ToInt(grd.CurrentRow.Cells["JobLotId"].Value) > 0)
				{
					CmbDrJobLot.Value = Conversion.ToInt(grd.CurrentRow.Cells["JobLotId"].Value);
				}
				if (Conversion.ToInt(grd.CurrentRow.Cells["CostCenterId"].Value) > 0)
				{
					CmbDrCostCenter.Value = Conversion.ToInt(grd.CurrentRow.Cells["CostCenterId"].Value);
				}
				if (BranchFeature)
				{
					CmbDrBranch.Value = Conversion.ToInt(grd.CurrentRow.Cells["BranchId"].Value);
				}
				DataRow CrObject = null;
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (Conversion.ToInt(table.Rows[i]["LineId"]) == UniqueId - 1)
					{
						CrObject = table.Rows[i];
						break;
					}
				}
				if (CrObject != null)
				{
					CmbCreditAccount.Value = Conversion.ToInt(CrObject["AccountId"]);
					CmbCr_Leave(null, null);
					if (SubsdiaryAccountCrDatasourceLength == 0)
					{
						DataTable dt = new DataTable();
						dt.Columns.Add("Id");
						dt.Columns.Add("SubsidiaryAccount");
						dt.Columns.Add("TypeId");
						dt.Columns.Add("AccountId");
						dt.Columns.Add("AccountTitle");
						dt.Rows.Add(Conversion.ToInt(CrObject["SubsidiaryAccountId"]), CrObject["SubsidiaryAccount"], 4, CrObject["AccountId"], CrObject["AccountTitle"]);
						DDL.BindDDL(dt, CmbCrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
						((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					}
					CmbDebitAccount.Value = Conversion.ToInt(CrObject["AgainstAccountId"]);
					if (Conversion.ToInt(CrObject["SubsidiaryAccountId"]) > 0)
					{
						CmbCrSubsidiaryAccount.Value = Conversion.ToInt(CrObject["SubsidiaryAccountId"]);
					}
					txtCrRmks.Text = Conversion.ToString(CrObject["Remarks"]);
					cmbTcyCodeDetail.Value = Conversion.ToInt(CrObject["TcyCodeId"]);
					txtTcyExchangeRateDetail.Text = Conversion.ToDouble(CrObject["TcyExchangeRate"]).ToString("#,##0.###");
					if (Conversion.ToInt(CrObject["JobLotId"]) > 0)
					{
						CmbCrJobLot.Value = Conversion.ToInt(CrObject["JobLotId"]);
					}
					txtCheq.Text = Conversion.ToString(CrObject["CheqNo"]);
					txtAmount.Text = Conversion.ToString(CrObject["AmountCr"]);
					if (BranchFeature)
					{
						CmbCrBranch.Value = Conversion.ToInt(CrObject["BranchId"]);
					}
					if (Conversion.ToInt(CrObject["CostCenterId"]) > 0)
					{
						CmbCrCostCenter.Value = Conversion.ToInt(CrObject["CostCenterId"]);
					}
				}
			}
			else
			{
				CmbCreditAccount.Value = grd.CurrentRow.Cells["AccountId"].Text.ToString();
				CmbCr_Leave(null, null);
				if (SubsdiaryAccountCrDatasourceLength == 0)
				{
					DataTable dt4 = new DataTable();
					dt4.Columns.Add("Id");
					dt4.Columns.Add("SubsidiaryAccount");
					dt4.Columns.Add("TypeId");
					dt4.Columns.Add("AccountId");
					dt4.Columns.Add("AccountTitle");
					dt4.Rows.Add(Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccountId"].Value), grd.CurrentRow.Cells["SubsidiaryAccount"].Text, 4, grd.CurrentRow.Cells["AccountId"].Text, grd.CurrentRow.Cells["AccountTitle"].Text);
					DDL.BindDDL(dt4, CmbCrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
					((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				CmbDebitAccount.Value = grd.CurrentRow.Cells["AgainstAccountId"].Text.ToString();
				if (Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccountId"].Value) > 0)
				{
					CmbCrSubsidiaryAccount.Value = Conversion.ToInt(grd.CurrentRow.Cells["SubsidiaryAccountId"].Value);
				}
				txtCrRmks.Text = grd.CurrentRow.Cells["Remarks"].Text.ToString();
				txtCheq.Text = grd.CurrentRow.Cells["CheqNo"].Text.ToString();
				cmbTcyCodeDetail.Value = Conversion.ToInt(grd.CurrentRow.Cells["TcyCodeId"].Value);
				txtTcyExchangeRateDetail.Text = Conversion.ToDouble(grd.CurrentRow.Cells["TcyExchangeRate"].Value).ToString("#,##0.###");
				if (Conversion.ToInt(grd.CurrentRow.Cells["JobLotId"].Value) > 0)
				{
					CmbCrJobLot.Value = Conversion.ToInt(grd.CurrentRow.Cells["JobLotId"].Value);
				}
				txtAmount.Text = grd.CurrentRow.Cells["AmountCr"].Text.ToString();
				if (BranchFeature)
				{
					CmbCrBranch.Value = Conversion.ToInt(grd.CurrentRow.Cells["BranchId"].Value);
				}
				if (Conversion.ToInt(grd.CurrentRow.Cells["CostCenterId"].Value) > 0)
				{
					CmbCrCostCenter.Value = Conversion.ToInt(grd.CurrentRow.Cells["CostCenterId"].Value);
				}
				txtFcyAmountDetail.Text = Conversion.ToDouble(grd.CurrentRow.Cells["FcyAmount"].Value).ToString("#,##0.###");
				DataRow DrObject = null;
				for (int j = 0; j < table.Rows.Count; j++)
				{
					if (Conversion.ToInt(table.Rows[j]["LineId"]) == UniqueId + 1)
					{
						DrObject = table.Rows[j];
						break;
					}
				}
				if (DrObject != null)
				{
					CmbDebitAccount.Value = DrObject["AccountId"];
					CmbDr_Leave(null, null);
					if (SubsdiaryAccountDrDatasourceLength == 0)
					{
						DataTable dt3 = new DataTable();
						dt3.Columns.Add("Id");
						dt3.Columns.Add("SubsidiaryAccount");
						dt3.Columns.Add("TypeId");
						dt3.Columns.Add("AccountId");
						dt3.Columns.Add("AccountTitle");
						dt3.Rows.Add(Conversion.ToInt(DrObject["SubsidiaryAccountId"]), DrObject["SubsidiaryAccount"], 4, DrObject["AccountId"], DrObject["AccountTitle"]);
						DDL.BindDDL(dt3, CmbDrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Subsidiary Account ", ZeroIndex: false);
						((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					}
					CmbCreditAccount.Value = DrObject["AgainstAccountId"];
					if (Conversion.ToInt(DrObject["SubsidiaryAccountId"]) > 0)
					{
						CmbDrSubsidiaryAccount.Value = DrObject["SubsidiaryAccountId"];
					}
					txtDrRmks.Text = Conversion.ToString(DrObject["Remarks"]);
					if (Conversion.ToInt(DrObject["JobLotId"]) > 0)
					{
						CmbDrJobLot.Value = Conversion.ToInt(DrObject["JobLotId"]);
					}
					cmbTcyCodeDetail.Value = Conversion.ToInt(DrObject["TcyCodeId"]);
					txtTcyExchangeRateDetail.Text = Conversion.ToDouble(DrObject["TcyExchangeRate"]).ToString("#,##0.###");
					txtCheq.Text = Conversion.ToString(DrObject["CheqNo"]);
					txtAmount.Text = Conversion.ToString(DrObject["AmountDr"]);
					if (BranchFeature)
					{
						CmbDrBranch.Value = Conversion.ToInt(DrObject["BranchId"]);
					}
					if (Conversion.ToInt(DrObject["CostCenterId"]) > 0)
					{
						CmbDrCostCenter.Value = Conversion.ToInt(DrObject["CostCenterId"]);
					}
				}
			}
			CmbDebitAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidation() || !FormValidation() || updateRowIndex < 0)
			{
				return;
			}
			bool num = updateRowIndex % 2 == 0;
			int debitLineId = (num ? updateRowIndex : (updateRowIndex + 1));
			int creditLineId = (num ? (updateRowIndex - 1) : updateRowIndex);
			DataRow debitRow = GetRowByLineId(debitLineId);
			DataRow creditRow = GetRowByLineId(creditLineId);
			if (debitRow != null && creditRow != null)
			{
				double amount = Conversion.ToDouble((object)txtAmount.Text);
				double fcyAmount = Conversion.ToDouble((object)txtFcyAmountDetail.Text);
				FillDebitRow(debitRow, amount, fcyAmount, debitLineId);
				FillCreditRow(creditRow, amount, fcyAmount, creditLineId);
				txtExchangeRate_TextChanged(null, null);
				((Control)(object)CmbDebitAccount).Text = string.Empty;
				CmbDr_Leave(null, null);
				((Control)(object)CmbCreditAccount).Text = string.Empty;
				CmbCr_Leave(null, null);
				txtCrRmks.Clear();
				txtDrRmks.Clear();
				txtAmount.Clear();
				txtFcyAmountDetail.Clear();
				txtCheq.Clear();
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				CmbDebitAccount.Focus();
				updateRowIndex = -1;
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

	private DataRow GetRowByLineId(int lineId)
	{
		try
		{
			return table.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt(r["LineId"]) == lineId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillDebitRow(DataRow row, double amount, double fcyAmount, int lineId)
	{
		try
		{
			row["AccountCode"] = ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[2].Value;
			row["AccountId"] = CmbDebitAccount.Value;
			row["AccountTitle"] = ((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[1].Value;
			row["AgainstAccountId"] = CmbCreditAccount.Value;
			FillSubsidiary(row, isDebit: true);
			row["GlCurrencyId"] = (MultiCurrencyGridWorking ? Conversion.ToInt(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[5].Value) : 0);
			row["GlCurrency"] = (MultiCurrencyGridWorking ? Conversion.ToString(((UltraDropDownBase)CmbDebitAccount).SelectedRow.Cells[6].Value) : "");
			row["Remarks"] = txtDrRmks.Text;
			row["JobLotId"] = CmbDrJobLot.Value;
			row["JobLot"] = ((Control)(object)CmbDrJobLot).Text;
			row["CheqNo"] = txtCheq.Text;
			row["TcyCodeId"] = cmbTcyCodeDetail.Value;
			row["TcyCode"] = ((Control)(object)cmbTcyCodeDetail).Text;
			row["TcyExchangeRate"] = txtTcyExchangeRateDetail.Text;
			row["FcyAmount"] = fcyAmount;
			row["AmountDr"] = amount;
			row["AmountCr"] = 0;
			row["LineId"] = lineId;
			row["BranchId"] = Conversion.ToInt(CmbDrBranch.Value);
			row["BranchName"] = ((Control)(object)CmbDrBranch).Text;
			row["CostCenterId"] = Conversion.ToInt(CmbDrCostCenter.Value);
			row["CostCenter"] = ((Control)(object)CmbDrCostCenter).Text;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillCreditRow(DataRow row, double amount, double fcyAmount, int lineId)
	{
		try
		{
			row["AccountCode"] = ((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[2].Value;
			row["AccountId"] = CmbCreditAccount.Value;
			row["AccountTitle"] = ((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[1].Value;
			row["AgainstAccountId"] = CmbDebitAccount.Value;
			row["GlCurrencyId"] = (MultiCurrencyGridWorking ? Conversion.ToInt(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[5].Value) : 0);
			row["GlCurrency"] = (MultiCurrencyGridWorking ? Conversion.ToString(((UltraDropDownBase)CmbCreditAccount).SelectedRow.Cells[6].Value) : "");
			FillSubsidiary(row, isDebit: false);
			row["Remarks"] = txtCrRmks.Text;
			row["JobLotId"] = CmbCrJobLot.Value;
			row["JobLot"] = ((Control)(object)CmbCrJobLot).Text;
			row["CheqNo"] = txtCheq.Text;
			row["TcyCodeId"] = cmbTcyCodeDetail.Value;
			row["TcyCode"] = ((Control)(object)cmbTcyCodeDetail).Text;
			row["TcyExchangeRate"] = txtTcyExchangeRateDetail.Text;
			row["FcyAmount"] = fcyAmount;
			row["AmountDr"] = 0;
			row["AmountCr"] = amount;
			row["LineId"] = lineId;
			row["BranchId"] = Conversion.ToInt(CmbCrBranch.Value);
			row["BranchName"] = ((Control)(object)CmbCrBranch).Text;
			row["CostCenterId"] = Conversion.ToInt(CmbCrCostCenter.Value);
			row["CostCenter"] = ((Control)(object)CmbCrCostCenter).Text;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillSubsidiary(DataRow row, bool isDebit)
	{
		try
		{
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				row["SubsidiaryAccountId"] = 0;
				row["SubsidiaryAccount"] = "";
				row["SubsidiaryAccountTypeId"] = 0;
				return;
			}
			UltraCombo cmb = (isDebit ? CmbDrSubsidiaryAccount : CmbCrSubsidiaryAccount);
			UltraCombo main = (isDebit ? CmbDebitAccount : CmbCreditAccount);
			if (((UltraGridBase)cmb).ActiveRow != null && !string.IsNullOrEmpty(((Control)(object)cmb).Text))
			{
				row["SubsidiaryAccountId"] = cmb.Value;
				row["SubsidiaryAccount"] = ((Control)(object)cmb).Text;
				row["SubsidiaryAccountTypeId"] = ((UltraDropDownBase)cmb).SelectedRow.Cells[2].Value;
			}
			else
			{
				row["SubsidiaryAccountId"] = main.Value;
				row["SubsidiaryAccount"] = ((UltraDropDownBase)main).SelectedRow.Cells[1].Value;
				row["SubsidiaryAccountTypeId"] = 4;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			CmbCr_Leave(null, null);
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			CmbDr_Leave(null, null);
			txtCrRmks.Clear();
			txtDrRmks.Clear();
			txtAmount.Clear();
			txtFcyAmountDetail.Clear();
			txtCheq.Clear();
			updateRowIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "Delete")
			{
				int uniqueId = Conversion.ToInt(item.Cells["LineId"].Value);
				int relatedId = ((uniqueId % 2 == 0) ? (uniqueId - 1) : (uniqueId + 1));
				DataRow[] relatedRows = table.Select($"LineId = '{relatedId}'");
				if (relatedRows.Length != 0)
				{
					relatedRows[0].Delete();
				}
				DataRow[] currentRows = table.Select($"LineId = '{uniqueId}'");
				if (currentRows.Length != 0)
				{
					currentRows[0].Delete();
				}
				table.AcceptChanges();
				grd.DataSource = null;
				grd.DataSource = table;
				grd.Refetch();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			BranchesFill();
			LocationType();
			CurrencyFill();
			combojoblotfill();
			DebitAccountTitleFill();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				BindAllSubsidiaryaccounts();
			}
			DefaultConfigurations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void New_Click(object sender, EventArgs e)
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
			RecId = 0;
			txtFcyAmount.Text = string.Empty;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			BindCreditSubsidiaryAccount();
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			BindDebitSubsidiaryAccount();
			txtVoucherremarks.Text = string.Empty;
			txtDebit.Text = "";
			txtCredit.Text = "";
			txtCrRmks.Clear();
			txtDrRmks.Clear();
			txtFcyAmountDetail.Clear();
			txtAmount.Clear();
			txtCheq.Clear();
			table.Clear();
			Save.Visible = true;
			btnUpdate.Visible = false;
			grd.ClearStructure();
			VoucherCode();
			updateRowIndex = -1;
			DocDate.Focus();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			dtVoucherDetailMultiCurrency.Rows.Clear();
			grdMultiCurrency.DataSource = dtVoucherDetailMultiCurrency;
			cmbCurrency_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DayBookVoucher_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 600);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			Save.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			BranchesFill();
			CombProjectFill();
			BranchFeature = CommonServices.GetERPFeatureById(17);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				BindAllSubsidiaryaccounts();
				DebitSubsidiaryPanel.Visible = true;
				CrSubsidiaryPanel.Visible = true;
				lblCrSubsidiaryaccount.Visible = true;
				((Control)(object)CmbCrSubsidiaryAccount).Visible = true;
				lblDrSubsidiaryaccount.Visible = true;
				((Control)(object)CmbDrSubsidiaryAccount).Visible = true;
			}
			else
			{
				DebitSubsidiaryPanel.Visible = false;
				CrSubsidiaryPanel.Visible = false;
				labelCrSubsidiary.Visible = false;
				labelSubsidiaryCreditBal.Visible = false;
				lblCrSubsidiaryaccount.Visible = false;
				((Control)(object)CmbCrSubsidiaryAccount).Visible = false;
				labelDrSubsidiary.Visible = false;
				labelSubsidiaryDebitBal.Visible = false;
				lblDrSubsidiaryaccount.Visible = false;
				((Control)(object)CmbDrSubsidiaryAccount).Visible = false;
			}
			if (BranchFeature)
			{
				CrBranchPanel.Visible = true;
				DrBranchPanel.Visible = true;
				label12.Visible = true;
				((Control)(object)CmbDrBranch).Visible = true;
				label19.Visible = true;
				((Control)(object)CmbCrBranch).Visible = true;
			}
			else
			{
				CrBranchPanel.Visible = false;
				DrBranchPanel.Visible = false;
				label12.Visible = false;
				((Control)(object)CmbDrBranch).Visible = false;
				label19.Visible = false;
				((Control)(object)CmbCrBranch).Visible = false;
			}
			MultiCurrencyGridWorking = CommonServices.GetERPFeatureById(6);
			if (!MultiCurrencyGridWorking)
			{
				((Control)(object)grdMultiCurrency).Visible = false;
				lblBaseFcyCode.Visible = false;
				txtBaseFcyCode.Visible = false;
				lblBaseFcyExchangeRate.Visible = false;
				txtBaseFcyExchangeRate.Visible = false;
			}
			CurrencyFill();
			InventoryRelatedAccountsShowInVouchers = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("InventoryRelatedAccountsShowInVouchers"));
			table.Columns.Add("AccountCode");
			table.Columns.Add("AccountId");
			table.Columns.Add("AccountTitle");
			table.Columns.Add("GlCurrencyId", typeof(int));
			table.Columns.Add("GlCurrency");
			table.Columns.Add("AgainstAccountId");
			table.Columns.Add("SubsidiaryAccountId");
			table.Columns.Add("SubsidiaryAccount");
			table.Columns.Add("SubsidiaryAccountTypeId");
			table.Columns.Add("Remarks");
			table.Columns.Add("JobLotId");
			table.Columns.Add("JobLot");
			table.Columns.Add("CheqNo");
			table.Columns.Add("TcyCodeId", typeof(int));
			table.Columns.Add("TcyCode");
			table.Columns.Add("TcyExchangeRate", typeof(double));
			table.Columns.Add("FcyAmount", typeof(double));
			table.Columns.Add("AmountDr", typeof(double));
			table.Columns.Add("AmountCr", typeof(double));
			table.Columns.Add("LineId");
			table.Columns.Add("BranchId", typeof(int));
			table.Columns.Add("BranchName");
			table.Columns.Add("CostCenterId", typeof(int));
			table.Columns.Add("CostCenter");
			table.Columns.Add("SortIndex", typeof(int));
			grd.DataSource = table;
			grd.RetrieveStructure();
			gridsetting();
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
			dtDrAc.Columns.Add("Id", typeof(int));
			dtDrAc.Columns.Add("AccountTitle");
			dtDrAc.Columns.Add("AccountCode");
			dtDrAc.Columns.Add("ParentAccount");
			dtDrAc.Columns.Add("AccountClass");
			dtDrAc.Columns.Add("CurrencyId");
			dtDrAc.Columns.Add("CurrencyCode");
			combojoblotfill();
			DebitAccountTitleFill();
			LocationType();
			CostCenterFill();
			ApplicationsAllocateToCompany obj = clsGlobalVariables.GetApplicationsAllocateToCompanyList.Find((ApplicationsAllocateToCompany row) => row.AppId == 5 && row.CompanyId == UserAccount.CompanyId);
			IsBookingOffice = obj != null && obj.IsActive;
			if (!IsBookingOffice)
			{
				CrCostCenterPanel.Visible = false;
				DrCostCenterPanel.Visible = false;
			}
			if (IsBookingOffice && BranchFeature && !SubsidiaryAccountAllownOnVouchers)
			{
				DrCommentsPanel.Size = new Size(353, 47);
				txtDrRmks.Size = new Size(353, 23);
				CrCommentsPanel.Size = new Size(353, 47);
				txtCrRmks.Size = new Size(353, 23);
			}
			else if (IsBookingOffice && !BranchFeature && !SubsidiaryAccountAllownOnVouchers)
			{
				DrCommentsPanel.Size = new Size(500, 47);
				txtDrRmks.Size = new Size(500, 23);
				CrCommentsPanel.Size = new Size(500, 47);
				txtCrRmks.Size = new Size(500, 23);
			}
			else if (!IsBookingOffice && BranchFeature && !SubsidiaryAccountAllownOnVouchers)
			{
				DrCommentsPanel.Size = new Size(476, 47);
				txtDrRmks.Size = new Size(476, 23);
				CrCommentsPanel.Size = new Size(476, 47);
				txtCrRmks.Size = new Size(476, 23);
			}
			else if (!IsBookingOffice && !BranchFeature && !SubsidiaryAccountAllownOnVouchers)
			{
				DrCommentsPanel.Size = new Size(350, 47);
				txtDrRmks.Size = new Size(350, 23);
				CrCommentsPanel.Size = new Size(350, 47);
				txtCrRmks.Size = new Size(350, 23);
			}
			else if (!IsBookingOffice && !BranchFeature && SubsidiaryAccountAllownOnVouchers)
			{
				DrCommentsPanel.Size = new Size(455, 47);
				txtDrRmks.Size = new Size(455, 23);
				CrCommentsPanel.Size = new Size(455, 47);
				txtCrRmks.Size = new Size(455, 23);
			}
			else if (!IsBookingOffice && BranchFeature && SubsidiaryAccountAllownOnVouchers)
			{
				DrCommentsPanel.Size = new Size(308, 47);
				txtDrRmks.Size = new Size(308, 23);
				CrCommentsPanel.Size = new Size(308, 47);
				txtCrRmks.Size = new Size(308, 23);
			}
			txtCredit.Visible = false;
			txtDebit.Visible = false;
			ChkBoxPrintPreview.Checked = true;
			DocDate.Focus();
			Save.Visible = true;
			btnUpdate.Visible = false;
			VoucherCode();
			DefaultConfigurations();
			StatusFill();
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
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

	private void DefaultConfigurations()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbDrJobLot.Value = JobId;
				CmbCrJobLot.Value = JobId;
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

	private void btnSaveAs_Click(object sender, EventArgs e)
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

	private void Insert()
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Expected O, but got Unknown
		//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Expected O, but got Unknown
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c85: Expected O, but got Unknown
		//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9f: Expected O, but got Unknown
		//IL_0e46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8c: Expected O, but got Unknown
		//IL_0ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			BaseCurrencyCheck();
			if (Conversion.ToInt(CmbProjectId.Value) == 0)
			{
				MessageBox.Show("Cost Center Field is Required");
				CmbProjectId.Focus();
			}
			else if (((UltraGridBase)cmbLocationType).ActiveRow == null || Conversion.ToInt(cmbLocationType.Value) == 0)
			{
				MessageBox.Show("Location Type Field is Required");
				cmbLocationType.Focus();
			}
			else if (grd.RowCount > 0)
			{
				if (Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					MessageBox.Show("Fcy Code Field is Required");
					cmbCurrency.Focus();
					return;
				}
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
				{
					MessageBox.Show("Exchange Rate Field is Required");
					txtExchangeRate.Focus();
					return;
				}
				if (Conversion.ToDouble((object)txtFcyAmount.Text) == 0.0)
				{
					MessageBox.Show("Fcy Amount Field is Required");
					txtFcyAmount.Focus();
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
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (Conversion.ToDouble(r3.Cells["AmountDr"].Value) > 0.0 && Conversion.ToInt(r3.Cells["AccountId"].Value) == 0)
					{
						MessageBox.Show("Please Select Debit Account Title First");
						return;
					}
				}
				rows = grd.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToDouble(r2.Cells["AmountCr"].Value) > 0.0 && Conversion.ToInt(r2.Cells["AccountId"].Value) == 0)
					{
						MessageBox.Show("Please Select Credit Account Title First");
						return;
					}
				}
				vh.DocumentTypeId = 5;
				vh.BaseDocumentTypeId = 4;
				vh.DocumentTypeSrNo = 0;
				vh.VoucherCode = Conversion.ToInt((object)txtdocno.Text.Trim());
				vh.ProjectId = Conversion.ToInt((object)CmbProjectId.Value.ToString());
				vh.VoucherDate = DocDate.Value;
				vh.RemarksOtherLingo = "";
				vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				vh.ChequeNo = txtCheq.Text.Trim().ToString();
				vh.Remarks = Conversion.ToString((object)txtVoucherremarks.Text);
				vh.EntryDate = DateTime.Now;
				vh.EntryUser = UserAccount.ID;
				vh.ModifyDate = DateTime.Now;
				vh.ModifyUser = UserAccount.ID;
				vh.OrganizationId = UserAccount.OrganizationId;
				vh.CompanyId = UserAccount.CompanyId;
				vh.BranchId = UserAccount.BranchesId;
				vh.MultiCurrencyId = Conversion.ToInt(cmbCurrency.Value);
				vh.ExchangeCurrencyRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				vh.FcAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
				vh.CustomAccounts = chkCustomAccounts.Checked;
				vh.voucherDetailList = new List<VoucherDetail>();
				rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					VoucherDetail vd = new VoucherDetail();
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
					vd.DebitAmount = Conversion.ToDouble(r.Cells["AmountDr"].Value);
					vd.CreditAmount = Conversion.ToDouble(r.Cells["AmountCr"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.LineId = Conversion.ToInt(r.Cells["LineId"].Value);
					vd.SortNo = Conversion.ToInt((object)(r.RowIndex + 1));
					vd.DMultiCurrencyId = Conversion.ToInt(r.Cells["TcyCodeId"].Value);
					vd.DExchangeCurrencyRate = Conversion.ToDouble(r.Cells["TcyExchangeRate"].Value);
					vd.DCurrencyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
					vd.ActionId = ((RecId <= 0) ? 1 : 2);
					vd.BranchesId = (BranchFeature ? Conversion.ToInt(r.Cells["BranchId"].Value) : UserAccount.BranchesId);
					vd.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
					vd.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
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
						vd.ThirdCurrencyAmount = ((vd.DebitAmount > 0.0) ? vd.DebitAmount : (vd.CreditAmount / vd.ThirdCurrencyFcyExchangeRate));
						vd.BaseFcyId = Conversion.ToInt(txtBaseFcyCode.Tag);
						if (vd.DMultiCurrencyId == vd.BaseFcyId)
						{
							vd.BaseFcyExchangeRate = vd.DExchangeCurrencyRate;
						}
						else
						{
							vd.BaseFcyExchangeRate = Conversion.ToDouble((object)txtBaseFcyExchangeRate.Text);
						}
						vd.BaseFcyAmount = ((vd.DebitAmount > 0.0) ? vd.DebitAmount : (vd.CreditAmount / vd.BaseFcyExchangeRate));
					}
					vh.voucherDetailList.Add(vd);
				}
				object Debit = grd.GetTotal(grd.RootTable.Columns["AmountDr"], (AggregateFunction)2);
				object total = grd.GetTotal(grd.RootTable.Columns["AmountCr"], (AggregateFunction)2);
				double DebitAmount = Conversion.ToDouble(Debit);
				if (Conversion.ToDouble(total) != DebitAmount)
				{
					MessageBox.Show("Debit & Credit side not equal");
					return;
				}
				DataTable source = (DataTable)grd.DataSource;
				var rowsDr = (from x in source.AsEnumerable().Select((DataRow row, int index) => new
					{
						Row = row,
						RowIndex = index
					})
					where x.Row.Field<int>("CostCenterId") > 0
					select new
					{
						RowIndex = x.RowIndex + 1,
						Amount = x.Row.Field<double>("AmountDr"),
						CostCenterId = x.Row.Field<int>("CostCenterId")
					}).ToList();
				var rowsCr = (from x in source.AsEnumerable().Select((DataRow row, int index) => new
					{
						Row = row,
						RowIndex = index
					})
					where x.Row.Field<int>("CostCenterId") > 0
					select new
					{
						RowIndex = x.RowIndex + 1,
						Amount = x.Row.Field<double>("AmountCr"),
						CostCenterId = x.Row.Field<int>("CostCenterId")
					}).ToList();
				vh.costCenterDetailList = new List<voucherCostCenterDetail>();
				if (rowsDr.Count > 0 || rowsCr.Count > 0)
				{
					foreach (var item3 in rowsDr)
					{
						voucherCostCenterDetail d3 = new voucherCostCenterDetail();
						d3.Id = 0;
						d3.SortNo = Conversion.ToInt((object)item3.RowIndex);
						d3.CostCenterId = Conversion.ToInt((object)item3.CostCenterId);
						d3.costPrcent = 100m;
						d3.costAmount = Conversion.ToDouble((object)item3.Amount);
						vh.costCenterDetailList.Add(d3);
					}
					foreach (var item2 in rowsCr)
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
				vh.costCenterDetailList.Sort((voucherCostCenterDetail p1, voucherCostCenterDetail p2) => p1.SortNo.CompareTo(p2.SortNo));
				vh.VoucherAmount = DebitAmount;
				if (MultiCurrencyGridWorking)
				{
					rows = grdMultiCurrency.GetDataRows();
					foreach (GridEXRow item in rows)
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
				if (string.IsNullOrEmpty(AttachmentsValues) || AT.lst.Count > 0)
				{
					AttachmentsValues = "";
				}
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
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, vh.DocumentTypeId, base.Name);
				}
				Reset();
				if (ChkBoxPrintPreview.Checked)
				{
					DataTable dt = new DataTable();
					dt = VoucherReports.VoucherReport(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 5,
						Id = success,
						ApprovedFilter = "All"
					});
					if (dt == null || dt.Rows.Count == 0)
					{
						MessageBox.Show("Not Record Found For Display");
					}
					else
					{
						Reporting val = new Reporting();
						val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
					}
				}
				Save.Visible = true;
				grd.ClearStructure();
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

	private void btnUpdate_Click(object sender, EventArgs e)
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

	public void ReadById(int ID, bool SaveAs = false)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		try
		{
			Reset();
			RecId = Conversion.ToInt((object)ID);
			VoucherHead voucherHead = new VoucherHead();
			voucherHead = VoucherHead.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			CmbProjectId.Value = voucherHead.ProjectId;
			DocDate.Value = voucherHead.VoucherDate;
			txtdocno.Text = voucherHead.VoucherCode.ToString();
			txtVoucherremarks.Text = Conversion.ToString((object)voucherHead.Remarks);
			cmbCurrency.Value = voucherHead.MultiCurrencyId;
			txtExchangeRate.Text = voucherHead.ExchangeCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			txtFcyAmount.Text = voucherHead.FcAmount.ToString("#,##0.###");
			chkCustomAccounts.Checked = voucherHead.CustomAccounts;
			if (voucherHead.MultiCurrencyId == 0 || voucherHead.ExchangeCurrencyRate == 0.0)
			{
				DefaultConfigurations();
			}
			AttachmentsValues = voucherHead.AttachmentsValues;
			CustomAttachmentsValues = voucherHead.CustomAttachmentsValues;
			grd.DataSource = null;
			grd.RetrieveStructure();
			table.DefaultView.Sort = null;
			table.Clear();
			table.Columns.Clear();
			if (table.Columns.Count == 0)
			{
				table.Columns.Add("AccountCode");
				table.Columns.Add("AccountId");
				table.Columns.Add("AccountTitle");
				table.Columns.Add("GlCurrencyId", typeof(int));
				table.Columns.Add("GlCurrency");
				table.Columns.Add("AgainstAccountId");
				table.Columns.Add("SubsidiaryAccountId");
				table.Columns.Add("SubsidiaryAccount");
				table.Columns.Add("SubsidiaryAccountTypeId");
				table.Columns.Add("Remarks");
				table.Columns.Add("JobLotId");
				table.Columns.Add("JobLot");
				table.Columns.Add("CheqNo");
				table.Columns.Add("TcyCodeId", typeof(int));
				table.Columns.Add("TcyCode");
				table.Columns.Add("TcyExchangeRate", typeof(double));
				table.Columns.Add("FcyAmount", typeof(double));
				table.Columns.Add("AmountDr", typeof(double));
				table.Columns.Add("AmountCr", typeof(double));
				table.Columns.Add("LineId");
				table.Columns.Add("BranchId", typeof(int));
				table.Columns.Add("BranchName");
				table.Columns.Add("CostCenterId", typeof(int));
				table.Columns.Add("CostCenter");
				table.Columns.Add("SortIndex", typeof(int));
			}
			for (int i = 0; i < voucherHead.voucherDetailList.Count; i++)
			{
				cmbLocationType.Value = ((voucherHead.voucherDetailList.Count > 0) ? Conversion.ToInt((object)voucherHead.voucherDetailList[0].LocationTypeId) : 0);
				table.Rows.Add(voucherHead.voucherDetailList[i].AccountCode, voucherHead.voucherDetailList[i].AccountId, voucherHead.voucherDetailList[i].AccountTitle, voucherHead.voucherDetailList[i].ThirdCurrencyId, voucherHead.voucherDetailList[i].GlcyCode, voucherHead.voucherDetailList[i].AgainstAccountId, voucherHead.voucherDetailList[i].SubsidiaryAccountId, voucherHead.voucherDetailList[i].SubsidiaryAccountTitle, voucherHead.voucherDetailList[i].SubsidiaryTypeId, voucherHead.voucherDetailList[i].Comments, voucherHead.voucherDetailList[i].JobLotId, voucherHead.voucherDetailList[i].JobLotDescription, (!SaveAs) ? voucherHead.voucherDetailList[i].CheqNoDetail : "", voucherHead.voucherDetailList[i].DMultiCurrencyId, voucherHead.voucherDetailList[i].TcyCode, voucherHead.voucherDetailList[i].DExchangeCurrencyRate, voucherHead.voucherDetailList[i].DCurrencyAmount, voucherHead.voucherDetailList[i].DebitAmount, voucherHead.voucherDetailList[i].CreditAmount, voucherHead.voucherDetailList[i].LineId, BranchFeature ? voucherHead.voucherDetailList[i].BranchesId : 0, BranchFeature ? voucherHead.voucherDetailList[i].BranchName : "", voucherHead.voucherDetailList[i].CostCenterId, voucherHead.voucherDetailList[i].CostCenterName, (i % 2 == 0) ? (i + 2) : i);
			}
			table.DefaultView.Sort = "SortIndex ASC";
			grd.DataSource = table;
			grd.Refetch();
			grd.RetrieveStructure();
			gridsetting();
			if (MultiCurrencyGridWorking && voucherHead.voucherDetailList != null && voucherHead.voucherDetailList.Count > 0)
			{
				txtBaseFcyCode.Tag = voucherHead.voucherDetailList[0].BaseFcyId;
				txtBaseFcyCode.Text = Conversion.ToString((object)voucherHead.voucherDetailList[0].BaseFcyCode);
				txtBaseFcyExchangeRate.Text = Conversion.ToString((object)voucherHead.voucherDetailList[0].BaseFcyExchangeRate);
			}
			dtVoucherDetailMultiCurrency.Rows.Clear();
			foreach (VoucherDetailMultiCurrency item in voucherHead.VoucherDetailMultiCurrencyList)
			{
				dtVoucherDetailMultiCurrency.Rows.Add(item.Id, item.LocalCurrencyId, item.LcyCurrencyCode, "To", item.GlCurrencyId, item.GlFcyCurrencyCode, item.ExchangeRate);
			}
			grdMultiCurrency.DataSource = dtVoucherDetailMultiCurrency;
			grdMultiCurrency.RetrieveStructure();
			GrdMultiCurrencySetting();
			if (!SaveAs)
			{
				FormHelper.LoadAttachmentsForObject(AT, RecId, null, 5);
			}
			Save.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
			txtExchangeRate_TextChanged(null, null);
			DocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill(int NoofRecords = 0)
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
			vh.Ids = "5";
			vh.PostState = formright.DoHaveCanViewAllRecordRights;
			if (!vh.PostState)
			{
				vh.EntryUser = UserAccount.ID;
			}
			if (rdentdoc.Checked)
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
			else if (rdententry.Checked)
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
			else if (rdentmodify.Checked)
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
			else if (rdentapproved.Checked)
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
					dtHistory.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["ManualBillNo"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["AgainstAccount"], lst.Rows[i]["Remarks"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["ModifyUserName"], lst.Rows[i]["ModifyDate"], lst.Rows[i]["ApprovedUserName"], lst.Rows[i]["PostDate"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["NoOfAttachments"]);
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
				DataGridHistory.ClearStructure();
				grdDetail.ClearStructure();
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
			DataGridHistory.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			DataGridHistory.RootTable.Columns["VoucherCode"].Caption = "V.No";
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Caption = "V.Type";
			DataGridHistory.RootTable.Columns["FcyAmount"].Caption = "Tcy Amount";
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["VoucherCode"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
			DataGridHistory.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			DataGridHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			DataGridHistory.RootTable.Columns["AgainstAccount"].Width = Constants.InventoryConstants.AccountTitle;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 100;
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
			DataGridHistory.RootTable.Columns["CheqNo"].Width = Constants.InventoryConstants.StringConstant;
			DataGridHistory.RootTable.Columns["Attachment"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 20;
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
			DataGridHistory.RootTable.Columns["Edit"].Width = 35;
			DataGridHistory.RootTable.Columns.Add("AddAttachment");
			DataGridHistory.RootTable.Columns["AddAttachment"].Caption = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["AddAttachment"].ButtonText = "Add Attachments";
			DataGridHistory.RootTable.Columns["AddAttachment"].Width = 110;
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["VoucherCode"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["VoucherDate"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["EntryUser"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["VoucherAmount"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Attachment"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns["Edit"].Position = 1;
			GridEX_Helper.AddButton(DataGridHistory, "SaveAs", "Save As", 60, 2);
			DataGridHistory.RootTable.Columns["DocumentTypeCode"].Position = 3;
			DataGridHistory.FrozenColumns = 4;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
			DataGridHistory.RootTable.Columns["AccountTitle"].Visible = false;
			DataGridHistory.RootTable.Columns["CheqNo"].Visible = false;
			DataGridHistory.RootTable.Columns["Remarks"].Visible = false;
			DataGridHistory.RootTable.Columns["AgainstAccount"].Visible = false;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Visible = false;
			DataGridHistory.RootTable.Columns["FcyCode"].Visible = false;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridDetailsetting()
	{
		try
		{
			grdDetail.RootTable.Columns["SubsidiaryAccount"].Visible = SubsidiaryAccountAllownOnVouchers;
			grdDetail.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grdDetail.RootTable.Columns["CostCenter"].Visible = IsBookingOffice;
			grdDetail.RootTable.Columns["GlCurrency"].Visible = MultiCurrencyGridWorking;
			grdDetail.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["AccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["AccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["AccountId"].Caption = "Account Title";
			grdDetail.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtDrAc.DefaultView, "Id", "AccountTitle");
			grdDetail.RootTable.Columns["AgainstAccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["AgainstAccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["AgainstAccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["AgainstAccountId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["AgainstAccountId"].Caption = "Offset Account";
			grdDetail.RootTable.Columns["AgainstAccountId"].ValueList.PopulateValueList((IEnumerable)dtDrAc.DefaultView, "Id", "AccountTitle");
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3);
			grdDetail.RootTable.Columns["AccountId"].Width = Constants.InventoryConstants.AccountTitle + 15;
			grdDetail.RootTable.Columns["AgainstAccountId"].Width = Constants.InventoryConstants.AccountTitle + 15;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 100;
			grdDetail.RootTable.Columns["TcyCode"].Width = Constants.InventoryConstants.FcyConstant + 10;
			grdDetail.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["CheqNo"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["AmountDr"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["AmountCr"].FilterEditType = (FilterEditType)1;
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
			grdDetail.RootTable.Columns["SubsidiaryAccount"].Visible = false;
			grdDetail.RootTable.Columns["AmountDr"].Caption = "Debit";
			grdDetail.RootTable.Columns["AmountCr"].Caption = "Credit";
			grdDetail.RootTable.Columns["DebitFcyAmount"].Caption = "Tcy Debit";
			grdDetail.RootTable.Columns["CreditFcyAmount"].Caption = "Tcy Credit";
			ctrlGrdBar3_Load(null, null);
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

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick_1(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Print")
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 5);
				}
				else if (e.Column.Key == "Edit")
				{
					RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
					ReadById(RecId);
				}
				if (e.Column.Key == "SaveAs")
				{
					DataGridHistory_SaveAs(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				else if (e.Column.Key == "AddAttachment")
				{
					CommonServices.GetSaveAndDeleteAttachmentsForRecord(UserAccount, Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 5, base.Name, ShowSavebutton: true);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SaveAs(int CurrId)
	{
		try
		{
			Reset();
			ReadById(CurrId, SaveAs: true);
			txtCheq.Text = string.Empty;
			Save.Visible = false;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = true;
			DocDate.Value = DateTime.Now;
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 5);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		HistoryGridFill();
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
			MessageBox.Show(ex.Message);
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

	private void DayBookVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
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
				btnshow_Click(null, null);
			}
		}
		if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				New_Click(null, null);
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
				DocDate.Focus();
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
		if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
		{
			Print_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.R && tabControl1.SelectedIndex == 0)
		{
			btnRefresh_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F10)
		{
			btnAttachment_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				DocDate.Focus();
			}
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grd).Focus();
			}
			if (tabControl1.SelectedIndex == 1)
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

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.AcRptPaymentReceiptsVoucherSlip_102(RecId, 5);
				return;
			}
			throw new Exception("No Data Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtCrRmks_Leave(object sender, EventArgs e)
	{
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

	private void txtAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text) == 0.0 && base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "DebitAmount")
			{
				txtFcyAmountDetail.Text = "0";
				txtAmount.Text = "0";
				throw new Exception("Please enter exchange Rate First");
			}
			CommonServices.UpdateRelatedFieldsCalculation(txtAmount, txtFcyAmountDetail, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
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
			if (((UltraGridBase)CmbCreditAccount).ActiveRow != null && ((Control)(object)CmbCreditAccount).Text != string.Empty)
			{
				if (dtSubsidory.Rows.Count > 0)
				{
					if (((UltraGridBase)CmbCreditAccount).ActiveRow != null && ((Control)(object)CmbCreditAccount).Text != string.Empty)
					{
						int selectedAcId = Conversion.ToInt(CmbCreditAccount.Value);
						if (selectedAcId != 0)
						{
							DataTable matchingTable = dtSubsidory.Clone();
							foreach (DataRow row2 in dtSubsidory.Rows)
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
								SubsdiaryAccountCrDatasourceLength = matchingTable.Rows.Count;
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
								SubsdiaryAccountCrDatasourceLength = 0;
							}
						}
						else
						{
							((UltraGridBase)CmbCrSubsidiaryAccount).DataSource = null;
							((Control)(object)CmbCrSubsidiaryAccount).Text = string.Empty;
							SubsdiaryAccountCrDatasourceLength = 0;
						}
					}
					else
					{
						SubsdiaryAccountCrDatasourceLength = dtSubsidory.Rows.Count;
						DDL.BindDDL(dtSubsidory, CmbCrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
						((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
						((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					}
				}
				else
				{
					((UltraGridBase)CmbCrSubsidiaryAccount).DataSource = null;
					((Control)(object)CmbCrSubsidiaryAccount).Text = string.Empty;
					SubsdiaryAccountCrDatasourceLength = 0;
				}
			}
			else
			{
				((UltraGridBase)CmbCrSubsidiaryAccount).DataSource = null;
				((Control)(object)CmbCrSubsidiaryAccount).Text = string.Empty;
				SubsdiaryAccountCrDatasourceLength = 0;
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
			if (((UltraGridBase)CmbDebitAccount).ActiveRow != null && ((Control)(object)CmbDebitAccount).Text != string.Empty)
			{
				if (dtSubsidory.Rows.Count > 0)
				{
					if (((UltraGridBase)CmbDebitAccount).ActiveRow != null && ((Control)(object)CmbDebitAccount).Text != string.Empty)
					{
						int selectedAcId = Conversion.ToInt(CmbDebitAccount.Value);
						if (selectedAcId != 0)
						{
							DataTable matchingTable = dtSubsidory.Clone();
							foreach (DataRow row2 in dtSubsidory.Rows)
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
								SubsdiaryAccountDrDatasourceLength = matchingTable.Rows.Count;
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
								SubsdiaryAccountDrDatasourceLength = 0;
							}
						}
						else
						{
							((UltraGridBase)CmbDrSubsidiaryAccount).DataSource = null;
							((Control)(object)CmbDrSubsidiaryAccount).Text = string.Empty;
							SubsdiaryAccountDrDatasourceLength = 0;
						}
					}
					else
					{
						SubsdiaryAccountDrDatasourceLength = dtSubsidory.Rows.Count;
						DDL.BindDDL(dtSubsidory, CmbDrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
						((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[3].Hidden = true;
						((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					}
				}
				else
				{
					((UltraGridBase)CmbDrSubsidiaryAccount).DataSource = null;
					((Control)(object)CmbDrSubsidiaryAccount).Text = string.Empty;
					SubsdiaryAccountDrDatasourceLength = 0;
				}
			}
			else
			{
				((Control)(object)CmbDrSubsidiaryAccount).Text = string.Empty;
				((UltraGridBase)CmbDrSubsidiaryAccount).DataSource = null;
				SubsdiaryAccountDrDatasourceLength = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				VoucherHead byID = VoucherHead.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
				DataTable table = new DataTable();
				table.Columns.Add("AccountCode");
				table.Columns.Add("AccountId");
				table.Columns.Add("AgainstAccountId");
				table.Columns.Add("SubsidiaryAccount");
				table.Columns.Add("GlCurrency");
				table.Columns.Add("JobLot");
				table.Columns.Add("Remarks");
				table.Columns.Add("CheqNo");
				table.Columns.Add("TcyCode", typeof(string));
				table.Columns.Add("TcyExchangeRate", typeof(double));
				table.Columns.Add("DebitFcyAmount", typeof(double));
				table.Columns.Add("AmountDr", typeof(double));
				table.Columns.Add("CreditFcyAmount", typeof(double));
				table.Columns.Add("AmountCr", typeof(double));
				table.Columns.Add("BranchName", typeof(string));
				table.Columns.Add("CostCenter", typeof(string));
				table.Rows.Clear();
				List<VoucherDetail> sortingVoucherDetailList = byID.voucherDetailList.OrderBy((VoucherDetail x) => x.SubNo).ToList();
				for (int i = 0; i < sortingVoucherDetailList.Count; i++)
				{
					table.Rows.Add(sortingVoucherDetailList[i].AccountCode, sortingVoucherDetailList[i].AccountId, sortingVoucherDetailList[i].AgainstAccountId, sortingVoucherDetailList[i].SubsidiaryAccountTitle, sortingVoucherDetailList[i].GlcyCode, sortingVoucherDetailList[i].JobLotDescription, sortingVoucherDetailList[i].Comments, sortingVoucherDetailList[i].CheqNoDetail, sortingVoucherDetailList[i].TcyCode, sortingVoucherDetailList[i].DExchangeCurrencyRate, (sortingVoucherDetailList[i].DebitAmount > 0.0) ? sortingVoucherDetailList[i].DCurrencyAmount : 0.0, sortingVoucherDetailList[i].DebitAmount, (sortingVoucherDetailList[i].CreditAmount > 0.0) ? sortingVoucherDetailList[i].DCurrencyAmount : 0.0, sortingVoucherDetailList[i].CreditAmount, sortingVoucherDetailList[i].BranchName, sortingVoucherDetailList[i].CostCenterName);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				gridDetailsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindAllSubsidiaryaccounts()
	{
		try
		{
			dtSubsidory = VoucherHead.BindSubsidiaryAccount(UserAccount.CompanyId, 0, (string)null);
			if (dtSubsidory.Rows.Count > 0)
			{
				DDL.BindDDL(dtSubsidory, CmbCrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
				((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				DDL.BindDDL(dtSubsidory, CmbDrSubsidiaryAccount, "Id", "SubsidiaryAccount", "Account Title", ZeroIndex: false);
				((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbDrSubsidiaryAccount).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbCrSubsidiaryAccount_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbCreditAccount.Value) > 0 && Conversion.ToInt(CmbCrSubsidiaryAccount.Value) > 0)
			{
				labelCrSubsidiary.Visible = true;
				labelSubsidiaryCreditBal.Visible = true;
				labelSubsidiaryCreditBal.Text = "0";
				double ClosingBalance = CommonServices.GetSubsiadiaryBalance(Conversion.ToInt(CmbCreditAccount.Value), Conversion.ToInt(CmbCrSubsidiaryAccount.Value), DocDate.Value);
				labelSubsidiaryCreditBal.Text = ClosingBalance.ToString("#,#;(#,#);0");
				if (ClosingBalance > 0.0)
				{
					labelCrSubsidiary.Text = "Dr";
				}
				else
				{
					labelCrSubsidiary.Text = "Cr";
				}
				if (ClosingBalance == 0.0)
				{
					labelCrSubsidiary.Text = "Nill";
				}
			}
			else
			{
				labelCrSubsidiary.Visible = false;
				labelSubsidiaryCreditBal.Visible = false;
				labelSubsidiaryCreditBal.Text = "0";
			}
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
			if (!AfterCloseupcrSubsidiary && ((UltraGridBase)CmbCrSubsidiaryAccount).ActiveRow != null && updateRowIndex <= -1)
			{
				CmbCreditAccount.Value = Conversion.ToInt(((UltraDropDownBase)CmbCrSubsidiaryAccount).SelectedRow.Cells[4].Value);
				CmbCr_Leave(null, null);
			}
			AfterCloseupcrSubsidiary = false;
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
			if (Conversion.ToInt(CmbDebitAccount.Value) > 0 && Conversion.ToInt(CmbDrSubsidiaryAccount.Value) > 0)
			{
				labelDrSubsidiary.Visible = true;
				labelSubsidiaryDebitBal.Visible = true;
				labelSubsidiaryDebitBal.Text = "0";
				double ClosingBalance = CommonServices.GetSubsiadiaryBalance(Conversion.ToInt(CmbDebitAccount.Value), Conversion.ToInt(CmbDrSubsidiaryAccount.Value), DocDate.Value);
				labelSubsidiaryDebitBal.Text = ClosingBalance.ToString("#,#;(#,#);0");
				if (ClosingBalance > 0.0)
				{
					labelDrSubsidiary.Text = "Dr";
				}
				else
				{
					labelDrSubsidiary.Text = "Cr";
				}
				if (ClosingBalance == 0.0)
				{
					labelDrSubsidiary.Text = "Nill";
				}
			}
			else
			{
				labelDrSubsidiary.Visible = false;
				labelSubsidiaryDebitBal.Visible = false;
				labelSubsidiaryDebitBal.Text = "0";
			}
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
			if (!AfterCloseupDrSubsidiary && ((UltraGridBase)CmbDrSubsidiaryAccount).ActiveRow != null && updateRowIndex <= -1)
			{
				CmbDebitAccount.Value = Conversion.ToInt(((UltraDropDownBase)CmbDrSubsidiaryAccount).SelectedRow.Cells[4].Value);
				CmbDr_Leave(null, null);
			}
			AfterCloseupDrSubsidiary = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void JournalVoucher_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtDrRmks_Leave(object sender, EventArgs e)
	{
		try
		{
			if (txtCrRmks.Text == string.Empty && txtDrRmks.Text != string.Empty)
			{
				txtCrRmks.Text = txtDrRmks.Text.Trim();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
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

	private void btnplus_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "grd")
			{
				CmbDebitAccount.Focus();
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
				CmbDebitAccount.Focus();
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
				int UniqueId = Conversion.ToInt(item.Cells["LineId"].Value);
				if (UniqueId % 2 == 0)
				{
					for (int i = 0; i < table.Rows.Count; i++)
					{
						if (Conversion.ToInt(table.Rows[i]["LineId"]) == UniqueId - 1)
						{
							table.Rows[i].Delete();
							break;
						}
					}
					item.Delete();
				}
				else
				{
					for (int j = 0; j < table.Rows.Count; j++)
					{
						if (Conversion.ToInt(table.Rows[j]["LineId"]) == UniqueId + 1)
						{
							table.Rows[j].Delete();
							break;
						}
					}
					item.Delete();
				}
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(id);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.AcRptPaymentReceiptsVoucherSlip_102(id, 5);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(id);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Attachment")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, 5);
				}
				else if (DataGridHistory.CurrentColumn.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, id, 5, base.Name);
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
			GridEXRow[] rows = grd.GetRows();
			if (rows == null || rows.Length == 0)
			{
				txtFcyAmount.Text = "0";
				return;
			}
			double totalFcyAmount = rows.Where((GridEXRow r) => Conversion.ToDouble(r.Cells["AmountDr"].Value) > 0.0).Sum((GridEXRow r) => Conversion.ToDouble(r.Cells["FcyAmount"].Value));
			txtFcyAmount.Text = totalFcyAmount.ToString("#,##0.###");
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
					DocumentTypeIds = "5",
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

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
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

	private void btnshow_Click(object sender, EventArgs e)
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

	private void CmbDrSubsidiaryAccount_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbDrSubsidiaryAccount).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloseupDrSubsidiary = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbDebitAccount.Value = GLOfSubSidory;
					CmbDr_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCrSubsidiaryAccount_AfterCloseUp(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbCrSubsidiaryAccount).SelectedRow;
			if (selectedRow != null)
			{
				AfterCloseupcrSubsidiary = true;
				int GLOfSubSidory = Conversion.ToInt(selectedRow.Cells[4].Value);
				if (GLOfSubSidory != 0)
				{
					CmbCreditAccount.Value = GLOfSubSidory;
					CmbCr_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
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

	private void CmbDrBranch_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbCrBranch).Text == null || ((Control)(object)CmbCrBranch).Text == string.Empty)
			{
				((Control)(object)CmbCrBranch).Text = ((Control)(object)CmbDrBranch).Text.Trim();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDrSubsidiaryAccount_Leave(object sender, EventArgs e)
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

	private void CmbCrSubsidiaryAccount_Leave(object sender, EventArgs e)
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

	private void txtFcyAmountDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text) == 0.0 && base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "FcyAmount")
			{
				txtFcyAmountDetail.Text = "0";
				txtAmount.Text = "0";
				throw new Exception("Please enter exchange Rate First");
			}
			CommonServices.UpdateRelatedFieldsCalculation(txtAmount, txtFcyAmountDetail, Conversion.ToDouble((object)txtTcyExchangeRateDetail.Text), this, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, 1, "", clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyAmountDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void rdSearchByAccountName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtDrAc.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(CmbDebitAccount.Value) > 0)
				{
					Id = Conversion.ToInt(CmbDebitAccount.Value);
				}
				string displayField = (rdSearchByAccountName.Checked ? "AccountTitle" : "AccountCode");
				string headerText = (rdSearchByAccountName.Checked ? "Debit Account" : "Account Code");
				DDL.BindDDL(dtDrAc, CmbDebitAccount, "Id", displayField, headerText, ZeroIndex: true);
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

	private void rdSearchByCrAccountName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtDrAc.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(CmbCreditAccount.Value) > 0)
				{
					Id = Conversion.ToInt(CmbCreditAccount.Value);
				}
				string displayField = (rdSearchByCrAccountName.Checked ? "AccountTitle" : "AccountCode");
				string headerText = (rdSearchByCrAccountName.Checked ? "Credit Account" : "Account Code");
				DDL.BindDDL(dtDrAc, CmbCreditAccount, "Id", displayField, headerText, ZeroIndex: true);
				if (Id > 0)
				{
					CmbCreditAccount.Value = Id;
					CmbCreditAccount.Focus();
				}
			}
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
		txtAmount_TextChanged(null, null);
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
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Expected O, but got Unknown
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Expected O, but got Unknown
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Expected O, but got Unknown
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Expected O, but got Unknown
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Expected O, but got Unknown
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Expected O, but got Unknown
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_082d: Expected O, but got Unknown
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Expected O, but got Unknown
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Expected O, but got Unknown
		//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Expected O, but got Unknown
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0977: Expected O, but got Unknown
		//IL_0a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a27: Expected O, but got Unknown
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5e: Expected O, but got Unknown
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Expected O, but got Unknown
		//IL_1209: Unknown result type (might be due to invalid IL or missing references)
		//IL_1213: Expected O, but got Unknown
		//IL_8bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_8bc8: Expected O, but got Unknown
		//IL_8bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_8bf6: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
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
		Appearance appearance112 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
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
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance105 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.JournalVoucher));
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.DebitAccountPanel = new System.Windows.Forms.Panel();
		this.txtDebit = new System.Windows.Forms.TextBox();
		this.rdSearchByAccountCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByAccountName = new System.Windows.Forms.RadioButton();
		this.CmbDebitAccount = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.DebitSubsidiaryPanel = new System.Windows.Forms.Panel();
		this.CmbDrSubsidiaryAccount = new UltraCombo();
		this.labelDrSubsidiary = new System.Windows.Forms.Label();
		this.labelSubsidiaryDebitBal = new System.Windows.Forms.Label();
		this.lblDrSubsidiaryaccount = new System.Windows.Forms.Label();
		this.DrCommentsPanel = new System.Windows.Forms.Panel();
		this.txtDrRmks = new System.Windows.Forms.TextBox();
		this.lblDrComments = new System.Windows.Forms.Label();
		this.DrJobLotPanel = new System.Windows.Forms.Panel();
		this.CmbDrJobLot = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.DrBranchPanel = new System.Windows.Forms.Panel();
		this.CmbDrBranch = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.DrCostCenterPanel = new System.Windows.Forms.Panel();
		this.CmbDrCostCenter = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.txtCheq = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.panel26 = new System.Windows.Forms.Panel();
		this.cmbTcyCodeDetail = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.txtTcyExchangeRateDetail = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.txtFcyAmountDetail = new System.Windows.Forms.TextBox();
		this.panelChqRefAndAmount = new System.Windows.Forms.Panel();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.CreditAmountPanel = new System.Windows.Forms.Panel();
		this.txtCredit = new System.Windows.Forms.TextBox();
		this.rdSearchByCrAccountCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByCrAccountName = new System.Windows.Forms.RadioButton();
		this.CmbCreditAccount = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.CrSubsidiaryPanel = new System.Windows.Forms.Panel();
		this.labelSubsidiaryCreditBal = new System.Windows.Forms.Label();
		this.CmbCrSubsidiaryAccount = new UltraCombo();
		this.lblCrSubsidiaryaccount = new System.Windows.Forms.Label();
		this.labelCrSubsidiary = new System.Windows.Forms.Label();
		this.CrCommentsPanel = new System.Windows.Forms.Panel();
		this.txtCrRmks = new System.Windows.Forms.TextBox();
		this.lblCrComments = new System.Windows.Forms.Label();
		this.CrJobLotPanel = new System.Windows.Forms.Panel();
		this.CmbCrJobLot = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CrBranchPanel = new System.Windows.Forms.Panel();
		this.CmbCrBranch = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.CrCostCenterPanel = new System.Windows.Forms.Panel();
		this.CmbCrCostCenter = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.panelDetailButtons = new System.Windows.Forms.Panel();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdMultiCurrency = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtBaseFcyCode = new System.Windows.Forms.TextBox();
		this.lblBaseFcyCode = new System.Windows.Forms.Label();
		this.txtBaseFcyExchangeRate = new System.Windows.Forms.TextBox();
		this.lblBaseFcyExchangeRate = new System.Windows.Forms.Label();
		this.CmbProjectId = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtVoucherremarks = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.chkCustomAccounts = new System.Windows.Forms.CheckBox();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBoxPrintPreview = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnCreateUpdateMultiCurrency = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.VoucherInfoBox = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.lblUnApprovedVouchers = new System.Windows.Forms.Label();
		this.lblTotalVouchers = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.lblApprovedVouchers = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdentapproved = new System.Windows.Forms.RadioButton();
		this.rdentmodify = new System.Windows.Forms.RadioButton();
		this.rdentdoc = new System.Windows.Forms.RadioButton();
		this.rdententry = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbApproveStatus = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel12.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel14.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.DebitAccountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).BeginInit();
		this.DebitSubsidiaryPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrSubsidiaryAccount).BeginInit();
		this.DrCommentsPanel.SuspendLayout();
		this.DrJobLotPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrJobLot).BeginInit();
		this.DrBranchPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrBranch).BeginInit();
		this.DrCostCenterPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrCostCenter).BeginInit();
		this.panel15.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTcyCodeDetail).BeginInit();
		this.panel25.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panelChqRefAndAmount.SuspendLayout();
		this.CreditAmountPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		this.CrSubsidiaryPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrSubsidiaryAccount).BeginInit();
		this.CrCommentsPanel.SuspendLayout();
		this.CrJobLotPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrJobLot).BeginInit();
		this.CrBranchPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrBranch).BeginInit();
		this.CrCostCenterPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrCostCenter).BeginInit();
		this.panelDetailButtons.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel10.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.VoucherInfoBox.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1387, 689);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1387, 689);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1379, 663);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 297);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1375, 364);
		this.panel5.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1375, 334);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Controls.Add(this.label4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1375, 30);
		this.panel12.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1334, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(9, 6);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 17);
		this.label4.TabIndex = 1;
		this.label4.Text = "Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel14);
		this.panel4.Controls.Add(this.panel13);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1375, 240);
		this.panel4.TabIndex = 0;
		this.panel14.Controls.Add(this.groupBox2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel14.Location = new System.Drawing.Point(0, 115);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1351, 125);
		this.panel14.TabIndex = 3;
		this.groupBox2.Controls.Add(this.flowLayoutPanel1);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1345, 118);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.flowLayoutPanel1.Controls.Add(this.DebitAccountPanel);
		this.flowLayoutPanel1.Controls.Add(this.DebitSubsidiaryPanel);
		this.flowLayoutPanel1.Controls.Add(this.DrCommentsPanel);
		this.flowLayoutPanel1.Controls.Add(this.DrJobLotPanel);
		this.flowLayoutPanel1.Controls.Add(this.DrBranchPanel);
		this.flowLayoutPanel1.Controls.Add(this.DrCostCenterPanel);
		this.flowLayoutPanel1.Controls.Add(this.panel15);
		this.flowLayoutPanel1.Controls.Add(this.panel26);
		this.flowLayoutPanel1.Controls.Add(this.panel25);
		this.flowLayoutPanel1.Controls.Add(this.panel16);
		this.flowLayoutPanel1.Controls.Add(this.panelChqRefAndAmount);
		this.flowLayoutPanel1.Controls.Add(this.CreditAmountPanel);
		this.flowLayoutPanel1.Controls.Add(this.CrSubsidiaryPanel);
		this.flowLayoutPanel1.Controls.Add(this.CrCommentsPanel);
		this.flowLayoutPanel1.Controls.Add(this.CrJobLotPanel);
		this.flowLayoutPanel1.Controls.Add(this.CrBranchPanel);
		this.flowLayoutPanel1.Controls.Add(this.CrCostCenterPanel);
		this.flowLayoutPanel1.Controls.Add(this.panelDetailButtons);
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 19);
		this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(1336, 96);
		this.flowLayoutPanel1.TabIndex = 0;
		this.DebitAccountPanel.Controls.Add(this.txtDebit);
		this.DebitAccountPanel.Controls.Add(this.rdSearchByAccountCode);
		this.DebitAccountPanel.Controls.Add(this.rdSearchByAccountName);
		this.DebitAccountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccount);
		this.DebitAccountPanel.Controls.Add(this.label6);
		this.DebitAccountPanel.Location = new System.Drawing.Point(0, 0);
		this.DebitAccountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitAccountPanel.Name = "DebitAccountPanel";
		this.DebitAccountPanel.Size = new System.Drawing.Size(238, 47);
		this.DebitAccountPanel.TabIndex = 0;
		this.txtDebit.BackColor = System.Drawing.Color.White;
		this.txtDebit.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtDebit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDebit.Location = new System.Drawing.Point(143, 3);
		this.txtDebit.Name = "txtDebit";
		this.txtDebit.ReadOnly = true;
		this.txtDebit.Size = new System.Drawing.Size(92, 16);
		this.txtDebit.TabIndex = 36;
		this.txtDebit.TabStop = false;
		this.txtDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.rdSearchByAccountCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rdSearchByAccountCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByAccountCode.Location = new System.Drawing.Point(96, 1);
		this.rdSearchByAccountCode.Margin = new System.Windows.Forms.Padding(0);
		this.rdSearchByAccountCode.Name = "rdSearchByAccountCode";
		this.rdSearchByAccountCode.Size = new System.Drawing.Size(50, 20);
		this.rdSearchByAccountCode.TabIndex = 143;
		this.rdSearchByAccountCode.Text = "Code";
		this.rdSearchByAccountCode.UseVisualStyleBackColor = true;
		this.rdSearchByAccountName.Checked = true;
		this.rdSearchByAccountName.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rdSearchByAccountName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByAccountName.Location = new System.Drawing.Point(56, 1);
		this.rdSearchByAccountName.Margin = new System.Windows.Forms.Padding(0);
		this.rdSearchByAccountName.Name = "rdSearchByAccountName";
		this.rdSearchByAccountName.Size = new System.Drawing.Size(40, 20);
		this.rdSearchByAccountName.TabIndex = 142;
		this.rdSearchByAccountName.TabStop = true;
		this.rdSearchByAccountName.Text = "Title";
		this.rdSearchByAccountName.UseVisualStyleBackColor = true;
		this.rdSearchByAccountName.CheckedChanged += new System.EventHandler(rdSearchByAccountName_CheckedChanged);
		this.CmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Location = new System.Drawing.Point(2, 21);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Name = "CmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Leave += new System.EventHandler(CmbDr_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 4);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(53, 15);
		this.label6.TabIndex = 24;
		this.label6.Text = "Debit Ac";
		this.DebitSubsidiaryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount);
		this.DebitSubsidiaryPanel.Controls.Add(this.labelDrSubsidiary);
		this.DebitSubsidiaryPanel.Controls.Add(this.labelSubsidiaryDebitBal);
		this.DebitSubsidiaryPanel.Controls.Add(this.lblDrSubsidiaryaccount);
		this.DebitSubsidiaryPanel.Location = new System.Drawing.Point(238, 0);
		this.DebitSubsidiaryPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DebitSubsidiaryPanel.Name = "DebitSubsidiaryPanel";
		this.DebitSubsidiaryPanel.Size = new System.Drawing.Size(168, 47);
		this.DebitSubsidiaryPanel.TabIndex = 1;
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
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).Name = "CmbDrSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).TabIndex = 0;
		this.CmbDrSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbDrSubsidiaryAccount_AfterCloseUp);
		this.CmbDrSubsidiaryAccount.ValueChanged += new System.EventHandler(CmbDrSubsidiaryAccount_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbDrSubsidiaryAccount).Leave += new System.EventHandler(CmbDrSubsidiaryAccount_Leave);
		this.labelDrSubsidiary.AutoSize = true;
		this.labelDrSubsidiary.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold);
		this.labelDrSubsidiary.Location = new System.Drawing.Point(75, 4);
		this.labelDrSubsidiary.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.labelDrSubsidiary.Name = "labelDrSubsidiary";
		this.labelDrSubsidiary.Size = new System.Drawing.Size(17, 13);
		this.labelDrSubsidiary.TabIndex = 1;
		this.labelDrSubsidiary.Text = "Bl";
		this.labelSubsidiaryDebitBal.AutoSize = true;
		this.labelSubsidiaryDebitBal.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelSubsidiaryDebitBal.Location = new System.Drawing.Point(92, 4);
		this.labelSubsidiaryDebitBal.Name = "labelSubsidiaryDebitBal";
		this.labelSubsidiaryDebitBal.Size = new System.Drawing.Size(47, 13);
		this.labelSubsidiaryDebitBal.TabIndex = 597;
		this.labelSubsidiaryDebitBal.Text = "label8";
		this.labelSubsidiaryDebitBal.Visible = false;
		this.lblDrSubsidiaryaccount.AutoSize = true;
		this.lblDrSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDrSubsidiaryaccount.Location = new System.Drawing.Point(0, 3);
		this.lblDrSubsidiaryaccount.Name = "lblDrSubsidiaryaccount";
		this.lblDrSubsidiaryaccount.Size = new System.Drawing.Size(78, 15);
		this.lblDrSubsidiaryaccount.TabIndex = 46;
		this.lblDrSubsidiaryaccount.Text = "Dr Subsidiary";
		this.DrCommentsPanel.Controls.Add(this.txtDrRmks);
		this.DrCommentsPanel.Controls.Add(this.lblDrComments);
		this.DrCommentsPanel.Location = new System.Drawing.Point(406, 0);
		this.DrCommentsPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrCommentsPanel.Name = "DrCommentsPanel";
		this.DrCommentsPanel.Size = new System.Drawing.Size(185, 47);
		this.DrCommentsPanel.TabIndex = 2;
		this.txtDrRmks.BackColor = System.Drawing.Color.White;
		this.txtDrRmks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDrRmks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDrRmks.Location = new System.Drawing.Point(0, 22);
		this.txtDrRmks.Name = "txtDrRmks";
		this.txtDrRmks.Size = new System.Drawing.Size(185, 23);
		this.txtDrRmks.TabIndex = 0;
		this.txtDrRmks.Leave += new System.EventHandler(txtDrRmks_Leave);
		this.lblDrComments.AutoSize = true;
		this.lblDrComments.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDrComments.Location = new System.Drawing.Point(1, 4);
		this.lblDrComments.Name = "lblDrComments";
		this.lblDrComments.Size = new System.Drawing.Size(81, 15);
		this.lblDrComments.TabIndex = 34;
		this.lblDrComments.Text = "Dr Comments";
		this.DrJobLotPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDrJobLot);
		this.DrJobLotPanel.Controls.Add(this.label7);
		this.DrJobLotPanel.Location = new System.Drawing.Point(591, 0);
		this.DrJobLotPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrJobLotPanel.Name = "DrJobLotPanel";
		this.DrJobLotPanel.Size = new System.Drawing.Size(135, 47);
		this.DrJobLotPanel.TabIndex = 3;
		this.CmbDrJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbDrJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDrJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDrJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDrJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDrJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).Name = "CmbDrJobLot";
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbDrJobLot).TabIndex = 0;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(65, 15);
		this.label7.TabIndex = 50;
		this.label7.Text = "Dr Job Lot ";
		this.DrBranchPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDrBranch);
		this.DrBranchPanel.Controls.Add(this.label12);
		this.DrBranchPanel.Location = new System.Drawing.Point(726, 0);
		this.DrBranchPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrBranchPanel.Name = "DrBranchPanel";
		this.DrBranchPanel.Size = new System.Drawing.Size(140, 47);
		this.DrBranchPanel.TabIndex = 4;
		this.CmbDrBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbDrBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDrBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDrBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDrBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDrBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Name = "CmbDrBranch";
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbDrBranch).Leave += new System.EventHandler(CmbDrBranch_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 4);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 52;
		this.label12.Text = "Dr Branch";
		this.DrCostCenterPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDrCostCenter);
		this.DrCostCenterPanel.Controls.Add(this.label20);
		this.DrCostCenterPanel.Location = new System.Drawing.Point(866, 0);
		this.DrCostCenterPanel.Margin = new System.Windows.Forms.Padding(0);
		this.DrCostCenterPanel.Name = "DrCostCenterPanel";
		this.DrCostCenterPanel.Size = new System.Drawing.Size(123, 47);
		this.DrCostCenterPanel.TabIndex = 5;
		this.CmbDrCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbDrCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDrCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDrCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
		((SpecialBoxBase)((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDrCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDrCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).Name = "CmbDrCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.CmbDrCostCenter).TabIndex = 0;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(-1, 4);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(83, 15);
		this.label20.TabIndex = 52;
		this.label20.Text = "Dr Cost Center";
		this.panel15.Controls.Add(this.txtCheq);
		this.panel15.Controls.Add(this.label8);
		this.panel15.Location = new System.Drawing.Point(989, 0);
		this.panel15.Margin = new System.Windows.Forms.Padding(0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(104, 47);
		this.panel15.TabIndex = 6;
		this.txtCheq.BackColor = System.Drawing.Color.White;
		this.txtCheq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCheq.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCheq.Location = new System.Drawing.Point(1, 22);
		this.txtCheq.Name = "txtCheq";
		this.txtCheq.Size = new System.Drawing.Size(103, 23);
		this.txtCheq.TabIndex = 0;
		this.txtCheq.UseWaitCursor = true;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(2, 4);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 33;
		this.label8.Text = "Cheq / Ref";
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail);
		this.panel26.Controls.Add(this.label54);
		this.panel26.Location = new System.Drawing.Point(1093, 0);
		this.panel26.Margin = new System.Windows.Forms.Padding(0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(112, 47);
		this.panel26.TabIndex = 7;
		this.cmbTcyCodeDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbTcyCodeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTcyCodeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTcyCodeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTcyCodeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTcyCodeDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTcyCodeDetail).Location = new System.Drawing.Point(1, 20);
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
		this.panel25.Location = new System.Drawing.Point(1205, 0);
		this.panel25.Margin = new System.Windows.Forms.Padding(0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(108, 47);
		this.panel25.TabIndex = 8;
		this.txtTcyExchangeRateDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTcyExchangeRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTcyExchangeRateDetail.Location = new System.Drawing.Point(0, 22);
		this.txtTcyExchangeRateDetail.Name = "txtTcyExchangeRateDetail";
		this.txtTcyExchangeRateDetail.ShortcutsEnabled = false;
		this.txtTcyExchangeRateDetail.Size = new System.Drawing.Size(106, 23);
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
		this.panel16.Controls.Add(this.label22);
		this.panel16.Controls.Add(this.txtFcyAmountDetail);
		this.panel16.Location = new System.Drawing.Point(0, 47);
		this.panel16.Margin = new System.Windows.Forms.Padding(0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(104, 47);
		this.panel16.TabIndex = 9;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(2, 4);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(72, 15);
		this.label22.TabIndex = 46;
		this.label22.Text = "Tcy Amount";
		this.txtFcyAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmountDetail.Location = new System.Drawing.Point(1, 22);
		this.txtFcyAmountDetail.Name = "txtFcyAmountDetail";
		this.txtFcyAmountDetail.Size = new System.Drawing.Size(103, 23);
		this.txtFcyAmountDetail.TabIndex = 1;
		this.txtFcyAmountDetail.Tag = "FcyAmount";
		this.txtFcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountDetail.TextChanged += new System.EventHandler(txtFcyAmountDetail_TextChanged);
		this.txtFcyAmountDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFcyAmountDetail_KeyPress);
		this.panelChqRefAndAmount.Controls.Add(this.txtAmount);
		this.panelChqRefAndAmount.Controls.Add(this.label9);
		this.panelChqRefAndAmount.Location = new System.Drawing.Point(104, 47);
		this.panelChqRefAndAmount.Margin = new System.Windows.Forms.Padding(0);
		this.panelChqRefAndAmount.Name = "panelChqRefAndAmount";
		this.panelChqRefAndAmount.Size = new System.Drawing.Size(106, 47);
		this.panelChqRefAndAmount.TabIndex = 10;
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(0, 22);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ShortcutsEnabled = false;
		this.txtAmount.Size = new System.Drawing.Size(105, 23);
		this.txtAmount.TabIndex = 2;
		this.txtAmount.Tag = "DebitAmount";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.TextChanged += new System.EventHandler(txtAmount_TextChanged);
		this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmount_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(4, 4);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(51, 15);
		this.label9.TabIndex = 32;
		this.label9.Text = "Amount";
		this.CreditAmountPanel.Controls.Add(this.txtCredit);
		this.CreditAmountPanel.Controls.Add(this.rdSearchByCrAccountCode);
		this.CreditAmountPanel.Controls.Add(this.rdSearchByCrAccountName);
		this.CreditAmountPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.CreditAmountPanel.Controls.Add(this.label11);
		this.CreditAmountPanel.Location = new System.Drawing.Point(210, 47);
		this.CreditAmountPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CreditAmountPanel.Name = "CreditAmountPanel";
		this.CreditAmountPanel.Size = new System.Drawing.Size(238, 47);
		this.CreditAmountPanel.TabIndex = 11;
		this.txtCredit.BackColor = System.Drawing.Color.White;
		this.txtCredit.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtCredit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCredit.Location = new System.Drawing.Point(144, 3);
		this.txtCredit.Name = "txtCredit";
		this.txtCredit.ReadOnly = true;
		this.txtCredit.Size = new System.Drawing.Size(92, 16);
		this.txtCredit.TabIndex = 35;
		this.txtCredit.TabStop = false;
		this.txtCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.rdSearchByCrAccountCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rdSearchByCrAccountCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCrAccountCode.Location = new System.Drawing.Point(96, 4);
		this.rdSearchByCrAccountCode.Margin = new System.Windows.Forms.Padding(0);
		this.rdSearchByCrAccountCode.Name = "rdSearchByCrAccountCode";
		this.rdSearchByCrAccountCode.Size = new System.Drawing.Size(50, 15);
		this.rdSearchByCrAccountCode.TabIndex = 145;
		this.rdSearchByCrAccountCode.Text = "Code";
		this.rdSearchByCrAccountCode.UseVisualStyleBackColor = true;
		this.rdSearchByCrAccountName.Checked = true;
		this.rdSearchByCrAccountName.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rdSearchByCrAccountName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCrAccountName.Location = new System.Drawing.Point(56, 1);
		this.rdSearchByCrAccountName.Margin = new System.Windows.Forms.Padding(0);
		this.rdSearchByCrAccountName.Name = "rdSearchByCrAccountName";
		this.rdSearchByCrAccountName.Size = new System.Drawing.Size(40, 20);
		this.rdSearchByCrAccountName.TabIndex = 144;
		this.rdSearchByCrAccountName.TabStop = true;
		this.rdSearchByCrAccountName.Text = "Title";
		this.rdSearchByCrAccountName.UseVisualStyleBackColor = true;
		this.rdSearchByCrAccountName.CheckedChanged += new System.EventHandler(rdSearchByCrAccountName_CheckedChanged);
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Leave += new System.EventHandler(CmbCr_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(4, 4);
		this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(55, 15);
		this.label11.TabIndex = 23;
		this.label11.Text = "Credit Ac";
		this.CrSubsidiaryPanel.Controls.Add(this.labelSubsidiaryCreditBal);
		this.CrSubsidiaryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount);
		this.CrSubsidiaryPanel.Controls.Add(this.lblCrSubsidiaryaccount);
		this.CrSubsidiaryPanel.Controls.Add(this.labelCrSubsidiary);
		this.CrSubsidiaryPanel.Location = new System.Drawing.Point(448, 47);
		this.CrSubsidiaryPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CrSubsidiaryPanel.Name = "CrSubsidiaryPanel";
		this.CrSubsidiaryPanel.Size = new System.Drawing.Size(168, 47);
		this.CrSubsidiaryPanel.TabIndex = 12;
		this.labelSubsidiaryCreditBal.AutoSize = true;
		this.labelSubsidiaryCreditBal.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelSubsidiaryCreditBal.Location = new System.Drawing.Point(92, 5);
		this.labelSubsidiaryCreditBal.Name = "labelSubsidiaryCreditBal";
		this.labelSubsidiaryCreditBal.Size = new System.Drawing.Size(47, 13);
		this.labelSubsidiaryCreditBal.TabIndex = 598;
		this.labelSubsidiaryCreditBal.Text = "label8";
		this.labelSubsidiaryCreditBal.Visible = false;
		this.CmbCrSubsidiaryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCrSubsidiaryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCrSubsidiaryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCrSubsidiaryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCrSubsidiaryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCrSubsidiaryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Name = "CmbCrSubsidiaryAccount";
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).TabIndex = 0;
		this.CmbCrSubsidiaryAccount.AfterCloseUp += new System.EventHandler(CmbCrSubsidiaryAccount_AfterCloseUp);
		this.CmbCrSubsidiaryAccount.ValueChanged += new System.EventHandler(CmbCrSubsidiaryAccount_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbCrSubsidiaryAccount).Leave += new System.EventHandler(CmbCrSubsidiaryAccount_Leave);
		this.lblCrSubsidiaryaccount.AutoSize = true;
		this.lblCrSubsidiaryaccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCrSubsidiaryaccount.Location = new System.Drawing.Point(0, 4);
		this.lblCrSubsidiaryaccount.Name = "lblCrSubsidiaryaccount";
		this.lblCrSubsidiaryaccount.Size = new System.Drawing.Size(76, 15);
		this.lblCrSubsidiaryaccount.TabIndex = 44;
		this.lblCrSubsidiaryaccount.Text = "Cr Subsidiary";
		this.labelCrSubsidiary.AutoSize = true;
		this.labelCrSubsidiary.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold);
		this.labelCrSubsidiary.Location = new System.Drawing.Point(75, 4);
		this.labelCrSubsidiary.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.labelCrSubsidiary.Name = "labelCrSubsidiary";
		this.labelCrSubsidiary.Size = new System.Drawing.Size(17, 13);
		this.labelCrSubsidiary.TabIndex = 37;
		this.labelCrSubsidiary.Text = "Bl";
		this.CrCommentsPanel.Controls.Add(this.txtCrRmks);
		this.CrCommentsPanel.Controls.Add(this.lblCrComments);
		this.CrCommentsPanel.Location = new System.Drawing.Point(616, 47);
		this.CrCommentsPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CrCommentsPanel.Name = "CrCommentsPanel";
		this.CrCommentsPanel.Size = new System.Drawing.Size(185, 47);
		this.CrCommentsPanel.TabIndex = 13;
		this.txtCrRmks.BackColor = System.Drawing.Color.White;
		this.txtCrRmks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCrRmks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCrRmks.Location = new System.Drawing.Point(0, 22);
		this.txtCrRmks.Name = "txtCrRmks";
		this.txtCrRmks.Size = new System.Drawing.Size(185, 23);
		this.txtCrRmks.TabIndex = 0;
		this.txtCrRmks.Leave += new System.EventHandler(txtCrRmks_Leave);
		this.lblCrComments.AutoSize = true;
		this.lblCrComments.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblCrComments.Location = new System.Drawing.Point(1, 2);
		this.lblCrComments.Name = "lblCrComments";
		this.lblCrComments.Size = new System.Drawing.Size(79, 15);
		this.lblCrComments.TabIndex = 31;
		this.lblCrComments.Text = "Cr Comments";
		this.CrJobLotPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCrJobLot);
		this.CrJobLotPanel.Controls.Add(this.label5);
		this.CrJobLotPanel.Location = new System.Drawing.Point(801, 47);
		this.CrJobLotPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CrJobLotPanel.Name = "CrJobLotPanel";
		this.CrJobLotPanel.Size = new System.Drawing.Size(135, 47);
		this.CrJobLotPanel.TabIndex = 14;
		this.CmbCrJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbCrJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCrJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCrJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCrJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCrJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCrJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCrJobLot).Location = new System.Drawing.Point(1, 21);
		((System.Windows.Forms.Control)(object)this.CmbCrJobLot).Name = "CmbCrJobLot";
		((System.Windows.Forms.Control)(object)this.CmbCrJobLot).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbCrJobLot).TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(63, 15);
		this.label5.TabIndex = 48;
		this.label5.Text = "Cr Job Lot ";
		this.CrBranchPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCrBranch);
		this.CrBranchPanel.Controls.Add(this.label19);
		this.CrBranchPanel.Location = new System.Drawing.Point(936, 47);
		this.CrBranchPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CrBranchPanel.Name = "CrBranchPanel";
		this.CrBranchPanel.Size = new System.Drawing.Size(140, 47);
		this.CrBranchPanel.TabIndex = 15;
		this.CmbCrBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbCrBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCrBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCrBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCrBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCrBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCrBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCrBranch).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbCrBranch).Name = "CmbCrBranch";
		((System.Windows.Forms.Control)(object)this.CmbCrBranch).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbCrBranch).TabIndex = 0;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(4, 4);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(58, 15);
		this.label19.TabIndex = 54;
		this.label19.Text = "Cr Branch";
		this.CrCostCenterPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCrCostCenter);
		this.CrCostCenterPanel.Controls.Add(this.label21);
		this.CrCostCenterPanel.Location = new System.Drawing.Point(1076, 47);
		this.CrCostCenterPanel.Margin = new System.Windows.Forms.Padding(0);
		this.CrCostCenterPanel.Name = "CrCostCenterPanel";
		this.CrCostCenterPanel.Size = new System.Drawing.Size(123, 47);
		this.CrCostCenterPanel.TabIndex = 16;
		this.CmbCrCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCrCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCrCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCrCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCrCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCrCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCrCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCrCostCenter).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.CmbCrCostCenter).Name = "CmbCrCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCrCostCenter).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.CmbCrCostCenter).TabIndex = 0;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(-1, 4);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(81, 15);
		this.label21.TabIndex = 52;
		this.label21.Text = "Cr Cost Center";
		this.panelDetailButtons.Controls.Add(this.btnCancelDetail);
		this.panelDetailButtons.Controls.Add(this.btnplus);
		this.panelDetailButtons.Controls.Add(this.btnUpdateDetail);
		this.panelDetailButtons.Location = new System.Drawing.Point(1199, 47);
		this.panelDetailButtons.Margin = new System.Windows.Forms.Padding(0);
		this.panelDetailButtons.Name = "panelDetailButtons";
		this.panelDetailButtons.Size = new System.Drawing.Size(83, 47);
		this.panelDetailButtons.TabIndex = 17;
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(2, -4);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(71, 27);
		this.btnCancelDetail.TabIndex = 2;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(btnCancelDetail_Leave);
		this.btnCancelDetail.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(btnCancelDetail_PreviewKeyDown);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(2, 20);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 25);
		this.btnplus.TabIndex = 1;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(2, 20);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 27);
		this.btnUpdateDetail.TabIndex = 1;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdMultiCurrency);
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1375, 115);
		this.panel13.TabIndex = 2;
		((ScrollableBase)this.grdMultiCurrency).BackColor = System.Drawing.Color.White;
		grdMultiCurrency_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMultiCurrency.DesignTimeLayout = grdMultiCurrency_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Dock = System.Windows.Forms.DockStyle.Left;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMultiCurrency.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Location = new System.Drawing.Point(1011, 0);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Name = "grdMultiCurrency";
		((ScrollableBase)this.grdMultiCurrency).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).Size = new System.Drawing.Size(338, 115);
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabIndex = 60;
		((System.Windows.Forms.Control)(object)this.grdMultiCurrency).TabStop = false;
		this.grdMultiCurrency.TotalRow = (InheritableBoolean)1;
		this.grdMultiCurrency.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMultiCurrency).VisualStyle = (VisualStyle)4;
		this.grdMultiCurrency.SelectionChanged += new System.EventHandler(grdMultiCurrency_SelectionChanged);
		this.groupBox1.Controls.Add(this.txtBaseFcyCode);
		this.groupBox1.Controls.Add(this.lblBaseFcyCode);
		this.groupBox1.Controls.Add(this.txtBaseFcyExchangeRate);
		this.groupBox1.Controls.Add(this.lblBaseFcyExchangeRate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProjectId);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.lblFcyAmount);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.lblExchangeRate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.lblFcyCode);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.txtVoucherremarks);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1011, 115);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtBaseFcyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyCode.Enabled = false;
		this.txtBaseFcyCode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyCode.Location = new System.Drawing.Point(501, 23);
		this.txtBaseFcyCode.Name = "txtBaseFcyCode";
		this.txtBaseFcyCode.ShortcutsEnabled = false;
		this.txtBaseFcyCode.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyCode.TabIndex = 579;
		this.txtBaseFcyCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyCode.AutoSize = true;
		this.lblBaseFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyCode.Location = new System.Drawing.Point(418, 27);
		this.lblBaseFcyCode.Name = "lblBaseFcyCode";
		this.lblBaseFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblBaseFcyCode.TabIndex = 580;
		this.lblBaseFcyCode.Text = "Fcy Code";
		this.txtBaseFcyExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBaseFcyExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBaseFcyExchangeRate.Location = new System.Drawing.Point(501, 49);
		this.txtBaseFcyExchangeRate.Name = "txtBaseFcyExchangeRate";
		this.txtBaseFcyExchangeRate.ShortcutsEnabled = false;
		this.txtBaseFcyExchangeRate.Size = new System.Drawing.Size(111, 23);
		this.txtBaseFcyExchangeRate.TabIndex = 577;
		this.txtBaseFcyExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblBaseFcyExchangeRate.AutoSize = true;
		this.lblBaseFcyExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBaseFcyExchangeRate.Location = new System.Drawing.Point(418, 45);
		this.lblBaseFcyExchangeRate.Name = "lblBaseFcyExchangeRate";
		this.lblBaseFcyExchangeRate.Size = new System.Drawing.Size(82, 30);
		this.lblBaseFcyExchangeRate.TabIndex = 578;
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
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProjectId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Location = new System.Drawing.Point(73, 72);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Name = "CmbProjectId";
		((System.Windows.Forms.Control)(object)this.CmbProjectId).Size = new System.Drawing.Size(123, 24);
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbProjectId).TabStop = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 77);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 15);
		this.label10.TabIndex = 566;
		this.label10.Text = "Cost Center";
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFcyAmount.Location = new System.Drawing.Point(200, 77);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(72, 15);
		this.lblFcyAmount.TabIndex = 46;
		this.lblFcyAmount.Text = "Tcy Amount";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(284, 73);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(130, 23);
		this.txtFcyAmount.TabIndex = 5;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(284, 49);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(130, 23);
		this.txtExchangeRate.TabIndex = 4;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.lblExchangeRate.AutoSize = true;
		this.lblExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblExchangeRate.Location = new System.Drawing.Point(200, 45);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(82, 30);
		this.lblExchangeRate.TabIndex = 44;
		this.lblExchangeRate.Text = "Tcy Exchange \r\nRate";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(284, 21);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(200, 27);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 41;
		this.lblFcyCode.Text = "Tcy Code";
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(615, 23);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(56, 38);
		this.label1.TabIndex = 12;
		this.label1.Text = "Voucher Remarks";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(73, 49);
		this.DocDate.Margin = new System.Windows.Forms.Padding(2);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(123, 22);
		this.DocDate.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(73, 23);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(123, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(8, 53);
		this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(57, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Doc Date";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(8, 27);
		this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Doc No";
		this.txtVoucherremarks.BackColor = System.Drawing.Color.White;
		this.txtVoucherremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoucherremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVoucherremarks.Location = new System.Drawing.Point(672, 24);
		this.txtVoucherremarks.Multiline = true;
		this.txtVoucherremarks.Name = "txtVoucherremarks";
		this.txtVoucherremarks.Size = new System.Drawing.Size(329, 72);
		this.txtVoucherremarks.TabIndex = 6;
		this.txtVoucherremarks.Leave += new System.EventHandler(txtDrRmks_Leave);
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.chkCustomAccounts);
		this.panel3.Controls.Add(this.labelLocationType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBoxPrintPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 29);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1375, 28);
		this.panel3.TabIndex = 1;
		this.chkCustomAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkCustomAccounts.AutoSize = true;
		this.chkCustomAccounts.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCustomAccounts.ForeColor = System.Drawing.Color.White;
		this.chkCustomAccounts.Location = new System.Drawing.Point(1123, 5);
		this.chkCustomAccounts.Name = "chkCustomAccounts";
		this.chkCustomAccounts.Size = new System.Drawing.Size(122, 19);
		this.chkCustomAccounts.TabIndex = 7;
		this.chkCustomAccounts.Text = "Custom Accounts";
		this.chkCustomAccounts.UseVisualStyleBackColor = true;
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(174, 8);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 508;
		this.labelLocationType.Text = "Location Type";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(263, 2);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 507;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(140, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Journal Voucher";
		this.ChkBoxPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintPreview.AutoSize = true;
		this.ChkBoxPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintPreview.Location = new System.Drawing.Point(1247, 4);
		this.ChkBoxPrintPreview.Name = "ChkBoxPrintPreview";
		this.ChkBoxPrintPreview.Size = new System.Drawing.Size(122, 20);
		this.ChkBoxPrintPreview.TabIndex = 6;
		this.ChkBoxPrintPreview.Text = "Print Preview";
		this.ChkBoxPrintPreview.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1375, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.New, this.btnRefresh, this.btnUpdate, this.Save, this.btnSaveAs, this.btnAttachment, this.Print, this.btnShortCutKey, this.btnCreateUpdateMultiCurrency });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1375, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(59, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(New_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(64, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
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
		this.btnCreateUpdateMultiCurrency.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCreateUpdateMultiCurrency.Image = (System.Drawing.Image)resources.GetObject("btnCreateUpdateMultiCurrency.Image");
		this.btnCreateUpdateMultiCurrency.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCreateUpdateMultiCurrency.Name = "btnCreateUpdateMultiCurrency";
		this.btnCreateUpdateMultiCurrency.Size = new System.Drawing.Size(127, 24);
		this.btnCreateUpdateMultiCurrency.Text = "MultiCurrencyInfo";
		this.btnCreateUpdateMultiCurrency.Click += new System.EventHandler(btnCreateUpdateMultiCurrency_Click);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.toolStrip3);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1379, 663);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 379);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1375, 282);
		this.panel9.TabIndex = 50;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 26);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1375, 256);
		this.panel11.TabIndex = 1;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1375, 256);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 46;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label2);
		this.panel10.Controls.Add(this.ctrlGrdBar3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1375, 26);
		this.panel10.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(242, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "Detail of above selected row";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1382, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 135);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1375, 244);
		this.panel8.TabIndex = 49;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1375, 244);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 2;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick_1);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel7.Controls.Add(this.VoucherInfoBox);
		this.panel7.Controls.Add(this.groupBox3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 57);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1375, 78);
		this.panel7.TabIndex = 48;
		this.VoucherInfoBox.Controls.Add(this.label36);
		this.VoucherInfoBox.Controls.Add(this.lblUnApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.lblTotalVouchers);
		this.VoucherInfoBox.Controls.Add(this.label38);
		this.VoucherInfoBox.Controls.Add(this.lblApprovedVouchers);
		this.VoucherInfoBox.Controls.Add(this.label37);
		this.VoucherInfoBox.Location = new System.Drawing.Point(937, 5);
		this.VoucherInfoBox.Name = "VoucherInfoBox";
		this.VoucherInfoBox.Size = new System.Drawing.Size(242, 69);
		this.VoucherInfoBox.TabIndex = 9;
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
		this.groupBox3.Controls.Add(this.rdentapproved);
		this.groupBox3.Controls.Add(this.rdentmodify);
		this.groupBox3.Controls.Add(this.rdentdoc);
		this.groupBox3.Controls.Add(this.rdententry);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatus);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(926, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdentapproved.AutoSize = true;
		this.rdentapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentapproved.Location = new System.Drawing.Point(816, 48);
		this.rdentapproved.Name = "rdentapproved";
		this.rdentapproved.Size = new System.Drawing.Size(105, 19);
		this.rdentapproved.TabIndex = 8;
		this.rdentapproved.Text = "Approved Date";
		this.rdentapproved.UseVisualStyleBackColor = true;
		this.rdentmodify.AutoSize = true;
		this.rdentmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentmodify.Location = new System.Drawing.Point(816, 28);
		this.rdentmodify.Name = "rdentmodify";
		this.rdentmodify.Size = new System.Drawing.Size(91, 19);
		this.rdentmodify.TabIndex = 7;
		this.rdentmodify.Text = "Modify Date";
		this.rdentmodify.UseVisualStyleBackColor = true;
		this.rdentdoc.AutoSize = true;
		this.rdentdoc.Checked = true;
		this.rdentdoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentdoc.Location = new System.Drawing.Point(735, 28);
		this.rdentdoc.Name = "rdentdoc";
		this.rdentdoc.Size = new System.Drawing.Size(75, 19);
		this.rdentdoc.TabIndex = 5;
		this.rdentdoc.TabStop = true;
		this.rdentdoc.Text = "Doc Date";
		this.rdentdoc.UseVisualStyleBackColor = true;
		this.rdententry.AutoSize = true;
		this.rdententry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdententry.Location = new System.Drawing.Point(735, 48);
		this.rdententry.Name = "rdententry";
		this.rdententry.Size = new System.Drawing.Size(80, 19);
		this.rdententry.TabIndex = 6;
		this.rdententry.Text = "Entry Date";
		this.rdententry.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(673, 39);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(501, 22);
		this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(95, 15);
		this.label18.TabIndex = 142;
		this.label18.Text = "Approved Status";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(393, 22);
		this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(64, 15);
		this.label17.TabIndex = 141;
		this.label17.Text = "To Doc No";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(288, 22);
		this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(79, 15);
		this.label16.TabIndex = 140;
		this.label16.Text = "From Doc No";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(146, 22);
		this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(48, 15);
		this.label15.TabIndex = 139;
		this.label15.Text = "To Date";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(5, 23);
		this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(63, 15);
		this.label14.TabIndex = 13;
		this.label14.Text = "From Date";
		this.cmbApproveStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApproveStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Location = new System.Drawing.Point(499, 39);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Name = "cmbApproveStatus";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(393, 41);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(288, 41);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(146, 41);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 41);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(135, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 29);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1375, 28);
		this.panel6.TabIndex = 47;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1381, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(203, 19);
		this.label3.TabIndex = 22;
		this.label3.Text = "Journal Voucher History";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNewHistory });
		this.toolStrip3.Location = new System.Drawing.Point(2, 2);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1375, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1387, 689);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "JournalVoucher";
		this.Text = "Journal Voucher";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(JournalVoucher_FormClosing);
		base.Load += new System.EventHandler(DayBookVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DayBookVoucher_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		this.DebitAccountPanel.ResumeLayout(false);
		this.DebitAccountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).EndInit();
		this.DebitSubsidiaryPanel.ResumeLayout(false);
		this.DebitSubsidiaryPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrSubsidiaryAccount).EndInit();
		this.DrCommentsPanel.ResumeLayout(false);
		this.DrCommentsPanel.PerformLayout();
		this.DrJobLotPanel.ResumeLayout(false);
		this.DrJobLotPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrJobLot).EndInit();
		this.DrBranchPanel.ResumeLayout(false);
		this.DrBranchPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrBranch).EndInit();
		this.DrCostCenterPanel.ResumeLayout(false);
		this.DrCostCenterPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDrCostCenter).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTcyCodeDetail).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panelChqRefAndAmount.ResumeLayout(false);
		this.panelChqRefAndAmount.PerformLayout();
		this.CreditAmountPanel.ResumeLayout(false);
		this.CreditAmountPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		this.CrSubsidiaryPanel.ResumeLayout(false);
		this.CrSubsidiaryPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrSubsidiaryAccount).EndInit();
		this.CrCommentsPanel.ResumeLayout(false);
		this.CrCommentsPanel.PerformLayout();
		this.CrJobLotPanel.ResumeLayout(false);
		this.CrJobLotPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrJobLot).EndInit();
		this.CrBranchPanel.ResumeLayout(false);
		this.CrBranchPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrBranch).EndInit();
		this.CrCostCenterPanel.ResumeLayout(false);
		this.CrCostCenterPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCrCostCenter).EndInit();
		this.panelDetailButtons.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMultiCurrency).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProjectId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.VoucherInfoBox.ResumeLayout(false);
		this.VoucherInfoBox.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
